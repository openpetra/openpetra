//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2017 by OM International
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
using System.Collections;
using System.Data.Odbc;
using System.Data;

using Ict.Common;
using Ict.Common.DB;

namespace Ict.Common.DB.DBCaching
{
    /// <summary>
    /// This class allows to cache common SQL queries.
    /// <para>
    /// FIXME This class does not operate in a thread-safe manner - it needs to be
    /// made thread-safe by using locks anywhere where the two internally-held ArrayLists are accessed!!!
    /// </para>
    /// </summary>
    /// <remarks>
    /// The queries are just stored as strings and compared as strings.
    /// </remarks>
    public class TSQLCache
    {
        private ArrayList storedDataSet;
        private ArrayList storedSQLQuery;
        private ArrayList storedParameters;

        /// <summary>
        /// Provides a simple method for caching datasets for queries that are
        /// called again and again.
        /// </summary>
        /// <remarks>The queries are just stored as strings and compared as strings.</remarks>
        public TSQLCache() : base()
        {
            storedDataSet = new ArrayList();
            storedSQLQuery = new ArrayList();
            storedParameters = new ArrayList();
        }

        /// <summary>
        /// checks if the result for this query is already cached.
        /// If not, the result is retrieved from the database.
        /// The result is added to the cache, and the result is returned
        /// as a DataSet from the cache.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="AParameters">odbc parameters</param>
        /// <param name="ATable">can already have some prepared columns; optional parameter, can be null
        /// </param>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null (default = null). If null
        /// gets passed then the Method executes DB commands with the 'globally available'
        /// <see cref="DBAccess.GDBAccessObj" /> instance, otherwise with the instance that gets passed in with this
        /// Argument!</param>
        /// <returns>void</returns>
        public DataSet GetDataSet(String sql, OdbcParameter[] AParameters, DataTable ATable, TDataBase ADataBase = null)
        {
            TDataBase DBAccessObj = DBAccess.GetDBAccessObj(ADataBase);
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            DataSet newDataSet;

            int counter = 0;

            foreach (string sqlstr in storedSQLQuery)
            {
                if (sqlstr == sql)
                {
                    bool SameParameters = false;

                    if (AParameters == null && storedParameters[counter] == null)
                    {
                        SameParameters = true;
                    }
                    else if (AParameters == null || storedParameters[counter] == null)
                    {
                        SameParameters = false;
                    }
                    else if (AParameters.Length != ((OdbcParameter[])storedParameters[counter]).Length)
                    {
                        SameParameters = false;
                    }
                    else
                    {
                        int countParam = 0;

                        SameParameters = true;

                        foreach (OdbcParameter p in AParameters)
                        {
                            if (p.OdbcType != ((OdbcParameter[])storedParameters[counter])[countParam].OdbcType)
                            {
                                SameParameters = false;
                            }
                            else
                            {
                                SameParameters = (p.Value.ToString() == ((OdbcParameter[])storedParameters[counter])[countParam].ToString());
                            }
                            countParam ++;
                        }
                    }

                    if (SameParameters)
                    {
                        // create a clone of the result, so that the returned datasets
                        // can be treated separately
                        return ((DataSet)storedDataSet[counter]).Copy();
                    }
                }

                counter++;
            }

            try
            {
                ReadTransaction = DBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                if (ATable == null)
                {
                    newDataSet = DBAccessObj.Select(sql, "Cache", ReadTransaction, AParameters);
                }
                else
                {
                    newDataSet = new DataSet();
                    newDataSet.Tables.Add(ATable);
                    ATable.TableName = "Cache";
                    DBAccessObj.Select(newDataSet, sql, "Cache", ReadTransaction, AParameters);
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccessObj.CommitTransaction();
                }
            }
            storedDataSet.Add(newDataSet);
            storedSQLQuery.Add(sql);
            storedParameters.Add(AParameters);

            // return a copy, so that changes to the dataset don't affect the stored copy.
            return newDataSet.Copy();
        }

        /// <summary>
        /// checks if the result for this query is already cached.
        /// If not, the result is retrieved from the database.
        /// The result is added to the cache, and the result is returned
        /// as a DataSet from the cache.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="AParameters">odbc parameters</param>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null (default = null). If null
        /// gets passed then the Method executes DB commands with the 'globally available'
        /// <see cref="DBAccess.GDBAccessObj" /> instance, otherwise with the instance that gets passed in with this
        /// Argument!</param>
        /// <returns></returns>
        public DataSet GetDataSet(String sql, OdbcParameter[] AParameters, TDataBase ADataBase = null)
        {
            return GetDataSet(sql, AParameters, null, ADataBase);
        }

        /// <summary>
        /// checks if the result for this query is already cached.
        /// If not, the result is retrieved from the database.
        /// The result is added to the cache, and the result is returned
        /// as a DataTable from the cache.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="AParameters">odbc parameters</param>
        /// <param name="ATable">can already have some prepared columns; optional parameter, can be nil
        /// </param>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null (default = null). If null
        /// gets passed then the Method executes DB commands with the 'globally available'
        /// <see cref="DBAccess.GDBAccessObj" /> instance, otherwise with the instance that gets passed in with this
        /// Argument!</param>
        /// <returns>void</returns>
        public DataTable GetDataTable(string sql, OdbcParameter[] AParameters, DataTable ATable, TDataBase ADataBase = null)
        {
            return GetDataSet(sql, AParameters, ATable, ADataBase).Tables["Cache"];
        }

        /// <summary>
        /// checks if the result for this query is already cached.
        /// If not, the result is retrieved from the database.
        /// The result is added to the cache, and the result is returned
        /// as a DataTable from the cache.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="AParameters">odbc parameters</param>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null (default = null). If null
        /// gets passed then the Method executes DB commands with the 'globally available'
        /// <see cref="DBAccess.GDBAccessObj" /> instance, otherwise with the instance that gets passed in with this
        /// Argument!</param>
        /// <returns></returns>
        public DataTable GetDataTable(String sql, OdbcParameter[] AParameters = null, TDataBase ADataBase = null)
        {
            return GetDataTable(sql, AParameters, null, ADataBase);
        }

        /// <summary>
        /// overloaded method
        /// </summary>
        public DataTable GetDataTable(String sql, TDataBase ADataBase)
        {
            return GetDataTable(sql, null, null, ADataBase);
        }

        /// <summary>
        /// checks if the result for this query is already cached.
        /// If not, the result is retrieved from the database.
        /// The result is added to the cache, and the result is returned
        /// as a ArrayList from the cache.
        /// The ArrayList consists of the strings
        /// of the first column of the first table in the dataset.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="AParameters">odbc parameters</param>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null (default = null). If null
        /// gets passed then the Method executes DB commands with the 'globally available'
        /// <see cref="DBAccess.GDBAccessObj" /> instance, otherwise with the instance that gets passed in with this
        /// Argument!</param>
        /// <returns>void</returns>
        public ArrayList GetStringList(String sql, OdbcParameter[] AParameters = null, TDataBase ADataBase = null)
        {
            ArrayList ReturnValue = new ArrayList();
            DataSet ds = GetDataSet(sql, AParameters, ADataBase);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ReturnValue.Add(Convert.ToString(row[0]));
            }

            return ReturnValue;
        }

        /// <summary>
        /// overloaded method
        /// </summary>
        public ArrayList GetStringList(String sql, TDataBase ADataBase)
        {
            return GetStringList(sql, null, ADataBase);
        }

        /// <summary>
        /// remove all cached resultsets which have the table in their sql statement.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InvalidateTable(String tablename)
        {
            int counter = 0;

            while (counter < storedSQLQuery.Count)
            {
                String sqlstr = (String)storedSQLQuery[counter];

                if (sqlstr.IndexOf(tablename) != -1)
                {
                    storedDataSet.RemoveAt(counter);
                    storedSQLQuery.RemoveAt(counter);
                }
                else
                {
                    counter++;
                }
            }
        }

        /// <summary>
        /// remove all cached resultsets
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Invalidate()
        {
            storedDataSet.Clear();
            storedSQLQuery.Clear();
        }
    }
}