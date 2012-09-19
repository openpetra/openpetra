// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template controlMaintainTable
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
using System.Resources;
using System.Collections.Specialized;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;
using GNU.Gettext;
using SourceGrid;
{#IFDEF SHAREDVALIDATIONNAMESPACEMODULE}
using {#SHAREDVALIDATIONNAMESPACEMODULE};
{#ENDIF SHAREDVALIDATIONNAMESPACEMODULE}
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{

  /// auto generated user control
  public partial class {#CLASSNAME}: System.Windows.Forms.UserControl, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;
  
    /// <summary>
    /// Dictionary that contains Controls on whose data Data Validation should be run.
    /// </summary>
    private TValidationControlsDict FValidationControlsDict = new TValidationControlsDict();
  
    private {#DATASETTYPE} FMainDS;
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

    /// dataset for the whole screen
    public {#DATASETTYPE} MainDS
    {
        set
        {
            FMainDS = value;
        }
    }

    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
        {#INITUSERCONTROLS}
        {#INITMANUALCODE}
{#IFDEF ACTIONENABLING}
        FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}
      
        if((FMainDS != null)
          && (FMainDS.{#DETAILTABLE} != null))
        {
            DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

{#IFDEF MASTERTABLE OR DETAILTABLE}
            BuildValidationControlsDict();
{#ENDIF MASTERTABLE OR DETAILTABLE}

            ShowData();
        }
    }
    
    {#EVENTHANDLERSIMPLEMENTATION}

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

            int newRowIndex = FMainDS.{#DETAILTABLE}.Rows.Count - 1;
            SelectDetailRowByDataTableIndex(newRowIndex);
            InvokeFocusedRowChanged(grdDetails.SelectedRowIndex());

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
            
            foreach ({#DETAILTABLE}Row myRow in FMainDS.{#DETAILTABLE}.Rows)
            {
                bool found = true;
                foreach (DataColumn myColumn in FMainDS.{#DETAILTABLE}.PrimaryKey)
                {
                    if (myRow.RowState != DataRowState.Deleted)
                    {
                        string value1 = myRow[myColumn].ToString();
                        string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[gridRowIndex - 1][myColumn.Ordinal].ToString();
                        if (value1 != value2)
                        {
                            found = false;
                        }
                    }
                    else
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


    /// make sure that the primary key cannot be edited anymore
    public void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
        {#PRIMARYKEYCONTROLSREADONLY}
    }
    private bool pnlDetailsProtected = false;
    /// <summary>
    /// protect the pnlDetail (only for this tab)
    /// </summary>
    public bool PnlDetailsProtected
    {
        get { return pnlDetailsProtected; }
        set { pnlDetailsProtected = value; }
    }
    
    private void ShowData()
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        pnlDetails.Enabled = false;
        {#SHOWDATA}
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
                grdDetails.SelectRowInGrid(1, TSgrdDataGrid.TInvokeGridFocusEventEnum.NoFocusEvent);
                InvokeFocusedRowChanged(1);
                pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode && !pnlDetailsProtected;
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
{#IFDEF SHOWDETAILS}
    private void ShowDetails({#DETAILTABLETYPE}Row ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        grdDetails.Selection.FocusRowLeaving -= new SourceGrid.RowCancelEventHandler(FocusRowLeaving);

        if (ARow == null)
        {
            pnlDetails.Enabled = false;
            {#CLEARDETAILS}
        }
        else
        {
            FPreviouslySelectedDetailRow = ARow;
            pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode && !pnlDetailsProtected;
            {#SHOWDETAILS}
        }
        FPetraUtilsObject.EnableDataChangedEvent();
        grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
    }

    private {#DETAILTABLETYPE}Row FPreviouslySelectedDetailRow = null;

    private bool FInitialFocusEventCompleted = false;
    private bool FNewFocusEvent = false;
    private bool FGridFilterChanged = false;
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
            // check also if it is a filter change
            if((e.Row != FCurrentRow && FDetailGridRowsChangedState == 0)
              || FGridFilterChanged)
            {
                FGridFilterChanged = false;
{#IFDEF SAVEDETAILS}
                // Transfer data from Controls into the DataTable
                if (FPreviouslySelectedDetailRow != null)
                {
                    GetDetailsFromControls(FPreviouslySelectedDetailRow);
                }
{#ENDIF SAVEDETAILS}
    
                // Display the details of the currently selected Row
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                //pnlDetails.Enabled = true;
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
                    //pnlDetails.Enabled = true;
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

    /// get the data from the controls and store in the currently selected detail row
    public void GetDataFromControls({#MASTERTABLETYPE}Row ARow, Control AControl=null)
    {
{#IFDEF SAVEDATA}
        {#SAVEDATA}
{#ENDIF SAVEDATA}
    }
{#ENDIF MASTERTABLE}
{#IFNDEF MASTERTABLE}

    /// get the data from the controls and store in the currently selected detail row
    public void GetDataFromControls()
    {
{#IFDEF SAVEDETAILS}
        GetDetailsFromControls(FPreviouslySelectedDetailRow);
{#ENDIF SAVEDETAILS}
    }
{#ENDIFN MASTERTABLE}
    
{#IFDEF SAVEDETAILS}

    private void GetDetailsFromControls({#DETAILTABLETYPE}Row ARow, bool AIsNewRow = false, Control AControl=null)
    {
        if (ARow != null && !pnlDetailsProtected && !grdDetails.Sorting)
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
    /// <param name="ADontRecordNewDataValidationRun">Set to false if no new DataValidationRun should be recorded. 
    /// Should be set to true only if called from within this very UserControl to ensure that an external call to the 
    /// UserControl's ValidateAllData Method doesn't change a recorded DataValidationRun that was set from the 
    /// Form/UserControl that embeds this UserControl! (Default=true).</param>    
    /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
    public bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors, Control AValidateSpecificControl = null, bool ADontRecordNewDataValidationRun = true)
    {
        bool ReturnValue = false;
        Control ControlToValidate = null;

        if (!ADontRecordNewDataValidationRun)
        {
            // Record a new Data Validation Run. (All TVerificationResults/TScreenVerificationResults that are created during this 'run' are associated with this 'run' through that.)
            FPetraUtilsObject.VerificationResultCollection.RecordNewDataValidationRun();
        }
        
{#IFDEF SHOWDETAILS}
        if (FPreviouslySelectedDetailRow != null)
        {
{#ENDIF SHOWDETAILS}
        if (AValidateSpecificControl != null) 
        {
            ControlToValidate = AValidateSpecificControl;
        }
        else
        {
            ControlToValidate = this.ActiveControl;
        }

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
                // Only process the Data Validations here if ControlToValidate is not null.
                // It can be null if this.ActiveControl yields null - this would happen if no Control
                // on this UserControl has got the Focus.
                if (ControlToValidate != null) 
                {
                    if(ControlToValidate.FindUserControlOrForm(true) == this)
                    {
{#IFDEF SHOWDETAILS}
                        ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                            this.GetType(), ARecordChangeVerification ? ControlToValidate.FindUserControlOrForm(true).GetType() : null);
{#ENDIF SHOWDETAILS}
{#IFNDEF SHOWDETAILS}
                        ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(false, FPetraUtilsObject.VerificationResultCollection,
                            this.GetType(), ControlToValidate.FindUserControlOrForm(true).GetType());
{#ENDIFN SHOWDETAILS}
                    }
                    else
                    {
                        ReturnValue = true;
                    }
                }
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

#region Data Validation
    
    private void ControlValidatedHandler(object sender, EventArgs e)
    {
        int dataRowIndex = GetDetailGridRowDataTableIndex();
        
        TScreenVerificationResult SingleVerificationResult;
        
        ValidateAllData(true, false, (Control)sender, false);
        
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

        ResetGridFocus(dataRowIndex);
        
    }
{#IFDEF MASTERTABLE}
    private void ValidateData({#MASTERTABLE}Row ARow)
    {
        TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

        {#MASTERTABLE}Validation.Validate(this, ARow, ref VerificationResultCollection,
            FValidationControlsDict);
    }
{#ENDIF MASTERTABLE}
{#IFDEF DETAILTABLE}
    private void ValidateDataDetails({#DETAILTABLE}Row ARow)
    {
        TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

        {#DETAILTABLE}Validation.Validate(this, ARow, ref VerificationResultCollection,
            FValidationControlsDict);
    }
{#ENDIF DETAILTABLE}
{#IFDEF MASTERTABLE OR DETAILTABLE}

    private void BuildValidationControlsDict()
    {
        if (FMainDS != null)
        {
{#IFDEF ADDCONTROLTOVALIDATIONCONTROLSDICT}
            {#ADDCONTROLTOVALIDATIONCONTROLSDICT}
{#ENDIF ADDCONTROLTOVALIDATIONCONTROLSDICT}
        }
    }
{#ENDIF MASTERTABLE OR DETAILTABLE}    

#endregion
  }
}

{#INCLUDE copyvalues.cs}
{#INCLUDE validationcontrolsdict.cs}