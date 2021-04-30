//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2021 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Threading;
using System.Text;
using Ict.Common;
using Ict.Common.Verification;

namespace Ict.Petra.Server.MFinance.BankImport.Logic
{
    /// <summary>
    /// parses bank statement files (ISO 20022 CAMT.053) in Germany;
    /// for the structure of the file see
    /// https://www.rabobank.com/nl/images/Format%20description%20CAMT.053.pdf
    /// http://www.national-bank.de/fileadmin/user_upload/nationalbank/Service_Center/Electronic_Banking_Center/Downloads/Handbuecher_und_Bedingungen/SRZ-Anlage_5b_Kontoauszug_ISO_20022_camt_2010-06-15b.pdf
    /// http://www.hettwer-beratung.de/sepa-spezialwissen/sepa-technische-anforderungen/camt-format-camt-053/
    /// </summary>
    public class TCAMTParser
    {
        /// <summary>
        /// the parsed bank statements
        /// </summary>
        public List <TStatement>statements;

        private static string WithoutLeadingZeros(string ACode)
        {
            // cut off leading zeros
            try
            {
                return Convert.ToInt64(ACode).ToString();
            }
            catch (Exception)
            {
                // IBAN or BIC
                return ACode;
            }
        }

        private XmlNamespaceManager GetNamespaceManager(XmlDocument doc, string ANamespaceName)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("camt", ANamespaceName);

            XmlNode nodeDocument = doc.DocumentElement;

            if ((nodeDocument == null) || (nodeDocument.Attributes["xmlns"].Value != ANamespaceName))
            {
                return null;
            }

            return nsmgr;
        }
        

        /// <summary>
        /// processing CAMT file
        /// </summary>
        public void ProcessFileContent(string content, bool AParsePreviousYear, out TVerificationResultCollection AVerificationResult)
        {
            statements = new List <TStatement>();
            AVerificationResult = new TVerificationResultCollection();

            CultureInfo backupCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(content);
                XmlNode nodeDocument = doc.DocumentElement;

                string CAMTVersion = "CAMT.53";
                XmlNamespaceManager nsmgr = GetNamespaceManager(doc, "urn:iso:std:iso:20022:tech:xsd:camt.053.001.02");

                if (nsmgr == null)
                {
                    CAMTVersion = "CAMT.52";
                    nsmgr = GetNamespaceManager(doc, "urn:iso:std:iso:20022:tech:xsd:camt.052.001.02");
                }

                if (nsmgr == null)
                {
                    throw new Exception("expecting xmlns for CAMT.52 or CAMT.53");
                }

                XmlNodeList stmts = null;
                
                if (CAMTVersion == "CAMT.53")
                {
                    stmts = nodeDocument.SelectNodes("camt:BkToCstmrStmt/camt:Stmt", nsmgr);
                }
                else if (CAMTVersion == "CAMT.52")
                {
                    stmts = nodeDocument.SelectNodes("camt:BkToCstmrAcctRpt/camt:Rpt", nsmgr);
                }

                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

                foreach (XmlNode nodeStatement in stmts)
                {
                    TStatement stmt = new TStatement();

                    stmt.id = nodeStatement.SelectSingleNode("camt:ElctrncSeqNb", nsmgr).InnerText;
                    stmt.accountCode = nodeStatement.SelectSingleNode("camt:Acct/camt:Id/camt:IBAN", nsmgr).InnerText;
                    stmt.bankCode = nodeStatement.SelectSingleNode("camt:Acct/camt:Svcr/camt:FinInstnId/camt:BIC", nsmgr).InnerText;
                    stmt.currency = nodeStatement.SelectSingleNode("camt:Acct/camt:Ccy", nsmgr).InnerText;

                    Int32 DiffElctrncSeqNb = TAppSettingsManager.GetInt32("DiffElctrncSeqNbFor" + stmt.bankCode + "/" + stmt.accountCode, 0);
                    stmt.id = (Convert.ToInt32(stmt.id) + DiffElctrncSeqNb).ToString();

                    stmt.severalYears = false;
                    XmlNode nm = nodeStatement.SelectSingleNode("camt:Acct/camt:Ownr/camt:Nm", nsmgr);
                    string ownName = nm!=null?nm.InnerText:
                        TAppSettingsManager.GetValue("AccountNameFor" + stmt.bankCode + "/" + stmt.accountCode, String.Empty, false);
                    XmlNodeList nodeBalances = nodeStatement.SelectNodes("camt:Bal", nsmgr);

                    foreach (XmlNode nodeBalance in nodeBalances)
                    {
                        // PRCD: PreviouslyClosedBooked
                        if (nodeBalance.SelectSingleNode("camt:Tp/camt:CdOrPrtry/camt:Cd", nsmgr).InnerText == "PRCD")
                        {
                            stmt.startBalance = Decimal.Parse(nodeBalance.SelectSingleNode("camt:Amt", nsmgr).InnerText);

                            // CreditDebitIndicator: CRDT or DBIT for credit or debit
                            if (nodeBalance.SelectSingleNode("camt:CdtDbtInd", nsmgr).InnerText == "DBIT")
                            {
                                stmt.startBalance *= -1.0m;
                            }

                            stmt.date = DateTime.Parse(nodeBalance.SelectSingleNode("camt:Dt", nsmgr).InnerText);
                        }
                        // CLBD: ClosingBooked
                        else if (nodeBalance.SelectSingleNode("camt:Tp/camt:CdOrPrtry/camt:Cd", nsmgr).InnerText == "CLBD")
                        {
                            stmt.endBalance = Decimal.Parse(nodeBalance.SelectSingleNode("camt:Amt", nsmgr).InnerText);

                            // CreditDebitIndicator: CRDT or DBIT for credit or debit
                            if (nodeBalance.SelectSingleNode("camt:CdtDbtInd", nsmgr).InnerText == "DBIT")
                            {
                                stmt.endBalance *= -1.0m;
                            }

                            stmt.date = DateTime.Parse(nodeBalance.SelectSingleNode("camt:Dt", nsmgr).InnerText);
                        }

                        // ITBD: InterimBooked
                        // CLAV: ClosingAvailable
                        // FWAV: ForwardAvailable
                    }

                    string strDiffBalance = TAppSettingsManager.GetValue("DiffBalanceFor" + stmt.bankCode + "/" + stmt.accountCode, "0");
                    Decimal DiffBalance = 0.0m;
                    if (Decimal.TryParse(strDiffBalance, out DiffBalance))
                    {
                        stmt.startBalance += DiffBalance;
                        stmt.endBalance += DiffBalance;
                    }
                    else
                    {
                        TLogging.Log("problem parsing decimal from configuration setting DiffBalanceFor" + stmt.bankCode + "/" + stmt.accountCode);
                    }

                    // if we should parse the transactions only of the past year
                    if (AParsePreviousYear
                        && stmt.date.Month != 12
                        && stmt.date.Day != 31)
                    {
                        stmt.date = new DateTime(stmt.date.Year - 1, 12, 31);
                    }

                    XmlNodeList nodeEntries = nodeStatement.SelectNodes("camt:Ntry", nsmgr);

                    foreach (XmlNode nodeEntry in nodeEntries)
                    {
                        TTransaction tr = new TTransaction();
                        tr.inputDate = DateTime.Parse(nodeEntry.SelectSingleNode("camt:BookgDt/camt:Dt", nsmgr).InnerText);
                        tr.valueDate = DateTime.Parse(nodeEntry.SelectSingleNode("camt:ValDt/camt:Dt", nsmgr).InnerText);

                        if (tr.valueDate.Year != stmt.date.Year)
                        {
                            // ignore transactions that are in a different year than the statement
                            stmt.severalYears = true;
                            continue;
                        }

                        tr.amount = Decimal.Parse(nodeEntry.SelectSingleNode("camt:Amt", nsmgr).InnerText);

                        if (nodeEntry.SelectSingleNode("camt:Amt", nsmgr).Attributes["Ccy"].Value != stmt.currency)
                        {
                            throw new Exception("transaction currency " + nodeEntry.SelectSingleNode("camt:Amt",
                                    nsmgr).Attributes["Ccy"].Value + " does not match the bank statement currency");
                        }

                        if (nodeEntry.SelectSingleNode("camt:CdtDbtInd", nsmgr).InnerText == "DBIT")
                        {
                            tr.amount *= -1.0m;
                        }

                        XmlNode desc = nodeEntry.SelectSingleNode("camt:NtryDtls/camt:TxDtls/camt:RmtInf/camt:Ustrd", nsmgr);
                        tr.description = desc!=null?desc.InnerText:String.Empty;
                        XmlNode partnerName = nodeEntry.SelectSingleNode("camt:NtryDtls/camt:TxDtls/camt:RltdPties/camt:Dbtr/camt:Nm", nsmgr);

                        if (partnerName != null)
                        {
                            tr.partnerName = partnerName.InnerText;
                        }

                        XmlNode accountCode = nodeEntry.SelectSingleNode("camt:NtryDtls/camt:TxDtls/camt:RltdPties/camt:DbtrAcct/camt:Id/camt:IBAN",
                            nsmgr);

                        if (accountCode != null)
                        {
                            tr.accountCode = accountCode.InnerText;
                        }

                        XmlNode CrdtName = nodeEntry.SelectSingleNode("camt:NtryDtls/camt:TxDtls/camt:RltdPties/camt:Cdtr/camt:Nm", nsmgr);
                        XmlNode DbtrName = nodeEntry.SelectSingleNode("camt:NtryDtls/camt:TxDtls/camt:RltdPties/camt:Dbtr/camt:Nm", nsmgr);

                        if ((CrdtName != null) && (CrdtName.InnerText != ownName))
                        {
                            if ((DbtrName != null) && (DbtrName.InnerText == ownName))
                            {
                                // we are the debitor
                            }
                            else if (ownName != String.Empty)
                            {
                                // sometimes donors write the project or recipient in the field where the organisation is supposed to be
                                TLogging.Log("CrdtName is not like expected: " + tr.description + " --- " + CrdtName.InnerText);
                                tr.description += " " + CrdtName.InnerText;
                            }
                        }

                        XmlNode EndToEndId = nodeEntry.SelectSingleNode("camt:NtryDtls/camt:TxDtls/camt:Refs/camt:EndToEndId", nsmgr);

                        if ((EndToEndId != null)
                            && (EndToEndId.InnerText != "NOTPROVIDED")
                            && !EndToEndId.InnerText.StartsWith("LS-")
                            && !EndToEndId.InnerText.StartsWith("ZV")
                            && !EndToEndId.InnerText.StartsWith("IZV-DISPO"))
                        {
                            // sometimes donors write the project or recipient in unexpected field
                            TLogging.Log("EndToEndId: " + tr.description + " --- " + EndToEndId.InnerText);
                            tr.description += " " + EndToEndId.InnerText;
                        }

                        // eg NSTO+152+00900. look for SEPA Geschäftsvorfallcodes
                        // see the codes: https://www.wgzbank.de/export/sites/wgzbank/de/wgzbank/downloads/produkte_leistungen/firmenkunden/zv_aktuelles/Uebersicht-GVC-und-Buchungstexte-WGZ-BANK_V062015.pdf
                        string[] GVCCode =
                            nodeEntry.SelectSingleNode("camt:NtryDtls/camt:TxDtls/camt:BkTxCd/camt:Prtry/camt:Cd", nsmgr).InnerText.Split(
                                new char[] { '+' });
                        tr.typecode = GVCCode[1];

                        // for SEPA direct debit batches, there are multiple TxDtls records
                        XmlNodeList transactionDetails = nodeEntry.SelectNodes("camt:NtryDtls/camt:TxDtls", nsmgr);

                        if (transactionDetails.Count > 1)
                        {
                            tr.partnerName = String.Empty;
                            tr.description = String.Format(
                                Catalog.GetString("SEPA Sammel-Basislastschrift mit {0} Lastschriften"),
                                transactionDetails.Count);
                        }

                        stmt.transactions.Add(tr);

                        TLogging.LogAtLevel(2, "count : " + stmt.transactions.Count.ToString());
                    }

                    statements.Add(stmt);
                }
            }
            catch (Exception e)
            {
                AVerificationResult.Add(new TVerificationResult(null, "problem with importing camt file; " + e.Message, TResultSeverity.Resv_Critical));
                TLogging.Log("problem with importing camt file; " + e.Message + Environment.NewLine + e.StackTrace);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = backupCulture;
            }
        }
    }

    /// todoComment
    public class TTransaction
    {
        /// todoComment
        public DateTime valueDate;

        /// todoComment
        public DateTime inputDate;

        /// todoComment
        public decimal amount;

        /// todoComment
        public string text;

        /// todoComment
        public string typecode;

        /// todoComment
        public string description;

        /// todoComment
        public string bankCode;

        /// todoComment
        public string accountCode;

        /// todoComment
        public string partnerName;
    }

    /// todoComment
    public class TStatement
    {
        /// todoComment
        public string id;

        /// todoComment
        public string bankCode;

        /// todoComment
        public string accountCode;

        /// todoComment
        public string currency;

        /// todoComment
        public decimal startBalance;

        /// todoComment
        public decimal endBalance;

        /// todoComment
        public DateTime date;

        /// across several years
        public bool severalYears;

        /// todoComment
        public List <TTransaction>transactions = new List <TTransaction>();
    }
}
