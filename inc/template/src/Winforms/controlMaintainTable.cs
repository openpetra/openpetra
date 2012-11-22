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
        grdDetails.Enter += new EventHandler(grdDetails_Enter);
        grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
      
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
            //Console.WriteLine("{0}:  UC SRIG: ShowDetails for {1}", DateTime.Now.Millisecond, ARowIndex);
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

    /// <summary>
    /// Gets the highlighted Data Row as a {#DETAILTABLE} record from the grid 
    /// </summary>
    /// <returns>The selected row - or null if no row is selected</returns>
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
                SelectRowInGrid(1);
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
    private void ShowDetails({#DETAILTABLETYPE}Row ARow)
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
            pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode && !pnlDetailsProtected;
            {#SHOWDETAILS}
        }
        FPetraUtilsObject.EnableDataChangedEvent();
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
        int nRow = grdDetails.SelectedRowIndex();       // should be the same as FPrevRowChangedRow
        if (nRow > 0)
        {
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
            ShowDetails();
        }
        FPrevRowChangedRow = e.Row;
	}

    /// <summary>
    /// Standard method to delete the Data Row whose Details are currently displayed.
    /// Optional manual code can be included to take action prior, during or after deletion.
    /// When the row has been deleted the highlighted row index stays the same unless the deleted row was the last one.
    /// The Details for the newly highlighted row are automatically displayed - or not, if the grid has now become empty.
    /// </summary>
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
		
		{#PREDELETEMANUAL}
		if(allowDeletion)
		{
        	if ((MessageBox.Show(deletionQuestion,
					 Catalog.GetString("Confirm Delete"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
			{
                int nSelectedRow = grdDetails.SelectedRowIndex();
{#IFDEF DELETEROWMANUAL}
				{#DELETEROWMANUAL}
{#ENDIF DELETEROWMANUAL}
{#IFNDEF DELETEROWMANUAL}				
				FPreviouslySelectedDetailRow.Delete();
				deletionPerformed = true;
{#ENDIFN DELETEROWMANUAL}	
			
				if (deletionPerformed)
				{
					FPetraUtilsObject.SetChangedFlag();
                    // Select and display the details of the nearest row to the one previously selected
                    SelectRowInGrid(nSelectedRow);
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
        if (FPreviouslySelectedDetailRow != null)
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
            
            int prevRowChangedRowBeforeValidation = FPrevRowChangedRow;
// :CMT:GetDetailsFromControls
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
            ValidateDataDetails(FPreviouslySelectedDetailRow);
{#IFDEF VALIDATEDATADETAILSMANUAL}
// :CMT:ValidateDataDetailsManual
			ValidateDataDetailsManual(FPreviouslySelectedDetailRow);
{#ENDIF VALIDATEDATADETAILSMANUAL}
            
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

#endregion
  }
}

{#INCLUDE copyvalues.cs}
{#INCLUDE validationcontrolsdict.cs}