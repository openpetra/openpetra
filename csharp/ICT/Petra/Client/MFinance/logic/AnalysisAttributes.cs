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

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Exceptions;

using Ict.Petra.Client.App.Core.RemoteObjects;

using Ict.Petra.Shared.MFinance;
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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

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
            string CSVRetVal = string.Empty;

            int SizeCollection = AStringCollection.Count;

            if (SizeCollection > 0)
            {
                string[] allStrings = new string[SizeCollection];
                AStringCollection.CopyTo(allStrings, 0);

                CSVRetVal = AWrapString + String.Join(AWrapString + ", " + AWrapString, allStrings) + AWrapString;
            }

            return CSVRetVal;
        }

        /// <summary>
        /// Return selected row
        /// </summary>
        /// <param name="AGrid"></param>
        /// <returns></returns>
        public static ATransAnalAttribRow GetSelectedAttributeRow(TSgrdDataGrid AGrid)
        {
            #region Validate Arguments

            if (AGrid == null)
            {
                return null;
            }

            #endregion Validate Arguments

            DataRowView[] SelectedGridRow = AGrid.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                return (ATransAnalAttribRow)SelectedGridRow[0].Row;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Return selected row
        /// </summary>
        /// <param name="AGrid"></param>
        /// <returns></returns>
        public static ARecurringTransAnalAttribRow GetSelectedRecurringAttributeRow(TSgrdDataGrid AGrid)
        {
            #region Validate Arguments

            if (AGrid == null)
            {
                return null;
            }

            #endregion Validate Arguments

            DataRowView[] SelectedGridRow = AGrid.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                return (ARecurringTransAnalAttribRow)SelectedGridRow[0].Row;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AGLBatchDS"></param>
        /// <param name="ATransactionNumber"></param>
        /// <param name="AAnalysisCodeFilterValues"></param>
        public void SetTransAnalAttributeDefaultView(GLBatchTDS AGLBatchDS,
            Int32 ATransactionNumber = 0,
            String AAnalysisCodeFilterValues = "")
        {
            #region Validate Arguments

            if (AGLBatchDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AGLBatchDS.ATransAnalAttrib == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Transaction Analysis Attributes table is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ATransactionNumber < 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Transaction number is < 0!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            if (FBatchNumber != -1)
            {
                if (ATransactionNumber > 0)
                {
                    if (AAnalysisCodeFilterValues.Length == 0)
                    {
                        AGLBatchDS.ATransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                            ATransAnalAttribTable.GetBatchNumberDBName(),
                            FBatchNumber,
                            ATransAnalAttribTable.GetJournalNumberDBName(),
                            FJournalNumber,
                            ATransAnalAttribTable.GetTransactionNumberDBName(),
                            ATransactionNumber);
                    }
                    else
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
                }
                else
                {
                    if (AAnalysisCodeFilterValues.Length == 0)
                    {
                        AGLBatchDS.ATransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3}",
                            ATransAnalAttribTable.GetBatchNumberDBName(),
                            FBatchNumber,
                            ATransAnalAttribTable.GetJournalNumberDBName(),
                            FJournalNumber);
                    }
                    else
                    {
                        AGLBatchDS.ATransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4} IN ({5})",
                            ATransAnalAttribTable.GetBatchNumberDBName(),
                            FBatchNumber,
                            ATransAnalAttribTable.GetJournalNumberDBName(),
                            FJournalNumber,
                            ATransAnalAttribTable.GetAnalysisTypeCodeDBName(),
                            AAnalysisCodeFilterValues);
                    }
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
        /// <param name="AGLBatchDS"></param>
        /// <param name="ATransactionNumber"></param>
        /// <param name="AAnalysisCodeFilterValues"></param>
        public void SetRecurringTransAnalAttributeDefaultView(GLBatchTDS AGLBatchDS,
            Int32 ATransactionNumber = 0,
            String AAnalysisCodeFilterValues = "")
        {
            #region Validate Arguments

            if (AGLBatchDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AGLBatchDS.ARecurringTransAnalAttrib == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring Transaction Analysis Attributes table is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ATransactionNumber < 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Transaction number is < 0!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            if (FBatchNumber != -1)
            {
                if (ATransactionNumber > 0)
                {
                    if (AAnalysisCodeFilterValues.Length == 0)
                    {
                        AGLBatchDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                            ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                            FBatchNumber,
                            ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                            FJournalNumber,
                            ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                            ATransactionNumber);
                    }
                    else
                    {
                        AGLBatchDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5} AND {6} IN ({7})",
                            ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                            FBatchNumber,
                            ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                            FJournalNumber,
                            ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                            ATransactionNumber,
                            ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName(),
                            AAnalysisCodeFilterValues);
                    }
                }
                else
                {
                    if (AAnalysisCodeFilterValues.Length == 0)
                    {
                        AGLBatchDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3}",
                            ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                            FBatchNumber,
                            ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                            FJournalNumber);
                    }
                    else
                    {
                        AGLBatchDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4} IN ({5})",
                            ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                            FBatchNumber,
                            ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                            FJournalNumber,
                            ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName(),
                            AAnalysisCodeFilterValues);
                    }
                }

                AGLBatchDS.ARecurringTransAnalAttrib.DefaultView.Sort = String.Format("{0} ASC, {1} ASC",
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName()
                    );
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AAccountCode"></param>
        /// <param name="AAnalysisAttributeTbl"></param>
        /// <param name="AAnalysisCode"></param>
        /// <returns></returns>
        public bool AnalysisCodeIsActive(string AAccountCode, AAnalysisAttributeTable AAnalysisAttributeTbl, String AAnalysisCode = "")
        {
            #region Validate Arguments

            if ((AAccountCode == string.Empty) || (AAnalysisCode == string.Empty))
            {
                return true;
            }
            else if (AAnalysisAttributeTbl == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Analysis Attributes table is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            DataView AnalysisDV = new DataView(AAnalysisAttributeTbl);

            AnalysisDV.RowFilter = String.Format("{0}={1} AND {2}='{3}' AND {4}='{5}' AND {6}=true",
                AAnalysisAttributeTable.GetLedgerNumberDBName(),
                FLedgerNumber,
                AAnalysisAttributeTable.GetAccountCodeDBName(),
                AAccountCode,
                AAnalysisAttributeTable.GetAnalysisTypeCodeDBName(),
                AAnalysisCode,
                AAnalysisAttributeTable.GetActiveDBName());

            return AnalysisDV.Count > 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AGridCombo"></param>
        /// <param name="AFreeformAnalysisTbl"></param>
        /// <param name="AAnalysisCode"></param>
        /// <param name="AAnalysisAttributeValue"></param>
        /// <returns></returns>
        public static bool AnalysisAttributeValueIsActive(ref SourceGrid.Cells.Editors.ComboBox AGridCombo,
            AFreeformAnalysisTable AFreeformAnalysisTbl,
            String AAnalysisCode = "",
            String AAnalysisAttributeValue = "")
        {
            #region Validate Arguments

            if ((AAnalysisCode == string.Empty) || (AAnalysisAttributeValue == string.Empty))
            {
                return true;
            }
            else if (AFreeformAnalysisTbl == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Freeform Analysis table is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            //Make sure the grid combobox has right font else it will adopt strikeout
            // for all items in the list.
            AGridCombo.Control.Font = new Font(FontFamily.GenericSansSerif, 8);

            DataView dv = new DataView(AFreeformAnalysisTbl);

            dv.RowFilter = String.Format("{0}='{1}' AND {2}='{3}' AND {4}=true",
                AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                AAnalysisCode,
                AFreeformAnalysisTable.GetAnalysisValueDBName(),
                AAnalysisAttributeValue,
                AFreeformAnalysisTable.GetActiveDBName());

            return dv.Count > 0;
        }

        /// <summary>
        /// Need to ensure that the Analysis Attributes grid has all the entries
        /// that are required for the selected account.
        /// There may or may not already be attribute assignments for this transaction.
        /// </summary>
        /// <param name="AGLBatchDS"></param>
        /// <param name="AGLSetupDS">Can be null.  If supplied the code will use this data set to work out the required analysis attributes
        /// without a need to make a server call.  If not supplied the code will make a separate server call for each transaction row.  This may take several seconds.</param>
        /// <param name="ATransactionNumbers"></param>
        public void ReconcileTransAnalysisAttributes(GLBatchTDS AGLBatchDS, GLSetupTDS AGLSetupDS, out string ATransactionNumbers)
        {
            #region Validate Arguments

            if (AGLBatchDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AGLSetupDS == null)
            {
                AGLSetupDS = (GLSetupTDS)TRemote.MFinance.GL.WebConnectors.LoadAAnalysisAttributes(FLedgerNumber, false);
            }
            else if ((AGLSetupDS.AAnalysisAttribute == null) || (AGLSetupDS.AAnalysisAttribute.Count == 0))
            {
                AGLSetupDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadAAnalysisAttributes(FLedgerNumber, false));
            }

            #endregion Validate Arguments

            ATransactionNumbers = string.Empty;

            foreach (DataRowView drv in AGLBatchDS.ATransaction.DefaultView)
            {
                ATransactionRow tr = (ATransactionRow)drv.Row;

                if (TransAnalAttrRequiredUpdating(AGLBatchDS, AGLSetupDS, tr.AccountCode, tr.TransactionNumber))
                {
                    ATransactionNumbers += tr.TransactionNumber.ToString() + ", ";
                }
            }
        }

        /// <summary>
        /// Need to ensure that the Analysis Attributes grid has all the entries
        /// that are required for the selected account.
        /// There may or may not already be attribute assignments for this transaction.
        /// </summary>
        /// <param name="AGLBatchDS"></param>
        /// <param name="AGLSetupDS">Can be null.  If supplied the code will use this to discover the required attributes without making a trip to the server.
        /// Otherwise a server request is made.</param>
        /// <param name="AAccountCode"></param>
        /// <param name="ATransactionNumber"></param>
        /// <param name="ACheckBatchStatus"></param>
        /// <returns></returns>
        public bool TransAnalAttrRequiredUpdating(GLBatchTDS AGLBatchDS,
            GLSetupTDS AGLSetupDS,
            string AAccountCode,
            int ATransactionNumber,
            bool ACheckBatchStatus = true)
        {
            #region Validate Arguments

            if (AGLBatchDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACheckBatchStatus && ((AGLBatchDS.ABatch == null) || (AGLBatchDS.ABatch.Count == 0)))
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Batch table in the dataset was required but is null or empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AGLBatchDS.ATransAnalAttrib == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Transaction Analysis Attributes table in the dataset is null or empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AAccountCode.Length == 0)
            {
                return false;
            }
            else if (ATransactionNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Transaction number must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            try
            {
                if (ACheckBatchStatus)
                {
                    //Check Batch Status and return if not unposted
                    string batchFilter = String.Format("{0}={1}",
                        ABatchTable.GetBatchNumberDBName(),
                        FBatchNumber);

                    DataRow[] batchRows = AGLBatchDS.ABatch.Select(batchFilter);

                    #region Validate Data

                    if (batchRows.Length != 1)
                    {
                        throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                                    "Function:{0} - GL Batch number {1} does not exist in the table!"),
                                Utilities.GetMethodName(true),
                                FBatchNumber));
                    }

                    #endregion Validate Data

                    ABatchRow batchRow = (ABatchRow)batchRows[0];

                    if (batchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
                    {
                        return false;
                    }
                }

                //See what analysis attribute codes are required for the specified account code in this ledger
                StringCollection requiredAnalAttrCodes = new StringCollection();
                StringCollection currentAnalAttrCodes = new StringCollection();
                StringCollection analAttrCodesToDelete = new StringCollection();
                StringCollection analAttrCodesToAdd = new StringCollection();

                if ((AGLSetupDS != null) && (AGLSetupDS.AAnalysisAttribute != null))
                {
                    // This makes use of the supplied GLSetupTDS
                    AGLSetupDS.AAnalysisAttribute.DefaultView.RowFilter = String.Format("{0}='{1}' And {2}=true",
                        AAnalysisAttributeTable.GetAccountCodeDBName(),
                        AAccountCode,
                        AAnalysisAttributeTable.GetActiveDBName());

                    foreach (DataRowView drv in AGLSetupDS.AAnalysisAttribute.DefaultView)
                    {
                        requiredAnalAttrCodes.Add(drv.Row[AAnalysisAttributeTable.ColumnAnalysisTypeCodeId].ToString());
                    }
                }
                else
                {
                    //The server call is needed
                    requiredAnalAttrCodes = TRemote.MFinance.Setup.WebConnectors.RequiredAnalysisAttributesForAccount(FLedgerNumber,
                        AAccountCode,
                        true);
                }

                //Populate current codes and which ones to add or delete
                // (Check if loading required)
                SetTransAnalAttributeDefaultView(AGLBatchDS, ATransactionNumber);

                if (AGLBatchDS.ATransAnalAttrib.DefaultView.Count == 0)
                {
                    AGLBatchDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransAnalAttribForJournal(FLedgerNumber, FBatchNumber, FJournalNumber));
                }

                foreach (DataRowView drv in AGLBatchDS.ATransAnalAttrib.DefaultView)
                {
                    ATransAnalAttribRow transAnalAttrRow = (ATransAnalAttribRow)drv.Row;

                    //Populate the current codes string collection
                    string analAttrCode = transAnalAttrRow.AnalysisTypeCode;
                    currentAnalAttrCodes.Add(analAttrCode);

                    if ((requiredAnalAttrCodes.Count == 0) || !requiredAnalAttrCodes.Contains(analAttrCode))
                    {
                        //Populate the invalid codes string collection
                        analAttrCodesToDelete.Add(analAttrCode);
                    }
                }

                foreach (string analAttrCode in requiredAnalAttrCodes)
                {
                    if (!currentAnalAttrCodes.Contains(analAttrCode))
                    {
                        //Populate the needed codes string collection
                        analAttrCodesToAdd.Add(analAttrCode);
                    }
                }

                //count collection sizes
                int codesNumToAdd = analAttrCodesToAdd.Count;
                int codesNumToDelete = analAttrCodesToDelete.Count;

                //Nothing to add or take away
                if ((codesNumToAdd == 0) && (codesNumToDelete == 0))
                {
                    //No difference detected
                    return false;
                }

                //Delete invalid ones
                if (codesNumToDelete > 0)
                {
                    foreach (DataRowView drv in AGLBatchDS.ATransAnalAttrib.DefaultView)
                    {
                        ATransAnalAttribRow attrRowCurrent = (ATransAnalAttribRow)drv.Row;

                        if (analAttrCodesToDelete.Contains(attrRowCurrent.AnalysisTypeCode))
                        {
                            attrRowCurrent.Delete();
                        }
                    }
                }

                //Add missing ones
                if (codesNumToAdd > 0)
                {
                    foreach (string analysisTypeCode in analAttrCodesToAdd)
                    {
                        ATransAnalAttribRow newRow = AGLBatchDS.ATransAnalAttrib.NewRowTyped(true);
                        newRow.LedgerNumber = FLedgerNumber;
                        newRow.BatchNumber = FBatchNumber;
                        newRow.JournalNumber = FJournalNumber;
                        newRow.TransactionNumber = ATransactionNumber;
                        newRow.AnalysisTypeCode = analysisTypeCode;
                        newRow.AccountCode = AAccountCode;
                        newRow.AnalysisAttributeValue = string.Empty;

                        AGLBatchDS.ATransAnalAttrib.Rows.Add(newRow);
                    }
                }
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }

            return true;
        }

        /// <summary>
        /// Need to ensure that the Analysis Attributes grid has all the entries
        /// that are required for the selected account.
        /// There may or may not already be attribute assignments for this transaction.
        /// </summary>
        /// <param name="AGLBatchDS"></param>
        /// <param name="ATransactionNumbers"></param>
        public void ReconcileRecurringTransAnalysisAttributes(GLBatchTDS AGLBatchDS, out string ATransactionNumbers)
        {
            #region Validate Arguments

            if (AGLBatchDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            ATransactionNumbers = string.Empty;

            foreach (DataRowView drv in AGLBatchDS.ARecurringTransaction.DefaultView)
            {
                ARecurringTransactionRow tr = (ARecurringTransactionRow)drv.Row;

                if (RecurringTransAnalAttrRequiredUpdating(AGLBatchDS, tr.AccountCode, tr.TransactionNumber))
                {
                    ATransactionNumbers += tr.TransactionNumber.ToString() + ", ";
                }
            }
        }

        /// <summary>
        /// Need to ensure that the Analysis Attributes grid has all the entries
        /// that are required for the selected account.
        /// There may or may not already be attribute assignments for this transaction.
        /// </summary>
        /// <param name="AGLBatchDS"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ATransactionNumber"></param>
        /// <returns></returns>
        public bool RecurringTransAnalAttrRequiredUpdating(GLBatchTDS AGLBatchDS,
            string AAccountCode,
            int ATransactionNumber)
        {
            #region Validate Arguments

            if (AGLBatchDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if ((AGLBatchDS.ARecurringBatch == null) || (AGLBatchDS.ARecurringBatch.Count == 0))
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Batch table in the dataset is null or empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if ((AGLBatchDS.ARecurringTransaction == null) || (AGLBatchDS.ARecurringTransaction.Count == 0))
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Transaction table in the dataset is null or empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AGLBatchDS.ARecurringTransAnalAttrib == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Transaction Analysis Attributes table in the dataset is null or empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AAccountCode.Length == 0)
            {
                return false;
            }
            else if (ATransactionNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Transaction number must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            try
            {
                //See what analysis attribute codes are required for the specified account code in this ledger
                StringCollection requiredAnalAttrCodes = new StringCollection();
                StringCollection currentAnalAttrCodes = new StringCollection();
                StringCollection analAttrCodesToDelete = new StringCollection();
                StringCollection analAttrCodesToAdd = new StringCollection();

                //The server call is needed
                requiredAnalAttrCodes = TRemote.MFinance.Setup.WebConnectors.RequiredAnalysisAttributesForAccount(FLedgerNumber,
                    AAccountCode,
                    false);

                //Populate current codes and which ones to add or delete
                // (Check if loading required)
                SetRecurringTransAnalAttributeDefaultView(AGLBatchDS, ATransactionNumber);

                //First check if loading required
                if (AGLBatchDS.ARecurringTransAnalAttrib.DefaultView.Count == 0)
                {
                    AGLBatchDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransAnalAttribForJournal(FLedgerNumber, FBatchNumber,
                            FJournalNumber));
                }

                foreach (DataRowView drv in AGLBatchDS.ARecurringTransAnalAttrib.DefaultView)
                {
                    ARecurringTransAnalAttribRow transAnalAttrRow = (ARecurringTransAnalAttribRow)drv.Row;

                    string analAttrCode = transAnalAttrRow.AnalysisTypeCode;

                    //Populate the current codes string collection
                    currentAnalAttrCodes.Add(analAttrCode);

                    if ((requiredAnalAttrCodes.Count == 0) || !requiredAnalAttrCodes.Contains(analAttrCode))
                    {
                        //Populate the invalid codes string collection
                        analAttrCodesToDelete.Add(analAttrCode);
                    }
                }

                foreach (string analAttrCode in requiredAnalAttrCodes)
                {
                    if (!currentAnalAttrCodes.Contains(analAttrCode))
                    {
                        //Populate the needed codes string collection
                        analAttrCodesToAdd.Add(analAttrCode);
                    }
                }

                //count collection sizes
                int codesNumToAdd = analAttrCodesToAdd.Count;
                int codesNumToDelete = analAttrCodesToDelete.Count;

                //Nothing to add or take away
                if ((codesNumToAdd == 0) && (codesNumToDelete == 0))
                {
                    //No difference detected
                    return false;
                }

                //Delete invalid ones
                if (codesNumToDelete > 0)
                {
                    foreach (DataRowView drv in AGLBatchDS.ARecurringTransAnalAttrib.DefaultView)
                    {
                        ARecurringTransAnalAttribRow attrRowCurrent = (ARecurringTransAnalAttribRow)drv.Row;

                        if (analAttrCodesToDelete.Contains(attrRowCurrent.AnalysisTypeCode))
                        {
                            attrRowCurrent.Delete();
                        }
                    }
                }

                //Add missing ones
                if (codesNumToAdd > 0)
                {
                    //Access the transaction row
                    string transFilter = String.Format("{0}={1} And {2}={3} And {4}={5}",
                        ARecurringTransactionTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ARecurringTransactionTable.GetJournalNumberDBName(),
                        FJournalNumber,
                        ARecurringTransactionTable.GetTransactionNumberDBName(),
                        ATransactionNumber);

                    DataRow[] transRows = AGLBatchDS.ARecurringTransaction.Select(transFilter);

                    #region Validate Data

                    if (transRows.Length != 1)
                    {
                        throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(
                                Catalog.GetString(
                                    "Function:{0} - Transaction number {1} in Recurring GL Batch {2} and Journal {3} does not exist in the table!"),
                                Utilities.GetMethodName(true),
                                FBatchNumber,
                                FJournalNumber,
                                ATransactionNumber));
                    }

                    #endregion Validate Data

                    ARecurringTransactionRow transRow = (ARecurringTransactionRow)transRows[0];

                    foreach (string analysisTypeCode in analAttrCodesToAdd)
                    {
                        ARecurringTransAnalAttribRow newRow = AGLBatchDS.ARecurringTransAnalAttrib.NewRowTyped(true);
                        newRow.LedgerNumber = FLedgerNumber;
                        newRow.BatchNumber = FBatchNumber;
                        newRow.JournalNumber = FJournalNumber;
                        newRow.TransactionNumber = ATransactionNumber;
                        newRow.AnalysisTypeCode = analysisTypeCode;
                        newRow.AccountCode = AAccountCode;
                        newRow.CostCentreCode = transRow.CostCentreCode;
                        newRow.AnalysisAttributeValue = string.Empty;

                        AGLBatchDS.ARecurringTransAnalAttrib.Rows.Add(newRow);
                    }
                }
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }

            return true;
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
            int NumberOfAttributes = 0;

            if (!AIsUnposted || string.IsNullOrEmpty(AAccountCode))
            {
                return RetVal;
            }

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
        /// <returns></returns>
        public bool AccountRecurringAnalysisAttributeCountIsCorrect(int ATransactionNumber,
            string AAccountCode,
            GLBatchTDS AGLBatchDS)
        {
            bool RetVal = true;

            if (string.IsNullOrEmpty(AAccountCode))
            {
                return RetVal;
            }

            int NumberOfAttributes = 0;

            TRemote.MFinance.Setup.WebConnectors.AccountHasAnalysisAttributes(FLedgerNumber, AAccountCode, out NumberOfAttributes, false);

            if (NumberOfAttributes == 0)
            {
                return RetVal;
            }

            DataView analAttrib = new DataView(AGLBatchDS.ARecurringTransAnalAttrib);

            analAttrib.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5} AND {6}={7}",
                ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                FBatchNumber,
                ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                FJournalNumber,
                ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                ATransactionNumber,
                ARecurringTransAnalAttribTable.GetAccountCodeDBName(),
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
        /// <param name="AValueRequiredForType"></param>
        /// <param name="AIsUnposted"></param>
        /// <returns></returns>
        public bool AccountAnalysisAttributesValuesExist(int ATransactionNumber,
            string AAccountCode,
            GLBatchTDS AGLBatchDS,
            out String AValueRequiredForType,
            bool AIsUnposted = true)
        {
            AValueRequiredForType = "";

            if (!AIsUnposted || string.IsNullOrEmpty(AAccountCode) || (AGLBatchDS.ATransAnalAttrib.DefaultView.Count == 0))
            {
                return true;
            }

            StringCollection RequiredAnalAttrCodes = TRemote.MFinance.Setup.WebConnectors.RequiredAnalysisAttributesForAccount(FLedgerNumber,
                AAccountCode, true);

            if (RequiredAnalAttrCodes.Count == 0)
            {
                return true;
            }

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
                        AValueRequiredForType = rw.AnalysisTypeCode;
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Used for the validation of Analysis Attributes
        /// </summary>
        /// <param name="ATransactionNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="AGLBatchDS"></param>
        /// <param name="AValueRequiredForType"></param>
        /// <returns></returns>
        public bool AccountRecurringAnalysisAttributesValuesExist(int ATransactionNumber,
            string AAccountCode,
            GLBatchTDS AGLBatchDS,
            out String AValueRequiredForType)
        {
            AValueRequiredForType = "";

            if (string.IsNullOrEmpty(AAccountCode) || (AGLBatchDS.ARecurringTransAnalAttrib.DefaultView.Count == 0))
            {
                return true;
            }

            StringCollection RequiredAnalAttrCodes = TRemote.MFinance.Setup.WebConnectors.RequiredAnalysisAttributesForAccount(FLedgerNumber,
                AAccountCode, false);

            string AnalysisCodeFilterValues = TAnalysisAttributes.ConvertStringCollectionToCSV(RequiredAnalAttrCodes, "'");

            DataView analAttrib = new DataView(AGLBatchDS.ARecurringTransAnalAttrib);

            analAttrib.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5} AND {6} IN ({7})",
                ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                FBatchNumber,
                ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                FJournalNumber,
                ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                ATransactionNumber,
                ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName(),
                AnalysisCodeFilterValues);

            foreach (DataRowView drv in analAttrib)
            {
                ARecurringTransAnalAttribRow rw = (ARecurringTransAnalAttribRow)drv.Row;

                string analysisCode = rw.AnalysisTypeCode;

                if (TRemote.MFinance.Setup.WebConnectors.AccountAnalysisAttributeRequiresValues(FLedgerNumber, analysisCode, false))
                {
                    if (rw.IsAnalysisAttributeValueNull() || (rw.AnalysisAttributeValue == string.Empty))
                    {
                        AValueRequiredForType = rw.AnalysisTypeCode;
                        return false;
                    }
                }
            }

            return true;
        }
    }
}