//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, christophert, timop
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Xml;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.MSysMan.Maintenance;
using Ict.Petra.Server.MSysMan.Security;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;

namespace Ict.Petra.Server.MFinance.ICH
{
    /// <summary>
    /// Class for the performance of the Stewardship Calculation
    /// </summary>
    public class TGenFilesReports
    {
        /// <summary>
        /// Performs the ICH code to generate Stewardship Calculation.
        ///  Relates to gi3100.p
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="AICHNumber"></param>
        /// <param name="ACurrencyType"></param>
        /// <param name="AFileName"></param>
        /// <param name="AEmail"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public void GenerateStewardshipFile(int ALedgerNumber,
            int APeriodNumber,
            int AICHNumber,
            int ACurrencyType,
            string AFileName,
            bool AEmail,
            out TVerificationResultCollection AVerificationResult
            )
        {
            string CostCentre;
            decimal IncomeAmount = 0;
            decimal ExpenseAmount = 0;
            decimal XferAmount = 0;
            string Currency;

            string LedgerName;

            AVerificationResult = new TVerificationResultCollection();

            //Begin the transaction
            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                //Find the LedgerRow
                ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, DBTransaction);
                ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

                //Find the Partner Short Name
                PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(LedgerRow.PartnerKey, DBTransaction);
                PPartnerRow PartnerRow = (PPartnerRow)PartnerTable.Rows[0];

                LedgerName = PartnerRow.PartnerShortName;

                //Specify currency type
                if (ACurrencyType == MFinanceConstants.CURRENCY_BASE_NUM)
                {
                    Currency = LedgerRow.BaseCurrency;
                }
                else
                {
                    Currency = LedgerRow.IntlCurrency;
                }

                //Create table for conversion to XML and export to CSV
                AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, APeriodNumber, null);
                AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                DateTime PeriodEndDate = AccountingPeriodRow.PeriodEndDate;
                string StandardCostCentre = ALedgerNumber.ToString() + "00";
                DateTime DateToday = DateTime.Today;

                //First four fields are constant for each row
                DataTable TableForExport = new DataTable();

                TableForExport.Columns.Add("PeriodEndDate", typeof(DateTime));
                TableForExport.Columns.Add("StandardCostCentre", typeof(string));
                TableForExport.Columns.Add("DateToday", typeof(DateTime));
                TableForExport.Columns.Add("Currency", typeof(string));
                TableForExport.Columns.Add("CostCentre", typeof(string));
                TableForExport.Columns.Add("Income", typeof(decimal));
                TableForExport.Columns.Add("Expense", typeof(decimal));
                TableForExport.Columns.Add("DirectTransfer", typeof(decimal));

                CostCentre = string.Empty;

                AIchStewardshipTable IchStewTable = new AIchStewardshipTable();
                AIchStewardshipRow TemplateRow = (AIchStewardshipRow)IchStewTable.NewRowTyped(false);

                TemplateRow.LedgerNumber = ALedgerNumber;
                TemplateRow.PeriodNumber = APeriodNumber;

                StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=" });

                AIchStewardshipTable IchStewardshipTable = AIchStewardshipAccess.LoadUsingTemplate(TemplateRow, operators, null, DBTransaction);
                AIchStewardshipRow IchStewardshipRow = null;

                for (int i = 0; i < IchStewardshipTable.Count; i++)
                {
                    IchStewardshipRow = (AIchStewardshipRow)IchStewardshipTable.Rows[i];

                    if ((AICHNumber == 0)
                        || (IchStewardshipRow.IchNumber == AICHNumber))
                    {
                        if ((CostCentre != string.Empty)
                            && (CostCentre != IchStewardshipRow.CostCentreCode))
                        {
                            if ((IncomeAmount != 0) || (ExpenseAmount != 0) || (XferAmount != 0))
                            {
                                DataRow DR = (DataRow)TableForExport.NewRow();

                                DR.ItemArray[0] = PeriodEndDate;
                                DR.ItemArray[1] = StandardCostCentre;
                                DR.ItemArray[2] = DateToday;
                                DR.ItemArray[3] = Currency;
                                DR.ItemArray[4] = CostCentre;
                                DR.ItemArray[5] = IncomeAmount;
                                DR.ItemArray[6] = ExpenseAmount;
                                DR.ItemArray[7] = XferAmount;

                                TableForExport.Rows.Add(DR);
                            }

                            IncomeAmount = 0;
                            ExpenseAmount = 0;
                            XferAmount = 0;
                        }

                        if (ACurrencyType == MFinanceConstants.CURRENCY_BASE_NUM)
                        {
                            IncomeAmount += IchStewardshipRow.IncomeAmount;
                            ExpenseAmount += IchStewardshipRow.ExpenseAmount;
                            XferAmount += IchStewardshipRow.DirectXferAmount;
                        }
                        else
                        {
                            IncomeAmount += IchStewardshipRow.IncomeAmountIntl;
                            ExpenseAmount += IchStewardshipRow.ExpenseAmountIntl;
                            XferAmount += IchStewardshipRow.DirectXferAmountIntl;
                        }

                        CostCentre = IchStewardshipRow.CostCentreCode;
                    }
                }

                if ((CostCentre != string.Empty) && ((IncomeAmount != 0) || (ExpenseAmount != 0) || (XferAmount != 0)))
                {
                    DataRow DR = (DataRow)TableForExport.NewRow();

                    DR.ItemArray[0] = PeriodEndDate;
                    DR.ItemArray[1] = StandardCostCentre;
                    DR.ItemArray[2] = DateToday;
                    DR.ItemArray[3] = Currency;
                    DR.ItemArray[4] = CostCentre;
                    DR.ItemArray[5] = IncomeAmount;
                    DR.ItemArray[6] = ExpenseAmount;
                    DR.ItemArray[7] = XferAmount;

                    TableForExport.Rows.Add(DR);
                }

                //Create the XMLDoc ready for export to CSV
                XmlDocument doc = TDataBase.DataTableToXml(TableForExport);

                TCsv2Xml.Xml2Csv(doc, AFileName);

                if (AEmail)
                {
                    AEmailDestinationTable AEmailDestTable = new AEmailDestinationTable();
                    AEmailDestinationRow TemplateRow2 = (AEmailDestinationRow)AEmailDestTable.NewRowTyped(false);

                    TemplateRow2.FileCode = MFinanceConstants.EMAIL_FILE_CODE_STEWARDSHIP;

                    StringCollection operators2 = StringHelper.InitStrArr(new string[] { "=" });

                    AEmailDestinationTable AEmailDestinationTable = AEmailDestinationAccess.LoadUsingTemplate(TemplateRow2,
                        operators2,
                        null,
                        DBTransaction);
                    AEmailDestinationRow EmailDestinationRow = (AEmailDestinationRow)AEmailDestinationTable.Rows[0];

                    string SenderAddress = "";
                    string EmailAddress = EmailDestinationRow.EmailAddress;
                    string EmailSubject = "Stewardship File from " + LedgerName;
                    string HTMLText = string.Empty;

                    //Read the file title
                    FileInfo FileDetails = new FileInfo(AFileName);
                    string FileTitle = FileDetails.Name;

                    if (!File.Exists(AFileName))
                    {
                        HTMLText = "<html><body>" + String.Format("Cannot find file {0}", AFileName) + "</body></html>";
                    }
                    else
                    {
                        HTMLText = "<html><body>" + EmailSubject + ": " + FileTitle + " is attached.</body></html>";
                    }

                    TSmtpSender SendMail = new TSmtpSender();

                    MailMessage msg = new MailMessage(SenderAddress,
                        EmailAddress,
                        EmailSubject,
                        HTMLText);

                    msg.Attachments.Add(new Attachment(AFileName));
                    //msg.Bcc.Add(BCCAddress);

                    SendMail.SendMessage(ref msg);
                }

                DBAccess.GDBAccessObj.RollbackTransaction();
            }
            catch (Exception Exp)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();

                TLogging.Log(Exp.Message);
                TLogging.Log(Exp.StackTrace);

                throw Exp;
            }
        }

        /// <summary>
        /// Searches all transaction tables to see if there is a stewardship
        ///   batch that already exists which appears to match the one
        ///   currently being processed.
        /// </summary>
        /// <param name="AICHLedgerNumber">ICH Ledger number</param>
        /// <param name="ABatchRef">Reference of current stewardship batch</param>
        /// <param name="AYear">Year to which current stewardship batch applies</param>
        /// <param name="AMonth">Month to which current stewardship batch applies</param>
        /// <param name="ACurrentPeriod">Current period of ICH ledger</param>
        /// <param name="ACurrentYear">Current year of ICH ledger</param>
        /// <param name="ADBTransaction">Current database transaction</param>
        /// <returns>The batch number of the matching batch or 0 if there is no match.</returns>
        public int FindMatchingStewardshipBatchInICH(int AICHLedgerNumber,
            string ABatchRef,
            int AYear,
            int AMonth,
            int ACurrentPeriod,
            int ACurrentYear,
            ref TDBTransaction ADBTransaction
           )
        {
            int BatchNumber = 0;

            bool MatchFound = false;

            /*cOldStyleDescription = REPLACE(REPLACE(pcBatchDescription, STRING(piYear) + " ", ""), " (Run " + STRING(piRunNumber) + ")", "").*/

            /* Old reference format (prior to this program being available) was xxyy where xx was the fund number
             * and yy was the period number. Therefore we need to strip off the final 3 characters to convert
             * from the new style reference to the old style reference (those 3 characters are the run number).
             * We need to do this because some of the batches we will search through will be in old format. */
            string cOldStyleReference = ABatchRef.Substring(0, ABatchRef.Length - 3);
            int cOldStyleReferenceLen = cOldStyleReference.Length;

            /* Note: In the queries below we need to check the length of the reference as well because of the
             * 3 digit fund numbers (eg. Central America Period 12 would have reference 2012 while NAA Period 2 would be
             * 20112) */

            /* look in current period */
            ATransactionTable TransTable = new ATransactionTable();
            ATransactionRow TemplateRow = (ATransactionRow)TransTable.NewRowTyped(false);

            TemplateRow.LedgerNumber = AICHLedgerNumber;
            TemplateRow.TransactionStatus = true;

            StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=" });

            ATransactionTable TransactionTable = ATransactionAccess.LoadUsingTemplate(TemplateRow, operators, null, ADBTransaction);
            ATransactionRow TransactionRow = null;

            string TransRef;
            int TransRefLen;

            for (int i = 0; i < TransactionTable.Count; i++)
            {
                TransactionRow = (ATransactionRow)TransactionTable.Rows[i];

                TransRef = TransactionRow.Reference;
                TransRefLen = TransRef.Length;

                if ((TransRef.Substring(0, cOldStyleReferenceLen) == cOldStyleReference)
                    && ((TransRefLen == cOldStyleReferenceLen) || (TransRefLen == (cOldStyleReferenceLen + 3))))
                {
                    BatchNumber = TransactionRow.BatchNumber;
                    MatchFound = true;
                    break;
                }
            }

            if (!MatchFound)
            {
                /* look in previous periods (need to compare date of transactions to the month of the stewardship
                 *             because there may be a batch present which was for last years stewardship - eg. Dec 2007
                 *             Stewardship for US may have been processed in Jan 2008) */
                AThisYearOldTransactionTable YearOldTransTable = new AThisYearOldTransactionTable();
                AThisYearOldTransactionRow TemplateRow2 = (AThisYearOldTransactionRow)YearOldTransTable.NewRowTyped(false);

                TemplateRow2.LedgerNumber = AICHLedgerNumber;
                TemplateRow2.TransactionStatus = true;

                StringCollection operators2 = StringHelper.InitStrArr(new string[] { "=", "=" });

                AThisYearOldTransactionTable ThisYearOldTransactionTable = AThisYearOldTransactionAccess.LoadUsingTemplate(TemplateRow2,
                    operators2,
                    null,
                    ADBTransaction);
                AThisYearOldTransactionRow ThisYearOldTransactionRow = null;

                string OldTransRef;
                int OldTransRefLen;

                for (int i = 0; i < ThisYearOldTransactionTable.Count; i++)
                {
                    ThisYearOldTransactionRow = (AThisYearOldTransactionRow)ThisYearOldTransactionTable.Rows[i];

                    OldTransRef = ThisYearOldTransactionRow.Reference;
                    OldTransRefLen = OldTransRef.Length;

                    if ((OldTransRef.Substring(0, cOldStyleReferenceLen) == cOldStyleReference)
                        && ((OldTransRefLen == cOldStyleReferenceLen) || (OldTransRefLen == (cOldStyleReferenceLen + 3)))
                        && ((AMonth <= ThisYearOldTransactionRow.TransactionDate.Month) || (AMonth > ACurrentPeriod)))
                    {
                        BatchNumber = ThisYearOldTransactionRow.BatchNumber;
                        MatchFound = true;
                        break;
                    }
                }

                /* look in previous years (need to make sure that you only match batches that are stewardships
                 * for the same year as the one being processed). */
                if (!MatchFound && (AYear < ACurrentYear))
                {
                    APreviousYearTransactionTable YearPreviousTransTable = new APreviousYearTransactionTable();
                    APreviousYearTransactionRow TemplateRow3 = (APreviousYearTransactionRow)YearPreviousTransTable.NewRowTyped(false);

                    TemplateRow3.LedgerNumber = AICHLedgerNumber;
                    TemplateRow3.TransactionStatus = true;

                    StringCollection operators3 = StringHelper.InitStrArr(new string[] { "=", "=" });

                    APreviousYearTransactionTable PreviousYearTransactionTable = APreviousYearTransactionAccess.LoadUsingTemplate(TemplateRow3,
                        operators3,
                        null,
                        ADBTransaction);
                    APreviousYearTransactionRow PreviousYearTransactionRow = null;

                    string PreviousTransRef;
                    int PreviousTransRefLen;

                    for (int i = 0; i < PreviousYearTransactionTable.Count; i++)
                    {
                        PreviousYearTransactionRow = (APreviousYearTransactionRow)PreviousYearTransactionTable.Rows[i];

                        PreviousTransRef = PreviousYearTransactionRow.Reference;
                        PreviousTransRefLen = PreviousTransRef.Length;

                        if ((PreviousTransRef.Substring(0, cOldStyleReferenceLen) == cOldStyleReference)
                            && ((PreviousTransRefLen == cOldStyleReferenceLen) || (PreviousTransRefLen == (cOldStyleReferenceLen + 3)))
                            && (PreviousYearTransactionRow.TransactionDate.Month >= AMonth)
                            && (AMonth > ACurrentPeriod)
                            && (PreviousYearTransactionRow.TransactionDate.Year == AYear))
                        {
                            BatchNumber = PreviousYearTransactionRow.BatchNumber;
                            break;
                        }
                    }
                }
            }

            return BatchNumber;
        }

        /// <summary>
        /// Checks the specified stewardship batch to see whether the summary transactions
        ///    match the amounts specified.
        /// </summary>
        /// <param name="AICHLedgerNumber">ICH Ledger number</param>
        /// <param name="ABatchNumber">The batch number of the batch to check</param>
        /// <param name="ACostCentre">Fund to which stewardship batch applies</param>
        /// <param name="AIncomeAmount">Income total to check against</param>
        /// <param name="AExpenseAmount">Expense total to check against</param>
        /// <param name="ATransferAmount">Transfer total to check against</param>
        /// <param name="ADBTransaction">Current database transaction</param>
        /// <returns>True if transasctions match, false otherwise</returns>
        public bool SummaryStewardshipTransactionsMatch(int AICHLedgerNumber,
            int ABatchNumber,
            string ACostCentre,
            decimal AIncomeAmount,
            decimal AExpenseAmount,
            decimal ATransferAmount,
            ref TDBTransaction ADBTransaction)
        {
            decimal dExistingIncExpTotal;

            ABatchTable BatchTable = ABatchAccess.LoadByPrimaryKey(AICHLedgerNumber, ABatchNumber, ADBTransaction);
            ABatchRow BatchRow = (ABatchRow)BatchTable.Rows[0];

            if (BatchRow != null)
            {
                /* look for summary transaction for transfers */
                ATransactionTable TransTable = new ATransactionTable();
                ATransactionRow TemplateRow = (ATransactionRow)TransTable.NewRowTyped(false);

                TemplateRow.LedgerNumber = AICHLedgerNumber;
                TemplateRow.BatchNumber = ABatchNumber;
                TemplateRow.CostCentreCode = ACostCentre;
                TemplateRow.AccountCode = MFinanceConstants.ICH_ACCT_DEPOSITS_WITH_ICH;                 //i.e. 8540

                StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });

                ATransactionTable TransactionTable = ATransactionAccess.LoadUsingTemplate(TemplateRow, operators, null, ADBTransaction);
                ATransactionRow TransactionRow = (ATransactionRow)TransactionTable.Rows[0];

                if (TransactionRow != null)
                {
                    if (TransactionRow.TransactionAmount != ATransferAmount)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                        return false;
                    }
                }

                /* find the summary transactions for income and expense and sum them */
                dExistingIncExpTotal = 0;

                ATransactionTable TransTable2 = new ATransactionTable();
                ATransactionRow TemplateRow2 = (ATransactionRow)TransTable2.NewRowTyped(false);

                TemplateRow2.LedgerNumber = AICHLedgerNumber;
                TemplateRow2.BatchNumber = ABatchNumber;
                TemplateRow2.CostCentreCode = ACostCentre;
                TemplateRow2.AccountCode = MFinanceConstants.ICH_ACCT_LOCAL_LEDGER;                 //i.e. 8520

                StringCollection operators2 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });

                ATransactionTable TransactionTable2 = ATransactionAccess.LoadUsingTemplate(TemplateRow2, operators2, null, ADBTransaction);
                ATransactionRow TransactionRow2 = null;

                for (int i = 0; i < TransactionTable2.Count; i++)
                {
                    TransactionRow2 = (ATransactionRow)TransactionTable2.Rows[i];

                    dExistingIncExpTotal += TransactionRow2.TransactionAmount;
                }

                DBAccess.GDBAccessObj.RollbackTransaction();
                return dExistingIncExpTotal == (AIncomeAmount + AExpenseAmount);
            }

            /* now check previous periods if batch wasn't in current period */
            AThisYearOldBatchTable ThisYearOldBatchTable = AThisYearOldBatchAccess.LoadByPrimaryKey(AICHLedgerNumber, ABatchNumber, ADBTransaction);
            AThisYearOldBatchRow ThisYearOldBatchRow = (AThisYearOldBatchRow)ThisYearOldBatchTable.Rows[0];

            if (ThisYearOldBatchRow != null)
            {
                /* look for summary transaction for transfers */
                AThisYearOldTransactionTable ThisYearOldTransTable = new AThisYearOldTransactionTable();
                AThisYearOldTransactionRow TemplateRow3 = (AThisYearOldTransactionRow)ThisYearOldTransTable.NewRowTyped(false);

                TemplateRow3.LedgerNumber = AICHLedgerNumber;
                TemplateRow3.BatchNumber = ABatchNumber;
                TemplateRow3.CostCentreCode = ACostCentre;
                TemplateRow3.AccountCode = MFinanceConstants.ICH_ACCT_DEPOSITS_WITH_ICH;                 //i.e. 8540

                StringCollection operators3 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });

                AThisYearOldTransactionTable ThisYearOldTransactionTable = AThisYearOldTransactionAccess.LoadUsingTemplate(TemplateRow3,
                    operators3,
                    null,
                    ADBTransaction);
                AThisYearOldTransactionRow ThisYearOldTransactionRow = (AThisYearOldTransactionRow)ThisYearOldTransactionTable.Rows[0];

                if (ThisYearOldTransactionRow != null)
                {
                    if (ThisYearOldTransactionRow.TransactionAmount != ATransferAmount)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                        return false;
                    }
                }

                /* find the summary transactions for income and expense and sum them */
                dExistingIncExpTotal = 0;

                AThisYearOldTransactionTable ThisYearOldTransTable4 = new AThisYearOldTransactionTable();
                AThisYearOldTransactionRow TemplateRow4 = (AThisYearOldTransactionRow)ThisYearOldTransTable4.NewRowTyped(false);

                TemplateRow4.LedgerNumber = AICHLedgerNumber;
                TemplateRow4.BatchNumber = ABatchNumber;
                TemplateRow4.CostCentreCode = ACostCentre;
                TemplateRow4.AccountCode = MFinanceConstants.ICH_ACCT_LOCAL_LEDGER;                 //i.e. 8520

                StringCollection operators4 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });

                AThisYearOldTransactionTable ThisYearOldTransactionTable2 = AThisYearOldTransactionAccess.LoadUsingTemplate(TemplateRow4,
                    operators4,
                    null,
                    ADBTransaction);
                AThisYearOldTransactionRow ThisYearOldTransactionRow2 = null;

                for (int i = 0; i < ThisYearOldTransactionTable2.Count; i++)
                {
                    ThisYearOldTransactionRow2 = (AThisYearOldTransactionRow)ThisYearOldTransactionTable2.Rows[i];

                    dExistingIncExpTotal += ThisYearOldTransactionRow2.TransactionAmount;
                }

                DBAccess.GDBAccessObj.RollbackTransaction();
                return dExistingIncExpTotal == (AIncomeAmount + AExpenseAmount);
            }

            /* now check previous years if batch wasn't in current year */
            APreviousYearBatchTable PreviousYearBatchTable = APreviousYearBatchAccess.LoadByPrimaryKey(AICHLedgerNumber, ABatchNumber, ADBTransaction);
            APreviousYearBatchRow PreviousYearBatchRow = (APreviousYearBatchRow)PreviousYearBatchTable.Rows[0];

            if (PreviousYearBatchRow != null)
            {
                /* look for summary transaction for transfers */
                APreviousYearTransactionTable PreviousYearTransTable = new APreviousYearTransactionTable();
                APreviousYearTransactionRow TemplateRow5 = (APreviousYearTransactionRow)PreviousYearTransTable.NewRowTyped(false);

                TemplateRow5.LedgerNumber = AICHLedgerNumber;
                TemplateRow5.BatchNumber = ABatchNumber;
                TemplateRow5.CostCentreCode = ACostCentre;
                TemplateRow5.AccountCode = MFinanceConstants.ICH_ACCT_DEPOSITS_WITH_ICH; //i.e. 8540

                StringCollection operators5 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });

                APreviousYearTransactionTable PreviousYearTransactionTable = APreviousYearTransactionAccess.LoadUsingTemplate(TemplateRow5,
                    operators5,
                    null,
                    ADBTransaction);
                APreviousYearTransactionRow PreviousYearTransactionRow = (APreviousYearTransactionRow)PreviousYearTransactionTable.Rows[0];

                if (PreviousYearTransactionRow != null)
                {
                    if (PreviousYearTransactionRow.TransactionAmount != ATransferAmount)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                        return false;
                    }
                }

                /* find the summary transactions for income and expense and sum them */
                dExistingIncExpTotal = 0;

                APreviousYearTransactionTable PreviousYearTransTable4 = new APreviousYearTransactionTable();
                APreviousYearTransactionRow TemplateRow6 = (APreviousYearTransactionRow)PreviousYearTransTable4.NewRowTyped(false);

                TemplateRow6.LedgerNumber = AICHLedgerNumber;
                TemplateRow6.BatchNumber = ABatchNumber;
                TemplateRow6.CostCentreCode = ACostCentre;
                TemplateRow6.AccountCode = MFinanceConstants.ICH_ACCT_LOCAL_LEDGER; //i.e. 8520

                StringCollection operators6 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });

                APreviousYearTransactionTable PreviousYearTransactionTable2 = APreviousYearTransactionAccess.LoadUsingTemplate(TemplateRow6,
                    operators6,
                    null,
                    ADBTransaction);
                APreviousYearTransactionRow PreviousYearTransactionRow2 = null;

                for (int i = 0; i < PreviousYearTransactionTable2.Count; i++)
                {
                    PreviousYearTransactionRow2 = (APreviousYearTransactionRow)PreviousYearTransactionTable2.Rows[i];

                    dExistingIncExpTotal += PreviousYearTransactionRow2.TransactionAmount;
                }

                DBAccess.GDBAccessObj.RollbackTransaction();
                return dExistingIncExpTotal == (AIncomeAmount + AExpenseAmount);
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
            return false;
        }

        /// <summary>
        /// Imports all available stewardships (reading from a specific directory)
        ///   into the current period.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AICHFolder"></param>
        public void ImportAllAvailableStewardshipReports(int ALedgerNumber, string AICHFolder)
        {
            string cLogFile;
            string cPendingDir;
            string cNewDir;
            string cCurrentFile;
            string cInputLine;
            string cPeriod;
            string cUnsuccessfulFileList;
            DateTime cTime;
            int iHours;
            int iCount;
            int iMins;
            bool lFormatOK = true;
            DateTime dtPeriodDate;
            bool lDateLineValid;

            /* temp table to store basic details about each stewardship file */
            DataTable ttFileList = new DataTable();

            ttFileList.Columns.Add("cFileName", typeof(string));
            ttFileList.Columns.Add("dtReportDate", typeof(DateTime));
            ttFileList.Columns.Add("iReportTimeInMins", typeof(int));
            ttFileList.Columns.Add("iLedger", typeof(int));
            ttFileList.Columns.Add("iPeriod", typeof(int));
            ttFileList.Columns.Add("iYear", typeof(int));
            ttFileList.Columns.Add("iRunNumber", typeof(int));

            cPendingDir = AICHFolder + @"\pending";

            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);


            /* Check that previous period has been closed so that these stewardships go into the right period.
             *                     If it hasn't been closed then don't go any further */
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, DBTransaction);
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            int LedgerCurrentPeriod = LedgerRow.CurrentPeriod;

            AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, LedgerCurrentPeriod, DBTransaction);
            AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

            if (AccountingPeriodRow != null)
            {
                if (DateTime.Today > AccountingPeriodRow.PeriodStartDate.AddDays(60))
                {
                    //TODO: MESSAGE "It looks like you need to close the current period before processing the current batch of Stewardships"
                    return;
                }
            }

            cLogFile = Path.GetTempPath() + @"\Stewardship Import.log";

            TextWriter tw = new StreamWriter(cLogFile);

            // write a line of text to the file
            tw.WriteLine("Import Started: " + DateTime.Today.ToShortDateString() + " " + DateTime.Today.ToShortTimeString());

            /* create new directory to store processed stewardships (named by current date) */
            cNewDir = AICHFolder + @"\" + DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00");
            Directory.CreateDirectory(cNewDir);

            cUnsuccessfulFileList = string.Empty;

            DateTime fileDate;
            DataRow dr;

            /* Process every .txt file in pending stewardship directory */
            string[] fileEntries = Directory.GetFiles(cPendingDir, "*.txt");

            foreach (string fileName in fileEntries)
            {
                cCurrentFile = fileName;
                lFormatOK = true;
                iCount = 1;

                dr = (DataRow)ttFileList.NewRow();

                int YearNo;
                int PeriodNo;
                int RunNo = 0;

                /* look at first 6 lines in file (ie. header lines) and pick up key information
                 * (report date and time, fund number, period, year, run number) */
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while (iCount < 7 && lFormatOK)
                    {
                        cInputLine = sr.ReadLine().Trim();

                        if (sr.Peek() >= 0)
                        {
                            if (!((cInputLine.Length == 0) && (iCount == 1)))
                            {
                                switch (iCount)
                                {
                                    case 1:
                                        lFormatOK = (DateTime.TryParse(cInputLine.Substring(69, 11), out fileDate));
                                        dr["dtReportDate"] = fileDate;
                                        break;

                                    case 2:
                                        cTime = Convert.ToDateTime(cInputLine.Substring(69).Trim());
                                        iHours = cTime.Hour;
                                        iMins = cTime.Minute;
                                        dr["iReportTimeInMins"] = (iHours * 60) + iMins;
                                        break;

                                    case 5:

                                        if (cInputLine.Length == 0)
                                        {
                                            /* on reports for closed years there are some extra blank lines before the Ledger line */
                                            cInputLine = sr.ReadLine().Trim();
                                        }

                                        dr["iLedger"] = cInputLine.Substring(7, 3);
                                        lDateLineValid = (DateTime.TryParse(cInputLine.Substring(64, 12), out dtPeriodDate));

                                        /* if the report is from an old year then the date may be offset by 3 characters */

                                        if (!lDateLineValid)
                                        {
                                            lDateLineValid = (DateTime.TryParse(cInputLine.Substring(67, 12), out dtPeriodDate));
                                        }

                                        if (!lDateLineValid)
                                        {
                                            lFormatOK = false;
                                        }
                                        else
                                        {
                                            /* if start date is greater than 20th of the month then consider it the next month (to cope with
                                             * strange period dates in Korea) */
                                            if (dtPeriodDate.Day > 20)
                                            {
                                                dtPeriodDate.AddDays(30);
                                            }

                                            /* handle 13th period stewardship separately (needs to be treated as
                                             *                 another period 12 one with run number 999) */
                                            if ((dtPeriodDate.Day == 31) && (dtPeriodDate.Month == 12))
                                            {
                                                dr["iPeriod"] = 12;
                                                dr["iYear"] = dtPeriodDate.Year;
                                                RunNo = 999;
                                                dr["iRunNumber"] = RunNo;
                                            }
                                            else
                                            {
                                                AAccountingPeriodTable AccPeriodTable = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber,
                                                    DBTransaction);

                                                string SqlExpression = "MONTH(" + AAccountingPeriodTable.GetPeriodStartDateDBName() + ") = " +
                                                                       dtPeriodDate.Month.ToString();
                                                DataRow[] AccPeriodRows = AccPeriodTable.Select(SqlExpression);
                                                DataRow AccPeriodRow = AccPeriodRows[0];

                                                PeriodNo = Convert.ToInt32(AccPeriodRow[AAccountingPeriodTable.GetAccountingPeriodNumberDBName()]);
                                                dr["iPeriod"] = PeriodNo;
                                                YearNo = dtPeriodDate.Year;
                                                dr["iYear"] = YearNo;
                                                cPeriod = PeriodNo.ToString("00") + (YearNo - 2000).ToString("00");
                                            }
                                        }

                                        break;

                                    case 6:

                                        if (RunNo != 999)
                                        {
                                            dr["iRunNumber"] = Convert.ToInt32(cInputLine.Substring(76, 6).Trim());
                                        }

                                        break;

                                    default:
                                        break;
                                }

                                iCount += 1;
                            }
                        }
                    }
                }

                if (lFormatOK)
                {
                    dr["cFileName"] = cCurrentFile;
                    //Add the new row
                    ttFileList.Rows.Add(dr);
                }
                else
                {
                    tw.WriteLine("File: " + cCurrentFile + " is not in the correct format and will therefore be skipped.");
                    cUnsuccessfulFileList += cCurrentFile + ",";
                    dr.Delete();
                }
            }

            GenerateStewardshipBatchFromFileList(ALedgerNumber, ref ttFileList, ref tw, ref cUnsuccessfulFileList, cNewDir, ref DBTransaction);

            // close the stream
            tw.Close();

            ListUnprocessedFiles(cUnsuccessfulFileList);
        }

        
        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AFileList"></param>
        /// <param name="Atw"></param>
        /// <param name="AUnsuccessfulFileList"></param>
        /// <param name="ANewDir"></param>
        /// <param name="ADBTransaction"></param>
        private void GenerateStewardshipBatchFromFileList(int ALedgerNumber,
            ref DataTable AFileList,
            ref TextWriter Atw,
            ref string AUnsuccessfulFileList,
            string ANewDir,
            ref TDBTransaction ADBTransaction)
        {
            int iPreviousLedger = 0;
            int iPreviousRunNumber = 0;
            int iPreviousPeriod = 0;
            string cPreviousFileName = string.Empty;

            string cNewFileName = string.Empty;

            string FileName;
            string LogMessage = string.Empty;

            int LedgerNo;
            int YearNo;
            int PeriodNo;
            int RunNo;

            bool lSuccessful = false;

            DataView dv = AFileList.DefaultView;

            dv.Sort = "iLedger asc, iPeriod asc, iRunNumber asc, dtReportDate desc, iReportTimeInMins desc";
            dv.RowFilter = "cFileName <> ''";

            foreach (DataRow dr in dv)
            {
                /* if this is not the same as the previous stewardship then go ahead and try to create the
                 * stewardship batch from it */
                FileName = Convert.ToString(dr["cFileName"]);
                LedgerNo = Convert.ToInt32(dr["iLedger"]);
                YearNo = Convert.ToInt32(dr["iYear"]);
                PeriodNo = Convert.ToInt32(dr["iPeriod"]);
                RunNo = Convert.ToInt32(dr["iRunNumber"]);

                if ((LedgerNo != iPreviousLedger) || (PeriodNo != iPreviousPeriod) || (RunNo != iPreviousRunNumber))
                {
                    GenerateStewardshipBatch(ALedgerNumber, YearNo, PeriodNo, RunNo, LedgerNo.ToString("00") + "00", FileName, ref Atw, out lSuccessful);
                }
                else
                {
                    Atw.WriteLine("File " + FileName + " is a duplicate of " + cPreviousFileName + " and will therefore be skipped.");
                }

                if (lSuccessful)
                {
                    AAccountingPeriodTable APT = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, PeriodNo, ADBTransaction);
                    AAccountingPeriodRow APR = (AAccountingPeriodRow)APT.Rows[0];

                    //TODO: Need to add function for LangSpecMonthName
                    cNewFileName = LedgerNo.ToString("000") + RunNo.ToString("000") + ".txt";

                    FileInfo fi = new FileInfo(FileName);

                    if (fi.Exists)
                    {
                        fi.MoveTo(ANewDir + @"\" + cNewFileName);
                    }
                }
                else
                {
                    AUnsuccessfulFileList += FileName + ",";
                }

                iPreviousLedger = LedgerNo;
                iPreviousRunNumber = RunNo;
                iPreviousPeriod = PeriodNo;
                cPreviousFileName = FileName;
            }

            Atw.WriteLine("Import Completed: " + DateTime.Today.ToShortTimeString() + " " + DateTime.Today.ToShortTimeString());
        }


        /// <summary>
        /// Creates stewardship batch from the specified stewardship report file.
        /// </summary>
        /// <param name="ALedgerNumber">ICH Ledger number</param>
        /// <param name="AYear">Year to which stewardship applies</param>
        /// <param name="APeriod">Period to which stewardship applies</param>
        /// <param name="ARunNumber">Run number of stewardship</param>
        /// <param name="AFromCostCentre">Fund to which stewardship relates</param>
        /// <param name="AFileName">Filename of stewardship report to process</param>
        /// <param name="Atw">TextWriter for log file</param>
        /// <param name="ASuccessful">Returns whether process successful</param>
        private void GenerateStewardshipBatch(int ALedgerNumber,
            int AYear,
            int APeriod,
            int ARunNumber,
            string AFromCostCentre,
            string AFileName,
            ref TextWriter Atw,
            out bool ASuccessful)
        {
            ASuccessful = false;

            string InputLine;
            string LogMessage = string.Empty;
            string PeriodName;
            string FromCostCentreName;
            string BatchDescription;
            string Currency = string.Empty;
            string Reference;
            string ToCostCentre = string.Empty;
            string Narrative;
            int Count;
            int BatchNumber = 0;
            int JournalNumber = 0;
            int MatchingBatchNumber;
            decimal ExchangeRate;
            decimal Income = 0;
            decimal Expense = 0;
            decimal Transfer = 0;
            decimal TotalIncome = 0;
            decimal TotalExpense = 0;
            decimal TotalTransfer = 0;
            bool BatchCreateOk = false;
            bool JournalCreateOk = false;
            bool TransCreateOk = false;
            bool Complete = false;
            bool ContinueImporting;
            bool PostingSuccessful = false;
            bool ProcessFile = true;
            bool EmptyStewardship = false;
            DateTime CurrentPeriodDate;
            DateTime ExchangeRateDate;

            //DEFINE BUFFER a_current_period_b FOR a_accounting_period.

            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, DBTransaction);
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            /* find accounting period to which this stewardship applies */
            AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, APeriod, DBTransaction);
            AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

            PeriodName = AccountingPeriodRow.AccountingPeriodDesc;

            /* find current period of ICH ledger */
            AAccountingPeriodTable AccPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, LedgerRow.CurrentPeriod, DBTransaction);
            AAccountingPeriodRow AccPeriodRow = (AAccountingPeriodRow)AccPeriodTable.Rows[0];

            /* if this stewardship applies to a period more than 12 months ago we can't process it as it gets
             * too difficult to figure out whether it is a duplicate or not */
            if ((AYear < AccPeriodRow.PeriodStartDate.Year)
                && (APeriod < LedgerRow.CurrentPeriod))
            {
                LogMessage = "File " + AFileName + " is for a period more than 12 months prior to the current period and will need to be processed manually.";
                Atw.WriteLine(LogMessage);
                return;
            }

            ACostCentreTable CostCentreTable = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, AFromCostCentre, DBTransaction);
            ACostCentreRow CostCentreRow = (ACostCentreRow)CostCentreTable.Rows[0];

            if (CostCentreRow == null)
            {
                LogMessage = "Cost Centre " + AFromCostCentre + " does not exist. File " + AFileName + " will be skipped";
                Atw.WriteLine(LogMessage);
                return;
            }

            /* set batch description and transaction reference based on fund, period, year and run number */
            FromCostCentreName = CostCentreRow.CostCentreName;
            BatchDescription = PeriodName + " " + AYear.ToString() + " Stewardship For " + FromCostCentreName;
            Reference = AFromCostCentre.Substring(0, AFromCostCentre.Length - 2) + APeriod.ToString("00") + ARunNumber.ToString("000");

            /* if run number is not 0 then add it to the description (if it is 999 this means it is a period 13
             *      stewardship and this needs to be specified) */
            if (ARunNumber == 999)
            {
                BatchDescription += " (Period 13)";
            }
            else if (ARunNumber > 0)
            {
                BatchDescription += " (Run " + ARunNumber.ToString() + ")";
            }

            /* look for any previously entered batches that appear to match this stewardship (ie. same fund,
             * period, run number) */
            MatchingBatchNumber = FindMatchingStewardshipBatchInICH(ALedgerNumber,
                Reference,
                AYear,
                APeriod,
                LedgerRow.CurrentPeriod,
                AccPeriodRow.PeriodStartDate.Year,
                ref DBTransaction);

            if (!CostCentreRow.CostCentreActiveFlag)
            {
                LogMessage = "Cost Centre " + AFromCostCentre + " is not active. File " + AFileName + " will be skipped";
                Atw.WriteLine(LogMessage);
                return;
            }

            /* get currency information from the header */
            Count = 1;
            using (StreamReader sr = new StreamReader(AFileName))
            {
                while (Count <= 6)
                {
                    InputLine = sr.ReadLine().Trim();

                    if (sr.Peek() >= 0)
                    {
                        /* ignore any blank lines at beginning */
                        if (!((Count == 1) && (InputLine == string.Empty)))
                        {
                            if (Count == 6)
                            {
                                Currency = InputLine.Substring(10, 3);
                            }
                        }
                    }

                    Count += 1;
                }
            }

            ACurrencyTable CurrencyTable = ACurrencyAccess.LoadByPrimaryKey(Currency, DBTransaction);
            ACurrencyRow CurrencyRow = (ACurrencyRow)CurrencyTable.Rows[0];

            if (CurrencyRow == null)
            {
                LogMessage = "Currency " + Currency + " does not exist. File " + AFileName + " will be skipped";
                Atw.WriteLine(LogMessage);
                return;
            }

            /* work out which date to get the exchange rate for (should be for the period of the stewardship) */
            ExchangeRateDate = AccountingPeriodRow.PeriodStartDate;

            while (ExchangeRateDate.Year > AYear)
            {
                ExchangeRateDate = ExchangeRateDate.AddYears(-1);
            }

            /* get the rate */
            if (Currency == LedgerRow.BaseCurrency)
            {
                ExchangeRate = 1;
            }
            else
            {
                ACorporateExchangeRateTable CorporateExchangeRateTable = ACorporateExchangeRateAccess.LoadByPrimaryKey(Currency,
                    LedgerRow.BaseCurrency,
                    ExchangeRateDate,
                    DBTransaction);
                ACorporateExchangeRateRow CorporateExchangeRateRow = (ACorporateExchangeRateRow)CorporateExchangeRateTable.Rows[0];

                if (CorporateExchangeRateRow != null)
                {
                    ExchangeRate = CorporateExchangeRateRow.RateOfExchange;
                }
                else
                {
	                LogMessage = "File " + AFileName + " could not be imported as there is no Corporate Exchange Rate ";
	                LogMessage += "for currency " + Currency + " period " + APeriod.ToString() + " year " + AYear;
	                Atw.WriteLine(LogMessage);
                    return;
                }
            }

            CurrentPeriodDate = AccPeriodRow.PeriodEndDate;

            /* create the batch, journal and transactions */
			
            //Start of GenerateBatch process
			
            //Use Commit and Rollback here

            /*RUN gl1110o.p ("new":U,
             * ALedgerNumber,
             * ?,
             * cBatchDescription,
             * 0,
             * dtCurrentPeriodDate,
             * OUTPUT iBatchNumber,
             * OUTPUT lBatchCreateOk). */
            if (!BatchCreateOk)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                goto PostGenerateBatch;
            }

            /* create journal record
             * RUN gl1120o.p ("new":U,
             * ALedgerNumber,
             * iBatchNumber,
             * ?,
             * cBatchDescription,
             * "{&GENERAL-LEDGER}":U,
             * "{&STANDARD-JOURNAL}":U,
             * cCurrency,
             * dExchangeRate,
             * dtCurrentPeriodDate,
             * OUTPUT iJournalNumber,
             * OUTPUT lJournalCreateOk).*/

            if (!JournalCreateOk)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                goto PostGenerateBatch;
            }

            ContinueImporting = true;

            using (StreamReader sr = new StreamReader(AFileName))
            {
                while (ContinueImporting)
                {
                    InputLine = sr.ReadLine().Trim();

                    if (sr.Peek() >= 0)
                    {
                        if (Count > 10)
                        {
                            if (InputLine.IndexOf("---") != -1)
                            {
                                ContinueImporting = false;                 /* if this happens it is an empty stewardship */
                            }
                            else
                            {
                                /* read cost centre and income, expense, transfer amounts for this transaction
                                 * and accumulate totals for use in creating the summary transactions later */
                                ToCostCentre = InputLine.Substring(0, 5).Trim();
                                Income = Convert.ToDecimal(ChangeToAmericanFormat(InputLine.Substring(44, 19).Trim()));
                                Expense = Convert.ToDecimal(ChangeToAmericanFormat(InputLine.Substring(64, 19).Trim()));
                                Transfer = Convert.ToDecimal(ChangeToAmericanFormat(InputLine.Substring(84, 19).Trim()));
                                TotalIncome += Income;
                                TotalExpense += Expense;
                                TotalTransfer += Transfer;

                                /* create one transaction for income, one for expense, one for transfers */
                                Narrative = PeriodName + " Income " + FromCostCentreName;
                                CreateStewardshipTransaction(ALedgerNumber,
                                    BatchNumber,
                                    JournalNumber,
                                    MFinanceConstants.TRANSACTION_TYPE_INCOME,
                                    ToCostCentre,
                                    Narrative,
                                    Income,
                                    Reference,
                                    CurrentPeriodDate,
                                    false,
                                    ref Atw,
                                    ref DBTransaction,
                                    out TransCreateOk);

                                if (!TransCreateOk)
                                {
                                    DBAccess.GDBAccessObj.RollbackTransaction();
                                    goto PostGenerateBatch;
                                }

                                Narrative = "AE " + PeriodName + " " + FromCostCentreName;
                                CreateStewardshipTransaction(ALedgerNumber,
                                    BatchNumber,
                                    JournalNumber,
                                    MFinanceConstants.TRANSACTION_TYPE_EXPENSE,
                                    ToCostCentre,
                                    Narrative,
                                    Expense,
                                    Reference,
                                    CurrentPeriodDate,
                                    false,
                                    ref Atw,
                                    ref DBTransaction,
                                    out TransCreateOk);

                                if (!TransCreateOk)
                                {
                                    DBAccess.GDBAccessObj.RollbackTransaction();
                                    goto PostGenerateBatch;
                                }

                                Narrative = PeriodName + " Direct xfer " + FromCostCentreName;
                                CreateStewardshipTransaction(ALedgerNumber,
                                    BatchNumber,
                                    JournalNumber,
                                    MFinanceConstants.TRANSACTION_TYPE_TRANSFER,
                                    ToCostCentre,
                                    Narrative,
                                    Transfer,
                                    Reference,
                                    CurrentPeriodDate,
                                    false,
                                    ref Atw,
                                    ref DBTransaction,
                                    out TransCreateOk);

                                if (!TransCreateOk)
                                {
                                    DBAccess.GDBAccessObj.RollbackTransaction();
                                    goto PostGenerateBatch;
                                }
                            }
                        }
                    }

                    Count += 1;
                }
            }

            /* if we found an apparently matching batch for this stewardship earlier then we now need to
             *     check whether the totals we have just calculated from this file match that previously entered
             *     batch (this will affect what we write to the logfile about it and may affect whether
             *     or not we go ahead and process it) */

            if (MatchingBatchNumber != 0)
            {
                DealWithMatchingStewardshipBatch(ALedgerNumber,
                    MatchingBatchNumber,
                    ARunNumber,
                    AYear,
                    AFileName,
                    AFromCostCentre,
                    FromCostCentreName,
                    PeriodName,
                    TotalIncome,
                    TotalExpense,
                    TotalTransfer,
                    ref Atw,
                    ref DBTransaction,
                    out ProcessFile);

                if (!ProcessFile)
                {
                    Complete = true;
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    goto PostGenerateBatch;
                }
            }

            /* create the summary transactions for income, expense and transfers */
            Narrative = PeriodName + " Income For Others " + FromCostCentreName;
            CreateStewardshipTransaction(ALedgerNumber,
                BatchNumber,
                JournalNumber,
                MFinanceConstants.TRANSACTION_TYPE_INCOME,
                AFromCostCentre,
                Narrative,
                TotalIncome,
                Reference,
                CurrentPeriodDate,
                true,
                ref Atw,
                ref DBTransaction,
                out TransCreateOk);

            if (!TransCreateOk)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                goto PostGenerateBatch;
            }

            Narrative = "AE For Others " + PeriodName + " " + FromCostCentreName;
            CreateStewardshipTransaction(ALedgerNumber,
                BatchNumber,
                JournalNumber,
                MFinanceConstants.TRANSACTION_TYPE_EXPENSE,
                AFromCostCentre,
                Narrative,
                TotalExpense,
                Reference,
                CurrentPeriodDate,
                true,
                ref Atw,
                ref DBTransaction,
                out TransCreateOk);

            if (!TransCreateOk)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                goto PostGenerateBatch;
            }

            Narrative = PeriodName + " Direct xfer For Others " + FromCostCentreName;
            CreateStewardshipTransaction(ALedgerNumber,
                BatchNumber,
                JournalNumber,
                MFinanceConstants.TRANSACTION_TYPE_TRANSFER,
                AFromCostCentre,
                Narrative,
                TotalTransfer,
                Reference,
                CurrentPeriodDate,
                true,
                ref Atw,
                ref DBTransaction,
                out TransCreateOk);

            if (!TransCreateOk)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                goto PostGenerateBatch;
            }

            /* if it was an empty stewardship then undo batch creation */
            if ((TotalIncome == 0) && (TotalExpense == 0) && (TotalTransfer == 0) && (Income == 0) && (Expense == 0) && (Transfer == 0))
            {
                Complete = true;
                EmptyStewardship = true;
                DBAccess.GDBAccessObj.RollbackTransaction();
                goto PostGenerateBatch;
            }

            Complete = true;

            //End of Generate Batch
PostGenerateBatch:

            /* if something went wrong write something appropriate to the logfile */
            if (!Complete)
            {
                if (!BatchCreateOk)
                {
	                LogMessage = "File " + AFileName + " could not be imported as batch creation failed.";
	                Atw.WriteLine(LogMessage);
                    return;
                }
                else if (!JournalCreateOk)
                {
	                LogMessage = "File " + AFileName + " could not be imported as journal creation failed.";
	                Atw.WriteLine(LogMessage);
                    return;
                }
                else if (!TransCreateOk)
                {
	                LogMessage = "File " + AFileName + " could not be imported as transaction creation failed.";
	                Atw.WriteLine(LogMessage);
                    return;
                }
                else
                {
	                LogMessage = "File " + AFileName + " could not be . Cause unknown. Cost Centre " + ToCostCentre;
	                Atw.WriteLine(LogMessage);
                    //"File " pcFileName " could not be imported. Cause unknown. Cost Centre "  + ToCostCentre
                }
            }
            else if (ProcessFile && !EmptyStewardship)
            {
                /* if the file was processed successfully and it was not an empty stewardship we can now post the
                 * batch that we have just created */

                /*RUN gl1210.p (piLedgerNumber,
                 * iBatchNumber,
                 * FALSE,
                 * OUTPUT lPostingSuccessful).*/
                ASuccessful = PostingSuccessful;
            }
            else if (ProcessFile && EmptyStewardship)
            {
                /* if the stewardship was empty we still want to make it as successful even though we didn't create
                 *              a batch */
                ASuccessful = true;
            }
            else
            {
                ASuccessful = PostingSuccessful;
            }

            //TODO:
            //Close inputfile
        }

        /// <summary>
        /// Looks at the specified batch and works out whether it is a duplicate
        ///       of the stewardship currently being processed (defined by the other
        ///       parameters). There are a number of possibilities from definite duplicate,
        ///       through possible duplicate to definitely not a duplicate, depending on
        ///       comparison of run numbers and comparison of amount totals. The
        ///       procedure works out which it is and writes to the log file appropriately.
        /// </summary>
        /// <param name="ALedgerNumber">ICH Ledger number</param>
        /// <param name="ABatchNumber">The number of the batch to check</param>
        /// <param name="ARunNumber">The run number of the stewardship being processed</param>
        /// <param name="AYear">The year of the stewardship being processed</param>
        /// <param name="AFileName">The name of the stewardship file being processed</param>
        /// <param name="AFromCostCentre">Fund number to which the stewardship applies</param>
        /// <param name="AFromCostCentreName">Name of the fund</param>
        /// <param name="APeriodName">Name of the period to which the stewardship applies (ie. Month)</param>
        /// <param name="ATotalIncome">Total amount of income transactions in stewardship being processed</param>
        /// <param name="ATotalExpense">Total amount of expense transactions in stewardship being processed</param>
        /// <param name="ATotalTransfer">Total amount of transfer transactions in stewardship being processed</param>
        /// <param name="Atw">TextWriter for log file</param>
        /// <param name="ADBTransaction">Current database transaction</param>
        /// <param name="AProcessFile">Set to True if processing of the stewardship should go ahead or False if it should be rejected (ie. if a duplicate or possible duplicate</param>
        private void DealWithMatchingStewardshipBatch(int ALedgerNumber, int ABatchNumber, int ARunNumber,
            int AYear, string AFileName, string AFromCostCentre,
            string AFromCostCentreName, string APeriodName, decimal ATotalIncome,
            decimal ATotalExpense, decimal ATotalTransfer, ref TextWriter Atw, ref TDBTransaction ADBTransaction, out bool AProcessFile)
        {
            string LogMessage = string.Empty;

            AProcessFile = false;

            int BatchRunNumber = 0;

            /*
             *          Possible scenarios:
             *
             *          matching batch exists, both are run 0                           - full duplicate
             *          matching batch exists, original is run 0 new one is run xxx     - possible duplicate if totals identical (return flag to indicate that totals
             *                                                                                                                    need to be compared)
             *          matching batch exists, original is run xxx new one is run 0     - not valid
             *          matching batch exists, original is run xxx new one is run xxx   - full duplicate
             *          matching batch exists, original is run xxx new one is run yyy   - not duplicate
             *          matching batch exists, original is run ? new one is run 0       - full duplicate
             *          matching batch exists, original is run ? new one is run xxx     - possible duplicate if totals identical (return flag to indicate that totals
             */

            /* check if batch is in current period and do the appropriate comparisons if it is (based around
             * the scenarios above) */
            ABatchTable BatchTable = ABatchAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, ADBTransaction);
            ABatchRow BatchRow = (ABatchRow)BatchTable.Rows[0];

            if (BatchRow != null)
            {
                if (ARunNumber == 0)
                {
                    if (SummaryStewardshipTransactionsMatch(ALedgerNumber, ABatchNumber, AFromCostCentre, ATotalIncome, ATotalExpense, ATotalTransfer,
                            ref ADBTransaction))
                    {
                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
		                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ") and data is identical. ";
                        LogMessage += AFileName + " will be skipped";
		                Atw.WriteLine(LogMessage);
                        AProcessFile = false;
                    }
                    else
                    {
                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
		                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ") and data has changed. ";
                        LogMessage += AFileName + " will be skipped";
		                Atw.WriteLine(LogMessage);
                        AProcessFile = false;
                    }
                }
                else
                {
                    ATransactionTable TransTable = new ATransactionTable();
                    ATransactionRow TemplateRow = (ATransactionRow)TransTable.NewRowTyped(false);

                    TemplateRow.LedgerNumber = ALedgerNumber;
                    TemplateRow.BatchNumber = ABatchNumber;

                    StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=" });

                    ATransactionTable TransactionTable = ATransactionAccess.LoadUsingTemplate(TemplateRow, operators, null, ADBTransaction);
                    ATransactionRow TransactionRow = (ATransactionRow)TransactionTable.Rows[0];

                    if (TransactionTable.Count > 0)
                    {
                        string TransRowRef = TransactionRow.Reference;
                        BatchRunNumber = Convert.ToInt32(TransRowRef.Substring(TransRowRef.Length - 3, 3));

                        if (ARunNumber == BatchRunNumber)
                        {
                            if (SummaryStewardshipTransactionsMatch(ALedgerNumber, ABatchNumber, AFromCostCentre, ATotalIncome, ATotalExpense,
                                    ATotalTransfer, ref ADBTransaction))
                            {
		                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
				                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ") and data is identical. ";
		                        LogMessage += AFileName + " will be skipped";
				                Atw.WriteLine(LogMessage);
                                AProcessFile = false;
                            }
                            else
                            {
		                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
				                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ") and data has changed. ";
		                        LogMessage += AFileName + " will be skipped";
				                Atw.WriteLine(LogMessage);
                                AProcessFile = false;
                            }

                            return;
                        }
                        else if (BatchRunNumber == 0)
                        {
                            if (SummaryStewardshipTransactionsMatch(ALedgerNumber, ABatchNumber, AFromCostCentre, ATotalIncome, ATotalExpense,
                                    ATotalTransfer, ref ADBTransaction))
                            {
		                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
				                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
				                LogMessage += TransactionRow.TransactionDate.ToShortDateString() + ") and is run number 0. ";
				                LogMessage += "File " + AFileName + " is for run number " + ARunNumber.ToString() + " but the data is identical to the ";
				                LogMessage += "existing batch so it will not be processed.";
				                Atw.WriteLine(LogMessage);
                                AProcessFile = false;
                            }
                            else
                            {
                                AProcessFile = true;
                            }
                        }
                        else
                        {
                            AProcessFile = true;
                        }
                    }
                }

                return;
            }

            /* if the batch wasn't in the current period try previous periods. In this case we can't assume
             * that the transaction reference will be in the "new style" (ie. LLLPPRRR). It might be in the
             * old style (ie. LLLPP). We are able to do more checks where it is in the new style as we can
             * get hold of the run number */

            AThisYearOldTransactionTable OldTransTable = new AThisYearOldTransactionTable();
            AThisYearOldTransactionRow TemplateRow2 = (AThisYearOldTransactionRow)OldTransTable.NewRowTyped(false);

            TemplateRow2.LedgerNumber = ALedgerNumber;
            TemplateRow2.BatchNumber = ABatchNumber;

            StringCollection operators2 = StringHelper.InitStrArr(new string[] { "=", "=" });

            AThisYearOldTransactionTable OldTransactionTable = AThisYearOldTransactionAccess.LoadUsingTemplate(TemplateRow2,
                operators2,
                null,
                ADBTransaction);
            AThisYearOldTransactionRow OldTransactionRow = (AThisYearOldTransactionRow)OldTransactionTable.Rows[0];

            if (OldTransactionTable.Count > 0)
            {
                string OldTransRowRef = OldTransactionRow.Reference;
                //BatchRunNumber = Convert.ToInt32(TransRowRef.Substring(TransRowRef.Length - 3, 3));

                if (OldTransRowRef.Length < 7)
                {
                    if (ARunNumber == 0)
                    {
                        if (SummaryStewardshipTransactionsMatch(ALedgerNumber, ABatchNumber, AFromCostCentre, ATotalIncome, ATotalExpense,
                                ATotalTransfer, ref ADBTransaction))
                        {
	                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
			                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ") and data is identical. ";
	                        LogMessage += AFileName + " will be skipped";
			                Atw.WriteLine(LogMessage);
                            AProcessFile = false;
                        }
                        else
                        {
	                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
			                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
			                LogMessage += OldTransactionRow.TransactionDate.ToShortDateString() + ") but data has changed. ";
			                LogMessage += "File " + AFileName + " will be skipped.";
			                Atw.WriteLine(LogMessage);
                            AProcessFile = false;
                        }
                    }
                    else
                    {
                        if (SummaryStewardshipTransactionsMatch(ALedgerNumber, ABatchNumber, AFromCostCentre, ATotalIncome, ATotalExpense,
                                ATotalTransfer, ref ADBTransaction))
                        {
	                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
			                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
			                LogMessage += OldTransactionRow.TransactionDate.ToShortDateString() + ") but the run number is unknown. ";
			                LogMessage += "File " + AFileName + " is for run number " + ARunNumber.ToString() + " but the data is identical to the ";
			                LogMessage += "existing batch so it will not be processed.";
			                Atw.WriteLine(LogMessage);
			                AProcessFile = false;
                        }
                        else
                        {
                            AProcessFile = true;
                        }
                    }
                }
                else
                {
                    BatchRunNumber = Convert.ToInt32(OldTransRowRef.Substring(OldTransRowRef.Length - 3, 3));

                    if (ARunNumber == BatchRunNumber)
                    {
                        if (SummaryStewardshipTransactionsMatch(ALedgerNumber, ABatchNumber, AFromCostCentre, ATotalIncome, ATotalExpense,
                                ATotalTransfer, ref ADBTransaction))
                        {
	                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
			                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
			                LogMessage += OldTransactionRow.TransactionDate.ToShortDateString() + ") and data is identical. ";
	                        LogMessage += AFileName + " will be skipped";
			                Atw.WriteLine(LogMessage);
                            AProcessFile = false;
                        }
                        else
                        {
	                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
			                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
			                LogMessage += OldTransactionRow.TransactionDate.ToShortDateString() + ") but data has changed. ";
	                        LogMessage += AFileName + " will be skipped";
			                Atw.WriteLine(LogMessage);
                            AProcessFile = false;
                        }
                    }
                    else if (ARunNumber < BatchRunNumber)
                    {
                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
		                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
		                LogMessage += OldTransactionRow.TransactionDate.ToShortDateString() + ").  The run number on that batch ";
		                LogMessage += "(" + BatchRunNumber.ToString() + ") is higher than the run number on the file being processed and therefore ";
                        LogMessage += AFileName + " will be skipped";
		                Atw.WriteLine(LogMessage);
                        AProcessFile = false;
                    }
                    else if (BatchRunNumber == 0)
                    {
                        if (SummaryStewardshipTransactionsMatch(ALedgerNumber, ABatchNumber, AFromCostCentre, ATotalIncome, ATotalExpense,
                                ATotalTransfer, ref ADBTransaction))
                        {
	                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
			                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
			                LogMessage += OldTransactionRow.TransactionDate.ToShortDateString() + ") and is run number 0. ";
			                LogMessage += "File " + AFileName + " is for run number " + ARunNumber.ToString() + " but the data is identical to the ";
			                LogMessage += "existing batch so it will not be processed.";
			                Atw.WriteLine(LogMessage);
                            AProcessFile = false;
                        }
                        else
                        {
                            AProcessFile = true;
                        }
                    	
                    }
                    else
                    {
                    	AProcessFile = true;
                    }
                }
                return;
            }
            
            /* if we still haven't found the batch try previous years */
            APreviousYearTransactionTable PreviousTransTable = new APreviousYearTransactionTable();
            APreviousYearTransactionRow TemplateRow3 = (APreviousYearTransactionRow)PreviousTransTable.NewRowTyped(false);

            TemplateRow3.LedgerNumber = ALedgerNumber;
            TemplateRow3.BatchNumber = ABatchNumber;

            StringCollection operators3 = StringHelper.InitStrArr(new string[] { "=", "=" });

            APreviousYearTransactionTable PreviousTransactionTable = APreviousYearTransactionAccess.LoadUsingTemplate(TemplateRow3,
                operators3,
                null,
                ADBTransaction);
            APreviousYearTransactionRow PreviousTransactionRow = (APreviousYearTransactionRow)PreviousTransactionTable.Rows[0];

            if (PreviousTransactionTable.Count > 0)
            {
                string PreviousTransRowRef = PreviousTransactionRow.Reference;
                //BatchRunNumber = Convert.ToInt32(TransRowRef.Substring(TransRowRef.Length - 3, 3));

                if (PreviousTransRowRef.Length < 7)
                {
                    if (ARunNumber == 0)
                    {
                        if (SummaryStewardshipTransactionsMatch(ALedgerNumber, ABatchNumber, AFromCostCentre, ATotalIncome, ATotalExpense,
                                ATotalTransfer, ref ADBTransaction))
                        {
	                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
			                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
			                LogMessage += PreviousTransactionRow.TransactionDate.ToShortDateString() + ") and data is identical. ";
	                        LogMessage += AFileName + " will be skipped";
			                Atw.WriteLine(LogMessage);
                            AProcessFile = false;
                        }
                        else
                        {
	                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
			                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
			                LogMessage += PreviousTransactionRow.TransactionDate.ToShortDateString() + ") but data has changed. ";
	                        LogMessage += AFileName + " will be skipped";
			                Atw.WriteLine(LogMessage);
                            AProcessFile = false;
                        }
                    }
                    else
                    {
                        if (SummaryStewardshipTransactionsMatch(ALedgerNumber, ABatchNumber, AFromCostCentre, ATotalIncome, ATotalExpense,
                                ATotalTransfer, ref ADBTransaction))
                        {
	                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
			                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
			                LogMessage += PreviousTransactionRow.TransactionDate.ToShortDateString() + ") but the run number is unknown. ";
			                LogMessage += "File " + AFileName + " is for run number " + ARunNumber.ToString() + " but the data is identical to the ";
			                LogMessage += "existing batch so it will not be processed.";
			                Atw.WriteLine(LogMessage);
                            AProcessFile = false;
                        }
                        else
                        {
                            AProcessFile = true;
                        }
                    }
                }
                else
                {
                    BatchRunNumber = Convert.ToInt32(PreviousTransRowRef.Substring(PreviousTransRowRef.Length - 3, 3));

                    if (ARunNumber == BatchRunNumber)
                    {
                        if (SummaryStewardshipTransactionsMatch(ALedgerNumber, ABatchNumber, AFromCostCentre, ATotalIncome, ATotalExpense,
                                ATotalTransfer, ref ADBTransaction))
                        {
	                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
			                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
			                LogMessage += PreviousTransactionRow.TransactionDate.ToShortDateString() + ") and data is identical. ";
	                        LogMessage += AFileName + " will be skipped";
			                Atw.WriteLine(LogMessage);
                            AProcessFile = false;
                        }
                        else
                        {
	                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
			                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
			                LogMessage += PreviousTransactionRow.TransactionDate.ToShortDateString() + ") but data has changed. ";
	                        LogMessage += AFileName + " will be skipped";
			                Atw.WriteLine(LogMessage);
                            AProcessFile = false;
                        }
                    }
                    else if (ARunNumber < BatchRunNumber)
                    {
                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
		                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
		                LogMessage += PreviousTransactionRow.TransactionDate.ToShortDateString() + ").  The run number on that batch ";
		                LogMessage += "(" + BatchRunNumber.ToString() + ") is higher than the run number on the file being processed and therefore ";
                        LogMessage += AFileName + " will be skipped";
		                Atw.WriteLine(LogMessage);
                        AProcessFile = false;
                    }
                    else if (BatchRunNumber == 0)
                    {
                        if (SummaryStewardshipTransactionsMatch(ALedgerNumber, ABatchNumber, AFromCostCentre, ATotalIncome, ATotalExpense,
                                ATotalTransfer, ref ADBTransaction))
                        {
	                		LogMessage = "Stewardship Batch for " + AFromCostCentreName + " " + APeriodName + " " + AYear.ToString();
			                LogMessage += " already exists (Batch " + ABatchNumber.ToString() + ", date ";
			                LogMessage += PreviousTransactionRow.TransactionDate.ToShortDateString() + ") and is run number 0. ";
			                LogMessage += "File " + AFileName + " is for run number " + ARunNumber.ToString() + " but the data is identical to the ";
			                LogMessage += "existing batch so it will not be processed.";
			                Atw.WriteLine(LogMessage);
                            AProcessFile = false;
                        }
                        else
                        {
                            AProcessFile = true;
                        }
                    }
                    else
                    {
                        AProcessFile = true;
                    }
                }

            }
            
        }
        

        /// <summary>
        /// Creates a stewardship transaction using the parameters specified
        /// </summary>
        /// <param name="ALedgerNumber">ICH Ledger number</param>
        /// <param name="ABatchNumber">The number of the batch in which this transaction should be created</param>
        /// <param name="AJournalNumber">The number of the journal in which this transaction should be created </param>
        /// <param name="ATransactionType">"INCOME", "EXPENSE" or "TRANSFER"</param>
        /// <param name="ACostCentre">Cost Centre for transaction</param>
        /// <param name="ANarrative">Narrative to use for transaction</param>
        /// <param name="AAmount">Transaction amount</param>
        /// <param name="AReference">Transaction reference</param>
        /// <param name="ADate">Transaction date</param>
        /// <param name="ASummary">Is this a summary transaction or a detail transaction</param>
        /// <param name="Atw">TextWriter for log file</param>
        /// <param name="ADBTransaction">Current database transaction</param>
        /// <param name="ASuccessful">Was the transaction successfully created</param>
        private void CreateStewardshipTransaction(int ALedgerNumber, int ABatchNumber, int AJournalNumber,
            string ATransactionType, string ACostCentre, string ANarrative,
            decimal AAmount, string AReference, DateTime ADate,
            bool ASummary, ref TextWriter Atw, ref TDBTransaction ADBTransaction, out bool ASuccessful)
        {
            string Account = string.Empty;
            bool TransCreateOk = false;
            bool DrCrIndicator;
            string LogMessage = string.Empty;

            ASuccessful = false;

            /* only create the transaction if it is non-zero */
            if (AAmount != 0)
            {
                /* set debit credit indicator based on type of transaction and sign of amount */
                if (((ATransactionType == MFinanceConstants.TRANSACTION_TYPE_INCOME) && (AAmount < 0))
                    || ((ATransactionType == MFinanceConstants.TRANSACTION_TYPE_EXPENSE) && (AAmount > 0))
                    || ((ATransactionType == MFinanceConstants.TRANSACTION_TYPE_TRANSFER) && (AAmount > 0))
                    )
                {
                    DrCrIndicator = !ASummary;
                }
                else
                {
                    DrCrIndicator = ASummary;
                }

                if (AAmount < 0)
                {
                    AAmount = -AAmount;
                }

                ACostCentreTable CostCentreTable = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, ACostCentre, ADBTransaction);
                ACostCentreRow CostCentreRow = (ACostCentreRow)CostCentreTable.Rows[0];

                if (CostCentreRow == null)
                {
                    ASuccessful = false;
					LogMessage = "Cost Centre " + ACostCentre +" does not exist.";
					Atw.WriteLine(LogMessage);
                    return;
                }

                if (CostCentreRow.CostCentreActiveFlag)
                {
                    /* set account code based on type of transaction and whether or not it is a summary
                     * transaction */
                    switch (ATransactionType)
                    {
                        case MFinanceConstants.TRANSACTION_TYPE_INCOME:

                            if (ASummary)
                            {
                                Account = MFinanceConstants.ICH_ACCT_LOCAL_LEDGER;                                 //8520
                            }
                            else
                            {
                                Account = MFinanceConstants.ICH_ACCT_FOREIGN_INCOME_UNIDENTIFIED;                                 //1900
                            }

                            break;

                        case MFinanceConstants.TRANSACTION_TYPE_EXPENSE:

                            if (ASummary)
                            {
                                Account = MFinanceConstants.ICH_ACCT_LOCAL_LEDGER;                                 //8520
                            }
                            else
                            {
                                Account = MFinanceConstants.ICH_ACCT_FOREIGN_EXPENSES_UNIDENTIFIED;                                 //5400
                            }

                            break;

                        case MFinanceConstants.TRANSACTION_TYPE_TRANSFER:

                            if (ASummary)
                            {
                                Account = MFinanceConstants.ICH_ACCT_DEPOSITS_WITH_ICH;                                 //8540
                            }
                            else
                            {
                                Account = MFinanceConstants.ICH_ACCT_SETTLEMENT_TRANSFERS;                                 //5600
                            }

                            break;
                    }
                }
                else
                {
                    /* if the fund is no longer active we post to ICH (0400) instead */
                    Account = MFinanceConstants.ICH_ACCT_SUSPENSE_ACCOUNT;                     //8200
                    ACostCentre = MFinanceConstants.ICH_COST_CENTRE;             //0400
                }

                /* create the transaction */

                /*RUN gl1130o.p ("new":U,
                 * ALedgerNumber,
                 * ABatchNumber,
                 * AJournalNumber,
                 * ?,
                 * ANarrative,
                 * Account,
                 * ACostCentre,
                 * AAmount,
                 * ADate,
                 * DrCrIndicator,
                 * ?,
                 * ?,
                 * ?,
                 * ?,
                 * ?,
                 * AReference,
                 * FALSE,
                 * 0,
                 * OUTPUT TransactionNumber,
                 * OUTPUT TransCreateOk).*/

                ASuccessful = TransCreateOk;
            }
            else
            {
                ASuccessful = true;
            }
        }

        /// <summary>
        /// Checks to see if any files failed the import
        /// </summary>
        /// <param name="AUnsuccessfulFileList"></param>
        /// <returns>Returns the list of all files that failed</returns>
        private string ListUnprocessedFiles(string AUnsuccessfulFileList)
        {
            string UnprocessedFileList = "The following files could not be processed. Please see the logfile for details.\r\n";

            string[] UnprocessedFiles = AUnsuccessfulFileList.Split(',');

            if (AUnsuccessfulFileList != string.Empty)
            {
                foreach (string FailedFile in UnprocessedFiles)
                {
                    if (FailedFile.Trim() != string.Empty)
                    {
                        UnprocessedFileList += "\r\n" + FailedFile;
                    }
                }
            }

            return UnprocessedFileList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AValue"></param>
        /// <returns></returns>
        private string ChangeToAmericanFormat(string AValue)
        {
            return AValue;
        }
    }
}