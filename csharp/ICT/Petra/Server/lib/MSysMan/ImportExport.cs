//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
//
// Copyright 2004-2016 by OM International
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
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Text;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MSysMan.ImportExport.WebConnectors;

namespace Ict.Petra.Server.MSysMan.ImportExport.WebConnectors
{
    /// <summary>
    /// import and export of all data in this database
    /// </summary>
    public class TImportExportWebConnector
    {
        /// <summary>
        /// return a compressed XmlDocument with all data in this database;
        /// this is useful to convert data between different database systems, etc
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("SYSMAN")]
        public static string ExportAllTables()
        {
            TDataBase DBConnectionObj = null;
            TDBTransaction ReadTransaction = null;
            XmlDocument OpenPetraData = TYml2Xml.CreateXmlDocument();
            XmlNode rootNode = OpenPetraData.FirstChild.NextSibling;

            Assembly TypedTablesAssembly = Assembly.LoadFrom(
                TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + "Ict.Petra.Shared.lib.data.dll");

            try
            {
                // Open a separate DB Connection for the exporting of the data...
                DBConnectionObj = DBAccess.SimpleEstablishDBConnection("ExportAllTables");

                // ...and start a DB Transaction on that separate DB Connection
                ReadTransaction = DBConnectionObj.BeginTransaction(IsolationLevel.Serializable, 0, "ExportAllTables");

                ExportTables(rootNode, "MSysMan", "", TypedTablesAssembly, ReadTransaction);
                ExportTables(rootNode, "MCommon", "", TypedTablesAssembly, ReadTransaction);
                ExportTables(rootNode, "MPartner", "Partner", TypedTablesAssembly, ReadTransaction);
                ExportTables(rootNode, "MPartner", "Mailroom", TypedTablesAssembly, ReadTransaction);
                ExportTables(rootNode, "MFinance", "Account", TypedTablesAssembly, ReadTransaction);
                ExportTables(rootNode, "MFinance", "Gift", TypedTablesAssembly, ReadTransaction);
                ExportTables(rootNode, "MFinance", "AP", TypedTablesAssembly, ReadTransaction);
                ExportTables(rootNode, "MFinance", "AR", TypedTablesAssembly, ReadTransaction);
                ExportTables(rootNode, "MPersonnel", "Personnel", TypedTablesAssembly, ReadTransaction);
                ExportTables(rootNode, "MPersonnel", "Units", TypedTablesAssembly, ReadTransaction);
                ExportTables(rootNode, "MConference", "", TypedTablesAssembly, ReadTransaction);
                ExportTables(rootNode, "MHospitality", "", TypedTablesAssembly, ReadTransaction);

                ExportSequences(rootNode, ReadTransaction);
            }
            finally
            {
                if (DBConnectionObj != null)
                {
                    DBConnectionObj.RollbackTransaction();

                    DBConnectionObj.CloseDBConnection();
                }
            }

            return TYml2Xml.Xml2YmlGz(OpenPetraData);
        }

        /// <summary>
        /// export one module at the time
        /// </summary>
        /// <param name="ARootNode"></param>
        /// <param name="AModuleName"></param>
        /// <param name="ASubModuleName">can be empty if there is no submodule</param>
        /// <param name="ATypedTablesAssembly">the assembly with all types of the tables in the database</param>
        /// <param name="AReadTransaction">Instantiated DB Transaction.</param>
        private static void ExportTables(XmlNode ARootNode, string AModuleName, string ASubModuleName,
            Assembly ATypedTablesAssembly, TDBTransaction AReadTransaction)
        {
            XmlElement moduleNode = ARootNode.OwnerDocument.CreateElement(AModuleName + ASubModuleName);

            ARootNode.AppendChild(moduleNode);

            string namespaceName = "Ict.Petra.Shared." + AModuleName;

            if (ASubModuleName.Length > 0)
            {
                namespaceName += "." + ASubModuleName;
            }

            namespaceName += ".Data";

            SortedList <string, Type>SortedTypes = new SortedList <string, Type>();

            foreach (Type type in ATypedTablesAssembly.GetTypes())
            {
                if ((type.Namespace == namespaceName) && type.Name.EndsWith("Table"))
                {
                    SortedTypes.Add(type.Name, type);
                }
            }

            // export the tables, ordered by name
            foreach (string typeName in SortedTypes.Keys)
            {
                ExportTable(moduleNode, ATypedTablesAssembly, SortedTypes[typeName], AReadTransaction);
            }
        }

        private static void ExportTable(XmlNode AModuleNode, Assembly AAsm, Type ATableType, TDBTransaction AReadTransaction)
        {
            if (ATableType.Name.Contains("TDS"))
            {
                // this is probably a table in a typed dataset
                return;
            }

            MethodInfo mi = ATableType.GetMethod("GetTableDBName", BindingFlags.Static | BindingFlags.Public);
            string TableDBName = mi.Invoke(null, null).ToString();
            DataTable table = AReadTransaction.DataBaseObj.SelectDT("Select * from " + TableDBName, TableDBName, AReadTransaction);

            XmlElement tableNode = AModuleNode.OwnerDocument.CreateElement(ATableType.Name);

            AModuleNode.AppendChild(tableNode);

            if (table.Rows.Count == 0)
            {
                return;
            }

            // for SQLite the table is not sorted by primary key. Therefore do it manually
            DataView v = table.DefaultView;
            DataTable t = (DataTable)AAsm.CreateInstance(ATableType.Namespace + "." + ATableType.Name);
            string sortOrderPrimaryKey = String.Empty;

            foreach (DataColumn c in t.PrimaryKey)
            {
                if (sortOrderPrimaryKey.Length > 0)
                {
                    sortOrderPrimaryKey += ",";
                }

                sortOrderPrimaryKey += c.ColumnName;
            }

            v.Sort = sortOrderPrimaryKey;

            Int32 RowCounter = 0;

            ConvertColumnNames(table.Columns);

            // TODO: automatically filter column values that are the same and group the data?
            foreach (DataRowView rv in v)
            {
                DataRow row = rv.Row;
                RowCounter++;
                XmlElement rowNode = AModuleNode.OwnerDocument.CreateElement("Row" + RowCounter.ToString());
                tableNode.AppendChild(rowNode);

                foreach (DataColumn col in table.Columns)
                {
                    if ((row[col].GetType() != typeof(DBNull)) && (col.ColumnName != "ModificationId"))
                    {
                        if (col.DataType == typeof(DateTime))
                        {
                            DateTime d = Convert.ToDateTime(row[col]);

                            if (d.TimeOfDay == TimeSpan.Zero)
                            {
                                rowNode.SetAttribute(col.ColumnName, d.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                rowNode.SetAttribute(col.ColumnName, d.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                        else if ((col.DataType == typeof(double)) || (col.DataType == typeof(decimal)))
                        {
                            // store decimals always with decimal point, no thousands separator
                            decimal dval = Convert.ToDecimal(row[col]);
                            rowNode.SetAttribute(col.ColumnName, dval.ToString("0.########", CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            rowNode.SetAttribute(col.ColumnName, row[col].ToString().Replace("\"", "&quot;"));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// export the sequences
        /// </summary>
        /// <param name="ARootNode"></param>
        /// <param name="AReadTransaction">Instantiated DB Transaction.</param>
        private static void ExportSequences(XmlNode ARootNode, TDBTransaction AReadTransaction)
        {
            XmlElement sequencesNode = ARootNode.OwnerDocument.CreateElement("Sequences");

            ARootNode.AppendChild(sequencesNode);

            foreach (string seq in TTableList.GetDBSequenceNames())
            {
                XmlElement sequenceNode = ARootNode.OwnerDocument.CreateElement(StringHelper.UpperCamelCase(seq, false, false));
                sequencesNode.AppendChild(sequenceNode);

                sequenceNode.SetAttribute("value", AReadTransaction.DataBaseObj.GetCurrentSequenceValue(seq, AReadTransaction).ToString());
            }
        }

        /// <summary>
        /// this will reset the current database, and load the data from the given XmlDocument
        /// </summary>
        /// <param name="AZippedNewDatabaseData">zipped YML</param>
        /// <returns></returns>
        [RequireModulePermission("SYSMAN")]
        public static bool ResetDatabase(string AZippedNewDatabaseData)
        {
            TDataBase DBConnectionObj = null;

            List <string>tables = TTableList.GetDBNames();
            bool SubmissionResult = false;
            TDBTransaction ReadWriteTransaction = null;

            string ClientID = "ClientID";

            try
            {
                ClientID = DomainManager.GClientID.ToString();
            }
            catch (Exception)
            {
            }

            TProgressTracker.InitProgressTracker(ClientID,
                Catalog.GetString("Restoring Database..."),
                tables.Count + 3);

            try
            {
                // Open a separate DB Connection for the importing of the data...
                DBConnectionObj = DBAccess.SimpleEstablishDBConnection("ExportAllTables");

                // ...and start a DB Transaction on that separate DB Connection
                DBConnectionObj.BeginAutoTransaction(IsolationLevel.Serializable, ref ReadWriteTransaction, ref SubmissionResult, delegate
                    {
                        try
                        {
                            tables.Reverse();

                            TProgressTracker.SetCurrentState(ClientID,
                                Catalog.GetString("Deleting current data..."),
                                0);

                            foreach (string table in tables)
                            {
                                ReadWriteTransaction.DataBaseObj.ExecuteNonQuery("DELETE FROM pub_" + table, ReadWriteTransaction);
                            }

                            if (TProgressTracker.GetCurrentState(ClientID).CancelJob == true)
                            {
                                TProgressTracker.FinishJob(ClientID);

                                // As SubmissionResult is still false, a DB Transaction Rollback will get
                                // executed automatically and the Method will be exited with return value 'false'!
                                return;
                            }

                            TSimpleYmlParser ymlParser = new TSimpleYmlParser(PackTools.UnzipString(AZippedNewDatabaseData));

                            ymlParser.ParseCaptions();

                            tables.Reverse();

                            TProgressTracker.SetCurrentState(ClientID,
                                Catalog.GetString("Loading initial tables..."),
                                1);

                            // one transaction to import the user table and user permissions. otherwise logging in will not be possible if other import fails?
                            bool success = true;
                            success = success && LoadTable("s_user", ymlParser, ReadWriteTransaction);
                            success = success && LoadTable("s_module", ymlParser, ReadWriteTransaction);
                            success = success && LoadTable("s_user_module_access_permission", ymlParser, ReadWriteTransaction);
                            success = success && LoadTable("s_system_defaults", ymlParser, ReadWriteTransaction);
                            success = success && LoadTable("s_system_status", ymlParser, ReadWriteTransaction);

                            // make sure we have the correct database version
                            TFileVersionInfo serverExeInfo = new TFileVersionInfo(TSrvSetting.ApplicationVersion);
                            ReadWriteTransaction.DataBaseObj.ExecuteNonQuery(String.Format(
                                    "UPDATE PUB_s_system_defaults SET s_default_value_c = '{0}' WHERE s_default_code_c = 'CurrentDatabaseVersion'",
                                    serverExeInfo.ToString()), ReadWriteTransaction);

                            if (!success)
                            {
                                // As SubmissionResult is still TSubmitChangesResult.scrError, a DB Transaction Rollback will get
                                // executed automatically and the Method will be exited with return value 'false'!
                                return;
                            }

                            if (TProgressTracker.GetCurrentState(ClientID).CancelJob == true)
                            {
                                TProgressTracker.FinishJob(ClientID);

                                // As SubmissionResult is still false, a DB Transaction Rollback will get
                                // executed automatically and the Method will be exited with return value 'false'!
                                return;
                            }

                            tables.Remove("s_user");
                            tables.Remove("s_module");
                            tables.Remove("s_user_module_access_permission");
                            tables.Remove("s_system_defaults");
                            tables.Remove("s_system_status");

                            FCurrencyPerLedger = new SortedList <int, string>();

                            int tableCounter = 2;

                            foreach (string table in tables)
                            {
                                TProgressTracker.SetCurrentState(ClientID,
                                    String.Format(Catalog.GetString("Loading Table {0}..."), table),
                                    tableCounter);

                                tableCounter++;

                                if (TProgressTracker.GetCurrentState(ClientID).CancelJob == true)
                                {
                                    TProgressTracker.FinishJob(ClientID);

                                    // As SubmissionResult is still false, a DB Transaction Rollback will get
                                    // executed automatically and the Method will be exited with return value 'false'!
                                    return;
                                }

                                LoadTable(table, ymlParser, ReadWriteTransaction);
                            }

                            TProgressTracker.SetCurrentState(ClientID,
                                Catalog.GetString("Loading Sequences..."),
                                tables.Count + 5 + 3);

                            // set sequences appropriately, not lagging behind the imported data
                            foreach (string seq in TTableList.GetDBSequenceNames())
                            {
                                LoadSequence(seq, ymlParser, ReadWriteTransaction);
                            }

                            TProgressTracker.SetCurrentState(ClientID,
                                Catalog.GetString("Finishing Restore..."),
                                tables.Count + 5 + 4);

                            SubmissionResult = true;

                            // reset all cached tables
                            TCacheableTablesManager.GCacheableTablesManager.MarkAllCachedTableNeedsRefreshing();

                            TProgressTracker.FinishJob(ClientID);
                        }
                        catch (Exception e)
                        {
                            TLogging.Log("Problem in ResetDatabase: " + e.ToString());
                            TLogging.LogStackTrace(TLoggingType.ToLogfile);

                            throw;
                        }
                    });
            }
            finally
            {
                if (DBConnectionObj != null)
                {
                    DBConnectionObj.CloseDBConnection();
                }
            }

            return SubmissionResult;
        }

        private static bool LoadSequence(string ASequenceName, TSimpleYmlParser AYmlParser, TDBTransaction AReadWriteTransaction)
        {
            if (!AYmlParser.StartParseList("Sequences"))
            {
                return false;
            }

            SortedList <string, string>RowDetails;

            while ((RowDetails = AYmlParser.GetNextLineAttributes()) != null)
            {
                if (RowDetails["RowName"] == StringHelper.UpperCamelCase(ASequenceName, false, false))
                {
                    AReadWriteTransaction.DataBaseObj.RestartSequence(
                        ASequenceName,
                        AReadWriteTransaction,
                        Convert.ToInt64(RowDetails["value"]));

                    return true;
                }
            }

            return false;
        }

        private static SortedList <Int32, string>FCurrencyPerLedger = new SortedList <int, string>();

        private static bool LoadTable(string ATableName, TSimpleYmlParser AYmlParser, TDBTransaction AReadWriteTransaction)
        {
            if (!AYmlParser.StartParseList(StringHelper.UpperCamelCase(ATableName, false, false) + "Table"))
            {
                // TLogging.Log("empty table " + ATableName + " " + StringHelper.UpperCamelCase(ATableName, false, false) + "Table");
                return false;
            }

            DataTable table = AReadWriteTransaction.DataBaseObj.SelectDT("Select * from " + ATableName, ATableName, AReadWriteTransaction);
            List <OdbcParameter>Parameters = new List <OdbcParameter>();

            string OrigInsertStatement = "INSERT INTO pub_" + ATableName + "() VALUES ";
            StringBuilder InsertStatement = new StringBuilder(OrigInsertStatement);

            ConvertColumnNames(table.Columns);

            SortedList <string, string>RowDetails;
            int count = 0;

            while ((RowDetails = AYmlParser.GetNextLineAttributes()) != null)
            {
                count++;

                if (count != 1)
                {
                    if ((CommonTypes.ParseDBType(AReadWriteTransaction.DataBaseObj.DBType) == TDBType.SQLite) || ((count % 500) == 0))
                    {
                        // SQLite does not support INSERT of several rows at the same time
                        try
                        {
                            AReadWriteTransaction.DataBaseObj.ExecuteNonQuery(InsertStatement.ToString(), AReadWriteTransaction, Parameters.ToArray());
                        }
                        catch (Exception e)
                        {
                            TLogging.Log("error in ResetDatabase, LoadTable " + ATableName + ":" + e.Message);
                            throw;
                        }

                        InsertStatement = new StringBuilder(OrigInsertStatement);
                        Parameters = new List <OdbcParameter>();
                    }
                    else
                    {
                        InsertStatement.Append(",");
                    }
                }

                InsertStatement.Append("(");

                // needed for workaround for version 0.2.24, AAPDocument.BaseCurrency and AAPPayment.BaseCurrency;
                if (ATableName == "a_ledger")
                {
                    FCurrencyPerLedger.Add(Convert.ToInt32(RowDetails["LedgerNumber"]), RowDetails["BaseCurrency"]);
                }

                // workaround for 2015.09: a_account_hierarchy_detail.a_report_order_i must not be NULL
                if (ATableName == "a_account_hierarchy_detail")
                {
                    if (!RowDetails.ContainsKey("ReportOrder") || RowDetails["ReportOrder"] == string.Empty)
                    {
                        RowDetails["ReportOrder"] = "-1";
                    }
                }

                if (ATableName == "a_gift")
                {
                    if (!RowDetails.ContainsKey("LinkToPreviousGift") || RowDetails["LinkToPreviousGift"] == string.Empty)
                    {
                        RowDetails["LinkToPreviousGift"] = "false";
                    }
                    if (!RowDetails.ContainsKey("PrintReceipt") || RowDetails["PrintReceipt"] == string.Empty)
                    {
                        RowDetails["PrintReceipt"] = "true";
                    }
                }

                if (ATableName == "a_recurring_transaction")
                {
                    if (!RowDetails.ContainsKey("Reference") || RowDetails["Reference"] == string.Empty)
                    {
                        RowDetails["Reference"] = "EMPTY";
                    }
                }

                if (ATableName == "a_transaction")
                {
                    if (!RowDetails.ContainsKey("Reference") || RowDetails["Reference"] == string.Empty)
                    {
                        RowDetails["Reference"] = "EMPTY";
                    }
                }

                bool firstColumn = true;

                foreach (DataColumn col in table.Columns)
                {
                    if (!firstColumn)
                    {
                        InsertStatement.Append(",");
                    }

                    firstColumn = false;

                    if (RowDetails.ContainsKey(col.ColumnName))
                    {
                        string strValue = RowDetails[col.ColumnName];

                        if (col.DataType == typeof(DateTime))
                        {
                            OdbcParameter p;

                            if (strValue.Length == "yyyy-MM-dd".Length)
                            {
                                p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.Date);
                                p.Value = DateTime.ParseExact(strValue, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.DateTime);
                                p.Value = DateTime.ParseExact(strValue, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            }

                            Parameters.Add(p);
                        }
                        else if (col.DataType == typeof(String))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.VarChar);
                            p.Value = strValue.Replace("&quot;", "\"");
                            Parameters.Add(p);
                        }
                        else if (col.DataType == typeof(Int32))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.Int);
                            p.Value = Convert.ToInt32(strValue);
                            Parameters.Add(p);
                        }
                        else if (col.DataType == typeof(Int64))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.Decimal);
                            p.Value = Convert.ToInt64(strValue);
                            Parameters.Add(p);
                        }
                        else if (col.DataType == typeof(double))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.Decimal);
                            p.Value = Convert.ToDouble(strValue, CultureInfo.InvariantCulture);
                            Parameters.Add(p);
                        }
                        else if (col.DataType == typeof(bool))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.Bit);
                            p.Value = Convert.ToBoolean(strValue);
                            Parameters.Add(p);
                        }
                        else if (col.DataType == typeof(Decimal))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.Decimal);
                            p.Value = Convert.ToDecimal(strValue, CultureInfo.InvariantCulture);
                            Parameters.Add(p);
                        }
                        else
                        {
                            // should not get here?
                            throw new Exception("error in ResetDatabase, LoadTable: " + col.DataType.ToString() + " has not yet been implemented");
                        }

                        InsertStatement.Append("?");
                    }
                    else
                    {
                        // the following statements are for the demo databases generated before version 0.2.24.
                        // CurrencyCode was added to a_ap_document and a_ap_payment.
                        // it is impossible during the load, to get the correct currencycode, via the supplier, because a_ap_supplier is loaded after a_ap_document.
                        // as a temporary workaround, and because we are still in Alpha, we are using the Base currency of the ledger
                        if ((ATableName == "a_ap_document") && (col.ColumnName == "CurrencyCode"))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.VarChar);
                            p.Value = FCurrencyPerLedger[Convert.ToInt32(RowDetails["LedgerNumber"])];
                            Parameters.Add(p);
                            InsertStatement.Append("?");
                        }
                        else if ((ATableName == "a_ap_payment") && (col.ColumnName == "CurrencyCode"))
                        {
                            OdbcParameter p = new OdbcParameter(Parameters.Count.ToString(), OdbcType.VarChar);
                            p.Value = FCurrencyPerLedger[Convert.ToInt32(RowDetails["LedgerNumber"])];
                            Parameters.Add(p);
                            InsertStatement.Append("?");
                        }
                        else
                        {
                            InsertStatement.Append("NULL"); // DEFAULT
                        }
                    }
                }

                InsertStatement.Append(")");
            }

            try
            {
                AReadWriteTransaction.DataBaseObj.ExecuteNonQuery(InsertStatement.ToString(), AReadWriteTransaction, Parameters.ToArray());
            }
            catch (Exception e)
            {
                TLogging.Log("error in ResetDatabase, LoadTable " + ATableName + ":" + e.Message);
                throw;
            }
            return true;
        }

        private static void ConvertColumnNames(DataColumnCollection AColumns)
        {
            foreach (DataColumn col in AColumns)
            {
                string colName = StringHelper.UpperCamelCase(col.ColumnName, true, true);

                if (AColumns.Contains(colName))
                {
                    // this column is not unique. happens in p_recent_partner, columns p_when_d and p_when_t
                    colName = StringHelper.UpperCamelCase(col.ColumnName, true, false);
                }

                col.ColumnName = colName;
            }
        }

        /// <summary>
        /// Commits the given SampleDataConstructorTDS TDS to the database
        /// </summary>
        /// <remarks>
        /// Why did I have to spend _several hours_ trying to get into how OpenPetra does things
        /// just in order to add this one line?
        /// </remarks>
        /// <param name="dataTDS"></param>
        /// <returns></returns>
        [RequireModulePermission("SYSMAN")]
        public static bool SaveTDS(SampleDataConstructorTDS dataTDS)
        {
            SampleDataConstructorTDSAccess.SubmitChanges(dataTDS);

            return true;
        }
    }
}

namespace Ict.Petra.Server.MSysMan.ImportExport
{
    /// <summary>
    /// this manager is called from the server admin console
    /// </summary>
    public class TImportExportManager : IImportExportManager
    {
        /// <summary>
        /// BackupDatabaseToYmlGZ
        /// </summary>
        public string BackupDatabaseToYmlGZ()
        {
            return TImportExportWebConnector.ExportAllTables();
        }

        /// <summary>
        /// RestoreDatabaseFromYmlGZ
        /// </summary>
        public bool RestoreDatabaseFromYmlGZ(string AYmlGzData)
        {
            return TImportExportWebConnector.ResetDatabase(AYmlGzData);
        }
    }
}
