// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template controlMaintainCachableTable
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
using SourceGrid;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Generic;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
{#IFDEF SHAREDVALIDATIONNAMESPACEMODULE}
using {#SHAREDVALIDATIONNAMESPACEMODULE};
{#ENDIF SHAREDVALIDATIONNAMESPACEMODULE}
{#USINGNAMESPACES}
#endregion

namespace {#NAMESPACE}
{

  /// auto generated: {#FORMTITLE}
  public partial class {#CLASSNAME}: System.Windows.Forms.UserControl, {#INTERFACENAME}
  {
#region Declarations

    private {#UTILOBJECTCLASS} FPetraUtilsObject;

    /// <summary>
    /// Dictionary that contains Controls on whose data Data Validation should be run.
    /// </summary>
    private TValidationControlsDict FValidationControlsDict = new TValidationControlsDict();
    
{#FILTERVAR}
{#IFDEF DATASETTYPE}
    private {#DATASETTYPE} FMainDS;
{#ENDIF DATASETTYPE}
{#IFNDEF DATASETTYPE}
    {#INLINETYPEDDATASET}
{#ENDIFN DATASETTYPE}
{#IFDEF SHOWDETAILS}       
    private DataColumn FPrimaryKeyColumn = null;
    private Control FPrimaryKeyControl = null;
    private Label FPrimaryKeyLabel = null;
    private string FDefaultDuplicateRecordHint = String.Empty;
{#ENDIF SHOWDETAILS}
{#IFDEF FILTERANDFIND}
    {#FILTERANDFINDDECLARATIONS}
{#ENDIF FILTERANDFIND}
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

    /// <summary>Loads the data for the screen and finishes the setting up of the screen.</summary>
    /// <returns>void</returns>    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
      {#INITUSERCONTROLS}
      Type DataTableType;
      
      // Load Data     
{#IFDEF DATASETTYPE}
      FMainDS = new {#DATASETTYPE}();
{#ENDIF DATASETTYPE}
{#IFNDEF DATASETTYPE}
      FMainDS = new TLocalMainTDS();
{#ENDIFN DATASETTYPE}
      DataTable CacheDT = TDataCache.{#CACHEABLETABLERETRIEVEMETHOD}({#CACHEABLETABLE}, {#CACHEABLETABLESPECIFICFILTERLOAD}, out DataTableType);
      FMainDS.{#DETAILTABLE}.Merge(CacheDT);    
      
{#IFDEF ACTIONENABLING}
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}
      {#INITMANUALCODE}
{#IFDEF SAVEDETAILS}
      grdDetails.Enter += new EventHandler(grdDetails_Enter);
      grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(grdDetails_FocusRowLeaving);
      grdDetails.Selection.SelectionChanged += new RangeRegionChangedEventHandler(grdDetails_RowSelected);
      {#GRIDMULTISELECTION}
{#ENDIF SAVEDETAILS}
      pnlDetails.Enabled = false;
      
      DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
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
      SetupFilterAndFindControls();
{#ENDIF FILTERANDFIND}
      ShowData();
      SelectRowInGrid(1);
    }
#endregion

#region Standard Event Handlers

    {#EVENTHANDLERSIMPLEMENTATION}
#endregion

#region Create New Record

    /// <summary>
    /// This automatically generated method creates a new record of {#DETAILTABLE}, highlights it in the grid
    /// and displays it on the edit screen.  We create the table locally, no dataset
    /// </summary>
    /// <returns>True if the existing Details data was validated successfully and the new row was added.</returns>
    private bool CreateNew{#DETAILTABLE}()
    {
        if(ValidateAllData(true, true, false))
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
{#IFDEF FILTERANDFIND}

            if (!SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1))
            {
                if (FCurrentActiveFilter != FFilterPanelControls.BaseFilter)
                {
                    MessageBox.Show(
                        MCommonResourcestrings.StrNewRecordIsFiltered,
                        MCommonResourcestrings.StrAddNewRecordTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FFilterPanelControls.ClearAllDiscretionaryFilters();

                    if (FucoFilterAndFind.ShowApplyFilterButton != TUcoFilterAndFind.FilterContext.None)
                    {
                        ApplyFilter();
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
                //Look for Key & Description fields
                Control keyControl = null;
                Control descriptionControl = null;
                GetKeyControlsOnPanel(pnl[0], ref keyControl, ref descriptionControl);

                if ((descriptionControl != null) && (descriptionControl.Text == String.Empty))
                {
                    descriptionControl.Text = MCommonResourcestrings.StrPleaseEnterDescription;
                }

                ValidateAllData(true, false);

                if (keyControl != null)
                {
                    keyControl.Focus();
                }
            }
    
{#IFDEF BUTTONPANEL}
            UpdateRecordNumberDisplay();
{#ENDIF BUTTONPANEL}
            
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// A generic method to discover the key controls on a Panel (usually pnlDetails).
    /// </summary>
    /// <param name="APanel">A reference to the Panel to search</param>
    /// <param name="AKeyControl">Initialise this to null.  On return it will contain a reference to the 'prime' control (if found) editable by the user.</param>
    /// <param name="ADescriptionControl">Initialise this to null.  On return it will contain a reference to the TextBox control (if found)
    ///    that corresponds to a 'Description' column in the database.</param>
    /// <param name="ASkipSearchForDescription">You can set this to True if you only need to know the KeyControl.</param>
      private void GetKeyControlsOnPanel(Control APanel, ref Control AKeyControl, ref Control ADescriptionControl, bool ASkipSearchForDescription = false)
    {
        foreach (Control detailsCtrl in APanel.Controls)
        {
            if (detailsCtrl is Panel)
            {
                // If the control is a panel we call ourself recursively
                GetKeyControlsOnPanel(detailsCtrl, ref AKeyControl, ref ADescriptionControl, ASkipSearchForDescription);

                if ((ADescriptionControl != null) || ((AKeyControl != null) && ASkipSearchForDescription))
                {
                    break;
                }

                continue;
            }

            if (AKeyControl == null && (detailsCtrl is TextBox || detailsCtrl is ComboBox || detailsCtrl is TCmbAutoPopulated))
            {
                AKeyControl = detailsCtrl;

                if (ASkipSearchForDescription)
                {
                    break;
                }
            }

            if (detailsCtrl is TextBox && detailsCtrl.Name.Contains("Desc"))
            {
                ADescriptionControl = detailsCtrl;
                break;
            }
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
    private void SelectRowInGrid(int ARowIndex)
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

        SelectRowInGrid(RowNumberGrid);

        return RowNumberGrid >= 0;
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
            int dataRowIndex = 0;
            
            foreach ({#DETAILTABLETYPE}Row myRow in FMainDS.{#DETAILTABLETYPE}.Rows)
            {
                bool found = true;
                foreach (DataColumn myColumn in FMainDS.{#DETAILTABLETYPE}.PrimaryKey)
                {
                    if (myRow.RowState != DataRowState.Deleted)
                    {
                        string value1 = myRow[myColumn].ToString();
                        string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[FPrevRowChangedRow - 1][myColumn.Ordinal].ToString();
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

    /// <summary>
    /// Gets the selected Data Row as a PBusiness record from the grid
    /// </summary>
    /// <returns>The selected row - or null if no row is selected</returns>
    public {#DETAILTABLETYPE}Row GetSelectedDetailRow()
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

#region Show and Undo Data

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
#endregion

#region Data Validation

    /// <summary>
    /// Performs data validation.
    /// </summary>
    /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing 
    /// to another record, otherwise set it to false.</param>
    /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
    /// user, otherwise set it to false.</param>
    /// <param name="ADontRecordNewDataValidationRun">Set to false if no new DataValidationRun should be recorded. 
    /// Should be set to true only if called from within this very UserControl to ensure that an external call to the 
    /// UserControl's ValidateAllData Method doesn't change a recorded DataValidationRun that was set from the 
    /// Form/UserControl that embeds this UserControl! (Default=true).</param>
    /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>    
    private bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors, bool ADontRecordNewDataValidationRun = true)
    {
        bool ReturnValue = false;
        if (!ADontRecordNewDataValidationRun)
        {
            // Record a new Data Validation Run. (All TVerificationResults/TScreenVerificationResults that are created during this 'run' are associated with this 'run' through that.)
            FPetraUtilsObject.VerificationResultCollection.RecordNewDataValidationRun();
        }

{#IFNDEF SHOWDETAILS}
{#IFDEF MASTERTABLE}
// :CMCT:GetDataFromControls
        GetDataFromControls(FMainDS.{#MASTERTABLE}[0]);
        ValidateData(FMainDS.{#MASTERTABLE}[0]);
{#IFDEF VALIDATEDATAMANUAL}
// :CMCT:ValidateDataManual
        ValidateDataManual(FMainDS.{#MASTERTABLE}[0]);
{#ENDIF VALIDATEDATAMANUAL}
{#ENDIF MASTERTABLE}
{#ENDIFN SHOWDETAILS}
{#IFDEF SHOWDETAILS}
        if (FPreviouslySelectedDetailRow != null)
        {
            bool bGotConstraintException = false;
            int prevRowBeforeValidation = FPrevRowChangedRow;
// :CMCT:GetDetailsFromControls
            try
            {
                GetDetailsFromControls(FPreviouslySelectedDetailRow);
                ValidateDataDetails(FPreviouslySelectedDetailRow);
{#IFDEF VALIDATEDATADETAILSMANUAL}
// :CMCT:ValidateDataDetailsManual
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
            // If it has moved we will call the special grid-sorting method SelectRowWithoutFocus to highlight the new row.
            // This will give rise to a Selection_SelectionChanged event with a new ActivePosition but the grdDetails.Sorting property will be True
            // Furthermore with Filter/Find we need to be sure that we have not lost the row altogether by using a different row filter
            //  and we also need to protect against recursively call SelectRowWithoutFocus
            int newRowAfterValidation = grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow, prevRowBeforeValidation - 1) + 1;
            if (newRowAfterValidation == 0)
            {
{#IFDEF FILTERANDFIND}
                // The row is no longer in the view - probably because the new data values are being filtered out
                if ((FCurrentActiveFilter != FFilterPanelControls.BaseFilter) || !FFilterPanelControls.BaseFilterShowsAllRecords)
                {
                    // Remember the control with the focus
                    Control c = FPetraUtilsObject.GetFocusedControl(pnlDetails);

                    // Clear the filters without selecting a new row
                    FClearingDiscretionaryFilters = true;
                    FFilterPanelControls.ClearAllDiscretionaryFilters();

                    if (FucoFilterAndFind.ShowApplyFilterButton != TUcoFilterAndFind.FilterContext.None)
                    {
                        ApplyFilter();
                    }

                    FClearingDiscretionaryFilters = false;

                    // Now find the row again - this time we should succeed.  If not some other piece of code will have to do the selection
                    newRowAfterValidation = grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow, prevRowBeforeValidation - 1) + 1;

                    if (newRowAfterValidation > 0)
                    {
                        // Select the row
                        grdDetails.SelectRowWithoutFocus(newRowAfterValidation);
                    
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
                    grdDetails.SelectRowWithoutFocus(newRowAfterValidation);
                    //Console.WriteLine("{0}:    Validation: validated row moved to {1}.", DateTime.Now.Millisecond, newRowAfterValidation);
                }
            }
{#ENDIF SHOWDETAILS}
            
{#IFDEF PERFORMUSERCONTROLVALIDATION}

            // Perform validation in UserControls, too
            {#USERCONTROLVALIDATION}
{#ENDIF PERFORMUSERCONTROLVALIDATION}

            if (AProcessAnyDataValidationErrors)
            {
                ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                    this.GetType());    
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

#region Show Data, Show Details, Grid Events and Deletion
{#IFDEF SHOWDATA}

    #region Show Data
    private void ShowData({#MASTERTABLETYPE}Row ARow)
    {
        {#SHOWDATA}
    }
    #endregion
{#ENDIF SHOWDATA}
{#IFDEF SHOWDETAILS}

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
                FPreviouslySelectedDetailRow = ({#DETAILTABLE}Row)(rowView.Row);
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
    private void ShowDetails({#DETAILTABLE}Row ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();

        if (ARow == null)
        {
            pnlDetails.Enabled = false;
            {#CLEARDETAILS}
        }
        else
        {
            pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
            {#SHOWDETAILS}
        }

        {#ENABLEDELETEBUTTON}FPetraUtilsObject.EnableDataChangedEvent();
    }
{#CANDELETESELECTION}
#endregion

    #region Grid Events

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
    private {#DETAILTABLE}Row FPreviouslySelectedDetailRow = null;
    
    /// <summary>
    /// This variable may become obsolete in future.  It used to hold the most recent row passed as the parameter to the FocusedRowChanged event.
    /// However we no longer use this event.  If you want to know the current row index use GetSelectedRowIndex() instead.
    /// </summary>
    private int FPrevRowChangedRow = -1;

{#IFDEF SAVEDETAILS}
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
            ShowDetails(gridRow);
            // Console.WriteLine("{0}: RowSelected: ShowDetails() for row {1}", DateTime.Now.Millisecond, gridRow);
        }
    }

    /// <summary>
    /// FocusedRowLeaving is called when the user (or code) requests a change to the selected row.
    /// </summary>
    private void grdDetails_FocusRowLeaving(object sender, SourceGrid.RowCancelEventArgs e)
    {        
        if (!ValidateAllData(true, true))
        {
            e.Cancel = true;
        }
    }
{#ENDIF SAVEDETAILS}
#endregion

    #region Deleting records
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
        string CompletionMessage = String.Empty;
        
        if ((FPreviouslySelectedDetailRow == null) || (FPrevRowChangedRow == -1))
        {
            return;
        }

        DataRowView[] HighlightedRows = grdDetails.SelectedDataRowsAsDataRowView;

        if (HighlightedRows.Length == 1)
        {
            TVerificationResultCollection VerificationResults = null;

            {#DELETEREFERENCECOUNT}

            if ((VerificationResults != null)
                && (VerificationResults.Count > 0))
            {
                MessageBox.Show(Messages.BuildMessageFromVerificationResult(
                        MCommonResourcestrings.StrRecordCannotBeDeleted +
                        Environment.NewLine +
                        Catalog.GetPluralString(MCommonResourcestrings.StrReasonColon, MCommonResourcestrings.StrReasonsColon, VerificationResults.Count),
                        VerificationResults),
                        MCommonResourcestrings.StrRecordDeletionTitle);
                return;
            }

            string DeletionQuestion = MCommonResourcestrings.StrDefaultDeletionQuestion;
            if ((FPrimaryKeyControl != null) && (FPrimaryKeyLabel != null))
            {
                DeletionQuestion += String.Format("{0}{0}({1} {2})",
                    Environment.NewLine,
                    FPrimaryKeyLabel.Text.Replace("&", ""),
                    TControlExtensions.GetDisplayTextForControl(FPrimaryKeyControl));
            }

            bool AllowDeletion = true;
            bool DeletionPerformed = false;

            {#PREDELETEMANUAL}
            if(AllowDeletion)
            {
                if ((MessageBox.Show(DeletionQuestion,
                         MCommonResourcestrings.StrConfirmDeleteTitle,
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Question,
                         MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes))
                {
{#IFDEF DELETEROWMANUAL}
                    try
                    {
                        {#DELETEROWMANUAL}
{#ENDIF DELETEROWMANUAL}
{#IFNDEF DELETEROWMANUAL}               
                    try
                    {
                        FPreviouslySelectedDetailRow.Delete();
                        DeletionPerformed = true;
{#ENDIFN DELETEROWMANUAL}               
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format(MCommonResourcestrings.StrErrorWhileDeleting,
                            Environment.NewLine, ex.Message),
                            MCommonResourcestrings.StrGenericError,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
            
                    if (DeletionPerformed)
                    {
                        FPetraUtilsObject.SetChangedFlag();
                    }
            
                    // Select and display the details of the nearest row to the one previously selected
                    SelectRowInGrid(FPrevRowChangedRow);
                    // Clear any errors left over from  the deleted row
                    FPetraUtilsObject.VerificationResultCollection.Clear();
{#IFDEF BUTTONPANEL}
                    UpdateRecordNumberDisplay();
{#ENDIF BUTTONPANEL}
                }
            }

{#IFDEF POSTDELETEMANUAL}
            {#POSTDELETEMANUAL}
{#ENDIF POSTDELETEMANUAL}
{#IFNDEF POSTDELETEMANUAL}
            if(DeletionPerformed && CompletionMessage.Length > 0)
            {
                MessageBox.Show(CompletionMessage, MCommonResourcestrings.StrDeletionCompletedTitle);
            }
{#ENDIFN POSTDELETEMANUAL}
        }
        else
        {
            string DeletionQuestion = String.Format(MCommonResourcestrings.StrMultiRowDeletionQuestion, HighlightedRows.Length, Environment.NewLine);
            DeletionQuestion += MCommonResourcestrings.StrMultiRowDeletionCheck;
            if (MessageBox.Show(DeletionQuestion,
                    MCommonResourcestrings.StrConfirmDeleteTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                int recordsDeleted = 0;
                int recordsUndeletable = 0;
                int recordsDeleteDisallowed = 0;
                List<TMultiDeleteResult> listConflicts = new List<TMultiDeleteResult>();
                List<TMultiDeleteResult> listExceptions = new List<TMultiDeleteResult>();

                this.Cursor = Cursors.WaitCursor;

                foreach (DataRowView drv in HighlightedRows)
                {
                    {#DETAILTABLE}Row rowToDelete = ({#DETAILTABLE}Row)(drv.Row);
                    string rowDetails = MakePKValuesString(rowToDelete);

                    {#MULTIDELETEDELETABLE}

                    TVerificationResultCollection VerificationResults = null;
                    {#MULTIDELETEREFERENCECOUNT}

                    if ((VerificationResults != null) && (VerificationResults.Count > 0))
                    {
                        TMultiDeleteResult result = new TMultiDeleteResult(rowDetails,
                            Messages.BuildMessageFromVerificationResult(String.Empty, VerificationResults));
                        listConflicts.Add(result);
                        continue;
                    }

                    bool AllowDeletion = true;
                    bool DeletionPerformed = false;

                    {#PREMULTIDELETEMANUAL}
{#IFDEF DELETEROWMANUAL}
                    if (!AllowDeletion)
                    {
                        recordsDeleteDisallowed++;
                    }

                    try
                    {
                        {#DELETEMULTIROWMANUAL}
                    }
                    catch (Exception ex)
                    {
                        TMultiDeleteResult result = new TMultiDeleteResult(rowDetails, ex.Message);
                        listExceptions.Add(result);
                    }
{#ENDIF DELETEROWMANUAL}
{#IFNDEF DELETEROWMANUAL}               

                    if (AllowDeletion)
                    {
                        try
                        {
                            rowToDelete.Delete();
                            DeletionPerformed = true;
                        }
                        catch (Exception ex)
                        {
                            TMultiDeleteResult result = new TMultiDeleteResult(rowDetails, ex.Message);
                            listExceptions.Add(result);
                        }
                    }
                    else
                    {
                        recordsDeleteDisallowed++;
                    }
{#ENDIFN DELETEROWMANUAL}               

                    if (DeletionPerformed)
                    {
                        FPetraUtilsObject.SetChangedFlag();
                        recordsDeleted++;
                    }

                    {#POSTMULTIDELETEMANUAL}
                }

                this.Cursor = Cursors.Default;
                SelectRowInGrid(FPrevRowChangedRow);
{#IFDEF BUTTONPANEL}
                UpdateRecordNumberDisplay();
{#ENDIF BUTTONPANEL}

                if (recordsDeleted > 0 && CompletionMessage.Length > 0)
                {
                    MessageBox.Show(CompletionMessage, MCommonResourcestrings.StrDeletionCompletedTitle);
                }

                //  Show the results of the multi-deletion
                string results = null;
                
                if (recordsDeleted > 0)
                {
                    results = String.Format(
                            Catalog.GetPluralString(MCommonResourcestrings.StrRecordSuccessfullyDeleted, MCommonResourcestrings.StrRecordsSuccessfullyDeleted, recordsDeleted),
                            recordsDeleted);
                }
                else
                {
                    results = MCommonResourcestrings.StrNoRecordsWereDeleted;                }
                
                if (recordsUndeletable > 0)
                {
                    results += String.Format(
                        Catalog.GetPluralString(MCommonResourcestrings.StrRowNotDeletedBecauseNonDeletable,
                                                MCommonResourcestrings.StrRowsNotDeletedBecauseNonDeletable,
                                                recordsUndeletable),
                        Environment.NewLine,
                        recordsUndeletable);
                }

                if (recordsDeleteDisallowed > 0)
                {
                    results += String.Format(
                        Catalog.GetPluralString(MCommonResourcestrings.StrRowNotDeletedBecauseDeleteNotAllowed,
                                                MCommonResourcestrings.StrRowsNotDeletedBecauseDeleteNotAllowed,
                                                recordsDeleteDisallowed),
                        Environment.NewLine,
                        recordsDeleteDisallowed);
                }

                bool showCancel = false;
                
                if (listConflicts.Count > 0)
                {
                    showCancel = true;
                    results += String.Format(
                        Catalog.GetPluralString(MCommonResourcestrings.StrRowNotDeletedBecauseReferencedElsewhere,
                                                MCommonResourcestrings.StrRowsNotDeletedBecauseReferencedElsewhere,
                                                listConflicts.Count),
                        Environment.NewLine,
                        listConflicts.Count);
                }
                
                if (listExceptions.Count > 0)
                {
                    showCancel = true;
                    results += String.Format(
                        Catalog.GetPluralString(MCommonResourcestrings.StrRowNotDeletedDueToUnexpectedException,
                                                MCommonResourcestrings.StrRowNotDeletedDueToUnexpectedException,
                                                listExceptions.Count),
                        Environment.NewLine,
                        listExceptions.Count);
                }
                
                if (showCancel)
                {
                    results += String.Format(MCommonResourcestrings.StrClickToReviewDeletionOrCancel, Environment.NewLine);

                    if (MessageBox.Show(results,
                            MCommonResourcestrings.StrDeleteActionSummaryTitle,
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                    {
                        ReviewMultiDeleteResults(listConflicts, MCommonResourcestrings.StrRowsReferencedByOtherTables);
                        ReviewMultiDeleteResults(listExceptions, MCommonResourcestrings.StrExceptions);
                    }
                }
                else
                {
                    MessageBox.Show(results,
                        MCommonResourcestrings.StrDeleteActionSummaryTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }
    }

    private string MakePKValuesString({#DETAILTABLE}Row ARow)
    {
        string ReturnValue = String.Empty;
        object[] items = DataUtilities.GetPKValuesFromDataRow(ARow);

        for (int i = 0; i < items.Length; i++)
        {
            if (i > 0)
            {
                ReturnValue += ", ";
            }

            ReturnValue += items[i].ToString();
        }

        return ReturnValue;
    }

    private class TMultiDeleteResult
    {
        private string FRecordID;
        private string FResult;

        public TMultiDeleteResult(string ARecordID, string AResult)
        {
            FRecordID = ARecordID;
            FResult = AResult;
        }

        public string RecordID
        {
            get
            {
                return FRecordID;
            }
        }

        public string Result
        {
            get
            {
                return FResult;
            }
        }

    }

    private void ReviewMultiDeleteResults(List<TMultiDeleteResult> AList, string ATitle)
    {
        int allItemsCount = AList.Count;
        int item = 0;

        foreach (TMultiDeleteResult result in AList)
        {
            item++;
            string s1 = result.RecordID;
            string s2 = result.Result;

            string details = String.Format(MCommonResourcestrings.StrItemXofYRecordColon,
                ATitle, item, allItemsCount, Environment.NewLine, s1, s2);

            if (item < allItemsCount)
            {
                details += String.Format(MCommonResourcestrings.StrViewNextDetailOrCancel, Environment.NewLine);
                if (MessageBox.Show(details, MCommonResourcestrings.StrMoreDetailsAboutRowsNotDeleted, MessageBoxButtons.OKCancel)
                    == System.Windows.Forms.DialogResult.Cancel)
                {
                    break;
                }
            }
            else
            {
                MessageBox.Show(details, MCommonResourcestrings.StrMoreDetailsAboutRowsNotDeleted, MessageBoxButtons.OK);
            }
        }
    }
#endregion
{#ENDIF SHOWDETAILS}

#endregion

#region Get Data    
{#IFDEF MASTERTABLE}

    /// This method may throw an exception at ARow.EndEdit()
    private void GetDataFromControls({#MASTERTABLETYPE}Row ARow, Control AControl=null)
    {
{#IFDEF SAVEDATA}
        if (ARow == null) return;

        object[] beforeEdit = ARow.ItemArray;
        ARow.BeginEdit();
        {#SAVEDATA}
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

    private void GetDataFromControls()
    {
{#IFDEF SAVEDATA}
        {#SAVEDATA}
{#ENDIF SAVEDATA}
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
                object[] beforeEdit = ARow.ItemArray;
                ARow.BeginEdit();
                {#SAVEDETAILS}
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
    
    private void UpdateRecordNumberDisplay()
    {
        int RecordCount;
        
        if (grdDetails.DataSource != null) 
        {
            RecordCount = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).Count;
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

#region Data Validation Control Handlers
    
    private void ControlValidatedHandler(object sender, EventArgs e)
    {
        TScreenVerificationResult SingleVerificationResult;
        
        ValidateAllData(true, false, false);
        
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

                    FPetraUtilsObject.ValidationToolTipSeverity = SingleVerificationResult.ResultSeverity;

                    if (SingleVerificationResult.ResultTextCaption != String.Empty) 
                    {
                        FPetraUtilsObject.ValidationToolTip.ToolTipTitle += ":  " + SingleVerificationResult.ResultTextCaption;    
                    }
{#IFDEF UNDODATA}

                    if(SingleVerificationResult.ControlValueUndoRequested)
                    {
                        UndoData(SingleVerificationResult.ResultColumn.Table.Rows[0], SingleVerificationResult.ResultControl);
                        SingleVerificationResult.OverrideResultText(SingleVerificationResult.ResultText + Environment.NewLine + Environment.NewLine + 
                            Catalog.GetString("--> The value you entered has been changed back to what it was before! <--"));
                    }
{#ENDIF UNDODATA}

                    FPetraUtilsObject.ValidationToolTip.Show(SingleVerificationResult.ResultText, (Control)sender, 
                        ((Control)sender).Width / 2, ((Control)sender).Height);
                }
            }
        }
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
{#IFDEF DATASETTYPE}
        if (FMainDS != null)
        {
{#ENDIF DATASETTYPE}        
{#IFDEF ADDCONTROLTOVALIDATIONCONTROLSDICT}
            {#ADDCONTROLTOVALIDATIONCONTROLSDICT}
{#ENDIF ADDCONTROLTOVALIDATIONCONTROLSDICT}
{#IFDEF DATASETTYPE}
        }
{#ENDIF DATASETTYPE}
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
{#INCLUDE inline_typed_dataset.cs}
{#INCLUDE findandfilter.cs}

{##SNIPDELETEREFERENCECOUNT}
if (!FPetraUtilsObject.VerificationResultCollection.HasCriticalErrors)
{
    this.Cursor = Cursors.WaitCursor;
    TRemote.{#CONNECTORNAMESPACE}.ReferenceCount.WebConnectors.GetCacheableRecordReferenceCount(
        "{#CACHEABLETABLENAME}",
        DataUtilities.GetPKValuesFromDataRow(FPreviouslySelectedDetailRow),
        out VerificationResults);
    this.Cursor = Cursors.Default;
}

{##SNIPMULTIDELETEREFERENCECOUNT}
TRemote.{#CONNECTORNAMESPACE}.ReferenceCount.WebConnectors.GetCacheableRecordReferenceCount(
    "{#CACHEABLETABLENAME}",
    DataUtilities.GetPKValuesFromDataRow(rowToDelete),
    out VerificationResults);

{##SNIPMULTIDELETEDELETABLE}
if (!rowToDelete.{#DELETEABLEFLAG})
{
    recordsUndeletable++;
    continue;
}

{##SNIPCANDELETESELECTION}

    /// <summary>
    /// Returns true if all the selected rows can be deleted.
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

