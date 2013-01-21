//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2011 by OM International
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

namespace Ict.Petra.Client.MPartner.Gui
{
    /// manual methods for the generated window
    public partial class TFrmSubscriptionsCancelAllDialog : System.Windows.Forms.Form
    {
        private DateTime FDateEndedPreset = DateTime.MinValue;

        /// <summary>
        /// Set this Property to a Date to preset the 'Date Ended' date in the Dialog to
        /// this date (instead of to today, to which it defaults).
        ///
        /// </summary>
        public DateTime DateEnded
        {
            get
            {
                return FDateEndedPreset;
            }

            set
            {
                FDateEndedPreset = value;
            }
        }

        private void InitializeManualCode()
        {
            DateTime DateEnded;

            lblExplanationText.Text = Catalog.GetString(
                "Select the 'Reason ended' and enter the 'Date ended'." + "\r\n" +
                "On clicking OK these will be applied to all active Subscriptions." + "\r\n" +
                "The Partner will be left with no active Subscriptions!");

            if (FDateEndedPreset == DateTime.MinValue)
            {
                DateEnded = DateTime.Today;
            }
            else
            {
                /* MessageBox.Show('FDateEndedPreset: ' + FDateEndedPreset.ToString); */
                DateEnded = FDateEndedPreset;

                /* MessageBox.Show('Presetting the Reason Ended to ''BADADDR'''); */
                /* FDateEnded was set via the DateEnded property, which happens if this */
                /* Dialog is called from the 'Deactivate Partner' process. In this case */
                /* the probability is high that this is done because mail was returned */
                /* undeliverable, so we preset the Reason Ended to 'BADADDR'. */
                cmbPSubscriptionReasonSubsCancelledCode.cmbCombobox.SelectedValue = MPartnerConstants.SUBSCRIPTIONS_REASON_ENDED_BADADDR;
            }

            dtpPSubscriptionDateCancelled.Date = DateEnded;

            // show this dialog in center of screen
            this.StartPosition = FormStartPosition.CenterScreen;
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
        ///
        /// </summary>
        /// <param name="AReasonEnded">Text that gives the reason for ending the Subscriptions</param>
        /// <param name="ADateEnded">Date when the Subscriptions should end (can be empty)</param>
        /// <returns>false if the Dialog is still uninitialised, otherwise true.
        /// </returns>
        public Boolean GetReturnedParameters(out String AReasonEnded, out DateTime ADateEnded)
        {
            Boolean ReturnValue = true;

            AReasonEnded = cmbPSubscriptionReasonSubsCancelledCode.GetSelectedString();
            ADateEnded = (DateTime)dtpPSubscriptionDateCancelled.Date;

            return ReturnValue;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}