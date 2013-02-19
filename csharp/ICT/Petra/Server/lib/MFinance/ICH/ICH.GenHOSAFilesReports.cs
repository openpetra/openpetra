//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christophert, timop
//
// Copyright 2004-2013 by OM International
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
        /// <param name="ACurrency">Currency 0 = base 1 = intl</param>
        /// <param name="AFileName">File name</param>
        /// <param name="AVerificationResult">Error messaging</param>
        /// <returns>Successful or not</returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool GenerateHOSAFiles(int ALedgerNumber,
            int APeriodNumber,
            int AIchNumber,
            string ACostCentre,
            int ACurrency,
            string AFileName,
            out TVerificationResultCollection AVerificationResult
            )
        {
            bool Successful = false;

            string MonthName;
            //int NumDecPl;
            string StoreNumericFormat;
            string Currency;
            //bool FileChosen;
            //string Directory;
            //string FileName;
            //int Separator;
            string CurrencySelect;
            decimal DebitTotal;  //FORMAT "->>>,>>>,>>>,>>9.99"
            decimal CreditTotal;  //FORMAT "->>>,>>>,>>>,>>9.99"
            int Choice;

            string StandardCostCentre = ALedgerNumber.ToString() + "00";

            DataTable TableForExport = new DataTable();

            AVerificationResult = new TVerificationResultCollection();

            //Begin the transaction
            bool NewTransaction = false;

            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            try
            {
                GLBatchTDS MainDS = new GLBatchTDS();

                //Load tables needed: AccountingPeriod, Ledger, Account, Cost Centre, Transaction, Gift Batch, ICHStewardship
                ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, DBTransaction);

                /* Retrieve info on the ledger. */
                ALedgerRow LedgerRow = (ALedgerRow)MainDS.ALedger.Rows[0];

                if (ACurrency == 1)
                {
                    Currency = LedgerRow.BaseCurrency;
                    CurrencySelect = MFinanceConstants.CURRENCY_BASE;
                }
                else
                {
                    Currency = LedgerRow.IntlCurrency;
                    CurrencySelect = MFinanceConstants.CURRENCY_INTERNATIONAL;
                }

                //TODO: get number of decimal places for Currency and set format below
                StoreNumericFormat = "#" + CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + "##0";

                if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits > 0)
                {
                    string DecPls = new String('0', CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits);
                    StoreNumericFormat += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + DecPls;
                }

                //NumDecPl = 2;

                AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, APeriodNumber, DBTransaction);

                AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                /* Change expected number format if necessary - ensure ok to read in ICH */
                //StoreNumericFormat = "";
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(AccountingPeriodRow.PeriodEndDate.Month);

                //Create table definitions
                TableForExport.Columns.Add("CostCentre", typeof(string));
                TableForExport.Columns.Add("Account", typeof(string));
                TableForExport.Columns.Add("LedgerMonth", typeof(string));
                TableForExport.Columns.Add("ICHPeriod", typeof(string));
                TableForExport.Columns.Add("Date", typeof(DateTime));
                TableForExport.Columns.Add("IndividualDebitTotal", typeof(decimal));
                TableForExport.Columns.Add("IndividualCreditTotal", typeof(decimal));

                string TableForExportHeader = "/** Header **" + "," +
                                              APeriodNumber.ToString() + "," +
                                              StandardCostCentre + "," +
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
                            CurrencySelect,
                            AIchNumber,
                            ref TableForExport,
                            ref AVerificationResult);
                    }

                    /* Then see if there are any GL transactions to export */
                    //gi3200.i ln:33

                    /*
                     * This scheme with ODBC parameters consistently causes an "input string is the wrong type" eror:
                     * 
                    strSql = TDataBase.ReadSqlFile("ICH.HOSAExportGLTrans.sql");
                    OdbcParameter[] SqlParams = new OdbcParameter[] {
                            new OdbcParameter("LedgerNumber", (Int32)ALedgerNumber),
                            new OdbcParameter("Account", (String)gLMAcctCode),
                            new OdbcParameter("CostCentre", (String)gLMCostCCode),
                            new OdbcParameter("Narrative", (String)MFinanceConstants.NARRATIVE_YEAR_END_REALLOCATION),
                            new OdbcParameter("ICHNumber", (Int32)AIchNumber),
                            new OdbcParameter("ICHNumber2", (Int32)AIchNumber),
                            new OdbcParameter("PeriodNumber", (Int32)APeriodNumber)
                        };
                    DataTable TmpTransTable = DBAccess.GDBAccessObj.SelectDT(strSql, "Transactions", DBTransaction, SqlParams);
                    */

                    strSql = "SELECT Trans.a_ledger_number_i, Trans.a_batch_number_i, Trans.a_journal_number_i, Trans.a_transaction_number_i, "
                    + "Trans.a_account_code_c, Trans.a_cost_centre_code_c, Trans.a_transaction_date_d, Trans.a_transaction_amount_n, "
                    + "Trans.a_amount_in_base_currency_n, Trans.a_amount_in_intl_currency_n, Trans.a_ich_number_i, Trans.a_system_generated_l, "
                    + "Trans.a_narrative_c, Trans.a_debit_credit_indicator_l  FROM public.a_transaction AS Trans, public.a_journal AS Journal "
                    + "WHERE Trans.a_ledger_number_i = Journal.a_ledger_number_i AND Trans.a_batch_number_i = Journal.a_batch_number_i "
                    + "AND Trans.a_journal_number_i = Journal.a_journal_number_i "
                    + String.Format("AND Trans.a_ledger_number_i = {0} AND Trans.a_account_code_c = '{1}' AND Trans.a_cost_centre_code_c = '{2}' "
                        + "AND Trans.a_transaction_status_l = true AND NOT (Trans.a_narrative_c LIKE '{3}%' AND Trans.a_system_generated_l = true) "
                        + "AND ((Trans.a_ich_number_i + {4}) = Trans.a_ich_number_i OR Trans.a_ich_number_i = {4}) "
                        + "AND Journal.a_journal_period_i = {5};",
                            ALedgerNumber, gLMAcctCode, gLMCostCCode,
                            MFinanceConstants.NARRATIVE_YEAR_END_REALLOCATION,
                            AIchNumber, APeriodNumber);
                    DataTable TmpTransTable = DBAccess.GDBAccessObj.SelectDT(strSql, "Transactions", DBTransaction);

                    foreach (DataRow untypedTransactRow in TmpTransTable.Rows)
                    {
                        DebitTotal = 0;
                        CreditTotal = 0;
                        Choice = ACurrency;

                        bool Debit = Convert.ToBoolean(untypedTransactRow[13]);          //a_transaction.a_debit_credit_indicator_l
                        bool SystemGenerated = Convert.ToBoolean(untypedTransactRow[11]);          //a_transaction.a_system_generated_l
                        decimal AmountInBaseCurrency = Convert.ToDecimal(untypedTransactRow[8]);          //a_transaction.a_amount_in_base_currency_n
                        decimal AmountInIntlCurrency = Convert.ToDecimal(untypedTransactRow[9]);          //a_transaction.a_amount_in_intl_currency_n
                        string Narrative = untypedTransactRow[12].ToString();          //a_transaction.a_narrative_c
                        DateTime TransactionDate = Convert.ToDateTime(untypedTransactRow[6]);          //a_transaction.a_transaction_date_d

                        // the following variables are not actually used anywhere at the moment
                        // string TransAmount1;  //FORMAT "X(19)"
                        // string TransAmount2;  //FORMAT "X(19)"

                        if (Choice == 1)
                        {
                            /* find transaction amount and store as debit or credit */
                            if (Debit)
                            {
                                DebitTotal += AmountInBaseCurrency;
                                // TransAmount1 = AmountInBaseCurrency.ToString("#,##0.00");
                                // TransAmount2 = " ";
                            }
                            else
                            {
                                CreditTotal += AmountInBaseCurrency;
                                // TransAmount2 = AmountInBaseCurrency.ToString("#,##0.00");
                                // TransAmount1 = " ";
                            }
                        }
                        else
                        {
                            if (Debit)
                            {
                                DebitTotal += AmountInIntlCurrency;
                                // TransAmount1 = AmountInIntlCurrency.ToString("#,##0.00");
                                // TransAmount2 = " ";
                            }
                            else
                            {
                                CreditTotal += AmountInIntlCurrency;
                                // TransAmount2 = AmountInIntlCurrency.ToString("#,##0.00");
                                // TransAmount1 = " ";
                            }
                        }

                        Choice = 0;

                        if ((gLMAcctType != MFinanceConstants.ACCOUNT_TYPE_INCOME.ToUpper())
                            || !(SystemGenerated && ((Narrative.Substring(0, 27) == MFinanceConstants.NARRATIVE_GIFTS_RECEIVED_GIFT_BATCH)
                                                     || (Narrative.Substring(0, 15) == MFinanceConstants.NARRATIVE_GB_GIFT_BATCH)))
                            )
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

                //DataTables to XML to CSV
                XmlDocument doc = TDataBase.DataTableToXml(TableForExport);

                TCsv2Xml.Xml2Csv(doc, AFileName);

                //Replace the default CSV header row with OM specific
                ReplaceHeaderInFile(AFileName, TableForExportHeader, ref AVerificationResult);

                /* Change number format back */
                //TODO

                Successful = true;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }

            // rollback the reading transaction
            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return Successful;
        }

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
        /// <param name="ABase"></param>
        /// <param name="AIchNumber"></param>
        /// <param name="AExportDataTable"></param>
        /// <param name="AVerificationResult"></param>
        [RequireModulePermission("FINANCE-3")]
        public static void ExportGifts(int ALedgerNumber,
            string ACostCentre,
            string AAcctCode,
            string AMonthName,
            int APeriodNumber,
            DateTime APeriodStartDate,
            DateTime APeriodEndDate,
            string ABase,
            int AIchNumber,
            ref DataTable AExportDataTable,
            ref TVerificationResultCollection AVerificationResult)
        {
            /* Define local variables */
            bool FirstLoopFlag = true;
            Int32 LastRecipKey = 0;             //FORMAT "9999999999"
            string LastGroup = string.Empty;
            string LastDetail = string.Empty;
            string LastDetailDesc = string.Empty; //FORMAT "X(15)"
            string Desc = string.Empty; //FORMAT "X(44)"
            // string CurrentYearTotals = string.Empty;
            decimal IndividualDebitTotal = 0; //FORMAT "->>>,>>>,>>>,>>9.99"
            decimal IndividualCreditTotal = 0; //FORMAT "->>>,>>>,>>>,>>9.99"

            decimal GiftAmount = 0;
            decimal IntlGiftAmount = 0;

            string ExportDescription = string.Empty;
            string tmpLastGroup = string.Empty;
            string tmpLastDetail = string.Empty;

            //Export Gifts gi3200-1.i
            //Find and total each gift transaction
            string SQLStmt = TDataBase.ReadSqlFile("ICH.HOSAExportGiftsInner.sql");

            //Begin the transaction
            bool NewTransaction = false;

            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

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

                Int32 tmpLastRecipKey = Convert.ToInt32(untypedTransRow[8]);         //a_gift_detail.p_recipient_key_n
                tmpLastGroup = untypedTransRow[6].ToString();         //a_motivation_detail.a_motivation_group_code_c
                tmpLastDetail = untypedTransRow[7].ToString();         //a_motivation_detail.a_motivation_detail_code_c

                if ((FirstLoopFlag == false)
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

                            Desc = MotivationGroupRow.MotivationGroupDescription.TrimEnd(new Char[] { (' ') }) + "," + LastDetailDesc;

                            ExportDescription = ALedgerNumber.ToString() + AMonthName + ":" + Desc;
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

                GiftAmount = Convert.ToDecimal(untypedTransRow[4]);          //a_gift_detail.a_gift_amount_n
                IntlGiftAmount = Convert.ToDecimal(untypedTransRow[5]);          //a_gift_detail.a_gift_amount_intl_n

                if (ABase == MFinanceConstants.CURRENCY_BASE)
                {
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
            }

            /* Print totals etc. found for last recipient */
            /* Only do after first loop due to last recipient key check */
            if (FirstLoopFlag == false)
            {
                if ((IndividualCreditTotal != 0)
                    || (IndividualDebitTotal != 0))
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
                        AMotivationGroupTable MotivationGroupTable = AMotivationGroupAccess.LoadByPrimaryKey(ALedgerNumber, LastGroup, DBTransaction);
                        AMotivationGroupRow MotivationGroupRow = (AMotivationGroupRow)MotivationGroupTable.Rows[0];

                        Desc = MotivationGroupRow.MotivationGroupDescription.TrimEnd() + "," + LastDetailDesc;

                        ExportDescription = ALedgerNumber.ToString() + AMonthName + ":" + Desc;
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
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }

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

        /// <summary>
        /// Performs the ICH code to generate HOSA Files/Reports.
        ///  Relates to gl2120-1.i
        /// </summary>
        /// <param name="ALedgerNumber">ICH Ledger number</param>
        /// <param name="APeriodNumber">Period number</param>
        /// <param name="AIchNumber">ICH number</param>
        /// <param name="ACurrency">Currency</param>
        /// <param name="AVerificationResult">Error messaging</param>
        [RequireModulePermission("FINANCE-3")]
        public static void GenerateHOSAReports(int ALedgerNumber,
            int APeriodNumber,
            int AIchNumber,
            string ACurrency,
            out TVerificationResultCollection AVerificationResult
            )
        {
            AVerificationResult = new TVerificationResultCollection();

            //Begin the transaction
            bool NewTransaction = false;

            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            GLPostingTDS PostingDS = new GLPostingTDS();
            GLBatchTDS BatchDS = new GLBatchTDS();

            //Load tables needed: AccountingPeriod, Ledger, Account, Cost Centre, Transaction, Gift Batch, ICHStewardship
            ALedgerAccess.LoadByPrimaryKey(PostingDS, ALedgerNumber, DBTransaction);
            //AAccountAccess.LoadViaALedger(PostingDS, ALedgerNumber, DBTransaction);
            ACostCentreAccess.LoadViaALedger(PostingDS, ALedgerNumber, DBTransaction);
            ATransactionAccess.LoadViaALedger(BatchDS, ALedgerNumber, DBTransaction);
            //AIchStewardshipAccess.LoadViaALedger(PostingDS, ALedgerNumber, DBTransaction);
            //AAccountHierarchyAccess.LoadViaALedger(PostingDS, ALedgerNumber, DBTransaction);
            AJournalAccess.LoadViaALedger(BatchDS, ALedgerNumber, DBTransaction);


            try
            {
#if TODO
                ALedgerRow LedgerRow = (ALedgerRow)PostingDS.ALedger.Rows[0];

                //Find the Ledger Name = Partner Short Name
                PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(LedgerRow.PartnerKey, DBTransaction);
                PPartnerRow PartnerRow = (PPartnerRow)PartnerTable.Rows[0];

                string LedgerName = PartnerRow.PartnerShortName;
#endif

                //Iterate through the cost centres
                string WhereClause = ACostCentreTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString() +
                                     " AND " + ACostCentreTable.GetPostingCostCentreFlagDBName() + " = True" +
                                     " AND " + ACostCentreTable.GetCostCentreTypeDBName() +
                                     " LIKE '" + MFinanceConstants.FOREIGN_CC_TYPE + "'";

                string OrderBy = ACostCentreTable.GetCostCentreCodeDBName();

                DataRow[] FoundCCRows = PostingDS.ACostCentre.Select(WhereClause, OrderBy);

                //AIchStewardshipTable IchStewardshipTable = new AIchStewardshipTable();

                foreach (DataRow untypedCCRow in FoundCCRows)
                {
                    ACostCentreRow CostCentreRow = (ACostCentreRow)untypedCCRow;

                    string CostCentreCode = CostCentreRow.CostCentreCode;

                    bool TransactionExists = false;

                    //Iterate through the journals
                    WhereClause = AJournalTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString() +
                                  " AND " + AJournalTable.GetJournalPeriodDBName() + " = " + APeriodNumber.ToString();

                    DataRow[] FoundJnlRows = BatchDS.AJournal.Select(WhereClause);

                    foreach (DataRow untypedJnlRow in FoundJnlRows)
                    {
                        AJournalRow JournalRow = (AJournalRow)untypedJnlRow;

                        int BatchNumber = JournalRow.BatchNumber;
                        int JournalNumber = JournalRow.JournalNumber;

                        WhereClause = ATransactionTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString() +
                                      " AND " + ATransactionTable.GetBatchNumberDBName() + " = " + BatchNumber.ToString() +
                                      " AND " + ATransactionTable.GetJournalNumberDBName() + " = " + JournalNumber.ToString() +
                                      " AND " + ATransactionTable.GetCostCentreCodeDBName() + " = '" + CostCentreCode + "'" +
                                      " AND (" + ATransactionTable.GetIchNumberDBName() + " = 0" +
                                      "      OR " + ATransactionTable.GetIchNumberDBName() + " = " + AIchNumber.ToString() +
                                      ")";

                        DataRow[] FoundTransRows = BatchDS.ATransaction.Select(WhereClause);

                        foreach (DataRow untypedTransRow in FoundTransRows)
                        {
#if TODO
                            ATransactionRow TransactionRow = (ATransactionRow)untypedTransRow;

                            TransactionExists = true;

                            string DefaultData = ALedgerNumber.ToString() + "," +
                                                 LedgerName + "," +
                                                 APeriodNumber.ToString() + "," +
                                                 APeriodNumber.ToString() + "," +
                                                 CostCentreCode + "," +
                                                 "" + "," +
                                                 "" + "," +
                                                 "" + "," +
                                                 "A" + "," +
                                                 LedgerRow.CurrentFinancialYear.ToString() + "," +
                                                 LedgerRow.CurrentPeriod.ToString() + "," +
                                                 MFinanceConstants.MAX_PERIODS.ToString() + "," +
                                                 "h" + "," +
                                                 ACurrency + "," +
                                                 AIchNumber.ToString();

                            string ReportTitle = "Home Office Stmt of Acct: " + CostCentreRow.CostCentreName;

                            //call code for gl2120p.p Produces Account Detail, Analysis Attribute and HOSA Reprint reports.

                            /* RUN sm9000.w ("gl2120p.p",
                             *                               lv_report_title_c,
                             *                               lv_default_data_c).*/
                            //TODO: call code to produce reports
#endif
                            break;
                        }

                        if (TransactionExists)
                        {
                            //only need to run above code once for 1 transaction per cost centre code
                            break;     //goto next cost centre else try next journal
                        }
                    }
                }

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            catch (Exception Exp)
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                TLogging.Log(Exp.Message);
                TLogging.Log(Exp.StackTrace);

                throw;
            }
        }
    }
}