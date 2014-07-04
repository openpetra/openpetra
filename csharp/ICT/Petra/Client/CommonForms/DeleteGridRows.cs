//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// A utility class whose job is to handle deletion of rows from a dataset that is associated with a grid
    /// The class supports single and multi-row deletion
    /// </summary>
    public class TDeleteGridRows
    {
        /// <summary>
        /// A helper class that stores information about the deletion result of a single row when deleting multiple rows
        /// </summary>
        private class TMultiDeleteResult
        {
            private string FRecordID;
            private string FResult;

            /// <summary>
            /// Constructor
            /// </summary>
            public TMultiDeleteResult(string ARecordID, string AResult)
            {
                FRecordID = ARecordID;
                FResult = AResult;
            }

            /// <summary>
            /// Get the recordID
            /// </summary>
            public string RecordID
            {
                get
                {
                    return FRecordID;
                }
            }

            /// <summary>
            /// Get the result string
            /// </summary>
            public string Result
            {
                get
                {
                    return FResult;
                }
            }
        }

        /// <summary>
        /// Shows message boxes for each result where a row was not deleted
        /// </summary>
        /// <param name="AList"></param>
        /// <param name="ATitle"></param>
        private static void ReviewMultiDeleteResults(List <TMultiDeleteResult>AList, string ATitle)
        {
            int allItemsCount = AList.Count;
            int item = 0;

            foreach (TMultiDeleteResult result in AList)
            {
                item++;
                string s1 = result.RecordID;
                string s2 = result.Result;

                string details = String.Format(MCommonResourcestrings.StrItemXofYRecordColon,
                    ATitle, item, allItemsCount, Environment.NewLine, s1, s2);

                if (item < allItemsCount)
                {
                    details += String.Format(MCommonResourcestrings.StrViewNextDetailOrCancel, Environment.NewLine);

                    if (MessageBox.Show(details, MCommonResourcestrings.StrMoreDetailsAboutRowsNotDeleted, MessageBoxButtons.OKCancel)
                        == System.Windows.Forms.DialogResult.Cancel)
                    {
                        break;
                    }
                }
                else
                {
                    MessageBox.Show(details, MCommonResourcestrings.StrMoreDetailsAboutRowsNotDeleted, MessageBoxButtons.OK);
                }
            }
        }

        /// <summary>
        /// Makes a comma separated string of primary key values in order to identify the row to the user
        /// </summary>
        private static string MakePKValuesString(DataRow ARow)
        {
            string ReturnValue = String.Empty;

            object[] items = DataUtilities.GetPKValuesFromDataRow(ARow);

            for (int i = 0; i < items.Length; i++)
            {
                if (i > 0)
                {
                    ReturnValue += ", ";
                }

                ReturnValue += items[i].ToString();
            }

            return ReturnValue;
        }

        /// <summary>
        /// The main method of this class.  It deletes one or more rows selected in a grid on the caller form
        /// </summary>
        /// <param name="ACallerFormOrControl">The form or user control that is making the call.  The form must implement the IDeleteGridRows interface</param>
        /// <param name="AGrid">A reference to the grid object</param>
        /// <param name="APetraUtilsObject">A reference to the PetraUtilsObject associated with the form or control making the call</param>
        /// <param name="AButtonPanel">A reference a form or control that implements the IButtonPanel interface.  This parameter can be null.</param>
        /// <returns>True if any rows were actually deleted</returns>
        public static bool DeleteRows(IDeleteGridRows ACallerFormOrControl,
            TSgrdDataGrid AGrid,
            TFrmPetraEditUtils APetraUtilsObject,
            IButtonPanel AButtonPanel)
        {
            DataRow currentDataRow = ACallerFormOrControl.GetSelectedDataRow();
            Int32 currentRowIndex = ACallerFormOrControl.GetSelectedRowIndex();

            if ((currentDataRow == null) || (currentRowIndex == -1))
            {
                return false;
            }

            string CompletionMessage = String.Empty;
            DataRowView[] HighlightedRows = AGrid.SelectedDataRowsAsDataRowView;

            if (HighlightedRows.Length == 1)
            {
                // Single row deletion

                TVerificationResultCollection VerificationResults = null;

                if (TVerificationHelper.IsNullOrOnlyNonCritical(APetraUtilsObject.VerificationResultCollection))
                {
                    ACallerFormOrControl.GetReferenceCount(currentDataRow, APetraUtilsObject.MaxReferenceCountOnDelete, out VerificationResults);
                }

                if ((VerificationResults != null)
                    && (VerificationResults.Count > 0))
                {
                    TCascadingReferenceCountHandler countHandler = new TCascadingReferenceCountHandler();
                    TFrmExtendedMessageBox.TResult result = countHandler.HandleReferences(APetraUtilsObject, VerificationResults, true);

                    if (result == TFrmExtendedMessageBox.TResult.embrYes)
                    {
                        // repeat the count but with no limit to the number of references
                        ACallerFormOrControl.GetReferenceCount(currentDataRow, 0, out VerificationResults);
                        countHandler.HandleReferences(APetraUtilsObject, VerificationResults, false);
                    }

                    return false;
                }

                string DeletionQuestion = ACallerFormOrControl.GetDefaultDeletionQuestion();
                bool AllowDeletion = true;
                bool DeletionPerformed = false;

                ACallerFormOrControl.HandlePreDelete(currentDataRow, ref AllowDeletion, ref DeletionQuestion);

                if (AllowDeletion)
                {
                    if ((MessageBox.Show(DeletionQuestion,
                             MCommonResourcestrings.StrConfirmDeleteTitle,
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question,
                             MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes))
                    {
                        try
                        {
                            if (!ACallerFormOrControl.HandleDeleteRow(currentDataRow, ref DeletionPerformed, ref CompletionMessage))
                            {
                                currentDataRow.Delete();
                                DeletionPerformed = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(String.Format(MCommonResourcestrings.StrErrorWhileDeleting,
                                    Environment.NewLine, ex.Message),
                                MCommonResourcestrings.StrGenericError,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }

                        if (DeletionPerformed)
                        {
                            APetraUtilsObject.SetChangedFlag();
                        }

                        // Select and display the details of the nearest row to the one previously selected
                        ACallerFormOrControl.SelectRowInGrid(currentRowIndex);
                        // Clear any errors left over from  the deleted row
                        APetraUtilsObject.VerificationResultCollection.Clear();

                        if (AButtonPanel != null)
                        {
                            AButtonPanel.UpdateRecordNumberDisplay();
                        }
                    }
                }

                if (!ACallerFormOrControl.HandlePostDelete(currentDataRow, AllowDeletion, DeletionPerformed, CompletionMessage))
                {
                    if (DeletionPerformed && (CompletionMessage.Length > 0))
                    {
                        MessageBox.Show(CompletionMessage, MCommonResourcestrings.StrDeletionCompletedTitle);
                    }
                }

                return DeletionPerformed;
            }
            else
            {
                // Multi-row deletion

                int recordsDeleted = 0;

                string DeletionQuestion = String.Format(MCommonResourcestrings.StrMultiRowDeletionQuestion,
                    HighlightedRows.Length,
                    Environment.NewLine);
                DeletionQuestion += MCommonResourcestrings.StrMultiRowDeletionCheck;

                if (MessageBox.Show(DeletionQuestion,
                        MCommonResourcestrings.StrConfirmDeleteTitle,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                {
                    int recordsUndeletable = 0;
                    int recordsDeleteDisallowed = 0;
                    List <TMultiDeleteResult>listConflicts = new List <TMultiDeleteResult>();
                    List <TMultiDeleteResult>listExceptions = new List <TMultiDeleteResult>();

                    APetraUtilsObject.GetForm().Cursor = Cursors.WaitCursor;

                    foreach (DataRowView drv in HighlightedRows)
                    {
                        DataRow rowToDelete = drv.Row;
                        string rowDetails = MakePKValuesString(rowToDelete);

                        if (!ACallerFormOrControl.IsRowDeletable(rowToDelete))
                        {
                            recordsUndeletable++;
                            continue;
                        }

                        TVerificationResultCollection VerificationResults = null;
                        ACallerFormOrControl.GetReferenceCount(currentDataRow, APetraUtilsObject.MaxReferenceCountOnDelete, out VerificationResults);

                        if ((VerificationResults != null) && (VerificationResults.Count > 0))
                        {
                            TMultiDeleteResult result = new TMultiDeleteResult(rowDetails,
                                Messages.BuildMessageFromVerificationResult(String.Empty, VerificationResults));
                            listConflicts.Add(result);
                            continue;
                        }

                        bool AllowDeletion = true;
                        bool DeletionPerformed = false;

                        ACallerFormOrControl.HandlePreDelete(rowToDelete, ref AllowDeletion, ref DeletionQuestion);

                        if (AllowDeletion)
                        {
                            try
                            {
                                if (!ACallerFormOrControl.HandleDeleteRow(rowToDelete, ref DeletionPerformed, ref CompletionMessage))
                                {
                                    rowToDelete.Delete();
                                    DeletionPerformed = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                TMultiDeleteResult result = new TMultiDeleteResult(rowDetails, ex.Message);
                                listExceptions.Add(result);
                            }
                        }
                        else
                        {
                            recordsDeleteDisallowed++;
                        }

                        if (DeletionPerformed)
                        {
                            APetraUtilsObject.SetChangedFlag();
                            recordsDeleted++;
                        }

                        ACallerFormOrControl.HandlePostDelete(rowToDelete, AllowDeletion, DeletionPerformed, String.Empty);
                    }

                    APetraUtilsObject.GetForm().Cursor = Cursors.Default;
                    ACallerFormOrControl.SelectRowInGrid(currentRowIndex);

                    if (AButtonPanel != null)
                    {
                        AButtonPanel.UpdateRecordNumberDisplay();
                    }

                    if ((recordsDeleted > 0) && (CompletionMessage.Length > 0))
                    {
                        MessageBox.Show(CompletionMessage, MCommonResourcestrings.StrDeletionCompletedTitle);
                    }

                    //  Show the results of the multi-deletion
                    string results = null;

                    if (recordsDeleted > 0)
                    {
                        results = String.Format(
                            Catalog.GetPluralString(MCommonResourcestrings.StrRecordSuccessfullyDeleted,
                                MCommonResourcestrings.StrRecordsSuccessfullyDeleted, recordsDeleted),
                            recordsDeleted);
                    }
                    else
                    {
                        results = MCommonResourcestrings.StrNoRecordsWereDeleted;
                    }

                    if (recordsUndeletable > 0)
                    {
                        results += String.Format(
                            Catalog.GetPluralString(MCommonResourcestrings.StrRowNotDeletedBecauseNonDeletable,
                                MCommonResourcestrings.StrRowsNotDeletedBecauseNonDeletable,
                                recordsUndeletable),
                            Environment.NewLine,
                            recordsUndeletable);
                    }

                    if (recordsDeleteDisallowed > 0)
                    {
                        results += String.Format(
                            Catalog.GetPluralString(MCommonResourcestrings.StrRowNotDeletedBecauseDeleteNotAllowed,
                                MCommonResourcestrings.StrRowsNotDeletedBecauseDeleteNotAllowed,
                                recordsDeleteDisallowed),
                            Environment.NewLine,
                            recordsDeleteDisallowed);
                    }

                    bool showCancel = false;

                    if (listConflicts.Count > 0)
                    {
                        showCancel = true;
                        results += String.Format(
                            Catalog.GetPluralString(MCommonResourcestrings.StrRowNotDeletedBecauseReferencedElsewhere,
                                MCommonResourcestrings.StrRowsNotDeletedBecauseReferencedElsewhere,
                                listConflicts.Count),
                            Environment.NewLine,
                            listConflicts.Count);
                    }

                    if (listExceptions.Count > 0)
                    {
                        showCancel = true;
                        results += String.Format(
                            Catalog.GetPluralString(MCommonResourcestrings.StrRowNotDeletedDueToUnexpectedException,
                                MCommonResourcestrings.StrRowNotDeletedDueToUnexpectedException,
                                listExceptions.Count),
                            Environment.NewLine,
                            listExceptions.Count);
                    }

                    if (showCancel)
                    {
                        results += String.Format(MCommonResourcestrings.StrClickToReviewDeletionOrCancel, Environment.NewLine);

                        if (MessageBox.Show(results,
                                MCommonResourcestrings.StrDeleteActionSummaryTitle,
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                        {
                            ReviewMultiDeleteResults(listConflicts, MCommonResourcestrings.StrRowsReferencedByOtherTables);
                            ReviewMultiDeleteResults(listExceptions, MCommonResourcestrings.StrExceptions);
                        }
                    }
                    else
                    {
                        MessageBox.Show(results,
                            MCommonResourcestrings.StrDeleteActionSummaryTitle,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }

                return recordsDeleted > 0;
            }
        }
    }


    /// <summary>
    /// The interface required by the TDeleteGridRows class in order to work with screens or user controls that support grid selection of rows to delete from a dataset
    /// </summary>
    public interface IDeleteGridRows : IGridBase
    {
        /// <summary>
        /// Specifies whether a row can be deleted from the data table.  Some tables have a column that indicates that the row is a non-deletable system row.
        /// </summary>
        /// <param name="ARowToDelete">The row to test</param>
        /// <returns>True if the row can be deleted</returns>
        bool IsRowDeletable(DataRow ARowToDelete);

        /// <summary>
        /// Gets the reference count for the specified row (the number of places that use the row as a foreign key)
        /// </summary>
        /// <param name="ADataRow">The row to test</param>
        /// <param name="AMaxCount">The counting will stop if the number of references reaches this value.  Use this to limit the time to execute this call.  Set to 0 for 'no limit'. </param>
        /// <param name="AVerificationResults">The results of the search</param>
        void GetReferenceCount(DataRow ADataRow, int AMaxCount, out TVerificationResultCollection AVerificationResults);

        /// <summary>
        /// Specifies the default question in the dialog that asks a user if deletion should proceeed.
        /// </summary>
        /// <returns>The question as a string</returns>
        string GetDefaultDeletionQuestion();

        /// <summary>
        /// Handle the optional manual code action prior to deletion.  The handler can be 'empty', because there is no default action.
        /// </summary>
        /// <param name="ARowToDelete">The row that is about to be deleted</param>
        /// <param name="AAllowDeletion">A boolean that, if set to false, will prevent the row from being deleted</param>
        /// <param name="ADeletionQuestion">The Deletion Question</param>
        void HandlePreDelete(DataRow ARowToDelete, ref bool AAllowDeletion, ref string ADeletionQuestion);

        /// <summary>
        /// Handle the optional manual code action associated with deletion.
        /// </summary>
        /// <param name="ARowToDelete">The row that is about to be deleted</param>
        /// <param name="ADeletionPerformed">A boolean that indicates whether a row was successfully deleted</param>
        /// <param name="ACompletionMessage">A message that will be displayed after deletion</param>
        /// <returns>Return True if manual code handled the deletion.  Return False to use the default processing for row deletion.</returns>
        bool HandleDeleteRow(DataRow ARowToDelete, ref bool ADeletionPerformed, ref string ACompletionMessage);

        /// <summary>
        /// Handle the optional manual code action after deletion.
        /// </summary>
        /// <param name="ARowToDelete">The row that has been deleted</param>
        /// <param name="AAllowDeletion">A boolean indicating if deletion was allowed/disallowed</param>
        /// <param name="ADeletionPerformed">A boolean indicating if deletion was or was not performed</param>
        /// <param name="ACompletionMessage">The completion message, if one was specified</param>
        /// <returns>Return True if manual code handled the post-deletion.  Return False to use the default processing for post-deletion.</returns>
        bool HandlePostDelete(DataRow ARowToDelete, bool AAllowDeletion, bool ADeletionPerformed, string ACompletionMessage);
    }
}