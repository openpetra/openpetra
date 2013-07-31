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
    private DataColumn FPrimaryKeyColumn = null;
    private Control FPrimaryKeyControl = null;
    private Label FPrimaryKeyLabel = null;
    private string FDefaultDuplicateRecordHint = String.Empty;
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
{#IFDEF ACTIONENABLING}
        FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}
        {#INITMANUALCODE}
        grdDetails.Enter += new EventHandler(grdDetails_Enter);
        grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
        {#GRIDMULTISELECTION}
        pnlDetails.Enabled = false;
      
        if((FMainDS != null)
          && (FMainDS.{#DETAILTABLE} != null))
        {
            DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

{#IFDEF MASTERTABLE OR DETAILTABLE}
            BuildValidationControlsDict();
{#ENDIF MASTERTABLE OR DETAILTABLE}
{#IFDEF SHOWDETAILS}       
            SetPrimaryKeyControl();
{#ENDIF SHOWDETAILS}

            ShowData();
        }
{#IFDEF ENABLEDELETEBUTTON}

        btnDelete.Enabled = pnlDetails.Enabled;
{#ENDIF ENABLEDELETEBUTTON}
    }
    
    {#EVENTHANDLERSIMPLEMENTATION}

    /// <summary>
    /// This automatically generated method creates a new record of {#DETAILTABLE}, highlights it in the grid
    /// and displays it on the edit screen.  We create the table locally, no dataset
    /// </summary>
    /// <returns>True if the existing Details data was validated successfully and the new row was added.</returns>
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
            
            Control[] pnl = this.Controls.Find("pnlDetails", true);
            if (pnl.Length > 0)
            {
                //Look for Key & Description fields
                Control keyControl = null;
                foreach (Control detailsCtrl in pnl[0].Controls)
                {
                    if (keyControl == null && (detailsCtrl is TextBox || detailsCtrl is ComboBox || detailsCtrl is TCmbAutoPopulated))
                    {
                        keyControl = detailsCtrl;
                    }

                    if (detailsCtrl is TextBox && detailsCtrl.Name.Contains("Descr") && detailsCtrl.Text == string.Empty)
                    {
                        detailsCtrl.Text = Catalog.GetString("PLEASE ENTER DESCRIPTION");
                        break;
                    }
                }

                ValidateAllData(true, false);
                if (keyControl != null) keyControl.Focus();
            }
            
            return true;
        }
        else
        {
            return false;
        }
    }

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
        int nPrevRowChangedRow = FPrevRowChangedRow;
        grdDetails.SelectRowInGrid(ARowIndex, true);
        if (nPrevRowChangedRow == FPrevRowChangedRow)
        {
            // No row change occurred, so we still need to show details, because the data may be different
            //Console.WriteLine("{0}:  UC SRIG: ShowDetails for {1}", DateTime.Now.Millisecond, ARowIndex);
            ShowDetails(ARowIndex);
        }
    }

    /// <summary>
    /// Selects a grid row based its index in the data table (often the last, newest, row).
    /// The details panel is automatically updated to show the new details.
    /// If the grid is not displaying the specified data row, the first row will be selected, if it exists.
    /// </summary>
    /// <param name="ARowNumberInTable">Table row number (0-based)</param>
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

        SelectRowInGrid(RowNumberGrid);
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
            
            foreach ({#DETAILTABLE}Row myRow in FMainDS.{#DETAILTABLE}.Rows)
            {
                bool found = true;
                foreach (DataColumn myColumn in FMainDS.{#DETAILTABLE}.PrimaryKey)
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
                SelectRowInGrid(1);
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
        
        {#ENABLEDELETEBUTTON}FPetraUtilsObject.EnableDataChangedEvent();
    }

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

    private void grdDetails_Enter(object sender, EventArgs e)
    {
        if (FPetraUtilsObject.VerificationResultCollection.Count > 0)
        {
            grdDetails.Selection.Focus(new SourceGrid.Position(FPrevRowChangedRow, 0), false);
            //Console.WriteLine("{0}: GridFocus - setting Selection.Focus to {1},0", DateTime.Now.Millisecond, nRow);
        }
    }
{#SELECTIONCHANGEDHANDLER}

    /// <summary>
    /// Used for determining the time elapsed between FocusRowLeaving Events.
    /// </summary>
    private DateTime FDtPrevLeaving = DateTime.UtcNow;
    private int FPrevLeavingFrom = -1;
    private int FPrevLeavingTo = -1;

    /// FocusedRowLeaving can be called multiple times (e.g. 3 or 4) for just one FocusedRowChanged event.
    /// The key is not to cancel the extra events, but to ensure that we only ValidateAllData once.
    /// We ignore any event that is leaving to go to row # -1
    /// We validate on the first of a cascade of events that leave to a real row.
    /// We detect a duplicate event by testing for the elapsed time since the event we validated on...
    /// If the elapsed time is &lt; 2 ms it is a duplicate, because repeat keypresses are separated by 30 ms
    /// and these duplicates come with a gap of fractions of a microsecond, so 2 ms is a very long time!
    /// All we do is store the previous row from/to and the previous UTC time
    /// These three form level variables are totally private to this event call.
    private void FocusRowLeaving(object sender, SourceGrid.RowCancelEventArgs e)
    {        
        if (!grdDetails.Sorting && e.ProposedRow >= 0)
        {
            double elapsed = (DateTime.UtcNow - FDtPrevLeaving).TotalMilliseconds;
            bool bIsDuplicate = (e.Row == FPrevLeavingFrom && e.ProposedRow == FPrevLeavingTo && elapsed < 2.0);
            if (!bIsDuplicate)
            {
                //Console.WriteLine("{0}: UC  FocusRowLeaving: from {1} to {2}", DateTime.Now.Millisecond, e.Row, e.ProposedRow);
                if (!ValidateAllData(true, true))
                {
                    //Console.WriteLine("{0}:    --- UC  Cancelled", DateTime.Now.Millisecond);
                    e.Cancel = true;
                }
            }
            FPrevLeavingFrom = e.Row;
            FPrevLeavingTo = e.ProposedRow;
            FDtPrevLeaving = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// This variable is managed by the generated code.  It is used to manage row changed events, including changes that occur in data validation on sorted grids.
    /// Do not set this variable in manual code.
    /// You may read the variable.  Its value always tracks the index of the highlighted grid row.
    /// </summary>
    private int FPrevRowChangedRow = -1;
    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
        // The FocusedRowChanged event simply calls ShowDetails for the new 'current' row implied by e.Row
        // We do get a duplicate event if the user tabs round all the controls multiple times
        // It is not advisable to call it on duplicate events because that would re-populate the controls from the table, 
        //   which may not now be up to date, so we compare e.Row and FPrevRowChangedRow first.
        if (!grdDetails.Sorting && e.Row != FPrevRowChangedRow)
        {
            //Console.WriteLine("{0}:   UC  FRC ShowDetails for {1}", DateTime.Now.Millisecond, e.Row);
            ShowDetails(e.Row);
        }
        FPrevRowChangedRow = e.Row;
    }
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
                        Catalog.GetString("Record cannot be deleted!") +
                        Environment.NewLine +
                        Catalog.GetPluralString("Reason:", "Reasons:", VerificationResults.Count),
                        VerificationResults),
                        Catalog.GetString("Record Deletion"));
                return;
            }

            string DeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");
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
                         Catalog.GetString("Confirm Delete"),
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
                        MessageBox.Show(String.Format(Catalog.GetString("An error occurred while deleting this record.{0}{0}{1}"),
                            Environment.NewLine, ex.Message),
                            Catalog.GetString("Error"),
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
                }
            }

{#IFDEF POSTDELETEMANUAL}
            {#POSTDELETEMANUAL}
{#ENDIF POSTDELETEMANUAL}
{#IFNDEF POSTDELETEMANUAL}
            if(DeletionPerformed && CompletionMessage.Length > 0)
            {
                MessageBox.Show(CompletionMessage,
                                 Catalog.GetString("Deletion Completed"));
            }
{#ENDIFN POSTDELETEMANUAL}
        }
        else
        {
            string DeletionQuestion = String.Format(Catalog.GetString("Do you want to delete the {0} highlighted rows?{1}{1}"), HighlightedRows.Length, Environment.NewLine);
            DeletionQuestion += Catalog.GetString("Each record will be checked to confirm that it can be deleted.");
            if (MessageBox.Show(DeletionQuestion,
                    Catalog.GetString("Confirm Delete"),
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
                    {#DETAILTABLETYPE}Row rowToDelete = ({#DETAILTABLETYPE}Row)(drv.Row);
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

                if (recordsDeleted > 0 && CompletionMessage.Length > 0)
                {
                    MessageBox.Show(CompletionMessage,
                                     Catalog.GetString("Deletion Completed"));
                }

                //  Show the results of the multi-deletion
                string results = null;
                
                if (recordsDeleted > 0)
                {
                    string s1 = Catalog.GetPluralString("record", "records", recordsDeleted);
                    string s2 = Catalog.GetPluralString("was", "were", recordsDeleted);
                    results = String.Format(Catalog.GetString("{0} {1} {2} successfully deleted."), recordsDeleted, s1, s2);
                }
                else
                {
                    results = "No records were deleted.";
                }
                
                if (recordsUndeletable > 0)
                {
                    string s1 = Catalog.GetPluralString("record", "records", recordsUndeletable);
                    string s2 = Catalog.GetPluralString("it is marked", "they are marked", recordsUndeletable);
                    results += String.Format(Catalog.GetString("{0}{1} {2} could not be deleted because {3} as non-deletable."),
                        Environment.NewLine,
                        recordsUndeletable,
                        s1, s2);
                }

                if (recordsDeleteDisallowed > 0)
                {
                    string s1 = Catalog.GetPluralString("record was not be deleted", "records were not be deleted", recordsUndeletable);
                    results += String.Format(Catalog.GetString("{0}{1} {2} because deletion was not allowed."),
                        Environment.NewLine,
                        recordsDeleteDisallowed,
                        s1);
                }

                bool showCancel = false;
                
                if (listConflicts.Count > 0)
                {
                    showCancel = true;
                    string s1 = Catalog.GetPluralString("record", "records", listConflicts.Count);
                    string s2 = Catalog.GetPluralString("it is referenced", "they are referenced", listConflicts.Count);
                    results += String.Format(Catalog.GetString("{0}{1} {2} could not be deleted because {3} by at least one other table."),
                        Environment.NewLine,
                        listConflicts.Count,
                        s1, s2);
                }
                
                if (listExceptions.Count > 0)
                {
                    showCancel = true;
                    string s1 = Catalog.GetPluralString("record", "records", listExceptions.Count);
                    results += String.Format(Catalog.GetString("{0}{1} {2} could not be deleted because the delete action failed unexpectedly."),
                        Environment.NewLine,
                        listExceptions.Count,
                        s1);
                }
                
                if (showCancel)
                {
                    results += String.Format(Catalog.GetString("{0}{0}Click OK to review the details, or Cancel to return direct to the data screen"),
                        Environment.NewLine);

                    if (MessageBox.Show(results,
                            Catalog.GetString("Delete Action Summary"),
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                    {
                        ReviewMultiDeleteResults(listConflicts, Catalog.GetString("Rows in this table that are referenced by other tables"));
                        ReviewMultiDeleteResults(listExceptions, Catalog.GetString("Unexpected Exceptions"));
                    }
                }
                else
                {
                    MessageBox.Show(results,
                        Catalog.GetString("Delete Action Summary"),
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

            string details = String.Format(Catalog.GetString("{0}: {1} of {2}{3}Record: {4}{3}{5}"),
                ATitle, item, allItemsCount, Environment.NewLine, s1, s2);

            if (item < allItemsCount)
            {
                details += String.Format(Catalog.GetString("{0}{0}Click OK to review the next detail or Cancel to finish."), Environment.NewLine);
                if (MessageBox.Show(details, Catalog.GetString("More Details About Rows Not Deleted"), MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                {
                    break;
                }
            }
            else
            {
                MessageBox.Show(details, Catalog.GetString("More Details About Rows Not Deleted"), MessageBoxButtons.OK);
            }
        }
    }
{#ENDIF SHOWDETAILS}
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
        ValidateAllData(false, false);
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
        if (ARow != null && ARow.RowState != DataRowState.Deleted && !pnlDetailsProtected && !grdDetails.Sorting)
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
            int prevRowChangedRowBeforeValidation = FPrevRowChangedRow;
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
                    String.Format(Catalog.GetString("You have attempted to create a duplicate record.  Please ensure that you have unique input data for the field(s) {0}."), FDefaultDuplicateRecordHint),
                    CommonErrorCodes.ERR_DUPLICATE_RECORD, null, TResultSeverity.Resv_Critical) : null, null);
            }
            else
            {
                TControlExtensions.ValidateNonDuplicateRecord(this, bGotConstraintException, FPetraUtilsObject.VerificationResultCollection, 
                            FPrimaryKeyColumn, FPrimaryKeyControl, FMainDS.{#DETAILTABLE}.PrimaryKey);
            }
            
            // Validation might have moved the row, so we need to locate it again
            // If it has moved we will call SelectRowInGrid (with events) to highlight the new row.
            // This will result in us getting called a second time (from FocusedRowLeaving), but the move will not be repeated a second time.
            // We thus avoid a cyclic loop and a stack overflow, yet never need to turn events off, or make a move without events
            // Note that we can (and must) set FPrevRowChangedRow here only because validation never actually changes the row object or the displayed details.
            FPrevRowChangedRow = grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow) + 1;
            if (FPrevRowChangedRow == prevRowChangedRowBeforeValidation)
            {
                //Console.WriteLine("{0}:    UC Validation: validated row is at {1}. No move required.  ProcessErrors={2}", DateTime.Now.Millisecond, FPrevRowChangedRow, AProcessAnyDataValidationErrors.ToString());
            }
            else
            {
                grdDetails.SelectRowInGrid(FPrevRowChangedRow);
                //Console.WriteLine("{0}:    UC Validation: validated row is at {1}. Moved 'with events'.  ProcessErrors={2}", DateTime.Now.Millisecond, FPrevRowChangedRow, AProcessAnyDataValidationErrors.ToString());
            }
{#ENDIF SHOWDETAILS}
{#IFDEF PERFORMUSERCONTROLVALIDATION}

// :CMT:ucValidation
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

{##SNIPDELETEREFERENCECOUNT}
if (!FPetraUtilsObject.VerificationResultCollection.HasCriticalErrors)
{
    this.Cursor = Cursors.WaitCursor;
    TRemote.{#CONNECTORNAMESPACE}.ReferenceCount.WebConnectors.GetNonCacheableRecordReferenceCount(
        FMainDS.{#NONCACHEABLETABLENAME},
        DataUtilities.GetPKValuesFromDataRow(FPreviouslySelectedDetailRow),
        out VerificationResults);
    this.Cursor = Cursors.Default;
}

{##SNIPMULTIDELETEREFERENCECOUNT}
TRemote.{#CONNECTORNAMESPACE}.ReferenceCount.WebConnectors.GetNonCacheableRecordReferenceCount(
    FMainDS.{#NONCACHEABLETABLENAME},
    DataUtilities.GetPKValuesFromDataRow(rowToDelete),
    out VerificationResults);

{##SNIPMULTIDELETEDELETABLE}
if (!rowToDelete.{#DELETEABLEFLAG})
{
    recordsUndeletable++;
    continue;
}

{##SNIPSELECTIONCHANGEDHANDLER}

    /// <summary>
    /// This method is required where the table has a deletable_flag column
    /// It ensures the correct enabled/disabled state of the delete button by calling ShowDetails() on the current row
    /// </summary>
        private void Selection_SelectionChanged(object sender, RangeRegionChangedEventArgs e)
    {
        if (e.RemovedRange != null && e.RemovedRange.GetRowsIndex().Length > 0 && e.RemovedRange.GetColumnsIndex().Length > 1 && grdDetails.Selection.EnableMultiSelection == true)
        {
            // This is called when the user CTRL+clicks the mouse to un-highlight a row
            ShowDetails();
        }
        else if (e.AddedRange != null && e.AddedRange.GetRowsIndex().Length > 0 && grdDetails.Selection.EnableMultiSelection == true)
        {
            // This is called (possibly several times) and is required for handling the case where the user is using SHIFT+up/down 
            ShowDetails();
        }
    }
