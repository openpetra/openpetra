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
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using GNU.Gettext;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupAnalysisTypes
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

                Ict.Common.Data.TTypedDataTable TypedTable;
                TRemote.MCommon.DataReader.GetData(AAnalysisTypeTable.GetTableDBName(), null, out TypedTable);
                FMainDS.AAnalysisType.Merge(TypedTable);

                ucoValues.LedgerNumber = value;
                ucoValues.LoadValues(FLedgerNumber);
            }
        }
        private void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewAAnalysisType();
            ucoValues.Enabled = true;
            txtDetailAnalysisTypeCode.Enabled = true;
        }

        private void NewRowManual(ref AAnalysisTypeRow ARow)
        {
            string newName = Catalog.GetString("NEWTYPE");
            Int32 countNewDetail = 0;

            if (FMainDS.AAnalysisType.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.AAnalysisType.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.AnalysisTypeCode = newName;
        }

        private TSubmitChangesResult StoreManualCode(ref GLSetupTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            return TRemote.MFinance.Setup.WebConnectors.SaveGLSetupTDS(FLedgerNumber, ref ASubmitChanges, out AVerificationResult);
        }

        private void DeleteRow(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            DataView view = new DataView(FMainDS.AFreeformAnalysis);
            view.RowStateFilter = DataViewRowState.CurrentRows;
            view.RowFilter = String.Format("{0} = '{1}'",
                AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                FPreviouslySelectedDetailRow.AnalysisTypeCode);

            if (view.Count > 0)
            {
                MessageBox.Show(Catalog.GetString(
                        "Please delete the unused values first!\n\nNote:Used types and types with used values cannot be deleted."));
                return;
            }

            int num = TRemote.MFinance.Setup.WebConnectors.CheckDeleteAAnalysisType(FPreviouslySelectedDetailRow.AnalysisTypeCode);

            if (num > 0)
            {
                MessageBox.Show(Catalog.GetString(
                        "This type is already referenced and cannot be deleted."));
                return;
            }

            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                || (MessageBox.Show(String.Format(Catalog.GetString(
                                "You have chosen to delete thistype ({0}).\n\nDo you really want to delete it?"),
                            FPreviouslySelectedDetailRow.AnalysisTypeCode), Catalog.GetString("Confirm Delete"),
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                int rowIndex = CurrentRowIndex();
                FPreviouslySelectedDetailRow.Delete();
                FPetraUtilsObject.SetChangedFlag();
                SelectByIndex(rowIndex);
            }
        }

        private void GetDetailDataFromControlsManual(AAnalysisTypeRow ARow)
        {
            ucoValues.GetDataFromControls();
        }

        private void GetDataFromControlsManual()
        {
            // TODO
        }

        private void ShowDetailsManual(AAnalysisTypeRow ARow)
        {
            if (ARow == null)
            {
                pnlDetails.Enabled = false;
                ucoValues.Enabled = false;
            }
            else
            {
                pnlDetails.Enabled = true;
                ucoValues.Enabled = true;
                ucoValues.TypeCode = ARow.AnalysisTypeCode;
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
            }
        }

        /// <summary>
        /// freeze the typecode after before adding a new value in the user control
        /// </summary>
        public String FreezeTypeCode()
        {
            txtDetailAnalysisTypeCode.Enabled = false;
            return txtDetailAnalysisTypeCode.Text;
        }

        private void ValidateDataManual(AAnalysisTypeRow ARow)
        {
            DataColumn ValidationColumn;

//TLogging.Log("ValidateDataManual: AnalysisTypeCode = " + ARow.AnalysisTypeCode.ToString() + "; AnalysisTypeDescription = " + ARow.AnalysisTypeDescription.ToString() );
            // 'International Telephone Code' must be positive or 0
            ValidationColumn = ARow.Table.Columns[AAnalysisTypeTable.ColumnAnalysisTypeDescriptionId];

            FPetraUtilsObject.VerificationResultCollection.AddOrRemove(
                TStringChecks.StringMustNotBeEmpty(ARow.AnalysisTypeDescription,
                    lblDetailAnalysisTypeDescription.Text,
                    this, ValidationColumn, txtDetailAnalysisTypeDescription), ValidationColumn);

//            FPetraUtilsObject.VerificationResultCollection.Add(new TScreenVerificationResult( "TestContext", ValidationColumn, "test warning", txtDetailTimeZoneMinimum, TResultSeverity.Resv_Noncritical));
        }
    }
}