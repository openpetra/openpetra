//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timh, christiank
//
// Copyright 2004-2015 by OM International
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms.Logic;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using SourceGrid;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// Location Find screen.
    /// </summary>
    public partial class TLocationFindDialogWinForm : System.Windows.Forms.Form,
           Ict.Petra.Client.CommonForms.IFrmPetra
    {
        private const String StrSearchTargetText = "Location";
        private const String StrSearchTargetPluralText = "Locations";

        #region Fields

        private readonly Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;
        private IPartnerUIConnectorsPartnerLocationFind FLocationFindObject;
        private DataTable FPagedDataTable;
        private DataRow FDefaultValues;
        private bool FKeepUpSearchFinishedCheck;

        #endregion

        #region Properties

        /// <summary>
        /// The Location that was selected by the user. Null if none was selected.
        /// </summary>
        public TLocationPK SelectedLocation
        {
            get
            {
                DataRowView[] miRows;
                Int64 miSiteKey;
                Int64 miLocationKey;
                TLocationPK miLocationPK;

                if (grdResult != null)
                {
                    miRows = grdResult.SelectedDataRowsAsDataRowView;

                    if (miRows.Length <= 0)
                    {
                        // No Row is selected!
                        return null;
                    }
                }
                else
                {
                    // Grid not available = no row available!
                    return null;
                }

                miSiteKey = Convert.ToInt64(miRows[0][PLocationTable.GetSiteKeyDBName()]);
                miLocationKey = Convert.ToInt64(miRows[0][PLocationTable.GetLocationKeyDBName()]);
                miLocationPK = new TLocationPK(miSiteKey, (Int32)miLocationKey);

                return miLocationPK;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public TLocationFindDialogWinForm() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            FDefaultValues = FFindCriteriaDataTable.NewRow();

            lblSearchInfo.Text = "";
            grdResult.SendToBack();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="AParentForm"></param>
        public TLocationFindDialogWinForm(Form AParentForm) : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentForm, this, stbMain);

            FLocationFindObject = TRemote.MPartner.Partner.UIConnectors.PartnerLocationFind();

            FPetraUtilsObject.InitActionState();
            FDefaultValues = FFindCriteriaDataTable.NewRow();

            lblSearchInfo.Text = "";
            grdResult.SendToBack();

            // catch enter on all controls, to trigger search or accept (could use this.AcceptButton, but we have a search button and an Accept Button)
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CatchEnterKey);

            this.Closed += new System.EventHandler(this.TLocationFindDialogWinForm_Closed);
        }

        #endregion

        #region Private Methods

        #region Main functionality

        /// <summary>
        /// Checks if the internal CriteriaDataTable has any criteria to search for.
        /// </summary>
        /// <returns>true if there are any search criterias </returns>
        private bool HasSearchCriteria()
        {
            string CurrentColumnsContent;

            if (FFindCriteriaDataTable.Rows.Count != 1)
            {
                return true;
            }

            DataRow SearchDataRow = FFindCriteriaDataTable.NewRow();
            SearchDataRow.ItemArray = FFindCriteriaDataTable.Rows[0].ItemArray;

            for (int Counter = 0; Counter < SearchDataRow.ItemArray.Length; ++Counter)
            {
                CurrentColumnsContent = SearchDataRow[Counter].ToString();

                if (FFindCriteriaDataTable.Columns[Counter].ColumnName.EndsWith("Match"))
                {
                    // ignore changes of the Values like "ExactPartnerKeyMatch" or
                    // "EmailMatch" or "Address3Match"...
                    // because just a change in these values doesn't mean that there is a search criteria
                    continue;
                }

                if (FFindCriteriaDataTable.Columns[Counter].ColumnName.CompareTo("PartnerStatus") == 0)
                {
                    if ((CurrentColumnsContent == "ALL")
                        || (CurrentColumnsContent == "ACTIVE"))
                    {
                        // if there is partner status "All" or "Active" marked
                        // treat it as if there is no search criteria selected
                        continue;
                    }
                    else
                    {
                        return true;
                    }
                }

                if ((CurrentColumnsContent != FDefaultValues[Counter].ToString())
                    && (CurrentColumnsContent != "*")
                    && (CurrentColumnsContent != "%"))
                {
                    if ((CurrentColumnsContent.Length > 1)
                        && ((CurrentColumnsContent.StartsWith("*"))
                            || (CurrentColumnsContent.StartsWith("%"))))
                    {
                        // Ensure that the whole string doesn't consist just of * characters
                        for (int CharCounter = 1; CharCounter < CurrentColumnsContent.Length; CharCounter++)
                        {
                            if ((CurrentColumnsContent[CharCounter] != '*')
                                && (CurrentColumnsContent[CharCounter] != '%'))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Starts the Search operation.
        /// </summary>
        /// <returns>void</returns>
        private void PerformSearch()
        {
            // Start the asynchronous search operation on the PetraServer
            var CriteriaDataTable = ProcessWildCardsAndStops(FFindCriteriaDataTable);

            FLocationFindObject.PerformSearch(CriteriaDataTable);

            timerSearchResults.Enabled = true;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ANeededPage"></param>
        /// <param name="APageSize"></param>
        /// <param name="ATotalRecords"></param>
        /// <param name="ATotalPages"></param>
        /// <returns></returns>
        private DataTable GetDataPagedResult(Int16 ANeededPage, Int16 APageSize, out Int32 ATotalRecords, out Int16 ATotalPages)
        {
            ATotalRecords = 0;
            ATotalPages = 0;

            if (FLocationFindObject != null)
            {
                return FLocationFindObject.GetDataPagedResult(ANeededPage, APageSize, out ATotalRecords, out ATotalPages);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Enables and disables the UI. Invokes setting up of the Grid after a
        /// successful search operation.
        /// </summary>
        private void EnableDisableUI(bool AEnable)
        {
            String SearchTarget;

            pnlBtnOKCancelHelpLayout.Enabled = Convert.ToBoolean(AEnable);

            if (Convert.ToBoolean(AEnable))
            {
                if (FLocationFindObject.Progress.JobFinished)
                {
                    if (FPagedDataTable.Rows.Count > 0)
                    {
                        /* hide message */
                        Application.DoEvents();

                        this.Cursor = Cursors.Default;

                        /*
                         * Setup result DataGrid
                         */
                        SetupResultDataGrid();

                        grdResult.Visible = true;

                        grdResult.BringToFront();

                        // Make the Grid respond on updown keys
                        grdResult.Focus();
                        // this is needed so that the next 'Enter' press acts as if the the 'Accept' Button got pressed
                        grdResult.Selection.Focus(new Position(1, 2), true);

                        // Display the number of found Locations
                        if (grdResult.TotalRecords > 1)
                        {
                            SearchTarget = StrSearchTargetPluralText;
                        }
                        else
                        {
                            SearchTarget = StrSearchTargetText;
                        }

                        grpResult.Text = MPartnerResourcestrings.StrSearchResult + ": " + grdResult.TotalRecords.ToString() + ' ' + SearchTarget +
                                         ' ' +
                                         MPartnerResourcestrings.StrFoundText;

                        btnOK.Enabled = true;
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;

                        /* no results, inform user, then no further action */
                        lblSearchInfo.Text = MPartnerResourcestrings.StrNoRecordsFound1Text + ' ' + StrSearchTargetText +
                                             MPartnerResourcestrings.StrNoRecordsFound2Text;
                        lblSearchInfo.BringToFront();
                        btnOK.Enabled = false;
                        grdResult.Enabled = false;

                        Application.DoEvents();
                    }
                }
                else
                {
                    /* Search operation interrupted by user */
                    this.Cursor = Cursors.Default;
                    grpResult.Text = MPartnerResourcestrings.StrSearchResult;
                    lblSearchInfo.Text = MPartnerResourcestrings.StrSearchStopped;

                    btnSearch.Enabled = true;
                    btnOK.Enabled = false;

                    // StatusBar update
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrGenericReady);
                    FPetraUtilsObject.SetStatusBarText(btnSearch, MPartnerResourcestrings.StrSearchButtonHelpText);
                    Application.DoEvents();

                    // Ensure Client doesn't crash in case user presses <Tab> followed by <Enter>
                    grdResult.Dispose();
                    grdResult = null;
                }
            }

            /* Enable/disable remaining controls */
            btnClearCriteria.Enabled = Convert.ToBoolean(AEnable);
            tlpAllCriteria.Enabled = Convert.ToBoolean(AEnable);

            /* Set search button text */
            if (Convert.ToBoolean(AEnable))
            {
                btnSearch.Text = MPartnerResourcestrings.StrSearchButtonText;
            }
            else
            {
                btnSearch.Text = MPartnerResourcestrings.StrSearchButtonStopText;
            }
        }

        #endregion

        #region Helper Methods

        private void EnableDisableAllCriteria(bool AEnable)
        {
            pnlAddr1.Enabled = AEnable;
            pnlAddr2.Enabled = AEnable;
            pnlAddr3.Enabled = AEnable;
            pnlCity.Enabled = AEnable;
            pnlPostCode.Enabled = AEnable;
            pnlCounty.Enabled = AEnable;
            cmbCountry.Enabled = AEnable;
        }

        private void SetSearchCriteriaDefaultValues(ref DataRow SingleDataRow)
        {
            SingleDataRow["Addr1"] = "";
            SingleDataRow["Addr1Match"] = "BEGINS";
            SingleDataRow["Street2"] = "";
            SingleDataRow["Street2Match"] = "BEGINS";
            SingleDataRow["Addr3"] = "";
            SingleDataRow["Addr3Match"] = "BEGINS";
            SingleDataRow["City"] = "";
            SingleDataRow["CityMatch"] = "BEGINS";
            SingleDataRow["PostCode"] = "";
            SingleDataRow["PostCodeMatch"] = "BEGINS";
            SingleDataRow["County"] = "";
            SingleDataRow["CountyMatch"] = "BEGINS";
            SingleDataRow["LocationKey"] = "";
            SingleDataRow["Country"] = "";

            AssociateCriteriaButtons();
        }

        private void ResetSearchCriteriaValuesToDefault()
        {
            DataRow SingleDataRow;

            if (FFindCriteriaDataTable.Rows.Count != 0)
            {
                SingleDataRow = FFindCriteriaDataTable.Rows[0];
                SingleDataRow.BeginEdit();
                SetSearchCriteriaDefaultValues(ref SingleDataRow);
                SingleDataRow.EndEdit();
            }
            else
            {
                SingleDataRow = FFindCriteriaDataTable.NewRow();
                SetSearchCriteriaDefaultValues(ref SingleDataRow);
                FFindCriteriaDataTable.Rows.Add(SingleDataRow);
            }

            FFindCriteriaDataTable.Rows[0].BeginEdit();
            FDefaultValues.ItemArray = FFindCriteriaDataTable.Rows[0].ItemArray;
        }

        private void AssociateCriteriaButtons()
        {
            critAddress1.AssociatedTextBox = txtAddress1;
            critAddress2.AssociatedTextBox = txtAddress2;
            critAddress3.AssociatedTextBox = txtAddress3;
            critPostCode.AssociatedTextBox = txtPostCode;
            critCity.AssociatedTextBox = txtCity;
            critCounty.AssociatedTextBox = txtCounty;

            critAddress1.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(@TFindscreensHelper.RemoveJokersFromTextBox);
            critAddress2.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(@TFindscreensHelper.RemoveJokersFromTextBox);
            critAddress3.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(@TFindscreensHelper.RemoveJokersFromTextBox);
            critPostCode.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(@TFindscreensHelper.RemoveJokersFromTextBox);
            critCity.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(@TFindscreensHelper.RemoveJokersFromTextBox);
            critCounty.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(@TFindscreensHelper.RemoveJokersFromTextBox);
        }

        /// <summary>
        /// Changes any * character(s) in the middle of a Search Criteria's text into % character(s)
        /// for all textual Search Criteria into which the user can type text. This is to make the
        /// SQL-92 'LIKE' operator do what the user intended. The only case when this isn't done is
        /// when the Search Criteria's text starts with || AND ends with ||. This signalises that the
        /// Search Criteria's text is to be taken absolutely literally, that is, wild card characters are
        /// to be processed as the characters they really are, and not as wildcards.
        /// </summary>
        /// <remarks>IMPORTANT: The Method must work with a *copy* of ACriteriaDT and apply data changes only in there as
        /// otherwise the Search Criterias' text on the screen gets updated (eg. * characters would get replaced with %
        /// characters on screen)!</remarks>
        /// <param name="ACriteriaDT">DataTable holding the one DataRow that contains the Search Criteria data.</param>
        /// <returns>New DataTable holding the one DataRow that contains the Search Criteria Data in which the wildcard
        /// and 'stops' processing was applied to the relevant Search Criteria.</returns>
        private DataTable ProcessWildCardsAndStops(DataTable ACriteriaDT)
        {
            DataTable ReturnValue = ACriteriaDT.Copy();
            DataRow IndividualDR = ReturnValue.Rows[0];

            IndividualDR["Addr1"] = TFindscreensHelper.ReplaceWildCardsInMiddleOfSearchCriteriaAndRemoveStops(IndividualDR["Addr1"].ToString());
            IndividualDR["Street2"] = TFindscreensHelper.ReplaceWildCardsInMiddleOfSearchCriteriaAndRemoveStops(IndividualDR["Street2"].ToString());
            IndividualDR["Addr3"] = TFindscreensHelper.ReplaceWildCardsInMiddleOfSearchCriteriaAndRemoveStops(IndividualDR["Addr3"].ToString());
            IndividualDR["City"] = TFindscreensHelper.ReplaceWildCardsInMiddleOfSearchCriteriaAndRemoveStops(IndividualDR["City"].ToString());
            IndividualDR["PostCode"] = TFindscreensHelper.ReplaceWildCardsInMiddleOfSearchCriteriaAndRemoveStops(IndividualDR["PostCode"].ToString());
            IndividualDR["County"] = TFindscreensHelper.ReplaceWildCardsInMiddleOfSearchCriteriaAndRemoveStops(IndividualDR["County"].ToString());

            return ReturnValue;
        }

        private void ReleaseServerObject()
        {
            FLocationFindObject = null;
        }

        #endregion

        #endregion

        #region Setup SourceDataGrid

        private void CreateGrid()
        {
            this.grdResult = new Ict.Common.Controls.TSgrdDataGridPaged();

            this.grdResult.AutoFindColumn = ((short)(-1));
            this.grdResult.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdResult.CancelEditingWithEscapeKey = false;
            this.grdResult.DeleteQuestionMessage = "You have chosen to delete this record.\'#13#10#13#10\'Do you really want to delete" +
                                                   " it?";
            this.grdResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResult.FixedColumns = 2;     // Make City/Town and Post Code fixed columns
            this.grdResult.FixedRows = 1;
            this.grdResult.KeepRowSelectedAfterSort = false;
            this.grdResult.Location = new System.Drawing.Point(6, 17);
            this.grdResult.MinimumHeight = 21;
            this.grdResult.Name = "grdResult";
            this.grdResult.Size = new System.Drawing.Size(504, 85);
            this.grdResult.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows | SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) |
                                               SourceGrid.GridSpecialKeys.Shift)));
            this.grdResult.TabIndex = 0;
            this.grdResult.TabStop = true;
            this.grdResult.ToolTipText = "";
            this.grdResult.ToolTipTextDelegate = null;
            this.grdResult.Visible = true;
            this.grdResult.DataPageLoaded += new Ict.Common.Controls.TDataPageLoadedEventHandler(this.GrdResults_DataPageLoaded);
            this.grdResult.DataPageLoading += new Ict.Common.Controls.TDataPageLoadingEventHandler(this.GrdResults_DataPageLoading);
            this.grdResult.DoubleClickCell += new Ict.Common.Controls.TDoubleClickCellEventHandler(this.GrdResults_DoubleClickCell);
            this.grdResult.EnterKeyPressed += new Ict.Common.Controls.TKeyPressedEventHandler(this.GrdResults_EnterKeyPressed);

            this.grpResult.Controls.Add(this.grdResult);

            pnlBlankSearchResult.BringToFront();

            FPetraUtilsObject.SetStatusBarText(grdResult,
                MPartnerResourcestrings.StrResultGridHelpText2 + MPartnerResourcestrings.StrPartnerFindSearchTargetText);
        }

        private void CreateColumns()
        {
            string Tmp;
            string LocalisedCountyLabel;

            LocalisedStrings.GetLocStrCounty(out LocalisedCountyLabel, out Tmp);

            // Done this way in case it changes
            LocalisedCountyLabel = LocalisedCountyLabel.Replace(":", "").Replace("&", "");

            grdResult.Columns.Clear();

            grdResult.AddTextColumn("City", FPagedDataTable.Columns[PLocationTable.GetCityDBName()]);
            grdResult.AddTextColumn("Post Code", FPagedDataTable.Columns[PLocationTable.GetPostalCodeDBName()]);
            grdResult.AddTextColumn("Addr1", FPagedDataTable.Columns[PLocationTable.GetLocalityDBName()]);
            grdResult.AddTextColumn("Street-2", FPagedDataTable.Columns[PLocationTable.GetStreetNameDBName()]);
            grdResult.AddTextColumn("Addr3", FPagedDataTable.Columns[PLocationTable.GetAddress3DBName()]);
            grdResult.AddTextColumn(LocalisedCountyLabel, FPagedDataTable.Columns[PLocationTable.GetCountyDBName()]);
            grdResult.AddTextColumn("Country", FPagedDataTable.Columns[PLocationTable.GetCountryCodeDBName()]);
            grdResult.AddTextColumn("Location Key", FPagedDataTable.Columns[PLocationTable.GetLocationKeyDBName()]);
            grdResult.AddTextColumn("SiteKey", FPagedDataTable.Columns[PLocationTable.GetSiteKeyDBName()]);

            grdResult.AutoResizeGrid();
        }

        /// <summary>
        /// Sets up paged search result DataTable with the result of the Servers query
        /// and DataBind the DataGrid.
        /// </summary>
        private void SetupResultDataGrid()
        {
            try
            {
                if (grdResult.TotalPages > 0)
                {
                    /* Create SourceDataGrid columns */
                    CreateColumns();

//TLogging.Log("SetupResultDataGrid: Before calling SetupDataGridDataBinding()...");

                    /* DataBindingrelated stuff */
                    TFindscreensHelper.SetupDataGridDataBinding(grdResult, FPagedDataTable);
//TLogging.Log("SetupResultDataGrid: Before calling SetupDataGridVisualAppearance()...");

                    /* Setup the DataGrid's visual appearance */
                    TFindscreensHelper.SetupDataGridVisualAppearance(grdResult);
//TLogging.Log("SetupResultDataGrid: Before calling SelectRow()...");
                    // For speed reasons we must add the necessary amount of emtpy Rows only here (after .AutoSizeCells() has already
                    // been run! See XML Comment on the called Method TSgrdDataGridPaged.AddEmptyRows() for details!
                    grdResult.AddEmptyRows();
//TLogging.Log("After AddEmptyRows()");

                    /* Select (highlight) first Row */
                    grdResult.Selection.SelectRow(1, true);

                    /* Scroll grid to first line (the grid might have been scrolled before to another position) */
                    grdResult.ShowCell(new Position(1, 1), true);

                    btnOK.Enabled = true;
                }
                else
                {
                    btnOK.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception occured in SetupResultDataGrid: " + exp.Message);
            }
        }

        #endregion

        #region Threading Helper Functions

        /// <summary>
        /// This polls the server until the search has finished,
        /// </summary>
        /// <returns>void</returns>
        private void TimerSearchResults_Tick(System.Object sender, System.EventArgs e)
        {
            TProgressState state = FLocationFindObject.Progress;

            if (state.JobFinished)
            {
                /* we are finished: */
                /* prevent further calls */
                timerSearchResults.Enabled = false;
                FKeepUpSearchFinishedCheck = false;

                /* Fetch the first page of data */
                try
                {
                    // For speed reasons we must add the necessary amount of emtpy Rows only *after* .AutoSizeCells()
                    // has already been run! See XML Comment on the called Method
                    // TSgrdDataGridPaged.LoadFirstDataPage for details!
                    FPagedDataTable = grdResult.LoadFirstDataPage(@GetDataPagedResult, false);
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.ToString());
                }

                EnableDisableUI(true);
            }
            else if (state.CancelJob)
            {
                /* we are finished: */
                /* prevent further calls */
                timerSearchResults.Enabled = false;
                FKeepUpSearchFinishedCheck = false;

                EnableDisableUI(true);

            }
        }

        #endregion

        #region Event Handling

        #region Form-related Events

        private void TLocationFindDialogWinForm_Load(System.Object sender, System.EventArgs e)
        {
            string LocalisedCountyLabel;
            string dummy;

            if (!DesignMode)
            {
                this.Cursor = Cursors.WaitCursor;

                cmbCountry.PerformDataBinding(FFindCriteriaDataTable, "Country");
                cmbCountry.AddNotSetRow("", "");

                ResetSearchCriteriaValuesToDefault();

                LocalisedStrings.GetLocStrCounty(out LocalisedCountyLabel, out dummy);
                lblCounty.Text = LocalisedCountyLabel;

                /* Set status bar texts */
                FPetraUtilsObject.SetStatusBarText(txtAddress1, MPartnerResourcestrings.StrAddress1Helptext);
                FPetraUtilsObject.SetStatusBarText(txtAddress2, MPartnerResourcestrings.StrAddress2Helptext);
                FPetraUtilsObject.SetStatusBarText(txtAddress3, MPartnerResourcestrings.StrAddress3Helptext);
                FPetraUtilsObject.SetStatusBarText(txtCity, MPartnerResourcestrings.StrCityHelptext);
                FPetraUtilsObject.SetStatusBarText(txtCounty, MPartnerResourcestrings.StrCountyHelpText);
                FPetraUtilsObject.SetStatusBarText(cmbCountry, MPartnerResourcestrings.StrCountryHelpText);
                FPetraUtilsObject.SetStatusBarText(txtPostCode, MPartnerResourcestrings.StrPostCodeHelpText);
                FPetraUtilsObject.SetStatusBarText(txtLocationKey, MPartnerResourcestrings.StrLocationKeyHelpText);
                FPetraUtilsObject.SetStatusBarText(btnOK, MPartnerResourcestrings.StrAcceptButtonHelpText + StrSearchTargetText);
                FPetraUtilsObject.SetStatusBarText(btnCancel, MPartnerResourcestrings.StrCancelButtonHelpText + StrSearchTargetText);
                FPetraUtilsObject.SetStatusBarText(btnClearCriteria, MPartnerResourcestrings.StrClearCriteriaButtonHelpText);
                FPetraUtilsObject.SetStatusBarText(btnSearch, MPartnerResourcestrings.StrSearchButtonHelpText);

                this.Cursor = Cursors.Default;
            }
        }

        private void TLocationFindDialogWinForm_Activated(System.Object sender, System.EventArgs e)
        {
            txtAddress1.Focus();
        }

        private void TLocationFindDialogWinForm_Closed(System.Object sender, System.EventArgs e)
        {
            ReleaseServerObject();
        }

        #endregion

        #region Key Events

        private void GeneralKeyHandler(TextBox ATextBox, SplitButton ACriteriaControl, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                /* Without this  databinding fails */
                ACriteriaControl.ShowContextMenu();
            }
        }

        private void CatchEnterKey(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // make sure that the 'Enter' key has not been pressed to select a value from a combo boxes dropped down list
                if (!ComboboxDroppedDown())
                {
                    BtnSearch_Click(sender, e);

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            else
            {
                e.Handled = false;
            }
        }

        private void TxtAddress1_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtAddress1, critAddress1, e);
        }

        private void TxtAddress1_Leave(System.Object sender, EventArgs e)
        {
            TFindscreensHelper.CriteriaTextBoxLeaveHandler(FFindCriteriaDataTable, txtAddress1, critAddress1);
        }

        private void TxtAddress2_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtAddress2, critAddress2, e);
        }

        private void TxtAddress2_Leave(System.Object sender, EventArgs e)
        {
            TFindscreensHelper.CriteriaTextBoxLeaveHandler(FFindCriteriaDataTable, txtAddress2, critAddress2);
        }

        private void TxtAddress3_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtAddress3, critAddress3, e);
        }

        private void TxtAddress3_Leave(System.Object sender, EventArgs e)
        {
            TFindscreensHelper.CriteriaTextBoxLeaveHandler(FFindCriteriaDataTable, txtAddress3, critAddress3);
        }

        private void TxtCounty_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtCounty, critCounty, e);
        }

        private void TxtCounty_Leave(System.Object sender, EventArgs e)
        {
            TFindscreensHelper.CriteriaTextBoxLeaveHandler(FFindCriteriaDataTable, txtCounty, critCounty);
        }

        private void TxtPostCode_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtPostCode, critPostCode, e);
        }

        private void TxtPostCode_Leave(System.Object sender, EventArgs e)
        {
            /* capitalise when leaving control */
            txtPostCode.Text = txtPostCode.Text.ToUpper();

            TFindscreensHelper.CriteriaTextBoxLeaveHandler(FFindCriteriaDataTable, txtPostCode, critPostCode);
        }

        private void TxtCity_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtCity, critCity, e);
        }

        private void TxtCity_Leave(System.Object sender, EventArgs e)
        {
            TFindscreensHelper.CriteriaTextBoxLeaveHandler(FFindCriteriaDataTable, txtCity, critCity);
        }

        private void TxtLocationKey_KeyPress(System.Object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            e.Handled = false;

            if (System.Char.IsLetter(e.KeyChar) == true)
            {
                e.Handled = true;
            }

            if (System.Char.IsPunctuation(e.KeyChar) == true)
            {
                e.Handled = true;
            }

            if (System.Char.IsSymbol(e.KeyChar) == true)
            {
                e.Handled = true;
            }
        }

        private void TxtLocationKey_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // When any key pressed in LocationKey
            // See how long the text is
            // If longer than 0, disable other controls
            if (txtLocationKey.Text.Length == 0)
            {
                EnableDisableAllCriteria(true);
            }

            if (txtLocationKey.Text.Length > 0)
            {
                EnableDisableAllCriteria(false);
            }
        }

        #endregion

        #region Comboboxes and the Enter key

        // This is used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down.

        private bool CountryDroppedDown = false;

        /// <summary>
        /// Determines if a combo box's value has been changed while the list is dropped down
        /// and that that combo box still contains the focus.
        /// </summary>
        /// <returns></returns>
        private bool ComboboxDroppedDown()
        {
            if (CountryDroppedDown && cmbCountry.ContainsFocus)
            {
                CountryDroppedDown = false;
                return true;
            }

            CountryDroppedDown = false;

            return false;
        }

        // events triggered when a combo box's value is changed

        private void UcoCountryComboBox_SelectedValueChanged(System.Object sender, System.EventArgs e)
        {
            // if the list is dropped down while the value is changed (not the case when a value from the list is clicked on)
            if (cmbCountry.DroppedDown)
            {
                CountryDroppedDown = true;
            }
        }

        #endregion

        #region Button Click-Events

        private void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }

        /// <summary>
        /// This starts the ball rolling for a search.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(System.Object sender, System.EventArgs e)
        {
            if (!FKeepUpSearchFinishedCheck)
            {
                // get the data from the currently edited control;
                // otherwise there are not the right search criteria when hitting the enter key
                this.BindingContext[FFindCriteriaDataTable].EndCurrentEdit();

                btnSearch.Focus();

                if (!HasSearchCriteria())
                {
                    MessageBox.Show(MPartnerResourcestrings.StrNoCriteriaSpecified, Catalog.GetString("Location Find"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return;
                }

                CreateGrid();

                /* MessageBox.Show(dtCriteria.DataSet.GetXml.ToString(),'dtCriteria'); */
                Application.DoEvents();
                lblSearchInfo.Visible = true;
                lblSearchInfo.Text = MPartnerResourcestrings.StrSearching;
                grdResult.SendToBack();
                grpResult.Text = MPartnerResourcestrings.StrSearchResult;

                this.Cursor = Cursors.AppStarting;
                FKeepUpSearchFinishedCheck = true;
                EnableDisableUI(false);
                Application.DoEvents();

                /* Clear result table */
                try
                {
                    FPagedDataTable = null;
                }
                catch (Exception)
                {
                    // don't do anything since this happens if the DataTable has no data yet
                }

                var CriteriaDataTable = ProcessWildCardsAndStops(FFindCriteriaDataTable);

                // Start asynchronous search operation
                PerformSearch();
            }
            else
            {
                /* Asynchronous search operation is being interrupted */
                btnSearch.Enabled = false;
                lblSearchInfo.Text = MPartnerResourcestrings.StrStoppingSearch;
                FPetraUtilsObject.WriteToStatusBar(MPartnerResourcestrings.StrStoppingSearch);
                FPetraUtilsObject.SetStatusBarText(btnSearch, MPartnerResourcestrings.StrStoppingSearch);

                Application.DoEvents();

                /* Stop asynchronous search operation */
                FLocationFindObject.StopSearch();
            }
        }

        /// <summary>
        /// Reset all criteria fields to default values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearCriteria_Click(System.Object sender, System.EventArgs e)
        {
            ResetSearchCriteriaValuesToDefault();

            EnableDisableAllCriteria(true);

            lblSearchInfo.Text = "";
            grpResult.Text = MPartnerResourcestrings.StrSearchResult;

            if (grdResult != null)
            {
                this.grpResult.Controls.Remove(grdResult);
                grdResult = null;
            }

            txtAddress1.Focus();

            btnOK.Enabled = false;

            FPagedDataTable = null;
        }

        private void BtnHelp_Click(System.Object sender, System.EventArgs e)
        {
            // TODO
        }

        #endregion

        #region Grid-related Events

        private void GrdResults_DataPageLoading(System.Object Sender, TDataPageLoadEventArgs e)
        {
            /* MessageBox.Show('DataPageLoading:  Page: ' + e.DataPage.ToString); */
            if (e.DataPage > 0)
            {
                this.Cursor = Cursors.WaitCursor;
                FPetraUtilsObject.WriteToStatusBar(MPartnerResourcestrings.StrTransferringDataForPageText + e.DataPage.ToString() + ")");
                FPetraUtilsObject.SetStatusBarText(grdResult, MPartnerResourcestrings.StrTransferringDataForPageText + e.DataPage.ToString() + ')');
            }
        }

        private void GrdResults_DataPageLoaded(System.Object Sender, TDataPageLoadEventArgs e)
        {
            /* MessageBox.Show('DataPageLoaded:  Page: ' + e.DataPage.ToString); */
            if (e.DataPage > 0)
            {
                this.Cursor = Cursors.Default;
                FPetraUtilsObject.WriteToStatusBar(MPartnerResourcestrings.StrResultGridHelpText2 + StrSearchTargetText);
                FPetraUtilsObject.SetStatusBarText(grdResult,
                    MPartnerResourcestrings.StrResultGridHelpText2 + MPartnerResourcestrings.StrPartnerFindSearchTargetText);
            }
        }

        private void GrdResults_DoubleClickCell(System.Object Sender, SourceGrid.CellContextEventArgs e)
        {
            BtnOK_Click(this, null);
        }

        private void GrdResults_EnterKeyPressed(object Sender, SourceGrid.RowEventArgs e)
        {
            BtnOK_Click(this, null);
        }

        #endregion

        #endregion

        #region Implement interface functions

        /// normally this would be auto generated
        public void RunOnceOnActivation()
        {
        }

        /// normally this would be auto generated
        public bool CanClose()
        {
            return FPetraUtilsObject.CanClose();
        }

        /// normally this would be auto generated
        public TFrmPetraUtils GetPetraUtilsObject()
        {
            return (TFrmPetraUtils)FPetraUtilsObject;
        }

        #endregion
    }
}
