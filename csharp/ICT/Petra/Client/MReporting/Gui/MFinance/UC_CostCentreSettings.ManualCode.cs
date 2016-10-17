//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
//       Tim Ingham
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
using SourceGrid.Selection;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    /// <summary>
    /// Description of TFrmUC_CostCentreSettings.
    /// </summary>
    public partial class TFrmUC_CostCentreSettings
    {
        private int FLedgerNumber;
        private String FCostCenterCodesDuringLoad;

        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FCostCenterCodesDuringLoad = "";

            // otherwise rbtSelectedCostCentres slightly obscures clbCostCentres
            rbtSelectedCostCentres.SendToBack();
            clbCostCentres.MouseDown += clbCostCentres_MouseDown;
        }

        void clbCostCentres_MouseDown(object sender, MouseEventArgs e)
        {
            rbtSelectedCostCentres.Checked = true;
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
            if (rbtSelectedCostCentres.Checked
                && (clbCostCentres.GetCheckedStringList().Length == 0)
                && (AReportAction == TReportActionEnum.raGenerate))
            {
                TVerificationResult VerificationResult = new TVerificationResult(Catalog.GetString("Selected Cost Centres"),
                    Catalog.GetString("No cost centre was selected!"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            /* cost centre options */
            if (rbtAccountLevel.Checked)
            {
                ACalculator.AddParameter("param_costcentreoptions", "AccountLevel");
            }
            else if (rbtAllCostCentres.Checked)
            {
                ACalculator.AddParameter("param_costcentreoptions", "AllCostCentres");
            }
            else if (rbtAllActiveCostCentres.Checked)
            {
                ACalculator.AddParameter("param_costcentreoptions", "AllActiveCostCentres");
            }
            else if (rbtSelectedCostCentres.Checked)
            {
                ACalculator.AddParameter("param_costcentreoptions", "SelectedCostCentres");
            }

            ACalculator.AddParameter("param_paginate", chkPaginate.Checked);
            ACalculator.AddParameter("param_auto_email", chkAutoEmail.Checked);

            String CostCentreListTitle;

            if (rbtAllCostCentres.Checked || rbtAllActiveCostCentres.Checked)
            {
                CostCentreListTitle = clbCostCentres.GetAllStringList(false);
            }
            else
            {
                CostCentreListTitle = clbCostCentres.GetCheckedStringList();
            }

            ACalculator.AddStringParameter("param_cost_centre_codes", CostCentreListTitle);
            CostCentreListTitle = CostCentreListTitle.Replace("\"", "");

            if (CostCentreListTitle.Length > 25)
            {
                CostCentreListTitle = "Selected Cost Centres";
            }

            ACalculator.AddParameter("param_cost_centre_list_title", CostCentreListTitle);

            ACalculator.AddParameter("param_cost_centre_summary", chkCostCentreBreakdown.Checked);
            ACalculator.AddParameter("param_cost_centre_breakdown", chkCostCentreBreakdown.Checked);
            ACalculator.AddParameter("ExcludeInactiveCostCentres", chkExcludeInactiveCostCentres.Checked);

            /* Level of Detail */
            if (this.rbtDetail.Checked)
            {
                ACalculator.AddParameter("param_depth", "Detail");
            }
            else if (this.rbtStandard.Checked)
            {
                ACalculator.AddParameter("param_depth", "Standard");
            }
            else
            {
                ACalculator.AddParameter("param_depth", "Summary");
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
            /* cost centre options */
            rbtAccountLevel.Checked = AParameters.Get("param_costcentreoptions").ToString() == "AccountLevel";
            rbtAllCostCentres.Checked = AParameters.Get("param_costcentreoptions").ToString() == "AllCostCentres";
            rbtAllActiveCostCentres.Checked = AParameters.Get("param_costcentreoptions").ToString() == "AllActiveCostCentres";
            rbtSelectedCostCentres.Checked = AParameters.Get("param_costcentreoptions").ToString() == "SelectedCostCentres";

            FCostCenterCodesDuringLoad = AParameters.Get("param_cost_centre_codes").ToString();
            clbCostCentres.SetCheckedStringList(FCostCenterCodesDuringLoad);

            chkCostCentreBreakdown.Checked = AParameters.Get("param_cost_centre_breakdown").ToBool();
            chkExcludeInactiveCostCentres.Checked = AParameters.Get("ExcludeInactiveCostCentres").ToBool();

            /* Level of Detail */
            rbtDetail.Checked = AParameters.Get("param_depth").ToString() == "Detail";
            rbtStandard.Checked = AParameters.Get("param_depth").ToString() == "Standard";
            rbtSummary.Checked = AParameters.Get("param_depth").ToString() == "Summary";
            chkPaginate.Checked = AParameters.Get("param_paginate").ToBool();
            chkAutoEmail.Checked = AParameters.Get("param_auto_email").ToBool();
        }

        #endregion
        private void UnselectAll(System.Object sender, System.EventArgs e)
        {
            clbCostCentres.ClearSelected();
        }

        private void rbtSelectedCostCentresChanged(System.Object sender, System.EventArgs e)
        {
            if (!rbtSelectedCostCentres.Checked)
            {
                chkCostCentreBreakdown.Checked = false;
                chkCostCentreBreakdown.Enabled = false;
            }
            else
            {
                chkCostCentreBreakdown.Enabled = true;
            }
        }

        private void chkExcludeCostCentresChanged(System.Object sender, System.EventArgs e)
        {
            if (FLedgerNumber > 0)
            {
                InitialiseCostCentreList(FLedgerNumber);
            }
        }

        /// <summary>
        /// Init the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void InitialiseCostCentreList(int ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;

            TFinanceControls.InitialiseCostCentreList(ref clbCostCentres, ALedgerNumber, false, false, chkExcludeInactiveCostCentres.Checked, true);

            if (FCostCenterCodesDuringLoad.Length > 0)
            {
                clbCostCentres.SetCheckedStringList(FCostCenterCodesDuringLoad);
                FCostCenterCodesDuringLoad = "";
            }
            else
            {
                clbCostCentres.SetCheckedStringList("");
            }
        }
    }
}