// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template controlMaintainCachableTable
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
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{

  /// auto generated: {#FORMTITLE}
  public partial class {#CLASSNAME}: System.Windows.Forms.UserControl, {#INTERFACENAME}
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
    public {#CLASSNAME}() : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      {#CATALOGI18N}
      #endregion

      {#ASSIGNFONTATTRIBUTES}

    }

    /// helper object for the whole screen
    public {#UTILOBJECTCLASS} PetraUtilsObject
    {
        set
        {
            FPetraUtilsObject = value;
        }
    }
    {#IFDEF DATASETTYPE}
        /// dataset for the whole screen
    public {#DATASETTYPE} MainDS
    {
        set
        {
            FMainDS = value;
        }
    }
    {#ENDIF DATASETTYPE}

    /// <summary>Loads the data for the screen and finishes the setting up of the screen.</summary>
    /// <returns>void</returns>    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
      {#INITUSERCONTROLS}
      Type DataTableType;
      
      // Load Data
{#IFNDEF DATASETTYPE}
      FMainDS.{#DETAILTABLE} = new {#DETAILTABLE}Table();
{#ENDIFN DATASETTYPE}      
      DataTable CacheDT = TDataCache.{#CACHEABLETABLERETRIEVEMETHOD}({#CACHEABLETABLE}, {#CACHEABLETABLESPECIFICFILTERLOAD}, out DataTableType);
      FMainDS.{#DETAILTABLE}.Merge(CacheDT);    
      
      {#INITMANUALCODE}
{#IFDEF ACTIONENABLING}
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}
      
      DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

      ShowData();
    }
    
    {#EVENTHANDLERSIMPLEMENTATION}

	private bool newRecordUnsavedInFocus = false;
	
    /// automatically generated, create a new record of {#DETAILTABLE} and display on the edit screen
    public bool CreateNew{#DETAILTABLE}()
    {
        if(ValidateAllData(true, true))
        {
{#IFNDEF CANFINDWEBCONNECTOR_CREATEDETAIL}
            // we create the table locally, no dataset
            {#DETAILTABLETYPE}Row NewRow = FMainDS.{#DETAILTABLE}.NewRowTyped(true);
            {#INITNEWROWMANUAL}
            FMainDS.{#DETAILTABLE}.Rows.Add(NewRow);
{#ENDIFN CANFINDWEBCONNECTOR_CREATEDETAIL}
{#IFDEF CANFINDWEBCONNECTOR_CREATEDETAIL}
            FMainDS.Merge({#WEBCONNECTORDETAIL}.Create{#DETAILTABLE}({#CREATEDETAIL_ACTUALPARAMETERS_LOCAL}));
{#ENDIF CANFINDWEBCONNECTOR_CREATEDETAIL}

            FPetraUtilsObject.SetChangedFlag();

			grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.{#DETAILTABLE}.DefaultView);

            SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);
            InvokeFocusedRowChanged(grdDetails.SelectedRowIndex());

            //Must be set after the FocusRowChanged event is called as it sets this flag to false
            newRecordUnsavedInFocus = true;

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

    /// return the selected row
    public {#DETAILTABLE}Row GetSelectedDetailRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return ({#DETAILTABLE}Row)SelectedGridRow[0].Row;
        }

        return null;
    }

    private void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
        {#PRIMARYKEYCONTROLSREADONLY}
    }

    private void ShowData()
    {
        FPetraUtilsObject.DisableDataChangedEvent();
{#IFDEF SHOWDATA}        
        {#SHOWDATA}
{#ENDIF SHOWDATA}        
        pnlDetails.Enabled = false;
        if (FMainDS.{#DETAILTABLE} != null)
        {
            DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
{#IFDEF DETAILTABLESORT}
            myDataView.Sort = "{#DETAILTABLESORT}";
{#ENDIF DETAILTABLESORT}
{#IFDEF DETAILTABLEFILTER}
            myDataView.RowFilter = {#DETAILTABLEFILTER};
{#ENDIF DETAILTABLEFILTER}
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            if (myDataView.Count > 0)
            {
                grdDetails.Selection.ResetSelection(false);
                grdDetails.Selection.SelectRow(1, true);
                FocusedRowChanged(this, new SourceGrid.RowEventArgs(1));
                pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
            }
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

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

        if (FPreviouslySelectedDetailRow != null)
        {
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
            
{#IFDEF VALIDATEDATADETAILSMANUAL}
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
{#ENDIF VALIDATEDATADETAILSMANUAL}

            if (AProcessAnyDataValidationErrors)
            {
                ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                    this.GetType());    
            }
        }
        else
        {
            ReturnValue = true;
        }

        if(ReturnValue)
        {
            // Remove a possibly shown Validation ToolTip as the data validation succeeded
            FPetraUtilsObject.ValidationToolTip.RemoveAll();
        }

        return ReturnValue;
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
            {#SHOWDETAILS}
            pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
        }

        FPetraUtilsObject.EnableDataChangedEvent();
{#IFDEF SAVEDETAILS}
        grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
{#ENDIF SAVEDETAILS}
    }

    private {#DETAILTABLE}Row FPreviouslySelectedDetailRow = null;

    private bool firstFocusEventHasRun = false;
    private bool isRepeatLeaveEvent = false;
    private int gridRowsCount = 0;
    private int numGridRows	 = 0;
    private int gridRowsCountHasChanged = 0;
    private bool newFocusEventStarted = false;

    private void FocusPreparation(bool AIsLeaveEvent)
    {
    	if (isRepeatLeaveEvent)
    	{
    		return;
    	}
    	
    	numGridRows = grdDetails.Rows.Count;

		//first run only
    	if (!firstFocusEventHasRun)
    	{
    		firstFocusEventHasRun = true;
    		gridRowsCount = numGridRows;
    	}
    	
    	//Specify if it is a row change, add or delete
    	if (gridRowsCount == numGridRows)
    	{
    		gridRowsCountHasChanged = 0;
    	}
    	else if (gridRowsCount > numGridRows)
        {
        	gridRowsCount = numGridRows;
        	gridRowsCountHasChanged = -1;
        }
    	else if (gridRowsCount < numGridRows)
    	{
        	gridRowsCount = numGridRows;
        	gridRowsCountHasChanged = 1;
    	}
    	
    }
    
    private void InvokeFocusedRowChanged(int AGridRowNumber)
    {
		SourceGrid.RowEventArgs rowArgs  = new SourceGrid.RowEventArgs(AGridRowNumber);
		FocusedRowChanged(grdDetails, rowArgs);
    }
	
{#IFDEF SAVEDETAILS}

    private void FocusRowLeaving(object sender, SourceGrid.RowCancelEventArgs e)
    {        
		//Ignore this event if currently sorting
    	if (grdDetails.Sorting)
    	{
    		newFocusEventStarted = false;
    		return;
    	}    	
    	
    	if (newFocusEventStarted == false)
    	{
    		newFocusEventStarted = true;
    	}

    	FocusPreparation(true);

    	if (!isRepeatLeaveEvent)
        {
	    	isRepeatLeaveEvent = true;
	    	
            if (gridRowsCountHasChanged == -1 || numGridRows == 2)  //do not run validation if cancelling current row
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
	    	isRepeatLeaveEvent = false;
            e.Cancel = true;
        }
    }
{#ENDIF SAVEDETAILS}

    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
        newRecordUnsavedInFocus = false;
    	
        isRepeatLeaveEvent = false;

        if (!grdDetails.Sorting)
        {
	    	//Sometimes, FocusedRowChanged get called without FocusRowLeaving
	    	//  so need to handle that
	    	if (!newFocusEventStarted)
	    	{
	    		//This implies start of a new event chain without a previous FocusRowLeaving
	    		FocusPreparation(false);
	    	}
	    	
	        //Only allow, row change, add or delete, not repeat events from grid changing focus
	    	if(e.Row != FCurrentRow && gridRowsCountHasChanged == 0)
	        {
	    		// Transfer data from Controls into the DataTable
	            if (FPreviouslySelectedDetailRow != null)
	            {
	                GetDetailsFromControls(FPreviouslySelectedDetailRow);
	            }
	
	            // Display the details of the currently selected Row
	            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
	            ShowDetails(FPreviouslySelectedDetailRow);
	            pnlDetails.Enabled = true;
	    	}
	    	else if (gridRowsCountHasChanged == 1) //Addition
	    	{
	    		
	    	}
	    	else if (gridRowsCountHasChanged == -1) //Deletion
	    	{
	    		if (numGridRows > 1) //Implies at least one record still left
	    		{
	    			// Select and display the details of the currently selected Row without causing an event
	    			grdDetails.SelectRowInGrid(e.Row, TSgrdDataGrid.TInvokeGridFocusEventEnum.NoFocusEvent);
		            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
		            ShowDetails(FPreviouslySelectedDetailRow);
		            pnlDetails.Enabled = true;
	    		}
	    		else	
	    		{
	                FPreviouslySelectedDetailRow = null;
		            pnlDetails.Enabled = false;
	    		}
	    	}
        }
        
    	FCurrentRow = e.Row;
	    
	    //Event chain tidy-up
		gridRowsCountHasChanged = 0;
	    newFocusEventStarted = false;
	}
{#ENDIF SHOWDETAILS}
    
{#IFDEF SAVEDETAILS}
    private void GetDetailsFromControls({#DETAILTABLE}Row ARow)
    {
        if (ARow != null && !grdDetails.Sorting)
        {
            ARow.BeginEdit();
            {#SAVEDETAILS}
            ARow.EndEdit();
        }
    }
{#ENDIF SAVEDETAILS}

#region Implement interface functions
    
    /// auto generated
    public void RunOnceOnActivation()
    {
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

    /// <summary>
    /// Get the latest data from the controls and validate it
    /// </summary>
    /// <returns>True if data was validated successfully, otherwise false.  If successful, it is safe to call SaveChanges()</returns>    
    public bool ValidateBeforeSave()
    {
        // Call this before any call to SaveChanges().  It will automatically get the data from the current controls first.
        return ValidateAllData(false, true);
    }

    /// <summary>
    /// save the changes on the screen
    /// </summary>
    /// <returns>True if data was saved successfully, otherwise false.</returns>    
    public bool SaveChanges()
    {
        // Be sure to have called ValidateBeforeSave() before calling this method

        // Clear any validation errors so that the following call to ValidateAllData starts with a 'clean slate'.
        FPetraUtilsObject.VerificationResultCollection.Clear();

        foreach (DataRow InspectDR in FMainDS.{#DETAILTABLE}.Rows)
        {
            InspectDR.EndEdit();
        }

        if (!FPetraUtilsObject.HasChanges)
        {
            return true;
        }
        else
        {
            FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataInProgress);
            this.Cursor = Cursors.WaitCursor;

            TSubmitChangesResult SubmissionResult;
            TVerificationResultCollection VerificationResult;

            Ict.Common.Data.TTypedDataTable SubmitDT = FMainDS.{#DETAILTABLE}.GetChangesTyped();

            if (SubmitDT == null)
            {
                // nothing to be saved, so it is ok to close the screen etc
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
                    
                return false;
            }
            catch (EDBConcurrencyException Exp)
            {
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                this.Cursor = Cursors.Default;

                TMessages.MsgDBConcurrencyException(Exp, this.GetType());
                    
                return false;
            }
            catch (Exception)
            {
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                this.Cursor = Cursors.Default;
                    
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

                    SetPrimaryKeyReadOnly(true);

                    return true;

                case TSubmitChangesResult.scrError:
                    this.Cursor = Cursors.Default;
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataErrorOccured);

                    MessageBox.Show(Messages.BuildMessageFromVerificationResult(null, VerificationResult), 
                        Catalog.GetString("Data Cannot Be Saved"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    FPetraUtilsObject.SubmitChangesContinue = false;
                        
                    return false;

                case TSubmitChangesResult.scrNothingToBeSaved:
                    this.Cursor = Cursors.Default;
                    return true;

                case TSubmitChangesResult.scrInfoNeeded:

                    // TODO scrInfoNeeded
                    this.Cursor = Cursors.Default;
                    break;
            }

			//The sorting will be affected when a new row is saved, so need to reselect row
			if (newRecordUnsavedInFocus)
			{
				SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);
				InvokeFocusedRowChanged(grdDetails.SelectedRowIndex());
			}
        }

        return false;
    }

#endregion
{#IFDEF ACTIONENABLING}

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        {#ACTIONENABLING}
        {#ACTIONENABLINGDISABLEMISSINGFUNCS}
    }

    {#ACTIONHANDLERS}

#endregion
{#ENDIF ACTIONENABLING}
  }
}

{#INCLUDE copyvalues.cs}