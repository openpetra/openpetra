/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank, timh
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting;
using Ict.Common;
using Ict.Petra.Shared.MPartner;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using SourceGrid;
using System.Globalization;
using EWSoftware.StatusBarText;
using Ict.Petra.Client.MPartner;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TPartnerLocationFind : TFrmPetraDialog
    {
        public const String StrSearchTargetText = "Location";
        public const String StrSearchTargetPluralText = "Locations";

        /// <summary>Private Declarations</summary>
        private IPartnerUIConnectorsPartnerLocationFind FPartnerLocationFindObject;
        private Int32 FTotalRows;
        private Int16 FTotalPages;
        private DataTable FPagedDataTable;
        private String FDefaultCriteriaXML;
        private DataView FDataView;

        public TPartnerLocationFind() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnOK.Text = Catalog.GetString("&Accept");
            this.btnClearCriteria.Text = Catalog.GetString("Clea&r");
            this.grpCriteria.Text = Catalog.GetString("&Find Criteria:");
            this.lblPostCode.Text = Catalog.GetString("P&ost Code:");
            this.lblCounty.Text = Catalog.GetString("Co&unty:");
            this.lblCountry.Text = Catalog.GetString("Cou&ntry:");
            this.lblLocationKey.Text = Catalog.GetString("Location &Key:");
            this.lblCity.Text = Catalog.GetString("Ci&ty:");
            this.lblStreet2.Text = Catalog.GetString("Address &2:");
            this.lblAddr3.Text = Catalog.GetString("Address &3:");
            this.lblAddr1.Text = Catalog.GetString("Address &1:");
            this.btnSearch.Text = Catalog.GetString(" &Search");
            this.grpResult.Text = Catalog.GetString("Find R&esult");
            this.Text = Catalog.GetString("Location Find");
            #endregion

            grdResult.SendToBack();
        }

        public TLocationPK SelectedLocation
        {
            get
            {
                DataRowView[] miRows;
                Int64 miSiteKey;
                Int64 miLocationKey;
                TLocationPK miLocationPK;
                miRows = grdResult.SelectedDataRowsAsDataRowView;

                if (miRows.Length <= 0)
                {
                    // no Row is selected
                    return null;
                }

                miSiteKey = Convert.ToInt64(miRows[0]["p_site_key_n"]);
                miLocationKey = Convert.ToInt64(miRows[0]["p_location_key_i"]);
                miLocationPK = new TLocationPK(miSiteKey, (Int32)miLocationKey);
                return miLocationPK;
            }
        }

        private void InitialiseCriteria()
        {
            DataRow miRow;

            miRow = FFindCriteriaDataTable.NewRow();
            miRow["Addr1"] = "";
            miRow["Addr1Match"] = "BEGINS";
            miRow["Street2"] = "";
            miRow["Street2Match"] = "BEGINS";
            miRow["Addr3"] = "";
            miRow["Addr3Match"] = "BEGINS";
            miRow["City"] = "";
            miRow["CityMatch"] = "BEGINS";
            miRow["PostCode"] = "";
            miRow["PostCodeMatch"] = "BEGINS";
            miRow["County"] = "";
            miRow["CountyMatch"] = "BEGINS";
            miRow["LocationKey"] = "";
            miRow["Country"] = "";
            FFindCriteriaDataTable.Rows.Add(miRow);
            FDefaultCriteriaXML = FFindCriteriaDataTable.DataSet.GetXml();

            AssociateCriteriaButtons();
        }

        public void AssociateCriteriaButtons()
        {
            critAddress1.AssociatedTextBox = txtAddress1;
            critAddress2.AssociatedTextBox = txtAddress2;
            critAddress3.AssociatedTextBox = txtAddress3;
            critPostCode.AssociatedTextBox = txtPostCode;
            critCity.AssociatedTextBox = txtCity;
            critCounty.AssociatedTextBox = txtCounty;

            critAddress1.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critAddress2.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critAddress3.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critPostCode.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critCity.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
            critCounty.RemoveJokersFromTextBox += new TRemoveJokersFromTextBox(this.@RemoveJokersFromTextBox);
        }

        private void InitialiseGrid()
        {
            grdResult.Columns.Clear();
            grdResult.AddTextColumn("City", FPagedDataTable.Columns["p_city_c"]);

            // 80
            grdResult.AddTextColumn("Post Code", FPagedDataTable.Columns["p_postal_code_c"]);

            // 60
            grdResult.AddTextColumn("Addr1", FPagedDataTable.Columns["p_locality_c"]);

            // 120
            grdResult.AddTextColumn("Street-2", FPagedDataTable.Columns["p_street_name_c"]);

            // 120
            grdResult.AddTextColumn("Addr3", FPagedDataTable.Columns["p_address_3_c"]);

            // 80
            grdResult.AddTextColumn("County", FPagedDataTable.Columns["p_county_c"]);

            // 80
            grdResult.AddTextColumn("Country", FPagedDataTable.Columns["p_country_code_c"]);

            // 30
            grdResult.AddTextColumn("Location Key", FPagedDataTable.Columns["p_location_key_i"]);

            // 70
            grdResult.AddTextColumn("SiteKey", FPagedDataTable.Columns["p_site_key_n"]);

            // 0
            grdResult.FixedColumns = 2;
        }

        public DataTable GetDataPagedResult(Int16 ANeededPage, Int16 APageSize, out Int32 ATotalRecords, out Int16 ATotalPages)
        {
            ATotalRecords = 0;
            ATotalPages = 0;

            if (FPartnerLocationFindObject != null)
            {
                return FPartnerLocationFindObject.GetDataPagedResult(ANeededPage, APageSize, out ATotalRecords, out ATotalPages);
            }
            else
            {
                return null;
            }
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
            // when any key pressed in LocationKey
            // see how long the text is
            // if longer than0, disable other controls
            if (txtLocationKey.Text.Length == 0)
            {
                EnableDisableAllCriteria(true);
            }

            if (txtLocationKey.Text.Length > 0)
            {
                EnableDisableAllCriteria(false);
            }
        }

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

        private void GrdResults_DataPageLoading(System.Object Sender, TDataPageLoadEventArgs e)
        {
            // MessageBox.Show('DataPageLoading:  Page: ' + e.DataPage.ToString);
            if (e.DataPage > 0)
            {
                this.Cursor = Cursors.WaitCursor;
                stbMain.Panels[stbMain.Panels.IndexOf(stpInfo)].Text = Resourcestrings.StrTransferringDataForPageText + e.DataPage.ToString() + ')';
            }
        }

        private void GrdResults_DataPageLoaded(System.Object Sender, TDataPageLoadEventArgs e)
        {
            // MessageBox.Show('DataPageLoaded:  Page: ' + e.DataPage.ToString);
            if (e.DataPage > 0)
            {
                this.Cursor = Cursors.Default;
                stbMain.Panels[stbMain.Panels.IndexOf(stpInfo)].Text = Resourcestrings.StrResultGridHelpText + StrSearchTargetText;
            }
        }

        private void GrdResults_DoubleClickCell(System.Object Sender, SourceGrid.CellContextEventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void TPartnerLocationFind_Activated(System.Object sender, System.EventArgs e)
        {
            txtAddress1.Focus();
        }

        private void TPartnerLocationFind_Load(System.Object sender, System.EventArgs e)
        {
            string LocalisedCountyLabel;
            string dummy;

            FFindCriteriaDataTable.Clear();
            cmbCountry.PerformDataBinding(FFindCriteriaDataTable, "Country");
            InitialiseCriteria();

            LocalisedStrings.GetLocStrCounty(out LocalisedCountyLabel, out dummy);
            lblCounty.Text = LocalisedCountyLabel;

            // set information message colours
            lblSearchInfo.BackColor = Color.White;
            lblSearchInfo.ForeColor = Color.Blue;
            sbtForm.InstanceStatusBar = this.stbMain;
TODO: no resource strings
            sbtForm.SetStatusBarText(txtAddress1, Resourcestrings.StrAddress1Helptext);
            sbtForm.SetStatusBarText(txtAddress2, Resourcestrings.StrAddress2Helptext);
            sbtForm.SetStatusBarText(txtAddress3, Resourcestrings.StrAddress3Helptext);
            sbtForm.SetStatusBarText(txtCity, Resourcestrings.StrCityHelptext);
            sbtForm.SetStatusBarText(txtCounty, Resourcestrings.StrCountyHelpText);
            sbtForm.SetStatusBarText(cmbCountry, Resourcestrings.StrCountryHelpText);
            sbtForm.SetStatusBarText(txtPostCode, Resourcestrings.StrPostCodeHelpText);
            sbtForm.SetStatusBarText(txtLocationKey, Resourcestrings.StrLocationKeyHelpText);
            sbtForm.SetStatusBarText(btnOK, Resourcestrings.StrAcceptButtonHelpText + StrSearchTargetText);
            sbtForm.SetStatusBarText(btnCancel, Resourcestrings.StrCancelButtonHelpText + StrSearchTargetText);
            sbtForm.SetStatusBarText(btnClearCriteria, Resourcestrings.StrClearCriteriaButtonHelpText);
            sbtForm.SetStatusBarText(btnSearch, Resourcestrings.StrSearchButtonHelpText);
            sbtForm.SetStatusBarText(grdResult, Resourcestrings.StrResultGridHelpText + StrSearchTargetText);
        }

        /// <summary>
        /// This starts the ball rolling for a search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Click(System.Object sender, System.EventArgs e)
        {
            ReleaseServerObject();
            btnSearch.Focus();

            if (FFindCriteriaDataTable.DataSet.GetXml() == FDefaultCriteriaXML)
            {
                // this will find EVERYTHING!
                // don't allow this
                MessageBox.Show(Resourcestrings.StrNoCriteriaSpecified);
                return;
            }

            // MessageBox.Show(dtCriteria.DataSet.GetXml.ToString(),'dtCriteria');
            Application.DoEvents();
            lblSearchInfo.Visible = true;
            lblSearchInfo.Text = Resourcestrings.StrSearching;
            grdResult.SendToBack();
            grpResult.Text = Resourcestrings.StrSearchResult;
            try
            {
                FPagedDataTable = null;
            }
            catch (Exception)
            {
            }

            // don't do anything since this happens if the DataTable has no data yet
            Application.DoEvents();
            FPartnerLocationFindObject = TRemote.MPartner.Partner.UIConnectors.PartnerLocationFind(FFindCriteriaDataTable);

            // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
            TEnsureKeepAlive.Register(FPartnerLocationFindObject);
            timerSearchResults.Enabled = true;
        }

        /// <summary>
        /// reset all criteria fields to default values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearCriteria_Click(System.Object sender, System.EventArgs e)
        {
            EnableDisableAllCriteria(true);
            FFindCriteriaDataTable.Clear();
            InitialiseCriteria();

            lblSearchInfo.Text = "";
            grdResult.SendToBack();

            txtAddress1.Focus();

            btnOK.Enabled = false;

            grdResult.DataSource = null;
            FPagedDataTable = null;
        }

        /// <summary>
        /// This polls the server until the search has finished,
        /// </summary>
        /// <returns>void</returns>
        private void TimerSearchResults_Tick(System.Object sender, System.EventArgs e)
        {
            String SearchTarget;

            // check regularly for this to say we are finished
            if (FPartnerLocationFindObject.AsyncExecProgress.ProgressState == TAsyncExecProgressState.Aeps_Finished)
            {
                // we are finished:
                // prevent further calls
                timerSearchResults.Enabled = false;
                FPagedDataTable = grdResult.LoadFirstDataPage(@GetDataPagedResult);

                if (FPagedDataTable.Rows.Count == 0)
                {
                    // no results, inform user, then no further action
                    lblSearchInfo.Text = Resourcestrings.StrNoRecordsFound1Text + ' ' + StrSearchTargetText + Resourcestrings.StrNoRecordsFound2Text;
                    lblSearchInfo.BringToFront();
                    Application.DoEvents();
                }
                else
                {
                    // hide message
                    Application.DoEvents();

                    // setup grid
                    InitialiseGrid();
                    FDataView = FPagedDataTable.DefaultView;
                    FDataView.AllowNew = false;
                    grdResult.DataSource = new DevAge.ComponentModel.BoundDataView(FDataView);
                    grdResult.AutoSizeCells();
                    grdResult.Visible = true;

                    grdResult.BringToFront();

                    // Highlight first Row
                    grdResult.Selection.SelectRow(1, true);

                    // Make the Grid respond on updown keys
                    grdResult.Focus();

                    // Display the number of found Partners/Locations
                    if (grdResult.TotalRecords > 1)
                    {
                        SearchTarget = StrSearchTargetPluralText;
                    }
                    else
                    {
                        SearchTarget = StrSearchTargetText;
                    }

                    grpResult.Text = Resourcestrings.StrSearchResult + ": " + grdResult.TotalRecords.ToString() + ' ' + SearchTarget + ' ' +
                                     Resourcestrings.StrFoundText;

                    btnOK.Enabled = true;
                }
            }
        }

        private void ReleaseServerObject()
        {
            if (FPartnerLocationFindObject != null)
            {
                // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
                TEnsureKeepAlive.UnRegister(FPartnerLocationFindObject);
                FPartnerLocationFindObject = null;
            }
        }

        #region Key Events
        private void GeneralKeyHandler(TextBox ATextBox, SplitButton ACriteriaControl, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                // Without this  databinding fails
                ACriteriaControl.ShowContextMenu();
            }
        }

        public void GeneralLeaveHandler(TextBox ATextBox, SplitButton ACriteriaControl)
        {
            TMatches NewMatchValue;
            string TextBoxText = ATextBox.Text;
            string CriteriaValue;

            //            TLogging.Log("GeneralLeaveHandler for " + ATextBox.Name + ". SplitButton: " + ACriteriaControl.Name);

            if (ATextBox.Text.Contains("*"))
            {
                if (ATextBox.Text.StartsWith("*")
                    && !(ATextBox.Text.EndsWith("*")))
                {
                    //                    TLogging.Log(ATextBox.Name + " starts with *");
                    NewMatchValue = TMatches.ENDS;
                }
                else if (ATextBox.Text.EndsWith("*")
                         && !(ATextBox.Text.StartsWith("*")))
                {
                    //                    TLogging.Log(ATextBox.Name + " ends with *");
                    NewMatchValue = TMatches.BEGINS;
                }
                else
                {
                    //                    TLogging.Log(ATextBox.Name + " contains *");
                    NewMatchValue = TMatches.CONTAINS;
                }

                // See what the Criteria Value would be without any 'joker' characters ( * )
                CriteriaValue = TextBoxText.Replace("*", String.Empty);

                if (CriteriaValue != String.Empty)
                {
                    // There is still a valid CriteriaValue

                    FFindCriteriaDataTable.Rows[0].BeginEdit();
                    ACriteriaControl.SelectedValue = Enum.GetName(typeof(TMatches), NewMatchValue);
                    FFindCriteriaDataTable.Rows[0].EndEdit();

                    //TODO: It seems databinding is broken on this control
                    // this needs to happen in the SplitButton control really
                    string fieldname = ((SplitButton)ACriteriaControl).DataBindings[0].BindingMemberInfo.BindingMember;
                    FFindCriteriaDataTable.Rows[0][fieldname] = Enum.GetName(typeof(TMatches), NewMatchValue);

                    //TODO: DataBinding is really doing strange things here; we have to
                    //assign the just entered Text again, otherwise it is lost!!!
                    ATextBox.Text = TextBoxText;
                }
                else
                {
                    // No valid Criteria Value, therefore empty the TextBox's Text.
                    ATextBox.Text = String.Empty;
                }
            }
        }

        private void RemoveJokersFromTextBox(SplitButton ASplitButton,
            TextBox AAssociatedTextBox,
            TMatches ALastSelection)
        {
            string NewText;

            try
            {
                if (AAssociatedTextBox != null)
                {
                    NewText = AAssociatedTextBox.Text.Replace("*", String.Empty);

//                    TLogging.Log(
//                        "RemoveJokersFromTextBox:  Associated TextBox's (" + AAssociatedTextBox.Name + ") Text (1): " + AAssociatedTextBox.Text);

//                    FFindCriteriaDataTable.Rows[0].BeginEdit();
//                    AAssociatedTextBox.Text = NewText;
//                    FFindCriteriaDataTable.Rows[0].EndEdit();

                    string fieldname = ((TextBox)AAssociatedTextBox).DataBindings[0].BindingMemberInfo.BindingMember;
                    FFindCriteriaDataTable.Rows[0][fieldname] = NewText;
                    fieldname = ((SplitButton)ASplitButton).DataBindings[0].BindingMemberInfo.BindingMember;
                    FFindCriteriaDataTable.Rows[0][fieldname] = Enum.GetName(typeof(TMatches), ALastSelection);
                    FFindCriteriaDataTable.Rows[0].EndEdit();

//
//                    AAssociatedTextBox.Text = NewText;
//
//                    TLogging.Log(
//                        "RemoveJokersFromTextBox:  Associated TextBox's (" + AAssociatedTextBox.Name + ") Text (2): " + AAssociatedTextBox.Text);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception in RemoveJokersFromTextBox: " + exp.ToString());
            }
        }

        private void TxtAddress1_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtAddress1, critAddress1, e);
        }

        private void TxtAddress1_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtAddress1, critAddress1);
        }

        private void TxtAddress2_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtAddress2, critAddress2, e);
        }

        private void TxtAddress2_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtAddress2, critAddress2);
        }

        private void TxtAddress3_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtAddress3, critAddress3, e);
        }

        private void TxtAddress3_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtAddress3, critAddress3);
        }

        private void TxtCounty_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtCounty, critCounty, e);
        }

        private void TxtCounty_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtCounty, critCounty);
        }

        private void TxtPostCode_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtPostCode, critPostCode, e);
        }

        private void TxtPostCode_Leave(System.Object sender, EventArgs e)
        {
            // capitalise when leaving control
            txtPostCode.Text = txtPostCode.Text.ToUpper();

            GeneralLeaveHandler(txtPostCode, critPostCode);
        }

        private void TxtCity_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            GeneralKeyHandler(txtCity, critCity, e);
        }

        private void TxtCity_Leave(System.Object sender, EventArgs e)
        {
            GeneralLeaveHandler(txtCity, critCity);
        }

        #endregion
    }
}