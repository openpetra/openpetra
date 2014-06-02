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
using System.Data;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Controls.Formatting;

using Owf.Controls;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// UserControl that handles one or two Filter Panels and optionally
    /// a Find Panel.
    /// </summary>
    public partial class TUcoFilterAndFind : UserControl
    {
        /// <summary>
        /// Used to specify colours for several aspects of a TUcoFilterAndFind.
        /// </summary>
        public struct ColourInformation
        {
            private Color FFilterColour;
            private Color FFindColour;

            /// <summary>
            /// Colour of the Filter panel.
            /// </summary>
            public Color FilterColour
            {
                get
                {
                    return FFilterColour;
                }

                set
                {
                    FFilterColour = value;
                }
            }

            /// <summary>
            /// Colour of the Find panel.
            /// </summary>
            public Color FindColour
            {
                get
                {
                    return FFindColour;
                }

                set
                {
                    FFindColour = value;
                }
            }
        }

        private Color FFilterColour;
        private Color FFindColour;
        private static ColourInformation FColourInfo;
        private static bool FColourInfoSetup = false;

        /// <summary>
        /// Used to refresh Filter Find panel colours after they have been changed in user preferences.
        /// </summary>
        public static bool ColourInfoSetup
        {
            set
            {
                FColourInfoSetup = value;
            }
        }

        /// <summary>
        /// Used for passing Colour information to the Filter Find. Re-used by all instances of the Filter Find!
        /// </summary>
        public static Func <ColourInformation>SetColourInformation;

        /// <summary>
        /// Defines to which context a specific setting applies.
        /// </summary>
        public enum FilterContext
        {
            /// <summary>No Filter Context (=do not show!).</summary>
            None,

            /// <summary>'Standard' Filter Context only (=not 'Extra' Filter Context).</summary>
            StandardFilterOnly,

            /// <summary>'Extra' Filter Context only (=not 'Standard' Filter Context).</summary>
            ExtraFilterOnly,

            /// <summary>Both 'Standard' and 'Extra' Filter Context.</summary>
            StandardAndExtraFilter
        }

        /// <summary>
        /// Defines the context in which an Event happened.
        /// </summary>
        public enum EventContext
        {
            /// <summary>
            /// Event happened on the 'Standard' Filter Panel.
            /// </summary>
            ecStandardFilterPanel,

            /// <summary>
            /// Event happened on the 'Extra' Filter Panel.
            /// </summary>
                ecExtraFilterPanel,

            /// <summary>
            /// Event happened on the Find Panel.
            /// </summary>
                ecFindPanel,

            /// <summary>
            /// Event happened on the 'Extra' Filter Panel.
            /// </summary>
                ecFilterTab,

            /// <summary>
            /// Event happened on the Find Panel.
            /// </summary>
                ecFindTab,

            /// <summary>
            /// Event happened on a Control that isn't known to the UserControl
            /// </summary>
                ecUnknownControl
        }

        /// <summary>
        ///  Structure for holding parameters for a <see cref="TUcoFilterAndFind" /> instance (that migth get created
        ///  at a later point).
        /// </summary>
        public struct FilterAndFindParameters
        {
            private int FFilterAndFindInitialWidth;
            private bool FFilterAndFindInitiallyExpanded;
            private FilterContext FApplyFilterButtonContext;
            private FilterContext FShowKeepFilterTurnedOnButtonContext;
            private FilterContext FShowFilterIsAlwaysOnLabelContext;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="AFilterAndFindInitialWidth">Initial Width of the Filter and Find Panel.</param>
            /// <param name="AFilterAndFindInitiallyExpanded">Wether the Filter and Find Panel are shown when the containing Form/UserControl
            /// is opened/shown.</param>
            /// <param name="AApplyFilterButtonContext">The context in which an 'Apply Filter' Button is shown (if any).</param>
            /// <param name="AShowKeepFilterTurnedOnButtonContext">The context in which a 'Keep Filter Turned On' Button is shown (if any).</param>
            /// <param name="AShowFilterIsAlwaysOnLabelContext">The context in which a 'Filter Is Always On' Label is shown (if any).</param>
            public FilterAndFindParameters(int AFilterAndFindInitialWidth, bool AFilterAndFindInitiallyExpanded,
                FilterContext AApplyFilterButtonContext, FilterContext AShowKeepFilterTurnedOnButtonContext,
                FilterContext AShowFilterIsAlwaysOnLabelContext)
            {
                FFilterAndFindInitialWidth = AFilterAndFindInitialWidth;
                FFilterAndFindInitiallyExpanded = AFilterAndFindInitiallyExpanded;
                FApplyFilterButtonContext = AApplyFilterButtonContext;
                FShowKeepFilterTurnedOnButtonContext = AShowKeepFilterTurnedOnButtonContext;
                FShowFilterIsAlwaysOnLabelContext = AShowFilterIsAlwaysOnLabelContext;
            }

            /// <summary>
            /// Initial Width of the Filter and Find Panel.
            /// </summary>
            public int FindAndFilterInitialWidth
            {
                get
                {
                    return FFilterAndFindInitialWidth;
                }
            }

            /// <summary>
            /// Wether the Filter and Find Panel are shown when the containing Form/UserControl
            /// is first opened/shown.
            /// </summary>
            public bool FindAndFilterInitiallyExpanded
            {
                get
                {
                    return FFilterAndFindInitiallyExpanded;
                }
            }

            /// <summary>
            /// The context in which an 'Apply Filter' Button is shown (if any).
            /// </summary>
            public FilterContext ApplyFilterButtonContext
            {
                get
                {
                    return FApplyFilterButtonContext;
                }
            }

            /// <summary>
            /// The context in which a 'Keep Filter Turned On' Button is shown (if any).
            /// </summary>
            public FilterContext ShowKeepFilterTurnedOnButtonContext
            {
                get
                {
                    return FShowKeepFilterTurnedOnButtonContext;
                }
            }

            /// <summary>
            /// The context in which a 'Filter Is Always On' Label is shown (if any).
            /// </summary>
            public FilterContext ShowFilterIsAlwaysOnLabelContext
            {
                get
                {
                    return FShowFilterIsAlwaysOnLabelContext;
                }
            }
        }

        /// <summary>
        /// Event Arguments for Events that are related to an <see cref="EventContext" />.
        /// </summary>
        public class TContextEventArgs : EventArgs
        {
            private EventContext FContext;
            private Action <Control>FAction = null;
            private Action <Control>FResetAction = null;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="AContext"></param>
            public TContextEventArgs(EventContext AContext)
            {
                FContext = AContext;
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="AContext"></param>
            /// <param name="AAction">The Action that is associated with the Event (optional!).</param>
            /// <param name="AResetAction">The Action that resets the Action that is associated with the Event (optional!).</param>
            public TContextEventArgs(EventContext AContext, Action <Control>AAction, Action <Control>AResetAction = null)
            {
                FContext = AContext;
                FAction = AAction;
                FResetAction = AResetAction;
            }

            /// <summary>
            /// The Context in which the Event happened.
            /// </summary>
            public EventContext Context
            {
                get
                {
                    return FContext;
                }
            }

            /// <summary>
            /// The Action that is associated with the Event (optional!).
            /// </summary>
            public Action <Control>Action
            {
                get
                {
                    return FAction;
                }
            }

            /// <summary>
            /// The Action that resets the Action that is associated with the Event (optional!).
            /// </summary>
            public Action <Control>ResetAction
            {
                get
                {
                    return FResetAction;
                }
            }
        }

        /// <summary>
        /// Event Arguments for Events that are related to an <see cref="EventContext" /> and
        /// who are sent by a Button that can be in a depressed and non-depressed state.
        /// </summary>
        public class TContextEventExtButtonDepressedArgs : TContextEventArgs
        {
            private bool FButtonIsDepressed;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="AContext">Context in which the Button got pressed.</param>
            /// <param name="AButtonIsDepressed">Whether the Button is in a depressed or non-depressed state.</param>
            public TContextEventExtButtonDepressedArgs(EventContext AContext, bool AButtonIsDepressed) : base(AContext)
            {
                FButtonIsDepressed = AButtonIsDepressed;
            }

            /// <summary>
            /// Whether the Button that sends this Event is in a depressed or non-depressed state.
            /// </summary>
            public bool ButtonIsDepressed
            {
                get
                {
                    return FButtonIsDepressed;
                }
            }
        }

        /// <summary>
        /// Event Arguments for Events that are fired when the 'Find Next' Button is pressed.
        /// </summary>
        public class TContextEventExtSearchDirectionArgs : TContextEventArgs
        {
            private bool FSearchUpwards;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="AContext">Context in which the 'Find Next' Button got pressed.</param>
            /// <param name="ASearchUpwards">Whether the search direction is 'upwards' or 'downwards'.</param>
            public TContextEventExtSearchDirectionArgs(EventContext AContext, bool ASearchUpwards) : base(AContext)
            {
                FSearchUpwards = ASearchUpwards;
            }

            /// <summary>
            /// Whether the search direction is 'upwards' or 'downwards'.
            /// </summary>
            public bool SearchUpwards
            {
                get
                {
                    return FSearchUpwards;
                }
            }
        }

        /// <summary>
        /// Event Arguments for Events that are related to a <see cref="System.Windows.Forms.Control" />.
        /// </summary>
        public class TContextEventExtControlArgs : TContextEventArgs
        {
            private Control FAffectedControl;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="AContext">Context in which the Event was fired.</param>
            /// <param name="AAffectedControl">The Control that is affected by the Event.</param>
            public TContextEventExtControlArgs(EventContext AContext, Control AAffectedControl) : base(AContext)
            {
                FAffectedControl = AAffectedControl;
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="AContext">Context in which the Event was fired.</param>
            /// <param name="AAffectedControl">The Control that is affected by the Event.</param>
            /// <param name="AAction">The Action that is associated with the Event (optional!).</param>
            /// <param name="AResetAction">The Action that resets the Action that is associated with the Event (optional!).</param>
            public TContextEventExtControlArgs(EventContext AContext,
                Control AAffectedControl,
                Action <Control>AAction,
                Action <Control>AResetAction = null) : base(AContext, AAction, AResetAction)
            {
                FAffectedControl = AAffectedControl;
            }

            /// <summary>
            /// The Control that is affected by the Event.
            /// </summary>
            public Control AffectedControl
            {
                get
                {
                    return FAffectedControl;
                }
            }
        }

        /// <summary>
        /// Event Arguments for Events that are related to a <see cref="System.Windows.Forms.Control" />
        /// and that transport a value.
        /// </summary>
        public class TContextEventExtControlValueArgs : TContextEventExtControlArgs
        {
            private object FValue;
            private System.Type FTypeOfValue;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="AContext">Context in which the Event was fired.</param>
            /// <param name="AAffectedControl">The Control that is affected by the Event.</param>
            /// <param name="AValue">The Value that is associated with the affected Control.</param>
            /// <param name="ATypeOfValue">The <see cref="System.Type" /> of the Value that is associated with the affected Control.</param>
            public TContextEventExtControlValueArgs(EventContext AContext, Control AAffectedControl,
                object AValue, System.Type ATypeOfValue) : base(AContext, AAffectedControl)
            {
                FValue = AValue;
                FTypeOfValue = ATypeOfValue;
            }

            /// <summary>
            /// The Value that is associated with the affected Control.
            /// </summary>
            public object Value
            {
                get
                {
                    return FValue;
                }
            }

            /// <summary>
            /// The <see cref="System.Type" /> of the Value that is associated with the affected Control.
            /// </summary>
            public System.Type TypeOfValue
            {
                get
                {
                    return FTypeOfValue;
                }
            }
        }

        private const string BTN_APPLY_FILTER_NAME = "btnApplyFilter";
        private const string BTN_KEEP_FILTER_TURNED_ON_NAME = "btnKeepFilterTurnedOn";
        private const string LBL_FILTER_IS_ALWAYS_TURNED_ON_NAME = "lblFilterIsAlwaysTurned On";
        private readonly string StrApplyFilter = Catalog.GetString("Appl&y Filter");
        private readonly string StrApplyingFilter = Catalog.GetString("Applying Filter...");

        private int FInitialWidth = 150;
        private List <Panel>FFilterControls;
        private List <Panel>FExtraFilterControls;
        private List <Panel>FFindControls;
        private bool FShowExtraFilter = false;
        private bool FShowFindPanel = false;
        private FilterContext FShowApplyFilterButton = FilterContext.None;
        private FilterContext FShowKeepFilterTurnedOnButton = FilterContext.None;
        private FilterContext FShowFilterIsAlwaysTurnedOnLabel = FilterContext.None;
        private Owf.Controls.A1Panel FPnlFindControls;
        private Ict.Common.Controls.TTabVersatile FTabFilterAndFind = new TTabVersatile();
        private System.Windows.Forms.TabPage FTbpFilter = new System.Windows.Forms.TabPage();
        private System.Windows.Forms.TabPage FTbpFind = new System.Windows.Forms.TabPage();
        TSingleLineFlow FLayoutManagerFilterControls;
        Control FFilterPanelFirstArgumentControl;
        Control FExtraFilterPanelFirstArgumentControl;
        Control FFindPanelFirstArgumentControl;
        FilterContext FKeepFilterTurnedOnButtonDepressed = FilterContext.None;

        private bool FIgnoreValueChangedEvent = false;

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
            this.btnCloseFilter.Text = Catalog.GetString("X");
            this.lblTitle.Text = Catalog.GetString("List Filter");
            #endregion
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TUcoFilterAndFind(List <Panel>AFilterControls, List <Panel>AExtraFilterControls, List <Panel>AFindControls,
            FilterAndFindParameters AParameters) : this(AFilterControls, AExtraFilterControls, AFindControls,
                                                        AParameters.ApplyFilterButtonContext, AParameters.ShowKeepFilterTurnedOnButtonContext,
                                                        AParameters.ShowFilterIsAlwaysOnLabelContext, AParameters.FindAndFilterInitialWidth)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TUcoFilterAndFind(List <Panel>AFilterControls, List <Panel>AExtraFilterControls, List <Panel>AFindControls,
            FilterContext AShowApplyFilterButton = FilterContext.None,
            FilterContext AShowKeepFilterTurnedOnButton = FilterContext.None,
            FilterContext AShowFilterIsAlwaysOnLabel = FilterContext.None,
            int AWidth = 150)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            FFilterControls = AFilterControls;
            FExtraFilterControls = AExtraFilterControls;
            FFindControls = AFindControls;

            FShowApplyFilterButton = AShowApplyFilterButton;
            FShowKeepFilterTurnedOnButton = AShowKeepFilterTurnedOnButton;
            FShowFilterIsAlwaysTurnedOnLabel = AShowFilterIsAlwaysOnLabel;

            this.Width = AWidth;
            FInitialWidth = AWidth;

            InitUserControlInternal();
        }

        #endregion

        #region Properties

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
        /// Sets the context in which a 'Keep Filter Turned On' Button is shown (if any).
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
        /// Sets the context in which a 'Filter Is Always On' Label is shown (if any).
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
        /// Gets which 'Keep Filter Turned On' button is currently depressed (if any).
        /// </summary>
        public FilterContext KeepFilterTurnedOnButtonDepressed
        {
            get
            {
                return FKeepFilterTurnedOnButtonDepressed;
            }
        }

        /// <summary>
        /// Whether the Control is collapsed.
        /// </summary>
        public bool IsCollapsed
        {
            get
            {
                return this.Width == 0;
            }
        }

        /// <summary>
        /// Whether the 'Extra Filter' Panel is shown.
        /// </summary>
        /// <returns></returns>
        public bool IsExtraFilterShown
        {
            get
            {
                return FShowExtraFilter;
            }
        }

        /// <summary>
        /// Whether a TabControl with both Filter and Find options is shown.
        /// </summary>
        /// <returns></returns>
        public bool IsFindTabShown
        {
            get
            {
                return FShowFindPanel;
            }
        }

        /// <summary>
        /// Whether a TabControl with both Filter and Find options is shown and whether the Find tab is the active tab.
        /// </summary>
        /// <returns></returns>
        public bool IsFindTabActive
        {
            get
            {
                return FShowFindPanel && (FTabFilterAndFind.SelectedIndex == 1);
            }
        }

        /// <summary>
        /// Returns True if a ComboBox is firing a SelectedValueChanged event that should be ignored.
        /// ComboBoxes that are being cleared fire an event for index 0 and then another for index -1.
        /// </summary>
        public bool CanIgnoreChangeEvent
        {
            get
            {
                return FIgnoreValueChangedEvent;
            }
        }

        /// <summary>
        /// Kludge required for TCmbAutoComplete: This control requires 2 calls to set the selectedIndex to -1.
        /// This property is set to true before the first call and false afterwards.
        /// </summary>
        /// <returns></returns>
        public bool IgnoreValueChangedEvent
        {
            get
            {
                return FIgnoreValueChangedEvent;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the UserControl got expanded.
        /// </summary>
        public event EventHandler Expanded;

        /// <summary>
        /// Raised when the UserControl got collapsed.
        /// </summary>
        public event EventHandler Collapsed;

        /// <summary>
        /// Raised when the Find Tab is displayed on request by calling Method <see cref="DisplayFindTab" />.
        /// </summary>
        public event EventHandler FindTabDisplayed;

        /// <summary>
        /// Raised when the 'Apply Filter' Button has been clicked.
        /// </summary>
        public event EventHandler <TContextEventExtControlArgs>ApplyFilterClicked;

        /// <summary>
        /// Raised when the 'Keep Filter Turned On' Button has been clicked.
        /// </summary>
        public event EventHandler <TContextEventExtButtonDepressedArgs>KeepFilterTurnedOnClicked;

        /// <summary>
        /// Raised when the 'Find Next' Button has been clicked.
        /// </summary>
        public event EventHandler <TContextEventExtSearchDirectionArgs>FindNextClicked;

        /// <summary>
        /// Raised when a 'Clear Argument' Button has been clicked.
        /// </summary>
        public event EventHandler <TContextEventExtControlArgs>ClearArgumentCtrlButtonClicked;

        /// <summary>
        /// Raised when the Tab of the TabControl is changed.
        /// </summary>
        public event EventHandler <TContextEventArgs>TabSwitched;

        /// <summary>
        /// Raised when a value of an Argument Control has changed.
        /// </summary>
        public event EventHandler <TContextEventExtControlValueArgs>ArgumentCtrlValueChanged;

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows the Find Tab if it is available (<see cref="ShowFindTab" /> needs to have been called).
        /// If the Control is collapsed it will be expanded automatically, too.
        /// </summary>
        public void DisplayFindTab()
        {
            if (FShowFindPanel)
            {
                FTabFilterAndFind.SelectedTab = FTbpFind;

                // Focus 'first' Argument Control
                if (FFindPanelFirstArgumentControl != null)
                {
                    FFindPanelFirstArgumentControl.Focus();
                }

                OnFindTabDisplayed();
            }

            if (IsCollapsed)
            {
                Expand();
            }
        }

        /// <summary>
        /// Displays the filter tab
        /// </summary>
        public void DisplayFilterTab()
        {
            if (FTabFilterAndFind.TabCount > 0)
            {
                this.FTabFilterAndFind.SelectedIndex = 0;
            }

            if (IsCollapsed)
            {
                Expand();
            }
        }

        /// <summary>
        /// Expands (=shows) the Filter/Find Panel to its original width.
        /// </summary>
        public void Expand()
        {
            if (this.Width != FInitialWidth)
            {
                this.Width = FInitialWidth;
                this.Visible = true;

                FocusFirstArgumentControl();

                OnExpanded();
            }
        }

        /// <summary>
        /// Collapses (=hides) the Filter/Find Panel.
        /// </summary>
        public void Collapse()
        {
            if (this.Width != 0)
            {
                this.Width = 0;

                OnCollapsed();
            }
        }

        /// <summary>
        /// Focuses the first Argument Control.
        /// </summary>
        public void FocusFirstArgumentControl()
        {
            if (FShowFindPanel)
            {
                if (FTabFilterAndFind.SelectedTab == FTbpFind)
                {
                    if (FFindPanelFirstArgumentControl != null)
                    {
                        FFindPanelFirstArgumentControl.Focus();
                        return;
                    }
                }
            }

            if (FFilterPanelFirstArgumentControl != null)
            {
                FFilterPanelFirstArgumentControl.Focus();
            }
            else if (FExtraFilterPanelFirstArgumentControl != null)
            {
                FExtraFilterPanelFirstArgumentControl.Focus();
            }
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
            FLayoutManagerFilterControls.SpacerDistance = 5;

            if ((FExtraFilterControls != null)
                && (FExtraFilterControls.Count > 0))
            {
                FShowExtraFilter = true;
            }

            if (FShowExtraFilter)
            {
                // Layout Manager for the 'Extra' Filter Panel.
                // This will arrange 'Argument Panels' that will be added later to the 'Extra' Filter Panel.
                LayoutManagerExtraFilterControls = new TSingleLineFlow(pnlExtraFilterControls, 4, 3);
                LayoutManagerExtraFilterControls.TopMargin = 5;
                LayoutManagerExtraFilterControls.RightMargin = 9;
                LayoutManagerExtraFilterControls.SpacerDistance = 5;

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
                    ProcessArgumentPanel(ArgumentPanel, pnlFilterControls.Width, false);

                    pnlFilterControls.Controls.Add(ArgumentPanel);

                    if (FFilterPanelFirstArgumentControl == null)
                    {
                        FFilterPanelFirstArgumentControl = DetermineFirstArgumentControl(ArgumentPanel);
                    }
                }
            }

            // Add individual 'Argument Panels' to the 'Extra Filter' Panel (below 'Standard Filter' Panel on 'Filter' Tab)
            // Layout is taken care of automatically due to a TSingleLineFlow Layout Manager!
            if (FExtraFilterControls != null)
            {
                foreach (Panel ArgumentPanel in FExtraFilterControls)
                {
                    ProcessArgumentPanel(ArgumentPanel, pnlExtraFilterControls.Width, false);

                    pnlExtraFilterControls.Controls.Add(ArgumentPanel);

                    if (FExtraFilterPanelFirstArgumentControl == null)
                    {
                        FExtraFilterPanelFirstArgumentControl = DetermineFirstArgumentControl(ArgumentPanel);
                    }
                }
            }

            UpdateKeepFilterTurnedOnButtons(false);
            UpdateFilterIsAlwaysTurnedOnLabels(false);
            UpdateFilterApplyButtons(false);

            UpdateExtraFilterDisplay();

            SetupTitleText();
            pnlTitle.SetDoubleBuffered(true);

            if ((FFindControls != null)
                && (FFindControls.Count > 0))
            {
                ShowFindTab();
            }

            // Reverse the Z-Order of the Panels so they 'stack up' correctly
            pnlFilterControls.SendToBack();
            pnlExtraFilterControls.SendToBack();

            this.Invalidate();

            AutoSizeFilterPanelsHeights();

            // set the panel colours
            SetPanelColours();
        }

        /// <summary>
        /// Shows a TabControl with both Filter and Find options.
        /// </summary>
        private void ShowFindTab()
        {
            FShowFindPanel = true;

            AddTabs();

            SetupTitleText();
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

        private Control DetermineFirstArgumentControl(Panel AArgumentPanel)
        {
            foreach (Control IndivControl in AArgumentPanel.Controls)
            {
                if ((!(IndivControl is Label))
                    && (!(IndivControl is Button)))
                {
                    return IndivControl;
                }
            }

            return null;
        }

        #region 'Apply Filter' Button adding/removal

        private void UpdateFilterApplyButtons(bool AAutoSizePanels = true)
        {
            switch (FShowApplyFilterButton)
            {
                case TUcoFilterAndFind.FilterContext.None:

                    RemoveButtonApplyFilter(pnlFilterControls);
                    RemoveButtonApplyFilter(pnlExtraFilterControls);

                    break;

                case TUcoFilterAndFind.FilterContext.StandardFilterOnly:

                    AddButtonApplyFilter(pnlFilterControls);
                    RemoveButtonApplyFilter(pnlExtraFilterControls);

                    break;

                case TUcoFilterAndFind.FilterContext.ExtraFilterOnly:

                    AddButtonApplyFilter(pnlExtraFilterControls);
                    RemoveButtonApplyFilter(pnlFilterControls);

                    break;

                case TUcoFilterAndFind.FilterContext.StandardAndExtraFilter:

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
            Action <Control>FilterIsBeingApplied = null;
            Action <Control>FilterApplyingFinished = null;


            ControlsArray = AFilterPanel.Controls.Find(BTN_APPLY_FILTER_NAME, false);

            if (ControlsArray.Length == 0)
            {
                BtnApplyFilter = new Button();
                BtnApplyFilter.Name = BTN_APPLY_FILTER_NAME;

                BtnApplyFilter.Left = 5;
                BtnApplyFilter.Text = StrApplyFilter;
                BtnApplyFilter.BackColor = System.Drawing.SystemColors.ButtonFace;
                BtnApplyFilter.ImageList = imlButtonIcons;
                BtnApplyFilter.ImageIndex = 2;
                BtnApplyFilter.ImageAlign = ContentAlignment.MiddleRight;

                FilterIsBeingApplied += delegate(Control obj) {
                    SetApplyFilterButtonState(obj, "APPLYING");
                };
                FilterApplyingFinished += delegate(Control obj) {
                    SetApplyFilterButtonState(obj, "FINISHED");
                };

                BtnApplyFilter.Click += delegate(object sender, EventArgs e) {
                    OnApplyFilterClicked(sender, FilterIsBeingApplied, FilterApplyingFinished);
                };

                tipGeneral.SetToolTip(BtnApplyFilter, "Click to filter the data");

                AFilterPanel.Controls.Add(BtnApplyFilter);

                // In case there is no 'Keep Filter Turned On' Button and no 'Filter Always Turned On' Label
                if ((AFilterPanel.Controls.Find(BTN_KEEP_FILTER_TURNED_ON_NAME, false).Length == 0)
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
                case TUcoFilterAndFind.FilterContext.None:

                    RemoveButtonKeepFilterTurnedOn(pnlFilterControls);
                    RemoveButtonKeepFilterTurnedOn(pnlExtraFilterControls);

                    break;

                case TUcoFilterAndFind.FilterContext.StandardFilterOnly:

                    AddButtonKeepFilterTurnedOn(pnlFilterControls);
                    RemoveButtonKeepFilterTurnedOn(pnlExtraFilterControls);

                    break;

                case TUcoFilterAndFind.FilterContext.ExtraFilterOnly:

                    AddButtonKeepFilterTurnedOn(pnlExtraFilterControls);
                    RemoveButtonKeepFilterTurnedOn(pnlFilterControls);

                    break;

                case TUcoFilterAndFind.FilterContext.StandardAndExtraFilter:

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
                BtnKeepFilterTurnedOn.Tag = CommonTagString.SUPPRESS_CHANGE_DETECTION + ";" + TSingleLineFlow.BeginGroupIndicator;
                BtnKeepFilterTurnedOn.FlatStyle = FlatStyle.System;               // this is set so that the Button doesn't let the background colour shine through, but uses the system's colours and uses a gradient!
                BtnKeepFilterTurnedOn.Appearance = Appearance.Button;
                BtnKeepFilterTurnedOn.TextAlign = ContentAlignment.MiddleCenter;  // Same as 'real' Button
                BtnKeepFilterTurnedOn.MinimumSize = new Size(75, 22);             // To prevent shrinkage!
                BtnKeepFilterTurnedOn.Click += delegate(object sender, EventArgs e) {
                    OnKeepFilterTurnedOnClicked(sender, e);
                };

                tipGeneral.SetToolTip(BtnKeepFilterTurnedOn, "Depress to keep the filter active\r\neven when the Filter Panel is closed");

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
                    if (AFilterPanel.Controls.GetChildIndex(BtnKeepFilterTurnedOn) == AFilterPanel.Controls.Count - 1)
                    {
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

                if (FShowFilterIsAlwaysTurnedOnLabel == FilterContext.StandardAndExtraFilter)
                {
                    if (AFilterPanel == pnlFilterControls)
                    {
                        FShowFilterIsAlwaysTurnedOnLabel = FilterContext.ExtraFilterOnly;
                    }
                    else
                    {
                        FShowFilterIsAlwaysTurnedOnLabel = FilterContext.StandardFilterOnly;
                    }
                }
                else if ((FShowFilterIsAlwaysTurnedOnLabel == FilterContext.StandardFilterOnly)
                         || (FShowFilterIsAlwaysTurnedOnLabel == FilterContext.ExtraFilterOnly))
                {
                    FShowFilterIsAlwaysTurnedOnLabel = FilterContext.None;
                }
            }

            UpdateKeepFilterTurnedOnButtonDepressedState();
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

            UpdateKeepFilterTurnedOnButtonDepressedState();
        }

        #endregion

        #region 'Filter Always Turned On' Label adding/removal

        private void UpdateFilterIsAlwaysTurnedOnLabels(bool AAutoSizePanel = true)
        {
            switch (FShowFilterIsAlwaysTurnedOnLabel)
            {
                case TUcoFilterAndFind.FilterContext.None:

                    RemoveLabelFilterIsAlwaysTurnedOn(pnlFilterControls);
                    RemoveLabelFilterIsAlwaysTurnedOn(pnlExtraFilterControls);

                    break;

                case TUcoFilterAndFind.FilterContext.StandardFilterOnly:

                    AddLabelFilterIsAlwaysTurnedOn(pnlFilterControls);
                    RemoveLabelFilterIsAlwaysTurnedOn(pnlExtraFilterControls);

                    break;

                case TUcoFilterAndFind.FilterContext.ExtraFilterOnly:

                    AddLabelFilterIsAlwaysTurnedOn(pnlExtraFilterControls);
                    RemoveLabelFilterIsAlwaysTurnedOn(pnlFilterControls);

                    break;

                case TUcoFilterAndFind.FilterContext.StandardAndExtraFilter:

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

                tipGeneral.SetToolTip(LblFilterIsAlwaysTurnedOn, "This filter will be kept active even\r\nwhen the Filter Panel is closed!");

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

                if (FShowKeepFilterTurnedOnButton == FilterContext.StandardAndExtraFilter)
                {
                    if (AFilterPanel == pnlFilterControls)
                    {
                        FShowKeepFilterTurnedOnButton = FilterContext.ExtraFilterOnly;
                    }
                    else
                    {
                        FShowKeepFilterTurnedOnButton = FilterContext.StandardFilterOnly;
                    }
                }
                else if ((FShowKeepFilterTurnedOnButton == FilterContext.StandardFilterOnly)
                         || (FShowKeepFilterTurnedOnButton == FilterContext.ExtraFilterOnly))
                {
                    FShowKeepFilterTurnedOnButton = FilterContext.None;
                }
            }

            UpdateKeepFilterTurnedOnButtonDepressedState();
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

            UpdateKeepFilterTurnedOnButtonDepressedState();
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

            if (APanel.Controls.Count > 0)
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
            Button btnFindNext = new Button();
            GroupBox grpFindDirection = new GroupBox();
            RadioButton rbtFindDirUp = new RadioButton();
            RadioButton rbtFindDirDown = new RadioButton();
            TSingleLineFlow LayoutManagerFindControls;
            TSingleLineFlow LayoutManagerFilterTab;
            TSingleLineFlow LayoutManagerFindTab;

            this.SuspendLayout();
            grpFindDirection.SuspendLayout();
            pnlFindOptions.SuspendLayout();
            FTabFilterAndFind.SuspendLayout();

            FPnlFindControls = new Owf.Controls.A1Panel();
            FPnlFindControls.SuspendLayout();
            FPnlFindControls.Name = "FPnlFindControls";
            FPnlFindControls.Left = 7;
            FPnlFindControls.Top = 8;
            FPnlFindControls.Width = FInitialWidth - 5;
            FPnlFindControls.Height = 174;
            FPnlFindControls.BorderColor = System.Drawing.Color.CadetBlue;
            FPnlFindControls.ShadowOffSet = 4;
            FPnlFindControls.RoundCornerRadius = 4;
            FPnlFindControls.GradientDirection = LinearGradientMode.Horizontal;

            // Layout Manager for the 'Find' Panel.
            // This will arrange 'Argument Panels' that will be added later to the 'Find' Panel.
            LayoutManagerFindControls = new TSingleLineFlow(FPnlFindControls, 4, 3);
            LayoutManagerFindControls.TopMargin = 5;
            LayoutManagerFindControls.RightMargin = 9;
            LayoutManagerFindControls.SpacerDistance = 3;

            btnFindNext.Top = 2;
            btnFindNext.Left = 1;
            btnFindNext.Text = "Find Ne&xt";
            btnFindNext.Name = "btnFindNext";
            btnFindNext.BackColor = System.Drawing.SystemColors.ButtonFace;
            btnFindNext.ImageList = imlButtonIcons;
            btnFindNext.ImageIndex = 4;
            btnFindNext.ImageAlign = ContentAlignment.MiddleRight;
            btnFindNext.Click += delegate(object sender, EventArgs e) {
                OnFindNextClicked(sender, e);
            };

            tipGeneral.SetToolTip(btnFindNext, "Click to find the next occurance\r\nin the search direction");

            rbtFindDirUp.Top = 14;
            rbtFindDirUp.Left = 10;
            rbtFindDirUp.AutoSize = true;
            rbtFindDirUp.Name = "rbtFindDirUp";
            rbtFindDirUp.Text = Catalog.GetString("&Up");
            rbtFindDirUp.Tag = CommonTagString.SUPPRESS_CHANGE_DETECTION;

            rbtFindDirDown.Top = 14;
            rbtFindDirDown.Left = 60;
            rbtFindDirDown.AutoSize = true;
            rbtFindDirDown.Checked = true;
            rbtFindDirDown.Name = "rbtFindDirDown";
            rbtFindDirDown.Text = Catalog.GetString("D&own");
            rbtFindDirDown.Tag = CommonTagString.SUPPRESS_CHANGE_DETECTION;

            grpFindDirection.Top = 30;
            grpFindDirection.Left = 3;
            grpFindDirection.Height = 38;
            grpFindDirection.BackColor = System.Drawing.Color.Transparent;
            grpFindDirection.Name = "grpFindDirection";
            grpFindDirection.Text = "Direction";
            grpFindDirection.Controls.Add(rbtFindDirUp);
            grpFindDirection.Controls.Add(rbtFindDirDown);

            pnlFindOptions.Name = "FPnlFindControls";
            pnlFindOptions.Left = 0;
            pnlFindOptions.Width = FTabFilterAndFind.Width;
            pnlFindOptions.Height = 72;
            pnlFindOptions.BackColor = System.Drawing.Color.Transparent;
            pnlFindOptions.Tag = TSingleLineFlow.BeginGroupIndicator;
            pnlFindOptions.Controls.Add(btnFindNext);
            pnlFindOptions.Controls.Add(grpFindDirection);
            pnlFindOptions.SetDoubleBuffered(true);

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
            FTabFilterAndFind.SelectedIndexChanged += delegate(object sender, EventArgs e) {
                FTabFilterAndFind_SelectedIndexChanged(sender, e);
            };

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
            LayoutManagerFilterTab.SpacerDistance = 5;

            FTbpFilter.Controls.Add(pnlFilterControls);

            if (IsExtraFilterShown)
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
            LayoutManagerFindTab.SpacerDistance = 5;

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
                ProcessArgumentPanel(ArgumentPanel, FPnlFindControls.Width, true);

                FPnlFindControls.Controls.Add(ArgumentPanel);

                if (FFindPanelFirstArgumentControl == null)
                {
                    FFindPanelFirstArgumentControl = DetermineFirstArgumentControl(ArgumentPanel);
                }
            }

            FTabFilterAndFind.ResumeLayout();
            FPnlFindControls.ResumeLayout();
            pnlFindOptions.ResumeLayout();
            grpFindDirection.ResumeLayout();
            this.ResumeLayout();

            btnFindNext.Width = FInitialWidth - 31;
            grpFindDirection.Width = FInitialWidth - 34;

            // Ensure that pnlFindOptions is always the bottommost of the Controls in the Panel
            FPnlFindControls.Controls.SetChildIndex(pnlFindOptions, FPnlFindControls.Controls.Count);
        }

        // set the panel colours
        private void SetPanelColours()
        {
            // set colours from user preferences
            if (SetColourInformation != null)
            {
                if (!FColourInfoSetup)
                {
                    FColourInfo = SetColourInformation();
                    FColourInfoSetup = true;
                }
            }
            else
            {
                FColourInfo.FilterColour = System.Drawing.Color.LightBlue;
                FColourInfo.FindColour = System.Drawing.Color.BurlyWood;
            }

            this.FFilterColour = FColourInfo.FilterColour;
            this.FFindColour = FColourInfo.FindColour;

            // panels have a gradient colour so an end colour must be defined
            int FilterRed = 0;
            int FilterGreen = 0;
            int FilterBlue = 0;

            if (FFilterColour.R >= 30)
            {
                FilterRed = FFilterColour.R - 30;
            }

            if (FFilterColour.G >= 30)
            {
                FilterGreen = FFilterColour.G - 30;
            }

            if (FFilterColour.B >= 30)
            {
                FilterBlue = FFilterColour.B - 30;
            }

            int FindRed = 0;
            int FindGreen = 0;
            int FindBlue = 0;

            if (FFindColour.R >= 30)
            {
                FindRed = FFindColour.R - 30;
            }

            if (FFindColour.G >= 30)
            {
                FindGreen = FFindColour.G - 30;
            }

            if (FFindColour.B >= 30)
            {
                FindBlue = FFindColour.B - 30;
            }

            System.Drawing.Color FilterGradientEndColor = Color.FromArgb(FilterRed, FilterGreen, FilterBlue);
            System.Drawing.Color FindGradientEndColor = Color.FromArgb(FindRed, FindGreen, FindBlue);

            pnlFilterControls.GradientStartColor = FFilterColour;
            pnlFilterControls.GradientEndColor = FilterGradientEndColor;

            if (FPnlFindControls != null)
            {
                FPnlFindControls.GradientStartColor = FFindColour;
                FPnlFindControls.GradientEndColor = FindGradientEndColor;
            }
        }

        private void ProcessArgumentPanel(Panel AArgumentPanel, int AContainerPanelWidth, bool AContainerPanelIsFindPanel)
        {
            string ArgumentPanelTag;
            bool KeepPanelBackColour = false;
            bool NoAutomaticArgumentClearButton = false;
            Control ProbeControl;
            Control ProbeControl2;
            TextBox ControlAsTextBox;
            ComboBox ControlAsComboBox;
            CheckBox ControlAsCheckBox;
            GroupBox ControlAsGroupBox;
            TtxtPetraDate ControlAsPetraDateBox;
            Button ClearArgumentCtrlButton;
            int ControlLeftOfButtonMaxWidth;
            int TopAdjustment = 0;

            if (AArgumentPanel.Tag != null)
            {
                ArgumentPanelTag = AArgumentPanel.Tag.ToString();
            }
            else
            {
                ArgumentPanelTag = String.Empty;
            }

            KeepPanelBackColour = ArgumentPanelTag.Contains(CommonTagString.ARGUMENTPANELTAG_KEEPBACKCOLOUR);
            NoAutomaticArgumentClearButton = ArgumentPanelTag.Contains(CommonTagString.ARGUMENTPANELTAG_NO_AUTOM_ARGUMENTCLEARBUTTON);

            // Remove any BackColour if it wasn't requested to keep it
            if (!KeepPanelBackColour)
            {
                AArgumentPanel.BackColor = System.Drawing.Color.Transparent;
            }

            foreach (Control ArgumentPanelCtrl in AArgumentPanel.Controls)
            {
                // Hook up 'value change' Events according to type of Control
                ControlAsTextBox = ArgumentPanelCtrl as TextBox;
                ControlAsPetraDateBox = ArgumentPanelCtrl as TtxtPetraDate;

                if (ControlAsPetraDateBox != null)
                {
                    ControlAsPetraDateBox.DateChanged += delegate(object sender, TPetraDateChangedEventArgs e) {
                        OnArgumentCtrlValueChanged(sender, e);
                    };
                }
                else if (ControlAsTextBox != null)
                {
                    ControlAsTextBox.TextChanged += delegate(object sender, EventArgs e) {
                        OnArgumentCtrlValueChanged(sender, e);
                    };
                }

                ControlAsComboBox = ArgumentPanelCtrl as ComboBox;

                if (ControlAsComboBox != null)
                {
                    ControlAsComboBox.TextChanged += delegate(object sender, EventArgs e)
                    {
                        OnArgumentCtrlValueChanged(sender, e);
                    };
                    ControlAsComboBox.SelectedValueChanged += delegate(object sender, EventArgs e)
                    {
                        OnArgumentCtrlValueChanged(sender, e);
                    };
                }

                ControlAsCheckBox = ArgumentPanelCtrl as CheckBox;

                if (ControlAsCheckBox != null)
                {
                    ControlAsCheckBox.CheckStateChanged += delegate(object sender, EventArgs e) {
                        OnArgumentCtrlValueChanged(sender, e);
                    };
                }

                ControlAsGroupBox = ArgumentPanelCtrl as GroupBox;

                if (ControlAsGroupBox != null)
                {
                    foreach (Control groupBoxCtrl in ControlAsGroupBox.Controls)
                    {
                        if (groupBoxCtrl is RadioButton)
                        {
                            ((RadioButton)groupBoxCtrl).CheckedChanged += delegate(object sender, EventArgs e)
                            {
                                if (((RadioButton)sender).Checked)
                                {
                                    OnArgumentCtrlValueChanged(sender, e);
                                }
                            };
                        }
                    }
                }

                // Create an 'argument clear Button' if it wasn't requested to not create one
                if (!NoAutomaticArgumentClearButton)
                {
                    ProbeControl = ArgumentPanelCtrl as Label;
                    ProbeControl2 = ArgumentPanelCtrl as Button;

                    // Check if we found a Control that isn't a Label and also not a Button
                    if ((ProbeControl == null)
                        && (ProbeControl2 == null))
                    {
                        if (ArgumentPanelCtrl is ComboBox)
                        {
                            TopAdjustment = 1;
                        }

                        if (ArgumentPanelCtrl is CheckBox)
                        {
                            TopAdjustment = -2;
                        }

                        ControlLeftOfButtonMaxWidth = AContainerPanelWidth - ArgumentPanelCtrl.Left - 20 - 18;

                        if (FShowFindPanel)
                        {
                            ControlLeftOfButtonMaxWidth -= 5;
                        }

                        if (AContainerPanelIsFindPanel)
                        {
                            //ControlLeftOfButtonMaxWidth -= 15;
                        }

                        // Check if the 'argument clear Button' will fit to the right of the Control
                        if (ArgumentPanelCtrl.Width > ControlLeftOfButtonMaxWidth)
                        {
                            // Reduce the width of the found Control so that we have space for the Button
                            ArgumentPanelCtrl.AutoSize = false;
                            ArgumentPanelCtrl.Width = ControlLeftOfButtonMaxWidth;
                        }

                        // Create a Button on-the-fly that will clear the value of the Control on the Argument Panel
                        ClearArgumentCtrlButton = new Button();
                        ClearArgumentCtrlButton.Left = ArgumentPanelCtrl.Left + ArgumentPanelCtrl.Width;
                        ClearArgumentCtrlButton.Top = ArgumentPanelCtrl.Top + 2 + TopAdjustment;
                        ClearArgumentCtrlButton.Width = 20;
                        ClearArgumentCtrlButton.Height = 20;
                        ClearArgumentCtrlButton.Name = "btnClearArgument_" + ArgumentPanelCtrl.Name;
                        ClearArgumentCtrlButton.FlatStyle = FlatStyle.Flat;
                        ClearArgumentCtrlButton.FlatAppearance.BorderSize = 0;
                        ClearArgumentCtrlButton.FlatAppearance.MouseOverBackColor = AArgumentPanel.BackColor;
                        ClearArgumentCtrlButton.ImageAlign = ContentAlignment.BottomCenter;
                        ClearArgumentCtrlButton.ImageList = imlButtonIcons;
                        ClearArgumentCtrlButton.ImageIndex = 0;  // start off with 'normal' appearance
                        ClearArgumentCtrlButton.TabStop = false;
                        ClearArgumentCtrlButton.MouseHover += delegate
                        {
                            ClearArgumentCtrlButton.ImageIndex = 1;
                            ClearArgumentCtrlButton.Cursor = System.Windows.Forms.Cursors.Hand;
                        };  // Turn the button to 'hot' appearance
                        ClearArgumentCtrlButton.MouseLeave += delegate
                        {
                            ClearArgumentCtrlButton.ImageIndex = 0;
                            ClearArgumentCtrlButton.Cursor = System.Windows.Forms.Cursors.Default;
                        };  // Turn the button back to 'normal' appearance
                        ClearArgumentCtrlButton.Tag = CommonTagString.SUPPRESS_CHANGE_DETECTION + ";" + ArgumentPanelCtrl.Name;

                        ClearArgumentCtrlButton.Click += delegate(object sender, EventArgs e)
                        {
                            ClearArgumentCtrlButtonClick(sender, e);
                        };

                        tipGeneral.SetToolTip(ClearArgumentCtrlButton, "Clear value");

                        AArgumentPanel.Controls.Add(ClearArgumentCtrlButton);
                    }
                }
                else
                {
                    // we still need to restrict the width, even when there is no clear button
                    ProbeControl = ArgumentPanelCtrl as Label;
                    ProbeControl2 = ArgumentPanelCtrl as Button;

                    // Check if we found a Control that isn't a Label and also not a Button
                    if ((ProbeControl == null)
                        && (ProbeControl2 == null))
                    {
                        int maxWidth = AContainerPanelWidth - ArgumentPanelCtrl.Left - 18;

                        if (ArgumentPanelCtrl.Width > maxWidth)
                        {
                            ArgumentPanelCtrl.Width = maxWidth;
                        }
                    }
                }
            }
        }

        private void SetApplyFilterButtonState(Control AButton, string AState)
        {
            Button ApplyFilterButton = AButton as Button;

            if (ApplyFilterButton == null)
            {
                throw new ArgumentNullException("Argument 'AButton' must not be null and must be a Button");
            }

            if (AState == "APPLYING")
            {
                ApplyFilterButton.Text = StrApplyingFilter;
                ApplyFilterButton.Enabled = false;
                ApplyFilterButton.Font = new Font(ApplyFilterButton.Font, FontStyle.Italic);
            }
            else
            {
                ApplyFilterButton.Text = StrApplyFilter;
                ApplyFilterButton.Enabled = true;
                ApplyFilterButton.Font = new Font(ApplyFilterButton.Font, FontStyle.Regular);
            }
        }

        #endregion

        #region Event Handlers

        private void BtnCloseFilterClick(object sender, System.EventArgs e)
        {
            Collapse();
        }

        private void FTabFilterAndFind_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ((FShowFindPanel)
                && ((FTabFilterAndFind.SelectedIndex == 1)
                    && (FTabFilterAndFind.Tag == null)))
            {
                AutoSizePanelHeight(FPnlFindControls);

                FTabFilterAndFind.Tag = "AutoSized";
            }

            FocusFirstArgumentControl();

            OnTabSwitched(sender, e);
        }

        /// <summary>
        /// Occurs when any Argument Clear Button is clicked. The Argument Control is determined and 'cleared',
        /// which works differently on different kind of Controls (TextBoxes, ComboBoxes, CheckBoxes).
        /// </summary>
        /// <param name="sender">The Argument Clear Button that got clicked.</param>
        /// <param name="e">Ignored.</param>
        private void ClearArgumentCtrlButtonClick(object sender, System.EventArgs e)
        {
            Button ClearArgumentButton = (Button)sender;
            string TagAsString = ClearArgumentButton.Tag != null ? ClearArgumentButton.Tag.ToString() : String.Empty;
            string ControlToClearName = String.Empty;
            string ControlToClearTagParameter;
            Control ControlToClearInstance = null;
            int SeparatorIndex = TagAsString.IndexOf(';');
            int ControlToClearTagParameterAsInt;
            TextBox ControlToClearAsTextBox;
            TtxtPetraDate ControlToClearAsPetraDateBox;
            CheckBox ControlToClearAsCheckBox;
            ComboBox ControlToClearAsCombo;
            TCmbAutoComplete ControlToClearAsAutoComplete;

            // Determine the Argument Control whose value should be cleared.
            // This information is held in the Tag of the Button that sends this Event;
            // the Tag also contains the string "SuppressChangeDetection" before a semicolon comes as a separator.
            if (SeparatorIndex != -1)
            {
                ControlToClearName = TagAsString.Substring(SeparatorIndex + 1);
            }

            if (!String.IsNullOrEmpty(ControlToClearName))
            {
                ControlToClearInstance = ((Panel)ClearArgumentButton.Parent).Controls[ControlToClearName];
            }

            if (ControlToClearInstance != null)
            {
                // Different 'clear' logic for different kinds of Controls (TextBoxes, ComboBoxes, CheckBoxes)

                //
                // Clearing of a TextBox or DateBox
                //
                ControlToClearAsTextBox = ControlToClearInstance as TextBox;
                ControlToClearAsPetraDateBox = ControlToClearInstance as TtxtPetraDate;

                if (ControlToClearAsPetraDateBox != null)
                {
                    ControlToClearAsPetraDateBox.Date = null;

                    OnClearArgumentCtrlButtonClicked(sender, ControlToClearAsPetraDateBox);

                    return;
                }

                if (ControlToClearAsTextBox != null)
                {
                    ControlToClearAsTextBox.Text = String.Empty;

                    OnClearArgumentCtrlButtonClicked(sender, ControlToClearAsTextBox);

                    return;
                }

                //
                // Clearing of a CheckBox
                //
                ControlToClearAsCheckBox = ControlToClearInstance as CheckBox;

                if (ControlToClearAsCheckBox != null)
                {
                    TagAsString = ControlToClearAsCheckBox.Tag != null ? ControlToClearAsCheckBox.Tag.ToString() : String.Empty;

                    if (!String.IsNullOrEmpty(TagAsString))
                    {
                        if (TagAsString.Contains(CommonTagString.ARGUMENTCONTROLTAG_CLEARVALUE))
                        {
                            ControlToClearAsCheckBox.Checked = (TagAsString.Contains(CommonTagString.ARGUMENTCONTROLTAG_CLEARVALUE + "=true"));
                        }
                        else
                        {
                            ControlToClearAsCheckBox.CheckState = CheckState.Indeterminate;
                        }
                    }
                    else
                    {
                        ControlToClearAsCheckBox.CheckState = CheckState.Indeterminate;
                    }

                    OnClearArgumentCtrlButtonClicked(sender, ControlToClearAsCheckBox);

                    return;
                }

                //
                // Clearing a TCmbAutoComplete (with IgnoreNewValues set)
                //
                ControlToClearAsAutoComplete = ControlToClearInstance as TCmbAutoComplete;

                if ((ControlToClearAsAutoComplete != null) && (ControlToClearAsAutoComplete.IgnoreNewValues == true))
                {
                    // Here is a quirk of the ComboBox.SelectedIndex method
                    // If you want to set it to -1 you have to make the call twice - first it goes to 0 and second call goes to -1
                    // See the comment in TCmbAutoComplete SetSelectedString() method
                    // In our case here we do not want the Filter panel to get two events - one when the selected index is 0 - because this can upset
                    // the selected row if the first index has, say no matching rows, then the highlight will move to the top - and then
                    // an index of -1 will match all rows, but by then the highlight will have changed.
                    // DO NOT REMOVE ONE OF THE SELECTEDINDEX = -1 ROWS FOR THIS REASON
                    FIgnoreValueChangedEvent = (ControlToClearAsAutoComplete.SelectedIndex > 0);
                    ControlToClearAsAutoComplete.SelectedIndex = -1;
                    FIgnoreValueChangedEvent = false;
                    ControlToClearAsAutoComplete.SelectedIndex = -1;
                    ControlToClearAsAutoComplete.Text = String.Empty;

                    OnClearArgumentCtrlButtonClicked(sender, ControlToClearAsAutoComplete);

                    return;
                }

                //
                // Clearing of a 'standard' ComboBox
                //
                ControlToClearAsCombo = ControlToClearInstance as ComboBox;

                if (ControlToClearAsCombo != null)
                {
                    TagAsString = ControlToClearAsCombo.Tag != null ? ControlToClearAsCombo.Tag.ToString() : String.Empty;

                    if (!String.IsNullOrEmpty(TagAsString))
                    {
                        if ((!TagAsString.Contains(CommonTagString.ARGUMENTCONTROLTAG_CLEARVALUE))
                            || (TagAsString.Contains(CommonTagString.ARGUMENTCONTROLTAG_CLEARVALUE + "=0")))
                        {
                            // Index 0 is the 'nothing selected' (=clear) value
                            ControlToClearAsCombo.SelectedIndex = 0;
                        }
                        else
                        {
                            SeparatorIndex = TagAsString.IndexOf('=');

                            if (SeparatorIndex != -1)
                            {
                                ControlToClearTagParameter = TagAsString.Substring(SeparatorIndex + 1);

                                if (!String.IsNullOrEmpty(ControlToClearTagParameter))
                                {
                                    if (System.Int32.TryParse(ControlToClearTagParameter, out ControlToClearTagParameterAsInt))
                                    {
                                        // The Index specified through ControlToClearTagParameterAsInt is the 'nothing selected' (=clear) value
                                        ControlToClearAsCombo.SelectedIndex = ControlToClearTagParameterAsInt;
                                    }
                                    else
                                    {
                                        // Index 0 is the 'nothing selected' (=clear) value
                                        ControlToClearAsCombo.SelectedIndex = 0;
                                    }
                                }
                                else
                                {
                                    // Index 0 is the 'nothing selected' (=clear) value
                                    ControlToClearAsCombo.SelectedIndex = 0;
                                }
                            }
                            else
                            {
                                // Index 0 is the 'nothing selected' (=clear) value
                                ControlToClearAsCombo.SelectedIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        // Index 0 is assumed to be the 'nothing selected' (=clear) value
                        ControlToClearAsCombo.SelectedIndex = 0;
                    }

                    OnClearArgumentCtrlButtonClicked(sender, ControlToClearAsCombo);

                    return;
                }

                // optional implementation for further kinds of Controls can be done here....
            }
        }

        /// <summary>
        /// Gets the complete list of individual panels that make up the standard filter
        /// </summary>
        public List <Panel>FilterPanelControls
        {
            get
            {
                return FFilterControls;
            }
        }

        /// <summary>
        /// Gets the complete list of individual panels that make up the extra filter
        /// </summary>
        public List <Panel>ExtraFilterPanelControls
        {
            get
            {
                return FExtraFilterControls;
            }
        }

        /// <summary>
        /// Gets the complete list of individual panels that make up the find
        /// </summary>
        public List <Panel>FindPanelControls
        {
            get
            {
                return FFindControls;
            }
        }

        #region Custom Events

        /// <summary>
        /// Raises the 'Expanded' Event.
        /// </summary>
        private void OnExpanded()
        {
            if (Expanded != null)
            {
                Expanded(this, null);
            }
        }

        /// <summary>
        /// Raises the 'Collapsed' Event.
        /// </summary>
        private void OnCollapsed()
        {
            if (Collapsed != null)
            {
                Collapsed(this, null);
            }
        }

        /// <summary>
        /// Raises the 'FindTabDisplayed' Event.
        /// </summary>
        private void OnFindTabDisplayed()
        {
            if (FindTabDisplayed != null)
            {
                FindTabDisplayed(this, null);
            }
        }

        /// <summary>
        /// Raises the 'ApplyFilterClicked' Event.
        /// </summary>
        private void OnApplyFilterClicked(object sender, Action <Control>AAction, Action <Control>AResetAction)
        {
            Button ClickedButton = sender as Button;
            Panel ContainingPanel;
            EventContext Context;

            if (ClickedButton == null)
            {
                throw new ArgumentNullException("Argument 'sender' must not be null and must be a Button");
            }

            if (ApplyFilterClicked != null)
            {
                ContainingPanel = ClickedButton.Parent as Panel;

                if (ContainingPanel != null)
                {
                    if (ContainingPanel == pnlFilterControls)
                    {
                        Context = EventContext.ecStandardFilterPanel;
                    }
                    else
                    {
                        Context = EventContext.ecExtraFilterPanel;
                    }

                    ApplyFilterClicked(this, new TContextEventExtControlArgs(Context, ClickedButton, AAction, AResetAction));
                }
            }
        }

        /// <summary>
        /// Raises the 'KeepFilterTurnedOnClicked' Event.
        /// </summary>
        private void OnKeepFilterTurnedOnClicked(object sender, EventArgs e)
        {
            CheckBox ClickedButton = sender as CheckBox;
            Panel ContainingPanel;
            EventContext EContext;

            if (ClickedButton == null)
            {
                throw new ArgumentNullException(
                    "Argument 'sender' must not be null and must be a CheckBox in Button form (Appearance Property: Appearance.Button)");
            }

            ContainingPanel = ClickedButton.Parent as Panel;

            if (ContainingPanel != null)
            {
                if (ContainingPanel.Name == pnlFilterControls.Name)
                {
                    EContext = EventContext.ecStandardFilterPanel;
                }
                else
                {
                    EContext = EventContext.ecExtraFilterPanel;
                }

                UpdateKeepFilterTurnedOnButtonDepressedState();

                if (KeepFilterTurnedOnClicked != null)
                {
                    KeepFilterTurnedOnClicked(this, new TContextEventExtButtonDepressedArgs(EContext, ClickedButton.Checked));
                }
            }
        }

        private void UpdateKeepFilterTurnedOnButtonDepressedState()
        {
            Control[] ControlsArray = pnlFilterControls.Controls.Find(BTN_KEEP_FILTER_TURNED_ON_NAME, false);

            FKeepFilterTurnedOnButtonDepressed = FilterContext.None;

            // If a 'Keep Filter Turned On' Button is shown: check whether it is checked
            if (ControlsArray.Length != 0)
            {
                if (((CheckBox)ControlsArray[0]).Checked)
                {
                    ControlsArray = pnlExtraFilterControls.Controls.Find(BTN_KEEP_FILTER_TURNED_ON_NAME, false);

                    if (ControlsArray.Length != 0)
                    {
                        if (((CheckBox)ControlsArray[0]).Checked)
                        {
                            FKeepFilterTurnedOnButtonDepressed = FilterContext.StandardAndExtraFilter;

                            return;
                        }
                        else
                        {
                            FKeepFilterTurnedOnButtonDepressed = FilterContext.StandardFilterOnly;

                            return;
                        }
                    }
                    else
                    {
                        FKeepFilterTurnedOnButtonDepressed = FilterContext.StandardFilterOnly;

                        return;
                    }
                }
            }

            // Now Check the Extra Filter Panel
            ControlsArray = pnlExtraFilterControls.Controls.Find(BTN_KEEP_FILTER_TURNED_ON_NAME, false);

            // If a 'Keep Filter Turned On' Button is shown: check whether it is checked
            if (ControlsArray.Length != 0)
            {
                if (((CheckBox)ControlsArray[0]).Checked)
                {
                    FKeepFilterTurnedOnButtonDepressed = FilterContext.ExtraFilterOnly;

                    return;
                }
                else
                {
                    FKeepFilterTurnedOnButtonDepressed = FilterContext.None;
                }
            }
        }

        /// <summary>
        /// Raises the 'FindNextClicked' Event.
        /// </summary>
        private void OnFindNextClicked(object sender, EventArgs e)
        {
            Button ClickedButton = sender as Button;
            Panel ContainingPanel;

            if (ClickedButton == null)
            {
                throw new ArgumentNullException("Argument 'sender' must not be null and must be a Button");
            }

            if (FindNextClicked != null)
            {
                ContainingPanel = ClickedButton.Parent as Panel;

                if (ContainingPanel != null)
                {
                    FindNextClicked(ClickedButton, new TContextEventExtSearchDirectionArgs(EventContext.ecFindPanel,
                            ((RadioButton)ContainingPanel.Controls["grpFindDirection"].Controls["rbtFindDirUp"]).Checked));
                }
            }
        }

        /// <summary>
        /// Raises the 'ClearArgumentCtrlButtonClicked' Event.
        /// </summary>
        private void OnClearArgumentCtrlButtonClicked(object sender, Control AAssociatedArgumentCtrl)
        {
            Button ClickedButton = sender as Button;
            Panel ContainingPanel;
            EventContext Context;

            if (ClickedButton == null)
            {
                throw new ArgumentNullException("Argument 'sender' must not be null and must be a Button");
            }

            if (AAssociatedArgumentCtrl == null)
            {
                throw new ArgumentNullException("Argument 'AAssociatedArgumentCtrl' must not be null");
            }

            if (ClearArgumentCtrlButtonClicked != null)
            {
                ContainingPanel = (ClickedButton.Parent.Parent) as Panel;

                if (ContainingPanel != null)
                {
                    if (ContainingPanel.Name == pnlFilterControls.Name)
                    {
                        Context = EventContext.ecStandardFilterPanel;
                    }
                    else if (ContainingPanel.Name == pnlExtraFilterControls.Name)
                    {
                        Context = EventContext.ecExtraFilterPanel;
                    }
                    else if (ContainingPanel.Name == FPnlFindControls.Name)
                    {
                        Context = EventContext.ecFindPanel;
                    }
                    else
                    {
                        Context = EventContext.ecUnknownControl;
                    }

                    ClearArgumentCtrlButtonClicked(sender, new TContextEventExtControlArgs(Context,
                            AAssociatedArgumentCtrl));
                }
            }
        }

        /// <summary>
        /// Raises the 'TabSwitched' Event.
        /// </summary>
        private void OnTabSwitched(object sender, EventArgs e)
        {
            if (TabSwitched != null)
            {
                TabSwitched(sender,
                    new TContextEventArgs(FTabFilterAndFind.SelectedIndex == 0 ? EventContext.ecFilterTab : EventContext.ecFindTab));
            }
        }

        /// <summary>
        /// Raises the 'ArgumentCtrlValueChanged' Event.
        /// </summary>
        private void OnArgumentCtrlValueChanged(object sender, EventArgs e)
        {
            Control ArgumentControl = sender as Control;
            Panel ContainingPanel;
            EventContext Context;
            TextBox SenderAsTextBox;
            CheckBox SenderAsCheckBox;
            ComboBox SenderAsComboBox;
            RadioButton SenderAsRadioButton;
            object Value = null;

            System.Type ValueType = null;

            if (ArgumentControl == null)
            {
                throw new ArgumentNullException("Argument 'sender' must not be null and must be a Control");
            }

            if (ArgumentCtrlValueChanged != null)
            {
                if (ArgumentControl is RadioButton)
                {
                    ContainingPanel = (ArgumentControl.Parent.Parent.Parent) as Panel;
                }
                else
                {
                    ContainingPanel = (ArgumentControl.Parent.Parent) as Panel;
                }

                if (ContainingPanel != null)
                {
                    if (ContainingPanel.Name == pnlFilterControls.Name)
                    {
                        Context = EventContext.ecStandardFilterPanel;
                    }
                    else if (ContainingPanel.Name == pnlExtraFilterControls.Name)
                    {
                        Context = EventContext.ecExtraFilterPanel;
                    }
                    else if (ContainingPanel.Name == FPnlFindControls.Name)
                    {
                        Context = EventContext.ecFindPanel;
                    }
                    else
                    {
                        Context = EventContext.ecUnknownControl;
                    }

                    SenderAsTextBox = sender as TextBox;

                    if (SenderAsTextBox != null)
                    {
                        Value = SenderAsTextBox.Text;
                        ValueType = typeof(System.String);
                    }

                    SenderAsCheckBox = sender as CheckBox;

                    if (SenderAsCheckBox != null)
                    {
                        Value = SenderAsCheckBox.CheckState;
                        ValueType = typeof(CheckState);
                    }

                    SenderAsComboBox = sender as ComboBox;

                    if (SenderAsComboBox != null)
                    {
                        if (SenderAsComboBox is TCmbAutoComplete)
                        {
                            Value = SenderAsComboBox.Text;
                            ValueType = typeof(System.String);
                        }
                        else
                        {
                            Value = SenderAsComboBox.SelectedIndex;
                            ValueType = typeof(System.Int32);
                        }
                    }

                    SenderAsRadioButton = sender as RadioButton;

                    if (SenderAsRadioButton != null)
                    {
                        Value = SenderAsRadioButton.Checked;
                        ValueType = typeof(Boolean);
                    }

                    ArgumentCtrlValueChanged(sender, new TContextEventExtControlValueArgs(Context,
                            ArgumentControl, Value, ValueType));
                }
            }
        }

        #endregion

        #endregion

        #region Overrides

        /// <summary>
        /// This overrides the behaviour of TAB keypresses. We skip all the controls that make up the body of the filter/find panel
        /// The two end points are the Filter/Find tabs at the bottom of the panel and the close button at the top right
        /// This code takes us straight between these two points.
        /// If you want to enter data in the other controls you need to use the mouse, but then you can continue tabbing through those controls.
        ///
        /// I had wanted to add the functionality that if you pressed CTRL+TAB you stuck with the original tab order (no skipping).
        /// However I was thwarted by the fact that CTL+TAB switches tabs so we always switch between Filter and Find with CTRL+TAB.
        /// </summary>
        /// <param name="forward">True when SHIFT not pressed, false when SHIFT is pressed</param>
        /// <returns>Handled</returns>
        protected override bool ProcessTabKey(bool forward)
        {
            if (forward)
            {
                if (FTabFilterAndFind.Focused)
                {
                    btnCloseFilter.Focus();
                    return true;
                }
            }
            else
            {
                if (btnCloseFilter.Focused)
                {
                    FTabFilterAndFind.Focus();
                    return true;
                }
            }

            return base.ProcessTabKey(forward);
        }

        #endregion

        /// <summary>
        /// Helper Class for 'Argument Panels' for the TUcoFilterAndFind UserControl.
        /// </summary>
        public static class ArgumentPanelHelper
        {
            /// <summary>
            /// Helper Method to create an Argument Panel out of a Label and a Control. Such an 'Argument Panel'
            /// can be passed to the TUcoFilterAndFind UserControl.
            /// </summary>
            /// <param name="AControlLabel">Label of the Argument Control.
            /// If this is null, no Label will be shown (useful e.g. for CheckBox Controls who have their own
            /// 'attached' Label).</param>
            /// <param name="AControl">Argument Control.</param>
            /// <param name="AAutomaticClearButton">Whether to automatically create an 'Clear Value' Button (default=true).</param>
            /// <returns>An Argument Panel that can be passed to the TUcoFilterAndFind UserControl.</returns>
            public static Panel CreateArgumentPanel(Label AControlLabel, Control AControl, bool AAutomaticClearButton = true)
            {
                const int ARGUMENT_PANEL_BOTTOM_BORDER = 6; // in Pixels;
                int NextControlVPos = 0;
                Panel ArgumentPanel = new Panel();

                if (AControl == null)
                {
                    throw new ArgumentNullException("Argument AControl must not be null");
                }

                if (AControlLabel != null)
                {
                    AControlLabel.Location = new System.Drawing.Point(3, 0);
                    AControlLabel.AutoSize = true;
                    AControlLabel.Font = new System.Drawing.Font("Verdana", 7F);
                    AControlLabel.TabIndex = 0;

                    NextControlVPos = 17;
                }

                AControl.Location = new System.Drawing.Point(3, NextControlVPos);
                AControl.TabIndex = 1;

                if (!AAutomaticClearButton)
                {
                    ArgumentPanel.Tag = CommonTagString.ARGUMENTPANELTAG_NO_AUTOM_ARGUMENTCLEARBUTTON;
                }

                if (!(AControl is CheckBox))
                {
                    ArgumentPanel.Height = AControl.Bottom + ARGUMENT_PANEL_BOTTOM_BORDER;
                }
                else
                {
                    ArgumentPanel.Height = AControl.Bottom + 3;
                    AControl.Font = new System.Drawing.Font("Verdana", 7F);  // for the Label that is part of the CheckBox Control!
                }

                if (AControlLabel != null)
                {
                    ArgumentPanel.Controls.Add(AControlLabel);
                }

                ArgumentPanel.Controls.Add(AControl);

                ArgumentPanel.BackColor = System.Drawing.Color.Teal;
                ArgumentPanel.SetDoubleBuffered(true);


                return ArgumentPanel;
            }

            /// <summary>
            /// Helper Method to create an Argument Panel out of a Label and a Control. Such an 'Argument Panel'
            /// can be passed to the TUcoFilterAndFind UserControl.
            /// </summary>
            /// <param name="AControlLabelText">Text of the Label of the Argument Control.
            /// If this is null or an empty string, no Label will get created (useful e.g. for CheckBox Controls who
            /// have their own 'attached' Label).</param>
            /// <param name="AControl">Argument Control.</param>
            /// <param name="AAutomaticClearButton">Whether to automatically create an 'Clear Value' Button (default=true).</param>
            /// <returns>An Argument Panel that can be passed to the TUcoFilterAndFind UserControl.</returns>
            public static Panel CreateArgumentPanel(string AControlLabelText, Control AControl, bool AAutomaticClearButton = true)
            {
                Label ControlLabel = null;

                if (AControl == null)
                {
                    throw new ArgumentNullException("Argument AControl must not be null");
                }

                if (!String.IsNullOrEmpty(AControlLabelText))
                {
                    ControlLabel = new Label();

                    ControlLabel.Name = "lbl" + AControl.Name.Substring(4);
                    ControlLabel.Text = AControlLabelText;
                    ControlLabel.TabIndex = 0;
                }

                return CreateArgumentPanel(ControlLabel, AControl, AAutomaticClearButton);
            }
        }
    }
}