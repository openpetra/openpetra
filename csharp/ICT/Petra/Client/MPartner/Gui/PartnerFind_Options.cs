//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// Partner Find screen Options dialog:
    ///   - allows dynamic adding/removing of search criteria fields such as
    ///       County/Province, Previous Family Name, etc in the 'Options' dialog
    ///   - allows dynamic rearranging of search criteria fiels to the users' liking.
    ///   - settings are retained in User Defaults.
    ///
    /// @Comment Should probably become a sub-screen of a much bigger 'User
    ///          Preferences' or 'Options' dialog.
    public partial class TFindOptionsForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// the default fields for the left side
        /// </summary>
        public const String PARTNER_FINDOPTIONS_CRITERIAFIELDSLEFT_DEFAULT =
            "PartnerName;PersonalName;Address1;City;PostCode;Country;MailingAddressOnly;";

        /// <summary>
        /// the default fields for the right side
        /// </summary>
        public const String PARTNER_FINDOPTIONS_CRITERIAFIELDSRIGHT_DEFAULT =
            "PartnerClass;PartnerKey;OMSSKey;PartnerStatus";

        /// <summary>
        /// the default fields for the left side when searching by bank details
        /// </summary>
        public const String PARTNER_FINDOPTIONSBYBANKDETAILS_CRITERIAFIELDSLEFT_DEFAULT =
            "PartnerName;AccountName;AccountNumber;Iban;BranchCode;Bic";

        /// <summary>
        /// the default fields for the right side when searching by bank details
        /// </summary>
        public const String PARTNER_FINDOPTIONSBYBANKDETAILS_CRITERIAFIELDSRIGHT_DEFAULT =
            "PartnerClass;OMSSKey;PartnerStatus";
#if TODO
        #region Resourcestrings

        private static readonly string StrMoreOptions = Catalog.GetString("&More >>");
        private static readonly string StrLessOptions = Catalog.GetString("<< &Less");

        #endregion

        /// <summary>Private Declarations</summary>
        private bool FSaveChangedOptions;
        private Boolean FCriteriaSetup;
        private Boolean FFindCriteriaListsSetupRunning;
        private ArrayList FCriteriaFieldsLeft;
        private ArrayList FCriteriaFieldsRight;
        private ArrayList FCriteriaFieldsLeftDefaultOrder;
        private ArrayList FCriteriaFieldsRightDefaultOrder;
        private String FLastSelectedCriteria;

        public TFindOptionsForm() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnMore.Text = Catalog.GetString("&More >>");
            this.btnMoveUp.Text = Catalog.GetString("Move &Up");
            this.grpMoreOptions.Text = Catalog.GetString("Rearrange Find Criteria");
            this.btnShowRightListItems.Text = Catalog.GetString("R.List Items");
            this.btnShowLeftListItems.Text = Catalog.GetString("L.List Items");
            this.btnMoveDown.Text = Catalog.GetString("Move &Down");
            this.btnMoveToLeftColumn.Text = Catalog.GetString("Move To &Left");
            this.btnMoveToRightColumn.Text = Catalog.GetString("Move To &Right");
            this.grpDisplayedSearchCriteria.Text = Catalog.GetString("Displayed Find Criteria");
            this.Label1.Text = Catalog.GetString("&Partner Find Criteria") + ":";
            this.Label2.Text = Catalog.GetString("&Address Find Criteria") + ":";
            this.Label3.Text = Catalog.GetString("O&ther Find Criteria") + ":";
            this.chkShowMatchButtons.Text = Catalog.GetString("Sho&w \'Matching Pattern\' buttons");
            this.chkExactPartnerKeyMatchSearch.Text = Catalog.GetString("Exact Partner &Key Match");
            this.btnReset.Text = Catalog.GetString("&Reset");
            this.Text = Catalog.GetString("Find Options");
            #endregion

            grpMoreOptions.Visible = false;
        }

        private void ChkHideMatchButtons_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            // if the advanced options panel is visible, then refresh the display
            if (grpMoreOptions.Visible)
            {
                ucoPartnerFindCriteriaSetup.DisplayCriteriaFieldControls();
            }
        }

        private void BtnShowRightListItems_Click(System.Object sender, System.EventArgs e)
        {
            IEnumerator ListEnumerator;
            String ListContents = "";

            ListEnumerator = ucoPartnerFindCriteriaSetup.CriteriaFieldsRight.GetEnumerator();

            while (ListEnumerator.MoveNext())
            {
                ListContents = ListContents + ListEnumerator.Current.ToString() + Environment.NewLine;
            }

            MessageBox.Show(ListContents);
        }

        private void BtnShowLeftListItems_Click(System.Object sender, System.EventArgs e)
        {
            IEnumerator ListEnumerator;
            String ListContents = "";

            ListEnumerator = ucoPartnerFindCriteriaSetup.CriteriaFieldsLeft.GetEnumerator();

            while (ListEnumerator.MoveNext())
            {
                ListContents = ListContents + ListEnumerator.Current.ToString() + Environment.NewLine;
            }

            MessageBox.Show(ListContents);
        }

        private void BtnMoveToLeftColumn_Click(System.Object sender, System.EventArgs e)
        {
            ucoPartnerFindCriteriaSetup.MoveLeftSelectedCriteriaPanel();
        }

        private void BtnMoveToRightColumn_Click(System.Object sender, System.EventArgs e)
        {
            ucoPartnerFindCriteriaSetup.MoveRightSelectedCriteriaPanel();
        }

        private void BtnMoveDown_Click(System.Object sender, System.EventArgs e)
        {
            ucoPartnerFindCriteriaSetup.MoveDownSelectedCriteriaPanel();
        }

        private void BtnMoveUp_Click(System.Object sender, System.EventArgs e)
        {
            ucoPartnerFindCriteriaSetup.MoveUpSelectedCriteriaPanel();
        }

        private void BtnFindCriteriaHelpClick(object sender, EventArgs e)
        {
            TBalloonTip BalloonTip = new TBalloonTip();

            BalloonTip.ShowBalloonTipHelp(
                "Find Criteria",
                "* Individual Find Critiera Fields can be shown or hidden as needed by checking or\r\n" +
                "  unchecking the box next to them." + "\r\n" +
                "* The 'Exact Partner Key Match' option determines whether an entered Partner Key is\r\n" +
                "  taken exactly as it is, or whether trailing zeroes ( 0 ) are treated as wildcards.\r\n" +
                "* The 'Show \"Matching Pattern\"' option shows or hides the Criteria Match Buttons for\r\n" +
                "  Find Criteria that consist of characters.",
                btnFindCriteriaHelp);
        }

        private void BtnRearrangeCriteriaHelpClick(object sender, EventArgs e)
        {
            TBalloonTip BalloonTip = new TBalloonTip();

            BalloonTip.ShowBalloonTipHelp(
                "Rearrange Find Criteria",
                "* Individual Find Critiera Fields can be moved as needed by using the four 'Move' buttons.\r\n" +
                "  This allows for arranging the Find Critiera Fields on screen exactly as you prefer.\r\n" +
                "* Click on a label to the left of a Find Criteria Field to start.",
                btnRearrangeCriteriaHelp);
        }

        private void ClbOtherFindCriteria_ItemCheck(System.Object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            HandleItemCheckEvent(sender, e);
        }

        private void ClbAddressFindCriteria_ItemCheck(System.Object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            HandleItemCheckEvent(sender, e);
        }

        private void ClbPartnerFindCriteria_ItemCheck(System.Object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            HandleItemCheckEvent(sender, e);
        }

        private void BtnHelpDisplayedFields_Click(System.Object sender, System.EventArgs e)
        {
            /* XPBalloonTip1.Show(btnHelpDisplayedFields, 'This allows you to select criteria fields that should appear in the Find Screen.' + Environment.NewLine + 'Those who have a check mark next to it are / will be available in the Find screen,
             *the others not.'); */
        }

        private void BtnHelpRearrange_Click(System.Object sender, System.EventArgs e)
        {
            /* XPBalloonTip1.Show(btnHelpRearrange, 'Select a criteria field by clicking on its name and then use the buttons below to move it to where you want it to go.' + Environment.NewLine + 'Note: To add a criteria field, place a check mark
             *next to it in the upper screen area, to remove it, uncheck in the upper screen area.'); */
        }

        private void BtnMore_Click(System.Object sender, System.EventArgs e)
        {
            if (!grpMoreOptions.Visible)
            {
                if (!FCriteriaSetup)
                {
                    ucoPartnerFindCriteriaSetup.ResetSearchCriteriaValuesToDefault();
                    ucoPartnerFindCriteriaSetup.InitUserControl();
                    ucoPartnerFindCriteriaSetup.CriteriaSetupMode = true;
                    ucoPartnerFindCriteriaSetup.CriteriaFieldsLeft = FCriteriaFieldsLeft;
                    ucoPartnerFindCriteriaSetup.CriteriaFieldsRight = FCriteriaFieldsRight;
                    ucoPartnerFindCriteriaSetup.InitialiseCriteriaFields();

                    // on options page, ensure match buttons are disabled
                    ucoPartnerFindCriteriaSetup.SetMatchButtonFunctionality(false);
                    FCriteriaSetup = true;
                }

                btnMore.Text = StrLessOptions;
                grpMoreOptions.Visible = true;

                // show hide the criteria fields
                ucoPartnerFindCriteriaSetup.DisplayCriteriaFieldControls();
                this.Height = grpDisplayedSearchCriteria.Height + grpMoreOptions.Height +
                              pnlBtnOKCancelHelpLayout.Height + stbMain.Height + 54;
            }
            else
            {
                btnMore.Text = StrMoreOptions;
                grpMoreOptions.Visible = false;
                this.Height = grpDisplayedSearchCriteria.Height +
                              pnlBtnOKCancelHelpLayout.Height + stbMain.Height + 22;

                // show hide the criteria fields
                ucoPartnerFindCriteriaSetup.DisplayCriteriaFieldControls();
            }
        }

        private new void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            FSaveChangedOptions = true;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void TFindOptionsForm_Closing(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (FSaveChangedOptions == true)
            {
                TUserDefaults.SetDefault(TUserDefaults.PARTNER_FINDOPTIONS_CRITERIAFIELDSLEFT, MakeCriteriaListString(FCriteriaFieldsLeft));
                TUserDefaults.SetDefault(TUserDefaults.PARTNER_FINDOPTIONS_CRITERIAFIELDSRIGHT, MakeCriteriaListString(FCriteriaFieldsRight));

                TUserDefaults.SetDefault(TUserDefaults.PARTNER_FINDOPTIONS_EXACTPARTNERKEYMATCHSEARCH, chkExactPartnerKeyMatchSearch.Checked);
                TUserDefaults.SetDefault(TUserDefaults.PARTNER_FINDOPTIONS_SHOWMATCHBUTTONS, chkShowMatchButtons.Checked);
            }
        }

        private void TFindOptionsForm_Load(System.Object sender, System.EventArgs e)
        {
            // Form initially shows itself collapsed
            this.Height = grpDisplayedSearchCriteria.Height +
                          pnlBtnOKCancelHelpLayout.Height + stbMain.Height + 22;

            FSaveChangedOptions = false;

            // The following Buttons are only for debugging...
            btnShowRightListItems.Visible = false;
            btnShowLeftListItems.Visible = false;

            // The rearranging of Find Criteria isn't quite stable yet, so we don't show the Button that enables the rearranging...
            btnMore.Visible = false;

            // Define default order in which the items in the left and right columns should appear
            // This is used to determine the place where added fields should appear in the columns
            FCriteriaFieldsLeftDefaultOrder = new ArrayList(new String[]
                { "PartnerName", "PersonalName",
                  "PreviousName", "Email",
                  "Address1", "Address2", "Address3", "City",
                  "PostCode", "County",
                  "Country", "MailingAddressOnly", "PhoneNumber" });

            FCriteriaFieldsRightDefaultOrder = new ArrayList(new String[]
                { "PartnerClass", "PartnerKey",
                  "OMSSKey", "LocationKey", "PartnerStatus",
                  "PersonnelCriteria" });

            // Load items that should go into the left and right columns from User Defaults
            FCriteriaFieldsLeft =
                new ArrayList(TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_FINDOPTIONS_CRITERIAFIELDSLEFT,
                        PARTNER_FINDOPTIONS_CRITERIAFIELDSLEFT_DEFAULT).Split(new Char[] { (';') }));
            FCriteriaFieldsRight =
                new ArrayList(TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_FINDOPTIONS_CRITERIAFIELDSRIGHT,
                        PARTNER_FINDOPTIONS_CRITERIAFIELDSRIGHT_DEFAULT).Split(new Char[] { (';') }));
            RemoveInvalidCriteria();

            // Populate the three CheckedListBoxes and check the correct items
            SetupFindCriteriaLists();
            chkShowMatchButtons.Checked = TUserDefaults.GetBooleanDefault(TUserDefaults.PARTNER_FINDOPTIONS_SHOWMATCHBUTTONS, true);
            chkExactPartnerKeyMatchSearch.Checked = TUserDefaults.GetBooleanDefault(TUserDefaults.PARTNER_FINDOPTIONS_EXACTPARTNERKEYMATCHSEARCH,
                true);

            // Hook up FindCriteriaSelectionChanged that is fired by ucoPartnerFindCriteriaSetup
            ucoPartnerFindCriteriaSetup.FindCriteriaSelectionChanged += new FindCriteriaSelectionChangedHandler(
                this.UcoPartnerFindCriteriaSetup_FindCriteriaSelectionChanged);
        }

        private void BtnCancel_Click(System.Object sender, System.EventArgs e)
        {
            FSaveChangedOptions = false;
            this.Close();
        }

        private void BtnReset_Click(System.Object sender, System.EventArgs e)
        {
            DialogResult ResetQuestionResult;

            ResetQuestionResult = MessageBox.Show(Catalog.GetString("Do you want to reset the Find Criteria to the OpenPetra default?"),
                Catalog.GetString("Reset Find Criteria"), MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (ResetQuestionResult == DialogResult.Yes)
            {
                // Load items that should go into the left and right columns from User Defaults
                FCriteriaFieldsLeft =
                    new ArrayList(PARTNER_FINDOPTIONS_CRITERIAFIELDSLEFT_DEFAULT.Split(new Char[] { (';') }));
                FCriteriaFieldsRight =
                    new ArrayList(PARTNER_FINDOPTIONS_CRITERIAFIELDSRIGHT_DEFAULT.Split(new Char[] { (';') }));

                // Populate the three CheckedListBoxes and check the correct items
                clbPartnerFindCriteria.Items.Clear();
                clbAddressFindCriteria.Items.Clear();
                clbOtherFindCriteria.Items.Clear();

                SetupFindCriteriaLists();

                chkShowMatchButtons.Checked = true;
                chkExactPartnerKeyMatchSearch.Checked = true;
            }
        }

        private void SetupFindCriteriaLists()
        {
            object[] CheckedListItemArray;
            CheckedListBox TheCheckedListBox;
            Int32 Counter1;
            Int32 Counter2;
            String LocalisedCountyLabel;
            String Dummy;
            FFindCriteriaListsSetupRunning = true;
            LocalisedStrings.GetLocStrCounty(out LocalisedCountyLabel, out Dummy);
            LocalisedCountyLabel = LocalisedCountyLabel.Replace(":", "").Replace("&", "");
            CheckedListItemArray = new object[7];
            CheckedListItemArray[0] = new TFindCriteriaListItem("Partner Name", "PartnerName", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[1] = new TFindCriteriaListItem("Personal (First) Name", "PersonalName", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[2] = new TFindCriteriaListItem("Previous Name", "PreviousName", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[3] = new TFindCriteriaListItem("Partner Key", "PartnerKey", TFindCriteriaColumn.fccRight);
            CheckedListItemArray[4] = new TFindCriteriaListItem("OMSS Key", "OMSSKey", TFindCriteriaColumn.fccRight);
            CheckedListItemArray[5] = new TFindCriteriaListItem("Partner Class", "PartnerClass", TFindCriteriaColumn.fccRight);
            CheckedListItemArray[6] = new TFindCriteriaListItem("Partner Status", "PartnerStatus", TFindCriteriaColumn.fccRight);
            clbPartnerFindCriteria.Items.AddRange(CheckedListItemArray);
            CheckedListItemArray = new object[11];
            CheckedListItemArray[0] = new TFindCriteriaListItem("Address 1", "Address1", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[1] = new TFindCriteriaListItem("Address 2", "Address2", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[2] = new TFindCriteriaListItem("Address 3", "Address3", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[3] = new TFindCriteriaListItem("Post Code", "PostCode", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[4] = new TFindCriteriaListItem("City/Town", "City", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[5] = new TFindCriteriaListItem(LocalisedCountyLabel, "County", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[6] = new TFindCriteriaListItem("Country", "Country", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[7] = new TFindCriteriaListItem("Mailing Addresses Only", "MailingAddressOnly", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[8] = new TFindCriteriaListItem("Phone Number", "PhoneNumber", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[9] = new TFindCriteriaListItem("Email", "Email", TFindCriteriaColumn.fccLeft);
            CheckedListItemArray[10] = new TFindCriteriaListItem("Location Key", "LocationKey", TFindCriteriaColumn.fccRight);
            clbAddressFindCriteria.Items.AddRange(CheckedListItemArray);
            CheckedListItemArray = new object[1];
            CheckedListItemArray[0] = new TFindCriteriaListItem("Personnel Criteria Section", "PersonnelCriteria", TFindCriteriaColumn.fccRight);
            clbOtherFindCriteria.Items.AddRange(CheckedListItemArray);
            Counter2 = 0;
            TheCheckedListBox = clbPartnerFindCriteria;

            while (Counter2 <= 2)
            {
                for (Counter1 = 0; Counter1 <= TheCheckedListBox.Items.Count - 1; Counter1 += 1)
                {
                    TFindCriteriaListItem CurrentItem = (TFindCriteriaListItem)TheCheckedListBox.Items[Counter1];

                    if (FCriteriaFieldsLeft != null)
                    {
                        if (FCriteriaFieldsLeft.Contains(CurrentItem.InternalName))
                        {
                            TheCheckedListBox.SetItemChecked(Counter1, true);
                        }
                    }

                    if (FCriteriaFieldsRight != null)
                    {
                        if (FCriteriaFieldsRight.Contains(CurrentItem.InternalName))
                        {
                            TheCheckedListBox.SetItemChecked(Counter1, true);
                        }
                    }
                }

                Counter2 = Counter2 + 1;

                if (Counter2 == 1)
                {
                    TheCheckedListBox = clbAddressFindCriteria;
                }

                if (Counter2 == 2)
                {
                    TheCheckedListBox = clbOtherFindCriteria;
                }
            }

            FFindCriteriaListsSetupRunning = false;

            // disable PartnerKeyBehavior checkbox
            Counter1 = 0;

            foreach (TFindCriteriaListItem CurrentItem in clbPartnerFindCriteria.Items)
            {
                if (CurrentItem.InternalName == "PartnerKey")
                {
                    chkExactPartnerKeyMatchSearch.Enabled = clbPartnerFindCriteria.GetItemChecked(Counter1);
                    return;
                }

                Counter1 = Counter1 + 1;
            }
        }

        private void HandleItemCheckEvent(System.Object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            CheckedListBox CurrentCheckedListBox;
            TFindCriteriaListItem CurrentListItem;
            Int32 NumberOfCheckedItems;

            NumberOfCheckedItems = 0;

            if (!FFindCriteriaListsSetupRunning)
            {
                CurrentCheckedListBox = ((CheckedListBox)sender);
                CurrentListItem = (TFindCriteriaListItem)CurrentCheckedListBox.Items[e.Index];

                if (CurrentListItem.InternalName == "PartnerKey")
                {
                    chkExactPartnerKeyMatchSearch.Enabled = (e.NewValue == CheckState.Checked);
                }

                if (e.NewValue == CheckState.Checked)
                {
                    if (FCriteriaFieldsLeft != null)
                    {
                        FCriteriaFieldsLeft.Remove("Spacer");
                        FCriteriaFieldsLeft.Remove("Spacer");
                    }

                    if (FCriteriaFieldsRight != null)
                    {
                        FCriteriaFieldsRight.Remove("Spacer");
                    }

                    AddNewFindCriteriaAtBestPosition(CurrentListItem);

                    if (ucoPartnerFindCriteriaSetup.CriteriaSetupMode)
                    {
                        ucoPartnerFindCriteriaSetup.DisableCriteria(CurrentListItem.InternalName);
                    }
                }
                else
                {
                    if (FCriteriaFieldsLeft != null)
                    {
                        FCriteriaFieldsLeft.Remove("Spacer");
                        FCriteriaFieldsLeft.Remove("Spacer");
                        NumberOfCheckedItems = FCriteriaFieldsLeft.Count;
                    }

                    if (FCriteriaFieldsRight != null)
                    {
                        FCriteriaFieldsRight.Remove("Spacer");
                        NumberOfCheckedItems = NumberOfCheckedItems + FCriteriaFieldsRight.Count;
                    }

                    // MessageBox.Show('FCriteriaFieldsLeft.Count + FCriteriaFieldsRight.Count: ' + Int16(FCriteriaFieldsLeft.Count + FCriteriaFieldsRight.Count).ToString);
                    // remove Criteria Field, but only if it isn't the last one
                    if (NumberOfCheckedItems > 1)
                    {
                        if (FCriteriaFieldsLeft != null)
                        {
                            FCriteriaFieldsLeft.Remove(CurrentListItem.InternalName);
                        }

                        if (FCriteriaFieldsRight != null)
                        {
                            FCriteriaFieldsRight.Remove(CurrentListItem.InternalName);
                        }

                        if (ucoPartnerFindCriteriaSetup.CriteriaSetupMode)
                        {
                            ucoPartnerFindCriteriaSetup.UnSelectCriteriaPanel(CurrentListItem.InternalName);
                        }

                        // MessageBox.Show('FCriteriaFieldsLeft items left: ' + FCriteriaFieldsLeft.Count.ToString + Environment.NewLine +
                        // 'FCriteriaFieldsRight items left: ' + FCriteriaFieldsRight.Count.ToString);
                    }
                    else
                    {
                        // check item again
                        e.NewValue = CheckState.Checked;
                        MessageBox.Show(
                            "You are trying to remove the last find criteria." + Environment.NewLine +
                            "This is not allowed - there must be at least one find criteria left to search for!",
                            "Cannot Remove Last Find Criteria!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (ucoPartnerFindCriteriaSetup.CriteriaSetupMode)
                {
                    ucoPartnerFindCriteriaSetup.DisplayCriteriaFieldControls();

                    // reselect the panel; this throws an FindCriteriaSelectionChanged in which information about
                    // changed position, etc. are passed again to this form
                    ucoPartnerFindCriteriaSetup.SelectCriteriaPanel(FLastSelectedCriteria);
                }
            }
        }

        private void UcoPartnerFindCriteriaSetup_FindCriteriaSelectionChanged(System.Object sender, TPartnerFindCriteriaSelectionChangedEventArgs e)
        {
            // MessageBox.Show('Received FindCriteriaSelectionChanged event!');
            FLastSelectedCriteria = e.FSelectedCriteria;

            if (FLastSelectedCriteria != null)
            {
                btnMoveUp.Enabled = true;
                btnMoveDown.Enabled = true;

                if (e.IsFirstInColumn)
                {
                    btnMoveUp.Enabled = false;
                }

                if (e.IsLastInColumn)
                {
                    btnMoveDown.Enabled = false;
                }

                if (e.FCriteriaColumn == TFindCriteriaColumn.fccLeft)
                {
                    btnMoveToRightColumn.Enabled = true;
                    btnMoveToLeftColumn.Enabled = false;
                }
                else
                {
                    btnMoveToLeftColumn.Enabled = true;
                    btnMoveToRightColumn.Enabled = false;
                }
            }
            else
            {
                // no criteria selected
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = false;
                btnMoveToLeftColumn.Enabled = false;
                btnMoveToRightColumn.Enabled = false;
            }
        }

        private String MakeCriteriaListString(ArrayList CriteriaFields)
        {
            String ReturnValue = "";
            Int16 Counter1;

            // Built result string
            if (CriteriaFields != null)
            {
                for (Counter1 = 0; Counter1 <= CriteriaFields.Count - 1; Counter1 += 1)
                {
                    ReturnValue = ReturnValue + CriteriaFields[Counter1].ToString() + ';';
                }

                // Remove trailing semicolon
                if (ReturnValue != null)
                {
                    ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 1);
                }
            }

            // MessageBox.Show('MakeCriteriaListString Result: ' + Result.ToString);
            return ReturnValue;
        }

        private void RemoveInvalidCriteria()
        {
            Int32 Counter;
            ArrayList InspectedFieldsArrayList;
            ArrayList InspectedFieldsArrayListDefault;

            InspectedFieldsArrayList = FCriteriaFieldsLeft;
            InspectedFieldsArrayListDefault = FCriteriaFieldsLeftDefaultOrder;
            Counter = 0;

            while (1 == 1)
            {
                if (InspectedFieldsArrayList != null)
                {
                    while (Counter < InspectedFieldsArrayList.Count)
                    {
                        if (!InspectedFieldsArrayListDefault.Contains(InspectedFieldsArrayList[Counter]))
                        {
                            // MessageBox.Show('InspectedFieldsArrayList[' + Counter.ToString + '] ''' + InspectedFieldsArrayList[Counter].ToString + ''' is not in InspectedFieldsArrayListDefault!');
                            InspectedFieldsArrayList.RemoveAt(Counter);
                        }
                        else
                        {
                            Counter = Counter + 1;
                        }
                    }
                }

                if (InspectedFieldsArrayList == FCriteriaFieldsLeft)
                {
                    Counter = 0;
                    InspectedFieldsArrayList = FCriteriaFieldsRight;
                    InspectedFieldsArrayListDefault = FCriteriaFieldsRightDefaultOrder;
                }
                else
                {
                    return;
                }
            }
        }

        private void AddNewFindCriteriaAtBestPosition(TFindCriteriaListItem ListItem)
        {
            TFindCriteriaColumn InsertCriteriaColumn;

            //          TFindCriteriaColumn FoundInDefaultColumn;
            Int32 Counter;
            Int32 Counter2;
            Int32 Counter2Start = 0;
            Int32 DefaultPosition = -1;
            Int32 InsertPosition = -1;
            Int32 OtherItemFoundPosition;
            String ListItemName;
            ArrayList LookupCritieriaDefaultColumn = new ArrayList();
            ArrayList CriteriaFieldsList = new ArrayList();
            TSearchDirection SearchDirection;
            Boolean SearchInCurrentDirectionNecessary;
            Boolean ContinueSearch;

            ListItemName = ListItem.InternalName;

            // Find default position of the ListItem in one of the DefaultOrder lists
            if (FCriteriaFieldsLeftDefaultOrder.IndexOf(ListItemName) != -1)
            {
                DefaultPosition = FCriteriaFieldsLeftDefaultOrder.IndexOf(ListItemName);

                //              FoundInDefaultColumn = TFindCriteriaColumn.fccLeft;
                LookupCritieriaDefaultColumn = FCriteriaFieldsLeftDefaultOrder;
            }

            if (FCriteriaFieldsRightDefaultOrder.IndexOf(ListItemName) != -1)
            {
                DefaultPosition = FCriteriaFieldsRightDefaultOrder.IndexOf(ListItemName);

                //              FoundInDefaultColumn = TFindCriteriaColumn.fccRight;
                LookupCritieriaDefaultColumn = FCriteriaFieldsRightDefaultOrder;
            }

            // MessageBox.Show('Looking for other item to insert after in ' + FoundInDefaultColumn.ToString("G") + ' before position ' + DefaultPosition.ToString + '...');
            OtherItemFoundPosition = -1;
            SearchDirection = TSearchDirection.sdBackward;
            ContinueSearch = true;
            SearchInCurrentDirectionNecessary = DefaultPosition > 0;

            // Determine which CriteriaFields list to search initially
            if (FCriteriaFieldsLeft != null)
            {
                CriteriaFieldsList = FCriteriaFieldsLeft;
                Counter2Start = 0;
                Counter2 = Counter2Start;
            }
            else if (FCriteriaFieldsRight != null)
            {
                CriteriaFieldsList = FCriteriaFieldsRight;
                Counter2Start = 1;
                Counter2 = Counter2Start;
            }
            else
            {
                // none of the two criteria field lists has any items (shouldn't happen!)
            }

            // Loop backwards/forwards in the LookupCritieriaDefaultColumn to check if any item
            // before/after the ListItemName is in one of the CriteriaFieldsLists.
            while (1 == 1)
            {
                if (SearchInCurrentDirectionNecessary)
                {
                    Counter = DefaultPosition;
                    ContinueSearch = true;

                    while (ContinueSearch)
                    {
                        if (SearchDirection == TSearchDirection.sdBackward)
                        {
                            Counter = Counter - 1;

                            // MessageBox.Show('Searching backwards. Checking position ' + Counter.ToString + '...');
                            // Lower end of LookupCritieriaDefaultColumn reached?
                            if (Counter < 0)
                            {
                                // MessageBox.Show('Lower end of list reached!');
                                break;
                            }
                        }
                        else
                        {
                            Counter = Counter + 1;

                            // MessageBox.Show('Searching forward. Checking position ' + Counter.ToString + '...');
                            // Upper end of LookupCritieriaDefaultColumn reached?
                            if (Counter == LookupCritieriaDefaultColumn.Count)
                            {
                                // MessageBox.Show('Upper end of list reached!');
                                ContinueSearch = false;
                                break;
                            }
                        }

                        if (ContinueSearch)
                        {
                            Counter2 = Counter2Start;
                            CriteriaFieldsList = FCriteriaFieldsLeft;

                            // Look in the left and right CriteriaFieldsLists for a predecessor/successor of ListItemName
                            while (Counter2 <= 1)
                            {
                                // MessageBox.Show('Looking for ' + LookupCritieriaDefaultColumn[Counter].ToString);
                                if (CriteriaFieldsList.IndexOf(LookupCritieriaDefaultColumn[Counter].ToString()) != -1)
                                {
                                    OtherItemFoundPosition = CriteriaFieldsList.IndexOf(LookupCritieriaDefaultColumn[Counter]);

                                    if (SearchDirection == TSearchDirection.sdBackward)
                                    {
                                        // New item should be inserted after the found item
                                        InsertPosition = OtherItemFoundPosition + 1;
                                    }
                                    else
                                    {
                                        // New item should be inserted before the found item
                                        InsertPosition = OtherItemFoundPosition - 1;

                                        if (InsertPosition == -1)
                                        {
                                            InsertPosition = 0;
                                        }
                                    }

                                    // MessageBox.Show('Found other item at position ' + OtherItemFoundPosition.ToString + ', will insert new at position ' + InsertPosition.ToString);
                                    break;
                                }

                                // Other item not found in the first CriteriaFieldsList?
                                if (OtherItemFoundPosition == -1)
                                {
                                    Counter2 = Counter2 + 1;

                                    if (Counter2 == 1)
                                    {
                                        // MessageBox.Show('Other item not found in FCriteriaFieldsLeft, looking in FCriteriaFieldsRight...');
                                        CriteriaFieldsList = FCriteriaFieldsRight;
                                    }
                                }
                                else
                                {
                                    // New item found in the first CriteriaFieldsList
                                    break;
                                }
                            }

                            // while (Counter2 <= 1)
                        }

                        // ContinueSearch = true
                        if (OtherItemFoundPosition != -1)
                        {
                            // New item found
                            break;
                        }
                    }

                    // while 2 = 2
                }

                // SearchInCurrentDirectionNecessary = true
                // Other item was found?
                if (OtherItemFoundPosition != -1)
                {
                    break;
                }
                else if (ContinueSearch)
                {
                    SearchDirection = TSearchDirection.sdForward;

                    if (DefaultPosition < CriteriaFieldsList.Count)
                    {
                        SearchInCurrentDirectionNecessary = true;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            // while 1 = 1
            // Item found in the same LookupCritieriaDefaultColumn that is currently displayed
            if (OtherItemFoundPosition != -1)
            {
                // Found in left column
                if (CriteriaFieldsList == FCriteriaFieldsLeft)
                {
                    // Add the checked item at the appropriate position
                    FCriteriaFieldsLeft.Insert(InsertPosition, ListItemName);
                }
                else
                {
                    // Add the checked item at the appropriate position
                    FCriteriaFieldsRight.Insert(InsertPosition, ListItemName);
                }
            }
            else
            {
                InsertCriteriaColumn = ListItem.CriteriaColumn;

                if (InsertCriteriaColumn == TFindCriteriaColumn.fccLeft)
                {
                    if (FCriteriaFieldsLeft == null)
                    {
                        // Make new list
                        FCriteriaFieldsLeft = new ArrayList();
                    }

                    // Add the item at the end
                    FCriteriaFieldsLeft.Add(ListItemName);
                }
                else
                {
                    if (FCriteriaFieldsRight == null)
                    {
                        // Make new list
                        FCriteriaFieldsRight = new ArrayList();
                    }

                    // Add the item at the end
                    FCriteriaFieldsRight.Add(ListItemName);
                }
            }
        }
#endif

    }
#if TODO
    public class TFindCriteriaListItem : System.Object
    {
        public String DisplayName;
        public String InternalName;
        public TFindCriteriaColumn CriteriaColumn;

        #region TFindCriteriaListItem
        public TFindCriteriaListItem(String ADisplayName, String AInternalName, TFindCriteriaColumn ACriteriaColumn) : base()
        {
            DisplayName = ADisplayName;
            InternalName = AInternalName;
            CriteriaColumn = ACriteriaColumn;
        }

        public TFindCriteriaListItem() : base()
        {
        }

        public override String ToString()
        {
            return DisplayName;
        }

        #endregion
    }

    public enum TSearchDirection
    {
        sdForward, sdBackward
    };
#endif
}