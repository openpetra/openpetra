//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using GNU.Gettext;
using System.Security.Principal;
using Ict.Petra.Client.App.Core;
using System.Threading;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MCommon;

namespace Ict.Petra.Client.CommonDialogs
{
    /// <summary>
    /// dialog for watching the progress of a webconnector method, with the option to cancel the job
    /// </summary>
    public partial class TProgressDialog : System.Windows.Forms.Form
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TProgressDialog(Thread t) : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnCancel.Text = Catalog.GetString("Cancel");
            this.Text = Catalog.GetString("Progress Dialog");
            #endregion

            TRemote.MCommon.WebConnectors.Reset();
            t.Start();
            timer1.Start();
        }

        void BtnCancelClick(object sender, EventArgs e)
        {
            if (MessageBox.Show(Catalog.GetString("Do you really want to cancel?"),
                    Catalog.GetString("Confirm cancellation"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                TRemote.MCommon.WebConnectors.CancelJob();

                this.DialogResult = DialogResult.Cancel;

                ConfirmedClosing = true;
                Close();
            }
        }

        private bool ConfirmedClosing = false;

        void Timer1Tick(object sender, EventArgs e)
        {
            string caption;
            string message;
            int percentage;
            bool finished;

            if (TRemote.MCommon.WebConnectors.GetCurrentState(out caption,
                    out message,
                    out percentage,
                    out finished))
            {
                this.Text = caption;
                this.lblMessage.Text = message;
                this.progressBar.Value = percentage;

                if (finished)
                {
                    this.DialogResult = DialogResult.OK;
                    ConfirmedClosing = true;
                    Close();
                }
            }
        }

        void TProgressDialogFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ConfirmedClosing)
            {
                e.Cancel = true;
                BtnCancelClick(null, null);
            }
        }
    }
}