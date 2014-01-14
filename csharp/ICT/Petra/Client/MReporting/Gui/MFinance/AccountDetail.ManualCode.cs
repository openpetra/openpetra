//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//
// Copyright 2004-2010 by OM International
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
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Windows.Forms;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Common;
using Ict.Common.Remoting.Client;
using System.Collections;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared;
using System.Data;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmAccountDetail
    {
        private Int32 FLedgerNumber;
        FastReportsWrapper FFastReportsPlugin;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                uco_GeneralSettings.EnableDateSelection(true);

                FLedgerNumber = value;

                uco_AccountCostCentreSettings.InitialiseLedger(FLedgerNumber);
                uco_GeneralSettings.InitialiseLedger(FLedgerNumber);
                pnlSorting.Padding = new System.Windows.Forms.Padding(8); // This tweak bring controls inline.
                FPetraUtilsObject.LoadDefaultSettings();
                FFastReportsPlugin = new FastReportsWrapper(FPetraUtilsObject, LoadReportData);
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_with_analysis_attributes", false);
        }

        private bool LoadReportData(TRptCalculator ACalc)
        {
            Shared.MReporting.TParameterList pm = ACalc.GetParameters();
            String LedgerFilter = "a_ledger_number_i=" + pm.Get("param_ledger_number_i").ToInt32();

            String AccountCodeFilter = ""; // Account Filter, as range or list:
            String TranctAccountCodeFilter = "";

            if (pm.Get("param_rgrAccounts").ToString() == "AccountList")
            {
                String Filter = "'" + pm.Get("param_account_codes") + "'";
                Filter = Filter.Replace(",", "','");
                AccountCodeFilter = " AND a_account_code_c in (" + Filter + ")";
                TranctAccountCodeFilter = " AND a_transaction.a_account_code_c in (" + Filter + ")";
            }

            if (pm.Get("param_rgrAccounts").ToString() == "AccountRange")
            {
                AccountCodeFilter = " AND a_account_code_c>='" + pm.Get("param_account_code_start") + "' AND a_account_code_c<='" + pm.Get(
                    "param_account_code_end") + "'";
                TranctAccountCodeFilter = " AND a_transaction.a_account_code_c>='" + pm.Get("param_account_code_start") +
                                          "' AND a_transaction.a_account_code_c<='" + pm.Get("param_account_code_end") + "'";
            }

            String CostCentreFilter = ""; // Cost Centre Filter, as range or list:
            String TranctCostCentreFilter = "";

            if (pm.Get("param_rgrCostCentres").ToString() == "CostCentreList")
            {
                String Filter = "'" + pm.Get("param_cost_centre_codes") + "'";
                Filter = Filter.Replace(",", "','");
                CostCentreFilter = " AND a_cost_centre_code_c in (" + Filter + ")";
                TranctCostCentreFilter = " AND a_transaction.a_cost_centre_code_c in (" + Filter + ")";
            }

            if (pm.Get("param_rgrCostCentres").ToString() == "CostCentreRange")
            {
                CostCentreFilter = " AND a_cost_centre_code_c>='" + pm.Get("param_cost_centre_code_start") + "' AND a_cost_centre_code_c<='" + pm.Get(
                    "param_cost_centre_code_end") + "'";
                TranctCostCentreFilter = " AND a_transaction.a_cost_centre_code_c>='" + pm.Get("param_cost_centre_code_start") +
                                         "' AND a_transaction.a_cost_centre_code_c<='" + pm.Get("param_cost_centre_code_end") + "'";
            }

            String TranctDateFilter = ""; // Optional Date Filter, as periods or dates

            if (pm.Get("param_period").ToBool() == true)
            {
                DataTable AccountingPeriodTbl =
                    (AAccountingPeriodTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountingPeriodList,
                        pm.Get("param_ledger_number_i").ToInt32());
                AccountingPeriodTbl.DefaultView.RowFilter = LedgerFilter + " AND a_accounting_period_number_i=" +
                                                            pm.Get("param_start_period_i").ToInt32();
                pm.Add("param_start_date", Convert.ToDateTime(AccountingPeriodTbl.DefaultView[0].Row["a_period_start_date_d"]));

                AccountingPeriodTbl.DefaultView.RowFilter = LedgerFilter + " AND a_accounting_period_number_i=" +
                                                            pm.Get("param_end_period_i").ToInt32();
                pm.Add("param_end_date", Convert.ToDateTime(AccountingPeriodTbl.DefaultView[0].Row["a_period_end_date_d"]));
            }

            TranctDateFilter = "a_transaction_date_d>='" + pm.Get("param_start_date").DateToString("yyyy-MM-dd") +
                               "' AND a_transaction_date_d<='" + pm.Get("param_end_date").DateToString("yyyy-MM-dd") + "'";

            String ReferenceFilter = "";
            String AnalysisTypeFilter = "";
            String GroupField = "a_account_code_c";

            if (pm.Get("param_sortby").ToString() == "Cost Centre")
            {
                GroupField = "a_cost_centre_code_c";
            }

            if (pm.Get("param_sortby").ToString() == "Reference")
            {
                GroupField = "a_reference_c";
                String FilterItem = pm.Get("param_reference_start").ToString();

                if (FilterItem != "")
                {
                    ReferenceFilter = " AND a_reference_c >='" + FilterItem + "'";
                }

                FilterItem = pm.Get("param_reference_end").ToString();

                if (FilterItem != "")
                {
                    ReferenceFilter += " AND a_reference_c <='" + FilterItem + "'";
                }
            }

            if (pm.Get("param_sortby").ToString() == "Analysis Type")
            {
                GroupField = "a_analysis_type_code_c";
                String FilterItem = pm.Get("param_analyis_type_start").ToString();

                if (FilterItem != "")
                {
                    AnalysisTypeFilter = " AND a_trans_anal_attrib.a_analysis_type_code_c >='" + FilterItem + "'";
                }

                FilterItem = pm.Get("param_analyis_type_end").ToString();

                if (FilterItem != "")
                {
                    AnalysisTypeFilter += " AND a_trans_anal_attrib.a_analysis_type_code_c <='" + FilterItem + "'";
                }
            }

            String OrderBy = " ORDER BY " + GroupField + ", a_transaction_date_d";
            pm.Add("param_groupfield", GroupField);

            String Csv = "";
            Csv = StringHelper.AddCSV(Csv, "ALedger/*/a_ledger/" + LedgerFilter);
            Csv = StringHelper.AddCSV(Csv,
                "AAccount/*/a_account/" + LedgerFilter + AccountCodeFilter + "AND a_posting_status_l=true AND a_account_active_flag_l=true");
            Csv = StringHelper.AddCSV(
                Csv,
                "ACostCentre/*/a_cost_centre/" + LedgerFilter + CostCentreFilter +
                " AND a_posting_cost_centre_flag_l=true AND a_cost_centre_active_flag_l=true");

            if (pm.Get("param_sortby").ToString() == "Analysis Type")  // To sort by analysis type, I need a different (and more horible) query:
            {
                Csv = StringHelper.AddCSV(
                    Csv,
                    "ATransaction/" +
                    "a_transaction.*,a_trans_anal_attrib.a_analysis_type_code_c,a_analysis_type.a_analysis_type_description_c,a_trans_anal_attrib.a_analysis_attribute_value_c"
                    +
                    "/a_transaction, a_trans_anal_attrib, a_analysis_type/" +
                    "a_transaction." + LedgerFilter +
                    " AND a_trans_anal_attrib.a_ledger_number_i = a_transaction.a_ledger_number_i " +
                    " AND a_trans_anal_attrib.a_batch_number_i = a_transaction.a_batch_number_i" +
                    " AND a_trans_anal_attrib.a_journal_number_i = a_transaction.a_journal_number_i" +
                    " AND a_trans_anal_attrib.a_transaction_number_i = a_transaction.a_transaction_number_i" +
                    " AND a_trans_anal_attrib.a_analysis_type_code_c = a_analysis_type.a_analysis_type_code_c" +
                    AnalysisTypeFilter +
                    TranctAccountCodeFilter + TranctCostCentreFilter + " AND " + TranctDateFilter +
                    " AND a_transaction_status_l=true AND NOT (a_system_generated_l=true AND a_narrative_c LIKE 'Year end re-allocation%')" +
                    "/" + OrderBy);
            }
            else
            {
                Csv = StringHelper.AddCSV(Csv, "ATransaction/*/a_transaction/" + LedgerFilter + TranctAccountCodeFilter +
                    TranctCostCentreFilter + " AND " + TranctDateFilter + ReferenceFilter +
                    " AND a_transaction_status_l=true AND NOT (a_system_generated_l=true AND a_narrative_c LIKE 'Year end re-allocation%')" +
                    "/" + OrderBy);
            }

            GLReportingTDS ReportDs = TRemote.MFinance.Reporting.WebConnectors.GetReportingDataSet(Csv);

            //
            // If I'm reporting period, 
            // I want to include opening and closing balances for each Cost Centre / Account, in the selected currency:
            if (pm.Get("param_period").ToBool() == true)
            {
                DataTable Balances = TRemote.MFinance.Reporting.WebConnectors.GetPeriodBalances(
                    LedgerFilter,
                    AccountCodeFilter,
                    CostCentreFilter,
                    pm.Get("param_start_period_i").ToInt32(),
                    pm.Get("param_end_period_i").ToInt32(),
                    pm.Get("param_currency").ToString() == "International"
                    );
                ReportDs.Merge(Balances);
                FFastReportsPlugin.RegisterData(Balances, "balances");
            }


            // My report doesn't need a ledger row - only the name of the ledger. And I need the currency formatter..
            {
                ALedgerRow Row = ReportDs.ALedger[0];
                ACalc.AddStringParameter("param_ledger_name", Row.LedgerName);
                ACalc.AddStringParameter("param_currency_formatter", "0,0.000");
            }

            //
            // If I need to show Analysis Attributes, I need to get the rows that pertain to the Transactions I've selected above.
            if (ReportDs.ATransaction.Rows.Count > 0)
            {
                DataView BatchSorted = new DataView(ReportDs.ATransaction);
                BatchSorted.Sort = "a_batch_number_i";
                ATransactionRow Row = (ATransactionRow)BatchSorted[0].Row;
                Int32 FirstBatch = Row.BatchNumber;
                Row = (ATransactionRow)BatchSorted[BatchSorted.Count - 1].Row;
                Int32 LastBatch = Row.BatchNumber;

                Csv = "";
                Csv = StringHelper.AddCSV(
                    Csv,
                    "ATransAnalAttrib/*/a_trans_anal_attrib/" + LedgerFilter + " AND a_batch_number_i >= " + FirstBatch +
                    " AND a_batch_number_i <= " +
                    LastBatch);
                Csv = StringHelper.AddCSV(Csv, "AAnalysisType/*/a_analysis_type/1=1");
                ReportDs.Merge(TRemote.MFinance.Reporting.WebConnectors.GetReportingDataSet(Csv));
            }

            FFastReportsPlugin.RegisterData(ReportDs.ATransAnalAttrib, "a_trans_anal_attrib");
            FFastReportsPlugin.RegisterData(ReportDs.AAnalysisType, "a_analysis_type");
            FFastReportsPlugin.RegisterData(ReportDs.AAccount, "a_account");
            FFastReportsPlugin.RegisterData(ReportDs.ACostCentre, "a_costCentre");
            FFastReportsPlugin.RegisterData(ReportDs.ATransaction, "a_transaction");
            return true;
        }
    }
}