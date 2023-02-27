//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, haeder
//
// Copyright 2004-2022 by OM International
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
using System.Globalization;
using System.Xml;
using System.IO;
using System.Text;

namespace Ict.Common.IO
{
    /// <summary>
    /// Write SEPA files for Direct Debit
    /// </summary>
    public class TSEPAWriterDirectDebit
    {
        private XmlDocument FDocument;
        private DateTime FCollectionDate;
        private string FCreditorName;
        private string FCreditorIBAN;
        private string FCreditorBIC;
        private string FCreditorSchemeID;

        /// Create the header of an SEPA file
        public bool Init(
            string AInitiatorName,
            DateTime ACollectionDate,
            string ACreditorName,
            string ACreditorIBAN,
            string ACreditorBIC,
            string ACreditorSchemeID
            )
        {
            FCollectionDate = ACollectionDate;
            FCreditorName = ACreditorName;
            FCreditorIBAN = ACreditorIBAN;
            FCreditorBIC = ACreditorBIC;
            FCreditorSchemeID = ACreditorSchemeID;

            FDocument = new XmlDocument();
            FDocument.PrependChild(FDocument.CreateXmlDeclaration("1.0", "UTF-8", ""));
            XmlElement docNode = FDocument.CreateElement("Document");
            XmlAttribute attr = FDocument.CreateAttribute("xmlns");
            attr.Value = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";
            docNode.Attributes.Append(attr);

            attr = FDocument.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            attr.Value = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02 pain.008.001.02.xsd";
            docNode.Attributes.Append(attr);
            FDocument.AppendChild(docNode);

            XmlElement CstmrDrctDbtInitn = FDocument.CreateElement("CstmrDrctDbtInitn");
            docNode.AppendChild(CstmrDrctDbtInitn);

            XmlElement GrpHdr = FDocument.CreateElement("GrpHdr");
            CstmrDrctDbtInitn.AppendChild(GrpHdr);

            GrpHdr.AppendChild(FDocument.CreateElement("MsgId")).InnerText =
                "M" + DateTime.Now.ToString("yyyMMddHHmmss");

            GrpHdr.AppendChild(FDocument.CreateElement("CreDtTm")).InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            GrpHdr.AppendChild(FDocument.CreateElement("NbOfTxs")).InnerText = "0";
            GrpHdr.AppendChild(FDocument.CreateElement("InitgPty")).AppendChild
                (FDocument.CreateElement("Nm")).InnerText = AInitiatorName;

            return true;
        }

        /// <summary>
        /// add a section for payments of the same sequence type, eg. one off, recurring, first or last
        /// </summary>
        /// <param name="ASequenceType">FRST, OOFF, RCUR, or FNAL</param>
        /// <returns></returns>
        public XmlNode AddPaymentSectionToSEPADirectDebitFile(string ASequenceType)
        {
            XmlNode CstmrDrctDbtInitn = TXMLParser.GetChild(FDocument.DocumentElement, "CstmrDrctDbtInitn");

            foreach (XmlNode PmtInfSearch in CstmrDrctDbtInitn.ChildNodes)
            {
                if ((PmtInfSearch.Name == "PmtInf") && TXMLParser.GetChild(PmtInfSearch, "PmtInfId").InnerText.EndsWith(ASequenceType))
                {
                    return PmtInfSearch;
                }
            }

            XmlElement PmtInf = FDocument.CreateElement("PmtInf");
            CstmrDrctDbtInitn.AppendChild(PmtInf);
            PmtInf.AppendChild(FDocument.CreateElement("PmtInfId")).InnerText =
                "P" + DateTime.Now.ToString("yyyMMddHHmmss") + ASequenceType;
            PmtInf.AppendChild(FDocument.CreateElement("PmtMtd")).InnerText = "DD";
            PmtInf.AppendChild(FDocument.CreateElement("NbOfTxs")).InnerText = "0";
            PmtInf.AppendChild(FDocument.CreateElement("CtrlSum")).InnerText = "0.00";

            XmlElement PmtTpInf = FDocument.CreateElement("PmtTpInf");
            PmtInf.AppendChild(PmtTpInf);

            PmtTpInf.AppendChild(FDocument.CreateElement("SvcLvl")).AppendChild
                (FDocument.CreateElement("Cd")).InnerText = "SEPA";
            PmtTpInf.AppendChild(FDocument.CreateElement("LclInstrm")).AppendChild
                (FDocument.CreateElement("Cd")).InnerText = "CORE";
            PmtTpInf.AppendChild(FDocument.CreateElement("SeqTp")).InnerText = ASequenceType;
            PmtInf.AppendChild(FDocument.CreateElement("ReqdColltnDt")).InnerText = FCollectionDate.ToString("yyyy-MM-dd");
            PmtInf.AppendChild(FDocument.CreateElement("Cdtr")).AppendChild
                (FDocument.CreateElement("Nm")).InnerText = FCreditorName;
            PmtInf.AppendChild(FDocument.CreateElement("CdtrAcct")).AppendChild
                (FDocument.CreateElement("Id")).AppendChild(FDocument.CreateElement("IBAN")).InnerText =
                FCreditorIBAN.Replace(" ", "");
            PmtInf.AppendChild(FDocument.CreateElement("CdtrAgt")).AppendChild
                (FDocument.CreateElement("FinInstnId")).AppendChild
                (FDocument.CreateElement("BIC")).InnerText = FCreditorBIC;
            PmtInf.AppendChild(FDocument.CreateElement("ChrgBr")).InnerText = "SLEV";

            XmlElement CdtrSchmeId = FDocument.CreateElement("CdtrSchmeId");
            PmtInf.AppendChild(CdtrSchmeId);
            XmlElement Id = FDocument.CreateElement("Id");
            CdtrSchmeId.AppendChild(Id);
            XmlElement PrvtId = FDocument.CreateElement("PrvtId");
            Id.AppendChild(PrvtId);
            XmlElement Othr = FDocument.CreateElement("Othr");
            PrvtId.AppendChild(Othr);
            Id = FDocument.CreateElement("Id");

            Othr.AppendChild(Id).InnerText = FCreditorSchemeID;
            Othr.AppendChild(FDocument.CreateElement("SchmeNm")).AppendChild
                (FDocument.CreateElement("Prtry")).InnerText = "SEPA";

            return PmtInf;
        }

        /// <summary>
        /// add a payment to an SEPA document
        /// </summary>
        public bool AddPaymentToSEPADirectDebitFile(
            string ASequenceType,
            string ADebtorName,
            string ADebtorIBAN,
            string ADebtorBIC,
            string AMandateID,
            DateTime AMandateSignatureDate,
            Decimal AAmount,
            string ADescription,
            string AEndToEndId
            )
        {
            XmlNode CstmrDrctDbtInitn = TXMLParser.GetChild(FDocument.DocumentElement, "CstmrDrctDbtInitn");
            XmlNode GrpHdr = TXMLParser.GetChild(CstmrDrctDbtInitn, "GrpHdr");
            XmlNode NbOfTxs = TXMLParser.GetChild(GrpHdr, "NbOfTxs");

            NbOfTxs.InnerText = (Convert.ToInt32(NbOfTxs.InnerText) + 1).ToString();

            XmlNode PmtInf = AddPaymentSectionToSEPADirectDebitFile(ASequenceType);

            NbOfTxs = TXMLParser.FindNodeRecursive(PmtInf, "NbOfTxs");
            NbOfTxs.InnerText = (Convert.ToInt32(NbOfTxs.InnerText) + 1).ToString();

            XmlNode CtrlSum = TXMLParser.FindNodeRecursive(PmtInf, "CtrlSum");
            CtrlSum.InnerText = (Convert.ToDecimal(CtrlSum.InnerText.
                                     Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)) + AAmount).
                                ToString("0.00").Replace(",", ".");

            XmlElement DrctDbtTxInf = FDocument.CreateElement("DrctDbtTxInf");
            PmtInf.AppendChild(DrctDbtTxInf);

            DrctDbtTxInf.AppendChild(FDocument.CreateElement("PmtId")).AppendChild
                (FDocument.CreateElement("EndToEndId")).InnerText = AEndToEndId;

            XmlElement InstdAmt = FDocument.CreateElement("InstdAmt");
            DrctDbtTxInf.AppendChild(InstdAmt);
            XmlAttribute attr = FDocument.CreateAttribute("Ccy");
            attr.Value = "EUR";
            InstdAmt.Attributes.Append(attr);
            InstdAmt.InnerText = AAmount.ToString("0.00").Replace(",", ".");

            XmlElement DrctDbtTx = FDocument.CreateElement("DrctDbtTx");
            DrctDbtTxInf.AppendChild(DrctDbtTx);
            XmlElement MndtRltdInf = FDocument.CreateElement("MndtRltdInf");
            DrctDbtTx.AppendChild(MndtRltdInf);
            MndtRltdInf.AppendChild(FDocument.CreateElement("MndtId")).InnerText = AMandateID;
            MndtRltdInf.AppendChild(FDocument.CreateElement("DtOfSgntr")).InnerText = AMandateSignatureDate.ToString("yyyy-MM-dd");

            DrctDbtTxInf.AppendChild(FDocument.CreateElement("DbtrAgt")).AppendChild(FDocument.CreateElement("FinInstnId")).AppendChild
                (FDocument.CreateElement("BIC")).InnerText = ADebtorBIC;
            DrctDbtTxInf.AppendChild(FDocument.CreateElement("Dbtr")).AppendChild(FDocument.CreateElement("Nm")).InnerText = ADebtorName;

            DrctDbtTxInf.AppendChild(FDocument.CreateElement("DbtrAcct")).AppendChild(FDocument.CreateElement("Id")).AppendChild
                (FDocument.CreateElement("IBAN")).InnerText = ADebtorIBAN.Replace(" ", "");
            // Do we need this: DrctDbtTxInf.AppendChild(FDocument.CreateElement("UltmtDbtr")).AppendChild(FDocument.CreateElement("Nm")).InnerText = ADebtorName;
            DrctDbtTxInf.AppendChild(FDocument.CreateElement("RmtInf")).AppendChild(FDocument.CreateElement("Ustrd")).InnerText = ADescription;

            return true;
        }

        /// <summary>
        /// read the document
        /// </summary>
        public XmlDocument Document
        {
            get
            {
                return FDocument;
            }
        }

        /// format an IBAN with spaces
        public static string FormatIBAN(string AIban, bool ADisplayReadable)
        {
            AIban = AIban.Replace(" ", "").ToUpper();

            if (!ADisplayReadable)
            {
                return AIban;
            }

            int count = 0;
            string orig = AIban;
            AIban = String.Empty;
            while (count + 4 < orig.Length)
            {
                AIban += orig.Substring(count, 4) + " ";
                count += 4;
            }
            return (AIban + orig.Substring(count)).Trim();
        }
    }
}