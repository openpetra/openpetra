//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//
// Copyright 2004-2015 by OM International
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
using System.Collections.Generic;
using System.Data;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.Setup.WebConnectors;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MSysMan.Common.WebConnectors;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Server.MFinance.Reporting.WebConnectors
{
    ///<summary>
    /// This WebConnector provides data for the finance reporting screens
    ///</summary>
    public partial class TFinanceReportingWebConnector
    {
        /// <summary>
        /// get the details of the given ledger
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static void GetLedgerPeriodDetails(
            int ALedgerNumber,
            out int ANumberAccountingPeriods,
            out int ANumberForwardingPeriods,
            out int ACurrentPeriod,
            out int ACurrentYear)
        {
            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            ALedgerTable CachedDataTable = (ALedgerTable)CachePopulator.GetCacheableTable(
                TCacheableFinanceTablesEnum.LedgerDetails,
                "",
                false,
                ALedgerNumber,
                out typeofTable);

            if (CachedDataTable.Rows.Count > 0)
            {
                ANumberAccountingPeriods = CachedDataTable[0].NumberOfAccountingPeriods;
                ANumberForwardingPeriods = CachedDataTable[0].NumberFwdPostingPeriods;
                ACurrentPeriod = CachedDataTable[0].CurrentPeriod;
                ACurrentYear = CachedDataTable[0].CurrentFinancialYear;
            }
            else
            {
                ANumberAccountingPeriods = -1;
                ANumberForwardingPeriods = -1;
                ACurrentPeriod = -1;
                ACurrentYear = -1;
            }
        }

        /// <summary>
        /// Loads all available financial years into a table
        /// To be used by a combobox to select the financial year
        ///
        /// </summary>
        /// <returns>DataTable</returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable GetAvailableFinancialYears(int ALedgerNumber,
            System.Int32 ADiffPeriod,
            out String ADisplayMember,
            out String AValueMember)
        {
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector.GetAvailableGLYears(
                ALedgerNumber,
                ADiffPeriod,
                false,
                out ADisplayMember,
                out AValueMember);
        }

        /// <summary>
        /// Load all the receiving fields
        /// </summary>
        /// <returns>Table with the field keys and the field names</returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable GetReceivingFields(int ALedgerNumber,
            out String ADisplayMember,
            out String AValueMember,
            Boolean ASeparateDBConnection = false)
        {
            DataTable ReturnTable = null;
            TReportingDbAdapter DbAdapter = new TReportingDbAdapter(ASeparateDBConnection);
            TDBTransaction ReadTransaction = null;

            ADisplayMember = "FieldName";
            AValueMember = "FieldKey";

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    /*
                     * This SQL retrieves all partners that have "LEDGER" or "COSTCENTRE" type
                     * but this doesn't seem to be appropriate.
                     * The new SQL retrieves all units with u_unit_type_code_c is "A", "D", or "F" (Area, Special Fund, or Field).
                     *                                                        Tim Ingham, April 2015
                     *
                     * Actually, the original SQL is right so I've reverted it. Confirmed by OM Swiss and Rob. July 2015
                     */

                    String sql = "SELECT DISTINCT p_partner.p_partner_key_n, " +
                                 "p_partner.p_partner_short_name_c AS FieldName" +
                                 " FROM p_partner, p_partner_type" +
                                 " WHERE p_partner_type.p_partner_key_n = p_partner.p_partner_key_n " +
                                 " AND p_partner_type.p_type_code_c IN ('LEDGER','COSTCENTRE')" +
                                 " ORDER BY p_partner.p_partner_short_name_c";

                    ReturnTable = DbAdapter.RunQuery(sql, "receivingFields", ReadTransaction);
                });

            if (ReturnTable != null)
            {
                ReturnTable.Columns.Add("FieldKey", typeof(String));

                foreach (DataRow Row in ReturnTable.Rows)
                {
                    Int64 FieldKey;

                    if (Int64.TryParse(Row["p_partner_key_n"].ToString(), out FieldKey))
                    {
                        Row["FieldKey"] = FieldKey.ToString("D10");
                    }
                }
            }

            DbAdapter.CloseConnection();
            return (DbAdapter.IsCancelled) ? null : ReturnTable;
        } // Get Receiving Fields

        private static void GetReportingCostCentres(ACostCentreTable ACostCentres, List <string>AResult, string ASummaryCostCentreCode)
        {
            if (ASummaryCostCentreCode.Length == 0)
            {
                return;
            }

            string[] CostCentres = ASummaryCostCentreCode.Split(new char[] { ',' });

            foreach (string costcentre in CostCentres)
            {
                DataRowView[] ReportingCostCentres = ACostCentres.DefaultView.FindRows(costcentre);

                if (ReportingCostCentres.Length > 0)
                {
                    foreach (DataRowView rv in ReportingCostCentres)
                    {
                        ACostCentreRow row = (ACostCentreRow)rv.Row;

                        if (row.PostingCostCentreFlag)
                        {
                            AResult.Add(row.CostCentreCode);
                        }
                        else
                        {
                            GetReportingCostCentres(ACostCentres, AResult, row.CostCentreCode);
                        }
                    }
                }
                else
                {
                    DataView dv = new DataView(ACostCentres);
                    dv.Sort = ACostCentreTable.GetCostCentreCodeDBName();
                    ACostCentreRow cc = (ACostCentreRow)dv.FindRows(costcentre)[0].Row;

                    if (cc.PostingCostCentreFlag)
                    {
                        AResult.Add(costcentre);
                    }
                }
            }
        }

        /// <summary>
        /// Get all cost centres that report into the given summary cost centre
        /// </summary>
        /// <returns>a CSV list of the reporting cost centres</returns>
        [RequireModulePermission("FINANCE-1")]
        public static string GetReportingCostCentres(int ALedgerNumber, String ASummaryCostCentreCode, string ARemoveCostCentresFromList)
        {
            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            ACostCentreTable CachedDataTable = (ACostCentreTable)CachePopulator.GetCacheableTable(
                TCacheableFinanceTablesEnum.CostCentreList,
                "",
                false,
                ALedgerNumber,
                out typeofTable);

            CachedDataTable.DefaultView.Sort = ACostCentreTable.GetCostCentreToReportToDBName();

            List <string>Result = new List <string>();

            GetReportingCostCentres(CachedDataTable, Result, ASummaryCostCentreCode);

            List <string>IgnoreCostCentres = new List <string>();

            GetReportingCostCentres(CachedDataTable, IgnoreCostCentres, ARemoveCostCentresFromList);

            foreach (string s in IgnoreCostCentres)
            {
                if (Result.Contains(s))
                {
                    Result.Remove(s);
                }
            }

            return StringHelper.StrMerge(Result.ToArray(), ',');
        }

        private static void GetReportingAccounts(AAccountHierarchyDetailTable AAccountHierarchyDetail,
            List <string>AResult,
            string ASummaryAccountCodes,
            string AAccountHierarchy)
        {
            string[] Accounts = ASummaryAccountCodes.Split(new char[] { ',' });

            foreach (string account in Accounts)
            {
                DataRowView[] ReportingAccounts = AAccountHierarchyDetail.DefaultView.FindRows(new object[] { AAccountHierarchy, account });

                if (ReportingAccounts.Length == 0)
                {
                    AResult.Add(account);
                }
                else
                {
                    foreach (DataRowView rv in ReportingAccounts)
                    {
                        AAccountHierarchyDetailRow row = (AAccountHierarchyDetailRow)rv.Row;

                        GetReportingAccounts(AAccountHierarchyDetail, AResult, row.ReportingAccountCode, AAccountHierarchy);
                    }
                }
            }
        }

        /// <summary>
        /// Get all accounts that report into the given summary account
        /// </summary>
        /// <returns>a CSV list of the reporting accounts</returns>
        [RequireModulePermission("FINANCE-1")]
        public static string GetReportingAccounts(int ALedgerNumber, string ASummaryAccountCodes, string ARemoveAccountsFromList)
        {
            GLSetupTDS MainDS = TGLSetupWebConnector.LoadAccountHierarchies(ALedgerNumber);

            List <string>accountcodes = new List <string>();

            MainDS.AAccountHierarchyDetail.DefaultView.Sort =
                AAccountHierarchyDetailTable.GetAccountHierarchyCodeDBName() + "," +
                AAccountHierarchyDetailTable.GetAccountCodeToReportToDBName();

            GetReportingAccounts(MainDS.AAccountHierarchyDetail, accountcodes, ASummaryAccountCodes, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD);

            string[] RemoveAccountsFromList = ARemoveAccountsFromList.Split(new char[] { ',' });

            foreach (string s in RemoveAccountsFromList)
            {
                if (accountcodes.Contains(s))
                {
                    accountcodes.Remove(s);
                }
            }

            return StringHelper.StrMerge(accountcodes.ToArray(), ',');
        }

        // Returns reporting accounts as a comma separated string with each account encased in commas. Needed for SQL.
        private static string GetFormattedReportingAccounts(int ALedgerNumber,
            string ASummaryAccountCodes,
            string AAccountHierarchy)
        {
            string ReturnValue = string.Empty;

            string[] Accounts = ASummaryAccountCodes.Split(new char[] { ',' });

            GLSetupTDS MainDS = TGLSetupWebConnector.LoadAccountHierarchies(ALedgerNumber);

            MainDS.AAccountHierarchyDetail.DefaultView.Sort =
                AAccountHierarchyDetailTable.GetAccountHierarchyCodeDBName() + "," +
                AAccountHierarchyDetailTable.GetAccountCodeToReportToDBName();

            foreach (string account in Accounts)
            {
                DataRowView[] ReportingAccounts = MainDS.AAccountHierarchyDetail.DefaultView.FindRows(new object[] { AAccountHierarchy, account });

                if (ReportingAccounts.Length == 0)
                {
                    ReturnValue += "'" + account + "', ";
                }
                else
                {
                    foreach (DataRowView rv in ReportingAccounts)
                    {
                        AAccountHierarchyDetailRow row = (AAccountHierarchyDetailRow)rv.Row;

                        GetFormattedReportingAccounts(MainDS.AAccountHierarchyDetail, ref ReturnValue, row.ReportingAccountCode, AAccountHierarchy);
                    }
                }
            }

            ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 2);

            return ReturnValue;
        }

        private static void GetFormattedReportingAccounts(AAccountHierarchyDetailTable AAccountHierarchyDetail,
            ref string AResult,
            string ASummaryAccountCodes,
            string AAccountHierarchy)
        {
            string[] Accounts = ASummaryAccountCodes.Split(new char[] { ',' });

            foreach (string account in Accounts)
            {
                DataRowView[] ReportingAccounts = AAccountHierarchyDetail.DefaultView.FindRows(new object[] { AAccountHierarchy, account });

                if (ReportingAccounts.Length == 0)
                {
                    AResult += "'" + account + "', ";
                }
                else
                {
                    foreach (DataRowView rv in ReportingAccounts)
                    {
                        AAccountHierarchyDetailRow row = (AAccountHierarchyDetailRow)rv.Row;

                        GetFormattedReportingAccounts(AAccountHierarchyDetail, ref AResult, row.ReportingAccountCode, AAccountHierarchy);
                    }
                }
            }
        }

        /// <summary>
        /// Get the name for this Ledger, using a new DB connection
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static string GetLedgerName(int ALedgerNumber)
        {
            return TLedgerInfo.GetLedgerNameUsingSeparateDb(ALedgerNumber);
        }

        /// <summary>
        /// Collects the period opening / closing balances for the Account Detail report
        /// </summary>
        /// <param name="ALedgerFilter"></param>
        /// <param name="AAccountCodeFilter"></param>
        /// <param name="ACostCentreFilter"></param>
        /// <param name="AFinancialYear"></param>
        /// <param name="ASortBy"></param>
        /// <param name="ATransactionsTbl"></param>
        /// <param name="AStartPeriod"></param>
        /// <param name="AEndPeriod"></param>
        /// <param name="ASelectedCurrency"></param>
        /// <returns>DataTable</returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable GetPeriodBalances(
            String ALedgerFilter,
            String AAccountCodeFilter,
            String ACostCentreFilter,
            Int32 AFinancialYear,
            String ASortBy,
            DataTable ATransactionsTbl,
            Int32 AStartPeriod,
            Int32 AEndPeriod,
            String ASelectedCurrency)
        {
            DataTable Results = new DataTable();

            Results.Columns.Add(new DataColumn("a_cost_centre_code_c", typeof(string)));
            Results.Columns.Add(new DataColumn("a_account_code_c", typeof(string)));
            Results.Columns.Add(new DataColumn("OpeningBalance", typeof(Decimal)));
            Results.Columns.Add(new DataColumn("ClosingBalance", typeof(Decimal)));
            Results.Columns.Add(new DataColumn("Currency", typeof(string)));

            Boolean FromStartOfYear = (AStartPeriod == 1);
            TReportingDbAdapter DbAdapter = new TReportingDbAdapter(true);

            if (!FromStartOfYear)
            {
                AStartPeriod -= 1; // I want the closing balance of the previous period.
            }

            String CurrencyCodeField = ASelectedCurrency.StartsWith("Int") ? "a_ledger.a_intl_currency_c" :
                                       ASelectedCurrency == "Base" ? "a_ledger.a_base_currency_c" :
                                       "CASE WHEN a_account.a_foreign_currency_flag_l=TRUE THEN a_account.a_foreign_currency_code_c ELSE a_ledger.a_base_currency_c END";
            String GroupField = "";

            if (ASortBy == "Account")
            {
                GroupField = " ORDER BY glm.a_account_code_c, glm.a_cost_centre_code_c";
            }

            if (ASortBy == "Cost Centre")
            {
                GroupField = " ORDER BY glm.a_cost_centre_code_c, glm.a_account_code_c";
            }

            //
            // This can only be used posting Accounts and Cost Centres;
            // it does no summarisation, so summary Accounts and Cost Centres won't work here:

            String Query = "SELECT glm.a_cost_centre_code_c, glm.a_account_code_c, glmp.a_period_number_i, " +
                           "a_account.a_debit_credit_indicator_l AS Debit, " +
                           "glm.a_start_balance_base_n AS StartBalanceBase, " +
                           "glm.a_start_balance_foreign_n AS StartBalanceForeign, " +
                           "glm.a_start_balance_intl_n AS StartBalanceIntl, " +
                           "glmp.a_actual_base_n AS BalanceBase, " +
                           "glmp.a_actual_foreign_n AS BalanceForeign, " +
                           "glmp.a_actual_intl_n AS BalanceIntl," +
                           CurrencyCodeField + " AS Currency, " +
                           " a_ledger.a_base_currency_c AS BaseCurrency " +
                           " FROM a_general_ledger_master AS glm, a_general_ledger_master_period AS glmp, a_account, a_cost_centre, a_ledger" +
                           " WHERE glm." + ALedgerFilter +
                           " AND a_account." + ALedgerFilter +
                           " AND a_cost_centre." + ALedgerFilter +
                           " AND a_ledger." + ALedgerFilter +
                           " AND a_account.a_posting_status_l = TRUE" +
                           " AND a_cost_centre.a_posting_cost_centre_flag_l = TRUE" +
                           " AND glm.a_year_i = " + AFinancialYear +
                           " AND glm.a_account_code_c = a_account.a_account_code_c " +
                           " AND glm.a_cost_centre_code_c = a_cost_centre.a_cost_centre_code_c " +
                           AAccountCodeFilter +
                           ACostCentreFilter +
                           " AND glm.a_glm_sequence_i = glmp.a_glm_sequence_i" +
                           " AND glmp.a_period_number_i BETWEEN " + AStartPeriod + " AND " + AEndPeriod +
                           GroupField;


            DataTable GlmTbl = null;
            TDBTransaction ReadTrans = null;
            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTrans,
                delegate
                {
                    GlmTbl = DbAdapter.RunQuery(Query, "balances", ReadTrans);
                });

            DbAdapter.CloseConnection();

            String CostCentre = "";
            String AccountCode = "";
            Int32 MaxPeriod = -1;
            Int32 MinPeriod = 99;
            DataRow NewRow = null;
            Decimal MakeItDebit = 1;

            // For each CostCentre / Account combination  I want just a single row, with the opening and closing balances,
            // so I need to pre-process the stuff I've got in this table, and generate rows in the Results table.

            foreach (DataRow row in GlmTbl.Rows)
            {
                if (DbAdapter.IsCancelled)
                {
                    return Results;
                }

                MakeItDebit = (Convert.ToBoolean(row["Debit"])) ? -1 : 1;

                if ((row["a_cost_centre_code_c"].ToString() != CostCentre) || (row["a_account_code_c"].ToString() != AccountCode)) // a new CC/AC combination
                {
                    NewRow = Results.NewRow();
                    NewRow["Currency"] = row["Currency"].ToString();

                    CostCentre = row["a_cost_centre_code_c"].ToString();
                    NewRow["a_cost_centre_code_c"] = CostCentre;

                    AccountCode = row["a_account_code_c"].ToString();
                    MakeItDebit = (Convert.ToBoolean(row["Debit"])) ? -1 : 1;
                    NewRow["a_account_code_c"] = AccountCode;
                    Results.Rows.Add(NewRow);

                    MaxPeriod = -1;
                    MinPeriod = 99;
                }

                Int32 ThisPeriod = Convert.ToInt32(row["a_period_number_i"]);

                String BalanceField = "BalanceBase";

                if (ASelectedCurrency.StartsWith("Int"))
                {
                    BalanceField = "BalanceIntl";
                }
                else
                {
                    if (ASelectedCurrency.StartsWith("Trans") && (row["Currency"].ToString() != row["BaseCurrency"].ToString()))
                    {
                        BalanceField = "BalanceForeign";
                    }
                }

                if (ThisPeriod < MinPeriod)
                {
                    MinPeriod = ThisPeriod;

                    Decimal OpeningBalance = 0;

                    if (FromStartOfYear)
                    {
                        String StartBalanceField = "StartBalanceBase";

                        if (ASelectedCurrency.StartsWith("Int"))
                        {
                            StartBalanceField = "StartBalanceIntl";
                        }
                        else
                        {
                            if (ASelectedCurrency.StartsWith("Trans") && (row["Currency"].ToString() != row["BaseCurrency"].ToString()))
                            {
                                StartBalanceField = "StartBalanceForeign";
                            }
                        }

                        if (row[StartBalanceField].GetType() != typeof(DBNull))
                        {
                            OpeningBalance = Convert.ToDecimal(row[StartBalanceField]);
                        }
                    }
                    else
                    {
                        if (row[BalanceField].GetType() != typeof(DBNull))
                        {
                            OpeningBalance = Convert.ToDecimal(row[BalanceField]);
                        }
                    }

                    NewRow["OpeningBalance"] = MakeItDebit * OpeningBalance;
                }

                if (ThisPeriod > MaxPeriod)
                {
                    MaxPeriod = ThisPeriod;
                    Decimal ClosingBalance = 0;

                    if (row[BalanceField].GetType() != typeof(DBNull))
                    {
                        ClosingBalance = Convert.ToDecimal(row[BalanceField]);
                    }

                    NewRow["ClosingBalance"] = MakeItDebit * ClosingBalance;
                }
            } // foreach

            for (Int32 Idx = Results.Rows.Count - 1; Idx >= 0; Idx--)
            {
                DataRow ResultsRow = Results.Rows[Idx];

                //
                // Since a revision in October 2014, this balances table can be the master table for the Account Detail report
                // (That is, "for each opening and closing balance, list any applicable transactions",
                // rather than, "for each Account/Cost Centre combination where we see transactions, show opening and closing balance".)
                // The effect of this is I need to remove rows where opening and closing balances both are zero,
                // AND there were no transactions in the selected period.
                if ((Convert.ToDecimal(ResultsRow["OpeningBalance"]) == 0) && (Convert.ToDecimal(ResultsRow["ClosingBalance"]) == 0))
                {
                    ATransactionsTbl.DefaultView.RowFilter = String.Format("AccountCode='{0}' AND CostCentreCode ='{1}'",
                        ResultsRow["a_account_code_c"],
                        ResultsRow["a_cost_centre_code_c"]);

                    if (ATransactionsTbl.DefaultView.Count == 0)
                    {
                        ResultsRow.Delete();
                    }
                }
            }

            Results.AcceptChanges();
            return Results;
        }

        //
        // Find the level of nesting for this account, using recursive call
        // The nesting level affects indentation in the printed report.
        private static Int32 GetAccountLevel(DataView HierarchyView, String AccountCode, ref String AccountPath, Int32 ChildLevel)
        {
            Int32 RowNum = HierarchyView.Find(AccountCode);

            if (RowNum < 0)
            {
                return ChildLevel;
            }
            else
            {
                DataRow Row = HierarchyView[RowNum].Row;
                AccountPath = Row["AccountCode"].ToString() + "~" + AccountPath;
                return GetAccountLevel(HierarchyView, Row["ReportsTo"].ToString(), ref AccountPath, ChildLevel + 1);
            }
        }

        /// <summary>
        /// Utility function for IncomeExpenseTable and BalanceSheet.
        /// Create or update the row that this account reports to.
        /// USES RECURSION to create or update grandparents, and update AccountLevel and AccountPath.
        /// </summary>
        private static Int32 AddTotalsToParentAccountRow(
            DataTable filteredResults,
            AAccountHierarchyDetailTable HierarchyTbl,
            Int32 LedgerNumber,
            String CostCentreCode,
            String AccountCode,
            DataRow NewDataRow,
            Boolean SortAccountFirst,
            Int32 NumPeriodFields,
            out string ParentAccountPath,
            out Int32 ParentAccountTypeOrder,
            TDBTransaction ReadTrans)
        {
            Boolean ByPeriod = (NumPeriodFields > 0);
            Int32 Idx = HierarchyTbl.DefaultView.Find(AccountCode);
            // If Idx < 0 that's pretty serious. The next line will raise an exception.
            AAccountHierarchyDetailRow HDRow = (AAccountHierarchyDetailRow)HierarchyTbl.DefaultView[Idx].Row;
            String ParentAccountCode = HDRow.AccountCodeToReportTo;

            if (((filteredResults.TableName == "IncomeExpense") && (ParentAccountCode == "RET EARN")) // This is a literal AccountCode, which (in a non-OM setting) the user could potentially change.
                || (ParentAccountCode == LedgerNumber.ToString()))                                    // And actually this is effectively a literal too.
            {
                // The calling Row is a "first level" account with no parent.
                ParentAccountPath = "";
                ParentAccountTypeOrder = 0;
                return 0;
            }
            else
            {
                String MyParentAccountPath;
                Int32 MyParentAccountTypeOrder;

                if (CostCentreCode == "")
                {
                    Idx = filteredResults.DefaultView.Find(new object[] { ParentAccountCode });
                }
                else
                {
                    if (SortAccountFirst)
                    {
                        Idx = filteredResults.DefaultView.Find(new object[] { ParentAccountCode, CostCentreCode });
                    }
                    else
                    {
                        Idx = filteredResults.DefaultView.Find(new object[] { CostCentreCode, ParentAccountCode });
                    }
                }

                DataRow ParentRow;

                if (Idx < 0)                // This Parent Account Code should have a row in the table - if not I need to create one now.
                {
                    ParentRow = filteredResults.NewRow();
                    DataUtilities.CopyAllColumnValues(NewDataRow, ParentRow);
                    ParentRow["AccountCode"] = ParentAccountCode;

                    // I need to find the name of this parent account.
                    AAccountRow ParentAccountRow = AAccountAccess.LoadByPrimaryKey(LedgerNumber, ParentAccountCode, ReadTrans)[0];
                    ParentRow["AccountName"] = ParentAccountRow.AccountCodeShortDesc;
                    ParentRow["AccountType"] = ParentAccountRow.AccountType;
                    ParentRow["DebitCredit"] = ParentAccountRow.DebitCreditIndicator;


                    //
                    // This AccountTypeOrder will be used for sorting the rows in the report,
                    // but could be overwritten since the "top level" AccountTypeOrder ends up getting copied to all children.

                    switch (ParentAccountRow.AccountType)
                    {
                        case "Income":
                        case "Asset":
                            ParentRow["AccountTypeOrder"] = 1;
                            break;

                        case "Expense":
                        case "Liability":
                            ParentRow["AccountTypeOrder"] = 2;
                            break;

                        case "Equity":
                            ParentRow["AccountTypeOrder"] = 3;
                            break;
                    }

                    ParentRow["AccountIsSummary"] = true;
                    filteredResults.Rows.Add(ParentRow);

                    Decimal Sign = (Convert.ToBoolean(ParentRow["DebitCredit"]) == Convert.ToBoolean(NewDataRow["DebitCredit"])) ? 1 : -1;

                    if (ByPeriod)
                    {
                        for (Int32 period = 1; period <= NumPeriodFields; period++)
                        {
                            String PeriodField = "P" + period;
                            ParentRow[PeriodField] = Sign * Convert.ToDecimal(NewDataRow[PeriodField]);
                        }

                        ParentRow["Actual"] = 0;
                        ParentRow["ActualYTD"] = 0;
                        ParentRow["Budget"] = 0;
                        ParentRow["BudgetYTD"] = 0;
                        ParentRow["LastYearActual"] = 0;
                    }
                    else
                    {
                        ParentRow["Actual"] = Sign * Convert.ToDecimal(NewDataRow["Actual"]);
                        ParentRow["ActualYTD"] = Sign * Convert.ToDecimal(NewDataRow["ActualYTD"]);
                        ParentRow["LastYearActual"] = Sign * Convert.ToDecimal(NewDataRow["LastYearActual"]);

                        if (filteredResults.Columns.Contains("LastYearEnd"))
                        {
                            ParentRow["LastYearEnd"] = Sign * Convert.ToDecimal(NewDataRow["LastYearEnd"]);
                            ParentRow["LastYearActualYtd"] = Sign * Convert.ToDecimal(NewDataRow["LastYearActualYtd"]);
                            ParentRow["LastYearLastMonthYtd"] = Sign * Convert.ToDecimal(NewDataRow["LastYearLastMonthYtd"]);
                        }
                    }

                    //
                    // Now ensure that my newly-created parent also creates her own parent:

                    Int32 AccountLevel = AddTotalsToParentAccountRow( // Update my parent first
                        filteredResults,
                        HierarchyTbl,
                        LedgerNumber,
                        CostCentreCode,
                        ParentAccountCode,
                        NewDataRow,
                        SortAccountFirst,
                        NumPeriodFields,
                        out MyParentAccountPath,
                        out MyParentAccountTypeOrder,
                        ReadTrans);

                    //
                    // I need to find the "Report Order" for the parent row:

                    Idx = HierarchyTbl.DefaultView.Find(ParentAccountCode);
                    // If Idx < 0 that's pretty serious. The next line will raise an exception.
                    HDRow = (AAccountHierarchyDetailRow)HierarchyTbl.DefaultView[Idx].Row;

                    ParentRow["AccountPath"] = MyParentAccountPath + HDRow.ReportOrder + "-" + ParentAccountCode + "~";
                    ParentRow["AccountLevel"] = AccountLevel;
                }
                else  // Parent row exists already
                {
                    ParentRow = filteredResults.DefaultView[Idx].Row;
                    //
                    // I need to add or subtract these values depending on the account type I'm summarizing into.
                    //
                    Decimal Sign = (Convert.ToBoolean(ParentRow["DebitCredit"]) == Convert.ToBoolean(NewDataRow["DebitCredit"])) ? 1 : -1;

                    if (ByPeriod)
                    {
                        for (Int32 period = 1; period <= NumPeriodFields; period++)
                        {
                            String PeriodField = "P" + period;
                            ParentRow[PeriodField] = Convert.ToDecimal(ParentRow[PeriodField]) + Sign * Convert.ToDecimal(NewDataRow[PeriodField]);
                        }
                    }
                    else
                    {
                        ParentRow["Actual"] = Convert.ToDecimal(ParentRow["Actual"]) + (Sign * Convert.ToDecimal(NewDataRow["Actual"]));
                        ParentRow["ActualYTD"] = Convert.ToDecimal(ParentRow["ActualYTD"]) + (Sign * Convert.ToDecimal(NewDataRow["ActualYTD"]));
                        ParentRow["LastYearActual"] = Convert.ToDecimal(ParentRow["LastYearActual"]) +
                                                      (Sign * Convert.ToDecimal(NewDataRow["LastYearActual"]));

                        //
                        // The Income Expense row contains more LastYear fields:
                        if (filteredResults.Columns.Contains("LastYearEnd"))
                        {
                            ParentRow["LastYearEnd"] = Convert.ToDecimal(ParentRow["LastYearEnd"]) +
                                                       (Sign * Convert.ToDecimal(NewDataRow["LastYearEnd"]));
                            ParentRow["LastYearActualYtd"] = Convert.ToDecimal(ParentRow["LastYearActualYtd"]) +
                                                             (Sign * Convert.ToDecimal(NewDataRow["LastYearActualYtd"]));
                            ParentRow["LastYearLastMonthYtd"] = Convert.ToDecimal(ParentRow["LastYearLastMonthYtd"]) +
                                                                (Sign * Convert.ToDecimal(NewDataRow["LastYearLastMonthYtd"]));
                        }

                        //
                        // The Balance Sheet row doesn't have budgets, but the Income Expense does:
                        if (ParentRow.Table.Columns.Contains("Budget"))
                        {
                            ParentRow["Budget"] = Convert.ToDecimal(ParentRow["Budget"]) + (Sign * Convert.ToDecimal(NewDataRow["Budget"]));
                            ParentRow["BudgetYTD"] = Convert.ToDecimal(ParentRow["BudgetYTD"]) + (Sign * Convert.ToDecimal(NewDataRow["BudgetYTD"]));
                            ParentRow["LastYearBudget"] = Convert.ToDecimal(ParentRow["LastYearBudget"]) +
                                                          (Sign * Convert.ToDecimal(NewDataRow["LastYearBudget"]));
                            ParentRow["WholeYearBudget"] = Convert.ToDecimal(ParentRow["WholeYearBudget"]) +
                                                           (Sign * Convert.ToDecimal(NewDataRow["WholeYearBudget"]));
                            ParentRow["NextYearBudget"] = Convert.ToDecimal(ParentRow["NextYearBudget"]) +
                                                          (Sign * Convert.ToDecimal(NewDataRow["NextYearBudget"]));
                        }
                    }

                    //
                    // Now ensure that my parent also updates her own parent:

                    AddTotalsToParentAccountRow( // Update my parent first
                        filteredResults,
                        HierarchyTbl,
                        LedgerNumber,
                        CostCentreCode,
                        ParentAccountCode,
                        NewDataRow,
                        SortAccountFirst,
                        NumPeriodFields,
                        out MyParentAccountPath,
                        out MyParentAccountTypeOrder,
                        ReadTrans);
                }

                //
                // I'm going to adopt my parent's sort order so that I'm listed under my parent, even if I'm a different account type.
                // BUT NOT if my parent is the root of the whole Hierarchy.
                if (Convert.ToInt32(ParentRow["AccountLevel"]) > 1)
                {
                    ParentRow["AccountTypeOrder"] = MyParentAccountTypeOrder;
                    NewDataRow["AccountTypeOrder"] = MyParentAccountTypeOrder;
                }

                ParentAccountTypeOrder = Convert.ToInt32(ParentRow["AccountTypeOrder"]);
                ParentAccountPath = ParentRow["AccountPath"].ToString();
                return 1 + Convert.ToInt32(ParentRow["AccountLevel"]);
            }
        }

        /// <summary>
        /// Returns a DataTable for use in client-side reporting
        /// This version begins with GLM and GLMP tables, but calculates amounts for the summary accounts,
        /// adding individual posting Cost Centres, so it does not rely on the summarisation in GLMP.
        /// </summary>
        [NoRemoting]
        public static DataTable BalanceSheetTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            /* Required columns:
             * Actual
             * LastYearActual
             * LastYearEnd
             */

            Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            Int32 NumberOfAccountingPeriods;
            Int32 AccountingYear = AParameters["param_year_i"].ToInt32();
            String yearFilter;

            Int32 ReportPeriodEnd = AParameters["param_end_period_i"].ToInt32();
            String periodFilter;
            String thisYearSelect;
            String lastYearSelect;

            String HierarchyName = AParameters["param_account_hierarchy_c"].ToString();
            String RootCostCentre = AParameters["param_cost_centre_code"].ToString();

            Boolean International = AParameters["param_currency"].ToString().StartsWith("Int");
            Decimal exchangeRateNow = 1;
            Decimal LastYearExchangeRate = 1;

            NumberOfAccountingPeriods = new TLedgerInfo(LedgerNumber, DbAdapter.FPrivateDatabaseObj).NumberOfAccountingPeriods;

            if (ReportPeriodEnd > NumberOfAccountingPeriods)
            {
                yearFilter = " AND glm.a_year_i = " + AccountingYear;
                periodFilter = " AND glmp.a_period_number_i IN (" +
                               (ReportPeriodEnd - NumberOfAccountingPeriods) +
                               "," +
                               ReportPeriodEnd +
                               ")";
                thisYearSelect = "SUM (CASE WHEN glmp.a_period_number_i= " +
                                 ReportPeriodEnd + " THEN glmp.a_actual_base_n ELSE 0 END)";
                lastYearSelect = "SUM (CASE WHEN glmp.a_period_number_i= " +
                                 (ReportPeriodEnd - NumberOfAccountingPeriods) + " THEN glmp.a_actual_base_n ELSE 0 END)";
            }
            else
            {
                yearFilter = " AND glm.a_year_i IN (" + (AccountingYear - 1) + ", " + AccountingYear + ")";
                periodFilter = " AND glmp.a_period_number_i=" + ReportPeriodEnd;
                thisYearSelect = "SUM (CASE WHEN glm.a_year_i = " +
                                 AccountingYear + " AND glmp.a_period_number_i= " +
                                 ReportPeriodEnd + " THEN glmp.a_actual_base_n ELSE 0 END)";
                lastYearSelect = "SUM (CASE WHEN glm.a_year_i = " +
                                 (AccountingYear - 1) + " AND glmp.a_period_number_i= " +
                                 ReportPeriodEnd + " THEN glmp.a_actual_base_n ELSE 0 END)";
            }

            if (International)
            {
                TCorporateExchangeRateCache ExchangeRateCache = new TCorporateExchangeRateCache();
                TLedgerInfo ledgerInfo = new TLedgerInfo(LedgerNumber);
                exchangeRateNow = ExchangeRateCache.GetCorporateExchangeRate(DbAdapter.FPrivateDatabaseObj,
                    LedgerNumber,
                    ledgerInfo.CurrentFinancialYear,
                    ledgerInfo.CurrentPeriod,
                    -1);

                if (ReportPeriodEnd > NumberOfAccountingPeriods)
                {
                    LastYearExchangeRate = ExchangeRateCache.GetCorporateExchangeRate(DbAdapter.FPrivateDatabaseObj,
                        LedgerNumber,
                        AccountingYear,
                        ReportPeriodEnd - NumberOfAccountingPeriods,
                        -1);
                }
                else
                {
                    LastYearExchangeRate = ExchangeRateCache.GetCorporateExchangeRate(DbAdapter.FPrivateDatabaseObj,
                        LedgerNumber,
                        AccountingYear - 1,
                        ReportPeriodEnd,
                        -1);
                }
            } // International

            TLogging.Log(Catalog.GetString("Loading data.."), TLoggingType.ToStatusBar);
            DataTable resultTable = null;
            TDBTransaction ReadTrans = null;
            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTrans,
                delegate
                {
                    ACostCentreTable AllCostCentres = ACostCentreAccess.LoadViaALedger(LedgerNumber, ReadTrans);
                    AllCostCentres.DefaultView.Sort = ACostCentreTable.GetCostCentreToReportToDBName();
                    List <string>ReportingCostCentres = new List <string>();
                    GetReportingCostCentres(AllCostCentres, ReportingCostCentres, RootCostCentre);
                    String CostCentreList = StringHelper.StrMerge(ReportingCostCentres.ToArray(), ',');
                    CostCentreList = "'" + CostCentreList.Replace(",", "','") + "'";

                    String Summarised =                                                                  // This query gets the totals I need
                                        "(SELECT " +
                                        " a_account.a_account_type_c AS AccountType," +
                                        " a_account.a_debit_credit_indicator_l AS DebitCredit," +
                                        " glm.a_account_code_c AS AccountCode," +
                                        " a_account.a_account_code_short_desc_c AS AccountName," +
                                        LastYearExchangeRate + " * (" + lastYearSelect + ") AS LastYearActual," +
                                        exchangeRateNow + " * (" + thisYearSelect + ") AS ActualYTD" +

                                        " FROM a_general_ledger_master AS glm, a_general_ledger_master_period AS glmp, a_account" +
                                        " WHERE glm.a_ledger_number_i=" + LedgerNumber +
                                        " AND glm.a_cost_centre_code_c in (" + CostCentreList + ") " +
                                        yearFilter +
                                        " AND glm.a_glm_sequence_i = glmp.a_glm_sequence_i" +
                                        periodFilter +
                                        " AND a_account.a_account_code_c = glm.a_account_code_c" +
                                        " AND a_account.a_ledger_number_i = glm.a_ledger_number_i" +
                                        " AND a_account.a_posting_status_l = true" +
                                        " GROUP BY a_account.a_account_type_c," +
                                        "   a_account.a_debit_credit_indicator_l, glm.a_account_code_c," +
                                        "   a_account.a_account_code_short_desc_c" +
                                        " ORDER BY glm.a_account_code_c) AS summarised"
                    ;

                    String Query =
                        "SELECT " +
                        "summarised.*," +
                        " 0 AS Actual, " +
                        " 1 AS AccountLevel," +
                        " false AS HasChildren," +
                        " false AS ParentFooter," +
                        " 0 AS AccountTypeOrder," +
                        " false AS AccountIsSummary," +
                        " 'Path' AS AccountPath" +
                        " FROM " + Summarised;

                    resultTable = DbAdapter.RunQuery(Query, "BalanceSheet", ReadTrans);

                    //
                    // I only have "posting accounts" - I need to add the summary accounts.

                    AAccountHierarchyDetailTable HierarchyTbl = AAccountHierarchyDetailAccess.LoadViaAAccountHierarchy(LedgerNumber,
                        HierarchyName,
                        ReadTrans);

                    HierarchyTbl.DefaultView.Sort = "a_reporting_account_code_c";  // These two sort orders
                    resultTable.DefaultView.Sort = "AccountCode";                  // Are required by AddTotalsToParentAccountRow, below.

                    Int32 PostingAccountRecords = resultTable.Rows.Count;

                    TLogging.Log(Catalog.GetString("Summarise to parent accounts.."), TLoggingType.ToStatusBar);

                    for (Int32 Idx = 0; Idx < PostingAccountRecords; Idx++)
                    {
                        if (DbAdapter.IsCancelled)
                        {
                            return;
                        }

                        DataRow Row = resultTable.Rows[Idx];
                        String accountCode = Row["AccountCode"].ToString();
                        String ParentAccountPath;
                        Int32 ParentAccountTypeOrder;
                        Int32 AccountLevel = AddTotalsToParentAccountRow(
                            resultTable,
                            HierarchyTbl,
                            LedgerNumber,
                            "", // No Cost Centres on Balance Sheet
                            accountCode,
                            Row,
                            false,
                            0,
                            out ParentAccountPath,
                            out ParentAccountTypeOrder,
                            ReadTrans);
                        Row["AccountLevel"] = AccountLevel;
                        Row["AccountPath"] = ParentAccountPath + accountCode;
                    }
                }); // Get NewOrExisting AutoReadTransaction

            //
            // Now if I re-order the result by AccountPath, hide all the empty rows,
            // and rows that are too detailed, it should be what I need!

            Int32 DetailLevel = AParameters["param_nesting_depth"].ToInt32();
            String DepthFilter = " AND AccountLevel<=" + DetailLevel.ToString();
            resultTable.DefaultView.Sort = "AccountTypeOrder, AccountPath";

            resultTable.DefaultView.RowFilter = "(ActualYTD <> 0 OR LastYearActual <> 0 )" + // Only non-zero rows
                                                DepthFilter;        // Nothing too detailed

            resultTable = resultTable.DefaultView.ToTable("BalanceSheet");

            //
            // The income and expense accounts have been used to produce the balance of 'PL',
            // but now I don't want to see those details - only the total.

            resultTable.DefaultView.RowFilter = "AccountPath NOT LIKE '%-PL~%' OR AccountLevel < 3"; // Don't include Children of PL account
            resultTable = resultTable.DefaultView.ToTable("BalanceSheet");

            //
            // Finally, to make the hierarchical report possible,
            // I want to include a note to show whether a row has child rows,
            // and if it does, I'll copy this row to a new row, below the children, marking the new row as "footer".

            TLogging.Log(Catalog.GetString("Format data for reporting.."), TLoggingType.ToStatusBar);

            for (Int32 RowIdx = 0; RowIdx < resultTable.Rows.Count - 1; RowIdx++)
            {
                if (DbAdapter.IsCancelled)
                {
                    return null;
                }

                Int32 ParentAccountLevel = Convert.ToInt32(resultTable.Rows[RowIdx]["AccountLevel"]);
                Boolean HasChildren = (Convert.ToInt32(resultTable.Rows[RowIdx + 1]["AccountLevel"]) > ParentAccountLevel);
                resultTable.Rows[RowIdx]["HasChildren"] = HasChildren;

                if (HasChildren)
                {
                    Int32 NextSiblingPos = -1;

                    for (Int32 ChildIdx = RowIdx + 2; ChildIdx < resultTable.Rows.Count; ChildIdx++)
                    {
                        if (Convert.ToInt32(resultTable.Rows[ChildIdx]["AccountLevel"]) <= ParentAccountLevel)  // This row is not a child of mine
                        {                                                                                       // so I insert my footer before here.
                            NextSiblingPos = ChildIdx;
                            break;
                        }
                    }

                    DataRow FooterRow = resultTable.NewRow();
                    DataUtilities.CopyAllColumnValues(resultTable.Rows[RowIdx], FooterRow);
                    FooterRow["ParentFooter"] = true;
                    FooterRow["HasChildren"] = false;

                    if (NextSiblingPos > 0)
                    {
                        resultTable.Rows.InsertAt(FooterRow, NextSiblingPos);
                    }
                    else
                    {
                        resultTable.Rows.Add(FooterRow);
                    }
                }
            } // for

            TLogging.Log("", TLoggingType.ToStatusBar);
            return resultTable;
        } // Balance Sheet Table

        /// <summary>
        /// For "Cost Centre Breakdown", I need to add records summarising the transactions for Summary Accounts by Cost Centre.
        /// This method will create the new row if necessary.
        /// </summary>
        /// <param name="filteredView">A view that includes only the breakdown rows.</param>
        /// <param name="DetailLevel">The new summary row should be at this level.</param>
        /// <param name="NewDataRow">This row will be removed - its data should be copied to the new summary row.</param>
        private static void AddToCostCentreBreakdownSummary(
            DataView filteredView,
            Int32 DetailLevel,
            DataRow NewDataRow)
        {
            //
            // If "detail" level has been selected, I'm only looking to summarise posting accounts:
            if (DetailLevel == 99)
            {
                if (Convert.ToBoolean(NewDataRow["AccountIsSummary"]))
                {
                    return;
                }

                DetailLevel = Convert.ToInt32(NewDataRow["AccountLevel"]);
            }
            //
            // Otherwise, if the AccountLevel of this row is too low, I'm completely ignoring it:
            else
            {
                if ((Convert.ToInt32(NewDataRow["AccountLevel"]) < DetailLevel)
                    || (Convert.ToBoolean(NewDataRow["AccountIsSummary"]) == true)
                    )
                {
                    return;
                }
            }

            //
            // I need to crop the AccountPath to before the "Nth Slash" so the transactions get summarised to the right level.
            String SummaryAccountPath = NewDataRow["AccountPath"].ToString();
            Int32 NthSlash = DetailLevel + 2;

            for (Int32 i = 0; i < SummaryAccountPath.Length; i++)
            {
                if (SummaryAccountPath[i] == '~')
                {
                    if (--NthSlash == 0)
                    {
                        SummaryAccountPath = SummaryAccountPath.Substring(0, i);
                        break;
                    }
                }
            }

            DataRow SummaryRow;
            Int32 ViewIdx = filteredView.Find(new object[] { NewDataRow["AccountTypeOrder"], SummaryAccountPath, NewDataRow["CostCentreCode"] });

            if (ViewIdx < 0) // No record yet..
            {
                SummaryRow = filteredView.Table.NewRow();
                DataUtilities.CopyAllColumnValues(NewDataRow, SummaryRow);
                SummaryRow["Breakdown"] = true;
                SummaryRow["AccountName"] = "";
                SummaryRow["AccountLevel"] = DetailLevel;
                SummaryRow["AccountPath"] = SummaryAccountPath;
                filteredView.Table.Rows.Add(SummaryRow);
            }
            else
            {
                Decimal Sign = 1;
                SummaryRow = filteredView[ViewIdx].Row;

                SummaryRow["Actual"] = Convert.ToDecimal(SummaryRow["Actual"]) + (Sign * Convert.ToDecimal(NewDataRow["Actual"]));
                SummaryRow["ActualYTD"] = Convert.ToDecimal(SummaryRow["ActualYTD"]) + (Sign * Convert.ToDecimal(NewDataRow["ActualYTD"]));
                SummaryRow["LastYearActualYtd"] = Convert.ToDecimal(SummaryRow["LastYearActualYtd"]) +
                                                  (Sign * Convert.ToDecimal(NewDataRow["LastYearActualYtd"]));
                SummaryRow["LastYearLastMonthYtd"] = Convert.ToDecimal(SummaryRow["LastYearLastMonthYtd"]) +
                                                     (Sign * Convert.ToDecimal(NewDataRow["LastYearLastMonthYtd"]));
                SummaryRow["LastYearActual"] = Convert.ToDecimal(SummaryRow["LastYearActual"]) +
                                               (Sign * Convert.ToDecimal(NewDataRow["LastYearActual"]));
                SummaryRow["LastYearEnd"] = Convert.ToDecimal(SummaryRow["LastYearEnd"]) + (Sign * Convert.ToDecimal(NewDataRow["LastYearEnd"]));
                SummaryRow["Budget"] = Convert.ToDecimal(SummaryRow["Budget"]) + (Sign * Convert.ToDecimal(NewDataRow["Budget"]));
                SummaryRow["BudgetYTD"] = Convert.ToDecimal(SummaryRow["BudgetYTD"]) + (Sign * Convert.ToDecimal(NewDataRow["BudgetYTD"]));
                SummaryRow["LastYearBudget"] = Convert.ToDecimal(SummaryRow["LastYearBudget"]) +
                                               (Sign * Convert.ToDecimal(NewDataRow["LastYearBudget"]));
                SummaryRow["WholeYearBudget"] = Convert.ToDecimal(SummaryRow["WholeYearBudget"]) +
                                                (Sign * Convert.ToDecimal(NewDataRow["WholeYearBudget"]));
                SummaryRow["NextYearBudget"] = Convert.ToDecimal(SummaryRow["NextYearBudget"]) +
                                               (Sign * Convert.ToDecimal(NewDataRow["NextYearBudget"]));
            }
        } // Add To CostCentre Breakdown Summary

        /// <summary>
        /// For posting accounts in "details" view, the cost centre breakdown rows will be presented after one or more rows with the same account.
        /// The last account row will become a header, below, and any other rows with the same account will be removed.
        /// So I need the values in those rows to accumulate into the last row.
        /// </summary>
        /// <param name="DetailRow"></param>
        /// <param name="AccumulatingRow"></param>
        private static void AccumulateTotalsPerCostCentre(DataRow DetailRow, DataRow AccumulatingRow)
        {
            if (DetailRow["AccountPath"].ToString() != AccumulatingRow["AccountPath"].ToString())
            {
                AccumulatingRow["AccountPath"] = DetailRow["AccountPath"].ToString();
                AccumulatingRow["Actual"] = Convert.ToDecimal(DetailRow["Actual"]);
                AccumulatingRow["ActualYTD"] = Convert.ToDecimal(DetailRow["ActualYTD"]);
                AccumulatingRow["LastYearActualYtd"] = Convert.ToDecimal(DetailRow["LastYearActualYtd"]);
                AccumulatingRow["LastYearLastMonthYtd"] = Convert.ToDecimal(DetailRow["LastYearLastMonthYtd"]);
                AccumulatingRow["LastYearActual"] = Convert.ToDecimal(DetailRow["LastYearActual"]);
                AccumulatingRow["LastYearEnd"] = Convert.ToDecimal(DetailRow["LastYearEnd"]);
                AccumulatingRow["Budget"] = Convert.ToDecimal(DetailRow["Budget"]);
                AccumulatingRow["BudgetYTD"] = Convert.ToDecimal(DetailRow["BudgetYTD"]);
                AccumulatingRow["LastYearBudget"] = Convert.ToDecimal(DetailRow["LastYearBudget"]);
                AccumulatingRow["WholeYearBudget"] = Convert.ToDecimal(DetailRow["WholeYearBudget"]);
                AccumulatingRow["NextYearBudget"] = Convert.ToDecimal(DetailRow["NextYearBudget"]);
            }
            else
            {
                AccumulatingRow["Actual"] = Convert.ToDecimal(AccumulatingRow["Actual"]) + Convert.ToDecimal(DetailRow["Actual"]);
                AccumulatingRow["ActualYTD"] = Convert.ToDecimal(AccumulatingRow["ActualYTD"]) + Convert.ToDecimal(DetailRow["ActualYTD"]);
                AccumulatingRow["LastYearActualYtd"] = Convert.ToDecimal(AccumulatingRow["LastYearActualYtd"]) +
                                                       Convert.ToDecimal(DetailRow["LastYearActualYtd"]);
                AccumulatingRow["LastYearLastMonthYtd"] = Convert.ToDecimal(AccumulatingRow["LastYearLastMonthYtd"]) +
                                                          Convert.ToDecimal(DetailRow["LastYearLastMonthYtd"]);
                AccumulatingRow["LastYearActual"] = Convert.ToDecimal(AccumulatingRow["LastYearActual"]) +
                                                    Convert.ToDecimal(DetailRow["LastYearActual"]);
                AccumulatingRow["LastYearEnd"] = Convert.ToDecimal(AccumulatingRow["LastYearEnd"]) + Convert.ToDecimal(DetailRow["LastYearEnd"]);
                AccumulatingRow["Budget"] = Convert.ToDecimal(AccumulatingRow["Budget"]) + Convert.ToDecimal(DetailRow["Budget"]);
                AccumulatingRow["BudgetYTD"] = Convert.ToDecimal(AccumulatingRow["BudgetYTD"]) + Convert.ToDecimal(DetailRow["BudgetYTD"]);
                AccumulatingRow["LastYearBudget"] = Convert.ToDecimal(AccumulatingRow["LastYearBudget"]) +
                                                    Convert.ToDecimal(DetailRow["LastYearBudget"]);
                AccumulatingRow["WholeYearBudget"] = Convert.ToDecimal(AccumulatingRow["WholeYearBudget"]) +
                                                     Convert.ToDecimal(DetailRow["WholeYearBudget"]);
                AccumulatingRow["NextYearBudget"] = Convert.ToDecimal(AccumulatingRow["NextYearBudget"]) +
                                                    Convert.ToDecimal(DetailRow["NextYearBudget"]);

                DetailRow["Actual"] = Convert.ToDecimal(AccumulatingRow["Actual"]);
                DetailRow["ActualYTD"] = Convert.ToDecimal(AccumulatingRow["ActualYTD"]);
                DetailRow["LastYearActualYtd"] = Convert.ToDecimal(AccumulatingRow["LastYearActualYtd"]);
                DetailRow["LastYearLastMonthYtd"] = Convert.ToDecimal(AccumulatingRow["LastYearLastMonthYtd"]);
                DetailRow["LastYearActual"] = Convert.ToDecimal(AccumulatingRow["LastYearActual"]);
                DetailRow["LastYearEnd"] = Convert.ToDecimal(AccumulatingRow["LastYearEnd"]);
                DetailRow["Budget"] = Convert.ToDecimal(AccumulatingRow["Budget"]);
                DetailRow["BudgetYTD"] = Convert.ToDecimal(AccumulatingRow["BudgetYTD"]);
                DetailRow["LastYearBudget"] = Convert.ToDecimal(AccumulatingRow["LastYearBudget"]);
                DetailRow["WholeYearBudget"] = Convert.ToDecimal(AccumulatingRow["WholeYearBudget"]);
                DetailRow["NextYearBudget"] = Convert.ToDecimal(AccumulatingRow["NextYearBudget"]);
            }
        } // Accumulate Totals Per CostCentre

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// This version begins with GLM and GLMP tables, but calculates amounts for the summary accounts,
        /// so it does not rely on summarisation in GLMP.
        /// </summary>
        [NoRemoting]
        public static DataTable IncomeExpenseTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            /* Required columns:
             *   CostCentreCode
             *   CostCentreName
             *   AccountType
             *   AccountLevel
             *   HasChildren
             *   Breakdown
             *   ParentFooter
             *   AccountPath
             *   AccountCode
             *   AccountName
             *   AccountIsSummary
             *   YearStart
             *   Actual
             *   ActualYTD
             *   LastYearActual
             *   LastYearActualYtd
             *   LastYearEnd
             *   Budget
             *   BudgetYTD
             *   LastYearBudget
             *   WholeYearBudget
             *   NextYearBudget
             */


            /*
             *  Cost Centre Breakdown process, in English:
             *
             *  Find all the transactions for this period (and last month, last year) in glmp, sorting by Account, CostCentre
             *  For each account, re-calculate the summary accounts, generating parent records and AccountPath, using the given hierarchy
             *  Summarise to the required detail level by copying into new "breakdown" records:
             *      Headers and footers at a lower level are just copied,
             *      Accounts at the highest level must be made into header/footer pairs. The totals should be correct.
             *      all transactions at the required detail level or higher must be combined by CostCentreCode and listed within the appropriate level account.
             *
             *  The initial query and calculation of previous periods and budget figures is all the same; only the summarisation is different.
             */

            /*
             *  "Whole year breakdown by period" process, in English:
             *
             *  Find all the transactions for the whole year (to period 12) in glmp, sorting by CostCentre, Account
             *  For each account, summarise into 12 fields of summary accounts, generating parent records and AccountPath, using the given hierarchy
             *  Summarise to the required level of detail
             *  For each remaining posting account, create a "breakdown" record with 12 fields for the summation
             *  Remove all records that are not a summary or a breakdown
             */

            Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            Int32 NumberOfAccountingPeriods = new TLedgerInfo(LedgerNumber).NumberOfAccountingPeriods;
            Int32 AccountingYear = AParameters["param_year_i"].ToInt32();
            Int32 ReportPeriodStart = AParameters["param_start_period_i"].ToInt32();
            Int32 ReportPeriodEnd = AParameters["param_end_period_i"].ToInt32();
            String HierarchyName = AParameters["param_account_hierarchy_c"].ToString();

            Boolean International = AParameters["param_currency"].ToString().StartsWith("Int");
            Decimal exchangeRateNow = 1;
            Decimal LastYearExchangeRate = 1;

            if (International)
            {
                TCorporateExchangeRateCache ExchangeRateCache = new TCorporateExchangeRateCache();
                TLedgerInfo ledgerInfo = new TLedgerInfo(LedgerNumber);

                exchangeRateNow = ExchangeRateCache.GetCorporateExchangeRate(DbAdapter.FPrivateDatabaseObj,
                    LedgerNumber,
                    ledgerInfo.CurrentFinancialYear,
                    ledgerInfo.CurrentPeriod,
                    -1);
                LastYearExchangeRate = ExchangeRateCache.GetCorporateExchangeRate(DbAdapter.FPrivateDatabaseObj,
                    LedgerNumber,
                    AccountingYear - 1,
                    ReportPeriodEnd,
                    -1);
            }

            //
            // Read different DB fields according to currency setting
            Boolean CostCentreBreakdown = AParameters["param_cost_centre_breakdown"].ToBool();
            Boolean WholeYearPeriodsBreakdown = AParameters["param_period_breakdown"].ToBool();

            List <String>SelectedCostCentres = GetCostCentreList(AParameters, DbAdapter.FPrivateDatabaseObj);

            DataTable FilteredResults = new DataTable();
            FilteredResults.TableName = "IncomeExpense";

            TDBTransaction ReadTrans = null;
            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTrans,
                delegate
                {
                    String YearFilter = " AND glm.a_year_i>=" + (AccountingYear - 1) +   // Last year's values are needed.
                                        " AND glm.a_year_i<=" + (AccountingYear + 1);    // Next year can only contain budgets

                    String isThisYear = " Year=" + AccountingYear;
                    String isLastYear = " Year=" + (AccountingYear - 1);
                    String isNextYear = " Year=" + (AccountingYear + 1);

                    Int32 LastPeriod = Math.Max(ReportPeriodEnd,
                        NumberOfAccountingPeriods);                         // I need the whole year to see "whole year budget".
                    String PeriodFilter = " AND glmp.a_period_number_i<=" + LastPeriod;
                    String isEndPeriod = "Period=" + ReportPeriodEnd;
                    String isPrevPeriod = "Period=" + (ReportPeriodStart - 1);
                    String is12MothsBeforeEnd = "Period=" + (ReportPeriodEnd - 12);
                    String isLastPeriod = "Period=" + NumberOfAccountingPeriods;

                    String ActualYtdQuery = "SUM (CASE WHEN " + isThisYear + " AND " + isEndPeriod + " THEN ActualGLM ELSE 0 END) AS ActualTemp, ";

                    String PrevPeriodQuery = (ReportPeriodStart == 1) ?
                                             "AVG (CASE WHEN " + isThisYear + " THEN StartBalance ELSE 0 END) AS LastMonthTemp, "
                                             :
                                             "SUM (CASE WHEN " + isThisYear + " AND " + isPrevPeriod +
                                             " THEN ActualGLM ELSE 0 END) AS LastMonthTemp, ";

                    String LastYearActualYtdQuery = (ReportPeriodEnd > NumberOfAccountingPeriods) ? // After the end of the year I'm actually looking back to the beginning of this year!
                                                    "SUM (CASE WHEN " + isThisYear + " AND Period=" +
                                                    (ReportPeriodEnd -
                                                     NumberOfAccountingPeriods) + " THEN ActualGLM ELSE 0 END) AS LastYearActualYtd, "
                                                    :
                                                    "SUM (CASE WHEN " + isLastYear + " AND " + isEndPeriod +
                                                    " THEN ActualGLM ELSE 0 END) AS LastYearActualYtd, ";

                    String LastYearPrevPeriodQuery = (ReportPeriodEnd > NumberOfAccountingPeriods) ? // After the end of the year I'm actually looking back to the beginning of this year!
                                                     (ReportPeriodStart == NumberOfAccountingPeriods + 1) ?
                                                     "SUM (CASE WHEN " + isThisYear + " THEN StartBalance ELSE 0 END) AS LastYearLastMonthYtd, "
                                                     :
                                                     "SUM (CASE WHEN " + isThisYear + " AND Period=" +
                                                     (ReportPeriodStart +  - NumberOfAccountingPeriods -
                                                      1) + " THEN ActualGLM ELSE 0 END) AS LastYearLastMonthYtd, "
                                                     : (ReportPeriodStart == 1) ?
                                                     "AVG (CASE WHEN " + isLastYear + " THEN StartBalance ELSE 0 END) AS LastYearLastMonthYtd, "
                                                     :
                                                     "SUM (CASE WHEN " + isLastYear + " AND " + isPrevPeriod +
                                                     " THEN ActualGLM ELSE 0 END) AS LastYearLastMonthYtd, ";

                    String LastYearEndQuery = "SUM(CASE WHEN " + isLastYear + " AND " + isLastPeriod + " THEN ActualGLM ELSE 0 END) AS LastYearEnd,";
                    String BudgetQuery = (ReportPeriodEnd > NumberOfAccountingPeriods) ? // After the end of the year I can get next year's budget (if it's there!)
                                         "SUM (CASE WHEN " + isNextYear + " AND Period>=" + (ReportPeriodStart - NumberOfAccountingPeriods) +
                                         " AND Period <= " + (ReportPeriodEnd - NumberOfAccountingPeriods) +
                                         " THEN Budget ELSE 0 END) AS Budget, "
                                         :
                                         "SUM (CASE WHEN " + isThisYear + " AND Period>=" + ReportPeriodStart + " AND Period <= " + ReportPeriodEnd +
                                         " THEN Budget ELSE 0 END) AS Budget, ";

                    String BudgetYtdQuery = (ReportPeriodEnd > NumberOfAccountingPeriods) ? // After the end of the year I can get next year's budget (if it's there!)
                                            "SUM (CASE WHEN " + isNextYear + " AND Period<=" + (ReportPeriodEnd - NumberOfAccountingPeriods) +
                                            " THEN Budget ELSE 0 END) AS BudgetYTD, "
                                            :
                                            "SUM (CASE WHEN " + isThisYear + " AND Period<=" + ReportPeriodEnd +
                                            " THEN Budget ELSE 0 END) AS BudgetYTD, ";

                    String budgetWholeYearQuery = (ReportPeriodEnd > NumberOfAccountingPeriods) ? // After the end of the year it's next year's budget I'm showing
                                                  "SUM (CASE WHEN " + isNextYear + " THEN Budget ELSE 0 END) AS WholeYearBudget, "
                                                  :
                                                  "SUM (CASE WHEN " + isThisYear + " THEN Budget ELSE 0 END) AS WholeYearBudget, ";

                    String budgetLastYearQuery = (ReportPeriodEnd > NumberOfAccountingPeriods) ? // After the end of the year it's this year's budget I'm showing
                                                 "SUM (CASE WHEN " + isThisYear + " THEN Budget ELSE 0 END) AS LastYearBudget, "
                                                 :
                                                 "SUM (CASE WHEN " + isLastYear + " THEN Budget ELSE 0 END) AS LastYearBudget, ";
                    String budgetNextYearQuery = "SUM (CASE WHEN " + isNextYear + " THEN Budget ELSE 0 END) AS NextYearBudget, ";

                    String MonthlyBreakdownQuery =
                        "0.0 AS P1, 0.0 AS P2, 0.0 AS P3, 0.0 AS P4, 0.0 AS P5, 0.0 AS P6 , 0.0 AS P7, 0.0 AS P8, 0.0 AS P9, 0.0 AS P10, 0.0 AS P11, 0.0 AS P12 ";

                    String NoZeroesFilter =
                        "WHERE (LastMonthTemp != 0 OR ActualTemp != 0" +
                        " OR Budget != 0 OR BudgetYTD != 0 OR WholeYearBudget != 0 OR LastYearBudget != 0 OR LastYearLastMonthYtd != 0 OR LastYearActualYtd != 0)";

                    if (WholeYearPeriodsBreakdown)
                    {
                        //TODO: Calendar vs Financial Date Handling - Check if this should use financial num periods and not assume 12
                        CostCentreBreakdown = false;                            // Hopefully the client will have ensured this is false anyway - I'm just asserting it!
                        MonthlyBreakdownQuery =
                            "SUM (CASE WHEN Period=1 THEN ActualGLM ELSE 0 END) AS P1, " +
                            "SUM (CASE WHEN Period=2 THEN ActualGLM ELSE 0 END) AS P2, " +
                            "SUM (CASE WHEN Period=3 THEN ActualGLM ELSE 0 END) AS P3, " +
                            "SUM (CASE WHEN Period=4 THEN ActualGLM ELSE 0 END) AS P4, " +
                            "SUM (CASE WHEN Period=5 THEN ActualGLM ELSE 0 END) AS P5, " +
                            "SUM (CASE WHEN Period=6 THEN ActualGLM ELSE 0 END) AS P6, " +
                            "SUM (CASE WHEN Period=7 THEN ActualGLM ELSE 0 END) AS P7, " +
                            "SUM (CASE WHEN Period=8 THEN ActualGLM ELSE 0 END) AS P8, " +
                            "SUM (CASE WHEN Period=9 THEN ActualGLM ELSE 0 END) AS P9, " +
                            "SUM (CASE WHEN Period=10 THEN ActualGLM ELSE 0 END) AS P10, " +
                            "SUM (CASE WHEN Period=11 THEN ActualGLM ELSE 0 END) AS P11, " +
                            "SUM (CASE WHEN Period=12 THEN ActualGLM ELSE 0 END) AS P12 "; // No comma because this is the last field!

                        ActualYtdQuery = "0 AS ActualTemp, ";
                        PrevPeriodQuery = "0.0 AS LastMonthTemp, ";
                        LastYearActualYtdQuery = "0 AS LastYearActualYtd, ";
                        LastYearPrevPeriodQuery = "0.0 AS LastYearLastMonthYtd, ";
                        LastYearEndQuery = "0.0 AS LastYearEnd, ";
                        BudgetQuery = "0.0 AS Budget,";
                        BudgetYtdQuery = "0.0 AS BudgetYTD,";
                        budgetWholeYearQuery = "0.0 AS WholeYearBudget, ";
                        budgetLastYearQuery = "0.0 AS LastYearBudget, ";
                        budgetNextYearQuery = "0.0 AS NextYearBudget, ";

                        YearFilter = " AND glm.a_year_i=" + AccountingYear;
                        PeriodFilter = " AND glmp.a_period_number_i<=12";
                        NoZeroesFilter = "WHERE (P1<>0 OR P2<>0 OR P3<>0 OR P4<>0 OR P5<>0 OR P6<>0 " +
                                         "OR P7<>0 OR P8<>0 OR P9<>0 OR P10<>0 OR P11<>0 OR P12<>0) "; // No blank rows
                    }

                    TLogging.Log(Catalog.GetString("Loading data.."), TLoggingType.ToStatusBar);

                    //
                    // I can't use summary rows in GLM. Each Summary Cost Centre must be expressed as the sum of all the posting Cost Centres it represents.
                    // Accordingly, the query below is called for each Cost Centre, and the results appended into one table.

                    foreach (String ParentCC in SelectedCostCentres)
                    {
                        String[] Parts = ParentCC.Split(',');
                        String CostCentreFilter = GetReportingCostCentres(LedgerNumber, Parts[0], "");
                        CostCentreFilter = CostCentreFilter.Replace(",",
                            "','");                                                                          // SQL IN List items in single quotes
                        CostCentreFilter = " AND glm.a_cost_centre_code_c in ('" + CostCentreFilter + "') ";


                        String AllGlmp =                                                                     // This query fetches all the data I need from GLM and GLMP
                                         "(SELECT a_account.a_account_code_c AS AccountCode, a_account.a_account_type_c AS AccountType, " +
                                         "a_account.a_account_code_short_desc_c AS AccountName, " +
                                         "CASE a_account.a_account_type_c WHEN 'Income' THEN 1 WHEN 'Expense' THEN 2 END AS AccountTypeOrder, " +
                                         "a_account.a_debit_credit_indicator_l AS DebitCredit, " +
                                         "glm.a_year_i AS Year, " +
                                         "glm.a_start_balance_base_n AS StartBalance, " +
                                         "glm.a_closing_period_actual_base_n AS EndBalance, " +
                                         "glmp.a_period_number_i AS Period, " +
                                         "glmp.a_actual_base_n AS ActualGLM, " +
                                         "glmp.a_budget_base_n AS Budget " +
                                         "FROM a_general_ledger_master AS glm, a_general_ledger_master_period AS glmp, a_account " +
                                         "WHERE " +
                                         "glm.a_ledger_number_i=" + LedgerNumber + " " +
                                         YearFilter +
                                         PeriodFilter +
                                         " AND glm.a_glm_sequence_i = glmp.a_glm_sequence_i" +
                                         " AND a_account.a_account_code_c = glm.a_account_code_c" +
                                         " AND (a_account.a_account_type_c = 'Income' OR a_account.a_account_type_c = 'Expense') AND a_account.a_ledger_number_i = glm.a_ledger_number_i "
                                         +
                                         "AND a_account.a_posting_status_l = true " +
                                         CostCentreFilter +
                                         "AND (glmp.a_actual_base_n != 0 OR glmp.a_budget_base_n != 0) " +
                                         ") AS AllGlmp ";


                        String Summarised =                                                                  // This query reduces the result set from AllGlmp
                                            "(SELECT " +
                                            "AccountCode, AccountType, AccountName, DebitCredit, " +
                                            "SUM (CASE WHEN " + isThisYear + " THEN StartBalance ELSE 0 END) AS YearStart, " +
                                            "SUM (CASE WHEN " + isThisYear + " AND Period=" + NumberOfAccountingPeriods +
                                            " THEN ActualGLM ELSE 0 END) AS Period12End," +
                                            PrevPeriodQuery +
                                            ActualYtdQuery +
                                            LastYearActualYtdQuery +
                                            LastYearPrevPeriodQuery +
                                            LastYearEndQuery +
                                            BudgetQuery +
                                            BudgetYtdQuery +
                                            budgetWholeYearQuery +
                                            budgetLastYearQuery +
                                            budgetNextYearQuery +
                                            "AccountTypeOrder, " +
                                            MonthlyBreakdownQuery +
                                            "FROM " +
                                            AllGlmp +

                                            "GROUP BY AccountType, AccountCode, AccountName, DebitCredit, AccountTypeOrder " +
                                            ") AS Summarised ";


                        String Query = "SELECT " +                              // This query adds extra columns to Summarised

                                       " '" + Parts[0].Replace("'", "''") + "' AS CostCentreCode," +
                                       " '" + Parts[1].Replace("'",
                            "''") + "' AS CostCentreName," +

                                       "Summarised.*, " +
                                       (
                            (ReportPeriodEnd > NumberOfAccountingPeriods) ?
                            " ActualTemp  - Period12End AS ActualYtd, " +
                            " LastMonthTemp  - Period12End AS LastMonthYtd, "
                            :
                            " ActualTemp AS ActualYtd, " +
                            " LastMonthTemp AS LastMonthYtd, "
                                       ) +
                                       "ActualTemp - LastMonthTemp AS Actual, " +
                                       "LastYearActualYtd - LastYearLastMonthYtd AS LastYearActual, " +
                                       "1 AS AccountLevel, false AS HasChildren, false AS ParentFooter, false AS AccountIsSummary, false AS Breakdown,'Path' AS AccountPath "
                                       +

                                       "FROM " +
                                       Summarised +
                                       NoZeroesFilter +
                                       "ORDER BY AccountTypeOrder, AccountCode ";

                        FilteredResults.Merge(DbAdapter.RunQuery(Query, "IncomeExpense", ReadTrans));

                        if (DbAdapter.IsCancelled)
                        {
                            return;
                        }
                    } // foreach ParentCC

                    if (CostCentreBreakdown) // I need to re-order the resulting table by Account:
                    {
                        FilteredResults.DefaultView.Sort = "AccountCode";
                        FilteredResults = FilteredResults.DefaultView.ToTable();
                    }

                    //
                    // I only have "posting accounts" - I need to add the summary accounts.
                    TLogging.Log(Catalog.GetString("Summarise to parent accounts.."), TLoggingType.ToStatusBar);
                    AAccountHierarchyDetailTable HierarchyTbl = AAccountHierarchyDetailAccess.LoadViaAAccountHierarchy(LedgerNumber,
                        HierarchyName,
                        ReadTrans);

                    HierarchyTbl.DefaultView.Sort = "a_reporting_account_code_c";   // These two sort orders

                    if (CostCentreBreakdown)                                        // Are required by AddTotalsToParentAccountRow, below.
                    {
                        FilteredResults.DefaultView.Sort = "AccountCode";
                    }
                    else
                    {
                        FilteredResults.DefaultView.Sort = "CostCentreCode, AccountCode";
                    }

                    Int32 PostingAccountRecords = FilteredResults.Rows.Count;

                    for (Int32 Idx = 0; Idx < PostingAccountRecords; Idx++)
                    {
                        if (DbAdapter.IsCancelled)
                        {
                            return;
                        }

                        DataRow Row = FilteredResults.Rows[Idx];

                        if (WholeYearPeriodsBreakdown) // The query gave me YTD values; I need monthly actuals.
                        {
                            for (Int32 i = NumberOfAccountingPeriods; i > 1; i--)
                            {
                                Row["P" + i] = Convert.ToDecimal(Row["P" + i]) - Convert.ToDecimal(Row["P" + (i - 1)]);
                            }

                            Row["P1"] = Convert.ToDecimal(Row["P1"]) - Convert.ToDecimal(Row["YearStart"]);
                        }

                        String CostCentreParam = (CostCentreBreakdown) ? "" : Row["CostCentreCode"].ToString();
                        String ParentAccountPath;
                        Int32 ParentAccountTypeOrder;

                        Int32 AccountLevel = AddTotalsToParentAccountRow(
                            FilteredResults,
                            HierarchyTbl,
                            LedgerNumber,
                            CostCentreParam,
                            Row["AccountCode"].ToString(),
                            Row,
                            CostCentreBreakdown,
                            (WholeYearPeriodsBreakdown) ? NumberOfAccountingPeriods : 0,
                            out ParentAccountPath,
                            out ParentAccountTypeOrder,
                            ReadTrans);
                        Row["AccountLevel"] = AccountLevel;
                        Row["AccountPath"] = ParentAccountPath + "~" + Row["AccountCode"];
                    }

                    //
                    // Now if I re-order the result, and hide any rows that are empty or too detailed, it should be what I need!

                    Int32 DetailLevel = AParameters["param_nesting_depth"].ToInt32();

                    if (CostCentreBreakdown)
                    {
                        TLogging.Log(Catalog.GetString("Get Cost Centre Breakdown.."), TLoggingType.ToStatusBar);

                        // I'm creating additional "breakdown" records for the per-CostCentre breakdown, and potentially removing
                        // some records that were summed into those "breakdown" records.
                        FilteredResults.DefaultView.Sort = "AccountType DESC, AccountPath ASC, CostCentreCode";
                        FilteredResults.DefaultView.RowFilter = "Breakdown=false";

                        // At this point I need to add together any transactions in more detailed levels, summarising them by Cost Centre,
                        // and listing them under the account to which they relate:
                        DataView SummaryView = new DataView(FilteredResults);
                        SummaryView.Sort = "AccountTypeOrder, AccountPath ASC, CostCentreCode";
                        SummaryView.RowFilter = "Breakdown=true";

                        DataRow AccumulatingRow = FilteredResults.NewRow(); // This temporary row is not part of the result set - it's just a line of temporary vars.

                        for (Int32 RowIdx = 0; RowIdx < FilteredResults.DefaultView.Count; RowIdx++)
                        {
                            if (DbAdapter.IsCancelled)
                            {
                                return;
                            }

                            DataRow DetailRow = FilteredResults.DefaultView[RowIdx].Row;
                            AddToCostCentreBreakdownSummary(SummaryView, DetailLevel, DetailRow);

                            //
                            // For posting accounts in "details" view, the cost centre breakdown rows will be presented after one or more rows with the same account.
                            // The last account row will become a header, below, and any other rows with the same account will be removed.
                            // So I need the values in those rows to accumulate into the last row.

                            AccumulateTotalsPerCostCentre(DetailRow, AccumulatingRow);
                        }

                        FilteredResults.DefaultView.Sort = "AccountTypeOrder, AccountPath ASC, Breakdown, CostCentreCode";
                    } // if (CostCentreBreakdown)
                    else
                    {
                        FilteredResults.DefaultView.Sort = "CostCentreCode, AccountTypeOrder, AccountPath ASC";
                    }

                    FilteredResults.DefaultView.RowFilter = "AccountLevel<=" + DetailLevel.ToString(); // Nothing too detailed
                    FilteredResults = FilteredResults.DefaultView.ToTable("IncomeExpense");

                    //
                    // Finally, to make the hierarchical report possible,
                    // I want to include a note to show whether a row has child rows,
                    // and if it does, I'll copy this row to a new row, below the children, marking the new row as "footer".

                    TLogging.Log(Catalog.GetString("Format data for reporting.."), TLoggingType.ToStatusBar);

                    for (Int32 RowIdx = 0; RowIdx < FilteredResults.Rows.Count - 1; RowIdx++)
                    {
                        if (DbAdapter.IsCancelled)
                        {
                            return;
                        }

                        Int32 ParentAccountLevel = Convert.ToInt32(FilteredResults.Rows[RowIdx]["AccountLevel"]);
                        Boolean HasChildren = (Convert.ToInt32(FilteredResults.Rows[RowIdx + 1]["AccountLevel"]) > ParentAccountLevel)
                                              || (Convert.ToBoolean(FilteredResults.Rows[RowIdx]["Breakdown"]) == false
                                                  && Convert.ToBoolean(FilteredResults.Rows[RowIdx + 1]["Breakdown"]) == true);
                        FilteredResults.Rows[RowIdx]["HasChildren"] = HasChildren;

                        if (HasChildren)
                        {
                            if (CostCentreBreakdown)
                            {
                                //
                                // Header and footer rows do not have Cost Centres -
                                // The Cost Centre fields were used for sorting, but they're misleading so I'll remove them here:
                                FilteredResults.Rows[RowIdx]["CostCentreCode"] = "";
                                FilteredResults.Rows[RowIdx]["CostCentreName"] = "";
                            }

                            Int32 NextSiblingPos = -1;

                            for (Int32 ChildIdx = RowIdx + 2; ChildIdx < FilteredResults.Rows.Count; ChildIdx++)
                            {
                                if ((Convert.ToInt32(FilteredResults.Rows[ChildIdx]["AccountLevel"]) <= ParentAccountLevel)
                                    && (Convert.ToBoolean(FilteredResults.Rows[ChildIdx]["Breakdown"]) == false)) // This row is not a child of mine
                                {                                                                                 // so I insert my footer before here.
                                    NextSiblingPos = ChildIdx;
                                    break;
                                }
                            }

                            DataRow FooterRow = FilteredResults.NewRow();
                            DataUtilities.CopyAllColumnValues(FilteredResults.Rows[RowIdx], FooterRow);
                            FooterRow["ParentFooter"] = true;
                            FooterRow["HasChildren"] = false;

                            if (NextSiblingPos > 0)
                            {
                                FilteredResults.Rows.InsertAt(FooterRow, NextSiblingPos);
                            }
                            else
                            {
                                FilteredResults.Rows.Add(FooterRow);
                            }
                        }
                    }

                    // For "Cost Centre Breakdown", the only transactions I want to see are the "breakdown" rows I've added.
                    // Everything else is removed unless it's a header or footer:

                    if (CostCentreBreakdown)
                    {
                        FilteredResults.DefaultView.RowFilter = "Breakdown=true OR HasChildren=true OR ParentFooter=true";
                        FilteredResults = FilteredResults.DefaultView.ToTable("IncomeExpense");
                    }

                    if (exchangeRateNow != 1)
                    {
                        if (WholeYearPeriodsBreakdown)
                        {
                            foreach (DataRow Row in FilteredResults.Rows)
                            {
                                Row["p1"] = Convert.ToDecimal(Row["p1"]) * exchangeRateNow;
                                Row["p2"] = Convert.ToDecimal(Row["p2"]) * exchangeRateNow;
                                Row["p3"] = Convert.ToDecimal(Row["p3"]) * exchangeRateNow;
                                Row["p4"] = Convert.ToDecimal(Row["p4"]) * exchangeRateNow;
                                Row["p5"] = Convert.ToDecimal(Row["p5"]) * exchangeRateNow;
                                Row["p6"] = Convert.ToDecimal(Row["p6"]) * exchangeRateNow;
                                Row["p7"] = Convert.ToDecimal(Row["p7"]) * exchangeRateNow;
                                Row["p8"] = Convert.ToDecimal(Row["p8"]) * exchangeRateNow;
                                Row["p9"] = Convert.ToDecimal(Row["p9"]) * exchangeRateNow;
                                Row["p10"] = Convert.ToDecimal(Row["p10"]) * exchangeRateNow;
                                Row["p11"] = Convert.ToDecimal(Row["p11"]) * exchangeRateNow;
                                Row["p12"] = Convert.ToDecimal(Row["p12"]) * exchangeRateNow;
                            }
                        }
                        else
                        {
                            foreach (DataRow Row in FilteredResults.Rows)
                            {
                                Row["yearstart"] = Convert.ToDecimal(Row["yearstart"]) * exchangeRateNow;
                                Row["lastmonthytd"] = Convert.ToDecimal(Row["lastmonthytd"]) * exchangeRateNow;
                                Row["actualytd"] = Convert.ToDecimal(Row["actualytd"]) * exchangeRateNow;
                                Row["LastYearlastmonthytd"] = Convert.ToDecimal(Row["LastYearlastmonthytd"]) * exchangeRateNow;
                                Row["LastYearactualytd"] = Convert.ToDecimal(Row["LastYearactualytd"]) * exchangeRateNow;
                                Row["LastYearEnd"] = Convert.ToDecimal(Row["LastYearEnd"]) * LastYearExchangeRate;
                                Row["budget"] = Convert.ToDecimal(Row["budget"]) * exchangeRateNow;
                                Row["budgetytd"] = Convert.ToDecimal(Row["budgetytd"]) * exchangeRateNow;
                                Row["wholeyearbudget"] = Convert.ToDecimal(Row["wholeyearbudget"]) * exchangeRateNow;
                                Row["LastYearBudget"] = Convert.ToDecimal(Row["LastYearBudget"]) * LastYearExchangeRate;
                                Row["actual"] = Convert.ToDecimal(Row["actual"]) * exchangeRateNow;
                                Row["LastYearactual"] = Convert.ToDecimal(Row["LastYearactual"]) * exchangeRateNow;
                            }
                        }
                    }

                    TLogging.Log("", TLoggingType.ToStatusBar);
                }); // Get NewOrExisting AutoReadTransaction

            return FilteredResults;
        } // Income Expense Table

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable HosaGiftsTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataTable resultTable = null;
            TDBTransaction Transaction = null;

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    Boolean PersonalHosa = (AParameters["param_filter_cost_centres"].ToString() == "PersonalCostcentres");
                    Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
                    String CostCentreCodes = AParameters["param_cost_centre_codes"].ToString();

                    String LinkedCC_CCFilter = "";
                    String GiftDetail_CCfilter = "";

                    if (CostCentreCodes != "ALL")
                    {
                        LinkedCC_CCFilter = " AND LinkedCostCentre.a_cost_centre_code_c IN (" + CostCentreCodes + ") ";
                        GiftDetail_CCfilter = " AND GiftDetail.a_cost_centre_code_c IN (" + CostCentreCodes + ") ";
                    }

                    Int32 IchNumber = AParameters["param_ich_number"].ToInt32();

                    String DateFilter = "";

                    if (AParameters["param_period"].ToBool() == true)
                    {
                        Int32 periodYear = AParameters["param_year_i"].ToInt32();
                        Int32 periodStart = AParameters["param_start_period_i"].ToInt32();
                        Int32 periodEnd = AParameters["param_end_period_i"].ToInt32();
                        DateFilter = "AND GiftBatch.a_batch_year_i = " + periodYear;

                        if (periodStart == periodEnd)
                        {
                            DateFilter += (" AND GiftBatch.a_batch_period_i = " + periodStart + " ");
                        }
                        else
                        {
                            DateFilter += (" AND GiftBatch.a_batch_period_i >= " + periodStart +
                                           " AND GiftBatch.a_batch_period_i <= " + periodEnd + " ");
                        }
                    }
                    else
                    {
                        DateTime dateStart = AParameters["param_start_date"].ToDate();
                        DateTime dateEnd = AParameters["param_end_date"].ToDate();
                        DateFilter = "AND GiftBatch.a_gl_effective_date_d >= '" + dateStart.ToString("yyyy-MM-dd") + "'" +
                                     " AND GiftBatch.a_gl_effective_date_d <= '" + dateEnd.ToString("yyyy-MM-dd") + "' ";
                    }

                    bool TaxDeductiblePercentageEnabled =
                        TSystemDefaults.GetBooleanDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, false);

                    String Query = string.Empty;

                    // If tax deductibility % is enabled then we need to use two querys - one for tax deduct and one for non-tax deduct.
                    // The results from these queries are then merged together.
                    if (TaxDeductiblePercentageEnabled)
                    {
                        Query = "SELECT ";

                        if (PersonalHosa)
                        {
                            Query += "UnionTable.CostCentre AS CostCentre, ";
                        }
                        else
                        {
                            Query += "UnionTable.CostCentre AS CostCentre, ";
                        }

                        Query +=
                            "UnionTable.AccountCode AS AccountCode, " +
                            "SUM(UnionTable.GiftBaseAmount) AS GiftBaseAmount, " +
                            "SUM(UnionTable.GiftIntlAmount) AS GiftIntlAmount, " +
                            "SUM(UnionTable.GiftTransactionAmount) AS GiftTransactionAmount, " +
                            "UnionTable.RecipientKey AS RecipientKey, " +
                            "UnionTable.RecipientShortname AS RecipientShortname, " +
                            "UnionTable.Narrative AS Narrative " +

                            "FROM (" +
                            GetHOSASQLQuery(true, false, PersonalHosa, LedgerNumber, DateFilter, IchNumber, LinkedCC_CCFilter, GiftDetail_CCfilter) +
                            " UNION ALL " +
                            GetHOSASQLQuery(true,
                                true,
                                PersonalHosa,
                                LedgerNumber,
                                DateFilter,
                                IchNumber,
                                LinkedCC_CCFilter,
                                GiftDetail_CCfilter) +
                            ") AS UnionTable " +

                            "GROUP BY Uniontable.Narrative, UnionTable.CostCentre, UnionTable.AccountCode, UnionTable.RecipientKey, UnionTable.RecipientShortname ";
                    }
                    else
                    {
                        Query =
                            GetHOSASQLQuery(false, false, PersonalHosa, LedgerNumber, DateFilter, IchNumber, LinkedCC_CCFilter, GiftDetail_CCfilter);
                    }

                    Query += " ORDER BY CostCentre, AccountCode, RecipientShortname ASC";

                    TLogging.Log(Catalog.GetString("Loading data.."), TLoggingType.ToStatusBar);
                    resultTable = DbAdapter.RunQuery(Query, "Gifts", Transaction);

                    resultTable.Columns.Add("Reference", typeof(string));

                    foreach (DataRow r in resultTable.Rows)
                    {
                        if (DbAdapter.IsCancelled)
                        {
                            return;
                        }

                        r["Reference"] = StringHelper.PartnerKeyToStr(Convert.ToInt64(r["RecipientKey"]));
                    }

                    TLogging.Log("", TLoggingType.ToStatusBar);
                    return;
                }); // Get NewOrExisting AutoReadTransaction
            return resultTable;
        } // Hosa Gifts Table

        /// <summary>
        /// Gets single HOSA query.
        /// </summary>
        /// <param name="ATaxDeductEnabled">True if tax deductible % is enabled</param>
        /// <param name="ATaxDeductCycle">True if we are looking for tax deductible amount / false if looking for non-tax deductible amount</param>
        /// <param name="APersonalHosa"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADateFilter"></param>
        /// <param name="AIchNumber"></param>
        /// <param name="ALinkedCC_CCFilter"></param>
        /// <param name="AGiftDetail_CCfilter"></param>
        /// <returns></returns>
        private static string GetHOSASQLQuery(bool ATaxDeductEnabled,
            bool ATaxDeductCycle,
            bool APersonalHosa,
            Int32 ALedgerNumber,
            string ADateFilter,
            Int32 AIchNumber,
            string ALinkedCC_CCFilter,
            string AGiftDetail_CCfilter)
        {
            String Query = "SELECT ";

            if (APersonalHosa)
            {
                Query += "LinkedCostCentre.a_cost_centre_code_c AS CostCentre, ";
            }
            else
            {
                Query += "GiftDetail.a_cost_centre_code_c AS CostCentre, ";
            }

            if (ATaxDeductEnabled)
            {
                if (ATaxDeductCycle)
                {
                    Query += "GiftDetail.a_tax_deductible_account_code_c AS AccountCode, " +
                             "SUM(GiftDetail.a_tax_deductible_amount_base_n) AS GiftBaseAmount, " +
                             "SUM(GiftDetail.a_tax_deductible_amount_intl_n) AS GiftIntlAmount, " +
                             "SUM(GiftDetail.a_tax_deductible_amount_n) AS GiftTransactionAmount, ";
                }
                else
                {
                    // If % is 0 then tax deduct field might not actually be filled in for this gift (i.e. old gifts).
                    // Even if the fields are filled in they will be same as the standard fields anyway. So use them.
                    Query += "GiftDetail.a_account_code_c AS AccountCode, " +
                             "CASE WHEN GiftDetail.a_tax_deductible_pct_n = 0  " +
                             "THEN SUM(GiftDetail.a_gift_amount_n) " +
                             "ELSE SUM(GiftDetail.a_non_deductible_amount_base_n) END AS GiftBaseAmount, " +
                             "CASE WHEN GiftDetail.a_tax_deductible_pct_n = 0  " +
                             "THEN SUM(GiftDetail.a_gift_amount_intl_n) " +
                             "ELSE SUM(GiftDetail.a_non_deductible_amount_intl_n) END AS GiftIntlAmount, " +
                             "CASE WHEN GiftDetail.a_tax_deductible_pct_n = 0  " +
                             "THEN SUM(GiftDetail.a_gift_transaction_amount_n) " +
                             "ELSE SUM(GiftDetail.a_non_deductible_amount_n) END AS GiftTransactionAmount, ";
                }
            }
            else
            {
                Query += "GiftDetail.a_account_code_c AS AccountCode, " +
                         "SUM(GiftDetail.a_gift_amount_n) AS GiftBaseAmount, " +
                         "SUM(GiftDetail.a_gift_amount_intl_n) AS GiftIntlAmount, " +
                         "SUM(GiftDetail.a_gift_transaction_amount_n) AS GiftTransactionAmount, ";
            }

            Query +=
                "GiftDetail.p_recipient_key_n AS RecipientKey, " +
                "Partner.p_partner_short_name_c AS RecipientShortname, " +
                "Partner.p_partner_short_name_c AS Narrative " +

                "FROM a_gift_detail AS GiftDetail, a_gift_batch AS GiftBatch, " +
                "a_motivation_detail AS MotivationDetail, " +
                "p_partner AS Partner";

            if (APersonalHosa)
            {
                Query += ",PUB_a_valid_ledger_number AS LinkedCostCentre";
            }

            Query += " WHERE GiftDetail.a_ledger_number_i = GiftBatch.a_ledger_number_i " +
                     "AND GiftDetail.a_batch_number_i = GiftBatch.a_batch_number_i " +
                     "AND GiftDetail.a_ledger_number_i = MotivationDetail.a_ledger_number_i " +
                     "AND GiftDetail.a_motivation_group_code_c = MotivationDetail.a_motivation_group_code_c " +
                     "AND GiftDetail.a_motivation_detail_code_c = MotivationDetail.a_motivation_detail_code_c " +
                     "AND Partner.p_partner_key_n = GiftDetail.p_recipient_key_n " +
                     "AND GiftDetail.a_ledger_number_i = " + ALedgerNumber + " " +
                     "AND GiftBatch.a_batch_status_c = '" + MFinanceConstants.BATCH_POSTED + "' " +
                     ADateFilter;

            if (APersonalHosa)
            {
                Query += "AND LinkedCostCentre.a_ledger_number_i = GiftDetail.a_ledger_number_i " +
                         ALinkedCC_CCFilter +
                         "AND GiftDetail.p_recipient_key_n = LinkedCostCentre.p_partner_key_n ";
            }
            else
            {
                Query += AGiftDetail_CCfilter;
            }

            if (AIchNumber != 0)
            {
                Query += "AND GiftDetail.a_ich_number_i = " + AIchNumber + " ";
            }

            // if looking for tax deductible gifts then the percentage must be over 0
            // (otherwise we could get accounts with 0 amount)
            if (ATaxDeductEnabled && ATaxDeductCycle)
            {
                Query += "AND GiftDetail.a_tax_deductible_pct_n > 0 ";
            }

            // if looking for non-tax deductible gifts then the percentage must be less than 100
            // (otherwise we could get accounts with 0 amount)
            if (ATaxDeductEnabled && !ATaxDeductCycle)
            {
                Query += "AND GiftDetail.a_tax_deductible_pct_n <> 100 ";
            }

            Query += "GROUP BY CostCentre, AccountCode, GiftDetail.p_recipient_key_n, Partner.p_partner_short_name_c";

            if (ATaxDeductEnabled && !ATaxDeductCycle)
            {
                Query += ", GiftDetail.a_tax_deductible_pct_n";
            }

            return Query;
        }

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable KeyMinGiftsTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataTable resultTable = null;
            TDBTransaction Transaction = null;

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
                    String CostCentreCodes = AParameters["param_cost_centre_codes"].ToString();

                    String CCfilter = "";

                    if (CostCentreCodes == "ALL")
                    {
                        CCfilter = "AND CostCentre.a_cost_centre_code_c=GiftDetail.a_cost_centre_code_c " +
                                   "AND CostCentre.a_cost_centre_type_c='" + MFinanceConstants.FOREIGN_CC_TYPE + "' " +
                                   "AND CostCentre.a_ledger_number_i=" + LedgerNumber + " ";
                    }
                    else
                    {
                        CCfilter = " AND GiftDetail.a_cost_centre_code_c IN (" + CostCentreCodes + ") ";
                    }

                    Int32 IchNumber = AParameters["param_ich_number"].ToInt32();

                    String DateFilter = "";

                    if (AParameters["param_period"].ToBool() == true)
                    {
                        Int32 periodYear = AParameters["param_year_i"].ToInt32();
                        Int32 periodStart = AParameters["param_start_period_i"].ToInt32();
                        Int32 periodEnd = AParameters["param_end_period_i"].ToInt32();
                        DateFilter = "AND GiftBatch.a_batch_year_i = " + periodYear;

                        if (periodStart == periodEnd)
                        {
                            DateFilter += (" AND GiftBatch.a_batch_period_i = " + periodStart + " ");
                        }
                        else
                        {
                            DateFilter += (" AND GiftBatch.a_batch_period_i >= " + periodStart +
                                           " AND GiftBatch.a_batch_period_i <= " + periodEnd + " ");
                        }
                    }
                    else
                    {
                        DateTime dateStart = AParameters["param_start_date"].ToDate();
                        DateTime dateEnd = AParameters["param_end_date"].ToDate();
                        DateFilter = "AND GiftBatch.a_gl_effective_date_d >= '" + dateStart.ToString("yyyy-MM-dd") + "'" +
                                     " AND GiftBatch.a_gl_effective_date_d <= '" + dateEnd.ToString(
                            "yyyy-MM-dd") + "' ";
                    }

                    String Query = "SELECT " +
                                   "GiftBatch.a_gl_effective_date_d AS date, " +
                                   "GiftBatch.a_batch_number_i AS BatchNumber, " +
                                   "Gift.a_gift_transaction_number_i AS TransactionNumber, " +
                                   "GiftDetail.a_cost_centre_code_c AS CostCentreCode, " +
                                   "GiftDetail.a_gift_amount_n AS GiftBaseAmount, " +
                                   "GiftDetail.a_gift_amount_intl_n AS GiftIntlAmount, " +
                                   "CASE WHEN GiftDetail.a_recipient_ledger_number_n=GiftDetail.p_recipient_key_n THEN 'FIELD' ELSE 'KEYMIN' END AS RecipientType, "
                                   +
                                   "GiftDetail.p_recipient_key_n AS RecipientKey, " +
                                   "Recipient.p_partner_short_name_c AS RecipientShortname, " +
                                   "Donor.p_partner_key_n AS DonorKey, " +
                                   "Donor.p_partner_short_name_c AS DonorShortname, " +
                                   "CASE WHEN GiftDetail.a_comment_one_type_c='Donor' THEN '' ELSE GiftDetail.a_gift_comment_one_c END AS Comment1, "
                                   +
                                   "CASE WHEN GiftDetail.a_comment_two_type_c='Donor' THEN '' ELSE GiftDetail.a_gift_comment_two_c END AS Comment2, "
                                   +
                                   "CASE WHEN GiftDetail.a_comment_three_type_c='Donor' THEN '' ELSE GiftDetail.a_gift_comment_three_c END AS Comment3 "
                                   +
                                   "FROM a_gift_detail AS GiftDetail, a_gift AS Gift, a_gift_batch AS GiftBatch, ";

                    if (CostCentreCodes == "ALL")
                    {
                        Query += "a_cost_centre AS CostCentre, ";
                    }

                    Query += "p_partner AS Donor, p_partner AS Recipient " +

                             "WHERE GiftBatch.a_ledger_number_i = " + LedgerNumber + " " +
                             "AND GiftDetail.a_batch_number_i = GiftBatch.a_batch_number_i " +
                             "AND (GiftDetail.a_recipient_ledger_number_n = GiftDetail.p_recipient_key_n " + // Field Gifts
                             "OR (SELECT COUNT(p_partner_key_n) FROM p_unit WHERE p_unit.p_partner_key_n=GiftDetail.p_recipient_key_n AND p_unit.u_unit_type_code_c='KEY-MIN') > 0) "
                             +
                             "AND Gift.a_ledger_number_i = " + LedgerNumber + " " +
                             "AND Gift.a_batch_number_i = GiftDetail.a_batch_number_i " +
                             "AND Gift.a_gift_transaction_number_i = GiftDetail.a_gift_transaction_number_i " +
                             CCfilter +
                             "AND Donor.p_partner_key_n = Gift.p_donor_key_n " +
                             "AND Recipient.p_partner_key_n = GiftDetail.p_recipient_key_n " +
                             "AND GiftDetail.a_ledger_number_i = " + LedgerNumber + " " +
                             "AND GiftBatch.a_batch_status_c = '" + MFinanceConstants.BATCH_POSTED + "' " +
                             DateFilter;

                    if (IchNumber != 0)
                    {
                        Query += "AND GiftDetail.a_ich_number_i = " + IchNumber + " ";
                    }

                    Query += "ORDER BY CostCentreCode, RecipientType Desc, RecipientKey, date, BatchNumber, TransactionNumber";


                    TLogging.Log(Catalog.GetString("Loading data.."), TLoggingType.ToStatusBar);
                    resultTable = DbAdapter.RunQuery(Query, "Recipient", Transaction);
                    TLogging.Log("", TLoggingType.ToStatusBar);
                    return;
                }); // Get NewOrExisting AutoReadTransaction

            return resultTable;
        } // KeyMin Gifts Table

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable StewardshipTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataTable resultsTable = null;
            TDBTransaction Transaction = null;

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    TLogging.Log(Catalog.GetString("Loading data.."), TLoggingType.ToStatusBar);
                    Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
                    Int32 IchNumber = AParameters["param_cmbICHNumber"].ToInt32();
                    Int32 period = AParameters["param_cmbReportPeriod"].ToInt32();
                    Int32 Year = AParameters["param_cmbYearEnding"].ToInt32();
                    Int32 CurrentFinancialYear = ALedgerAccess.LoadByPrimaryKey(LedgerNumber, Transaction)[0].CurrentFinancialYear;
                    string AccountHierarchyCode = MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD;
                    bool BaseCurrency = AParameters["param_currency"].ToString() == "Base";
                    String Query = string.Empty;

                    if (CurrentFinancialYear == Year) // if current year
                    {
                        string IncomeAmount = string.Empty;
                        string ExpenseAmount = string.Empty;
                        string XferAmount = string.Empty;

                        if (BaseCurrency)
                        {
                            IncomeAmount = "a_income_amount_n";
                            ExpenseAmount = "a_expense_amount_n";
                            XferAmount = "a_direct_xfer_amount_n";
                        }
                        else
                        {
                            IncomeAmount = "a_income_amount_intl_n";
                            ExpenseAmount = "a_expense_amount_intl_n";
                            XferAmount = "a_direct_xfer_amount_intl_n";
                        }

                        String StewardshipFilter = "a_ich_stewardship.a_ledger_number_i = " + LedgerNumber;

                        if (IchNumber == 0)
                        {
                            StewardshipFilter += " AND a_ich_stewardship.a_period_number_i = " + period;
                        }
                        else
                        {
                            StewardshipFilter += " AND a_ich_stewardship.a_ich_number_i = " + IchNumber;
                        }

                        Query = "SELECT" +
                                " a_ich_stewardship.a_cost_centre_code_c AS CostCentreCode, " +
                                " a_cost_centre.a_cost_centre_name_c AS CostCentreName, " +
                                " sum(a_ich_stewardship." + IncomeAmount + ") AS Income, " +
                                " sum(a_ich_stewardship." + ExpenseAmount + ") AS Expense, " +
                                " sum(a_ich_stewardship." + XferAmount + ") AS Xfer" +
                                " FROM a_ich_stewardship, a_cost_centre WHERE " +
                                StewardshipFilter +
                                " AND a_cost_centre.a_ledger_number_i = a_ich_stewardship.a_ledger_number_i" +
                                " AND a_cost_centre.a_cost_centre_code_c = a_ich_stewardship.a_cost_centre_code_c " +
                                " AND a_cost_centre.a_cost_centre_type_c = '" + MFinanceConstants.FOREIGN_CC_TYPE + "'" +
                                " AND a_cost_centre.a_clearing_account_c = '" + MFinanceConstants.ICH_ACCT_ICH + "'" +
                                " GROUP BY CostCentreCode, CostCentreName " +
                                " ORDER BY CostCentreCode";
                    }
                    else  // if past year
                    {
                        string ActualCurrency = string.Empty;

                        if (BaseCurrency)
                        {
                            ActualCurrency = "a_actual_base_n";
                        }
                        else
                        {
                            ActualCurrency = "a_actual_intl_n";
                        }

                        string Actual = "GLMP1." + ActualCurrency;

                        // if period is not 1 then we need to subract the actual for the previous period from the actual for the current period
                        if (period > 1)
                        {
                            Actual = "(" + Actual + " - GLMP2." + ActualCurrency + ")";
                        }

                        // obtain accounts that report to account INC
                        string IncomeAccountsString =
                            GetFormattedReportingAccounts(LedgerNumber, MFinanceConstants.INCOME_HEADING, AccountHierarchyCode);

                        // obtain accounts that report to account EXP
                        string ExpenseAccountsString =
                            GetFormattedReportingAccounts(LedgerNumber, MFinanceConstants.EXPENSE_HEADING, AccountHierarchyCode);

                        Query = "SELECT" +
                                " a_cost_centre.a_cost_centre_code_c AS CostCentreCode, " +
                                " a_cost_centre.a_cost_centre_name_c AS CostCentreName, " +

                                /* Revenue: Income received for the foreign ledger. */
                                " (SUM(CASE WHEN GLM.a_account_code_c IN (" + IncomeAccountsString + ")" +
                                " THEN " + Actual + " ELSE 0 END)) AS Income, " +

                                /* Expenses: Fees & other expenses charged to the foreign ledger. */

                                // Get "Direct Transfer" information. Set up for money that is not sent
                                // through the clearing house but directly to a field. Really an expense account.
                                " (SUM(CASE WHEN GLM.a_account_code_c = '" + MFinanceConstants.DIRECT_XFER_ACCT + "'" +
                                " THEN " + Actual + " ELSE 0 END)) AS Xfer, " +

                                // Get other expense information. Lookup in the gl master file the
                                // summary heading of total EXPENSES for the entire cost centre.
                                " ((SUM(CASE WHEN GLM.a_account_code_c IN (" + ExpenseAccountsString + ")" +
                                " THEN " + Actual + " ELSE 0 END)) - " +
                                // Subtract "Direct Transfer" information.
                                " (SUM(CASE WHEN GLM.a_account_code_c = '" + MFinanceConstants.DIRECT_XFER_ACCT + "'" +
                                " THEN " + Actual + " ELSE 0 END)) - " +
                                // Subtract "ICH Settlement" information. The account used to balance out each foreign cost centre at the period end.
                                // Set up as an expense account and thus must be removed from the total expenses.
                                " (SUM(CASE WHEN GLM.a_account_code_c = '" + MFinanceConstants.ICH_ACCT_SETTLEMENT + "'" +
                                " THEN " + Actual + " ELSE 0 END))) " + " AS Expense " +

                                " FROM a_cost_centre" +

                                " INNER JOIN a_general_ledger_master AS GLM" +
                                " ON GLM.a_cost_centre_code_c = a_cost_centre.a_cost_centre_code_c" +
                                " AND GLM.a_account_code_c IN (" + IncomeAccountsString + ", " + ExpenseAccountsString + ", '" +
                                MFinanceConstants.DIRECT_XFER_ACCT + "', '" + MFinanceConstants.ICH_ACCT_SETTLEMENT + "')" +
                                " AND GLM.a_year_i = " + Year +

                                " INNER JOIN a_general_ledger_master_period AS GLMP1" +
                                " ON GLMP1.a_glm_sequence_i = GLM.a_glm_sequence_i" +
                                " AND GLMP1.a_period_number_i = " + period;

                        // if needed also get the GLMP record for the previous period
                        if (period > 1)
                        {
                            Query += " INNER JOIN a_general_ledger_master_period AS GLMP2" +
                                     " ON GLMP2.a_glm_sequence_i = GLM.a_glm_sequence_i" +
                                     " AND GLMP2.a_period_number_i = " + (period - 1);
                        }

                        Query += " WHERE " +
                                 " a_cost_centre.a_ledger_number_i = " + LedgerNumber +
                                 " AND a_cost_centre.a_cost_centre_type_c = '" + MFinanceConstants.FOREIGN_CC_TYPE + "'" +
                                 " AND a_cost_centre.a_clearing_account_c = '" + MFinanceConstants.ICH_ACCT_ICH + "'" +
                                 " GROUP BY CostCentreCode, CostCentreName " +
                                 " ORDER BY CostCentreCode";
                    }

                    TLogging.Log(Catalog.GetString(""), TLoggingType.ToStatusBar);
                    resultsTable = DbAdapter.RunQuery(Query, "Stewardship", Transaction);
                }); // Get NewOrExisting AutoReadTransaction
            return resultsTable;
        } // Stewardship Table

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable FeesTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataTable resultsTable = null;
            TDBTransaction Transaction = null;

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    TLogging.Log(Catalog.GetString("Loading data.."), TLoggingType.ToStatusBar);
                    Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
                    Int32 period = AParameters["param_cmbReportPeriod"].ToInt32();
                    Int32 YearNumber = AParameters["param_cmbYearEnding"].ToInt32();
                    String[] SelectedFees = AParameters["param_fee_codes"].ToString().Split(',');
                    Int32 FeeCols = SelectedFees.Length;

                    // Full report lists every gift transaction for all cost centres
                    // Summary report groups and summarises gifts with a common Foreign cost centre
                    bool FullReport = AParameters["param_rgrFees"].ToString() == "ByGiftDetail";

                    //
                    // This constant is copied from the client - it represents a reasonable maximum
                    // number of columns in the report, and hopefully it is as many as any field needs.
                    Int32 MAX_FEE_COUNT = 11;

                    String Query = "SELECT ";

                    if (!FullReport)
                    {
                        Query += "a_cost_centre.a_cost_centre_code_c AS CostCentreCode, " +
                                 "a_cost_centre.a_cost_centre_name_c AS CostCentreName," +
                                 "0 AS BatchNumber, " +
                                 "0 AS TransactionNumber, " +
                                 "0 AS DetailNumber, " +
                                 "0 AS Field, " +

                                 "(SELECT SUM(GiftDetail2.a_gift_amount_n) FROM a_gift_detail AS GiftDetail2, a_gift_batch AS GiftBatch2" +
                                 " WHERE GiftDetail2.a_ledger_number_i = " + LedgerNumber +
                                 " AND GiftDetail2.a_cost_centre_code_c = a_cost_centre.a_cost_centre_code_c" +
                                 " AND GiftBatch2.a_ledger_number_i = GiftDetail2.a_ledger_number_i" +
                                 " AND GiftBatch2.a_batch_number_i = GiftDetail2.a_batch_number_i" +
                                 " AND GiftBatch2.a_batch_year_i = " + YearNumber + " AND GiftBatch2.a_batch_period_i = " + period +
                                 ") AS GiftAmount";
                    }
                    else
                    {
                        Query += "a_gift_detail.a_gift_amount_n AS GiftAmount, " +
                                 "a_gift_detail.a_batch_number_i AS BatchNumber, " +
                                 "a_gift_detail.a_gift_transaction_number_i AS TransactionNumber, " +
                                 "a_gift_detail.a_detail_number_i AS DetailNumber, " +
                                 "a_gift_detail.a_recipient_ledger_number_n AS Field, " +
                                 "0 AS CostCentreCode, " +
                                 "0 AS CostCentreName";
                    }

                    for (Int32 Idx = 0; Idx < MAX_FEE_COUNT; Idx++)
                    {
                        if (Idx < FeeCols)
                        {
                            Query += ", SUM(CASE WHEN (a_processed_fee.a_fee_code_c  = '" + SelectedFees[Idx] +
                                     "') THEN a_processed_fee.a_periodic_amount_n ELSE 0 END)as F" + Idx;
                        }
                        else // I'm always providing {MAX_FEE_COUNT} columns - some may be blank at RHS.
                        {
                            Query += ", 0 as F" + Idx;
                        }
                    }

                    Query += " FROM a_gift_batch, a_gift_detail " +

                             "LEFT JOIN a_processed_fee " +
                             "ON a_processed_fee.a_ledger_number_i = " + LedgerNumber +
                             " AND a_processed_fee.a_period_number_i = " + period +
                             " AND a_processed_fee.a_batch_number_i = a_gift_detail.a_batch_number_i" +
                             " AND a_processed_fee.a_gift_transaction_number_i = a_gift_detail.a_gift_transaction_number_i" +
                             " AND a_processed_fee.a_detail_number_i = a_gift_detail.a_detail_number_i ";

                    if (!FullReport)
                    {
                        Query += "JOIN a_cost_centre " +
                                 "ON a_cost_centre.a_ledger_number_i = a_processed_fee.a_ledger_number_i " +
                                 "AND a_cost_centre.a_cost_centre_code_c = a_processed_fee.a_cost_centre_code_c " +
                                 "AND a_cost_centre.a_cost_centre_type_c = 'Foreign' ";
                    }

                    Query += "WHERE " +
                             "a_gift_batch.a_ledger_number_i = " + LedgerNumber +
                             " AND a_gift_batch.a_batch_year_i = " + YearNumber + " AND a_gift_batch.a_batch_period_i = " + period +
                             " AND a_gift_detail.a_ledger_number_i = " + LedgerNumber +
                             " AND a_gift_batch.a_batch_number_i = a_gift_detail.a_batch_number_i ";

                    if (!FullReport)
                    {
                        Query += "GROUP BY CostCentreCode, CostCentreName " +
                                 "ORDER BY CostCentreCode";
                    }
                    else
                    {
                        Query += "GROUP BY GiftAmount, BatchNumber, TransactionNumber, DetailNumber, Field " +
                                 "ORDER BY BatchNumber, TransactionNumber, DetailNumber";
                    }

                    TLogging.Log(Catalog.GetString(""), TLoggingType.ToStatusBar);
                    resultsTable = DbAdapter.RunQuery(Query, "Fees", Transaction);
                }); // Get NewOrExisting AutoReadTransaction

            return resultsTable;
        } // Fees Table

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable StewardshipForPeriodTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataTable Result = null;
            TDBTransaction Transaction = null;
            Int32 Year = AParameters["param_year_i"].ToInt32();
            Int32 startPeriod = AParameters["param_start_period_i"].ToInt32();
            Int32 endPeriod = AParameters["param_end_period_i"].ToInt32();
            Int32 ledgerNumber = AParameters["param_ledger_number_i"].ToInt32();


            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    String ichSettlementAccount = MFinanceConstants.ICH_ACCT_SETTLEMENT;
                    TGetAccountHierarchyDetailInfo AccountInfo = new TGetAccountHierarchyDetailInfo(ledgerNumber);
                    List <String>incomeAccounts = AccountInfo.GetChildren(MFinanceConstants.INCOME_HEADING, true);
                    incomeAccounts.Remove(ichSettlementAccount);
                    List <String>expenseAccounts = AccountInfo.GetChildren(MFinanceConstants.EXPENSE_HEADING, true);
                    expenseAccounts.Remove(ichSettlementAccount);
                    expenseAccounts.Remove(MFinanceConstants.DIRECT_XFER_ACCT);

                    String incomeAccountList = "('" +
                                               String.Join("','", incomeAccounts.ToArray()) +
                                               "')";
                    String expenseAccountList = "('" +
                                                String.Join("','", expenseAccounts.ToArray()) +
                                                "')";
                    String incomeAndExpenseAccountList = "('" +
                                                         String.Join("','", incomeAccounts.ToArray()) + "','" +
                                                         String.Join("','",
                        expenseAccounts.ToArray()) +
                                                         "','" + MFinanceConstants.DIRECT_XFER_ACCT +
                                                         "')";
                    String openingBalanceIncomeCcSelect = "SUM (CASE WHEN GLM.a_account_code_c IN " + incomeAccountList +
                                                          " THEN GLM.a_start_balance_base_n ELSE 0 END)";
                    String openingBalanceExpenseCcSelect = "SUM (CASE WHEN GLM.a_account_code_c IN " + expenseAccountList +
                                                           " THEN GLM.a_start_balance_base_n ELSE 0 END)";
                    String openingBalanceXferCcSelect = "SUM (CASE WHEN GLM.a_account_code_c = '" + MFinanceConstants.DIRECT_XFER_ACCT +
                                                        "' THEN GLM.a_start_balance_base_n ELSE 0 END)";
                    String periodFilter = " AND GLMP.a_period_number_i=" + endPeriod;

                    if (startPeriod > 1)
                    {
                        startPeriod -= 1;
                        openingBalanceIncomeCcSelect = " SUM(CASE WHEN (GLMP.a_period_number_i=" + startPeriod + " AND GLM.a_account_code_c IN " +
                                                       incomeAccountList + ") THEN GLMP.a_actual_base_n ELSE 0 END)";
                        openingBalanceExpenseCcSelect = " SUM(CASE WHEN (GLMP.a_period_number_i=" + startPeriod + " AND GLM.a_account_code_c IN " +
                                                        expenseAccountList + ") THEN GLMP.a_actual_base_n ELSE 0 END)";
                        openingBalanceXferCcSelect = " SUM(CASE WHEN (GLMP.a_period_number_i=" + startPeriod + " AND GLM.a_account_code_c = '" +
                                                     MFinanceConstants.DIRECT_XFER_ACCT + "') THEN GLMP.a_actual_base_n ELSE 0 END)";
                        periodFilter = " AND (GLMP.a_period_number_i=" + startPeriod + " OR GLMP.a_period_number_i=" + endPeriod + ")";
                    }

                    String Query =
                        "SELECT CostCentreCode, CostCentreName, ClosingIncome-OpeningIncome AS Income, ClosingExpense-OpeningExpense AS Expense, " +
                        " ClosingXfer-OpeningXfer AS Xfer FROM (" +
                        " SELECT CC.a_cost_centre_code_c AS CostCentreCode, " +
                        " CC.a_cost_centre_name_c AS CostCentreName, " +
                        openingBalanceIncomeCcSelect + " AS OpeningIncome, " +
                        " SUM(CASE WHEN (GLMP.a_period_number_i=" + endPeriod + " AND GLM.a_account_code_c IN " + incomeAccountList +
                        ") THEN GLMP.a_actual_base_n ELSE 0 END) AS ClosingIncome, " +
                        openingBalanceExpenseCcSelect + " AS OpeningExpense, " +
                        " SUM(CASE WHEN (GLMP.a_period_number_i=" + endPeriod + " AND GLM.a_account_code_c IN " + expenseAccountList +
                        ") THEN GLMP.a_actual_base_n ELSE 0 END) AS ClosingExpense, " +
                        openingBalanceXferCcSelect + " AS OpeningXfer, " +
                        " SUM(CASE WHEN (GLMP.a_period_number_i=" + endPeriod + " AND GLM.a_account_code_c = '" +
                        MFinanceConstants.DIRECT_XFER_ACCT + "') THEN GLMP.a_actual_base_n ELSE 0 END) AS ClosingXfer " +
                        " FROM a_cost_centre CC, " +
                        " a_general_ledger_master GLM, " +
                        " a_general_ledger_master_period GLMP " +
                        " WHERE " +
                        " GLM.a_year_i=" + Year +
                        periodFilter +
                        " AND GLM.a_cost_centre_code_c = CC.a_cost_centre_code_c " +
                        " AND GLMP.a_glm_sequence_i = GLM.a_glm_sequence_i " +
                        " AND CC.a_cost_centre_type_c='Foreign' " +
                        " AND CC.a_clearing_account_c= '" + MFinanceConstants.ICH_ACCT_ICH + "' " +
                        " AND GLM.a_account_code_c IN " + incomeAndExpenseAccountList +
                        " GROUP BY CC.a_cost_centre_code_c, CC.a_cost_centre_name_c " +
                        " ORDER BY CC.a_cost_centre_code_c " +
                        ") DETAILS";
                    Result = DbAdapter.RunQuery(Query, "StewardshipForPeriod", Transaction);
                }); // Get NewOrExisting AutoReadTransaction
            return Result;
        } // Stewardship For Period Table

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable AFOTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            AAccountTable AccountTable = new AAccountTable();

            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            string StandardSummaryCostCentre = LedgerNumber.ToString("00") + "00S";
            Int32 Year = AParameters["param_year_i"].ToInt32();
            Int32 Period = AParameters["param_end_period_i"].ToInt32();

            string AccountHierarchyCode = AParameters["param_account_hierarchy_c"].ToString();

            String CostCentreFilter = GetReportingCostCentres(LedgerNumber, StandardSummaryCostCentre, "");

            CostCentreFilter = " AND glm.a_cost_centre_code_c in ('" + CostCentreFilter.Replace(",", "','") + "') ";
            // create new datatable
            DataTable Results = new DataTable();

            Results.Columns.Add(new DataColumn("a_account_code_c", typeof(string)));
            Results.Columns.Add(new DataColumn("a_account_code_short_desc_c", typeof(string)));
            Results.Columns.Add(new DataColumn("DebitCreditIndicator", typeof(bool)));
            Results.Columns.Add(new DataColumn("ActualDebitBase", typeof(Decimal)));
            Results.Columns.Add(new DataColumn("ActualCreditBase", typeof(Decimal)));
            Results.Columns.Add(new DataColumn("ActualDebitIntl", typeof(Decimal)));
            Results.Columns.Add(new DataColumn("ActualCreditIntl", typeof(Decimal)));

            TDBTransaction Transaction = null;
            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    ALedgerRow LedgerRow = (ALedgerRow)ALedgerAccess.LoadByPrimaryKey(LedgerNumber, Transaction).Rows[0];

                    AAccountHierarchyRow HierarchyRow = (AAccountHierarchyRow)AAccountHierarchyAccess.LoadByPrimaryKey(
                        LedgerNumber, AccountHierarchyCode, Transaction).Rows[0];
                    TLedgerInfo ledgerInfo = new TLedgerInfo(LedgerNumber);

                    Decimal exchangeRateNow = TExchangeRateTools.GetCorporateExchangeRate(
                        ledgerInfo.BaseCurrency,
                        ledgerInfo.InternationalCurrency,
                        DateTime.Now.AddMonths(-1),
                        DateTime.Now);

                    if (exchangeRateNow == 0)
                    {
                        exchangeRateNow = 1;
                    }

                    List <String>list = new List <string>();

                    ScanHierarchy(ref list, LedgerNumber, HierarchyRow.RootAccountCode, AccountHierarchyCode, Transaction);

                    // get AAccountRows for each account number in list
                    foreach (string AccountCode in list)
                    {
                        AAccountRow AddRow = (AAccountRow)AAccountAccess.LoadByPrimaryKey(LedgerNumber, AccountCode, Transaction).Rows[0];

                        if (AddRow != null)
                        {
                            AccountTable.Rows.Add((object[])AddRow.ItemArray.Clone());
                        }
                    }

                    // Populate the Results Dataset
                    foreach (AAccountRow Account in AccountTable.Rows)
                    {
                        String ActualField = Account.DebitCreditIndicator ? "ActualDebitBase" : "ActualCreditBase";
                        String IntlField = Account.DebitCreditIndicator ? "ActualDebitIntl" : "ActualCreditIntl";
                        String AccountFilter = GetReportingAccounts(LedgerNumber, Account.AccountCode, "");
                        AccountFilter = " AND glm.a_account_code_c IN ('" + AccountFilter.Replace(",",
                            "','") + "')";
                        String subtractOrAddBase = (Account.DebitCreditIndicator) ?
                                                   "CASE WHEN debit=TRUE THEN Base ELSE 0-Base END"
                                                   :
                                                   "CASE WHEN debit=TRUE THEN 0-Base ELSE Base END";

                        String Query =
                            "SELECT sum(" + subtractOrAddBase + ") AS Actual " +
                            " FROM" +
                            " (SELECT DISTINCT a_account.a_account_code_c AS AccountCode, " +
                            " glm.a_cost_centre_code_c AS CostCentreCode, " +
                            " a_account.a_debit_credit_indicator_l AS debit," +
                            " glmp.a_actual_base_n AS Base" +
                            " FROM a_general_ledger_master AS glm, a_general_ledger_master_period AS glmp, a_account" +
                            " WHERE glm.a_glm_sequence_i=glmp.a_glm_sequence_i" +
                            " AND glm.a_account_code_c=a_account.a_account_code_c" +
                            " AND a_account.a_ledger_number_i=" + LedgerNumber +
                            " AND glm.a_ledger_number_i=" + LedgerNumber +
                            " AND glm.a_year_i=" + Year +
                            " AND glmp.a_period_number_i=" + Period +
                            AccountFilter +
                            CostCentreFilter +
                            ") AS AllGlm";
                        DataTable tempTable = DbAdapter.RunQuery(Query, "AFO", Transaction);

                        Decimal actualBase;

                        if (Decimal.TryParse(tempTable.Rows[0]["Actual"].ToString(), out actualBase))
                        {
                            DataRow NewRow = Results.NewRow();
                            NewRow["a_account_code_c"] = Account.AccountCode;
                            NewRow["a_account_code_short_desc_c"] = Account.AccountCodeShortDesc;
                            NewRow["DebitCreditIndicator"] = Account.DebitCreditIndicator;
                            NewRow[ActualField] = actualBase;
                            NewRow[IntlField] = actualBase / exchangeRateNow;
                            Results.Rows.Add(NewRow);
                        }
                    }
                });  // Get NewOrExisting AutoReadTransaction

            return Results;
        } // AFO Table

        /* We want to report on summary accounts at the highest level of the tree that */
        /* have posting accounts reporting to them.  Recursively descend the tree      */
        /* looking for accounts that summarize a posting account.  As soon as we find  */
        /* one, add it to the list, and don't descend any further.  Any values in      */
        /* summary accounts at lower levels will already have been added in to this    */
        /* level.                                                                      */
        private static void ScanHierarchy(ref List <String>AAccountList,
            int ALedgerNumber,
            string AAccountCode,
            string AHierarchyCode,
            TDBTransaction ATransaction)
        {
            AAccountHierarchyDetailTable AccountHierarchyDetailTable =
                AAccountHierarchyDetailAccess.LoadViaAAccountAccountCodeToReportTo(
                    ALedgerNumber,
                    AAccountCode,
                    ATransaction);

            if (AccountHierarchyDetailTable.Rows.Count > 0)
            {
                foreach (AAccountHierarchyDetailRow Row in AccountHierarchyDetailTable.Rows)
                {
                    if (Row.AccountHierarchyCode == AHierarchyCode)
                    {
                        // check if Row.ReportingAccountCode has any posting accounts reporting to it
                        string Query = "SELECT PUB_a_account.* FROM PUB_a_account, PUB_a_account_hierarchy_detail " +
                                       "WHERE PUB_a_account_hierarchy_detail.a_ledger_number_i = " + ALedgerNumber +
                                       " AND PUB_a_account_hierarchy_detail.a_account_hierarchy_code_c = '" + AHierarchyCode + "'" +
                                       " AND PUB_a_account_hierarchy_detail.a_account_code_to_report_to_c = '" + Row.ReportingAccountCode + "'" +
                                       " AND PUB_a_account.a_ledger_number_i = " + ALedgerNumber +
                                       " AND PUB_a_account.a_account_code_c = PUB_a_account_hierarchy_detail.a_reporting_account_code_c" +
                                       " AND PUB_a_account.a_posting_status_l";

                        DataTable NewTable = DBAccess.GetDBAccessObj(ATransaction).SelectDT(Query, "NewTable", ATransaction);

                        // if posting accounts found
                        if (NewTable.Rows.Count > 0)
                        {
                            AAccountList.Add(Row.ReportingAccountCode);
                        }
                        else
                        {
                            ScanHierarchy(ref AAccountList, ALedgerNumber, Row.ReportingAccountCode, AHierarchyCode, ATransaction);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable ExecutiveSummaryTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            int EndPeriod = AParameters["param_end_period_i"].ToInt32();
            int Year = AParameters["param_year_i"].ToInt32();
            DateTime EndPeriodEndDate = AParameters["param_end_date"].ToDate();
            DateTime EndPeriodStartDate = new DateTime(EndPeriodEndDate.Year, EndPeriodEndDate.Month, 1);
            DateTime NextPeriodStartDate = new DateTime();
            DateTime NextPeriodEndDate = new DateTime();

            decimal[] ActualsAndBudget;
            Int64 LedgerPartnerKey;
            string StandardSummaryCostCentre;

            // create new datatable
            DataTable Results = new DataTable();

            string[] Columns =
            {
                "ThisMonth", "Budget", "ActualYTD", "BudgetYTD", "PriorYTD"
            };

            foreach (string Column in Columns)
            {
                Results.Columns.Add(new DataColumn("Income" + Column, typeof(Decimal)));
                Results.Columns.Add(new DataColumn("Expenses" + Column, typeof(Decimal)));
                Results.Columns.Add(new DataColumn("PersonnelCosts" + Column, typeof(Decimal)));
                Results.Columns.Add(new DataColumn("SupportIncome" + Column, typeof(Decimal)));
                Results.Columns.Add(new DataColumn("CashAndBank" + Column, typeof(Decimal)));
                Results.Columns.Add(new DataColumn("FromToICH" + Column, typeof(Decimal)));
                Results.Columns.Add(new DataColumn("PaymentsDue" + Column, typeof(Decimal)));
                Results.Columns.Add(new DataColumn("GiftsForOtherFields" + Column, typeof(Decimal)));
                Results.Columns.Add(new DataColumn("Personnel" + Column, typeof(Int32)));
                Results.Columns.Add(new DataColumn("PersonnelOtherFields" + Column, typeof(Int32)));
            }

            DataRow ResultRow = Results.NewRow();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    ALedgerRow LedgerRow = (ALedgerRow)ALedgerAccess.LoadByPrimaryKey(LedgerNumber, Transaction).Rows[0];
                    LedgerPartnerKey = LedgerRow.PartnerKey;
                    StandardSummaryCostCentre = LedgerRow.LedgerNumber.ToString("00") + "00S";

                    /* Income and Expenses */
                    string[, ] IncomeExpense = { { "Income", MFinanceConstants.INCOME_HEADING },        // "INC"
                                                 { "Expenses", MFinanceConstants.EXPENSE_HEADING } };   // "EXP"

                    for (int i = 0; i < 2; i++)
                    {
                        ActualsAndBudget = GetActualsAndBudget(DbAdapter, LedgerRow, IncomeExpense[i, 1], StandardSummaryCostCentre, EndPeriod, Year);

                        ResultRow[IncomeExpense[i, 0] + "ThisMonth"] = ActualsAndBudget[0];
                        ResultRow[IncomeExpense[i, 0] + "PriorYTD"] = ActualsAndBudget[1];
                        ResultRow[IncomeExpense[i, 0] + "ActualYTD"] = ActualsAndBudget[2];
                        ResultRow[IncomeExpense[i, 0] + "Budget"] = ActualsAndBudget[3];
                        ResultRow[IncomeExpense[i, 0] + "BudgetYTD"] = ActualsAndBudget[4];
                    }

                    /* Personnel Costs */

                    // Calculate how many team members
                    int PersonnelCount = 0;
                    PmStaffDataTable StaffDataTable = PmStaffDataAccess.LoadViaPUnitReceivingField(LedgerPartnerKey, Transaction);

                    // this month - includes people who are only there for part of the month
                    foreach (PmStaffDataRow Row in StaffDataTable.Rows)
                    {
                        if (((Row.EndOfCommitment == null) || (Row.EndOfCommitment >= EndPeriodStartDate))
                            && (Row.StartOfCommitment <= EndPeriodEndDate))
                        {
                            PersonnelCount++;
                        }
                    }

                    ResultRow["PersonnelThisMonth"] = PersonnelCount;

                    // this year
                    PersonnelCount = 0;

                    foreach (PmStaffDataRow Row in StaffDataTable.Rows)
                    {
                        if (((Row.EndOfCommitment == null) || (Row.EndOfCommitment >= new DateTime(EndPeriodStartDate.Year, 1, 1)))
                            && (Row.StartOfCommitment <= EndPeriodEndDate))
                        {
                            PersonnelCount++;
                        }
                    }

                    ResultRow["PersonnelActualYTD"] = PersonnelCount;

                    // last year
                    PersonnelCount = 0;

                    foreach (PmStaffDataRow Row in StaffDataTable.Rows)
                    {
                        if (((Row.EndOfCommitment == null) || (Row.EndOfCommitment >= new DateTime(EndPeriodStartDate.Year - 1, 1, 1)))
                            && (Row.StartOfCommitment <= EndPeriodEndDate.AddYears(-1)))
                        {
                            PersonnelCount++;
                        }
                    }

                    ResultRow["PersonnelPriorYTD"] = PersonnelCount;

                    /* calculate Personnel Costs per Team Member */
                    ActualsAndBudget =
                        GetActualsAndBudget(DbAdapter, LedgerRow, MFinanceConstants.PERSONNEL_EXPENSES, StandardSummaryCostCentre, EndPeriod, Year);                                       //4300S

                    decimal TotalPersonnelCostsThisMonth = ActualsAndBudget[0];
                    decimal TotalPersonnelCostsPriorYTD = ActualsAndBudget[1];
                    decimal TotalPersonnelCostsActualYTD = ActualsAndBudget[2];
                    decimal TotalPersonnelCostsBudget = ActualsAndBudget[3];
                    decimal TotalPersonnelCostsBudgetYTD = ActualsAndBudget[4];

                    if (Convert.ToInt32(ResultRow["PersonnelThisMonth"]) != 0)
                    {
                        ResultRow["PersonnelCostsThisMonth"] = TotalPersonnelCostsThisMonth / Convert.ToInt32(ResultRow["PersonnelThisMonth"]);
                    }

                    if (Convert.ToInt32(ResultRow["PersonnelActualYTD"]) != 0)
                    {
                        ResultRow["PersonnelCostsActualYTD"] = TotalPersonnelCostsActualYTD / Convert.ToInt32(ResultRow["PersonnelActualYTD"]);
                        ResultRow["PersonnelCostsBudget"] = TotalPersonnelCostsBudget / Convert.ToInt32(ResultRow["PersonnelActualYTD"]);
                        ResultRow["PersonnelCostsBudgetYTD"] = TotalPersonnelCostsBudgetYTD / Convert.ToInt32(ResultRow["PersonnelActualYTD"]);
                    }

                    if (Convert.ToInt32(ResultRow["PersonnelPriorYTD"]) != 0)
                    {
                        ResultRow["PersonnelCostsPriorYTD"] = TotalPersonnelCostsPriorYTD / Convert.ToInt32(ResultRow["PersonnelPriorYTD"]);
                    }

                    /* SUPPORT INCOME % */
                    ActualsAndBudget = GetActualsAndBudget(DbAdapter,
                        LedgerRow, MFinanceConstants.SUPPORT_GIFTS_LOCAL, StandardSummaryCostCentre, EndPeriod, Year);          // 0100S

                    decimal TotalSupportIncomeThisMonth = ActualsAndBudget[0];
                    decimal TotalSupportIncomePriorYTD = ActualsAndBudget[1];
                    decimal TotalSupportIncomeActualYTD = ActualsAndBudget[2];
                    decimal TotalSupportIncomeBudget = ActualsAndBudget[3];
                    decimal TotalSupportIncomeBudgetYTD = ActualsAndBudget[4];

                    ActualsAndBudget = GetActualsAndBudget(DbAdapter,
                        LedgerRow, MFinanceConstants.SUPPORT_GIFTS_FOREIGN, StandardSummaryCostCentre, EndPeriod, Year);        // 1100S

                    TotalSupportIncomeThisMonth += ActualsAndBudget[0];
                    TotalSupportIncomePriorYTD += ActualsAndBudget[1];
                    TotalSupportIncomeActualYTD += ActualsAndBudget[2];
                    TotalSupportIncomeBudget += ActualsAndBudget[3];
                    TotalSupportIncomeBudgetYTD += ActualsAndBudget[4];

                    ResultRow["SupportIncomeThisMonth"] = TotalPersonnelCostsThisMonth != 0 ?
                                                          (TotalSupportIncomeThisMonth / TotalPersonnelCostsThisMonth) : 0;
                    ResultRow["SupportIncomePriorYTD"] = TotalPersonnelCostsPriorYTD != 0 ?
                                                         (TotalSupportIncomePriorYTD / TotalPersonnelCostsPriorYTD) : 0;
                    ResultRow["SupportIncomeActualYTD"] = TotalPersonnelCostsActualYTD != 0 ?
                                                          (TotalSupportIncomeActualYTD / TotalPersonnelCostsActualYTD) : 0;
                    ResultRow["SupportIncomeBudget"] = TotalPersonnelCostsBudget != 0 ?
                                                       (TotalSupportIncomeBudget / TotalPersonnelCostsBudget) : 0;
                    ResultRow["SupportIncomeBudgetYTD"] = TotalPersonnelCostsBudgetYTD != 0 ?
                                                          (TotalSupportIncomeBudgetYTD / TotalPersonnelCostsBudgetYTD) : 0;

                    /* Bank */
                    string[, ] Bank =
                    {
                        { "CashAndBank", MFinanceConstants.BANK_HEADING }, { "FromToICH", MFinanceConstants.ICH_ACCT_ICH + "S" }
                    };                                                                                                            // "CASH", 8500S

                    for (int i = 0; i < 2; i++)
                    {
                        int Multiplier = 1;

                        // find out if ICH balance is credit or debit
                        AAccountRow AccountRow = (AAccountRow)AAccountAccess.LoadByPrimaryKey(LedgerNumber, Bank[i, 1], Transaction).Rows[0];

                        if ((AccountRow != null) && !AccountRow.DebitCreditIndicator)
                        {
                            Multiplier = -1;
                        }

                        ActualsAndBudget = GetActualsAndBudget(DbAdapter, LedgerRow, Bank[i, 1], StandardSummaryCostCentre, EndPeriod, Year);

                        // balance sheet item so will be the same as the ytd figure
                        ResultRow[Bank[i, 0] + "ThisMonth"] = ActualsAndBudget[2] * Multiplier;
                        ResultRow[Bank[i, 0] + "PriorYTD"] = ActualsAndBudget[1] * Multiplier;
                        ResultRow[Bank[i, 0] + "ActualYTD"] = ActualsAndBudget[2] * Multiplier;
                        ResultRow[Bank[i, 0] + "Budget"] = ActualsAndBudget[3] * Multiplier;
                        ResultRow[Bank[i, 0] + "BudgetYTD"] = ActualsAndBudget[4] * Multiplier;
                    }

                    /* ACCOUNTS PAYABLE */
                    // find the dates of the next period
                    AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadViaALedger(LedgerNumber, Transaction);
                    AAccountingPeriodRow AccountingPeriodRow =
                        (AAccountingPeriodRow)AccountingPeriodTable.Rows.Find(new object[] { LedgerNumber, EndPeriod + 1 });

                    if (AccountingPeriodRow == null)
                    {
                        AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows.Find(new object[] { LedgerNumber, EndPeriod });

                        if (AccountingPeriodRow == null)
                        {
                            throw new Exception("TFinanceReportingWebConnector.ExecutiveSummaryTable: Accounting Period not available");
                        }

                        NextPeriodStartDate = AccountingPeriodRow.PeriodEndDate.AddDays(1);
                        // approximate the end date
                        NextPeriodEndDate = AccountingPeriodRow.PeriodEndDate.AddDays(30);
                    }
                    else
                    {
                        NextPeriodStartDate = AccountingPeriodRow.PeriodStartDate;
                        NextPeriodEndDate = AccountingPeriodRow.PeriodEndDate;
                    }

                    if (NextPeriodStartDate == NextPeriodEndDate)
                    {
                        // If 13th period then add 32 days to the end date to go into end of January
                        NextPeriodEndDate = NextPeriodEndDate.AddDays(31);
                    }

                    // this month
                    ResultRow["PaymentsDueThisMonth"] = 0;
                    AApDocumentTable ApDocumentTable = AApDocumentAccess.LoadViaALedger(LedgerNumber, Transaction);

                    foreach (AApDocumentRow ApDocumentRow in ApDocumentTable.Rows)
                    {
                        if ((ApDocumentRow.DateIssued.AddDays(ApDocumentRow.CreditTerms) >= NextPeriodStartDate)
                            && (ApDocumentRow.DateIssued.AddDays(ApDocumentRow.CreditTerms) <= NextPeriodEndDate)
                            && (!ApDocumentRow.DocumentStatus.Equals(MFinanceConstants.BATCH_UNPOSTED, StringComparison.InvariantCultureIgnoreCase))
                            && (!ApDocumentRow.DocumentStatus.Equals(MFinanceConstants.BATCH_CANCELLED, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            ResultRow["PaymentsDueThisMonth"] = Convert.ToDecimal(ResultRow["PaymentsDueThisMonth"]) + ApDocumentRow.TotalAmount;
                        }
                    }

                    /* FOREIGN GIFTS */
                    ActualsAndBudget = GetActualsAndBudget(DbAdapter,
                        LedgerRow, MFinanceConstants.GIFT_HEADING, MFinanceConstants.INTER_LEDGER_HEADING, EndPeriod, Year);            // "GIFT", "ILT"

                    ResultRow["GiftsForOtherFieldsThisMonth"] = ActualsAndBudget[0];
                    ResultRow["GiftsForOtherFieldsPriorYTD"] = ActualsAndBudget[1];
                    ResultRow["GiftsForOtherFieldsActualYTD"] = ActualsAndBudget[2];
                    ResultRow["GiftsForOtherFieldsBudget"] = ActualsAndBudget[3];
                    ResultRow["GiftsForOtherFieldsBudgetYTD"] = ActualsAndBudget[4];

                    /* NUMBER OF PERSONNEL ON OTHER FIELDS */
                    PersonnelCount = 0;
                    StaffDataTable = PmStaffDataAccess.LoadViaPUnitHomeOffice(LedgerPartnerKey, Transaction);

                    // this month - includes people who are only there for part of the month
                    foreach (PmStaffDataRow Row in StaffDataTable.Rows)
                    {
                        if (((Row.EndOfCommitment == null) || (Row.EndOfCommitment >= EndPeriodStartDate))
                            && (Row.StartOfCommitment <= EndPeriodEndDate)
                            && (Row.ReceivingField != LedgerPartnerKey))
                        {
                            PersonnelCount++;
                        }
                    }

                    ResultRow["PersonnelOtherFieldsThisMonth"] = PersonnelCount;

                    // this year
                    PersonnelCount = 0;

                    foreach (PmStaffDataRow Row in StaffDataTable.Rows)
                    {
                        if (((Row.EndOfCommitment == null) || (Row.EndOfCommitment >= new DateTime(EndPeriodStartDate.Year, 1, 1)))
                            && (Row.StartOfCommitment <= EndPeriodEndDate)
                            && (Row.ReceivingField != LedgerPartnerKey))
                        {
                            PersonnelCount++;
                        }
                    }

                    ResultRow["PersonnelOtherFieldsActualYTD"] = PersonnelCount;

                    // last year
                    PersonnelCount = 0;

                    foreach (PmStaffDataRow Row in StaffDataTable.Rows)
                    {
                        if (((Row.EndOfCommitment == null) || (Row.EndOfCommitment >= new DateTime(EndPeriodStartDate.Year - 1, 1, 1)))
                            && (Row.StartOfCommitment <= EndPeriodEndDate.AddYears(-1))
                            && (Row.ReceivingField != LedgerPartnerKey))
                        {
                            PersonnelCount++;
                        }
                    }

                    ResultRow["PersonnelOtherFieldsPriorYTD"] = PersonnelCount;
                }); // Get NewOrExisting AutoReadTransaction

            Results.Rows.Add(ResultRow);

            return Results;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable FieldLeaderGiftSummary(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            String selectedFieldList = AParameters["param_clbFields"].ToString();

            selectedFieldList = selectedFieldList.Replace('\'', ' ');
            String FieldFilter = " AND partnerfield.p_partner_key_n IN (" + selectedFieldList + ") ";
            Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            DateTime PeriodStart = AParameters["param_from_date"].ToDate();
            DateTime PeriodEnd = AParameters["param_to_date"].ToDate();
            String PeriodRange = "BETWEEN '" + PeriodStart.ToString("yyyy-MM-dd") + "' AND '" + PeriodEnd.ToString("yyyy-MM-dd") + "'";
            Int32 PeriodYear = PeriodEnd.Year;


            String exch = "";

            if (AParameters["param_currency"].ToString().StartsWith("Int"))
            {
                //
                // Read different DB fields according to currency setting
                // NOTE: Modified August 2016 to return a_gift_amount_n * exchangeRate, rather than the recorded a_gift_amount_intl_n
                TLedgerInfo ledgerInfo = new TLedgerInfo(LedgerNumber);
                TCorporateExchangeRateCache ExchangeRateCache = new TCorporateExchangeRateCache();
                Decimal exchangeRateNow = ExchangeRateCache.GetCorporateExchangeRate(DbAdapter.FPrivateDatabaseObj,
                    LedgerNumber,
                    ledgerInfo.CurrentFinancialYear,
                    ledgerInfo.CurrentPeriod,
                    -1);
                exch = " * " + exchangeRateNow;
            }

            DateTime Year = new DateTime(AParameters["param_year0"].ToInt32(), 1, 1);
            String Year1Range = "BETWEEN '" + Year.ToString("yyyy-MM-dd") + "' AND '" + Year.AddYears(1).AddDays(-1).ToString("yyyy-MM-dd") + "'";

            Year = new DateTime(AParameters["param_year1"].ToInt32(), 1, 1);
            String Year2Range = "BETWEEN '" + Year.ToString("yyyy-MM-dd") + "' AND '" + Year.AddYears(1).AddDays(-1).ToString("yyyy-MM-dd") + "'";

            Year = new DateTime(AParameters["param_year2"].ToInt32(), 1, 1);
            String Year3Range = "BETWEEN '" + Year.ToString("yyyy-MM-dd") + "' AND '" + Year.AddYears(1).AddDays(-1).ToString("yyyy-MM-dd") + "'";

            Year = new DateTime(AParameters["param_year3"].ToInt32(), 1, 1);
            String Year4Range = "BETWEEN '" + Year.ToString("yyyy-MM-dd") + "' AND '" + Year.AddYears(1).AddDays(-1).ToString("yyyy-MM-dd") + "'";
            DateTime FirstDate = Year;

            if (PeriodStart < FirstDate)
            {
                FirstDate = PeriodStart;
            }

            //TODO: Calendar vs Financial Date Handling - Check if this should use financial year start/end and not assume calendar
            String TotalDateRange = "BETWEEN '" + FirstDate.ToString("yyyy-MM-dd") + "' AND '" + new DateTime(DateTime.Today.Year, 12, 31).ToString(
                "yyyy-MM-dd") + "'";


            String Query =
                " SELECT * FROM (" +
                " SELECT " +
                " partnerfield.p_partner_key_n AS FieldKey," +
                " partnerfield.p_partner_short_name_c AS FieldName,"

                + " partnerrecipient.p_partner_key_n AS RecipientKey," +
                " partnerrecipient.p_partner_short_name_c AS RecipientName," +
                " partnerrecipient.p_partner_class_c AS RecipientClass," +
                " SUM(CASE WHEN gift.a_date_entered_d " + PeriodRange + " THEN detail.a_gift_amount_n ELSE 0 END) " + exch + " AS AmountPeriod," +
                " SUM(CASE WHEN gift.a_date_entered_d " + Year1Range + " THEN detail.a_gift_amount_n ELSE 0 END) " + exch + " AS AmountYear1," +
                " SUM(CASE WHEN gift.a_date_entered_d " + Year2Range + " THEN detail.a_gift_amount_n ELSE 0 END) " + exch + " AS AmountYear2," +
                " SUM(CASE WHEN gift.a_date_entered_d " + Year3Range + " THEN detail.a_gift_amount_n ELSE 0 END) " + exch + " AS AmountYear3," +
                " SUM(CASE WHEN gift.a_date_entered_d " + Year4Range + " THEN detail.a_gift_amount_n ELSE 0 END) " + exch + " AS AmountYear4"

                + " FROM" +
                " PUB_p_partner as partnerfield," +
                " PUB_p_partner as partnerrecipient," +
                " PUB_a_gift as gift," +
                " PUB_a_gift_detail as detail," +
                " PUB_a_gift_batch as giftbatch"

                + " WHERE" +
                " detail.a_batch_number_i = gift.a_batch_number_i" +
                " AND detail.a_gift_transaction_number_i = gift.a_gift_transaction_number_i" +
                " AND detail.a_recipient_ledger_number_n = partnerfield.p_partner_key_n" +
                " AND detail.a_ledger_number_i = " + LedgerNumber +
                " AND gift.a_ledger_number_i = " + LedgerNumber +
                " AND gift.a_date_entered_d " + TotalDateRange +
                " AND giftbatch.a_batch_status_c = 'Posted'" +
                " AND giftbatch.a_batch_number_i = gift.a_batch_number_i" +
                " AND giftbatch.a_ledger_number_i = " + LedgerNumber

                + " AND partnerrecipient.p_partner_key_n = detail.p_recipient_key_n"

                + FieldFilter +
                " GROUP BY partnerfield.p_partner_short_name_c, partnerfield.p_partner_key_n, partnerrecipient.p_partner_key_n, partnerrecipient.p_partner_short_name_c, partnerrecipient.p_partner_class_c "
                +
                ") AllData WHERE AmountPeriod > 0 ORDER BY FieldKey, RecipientName"
            ;
            DataTable resultTable = new DataTable();
            TDBTransaction Transaction = null;
            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    resultTable = DbAdapter.RunQuery(Query, "FieldLeaderGiftSummary", Transaction);
                });
            return resultTable;
        } // Field Leader Gift Summary

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// Derived from the existing method in MFinanceQueries\ReportFinance.cs
        /// </summary>
        [NoRemoting]
        public static DataTable TotalGiftsThroughField(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            DateTime startDate = AParameters["param_StartDate"].ToDate();
            string strStartDate = startDate.ToString("#yyyy-MM-dd#");
            DateTime endDate = new DateTime(DateTime.Now.Year, 12, 31);
            string strEndDate = endDate.ToString("#yyyy-MM-dd#");

            Int32 LedgerAccountingPeriods;
            Int32 LedgerForwardPeriods;
            Int32 LedgerCurrentPeriod;
            Int32 LedgerCurrentYear;

            GetLedgerPeriodDetails(LedgerNumber,
                out LedgerAccountingPeriods,
                out LedgerForwardPeriods,
                out LedgerCurrentPeriod,
                out LedgerCurrentYear);
            Int32 MostRecentCompletedMonth = 0;

            bool TaxDeductiblePercentageEnabled =
                TSystemDefaults.GetBooleanDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, false);

            string SqlQuery = "SELECT batch.a_gl_effective_date_d as Date, motive.a_report_column_c AS ReportColumn, ";

            if (AParameters["param_currency"].ToString() == "Base")
            {
                SqlQuery += "detail.a_gift_amount_n AS Amount";

                if (TaxDeductiblePercentageEnabled)
                {
                    SqlQuery += ", detail.a_tax_deductible_amount_base_n AS TaxDeductAmount";
                }
            }
            else
            {
                //
                // Read different DB fields according to currency setting
                // NOTE: Modified August 2016 to return a_gift_amount_n * exchangeRate,
                // rather than the recorded a_gift_amount_intl_n
                TLedgerInfo ledgerInfo = new TLedgerInfo(LedgerNumber);
                TCorporateExchangeRateCache ExchangeRateCache = new TCorporateExchangeRateCache();
                Decimal exchangeRateNow = ExchangeRateCache.GetCorporateExchangeRate(DbAdapter.FPrivateDatabaseObj,
                    LedgerNumber,
                    ledgerInfo.CurrentFinancialYear,
                    ledgerInfo.CurrentPeriod,
                    -1);

                SqlQuery += "detail.a_gift_amount_n * " + exchangeRateNow + " AS Amount";

                if (TaxDeductiblePercentageEnabled)
                {
                    SqlQuery += ", detail.a_tax_deductible_amount_base_n * " + exchangeRateNow + " AS TaxDeductAmount";
                }
            }

            SqlQuery += (" FROM PUB_a_gift as gift, PUB_a_gift_detail as detail, PUB_a_gift_batch as batch, PUB_a_motivation_detail AS motive"

                         + " WHERE detail.a_ledger_number_i = " + LedgerNumber +
                         " AND batch.a_batch_status_c = 'Posted'" +
                         " AND batch.a_batch_number_i = gift.a_batch_number_i" +
                         " AND batch.a_ledger_number_i = " + LedgerNumber +
                         " AND batch.a_gl_effective_date_d >= " + strStartDate +

                         " AND gift.a_ledger_number_i = " + LedgerNumber +
                         " AND detail.a_batch_number_i = gift.a_batch_number_i" +
                         " AND detail.a_gift_transaction_number_i = gift.a_gift_transaction_number_i"

                         + " AND motive.a_ledger_number_i = " + LedgerNumber +
                         " AND motive.a_motivation_group_code_c = detail.a_motivation_group_code_c" +
                         " AND motive.a_motivation_detail_code_c = detail.a_motivation_detail_code_c" +
                         " AND motive.a_receipt_l=true"

                         + " ORDER BY batch.a_gl_effective_date_d"
                         );
            DataTable tempTbl = null;
            TDBTransaction Transaction = null;
            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    String todaysDateSql = DateTime.Now.ToString("yyyy-MM-dd");

                    AAccountingPeriodTable periodTbl = AAccountingPeriodAccess.LoadAll(Transaction);
                    periodTbl.DefaultView.RowFilter = "a_ledger_number_i=" + LedgerNumber +
                                                      " AND a_period_start_date_d<='" + todaysDateSql + "'" +
                                                      " AND a_period_end_date_d>='" + todaysDateSql + "'";

                    if (periodTbl.DefaultView.Count > 0)
                    {
                        Int32 accountingPeriodToday = ((AAccountingPeriodRow)periodTbl.DefaultView[0].Row).AccountingPeriodNumber;
                        MostRecentCompletedMonth = accountingPeriodToday - 1;

                        if (accountingPeriodToday > 1)
                        {
                            periodTbl.DefaultView.RowFilter = "a_accounting_period_number_i=" + (accountingPeriodToday - 1);

                            if (periodTbl.DefaultView.Count > 0)
                            {
                                MostRecentCompletedMonth = ((AAccountingPeriodRow)periodTbl.DefaultView[0].Row).PeriodStartDate.Month;
                            }
                        }
                    }

                    tempTbl = DbAdapter.RunQuery(SqlQuery, "AllGifts", Transaction);
                });

            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("Year", typeof(Int32));
            resultTable.Columns.Add("Month", typeof(Int32));
            resultTable.Columns.Add("MonthName", typeof(String));
            resultTable.Columns.Add("MonthWorker", typeof(Decimal));
            resultTable.Columns.Add("MonthWorkerCount", typeof(Int32));
            resultTable.Columns.Add("MonthField", typeof(Decimal));
            resultTable.Columns.Add("MonthFieldCount", typeof(Int32));
            resultTable.Columns.Add("MonthTotal", typeof(Decimal));
            resultTable.Columns.Add("MonthTotalCount", typeof(Int32));
            resultTable.Columns.Add("MonthWorkerTaxDeduct", typeof(Decimal));
            resultTable.Columns.Add("MonthFieldTaxDeduct", typeof(Decimal));
            resultTable.Columns.Add("MonthTotalTaxDeduct", typeof(Decimal));

            for (Int32 Year = endDate.Year; Year >= startDate.Year; Year--)
            {
                Int32 MaxMonth = (Year == endDate.Year) ? MostRecentCompletedMonth : LedgerAccountingPeriods;

                for (Int32 Month = 1; Month <= MaxMonth; Month++)
                {
                    DateTime monthStart = new DateTime(Year, 1, 1).AddMonths(Month - 1);
                    string monthStartSql = monthStart.ToString("yyyy-MM-dd");
                    DateTime nextMonthStart = new DateTime(Year, 1, 1).AddMonths(Month);
                    string nextMonthStartSql = nextMonthStart.ToString("yyyy-MM-dd");

                    tempTbl.DefaultView.RowFilter = "Date >= '" + monthStartSql + "' AND Date < '" + nextMonthStartSql + "'";

                    DataRow resultRow = resultTable.NewRow();

                    Decimal WorkerTotal = 0;
                    Decimal FieldTotal = 0;
                    Int32 WorkerCount = 0;
                    Int32 FieldCount = 0;
                    Int32 TotalCount = tempTbl.DefaultView.Count;

                    Decimal WorkerTotalTaxDeduct = 0;
                    Decimal FieldTotalTaxDeduct = 0;

                    foreach (DataRowView rv in tempTbl.DefaultView)
                    {
                        DataRow Row = rv.Row;

                        if (Row["ReportColumn"].ToString() == "Worker")
                        {
                            WorkerCount++;
                            WorkerTotal += Convert.ToDecimal(Row["Amount"]);

                            if (TaxDeductiblePercentageEnabled)
                            {
                                WorkerTotalTaxDeduct += Convert.ToDecimal(Row["TaxDeductAmount"]);
                            }
                        }
                        else
                        {
                            FieldCount++;
                            FieldTotal += Convert.ToDecimal(Row["Amount"]);

                            if (TaxDeductiblePercentageEnabled)
                            {
                                FieldTotalTaxDeduct += Convert.ToDecimal(Row["TaxDeductAmount"]);
                            }
                        }
                    }

                    resultRow["Year"] = Year;
                    resultRow["Month"] = Month;
                    Int32 monthMod12 = 1 + ((Month - 1) % 12);
                    resultRow["MonthName"] = StringHelper.GetLongMonthName(monthMod12);
                    resultRow["MonthWorker"] = WorkerTotal;
                    resultRow["MonthWorkerCount"] = WorkerCount;
                    resultRow["MonthField"] = FieldTotal;
                    resultRow["MonthFieldCount"] = FieldCount;
                    resultRow["MonthTotal"] = WorkerTotal + FieldTotal;
                    resultRow["MonthTotalCount"] = TotalCount;

                    resultRow["MonthWorkerTaxDeduct"] = WorkerTotalTaxDeduct;
                    resultRow["MonthFieldTaxDeduct"] = FieldTotalTaxDeduct;
                    resultRow["MonthTotalTaxDeduct"] = WorkerTotalTaxDeduct + FieldTotalTaxDeduct;

                    resultTable.Rows.Add(resultRow);
                } // For Month

            } // For Year

            return resultTable;
        } // Total Gifts Through Field

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable DonorGiftsToField(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            Int32 LedgerNum = AParameters["param_ledger_number_i"].ToInt32();
            DateTime startDate = AParameters["param_StartDate"].ToDate();
            string strStartDate = startDate.ToString("#yyyy-MM-dd#");
            DateTime endDate = AParameters["param_EndDate"].ToDate();
            string strEndDate = endDate.ToString("#yyyy-MM-dd#");

            String AmountField = "detail.a_gift_amount_n";
            String CurrencyOption = AParameters["param_currency"].ToString().ToLower();

            if (CurrencyOption.StartsWith("int"))
            {
                AmountField = "detail.a_gift_amount_intl_n";
            }

            Decimal MinAmount = AParameters["param_min_amount"].ToDecimal();
            Decimal MaxAmount = AParameters["param_max_amount"].ToDecimal();

            bool TaxDeductiblePercentageEnabled =
                TSystemDefaults.GetBooleanDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, false);

            DataTable resultTable = new DataTable();
            String ExtractTables = " ";
            String donorOption = AParameters["param_donor"].ToString();
            String AmountFilter = " AND ABS(" + AmountField + ") >= " + MinAmount + " AND ABS(" + AmountField + ") <= " + MaxAmount;
            String FieldFilter = "";

            if (AParameters["param_field_selection"].ToString() == "selected_fields")
            {
                String FieldList = AParameters["param_clbFields"].ToString();
                FieldFilter = " AND detail.a_recipient_ledger_number_n in (" + FieldList + ")";
            }

            if (donorOption == "Extract")
            {
                ExtractTables = ", PUB_m_extract, PUB_m_extract_master";
            }

            String Query =
                "SELECT DISTINCT " +
                " gift.p_donor_key_n AS DonorKey, " +
                " donor.p_partner_short_name_c AS DonorName, " +
                " donor.p_partner_class_c AS DonorClass, " +
                " gift.a_date_entered_d AS GiftDate, " +
                " recipient.p_partner_key_n AS RecipientKey, " +
                " recipient.p_partner_short_name_c AS RecipientName, " +
                " recipient.p_partner_class_c AS RecipientClass, " +
                " detail.a_confidential_gift_flag_l AS Confidential, " +
                " gift.a_receipt_number_i AS Receipt, " +
                " gift.a_method_of_giving_code_c AS Method, " +
                " detail.a_recipient_ledger_number_n AS FieldKey, " +
                " detail.a_motivation_group_code_c AS MotivationGroup, " +
                " detail.a_motivation_detail_code_c AS MotivationDetail, " +
                " PUB_a_motivation_detail.a_motivation_detail_desc_c AS MotivationDetailDescr, " +
                " PUB_a_motivation_group.a_motivation_group_description_c AS MotivationGroupDescr, " +

                AmountField + " AS GiftAmount " +
                " FROM " +
                " PUB_a_gift as gift, " +
                " PUB_a_gift_detail as detail, " +
                " PUB_a_gift_batch, " +
                " PUB_p_partner as donor, " +
                " PUB_p_partner as recipient, " +
                " PUB_a_motivation_group, " +
                " PUB_a_motivation_detail " +
                ExtractTables;

            switch (donorOption)
            {
                case "Extract":
                    Query +=
                        " WHERE" +
                        " gift.p_donor_key_n =  PUB_m_extract.p_partner_key_n" +
                        " AND PUB_m_extract.m_extract_id_i = PUB_m_extract_master.m_extract_id_i" +
                        " AND PUB_m_extract_master.m_extract_name_c = '" + AParameters["param_extract_name"].ToString() + "'" +
                        " AND";
                    break;

                case "All Donors":
                    Query +=
                        " WHERE";
                    break;

                case "One Donor":
                    Query +=
                        " WHERE" +
                        " gift.p_donor_key_n = " + AParameters["param_donorkey"].ToInt32() +
                        " AND";
                    break;
            }

            Query +=
                " detail.a_ledger_number_i = " + LedgerNum +
                " AND detail.a_batch_number_i = gift.a_batch_number_i" +
                " AND detail.a_gift_transaction_number_i = gift.a_gift_transaction_number_i" +
                " AND gift.a_date_entered_d BETWEEN " + strStartDate + " AND " + strEndDate +
                " AND gift.a_ledger_number_i = " + LedgerNum +
                " AND PUB_a_gift_batch.a_batch_status_c = 'Posted'" +
                " AND PUB_a_gift_batch.a_batch_number_i = gift.a_batch_number_i" +
                " AND PUB_a_gift_batch.a_ledger_number_i = " + LedgerNum +
                " AND donor.p_partner_key_n = gift.p_donor_key_n" +
                " AND recipient.p_partner_key_n = detail.p_recipient_key_n" +

                " AND a_motivation_group.a_ledger_number_i = " + LedgerNum +
                " AND a_motivation_detail.a_ledger_number_i = " + LedgerNum +
                " AND detail.a_motivation_group_code_c = a_motivation_group.a_motivation_group_code_c" +
                " AND a_motivation_detail.a_motivation_group_code_c = detail.a_motivation_group_code_c" +
                " AND detail.a_motivation_detail_code_c = a_motivation_detail.a_motivation_detail_code_c" +

                AmountFilter +
                FieldFilter +
                " ORDER BY donor.p_partner_short_name_c, GiftDate";

            TDBTransaction Transaction = null;
            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    resultTable = DbAdapter.RunQuery(Query, "DonorGiftsToField", Transaction);
                    resultTable.Columns.Add("DonorAddress", typeof(String));

                    // I need to add the donor address to each row:
                    Int64 existingDonorKey = -1;
                    String existingDonorAddress = ""; // For givers of multiple gifts, this caching makes the loop slightly faster.

                    foreach (DataRow resultRow in resultTable.Rows)
                    {
                        Int64 DonorKey = Convert.ToInt64(resultRow["DonorKey"]);

                        if (DonorKey != existingDonorKey)
                        {
                            existingDonorKey = DonorKey;
                            PLocationTable LocationTable;
                            String CountryName;
                            TAddressTools.GetBestAddress(DonorKey, out LocationTable, out CountryName, Transaction);

                            if (LocationTable.Rows.Count > 0)
                            {
                                existingDonorAddress = Calculations.DetermineLocationString(
                                    LocationTable[0],
                                    Calculations.TPartnerLocationFormatEnum.plfCommaSeparated
                                    );
                            }
                        }

                        resultRow["DonorAddress"] = existingDonorAddress;
                    }
                });
            return resultTable;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable GiftDestination(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DateTime GiftDate = AParameters["param_giftdate"].ToDate();
            string strGiftDate = "'" + GiftDate.ToString("yyyy-MM-dd") + "'";

            DataTable resultTable = new DataTable();
            String Query = "SELECT q.*, " +
                           " GiftDest.p_partner_short_name_c AS GiftDestnName FROM ( " +
                           " SELECT p_person.p_partner_key_n AS StaffKey, " +
                           " p_person.p_family_key_n AS FamilyKey, " +
                           " Staff.p_partner_short_name_c AS StaffName, " +
                           " pm_staff_data.pm_receiving_field_n AS FieldKey, " +
                           " RecvField.p_partner_short_name_c AS FieldName, " +
                           " p_partner_gift_destination.p_field_key_n AS GiftDestn " +

                           " FROM " +
                           " pm_staff_data, p_partner Staff, p_partner RecvField, p_person " +
                           " LEFT JOIN p_partner_gift_destination ON (" +
                           "   p_partner_gift_destination.p_partner_key_n=p_person.p_family_key_n " +
                           "   AND p_partner_gift_destination.p_date_effective_d<=" + strGiftDate +
                           "   AND (p_partner_gift_destination.p_date_expires_d IS NULL OR p_partner_gift_destination.p_date_expires_d>=" +
                           strGiftDate + ") " +
                           " ) " +

                           " WHERE " +
                           " p_person.p_partner_key_n=pm_staff_data.p_partner_key_n " +
                           " AND pm_staff_data.pm_receiving_field_n=RecvField.p_partner_key_n " +
                           " AND p_person.p_partner_key_n=Staff.p_partner_key_n " +
                           " AND pm_staff_data.pm_start_of_commitment_d<=" + strGiftDate +
                           " AND (pm_staff_data.pm_end_of_commitment_d IS NULL OR pm_staff_data.pm_end_of_commitment_d>=" + strGiftDate + ") " +
                           " ) q " +
                           " LEFT JOIN p_partner GiftDest " +
                           " ON GiftDest.p_partner_key_n=GiftDestn " +
                           " ORDER BY StaffKey ";
            TDBTransaction Transaction = null;

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    resultTable = DbAdapter.RunQuery(Query, "GiftDestination", Transaction);

                    if (resultTable != null)
                    {
                        foreach (DataRow Row in resultTable.Rows)
                        {
                            if (Row["GiftDestnName"] is DBNull)
                            {
                                Row["GiftDestnName"] = "NONE";
                            }
                        }
                    }
                });
            return resultTable;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable TotalForRecipients(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            Int32 LedgerNum = AParameters["param_ledger_number_i"].ToInt32();
            Boolean onlySelectedTypes = AParameters["param_type_selection"].ToString() == "selected_types";
            Boolean onlySelectedFields = AParameters["param_field_selection"].ToString() == "selected_fields";
            Boolean fromExtract = AParameters["param_recipient"].ToString() == "Extract";
            Boolean oneRecipient = AParameters["param_recipient"].ToString() == "One Recipient";
            String period0Start = AParameters["param_from_date_0"].ToDate().ToString("yyyy-MM-dd");
            String period0End = AParameters["param_to_date_0"].ToDate().ToString("yyyy-MM-dd");
            String period1Start = AParameters["param_from_date_1"].ToDate().ToString("yyyy-MM-dd");
            String period1End = AParameters["param_to_date_1"].ToDate().ToString("yyyy-MM-dd");
            String period2Start = AParameters["param_from_date_2"].ToDate().ToString("yyyy-MM-dd");
            String period2End = AParameters["param_to_date_2"].ToDate().ToString("yyyy-MM-dd");
            String period3Start = AParameters["param_from_date_3"].ToDate().ToString("yyyy-MM-dd");
            String period3End = AParameters["param_to_date_3"].ToDate().ToString("yyyy-MM-dd");
            String amountFieldName = (AParameters["param_currency"].ToString() == "International") ?
                                     "detail.a_gift_amount_intl_n" : "detail.a_gift_amount_n";

            string SqlQuery = "SELECT " +
                              "recipient.p_partner_key_n AS RecipientKey, " +
                              "recipient.p_partner_short_name_c AS RecipientName, " +
//                              "RecipientType.p_type_code_c AS RecipientType, " +
//                              "detail.a_recipient_ledger_number_n AS RecipientField, " +
                              "SUM(CASE WHEN gift.a_date_entered_d BETWEEN '" + period0Start + "' AND '" + period0End + "' " +
                              "THEN " + amountFieldName + " ELSE 0 END )as YearTotal0, " +
                              "SUM(CASE WHEN gift.a_date_entered_d BETWEEN '" + period1Start + "' AND '" + period1End + "' " +
                              "THEN " + amountFieldName + " ELSE 0 END )as YearTotal1, " +
                              "SUM(CASE WHEN gift.a_date_entered_d BETWEEN '" + period2Start + "' AND '" + period2End + "' " +
                              "THEN " + amountFieldName + " ELSE 0 END )as YearTotal2, " +
                              "SUM(CASE WHEN gift.a_date_entered_d BETWEEN '" + period3Start + "' AND '" + period3End + "' " +
                              "THEN " + amountFieldName + " ELSE 0 END )as YearTotal3 " +
                              "FROM PUB_a_gift as gift, PUB_a_gift_detail as detail, PUB_a_gift_batch AS GiftBatch, PUB_p_partner AS recipient "
            ;

            if (fromExtract)
            {
                String extractName = AParameters["param_extract_name"].ToString();
                SqlQuery += (", PUB_m_extract AS Extract, PUB_m_extract_master AS ExtractMaster " +
                             "WHERE " +
                             "recipient.p_partner_key_n = Extract.p_partner_key_n " +
                             "AND Extract.m_extract_id_i = ExtractMaster.m_extract_id_i " +
                             "AND ExtractMaster.m_extract_name_c = '" + extractName + "' " +
                             "AND "
                             );
            }
            else
            {
                SqlQuery += "WHERE ";
            }

            SqlQuery += ("detail.a_ledger_number_i = " + LedgerNum +
                         " AND detail.p_recipient_key_n = recipient.p_partner_key_n " +
                         " AND detail.a_batch_number_i = gift.a_batch_number_i " +
                         " AND detail.a_gift_transaction_number_i = gift.a_gift_transaction_number_i " +
                         " AND gift.a_date_entered_d BETWEEN '" + period3Start + "' AND '" + period0End + "' " +
                         " AND gift.a_ledger_number_i = " + LedgerNum +
                         " AND GiftBatch.a_batch_status_c = 'Posted' " +
                         " AND GiftBatch.a_batch_number_i = gift.a_batch_number_i " +
                         " AND GiftBatch.a_ledger_number_i = " + LedgerNum
                         );

            if (oneRecipient)
            {
                String recipientKey = AParameters["param_recipient_key"].ToString();
                SqlQuery += (" AND recipient.p_partner_key_n = " + recipientKey);
            }
            else
            {
                if (onlySelectedTypes)
                {
                    String selectedTypeList = AParameters["param_clbTypes"].ToString();
                    selectedTypeList = selectedTypeList.Replace(",", "','");

                    SqlQuery +=
                        (" AND recipient.p_partner_key_n IN (SELECT DISTINCT p_partner_key_n FROM p_partner_type WHERE p_type_code_c IN ('" +
                         selectedTypeList + "'))");
                }
            }

            if (onlySelectedFields)
            {
                String selectedFieldList = AParameters["param_clbFields"].ToString();
                selectedFieldList = selectedFieldList.Replace('\'', ' ');
                SqlQuery += (" AND detail.a_recipient_ledger_number_n IN (" + selectedFieldList + ")");
            }

            SqlQuery +=
                (
                    " GROUP by recipient.p_partner_key_n, recipient.p_partner_short_name_c" + // , RecipientType.p_type_code_c, detail.a_recipient_ledger_number_n" +
                    " ORDER BY recipient.p_partner_short_name_c");

            DataTable resultTable = new DataTable();
            TDBTransaction Transaction = null;

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    resultTable = DbAdapter.RunQuery(SqlQuery, "TotalForRecipients", Transaction);
                });
            return resultTable;
        }

        // get Actuals for this month, YTD and Prior YTD and Budget YTD
        private static Decimal[] GetActualsAndBudget(TReportingDbAdapter DbAdapter,
            ALedgerRow ALedger, string AAccountCode, string ACostCentreCode, int APeriodNumber, int AYear)
        {
            decimal[] Results = new decimal[5];

            TDBTransaction Transaction = null;
            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    Int32 ledgerPeriods = ALedger.NumberOfAccountingPeriods;
                    Int32 budgetYear = (APeriodNumber > ledgerPeriods) ? AYear + 1 : AYear;
                    Int32 budgetPeriod = (APeriodNumber > ledgerPeriods) ? APeriodNumber - ledgerPeriods : APeriodNumber;

                    String YearFilter = String.Format(" AND glm.a_year_i between {0} AND {1}", AYear - 1, budgetYear);

                    AAccountRow AccountRow = (AAccountRow)AAccountAccess.LoadByPrimaryKey(ALedger.LedgerNumber, AAccountCode, Transaction).Rows[0];
                    String AccountFilter = GetReportingAccounts(ALedger.LedgerNumber, AAccountCode, "");
                    AccountFilter = " AND glm.a_account_code_c IN ('" + AccountFilter.Replace(",", "','") + "')";
                    String CostCentreFilter = GetReportingCostCentres(ALedger.LedgerNumber, ACostCentreCode, "");
                    CostCentreFilter = " AND glm.a_cost_centre_code_c in ('" + CostCentreFilter.Replace(",",
                        "','") + "') ";

                    String subtractOrAddBase =
                        (AccountRow.DebitCreditIndicator) ?
                        "CASE WHEN debit=TRUE THEN Base ELSE 0-Base END"
                        :
                        "CASE WHEN debit=TRUE THEN 0-Base ELSE Base END";

                    String subtractOrAddLastMonth =
                        (APeriodNumber == 1) ?
                        "CASE WHEN Year=" + AYear + " THEN " +
                        (
                            (AccountRow.DebitCreditIndicator) ?
                            "CASE WHEN debit=TRUE THEN YearStart ELSE 0-YearStart END"
                            :
                            "CASE WHEN debit=TRUE THEN 0-YearStart ELSE YearStart END"
                        ) +
                        " END"
                        :
                        "CASE WHEN Year=" + AYear + " AND Period=" +
                        (APeriodNumber - 1) + " THEN " + subtractOrAddBase + " END";

                    String subtractOrAddThisYear = "CASE WHEN Year=" + AYear + " AND Period=" + APeriodNumber + " THEN " + subtractOrAddBase + " END";

                    Int32 yearAgoYear = AYear - 1;
                    Int32 yearAgoMonth = APeriodNumber;
                    String yearAgoMonthFilter = "";

                    if (APeriodNumber > ledgerPeriods)
                    {
                        yearAgoYear = AYear;
                        yearAgoMonth = APeriodNumber - ledgerPeriods;
                        yearAgoMonthFilter = yearAgoMonth.ToString() + ",";
                    }

                    String PeriodFilter =
                        (APeriodNumber > 1) ?
                        String.Format(" AND glmp.a_period_number_i IN ({0}{1},{2})", yearAgoMonthFilter, APeriodNumber - 1, APeriodNumber)
                        :
                        " AND glmp.a_period_number_i=1";

                    String subtractOrAddLastYear =
                        "CASE WHEN Year=" + yearAgoYear + " AND Period=" + yearAgoMonth +
                        " THEN " + subtractOrAddBase + " END";

                    String subtractOrAddBudget =
                        "CASE WHEN Year=" + budgetYear + " AND Period=" + budgetPeriod + " THEN " +
                        (
                            (AccountRow.DebitCreditIndicator) ?
                            "CASE WHEN debit=TRUE THEN Budget ELSE 0-Budget END"
                            :
                            "CASE WHEN debit=TRUE THEN 0-Budget ELSE Budget END"
                        ) +
                        " END";

                    String subtractOrAddYearStart =
                        (APeriodNumber > ledgerPeriods) ?
                        "CASE WHEN Year=" + AYear + " AND Period=" + ledgerPeriods +
                        " THEN " + subtractOrAddBase + " END"
                        :
                        "0";

                    String Query =
                        "SELECT sum(" + subtractOrAddLastMonth + ") AS SumLastMonthYtd," +
                        " sum(" + subtractOrAddThisYear + ") AS SumYtd," +
                        " sum(" + subtractOrAddLastYear + ") AS SumLastYear," +
                        " sum(" + subtractOrAddBudget + ") AS SumBudget, " +
                        " sum(" + subtractOrAddYearStart + ") AS SumOpeningBalance" +

                        " FROM" +
                        " (SELECT DISTINCT a_account.a_account_code_c AS AccountCode, " +
                        " glm.a_cost_centre_code_c AS CostCentreCode, " +
                        " a_account.a_debit_credit_indicator_l AS debit," +
                        " glm.a_year_i AS Year," +
                        " glm.a_start_balance_base_n AS YearStart," +
                        " glmp.a_period_number_i AS Period," +
                        " glmp.a_actual_base_n AS Base, glmp.a_budget_base_n AS Budget" +
                        " FROM a_general_ledger_master AS glm, a_general_ledger_master_period AS glmp, a_account" +
                        " WHERE glm.a_glm_sequence_i=glmp.a_glm_sequence_i" +
                        " AND glm.a_account_code_c=a_account.a_account_code_c" +
                        " AND a_account.a_ledger_number_i=" + ALedger.LedgerNumber +
                        " AND glm.a_ledger_number_i=" + ALedger.LedgerNumber +
                        YearFilter +
                        PeriodFilter +
                        AccountFilter +
                        CostCentreFilter +
                        ") AS AllGlm";

                    DataTable tempTable = DbAdapter.RunQuery(Query, "ExecSummary", Transaction);

                    if (tempTable.Rows.Count > 0)
                    {
                        Decimal sumYtd = 0;
                        Decimal sumLastMonthYtd = 0;
                        Decimal sumLastYear = 0;
                        Decimal sumBudget = 0;
                        Decimal sumOpeningBalance = 0;

                        Decimal.TryParse((tempTable.Rows[0]["SumYtd"]).ToString(), out sumYtd);
                        Decimal.TryParse((tempTable.Rows[0]["SumLastMonthYtd"]).ToString(), out sumLastMonthYtd);
                        Decimal.TryParse((tempTable.Rows[0]["SumLastYear"]).ToString(), out sumLastYear);
                        Decimal.TryParse((tempTable.Rows[0]["SumBudget"]).ToString(), out sumBudget);
                        Decimal.TryParse((tempTable.Rows[0]["SumOpeningBalance"]).ToString(), out sumOpeningBalance);

                        Results[0] = sumYtd - sumLastMonthYtd;
                        Results[1] = sumLastYear;
                        Results[2] = sumYtd - sumOpeningBalance;
                        Results[3] = sumBudget;
                    }

                    //Get BudgetYTD. The budget figures in the glmp are different to the actuals, as they are simply per period amounts
                    String subtractOrAddBudgetYTD =
                        "CASE WHEN Year=" + budgetYear + " AND Period<=" + budgetPeriod + " THEN " +
                        (
                            (AccountRow.DebitCreditIndicator) ?
                            "CASE WHEN debit=TRUE THEN Budget ELSE 0-Budget END"
                            :
                            "CASE WHEN debit=TRUE THEN 0-Budget ELSE Budget END"
                        ) +
                        " END";

                    String YearFilter2 = String.Format(" AND glm.a_year_i = {0}", budgetYear);
                    String PeriodFilter2 =
                        (APeriodNumber > 1) ?
                        String.Format(" AND glmp.a_period_number_i between 1 AND {0}", budgetPeriod)
                        :
                        " AND glmp.a_period_number_i=1";

                    String Query2 =
                        "SELECT sum(" + subtractOrAddBudgetYTD + ") AS SumBudgetYTD" +
                        " FROM" +
                        " (SELECT DISTINCT a_account.a_account_code_c AS AccountCode, " +
                        " glm.a_cost_centre_code_c AS CostCentreCode, " +
                        " a_account.a_debit_credit_indicator_l AS debit," +
                        " glm.a_year_i AS Year," +
                        " glm.a_start_balance_base_n AS YearStart," +
                        " glmp.a_period_number_i AS Period," +
                        " glmp.a_actual_base_n AS Base, glmp.a_budget_base_n AS Budget" +
                        " FROM a_general_ledger_master AS glm, a_general_ledger_master_period AS glmp, a_account" +
                        " WHERE glm.a_glm_sequence_i=glmp.a_glm_sequence_i" +
                        " AND glm.a_account_code_c=a_account.a_account_code_c" +
                        " AND a_account.a_ledger_number_i=" + ALedger.LedgerNumber +
                        " AND glm.a_ledger_number_i=" + ALedger.LedgerNumber +
                        YearFilter2 +
                        PeriodFilter2 +
                        AccountFilter +
                        CostCentreFilter +
                        ") AS AllGlm";

                    DataTable tempTable2 = DbAdapter.RunQuery(Query2, "ExecSummary2", Transaction);

                    if (tempTable2.Rows.Count > 0)
                    {
                        Decimal sumBudgetYTD = 0;

                        Decimal.TryParse((tempTable2.Rows[0]["SumBudgetYTD"]).ToString(), out sumBudgetYTD);

                        Results[4] = sumBudgetYTD;
                    }

                    /*
                     *          int GLMSeqLastYear = TCommonBudgetMaintain.GetGLMSequenceForBudget(
                     *              ALedger.LedgerNumber, AAccountCode, ACostCentreCode, AYear - 1);
                     *          int GLMSeqThisYear = TCommonBudgetMaintain.GetGLMSequenceForBudget(
                     *              ALedger.LedgerNumber, AAccountCode, ACostCentreCode, AYear);
                     *          int GLMSeqNextYear = TCommonBudgetMaintain.GetGLMSequenceForBudget(
                     *              ALedger.LedgerNumber, AAccountCode, ACostCentreCode, AYear + 1);
                     *
                     *          Results[0] = TCommonBudgetMaintain.GetActual(ALedger.LedgerNumber,
                     *              GLMSeqThisYear,
                     *              -1,
                     *              APeriodNumber,
                     *              ALedger.NumberOfAccountingPeriods,
                     *              ALedger.CurrentFinancialYear,
                     *              AYear,
                     *              false,
                     *              MFinanceConstants.CURRENCY_BASE);
                     *          Results[1] = TCommonBudgetMaintain.GetActual(ALedger.LedgerNumber,
                     *              GLMSeqLastYear,
                     *              GLMSeqThisYear,
                     *              APeriodNumber,
                     *              ALedger.NumberOfAccountingPeriods,
                     *              ALedger.CurrentFinancialYear,
                     *              AYear - 1,
                     *              true,
                     *              MFinanceConstants.CURRENCY_BASE);
                     *          Results[2] = TCommonBudgetMaintain.GetActual(ALedger.LedgerNumber,
                     *              GLMSeqThisYear,
                     *              -1,
                     *              APeriodNumber,
                     *              ALedger.NumberOfAccountingPeriods,
                     *              ALedger.CurrentFinancialYear,
                     *              AYear,
                     *              true,
                     *              MFinanceConstants.CURRENCY_BASE);
                     *          Results[3] = TCommonBudgetMaintain.GetBudget(GLMSeqThisYear,
                     *              GLMSeqNextYear,
                     *              APeriodNumber,
                     *              ALedger.NumberOfAccountingPeriods,
                     *              true,
                     *              MFinanceConstants.CURRENCY_BASE);
                     */
                });
            return Results;
        }

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable TrialBalanceTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            /* Required columns:
             *   CostCentreCode
             *   CostCentreName
             *   AccountCode
             *   AccountName
             *   Debit
             *   Credit
             */


            /*
             * Trial balance is simply a list of all the account / cost centre balances, at the end of the period specified.
             * (If the period is open, it still works.)
             *
             * Trial balance works on Posting accounts and cost centres, so there's no chasing up the hierarchy tree.
             */

            Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            Int32 AccountingYear = AParameters["param_year_i"].ToInt32();
            Int32 ReportPeriodEnd = AParameters["param_end_period_i"].ToInt32();
            bool YTDBalance = AParameters["param_chkYTD"].ToBool();

            //
            // Read different DB fields according to currency setting
            // NOTE: Modified August 2016 to return base * exchangeRate, rather than the recorded a_actual_intl_n
            TCorporateExchangeRateCache ExchangeRateCache = new TCorporateExchangeRateCache();
            TLedgerInfo ledgerInfo = new TLedgerInfo(LedgerNumber);

            Decimal exchangeRateNow = ExchangeRateCache.GetCorporateExchangeRate(DbAdapter.FPrivateDatabaseObj,
                LedgerNumber,
                ledgerInfo.CurrentFinancialYear,
                ledgerInfo.CurrentPeriod,
                -1);

            String ActualFieldName = AParameters["param_currency"].ToString().StartsWith("Int")
                                     ?
                                     "a_actual_base_n * " + exchangeRateNow
                                     :
                                     "a_actual_base_n";

            String CostCentreFilter;
            String AccountCodeFilter;

            // create filters from parameters
            AccountAndCostCentreFilters(AParameters, out CostCentreFilter, out AccountCodeFilter);

            TDBTransaction ReadTrans = null;
            DataTable resultTable = null;
            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTrans,
                delegate
                {
                    String OrderBy = " ORDER BY a_cost_centre.a_cost_centre_type_c DESC, glm.a_cost_centre_code_c, glm.a_account_code_c";

                    if (AParameters["param_sortby"].ToString() == "Account")
                    {
                        OrderBy = " ORDER BY glm.a_account_code_c, a_cost_centre.a_cost_centre_type_c DESC, glm.a_cost_centre_code_c";
                    }

                    String Query = "SELECT DISTINCT" +
                                   " glm.a_year_i AS Year," +
                                   " glmp.a_period_number_i AS Period," +
                                   " glm.a_cost_centre_code_c AS CostCentreCode," +
                                   " a_cost_centre.a_cost_centre_name_c AS CostCentreName," +
                                   " a_cost_centre.a_cost_centre_type_c AS CostCentreType," +
                                   " a_account.a_debit_credit_indicator_l AS IsDebit,";

                    if (YTDBalance)
                    {
                        Query += " glmp." + ActualFieldName + " AS Balance,";
                    }
                    else
                    {
                        Query += " (glmp." + ActualFieldName + " - glmpPrevious." + ActualFieldName + ") AS Balance,";
                    }

                    Query += " 0.0 as Debit, " +
                             " 0.0 as Credit, " +
                             " glm.a_account_code_c AS AccountCode," +
                             " a_account.a_account_code_short_desc_c AS AccountName" +

                             " FROM a_general_ledger_master AS glm, a_general_ledger_master_period AS glmp, a_account, a_cost_centre";

                    if (!YTDBalance)
                    {
                        Query += ", a_general_ledger_master_period AS glmpPrevious";
                    }

                    Query += " WHERE glm.a_ledger_number_i=" + LedgerNumber +
                             " AND glm.a_year_i=" + AccountingYear +
                             " AND glm.a_glm_sequence_i = glmp.a_glm_sequence_i" +
                             " AND glmp.a_period_number_i=" + ReportPeriodEnd +
                             " AND glmp." + ActualFieldName + " <> 0";

                    if (!YTDBalance)
                    {
                        Query += " AND glm.a_glm_sequence_i = glmpPrevious.a_glm_sequence_i" +
                                 " AND glmpPrevious.a_period_number_i = " + (ReportPeriodEnd - 1);
                    }

                    Query += " AND a_account.a_account_code_c = glm.a_account_code_c" +
                             " AND a_account.a_ledger_number_i = glm.a_ledger_number_i" +
                             " AND a_account.a_posting_status_l = true" +
                             " AND a_cost_centre.a_ledger_number_i = glm.a_ledger_number_i" +
                             " AND a_cost_centre.a_cost_centre_code_c = glm.a_cost_centre_code_c" +
                             " AND a_cost_centre.a_posting_cost_centre_flag_l = true" +
                             CostCentreFilter +
                             AccountCodeFilter +
                             OrderBy;
                    TLogging.Log(Catalog.GetString("Loading data.."), TLoggingType.ToStatusBar);
                    resultTable = DbAdapter.RunQuery(Query, "TrialBalance", ReadTrans);

                    foreach (DataRow Row in resultTable.Rows)
                    {
                        Decimal Amount = Convert.ToDecimal(Row["Balance"]);
                        Boolean IsDebit = Convert.ToBoolean(Row["IsDebit"]);

                        if (Amount < 0)
                        {
                            IsDebit = !IsDebit;
                            Amount = 0 - Amount;
                        }

                        String ToField = IsDebit ? "Debit" : "Credit";
                        Row[ToField] = Amount;
                    }

                    TLogging.Log("", TLoggingType.ToStatusBar);
                }); // Get NewOrExisting AutoReadTransaction

            return resultTable;
        } // Trial Balance Table

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable SurplusDeficitTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            Int32 AccountingYear = AParameters["param_year_i"].ToInt32();
            Int32 ReportPeriodEnd = AParameters["param_end_period_i"].ToInt32();

            // Read different DB fields according to currency setting
            //String ActualFieldName = AParameters["param_currency"].ToString().StartsWith("Int") ? "a_actual_intl_n" : "a_actual_base_n";

            String CostCentreFilter;
            String AccountCodeFilter;

            // create filters from parameters
            AccountAndCostCentreFilters(AParameters, out CostCentreFilter, out AccountCodeFilter);

            DataTable resultTable = null;
            TDBTransaction ReadTrans = null;
            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTrans,
                delegate
                {
                    TCorporateExchangeRateCache ExchangeRateCache = new TCorporateExchangeRateCache();
                    TLedgerInfo ledgerInfo = new TLedgerInfo(LedgerNumber);

                    decimal ExchangeRateNow = 1;

                    if (AParameters["param_currency"].ToString().StartsWith("Int"))
                    {
                        ExchangeRateNow = ExchangeRateCache.GetCorporateExchangeRate(DbAdapter.FPrivateDatabaseObj,
                            LedgerNumber,
                            ledgerInfo.CurrentFinancialYear,
                            ledgerInfo.CurrentPeriod,
                            -1);
                    }

                    String OrderBy = " ORDER BY glm.a_cost_centre_code_c";

                    String Query = "SELECT " +
                                   " glm.a_year_i AS Year," +
                                   " glmp.a_period_number_i AS Period," +
                                   " glm.a_cost_centre_code_c AS CostCentreCode," +
                                   " a_cost_centre.a_cost_centre_name_c AS CostCentreName," +
                                   " a_cost_centre.a_cost_centre_type_c AS CostCentreType," +

                                   " SUM (CASE" +
                                   " WHEN (a_account.a_debit_credit_indicator_l = false AND glmp.a_actual_base_n > 0)" +
                                   " OR (a_account.a_debit_credit_indicator_l = true AND glmp.a_actual_base_n < 0)" +
                                   " THEN ABS(glmp.a_actual_base_n) * " + ExchangeRateNow +
                                   " ELSE 0 END) AS Credit," +
                                   " SUM (CASE" +
                                   " WHEN (a_account.a_debit_credit_indicator_l = true AND glmp.a_actual_base_n > 0)" +
                                   " OR (a_account.a_debit_credit_indicator_l = false AND glmp.a_actual_base_n < 0)" +
                                   " THEN ABS(glmp.a_actual_base_n) * " + ExchangeRateNow +
                                   " ELSE 0 END) AS Debit" +

                                   " FROM a_general_ledger_master AS glm, a_general_ledger_master_period AS glmp, a_account, a_cost_centre" +
                                   " WHERE glm.a_ledger_number_i=" + LedgerNumber +
                                   " AND glm.a_year_i=" + AccountingYear +
                                   " AND glm.a_glm_sequence_i = glmp.a_glm_sequence_i" +
                                   " AND glmp.a_period_number_i=" + ReportPeriodEnd +
                                   " AND glmp.a_actual_base_n <> 0" +

                                   " AND a_account.a_account_code_c = glm.a_account_code_c" +
                                   " AND a_account.a_ledger_number_i = glm.a_ledger_number_i" +
                                   " AND a_account.a_posting_status_l = true" +
                                   " AND a_cost_centre.a_ledger_number_i = glm.a_ledger_number_i" +
                                   " AND a_cost_centre.a_cost_centre_code_c = glm.a_cost_centre_code_c" +
                                   " AND a_cost_centre.a_posting_cost_centre_flag_l = true" +
                                   CostCentreFilter +
                                   AccountCodeFilter +
                                   " GROUP BY Year, Period, CostCentreCode, CostCentreName, CostCentreType " +
                                   OrderBy;

                    TLogging.Log(Catalog.GetString("Loading data.."), TLoggingType.ToStatusBar);
                    resultTable = DbAdapter.RunQuery(Query, "SurplusDeficitTable", ReadTrans);

                    TLogging.Log("", TLoggingType.ToStatusBar);
                }); // Get NewOrExisting AutoReadTransaction
            return resultTable;
        } // Surplus Deficit Table

        /// <summary>
        /// Retrieve a list of all the Cost Centres the user has chosen
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ADbConnection"></param>
        /// <returns></returns>
        private static List <String>GetCostCentreList(Dictionary <String, TVariant>AParameters, TDataBase ADbConnection)
        {
            List <String>MatchingCostCentres = new List <String>();
            String CostCentreOptions =
                AParameters.ContainsKey("param_costcentreoptions") ? AParameters["param_costcentreoptions"].ToString() : "Unspecified";
            Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();

            String Query = // This "AllActiveCostCentres" query is the default, if CostCentreOptions isn't set properly:
                           "SELECT a_cost_centre_code_c AS CC, a_cost_centre_name_c AS Name FROM a_cost_centre WHERE a_ledger_number_i= " +
                           LedgerNumber +
                           " AND a_cost_centre_active_flag_l=TRUE" +
                           " AND a_posting_cost_centre_flag_l=TRUE" +
                           " AND a_cost_centre_type_c='Local'";

            switch (CostCentreOptions)
            {
                case "AllCostCentres":
                {
                    Query = "SELECT a_cost_centre_code_c AS CC, a_cost_centre_name_c AS Name FROM a_cost_centre WHERE a_ledger_number_i= " +
                            LedgerNumber +
                            " AND a_posting_cost_centre_flag_l=TRUE" +
                            " AND a_cost_centre_type_c='Local'";
                    break;
                } // "AllCostCentres"

                case "SelectedCostCentres":
                {
                    String CostCentreList = AParameters["param_cost_centre_codes"].ToString();
                    CostCentreList = CostCentreList.Replace(",", "','");                           // SQL IN List items in single quotes
                    Query = "SELECT a_cost_centre_code_c AS CC, a_cost_centre_name_c AS Name FROM a_cost_centre WHERE a_ledger_number_i= " +
                            LedgerNumber +
                            " AND a_cost_centre_code_c in('" + CostCentreList + "')";
                    break;
                } // "SelectedCostCentres"

                case "CostCentreRange":
                {
                    Query = "SELECT a_cost_centre_code_c AS CC, a_cost_centre_name_c AS Name FROM a_cost_centre WHERE a_ledger_number_i= " +
                            LedgerNumber +
                            " AND a_posting_cost_centre_flag_l=TRUE" +
                            " AND a_cost_centre_code_c >='" + AParameters["param_cost_centre_code_start"].ToString() +
                            "' AND a_cost_centre_code_c <='" + AParameters["param_cost_centre_code_end"].ToString() + "'";
                    break;
                } // "CostCentreRange"

                case "AccountLevel":
                {
                    string StandardSummaryCostCentre = LedgerNumber.ToString("00") + "00S";
                    Query = "SELECT a_cost_centre_code_c AS CC, a_cost_centre_name_c AS Name FROM a_cost_centre WHERE a_ledger_number_i= " +
                            LedgerNumber +
                            " AND a_cost_centre_code_c='" + StandardSummaryCostCentre + "'";
                    break;
                } // "AccountLevel"
            } // switch

            TDBTransaction Transaction = null;

            ADbConnection.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    DataTable tbl = ADbConnection.SelectDT(Query, "CC", Transaction);

                    foreach (DataRow Row in tbl.Rows)
                    {
                        MatchingCostCentres.Add(Row["CC"].ToString() + "," + Row["Name"].ToString());
                    }
                }); // Get NewOrExisting AutoReadTransaction

            return MatchingCostCentres;
        }

        private static void AccountAndCostCentreFilters(Dictionary <String, TVariant>AParameters,
            out string ACostCentreFilter,
            out string AAccountCodeFilter)
        {
            ACostCentreFilter = "";
            String CostCentreOptions = AParameters["param_rgrCostCentres"].ToString();

            if (CostCentreOptions == "CostCentreList")
            {
                String CostCentreList = AParameters["param_cost_centre_codes"].ToString();
                CostCentreList = CostCentreList.Replace(",", "','");                             // SQL IN List items in single quotes
                ACostCentreFilter = " AND glm.a_cost_centre_code_c in ('" + CostCentreList + "')";
            }

            if (CostCentreOptions == "CostCentreRange")
            {
                ACostCentreFilter = " AND glm.a_cost_centre_code_c >='" + AParameters["param_cost_centre_code_start"].ToString() +
                                    "' AND glm.a_cost_centre_code_c <='" + AParameters["param_cost_centre_code_end"].ToString() + "'";
            }

            if (CostCentreOptions == "AllActiveCostCentres") // THIS IS NOT SET AT ALL!
            {
                ACostCentreFilter = " AND a_cost_centre.a_cost_centre_active_flag_l=true";
            }

            AAccountCodeFilter = "";
            String AccountCodeOptions = AParameters["param_rgrAccounts"].ToString();

            if (AccountCodeOptions == "AccountList")
            {
                String AccountCodeList = AParameters["param_account_codes"].ToString();
                AccountCodeList = AccountCodeList.Replace(",", "','");                             // SQL IN List items in single quotes
                AAccountCodeFilter = " AND glm.a_account_code_c in ('" + AccountCodeList + "')";
            }

            if (AccountCodeOptions == "AccountRange")
            {
                AAccountCodeFilter = " AND glm.a_account_code_c >='" + AParameters["param_account_code_start"].ToString() +
                                     "' AND glm.a_account_code_c <='" + AParameters["param_account_code_end"].ToString() + "'";
            }

            if (AccountCodeOptions == "AllActiveAccounts") // THIS IS NOT SET AT ALL
            {
                AAccountCodeFilter = " AND a_account.a_account_active_flag_l=true";
            }
        }

        /// <summary>
        /// Returns the transaction currency for a Gift Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="APrivateDB"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static string GetTransactionCurrency(int ALedgerNumber, int ABatchNumber, Boolean APrivateDB)
        {
            TDBTransaction Transaction = null;
            TDataBase dbConnection = APrivateDB ? TReportingDbAdapter.EstablishDBConnection(APrivateDB, "GetTransactionCurrency")
                                     : DBAccess.GDBAccessObj;
            string ReturnValue = "";

            dbConnection.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    AGiftBatchTable GiftBatchTable = AGiftBatchAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, Transaction);

                    if ((GiftBatchTable != null) && (GiftBatchTable.Rows.Count > 0))
                    {
                        ReturnValue = GiftBatchTable[0].CurrencyCode;
                    }
                }); // Get NewOrExisting AutoReadTransaction

            if (APrivateDB)
            {
                dbConnection.CloseDBConnection();
            }

            return ReturnValue;
        } // Get Transaction Currency
    }
}
