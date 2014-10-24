{##DYNAMICTABPAGE}
private void OnTabPageEvent(TTabPageEventArgs e)
{
    if (FTabPageEvent != null)
    {
        FTabPageEvent(this, e);
    }
}

{#IFDEF ISUSERCONTROL}
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

/// <summary>
/// Dynamically loads UserControls that are associated with the Tabs. AUTO-GENERATED, don't modify by hand!
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void TabSelectionChanged(System.Object sender, EventArgs e)
{
{#IFDEF FIRSTTABPAGESELECTIONCHANGEDVAR}
    bool FirstTabPageSelectionChanged = false;
{#ENDIF FIRSTTABPAGESELECTIONCHANGEDVAR}
    //MessageBox.Show("TabSelectionChanged. Current Tab: " + tabPartners.SelectedTab.ToString());

    if (FTabSetup == null)
    {
        FTabSetup = new SortedList<TDynamicLoadableUserControls, UserControl>();
{#IFDEF FIRSTTABPAGESELECTIONCHANGEDVAR}
        FirstTabPageSelectionChanged = true;
{#ENDIF FIRSTTABPAGESELECTIONCHANGEDVAR}
    }

    {#IGNOREFIRSTTABPAGESELECTIONCHANGEDEVENT}
    
    {#DYNAMICTABPAGEUSERCONTROLSELECTIONCHANGED}

    // Ensure that a Validation ToolTip is removed when the user switches to another Tab
    FPetraUtilsObject.ValidationToolTip.RemoveAll();
}

/// <summary>
/// This Method *CAN* be implemented in ManualCode to perform special initialisations *before* 
/// InitUserControl() gets called.
/// </summary>
/// <param name="AUserControl"></param>
partial void PreInitUserControl(UserControl AUserControl);

/// <summary>
/// This Method *CAN* be implemented in ManualCode to perform special initialisations *after*
/// InitUserControl() gets called.
/// </summary>
/// <param name="AUserControl"></param>
partial void PostInitUserControl(UserControl AUserControl);

{#ENDIF ISUSERCONTROL}
{#DYNAMICTABPAGEUSERCONTROLSETUPMETHODS}

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

{#INCLUDE dynamictabpage_usercontrol_setup.cs}
{#INCLUDE dynamictabpage_usercontrol_setup_method.cs}