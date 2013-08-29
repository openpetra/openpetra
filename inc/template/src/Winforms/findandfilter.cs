{##FILTERANDFINDDECLARATIONS}
#region Filter and Find

TUcoFilterAndFind FucoFilterAndFind = null;
TUcoFilterAndFind.FilterAndFindParameters FFilterAndFindParameters;
ImageList FFilterImages;

#endregion

{##FILTERANDFINDMETHODS}
#region Filter and Find

private TFilterPanelControls FFilterPanelControls = new TFilterPanelControls();
private TFindPanelControls FFindPanelControls = new TFindPanelControls();

///<summary>
/// Sets up the Filter Button and the Filter and Find UserControl.
/// </summary>
private void SetupFilterAndFindControls()
{
    LoadFilterIcons();

    // Further set up certain Controls Properties that can't be set directly in the WinForms Generator...
    chkToggleFilter.AutoSize = true;
    chkToggleFilter.Text = Catalog.GetString("Filte&r");
    chkToggleFilter.Tag = "SuppressChangeDetection";
    chkToggleFilter.Image = FFilterImages.Images[0];  // 'Filter is inactive' icon
    chkToggleFilter.ImageAlign = ContentAlignment.MiddleLeft;
    chkToggleFilter.Appearance = Appearance.Button;
    chkToggleFilter.TextAlign = ContentAlignment.MiddleCenter;  // Same as 'real' Button
    chkToggleFilter.MinimumSize = new Size(75, 22);             // To prevent shrinkage!
    chkToggleFilter.Click += delegate { ToggleFilter(); };
    
    GetPetraUtilsObject().SetToolTip(chkToggleFilter, CommonFormsResourcestrings.StrFilterIsTurnedOff);
                                     
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

private void ToggleFilterPanel(System.Object sender, EventArgs e)
{
    ToggleFilter();
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
        }

        pnlFilterAndFind.Width = FFilterAndFindParameters.FindAndFilterInitialWidth;
        chkToggleFilter.Checked = true;
        {#FILTERTOGGLEDMANUAL}

        FucoFilterAndFind_ArgumentCtrlValueChanged(FucoFilterAndFind, new TUcoFilterAndFind.TContextEventExtControlValueArgs(TUcoFilterAndFind.EventContext.ecFilterTab, null, null, null));

        lblRecordCounter.ForeColor = System.Drawing.Color.MidnightBlue;
        lblRecordCounter.Font = new Font(lblRecordCounter.Font, FontStyle.Italic);

        chkToggleFilter.Image =  FFilterImages.Images[1];  // 'Filter is active' icon
        
        GetPetraUtilsObject().SetToolTip(chkToggleFilter, CommonFormsResourcestrings.StrFilterIsTurnedOn);
    }
    else
    {
        // Collapse the filter panel and uncheck the button if there is no active filter
        pnlFilterAndFind.Width = 0;
        {#FILTERTOGGLEDMANUAL}
        string filterWhileOff = FFilterPanelControls.GetCurrentOffFilter(
            FFilterAndFindParameters,
            FucoFilterAndFind.IsExtraFilterShown,
            FucoFilterAndFind.KeepFilterTurnedOnButtonDepressed);

        ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView.RowFilter = filterWhileOff; 

        if (filterWhileOff == String.Empty)
        {
            chkToggleFilter.Checked = false;

            lblRecordCounter.ForeColor = System.Drawing.Color.SlateGray;
            lblRecordCounter.Font = new Font(lblRecordCounter.Font, FontStyle.Regular);
            chkToggleFilter.Image = FFilterImages.Images[0]; // 'Filter is inactive' icon
        
            GetPetraUtilsObject().SetToolTip(chkToggleFilter, CommonFormsResourcestrings.StrFilterIsTurnedOff);
        }
    }       

    UpdateRecordNumberDisplay();
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
                    return;
                }
            }
        }
        else
        {
            for (int rowNum = FPrevRowChangedRow + 1; rowNum < grdDetails.Rows.Count; rowNum++)
            {
                DataRowView rowView = (DataRowView)grdDetails.Rows.IndexToDataSourceRow(rowNum);
                if ({#ISMATCHINGROW}(rowView.Row))
                {
                    SelectRowInGrid(rowNum);
                    return;
                }
            }
        }
    }
}

void FucoFilterAndFind_ArgumentCtrlValueChanged(object sender, TUcoFilterAndFind.TContextEventExtControlValueArgs e)
{
    // Something has changed - do we need to update the filter?
    // Yes if 
    //  1. the panel is being shown and one or other has no ApplyNow button
    //  2. a control has been changed on a panel with no ApplyNow button
    bool DynamicStandardFilterPanel = (((FucoFilterAndFind.ShowApplyFilterButton == TUcoFilterAndFind.FilterContext.None) ||
        (FucoFilterAndFind.ShowApplyFilterButton == TUcoFilterAndFind.FilterContext.ExtraFilterOnly)) &&
        ((FucoFilterAndFind.ShowFilterIsAlwaysOnLabel == TUcoFilterAndFind.FilterContext.None) ||
        (FucoFilterAndFind.ShowFilterIsAlwaysOnLabel == TUcoFilterAndFind.FilterContext.ExtraFilterOnly)));

    bool DynamicExtraFilterPanel = (((FucoFilterAndFind.ShowApplyFilterButton == TUcoFilterAndFind.FilterContext.None) ||
        (FucoFilterAndFind.ShowApplyFilterButton == TUcoFilterAndFind.FilterContext.StandardFilterOnly)) &&
        ((FucoFilterAndFind.ShowFilterIsAlwaysOnLabel == TUcoFilterAndFind.FilterContext.None) ||
        (FucoFilterAndFind.ShowFilterIsAlwaysOnLabel == TUcoFilterAndFind.FilterContext.StandardFilterOnly)));

    if ((sender is TUcoFilterAndFind) && (pnlFilterAndFind.Width > 0) && (DynamicStandardFilterPanel || DynamicExtraFilterPanel))
    {
        if ((DynamicStandardFilterPanel &&
            (FucoFilterAndFind.KeepFilterTurnedOnButtonDepressed == TUcoFilterAndFind.FilterContext.None ||
            FucoFilterAndFind.KeepFilterTurnedOnButtonDepressed == TUcoFilterAndFind.FilterContext.ExtraFilterOnly)) ||
            (DynamicExtraFilterPanel &&
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
}

void FucoFilterAndFind_ApplyFilterClicked(object sender, TUcoFilterAndFind.TContextEventExtControlArgs e)
{
    ApplyFilter();
}

void ApplyFilter()
{
    // Get the current filter and optionally call manual code so user can modify it, if necessary
    string filter = FFilterPanelControls.GetCurrentFilter();
    {#APPLYFILTERMANUAL}

    ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView.RowFilter = filter;

    UpdateRecordNumberDisplay();
}


#endregion

{##SNIPCLONELABEL}
TCloneFilterFindControl.ShallowClone<Label>({#CLONEDFROMLABEL}, T{#PANELTYPE}PanelControls.{#PANELTYPEUC}_NAME_SUFFIX),

{##SNIPINDIVIDUALFILTERFINDPANEL}
iffp = new TIndividualFilterFindPanel(
    {#CLONELABEL}TCloneFilterFindControl.{#CONTROLCLONE}<{#CONTROLTYPE}>({#CLONEDFROMCONTROL}, T{#PANELTYPE}PanelControls.{#PANELTYPEUC}_NAME_SUFFIX),
    {#DETAILTABLE}Table.Get{#COLUMNNAME}DBName(),
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

