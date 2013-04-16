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
using Ict.Common.Controls;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
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

namespace {#NAMESPACE}
{

  /// auto generated: {#FORMTITLE}
  public partial class {#CLASSNAME}: System.Windows.Forms.Form, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;
{#FILTERVAR}
{#IFDEF SHOWDETAILS}       
    private DataColumn FPrimaryKeyColumn = null;
    private Control FPrimaryKeyControl = null;
    private string FDefaultDuplicateRecordHint = String.Empty;
{#ENDIF SHOWDETAILS}
{#IFDEF DATASETTYPE}
    private {#DATASETTYPE} FMainDS;
{#ENDIF DATASETTYPE}
{#IFNDEF DATASETTYPE}
    {#INLINETYPEDDATASET}
{#ENDIFN DATASETTYPE}

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
{#IFNDEF DATASETTYPE}
      FMainDS = new TLocalMainTDS();
{#ENDIFN DATASETTYPE}
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
{#IFDEF SHOWDETAILS}       
      SetPrimaryKeyControl();
{#ENDIF SHOWDETAILS}
    }

    #region Show Method overrides

    /// <summary>
    /// Override of Form.Show(IWin32Window owner) Method. Caters for singleton Forms.
    /// </summary>
    /// <param name="owner">Any object that implements <see cref="IWin32Window" /> and represents the top-level window that will own this Form. </param>    
    public new void Show(IWin32Window owner)
    {
        Form OpenScreen = TFormsList.GFormsList[this.GetType().FullName];
        bool OpenSelf = true;

        if ((OpenScreen != null)
            && (OpenScreen.Modal != true))            
        {
            if (TFormsList.GSingletonForms.Contains(this.GetType().Name)) 
            {
//                MessageBox.Show("Activating singleton screen of Type '" + this.GetType().FullName + "'.");
                                   
                OpenSelf = false;
                this.Visible = false;   // needed as this.Close() would otherwise bring this Form to the foreground and OpenScreen.BringToFront() would not help...
                this.Close();
                
                OpenScreen.BringToFront();
            }            
        }
        
        if (OpenSelf) 
        {
            if (owner != null) 
            {
                base.Show(owner);    
            }
            else
            {
                base.Show();
            }            
        }        
    }

    /// <summary>
    /// Override of Form.Show() Method. Caters for singleton Forms.
    /// </summary>        
    public new void Show()
    {
        this.Show(null);
    }

    #endregion

    {#EVENTHANDLERSIMPLEMENTATION}

    /// <summary>Loads the data for the screen and finishes the setting up of the screen.</summary>
    /// <returns>void</returns>
    private void LoadDataAndFinishScreenSetup()
    {      
      Type DataTableType;
      
      // Load Data
      DataTable CacheDT = TDataCache.{#CACHEABLETABLERETRIEVEMETHOD}({#CACHEABLETABLE}, {#CACHEABLETABLESPECIFICFILTERLOAD}, out DataTableType);
      FMainDS.{#DETAILTABLE}.Merge(CacheDT);    
      
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
      {#INITMANUALCODE}
{#IFDEF SAVEDETAILS}
      grdDetails.Enter += new EventHandler(grdDetails_Enter);
      grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
{#ENDIF SAVEDETAILS}
      
      DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
{#IFDEF GRIDSORT}
      myDataView.Sort = "{#GRIDSORT}";
{#ENDIF GRIDSORT}
{#IFDEF GRIDFILTER}
            myDataView.RowFilter = {#GRIDFILTER};
{#ENDIF GRIDFILTER}
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

    /// <summary>
    /// This automatically generated method creates a new record of {#DETAILTABLE}, highlights it in the grid
    /// and displays it on the edit screen.  We create the table locally, no dataset
    /// </summary>
    /// <returns>True if the existing Details data was validated successfully and the new row was added.</returns>
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

            SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);
            
            Control[] pnl = this.Controls.Find("pnlDetails", true);
            if (pnl.Length > 0)
            {
                //Look for Key & Description fields
                Control keyControl = null;
                foreach (Control detailsCtrl in pnl[0].Controls)
                {
                    if (keyControl == null && (detailsCtrl is TextBox || detailsCtrl is ComboBox))
                    {
                        keyControl = detailsCtrl;
                    }

                    if (detailsCtrl is TextBox && detailsCtrl.Name.Contains("Descr") && detailsCtrl.Text == string.Empty)
                    {
                        detailsCtrl.Text = "PLEASE ENTER DESCRIPTION";
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
            //Console.WriteLine("{0}:  SRIG: ShowDetails for {1}", DateTime.Now.Millisecond, ARowIndex);
            ShowDetails();
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
        
        int gridRowIndex = grdDetails.SelectedRowIndex();
        
        if (gridRowIndex > 0 && FPreviouslySelectedDetailRow != null)
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

    /// <summary>
    /// Gets the highlighted Data Row as a {#DETAILTABLE} record from the grid 
    /// </summary>
    /// <returns>The selected row - or null if no row is selected</returns>
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

{#IFNDEF SHOWDETAILS}
{#IFDEF MASTERTABLE}
// :WMCT:GetDataFromControls
        GetDataFromControls(FMainDS.{#MASTERTABLE}[0]);
        ValidateData(FMainDS.{#MASTERTABLE}[0]);
{#IFDEF VALIDATEDATAMANUAL}
// :WMCT:ValidateDataManual
        ValidateDataManual(FMainDS.{#MASTERTABLE}[0]);
{#ENDIF VALIDATEDATAMANUAL}
{#ENDIF MASTERTABLE}
{#IFDEF PERFORMUSERCONTROLVALIDATION}

        // Perform validation in UserControls too
// :WMCT:ucValidation
        {#USERCONTROLVALIDATION}

{#ENDIF PERFORMUSERCONTROLVALIDATION}

        if (AProcessAnyDataValidationErrors)
        {
            ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                this.GetType(), null);
        }
{#ENDIFN SHOWDETAILS}
{#IFDEF SHOWDETAILS}       
        if (FPreviouslySelectedDetailRow != null)
        {
            bool bGotConstraintException = false;
            int prevRowChangedRowBeforeValidation = FPrevRowChangedRow;
// :WMCT:GetDetailsFromControls
            try
            {
                GetDetailsFromControls(FPreviouslySelectedDetailRow);
                ValidateDataDetails(FPreviouslySelectedDetailRow);
{#IFDEF VALIDATEDATADETAILSMANUAL}
// :WMCT:ValidateDataDetailsManual
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
                //Console.WriteLine("{0}:    Validation: validated row is at {1}. No move required.  ProcessErrors={2}", DateTime.Now.Millisecond, FPrevRowChangedRow, AProcessAnyDataValidationErrors.ToString());
            }
            else
            {
                grdDetails.SelectRowInGrid(FPrevRowChangedRow);
                //Console.WriteLine("{0}:    Validation: validated row is at {1}. Moved 'with events'.  ProcessErrors={2}", DateTime.Now.Millisecond, FPrevRowChangedRow, AProcessAnyDataValidationErrors.ToString());
            }
{#IFDEF PERFORMUSERCONTROLVALIDATION}

            // Perform validation in UserControls too
// :WMCT:ucValidation
            {#USERCONTROLVALIDATION}
{#ENDIF PERFORMUSERCONTROLVALIDATION}

            if (AProcessAnyDataValidationErrors)
            {
                ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                    this.GetType(), null);
            }
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

{#IFDEF SHOWDETAILS}
    /// <summary>
    /// This overload shows the details for the currently highlighted row.
    /// The method still works when the grid is empty and no row can be selected.
    /// The Details panel is disabled when the grid is empty, or when in Detail Protected Mode
    /// The variable FPreviouslySelectedDetailRow is set by this call.
    /// </summary>
    private void ShowDetails()
    {
        ShowDetails(GetSelectedDetailRow());
    }

    /// <summary>
    /// This overload shows the details for the specified row, which can be null.
    /// The Details panel is disabled when the row is Null, or when in Detail Protected Mode
    /// The variable FPreviouslySelectedDetailRow is set by this call.
    /// </summary>
    /// <param name="ARow">The row for which details will be shown</param>
    private void ShowDetails({#DETAILTABLE}Row ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();

        FPreviouslySelectedDetailRow = ARow;
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
    
{#IFDEF SAVEDETAILS}
    private void grdDetails_Enter(object sender, EventArgs e)
    {
        int nRow = grdDetails.SelectedRowIndex();       // should be the same as FPrevRowChangedRow
        if (nRow > 0 && FPetraUtilsObject.VerificationResultCollection.Count > 0)
        {
            // No need to focus the row if there are no errors.  This allows the user to have scrolled the view-port away from the selected row and keep it there.
            grdDetails.Selection.Focus(new SourceGrid.Position(nRow, 0), false);
            //Console.WriteLine("{0}: GridFocus - setting Selection.Focus to {1},0", DateTime.Now.Millisecond, nRow);
        }
    }

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
                //Console.WriteLine("{0}: FocusRowLeaving: from {1} to {2}", DateTime.Now.Millisecond, e.Row, e.ProposedRow);
                if (!ValidateAllData(true, true))
                {
                    //Console.WriteLine("{0}:    --- Cancelled", DateTime.Now.Millisecond);
                    e.Cancel = true;
                }
            }
            FPrevLeavingFrom = e.Row;
            FPrevLeavingTo = e.ProposedRow;
            FDtPrevLeaving = DateTime.UtcNow;
        }
    }
{#ENDIF SAVEDETAILS}

    private int FPrevRowChangedRow = -1;        // Totally private to this method call
    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
        // The FocusedRowChanged event simply calls ShowDetails for the new 'current' row implied by e.Row
        // We do get a duplicate event if the user tabs round all the controls multiple times
        // There is no need to call it on duplicate events, so we just remember the previous row number we changed to.
        if (!grdDetails.Sorting && e.Row != FPrevRowChangedRow)
        {
            //Console.WriteLine("{0}:   FRC ShowDetails for {1}", DateTime.Now.Millisecond, e.Row);
            ShowDetails();
        }
        FPrevRowChangedRow = e.Row;
    }
{#DELETERECORD}
    /// <summary>
    /// Standard method to delete the Data Row whose Details are currently displayed.
    /// Optional manual code can be included to take action prior, during or after deletion.
    /// When the row has been deleted the highlighted row index stays the same unless the deleted row was the last one.
    /// The Details for the newly highlighted row are automatically displayed - or not, if the grid has now become empty.
    /// </summary>
    private void Delete{#DETAILTABLE}()
    {
        bool AllowDeletion = true;
        bool DeletionPerformed = false;
        string DeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");
        string CompletionMessage = string.Empty;
        TVerificationResultCollection VerificationResults = null;
        
        int SelectedRow = grdDetails.SelectedRowIndex();

        if ((FPreviouslySelectedDetailRow == null) || (SelectedRow == -1))
        {
            return;
        }

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
                {#DELETEROWMANUAL}
{#ENDIF DELETEROWMANUAL}
{#IFNDEF DELETEROWMANUAL}               
                FPreviouslySelectedDetailRow.Delete();
                DeletionPerformed = true;
{#ENDIFN DELETEROWMANUAL}               
            
                if (DeletionPerformed)
                {
                    FPetraUtilsObject.SetChangedFlag();
                    // Select and display the details of the nearest row to the one previously selected
                    SelectRowInGrid(SelectedRow);
                }
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
{#ENDIF SHOWDETAILS}
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

    private void GetDataFromControls(Control AControl=null)
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
            
            Label dummy;
            Control control;
            if (TControlExtensions.GetControlsForPrimaryKey(column, this, out dummy, out control))
            {
                if (control.TabIndex > lastTabIndex)
                {
                    FPrimaryKeyColumn = column;
                    FPrimaryKeyControl = control;
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

{##SNIPDELETEREFERENCECOUNT}
this.Cursor = Cursors.WaitCursor;
TRemote.{#CONNECTORNAMESPACE}.ReferenceCount.WebConnectors.GetCacheableRecordReferenceCount(
    "{#CACHEABLETABLENAME}",
    DataUtilities.GetPKValuesFromDataRow(FPreviouslySelectedDetailRow),
    out VerificationResults);
this.Cursor = Cursors.Default;
