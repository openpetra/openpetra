//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
        /// Loads all the currently assigned AnalTypeCodes into the Combo -
        /// Even where some of these may not be valid DB values...
        /// </summary>
        private void LoadCmbAnalType()
        {
            cmbDetailAnalTypeCode.Items.Clear();

            foreach (AAnalysisTypeRow Row in FMainDS.AAnalysisType.Rows)
            {
                if (Row.RowState != DataRowState.Deleted)
                {
                    if (!cmbDetailAnalTypeCode.Items.Contains(Row.AnalysisTypeCode))
                    {
                        cmbDetailAnalTypeCode.Items.Add(Row.AnalysisTypeCode);
                    }
                }
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
                FMainDS.AAnalysisAttribute.DefaultView.RowFilter = String.Format("{0}={1} and {2}='{3}'",
                    AAnalysisAttributeTable.GetLedgerNumberDBName(),
                    FLedgerNumber,
                    AAnalysisAttributeTable.GetAccountCodeDBName(),
                    FAccountCode);

                LoadCmbAnalType();
                pnlDetails.Enabled = false;
            }
        }

        private void NewRow(System.Object sender, EventArgs e)
        {
            if (cmbDetailAnalTypeCode.Items.Count == 0)
            {
                MessageBox.Show(Catalog.GetString("Please create an analysis type first"), Catalog.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

//          GetDataFromControls();
            this.CreateNewAAnalysisAttribute();
            pnlDetails.Enabled = true;
        }

        private string NewUniqueAnalTypeCode()
        {
            string NewTypeCode = "Unassigned";
            int ItmIdx = -1;
            int FindAttempt = 0;

            FMainDS.AAnalysisAttribute.DefaultView.Sort =
                AAnalysisAttributeTable.GetLedgerNumberDBName() + ", " +
                AAnalysisAttributeTable.GetAnalysisTypeCodeDBName() + ", " +
                AAnalysisAttributeTable.GetAccountCodeDBName();

            do
            {
                ItmIdx = FMainDS.AAnalysisAttribute.DefaultView.Find(new object[] { FLedgerNumber, NewTypeCode, FAccountCode });

//                TLogging.Log("NewUniqueAnalTypeCode: Find (" + NewTypeCode + ") = " + ItmIdx);
                if (ItmIdx >= 0)
                {
                    FindAttempt++;
                    NewTypeCode = "Unassigned-" + FindAttempt.ToString();
                }
            } while (ItmIdx >= 0);

            return NewTypeCode;
        }

        private void ShowDetailsManual(AAnalysisAttributeRow ARow)
        {
            if (ARow != null)  // How can ARow ever be null!!
            {
                LoadCmbAnalType();
                cmbDetailAnalTypeCode.Text = ARow.AnalysisTypeCode;
                pnlDetails.Enabled = false;
            }
        }

        private void NewRowManual(ref AAnalysisAttributeRow ARow)
        {
            ARow.LedgerNumber = FLedgerNumber;
            ARow.Active = true;
            ARow.AccountCode = FAccountCode;

            //            cmbDetailAnalTypeCode.SelectedIndex = 0; // I'm not convinced about this...

            ARow.AnalysisTypeCode = NewUniqueAnalTypeCode();
        }

        private void DeleteRow(System.Object sender, EventArgs e)
        {
            DeleteAAnalysisAttribute();
        }

        private bool PreDeleteManual(AAnalysisAttributeRow ARowToDelete, ref string ADeletionQuestion)
        {
            ADeletionQuestion = String.Format(
                Catalog.GetString("Confirm you want to Remove {0} from this account."),
                FPreviouslySelectedDetailRow.AnalysisTypeCode);
            return true;
        }

        private void OnDetailAnalysisTypeCodeChange(System.Object sender, EventArgs e)
        {
            GetDataFromControls();
        }

        private void GetDetailDataFromControlsManual(AAnalysisAttributeRow ARow)
        {
            if (ARow != null) // Why would it ever be null!
            {
                // I need to check whether this row will break a DB constraint.

                // The row is being edited right now, (It's in a BeginEdit ... EndEdit bracket) so it doesn't show up in the DefaultView.
                // I need to call EndEdit, but I'll give this row a "safe" value first.

                string TempEdit = cmbDetailAnalTypeCode.Text;
                string PreviousValue = ARow.AnalysisTypeCode;
                ARow.AnalysisTypeCode = "temp_edit";
                ARow.EndEdit();

                string FilterString = String.Format("{0}={3} and {1}='{4}' and {2}='{5}'",
                    AAnalysisAttributeTable.GetLedgerNumberDBName(),
                    AAnalysisAttributeTable.GetAnalysisTypeCodeDBName(),
                    AAnalysisAttributeTable.GetAccountCodeDBName(),
                    FLedgerNumber,
                    TempEdit,
                    FAccountCode);
                FMainDS.AAnalysisAttribute.DefaultView.RowFilter = FilterString;
                //            TLogging.Log("Check for unique TypeCode (" + TempEdit + ") : " + FMainDS.AAnalysisAttribute.DefaultView.Count + " Matches.");
                Boolean MustReplaceName = (FMainDS.AAnalysisAttribute.DefaultView.Count > 0);

                FMainDS.AAnalysisAttribute.DefaultView.RowFilter = String.Format("{0}={1} and {2}='{3}'",
                    AAnalysisAttributeTable.GetLedgerNumberDBName(),
                    FLedgerNumber,
                    AAnalysisAttributeTable.GetAccountCodeDBName(),
                    FAccountCode);

                ARow.BeginEdit();

                if (MustReplaceName)
                {
                    ARow.AnalysisTypeCode = PreviousValue;
                    cmbDetailAnalTypeCode.Text = PreviousValue;
                    //                TLogging.Log("Replace name: " + ARow.AnalysisTypeCode);
                }
                else
                {
                    ARow.AnalysisTypeCode = TempEdit;
                    //                TLogging.Log("Keep name: " + ARow.AnalysisTypeCode);
                }

                ARow.EndEdit(); // Apply these changes now!
                ARow.BeginEdit();
            }
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