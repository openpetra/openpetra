//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr, timop
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.App.Core.RemoteObjects;
using SourceGrid.Selection;
using System.Data;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    /// <summary>
    /// Description of TFrmUC_CostCentreSettings.
    /// </summary>
    public partial class TFrmUC_AccountCostCentreSettings
    {
        /// <summary>Indicator if the report is generated with balance or not</summary>
        public bool FReportWithBalance;
        private int FLedgerNumber;
        private String FCostCenterCodesDuringLoad;
        private String FAccountCodesDuringLoad;

        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FLedgerNumber = -1;
            FReportWithBalance = true;
            this.rbtAllAccounts.CheckedChanged += new System.EventHandler(this.LateInitialise);
            this.rbtAccountRange.CheckedChanged += new System.EventHandler(this.LateInitialise);
            this.rbtAccountFromList.CheckedChanged += new System.EventHandler(this.LateInitialise);
            this.rbtAllCostCentres.CheckedChanged += new System.EventHandler(this.LateInitialise);
            this.rbtCostCentreRange.CheckedChanged += new System.EventHandler(this.LateInitialise);
            this.rbtCostCentreFromList.CheckedChanged += new System.EventHandler(this.LateInitialise);

            rbtAllAccounts.Checked = true;
            rbtAllCostCentres.Checked = true;

/*
 *          rbtAccountRange.Checked = true;
 *          rbtAccountFromListCheckedChanged(null, null);
 *          rbtCostCentreRange.Checked = true;
 *          rbtCostCentreFromListCheckedChanged(null, null);
 */
        }

        /// <summary>
        /// initialise the controls with the values from the ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void InitialiseLedger(int ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;
        }

        void LateInitialise(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                if ((sender == rbtAccountRange) && (cmbFromAccountCode.Count == 0))
                {
                    TFinanceControls.InitialiseAccountList(ref cmbFromAccountCode, FLedgerNumber, true, false, false, false);
                    TFinanceControls.InitialiseAccountList(ref cmbToAccountCode, FLedgerNumber, true, false, false, false);
                    cmbFromAccountCode.SelectedIndex = 1;
                    cmbToAccountCode.SelectedIndex = cmbToAccountCode.Count - 1;
                }

                if ((sender == rbtAccountFromList) && (clbAccountCodes.Rows.Count == 1))
                {
                    TFinanceControls.InitialiseAccountList(ref clbAccountCodes, FLedgerNumber, true, false, false, false);
                    clbAccountCodes.SetCheckedStringList("");
                }

                if ((sender == rbtCostCentreRange) && (cmbFromCostCentre.Count == 0))
                {
                    TFinanceControls.InitialiseCostCentreList(ref cmbFromCostCentre, FLedgerNumber, true, false, false, false);
                    TFinanceControls.InitialiseCostCentreList(ref cmbToCostCentre, FLedgerNumber, true, false, false, false);
                    cmbFromCostCentre.SelectedIndex = 1;
                    cmbToCostCentre.SelectedIndex = cmbToCostCentre.Count - 1;
                }

                if ((sender == rbtCostCentreFromList) && (clbCostCentres.Rows.Count == 1))
                {
                    TFinanceControls.InitialiseCostCentreList(ref cmbSummaryCostCentres, FLedgerNumber, false, true, false, false);
                    TFinanceControls.InitialiseCostCentreList(ref clbCostCentres, FLedgerNumber, true, false, false, false);
                    clbCostCentres.SetCheckedStringList("");
                }
            }
        }

        #region Parameter/Settings Handling

        /// <summary>
        /// Sets the available functions (fields) that can be used for this report.
        /// </summary>
        /// <param name="AAvailableFunctions">List of TColumnFunction</param>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
        }

        /// <summary>
        /// Reads the selected values from the controls,
        /// and stores them into the parameter system of FCalculator
        ///
        /// </summary>
        /// <param name="ACalculator"></param>
        /// <param name="AReportAction"></param>
        /// <returns>void</returns>
        public void ReadControls(TRptCalculator ACalculator, TReportActionEnum AReportAction)
        {
            TVerificationResult VerificationResult;

            if (rbtAllAccounts.Checked)
            {
                ACalculator.AddParameter("param_rgrAccounts", "AllAccounts");
                ACalculator.AddParameter("param_account_list_title", "All Accounts");
            }
            else if (rbtAccountFromList.Checked)
            {
                String SelectedAccountCodes = clbAccountCodes.GetCheckedStringList();

                if (SelectedAccountCodes.Length == 0)
                {
                    VerificationResult = new TVerificationResult(Catalog.GetString("Select Account Codes"),
                        Catalog.GetString("No Account Code was selected!"),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                ACalculator.AddStringParameter("param_account_codes", SelectedAccountCodes);

                if (SelectedAccountCodes.Length > 25)
                {
                    SelectedAccountCodes = "Selected Accounts";
                }

                // need to set NOTUSED,
                // otherwise the report generator complains about the missing parameter
                // *NOTUSED* is used as an invalid value, there is no account with this name
                ACalculator.AddParameter("param_account_list_title", SelectedAccountCodes);
                ACalculator.AddParameter("param_account_code_start", "*NOTUSED*");
                ACalculator.AddParameter("param_account_code_end", "*NOTUSED*");
                ACalculator.AddParameter("param_rgrAccounts", "AccountList");
            }
            else
            {
                ACalculator.AddParameter("param_account_list_title",
                    cmbFromAccountCode.GetSelectedString() + " To " + cmbToAccountCode.GetSelectedString());
                ACalculator.AddParameter("param_account_codes", "*NOTUSED*");
                ACalculator.AddParameter("param_account_code_start", cmbFromAccountCode.GetSelectedString());
                ACalculator.AddParameter("param_account_code_end", cmbToAccountCode.GetSelectedString());
                ACalculator.AddParameter("param_rgrAccounts", "AccountRange");

                VerificationResult = TStringChecks.FirstLesserOrEqualThanSecondString(
                    cmbFromAccountCode.GetSelectedString(),
                    cmbToAccountCode.GetSelectedString(),
                    Catalog.GetString("Account Range From"),
                    Catalog.GetString("Account Range To"));

                if (VerificationResult != null)
                {
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
            }

            if (rbtAllCostCentres.Checked)
            {
                ACalculator.AddParameter("param_cost_centre_list_title", "All Cost Centres");
                ACalculator.AddParameter("param_rgrCostCentres", "AllCostCentres");
            }
            else if (rbtCostCentreFromList.Checked)
            {
                VerificationResult = TGuiChecks.ValidateCheckedListBoxVersatile(clbCostCentres);

                if (VerificationResult != null)
                {
                    VerificationResult = new TVerificationResult(Catalog.GetString("Select Cost Centre Codes from list"),
                        Catalog.GetString("No Cost Centre was selected!"),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                String CostCentreListTitle = clbCostCentres.GetCheckedStringList();
                ACalculator.AddStringParameter("param_cost_centre_codes", CostCentreListTitle);
                CostCentreListTitle = CostCentreListTitle.Replace("\"", "");

                if (CostCentreListTitle.Length > 25)
                {
                    CostCentreListTitle = "Selected Cost Centres";
                }

                // need to set NOTUSED,
                // otherwise the report generator complains about the missing parameter
                // NOTUSED is used as an invalid value, there is no Cost Centre with this name
                ACalculator.AddParameter("param_cost_centre_list_title", CostCentreListTitle);
                ACalculator.AddParameter("param_cost_centre_code_start", "*NOTUSED*");
                ACalculator.AddParameter("param_cost_centre_code_end", "*NOTUSED*");
                ACalculator.AddParameter("param_rgrCostCentres", "CostCentreList");
            }
            else
            {
                ACalculator.AddParameter("param_cost_centre_list_title",
                    cmbFromCostCentre.GetSelectedString() + " To " + cmbToCostCentre.GetSelectedString());
                ACalculator.AddParameter("param_cost_centre_codes", "*NOTUSED*");
                ACalculator.AddParameter("param_cost_centre_code_start", cmbFromCostCentre.GetSelectedString());
                ACalculator.AddParameter("param_cost_centre_code_end", cmbToCostCentre.GetSelectedString());

                VerificationResult = TStringChecks.FirstLesserOrEqualThanSecondString(
                    cmbFromCostCentre.GetSelectedString(),
                    cmbToCostCentre.GetSelectedString(),
                    Catalog.GetString("Cost Centre Range From"),
                    Catalog.GetString("Cost Centre Range To"));

                if (VerificationResult != null)
                {
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                ACalculator.AddParameter("param_rgrCostCentres", "CostCentreRange");
            }

            ACalculator.AddParameter("param_depth", "standard"); // I don't want this, but I'll keep it for a while...
        }

        /// <summary>
        /// Sets the selected values in the controls, using the parameters loaded from a file
        ///
        /// </summary>
        /// <param name="AParameters"></param>
        /// <returns>void</returns>
        public void SetControls(TParameterList AParameters)
        {
            if (FLedgerNumber == -1)
            {
                // we will wait until the ledger number has been set
                return;
            }

            /* cost centre options */

            FCostCenterCodesDuringLoad = AParameters.Get("param_cost_centre_codes").ToString();
            FAccountCodesDuringLoad = AParameters.Get("param_account_codes").ToString();

            String rbtSel = AParameters.Get("param_rgrAccounts").ToString();

            if (rbtSel == "AllAccounts")
            {
                rbtAllAccounts.Checked = true;
            }
            else if (rbtSel == "AccountList")
            {
                rbtAccountFromList.Checked = true;
            }
            else
            {
                rbtAccountRange.Checked = true;
                cmbFromAccountCode.SetSelectedString(AParameters.Get("param_account_code_start").ToString());
                cmbToAccountCode.SetSelectedString(AParameters.Get("param_account_code_end").ToString());

                if (cmbFromAccountCode.GetSelectedString().Length == 0)
                {
                    // select first valid entry
                    cmbFromAccountCode.SelectedIndex = 1;
                }

                if (cmbToAccountCode.GetSelectedString().Length == 0)
                {
                    // select last valid entry
                    cmbToAccountCode.SelectedIndex = cmbToAccountCode.Count - 1;
                }
            }

            rbtSel = AParameters.Get("param_rgrCostCentres").ToString();

            if (rbtSel == "AllCostCentres")
            {
                rbtAllCostCentres.Checked = true;
            }
            else if (rbtSel == "CostCentreList")
            {
                if (AParameters.Get("param_cost_centre_codes").ToString() == "*LOCAL*")
                {
                    /* this happens when the default.xml setting file is loaded */
                    FCostCenterCodesDuringLoad = FLedgerNumber.ToString() + "00";
                }

                rbtCostCentreFromList.Checked = true;
            }
            else
            {
                rbtCostCentreRange.Checked = true;
                cmbFromCostCentre.SetSelectedString(AParameters.Get("param_cost_centre_code_start").ToString());
                cmbToCostCentre.SetSelectedString(AParameters.Get("param_cost_centre_code_end").ToString());

                if (cmbFromCostCentre.GetSelectedString().Length == 0)
                {
                    // select first valid entry
                    cmbFromCostCentre.SelectedIndex = 1;
                }

                if (cmbToCostCentre.GetSelectedString().Length == 0)
                {
                    // select last valid entry
                    cmbToCostCentre.SelectedIndex = cmbToCostCentre.Count - 1;
                }
            }

            if (FLedgerNumber != -1)
            {
                SetLists();
            }
        }

        #endregion
        #region eventhandler
        private void UnselectAllCostCentres(System.Object sender, System.EventArgs e)
        {
            clbCostCentres.ClearSelected();
            cmbSummaryCostCentres.SetSelectedString("");
        }

        private void UnselectAllAccountCodes(System.Object sender, System.EventArgs e)
        {
            clbAccountCodes.ClearSelected();
        }

        private void SelectAllReportingCostCentres(System.Object sender, System.EventArgs e)
        {
            string SelectedCostCentres = clbCostCentres.GetCheckedStringList();

            SelectedCostCentres = StringHelper.ConcatCSV(SelectedCostCentres,
                TRemote.MFinance.Reporting.WebConnectors.GetReportingCostCentres(
                    FLedgerNumber,
                    cmbSummaryCostCentres.GetSelectedString(),
                    string.Empty));
            clbCostCentres.SetCheckedStringList(SelectedCostCentres);
        }

        private void SelectAllExpenseAccounts(System.Object sender, System.EventArgs e)
        {
            String expenseAccounts = "";
            DataTable AccountsTable = clbAccountCodes.BoundDataTable;

            for (Int32 Idx = 0; Idx < AccountsTable.Rows.Count; Idx++)
            {
                if (AccountsTable.Rows[Idx]["a_account_type_c"].ToString() == "Expense")
                {
                    if (expenseAccounts != "")
                    {
                        expenseAccounts += ",";
                    }

                    expenseAccounts += AccountsTable.Rows[Idx]["a_account_code_c"].ToString();
                }
            }

            clbAccountCodes.SetCheckedStringList(expenseAccounts);
        }

        // Enable/disable btnSelectAllReportingCostCentres depending on value of cmbSummaryCostCentres.
        // Fired when rbtCostCentreFromList is selected or when a value is chosen from cmbSummaryCostCentres.
        private void SummaryCostCentresChanged(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbSummaryCostCentres.cmbCombobox.Text))
            {
                btnSelectAllReportingCostCentres.Enabled = false;
            }
            else
            {
                btnSelectAllReportingCostCentres.Enabled = true;
            }
        }

        // fired when rbtCostCentreFromList is changed
        private void CostCentreChanged(System.Object sender, System.EventArgs e)
        {
            btnUnselectAllCostCentres.Enabled = rbtCostCentreFromList.Checked;
            cmbSummaryCostCentres.Enabled = rbtCostCentreFromList.Checked;

            if (rbtCostCentreFromList.Checked)
            {
                SummaryCostCentresChanged(this, null);
            }
            else
            {
                btnSelectAllReportingCostCentres.Enabled = false;
            }
        }

        // fired when rbtAccountFromList is changed
        private void AccountChanged(System.Object sender, System.EventArgs e)
        {
            btnAllExpenseAccounts.Enabled = rbtAccountFromList.Checked;
            btnUnselectAllAccountCodes.Enabled = rbtAccountFromList.Checked;
        }

        #endregion

        private void SetLists()
        {
            if ((FCostCenterCodesDuringLoad.Length > 0) && (FCostCenterCodesDuringLoad != "*NOTUSED*"))
            {
                clbCostCentres.SetCheckedStringList(FCostCenterCodesDuringLoad);
                FCostCenterCodesDuringLoad = "";
            }

            if ((FAccountCodesDuringLoad.Length > 0) && (FAccountCodesDuringLoad != "*NOTUSED*"))
            {
                clbAccountCodes.SetCheckedStringList(FAccountCodesDuringLoad);
                FAccountCodesDuringLoad = "";
            }
        }
    }
}