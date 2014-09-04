//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Generic;
using System.Runtime.Serialization;
using GNU.Gettext;

namespace Ict.Common.Data
{
    /// <summary>
    /// This is the base class for the typed datatables.
    /// </summary>
    [Serializable()]
    public abstract class TTypedDataTable : DataTable
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">name of table</param>
        public TTypedDataTable(string name) : base(name)
        {
            this.InitClass();
            this.InitVars();
        }

        /// <summary>
        /// on purpose, this constructor does not call InitClass or InitVars;
        /// used for serialization
        /// </summary>
        /// <param name="tab">table for copying the table name</param>
        public TTypedDataTable(DataTable tab) : base(tab.TableName)
        {
            // System.Console.WriteLine('TTypedDataTable constructor tab:DataTable');
        }

        /// <summary>
        /// default constructor
        /// not needed, but for clarity
        /// </summary>
        public TTypedDataTable() : base()
        {
            this.InitClass();
            this.InitVars();
        }

        /// <summary>
        /// serialization constructor
        /// </summary>
        /// <param name="info">required for serialization</param>
        /// <param name="context">required for serialization</param>
        public TTypedDataTable(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
            // Console.WriteLine('TTypeddatatable.create serialization');
            this.InitVars();
        }

        /// <summary>
        /// abstract method to be implemented by generated code
        /// </summary>
        protected abstract void InitClass();

        /// <summary>
        /// abstract method to be implemented by generated code
        /// </summary>
        public abstract void InitVars();

        /// <summary>
        /// abstract method to be implemented by generated code
        /// </summary>
        public abstract string TableDBLabel {
            get;
        }

        /// <summary>
        /// abstract method to be implemented by generated code
        /// </summary>
        public abstract OdbcParameter CreateOdbcParameter(Int32 AColNumber);

        /// <summary>
        /// make sure that we use GetChangesType instead of GetChanges
        /// </summary>
        /// <returns>throws an exception</returns>
        public new DataTable GetChanges()
        {
            throw new Exception("Note to the developer: use GetChangesTyped instead of DataTable.GetChanges");

            // return null;
        }

        /// <summary>
        /// if you want the datatable to be cleared after submitchanges.
        /// This will increase the speed significantly: no updating of modificationID, no slow AcceptChanges.
        /// </summary>
        public bool ThrowAwayAfterSubmitChanges = false;

        /// <summary>
        /// if you want no warning about that the datatable should be cleared after submitchanges.
        /// in some cases you must keep the data, eg when you need the new keys after INSERT
        /// </summary>
        public bool DontThrowAwayAfterSubmitChanges = false;

        /// <summary>
        /// our own version of GetChanges
        /// </summary>
        /// <returns>returns a typed table with the changes</returns>
        public DataTable GetChangesTypedInternal()
        {
            DataTable ReturnValue;

            ReturnValue = base.GetChanges();

            if (ReturnValue != null)
            {
                // might not be necessary. The casting in the derived class might already call the contructor?
                ((TTypedDataTable)ReturnValue).InitVars();
            }

            return ReturnValue;
        }

        /// <summary>
        /// the number of rows in the current table
        /// </summary>
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        /// <summary>
        /// remove columns that are not needed
        /// </summary>
        /// <param name="ATableTemplate">this table only contains the columns that should be kept</param>
        public void RemoveColumnsNotInTableTemplate(DataTable ATableTemplate)
        {
            DataUtilities.RemoveColumnsNotInTableTemplate(this, ATableTemplate);
        }

        /// <summary>
        /// stores information about typed tables
        /// </summary>
        protected static SortedList <short, TTypedTableInfo>TableInfo = new SortedList <short, TTypedTableInfo>();

        /// will be filled by generated code
        public class TTypedColumnInfo
        {
            /// identification of the column, by order
            public short orderNumber;

            /// nice name of column (CamelCase)
            public string name;

            /// name of the column as it is in the SQL database
            public string dbname;

            /// Label (with translation if it is used on a generated screen on the client side)
            public string label;

            /// odbc type of the column
            public System.Data.Odbc.OdbcType odbctype;

            /// if this type has a length, here it is
            public Int32 length;

            /// can the column never be NULL
            public bool bNotNull;

            /// constructor
            public TTypedColumnInfo(short AOrderNumber,
                string AName,
                string ADBName,
                string ALabel,
                System.Data.Odbc.OdbcType AOdbcType,
                Int32 ALength,
                bool ANotNull)
            {
                orderNumber = AOrderNumber;
                name = AName;
                dbname = ADBName;
                label = ALabel;
                odbctype = AOdbcType;
                length = ALength;
                bNotNull = ANotNull;
            }
        }

        /// will be filled by generated code
        public class TTypedTableInfo
        {
            /// identification of the table, by order
            public short id;

            /// nice name of table (CamelCase)
            public string name;

            /// name of the table as it is in the SQL database
            public string dbname;

            /// the order number of the columns that are part of the primary key
            public int[] PrimaryKeyColumns;

            /// the order number of the columns that are part of the unique key
            public int[] UniqueKeyColumns;

            /// the columns of this table
            public TTypedColumnInfo[] columns;

            /// constructor
            public TTypedTableInfo(short AId, string AName, string ADBName, TTypedColumnInfo[] AColumns, int[] APrimaryKeyColumns)
                : this(AId, AName, ADBName, AColumns, APrimaryKeyColumns, new int[]
                       {
                       })
            {
                id = AId;
                name = AName;
                dbname = ADBName;
                columns = AColumns;
                PrimaryKeyColumns = APrimaryKeyColumns;
            }

            /// constructor
            public TTypedTableInfo(short AId,
                string AName,
                string ADBName,
                TTypedColumnInfo[] AColumns,
                int[] APrimaryKeyColumns,
                int[] AUniqueKeyColumns)
            {
                id = AId;
                name = AName;
                dbname = ADBName;
                columns = AColumns;
                PrimaryKeyColumns = APrimaryKeyColumns;
                UniqueKeyColumns = AUniqueKeyColumns;
            }
        }

        /// the table name as it is in the SQL database
        public static string GetTableNameSQL(short ATableNumber)
        {
            return TableInfo[ATableNumber].dbname;
        }

        /// the table name in CamelCase
        public static string GetTableName(short ATableNumber)
        {
            return TableInfo[ATableNumber].name;
        }

        /// the table name in CamelCase
        public static TTypedTableInfo GetTableByName(string ATableName)
        {
            foreach (TTypedTableInfo info in TableInfo.Values)
            {
                if (info.name == ATableName)
                {
                    return info;
                }
            }

            throw new Exception("TTypedDataTable::GetTableByName cannot find " + ATableName);
        }

        /// the column name as it is in the SQL database
        public static string GetColumnNameSQL(short ATableNumber, short AColumnNr)
        {
            return TableInfo[ATableNumber].columns[AColumnNr].dbname;
        }

        /// returns the translated label for the column
        public static string GetLabel(short ATableNumber, short AColumnNr)
        {
            return Catalog.GetString(TableInfo[ATableNumber].columns[AColumnNr].label);
        }

        /// get the maximum length for the field
        public static Int32 GetLength(short ATableNumber, short AColumnNr)
        {
            return TableInfo[ATableNumber].columns[AColumnNr].length;
        }

        /// get the maximum length for the field
        public static Int32 GetLength(string ATableName, string AColumnName)
        {
            try
            {
                TTypedTableInfo table = GetTableByName(ATableName);

                foreach (TTypedColumnInfo col in table.columns)
                {
                    if ((col.name == AColumnName) || (col.dbname == AColumnName))
                    {
                        if (col.odbctype == OdbcType.Date)
                        {
                            // to avoid errors in CommonControls::TexpTextBoxStringLengthCheck::RetrieveTextBoxMaxLength
                            return 20;
                        }

                        if (col.length <= 0)
                        {
                            // to avoid errors in CommonControls::TexpTextBoxStringLengthCheck::RetrieveTextBoxMaxLength
                            return 20;
                        }

                        return col.length;
                    }
                }
            }
            catch (Exception)
            {
            }
            return -1;
        }

        /// get the names of the columns that are part of the key
        public static string[] GetKeyColumnStringList(short ATableNumber, int[] AKeyColumnsOrder)
        {
            string s = "";

            foreach (int item in AKeyColumnsOrder)
            {
                if (s.Length > 0)
                {
                    s += ",";
                }

                s += TableInfo[ATableNumber].columns[item].dbname;
            }

            return s.Split(',');
        }

        /// get the names of the columns that are part of the primary key
        public static string[] GetPrimaryKeyColumnStringList(short ATableNumber)
        {
            return GetKeyColumnStringList(ATableNumber, GetPrimaryKeyColumnOrdList(ATableNumber));
        }

        /// get the names of the columns that are part of the unique key
        public static string[] GetUniqueKeyColumnStringList(short ATableNumber)
        {
            return GetKeyColumnStringList(ATableNumber, GetUniqueKeyColumnOrdList(ATableNumber));
        }

        /// get the order number of the columns that are part of the primary key
        public static int[] GetPrimaryKeyColumnOrdList(short ATableNumber)
        {
            return TableInfo[ATableNumber].PrimaryKeyColumns;
        }

        /// get the order number of the columns that are part of a unique key
        public static int[] GetUniqueKeyColumnOrdList(short ATableNumber)
        {
            return TableInfo[ATableNumber].UniqueKeyColumns;
        }

        /// get the names of the columns in this table
        public static string[] GetColumnStringList(short ATableNumber)
        {
            string[] ReturnValue = new string[TableInfo[ATableNumber].columns.Length];
            short counter = 0;

            foreach (TTypedColumnInfo col in TableInfo[ATableNumber].columns)
            {
                ReturnValue[counter++] = col.dbname;
            }

            return ReturnValue;
        }

        /// get the details of a column
        private static TTypedColumnInfo GetColumn(short ATableNumber, string colname)
        {
            foreach (TTypedColumnInfo col in TableInfo[ATableNumber].columns)
            {
                if ((col.name == colname) || (col.dbname == colname))
                {
                    return col;
                }
            }

            throw new Exception("TTypedDataTable::GetColumn cannot find column " + colname);
        }

        /// create an odbc parameter for the given column
        public static OdbcParameter CreateOdbcParameter(short ATableNumber, TSearchCriteria ASearchCriteria)
        {
            TTypedColumnInfo columnInfo = GetColumn(ATableNumber, ASearchCriteria.fieldname);

            if (columnInfo.odbctype == OdbcType.VarChar)
            {
                return new System.Data.Odbc.OdbcParameter("", columnInfo.odbctype, columnInfo.length);
            }

            return new System.Data.Odbc.OdbcParameter("", columnInfo.odbctype);
        }

        /// create an odbc parameter for the given column
        public static OdbcParameter CreateOdbcParameter(short ATableNumber, Int32 AColumnNr)
        {
            TTypedColumnInfo columnInfo = TableInfo[ATableNumber].columns[AColumnNr];

            if ((columnInfo.odbctype == OdbcType.VarChar)
                || (columnInfo.odbctype == OdbcType.Text))
            {
                return new System.Data.Odbc.OdbcParameter("", columnInfo.odbctype, columnInfo.length);
            }

            return new System.Data.Odbc.OdbcParameter("", columnInfo.odbctype);
        }
    }
}