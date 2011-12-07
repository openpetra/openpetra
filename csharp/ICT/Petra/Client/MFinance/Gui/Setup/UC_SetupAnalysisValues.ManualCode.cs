//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
//
// Copyright 2004-2010 by OM International
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
    public partial class TUC_SetupAnalysisValues
    {
        private Int32 FLedgerNumber;


        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                txtHeaderLedgerNumber.Text = "" + value;
            }
        }
        private String FTypeCode;
        /// <summary>
        /// these values are for this TypeCode
        /// </summary>
        public String TypeCode
        {
            set
            {
                FTypeCode = value;
                //save the position of the actual row
                int rowIndex = CurrentRowIndex();
                FMainDS.AFreeformAnalysis.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                    AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                    FTypeCode);
                SelectByIndex(rowIndex);
            }
        }
        private void NewRow(System.Object sender, EventArgs e)
        {
            GetDataFromControls();
            TypeCode = ((TFrmSetupAnalysisTypes)ParentForm).FreezeTypeCode();
            this.CreateNewAFreeformAnalysis();
            pnlDetails.Enabled = true;
        }

        private void NewRowManual(ref AFreeformAnalysisRow ARow)
        {
            string newName = Catalog.GetString("NEWVALUE");
            Int32 countNewDetail = 0;

            if (FMainDS.AFreeformAnalysis.Rows.Find(new object[] { FTypeCode, newName, FLedgerNumber }) != null)
            {
                while (FMainDS.AFreeformAnalysis.Rows.Find(new object[] { FTypeCode, newName + countNewDetail.ToString(), FLedgerNumber }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.AnalysisValue = newName;
            ARow.LedgerNumber = FLedgerNumber;
            ARow.AnalysisTypeCode = FTypeCode;
        }

        private void DeleteRow(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

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
        }

        private void GetDetailDataFromControlsManual(AFreeformAnalysisRow ARow)
        {
            // TODO
        }

        /// <summary>
        /// load the values into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadValues(Int32 ALedgerNumber)
        {
            //GLSetupTDS MergeDS=TRemote.MFinance.Setup.WebConnectors.LoadValues(FLedgerNumber);
            AFreeformAnalysisTable AT = TRemote.MFinance.Setup.WebConnectors.LoadAFreeformAnalysis(FLedgerNumber);
            AFreeformAnalysisTable myAT = FMainDS.AFreeformAnalysis;

            myAT.Merge(AT);
            //FMainDS.AFreeformAnalysis.Merge(TRemote.MFinance.Setup.WebConnectors.LoadValues(FLedgerNumber).AFreeformAnalysis);
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
                txtDetailAnalysisValue.ResetText();
                chkDetailActive.Checked = false;
            }
        }
    }
}