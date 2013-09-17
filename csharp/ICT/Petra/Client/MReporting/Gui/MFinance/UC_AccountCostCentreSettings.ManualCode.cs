//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr, timop
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
            rbtAccountRange.Checked = true;
            rbtAccountFromListCheckedChanged(null, null);
            rbtCostCentreRange.Checked = true;
            rbtCostCentreFromListCheckedChanged(null, null);
        }

        /// <summary>
        /// initialise the controls with the values from the ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void InitialiseLedger(int ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;

            TFinanceControls.InitialiseAccountList(ref cmbFromAccountCode, FLedgerNumber, true, false, false, false);
            TFinanceControls.InitialiseAccountList(ref cmbToAccountCode, FLedgerNumber, true, false, false, false);
            TFinanceControls.InitialiseCostCentreList(ref cmbFromCostCentre, FLedgerNumber, true, false, false, false);
            TFinanceControls.InitialiseCostCentreList(ref cmbToCostCentre, FLedgerNumber, true, false, false, false);
            TFinanceControls.InitialiseCostCentreList(ref cmbSummaryCostCentres, FLedgerNumber, false, true, false, false);
            TFinanceControls.InitialiseAccountList(ref clbAccountCodes, FLedgerNumber, true, false, false, false);
            TFinanceControls.InitialiseCostCentreList(ref clbCostCentres, FLedgerNumber, true, false, false, false);

            cmbFromAccountCode.SelectedIndex = 0;
            cmbToAccountCode.SelectedIndex = cmbToAccountCode.Count - 1;
            cmbFromCostCentre.SelectedIndex = 0;
            cmbToCostCentre.SelectedIndex = cmbToCostCentre.Count - 1;
            clbAccountCodes.SetCheckedStringList("");
            clbCostCentres.SetCheckedStringList("");
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

            if (rbtAccountFromList.Checked)
            {
                if ((clbAccountCodes.GetCheckedStringList().Length == 0)
                    && (AReportAction == TReportActionEnum.raGenerate))
                {
                    VerificationResult = new TVerificationResult(Catalog.GetString("Select Account Codes"),
                        Catalog.GetString("No Account Code was selected!"),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                ACalculator.AddParameter("param_account_codes", clbAccountCodes.GetCheckedStringList());
                // need to set NOTUSED,
                // otherwise the report generator cannot find the parameter,
                // and complains in the log file and on the status bar about the missing parameter
                // NOTUSED is used as an invalid value, there is no account with this name
                ACalculator.AddParameter("param_account_code_start", "*NOTUSED*");
                ACalculator.AddParameter("param_account_code_end", "*NOTUSED*");
                ACalculator.AddParameter("param_rgrAccounts", "AccountList");
            }
            else
            {
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

            if (rbtCostCentreFromList.Checked)
            {
                VerificationResult = TGuiChecks.ValidateCheckedListBoxVersatile(clbCostCentres);

                if (VerificationResult != null)
                {
                    VerificationResult = new TVerificationResult(Catalog.GetString("Select CostCentre Codes from list"),
                        Catalog.GetString("No cost centre was selected!"),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }

                String CostCentreListTitle = clbCostCentres.GetCheckedStringList();
                ACalculator.AddParameter("param_cost_centre_codes", CostCentreListTitle);
                CostCentreListTitle = CostCentreListTitle.Replace("\"", "");
                if (CostCentreListTitle.Length > 25)
                {
                    CostCentreListTitle = "Selected Cost Centres";
                }
                ACalculator.AddParameter("param_cost_centre_list_title", CostCentreListTitle);

                ACalculator.AddParameter("param_cost_centre_code_start", "*NOTUSED*");
                ACalculator.AddParameter("param_cost_centre_code_end", "*NOTUSED*");
                ACalculator.AddParameter("param_rgrCostCentres", "CostCentreList");
            }
            else
            {
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

            ACalculator.AddParameter("param_depth", "standard");

            /* layout */
            ACalculator.AddColumnLayout(0, 6, 0, 2);
            ACalculator.AddColumnLayout(1, 8, 0, 3);
            ACalculator.AddColumnLayout(2, 11, 0, 1);
            ACalculator.AddColumnLayout(3, 12, 0, 7);
//            ACalculator.AddColumnLayout(4, 13, 0, 3);
//            ACalculator.AddColumnLayout(5, 16, 0, 3);
            ACalculator.AddColumnLayout(4, 19, 0, 3);
            ACalculator.AddColumnLayout(5, 22, 0, 3);

            if (FReportWithBalance == true)
            {
                ACalculator.SetMaxDisplayColumns(6);
            }
            else
            {
                ACalculator.SetMaxDisplayColumns(4);
            }

            ACalculator.AddColumnCalculation(0, "Debit");
            ACalculator.AddColumnCalculation(1, "Credit");
//          ACalculator.AddColumnCalculation(2, "Transaction Currency");
            ACalculator.AddColumnCalculation(3, "Transaction Narrative");

            if (FReportWithBalance == true)
            {
                ACalculator.AddColumnCalculation(4, "Start Balance");
                ACalculator.AddColumnCalculation(5, "End Balance");
            }
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

            if (AParameters.Get("param_rgrAccounts").ToString() == "AccountList")
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

            if (AParameters.Get("param_rgrCostCentres").ToString() == "CostCentreList")
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

        #endregion

        private void SetLists()
        {
            if (FCostCenterCodesDuringLoad.Length > 0)
            {
                clbCostCentres.SetCheckedStringList(FCostCenterCodesDuringLoad);
                FCostCenterCodesDuringLoad = "";
            }
            else
            {
                clbCostCentres.SetCheckedStringList("");
            }

            if (FAccountCodesDuringLoad.Length > 0)
            {
                clbAccountCodes.SetCheckedStringList(FAccountCodesDuringLoad);
                FAccountCodesDuringLoad = "";
            }
            else
            {
                clbAccountCodes.SetCheckedStringList("");
            }
        }
    }
}