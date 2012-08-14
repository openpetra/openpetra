// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template windowMaintainCachableTable
//
// DO NOT edit manually, DO NOT edit with the designer
//
{#GPLFILEHEADER}
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using SourceGrid;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Common.Remoting.Shared;
{#IFDEF SHAREDVALIDATIONNAMESPACEMODULE}
using {#SHAREDVALIDATIONNAMESPACEMODULE};
{#ENDIF SHAREDVALIDATIONNAMESPACEMODULE}
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{

  /// auto generated: {#FORMTITLE}
  public partial class {#CLASSNAME}: System.Windows.Forms.Form, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;
{#FILTERVAR}
{#IFDEF DATASETTYPE}
    private {#DATASETTYPE} FMainDS;
{#ENDIF DATASETTYPE}
{#IFNDEF DATASETTYPE}
    private class FMainDS
    {
        public static {#DETAILTABLE}Table {#DETAILTABLE};
    }
{#ENDIFN DATASETTYPE} 
{#IFDEF SHOWDETAILS}

    private int FCurrentRow;
{#ENDIF SHOWDETAILS}

    /// constructor
    public {#CLASSNAME}(Form AParentForm) : base()
    {
      Control[] FoundCheckBoxes;  
      
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      {#CATALOGI18N}
      #endregion
      
      {#ASSIGNFONTATTRIBUTES}
      
      FPetraUtilsObject = new {#UTILOBJECTCLASS}(AParentForm, this, stbMain);
      {#IFDEF DATASETTYPE}
      FMainDS = new {#DATASETTYPE}();
      {#ENDIF DATASETTYPE}
      {#INITUSERCONTROLS}
      
      /*
       * Automatically disable 'Deletable' CheckBox (it must not get changed by the user because records where the 
       * 'Deletable' flag is true are system records that must not be deleted)
       */
      FoundCheckBoxes = this.Controls.Find("chkDetailDeletable", true);
      
      if (FoundCheckBoxes.Length > 0) 
      {
          FoundCheckBoxes[0].Enabled = false;
      }
      
      {#LOADDATAONCONSTRUCTORRUN}

{#IFDEF MASTERTABLE OR DETAILTABLE}
      BuildValidationControlsDict();
{#ENDIF MASTERTABLE OR DETAILTABLE}
    }

    {#EVENTHANDLERSIMPLEMENTATION}

    /// <summary>Loads the data for the screen and finishes the setting up of the screen.</summary>
    /// <returns>void</returns>
    private void LoadDataAndFinishScreenSetup()
    {      
      Type DataTableType;
      
      // Load Data
{#IFNDEF DATASETTYPE}
      FMainDS.{#DETAILTABLE} = new {#DETAILTABLE}Table();
{#ENDIFN DATASETTYPE}      
      DataTable CacheDT = TDataCache.{#CACHEABLETABLERETRIEVEMETHOD}({#CACHEABLETABLE}, {#CACHEABLETABLESPECIFICFILTERLOAD}, out DataTableType);
      FMainDS.{#DETAILTABLE}.Merge(CacheDT);    
      
      {#INITMANUALCODE}
      
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
      
      DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

      // Ensure that the Details Panel is disabled if there are no records
      if (FMainDS.{#DETAILTABLE}.Rows.Count == 0) 
      {
        ShowDetails(null);
      }
      
      {#INITACTIONSTATE}            
      {#DISPLAYFILTERINFORMTITLE}
    }
    
    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position

    }

	private bool FNewRecordUnsavedInFocus = false;
	
    /// automatically generated, create a new record of {#DETAILTABLE} and display on the edit screen
    /// we create the table locally, no dataset
    public bool CreateNew{#DETAILTABLE}()
    {
        if(ValidateAllData(true, true))
        {
            {#DETAILTABLE}Row NewRow = FMainDS.{#DETAILTABLE}.NewRowTyped();
            {#INITNEWROWMANUAL}
            FMainDS.{#DETAILTABLE}.Rows.Add(NewRow);
            
            FPetraUtilsObject.SetChangedFlag();

			grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.{#DETAILTABLE}.DefaultView);

            int newRowIndex = FMainDS.{#DETAILTABLE}.Rows.Count - 1;
			SelectDetailRowByDataTableIndex(newRowIndex);
            InvokeFocusedRowChanged(grdDetails.SelectedRowIndex());

            //Must be set after the FocusRowChanged event is called as it sets this flag to false
            FNewRecordUnsavedInFocus = true;

            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
            ShowDetails(FPreviouslySelectedDetailRow);
			
            Control[] pnl = this.Controls.Find("pnlDetails", true);
            if (pnl.Length > 0)
            {
	            //Look for Key & Description fields
	            bool keyFieldFound = false;
	            foreach (Control detailsCtrl in pnl[0].Controls)
	            {
	                if (!keyFieldFound && (detailsCtrl is TextBox || detailsCtrl is ComboBox))
	                {
	                    keyFieldFound = true;
	                    detailsCtrl.Focus();
	                }
	
	                if (detailsCtrl is TextBox && detailsCtrl.Name.Contains("Descr") && detailsCtrl.Text == string.Empty)
	                {
	                    detailsCtrl.Text = "PLEASE ENTER DESCRIPTION";
	                    break;
	                }
	            }

				GetDetailsFromControls(FPreviouslySelectedDetailRow, true);

                //Need to redo this just in case the sorting is not on primary key
	            SelectDetailRowByDataTableIndex(newRowIndex);
            }
	
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.{#DETAILTABLE}.PrimaryKey)
            {
                string value1 = FMainDS.{#DETAILTABLE}.Rows[ARowNumberInTable][myColumn].ToString();
                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][myColumn.Ordinal].ToString();
                if (value1 != value2)
                {
                    found = false;
                }
            }
            if (found)
            {
                RowNumberGrid = Counter + 1;
                break;
            }
        }

        grdDetails.SelectRowInGrid(RowNumberGrid, TSgrdDataGrid.TInvokeGridFocusEventEnum.NoFocusEvent);
    }

    private int GetDetailGridRowDataTableIndex()
    {
    	Int32 RowNumberInData = -1;
    	
    	int gridRowIndex = grdDetails.SelectedRowIndex();
    	
    	if (gridRowIndex > 0 && FPreviouslySelectedDetailRow != null)
    	{
    	    	
	    	int dataRowIndex = 0;
	    	
	    	foreach ({#DETAILTABLETYPE}Row myRow in FMainDS.{#DETAILTABLETYPE}.Rows)
	        {
	    		bool found = true;
	            foreach (DataColumn myColumn in FMainDS.{#DETAILTABLETYPE}.PrimaryKey)
	            {
	                string value1 = myRow[myColumn].ToString();
	                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[gridRowIndex - 1][myColumn.Ordinal].ToString();
	                if (value1 != value2)
	                {
	                    found = false;
	                }
	            }
	            
	            if (found)
	            {
	                RowNumberInData = dataRowIndex;
	                break;
	            }
	            
	            dataRowIndex++;
	        }
    	}
    	
    	return RowNumberInData;
    }
	
{#IFDEF SHOWDETAILS OR GENERATEGETSELECTEDDETAILROW}

    /// return the selected row
    public {#DETAILTABLETYPE}Row GetSelectedDetailRow()
    {
        {#GETSELECTEDDETAILROW}
    }
{#ENDIF SHOWDETAILS OR GENERATEGETSELECTEDDETAILROW}


    private void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
        {#PRIMARYKEYCONTROLSREADONLY}
    }

{#IFDEF SHOWDATA}
    private void ShowData({#MASTERTABLE}Row ARow)
    {
        {#SHOWDATA}
    }
{#ENDIF SHOWDATA}
{#IFDEF UNDODATA}

    private void UndoData(DataRow ARow, Control AControl)
    {
        {#UNDODATA}
    }
{#ENDIF UNDODATA}

    /// <summary>
    /// Performs data validation.
    /// </summary>
    /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing 
    /// to another record, otherwise set it to false.</param>
    /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
    /// user, otherwise set it to false.</param>
    /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>    
    private bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors)
    {
        bool ReturnValue = false;
        // Record a new Data Validation Run. (All TVerificationResults/TScreenVerificationResults that are created during this 'run' are associated with this 'run' through that.)
        FPetraUtilsObject.VerificationResultCollection.RecordNewDataValidationRun();

{#IFDEF SHOWDETAILS}
        if (FPreviouslySelectedDetailRow != null)
        {
{#ENDIF SHOWDETAILS}        
{#IFNDEF SHOWDETAILS}
{#IFDEF MASTERTABLE}
        GetDataFromControls(FMainDS.{#MASTERTABLE}[0]);
        ValidateData(FMainDS.{#MASTERTABLE}[0]);
{#ENDIF MASTERTABLE}        
{#ENDIFN SHOWDETAILS}
{#IFDEF SHOWDETAILS}
        GetDetailsFromControls(FPreviouslySelectedDetailRow);
        ValidateDataDetails(FPreviouslySelectedDetailRow);
{#ENDIF SHOWDETAILS}

{#IFDEF VALIDATEDATADETAILSMANUAL}
{#IFDEF SHOWDETAILS}
            // Remember the current rowID and perform automatic validation of data, based on the DB Table specifications (e.g. 'not null' checks)
            int previousRowNum = FCurrentRow;// grdDetails.DataSourceRowToIndex2(CurrentRow) + 1;
            ValidateDataDetailsManual(FPreviouslySelectedDetailRow);

            // Validation might have moved the row, so we need to locate it again, updating our FCurrentRow global variable
            FCurrentRow = grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow) + 1;
            if (FCurrentRow != previousRowNum)
            {
                // Yes it did move so we need to keep the row selected, without firing off the event that brought us here in the first place!
                grdDetails.Selection.FocusRowLeaving -= new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
                grdDetails.SelectRowInGrid(FCurrentRow);
                grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
            }
{#ENDIF SHOWDETAILS}            
{#ENDIF VALIDATEDATADETAILSMANUAL}
{#IFDEF VALIDATEDATAMANUAL}
{#IFDEF MASTERTABLE}
            ValidateDataManual(FMainDS.{#MASTERTABLE}[0]);
{#ENDIF MASTERTABLE}
{#ENDIF VALIDATEDATAMANUAL}
{#IFDEF PERFORMUSERCONTROLVALIDATION}

            // Perform validation in UserControls, too
            {#USERCONTROLVALIDATION}
{#ENDIF PERFORMUSERCONTROLVALIDATION}
        
        if (AProcessAnyDataValidationErrors)
        {
            ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                this.GetType(), null);
        }
{#IFDEF SHOWDETAILS}            
        }
        else
        {
            ReturnValue = true;
        }
{#ENDIF SHOWDETAILS}

        if(ReturnValue)
        {
            // Remove a possibly shown Validation ToolTip as the data validation succeeded
            FPetraUtilsObject.ValidationToolTip.RemoveAll();
        }

        return ReturnValue;
    }

    private void InvokeFocusedRowChanged(int AGridRowNumber)
    {
		SourceGrid.RowEventArgs rowArgs  = new SourceGrid.RowEventArgs(AGridRowNumber);
		FocusedRowChanged(grdDetails, rowArgs);
    }

{#IFDEF SHOWDETAILS}
    private void ShowDetails({#DETAILTABLE}Row ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
{#IFDEF SAVEDETAILS}
        grdDetails.Selection.FocusRowLeaving -= new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
{#ENDIF SAVEDETAILS}

        if (ARow == null)
        {
            pnlDetails.Enabled = false;
            {#CLEARDETAILS}
        }
        else
        {
            FPreviouslySelectedDetailRow = ARow;
            pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
            {#SHOWDETAILS}
        }
        
        FPetraUtilsObject.EnableDataChangedEvent();
{#IFDEF SAVEDETAILS}
        grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
{#ENDIF SAVEDETAILS}
    }

    private {#DETAILTABLE}Row FPreviouslySelectedDetailRow = null;   
	
    private bool FInitialFocusEventCompleted = false;
    private bool FNewFocusEvent = false;
    private bool FRepeatLeaveEventDetected = false;
    private int FDetailGridRowsCountPrevious = 0;
    private int FDetailGridRowsCountCurrent = 0;
    private int FDetailGridRowsChangedState = 0;

    private void FocusPreparation(bool AIsLeaveEvent)
    {
    	if (FRepeatLeaveEventDetected)
    	{
    		return;
    	}
    	
    	FDetailGridRowsCountCurrent = grdDetails.Rows.Count;

		//first run only
    	if (!FInitialFocusEventCompleted)
    	{
    		FInitialFocusEventCompleted = true;
    		FDetailGridRowsCountPrevious = FDetailGridRowsCountCurrent;
    	}
    	
    	//Specify if it is a row change, add or delete
    	if (FDetailGridRowsCountPrevious == FDetailGridRowsCountCurrent)
    	{
    		FDetailGridRowsChangedState = 0;
    	}
    	else if (FDetailGridRowsCountPrevious > FDetailGridRowsCountCurrent)
        {
        	FDetailGridRowsCountPrevious = FDetailGridRowsCountCurrent;
        	FDetailGridRowsChangedState = -1;
        }
    	else if (FDetailGridRowsCountPrevious < FDetailGridRowsCountCurrent)
    	{
        	FDetailGridRowsCountPrevious = FDetailGridRowsCountCurrent;
        	FDetailGridRowsChangedState = 1;
    	}
    	
    }
    
{#IFDEF SAVEDETAILS}

    private void FocusRowLeaving(object sender, SourceGrid.RowCancelEventArgs e)
    {        
		//Ignore this event if currently sorting
    	if (grdDetails.Sorting)
    	{
    		FNewFocusEvent = false;
    		return;
    	}    	
    	
    	if (FNewFocusEvent == false)
    	{
    		FNewFocusEvent = true;
    	}

    	FocusPreparation(true);

    	if (!FRepeatLeaveEventDetected)
        {
	    	FRepeatLeaveEventDetected = true;
	    	
            if (FDetailGridRowsChangedState == -1 || FDetailGridRowsCountCurrent == 2)  //do not run validation if cancelling current row
																	// OR only 1 row present so no rowleaving event possible
            {
            	e.Cancel = true;
            }
            
            Console.WriteLine("FocusRowLeaving");
            
            if (!ValidateAllData(true, true))
            {
                e.Cancel = true;
            }
        }
        else
        {
            // Reset flag
	    	FRepeatLeaveEventDetected = false;
            e.Cancel = true;
        }
    }
{#ENDIF SAVEDETAILS}

    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
        FNewRecordUnsavedInFocus = false;
    	
        FRepeatLeaveEventDetected = false;

        if (!grdDetails.Sorting)
        {
	    	//Sometimes, FocusedRowChanged get called without FocusRowLeaving
	    	//  so need to handle that
	    	if (!FNewFocusEvent)
	    	{
	    		//This implies start of a new event chain without a previous FocusRowLeaving
	    		FocusPreparation(false);
	    	}
	    	
	        //Only allow, row change, add or delete, not repeat events from grid changing focus
	    	if(e.Row != FCurrentRow && FDetailGridRowsChangedState == 0)
	        {
	    		// Transfer data from Controls into the DataTable
	            if (FPreviouslySelectedDetailRow != null)
	            {
	                GetDetailsFromControls(FPreviouslySelectedDetailRow);
	            }
	
	            // Display the details of the currently selected Row
	            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
	            pnlDetails.Enabled = true;
	            ShowDetails(FPreviouslySelectedDetailRow);
	    	}
	    	else if (FDetailGridRowsChangedState == 1) //Addition
	    	{
	    		
	    	}
	    	else if (FDetailGridRowsChangedState == -1) //Deletion
	    	{
                if (FDetailGridRowsCountCurrent > 1) //Implies at least one record still left
                {
                    int nextRowToSelect = e.Row;
                    //If last row deleted, subtract row index to select by 1
                    if (nextRowToSelect == FDetailGridRowsCountCurrent)
                    {
                    	nextRowToSelect--;
                    }
                	// Select and display the details of the currently selected Row without causing an event
                    grdDetails.SelectRowInGrid(nextRowToSelect, TSgrdDataGrid.TInvokeGridFocusEventEnum.NoFocusEvent);
                    FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                    pnlDetails.Enabled = true;
                    ShowDetails(FPreviouslySelectedDetailRow);
                }
                else
                {
                    e.Row = 0;
                	FPreviouslySelectedDetailRow = null;
                    pnlDetails.Enabled = false;
                }
	    	}
        }
        
    	FCurrentRow = e.Row;
	    
	    //Event chain tidy-up
		FDetailGridRowsChangedState = 0;
	    FNewFocusEvent = false;
	}

    private void Delete{#DETAILTABLE}()
    {
		bool allowDeletion = true;
		bool deletionPerformed = false;
		string deletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");
		string completionMessage = string.Empty;
		
		if (FPreviouslySelectedDetailRow == null)
		{
			return;
		}

		int rowIndexToDelete = grdDetails.SelectedRowIndex();

        if (rowIndexToDelete == -1)
        {
        	MessageBox.Show(Catalog.GetString("There is no row currently selected in the grid."),
        	               Catalog.GetString("Delete Row"));
        	return;
        }

		{#DETAILTABLETYPE}Row rowToDelete = GetSelectedDetailRow();
		
		{#PREDELETEMANUAL}
		
		if(allowDeletion)
		{
        	if ((MessageBox.Show(deletionQuestion,
					 Catalog.GetString("Confirm Delete"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
			{
{#IFDEF DELETEROWMANUAL}
				{#DELETEROWMANUAL}
{#ENDIF DELETEROWMANUAL}
{#IFNDEF DELETEROWMANUAL}				
				rowToDelete.Delete();
				deletionPerformed = true;
{#ENDIFN DELETEROWMANUAL}				
			
				if (deletionPerformed)
				{
					FPetraUtilsObject.SetChangedFlag();
					//Select and call the event that doesn't occur automatically
					InvokeFocusedRowChanged(rowIndexToDelete);
				}
			}
		}

{#IFDEF POSTDELETEMANUAL}
		{#POSTDELETEMANUAL}
{#ENDIF POSTDELETEMANUAL}
{#IFNDEF POSTDELETEMANUAL}
		if(deletionPerformed && completionMessage.Length > 0)
		{
			MessageBox.Show(completionMessage,
							 Catalog.GetString("Deletion Completed"));
		}
{#ENDIFN POSTDELETEMANUAL}

    }

    private void ResetGridFocus(int ADataRowPosition)
    {
    	int selectedRowIndex = grdDetails.SelectedRowIndex();
    	
    	if (selectedRowIndex > 0 && !grdDetails.Sorting)
    	{
    		if (ADataRowPosition != GetDetailGridRowDataTableIndex())
	        {
	            grdDetails.DataSource = null;
	            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.{#DETAILTABLE}.DefaultView);
	        	SelectDetailRowByDataTableIndex(ADataRowPosition);
	        	InvokeFocusedRowChanged(grdDetails.SelectedRowIndex());
	        }
    	}

    }
{#ENDIF SHOWDETAILS}
{#IFDEF MASTERTABLE}

    private void GetDataFromControls({#MASTERTABLETYPE}Row ARow, Control AControl=null)
    {
{#IFDEF SAVEDATA}
        {#SAVEDATA}
{#ENDIF SAVEDATA}
    }
{#ENDIF MASTERTABLE}
{#IFNDEF MASTERTABLE}

    private void GetDataFromControls(Control AControl=null)
    {
{#IFDEF SAVEDATA}
        {#SAVEDATA}
{#ENDIF SAVEDATA}
    }
{#ENDIFN MASTERTABLE}    
{#IFDEF SAVEDETAILS}
    private void GetDetailsFromControls({#DETAILTABLE}Row ARow, bool AIsNewRow = false, Control AControl=null)
    {
        if (ARow != null && !grdDetails.Sorting)
        {
            if (AIsNewRow)
            {
				{#SAVEDETAILS}
            }
            else
            {
				ARow.BeginEdit();
				{#SAVEDETAILS}
				ARow.EndEdit();
            }
        }
    }
{#IFDEF GENERATECONTROLUPDATEDATAHANDLER}

    private void ControlUpdateDataHandler(object sender, EventArgs e)
    {
        GetDetailsFromControls(FPreviouslySelectedDetailRow, false, (Control)sender);
    }
{#ENDIF GENERATECONTROLUPDATEDATAHANDLER}
{#ENDIF SAVEDETAILS}

#region Implement interface functions

    /// auto generated
    public void RunOnceOnActivation()
    {
        {#RUNONCEONACTIVATIONMANUAL}
        {#RUNONCEINTERFACEIMPLEMENTATION}
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    public void HookupAllControls()
    {
        {#HOOKUPINTERFACEIMPLEMENTATION}
    }

    /// auto generated
    public void HookupAllInContainer(Control container)
    {
        FPetraUtilsObject.HookupAllInContainer(container);
    }

    /// auto generated
    public bool CanClose()
    {
        return FPetraUtilsObject.CanClose();
    }

    /// auto generated
    public TFrmPetraUtils GetPetraUtilsObject()
    {
        return (TFrmPetraUtils)FPetraUtilsObject;
    }

    /// auto generated
    public void FileSave(object sender, EventArgs e)
    {
        SaveChanges();
    }

    /// <summary>
    /// save the changes on the screen
    /// </summary>
    /// <returns></returns>
    public bool SaveChanges()
    {
        bool ReturnValue = false;

        FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

        GetDetailsFromControls(FPreviouslySelectedDetailRow);

        // Clear any validation errors so that the following call to ValidateAllData starts with a 'clean slate'.
        FPetraUtilsObject.VerificationResultCollection.Clear();

        if (ValidateAllData(false, true))
        {       
            foreach (DataRow InspectDR in FMainDS.{#DETAILTABLE}.Rows)
            {
                InspectDR.EndEdit();
            }

            if (FPetraUtilsObject.HasChanges)
            {
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataInProgress);
                this.Cursor = Cursors.WaitCursor;

                TSubmitChangesResult SubmissionResult;
                TVerificationResultCollection VerificationResult;

                Ict.Common.Data.TTypedDataTable SubmitDT = FMainDS.{#DETAILTABLE}.GetChangesTyped();

                if (SubmitDT == null)
                {
                    // There is nothing to be saved.
                    // Update UI
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);
                    this.Cursor = Cursors.Default;

                    // We don't have unsaved changes anymore
                    FPetraUtilsObject.DisableSaveButton();
                    
                    return true;
                }
                
                // Submit changes to the PETRAServer
                try
                {
                    SubmissionResult = TDataCache.{#CACHEABLETABLESAVEMETHOD}({#CACHEABLETABLE}, ref SubmitDT{#CACHEABLETABLESPECIFICFILTERSAVE}, out VerificationResult);
                }
                catch (ESecurityDBTableAccessDeniedException Exp)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                    this.Cursor = Cursors.Default;

                    TMessages.MsgSecurityException(Exp, this.GetType());
                    
                    ReturnValue = false;
                    FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
                catch (EDBConcurrencyException Exp)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                    this.Cursor = Cursors.Default;

                    TMessages.MsgDBConcurrencyException(Exp, this.GetType());
                    
                    ReturnValue = false;
                    FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
                catch (Exception)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                    this.Cursor = Cursors.Default;
                    
                    FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));                    
                    throw;
                }

                switch (SubmissionResult)
                {
                    case TSubmitChangesResult.scrOK:
                        // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                        FMainDS.{#DETAILTABLE}.AcceptChanges();

                        // Merge back with data from the Server (eg. for getting Sequence values)
                        SubmitDT.AcceptChanges();
                        FMainDS.{#DETAILTABLE}.Merge(SubmitDT, false);

                        // need to accept the new modification ID
                        FMainDS.{#DETAILTABLE}.AcceptChanges();

                        // Update UI
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataSuccessful);
                        this.Cursor = Cursors.Default;

                        // We don't have unsaved changes anymore
                        FPetraUtilsObject.DisableSaveButton();

                        SetPrimaryKeyReadOnly(true);

                        ReturnValue = true;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));

                        if((VerificationResult != null)
                            && (VerificationResult.HasCriticalOrNonCriticalErrors))
                        {
                            TDataValidation.ProcessAnyDataValidationErrors(false, VerificationResult,
                                this.GetType(), null);
                        }

                        break;

                    case TSubmitChangesResult.scrError:
                        this.Cursor = Cursors.Default;
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataErrorOccured);

                        TDataValidation.ProcessAnyDataValidationErrors(false, VerificationResult,
                            this.GetType(), null);

                        FPetraUtilsObject.SubmitChangesContinue = false;
                        
                        ReturnValue = false;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        break;

                    case TSubmitChangesResult.scrNothingToBeSaved:
                        this.Cursor = Cursors.Default;
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);

                        // We don't have unsaved changes anymore
                        FPetraUtilsObject.DisableSaveButton();
                        
                        ReturnValue = true;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        break;

                    case TSubmitChangesResult.scrInfoNeeded:

                        // TODO scrInfoNeeded
                        this.Cursor = Cursors.Default;
                        break;
                }
				
				//The sorting will be affected when a new row is saved, so need to reselect row
				if (FNewRecordUnsavedInFocus)
				{
					SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);
					InvokeFocusedRowChanged(grdDetails.SelectedRowIndex());
				}
            }
            else
            {
                // Update UI
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);
                this.Cursor = Cursors.Default;
                FPetraUtilsObject.DisableSaveButton();

                // We don't have unsaved changes anymore
                FPetraUtilsObject.HasChanges = false;

                ReturnValue = true;
                FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
            }                
        }

        return ReturnValue;
    }

#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        {#ACTIONENABLING}
        {#ACTIONENABLINGDISABLEMISSINGFUNCS}
    }

    {#ACTIONHANDLERS}

#endregion

#region Data Validation
    
    private void ControlValidatedHandler(object sender, EventArgs e)
    {
{#IFDEF SHOWDETAILS}
    	int dataRowIndex = GetDetailGridRowDataTableIndex();
{#ENDIF SHOWDETAILS}
        TScreenVerificationResult SingleVerificationResult;
        
        ValidateAllData(true, false);
        
        FPetraUtilsObject.ValidationToolTip.RemoveAll();
        
        if (FPetraUtilsObject.VerificationResultCollection.Count > 0) 
        {
            for (int Counter = 0; Counter < FPetraUtilsObject.VerificationResultCollection.Count; Counter++) 
            {
                SingleVerificationResult = (TScreenVerificationResult)FPetraUtilsObject.VerificationResultCollection[Counter];
                
                if (SingleVerificationResult.ResultControl == sender) 
                {
                    if (FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested)
                    {
                        SingleVerificationResult.ResultControl.Focus();
                        FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = false;
                    }

{#IFDEF UNDODATA}
                    if(SingleVerificationResult.ControlValueUndoRequested)
                    {
                        UndoData(SingleVerificationResult.ResultColumn.Table.Rows[0], SingleVerificationResult.ResultControl);
                        SingleVerificationResult.OverrideResultText(SingleVerificationResult.ResultText + Environment.NewLine + Environment.NewLine + 
                            Catalog.GetString("--> The value you entered has been changed back to what it was before! <--"));
                    }

{#ENDIF UNDODATA}
                    if (!SingleVerificationResult.SuppressValidationToolTip) 
                    {
                        FPetraUtilsObject.ValidationToolTipSeverity = SingleVerificationResult.ResultSeverity;

                        if (SingleVerificationResult.ResultTextCaption != String.Empty) 
                        {
                            FPetraUtilsObject.ValidationToolTip.ToolTipTitle += ":  " + SingleVerificationResult.ResultTextCaption;    
                        }

                        FPetraUtilsObject.ValidationToolTip.Show(SingleVerificationResult.ResultText, (Control)sender, 
                            ((Control)sender).Width / 2, ((Control)sender).Height);
					}
                }
            }
        }
{#IFDEF SHOWDETAILS}
        ResetGridFocus(dataRowIndex);
{#ENDIF SHOWDETAILS}        
    }
{#IFDEF MASTERTABLE}
    private void ValidateData({#MASTERTABLE}Row ARow)
    {
        TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

        {#MASTERTABLE}Validation.Validate(this, ARow, ref VerificationResultCollection,
            FPetraUtilsObject.ValidationControlsDict);
    }
{#ENDIF MASTERTABLE}
{#IFDEF DETAILTABLE}
    private void ValidateDataDetails({#DETAILTABLE}Row ARow)
    {
        TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

        {#DETAILTABLE}Validation.Validate(this, ARow, ref VerificationResultCollection,
            FPetraUtilsObject.ValidationControlsDict);
    }
{#ENDIF DETAILTABLE}
{#IFDEF MASTERTABLE OR DETAILTABLE}

    private void BuildValidationControlsDict()
    {
{#IFDEF ADDCONTROLTOVALIDATIONCONTROLSDICT}
        {#ADDCONTROLTOVALIDATIONCONTROLSDICT}
{#ENDIF ADDCONTROLTOVALIDATIONCONTROLSDICT}
    }
{#ENDIF MASTERTABLE OR DETAILTABLE}    

#endregion
  }
}

{#INCLUDE copyvalues.cs}
{#INCLUDE validationcontrolsdict.cs}