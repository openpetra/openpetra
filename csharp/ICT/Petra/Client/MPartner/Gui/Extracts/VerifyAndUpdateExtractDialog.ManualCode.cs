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
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    /// manual methods for the generated window
    public partial class TFrmVerifyAndUpdateExtractDialog : System.Windows.Forms.Form
    {
        /// <summary>
        /// set the initial value for extract name in the dialog
        /// </summary>
        /// <param name="AExtractNameAndCreator"></param>
        /// <param name="AExtractName"></param>
        /// <param name="AKeyCount"></param>
        public void SetExtractNameAndDetails(String AExtractNameAndCreator, String AExtractName, int AKeyCount)
        {
            lblExtractNameAndCreator.Text = Catalog.GetString("Extract Name: ") + AExtractNameAndCreator;
            lblExtractNameAndCreator.Font = new System.Drawing.Font(lblExtractNameAndCreator.Font.FontFamily.Name, 10, System.Drawing.FontStyle.Bold);
            lblAction1.Text = lblAction1.Text.Replace("ZZZ", AExtractName);
            lblAction2.Text = lblAction2.Text.Replace("ZZZ", AKeyCount.ToString());
        }

        private void InitializeManualCode()
        {
            // show this dialog in center of screen
            this.StartPosition = FormStartPosition.CenterScreen;
            lblAction1.Anchor |= AnchorStyles.Right;
            lblAction2.Anchor |= AnchorStyles.Right;
        }

        private void RunOnceOnActivationManual()
        {
            btnCancel.Focus();
        }

        private void CustomClosingHandler(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CanClose())
            {
                e.Cancel = true;
            }
            else
            {
                // Needs to be set to false because it got set to true in ancestor Form!
                e.Cancel = false;

                // Need to call the following method in the Base Form to remove this Form from the Open Forms List
                FPetraUtilsObject.TFrmPetra_Closing(this, null);
            }
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}