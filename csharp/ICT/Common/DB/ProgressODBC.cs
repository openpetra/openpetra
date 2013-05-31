//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
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
using System.Data.Common;
using System.Data.Odbc;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using Ict.Common;

namespace Ict.Common.DB
{
    /// <summary>
    /// this class allows access to Progress databases via ODBC
    /// </summary>
    public class TProgressODBC : IDataBaseRDBMS
    {
        /// <summary>
        /// Create an ODBC connection
        /// </summary>
        /// <param name="ADSN">The DSN defining the connection to the database server</param>
        /// <param name="APort">not in use</param>
        /// <param name="ADatabaseName">not in use</param>
        /// <param name="AUsername">odbc user name</param>
        /// <param name="APassword">The password for opening the database</param>
        /// <param name="AConnectionString">not in use</param>
        /// <param name="AStateChangeEventHandler">for connection state changes</param>
        /// <returns>the connection</returns>
        public IDbConnection GetConnection(String ADSN, String APort,
            String ADatabaseName,
            String AUsername, ref String APassword,
            ref String AConnectionString,
            StateChangeEventHandler AStateChangeEventHandler)
        {
            ArrayList ExceptionList;
            OdbcConnection TheConnection = null;

            if (AConnectionString == "")
            {
                AConnectionString = "DSN=" + ADSN + ";UID=" + AUsername + ";PWD=";
            }

            try
            {
                // Now try to connect to the DB
                TheConnection = new OdbcConnection();
                TheConnection.ConnectionString = AConnectionString + APassword;
            }
            catch (Exception exp)
            {
                ExceptionList = new ArrayList();
                ExceptionList.Add((("Error establishing a DB connection to: " + AConnectionString) + Environment.NewLine));
                ExceptionList.Add((("Exception thrown :- " + exp.ToString()) + Environment.NewLine));
                TLogging.Log(ExceptionList, true);
            }

            if (TheConnection != null)
            {
                ((OdbcConnection)TheConnection).StateChange += AStateChangeEventHandler;
            }

            FDBEncoding = System.Text.Encoding.Default;
            try
            {
                Int16 sqlClientCodePage = Convert.ToInt16(System.Environment.GetEnvironmentVariable("SQL_CLIENT_CHARSET"));
                FDBEncoding = System.Text.Encoding.GetEncoding(sqlClientCodePage);
            }
            catch (Exception)
            {
            }

            return TheConnection;
        }

        /// init the connection after it was opened
        public void InitConnection(IDbConnection AConnection)
        {
        }

        /// <summary>
        /// format an error message for exception of type OdbcException
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="AErrorMessage"></param>
        /// <returns>true if this is an OdbcException</returns>
        public bool LogException(Exception AException, ref string AErrorMessage)
        {
            if (AException is OdbcException)
            {
                for (int Counter = 0; Counter <= ((OdbcException)AException).Errors.Count - 1; Counter += 1)
                {
                    AErrorMessage = AErrorMessage + "Index #" + Counter.ToString() + Environment.NewLine +
                                    "Message: " + ((OdbcException)AException).Errors[Counter].Message + Environment.NewLine +
                                    "NativeError: " + ((OdbcException)AException).Errors[Counter].NativeError.ToString() + Environment.NewLine +
                                    "Source: " + ((OdbcException)AException).Errors[Counter].Source + Environment.NewLine +
                                    "SQL: " + ((OdbcException)AException).Errors[Counter].SQLState + Environment.NewLine;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// format the sql query so that it works for Progress ODBC
        /// see also the comments for TDataBase.FormatQueryRDBMSSpecific
        /// </summary>
        /// <param name="ASqlQuery"></param>
        /// <returns></returns>
        public String FormatQueryRDBMSSpecific(String ASqlQuery)
        {
            string ReturnValue = ASqlQuery;

            ReturnValue = ReturnValue.Replace("PUB_", "PUB.");
            ReturnValue = ReturnValue.Replace("pub_", "pub.");
            ReturnValue = ReturnValue.Replace(" = true", " = 1");
            ReturnValue = ReturnValue.Replace(" = TRUE", " = 1");
            ReturnValue = ReturnValue.Replace(" = false", " = 0");
            ReturnValue = ReturnValue.Replace(" = FALSE", " = 0");
            ReturnValue = ReturnValue.Replace("\"", "'");

            Match m = Regex.Match(ReturnValue, "#([0-9][0-9][0-9][0-9])-([0-9][0-9])-([0-9][0-9])#");

            while (m.Success)
            {
                // needs to be 'MM/dd/yyyy'
                ReturnValue = ReturnValue.Replace("#" + m.Groups[1] + "-" + m.Groups[2] + "-" + m.Groups[3] + "#",
                    "'" + m.Groups[2] + "/" + m.Groups[3] + "/" + m.Groups[1] + "'");
                m = Regex.Match(ReturnValue, "#([0-9][0-9][0-9][0-9])-([0-9][0-9])-([0-9][0-9])#");
            }

            // some special cases require double quotes
            ReturnValue = ReturnValue.Replace("'_Sequence'", "\"_Sequence\"");
            ReturnValue = ReturnValue.Replace("'_Seq-Name'", "\"_Seq-Name\"");

            return ReturnValue;
        }

        /// <summary>
        /// copy the ODBC parameters, but apply unicode conversion
        /// </summary>
        /// <param name="AParameterArray">Array of DbParameter that is to be converted.</param>
        /// <param name="ASqlStatement">SQL Statement will stay the same.</param>
        /// <returns>modified array of ODBCParameters (unicode)</returns>
        public DbParameter[] ConvertOdbcParameters(DbParameter[] AParameterArray, ref string ASqlStatement)
        {
            OdbcParameter[] ReturnValue = new OdbcParameter[AParameterArray.Length];
            OdbcParameter[] ArrayOdbc = (OdbcParameter[])AParameterArray;

            // QUESTION: is this only called once, or do we have a problem with ConvertFrmUnicode to be called twice?

            for (int Counter = 0; Counter < AParameterArray.Length; Counter++)
            {
                ReturnValue[Counter] = new OdbcParameter(ArrayOdbc[Counter].ParameterName, ArrayOdbc[Counter].OdbcType);

                if (ArrayOdbc[Counter].OdbcType == OdbcType.VarChar)
                {
                    ReturnValue[Counter].Value = ConvertFromUnicode(Convert.ToString(ArrayOdbc[Counter].Value));
                }
                else
                {
                    ReturnValue[Counter].Value = ArrayOdbc[Counter].Value;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// create a IDbCommand object
        /// this formats the sql query for Progress ODBC, and transforms the parameters
        /// </summary>
        /// <param name="ACommandText"></param>
        /// <param name="AConnection"></param>
        /// <param name="AParametersArray"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        public IDbCommand NewCommand(ref string ACommandText, IDbConnection AConnection, DbParameter[] AParametersArray, TDBTransaction ATransaction)
        {
            IDbCommand ObjReturn = null;

            ACommandText = FormatQueryRDBMSSpecific(ACommandText);

            if (ATransaction == null)
            {
                ObjReturn = new OdbcCommand(ACommandText, (OdbcConnection)AConnection);
            }
            else
            {
                ObjReturn = new OdbcCommand(ACommandText, (OdbcConnection)AConnection, (OdbcTransaction)ATransaction.WrappedTransaction);
            }

            if (AParametersArray != null)
            {
                // add parameters
                foreach (DbParameter param in AParametersArray)
                {
                    ObjReturn.Parameters.Add(param);
                }
            }

            return ObjReturn;
        }

        /// <summary>
        /// create an IDbDataAdapter for ODBC
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter NewAdapter()
        {
            IDbDataAdapter TheAdapter = new OdbcDataAdapter();

            return TheAdapter;
        }

        /// <summary>
        /// fill an IDbDataAdapter that was created with NewAdapter
        /// </summary>
        /// <param name="TheAdapter"></param>
        /// <param name="AFillDataSet"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        /// <param name="ADataTableName"></param>
        public void FillAdapter(IDbDataAdapter TheAdapter,
            ref DataSet AFillDataSet,
            Int32 AStartRecord,
            Int32 AMaxRecords,
            string ADataTableName)
        {
            ((OdbcDataAdapter)TheAdapter).Fill(AFillDataSet, AStartRecord, AMaxRecords, ADataTableName);
            ConvertToUnicode(AFillDataSet.Tables[ADataTableName].Rows);
        }

        /// <summary>
        /// overload of FillAdapter, just for one table
        /// IDbDataAdapter was created with NewAdapter
        /// </summary>
        /// <param name="TheAdapter"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        /// <param name="AFillDataTable"></param>
        public void FillAdapter(IDbDataAdapter TheAdapter,
            ref DataTable AFillDataTable,
            Int32 AStartRecord,
            Int32 AMaxRecords)
        {
            ((OdbcDataAdapter)TheAdapter).Fill(AFillDataTable);
            ConvertToUnicode(AFillDataTable.Rows);
        }

        /// <summary>Is used to know the codepage of the database; is retrieved from the environment variable SQL_CLIENT_CHARSET</summary>
        private System.Text.Encoding FDBEncoding;

        /// <summary>
        /// Convert string from the codepage of the database to proper Unicode
        /// using the SQL_CLIENT_CHARSET environment variable, or the default Windows ANSI codepage
        ///
        /// </summary>
        /// <param name="s">the original string, as it is retrieved from the database</param>
        /// <returns>the string correctly converted to Unicode
        /// </returns>
        public String ConvertToUnicode(String s)
        {
            byte[] bytes;
            bytes = Encoding.Default.GetBytes(s);
            String result = FDBEncoding.GetString(bytes);

            // deal with a Mono bug: see http://bugs.om.org/petra/show_bug.cgi?id=762
            result = result.Replace(new string((char)0, 1), "");
            return result;
        }

        /// <summary>
        /// Convert string from proper Unicode to the codepage of the database
        /// using the SQL_CLIENT_CHARSET environment variable, or the default Windows ANSI codepage
        ///
        /// </summary>
        /// <param name="s">the Unicode string from the GUI</param>
        /// <returns>string to be stored in the database
        /// </returns>
        public String ConvertFromUnicode(String s)
        {
            byte[] bytes;
            bytes = FDBEncoding.GetBytes(s);
            return Encoding.Default.GetString(bytes);
        }

        /// <summary>
        /// Convert all the string values in a result set from the codepage of the database to proper Unicode
        /// using the SQL_CLIENT_CHARSET environment variable, or the default Windows ANSI codepage
        /// </summary>
        /// <param name="rows">The DataRowCollection that needs to be converted
        /// </param>
        /// <returns>void</returns>
        public void ConvertToUnicode(DataRowCollection rows)
        {
            foreach (DataRow row in rows)
            {
                foreach (DataColumn column in row.Table.Columns)
                {
                    if (row[column].GetType() == typeof(string))
                    {
                        row[column] = ConvertToUnicode(Convert.ToString(row[column]));
                    }
                }
            }
        }

        /// <summary>
        /// some databases have some problems with certain Isolation levels
        /// </summary>
        /// <param name="AIsolationLevel"></param>
        /// <returns>true if isolation level was modified</returns>
        public bool AdjustIsolationLevel(ref IsolationLevel AIsolationLevel)
        {
            // no problem with isolation levels
            return false;
        }

        /// <summary>
        /// Returns the next sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <param name="ADatabase">the database object that can be used for querying</param>
        /// <param name="AConnection"></param>
        /// <returns>Sequence Value.</returns>
        public System.Int64 GetNextSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase, IDbConnection AConnection)
        {
            DataTable table = ADatabase.SelectDT(
                "SELECT PUB." + ASequenceName + ".NEXTVAL FROM PUB.\"_Sequence\" WHERE \"_Seq-Name\" = '" + ASequenceName + "'",
                "sequence",
                ATransaction);

            if (table.Rows.Count > 0)
            {
                return Convert.ToInt64(table.Rows[0][0]);
            }

            return -1;
        }

        /// <summary>
        /// Returns the current sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <param name="ADatabase">the database object that can be used for querying</param>
        /// <param name="AConnection"></param>
        /// <returns>Sequence Value.</returns>
        public System.Int64 GetCurrentSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase, IDbConnection AConnection)
        {
            DataTable table = ADatabase.SelectDT(
                "SELECT PUB." + ASequenceName + ".CURRVAL FROM PUB.\"_Sequence\" WHERE \"_Seq-Name\" = \"" + ASequenceName + "\"",
                "sequence",
                ATransaction);

            if (table.Rows.Count > 0)
            {
                return Convert.ToInt64(table.Rows[0][0]);
            }

            return -1;
        }

        /// <summary>
        /// restart a sequence with the given value has not been implemented
        /// </summary>
        public void RestartSequence(String ASequenceName,
            TDBTransaction ATransaction,
            TDataBase ADatabase,
            IDbConnection AConnection,
            Int64 ARestartValue)
        {
            // not implemented
        }

        /// Updating of a Progress or ODBC database has not been implemented yet, need to do this still manually
        public void UpdateDatabase(TFileVersionInfo ADBVersion, TFileVersionInfo AExeVersion,
            string AHostOrFile, string ADatabasePort, string ADatabaseName, string AUsername, string APassword)
        {
            throw new Exception(
                "Cannot connect to old database, please restore the latest clean demo database or run nant patchDatabase");
        }
    }
}