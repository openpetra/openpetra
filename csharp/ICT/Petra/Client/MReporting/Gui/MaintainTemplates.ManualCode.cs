// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
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
using Ict.Petra.Shared.MSysMan.Data;
using System.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using System.Windows.Forms;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.Data;

namespace Ict.Petra.Client.MReporting.Gui
{
    public partial class TFrmMaintainTemplates : IFrmPetra
    {
        SReportTemplateTable FTemplateTable = null;
        SReportTemplateRow FSelectedRow = null;
        private TFrmPetraReportingUtils FPetraUtilsObject;
        private String FCurrentUser;
        private Boolean FInChangeEvent = false;
        private Boolean DataChangedFlag = false;
        private Int32 FNewTemplateId = -1;

        private void InitManualCode()
        {
            FCurrentUser = UserInfo.GUserInfo.UserID;
            FPetraUtilsObject = new TFrmPetraReportingUtils(this, this, null);
            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(Form_KeyDown);
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            FPetraUtilsObject.Form_KeyDown(sender, e);
        }

        private bool SaveChanges()
        {
            if (DataChangedFlag)
            {
                SReportTemplateTable Tbl = TRemote.MReporting.WebConnectors.SaveTemplates(FTemplateTable);

                if (Tbl != null)
                {
                    FTemplateTable.Merge(Tbl);
                }

                DataChangedFlag = false;
            }

            return true;
        }

        private void SetControlsVisible()
        {
            btnSelect.Enabled = (FSelectedRow != null);
            btnDuplicate.Enabled = (FSelectedRow != null);
            btnRemove.Enabled = (FSelectedRow != null);

            if (FSelectedRow == null)
            {
                return;
            }

            chkDefault.Enabled = !chkPrivate.Checked;
            txtDescription.Enabled = !chkReadonly.Checked;
            chkPrivate.Visible = (!chkDefault.Checked && FSelectedRow.Author == FCurrentUser);
            chkReadonly.Visible = (Control.ModifierKeys == Keys.Control);
            btnRemove.Enabled = !FSelectedRow.Readonly;

            lblPrivate.Visible = chkPrivate.Visible;
            lblReadonly.Visible = chkReadonly.Visible;

            chkPrivateDefault.Visible = (!chkDefault.Checked && FSelectedRow.Author == FCurrentUser);
            lblPrivateDefault.Visible = (!chkDefault.Checked && FSelectedRow.Author == FCurrentUser);
        }

        private void chkDefaultCheckedChanged(System.Object sender, System.Object e)
        {
            if (!FInChangeEvent)
            {
                if (chkDefault.Checked) // Is it OK to make this default?
                {
                    if (chkPrivate.Checked)  // not if it's private to me.
                    {
                        MessageBox.Show(Catalog.GetString("This Template is private, and cannot be made default."),
                            Catalog.GetString("Default Template"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        chkDefault.Checked = false;
                        return;
                    }

                    //
                    // If so, then I need to unset the current Default:
                    foreach (SReportTemplateRow Row in FTemplateTable.Rows)
                    {
                        if (Row.RowState != DataRowState.Deleted)
                        {
                            Row.Default = (Row == FSelectedRow);
                        }
                    }
                }
                else // The user wants to unset this but he can't.
                {    // Rather than just disabling the control, I'll allow him to try,
                     // and then explain why it doesn't work...
                    MessageBox.Show(Catalog.GetString(
                            "There must always be a default Template.\r\nDon't de-select this; set another template as default instead."),
                        Catalog.GetString("Default Template"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    chkDefault.Checked = true;
                    return;
                }

                GetDataFromControls();
                DataChangedFlag = true;
                SetControlsVisible();
            }
        }

        private void chkPrivateCheckedChanged(System.Object sender, System.Object e)
        {
            if (!FInChangeEvent)
            {
                //
                // I can only set private if this is my template:
                if (FSelectedRow.Author != FCurrentUser)
                {
                    chkPrivate.Checked = false;
                    chkPrivateDefault.Checked = false;
                }

                //
                // If it's not private, it can't be private default.
                if (!chkPrivate.Checked)
                {
                    chkPrivateDefault.Checked = false;
                }

                GetDataFromControls();
                DataChangedFlag = true;
                SetControlsVisible();
            }
        }

        private void chkPrivateDefaultCheckedChanged(System.Object sender, System.Object e)
        {
            if (!FInChangeEvent)
            {
                GetDataFromControls();
                DataChangedFlag = true;
            }
        }

        private void chkReadonlyCheckedChanged(System.Object sender, System.Object e)
        {
            if (!FInChangeEvent)
            {
                GetDataFromControls();
                DataChangedFlag = true;
            }
        }

        private void GetDataFromControls()
        {
            if (FSelectedRow != null)
            {
                try
                {
                    if (FSelectedRow.ReportVariant != txtDescription.Text)
                    {
                        DataChangedFlag = true;
                    }

                    FSelectedRow.ReportVariant = txtDescription.Text;   // This previously could fail, but now ReportVariant is not a Primary Key.
                }                                                       // the Try-Catch block is left here in case the previous style comes back!
                catch (Exception)
                {
                    MessageBox.Show(String.Format(Catalog.GetString("Cannot rename template.\r\nAnother template is called {0}."),
                            txtDescription.Text),
                        Catalog.GetString("Rename Template"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    chkDefault.Checked = true;
                }

                FSelectedRow.Default = chkDefault.Checked;
                FSelectedRow.Private = chkPrivate.Checked;
                FSelectedRow.PrivateDefault = chkPrivateDefault.Checked;
                FSelectedRow.Readonly = chkReadonly.Checked;
            }
        }

        private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            FInChangeEvent = true;
            DataRowView[] SelectedRows = grdTemplateList.SelectedDataRowsAsDataRowView;

            if (SelectedRows.Length > 0)
            {
                GetDataFromControls();
                FSelectedRow = (SReportTemplateRow)SelectedRows[0].Row;

                txtDescription.Text = FSelectedRow.ReportVariant;
                chkDefault.Checked = FSelectedRow.Default;
                chkPrivate.Checked = FSelectedRow.Private;
                chkPrivateDefault.Checked = FSelectedRow.PrivateDefault;
                chkReadonly.Checked = FSelectedRow.Readonly;
            }
            else
            {
                FSelectedRow = null;
            }

            SetControlsVisible();
            FInChangeEvent = false;
        }

        void IFrmPetra.RunOnceOnActivation()
        {
        }

        bool IFrmPetra.CanClose()
        {
            return true;
        }

        TFrmPetraUtils IFrmPetra.GetPetraUtilsObject()
        {
            return FPetraUtilsObject;
        }

        private void SelectRowByTemplateId(Int32 ASelectedTemplateId)
        {
            for (Int32 RowIdx = 0; RowIdx < FTemplateTable.DefaultView.Count; RowIdx++)
            {
                if (((SReportTemplateRow)FTemplateTable.DefaultView[RowIdx].Row).TemplateId == ASelectedTemplateId)
                {
                    grdTemplateList.SelectRowInGrid(RowIdx + 1);
                    break;
                }
            }
        }

        /// <summary>
        /// Call this to display the form and show the available templates.
        /// </summary>
        /// <param name="AReportType"></param>
        /// <param name="ASelectedTemplateId">Supply this to set the current row</param>
        public DialogResult SelectTemplate(String AReportType, Int32 ASelectedTemplateId)
        {
            FTemplateTable = TRemote.MReporting.WebConnectors.GetTemplateVariants(AReportType, FCurrentUser, false);
            DataView myDataView = FTemplateTable.DefaultView;
            myDataView.AllowNew = false;
            grdTemplateList.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            grdTemplateList.Columns.Clear();
            grdTemplateList.AddTextColumn(AReportType, FTemplateTable.ColumnReportVariant);
            SelectRowByTemplateId(ASelectedTemplateId);
            FocusedRowChanged(null, null);
            FNewTemplateId = -1;

            this.FormClosing += TFrmPetra_Closed; // Apparently I need to do this myself - it's not called automatically.
            return ShowDialog();
        }

        /// <summary>
        /// After the form is closed, DON'T THROW IT AWAY - call this to get result!
        /// </summary>
        /// <returns></returns>
        public SReportTemplateRow GetSelectedTemplate()
        {
            return FSelectedRow;
        }

        private void ExitManualCode()
        {
            if (DataChangedFlag)
            {
                if (
                    MessageBox.Show(
                        Catalog.GetString("Save changes before closing?"),
                        Catalog.GetString("Maintain Templates"),
                        MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    SaveChanges();
                }
            }
        }

        private void DeleteRecord(object sender, EventArgs e)
        {
            if (FSelectedRow.Default)
            {
                MessageBox.Show(Catalog.GetString(
                        "There must always be a default Template.\r\nSet another template as the default before deleting this."),
                    Catalog.GetString("Default Template"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            FSelectedRow.Delete();
            DataChangedFlag = true;
            FSelectedRow = null;
            FocusedRowChanged(null, null);
        }

        private void DuplicateRecord(object sender, EventArgs e)
        {
            GetDataFromControls();
            SReportTemplateRow NewRow = FTemplateTable.NewRowTyped();
            DataUtilities.CopyAllColumnValues(FSelectedRow, NewRow);
            NewRow.TemplateId = FNewTemplateId;
            NewRow.Author = FCurrentUser;
            NewRow.Default = false;
            NewRow.Readonly = false;
            NewRow.ReportVariant = String.Format(Catalog.GetString("{0} copy of {1}"), FCurrentUser, NewRow.ReportVariant);
            FTemplateTable.Rows.Add(NewRow);
            SelectRowByTemplateId(FNewTemplateId);
            DataChangedFlag = true;
            FNewTemplateId--;
        }

        private void ReturnSelected(object sender, EventArgs e)
        {
            GetDataFromControls();
            SaveChanges();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void ReturnCancel(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}