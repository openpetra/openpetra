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
                int rowIndex = GetSelectedRowIndex();
                FMainDS.AFreeformAnalysis.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                    AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                    FTypeCode);
                SelectRowInGrid(rowIndex);
                
                UpdateRecordNumberDisplay();
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
            FMainDS.AFreeformAnalysis.DefaultView.Sort = AFreeformAnalysisTable.GetAnalysisValueDBName();
        }

        private void ShowDetailsManual(AFreeformAnalysisRow ARow)
        {
            if (ARow == null)
            {
                txtDetailAnalysisValue.Clear();
                return;
            }
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