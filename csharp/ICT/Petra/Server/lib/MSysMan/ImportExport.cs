/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Globalization;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Petra.Shared;

namespace Ict.Petra.Server.MSysMan.ImportExport.WebConnectors
{
    /// <summary>
    /// import and export of all data in this database
    /// </summary>
    public class TImportExportWebConnector
    {
        /// <summary>
        /// return an XmlDocument with all data in this database;
        /// this is useful to convert data between different database systems, etc
        /// </summary>
        /// <returns></returns>
        public static string ExportAllTables()
        {
            XmlDocument OpenPetraData = TYml2Xml.CreateXmlDocument();

            XmlNode rootNode = OpenPetraData.FirstChild.NextSibling;

            ExportTables(rootNode, "MSysMan", "");
            ExportTables(rootNode, "MCommon", "");
            ExportTables(rootNode, "MPartner", "Partner");
            ExportTables(rootNode, "MPartner", "Mailroom");
            ExportTables(rootNode, "MFinance", "Account");
            ExportTables(rootNode, "MFinance", "Gift");
            ExportTables(rootNode, "MFinance", "AP");
            ExportTables(rootNode, "MFinance", "AR");
            ExportTables(rootNode, "MPersonnel", "Personnel");
            ExportTables(rootNode, "MPersonnel", "Units");
            ExportTables(rootNode, "MConference", "");
            ExportTables(rootNode, "MHospitality", "");
            return TXMLParser.XmlToString(OpenPetraData);
        }

        /// <summary>
        /// export one module at the time
        /// </summary>
        /// <param name="ARootNode"></param>
        /// <param name="AModuleName"></param>
        /// <param name="ASubModuleName">can be empty if there is no submodule</param>
        private static void ExportTables(XmlNode ARootNode, string AModuleName, string ASubModuleName)
        {
            XmlElement moduleNode = ARootNode.OwnerDocument.CreateElement(AModuleName + ASubModuleName);

            ARootNode.AppendChild(moduleNode);

            string namespaceName = "Ict.Petra.Shared." + AModuleName;

            if (ASubModuleName.Length > 0)
            {
                namespaceName += "." + ASubModuleName;
            }

            namespaceName += ".Data";
            Assembly asm = Assembly.LoadFrom("Ict.Petra.Shared." + AModuleName + ".DataTables.dll");

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            foreach (Type type in asm.GetTypes())
            {
                if ((type.Namespace == namespaceName) && type.Name.EndsWith("Table"))
                {
                    ExportTable(moduleNode, type, Transaction);
                }
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
        }

        private static void ExportTable(XmlNode AModuleNode, Type ATableType, TDBTransaction ATransaction)
        {
            MethodInfo mi = ATableType.GetMethod("GetTableDBName", BindingFlags.Static | BindingFlags.Public);
            string TableDBName = mi.Invoke(null, null).ToString();
            DataTable table = DBAccess.GDBAccessObj.SelectDT("Select * from " + TableDBName, TableDBName, ATransaction);

            XmlElement tableNode = AModuleNode.OwnerDocument.CreateElement(ATableType.Name);

            AModuleNode.AppendChild(tableNode);

            Int32 RowCounter = 0;

            ConvertColumnNames(table.Columns);

            // TODO: automatically filter column values that are the same and group the data?
            foreach (DataRow row in table.Rows)
            {
                RowCounter++;
                XmlElement rowNode = AModuleNode.OwnerDocument.CreateElement("Row" + RowCounter.ToString());
                tableNode.AppendChild(rowNode);

                foreach (DataColumn col in table.Columns)
                {
                    if (row[col].GetType() != typeof(DBNull))
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
                            double dval = Convert.ToDouble(row[col]);
                            rowNode.SetAttribute(col.ColumnName, dval.ToString("G", CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            rowNode.SetAttribute(col.ColumnName, row[col].ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// this will reset the current database, and load the data from the given XmlDocument
        /// </summary>
        /// <param name="ANewDatabaseData"></param>
        /// <returns></returns>
        public static bool ResetDatabase(string ANewDatabaseData)
        {
            List <string>tables = TTableList.GetDBNames();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                tables.Reverse();

                foreach (string table in tables)
                {
                    DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM pub_" + table, Transaction, false);
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(ANewDatabaseData);

                tables.Reverse();

                // one transaction to import the user table and user permissions. otherwise logging in will not be possible if other import fails?
                bool success = true;
                success = success && LoadTable("s_user", doc, Transaction);
                success = success && LoadTable("s_module", doc, Transaction);
                success = success && LoadTable("s_user_module_access_permission", doc, Transaction);
                success = success && LoadTable("s_system_defaults", doc, Transaction);

                if (!success)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    return false;
                }

                DBAccess.GDBAccessObj.CommitTransaction();
                tables.Remove("s_user");
                tables.Remove("s_module");
                tables.Remove("s_user_module_access_permission");
                tables.Remove("s_system_defaults");

                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                foreach (string table in tables)
                {
                    LoadTable(table, doc, Transaction);
                }

                // TODO: what about sequences? they should be set appropriately, not lagging behind the imported data?

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception e)
            {
                TLogging.Log("Problem in ResetDatabase: " + e.Message);
                TLogging.Log(e.StackTrace);
                DBAccess.GDBAccessObj.RollbackTransaction();
                return false;
            }

            return true;
        }

        private static XmlNode FindNode(XmlDocument ADoc, string ATableName)
        {
            XmlNode rootNode = ADoc.FirstChild.NextSibling;

            foreach (XmlNode ModuleNode in rootNode.ChildNodes)
            {
                foreach (XmlNode TableNode in ModuleNode.ChildNodes)
                {
                    if (TableNode.Name == ATableName + "Table")
                    {
                        return TableNode;
                    }
                }
            }

            return null;
        }

        private static bool LoadTable(string ATableName, XmlDocument ADoc, TDBTransaction ATransaction)
        {
            XmlNode TableNode = FindNode(ADoc, StringHelper.UpperCamelCase(ATableName, false, false));

            if (TableNode == null)
            {
                // TLogging.Log("tablenode null: " + ATableName);
                return false;
            }

            if (TableNode.ChildNodes.Count == 0)
            {
                // TLogging.Log("no children: " + ATableName);
                return false;
            }

            DataTable table = DBAccess.GDBAccessObj.SelectDT("Select * from " + ATableName, ATableName, ATransaction);
            List <OdbcParameter>Parameters = new List <OdbcParameter>();

            ConvertColumnNames(table.Columns);

            string InsertStatement = "INSERT INTO pub_" + ATableName + "() VALUES ";

            bool firstRow = true;

            foreach (XmlNode RowNode in TableNode.ChildNodes)
            {
                if (!firstRow)
                {
                    if (CommonTypes.ParseDBType(DBAccess.GDBAccessObj.DBType) == TDBType.SQLite)
                    {
                        // SQLite does not support INSERT of several rows at the same time
                        try
                        {
                            DBAccess.GDBAccessObj.ExecuteNonQuery(InsertStatement, ATransaction, false, Parameters.ToArray());
                        }
                        catch (Exception e)
                        {
                            TLogging.Log("error in ResetDatabase, LoadTable " + ATableName + ":" + e.Message);
                            throw e;
                        }

                        InsertStatement = "INSERT INTO pub_" + ATableName + "() VALUES ";
                        Parameters = new List <OdbcParameter>();
                    }
                    else
                    {
                        InsertStatement += ",";
                    }
                }

                firstRow = false;

                InsertStatement += "(";

                bool firstColumn = true;

                foreach (DataColumn col in table.Columns)
                {
                    if (!firstColumn)
                    {
                        InsertStatement += ",";
                    }

                    firstColumn = false;

                    if (TYml2Xml.HasAttribute(RowNode, col.ColumnName))
                    {
                        string strValue = TYml2Xml.GetAttribute(RowNode, col.ColumnName);

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
                            p.Value = strValue;
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
                            p.Value = Convert.ToDouble(strValue.Replace(",", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
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
                            p.Value = Convert.ToDecimal(strValue);
                            Parameters.Add(p);
                        }
                        else
                        {
                            // should not get here?
                            throw new Exception("error in ResetDatabase, LoadTable: " + col.DataType.ToString() + " has not yet been implemented");
                        }

                        InsertStatement += "?";
                    }
                    else
                    {
                        InsertStatement += "NULL"; // DEFAULT
                    }
                }

                InsertStatement += ")";
            }

            try
            {
                DBAccess.GDBAccessObj.ExecuteNonQuery(InsertStatement, ATransaction, false, Parameters.ToArray());
            }
            catch (Exception e)
            {
                TLogging.Log("error in ResetDatabase, LoadTable " + ATableName + ":" + e.Message);
                throw e;
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
    }
}