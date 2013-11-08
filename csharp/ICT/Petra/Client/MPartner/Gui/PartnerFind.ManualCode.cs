//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timh
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
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared.MSysMan;

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
        #region Resourcestrings

// TODO        private static readonly string StrPartnersAddedToExtractText = Catalog.GetString(
// TODO            "{0} Partner was added to the new Extract.");

// TODO        private static readonly string StrPartnersAddedToExtractPluralText = Catalog.GetString(
// TODO            "{0} Partners were added to the new Extract.");

// TODO        private static readonly string StrPartnersAddedToExtractTitle = Catalog.GetString(
// TODO            "Generate Extract From Found Partners");

        /// <summary>String for the title</summary>
        private static readonly string StrTitleFirstPart = Catalog.GetString("Partner");

        /// <summary>String for the title</summary>
        private static readonly string StrTitleLastPart = Catalog.GetString(" Find");

        /// <summary>String for the title</summary>
        private static readonly string StrTitleRecipient = Catalog.GetString("Recipient");

        /// <summary>String for the title</summary>
        private static readonly string StrWorkerFamily = Catalog.GetString("Worker Family");

        #endregion

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

        private Boolean FRunAsModalForm;

        /// <summary>
        /// initialize (called by the constructor)
        /// </summary>
        public void InitializeManualCode()
        {
            FFormSetupFinished = false;

            ArrangeMenuItemsAndToolBarButtons();

            CancelButton = btnCancel;

            tbbEditPartner.Enabled = false;
            mniFileEditPartner.Enabled = false;

            // Initialise FRestrictToPartnerClasses Array
            FRestrictToPartnerClasses = new string[0];

            // Set status bar texts
            FPetraUtilsObject.SetStatusBarText(btnAccept,
                MPartnerResourcestrings.StrAcceptButtonHelpText + MPartnerResourcestrings.StrPartnerFindSearchTargetText);
            FPetraUtilsObject.SetStatusBarText(btnCancel,
                MPartnerResourcestrings.StrCancelButtonHelpText + MPartnerResourcestrings.StrPartnerFindSearchTargetText);

            // catch enter on all controls, to trigger search or accept (could use this.AcceptButton, but we have several search buttons etc)
            this.KeyPreview = true;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CatchEnterKey);

            mniFile.DropDownOpening += new System.EventHandler(MniFile_DropDownOpening);
            mniEdit.DropDownOpening += new System.EventHandler(MniEdit_DropDownOpening);
            mniMaintain.DropDownOpening += new System.EventHandler(MniMaintain_DropDownOpening);
            mniMailing.DropDownOpening += new System.EventHandler(MniMailing_DropDownOpening);
            mniFileRecentPartners.DropDownOpening += new System.EventHandler(MniFileRecentPartner_DropDownOpening);

            ucoFindByPartnerDetails.PartnerAvailable += new TUC_PartnerFind_ByPartnerDetails.TPartnerAvailableChangeEventHandler(
                ucoFindByPartnerDetails_PartnerAvailable);
            ucoFindByPartnerDetails.SearchOperationStateChange += new TUC_PartnerFind_ByPartnerDetails.TSearchOperationStateChangeEventHandler(
                ucoFindByPartnerDetails_SearchOperationStateChange);
            ucoFindByPartnerDetails.PartnerInfoPaneCollapsed += new EventHandler(ucoFindByPartnerDetails_PartnerInfoPaneCollapsed);
            ucoFindByPartnerDetails.PartnerInfoPaneExpanded += new EventHandler(ucoFindByPartnerDetails_PartnerInfoPaneExpanded);
            ucoFindByPartnerDetails.EnableAcceptButton += new EventHandler(ucoFindByPartnerDetails_EnableAcceptButton);
            ucoFindByPartnerDetails.DisableAcceptButton += new EventHandler(ucoFindByPartnerDetails_DisableAcceptButton);

            ucoFindByPartnerDetails.SetupPartnerInfoPane();
        }

        void ucoFindByPartnerDetails_SearchOperationStateChange(TSearchOperationStateChangeEventArgs e)
        {
            if (e.SearchOperationIsRunning)
            {
                mniEditSearch.Text = MPartnerResourcestrings.StrSearchMenuItemStopText;
            }
            else
            {
                mniEditSearch.Text = MPartnerResourcestrings.StrSearchButtonText;
            }
        }

        private void ucoFindByPartnerDetails_PartnerAvailable(TPartnerAvailableEventArgs e)
        {
            tbbEditPartner.Enabled = e.PartnerAvailable;
            mniFileEditPartner.Enabled = e.PartnerAvailable;
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

            TUserDefaults.NamedDefaults.SetLastPartnerWorkedWith(ucoFindByPartnerDetails.PartnerKey, TLastPartnerUse.lpuMailroomPartner);
        }

        private void MniFile_DropDownOpening(System.Object sender, System.EventArgs e)
        {
            DataRowView[] GridRows;
            int Counter;
            GridRows = ucoFindByPartnerDetails.SelectedDataRowsAsDataRowView;

            // Check if a Grid Row is selected
            if (GridRows.Length <= 0)
            {
                for (Counter = 0; Counter <= mniFile.DropDownItems.Count - 1; Counter += 1)
                {
                    mniFile.DropDownItems[Counter].Enabled = false;
                }

                mniClose.Enabled = true;
                mniFileRecentPartners.Enabled = true;
                mniFileNewPartner.Enabled = true;
                mniFileImportPartner.Enabled = true;
                mniFileMergePartners.Enabled = true;
            }
            else
            {
                for (Counter = 0; Counter <= mniFile.DropDownItems.Count - 1; Counter += 1)
                {
                    mniFile.DropDownItems[Counter].Enabled = true;
                }
            }

            // check if menu item "Work with last partner" can be enabled
            long LastPartnerKey = TUserDefaults.GetInt64Default(TUserDefaults.USERDEFAULT_LASTPARTNERMAILROOM, 0);

            if (ucoFindByPartnerDetails.CanAccessPartner(LastPartnerKey))
            {
                mniFileWorkWithLastPartner.Enabled = true;
            }
            else
            {
                mniFileWorkWithLastPartner.Enabled = false;
            }
        }

        private void MniFileRecentPartner_DropDownOpening(System.Object sender, System.EventArgs e)
        {
            this.SetupRecentlyUsedPartnersMenu();
        }

        private void MniFileRecentPartner_Click(System.Object sender, System.EventArgs e)
        {
            String ClickedMenuItemText = ((ToolStripMenuItem)sender).Text;

            foreach (ToolStripDropDownItem CurrentMenu in mniFileRecentPartners.DropDownItems)
            {
                if (CurrentMenu.Text == ClickedMenuItemText)
                {
                    ucoFindByPartnerDetails.OpenPartnerEditScreen(TPartnerEditTabPageEnum.petpDefault, (long)CurrentMenu.Tag, true);
                }
            }
        }

        void MniEdit_DropDownOpening(object sender, EventArgs e)
        {
            DataRowView[] GridRows;
            int Counter;

            GridRows = ucoFindByPartnerDetails.SelectedDataRowsAsDataRowView;

            // Check if we have access to the currently selected Partner or if we have Find Result
            if (!(ucoFindByPartnerDetails.CanAccessPartner(-1))
                || (GridRows.Length <= 0))
            {
                // Currently selected Partner is not accessible or no Find Result
                for (Counter = 2; Counter <= mniEdit.DropDownItems.Count - 1; Counter += 1)
                {
                    // Disable all MenuItems, except for 'Copy Partner's Partner Key'
                    if (mniEdit.DropDownItems[Counter] != mniEditCopyPartnerKey)
                    {
                        mniEdit.DropDownItems[Counter].Enabled = false;
                    }
                    else
                    {
                        if (GridRows.Length > 0)
                        {
                            mniEdit.DropDownItems[Counter].Enabled = true;
                        }
                        else
                        {
                            mniEdit.DropDownItems[Counter].Enabled = false;
                        }
                    }
                }
            }
            else
            {
                // Currently selected Partner is accessible
                for (Counter = 1; Counter <= mniEdit.DropDownItems.Count - 1; Counter += 1)
                {
                    mniEdit.DropDownItems[Counter].Enabled = true;
                }
            }
        }

        private void MniMailing_DropDownOpening(System.Object sender, System.EventArgs e)
        {
            DataRowView[] GridRows;
            GridRows = ucoFindByPartnerDetails.SelectedDataRowsAsDataRowView;

            // Check if a Grid Row is selected
            if (GridRows.Length <= 0)
            {
                mniMailingGenerateExtract.Enabled = false;
            }
            else
            {
                mniMailingGenerateExtract.Enabled = true;
            }
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
                                MessageText = MPartnerResourcestrings.StrPartnersAddedToExtractText;
                            }
                            else
                            {
                                MessageText = MPartnerResourcestrings.StrPartnersAddedToExtractPluralText;
                            }

                            MessageBox.Show(String.Format(MessageText,
                                    ExtractPartners), MPartnerResourcestrings.StrPartnersAddedToExtractTitle, MessageBoxButtons.OK,
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

        private void MniMaintain_DropDownOpening(System.Object sender, System.EventArgs e)
        {
            DataRowView[] GridRows;
            String PartnerClass = "";
            ToolStripItemCollection MenuItemCollection = mniMaintain.DropDownItems;
            Boolean SenderIsContextMenu = false;

            // Is the Maintain Menu or the ContextMenu calling us?
            if (sender == mniMaintain)
            {
                MenuItemCollection = mniMaintain.DropDownItems;
                SenderIsContextMenu = false;
            }

// TODO Context Menu
//            else
//            {
//                MenuItemCollection = mnuPartnerFindContext.DropDownItems;
//                SenderIsContextMenu = true;
//            }

            GridRows = ucoFindByPartnerDetails.SelectedDataRowsAsDataRowView;

            // Check if a Grid Row is selected
            if (GridRows.Length <= 0)
            {
                // No Row is selected > disable all MenuItems
                foreach (ToolStripItem IndividualMenuItem in MenuItemCollection)
                {
                    IndividualMenuItem.Enabled = false;
                }

                // Only need to work on the Maintain Menu, and not the ContextMenu
                // (ContextMenu can't be activated when there are no Rows)
                mniMaintainFamilyMembers.Text = MPartnerResourcestrings.StrFamilyMembersMenuItemText;
            }
            else
            {
                // A Row is selected.
                // Start off with all MenuItems enabled
                foreach (ToolStripItem IndividualMenuItem in MenuItemCollection)
                {
                    IndividualMenuItem.Enabled = true;

                    if (SenderIsContextMenu)
                    {
                        if (IndividualMenuItem.Text == MPartnerResourcestrings.StrPersonnelUnitMenuItemText)
                        {
                            IndividualMenuItem.Text = MPartnerResourcestrings.StrPersonnelPersonMenuItemText;
                        }
                    }
                }

                mniMaintainPersonnelData.Text = MPartnerResourcestrings.StrPersonnelPersonMenuItemText;

                /*
                 * Set the Maintain Menu and ContextMenu MenuItems up - according to the
                 * Partner Class of Partner of the selected Grid Row.
                 */
                PartnerClass = GridRows[0][PPartnerTable.GetPartnerClassDBName()].ToString();

                if (PartnerClass == "FAMILY")
                {
                    // PERSON needs 'Family Members', but not 'Personnel' MenuItems

                    if (SenderIsContextMenu)
                    {
                        // Work on Context Menu
                        Int32 Counter = 0;

                        foreach (ToolStripItem IndividualMenuItem in MenuItemCollection)
                        {
                            if ((IndividualMenuItem.Text == MPartnerResourcestrings.StrFamilyMenuItemText)
                                || (IndividualMenuItem.Text == mniMaintainPersonnelData.Text))
                            {
                                if (IndividualMenuItem.Text == MPartnerResourcestrings.StrFamilyMenuItemText)
                                {
                                    IndividualMenuItem.Text = MPartnerResourcestrings.StrFamilyMembersMenuItemText;

                                    // Exchange the 'Family' icon with the 'Family Members' icon!
                                    //
// TODO                                                           this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter], imlMenuHelper.Images[1]);
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
                        mniMaintainFamilyMembers.Text = MPartnerResourcestrings.StrFamilyMembersMenuItemText;

                        // Exchange the 'Family' icon with the 'Family Members' icon!
// TODO                   this.XPMenuItemExtender.SetMenuGlyph(this.mniMaintainFamilyMembers, imlMenuHelper.Images[1]);
                        mniMaintainPersonnelData.Text = MPartnerResourcestrings.StrPersonnelPersonMenuItemText;
                        mniMaintainPersonnelData.Enabled = false;
                    }
                }
                else if (PartnerClass == "PERSON")
                {
                    // PERSON needs 'Family' and 'Personnel' MenuItems

                    if (SenderIsContextMenu)
                    {
                        // Work on Context Menu
                        Int32 Counter = 0;

                        foreach (ToolStripItem IndividualMenuItem in MenuItemCollection)
                        {
                            if (IndividualMenuItem.Text == MPartnerResourcestrings.StrFamilyMembersMenuItemText)
                            {
                                IndividualMenuItem.Text = MPartnerResourcestrings.StrFamilyMenuItemText;

                                // Exchange the 'Family Members' icon with the 'Family' icon!
// TODO                           this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter], imlMenuHelper.Images[0]);

                                if (!UserHasPersonnelAccess())
                                {
                                    IndividualMenuItem.Enabled = false;
                                }

                                break;
                            }

                            Counter++;
                        }
                    }
                    else
                    {
                        // Work on Maintain Menu
                        mniMaintainFamilyMembers.Text = MPartnerResourcestrings.StrFamilyMenuItemText;

                        // Exchange the 'Family Members' icon with the 'Family' icon!
// TODO                   this.XPMenuItemExtender.SetMenuGlyph(this.mniMaintainFamilyMembers, imlMenuHelper.Images[0]);

                        if (!UserHasPersonnelAccess())
                        {
                            mniMaintainFamilyMembers.Enabled = false;
                        }
                    }
                }
                else if (PartnerClass == "UNIT")
                {
                    // UNIT doesn't needs 'Family', but 'Personnel' MenuItems

                    if (SenderIsContextMenu)
                    {
                        // Work on Context Menu
                        foreach (ToolStripItem IndividualMenuItem in MenuItemCollection)
                        {
                            if ((IndividualMenuItem.Text == MPartnerResourcestrings.StrFamilyMenuItemText)
                                || (IndividualMenuItem.Text == MPartnerResourcestrings.StrFamilyMembersMenuItemText)
                                || (IndividualMenuItem.Text == MPartnerResourcestrings.StrPersonnelPersonMenuItemText)
                                || (IndividualMenuItem.Text == mniMaintainWorkerField.Text))
                            {
                                if (IndividualMenuItem.Text != MPartnerResourcestrings.StrPersonnelPersonMenuItemText)
                                {
                                    IndividualMenuItem.Enabled = false;
                                }
                                else
                                {
                                    IndividualMenuItem.Text = MPartnerResourcestrings.StrPersonnelUnitMenuItemText;

                                    if (!UserHasPersonnelAccess())
                                    {
                                        IndividualMenuItem.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Work on Maintain Menu
                        mniMaintainFamilyMembers.Enabled = false;
                        mniMaintainWorkerField.Enabled = false;
                        mniMaintainPersonnelData.Text = MPartnerResourcestrings.StrPersonnelUnitMenuItemText;

                        if (!UserHasPersonnelAccess())
                        {
                            mniMaintainPersonnelData.Enabled = false;
                        }
                    }
                }
                else
                {
                    // All other PartnerClasses: disable Family (Members) and Personnel
                    if (SenderIsContextMenu)
                    {
                        // Work on Context Menu
                        foreach (ToolStripItem IndividualMenuItem in MenuItemCollection)
                        {
                            // MessageBox.Show('FAMILY: SenderIsContextMenu: ' + SenderIsContextMenu.ToString);
                            if ((IndividualMenuItem.Text == MPartnerResourcestrings.StrFamilyMenuItemText)
                                || (IndividualMenuItem.Text == MPartnerResourcestrings.StrFamilyMembersMenuItemText)
                                || (IndividualMenuItem.Text == mniMaintainPersonnelData.Text)
                                || (IndividualMenuItem.Text == mniMaintainWorkerField.Text))
                            {
                                if (IndividualMenuItem.Text == MPartnerResourcestrings.StrFamilyMenuItemText)
                                {
                                    IndividualMenuItem.Text = MPartnerResourcestrings.StrFamilyMembersMenuItemText;
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
                        mniMaintainFamilyMembers.Text = MPartnerResourcestrings.StrFamilyMembersMenuItemText;
                        mniMaintainFamilyMembers.Enabled = false;
                        mniMaintainPersonnelData.Enabled = false;
                        mniMaintainWorkerField.Enabled = false;
                    }
                }
            }

            Boolean IsFoundation = false;

            if ((PartnerClass == "ORGANISATION"))
            {
// TODO Foundations              FLogic.DetermineCurrentFoundationStatus(out IsFoundation);
            }

            if (IsFoundation)
            {
                mniMaintainFoundationDetails.Enabled = true;

                // Work on Context Menu
                foreach (ToolStripItem IndividualMenuItem in MenuItemCollection)
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
                foreach (ToolStripItem IndividualMenuItem in MenuItemCollection)
                {
                    if (IndividualMenuItem.Text == mniMaintainFoundationDetails.Text)
                    {
                        IndividualMenuItem.Enabled = false;
                    }
                }
            }

            // Disable 'Local Partner Data' MenuItem if there are no DataLabels for the Partner's PartnerClass available
            if (!Checks.HasPartnerClassLocalPartnerDataLabels(SharedTypes.PartnerClassStringToEnum(PartnerClass),
                    TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.DataLabelsForPartnerClassesList)))
            {
                if (SenderIsContextMenu)
                {
                    /* Work on Context Menu */
                    foreach (ToolStripItem IndividualMenuItem in MenuItemCollection)
                    {
                        if (IndividualMenuItem.Text == mniMaintainLocalPartnerData.Text)
                        {
                            IndividualMenuItem.Enabled = false;
                        }
                    }
                }
                else
                {
                    mniMaintainLocalPartnerData.Enabled = false;
                }
            }
        }

        #region Menu/ToolBar command handling

        private void MniFile_Click(System.Object sender, System.EventArgs e)
        {
            ucoFindByPartnerDetails.HandleMenuItemOrToolBarButton(mniFile, (ToolStripItem)sender, FRunAsModalForm);
        }

        private void MniEdit_Click(System.Object sender, System.EventArgs e)
        {
            ucoFindByPartnerDetails.HandleMenuItemOrToolBarButton(mniEdit, (ToolStripItem)sender, FRunAsModalForm);
        }

        private void MniMaintain_Click(System.Object sender, System.EventArgs e)
        {
            ucoFindByPartnerDetails.HandleMenuItemOrToolBarButton(mniMaintain, (ToolStripItem)sender, FRunAsModalForm);
        }

        private void MniMailing_Click(System.Object sender, System.EventArgs e)
        {
            ucoFindByPartnerDetails.HandleMenuItemOrToolBarButton(mniMailing, (ToolStripItem)sender, FRunAsModalForm);
        }

        private void MniTools_Click(System.Object sender, System.EventArgs e)
        {
            ucoFindByPartnerDetails.HandleMenuItemOrToolBarButton(mniTools, (ToolStripItem)sender, FRunAsModalForm);
        }

        private void MniView_Click(System.Object sender, System.EventArgs e)
        {
            ucoFindByPartnerDetails.HandleMenuItemOrToolBarButton(mniView, (ToolStripItem)sender, FRunAsModalForm);
        }

        #endregion

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

        private bool UserHasPersonnelAccess()
        {
            return UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_PERSONNEL);
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

            if (!FRunAsModalForm)
            {
                /*
                 * The non-Modal ContextMenu contains all MenuItems from the 'Maintain' Menu
                 */

                // Copy over all MenuItems  including their Events (using CloneMenu()) and
                // Icons
                for (Counter1 = 0; Counter1 <= mniMaintain.DropDownItems.Count - 1; Counter1 += 1)
                {
                    mnuPartnerFindContext.DropDownItems.Add(mniMaintain.DropDownItems[Counter1].CloneMenu());
// TODO                    this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter1],
// TODO                        this.XPMenuItemExtender.GetMenuGlyph(mniMaintain.MenuItems[Counter1]));
// TODO                    this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter1], true);
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
// TODO                this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter2],
// TODO                    this.XPMenuItemExtender.GetMenuGlyph(mniFileEditPartner));
// TODO                this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter2], true);
                Counter2++;

                // Separator
                mnuPartnerFindContext.MenuItems.Add("-");
// TODO                this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter2], true);
                Counter2++;

                // Donor History
                mnuPartnerFindContext.MenuItems.Add(mniMaintainDonorHistory.CloneMenu());
// TODO                this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter2],
// TODO                    this.XPMenuItemExtender.GetMenuGlyph(mniMaintainDonorHistory));
// TODO                this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter2], true);
                Counter2++;

                // Recipient History
                mnuPartnerFindContext.MenuItems.Add(mniMaintainRecipientHistory.CloneMenu());
// TODO                this.XPMenuItemExtender.SetMenuGlyph(mnuPartnerFindContext.MenuItems[Counter2],
// TODO                    this.XPMenuItemExtender.GetMenuGlyph(mniMaintainRecipientHistory));
// TODO                this.XPMenuItemExtender.SetNewStyleActive(mnuPartnerFindContext.MenuItems[Counter2], true);
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
            if (FRunAsModalForm)
            {
                // Hide the menu items that are in "HiddenItems"
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
                HiddenItems.Add("mniMaintainLocalPartnerData");
                HiddenItems.Add("mniMaintainWorkerField");
                HiddenItems.Add("mniMaintainPersonnelData");
                HiddenItems.Add("mniMaintainFinanceDetails");
                HiddenItems.Add("mniSeparator0");
                HiddenItems.Add("mniSeparator1");

                for (int Counter = 0; Counter < mniMaintain.DropDownItems.Count; ++Counter)
                {
                    if (HiddenItems.Contains(mniMaintain.DropDownItems[Counter].Name))
                    {
                        mniMaintain.DropDownItems[Counter].Visible = false;
                    }
                    else
                    {
                        mniMaintain.DropDownItems[Counter].Visible = true;
                    }
                }
            }
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
            if (FRunAsModalForm)
            {
                // Hide the menu items that are in "HiddenItems"
                ArrayList HiddenItems = new ArrayList();
                HiddenItems.Add("mniFileWorkWithLastPartner");
                HiddenItems.Add("mniFileRecentPartners");
                HiddenItems.Add("mniFileSeparator1");
                HiddenItems.Add("mniFileNewPartner");
                HiddenItems.Add("mniFileMergePartners");
                HiddenItems.Add("mniFileDeletePartner");
                HiddenItems.Add("mniFilePrintPartner");
                HiddenItems.Add("mniFileSeparator4");

                for (int Counter = 0; Counter < mniFile.DropDownItems.Count; ++Counter)
                {
                    if (HiddenItems.Contains(mniFile.DropDownItems[Counter].Name))
                    {
                        mniFile.DropDownItems[Counter].Visible = false;
                    }
                    else
                    {
                        mniFile.DropDownItems[Counter].Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Makes a server call to get the recently used partners
        /// and displays them on the menu.
        /// </summary>
        private void SetupRecentlyUsedPartnersMenu()
        {
            Dictionary <long, string>RecentlyUsedPartners;
            ArrayList PartnerClasses = new ArrayList();

            PartnerClasses.Add("*");

            int MaxPartnersCount = TUserDefaults.GetInt16Default(MSysManConstants.USERDEFAULT_NUMBEROFRECENTPARTNERS, 10);
            TServerLookup.TMPartner.GetRecentlyUsedPartners(MaxPartnersCount, PartnerClasses, out RecentlyUsedPartners);

            int Counter = 0;

            foreach (KeyValuePair <long, string>CurrentEntry in RecentlyUsedPartners)
            {
                mniFileRecentPartners.DropDownItems[Counter].Text = Counter.ToString() + " - " + CurrentEntry.Value;
                mniFileRecentPartners.DropDownItems[Counter].Tag = CurrentEntry.Key;
                mniFileRecentPartners.DropDownItems[Counter].Enabled = true;
                mniFileRecentPartners.DropDownItems[Counter].Visible = true;

                ++Counter;
            }

            // If there are less partners than menu items, then disable them
            for (; Counter < mniFileRecentPartners.DropDownItems.Count; ++Counter)
            {
                mniFileRecentPartners.DropDownItems[Counter].Enabled = false;
                mniFileRecentPartners.DropDownItems[Counter].Visible = false;
            }

            // If there are no recently used partners at all show a message
            if (RecentlyUsedPartners.Count == 0)
            {
                mniFileRecentPartners.DropDownItems[0].Text = "No partners used yet";
                mniFileRecentPartners.DropDownItems[0].Tag = -1;
                mniFileRecentPartners.DropDownItems[0].Enabled = false;
                mniFileRecentPartners.DropDownItems[0].Visible = true;
            }
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

        #region Screen Parameters

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
                //
                // Only one entry
                //
                if (FRestrictToPartnerClasses[0] != "")
                {
                    if (FRestrictToPartnerClasses[0] != "WORKER-FAM")
                    {
                        //
                        // Set the title, with the first character of the word
                        // being upper-case, the rest lower-case.
                        //
                        this.Text = FRestrictToPartnerClasses[0].Substring(0, 1).ToUpper() +
                                    FRestrictToPartnerClasses[0].Substring(1).ToLower() + StrTitleLastPart;
                    }
                    else
                    {
                        this.Text = StrWorkerFamily + StrTitleLastPart;
                    }
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

        #endregion


        #region Form events

        private void TPartnerFindScreen_Load(System.Object sender, System.EventArgs e)
        {
            FPetraUtilsObject.TFrmPetra_Load(sender, e);

            this.Cursor = Cursors.WaitCursor;

            // Restore Window Position and Size
            // TODO TUserDefaults.NamedDefaults.GetWindowPositionAndSize(this, WINDOWSETTINGSDEFAULT_NAME);
#if TODO
            // Set up Splitter distances
            ucoPartnerFindCriteria.RestoreSplitterSetting();
            RestoreSplitterSettings();
#endif

            FRunAsModalForm = this.Modal;
            ucoFindByPartnerDetails.RunnningInsideModalForm = FRunAsModalForm;

            SetupGridContextMenu();
            SetupFileMenu();
            SetupMaintainMenu();

            if (!FRunAsModalForm)
            {
                pnlModalButtons.Visible = false;

                //                grdResult.Height = grdResult.Height + 24;
                //                pnlBlankSearchResult.Height = pnlBlankSearchResult.Height + 24;
            }
            else
            {
                this.mnuMain.Visible = false;  // Modal Dialogs don't have menus
                pnlModalButtons.Visible = true;
                pnlModalButtons.SendToBack();
                pnlModalButtons.AutoScroll = false;

                // Modify auto-generated appearance of Panels
                pnlModalButtons.Height = 33;
                pnlAcceptCancelButtons.Location = new System.Drawing.Point(this.Width - pnlAcceptCancelButtons.Width - 10, 0);

                // Modify auto-generated appearance of Buttons
                btnHelp.Location = new System.Drawing.Point(btnHelp.Location.X, 5);
                btnAccept.Location = new System.Drawing.Point(btnAccept.Location.X, 5);
                btnCancel.Location = new System.Drawing.Point(btnCancel.Location.X, 5);
                btnHelp.Height = 23;
                btnAccept.Height = 23;
                btnCancel.Height = 23;

                // Modify automatic TabIndexes so that the Help Button comes last
                btnHelp.TabIndex = 1;
                pnlAcceptCancelButtons.TabIndex = 0;
            }

#if TODO
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
            
            if (TClientSettings.AutoTestParameters.Contains("run_randomfind"))
            {
                ucoFindByPartnerDetails.SetupRandomTestSearchCriteriaAndRunSearch();
            }                        
        }

        private void TPartnerFindScreen_Closed(System.Object sender, System.EventArgs e)
        {
            ucoFindByPartnerDetails.StoreUserDefaults();

            // Save Window Position and Size
            // TODO? TUserDefaults.NamedDefaults.SetWindowPositionAndSize(this, WINDOWSETTINGSDEFAULT_NAME);

            ReleaseServerObject();

            // Stop the Timer for the fetching of data for the Partner Info Panel (necessary for a Garbage Collection of this Form!)
            ucoFindByPartnerDetails.StopTimer();
        }

        #endregion


        #region Event Handlers

        void ucoFindByPartnerDetails_PartnerInfoPaneExpanded(object sender, EventArgs e)
        {
            tbbPartnerInfo.Checked = true;
            mniViewPartnerInfo.Checked = true;
        }

        void ucoFindByPartnerDetails_PartnerInfoPaneCollapsed(object sender, EventArgs e)
        {
            tbbPartnerInfo.Checked = false;
            mniViewPartnerInfo.Checked = false;
        }

        void ucoFindByPartnerDetails_EnableAcceptButton(object sender, EventArgs e)
        {
            btnAccept.Enabled = true;
        }

        void ucoFindByPartnerDetails_DisableAcceptButton(object sender, EventArgs e)
        {
            btnAccept.Enabled = false;
        }

        private void BtnFullyLoadData_Click(System.Object sender, System.EventArgs e)
        {
            // FullLoadDataSet;
        }

        #endregion


        #region Helper Methods

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

        #endregion
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
        /// <param name="AParentForm"></param>
        /// <returns>void</returns>
        public static void OpenNewOrExistingForm(Form AParentForm)
        {
            bool FormWasAlreadyOpened;

            OpenNewOrExistingForm(out FormWasAlreadyOpened, AParentForm);
        }

        /// <summary>
        /// Manages the opening of a new/showing of an existing Instance of the Partner Find Screen.
        /// </summary>
        /// <remarks>A call to this Method will create a new Instance of the Partner Find Screen
        /// if there was no running Instance, otherwise it will just activate any Instance of
        /// the Partner Find Screen if finds.</remarks>
        /// <param name="AFormWasAlreadyOpened">False if a new Partner Find Screen was opened,
        /// false if an existing Instance of the Partner Find Screen was activated.</param>
        /// <param name="AParentForm"></param>
        /// <returns>An Instance of the Partner Find Screen (either newly created or
        /// just activated).</returns>
        public static Form OpenNewOrExistingForm(out bool AFormWasAlreadyOpened, Form AParentForm)
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
                NewFindScreen = new TPartnerFindScreen(AParentForm);
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
        /// <param name="AParentForm"></param>
        /// <returns>True if a Partner was found and accepted by the user,
        /// otherwise false.</returns>
        public static bool OpenModalForm(String ARestrictToPartnerClasses,
            out Int64 APartnerKey,
            out String AShortName,
            out TLocationPK ALocationPK,
            Form AParentForm)
        {
            TPartnerFindScreen PartnerFindForm;
            DialogResult dlgResult;

            APartnerKey = -1;
            AShortName = String.Empty;
            ALocationPK = new TLocationPK(-1, -1);

            PartnerFindForm = new TPartnerFindScreen(AParentForm);
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


    /// <summary>
    /// Event Arguments
    /// </summary>
    public class TPartnerAvailableEventArgs : System.EventArgs
    {
        private bool FPartnerAvailable;

        /// <summary>
        /// todoComment
        /// </summary>
        public bool PartnerAvailable
        {
            get
            {
                return FPartnerAvailable;
            }

            set
            {
                FPartnerAvailable = value;
            }
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <returns>void</returns>
        public TPartnerAvailableEventArgs() : base()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerAvailable"></param>
        public TPartnerAvailableEventArgs(bool APartnerAvailable) : base()
        {
            FPartnerAvailable = APartnerAvailable;
        }
    }

    /// <summary>
    /// Event Arguments
    /// </summary>
    public class TSearchOperationStateChangeEventArgs : System.EventArgs
    {
        private bool FSearchOperationIsRunning;

        /// <summary>
        /// todoComment
        /// </summary>
        public bool SearchOperationIsRunning
        {
            get
            {
                return FSearchOperationIsRunning;
            }

            set
            {
                FSearchOperationIsRunning = value;
            }
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <returns>void</returns>
        public TSearchOperationStateChangeEventArgs() : base()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASearchOperationIsRunning"></param>
        public TSearchOperationStateChangeEventArgs(bool ASearchOperationIsRunning) : base()
        {
            FSearchOperationIsRunning = ASearchOperationIsRunning;
        }
    }
}