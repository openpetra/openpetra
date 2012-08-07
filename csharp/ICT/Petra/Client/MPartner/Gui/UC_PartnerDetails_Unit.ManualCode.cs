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
using Ict.Petra.Client.MPersonnel.Gui.Setup;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core;

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
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
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

            if (FMainDS.UmUnitStructure.Rows.Count > 0)
            {
                txtParentKey.Text = FMainDS.UmUnitStructure[0].ParentUnitKey.ToString("D10");
                SetParentLabel(FMainDS.UmUnitStructure[0].ParentUnitKey);
                btnOrganise.Enabled = true;
            }
            else
            {
                btnOrganise.Enabled = false;
            }
        }

        private void OpenUnitHierarchy(object sender, EventArgs e)
        {
            TFrmUnitHierarchy HierarchyForm = new TFrmUnitHierarchy(this.ParentForm);

            HierarchyForm.Show();
            HierarchyForm.ShowThisUnit(FMainDS.PPartner[0].PartnerKey);
            HierarchyForm.ReassignEvent += new UnitReassignHandler(HierarchyForm_ReassignEvent);
        }

        void HierarchyForm_ReassignEvent(long ChildKey, long ParentKey)
        {
            if (ChildKey == FMainDS.PPartner[0].PartnerKey)
            {
                FPetraUtilsObject.UnhookControl(txtParentKey, false); // I don't want this change to cause SetChangedFlag.
                txtParentKey.Text = ParentKey.ToString("D10");
                SetParentLabel(ParentKey);
            }
        }
    }
}