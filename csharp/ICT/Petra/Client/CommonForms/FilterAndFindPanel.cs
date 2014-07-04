//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2014 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MCommon;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// The main class that handles the logic for Filter and Find.  It will be instantiated by every screen/control that requires Filter/Find
    /// </summary>
    public class TFilterAndFindPanel
    {
        /// <summary>
        /// This class stores the event arguments that applied to an event handler that could not be completed because of invalid data.
        /// When the data becomes valid the event can be 'replayed' using these stored arguments.
        /// </summary>
        public class TEventArgsInfo
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

        private IFilterAndFind FCallerFormOrControl = null;
        private TFrmPetraUtils FPetraUtilsObject = null;
        private TSgrdDataGrid FGrid = null;
        private IButtonPanel FButtonPanel = null;
        private Panel FPnlFilterFind = null;
        private CheckBox FChkToggleFilter = null;

        private TUcoFilterAndFind FucoFilterAndFind = null;
        private TUcoFilterAndFind.FilterAndFindParameters FFilterAndFindParameters;
        private ImageList FFilterImages;
        private TFilterPanelControls FFilterPanelControls = new TFilterPanelControls();
        private TFindPanelControls FFindPanelControls = new TFindPanelControls();
        private bool FIsFilterFindInitialised = false;
        private bool FClearingDiscretionaryFilters = false;
        private TEventArgsInfo FFailedValidation_CtrlChangeEventArgsInfo = null;
        private string FCurrentActiveFilter = String.Empty;
        private string FPreviousFilterTooltip = String.Format("{0}{1}{2}", CommonFormsResourcestrings.StrFilterIsHidden, Environment.NewLine, CommonFormsResourcestrings.StrFilterClickToTurnOn);


        /// <summary>
        /// The main constructor
        /// </summary>
        /// <param name="ACallerFormOrControl">The form or control that is instantiating this class</param>
        /// <param name="APetraUtilsObject">The TFrmPetraUtils instance associated with the caller form or control</param>
        /// <param name="AGrid">The grid associated with the caller form or control</param>
        /// <param name="AButtonPanel">The IButtonPanel associated with the caller form or control.  (Typically simply pass 'this').</param>
        /// <param name="APanelFilterFind">The Panel control associated with the caller form or control.</param>
        /// <param name="AChkToggleFilter">The checkbox control associated with the caller form or control.</param>
        public TFilterAndFindPanel(IFilterAndFind ACallerFormOrControl, TFrmPetraUtils APetraUtilsObject, TSgrdDataGridPaged AGrid, IButtonPanel AButtonPanel,
            Panel APanelFilterFind, CheckBox AChkToggleFilter)
        {
            FCallerFormOrControl = ACallerFormOrControl;
            FPetraUtilsObject = APetraUtilsObject;
            FGrid = AGrid;
            FButtonPanel = AButtonPanel;
            FPnlFilterFind = APanelFilterFind;
            FChkToggleFilter = AChkToggleFilter;

            SetupFilterAndFindControls();
        }

        /// <summary>
        /// Gets the initial parameters defined in the YAML file
        /// </summary>
        public TUcoFilterAndFind.FilterAndFindParameters FilterAndFindParameters
        {
            get
            {
                return FFilterAndFindParameters;
            }
        }

        /// <summary>
        /// Gets/sets the active filter string.  Note that setting the filter does not apply it.  You can use this call to initialise the current filter but apply it later
        /// </summary>
        public string CurrentActiveFilter
        {
            get
            {
                return FCurrentActiveFilter;
            }
            set
            {
                FCurrentActiveFilter = value;
            }
        }

        /// <summary>
        /// Returns true if the active filter string is the same as the base filter string
        /// </summary>
        public bool IsActiveFilterEqualToBase
        {
            get
            {
                return FCurrentActiveFilter == FFilterPanelControls.BaseFilter;
            }
        }

        /// <summary>
        /// Returns true if the base filter is showing all records
        /// </summary>
        public bool IsBaseFilterShowingAllRecords
        {
            get
            {
                return FFilterPanelControls.BaseFilterShowsAllRecords;
            }
        }

        /// <summary>
        /// Gets/sets the 'previous' filter tooltip string
        /// </summary>
        public string PreviousFilterTooltip
        {
            get
            {
                return FPreviousFilterTooltip;
            }
            set
            {
                FPreviousFilterTooltip = value;
            }
        }

        /// <summary>
        /// Gets the Filter/Find user control object
        /// </summary>
        public TUcoFilterAndFind FilterFindPanel
        {
            get
            {
                return FucoFilterAndFind;
            }
        }

        /// <summary>
        /// Gets the Filter Panel control objects (standard and Extra)
        /// </summary>
        public TFilterPanelControls FilterPanelControls
        {
            get
            {
                return FFilterPanelControls;
            }
        }

        /// <summary>
        /// Gets the Find Panel controls object
        /// </summary>
        public TFindPanelControls FindPanelControls
        {
            get
            {
                return FFindPanelControls;
            }
        }

        /// <summary>
        /// Gets the filter images list
        /// </summary>
        public ImageList FilterImages
        {
            get
            {
                return FFilterImages;
            }
        }

        /// <summary>
        /// Sets the flag indicating that discretionary filters are being cleared to prevent events being fired
        /// </summary>
        public bool ClearingDiscretionaryFilters
        {
            set
            {
                FClearingDiscretionaryFilters = value;
            }
        }

        /// <summary>
        /// Get or set the failed validation change event args so they can be stored/replayed
        /// </summary>
        public TEventArgsInfo FailedValidation_CtrlChangeEventArgsInfo
        {
            get
            {
                return FFailedValidation_CtrlChangeEventArgsInfo;
            }
            set
            {
                FFailedValidation_CtrlChangeEventArgsInfo = value;
            }
        }

        ///<summary>
        /// Sets up the Filter Button and the Filter and Find UserControl.
        /// </summary>
        private void SetupFilterAndFindControls()
        {
            LoadFilterIcons();

            // Further set up certain Controls Properties that can't be set directly in the WinForms Generator...
            FChkToggleFilter.AutoSize = true;
            FChkToggleFilter.Text = MCommonResourcestrings.StrBtnTextFilter;
            FChkToggleFilter.Tag = MCommonResourcestrings.StrCtrlSuppressChangeDetection;
            FChkToggleFilter.Image = FFilterImages.Images[0];  // 'Filter is inactive' icon
            FChkToggleFilter.ImageAlign = ContentAlignment.MiddleLeft;
            FChkToggleFilter.Appearance = Appearance.Button;
            FChkToggleFilter.TextAlign = ContentAlignment.MiddleCenter;  // Same as 'real' Button
            FChkToggleFilter.MinimumSize = new Size(75, 22);             // To prevent shrinkage!
            FChkToggleFilter.Click += new EventHandler(this.ToggleFilterPanel);

            FPetraUtilsObject.SetToolTip(FChkToggleFilter, FPreviousFilterTooltip);
            FPetraUtilsObject.SetStatusBarText(FChkToggleFilter, MCommonResourcestrings.StrClickToShowHideFilterPanel);

            // Prepare parameters for the UserControl that will display the Filter and Find Panels
            FCallerFormOrControl.InitialiseFilterFindParameters(out FFilterAndFindParameters);

            // Show Filter and Find Panels initially collapsed or expanded
            FPnlFilterFind.Width = 0;

            // Ensure that the Filter and Find Panel 'pushes' the Grid away instead of overlaying the Grid
            FGrid.BringToFront();

            if (FFilterAndFindParameters.FindAndFilterInitiallyExpanded)
            {
                ToggleFilter();
            }
        }

        /// <summary>
        /// Load the two icons for the filter button
        /// </summary>
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

        /// <summary>
        /// Sets the text in the main window status bar
        /// </summary>
        private void SetStatusBarText()
        {
            Control[] button = FucoFilterAndFind.Controls.Find("btnCloseFilter", true);
            if (button.Length > 0)
            {
                FPetraUtilsObject.SetStatusBarText(button[0], MCommonResourcestrings.StrClickToHideFilterPanel);
            }

            button = FucoFilterAndFind.Controls.Find("btnFindNext", true);
            if (button.Length > 0)
            {
                FPetraUtilsObject.SetStatusBarText(button[0], MCommonResourcestrings.StrClickToFindNextRecord);
            }

            button = FucoFilterAndFind.Controls.Find("rbtFindDirUp", true);
            if (button.Length > 0)
            {
                FPetraUtilsObject.SetStatusBarText(button[0], MCommonResourcestrings.StrUpDownFindDirection);
            }

            button = FucoFilterAndFind.Controls.Find("rbtFindDirDown", true);
            if (button.Length > 0)
            {
                FPetraUtilsObject.SetStatusBarText(button[0], MCommonResourcestrings.StrUpDownFindDirection);
            }

            if (FucoFilterAndFind.FilterPanelControls != null)
            {
                foreach (Panel panel in FucoFilterAndFind.FilterPanelControls)
                {
                    foreach (Control c in panel.Controls)
                    {
                        if (c.Name.StartsWith("btnClearArgument_"))
                        {
                            FPetraUtilsObject.SetStatusBarText(c, MCommonResourcestrings.StrClickToClearFilterAttribute);
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
                            FPetraUtilsObject.SetStatusBarText(c, MCommonResourcestrings.StrClickToClearFilterAttribute);
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
                            FPetraUtilsObject.SetStatusBarText(c, MCommonResourcestrings.StrClickToClearFindAttribute);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Private event handler for the toggle event
        /// </summary>
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

        /// <summary>
        /// Main method that handles the actions arising from toggling the filter panel on/off
        /// </summary>
        public void ToggleFilter()
        {
            if (FPnlFilterFind.Width == 0)
            {
                // Create the Filter and Find UserControl on-the-fly (loading the UserControl only when is is shown so that the screen can load faster!)
                if (FucoFilterAndFind == null)
                {
                    // Add columns to data table where filtering is numeric
                    FCallerFormOrControl.AddNumericColumnConversions();

                    // Create the individual panels from the YAML
                    FCallerFormOrControl.CreateFilterFindPanels();

                    // Construct a complete new Filter/Find Panel
                    FucoFilterAndFind = new TUcoFilterAndFind(
                        FFilterPanelControls.GetFilterPanels(),
                        FFilterPanelControls.GetExtraFilterPanels(),
                        FFindPanelControls.GetFindPanels(),
                        FFilterAndFindParameters);

                    FucoFilterAndFind.Dock = DockStyle.Fill;
                    FPnlFilterFind.Controls.Add(FucoFilterAndFind);

                    FucoFilterAndFind.Expanded += delegate { ToggleFilter(); };
                    FucoFilterAndFind.Collapsed += delegate { ToggleFilter(); };

                    FucoFilterAndFind.ApplyFilterClicked += new EventHandler<TUcoFilterAndFind.TContextEventExtControlArgs>(FucoFilterAndFind_ApplyFilterClicked);
                    FucoFilterAndFind.ArgumentCtrlValueChanged += new EventHandler<TUcoFilterAndFind.TContextEventExtControlValueArgs>(FucoFilterAndFind_ArgumentCtrlValueChanged);
                    FucoFilterAndFind.FindNextClicked += new EventHandler<TUcoFilterAndFind.TContextEventExtSearchDirectionArgs>(FucoFilterAndFind_FindNextClicked);

                    SetStatusBarText();
                }

                FPnlFilterFind.Width = FFilterAndFindParameters.FindAndFilterInitialWidth;
                FChkToggleFilter.Checked = true;
                FCallerFormOrControl.FilterToggled();

                FucoFilterAndFind_ArgumentCtrlValueChanged(FucoFilterAndFind, new TUcoFilterAndFind.TContextEventExtControlValueArgs(TUcoFilterAndFind.EventContext.ecFilterTab, null, null, null));
            }
            else
            {
                // Collapse the filter panel and uncheck the button if there is no active filter
                FPnlFilterFind.Width = 0;
                FCallerFormOrControl.FilterToggled();
                FCurrentActiveFilter = FFilterPanelControls.GetCurrentFilter(
                    true,
                    FucoFilterAndFind.KeepFilterTurnedOnButtonDepressed,
                    FucoFilterAndFind.ShowFilterIsAlwaysOnLabel);

                ((DevAge.ComponentModel.BoundDataView)FGrid.DataSource).DataView.RowFilter = FCurrentActiveFilter;

                FChkToggleFilter.Checked = false;
            }

            FButtonPanel.UpdateRecordNumberDisplay();
            FCallerFormOrControl.SetRecordNumberDisplayProperties();
            FCallerFormOrControl.SelectRowInGrid(this.FCallerFormOrControl.GetSelectedRowIndex());
            FPnlFilterFind.Visible = FPnlFilterFind.Width > 0;
        }

        /// <summary>
        /// Main method that is called when the Find Next button is clicked
        /// </summary>
        public void FucoFilterAndFind_FindNextClicked(object sender, TUcoFilterAndFind.TContextEventExtSearchDirectionArgs e)
        {
            if (e.Context == TUcoFilterAndFind.EventContext.ecFindPanel)
            {
                if (e.SearchUpwards)
                {
                    for (int rowNum = FCallerFormOrControl.GetSelectedRowIndex() - 1; rowNum > 0; rowNum--)
                    {
                        DataRowView rowView = (DataRowView)FGrid.Rows.IndexToDataSourceRow(rowNum);
                        if (FCallerFormOrControl.IsMatchingRow(rowView.Row))
                        {
                            FCallerFormOrControl.SelectRowInGrid(rowNum);
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
                    for (int rowNum = Math.Max(2, FCallerFormOrControl.GetSelectedRowIndex() + 1); rowNum < FGrid.Rows.Count; rowNum++)
                    {
                        DataRowView rowView = (DataRowView)FGrid.Rows.IndexToDataSourceRow(rowNum);
                        if (FCallerFormOrControl.IsMatchingRow(rowView.Row))
                        {
                            FCallerFormOrControl.SelectRowInGrid(rowNum);
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

        /// <summary>
        /// Main method that is called when a control on the filter/find panel is changed
        /// </summary>
        public void FucoFilterAndFind_ArgumentCtrlValueChanged(object sender, TUcoFilterAndFind.TContextEventExtControlValueArgs e)
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
                if (!FCallerFormOrControl.DoValidation(true, true))
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

            if ((sender is TUcoFilterAndFind) && (FPnlFilterFind.Width > 0))
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
                int newRowAfterFiltering = FGrid.DataSourceRowToIndex2(FCallerFormOrControl.GetSelectedDataRow(), FCallerFormOrControl.GetSelectedRowIndex() - 1) + 1;
                FCallerFormOrControl.SelectRowInGrid(newRowAfterFiltering == 0 ? FCallerFormOrControl.GetSelectedRowIndex() : newRowAfterFiltering);

                if (sender as TUcoFilterAndFind == null)
                {
                    ((Control)sender).Focus();
                }
            }
        }

        void FucoFilterAndFind_ApplyFilterClicked(object sender, TUcoFilterAndFind.TContextEventExtControlArgs e)
        {
            if (FCallerFormOrControl.DoValidation(true, true))
            {
                ApplyFilter();
                int newRowAfterFiltering = FGrid.DataSourceRowToIndex2(FCallerFormOrControl.GetSelectedDataRow(), FCallerFormOrControl.GetSelectedRowIndex() - 1) + 1;
                FCallerFormOrControl.SelectRowInGrid(newRowAfterFiltering == 0 ? FCallerFormOrControl.GetSelectedRowIndex() : newRowAfterFiltering);

                if (sender as TUcoFilterAndFind == null)
                {
                    ((Control)sender).Focus();
                }
            }
        }

        /// <summary>
        /// The main method that applies a filter to the default data view
        /// </summary>
        public void ApplyFilter()
        {
            // Get the current filter and optionally call manual code so user can modify it, if necessary
            string previousFilter = FCurrentActiveFilter;

            try
            {
                FCurrentActiveFilter = FFilterPanelControls.GetCurrentFilter(
                    FucoFilterAndFind == null || FucoFilterAndFind.IsCollapsed,
                    (FucoFilterAndFind == null) ? TUcoFilterAndFind.FilterContext.None : FucoFilterAndFind.KeepFilterTurnedOnButtonDepressed,
                    FFilterAndFindParameters.ShowFilterIsAlwaysOnLabelContext);
                FCallerFormOrControl.ApplyFilterString(ref FCurrentActiveFilter);

                if (FGrid.DataSource != null)
                {
                    ((DevAge.ComponentModel.BoundDataView)FGrid.DataSource).DataView.RowFilter = FCurrentActiveFilter;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(MCommonResourcestrings.StrErrorInFilterCriterion, MCommonResourcestrings.StrFilterTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                FCurrentActiveFilter = previousFilter;
            }

            FButtonPanel.UpdateRecordNumberDisplay();
            FCallerFormOrControl.SetRecordNumberDisplayProperties();
        }

        /// <summary>
        /// This is called when the filter/find menu has been clicked.  The sender may be a menu item of a shortcut key.
        /// </summary>
        public void MniFilterFind_Click(object sender, EventArgs e)
        {
            if (FucoFilterAndFind == null)
            {
                ToggleFilter();
            }

            string senderName;

            if (sender is ToolStripMenuItem)
            {
                senderName = ((ToolStripMenuItem)sender).Name;
            }
            else
            {
                senderName = sender.ToString();
            }

            if (senderName.StartsWith("mniEditFind"))
            {
                if (FucoFilterAndFind.IsFindTabShown)
                {
                    FucoFilterAndFind.DisplayFindTab();
                    FFindPanelControls.FFindPanels[0].PanelControl.Focus();

                    if (senderName.Length > 12)
                    {
                        // Act as if Find button was clicked and use up/down depending on whether Next or Previous
                        FucoFilterAndFind_FindNextClicked(null,
                            new TUcoFilterAndFind.TContextEventExtSearchDirectionArgs(
                                TUcoFilterAndFind.EventContext.ecFindPanel, (senderName == "mniEditFindPrevious")));
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

    /// <summary>
    /// Interface for screens/controls that implement Filter/Find
    /// </summary>
    public interface IFilterAndFind : IGridBase
    {
        /// <summary>
        /// Set the record number display properties on the screen/control
        /// </summary>
        void SetRecordNumberDisplayProperties();

        /// <summary>
        /// The screen should initialise the filter/find start up parameters from the YAML file
        /// </summary>
        void InitialiseFilterFindParameters(out TUcoFilterAndFind.FilterAndFindParameters AParams);

        /// <summary>
        /// Create the filter/Find panels using the data in the YAML file
        /// </summary>
        void CreateFilterFindPanels();

        /// <summary>
        /// The screen or control should perform the standard validation routine
        /// </summary>
        bool DoValidation(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors);

        /// <summary>
        /// Placeholder for optional additions to the data columns to handle LIKE comparisons for numbers
        /// </summary>
        void AddNumericColumnConversions();

        /// <summary>
        /// A notification to the screen that the filter has been toggled.  May call manual user code.
        /// </summary>
        void FilterToggled();

        /// <summary>
        /// Request to the screen to determine if the specified row matches the current Find criteria.  Optionally can call manual code.
        /// </summary>
        bool IsMatchingRow(DataRow ARow);

        /// <summary>
        /// Notification that the filter is about to be applied using the specified filter string.
        /// </summary>
        void ApplyFilterString(ref string AFilterString);
    }
}