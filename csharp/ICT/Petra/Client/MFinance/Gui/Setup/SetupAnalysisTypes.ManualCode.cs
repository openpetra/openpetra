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
using Ict.Petra.Shared.MFinance.Validation;
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
                TRemote.MCommon.DataReader.WebConnectors.GetData(AAnalysisTypeTable.GetTableDBName(), null, out TypedTable);
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
                                "You have chosen to delete this type ({0}).\n\nDo you really want to delete it?"),
                            FPreviouslySelectedDetailRow.AnalysisTypeCode), Catalog.GetString("Confirm Delete"),
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                int rowIndex = grdDetails.SelectedRowIndex();
                FPreviouslySelectedDetailRow.Delete();
                FPetraUtilsObject.SetChangedFlag();
                SelectRowInGrid(rowIndex);
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
                txtDetailAnalysisTypeCode.Enabled = ucoValues.Count == 0;
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
    }
}