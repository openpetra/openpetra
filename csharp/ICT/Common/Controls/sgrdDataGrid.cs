//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2016 by OM International
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
using System.Drawing.Drawing2D;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using DevAge.ComponentModel;
using Ict.Common.Exceptions;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.DataGrid;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Models;
using SourceGrid.Cells.Views;
using SourceGrid.Cells.Virtual;
using SourceGrid.Selection;
using DevAge.ComponentModel.Converter;
using DevAge.ComponentModel.Validator;
using DevAge.Windows.Forms;
using DevAge.Drawing;
using System.Globalization;

namespace Ict.Common.Controls
{
    #region TSgrdDataGrid

    /// <summary>
    /// TSgrdDataGrid is an extension of SourceGrid.DataGrid that contains several
    /// customisations and helper functions, especially for viewing DataTable data in
    /// a List-like manner.
    ///
    /// </summary>
    public class TSgrdDataGrid : SourceGrid.DataGrid
    {
        /// <summary>
        /// Used to specify colours for several aspects of a TSgrdDataGrid.
        /// </summary>
        public struct ColourInformation
        {
            private Color FBackColour;
            private Color FCellBackgroundColour;
            private Color FAlternatingBackgroundColour;
            private Color FSelectionColour;
            private Color FGridLinesColour;

            /// <summary>
            /// Colour of the background of the Grid.
            /// </summary>
            public Color BackColour
            {
                get
                {
                    return FBackColour;
                }

                set
                {
                    FBackColour = value;
                }
            }

            /// <summary>
            /// Colour of the background of every cell and row.
            /// </summary>
            public Color CellBackgroundColour
            {
                get
                {
                    return FCellBackgroundColour;
                }

                set
                {
                    FCellBackgroundColour = value;
                }
            }

            /// <summary>
            /// Used to colour the background of every odd numbered row differently to
            /// generate a 'banding' effect (works only with columns defined in
            /// sgrdDataGrid.Columns!).
            /// </summary>
            public Color AlternatingBackgroundColour
            {
                get
                {
                    return FAlternatingBackgroundColour;
                }

                set
                {
                    FAlternatingBackgroundColour = value;
                }
            }

            /// <summary>
            /// Colour of the Selection.
            /// </summary>
            public Color SelectionColour
            {
                get
                {
                    return FSelectionColour;
                }

                set
                {
                    FSelectionColour = value;
                }
            }

            /// <summary>
            /// Colour of the Grid Lines (works only with columns defined in
            /// sgrdDataGrid.Columns!).
            /// </summary>
            public Color GridLinesColour
            {
                get
                {
                    return FGridLinesColour;
                }

                set
                {
                    FGridLinesColour = value;
                }
            }
        }

        /// <summary>
        /// Used for passing Colour information to the Grid. Re-used by all instances of the Grid!
        /// </summary>
        public static Func <ColourInformation>SetColourInformation;

        private const Int32 WM_KEYDOWN = 0x100;

        private static ColourInformation FColourInfo;
        private static bool FColourInfoSetup = false;

        /// <summary>
        /// Used to refresh grid colours after they have been changed in user preferences.
        /// </summary>
        public static bool ColourInfoSetup
        {
            set
            {
                FColourInfoSetup = value;
            }
        }

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// View for the ColumnHeaders of this Grid
        ///
        /// </summary>
        private SourceGrid.Cells.Views.ColumnHeader FColumnHeaderView;

        /// <summary>
        /// Determines whether the column headers should appear 'greyed out' if the Grid
        /// is disabled.
        ///
        /// </summary>
        private Boolean FShowColumnHeadersDisabled;

        /// <summary>
        /// Colour of the background of the Grid. This is only seen in a space inside the Grid that is not covered by Rows!
        /// </summary>
        private Color FBackColour;

        /// <summary>
        /// Colour of the background of every cell and row.
        /// </summary>
        private Color FCellBackgroundColour;

        /// <summary>
        /// Used to colour the background of every odd numbered row differently to
        /// generate a 'banding' effect (works only with columns defined in
        /// sgrdDataGrid.Columns!).
        /// </summary>
        private Color FAlternateBackColour;

        /// <summary>
        /// Colour of the Selection.
        /// </summary>
        private Color FSelectionColour;

        /// <summary>
        /// Colour of the Grid Lines (works only with columns defined in
        /// sgrdDataGrid.Columns!).
        /// </summary>
        private Color FGridLinesColour;

        /// <summary>
        /// If set to an appropriate delegate function, this provides ToolTips on each
        /// Cell of the Grid (works only with columns defined in sgrdDataGrid.Columns!).
        ///
        /// </summary>
        private TDelegateGetToolTipText FToolTipTextDelegate;

        /// <summary>
        /// Determines whether the column headers should support sorting by clicking on
        /// them (works only with columns defined in sgrdDataGrid.Columns!).
        ///
        /// </summary>
        private Boolean FSortableHeaders;

        /// <summary>
        /// Keeps track of the the selected row before sorting in order to be able
        /// to select it again after sorting the Grid.
        ///
        /// </summary>
        private DataRowView FRowSelectedBeforeSort;

        /// <summary>
        /// Maintains the state of whether the currently selected row should stay
        /// selected after sorting the Grid.
        ///
        /// </summary>
        private Boolean FKeepRowSelectedAfterSort;

        /// <summary>
        /// Used by the PerformAutoFindFirstCharacter procedure.
        ///
        /// </summary>
        private Keys FLastKeyCode;

        /// <summary>
        /// Used by the PerformAutoFindFirstCharacter procedure.
        ///
        /// </summary>
        protected TAutoFindModeEnum FAutoFindMode;

        /// <summary>
        /// Used by the PerformAutoFindFirstCharacter procedure.
        ///
        /// </summary>
        protected Int16 FAutoFindColumn = -1;

        /// <summary>
        /// Used by the PerformAutoFindFirstCharacter procedure.
        ///
        /// </summary>
        private DataView FAutoFindMatchingDataView;

        /// <summary>
        /// Maintains a state for the PerformAutoFindFirstCharacter procedure.
        ///
        /// </summary>
        private Boolean FAutoFindListRebuildNeeded;

        /// <summary>
        /// A flag that is true while the mouse is down.
        /// </summary>
        private Boolean FMouseIsDown;

        /// <summary>
        /// A flag that is true during the 'Sorted' event.
        /// </summary>
        private Boolean FSorting;

        /// <summary>
        /// The maximum number of rows to check when auto-sizing columns.  If the value is -1 we calculate the answer based on the number of columns.
        /// </summary>
        private int FMaxAutoSizeRows = -1;

        /// <summary>
        /// Flag indicating whether the Fixed Rows should be included in the column auto-size calculation or not
        /// </summary>
        private Boolean FIncludeFixedRowsInAutoSizeColumns = true;

        private ToolTipText FGridTooltipController;

        /// <summary>
        /// Gets/sets the flag indicating whether the Fixed Rows should be included in the column auto-size calculation or not
        /// </summary>
        public Boolean IncludeFixedRowsInAutoSizeColumns
        {
            get
            {
                return FIncludeFixedRowsInAutoSizeColumns;
            }
            set
            {
                FIncludeFixedRowsInAutoSizeColumns = value;
            }
        }

        /// <summary>
        /// Gets/sets the maximum number of rows to check when auto-sizing columns
        /// </summary>
        public int MaxAutoSizeRows
        {
            get
            {
                if (FMaxAutoSizeRows == -1)
                {
                    // automatic calculation
                    if ((this.Rows.Count < 10) || (this.Columns.Count == 0))
                    {
                        return 10;
                    }
                    else
                    {
                        return Math.Max(100, 1000 / this.Columns.Count);
                    }
                }

                return FMaxAutoSizeRows;
            }
            set
            {
                FMaxAutoSizeRows = value;
            }
        }

        /// <summary>
        /// Returns true when the mouse is down inside the grid.
        /// </summary>
        public Boolean IsMouseDown
        {
            get
            {
                return FMouseIsDown;
            }
        }

        /// <summary>
        /// Returns true when the grid is re-ordering rows after a sort operation.  Can be used to ignore updates from a panel to the grid
        /// because the sort operation never changes the selected row.
        /// </summary>
        public Boolean Sorting
        {
            get
            {
                return FSorting;
            }
        }

        /// <summary>
        /// Read access to the View for the ColumnHeaders of this Grid (used by
        /// sgrdDataGrid.Columns).
        ///
        /// </summary>
        public SourceGrid.Cells.Views.ColumnHeader ColumnHeaderView
        {
            get
            {
                return FColumnHeaderView;
            }
        }

        /// <summary>
        /// Set this to an appropriate delegate function to provide ToolTips for each
        /// Cell of the Grid.
        ///
        /// </summary>
        public TDelegateGetToolTipText ToolTipTextDelegate
        {
            get
            {
                return FToolTipTextDelegate;
            }

            set
            {
                FToolTipTextDelegate = value;
            }
        }

        /**
         * This property determines whether AutoStretchColumnsToFitWidth should be used.
         *
         */
        [Category("Misc"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(true),
         Browsable(true)]
        public new Boolean AutoStretchColumnsToFitWidth
        {
            get
            {
                return base.AutoStretchColumnsToFitWidth;
            }

            set
            {
                base.AutoStretchColumnsToFitWidth = value;
            }
        }

        /**
         * This property determines which MinimumHeight should be used.
         *
         */
        [Category("Misc"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(19),
         Browsable(true)]
        public new Int32 MinimumHeight
        {
            get
            {
                return base.MinimumHeight;
            }

            set
            {
                base.MinimumHeight = value;
            }
        }

        /// <summary>
        /// This property determines which SpecialKeys should be used.
        /// </summary>
        public new GridSpecialKeys SpecialKeys
        {
            get
            {
                return base.SpecialKeys;
            }

            set
            {
                base.SpecialKeys = value;
            }
        }

        /// <summary>
        /// Helper function for SourceGrid 4 that enables us to hook up our ListChangedEventHandler
        /// </summary>
        public new DevAge.ComponentModel.IBoundList DataSource
        {
            get
            {
                return base.DataSource;
            }

            set
            {
                bool HookupListChangedEvent = false;

                //              if (((DevAge.ComponentModel.BoundDataView) base.DataSource).mDataView.ListChanged != null)  // the compiler doesn't get over this code, so I used the next line to get approximately what I want...
                if (base.DataSource == null)
                {
                    HookupListChangedEvent = true;
                }

                base.DataSource = value;

                if (value != null)
                {
                    if (HookupListChangedEvent)
                    {
                        ((DevAge.ComponentModel.BoundDataView) base.DataSource).ListChanged += new ListChangedEventHandler(this.OnDataViewChanged);
                    }
                }
            }
        }

        /// <summary>
        /// Helper function for SourceGrid 4 that returns the SelectedDataRows as a DataRowView
        /// (as SourceGrid 3 did).
        /// </summary>
        public DataRowView[] SelectedDataRowsAsDataRowView
        {
            get
            {
                if (this.SelectedDataRows != null)
                {
                    DataRowView[] ReturnValue = new DataRowView[this.SelectedDataRows.Length];

                    for (int Counter = 0; Counter < this.SelectedDataRows.Length; Counter++)
                    {
                        ReturnValue[Counter] = (DataRowView) this.SelectedDataRows[Counter];
                    }

                    return ReturnValue;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Adds an image annotation to the cells in the specified column of the grid.
        /// </summary>
        /// <param name="ACallerForm">The form or user control that is hosting the grid</param>
        /// <param name="AImageColumnIndex">The column index that will display the optional annotation</param>
        /// <param name="AColumnContext">An arbitrary context string that identifies the column when more than one column is annotated</param>
        /// <param name="AImageEnum">The image selected from one of the available enumerated items</param>
        public void AddAnnotationImage(IBoundImageEvaluator ACallerForm,
            int AImageColumnIndex,
            BoundGridImage.AnnotationContextEnum AColumnContext,
            BoundGridImage.DisplayImageEnum AImageEnum)
        {
            // Add a model for the image
            this.Columns[AImageColumnIndex].DataCell.Model.AddModel(new BoundGridImage(ACallerForm, AColumnContext, AImageEnum));

            // Add a standard SourceGrid tool tip for the bound image
            this.Columns[AImageColumnIndex].DataCell.AddController(SourceGrid.Cells.Controllers.ToolTipText.Default);
            this.Columns[AImageColumnIndex].DataCell.Model.AddModel(new BoundImageToolTipModel(ACallerForm, AColumnContext, AImageEnum));
        }

        /*
         * Custom properties follow
         */

        /// <summary>
        /// Colour of the background of the Grid. This is only seen in a space inside the Grid that is not covered by Rows!
        /// </summary>
        [Category("Appearance"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Browsable(true),
         Description("The colour of the of background of the Grid. This is only seen in a space inside the Grid that is not covered by Rows!")]
        public Color BackColour
        {
            get
            {
                return FBackColour;
            }

            set
            {
                FBackColour = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Colour of the background of every cell and row.
        /// </summary>
        [Category("Appearance"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Browsable(true),
         Description("The colour of the background of every cell and row.")]
        public Color CellBackgroundColour
        {
            get
            {
                return FCellBackgroundColour;
            }

            set
            {
                FCellBackgroundColour = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Alternating background colour of the Grid. This is the colour that is used to set
        /// the background colour of every second line.
        /// </summary>
        [Category("Appearance"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Browsable(true),
         Description("The colour that is used to set the background colour of every second line.")]
        public Color AlternatingBackgroundColour
        {
            get
            {
                return FAlternateBackColour;
            }

            set
            {
                FAlternateBackColour = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Colour of the Selection.
        /// </summary>
        [Category("Appearance"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Browsable(true),
         Description("The colour of the Selection.")]
        public Color SelectionColour
        {
            get
            {
                return FSelectionColour;
            }

            set
            {
                FSelectionColour = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Colour of the Grid Lines.
        /// </summary>
        [Category("Appearance"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Browsable(true),
         Description("The colour of the Grid Lines.")]
        public Color GridLinesColour
        {
            get
            {
                return FGridLinesColour;
            }

            set
            {
                FGridLinesColour = value;
                this.Refresh();
            }
        }

        /**
         * This property determines whether the column headers should support sorting
         * by clicking on them.
         *
         */
        [Category("Sorting"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(true),
         Browsable(true),
         Description("Determines whether the column headers should support sorting by clicking on them.")]
        public Boolean SortableHeaders
        {
            get
            {
                return FSortableHeaders;
            }

            set
            {
                FSortableHeaders = value;
            }
        }

        /**
         * This property determines whether the currently selected row should stay
         * selected after sorting the Grid.
         *
         */
        [Category("Sorting"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(true),
         Browsable(true),
         Description("Determines whether the currently selected row should stay selected after sorting the Grid.")]
        public Boolean KeepRowSelectedAfterSort
        {
            get
            {
                return FKeepRowSelectedAfterSort;
            }

            set
            {
                FKeepRowSelectedAfterSort = value;
            }
        }

        /**
         * This property determines which AutoFindMode should be used.
         * If the auto-find column has not already been set, it is set to the first column.
         */
        [Category("AutoFind"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(TAutoFindModeEnum.NoAutoFind),
         Browsable(true),
         Description("Determines which AutoFindMode should be used.")]
        public TAutoFindModeEnum AutoFindMode
        {
            get
            {
                return FAutoFindMode;
            }

            set
            {
                if (value == TAutoFindModeEnum.FullString)
                {
                    if (DesignMode)
                    {
                        throw new EDataGridAutoFindModeNotImplementedYetException(
                            "Sorry, AutoFindMode 'FullString' is not implemented yet! You could implement it, though, if you really need it!");
                    }
                }

                FAutoFindMode = value;

                // If the auto-find column has not been set then we set that as well
                if (FAutoFindColumn == -1)
                {
                    FAutoFindColumn = 0;
                }
            }
        }

        /**
         * This property determines which Column of the DataGrid should be
         * enabled for AutoFind (Note: This is not the DataColumn of the DataView!).
         *
         */
        [Category("AutoFind"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         DefaultValue(-1),
         Browsable(true),
         Description("Determines which Column of the DataGrid should be enabled for AutoFind (Note: This is not the DataColumn of the DataView!).")]
        public Int16 AutoFindColumn
        {
            get
            {
                return FAutoFindColumn;
            }

            set
            {
                FAutoFindColumn = value;
                FAutoFindListRebuildNeeded = true;
            }
        }

        // Custom Events follow
        //      /**
        //      This Event is thrown when a Cell of the Grid is DoubleClicked with the mouse.
        //      */
        //      [Category("Action"),
        //       Browsable(true),
        //       RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
        //       Description ("Occurs when when a Cell of the Grid is Clicked with the mouse.")]
        //       property ClickCell: TClickCellEventHandler add FClickCell remove FClickCell;
//
        //       [Category("Action"),
        //       Browsable(true),
        //       RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
        //       Description ("Occurs when when a HeaderCell of the Grid is Clicked with the mouse.")]
        //       property ClickHeaderCell: TClickHeaderCellEventHandler add FClickHeaderCell remove FClickHeaderCell;

        /**
         * This Event is thrown when a Cell of the Grid is DoubleClicked with the mouse.
         *
         */
        [Category("Action"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when when a Cell of the Grid is DoubleClicked with the mouse.")]
        public event TDoubleClickCellEventHandler DoubleClickCell;

        /// <summary>
        /// Occurs when when a HeaderCell of the Grid is DoubleClicked with the mouse.
        /// </summary>
        [Category("Action"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when when a HeaderCell of the Grid is DoubleClicked with the mouse.")]
        public event TDoubleClickHeaderCellEventHandler DoubleClickHeaderCell;

        /**
         * This Event is thrown when the Insert key is pressed on the Grid.
         *
         */
        [Category("Custom Key Handling"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when the Insert key is pressed on the Grid.")]
        public event TKeyPressedEventHandler InsertKeyPressed;

        /**
         * This Event is thrown when the Delete key is pressed on the Grid.
         *
         */
        [Category("Custom Key Handling"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when the Delete key is pressed on the Grid.")]
        public event TKeyPressedEventHandler DeleteKeyPressed;

        /**
         * This Event is thrown when the Enter key is pressed on the Grid.
         *
         */
        [Category("Custom Key Handling"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when the Enter key is pressed on the Grid.")]
        public event TKeyPressedEventHandler EnterKeyPressed;

        /**
         * This Event is thrown when the Space key is pressed on the Grid.
         *
         */
        [Category("Custom Key Handling"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when the Space key is pressed on the Grid.")]
        public event TKeyPressedEventHandler SpaceKeyPressed;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            //
            // TSgrdDataGrid
            //
            this.Name = "TSgrdDataGrid";
        }

        #endregion

        /// <summary>
        /// TSgrdDataGrid is an extension of SourceGrid.DataGrid that contains several
        /// customisations and helper functions, especially for viewing DataTable data in
        /// a List-like manner.
        /// </summary>
        public TSgrdDataGrid() : base()
        {
            this.Set_DefaultProperties();
            InitializeComponent();

            FColumnHeaderView = new SourceGrid.Cells.Views.ColumnHeader();

            FGridTooltipController = new SourceGrid.Cells.Controllers.ToolTipText();
            FGridTooltipController.IsBalloon = false;
            FGridTooltipController.ToolTipTitle = Catalog.GetString("Column Header:");

            // Hook up our custom DoubleClick Handler
            this.Controller.AddController(new DoubleClickController());
            SpecialKeys = SourceGrid.GridSpecialKeys.Default ^ SourceGrid.GridSpecialKeys.Tab;

            this.MouseDown += new MouseEventHandler(TSgrdDataGrid_MouseDown);
            this.MouseUp += new MouseEventHandler(TSgrdDataGrid_MouseUp);
            this.SizeChanged += TSgrdDataGrid_SizeChanged;

            TToolTipModel.InitializeUnit();
        }

        private FormWindowState FFormWindowState = FormWindowState.Normal;
        private bool FDoneFirstResize = false;
        private void TSgrdDataGrid_SizeChanged(object sender, EventArgs e)
        {
            Form form = FindForm();

            if (form == null)
            {
                return;
            }

            FormWindowState curWindowState = form.WindowState;

            switch (curWindowState)
            {
                case FormWindowState.Maximized:
                    AutoResizeGrid();
                    break;

                case FormWindowState.Normal:

                    if ((FFormWindowState == FormWindowState.Maximized) || !FDoneFirstResize)
                    {
                        AutoResizeGrid();
                    }

                    break;
            }

            FFormWindowState = curWindowState;
            FDoneFirstResize = true;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        /// <param name="Disposing"></param>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        /// <summary>
        /// This procedure sets the default properties.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void Set_DefaultProperties()
        {
            // Default size
            this.Height = 100;
            this.Width = 400;

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
                FColourInfo.BackColour = System.Drawing.Color.White;
                FColourInfo.CellBackgroundColour = System.Drawing.Color.White;
                FColourInfo.AlternatingBackgroundColour = System.Drawing.Color.FromArgb(230, 230, 230);
                FColourInfo.SelectionColour = Color.FromArgb(150, Color.FromKnownColor(KnownColor.Highlight));
                FColourInfo.GridLinesColour = System.Drawing.SystemColors.ControlDark;
            }

            // Default look
            this.BackColor = FColourInfo.BackColour;
            this.CellBackgroundColour = FColourInfo.CellBackgroundColour;
            this.AlternatingBackgroundColour = FColourInfo.AlternatingBackgroundColour;
            this.GridLinesColour = FColourInfo.GridLinesColour;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.AutoStretchColumnsToFitWidth = true;
            this.MinimumHeight = 19;
            ((SelectionBase) this.Selection).Border = new DevAge.Drawing.RectangleBorder();
            ((SelectionBase) this.Selection).BackColor = FColourInfo.SelectionColour;
            ((SelectionBase) this.Selection).FocusBackColor = ((SelectionBase) this.Selection).BackColor;

            // Default behaviour
            this.TabStop = true;
            this.SpecialKeys = SourceGrid.GridSpecialKeys.Default ^ SourceGrid.GridSpecialKeys.Tab;
            this.DeleteQuestionMessage = "You have chosen to delete this record." + Environment.NewLine + Environment.NewLine +
                                         "Do you really want to delete it?";
            this.FixedRows = 1;
            this.SortableHeaders = true;
            this.KeepRowSelectedAfterSort = true;
            this.AutoFindColumn = -1;
            this.Selection.FocusStyle = FocusStyle.None;        // We handle all focus issues ourselves.  In a multi-selection world this is important
            this.Invalidate();
        }

        #region Methods for adding typed columns

        /// <summary>
        /// Overload for adding a text column with an editor
        ///
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        /// <param name="AEditor">An instance of an Editor (based on ICellVirtual.Editor)</param>
        /// <returns>void</returns>
        public void AddTextColumn(String AColumnTitle,
            DataColumn ADataColumn,
            Int16 AColumnWidth,
            EditorBase AEditor)
        {
            AddTextColumn(AColumnTitle, ADataColumn, AColumnWidth, null, AEditor);
        }

        /// <summary>
        /// Easy method to add a new Text column.
        ///
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        /// <param name="AController"></param>
        /// <param name="AEditor">An instance of an Editor (based on ICellVirtual.Editor)</param>
        /// <param name="AModel"></param>
        /// <param name="AView"></param>
        /// <param name="AConditionView"></param>
        ///
        /// <returns>void</returns>
        public void AddTextColumn(String AColumnTitle,
            DataColumn ADataColumn,
            Int16 AColumnWidth = -1,
            ControllerBase AController = null,
            EditorBase AEditor = null,
            ModelContainer AModel = null,
            IView AView = null,
            SourceGrid.Conditions.ConditionView AConditionView = null)
        {
            SourceGrid.Cells.ICellVirtual ADataCell;
            SourceGrid.DataGridColumn AGridColumn;

            if (ADataColumn == null)
            {
                throw new ArgumentNullException("ADataColumn", "ADataColumn must not be nil!");
            }

            ADataCell = new SourceGrid.Cells.DataGrid.Cell();

            if (AController != null)
            {
                MessageBox.Show("AController <> nil!");
                try
                {
                    ADataCell.AddController(AController);
                }
                catch (Exception Exp)
                {
                    MessageBox.Show("TSgrdDataGrid.AddTextColumn: Exeption: " + Exp.ToString());
                }
            }

            if (AEditor != null)
            {
                ADataCell.Editor = AEditor;
            }

            if (AModel != null)
            {
                ADataCell.Model = AModel;
            }

            if (AView != null)
            {
                ADataCell.View = AView;
            }

            AGridColumn = new TSgrdTextColumn(this, ADataColumn, AColumnTitle, ADataCell, AColumnWidth, FSortableHeaders);

            if (AConditionView != null)
            {
                AGridColumn.Conditions.Add(AConditionView);
            }

            this.Columns.Insert(this.Columns.Count, AGridColumn);
        }

        /// <summary>
        /// Overload for adding a CheckBox column
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AReadOnly">Set to true if the column should be read-only</param>
        /// <returns>void</returns>
        public void AddCheckBoxColumn(String AColumnTitle, DataColumn ADataColumn, bool AReadOnly)
        {
            AddCheckBoxColumn(AColumnTitle, ADataColumn, -1, AReadOnly, null);
        }

        /// <summary>
        /// Easy method to add a new CheckBox column.
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        /// <param name="AReadOnly">Set to true if the column should be read-only</param>
        /// <param name="AEditor"></param>
        /// <returns>void</returns>
        public void AddCheckBoxColumn(String AColumnTitle,
            DataColumn ADataColumn,
            Int16 AColumnWidth = -1,
            bool AReadOnly = true,
            EditorBase AEditor = null)
        {
            if (ADataColumn == null)
            {
                throw new ArgumentNullException("ADataColumn", "ADataColumn must not be nil!");
            }

            SourceGrid.Cells.ICellVirtual ADataCell = new SourceGrid.Cells.DataGrid.CheckBox();

            if (AEditor != null)
            {
                ADataCell.Editor = AEditor;
            }

            SourceGrid.DataGridColumn AGridColumn = new TSgrdTextColumn(this, ADataColumn, AColumnTitle, ADataCell, AColumnWidth, FSortableHeaders);

            if (AReadOnly)
            {
                ADataCell.Editor.EnableEdit = false;
            }

            this.Columns.Insert(this.Columns.Count, AGridColumn);
        }

        /// <summary>
        /// Easy method to add a new Image column without a header text.
        ///
        /// </summary>
        /// <param name="AGetImageDelegate">Delegate method that will be called to retrieve
        /// the Image which should be displayed in the cell.
        /// </param>
        /// <returns>void</returns>
        public void AddImageColumn(DelegateGetImageForRow AGetImageDelegate)
        {
            AddImageColumn("", AGetImageDelegate);
        }

        /// <summary>
        /// Easy method to add a new Image column with header text.
        ///
        /// </summary>
        /// <param name="AGetImageDelegate">Delegate method that will be called to retrieve
        /// the Image which should be displayed in the cell.</param>
        /// <param name="AColumnTitle">Title of the HeaderColumn
        /// </param>
        /// <returns>void</returns>
        public void AddImageColumn(String AColumnTitle, DelegateGetImageForRow AGetImageDelegate)
        {
            SourceGrid.DataGridColumn AGridColumn;

            if (!(AGetImageDelegate != null))
            {
                throw new ArgumentNullException("AGetImageDelegate", "AGetImageDelegate must contain an assigned Delegate!");
            }

            AGridColumn = new TSgrdImageColumn(this, AColumnTitle, AGetImageDelegate);
            this.Columns.Insert(this.Columns.Count, AGridColumn);
        }

        /// <summary>
        /// Add a date column that is read-only. The date is displayed in an a common international data format, independent of a computer's date formatting settings.
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        public void AddDateColumn(String AColumnTitle, DataColumn ADataColumn, Int16 AColumnWidth = -1)
        {
            SourceGrid.Cells.Editors.TextBoxUITypeEditor DateEditor = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(DateTime));
            Ict.Common.TypeConverter.TDateConverter DateTypeConverter = new Ict.Common.TypeConverter.TDateConverter();

            DateEditor.EditableMode = EditableMode.None;
            DateEditor.TypeConverter = DateTypeConverter;

            AddTextColumn(AColumnTitle, ADataColumn, AColumnWidth, null, DateEditor, null, null);
        }

        /// <summary>
        /// add a column that shows a decimal value.
        /// aligns the value to the right.
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="ADecimalDigits">Number of digits after the numeric decimal point</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        public void AddDecimalColumn(String AColumnTitle, DataColumn ADataColumn, int ADecimalDigits = 2, Int16 AColumnWidth = -1)
        {
            SourceGrid.Cells.Editors.TextBox DecimalEditor = new SourceGrid.Cells.Editors.TextBox(typeof(decimal));
            DecimalEditor.TypeConverter = new Ict.Common.TypeConverter.TDecimalConverter(
                ADataColumn.ColumnName, Thread.CurrentThread.CurrentCulture.NumberFormat, ADecimalDigits);
            DecimalEditor.EditableMode = EditableMode.None;

            SourceGrid.Cells.Views.Cell view = new SourceGrid.Cells.Views.Cell();
            view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            AddTextColumn(AColumnTitle, ADataColumn, AColumnWidth, null, DecimalEditor, null, view, null);
        }

        /// <summary>
        /// add a column that shows a currency value.
        /// aligns the value to the right.
        /// prints number in red if it is negative
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="ADecimalDigits">Number of digits after the currency decimal point</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        public void AddCurrencyColumn(String AColumnTitle, DataColumn ADataColumn, int ADecimalDigits = 2, Int16 AColumnWidth = -1)
        {
            SourceGrid.Cells.Editors.TextBox CurrencyEditor = new SourceGrid.Cells.Editors.TextBox(typeof(decimal));
            CurrencyEditor.TypeConverter = new Ict.Common.TypeConverter.TCurrencyConverter(
                ADataColumn, Thread.CurrentThread.CurrentCulture.NumberFormat, ADecimalDigits);

            CurrencyEditor.EditableMode = EditableMode.None;


            // Non-negative value View
            SourceGrid.Cells.Views.Cell view = new SourceGrid.Cells.Views.Cell();
            view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            // Negative value View
            SourceGrid.Cells.Views.Cell NegativeNumberView = new SourceGrid.Cells.Views.Cell();
            NegativeNumberView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            NegativeNumberView.ForeColor = Color.Red;

            // Condition for negative value View
            SourceGrid.Conditions.ConditionView selectedConditionNegative =
                new SourceGrid.Conditions.ConditionView(NegativeNumberView);
            selectedConditionNegative.EvaluateFunction = (delegate(SourceGrid.DataGridColumn column,
                                                                   int gridRow, object itemRow)
                                                          {
                                                              DataRowView row = (DataRowView)itemRow;
                                                              return row[ADataColumn.ColumnName] is decimal
                                                              && (decimal)row[ADataColumn.ColumnName] < 0;
                                                          });

            AddTextColumn(AColumnTitle, ADataColumn, AColumnWidth, null, CurrencyEditor, null, view, selectedConditionNegative);
        }

        class BooleanConverter : System.ComponentModel.TypeConverter
        {
            /// <summary>
            /// text that should be displayed if the value is true
            /// </summary>
            protected string FTextTrue;

            /// <summary>
            /// text that should be displayed if the value is false
            /// </summary>
            protected string FTextFalse;

            /// <summary>
            /// constructor
            /// </summary>
            public BooleanConverter(string ATextTrue, string ATextFalse)
            {
                FTextTrue = ATextTrue;
                FTextFalse = ATextFalse;
            }

            /// <summary>
            /// we don't need conversions to boolean, but somehow we need to return true so that the conversion works the other way
            /// </summary>
            public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context,
                Type sourceType)
            {
                return true;
            }

            /// <summary>
            /// allow all conversions from boolean
            /// </summary>
            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context,
                Type destinationType)
            {
                return true;
            }

            /// <summary>
            /// convert boolean to string, the destinationType is ignored
            /// </summary>
            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context,
                System.Globalization.CultureInfo culture,
                object value,
                Type destinationType)
            {
                if ((bool)value == true)
                {
                    return FTextTrue;
                }

                return FTextFalse;
            }
        }

        /// <summary>
        /// add a column that shows a boolean value.
        /// this allows to show a specific text for true and another text for false.
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="ATextTrue">text to be displayed in the column if the value is true</param>
        /// <param name="ATextFalse">text to be displayed in the column if the value is false</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        public void AddBooleanColumn(String AColumnTitle, DataColumn ADataColumn, string ATextTrue, string ATextFalse, Int16 AColumnWidth = -1)
        {
            SourceGrid.Cells.Editors.TextBox BooleanEditor = new SourceGrid.Cells.Editors.TextBox(typeof(bool));
            BooleanEditor.TypeConverter = new BooleanConverter(ATextTrue, ATextFalse);

            BooleanEditor.EditableMode = EditableMode.None;

            AddTextColumn(AColumnTitle, ADataColumn, AColumnWidth, null, BooleanEditor, null, null, null);
        }

        /// <summary>
        /// A converter for displaying partner keys in 10 digit format
        /// </summary>
        public class PartnerKeyConverter : System.ComponentModel.TypeConverter
        {
            /// <summary>
            /// constructor
            /// </summary>
            public PartnerKeyConverter()
            {
            }

            /// <summary>
            /// we don't need conversions to PartnerKey, but somehow we need to return true so that the conversion works the other way
            /// </summary>
            public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context,
                Type sourceType)
            {
                return true;
            }

            /// <summary>
            /// allow all conversions from PartnerKey
            /// </summary>
            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context,
                Type destinationType)
            {
                return true;
            }

            /// <summary>
            /// convert PartnerKey to string, the destinationType is ignored
            /// </summary>
            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context,
                System.Globalization.CultureInfo culture,
                object value,
                Type destinationType)
            {
                if (value != null)
                {
                    return String.Format("{0:0000000000}", (Int64)value);
                }
                else
                {
                    return String.Format("{0:0000000000}", 0);
                }
            }
        }

        /// <summary>
        /// add a column that shows a PartnerKey value (include leading zeros to display a 10 digit number)
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        public void AddPartnerKeyColumn(String AColumnTitle, DataColumn ADataColumn, Int16 AColumnWidth = -1)
        {
            SourceGrid.Cells.Editors.TextBox PartnerKeyEditor = new SourceGrid.Cells.Editors.TextBox(typeof(Int64));
            PartnerKeyEditor.TypeConverter = new PartnerKeyConverter();

            PartnerKeyEditor.EditableMode = EditableMode.None;

            AddTextColumn(AColumnTitle, ADataColumn, AColumnWidth, null, PartnerKeyEditor, null, null, null);
        }

        /// <summary>
        /// Add a column that shows a time value in localised short string format.  Data should be in the form of a numeric seconds, or a parsable HH:MM:SS string.
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        public void AddShortTimeColumn(String AColumnTitle, DataColumn ADataColumn, Int16 AColumnWidth = -1)
        {
            SourceGrid.Cells.Editors.TextBoxUITypeEditor TimeEditor = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(DateTime));
            TimeEditor.EditableMode = EditableMode.None;
            TimeEditor.TypeConverter = new Ict.Common.TypeConverter.TShortTimeConverter();

            AddTextColumn(AColumnTitle, ADataColumn, AColumnWidth, null, TimeEditor);
        }

        /// <summary>
        /// Add a column that shows a time value in localised long string format.  Data should be in the form of a numeric seconds, or a parsable HH:MM string.
        /// </summary>
        /// <param name="AColumnTitle">Title of the HeaderColumn</param>
        /// <param name="ADataColumn">DataColumn to which this column should be DataBound</param>
        /// <param name="AColumnWidth">Column width in pixels (-1 for automatic width)</param>
        public void AddLongTimeColumn(String AColumnTitle, DataColumn ADataColumn, Int16 AColumnWidth = -1)
        {
            SourceGrid.Cells.Editors.TextBoxUITypeEditor TimeEditor = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(DateTime));
            TimeEditor.EditableMode = EditableMode.None;
            TimeEditor.TypeConverter = new Ict.Common.TypeConverter.TLongTimeConverter();

            AddTextColumn(AColumnTitle, ADataColumn, AColumnWidth, null, TimeEditor);
        }

        /// <summary>
        /// Set a tooltip that is displayed when the user mouses over the specified column header row
        /// </summary>
        /// <param name="AColumnNumber">0-based column index</param>
        /// <param name="ATipText">Text to show in the tooltip</param>
        public void SetHeaderTooltip(int AColumnNumber, string ATipText)
        {
            if (AColumnNumber < this.Columns.Count)
            {
                SourceGrid.Cells.Cell cell = (SourceGrid.Cells.Cell) this.GetCell(0, AColumnNumber);

                // If the cell is null we cannot set the text
                // The cell will be null if the DataSource has not yet been bound to the grid.
                if (cell != null)
                {
                    cell.ToolTipText = ATipText;
                    cell.AddController(FGridTooltipController);
                }
            }
        }

        #endregion

        #region Overridden Events

        /// <summary>
        /// when the grid is enabled or disabled
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(System.EventArgs e)
        {
            // MessageBox.Show('TSgrdDataGrid.OnEnabledChanged');
            if (!FShowColumnHeadersDisabled)
            {
                FShowColumnHeadersDisabled = true;
                FColumnHeaderView.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            }
            else
            {
                FShowColumnHeadersDisabled = false;
                FColumnHeaderView.ForeColor = System.Drawing.SystemColors.ControlText;
            }

            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// after sorting
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSortedRangeRows(SourceGrid.SortRangeRowsEventArgs e)
        {
            base.OnSortedRangeRows(e);

            if ((FRowSelectedBeforeSort != null) && FKeepRowSelectedAfterSort)
            {
                int nNewRowIndex = this.Rows.DataSourceRowToIndex(FRowSelectedBeforeSort) + 1;
                SelectRowAfterSort(nNewRowIndex);
            }
        }

        /// <summary>
        /// before sorting
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSortingRangeRows(SourceGrid.SortRangeRowsEventArgs e)
        {
            FRowSelectedBeforeSort = (DataRowView) this.Rows.IndexToDataSourceRow(this.Selection.ActivePosition.Row);
            base.OnSortingRangeRows(e);
        }

        /// <summary>
        /// Selects a grid row after a row has moved due to sorting without putting the focus on the grid.
        /// Note that no FocusRowLeaving event is called so this call will by-pass any validation (so it can be called from within a validation routine)
        /// Note also that this will give rise to a RowChanged event, but the grid's 'Sorting' property will be true.
        /// </summary>
        public void SelectRowAfterSort(Int32 ARowNumberInGrid)
        {
            FSorting = true;
            SelectRowWithoutFocus(ARowNumberInGrid);
            FSorting = false;
        }

        /// <summary>
        /// Used as a way of selecting a grid row without putting the focus on the grid.
        /// Note that no FocusRowLeaving event is called so this call will by-pass any validation (so it can be called from within a validation routine)
        /// However a SelectionChanged event will be fired.
        /// </summary>
        public void SelectRowWithoutFocus(Int32 ARowNumberInGrid)
        {
            if (this.Rows.Count > this.FixedRows)
            {
                ARowNumberInGrid = Math.Max(Math.Min(ARowNumberInGrid, this.Rows.Count - 1), this.FixedRows);
            }
            else
            {
                ARowNumberInGrid = -1;
            }

            this.Selection.ResetSelection(false, true);
            this.Selection.SelectRow(ARowNumberInGrid, true);
            this.ShowCell(ARowNumberInGrid);
        }

        #endregion

        #region Custom Events

        /// <summary>
        /// something changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataViewChanged(Object sender, ListChangedEventArgs e)
        {
            FAutoFindListRebuildNeeded = true;
        }

        private void TSgrdDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            FMouseIsDown = false;
        }

        private void TSgrdDataGrid_MouseDown(object sender, MouseEventArgs e)
        {
            FMouseIsDown = true;
        }

        /// <summary>
        /// double click on cell
        /// </summary>
        /// <param name="e"></param>
        private void OnDoubleClickCell(CellContextEventArgs e)
        {
            // MessageBox.Show('OnCellDoubleClick.  Column: ' + e.CellContext.Position.Column.ToString  + '; Row: ' + e.CellContext.Position.Row.ToString);
            if (DoubleClickCell != null)
            {
                DoubleClickCell(e.CellContext.Grid, e);
            }
        }

        /// <summary>
        /// double click on the header cell
        /// </summary>
        /// <param name="e"></param>
        private void OnDoubleClickHeaderCell(ColumnEventArgs e)
        {
            // MessageBox.Show('OnDoubleClickHeaderCell.  Column: ' + e.Column.ToString);
            if (DoubleClickHeaderCell != null)
            {
                DoubleClickHeaderCell(this, e);
            }
        }

        /// <summary>
        /// key has been pressed
        /// </summary>
        /// <param name="e"></param>
        private void OnInsertKeyPressed(RowEventArgs e)
        {
            // MessageBox.Show('OnInsertKeyPressed.  Row: ' + e.Row.ToString);
            if (InsertKeyPressed != null)
            {
                InsertKeyPressed(this, e);
            }
        }

        /// <summary>
        /// delete key has been pressed
        /// </summary>
        /// <param name="e"></param>
        private void OnDeleteKeyPressed(RowEventArgs e)
        {
            // MessageBox.Show('OnDeleteKeyPressed.  Row: ' + e.Row.ToString);
            if (DeleteKeyPressed != null)
            {
                // prevent the Grid from deleting the row as well!
                this.DeleteRowsWithDeleteKey = false;
                DeleteKeyPressed(this, e);
            }
        }

        /// <summary>
        /// enter key has been pressed
        /// </summary>
        /// <param name="e"></param>
        private void OnEnterKeyPressed(RowEventArgs e)
        {
            // MessageBox.Show('OnEnterKeyPressed.  Row: ' + e.Row.ToString);
            if (EnterKeyPressed != null)
            {
                EnterKeyPressed(this, e);
            }
        }

        /// <summary>
        /// space key has been pressed
        /// </summary>
        /// <param name="e"></param>
        private void OnSpaceKeyPressed(RowEventArgs e)
        {
            // MessageBox.Show('OnEnterKeyPressed.  Row: ' + e.Row.ToString);
            if (SpaceKeyPressed != null)
            {
                SpaceKeyPressed(this, e);
            }
        }

        #endregion

        #region Functionality customisation

        /**
         * This custom SourceGrid Controller handles the DoubleClick event of the Grid and
         * fires either the OnDoubleClickCell or OnDoubleClickHeaderCell Event.
         */
        class DoubleClickController : SourceGrid.Cells.Controllers.ControllerBase
        {
            public override void OnDoubleClick(SourceGrid.CellContext sender, EventArgs e)
            {
                base.OnDoubleClick(sender, e);

                SourceGrid.Position ClickPosition;
                SourceGrid.CellContextEventArgs CellArgs;
                SourceGrid.ColumnEventArgs HeaderCellArgs;

                // MessageBox.Show('TSgrdDataGrid.OnDoubleClick');

                ClickPosition = sender.Grid.PositionAtPoint(sender.Grid.PointToClient(MousePosition));

                if (ClickPosition != SourceGrid.Position.Empty)
                {
                    if ((sender.Grid.FixedRows == 1) && (ClickPosition.Row == 0))
                    {
                        // DoubleClick occured in a HeaderCell > fire OnDoubleClickHeaderCell Event
                        HeaderCellArgs = new SourceGrid.ColumnEventArgs(ClickPosition.Column);
                        ((TSgrdDataGrid)(sender.Grid)).OnDoubleClickHeaderCell(HeaderCellArgs);
                    }
                    else
                    {
                        Position ClickPosWithoutHeader = new Position(ClickPosition.Row, ClickPosition.Column);

                        if (sender.Grid.FixedRows == 1)
                        {
                            ClickPosWithoutHeader = new Position(ClickPosition.Row - 1, ClickPosition.Column);
                        }

                        // DoubleClick occured in a Cell > fire OnDoubleClickCell Event
                        CellArgs =
                            new SourceGrid.CellContextEventArgs(new CellContext(sender.Grid, ClickPosWithoutHeader));
                        ((TSgrdDataGrid)(sender.Grid)).OnDoubleClickCell(CellArgs);
                    }
                }
            }
        }

        /// <summary>
        /// Used only internally to process the Enter and Space keys. Must be public to
        /// work.
        ///
        /// </summary>
        /// <param name="AKeyData">Passed by the Operating System.
        /// @result See .NET 1.1 API documentation on how this needs to be set.
        /// </param>
        /// <returns>void</returns>
        protected override Boolean ProcessDialogKey(Keys AKeyData)
        {
            Int32 SelectedDataRow;

            // MessageBox.Show('TSgrdDataGrid.ProcessDialogKey.  KeyCode: ' + Enum(AKeyData).ToString("G"));
            if (AKeyData == Keys.Enter)
            {
                // Enter key should raise OnEnterKeyPressed Event (if assigned)
                if (EnterKeyPressed != null)
                {
                    if (this.SelectedDataRows.Length > 0)
                    {
                        SelectedDataRow = this.Rows.DataSourceRowToIndex(this.SelectedDataRows[0]);
                    }
                    else
                    {
                        SelectedDataRow = -1;
                    }

                    this.OnEnterKeyPressed(new RowEventArgs(SelectedDataRow));
                    return true;
                }
            }
            else if (AKeyData == Keys.Space)
            {
                // Space key should raise OnSpaceKeyPressed Event (if assigned)
                if (SpaceKeyPressed != null)
                {
                    if (this.SelectedDataRows.Length > 0)
                    {
                        SelectedDataRow = this.Rows.DataSourceRowToIndex(this.SelectedDataRows[0]);
                    }
                    else
                    {
                        SelectedDataRow = -1;
                    }

                    this.OnSpaceKeyPressed(new RowEventArgs(SelectedDataRow));
                    return true;
                }
            }

            return base.ProcessDialogKey(AKeyData);
        }

        /// <summary>
        /// This is a replacement for the DataGrid.Rows.DataSourceRowToIndex function.
        /// The DataSourceRowToIndex function internally uses IList.IndexOf, which doesn't find DataRowView
        /// objects that are returned from a DataView that is created from the Grid's DataView
        /// [((DevAge.ComponentModel.BoundDataView) base.DataSource).mDataView] when searchin in the Grid's DataView
        /// --- for IList.IndexOf(), the same DataRowView object seem to be two differnt objects...!
        ///
        /// DataSourceRowToIndex2 manually iterates through the Grid's DataView and compares Rows objects. This works!
        /// </summary>
        /// <returns>The 0-based index of the specified DataRowView in the grid's DataView</returns>
        public int DataSourceRowToIndex2(DataRowView ADataRowView, int AHintRowToTryFirst = -1)
        {
            return DataSourceRowToIndex2(ADataRowView.Row, AHintRowToTryFirst);
        }

        /// <summary>
        /// This overload takes a DataRow as the parameter in place of a DataRowView.  See also the comment for the DataRowView overload.
        /// </summary>
        /// <param name="ADataRow">The Row object whose rowindex is required</param>
        /// <param name="AHintRowToTryFirst">You can supply a 0-based row number where you expect the row to be.
        /// If you are correct, this saves the code from iterating through all the grid rows!</param>
        /// <returns>The 0-based index of the specified DataRow in the grid's DataView</returns>
        public int DataSourceRowToIndex2(DataRow ADataRow, int AHintRowToTryFirst = -1)
        {
            int RowIndex = -1;

            if (ADataRow != null)
            {
                DataView dv = (this.DataSource as BoundDataView).DataView;

                if ((AHintRowToTryFirst >= 0) && (AHintRowToTryFirst < dv.Count))
                {
                    if (dv[AHintRowToTryFirst].Row == ADataRow)
                    {
                        // Good hint!
                        return AHintRowToTryFirst;
                    }
                }

                for (int Counter2 = 0; Counter2 < dv.Count; Counter2++)
                {
                    if (dv[Counter2].Row == ADataRow)
                    {
                        RowIndex = Counter2;
                        break;
                    }
                }
            }

            return RowIndex;
        }

        /// <summary>
        /// Returns the index of the first (topmost) highlighted row or -1 if no row is highlighted
        /// </summary>
        /// <returns>int</returns>
        public int GetFirstHighlightedRowIndex()
        {
            int rowIndex = -1;

            SourceGrid.RangeRegion selectedRegion = Selection.GetSelectionRegion();

            if ((selectedRegion != null) && (selectedRegion.GetRowsIndex().Length > 0))
            {
                rowIndex = selectedRegion.GetRowsIndex()[0];
            }

            return rowIndex;
        }

        /// select a row in the grid without checking the bounds
        public void SelectRowInGrid(Int32 ARowNumberInGrid)
        {
            SelectRowInGrid(ARowNumberInGrid, false);
        }

        /// select a row in the grid.  By default generate the event(s) for focus changes.
        public void SelectRowInGrid(Int32 ARowNumberInGrid, Boolean ASelectBorderIfOutsideLimit)
        {
            if (ASelectBorderIfOutsideLimit)
            {
                if (ARowNumberInGrid >= Rows.Count)
                {
                    ARowNumberInGrid = Rows.Count - 1;
                }

                if ((ARowNumberInGrid < 1) && (Rows.Count > 1))
                {
                    ARowNumberInGrid = 1;
                }
            }

            Position newPosition;

            if (ARowNumberInGrid >= this.FixedRows)
            {
                int column = this.FixedColumns;

                while ((column < this.Columns.Count - 1) && !this.Columns[column].Visible)
                {
                    column++;
                }

                newPosition = new Position(ARowNumberInGrid, column);
            }
            else
            {
                newPosition = Position.Empty;
            }

            // This call will set the active cell.
            // If the cell row to activate is not the current row a FocusRowLeaving event (which can be cancelled) will be fired.
            // In all cases a SelectionChanged event will be fired after that, even if the row to select is the current row.
            //  (This is a good thing because the data in the row may have changed)
            this.Selection.Focus(newPosition, true);

            if (newPosition != Position.Empty)
            {
                // scroll to the row
                ShowCell(newPosition.Row);
            }
        }

        /// <summary>
        /// This is the OpenPetra override.  It scrolls the window so that the specified row is shown.
        /// The standard grid behaviour would be simply to ensure the selected row is within the grid.
        /// With this method, where possible there is always one unselected row above or one row below.
        /// </summary>
        /// <param name="ARowNumberInGrid">The grid row number that needs to be inside the viewport</param>
        /// <returns>False if the grid scrolls to a new position.  True if the specified row is already in the view port</returns>
        public bool ShowCell(int ARowNumberInGrid)
        {
            // Assume we will show the specified row
            int rowToShow = ARowNumberInGrid;

            // Get the list of displayed rows, not including partial rows
            Rectangle displayRectangle = this.DisplayRectangle;

            List <int>displayedRows = this.Rows.RowsInsideRegion(displayRectangle.Y, displayRectangle.Height, false, false);

            if (displayedRows.Count >= 3)
            {
                // If the row to show is the current top row or above, we ensure that we show the row above the selected one
                // If the row to show is the current bottom row or below, we ensure we show the row below the selected one
                if (rowToShow <= displayedRows[0])
                {
                    rowToShow = Math.Max(rowToShow - 1, 1);
                }
                else if (rowToShow >= displayedRows[displayedRows.Count - 1])
                {
                    rowToShow = Math.Min(rowToShow + 1, Rows.Count - 1);
                }
                else
                {
                    // It is in the view port already
                    return true;
                }
            }

            // So use the standard grid call to show the row we have come up with.
            // Note: this call is misleading!  Intellisense has the boolean as 'ignorePartial', but actually the parameter is passed direct to
            //  the call to list all rows in the viewport and INCLUDE partial!!  Having got this list, the decision is made whether to scroll.
            //  So we pass false so as not to include partial rows in this decision.
            return this.ShowCell(new SourceGrid.Position(rowToShow, 0), false);
        }

        /// <summary>
        /// Performs selection of rows with a matching first character as the user
        /// presses certain keys that produce characters that this procedure can search
        /// for.
        ///
        /// </summary>
        /// <param name="AKey">Keyboard code, passed in from ProcessSpecialGridKey
        /// </param>
        /// <returns>void</returns>
        private void PerformAutoFindFirstCharacter(Keys AKey)
        {
            DataTable GridDataTable;
            Int16 Counter;
            Int32 CurrentlySelectedGridRow;
            Int32 FoundGridRow;
            Int32 NewSelectedItemRow;
            DataRowView TmpDataRowView;
            String DataColumnName;
            String SearchCharacter = "";
            String SelectClause;

            if ((AKey != FLastKeyCode) || (FAutoFindListRebuildNeeded))
            {
                // Build a DataView that start with the typed character
                GridDataTable = ((DevAge.ComponentModel.BoundDataView) base.DataSource).DataView.Table;

                if ((FAutoFindColumn < 0) || (FAutoFindColumn >= this.Columns.Count))
                {
                    // GridDataTable.Columns.Count
                    throw new EDataGridInvalidAutoFindColumnException(
                        "The specified AutoFindColumn is out of the range of DataGridColumns that the Grid has");
                }

                if (this.Columns[FAutoFindColumn].PropertyName == null)
                {
                    throw new EDataGridInvalidAutoFindColumnException(
                        "The specified AutoFindColumn is not a DataBound DataGridColumn! AutoFind can only be used with DataBound DataGridColumns.");
                }
                else
                {
                    DataColumnName = this.Columns[FAutoFindColumn].PropertyName;

                    //                  MessageBox.Show("DataColumnName: " + DataColumnName);
                }

                // Determine SearchCharacter
                if ((AKey >= Keys.A) && (AKey <= Keys.Z))
                {
                    SearchCharacter = AKey.ToString("G");
                }
                else if ((AKey == Keys.Add) || (AKey == Keys.Oemplus))
                {
                    SearchCharacter = "+";
                }
                else if ((AKey == Keys.Subtract) || (AKey == Keys.OemMinus))
                {
                    SearchCharacter = "-";
                }
                else if (AKey == Keys.Space)
                {
                    // Note: Space only gets through to there when SpaceKeyPressed is not assigned!
                    SearchCharacter = " ";
                }
                else if ((AKey >= Keys.D0) && (AKey <= Keys.D9))
                {
                    // Remove leading 'D'
                    SearchCharacter = AKey.ToString("G").Substring(1);
                }
                else if ((AKey >= Keys.NumPad0) && (AKey <= Keys.NumPad9))
                {
                    // Remove leading 'NumPad'
                    SearchCharacter = AKey.ToString("G").Substring(6);
                }

                //              MessageBox.Show("SearchCharacter: " + SearchCharacter);

                SelectClause = DataColumnName + " LIKE '" + SearchCharacter + "%'";

                //              MessageBox.Show("SelectClause: " + SelectClause);
                FAutoFindMatchingDataView = new DataView(GridDataTable,
                    SelectClause,
                    ((DevAge.ComponentModel.BoundDataView) base.DataSource).DataView.Sort,
                    DataViewRowState.CurrentRows);

                //              MessageBox.Show("FAutoFindMatchingDataView.Count: " + FAutoFindMatchingDataView.Count.ToString());
                FAutoFindListRebuildNeeded = false;
            }

            if (FAutoFindMatchingDataView.Count > 0)
            {
                if (this.Selection.IsEmpty())
                {
                    CurrentlySelectedGridRow = 0;
                }
                else
                {
                    CurrentlySelectedGridRow = this.Selection.ActivePosition.Row;
                }

                //              MessageBox.Show("CurrentlySelectedGridRow: " + CurrentlySelectedGridRow.ToString());

                NewSelectedItemRow = -1;

                // Remove focus from previous DataCell (to unhighlight it)
                this.Selection.Focus(Position.Empty, true);

                for (Counter = 0; Counter <= FAutoFindMatchingDataView.Count - 1; Counter += 1)
                {
                    TmpDataRowView = FAutoFindMatchingDataView[Counter];

                    /*
                     * Can't use "FoundGridRow := Self.Rows.DataSourceRowToIndex(TmpDataRowView);" - this internally
                     * uses IList.IndexOf, which doesn't find DataRowView Objects returned from FAutoFindMatchingDataView
                     * in ((DevAge.ComponentModel.BoundDataView) base.DataSource).mDataView - they seem to be two differnt
                     * objects...!
                     * Therefore I need to use our own function, DataSourceRowToIndex2, which manually iterates through the
                     * mDataView and compares Row objects. This works!
                     */
                    FoundGridRow = this.DataSourceRowToIndex2(TmpDataRowView);

                    //                  MessageBox.Show("FoundGridRow: " + FoundGridRow.ToString());
                    if (FoundGridRow >= CurrentlySelectedGridRow)
                    {
                        NewSelectedItemRow = FoundGridRow;

                        //                      MessageBox.Show("Found Row below CurrentGridRow. NewSelectedItemRow: " + NewSelectedItemRow.ToString());
                        break;
                    }
                }

                int colToFocus = 0;

                while ((this.Columns[colToFocus].Visible == false) && (colToFocus < this.Columns.Count - 1))
                {
                    colToFocus++;
                }

                if (NewSelectedItemRow != -1)
                {
                    // A matching Row was found after the currently selected row, so select it
                    // Scroll grid to line where the new record is now displayed and keep the focus on the grid
                    this.ShowCell(NewSelectedItemRow + 1);
                    this.Selection.Focus(new Position(NewSelectedItemRow + 1, colToFocus), true);
                }
                else
                {
                    // No matching Row was found after the currently selected row, so select the first matching Row
                    TmpDataRowView = FAutoFindMatchingDataView[0];

                    /*
                     * Can't use "FoundGridRow := Self.Rows.DataSourceRowToIndex(TmpDataRowView);" - this internally
                     * uses IList.IndexOf, which doesn't find DataRowView Objects returned from FAutoFindMatchingDataView
                     * in ((DevAge.ComponentModel.BoundDataView) base.DataSource).mDataView - they seem to be two differnt
                     * objects...!
                     * Therefore I need to use our own function, DataSourceRowToIndex2, which manually iterates through the
                     * mDataView and compare Rows objects. This works!
                     */
                    NewSelectedItemRow = this.DataSourceRowToIndex2(TmpDataRowView);

                    //                  MessageBox.Show("Only found Row above CurrentGridRow! NewSelectedItemRow: " + NewSelectedItemRow.ToString());

                    // Scroll grid to line where the new record is now displayed and keep the focus on the grid
                    this.ShowCell(NewSelectedItemRow + 1);
                    this.Selection.Focus(new Position(NewSelectedItemRow + 1, colToFocus), true);
                }
            }
        }

        /// <summary>
        /// Used only internally to to be able to implement other custom key handling.
        /// Must be public to work.
        /// </summary>
        /// <param name="AKeyEventArgs">Passed by the Operating System.</param>
        /// <returns>void</returns>
        public override void ProcessSpecialGridKey(KeyEventArgs AKeyEventArgs)
        {
            base.ProcessSpecialGridKey(AKeyEventArgs);
            Int32 SelectedDataRow;

            // MessageBox.Show('TSgrdDataGrid.ProcessSpecialGridKey:  KeyCode: ' + Enum(AKeyEventArgs.KeyCode).ToString("G") +
            // 'Modifiers: ' + Enum(AKeyEventArgs.Modifiers).ToString("G")  );
            if (AKeyEventArgs.KeyCode == Keys.Home)
            {
                // Key for scrolling to and selecting the first row in the Grid
                // MessageBox.Show('Home pressed!');
                SelectRowInGrid(1);
            }
            // Key for scrolling to and selecting the last row in the Grid
            else if (AKeyEventArgs.KeyCode == Keys.End)
            {
                // MessageBox.Show('End pressed!  Rows: ' + this.Rows.Count.ToString);
                SelectRowInGrid(this.Rows.Count - 1);
            }
            // Key for firing OnInsertKeyPressed event
            else if (AKeyEventArgs.KeyCode == Keys.Insert)
            {
                // MessageBox.Show('Insert pressed!');
                // Fire OnInsertKeyPressed event.
                if (this.SelectedDataRows.Length > 0)
                {
                    SelectedDataRow = this.Rows.DataSourceRowToIndex(this.SelectedDataRows[0]) + 1;
                }
                else
                {
                    SelectedDataRow = -1;
                }

                this.OnInsertKeyPressed(new RowEventArgs(SelectedDataRow));

                //If a New button exists call its code.
                Control insertButton = this.FindNearestControl("btnNew");

                if (insertButton != null)
                {
                    ((System.Windows.Forms.Button)insertButton).PerformClick();
                }
            }
            // Key for firing OnDeleteKeyPressed event
            else if (AKeyEventArgs.KeyCode == Keys.Delete)
            {
                // MessageBox.Show('Delete pressed!');
                // Fire OnDeleteKeyPressed event.
                if (this.SelectedDataRows.Length > 0)
                {
                    SelectedDataRow = this.Rows.DataSourceRowToIndex(this.SelectedDataRows[0]) + 1;
                }
                else
                {
                    SelectedDataRow = -1;
                }

                this.OnDeleteKeyPressed(new RowEventArgs(SelectedDataRow));

                //If a Delete button exists call its code.
                Control deleteButton = this.FindNearestControl("btnDelete");

                if (deleteButton != null)
                {
                    ((System.Windows.Forms.Button)deleteButton).PerformClick();
                }
            }
            // Keys that can trigger AutoFind
            else if (((AKeyEventArgs.KeyCode >= Keys.A) && (AKeyEventArgs.KeyCode <= Keys.Z) && (Control.ModifierKeys == Keys.None))
                     || (AKeyEventArgs.KeyCode == Keys.Add) || (AKeyEventArgs.KeyCode == Keys.Subtract)
                     || (AKeyEventArgs.KeyCode == Keys.Oemplus) || (AKeyEventArgs.KeyCode == Keys.OemMinus) || (AKeyEventArgs.KeyCode == Keys.Space)
                     || ((AKeyEventArgs.KeyCode >= Keys.D0) && (AKeyEventArgs.KeyCode <= Keys.D9))
                     || ((AKeyEventArgs.KeyCode >= Keys.NumPad0) && (AKeyEventArgs.KeyCode <= Keys.NumPad9)))
            {
                // Note: Space only gets through to there when SpaceKeyPressed is not assigned!
                if (AutoFindMode == TAutoFindModeEnum.FirstCharacter)
                {
                    PerformAutoFindFirstCharacter(AKeyEventArgs.KeyCode);
                }
            }

            FLastKeyCode = AKeyEventArgs.KeyCode;
        }

        private Control FindNearestControl(String AControlName)
        {
            Control TryParent = this.Parent;

            while (TryParent != null)
            {
                Control[] TryButtons = TryParent.Controls.Find(AControlName, true);

                if (TryButtons.Length > 0)
                {
                    return TryButtons[0];
                }

                TryParent = TryParent.Parent;
            }

            return null;
        }

        /// <summary>
        /// Resizes the grid columns, taking account of the MaxAutoSizeRows property and the IncludeFixedRowsInAutoSizeColumns property.
        /// Then, by default, automatically stretches the columns to fit.
        /// </summary>
        public void AutoResizeGrid()
        {
            this.Columns.AutoSize(false, IncludeFixedRowsInAutoSizeColumns ? 0 : FixedRows, Math.Min(MaxAutoSizeRows, this.Rows.Count - 1));
            base.OnResize(new EventArgs());
        }

        /// <summary>
        /// Gets a value of true if the current cell is being edited
        /// </summary>
        public bool IsEditorEditing
        {
            get
            {
                return (new CellContext(this, Selection.ActivePosition)).IsEditing();
            }
        }

        /// <summary>
        /// Ends an outstanding edit on a grid
        /// </summary>
        /// <param name="ACancel">Set to true if the value is to be restored to its start value and the current value discarded.</param>
        /// <returns>True if the edit was ended successfully, false otherwise (indicating a value that cannot be stored according to the validation rule)</returns>
        public bool EndEdit(bool ACancel)
        {
            // Can we successfully end it?
            return (new CellContext(this, Selection.ActivePosition)).EndEdit(ACancel);
        }

        #region IndexedGridRowsHelper

        /// <summary>
        /// Helper Class for Grids that are sorted based on an 'Index' Column.
        /// </summary>
        /// <remarks>Create an instance of this Class in a Form's 'RunOnceOnActivationManual' Method
        /// (or in a UserControls' 'InitializeManualCode' Method) and then call this Classes' Methods,
        /// as required.</remarks>
        public class IndexedGridRowsHelper
        {
            readonly TSgrdDataGrid FGrid;
            readonly int FIndexColumnNr;
            readonly System.Windows.Forms.Button FBtnDemote;
            readonly System.Windows.Forms.Button FBtnPromote;
            readonly Action FActionAfterSwapping;
            bool FDemoteAndPromoteButtonsDisabledDueToManualSort = false;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="AGrid">Grid instance.</param>
            /// <param name="AIndexColumnNr">Number of the DataColumn in the DataTables' DataColumns Collection that represents the 'index' of the Rows.</param>
            /// <param name="ABtnDemote">Demote Button.</param>
            /// <param name="ABtnPromote">Promote Button.</param>
            /// <param name="AActionAfterSwapping">Delegate that should be executed after the swap of Rows was performed (optional).</param>
            public IndexedGridRowsHelper(TSgrdDataGrid AGrid, int AIndexColumnNr,
                System.Windows.Forms.Button ABtnDemote, System.Windows.Forms.Button ABtnPromote, Action AActionAfterSwapping = null)
            {
                FGrid = AGrid;
                FIndexColumnNr = AIndexColumnNr;
                FBtnDemote = ABtnDemote;
                FBtnPromote = ABtnPromote;
                FActionAfterSwapping = AActionAfterSwapping;

                // When the user sorts the Grid manually: disable promote/demote buttons and show MessageBox
                AGrid.SortedRangeRows += HandleSortedRangeRows;
            }

            /// <summary>
            /// If there are any Rows in the Grid that are 'below' the selected Row: increase/decrease their Indexes accordingly.
            /// </summary>
            /// <param name="ASelectedRowIndex">Grid Row that is the selected Row (Row numbers start with 1!).</param>
            /// <param name="AIncrease">True to increase the indexes, false to decrease the indexes of following rows.</param>
            public void AdjustIndexesOfFollowingRows(int ASelectedRowIndex, bool AIncrease)
            {
                DataRow PresentRow;

                if (AIncrease)
                {
                    for (int Counter = FGrid.Rows.Count - 1; Counter > ASelectedRowIndex; Counter--)
                    {
                        PresentRow = ((DataRowView)FGrid.Rows.IndexToDataSourceRow(Counter)).Row;

                        PresentRow[FIndexColumnNr] = ((int)PresentRow[FIndexColumnNr]) + 1;
                    }
                }
                else
                {
                    for (int Counter = ASelectedRowIndex; Counter < FGrid.Rows.Count; Counter++)
                    {
                        PresentRow = ((DataRowView)FGrid.Rows.IndexToDataSourceRow(Counter)).Row;

                        PresentRow[FIndexColumnNr] = ((int)PresentRow[FIndexColumnNr]) - 1;
                    }
                }
            }

            /// <summary>
            /// Swaps the Indexes of two DataRows.
            /// </summary>
            /// <param name="ARow1">First <see cref="DataRow"/>.</param>
            /// <param name="ARow2">Second <see cref="DataRow"/>.</param>
            public void SwapRowIndexes(DataRow ARow1, DataRow ARow2)
            {
                // Actually do the row updates in the table
                int PresentIndex = (int)ARow1[FIndexColumnNr];

                ARow1.BeginEdit();
                ARow1[FIndexColumnNr] = ARow2[FIndexColumnNr];
                ARow1.EndEdit();
                ARow2.BeginEdit();
                ARow2[FIndexColumnNr] = PresentIndex;
                ARow2.EndEdit();

                if (FActionAfterSwapping != null)
                {
                    FActionAfterSwapping();
                }
            }

            /// <summary>
            /// Determines and sets the 'Index' of a DataRow that is to be added.
            /// </summary>
            /// <param name="ARow">New DataRow that is to be added.</param>
            public void DetermineIndexForNewRow(DataRow ARow)
            {
                DataRow CurrentRow;

                int[] SelectedRegion = FGrid.Selection.GetSelectionRegion().GetRowsIndex();
                int SelectedRowIndex = SelectedRegion.Length > 0 ? SelectedRegion[0] : -1;

                if (SelectedRowIndex == -1)
                {
                    // There is no selected Row as there are no Rows yet: Index is set to 0
                    ARow[FIndexColumnNr] = 0;
                }
                else
                {
                    // Determine the currently selected Row...
                    CurrentRow = ((DataRowView)FGrid.Rows.IndexToDataSourceRow(SelectedRowIndex)).Row;

                    // and base the Index of a new Row on the selected Row's index
                    int NewRowIndex = (int)CurrentRow[FIndexColumnNr] + 1;

                    AdjustIndexesOfFollowingRows(SelectedRowIndex, true);

                    ARow[FIndexColumnNr] = NewRowIndex;
                }
            }

            /// <summary>
            /// Promotes the current Row, i.e. moves the current row further down in the Grid.
            /// </summary>
            public void PromoteRow()
            {
                int SelectedRowIndex = FGrid.Selection.GetSelectionRegion().GetRowsIndex()[0];

                var CurrentRow = ((DataRowView)FGrid.Rows.IndexToDataSourceRow(SelectedRowIndex)).Row;
                var OtherRow = ((DataRowView)FGrid.Rows.IndexToDataSourceRow(SelectedRowIndex + 1)).Row;

                SwapRowIndexes(CurrentRow, OtherRow);

                // Move the selection so it tracks the current row (grid rows start at 1)
                FGrid.SelectRowInGrid(SelectedRowIndex + 1);
            }

            /// <summary>
            /// Demotes the current Row, i.e. moves the current row further up in the Grid.
            /// </summary>
            public void DemoteRow()
            {
                int SelectedRowIndex = FGrid.Selection.GetSelectionRegion().GetRowsIndex()[0];

                var CurrentRow = ((DataRowView)FGrid.Rows.IndexToDataSourceRow(SelectedRowIndex)).Row;
                var OtherRow = ((DataRowView)FGrid.Rows.IndexToDataSourceRow(SelectedRowIndex - 1)).Row;

                SwapRowIndexes(CurrentRow, OtherRow);

                // Move the selection so it tracks the current row (grid rows start at 1)
                FGrid.SelectRowInGrid(SelectedRowIndex - 1);
            }

            /// <summary>
            /// Updates the enabled/disabled state of Demote and Promote Buttons.
            /// </summary>
            /// <param name="ACurrentRow">Current Row number (Row numbers start at 1!).</param>
            /// <param name="AReadOnlyMode">Pass true to prevent the Buttons from becoming enabled (default=false).</param>
            public void UpdateButtons(int ACurrentRow, bool AReadOnlyMode = false)
            {
                if (FDemoteAndPromoteButtonsDisabledDueToManualSort)
                {
                    FBtnDemote.Enabled = false;
                    FBtnPromote.Enabled = false;
                }
                else
                {
                    if (!AReadOnlyMode)
                    {
                        // The grid rows start at 1 due to the one-row header
                        FBtnDemote.Enabled = ACurrentRow > 1;
                        FBtnPromote.Enabled = ((ACurrentRow < FGrid.Rows.Count - 1)
                                               && (ACurrentRow != -1));
                    }
                    else
                    {
                        FBtnDemote.Enabled = false;
                        FBtnPromote.Enabled = false;
                    }
                }
            }

            /// <summary>
            /// When the user sorts the Grid manually: show information and disable promote/demote buttons!
            /// </summary>
            /// <param name="sender">Ignored.</param>
            /// <param name="e">Ignored.</param>
            void HandleSortedRangeRows(object sender, SourceGrid.SortRangeRowsEventArgs e)
            {
                if (!FDemoteAndPromoteButtonsDisabledDueToManualSort)
                {
                    FDemoteAndPromoteButtonsDisabledDueToManualSort = true;

                    UpdateButtons(-1);

                    MessageBox.Show(
                        Catalog.GetString(
                            "You have sorted the list manually. Because of that you will not be able to move individual rows 'up' and 'down' anymore with the respective buttons "
                            +
                            "because the re-ordering of rows only makes sense when the list is sorted according to its internal sort order!\r\n\r\n"
                            +
                            "If you need to re-order the rows: you will need to close this form and re-open it again."),
                        Catalog.GetString("List Manually Sorted: Re-ordering of Rows no Longer Possible"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        #endregion

        #endregion
    }
    #endregion

    /// <summary>
    /// behaviour for typing a value and going to an appropriate row
    /// </summary>
    public enum TAutoFindModeEnum
    {
        /// <summary>
        /// no auto find at all
        /// </summary>
        NoAutoFind,

        /// <summary>
        /// look for lines starting with a given character
        /// </summary>
        FirstCharacter,

        /// <summary>
        /// look for the full string (not implemented yet)
        /// </summary>
        FullString
    };

    /// <summary>
    /// delegate for tooltip
    /// </summary>
    public delegate String TDelegateGetToolTipText(Int16 AColumn, Int16 ARow);

    /// <summary>
    /// what happens when key has been pressed
    /// </summary>
    public delegate void TKeyPressedEventHandler(System.Object Sender, RowEventArgs e);

    /// <summary>
    /// cell has been clicked
    /// </summary>
    public delegate void TClickCellEventHandler(System.Object Sender, CellContextEventArgs e);

    /// <summary>
    /// header row has been clicked
    /// </summary>
    public delegate void TClickHeaderCellEventHandler(System.Object Sender, ColumnEventArgs e);

    /// <summary>
    /// cell has been double clicked
    /// </summary>
    public delegate void TDoubleClickCellEventHandler(System.Object Sender, CellContextEventArgs e);

    /// <summary>
    /// header cell has been double clicked
    /// </summary>
    public delegate void TDoubleClickHeaderCellEventHandler(System.Object Sender, ColumnEventArgs e);


    #region EDataGridInvalidAutoFindColumnException

    /// <summary>
    /// Cannot find.
    /// </summary>
    public class EDataGridInvalidAutoFindColumnException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDataGridInvalidAutoFindColumnException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDataGridInvalidAutoFindColumnException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDataGridInvalidAutoFindColumnException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EDataGridAutoFindModeNotImplementedYetException

    /// <summary>
    /// Auto Find not implemented yet.
    /// </summary>
    public class EDataGridAutoFindModeNotImplementedYetException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDataGridAutoFindModeNotImplementedYetException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDataGridAutoFindModeNotImplementedYetException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDataGridAutoFindModeNotImplementedYetException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion
}