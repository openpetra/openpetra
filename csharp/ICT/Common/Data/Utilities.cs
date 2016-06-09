//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2012 by OM International
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
using System.Data.Odbc;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace Ict.Common.Data
{
    /// <summary>
    /// Contains Utility functions for ADO.NET Data operations.
    /// </summary>
    public class DataUtilities
    {
        /// <summary>
        /// Gets the 0-based index in the specified DataView by specifying an index in a Datatable
        /// </summary>
        /// <param name="ADataView">The DataView to search</param>
        /// <param name="ADataTable">The DataTable containing the known record</param>
        /// <param name="ARowNumberInTable">The 0-based row in the DataTable</param>
        /// <returns>0-based index in the DataView</returns>
        public static int GetDataViewIndexByDataTableIndex(DataView ADataView, DataTable ADataTable, int ARowNumberInTable)
        {
            Int32 RowNumberInView = -1;

            for (int Counter = 0; Counter < ADataView.Count; Counter++)
            {
                bool found = true;

                foreach (DataColumn myColumn in ADataTable.PrimaryKey)
                {
                    string value1 = ADataTable.Rows[ARowNumberInTable][myColumn].ToString();
                    string value2 = ADataView[Counter][myColumn.Ordinal].ToString();

                    if (value1 != value2)
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    RowNumberInView = Counter;
                    break;
                }
            }

            return RowNumberInView;
        }

        /// <summary>
        /// Gets the SQL friendly text for a date formatted as "yyyy-MM-dd"
        /// </summary>
        /// <param name="ADateTime">The DataView to search</param>
        /// <returns>string in format "yyy-MM-dd"</returns>
        public static string DateToSQLString(DateTime ? ADateTime)
        {
            if (ADateTime.HasValue)
            {
                return ADateTime.Value.ToString("yyyy-MM-dd");
            }

            return null;
        }

        /// <summary>
        /// Gets the 0-based index of a row in a DataTable by specifying a DataRowView
        /// </summary>
        /// <param name="ADataTable">The DataTable to search</param>
        /// <param name="ADataRowView">The DataRowView that is the DataRow to find</param>
        /// <returns>The 0-based index in the DataTable</returns>
        public static int GetDataTableIndexByDataRowView(DataTable ADataTable, DataRowView ADataRowView)
        {
            int RowNumberInData = -1;

            int dataRowIndex = 0;

            foreach (DataRow myRow in ADataTable.Rows)
            {
                bool found = true;

                foreach (DataColumn myColumn in ADataTable.PrimaryKey)
                {
                    if (myRow.RowState != DataRowState.Deleted)
                    {
                        string value1 = myRow[myColumn].ToString();
                        string value2 = ADataRowView[myColumn.Ordinal].ToString();

                        if (value1 != value2)
                        {
                            found = false;
                        }
                    }
                    else
                    {
                        found = false;
                    }
                }

                if (found)
                {
                    RowNumberInData = dataRowIndex;
                    break;
                }

                dataRowIndex++;
            }

            return RowNumberInData;
        }

        /// <summary>
        /// Remove the columns from the table that are not part of the template table
        /// </summary>
        /// <param name="ATable">table to be modified</param>
        /// <param name="ATableTemplate">template with requested columns</param>
        public static void RemoveColumnsNotInTableTemplate(DataTable ATable, DataTable ATableTemplate)
        {
            Int16 AllColumnsCounter;
            ArrayList DataColumnsToBeRemoved = new ArrayList();

            // Console.WriteLine('LocationTable.Columns.Count: ' + LocationTable.Columns.Count.ToString);

            for (AllColumnsCounter = 0; AllColumnsCounter <= ATable.Columns.Count - 1; AllColumnsCounter += 1)
            {
                // Console.WriteLine('Inspecting Column ''' + LocationTable.Columns[Counter].ColumnName + '''...');
                if (!ATableTemplate.Columns.Contains(ATable.Columns[AllColumnsCounter].ColumnName))
                {
                    // Console.WriteLine('Adding Column ''' + LocationTable.Columns[Counter].ColumnName + ''' to be removed...');
                    DataColumnsToBeRemoved.Add(ATable.Columns[ATable.Columns[AllColumnsCounter].ColumnName]);
                }

                // Console.WriteLine('Removing Column ''' + LocationTable.Columns[Counter].ColumnName + '''...');
                // LocationTable.Columns.Remove(LocationTable.Columns[Counter].ColumnName);
                // Console.WriteLine('Removed Column.');
            }

            foreach (object columnName in DataColumnsToBeRemoved)
            {
                // Console.WriteLine('Removing Column ''' + DataColumnsToBeRemoved[Counter2].ColumnName + '''...');
                ATable.Columns.Remove(columnName.ToString());

                // Console.WriteLine('Removed Column.');
            }

            DataColumnsToBeRemoved = null;
        }

        /// <summary>
        /// get an array of the values ordered by the columns in the destination table
        /// </summary>
        /// <param name="ADestinationRow">defines the desired order</param>
        /// <param name="ACopyRow">the values to be ordered</param>
        /// <returns>an array of ordered values</returns>
        public static Object[] DestinationSaveItemArray(DataRow ADestinationRow, DataRow ACopyRow)
        {
            DataColumnCollection DestinationTableColumns;
            Int32 Counter;

            DestinationTableColumns = ADestinationRow.Table.Columns;
            object[] Result = new object[DestinationTableColumns.Count];

            for (Counter = 0; Counter <= DestinationTableColumns.Count - 1; Counter += 1)
            {
                Result[Counter] = ACopyRow[DestinationTableColumns[Counter].ColumnName];
            }

            return Result;
        }

        /// <summary>
        /// convert a normal datatable to a typed datatable
        /// using reflection
        /// </summary>
        /// <param name="ADataTable">the table to be converted</param>
        /// <param name="ATypedDataTableType">the type of the typed datatable</param>
        /// <param name="ATableName">the name for the table</param>
        public static void ChangeDataTableToTypedDataTable(ref DataTable ADataTable, System.Type ATypedDataTableType, string ATableName)
        {
            String DataTableName;
            DataSet TmpDS;
            object TmpObj;

            TmpDS = new DataSet();

            // Handle optional Argument
            if (ATableName == "")
            {
                DataTableName = ADataTable.TableName;
            }
            else
            {
                DataTableName = ATableName;
            }

            // Create an Object of the Type that was passed in with Argument ATypedDataTableType
            TmpObj = Activator.CreateInstance(ATypedDataTableType,
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance),
                null,
                null,
                null);

            // Check if that Object derives from TTypedDataTable
            if (!(TmpObj.GetType().IsSubclassOf(typeof(TTypedDataTable))))
            {
                throw new ArgumentException(
                    "Type handed in in ATypedDataTableType must be a descendant of TTypedDataTable, but it is '" + TmpObj.GetType().FullName + "'");
            }

            TmpDS.Tables.Add((DataTable)TmpObj);
            TmpDS.Tables[0].TableName = DataTableName;

            // Now merge in data from the untyped DataTable to the Typed DataTable!
            TmpDS.Merge(ADataTable);

            // The result is a Typed DataTable of the desired Type, filled with data from
            // an untyped DataTable
            ADataTable = TmpDS.Tables[0];
            ((TTypedDataTable)ADataTable).InitVars();

            // Remove typed DataTable from TmpDS so that it can be added to another
            // DataSet by the caller (a DataTable can only belong to one DataSet at any
            // given time).
            TmpDS.Tables.RemoveAt(0);
        }

        /// <summary>
        /// calculate a hash for a table and the size of the table
        /// </summary>
        /// <param name="AHashDT">the table to be analysed</param>
        /// <param name="AHash">returns the hash value of the table</param>
        /// <param name="ASize">returns the size of the table</param>
        public static void CalculateHashAndSize(DataTable AHashDT, out String AHash, out Int32 ASize)
        {
            CalculateHashAndSize(AHashDT.DefaultView, out AHash, out ASize);
        }

        /// <summary>
        /// calculate a hash for a table and the size of the dataview
        /// </summary>
        /// <param name="AHashDV">the dataview to be analysed</param>
        /// <param name="AHash">returns the hash value of the table</param>
        /// <param name="ASize">returns the size of the table</param>
        public static void CalculateHashAndSize(DataView AHashDV, out String AHash, out Int32 ASize)
        {
            StringBuilder HashStringBuilder;
            SHA1CryptoServiceProvider HashingProvider;
            Int32 RowCounter;
            Int32 ColumnCounter;
            Int32 TmpSize = 0;

            ASize = 0;

            /*
             * Build a string that contains all values of all rows in the DataView, using
             * a StringBuilder for efficiency.
             */
            HashStringBuilder = new StringBuilder();

            for (RowCounter = 0; RowCounter <= AHashDV.Count - 1; RowCounter += 1)
            {
                for (ColumnCounter = 0; ColumnCounter <= AHashDV.Table.Columns.Count - 1; ColumnCounter += 1)
                {
                    if (AHashDV.Table.Columns[ColumnCounter].DataType != System.Type.GetType("System.DateTime"))
                    {
                        HashStringBuilder.Append(
                            RowCounter.ToString() + '/' + ColumnCounter.ToString() + ':' + AHashDV[RowCounter][ColumnCounter].ToString());
                    }
                    else
                    {
                        HashStringBuilder.Append(RowCounter.ToString() + '/' + ColumnCounter.ToString() + ':' +
                            TSaveConvert.DateColumnToDate(AHashDV.Table.Columns[ColumnCounter], AHashDV[RowCounter].Row).ToString("dd-MM-yy HH:MM"));
                    }

                    // Increment the size
                    TmpSize = AHashDV[RowCounter][ColumnCounter].ToString().Length;
                    ASize = ASize + TmpSize;
                }
            }

            /*
             * Calculate the hash of the string containing all values of all rows
             */
            HashingProvider = new SHA1CryptoServiceProvider();
            AHash = Convert.ToBase64String(HashingProvider.ComputeHash(Encoding.UTF8.GetBytes(HashStringBuilder.ToString())));
        }

        /// <summary>
        /// compare the values of two rows
        /// </summary>
        /// <param name="ADataRow1">first row</param>
        /// <param name="ADataRow2">second row</param>
        /// <returns>true if identical values</returns>
        public static Boolean HaveDataRowsIdenticalValues(DataRow ADataRow1, DataRow ADataRow2)
        {
            Boolean ReturnValue;
            int Counter;

            ReturnValue = true;
            object[] DR1ItemArray = ADataRow1.ItemArray;
            object[] DR2ItemArray = ADataRow2.ItemArray;

            if (DR1ItemArray.Length == DR2ItemArray.Length)
            {
                for (Counter = 0; Counter <= DR1ItemArray.Length - 1; Counter += 1)
                {
                    if (DR1ItemArray[Counter].ToString() != DR2ItemArray[Counter].ToString())
                    {
                        ReturnValue = false;
                        break;
                    }
                }
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// compare the values of two data rows expressed as object arrays
        /// </summary>
        /// <param name="ADataRow1">first row</param>
        /// <param name="ADataRow2">second row</param>
        /// <returns>true if identical values</returns>
        public static Boolean HaveDataRowsIdenticalValues(object[] ADataRow1, object[] ADataRow2)
        {
            // Check for matching number of columns
            if (ADataRow1.Length != ADataRow2.Length)
            {
                return false;
            }

            for (Int32 col = 0; col < ADataRow1.Length; col++)
            {
                // Column data must be of same type
                if (ADataRow1[col].GetType().ToString() != ADataRow2[col].GetType().ToString())
                {
                    return false;
                }

                // Column content must be the same
                if (!ADataRow1[col].Equals(ADataRow2[col]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// compare the changed columns of a row.
        /// for some reasons, on the client the values are read from the controls, and despite the row has not changed, the row is marked modified
        /// </summary>
        public static bool IsReallyChanged(DataRow ADataRow)
        {
            int DEBUGLEVEL_REALLYCHANGED = 1;

            if (ADataRow.RowState == DataRowState.Added)
            {
                if (TLogging.DebugLevel >= DEBUGLEVEL_REALLYCHANGED)
                {
                    TLogging.Log("Row has been added:");

                    foreach (DataColumn dc in ADataRow.Table.Columns)
                    {
                        TLogging.Log("  " + dc.ColumnName + ": " + ADataRow[dc.Ordinal].ToString());
                    }
                }

                return true;
            }
            else if (ADataRow.RowState == DataRowState.Deleted)
            {
                if (TLogging.DebugLevel >= DEBUGLEVEL_REALLYCHANGED)
                {
                    TLogging.Log("Row has been deleted:");

                    foreach (DataColumn dc in ADataRow.Table.Columns)
                    {
                        TLogging.Log("  " + dc.ColumnName + ": " + ADataRow[dc.Ordinal, DataRowVersion.Original].ToString());
                    }
                }

                return true;
            }
            else if (ADataRow.RowState == DataRowState.Modified)
            {
                if (TLogging.DebugLevel >= DEBUGLEVEL_REALLYCHANGED)
                {
                    TLogging.Log("Row has been modified:");
                }

                bool changed = false;

                foreach (DataColumn dc in ADataRow.Table.Columns)
                {
                    if (ADataRow[dc.Ordinal, DataRowVersion.Original].ToString() != ADataRow[dc.Ordinal, DataRowVersion.Current].ToString())
                    {
                        changed = true;

                        if (TLogging.DebugLevel >= DEBUGLEVEL_REALLYCHANGED)
                        {
                            TLogging.Log("***  " + dc.ColumnName + ": from " +
                                ADataRow[dc.Ordinal, DataRowVersion.Original].ToString() + " to " +
                                ADataRow[dc.Ordinal, DataRowVersion.Current].ToString());
                        }
                    }
                    else
                    {
                        if (TLogging.DebugLevel >= DEBUGLEVEL_REALLYCHANGED)
                        {
                            TLogging.Log("  " + dc.ColumnName + ": " + ADataRow[dc.Ordinal, DataRowVersion.Original].ToString());
                        }
                    }
                }

                return changed;
            }

            return false;
        }

        /// <summary>
        /// Check if non-system bound data columns have actually changed in the given
        ///  row. Only system fields begin 's_'
        /// </summary>
        /// <param name="ADataRow"></param>
        /// <returns></returns>
        public static Boolean DataRowColumnsHaveChanged(DataRow ADataRow)
        {
            bool ColumnValueHasChanged = false;

            if ((ADataRow.RowState != DataRowState.Unchanged)
                && (ADataRow.RowState != DataRowState.Deleted)
                && (ADataRow.RowState != DataRowState.Added))
            {
                string columnName = string.Empty;

                for (int i = 0; i < ADataRow.Table.Columns.Count; i++)
                {
                    columnName = ADataRow.Table.Columns[i].ColumnName;

                    //Ignore the system and temporary fields, all other fields are their SQL name, e.g. a_ledger_number_i
                    if (columnName.StartsWith("s_") || !columnName.Contains("_"))
                    {
                        continue;
                    }

                    string originalValue = ADataRow[i, DataRowVersion.Original].ToString();
                    string currentValue = ADataRow[i, DataRowVersion.Current].ToString();

                    if (originalValue != currentValue)
                    {
                        ColumnValueHasChanged = true;
                        break;
                    }
                }

                if (!ColumnValueHasChanged)
                {
                    ADataRow.RejectChanges();
                }
            }
            else if ((ADataRow.RowState == DataRowState.Deleted) || (ADataRow.RowState == DataRowState.Added))
            {
                ColumnValueHasChanged = true;
            }

            return ColumnValueHasChanged;
        }

        /// <summary>
        /// copy the modification ids from one datarow to another
        /// </summary>
        /// <param name="ADestinationDR">datarow to be modified</param>
        /// <param name="ACopyDR">original values</param>
        public static void CopyModificationIDsOver(DataRow ADestinationDR, DataRow ACopyDR)
        {
            if (ADestinationDR == null)
            {
                throw new System.ArgumentException("ADestinationDR must not be nil");
            }

            if (ACopyDR == null)
            {
                throw new System.ArgumentException("ACopyDR must not be nil");
            }

            if (!ADestinationDR.Table.Columns.Contains(TTypedDataAccess.MODIFICATION_ID))
            {
                throw new System.ArgumentException("ADestinationDR must contain a DataColumn named '" + TTypedDataAccess.MODIFICATION_ID + "'");
            }

            if (!ACopyDR.Table.Columns.Contains(TTypedDataAccess.MODIFICATION_ID))
            {
                throw new System.ArgumentException("ACopyDR must contain a DataColumn named '" + TTypedDataAccess.MODIFICATION_ID + "'");
            }

            // TLogging.Log('CopyModificationIDsOver: ACopyDR.Item[MODIFICATION_ID]: ''' + ACopyDR.Item[MODIFICATION_ID].ToString + '''');
            // TLogging.Log('CopyModificationIDsOver  before copying: ADestinationDR.Item[MODIFICATION_ID]: ''' + ADestinationDR.Item[MODIFICATION_ID].ToString + '''');
            ADestinationDR[TTypedDataAccess.MODIFICATION_ID] = ACopyDR[TTypedDataAccess.MODIFICATION_ID];

            // TLogging.Log('CopyModificationIDsOver  after copying: ADestinationDR.Item[MODIFICATION_ID]: ''' + ADestinationDR.Item[MODIFICATION_ID].ToString + '''');
            // Mark row as beeing changed
            ADestinationDR[2] = ADestinationDR[2].ToString() + ' ';
            ADestinationDR.AcceptChanges();
            ADestinationDR[2] = (ADestinationDR[2].ToString()).Substring(0, ADestinationDR[2].ToString().Length - 1);

            //I think Counter can only ever be 0 here
            //and nothing happens anyway
            //for (Counter = 0; Counter <= DestOrigItemArray.Length - 1; Counter += 1)
            //{
            //  if ((ADestinationDR[ADestinationDR.Table.Columns[Counter].ToString(),DataRowVersion.Current]) != (ADestinationDR[ADestinationDR.Table.Columns[Counter].ToString(),DataRowVersion.Original]))
            //  {
            //  }
            // TLogging.Log('CopyModificationIDsOver: Current and Original data is different in Column #' + Counter.ToString);
            //}
        }

        /// <summary>
        /// copy the modification ids from one datatable to another
        /// </summary>
        /// <param name="ADestinationDT">datatable to be modified</param>
        /// <param name="ACopyDT">original values</param>
        public static void CopyModificationIDsOver(DataTable ADestinationDT, DataTable ACopyDT)
        {
            int Counter;
            int Counter2;
            DataRow UpdateRow;

            if (ADestinationDT == null)
            {
                throw new System.ArgumentException("ADestinationDT must not be nil");
            }

            if (ACopyDT == null)
            {
                throw new System.ArgumentException("ACopyDT must not be nil");
            }

            DataColumn[] PrimaryKeyColumns = ADestinationDT.PrimaryKey;
            object[] PrimaryKeyObj = new object[PrimaryKeyColumns.Length];

            for (Counter = 0; Counter <= ADestinationDT.Rows.Count - 1; Counter += 1)
            {
                // TLogging.Log('CopyModificationIDsOver: working on Row #' + Counter.ToString);
                for (Counter2 = 0; Counter2 <= PrimaryKeyColumns.Length - 1; Counter2 += 1)
                {
                    PrimaryKeyObj[Counter2] = (object)ADestinationDT.Rows[Counter][PrimaryKeyColumns[Counter2]];

                    // TLogging.Log('CopyModificationIDsOver: working on Row #' + Counter.ToString + '.  PrimaryKeyObj[' + Counter2.ToString + ']: ' + PrimaryKeyObj[Counter2].ToString);
                }

                UpdateRow = ACopyDT.Rows.Find(PrimaryKeyObj);

                if (UpdateRow != null)
                {
                    // TLogging.Log('CopyModificationIDsOver: copying ModificationID over for Row #' + Counter.ToString);
                    CopyModificationIDsOver(ADestinationDT.Rows[Counter], UpdateRow);
                }
                else
                {
                }

                // TLogging.Log('CopyModificationIDsOver: Row #' + Counter.ToString + ': no matching Row in ADestinationDT found!');
            }
        }

        /// <summary>
        /// Copy all values from one row to the other.  Only copies columns with matching column names.
        /// </summary>
        /// <param name="ASourceRow"></param>
        /// <param name="ADestinationRow"></param>
        public static void CopyAllColumnValues(DataRow ASourceRow, DataRow ADestinationRow)
        {
            for (Int32 col = 0; col < ASourceRow.Table.Columns.Count; col++)
            {
                string columnName = ASourceRow.Table.Columns[col].ColumnName;

                if (ADestinationRow.Table.Columns.Contains(columnName))
                {
                    object data1 = ADestinationRow[ADestinationRow.Table.Columns[columnName].Ordinal];
                    object data2 = ASourceRow[col];

                    // Only copy if different - that way the destination row RowState can remain 'Unchanged'.
                    if (!data1.Equals(data2))
                    {
                        ADestinationRow[ADestinationRow.Table.Columns[columnName].Ordinal] = data2;
                    }
                }
            }
        }

        /// <summary>
        /// Copy all values from one row to the other; omit Primary Key Columns
        /// Only copies columns with matching column names
        /// </summary>
        /// <param name="ASourceRow"></param>
        /// <param name="ADestinationRow"></param>
        public static void CopyAllColumnValuesWithoutPK(DataRow ASourceRow, DataRow ADestinationRow)
        {
            for (Int32 col = 0; col < ASourceRow.Table.Columns.Count; col++)
            {
                Int32 pkIndex = 0;
                bool pk = false;

                while (pkIndex < ADestinationRow.Table.PrimaryKey.Length)
                {
                    if (col == ADestinationRow.Table.PrimaryKey[pkIndex].Ordinal)
                    {
                        pk = true;
                        break;
                    }

                    pkIndex++;
                }

                string columnName = ASourceRow.Table.Columns[col].ColumnName;

                if (!pk && ADestinationRow.Table.Columns.Contains(columnName))
                {
                    ADestinationRow[ADestinationRow.Table.Columns[columnName].Ordinal] = ASourceRow[col];
                }
            }
        }

        /// <summary>
        /// small structure for comparing 2 DataRows by columns, used by CompareAllColumnValues
        /// </summary>
        public struct TColumnDifference
        {
            /// <summary> the name of the column that is different </summary>
            public string FColumnName;
            /// <summary> the new value </summary>
            public string FSourceValue;
            /// <summary> the original value </summary>
            public string FDestinationValue;
            /// <summary> Constructor </summary>
            public TColumnDifference(string AColumnName, string ASourceName, string ADestinationValue)
            {
                FColumnName = AColumnName;
                FSourceValue = ASourceName;
                FDestinationValue = ADestinationValue;
            }
        }

        /// <summary>
        /// compare the values of two rows; must have the same columns
        /// </summary>
        /// <param name="ASourceRow"></param>
        /// <param name="ADestinationRow"></param>
        /// <param name="ADifferences"></param>
        public static void CompareAllColumnValues(DataRow ASourceRow, DataRow ADestinationRow, out List <TColumnDifference>ADifferences)
        {
            ADifferences = new List <TColumnDifference>();

            for (Int32 col = 0; col < ASourceRow.Table.Columns.Count; col++)
            {
                string columnName = ASourceRow.Table.Columns[col].ColumnName;

                if ((columnName != "s_created_by_c") && (columnName != "s_modified_by_c")
                    && (columnName != "s_date_created_d") && (columnName != "s_date_modified_d")
                    && (columnName != TTypedDataAccess.MODIFICATION_ID)
                    && ADestinationRow.Table.Columns.Contains(columnName))
                {
                    object SourceValue = ASourceRow[col];
                    object DestinationValue = ADestinationRow[ADestinationRow.Table.Columns[columnName].Ordinal];

                    if (!SourceValue.Equals(DestinationValue))
                    {
                        ADifferences.Add(new TColumnDifference(columnName, SourceValue.ToString(), DestinationValue.ToString()));
                    }
                }
            }
        }

        /// delete the destination table, copy all rows from the source table;
        /// we assume both tables have the same columns, same type.
        /// this is needed for datasets, when the table is readonly and cannot be assigned directly
        public static void CopyTo(DataTable ASrc, DataTable ADest)
        {
            ADest.Rows.Clear();

            foreach (DataRow r in ASrc.Rows)
            {
                ADest.ImportRow(r);
            }
        }

        /// <summary>
        /// make sure that unmodified rows are marked as accepted
        /// </summary>
        /// <param name="AInspectDT"></param>
        /// <param name="AMaxColumn"></param>
        /// <param name="AExcludeLocation0"></param>
        /// <returns></returns>
        public static Int32 AcceptChangesForUnmodifiedRows(DataTable AInspectDT, Int32 AMaxColumn, Boolean AExcludeLocation0)
        {
            Int32 ReturnValue;
            Int32 MaxColumn;
            Int16 Counter;
            Int16 Counter2;
            Int16 ChangedDRColumns;

            #region Process Arguments

            if (AInspectDT == null)
            {
                throw new ArgumentException("AInspectDT must not be nil");
            }

            if (AMaxColumn != -1)
            {
                MaxColumn = AMaxColumn;
            }
            else
            {
                MaxColumn = AInspectDT.Columns.Count;
            }

            #endregion
            ReturnValue = 0;

            for (Counter = 0; Counter <= AInspectDT.Rows.Count - 1; Counter += 1)
            {
                ChangedDRColumns = 0;

                if (((AInspectDT.Rows[Counter].RowState == DataRowState.Modified)
                     || (AInspectDT.Rows[Counter].RowState == DataRowState.Added))
                    && ((AExcludeLocation0) && (Convert.ToInt32(AInspectDT.Rows[Counter]["p_location_key_i"]) != 0)))
                {
                    for (Counter2 = 0; Counter2 <= MaxColumn - 1; Counter2 += 1)
                    {
                        if ((AInspectDT.Rows[Counter].RowState == DataRowState.Added)
                            || (AInspectDT.Rows[Counter][Counter2,
                                                         DataRowVersion.Original] != AInspectDT.Rows[Counter][Counter2, DataRowVersion.Current]))
                        {
                            ChangedDRColumns++;
                            ReturnValue++;
                        }
                    }

                    if (ChangedDRColumns == 0)
                    {
                        // Make DataRow unchanged since the Original values don't differ
                        // from the Current values for any of the DataColumns!
                        // MessageBox.Show('Calling AcceptChanges on Row ' + Counter.ToString + '; Contents of first 2 columns: ' +
                        // AInspectDT.Rows[Counter][0].ToString + '; ' + AInspectDT.Rows[Counter][1].ToString);
                        AInspectDT.Rows[Counter].AcceptChanges();
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// make sure that unmodified rows are marked as accepted
        /// </summary>
        /// <param name="AInspectDT"></param>
        /// <param name="AMaxColumn"></param>
        /// <returns></returns>
        public static Int32 AcceptChangesForUnmodifiedRows(DataTable AInspectDT, Int32 AMaxColumn)
        {
            return AcceptChangesForUnmodifiedRows(AInspectDT, AMaxColumn, false);
        }

        /// <summary>
        /// make sure that unmodified rows are marked as accepted
        /// </summary>
        /// <param name="AInspectDT"></param>
        /// <returns></returns>
        public static Int32 AcceptChangesForUnmodifiedRows(DataTable AInspectDT)
        {
            return AcceptChangesForUnmodifiedRows(AInspectDT, -1, false);
        }

        /// <summary>
        /// Removes any DataRow from the Destination DataTable that isn't found in the Source DataTable.
        /// </summary>
        /// <remarks><para><em>Both DataTables <em>must have the same Primary Key</em> for this Method to work!</em></para>
        /// <para>A possible use for this Method is the removing of DataRows from a DataTable that is held on
        /// the Client side when the DataTable is re-loaded from the DB and the reloaded DataTable may contains less rows.
        /// Performing a DataSet.Merge operation (DataTable reloaded from the DB merged into client-side DataSet)
        /// does not remove DataRows that don't exist in the DataTable that got reloaded from the DB,
        /// but calling this Method after the DataSet.Merge does that.</para></remarks>
        /// <param name="ASourceDT">Source DataTable.</param>
        /// <param name="ADestinationDT">Destination DataTable.</param>
        /// <param name="ADontAttemptToProcessDTsWithoutPKs">Set this to true to not attemt to process DataTables that don't have Primary Keys.
        /// If this Argument is set to true, no Exception is thrown and the Method simply doesn't do any work, as it would need Primary Keys for
        /// performing its work (default=false).</param>
        public static void RemoveRowsNotPresentInDT(DataTable ASourceDT, DataTable ADestinationDT, bool ADontAttemptToProcessDTsWithoutPKs = false)
        {
            DataRow FoundRow;

            DataColumn[] PrimaryKeyArr = ADestinationDT.PrimaryKey;

            if (ASourceDT.PrimaryKey.Length == 0)
            {
                if (ADontAttemptToProcessDTsWithoutPKs)
                {
                    return;
                }
                else
                {
                    throw new ArgumentException("DataTable specified with ASourceDT must have a Primary Key specified");
                }
            }

            if (PrimaryKeyArr.Length == 0)
            {
                if (ADontAttemptToProcessDTsWithoutPKs)
                {
                    return;
                }
                else
                {
                    throw new ArgumentException("DataTable specified with ADestinationDT must have a Primary Key specified");
                }
            }

            for (int Counter = ADestinationDT.Rows.Count - 1; Counter >= 0; Counter--) // Counting backwards so that I can delete rows as I go
            {
                FoundRow = ASourceDT.Rows.Find(GetPKValuesFromDataRow(ADestinationDT.Rows[Counter]));

                if (FoundRow == null)
                {
                    ADestinationDT.Rows.Remove(ADestinationDT.Rows[Counter]);
                }
            }
        }

        /// <summary>
        /// Returns the values of the DataColumns that make up the Primary Key of the DataTable
        /// that the passed in DataRow is part of.
        /// </summary>
        /// <param name="ADataRow">DataRow from which to return the values of the DataColumns that
        /// make up the Primary Key of the DataTable that this DataRow is part of.</param>
        /// <returns>The values of the DataColumns that make up the Primary Key of the DataTable
        /// that the passed in DataRow is part of.</returns>
        public static object[] GetPKValuesFromDataRow(DataRow ADataRow)
        {
            DataColumn[] PrimaryKeyArr = ADataRow.Table.PrimaryKey;
            object[] ReturnValue = new object[PrimaryKeyArr.Length];

            if (PrimaryKeyArr.Length == 0)
            {
                throw new ArgumentException("DataTable that holds the DataRow that is passed in Argument ADataRow must have a Primary Key specified");
            }

            for (int Counter = 0; Counter < PrimaryKeyArr.Length; Counter++)
            {
                if (ADataRow.RowState != DataRowState.Deleted)
                {
                    ReturnValue[Counter] = ADataRow[PrimaryKeyArr[Counter]];
                }
                else
                {
                    ReturnValue[Counter] = ADataRow[PrimaryKeyArr[Counter], DataRowVersion.Original];
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Changes a DataColumns' DataType to the desired DataType. This works even when the DataTable already holds data!
        /// </summary>
        /// <remarks>
        /// <para>The Method traverses the full DataRows Collection of the DataTable to copy data to the new DataType.
        /// This is likely not very efficient on huge DataTables!
        /// </para>
        /// <para>
        /// The Method utilises <see cref="System.Convert.ChangeType(object, Type)" /> for the conversion of the type. Only conversions
        /// that <see cref="System.Convert.ChangeType(object, Type)" /> supports will work. If
        /// <see cref="System.Convert.ChangeType(object, Type)" /> throws an Exception then the
        /// <see cref="ChangeDataColumnDataType" /> Method will return false.
        /// </para>
        /// </remarks>
        /// <param name="ATable">DataTable that holds the DataColumn whose DataType should be changed.</param>
        /// <param name="AColumnName">Name of the DataColumn whose DataType should be changed.</param>
        /// <param name="ANewType">Desired type that the DataColumn whose DataType should be changed should be changed to.</param>
        /// <returns>true if the change of DataType was successful or if the DataColumns' DataType was already the requested DataType, otherwise false.</returns>
        public static bool ChangeDataColumnDataType(DataTable ATable, string AColumnName, Type ANewType)
        {
            if (ATable.Columns.Contains(AColumnName) == false)
            {
                return false;
            }

            DataColumn ColumnToReplace = ATable.Columns[AColumnName];

            if (ColumnToReplace.DataType == ANewType)
            {
                return true;
            }

            try
            {
                DataColumn ColumnWithReplacedType = new DataColumn("Tmp", ANewType);

                ATable.Columns.Add(ColumnWithReplacedType);
                ColumnWithReplacedType.SetOrdinal(ATable.Columns.IndexOf(ColumnToReplace)); // Ensures that column is inserted at the same spot in the DataColumn Collection

                foreach (DataRow ConvertRow in ATable.Rows)
                {
                    ConvertRow["Tmp"] = Convert.ChangeType(ConvertRow[AColumnName], ANewType);
                }

                ATable.Columns.Remove(AColumnName);

                ColumnWithReplacedType.ColumnName = AColumnName;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Helper methods and functions.
    /// </summary>
    /// <typeparam name="T">A strongly type DataTable.
    /// A DataTable of type T will be returned from the DataSet.
    /// </typeparam>
    public static class DataSetAdapter <T>
    where T : DataTable, new ()
    {
        //To call this class's methods:
        //      DataSet employeesDataSet = OracleHelper.ExecuteDataset( connectionString, storedProcedure, parameters);
//		EmployeesDataTable employees = DataSetAdapter<EmployeesDataTable>.convert(employeesDataSet);

        /// <summary>
        /// Convert the first DataTable from a DataSet to a
        /// strongly-typed data table.
        /// </summary>
        public static T convert(DataSet dataSet)
        {
            if (dataSet == null)
            {
                return null;
            }

            if (dataSet.Tables.Count == 0)
            {
                return null;
            }

            DataTable dataTable = dataSet.Tables[0];
            return convert(dataTable);
        }

        /// <summary>
        /// Convert an ordinary DataTable to a strongly-typed
        /// data table.
        /// </summary>
        public static T convert(DataTable dataTable)
        {
            if (dataTable == null)
            {
                return null;
            }

            T stronglyTyped = new T();
            // add data from the regular DataTable to the
            // strongly typed DataTable.
            stronglyTyped.Merge(dataTable);
            return stronglyTyped;
        }
    }
}