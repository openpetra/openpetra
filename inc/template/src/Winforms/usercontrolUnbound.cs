// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template usercontrolUnbound
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
using Ict.Common.Remoting.Client;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
{#IFDEF FILTERANDFIND}
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;
{#ENDIF FILTERANDFIND}
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{

  /// auto generated user control
  public partial class {#CLASSNAME}: {#BASECLASSNAME}, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;

{#IFDEF DATASETTYPE}
    private {#DATASETTYPE} FMainDS;
{#ENDIF DATASETTYPE}

{#IFDEF TABPAGECTRL}
    private SortedList<TDynamicLoadableUserControls, UserControl> FTabSetup;       
    private event TTabPageEventHandler FTabPageEvent;
    private UserControl FCurrentUserControl;
    
    {#DYNAMICTABPAGEUSERCONTROLDECLARATION}
{#ENDIF TABPAGECTRL}
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
        get
        {
            return FPetraUtilsObject;
        }

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
        get
        {
            // not really needed, but helps to avoid compiler warning if FMainDS is not used anywhere else
            return FMainDS;
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

{#IFDEF TABPAGECTRL}

    /// <summary>
    /// Enumeration of dynamic loadable UserControls which are used
    /// on the Tabs of a TabControl. AUTO-GENERATED, don't modify by hand!
    /// </summary>
    public enum TDynamicLoadableUserControls
    {
        {#DYNAMICTABPAGEUSERCONTROLENUM}
    }
{#ENDIF TABPAGECTRL}

    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
        {#INITUSERCONTROLS}
{#IFDEF ACTIONENABLING}
        FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}
        {#INITMANUALCODE}
{#IFDEF FILTERANDFIND}
        SetupFilterAndFindControls();
{#ENDIF FILTERANDFIND}
    }
    
    {#EVENTHANDLERSIMPLEMENTATION}


#region Implement interface functions
    /// auto generated
    public void RunOnceOnActivation()
    {
        {#RUNONCEINTERFACEIMPLEMENTATION}
    }

    /// auto generated
    public void RunOnceOnParentActivation()
    {
{#IFDEF FILTERANDFIND}
        if (FFilterAndFindParameters.FindAndFilterInitiallyExpanded)
        {
            FFilterPanelControls.InitialiseComboBoxes();
            FFindPanelControls.InitialiseComboBoxes();
        }
{#ENDIF FILTERANDFIND}    
        {#RUNONCEONPARENTACTIVATIONMANUAL}    
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
{#IFDEF FILTERANDFIND}

#region Filter and Find
    {#FILTERANDFINDMETHODS}

    // Dummy method that prevents build message of 'declared but never used'
    private void ControlValidatedHandler(object sender, EventArgs e)
    {
        if (FFailedValidation_CtrlChangeEventArgsInfo != null)
        {
            FFailedValidation_CtrlChangeEventArgsInfo = null;
        }
    }
#endregion
{#ENDIF FILTERANDFIND}    
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
    {#DYNAMICTABPAGEBASICS}
{#ENDIF TABPAGECTRL}

#region Keyboard handler

    /// Our main keyboard handler
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
{#IFDEF FILTERANDFIND}
        {#PROCESSCMDKEYCTRLF}
        {#PROCESSCMDKEYCTRLR}
{#ENDIF FILTERANDFIND}
        {#PROCESSCMDKEY}    
        {#PROCESSCMDKEYMANUAL}    

        return base.ProcessCmdKey(ref msg, keyData);
    }

#endregion

  }
}

{#INCLUDE copyvalues.cs}
{#INCLUDE findandfilter.cs}

{#INCLUDE dynamictabpage_basics.cs}
{#INCLUDE dynamictabpage_usercontrol_selectionchanged.cs}
{#INCLUDE dynamictabpage_usercontrol_loading.cs}