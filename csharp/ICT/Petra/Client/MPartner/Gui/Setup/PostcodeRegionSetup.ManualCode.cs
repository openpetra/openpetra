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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmPostcodeRegionSetup
    {
        private PPostcodeRegionRangeRow FPreviouslySelectedRangeRow = null;

        private void InitializeManualCode()
        {
            Type DataTableType;

            DataTable CacheRegionDT =
                TDataCache.GetCacheableDataTableFromCache("PostcodeRegionList", String.Empty, null, out DataTableType);
            DataTable CacheRegionRangeDT =
                TDataCache.GetCacheableDataTableFromCache("PostcodeRegionRangeList", String.Empty, null, out DataTableType);
            PPostcodeRangeTable RangeTable =
                (PPostcodeRangeTable)TDataCache.GetCacheableDataTableFromCache("PostcodeRangeList", String.Empty, null, out DataTableType);

            FMainDS.PPostcodeRegion.Merge(CacheRegionDT);
            FMainDS.PPostcodeRegionRange.Merge(CacheRegionRangeDT);

            foreach (PostcodeRegionsTDSPPostcodeRegionRangeRow Row in FMainDS.PPostcodeRegionRange.Rows)
            {
                PPostcodeRangeRow RangeRow = (PPostcodeRangeRow)RangeTable.Rows.Find(new object[] { Row.Range });
                Row.From = RangeRow.From;
                Row.To = RangeRow.To;
            }

            FMainDS.AcceptChanges();
        }

        private void ShowDetailsManual(PPostcodeRegionRow ARow)
        {
            grdRanges.Columns.Clear();
            grdRanges.AddTextColumn(Catalog.GetString("Range Name"), FMainDS.PPostcodeRegionRange.ColumnRange, 260);
            grdRanges.AddTextColumn(Catalog.GetString("From"), FMainDS.PPostcodeRegionRange.ColumnFrom, 140);
            grdRanges.AddTextColumn(Catalog.GetString("To"), FMainDS.PPostcodeRegionRange.ColumnTo, 140);
            grdRanges.Selection.EnableMultiSelection = true;

            DataView MyDataView = FMainDS.PPostcodeRegionRange.DefaultView;
            MyDataView.AllowNew = false;

            // do not apply these properties if the grid is empty
            if (ARow != null)
            {
                MyDataView.RowFilter = PPostcodeRegionRangeTable.GetRegionDBName() + " = " + "'" + ARow.Region + "'";
                MyDataView.Sort = "p_range_c ASC";

                btnAdd.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = false;
            }

            grdRanges.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);

            MyDataView = FMainDS.PPostcodeRegion.DefaultView;
            MyDataView.AllowNew = false;
            MyDataView.Sort = "p_region_c ASC";
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);

            btnRemove.Enabled = false;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPPostcodeRegion();
        }

        private void NewRowManual(ref PPostcodeRegionRow ARow)
        {
            string NewName = Catalog.GetString("NEWREGION");
            int CountNewDetail = 0;

            // increment new region's name if default name already exists
            if (FMainDS.PPostcodeRegion.Rows.Find(new object[] { NewName }) != null)
            {
                while (FMainDS.PPostcodeRegion.Rows.Find(new object[] { NewName + CountNewDetail.ToString() }) != null)
                {
                    CountNewDetail++;
                }

                NewName += CountNewDetail.ToString();
            }

            ARow.Region = NewName;
        }

        // Deal with change of focus on grdRanges
        private int FPrevRangeRowChangedRow = -1;
        private void FocusedRangeRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            DataRowView rowView = (DataRowView)grdRanges.Rows.IndexToDataSourceRow(e.Row);

            if (rowView != null)
            {
                FPreviouslySelectedRangeRow = (PPostcodeRegionRangeRow)(rowView.Row);
            }

            FPrevRangeRowChangedRow = e.Row;

            btnRemove.Enabled = true;
        }

        private bool DeleteRowManual(PPostcodeRegionRow ARowToDelete, ref String ACompletionMessage)
        {
            ACompletionMessage = String.Empty;

            FMainDS.PPostcodeRegionRange.DefaultView.Sort = PPostcodeRegionRangeTable.GetRegionDBName();
            DataRowView[] RangeRowsToDelete = FMainDS.PPostcodeRegionRange.DefaultView.FindRows(ARowToDelete.Region);

            foreach (DataRowView RangeRowToDelete in RangeRowsToDelete)
            {
                RangeRowToDelete.Row.Delete();
            }

            ARowToDelete.Delete();

            return true;
        }

        private void AddRangeRecord(Object sender, EventArgs e)
        {
            Form MainWindow = FPetraUtilsObject.GetCallerForm();

            String RegionName = ((PPostcodeRegionRow)GetSelectedDetailRow()).Region;

            String[] RangeName;
            String[] RangeFrom;
            String[] RangeTo;

            // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
            if (TCommonScreensForwarding.OpenRangeFindScreen != null)
            {
                // delegate IS defined
                try
                {
                    TCommonScreensForwarding.OpenRangeFindScreen.Invoke
                        (RegionName,
                        out RangeName,
                        out RangeFrom,
                        out RangeTo,
                        MainWindow);

                    if (RangeName == null)
                    {
                        return;
                    }

                    ShowDetailsManual(FPreviouslySelectedDetailRow);

                    for (int i = 0; i < RangeName.Length; i++)
                    {
                        if (RangeName[i] != null)
                        {
                            // check if this range already exists for region
                            Boolean RangeExists = false;

                            if (FMainDS.PPostcodeRegionRange.Rows.Find(new object[] { RegionName, RangeName[i] }) != null)
                            {
                                RangeExists = true;
                            }

                            if (!RangeExists && (RangeName[i].Length > 0))
                            {
                                PostcodeRegionsTDSPPostcodeRegionRangeRow NewRow = FMainDS.PPostcodeRegionRange.NewRowTyped(true);
                                NewRow.Region = RegionName;
                                NewRow.Range = RangeName[i];
                                NewRow.From = RangeFrom[i];
                                NewRow.To = RangeTo[i];
                                FMainDS.PPostcodeRegionRange.Rows.Add(NewRow);
                                FPetraUtilsObject.SetChangedFlag();
                            }
                            else if (RangeName[i].Length > 0)
                            {
                                string Message = string.Format(Catalog.GetString("The {0} range already exists for this region"), RangeName[i]);
                                MessageBox.Show(Message, Catalog.GetString(
                                        "Add Range"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    throw new ApplicationException("Exception occured while calling OpenRangeFindScreen Delegate!", exp);
                }
            }
        }

        // save data
        private TSubmitChangesResult StoreManualCode(ref PostcodeRegionsTDS ASubmitDS, out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult Result = TRemote.MPartner.Mailroom.WebConnectors.SavePostcodeRegionsTDS(ref ASubmitDS, out AVerificationResult);

            if (ASubmitDS.PPostcodeRegion != null)
            {
                ASubmitDS.PPostcodeRegion.AcceptChanges();
            }

            if (ASubmitDS.PPostcodeRegionRange != null)
            {
                ASubmitDS.PPostcodeRegionRange.AcceptChanges();
            }

            return Result;
        }

        private void DeleteRangeRecord(Object sender, EventArgs e)
        {
            DeletePPostcodeRegionRange();

            if (grdRanges.Rows.Count < 2)
            {
                btnRemove.Enabled = false;
            }
        }

        /// <summary>
        /// Standard method to delete the Data Row whose Details are currently displayed.
        /// There is full support for multi-row deletion.
        /// Optional manual code can be included to take action prior, during or after each deletion.
        /// When the row(s) have been deleted the highlighted row index stays the same unless the deleted row was the last one.
        /// The Details for the newly highlighted row are automatically displayed - or not, if the grid has now become empty.
        /// </summary>
        private void DeletePPostcodeRegionRange()
        {
            string CompletionMessage = String.Empty;

            if ((FPreviouslySelectedRangeRow == null) || (FPrevRangeRowChangedRow == -1))
            {
                return;
            }

            DataRowView[] HighlightedRows = grdRanges.SelectedDataRowsAsDataRowView;

            if (HighlightedRows.Length == 1)
            {
                TVerificationResultCollection VerificationResults = null;

                if (!FPetraUtilsObject.VerificationResultCollection.HasCriticalErrors)
                {
                    this.Cursor = Cursors.WaitCursor;
                    TRemote.MPartner.ReferenceCount.WebConnectors.GetNonCacheableRecordReferenceCount(
                        FMainDS.PPostcodeRegionRange,
                        DataUtilities.GetPKValuesFromDataRow(FPreviouslySelectedRangeRow),
                        out VerificationResults);
                    this.Cursor = Cursors.Default;
                }

                if ((VerificationResults != null)
                    && (VerificationResults.Count > 0))
                {
                    MessageBox.Show(Messages.BuildMessageFromVerificationResult(
                            Catalog.GetString("Record cannot be deleted!") +
                            Environment.NewLine +
                            Catalog.GetPluralString("Reason:", "Reasons:", VerificationResults.Count),
                            VerificationResults),
                        Catalog.GetString("Record Deletion"));
                    return;
                }

                string DeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");

                if ((FPrimaryKeyControl != null) && (FPrimaryKeyLabel != null))
                {
                    DeletionQuestion += String.Format("{0}{0}({1} {2})",
                        Environment.NewLine,
                        "Range Name:",
                        FPreviouslySelectedRangeRow.Range);
                }

                bool AllowDeletion = true;
                bool DeletionPerformed = false;

                if (AllowDeletion)
                {
                    if ((MessageBox.Show(DeletionQuestion,
                             Catalog.GetString("Confirm Delete"),
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question,
                             MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes))
                    {
                        try
                        {
                            FPreviouslySelectedRangeRow.Delete();
                            DeletionPerformed = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(String.Format(Catalog.GetString("An error occurred while deleting this record.{0}{0}{1}"),
                                    Environment.NewLine, ex.Message),
                                Catalog.GetString("Error"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }

                        if (DeletionPerformed)
                        {
                            FPetraUtilsObject.SetChangedFlag();
                        }

                        // Select and display the details of the nearest row to the one previously selected
                        grdRanges.SelectRowInGrid(FPrevRangeRowChangedRow, true);

                        // Clear any errors left over from  the deleted row
                        FPetraUtilsObject.VerificationResultCollection.Clear();
                    }
                }

                if (DeletionPerformed && (CompletionMessage.Length > 0))
                {
                    MessageBox.Show(CompletionMessage,
                        Catalog.GetString("Deletion Completed"));
                }
            }
            else
            {
                string DeletionQuestion = String.Format(Catalog.GetString(
                        "Do you want to delete the {0} highlighted rows?{1}{1}"), HighlightedRows.Length, Environment.NewLine);
                DeletionQuestion += Catalog.GetString("Each record will be checked to confirm that it can be deleted.");

                if (MessageBox.Show(DeletionQuestion,
                        Catalog.GetString("Confirm Delete"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                {
                    int recordsDeleted = 0;
                    int recordsUndeletable = 0;
                    int recordsDeleteDisallowed = 0;
                    List <TMultiDeleteResult>listConflicts = new List <TMultiDeleteResult>();
                    List <TMultiDeleteResult>listExceptions = new List <TMultiDeleteResult>();

                    this.Cursor = Cursors.WaitCursor;

                    foreach (DataRowView drv in HighlightedRows)
                    {
                        PPostcodeRegionRangeRow rowToDelete = (PPostcodeRegionRangeRow)(drv.Row);
                        string rowDetails = MakePKValuesStringManual(rowToDelete);

                        TVerificationResultCollection VerificationResults = null;
                        TRemote.MPartner.ReferenceCount.WebConnectors.GetNonCacheableRecordReferenceCount(
                            FMainDS.PPostcodeRegionRange,
                            DataUtilities.GetPKValuesFromDataRow(rowToDelete),
                            out VerificationResults);

                        if ((VerificationResults != null) && (VerificationResults.Count > 0))
                        {
                            TMultiDeleteResult result = new TMultiDeleteResult(rowDetails,
                                Messages.BuildMessageFromVerificationResult(String.Empty, VerificationResults));
                            listConflicts.Add(result);
                            continue;
                        }

                        bool AllowDeletion = true;
                        bool DeletionPerformed = false;

                        if (AllowDeletion)
                        {
                            try
                            {
                                rowToDelete.Delete();
                                DeletionPerformed = true;
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
                            FPetraUtilsObject.SetChangedFlag();
                            recordsDeleted++;
                        }
                    }

                    this.Cursor = Cursors.Default;

                    // Select and display the details of the nearest row to the one previously selected
                    grdRanges.SelectRowInGrid(FPrevRangeRowChangedRow, true);

                    if ((recordsDeleted > 0) && (CompletionMessage.Length > 0))
                    {
                        MessageBox.Show(CompletionMessage,
                            Catalog.GetString("Deletion Completed"));
                    }

                    //  Show the results of the multi-deletion
                    string results = null;

                    if (recordsDeleted > 0)
                    {
                        string s1 = Catalog.GetPluralString("record", "records", recordsDeleted);
                        string s2 = Catalog.GetPluralString("was", "were", recordsDeleted);
                        results = String.Format(Catalog.GetString("{0} {1} {2} successfully deleted."), recordsDeleted, s1, s2);
                    }
                    else
                    {
                        results = "No records were deleted.";
                    }

                    if (recordsUndeletable > 0)
                    {
                        string s1 = Catalog.GetPluralString("record", "records", recordsUndeletable);
                        string s2 = Catalog.GetPluralString("it is marked", "they are marked", recordsUndeletable);
                        results += String.Format(Catalog.GetString("{0}{1} {2} could not be deleted because {3} as non-deletable."),
                            Environment.NewLine,
                            recordsUndeletable,
                            s1, s2);
                    }

                    if (recordsDeleteDisallowed > 0)
                    {
                        string s1 = Catalog.GetPluralString("record was not be deleted", "records were not be deleted", recordsUndeletable);
                        results += String.Format(Catalog.GetString("{0}{1} {2} because deletion was not allowed."),
                            Environment.NewLine,
                            recordsDeleteDisallowed,
                            s1);
                    }

                    bool showCancel = false;

                    if (listConflicts.Count > 0)
                    {
                        showCancel = true;
                        string s1 = Catalog.GetPluralString("record", "records", listConflicts.Count);
                        string s2 = Catalog.GetPluralString("it is referenced", "they are referenced", listConflicts.Count);
                        results += String.Format(Catalog.GetString("{0}{1} {2} could not be deleted because {3} by at least one other table."),
                            Environment.NewLine,
                            listConflicts.Count,
                            s1, s2);
                    }

                    if (listExceptions.Count > 0)
                    {
                        showCancel = true;
                        string s1 = Catalog.GetPluralString("record", "records", listExceptions.Count);
                        results += String.Format(Catalog.GetString("{0}{1} {2} could not be deleted because the delete action failed unexpectedly."),
                            Environment.NewLine,
                            listExceptions.Count,
                            s1);
                    }

                    if (showCancel)
                    {
                        results +=
                            String.Format(Catalog.GetString("{0}{0}Click OK to review the details, or Cancel to return direct to the data screen"),
                                Environment.NewLine);

                        if (MessageBox.Show(results,
                                Catalog.GetString("Delete Action Summary"),
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                        {
                            ReviewMultiDeleteResults(listConflicts, Catalog.GetString("Rows in this table that are referenced by other tables"));
                            ReviewMultiDeleteResults(listExceptions, Catalog.GetString("Unexpected Exceptions"));
                        }
                    }
                    else
                    {
                        MessageBox.Show(results,
                            Catalog.GetString("Delete Action Summary"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
        }

        private string MakePKValuesStringManual(PPostcodeRegionRangeRow ARow)
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
    }
}