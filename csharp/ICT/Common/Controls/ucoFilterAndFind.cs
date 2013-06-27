//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Ict.Common.Controls.Formatting;

namespace Ict.Common.Controls
{
    /// <summary>
    /// UserControl that handles one or two Filter Panels and optionally
    /// a Find Panel.
    /// </summary>
    public partial class TUcoFilterAndFind : UserControl
    {
        /// <summary>
        /// Defines to which context a specific setting applies.
        /// </summary>
        public enum FilterContext
        {
            /// <summary>No Filter Context (=do not show!).</summary>
            fcNone,
            
            /// <summary>'Standard' Filter Context only (=not 'Extra' Filter Context).</summary>
            fcStandardFilterOnly,
            
            /// <summary>'Extra' Filter Context only (=not 'Standard' Filter Context).</summary>
            fcExtraFilterOnly,
            
            /// <summary>Both 'Standard' and 'Extra' Filter Context.</summary>
            fcStandardAndExtraFilter
        }

        private const string BTN_APPLY_FILTER_NAME = "btnApplyFilter";
        private const string BTN_KEEP_FILTER_TURNED_ON_NAME = "btnKeepFilterTurnedOn";
        private const string LBL_FILTER_IS_ALWAYS_TURNED_ON_NAME = "lblFilterIsAlwaysTurned On";

        private int FInitialWidth = 150;        
        private List<Panel> FFilterControls;
        private List<Panel> FExtraFilterControls;
        private List<Panel> FFindControls;
        private bool FShowExtraFilter = false;
        private bool FShowFindPanel = false;
        private FilterContext FShowApplyFilterButton = FilterContext.fcNone;
        private FilterContext FShowKeepFilterTurnedOnButton = FilterContext.fcNone;        
        private FilterContext FShowFilterIsAlwaysTurnedOnLabel = FilterContext.fcNone;
        private Owf.Controls.A1Panel FPnlFindControls;
        private Ict.Common.Controls.TTabVersatile FTabFilterAndFind = new TTabVersatile();
        private System.Windows.Forms.TabPage FTbpFilter = new System.Windows.Forms.TabPage();
        private System.Windows.Forms.TabPage FTbpFind = new System.Windows.Forms.TabPage();
        TSingleLineFlow FLayoutManagerFilterControls;
        
        #region Constructors
        
        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <remarks>
        /// IMPORTANT: This constructor only exists for WinForms Designer support.
        /// DO NOT create the UserControl using this Constructor at runtime - use the 
        /// overloaded Constructor for that instead!!!
        /// </remarks>
        public TUcoFilterAndFind()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TUcoFilterAndFind(List<Panel> AFilterControls, List<Panel> AExtraFilterControls, List<Panel> AFindControls,
            bool AShowExtraFilter = false, bool AShowFindPanel = false, 
            FilterContext AShowApplyFilterButton = FilterContext.fcNone,
            FilterContext AShowKeepFilterTurnedOnButton = FilterContext.fcNone,
            FilterContext AShowFilterIsAlwaysOnLabel = FilterContext.fcNone,
            int AWidth = 150)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            FFilterControls = AFilterControls;
            FExtraFilterControls = AExtraFilterControls;
            FFindControls = AFindControls;
            
            FShowExtraFilter = AShowExtraFilter;
            FShowApplyFilterButton = AShowApplyFilterButton;
            FShowKeepFilterTurnedOnButton = AShowKeepFilterTurnedOnButton;
            FShowFilterIsAlwaysTurnedOnLabel = AShowFilterIsAlwaysOnLabel;
            
            this.Width = AWidth;
            FInitialWidth = AWidth;
               
            FShowFindPanel = AShowFindPanel;
            
            InitUserControlInternal();
        }
        
        #endregion
        
        #region Properties
        
        /// <summary>
        /// Whether to show a second ('Extra') Filter Panel in addition to the one that is always displayed (=the 'Standard' one).
        /// If shown it is shown it is displayed below the 'standard' Filter Panel.
        /// </summary>
        public bool ShowExtraFilter
        {
            get
            {
                return FShowExtraFilter;
            }
            
            set
            {
                FShowExtraFilter = value;
                
                UpdateExtraFilterDisplay();
            }
        }
        
        /// <summary>
        /// Sets the context in which an 'Apply Filter' Button is shown (if any).
        /// </summary>
        public FilterContext ShowApplyFilterButton
        {
            get
            {
                return FShowApplyFilterButton;
            }
            
            set
            {
                FShowApplyFilterButton = value;
                
                UpdateFilterApplyButtons();
            }
        }

        /// <summary>
        /// Sets the context in which an 'Keep Filter Turned On' Button is shown (if any).
        /// </summary>
        public FilterContext ShowKeepFilterTurnedOnButton
        {
            get
            {
                return FShowKeepFilterTurnedOnButton;
            }
            
            set
            {
                FShowKeepFilterTurnedOnButton = value;
                
                UpdateKeepFilterTurnedOnButtons();
            }
        }

        /// <summary>
        /// Sets the context in which an 'Filter Is Always On' Label is shown (if any).
        /// </summary>
        public FilterContext ShowFilterIsAlwaysOnLabel
        {
            get
            {
                return FShowFilterIsAlwaysTurnedOnLabel;
            }
            
            set
            {
                FShowFilterIsAlwaysTurnedOnLabel = value;
                
                UpdateFilterIsAlwaysTurnedOnLabels();
            }
        }
        
        /// <summary>
        /// Whether the Control is collapsed.
        /// </summary>
        public bool Collapsed
        {
            get
            {
                return this.Width == 0;
            }
        }
        
        #endregion

        #region Events

        // TODO
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Call this Method to show a TabControl with both Filter and Find options.
        /// </summary>
        public void SetShowFindTab()
        {
            FShowFindPanel = true;                   
            
            AddTabs();

            SetupTitleText();
        }

        /// <summary>
        /// Tells whether a TabControl with both Filter and Find options is shown.
        /// </summary>
        /// <returns></returns>
        public bool GetShowFindTab()
        {            
            return FShowFindPanel;
        }
        
        /// <summary>
        /// Shows the Find Tab if it is available (<see cref="SetShowFindTab" /> needs to have been called). 
        /// If the Control is collapsed it will be expanded automatically, too.
        /// </summary>
        public void DisplayFindTab()
        {
            if(FShowFindPanel)
            {
                FTabFilterAndFind.SelectedTab = FTbpFind;
            }
            
            if (Collapsed) 
            {
                Expand();    
            }
        }

        /// <summary>
        /// Expands (=shows) the Filter/Find Panel to its original width.
        /// </summary>        
        public void Expand()
        {
            this.Width = FInitialWidth;
        }
        
        /// <summary>
        /// Collapses (=hides) the Filter/Find Panel.
        /// </summary>
        public void Collapse()
        {
            this.Width = 0;
        }
        
        #endregion

        #region Private Methods

        private void InitUserControlInternal()
        {
            TSingleLineFlow LayoutManagerUserControl;
            
            TSingleLineFlow LayoutManagerExtraFilterControls;

            //
            // Set up Layout Managers (Note: Layout Managers for TabPages are set up dynamically in Method 'AddTabs'!)
            //
            
            // Layout Manager for the 'Standard' Filter Panel
            // This will arrange 'Argument Panels' that will be added later to the 'Standard' Filter Panel.
            FLayoutManagerFilterControls = new TSingleLineFlow(pnlFilterControls, 4, 3);             
            FLayoutManagerFilterControls.TopMargin = 5;
            FLayoutManagerFilterControls.RightMargin = 9;
            FLayoutManagerFilterControls.SpacerDistance = 7;
            
            if (FShowExtraFilter) 
            {
                // Layout Manager for the 'Extra' Filter Panel.
                // This will arrange 'Argument Panels' that will be added later to the 'Extra' Filter Panel.
                LayoutManagerExtraFilterControls = new TSingleLineFlow(pnlExtraFilterControls, 4, 3);             
                LayoutManagerExtraFilterControls.TopMargin = 5;
                LayoutManagerExtraFilterControls.RightMargin = 9;
                LayoutManagerExtraFilterControls.SpacerDistance = 7;                
                
                // Set off the 'Extra' Filter Panel from the 'Standard' Filter Panel once the Layout Manager is applied to the containing Panel/TabPage
                pnlExtraFilterControls.Tag = TSingleLineFlow.BeginGroupIndicator;                
            }
            else
            {
                pnlExtraFilterControls.Visible = false;
            }
            
            // Use a Layout Manager for the whole UserControl only if we don't display a Tab Control (it would be useless then).
            // This will arrange the 'Standard' and 'Extra' Filter Panel that are already shown on the UserControl.
            if (!FShowFindPanel) 
            {
                LayoutManagerUserControl = new TSingleLineFlow(this, 5, 2);             
                LayoutManagerUserControl.SpacerDistance = 5;                
            }
                
            
            // Add individual 'Argument Panels' to the 'Standard Filter' Panel (topmost on 'Filter' Tab)
            // Layout is taken care of automatically due to a TSingleLineFlow Layout Manager!
            if (FFilterControls != null) 
            {
                foreach (Panel ArgumentPanel in FFilterControls) 
                {
                    pnlFilterControls.Controls.Add(ArgumentPanel);
                }               
            }
            
            // Add individual 'Argument Panels' to the 'Extra Filter' Panel (below 'Standard Filter' Panel on 'Filter' Tab)
            // Layout is taken care of automatically due to a TSingleLineFlow Layout Manager!
            if (FExtraFilterControls != null) 
            {            
                foreach (Panel ArgumentPanel in FExtraFilterControls) 
                {
                    pnlExtraFilterControls.Controls.Add(ArgumentPanel);
                }
            }
            
            UpdateKeepFilterTurnedOnButtons(false);
            UpdateFilterIsAlwaysTurnedOnLabels(false);
            UpdateFilterApplyButtons(false);
            
            UpdateExtraFilterDisplay();            
            
            SetupTitleText();



            if (FShowFindPanel) 
            {
                SetShowFindTab();
            }
            
            // Reverse the Z-Order of the Panels so they 'stack up' correctly
            pnlFilterControls.SendToBack();
            pnlExtraFilterControls.SendToBack();
         
            
            this.Invalidate();
            
            AutoSizeFilterPanelsHeights();
        }        
        
        private void UpdateExtraFilterDisplay()
        {
            pnlExtraFilterControls.Visible = FShowExtraFilter;
        }
        
        private void SetupTitleText()
        {            
            if (FShowFindPanel) 
            {
                lblTitle.Text = Catalog.GetString("List Filter and Find");
            }
            else
            {
                lblTitle.Text = Catalog.GetString("List Filter");
            }            
        }
        
        #region 'Apply Filter' Button adding/removal

        private void UpdateFilterApplyButtons(bool AAutoSizePanels = true)
        {
            switch (FShowApplyFilterButton) 
            {
                case TUcoFilterAndFind.FilterContext.fcNone:
                    
                    RemoveButtonApplyFilter(pnlFilterControls);
                    RemoveButtonApplyFilter(pnlExtraFilterControls);

                    break;
                    
                case TUcoFilterAndFind.FilterContext.fcStandardFilterOnly:
                    
                    AddButtonApplyFilter(pnlFilterControls);
                    RemoveButtonApplyFilter(pnlExtraFilterControls);
                    
                    break;
                    
                case TUcoFilterAndFind.FilterContext.fcExtraFilterOnly:

                    AddButtonApplyFilter(pnlExtraFilterControls);
                    RemoveButtonApplyFilter(pnlFilterControls);
                    
                    break;
                    
                case TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter:
                    
                    AddButtonApplyFilter(pnlFilterControls);
                    AddButtonApplyFilter(pnlExtraFilterControls);
                    
                    break;
                    
                default:
                    throw new Exception("Invalid value for TUcoFilterAndFind.FilterContext");
            }
            
            if (AAutoSizePanels) 
            {
                AutoSizeFilterPanelsHeights();    
            }
            
        }
        
        /// <summary>
        /// Adds an 'Apply Filter' Button to a Filter Panel (if it isn't already there).
        /// </summary>
        /// <param name="AFilterPanel">The Filter Panel to add the Button to.</param>
        private void AddButtonApplyFilter(Panel AFilterPanel)
        {
            Control[] ControlsArray;
            Button BtnApplyFilter;
            
            ControlsArray = AFilterPanel.Controls.Find(BTN_APPLY_FILTER_NAME, false);
                        
            if (ControlsArray.Length == 0) 
            {
                BtnApplyFilter = new Button();
                BtnApplyFilter.Name = BTN_APPLY_FILTER_NAME;
                
                BtnApplyFilter.Left = 5;
                BtnApplyFilter.Text = "Appl&y Filter";                
                BtnApplyFilter.BackColor = System.Drawing.SystemColors.ButtonFace;
                BtnApplyFilter.Click += delegate { BtnApplyFilter.Text = "Applying Filter..."; BtnApplyFilter.Enabled = false; System.Threading.Thread.Sleep(2000); BtnApplyFilter.Text = "Appl&y Filter"; BtnApplyFilter.Enabled = true; };                
                
                AFilterPanel.Controls.Add(BtnApplyFilter);


                // In case there is no 'Keep Filter Turned On' Button and no 'Filter Always Turned On' Label
                if((AFilterPanel.Controls.Find(BTN_KEEP_FILTER_TURNED_ON_NAME, false).Length == 0)
                   && (AFilterPanel.Controls.Find(LBL_FILTER_IS_ALWAYS_TURNED_ON_NAME, false).Length == 0))
                {
                    BtnApplyFilter.Tag = TSingleLineFlow.BeginGroupIndicator;                
                }
                
                // Ensure that this Control is always the bottommost of the Controls in the Panel
                if (AFilterPanel.Controls.GetChildIndex(BtnApplyFilter) != AFilterPanel.Controls.Count) 
                {
                    AFilterPanel.Controls.SetChildIndex(BtnApplyFilter, AFilterPanel.Controls.Count);
                }
            }
        }

        /// <summary>
        /// Removes an 'Apply Filter' Button from a Filter Panel (if it is there).
        /// </summary>
        /// <param name="AFilterPanel">The Filter Panel to remove the Button from.</param>        
        private void RemoveButtonApplyFilter(Panel AFilterPanel)
        {
            // Remove 'Apply Filter' Button from Filter Panel
            AFilterPanel.Controls.RemoveByKey(BTN_APPLY_FILTER_NAME);
        }
                
        #endregion       
        
        #region 'Keep Filter Turned On' Button adding/removal
        
        private void UpdateKeepFilterTurnedOnButtons(bool AAutoSizePanel = true)
        {
            switch (FShowKeepFilterTurnedOnButton) 
            {
                case TUcoFilterAndFind.FilterContext.fcNone:
                    
                    RemoveButtonKeepFilterTurnedOn(pnlFilterControls);
                    RemoveButtonKeepFilterTurnedOn(pnlExtraFilterControls);

                    break;
                    
                case TUcoFilterAndFind.FilterContext.fcStandardFilterOnly:
                    
                    AddButtonKeepFilterTurnedOn(pnlFilterControls);
                    RemoveButtonKeepFilterTurnedOn(pnlExtraFilterControls);
                    
                    break;
                    
                case TUcoFilterAndFind.FilterContext.fcExtraFilterOnly:

                    AddButtonKeepFilterTurnedOn(pnlExtraFilterControls);
                    RemoveButtonKeepFilterTurnedOn(pnlFilterControls);
                    
                    break;
                    
                case TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter:
                    
                    AddButtonKeepFilterTurnedOn(pnlFilterControls);
                    AddButtonKeepFilterTurnedOn(pnlExtraFilterControls);
                    
                    break;
                    
                default:
                    throw new Exception("Invalid value for TUcoFilterAndFind.FilterContext");
            }
            
            if (AAutoSizePanel) 
            {
                AutoSizeFilterPanelsHeights();    
            }
        }
        
        /// <summary>
        /// Adds a 'Keep Filter Turned On' Button to a Filter Panel (if it isn't already there).
        /// </summary>
        /// <param name="AFilterPanel">The Filter Panel to add the Button to.</param>
        private void AddButtonKeepFilterTurnedOn(Panel AFilterPanel)
        {
            Control[] ControlsArray;
            CheckBox BtnKeepFilterTurnedOn;
            
            ControlsArray = AFilterPanel.Controls.Find(BTN_KEEP_FILTER_TURNED_ON_NAME, false);
                        
            if (ControlsArray.Length == 0) 
            {
                BtnKeepFilterTurnedOn = new CheckBox();
                BtnKeepFilterTurnedOn.Name = BTN_KEEP_FILTER_TURNED_ON_NAME;
                
                BtnKeepFilterTurnedOn.Left = 5;
                BtnKeepFilterTurnedOn.Height = 22;
                BtnKeepFilterTurnedOn.Font = new System.Drawing.Font("Verdana", 8.0f);
                BtnKeepFilterTurnedOn.Text = Catalog.GetString("Kee&p Filter Turned On");                
                BtnKeepFilterTurnedOn.Tag = "SuppressChangeDetection" + ";" + TSingleLineFlow.BeginGroupIndicator;
                BtnKeepFilterTurnedOn.FlatStyle = FlatStyle.System;               // this is set so that the Button doesn't let the background colour shine through, but uses the system's colours and uses a gradient!
                BtnKeepFilterTurnedOn.Appearance = Appearance.Button;
                BtnKeepFilterTurnedOn.TextAlign = ContentAlignment.MiddleCenter;  // Same as 'real' Button 
                BtnKeepFilterTurnedOn.MinimumSize = new Size(75, 22);             // To prevent shrinkage!
                
                AFilterPanel.Controls.Add(BtnKeepFilterTurnedOn);

                ControlsArray = AFilterPanel.Controls.Find(BTN_APPLY_FILTER_NAME, false);
                
                // Ensure that this Control is...
                if (ControlsArray.Length == 0)
                {
                    // ... always the bottommost of the Controls in the Panel in case there is no 'Apply Filter' Button
                    if (AFilterPanel.Controls.GetChildIndex(BtnKeepFilterTurnedOn) != AFilterPanel.Controls.Count - 1) 
                    {
                        AFilterPanel.Controls.SetChildIndex(BtnKeepFilterTurnedOn, AFilterPanel.Controls.Count - 1);                        
                    }                    
                }
                else
                {
                    // ... always one up from the bottommost of the Controls in the Panel in case there is a 'Apply Filter' Button (which is always the bottommost Control)
                    int tmp = AFilterPanel.Controls.GetChildIndex(BtnKeepFilterTurnedOn);
                    
                    if (AFilterPanel.Controls.GetChildIndex(BtnKeepFilterTurnedOn) == AFilterPanel.Controls.Count - 1) 
                    {
//                        BtnKeepFilterTurnedOn.Tag += TSingleLineFlow.BeginGroupIndicator;                        
                        ControlsArray[0].Tag = ""; // remove any TSingleLineFlow.BeginGroupIndicator!
                        AFilterPanel.Controls.SetChildIndex(BtnKeepFilterTurnedOn, AFilterPanel.Controls.Count - 2);
                    }
                }
            }
            
            //
            // Ensure that no 'Filter Always Turned On' Label is left on the Panel as it is mutually exclusive to the 'Keep Filter Turned On' Button
            //
            ControlsArray = AFilterPanel.Controls.Find(LBL_FILTER_IS_ALWAYS_TURNED_ON_NAME, false);
                        
            if (ControlsArray.Length != 0) 
            {
                RemoveLabelFilterIsAlwaysTurnedOn(AFilterPanel);

                if (FShowFilterIsAlwaysTurnedOnLabel == FilterContext.fcStandardAndExtraFilter) 
                {
                    if (AFilterPanel == pnlFilterControls) 
                    {
                        FShowFilterIsAlwaysTurnedOnLabel = FilterContext.fcExtraFilterOnly;
                    }
                    else
                    {
                        FShowFilterIsAlwaysTurnedOnLabel = FilterContext.fcStandardFilterOnly;
                    }
                }
                else if((FShowFilterIsAlwaysTurnedOnLabel == FilterContext.fcStandardFilterOnly) 
                        || (FShowFilterIsAlwaysTurnedOnLabel == FilterContext.fcExtraFilterOnly))
                {
                    FShowFilterIsAlwaysTurnedOnLabel = FilterContext.fcNone;
                }                                    
            }            
        }
        
        /// <summary>
        /// Removes a 'Keep Filter Turned On' Button from a Filter Panel (if it is there).
        /// </summary>
        /// <param name="AFilterPanel">The Filter Panel to remove the Button from.</param>
        private void RemoveButtonKeepFilterTurnedOn(Panel AFilterPanel)
        {
            Control[] ControlsArray = AFilterPanel.Controls.Find(BTN_APPLY_FILTER_NAME, false);
            Control[] ControlsArray2 = AFilterPanel.Controls.Find(LBL_FILTER_IS_ALWAYS_TURNED_ON_NAME, false);
            
            // If an 'Apply Filter' Button is shown but no 'Filter Always Turned On' Label: set up 'Apply Filter' Button so that it is set off from the rest of the Controls
            if ((ControlsArray.Length != 0)
                && (ControlsArray2.Length == 0))
            {
                ControlsArray[0].Tag = TSingleLineFlow.BeginGroupIndicator;
            }
            
            // Remove 'Keep Filter Turned On'  Button from Filter Panel
            AFilterPanel.Controls.RemoveByKey(BTN_KEEP_FILTER_TURNED_ON_NAME);
        }
        
        #endregion

        #region 'Filter Always Turned On' Label adding/removal
        
        private void UpdateFilterIsAlwaysTurnedOnLabels(bool AAutoSizePanel = true)
        {
            switch (FShowFilterIsAlwaysTurnedOnLabel) 
            {
                case TUcoFilterAndFind.FilterContext.fcNone:
                    
                    RemoveLabelFilterIsAlwaysTurnedOn(pnlFilterControls);
                    RemoveLabelFilterIsAlwaysTurnedOn(pnlExtraFilterControls);

                    break;
                    
                case TUcoFilterAndFind.FilterContext.fcStandardFilterOnly:
                    
                    AddLabelFilterIsAlwaysTurnedOn(pnlFilterControls);
                    RemoveLabelFilterIsAlwaysTurnedOn(pnlExtraFilterControls);
                    
                    break;
                    
                case TUcoFilterAndFind.FilterContext.fcExtraFilterOnly:

                    AddLabelFilterIsAlwaysTurnedOn(pnlExtraFilterControls);
                    RemoveLabelFilterIsAlwaysTurnedOn(pnlFilterControls);
                    
                    break;
                    
                case TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter:
                    
                    AddLabelFilterIsAlwaysTurnedOn(pnlFilterControls);
                    AddLabelFilterIsAlwaysTurnedOn(pnlExtraFilterControls);
                    
                    break;
                    
                default:
                    throw new Exception("Invalid value for TUcoFilterAndFind.FilterContext");
            }
            
            if (AAutoSizePanel) 
            {
                AutoSizeFilterPanelsHeights();    
            }
        }
        
        /// <summary>
        /// Adds a 'Filter Always Turned On' Label to a Filter Panel (if it isn't already there).
        /// </summary>
        /// <param name="AFilterPanel">The Filter Panel to add the Label to.</param>
        private void AddLabelFilterIsAlwaysTurnedOn(Panel AFilterPanel)
        {
            Control[] ControlsArray;
            Label LblFilterIsAlwaysTurnedOn;
            
            ControlsArray = AFilterPanel.Controls.Find(LBL_FILTER_IS_ALWAYS_TURNED_ON_NAME, false);
                        
            if (ControlsArray.Length == 0) 
            {
                LblFilterIsAlwaysTurnedOn = new Label();
                LblFilterIsAlwaysTurnedOn.Name = LBL_FILTER_IS_ALWAYS_TURNED_ON_NAME;
                
                LblFilterIsAlwaysTurnedOn.Left = 5;
                LblFilterIsAlwaysTurnedOn.Height = 20;
                LblFilterIsAlwaysTurnedOn.BackColor = System.Drawing.Color.Transparent;
                LblFilterIsAlwaysTurnedOn.Tag = TSingleLineFlow.BeginGroupIndicator;
                LblFilterIsAlwaysTurnedOn.Font = new System.Drawing.Font("Verdana", 8.0f, FontStyle.Italic);
                LblFilterIsAlwaysTurnedOn.Text = Catalog.GetString("Filter Always Turned On");                
                LblFilterIsAlwaysTurnedOn.TextAlign = ContentAlignment.MiddleCenter;
                
                AFilterPanel.Controls.Add(LblFilterIsAlwaysTurnedOn);

                ControlsArray = AFilterPanel.Controls.Find(BTN_APPLY_FILTER_NAME, false);
                
                // Ensure that this Control is...
                if (ControlsArray.Length == 0)
                {
                    // ... always the bottommost of the Controls in the Panel in case there is no 'Apply Filter' Button
                    if (AFilterPanel.Controls.GetChildIndex(LblFilterIsAlwaysTurnedOn) != AFilterPanel.Controls.Count - 1) 
                    {
                        AFilterPanel.Controls.SetChildIndex(LblFilterIsAlwaysTurnedOn, AFilterPanel.Controls.Count - 1);                        
                    }                    
                }
                else
                {
                    // ... always one up from the bottommost of the Controls in the Panel in case there is a 'Apply Filter' Button (which is always the bottommost Control)
                    int tmp = AFilterPanel.Controls.GetChildIndex(LblFilterIsAlwaysTurnedOn);
                    
                    if (AFilterPanel.Controls.GetChildIndex(LblFilterIsAlwaysTurnedOn) == AFilterPanel.Controls.Count - 1) 
                    {
                        ControlsArray[0].Tag = ""; // remove any TSingleLineFlow.BeginGroupIndicator!
                        AFilterPanel.Controls.SetChildIndex(LblFilterIsAlwaysTurnedOn, AFilterPanel.Controls.Count - 2);
//                        AFilterPanel.Refresh();
                    }
                }
            }
            
            //
            // Ensure that no 'Keep Filter Turned On' Button is left on the Panel as it is mutually exclusive to the 'Filter Always Turned On' Label
            //
            ControlsArray = AFilterPanel.Controls.Find(BTN_KEEP_FILTER_TURNED_ON_NAME, false);
                        
            if (ControlsArray.Length != 0) 
            {
                RemoveButtonKeepFilterTurnedOn(AFilterPanel);
                
                if (FShowKeepFilterTurnedOnButton == FilterContext.fcStandardAndExtraFilter) 
                {
                    if (AFilterPanel == pnlFilterControls) 
                    {
                        FShowKeepFilterTurnedOnButton = FilterContext.fcExtraFilterOnly;
                    }
                    else
                    {
                        FShowKeepFilterTurnedOnButton = FilterContext.fcStandardFilterOnly;
                    }
                }
                else if((FShowKeepFilterTurnedOnButton == FilterContext.fcStandardFilterOnly) 
                        || (FShowKeepFilterTurnedOnButton == FilterContext.fcExtraFilterOnly))
                {
                    FShowKeepFilterTurnedOnButton = FilterContext.fcNone;
                }                    
            }
        }
        
        /// <summary>
        /// Removes a 'Filter Always Turned On' Label from a Filter Panel (if it is there).
        /// </summary>
        /// <param name="AFilterPanel">The Filter Panel to remove the Label from.</param>
        private void RemoveLabelFilterIsAlwaysTurnedOn(Panel AFilterPanel)
        {
            Control[] ControlsArray = AFilterPanel.Controls.Find(BTN_APPLY_FILTER_NAME, false);
            Control[] ControlsArray2 = AFilterPanel.Controls.Find(BTN_KEEP_FILTER_TURNED_ON_NAME, false);

            // If an 'Apply Filter' Button is shown but no 'Keep Filter Turned On' Button: set up 'Apply Filter' Button so that it is set off from the rest of the Controls
            if ((ControlsArray.Length != 0)
                && (ControlsArray2.Length == 0))
            {
                ControlsArray[0].Tag = TSingleLineFlow.BeginGroupIndicator;
            }
            
            // Remove 'Filter Always Turned On' Label from Filter Panel
            AFilterPanel.Controls.RemoveByKey(LBL_FILTER_IS_ALWAYS_TURNED_ON_NAME);
        }
        
        #endregion
                        
        /// <summary>
        /// Adjusts the heights of both Filter Panels so that all Controls fit on it.
        /// </summary>
        /// <remarks>
        /// Adjusting the height of the Find Panel (FPnlFindControls) needs to be done
        /// in Method 'FTabFilterAndFind_SelectedIndexChanged' as the adjusting only works
        /// once the Tab that contains the Find Panel has been shown, which only happens once
        /// it has been switched to once!
        /// </remarks>
        private void AutoSizeFilterPanelsHeights()
        {
            AutoSizePanelHeight(pnlFilterControls);
            
            if (FShowExtraFilter) 
            {
                AutoSizePanelHeight(pnlExtraFilterControls);    
            }
        }
        
        /// <summary>
        /// Adjusts the Height of a certain Panel so that all Controls fit on it.
        /// A call to this Method has no effect if there are no Controls on the specified Panel.
        /// </summary>
        /// <param name="APanel">The Panel whose Height should be adjusted.</param>
        private void AutoSizePanelHeight(Panel APanel)
        {
            const int BOTTOM_BORDER = 10; // in Pixels;
            
            int LastControlsBottom = -1;
            
            if (APanel == null)
            {
                return;
            }
                                
            if(APanel.Controls.Count > 0)
            {
                // Determine the Control that lies furthest down on the Panel
                foreach (Control SingleControl in APanel.Controls)
                {
                    if (SingleControl.Bottom > LastControlsBottom) 
                    {
                        LastControlsBottom = SingleControl.Bottom;
                    }
                } 
                
                // If we found at least one Control...
                if (LastControlsBottom > -1) 
                {
                    // ...resize Panel in height so it fits all Controls and is a bit higher still
                    APanel.Height = LastControlsBottom + BOTTOM_BORDER;
                }                
            }
            else
            {
                // Default to BOTTOM_BORDER height if there are no Controls on this Panel
                APanel.Height = BOTTOM_BORDER;
            }
        }
        
        private void AddTabs()
        {
            Panel pnlFindOptions = new Panel();
            Button btnResetFilterCode = new Button();            
            Button btnResetFilterName = new Button();            
            Button btnFindNext = new Button();
            GroupBox grpFindDirection = new GroupBox();
            RadioButton rbtFindDirUp = new RadioButton();
            RadioButton rbtFindDirDown = new RadioButton();
            TSingleLineFlow LayoutManagerFindControls;
            TSingleLineFlow LayoutManagerFilterTab;
            TSingleLineFlow LayoutManagerFindTab;
            System.Drawing.Color GradientEndColor = Color.FromArgb(
                System.Drawing.Color.BurlyWood.R - 30,
                System.Drawing.Color.BurlyWood.G - 20,
                System.Drawing.Color.BurlyWood.B - 15);
            
            this.SuspendLayout();
            grpFindDirection.SuspendLayout();
            pnlFindOptions.SuspendLayout();
            FTabFilterAndFind.SuspendLayout();
            
            FPnlFindControls = new Owf.Controls.A1Panel();
            FPnlFindControls.SuspendLayout();
            FPnlFindControls.Name = "FPnlFindControls";
            FPnlFindControls.Left = 7;
            FPnlFindControls.Top = 8;
            FPnlFindControls.Width = 154;
            FPnlFindControls.Height = 174;
            FPnlFindControls.BorderColor = System.Drawing.Color.CadetBlue;
            FPnlFindControls.ShadowOffSet = 4;
            FPnlFindControls.RoundCornerRadius = 4;
            FPnlFindControls.GradientStartColor = System.Drawing.Color.BurlyWood;
            FPnlFindControls.GradientEndColor = GradientEndColor;
            FPnlFindControls.GradientDirection = LinearGradientMode.Horizontal;
            
            // Layout Manager for the 'Find' Panel.
            // This will arrange 'Argument Panels' that will be added later to the 'Find' Panel.
            LayoutManagerFindControls = new TSingleLineFlow(FPnlFindControls, 4, 3);             
            LayoutManagerFindControls.TopMargin = 5;
            LayoutManagerFindControls.RightMargin = 9;
            LayoutManagerFindControls.SpacerDistance = 7;           
            
            btnFindNext.Top = 5;
            btnFindNext.Left = 5;
            btnFindNext.Width = 139;
            btnFindNext.Text = "Find Ne&xt";
            btnFindNext.BackColor = System.Drawing.SystemColors.ButtonFace;
            
            rbtFindDirUp.Top = 14;
            rbtFindDirUp.Left = 10;
            rbtFindDirUp.AutoSize = true;
            rbtFindDirUp.Text = Catalog.GetString("Up");
            
            rbtFindDirDown.Top = 14;
            rbtFindDirDown.Left = 60;
            rbtFindDirDown.AutoSize = true;
            rbtFindDirDown.Checked = true;
            rbtFindDirDown.Text = Catalog.GetString("Down");            
            
            grpFindDirection.Top = 33;
            grpFindDirection.Left = 5;
            grpFindDirection.Width = 140;
            grpFindDirection.Height = 38;
            grpFindDirection.BackColor = System.Drawing.Color.Transparent;
            grpFindDirection.Text = "Direction";
            grpFindDirection.Controls.Add(rbtFindDirUp);
            grpFindDirection.Controls.Add(rbtFindDirDown);

            pnlFindOptions.Name = "FPnlFindControls";
            pnlFindOptions.Left = 0;
            pnlFindOptions.Width = FTabFilterAndFind.Width;
            pnlFindOptions.Height = 75;
            pnlFindOptions.BackColor = System.Drawing.Color.Transparent;
            pnlFindOptions.Tag = TSingleLineFlow.BeginGroupIndicator;
            pnlFindOptions.Controls.Add(btnFindNext);
            pnlFindOptions.Controls.Add(grpFindDirection);           
            
            FPnlFindControls.Controls.Add(pnlFindOptions);
            
            //
            // FTabFilterAndFind
            //
            FTabFilterAndFind.BackColor = System.Drawing.Color.LightSteelBlue;
            FTabFilterAndFind.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            FTabFilterAndFind.Controls.Add(FTbpFilter);
            FTabFilterAndFind.Controls.Add(FTbpFind);
            FTabFilterAndFind.Dock = System.Windows.Forms.DockStyle.Fill;
            FTabFilterAndFind.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            FTabFilterAndFind.Font = new System.Drawing.Font("Verdana",
                7F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            FTabFilterAndFind.Location = new System.Drawing.Point(0, 0);
            FTabFilterAndFind.Name = "tabFilterAndFind";
            FTabFilterAndFind.SelectedIndex = 0;
            FTabFilterAndFind.ShowToolTips = true;
            FTabFilterAndFind.TabIndex = 10;
            FTabFilterAndFind.SelectedIndexChanged += delegate(object sender, EventArgs e) { FTabFilterAndFind_SelectedIndexChanged(sender, e); };

            //
            // FTbpFilter
            //
            FTbpFilter.BackColor = System.Drawing.Color.LightSteelBlue;
            FTbpFilter.Font =
                new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            FTbpFilter.Location = new System.Drawing.Point(4, 4);
            FTbpFilter.Name = "tbpFilter";
            FTbpFilter.Padding = new System.Windows.Forms.Padding(6, 3, 2, 3);
            FTbpFilter.Size = new System.Drawing.Size(553, 157);
            FTbpFilter.TabIndex = 0;
            FTbpFilter.Text = "Filter";
            FTbpFilter.ToolTipText = Catalog.GetString("Filter the rows shown in the list");

            LayoutManagerFilterTab = new TSingleLineFlow(FTbpFilter, 4, 3);             
            LayoutManagerFilterTab.SpacerDistance = 7;
            
            FTbpFilter.Controls.Add(pnlFilterControls);

            if (ShowExtraFilter) 
            {
                FTbpFilter.Controls.Add(pnlExtraFilterControls);    
            }

            //
            // FTbpFind
            //
            FTbpFind.BackColor = System.Drawing.Color.LightSteelBlue;
            FTbpFind.Controls.Add(FPnlFindControls);
            FTbpFind.Font =
                new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            FTbpFind.Location = new System.Drawing.Point(4, 4);
            FTbpFind.Name = "tbpFind";
            FTbpFind.Padding = new System.Windows.Forms.Padding(6, 3, 2, 3);
            FTbpFind.Size = new System.Drawing.Size(553, 157);
            FTbpFind.TabIndex = 0;
            FTbpFind.Text = "Find";
            FTbpFind.ToolTipText = Catalog.GetString("Find within the rows shown in the list");
            
            LayoutManagerFindTab = new TSingleLineFlow(FTbpFind, 4, 3);             
            LayoutManagerFindTab.SpacerDistance = 7;
            
            FTabFilterAndFind.Dock = DockStyle.Fill;
            
            this.Controls.Clear();
            pnlTitle.Dock = DockStyle.Top;
            this.Controls.Add(pnlTitle);
            this.Controls.Add(FTabFilterAndFind);
            FTabFilterAndFind.BringToFront();
            
            FPnlFindControls.Top = pnlFilterControls.Top - 22;


            // Add individual 'Argument Panels' Panels to the 'Find' Panel (on 'Find' Tab)
            // Layout is taken care of automatically due to a TSingleLineFlow Layout Manager!
            foreach (Panel ArgumentPanel in FFindControls) 
            {
                FPnlFindControls.Controls.Add(ArgumentPanel);
            }            

            FTabFilterAndFind.ResumeLayout();
            FPnlFindControls.ResumeLayout();
            pnlFindOptions.ResumeLayout();
            grpFindDirection.ResumeLayout();                        
            this.ResumeLayout();
            
            // Ensure that pnlFindOptions is always the bottommost of the Controls in the Panel
            FPnlFindControls.Controls.SetChildIndex(pnlFindOptions, FPnlFindControls.Controls.Count);           
        }

        #endregion

        #region Event Handlers        
        
        void BtnCloseFilterClick(object sender, System.EventArgs e)
        {
            this.Width = 0;
        }
        
        void FTabFilterAndFind_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ((FShowFindPanel)
            && ((FTabFilterAndFind.SelectedIndex == 1)
                && (FTabFilterAndFind.Tag == null)))
            {
                AutoSizePanelHeight(FPnlFindControls);    
                FTabFilterAndFind.Tag = "AutoSized";
            }
        }
        
        #endregion
    }
}