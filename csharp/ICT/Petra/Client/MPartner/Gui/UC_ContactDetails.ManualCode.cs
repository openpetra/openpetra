//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Specialized;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_ContactDetails
    {
        private enum TTimerDrivenMessageBoxKind
        {
            tdmbkNoPrimaryEmailAsNoCurrentAvailable,

            tdmbkNoPrimaryEmailButNonCurrentAvailable
        }

        private readonly string StrDefaultContactType = Catalog.GetString("Phone");

        private string StrPrimEmailConsequence = Catalog.GetString(
            "Please select a current E-mail Address from the available ones!");
        private string StrPrimEmailMessageBoxTitle = Catalog.GetString("Primary E-Mail Address Needs Adjusting");
        
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private string FDefaultContactType = String.Empty;

        private TPartnerAttributeTypeValueKind FValueKind = TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL;

        System.Windows.Forms.Timer ShowMessageBoxTimer = new System.Windows.Forms.Timer();

        private TTimerDrivenMessageBoxKind FTimerDrivenMessageBoxKind;

        private bool FRunningInsideShowDetails = false;
        
        private bool FSuppressOnContactTypeChangedEvent = false;
        
        private bool FPrimaryEmailSelectedValueChangedEvent = false;
        
        /// <summary>
        /// Usage: see Methods <see cref="PreDeleteManual"/> and <see cref="PostDeleteManual"/>. 
        /// </summary>
        private string FDeletedRowsAttributeType = String.Empty;
        
        /// <summary>
        /// Usage: see Methods <see cref="PreDeleteManual"/> and <see cref="PostDeleteManual"/>. 
        /// </summary>
        private string FDeletedRowsValue = String.Empty;
        
        /// <summary>
        /// Populated by Method <see cref="DetermineEmailPartnerAttributeTypes"/>.
        /// </summary>
        string FEmailAttributesConcatStr = String.Empty;

        #region Properties

        /// <summary>used for passing through the Clientside Proxy for the UIConnector</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }

        #endregion

        #region Events

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #endregion

        /// <summary>todoComment</summary>

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// This Procedure will get called from the SaveChanges procedure before it
        /// actually performs any saving operation.
        /// </summary>
        /// <param name="sender">The Object that throws this Event</param>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        private void DataSavingStarted(System.Object sender, System.EventArgs e)
        {
            // TODO DataSavingStarted
            // Do not call this method in your manual code.
            // This is a method that is private to the generated code and is part of the Validation process.
            // If you need to update the controls data into the Data Row object, you must use ValidateAllData and be prepared
            //   to handle the consequences of a failed validation.
//            GetDetailsFromControls(GetSelectedDetailRow());
        }

        #region Public Methods

        /// <summary>todoComment</summary>
        public void PostInitUserControl(PartnerEditTDS AMainDS)
        {
            DataRow[] DefaultContactTypes;

            FMainDS = AMainDS;

            // disable change event while controls are being initialized as otherwise save button might get enabled
            FPetraUtilsObject.DisableDataChangedEvent();

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);

            // enable grid to react to insert and delete keyboard keys
            grdDetails.InsertKeyPressed += new TKeyPressedEventHandler(grdDetails_InsertKeyPressed);

            // Create custom data columns on-the-fly
            CreateCustomDataColumns();

            /* Create SourceDataGrid columns */
            CreateGridColumns();

            /* Setup the DataGrid's visual appearance */
//            SetupDataGridVisualAppearance();

            // Set up special sort order of Rows in Grid:
            // PPartnerAttributeCategory.Index followed by PPartnerAttributeType.Index followed by PPartnerAttribute.Index!
            DataView gridView = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;
            gridView.Sort = "Parent_Parent_CategoryIndex ASC, Parent_AttributeIndex ASC, " +
                            PPartnerAttributeTable.GetIndexDBName() + " ASC";

            string FilterStr = String.Format("{0}='{1}'", PartnerEditTDSPPartnerAttributeTable.GetPartnerContactDetailDBName(), true);

            FFilterAndFindObject.FilterPanelControls.SetBaseFilter(FilterStr, true);
            FFilterAndFindObject.ApplyFilter();

            if (grdDetails.Rows.Count > 1)
            {
                grdDetails.SelectRowInGrid(1);
            }
            else
            {
                MakeDetailsInvisible(true);
                btnDelete.Enabled = false;
            }

            // now changes to controls can trigger enabling of save button again
            FPetraUtilsObject.EnableDataChangedEvent();


            // Try to find a Contact Type that matches the default; if found, new Records are defaulting to that Contact Type.
            DefaultContactTypes = FMainDS.PPartnerAttributeType.Select(
                PPartnerAttributeTypeTable.GetAttributeTypeDBName() + " = '" + StrDefaultContactType + "'");

            if (DefaultContactTypes.Length > 0)
            {
                FDefaultContactType = StrDefaultContactType;
            }
            else
            {
                if (FMainDS.PPartnerAttributeType.Count > 0)
                {
                    // The Contact Type that should be Default wasn't found, therefore use the first Contact Type of the first Contact Category
                    DataView SortedPartnerAttr = new DataView(FMainDS.PPartnerAttributeType, String.Empty,
                        "CategoryIndex ASC, " + PPartnerAttributeTypeTable.GetIndexDBName() + " ASC", DataViewRowState.CurrentRows);
                    FDefaultContactType = ((PPartnerAttributeTypeRow)SortedPartnerAttr[0].Row).AttributeType;
                }
            }

            // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
            // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//            rtbValue.LinkClicked += new Ict.Common.Controls.TRtbHyperlinks.THyperLinkClickedArgs(rtbValue.Helper.LaunchHyperLink);

            // TODO ApplySecurity();
        }

        private void CreateFilterFindPanelsManual()
        {
            // By default only 'current' Contact Details should be shown
// TODO Make initial checking of 'Current' Filter CheckBox work          ((CheckBox)FFilterAndFindObject.FilterPanelControls.FindControlByClonedFrom(chkCurrent)).Checked = true;
        }

        /// <summary>
        /// Performs necessary actions to make the Merging of rows that were changed on
        /// the Server side into the Client-side DataSet possible.
        /// </summary>
        /// <returns>void</returns>
        public void CleanupRecordsBeforeMerge()
        {
            DataView NewPartnerAttributesDV;

            List <DataRow>PPartnerAttributeDeleteRows = new List <DataRow>();

            /*
             * Check if PartnerAttributes have been added
             * -> remove them on the Client side, otherwise we will end up with these rows
             *   (having Sequence values below 0) plus the rows coming from the Server
             *   (having Sequence values that were determined by a Sequence) after merging
             */
            NewPartnerAttributesDV = new DataView(FMainDS.PPartnerAttribute, "", "", DataViewRowState.Added);

            // First check and remember affected DataRows
            for (int Counter = 0; Counter <= NewPartnerAttributesDV.Count - 1; Counter += 1)
            {
                PPartnerAttributeDeleteRows.Add(NewPartnerAttributesDV[Counter].Row);
            }

            // Now remove affected DataRows
            foreach (DataRow DeleteRow in PPartnerAttributeDeleteRows)
            {
                NewPartnerAttributesDV.Table.Rows.Remove(DeleteRow);
            }
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
            if (!FMainDS.Tables.Contains(PPartnerAttributeTable.GetTableName()))
            {
                FMainDS.Tables.Add(new PartnerEditTDSPPartnerAttributeTable());
                FMainDS.InitVars();
            }

            // Set up Timer that is needed for showing MessageBoxes from a Grid Event
            ShowMessageBoxTimer.Tick += new EventHandler(ShowTimerDrivenMessageBox);
            ShowMessageBoxTimer.Interval = 500;

            //
            // Ensure we have instances of PPartnerAttributeCategoryTable and PPartnerAttributeTypeTable in FMainDS. They are needed because
            // the Grid's underlying DataTable has got custom DataColumns with Expressions that reference those DataTables in the DataSet!
            //
            // Note 1: When an existing Partner gets opened, FMainDS does not contain instances of PPartnerAttributeCategoryTable or PPartnerAttributeTypeTable
            // hence we add them throught he following code
            // Note 2: When a new Partner gets created, FMainDS contains instances of PPartnerAttributeCategoryTable and PPartnerAttributeTypeTable hence they will
            // not be created by the following code
            if (!FMainDS.Tables.Contains(PPartnerAttributeCategoryTable.GetTableName())
                && (!FMainDS.Tables.Contains(PPartnerAttributeTypeTable.GetTableName())))
            {
                FMainDS.Tables.Add(new PPartnerAttributeCategoryTable(PPartnerAttributeCategoryTable.GetTableName()));
                FMainDS.Tables.Add(new PPartnerAttributeTypeTable(PPartnerAttributeTypeTable.GetTableName()));
                FMainDS.InitVars();
            }

            if (FMainDS.PPartnerAttributeCategory.Count == 0)
            {
                // Note: If FMainDS contains an instance of the PPartnerAttributeCategoryTable, but it hasn't got any rows
                // we add them here from the corresponding Cacheable DataTable (that is also the case when a new Partner gets created)
                FMainDS.Merge((PPartnerAttributeCategoryTable)TDataCache.TMPartner.GetCacheablePartnerTable2(TCacheablePartnerTablesEnum.
                        ContactCategoryList, PPartnerAttributeCategoryTable.GetTableName()));

                if (FMainDS.PPartnerAttributeCategory.Count == 0)
                {
                    MessageBox.Show(Catalog.GetString(
                            "There are no Partner Contact Categories available. Due to this, this Tab will not work correctly!\r\n\r\nPlease set up at least one Partner Contact Category!"),
                        Catalog.GetString("Partner Contact Details Tab: Not Functional"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (FMainDS.PPartnerAttributeType.Count == 0)
            {
                // Note: If FMainDS contains an instance of the PPartnerAttributeTypeTable, but it hasn't got any rows
                // we add them here from the corresponding Cacheable DataTable (that is also the case when a new Partner gets created)
                FMainDS.Merge((PPartnerAttributeTypeTable)TDataCache.TMPartner.GetCacheablePartnerTable2(TCacheablePartnerTablesEnum.ContactTypeList,
                        PPartnerAttributeTypeTable.GetTableName()));

                if (FMainDS.PPartnerAttributeType.Count == 0)
                {
                    MessageBox.Show(Catalog.GetString(
                            "There are no Partner Contact Types available. Due to this, this Tab will not work correctly!\r\n\r\nPlease set up at least one Partner Contact Type!"),
                        Catalog.GetString("Partner Contact Details Tab: Not Functional"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Need to enable Relations as the PPartnerAttributeCategoryTable and PPartnerAttributeTypeTable have not been part of the PartnerEditTDS when transferred from the OpenPetra Server!
            // These Relations are needed in Method 'CreateCustomDataColumns'.
            FMainDS.EnableRelation("ContactDetails1");
            FMainDS.EnableRelation("ContactDetails2");

            DetermineEmailPartnerAttributeTypes();

            // Show the 'Within The Organisation' GroupBox only if the Partner is of Partner Class PERSON
            if (FMainDS.PPartner[0].PartnerClass != SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
            {
                grpWithinTheOrganisation.Visible = false;
            }

            // Move the 'Within the Organsiation' GroupBox a bit up from it's automatically assigned position
            grpWithinTheOrganisation.Top = 16;

            // Move the Panel that groups the 'Current' Controls for layout purposes a bit up from it's automatically assigned position
            pnlCurrentGrouping.Top = 53;
            chkCurrent.Top = 7;
            dtpNoLongerCurrentFrom.Top = 4;
            lblNoLongerCurrentFrom.Top = 9;

            chkConfidential.Top = 88;
            lblConfidential.Top = 93;


            // Set up status bar texts for unbound controls and for bound controls whose auto-assigned texts don't match the use here on this screen (these talk about 'Partner Attributes')
            FPetraUtilsObject.SetStatusBarText(cmbPrimaryWayOfContacting,
                Catalog.GetString("Select the primary method by which the Partner should be contacted. Purely for information."));
            FPetraUtilsObject.SetStatusBarText(cmbPrimaryPhoneForContacting,
                Catalog.GetString("Select one of the Partner's telephone numbers. Purely for information."));
            FPetraUtilsObject.SetStatusBarText(cmbPrimaryEMail,
                Catalog.GetString(
                    "Select one of the Partner's e-mail addresses. This will be used whenever an automated e-mail is to be sent to this Partner."));
            FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkPrefEMail,
                Catalog.GetString("Click this button to send an email to the Partner's Primary E-mail address."));
            FPetraUtilsObject.SetStatusBarText(cmbPhoneWithinTheOrganisation,
                Catalog.GetString(
                    "Select one of the Partner's telephone numbers to designate it as her/his telephone number within The Organisation."));
            FPetraUtilsObject.SetStatusBarText(cmbEMailWithinTheOrganisation,
                Catalog.GetString("Select one of the Partner's e-mail addresses to designate it as her/his e-mail address within The Organisation."));
            FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkEMailWithinOrg,
                Catalog.GetString("Click this button to send an email to the Partner's Office E-mail address."));

            FPetraUtilsObject.SetStatusBarText(btnPromote,
                Catalog.GetString("Click this button to re-arrange a contact detail record within records of the same Contact Type."));
            FPetraUtilsObject.SetStatusBarText(btnDemote,
                Catalog.GetString("Click this button to re-arrange a contact detail record within records of the same Contact Type."));

            FPetraUtilsObject.SetStatusBarText(cmbContactCategory,
                Catalog.GetString("Contact Category to which the Contact Type belongs to (narrows down available Contact Types)."));
            FPetraUtilsObject.SetStatusBarText(cmbContactType,
                Catalog.GetString("Describes what the Value is (e.g. Phone Number, E-Mail Address, etc)."));
            FPetraUtilsObject.SetStatusBarText(chkSpecialised,
                Catalog.GetString("Tick this if the Value designates a business-related Contact Detail (e.g. business telephone number)."));

            FPetraUtilsObject.SetStatusBarText(txtComment, Catalog.GetString("Comment for this Contact Detail record."));
            FPetraUtilsObject.SetStatusBarText(chkCurrent, Catalog.GetString("Untick this if the Contact Detail record is no longer current."));
            FPetraUtilsObject.SetStatusBarText(dtpNoLongerCurrentFrom,
                Catalog.GetString("Date from which the Contact Detail record is no longer current."));

            FPetraUtilsObject.SetStatusBarText(chkConfidential,
                Catalog.GetString(
                    "Tick this if the Contact Detail record is confidential. Please refer to the User Guide what effect this setting has!"));

            // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
            // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
            FPetraUtilsObject.SetStatusBarText(txtValue,
                Catalog.GetString("Phone Number, Mobile Phone Number, E-mail Address, Internet Address, ... --- whatever the Contact Type is about."));
//            FPetraUtilsObject.SetStatusBarText(rtbValue, Catalog.GetString("Phone Number, Mobile Phone Number, E-mail Address, Internet Address, ... --- whatever the Contact Type is about."));

            // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
            // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//            rtbValue.BuildLinkWithValue = BuildLinkWithValue;


            // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
            // Hide all Controls in the 'Overall Contact Settings' GroupBox except 'Primary E-Mail' for the time being - their implementation will follow
            cmbPrimaryWayOfContacting.Visible = false;
            lblPrimaryWayOfContacting.Visible = false;
            cmbPrimaryPhoneForContacting.Visible = false;
            lblPrimaryPhoneForContacting.Visible = false;
            grpWithinTheOrganisation.Visible = false;
            pnlPromoteDemote.Visible = false;
        }

        private void ShowDataManual()
        {
            FPrimaryEmailSelectedValueChangedEvent = true;
            
            UpdatePrimaryEmailComboItems(false);
            
            FPrimaryEmailSelectedValueChangedEvent = false;
        }

        private void ShowDetailsManual(PPartnerAttributeRow ARow)
        {
//            long RelationPartnerKey;

            // show controls if not visible yet
            MakeDetailsInvisible(false);

            btnDelete.Enabled = false;

            if (ARow != null)
            {
                btnDelete.Enabled = true;
            }

            OnContactTypeChanged(null, null);
            
            FRunningInsideShowDetails = false;
        }

        /// <summary>
        /// Manual way for getting the data that is contained in the 'Overall Contact Settings' GroupBox' Control.
        /// </summary>
        /// <returns>True if successful.</returns>
        public bool GetOverallContactSettingsDataFromControls()
        {
            return UpdatePrimaryEmailAddressRecords(true);
        }

        /// <summary>
        /// Updates the records to reflect the 'Primary E-mail Address' setting.
        /// </summary>
        /// <param name="ARunValidation">If set to true, various validations are run against
        /// the 'Primary E-mail Address' setting.</param>
        /// <returns>True if <paramref name="ARunValidation"/> is false, or when
        /// <paramref name="ARunValidation"/> is true and validation succeeds, otherwise false.</returns>
        public bool UpdatePrimaryEmailAddressRecords(bool ARunValidation)
        {
            bool ReturnValue = true;
            string PrimaryEmailChoice;
            bool PrimaryEmailChoiceFoundAmongEligbleEmailAddr = false;
            DataView ElegibleEmailAddrsDV = DeterminePartnersEmailAddresses(false);

            PrimaryEmailChoice = cmbPrimaryEMail.GetSelectedString();

            if (PrimaryEmailChoice != String.Empty)
            {
                for (int Counter = 0; Counter < ElegibleEmailAddrsDV.Count; Counter++)
                {
                    var TheEmailPartnerAttributeRow = ((PPartnerAttributeRow)ElegibleEmailAddrsDV[Counter].Row);

                    // Modify Rows only as necessary
                    if (TheEmailPartnerAttributeRow.Primary)
                    {
                        if (TheEmailPartnerAttributeRow.Value != PrimaryEmailChoice)
                        {
                            TheEmailPartnerAttributeRow.Primary = false;
                        }
                    }
                    else
                    {
                        if (TheEmailPartnerAttributeRow.Value == PrimaryEmailChoice)
                        {
                            TheEmailPartnerAttributeRow.Primary = true;
                        }
                    }
                }
            }
            else
            {
                for (int Counter2 = 0; Counter2 < ElegibleEmailAddrsDV.Count; Counter2++)
                {
                    var TheEmailPartnerAttributeRow = ((PPartnerAttributeRow)ElegibleEmailAddrsDV[Counter2].Row);

                    // Modify Rows only as necessary
                    if (TheEmailPartnerAttributeRow.Primary)
                    {
                        TheEmailPartnerAttributeRow.Primary = false;
                    }
                }
            }
    
            if (ARunValidation) 
            {
                DataView CurrentEmailAddrsDV = DeterminePartnersEmailAddresses(true);
                
                if (CurrentEmailAddrsDV.Count != 0)
                {
                    if (PrimaryEmailChoice == String.Empty)
                    {
                        // Generate a Validation *Warning*, not an error. The user can ignore this if (s)he chooses to do so!
                        ValidationPrimaryEmailAddrNotSet();    
                    }
                    else
                    {
                        for (int Counter3 = 0; Counter3 < ElegibleEmailAddrsDV.Count; Counter3++) 
                        {
                            var TheEmailPartnerAttributeRow = ((PPartnerAttributeRow)ElegibleEmailAddrsDV[Counter3].Row);
                            
                            if (TheEmailPartnerAttributeRow.Value == PrimaryEmailChoice) 
                            {
                                PrimaryEmailChoiceFoundAmongEligbleEmailAddr = true;
    
                                if (!TheEmailPartnerAttributeRow.Current)
                                {              
                                    // This condition should not occur, unless the program code that runs when the 'Valid'
                                    // CheckBox is disabled and which should clear all the Items from cmbPrimaryEMail is 
                                    // somehow not working correctly, or not run at all. This condition is therefore a 'back-stop' 
                                    // that will prevent invalid data going to the DB!
                                    
                                    // Generate a Validation *Error*. The user cannot ignore this.
                                    ValidationPrimaryEmailAddrSetButItIsntCurrent();
                                    
                                    UpdatePrimaryEmailComboItems(false);
                                    
                                    ReturnValue = false;                                        
                                }    
                            }                        
                        }    
                        
                        if (!PrimaryEmailChoiceFoundAmongEligbleEmailAddr) 
                        {
                            // This condition should not occur, unless various bits of program code are somehow 
                            // not working correctly, or not run at all. This condition is therefore a 'back-stop' 
                            // that will prevent invalid data going to the DB!
                            
                            // Generate a Validation *Error*. The user cannot ignore this.
                            ValidationPrimaryEmailAddrSetButNotAmongEmailAddrs();
                            
                            UpdatePrimaryEmailComboItems(true);
                            
                            ReturnValue = false;                                                                    
                        }
                    }
                }
                else
                {
                    if (PrimaryEmailChoice != String.Empty)
                    {
                        // This condition should not occur, unless various bits of program code are somehow 
                        // not working correctly, or not run at all. This condition is therefore a 'back-stop' 
                        // that will prevent invalid data going to the DB!
    
                            // Generate a Validation *Error*. The user cannot ignore this.
                        ValidationPrimaryEmailAddrSetButNoEmailAddrAvailable();
    
                        UpdatePrimaryEmailComboItems(true);                    
                        
                        ReturnValue = false;                    
                    }
                }
            }
            
            return ReturnValue;
        }
        
        private DataView DetermineEmailAttributes()
        {
            return new DataView(FMainDS.PPartnerAttributeType,
                String.Format(PPartnerAttributeTypeTable.GetAttributeTypeValueKindDBName() + " = '{0}' AND " +
                               PPartnerAttributeTypeTable.GetUnassignableDBName() + " = false", TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS),
                "", DataViewRowState.CurrentRows);
        }

        private DataView DeterminePartnersEmailAddresses(bool AOnlyCurrentEmailAddresses)
        {
            string CurrentCriteria = AOnlyCurrentEmailAddresses ? PPartnerAttributeTable.GetCurrentDBName() + " = true AND " : String.Empty;

            if (FEmailAttributesConcatStr.Length > 0) 
            {
                return new DataView(FMainDS.PPartnerAttribute,
                    CurrentCriteria +
                    String.Format(PPartnerAttributeTable.GetAttributeTypeDBName() + " IN ({0})",
                        FEmailAttributesConcatStr),
                    PPartnerAttributeTable.GetIndexDBName() + " ASC", DataViewRowState.CurrentRows);                
            }
            else
            {
                return new DataView();
            }
        }

        /// <summary>
        /// Determines all Partner Attribute Types that are of Partner Attribute Category 'E-mail' and
        /// populates FEmailAttributesConcatStr with the result.
        /// </summary>
        private void DetermineEmailPartnerAttributeTypes()
        {
            string EmailAttributesConcatStr = String.Empty;

            DataView EmailAttributesDV = DetermineEmailAttributes();

            for (int Counter = 0; Counter < EmailAttributesDV.Count; Counter++)
            {
                EmailAttributesConcatStr += ((PPartnerAttributeTypeRow)EmailAttributesDV[Counter].Row).AttributeType + "', '";
            }

            if (EmailAttributesConcatStr.Length > 0) 
            {
                FEmailAttributesConcatStr = "'" + EmailAttributesConcatStr.Substring(0, EmailAttributesConcatStr.Length - 3);    
            }            
        }

        private void UpdatePrimaryEmailComboItems(bool ASuppressMessages, string ANewEmailAddressValue = null)
        {
            object[] PrimaryEmailAddresses;
            string ThePrimaryEmailAddress = String.Empty;
            DataView ElegibleEmailAddrsDV;
            DataView AllEmailAddrsDV;
            string CurrentlySelectedEmailAddr = cmbPrimaryEMail.GetSelectedString(-1);
            
            // Determine all Partner Attributes that have a Partner Attribute Type that constitutes an E-Mail
            // and that are Current.
            ElegibleEmailAddrsDV = DeterminePartnersEmailAddresses(true);

            PrimaryEmailAddresses = new object[ElegibleEmailAddrsDV.Count + 1];
            PrimaryEmailAddresses[0] = String.Empty;

            for (int Counter = 0; Counter < ElegibleEmailAddrsDV.Count; Counter++)
            {
                var TheEmailRow = ((PPartnerAttributeRow)ElegibleEmailAddrsDV[Counter].Row);

                PrimaryEmailAddresses[Counter + 1] = TheEmailRow.Value;

                if (TheEmailRow.Primary)
                {
                    ThePrimaryEmailAddress = TheEmailRow.Value;
                }
            }

            // Add the avilable E-Mail addresses to the ComboBox
            cmbPrimaryEMail.Items.Clear();
            cmbPrimaryEMail.Items.AddRange(PrimaryEmailAddresses);

            // Select the Primay Email Address in the ComboBox
            if (ThePrimaryEmailAddress != String.Empty)
            {
                FPrimaryEmailSelectedValueChangedEvent = true;
                
                cmbPrimaryEMail.SetSelectedString(ThePrimaryEmailAddress);
                
                FPrimaryEmailSelectedValueChangedEvent = false;
            }
            else
            {
                CurrentlySelectedEmailAddr = ANewEmailAddressValue ?? CurrentlySelectedEmailAddr;    

                cmbPrimaryEMail.SetSelectedString(CurrentlySelectedEmailAddr);
                
                if (!ASuppressMessages) 
                {
                    if (ElegibleEmailAddrsDV.Count > 0)
                    {
                        FTimerDrivenMessageBoxKind = TTimerDrivenMessageBoxKind.tdmbkNoPrimaryEmailAsNoCurrentAvailable;
                        ShowMessageBoxTimer.Start();
                    }
                    else
                    {
                        AllEmailAddrsDV = DeterminePartnersEmailAddresses(false);
    
                        if (AllEmailAddrsDV.Count > 0)
                        {
                            FTimerDrivenMessageBoxKind = TTimerDrivenMessageBoxKind.tdmbkNoPrimaryEmailButNonCurrentAvailable;
                            ShowMessageBoxTimer.Start();
                        }
                    }                    
                }
            }
        }

        private void BeforeShowDetailsManual(PPartnerAttributeRow ARow)
        {
            FRunningInsideShowDetails = true;
            
            if (ARow != null)
            {
                btnDelete.Enabled = true;

                if (FMainDS.PPartnerAttributeType != null)
                {
                    DataRow[] ParnterAttributeRow = FMainDS.PPartnerAttributeType.Select(
                        PPartnerAttributeTypeTable.GetAttributeTypeDBName() + " = " + "'" + ARow.AttributeType + "'");

                    if (ParnterAttributeRow.Length > 0)
                    {
                        SelectCorrespondingCategory((PPartnerAttributeTypeRow)ParnterAttributeRow[0]);
                    }

//                    cmbContactType.SetSelectedString(ARow.AttributeType, -1);
                }
            }
            else
            {
                cmbContactCategory.SelectedIndex = 0;
            }

            cmbContactCategory.Enabled = (ARow.RowState == DataRowState.Added);
        }

        private void GetDetailDataFromControlsManual(PPartnerAttributeRow ARow)
        {
        }
        
        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_InsertKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            NewRecord(this, null);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_DeleteKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            if (e.Row != -1)
            {
                this.DeleteRecord(this, null);
            }
        }

        /// <summary>
        /// adding a new partner relationship record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRecord(System.Object sender, EventArgs e)
        {
            if (CreateNewPPartnerAttribute())
            {
                cmbContactCategory.Focus();
            }

            // Fire OnRecalculateScreenParts event: reset counter in tab header
            DoRecalculateScreenParts();
        }

        /// <summary>
        /// manual code when adding new row
        /// </summary>
        /// <param name="ARow"></param>
        private void NewRowManual(ref PartnerEditTDSPPartnerAttributeRow ARow)
        {
            int LeastSequence = 0;
            int HighestIndex = 0;
            int ThisRow_Sequence = 0;
            int ThisRow_Index = 0;

            System.Data.DataRowVersion ThisRow_RowVersion;
            PPartnerAttributeTable ThisDT = (PPartnerAttributeTable)ARow.Table;

            ARow.PartnerKey = FMainDS.PPartner[0].PartnerKey;
            ARow.AttributeType = FDefaultContactType;

            for (int Counter = 0; Counter < ThisDT.Rows.Count; Counter++)
            {
                if (ThisDT.Rows[Counter].RowState == DataRowState.Deleted)
                {
                    ThisRow_RowVersion = DataRowVersion.Original;
                }
                else
                {
                    ThisRow_RowVersion = DataRowVersion.Current;
                }

                ThisRow_Sequence = Convert.ToInt32(ThisDT.Rows[Counter][PPartnerAttributeTable.GetSequenceDBName(), ThisRow_RowVersion]);

                if (ThisRow_Sequence < LeastSequence)
                {
                    LeastSequence = ThisRow_Sequence;
                }
            }

            ARow.Sequence = LeastSequence - 1;

            for (int Counter = 0; Counter < ThisDT.Rows.Count; Counter++)
            {
                if (ThisDT.Rows[Counter].RowState == DataRowState.Deleted)
                {
                    ThisRow_RowVersion = DataRowVersion.Original;
                }
                else
                {
                    ThisRow_RowVersion = DataRowVersion.Current;
                }

                ThisRow_Index = Convert.ToInt32(ThisDT.Rows[Counter][PPartnerAttributeTable.GetIndexDBName(), ThisRow_RowVersion]);

                if (ThisRow_Index > HighestIndex)
                {
                    HighestIndex = ThisRow_Index;
                }
            }

            ARow.Index = HighestIndex + 1;

            ARow.Value = "NEWVALUE" + ARow.Sequence.ToString();
            ARow.Primary = false;
            ARow.WithinOrgansiation = false;
            ARow.Specialised = false;
            ARow.Confidential = false;
            ARow.Current = true;
            ARow.PartnerContactDetail = true;
        }

        private void DeleteRecord(Object sender, EventArgs e)
        {
            // Before the auto-generated Method for deletion can be called we need to set the Expressions that refer to 'Parent.*' to "",
            // otherwise we get a 'System.IndexOutOfRangeException: Cannot find relation 0.' Exception when the DataTable gets deserialised
            // on the server side in the call to Method 'GetNonCacheableRecordReferenceCount'!
            // Confer http://bytes.com/topic/asp-net/answers/323437-cannot-file-relation-0-a
            foreach (DataColumn RemoveExpressionsColumn in FMainDS.PPartnerAttribute.Columns)
            {
                if (RemoveExpressionsColumn.Expression.Length != 0)
                {
                    // TLogging.Log(RemoveExpressionsColumn.Expression, [ToLogFile]);
                    RemoveExpressionsColumn.Expression = "";
                }
            }

            this.DeletePPartnerAttribute();

            // Restore Column Expressions again!
            SetColumnExpressions();
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(PPartnerAttributeRow ARowToDelete, ref string ADeletionQuestion)
        {
            // Those values are getting safed for use in the PostDeleteManual Method
            // Trying to establish those values there doesn't work in case the Partner was new, 
            // the Row was newly added, and then gets removed (DataRow has DataRowVersion.Detached in 
            // this case, and no Original data that can be accessed!)
            FDeletedRowsAttributeType = ARowToDelete.AttributeType;
            FDeletedRowsValue = ARowToDelete.Value;

            ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete the following Contact Detail record?\r\n\r\n    Type: '{0}'\r\n    Value: '{1}'"),
                ARowToDelete.AttributeType, ARowToDelete.Value);
            
            return true;
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(PPartnerAttributeRow ARowToDelete, ref string ACompletionMessage)
        {
            bool deletionSuccessful = false;

            // no message to be shown after deletion
            ACompletionMessage = "";

            try
            {
                ARowToDelete.Delete();
                deletionSuccessful = true;
            }
            catch (Exception ex)
            {
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return deletionSuccessful;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(PPartnerAttributeRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            bool NoEmailAddressesAvailableAnymore = false;
            
            if (ADeletionPerformed)
            {
                StringCollection EmailAttrColl = StringHelper.StrSplit(FEmailAttributesConcatStr, ", ");
            
                if (EmailAttrColl.Contains("'" + FDeletedRowsAttributeType + "'")) 
                {
                    // User deleted an E-mail Contact Detail
                    
                    if (cmbPrimaryEMail.GetSelectedString(-1) == FDeletedRowsValue) 
                    {
                        DataView ElegibleEmailAddrsDV = DeterminePartnersEmailAddresses(true);
                        
                        if (ElegibleEmailAddrsDV.Count == 0) 
                        {
                            NoEmailAddressesAvailableAnymore = true;
                            StrPrimEmailConsequence = Catalog.GetString("The Primary E-mail Address has been cleared as there is no other current E-Mail Address record.");
                            StrPrimEmailMessageBoxTitle = Catalog.GetString("Primary E-Mail Address Cleared");                                    
                        }
                        
                        // User deleted the E-mail Contact Detail that was set as the 'Primary E-Mail Address':
                        // Refresh the Primary E-Mail Address Combo (which upon that will no longer contain the E-mail 
                        // Address of the deleted Contact Detail!) and notify the user that (s)he needs to take action.
                        UpdatePrimaryEmailComboItems(true);                            
                        
                        MessageBox.Show(
                            String.Format(
                                Catalog.GetString("You have deleted the Contact Detail that was set as the 'Primary E-Mail Address'.\r\n\r\n{0}"),
                                StrPrimEmailConsequence),
                            StrPrimEmailMessageBoxTitle, 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    
                        if (!NoEmailAddressesAvailableAnymore)
                        {
                            // Show the other current e-mail-Address(es) to the user                    
                            cmbPrimaryEMail.DroppedDown = true;
                        }                    
                    }   
                    else
                    {
                        // User deleted a E-mail Contact Detail that was not set as the 'Primary E-Mail Address':
                        // Simply refresh the Primary E-Mail Address Combo
                        UpdatePrimaryEmailComboItems(true);
                    }         
                }
                
                DoRecalculateScreenParts();

                if (grdDetails.Rows.Count <= 1)
                {
                    // hide details part and disable buttons if no record in grid (first row for headings)
                    btnDelete.Enabled = false;
                    pnlDetails.Visible = false;
                }                
            }
            
            FDeletedRowsAttributeType = String.Empty;
            FDeletedRowsValue = String.Empty;
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        /// <summary>
        /// Sets this Usercontrol visible or unvisile true = visible, false = invisible.
        /// </summary>
        /// <returns>void</returns>
        private void MakeDetailsInvisible(Boolean value)
        {
            /* make the details part of this screen visible or invisible. */
            this.pnlDetails.Visible = !value;
        }

        /// <summary>
        /// Creates custom DataColumns that will be shown in the Grid.
        /// </summary>
        /// <returns>void</returns>
        public void CreateCustomDataColumns()
        {
            DataColumn ForeignTableColumn;

            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.String");
            ForeignTableColumn.ColumnName = "Parent_" + PPartnerAttributeTypeTable.GetAttributeTypeDBName();
            ForeignTableColumn.Expression = "";  // The real expression will be set in Method 'SetColumnExpressions'!
            FMainDS.PPartnerAttribute.Columns.Add(ForeignTableColumn);

            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.Int32");
            ForeignTableColumn.ColumnName = "Parent_AttributeIndex";
            ForeignTableColumn.Expression = "";  // The real expression will be set in Method 'SetColumnExpressions'!
            FMainDS.PPartnerAttribute.Columns.Add(ForeignTableColumn);

            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.Int32");
            ForeignTableColumn.ColumnName = "Parent_Parent_CategoryIndex";
            ForeignTableColumn.Expression = "";  // The real expression will be set in Method 'SetColumnExpressions'!
            FMainDS.PPartnerAttribute.Columns.Add(ForeignTableColumn);

            ForeignTableColumn = new DataColumn();
            ForeignTableColumn.DataType = System.Type.GetType("System.String");
            ForeignTableColumn.ColumnName = "ContactType";
            ForeignTableColumn.Expression = "";  // The real expression will be set in Method 'SetColumnExpressions'!
            FMainDS.PPartnerAttribute.Columns.Add(ForeignTableColumn);

            if (!FMainDS.PPartnerAttributeType.Columns.Contains("CategoryIndex"))
            {
                ForeignTableColumn = new DataColumn();
                ForeignTableColumn.DataType = System.Type.GetType("System.Int32");
                ForeignTableColumn.ColumnName = "CategoryIndex";
                ForeignTableColumn.Expression = "Parent." + PPartnerAttributeCategoryTable.GetIndexDBName();
                FMainDS.PPartnerAttributeType.Columns.Add(ForeignTableColumn);
            }

            SetColumnExpressions();
        }

        /// <summary>
        /// Sets the Column Expressions for the calculated DataColumns
        /// </summary>
        private void SetColumnExpressions()
        {
            FMainDS.PPartnerAttribute.Columns["Parent_" + PPartnerAttributeTypeTable.GetAttributeTypeDBName()].Expression =
                "Parent." + PPartnerAttributeTypeTable.GetAttributeTypeDBName();

            FMainDS.PPartnerAttribute.Columns["Parent_AttributeIndex"].Expression =
                "Parent." + PPartnerAttributeTypeTable.GetIndexDBName();

            FMainDS.PPartnerAttribute.Columns["Parent_Parent_CategoryIndex"].Expression =
                "Parent.CategoryIndex";

            FMainDS.PPartnerAttribute.Columns["ContactType"].Expression =
                "IIF(" + PPartnerAttributeTable.GetSpecialisedDBName() + " = true, ISNULL(Parent." +
                PPartnerAttributeTypeTable.GetSpecialLabelDBName() + ", Parent." + PPartnerAttributeTypeTable.GetAttributeTypeDBName() + "), Parent." +
                PPartnerAttributeTypeTable.GetAttributeTypeDBName() + ")";
        }

        /// <summary>
        /// Creates DataBound columns for the Grid control.
        /// </summary>
        /// <returns>void</returns>
        public void CreateGridColumns()
        {
            // Get rid of the Columns as added per YAML file as we need to show calculated Columns!
            grdDetails.Columns.Clear();
// TODO           FDataGrid.AddImageColumn(@GetAddressKindIconForGridRow);

            //
            // Contact Type
//            grdDetails.AddTextColumn("Type Code", FMainDS.PPartnerAttribute.Columns["Parent_" + PPartnerAttributeTypeTable.GetCodeDBName()]);

            //
            // Contact Type (Calculated Expression!)
            grdDetails.AddTextColumn("Contact Type", FMainDS.PPartnerAttribute.Columns["ContactType"]);

            // Comment
            grdDetails.AddTextColumn("Comment", FMainDS.PPartnerAttribute.ColumnComment);

            // Value
            grdDetails.AddTextColumn("Value", FMainDS.PPartnerAttribute.ColumnValue);

            // Current
            grdDetails.AddCheckBoxColumn("Current", FMainDS.PPartnerAttribute.ColumnCurrent);

            // Confidential
            grdDetails.AddCheckBoxColumn("Confidential", FMainDS.PPartnerAttribute.ColumnConfidential);

//            // Sequence (for testing purposes only...)
//            grdDetails.AddTextColumn("Sequence", FMainDS.PPartnerAttribute.ColumnSequence);
//
//            // Index (for testing purposes only...)
//            grdDetails.AddTextColumn("Index", FMainDS.PPartnerAttribute.ColumnIndex);
//
//            // Primary (for testing purposes only...)
//            grdDetails.AddCheckBoxColumn("Primary", FMainDS.PPartnerAttribute.ColumnPrimary);
//
//            // Within Organsiation (for testing purposes only...)
//            if (FMainDS.PPartner[0].PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
//            {
//
//                grdDetails.AddCheckBoxColumn("Within Org.", FMainDS.PPartnerAttribute.ColumnWithinOrgansiation);
//            }

            // Modification TimeStamp (for testing purposes only...)
            // grdDetails.AddTextColumn("Modification TimeStamp", FMainDS.PPartnerAttribute.ColumnModificationId);
        }

        private void ValidateDataDetailsManual(PPartnerAttributeRow ARow)
        {
//            bool NewPartner = (FMainDS.PPartner.Rows[0].RowState == DataRowState.Added);

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

//            TSharedPartnerValidation_Partner.ValidateRelationshipManual(this, ARow, ref VerificationResultCollection,
//                FValidationControlsDict, NewPartner, ((PPartnerRow)FMainDS.PPartner.Rows[0]).PartnerKey);
        }

        #endregion

        #region Event Handlers

        private void ContactTypePromote(object sender, EventArgs e)
        {
            throw new NotImplementedException("Promotion of Contact Types not implemented yet!");
        }

        private void ContactTypeDemote(object sender, EventArgs e)
        {
            throw new NotImplementedException("Demotion of Contact Types not implemented yet!");
        }

        private void LaunchHyperlinkPrefEMail(object sender, EventArgs e)
        {
            throw new NotImplementedException("Launching of E-Mail program not implemented yet!");
        }

        private void LaunchHyperlinkEMailWithinOrg(object sender, EventArgs e)
        {
            throw new NotImplementedException("Launching of E-Mail program not implemented yet!");
        }

        private void FilterCriteriaChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException("Filtering is not implemented yet!");
        }

        private void HandleCurrentFlagChanged(Object sender, EventArgs e)
        {
            bool PrimaryEmailAddressIsThisRecord = false;
            
            dtpNoLongerCurrentFrom.Enabled = !chkCurrent.Checked;

            if ((!FRunningInsideShowDetails) 
                && (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS))
            {
                // Ensure current Checked state is reflected in the DataRow
                GetSelectedDetailRow().Current = chkCurrent.Checked;
                
                if (cmbPrimaryEMail.GetSelectedString() == txtValue.Text)
                {
                    PrimaryEmailAddressIsThisRecord = true;
                }
                
                // Refresh the ComboBox so it shows only the 'current' E-Mail Address records (which could possibly be none!)
                UpdatePrimaryEmailComboItems(true);                
            }
            
            if (!chkCurrent.Checked)
            {
                dtpNoLongerCurrentFrom.Date = DateTime.Now.Date;
                dtpNoLongerCurrentFrom.Focus();
                
                if ((!FRunningInsideShowDetails) 
                    && (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS))
                {
                    CheckThatNonCurrentEmailAddressIsntPrimaryEmailAddr(PrimaryEmailAddressIsThisRecord);
                }
            }
            else
            {
                dtpNoLongerCurrentFrom.Date = null;
            }
            
            DoRecalculateScreenParts();
        }

        private void HandleSpecialisedFlagChanged(Object sender, EventArgs e)
        {
            // Ensure current Checked state is reflected in the DataRow
            GetSelectedDetailRow().Specialised = chkSpecialised.Checked;            
        }
        
        private void HandleValueLeave(Object sender, EventArgs e)
        {
            var SelectedDetailDR = GetSelectedDetailRow();
            if ((!FRunningInsideShowDetails) 
                && (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS))
            {
                if (SelectedDetailDR.RowState != DataRowState.Detached) 
                {
                    // Ensure current email address is reflected in the DataRow
                    SelectedDetailDR.Value = txtValue.Text;
                    
                    // Refresh the ComboBox so it reflects any change in the email address!
                    UpdatePrimaryEmailComboItems(true, txtValue.Text);                                                    
                }
            }
        }
            
        private void HandlePrimaryEmailSelectedValueChanged(object sender, EventArgs e)
        {
            if (!FPrimaryEmailSelectedValueChangedEvent)
            {
                UpdatePrimaryEmailAddressRecords(false);
            }
        }
        
        private void CheckThatNonCurrentEmailAddressIsntPrimaryEmailAddr(bool AIsPrimaryEmailAddressIsThisRecord)
        {
            bool NoEmailAddressesAvailableAnymore = false;
            
            if (AIsPrimaryEmailAddressIsThisRecord) 
            {
                DataView ElegibleEmailAddrsDV = DeterminePartnersEmailAddresses(true);
                
                if (ElegibleEmailAddrsDV.Count == 0) 
                {
                    NoEmailAddressesAvailableAnymore = true;
                    StrPrimEmailConsequence = Catalog.GetString("The Primary E-mail Address has been cleared as there is no other current E-Mail Address record.");
                    StrPrimEmailMessageBoxTitle = Catalog.GetString("Primary E-Mail Address Cleared");                                    
                }

                // Select the 'empty' ComboBox Item
                cmbPrimaryEMail.SelectedIndex = 0;                        
                
                MessageBox.Show(
                    String.Format(
                        Catalog.GetString("You have made the E-Mail Address no longer current, but up till now it was set to be the Primary E-Mail Address.\r\n\r\n{0}"), 
                        StrPrimEmailConsequence), 
                    StrPrimEmailMessageBoxTitle, 
                    MessageBoxButtons.OK, NoEmailAddressesAvailableAnymore ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        
                if (!NoEmailAddressesAvailableAnymore)
                {
                    // Show the other current e-mail-Address(es) to the user                    
                    cmbPrimaryEMail.DroppedDown = true;
                }
            }
        }
        
        private void FilterContactTypeCombo(Object sender, EventArgs e)
        {
                if (cmbContactCategory.Text != String.Empty)
                {
                    FSuppressOnContactTypeChangedEvent = true;
                    
                    cmbContactType.Filter = PPartnerAttributeTypeTable.GetCategoryCodeDBName() + " = '" + cmbContactCategory.Text + "'";
                    
                    FSuppressOnContactTypeChangedEvent = false;
    
                    // Select the first item in the ComboBox
                    if (cmbContactType.Count > 0)
                    {
                        cmbContactType.SelectedIndex = 0;
                    }
    
                    OnContactTypeChanged(null, null);
                }                
        }

        private void OnContactTypeChanged(Object sender, EventArgs e)
        {
            PPartnerAttributeTypeRow ContactTypeDR;
            TPartnerAttributeTypeValueKind ValueKind;

            if ((!FSuppressOnContactTypeChangedEvent)
                && (cmbContactType.Text != String.Empty))
            {
                ContactTypeDR = (PPartnerAttributeTypeRow)cmbContactType.GetSelectedItemsDataRow();

                SelectCorrespondingCategory(ContactTypeDR);

                if (Enum.TryParse <TPartnerAttributeTypeValueKind>(ContactTypeDR.AttributeTypeValueKind, out ValueKind))
                {
                    switch (ValueKind)
                    {
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL:
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK:
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK_WITHVALUE:
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS:
                        case TPartnerAttributeTypeValueKind.CONTACTDETAIL_SKYPEID:
                            FValueKind = ValueKind;

                            break;

                        default:
                            throw new Exception("Invalid value for TPartnerAttributeTypeValueKind");
                    }
                }
                else
                {
                    // Fallback!
                    FValueKind = TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL;
                }

                UpdateValueManual();
            }
        }

        private void UpdateValueManual()
        {
            var CurrentRow = GetSelectedDetailRow();

            if (CurrentRow == null)
            {
                return;
            }

            string Value = CurrentRow.Value;
//            string ValueText = String.Empty;

            switch (FValueKind)
            {
                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL:
                    // TODO UpdateValueManual / CONTACTDETAIL_GENERAL

                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK:
                    // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
                    // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//                    rtbValue.Helper.DisplayURL(Value);

                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK_WITHVALUE:
                    // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
                    // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//                    rtbValue.Helper.DisplayURLWithValue(Value);

                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS:
                    // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
                    // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//                    rtbValue.Helper.DisplayEmailAddress(Value);

                    break;

                case TPartnerAttributeTypeValueKind.CONTACTDETAIL_SKYPEID:
                    // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
                    // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//                    rtbValue.Helper.DisplaySkypeID(Value);

                    break;

                default:
                    throw new Exception("Invalid value for TPartnerAttributeTypeValueKind");
            }
        }

        private void SelectCorrespondingCategory(PPartnerAttributeTypeRow ARow)
        {
            int SelectionCounter = 0;

            foreach (DataRowView Drv in cmbContactCategory.Table.DefaultView)
            {
                if (((PPartnerAttributeCategoryRow)(Drv.Row)).CategoryCode == ARow.CategoryCode)
                {
                    break;
                }

                SelectionCounter++;
            }

            cmbContactCategory.SelectedIndex = SelectionCounter;
        }

        /// <summary>
        /// Constructs a valid URL string from a Value that is of a Contact Type that has got a Hyperlink Format set up.
        /// </summary>
        /// <param name="AValue">Value that should replace THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER in the Hyperlink Format strin</param>
        /// <returns>URL with the Value replacing THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER.</returns>
        private string BuildLinkWithValue(string AValue)
        {
            string HyperlinkFormat;
            string ReturnValue = String.Empty;

            if (FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK_WITHVALUE)
            {
                HyperlinkFormat = ((PPartnerAttributeTypeRow)cmbContactType.GetSelectedItemsDataRow()).HyperlinkFormat;

                if ((HyperlinkFormat != null)
                    && (HyperlinkFormat != String.Empty))
                {
                    if ((HyperlinkFormat.Contains("{"))
                        && HyperlinkFormat.Contains("}"))
                    {
                        ReturnValue = HyperlinkFormat.Substring(0, HyperlinkFormat.IndexOf('{')) +
                                      // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
                                      // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//                            rtbValue.Text;
                                      txtValue.Text;
                        ReturnValue += HyperlinkFormat.Substring(HyperlinkFormat.LastIndexOf('}') + 1);
                    }
                    else
                    {
                        throw new EProblemConstructingHyperlinkException(
                            "The Value should be used to construct a hyperlink-with-value-replacement but the HyperlinkFormat is not correct (it needs to contain both the '{' and '}' characters)");
                    }
                }
                else
                {
                    throw new Exception(
                        "The Value should be used to construct a hyperlink-with-value-replacement but the HyperlinkFormat of the Contact Type is not specified");
                }
            }
            else
            {
                throw new Exception(
                    "The Value should be used to construct a hyperlink-with-value-replacement but the LinkType of the Value Control is not 'TLinkTypes.Http_With_Value_Replacement'");
            }

            return ReturnValue;
        }

        /// <summary>
        /// Called from a timer, ShowMessageBoxTimer, so that the FocusRowLeaving Event processing can
        /// complete before the MessageBox is show (would the MessageBox be shown while the Event gets
        /// processed the Grid would get into a strange state in which mouse moves would cause the Grid
        /// to scroll!).
        /// </summary>
        /// <param name="Sender">Gets evaluated to make sure a Timer is calling this Method.</param>
        /// <param name="e">Ignored.</param>
        private void ShowTimerDrivenMessageBox(Object Sender, EventArgs e)
        {
            System.Windows.Forms.Timer SendingTimer = Sender as System.Windows.Forms.Timer;

            if (SendingTimer != null)
            {
                // I got called from a Timer: stop that now so that the following MessageBox gets shown only once!
                SendingTimer.Stop();

                switch (FTimerDrivenMessageBoxKind)
                {
                    case TUC_ContactDetails.TTimerDrivenMessageBoxKind.tdmbkNoPrimaryEmailAsNoCurrentAvailable:
                        MessageBox.Show(Catalog.GetString(
                            "No Primary Email Address has been chosen for this Partner, although the Partner has at least one current Email Address on record.\r\n\r\n"
                            +
                            "IMPORTANT: OpenPetra can't send emails to this Partner in automated situations unless a Primary Email Address has been chosen!\r\n" 
                            +
                            "Therefore, please choose an Email Address from the Primary Email Address setting,"),
                        Catalog.GetString("No Primary Email Address Set!"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        cmbPrimaryEMail.Focus();
                        cmbPrimaryEMail.DroppedDown = true;

                        break;

                    case TUC_ContactDetails.TTimerDrivenMessageBoxKind.tdmbkNoPrimaryEmailButNonCurrentAvailable:
                        MessageBox.Show(Catalog.GetString(
                            "No Primary Email Address has been chosen for this Partner.\r\n\r\nThere are non-current Email Addresses on record. You might want to\r\n"
                            +
                            "check whether a current email address is available for this Partner."),
                        Catalog.GetString("No Primary Email Address Set - No Current Email Address"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                        break;

                    default:
                        throw new Exception("Invalid value for TTimerDrivenMessageBoxKind");
                }
            }
        }

        #endregion
        
        #region Data Validation
        
        /// <summary>
        /// Creates a Data Validation *Warning* for cmbPrimaryEmail.
        /// </summary>
        private void ValidationPrimaryEmailAddrNotSet()
        {
            const string ResCont = "ContactDetails_PrimaryEmailAddress_Not_Set";
            TScreenVerificationResult VerificationResult;
            
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_NOT_SET_DESIPITE_EMAIL_ADDR_AVAIL),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbPrimaryEMail, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for cmbPrimaryEmail.
        /// </summary>
        private void ValidationPrimaryEmailAddrSetButNoEmailAddrAvailable()
        {
            const string ResCont = "ContactDetails_PrimaryEmailAddress_Set_But_No_Email_Addr_Available";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_SET_DESIPITE_NO_EMAIL_ADDR_AVAIL),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbPrimaryEMail, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for cmbPrimaryEmail.
        /// </summary>
        private void ValidationPrimaryEmailAddrSetButItIsntCurrent()
        {
            const string ResCont = "ContactDetails_PrimaryEmailAddress_Set_But_It_Isnt_Current";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_SET_BUT_IT_ISNT_CURRENT),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbPrimaryEMail, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }

        /// <summary>
        /// Creates a Data Validation *Error* for cmbPrimaryEmail.
        /// </summary>
        private void ValidationPrimaryEmailAddrSetButNotAmongEmailAddrs()
        {
            const string ResCont = "ContactDetails_PrimaryEmailAddress_Set_But_Not_Among_Email_Addrs";
            TScreenVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            VerificationResult = new TScreenVerificationResult(
                new TVerificationResult((object)ResCont,
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PRIMARY_EMAIL_ADDR_SET_BUT_NOT_AMONG_EMAIL_ADDR),
                    FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID),
                null, cmbPrimaryEMail, FPetraUtilsObject.VerificationResultCollection.CurrentDataValidationRunID);


            VerificationResultCollection.Remove(ResCont);

            if (VerificationResult != null)
            {
                VerificationResultCollection.Add(VerificationResult);
            }
        }
        
        #endregion        
    }

    /// <summary>
    /// Thrown if the the attempt to construct a Hyperlink from a Value and a Hyperlink Format fails.
    /// </summary>
    public class EProblemConstructingHyperlinkException : Exception
    {
        /// <summary>
        /// Constructor with inner Exception
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="message"></param>
        public EProblemConstructingHyperlinkException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor without inner Exception
        /// </summary>
        /// <param name="message"></param>
        public EProblemConstructingHyperlinkException(string message)
            : base(message)
        {
        }
    }
}