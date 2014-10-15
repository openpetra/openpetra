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
        private readonly string StrDefaultContactType = Catalog.GetString("Phone");
            
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private Boolean FPartnerAttributesExist;
        
        private string FDefaultContactType = String.Empty;
        
        private TPartnerAttributeTypeValueKind FValueKind = TPartnerAttributeTypeValueKind.CONTACTDETAIL_GENERAL;
        
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

        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
        }

        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

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

            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpContactDetails));

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);

            // enable grid to react to insert and delete keyboard keys
            grdDetails.InsertKeyPressed += new TKeyPressedEventHandler(grdDetails_InsertKeyPressed);

            /* Check if data needs to be retrieved from the PetraServer */
            if (FMainDS.PPartnerAttribute == null)
            {
                FPartnerAttributesExist = LoadDataOnDemand();
            }

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
                FMainDS.Merge((PPartnerAttributeCategoryTable) TDataCache.TMPartner.GetCacheablePartnerTable2(TCacheablePartnerTablesEnum.ContactCategoryList, PPartnerAttributeCategoryTable.GetTableName()));

                if (FMainDS.PPartnerAttributeCategory.Count == 0)
                {
                    MessageBox.Show(Catalog.GetString("There are no Partner Contact Categories available. Due to this, this Tab will not work correctly!\r\n\r\nPlease set up at least one Partner Contact Category!"),
                                    Catalog.GetString("Partner Contact Details Tab: Not Functional"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);                                    
                }                
            }

            if (FMainDS.PPartnerAttributeType.Count == 0)
            {
                // Note: If FMainDS contains an instance of the PPartnerAttributeTypeTable, but it hasn't got any rows
                // we add them here from the corresponding Cacheable DataTable (that is also the case when a new Partner gets created)
                FMainDS.Merge((PPartnerAttributeTypeTable) TDataCache.TMPartner.GetCacheablePartnerTable2(TCacheablePartnerTablesEnum.ContactTypeList, PPartnerAttributeTypeTable.GetTableName()));                
                
                if (FMainDS.PPartnerAttributeType.Count == 0) 
                {
                    MessageBox.Show(Catalog.GetString("There are no Partner Contact Types available. Due to this, this Tab will not work correctly!\r\n\r\nPlease set up at least one Partner Contact Type!"),
                                    Catalog.GetString("Partner Contact Details Tab: Not Functional"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            
            FPartnerAttributesExist = FMainDS.PPartnerAttribute.Rows.Count > 0;
            
            // Need to enable Relations as the PPartnerAttributeCategoryTable and PPartnerAttributeTypeTable have not been part of the PartnerEditTDS when transferred from the OpenPetra Server!
            // These Relations are needed in Method 'CreateCustomDataColumns'.
            FMainDS.EnableRelation("ContactDetails1");
            FMainDS.EnableRelation("ContactDetails2");
            
            // Create custom data columns on-the-fly
            CreateCustomDataColumns();
            
            
            /* Create SourceDataGrid columns */
            CreateGridColumns();

            /* Setup the DataGrid's visual appearance */
//            SetupDataGridVisualAppearance();

            /* Prepare the Demote and Promote buttons first time */
//            PrepareArrowButtons();

            /* Hook up event that fires when a different Row is selected */
//            grdDetails.Selection.FocusRowEntered += new RowEventHandler(this.DataGrid_FocusRowEntered);
            
            // Set up special sort order of Rows in Grid:
            // PPartnerAttributeCategory.Index followed by PPartnerAttributeType.Index followed by PPartnerAttribute.Index!
            DataView gridView = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;
            gridView.Sort = "Parent_Parent_CategoryIndex ASC, Parent_AttributeIndex ASC, " +
                PPartnerAttributeTable.GetIndexDBName() + " ASC";
            
            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpFamilyMembers));

            if (!FPartnerAttributesExist)
            {
                // TODO PostInitUserControl - if (!FPartnerAttributesExist) 
//                /* If Family has no members, these buttons are disabled */
//                this.btnFamilyMemberDemote.Enabled = false;
//                this.btnFamilyMemberPromote.Enabled = false;
//                this.btnMovePersonToOtherFamily.Enabled = false;
//                this.btnEditPerson.Enabled = false;
//                this.btnEditFamilyID.Enabled = false;

                /* this.btnAddExistingPersonToThisFamily.enabled := false; */
                /* this.btnAddNewPersonThisFamily.enabled := false; */
            }

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
            DefaultContactTypes = FMainDS.PPartnerAttributeType.Select(PPartnerAttributeTypeTable.GetCodeDBName() + " = '" + StrDefaultContactType + "'");
            
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
                    FDefaultContactType = ((PPartnerAttributeTypeRow)SortedPartnerAttr[0].Row).Code;
                }
            }
            
            // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
            // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!
//            rtbValue.LinkClicked += new Ict.Common.Controls.TRtbHyperlinks.THyperLinkClickedArgs(rtbValue.Helper.LaunchHyperLink);
   
            // TODO ApplySecurity();            
        }

        /// <summary>
        /// Performs necessary actions to make the Merging of rows that were changed on
        /// the Server side into the Client-side DataSet possible.
        /// </summary>
        /// <returns>void</returns>
        public void CleanupRecordsBeforeMerge()
        {
            DataView NewPartnerAttributesDV;
            List<DataRow> PPartnerAttributeDeleteRows = new List<DataRow>();

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
                FMainDS.Tables.Add(new PPartnerAttributeTable());
                FMainDS.InitVars();
            }

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
            FPetraUtilsObject.SetStatusBarText(cmbPrimaryWayOfContacting, Catalog.GetString("Select the primary method by which the Partner should be contacted. Purely for information."));
            FPetraUtilsObject.SetStatusBarText(cmbPrimaryPhoneForContacting, Catalog.GetString("Select one of the Partner's telephone numbers. Purely for information."));
            FPetraUtilsObject.SetStatusBarText(cmbPrimaryEMail, Catalog.GetString("Select one of the Partner's e-mail addresses. This will be used whenever an automated e-mail is to be sent to this Partner."));
            FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkPrefEMail, Catalog.GetString("Click this button to send an email to the Partner's Primary E-mail address."));
            FPetraUtilsObject.SetStatusBarText(cmbPhoneWithinTheOrganisation, Catalog.GetString("Select one of the Partner's telephone numbers to designate it as her/his telephone number within The Organisation."));
            FPetraUtilsObject.SetStatusBarText(cmbEMailWithinTheOrganisation, Catalog.GetString("Select one of the Partner's e-mail addresses to designate it as her/his e-mail address within The Organisation."));
            FPetraUtilsObject.SetStatusBarText(btnLaunchHyperlinkEMailWithinOrg, Catalog.GetString("Click this button to send an email to the Partner's Office E-mail address."));
                        
            FPetraUtilsObject.SetStatusBarText(btnPromote, Catalog.GetString("Click this button to re-arrange a contact detail record within records of the same Contact Type."));
            FPetraUtilsObject.SetStatusBarText(btnDemote, Catalog.GetString("Click this button to re-arrange a contact detail record within records of the same Contact Type."));
            
            FPetraUtilsObject.SetStatusBarText(cmbContactCategory, Catalog.GetString("Contact Category to which the Contact Type belongs to (narrows down available Contact Types)."));
            FPetraUtilsObject.SetStatusBarText(cmbContactType, Catalog.GetString("Describes what the Value is (e.g. Phone Number, E-Mail Address, etc)."));
            FPetraUtilsObject.SetStatusBarText(chkSpecialised, Catalog.GetString("Tick this if the Value designates a business-related Contact Detail (e.g. business telephone number)."));

            FPetraUtilsObject.SetStatusBarText(txtComment, Catalog.GetString("Comment for this Contact Detail record."));
            FPetraUtilsObject.SetStatusBarText(chkCurrent, Catalog.GetString("Untick this if the Contact Detail record is no longer current."));
            FPetraUtilsObject.SetStatusBarText(dtpNoLongerCurrentFrom, Catalog.GetString("Date from which the Contact Detail record is no longer current."));
           
            FPetraUtilsObject.SetStatusBarText(chkConfidential, Catalog.GetString("Tick this if the Contact Detail record is confidential. Please refer to the User Guide what effect this setting has!"));

            // TODO SHORTCUTS: Listed here are 'Shortcuts' for finishing the core of the functionality earlier. They will need to be addressed later for full functionality!
            // rtbValue will replace txtValue, but for the time being we have just a plain Textbox instead of the Hyperlink-enabled Rich Text Box!           
            FPetraUtilsObject.SetStatusBarText(txtValue, Catalog.GetString("Phone Number, Mobile Phone Number, E-mail Address, Internet Address, ... --- whatever the Contact Type is about."));
//            FPetraUtilsObject.SetStatusBarText(rtbValue, Catalog.GetString("Phone Number, Mobile Phone Number, E-mail Address, Internet Address, ... --- whatever the Contact Type is about."));
            
            // By default only valid Contact Details should be shown
//            chkValidContactDetailsOnly.Checked = true;  // TODO - work on Action, then uncomment this line

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

        /// <summary>
        /// Loads Partner Types Data from Petra Server into FMainDS.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        public Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            // retrieve Contact Details (stored in PPartnerAttribute Table) from PetraServer
            // If the Partner has got no Contact Details: returns false
            try
            {
                // Make sure that Typed DataTable is already there at Client side
                if (FMainDS.PPartnerAttribute == null)
                {
                    FMainDS.Tables.Add(new PPartnerAttributeTable(PPartnerAttributeTable.GetTableName()));
                    FMainDS.InitVars();
                }

                FMainDS.PPartnerAttribute.Rows.Clear();
                FMainDS.Merge(FPartnerEditUIConnector.GetDataContactDetails());
                FMainDS.PPartnerAttribute.AcceptChanges();

                if (FMainDS.PPartnerAttribute.Rows.Count > 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                ReturnValue = false;
                return false;
            }
            catch (Exception)
            {
                ReturnValue = false;

                // raise;
            }

            return ReturnValue;
        }
        
        private void ShowDataManual()
        {
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
        }
        
        private void BeforeShowDetailsManual(PPartnerAttributeRow ARow)
        {
            if (ARow != null)
            {
                btnDelete.Enabled = true;
                
                if (FMainDS.PPartnerAttributeType != null) 
                {
                    DataRow[] ParnterAttributeRow = FMainDS.PPartnerAttributeType.Select(PPartnerAttributeTypeTable.GetCodeDBName() + " = " + "'" + ARow.AttributeType + "'");
                                                                          
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
            TRecalculateScreenPartsEventArgs RecalculateScreenPartsEventArgs;

            if (CreateNewPPartnerAttribute())
            {
                cmbContactCategory.Focus();
            }

            // Fire OnRecalculateScreenParts event: reset counter in tab header
            RecalculateScreenPartsEventArgs = new TRecalculateScreenPartsEventArgs();
            RecalculateScreenPartsEventArgs.ScreenPart = TScreenPartEnum.spCounters;
            OnRecalculateScreenParts(RecalculateScreenPartsEventArgs);
        }

        /// <summary>
        /// manual code when adding new row
        /// </summary>
        /// <param name="ARow"></param>
        private void NewRowManual(ref PPartnerAttributeRow ARow)
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
            
            ARow.Sequence = LeastSequence  - 1;
         
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
            /*Code to execute before the delete can take place*/
//            ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete Contact Detail record: '{0}'?"),
//                ARowToDelete.RelationName);
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
            DoRecalculateScreenParts();

            if (grdDetails.Rows.Count <= 1)
            {
                // hide details part and disable buttons if no record in grid (first row for headings)
                btnDelete.Enabled = false;
                pnlDetails.Visible = false;
            }
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
            ForeignTableColumn.ColumnName = "Parent_" + PPartnerAttributeTypeTable.GetCodeDBName();
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
            FMainDS.PPartnerAttribute.Columns["Parent_" + PPartnerAttributeTypeTable.GetCodeDBName()].Expression =
                "Parent." + PPartnerAttributeTypeTable.GetCodeDBName();

            FMainDS.PPartnerAttribute.Columns["Parent_AttributeIndex"].Expression =
                "Parent."  + PPartnerAttributeTypeTable.GetIndexDBName();
            
            FMainDS.PPartnerAttribute.Columns["Parent_Parent_CategoryIndex"].Expression =
                "Parent.CategoryIndex";
            
            FMainDS.PPartnerAttribute.Columns["ContactType"].Expression =
                "IIF(" + PPartnerAttributeTable.GetSpecialisedDBName() + " = true, ISNULL(Parent." + PPartnerAttributeTypeTable.GetSpecialLabelDBName() + ", Parent." + PPartnerAttributeTypeTable.GetCodeDBName() + "), Parent." + PPartnerAttributeTypeTable.GetCodeDBName() + ")";
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
        
        private void EnableDisableNoLongerCurrentFromDate(Object sender, EventArgs e)
        {
            dtpNoLongerCurrentFrom.Enabled = !chkCurrent.Checked;

            if (!chkCurrent.Checked)
            {
                dtpNoLongerCurrentFrom.Date = DateTime.Now.Date;
                dtpNoLongerCurrentFrom.Focus();
            }
            else
            {
                dtpNoLongerCurrentFrom.Date = null;
            }
        }
        
        private void FilterContactTypeCombo(Object sender, EventArgs e)
        {
            if (cmbContactCategory.Text != String.Empty) 
            {
                cmbContactType.Filter = PPartnerAttributeTypeTable.GetAttributeCategoryDBName() + " = '" + cmbContactCategory.Text + "'";    

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
            
            if (cmbContactType.Text != String.Empty) 
            {
                ContactTypeDR = (PPartnerAttributeTypeRow)cmbContactType.GetSelectedItemsDataRow();
                
                SelectCorrespondingCategory(ContactTypeDR);
                
                if(Enum.TryParse<TPartnerAttributeTypeValueKind>(ContactTypeDR.AttributeTypeValueKind, out ValueKind))
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
            
            foreach(DataRowView Drv in cmbContactCategory.Table.DefaultView)
            {
                if (((PPartnerAttributeCategoryRow)(Drv.Row)).CategoryCode == ARow.AttributeCategory)
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

            if(FValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK_WITHVALUE)
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
                        throw new EProblemConstructingHyperlinkException("The Value should be used to construct a hyperlink-with-value-replacement but the HyperlinkFormat is not correct (it needs to contain both the '{' and '}' characters)");
                    }
                }
                else
                {
                    throw new Exception("The Value should be used to construct a hyperlink-with-value-replacement but the HyperlinkFormat of the Contact Type is not specified");    
                }
            }
            else
            {
                throw new Exception("The Value should be used to construct a hyperlink-with-value-replacement but the LinkType of the Value Control is not 'TLinkTypes.Http_With_Value_Replacement'");
            }
            
            return ReturnValue;
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