//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//
// Copyright 2004-2012 by OM International
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
        DataView FAnalysisTypesForCombo;
        Boolean FIamUpdating = false;

        /// <summary>Add this in the Status Box</summary>
        public delegate void UpdateParentStatus (String Message);

        /// <summary>Add this in the Status Box</summary>
        public UpdateParentStatus ShowStatus = null;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_DataSavingStarted);
                cmbDetailAnalTypeCode.SelectedValueChanged += new System.EventHandler(OnDetailAnalysisTypeCodeChange);
            }
        }

        void RemoveUnassignedRecords()
        {
            for (Int32 i = FMainDS.AAnalysisAttribute.Rows.Count; i > 0; i--)
            {
                AAnalysisAttributeRow Row = FMainDS.AAnalysisAttribute[i - 1];

                if (Row.RowState != DataRowState.Deleted)
                {
                    if (Row.AnalysisTypeCode.StartsWith("Unassigned"))
                    {
                        Row.Delete();
                    }
                }
            }
            if ((FPreviouslySelectedDetailRow != null) && (FPreviouslySelectedDetailRow.RowState == DataRowState.Detached))
            {
                FPreviouslySelectedDetailRow = null;
            }
        }

        //
        // Before the dataset is saved, I'll go through and pull out anything that still says, "Unassigned".
        void FPetraUtilsObject_DataSavingStarted(object Sender, EventArgs e)
        {
            RemoveUnassignedRecords();
        }

        /// <summary>
        /// Loads all available AnalTypeCodes into the Combo, ensuring that the current value is allowed!
        /// </summary>
        private void LoadCmbAnalType(String AalwaysAllow)
        {
            FAnalysisTypesForCombo = new DataView(FMainDS.AAnalysisType);
            FAnalysisTypesForCombo.Sort = AAnalysisTypeTable.GetAnalysisTypeCodeDBName();
            cmbDetailAnalTypeCode.DisplayMember = "a_analysis_type_code_c";
            cmbDetailAnalTypeCode.ValueMember = "a_analysis_type_code_c";
            String Filter = "";

            foreach (DataRowView rv in FMainDS.AAnalysisAttribute.DefaultView)
            {
                AAnalysisAttributeRow AttrRow = (AAnalysisAttributeRow)rv.Row;

                if (AttrRow.AnalysisTypeCode == AalwaysAllow) // The currently assigned value is always allowed!
                {
                    continue;
                }

                if (Filter != "")
                {
                    Filter += ",";
                }

                Filter += "'" + AttrRow.AnalysisTypeCode + "'";
            }

            if (Filter != "")
            {
                FAnalysisTypesForCombo.RowFilter = "a_analysis_type_code_c NOT IN (" + Filter + ")";
            }

            cmbDetailAnalTypeCode.DataSource = FAnalysisTypesForCombo;
        }

        /// <summary>
        /// we are editing this account
        /// </summary>
        public string AccountCode
        {
            set
            {
                FAccountCode = value;
                FMainDS.AAnalysisAttribute.DefaultView.RowFilter = String.Format("{0}={1} and {2}='{3}'",
                    AAnalysisAttributeTable.GetLedgerNumberDBName(),
                    FLedgerNumber,
                    AAnalysisAttributeTable.GetAccountCodeDBName(),
                    FAccountCode);
                FMainDS.AAnalysisAttribute.DefaultView.Sort = AAnalysisAttributeTable.GetAnalysisTypeCodeDBName();

                pnlDetails.Enabled = false;
                btnDelete.Enabled = (grdDetails.Rows.Count > 1);
                UpdateRecordNumberDisplay();
            }
        }

        private void NewRow(System.Object sender, EventArgs e)
        {
            FIamUpdating = true;
            LoadCmbAnalType("");
            FIamUpdating = false;

            if (cmbDetailAnalTypeCode.Items.Count == 0)
            {
                MessageBox.Show(Catalog.GetString("Please create an analysis type first"), Catalog.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RemoveUnassignedRecords();
            CreateNewAAnalysisAttribute();
            pnlDetails.Enabled = true;
            btnDelete.Enabled = true;

            SelectRowInGrid(grdDetails.Rows.Count);
            cmbDetailAnalTypeCode.Focus();
        }

        private void ShowDetailsManual(AAnalysisAttributeRow ARow)
        {
            if ((ARow != null) && !FIamUpdating)  // How can ARow ever be null!!
            {
                FIamUpdating = true;
                LoadCmbAnalType(ARow.AnalysisTypeCode);
                cmbDetailAnalTypeCode.Text = ARow.AnalysisTypeCode;
                String ServerMessage;
                Boolean CanBeChanged =TRemote.MFinance.Setup.WebConnectors.CanDetachTypeCodeFromAccount(ARow.LedgerNumber, ARow.AccountCode, ARow.AnalysisTypeCode, out ServerMessage);
                pnlDetails.Enabled = CanBeChanged;
                if (!CanBeChanged)
                {
                    if (ShowStatus != null)
                    {
                        ShowStatus(ServerMessage);
                    }
                }
                FIamUpdating = false;
            }
        }

        private void NewRowManual(ref AAnalysisAttributeRow ARow)
        {
            ARow.LedgerNumber = FLedgerNumber;
            ARow.Active = true;
            ARow.AccountCode = FAccountCode;
            ARow.AnalysisTypeCode = "Unassigned";
        }

        private bool PreDeleteManual(AAnalysisAttributeRow ARowToDelete, ref string ADeletionQuestion)
        {
            // I can't delete any Analysis Type code that's been used in transactions.
            if ((ARowToDelete != null) && (ARowToDelete.RowState != DataRowState.Deleted))
            {
                String ServerMessage;
                if (TRemote.MFinance.Setup.WebConnectors.CanDetachTypeCodeFromAccount(ARowToDelete.LedgerNumber, ARowToDelete.AccountCode, ARowToDelete.AnalysisTypeCode, out ServerMessage))
                {
                    ADeletionQuestion = String.Format(
                        Catalog.GetString("Confirm you want to Remove {0} from this account."),
                        ARowToDelete.AnalysisTypeCode);
                    return true;
                }
                else // The server reports that this can't be deleted.
                {
                    MessageBox.Show(ServerMessage, Catalog.GetString("Delete Analysis Type"));
                }
            }

            return false;
        }

        private bool DeleteRowManual(AAnalysisAttributeRow ARowToDelete, ref string ACompletionMessage)
        {
            bool success = false;

            try
            {
                ARowToDelete.Delete();
                success = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error trying to delete current row:" + Environment.NewLine + Environment.NewLine + ex.Message);
            }

            return success;
        }

        private void PostDeleteManual(AAnalysisAttributeRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
        }

        private void OnDetailAnalysisTypeCodeChange(System.Object sender, EventArgs e)
        {
            if (!FIamUpdating && (FPreviouslySelectedDetailRow != null) && (FPreviouslySelectedDetailRow.RowState != DataRowState.Deleted))
            {
                FPreviouslySelectedDetailRow.AnalysisTypeCode = cmbDetailAnalTypeCode.Text;

                //
                // The change may have altered the ordering of the list, 
                // so now I need to re-select the item, wherever it's gone!
                Int32 RowIdx = FMainDS.AAnalysisAttribute.DefaultView.Find(cmbDetailAnalTypeCode.Text);
                SelectByIndex(RowIdx + 1);
            }
        }

        private void GetDetailDataFromControlsManual(AAnalysisAttributeRow ARow)
        {
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
            grdDetails.Selection.ResetSelection(true);
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