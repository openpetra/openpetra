// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template windowTDS
//
// DO NOT edit manually, DO NOT edit with the designer
//
{#GPLFILEHEADER}
using System;
using System.Drawing;
using System.Collections;
{#IFDEF TABPAGECTRL}
using System.Collections.Generic;
{#ENDIF TABPAGECTRL}
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
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.RemotedExceptions;
{#USINGNAMESPACES}

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

{#IFDEF TABPAGECTRL}
    private SortedList<TDynamicLoadableUserControls, UserControl> FTabSetup;       
    private event TTabPageEventHandler FTabPageEvent;
    {#DYNAMICTABPAGEUSERCONTROLDECLARATION}
    
    /// <summary>
    /// Enumeration of dynamic loadable UserControls which are used
    /// on the Tabs of a TabControl. AUTO-GENERATED, don't modify by hand!
    /// </summary>
    public enum TDynamicLoadableUserControls
    {
        {#DYNAMICTABPAGEUSERCONTROLENUM}
    }
{#ENDIF TABPAGECTRL}

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
{#IFDEF DATASETTYPE}
      FMainDS = new {#DATASETTYPE}();
{#ENDIF DATASETTYPE}
      {#INITUSERCONTROLS}
      {#INITMANUALCODE}
{#IFDEF ACTIONENABLING}
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}

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

    private void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
        {#PRIMARYKEYCONTROLSREADONLY}
    }

{#IFDEF SHOWDATA}
    private void ShowData({#MASTERTABLETYPE}Row ARow)
    {
        {#SHOWDATA}
    }
{#ENDIF SHOWDATA}

{#IFDEF SHOWDETAILS}
    private void ShowDetails({#DETAILTABLETYPE}Row ARow)
    {
        {#SHOWDETAILS}
    }
{#ENDIF SHOWDETAILS}
{#IFDEF MASTERTABLE}

    private void GetDataFromControls({#MASTERTABLETYPE}Row ARow)
    {
        {#SAVEDATA}
    }
{#ENDIF MASTERTABLE}
{#IFNDEF MASTERTABLE}

    private void GetDataFromControls()
    {
        {#SAVEDATA}
    }
{#ENDIFN MASTERTABLE}
{#IFDEF SAVEDETAILS}

    private void GetDetailsFromControls({#DETAILTABLETYPE}Row ARow)
    {
        if (ARow != null)
        {
            {#SAVEDETAILS}
        }
    }
{#ENDIF SAVEDETAILS}
    
    
    private bool ValidateAllData(bool ARecordChangeVerification)
    {
        bool ReturnValue = false;
        string ErrorMessages;
        Control FirstErrorControl;
        object FirstErrorContext;

        GetDataFromControls();

        // TODO Generate automatic validation of data, based on the DB Table specifications (e.g. 'not null' checks)
{#IFDEF VALIDATEDATAMANUAL}
        ValidateDataManual();
{#ENDIF VALIDATEDATAMANUAL}

        ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
            this.GetType());

        return ReturnValue;
    }

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
      try {
         SaveChanges();
      } catch (CancelSaveException) {}
    }

    /// <summary>
    /// save the changes on the screen
    /// </summary>
    /// <returns></returns>
    public bool SaveChanges()
    {
        bool ReturnValue = false;
        
        FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

//TODO?  still needed?      FMainDS.AApDocument.Rows[0].BeginEdit();
{#IFDEF MASTERTABLE}
        GetDataFromControls(FMainDS.{#MASTERTABLE}[0]);
{#ENDIF MASTERTABLE}
{#IFNDEF MASTERTABLE}
        GetDataFromControls();
{#ENDIFN MASTERTABLE}

        if (ValidateAllData(false))
        {
            foreach (DataTable InspectDT in FMainDS.Tables)
            {
                foreach (DataRow InspectDR in InspectDT.Rows)
                {
                    InspectDR.EndEdit();
                }
            }

            if (FPetraUtilsObject.HasChanges)
            {
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataInProgress);
                this.Cursor = Cursors.WaitCursor;

                TSubmitChangesResult SubmissionResult;
                TVerificationResultCollection VerificationResult;

                {#DATASETTYPE} SubmitDS = FMainDS.GetChangesTyped(true);

                if (SubmitDS == null)
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
{#IFDEF STOREMANUALCODE}
                    {#STOREMANUALCODE}
{#ENDIF STOREMANUALCODE}
{#IFNDEF STOREMANUALCODE}
                    SubmissionResult = {#WEBCONNECTORTDS}.Save{#SHORTDATASETTYPE}(ref SubmitDS, out VerificationResult);
{#ENDIFN STOREMANUALCODE}
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
                        FMainDS.AcceptChanges();

                        // Merge back with data from the Server (eg. for getting Sequence values)
                        FMainDS.Merge(SubmitDS, false);

                        // need to accept the new modification ID
                        FMainDS.AcceptChanges();

                        // Update UI
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataSuccessful);
                        this.Cursor = Cursors.Default;

                        // We don't have unsaved changes anymore
                        FPetraUtilsObject.DisableSaveButton();

                        SetPrimaryKeyReadOnly(true);

                        ReturnValue = true;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        break;

                    case TSubmitChangesResult.scrError:
                        this.Cursor = Cursors.Default;
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataErrorOccured);

                        MessageBox.Show(Messages.BuildMessageFromVerificationResult(null, VerificationResult));                        

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
{#IFDEF TABPAGECTRL}

        private ToolStrip PreviouslyMergedToolbarItems = null;
        private ToolStrip PreviouslyMergedMenuItems = null;
        
        /// <summary>
        /// Changes the toolbars that are associated with the Tabs.
        /// Optionally dynamically loads UserControls that are associated with the Tabs. 
        /// AUTO-GENERATED, don't modify by hand!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabSelectionChanged(System.Object sender, EventArgs e)
        {
{#IFDEF FIRSTTABPAGESELECTIONCHANGEDVAR}
            bool FirstTabPageSelectionChanged = false;
{#ENDIF FIRSTTABPAGESELECTIONCHANGEDVAR}
            TabPage currentTab = {#TABPAGECTRL}.TabPages[{#TABPAGECTRL}.SelectedIndex];
            
            if (FTabSetup == null)
            {
                FTabSetup = new SortedList<TDynamicLoadableUserControls, UserControl>();
{#IFDEF FIRSTTABPAGESELECTIONCHANGEDVAR}
                FirstTabPageSelectionChanged = true;
{#ENDIF FIRSTTABPAGESELECTIONCHANGEDVAR}
            }

            {#IGNOREFIRSTTABPAGESELECTIONCHANGEDEVENT}            
            
            {#DYNAMICTABPAGEUSERCONTROLSELECTIONCHANGED}

            
            if (PreviouslyMergedToolbarItems != null)
            {
                ToolStripManager.RevertMerge(tbrMain, PreviouslyMergedToolbarItems);
                PreviouslyMergedToolbarItems = null;
            }

            if (PreviouslyMergedMenuItems != null)
            {
                ToolStripManager.RevertMerge(mnuMain, PreviouslyMergedMenuItems);
                PreviouslyMergedMenuItems = null;
            }
            
            Control[] tabToolbar = currentTab.Controls.Find("tbrTabPage", true);
            if (tabToolbar.Length == 1)
            {
                ToolStrip ItemsToMerge = (ToolStrip) tabToolbar[0];
                ItemsToMerge.Visible = false;
                foreach (ToolStripItem item in ItemsToMerge.Items)
                {
                    item.MergeAction = MergeAction.Append;
                }
                ToolStripManager.Merge(ItemsToMerge, tbrMain);
                
                PreviouslyMergedToolbarItems = ItemsToMerge;
            }

            Control[] tabMenu = currentTab.Controls.Find("mnuTabPage", true);
            if (tabMenu.Length == 1)
            {
                ToolStrip ItemsToMerge = (ToolStrip) tabMenu[0];
                ItemsToMerge.Visible = false;
                Int32 NewPosition = mnuMain.Items.IndexOf(mniHelp);
                foreach (ToolStripItem item in ItemsToMerge.Items)
                {
                    item.MergeAction = MergeAction.Insert;
                    item.MergeIndex = NewPosition++;
                }
                ToolStripManager.Merge(ItemsToMerge, mnuMain);
                
                PreviouslyMergedMenuItems = ItemsToMerge;
            }
        }

    {#DYNAMICTABPAGEBASICS}        
{#ENDIF TABPAGECTRL}
  }
}

{#INCLUDE copyvalues.cs}

{#INCLUDE dynamictabpage_basics.cs}
{#INCLUDE dynamictabpage_usercontrol_selectionchanged.cs}
{#INCLUDE dynamictabpage_usercontrol_loading.cs}