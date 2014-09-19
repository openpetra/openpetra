//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Collections.Specialized;
using System.Data;
using System.Drawing;

using Ict.Common.Controls;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Logic
{
    /// <summary>
    /// Common logic for Analysis Attributes in GL Transactions and Allocation/Reallocation Journal
    /// </summary>
    public class TAnalysisAttributes
    {
        private readonly int FLedgerNumber;
        private readonly int FBatchNumber;
        private readonly int FJournalNumber;

        /// <summary>
        /// contructor
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        public TAnalysisAttributes(int ALedgerNumber, int ABatchNumber, int AJournalNumber)
        {
            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FJournalNumber = AJournalNumber;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AStringCollection"></param>
        /// <param name="AWrapString"></param>
        /// <returns></returns>
        public static string ConvertStringCollectionToCSV(StringCollection AStringCollection, string AWrapString = "")
        {
            string csvRetVal = string.Empty;

            int sizeCollection = AStringCollection.Count;

            if (sizeCollection > 0)
            {
                string[] allStrings = new string[sizeCollection];
                AStringCollection.CopyTo(allStrings, 0);

                csvRetVal = AWrapString + String.Join(AWrapString + ", " + AWrapString, allStrings) + AWrapString;
            }

            return csvRetVal;
        }

        /// <summary>
        /// Return selected row
        /// </summary>
        /// <param name="AGrid"></param>
        /// <returns></returns>
        public static ATransAnalAttribRow GetSelectedAttributeRow(TSgrdDataGridPaged AGrid)
        {
            DataRowView[] SelectedGridRow = AGrid.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                return (ATransAnalAttribRow)SelectedGridRow[0].Row;
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AGLBatchDS"></param>
        /// <param name="AActiveOnly"></param>
        /// <param name="ATransactionNumber"></param>
        /// <param name="AAnalysisCodeFilterValues"></param>
        public void SetTransAnalAttributeDefaultView(GLBatchTDS AGLBatchDS,
            bool AActiveOnly,
            Int32 ATransactionNumber = 0,
            String AAnalysisCodeFilterValues = "")
        {
            if (FBatchNumber != -1)
            {
                if (ATransactionNumber > 0)
                {
                    if (AActiveOnly && (AAnalysisCodeFilterValues.Length > 0))
                    {
                        AGLBatchDS.ATransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5} AND {6} IN ({7})",
                            ATransAnalAttribTable.GetBatchNumberDBName(),
                            FBatchNumber,
                            ATransAnalAttribTable.GetJournalNumberDBName(),
                            FJournalNumber,
                            ATransAnalAttribTable.GetTransactionNumberDBName(),
                            ATransactionNumber,
                            ATransAnalAttribTable.GetAnalysisTypeCodeDBName(),
                            AAnalysisCodeFilterValues);
                    }
                    else
                    {
                        AGLBatchDS.ATransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                            ATransAnalAttribTable.GetBatchNumberDBName(),
                            FBatchNumber,
                            ATransAnalAttribTable.GetJournalNumberDBName(),
                            FJournalNumber,
                            ATransAnalAttribTable.GetTransactionNumberDBName(),
                            ATransactionNumber);
                    }
                }
                else
                {
                    AGLBatchDS.ATransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3}",
                        ATransAnalAttribTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ATransAnalAttribTable.GetJournalNumberDBName(),
                        FJournalNumber);
                }

                AGLBatchDS.ATransAnalAttrib.DefaultView.Sort = String.Format("{0} ASC, {1} ASC",
                    ATransAnalAttribTable.GetTransactionNumberDBName(),
                    ATransAnalAttribTable.GetAnalysisTypeCodeDBName()
                    );
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AAccountCode"></param>
        /// <param name="AAnalysisAttribute"></param>
        /// <param name="AAnalysisCode"></param>
        /// <returns></returns>
        public bool AnalysisCodeIsActive(string AAccountCode, AAnalysisAttributeTable AAnalysisAttribute, String AAnalysisCode = "")
        {
            bool retVal = true;

            if ((AAnalysisCode == string.Empty) || (AAccountCode == string.Empty))
            {
                return retVal;
            }

            DataView dv = new DataView(AAnalysisAttribute);

            dv.RowFilter = String.Format("{0}={1} AND {2}='{3}' AND {4}='{5}' AND {6}=true",
                AAnalysisAttributeTable.GetLedgerNumberDBName(),
                FLedgerNumber,
                AAnalysisAttributeTable.GetAccountCodeDBName(),
                AAccountCode,
                AAnalysisAttributeTable.GetAnalysisTypeCodeDBName(),
                AAnalysisCode,
                AAnalysisAttributeTable.GetActiveDBName());

            retVal = (dv.Count > 0);

            return retVal;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AGridCombo"></param>
        /// <param name="AAnalysisAttribute"></param>
        /// <param name="AAnalysisCode"></param>
        /// <param name="AAnalysisAttributeValue"></param>
        /// <returns></returns>
        public static bool AnalysisAttributeValueIsActive(ref SourceGrid.Cells.Editors.ComboBox AGridCombo, AFreeformAnalysisTable AAnalysisAttribute,
            String AAnalysisCode = "", String AAnalysisAttributeValue = "")
        {
            bool retVal = true;

            if ((AAnalysisCode == string.Empty) || (AAnalysisAttributeValue == string.Empty))
            {
                return retVal;
            }

            DataView dv = new DataView(AAnalysisAttribute);

            dv.RowFilter = String.Format("{0}='{1}' AND {2}='{3}' AND {4}=true",
                AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                AAnalysisCode,
                AFreeformAnalysisTable.GetAnalysisValueDBName(),
                AAnalysisAttributeValue,
                AFreeformAnalysisTable.GetActiveDBName());

            retVal = (dv.Count > 0);

            //Make sure the grid combobox has right font else it will adopt strikeout
            // for all items in the list.
            AGridCombo.Control.Font = new Font(FontFamily.GenericSansSerif, 8);

            return retVal;
        }

        /// <summary>
        /// Need to ensure that the Analysis Attributes grid has all the entries
        /// that are required for the selected account.
        /// There may or may not already be attribute assignments for this transaction.
        /// </summary>
        /// <param name="AGLBatchDS"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ATransactionNumber"></param>
        public void ReconcileTransAnalysisAttributes(ref GLBatchTDS AGLBatchDS, string AAccountCode, int ATransactionNumber)
        {
            if (string.IsNullOrEmpty(AAccountCode))
            {
                return;
            }

            StringCollection RequiredAnalAttrCodes = TRemote.MFinance.Setup.WebConnectors.RequiredAnalysisAttributesForAccount(FLedgerNumber,
                AAccountCode, true);

            SetTransAnalAttributeDefaultView(AGLBatchDS, true, ATransactionNumber,
                TAnalysisAttributes.ConvertStringCollectionToCSV(RequiredAnalAttrCodes, "'"));

            // If the AnalysisType list I'm currently using is the same as the list of required types, I can keep it (with any existing values).
            bool existingListIsOk = (RequiredAnalAttrCodes.Count == AGLBatchDS.ATransAnalAttrib.DefaultView.Count);

            if (existingListIsOk)
            {
                foreach (DataRowView rv in AGLBatchDS.ATransAnalAttrib.DefaultView)
                {
                    ATransAnalAttribRow row = (ATransAnalAttribRow)rv.Row;

                    if (!RequiredAnalAttrCodes.Contains(row.AnalysisTypeCode))
                    {
                        existingListIsOk = false;
                        break;
                    }
                }
            }

            if (existingListIsOk)
            {
                return;
            }

            // Delete any existing Analysis Type records and re-create the list (Removing any prior selections by the user).
            foreach (DataRowView rv in AGLBatchDS.ATransAnalAttrib.DefaultView)
            {
                ATransAnalAttribRow attrRowCurrent = (ATransAnalAttribRow)rv.Row;
                attrRowCurrent.Delete();
            }

            foreach (String analysisTypeCode in RequiredAnalAttrCodes)
            {
                ATransAnalAttribRow newRow = AGLBatchDS.ATransAnalAttrib.NewRowTyped(true);
                newRow.LedgerNumber = FLedgerNumber;
                newRow.BatchNumber = FBatchNumber;
                newRow.JournalNumber = FJournalNumber;
                newRow.TransactionNumber = ATransactionNumber;
                newRow.AnalysisTypeCode = analysisTypeCode;
                newRow.AccountCode = AAccountCode;

                AGLBatchDS.ATransAnalAttrib.Rows.Add(newRow);
            }
        }

        /// <summary>
        /// Used for the validation of Analysis Attributes
        /// </summary>
        /// <param name="ATransactionNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="AGLBatchDS"></param>
        /// <param name="AIsUnposted"></param>
        /// <returns></returns>
        public bool AccountAnalysisAttributeCountIsCorrect(int ATransactionNumber,
            string AAccountCode,
            GLBatchTDS AGLBatchDS,
            bool AIsUnposted = true)
        {
            bool RetVal = true;

            if (!AIsUnposted || string.IsNullOrEmpty(AAccountCode))
            {
                return RetVal;
            }

            int NumberOfAttributes = 0;

            TRemote.MFinance.Setup.WebConnectors.AccountHasAnalysisAttributes(FLedgerNumber, AAccountCode, out NumberOfAttributes, true);

            if (NumberOfAttributes == 0)
            {
                return RetVal;
            }

            DataView analAttrib = new DataView(AGLBatchDS.ATransAnalAttrib);

            analAttrib.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5} AND {6}={7}",
                ATransAnalAttribTable.GetBatchNumberDBName(),
                FBatchNumber,
                ATransAnalAttribTable.GetJournalNumberDBName(),
                FJournalNumber,
                ATransAnalAttribTable.GetTransactionNumberDBName(),
                ATransactionNumber,
                ATransAnalAttribTable.GetAccountCodeDBName(),
                AAccountCode);

            RetVal = (analAttrib.Count == NumberOfAttributes);

            return RetVal;
        }

        /// <summary>
        /// Used for the validation of Analysis Attributes
        /// </summary>
        /// <param name="ATransactionNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="AGLBatchDS"></param>
        /// <param name="ValueRequiredForType"></param>
        /// <param name="AIsUnposted"></param>
        /// <returns></returns>
        public bool AccountAnalysisAttributesValuesExist(int ATransactionNumber, string AAccountCode, GLBatchTDS AGLBatchDS,
            out String ValueRequiredForType, bool AIsUnposted = true)
        {
            ValueRequiredForType = "";

            if (!AIsUnposted || string.IsNullOrEmpty(AAccountCode) || (AGLBatchDS.ATransAnalAttrib.DefaultView.Count == 0))
            {
                return true;
            }

            StringCollection RequiredAnalAttrCodes = TRemote.MFinance.Setup.WebConnectors.RequiredAnalysisAttributesForAccount(FLedgerNumber,
                AAccountCode, true);

            string AnalysisCodeFilterValues = TAnalysisAttributes.ConvertStringCollectionToCSV(RequiredAnalAttrCodes, "'");

            DataView analAttrib = new DataView(AGLBatchDS.ATransAnalAttrib);

            analAttrib.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5} AND {6} IN ({7})",
                ATransAnalAttribTable.GetBatchNumberDBName(),
                FBatchNumber,
                ATransAnalAttribTable.GetJournalNumberDBName(),
                FJournalNumber,
                ATransAnalAttribTable.GetTransactionNumberDBName(),
                ATransactionNumber,
                ATransAnalAttribTable.GetAnalysisTypeCodeDBName(),
                AnalysisCodeFilterValues);

            foreach (DataRowView drv in analAttrib)
            {
                ATransAnalAttribRow rw = (ATransAnalAttribRow)drv.Row;

                string analysisCode = rw.AnalysisTypeCode;

                if (TRemote.MFinance.Setup.WebConnectors.AccountAnalysisAttributeRequiresValues(FLedgerNumber, analysisCode, true))
                {
                    if (rw.IsAnalysisAttributeValueNull() || (rw.AnalysisAttributeValue == string.Empty))
                    {
                        ValueRequiredForType = rw.AnalysisTypeCode;
                        return false;
                    }
                }
            }

            return true;
        }
    }
}