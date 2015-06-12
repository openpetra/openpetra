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
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    /// manual methods for the generated window
    public partial class TFrmUpdateExtractAddContactLogDialog : System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetraEdit
    {
        private PartnerEditTDS FMainDS;

        /// <summary>
        /// set the initial value for passport name in the dialog
        /// </summary>
        /// <param name="AExtractName"></param>
        public void SetExtractName(String AExtractName)
        {
            lblExtractName.Text = Catalog.GetString("Extract Name: ") + AExtractName;
        }

        private void InitializeManualCode()
        {
            // show this dialog in center of screen
            this.StartPosition = FormStartPosition.CenterScreen;

            FMainDS = new PartnerEditTDS();

            // now add the one subscription row to the DS that we are working with
            PContactLogTable ContactLogTable = new PContactLogTable();
            FMainDS.Merge(ContactLogTable);
            PContactLogRow ContactLogRow = FMainDS.PContactLog.NewRowTyped(true);
            ContactLogRow.ContactCode = "";
            FMainDS.PContactLog.Rows.Add(ContactLogRow);

            ucoContactLog.MainDS = FMainDS;
            ucoContactLog.SpecialInitUserControl();
            ucoContactLog.ShowDetails(ContactLogRow);
            FPetraUtilsObject.HasChanges = false;
        }

        private void CustomClosingHandler(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Needs to be set to false because it got set to true in ancestor Form!
            e.Cancel = false;

            // Need to call the following method in the Base Form to remove this Form from the Open Forms List
            FPetraUtilsObject.HasChanges = false; // this has to be set as otherwise the following call won't work
            FPetraUtilsObject.TFrmPetra_Closing(this, null);
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the values of Fields
        /// on the screen.
        /// </summary>
        /// <param name="ARow"></param>
        /// <param name="ATable"></param>
        /// <returns>Boolean</returns>
        public bool GetReturnedParameters(ref PContactLogRow ARow, ref PPartnerContactAttributeTable ATable)
        {
            bool ReturnValue = true;

            DataUtilities.CopyAllColumnValues(FMainDS.PContactLog.Rows[0], ARow);
            ATable.Merge(FMainDS.PPartnerContactAttribute);

            return ReturnValue;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            if (ucoContactLog.ValidateAllData(TErrorProcessingMode.Epm_All, ucoContactLog))
            {
                if (MessageBox.Show(Catalog.GetString("Are you sure that you want to add the selected contact log" +
                            "\r\nfor all partners in the extract?"),
                        Catalog.GetString("Add Contact Log"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
        }

        /// <summary>
        /// save the changes on the screen
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            // method needs to be provided here for interface but will never be called
            return false;
        }

        /// <summary>
        /// Get the number of changed records and specify a message to incorporate into the 'Do you want to save?' message box
        /// </summary>
        /// <param name="AMessage">An optional message to display.  If the parameter is an empty string a default message will be used</param>
        /// <returns>The number of changed records.  Return -1 to imply 'unknown'.</returns>
        public int GetChangedRecordCount(out string AMessage)
        {
            AMessage = String.Empty;
            return -1;
        }
    }
}