// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template windowEditUIConnector
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

  /// auto generated: {#FORMTITLE}
  public partial class {#CLASSNAME}: System.Windows.Forms.Form, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;

    /// <summary>holds a reference to the Proxy object of the Serverside UIConnector</summary>
    private {#UICONNECTORTYPE} FUIConnector = null;

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

      {#ASSIGNFONTATTRIBUTES}
      
      FPetraUtilsObject = new {#UTILOBJECTCLASS}(AParentFormHandle, this, stbMain);
      {#INITUSERCONTROLS}
      {#INITMANUALCODE}
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
      
      {#INITACTIONSTATE}
      
      FUIConnector = {#UICONNECTORCREATE}();
      // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
      TEnsureKeepAlive.Register(FUIConnector);
    }

    {#EVENTHANDLERSIMPLEMENTATION}

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position

        if (FUIConnector != null)
        {
            // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
            TEnsureKeepAlive.UnRegister(FUIConnector);
            FUIConnector = null;
        }
    }

{#IFDEF SHOWDATA}
    private void ShowData({#MASTERTABLE}Row ARow)
    {
        {#SHOWDATA}
    }
{#ENDIF SHOWDATA}

{#IFDEF SHOWDETAILS}
    private void ShowDetails({#DETAILTABLE}Row ARow)
    {
        {#SHOWDETAILS}
    }

    private {#DETAILTABLE}Row FPreviouslySelectedDetailRow = null;
    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
{#IFDEF SAVEDETAILS}
        // get the details from the previously selected row
        if (FPreviouslySelectedDetailRow != null)
        {
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
        }
{#ENDIF SAVEDETAILS}
        // display the details of the currently selected row
        FPreviouslySelectedDetailRow = GetSelectedDetailRow();
        ShowDetails(FPreviouslySelectedDetailRow);
        pnlDetails.Enabled = true;
    }
{#ENDIF SHOWDETAILS}
    
{#IFDEF SAVEDATA}
    private void GetDataFromControls({#MASTERTABLE}Row ARow)
    {
        {#SAVEDATA}
{#IFDEF SAVEDETAILS}
        GetDetailsFromControls(FPreviouslySelectedDetailRow);
{#ENDIF SAVEDETAILS}
    }
{#ENDIF SAVEDATA}

{#IFDEF SAVEDETAILS}
    private void GetDetailsFromControls({#DETAILTABLE}Row ARow)
    {
        if (ARow != null)
        {
            ARow.BeginEdit();
            {#SAVEDETAILS}
            ARow.EndEdit();
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

    /// auto generated
    public void FileSave(object sender, EventArgs e)
    {
        SaveChanges();
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
  }
}

{#INCLUDE copyvalues.cs}