//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christophert, timop
//
// Copyright 2004-2012 by OM International
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
    /// Class for the generation of "Home Office Statement of Accounts" reports for each
    ///   foreign cost centre (ledger/fund).  This is basically a modified Trial Balance.
    /// </summary>
    public class TGenHOSAFilesReports
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
            string TransAmount1;  //FORMAT "X(19)"
            string TransAmount2;  //FORMAT "X(19)"
            int Choice;

            string StandardCostCentre = ALedgerNumber.ToString() + "00";

            DataTable TableForExport = new DataTable();

            AVerificationResult = new TVerificationResultCollection();

            //Begin the transaction
            bool NewTransaction = false;

            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            try
            {
                GLBatchTDS MainDS = new GLBatchTDS();

                //Load tables needed: AccountingPeriod, Ledger, Account, Cost Centre, Transaction, Gift Batch, ICHStewardship
                ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, DBTransaction);
                //AAccountAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);
                ACostCentreAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);
                ATransactionAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);
                //AIchStewardshipAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);
                //AAccountHierarchyAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);
                AJournalAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);

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
                StoreNumericFormat = string.Empty;
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
                string strSql = "SELECT " +
                                "a_general_ledger_master.a_glm_sequence_i, " +
                                "a_general_ledger_master.a_ledger_number_i, " +
                                "a_general_ledger_master.a_year_i, " +
                                "a_general_ledger_master.a_account_code_c, " +
                                "a_general_ledger_master.a_cost_centre_code_c, " +
                                "a_account.a_account_type_c " +
                                "FROM  " +
                                "public.a_general_ledger_master, " +
                                "public.a_cost_centre, " +
                                "public.a_account " +
                                "WHERE " +
                                "a_general_ledger_master.a_ledger_number_i = a_cost_centre.a_ledger_number_i AND " +
                                "a_general_ledger_master.a_cost_centre_code_c = a_cost_centre.a_cost_centre_code_c AND " +
                                "a_general_ledger_master.a_ledger_number_i = a_account.a_ledger_number_i AND " +
                                "a_general_ledger_master.a_account_code_c = a_account.a_account_code_c AND " +
                                "a_account.a_posting_status_l = True AND " +
                                "a_cost_centre.a_posting_cost_centre_flag_l = True AND " +
                                "a_general_ledger_master.a_ledger_number_i = ? AND " +
                                "a_general_ledger_master.a_year_i = ? AND " +
                                "a_general_ledger_master.a_cost_centre_code_c = ? " +
                                "ORDER BY " +
                                "a_general_ledger_master.a_account_code_c ASC;";

                DataTable TmpTable = DBAccess.GDBAccessObj.SelectDT(strSql, "table", DBTransaction,
                    new OdbcParameter[] {
                        new OdbcParameter("LedgerNumber", (object)ALedgerNumber),
                        new OdbcParameter("Year", (object)LedgerRow.CurrentFinancialYear),
                        new OdbcParameter("CostCentre", ACostCentre)
                    });

                foreach (DataRow untypedTransRow in TmpTable.Rows)
                {
                    //string gLMsEQ = untypedTransRow[0].ToString();
                    //string gLMAcctCode = untypedTransRow[1].ToString();
                    //string gLMAcctCode = untypedTransRow[2].ToString();
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
                            ref DBTransaction,
                            ref AVerificationResult);
                    }

                    /* Then see if there are any GL transactions to export */
                    //gi3200.i
                    strSql = "SELECT " +
                             "  a_transaction.a_ledger_number_i, " +
                             "  a_transaction.a_batch_number_i, " +
                             "  a_transaction.a_journal_number_i, " +
                             "  a_transaction.a_transaction_number_i, " +
                             "  a_transaction.a_account_code_c, " +
                             "  a_transaction.a_cost_centre_code_c, " +
                             "  a_transaction.a_transaction_date_d, " +
                             "  a_transaction.a_transaction_amount_n, " +
                             "  a_transaction.a_amount_in_base_currency_n, " +
                             "  a_transaction.a_amount_in_intl_currency_n, " +
                             "  a_transaction.a_ich_number_i, " +
                             "  a_transaction.a_system_generated_l, " +
                             "  a_transaction.a_narrative_c " +
                             "  a_transaction.a_debit_credit_indicator_l " +
                             "FROM " +
                             "  public.a_transaction, " +
                             "  public.a_journal " +
                             "WHERE " +
                             "  a_transaction.a_ledger_number_i = a_journal.a_ledger_number_i AND " +
                             "  a_transaction.a_batch_number_i = a_journal.a_batch_number_i AND " +
                             "  a_transaction.a_journal_number_i = a_journal.a_journal_number_i AND " +
                             "  a_transaction.a_ledger_number_i = ? AND " +
                             "  a_transaction.a_account_code_c = ? AND " +
                             "  a_transaction.a_cost_centre_code_c = ? AND " +
                             "  a_transaction.a_transaction_status_l = true AND " +
                             "  NOT (LEFT(a_transaction.a_narrative_c, 22) = ? AND " +
                             "       a_transaction.a_system_generated_l = true) AND " +
                             "  ((a_transaction.a_ich_number_i + ?) = a_transaction.a_ich_number_i OR " +
                             "   a_transaction.a_ich_number_i = ?) AND " +
                             "  a_journal.a_journal_period_i = ?";

                    DataTable TmpTransTable = DBAccess.GDBAccessObj.SelectDT(strSql, "Transactions", DBTransaction,
                        new OdbcParameter[] {
                            new OdbcParameter("LedgerNumber", (object)ALedgerNumber),
                            new OdbcParameter("Account", (object)gLMAcctCode),
                            new OdbcParameter("CostCentre", gLMCostCCode),
                            new OdbcParameter("Narrative", (object)MFinanceConstants.NARRATIVE_YEAR_END_REALLOCATION),
                            new OdbcParameter("ICHNumber", (object)AIchNumber),
                            new OdbcParameter("ICHNumber2", (object)AIchNumber),
                            new OdbcParameter("PeriodNumber", (object)APeriodNumber)
                        });

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

                        if (Choice == 1)
                        {
                            /* find transaction amount and store as debit or credit */
                            if (Debit)
                            {
                                DebitTotal += AmountInBaseCurrency;
                                TransAmount1 = AmountInBaseCurrency.ToString("#,##0.00");
                                TransAmount2 = " ";
                            }
                            else
                            {
                                CreditTotal += AmountInBaseCurrency;
                                TransAmount2 = AmountInBaseCurrency.ToString("#,##0.00");
                                TransAmount1 = " ";
                            }
                        }
                        else
                        {
                            if (Debit)
                            {
                                DebitTotal += AmountInIntlCurrency;
                                TransAmount1 = AmountInIntlCurrency.ToString("#,##0.00");
                                TransAmount2 = " ";
                            }
                            else
                            {
                                CreditTotal += AmountInIntlCurrency;
                                TransAmount2 = AmountInIntlCurrency.ToString("#,##0.00");
                                TransAmount1 = " ";
                            }
                        }

                        Choice = 0;

                        if ((gLMAcctType != MFinanceConstants.ACCOUNT_TYPE_INCOME.ToUpper())
                            || !(SystemGenerated && ((Narrative.Substring(0, 27) == MFinanceConstants.NARRATIVE_GIFTS_RECEIVED_GIFT_BATCH)
                                                     || (Narrative.Substring(0, 15) == MFinanceConstants.NARRATIVE_GB_GIFT_BATCH)))
                            )
                        {
                            /* Put transaction information
                             *              //TODO
                             *              EXPORT DELIMITER ","
                             *               a_general_ledger_master.a_cost_centre_code_c
                             *               ConvertAccount(a_general_ledger_master.a_account_code_c)
                             *               STRING(pv_ledger_number_i) + cMonthName + ":" + {&TRANSACTION-TABLE-NAME}.a_narrative_c
                             *               "ICH-":U + STRING(pv_period_number_i, "99")    // {&TRANSACTION-TABLE-NAME}.a_reference_c
                             *               {&TRANSACTION-TABLE-NAME}.a_transaction_date_d
                             *               lv_debit_total_n
                             *               lv_credit_total_n*/
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
                ReplaceHeaderInFile(AFileName, TableForExportHeader);

                /* Change number format back */
                //TODO

                Successful = true;
            }
            catch (Exception)
            {
            }

            return Successful;
        }

        /// <summary>
        /// Replaces the first line of a text file with a specified string
        /// </summary>
        /// <param name="AFileName">File name (including path) to process</param>
        /// <param name="AHeaderText">Text to insert in first line</param>
        public static void ReplaceHeaderInFile(string AFileName, string AHeaderText)
        {
            StringBuilder newFile = new StringBuilder();

            string[] file = File.ReadAllLines(AFileName);

            bool IsFirstLine = true;

            foreach (string line in file)
            {
                //If first line
                if (IsFirstLine)
                {
                    newFile.Append(AHeaderText + "\r\n");
                    IsFirstLine = false;
                }
                else
                {
                    newFile.Append(line + "\r\n");
                }
            }

            File.WriteAllText(AFileName, newFile.ToString());
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
        /// <param name="ADBTransaction"></param>
        /// <param name="AVerificationResult"></param>
        private static void ExportGifts(int ALedgerNumber,
            string ACostCentre,
            string AAcctCode,
            string AMonthName,
            int APeriodNumber,
            DateTime APeriodStartDate,
            DateTime APeriodEndDate,
            string ABase,
            int AIchNumber,
            ref DataTable AExportDataTable,
            ref TDBTransaction ADBTransaction,
            ref TVerificationResultCollection AVerificationResult)
        {
            /* Define local variables */
            bool FirstLoopFlag = true;
            Int32 LastRecipKey = 0;             //FORMAT "9999999999"
            string LastGroup = string.Empty;
            string LastDetail = string.Empty;
            string LastDetailDesc = string.Empty; //FORMAT "X(15)"
            string Desc = string.Empty; //FORMAT "X(44)"
            string CurrentYearTotals = string.Empty;
            decimal IndividualDebitTotal = 0; //FORMAT "->>>,>>>,>>>,>>9.99"
            decimal IndividualCreditTotal = 0; //FORMAT "->>>,>>>,>>>,>>9.99"

            string ExportDescription = string.Empty;

            string SQLStmt = "SELECT " +
                             "a_gift_detail.a_ledger_number_i, " +
                             "a_gift_detail.a_batch_number_i, " +
                             "a_gift_detail.a_gift_transaction_number_i, " +
                             "a_gift_detail.a_detail_number_i, " +
                             "a_gift_detail.a_gift_amount_n, " +
                             "a_gift_detail.a_gift_amount_intl_n, " +
                             "a_gift_detail.a_motivation_group_code_c, " +
                             "a_gift_detail.a_motivation_detail_code_c, " +
                             "a_gift_detail.p_recipient_key_n, " +
                             "a_gift.a_gift_status_c, " +
                             "a_motivation_detail.a_motivation_detail_desc_c, " +
                             "a_gift_batch.a_batch_description_c " +
                             "FROM " +
                             "public.a_gift_detail, " +
                             "public.a_gift_batch, " +
                             "public.a_motivation_detail, " +
                             "public.a_gift " +
                             "WHERE " +
                             "a_gift_detail.a_ledger_number_i = a_gift_batch.a_ledger_number_i AND " +
                             "a_gift_detail.a_batch_number_i = a_gift_batch.a_batch_number_i AND " +
                             "a_gift_detail.a_ledger_number_i = a_motivation_detail.a_ledger_number_i AND " +
                             "a_gift_detail.a_motivation_group_code_c = a_motivation_detail.a_motivation_group_code_c AND " +
                             "a_gift_detail.a_motivation_detail_code_c = a_motivation_detail.a_motivation_detail_code_c AND " +
                             "a_gift_detail.a_ledger_number_i = a_gift.a_ledger_number_i AND " +
                             "a_gift_detail.a_batch_number_i = a_gift.a_batch_number_i AND " +
                             "a_gift_detail.a_gift_transaction_number_i = a_gift.a_gift_transaction_number_i AND " +
                             "a_gift_detail.a_ledger_number_i = ? AND " +
                             "a_gift_detail.a_cost_centre_code_c = ? AND " +
                             "a_gift_detail.a_ich_number_i = ? AND " +
                             "a_gift_batch.a_batch_status_c = ? AND " +
                             "a_gift_batch.a_gl_effective_date_d >= ? AND " +
                             "a_gift_batch.a_gl_effective_date_d <= ? AND " +
                             "a_motivation_detail.a_account_code_c = ? " +
                             "ORDER BY " +
                             "a_gift_detail.p_recipient_key_n ASC, " +
                             "a_gift_detail.a_motivation_group_code_c ASC, " +
                             "a_gift_detail.a_motivation_detail_code_c ASC;";

            DataTable TmpTable = DBAccess.GDBAccessObj.SelectDT(SQLStmt, "table", ADBTransaction,
                new OdbcParameter[] {
                    new OdbcParameter("LedgerNumber", (object)ALedgerNumber),
                    new OdbcParameter("CostCentre", ACostCentre),
                    new OdbcParameter("ICHNumber", (object)AIchNumber),
                    new OdbcParameter("BatchStatus", MFinanceConstants.BATCH_POSTED),
                    new OdbcParameter("StartDate", APeriodStartDate),
                    new OdbcParameter("EndDate", APeriodEndDate),
                    new OdbcParameter("AccountCode", AAcctCode)
                });

            foreach (DataRow untypedTransRow in TmpTable.Rows)
            {
                /* Print totals etc. found for last recipient */
                /* Only do after first loop due to last recipient key check */

                Int32 tmpLastRecipKey = Convert.ToInt32(untypedTransRow[8]);         //a_gift_detail.p_recipient_key_n
                string tmpLastGroup = untypedTransRow[6].ToString();         //a_motivation_detail.a_motivation_group_code_c
                string tmpLastDetail = untypedTransRow[7].ToString();         //a_motivation_detail.a_motivation_detail_code_c

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
                            PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(LastRecipKey, ADBTransaction);
                            PPartnerRow PartnerRow = (PPartnerRow)PartnerTable.Rows[0];

                            LastDetailDesc += " : " + PartnerRow.PartnerShortName;

                            ExportDescription = ALedgerNumber.ToString() + AMonthName + ":" + LastDetailDesc;
                        }
                        else
                        {
                            AMotivationGroupTable MotivationGroupTable = AMotivationGroupAccess.LoadByPrimaryKey(ALedgerNumber,
                                LastGroup,
                                ADBTransaction);
                            AMotivationGroupRow MotivationGroupRow = (AMotivationGroupRow)MotivationGroupTable.Rows[0];

                            Desc = MotivationGroupRow.MotivationGroupDescription.TrimEnd(new Char[] { (' ') }) + "," + LastDetailDesc;

                            ExportDescription = ALedgerNumber.ToString() + AMonthName + ":" + Desc;
                        }

                        //TODO: do export and use ExportDescription

                        /*EXPORT DELIMITER ","
                         * lv_cost_centre_c
                         * ConvertAccount(lv_account_c)
                         * STRING(pv_ledger_number_i) + cMonthName + ":" + ExportDescription
                         * "ICH-":U + STRING(pv_period_number_i, "99")
                         * lv_end_d
                         * lv_individual_debit_total_n
                         * lv_individual_credit_total_n*/

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

                decimal GiftAmount = Convert.ToDecimal(untypedTransRow[4]);          //a_gift_detail.a_gift_amount_n
                decimal IntlGiftAmount = Convert.ToDecimal(untypedTransRow[5]);          //a_gift_detail.a_gift_amount_intl_n

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
                        PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(LastRecipKey, ADBTransaction);
                        PPartnerRow PartnerRow = (PPartnerRow)PartnerTable.Rows[0];

                        LastDetailDesc += ":" + PartnerRow.PartnerShortName;

                        ExportDescription = ALedgerNumber.ToString() + AMonthName + ":" + LastDetailDesc;
                    }
                    else
                    {
                        AMotivationGroupTable MotivationGroupTable = AMotivationGroupAccess.LoadByPrimaryKey(ALedgerNumber, LastGroup, ADBTransaction);
                        AMotivationGroupRow MotivationGroupRow = (AMotivationGroupRow)MotivationGroupTable.Rows[0];

                        Desc = MotivationGroupRow.MotivationGroupDescription.TrimEnd() + "," + LastDetailDesc;

                        ExportDescription = ALedgerNumber.ToString() + AMonthName + ":" + Desc;
                    }

                    //TODO:

                    /*EXPORT DELIMITER ","
                     * lv_cost_centre_c
                     * ConvertAccount(lv_account_c)
                     * STRING(pv_ledger_number_i) + cMonthName + ":" + ExportDescription
                     * "ICH-":U + STRING(pv_period_number_i, "99")
                     * lv_end_d
                     * lv_individual_debit_total_n
                     * lv_individual_credit_total_n */
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

            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            GLBatchTDS MainDS = new GLBatchTDS();

            //Load tables needed: AccountingPeriod, Ledger, Account, Cost Centre, Transaction, Gift Batch, ICHStewardship
            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, DBTransaction);
            //AAccountAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);
            ACostCentreAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);
            ATransactionAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);
            //AIchStewardshipAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);
            //AAccountHierarchyAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);
            AJournalAccess.LoadViaALedger(MainDS, ALedgerNumber, DBTransaction);


            try
            {
                ALedgerRow LedgerRow = (ALedgerRow)MainDS.ALedger.Rows[0];

                //Find the Ledger Name = Partner Short Name
                PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(LedgerRow.PartnerKey, DBTransaction);
                PPartnerRow PartnerRow = (PPartnerRow)PartnerTable.Rows[0];

                string LedgerName = PartnerRow.PartnerShortName;

                //Iterate through the cost centres
                string WhereClause = ACostCentreTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString() +
                                     " AND " + ACostCentreTable.GetPostingCostCentreFlagDBName() + " = True" +
                                     " AND " + ACostCentreTable.GetCostCentreTypeDBName() +
                                     " LIKE '" + MFinanceConstants.FOREIGN_CC_TYPE + "'";

                string OrderBy = ACostCentreTable.GetCostCentreCodeDBName();

                DataRow[] FoundCCRows = MainDS.ACostCentre.Select(WhereClause, OrderBy);

                //AIchStewardshipTable IchStewardshipTable = new AIchStewardshipTable();

                foreach (DataRow untypedCCRow in FoundCCRows)
                {
                    ACostCentreRow CostCentreRow = (ACostCentreRow)untypedCCRow;

                    string CostCentreCode = CostCentreRow.CostCentreCode;

                    bool TransactionExists = false;

                    //Iterate through the journals
                    WhereClause = AJournalTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString() +
                                  " AND " + AJournalTable.GetJournalPeriodDBName() + " = " + APeriodNumber.ToString();

                    DataRow[] FoundJnlRows = MainDS.AJournal.Select(WhereClause);

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
                                      "      OR " + ATransactionTable.GetIchNumberDBName() + AIchNumber.ToString() +
                                      ")";

                        DataRow[] FoundTransRows = MainDS.ATransaction.Select(WhereClause);

                        foreach (DataRow untypedTransRow in FoundTransRows)
                        {
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

                throw Exp;
            }
        }
    }
}