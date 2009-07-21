/* auto generated with nant generateWinforms from {#XAMLSRCFILE} 
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
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
{#IFDEF ISEDITSCREEN}
using Ict.Common.Verification;
{#ENDIF ISEDITSCREEN}
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace {#NAMESPACE}
{

  /// auto generated: {#FORMTITLE}
  public partial class {#CLASSNAME}: System.Windows.Forms.Form, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;
{#IFDEF DATASETTYPE}
    private {#DATASETTYPE} FMainDS;
{#ENDIF DATASETTYPE}
    
{#IFDEF UICONNECTORTYPE}

    /// <summary>holds a reference to the Proxy object of the Serverside UIConnector</summary>
    private {#UICONNECTORTYPE} FUIConnector = null;
{#ENDIF UICONNECTORTYPE}

    /// constructor
    public {#CLASSNAME}(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      {#CATALOGI18N}
      #endregion

      FPetraUtilsObject = new {#UTILOBJECTCLASS}(AParentFormHandle, this, stbMain);
      {#INITUSERCONTROLS}
{#IFDEF DATASETTYPE}
      FMainDS = new {#DATASETTYPE}();
{#ENDIF DATASETTYPE}
      {#INITMANUALCODE}
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
      
      {#INITACTIONSTATE}
      
{#IFDEF UICONNECTORCREATE}
      FUIConnector = {#UICONNECTORCREATE}();
      // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
      TEnsureKeepAlive.Register(FUIConnector);
{#ENDIF UICONNECTORCREATE}
    }

    {#EVENTHANDLERSIMPLEMENTATION}

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position

{#IFDEF UICONNECTORCREATE}
        if (FUIConnector != null)
        {
            // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
            TEnsureKeepAlive.UnRegister(FUIConnector);
            FUIConnector = null;
        }
{#ENDIF UICONNECTORCREATE}
    }

{#IFDEF ISEDITSCREEN}
{#IFDEF CANFINDWEBCONNECTOR_CREATENEWMASTER}
    /// automatically generated function from webconnector
    public bool CreateNew{#MASTERTABLE}({#CREATENEWMASTER_FORMALPARAMETERS})
    {
{#IFDEF CREATENEWMASTER_WITHVERIFICATION}
        TVerificationResultCollection VerificationResult;

        FMainDS = {#WEBCONNECTORMASTER}.CreateNew{#MASTERTABLE}({#CREATENEWMASTER_ACTUALPARAMETERS}, out VerificationResult);

        if (VerificationResult != null && VerificationResult.Count > 0)
        {
            return CreateNewMasterManual({#CREATENEWMASTER_ACTUALPARAMETERS}, VerificationResult);
        }
        else
        {
            FPetraUtilsObject.SetChangedFlag();

            ShowData();
            
            return true;
        }
{#ENDIF CREATENEWMASTER_WITHVERIFICATION}
{#IFDEF CREATENEWMASTER_WITHOUTVERIFICATION}
        FMainDS = {#WEBCONNECTORMASTER}.CreateNew{#MASTERTABLE}({#CREATENEWMASTER_ACTUALPARAMETERS});

        FPetraUtilsObject.SetChangedFlag();

        ShowData();
        
        return true;
{#ENDIF CREATENEWMASTER_WITHOUTVERIFICATION}
    }
{#ENDIF CANFINDWEBCONNECTOR_CREATENEWMASTER}

{#IFDEF CANFINDWEBCONNECTOR_CREATENEWDETAIL}
    /// automatically generated, create a new record of {#DETAILTABLE} and display on the edit screen
    public bool CreateNew{#DETAILTABLE}({#CREATENEWDETAIL_FORMALPARAMETERS})
    {
        FMainDS.Merge({#WEBCONNECTORDETAIL}.CreateNew{#DETAILTABLE}({#CREATENEWDETAIL_ACTUALPARAMETERS}));

        FPetraUtilsObject.SetChangedFlag();

        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.{#DETAILTABLE}.DefaultView);
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);
        
        return true;
    }
{#ENDIF CANFINDWEBCONNECTOR_CREATENEWDETAIL}
{#IFDEF DETAILTABLE}

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
{#ENDIF DETAILTABLE}
{#ENDIF ISEDITSCREEN}

{#IFDEF CANFINDWEBCONNECTOR_LOADMASTER}

    /// automatically generated function from webconnector
    public bool Load{#MASTERTABLE}({#LOADMASTER_FORMALPARAMETERS})
    {
        FMainDS.Merge({#WEBCONNECTORMASTER}.Load{#MASTERTABLE}({#LOADMASTER_ACTUALPARAMETERS}));

        ShowData();
        
        return true;
    }
{#ENDIF CANFINDWEBCONNECTOR_LOADMASTER}

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
    
{#IFDEF SAVEDATA}
    private void GetDataFromControls()
    {
        {#SAVEDATA}
{#IFDEF SAVEDETAILS}
        GetDetailsFromControls(GetSelectedDetailDataTableIndex());
{#ENDIF SAVEDETAILS}
    }
{#ENDIF SAVEDATA}

{#IFDEF SAVEDETAILS}
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
{#IFDEF ISEDITSCREEN}

    /// auto generated
    public void FileSave(object sender, EventArgs e)
    {
        SaveChanges(FMainDS);
    }

    /// auto generated
    public bool SaveChanges()
    {
        return SaveChanges(FMainDS);
    }
{#ENDIF ISEDITSCREEN}
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
  }
}
