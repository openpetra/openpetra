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
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Windows.Forms;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Client;
using System.Collections;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared;
using System.Data;
using Ict.Petra.Client.App.Core;
using System.IO;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmAccountDetail
    {
        private Int32 FLedgerNumber;

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
                uco_GeneralSettings.ShowOnlyPeriodSelection();
                pnlSorting.Padding = new System.Windows.Forms.Padding(8); // This tweak bring controls inline.
                FPetraUtilsObject.LoadDefaultSettings();
                rbtSortByCostCentre.CheckedChanged += rbtSortByCostCentre_CheckedChanged;

                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            }
        }

        void rbtSortByCostCentre_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbtSortByCostCentre.Checked)
            {
                chkPaginate.Checked = false;
                chkAutoEmail.Checked = false;
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_with_analysis_attributes", false);

            // if rbtSortByCostCentre is checked then these parameters are added in generated code
            if (!rbtSortByCostCentre.Checked)
            {
                ACalc.AddParameter("param_paginate", false);
                ACalc.AddParameter("param_auto_email", false);
            }
        }

        //
        // This will be called if the Fast Reports Wrapper loaded OK.
        // Returns True if the data apparently loaded OK and the report should be printed.
        // (Returns FALSE if the user has selected "auto-email" and this method drives the email sending process itself, leaving nothing for the default process to do.)
        private bool LoadReportData(TRptCalculator ACalc)
        {
            Shared.MReporting.TParameterList parameters = ACalc.GetParameters();
            String LedgerFilter = "a_ledger_number_i=" + parameters.Get("param_ledger_number_i").ToInt32();

            String AccountCodeFilter = ""; // Account Filter, as range or list:
            String TransAccountCodeFilter = "";
            String GlmAccountCodeFilter = "";
            DataTable Balances = new DataTable();

            ACalc.AddStringParameter("param_linked_partner_cc", ""); // I may want to use this later, for auto_email, but usually it's unused.

            if (parameters.Get("param_rgrAccounts").ToString() == "AccountList")
            {
                String Filter = "'" + parameters.Get("param_account_codes") + "'";
                Filter = Filter.Replace(",", "','");
                AccountCodeFilter = "AND a_account_code_c in (" + Filter + ")";
                TransAccountCodeFilter = "AND a_transaction.a_account_code_c in (" + Filter + ")";
                GlmAccountCodeFilter = " AND glm.a_account_code_c in (" + Filter + ")";
            }

            if (parameters.Get("param_rgrAccounts").ToString() == "AccountRange")
            {
                AccountCodeFilter = "AND a_account_code_c BETWEEN '" + parameters.Get("param_account_code_start") + "' AND '" +
                                    parameters.Get("param_account_code_end") + "'";
                TransAccountCodeFilter = "AND a_transaction.a_account_code_c BETWEEN '" + parameters.Get("param_account_code_start") + "' AND '" +
                                         parameters.Get("param_account_code_end") + "'";
                GlmAccountCodeFilter = " AND glm.a_account_code_c BETWEEN '" + parameters.Get("param_account_code_start") + "' AND '" +
                                       parameters.Get("param_account_code_end") + "'";
            }

            String CostCentreFilter = ""; // Cost Centre Filter, as range or list:
            String TransCostCentreFilter = "";
            String GlmCostCentreFilter = "";

            if (parameters.Get("param_rgrCostCentres").ToString() == "CostCentreList")
            {
                String Filter = "'" + parameters.Get("param_cost_centre_codes") + "'";
                Filter = Filter.Replace(",", "','");
                CostCentreFilter = " AND a_cost_centre_code_c in (" + Filter + ")";
                TransCostCentreFilter = " AND a_transaction.a_cost_centre_code_c in (" + Filter + ")";
                GlmCostCentreFilter = " AND glm.a_cost_centre_code_c in (" + Filter + ")";
            }

            if (parameters.Get("param_rgrCostCentres").ToString() == "CostCentreRange")
            {
                CostCentreFilter = " AND a_cost_centre_code_c BETWEEN '" + parameters.Get("param_cost_centre_code_start") +
                                   "' AND  '" + parameters.Get("param_cost_centre_code_end") + "'";
                TransCostCentreFilter = " AND a_transaction.a_cost_centre_code_c BETWEEN '" + parameters.Get("param_cost_centre_code_start") +
                                        "' AND  '" + parameters.Get("param_cost_centre_code_end") + "'";
                GlmCostCentreFilter = " AND glm.a_cost_centre_code_c BETWEEN '" + parameters.Get("param_cost_centre_code_start") +
                                      "' AND '" + parameters.Get("param_cost_centre_code_end") + "'";
            }

            String TranctDateFilter = "a_transaction_date_d BETWEEN '" + parameters.Get("param_start_date").DateToString("yyyy-MM-dd") +
                                      "' AND '" + parameters.Get("param_end_date").DateToString("yyyy-MM-dd") + "'";

            String ReferenceFilter = "";
            String AnalysisTypeFilter = " ";
            String OrderByClause = "AccountCode, a_cost_centre_code_c, TransactionDate";
            String Sortby = parameters.Get("param_sortby").ToString();

            if (Sortby == "Cost Centre")
            {
                OrderByClause = "a_cost_centre_code_c, AccountCode, TransactionDate";
            }

            if (Sortby == "Reference")
            {
                OrderByClause = "a_reference_c, a_cost_centre_code_c, AccountCode, TransactionDate";
                String FilterItem = parameters.Get("param_reference_start").ToString();

                if (FilterItem != "")
                {
                    ReferenceFilter = " AND a_reference_c >='" + FilterItem + "'";
                }

                FilterItem = parameters.Get("param_reference_end").ToString();

                if (FilterItem != "")
                {
                    ReferenceFilter += " AND a_reference_c <='" + FilterItem + "'";
                }
            }

            if (Sortby == "Analysis Type")
            {
                OrderByClause = "AnalysisTypeCode, AnalysisValue, TransactionDate";
                String FilterItem = parameters.Get("param_analyis_type_start").ToString();

                if (FilterItem != "")
                {
                    AnalysisTypeFilter = " AND a_analysis_type.a_analysis_type_code_c >='" + FilterItem + "' ";
                }

                FilterItem = parameters.Get("param_analyis_type_end").ToString();

                if (FilterItem != "")
                {
                    AnalysisTypeFilter += " AND a_analysis_type.a_analysis_type_code_c <='" + FilterItem + "' ";
                }
            }

            parameters.Add("param_groupfield", OrderByClause);

            String Csv = "";
            Csv = StringHelper.AddCSV(Csv, "ALedger/SELECT * FROM a_ledger WHERE " + LedgerFilter);
            Csv = StringHelper.AddCSV(
                Csv,
                "AAccount/SELECT * FROM a_account WHERE " + LedgerFilter + AccountCodeFilter +
                " AND a_posting_status_l=true"
//                + " AND a_account_active_flag_l=true"
                );
            Csv = StringHelper.AddCSV(
                Csv,
                "ACostCentre/SELECT * FROM a_cost_centre WHERE " + LedgerFilter + CostCentreFilter +
                " AND a_posting_cost_centre_flag_l=true"
//                + " AND a_cost_centre_active_flag_l=true"
                );

            String CurrencySelected = parameters.Get("param_currency").ToString();

            //
            // FX Reval transactions should not be shown if "Transaction" currency was selected,
            // since these transactions only take place in Base currency; they're 0 in the account currency.
            String RevalFilter = "";

            if (CurrencySelected == "Transaction")
            {
                RevalFilter =
                    " AND ((a_account.a_foreign_currency_flag_l=FALSE) OR (a_account.a_foreign_currency_code_c = a_journal.a_transaction_currency_c))";
            }

            String AmountField = CurrencySelected.StartsWith("Int") ? "a_amount_in_intl_currency_n" :
                                 CurrencySelected.StartsWith("Trans") ? "a_transaction_amount_n" : "a_amount_in_base_currency_n";

            String CurrencyField = CurrencySelected.StartsWith("Int") ? "a_ledger.a_intl_currency_c" :
                                   CurrencySelected.StartsWith("Trans") ? "a_journal.a_transaction_currency_c" : "a_ledger.a_base_currency_c";

            if (Sortby == "Analysis Type")  // To sort by analysis type, I need a different (and more horible) query:
            {
                Csv = StringHelper.AddCSV(
                    Csv,
                    "Transactions/" +
                    "SELECT a_transaction.a_account_code_c AS AccountCode," +
                    "a_transaction.a_cost_centre_code_c AS CostCentreCode," +
                    "a_transaction.a_transaction_date_d AS TransactionDate," +
                    "a_transaction." + AmountField + " AS Amount," +
                    CurrencyField + " AS Currency," +
                    "a_transaction.a_debit_credit_indicator_l AS Debit," +
                    "a_transaction.a_narrative_c AS Narrative," +
                    "a_transaction.a_reference_c AS Reference," +
                    "a_trans_anal_attrib.a_analysis_type_code_c AS AnalysisTypeCode," +
                    "a_analysis_type.a_analysis_type_description_c AS AnalysisTypeDescr," +
                    "a_trans_anal_attrib.a_analysis_attribute_value_c AS AnalysisValue" +
                    " FROM a_transaction, a_journal, a_trans_anal_attrib, a_analysis_type, a_ledger, a_account" +
                    " WHERE a_transaction." + LedgerFilter +
                    " AND a_transaction.a_ledger_number_i = a_account.a_ledger_number_i " +
                    " AND a_transaction.a_account_code_c = a_account.a_account_code_c " +
                    " AND a_transaction.a_ledger_number_i = a_ledger.a_ledger_number_i " +
                    " AND a_transaction.a_ledger_number_i = a_journal.a_ledger_number_i " +
                    " AND a_transaction.a_batch_number_i = a_journal.a_batch_number_i " +
                    " AND a_transaction.a_journal_number_i = a_journal.a_journal_number_i " +
                    RevalFilter +
                    " AND a_trans_anal_attrib.a_ledger_number_i = a_transaction.a_ledger_number_i " +
                    " AND a_trans_anal_attrib.a_batch_number_i = a_transaction.a_batch_number_i" +
                    " AND a_trans_anal_attrib.a_journal_number_i = a_transaction.a_journal_number_i" +
                    " AND a_trans_anal_attrib.a_transaction_number_i = a_transaction.a_transaction_number_i" +
                    " AND a_trans_anal_attrib.a_analysis_type_code_c = a_analysis_type.a_analysis_type_code_c " +
                    AnalysisTypeFilter +
                    TransAccountCodeFilter + TransCostCentreFilter + " AND " + TranctDateFilter +
                    " AND a_transaction_status_l=true AND NOT (a_system_generated_l=true AND a_narrative_c LIKE 'Year end re-allocation%')" +
                    " ORDER BY " + OrderByClause + ", a_transaction_date_d");
            }
            else
            {
                Csv = StringHelper.AddCSV(
                    Csv,
                    "Transactions/" +
                    "SELECT a_transaction.a_account_code_c AS AccountCode," +
                    "a_transaction.a_cost_centre_code_c AS CostCentreCode," +
                    "a_transaction.a_transaction_date_d AS TransactionDate," +
                    "a_transaction." + AmountField + " AS Amount," +
                    CurrencyField + " AS Currency," +
                    "a_transaction.a_debit_credit_indicator_l AS Debit," +
                    "a_transaction.a_narrative_c AS Narrative," +
                    "a_transaction.a_reference_c AS Reference," +
                    "'' AS AnalysisTypeCode," +
                    "'' AS AnalysisTypeDescr," +
                    "'' AS AnalysisValue" +
                    " FROM a_transaction, a_journal, a_ledger, a_account WHERE " +
                    " a_transaction." + LedgerFilter +
                    " AND a_transaction.a_ledger_number_i = a_account.a_ledger_number_i " +
                    " AND a_transaction.a_account_code_c = a_account.a_account_code_c " +
                    " AND a_transaction.a_ledger_number_i = a_ledger.a_ledger_number_i " +
                    " AND a_transaction.a_ledger_number_i = a_journal.a_ledger_number_i " +
                    " AND a_transaction.a_batch_number_i = a_journal.a_batch_number_i " +
                    " AND a_transaction.a_journal_number_i = a_journal.a_journal_number_i " +
                    RevalFilter +
                    TransAccountCodeFilter +
                    TransCostCentreFilter + " AND " +
                    TranctDateFilter + ReferenceFilter +
                    " AND a_transaction_status_l=true AND NOT (a_system_generated_l=true AND a_narrative_c LIKE 'Year end re-allocation%')" +
                    " ORDER BY " + OrderByClause);
            }

            GLReportingTDS ReportDs = TRemote.MReporting.WebConnectors.GetReportingDataSet(Csv);

            if ((this.IsDisposed) // If the user has pressed Esc the form is closed!
                || (TRemote.MReporting.WebConnectors.DataTableGenerationWasCancelled()))
            {
                return false;
            }

            //
            // I want to include opening and closing balances for each Cost Centre / Account, in the selected currency.
            // Following a revision in Oct 2014, this table is the master table, and the transactions are the slave.

            Int32 Year = parameters.Get("param_year_i").ToInt32();
            Balances = TRemote.MFinance.Reporting.WebConnectors.GetPeriodBalances(
                LedgerFilter,
                GlmAccountCodeFilter,
                GlmCostCentreFilter,
                Year,
                Sortby,
                ReportDs.Tables["Transactions"],
                parameters.Get("param_start_period_i").ToInt32(),
                parameters.Get("param_end_period_i").ToInt32(),
                CurrencySelected
                );

            if ((this.IsDisposed) || (Balances == null))
            {
                return false;
            }

            // My report doesn't need a ledger row - only the name of the ledger. And I need the currency formatter..
            DataTable LedgerNameTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList);
            DataView LedgerView = new DataView(LedgerNameTable);
            LedgerView.RowFilter = "LedgerNumber=" + FLedgerNumber;
            String LedgerName = "";

            if (LedgerView.Count > 0)
            {
                LedgerName = LedgerView[0].Row["LedgerName"].ToString();
            }

            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");
            ACalc.AddStringParameter("param_base_currency_name", uco_GeneralSettings.GetBaseCurrency());

            if (TRemote.MReporting.WebConnectors.DataTableGenerationWasCancelled())
            {
                return false;
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
                    "ATransAnalAttrib/SELECT * FROM a_trans_anal_attrib WHERE " + LedgerFilter +
                    " AND a_batch_number_i >= " + FirstBatch +
                    " AND a_batch_number_i <= " + LastBatch);
                Csv = StringHelper.AddCSV(Csv, "AAnalysisType/SELECT * FROM a_analysis_type");
                ReportDs.Merge(TRemote.MReporting.WebConnectors.GetReportingDataSet(Csv));
            }

            if (TRemote.MReporting.WebConnectors.DataTableGenerationWasCancelled())
            {
                return false;
            }

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDs.ATransAnalAttrib, "a_trans_anal_attrib");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDs.AAnalysisType, "a_analysis_type");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDs.AAccount, "a_account");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDs.ACostCentre, "a_costCentre");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportDs.Tables["Transactions"], "Transactions");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(Balances, "balances");

            //
            // For Account Detail reports that must be sent on email, one page at a time,
            // I'm calling the FastReports plugin multiple times,
            // and then I'm going to return false, which will prevent the default action using this dataset.

            if ((parameters.Get("param_sortby").ToString() == "Cost Centre")
                && (parameters.Get("param_auto_email").ToBool())
                && !parameters.Get("param_design_template").ToBool()
                )
            {
                String Status = FastReportsWrapper.AutoEmailReports(FPetraUtilsObject,
                    FPetraUtilsObject.FFastReportsPlugin,
                    ACalc,
                    FLedgerNumber,
                    CostCentreFilter);
                MessageBox.Show(Status, Catalog.GetString("Account Detail Report"));
                return false;
            }

            return true;
        }
    }
}