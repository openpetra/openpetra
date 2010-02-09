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
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;

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
                        string colName = StringHelper.UpperCamelCase(col.ColumnName, true, true);

                        if (col.DataType == typeof(DateTime))
                        {
                            rowNode.SetAttribute(colName, Convert.ToDateTime(row[col]).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            rowNode.SetAttribute(colName, row[col].ToString());
                        }
                    }
                }
            }
        }
    }
}