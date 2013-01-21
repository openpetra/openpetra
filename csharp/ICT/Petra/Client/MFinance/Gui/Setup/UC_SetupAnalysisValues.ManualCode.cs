//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, timop
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
using Ict.Petra.Client.MFinance.Logic;

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
                txtHeaderLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
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
                int rowIndex = grdDetails.SelectedRowIndex();
                FMainDS.AFreeformAnalysis.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                    AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                    FTypeCode);
                SelectRowInGrid(rowIndex);
            }
        }
        private void NewRow(System.Object sender, EventArgs e)
        {
            TypeCode = ((TFrmSetupAnalysisTypes)ParentForm).FreezeTypeCode();
            this.CreateNewAFreeformAnalysis();
        }

        private void NewRowManual(ref AFreeformAnalysisRow ARow)
        {
            string newName = Catalog.GetString("NEWVALUE");
            Int32 countNewDetail = 0;

            if (FMainDS.AFreeformAnalysis.Rows.Find(new object[] { FLedgerNumber, FTypeCode, newName }) != null)
            {
                while (FMainDS.AFreeformAnalysis.Rows.Find(new object[] { FLedgerNumber, FTypeCode, newName + countNewDetail.ToString() }) != null)
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
                int rowIndex = grdDetails.SelectedRowIndex();
                FPreviouslySelectedDetailRow.Delete();
                FPetraUtilsObject.SetChangedFlag();
                SelectRowInGrid(rowIndex);
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

        /// <summary>
        /// The number of values in the grid for the current Type
        /// </summary>
        public int Count
        {
            get
            {
                return grdDetails.Rows.Count - 1;
            }
        }
    }
}