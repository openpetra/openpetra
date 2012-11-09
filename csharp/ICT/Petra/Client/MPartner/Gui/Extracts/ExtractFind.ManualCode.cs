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
    public partial class TFrmExtractFind
    {
        // indicates if option check box is ticked or not
        private int FExtractId;
        private String FExtractName;
        private String FExtractDescription;
        private String FExtractCreatedBy;
        private DateTime FExtractDateCreated;

        private MExtractMasterTable FExtractMasterTable;

        #region Public Methods

        /// <summary>
        /// show form as dialog with given parameters
        /// </summary>
        /// <returns>Boolean: true if Accept button was clicked</returns>
        public bool ShowDialog(bool ADummy)
        {
            FExtractId = -1;
            FExtractName = "";
            FExtractDescription = "";
            FExtractCreatedBy = "";

            // now show the actual dialog
            this.StartPosition = FormStartPosition.CenterScreen;

            FExtractMasterTable = new MExtractMasterTable();

            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Extract Name", FExtractMasterTable.ColumnExtractName);
            grdDetails.AddCheckBoxColumn("Deletable", FExtractMasterTable.ColumnDeletable);
            grdDetails.AddTextColumn("Key Count", FExtractMasterTable.ColumnKeyCount, 80);
            grdDetails.AddTextColumn("Description", FExtractMasterTable.ColumnExtractDesc);

            // clear fields for search criteria, preset user field with current user
            ClearSearchCriteria(null, null);
            cmbUserCreated.SetSelectedString(UserInfo.GUserInfo.UserID, -1);

            DataView myDataView = FExtractMasterTable.DefaultView;
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            // disable "Accept" button if no record is selected
            PrepareButtons();

            this.ShowDialog();

            return true;
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the result of the dialog
        ///
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <param name="AExtractName"></param>
        /// <param name="AExtractDescription"></param>
        /// <returns>information which button was pressed
        /// </returns>
        public bool GetResult(out int AExtractId, out String AExtractName, out String AExtractDescription)
        {
            AExtractId = FExtractId;
            AExtractName = FExtractName;
            AExtractDescription = FExtractDescription;

            return FExtractId >= 0;
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the result of the dialog
        ///
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <param name="AExtractName"></param>
        /// <param name="AExtractDescription"></param>
        /// <param name="AExtractCreatedBy"></param>
        /// <param name="AExtractDateCreated"></param>
        /// <returns>information which button was pressed
        /// </returns>
        public bool GetResult(out int AExtractId, out String AExtractName, out String AExtractDescription,
                              out String AExtractCreatedBy, out DateTime AExtractDateCreated)
        {
            AExtractId = FExtractId;
            AExtractName = FExtractName;
            AExtractDescription = FExtractDescription;
            AExtractCreatedBy = FExtractCreatedBy;
            AExtractDateCreated = FExtractDateCreated;

            return FExtractId >= 0;
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
                DataView myDataView = FExtractMasterTable.DefaultView;
                myDataView.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            }

            PrepareButtons();
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
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            PrepareButtons();
        }

        /// <summary>
        /// close the screen and return the selected extract as accepted record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcceptExtract(System.Object sender, EventArgs e)
        {
            MExtractMasterRow SelectedRow = null;

            DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                SelectedRow = (MExtractMasterRow)SelectedGridRow[0].Row;
                FExtractId = SelectedRow.ExtractId;
                FExtractName = SelectedRow.ExtractName;
                FExtractDescription = SelectedRow.ExtractDesc;
                FExtractCreatedBy = SelectedRow.CreatedBy;
                FExtractDateCreated = (DateTime)SelectedRow.DateCreated;
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
            FExtractId = -1;
            FExtractName = "";
            FExtractDescription = "";
            FExtractCreatedBy = "";
            Close();
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
            // "Accept" button needs to be disabled if no row is selected
            if (grdDetails.SelectedDataRowsAsDataRowView.Length >= 1)
            {
                btnAccept.Enabled = true;
            }
            else
            {
                btnAccept.Enabled = false;
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

            TFrmExtractFind SelectExtract = new TFrmExtractFind(AParentForm);

            if (SelectExtract.ShowDialog(true))
            {
                SelectExtract.GetResult(out AExtractId, out AExtractName, out AExtractDesc);
                return true;
            }

            return false;
        }
    }
}