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
using Mono.Unix;
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
  public partial class {#CLASSNAME}: {#BASECLASSNAME}, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;

    private {#DATASETTYPE} FMainDS;

{#IFDEF TABPAGECTRL}
    private SortedList<TDynamicLoadableUserControls, UserControl> FTabSetup;       
    private event TTabPageEventHandler FTabPageEvent;
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

    /// <summary>todoComment</summary>
    public event System.EventHandler DataLoadingStarted;

    /// <summary>todoComment</summary>
    public event System.EventHandler DataLoadingFinished;

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
        {#INITMANUALCODE}
{#IFDEF ACTIONENABLING}
        FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}
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

    private void OnDataLoadingFinished()
    {
        if (DataLoadingFinished != null)
        {
            DataLoadingFinished(this, new EventArgs());
        }
    }

    private void OnDataLoadingStarted()
    {
        if (DataLoadingStarted != null)
        {
            DataLoadingStarted(this, new EventArgs());
        }
    }

    private void OnTabPageEvent(TTabPageEventArgs e)
    {
        if (FTabPageEvent != null)
        {
            FTabPageEvent(this, e);
        }
    }

    /// <summary>
    /// Dynamically loads UserControls that are associated with the Tabs. AUTO-GENERATED, don't modify by hand!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TabSelectionChanged(System.Object sender, EventArgs e)
    {
        //MessageBox.Show("TabSelectionChanged. Current Tab: " + tabPartners.SelectedTab.ToString());

        if (FTabSetup == null)
	    {
		    FTabSetup = new SortedList<TDynamicLoadableUserControls, UserControl>();

            // The first time we run this Method we exit straight away; this is when the Form gets initialised        
            return;
	    }

        {#DYNAMICTABPAGEUSERCONTROLSELECTIONCHANGED}
    }

    /// <summary>
    /// Creates UserControls on request. AUTO-GENERATED, don't modify by hand!
    /// </summary>
    /// <param name="AUserControl">UserControl to load.</param>
    private UserControl DynamicLoadUserControl(TDynamicLoadableUserControls AUserControl)
    {
        UserControl ReturnValue = null;

        switch (AUserControl)
        {
            {#DYNAMICTABPAGEUSERCONTROLLOADING}
        }

        return ReturnValue;
    }

{#ENDIF TABPAGECTRL}
  }
}

{#INCLUDE copyvalues.cs}