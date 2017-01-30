//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//
// Copyright 2004-2016 by OM International
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
using System.Collections.Generic;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Collections;
using Ict.Petra.Shared.MFinance.GL.Data;
using System.Windows.Forms;
using Ict.Petra.Client.MReporting.Gui;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmHOSA
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                uco_GeneralSettings.HideDateRange();

                FLedgerNumber = value;

                InitialiseCostCentreList();
                uco_GeneralSettings.InitialiseLedger(FLedgerNumber);
                uco_GeneralSettings.CurrencyOptions(new object[] { "Base", "International" });

                FPetraUtilsObject.LoadDefaultSettings();
            }
        }

        private string FCostCenterCodesDuringLoad = string.Empty;

        /// <summary>
        /// Sets the selected values in the controls, using the parameters loaded from a file
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControlsManual(TParameterList AParameters)
        {
            FCostCenterCodesDuringLoad = AParameters.Get("param_cost_centre_codes").ToString();
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddColumnLayout(0, 8, 0, 3);
            ACalc.AddColumnLayout(1, 11, 0, 3);
            ACalc.AddColumnLayout(2, 14, 0, 7);
            ACalc.SetMaxDisplayColumns(3);
            ACalc.AddColumnCalculation(0, "Debit");
            ACalc.AddColumnCalculation(1, "Credit");
            ACalc.AddColumnCalculation(2, "Transaction Narrative");
            ACalc.AddParameter("param_daterange", "false");
            ACalc.AddParameter("param_rgrAccounts", "AllAccounts");
            ACalc.AddParameter("param_rgrCostCentres", "CostCentreList");
            // TODO need to allow to specify an ICH run number
            ACalc.AddParameter("param_ich_number", 0);
            ACalc.AddParameter("param_include_rgs", false);

            ACalc.AddStringParameter("param_cost_centre_codes", clbCostCentres.GetCheckedStringList(true));
        }

        private void chkExcludeCostCentresChanged(System.Object sender, System.EventArgs e)
        {
            if (FLedgerNumber > 0)
            {
                InitialiseCostCentreList();
            }
        }

        private void FilterCostCentre(System.Object sender, System.EventArgs e)
        {
            clbCostCentres.BoundDataTable.DefaultView.RowFilter = "a_cost_centre_name_c like '%" + txtCostCentreFilter.Text + "%'";
        }

        private void ClearAllCostCentres(System.Object sender, System.EventArgs e)
        {
            txtCostCentreFilter.Text = "";
            clbCostCentres.BoundDataTable.DefaultView.RowFilter = "";
            clbCostCentres.SetCheckedStringList("");
        }

        /// <summary>
        /// Init the grid
        /// </summary>
        private void InitialiseCostCentreList()
        {
            TFinanceControls.InitialiseCostCentreList(
                ref clbCostCentres,
                FLedgerNumber,
                true,   // postingonly
                false,  // excludeposting
                chkExcludeInactiveCostCentres.Checked,
                rbtFields.Checked,
                rbtDepartments.Checked,
                rbtPersonalCostcentres.Checked);

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