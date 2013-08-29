//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
    public partial class TFrmSendEmailOptions : System.Windows.Forms.Form
    {
        private void InitializeManualCode()
        {
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

        /// <summary>
        /// Attach the pdf file to the email
        /// </summary>
        public bool AttachPDF
        {
            get
            {
                return chkPDF.Checked;
            }
        }

        /// <summary>
        /// Attach the csv file to the email
        /// </summary>
        public bool AttachCSVFile
        {
            get
            {
                return chkCSVFile.Checked;
            }
        }

        /// <summary>
        /// Attach the excel file to the email
        /// </summary>
        public bool AttachExcelFile
        {
            get
            {
                return chkExcelFile.Checked;
            }
        }

        /// <summary>
        /// email addresses
        /// </summary>
        public string EmailAddresses
        {
            get
            {
                return txtEmail.Text;
            }
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            if (txtEmail.Text.Trim().Length == 0)
            {
                MessageBox.Show(Catalog.GetString("Please enter an Email Address"),
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