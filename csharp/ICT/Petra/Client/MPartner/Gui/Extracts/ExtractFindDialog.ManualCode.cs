//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2011 by OM International
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
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    /// manual methods for the generated window
    public partial class TFrmExtractFindDialog
    {
        private Boolean FAllowMultipleSelect = false;
        private String FCheckedColumnName = "CHECKED";

        private MExtractMasterTable FExtractMasterTable;
        private MExtractMasterTable FResultTable;
        private DataTable FDataTable;

        #region Public Methods

        /// get/set to allow multiple select (otherwise single select by default)
        public Boolean AllowMultipleSelect
        {
            get
            {
                return FAllowMultipleSelect;
            }

            set
            {
                FAllowMultipleSelect = value;
            }
        }

        /// <summary>
        /// show form as dialog with given parameters
        /// </summary>
        /// <returns>Boolean: true if Accept button was clicked</returns>
        public bool ShowDialog(bool ADummy)
        {
            // now show the actual dialog
            this.StartPosition = FormStartPosition.CenterScreen;

            cmbUserCreated.SetSelectedString(UserInfo.GUserInfo.UserID, -1);

            clbDetails.ValueChanged += new System.EventHandler(this.GridValueChanged);
            clbDetails.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.FocusedRowChanged);

            // only allow select and close with double click when dialog is single select
            if (!AllowMultipleSelect)
            {
                this.clbDetails.DoubleClick += new System.EventHandler(this.AcceptExtract);
            }

            // make sure search button is initially default button so user can press enter to search after entering search criteria
            AcceptButton = btnSearch;
            txtExtractName.Select();

            this.ShowDialog();

            return true;
        }

        private void InitializeManualCode()
        {
            // manually configure tab index
            pnlExtractMasterList.TabIndex = pnlFilterButtons.TabIndex + 1;
            pnlLeftButtons.TabIndex = pnlRightButtons.TabIndex + 1;
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the result of the dialog
        ///
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <param name="AExtractName"></param>
        /// <param name="AExtractDescription"></param>
        /// <returns>true if a row was selected
        /// </returns>
        public bool GetResult(out int AExtractId, out String AExtractName, out String AExtractDescription)
        {
            if ((FResultTable != null)
                && (FResultTable.Count > 0))
            {
                // use first row (this method should normally only be called for single select dialog)
                MExtractMasterRow Row = (MExtractMasterRow)FResultTable.Rows[0];
                AExtractId = Row.ExtractId;
                AExtractName = Row.ExtractName;
                AExtractDescription = Row.ExtractDesc;
            }
            else
            {
                // no selected rows -> initialize values
                AExtractId = -1;
                AExtractName = "";
                AExtractDescription = "";
            }

            return AExtractId >= 0;
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the result of the dialog
        ///
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <param name="AExtractName"></param>
        /// <param name="AExtractDescription"></param>
        /// <param name="AKeyCount"></param>
        /// <param name="AExtractCreatedBy"></param>
        /// <param name="AExtractDateCreated"></param>
        /// <returns>true if a row was selected
        /// </returns>
        public bool GetResult(out int AExtractId, out String AExtractName, out String AExtractDescription,
            out int AKeyCount, out String AExtractCreatedBy, out DateTime AExtractDateCreated)
        {
            if ((FResultTable != null)
                && (FResultTable.Count > 0))
            {
                // use first row (this method should normally only be called for single select dialog)
                MExtractMasterRow Row = (MExtractMasterRow)FResultTable.Rows[0];
                AExtractId = Row.ExtractId;
                AExtractName = Row.ExtractName;
                AExtractDescription = Row.ExtractDesc;
                AKeyCount = Row.KeyCount;
                AExtractCreatedBy = Row.CreatedBy;
                AExtractDateCreated = (DateTime)Row.DateCreated;
            }
            else
            {
                // no selected rows -> initialize values
                AExtractId = -1;
                AExtractName = "";
                AExtractDescription = "";
                AKeyCount = 0;
                AExtractCreatedBy = "";
                AExtractDateCreated = DateTime.Today;
            }

            return AExtractId >= 0;
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the result of the dialog
        ///
        /// </summary>
        /// <param name="AExtractMasterTable"></param>
        /// <returns>true if at least one row was selected
        /// </returns>
        public bool GetResult(ref MExtractMasterTable AExtractMasterTable)
        {
            MExtractMasterRow NewRow;

            AExtractMasterTable.Clear();

            if (FResultTable == null)
            {
                return false;
            }

            foreach (MExtractMasterRow Row in FResultTable.Rows)
            {
                NewRow = AExtractMasterTable.NewRowTyped();
                NewRow.ExtractId = Row.ExtractId;
                NewRow.ExtractName = Row.ExtractName;
                NewRow.ExtractDesc = Row.ExtractDesc;
                NewRow.KeyCount = Row.KeyCount;
                NewRow.CreatedBy = Row.CreatedBy;
                NewRow.DateCreated = Row.DateCreated;

                AExtractMasterTable.Rows.Add(NewRow);
            }

            return AExtractMasterTable.Count >= 0;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// reload extract list when search button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshExtractList(System.Object sender, EventArgs e)
        {
            bool AllUsers = true;
            String CreatedByUser = "";
            String ModifiedByUser = "";
            DateTime? DateCreatedFrom = null;
            DateTime? DateCreatedTo = null;
            DateTime? DateModifiedFrom = null;
            DateTime? DateModifiedTo = null;

            string CheckedMember = FCheckedColumnName;
            string IdMember = MExtractMasterTable.GetExtractIdDBName();
            string NameMember = MExtractMasterTable.GetExtractNameDBName();
            string DescriptionMember = MExtractMasterTable.GetExtractDescDBName();
            string DeletableMember = MExtractMasterTable.GetDeletableDBName();
            string KeyCountMember = MExtractMasterTable.GetKeyCountDBName();
            string CreatedByMember = MExtractMasterTable.GetCreatedByDBName();
            string DateCreatedMember = MExtractMasterTable.GetDateCreatedDBName();

            if (cmbUserCreated.GetSelectedString().Length > 0)
            {
                AllUsers = false;
                CreatedByUser = cmbUserCreated.GetSelectedString();
            }

            if (cmbUserModified.GetSelectedString().Length > 0)
            {
                AllUsers = false;
                ModifiedByUser = cmbUserModified.GetSelectedString();
            }

            if (dtpCreatedFrom.Text.Length > 0)
            {
                DateCreatedFrom = dtpCreatedFrom.Date;
            }

            if (dtpCreatedTo.Text.Length > 0)
            {
                DateCreatedTo = dtpCreatedTo.Date;
            }

            if (dtpModifiedFrom.Text.Length > 0)
            {
                DateModifiedFrom = dtpModifiedFrom.Date;
            }

            if (dtpModifiedTo.Text.Length > 0)
            {
                DateModifiedTo = dtpModifiedTo.Date;
            }

            FExtractMasterTable = TRemote.MPartner.Partner.WebConnectors.GetAllExtractHeaders(txtExtractName.Text,
                txtExtractDesc.Text, AllUsers, CreatedByUser, ModifiedByUser, DateCreatedFrom, DateCreatedTo,
                DateModifiedFrom, DateModifiedTo);

            if (FExtractMasterTable != null)
            {
                DataView view = new DataView(FExtractMasterTable);

                FDataTable = view.ToTable(true,
                    new string[] { IdMember, NameMember, DescriptionMember, DeletableMember, KeyCountMember, CreatedByMember, DateCreatedMember });
                FDataTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));
                clbDetails.Columns.Clear();

                if (FAllowMultipleSelect)
                {
                    clbDetails.AddCheckBoxColumn("Select", FDataTable.Columns[FCheckedColumnName], 17, false);
                }

                clbDetails.AddTextColumn("Extract Name", FDataTable.Columns[NameMember], 200);
                clbDetails.AddCheckBoxColumn("Deletable", FDataTable.Columns[DeletableMember], 120);
                clbDetails.AddTextColumn("Key Count", FDataTable.Columns[KeyCountMember], 80);
                clbDetails.AddTextColumn("Description", FDataTable.Columns[DescriptionMember], 300);

                clbDetails.DataBindGrid(FDataTable, NameMember, CheckedMember, NameMember, false, true, false);
                clbDetails.SetCheckedStringList("");
            }

            PrepareButtons();

            clbDetails.Select();
        }

        /// <summary>
        /// clear search criteria fields and empty grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearSearchCriteria(System.Object sender, EventArgs e)
        {
            txtExtractName.Text = "";
            txtExtractDesc.Text = "";
            cmbUserCreated.SetSelectedString("", -1);
            dtpCreatedFrom.Text = "";
            dtpCreatedTo.Text = "";
            cmbUserModified.SetSelectedString("", -1);
            dtpModifiedFrom.Text = "";
            dtpModifiedTo.Text = "";

            FExtractMasterTable.Clear();
            DataView myDataView = FExtractMasterTable.DefaultView;
            myDataView.AllowNew = false;
            clbDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            PrepareButtons();
        }

        /// <summary>
        /// close the screen and return the selected extract as accepted record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcceptExtract(System.Object sender, EventArgs e)
        {
            MExtractMasterRow ExtractMasterRow;

            FResultTable = new MExtractMasterTable();
            //MExtractMasterRow SelectedRow = null;
            DataRow SelectedRow = null;

            if (AllowMultipleSelect)
            {
                // multiple rows may have been selected: find all the ones where first column is ticked
                foreach (DataRow Row in FDataTable.Rows)
                {
                    if (Convert.ToBoolean(Row[FCheckedColumnName]) == true)
                    {
                        ExtractMasterRow = FResultTable.NewRowTyped();
                        ExtractMasterRow.ExtractId = (int)Row[MExtractMasterTable.GetExtractIdDBName()];
                        ExtractMasterRow.ExtractName = Row[MExtractMasterTable.GetExtractNameDBName()].ToString();
                        ExtractMasterRow.ExtractDesc = Row[MExtractMasterTable.GetExtractDescDBName()].ToString();
                        ExtractMasterRow.KeyCount = (int)Row[MExtractMasterTable.GetKeyCountDBName()];
                        ExtractMasterRow.CreatedBy = Row[MExtractMasterTable.GetCreatedByDBName()].ToString();
                        ExtractMasterRow.DateCreated = (DateTime)Row[MExtractMasterTable.GetDateCreatedDBName()];
                        FResultTable.Rows.Add(ExtractMasterRow);
                    }
                }
            }
            else
            {
                // just one row can be selected
                DataRowView[] SelectedGridRow = clbDetails.SelectedDataRowsAsDataRowView;

                if (SelectedGridRow.Length >= 1)
                {
                    SelectedRow = SelectedGridRow[0].Row;

                    ExtractMasterRow = FResultTable.NewRowTyped();
                    ExtractMasterRow.ExtractId = (int)SelectedRow[MExtractMasterTable.GetExtractIdDBName()];
                    ExtractMasterRow.ExtractName = SelectedRow[MExtractMasterTable.GetExtractNameDBName()].ToString();
                    ExtractMasterRow.ExtractDesc = SelectedRow[MExtractMasterTable.GetExtractDescDBName()].ToString();
                    ExtractMasterRow.KeyCount = (int)SelectedRow[MExtractMasterTable.GetKeyCountDBName()];
                    ExtractMasterRow.CreatedBy = SelectedRow[MExtractMasterTable.GetCreatedByDBName()].ToString();
                    ExtractMasterRow.DateCreated = (DateTime)SelectedRow[MExtractMasterTable.GetDateCreatedDBName()];
                    FResultTable.Rows.Add(ExtractMasterRow);
                }
            }

            Close();
        }

        /// <summary>
        /// close screen without accepting any extract
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseScreen(System.Object sender, EventArgs e)
        {
            if (FResultTable != null)
            {
                FResultTable.Clear();
            }

            Close();
        }

        private void GridValueChanged(object sender, EventArgs e)
        {
            this.PrepareButtons();
        }

        private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            this.PrepareButtons();
        }

        /// <summary>
        /// enable/disable screen buttons
        /// </summary>
        private void PrepareButtons()
        {
            btnOK.Enabled = false;

            // "Accept" button needs to be disabled if no row is selected
            if (AllowMultipleSelect)
            {
                if (clbDetails.CheckedItemsCount >= 1)
                {
                    btnOK.Enabled = true;
                }
                else
                {
                    btnOK.Enabled = false;
                }
            }
            else
            {
                if (clbDetails.SelectedDataRowsAsDataRowView.Length >= 1)
                {
                    btnOK.Enabled = true;
                }
                else
                {
                    btnOK.Enabled = false;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Manages the opening of a new/showing of an existing Instance of the Event Find Screen.
    /// </summary>
    public static class TExtractFindScreenManager
    {
        /// <summary>
        /// Opens a Modal instance of the Extract Find screen.
        /// </summary>
        /// <param name="AExtractId">Id of extract found</param>
        /// <param name="AExtractName">Name of the extract found</param>
        /// <param name="AExtractDesc">Description of the extract found</param>
        /// <param name="AParentForm"></param>
        /// <returns>True if an extrac was found and accepted by the user,
        /// otherwise false.</returns>
        public static bool OpenModalForm(out int AExtractId,
            out String AExtractName,
            out String AExtractDesc,
            Form AParentForm)
        {
            AExtractId = -1;
            AExtractName = String.Empty;
            AExtractDesc = String.Empty;

            TFrmExtractFindDialog SelectExtract = new TFrmExtractFindDialog(AParentForm);

            if (SelectExtract.ShowDialog(true))
            {
                SelectExtract.GetResult(out AExtractId, out AExtractName, out AExtractDesc);
                return true;
            }

            return false;
        }
    }
}