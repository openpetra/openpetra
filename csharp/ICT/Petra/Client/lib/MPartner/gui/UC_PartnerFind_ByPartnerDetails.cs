//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Windows.Forms;
using System.Data;
using System.Threading;
using SourceGrid;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// Description of TUC_PartnerFind_ByPartnerDetails.
    /// </summary>
    public partial class TUC_PartnerFind_ByPartnerDetails : UserControl
    {
        /// <summary>DataTable containing the search criteria</summary>
        private DataTable FCriteriaData;

        /// <summary>DataTable that holds all Pages of data (also empty ones that are not retrieved yet!)</summary>
        private DataTable FPagedDataTable;

        /// <summary>The number of the current Row in the Find Results Grid</summary>
        /// <remarks>Somehow, the Grid's own method of determining the current Position
        /// (Grid.Selection.ActivePosition) has sometimes problems, that's why we need
        /// to keep track of things manually  :-(</remarks>
        private int FCurrentGridRow = -1;

        /// <summary>Last PartnerKey for which the Partner Info Panel was opened.</summary>
        private Int64 FLastPartnerKeyInfoPanelOpened = -1;

        /// <summary>Last LocationKey for which the Partner Info Panel was opened.</summary>
        private TLocationPK FLastLocationPKInfoPanelOpened = new TLocationPK();

        /// <summary>Tells the screen whether it should still wait for the Server's result</summary>
        private Boolean FKeepUpSearchFinishedCheck;

        /// <summary>Tells whether the last search operation was done with 'Detailed Results' enabled</summary>
        private Boolean FLastSearchWasDetailedSearch;

        /// <summary>Tells whether any change occured in the Search Criteria since the last search operation</summary>
        private Boolean FCriteriaContentChanged;

        // <summary>If the Form should set the Focus to the LocationKey field, set the LocationKey to this value</summary>
// TODO        private Int32 FPassedLocationKey;
        /// <summary>Object that holds the logic for this screen</summary>
        private TPartnerFindScreenLogic FLogic;

        /// <summary>The Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerFind FPartnerFindObject;

//TODO        private Int32 FSplitterDistForm;
        private Int32 FSplitterDistFindByDetails;
        private bool FPartnerInfoPaneOpen = false;
        private bool FPartnerTasksPaneOpen = false;

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_PartnerFind_ByPartnerDetails()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnSearch.Text = Catalog.GetString(" &Search");
            this.chkDetailedResults.Text = Catalog.GetString("Detailed Results");
            this.btnClearCriteria.Text = Catalog.GetString("Clea&r");
            this.grpCriteria.Text = Catalog.GetString("Find Criteria");
            this.btnCustomCriteriaDemo.Text = Catalog.GetString("Custom Criteria Demo");
            this.grpResult.Text = Catalog.GetString("Fin&d Result");
            this.lblSearchInfo.Text = Catalog.GetString("Searching...");
            #endregion

            // Define the screen's Logic
            FLogic = new TPartnerFindScreenLogic();
            FLogic.ParentForm = this;
            FLogic.DataGrid = grdResult;

            lblSearchInfo.Text = "";
            grdResult.SendToBack();


            // Set up Partner Info pane
            FPartnerInfoPaneOpen = TUserDefaults.GetBooleanDefault(
                TUserDefaults.PARTNER_FIND_PARTNERDETAILS_OPEN, false);

            if (!FPartnerInfoPaneOpen)
            {
                ClosePartnerInfoPane();
            }
            else
            {
                OpenPartnerInfoPane();
            }
        }

        private TFrmPetraUtils FPetraUtilsObject;

        /// <summary>
        /// this provides general functionality for edit screens
        /// </summary>
        public TFrmPetraUtils PetraUtilsObject
        {
            get
            {
                return FPetraUtilsObject;
            }
            set
            {
                FPetraUtilsObject = value;

// todo: no resourcestrings
                FPetraUtilsObject.SetStatusBarText(btnSearch, Resourcestrings.StrSearchButtonHelpText);
                FPetraUtilsObject.SetStatusBarText(btnClearCriteria, Resourcestrings.StrClearCriteriaButtonHelpText);
                FPetraUtilsObject.SetStatusBarText(grdResult, Resourcestrings.StrResultGridHelpText + Resourcestrings.StrPartnerFindSearchTargetText);
                FPetraUtilsObject.SetStatusBarText(chkDetailedResults, Resourcestrings.StrDetailedResultsHelpText);
            }
        }

        /// <summary>
        /// needed for generated code
        /// </summary>
        public void InitUserControl()
        {
        }

        private void GrdResult_MouseDown(System.Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int PointY = -1;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                PointY = new Point(e.X, e.Y).Y;

                //                TLogging.Log("GrdResult_MouseDown: grdResult.Rows.RowAtPoint: " + grdResult.Rows.RowAtPoint(PointY).Value.ToString());

                grdResult.Selection.ResetSelection(false);
                grdResult.Selection.SelectRow(grdResult.Rows.RowAtPoint(PointY).Value, true);

                DataGrid_FocusRowEntered(this,
                    new RowEventArgs(grdResult.Rows.RowAtPoint(PointY).Value));

                // show the context menu:
                // TODO mnuPartnerFindContext.Show(grdResult, new Point(e.X, e.Y));
            }
        }

        private void ChkDetailedResults_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            ArrayList FieldList;

            if (FPagedDataTable != null)
            {
                // prepare a list of columns
                FieldList = new ArrayList();

                foreach (String eachField in ucoPartnerFindCriteria.CriteriaFieldsLeft)
                {
                    FieldList.Add(eachField);
                }

                foreach (String eachField in ucoPartnerFindCriteria.CriteriaFieldsRight)
                {
                    FieldList.Add(eachField);
                }

                if (!FCriteriaContentChanged)
                {
                    if (chkDetailedResults.Checked == false)
                    {
                        // Reduce the columns that are shown in the Grid
                        FLogic.CreateColumns(FPagedDataTable, false, FCriteriaData.Rows[0]["PartnerStatus"].ToString() != "ACTIVE", FieldList);
                        SetupDataGridVisualAppearance();
                        grdResult.Selection.SelectRow(FCurrentGridRow, true);
                    }
                    else
                    {
                        if (FLastSearchWasDetailedSearch)
                        {
                            // Show all columns in the Grid
                            FLogic.CreateColumns(FPagedDataTable, true, FCriteriaData.Rows[0]["PartnerStatus"].ToString() != "ACTIVE", FieldList);
                            SetupDataGridVisualAppearance();
                            grdResult.Selection.SelectRow(FCurrentGridRow, true);
                        }
                        else
                        {
                            BtnSearch_Click(this, null);
                        }
                    }
                }
                else
                {
                    BtnSearch_Click(this, null);
                }
            }
        }

        private void GrdResult_DoubleClickCell(System.Object Sender, SourceGrid.CellContextEventArgs e)
        {
            if (TPartnerFindScreen.URunAsModalForm == true)
            {
                BtnAccept_Click(this, null);
            }
            else
            {
                OpenPartnerEditScreen(TPartnerEditTabPageEnum.petpAddresses);
            }
        }

        private void GrdResult_EnterKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            if (TPartnerFindScreen.URunAsModalForm == true)
            {
                BtnAccept_Click(this, null);
            }
            else
            {
                OpenPartnerEditScreen(TPartnerEditTabPageEnum.petpAddresses);
            }
        }

        private void BtnAccept_Click(System.Object sender, System.EventArgs e)
        {
            // TODO: call the form BtnAccept_Click, by using an interface for the partnerfind class???
            ParentForm.DialogResult = System.Windows.Forms.DialogResult.OK;
            ParentForm.Close();
        }

        private void GrdResult_SpaceKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            TogglePartnerInfoPane();
        }

        private void GrdResult_KeyPressed(System.Object Sender, KeyEventArgs e)
        {
            //            MessageBox.Show(e.KeyCode.ToString());
            if (e.KeyCode == Keys.Apps)
            {
                //                grdResult.Focus();
                //                Position ActivePos = grdResult.Selection.ActivePosition;

                //                MessageBox.Show("ActivePos.Row: " + ActivePos.Row.ToString());

                /*
                 * Show the context menu:
                 *   X coordinate: 2/3 of the Width of the Grid
                 *   Y coordinate: Top of the current Row + 3/4 of the Height of the Row
                 */

                // TODO context menu
#if TODO
                mnuPartnerFindContext.Show(grdResult,
                    new Point((grdResult.Width - (grdResult.Width / 3)),
                        (grdResult.Rows.GetTop(FCurrentGridRow) +
                         (grdResult.Rows.GetHeight(FCurrentGridRow) / 4) * 3)));
#endif
            }
        }

        private void GrdResult_DataPageLoading(System.Object Sender, TDataPageLoadEventArgs e)
        {
            // MessageBox.Show('DataPageLoading:  Page: ' + e.DataPage.ToString);
            if (e.DataPage > 0)
            {
                this.Cursor = Cursors.WaitCursor;
                FPetraUtilsObject.WriteToStatusBar(Resourcestrings.StrTransferringDataForPageText + e.DataPage.ToString() + ')');
                FPetraUtilsObject.SetStatusBarText(grdResult, Resourcestrings.StrTransferringDataForPageText + e.DataPage.ToString() + ')');
            }
        }

        private void GrdResult_DataPageLoaded(System.Object Sender, TDataPageLoadEventArgs e)
        {
            //          MessageBox.Show("DataPageLoaded:  Page: " + e.DataPage.ToString());
            if (e.DataPage > 0)
            {
                this.Cursor = Cursors.Default;
                FPetraUtilsObject.WriteToStatusBar(Resourcestrings.StrResultGridHelpText + Resourcestrings.StrPartnerFindSearchTargetText);
                FPetraUtilsObject.SetStatusBarText(grdResult, Resourcestrings.StrResultGridHelpText + Resourcestrings.StrPartnerFindSearchTargetText);
            }
        }

        /// <summary>
        /// Event Handler for FocusRowEntered Event of the Grid
        /// </summary>
        /// <param name="ASender">The Grid</param>
        /// <param name="AEventArgs">RowEventArgs as specified by the Grid (use Row property to
        /// get the Grid Row for which this Event fires)
        /// </param>
        /// <returns>void</returns>
        private void DataGrid_FocusRowEntered(System.Object ASender, RowEventArgs AEventArgs)
        {
            FCurrentGridRow = AEventArgs.Row;

            FLogic.DetermineCurrentPartnerKey();

            //            MessageBox.Show("PartnerKey of newly selected Row: " + FLogic.PartnerKey.ToString());
            //            TLogging.Log("DataGrid_FocusRowEntered: PartnerKey of newly selected Row: " + FLogic.PartnerKey.ToString());

            // Update 'Partner Info' if this Panel is shown
            UpdatePartnerInfoPanel();
        }

        #region PartnerInfoPanel

        /// <summary>
        /// Updates the 'Partner Info' Panel if this Panel is shown and
        /// if there is a Find Result.
        /// </summary>
        /// <remarks>To avoid unneccessary roundtrips to the PetraServer, the
        /// 'Partner Info' Panel is only updated if the Partner/Location record
        /// combination has changed from the last time this Method was called.</remarks>
        void UpdatePartnerInfoPanel()
        {
            TLocationPK CurrentLocationPK;

            if (!ucoPartnerInfo.IsCollapsed)
            {
                CurrentLocationPK = FLogic.DetermineCurrentLocationPK();

                //                MessageBox.Show("Current PartnerKey: " + FLogic.PartnerKey.ToString() + Environment.NewLine +
                //                                "FLastPartnerKeyInfoPanelOpened: " + FLastPartnerKeyInfoPanelOpened.ToString() + Environment.NewLine +
                //                                "CurrentLocationPK: " + CurrentLocationPK.SiteKey.ToString() + ", " + CurrentLocationPK.LocationKey.ToString() + Environment.NewLine +
                //                                "FLastLocationPKInfoPanelOpened: " + FLastLocationPKInfoPanelOpened.SiteKey.ToString() + ", " + FLastLocationPKInfoPanelOpened.LocationKey.ToString());

                if ((FLogic.CurrentDataRow != null)
                    && (((FLastPartnerKeyInfoPanelOpened == FLogic.PartnerKey)
                         && ((FLastLocationPKInfoPanelOpened.SiteKey != CurrentLocationPK.SiteKey)
                             || (FLastLocationPKInfoPanelOpened.LocationKey != CurrentLocationPK.LocationKey)))
                        || (FLastPartnerKeyInfoPanelOpened != FLogic.PartnerKey)))
                {
                    FLastPartnerKeyInfoPanelOpened = FLogic.PartnerKey;
                    FLastLocationPKInfoPanelOpened = CurrentLocationPK;

                    if ((chkDetailedResults.Checked)
                        || ((!chkDetailedResults.Checked)
                            && FLastSearchWasDetailedSearch))
                    {
                        // We have Location data available
                        ucoPartnerInfo.PassPartnerDataPartialWithLocation(FLogic.PartnerKey, FLogic.CurrentDataRow);
                    }
                    else
                    {
                        // We don't have Location data available
                        ucoPartnerInfo.PassPartnerDataPartialWithoutLocation(FLogic.PartnerKey, FLogic.CurrentDataRow);
                    }
                }
            }
        }

        void UcoPartnerInfo_Collapsed(object sender, EventArgs e)
        {
            //            MessageBox.Show("UcoPartnerInfo_Collapsed");
            ClosePartnerInfoPane();
        }

        void UcoPartnerInfo_Expanded(object sender, EventArgs e)
        {
            //            MessageBox.Show("UcoPartnerInfo_Expanded");

            OpenPartnerInfoPane();
        }

        void TogglePartnerInfoPane()
        {
            if (!FPartnerInfoPaneOpen)
            {
                OpenPartnerInfoPane();
            }
            else
            {
                ClosePartnerInfoPane();
            }
        }

        void OpenPartnerInfoPane()
        {
            this.pnlPartnerInfoContainer.Height = TUC_PartnerFind_PartnerInfo.EXPANDEDHEIGHT;

// TODO            tbbPartnerInfo.Pushed = true;
// TODO            mniViewPartnerInfo.Checked = true;

            ucoPartnerInfo.Expand();

            UpdatePartnerInfoPanel();

            //            MessageBox.Show("FCurrentGridRow: " + FCurrentGridRow.ToString());
            if (FCurrentGridRow != -1)
            {
                // Scroll the selected Row into view
                // FIXME: this has an undesired side-effect if the selected Row is not hidden
                // behind the Partner Info pane, but is scrolled off the top of the Grid.
                //                bool CellWasVisible = grdResult.ShowCell(new Position(FCurrentGridRow, 0), false);

                /*
                 * TODO: Ideally. we would not have row displayed as the top row, but the bottm row.
                 * The approach below works - unless grdResult.ShowCell didn't put the row as the
                 * top row. No solution to that yet...
                 */

                //                if (!CellWasVisible)
                //                {
                //                    // Scrolling was necessary. Now the Row is displayed as the top row,
                //                    // but we want it to be displayed as the middle row. Therefore we need
                //                    // to scroll the Grid up until it is the middle row...
                ////                    MessageBox.Show("Scrolled FCurrentGridRow: " + FCurrentGridRow.ToString() + " into view.");
//
                //                    int ScrollAdditionalRows = (grdResult.Rows.LastVisibleScrollableRow.Value -
                //                        grdResult.Rows.FirstVisibleScrollableRow.Value) / 2;
                //                    MessageBox.Show("ScrollAdditionalRows: " + ScrollAdditionalRows.ToString());
//
                //                    for (int Counter = 1; Counter < ScrollAdditionalRows; Counter++)
                //                    {
                ////                        MessageBox.Show("about to scroll one line up...");
                //                        grdResult.CustomScrollLineUp();
                //                    }
                //                }
            }

            FPartnerInfoPaneOpen = true;
        }

        void ClosePartnerInfoPane()
        {
            this.pnlPartnerInfoContainer.Height = TUC_PartnerFind_PartnerInfo.COLLAPSEDHEIGHT;

// TODO            tbbPartnerInfo.Pushed = false;
// TODO            mniViewPartnerInfo.Checked = false;

            ucoPartnerInfo.Collapse();

            FPartnerInfoPaneOpen = false;
        }

        #endregion

        #region Main functionality

        /// <summary>
        /// Starts the Search operation.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void PerformSearch()
        {
            Thread FinishedCheckThread;

            // Start the asynchronous search operation on the PetraServer
            FPartnerFindObject.PerformSearch(FCriteriaData, chkDetailedResults.Checked);

            // Start thread that checks for the end of the search operation on the PetraServer
            FinishedCheckThread = new Thread(new ThreadStart(SearchFinishedCheckThread));
            FinishedCheckThread.Start();
        }

        /// <summary>
        /// Returns the values of the found partner.
        /// </summary>
        /// <param name="APartnerKey">Partner key</param>
        /// <param name="AShortName">Partner short name</param>
        /// <param name="ALocationPK">Location key</param>
        /// <returns></returns>
        public Boolean GetReturnedParameters(out Int64 APartnerKey, out String AShortName, out TLocationPK ALocationPK)
        {
            DataRowView[] SelectedGridRow = grdResult.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length <= 0)
            {
                // no Row is selected
                APartnerKey = -1;
                AShortName = "";
                ALocationPK = new TLocationPK(-1, -1);
            }
            else
            {
                // MessageBox.Show(SelectedGridRow[0]['p_partner_key_n'].ToString);
                APartnerKey = Convert.ToInt64(SelectedGridRow[0][PPartnerTable.GetPartnerKeyDBName()]);
                AShortName = Convert.ToString(SelectedGridRow[0][PPartnerTable.GetPartnerShortNameDBName()]);
                ALocationPK =
                    new TLocationPK(Convert.ToInt64(SelectedGridRow[0][PPartnerLocationTable.GetSiteKeyDBName()]),
                        Convert.ToInt32(SelectedGridRow[0][PPartnerLocationTable.GetLocationKeyDBName()]));
            }

            return true;
        }

        /// <summary>
        /// Event procedure that acts on ucoPartnerFindCriteria's OnCriteriaContentChanged
        /// event.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void UcoPartnerFindCriteria_CriteriaContentChanged(System.Object ASender, EventArgs AArgs)
        {
            btnSearch.Enabled = true;
            btnClearCriteria.Enabled = true;
            FCriteriaContentChanged = true;
        }

        #endregion

        #region Setup SourceDataGrid

        /// <summary>
        /// Sets up the DataBinding of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupDataGridDataBinding()
        {
            DataView FindResultPagedDV;

            FindResultPagedDV = FPagedDataTable.DefaultView;
            FindResultPagedDV.AllowNew = false;
            FindResultPagedDV.AllowDelete = false;
            FindResultPagedDV.AllowEdit = false;
            grdResult.DataSource = new DevAge.ComponentModel.BoundDataView(FindResultPagedDV);

            // Hook up event that fires when a different Row is selected
            grdResult.Selection.FocusRowEntered += new RowEventHandler(this.DataGrid_FocusRowEntered);
        }

        /// <summary>
        /// Sets up the visual appearance of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupDataGridVisualAppearance()
        {
            // Make PartnerClass and PartnerName fixed columns
            grdResult.FixedColumns = 2;
            grdResult.AutoSizeCells();
        }

        #endregion

        #region Form events

        /// <summary>
        /// set the criteria from user default
        /// </summary>
        public void InitialisePartnerFindCriteria()
        {
            ucoPartnerFindCriteria.PetraUtilsObject = FPetraUtilsObject;
            ucoPartnerFindCriteria.InitialiseUserControl();

            // Load items that should go into the left and right columns from User Defaults
            ucoPartnerFindCriteria.CriteriaFieldsLeft =
                new ArrayList(TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_FINDOPTIONS_CRITERIAFIELDSLEFT,
                        TFindOptionsForm.PARTNER_FINDOPTIONS_CRITERIAFIELDSLEFT_DEFAULT).Split(new Char[] { (';') }));
            ucoPartnerFindCriteria.CriteriaFieldsRight =
                new ArrayList(TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_FINDOPTIONS_CRITERIAFIELDSRIGHT,
                        TFindOptionsForm.PARTNER_FINDOPTIONS_CRITERIAFIELDSRIGHT_DEFAULT).Split(new Char[] { (';') }));

            ucoPartnerFindCriteria.DisplayCriteriaFieldControls();
            ucoPartnerFindCriteria.InitialiseCriteriaFields();

            // Load match button settings
            ucoPartnerFindCriteria.LoadMatchButtonSettings();

            // Register Event Handler for the OnCriteriaContentChanged event
            ucoPartnerFindCriteria.OnCriteriaContentChanged += new EventHandler(this.UcoPartnerFindCriteria_CriteriaContentChanged);
        }

        private void RestoreSplitterSettings()
        {
            spcPartnerFindByDetails.SplitterDistance = TUserDefaults.GetInt32Default(
                TUserDefaults.PARTNER_FIND_SPLITPOS_FINDBYDETAILS, 233);
            FSplitterDistFindByDetails = spcPartnerFindByDetails.SplitterDistance;
        }

        /// <summary>
        /// search for the partners with the currently entered criteria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnSearch_Click(System.Object sender, System.EventArgs e)
        {
            if (!FKeepUpSearchFinishedCheck)
            {
                // get the data from the currently edited control;
                // otherwise there are not the right search criteria when hitting the enter key
                ucoPartnerFindCriteria.CompleteBindings();

                FCriteriaData = ucoPartnerFindCriteria.CriteriaData;

                if (!ucoPartnerFindCriteria.HasSearchCriteria())
                {
                    MessageBox.Show(Resourcestrings.StrNoCriteriaSpecified);
                    return;
                }

                ucoPartnerFindCriteria.CriteriaData.Rows[0]["ExactPartnerKeyMatch"] = TUserDefaults.GetBooleanDefault(
                    TUserDefaults.PARTNER_FINDOPTIONS_EXACTPARTNERKEYMATCHSEARCH,
                    true);

                // used to destory server object here
                // Update UI
                grdResult.SendToBack();
                grpResult.Text = Resourcestrings.StrSearchResult;
                ucoPartnerInfo.ClearControls();
                lblSearchInfo.Text = Resourcestrings.StrSearching;
                FPetraUtilsObject.SetStatusBarText(btnSearch, Resourcestrings.StrSearching);

                //                stbMain.Panels[stbMain.Panels.IndexOf(stpInfo)].Text = Resourcestrings.StrSearching;

                this.Cursor = Cursors.AppStarting;
                FKeepUpSearchFinishedCheck = true;
                EnableDisableUI(false);
                Application.DoEvents();

                // If ctrl held down, show the dataset
#if DEBUGMODE
                if (System.Windows.Forms.Form.ModifierKeys == Keys.Control)
                {
                    MessageBox.Show(ucoPartnerFindCriteria.CriteriaData.DataSet.GetXml().ToString());
                    MessageBox.Show(ucoPartnerFindCriteria.CriteriaData.DataSet.GetChanges().GetXml().ToString());
                }
#endif

                // Clear result table
                try
                {
                    FPagedDataTable.Clear();
                }
                catch (Exception)
                {
                    // don't do anything since this happens if the DataTable has no data yet
                }

                // Reset internal status variables
                FLastSearchWasDetailedSearch = chkDetailedResults.Checked;
                FCriteriaContentChanged = false;
                FLastPartnerKeyInfoPanelOpened = -1;
                FLastLocationPKInfoPanelOpened = new TLocationPK(-1, -1);

                // Start asynchronous search operation
                this.PerformSearch();
            }
            else
            {
                // Asynchronous search operation is being interrupted
                btnSearch.Enabled = false;
                lblSearchInfo.Text = Resourcestrings.StrStoppingSearch;
                FPetraUtilsObject.WriteToStatusBar(Resourcestrings.StrStoppingSearch);
                FPetraUtilsObject.SetStatusBarText(btnSearch, Resourcestrings.StrStoppingSearch);

                Application.DoEvents();

                // Stop asynchronous search operation
                FPartnerFindObject.AsyncExecProgress.Cancel();
            }
        }

        private void BtnClearCriteria_Click(System.Object sender, System.EventArgs e)
        {
            ucoPartnerFindCriteria.ResetSearchCriteriaValuesToDefault();
            ucoPartnerInfo.ClearControls();
            lblSearchInfo.Text = "";
            grpResult.Text = Resourcestrings.StrSearchResult;
            grdResult.SendToBack();

// TODO            tbbEditPartner.Enabled = false;
// TODO            btnAccept.Enabled = false;
            ucoPartnerFindCriteria.Focus();

            grdResult.DataSource = null;
            FPagedDataTable = null;

            FCurrentGridRow = -1;
        }

        private void OpenPartnerEditScreen(TPartnerEditTabPageEnum AShowTabPage)
        {
            FPetraUtilsObject.WriteToStatusBar("Opening Partner in Partner Edit screen...");
            FPetraUtilsObject.SetStatusBarText(grdResult, "Opening Partner in Partner Edit screen...");
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // If ALT held down, show the conventional screen, otherwise the new, generated one.
                if (System.Windows.Forms.Form.ModifierKeys != Keys.Alt)
                {
                    TFrmPartnerEdit frm = new TFrmPartnerEdit(this.Handle);

                    frm.SetParameters(TScreenMode.smEdit, FLogic.PartnerKey,
                        FLogic.DetermineCurrentLocationPK().SiteKey, FLogic.DetermineCurrentLocationPK().LocationKey, AShowTabPage);
                    frm.Show();
                }
                else
                {
                    TFrmPartnerEdit frm = new TFrmPartnerEdit(this.Handle);

                    frm.SetParameters(TScreenMode.smEdit, FLogic.PartnerKey,
                        FLogic.DetermineCurrentLocationPK().SiteKey, FLogic.DetermineCurrentLocationPK().LocationKey, AShowTabPage);
                    frm.Show();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FPetraUtilsObject.SetStatusBarText(grdResult, Resourcestrings.StrResultGridHelpText + Resourcestrings.StrPartnerFindSearchTargetText);
            }
        }

        /// <summary>
        /// Opens the "copy address" dialog
        /// </summary>
        public void OpenCopyAddressToClipboardScreen()
        {
// TODO OpenCopyAddressToClipboardScreen
#if TODO
            TLocationPK LocationPK;

            Ict.Petra.Client.MPartner.
            TCopyPartnerAddressDialogWinForm cpad = new TCopyPartnerAddressDialogWinForm();

            LocationPK = FLogic.DetermineCurrentLocationPK();

            cpad.SetParameters(FLogic.PartnerKey, LocationPK.SiteKey, LocationPK.LocationKey);
            cpad.ShowDialog();
            cpad.Dispose();
#endif
        }

        private void BtnCustomCriteriaDemo_Click(System.Object sender, System.EventArgs e)
        {
            String[] CriteraFieldArray;
            CriteraFieldArray =
                "PartnerKey;abc;Spacer;Address1;Spacer;PartnerName;PersonalName;City;Country;PersonnelCriteria".Split(new Char[] { (';') });
            ucoPartnerFindCriteria.CriteriaFieldsLeft = new ArrayList(CriteraFieldArray);
            CriteraFieldArray = "OMSSKey;PartnerClass;PostCode;Status;Spacer;MailingAddressOnly".Split(new Char[] { (';') });
            ucoPartnerFindCriteria.CriteriaFieldsRight = new ArrayList(CriteraFieldArray);
            ucoPartnerFindCriteria.DisplayCriteriaFieldControls();
        }

        #endregion

        #region Helper functions

        /// <summary>
        /// Opens the Find Options dialog.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void OpenFindOptions()
        {
// TODO OpenFindOptions
#if TODO
            TFindOptionsForm OptionsDialog;

            OptionsDialog = new TFindOptionsForm();
            OptionsDialog.ShowDialog();

            if (OptionsDialog.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_FINDOPTIONS_CRITERIAFIELDSLEFT, "") != "")
                {
                    ucoPartnerFindCriteria.CriteriaFieldsLeft =
                        new ArrayList(TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_FINDOPTIONS_CRITERIAFIELDSLEFT,
                                "").Split(new Char[] { (';') }));
                }
                else
                {
                    ucoPartnerFindCriteria.CriteriaFieldsLeft = new ArrayList();
                }

                if (TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_FINDOPTIONS_CRITERIAFIELDSRIGHT, "") != "")
                {
                    ucoPartnerFindCriteria.CriteriaFieldsRight =
                        new ArrayList(TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_FINDOPTIONS_CRITERIAFIELDSRIGHT,
                                "").Split(new Char[] { (';') }));
                }
                else
                {
                    ucoPartnerFindCriteria.CriteriaFieldsRight = new ArrayList();
                }

                BtnClearCriteria_Click(this, null);
                ucoPartnerFindCriteria.DisplayCriteriaFieldControls();
                ucoPartnerFindCriteria.ShowOrHidePartnerKeyMatchInfoText();
            }
#endif
        }

        private void OpenPartnerFindForLocation()
        {
            TPartnerFindScreen frmPF;
            TLocationPK LocationPK;

            this.Cursor = Cursors.WaitCursor;
            LocationPK = FLogic.DetermineCurrentLocationPK();
            frmPF = new TPartnerFindScreen(this.Handle);
            frmPF.SetParameters(true, LocationPK.LocationKey);
            frmPF.Show();
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Retrieves some information of the partner and checks if the user can edit
        /// this partner.
        /// </summary>
        /// <param name="PartnerKey">The partner key to check.</param>
        /// <returns>True, if the partner can be edited, otherwise false</returns>
        private bool CanEditPartner(ref long PartnerKey)
        {
            // TODO: moved this function to Logic, CanAccessPartner
            TPartnerClass PartnerClass;
            String PartnerShortName;
            Boolean PartnerIsMerged;
            Boolean UserCanAccessPartner;

            TServerLookup.TMPartner.VerifyPartner(PartnerKey,
                out PartnerShortName, out PartnerClass, out PartnerIsMerged, out UserCanAccessPartner);

            if ((PartnerShortName.Length > 0)
                && UserCanAccessPartner
                && (PartnerKey > 0))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the the partner is merged. If so then show a dialog where the user can
        /// choose to work with the current partner or the merged partner.
        /// </summary>
        /// <param name="APartnerKey">The current partner to be checked</param>
        /// <param name="AMergedIntoPartnerKey">If the partner is merged the merged partner key.
        /// If the partner is not merged: -1</param>
        /// <returns>True if the user wants to work with the merged partner, otherwise false.</returns>
        public bool MergedPartnerHandling(Int64 APartnerKey,
            out Int64 AMergedIntoPartnerKey)
        {
            bool ReturnValue = false;

            AMergedIntoPartnerKey = -1;

// TODO MergedPartnerHandling
#if TODO
            bool IsMergedPartner;
            string MergedPartnerPartnerShortName;
            string MergedIntoPartnerShortName;
            TPartnerClass MergedPartnerPartnerClass;
            TPartnerClass MergedIntoPartnerClass;
            string MergedBy;
            DateTime MergeDate;

            IsMergedPartner = TServerLookup.TMPartner.MergedPartnerDetails(APartnerKey,
                out MergedPartnerPartnerShortName,
                out MergedPartnerPartnerClass,
                out AMergedIntoPartnerKey,
                out MergedIntoPartnerShortName,
                out MergedIntoPartnerClass,
                out MergedBy,
                out MergeDate);

            if (IsMergedPartner)
            {
                // Open the 'Merged Partner Info' Dialog
                using (TPartnerMergedPartnerInfoDialog MergedPartnerInfoDialog = new TPartnerMergedPartnerInfoDialog())
                {
                    MergedPartnerInfoDialog.SetParameters(APartnerKey,
                        MergedPartnerPartnerShortName,
                        MergedPartnerPartnerClass,
                        AMergedIntoPartnerKey,
                        MergedIntoPartnerShortName,
                        MergedIntoPartnerClass,
                        MergedBy,
                        MergeDate);

                    if (MergedPartnerInfoDialog.ShowDialog() == DialogResult.OK)
                    {
                        ReturnValue = true;
                    }
                    else
                    {
                        ReturnValue = false;
                    }
                }
            }
#endif
            return ReturnValue;
        }

        /// <summary>
        /// Enables and disables the UI. Invokes setting up of the Grid after a
        /// successful search operation.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void EnableDisableUI(System.Object AEnable)
        {
            object[] Args;
            TMyUpdateDelegate MyUpdateDelegate;
            String SearchTarget;
            bool UpdateUI;
            Int64 MergedPartnerKey;

            // Since this procedure is called from a separate (background) Thread, it is
            // necessary to executethis procedure in the Thread of the GUI
            if (btnSearch.InvokeRequired)
            {
                Args = new object[1];

                // messagebox.show('btnEditPartner.InvokeRequired: yes; AEnable: ' + Convert.ToBoolean(AEnable).ToString);
                try
                {
                    MyUpdateDelegate = new TMyUpdateDelegate(EnableDisableUI);
                    Args[0] = AEnable;
                    btnSearch.Invoke(MyUpdateDelegate, new object[] { AEnable });

                    // messagebox.show('Invoke finished!');
                }
                finally
                {
                    Args = new object[0];
                }
            }
            else
            {
                // Enable/disable buttons for working with found Partners
// TODO                tbbEditPartner.Enabled = Convert.ToBoolean(AEnable);

                // Enable/disable according to how the search operation ended
                if (Convert.ToBoolean(AEnable))
                {
                    if (FPartnerFindObject.AsyncExecProgress.ProgressState != TAsyncExecProgressState.Aeps_Stopped)
                    {
                        // Search operation ended without interruption
                        if (FPagedDataTable.Rows.Count > 0)
                        {
                            // At least one result was found by the search operation
                            lblSearchInfo.Text = "";
                            this.Cursor = Cursors.Default;

                            //
                            // Setup result DataGrid
                            //
                            SetupResultDataGrid();
                            grdResult.BringToFront();

                            // Make the Grid respond on updown keys
                            grdResult.Focus();
                            DataGrid_FocusRowEntered(this, new RowEventArgs(1));

                            // Display the number of found Partners/Locations
                            if (grdResult.TotalRecords > 1)
                            {
                                SearchTarget = Resourcestrings.StrPartnerFindSearchTargetPluralText;
                            }
                            else
                            {
                                SearchTarget = Resourcestrings.StrPartnerFindSearchTargetText;
                            }

                            grpResult.Text = Resourcestrings.StrSearchResult + ": " + grdResult.TotalRecords.ToString() + ' ' + SearchTarget + ' ' +
                                             Resourcestrings.StrFoundText;
                        }
                        else
                        {
                            // Search operation has found nothing

                            /*
                             * If PartnerKey wasn't 0, check if the Partner searched for
                             * was a MERGED Partner.
                             */
                            if ((Int64)FCriteriaData.Rows[0]["PartnerKey"] != 0)
                            {
                                if (MergedPartnerHandling(
                                        (Int64)FCriteriaData.Rows[0]["PartnerKey"],
                                        out MergedPartnerKey))
                                {
                                    FCriteriaData.Rows[0]["PartnerKey"] = MergedPartnerKey;
                                    BtnSearch_Click(this, null);
                                    UpdateUI = false;
                                }
                                else
                                {
                                    UpdateUI = true;
                                }
                            }
                            else
                            {
                                UpdateUI = true;
                            }

                            if (UpdateUI)
                            {
                                this.Cursor = Cursors.Default;
                                grpResult.Text = Resourcestrings.StrSearchResult;
                                lblSearchInfo.Text = Resourcestrings.StrNoRecordsFound1Text + ' ' + Resourcestrings.StrPartnerFindSearchTarget2Text +
                                                     Resourcestrings.StrNoRecordsFound2Text;

// TODO                                tbbEditPartner.Enabled = false;

                                // StatusBar update
                                FPetraUtilsObject.WriteToStatusBar(CommonResourcestrings.StrGenericReady);
                                FPetraUtilsObject.SetStatusBarText(btnSearch, Resourcestrings.StrSearchButtonHelpText);

                                FCurrentGridRow = -1;
                            }
                            else
                            {
                                return;
                            }
                        }

                        /*
                         * Saves 'Mailing Addresses Only' and 'Partner Status' Criteria
                         * settings to UserDefaults.
                         */
                        ucoPartnerFindCriteria.FindCriteriaUserDefaultSave();
                    }
                    else
                    {
                        // Search operation interrupted by user
                        // used to release server System.Object here
                        this.Cursor = Cursors.Default;
                        grpResult.Text = Resourcestrings.StrSearchResult;
                        lblSearchInfo.Text = Resourcestrings.StrSearchStopped;

// TODO                        tbbEditPartner.Enabled = false;
                        btnSearch.Enabled = true;

                        // StatusBar update

                        //                        WriteToStatusBar(CommonResourcestrings.StrGenericReady);
                        FPetraUtilsObject.SetStatusBarText(btnSearch, Resourcestrings.StrSearchButtonHelpText);

                        FCurrentGridRow = -1;
                    }
                }

                // Enable/disable remaining controls
                btnClearCriteria.Enabled = Convert.ToBoolean(AEnable);
                chkDetailedResults.Enabled = Convert.ToBoolean(AEnable);
                ucoPartnerFindCriteria.Enabled = Convert.ToBoolean(AEnable);

                // Set search button text
                if (Convert.ToBoolean(AEnable))
                {
                    btnSearch.Text = Resourcestrings.StrSearchButtonText;
                }
                else
                {
                    btnSearch.Text = Resourcestrings.StrSearchButtonStopText;
                }

                // messagebox.show('EnableDisableUI ran!');
            }
        }

        #endregion

        #region Threading helper functions

        /// <summary>
        /// Thread for the search operation. Monitor's the Server System.Object's
        /// AsyncExecProgress.ProgressState and invokes UI updates from that.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SearchFinishedCheckThread()
        {
            // Check whether this thread should still execute
            while (FKeepUpSearchFinishedCheck)
            {
                /* The next line of code calls a function on the PetraServer
                 * > causes a bit of data traffic everytime! */
                switch (FPartnerFindObject.AsyncExecProgress.ProgressState)
                {
                    case TAsyncExecProgressState.Aeps_Finished:
                        FKeepUpSearchFinishedCheck = false;

                        // Fetch the first page of data
                        try
                        {
                            FPagedDataTable = grdResult.LoadFirstDataPage(@GetDataPagedResult);
                        }
                        catch (Exception E)
                        {
                            MessageBox.Show(E.ToString());
                        }
                        break;

                    case TAsyncExecProgressState.Aeps_Stopped:
                        FKeepUpSearchFinishedCheck = false;
                        EnableDisableUI(true);
                        return;
                }

                // Sleep for some time. After that, this function is called again automatically.
                Thread.Sleep(200);
            }

            EnableDisableUI(true);
        }

        #endregion

        void BtnTogglePartnerDetailsClick(object sender, EventArgs e)
        {
            TogglePartnerInfoPane();
        }

        void spcPartnerFindByDetails_SplitterMoved(System.Object sender, System.Windows.Forms.SplitterEventArgs e)
        {
            FSplitterDistFindByDetails = ((SplitContainer)sender).SplitterDistance;
        }

        private void GrpCriteria_Enter(System.Object sender, System.EventArgs e)
        {
        }

        /// <summary>
        /// Sets up paged search result DataTable with the result of the Servers query
        /// and DataBind the DataGrid.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupResultDataGrid()
        {
            ArrayList FieldList;

            try
            {
                if (grdResult.TotalPages > 0)
                {
                    FieldList = new ArrayList();

                    foreach (string eachField in ucoPartnerFindCriteria.CriteriaFieldsLeft)
                    {
                        FieldList.Add(eachField);
                    }

                    foreach (string eachField in ucoPartnerFindCriteria.CriteriaFieldsRight)
                    {
                        FieldList.Add(eachField);
                    }

                    // Create SourceDataGrid columns
                    FLogic.CreateColumns(FPagedDataTable, chkDetailedResults.Checked,
                        FCriteriaData.Rows[0]["PartnerStatus"].ToString() != "ACTIVE", FieldList);

                    // DataBindingrelated stuff
                    SetupDataGridDataBinding();

                    // Setup the DataGrid's visual appearance
                    SetupDataGridVisualAppearance();

                    // Select (highlight) first Row
                    grdResult.Selection.SelectRow(1, true);

                    // Scroll grid to first line (the grid might have been scrolled before to another position)
                    grdResult.ShowCell(new Position(1, 1), true);

                    // TODO btnAccept.Enabled = true;
                }
                else
                {
                    // TODO btnAccept.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception occured in SetupResultDataGrid: " + exp.Message + exp.StackTrace);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ANeededPage"></param>
        /// <param name="APageSize"></param>
        /// <param name="ATotalRecords"></param>
        /// <param name="ATotalPages"></param>
        /// <returns></returns>
        public DataTable GetDataPagedResult(Int16 ANeededPage, Int16 APageSize, out Int32 ATotalRecords, out Int16 ATotalPages)
        {
            ATotalRecords = 0;
            ATotalPages = 0;

            if (FPartnerFindObject != null)
            {
                return FPartnerFindObject.GetDataPagedResult(ANeededPage, APageSize, out ATotalRecords, out ATotalPages);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// called when closing the window
        /// </summary>
        public void StoreUserDefaults()
        {
            // Save Find Criteria Match button settings
            ucoPartnerFindCriteria.SaveMatchButtonSettings();

            // Save SplitContainer settings
            ucoPartnerFindCriteria.SaveSplitterSetting();
            TUserDefaults.SetDefault(TUserDefaults.PARTNER_FIND_SPLITPOS_FINDBYDETAILS, spcPartnerFindByDetails.SplitterDistance);

            // Save Partner Info Pane and Partner Task Pane settings
            TUserDefaults.SetDefault(TUserDefaults.PARTNER_FIND_PARTNERDETAILS_OPEN, FPartnerInfoPaneOpen);
            TUserDefaults.SetDefault(TUserDefaults.PARTNER_FIND_PARTNERTASKS_OPEN, FPartnerTasksPaneOpen);
        }

        /// <summary>
        /// Pass on FRestrictToPartnerClasses to UC_PartnerFind_Criteria
        /// </summary>
        /// <param name="AInitiallyFocusLocationKey"></param>
        /// <param name="ARestrictToPartnerClasses">this will be an empty array
        ///     or an array of strings to determine which partner classes are allowed</param>
        /// <param name="APassedLocationKey"></param>
        public void Init(
            bool AInitiallyFocusLocationKey,
            string[] ARestrictToPartnerClasses, Int32 APassedLocationKey)
        {
            InitialisePartnerFindCriteria();
            ucoPartnerFindCriteria.RestrictedPartnerClass = ARestrictToPartnerClasses;

            if (AInitiallyFocusLocationKey)
            {
                ucoPartnerFindCriteria.FocusLocationKey(APassedLocationKey);
            }

            ucoPartnerFindCriteria.Focus();
        }

        /// <summary>todoComment</summary>
        public IPartnerUIConnectorsPartnerFind PartnerFindObject
        {
            set
            {
                FPartnerFindObject = value;
            }
        }
    }
}