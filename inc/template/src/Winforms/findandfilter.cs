{##FILTERANDFINDDECLARATIONS}
#region Filter and Find

TUcoFilterAndFind FucoFilterAndFind = null;
TUcoFilterAndFind.FilterAndFindParameters FFilterAndFindParameters;
ImageList FFilterImages;

#endregion

{##FILTERANDFINDMETHODS}
/// <summary>
/// This class stores the event arguments that applied to an event handler that could not be completed because of invalid data.
/// When the data becomes valid the event can be 'replayed' using these stored arguments.
/// </summary>
private class TEventArgsInfo
{
    private object FSender = null;
    private EventArgs FEventArgs = null;

    /// <summary>
    /// Constructor that specifies the arguments that cannot be applied because of invalid data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public TEventArgsInfo(object sender, EventArgs e)
    {
        FSender = sender;
        FEventArgs = e;
    }

    /// <summary>
    /// Gets the sender
    /// </summary>
    public object Sender
    {
        get
        {
            return FSender;
        }
    }

    /// <summary>
    /// Gets the context
    /// </summary>
    public EventArgs EventArgs
    {
        get
        {
            return FEventArgs;
        }
    }
}

private TFilterPanelControls FFilterPanelControls = new TFilterPanelControls();
private TFindPanelControls FFindPanelControls = new TFindPanelControls();
private bool FIsFilterFindInitialised = false;
private bool FClearingDiscretionaryFilters = false;
private TEventArgsInfo FFailedValidation_CtrlChangeEventArgsInfo = null;
private string FCurrentActiveFilter = String.Empty;
private string FPreviousFilterTooltip = String.Format("{0}{1}{2}", CommonFormsResourcestrings.StrFilterIsHidden, Environment.NewLine, CommonFormsResourcestrings.StrFilterClickToTurnOn);

///<summary>
/// Sets the image, font and color properties for the record counter label based on the current active filter string
/// </summary>
private void SetRecordNumberDisplayProperties()
{
    if (grdDetails.DataSource != null)
    {
        string recordsString;

        if (lblRecordCounter.Font.Italic)
        {
            // Do we need to change from 'filtered'?
            if ((FCurrentActiveFilter == FFilterPanelControls.BaseFilter) && FFilterPanelControls.BaseFilterShowsAllRecords)
            {
                // No filtering
                lblRecordCounter.ForeColor = System.Drawing.Color.SlateGray;
                lblRecordCounter.Font = new Font(lblRecordCounter.Font, FontStyle.Regular);
                chkToggleFilter.Image = FFilterImages.Images[0]; // 'Filter is inactive' icon
                recordsString = CommonFormsResourcestrings.StrFilterAllRecordsShown;
            }
            else
            {
                recordsString = CommonFormsResourcestrings.StrFilterSomeRecordsHidden;
            }
        }
        else
        {
            // Do we need to change from 'not filtered'?
            if ((FCurrentActiveFilter != FFilterPanelControls.BaseFilter) || !FFilterPanelControls.BaseFilterShowsAllRecords)
            {
                // Now we are filtering
                lblRecordCounter.ForeColor = System.Drawing.Color.MidnightBlue;
                lblRecordCounter.Font = new Font(lblRecordCounter.Font, FontStyle.Italic);
                chkToggleFilter.Image = FFilterImages.Images[1];  // 'Filter is active' icon
                recordsString = CommonFormsResourcestrings.StrFilterSomeRecordsHidden;
            }
            else
            {
                recordsString = CommonFormsResourcestrings.StrFilterAllRecordsShown;
            }
        }

        string clickString = (pnlFilterAndFind.Width > 0) ? CommonFormsResourcestrings.StrFilterClickToTurnOff : CommonFormsResourcestrings.StrFilterClickToTurnOn;
        string strToolTip = String.Format("{0}{1}{2}", recordsString, Environment.NewLine, clickString);

        if (strToolTip != FPreviousFilterTooltip)
        {
            GetPetraUtilsObject().SetToolTip(chkToggleFilter, strToolTip);
            FPreviousFilterTooltip = strToolTip;
        }
    }
}

///<summary>
/// Sets up the Filter Button and the Filter and Find UserControl.
/// </summary>
private void SetupFilterAndFindControls()
{
    LoadFilterIcons();

    // Further set up certain Controls Properties that can't be set directly in the WinForms Generator...
    chkToggleFilter.AutoSize = true;
    chkToggleFilter.Text = MCommonResourcestrings.StrBtnTextFilter;
    chkToggleFilter.Tag = MCommonResourcestrings.StrCtrlSuppressChangeDetection;
    chkToggleFilter.Image = FFilterImages.Images[0];  // 'Filter is inactive' icon
    chkToggleFilter.ImageAlign = ContentAlignment.MiddleLeft;
    chkToggleFilter.Appearance = Appearance.Button;
    chkToggleFilter.TextAlign = ContentAlignment.MiddleCenter;  // Same as 'real' Button
    chkToggleFilter.MinimumSize = new Size(75, 22);             // To prevent shrinkage!
    chkToggleFilter.Click += new EventHandler(this.ToggleFilterPanel);
    
    GetPetraUtilsObject().SetToolTip(chkToggleFilter, FPreviousFilterTooltip);
    GetPetraUtilsObject().SetStatusBarText(chkToggleFilter, MCommonResourcestrings.StrClickToShowHideFilterPanel);
                                     
    // Prepare parameters for the UserControl that will display the Filter and Find Panels
    FFilterAndFindParameters = new TUcoFilterAndFind.FilterAndFindParameters(
        {#FINDANDFILTERINITIALWIDTH}, {#FINDANDFILTERINITIALLYEXPANDED},
        {#FINDANDFILTERAPPLYFILTERBUTTONCONTEXT}, {#FINDANDFILTERSHOWKEEPFILTERTURNEDONBUTTONCONTEXT}, 
        {#FINDANDFILTERSHOWFILTERISALWAYSONLABELCONTEXT});

    // Show Filter and Find Panels initially collapsed or expanded
    pnlFilterAndFind.Width = 0;
    
    // Ensure that the Filter and Find Panel 'pushes' the Grid away instead of overlaying the Grid
    grdDetails.BringToFront();
    
    if (FFilterAndFindParameters.FindAndFilterInitiallyExpanded)
    {
        ToggleFilter();
    }
}

private void LoadFilterIcons()
{
    const string FILENAME_FILTER_ICON = "Filter.ico";
    const string FILENAME_FILTER_ACTIVEICON = "FilterActive.ico";
    string ResourceDirectory;

    if (FFilterImages == null)
    {
        FFilterImages = new ImageList();

        ResourceDirectory = TAppSettingsManager.GetValue("Resource.Dir");

        if (System.IO.File.Exists(ResourceDirectory + System.IO.Path.DirectorySeparatorChar + FILENAME_FILTER_ICON))
        {
            FFilterImages.Images.Add(FILENAME_FILTER_ICON, Image.FromFile(ResourceDirectory + System.IO.Path.DirectorySeparatorChar + FILENAME_FILTER_ICON));
        }
        else
        {
            MessageBox.Show("LoadFilterIcons: cannot find file " + ResourceDirectory + System.IO.Path.DirectorySeparatorChar + FILENAME_FILTER_ICON);
        }

        if (System.IO.File.Exists(ResourceDirectory + System.IO.Path.DirectorySeparatorChar + FILENAME_FILTER_ACTIVEICON))
        {
            FFilterImages.Images.Add(FILENAME_FILTER_ICON, Image.FromFile(ResourceDirectory + System.IO.Path.DirectorySeparatorChar + FILENAME_FILTER_ACTIVEICON));
        }
        else
        {
            MessageBox.Show("LoadFilterIcons: cannot find file " + ResourceDirectory + System.IO.Path.DirectorySeparatorChar + FILENAME_FILTER_ACTIVEICON);
        }
    }
}

private void CreateFilterFindPanels()
{
    TIndividualFilterFindPanel iffp;

    // Standard Filter Panel
    {#INDIVIDUALFILTERPANELS}

    // Extra Filter Panel
    {#INDIVIDUALEXTRAFILTERPANELS}
        
    // Find Panel
    {#INDIVIDUALFINDPANELS}

    // Filter/Find Panel Events
    {#INDIVIDUALFILTERFINDPANELEVENTS}
        
    // Filter/Find Panel Properties
    {#INDIVIDUALFILTERFINDPANELPROPERTIES}
}

private void SetStatusBarText()
{
    Control[] button = FucoFilterAndFind.Controls.Find("btnCloseFilter", true);
    if (button.Length > 0)
    {
        GetPetraUtilsObject().SetStatusBarText(button[0], MCommonResourcestrings.StrClickToHideFilterPanel);
    }

    button = FucoFilterAndFind.Controls.Find("btnFindNext", true);
    if (button.Length > 0)
    {
        GetPetraUtilsObject().SetStatusBarText(button[0], MCommonResourcestrings.StrClickToFindNextRecord);
    }

    button = FucoFilterAndFind.Controls.Find("rbtFindDirUp", true);
    if (button.Length > 0)
    {
        GetPetraUtilsObject().SetStatusBarText(button[0], MCommonResourcestrings.StrUpDownFindDirection);
    }

    button = FucoFilterAndFind.Controls.Find("rbtFindDirDown", true);
    if (button.Length > 0)
    {
        GetPetraUtilsObject().SetStatusBarText(button[0], MCommonResourcestrings.StrUpDownFindDirection);
    }

    if (FucoFilterAndFind.FilterPanelControls != null)
    {
        foreach (Panel panel in FucoFilterAndFind.FilterPanelControls)
        {
            foreach (Control c in panel.Controls)
            {
                if (c.Name.StartsWith("btnClearArgument_"))
                {
                    GetPetraUtilsObject().SetStatusBarText(c, MCommonResourcestrings.StrClickToClearFilterAttribute);
                }
            }
        }
    }

    if (FucoFilterAndFind.ExtraFilterPanelControls != null)
    {
        foreach (Panel panel in FucoFilterAndFind.ExtraFilterPanelControls)
        {
            foreach (Control c in panel.Controls)
            {
                if (c.Name.StartsWith("btnClearArgument_"))
                {
                    GetPetraUtilsObject().SetStatusBarText(c, MCommonResourcestrings.StrClickToClearFilterAttribute);
                }
            }
        }
    }

    if (FucoFilterAndFind.FindPanelControls != null)
    {
        foreach (Panel panel in FucoFilterAndFind.FindPanelControls)
        {
            foreach (Control c in panel.Controls)
            {
                if (c.Name.StartsWith("btnClearArgument_"))
                {
                    GetPetraUtilsObject().SetStatusBarText(c, MCommonResourcestrings.StrClickToClearFindAttribute);
                }
            }
        }
    }
}

private void ToggleFilterPanel(System.Object sender, EventArgs e)
{
    ToggleFilter();
        
    // If we have errors we need to focus the invalid control
    if (FPetraUtilsObject.VerificationResultCollection != null && FPetraUtilsObject.VerificationResultCollection.Count > 0)
    {
        ((TScreenVerificationResult)FPetraUtilsObject.VerificationResultCollection[0]).ResultControl.Focus();
    }
    else if (!FucoFilterAndFind.IsCollapsed)
    {
        // No errors so we can focus the first filter/find panel
        if (FucoFilterAndFind.IsFindTabActive)
        {
            FFindPanelControls.FFindPanels[0].PanelControl.Focus();
        }
        else
        {
            FFilterPanelControls.FStandardFilterPanels[0].PanelControl.Focus();
        }
    }
}

private void ToggleFilter()
{
    if (pnlFilterAndFind.Width == 0)
    {
        // Create the Filter and Find UserControl on-the-fly (loading the UserControl only when is is shown so that the screen can load faster!)
        if (FucoFilterAndFind == null)
        {
            // Add columns to data table where filtering is numeric
            {#NUMERICFILTERFINDCOLUMNS}

            // Create the individual panels from the YAML
            CreateFilterFindPanels();
            {#CREATEFILTERFINDPANELSMANUAL}

            // Construct a complete new Filter/Find Panel
            FucoFilterAndFind = new TUcoFilterAndFind(
                FFilterPanelControls.GetFilterPanels(),
                FFilterPanelControls.GetExtraFilterPanels(),
                FFindPanelControls.GetFindPanels(),
                FFilterAndFindParameters);

            FucoFilterAndFind.Dock = DockStyle.Fill;
            pnlFilterAndFind.Controls.Add(FucoFilterAndFind);

            FucoFilterAndFind.Expanded += delegate { ToggleFilter(); };
            FucoFilterAndFind.Collapsed += delegate { ToggleFilter(); };
    
            FucoFilterAndFind.ApplyFilterClicked += new EventHandler<TUcoFilterAndFind.TContextEventExtControlArgs>(FucoFilterAndFind_ApplyFilterClicked);
            FucoFilterAndFind.ArgumentCtrlValueChanged += new EventHandler<TUcoFilterAndFind.TContextEventExtControlValueArgs>(FucoFilterAndFind_ArgumentCtrlValueChanged);
            FucoFilterAndFind.FindNextClicked += new EventHandler<TUcoFilterAndFind.TContextEventExtSearchDirectionArgs>(FucoFilterAndFind_FindNextClicked);

            SetStatusBarText();
        }

        pnlFilterAndFind.Width = FFilterAndFindParameters.FindAndFilterInitialWidth;
        chkToggleFilter.Checked = true;
        {#FILTERTOGGLEDMANUAL}

        FucoFilterAndFind_ArgumentCtrlValueChanged(FucoFilterAndFind, new TUcoFilterAndFind.TContextEventExtControlValueArgs(TUcoFilterAndFind.EventContext.ecFilterTab, null, null, null));
    }
    else
    {
        // Collapse the filter panel and uncheck the button if there is no active filter
        pnlFilterAndFind.Width = 0;
        {#FILTERTOGGLEDMANUAL}
        FCurrentActiveFilter = FFilterPanelControls.GetCurrentFilter(
            true,
            FucoFilterAndFind.KeepFilterTurnedOnButtonDepressed,
            FucoFilterAndFind.ShowFilterIsAlwaysOnLabel);

        ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView.RowFilter = FCurrentActiveFilter; 

        chkToggleFilter.Checked = false;
    }       

    UpdateRecordNumberDisplay();
    SetRecordNumberDisplayProperties();
    SelectRowInGrid(FPrevRowChangedRow);
    pnlFilterAndFind.Visible = pnlFilterAndFind.Width > 0;
}

void FucoFilterAndFind_FindNextClicked(object sender, TUcoFilterAndFind.TContextEventExtSearchDirectionArgs e)
{
    if (e.Context == TUcoFilterAndFind.EventContext.ecFindPanel)
    {
        if (e.SearchUpwards)
        {
            for (int rowNum = FPrevRowChangedRow - 1; rowNum > 0; rowNum--)
            {
                DataRowView rowView = (DataRowView)grdDetails.Rows.IndexToDataSourceRow(rowNum);
                if ({#ISMATCHINGROW}(rowView.Row))
                {
                    SelectRowInGrid(rowNum);
                    if (sender != null)
                    {
                        ((Control)sender).Focus();
                    }
                    return;
                }
            }
        }
        else
        {
            for (int rowNum = Math.Max(2, FPrevRowChangedRow + 1); rowNum < grdDetails.Rows.Count; rowNum++)
            {
                DataRowView rowView = (DataRowView)grdDetails.Rows.IndexToDataSourceRow(rowNum);
                if ({#ISMATCHINGROW}(rowView.Row))
                {
                    SelectRowInGrid(rowNum);
                    if (sender != null)
                    {
                        ((Control)sender).Focus();
                    }
                    return;
                }
            }
        }
    }
}

void FucoFilterAndFind_ArgumentCtrlValueChanged(object sender, TUcoFilterAndFind.TContextEventExtControlValueArgs e)
{
    if (e.Context == TUcoFilterAndFind.EventContext.ecFindPanel)
    {
        // No need for any dynamic response on the Find panel
        return;
    }

    // Something has changed on the filter/find panel
    // If this is the first activation we need to initialise combo boxes because they will now have been populated for the first time
    if ((sender is TUcoFilterAndFind) && !FIsFilterFindInitialised)
    {
        // First time of diaplaying the panel(s)
        // we need to initialise all the combo boxes to their clear value
        FFilterPanelControls.InitialiseComboBoxes();
        FFindPanelControls.InitialiseComboBoxes();
        FIsFilterFindInitialised = true;
    }

    // We need to call ValidateAllData before applying the new filter,
    //   but not if it was ValidateAllData that is causing the argumentCtrlValue change by clearing the discretionary filters
    if (!FClearingDiscretionaryFilters)
    {
        if (!ValidateAllData(true, true))
        {
            // Remember who called us and why, so we can replay the event when the data becomes valid again
            FFailedValidation_CtrlChangeEventArgsInfo = new TEventArgsInfo(sender, e);
            return;
        }
    }

    // Do we need to update the filter?
    // Yes if
    //  1. the panel is being shown and one or other has no ApplyNow button
    //  2. a control has been changed on a panel with no ApplyNow button
    bool DynamicStandardFilterPanel = (((FucoFilterAndFind.ShowApplyFilterButton == TUcoFilterAndFind.FilterContext.None) ||
        (FucoFilterAndFind.ShowApplyFilterButton == TUcoFilterAndFind.FilterContext.ExtraFilterOnly)));

    bool DynamicExtraFilterPanel = FucoFilterAndFind.IsExtraFilterShown && (((FucoFilterAndFind.ShowApplyFilterButton == TUcoFilterAndFind.FilterContext.None) ||
        (FucoFilterAndFind.ShowApplyFilterButton == TUcoFilterAndFind.FilterContext.StandardFilterOnly)));

    if ((sender is TUcoFilterAndFind) && (pnlFilterAndFind.Width > 0))
    {
        if ((FucoFilterAndFind.KeepFilterTurnedOnButtonDepressed == TUcoFilterAndFind.FilterContext.None ||
            FucoFilterAndFind.KeepFilterTurnedOnButtonDepressed == TUcoFilterAndFind.FilterContext.ExtraFilterOnly) ||
            (FucoFilterAndFind.IsExtraFilterShown &&
            (FucoFilterAndFind.KeepFilterTurnedOnButtonDepressed == TUcoFilterAndFind.FilterContext.None ||
            FucoFilterAndFind.KeepFilterTurnedOnButtonDepressed == TUcoFilterAndFind.FilterContext.StandardFilterOnly)))
        {
            ApplyFilter();
            Console.WriteLine("The panel has been toggled ON: Applying the dynamic filter");
        }
        else
        {
            Console.WriteLine("The panel has been toggled ON: Skipping dynamic filtering because the KeepFilterOn button(s) were depressed.");
        }
    }
    else if (((e.Context == TUcoFilterAndFind.EventContext.ecStandardFilterPanel) && DynamicStandardFilterPanel) ||
        ((e.Context == TUcoFilterAndFind.EventContext.ecExtraFilterPanel) && DynamicExtraFilterPanel))
    {
        ApplyFilter();
        Console.WriteLine("Applying the filter dynamically, due to a control value change");
    }

    if ((DynamicStandardFilterPanel || DynamicExtraFilterPanel) && !FucoFilterAndFind.IgnoreValueChangedEvent && !FClearingDiscretionaryFilters)
    {
        // If we are dynamic filtering we try to re-select the row we highlighted before - otherwise we keep the highlight in the same position
        //  However, if we are clearing the filter or the userControl is sending duplicate events, we skip this step
        int newRowAfterFiltering = grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow, FPrevRowChangedRow - 1) + 1;
        SelectRowInGrid(newRowAfterFiltering == 0 ? FPrevRowChangedRow : newRowAfterFiltering);

        if (sender as TUcoFilterAndFind == null)
        {
            ((Control)sender).Focus();
        }
    }
}

void FucoFilterAndFind_ApplyFilterClicked(object sender, TUcoFilterAndFind.TContextEventExtControlArgs e)
{
    if (ValidateAllData(true, true))
    {
        ApplyFilter();
        int newRowAfterFiltering = grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow, FPrevRowChangedRow - 1) + 1;
        SelectRowInGrid(newRowAfterFiltering == 0 ? FPrevRowChangedRow : newRowAfterFiltering);

        if (sender as TUcoFilterAndFind == null)
        {
            ((Control)sender).Focus();
        }
    }
}

void ApplyFilter()
{
    // Get the current filter and optionally call manual code so user can modify it, if necessary
    string previousFilter = FCurrentActiveFilter;

    try
    {
        FCurrentActiveFilter = FFilterPanelControls.GetCurrentFilter(
            FucoFilterAndFind == null || FucoFilterAndFind.IsCollapsed,
            (FucoFilterAndFind == null) ? TUcoFilterAndFind.FilterContext.None : FucoFilterAndFind.KeepFilterTurnedOnButtonDepressed,
            FFilterAndFindParameters.ShowFilterIsAlwaysOnLabelContext);
        {#APPLYFILTERMANUAL}

        if (grdDetails.DataSource != null)        
        {
            ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView.RowFilter = FCurrentActiveFilter;
        }
    }
    catch (Exception)
    {
        MessageBox.Show(MCommonResourcestrings.StrErrorInFilterCriterion, MCommonResourcestrings.StrFilterTitle,
            MessageBoxButtons.OK, MessageBoxIcon.Information);

        FCurrentActiveFilter = previousFilter;
    }

    UpdateRecordNumberDisplay();
    SetRecordNumberDisplayProperties();
}

/// Handler for the menu items Edit/Filter and Edit/Find
/// If this is part of a user control, it can be called from the parent
public void MniFilterFind_Click(object sender, EventArgs e)
{
    if (ValidateAllData(true, true))
    {
        if (FucoFilterAndFind == null)
        {
            ToggleFilter();
        }

        if (((ToolStripMenuItem)sender).Name == "mniEditFind")
        {
            if (FucoFilterAndFind.IsFindTabShown)
            {
                FucoFilterAndFind.DisplayFindTab();
                FFindPanelControls.FFindPanels[0].PanelControl.Focus();

                if (e is KeyPressEventArgs)
                {
                    // Act as if Find button was clicked and use up/down depending on whether F3 or SHIFT+F3
                    FucoFilterAndFind_FindNextClicked(null,
                        new TUcoFilterAndFind.TContextEventExtSearchDirectionArgs(
                            TUcoFilterAndFind.EventContext.ecFindPanel, ((KeyPressEventArgs)e).KeyChar == '-'));
                }
            }
        }
        else
        {
            FucoFilterAndFind.DisplayFilterTab();
            FFilterPanelControls.FStandardFilterPanels[0].PanelControl.Focus();
        }
    }
}


{##SNIPCLONELABEL}
TCloneFilterFindControl.ShallowClone<Label>({#CLONEDFROMLABEL}, T{#PANELTYPE}PanelControls.{#PANELTYPEUC}_NAME_SUFFIX),

{##SNIPINDIVIDUALFILTERFINDPANEL}
iffp = new TIndividualFilterFindPanel(
    {#CLONELABEL}TCloneFilterFindControl.{#CONTROLCLONE}<{#CONTROLTYPE}>({#CLONEDFROMCONTROL}, T{#PANELTYPE}PanelControls.{#PANELTYPEUC}_NAME_SUFFIX),
    {#DETAILTABLETYPE}Table.Get{#COLUMNNAME}DBName(),
    "{#COLUMNDATATYPE}",
    {#TAG});
F{#PANELTYPE}PanelControls.F{#PANELSUBTYPE}Panels.Add(iffp);

{##SNIPINDIVIDUALFILTERFINDPANELNOCOLUMN}
iffp = new TIndividualFilterFindPanel(
    {#CLONELABEL}TCloneFilterFindControl.{#CONTROLCLONE}<{#CONTROLTYPE}>({#CLONEDFROMCONTROL}, T{#PANELTYPE}PanelControls.{#PANELTYPEUC}_NAME_SUFFIX),
    null,
    null,
    {#TAG});
F{#PANELTYPE}PanelControls.F{#PANELSUBTYPE}Panels.Add(iffp);

{##SNIPDYNAMICCREATECONTROL}
{#CONTROLTYPE} {#CONTROLNAME} = new {#CONTROLTYPE}();
{#CONTROLNAME}.Name = "{#CONTROLNAME}";

{##SNIPDYNAMICCREATECONTROLWITHTEXT}
{#CONTROLTYPE} {#CONTROLNAME} = new {#CONTROLTYPE}();
{#CONTROLNAME}.Name = "{#CONTROLNAME}";
{#CONTROLNAME}.Text = "{#CONTROLTEXT}";

{##SNIPDYNAMICEVENTHANDLER}
(({#CONTROLTYPE})F{#PANELTYPE}PanelControls.FindControlByName("{#CONTROLNAME}")).{#EVENTNAME} += new System.EventHandler(this.{#EVENTHANDLER});

{##SNIPDYNAMICSETPROPERTY}
(({#CONTROLTYPE})F{#PANELTYPE}PanelControls.FindControlByName("{#CONTROLNAME}")).{#PROPERTYNAME} = {#PROPERTYVALUE};

{##SNIPNUMERICFILTERFINDCOLUMN}
FMainDS.{#DETAILTABLE}.Columns.Add(new DataColumn("{#COLUMNNAME}_text", typeof(System.String), "CONVERT({#COLUMNNAME}, 'System.String')"));

{##SNIPDYNAMICCREATERGR}
{#CREATERGR}

{#CREATERBTNS}

{#CLONERGR}

