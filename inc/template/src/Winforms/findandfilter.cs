{##FILTERANDFINDDECLARATIONS}
#region Filter and Find

TUcoFilterAndFind FucoFilterAndFind = null;
TUcoFilterAndFind.FilterAndFindParameters FFilterAndFindParameters;
ImageList FFilterImages;

#endregion

{##FILTERANDFINDMETHODS}
#region Filter and Find

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
            FucoFilterAndFind = new TUcoFilterAndFind(null, null, null, FFilterAndFindParameters);

            FucoFilterAndFind.Dock = DockStyle.Fill;
            pnlFilterAndFind.Controls.Add(FucoFilterAndFind);

            FucoFilterAndFind.Expanded += delegate { ToggleFilter(); };
            FucoFilterAndFind.Collapsed += delegate { ToggleFilter(); };
        }

        pnlFilterAndFind.Width = 150;
        chkToggleFilter.Checked = true;

        lblRecordCounter.ForeColor = System.Drawing.Color.MidnightBlue;
        lblRecordCounter.Font = new Font(lblRecordCounter.Font, FontStyle.Italic);

        chkToggleFilter.Image =  FFilterImages.Images[1];  // 'Filter is active' icon
        
        GetPetraUtilsObject().SetToolTip(chkToggleFilter, CommonFormsResourcestrings.StrFilterIsTurnedOn);
    }
    else
    {
        pnlFilterAndFind.Width = 0;
        chkToggleFilter.Checked = false;

        lblRecordCounter.ForeColor = System.Drawing.Color.SlateGray;
        lblRecordCounter.Font = new Font(lblRecordCounter.Font, FontStyle.Regular);
        chkToggleFilter.Image = FFilterImages.Images[0]; // 'Filter is inactive' icon
        
        GetPetraUtilsObject().SetToolTip(chkToggleFilter, CommonFormsResourcestrings.StrFilterIsTurnedOff);
    }       

    UpdateRecordNumberDisplay();
}

#endregion