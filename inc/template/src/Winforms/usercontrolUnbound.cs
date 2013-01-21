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
    /// <summary>todoComment</summary>
    public event System.EventHandler DataLoadingStarted;

    /// <summary>todoComment</summary>
    public event System.EventHandler DataLoadingFinished;

    /// to avoid warning CS0067: unused event
    private void OnDataLoadingStarted(object sender, EventArgs e)
    {
        if (DataLoadingStarted != null)
        {
            DataLoadingStarted(sender, e);
        }
    }

    /// to avoid warning CS0067: unused event
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
    }
    
    {#EVENTHANDLERSIMPLEMENTATION}


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
{#IFDEF TABPAGECTRL}
    {#DYNAMICTABPAGEBASICS}
{#ENDIF TABPAGECTRL}
  }
}

{#INCLUDE copyvalues.cs}

{#INCLUDE dynamictabpage_basics.cs}
{#INCLUDE dynamictabpage_usercontrol_selectionchanged.cs}
{#INCLUDE dynamictabpage_usercontrol_loading.cs}