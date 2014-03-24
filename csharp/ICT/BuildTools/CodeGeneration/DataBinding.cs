//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration
{
    /// <summary>
    /// functions to connect controls and columns to fields in a table on a screen
    /// </summary>
    public class TDataBinding
    {
        /// <summary>the OpenPetra Database structure is stored in this variable</summary>
        public static TDataDefinitionStore FPetraXMLStore = null;
        /// <summary>the code storage for writing the forms file</summary>
        public static TCodeStorage FCodeStorage = null;
        /// <summary>store the dataset tables that are used on this form</summary>
        private static SortedList <string, SortedList <string, TTable>>FDatasetTables = null;
        private static SortedList <string, TTable>FCurrentDataset = null;

        /// <summary>
        /// reset the selection of the current dataset
        /// </summary>
        public static void ResetCurrentDataset()
        {
            FCurrentDataset = null;
        }

        /// <summary>
        /// load the dataset tables
        /// </summary>
        public static SortedList <string, TTable>LoadDatasetTables(string AICTPath, string ADataSetTypeWithNamespace, TCodeStorage ACodeStorage)
        {
            if (FDatasetTables == null)
            {
                FDatasetTables = new SortedList <string, SortedList <string, TTable>>();
            }

            FCodeStorage = ACodeStorage;

            if (!ADataSetTypeWithNamespace.StartsWith("Ict.Petra.Shared"))
            {
                throw new Exception("the DatasetType must contain the full namespace, starting with Ict.Petra.Shared");
            }

            if (FDatasetTables.ContainsKey(ADataSetTypeWithNamespace))
            {
                FCurrentDataset = FDatasetTables[ADataSetTypeWithNamespace];

                return FCurrentDataset;
            }

            string[] datasetTypeSplit = ADataSetTypeWithNamespace.Split(new char[] { '.' });
            string module = datasetTypeSplit[3];
            string datasetName = datasetTypeSplit[datasetTypeSplit.Length - 1];

            // find the correct xml file for the dataset.
            // look in Ict/Petra/Shared/lib/MODULE/data
            string dataPath = AICTPath + "/Petra/Shared/lib/" + module + "/data/";

            DirectoryInfo directory = new DirectoryInfo(dataPath);
            FileInfo[] xmlFiles = directory.GetFiles("*.xml");
            XmlNode datasetNode = null;

            foreach (FileInfo fileinfo in xmlFiles)
            {
                if (datasetNode == null)
                {
                    TXMLParser parser = new TXMLParser(dataPath + "/" + fileinfo.Name, false);
                    datasetNode = parser.GetDocument().SelectSingleNode(String.Format("//DataSet[@name='{0}']", datasetName));
                }
            }

            if (datasetNode == null)
            {
                throw new Exception("cannot find the xml file for dataset " + ADataSetTypeWithNamespace);
            }

            SortedList <string, TTable>result = new SortedList <string, TTable>();
            XmlNodeList tables = datasetNode.SelectNodes("Table|CustomTable");

            foreach (XmlNode tableNode in tables)
            {
                TTable table = new TTable();
                string tablename;

                if ((tableNode.Name == "Table") && TXMLParser.HasAttribute(tableNode, "sqltable"))
                {
                    tablename = TTable.NiceTableName(tableNode.Attributes["sqltable"].Value);
                    table.Assign(FPetraXMLStore.GetTable(tablename));

                    table.strVariableNameInDataset = TXMLParser.HasAttribute(tableNode, "name") ? tableNode.Attributes["name"].Value : tablename;

                    if ((tableNode.SelectNodes("CustomField").Count > 0) || (tableNode.SelectNodes("Field").Count > 0))
                    {
                        table.strDotNetName = datasetName + tablename;
                    }
                }
                else if ((tableNode.Name == "Table") && TXMLParser.HasAttribute(tableNode, "customtable"))
                {
                    table = new TTable();
                    tablename = tableNode.Attributes["customtable"].Value;
                    table.strName = tablename;
                    table.strDotNetName = tablename;
                    table.strVariableNameInDataset = TXMLParser.HasAttribute(tableNode, "name") ? tableNode.Attributes["name"].Value : tablename;
                }
                else
                {
                    table = new TTable();
                    tablename = tableNode.Attributes["name"].Value;
                    table.strName = tablename;
                    table.strDotNetName = datasetName + tablename;
                    table.strVariableNameInDataset = tablename;
                }

                // add the custom fields if there are any
                XmlNodeList customFields = tableNode.SelectNodes("CustomField");

                foreach (XmlNode customField in customFields)
                {
                    TTableField newField = new TTableField();
                    newField.strName = customField.Attributes["name"].Value;
                    newField.strNameDotNet = newField.strName;
                    newField.strType = customField.Attributes["type"].Value;
                    newField.strTypeDotNet = customField.Attributes["type"].Value;
                    newField.strTableName = tablename;
                    newField.strDescription = "";
                    newField.bNotNull =
                        TXMLParser.HasAttribute(customField, "notnull") && TXMLParser.GetAttribute(customField, "notnull").ToLower() == "true";
                    table.grpTableField.Add(newField);
                }

                // add other fields from other tables that are defined in petra.xml
                XmlNodeList otherFields = tableNode.SelectNodes("Field");

                foreach (XmlNode otherField in otherFields)
                {
                    TTable otherTable = FPetraXMLStore.GetTable(otherField.Attributes["sqltable"].Value);
                    TTableField newField = new TTableField(otherTable.GetField(otherField.Attributes["sqlfield"].Value));

                    if (TXMLParser.HasAttribute(otherField, "name"))
                    {
                        newField.strNameDotNet = otherField.Attributes["name"].Value;
                    }

                    newField.strTableName = tablename;
                    table.grpTableField.Add(newField);
                }

                result.Add(table.strVariableNameInDataset, table);
            }

            FDatasetTables.Add(ADataSetTypeWithNamespace, result);
            FCurrentDataset = result;
            return result;
        }

        /// <summary>
        /// resolve a reference to a field in the database;
        /// support master and detail tables
        /// </summary>
        /// <param name="ACtrl"></param>
        /// <param name="ADataField">string with potential reference to data field</param>
        /// <param name="AIsDetailNotMaster">returns if this is a field in the master or in the detail table</param>
        /// <param name="AShowWarningNonExistingField">fail on non existing field</param>
        /// <returns>reference to the table field, or null</returns>
        public static TTableField GetTableField(TControlDef ACtrl,
            string ADataField,
            out bool AIsDetailNotMaster,
            bool AShowWarningNonExistingField)
        {
            string tablename = "";
            string fieldname = "";

            // is there an explicit specification for which datafield to use
            if (ADataField.IndexOf(".") > 0)
            {
                tablename = ADataField.Split('.')[0];
                fieldname = ADataField.Split('.')[1];
            }
            else
            {
                if (ACtrl != null)
                {
                    if (ACtrl.HasAttribute("Unbound") && (ACtrl.GetAttribute("Unbound").ToLower() == "true"))
                    {
                        AIsDetailNotMaster = false;
                        return null;
                    }

                    if (ADataField.Length == 0)
                    {
                        ADataField = ACtrl.controlName.Substring(ACtrl.controlTypePrefix.Length);
                    }

                    if (ADataField.StartsWith("Detail"))
                    {
                        fieldname = ADataField;

                        if (fieldname.StartsWith("Detail"))
                        {
                            fieldname = fieldname.Substring("Detail".Length);
                        }

                        tablename = FCodeStorage.GetAttribute("DetailTable");
                    }
                    else
                    {
                        // check for other tables in the typed Dataset
                        if (FCurrentDataset != null)
                        {
                            // ADataField can either contain just the field, or the table name and the field

                            // does ADataField start with a table name?
                            foreach (TTable table2 in FCurrentDataset.Values)
                            {
                                if (ADataField.StartsWith(table2.strDotNetName) || ADataField.StartsWith(TTable.NiceTableName(table2.strName)))
                                {
                                    if (ADataField.StartsWith(table2.strDotNetName))
                                    {
                                        tablename = table2.strDotNetName;
                                    }
                                    else
                                    {
                                        tablename = TTable.NiceTableName(table2.strName);
                                    }

                                    foreach (TTableField field in table2.grpTableField)
                                    {
                                        if (TTable.NiceFieldName(field.strName) == ADataField.Substring(tablename.Length))
                                        {
                                            fieldname = TTable.NiceFieldName(field.strName);
                                        }

                                        if (field.strNameDotNet == ADataField.Substring(tablename.Length))
                                        {
                                            fieldname = field.strNameDotNet;
                                        }
                                    }
                                }
                            }

                            // check if the master table has such a field
                            if ((fieldname.Length == 0) && FCodeStorage.HasAttribute("MasterTable"))
                            {
                                TTable table2 = FCurrentDataset[FCodeStorage.GetAttribute("MasterTable")];

                                foreach (TTableField field in table2.grpTableField)
                                {
                                    if ((TTable.NiceFieldName(field.strName) == ADataField) || (field.strNameDotNet == ADataField))
                                    {
                                        tablename = FCodeStorage.GetAttribute("MasterTable");
                                        fieldname = ADataField;
                                    }
                                }
                            }

                            // check if the detail table has such a field
                            if ((fieldname.Length == 0) && FCodeStorage.HasAttribute("DetailTable"))
                            {
                                if ((FCurrentDataset != null) && FCurrentDataset.ContainsKey(FCodeStorage.GetAttribute("DetailTable")))
                                {
                                    TTable table2 = FCurrentDataset[FCodeStorage.GetAttribute("DetailTable")];

                                    foreach (TTableField field in table2.grpTableField)
                                    {
                                        if ((TTable.NiceFieldName(field.strName) == ADataField) || (field.strNameDotNet == ADataField))
                                        {
                                            tablename = FCodeStorage.GetAttribute("DetailTable");
                                            fieldname = ADataField;
                                        }
                                    }
                                }
                            }

                            // check if there is a unique match for this control name in all the tables
                            if (fieldname.Length == 0)
                            {
                                foreach (TTable table2 in FCurrentDataset.Values)
                                {
                                    foreach (TTableField field in table2.grpTableField)
                                    {
                                        if ((TTable.NiceFieldName(field.strName) == ADataField) || (field.strNameDotNet == ADataField))
                                        {
                                            if (fieldname.Length > 0)
                                            {
                                                throw new Exception(
                                                    String.Format("there are a several tables with field {0}: {1} and {2}",
                                                        fieldname, tablename, table2.strDotNetName));
                                            }

                                            if ((table2.strDotNetName != null) && (table2.strDotNetName.Length > 0))
                                            {
                                                tablename = table2.strDotNetName;
                                            }
                                            else
                                            {
                                                tablename = TTable.NiceTableName(table2.strName);
                                            }

                                            if ((field.strNameDotNet != null) && (field.strNameDotNet.Length > 0))
                                            {
                                                fieldname = field.strNameDotNet;
                                            }
                                            else
                                            {
                                                fieldname = TTable.NiceFieldName(field.strName);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            tablename = FCodeStorage.GetAttribute("MasterTable");
                            fieldname = ADataField;
                        }
                    }
                }
                else
                {
                    // eg used for columns in datagrid
                    if (ADataField.StartsWith("Detail"))
                    {
                        tablename = FCodeStorage.GetAttribute("DetailTable");
                        fieldname = ADataField.Substring("Detail".Length);
                    }
                    else
                    {
                        tablename = FCodeStorage.GetAttribute("MasterTable");
                        fieldname = ADataField;
                    }
                }
            }

            AIsDetailNotMaster = FCodeStorage.GetAttribute("DetailTable") == tablename;

            if (tablename.Length == 0)
            {
                // this is probably no data field at all
                if (AShowWarningNonExistingField)
                {
                    Console.WriteLine("Warning: Expected data field but cannot resolve " + ADataField);
                }

                return null;
            }

            TTable table = null;

            if ((FCurrentDataset != null) && FCurrentDataset.ContainsKey(tablename))
            {
                table = FCurrentDataset[tablename];
            }
            else
            {
                table = FPetraXMLStore.GetTable(tablename);
            }

            if (table == null)
            {
                if (!AShowWarningNonExistingField)
                {
                    return null;
                }

                throw new Exception("Cannot find table: " + tablename);
            }

            return table.GetField(fieldname, AShowWarningNonExistingField);
        }
    }
}