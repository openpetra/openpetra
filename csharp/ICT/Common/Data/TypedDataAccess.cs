//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;

namespace Ict.Common.Data
{
    /// <summary>
    /// This is the base class for the data access store.
    /// It mainly contains static methods.
    /// </summary>
    public class TTypedDataAccess
    {
        private const string STR_INDENTATION = "    ";
        
        /// name of the column used for tracking changes, one ID per change
        public const string MODIFICATION_ID = "s_modification_id_t";

        /// <summary>
        /// who has last modified the row
        /// </summary>
        public const string MODIFIED_BY = "s_modified_by_c";

        /// <summary>
        /// when was the last change to the row
        /// </summary>
        public const string MODIFIED_DATE = "s_date_modified_d";

        /// <summary>
        /// who has originally created this row
        /// </summary>
        public const string CREATED_BY = "s_created_by_c";

        /// <summary>
        /// when was this row originally created
        /// </summary>
        public const string CREATED_DATE = "s_date_created_d";

        /// <summary>
        /// indicates whether a row has been deleted
        /// </summary>
        public static DateTime MODIFICATION_ID_DELETEDROW_INDICATOR = DateTime.MaxValue;

        /// <summary>
        /// the max for sqlite is 500. for postgresql it could be higher.
        /// </summary>
        private const int MAX_SQL_PARAMETERS = 450;

        private static int FRowCount;

        /// <summary>
        /// how many rows are in the current query or have been processed
        /// this is used for the Progress bar, to watch the current status
        /// </summary>
        public static Int32 RowCount
        {
            get
            {
                return FRowCount;
            }

            set
            {
                FRowCount = value;
            }
        }

        /// <summary>
        /// This function returns true if the column name is not one of the special columns, that
        /// are written automatically: modified date and by, created date and by, modification id
        ///
        /// </summary>
        /// <returns>true if this is no special auto column</returns>
        public static bool NoDefaultColumn(String AColumnName)
        {
            return (AColumnName != MODIFICATION_ID) && (AColumnName != MODIFIED_BY) && (AColumnName != MODIFIED_DATE)
                   && (AColumnName != CREATED_BY) && (AColumnName != CREATED_DATE);
        }

        /// <summary>
        /// This function returns the modification details of a row
        ///
        /// </summary>
        /// <returns>void</returns>
        public static void GetStoredModification(short ATableId,
            string[] AColumnNames,
            int[] APrimKeyColumnOrdList,
            DataRow ADataRow,
            DB.TDBTransaction ATransaction,
            out DateTime AModificationID,
            out String AModifiedBy,
            out System.DateTime AModifiedDate,
            bool ATreatRowAsAdded = false)
        {
            Int32 Counter;
            Boolean First;
            String SqlString;
            DataTable table;

            SqlString = "SELECT " + MODIFICATION_ID + ", " + MODIFIED_BY + ", " + MODIFIED_DATE +
                        " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                        " WHERE ";
            First = true;

            foreach (int PrimKeyOrd in APrimKeyColumnOrdList)
            {
                if (First == true)
                {
                    First = false;
                }
                else
                {
                    SqlString = SqlString + " AND ";
                }

                SqlString = SqlString + AColumnNames[PrimKeyOrd] + " = ?";
            }

            OdbcParameter[] Parameters = new OdbcParameter[APrimKeyColumnOrdList.Length];

            Counter = 0;

            DataRowVersion WhichVersion = DataRowVersion.Original;

            if ((ADataRow.RowState == DataRowState.Added)
                || (ATreatRowAsAdded))
            {
                WhichVersion = DataRowVersion.Current;
            }

            foreach (int i in APrimKeyColumnOrdList)
            {
                Parameters[Counter] = CreateOdbcParameter(ATableId, i, ADataRow[i, WhichVersion].GetType());
                Parameters[Counter].Value = ADataRow[i, WhichVersion];
                Counter = Counter + 1;
            }

            table = DBAccess.GDBAccessObj.SelectDT(SqlString, TTypedDataTable.GetTableNameSQL(ATableId), ATransaction, Parameters);
            AModificationID = DateTime.MinValue;
            AModifiedBy = "";
            AModifiedDate = DateTime.MinValue;

            if (table.Rows.Count == 1)
            {
                if (table.Rows[0][0] != System.DBNull.Value)
                {
                    AModificationID = Convert.ToDateTime(table.Rows[0][0]);
                }

                if (table.Rows[0][1] != System.DBNull.Value)
                {
                    AModifiedBy = (string)table.Rows[0][1];
                }

                if (table.Rows[0][2] != System.DBNull.Value)
                {
                    AModifiedDate = (DateTime)(table.Rows[0][2]);
                }
            }
            else if (table.Rows.Count == 0)
            {
                AModificationID = MODIFICATION_ID_DELETEDROW_INDICATOR;
            }
        }

        /// <summary>
        /// This function is called by the DataStore (SubmitChanges).
        /// it will not update the modification id.
        /// </summary>
        public static void InsertRow(
            short ATableId,
            ref DataRow ADataRow,
            DB.TDBTransaction ATransaction,
            String ACurrentUser,
            StringBuilder AInsertStatement,
            List <OdbcParameter>AInsertParameters)
        {
            string[] Columns = TTypedDataTable.GetColumnStringList(ATableId);
            string query = GenerateInsertClause("PUB_" +
                TTypedDataTable.GetTableNameSQL(ATableId),
                Columns,
                ADataRow,
                true);
            List <OdbcParameter>parameters =
                GetParametersForInsertClause(ATableId, ref ADataRow, Columns.Length, ATransaction, ACurrentUser, true);

            if (AInsertStatement.Length > 0)
            {
                AInsertStatement.Append(",");
                AInsertStatement.Append(query.Substring(query.IndexOf("VALUES (") + "VALUES".Length));
            }
            else
            {
                AInsertStatement.Append(query);
            }

            AInsertParameters.AddRange(parameters);
        }

        /// <summary>
        /// This function is called by the DataStore (SubmitChanges).
        /// it will update the modification id.
        /// </summary>
        public static void InsertRow(
            short ATableId,
            ref DataRow ADataRow,
            DB.TDBTransaction ATransaction,
            String ACurrentUser,
            bool ATreatRowAsAdded)
        {
            string[] Columns = TTypedDataTable.GetColumnStringList(ATableId);
            string query = GenerateInsertClause("PUB_" +
                TTypedDataTable.GetTableNameSQL(ATableId),
                Columns,
                ADataRow, false);
            List <OdbcParameter>parameters =
                GetParametersForInsertClause(ATableId, ref ADataRow, Columns.Length, ATransaction, ACurrentUser, false);

            if (0 == DBAccess.GDBAccessObj.ExecuteNonQuery(query, ATransaction, false, parameters.ToArray()))
            {
                throw new Exception("problems inserting a row");
            }

            DateTime LastModificationId;
            string LastModifiedBy;
            DateTime LastModifiedDate;
            int[] PrimKeyColumnOrdList = TTypedDataTable.GetPrimaryKeyColumnOrdList(ATableId);

            GetStoredModification(ATableId,
                Columns,
                PrimKeyColumnOrdList,
                ADataRow,
                ATransaction,
                out LastModificationId,
                out LastModifiedBy,
                out LastModifiedDate,
                ATreatRowAsAdded);

            ADataRow[MODIFICATION_ID] = LastModificationId;
        }

        /// <summary>
        /// This function is called by the DataStore (SubmitChanges)
        ///
        /// </summary>
        /// <returns>void</returns>
        public static void UpdateRow(
            short ATableId,
            bool AThrowAwayAfterSubmitChanges,
            ref DataRow ADataRow,
            DB.TDBTransaction ATransaction,
            String ACurrentUser)
        {
            string[] Columns = TTypedDataTable.GetColumnStringList(ATableId);
            int[] PrimKeyColumnOrdList = TTypedDataTable.GetPrimaryKeyColumnOrdList(ATableId);
            string DBTableName = TTypedDataTable.GetTableNameSQL(ATableId);
            DateTime LastModificationId;
            String LastModifiedBy;
            System.DateTime LastModifiedDate;

            // First try to update with a where clause with the modification id
            if (0 == DBAccess.GDBAccessObj.ExecuteNonQuery(GenerateUpdateClause("PUB_" + DBTableName,
                        Columns,
                        ADataRow,
                        PrimKeyColumnOrdList), ATransaction, false,
                    GetParametersForUpdateClause(ATableId, ref ADataRow, PrimKeyColumnOrdList, Columns.Length, ATransaction, ACurrentUser)))
            {
                // Was not able to update the row,
                // the database has a different modification id on that row.
                // trying now the other way

                DateTime OriginalModificationID;
                DateTime CurrentModificationID;

                // check if modification id of the changed row is the same as currently stored in the database
                GetStoredModification(ATableId,
                    Columns,
                    PrimKeyColumnOrdList,
                    ADataRow,
                    ATransaction,
                    out LastModificationId,
                    out LastModifiedBy,
                    out LastModifiedDate);

                if (LastModificationId != MODIFICATION_ID_DELETEDROW_INDICATOR)
                {
                    if ((ADataRow[MODIFICATION_ID,
                                  DataRowVersion.Original] == System.DBNull.Value) || (ADataRow[MODIFICATION_ID, DataRowVersion.Original] == null))
                    {
                        OriginalModificationID = DateTime.MinValue;
                    }
                    else
                    {
                        OriginalModificationID = Convert.ToDateTime(ADataRow[MODIFICATION_ID, DataRowVersion.Original]);
                    }

                    if ((ADataRow[MODIFICATION_ID,
                                  DataRowVersion.Current] == System.DBNull.Value) || (ADataRow[MODIFICATION_ID, DataRowVersion.Current] == null))
                    {
                        CurrentModificationID = DateTime.MinValue;
                    }
                    else
                    {
                        CurrentModificationID = Convert.ToDateTime(ADataRow[MODIFICATION_ID, DataRowVersion.Current]);
                    }

                    if (OriginalModificationID == LastModificationId)
                    {
                        if (CurrentModificationID != LastModificationId)
                        {
                            // the modification id has been increased in a previous SubmitChanges, but then the transaction has been rolled back
                            ADataRow[MODIFICATION_ID] = LastModificationId;
                            CurrentModificationID = LastModificationId;
                        }
                    }

                    // check if modification id has been changed and committed to the database, but AcceptChanges has not been applied
                    if (OriginalModificationID != CurrentModificationID)
                    {
                        throw new Exception(
                            "Developer should fix this: Forgot to call AcceptChanges on table " + DBTableName);
                    }

                    if (LastModificationId != OriginalModificationID)
                    {
                        throw new EDBConcurrencyException(
                            "Cannot update row of table " + DBTableName + " because the row has been edited by user " + LastModifiedBy,
                            "update",
                            DBTableName,
                            LastModifiedBy,
                            LastModifiedDate);
                    }

                    int RowsChanged = DBAccess.GDBAccessObj.ExecuteNonQuery(GenerateUpdateClause("PUB_" + DBTableName,
                            Columns,
                            ADataRow,
                            PrimKeyColumnOrdList), ATransaction, false,
                        GetParametersForUpdateClause(ATableId, ref ADataRow, PrimKeyColumnOrdList, Columns.Length, ATransaction, ACurrentUser));

                    if (RowsChanged == 0)
                    {
                        throw new Exception("cannot UPDATE row due to problems most likely with the timestamp");
                    }
                }
                else
                {
                    throw new EDBConcurrencyRowDeletedException("Cannot update row of table " + DBTableName + " because the row has been deleted.",
                        "update",
                        DBTableName,
                        "",
                        DateTime.MinValue);
                }
            }

            if (!AThrowAwayAfterSubmitChanges)
            {
                GetStoredModification(ATableId,
                    Columns,
                    PrimKeyColumnOrdList,
                    ADataRow,
                    ATransaction,
                    out LastModificationId,
                    out LastModifiedBy,
                    out LastModifiedDate);

                ADataRow[MODIFICATION_ID] = LastModificationId;
            }
        }

        /// <summary>
        /// This function is called by the DataStore (SubmitChanges)
        ///
        /// </summary>
        /// <returns>void</returns>
        public static void DeleteRow(
            short ATableId,
            DataRow ADataRow,
            DB.TDBTransaction ATransaction)
        {
            string[] Columns = TTypedDataTable.GetColumnStringList(ATableId);
            int[] PrimKeyColumnOrdList = TTypedDataTable.GetPrimaryKeyColumnOrdList(ATableId);
            string DBTableName = TTypedDataTable.GetTableNameSQL(ATableId);

            DateTime LastModificationId;
            String LastModifiedBy = "";
            System.DateTime LastModifiedDate;

            // check if modification id of the changed row is the same as currently stored in the database
            GetStoredModification(ATableId,
                Columns,
                PrimKeyColumnOrdList,
                ADataRow,
                ATransaction,
                out LastModificationId,
                out LastModifiedBy,
                out LastModifiedDate);

            object OriginalModificationIdObject = ADataRow[MODIFICATION_ID, DataRowVersion.Original];
            DateTime OriginalLastModificationID = DateTime.MinValue;

            if (OriginalModificationIdObject != System.DBNull.Value)
            {
                OriginalLastModificationID = Convert.ToDateTime(OriginalModificationIdObject);
            }

            if (LastModificationId != OriginalLastModificationID)
            {
                throw new EDBConcurrencyException(
                    "Cannot delete row of table " + DBTableName + " because the row has been edited by user " + LastModifiedBy,
                    "delete",
                    DBTableName,
                    LastModifiedBy,
                    LastModifiedDate);
            }

            DBAccess.GDBAccessObj.ExecuteNonQuery(GenerateDeleteClause("PUB_" + DBTableName,
                    Columns,
                    ADataRow,
                    PrimKeyColumnOrdList), ATransaction, false,
                GetParametersForDeleteClause(ATableId, ADataRow, PrimKeyColumnOrdList));
        }

        /// <summary>
        /// This function expects a StringCollection that can be filled, empty or nil.
        /// It also needs the list of the primary key columns, because otherwise the returned row will fail the constraints.
        /// It will return a proper Select Clause.
        /// </summary>
        /// <returns>the Select Clause
        /// </returns>
        public static String GenerateSelectClause(StringCollection AFieldList, short ATableID, bool AIncludeTableNames = false)
        {
            String ReturnValue = String.Empty;
            string Tablename = String.Empty;
            
            ReturnValue = "SELECT ";

            if ((AFieldList == null) || (AFieldList.Count == 0))
            {
                ReturnValue += '*';
            }
            else
            {
                if (AIncludeTableNames) 
                {
                    Tablename = TTypedDataTable.GetTableNameSQL(ATableID);
                    
                    foreach (var Field in AFieldList) 
                    {
                        ReturnValue += Tablename + "." + Field + ",";
                    }
                    
                    // strip off trailing ','
                    ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 1);                    
                }
                else
                {
                    ReturnValue += StringHelper.StrMerge(AFieldList, ',');  
                }
               

                string[] PrimKeyColumnStringList = TTypedDataTable.GetPrimaryKeyColumnStringList(ATableID);

                foreach (string primKeyColumn in PrimKeyColumnStringList)
                {
                    if ((!AFieldList.Contains(primKeyColumn)))
                    {
                        ReturnValue += ',';
                        
                        if (AIncludeTableNames) 
                        {
                            ReturnValue += Tablename + ".";
                        }

                        ReturnValue += primKeyColumn;
                    }
                }

                if ((!AFieldList.Contains(MODIFICATION_ID)))
                {
                        ReturnValue += ',';
                        
                        if (AIncludeTableNames) 
                        {
                            ReturnValue += Tablename + ".";
                        }

                        ReturnValue += MODIFICATION_ID;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function expects a StringCollection that can be filled, empty or nil.
        /// It also needs the list of the primary key columns, because otherwise the returned row will fail the constraints.
        /// It will prefix all fields with the given tablename. needed for selecting from join statements.
        /// It will return a proper Select Clause.
        /// If a field has already a dot in the name, the ATablename is not put before it: That way you can do SUM(PUB_table.p_something).
        /// </summary>
        /// <returns>the Select Clause
        /// </returns>
        public static String GenerateSelectClauseLong(String ATableName, StringCollection AFieldList, short ATableId)
        {
            String ReturnValue = "SELECT ";

            string[] PrimaryKeyFields = TTypedDataTable.GetPrimaryKeyColumnStringList(ATableId);

            if ((AFieldList == null) || (AFieldList.Count == 0))
            {
                ReturnValue = ReturnValue + ATableName + ".*";
            }
            else
            {
                foreach (string s in AFieldList)
                {
                    if (ReturnValue != "SELECT ")
                    {
                        ReturnValue = ReturnValue + ',';
                    }

                    if (s.IndexOf('.') == -1)
                    {
                        ReturnValue = ReturnValue + ATableName + '.' + s;
                    }
                    else
                    {
                        ReturnValue = ReturnValue + s;
                    }
                }

                foreach (string primkeyField in PrimaryKeyFields)
                {
                    if ((!AFieldList.Contains(primkeyField)))
                    {
                        if (primkeyField.IndexOf('.') == -1)
                        {
                            ReturnValue = ReturnValue + ',' + ATableName + '.' + primkeyField;
                        }
                        else
                        {
                            ReturnValue = ReturnValue + ',' + primkeyField;
                        }
                    }
                }

                if ((!AFieldList.Contains(MODIFICATION_ID)))
                {
                    ReturnValue = ReturnValue + ',' + ATableName + '.' + MODIFICATION_ID;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function expects an empty table that contains all existing columns,
        /// and a datarow that has a string or an empty value for each column.
        /// It will return a Where clause, using the given values.
        /// </summary>
        /// <param name="AColumnNames">the column names</param>
        /// <param name="ADataRow">the data row with the the columns for the where clause</param>
        /// <param name="ATemplateOperators">Every template field can have an operator; the default version always used = or LIKE</param>
        /// <returns>the Where Clause
        /// </returns>
        public static String GenerateWhereClause(string[] AColumnNames, DataRow ADataRow, StringCollection ATemplateOperators)
        {
            String ReturnValue = "";
            Int32 Counter;
            Int32 CounterOperator;

            ReturnValue = "";
            CounterOperator = 0;

            for (Counter = 0; Counter <= ADataRow.ItemArray.Length - 1; Counter += 1)
            {
                if (ADataRow[Counter] != System.DBNull.Value)
                {
                    if (ReturnValue.Length == 0)                     //first time around
                    {
                        ReturnValue = " WHERE ";
                    }
                    else
                    {
                        ReturnValue = ReturnValue + " AND ";
                    }

                    ReturnValue = ReturnValue + AColumnNames[Counter];

                    if ((ATemplateOperators == null) || (CounterOperator >= ATemplateOperators.Count))
                    {
                        if (ADataRow[Counter].GetType() == typeof(String))
                        {
                            ReturnValue = ReturnValue + " LIKE ";
                        }
                        else
                        {
                            ReturnValue = ReturnValue + " = ";
                        }
                    }
                    else
                    {
                        ReturnValue = ReturnValue + ' ' + ATemplateOperators[CounterOperator] + ' ';
                    }

                    ReturnValue = ReturnValue + '?';
                    CounterOperator = CounterOperator + 1;
                }
                else
                {
                    if ((ATemplateOperators != null) && (ATemplateOperators.Count == ADataRow.ItemArray.Length))
                    {
                        CounterOperator = CounterOperator + 1;
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function expects an empty table that contains all existing columns,
        /// and a datarow that has a string or an empty value for each column.
        /// It will return a Where clause, using the given values.
        /// </summary>
        /// <returns>the Where Clause
        /// </returns>
        public static String GenerateWhereClause(string[] AColumnNames, DataRow ADataRow)
        {
            return GenerateWhereClause(AColumnNames, ADataRow, null);
        }

        /// <summary>
        /// This function expects an empty table that contains all existing columns,
        /// and search criteria.
        /// It will return a Where clause, using the given values.
        /// </summary>
        /// <param name="AColumnNames">the column names</param>
        /// <param name="ASearchCriteria"></param>
        /// <param name="ATemplateOperators">Every template field can have an operator; the default version always used = or LIKE</param>
        /// <returns>the Where Clause
        /// </returns>
        public static String GenerateWhereClause(string[] AColumnNames, TSearchCriteria[] ASearchCriteria, StringCollection ATemplateOperators)
        {
            String ReturnValue = "";

            foreach (TSearchCriteria searchcriterium in ASearchCriteria)
            {
                if (ReturnValue.Length == 0)                     //first time around
                {
                    ReturnValue = " WHERE ";
                }
                else
                {
                    ReturnValue += " AND ";
                }

                ReturnValue += searchcriterium.fieldname + " " + searchcriterium.comparator + " ?";
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function expects an empty table that contains all existing columns,
        /// and a datarow that has a string or an empty value for each column.
        /// It will return a Where clause, using the given values.
        /// </summary>
        /// <returns>the Where Clause
        /// </returns>
        public static String GenerateWhereClause(string[] AColumnNames, TSearchCriteria[] ASearchCriteria)
        {
            return GenerateWhereClause(AColumnNames, ASearchCriteria, null);
        }

        /// <summary>
        /// this function generates a simple where clause with the given fieldnames
        /// </summary>
        /// <param name="AFieldNames"></param>
        /// <returns></returns>
        private static String GenerateWhereClause(string[] AFieldNames)
        {
            String ReturnValue = "";

            foreach (string columnname in AFieldNames)
            {
                if (ReturnValue.Length == 0)                     //first time around
                {
                    ReturnValue = " WHERE ";
                }
                else
                {
                    ReturnValue += " AND ";
                }

                ReturnValue += columnname + " = ?";
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function generates a where clause for a JOIN SQL Query
        /// </summary>
        /// <returns></returns>
        private static String GenerateWhereClauseForJoin(string ATableName, string AOtherTableName, string[] APKFieldNames, string[] AOtherPKFieldNames)
        {
            String ReturnValue = "";

            for (int Counter = 0; Counter < APKFieldNames.Length; Counter++) 
            {
                if (ReturnValue.Length == 0)                     //first time around
                {
                    ReturnValue = " WHERE ";
                }
                else
                {
                    ReturnValue += " AND ";
                }

                if (!APKFieldNames[Counter].StartsWith(AOtherTableName + ".")) 
                {
                    ReturnValue += AOtherTableName + ".";
                }
                
                ReturnValue += APKFieldNames[Counter] + " = ";    
                
                if (!AOtherPKFieldNames[Counter].StartsWith(ATableName + ".")) 
                {
                    ReturnValue += ATableName + ".";
                }
                
                ReturnValue += AOtherPKFieldNames[Counter];
            }

            return ReturnValue;
        }
        
        /// <summary>
        /// this function generates a where clause for the primary key of the given table
        /// </summary>
        /// <param name="ATableId"></param>
        /// <returns></returns>
        public static String GenerateWhereClauseFromPrimaryKey(short ATableId)
        {
            return GenerateWhereClause(TTypedDataTable.GetPrimaryKeyColumnStringList(ATableId));
        }

        /// <summary>
        /// this function generates a where clause for the unique key of the given table
        /// </summary>
        /// <param name="ATableId"></param>
        /// <returns></returns>
        public static String GenerateWhereClauseFromUniqueKey(short ATableId)
        {
            return GenerateWhereClause(TTypedDataTable.GetUniqueKeyColumnStringList(ATableId));
        }

        /// <summary>
        /// This function expects a string list of all existing columns,
        /// and a datarow that has a value or an empty value for each column.
        /// It will return a Where clause, using the given values.
        /// It does not contain the WHERE keyword.
        /// It uses the long form, table.fieldname
        ///
        /// </summary>
        /// <param name="ATableName">the table that the where clause is generated for</param>
        /// <param name="ATableId">id of the table to get the list of all columns of that table</param>
        /// <param name="ADataRow">contains the values for the where clause</param>
        /// <param name="ATemplateOperators">Every template field can have an operator; the default version always used = or LIKE</param>
        /// <returns>the Where Clause
        /// </returns>
        public static String GenerateWhereClauseLong(String ATableName, short ATableId, DataRow ADataRow, StringCollection ATemplateOperators)
        {
            String ReturnValue;
            Int32 Counter;
            Int32 CounterOperator;

            ReturnValue = "";
            Counter = 0;
            CounterOperator = 0;
            string[] ColumnNames = TTypedDataTable.GetColumnStringList(ATableId);

            while (Counter < ADataRow.ItemArray.Length)
            {
                if (ADataRow[Counter] != System.DBNull.Value)
                {
                    ReturnValue = ReturnValue + " AND ";
                    ReturnValue = ReturnValue + ATableName + '.' + ColumnNames[Counter];

                    if ((ATemplateOperators == null) || (CounterOperator >= ATemplateOperators.Count))
                    {
                        if (ADataRow[Counter].GetType() == typeof(String))
                        {
                            ReturnValue = ReturnValue + " LIKE ";
                        }
                        else
                        {
                            ReturnValue = ReturnValue + " = ";
                        }
                    }
                    else
                    {
                        ReturnValue = ReturnValue + ' ' + ATemplateOperators[CounterOperator] + ' ';
                    }

                    ReturnValue = ReturnValue + '?';
                    CounterOperator = CounterOperator + 1;
                }
                else
                {
                    if ((ATemplateOperators != null) && (ATemplateOperators.Count == ADataRow.ItemArray.Length))
                    {
                        CounterOperator = CounterOperator + 1;
                    }
                }

                Counter = Counter + 1;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function expects a string list of all existing columns,
        /// and a criteria list of values to search for.
        /// It will return a Where clause, using the given values.
        /// It does not contain the WHERE keyword.
        /// It uses the long form, table.fieldname
        ///
        /// </summary>
        /// <param name="ATableName">the table that the where clause is generated for</param>
        /// <param name="ASearchCriteria"></param>
        /// <returns>the Where Clause
        /// </returns>
        public static String GenerateWhereClauseLong(String ATableName, TSearchCriteria[] ASearchCriteria)
        {
            String ReturnValue = "";

            foreach (TSearchCriteria searchcriterium in ASearchCriteria)
            {
                ReturnValue += " AND " + ATableName + '.' + searchcriterium.fieldname + ' ' + searchcriterium.comparator + " ?";
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function expects a string array with the names of all existing columns,
        /// and a datarow that has a value or an empty value for each column.
        /// It will return a Where clause, using the given values.
        /// It does not contain the WHERE keyword.
        /// It uses the long form, table.fieldname
        ///
        /// </summary>
        /// <param name="ATableName">the table that the where clause is generated for</param>
        /// <param name="ATableId">id of the table to get the list of all columns of that table</param>
        /// <param name="ADataRow">contains the values for the where clause</param>
        /// <returns>the Where Clause
        /// </returns>
        public static String GenerateWhereClauseLong(String ATableName, short ATableId, DataRow ADataRow)
        {
            return GenerateWhereClauseLong(ATableName, ATableId, ADataRow, null);
        }

        /// <summary>
        /// This function expects a list of string, where the first string is either "order by" or "group by",
        /// and the rest of the string is e.g. "p_partner.p_partner_key_n ASC"
        /// It will return the appropriate part of the sql query
        /// </summary>
        /// <param name="AOrderBy">String list with instructions how to order or group the result</param>
        /// <returns>the order by / group by clause
        /// </returns>
        public static String GenerateOrderByClause(StringCollection AOrderBy)
        {
            String ReturnValue;
            Int32 Counter;

            ReturnValue = "";

            if (AOrderBy != null)
            {
                ReturnValue = ' ' + AOrderBy[0] + ' ';

                for (Counter = 1; Counter <= AOrderBy.Count - 1; Counter += 1)
                {
                    if (Counter > 1)
                    {
                        ReturnValue = ReturnValue + ',';
                    }

                    ReturnValue = ReturnValue + AOrderBy[Counter];
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function creates an odbc parameter with the correct type, based on the obj which comes from a DataRow
        ///
        /// </summary>
        /// <param name="ATableId">Table that we are operating on</param>
        /// <param name="AColumnNr">which column should the parameter be generated for</param>
        /// <param name="AType">the value that needs to be converted to OdbcParameter</param>
        /// <returns>the Odbc parameter</returns>
        public static OdbcParameter CreateOdbcParameter(short ATableId, Int32 AColumnNr, Type AType)
        {
            OdbcParameter ReturnValue;
            OdbcType newType;

            System.Int32 Length;
            Length = -1;
            newType = OdbcType.Int;

            if (AType == typeof(System.Int64))
            {
                newType = OdbcType.Decimal;
                Length = 10;
            }
            else if (AType == typeof(double))
            {
                newType = OdbcType.Decimal;
                Length = 24;
            }
            else if (AType == typeof(bool))
            {
                newType = OdbcType.Bit;
            }
            else if (AType == typeof(String))
            {
                newType = OdbcType.VarChar;

                // The documentation says:
                // \\ begin quote
                // The Length property returns the number of Char objects in this instance,
                // not the number of Unicode characters.
                // The reason is that a Unicode character might be represented by more than one Char.
                // Use the System.Globalization.StringInfo class to work with each Unicode character instead of each Char.
                // \\ end quote
                // I wonder if Mono behaves different than Windows?
                // I should find the maximum number for this field specifically;
                // multiplying by 2 is not safe with multibyte characters?
                // Length := (obj as System.String).Length  2;
                Length = TTypedDataTable.CreateOdbcParameter(ATableId, AColumnNr).Size;

                if (Length == 0)
                {
                    Length = 1;
                }
            }
            else if (AType == typeof(System.DateTime))
            {
                newType = OdbcType.Date;
            }
            else if (AType == typeof(System.Int32))
            {
                newType = OdbcType.Int;
            }
            else if (AType == typeof(System.Decimal))
            {
                // hardly ever used... p_person.p_believer_since_year_i number(4)
                newType = OdbcType.Decimal;
                Length = 24;
            }

            if (Length == -1)
            {
                ReturnValue = new OdbcParameter("", newType);
            }
            else
            {
                ReturnValue = new OdbcParameter("", newType, Length);
            }

            return ReturnValue;
        }

        /// <summary>
        /// create the odbc parameters for the the given key, with the actual values
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="AKeyColumns"></param>
        /// <param name="AKeyValues"></param>
        /// <returns></returns>
        private static OdbcParameter[] CreateOdbcParameterArrayFromKey(short ATableId, int[] AKeyColumns, System.Object[] AKeyValues)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[AKeyValues.Length];
            int counter = 0;

            foreach (System.Object obj in AKeyValues)
            {
                ParametersArray[counter] = CreateOdbcParameter(ATableId, AKeyColumns[counter], obj.GetType());
                ParametersArray[counter].Value = obj;
                counter++;
            }

            return ParametersArray;
        }

        /// <summary>
        /// create the odbc parameters for the primary key, with the actual values
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="APrimaryKeyValues"></param>
        /// <returns></returns>
        private static OdbcParameter[] CreateOdbcParameterArrayFromPrimaryKey(short ATableId, System.Object[] APrimaryKeyValues)
        {
            return CreateOdbcParameterArrayFromKey(ATableId, TTypedDataTable.GetPrimaryKeyColumnOrdList(ATableId), APrimaryKeyValues);
        }

        /// <summary>
        /// create the odbc parameters for the unique key, with the actual values
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="AUniqueKeyValues"></param>
        /// <returns></returns>
        private static OdbcParameter[] CreateOdbcParameterArrayFromUniqueKey(short ATableId, System.Object[] AUniqueKeyValues)
        {
            return CreateOdbcParameterArrayFromKey(ATableId, TTypedDataTable.GetUniqueKeyColumnOrdList(ATableId), AUniqueKeyValues);
        }

        /// <summary>
        /// This function returns either a valid object or System.DBNull;
        /// this is necessary because mono sometimes has a nil pointer in the DataRow for the original version etc.
        ///
        /// </summary>
        /// <returns>either a valid object or System.DBNull</returns>
        public static System.Object GetSafeValue(DataRow ADataRow, System.Int32 AColumnNr, DataRowVersion AVersion)
        {
            System.Object ReturnValue;

            // special treatment for mono: sometimes the value can be nil
            ReturnValue = System.DBNull.Value;

            if (ADataRow[AColumnNr, AVersion] != null)
            {
                ReturnValue = ADataRow[AColumnNr, AVersion];
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function returns either a valid object or System.DBNull;
        /// this is necessary because mono sometimes has a nil pointer in the DataRow for the original version etc.
        ///
        /// </summary>
        /// <returns>either a valid object or System.DBNull</returns>
        public static System.Object GetSafeValue(DataRow ADataRow, String AColumnName, DataRowVersion AVersion)
        {
            System.Object ReturnValue;

            // special treatment for mono: sometimes the value can be nil
            ReturnValue = System.DBNull.Value;

            if (ADataRow[AColumnName, AVersion] != null)
            {
                ReturnValue = ADataRow[AColumnName, AVersion];
            }

            return ReturnValue;
        }

        /// <summary>
        /// Compare the original and the current value of a column in a datarow.
        /// Make sure some mono specific troubles are worked around.
        ///
        /// </summary>
        /// <returns>result of comparison</returns>
        public static bool NotEquals(DataRow ADataRow, System.Int32 AColumnNr, ref System.Object ACurrentValue)
        {
            bool ReturnValue;

            System.Object CurrentValue;
            System.Object OriginalValue;
            decimal CurrentValueDecimal;
            decimal OriginalValueDecimal;

            // special treatment for mono: sometimes the value can be nil
            OriginalValue = GetSafeValue(ADataRow, AColumnNr, DataRowVersion.Original);
            CurrentValue = GetSafeValue(ADataRow, AColumnNr, DataRowVersion.Current);

            if ((CurrentValue == System.DBNull.Value) && (OriginalValue == System.DBNull.Value))
            {
                ReturnValue = false;
            }
            else if ((CurrentValue == System.DBNull.Value) || (OriginalValue == System.DBNull.Value))
            {
                ReturnValue = true;
            }
            // simple comparison of the System.Object objects does not work;
            // mono has trouble with comparing objects of different types (e.g. decimals and integers)
            // result := OriginalValue <> CurrentValue;
            else if (OriginalValue.GetType() != CurrentValue.GetType())
            {
                if ((OriginalValue.GetType() == typeof(System.Int32)) || (OriginalValue.GetType() == typeof(double))
                    || (OriginalValue.GetType() == typeof(System.Decimal)))
                {
                    // convert all to decimal
                    CurrentValueDecimal = Convert.ToDecimal(CurrentValue);
                    OriginalValueDecimal = Convert.ToDecimal(OriginalValue);
                    ReturnValue = OriginalValueDecimal != CurrentValueDecimal;
                }
                else
                {
                    // cannot guarantuee for correct result; probably a bug
                    throw new Exception(
                        "TTypedDataAccess.NotEquals: cannot compare types " + OriginalValue.GetType().ToString() + " and " +
                        CurrentValue.GetType().ToString());
                }
            }
            else
            {
                ReturnValue = (!OriginalValue.Equals(CurrentValue));
            }

            ACurrentValue = CurrentValue;
            return ReturnValue;
        }

        /// <summary>
        /// This function provides the actual parameters for the GenerateWhereClause
        /// uses only the values that are not DBNULL
        /// </summary>
        /// <returns>an array of OdbcParameters
        /// </returns>
        public static OdbcParameter[] GetParametersForWhereClause(short ATableId, DataRow ADataRow)
        {
            OdbcParameter[] ReturnValue;
            System.Int32 Counter;
            System.Int32 ColumnCounter;

            Counter = 0;

            // find out how many template values there are, to be able to create the array in one go
            foreach (object i in ADataRow.ItemArray)
            {
                if (i != System.DBNull.Value)
                {
                    Counter = Counter + 1;
                }
            }

            ReturnValue = new OdbcParameter[Counter];
            Counter = 0;
            ColumnCounter = 0;

            foreach (object item in ADataRow.ItemArray)
            {
                if (item != System.DBNull.Value)
                {
                    ReturnValue[Counter] = CreateOdbcParameter(ATableId, ColumnCounter, item.GetType());
                    ReturnValue[Counter].Value = item;
                    Counter = Counter + 1;
                }

                ColumnCounter = ColumnCounter + 1;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function provides the actual parameters for the GenerateWhereClause
        /// uses only the values that are not DBNULL
        /// </summary>
        /// <param name="ADataRow">values that are used as parameters</param>
        /// <param name="ATableId">identifier of the table to get the primary key from it</param>
        /// <returns>an array of OdbcParameters
        /// </returns>
        public static OdbcParameter[] GetParametersForWhereClauseWithPrimaryKey(short ATableId, DataRow ADataRow)
        {
            OdbcParameter[] ReturnValue;
            System.Int32 Counter = 0;
            System.Int32 i;
            System.Object CurrentValue = null;
            Boolean First = true;
            int[] PrimaryKeyColumnOrderList = TTypedDataTable.GetPrimaryKeyColumnOrdList(ATableId);

            // find out how many template values there are, to be able to create the array in one go
            for (i = 0; i <= ADataRow.ItemArray.Length - 1; i += 1)
            {
                if (NotEquals(ADataRow, i, ref CurrentValue))
                {
                    First = false;

                    if (CurrentValue != System.DBNull.Value)
                    {
                        Counter = Counter + 1;
                    }
                }
            }

            if (First == true)
            {
                // we need at least 1 column, otherwise the SQL statement will be invalid...
                Counter = 1;
            }

            if (PrimaryKeyColumnOrderList != null)
            {
                Counter = Counter + PrimaryKeyColumnOrderList.Length;
            }

            ReturnValue = new OdbcParameter[Counter];

            Counter = 0;
            First = true;

            for (i = 0; i <= ADataRow.ItemArray.Length - 1; i += 1)
            {
                if (NotEquals(ADataRow, i, ref CurrentValue))
                {
                    First = false;

                    if (CurrentValue != System.DBNull.Value)
                    {
                        ReturnValue[Counter] = CreateOdbcParameter(ATableId, i, CurrentValue.GetType());
                        ReturnValue[Counter].Value = CurrentValue;
                        Counter = Counter + 1;
                    }
                }
            }

            if (First == true)
            {
                // we need at least 1 column, otherwise the SQL statement will be invalid...
                CurrentValue = ADataRow.ItemArray[0];
                ReturnValue[Counter] = CreateOdbcParameter(ATableId, 0, CurrentValue.GetType());
                ReturnValue[Counter].Value = CurrentValue;
                Counter = 1;
            }

            // for the SELECT COUNT statement
            if (PrimaryKeyColumnOrderList != null)
            {
                foreach (int item in PrimaryKeyColumnOrderList)
                {
                    ReturnValue[Counter] = CreateOdbcParameter(ATableId, item, ADataRow[item].GetType());
                    ReturnValue[Counter].Value = ADataRow[item, DataRowVersion.Original];
                    Counter = Counter + 1;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function provides the actual parameters for the GenerateWhereClause
        /// </summary>
        /// <returns>an array of OdbcParameters
        /// </returns>
        public static OdbcParameter[] GetParametersForWhereClause(short ATableNumber, TSearchCriteria[] ASearchCriteria)
        {
            OdbcParameter[] ReturnValue = new OdbcParameter[ASearchCriteria.Length];
            int Counter = 0;

            foreach (TSearchCriteria searchcriterium in ASearchCriteria)
            {
                ReturnValue[Counter] = TTypedDataTable.CreateOdbcParameter(ATableNumber, searchcriterium);
                ReturnValue[Counter].Value = searchcriterium.searchvalue;
                Counter = Counter + 1;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function provides the actual parameters for the GenerateInsertClause
        /// uses only the values that are not DBNULL
        /// adds the next modification ID
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="ADataRow">values that are used as parameters</param>
        /// <param name="ANumberDBColumns">the number of columns of this row that should be stored in the database; that allows some columns to be added temporarily, e.g. PPartnerLocation.BestAddress in PartnerEdit Dataset</param>
        /// <param name="ATransaction">need a transaction for getting the next modification id</param>
        /// <param name="ACurrentUser">for setting modified by</param>
        /// <param name="AIncludeDefaultColumns">only useful when saving many rows in one query</param>
        /// <returns>an array of OdbcParameters
        /// </returns>
        public static List <OdbcParameter>GetParametersForInsertClause(
            short ATableId,
            ref DataRow ADataRow,
            Int32 ANumberDBColumns,
            DB.TDBTransaction ATransaction,
            String ACurrentUser,
            bool AIncludeDefaultColumns)
        {
            List <OdbcParameter>ReturnValue = new List <OdbcParameter>();
            OdbcParameter param;

            for (int i = 0; i <= ANumberDBColumns - 1; i += 1)
            {
                object item = ADataRow.ItemArray[i];

                if ((AIncludeDefaultColumns || (item != System.DBNull.Value)) && NoDefaultColumn(ADataRow.Table.Columns[i].ColumnName))
                {
                    if (item == System.DBNull.Value)
                    {
                        param = CreateOdbcParameter(ATableId, i, ADataRow.Table.Columns[i].DataType);
                        param.Value = item;
                        ReturnValue.Add(param);
                    }
                    else
                    {
                        param = CreateOdbcParameter(ATableId, i, item.GetType());
                        param.Value = item;
                        ReturnValue.Add(param);
                    }
                }
            }

            param = new OdbcParameter("", OdbcType.VarChar, 20);
            param.Value = ACurrentUser;
            ADataRow[CREATED_BY] = param.Value;
            ReturnValue.Add(param);
            param = new OdbcParameter("", OdbcType.Date);
            param.Value = DateTime.Now;
            ADataRow[CREATED_DATE] = param.Value;
            ReturnValue.Add(param);

            return ReturnValue;
        }

        /// <summary>
        /// This function provides the actual parameters for the GenerateUpdateClause
        /// uses only the values that are changed (comparing the current and the original version of the row)
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="ADataRow">values that are used as parameters</param>
        /// <param name="APrimaryKeyColumnOrd">can be empty; is needed for the UPDATE WHERE statement; it has the order numbers of the columns that make up the primary key</param>
        /// <param name="ANumberDBColumns">the number of columns of this row that should be stored in the database; that allows some columns to be added temporarily, e.g. PPartnerLocation.BestAddress in PartnerEdit Dataset</param>
        /// <param name="ATransaction">required for increasing the modification id</param>
        /// <param name="ACurrentUser">for setting modified by</param>
        /// <returns>an array of OdbcParameters
        /// </returns>
        public static OdbcParameter[] GetParametersForUpdateClause(
            short ATableId,
            ref DataRow ADataRow,
            int[] APrimaryKeyColumnOrd,
            Int32 ANumberDBColumns,
            DB.TDBTransaction ATransaction,
            String ACurrentUser)
        {
            List <OdbcParameter>ReturnValue = new List <OdbcParameter>();
            System.Object CurrentValue = null;
            OdbcParameter parameter;

            for (Int32 i = 0; i <= ANumberDBColumns - 1; i += 1)
            {
                if (NoDefaultColumn(ADataRow.Table.Columns[i].ColumnName))
                {
                    if (NotEquals(ADataRow, i, ref CurrentValue))
                    {
                        if (CurrentValue != System.DBNull.Value)
                        {
                            parameter = CreateOdbcParameter(ATableId, i, CurrentValue.GetType());
                            parameter.Value = CurrentValue;
                            ReturnValue.Add(parameter);
                        }
                    }
                }
            }

            parameter = new OdbcParameter("s_modified_by_c", OdbcType.VarChar, 20);
            parameter.Value = ACurrentUser;
            ADataRow[MODIFIED_BY] = parameter.Value;
            ReturnValue.Add(parameter);
            parameter = new OdbcParameter("s_date_modified_d", OdbcType.Date);
            parameter.Value = DateTime.Now;
            ADataRow[MODIFIED_DATE] = parameter.Value;
            ReturnValue.Add(parameter);

            // for the UPDATE WHERE statement
            if (APrimaryKeyColumnOrd != null)
            {
                foreach (int pki in APrimaryKeyColumnOrd)
                {
                    parameter = CreateOdbcParameter(ATableId, pki, ADataRow[pki].GetType());
                    parameter.Value = ADataRow[pki, DataRowVersion.Original];
                    ReturnValue.Add(parameter);
                }
            }

            // modification id
            if (!((ADataRow.IsNull(MODIFICATION_ID) || (Convert.ToDateTime(ADataRow[MODIFICATION_ID]) == DateTime.MinValue))))
            {
                // modification id for the where clause
                parameter = new OdbcParameter("s_modification_id_t", OdbcType.DateTime);
                parameter.Value = Convert.ToDateTime(ADataRow[MODIFICATION_ID]);
                ReturnValue.Add(parameter);
            }

            return ReturnValue.ToArray();
        }

        /// <summary>
        /// This function provides the actual parameters for the GenerateDeleteClause
        /// it uses the OriginalVersion of the primary key values
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="ADataRow">values that are used as parameters</param>
        /// <param name="APrimaryKeyColumnOrd">has the order numbers of the columns that make up the primary key</param>
        /// <returns>an array of OdbcParameters
        /// </returns>
        public static OdbcParameter[] GetParametersForDeleteClause(
            short ATableId, DataRow ADataRow, int[] APrimaryKeyColumnOrd)
        {
            OdbcParameter[] ReturnValue;
            System.Int32 Counter;
            ReturnValue = new OdbcParameter[APrimaryKeyColumnOrd.Length];

            Counter = 0;

            foreach (int i in APrimaryKeyColumnOrd)
            {
                ReturnValue[Counter] = CreateOdbcParameter(ATableId, i, ADataRow[i, DataRowVersion.Original].GetType());
                ReturnValue[Counter].Value = ADataRow[i, DataRowVersion.Original];
                Counter = Counter + 1;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function expects an empty table that contains all existing columns,
        /// and a datarow that has a value or an empty value for each column.
        /// It will return a complete INSERT Clause.
        ///
        /// </summary>
        /// <returns>the INSERT statement
        /// </returns>
        public static String GenerateInsertClause(String ATableName, string[] AColumnNames, DataRow ADataRow, bool AIncludeDefaultColumns)
        {
            String ReturnValue = "INSERT INTO " + ATableName + " (";
            Boolean First = true;

            // should ignore additional fields at the end of the row
            for (Int32 Counter = 0; Counter <= AColumnNames.Length - 1; Counter += 1)
            {
                if ((AIncludeDefaultColumns || (ADataRow[Counter] != System.DBNull.Value)) && NoDefaultColumn(AColumnNames[Counter]))
                {
                    if ((!First))
                    {
                        ReturnValue = ReturnValue + ", ";
                    }
                    else
                    {
                        First = false;
                    }

                    ReturnValue = ReturnValue + AColumnNames[Counter];
                }
            }

            ReturnValue = ReturnValue + ", " + MODIFICATION_ID + ", " + CREATED_BY + ", " + CREATED_DATE;
            ReturnValue = ReturnValue + ") VALUES (";
            First = true;

            for (Int32 Counter = 0; Counter <= AColumnNames.Length - 1; Counter += 1)
            {
                if ((AIncludeDefaultColumns || (ADataRow[Counter] != System.DBNull.Value)) && NoDefaultColumn(AColumnNames[Counter]))
                {
                    if ((!First))
                    {
                        ReturnValue = ReturnValue + ", ";
                    }
                    else
                    {
                        First = false;
                    }

                    ReturnValue = ReturnValue + '?';
                }
            }

            if ((!First))
            {
                ReturnValue = ReturnValue + ", ";
            }

            // add modification id, created by, date created
            ReturnValue = ReturnValue + "NOW(), ?, ?";
            return ReturnValue + ")";
        }

        /// <summary>
        /// This function expects an empty table that contains all existing columns,
        /// and a datarow that has a value or an empty value for each column.
        ///
        /// </summary>
        /// <returns>the UPDATE statement
        /// </returns>
        public static String GenerateUpdateClause(String ATableName, string[] AColumnNames, DataRow ADataRow, int[] APrimKeyColumnOrdList)
        {
            String ReturnValue;
            Int32 Counter;

            //Int32 PrimKeyOrd;
            Boolean First;

            System.Object CurrentValue = null;
            ReturnValue = "UPDATE " + ATableName + " SET ";
            First = true;

            // should ignore additional fields at the end of the row
            if (ADataRow.ItemArray.Length >= AColumnNames.Length)
            {
                for (Counter = 0; Counter <= AColumnNames.Length - 1; Counter += 1)
                {
                    if (NoDefaultColumn(AColumnNames[Counter]))
                    {
                        if (NotEquals(ADataRow, Counter, ref CurrentValue))
                        {
                            if ((!First))
                            {
                                ReturnValue = ReturnValue + ", ";
                            }
                            else
                            {
                                First = false;
                            }

                            if (CurrentValue == System.DBNull.Value)
                            {
                                ReturnValue = ReturnValue + AColumnNames[Counter] + " = NULL";
                            }
                            else
                            {
                                ReturnValue = ReturnValue + AColumnNames[Counter] + " = ?";
                            }
                        }
                    }
                }

                if ((!First))
                {
                    ReturnValue = ReturnValue + ", ";
                }

                ReturnValue = ReturnValue + MODIFICATION_ID + " = NOW(), ";
                ReturnValue = ReturnValue + MODIFIED_BY + " = ?, ";
                ReturnValue = ReturnValue + MODIFIED_DATE + " = ? ";

                // WHERE clause, consisting of primary key
                ReturnValue = ReturnValue + " WHERE ";
                First = true;

                foreach (int PrimKeyOrd in APrimKeyColumnOrdList)
                {
                    if (First == true)
                    {
                        First = false;
                    }
                    else
                    {
                        ReturnValue = ReturnValue + " AND ";
                    }

                    ReturnValue = ReturnValue + AColumnNames[PrimKeyOrd] + " = ?";
                }

                if (ADataRow.IsNull(MODIFICATION_ID) || (Convert.ToDateTime(ADataRow[MODIFICATION_ID]) == DateTime.MinValue))
                {
                    ReturnValue += " AND " + MODIFICATION_ID + " IS NULL";
                }
                else
                {
                    ReturnValue += " AND " + MODIFICATION_ID + " = ?";
                }
            }
            else
            {
                throw new ApplicationException(
                    "Cannot generate UPDATE Clause for Table '" + ATableName + "' because the submitted DataTable has more columns (" +
                    Convert.ToString(ADataRow.ItemArray.Length) + ") than the Table in the DB (" + Convert.ToString(AColumnNames.Length) + ")!");
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function will build a DELETE statement using the DataRowVersion.Original values of the primary key columns
        /// </summary>
        /// <returns>the DELETE statement
        /// </returns>
        public static String GenerateDeleteClause(String ATableName, string[] AColumnNames, DataRow ADataRow, int[] APrimKeyColumnOrdList)
        {
            String ReturnValue;
            Boolean First;

            ReturnValue = "DELETE FROM " + ATableName + " WHERE ";

            // WHERE clause, consisting of primary key
            First = true;

            foreach (int PrimKeyOrd in APrimKeyColumnOrdList)
            {
                if (First == true)
                {
                    First = false;
                }
                else
                {
                    ReturnValue = ReturnValue + " AND ";
                }

                ReturnValue = ReturnValue + AColumnNames[PrimKeyOrd] + " = ?";
            }

            return ReturnValue;
        }

        #region CalledByORMGenerateCode

        /// <summary>
        /// load data row by primary key into a dataset
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="ADataSet"></param>
        /// <param name="APrimaryKeyValues"></param>
        /// <param name="AFieldList"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AOrderBy"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        public static DataRow LoadByPrimaryKey(short ATableId,
            DataSet ADataSet,
            System.Object[] APrimaryKeyValues,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy,
            int AStartRecord,
            int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = CreateOdbcParameterArrayFromPrimaryKey(ATableId, APrimaryKeyValues);
            DBAccess.GDBAccessObj.Select(ADataSet,
                GenerateSelectClause(AFieldList, ATableId) +
                " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                GenerateWhereClauseFromPrimaryKey(ATableId) +
                GenerateOrderByClause(AOrderBy),
                TTypedDataTable.GetTableName(ATableId),
                ATransaction, ParametersArray, AStartRecord, AMaxRecords);

            return ADataSet.Tables[TTypedDataTable.GetTableName(ATableId)].Rows.Find(APrimaryKeyValues);
        }

        /// <summary>
        /// different version for data table
        /// </summary>
        public static DataRow LoadByPrimaryKey(short ATableId,
            TTypedDataTable ADataTable,
            System.Object[] APrimaryKeyValues,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy,
            int AStartRecord,
            int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = CreateOdbcParameterArrayFromPrimaryKey(ATableId, APrimaryKeyValues);
            ADataTable = (TTypedDataTable)DBAccess.GDBAccessObj.SelectDT(ADataTable,
                GenerateSelectClause(AFieldList, ATableId) +
                " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                GenerateWhereClauseFromPrimaryKey(ATableId) +
                GenerateOrderByClause(AOrderBy),
                ATransaction, ParametersArray, AStartRecord, AMaxRecords);

            return ADataTable.Rows.Find(APrimaryKeyValues);
        }

        /// <summary>
        /// load data row by unique key into a dataset
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="ADataSet"></param>
        /// <param name="AUniqueKeyValues"></param>
        /// <param name="AFieldList"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AOrderBy"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        public static void LoadByUniqueKey(short ATableId,
            DataSet ADataSet,
            System.Object[] AUniqueKeyValues,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy,
            int AStartRecord,
            int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = CreateOdbcParameterArrayFromKey(ATableId, TTypedDataTable.GetUniqueKeyColumnOrdList(
                    ATableId), AUniqueKeyValues);
            DBAccess.GDBAccessObj.Select(ADataSet,
                GenerateSelectClause(AFieldList, ATableId) +
                " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                GenerateWhereClause(TTypedDataTable.GetUniqueKeyColumnStringList(ATableId)) +
                GenerateOrderByClause(AOrderBy),
                TTypedDataTable.GetTableName(ATableId),
                ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// <summary>
        /// different version for data table
        /// </summary>
        public static void LoadByUniqueKey(short ATableId,
            TTypedDataTable ADataTable,
            System.Object[] AUniqueKeyValues,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy,
            int AStartRecord,
            int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = CreateOdbcParameterArrayFromKey(ATableId, TTypedDataTable.GetUniqueKeyColumnOrdList(
                    ATableId), AUniqueKeyValues);
            ADataTable = (TTypedDataTable)DBAccess.GDBAccessObj.SelectDT(ADataTable,
                GenerateSelectClause(AFieldList, ATableId) +
                " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                GenerateWhereClause(TTypedDataTable.GetUniqueKeyColumnStringList(ATableId)) +
                GenerateOrderByClause(AOrderBy),
                ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// <summary>
        /// load data for the current table via a foreign key, eg. load all extracts created by user x
        /// </summary>
        public static void LoadViaForeignKey(
            short ATableId,
            short AOtherTableId,
            DataSet ADataSet,
            string[] AThisFieldNames,
            System.Object[] AForeignKeyValues,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy,
            int AStartRecord,
            int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = CreateOdbcParameterArrayFromPrimaryKey(AOtherTableId, AForeignKeyValues);
            DBAccess.GDBAccessObj.Select(ADataSet,
                GenerateSelectClause(AFieldList, ATableId) +
                " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                GenerateWhereClause(AThisFieldNames) +
                GenerateOrderByClause(AOrderBy),
                TTypedDataTable.GetTableName(ATableId),
                ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// <summary>
        /// load data for the current table via a foreign key, eg. load all extracts created by user x
        /// </summary>
        public static void LoadViaForeignKey(
            short ATableId,
            short AOtherTableId,
            TTypedDataTable ADataTable,
            string[] AThisFieldNames,
            System.Object[] AForeignKeyValues,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy,
            int AStartRecord,
            int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = CreateOdbcParameterArrayFromPrimaryKey(AOtherTableId, AForeignKeyValues);
            DBAccess.GDBAccessObj.SelectDT(ADataTable,
                GenerateSelectClause(AFieldList, ATableId) +
                " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                GenerateWhereClause(AThisFieldNames) +
                GenerateOrderByClause(AOrderBy),
                ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// Load via other table, using template values
        public static void LoadViaForeignKey(
            short ATableId,
            short AOtherTableId,
            DataSet ADataSet,
            string[] AThisFieldNames,
            DataRow ATemplateRow,
            StringCollection ATemplateOperators,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy,
            int AStartRecord,
            int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet,
                GenerateSelectClause(AFieldList, ATableId) +
                " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) + ", PUB_" + TTypedDataTable.GetTableNameSQL(AOtherTableId) +
                GenerateWhereClause(AThisFieldNames) +
                GenerateWhereClauseLong("PUB_" + TTypedDataTable.GetTableNameSQL(AOtherTableId),
                    AOtherTableId, ATemplateRow, ATemplateOperators) +
                GenerateOrderByClause(AOrderBy),
                TTypedDataTable.GetTableName(ATableId),
                ATransaction,
                GetParametersForWhereClause(AOtherTableId, ATemplateRow),
                AStartRecord, AMaxRecords);
        }

        /// Load via other table, using template values
        public static void LoadViaForeignKey(
            short ATableId,
            short AOtherTableId,
            TTypedDataTable ADataTable,
            string[] AThisFieldNames,
            DataRow ATemplateRow,
            StringCollection ATemplateOperators,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy,
            int AStartRecord,
            int AMaxRecords)
        {
            DBAccess.GDBAccessObj.SelectDT(ADataTable,
                GenerateSelectClause(AFieldList, ATableId, true) +
                " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) + ", PUB_" + TTypedDataTable.GetTableNameSQL(AOtherTableId) +
                GenerateWhereClauseForJoin(TTypedDataTable.GetTableNameSQL(AOtherTableId), TTypedDataTable.GetTableNameSQL(ATableId), AThisFieldNames, TTypedDataTable.GetPrimaryKeyColumnStringList(AOtherTableId)) +
                GenerateWhereClauseLong("PUB_" + TTypedDataTable.GetTableNameSQL(AOtherTableId),
                    AOtherTableId, ATemplateRow, ATemplateOperators) +
                GenerateOrderByClause(AOrderBy),
                ATransaction,
                GetParametersForWhereClause(AOtherTableId, ATemplateRow),
                AStartRecord, AMaxRecords);
        }

        /// Load via other table, using template values
        public static void LoadViaForeignKey(
            short ATableId,
            short AOtherTableId,
            DataSet ADataSet,
            string[] AThisFieldNames,
            TSearchCriteria[] ASearchCriteria,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy,
            int AStartRecord,
            int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet,
                GenerateSelectClause(AFieldList, ATableId) +
                " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) + ", PUB_" + TTypedDataTable.GetTableNameSQL(AOtherTableId) +
                GenerateWhereClause(AThisFieldNames) +
                GenerateWhereClauseLong("PUB_" + TTypedDataTable.GetTableNameSQL(AOtherTableId),
                    ASearchCriteria) +
                GenerateOrderByClause(AOrderBy),
                TTypedDataTable.GetTableName(ATableId),
                ATransaction,
                GetParametersForWhereClause(AOtherTableId, ASearchCriteria),
                AStartRecord, AMaxRecords);
        }

        /// Load via other table, using template values
        public static void LoadViaForeignKey(
            short ATableId,
            short AOtherTableId,
            TTypedDataTable ADataTable,
            string[] AThisFieldNames,
            TSearchCriteria[] ASearchCriteria,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy,
            int AStartRecord,
            int AMaxRecords)
        {
            DBAccess.GDBAccessObj.SelectDT(ADataTable,
                GenerateSelectClause(AFieldList, ATableId) +
                " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) + ", PUB_" + TTypedDataTable.GetTableNameSQL(AOtherTableId) +
                GenerateWhereClause(AThisFieldNames) +
                GenerateWhereClauseLong("PUB_" + TTypedDataTable.GetTableNameSQL(AOtherTableId),
                    ASearchCriteria) +
                GenerateOrderByClause(AOrderBy),
                ATransaction,
                GetParametersForWhereClause(AOtherTableId, ASearchCriteria),
                AStartRecord, AMaxRecords);
        }

        /// count the rows by the values of a foreign key
        public static int CountViaForeignKey(
            short ATableId, short AOtherTableId,
            string[] AThisFieldNames,
            System.Object[] AForeignKeyValues,
            TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = CreateOdbcParameterArrayFromPrimaryKey(AOtherTableId, AForeignKeyValues);
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(
                    "SELECT COUNT(*) FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                    GenerateWhereClause(AThisFieldNames),
                    ATransaction,
                    false,
                    ParametersArray));
        }

        /// count the rows by the values of a foreign key
        public static int CountViaForeignKey(
            short ATableId, short AOtherTableId,
            string[] AThisFieldNames,
            DataRow ATemplateRow,
            StringCollection ATemplateOperators,
            TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = GetParametersForWhereClause(AOtherTableId, ATemplateRow);
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(
                    "SELECT COUNT(*) FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                    GenerateWhereClause(AThisFieldNames) +
                    GenerateWhereClauseLong("PUB_" + TTypedDataTable.GetTableNameSQL(AOtherTableId),
                        AOtherTableId, ATemplateRow, ATemplateOperators),
                    ATransaction,
                    false,
                    ParametersArray));
        }

        /// count the rows by the values of a foreign key
        public static int CountViaForeignKey(
            short ATableId, short AOtherTableId,
            string[] AThisFieldNames,
            TSearchCriteria[] ASearchCriteria,
            TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = GetParametersForWhereClause(AOtherTableId, ASearchCriteria);
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(
                    "SELECT COUNT(*) FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                    GenerateWhereClause(AThisFieldNames) +
                    GenerateWhereClauseLong("PUB_" + TTypedDataTable.GetTableNameSQL(AOtherTableId),
                        ASearchCriteria),
                    ATransaction,
                    false,
                    ParametersArray));
        }

        /// <summary>
        /// delete by primary key values
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="APrimaryKeyValues"></param>
        /// <param name="ATransaction"></param>
        public static void DeleteByPrimaryKey(short ATableId, System.Object[] APrimaryKeyValues, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = CreateOdbcParameterArrayFromPrimaryKey(ATableId, APrimaryKeyValues);
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                GenerateWhereClauseFromPrimaryKey(ATableId),
                ATransaction, false, ParametersArray);
        }

        /// <summary>
        /// check if row exists with the primary key
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="APrimaryKeyValues"></param>
        /// <param name="ATransaction"></param>
        public static bool Exists(short ATableId, System.Object[] APrimaryKeyValues, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = CreateOdbcParameterArrayFromPrimaryKey(ATableId, APrimaryKeyValues);
            return 1 == Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_" +
                    TTypedDataTable.GetTableNameSQL(ATableId) +
                    GenerateWhereClauseFromPrimaryKey(ATableId),
                    ATransaction, false, ParametersArray));
        }

        /// <summary>
        /// check if row exists with the unique key
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="AUniqueKeyValues"></param>
        /// <param name="ATransaction"></param>
        public static bool ExistsUniqueKey(short ATableId, System.Object[] AUniqueKeyValues, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = CreateOdbcParameterArrayFromUniqueKey(ATableId, AUniqueKeyValues);
            return 1 == Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_" +
                    TTypedDataTable.GetTableNameSQL(ATableId) +
                    GenerateWhereClauseFromUniqueKey(ATableId),
                    ATransaction, false, ParametersArray));
        }

        /// <summary>
        /// loads all rows matching certain search criteria into a dataset
        /// </summary>
        /// <param name="ATableID">specify which typed table is used</param>
        /// <param name="ADataSet">the result will be added to this dataset</param>
        /// <param name="ASearchCriteria"></param>
        /// <param name="AFieldList">fields to load from the database table</param>
        /// <param name="ATransaction"></param>
        /// <param name="AOrderBy"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        public static void LoadUsingTemplate(short ATableID,
            DataSet ADataSet,
            TSearchCriteria[] ASearchCriteria,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy,
            int AStartRecord,
            int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, GenerateSelectClause(AFieldList, ATableID) +
                " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableID) +
                GenerateWhereClause(TTypedDataTable.GetColumnStringList(ATableID), ASearchCriteria) +
                GenerateOrderByClause(AOrderBy), TTypedDataTable.GetTableName(ATableID), ATransaction,
                GetParametersForWhereClause(ATableID, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// <summary>
        /// loads all rows matching certain search criteria into a typed data table
        /// </summary>
        /// <param name="ATableID">specify which typed table is used</param>
        /// <param name="ATypedTableToLoad">pre condition: has to have an object of the typed table</param>
        /// <param name="ASearchCriteria"></param>
        /// <param name="AFieldList"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AOrderBy"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        public static void LoadUsingTemplate(
            short ATableID,
            TTypedDataTable ATypedTableToLoad,
            TSearchCriteria[] ASearchCriteria,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy,
            int AStartRecord,
            int AMaxRecords)
        {
            ATypedTableToLoad = (TTypedDataTable)
                                DBAccess.GDBAccessObj.SelectDT(ATypedTableToLoad, GenerateSelectClause(AFieldList, ATableID) +
                " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableID) +
                GenerateWhereClause(TTypedDataTable.GetColumnStringList(ATableID), ASearchCriteria) +
                GenerateOrderByClause(AOrderBy), ATransaction,
                GetParametersForWhereClause(ATableID, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// <summary>
        /// load using a template row
        /// </summary>
        public static void LoadUsingTemplate(
            short ATableId,
            DataSet ADataSet,
            DataRow ATemplateRow,
            StringCollection ATemplateOperators,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet,
                (((GenerateSelectClause(AFieldList, ATableId) + " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId)) +
                  GenerateWhereClause(TTypedDataTable.GetColumnStringList(ATableId), ATemplateRow,
                      ATemplateOperators)) +
                 GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ATableId), ATransaction,
                GetParametersForWhereClause(ATableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// <summary>
        /// load using a template row
        /// </summary>
        public static void LoadUsingTemplate(
            short ATableId,
            TTypedDataTable ADataTable,
            DataRow ATemplateRow,
            StringCollection ATemplateOperators,
            StringCollection AFieldList,
            TDBTransaction ATransaction,
            StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataTable =
                (TTypedDataTable)DBAccess.GDBAccessObj.SelectDT(ADataTable,
                    (((GenerateSelectClause(AFieldList, ATableId) + " FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId)) +
                      GenerateWhereClause(TTypedDataTable.GetColumnStringList(
                              ATableId), ATemplateRow, ATemplateOperators)) +
                     GenerateOrderByClause(AOrderBy)), ATransaction,
                    GetParametersForWhereClause(ATableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// <summary>
        /// delete all rows matching the given row
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="ATemplateRow"></param>
        /// <param name="ATemplateOperators"></param>
        /// <param name="ATransaction"></param>
        public static void DeleteUsingTemplate(short ATableId, DataRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                GenerateWhereClause(TTypedDataTable.GetColumnStringList(ATableId), ATemplateRow, ATemplateOperators),
                ATransaction, false,
                GetParametersForWhereClause(ATableId, ATemplateRow));
        }

        /// <summary>
        /// delete all rows matching the search criteria
        /// </summary>
        /// <param name="ATableId">specify which typed table is used</param>
        /// <param name="ASearchCriteria"></param>
        /// <param name="ATransaction"></param>
        public static void DeleteUsingTemplate(short ATableId, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_" + TTypedDataTable.GetTableNameSQL(ATableId) +
                                                   GenerateWhereClause(TTypedDataTable.GetColumnStringList(ATableId), ASearchCriteria)),
                ATransaction, false,
                GetParametersForWhereClause(ATableId, ASearchCriteria));
        }

        /// <summary>
        /// enumeration of operations that can be selected for SubmitChanges
        /// </summary>
        public enum eSubmitChangesOperations
        {
            /// update records
            eUpdate = 1,

            /// delete records
            eDelete = 2,

            /// add new records
            eInsert = 4,

            /// execute all operations, no matter if it is update, delete, insert
            eAll = 7
        };

        /// <summary>
        /// submit those rows in the table that have been modified or created or deleted
        /// </summary>
        /// <param name="ATable"></param>
        /// <param name="ATransaction"></param>
        /// <param name="ASelectedOperations"></param>
        /// <param name="AVerificationResult"></param>
        /// <param name="AUserId">the current user, for auditing</param>
        /// <param name="ASequenceName"></param>
        /// <param name="ASequenceField"></param>
        /// <returns></returns>
        public static bool SubmitChanges(
            TTypedDataTable ATable,
            TDBTransaction ATransaction,
            eSubmitChangesOperations ASelectedOperations,
            out TVerificationResultCollection AVerificationResult,
            string AUserId,
            string ASequenceName, string ASequenceField)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            bool TreatRowAsAdded = false;
            DataRow TheRow = null;

            AVerificationResult = new TVerificationResultCollection();

            // allow this method to be called with null values, eg. when saving complex TypedDataSets with some removed empty tables
            if (ATable == null)
            {
                return true;
            }

            short TableId = Convert.ToInt16(ATable.GetType().GetField("TableId").GetValue(null));

            if (!ATable.ThrowAwayAfterSubmitChanges && (ATable.Rows.Count > 1000))
            {
                TLogging.Log(
                    "Warning to the developer: Saving " + ATable.Rows.Count.ToString() + " records to table " + ATable.TableName +
                    " without ThrowAwayAfterSubmitChanges is quite slow");
                TLogging.Log(
                    "It is recommended to use either ThrowAwayAfterSubmitChanges and Reload all data at once, or GetChanges to have less queries for updated modification timestamp");
                TLogging.LogStackTrace(TLoggingType.ToLogfile);
            }

            List <OdbcParameter>InsertParameters = new List <OdbcParameter>();
            StringBuilder InsertStatement = new StringBuilder();

            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable.Rows[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added) && ((ASelectedOperations & eSubmitChangesOperations.eInsert) != 0))
                    {
                        if (ASequenceField.Length > 0)
                        {
                            // only insert next sequence value if the field has negative value.
                            // this is needed when creating location 0 for a new site/ledger
                            if (Convert.ToInt64(TheRow[ASequenceField]) < 0)
                            {
                                // accept changes for the row, so that we can update the dataset on the client and still know the negative temp sequence number
                                TheRow.AcceptChanges();
                                TheRow[ASequenceField] = (System.Object)DBAccess.GDBAccessObj.GetNextSequenceValue(ASequenceName, ATransaction);
                                TreatRowAsAdded = true;   // setting this variable to 'true' is *vital* for the retrieval of the s_modification_id_t for that record once it is saved!
                            }
                        }

                        if (ATable.ThrowAwayAfterSubmitChanges)
                        {
                            TTypedDataAccess.InsertRow(TableId, ref TheRow, ATransaction, AUserId, InsertStatement, InsertParameters);
                        }
                        else
                        {
                            TTypedDataAccess.InsertRow(TableId, ref TheRow, ATransaction, AUserId, TreatRowAsAdded);
                        }
                    }
                    else
                    {
                        bool hasPrimaryKey = TTypedDataTable.GetPrimaryKeyColumnOrdList(TableId).Length > 0;

                        if ((TheRow.RowState == DataRowState.Modified) && ((ASelectedOperations & eSubmitChangesOperations.eUpdate) != 0))
                        {
                            if (!hasPrimaryKey)
                            {
                                AVerificationResult.Add(new TVerificationResult(
                                        "[DB] NO PRIMARY KEY",
                                        "Cannot update record because table " + TTypedDataTable.GetTableName(TableId) + " has no primary key.",
                                        "Primary Key missing", TTypedDataTable.GetTableNameSQL(TableId), TResultSeverity.Resv_Critical));
                            }
                            else
                            {
                                TTypedDataAccess.UpdateRow(TableId, ATable.ThrowAwayAfterSubmitChanges, ref TheRow, ATransaction, AUserId);
                            }
                        }

                        if ((TheRow.RowState == DataRowState.Deleted) && ((ASelectedOperations & eSubmitChangesOperations.eDelete) != 0))
                        {
                            if (!hasPrimaryKey)
                            {
                                AVerificationResult.Add(new TVerificationResult(
                                        "[DB] NO PRIMARY KEY",
                                        "Cannot delete record because table " + TTypedDataTable.GetTableName(TableId) + " has no primary key.",
                                        "Primary Key missing", TTypedDataTable.GetTableNameSQL(TableId), TResultSeverity.Resv_Critical));
                            }
                            else
                            {
                                TTypedDataAccess.DeleteRow(TableId, TheRow, ATransaction);
                            }
                        }
                    }

                    if (InsertParameters.Count > MAX_SQL_PARAMETERS)
                    {
                        // Inserts in one query
                        DBAccess.GDBAccessObj.ExecuteNonQuery(InsertStatement.ToString(), ATransaction, false, InsertParameters.ToArray());
                        InsertStatement = new StringBuilder();
                        InsertParameters = new List <OdbcParameter>();
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;

                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PLanguage",
                                ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }

            if (InsertStatement.Length > 0)
            {
                try
                {
                    // Inserts in one query
                    DBAccess.GDBAccessObj.ExecuteNonQuery(InsertStatement.ToString(), ATransaction, false, InsertParameters.ToArray());
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;

                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PLanguage",
                                ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }

            return ResultValue;
        }

        /// <summary>
        /// overloaded version without ASelectedOperations
        /// </summary>
        public static bool SubmitChanges(
            TTypedDataTable ATable,
            TDBTransaction ATransaction,
            out TVerificationResultCollection AVerificationResult,
            string AUserId,
            string ASequenceName, string ASequenceField)
        {
            return SubmitChanges(
                ATable,
                ATransaction,
                eSubmitChangesOperations.eAll,
                out AVerificationResult,
                AUserId,
                ASequenceName,
                ASequenceField);
        }

        /// <summary>
        /// overloaded version without sequence
        /// </summary>
        public static bool SubmitChanges(
            TTypedDataTable ATable,
            TDBTransaction ATransaction,
            out TVerificationResultCollection AVerificationResult,
            string AUserId)
        {
            return SubmitChanges(ATable, ATransaction, eSubmitChangesOperations.eAll, out AVerificationResult, AUserId, "", "");
        }

        /// <summary>
        /// overloaded version without sequence, but with ASelectedOperations
        /// </summary>
        public static bool SubmitChanges(
            TTypedDataTable ATable,
            TDBTransaction ATransaction,
            eSubmitChangesOperations ASelectedOperations,
            out TVerificationResultCollection AVerificationResult,
            string AUserId)
        {
            return SubmitChanges(ATable, ATransaction, ASelectedOperations, out AVerificationResult, AUserId, "", "");
        }

        /// <summary>
        /// Builds a <see cref="TVerificationResultCollection" /> from DB Table references created by a cascading count Method.
        /// It will contain only a single <see cref="TVerificationResult "/>.
        /// </summary>
        /// <returns><see cref="TVerificationResultCollection" /> from DB Table references created by a cascading count Method.
        /// In case <paramref name="AReferences" /> does not contain elements a <see cref="TVerificationResultCollection" /> 
        /// containing no elements will be returned.</returns>
        /// <param name="AThisTable">Name of the DB Table (as in the DB) that the references point to.</param>
        /// <param name="AThisTableLabel">Label (='friendly name' for the user) of the DB Table that the references point to.</param>
        /// <param name="APKInfo"><see cref="Dictionary{T, T}" /> consisting of a string-object pair for each Primary Key Column. 
        /// The string (Key) is the Label ('friendly name' for the user) of the PK Column and the object (Value) holds the actual data 
        /// of the PK Column.</param>
        /// <param name="AReferences">A <see cref="List{T}" /> of <see cref="TRowReferenceInfo" /> objects that contain information about
        /// the DB Table references created by a cascading count Method.</param>
        /// <param name="AResultSeverity">Allows the specification of a <see cref="TResultSeverity" /> for the <see cref="TVerificationResult "/>
        /// that gets added to the return value. (Default=<see cref="TResultSeverity.Resv_Critical" />.)</param>
        /// <returns>A <see cref="TVerificationResultCollection" /> containing a single <see cref="TVerificationResult "/> that contains information
        /// about DB Table references created by a cascading count Method.</returns>
        public static TVerificationResultCollection BuildVerificationResultCollectionFromRefTables(string AThisTable, string AThisTableLabel, Dictionary<string, object> APKInfo, List<TRowReferenceInfo> AReferences,
            TResultSeverity AResultSeverity = TResultSeverity.Resv_Critical)
        {
            const string STR_BULLET = "* ";
            
            Guid VerificationRunGuid = Guid.NewGuid();
            TVerificationResultCollection ReturnValue = new TVerificationResultCollection(VerificationRunGuid);
            string MessageHeaderPart1 = Catalog.GetString(String.Format("The '{0}' record", AThisTableLabel));
            string MessageHeaderPart2 = Catalog.GetString("{0}\r\n    cannot be deleted because\r\n");
            string[] MessageDetails = null;
            string CompleteMessageDetails = String.Empty;
            string MessageContinuation = Catalog.GetString(" and\r\n");
            string KeysAndValueInformation = String.Empty;
            List<KeyValuePair<string, TRowReferenceInfo>> ConsolidatedReferences = new List<KeyValuePair<string, TRowReferenceInfo>>();
            string SingleReferenceThisTable = String.Empty;
            
            if (AReferences.Count == 0) 
            {
                return null;    
            }
                        
            // Iterate through References backwards because the insertion order in AReference is backwards
            for(int Counter = AReferences.Count - 1; Counter >= 0; Counter--) 
            {      
                AddOrUpdateConsolidatedReferences(AReferences[Counter], ConsolidatedReferences);
            }
            
            MessageDetails = new string[ConsolidatedReferences.Count];
            
            for (int Counter = 0; Counter < ConsolidatedReferences.Count; Counter++) 
            {
                MessageDetails[Counter] = Catalog.GetPluralString(
                    String.Format(STR_INDENTATION + "a '{0}' record is {1} referencing it", ConsolidatedReferences[Counter].Value.ThisTableLabel, Counter != 0 ? Catalog.GetString("indirectly") : Catalog.GetString("still")),
                    String.Format(STR_INDENTATION + "{0} '{1}' records are {2} referencing it", ConsolidatedReferences[Counter].Value.ReferenceCount, ConsolidatedReferences[Counter].Value.ThisTableLabel, Counter != 0 ?  Catalog.GetString("indirectly") : Catalog.GetString("still")), ConsolidatedReferences[Counter].Value.ReferenceCount);
            }
                                
            if (MessageDetails.Length > 1) 
            {
                for (int Counter = 0; Counter < MessageDetails.Length; Counter++) 
                {
                    MessageDetails[Counter] = STR_INDENTATION + new string(' ', Counter) + STR_BULLET + MessageDetails[Counter].Substring(STR_INDENTATION.Length);                    
                    
                    if (Counter != MessageDetails.Length - 1) 
                    {
                        MessageDetails[Counter] += MessageContinuation;
                    }
                    
                    CompleteMessageDetails += MessageDetails[Counter];
                }    
            }
            else
            {
                CompleteMessageDetails = MessageDetails[0];
            }
            
            
            CompleteMessageDetails += ".";
            
            foreach (var element in APKInfo) 
            {
                KeysAndValueInformation += STR_INDENTATION + STR_INDENTATION + element.Key + ": " + element.Value.ToString() + Environment.NewLine;
            }
            
            // Strip off trailing Environment.NewLine
            KeysAndValueInformation = KeysAndValueInformation.Substring(0, KeysAndValueInformation.Length - Environment.NewLine.Length);
            
            MessageHeaderPart2 = String.Format(MessageHeaderPart2, "\r\n" + STR_INDENTATION + "  " + AThisTableLabel + " code:\r\n" + KeysAndValueInformation);
                        
            ReturnValue.Add(new TVerificationResult(new TRowReferenceInfo(AThisTable, AThisTableLabel, APKInfo, ConsolidatedReferences),
                MessageHeaderPart1 + MessageHeaderPart2 + CompleteMessageDetails, CommonErrorCodes.ERR_RECORD_DELETION_NOT_POSSIBLE_REFERENCED,  
                AResultSeverity, VerificationRunGuid));

            return ReturnValue;
        }
        
        private static void AddOrUpdateConsolidatedReferences(TRowReferenceInfo AFurtherReference, List<KeyValuePair<string, TRowReferenceInfo>> AConsolidatedReferences)
        {
            TRowReferenceInfo ExistingReference = null;
            
            for (int Counter = 0; Counter < AConsolidatedReferences.Count; Counter++) 
            {
                if (AConsolidatedReferences[Counter].Key == AFurtherReference.ThisTable) 
                {
                    ExistingReference = AConsolidatedReferences[Counter].Value;
                    break;
                }    
            }
            
            if (ExistingReference != null) 
            {
                ExistingReference.SetReferenceCount(ExistingReference.ReferenceCount + AFurtherReference.ReferenceCount);
                ExistingReference.PopulatePKInfoDataFromDataRow();
            }
            else
            {
                AFurtherReference.PopulatePKInfoDataFromDataRow();
                AConsolidatedReferences.Add(new KeyValuePair<string, TRowReferenceInfo>(AFurtherReference.ThisTable, AFurtherReference));
            }
        }
        
        #endregion CalledByORMGenerateCode
    }
    
    /// <summary>
    /// Data Class that holds information about DB Rows referencing another DB Row.
    /// </summary>
    [Serializable()]
    public class TRowReferenceInfo
    {
        private string FThisTable = String.Empty;
        private string FThisTableLabel = String.Empty;
        private bool FRootTable = false;
        private Dictionary<string, object> FPKInfo;
        private List<KeyValuePair<string, TRowReferenceInfo>> FOtherTableRefs = null;
        private long FReferenceCount = 0;
        [NonSerialized]
        private DataRow FDataRow = null;
        private object[] FDataRowContents;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="AThisTable">Name of the DB Table (as in the DB) that holds the DB Row in question.</param>
        /// <param name="AThisTableLabel">Label (='friendly name' for the user) of the DB Table that holds the DB Row in question.</param>
        /// <param name="APKInfo"><see cref="Dictionary{T,T}" /> consisting of a string-object pair for each Primary Key Column. 
        /// The string (Key) is the Label ('friendly name' for the user) of the PK Column and the object (Value) holds the actual data 
        /// of the PK Column.</param>
        /// <param name="AOtherTableRefs">References to the DB Row in question.</param>
        public TRowReferenceInfo(string AThisTable, string AThisTableLabel, Dictionary<string, object> APKInfo, List<KeyValuePair<string, TRowReferenceInfo>> AOtherTableRefs)
        {
            FThisTable = AThisTable;
            FRootTable = true;
            FThisTableLabel = AThisTableLabel;
            FOtherTableRefs = AOtherTableRefs;
            FPKInfo = APKInfo;
            FReferenceCount = 1;           
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="AThisTable">Name of the DB Table (as in the DB) that holds the DB Row in question.</param>
        /// <param name="AThisTableLabel">Label (='friendly name') of the DB Table that holds the DB Row in question.</param>
        /// <param name="AReferenceCount">References count.</param>
        /// <param name="ADataRow">Referencing DataRow.</param>
        public TRowReferenceInfo(string AThisTable, string AThisTableLabel, long AReferenceCount, DataRow ADataRow)
        {
            FThisTable = AThisTable;
            FThisTableLabel = AThisTableLabel;
            FReferenceCount = AReferenceCount;
            FDataRow = ADataRow;
            FDataRowContents = DataUtilities.GetPKValuesFromDataRow(FDataRow);
        }
                
        /// <summary>
        /// Name of the DB Table (as in the DB) that holds the DB Row in question.
        /// </summary>
        public string ThisTable 
        {
            get 
            {
                return FThisTable; 
            }
        }

        /// <summary>
        /// Label (='friendly name' for the user) of the DB Table that holds the DB Row in question.
        /// </summary>
        public string ThisTableLabel 
        {
            get 
            { 
                return FThisTableLabel; 
            }
        }

        /// <summary>
        /// True if this instance is for the 'root' table in a cascading table hierarachy.
        /// </summary>
        public bool RootTable
        {
            get
            {
                return FRootTable;
            }
        }
        
        /// <summary>
        /// <see cref="Dictionary{T,T}" /> consisting of a string-object pair for each Primary Key Column. 
        /// The string (Key) is the Label ('friendly name' for the user) of the PK Column and the object (Value) holds the actual data 
        /// of the PK Column.
        /// </summary>
        public Dictionary<string, object> PKInfo
        {
            get
            {
                return FPKInfo;
            }
        }
        
        /// <summary>
        /// Number of references that point to the DB Row in question.
        /// </summary>
        public long ReferenceCount 
        {
            get 
            { 
                return FReferenceCount; 
            }
        }

        /// <summary>
        /// References to the DB Row in question.
        /// </summary>
        public List<KeyValuePair<string, TRowReferenceInfo>> OtherTableRefs
        {
            get
            {
                return FOtherTableRefs;
            }
        }
        
        /// <summary>
        /// Referencing DataRow.
        /// </summary>
        /// <remarks>Does not get serialised because the <see cref="DataRow" /> Class is not Serializable.
        /// Use <see cref="DataRowContents" /> for accesing the data of the referencing DataRow instead in this case!</remarks>
        public DataRow DataRow 
        {
            get 
            { 
                return FDataRow; 
            }
        }
        
        /// <summary>
        /// Data of the referencing DataRow represented as an Array of object. Use this for accesing the data of 
        /// the referencing DataRow when the instance of this class got remoted using .NET Remoting.
        /// </summary>
        public object[] DataRowContents
        {
            get
            {
                return FDataRowContents;
            }
        }
        
        /// <summary>
        /// Allows the setting of the <see cref="ReferenceCount" /> Property. Introduced so that the 
        /// <see cref="ReferenceCount" /> Property can stay read-only, which it should be.
        /// </summary>
        /// <param name="AReferenceCount">Reference Count.</param>
        public void SetReferenceCount(long AReferenceCount)
        {
            FReferenceCount = AReferenceCount;
        }
        
        /// <summary>
        /// Allows the setting of the <see cref="PKInfo" /> Property. Introduced so that the 
        /// <see cref="PKInfo" /> Property can stay read-only, which it should be.
        /// </summary>
        /// <param name="APKInfo">See <see cref="PKInfo" /> Property.</param>
        public void SetPKInfo(Dictionary<string, object> APKInfo)
        {
            FPKInfo = APKInfo;
        }
        
        /// <summary>
        /// Populates missing data in the Value part of the <see cref="PKInfo" /> Property from
        /// the data in the <see cref="DataRow" /> (which must contain only Primary Key data for this
        /// to work).
        /// </summary>
        public void PopulatePKInfoDataFromDataRow()
        {
            if (FPKInfo != null) 
            {
                Dictionary<string, object> PKInfoNew = new Dictionary<string, object>(FPKInfo.Count);
                int Counter = 0;
                
                foreach (var SinglePKInfo in FPKInfo) 
                {
                    PKInfoNew.Add(SinglePKInfo.Key, FDataRow[Counter]);
                    Counter++;
                }
                
                FPKInfo = PKInfoNew;                
            }
        }
    }
}