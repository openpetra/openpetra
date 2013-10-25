// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template controlMaintainTableWithDataViewGrid
//
// DO NOT edit manually, DO NOT edit with the designer
//
{#GPLFILEHEADER}
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{

  /// auto generated user control
  public partial class {#CLASSNAME}: System.Windows.Forms.UserControl, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;

    private {#DATASETTYPE} FMainDS;
{#IFDEF SHOWDETAILS}
    private int FCurrentRow;
{#ENDIF SHOWDETAILS}
{#IFDEF FILTERANDFIND}
    {#FILTERANDFINDDECLARATIONS}
{#ENDIF FILTERANDFIND}

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
      
      DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = myDataView;
      grdDetails.Columns.AutoSizeMode = Fill;

{#IFDEF BUTTONPANEL}
      FinishButtonPanelSetup();
{#ENDIF BUTTONPANEL}
{#IFDEF FILTERANDFIND}
      SetupFilterAndFindControls();
{#ENDIF FILTERANDFIND}
      ShowData();
    }
    
    {#EVENTHANDLERSIMPLEMENTATION}

    /// automatically generated, create a new record of {#DETAILTABLE} and display on the edit screen
    public bool CreateNew{#DETAILTABLE}()
    {
        int previousGridRow = grdDetails.Selection.ActivePosition.Row;
{#IFNDEF CANFINDWEBCONNECTOR_CREATEDETAIL}
        // we create the table locally, no dataset
        {#DETAILTABLE}Row NewRow = FMainDS.{#DETAILTABLE}.NewRowTyped(true);
        {#INITNEWROWMANUAL}
        FMainDS.{#DETAILTABLE}.Rows.Add(NewRow);
{#ENDIFN CANFINDWEBCONNECTOR_CREATEDETAIL}
{#IFDEF CANFINDWEBCONNECTOR_CREATEDETAIL}
        FMainDS.Merge({#WEBCONNECTORDETAIL}.Create{#DETAILTABLE}({#CREATEDETAIL_ACTUALPARAMETERS_LOCAL}));
{#ENDIF CANFINDWEBCONNECTOR_CREATEDETAIL}

        FPetraUtilsObject.SetChangedFlag();

        grdDetails.DataSource = FMainDS.{#DETAILTABLE}.DefaultView;
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);
        int currentGridRow = grdDetails.Selection.ActivePosition.Row;
        if (currentGridRow == previousGridRow)
        {
            // The grid must be sorted so the new row is displayed where the old one was.  We will not have received a RowChanged event.
            // We need to enforce showing the new details.
            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
            ShowDetails(FPreviouslySelectedDetailRow);
        }

        return true;
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < ((DataView)grdDetails.DataSource).Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.{#DETAILTABLE}.PrimaryKey)
            {
                string value1 = FMainDS.{#DETAILTABLE}.Rows[ARowNumberInTable][myColumn].ToString();
                string value2 = ((DataView)grdDetails.DataSource)[Counter][myColumn.Ordinal].ToString();
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

        grdDetails.SelectRowInGrid(RowNumberGrid);
    }

    /// return the selected row
    private {#DETAILTABLE}Row GetSelectedDetailRow()
    {
        DataGridViewSelectedRowCollection SelectedGridRow = grdDetails.SelectedRows;

        if (SelectedGridRow.Count >= 1)
        {
            return ({#DETAILTABLE}Row)((DataRowView)SelectedGridRow[0].DataBoundItem).Row;;
        }

        return null;
    }

    private void ShowData()
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        {#SHOWDATA}
        pnlDetails.Enabled = false;
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
            grdDetails.DataSource = myDataView;
            grdDetails.Columns.AutoSizeMode = Fill;
            if (myDataView.Count > 0)
            {
                grdDetails.ClearSelection();
                grdDetails.Rows[0].Selected = true;
                FocusedRowChanged(this, new SourceGrid.RowEventArgs(1));
                pnlDetails.Enabled = true;
            }
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

{#IFDEF SHOWDETAILS}
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
            pnlDetails.Enabled = true;
            FPreviouslySelectedDetailRow = ARow;
            {#SHOWDETAILS}
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private {#DETAILTABLE}Row FPreviouslySelectedDetailRow = null;
    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
        if(e.Row != FCurrentRow && !grdDetails.Sorting)
        {       
{#IFDEF SAVEDETAILS}
            // Transfer data from Controls into the DataTable
            if (FPreviouslySelectedDetailRow != null)
            {
                GetDetailsFromControls(FPreviouslySelectedDetailRow);
            }
{#ENDIF SAVEDETAILS}

            // Display the details of the currently selected Row
            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
            ShowDetails(FPreviouslySelectedDetailRow);
            pnlDetails.Enabled = true;
        }
        FCurrentRow = e.Row;
    }
{#ENDIF SHOWDETAILS}
    
{#IFDEF SAVEDETAILS}
    /// get the data from the controls and store in the currently selected detail row
    public void GetDataFromControls()
    {
        GetDetailsFromControls(FPreviouslySelectedDetailRow);
    }

    private void GetDetailsFromControls({#DETAILTABLE}Row ARow, Control AControl=null)
    {
        if (ARow != null && !grdDetails.Sorting)
        {
            ARow.BeginEdit();
            {#SAVEDETAILS}
            ARow.EndEdit();
        }
    }
{#IFDEF GENERATECONTROLUPDATEDATAHANDLER}

    private void ControlUpdateDataHandler(object sender, EventArgs e)
    {
        GetDetailsFromControls(FPreviouslySelectedDetailRow, (Control)sender);
    }
{#ENDIF GENERATECONTROLUPDATEDATAHANDLER}
{#ENDIF SAVEDETAILS}

{#IFDEF BUTTONPANEL}
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
            lblRecordCounter.Text = String.Format(Catalog.GetPluralString("{0} record", "{0} records", RecordCount, true), RecordCount);
        }                
    }
{#ENDIF BUTTONPANEL}

{#IFDEF FILTERANDFIND}
    {#FILTERANDFINDMETHODS}
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
{#INCLUDE findandfilter.cs}