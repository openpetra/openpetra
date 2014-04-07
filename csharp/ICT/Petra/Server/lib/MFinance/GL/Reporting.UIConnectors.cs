//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Setup.WebConnectors;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MCommon;
using System.Data.Common;

namespace Ict.Petra.Server.MFinance.Reporting.WebConnectors
{
    ///<summary>
    /// This WebConnector provides data for the finance reporting screens
    ///</summary>
    public class TFinanceReportingWebConnector
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
        public static DataTable GetReceivingFields(int ALedgerNumber, out String ADisplayMember, out String AValueMember)
        {
            DataTable ReturnTable = new DataTable();
            String sql;

            TDBTransaction ReadTransaction;

            ADisplayMember = "FieldName";
            AValueMember = "FieldKey";

            ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction();
            try
            {
                sql = "SELECT DISTINCT " + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName() + " AS " + AValueMember +
                      ", " +
                      PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerShortNameDBName() + " AS " + ADisplayMember +
                      " FROM " + PPartnerTable.GetTableDBName() + ", " +
                      PPartnerTypeTable.GetTableDBName() +
                      " WHERE " +
                      PPartnerTypeTable.GetTableDBName() + "." + PPartnerTypeTable.GetPartnerKeyDBName() + " = " + PPartnerTable.GetTableDBName() +
                      "." + PPartnerTable.GetPartnerKeyDBName() +
                      " AND (" + PPartnerTypeTable.GetTableDBName() + "." + PPartnerTypeTable.GetTypeCodeDBName() + " = 'LEDGER' OR " +
                      PPartnerTypeTable.GetTableDBName() + "." + PPartnerTypeTable.GetTypeCodeDBName() + " = 'COSTCENTRE' " +
                      ") ORDER BY " + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerShortNameDBName();

                ReturnTable = DBAccess.GDBAccessObj.SelectDT(sql, "BatchYearTable", ReadTransaction);
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
            return ReturnTable;
        }

        private static void GetReportingCostCentres(ACostCentreTable ACostCentres, ref List <string>AResult, string ASummaryCostCentreCode)
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
                            GetReportingCostCentres(ACostCentres, ref AResult, row.CostCentreCode);
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

            GetReportingCostCentres(CachedDataTable, ref Result, ASummaryCostCentreCode);

            List <string>IgnoreCostCentres = new List <string>();

            GetReportingCostCentres(CachedDataTable, ref IgnoreCostCentres, ARemoveCostCentresFromList);

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
            ref List <string>AResult,
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

                        GetReportingAccounts(AAccountHierarchyDetail, ref AResult, row.ReportingAccountCode, AAccountHierarchy);
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


            GetReportingAccounts(MainDS.AAccountHierarchyDetail, ref accountcodes, ASummaryAccountCodes, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD);

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

        /// <summary>
        /// Get the name for this Ledger
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static string GetLedgerName(int ledgernumber)
        {
            String ReturnValue = "";
            String strSql = "SELECT p_partner_short_name_c FROM PUB_a_ledger, PUB_p_partner WHERE a_ledger_number_i=" +
                            StringHelper.IntToStr(ledgernumber) + " and PUB_a_ledger.p_partner_key_n = PUB_p_partner.p_partner_key_n";
            DataTable tab = DBAccess.GDBAccessObj.SelectDT(strSql, "GetLedgerName_TempTable", null);

            if (tab.Rows.Count > 0)
            {
                ReturnValue = Convert.ToString(tab.Rows[0]["p_partner_short_name_c"]);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Collects the period opening / closing balances for the Account Detail report
        /// </summary>
        /// <param name="ALedgerFilter"></param>
        /// <param name="AAccountCodeFilter"></param>
        /// <param name="ACostCentreFilter"></param>
        /// <param name="AStartPeriod"></param>
        /// <param name="AEndPeriod"></param>
        /// <param name="AInternational"></param>
        /// <returns>DataTable</returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable GetPeriodBalances(String ALedgerFilter,
            String AAccountCodeFilter,
            String ACostCentreFilter,
            Int32 AStartPeriod,
            Int32 AEndPeriod,
            Boolean AInternational)
        {
            Boolean FromStartOfYear = (AStartPeriod == 1);
            TReportingDbAdapter DbAdapter = new TReportingDbAdapter();

            if (!FromStartOfYear)
            {
                AStartPeriod -= 1; // I want the closing balance of the previous period.
            }

            String Query = "SELECT * FROM a_ledger WHERE " + ALedgerFilter;
            DataTable LedgerTable = DBAccess.GDBAccessObj.SelectDT(Query, "Ledger", null);
            Int32 FiancialYear = Convert.ToInt32(LedgerTable.Rows[0]["a_current_financial_year_i"]);

            String BalanceField = (AInternational) ? "glmp.a_actual_intl_n" : "glmp.a_actual_base_n";
            String StartBalanceField = (AInternational) ? "glm.a_start_balance_intl_n" : "glm.a_start_balance_base_n";

            Query = "SELECT glm.a_cost_centre_code_c, glm.a_account_code_c, glmp.a_period_number_i, " +
                    StartBalanceField + " AS start_balance, " +
                    BalanceField + " AS balance " +
                    " FROM a_general_ledger_master AS glm, a_general_ledger_master_period AS glmp" +
                    " WHERE glm." + ALedgerFilter +
                    " AND glm.a_year_i = " + FiancialYear +
                    AAccountCodeFilter +
                    ACostCentreFilter +
                    " AND glm.a_glm_sequence_i = glmp.a_glm_sequence_i" +
                    " AND glmp.a_period_number_i >= " + AStartPeriod +
                    " AND glmp.a_period_number_i <= " + AEndPeriod +
                    " ORDER BY glm.a_cost_centre_code_c, glm.a_account_code_c, glmp.a_period_number_i";
            DataTable GlmTbl = DbAdapter.RunQuery(Query, "balances", null);
            DataTable Results = new DataTable();
            Results.Columns.Add(new DataColumn("a_cost_centre_code_c", typeof(string)));
            Results.Columns.Add(new DataColumn("a_account_code_c", typeof(string)));
            Results.Columns.Add(new DataColumn("OpeningBalance", typeof(Decimal)));
            Results.Columns.Add(new DataColumn("ClosingBalance", typeof(Decimal)));

            String CostCentre = "";
            String AccountCode = "";
            Decimal OpeningBalance = 0;
            Decimal ClosingBalance = 0;
            Int32 MaxPeriod = -1;
            Int32 MinPeriod = 99;

            // For each CostCentre / Account combination  I want just a single row, with the opening and closing balances,
            // so I need to pre-process the stuff I've got in this table, and generate another table.

            foreach (DataRow row in GlmTbl.Rows)
            {
                if (DbAdapter.Cancelled)
                {
                    return Results;
                }

                if ((row["a_cost_centre_code_c"].ToString() != CostCentre) || (row["a_account_code_c"].ToString() != AccountCode)) // a new CC/AC combination
                {
                    if ((CostCentre != "") && (AccountCode != "")) // Add a new row, but not if there's no data yet.
                    {
                        DataRow NewRow = Results.NewRow();
                        NewRow["a_cost_centre_code_c"] = CostCentre;
                        NewRow["a_account_code_c"] = AccountCode;
                        NewRow["OpeningBalance"] = OpeningBalance;
                        NewRow["ClosingBalance"] = ClosingBalance;
                        Results.Rows.Add(NewRow);
                    }

                    CostCentre = row["a_cost_centre_code_c"].ToString();
                    AccountCode = row["a_account_code_c"].ToString();
                    MaxPeriod = -1;
                    MinPeriod = 99;
                }

                Int32 ThisPeriod = Convert.ToInt32(row["a_period_number_i"]);

                if (ThisPeriod < MinPeriod)
                {
                    MinPeriod = ThisPeriod;
                    OpeningBalance = (FromStartOfYear) ? Convert.ToDecimal(row["start_balance"]) : Convert.ToDecimal(row["balance"]);
                }

                if (ThisPeriod > MaxPeriod)
                {
                    MaxPeriod = ThisPeriod;
                    ClosingBalance = Convert.ToDecimal(row["balance"]);
                }
            }

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
        /// Utility function for IncomeExpenseTable.
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
            Boolean ByPeriod,
            out string ParentAccountPath,
            out Int32 ParentAccountTypeOrder,
            TDBTransaction ReadTrans)
        {
            Int32 Idx = HierarchyTbl.DefaultView.Find(AccountCode);
            // If Idx < 0 that's pretty serious. The next line will raise an exception.
            AAccountHierarchyDetailRow HDRow = (AAccountHierarchyDetailRow)HierarchyTbl.DefaultView[Idx].Row;
            String ParentAccountCode = HDRow.AccountCodeToReportTo;

            if (((filteredResults.TableName == "IncomeExpense") && (ParentAccountCode == "RET EARN")) // This is a literal AccountCode, which (in a non-OM setting) the user could potentially change.
               || (ParentAccountCode == LedgerNumber.ToString()))                                     // And actually this is effectively a literal too.
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

                Int32 Period = (ByPeriod) ? Convert.ToInt32(NewDataRow["Period"]) : 0;
                String PeriodField = "P" + Period;

                Decimal NewRowActual = Convert.ToDecimal(NewDataRow["Actual"]);

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
                    // but could be overwritten since the "top level" AccountTypeOrder end up getting copied to all children.

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
                        ParentRow[PeriodField] = Sign * NewRowActual;
                        ParentRow["Actual"] = 0;
                        ParentRow["ActualYTD"] = 0;
                        ParentRow["Budget"] = 0;
                        ParentRow["BudgetYTD"] = 0;
                        ParentRow["Period"] = 0;
                        ParentRow["ActualLastYear"] = 0;
//                      ParentRow["ActualLastYearComplete"] = 0;
                    }
                    else
                    {
                        ParentRow["Actual"] = Sign * NewRowActual;
                        ParentRow["ActualYTD"] = Sign * Convert.ToDecimal(ParentRow["ActualYTD"]);
                        ParentRow["ActualLastYear"] = Sign * Convert.ToDecimal(ParentRow["ActualLastYear"]);
//                      ParentRow["ActualLastYearComplete"] = Sign * Convert.ToDecimal(ParentRow["ActualLastYearComplete"]);
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
                        ByPeriod,
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
                        ParentRow[PeriodField] = Convert.ToDecimal(ParentRow[PeriodField]) + (Sign * NewRowActual);
                    }
                    else
                    {
                        ParentRow["Actual"] = Convert.ToDecimal(ParentRow["Actual"]) + (Sign * NewRowActual);
                        ParentRow["ActualYTD"] = Convert.ToDecimal(ParentRow["ActualYTD"]) + (Sign * Convert.ToDecimal(NewDataRow["ActualYTD"]));
                        ParentRow["ActualLastYear"] = Convert.ToDecimal(ParentRow["ActualLastYear"]) +
                                                      (Sign * Convert.ToDecimal(NewDataRow["ActualLastYear"]));

//                      ParentRow["ActualLastYearComplete"] = Convert.ToDecimal(ParentRow["ActualLastYearComplete"]) + (Sign * Convert.ToDecimal(NewDataRow["ActualLastYearComplete"]));

                        //
                        // The Balance Sheet row doesn't have budgets, but the Income Expense does:
                        if (ParentRow.Table.Columns.Contains("Budget"))
                        {
                            ParentRow["Budget"] = Convert.ToDecimal(ParentRow["Budget"]) + (Sign * Convert.ToDecimal(NewDataRow["Budget"]));
                            ParentRow["BudgetYTD"] = Convert.ToDecimal(ParentRow["BudgetYTD"]) + (Sign * Convert.ToDecimal(NewDataRow["BudgetYTD"]));
                            ParentRow["BudgetLastYear"] = Convert.ToDecimal(ParentRow["BudgetLastYear"]) +
                                                          (Sign * Convert.ToDecimal(NewDataRow["BudgetLastYear"]));
                            ParentRow["WholeYearBudget"] = Convert.ToDecimal(ParentRow["WholeYearBudget"]) +
                                                           (Sign * Convert.ToDecimal(NewDataRow["WholeYearBudget"]));
                        }
                    }
                    //
                    // Now ensure that my parent also updates her own parent:

                    Int32 AccountLevel = AddTotalsToParentAccountRow( // Update my parent first
                        filteredResults,
                        HierarchyTbl,
                        LedgerNumber,
                        CostCentreCode,
                        ParentAccountCode,
                        NewDataRow,
                        SortAccountFirst,
                        ByPeriod,
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
        /// so it does not rely on the summarisation in GLMP.
        /// </summary>
        [NoRemoting]
        public static DataTable BalanceSheetTable(Dictionary<String, TVariant> AParameters, TReportingDbAdapter DbAdapter)
        {
            DataTable FilteredResults = null;

            /* Required columns:
             * Actual
             * ActualLastYear
             * ActualLastYearComplete
             */

            Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            Int32 AccountingYear = AParameters["param_year_i"].ToInt32();
            Int32 ReportPeriodEnd = AParameters["param_end_period_i"].ToInt32();
            String HierarchyName = AParameters["param_account_hierarchy_c"].ToString();
            String RootCostCentre = AParameters["param_cost_centre_code"].ToString();

            //
            // Read different DB fields according to currency setting
            Boolean International = AParameters["param_currency"].ToString().StartsWith("Int");
            String ActualFieldName = International ? "a_actual_intl_n" : "a_actual_base_n";
            String StartBalanceFieldName = International ? "a_start_balance_intl_n" : "a_start_balance_base_n";

            String PlAccountCode = "PL"; // I could get this from the Ledger record, but in fact it's never set there!

            TDBTransaction ReadTrans = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                String Query = "SELECT DISTINCT" +
                               " 1 AS AccountLevel," +
                               " false AS HasChildren," +
                               " false AS ParentFooter," +
                               " glm.a_glm_sequence_i AS Seq," +
                               " glm.a_year_i AS Year," +
                               " glmp.a_period_number_i AS Period," +
                               " a_account.a_account_type_c AS AccountType," +
                               " 0 AS AccountTypeOrder," +
                               " a_account.a_debit_credit_indicator_l AS DebitCredit," +
                               " glm.a_account_code_c AS AccountCode," +
                               " glm.a_cost_centre_code_c AS CostCentreCode," +
                               " false AS AccountIsSummary," +
                               " 'Path' AS AccountPath," +
                               " a_account.a_account_code_short_desc_c AS AccountName," +
                               " glm." + StartBalanceFieldName + " AS YearStart," +
                               " 0.0 AS ActualYTD," +
                               " glmp." + ActualFieldName + " AS Actual," +
                               " 0.0 AS ActualLastYear" +

                               " FROM a_general_ledger_master AS glm, a_general_ledger_master_period AS glmp, a_account, a_cost_centre" +
                               " WHERE glm.a_ledger_number_i=" + LedgerNumber +
                               " AND glm.a_cost_centre_code_c = '" + RootCostCentre + "' " +
                               " AND glm.a_year_i IN (" + (AccountingYear - 1) + ", " + AccountingYear + ")" +
                               " AND glm.a_glm_sequence_i = glmp.a_glm_sequence_i" +
                               " AND glmp.a_period_number_i=" + ReportPeriodEnd +
                               " AND a_account.a_account_code_c = glm.a_account_code_c" +
                               " AND (a_account.a_account_type_c IN ('Asset','Liability','Equity') OR a_account.a_account_code_c = '" + PlAccountCode + "')" +
                               " AND a_account.a_ledger_number_i = glm.a_ledger_number_i" +
                               " AND (a_account.a_posting_status_l = true OR a_account.a_account_code_c = '" + PlAccountCode + "')" +
                               " ORDER BY glm.a_account_code_c"
                ;

                TLogging.Log(Catalog.GetString("Loading data.."), TLoggingType.ToStatusBar);
                DataTable resultTable = DbAdapter.RunQuery(Query, "BalanceSheet", ReadTrans);


                DataView OldPeriod = new DataView(resultTable);
                DataView ThisMonth = new DataView(resultTable);

                ThisMonth.RowFilter = String.Format("Year={0}", AccountingYear);

                //
                // Some of these rows are from a year ago. I'll copy those into the current period "LastYear" fields.

                TLogging.Log(Catalog.GetString("Get last year data.."), TLoggingType.ToStatusBar);
                foreach (DataRowView rv in ThisMonth)
                {
                    if (DbAdapter.Cancelled)
                    {
                        return FilteredResults;
                    }

                    DataRow Row = rv.Row;
                    OldPeriod.RowFilter = String.Format("Year={0} AND Period={1} AND AccountCode='{2}'",
                        AccountingYear - 1,
                        ReportPeriodEnd,
                        Row["AccountCode"].ToString()
                        );

                    if (OldPeriod.Count > 0)
                    {
                        DataRow LastYearRow = OldPeriod[0].Row;
                        Row["ActualLastYear"] = Convert.ToDecimal(LastYearRow["Actual"]);
                    }

                    if (Row["AccountCode"].ToString() == PlAccountCode)     // Tweak the PL account and pretend it's an Equity.
                    {                                                       // (It was probably previously an income account.)
                        Row["AccountType"] = "Equity";
                        Row["AccountTypeOrder"] = 3;
                    }
                }

                //
                // So now I don't have to look at last year's rows:
                FilteredResults = ThisMonth.ToTable("BalanceSheet");

                //
                // I only have "posting accounts" - I need to add the summary accounts.
                AAccountHierarchyDetailTable HierarchyTbl = AAccountHierarchyDetailAccess.LoadViaAAccountHierarchy(LedgerNumber, HierarchyName, ReadTrans);

                HierarchyTbl.DefaultView.Sort = "a_reporting_account_code_c";  // These two sort orders
                FilteredResults.DefaultView.Sort = "AccountCode";              // Are required by AddTotalsToParentAccountRow, below.

                Int32 PostingAccountRecords = FilteredResults.Rows.Count;

                TLogging.Log(Catalog.GetString("Summarise to parent accounts.."), TLoggingType.ToStatusBar);
                for (Int32 Idx = 0; Idx < PostingAccountRecords; Idx++)
                {
                    if (DbAdapter.Cancelled)
                    {
                        return FilteredResults;
                    }

                    DataRow Row = FilteredResults.Rows[Idx];
                    String ParentAccountPath;
                    Int32 ParentAccountTypeOrder;
                    Int32 AccountLevel = AddTotalsToParentAccountRow(
                        FilteredResults,
                        HierarchyTbl,
                        LedgerNumber,
                        "", // No Cost Centres on Balance Sheet
                        Row["AccountCode"].ToString(),
                        Row,
                        false,
                        false,
                        out ParentAccountPath,
                        out ParentAccountTypeOrder,
                        ReadTrans);
                    Row["AccountLevel"] = AccountLevel;
                    Row["AccountPath"] = ParentAccountPath + Row["AccountCode"];

                }

                //
                // Now if I re-order the result by AccountPath, hide all the old data and empty rows, and rows that are too detailed, it should be what I need!

                Int32 DetailLevel = AParameters["param_nesting_depth"].ToInt32();
                String DepthFilter = " AND AccountLevel<=" + DetailLevel.ToString();
                FilteredResults.DefaultView.Sort = "AccountTypeOrder, AccountPath";

                FilteredResults.DefaultView.RowFilter = "(Actual <> 0 OR ActualLastYear <> 0 )" + // Only non-zero rows
                                                        DepthFilter;    // Nothing too detailed

                FilteredResults = FilteredResults.DefaultView.ToTable("BalanceSheet");

                //
                // Finally, to make the hierarchical report possible,
                // I want to include a note to show whether a row has child rows,
                // and if it does, I'll copy this row to a new row, below the children, marking the new row as "footer".
                TLogging.Log(Catalog.GetString("Format data for reporting.."), TLoggingType.ToStatusBar);
                for (Int32 RowIdx = 0; RowIdx < FilteredResults.Rows.Count - 1; RowIdx++)
                {
                    if (DbAdapter.Cancelled)
                    {
                        return FilteredResults;
                    }

                    Int32 ParentAccountLevel = Convert.ToInt32(FilteredResults.Rows[RowIdx]["AccountLevel"]);
                    Boolean HasChildren = (Convert.ToInt32(FilteredResults.Rows[RowIdx + 1]["AccountLevel"]) > ParentAccountLevel);
                    FilteredResults.Rows[RowIdx]["HasChildren"] = HasChildren;

                    if (HasChildren)
                    {
                        Int32 NextSiblingPos = -1;

                        for (Int32 ChildIdx = RowIdx + 2; ChildIdx < FilteredResults.Rows.Count; ChildIdx++)
                        {
                            if (Convert.ToInt32(FilteredResults.Rows[ChildIdx]["AccountLevel"]) <= ParentAccountLevel)  // This row is not a child of mine
                            {                                                                                           // so I insert my footer before here.
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

                } // for
            }  // try
            catch (Exception ex) // if the report was cancelled, DB calls with the same transaction will raise exceptions.
            {
                TLogging.Log(ex.Message);
            }
                 
            finally  // Whatever happens, I need to do this:
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
            TLogging.Log("", TLoggingType.ToStatusBar);
            return FilteredResults;
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
                if (SummaryAccountPath[i] == '/')
                {
                    if (--NthSlash == 0)
                    {
                        SummaryAccountPath = SummaryAccountPath.Substring(0, i);
                        break;
                    }
                }
            }

            DataRow SummaryRow;
            Int32 ViewIdx = filteredView.Find(new object[] { NewDataRow["AccountType"], SummaryAccountPath, NewDataRow["CostCentreCode"] });

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
                SummaryRow["ActualLastYear"] = Convert.ToDecimal(SummaryRow["ActualLastYear"]) +
                                               (Sign * Convert.ToDecimal(NewDataRow["ActualLastYear"]));
                // SummaryRow["ActualLastYearComplete"] = Convert.ToDecimal(SummaryRow["ActualLastYearComplete"]) + (Sign * Convert.ToDecimal(NewDataRow["ActualLastYearComplete"]));
                SummaryRow["Budget"] = Convert.ToDecimal(SummaryRow["Budget"]) + (Sign * Convert.ToDecimal(NewDataRow["Budget"]));
                SummaryRow["BudgetYTD"] = Convert.ToDecimal(SummaryRow["BudgetYTD"]) + (Sign * Convert.ToDecimal(NewDataRow["BudgetYTD"]));
                SummaryRow["BudgetLastYear"] = Convert.ToDecimal(SummaryRow["BudgetLastYear"]) +
                                               (Sign * Convert.ToDecimal(NewDataRow["BudgetLastYear"]));
                SummaryRow["WholeYearBudget"] = Convert.ToDecimal(SummaryRow["WholeYearBudget"]) +
                                                (Sign * Convert.ToDecimal(NewDataRow["WholeYearBudget"]));
            }
        }

        /// <summary>
        /// For "Whole Year Periods Breakdown", I need to add records summarising the transactions by Period.
        /// This method will create the new row if necessary.
        /// (It is only used for "posting" accounts. The "summary" accounts have already been calculated by AddTotalsToParentAccountRow.)
        /// </summary>
        /// <param name="filteredView">A view that includes only the breakdown rows.</param>
        /// <param name="NewDataRow">This row will be removed - its "Actual" should be copied to the new summary row.</param>
        private static void AddToPeriodBreakdownSummary(DataView filteredView, DataRow NewDataRow)
        {
            DataRow SummaryRow;
            Int32 ViewIdx = filteredView.Find(new object[] { NewDataRow["CostCentreCode"], NewDataRow["AccountType"], NewDataRow["AccountPath"] });
            String PeriodField = "P" + Convert.ToInt32(NewDataRow["Period"]);
            Decimal Actual = Convert.ToDecimal(NewDataRow["Actual"]);

            if (ViewIdx < 0) // No record yet..
            {
                SummaryRow = filteredView.Table.NewRow();
                DataUtilities.CopyAllColumnValues(NewDataRow, SummaryRow);
                SummaryRow["Breakdown"] = true;
                SummaryRow[PeriodField] = Actual;
                SummaryRow["Actual"] = 0;
                SummaryRow["ActualYTD"] = 0;
                SummaryRow["Budget"] = 0;
                SummaryRow["BudgetYTD"] = 0;
                SummaryRow["Period"] = 0;
                SummaryRow["AccountLevel"] = Convert.ToInt32(NewDataRow["AccountLevel"]);
                SummaryRow["AccountPath"] = NewDataRow["AccountPath"].ToString();
                filteredView.Table.Rows.Add(SummaryRow);
            }
            else
            {
                SummaryRow = filteredView[ViewIdx].Row;
                SummaryRow[PeriodField] = Convert.ToDecimal(SummaryRow[PeriodField]) + Actual;
            }
        }

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
                AccumulatingRow["ActualLastYear"] = Convert.ToDecimal(DetailRow["ActualLastYear"]);
                // AccumulatingRow["ActualLastYearComplete"] = Convert.ToDecimal(DetailRow["ActualLastYearComplete"]);
                AccumulatingRow["Budget"] = Convert.ToDecimal(DetailRow["Budget"]);
                AccumulatingRow["BudgetYTD"] = Convert.ToDecimal(DetailRow["BudgetYTD"]);
                AccumulatingRow["BudgetLastYear"] = Convert.ToDecimal(DetailRow["BudgetLastYear"]);
                AccumulatingRow["WholeYearBudget"] = Convert.ToDecimal(DetailRow["WholeYearBudget"]);
            }
            else
            {
                AccumulatingRow["Actual"] = Convert.ToDecimal(AccumulatingRow["Actual"]) + Convert.ToDecimal(DetailRow["Actual"]);
                AccumulatingRow["ActualYTD"] = Convert.ToDecimal(AccumulatingRow["ActualYTD"]) + Convert.ToDecimal(DetailRow["ActualYTD"]);
                AccumulatingRow["ActualLastYear"] = Convert.ToDecimal(AccumulatingRow["ActualLastYear"]) +
                                                    Convert.ToDecimal(DetailRow["ActualLastYear"]);
                // AccumulatingRow["ActualLastYearComplete"] = Convert.ToDecimal(AccumulatingRow["ActualLastYearComplete"]) + Convert.ToDecimal(DetailRow["ActualLastYearComplete"]);
                AccumulatingRow["Budget"] = Convert.ToDecimal(AccumulatingRow["Budget"]) + Convert.ToDecimal(DetailRow["Budget"]);
                AccumulatingRow["BudgetYTD"] = Convert.ToDecimal(AccumulatingRow["BudgetYTD"]) + Convert.ToDecimal(DetailRow["BudgetYTD"]);
                AccumulatingRow["BudgetLastYear"] = Convert.ToDecimal(AccumulatingRow["BudgetLastYear"]) +
                                                    Convert.ToDecimal(DetailRow["BudgetLastYear"]);
                AccumulatingRow["WholeYearBudget"] = Convert.ToDecimal(AccumulatingRow["WholeYearBudget"]) +
                                                     Convert.ToDecimal(DetailRow["WholeYearBudget"]);

                DetailRow["Actual"] = Convert.ToDecimal(AccumulatingRow["Actual"]);
                DetailRow["ActualYTD"] = Convert.ToDecimal(AccumulatingRow["ActualYTD"]);
                DetailRow["ActualLastYear"] = Convert.ToDecimal(AccumulatingRow["ActualLastYear"]);
                // DetailRow["ActualLastYearComplete"] = Convert.ToDecimal(AccumulatingRow["ActualLastYearComplete"]);
                DetailRow["Budget"] = Convert.ToDecimal(AccumulatingRow["Budget"]);
                DetailRow["BudgetYTD"] = Convert.ToDecimal(AccumulatingRow["BudgetYTD"]);
                DetailRow["BudgetLastYear"] = Convert.ToDecimal(AccumulatingRow["BudgetLastYear"]);
                DetailRow["WholeYearBudget"] = Convert.ToDecimal(AccumulatingRow["WholeYearBudget"]);
            }
        }

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// This version begins with GLM and GLMP tables, but calculates amounts for the summary accounts,
        /// so it does not rely on the summarisation in GLMP.
        /// </summary>
        [NoRemoting]
        public static DataTable IncomeExpenseTable(Dictionary<String, TVariant> AParameters, TReportingDbAdapter DbAdapter)
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
             *   ActualLastYear
             *   ActualLastYearComplete // not currently supported
             *   Budget
             *   BudgetYTD
             *   BudgetLastYear
             *   BudgetWholeYear
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
            Int32 AccountingYear = AParameters["param_year_i"].ToInt32();
            Int32 ReportPeriodStart = AParameters["param_start_period_i"].ToInt32();
            Int32 ReportPeriodEnd = AParameters["param_end_period_i"].ToInt32();
            Int32 PeriodMonths = 1 + (ReportPeriodEnd - ReportPeriodStart);
            String HierarchyName = AParameters["param_account_hierarchy_c"].ToString();

            //
            // Read different DB fields according to currency setting
            String ActualFieldName = AParameters["param_currency"].ToString().StartsWith("Int") ? "a_actual_intl_n" : "a_actual_base_n";
            String StartBalanceFieldName = AParameters["param_currency"].ToString().StartsWith("Int") ? "a_start_balance_intl_n" : "a_start_balance_base_n";
            String BudgetFieldName = AParameters["param_currency"].ToString().StartsWith("Int") ? "a_budget_intl_n" : "a_budget_base_n";
            Boolean CostCentreBreakdown = AParameters["param_cost_centre_breakdown"].ToBool();
            Boolean WholeYearPeriodsBreakdown = AParameters["param_period_breakdown"].ToBool();

            String CostCentreFilter = "";
            String CostCentreOptions = AParameters["param_costcentreoptions"].ToString();

            if (CostCentreOptions == "SelectedCostCentres")
            {
                String CostCentreList = AParameters["param_cost_centre_codes"].ToString();
                CostCentreList = CostCentreList.Replace(",", "','");                             // SQL IN List items in single quotes
                CostCentreFilter = " AND glm.a_cost_centre_code_c in ('" + CostCentreList + "')";
            }

            if (CostCentreOptions == "AllActiveCostCentres")
            {
                CostCentreFilter = " AND a_cost_centre.a_cost_centre_active_flag_l=true";
            }

            TDBTransaction ReadTrans = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            DataTable FilteredResults = null;
            try
            {
                // To find the Budget YTD, I need to sum all the budget fields from the start of the year.

                String BudgetQuery = (PeriodMonths == 1) ? " glmp." + BudgetFieldName +  // For one month, the Budget is read directly from the record;
                                     " AS Budget," :
                                     " (CASE WHEN glm.a_year_i=" + AccountingYear +      // for multiple months, I need to do a sum.
                                     " AND a_period_number_i=" + ReportPeriodEnd +
                                     " THEN (SELECT SUM(" + BudgetFieldName + ") FROM a_general_ledger_master_period" +
                                     " WHERE a_glm_sequence_i= glm.a_glm_sequence_i " +
                                     " AND a_period_number_i >= " + ReportPeriodStart +
                                     " AND a_period_number_i <= " + ReportPeriodEnd +
                                     " ) ELSE 0.0 END) AS Budget,";

                String BudgetYtdQuery = " (CASE WHEN glm.a_year_i=" + AccountingYear +
                                        " AND a_period_number_i=" + ReportPeriodEnd +
                                        " THEN (SELECT SUM(" + BudgetFieldName + ") FROM a_general_ledger_master_period" +
                                        " WHERE a_glm_sequence_i= glm.a_glm_sequence_i AND a_period_number_i <= " + ReportPeriodEnd +
                                        " ) ELSE 0.0 END) AS BudgetYTD,";

                String YearFilter =
                    " AND glm.a_year_i>=" + (AccountingYear - 1) +
                    " AND glm.a_year_i<=" + AccountingYear;

                String PeriodFilter =
                    " AND glmp.a_period_number_i>=" + (ReportPeriodStart - PeriodMonths) +
                    " AND glmp.a_period_number_i<=" + ReportPeriodEnd;

                String WholeYearBreakdownFields =
                    ", 0.0 AS P1, 0.0 AS P2, 0.0 AS P3, 0.0 AS P4, 0.0 AS P5, 0.0 AS P6 " +
                    ", 0.0 AS P7, 0.0 AS P8, 0.0 AS P9, 0.0 AS P10, 0.0 AS P11, 0.0 AS P12 ";

                if (WholeYearPeriodsBreakdown)
                {
                    CostCentreBreakdown = false; // Hopefully the client will have ensured this is false anyway - I'm just asserting it!
                    BudgetQuery = "0.0 AS Budget,";
                    BudgetYtdQuery = "0.0 AS BudgetYTD,";
                    YearFilter = " AND glm.a_year_i=" + AccountingYear;
                    PeriodFilter = " AND glmp.a_period_number_i<=12";
                    PeriodMonths = 12;
                }

                String Query = "SELECT DISTINCT" +
                               " 1 AS AccountLevel," +
                               " false AS HasChildren," +
                               " false AS ParentFooter," +
                               " false AS AccountIsSummary," +
                               " false AS Breakdown," +
                               " glm.a_glm_sequence_i AS Seq," +
                               " glm.a_year_i AS Year," +
                               " glmp.a_period_number_i AS Period," +
                               " glm.a_cost_centre_code_c AS CostCentreCode," +
                               " a_cost_centre.a_cost_centre_name_c AS CostCentreName," +
                               " a_account.a_account_type_c AS AccountType," +
                               " CASE a_account.a_account_type_c WHEN 'Income' THEN 1 WHEN 'Expense' THEN 2 END AS AccountTypeOrder," +
                               " a_account.a_debit_credit_indicator_l AS DebitCredit," +
                               " glm.a_account_code_c AS AccountCode," +
                               " 'Path' AS AccountPath," +
                               " a_account.a_account_code_short_desc_c AS AccountName," +
                               " glm." + StartBalanceFieldName + " AS YearStart," +
                               " 0.0 AS Actual," +
                               " glmp." + ActualFieldName + " AS ActualYTD," +
                               " 0.0 AS ActualLastYear," +
                               BudgetQuery +
                               BudgetYtdQuery +
                               " 0.0 AS BudgetLastYear," +
                               " 0.0 AS WholeYearBudget" +
                               WholeYearBreakdownFields +

                               " FROM a_general_ledger_master AS glm, a_general_ledger_master_period AS glmp, a_account, a_cost_centre" +
                               " WHERE glm.a_ledger_number_i=" + LedgerNumber +
                               YearFilter +
                               " AND glm.a_glm_sequence_i = glmp.a_glm_sequence_i" +
                               PeriodFilter +
                               " AND a_account.a_account_code_c = glm.a_account_code_c" +
                               " AND (a_account.a_account_type_c = 'Income' OR a_account.a_account_type_c = 'Expense')" +
                               " AND a_account.a_ledger_number_i = glm.a_ledger_number_i" +
                               " AND a_account.a_posting_status_l = true" +
                               " AND a_cost_centre.a_ledger_number_i = glm.a_ledger_number_i" +
                               " AND a_cost_centre.a_cost_centre_code_c = glm.a_cost_centre_code_c" +
                               CostCentreFilter;

                if (CostCentreBreakdown)
                {
                    Query += " ORDER BY glm.a_account_code_c, glm.a_cost_centre_code_c";
                }
                else
                {
                    Query += " ORDER BY glm.a_cost_centre_code_c, glm.a_account_code_c";
                }

                TLogging.Log(Catalog.GetString("Loading data.."), TLoggingType.ToStatusBar);
                DataTable resultTable = DbAdapter.RunQuery(Query, "IncomeExpense", ReadTrans);

                if (WholeYearPeriodsBreakdown)
                {
                    FilteredResults = resultTable;
                }
                else
                {
                    //
                    // The table includes YTD balances, but I need the balance for the specified period.
                    TLogging.Log(Catalog.GetString("Calculate period transactions.."), TLoggingType.ToStatusBar);

                    DataView OldPeriod = new DataView(resultTable);
                    DataView ThisMonth = new DataView(resultTable);
                    ThisMonth.RowFilter = "Period=" + ReportPeriodEnd;

                    //
                    // If I have rows for the previous month too, I can subtract the previous month's YTD balance to get my "Actual".
                    if (ReportPeriodEnd > PeriodMonths)
                    {
                        foreach (DataRowView rv in ThisMonth)
                        {
                            if (DbAdapter.Cancelled)
                            {
                                return FilteredResults;
                            }

                            DataRow Row = rv.Row;
                            OldPeriod.RowFilter = String.Format("Year={0} AND Period={1} AND CostCentreCode='{2}' AND AccountCode='{3}'",
                                Convert.ToInt32(Row["Year"]),
                                ReportPeriodEnd - PeriodMonths,
                                Row["CostCentreCode"].ToString(),
                                Row["AccountCode"].ToString()
                                );
                            DataRow PreviousPeriodRow = OldPeriod[0].Row;
                            Row["Actual"] = Convert.ToDecimal(Row["ActualYTD"]) - Convert.ToDecimal(PreviousPeriodRow["ActualYTD"]);
                        }
                    }
                    else
                    {
                        //
                        // For the first period of the year, I can just subtract the YearStart balance, which I already have just here...
                        foreach (DataRowView rv in ThisMonth)
                        {
                            if (DbAdapter.Cancelled)
                            {
                                return FilteredResults;
                            }

                            DataRow Row = rv.Row;
                            Row["Actual"] = Convert.ToDecimal(Row["ActualYTD"]) - Convert.ToDecimal(Row["YearStart"]);
                        }
                    }

                    //
                    // Some of these rows are from a year ago. I've updated their "Actual" values;
                    // now I'll copy those into the current period "LastYear" fields.
                    TLogging.Log(Catalog.GetString("Get Last year actuals.."), TLoggingType.ToStatusBar);

                    foreach (DataRowView rv in ThisMonth)
                    {
                        if (DbAdapter.Cancelled)
                        {
                            return FilteredResults;
                        }

                        DataRow Row = rv.Row;
                        OldPeriod.RowFilter = String.Format("Year={0} AND Period={1} AND CostCentreCode='{2}' AND AccountCode='{3}'",
                            AccountingYear - 1,
                            ReportPeriodEnd,
                            Row["CostCentreCode"].ToString(),
                            Row["AccountCode"].ToString()
                            );

                        if (OldPeriod.Count > 0)
                        {
                            DataRow LastYearRow = OldPeriod[0].Row;
                            Row["ActualLastYear"] = Convert.ToDecimal(LastYearRow["Actual"]);
                            Row["BudgetLastYear"] = Convert.ToDecimal(LastYearRow["Budget"]);
                        }
                    }

                    //
                    // So now I don't have to look at last year's rows or last month's rows:
                    ThisMonth.RowFilter = "Year=" + AccountingYear + " AND Period=" + ReportPeriodEnd;  // Only current period
                    FilteredResults = ThisMonth.ToTable("IncomeExpense");

                    //
                    // I need to add in the "whole year budget" field:
                    TLogging.Log(Catalog.GetString("Get Budgets.."), TLoggingType.ToStatusBar);

                    foreach (DataRow Row in FilteredResults.Rows)
                    {
                        if (DbAdapter.Cancelled)
                        {
                            return FilteredResults;
                        }

                        Query = "SELECT SUM(" + BudgetFieldName + ") AS WholeYearBudget FROM a_general_ledger_master_period WHERE a_glm_sequence_i=" +
                                Convert.ToInt32(Row["Seq"]);
                        DataTable YearBudgetTbl = DbAdapter.RunQuery(Query, "YearBudget", ReadTrans);

                        if (YearBudgetTbl.Rows.Count > 0)
                        {
                            Row["WholeYearBudget"] = YearBudgetTbl.Rows[0]["WholeYearBudget"];
                        }
                    }
                } // If not Whole Year Periods Breakdown

                //
                // I only have "posting accounts" - I need to add the summary accounts.
                TLogging.Log(Catalog.GetString("Summarise to parent accounts.."), TLoggingType.ToStatusBar);
                AAccountHierarchyDetailTable HierarchyTbl = AAccountHierarchyDetailAccess.LoadViaAAccountHierarchy(LedgerNumber, HierarchyName, ReadTrans);

                HierarchyTbl.DefaultView.Sort = "a_reporting_account_code_c";       // These two sort orders

                if (CostCentreBreakdown)                                            // Are required by AddTotalsToParentAccountRow, below.
                {
                    FilteredResults.DefaultView.Sort = "AccountCode";
                }
                else
                {
                    FilteredResults.DefaultView.Sort = "CostCentreCode, AccountCode";
                }

                Int32 PostingAccountRecords = FilteredResults.Rows.Count;
                Decimal PreviousActualYTD = 0;
                String ActualAccountCode = "";

                for (Int32 Idx = 0; Idx < PostingAccountRecords; Idx++)
                {
                    if (DbAdapter.Cancelled)
                    {
                        return FilteredResults;
                    }

                    DataRow Row = FilteredResults.Rows[Idx];

                    if (WholeYearPeriodsBreakdown)  // Because I missed out a chunk of code above, these rows have no "Actual" - only "ActualYTD".
                    {                               // I need to calculate those Actuals before I go further.
                        if (Row["AccountCode"].ToString() != ActualAccountCode)
                        {
                            ActualAccountCode = Row["AccountCode"].ToString();
                            PreviousActualYTD = 0;
                        }

                        Row["Actual"] = Convert.ToDecimal(Row["ActualYTD"]) - PreviousActualYTD;
                        PreviousActualYTD = Convert.ToDecimal(Row["ActualYTD"]);
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
                        WholeYearPeriodsBreakdown,
                        out ParentAccountPath,
                        out ParentAccountTypeOrder,
                        ReadTrans);
                    Row["AccountLevel"] = AccountLevel;
                    Row["AccountPath"] = ParentAccountPath + "~" + Row["AccountCode"];
                }

                //
                // Now if I re-order the result, and hide any rows that are empty or too detailed, it should be what I need!

                Int32 DetailLevel = AParameters["param_nesting_depth"].ToInt32();
                String DepthFilter = " AccountLevel<=" + DetailLevel.ToString();

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

                    DataRow AccumulatingRow = FilteredResults.NewRow();  // This temporary row is not part of the result set - it's just a line of temporary vars.

                    for (Int32 RowIdx = 0; RowIdx < FilteredResults.DefaultView.Count - 1; RowIdx++)
                    {
                        if (DbAdapter.Cancelled)
                        {
                            return FilteredResults;
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
                }
                else
                {
                    FilteredResults.DefaultView.Sort = "CostCentreCode, AccountTypeOrder, AccountPath ASC";
                }

                if (WholeYearPeriodsBreakdown)
                {
                    FilteredResults.DefaultView.RowFilter =
                        DepthFilter;      // Nothing too detailed
                }
                else
                {
                    FilteredResults.DefaultView.RowFilter =
                        "(Actual <> 0 OR ActualYTD <> 0 OR Budget <> 0 OR BudgetYTD <> 0)" + // Only non-zero rows
                        " AND " + DepthFilter;                                               // Nothing too detailed
                }

                FilteredResults = FilteredResults.DefaultView.ToTable("IncomeExpense");

                if (WholeYearPeriodsBreakdown)
                {
                    TLogging.Log(Catalog.GetString("Get whole year breakdown.."), TLoggingType.ToStatusBar);
                    //
                    // If there are any unsummarised rows left after applying the Depth Filter,
                    // I need to summarise them into new "per period" rows (with 12 "Actual" fields), and throw the original rows away.
                    FilteredResults.DefaultView.RowFilter = "AccountIsSummary=false";
                    DataView SummaryView = new DataView(FilteredResults);
                    SummaryView.Sort = "CostCentreCode, AccountTypeOrder, AccountPath ASC";
                    SummaryView.RowFilter = "Breakdown=true";

                    foreach (DataRowView rv in FilteredResults.DefaultView)
                    {
                        if (DbAdapter.Cancelled)
                        {
                            return FilteredResults;
                        }

                        DataRow Row = rv.Row;
                        AddToPeriodBreakdownSummary(SummaryView, Row);
                    }

                    FilteredResults.DefaultView.RowFilter = "(Breakdown=true OR AccountIsSummary=true) " + // Only the new rows with the calculated summaries
                                                            "AND (P1<>0 OR P2<>0 OR P3<>0 OR P4<>0 OR P5<>0 OR P6<>0 " +
                                                            "OR P7<>0 OR P8<>0 OR P9<>0 OR P10<>0 OR P11<>0 OR P12<>0) "; // No blank rows
                    FilteredResults.DefaultView.Sort = "CostCentreCode, AccountType DESC, AccountPath ASC";
                    FilteredResults = FilteredResults.DefaultView.ToTable("IncomeExpense");
                }

                //
                // Finally, to make the hierarchical report possible,
                // I want to include a note to show whether a row has child rows,
                // and if it does, I'll copy this row to a new row, below the children, marking the new row as "footer".
                TLogging.Log(Catalog.GetString("Format data for reporting.."), TLoggingType.ToStatusBar);
                for (Int32 RowIdx = 0; RowIdx < FilteredResults.Rows.Count - 1; RowIdx++)
                {
                    if (DbAdapter.Cancelled)
                    {
                        return FilteredResults;
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
                                && (Convert.ToBoolean(FilteredResults.Rows[ChildIdx]["Breakdown"]) == false))  // This row is not a child of mine
                            {                                                                                  // so I insert my footer before here.
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

                DBAccess.GDBAccessObj.RollbackTransaction();
                TLogging.Log("", TLoggingType.ToStatusBar);
            }
            catch (Exception ex) // if the report was cancelled, DB calls with the same transaction will raise exceptions.
            {
                TLogging.Log(ex.Message);
            }

            return FilteredResults;
        } // IncomeExpenseTable

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable HosaGiftsTable(Dictionary<String, TVariant> AParameters, TReportingDbAdapter DbAdapter)
        {
            Boolean NewTransaction = false;

            try
            {
                Boolean PersonalHosa = (AParameters["param_filter_cost_centres"].ToString() == "PersonalCostcentres");
                Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
                String CostCentreCodes = AParameters["param_cost_centre_codes"].ToString();
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
                    DateFilter = "AND GiftBatch.a_gl_effective_date_d >= " + dateStart.ToString("yyyy-MM-dd") +
                                 " AND GiftBatch.a_gl_effective_date_d <= " + dateEnd.ToString("yyyy-MM-dd") + " ";
                }

                String Query = "SELECT ";

                if (PersonalHosa)
                {
                    Query += "LinkedCostCentre.a_cost_centre_code_c AS CostCentre, ";
                }
                else
                {
                    Query += "GiftDetail.a_cost_centre_code_c AS CostCentre, ";
                }

                Query +=
                    "MotivationDetail.a_account_code_c AS AccountCode, SUM(GiftDetail.a_gift_amount_n) AS GiftBaseAmount, SUM(a_gift_transaction_amount_n) AS GiftTransactionAmount, "
                    +
                    "GiftDetail.p_recipient_key_n AS RecipientKey, Partner.p_partner_short_name_c AS RecipientShortname, " +
                    "Partner.p_partner_short_name_c AS Narrative " +
                    "FROM a_gift_detail AS GiftDetail, a_gift_batch AS GiftBatch, " +
                    "a_motivation_detail AS MotivationDetail, a_gift AS Gift, p_partner AS Partner";

                if (PersonalHosa)
                {
                    Query += ",PUB_a_valid_ledger_number AS LinkedCostCentre";
                }

                Query += " WHERE GiftDetail.a_ledger_number_i = GiftBatch.a_ledger_number_i " +
                         "AND GiftDetail.a_batch_number_i = GiftBatch.a_batch_number_i " +
                         "AND GiftDetail.a_ledger_number_i = MotivationDetail.a_ledger_number_i " +
                         "AND GiftDetail.a_motivation_group_code_c = MotivationDetail.a_motivation_group_code_c " +
                         "AND GiftDetail.a_motivation_detail_code_c = MotivationDetail.a_motivation_detail_code_c " +
                         "AND GiftDetail.a_ledger_number_i = Gift.a_ledger_number_i " +
                         "AND GiftDetail.a_batch_number_i = Gift.a_batch_number_i " +
                         "AND GiftDetail.a_gift_transaction_number_i = Gift.a_gift_transaction_number_i " +
                         "AND Partner.p_partner_key_n = GiftDetail.p_recipient_key_n " +
                         "AND GiftDetail.a_ledger_number_i = " + LedgerNumber + " " +
                         "AND GiftBatch.a_batch_status_c = '" + MFinanceConstants.BATCH_POSTED + "' " +
                         DateFilter;

                if (PersonalHosa)
                {
                    Query += "AND LinkedCostCentre.a_ledger_number_i = GiftDetail.a_ledger_number_i " +
                             "AND LinkedCostCentre.a_cost_centre_code_c IN (" + CostCentreCodes + ") " +
                             "AND GiftDetail.p_recipient_key_n = LinkedCostCentre.p_partner_key_n ";
                }
                else
                {
                    Query += "AND GiftDetail.a_cost_centre_code_c IN (" + CostCentreCodes + ") ";
                }

                if (IchNumber != 0)
                {
                    Query += "AND GiftDetail.a_ich_number_i = " + IchNumber + " ";
                }

                Query += "GROUP BY CostCentre, AccountCode, GiftDetail.p_recipient_key_n, Partner.p_partner_short_name_c " +
                         "ORDER BY Partner.p_partner_short_name_c ASC";

                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);
                TLogging.Log(Catalog.GetString("Loading data.."), TLoggingType.ToStatusBar);
                DataTable resultTable = DbAdapter.RunQuery(Query, "Gifts", Transaction);

                resultTable.Columns.Add("GiftIntlAmount", typeof(Decimal));
                resultTable.Columns.Add("Reference", typeof(string));

                Boolean InternationalCurrency = AParameters["param_currency"].ToString().ToLower().StartsWith("int");
                Double ExchangeRate = 1.00;  // TODO Get exchange rate!

                foreach (DataRow r in resultTable.Rows)
                {
                    if (DbAdapter.Cancelled)
                    {
                        return resultTable;
                    }

                    if (InternationalCurrency)
                    {
                        r["GiftIntlAmount"] = (Decimal)(Convert.ToDouble(r["GiftBaseAmount"]) * ExchangeRate);
                    }

                    r["Reference"] = StringHelper.PartnerKeyToStr(Convert.ToInt64(r["RecipientKey"]));
                }

                TLogging.Log("", TLoggingType.ToStatusBar);
                return resultTable;
            }   // try
            catch (Exception e)
            {
                TLogging.Log("Problem gift rows for HOSA report: " + e.ToString());
                return null;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }
    }
}