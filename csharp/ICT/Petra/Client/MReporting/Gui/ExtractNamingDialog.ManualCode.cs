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
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// manual methods for the generated window
    public partial class TFrmExtractNamingDialog : System.Windows.Forms.Form
    {
        private void InitializeManualCode()
        {
        }

        private void CustomClosingHandler(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CanClose())
            {
                // MessageBox.Show('TFrmReportingPeriodSelectionDialog.TFormPetra_Closing: e.Cancel := true');
                e.Cancel = true;
            }
            else
            {
                //TODO UnRegisterUIConnector();

                // Needs to be set to false because it got set to true in ancestor Form!
                e.Cancel = false;

                // Need to call the following method in the Base Form to remove this Form from the Open Forms List
                FPetraUtilsObject.TFrmPetra_Closing(this, null);
            }
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the values of Fields
        /// on the screen.
        /// </summary>
        /// <param name="AExtractName"></param>
        /// <param name="AExtractDescription"></param>
        /// <returns></returns>
        public Boolean GetReturnedParameters(out String AExtractName, out String AExtractDescription)
        {
            Boolean ReturnValue = true;

            AExtractName = txtExtractName.Text;
            AExtractDescription = txtDescription.Text;

            return ReturnValue;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            // extract name has to be set
            if (txtExtractName.Text.Trim().Length == 0)
            {
                MessageBox.Show(Catalog.GetString("An extract must have a name"),
                    Catalog.GetString("Invalid Data entered"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }

            // check if extract already exists
            if (TRemote.MPartner.Partner.WebConnectors.ExtractExists(txtExtractName.Text))
            {
                MessageBox.Show(Catalog.GetString("An extract with this name already exists. Please enter a new name."),
                    Catalog.GetString("Invalid Data entered"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}