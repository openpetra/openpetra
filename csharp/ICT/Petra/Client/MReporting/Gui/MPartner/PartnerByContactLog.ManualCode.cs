//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr, andreww
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using GNU.Gettext;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    public partial class TFrmPartnerByContactLog
    {

        private void InitializeManualCode()
        {
            //ucoPartnerSelection.SetRestrictedPartnerClasses("PERSON");
        }

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            // if (!FDirectRelationshipTable.Select(FDirectRelationshipTable.DefaultView.RowFilter).Any(r => (bool)r["Selection"])
                // && !FReciprocalRelationshipTable.Select(FReciprocalRelationshipTable.DefaultView.RowFilter).Any(r => (bool)r["Selection"]))
            // {
                // TVerificationResult VerificationResult = new TVerificationResult(
                    // Catalog.GetString("Select at least one Relationship Type to run the report."),
                    // Catalog.GetString("No Relationship Type selected!"),
                    // TResultSeverity.Resv_Critical);
                // FPetraUtilsObject.AddVerificationResult(VerificationResult);
            // }
        }

        private void RunOnceOnActivationManual()
        {
            // InitRelationshipList();
            // this.cmbRelationCategory.SelectedIndexChanged += new System.EventHandler(this.CmbRelationCategorySelectedIndexChanged);
            // grdReciprocalRelationship.Visible = false;
            // lblSelectReciprocalRelationship.Visible = false;

            // if (CalledFromExtracts)
            // {
                // rbtDirectRelationship.Visible = false;
                // rbtReciprocalRelationship.Visible = false;
                // grdReciprocalRelationship.Visible = true;
                // lblSelectReciprocalRelationship.Visible = true;

                // tabReportSettings.Controls.Remove(tpgColumns);
                // tabReportSettings.Controls.Remove(tpgReportSorting);
            // }
            // else
            // {
                // grdReciprocalRelationship.Visible = false;
                // lblSelectReciprocalRelationship.Visible = false;
            // }

            // rbtDirectRelationship.Checked = true;
            // rbtDirectRelationship.Enabled = true;
            // chkCategoryFilter.Checked = false;
            // cmbRelationCategory.Enabled = false;


            // ucoChkFilter.ShowFamiliesOnly(false);
        }

        private void grdDirectRelationship_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
        }

        private void grdReciprocalRelationship_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
        }

        private void grdReciprocalRelationship_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
        }

        private void grdReciprocalRelationship_SetControls(TParameterList AParameters)
        {
        }

        private void grdDirectRelationship_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            // if (!CalledFromExtracts)
            // {
                // if (rbtReciprocalRelationship.Checked)
                // {
                    // ACalc.AddParameter("param_use_reciprocal_relationship", "true");
                // }
                // else
                // {
                    // ACalc.AddParameter("param_use_reciprocal_relationship", "false");
                // }

                // String RelationshipTypeList = GetSelectedRelationshipsAsCsv();
                // ACalc.AddParameter("param_relationship_types", RelationshipTypeList);
            // }
            // else
            // {
                // ACalc.AddParameter("param_explicit_relationships", GetSelectedRelationshipsAsCsv(true));
                // ACalc.AddParameter("param_reciprocal_relationships", GetSelectedRelationshipsAsCsv(false));
            // }
        }

        private void grdDirectRelationship_SetControls(TParameterList AParameters)
        {
            // if (AParameters.Get("param_use_reciprocal_relationship").ToString() == "true")
            // {
                // rbtReciprocalRelationship.Checked = true;
            // }
            // else
            // {
                // rbtDirectRelationship.Checked = true;
            // }

            // String SelectedRelationshipTypes = AParameters.Get("param_relationship_types").ToString();
            // SelectRelationshipTypes(SelectedRelationshipTypes);
        }

        #region EventHandling
        private void FilterRelationCategoryChanged(System.Object sender, EventArgs e)
        {
            // cmbRelationCategory.Enabled = chkCategoryFilter.Checked;

            // if (!chkCategoryFilter.Checked)
            // {
                ////Show all relationship types
                // UpdateRelationshipGrid("");
            // }
            // else
            // {
                // CmbRelationCategorySelectedIndexChanged(null, null);
            // }
        }

        private void rbtRelationshipDirectionChanged(System.Object sender, EventArgs e)
        {
            // if (!CalledFromExtracts)
            // {
                // grdReciprocalRelationship.Visible = rbtReciprocalRelationship.Checked;
                // lblSelectReciprocalRelationship.Visible = rbtReciprocalRelationship.Checked;
                // grdDirectRelationship.Visible = rbtDirectRelationship.Checked;
                // lblSelectDirectRelationship.Visible = rbtDirectRelationship.Checked;
            // }
        }

        private void CmbRelationCategorySelectedIndexChanged(object sender, EventArgs e)
        {
            // if (cmbRelationCategory.SelectedIndex >= 0)
            // {
                // String SelectedType = cmbRelationCategory.Items[cmbRelationCategory.SelectedIndex].ToString();

                // UpdateRelationshipGrid(SelectedType);
            // }
            // else
            // {
                // UpdateRelationshipGrid("");
            // }
        }

        #endregion
    }
}