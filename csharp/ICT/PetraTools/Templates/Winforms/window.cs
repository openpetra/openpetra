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
using Ict.Common.Verification;
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

        ShowData();
        SelectDetailRow(grdDetails.Rows.Count - 1);
        
        return true;
    }
    private void SelectDetailRow(Int32 ARowNumber)
    {
        ShowData();
        grdDetails.Selection.ResetSelection(false);
        grdDetails.Selection.SelectRow(ARowNumber, true);
        // scroll to the row
        grdDetails.ShowCell(new SourceGrid.Position(ARowNumber, 0), true);
        
        // TODO? DataGrid_FocusRowEntered(this, new RowEventArgs(ARowNumber));
    }
    {#ENDIF CANFINDWEBCONNECTOR_CREATENEWDETAIL}

    {#IFDEF SHOWDATA}
    private void ShowData()
    {
        {#SHOWDATA}
        ShowDataManual();
    }
    {#ENDIF SHOWDATA}

    {#IFDEF SHOWDATADETAILS}
    private void ShowDataDetails()
    {
        {#SHOWDATADETAILS}
        ShowDataDetailsManual();
    }
    {#ENDIF SHOWDATADETAILS}
    
    {#IFDEF SAVEDATA}
    private void GetDataFromControls()
    {
        {#SAVEDATA}
        GetDataFromControlsManual();
        {#IFDEF SAVEDETAILS}
        GetDetailDataFromControls();
        {#ENDIF SAVEDETAILS}
    }
    {#ENDIF SAVEDATA}
    
    {#IFDEF SAVEDETAILS}
    private void GetDetailDataFromControls()
    {
        {#SAVEDETAILS}
        GetDetailDataFromControlsManual();
    }

    private void ShowDetails()
    {
        {#SHOWDETAILS}
        ShowDetailsManual();
    }
    
    private void actNewDetail(Object sender, EventArgs e)
    {
        FPetraUtilsObject.SetChangedFlag();
        
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
    
{#IFDEF DATASETTYPE}
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
{#ENDIF DATASETTYPE}
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
