//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       andreww
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
using System.Collections.Generic;
using System.Windows.Forms;

using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    public partial class TFrmPartnerByContactLog
    {
        PPartnerContactAttributeTable FAttributeTable = new PPartnerContactAttributeTable();

        private void RunOnceOnActivationManual()
        {
            if (CalledFromExtracts)
            {
                tabReportSettings.Controls.Remove(tpgColumns);
            }

            var addressSettings = new TParameterList();
            addressSettings.Add("param_active", true);
            addressSettings.Add("param_mailing_addresses_only", true);
            addressSettings.Add("param_families_only", false);
            addressSettings.Add("param_exclude_no_solicitations", true);
            ucoChkFilter.SetControls(addressSettings);
        }

        /// <summary>
        /// Open a dialog to select Contact Attributes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectAttributes(object sender, EventArgs e)
        {
            // open the contact attributes dialog
            TFrmContactAttributesDialog ContactAttributesDialog = new TFrmContactAttributesDialog(FPetraUtilsObject.GetForm());

            ContactAttributesDialog.ContactID = -1;
            ContactAttributesDialog.SelectedContactAttributeTable = FAttributeTable;

            if (ContactAttributesDialog.ShowDialog() == DialogResult.OK)
            {
                PPartnerContactAttributeTable Changes = ContactAttributesDialog.SelectedContactAttributeTable.GetChangesTyped();

                // if changes were made
                if (Changes != null)
                {
                    FAttributeTable = ContactAttributesDialog.SelectedContactAttributeTable;

                    // we do not need the deleted rows
                    FAttributeTable.AcceptChanges();

                    ContactAttributesLogic.SetupContactAttributesGrid(ref grdSelectedAttributes, FAttributeTable, false);
                }
            }
        }

        // get selected Contact Attributes from the grid
        private void grdSelectedAttributes_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            String param_contact_attributes = string.Empty;

            if ((FAttributeTable != null) && (FAttributeTable.Rows.Count > 0))
            {
                // join Attribute Code and Attribute Detail Code
                foreach (PPartnerContactAttributeRow Row in FAttributeTable.Rows)
                {
                    param_contact_attributes += Row.ContactAttributeCode + ";" + Row.ContactAttrDetailCode + ",";
                }

                param_contact_attributes = param_contact_attributes.Substring(0, param_contact_attributes.Length - 1);
            }

            ACalc.AddParameter("param_contact_attributes", param_contact_attributes);
        }

        private void grdSelectedAttributes_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
        }

        private void grdSelectedAttributes_SetControls(TParameterList AParameters)
        {
        }
    }
}