//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MPersonnel.Gui.Setup;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_PartnerDetails_Unit
    {
        #region Public Methods

        /// <summary>
        /// Gets the data from all controls on this UserControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls2()
        {
            GetDataFromControls(FMainDS.PUnit[0]);
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamically loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        private void SetParentLabel(Int64 AParentUnitKey)
        {
            String PartnerShortName;

            Shared.TPartnerClass PartnerClass;
            TServerLookup.TMPartner.GetPartnerShortName(AParentUnitKey, out PartnerShortName, out PartnerClass, true);
            FPetraUtilsObject.UnhookControl(lblParentName, false); // I don't want this change to cause SetChangedFlag.
            lblParentName.Text = PartnerShortName;
        }

        private void ShowDataManual(PUnitRow UnitRow)
        {
            lblParentName.Left = 250;
            btnOrganise.Left = 430;

            if ((FMainDS.UmUnitStructure != null)
                && (FMainDS.UmUnitStructure.Rows.Count > 0))
            {
                txtParentKey.Text = FMainDS.UmUnitStructure[0].ParentUnitKey.ToString("D10");
                SetParentLabel(FMainDS.UmUnitStructure[0].ParentUnitKey);
                btnOrganise.Enabled = true;
            }
            else
            {
                btnOrganise.Enabled = false;
            }

            // If this unit has a corresponding conference in the database then the user should not be able to change the currency code from
            // the partner edit screen. They can change it in the Conference Master Settings screen.
            if (TRemote.MPartner.Partner.WebConnectors.IsPUnitAConference(UnitRow.PartnerKey))
            {
                cmbOutreachCostCurrencyCode.Enabled = false;
                FPetraUtilsObject.SetStatusBarText(txtOutreachCost, Catalog.GetString("Enter the cost of this outreach" +
                        Catalog.GetString(" (The currency can be modified in 'Conference Master Settings' in the the Conference module)")));
            }
        }

        private void OpenUnitHierarchy(object sender, EventArgs e)
        {
            TFrmUnitHierarchy HierarchyForm = new TFrmUnitHierarchy(this.ParentForm);

            HierarchyForm.Show();
            HierarchyForm.ShowThisUnit(FMainDS.PPartner[0].PartnerKey);
        }

        /// <summary>
        /// Refreshes position in Uni Hierarchy
        /// </summary>
        /// <param name="AUnitHierarchyChange">All Unit Hierarchies that have been changed.</param>
        public void RefreshUnitHierarchy(Tuple <string, Int64, Int64>AUnitHierarchyChange)
        {
            if (AUnitHierarchyChange.Item2 == FMainDS.PPartner[0].PartnerKey)
            {
                FPetraUtilsObject.UnhookControl(txtParentKey, false); // I don't want this change to cause SetChangedFlag.
                txtParentKey.Text = AUnitHierarchyChange.Item3.ToString("D10");

                FPetraUtilsObject.UnhookControl(lblParentName, false); // I don't want this change to cause SetChangedFlag.
                lblParentName.Text = AUnitHierarchyChange.Item1;

                btnOrganise.Enabled = true;
            }
        }
    }
}