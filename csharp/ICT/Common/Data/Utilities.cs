//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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

            /* $IFDEF DEBUGMODE TLogging.Log('ChangeDataTableToTypedDataTable: DataTable ''' + DataTableName + ''': got added as a new DataTable to TmpDS. DataTable.TableName: ' + TmpDS.Tables[0].TableName + '; DataTable Type: ' +
             *TmpDS.Tables[0].GetType.FullName + '; Rows: ' + TmpDS.Tables[0].Rows.Count.ToString); $ENDIF */

            // Now merge in data from the untyped DataTable to the Typed DataTable!
            TmpDS.Merge(ADataTable);

            /* $IFDEF DEBUGMODE TLogging.Log('ChangeDataTableToTypedDataTable: DataTable ''' + DataTableName + ''': got merged into DataTable in TmpDS. Table count: ' + TmpDS.Tables.Count.ToString + '; DataTable Type: ' +
             *TmpDS.Tables[0].GetType.FullName + '; Rows: ' + TmpDS.Tables[0].Rows.Count.ToString); $ENDIF */

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

                    /* $IFDEF DEBUGMODE  if TSrvSetting.DL >= 7 then TLogging.Log('Length(AHashDV[RowCounter][ColumnCounter].ToString]): ' + TmpSize.ToString + ' (Value: ''' + AHashDV[RowCounter][ColumnCounter].ToString + ''')'); $ENDIF   if
                     *TSrvSetting.DL >= 9 then */
                }
            }

            /*
             * Calculate the hash of the string containing all values of all rows
             */
            HashingProvider = new SHA1CryptoServiceProvider();
            AHash = Convert.ToBase64String(HashingProvider.ComputeHash(Encoding.UTF8.GetBytes(HashStringBuilder.ToString())));

            // $IFDEF DEBUGMODE  if TSrvSetting.DL >= 7 then TLogging.Log('HashStringBuilder.ToString: ' + HashStringBuilder.ToString); $ENDIF  if TSrvSetting.DL >= 9 then
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
        /// copy all values from one row to the other; must have the same columns
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
                    ADestinationRow[ADestinationRow.Table.Columns[columnName].Ordinal] = ASourceRow[col];
                }
            }
        }

        /// <summary>
        /// copy all values from one row to the other; must have the same columns; omit Primary Key Columns
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
                    && (columnName != "s_modification_id_c")
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
    }
}