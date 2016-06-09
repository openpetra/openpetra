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
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// manual methods for the generated window
    public partial class TFrmMergePartnersDialog
    {
        private long FFromPartnerKey = 0;
        private long FToPartnerKey = 0;
        private TPartnerClass FFromPartnerClass;
        private TPartnerClass FToPartnerClass;
        private static long[] FSiteKeys;
        private static int[] FLocationKeys;
        private static List <string[]>FContactDetails;
        private static int FMainBankingDetailsKey;

        /// Selected Addresses' SiteKeys
        public static long[] SiteKeys
        {
            set
            {
                FSiteKeys = value;
            }
        }

        /// Selected Addresses' LocationKeys
        public static int[] LocationKeys
        {
            set
            {
                FLocationKeys = value;
            }
        }

        /// Selected Contact Details
        public static List <string[]>ContactDetails
        {
            set
            {
                FContactDetails = value;
            }
        }

        /// Selected Main Bank Account's BankingDetailsKey
        public static int MainBankingDetailsKey
        {
            set
            {
                FMainBankingDetailsKey = value;
            }
        }

        /// <summary>
        /// PartnerKey of the 'From' Partner (available once the 'OK' Button has been clicked).
        /// </summary>
        public long FromPartnerKey
        {
            get
            {
                return FFromPartnerKey;
            }
            set
            {
                txtMergeFrom.Text = value.ToString();
            }
        }

        /// <summary>
        /// PartnerKey of the 'To' Partner (available once the 'OK' Button has been clicked).
        /// </summary>
        public long ToPartnerKey
        {
            get
            {
                return FToPartnerKey;
            }
        }

        private void InitializeManualCode()
        {
            txtMergeFrom.ShowLabel = true;
            txtMergeTo.ShowLabel = true;

            //txtMergeFrom.TextBoxPartEnabled = false;
            //txtMergeTo.TextBoxPartEnabled = false;

            btnOK.Enabled = false;
        }

        // partner to merge from is changed in txtMergeFrom
        private void MergeFromChanged(Int64 APartnerKey, String APartnerShortName, bool AValidSelection)
        {
            if (AValidSelection)
            {
                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(APartnerKey, out APartnerShortName, out FFromPartnerClass);

                string AllowedPartnerClasses = FFromPartnerClass.ToString();
                AllowedDifferentPartnerClasses(ref AllowedPartnerClasses, FFromPartnerClass);

                // restrict the choice of partner class for txtMergeTo
                txtMergeTo.PartnerClass = AllowedPartnerClasses;

                if ((txtMergeTo.Text != "0000000000") && (APartnerKey != Convert.ToInt64(txtMergeTo.Text)))
                {
                    btnOK.Enabled = true;
                }
                else
                {
                    btnOK.Enabled = false;
                }
            }
            else
            {
                txtMergeTo.PartnerClass = "";
            }
        }

        // partner to merge to is changed in txtMergeTo
        private void MergeToChanged(Int64 APartnerKey, String APartnerShortName, bool AValidSelection)
        {
            if (AValidSelection)
            {
                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(APartnerKey, out APartnerShortName, out FToPartnerClass);

                string AllowedPartnerClasses = FToPartnerClass.ToString();
                AllowedDifferentPartnerClasses(ref AllowedPartnerClasses, FToPartnerClass);

                // restrict the choice of partner class for txtMergeTo
                txtMergeFrom.PartnerClass = AllowedPartnerClasses;

                if ((txtMergeFrom.Text != "0000000000") && (Convert.ToInt64(txtMergeFrom.Text) != APartnerKey))
                {
                    btnOK.Enabled = true;
                }
                else
                {
                    btnOK.Enabled = false;
                }
            }
            else
            {
                txtMergeFrom.PartnerClass = "";
            }
        }

        // Check if selected partner class can be merged with any different partner classes
        private void AllowedDifferentPartnerClasses(ref string AAllowedPartnerClasses, TPartnerClass ASelectedPartnerClass)
        {
            if (ASelectedPartnerClass == TPartnerClass.ORGANISATION)
            {
                AAllowedPartnerClasses += "," + TPartnerClass.FAMILY;
            }
            else if (ASelectedPartnerClass == TPartnerClass.FAMILY)
            {
                AAllowedPartnerClasses += "," + TPartnerClass.ORGANISATION;
            }

            if (ASelectedPartnerClass == TPartnerClass.ORGANISATION)
            {
                AAllowedPartnerClasses += "," + TPartnerClass.CHURCH;
            }
            else if (ASelectedPartnerClass == TPartnerClass.CHURCH)
            {
                AAllowedPartnerClasses += "," + TPartnerClass.ORGANISATION;
            }

            if (ASelectedPartnerClass == TPartnerClass.FAMILY)
            {
                AAllowedPartnerClasses += "," + TPartnerClass.CHURCH;
            }
            else if (ASelectedPartnerClass == TPartnerClass.CHURCH)
            {
                AAllowedPartnerClasses += "," + TPartnerClass.FAMILY;
            }

            if (ASelectedPartnerClass == TPartnerClass.ORGANISATION)
            {
                AAllowedPartnerClasses += "," + TPartnerClass.BANK;
            }
            else if (ASelectedPartnerClass == TPartnerClass.BANK)
            {
                AAllowedPartnerClasses += "," + TPartnerClass.ORGANISATION;
            }
        }

        // switch partner keys
        private void Switch_Click(Object sender, EventArgs e)
        {
            string FromPartner = txtMergeFrom.Text;
            string ToPartner = txtMergeTo.Text;

            txtMergeFrom.Text = ToPartner;
            txtMergeTo.Text = FromPartner;
        }

        private void Clear_Click(Object sender, EventArgs e)
        {
            txtMergeFrom.Text = "";
            txtMergeTo.Text = "";
            txtMergeFrom.PartnerClass = "";
            txtMergeTo.PartnerClass = "";
            btnOK.Enabled = false;
        }

        // starts the merge process
        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            // Title for all message boxes
            string mergePartnersTitle = Catalog.GetString("Merge Partners");
            string mergeCancelledText = Catalog.GetString("Merge cancelled.");

            FFromPartnerKey = Convert.ToInt64(txtMergeFrom.Text);
            FToPartnerKey = Convert.ToInt64(txtMergeTo.Text);

            if (CheckPartnersCanBeMerged()
                && (MessageBox.Show(Catalog.GetString("WARNING: A Partner Merge operation cannot be undone and the From-Partner will be no longer " +
                            "accessible after the Partner Merge operation!") + "\n\n" + Catalog.GetString("Are you sure you want to continue?"),
                        mergePartnersTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes))
            {
                bool[] Categories = new bool[25];

                for (int i = 1; i < 25; i++)
                {
                    Categories[i] = true;
                }

                FSiteKeys = null;
                FLocationKeys = null;
                FContactDetails = null;
                FMainBankingDetailsKey = -1;
                TFrmExtendedMessageBox msgBox = null;
                string msg = string.Empty;
                bool DifferentFamilies = false;

                // open a dialog to select which From Partner's addresses should be merged
                if (GetSelectedAddresses() == false)
                {
                    MessageBox.Show(mergeCancelledText, mergePartnersTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // open a dialog to select which From Partner's contact details should be merged
                if (GetSelectedContactDetails() == false)
                {
                    MessageBox.Show(mergeCancelledText, mergePartnersTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // open a dialog to select which bank account should be set to MAIN (if necessary)
                if (GetMainBankAccount() == false)
                {
                    MessageBox.Show(mergeCancelledText, mergePartnersTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //
                if ((((FFromPartnerClass == TPartnerClass.FAMILY) && (FToPartnerClass == TPartnerClass.FAMILY))
                     || (FFromPartnerClass == TPartnerClass.PERSON))
                    && (GiftDestinationToMerge(out Categories[0]) == false))
                {
                    MessageBox.Show(mergeCancelledText, mergePartnersTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Thread t =
                    new Thread(() => MergeTwoPartners(Categories, ref DifferentFamilies));

                using (TProgressDialog dialog = new TProgressDialog(t))
                {
                    if ((dialog.ShowDialog() == DialogResult.Cancel) && (FWebConnectorResult == false))
                    {
                        MessageBox.Show(mergeCancelledText, mergePartnersTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (FWebConnectorResult == false)   // if merge is unsuccessful
                    {
                        msg = Catalog.GetString("The merge operation failed");

                        // Anything to display from the verification results?
                        if (FVerificationResultsOfMerge.Count > 0)
                        {
                            for (int i = 0; i < FVerificationResultsOfMerge.Count; i++)
                            {
                                if (FVerificationResultsOfMerge[i].ResultSeverity == TResultSeverity.Resv_Critical)
                                {
                                    msg += Environment.NewLine;
                                    msg += FVerificationResultsOfMerge[i].ResultText;
                                }
                            }

                            msg += Environment.NewLine;
                            msg += Catalog.GetString("More information is available in the Server.log file on the server at the date and time shown.");
                            msg += Environment.NewLine;
                            msg += Catalog.GetString("You can copy this message to the clipboard by clicking the button below.");
                        }

                        msgBox = new TFrmExtendedMessageBox(this);
                        msgBox.ShowDialog(msg, mergePartnersTitle, string.Empty,
                            TFrmExtendedMessageBox.TButtons.embbOK, TFrmExtendedMessageBox.TIcon.embiError);

                        dialog.Close();
                        return;
                    }
                }

                if (DifferentFamilies)
                {
                    MessageBox.Show(String.Format(Catalog.GetString("Partners were in different families.")) + "\n\n" +
                        Catalog.GetString("FAMILY relations of the From Partner are not taken over to the To Partner!") + "\n\n" +
                        Catalog.GetString("Please check the family relations of the To Partner after completion."),
                        mergePartnersTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                msg = String.Format(Catalog.GetString("Merge of Partner {0} ({1}) into {2} ({3}) completed successfully."),
                    txtMergeFrom.LabelText, FFromPartnerKey, txtMergeTo.LabelText, FToPartnerKey);

                if (FVerificationResultsOfMerge.Count > 0)
                {
                    msg += Environment.NewLine;

                    for (int i = 0; i < FVerificationResultsOfMerge.Count; i++)
                    {
                        msg += Environment.NewLine;
                        msg += FVerificationResultsOfMerge[i].ResultText;
                    }

                    msg += Environment.NewLine;
                }

                msg += Environment.NewLine;
                msg += Catalog.GetString("If necessary, edit the merged Partner to correct any information that may not have been " +
                    "merged and correct information that may have been overwritten.") + Environment.NewLine + Environment.NewLine;
                msg += Catalog.GetString("Tip: You can use the 'Work with Last Partner' command in the Partner module and the " +
                    "'Work with Last Person' command in the Personnel module to view and edit the merged Partner.") + Environment.NewLine +
                       Environment.NewLine;
                msg += Catalog.GetString("You can copy this message to the clipboard by clicking the button below.");

                msgBox = new TFrmExtendedMessageBox(this);
                msgBox.ShowDialog(msg, mergePartnersTitle, string.Empty, TFrmExtendedMessageBox.TButtons.embbOK,
                    TFrmExtendedMessageBox.TIcon.embiInformation);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        // open a dialog to select which From Partner's addresses should be merged
        private bool GetSelectedAddresses()
        {
            Form MainWindow = FPetraUtilsObject.GetCallerForm();

            // If the delegate is defined, the host form will launch a Modal screen for us
            if (TCommonScreensForwarding.OpenGetMergeDataDialog != null)
            {
                // delegate IS defined
                try
                {
                    return TCommonScreensForwarding.OpenGetMergeDataDialog.Invoke(FFromPartnerKey,
                        FToPartnerKey,
                        TMergeActionEnum.ADDRESS,
                        MainWindow);
                }
                catch (Exception exp)
                {
                    throw new EOPAppException("Exception occured while calling OpenGetMergeDataDialog Delegate!", exp);
                }
            }

            return false;
        }

        // open a dialog to select which From Partner's contact details should be merged
        private bool GetSelectedContactDetails()
        {
            Form MainWindow = FPetraUtilsObject.GetCallerForm();

            // If the delegate is defined, the host form will launch a Modal screen for us
            if (TCommonScreensForwarding.OpenGetMergeDataDialog != null)
            {
                // delegate IS defined
                try
                {
                    return TCommonScreensForwarding.OpenGetMergeDataDialog.Invoke(FFromPartnerKey,
                        FToPartnerKey,
                        TMergeActionEnum.CONTACTDETAIL,
                        MainWindow);
                }
                catch (Exception exp)
                {
                    throw new EOPAppException("Exception occured while calling OpenGetMergeDataDialog Delegate!", exp);
                }
            }

            return false;
        }

        // open a dialog to select which bank account should be set to MAIN (if necessary)
        private bool GetMainBankAccount()
        {
            Form MainWindow = FPetraUtilsObject.GetCallerForm();

            // run a check to determine if a new 'Main' account needs selected. If not then just return.
            if (TRemote.MPartner.Partner.WebConnectors.NeedMainBankAccount(FFromPartnerKey, FToPartnerKey) == false)
            {
                return true;
            }

            // If the delegate is defined, the host form will launch a Modal screen for us
            if (TCommonScreensForwarding.OpenGetMergeDataDialog != null)
            {
                // delegate IS defined
                try
                {
                    return TCommonScreensForwarding.OpenGetMergeDataDialog.Invoke(FFromPartnerKey,
                        FToPartnerKey,
                        TMergeActionEnum.BANKACCOUNT,
                        MainWindow);
                }
                catch (Exception exp)
                {
                    throw new EOPAppException("Exception occured while calling OpenGetMergeDataDialog Delegate!", exp);
                }
            }

            return false;
        }

        private bool GiftDestinationToMerge(out bool AMergeGiftDestination)
        {
            bool FromGiftDestinationNeedsEnded;
            bool ToGiftDestinationNeedsEnded;
            string Message = "";

            AMergeGiftDestination = false;

            // if no active Gift Destination then return true
            if (TRemote.MPartner.Partner.WebConnectors.ActiveGiftDestination(FFromPartnerKey, FToPartnerKey, FFromPartnerClass,
                    out FromGiftDestinationNeedsEnded, out ToGiftDestinationNeedsEnded) == false)
            {
                return true;
            }

            // ask permission to move Gift Destination
            if (FFromPartnerClass == TPartnerClass.PERSON)
            {
                Message = Catalog.GetString("A currently active Gift Destination exists for the 'From' Partner. " +
                    "Would you like to take this record over to the 'To' Partner?");
            }
            else if (FFromPartnerClass == TPartnerClass.FAMILY)
            {
                Message = Catalog.GetString("A currently active Gift Destination exists for the Family of the 'From' Partner. " +
                    "Would you like to take this record over to the Family of the 'To' Partner?");
            }

            // ask permission to modify expiry dates to allow move to happen
            if (FromGiftDestinationNeedsEnded)
            {
                Message += "\n\n" + Catalog.GetString(
                    "The Expiry Date of this record will need to be brought forward so it can fit in with a future Gift Destination for the 'To' Partner.");
            }

            if (ToGiftDestinationNeedsEnded)
            {
                Message += "\n\n" + Catalog.GetString(
                    "The 'To' Partner also has a currently active Gift Destination. This will need to be ended to allow this record to be moved over.");
            }

            DialogResult Result = MessageBox.Show(Message, Catalog.GetString(
                    "Gift Destination"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

            if (Result == DialogResult.Yes)
            {
                // permission to merge Gift Destination
                AMergeGiftDestination = true;
            }
            else if (Result == DialogResult.Cancel)
            {
                // cancel the entire merge
                return false;
            }

            return true;
        }

        private bool FWebConnectorResult = true;
        private TVerificationResultCollection FVerificationResultsOfMerge = null;

        private void MergeTwoPartners(bool[] ACategories, ref bool ADifferentFamilies)
        {
            FVerificationResultsOfMerge = new TVerificationResultCollection();
            FWebConnectorResult = TRemote.MPartner.Partner.WebConnectors.MergeTwoPartners(FFromPartnerKey, FToPartnerKey, FFromPartnerClass,
                FToPartnerClass, FSiteKeys, FLocationKeys, FContactDetails, FMainBankingDetailsKey, ACategories,
                ref ADifferentFamilies, ref FVerificationResultsOfMerge);
        }

        // check if the two partners can be merged and display any error/warning messages
        private bool CheckPartnersCanBeMerged()
        {
            TVerificationResultCollection VerificationResult;
            bool CanMerge;

            CanMerge = TRemote.MPartner.Partner.WebConnectors.CheckPartnersCanBeMerged(FFromPartnerKey,
                FToPartnerKey,
                FFromPartnerClass,
                FToPartnerClass,
                cmbReasonForMerging.Text,
                out VerificationResult);

            // No critical errors. Display any warning messages.
            if (CanMerge)
            {
                foreach (TVerificationResult Result in VerificationResult)
                {
                    if (MessageBox.Show(Result.ResultText, Catalog.GetString("Merge Partners"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                        == DialogResult.No)
                    {
                        return false;
                    }
                }
            }
            // Critical error
            else
            {
                MessageBox.Show(VerificationResult[0].ResultText, Catalog.GetString("Merge Partners"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                // disable 'Merge' button until a partner is changed
                btnOK.Enabled = false;
            }

            return CanMerge;
        }
    }
}