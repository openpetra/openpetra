//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Wolfgang Browa
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
using System.Drawing;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Person;
using Ict.Petra.Shared.MPersonnel.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_Applications
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;


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

        /// <summary>
        /// todoComment
        /// </summary>
        public void SpecialInitUserControl(IndividualDataTDS AMainDS)
        {
            FMainDS = AMainDS;

            LoadDataOnDemand();

            grdDetails.Columns.Clear();
            grdDetails.AddDateColumn(Catalog.GetString("Date entered"),
                FMainDS.PmGeneralApplication.Columns[PmGeneralApplicationTable.GetDateCreatedDBName()]);
            grdDetails.AddTextColumn(Catalog.GetString("Applied for"), FMainDS.PmGeneralApplication.ColumnApplicationForEventOrField);
            grdDetails.AddTextColumn(Catalog.GetString("Field / Event Name"), FMainDS.PmGeneralApplication.ColumnEventOrFieldName);
            grdDetails.AddTextColumn(Catalog.GetString("Status"),
                FMainDS.PmGeneralApplication.Columns[PmGeneralApplicationTable.GetGenApplicationStatusDBName()], 100);

            // initialize tab controls
            ucoApplicationEvent.InitialiseUserControl();
            ucoApplicationField.InitialiseUserControl();

            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);

            // enable grid to react to insert and delete keyboard keys
            grdDetails.InsertKeyPressed += new TKeyPressedEventHandler(grdDetails_InsertKeyPressed);
            grdDetails.DeleteKeyPressed += new TKeyPressedEventHandler(grdDetails_DeleteKeyPressed);

            // enable grid to react to modified event or field key in details part
            ucoApplicationEvent.ApplicationEventChanged += new TUC_Application_Event.TDelegateApplicationEventChanged(
                ProcessApplicationEventOrFieldChanged);
            ucoApplicationField.ApplicationFieldChanged += new TUC_Application_Field.TDelegateApplicationFieldChanged(
                ProcessApplicationEventOrFieldChanged);

            if (grdDetails.Rows.Count <= 1)
            {
                pnlDetails.Visible = false;
                btnDelete.Enabled = false;
            }
        }

        /// <summary>
        /// returns number of application records existing for current person record
        /// </summary>
        /// <returns>int</returns>
        public int CountApplications()
        {
            DataView TmpDV;

            if (FMainDS.Tables.Contains(PmGeneralApplicationTable.GetTableName()))
            {
                TmpDV = new DataView(FMainDS.PmGeneralApplication, "", "", DataViewRowState.CurrentRows);
                return TmpDV.Count;
            }
            else
            {
                return FMainDS.MiscellaneousData[0].ItemsCountApplications;
            }
        }

        /// <summary>
        /// add a new short term application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRowShortTermApp(System.Object sender, EventArgs e)
        {
   	        // Clear any validation errors so that the following call to ValidateAllData starts with a 'clean slate'.
	        FPetraUtilsObject.VerificationResultCollection.Clear();

            if (ValidateAllData(true, true))
            {
                // we create the table locally, no dataset
                IndividualDataTDSPmGeneralApplicationRow NewRowGeneralApp = FMainDS.PmGeneralApplication.NewRowTyped(true);
                PmShortTermApplicationRow NewRowShortTermApp = FMainDS.PmShortTermApplication.NewRowTyped(true);

                NewRowGeneralApp.PartnerKey = FMainDS.PPerson[0].PartnerKey;
                NewRowGeneralApp.ApplicationKey = Convert.ToInt32(TRemote.MCommon.WebConnectors.GetNextSequence(TSequenceNames.seq_application));
                NewRowGeneralApp.RegistrationOffice = Convert.ToInt64(TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_SITEKEY, ""));
                NewRowGeneralApp.GenAppDate = DateTime.Today;
                NewRowGeneralApp.ApplicationForEventOrField = Catalog.GetString("Event");

                //TODO temp, needs to be changed
                NewRowGeneralApp.AppTypeName = "CONFERENCE";
                NewRowGeneralApp.OldLink = "0";
                NewRowGeneralApp.GenApplicantType = "Participant";

                NewRowShortTermApp.PartnerKey = NewRowGeneralApp.PartnerKey;
                NewRowShortTermApp.ApplicationKey = NewRowGeneralApp.ApplicationKey;
                NewRowShortTermApp.RegistrationOffice = NewRowGeneralApp.RegistrationOffice;
                NewRowShortTermApp.StAppDate = NewRowGeneralApp.GenAppDate;

                //TODO temp, needs to be changed
                NewRowShortTermApp.StApplicationType = "A";
                NewRowShortTermApp.StBasicOutreachId = "0";

                FMainDS.PmGeneralApplication.Rows.Add(NewRowGeneralApp);
                FMainDS.PmShortTermApplication.Rows.Add(NewRowShortTermApp);

                FPetraUtilsObject.SetChangedFlag();

                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.PmGeneralApplication.DefaultView);
                grdDetails.Refresh();
                SelectDetailRowByDataTableIndex(FMainDS.PmGeneralApplication.Rows.Count - 1);

                ShowDetails(NewRowGeneralApp);
            }
        }

        /// <summary>
        /// add a new long term application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRowLongTermApp(System.Object sender, EventArgs e)
        {
   	        // Clear any validation errors so that the following call to ValidateAllData starts with a 'clean slate'.
	        FPetraUtilsObject.VerificationResultCollection.Clear();

	        if (ValidateAllData(true, true))
            {
                // we create the table locally, no dataset
                IndividualDataTDSPmGeneralApplicationRow NewRowGeneralApp = FMainDS.PmGeneralApplication.NewRowTyped(true);
                PmYearProgramApplicationRow NewRowLongTermApp = FMainDS.PmYearProgramApplication.NewRowTyped(true);

                NewRowGeneralApp.PartnerKey = FMainDS.PPerson[0].PartnerKey;
                NewRowGeneralApp.ApplicationKey = Convert.ToInt32(TRemote.MCommon.WebConnectors.GetNextSequence(TSequenceNames.seq_application));
                NewRowGeneralApp.RegistrationOffice = Convert.ToInt64(TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_SITEKEY, ""));
                NewRowGeneralApp.GenAppDate = DateTime.Today;
                NewRowGeneralApp.ApplicationForEventOrField = Catalog.GetString("Field");

                //TODO temp, needs to be changed
                NewRowGeneralApp.AppTypeName = "FIELD";
                NewRowGeneralApp.OldLink = "0";
                NewRowGeneralApp.GenApplicantType = "Participant";

                NewRowLongTermApp.PartnerKey = NewRowGeneralApp.PartnerKey;
                NewRowLongTermApp.ApplicationKey = NewRowGeneralApp.ApplicationKey;
                NewRowLongTermApp.RegistrationOffice = NewRowGeneralApp.RegistrationOffice;

                //TODO temp, needs to be changed
                NewRowLongTermApp.YpAppDate = NewRowGeneralApp.GenAppDate;
                NewRowLongTermApp.YpBasicAppType = "Field";

                FMainDS.PmGeneralApplication.Rows.Add(NewRowGeneralApp);
                FMainDS.PmYearProgramApplication.Rows.Add(NewRowLongTermApp);

                FPetraUtilsObject.SetChangedFlag();

                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.PmGeneralApplication.DefaultView);
                grdDetails.Refresh();
                SelectDetailRowByDataTableIndex(FMainDS.PmGeneralApplication.Rows.Count - 1);

                ShowDetails(NewRowGeneralApp);
            }
        }

        private void NewRowManual(ref IndividualDataTDSPmGeneralApplicationRow ARow)
        {
        }

        private void ShowDataManual()
        {
        }

        private void DeleteRow(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if (MessageBox.Show(String.Format(Catalog.GetString(
                            "You have choosen to delete the record for {0} {1}.\n\nDo you really want to delete it?"),
                        FPreviouslySelectedDetailRow.ApplicationForEventOrField,
                        FPreviouslySelectedDetailRow.EventOrFieldName),
                    Catalog.GetString("Confirm Delete"),
                    MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int rowIndex = grdDetails.SelectedRowIndex();

                // along with the general application record the specific record needs to be deleted
                if (IsEventApplication(FPreviouslySelectedDetailRow))
                {
                    GetEventApplicationRow(FPreviouslySelectedDetailRow).Delete();
                }
                else
                {
                    GetFieldApplicationRow(FPreviouslySelectedDetailRow).Delete();
                }

                FPreviouslySelectedDetailRow.Delete();
                FPetraUtilsObject.SetChangedFlag();

                // temporarily reset selected row to avoid interference with validation
                FPreviouslySelectedDetailRow = null;
                grdDetails.SelectRowInGrid(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
	            ShowDetails(FPreviouslySelectedDetailRow);

                DoRecalculateScreenParts();

                if (grdDetails.Rows.Count <= 1)
                {
                    // hide details part and disable buttons if no record in grid (first row for headings)
                    btnDelete.Enabled = false;
                    pnlDetails.Visible = false;
                }
            }
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        private bool IsEventApplication(IndividualDataTDSPmGeneralApplicationRow ARow)
        {
            // check if there is a corresponding record in short term applications
            if (FMainDS.PmShortTermApplication.Rows.Contains
                    (new object[] { ARow.PartnerKey, ARow.ApplicationKey, ARow.RegistrationOffice }))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private PmShortTermApplicationRow GetEventApplicationRow(IndividualDataTDSPmGeneralApplicationRow ARow)
        {
            DataRow Row;

            Row = FMainDS.PmShortTermApplication.Rows.Find
                      (new object[] { ARow.PartnerKey, ARow.ApplicationKey, ARow.RegistrationOffice });
            return (PmShortTermApplicationRow)Row;
        }

        private PmYearProgramApplicationRow GetFieldApplicationRow(IndividualDataTDSPmGeneralApplicationRow ARow)
        {
            DataRow Row;

            Row = FMainDS.PmYearProgramApplication.Rows.Find
                      (new object[] { ARow.PartnerKey, ARow.ApplicationKey, ARow.RegistrationOffice });
            return (PmYearProgramApplicationRow)Row;
        }

        private void ShowDetailsManual(IndividualDataTDSPmGeneralApplicationRow ARow)
        {
            if (ARow != null)
            {
                btnDelete.Enabled = true;
                pnlDetails.Visible = true;
            }

            if (IsEventApplication(ARow))
            {
                PmShortTermApplicationRow EventApplicationRow;
                EventApplicationRow = GetEventApplicationRow(ARow);
                pnlApplicationEvent.Visible = true;
                pnlApplicationField.Visible = false;
                ucoApplicationEvent.ShowDetails(ARow, EventApplicationRow);
            }
            else
            {
                PmYearProgramApplicationRow FieldApplicationRow;
                FieldApplicationRow = GetFieldApplicationRow(ARow);
                pnlApplicationEvent.Visible = false;
                pnlApplicationField.Visible = true;
                ucoApplicationField.ShowDetails(ARow, FieldApplicationRow);
            }

            // In theory, the next Method call could be done in Methods NewRowManual; however, NewRowManual runs before
            // the Row is actually added and this would result in the Count to be one too less, so we do the Method call here, short
            // of a non-existing 'AfterNewRowManual' Method....
            DoRecalculateScreenParts();
        }

        private void GetDetailsFromControls(IndividualDataTDSPmGeneralApplicationRow ARow)
        {
            if (IsEventApplication(ARow))
            {
                PmShortTermApplicationRow EventApplicationRow;
                EventApplicationRow = GetEventApplicationRow(ARow);
                ucoApplicationEvent.GetDetails(ARow, EventApplicationRow);
            }
            else
            {
                PmYearProgramApplicationRow FieldApplicationRow;
                FieldApplicationRow = GetFieldApplicationRow(ARow);
                ucoApplicationField.GetDetails(ARow, FieldApplicationRow);
            }
        }

        /// <summary>
        /// Gets the data from all controls on this UserControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls2()
        {
            // Get data out of the Controls only if there is at least one row of data (Note: Column Headers count as one row)
            if (grdDetails.Rows.Count > 1)
            {
                //GetDataFromControls();
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

        /// <summary>
        /// Loads Application Data from Petra Server into FMainDS, if not already loaded.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PmGeneralApplication == null)
                {
                    FMainDS.Tables.Add(new PmGeneralApplicationTable());

                    if (FMainDS.PmShortTermApplication == null)
                    {
                        FMainDS.Tables.Add(new PmShortTermApplicationTable());
                    }

                    if (FMainDS.PmYearProgramApplication == null)
                    {
                        FMainDS.Tables.Add(new PmYearProgramApplicationTable());
                    }

                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading
                    && (FMainDS.PmGeneralApplication.Rows.Count == 0))
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataPersonnelIndividualData(TIndividualDataItemEnum.idiApplications));

                    // Make DataRows unchanged
                    if (FMainDS.PmGeneralApplication.Rows.Count > 0)
                    {
                        if (FMainDS.PmGeneralApplication.Rows[0].RowState != DataRowState.Added)
                        {
                            FMainDS.PmGeneralApplication.AcceptChanges();
                        }
                    }

                    if (FMainDS.PmShortTermApplication.Rows.Count > 0)
                    {
                        if (FMainDS.PmShortTermApplication.Rows[0].RowState != DataRowState.Added)
                        {
                            FMainDS.PmShortTermApplication.AcceptChanges();
                        }
                    }

                    if (FMainDS.PmYearProgramApplication.Rows.Count > 0)
                    {
                        if (FMainDS.PmYearProgramApplication.Rows[0].RowState != DataRowState.Added)
                        {
                            FMainDS.PmYearProgramApplication.AcceptChanges();
                        }
                    }
                }

                if (FMainDS.PmGeneralApplication.Rows.Count != 0)
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
                return false;
            }
            catch (Exception)
            {
                throw;
            }

            return ReturnValue;
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_InsertKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            NewRowShortTermApp(this, null);
        }

        /// <summary>
        /// Event Handler for Grid Event
        /// </summary>
        /// <returns>void</returns>
        private void grdDetails_DeleteKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            if (e.Row != -1)
            {
                this.DeleteRow(this, null);
            }
        }

        private void FocusRowLeaving(object sender, SourceGrid.RowCancelEventArgs e)
        {
            if (grdDetails.Focused)
            {
	   	        // Clear any validation errors so that the following call to ValidateAllData starts with a 'clean slate'.
		        FPetraUtilsObject.VerificationResultCollection.Clear();
		        
                if (!ValidateAllData(true, true))
                {
                    e.Cancel = true;
                }
            }
            else
            {
                // This is needed because of a strange quirk in the Grid: if the user clicks with the Mouse to a different Row
                // (not when using the keyboard!), then the Method 'FocusRowLeaving' gets called twice, the second time
                // grdDetails.Focused is false. We need to Cancel in this case, otherwise the user can leave the Row with a
                // mouse click on another Row although it contains invalid data!!!
                e.Cancel = true;
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
            GetDetailsFromControls(GetSelectedDetailRow());
        }

        private void ProcessApplicationEventOrFieldChanged(Int64 APartnerKey, int AApplicationKey, Int64 ARegistrationOffice,
            Int64 AEventOrFieldKey, String AEventOrFieldName)
        {
            IndividualDataTDSPmGeneralApplicationRow Row;

            Row = (IndividualDataTDSPmGeneralApplicationRow)FMainDS.PmGeneralApplication.Rows.Find
                      (new object[] { APartnerKey, AApplicationKey, ARegistrationOffice });

            if (Row != null)
            {
                Row.EventOrFieldName = AEventOrFieldName;
            }
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <remarks>May be called by the Form that hosts this UserControl to invoke the data validation of
        /// the UserControl.</remarks>
        /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing
        /// to another record, otherwise set it to false.</param>
        /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
        /// user, otherwise set it to false.</param>
        /// <param name="AValidateSpecificControl">Pass in a Control to restrict Data Validation error checking to a
        /// specific Control for which Data Validation errors might have been recorded. (Default=this.ActiveControl).
        /// <para>
        /// This is useful for restricting Data Validation error checking to the current TabPage of a TabControl in order
        /// to only display Data Validation errors that pertain to the current TabPage. To do this, pass in a TabControl in
        /// this Argument.
        /// </para>
        /// </param>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        public bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors, Control AValidateSpecificControl = null)
        {
            bool ReturnValue = false;
            Control ControlToValidate;
            IndividualDataTDSPmGeneralApplicationRow CurrentRow;
            object OuterControl;

            CurrentRow = FPreviouslySelectedDetailRow;
            //CurrentRow = GetSelectedDetailRow();

            if (CurrentRow != null)
            {
                if (AValidateSpecificControl != null)
                {
                    ControlToValidate = AValidateSpecificControl;
                }
                else
                {
                    ControlToValidate = this.ActiveControl;
                }

                GetDetailsFromControls(CurrentRow);

                // TODO Generate automatic validation of data, based on the DB Table specifications (e.g. 'not null' checks)
                if (IsEventApplication(CurrentRow))
                {
                    ucoApplicationEvent.ValidateAllData(AProcessAnyDataValidationErrors);
                }
                else
                {
                    ucoApplicationField.ValidateAllData(AProcessAnyDataValidationErrors);
                }

                if (AProcessAnyDataValidationErrors)
                {
                    // Only process the Data Validations here if ControlToValidate is not null.
                    // It can be null if this.ActiveControl yields null - this would happen if no Control
                    // on this UserControl has got the Focus.
                    OuterControl = ControlToValidate.FindUserControlOrForm(true);

                    if ((OuterControl == this)
                        || (OuterControl == this.Parent)
                        || (OuterControl == this.Parent.Parent))
                    {
                        ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(false, FPetraUtilsObject.VerificationResultCollection,
                              this.GetType());
                    }
                    else
                    {
                        ReturnValue = true;
                    }
                }
            }
            else
            {
                ReturnValue = true;
            }

            if (ReturnValue)
            {
                // Remove a possibly shown Validation ToolTip as the data validation succeeded
                FPetraUtilsObject.ValidationToolTip.RemoveAll();
            }

            return ReturnValue;
        }
    }
}