// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2015 by OM International
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

using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using SourceGrid;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Ict.Petra.Client.MFinance.Gui.Setup.Gift
{
    public partial class TFrmEsrDefaultsSetup
    {
        private DataTable FesrDefaults;
        DataRow FselectedRow = null;
        Int32 FLedgerNumber = 0;
        Int32 FsuppressChangeEvent = 0;

        private void InitializeManualCode()
        {
            FesrDefaults = TRemote.MFinance.Gift.WebConnectors.GetEsrDefaults();
            grdDetails.Columns.Add("a_partner_key_n", "ESR Key", typeof(Int64));
            grdDetails.Columns.Add("a_new_partner_key_n", "Substitute", typeof(Int64));
            grdDetails.Columns.Add("a_motiv_group_s", "Motiv. Group", typeof(String));
            grdDetails.Columns.Add("a_motiv_detail_s", "Motiv. Detail", typeof(String));

            grdDetails.Selection.SelectionChanged += Selection_SelectionChanged;
            grdDetails.Selection.FocusRowLeaving += UpdateGrid;
            txtPartnerKey.Leave += OnLeavePartnerKey;
            txtNewPartnerKey.Leave += UpdateGrid;
            cmbMotivGroup.SelectedValueChanged += UpdateMotivationDetail;
            cmbMotivDetail.SelectedValueChanged += UpdateGrid;

            FesrDefaults.DefaultView.Sort = "a_partner_key_n";
            FesrDefaults.DefaultView.AllowNew = false;
            FesrDefaults.DefaultView.AllowEdit = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FesrDefaults.DefaultView);
        }

        void UpdateGrid(object sender, EventArgs e)
        {
            if (FsuppressChangeEvent == 0)
            {
                GetDataFromControlsManual();
            }
        }

        void OnLeavePartnerKey(object sender, EventArgs e)
        {
            if (FsuppressChangeEvent > 0)
            {
                return;
            }

            Control PrevFocus = FPetraUtilsObject.GetFocusedControl(this);

            if (PrevFocus == null)
            {
                PrevFocus = txtNewPartnerKey;
            }

            String PartnerKeySt = txtPartnerKey.Text;

            if (PartnerKeySt == "")
            {
/*
 *              MessageBox.Show("Error: Please enter a PartnerKey.",
 *                  "ESR Defaults", MessageBoxButtons.OK, MessageBoxIcon.Error);
 *              txtPartnerKey.Text = FselectedRow["a_partner_key_n"].ToString();
 *              txtPartnerKey.Focus();
 */
                return;
            }

            Int64 PartnerKey;
            Boolean IsNumeric = Int64.TryParse(PartnerKeySt, out PartnerKey);

            if (!IsNumeric)
            {
                MessageBox.Show("Error: Please check your entry for PartnerKey.",
                    "ESR Defaults", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPartnerKey.Text = FselectedRow["a_partner_key_n"].ToString();
                txtPartnerKey.Focus();
                return;
            }

            //
            // If the user didn't change the value,
            // there's nothing more to do here.

            if ((FselectedRow["a_new_partner_key_n"] != System.DBNull.Value)
                && (Convert.ToInt64(FselectedRow["a_partner_key_n"]) == PartnerKey))
            {
                return;
            }

            FesrDefaults.DefaultView.Sort = "a_partner_key_n";  // I don't know why I need to do this, since it was done previously in
                                                                // InitializeManualCode. But without it, I'm seeing exceptions here.

            if (FesrDefaults.DefaultView.Find(PartnerKey) > 0)
            {
                MessageBox.Show(String.Format("Error: An entry already exists for partner key {0}.", PartnerKeySt),
                    "ESR Defaults", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPartnerKey.Text = FselectedRow["a_partner_key_n"].ToString();
                txtPartnerKey.Focus();

                return;
            }

            if (txtNewPartnerKey.Text == "0000000000")
            {
                txtNewPartnerKey.Text = txtPartnerKey.Text;
            }

            UpdateGrid(sender, e);

            FsuppressChangeEvent++;
            // Since the Primary Key could have changed,
            // I need to re-select the correct row in the grid:
            Int32 NewRowPos = FesrDefaults.DefaultView.Find(PartnerKey);
            grdDetails.SelectRowInGrid(1 + NewRowPos, true); // This ends up calling back here!
            PrevFocus.Focus();
            FsuppressChangeEvent--;
        }

        void UpdateMotivationDetail(object sender, EventArgs e)
        {
            String motivationGroup = cmbMotivGroup.GetSelectedString();

            TFinanceControls.ChangeFilterMotivationDetailList(ref cmbMotivDetail, motivationGroup);

            if (FsuppressChangeEvent == 0)
            {
                UpdateGrid(sender, e);
            }
        }

        /// <summary>
        /// Only show motivation details for this Ledger:
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                FsuppressChangeEvent++;
                TFinanceControls.InitialiseMotivationGroupList(ref cmbMotivGroup, FLedgerNumber, true);
                TFinanceControls.InitialiseMotivationDetailList(ref cmbMotivDetail, FLedgerNumber, true);
                FsuppressChangeEvent--;

                grdDetails.SelectRowInGrid(1, true);
            }
        }

        private void Selection_SelectionChanged(object sender, RangeRegionChangedEventArgs e)
        {
            int gridRow = grdDetails.Selection.ActivePosition.Row;

            ShowGridRow(gridRow);
        }

        private void GetDataFromControlsManual()
        {
            if (FselectedRow != null)
            {
                Int64 partnerKey;

                if (Int64.TryParse(txtPartnerKey.Text, out partnerKey))
                {
                    if (FselectedRow["a_partner_key_n"].ToString() != txtPartnerKey.Text)
                    {
                        FselectedRow["a_partner_key_n"] = partnerKey;
                        FPetraUtilsObject.SetChangedFlag();
                    }

                    Int64 newPartnerKey = partnerKey;

                    if (Int64.TryParse(txtNewPartnerKey.Text, out newPartnerKey))
                    {
                        if ((FselectedRow["a_new_partner_key_n"] == System.DBNull.Value)
                            || (Convert.ToInt64(FselectedRow["a_new_partner_key_n"]) != newPartnerKey))
                        {
                            FselectedRow["a_new_partner_key_n"] = newPartnerKey;
                            FPetraUtilsObject.SetChangedFlag();
                        }
                    }

                    String SelString = cmbMotivGroup.GetSelectedString();

                    if (FselectedRow["a_motiv_group_s"].ToString() != SelString)
                    {
                        FselectedRow["a_motiv_group_s"] = SelString;
                        FPetraUtilsObject.SetChangedFlag();
                    }

                    SelString = cmbMotivDetail.GetSelectedString();

                    if (FselectedRow["a_motiv_detail_s"].ToString() != SelString)
                    {
                        FselectedRow["a_motiv_detail_s"] = SelString;
                        FPetraUtilsObject.SetChangedFlag();
                    }
                }
                else
                {
                    MessageBox.Show("Error: PartnerKey is empty. Please delete unused row.",
                        "ESR Defaults", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPartnerKey.Text = FselectedRow["a_partner_key_n"].ToString();
                }
            }
        }

        private void ShowGridRow(Int32 ARowNumberInGrid)
        {
            DataRowView rowView = (DataRowView)grdDetails.Rows.IndexToDataSourceRow(ARowNumberInGrid);

            if (rowView != null)
            {
                FselectedRow = rowView.Row;
                ShowDataRow(FselectedRow);
            }
        }

        private void ShowDataRow(DataRow Row)
        {
            FPetraUtilsObject.SuppressChangeDetection = true;
            FsuppressChangeEvent++;
            txtPartnerKey.Text = Row["a_partner_key_n"].ToString();
            txtNewPartnerKey.Text = Row["a_new_partner_key_n"].ToString();
            cmbMotivGroup.SetSelectedString(Row["a_motiv_group_s"].ToString());
            cmbMotivDetail.SetSelectedString(Row["a_motiv_detail_s"].ToString());
            FsuppressChangeEvent--;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            GetDataFromControls();
            DataRow newRow = FesrDefaults.NewRow();
            newRow["a_motiv_group_s"] = "GIFT";
            FesrDefaults.Rows.Add(newRow);
            grdDetails.SelectRowInGrid(1, true);
            txtPartnerKey.Focus();

            FPetraUtilsObject.SetChangedFlag();
        }

        private void DeleteRecord(Object sender, EventArgs e)
        {
            int gridRow = grdDetails.Selection.ActivePosition.Row;

            FselectedRow.Delete();
            grdDetails.SelectRowInGrid(gridRow, true);
            FPetraUtilsObject.SetChangedFlag();
        }

        private void FileSave(Object sender, EventArgs e)
        {
            SaveChanges();
        }

        /// <summary>
        /// Called from PetraEditForm
        /// </summary>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public Int32 GetChangedRecordCount(out String Msg)
        {
            Msg = "";
            DataTable changes = FesrDefaults.GetChanges();

            return (changes == null) ? 0 : changes.Rows.Count;
        }

        /// <summary>
        /// Called from PetraEditForm
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            GetDataFromControlsManual();
            Boolean Res = TRemote.MFinance.Gift.WebConnectors.CommitEsrDefaults(FesrDefaults);

            if (Res)
            {
                FPetraUtilsObject.DisableSaveButton();
            }

            return Res;
        }
    }
}