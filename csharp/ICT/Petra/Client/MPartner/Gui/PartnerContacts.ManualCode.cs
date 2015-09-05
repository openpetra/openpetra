//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, peters
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
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Printing;
using GNU.Gettext;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TFrmPartnerContacts
    {
        DataTable FResultsTable = new DataTable();
        PPartnerContactAttributeTable FAttributeTable = new PPartnerContactAttributeTable();

        private void InitializeManualCode()
        {
            // set to blank initially
            lblRecordCounter.Text = "";

            // add divider line (can't currently do this in YAML)
            DevAge.Windows.Forms.Line linCriteriaDivider = new DevAge.Windows.Forms.Line();
            linCriteriaDivider.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            linCriteriaDivider.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            linCriteriaDivider.FirstColor = System.Drawing.SystemColors.ControlDark;
            linCriteriaDivider.LineStyle = DevAge.Windows.Forms.LineStyle.Horizontal;
            linCriteriaDivider.Location = new System.Drawing.Point(grpFindCriteria.Location.Y + 6, btnSearch.Location.Y - 2);
            linCriteriaDivider.Name = "linCriteriaDivider";
            linCriteriaDivider.SecondColor = System.Drawing.SystemColors.ControlLightLight;
            linCriteriaDivider.Size = new System.Drawing.Size(grpFindCriteria.Size.Width - 12, 2);
            grpFindCriteria.Controls.Add(linCriteriaDivider);

            // set button to be on the very right of the screen (can't make this work in YAML)
            btnClear.Location = new System.Drawing.Point(linCriteriaDivider.Location.X + linCriteriaDivider.Width - btnClear.Width,
                btnClear.Location.Y);

            // fix tab index order (also can't make this work in YAML!)
            btnSelectAttributes.TabIndex = cmbMailingCode.TabIndex + 1;
            grdSelectedAttributes.TabIndex = btnSelectAttributes.TabIndex + 1;

            // initially focus on this control
            this.ActiveControl = dtpContactDate;

            // catch enter on all controls, to trigger search or accept (could use this.AcceptButton, but we have several search buttons etc)
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CatchEnterKey);

            FinishButtonPanelSetup();
        }

        private void Search(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            FResultsTable.Clear();

            FResultsTable = TRemote.MPartner.Partner.WebConnectors.FindContacts(
                txtContactor.Text,
                dtpContactDate.Date,
                txtCommentContains.Text,
                cmbContactCode.Text,
                "",
                cmbMailingCode.Text,
                FAttributeTable);

            // if there are results to show for the first time only
            if ((grdDetails.Columns.Count == 0) && (FResultsTable.Rows.Count > 0))
            {
                grdDetails.Columns.Clear();
                grdDetails.AddTextColumn(Catalog.GetString("ID"), FResultsTable.Columns["p_contact_log_id_i"]);
                grdDetails.AddPartnerKeyColumn(Catalog.GetString("Partner Key"), FResultsTable.Columns["p_partner_key_n"]);
                grdDetails.AddTextColumn(Catalog.GetString("Partner Name"), FResultsTable.Columns["p_partner_short_name_c"]);
                grdDetails.AddDateColumn(Catalog.GetString("Contact Date"), FResultsTable.Columns["s_contact_date_d"]);
                grdDetails.AddTextColumn(Catalog.GetString("Contact Code"), FResultsTable.Columns["p_contact_code_c"]);
                grdDetails.AddTextColumn(Catalog.GetString("Contactor"), FResultsTable.Columns["p_contactor_c"]);
                grdDetails.AddTextColumn(Catalog.GetString("Mailing Code"), FResultsTable.Columns["p_mailing_code_c"]);
                grdDetails.AddTextColumn(Catalog.GetString("Description"), FResultsTable.Columns["p_contact_comment_c"]);
                //grdDetails.AddTextColumn(Catalog.GetString("Module ID"), FResultsTable.Columns["s_module_id_c"]);

                grdDetails.Selection.EnableMultiSelection = true;

                // catch enter on grid to view the selected transaction
                grdDetails.EnterKeyPressed += new Ict.Common.Controls.TKeyPressedEventHandler(this.ViewPartner);
                grdDetails.DoubleClickCell += new Ict.Common.Controls.TDoubleClickCellEventHandler(this.GrdDetail_DoubleClickCell);
            }

            if (FResultsTable.Rows.Count > 0)
            {
                DataView myDataView = FResultsTable.DefaultView;
                myDataView.Sort = FResultsTable.Columns["p_contact_log_id_i"].ColumnName + ", " + FResultsTable.Columns["p_partner_key_n"].ColumnName;
                myDataView.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

                grdDetails.AutoResizeGrid();

                grdDetails.SelectRowWithoutFocus(1);
            }

            UpdateRecordNumberDisplay();

            this.Cursor = Cursors.Default;
        }

        private void Clear(object sender, EventArgs e)
        {
            dtpContactDate.Clear();
            txtContactor.Clear();
            txtCommentContains.Clear();
            cmbContactCode.Clear();
            cmbMailingCode.Clear();

            FAttributeTable.Clear();
            grdSelectedAttributes.DataSource = null;
            FResultsTable.Clear();

            UpdateRecordNumberDisplay();
        }

        // display the partner edit screen, open to the Contact Log tab and display the contact log that has been selected in the grid
        private void ViewPartner(object Sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            // get data for contact log to display
            DataRow CurrentRow = GetCurrentDataRow();
            Int64 PartnerKey = Convert.ToInt64(CurrentRow["p_partner_key_n"]);
            string ContactLogID = CurrentRow["p_contact_log_id_i"].ToString();

            TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

            frm.SetParameters(TScreenMode.smEdit, PartnerKey, TPartnerEditTabPageEnum.petpContacts);
            frm.Show();
            frm.SelectContactLog(ContactLogID);

            this.Cursor = Cursors.Default;
        }

        // delete all contact logs selected by the user
        private void DeleteSelectedContacts(object Sender, EventArgs e)
        {
            DataRowView[] DataRowView = grdDetails.SelectedDataRowsAsDataRowView;

            string Msg = String.Format(Catalog.GetPluralString(
                    "Do you really want to delete this contact log?",
                    "Do you really want to delete all {0} selected contact logs?",
                    DataRowView.Length), DataRowView.Length);

            if (MessageBox.Show(Msg,
                    Catalog.GetString("Confirm deletion"),
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                DataTable DeleteTable = FResultsTable.Clone();

                foreach (DataRowView RowView in DataRowView)
                {
                    DataRow Row = FResultsTable.Rows.Find(new object[] { RowView["p_partner_key_n"], RowView["p_contact_log_id_i"] });
                    DeleteTable.Rows.Add((object[])Row.ItemArray.Clone());
                    Row.Delete();
                }

                // delete all ppartnercontact rows in the table
                TRemote.MPartner.Partner.WebConnectors.DeleteContacts(DeleteTable);
            }
        }

        // delete all contact logs return in the search
        private void DeleteAllContacts(object Sender, EventArgs e)
        {
            string Msg = String.Format(Catalog.GetPluralString(
                    "Do you really want to delete this contact log?",
                    "Do you really want to delete all {0} contact logs?",
                    FResultsTable.Rows.Count), FResultsTable.Rows.Count);

            if (MessageBox.Show(Msg,
                    Catalog.GetString("Confirm deletion"),
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (MessageBox.Show(Catalog.GetString("Are you really sure? This cannot be undone!"),
                        Catalog.GetString("Confirm deletion"),
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    DataTable DeleteTable = FResultsTable.Copy();

                    foreach (DataRow Row in FResultsTable.Rows)
                    {
                        Row.Delete();
                    }

                    // delete all ppartnercontact rows in the table
                    TRemote.MPartner.Partner.WebConnectors.DeleteContacts(DeleteTable);
                }
            }
        }

        private void GrdDetail_DoubleClickCell(System.Object Sender, SourceGrid.CellContextEventArgs e)
        {
            ViewPartner(this, null);
        }

        /// <summary>
        /// Open a dialog to select Contact Attributes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectAttributes(object sender, EventArgs e)
        {
            // open the contact attributes dialog
            TFrmContactAttributesDialog ContactAttributesDialog = new TFrmContactAttributesDialog(FPetraUtilsObject.GetForm());

            ContactAttributesDialog.ContactID = -1;
            ContactAttributesDialog.SelectedContactAttributeTable = FAttributeTable;

            if (ContactAttributesDialog.ShowDialog() == DialogResult.OK)
            {
                PPartnerContactAttributeTable Changes = ContactAttributesDialog.SelectedContactAttributeTable.GetChangesTyped();

                // if changes were made
                if (Changes != null)
                {
                    FAttributeTable = ContactAttributesDialog.SelectedContactAttributeTable;

                    // we do not need the deleted rows
                    FAttributeTable.AcceptChanges();

                    ContactAttributesLogic.SetupContactAttributesGrid(ref grdSelectedAttributes, FAttributeTable, false);
                }
            }
        }

        private void CatchEnterKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!ComboboxDroppedDown())
                {
                    Search(sender, e);
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = false;
            }
        }

        // These are used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down.
        private bool ContactCodeDroppedDown = false;
        private bool MailingCodeDroppedDown = false;

        private void OnCmomboChange(object sender, EventArgs e)
        {
            // if the list is dropped down while the value is changed (not the case when a value from the list is clicked on)
            if (cmbContactCode.cmbCombobox.DroppedDown)
            {
                // this is used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down
                ContactCodeDroppedDown = true;
            }

            if (cmbMailingCode.cmbCombobox.DroppedDown)
            {
                // this is used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down
                MailingCodeDroppedDown = true;
            }
        }

        /// <summary>
        /// Returns the currently selected row (or the first selected row if multiple rows are selected)
        /// </summary>
        private DataRow GetCurrentDataRow()
        {
            if (grdDetails != null)
            {
                DataRowView[] TheDataRowViewArray = grdDetails.SelectedDataRowsAsDataRowView;

                if (TheDataRowViewArray.Length > 0)
                {
                    return TheDataRowViewArray[0].Row;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void FinishButtonPanelSetup()
        {
            // Further set up certain Controls Properties that can't be set directly in the WinForms Generator...
            lblRecordCounter.AutoSize = true;
            lblRecordCounter.Padding = new Padding(4, 3, 0, 0);
            lblRecordCounter.ForeColor = System.Drawing.Color.SlateGray;

            pnlButtonsRecordCounter.AutoSize = true;
        }

        // update record counter
        private void UpdateRecordNumberDisplay()
        {
            int RecordCount;

            if (grdDetails.DataSource != null)
            {
                RecordCount = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).Count;
            }
            else
            {
                RecordCount = 0;
            }

            lblRecordCounter.Text = String.Format(Catalog.GetPluralString("{0} record", "{0} records", RecordCount, true), RecordCount);

            // only enabled if there are results
            btnView.Enabled = RecordCount > 0;
            btnDelete.Enabled = RecordCount > 0;
            btnDeleteAll.Enabled = RecordCount > 0;
        }

        /// <summary>
        /// Determines if a combo box's value has been changed while the list is dropped down
        /// and that that combo box still contains the focus.
        /// </summary>
        /// <returns></returns>
        public bool ComboboxDroppedDown()
        {
            if (ContactCodeDroppedDown && cmbContactCode.ContainsFocus)
            {
                ContactCodeDroppedDown = false;
                return true;
            }
            else if (MailingCodeDroppedDown && cmbMailingCode.ContainsFocus)
            {
                MailingCodeDroppedDown = false;
                return true;
            }

            ContactCodeDroppedDown = false;
            MailingCodeDroppedDown = false;

            return false;
        }
    }
}