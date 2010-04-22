/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank, timh
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Threading;
using SourceGrid;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.RemotedExceptions;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// Partner Find screen.
    ///
    /// @Comment Main features:
    ///   - Configurable Search Critiera (which ones to show, and where) in two
    ///     columns!
    ///   - Match criteria can be defined (--*,///--, -*-, ---)!
    ///   - Controls resize when the screen is resized
    ///   - 'Paging' DataGrid that only transfers the data that is needed!
    ///   - Option to transfer a smaller number of columns or all columns for the
    ///     search results (to reduce data transfer on slow dial-up connections)
    public partial class TPartnerFindScreen
    {
        /// <summary>String for the title</summary>
        public const String StrTitleFirstPart = "Partner";

        /// <summary>String for the title</summary>
        public const String StrTitleLastPart = " Find";

        /// <summary>String for the title</summary>
        public const String StrTitleRecipient = "Recipient";

        /// <summary>String for the partner find</summary>
        public const String WINDOWSETTINGSDEFAULT_NAME = "PartnerFind";

        /// <summary>Set to true to run the partner find screen in modal mode</summary>
        public static bool URunAsModalForm = false;

        /// <summary>Contains PartnerClasses that the screen should be restricted toPartnerClasses that the screen should be restricted to (if SetParameters is never called it contains only one item, '')</summary>
        private String[] FRestrictToPartnerClasses;

        /// <summary>Tells whether the Form should set the Focus to the LocationKey field on load, or not</summary>
        private Boolean FInitiallyFocusLocationKey;

        /// <summary>The Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerFind FPartnerFindObject;

        /// <summary>Indicates whether the Form's initial setup is finished; used when checking for whether retrieval of parameters from Form is valid yet, or not</summary>
        private Boolean FFormSetupFinished;

        /// <summary>
        /// initialize (called by the constructor)
        /// </summary>
        public void InitializeManualCode()
        {
            FFormSetupFinished = false;

            ArrangeMenuItemsAndToolBarButtons();
            SetupGridContextMenu();
            SetupFileMenu();
            SetupMaintainMenu();

            // Initialise FRestrictToPartnerClasses Array
            FRestrictToPartnerClasses = new string[0];

            // Set status bar texts
            FPetraUtilsObject.SetStatusBarText(btnAccept, Resourcestrings.StrAcceptButtonHelpText + Resourcestrings.StrPartnerFindSearchTargetText);
            FPetraUtilsObject.SetStatusBarText(btnCancel, Resourcestrings.StrCancelButtonHelpText + Resourcestrings.StrPartnerFindSearchTargetText);

            // catch enter on all controls, to trigger search or accept (could use this.AcceptButton, but we have several search buttons etc)
            this.KeyPreview = true;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CatchEnterKey);
        }

        private void CatchEnterKey(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ucoFindByPartnerDetails.BtnSearch_Click(sender, e);
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void SetUserDefaultLastPartnerWorkedWith()
        {
            /*
             * Store the fact that this Partner is the 'Last Partner' that was worked with
             */

// TODO            TUserDefaults.NamedDefaults.SetLastPartnerWorkedWith(FLogic.PartnerKey, TLastPartnerUse.lpuMailroomPartner);

            // TUserDefaults.SetDefault(TUserDefaults.PARTNER_LASTPARTNERWORKEDWITH, FLogic.DetermineCurrentPartnerKey);
            // This needs to be saved instantaneously because the Partner Module main screen
            // (mailroom.w) in Progress 4GL will read it when 'Partner' >
            // 'Work with Last Partner' is chosen!
            // TUserDefaults.SaveChangedUserDefault(TUserDefaults.PARTNER_LASTPARTNERWORKEDWITH);
        }

        private void MniMailing_Popup(System.Object sender, System.EventArgs e)
        {
// TODO MniMailing_Popup
#if TODO
            DataRowView[] GridRows;
            GridRows = grdResult.SelectedDataRowsAsDataRowView;

            // Check if a Grid Row is selected
            if (GridRows.Length <= 0)
            {
                mniMailingGenerateExtract.Enabled = false;
            }
            else
            {
                mniMailingGenerateExtract.Enabled = true;
            }
#endif
        }

        private void MniFile_Popup(System.Object sender, System.EventArgs e)
        {
// TODO MniFile_Popup
#if TODO
            DataRowView[] GridRows;
            int Counter;
            GridRows = grdResult.SelectedDataRowsAsDataRowView;

            // Check if a Grid Row is selected
            if (GridRows.Length <= 0)
            {
                for (Counter = 0; Counter <= mniFile.MenuItems.Count - 1; Counter += 1)
                {
                    mniFile.MenuItems[Counter].Enabled = false;
                }

                mniFileSearch.Enabled = true;
                mniFileClose.Enabled = true;
                mniFileRecentPartners.Enabled = true;
                mniFileNewPartner.Enabled = true;
                mniFileImportPartner.Enabled = true;
                mniFileMergePartners.Enabled = true;
            }
            else
            {
                for (Counter = 0; Counter <= mniFile.MenuItems.Count - 1; Counter += 1)
                {
                    mniFile.MenuItems[Counter].Enabled = true;
                }
            }

            // check if menu item "Work with last partner" can be enabled
            long LastPartnerKey = TUserDefaults.GetInt64Default(TUserDefaults.USERDEFAULT_LASTPARTNERMAILROOM, 0);

            if (CanEditPartner(ref LastPartnerKey))
            {
                mniFileWorkWithLastPartner.Enabled = true;
            }
            else
            {
                mniFileWorkWithLastPartner.Enabled = false;
            }
#endif
        }

        private void MniMailing_Click(System.Object sender, System.EventArgs e)
        {
// TODO MniMailing_Click
#if TODO
            String ClickedMenuItemText;

            ClickedMenuItemText = ((MenuItem)sender).Text;

            if (ClickedMenuItemText == mniMailingExtracts.Text)
            {
                TMenuFunctions.OpenExtracts();
            }
            else if (ClickedMenuItemText == mniMailingDuplicateAddressCheck.Text)
            {
                TMenuFunctions.DuplicateAddressCheck();
            }
            else if (ClickedMenuItemText == mniMailingMergeAddresses.Text)
            {
                TMenuFunctions.MergeAddresses();
            }
            else if (ClickedMenuItemText == mniMailingPartnersAtLocation.Text)
            {
                OpenPartnerFindForLocation();
            }
            else if (ClickedMenuItemText == mniMailingSubscriptionCancellation.Text)
            {
                TMenuFunctions.SubscriptionCancellation();
            }
            else if (ClickedMenuItemText == mniMailingSubscriptionExpNotice.Text)
            {
                TMenuFunctions.SubscriptionExpiryNotices();
            }
            else if (ClickedMenuItemText == mniMailingGenerateExtract.Text)
            {
                CreateNewExtractFromFoundPartners();
            }
#endif
        }

        private void CreateNewExtractFromFoundPartners()
        {
// TODO: CreateNewExtractFromFoundPartners
#if TODO
            bool Success = false;

            using (TPartnerNewExtract CreateExtractDialog = new TPartnerNewExtract())
            {
                // Open the Dialog for creating a New Extract
                if (CreateExtractDialog.ShowDialog() == DialogResult.OK)
                {
                    // Make Dialog visible again to be able to show Text in its StatusBar!
                    CreateExtractDialog.Visible = true;
                    CreateExtractDialog.ShowProgressAfterOK("Adding Partners to Extract. Please wait...");

                    this.Cursor = Cursors.WaitCursor;
                    Application.DoEvents();

                    int ExtractId;
                    CreateExtractDialog.GetReturnedParameters(out ExtractId);

                    /*
                     * Make Server call to add all found Partners to the new Extract.
                     * This can take some time to finish...
                     */
                    try
                    {
                        TVerificationResultCollection VerificationResult;
                        int ExtractPartners = FPartnerFindObject.AddAllFoundPartnersToExtract(ExtractId, out VerificationResult);

                        if (ExtractPartners != -1)
                        {
                            string MessageText;

                            if (ExtractPartners == 1)
                            {
                                MessageText = Resourcestrings.StrPartnersAddedToExtractText;
                            }
                            else
                            {
                                MessageText = Resourcestrings.StrPartnersAddedToExtractPluralText;
                            }

                            MessageBox.Show(String.Format(MessageText,
                                    ExtractPartners), Resourcestrings.StrPartnersAddedToExtractTitle, MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            Success = true;
                        }
                        else
                        {
                            if (VerificationResult != null)
                            {
                                MessageBox.Show(Messages.BuildMessageFromVerificationResult(null, VerificationResult));
                            }
                            else
                            {
                                MessageBox.Show("An unknown error occured while Parters were added to the Extract.");
                            }

                            Success = false;
                        }
                    }
                    finally
                    {
                        if (!Success)
                        {
                            CreateExtractDialog.DeleteExtractAgain();
                        }

                        this.Cursor = Cursors.Default;
                        Application.DoEvents();
                    }
                }
            }
#endif
        }

        private void MniMaintain_Popup(System.Object sender, System.EventArgs e)
        {
// TODO MniMaintain_Popup
#if TODO
            DataRowView[] GridRows;
            String PartnerClass = "";
            System.Windows.Forms.Menu.MenuItemCollection MenuItemCollectionEnum;
            Boolean SenderIsContextMenu;

            // Is the Maintain Menu or the ContextMenu calling us?
            if (sender == mniMaintain)
            {
                MenuItemCollectionEnum = mniMaintain.MenuItems;
                SenderIsContextMenu = false;
            }
            else
            {
                MenuItemCollectionEnum = mnuPartnerFindContext.MenuItems;
                SenderIsContextMenu = true;
            }

            GridRows = grdResult.SelectedDataRowsAsDataRowView;

            // Check if a Grid Row is selected
            if (GridRows.Length <= 0)
            {
                // No Row is selected > disable all MenuItems
                foreach (MenuItem IndividualMenuItem in MenuItemCollectionEnum)
                {
                    IndividualMenuItem.Enabled = false;
                }

                // Only need to work on the Maintain Menu, and not the ContextMenu
                // (ContextMenu can't be activated when there are no Rows)
                mniMaintainFamilyMembers.Text = Resourcestrings.StrFamilyMembersMenuItemText;
            }
            else
            {
                // A Row is selected.
                // Start off with all MenuItems enabled
                foreach (MenuItem IndividualMenuItem in MenuItemCollectionEnum)
                {
                    IndividualMenuItem.Enabled = true;

                    if (SenderIsContextMenu)
                    {
                        if (IndividualMenuItem.Text == Resourcestrings.StrPersonnelUnitMenuItemText)
                        {
                            IndividualMenuItem.Text = Resourcestrings.StrPersonnelPersonMenuItemText;
                        }
                    }
                }

                mniMaintainPersonnelIndividualData.Text = Resourcestrings.StrPersonnelPersonMenuItemText;

                /*
                 * Set the Maintain Menu and ContextMenu MenuItems up - according to the
                 * PartnerClass of Partner of the selected Grid Row.
                 */
                PartnerClass = GridRows[0]["p_partner_class_c"].ToString();

                if (PartnerClass == "FAMILY")
                {
                    // PERSON needs 'Family Members', but not 'Personnel' MenuItems

                    if (SenderIsContextMenu)
                    {
                        // Work on Context Menu
                        Int32 Counter = 0;

                        foreach (MenuItem IndividualMenuItem in MenuItemCollectionEnum)
                        {
                            if ((IndividualMenuItem.Text == Resourcestrings.StrFamilyMenuItemText)
                                || (IndividualMenuItem.Text == mniMaintainPersonnelIndividualData.Text))
                            {
                                if (IndividualMenuItem.Text == Resourcestrings.StrFamilyMenuItemText)
                                {
                                    IndividualMenuItem.Text = Resourcestrings.StrFamilyMembersMenuItemText;

                                    // Exchange the 'Family' icon with the 'Family Members' icon!
                                    this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter], imlMenuHelper.Images[1]);
                                    break;
                                }
                                else
                                {
                                    IndividualMenuItem.Enabled = false;
                                }
                            }

                            Counter++;
                        }
                    }
                    else
                    {
                        // Work on Maintain Menu
                        mniMaintainFamilyMembers.Text = Resourcestrings.StrFamilyMembersMenuItemText;

                        // Exchange the 'Family' icon with the 'Family Members' icon!
                        this.XPMenuItemExtender.SetMenuGlyph(this.mniMaintainFamilyMembers, imlMenuHelper.Images[1]);
                        mniMaintainPersonnelIndividualData.Text = Resourcestrings.StrPersonnelPersonMenuItemText;
                        mniMaintainPersonnelIndividualData.Enabled = false;
                    }
                }
                else if (PartnerClass == "PERSON")
                {
                    // PERSON needs 'Family' and 'Personnel' MenuItems

                    if (SenderIsContextMenu)
                    {
                        // Work on Context Menu
                        Int32 Counter = 0;

                        foreach (MenuItem IndividualMenuItem in MenuItemCollectionEnum)
                        {
                            if (IndividualMenuItem.Text == Resourcestrings.StrFamilyMembersMenuItemText)
                            {
                                IndividualMenuItem.Text = Resourcestrings.StrFamilyMenuItemText;

                                // Exchange the 'Family Members' icon with the 'Family' icon!
                                this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter], imlMenuHelper.Images[0]);
                                break;
                            }

                            Counter++;
                        }
                    }
                    else
                    {
                        // Work on Maintain Menu
                        mniMaintainFamilyMembers.Text = Resourcestrings.StrFamilyMenuItemText;

                        // Exchange the 'Family Members' icon with the 'Family' icon!
                        this.XPMenuItemExtender.SetMenuGlyph(this.mniMaintainFamilyMembers, imlMenuHelper.Images[0]);
                    }
                }
                else if (PartnerClass == "UNIT")
                {
                    // UNIT doesn't needs 'Family', but 'Personnel' MenuItems

                    if (SenderIsContextMenu)
                    {
                        // Work on Context Menu
                        foreach (MenuItem IndividualMenuItem in MenuItemCollectionEnum)
                        {
                            if ((IndividualMenuItem.Text == Resourcestrings.StrFamilyMenuItemText)
                                || (IndividualMenuItem.Text == Resourcestrings.StrFamilyMembersMenuItemText)
                                || (IndividualMenuItem.Text == Resourcestrings.StrPersonnelPersonMenuItemText)
                                || (IndividualMenuItem.Text == mniMaintainWorkerField.Text))
                            {
                                if (IndividualMenuItem.Text != Resourcestrings.StrPersonnelPersonMenuItemText)
                                {
                                    IndividualMenuItem.Enabled = false;
                                }
                                else
                                {
                                    IndividualMenuItem.Text = Resourcestrings.StrPersonnelUnitMenuItemText;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Work on Maintain Menu
                        mniMaintainFamilyMembers.Enabled = false;
                        mniMaintainWorkerField.Enabled = false;
                        mniMaintainPersonnelIndividualData.Text = Resourcestrings.StrPersonnelUnitMenuItemText;
                    }
                }
                else
                {
                    // All other PartnerClasses: disable Family (Members) and Personnel
                    if (SenderIsContextMenu)
                    {
                        // Work on Context Menu
                        foreach (MenuItem IndividualMenuItem in MenuItemCollectionEnum)
                        {
                            // MessageBox.Show('FAMILY: SenderIsContextMenu: ' + SenderIsContextMenu.ToString);
                            if ((IndividualMenuItem.Text == Resourcestrings.StrFamilyMenuItemText)
                                || (IndividualMenuItem.Text == Resourcestrings.StrFamilyMembersMenuItemText)
                                || (IndividualMenuItem.Text == mniMaintainPersonnelIndividualData.Text)
                                || (IndividualMenuItem.Text == mniMaintainWorkerField.Text))
                            {
                                if (IndividualMenuItem.Text == Resourcestrings.StrFamilyMenuItemText)
                                {
                                    IndividualMenuItem.Text = Resourcestrings.StrFamilyMembersMenuItemText;
                                    IndividualMenuItem.Enabled = false;
                                }
                                else
                                {
                                    IndividualMenuItem.Enabled = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Work on Maintain Menu
                        mniMaintainFamilyMembers.Text = Resourcestrings.StrFamilyMembersMenuItemText;
                        mniMaintainFamilyMembers.Enabled = false;
                        mniMaintainPersonnelIndividualData.Enabled = false;
                        mniMaintainWorkerField.Enabled = false;
                    }
                }
            }

            Boolean IsFoundation = false;

            if ((PartnerClass == "ORGANISATION"))
            {
                FLogic.DetermineCurrentFoundationStatus(out IsFoundation);
            }

            if (IsFoundation)
            {
                mniMaintainFoundationDetails.Enabled = true;

                // Work on Context Menu
                foreach (MenuItem IndividualMenuItem in MenuItemCollectionEnum)
                {
                    if (IndividualMenuItem.Text == mniMaintainFoundationDetails.Text)
                    {
                        IndividualMenuItem.Enabled = true;
                    }
                }
            }
            else
            {
                mniMaintainFoundationDetails.Enabled = false;

                // Work on Context Menu
                foreach (MenuItem IndividualMenuItem in MenuItemCollectionEnum)
                {
                    if (IndividualMenuItem.Text == mniMaintainFoundationDetails.Text)
                    {
                        IndividualMenuItem.Enabled = false;
                    }
                }
            }
#endif
        }

        private void MniMaintain_Click(System.Object sender, System.EventArgs e)
        {
// TODO MniMaintain_Click
#if TODO
            String ClickedMenuItemText;

            ClickedMenuItemText = ((MenuItem)sender).Text;

            if (ClickedMenuItemText == mniMaintainAddresses.Text)
            {
                OpenPartnerEditScreen(TPartnerEditTabPageEnum.petpAddresses);
            }
            else if (ClickedMenuItemText == mniMaintainPartnerDetails.Text)
            {
                OpenPartnerEditScreen(TPartnerEditTabPageEnum.petpDetails);
            }
            else if (ClickedMenuItemText == mniMaintainFoundationDetails.Text)
            {
                OpenPartnerEditScreen(TPartnerEditTabPageEnum.petpFoundationDetails);
            }
            else if (ClickedMenuItemText == mniMaintainSubscriptions.Text)
            {
                OpenPartnerEditScreen(TPartnerEditTabPageEnum.petpSubscriptions);
            }
            else if (ClickedMenuItemText == mniMaintainSpecialTypes.Text)
            {
                OpenPartnerEditScreen(TPartnerEditTabPageEnum.petpPartnerTypes);
            }
            else if (ClickedMenuItemText == mniMaintainContacts.Text)
            {
                TMenuFunctions.OpenPartnerContacts();
            }
            else if ((ClickedMenuItemText == Resourcestrings.StrFamilyMembersMenuItemText)
                     || (ClickedMenuItemText == Resourcestrings.StrFamilyMenuItemText))
            {
                OpenPartnerEditScreen(TPartnerEditTabPageEnum.petpFamilyMembers);
            }
            else if (ClickedMenuItemText == mniMaintainRelationships.Text)
            {
                TMenuFunctions.OpenPartnerRelationships();
            }
            else if (ClickedMenuItemText == mniMaintainInterests.Text)
            {
                TMenuFunctions.OpenPartnerInterests();
                SetUserDefaultLastPartnerWorkedWith();
            }
            else if (ClickedMenuItemText == mniMaintainReminders.Text)
            {
                TMenuFunctions.OpenPartnerReminders();
            }
            else if (ClickedMenuItemText == mniMaintainNotes.Text)
            {
                OpenPartnerEditScreen(TPartnerEditTabPageEnum.petpNotes);
            }
            else if (ClickedMenuItemText == mniMaintainOfficeSpecific.Text)
            {
                OpenPartnerEditScreen(TPartnerEditTabPageEnum.petpOfficeSpecific);
            }
            else if (ClickedMenuItemText == mniMaintainWorkerField.Text)
            {
                TMenuFunctions.OpenWorkerField();
            }
            else if ((ClickedMenuItemText == Resourcestrings.StrPersonnelPersonMenuItemText)
                     || (ClickedMenuItemText == Resourcestrings.StrPersonnelUnitMenuItemText))
            {
                TMenuFunctions.OpenPersonnelIndivData();
            }
            else if (ClickedMenuItemText == mniMaintainDonorHistory.Text)
            {
                TMenuFunctions.OpenDonorGiftHistory(this);
            }
            else if (ClickedMenuItemText == mniMaintainRecipientHistory.Text)
            {
                TMenuFunctions.OpenRecipientGiftHistory(this);
            }
            else if (ClickedMenuItemText == mniMaintainFinanceDetails.Text)
            {
                TMenuFunctions.OpenPartnerFinanceDetails();
            }
#endif
        }

        private void MniView_Click(System.Object sender, System.EventArgs e)
        {
// TODO MniView_Click
#if TODO
            String ClickedMenuItemText;

            ClickedMenuItemText = ((MenuItem)sender).Text;

            if (ClickedMenuItemText == mniViewPartnerInfo.Text)
            {
                if (!FPartnerInfoPaneOpen)
                {
                    OpenPartnerInfoPane();
                }
                else
                {
                    ClosePartnerInfoPane();
                }
            }
            else if (ClickedMenuItemText == mniViewPartnerTasks.Text)
            {
                if (!FPartnerTasksPaneOpen)
                {
                    OpenPartnerTasksPane();
                }
                else
                {
                    ClosePartnerTasksPane();
                }
            }
#endif
        }

        private void MniFile_Click(System.Object sender, System.EventArgs e)
        {
// TODO MniFile_Click
#if TODO
            String ClickedMenuItemText;

            ClickedMenuItemText = ((MenuItem)sender).Text;

            if (ClickedMenuItemText == mniFileSearch.Text)
            {
                BtnSearch_Click(this, new EventArgs());
            }
            else if (ClickedMenuItemText == mniFileNewPartner.Text)
            {
                OpenNewPartnerEditScreen();
            }
            else if (ClickedMenuItemText == mniFileEditPartner.Text)
            {
                OpenPartnerEditScreen(TPartnerEditTabPageEnum.petpAddresses);
            }
            else if (ClickedMenuItemText == mniFileMergePartners.Text)
            {
                TMenuFunctions.MergePartners();
            }
            else if (ClickedMenuItemText == mniFileDeletePartner.Text)
            {
                TMenuFunctions.DeletePartner();
            }
            else if (ClickedMenuItemText == mniFileCopyAddress.Text)
            {
                OpenCopyAddressToClipboardScreen();
            }
            else if (ClickedMenuItemText == mniFileCopyPartnerKey.Text)
            {
                TMenuFunctions.CopyPartnerKeyToClipboard();
            }
            else if (ClickedMenuItemText == mniFileSendEmail.Text)
            {
                TMenuFunctions.SendEmailToPartner();
            }
            else if (ClickedMenuItemText == mniFilePrintPartner.Text)
            {
                TMenuFunctions.PrintPartner();
            }
            else if (ClickedMenuItemText == mniFileExportPartner.Text)
            {
                TMenuFunctions.ExportPartner();
            }
            else if (ClickedMenuItemText == mniFileImportPartner.Text)
            {
                TMenuFunctions.ImportPartner();
            }
            else if (ClickedMenuItemText == mniFileClose.Text)
            {
                base.MniFile_Click(sender, e);
            }
            else if (ClickedMenuItemText == mniFileRecentPartners.Text)
            {
                SetupRecentlyUsedPartnersMenu();
            }
            else if (ClickedMenuItemText == mniFileWorkWithLastPartner.Text)
            {
                OpenLastUsedPartnerEditScreen();
            }
            else
            {
                NotifyFunctionNotYetImplemented(ClickedMenuItemText);
            }
#endif
        }

        private void MniFileRecentPartner_Popup(System.Object sender, System.EventArgs e)
        {
            this.SetupRecentlyUsedPartnersMenu();
        }

        private void MniFileRecentPartner_Click(System.Object sender, System.EventArgs e)
        {
// TODO MniFileRecentPartner_Click
#if TODO
            String ClickedMenuItemText = ((MenuItem)sender).Text;

            foreach (MenuItem CurrentMenu in mniFileRecentPartners.MenuItems)
            {
                if (CurrentMenu.Text == ClickedMenuItemText)
                {
                    // Set partner to the "last used person"
                    TUserDefaults.SetDefault(TUserDefaults.USERDEFAULT_LASTPARTNERMAILROOM, CurrentMenu.Tag);

                    // Open the Partner Edit screen
                    TPartnerEditDSWinForm frmPEDS;

                    this.Cursor = Cursors.WaitCursor;

                    frmPEDS = new TPartnerEditDSWinForm(this.Handle);
                    frmPEDS.SetParameters(TScreenMode.smEdit, (long)CurrentMenu.Tag);
                    frmPEDS.Show();

                    this.Cursor = Cursors.Default;
                }
            }
#endif
        }

        private void AddCopyContextMenuEntries(ContextMenu AMenuToAddInto)
        {
// TODO AddCopyContextMenuEntries
#if TODO
            Int32 Counter;

            Counter = AMenuToAddInto.MenuItems.Count;

            // Separator
            AMenuToAddInto.MenuItems.Add("-");
            this.XPMenuItemExtender.SetNewStyleActive(AMenuToAddInto.MenuItems[Counter], true);
            Counter++;

            // Copy Address...
            AMenuToAddInto.MenuItems.Add(mniFileCopyAddress.CloneMenu());
            this.XPMenuItemExtender.SetMenuGlyph(AMenuToAddInto.MenuItems[Counter], this.XPMenuItemExtender.GetMenuGlyph(mniFileCopyAddress));
            this.XPMenuItemExtender.SetNewStyleActive(AMenuToAddInto.MenuItems[Counter], true);
            Counter++;

            // Copy Partner Key
            AMenuToAddInto.MenuItems.Add(mniFileCopyPartnerKey.CloneMenu());
            this.XPMenuItemExtender.SetMenuGlyph(AMenuToAddInto.MenuItems[Counter], this.XPMenuItemExtender.GetMenuGlyph(mniFileCopyPartnerKey));
            this.XPMenuItemExtender.SetNewStyleActive(AMenuToAddInto.MenuItems[Counter], true);
            Counter++;

            // Separator
            AMenuToAddInto.MenuItems.Add("-");
            this.XPMenuItemExtender.SetNewStyleActive(AMenuToAddInto.MenuItems[Counter], true);
            Counter++;

            // Send Email
            mnuPartnerFindContext.MenuItems.Add(mniFileSendEmail.CloneMenu());
            this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter], this.XPMenuItemExtender.GetMenuGlyph(mniFileSendEmail));
            this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter], true);
            Counter++;

            // Separator
            AMenuToAddInto.MenuItems.Add("-");
            this.XPMenuItemExtender.SetNewStyleActive(AMenuToAddInto.MenuItems[Counter], true);
            Counter++;

            // Export Partner
            mnuPartnerFindContext.MenuItems.Add(mniFileExportPartner.CloneMenu());
            this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter], this.XPMenuItemExtender.GetMenuGlyph(mniFileExportPartner));
            this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter], true);
            Counter++;

            // Import Partner
            mnuPartnerFindContext.MenuItems.Add(mniFileImportPartner.CloneMenu());
            this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter], this.XPMenuItemExtender.GetMenuGlyph(mniFileImportPartner));
            this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter], true);
#endif
        }

        private void ArrangeMenuItemsAndToolBarButtons()
        {
// TODO ArrangeMenuItemsAndToolBarButtons
#if TODO
            mniFileClose.Index = mniFile.MenuItems.Count - 1;
            mniMaintain.Index = 2;
            mniMailing.Index = 3;
            mniTools.Index = 4;
            mniPetra.Index = 5;
            mniHelp.Index = mnuMain.MenuItems.Count - 1;

            // arrange ToolBarButtons (base.and Self ones) properly [get garbled up by the Designer due to inheritance]
            tbrMain.Buttons.Move(tbbClose, 0);
#endif
        }

        private void BtnAccept_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void NotifyFunctionNotYetImplemented(String AFunctionName)
        {
            String TitlePrefix;

            if (AFunctionName != "")
            {
                TitlePrefix = AFunctionName + ": ";
                TitlePrefix = TitlePrefix.Replace("&", "");
            }
            else
            {
                TitlePrefix = "";
            }

            MessageBox.Show("Sorry, this function is not yet implemented...", TitlePrefix + "Not Yet Implemented");
        }

        private void TPartnerFindScreen_Activated(System.Object sender, System.EventArgs e)
        {
            if (FFormSetupFinished)
            {
                if (!FPetraUtilsObject.FormActivatedForFirstTime)
                {
                    // FIRST TIME  do some initialisation
                    FPetraUtilsObject.FormActivatedForFirstTime = true;
                }
            }
        }

        #region Main functionality

        /// <summary>
        /// Builds the MenuItems of the Context Menu of the Grid by copying over MenuItems from existing
        /// Menus.
        /// </summary>
        /// <remarks>
        /// If the screen is opened in Modal mode, the Methods <see cref="SetupFileMenu" />
        /// and <see cref="SetupMaintainMenu" /> hide all MenuItems exept the ones that are
        /// added here in case the screen is opened in Modal mode. Therefore these methods need
        /// to be amended if the MenuItems that are added here in case the screen is opened in
        /// Modal mode is changed!
        /// </remarks>
        private void SetupGridContextMenu()
        {
// TODO SetupGridContextMenu
#if TODO
            int Counter1;
            int Counter2;

            if (!TPartnerFindScreen.URunAsModalForm)
            {
                /*
                 * The non-Modal ContextMenu contains all MenuItems from the 'Maintain' Menu
                 */

                // Copy over all MenuItems  including their Events (using CloneMenu()) and
                // Icons
                for (Counter1 = 0; Counter1 <= mniMaintain.MenuItems.Count - 1; Counter1 += 1)
                {
                    mnuPartnerFindContext.MenuItems.Add(mniMaintain.MenuItems[Counter1].CloneMenu());
                    this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter1],
                        this.XPMenuItemExtender.GetMenuGlyph(mniMaintain.MenuItems[Counter1]));
                    this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter1], true);
                }

                AddCopyContextMenuEntries(mnuPartnerFindContext);
            }
            else
            {
                /*
                 * The Modal ContextMenu is custom-made from selected MenuItems from
                 * different Menus.
                 *
                 * Those MenuItems are copied one by one - including their Events
                 * (using CloneMenu()) and Icons.
                 */
                Counter2 = 0;

                // Edit Partner...
                mnuPartnerFindContext.MenuItems.Add(mniFileEditPartner.CloneMenu());
                this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter2],
                    this.XPMenuItemExtender.GetMenuGlyph(mniFileEditPartner));
                this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter2], true);
                Counter2++;

                // Separator
                mnuPartnerFindContext.MenuItems.Add("-");
                this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter2], true);
                Counter2++;

                // Donor History
                mnuPartnerFindContext.MenuItems.Add(mniMaintainDonorHistory.CloneMenu());
                this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter2],
                    this.XPMenuItemExtender.GetMenuGlyph(mniMaintainDonorHistory));
                this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter2], true);
                Counter2++;

                // Recipient History
                mnuPartnerFindContext.MenuItems.Add(mniMaintainRecipientHistory.CloneMenu());
                this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter2],
                    this.XPMenuItemExtender.GetMenuGlyph(mniMaintainRecipientHistory));
                this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter2], true);
                AddCopyContextMenuEntries(mnuPartnerFindContext);
            }
#endif
        }

        /// <summary>
        /// Show or hide some of the maintain menu items. This depends if the
        /// form is run as modal or not.
        /// If it is run in modal mode we hide some of the menu items.
        /// </summary>
        /// <remarks>
        /// See Method <see cref="SetupGridContextMenu" /> for reference on what
        /// is contained in the menu if the form is run as modal. This method needs to
        /// be amended if the MenuItems that are removed here in case the screen is opened in
        /// Modal mode is changed!
        /// </remarks>
        private void SetupMaintainMenu()
        {
// TODO SetupMaintainMenu
#if TODO
            if (TPartnerFindScreen.URunAsModalForm)
            {
                /*
                 * Hide the menu items that are in "HiddenItems"
                 */

                ArrayList HiddenItems = new ArrayList();
                HiddenItems.Add("mniMaintainAddresses");
                HiddenItems.Add("mniMaintainPartnerDetails");
                HiddenItems.Add("mniMaintainFoundationDetails");
                HiddenItems.Add("mniMaintainSubscriptions");
                HiddenItems.Add("mniMaintainSpecialTypes");
                HiddenItems.Add("mniMaintainContacts");
                HiddenItems.Add("mniMaintainFamilyMembers");
                HiddenItems.Add("mniMaintainRelationships");
                HiddenItems.Add("mniMaintainInterests");
                HiddenItems.Add("mniMaintainReminders");
                HiddenItems.Add("mniMaintainNotes");
                HiddenItems.Add("mniMaintainOfficeSpecific");
                HiddenItems.Add("mniMaintainWorkerField");
                HiddenItems.Add("mniMaintainPersonnelIndividualData");
                HiddenItems.Add("mniMaintainFinanceDetails");
                HiddenItems.Add("mniMaintainSeparator1");
                HiddenItems.Add("mniMaintainSeparator2");

                for (int Counter = 0; Counter < mniMaintain.MenuItems.Count; ++Counter)
                {
                    if (HiddenItems.Contains(mniMaintain.MenuItems[Counter].Name))
                    {
                        mniMaintain.MenuItems[Counter].Visible = false;
                    }
                    else
                    {
                        mniMaintain.MenuItems[Counter].Visible = true;
                    }
                }
            }
#endif
        }

        /// <summary>
        /// Show or hide some of the file menu items. This depends if the
        /// form is run as modal or not.
        /// If it is run in modal mode we hide some of the menu items.
        /// </summary>
        /// <remarks>
        /// See Method <see cref="SetupGridContextMenu" /> for reference on what
        /// is contained in the menu if the form is run as modal. This method needs to
        /// be amended if the MenuItems that are removed here in case the screen is opened in
        /// Modal mode is changed!
        /// </remarks>
        private void SetupFileMenu()
        {
// TODO SetupFileMenu
#if TODO
            if (TPartnerFindScreen.URunAsModalForm)
            {
                /*
                 * Hide the menu items that are in "HiddenItems"
                 */

                ArrayList HiddenItems = new ArrayList();
                HiddenItems.Add("mniFileSearch");
                HiddenItems.Add("mniFileSeparator1");
                HiddenItems.Add("mniFileWorkWithLastPartner");
                HiddenItems.Add("mniFileRecentPartners");
                HiddenItems.Add("mniFileSeparator2");
                HiddenItems.Add("mniFileNewPartner");
                HiddenItems.Add("mniFileMergePartners");
                HiddenItems.Add("mniFileDeletePartner");
                HiddenItems.Add("mniFilePrintPartner");
                HiddenItems.Add("mniFileSeparator5");

                for (int Counter = 0; Counter < mniFile.MenuItems.Count; ++Counter)
                {
                    if (HiddenItems.Contains(mniFile.MenuItems[Counter].Name))
                    {
                        mniFile.MenuItems[Counter].Visible = false;
                    }
                    else
                    {
                        mniFile.MenuItems[Counter].Visible = true;
                    }
                }
            }
#endif
        }

        /// <summary>
        /// Makes a server call to get the recently used partners
        /// and displays them on the menu.
        /// </summary>
        private void SetupRecentlyUsedPartnersMenu()
        {
// TODO SetupRecentlyUsedPartnersMenu
#if TODO
            Dictionary <long, string>RecentlyUsedPartners;
            ArrayList PartnerClasses = new ArrayList();

            PartnerClasses.Add("*");

            int MaxPartnersCount = mniFileRecentPartners.MenuItems.Count;
            TServerLookup.TMPartner.GetRecentlyUsedPartners(MaxPartnersCount, PartnerClasses, out RecentlyUsedPartners);

            int Counter = 0;

            foreach (KeyValuePair <long, string>CurrentEntry in RecentlyUsedPartners)
            {
                mniFileRecentPartners.MenuItems[Counter].Text = Counter.ToString() + " - " + CurrentEntry.Value;
                mniFileRecentPartners.MenuItems[Counter].Tag = CurrentEntry.Key;
                mniFileRecentPartners.MenuItems[Counter].Enabled = true;
                mniFileRecentPartners.MenuItems[Counter].Visible = true;

                ++Counter;
            }

            // If there a less partners than menu items, then disable them
            for (; Counter < MaxPartnersCount; ++Counter)
            {
                mniFileRecentPartners.MenuItems[Counter].Enabled = false;
                mniFileRecentPartners.MenuItems[Counter].Visible = false;
            }

            // If there are no recently used partners at all show a message
            if (RecentlyUsedPartners.Count == 0)
            {
                mniFileRecentPartners.MenuItems[0].Text = "No partners used yet";
                mniFileRecentPartners.MenuItems[0].Tag = -1;
                mniFileRecentPartners.MenuItems[0].Enabled = false;
                mniFileRecentPartners.MenuItems[0].Visible = true;
            }
#endif
        }

        private void ReleaseServerObject()
        {
            if (FPartnerFindObject != null)
            {
                // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
                TEnsureKeepAlive.UnRegister(FPartnerFindObject);
                FPartnerFindObject = null;
            }
        }

        #endregion

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        ///
        /// </summary>
        /// <param name="ARestrictToPartnerClasses">Pass in one or several PartnerClasses
        /// (separated by comma) to restrict the choice in the 'Partner Class' criteria
        /// ComboBox, or empty String '' to not restrict to any Partner Class.
        /// </param>
        /// <returns>void</returns>
        public void SetParameters(String ARestrictToPartnerClasses)
        {
            SetParameters(ARestrictToPartnerClasses, false, -1);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// </summary>
        /// <param name="ARestrictToPartnerClasses">Pass in one or several PartnerClasses
        /// (separated by comma) to restrict the choice in the 'Partner Class' criteria
        /// ComboBox, or empty String '' to not restrict to any Partner Class.
        /// </param>
        /// <param name="AInitiallyFocusLocationKey">True to set the focus on the location key</param>
        /// <param name="APassedLocationKey">location key</param>
        public void SetParameters(String ARestrictToPartnerClasses, Boolean AInitiallyFocusLocationKey, Int32 APassedLocationKey)
        {
            FInitiallyFocusLocationKey = AInitiallyFocusLocationKey;

            if ((ARestrictToPartnerClasses == null) || (ARestrictToPartnerClasses.Length == 0))
            {
                ucoFindByPartnerDetails.Init(FInitiallyFocusLocationKey,
                    FRestrictToPartnerClasses,
                    APassedLocationKey);
                return;
            }

            ARestrictToPartnerClasses = ARestrictToPartnerClasses.Replace("WORKER", "WORKER-FAM");

            // Split String into String Array
            FRestrictToPartnerClasses = ARestrictToPartnerClasses.Split(new Char[] { (',') });

            // Check entries of String Array
            if (FRestrictToPartnerClasses.Length == 1)
            {
                /*
                 * Only one entry -> Set the title, with the first character of the word
                 * being upper-case, the rest lower-case.
                 */
                if (FRestrictToPartnerClasses[0] != "")
                {
                    this.Text = FRestrictToPartnerClasses[0].Substring(0, 1).ToUpper() +
                                FRestrictToPartnerClasses[0].Substring(1).ToLower() + StrTitleLastPart;
                }
                else
                {
                    // If the one entry is empty string (''), show default title
                    this.Text = StrTitleFirstPart + StrTitleLastPart;
                }
            }
            else
            {
                /*
                 * When a "Recipient" button has been pressed to bring up this screen, the
                 * ARestrictToPartnerClasses is "WORKER-FAM,UNIT".  When we find this to be the
                 * case, put 'Recipient Find' in the title.
                 * NOTE: this isn't a very robust solution...
                 */
                if ((FRestrictToPartnerClasses[0].ToUpper() == "WORKER-FAM") && (FRestrictToPartnerClasses[1].ToUpper() == "UNIT"))
                {
                    this.Text = StrTitleRecipient + StrTitleLastPart;
                }
            }

            btnAccept.Enabled = false;
            ucoFindByPartnerDetails.Init(FInitiallyFocusLocationKey,
                FRestrictToPartnerClasses,
                APassedLocationKey);
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// </summary>
        /// <param name="AInitiallyFocusLocationKey">True to set the focus on the location key</param>
        /// <param name="APassedLocationKey">Location key</param>
        public void SetParameters(Boolean AInitiallyFocusLocationKey, Int32 APassedLocationKey)
        {
            FInitiallyFocusLocationKey = AInitiallyFocusLocationKey;
            ucoFindByPartnerDetails.Init(FInitiallyFocusLocationKey,
                FRestrictToPartnerClasses,
                APassedLocationKey);
        }

        #region Form events
        private void TPartnerFindScreen_Load(System.Object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            // Restore Window Position and Size
            // TODO TUserDefaults.NamedDefaults.GetWindowPositionAndSize(this, WINDOWSETTINGSDEFAULT_NAME);
#if TODO
            // Set up Splitter distances
            ucoPartnerFindCriteria.RestoreSplitterSetting();
            RestoreSplitterSettings();
#endif

            TPartnerFindScreen.URunAsModalForm = this.Modal;
#if TODO
            if (!TPartnerFindScreen.URunAsModalForm)
            {
                pnlModalButtons.Visible = false;

                //                grdResult.Height = grdResult.Height + 24;
                //                pnlBlankSearchResult.Height = pnlBlankSearchResult.Height + 24;
            }
            else
            {
                //                pnlModalButtons.BringToFront();
                //tabPartnerFindMethods
                // TODO? stbMain.SendToBack();
                pnlModalButtons.Visible = true;
            }

            // Menu temporarily shown so we can test implemented 4GL calls from menus
            // TODO 1 oChristianK cModal : Menu needs to be hidden as soon 4GL can make nonmodal calls to this Form!
            // self.Menu:=nil;
            tbbEditPartner.Enabled = false;
            tabFindBankDetails.Enabled = false;
#endif

            FPartnerFindObject = TRemote.MPartner.Partner.UIConnectors.PartnerFind();
            ucoFindByPartnerDetails.PartnerFindObject = FPartnerFindObject;

            // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
            TEnsureKeepAlive.Register(FPartnerFindObject);

            // We're done!
            FFormSetupFinished = true;
            this.Cursor = Cursors.Default;
        }

        private void TPartnerFindScreen_Closed(System.Object sender, System.EventArgs e)
        {
            ucoFindByPartnerDetails.StoreUserDefaults();

            // Save Window Position and Size
            // TODO? TUserDefaults.NamedDefaults.SetWindowPositionAndSize(this, WINDOWSETTINGSDEFAULT_NAME);

            ReleaseServerObject();
        }

        #endregion

        private void BtnFullyLoadData_Click(System.Object sender, System.EventArgs e)
        {
            // FullLoadDataSet;
        }

        private void OpenNewPartnerEditScreen()
        {
// TODO OpenNewPartnerEditScreen
#if TODO
            String miPartnerClass;

            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (!TPartnerFindScreen.URunAsModalForm)
                {
                    // Not modal, so no restrictions on valid partner classes
                    TApplinkOrNetAutoSelector.OpenNewPartnerDialog(this, "", "");
                }
                else
                {
                    // Modal. May have restrictions, may not.

                    // default behavior is to allow all
                    miPartnerClass = "";

                    if (FRestrictToPartnerClasses.Length > 0)
                    {
                        // at least one entry so use first one
                        miPartnerClass = FRestrictToPartnerClasses[0];
                    }

                    miPartnerClass = miPartnerClass.Replace("WORKER-FAM", "FAMILY");
                    TApplinkOrNetAutoSelector.OpenNewPartnerDialog(this, miPartnerClass, "");
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
#endif
        }

        /// <summary>
        /// Opens the partner edit screen with the last partner worked on.
        /// Checks if the partner is merged.
        /// </summary>
        private void OpenLastUsedPartnerEditScreen()
        {
// TODO OpenLastUsedPartnerEditScreen
#if TODO
            long MergedPartnerKey = 0;
            long LastPartnerKey = TUserDefaults.GetInt64Default(TUserDefaults.USERDEFAULT_LASTPARTNERMAILROOM, 0);

            // we don't need to validate the partner key
            // because it's done in the mnuFile_Popup function.
            // If we don't have a valid partner key, this code can't be called from the file menu.

            if (MergedPartnerHandling(LastPartnerKey, out MergedPartnerKey))
            {
                // work with the merged partner
                LastPartnerKey = MergedPartnerKey;
            }
            else if (MergedPartnerKey > 0)
            {
                // The partner is merged but user cancelled the action
                return;
            }

            // Open the Partner Edit screen
            TPartnerEditDSWinForm frmPEDS;

            this.Cursor = Cursors.WaitCursor;

            frmPEDS = new TPartnerEditDSWinForm();
            frmPEDS.SetParameters(TScreenMode.smEdit, LastPartnerKey);
            frmPEDS.Show();

            this.Cursor = Cursors.Default;
#endif
        }

        /// <summary>
        /// Returns the values of the found partner.
        /// </summary>
        /// <param name="APartnerKey">Partner key</param>
        /// <param name="AShortName">Partner short name</param>
        /// <param name="ALocationPK">Location key</param>
        /// <returns></returns>
        public Boolean GetReturnedParameters(out Int64 APartnerKey, out String AShortName, out TLocationPK ALocationPK)
        {
            APartnerKey = -1;
            AShortName = "";
            ALocationPK = null;

            if (FFormSetupFinished)
            {
                return ucoFindByPartnerDetails.GetReturnedParameters(out APartnerKey, out AShortName, out ALocationPK);
            }

            return false;
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TMyUpdateDelegate(System.Object msg);

    /// <summary>
    /// Manages the opening of a new/showing of an existing Instance of the Partner Find Screen.
    /// </summary>
    public static class TPartnerFindScreenManager
    {
        /// <summary>
        /// Manages the opening of a new/showing of an existing Instance of the Partner Find Screen.
        /// </summary>
        /// <remarks>
        /// <para>A call to this Method will create a new Instance of the Partner Find Screen
        /// if there was no running Instance, otherwise it will just activate any Instance of
        /// the Partner Find Screen if finds.</para>
        /// </remarks>
        /// <param name="AParentFormHandle"></param>
        /// <returns>void</returns>
        public static void OpenNewOrExistingForm(IntPtr AParentFormHandle)
        {
            bool FormWasAlreadyOpened;

            OpenNewOrExistingForm(out FormWasAlreadyOpened, AParentFormHandle);
        }

        /// <summary>
        /// Manages the opening of a new/showing of an existing Instance of the Partner Find Screen.
        /// </summary>
        /// <remarks>A call to this Method will create a new Instance of the Partner Find Screen
        /// if there was no running Instance, otherwise it will just activate any Instance of
        /// the Partner Find Screen if finds.</remarks>
        /// <param name="AFormWasAlreadyOpened">False if a new Partner Find Screen was opened,
        /// false if an existing Instance of the Partner Find Screen was activated.</param>
        /// <param name="AParentFormHandle"></param>
        /// <returns>An Instance of the Partner Find Screen (either newly created or
        /// just activated).</returns>
        public static Form OpenNewOrExistingForm(out bool AFormWasAlreadyOpened, IntPtr AParentFormHandle)
        {
            Form OpenFindScreen;
            Form NewFindScreen;

            AFormWasAlreadyOpened = false;

            OpenFindScreen = TFormsList.GFormsList[typeof(TPartnerFindScreen).FullName];

            if (OpenFindScreen != null)
            {
                OpenFindScreen.BringToFront();

                AFormWasAlreadyOpened = true;

                return OpenFindScreen;
            }
            else
            {
                NewFindScreen = new TPartnerFindScreen(AParentFormHandle);
                NewFindScreen.Show();
                return NewFindScreen;
            }
        }

        /// <summary>
        /// Opens a Modal instance of the Partner Find screen.
        /// </summary>
        /// <param name="ARestrictToPartnerClasses">Pass in one or several PartnerClasses
        /// (separated by comma) to restrict the choice in the 'Partner Class' criteria
        /// ComboBox, or empty String '' to not restrict to any Partner Class.
        /// </param>
        /// <param name="APartnerKey">PartnerKey of the found Partner.</param>
        /// <param name="AShortName">Partner ShortName of the found Partner.</param>
        /// <param name="ALocationPK">LocationKey of the found Partner.</param>
        /// <param name="AParentFormHandle"></param>
        /// <returns>True if a Partner was found and accepted by the user,
        /// otherwise false.</returns>
        public static bool OpenModalForm(String ARestrictToPartnerClasses,
            out Int64 APartnerKey,
            out String AShortName,
            out TLocationPK ALocationPK,
            IntPtr AParentFormHandle)
        {
            TPartnerFindScreen PartnerFindForm;
            DialogResult dlgResult;

            APartnerKey = -1;
            AShortName = String.Empty;
            ALocationPK = new TLocationPK(-1, -1);

            PartnerFindForm = new TPartnerFindScreen(AParentFormHandle);
            PartnerFindForm.SetParameters(ARestrictToPartnerClasses);

            dlgResult = PartnerFindForm.ShowDialog();

            if (dlgResult == System.Windows.Forms.DialogResult.OK)
            {
                PartnerFindForm.GetReturnedParameters(out APartnerKey, out AShortName,
                    out ALocationPK);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}