/* auto generated with nant generateWinforms from {#XAMLSRCFILE} and template controlMaintainTable
 *
 * DO NOT edit manually, DO NOT edit with the designer
 *
 */
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
using Mono.Unix;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace {#NAMESPACE}
{

  /// auto generated user control
  public partial class {#CLASSNAME}: System.Windows.Forms.UserControl, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;

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
      
      DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
      grdDetails.AutoSizeCells();

      ShowData();
    }
    
    {#EVENTHANDLERSIMPLEMENTATION}

    /// automatically generated, create a new record of {#DETAILTABLE} and display on the edit screen
    public bool CreateNew{#DETAILTABLE}()
    {
{#IFNDEF CANFINDWEBCONNECTOR_CREATEDETAIL}
        // we create the table locally, no dataset
        {#DATATABLETYPE}Row NewRow = FMainDS.{#DETAILTABLE}.NewRowTyped(true);
        {#INITNEWROWMANUAL}
        FMainDS.{#DETAILTABLE}.Rows.Add(NewRow);
{#ENDIFN CANFINDWEBCONNECTOR_CREATEDETAIL}
{#IFDEF CANFINDWEBCONNECTOR_CREATEDETAIL}
        FMainDS.Merge({#WEBCONNECTORDETAIL}.Create{#DETAILTABLE}({#CREATEDETAIL_ACTUALPARAMETERS_LOCAL}));
{#ENDIF CANFINDWEBCONNECTOR_CREATEDETAIL}

        FPetraUtilsObject.SetChangedFlag();

        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.{#DETAILTABLE}.DefaultView);
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);

        return true;
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
                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).mDataView[Counter][myColumn.Ordinal].ToString();
                if (value1 != value2)
                {
                    found = false;
                }
            }
            if (found)
            {
                RowNumberGrid = Counter + 1;
            }
        }
        grdDetails.Selection.ResetSelection(false);
        grdDetails.Selection.SelectRow(RowNumberGrid, true);
        // scroll to the row
        grdDetails.ShowCell(new SourceGrid.Position(RowNumberGrid, 0), true);

        FocusedRowChanged(this, new SourceGrid.RowEventArgs(RowNumberGrid));
    }

    /// return the index in the detail datatable of the selected row, not the index in the datagrid
    private Int32 GetSelectedDetailDataTableIndex()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            // this would return the index in the grid: return grdDetails.DataSource.IndexOf(SelectedGridRow[0]);
            // we could keep track of the order in the datatable ourselves: return Convert.ToInt32(SelectedGridRow[0][ORIGINALINDEX]);
            // does not seem to work: return grdDetails.DataSourceRowToIndex2(SelectedGridRow[0]);

            for (int Counter = 0; Counter < FMainDS.{#DETAILTABLE}.Rows.Count; Counter++)
            {
                bool found = true;
                foreach (DataColumn myColumn in FMainDS.{#DETAILTABLE}.PrimaryKey)
                {
                    if (FMainDS.{#DETAILTABLE}.Rows[Counter][myColumn].ToString() != 
                        SelectedGridRow[0][myColumn.Ordinal].ToString())
                    {
                        found = false;
                    }
                    
                }
                if (found)
                {
                    return Counter;
                }
            }
        }

        return -1;
    }

{#IFDEF SHOWDATA}
    private void ShowData()
    {
        {#SHOWDATA}
    }
{#ENDIF SHOWDATA}

{#IFDEF SHOWDETAILS}
    private void ShowDetails(Int32 ACurrentDetailIndex)
    {
        {#SHOWDETAILS}
    }

    private Int32 FPreviouslySelectedDetailRow = -1;
    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
{#IFDEF SAVEDETAILS}
        // get the details from the previously selected row
        if (FPreviouslySelectedDetailRow != -1)
        {
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
        }
{#ENDIF SAVEDETAILS}
        // display the details of the currently selected row; e.Row: first row has number 1
        ShowDetails(GetSelectedDetailDataTableIndex());
        FPreviouslySelectedDetailRow = GetSelectedDetailDataTableIndex();
        pnlDetails.Enabled = true;
    }
{#ENDIF SHOWDETAILS}
    
{#IFDEF SAVEDETAILS}
    /// get the data from the controls and store in the currently selected detail row
    public void GetDataFromControls()
    {
        GetDetailsFromControls(GetSelectedDetailDataTableIndex());
    }

    private void GetDetailsFromControls(Int32 ACurrentDetailIndex)
    {
        if (ACurrentDetailIndex != -1)
        {
            {#SAVEDETAILS}
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
