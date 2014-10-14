//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
//
// Copyright 2004-2014 by OM International
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
using Ict.Petra.Client.App.Gui;
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
                FMainDS.AAnalysisType.DefaultView.Sort = AAnalysisTypeTable.GetAnalysisTypeCodeDBName();

                ucoValues.LedgerNumber = value;
                ucoValues.LoadValues(FLedgerNumber);

                /* fix tab order */
                pnlButtons.TabIndex = grdDetails.TabIndex + 1;
                
                SelectRowInGrid(1);
                UpdateRecordNumberDisplay();
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

        private TSubmitChangesResult StoreManualCode(ref GLSetupTDS ASubmitTDS, out TVerificationResultCollection AVerificationResult)
        {
            //
            // I'll warn the user if they have created analysis Types with no values:

            String EmptyTypesWarning = "";

            //
            // If the user has added a new type, I want to show her a warning if she didn't also add at least one value for the new type.

            if (ASubmitTDS.AAnalysisType != null)
            {
                foreach (AAnalysisTypeRow TypeRow in ASubmitTDS.AAnalysisType.Rows)
                {
                    if (TypeRow.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    Boolean NoValuesProvided = true;

                    if (ASubmitTDS.AFreeformAnalysis != null)
                    {
                        ASubmitTDS.AFreeformAnalysis.DefaultView.RowFilter =
                            String.Format("a_analysis_type_code_c='{0}'", TypeRow["a_analysis_type_code_c"]);
                        NoValuesProvided = ASubmitTDS.AFreeformAnalysis.DefaultView.Count == 0;
                    }

                    if (NoValuesProvided)
                    {
                        if (EmptyTypesWarning != "")
                        {
                            EmptyTypesWarning += "\r\n";
                        }

                        EmptyTypesWarning +=
                            String.Format(Catalog.GetString("Type {0} has no values, and therefore it cannot yet be applied to any account."),
                                TypeRow["a_analysis_type_code_c"]);
                    }
                }
            }

            if (EmptyTypesWarning != "")
            {
                MessageBox.Show(EmptyTypesWarning, Catalog.GetString("Empty Analysis Types"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return TRemote.MFinance.Setup.WebConnectors.SaveGLSetupTDS(FLedgerNumber, ref ASubmitTDS, out AVerificationResult);
        }

        private void DeleteRecord(object sender, EventArgs e)
        {
            if (ucoValues.Count > 0)
            {
                string msg = string.Empty;

                if (ucoValues.GridCount == 0)
                {
                    // The grid must be filtered because it has no rows!
                    msg += Catalog.GetString(
                        "The selected Analysis Type has Analysis Values associated with it, but they are being filtered out of the display.");
                    msg += "  ";
                }

                msg += Catalog.GetString("You must delete all the Analysis Values for the selected Analysis Type before you can delete this record.");
                MessageBox.Show(msg, MCommon.MCommonResourcestrings.StrRecordDeletionTitle);
                return;
            }

            DeleteAAnalysisType();
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(AAnalysisTypeRow ARowToDelete, ref string ADeletionQuestion)
        {
            /*Code to execute before the delete can take place*/
            DataView DependentRecordsDV = new DataView(FMainDS.AFreeformAnalysis);

            DependentRecordsDV.RowStateFilter = DataViewRowState.CurrentRows;
            DependentRecordsDV.RowFilter = String.Format("{0} = '{1}'",
                AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                ARowToDelete.AnalysisTypeCode);

            if (DependentRecordsDV.Count > 0)
            {
                // Tell the user that we cannot allow deletion if any rows exist in the DataView
                TMessages.MsgRecordCannotBeDeletedDueToDependantRecordsError(
                    "Analysis Type", "an Analysis Type", "Analysis Types", "Analysis Value", "an Analysis Value",
                    "Analysis Values", ARowToDelete.AnalysisTypeCode, DependentRecordsDV.Count);

                return false;
            }

            return true;
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