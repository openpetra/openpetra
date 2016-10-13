// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template controlMaintainTable
//
// DO NOT edit manually, DO NOT edit with the designer
//
{#GPLFILEHEADER}
#region Usings
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using System.Resources;
using System.Collections.Generic;
using System.Collections.Specialized;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;
using GNU.Gettext;
using SourceGrid;
{#IFDEF SHAREDVALIDATIONNAMESPACEMODULE}
using {#SHAREDVALIDATIONNAMESPACEMODULE};
{#ENDIF SHAREDVALIDATIONNAMESPACEMODULE}
{#USINGNAMESPACES}
#endregion

namespace {#NAMESPACE}
{

  /// auto generated user control
  public partial class {#CLASSNAME}: System.Windows.Forms.UserControl
                                     , {#INTERFACENAME}
{#IFDEF SHOWDETAILS}       
                                     , IDeleteGridRows
{#ENDIF SHOWDETAILS}
{#IFDEF FILTERANDFIND}
                                     , IFilterAndFind
{#ENDIF FILTERANDFIND}
{#IFDEF BUTTONPANEL}
                                     , IButtonPanel
{#ENDIF BUTTONPANEL}
  {
#region Declarations

    private {#UTILOBJECTCLASS} FPetraUtilsObject;
  
    /// <summary>
    /// Dictionary that contains Controls on whose data Data Validation should be run.
    /// </summary>
    private TValidationControlsDict FValidationControlsDict = new TValidationControlsDict();
  
    private {#DATASETTYPE} FMainDS;
{#IFDEF SHOWDETAILS}       
    private DataColumn FPrimaryKeyColumn = null;
    private Control FPrimaryKeyControl = null;
    private Label FPrimaryKeyLabel = null;
    private string FDefaultDuplicateRecordHint = String.Empty;
    private bool FIgnoreFocusRowLeaving = false;
    private bool FShowDetailsInProcess = false;
{#ENDIF SHOWDETAILS}
#endregion

#region Constructor and Initialisation

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

    /// <summary>
    /// Can be used by the Form/Control which contains this UserControl to
    /// suppress the Change Detection (using FPetraUtilsObject.SuppressChangeDetection = false).
    /// Raise this Event to tell the Form/Control which contains this UserControl to do that.
    /// </summary>
    public event System.EventHandler DataLoadingStarted;

    /// <summary>
    /// Can be used by the Form/Control which contains this UserControl to
    /// activate the Change Detection (using FPetraUtilsObject.SuppressChangeDetection = true).
    /// Raise this Event to tell the Form/Control which contains this UserControl to do that.
    /// </summary>
    public event System.EventHandler DataLoadingFinished;

    /// <summary>
    /// Raises the DataLoadingStarted Event if it is subscribed to.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void OnDataLoadingStarted(object sender, EventArgs e)
    {
        if (DataLoadingStarted != null)
        {
            DataLoadingStarted(sender, e);
        }
    }

    /// <summary>
    /// Raises the DataLoadingFinished Event if it is subscribed to.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void OnDataLoadingFinished(object sender, EventArgs e)
    {
        if (DataLoadingFinished != null)
        {
            DataLoadingFinished(sender, e);
        }
    }

    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
        {#INITUSERCONTROLS}
{#IFDEF ACTIONENABLING}
        FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}
        {#INITMANUALCODE}
{#IFDEF SHOWDETAILS}       
        grdDetails.Enter += new EventHandler(grdDetails_Enter);
        grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(grdDetails_FocusRowLeaving);
        grdDetails.Selection.SelectionChanged += new RangeRegionChangedEventHandler(grdDetails_RowSelected);
{#ENDIF SHOWDETAILS}
        {#GRIDMULTISELECTION}
        pnlDetails.Enabled = false;
      
        if((FMainDS != null)
          && (FMainDS.{#DETAILTABLE} != null))
        {
            DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            ShowData();
        }

{#IFDEF MASTERTABLE OR DETAILTABLE}
        BuildValidationControlsDict();
{#ENDIF MASTERTABLE OR DETAILTABLE}
{#IFDEF SHOWDETAILS}       
        SetPrimaryKeyControl();
{#ENDIF SHOWDETAILS}
{#IFDEF BUTTONPANEL}
        FinishButtonPanelSetup();
{#ENDIF BUTTONPANEL}
{#IFDEF FILTERANDFIND}
        FFilterAndFindObject = new TFilterAndFindPanel(this, FPetraUtilsObject, grdDetails, this, pnlFilterAndFind, chkToggleFilter, lblRecordCounter);
        FFilterAndFindObject.SetupFilterAndFindControls();
        {#FILTERBUTTON}
{#ENDIF FILTERANDFIND}
        SelectRowInGrid(1);
    }
#endregion

#region Event Handlers Implementation
    
    {#EVENTHANDLERSIMPLEMENTATION}
#endregion

#region Create New Record

    /// <summary>
    /// This automatically generated method creates a new record of {#DETAILTABLE}, highlights it in the grid
    /// and displays it on the edit screen.  We create the table locally, no dataset
    /// </summary>
    /// <returns>True if the existing Details data was validated successfully and the new row was added.</returns>
    public bool CreateNew{#DETAILTABLE}()
    {
        if(ValidateAllData(true, TErrorProcessingMode.Epm_IgnoreNonCritical))
        {
            FIgnoreFocusRowLeaving = false;

            if (FPetraUtilsObject.VerificationResultCollection.HasOnlyNonCriticalErrors)
            {
                // Some non-critical warnings exist
                if (TDataValidation.ProcessAnyDataValidationWarnings(FPetraUtilsObject.VerificationResultCollection,
                    MCommonResourcestrings.StrCreateNewRowAnywayQuestion, this.GetType()) == false)
                {
                    // Client cancelled
                    return false;
                }

                // Client wants to continue
                // We can ignore FocusRowLeaving because we have just displayed the message as a Yes/No dialog
                FIgnoreFocusRowLeaving = true;
            }
            
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
{#IFDEF FILTERANDFIND}
            
            if (!SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1))
            {
                if (!FFilterAndFindObject.IsActiveFilterEqualToBase)
                {
                    MessageBox.Show(
                        MCommonResourcestrings.StrNewRecordIsFiltered,
                        MCommonResourcestrings.StrAddNewRecordTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FFilterAndFindObject.FilterPanelControls.ClearAllDiscretionaryFilters();

                    if (FFilterAndFindObject.FilterFindPanel.ShowApplyFilterButton != TUcoFilterAndFind.FilterContext.None)
                    {
                        FFilterAndFindObject.ApplyFilter();
                    }

                    SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);
                }
            }
{#ENDIF FILTERANDFIND}
{#IFNDEF FILTERANDFIND}
            SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);
{#ENDIFN FILTERANDFIND}
            
            Control[] pnl = this.Controls.Find("pnlDetails", true);
            if (pnl.Length > 0)
            {
                // Build up a list of controls on this panel that is sorted in true nested tab order
                SortedList<string, Control> controlsSortedByTabOrder = new SortedList<string, Control>();
                FPetraUtilsObject.GetSortedControlList(ref controlsSortedByTabOrder, pnl[0], String.Empty);

                if (controlsSortedByTabOrder.Count > 0)
                {
                    foreach (Control c in controlsSortedByTabOrder.Values)
                    {
                        if (c is TextBox && c.Name.Contains("Desc"))
                        {
                            // Set the default text for the first TextBox whose name contains 'Desc'
                            c.Text = MCommonResourcestrings.StrPleaseEnterDescription;
                            ValidateAllData(true, TErrorProcessingMode.Epm_None);
                            break;
                        }
                    }

                    // Focus the first control in our sorted list
                    controlsSortedByTabOrder.Values[0].Focus();
                }
            }
            
{#IFDEF BUTTONPANEL}
            UpdateRecordNumberDisplay();

{#ENDIF BUTTONPANEL}
            FIgnoreFocusRowLeaving = false;
            return true;
        }
        else
        {
            return false;
        }
    }

#endregion

#region Row Selection and Discovery

    /// <summary>
    /// Selects the specified grid row and shows the details for the row in the details panel.
    /// The call still works even if the grid is empty (in which case no row is highlighted).
    /// Grid rows holding data are numbered 1..DataRowCount.
    /// If the specified grid row is less than 1, the first row is highlighted.
    /// If the specified grid row is greater than DataRowCount, the last row is highlighted.
    /// The details panel is disabled if the grid is empty or in Detail Protect Mode
    ///    otherwise the details are shown for the row that has been highlighted.
    /// </summary>
    /// <param name="ARowIndex">The row index to select.  Data rows start at 1</param>
    public void SelectRowInGrid(int ARowIndex)
    {
        grdDetails.SelectRowInGrid(ARowIndex, true);
    }

    /// <summary>
    /// Selects a grid row based its index in the data table (often the last, newest, row).
    /// The details panel is automatically updated to show the new details.
    /// If the grid is not displaying the specified data row, the first row will be selected, if it exists.
    /// </summary>
    /// <param name="ARowNumberInTable">Table row number (0-based)</param>
    /// <returns>True if the record is displayed in the grid, False otherwise</returns>
    private bool SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;
        Int32 RowNumberGrid = DataUtilities.GetDataViewIndexByDataTableIndex(dv, FMainDS.{#DETAILTABLE}, ARowNumberInTable) + 1;

        SelectRowInGrid(RowNumberGrid);

        return RowNumberGrid > 0;
    }
    
    /// <summary>
    /// Finds the grid row in the data table
    /// </summary>
    /// <returns>The data table row index for the data in the current grid row, or -1 if the row was not found</returns>
    private int GetDetailGridRowDataTableIndex()
    {
        Int32 RowNumberInData = -1;

        if (FPrevRowChangedRow > 0 && FPreviouslySelectedDetailRow != null)
        {
            DataRowView drv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView[FPrevRowChangedRow - 1];
            RowNumberInData = DataUtilities.GetDataTableIndexByDataRowView(FMainDS.{#DETAILTABLE}, drv);
        }

        return RowNumberInData;
    }
    
{#IFDEF SHOWDETAILS OR GENERATEGETSELECTEDDETAILROW}

    /// <summary>
    /// Gets the selected Data Row as a {#DETAILTABLETYPE} record from the grid
    /// </summary>
    /// <returns>The selected row - or null if no row is selected</returns>
    public {#DETAILTABLETYPE}Row GetSelectedDetailRow()
    {
        return FPreviouslySelectedDetailRow;
    }

    /// <summary>
    /// Gets the selected grid row as a generic DataRow for use by interfaces
    /// </summary>
    /// <returns>The selected row - or null if no row is selected</returns>
    public DataRow GetSelectedDataRow()
    {
        return FPreviouslySelectedDetailRow;
    }

    /// <summary>
    /// Gets the selected Data Row index in the grid.  The first data row is 1.
    /// </summary>
    /// <returns>The selected row - or -1 if no row is selected</returns>
    public Int32 GetSelectedRowIndex()
    {
        return FPrevRowChangedRow;
    }
{#ENDIF SHOWDETAILS OR GENERATEGETSELECTEDDETAILROW}
#endregion

#region Show and Undo data

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
        {#SHOWDATA}
        if (FMainDS.{#DETAILTABLE} != null)
        {
            DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
{#IFDEF GRIDSORT}
            myDataView.Sort = "{#GRIDSORT}";
{#ENDIF GRIDSORT}
{#IFDEF GRIDFILTER}
            myDataView.RowFilter = {#GRIDFILTER};
{#ENDIF GRIDFILTER}
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            if (myDataView.Count > 0)
            {
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
#endregion
{#IFDEF SHOWDETAILS}

#region Show Details, Grid events and Record Deletion

    #region Show Details
    /// <summary>
    /// Use this override to Show the Details for a specified row in the grid.
    /// This override is safe to use in  manual code because it will update the FPreviouslySelectedDetailRow and FPrevRowChangedRow internal variables.
    /// </summary>
    /// <param name="ARowNumberInGrid">The grid row.  Valid rows start at 1.</param>
    private void ShowDetails(Int32 ARowNumberInGrid)
    {
        FPreviouslySelectedDetailRow = null;
        int GridRowCount = grdDetails.Rows.Count;

        if (ARowNumberInGrid >= GridRowCount)
        {
            ARowNumberInGrid = GridRowCount - 1;
        }

        if ((ARowNumberInGrid < 1) && (GridRowCount > 1))
        {
            ARowNumberInGrid = 1;
        }

        if (ARowNumberInGrid > 0)
        {
            DataRowView rowView = (DataRowView)grdDetails.Rows.IndexToDataSourceRow(ARowNumberInGrid);

            if (rowView != null)
            {
                FPreviouslySelectedDetailRow = ({#DETAILTABLETYPE}Row)(rowView.Row);
            }

            FPrevRowChangedRow = ARowNumberInGrid;
        }
        else
        {
            FPrevRowChangedRow = -1;
        }

        ShowDetails(FPreviouslySelectedDetailRow);
    }

    /// <summary>
    /// This overload shows the details for the currently selected row.
    /// The method still works when the grid is empty and no row can be selected.
    /// This method is safe to use in manual code although it should be rarely necessary because the standard 
    /// code should always be showing the correct details.
    /// The Details panel is disabled when the grid is empty, or when in Detail Protected Mode
    /// The variable FPreviouslySelectedDetailRow is set by this call.
    /// </summary>
    private void ShowDetails()
    {
        ShowDetails(FPrevRowChangedRow);
    }

    /// <summary>
    /// This overload shows the details for the specified row, which can be null.
    /// The Details panel is disabled when the row is Null, or when in Detail Protected Mode
    /// IMPORTANT: Do not call this method from manual code because the internal variables will no longer match.
    /// </summary>
    /// <param name="ARow">The row for which details will be shown</param>
    private void ShowDetails({#DETAILTABLETYPE}Row ARow)
    {
        try
        {
            FShowDetailsInProcess = true;
            FPetraUtilsObject.DisableDataChangedEvent();

            if (ARow == null)
            {
                pnlDetails.Enabled = false;
                {#CLEARDETAILS}
            }
            else
            {
                pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode && !pnlDetailsProtected;
                {#SHOWDETAILS}
            }
        }
        finally
        {
            {#ENABLEDELETEBUTTON}

            {#EXTRADISABLEBUTTONHANDLING}

            FPetraUtilsObject.EnableDataChangedEvent();
            FShowDetailsInProcess = false;
        }
    }

    private bool ShowDetailsInProcess()
    {
        return FShowDetailsInProcess;
    }
{#CANDELETESELECTION}
    #endregion

    #region Grid events

    /// <summary>
    /// A reference to the Typed Data Row object from the grid whose Details are currently displayed.
    /// It is automatically updated when you call ShowDetails()
    /// You can use this variable in your manual code to access individual details, but you should take care
    ///   if you need to assign a different row object to it.  Try, if possible, to use 
    ///   SelectRowInGrid(N, true)
    /// or
    ///   ShowDetails(NewRow)
    /// so that the reference to the row object is updated automatically.
    /// </summary>
    private {#DETAILTABLETYPE}Row FPreviouslySelectedDetailRow = null;
    
    /// <summary>
    /// This variable may become obsolete in future.  It used to hold the most recent row passed as the parameter to the FocusedRowChanged event.
    /// However we no longer use this event.  If you want to know the current row index use GetSelectedRowIndex() instead.
    /// </summary>
    private int FPrevRowChangedRow = -1;

    /// <summary>
    /// Fired when the user tabs to, or clicks in, the grid
    /// </summary>
    private void grdDetails_Enter(object sender, EventArgs e)
    {
        if (FPetraUtilsObject.VerificationResultCollection.Count > 0)
        {
            // No need to show the cell if there are no errors.  This allows the user to have scrolled the view-port away from the selected row and keep it there.
            grdDetails.ShowCell(FPrevRowChangedRow);
        }
    }

    /// <summary>
    /// This is the main event handler for changes in the grid selection
    /// </summary>
    private void grdDetails_RowSelected(object sender, RangeRegionChangedEventArgs e)
    {
        int gridRow = grdDetails.Selection.ActivePosition.Row;

        if (grdDetails.Sorting)
        {
            // No need to ShowDetails - just update our (obsolete) variable
            FPrevRowChangedRow = gridRow;
        }
        else
        {
            if ((gridRow == FPrevRowChangedRow) && grdDetails.IsMouseDown)
            {
                // This deals with the special case where the user edits a detail and then clicks on the same row in the grid
                // If the change(s) were to control(s) that have no validation we will not have had a call to get the details from the controls.
                // So we do that here so that the grid will update and reflect the changes made.
                ValidateAllData(true, TErrorProcessingMode.Epm_None);
            }

            // In all other cases we need to ShowDetails, even if the active row is the same as it was before.  This covers the following cases:
            // - the row has changed
            // - the special case where, on a sorted grid, the row may be the same but contains different details
            //     because this event fires from SelectRowInGrid, even when the selected row hasn't changed but the data has.
            // - the case of SHIFT+click.  In that case the selected row stays the same but if the number of highlighted rows
            //     is different we may need to (for example) alter the enabled state of the delete button.
            ShowDetails(gridRow);
            //Console.WriteLine("{0}: RowSelected: ShowDetails() for row {1}", DateTime.Now.Millisecond, gridRow);
        }
    }

    /// <summary>
    /// FocusedRowLeaving is called when the user (or code) requests a change to the selected row.
    /// </summary>
    private void grdDetails_FocusRowLeaving(object sender, SourceGrid.RowCancelEventArgs e)
    {        
        if (FIgnoreFocusRowLeaving)
        {
            return;
        }

        if (!ValidateAllData(true, TErrorProcessingMode.Epm_All))
        {
            e.Cancel = true;
        }
    }
    #endregion

    #region Delete records
{#DELETERECORD}
    /// <summary>
    /// Standard method to delete the Data Row whose Details are currently displayed.
    /// There is full support for multi-row deletion.
    /// Optional manual code can be included to take action prior, during or after each deletion.
    /// When the row(s) have been deleted the highlighted row index stays the same unless the deleted row was the last one.
    /// The Details for the newly highlighted row are automatically displayed - or not, if the grid has now become empty.
    /// </summary>
    private void Delete{#DETAILTABLE}()
    {
{#IFNDEF BUTTONPANEL}
        TDeleteGridRows.DeleteRows(this, grdDetails, FPetraUtilsObject, null);
{#ENDIFN BUTTONPANEL}
{#IFDEF BUTTONPANEL}
        TDeleteGridRows.DeleteRows(this, grdDetails, FPetraUtilsObject, this);
{#ENDIF BUTTONPANEL}
    }

    #region IDeleteGridRows implementation

    /// <summary>
    /// Perform the reference count
    /// </summary>
    public void GetReferenceCount(DataRow ADataRow, int AMaxReferenceCount, out TVerificationResultCollection AVerificationResults)
    {
{#IFDEF DELETEREFERENCECOUNT}
        {#DELETEREFERENCECOUNT}
{#ENDIF DELETEREFERENCECOUNT}
{#IFNDEF DELETEREFERENCECOUNT}
        AVerificationResults = null;
{#ENDIFN DELETEREFERENCECOUNT}
    }

    /// <summary>
    /// Specifies the default deletion question
    /// </summary>
    public string GetDefaultDeletionQuestion()
    {
        string DeletionQuestion = MCommonResourcestrings.StrDefaultDeletionQuestion;
        if ((FPrimaryKeyControl != null) && (FPrimaryKeyLabel != null))
        {
            DeletionQuestion += String.Format("{0}{0}({1} {2})",
                Environment.NewLine,
                FPrimaryKeyLabel.Text.Replace("&", ""),
                TControlExtensions.GetDisplayTextForControl(FPrimaryKeyControl));
        }
        return DeletionQuestion;
    }

    /// <summary>
    /// Handler for optional Pre-Delete manual code
    /// </summary>
    public void HandlePreDelete(DataRow ARowToDelete, ref bool AAllowDeletion, ref string ADeletionQuestion)
    {
{#IFDEF HASPREDELETEMANUAL}
        AAllowDeletion = PreDeleteManual(({#DETAILTABLETYPE}Row)ARowToDelete, ref ADeletionQuestion);
{#ENDIF HASPREDELETEMANUAL}
    }

    /// <summary>
    /// Handler for optional manual deletion code.  Return True if manual code handles deletion or false to use the default processing
    /// </summary>
    public bool HandleDeleteRow(DataRow ARowToDelete, ref bool ADeletionPerformed, ref string ACompletionMessage)
    {
{#IFDEF HASDELETEROWMANUAL}
        ADeletionPerformed = DeleteRowManual(({#DETAILTABLETYPE}Row)ARowToDelete, ref ACompletionMessage);
        return true;
{#ENDIF HASDELETEROWMANUAL}
{#IFNDEF HASDELETEROWMANUAL}
        return false;
{#ENDIFN HASDELETEROWMANUAL}
    }

    /// <summary>
    /// Handler for optional Post-Delete manual code.  Return True if manual code handles post-deletion or false to use the default processing
    /// </summary>
    public bool HandlePostDelete(DataRow ARowToDelete, bool AAllowDeletion, bool ADeletionPerformed, string ACompletionMessage)
    {
{#IFDEF HASPOSTDELETEMANUAL}
        PostDeleteManual(({#DETAILTABLETYPE}Row)ARowToDelete, AAllowDeletion, ADeletionPerformed, ACompletionMessage);
        return true;
{#ENDIF HASPOSTDELETEMANUAL}
{#IFNDEF HASPOSTDELETEMANUAL}
        return false;
{#ENDIFN HASPOSTDELETEMANUAL}
    }

    /// <summary>
    /// Return False if special reasons exist to disallow deletion of the specified row (eg the row contains 'system' data)
    /// </summary>
    public bool IsRowDeletable(DataRow ARowToDelete)
    {
        {#CANDELETEROW}
    }
    #endregion

#endregion
#endregion
{#ENDIF SHOWDETAILS}

#region Get Data

{#IFDEF MASTERTABLE}

    /// get the data from the controls and store in the currently selected detail row
    /// This method may throw an exception at ARow.EndEdit()
    public void GetDataFromControls({#MASTERTABLETYPE}Row ARow, Control AControl=null)
    {
{#IFDEF SAVEDATA}
        if (ARow == null) return;

        object[] beforeEdit = ARow.ItemArray;
        ARow.BeginEdit();
        {#SAVEDATA}
        {#SAVEDATAEXTRA}
        if (Ict.Common.Data.DataUtilities.HaveDataRowsIdenticalValues(beforeEdit, ARow.ItemArray))
        {
            ARow.CancelEdit();
        }
        else
        {
            ARow.EndEdit();
        }
{#ENDIF SAVEDATA}
    }
{#ENDIF MASTERTABLE}
{#IFNDEF MASTERTABLE}

    /// get the data from the controls and store in the currently selected detail row
    public void GetDataFromControls()
    {
{#IFDEF SAVEDETAILS}
        ValidateAllData(false, TErrorProcessingMode.Epm_None);
{#ENDIF SAVEDETAILS}
    }
{#ENDIFN MASTERTABLE}
    
{#IFDEF SAVEDETAILS}

    /// <summary>
    /// Do not call this method in your manual code.
    /// This is a method that is private to the generated code and is part of the Validation process.
    /// If you need to update the controls data into the Data Row object, you must use ValidateAllData and be prepared 
    ///   to handle the consequences of a failed validation.
    /// </summary>
    /// <param name="ARow">Do not use</param>
    /// <param name="AIsNewRow">Do not use</param>
    /// <param name="AControl">Do not use</param>
    private void GetDetailsFromControls({#DETAILTABLETYPE}Row ARow, bool AIsNewRow = false, Control AControl=null)
    {
        if ((ARow != null) &&
            (ARow.RowState != DataRowState.Deleted) &&
            (ARow.RowState != DataRowState.Detached) &&
            !pnlDetailsProtected && !grdDetails.Sorting)
        {
            if (AIsNewRow)
            {
                {#SAVEDETAILS}
                {#SAVEDETAILSEXTRA}
            }
            else
            {
                object[] beforeEdit = ARow.ItemArray;
                ARow.BeginEdit();
                {#SAVEDETAILS}
                {#SAVEDETAILSEXTRA}
                if (Ict.Common.Data.DataUtilities.HaveDataRowsIdenticalValues(beforeEdit, ARow.ItemArray))
                {
                    ARow.CancelEdit();
                }
                else
                {
                    ARow.EndEdit();
                }
            }
        }
    }
{#IFDEF GENERATECONTROLUPDATEDATAHANDLER}

    private void ControlUpdateDataHandler(object sender, EventArgs e)
    {
        // This method should not normally be associated with a control that requires validation (because no validation takes place)
        // GetDetailsFromControls can return an exception if the control is associated with a primary key, so we use a try/catch just in case
        try
        {
            GetDetailsFromControls(FPreviouslySelectedDetailRow, false, (Control)sender);
        }
        catch (ConstraintException)
        {
        }
    }
{#ENDIF GENERATECONTROLUPDATEDATAHANDLER}
{#ENDIF SAVEDETAILS}
#endregion
{#IFDEF BUTTONPANEL}

#region Button Panel

    ///<summary>
    /// Finish the set up of the Button Panel.
    /// </summary>
    private void FinishButtonPanelSetup()
    {
        // Further set up certain Controls Properties that can't be set directly in the WinForms Generator...
        lblRecordCounter.AutoSize = true;
        lblRecordCounter.Padding = new Padding(4, 3, 0, 0);
        lblRecordCounter.ForeColor = System.Drawing.Color.SlateGray;

        pnlButtonsRecordCounter.AutoSize = true;
        
        UpdateRecordNumberDisplay();
    }
    
    ///<summary>
    /// Update the text in the button panel indicating details of the record count
    /// </summary>
    public void UpdateRecordNumberDisplay()
    {
        if (grdDetails.DataSource != null) 
        {
            int RecordCount = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).Count;
            lblRecordCounter.Text = String.Format(
                Catalog.GetPluralString(MCommonResourcestrings.StrSingularRecordCount, MCommonResourcestrings.StrPluralRecordCount, RecordCount, true),
                RecordCount);
        }                
    }
#endregion
{#ENDIF BUTTONPANEL}
{#IFDEF FILTERANDFIND}

#region Filter and Find
    {#FILTERANDFINDMETHODS}
#endregion
{#ENDIF FILTERANDFIND}    

#region Data Validation

    /// <summary>
    /// Performs data validation.
    /// </summary>
    /// <remarks>May be called by the Form that hosts this UserControl to invoke the data validation of
    /// the UserControl.</remarks>    
    /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing 
    /// to another record, otherwise set it to false.</param>
    /// <param name="ADataValidationProcessingMode">Set to <see cref="TErrorProcessingMode.Epm_None"/> if no data validation errors should be shown to the user,
    /// otherwise set it to one of <see cref="TErrorProcessingMode.Epm_IgnoreNonCritical"/> or <see cref="TErrorProcessingMode.Epm_All"/>.</param>
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
    public bool ValidateAllData(bool ARecordChangeVerification, TErrorProcessingMode ADataValidationProcessingMode, Control AValidateSpecificControl = null, bool ADontRecordNewDataValidationRun = true)
    {
        bool ReturnValue = false;
        Control ControlToValidate = null;

        if (!ADontRecordNewDataValidationRun)
        {
            // Record a new Data Validation Run. (All TVerificationResults/TScreenVerificationResults that are created during this 'run' are associated with this 'run' through that.)
            FPetraUtilsObject.VerificationResultCollection.RecordNewDataValidationRun();
        }
        
{#IFNDEF SHOWDETAILS}
// :CMT:ControlToValidate
        if (AValidateSpecificControl != null) 
        {
            ControlToValidate = AValidateSpecificControl;
        }
        else
        {
            ControlToValidate = this.ActiveControl;
        }
{#IFDEF MASTERTABLE}
// :CMT:GetDataFromControls
        GetDataFromControls(FMainDS.{#MASTERTABLE}[0]);
        ValidateData(FMainDS.{#MASTERTABLE}[0]);
{#IFDEF VALIDATEDATAMANUAL}
// :CMT:ValidateDataManual
        ValidateDataManual(FMainDS.{#MASTERTABLE}[0]);
{#ENDIF VALIDATEDATAMANUAL}
{#ENDIF MASTERTABLE}        
{#ENDIFN SHOWDETAILS}
{#IFDEF SHOWDETAILS}
        if ((FPreviouslySelectedDetailRow != null) 
            && (FPreviouslySelectedDetailRow.RowState != DataRowState.Deleted)
            && (FPreviouslySelectedDetailRow.RowState != DataRowState.Detached)
            )
        {
// :CMT:ControlToValidate
            if (AValidateSpecificControl != null) 
            {
                ControlToValidate = AValidateSpecificControl;
            }
            else
            {
                ControlToValidate = this.ActiveControl;
            }
            
            bool bGotConstraintException = false;
            int prevRowBeforeValidation = FPrevRowChangedRow;
// :CMT:GetDetailsFromControls
            try
            {
                GetDetailsFromControls(FPreviouslySelectedDetailRow);
                ValidateDataDetails(FPreviouslySelectedDetailRow);
{#IFDEF VALIDATEDATADETAILSMANUAL}
// :CMT:ValidateDataDetailsManual
                ValidateDataDetailsManual(FPreviouslySelectedDetailRow);
{#ENDIF VALIDATEDATADETAILSMANUAL}
            }
            catch (ConstraintException)
            {
                bGotConstraintException = true;
            }

            // Duplicate record validation
            if (FPrimaryKeyColumn == null)
            {
                // If controls have been named according to the column names, it should be impossible to get a constraint exception 
                //    without us knowing which is the 'prime' primary key column and control
                // But this is our ultimate fallback position.  This creates an exception message that simply lists all the primary key fields in a friendly format
                FPetraUtilsObject.VerificationResultCollection.AddOrRemove(
                    bGotConstraintException ? new TScreenVerificationResult(this, null,
                    String.Format(MCommonResourcestrings.StrDuplicateRecordNotAllowed, FDefaultDuplicateRecordHint),
                    CommonErrorCodes.ERR_DUPLICATE_RECORD, null, TResultSeverity.Resv_Critical) : null, null);
            }
            else
            {
                TControlExtensions.ValidateNonDuplicateRecord(this, bGotConstraintException, FPetraUtilsObject.VerificationResultCollection, 
                            FPrimaryKeyColumn, FPrimaryKeyControl, FMainDS.{#DETAILTABLE}.PrimaryKey);
            }
            
            // Validation might have moved the row, so we need to locate it again
            // If it has moved we will call the special grid-sorting method SelectRowAfterSort to highlight the new row.
            // This will give rise to a Selection_SelectionChanged event with a new ActivePosition but the grdDetails.Sorting property will be True
            // Furthermore with Filter/Find we need to be sure that we have not lost the row altogether by using a different row filter
            //  and we also need to protect against recursively call SelectRowAfterSort
            int newRowAfterValidation = grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow, prevRowBeforeValidation - 1) + 1;
            if (newRowAfterValidation == 0)
            {
{#IFDEF FILTERANDFIND}
                // The row is no longer in the view - probably because the new data values are being filtered out
                if (!FFilterAndFindObject.IsActiveFilterEqualToBase || !FFilterAndFindObject.IsBaseFilterShowingAllRecords)
                {
                    // Remember the control with the focus
                    Control c = FPetraUtilsObject.GetFocusedControl(pnlDetails);

                    // Clear the filters without selecting a new row
                    FFilterAndFindObject.ClearingDiscretionaryFilters = true;
                    FFilterAndFindObject.FilterPanelControls.ClearAllDiscretionaryFilters();

                    if (FFilterAndFindObject.FilterFindPanel.ShowApplyFilterButton != TUcoFilterAndFind.FilterContext.None)
                    {
                        FFilterAndFindObject.ApplyFilter();
                    }

                    FFilterAndFindObject.ClearingDiscretionaryFilters = false;

                    // Now find the row again - this time we should succeed.  If not some other piece of code will have to do the selection
                    newRowAfterValidation = grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow, prevRowBeforeValidation - 1) + 1;

                    if (newRowAfterValidation > 0)
                    {
                        // Select the row
                        grdDetails.SelectRowAfterSort(newRowAfterValidation);
                    
                        // Put the focus back again on the editable control
                        if (c != null)
                        {
                            c.Focus();
                        }
                        
                        // Tell the user what we have done
                        MessageBox.Show(MCommonResourcestrings.StrEditedRecordIsFiltered, MCommonResourcestrings.StrEditRecordTitle, 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
{#ENDIF FILTERANDFIND}
{#IFNDEF FILTERANDFIND}
                // There is no Filter/Find on this screen so we should never take this code path
{#ENDIFN FILTERANDFIND}
            }
            else
            {
                if ((newRowAfterValidation != prevRowBeforeValidation) && !grdDetails.Sorting)
                {
                    grdDetails.SelectRowAfterSort(newRowAfterValidation);
                    //Console.WriteLine("{0}:    Validation: validated row moved to {1}.", DateTime.Now.Millisecond, newRowAfterValidation);
                }
            }
{#ENDIF SHOWDETAILS}
{#IFDEF PERFORMUSERCONTROLVALIDATION}

// :CMT:ucValidation
            // Perform validation in UserControls, too
            {#USERCONTROLVALIDATION}
{#ENDIF PERFORMUSERCONTROLVALIDATION}

            if (ADataValidationProcessingMode != TErrorProcessingMode.Epm_None)
            {
                // Only process the Data Validations here if ControlToValidate is not null.
                // It can be null if this.ActiveControl yields null - this would happen if no Control
                // on this UserControl has got the Focus.
                if (ControlToValidate != null) 
                {
                    if(ControlToValidate.FindUserControlOrForm(true) == this)
                    {
                        bool ignoreWarnings = (ADataValidationProcessingMode == TErrorProcessingMode.Epm_IgnoreNonCritical) &&
                            !FPetraUtilsObject.VerificationResultCollection.HasCriticalErrors;
{#IFDEF SHOWDETAILS}
                        ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                            this.GetType(), ARecordChangeVerification ? ControlToValidate.FindUserControlOrForm(true).GetType() : null, ignoreWarnings);
{#ENDIF SHOWDETAILS}
{#IFNDEF SHOWDETAILS}
                        ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(false, FPetraUtilsObject.VerificationResultCollection,
                            this.GetType(), ControlToValidate.FindUserControlOrForm(true).GetType(), ignoreWarnings);
{#ENDIFN SHOWDETAILS}
                    }
                    else
                    {
                        ReturnValue = true;
                    }
                }
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
#endregion

#region Implement interface functions
    /// auto generated
    public void RunOnceOnActivation()
    {
        {#RUNONCEINTERFACEIMPLEMENTATION}
        {#FINALRUNONCEONACTIVATIONACTIONMANUAL}
    }

    /// auto generated
    public void RunOnceOnParentActivation()
    {
{#IFDEF FILTERANDFIND}
        if (FFilterAndFindObject.FilterAndFindParameters.FindAndFilterInitiallyExpanded)
        {
            FFilterAndFindObject.FilterPanelControls.InitialiseComboBoxes();
            FFilterAndFindObject.FindPanelControls.InitialiseComboBoxes();
        }
{#ENDIF FILTERANDFIND}    
        {#GRIDHEADERTOOLTIP}
        {#RUNONCEONPARENTACTIVATIONMANUAL}    
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
{#IFDEF FILTERANDFIND}
        if (e.ActionName == "cndFindFilterAvailable")
        {
            chkToggleFilter.Enabled = e.Enabled;
        }
{#ENDIF FILTERANDFIND}        
    }

    {#ACTIONHANDLERS}

#endregion
{#ENDIF ACTIONENABLING}

#region Keyboard handler

    /// Our main keyboard handler
    public bool ProcessParentCmdKey(ref Message msg, Keys keyData)
    {
        {#PREPROCESSCMDKEY}
{#IFDEF FILTERANDFIND}
        {#PROCESSCMDKEYCTRLF}
        {#PROCESSCMDKEYCTRLR}
{#ENDIF FILTERANDFIND}
        {#PROCESSCMDKEY}    
        {#PROCESSCMDKEYMANUAL}    

        return false;
    }

    private void FocusFirstEditableControl()
    {
        {#FOCUSFIRSTEDITABLEDETAILSPANELCONTROL}
    }

#endregion

#region Data Validation Control Handlers
    
    private void ControlValidatedHandler(object sender, EventArgs e)
    {
        TScreenVerificationResult SingleVerificationResult;
        
        ValidateAllData(true, TErrorProcessingMode.Epm_None, (Control)sender, false);
        
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
{#IFDEF FILTERANDFIND}
        else if (FFilterAndFindObject.FailedValidation_CtrlChangeEventArgsInfo != null)
        {
            // The validation is all ok...  But we do have an outstanding filter update that we did not show due to previous invalid data
            //  So we can call that now and update the display.
            FFilterAndFindObject.FucoFilterAndFind_ArgumentCtrlValueChanged(FFilterAndFindObject.FailedValidation_CtrlChangeEventArgsInfo.Sender,
                (TUcoFilterAndFind.TContextEventExtControlValueArgs)FFilterAndFindObject.FailedValidation_CtrlChangeEventArgsInfo.EventArgs);
            
            // Reset our cached change event
            FFilterAndFindObject.FailedValidation_CtrlChangeEventArgsInfo = null;
        }
{#ENDIF FILTERANDFIND}    
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
    private void ValidateDataDetails({#DETAILTABLETYPE}Row ARow)
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
{#IFDEF SHOWDETAILS}       

    private void SetPrimaryKeyControl()
    {
        // Make a default hint string from all the primary keys
        // and initialise the 'prime' primary key control on pnlDetails.
        // This is the last control in the tab order that matches a key
        int lastTabIndex = -1;
        DataRow row = (new {#DETAILTABLE}Table()).NewRow();
        for (int i = 0; i < row.Table.PrimaryKey.Length; i++)
        {
            DataColumn column = row.Table.PrimaryKey[i];
            if (FDefaultDuplicateRecordHint.Length > 0) FDefaultDuplicateRecordHint += ", ";
            FDefaultDuplicateRecordHint += TControlExtensions.DataColumnNameToFriendlyName(column, true);
            
            Label label;
            Control control;
            if (TControlExtensions.GetControlsForPrimaryKey(column, this, out label, out control))
            {
                if (control.TabIndex > lastTabIndex)
                {
                    FPrimaryKeyColumn = column;
                    FPrimaryKeyControl = control;
                    FPrimaryKeyLabel = label;
                    lastTabIndex = control.TabIndex;
                }
            }
        }
    }
{#ENDIF SHOWDETAILS}

#endregion
  }
}

{#INCLUDE copyvalues.cs}
{#INCLUDE validationcontrolsdict.cs}
{#INCLUDE findandfilter.cs}

{##SNIPDELETEREFERENCECOUNT}
bool previousCursorIsDefault = FPetraUtilsObject.GetForm().Cursor.Equals(Cursors.Default);
FPetraUtilsObject.ShowWaitCursor();

TRemote.{#CONNECTORNAMESPACE}.ReferenceCount.WebConnectors.GetNonCacheableRecordReferenceCount(
    FMainDS.{#NONCACHEABLETABLENAME},
    DataUtilities.GetPKValuesFromDataRow(ADataRow),
    AMaxReferenceCount,
    out AVerificationResults);

if (previousCursorIsDefault)
{
    FPetraUtilsObject.ShowDefaultCursor();
}

{##SNIPCANDELETEROW}
return (({#DETAILTABLE}Row)ARowToDelete).{#DELETEABLEFLAG};

{##SNIPCANDELETESELECTION}

    /// <summary>
    /// Returns true if one of the selected rows can be deleted.
    /// </summary>
    private bool CanDeleteSelection()
    {
        // This table has a {#DELETEABLEFLAG} column
        DataRowView[] selectedRows = grdDetails.SelectedDataRowsAsDataRowView;

        foreach (DataRowView drv in selectedRows)
        {
            if ((({#DETAILTABLE}Row)drv.Row).{#DELETEABLEFLAG})
            {
                return true;
            }
        }

        return false;
    }

{#INCLUDE security.cs}