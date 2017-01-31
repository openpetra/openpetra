//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Jakob Englert
//
// Copyright 2004-2016 by OM International
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
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Common;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TFrmPartnerDeactivateDialog
    {
        private void InitializeManualCode()
        {
            lblTop.Font = new System.Drawing.Font(lblTop.Font, System.Drawing.FontStyle.Bold);
            lblTop2.Left -= 4;
            lblTop.Dock = DockStyle.Top;
            chkCancelAllSubscriptions.Checked = true;
            chkChangePartnerStatus.Checked = true;
            chkSetEndDateForAllCurrentAddresses.Checked = true;

            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            dtpValidTo.Date = DateTime.Today;

            chkChangePartnerStatus.CheckedChanged += new System.EventHandler(UpdateComboBox);
            chkSetEndDateForAllCurrentAddresses.CheckedChanged += new System.EventHandler(UpdateDateField);

            cmbNewPartnerStatus.Filter =
                " p_status_code_c NOT LIKE 'ACTIVE' AND p_status_code_c NOT LIKE 'MERGED' AND p_status_code_c NOT LIKE 'PRIVATE'";
            cmbNewPartnerStatus.SetSelectedString("INACTIVE");
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            if (chkChangePartnerStatus.Checked && (cmbNewPartnerStatus.GetSelectedString() == String.Empty))
            {
                MessageBox.Show("Please select a Partner Status.", "Validate Data");
                cmbNewPartnerStatus.Focus();
            }
            else if (chkSetEndDateForAllCurrentAddresses.Checked && (dtpValidTo.Text == String.Empty))
            {
                MessageBox.Show("Please enter a date.", "Validate Data");
                dtpValidTo.Focus();
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void UpdateComboBox(object sender, EventArgs ea)
        {
            CheckBox chk = sender as CheckBox;

            if (chk != null)
            {
                if (chk.Checked)
                {
                    cmbNewPartnerStatus.Enabled = true;
                }
                else
                {
                    cmbNewPartnerStatus.Enabled = false;
                }
            }
        }

        private void UpdateDateField(object sender, EventArgs ea)
        {
            CheckBox chk = sender as CheckBox;

            if (chk != null)
            {
                if (chk.Checked)
                {
                    dtpValidTo.Enabled = true;
                }
                else
                {
                    dtpValidTo.Enabled = false;
                    dtpValidTo.Date = DateTime.Today;
                }
            }
        }

        /// <summary>
        /// Gets all parameters of the dialog
        /// </summary>
        /// <param name="AChangePartnerStatus"></param>
        /// <param name="ANewPartnerStatusCode"></param>
        /// <param name="ACancelAllSubscriptions"></param>
        /// <param name="AExpireAllCurrentAddresses"></param>
        /// <param name="AValidTo"></param>
        public void GetReturnedParameters(out bool AChangePartnerStatus,
            out string ANewPartnerStatusCode,
            out bool ACancelAllSubscriptions,
            out bool AExpireAllCurrentAddresses,
            out DateTime AValidTo)
        {
            AChangePartnerStatus = chkChangePartnerStatus.Checked;
            ANewPartnerStatusCode = cmbNewPartnerStatus.GetSelectedString();
            ACancelAllSubscriptions = chkCancelAllSubscriptions.Checked;
            AExpireAllCurrentAddresses = chkSetEndDateForAllCurrentAddresses.Checked;
            AValidTo = dtpValidTo.Date.Value;
        }
    }
}