//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    /// <summary>
    /// manual code for TFrmUnitHierarchy class
    /// </summary>
    public partial class TFrmUnitHierarchy
    {
        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (AReportAction == TReportActionEnum.raGenerate)
            {
                TVerificationResult VerificationResult = null;
                long UnitKey = 0;

                try
                {
                    UnitKey = Convert.ToInt64(txtPartnerKey.Text);
                }
                catch (Exception)
                {
                    UnitKey = 0;
                }

                if (UnitKey == 0)
                {
                    VerificationResult = new TVerificationResult("Insert a valid Key.",
                        "No Unit Key selected!",
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
            }

            int ColumnCounter;

            ColumnCounter = 0;
            ACalc.AddParameter("param_calculation", "UnitKey", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", "3", ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "UnitType", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", "4.5", ColumnCounter);
            ++ColumnCounter;
            ACalc.AddParameter("param_calculation", "UnitName", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", "12", ColumnCounter);

            ACalc.SetMaxDisplayColumns(3);

            ACalc.AddParameter("param_txtUnitCode", txtPartnerKey.Text);
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            txtPartnerKey.Text = AParameters.Get("param_txtUnitCode").ToString();
        }

        private void FindUnit(System.Object sender, EventArgs e)
        {
            Int64 PartnerKey = -1;
            String ResultStringLbl;
            TPartnerClass? PartnerClass;
            TLocationPK ResultLocationPK;

            // the user has to select an existing partner to make that partner a supplier
            if (TPartnerFindScreenManager.OpenModalForm("UNIT",
                    out PartnerKey,
                    out ResultStringLbl,
                    out PartnerClass,
                    out ResultLocationPK,
                    this))
            {
                txtPartnerKey.Text = PartnerKey.ToString();
            }
        }

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
        }

        private bool LoadReportData(TRptCalculator ACalc)
        {
            return FPetraUtilsObject.FFastReportsPlugin.LoadReportData("UnitHierarchyReport",
                false,
                new string[] { "UnitHierarchy" },
                ACalc,
                this,
                true,
                false);
        }

        /// <summary>
        /// Sets the Unit Key
        /// </summary>
        /// <param name="AUnitKey"></param>
        public void SetParameters(Int64 AUnitKey)
        {
            txtPartnerKey.Text = AUnitKey.ToString();
        }
    }
    /// <summary>
    /// Manages the opening of a new/showing of an existing Instance of the Print Unit Hierarchy Screen.
    /// </summary>
    public static class TPrintUnitHierarchy
    {
        /// <summary>
        /// Opens a Modal instance of the Print Unit Hierarchy screen.
        /// </summary>
        /// <param name="AUnitKey">UnitKey of the Partner to print.</param>
        /// <param name="AParentForm"></param>
        public static bool OpenModalForm(Int64 AUnitKey, Form AParentForm)
        {
            TFrmUnitHierarchy UnitHierarchyForm;
            DialogResult dlgResult;

            UnitHierarchyForm = new TFrmUnitHierarchy(AParentForm);
            UnitHierarchyForm.SetParameters(AUnitKey);

            dlgResult = UnitHierarchyForm.ShowDialog();

            if (dlgResult == System.Windows.Forms.DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}