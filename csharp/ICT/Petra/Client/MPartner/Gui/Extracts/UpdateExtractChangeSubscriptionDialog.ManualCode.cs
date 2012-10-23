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
using System.Collections.Generic;
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
    public partial class TFrmUpdateExtractChangeSubscriptionDialog : System.Windows.Forms.Form
    {
        private PartnerEditTDS FMainDS;
        private System.Drawing.Color ChangeControlBackgroundColor = System.Drawing.Color.Yellow;

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

            FPetraUtilsObject.HasChanges = false;

            // initialize all check box sections so fields are disabled
            OnTickChangeItem(chkChangeSubscriptionStatus, null);
            OnTickChangeItem(chkChangeGratisSubscription, null);
            
            OnTickChangeItem(chkChangeNumberComplimentary, null);
            OnTickChangeItem(chkChangePublicationCopies, null);
            OnTickChangeItem(chkChangeReasonSubsGivenCode, null);
            OnTickChangeItem(chkChangeReasonSubsCancelledCode, null);
            OnTickChangeItem(chkChangeGiftFromKey, null);

            OnTickChangeItem(chkChangeStartDate, null);
            OnTickChangeItem(chkChangeExpiryDate, null);
            OnTickChangeItem(chkChangeRenewalDate, null);
            OnTickChangeItem(chkChangeDateNoticeSent, null);
            OnTickChangeItem(chkChangeDateCancelled, null);

            OnTickChangeItem(chkChangeNumberIssuesReceived, null);
            OnTickChangeItem(chkChangeFirstIssue, null);
            OnTickChangeItem(chkChangeLastIssue, null);
        }

        private void OnTickChangeItem(System.Object sender, EventArgs e)
        {
            CheckBox CheckBoxSender;
            Control ChangeControl = null;
            
            // if check box is unticked then disable field, reset value and background colour, 
            // otherwise enable field and emphasize background colour
            
            if (sender == null)
            {
                return;
            }
                
            CheckBoxSender = (CheckBox)sender;
            
            if (sender == chkChangeSubscriptionStatus)
            {
                ChangeControl = cmbPSubscriptionSubscriptionStatus;
                if (!CheckBoxSender.Checked)
                {
                    cmbPSubscriptionSubscriptionStatus.SetSelectedString("", -1);
                }
                cmbPSubscriptionSubscriptionStatus.Enabled = CheckBoxSender.Checked;
            }
            else if (sender == chkChangeGratisSubscription)
            {
                ChangeControl = chkPSubscriptionGratisSubscription;
                if (!CheckBoxSender.Checked)
                {
                    chkPSubscriptionGratisSubscription.Checked = false;
                }
                chkPSubscriptionGratisSubscription.Enabled = CheckBoxSender.Checked;
            }

            else if (sender == chkChangeNumberComplimentary)
            {
                ChangeControl = txtPSubscriptionNumberComplimentary;
                if (!CheckBoxSender.Checked)
                {
                    txtPSubscriptionNumberComplimentary.NumberValueInt = null;
                }
                txtPSubscriptionNumberComplimentary.Enabled = CheckBoxSender.Checked;
            }
            else if (sender == chkChangePublicationCopies)
            {
                ChangeControl = txtPSubscriptionPublicationCopies;
                if (!CheckBoxSender.Checked)
                {
                    txtPSubscriptionPublicationCopies.NumberValueInt = null;
                }
                txtPSubscriptionPublicationCopies.Enabled = CheckBoxSender.Checked;
            }
            else if (sender == chkChangeReasonSubsGivenCode)
            {
                ChangeControl = cmbPSubscriptionReasonSubsGivenCode.cmbCombobox;
                if (!CheckBoxSender.Checked)
                {
                    cmbPSubscriptionReasonSubsGivenCode.SetSelectedString("", -1);
                }
                cmbPSubscriptionReasonSubsGivenCode.Enabled = CheckBoxSender.Checked;
            }
            else if (sender == chkChangeReasonSubsCancelledCode)
            {
                ChangeControl = cmbPSubscriptionReasonSubsCancelledCode.cmbCombobox;
                if (!CheckBoxSender.Checked)
                {
                    cmbPSubscriptionReasonSubsCancelledCode.SetSelectedString("", -1);
                }
                cmbPSubscriptionReasonSubsCancelledCode.Enabled = CheckBoxSender.Checked;
            }
            else if (sender == chkChangeGiftFromKey)
            {
                ChangeControl = txtPSubscriptionGiftFromKey;
                if (!CheckBoxSender.Checked)
                {
                    txtPSubscriptionGiftFromKey.Text = "0000000000";
                }
                txtPSubscriptionGiftFromKey.Enabled = CheckBoxSender.Checked;
            }

            else if (sender == chkChangeStartDate)
            {
                ChangeControl = dtpPSubscriptionStartDate;
                if (!CheckBoxSender.Checked)
                {
                    dtpPSubscriptionStartDate.Clear();
                }
                dtpPSubscriptionStartDate.Enabled = CheckBoxSender.Checked;
            }
            else if (sender == chkChangeExpiryDate)
            {
                ChangeControl = dtpPSubscriptionExpiryDate;
                if (!CheckBoxSender.Checked)
                {
                    dtpPSubscriptionExpiryDate.Clear();
                }
                dtpPSubscriptionExpiryDate.Enabled = CheckBoxSender.Checked;
            }
            else if (sender == chkChangeRenewalDate)
            {
                ChangeControl = dtpPSubscriptionSubscriptionRenewalDate;
                if (!CheckBoxSender.Checked)
                {
                    dtpPSubscriptionSubscriptionRenewalDate.Clear();
                }
                dtpPSubscriptionSubscriptionRenewalDate.Enabled = CheckBoxSender.Checked;
            }
            else if (sender == chkChangeDateNoticeSent)
            {
                ChangeControl = dtpPSubscriptionDateNoticeSent;
                if (!CheckBoxSender.Checked)
                {
                    dtpPSubscriptionDateNoticeSent.Clear();
                }
                dtpPSubscriptionDateNoticeSent.Enabled = CheckBoxSender.Checked;
            }
            else if (sender == chkChangeDateCancelled)
            {
                ChangeControl = dtpPSubscriptionDateCancelled;
                if (!CheckBoxSender.Checked)
                {
                    dtpPSubscriptionDateCancelled.Clear();
                }
                dtpPSubscriptionDateCancelled.Enabled = CheckBoxSender.Checked;
            }

            else if (sender == chkChangeNumberIssuesReceived)
            {
                ChangeControl = txtPSubscriptionNumberIssuesReceived;
                if (!CheckBoxSender.Checked)
                {
                    txtPSubscriptionNumberIssuesReceived.NumberValueInt = null;
                }
                txtPSubscriptionNumberIssuesReceived.Enabled = CheckBoxSender.Checked;
            }
            else if (sender == chkChangeFirstIssue)
            {
                ChangeControl = dtpPSubscriptionFirstIssue;
                if (!CheckBoxSender.Checked)
                {
                    dtpPSubscriptionFirstIssue.Clear();
                }
                dtpPSubscriptionFirstIssue.Enabled = CheckBoxSender.Checked;
            }
            else if (sender == chkChangeLastIssue)
            {
                ChangeControl = dtpPSubscriptionLastIssue;
                if (!CheckBoxSender.Checked)
                {
                    dtpPSubscriptionLastIssue.Clear();
                }
                dtpPSubscriptionLastIssue.Enabled = CheckBoxSender.Checked;
            }
            
            // now change background colour of selected field
            if (ChangeControl != null)
            {
                if (!CheckBoxSender.Checked)
                {
                    ChangeControl.ResetBackColor();
                }
                else
                {
                    ChangeControl.BackColor = ChangeControlBackgroundColor;   
                }
            }
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
        /// <param name="AFieldsToChange"></param>
        /// <returns>Boolean</returns>
        public Boolean GetReturnedParameters(ref PSubscriptionRow ARow, ref List<String> AFieldsToChange)
        {
            Boolean ReturnValue = true;

            ARow.InitValues();
            AFieldsToChange.Clear();
            
            // publication code needs to be set, otherwise change can not be performed
            if (cmbPSubscriptionPublicationCode.GetSelectedString() == "")
            {
                return false;
            }

            ARow.PublicationCode = cmbPSubscriptionPublicationCode.GetSelectedString();            
                
            if (chkChangeSubscriptionStatus.Checked)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetSubscriptionStatusDBName());
                ARow.SubscriptionStatus = cmbPSubscriptionSubscriptionStatus.Text;
            }
            if (chkChangeGratisSubscription.Checked)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetGratisSubscriptionDBName());
                ARow.GratisSubscription = chkPSubscriptionGratisSubscription.Checked;
            }

            if (   chkChangeNumberComplimentary.Checked
                && txtPSubscriptionNumberComplimentary.NumberValueInt.HasValue)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetNumberComplimentaryDBName());
                ARow.NumberComplimentary = txtPSubscriptionNumberComplimentary.NumberValueInt.Value;
            }
            if (   chkChangePublicationCopies.Checked
                && txtPSubscriptionPublicationCopies.NumberValueInt.HasValue)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetPublicationCopiesDBName());
                ARow.PublicationCopies = txtPSubscriptionPublicationCopies.NumberValueInt.Value;
            }
            if (chkChangeReasonSubsGivenCode.Checked)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetReasonSubsGivenCodeDBName());
                ARow.ReasonSubsGivenCode = cmbPSubscriptionReasonSubsGivenCode.GetSelectedString();
            }
            if (chkChangeReasonSubsCancelledCode.Checked)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetReasonSubsCancelledCodeDBName());
                ARow.ReasonSubsCancelledCode = cmbPSubscriptionReasonSubsCancelledCode.GetSelectedString();
            }
            if (chkChangeGiftFromKey.Checked)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetGiftFromKeyDBName());
                ARow.GiftFromKey = Convert.ToInt64(txtPSubscriptionGiftFromKey.Text);
            }

            if (   chkChangeStartDate.Checked
                && dtpPSubscriptionStartDate.Date.HasValue)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetStartDateDBName());
                ARow.StartDate = dtpPSubscriptionStartDate.Date.Value;
            }
            if (chkChangeExpiryDate.Checked)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetExpiryDateDBName());
                ARow.ExpiryDate = dtpPSubscriptionExpiryDate.Date;
            }
            if (chkChangeRenewalDate.Checked)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetSubscriptionRenewalDateDBName());
                ARow.SubscriptionRenewalDate = dtpPSubscriptionSubscriptionRenewalDate.Date;
            }
            if (chkChangeDateNoticeSent.Checked)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetDateNoticeSentDBName());
                ARow.DateNoticeSent = dtpPSubscriptionDateNoticeSent.Date;
            }
            if (chkChangeDateCancelled.Checked)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetDateCancelledDBName());
                ARow.DateCancelled = dtpPSubscriptionDateCancelled.Date;
            }

            if (   chkChangeNumberIssuesReceived.Checked
                && txtPSubscriptionNumberIssuesReceived.NumberValueInt.HasValue)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetNumberIssuesReceivedDBName());
                ARow.NumberIssuesReceived = txtPSubscriptionNumberIssuesReceived.NumberValueInt.Value;
            }
            if (chkChangeFirstIssue.Checked)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetFirstIssueDBName());
                ARow.FirstIssue = dtpPSubscriptionFirstIssue.Date;
            }
            if (chkChangeLastIssue.Checked)
            {
                AFieldsToChange.Add(PSubscriptionTable.GetLastIssueDBName());
                ARow.LastIssue = dtpPSubscriptionLastIssue.Date;
            }
            
            return ReturnValue;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            // publication code needs to be set, otherwise change can not be performed
            if (cmbPSubscriptionPublicationCode.GetSelectedString() == "")
            {
                MessageBox.Show(Catalog.GetString("Please select a publication"),
                        Catalog.GetString("Change Subscription"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show(Catalog.GetString("Are you sure that you want to change the selected subscription" +
                        "\r\nfor all partners in the extract?"),
                    Catalog.GetString("Change Subscription"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

    }

    // in addition derive class from IFrmPetraEdit so TFrmPetraEditUtils can be created
    public partial class TFrmUpdateExtractChangeSubscriptionDialog : Ict.Petra.Client.CommonForms.IFrmPetraEdit
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