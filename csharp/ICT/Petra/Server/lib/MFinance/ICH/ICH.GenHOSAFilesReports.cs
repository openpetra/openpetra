//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christophert, timop
//
// Copyright 2004-2014 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Xml;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.GL.Data.Access;
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
using Ict.Petra.Server.MSysMan.Maintenance.UserDefaults.WebConnectors;
using Ict.Petra.Server.MSysMan.Security;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MFinance.ICH.WebConnectors
{
    /// <summary>
    /// Class for the generation of "Home Office Statement of Accounts" reports for each
    ///   foreign cost centre (ledger/fund).  This is basically a modified Trial Balance.
    /// </summary>
    public class TGenHOSAFilesReportsWebConnector
    {
        /// <summary>
        /// Performs the ICH code to generate HOSA Files/Reports.
        ///  Relates to gi3200.p/gi3200-1.i
        /// </summary>
        /// <param name="ALedgerNumber">Ledger number</param>
        /// <param name="APeriodNumber">Period number</param>
        /// <param name="AIchNumber">ICH number</param>
        /// <param name="ACostCentre">Cost Centre</param>
        /// <param name="ACurrencySelect">Currency: B = base I = intl</param>
        /// <param name="AFileName">File name</param>
        /// <param name="AVerificationResult">Error messaging</param>
        /// <returns>True if successful</returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool GenerateHOSAFiles(int ALedgerNumber,
            int APeriodNumber,
            int AIchNumber,
            string ACostCentre,
            String ACurrencySelect,
            string AFileName,
            out TVerificationResultCollection AVerificationResult
            )
        {
            bool Successful = false;

            GLBatchTDS MainDS = new GLBatchTDS();

            TVerificationResultCollection VerificationResult = new TVerificationResultCollection();

            AVerificationResult = VerificationResult;

            TDBTransaction DBTransaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref DBTransaction,
                delegate
                {
                    //Load tables needed: AccountingPeriod, Ledger, Account, Cost Centre, Transaction, Gift Batch, ICHStewardship
                    ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, DBTransaction);

                    /* Retrieve info on the ledger. */
                    ALedgerRow LedgerRow = (ALedgerRow)MainDS.ALedger.Rows[0];
                    String Currency = (ACurrencySelect == MFinanceConstants.CURRENCY_BASE) ? LedgerRow.BaseCurrency : LedgerRow.IntlCurrency;

                    /*              String StoreNumericFormat = "#" + CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + "##0";
                     *
                     *              if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits > 0)
                     *              {
                     *                  string DecPls = new String('0', CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits);
                     *                  StoreNumericFormat += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + DecPls;
                     *              }
                     */
                    AAccountingPeriodTable AccountingPeriodTable =
                        AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, APeriodNumber, DBTransaction);
                    AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];
                    String MonthName = AccountingPeriodRow.AccountingPeriodDesc;

                    //Create table definitions
                    DataTable TableForExport = new DataTable();
                    TableForExport.Columns.Add("CostCentre", typeof(string));
                    TableForExport.Columns.Add("Account", typeof(string));
                    TableForExport.Columns.Add("LedgerMonth", typeof(string));
                    TableForExport.Columns.Add("ICHPeriod", typeof(string));
                    TableForExport.Columns.Add("Date", typeof(DateTime));
                    TableForExport.Columns.Add("IndividualDebitTotal", typeof(decimal));
                    TableForExport.Columns.Add("IndividualCreditTotal", typeof(decimal));

                    string TableForExportHeader = "/** Header **" + "," +
                                                  APeriodNumber.ToString() + "," +
                                                  TLedgerInfo.GetStandardCostCentre(ALedgerNumber) + "," +
                                                  ACostCentre + "," +
                                                  DateTime.Today.ToShortDateString() + "," +
                                                  Currency;

                    //See gi3200.p ln: 170
                    //Select any gift transactions to export
                    string strSql = TDataBase.ReadSqlFile("ICH.HOSAExportGifts.sql");

                    OdbcParameter parameter;

                    List <OdbcParameter>parameters = new List <OdbcParameter>();
                    parameter = new OdbcParameter("LedgerNumber", OdbcType.Int);
                    parameter.Value = ALedgerNumber;
                    parameters.Add(parameter);
                    parameter = new OdbcParameter("Year", OdbcType.Int);
                    parameter.Value = LedgerRow.CurrentFinancialYear;
                    parameters.Add(parameter);
                    parameter = new OdbcParameter("CostCentre", OdbcType.VarChar);
                    parameter.Value = ACostCentre;
                    parameters.Add(parameter);

                    DataTable TmpTable = DBAccess.GDBAccessObj.SelectDT(strSql, "table", DBTransaction, parameters.ToArray());

                    foreach (DataRow untypedTransRow in TmpTable.Rows)
                    {
                        string gLMAcctCode = untypedTransRow[3].ToString();
                        string gLMCostCCode = untypedTransRow[4].ToString();
                        string gLMAcctType = untypedTransRow[5].ToString();

                        if (gLMAcctType == MFinanceConstants.ACCOUNT_TYPE_INCOME)     //a_account.a_account_type_c
                        {
                            DateTime PeriodStartDate = AccountingPeriodRow.PeriodStartDate;
                            DateTime PeriodEndDate = AccountingPeriodRow.PeriodEndDate;

                            /*RUN Export_gifts(INPUT pv_ledger_number_i...*/

                            //gi3200-1.i
                            ExportGifts(ALedgerNumber,
                                ACostCentre,
                                gLMAcctCode,
                                MonthName,
                                APeriodNumber,
                                PeriodStartDate,
                                PeriodEndDate,
                                ACurrencySelect,
                                AIchNumber,
                                TableForExport,
                                VerificationResult);
                        }

                        /* Then see if there are any GL transactions to export */
                        //gi3200.i ln:33

                        /*
                         * This scheme with ODBC parameters consistently causes an "input string is the wrong type" eror:
                         *
                         * strSql = TDataBase.ReadSqlFile("ICH.HOSAExportGLTrans.sql");
                         * OdbcParameter[] SqlParams = new OdbcParameter[] {
                         *      new OdbcParameter("LedgerNumber", (Int32)ALedgerNumber),
                         *      new OdbcParameter("Account", (String)gLMAcctCode),
                         *      new OdbcParameter("CostCentre", (String)gLMCostCCode),
                         *      new OdbcParameter("Narrative", (String)MFinanceConstants.NARRATIVE_YEAR_END_REALLOCATION),
                         *      new OdbcParameter("ICHNumber", (Int32)AIchNumber),
                         *      new OdbcParameter("ICHNumber2", (Int32)AIchNumber),
                         *      new OdbcParameter("PeriodNumber", (Int32)APeriodNumber)
                         *  };
                         * DataTable TmpTransTable = DBAccess.GDBAccessObj.SelectDT(strSql, "Transactions", DBTransaction, SqlParams);
                         */

                        strSql = "SELECT Trans.a_ledger_number_i, Trans.a_batch_number_i, Trans.a_journal_number_i, Trans.a_transaction_number_i, " +
                                 "Trans.a_account_code_c, Trans.a_cost_centre_code_c, Trans.a_transaction_date_d, Trans.a_transaction_amount_n, " +
                                 "Trans.a_amount_in_base_currency_n, Trans.a_amount_in_intl_currency_n, Trans.a_ich_number_i, Trans.a_system_generated_l, "
                                 +
                                 "Trans.a_narrative_c, Trans.a_debit_credit_indicator_l  FROM public.a_transaction AS Trans, public.a_journal AS Journal "
                                 +
                                 "WHERE Trans.a_ledger_number_i = Journal.a_ledger_number_i AND Trans.a_batch_number_i = Journal.a_batch_number_i " +
                                 "AND Trans.a_journal_number_i = Journal.a_journal_number_i " +
                                 String.Format(
                            "AND Trans.a_ledger_number_i = {0} AND Trans.a_account_code_c = '{1}' AND Trans.a_cost_centre_code_c = '{2}' " +
                            "AND Trans.a_transaction_status_l = true AND NOT (Trans.a_narrative_c LIKE '{3}%' AND Trans.a_system_generated_l = true) "
                            +
                            "AND ((Trans.a_ich_number_i + {4}) = Trans.a_ich_number_i OR Trans.a_ich_number_i = {4}) " +
                            "AND Journal.a_journal_period_i = {5};",
                            ALedgerNumber,
                            gLMAcctCode,
                            gLMCostCCode,
                            MFinanceConstants.NARRATIVE_YEAR_END_REALLOCATION,
                            AIchNumber,
                            APeriodNumber
                            );

                        DataTable TmpTransTable = DBAccess.GDBAccessObj.SelectDT(strSql, "Transactions", DBTransaction);

                        foreach (DataRow untypedTransactRow in TmpTransTable.Rows)
                        {
                            Decimal DebitTotal = 0;
                            Decimal CreditTotal = 0;

                            bool Debit = Convert.ToBoolean(untypedTransactRow[13]);             //a_transaction.a_debit_credit_indicator_l
                            bool SystemGenerated = Convert.ToBoolean(untypedTransactRow[11]);   //a_transaction.a_system_generated_l
                            //TODO: Calendar vs Financial Date Handling - Check if number of ledger periods needs to be used here and not 12 assumed
                            string Narrative = untypedTransactRow[12].ToString();               //a_transaction.a_narrative_c
                            DateTime TransactionDate = Convert.ToDateTime(untypedTransactRow[6]); //a_transaction.a_transaction_date_d

                            if (ACurrencySelect == MFinanceConstants.CURRENCY_BASE)
                            {
                                decimal AmountInBaseCurrency = Convert.ToDecimal(untypedTransactRow[8]);  //a_transaction.a_amount_in_base_currency_n

                                /* find transaction amount and store as debit or credit */
                                if (Debit)
                                {
                                    DebitTotal += AmountInBaseCurrency;
                                }
                                else
                                {
                                    CreditTotal += AmountInBaseCurrency;
                                }
                            }
                            else
                            {
                                decimal AmountInIntlCurrency = Convert.ToDecimal(untypedTransactRow[9]);   //a_transaction.a_amount_in_intl_currency_n

                                if (Debit)
                                {
                                    DebitTotal += AmountInIntlCurrency;
                                }
                                else
                                {
                                    CreditTotal += AmountInIntlCurrency;
                                }
                            }

                            TLogging.LogAtLevel(4, "HOSA-Narrative: " + Narrative);

                            //Check for specific narrative strings
                            bool IsNarrativeGBGiftBatch = false;
                            int LenNarrativeGBGiftBatch = MFinanceConstants.NARRATIVE_GB_GIFT_BATCH.Length;
                            bool IsNarrativeGiftsReceivedGiftBatch = false;
                            int LenNarrativeGiftsReceivedGiftBatch = MFinanceConstants.NARRATIVE_GIFTS_RECEIVED_GIFT_BATCH.Length;

                            if (Narrative.Length >= LenNarrativeGiftsReceivedGiftBatch)
                            {
                                IsNarrativeGiftsReceivedGiftBatch =
                                    (Narrative.Substring(0,
                                         LenNarrativeGiftsReceivedGiftBatch) == MFinanceConstants.NARRATIVE_GIFTS_RECEIVED_GIFT_BATCH);
                            }

                            if (Narrative.Length >= LenNarrativeGBGiftBatch)
                            {
                                IsNarrativeGBGiftBatch =
                                    (Narrative.Substring(0, LenNarrativeGBGiftBatch) == MFinanceConstants.NARRATIVE_GB_GIFT_BATCH);
                            }

                            if ((gLMAcctType.ToUpper() != MFinanceConstants.ACCOUNT_TYPE_INCOME.ToUpper())
                                || !(SystemGenerated && (IsNarrativeGBGiftBatch || IsNarrativeGiftsReceivedGiftBatch)))
                            {
                                // Put transaction information
                                DataRow DR = (DataRow)TableForExport.NewRow();

                                DR[0] = gLMCostCCode;
                                DR[1] = ConvertAccount(gLMAcctCode);
                                DR[2] = ALedgerNumber.ToString() + MonthName + ":" + Narrative;
                                DR[3] = "ICH-" + APeriodNumber.ToString("00");
                                DR[4] = TransactionDate;
                                DR[5] = DebitTotal;
                                DR[6] = CreditTotal;

                                TableForExport.Rows.Add(DR);
                            }
                        }
                    }

                    TableForExport.AcceptChanges();

                    TLogging.LogAtLevel(4, "HOSA-TableForExport: " + TableForExport.Rows.Count.ToString());

                    //DataTables to XML to CSV
                    XmlDocument doc = TDataBase.DataTableToXml(TableForExport);

                    TCsv2Xml.Xml2Csv(doc, AFileName);

                    //Replace the default CSV header row with OM specific
                    ReplaceHeaderInFile(AFileName, TableForExportHeader, ref VerificationResult);
                    Successful = true;
                }); // Get NewOrExisting AutoReadTransaction

            return Successful;
        } // Generate HOSA Files

        /// <summary>
        /// Replaces the first line of a text file with a specified string
        /// </summary>
        /// <param name="AFileName">File name (including path) to process</param>
        /// <param name="AHeaderText">Text to insert in first line</param>
        /// <param name="AVerificationResult">Error messaging</param>
        [RequireModulePermission("FINANCE-3")]
        public static bool ReplaceHeaderInFile(string AFileName, string AHeaderText, ref TVerificationResultCollection AVerificationResult)
        {
            bool retVal = true;

            try
            {
                StringBuilder newFileContents = new StringBuilder();

                string[] file = File.ReadAllLines(AFileName);

                bool IsFirstLine = true;

                foreach (string line in file)
                {
                    //If first line
                    if (IsFirstLine)
                    {
                        newFileContents.Append(AHeaderText + "\r\n");
                        IsFirstLine = false;
                    }
                    else
                    {
                        newFileContents.Append(line + "\r\n");
                    }
                }

                File.WriteAllText(AFileName, newFileContents.ToString());
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                retVal = false;
                AVerificationResult.Add(new TVerificationResult("Generating HOSA Files",
                        "Unable to replace the header in file: " + AFileName,
                        "Replacing Header in Text File",
                        TResultSeverity.Resv_Critical, new Guid()));
                throw new Exception("Error in generating HOSA Files. Unable to replace the header in file: " + AFileName);
            }

            return retVal;
        }

        /// <summary>
        ///  Exports gifts in HOSA file format.
        ///  Relates to gi3200-1.i
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACostCentre"></param>
        /// <param name="AAcctCode"></param>
        /// <param name="AMonthName"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="APeriodStartDate"></param>
        /// <param name="APeriodEndDate"></param>
        /// <param name="ACurrencySelect"></param>
        /// <param name="AIchNumber"></param>
        /// <param name="AExportDataTable"></param>
        /// <param name="AVerificationResult"></param>
        [NoRemoting]
        [RequireModulePermission("FINANCE-3")]
        public static void ExportGifts(int ALedgerNumber,
            string ACostCentre,
            string AAcctCode,
            string AMonthName,
            int APeriodNumber,
            DateTime APeriodStartDate,
            DateTime APeriodEndDate,
            string ACurrencySelect,
            int AIchNumber,
            DataTable AExportDataTable,
            TVerificationResultCollection AVerificationResult)
        {
            /* Define local variables */
            bool FirstLoopFlag = true;
            Int32 LastRecipKey = 0;
            string LastGroup = string.Empty;
            string LastDetail = string.Empty;
            string LastDetailDesc = string.Empty;
            decimal IndividualDebitTotal = 0;
            decimal IndividualCreditTotal = 0;

            string ExportDescription = string.Empty;
            Int32 tmpLastRecipKey = 0;
            string tmpLastGroup = string.Empty;
            string tmpLastDetail = string.Empty;

            //Find and total each gift transaction
            string SQLStmt = TDataBase.ReadSqlFile("ICH.HOSAExportGiftsInner.sql");

            TDBTransaction DBTransaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref DBTransaction,
                delegate
                {
                    OdbcParameter parameter;

                    List <OdbcParameter>parameters = new List <OdbcParameter>();
                    parameter = new OdbcParameter("LedgerNumber", OdbcType.Int);
                    parameter.Value = ALedgerNumber;
                    parameters.Add(parameter);
                    parameter = new OdbcParameter("CostCentre", OdbcType.VarChar);
                    parameter.Value = ACostCentre;
                    parameters.Add(parameter);
                    parameter = new OdbcParameter("ICHNumber", OdbcType.Int);
                    parameter.Value = AIchNumber;
                    parameters.Add(parameter);
                    parameter = new OdbcParameter("BatchStatus", OdbcType.VarChar);
                    parameter.Value = MFinanceConstants.BATCH_POSTED;
                    parameters.Add(parameter);
                    parameter = new OdbcParameter("StartDate", OdbcType.DateTime);
                    parameter.Value = APeriodStartDate;
                    parameters.Add(parameter);
                    parameter = new OdbcParameter("EndDate", OdbcType.DateTime);
                    parameter.Value = APeriodEndDate;
                    parameters.Add(parameter);
                    parameter = new OdbcParameter("AccountCode", OdbcType.VarChar);
                    parameter.Value = AAcctCode;
                    parameters.Add(parameter);

                    DataTable TmpTable = DBAccess.GDBAccessObj.SelectDT(SQLStmt, "table", DBTransaction, parameters.ToArray());

                    foreach (DataRow untypedTransRow in TmpTable.Rows)
                    {
                        /* Print totals etc. found for last recipient */
                        /* Only do after first loop due to last recipient key check */

                        tmpLastRecipKey = Convert.ToInt32(untypedTransRow[8]);  //a_gift_detail.p_recipient_key_n
                        tmpLastGroup = untypedTransRow[6].ToString();           //a_motivation_detail.a_motivation_group_code_c
                        tmpLastDetail = untypedTransRow[7].ToString();          //a_motivation_detail.a_motivation_detail_code_c

                        if (!FirstLoopFlag
                            && ((tmpLastRecipKey != LastRecipKey)
                                || (tmpLastGroup != LastGroup)
                                || (tmpLastDetail != LastDetail)
                                )
                            )
                        {
                            if ((IndividualCreditTotal != 0)
                                || (IndividualDebitTotal != 0))
                            {
                                if (LastRecipKey != 0)
                                {
                                    /* Find partner short name details */
                                    PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(LastRecipKey, DBTransaction);
                                    PPartnerRow PartnerRow = (PPartnerRow)PartnerTable.Rows[0];

                                    LastDetailDesc += " : " + PartnerRow.PartnerShortName;

                                    ExportDescription = ALedgerNumber.ToString() + AMonthName + ":" + LastDetailDesc;
                                }
                                else
                                {
                                    AMotivationGroupTable MotivationGroupTable = AMotivationGroupAccess.LoadByPrimaryKey(ALedgerNumber,
                                        LastGroup,
                                        DBTransaction);
                                    AMotivationGroupRow MotivationGroupRow = (AMotivationGroupRow)MotivationGroupTable.Rows[0];

                                    ExportDescription = ALedgerNumber.ToString() + AMonthName + ":" +
                                                        MotivationGroupRow.MotivationGroupDescription.TrimEnd(
                                        new Char[] { (' ') }) + "," + LastDetailDesc;
                                }

                                //Add data to export table
                                DataRow DR = (DataRow)AExportDataTable.NewRow();

                                DR[0] = ACostCentre;
                                DR[1] = ConvertAccount(AAcctCode);
                                DR[2] = ExportDescription;
                                DR[3] = "ICH-" + APeriodNumber.ToString("00");
                                DR[4] = APeriodEndDate;
                                DR[5] = IndividualDebitTotal;
                                DR[6] = IndividualCreditTotal;

                                AExportDataTable.Rows.Add(DR);

                                /* Reset total */
                                IndividualDebitTotal = 0;
                                IndividualCreditTotal = 0;
                            }
                        }

                        if (ACurrencySelect == MFinanceConstants.CURRENCY_BASE)
                        {
                            Decimal GiftAmount = Convert.ToDecimal(untypedTransRow[4]);          //a_gift_detail.a_gift_amount_n

                            if (GiftAmount < 0)
                            {
                                IndividualDebitTotal -= GiftAmount;
                            }
                            else
                            {
                                IndividualCreditTotal += GiftAmount;
                            }
                        }
                        else
                        {
                            Decimal IntlGiftAmount = Convert.ToDecimal(untypedTransRow[5]);          //a_gift_detail.a_gift_amount_intl_n

                            if (IntlGiftAmount < 0)
                            {
                                IndividualDebitTotal -= IntlGiftAmount;
                            }
                            else
                            {
                                IndividualCreditTotal += IntlGiftAmount;
                            }
                        }

                        /* Set loop variables */
                        LastRecipKey = tmpLastRecipKey;
                        LastGroup = tmpLastGroup;
                        LastDetail = tmpLastDetail;
                        LastDetailDesc = Convert.ToString(untypedTransRow[10]);         //a_motivation_detail.a_motivation_detail_desc_c
                        FirstLoopFlag = false;
                    } // foreach

                    /* Print totals etc. found for last recipient */
                    /* Only do after first loop due to last recipient key check */
                    if (!FirstLoopFlag && ((IndividualCreditTotal != 0) || (IndividualDebitTotal != 0)))
                    {
                        if (LastRecipKey != 0)
                        {
                            /* Find partner short name details */
                            PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(LastRecipKey, DBTransaction);
                            PPartnerRow PartnerRow = (PPartnerRow)PartnerTable.Rows[0];

                            LastDetailDesc += ":" + PartnerRow.PartnerShortName;

                            ExportDescription = ALedgerNumber.ToString() + AMonthName + ":" + LastDetailDesc;
                        }
                        else
                        {
                            AMotivationGroupTable MotivationGroupTable =
                                AMotivationGroupAccess.LoadByPrimaryKey(ALedgerNumber, LastGroup, DBTransaction);
                            AMotivationGroupRow MotivationGroupRow = (AMotivationGroupRow)MotivationGroupTable.Rows[0];


                            ExportDescription = ALedgerNumber.ToString() + AMonthName + ":" +
                                                MotivationGroupRow.MotivationGroupDescription.TrimEnd() + "," + LastDetailDesc;
                        }

                        //Add rows to export table
                        DataRow DR = (DataRow)AExportDataTable.NewRow();

                        DR[0] = ACostCentre;
                        DR[1] = ConvertAccount(AAcctCode);
                        DR[2] = ExportDescription;
                        DR[3] = "ICH-" + APeriodNumber.ToString("00");
                        DR[4] = APeriodEndDate;;
                        DR[5] = IndividualDebitTotal;
                        DR[6] = IndividualCreditTotal;

                        AExportDataTable.Rows.Add(DR);
                    }
                }); // Get NewOrExisting AutoReadTransaction
        } // Export Gifts

        /// <summary>
        ///
        /// </summary>
        /// <param name="AAccountCode">Account to check for conversion</param>
        /// <returns>Converted string</returns>
        private static string ConvertAccount(string AAccountCode)
        {
            switch (AAccountCode)
            {
                case "0100":
                    AAccountCode = "1100";
                    break;

                case "0200":
                    AAccountCode = "1200";
                    break;

                case "0400":
                    AAccountCode = "1400";
                    break;

                case "5601":
                    AAccountCode = "8500";
                    break;

                default:
                    break;
            }

            return AAccountCode;
        }

        /*
         * Not used
         * /// <summary>
         * /// Performs the ICH code to generate HOSA Files/Reports.
         * /// ** CLEARLY INCOMPLETE! **  Relates to gl2120-1.i
         * /// </summary>
         * /// <param name="ALedgerNumber">ICH Ledger number</param>
         * /// <param name="APeriodNumber">Period number</param>
         * /// <param name="AIchNumber">ICH number</param>
         * /// <param name="ACurrencySelect">Currency: B=base, I=International</param>
         * [RequireModulePermission("FINANCE-3")]
         * public static void GenerateHOSAReports(int ALedgerNumber,
         *  int APeriodNumber,
         *  int AIchNumber,
         *  string ACurrencySelect
         *  )
         * {
         *  TDBTransaction DBTransaction = null;
         *  DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, ref DBTransaction,
         *      delegate
         *      {
         *          ALedgerTable ALedger = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, DBTransaction);
         *          AJournalTable AJournal = new AJournalTable();
         *          ATransactionTable ATransaction = new ATransactionTable();
         *          ACostCentreTable ACostCentre = new ACostCentreTable();
         *
         *          //
         *          // Load the Journals, and Transactions for this period:
         *          String JournalQuery = "SELECT PUB_a_journal.* FROM PUB_a_batch, PUB_a_journal WHERE " +
         *                                "PUB_a_batch.a_ledger_number_i = " + ALedgerNumber +
         *                                " AND PUB_a_batch.a_batch_year_i = " + ALedger[0].CurrentFinancialYear +
         *                                " AND PUB_a_batch.a_batch_period_i = " + APeriodNumber +
         *                                " AND PUB_a_batch.a_batch_status_c = 'Posted'" +
         *                                " AND PUB_a_batch.a_ledger_number_i = PUB_a_journal.a_ledger_number_i" +
         *                                " AND PUB_a_batch.a_batch_number_i = PUB_a_journal.a_batch_number_i";
         *
         *          DBAccess.GDBAccessObj.SelectDT(AJournal, JournalQuery, DBTransaction);
         *
         *          String TransactionQuery = "SELECT PUB_a_transaction.* FROM PUB_a_batch, PUB_a_transaction WHERE " +
         *                                    "PUB_a_batch.a_ledger_number_i = " + ALedgerNumber +
         *                                    " AND PUB_a_batch.a_batch_year_i = " + ALedger[0].CurrentFinancialYear +
         *                                    " AND PUB_a_batch.a_batch_period_i = " + APeriodNumber +
         *                                    " AND PUB_a_batch.a_batch_status_c = 'Posted'" +
         *                                    " AND PUB_a_batch.a_ledger_number_i = PUB_a_transaction.a_ledger_number_i" +
         *                                    " AND PUB_a_batch.a_batch_number_i = PUB_a_transaction.a_batch_number_i";
         *
         *          DBAccess.GDBAccessObj.SelectDT(ATransaction, TransactionQuery, DBTransaction);
         *
         *          String CostCentreQuery = "SELECT * FROM a_cost_centre WHERE " +
         *                                   ACostCentreTable.GetLedgerNumberDBName() + " = " + ALedgerNumber +
         *                                   " AND " + ACostCentreTable.GetPostingCostCentreFlagDBName() + " = True" +
         *                                   " AND " + ACostCentreTable.GetCostCentreTypeDBName() + " LIKE '" + MFinanceConstants.FOREIGN_CC_TYPE + "'" +
         *                                   " ORDER BY " + ACostCentreTable.GetCostCentreCodeDBName();
         *
         *          DBAccess.GDBAccessObj.SelectDT(ACostCentre, CostCentreQuery, DBTransaction);
         *
         *          //Iterate through the cost centres
         *          foreach (ACostCentreRow CostCentreRow in ACostCentre.Rows)
         *          {
         *              bool TransactionExists = false;
         *
         *              //Iterate through the journals
         *              foreach (AJournalRow JournalRow in AJournal.Rows)
         *              {
         *                  int BatchNumber = JournalRow.BatchNumber;
         *                  int JournalNumber = JournalRow.JournalNumber;
         *                  if (TransactionExists)
         *                  {
         *                      //only need to run above code once for 1 transaction per cost centre code
         *                      break;     //goto next cost centre else try next journal
         *                  }
         *              }
         *          }
         *      }); // Get NewOrExisting AutoReadTransaction
         * } // Generate HOSA Reports
         */
    }
}