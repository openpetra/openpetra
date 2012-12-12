//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Data;
using SourceGrid;
using System.Globalization;

namespace Ict.Common.Controls
{
    #region TSgrdDataGridPaged

    /// <summary>
    /// Extends TsgrdDataGrid (an extension to SourceGrid3.DataGrid) with the ability
    /// to load only data that is actually viewed in the Grid, therefore reducing
    /// the data transfer from the Server to the Client.
    ///
    /// How it works:
    /// The Grid initially loads only a subset of the resultset that is held by the
    /// Server. The size (=number of rows) of this subset is called 'Page'. The size
    /// of a 'Page' of data is determined by the vertical size of the Grid at the
    /// time where the InitialiseGrid method is called and stays the same, whether
    /// the Grid gets resized or not.
    /// As the user tries to access a row of data that is not yet transferred from
    /// the Server, the Grid calls a Delegate function that returns a 'page' of data
    /// that holds this row. This is also done when the Grid is being resized and as
    /// a result of that would need to display a row that is not yet transferred from
    /// the Server.
    ///
    /// How to use:
    /// Execute method on the Server that executes the query that yields the rows
    /// that you later want to display (all rows are held on the Server, but none
    /// are transferred yet). There are no restrictions on how the DataTable and
    /// its DataColumns are to be made up (eg. there needs to be no PrimaryKey)!
    /// Call the LoadFirstDataPage method, specifying for the
    /// AGetDataPagedResultFunction parameter a delegate function that will get
    /// called when a Page of data needs to be retrieved. This returns a
    /// DataTable containing all data pages (also empty ones that are not
    /// retrieved yet!). (After a call to this method the PageSize property may
    /// be inquired.)
    /// Set the Grid's DataSource to the DefaultView of the returned DataTable.
    /// Do everything else you would do with a TsgrdDataGrid (or
    /// SourceGrid3.DataGrid) control (eg. create Columns, setup visual
    /// appearance...
    /// A call to the LoadAllDataPages method can be made to force the loading
    /// of all data pages (not recommended unless you really need it). After that,
    /// the PagedDataTable holds all records that are held on the Server.
    ///
    /// Note: Due to the fact that the programmer is responsible to call first a
    /// method on the Server that executes the query and then call the
    /// LoadFirstDataPage method to load the data, this Grid can be used even
    /// in multithreaded GUI scenarios (see MPartner.PartnerFind screen for an
    /// example!)
    ///
    /// </summary>
    public class TSgrdDataGridPaged : TSgrdDataGrid
    {
        static Int16 DEFAULT_PAGESIZE_IF_GRID_TOO_SMALL = 5;

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Maintains a state that tells wheter the Grid has been intialised for loading data.</summary>
        private Boolean FGridInitialised;

        /// <summary>Maintains a state that tells wheter the Grid has already fetched data.</summary>
        private Boolean FDataTransferDone;

        /// <summary>Size of the Pages that are to be returned</summary>
        private Int16 FPageSize;

        /// <summary>Number of records resulting from the query</summary>
        private Int32 FTotalRecords;

        /// <summary>Number of Pages resulting from the query</summary>
        private Int16 FTotalPages;

        /// <summary>ArrayList that keeps track of which pages of data have already been retrieved</summary>
        private ArrayList FTransferredDataPages;

        /// <summary>DataTable that holds all Pages of data (also empty ones that are not retrieved yet!)</summary>
        private DataTable FPagedDataTable;

        /// <summary>Delegate function that gets called when a Page of data needs to be retrieved</summary>
        private TDelegateGetDataPagedResult FGetDataPagedResult;

        /// <summary>Maintains a state that tells wheter all Pages of data have already been retrieved once because a sort operation was done on the Grid.</summary>
        private Boolean FPerformFullLoadOnDataGridSort;

        /// <summary>Stores the last height of the Grid control.</summary>
        private Int32 FLastHeight;

        /// <summary>Keeps track of whether the OnIdle Handler is already hooked up</summary>
        private bool FIdleSet = false;

        /// <summary>Number of rows that would fit into the Grid at its current horizontal size.</summary>
        public Int32 PageSize
        {
            get
            {
                Int32 ReturnValue;

                if (FGridInitialised)
                {
                    ReturnValue = FPageSize;
                }
                else
                {
                    throw new TDataGridPagedNotInitialisedException(
                        "The " + this.GetType().FullName + " control is not properly initialised yet. " +
                        "The LoadFirstDataPage method needs to be called before the PageSize property can be accessed");
                }

                return ReturnValue;
            }
        }

        /// <summary>Number of records resulting from the query</summary>
        public Int32 TotalRecords
        {
            get
            {
                Int32 ReturnValue;

                if (FDataTransferDone)
                {
                    ReturnValue = FTotalRecords;
                }
                else
                {
                    throw new TDataGridPagedNoDataLoadedYetException(
                        "The " + this.GetType().FullName + " control has not loaded any data yet. " +
                        "The LoadFirstDataPage method needs to be called before the TotalRecords property can be accessed");
                }

                return ReturnValue;
            }
        }

        /// <summary>Number of Pages resulting from the query</summary>
        public Int32 TotalPages
        {
            get
            {
                Int32 ReturnValue;

                if (FDataTransferDone)
                {
                    ReturnValue = FTotalPages;
                }
                else
                {
                    throw new TDataGridPagedNoDataLoadedYetException(
                        "The " + this.GetType().FullName + " control has not loaded any data yet. " +
                        "The LoadFirstDataPage method needs to be called before the TotalPages property can be accessed");
                }

                return ReturnValue;
            }
        }

        /// <summary>DataTable that holds all Pages of data (also empty ones that are not retrieved yet!)</summary>
        public DataTable PagedDataTable
        {
            get
            {
                DataTable ReturnValue;

                if (FPagedDataTable != null)
                {
                    ReturnValue = FPagedDataTable;
                }
                else
                {
                    throw new TDataGridPagedNoDataLoadedYetException(
                        "The " + this.GetType().FullName + " control has not loaded any data yet. " +
                        "The LoadFirstDataPage method needs to be called before the PagedDataTable property can be accessed");
                }

                return ReturnValue;
            }
        }

        /// <summary>Set to true after the first page of data is loaded</summary>
        public Boolean IsInitialised
        {
            get
            {
                return FGridInitialised;
            }
        }
        /**
         * This property gets hidden because it doesn't work with sgrdDataGridPaged!
         *
         */
        [Category("AutoFind"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Browsable(false),
         Description("Determines which AutoFindMode should be used.")]
        public new TAutoFindModeEnum AutoFindMode
        {
            get
            {
                if (!InDesignMode)
                {
                    throw new TDataGridPagedAutoFindNotSupportedException("The AutoFind functionality is not supported in sgrdDataGridPaged");
                }

                return TAutoFindModeEnum.NoAutoFind;
            }
            set
            {
                if (value != TAutoFindModeEnum.NoAutoFind)
                {
                    if (!InDesignMode)
                    {
                        throw new TDataGridPagedAutoFindNotSupportedException("The AutoFind functionality is not supported in sgrdDataGridPaged");
                    }
                }
            }
        }

        /**
         * This property gets hidden because it doesn't work with sgrdDataGridPaged!
         *
         */
        [Category("AutoFind"),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Browsable(false),
         Description("Determines which Column of the DataGrid should be enabled for AutoFind (Note: This is not the DataColumn of the DataView!).")]
        public new Int16 AutoFindColumn
        {
            get
            {
                if (!InDesignMode)
                {
                    throw new TDataGridPagedAutoFindNotSupportedException("The AutoFind functionality is not supported in sgrdDataGridPaged");
                }

                return -1;
            }
            set
            {
                if (value != -1)
                {
                    if (!InDesignMode)
                    {
                        throw new TDataGridPagedAutoFindNotSupportedException("The AutoFind functionality is not supported in sgrdDataGridPaged");
                    }
                }
            }
        }

        /**
         * This Event is thrown when a data page is about to be loaded from the Server.
         *
         */
        [Category("DataPage"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when a data page is about to be loaded from the Server.")]
        public event TDataPageLoadingEventHandler DataPageLoading;

        /**
         * This Event is thrown when a data page loaded from the Server.
         *
         */
        [Category("DataPage"),
         Browsable(true),
         RefreshPropertiesAttribute(System.ComponentModel.RefreshProperties.All),
         Description("Occurs when a data page is loaded from the Server.")]
        public event TDataPageLoadedEventHandler DataPageLoaded;


        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            //
            // TSgrdDataGridPaged
            //
            this.Name = "TSgrdDataGridPaged";
        }

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public TSgrdDataGridPaged() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            FTransferredDataPages = new ArrayList();
            FGridInitialised = false;

            // Prevent the Application.Idle Event from being fired after the
            // Control has been disposed.
            this.Disposed += new System.EventHandler(this.OnDisposed);
        }

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
        /// </summary>
        /// <returns>void</returns>
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

        #region Main functionality

        /// <summary>
        /// Gets called as soon as the Server function has finished returning the first
        /// page of data.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void DataTransferDone()
        {
            DataRow EmptyRow;

            // MessageBox.Show('FTotalRecords: ' + FTotalRecords.ToString + '; FPageSize: ' + FPageSize.ToString);
            // Add empty rows if needed (these allow scrolling in the DataGrid!)
            try
            {
                if (FTotalRecords > FPageSize)
                {
                    for (int Counter = 0; Counter <= (FTotalRecords - FPageSize - 1); Counter += 1)
                    {
                        EmptyRow = FPagedDataTable.NewRow();
                        FPagedDataTable.Rows.Add(EmptyRow);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Empty rows cannot be added to the grid (because of DB constraints)", "Exception");
            }

            FDataTransferDone = true;

            // Setup the ArrayList that keeps track of which pages of data have already been retrieved
            ResetPaging();
        }

        /// <summary>
        /// Needs to be called as soon as it is desired to display the first page of data.
        ///
        /// @comment All further pages are loaded by the control on demand.
        ///
        /// </summary>
        /// <param name="ADelegateGetDataPagedResultFunction">Delegate function that gets called
        /// when a Page of data needs to be retrieved.
        /// </param>
        /// <returns>void</returns>
        public DataTable LoadFirstDataPage(TDelegateGetDataPagedResult ADelegateGetDataPagedResultFunction)
        {
            DataTable ReturnValue;
            TDataPageLoadEventArgs CustomEventArgs;

            DeterminePageSize();
            FGetDataPagedResult = ADelegateGetDataPagedResultFunction;
            FLastHeight = this.Height;

            if (FGetDataPagedResult != null)
            {
                // Fire OnDataPageLoading event.
                CustomEventArgs = new TDataPageLoadEventArgs();
                CustomEventArgs.DataPage = 0;
                this.OnDataPageLoading(CustomEventArgs);

                // Fetch the first page of data
                FPagedDataTable = FGetDataPagedResult(0, FPageSize, out FTotalRecords, out FTotalPages);
                ReturnValue = FPagedDataTable;
                DataTransferDone();

                // Fire OnDataPageLoaded event.
                CustomEventArgs = new TDataPageLoadEventArgs();
                CustomEventArgs.DataPage = 0;
                this.OnDataPageLoaded(CustomEventArgs);
            }
            else
            {
                throw new TDataGridPagedDelegateFunctionNotSpecifiedException(
                    "The " + this.GetType().FullName + " control is not properly initialised yet. " +
                    "The ADelegateGetDataPagedResultFunction parameter of the InitialiseGrid method needs to be set to the delegate function that returns a page of data");
            }

            FGridInitialised = true;

            return ReturnValue;
        }

        /// <summary>
        /// Loads a single data page into the paged table.
        ///
        /// </summary>
        /// <param name="ANeededPage">Page number of the data page to retrieve.
        /// </param>
        /// <returns>void</returns>
        private void LoadSingleDataPage(Int32 ANeededPage)
        {
            DataTable PagedTable;
            TDataPageLoadEventArgs CustomEventArgs;
            Int16 Counter;

            // Sanity check  just in case someone made the Grid so small that no Rows would be displayed...
            if (ANeededPage > 0)
            {
                // Fire OnDataPageLoading event.
                CustomEventArgs = new TDataPageLoadEventArgs();
                CustomEventArgs.DataPage = ANeededPage;
                this.OnDataPageLoading(CustomEventArgs);

                // MessageBox.Show('Retrieving Page ' + ANeededPage.ToString + '...');

                Int32 CurrentTotalRecords;  // These two values should be the same as FTotalRecords
                Int16 CurrentTotalPages;    // and FTotalPages, which were set when the first page was loaded.
                PagedTable = FGetDataPagedResult((short)ANeededPage, FPageSize, out CurrentTotalRecords, out CurrentTotalPages);

                if (PagedTable != null)
                {
                    FTransferredDataPages.Add(ANeededPage);

                    // MessageBox.Show('Inserting Page ' + ANeededPage.ToString + ' (PageSize: ' + FPageSize.ToString + '; Records returned: ' +  PagedTable.Rows.Count.ToString + ')...');
                    for (Counter = 0; Counter <= PagedTable.Rows.Count - 1; Counter += 1)
                    {
                        FPagedDataTable.Rows[(ANeededPage * FPageSize) + Counter].ItemArray = PagedTable.Rows[Counter].ItemArray;
                    }
                }

                // Fire OnDataPageLoaded event.
                CustomEventArgs = new TDataPageLoadEventArgs();
                CustomEventArgs.DataPage = ANeededPage;
                this.OnDataPageLoaded(CustomEventArgs);
            }
        }

        /// <summary>
        /// Loads all data pages that are not yet loaded into the paged table.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void LoadAllDataPages()
        {
            Int32 Counter;

            for (Counter = 0; Counter <= FTotalPages - 1; Counter += 1)
            {
                if (!FTransferredDataPages.Contains(Counter))
                {
                    // MessageBox.Show('LoadSingleDataPageIntoPagedTable(' + Counter.ToString + ')');
                    LoadSingleDataPage(Counter);
                }
            }

            /* Note: This procedure has a potential to reduce the amount of remoting traffic
             * when all pages should be loaded.
             * It could be rewritten to approximately gauge whether it makes more sense to
             * load the missing pages, each with a call to LoadSingleDataPage (if there
             * are only a few missing) or to call a (new) delegate function that would
             * simply retrieve the whole DataTable (containing all data pages) from the
             * Server. The second option might be more effective in terms of remoting
             * traffic and also faster, since only one remoting call would be made and the
             * schema information of the DataTable would be transferred only once.
             */
        }

        /// <summary>
        /// Initialises or re-sets variables. Needed before (new) data pages can be
        /// loaded.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void ResetPaging()
        {
            // Setup the ArrayList that keeps track of which pages of data have already been retrieved
            FTransferredDataPages.Clear();
            FTransferredDataPages.Add(Convert.ToInt32(0));
            FPerformFullLoadOnDataGridSort = false;
        }

        #endregion

        #region Helper functions

        /// <summary>
        /// Determines the PageSize, that is the number of rows that would fit into the
        /// Grid at its current vertical size.
        /// Takes the height of a possible visible HScrollBar and the height of a possible
        /// visible DataRowHeader into consideration.
        /// </summary>
        /// <returns>void</returns>
        private void DeterminePageSize()
        {
            Int32 HSize;
            Int32 HeaderHeight;
            Int32 RowHeight = 0;

            HSize = this.Height;

//            TLogging.Log("DetermineMaxNumberOfDisplayableRows:  HSize (initial): " + HSize.ToString());

            // If horizontal ScrollBar is visible: reduce the horizontal size by its height
            if (this.HScrollBarVisible)
            {
                HSize = HSize - this.HScrollBar.Size.Height;

//                TLogging.Log("DetermineMaxNumberOfDisplayableRows:  HSize (after reducing it by the HScrollBar Height): " + HSize.ToString());
            }

            // If a Header is visible: reduce the horizontal size by its height
            if ((this.Columns.Count != 0) && (this.Columns[1].HeaderCell != null))
            {
                HeaderHeight = Rows.GetHeight(0);

                // Let RowHeight be the same than HeaderHeight; this is the best we can assume to be accurate
                RowHeight = HeaderHeight;

                if (HeaderHeight != 0)
                {
                    HSize = HSize - HeaderHeight;

//                    TLogging.Log("DetermineMaxNumberOfDisplayableRows:  HSize (after reducing it by the Header Height): " + HSize.ToString());
                }
            }

            if (RowHeight == 0)
            {
                // RowHeight couldn't be derived from HeaderHeight because no header was visible - take MinimumHeight of Rows; this is the best we can assume to be accurate
                RowHeight = this.MinimumHeight;
            }

            FPageSize = Convert.ToInt16(((float)HSize / (float)RowHeight));   // + 1

            // Sanity check  just in case someone made the Grid so small that no Rows would be displayed...
            if (FPageSize <= 1)
            {
                FPageSize = TSgrdDataGridPaged.DEFAULT_PAGESIZE_IF_GRID_TOO_SMALL;
            }

//            TLogging.Log("DetermineMaxNumberOfDisplayableRows:  final FPageSize: " + FPageSize.ToString());
        }

        /// <summary>
        /// Special property to determine whether our code is running in the WinForms Designer.
        /// The result of this property is correct even if InitializeComponent() wasn't run yet
        /// (.NET's DesignMode property returns false in that case)!
        /// </summary>
        protected bool InDesignMode
        {
            get
            {
                return (this.GetService(typeof(IDesignerHost)) != null)
                       || (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime);
            }
        }

        #endregion

        #region Overridden Events

        /// <summary>
        /// size has changed (especially height); perhaps need to display more rows
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(System.EventArgs e)
        {
            Int32 HSize = this.Height;

            base.OnSizeChanged(e);

//            TLogging.Log("OnSizeChanged");

            if (HSize > FLastHeight)
            {
                // Grid is being made higher
                FLastHeight = HSize;

                if ((FPagedDataTable != null) && (FPagedDataTable.Rows.Count != 0))
                {
                    // There is data in the Grid
                    // MessageBox.Show('OnSizeChanged:  HSize;' + HSize.ToString);
                    OnVScrollPositionChanged(null);
                }
            }
            else
            {
                // Grid is being made less high
                FLastHeight = HSize;
            }
        }

        /// <summary>
        /// Defers the Resize Event until Application.Idle Event fires.
        /// </summary>
        /// <remarks>Deferring the Resize Event makes for smooth resizing of the
        /// Grid. The Grid is still redrawing (and loading not-yet-loaded DataRows),
        /// but the time-consuming calculations that the Grid performs once OnResize
        /// is called (eg. calculating the scrollbar positions) are deferred.</remarks>
        /// <param name="e">Not evaluated.</param>
        protected override void OnResize(EventArgs e)
        {
            if (!FIdleSet)
            {
                FIdleSet = true;

//                TLogging.Log("OnResize: Hooked up Application.Idle.");
                Application.Idle += new EventHandler(this.OnIdle);
            }
        }

        /// <summary>
        /// Raises the Resize Event once the Application isn't busy anymore.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Not evaluated.</param>
        private void OnIdle(object sender, EventArgs e)
        {
            FIdleSet = false;

            Application.Idle -= new EventHandler(this.OnIdle);

//             TLogging.Log("OnIdle: Calling base.OnResize.");
            this.Cursor = Cursors.WaitCursor;
            base.OnResize(e);
            this.Cursor = Cursors.Default;

//             TLogging.Log("OnIdle: Called base.OnResize.");
        }

        /// <summary>
        /// Prevents the Application.Idle Event from being fired
        /// after the Control has been disposed.
        /// </summary>
        /// <param name="sender">Not evaluated.</param>
        /// <param name="e">Not evaluated.</param>
        private void OnDisposed(object sender, EventArgs e)
        {
            Application.Idle -= new EventHandler(this.OnIdle);
        }

        /// <summary>
        /// when sorting
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSortingRangeRows(SourceGrid.SortRangeRowsEventArgs e)
        {
            base.OnSortingRangeRows(e);

            if (!FPerformFullLoadOnDataGridSort)
            {
                FPerformFullLoadOnDataGridSort = true;
                LoadAllDataPages();
            }
        }

        /// <summary>
        /// the grid has been scrolled vertically, different rows need to be displayed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnVScrollPositionChanged(SourceGrid.ScrollPositionChangedEventArgs e)
        {
            Int32 TopRowNumber;
            Int32 BottomRowNumber;
            Int32 Counter;
            Int32 CheckPage;
            Int32 LastCheckedPage = -1;

            base.OnVScrollPositionChanged(e);

//            TLogging.Log("OnVScrollPositionChanged.  PageSize: " + FPageSize.ToString());

            if (FPageSize != 0)
            {
                TopRowNumber = this.RangeAtArea(CellPositionType.Scrollable).Start.Row - 1; // this.ScrollablePanel.RangeAtDisplayRect(this.ScrollablePanel.ClientRectangle).Start.Row - 1;
                BottomRowNumber = this.RangeAtArea(CellPositionType.Scrollable).End.Row - 1; // this.ScrollablePanel.RangeAtDisplayRect(this.ScrollablePanel.ClientRectangle).End.Row - 1;

                // Need to increase BottomRowNumber by one to cater for possibly
                // partly visible next Grid Row that the user could scroll to without
                // causing an OnVScrollPositionChanged Event!
                BottomRowNumber = BottomRowNumber + 1;

//                TLogging.Log("TopRowNumber: " + TopRowNumber.ToString() + "; BottomRowNumber: " + BottomRowNumber.ToString());

                for (Counter = TopRowNumber; Counter <= BottomRowNumber; Counter++)
                {
                    CheckPage = (int)((float)Counter / (float)FPageSize);

//                    TLogging.Log("CheckPage: " + CheckPage.ToString());

                    if ((CheckPage != LastCheckedPage) && (CheckPage < FTotalPages))
                    {
//                        TLogging.Log("Checking if Page #" + CheckPage.ToString() + " is already transfered...");
                        if (!FTransferredDataPages.Contains(CheckPage))
                        {
//                            TLogging.Log("Page #" + CheckPage.ToString() + " is NOT transfered yet, requesting it from PetraServer...");
                            LoadSingleDataPage(CheckPage);
                        }

                        LastCheckedPage = CheckPage;
                    }
                }
            }
        }

        #endregion

        #region Custom Events
        private void OnDataPageLoading(TDataPageLoadEventArgs e)
        {
            // MessageBox.Show('OnDataPageLoading');
            if (DataPageLoading != null)
            {
                DataPageLoading(this, e);
            }
        }

        private void OnDataPageLoaded(TDataPageLoadEventArgs e)
        {
            // MessageBox.Show('OnDataPageLoaded');
            if (DataPageLoaded != null)
            {
                DataPageLoaded(this, e);
            }
        }

        #endregion
    }
    #endregion


    #region Exceptions
    #region TDataGridPagedNotInitialisedException

    /// <summary>
    /// error when not initialised
    /// </summary>
    public class TDataGridPagedNotInitialisedException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TDataGridPagedNotInitialisedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public TDataGridPagedNotInitialisedException(String msg) : base(msg)
        {
        }
    }
    #endregion

    #region TDataGridPagedNoDataLoadedYetException

    /// <summary>
    /// no data available yet
    /// </summary>
    public class TDataGridPagedNoDataLoadedYetException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TDataGridPagedNoDataLoadedYetException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public TDataGridPagedNoDataLoadedYetException(String msg) : base(msg)
        {
        }
    }
    #endregion

    #region TDataGridPagedDelegateFunctionNotSpecifiedException

    /// <summary>
    /// no delegate
    /// </summary>
    public class TDataGridPagedDelegateFunctionNotSpecifiedException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TDataGridPagedDelegateFunctionNotSpecifiedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public TDataGridPagedDelegateFunctionNotSpecifiedException(String msg) : base(msg)
        {
        }
    }
    #endregion

    #region TDataGridPagedAutoFindNotSupportedException

    /// <summary>
    /// auto find not supported
    /// </summary>
    public class TDataGridPagedAutoFindNotSupportedException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TDataGridPagedAutoFindNotSupportedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public TDataGridPagedAutoFindNotSupportedException(String msg) : base(msg)
        {
        }
    }
    #endregion
    #endregion


    /// <summary>Delegate declaration</summary>
    public delegate DataTable TDelegateGetDataPagedResult(Int16 ANeededPage, Int16 APageSize, out Int32 ATotalRecords, out Int16 ATotalPages);

    /// <summary>Event handler declaration</summary>
    public delegate void TDataPageLoadingEventHandler(System.Object Sender, TDataPageLoadEventArgs e);

    /// <summary>Event handler declaration</summary>
    public delegate void TDataPageLoadedEventHandler(System.Object Sender, TDataPageLoadEventArgs e);


    /// <summary>
    /// Event Arguments declaration
    /// </summary>
    public class TDataPageLoadEventArgs : System.EventArgs
    {
        /// <summary>
        /// number of data page
        /// </summary>
        public Int32 DataPage;
    }
}