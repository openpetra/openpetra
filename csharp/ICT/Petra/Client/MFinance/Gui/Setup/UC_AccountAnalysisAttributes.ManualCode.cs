//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance.Account.Data;
using GNU.Gettext;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TUC_AccountAnalysisAttributes
    {
        private Int32 FLedgerNumber;
        private string FAccountCode;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
            }
        }

        /// <summary>
        /// we are editing this account
        /// </summary>
        public string AccountCode
        {
            set
            {
                FAccountCode = value;
                FMainDS.AAnalysisAttribute.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                    AAnalysisAttributeTable.GetAccountCodeDBName(),
                    FAccountCode);
            }
        }

        private void NewRow(System.Object sender, EventArgs e)
        {
            // reload analysis types from cache table
            cmbDetailAnalysisTypeCode.InitialiseUserControl();

            if (cmbDetailAnalysisTypeCode.Count == 0)
            {
                MessageBox.Show(Catalog.GetString("Please create an analysis type first"), Catalog.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GetDataFromControls();
            this.CreateNewAAnalysisAttribute();
            pnlDetails.Enabled = true;
        }

        private void NewRowManual(ref AAnalysisAttributeRow ARow)
        {
            ARow.LedgerNumber = FLedgerNumber;
            ARow.Active = true;
            ARow.AccountCode = FAccountCode;

            // TODO: this does not work if there exists not type yet, or if the first type has not been used yet
            cmbDetailAnalysisTypeCode.SelectedIndex = 0;
            ARow.AnalysisTypeCode = cmbDetailAnalysisTypeCode.GetSelectedString();
        }

        private void DeleteRow(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

#if TODO
// TODO: this code might be similar to the code for FreeFormAnalysis
            int num = TRemote.MFinance.Setup.WebConnectors.CheckDeleteAFreeformAnalysis(FLedgerNumber,
                FPreviouslySelectedDetailRow.AnalysisTypeCode,
                FPreviouslySelectedDetailRow.AnalysisValue);

            if (num > 0)
            {
                MessageBox.Show(Catalog.GetString(
                        "This value is already referenced and cannot be deleted."));
                return;
            }

            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                || (MessageBox.Show(String.Format(Catalog.GetString(
                                "You have chosen to delete this value ({0}).\n\nDo you really want to delete it?"),
                            FPreviouslySelectedDetailRow.AnalysisValue), Catalog.GetString("Confirm Delete"),
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                int rowIndex = CurrentRowIndex();
                FPreviouslySelectedDetailRow.Delete();
                FPetraUtilsObject.SetChangedFlag();
                SelectByIndex(rowIndex);
            }
#endif
        }

        private void GetDetailDataFromControlsManual(AAnalysisAttributeRow ARow)
        {
            // TODO
        }

        private int CurrentRowIndex()
        {
            int rowIndex = -1;

            SourceGrid.RangeRegion selectedRegion = grdDetails.Selection.GetSelectionRegion();

            if ((selectedRegion != null) && (selectedRegion.GetRowsIndex().Length > 0))
            {
                rowIndex = selectedRegion.GetRowsIndex()[0];
            }

            return rowIndex;
        }

        private void SelectByIndex(int rowIndex)
        {
            if (rowIndex >= grdDetails.Rows.Count)
            {
                rowIndex = grdDetails.Rows.Count - 1;
            }

            if ((rowIndex < 1) && (grdDetails.Rows.Count > 1))
            {
                rowIndex = 1;
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);
            }
            else
            {
                FPreviouslySelectedDetailRow = null;
                chkDetailActive.Checked = false;
            }
        }
    }
}