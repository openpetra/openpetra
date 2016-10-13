//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp, christiank
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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;

using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    /// <summary>
    /// Manual Code implementation of partial class
    /// </summary>
    public partial class TFrmAddressLayoutCodeSetup
    {
        // Needed for Module-based Read-only security checks only.
        private string FContext;

        /// <summary>Needed for Module-based Read-only security checks only.</summary>
        public string Context
        {
            get
            {
                return FContext;
            }

            set
            {
                FContext = value;
            }
        }

        // Instance of a 'Helper Class' for handling the Indexes of the DataRows. (The Grid is sorted by the Index.)
        TSgrdDataGrid.IndexedGridRowsHelper FIndexedGridRowsHelper;

        #region Initialisation

        private void RunOnceOnActivationManual()
        {
            FIndexedGridRowsHelper = new TSgrdDataGrid.IndexedGridRowsHelper(
                grdDetails, PAddressLayoutCodeTable.ColumnDisplayIndexId, btnDemote, btnPromote,
                delegate { FPetraUtilsObject.SetChangedFlag(); });

            // Always disable the deletable flag
            chkDetailDeletable.Enabled = false;

            // No grid sorting because it is done using the DisplayIndex column
            grdDetails.SortableHeaders = false;
            FMainDS.PAddressLayoutCode.DefaultView.Sort = String.Format("{0} ASC", PAddressLayoutCodeTable.GetDisplayIndexDBName());

            // Select first row after altering the sort
            SelectRowInGrid(1);

            if (FPetraUtilsObject.SecurityReadOnly)
            {
                btnDelete.Enabled = false;
            }
        }

        #endregion

        #region Event Handlers

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPAddressLayoutCode();
        }

        private void PromoteRecord(Object sender, EventArgs e)
        {
            FIndexedGridRowsHelper.PromoteRow();
        }

        private void DemoteRecord(Object sender, EventArgs e)
        {
            FIndexedGridRowsHelper.DemoteRow();
        }

        private void PromoteOrDemoteRecord(Boolean APromote)
        {
            // Get the display index of the current row
            int curIndex = FPreviouslySelectedDetailRow.DisplayIndex;

            // Create a view on this display index and the neighbouring one
            DataView dv = new DataView(FMainDS.PAddressLayoutCode,
                String.Format("{0}={1} OR {0}={2}",
                    PAddressLayoutCodeTable.GetDisplayIndexDBName(),
                    curIndex,
                    APromote ? curIndex - 1 : curIndex + 1),
                String.Format("{0} ASC", PAddressLayoutCodeTable.GetDisplayIndexDBName()),
                DataViewRowState.CurrentRows);

            // We should have two rows
            if (dv.Count == 2)
            {
                PAddressLayoutCodeRow row1 = (PAddressLayoutCodeRow)dv[0].Row;
                PAddressLayoutCodeRow row2 = (PAddressLayoutCodeRow)dv[1].Row;

                row1.DisplayIndex = row1.DisplayIndex + 1;
                row2.DisplayIndex = row2.DisplayIndex - 1;

                FMainDS.PAddressLayoutCode.DefaultView.Sort = String.Format("{0} ASC", PAddressLayoutCodeTable.GetDisplayIndexDBName());

                if (APromote)
                {
                    SelectRowInGrid(GetSelectedRowIndex() - 1);
                }
                else
                {
                    SelectRowInGrid(GetSelectedRowIndex() + 1);
                }

                FPetraUtilsObject.SetChangedFlag();
            }
            else
            {
                // The display index values in the database have got mixed up.
                // Either there is a break in the sequence or there are two rows with the same index
                MessageBox.Show(Catalog.GetString(
                        "The application encountered a problem with the record order, but has attempted to fix it.  Please try clicking the button again."),
                    Catalog.GetString("Layout Code Order"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                // We can use the same code we use after deleting rows to restore a sequential display order
                FixDisplayIndexValues();
            }
        }

        private void DeleteRecord(Object sender, EventArgs e)
        {
            if (TDeleteGridRows.DeleteRows(this, grdDetails, FPetraUtilsObject, this))
            {
                // We did actually delete one or more rows so we need to fix up the display order again
                FixDisplayIndexValues();
            }
        }

        #endregion

        #region Manual method extensions

        private void NewRowManual(ref PAddressLayoutCodeRow ARow)
        {
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.PAddressLayoutCode.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PAddressLayoutCode.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.Code = newName;
            ARow.DisplayIndex = FMainDS.PAddressLayoutCode.Rows.Count + 1;
        }

        private void ShowDetailsManual(PAddressLayoutCodeRow ARow)
        {
            if (FIndexedGridRowsHelper != null)
            {
                FIndexedGridRowsHelper.UpdateButtons(GetSelectedRowIndex(), FPetraUtilsObject.SecurityReadOnly);
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns true if one of the selected rows can be deleted.
        /// </summary>
        private bool CanDeleteSelection()
        {
            DataRowView[] selectedRows = grdDetails.SelectedDataRowsAsDataRowView;

            foreach (DataRowView drv in selectedRows)
            {
                if (((PAddressLayoutCodeRow)drv.Row).Deletable)
                {
                    return true;
                }
            }

            return false;
        }

        // A method that fixes the values of DisplayIndex so they are sequential.
        // Used in particular after deleting rows
        private void FixDisplayIndexValues()
        {
            for (int i = 0; i < FMainDS.PAddressLayoutCode.DefaultView.Count; i++)
            {
                PAddressLayoutCodeRow row = (PAddressLayoutCodeRow)FMainDS.PAddressLayoutCode.DefaultView[i].Row;

                if (row.DisplayIndex != i + 1)
                {
                    row.DisplayIndex = i + 1;
                }
            }
        }

        #endregion

        #region Security

        private List <string>ApplySecurityManual()
        {
            // Whatever the Context is: the Module to check security for is *always* MPartner!
            FPetraUtilsObject.SecurityScreenContext = "MPartner";

            return new List <string>();
        }

        private void AfterRunOnceOnActivationManual()
        {
            TSetupScreensSecurityHelper.ShowMsgUserWillNeedToHaveDifferentAdminModulePermissionForEditing(
                this, FPetraUtilsObject.SecurityReadOnly, FContext, Catalog.GetString("Partner"), "PTNRADMIN",
                "AddressLayoutCodeSetup_R-O_");
        }

        #endregion
    }
}