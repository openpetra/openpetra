//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
    public partial class TFrmUpdateExtractAddSubscriptionDialog : System.Windows.Forms.Form
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
            PSubscriptionTable SubscriptionTable = new PSubscriptionTable();
            FMainDS.Merge(SubscriptionTable);
            PSubscriptionRow SubscriptionRow = FMainDS.PSubscription.NewRowTyped(true);
            SubscriptionRow.PublicationCode = ""; // avoid NOT NULL error message
            FMainDS.PSubscription.Rows.Add(SubscriptionRow);

            ucoSubscription.MainDS = FMainDS;
            ucoSubscription.SpecialInitUserControl();
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
        /// <returns>Boolean</returns>
        public Boolean GetReturnedParameters(ref PSubscriptionRow ARow)
        {
            Boolean ReturnValue = true;

            DataUtilities.CopyAllColumnValues(FMainDS.PSubscription.Rows[0], ARow);

            return ReturnValue;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            if (ucoSubscription.ValidateAllData(true, ucoSubscription))
            {
                if (MessageBox.Show(Catalog.GetString("Are you sure that you want to add the selected subscription" +
                            "\r\nfor all partners in the extract?"),
                        Catalog.GetString("Add Subscription"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
        }
    }

    // in addition derive class from IFrmPetraEdit so TFrmPetraEditUtils can be created
    public partial class TFrmUpdateExtractAddSubscriptionDialog : Ict.Petra.Client.CommonForms.IFrmPetraEdit
    {
        /// <summary>
        /// save the changes on the screen
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            // method needs to be provided here for interface but will never be called
            return false;
        }
    }
}