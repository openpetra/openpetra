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
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// manual methods for the generated window
    public partial class TFrmMergePartnersDialog
    {
        private long FromPartnerKey = 0;
        private long ToPartnerKey = 0;
        private TPartnerClass FromPartnerClass;
        private TPartnerClass ToPartnerClass;

        /// Selected Addresses' SiteKeys (set from webconnector)
        public static long[] FSiteKeys;

        /// Selected Addresses' LocationKeys (set from webconnector)
        public static int[] FLocationKeys;

        /// Selected Main Bank Account's BankingDetailsKey (set from webconnector)
        public static int FMainBankingDetailsKey;

        private void InitializeManualCode()
        {
            txtMergeFrom.ShowLabel = true;
            txtMergeTo.ShowLabel = true;

            txtMergeFrom.ReadOnly = true;
            txtMergeTo.ReadOnly = true;

            btnOK.Enabled = false;
        }

        private void MergeFromChanged(Int64 APartnerKey, String APartnerShortName, bool AValidSelection)
        {
            if (AValidSelection)
            {
                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(APartnerKey, out APartnerShortName, out FromPartnerClass);

                string AllowedPartnerClasses = FromPartnerClass.ToString();
                AllowedDifferentPartnerClasses(ref AllowedPartnerClasses, FromPartnerClass);

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
        }

        private void MergeToChanged(Int64 APartnerKey, String APartnerShortName, bool AValidSelection)
        {
            if (AValidSelection)
            {
                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(APartnerKey, out APartnerShortName, out ToPartnerClass);

                string AllowedPartnerClasses = ToPartnerClass.ToString();
                AllowedDifferentPartnerClasses(ref AllowedPartnerClasses, ToPartnerClass);

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
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            FromPartnerKey = Convert.ToInt64(txtMergeFrom.Text);
            ToPartnerKey = Convert.ToInt64(txtMergeTo.Text);

            if (CheckPartnersCanBeMerged()
                && (MessageBox.Show(Catalog.GetString("WARNING: A Partner Merge operation cannot be undone and the From-Partner will be no longer " +
                            "accessible after the Partner Merge operation!") + "\n\n" + Catalog.GetString("Are you sure you want to continue?"),
                        Catalog.GetString("Merge Partners"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes))
            {
                bool[] Categories = new bool[20];

                for (int i = 0; i < 20; i++)
                {
                    Categories[i] = true;
                }

                FSiteKeys = null;
                FLocationKeys = null;
                FMainBankingDetailsKey = -1;
                bool DifferentFamilies = false;

                // open a dialog to select which To Partner's addresses should be merged
                if (GetSelectedAddresses() == false)
                {
                    MessageBox.Show(Catalog.GetString("Merge cancelled."));
                    return;
                }

                // open a dialog to select which bank account should be set to MAIN (if necessary)
                if (GetMainBankAccount() == false)
                {
                    MessageBox.Show(Catalog.GetString("Merge cancelled."));
                    return;
                }

                Thread t =
                    new Thread(() => MergeTwoPartners(FromPartnerKey, ToPartnerKey, FromPartnerClass, ToPartnerClass, Categories,
                            ref DifferentFamilies));

                using (TProgressDialog dialog = new TProgressDialog(t))
                {
                    if ((dialog.ShowDialog() == DialogResult.Cancel) && (WebConnectorResult == true))
                    {
                        MessageBox.Show(Catalog.GetString("Merge cancelled."));
                        return;
                    }
                    else if (!WebConnectorResult)   // if merge is unsuccessful
                    {
                        MessageBox.Show(Catalog.GetString("Merge failed. Please check the Server.log file on the server"));
                        dialog.Close();
                        return;
                    }
                }

                if (DifferentFamilies)
                {
                    MessageBox.Show(String.Format(Catalog.GetString("Partners were in different families.")) + "\n\n" +
                        Catalog.GetString("FAMILY relations of the From Partner are not taken over to the To Partner!") + "\n\n" +
                        Catalog.GetString("Please check the family relations of the To Partner after completion."),
                        Catalog.GetString("Merge Partners"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                MessageBox.Show(String.Format(Catalog.GetString("Merge of Partner {0} into {1} complete."), FromPartnerKey, ToPartnerKey) +
                    "\n\n" + Catalog.GetString("If necessary edit the merged Partner to correct any information that may not have been " +
                        "merged and correct information that may have been overwritten.") + "\n\n" +
                    Catalog.GetString("Tip: You can use the 'Work with Last Partner' command in the Partner module and the " +
                        "'Work with Last Person' command in the Personnel module to view and edit the merged Partner."),
                    Catalog.GetString("Merge Partners"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        // open a dialog to select which To Partner's addresses should be merged
        private bool GetSelectedAddresses()
        {
            Form MainWindow = FPetraUtilsObject.GetCallerForm();

            // If the delegate is defined, the host form will launch a Modal screen for us
            if (TCommonScreensForwarding.OpenGetMergeDataDialog != null)
            {
                // delegate IS defined
                try
                {
                    string DataType = "ADDRESS";
                    return TCommonScreensForwarding.OpenGetMergeDataDialog.Invoke(FromPartnerKey, ToPartnerKey, DataType, MainWindow);
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
            if (TRemote.MPartner.Partner.WebConnectors.NeedMainBankAccount(FromPartnerKey, ToPartnerKey) == false)
            {
                return true;
            }

            // If the delegate is defined, the host form will launch a Modal screen for us
            if (TCommonScreensForwarding.OpenGetMergeDataDialog != null)
            {
                // delegate IS defined
                try
                {
                    string DataType = "BANKACCOUNT";
                    return TCommonScreensForwarding.OpenGetMergeDataDialog.Invoke(FromPartnerKey, ToPartnerKey, DataType, MainWindow);
                }
                catch (Exception exp)
                {
                    throw new EOPAppException("Exception occured while calling OpenGetMergeDataDialog Delegate!", exp);
                }
            }

            return false;
        }

        private static bool WebConnectorResult = true;

        private static void MergeTwoPartners(long AFromPartnerKey, long AToPartnerKey,
            TPartnerClass AFromPartnerClass, TPartnerClass AToPartnerClass, bool[] ACategories, ref bool ADifferentFamilies)
        {
            WebConnectorResult = TRemote.MPartner.Partner.WebConnectors.MergeTwoPartners(AFromPartnerKey, AToPartnerKey, AFromPartnerClass,
                AToPartnerClass, FSiteKeys, FLocationKeys, FMainBankingDetailsKey, ACategories, ref ADifferentFamilies);
        }

        // check if the two partners can be merged and display any error/warning messages
        private bool CheckPartnersCanBeMerged()
        {
            TVerificationResultCollection VerificationResult;
            bool CanMerge;

            CanMerge = TRemote.MPartner.Partner.WebConnectors.CheckPartnersCanBeMerged(FromPartnerKey, ToPartnerKey, FromPartnerClass, ToPartnerClass,
                cmbReasonForMerging.Text, out VerificationResult);

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