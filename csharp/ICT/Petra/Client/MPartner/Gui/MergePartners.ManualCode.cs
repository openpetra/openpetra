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
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// manual methods for the generated window
    public partial class TFrmMergePartners
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

            // customise OK and Cancel buttons
            btnOK.Width = 61;
            btnOK.Text = "Merge";
            btnOK.Enabled = false;
            btnCancel.Location = new System.Drawing.Point(btnOK.Location.X + btnOK.Width + 10, 0);
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
                    new Thread(() => MergeTwoPartners(FromPartnerKey, ToPartnerKey, FromPartnerClass, ToPartnerClass, Categories));

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
                    throw new ApplicationException("Exception occured while calling OpenGetMergeDataDialog Delegate!", exp);
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
                    throw new ApplicationException("Exception occured while calling OpenGetMergeDataDialog Delegate!", exp);
                }
            }

            return false;
        }

        private static bool WebConnectorResult = true;

        private static void MergeTwoPartners(long AFromPartnerKey, long AToPartnerKey,
            TPartnerClass AFromPartnerClass, TPartnerClass AToPartnerClass, bool[] ACategories)
        {
            WebConnectorResult = TRemote.MPartner.Partner.WebConnectors.MergeTwoPartners(AFromPartnerKey, AToPartnerKey, AFromPartnerClass,
                AToPartnerClass, FSiteKeys, FLocationKeys, FMainBankingDetailsKey, ACategories);
        }

        private bool CheckPartnersCanBeMerged()
        {
            if (FromPartnerClass != ToPartnerClass)
            {
                // confirm that user wants to merge partners from different partner classes
                if (MessageBox.Show(String.Format(Catalog.GetString("Do you really want to merge a Partner of class {0} into a Partner of class {1}?"),
                            FromPartnerClass, ToPartnerClass), Catalog.GetString("Merge Partners"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return false;
                }

                // Family Partner cannot be merged into a different partner class if family members, donations or bank accounts exist for that partner
                if (FromPartnerClass == TPartnerClass.FAMILY)
                {
                    int FamilyMergeResult = TRemote.MPartner.Partner.WebConnectors.CanFamilyMergeIntoDifferentClass(FromPartnerKey);

                    if (FamilyMergeResult != 0)
                    {
                        string ErrorMessage = "";

                        if (FamilyMergeResult == 1)
                        {
                            ErrorMessage = Catalog.GetString(
                                "This Family record cannot be merged into a Partner with different class as Family members exist!");
                        }
                        else if (FamilyMergeResult == 2)
                        {
                            ErrorMessage = Catalog.GetString(
                                "This Family record cannot be merged into a Partner with different class as donations were received for it!");
                        }
                        else if (FamilyMergeResult == 3)
                        {
                            ErrorMessage = Catalog.GetString(
                                "This record cannot be merged into a Partner with different class as bank accounts exist for it!");
                        }

                        MessageBox.Show(ErrorMessage, Catalog.GetString("Merge Partners"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnOK.Enabled = false;
                        return false;
                    }
                }
            }
            else // partner classes are the same
            {
                string FromPartnerSupplierCurrency;
                string ToPartnerSupplierCurrency;

                // if two partners are suppliers they must have the same currency
                if ((TRemote.MPartner.Partner.WebConnectors.GetSupplierCurrency(FromPartnerKey, out FromPartnerSupplierCurrency) == true)
                    && (TRemote.MPartner.Partner.WebConnectors.GetSupplierCurrency(ToPartnerKey, out ToPartnerSupplierCurrency) == true))
                {
                    if (FromPartnerSupplierCurrency != ToPartnerSupplierCurrency)
                    {
                        MessageBox.Show(Catalog.GetString(
                                "These Partners cannot be merged. Partners that are suppliers must have the same currency in order to merge."),
                            Catalog.GetString("Merge Partners"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnOK.Enabled = false;
                        return false;
                    }
                }

                if (FromPartnerClass == TPartnerClass.VENUE)
                {
                    if (MessageBox.Show(Catalog.GetString("You are about to merge VENUEs. This will imply merging of buildings, rooms and room " +
                                "allocations defined for these Venues in the Conference Module!") + "\n\n" + Catalog.GetString("Continue?"),
                            Catalog.GetString("Merge Partners"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return false;
                    }
                }

                if (FromPartnerClass == TPartnerClass.BANK)
                {
                    if (MessageBox.Show(Catalog.GetString("You are about to merge BANKSs. This will imply that all bank accounts that were with the "
                                +
                                "From-Bank Partner will become bank accounts of the To-Bank Partner. For this reason you should merge Banks only when "
                                +
                                "both Bank Partners actually represented the same Bank, or if two different Banks have merged their operations!") +
                            "\n\n" +
                            Catalog.GetString("Continue?"), Catalog.GetString("Merge Partners"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                        == DialogResult.No)
                    {
                        return false;
                    }
                }
            }

            if (cmbReasonForMerging.Text == "Duplicate Record Exists")
            {
                if (FromPartnerClass == TPartnerClass.FAMILY)
                {
                    // FromPartnerClass and ToPartnerClass are the same
                    int CheckCommitmentsResult = TRemote.MPartner.Partner.WebConnectors.CheckPartnerCommitments(
                        FromPartnerKey, ToPartnerKey, FromPartnerClass);

                    // if the from family Partner contains a person with an ongoing commitment
                    if (CheckCommitmentsResult != 0)
                    {
                        string ErrorMessage = string.Format(Catalog.GetString("WARNING: You are about to change the family of {0} ({1}).") + "\n\n" +
                            Catalog.GetString("Changing a person's family can affect the person's ability to see their support information in" +
                                " Caleb including any support that they may receive from other Fields."), txtMergeFrom.LabelText, FromPartnerKey);

                        if (CheckCommitmentsResult == 1)
                        {
                            ErrorMessage += "\n\n" + string.Format(Catalog.GetString("It is STRONGLY recommended that you do not continue and " +
                                    "consider merging family {0} ({1}) into family {2} ({3})."),
                                txtMergeTo.LabelText, ToPartnerKey, txtMergeFrom.LabelText, FromPartnerKey);
                        }

                        ErrorMessage += "\n\n" + Catalog.GetString("Do you want to continue?");

                        if (MessageBox.Show(ErrorMessage, Catalog.GetString("Merge Partners"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                            == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }
                else if (FromPartnerClass == TPartnerClass.PERSON)
                {
                    // FromPartnerClass and ToPartnerClass are the same
                    int CheckCommitmentsResult = TRemote.MPartner.Partner.WebConnectors.CheckPartnerCommitments(
                        FromPartnerKey, ToPartnerKey, FromPartnerClass);

                    // if the from Partner has an ongoing commitment
                    if (CheckCommitmentsResult != 0)
                    {
                        string ErrorMessage = "";

                        if (CheckCommitmentsResult == 3)
                        {
                            ErrorMessage = string.Format(Catalog.GetString("WARNING: You are about to change the family of {0} ({1}).") + "\n\n" +
                                Catalog.GetString("Changing a person's family can affect the person's ability to see their support information in" +
                                    " Caleb including any support that they may receive from other Fields."), txtMergeFrom.LabelText, FromPartnerKey);
                        }
                        else if (CheckCommitmentsResult == 2)
                        {
                            ErrorMessage = Catalog.GetString("WARNING: Both Persons have a current commitment. " +
                                "Be aware that merging these Persons may affect their usage of Caleb.") +
                                           "\n\n" + Catalog.GetString("Do you want to continue?");
                        }
                        else if (CheckCommitmentsResult == 1)
                        {
                            ErrorMessage = string.Format(Catalog.GetString("WARNING: Person {0} ({1}) has a current commitment. " +
                                    "We strongly recommend merging the other way around."), txtMergeFrom.LabelText, FromPartnerKey) +
                                           "\n\n" + Catalog.GetString("Do you want to continue?");
                        }

                        if (MessageBox.Show(ErrorMessage, Catalog.GetString("Merge Partners"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                            == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }
            }

            // checks if one of the partners is a Foundation organisation.
            if ((FromPartnerClass == TPartnerClass.ORGANISATION) || (ToPartnerClass == TPartnerClass.ORGANISATION))
            {
                PFoundationTable FromFoundationTable = null;
                PFoundationTable ToFoundationTable = null;

                string ErrorMessage = "";

                if (FromPartnerClass == TPartnerClass.ORGANISATION)
                {
                    FromFoundationTable = TRemote.MPartner.Partner.WebConnectors.GetOrganisationFoundation(FromPartnerKey);
                }

                if (ToPartnerClass == TPartnerClass.ORGANISATION)
                {
                    ToFoundationTable = TRemote.MPartner.Partner.WebConnectors.GetOrganisationFoundation(ToPartnerKey);
                }

                // if both partners are Foundation organisations check permissions
                if ((FromFoundationTable != null) && (ToFoundationTable != null))
                {
                    if (!TSecurity.CheckFoundationSecurity((PFoundationRow)FromFoundationTable.Rows[0]))
                    {
                        ErrorMessage = Catalog.GetString("The Partner that you are merging from is a Foundation, but you do not " +
                            "have access rights to view its data. Therefore you are not allowed to merge these Foundations!") + "\n\n" +
                                       Catalog.GetString("Access Denied");
                    }
                    else if (!TSecurity.CheckFoundationSecurity((PFoundationRow)ToFoundationTable.Rows[0]))
                    {
                        ErrorMessage = Catalog.GetString("The Partner that you are merging into is a Foundation, but you do not " +
                            "have access rights to view its data. Therefore you are not allowed to merge these Foundations!") + "\n\n" +
                                       Catalog.GetString("Access Denied");
                    }
                }
                // none or both partners must be Foundation organisations
                else if (FromFoundationTable != null)
                {
                    ErrorMessage = Catalog.GetString("The Partner that you are merging from is a Foundation, but the Partner that you " +
                        "are merging into is not a Foundation. This is not allowed!") + "\n\n" +
                                   Catalog.GetString("Both Merge Partners Need to be Foundations!");
                }
                else if (ToFoundationTable != null)
                {
                    ErrorMessage = Catalog.GetString("The Partner that you are merging from isn't a Foundation, but the Partner that you " +
                        "are merging into is a Foundation. This is not allowed!") + "\n\n" +
                                   Catalog.GetString("Both Merge Partners Need to be Foundations!");
                }

                if (ErrorMessage != "")
                {
                    MessageBox.Show(ErrorMessage, Catalog.GetString("Merge Partners"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnOK.Enabled = false;
                    return false;
                }
            }

            return true;
        }
    }
}