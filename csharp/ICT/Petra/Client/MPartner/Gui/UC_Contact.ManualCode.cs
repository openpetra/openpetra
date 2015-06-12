//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       andreww, peters
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
using System.Data;
using System.Windows.Forms;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_Contact
    {
        private PContactLogRow FContactDR = null;
        private DataView FGridTableDV = null;

        private bool FInitializationRunning {
            get; set;
        }

        /// <summary>todoComment</summary>
        public void SpecialInitUserControl()
        {
            if (FValidationControlsDict.Count == 0)
            {
                BuildValidationControlsDict();
            }
        }

        private PContactLogRow GetSelectedMasterRow()
        {
            return FContactDR;
        }

        /// <summary>
        /// Display data in control based on data from ARow
        /// </summary>
        /// <param name="ARow"></param>
        public void ShowDetails(PContactLogRow ARow)
        {
            FInitializationRunning = true;

            FContactDR = ARow;

            ShowData(ARow);

            // if this is the first row to be showing then we need to set up the grid
            if ((FGridTableDV == null) && (FMainDS.PPartnerContactAttribute != null) && (FMainDS.PPartnerContactAttribute.Count > 0))
            {
                ContactAttributesLogic.SetupContactAttributesGrid(ref grdSelectedAttributes,
                    FMainDS.PPartnerContactAttribute,
                    true,
                    FContactDR.ContactLogId);
            }

            if (FGridTableDV != null)
            {
                FGridTableDV.RowFilter = PPartnerContactAttributeTable.GetContactIdDBName() + " = " + ARow.ContactLogId;
            }

            FInitializationRunning = false;
        }

        /// <summary>
        /// Read data from controls into ARow parameter
        /// </summary>
        /// <param name="ARow"></param>
        public void GetDetails(PContactLogRow ARow)
        {
            ValidateAllData(TErrorProcessingMode.Epm_None);
            GetDataFromControls(ARow);
        }

        /// <summary>
        /// Code associated with ContactLog entry
        /// </summary>
        public string ContactCode {
            get
            {
                return FContactDR.ContactCode;
            }
        }

        /// <summary>
        /// Date of Contact
        /// </summary>
        public DateTime ContactDate {
            get
            {
                return FContactDR.ContactDate;
            }
        }

        /// <summary>
        /// Person/entity contacting this partner
        /// </summary>
        public string Contactor
        {
            get
            {
                return txtContactor.Text;
            }
            set
            {
                txtContactor.Text = value;
            }
        }

        /// <summary>
        /// Open a dialog to select Contact Attributes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectAttributes(object sender, EventArgs e)
        {
            TFrmContactAttributesDialog ContactAttributesDialog = new TFrmContactAttributesDialog(FPetraUtilsObject.GetForm());

            ContactAttributesDialog.ContactID = FContactDR.ContactLogId;
            ContactAttributesDialog.SelectedContactAttributeTable = FMainDS.PPartnerContactAttribute;

            if (ContactAttributesDialog.ShowDialog() == DialogResult.OK)
            {
                PPartnerContactAttributeTable Changes = ContactAttributesDialog.SelectedContactAttributeTable.GetChangesTyped();

                // if changes were made or a previously added row (unsaved) was deleted
                if ((Changes != null) || ContactAttributesDialog.AddedAttributeDeleted)
                {
                    FMainDS.PPartnerContactAttribute.Clear();
                    FMainDS.PPartnerContactAttribute.Merge(ContactAttributesDialog.SelectedContactAttributeTable);

                    FGridTableDV = ContactAttributesLogic.SetupContactAttributesGrid(ref grdSelectedAttributes,
                        FMainDS.PPartnerContactAttribute,
                        true,
                        FContactDR.ContactLogId);

                    // only enable save if there are actual changes from the original datatable
                    if (Changes != null)
                    {
                        FPetraUtilsObject.SetChangedFlag();
                    }
                }
            }
        }
    }
}