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
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.DataStore
{
    /// <summary>
    /// the code generator for typed tables
    /// </summary>
    public class CodeGenerationTable
    {
        /// <summary>
        /// create the code for the definition of a typed table
        /// </summary>
        /// <param name="Template"></param>
        /// <param name="currentTable"></param>
        /// <param name="origTable"></param>
        /// <param name="WhereToInsert"></param>
        /// <param name="CalledFromDataSet"></param>
        public static void InsertTableDefinition(ProcessTemplate Template,
            TTable currentTable,
            TTable origTable,
            string WhereToInsert,
            Boolean CalledFromDataSet)
        {
            ProcessTemplate snippet = Template.GetSnippet("TYPEDTABLE");
            string derivedTable = "";

            if (origTable != null)
            {
                snippet.SetCodelet("BASECLASSTABLE", TTable.NiceTableName(currentTable.strName) + "Table");
                derivedTable = "new ";
                snippet.SetCodelet("TABLEID", origTable.iOrder.ToString());
            }
            else
            {
                snippet.SetCodelet("BASECLASSTABLE", "TTypedDataTable");
                snippet.SetCodelet("TABLEID", currentTable.iOrder.ToString());
            }

            snippet.SetCodelet("NEW", derivedTable);
            snippet.SetCodeletComment("TABLE_DESCRIPTION", currentTable.strDescription);
            snippet.SetCodelet("TABLENAME", currentTable.strDotNetName);

            if (CalledFromDataSet)
            {
                snippet.SetCodelet("TABLEINTDS", "TableInTDS");
            }
            else
            {
                snippet.SetCodelet("TABLEINTDS", "");
            }

            if (currentTable.AvailableForCustomReport)
            {
                snippet.SetCodelet("AVAILABLEFORCUSTOMREPORT", "true");
            }
            else
            {
                snippet.SetCodelet("AVAILABLEFORCUSTOMREPORT", "false");
            }

            snippet.SetCodelet("CUSTOMREPORTPERMISSION", currentTable.CustomReportPermission);

            if (currentTable.strVariableNameInDataset != null)
            {
                snippet.SetCodelet("TABLEVARIABLENAME", currentTable.strVariableNameInDataset);
            }
            else
            {
                snippet.SetCodelet("TABLEVARIABLENAME", currentTable.strDotNetName);
            }

            snippet.SetCodelet("DBTABLENAME", currentTable.strName);

            snippet.SetCodelet("DBTABLELABEL", currentTable.strLabel);

            if (currentTable.HasPrimaryKey())
            {
                TConstraint primKey = currentTable.GetPrimaryKey();
                bool first = true;
                string primaryKeyColumns = "";
                int prevIndex = -1;

                // the fields in the primary key should be used in the same order as in the table.
                // otherwise this is causing confusion. eg. a_processed_fee
                foreach (TTableField column in currentTable.grpTableField)
                {
                    int newIndex = -1;

                    if (primKey.strThisFields.Contains(column.strName))
                    {
                        newIndex = primKey.strThisFields.IndexOf(column.strName);
                    }
                    else if (primKey.strThisFields.Contains(TTable.NiceFieldName(column)))
                    {
                        newIndex = primKey.strThisFields.IndexOf(TTable.NiceFieldName(column));
                    }

                    if (newIndex != -1)
                    {
                        if (newIndex < prevIndex)
                        {
                            throw new Exception("Please fix the order of the fields in the primary key of table " + currentTable.strName);
                        }

                        prevIndex = newIndex;
                    }
                }

                // the fields in the primary key should be used in the same order as in the table.
                // otherwise this is causing confusion. eg. a_processed_fee
                foreach (TTableField column in currentTable.grpTableField)
                {
                    if (primKey.strThisFields.Contains(column.strName) || primKey.strThisFields.Contains(TTable.NiceFieldName(column)))
                    {
                        string columnName = column.strName;

                        string toAdd = currentTable.grpTableField.IndexOf(currentTable.GetField(columnName)).ToString();

                        if (!first)
                        {
                            toAdd = ", " + toAdd;
                            primaryKeyColumns += ",";
                        }

                        first = false;

                        snippet.AddToCodelet("COLUMNPRIMARYKEYORDER", toAdd);
                        primaryKeyColumns += "Column" + TTable.NiceFieldName(currentTable.GetField(columnName));
                    }
                }

                if (primaryKeyColumns.Length > 0)
                {
                    snippet.SetCodelet("PRIMARYKEYCOLUMNS", primaryKeyColumns);
                    snippet.SetCodelet("PRIMARYKEYCOLUMNSCOUNT", primKey.strThisFields.Count.ToString());
                }
            }
            else
            {
                snippet.AddToCodelet("COLUMNPRIMARYKEYORDER", "");
            }

            if (currentTable.HasUniqueKey())
            {
                TConstraint primKey = currentTable.GetFirstUniqueKey();
                bool first = true;

                foreach (string columnName in primKey.strThisFields)
                {
                    string toAdd = currentTable.grpTableField.IndexOf(currentTable.GetField(columnName)).ToString();

                    if (!first)
                    {
                        toAdd = ", " + toAdd;
                    }

                    first = false;

                    snippet.AddToCodelet("COLUMNUNIQUEKEYORDER", toAdd);
                }
            }
            else
            {
                snippet.AddToCodelet("COLUMNUNIQUEKEYORDER", "");
            }

            int colOrder = 0;
            Boolean CustomReportFieldAdded = false;
            ProcessTemplate tempTemplate = null;

            foreach (TTableField col in currentTable.grpTableField)
            {
                col.strTableName = currentTable.strName;
                string columnOverwrite = "";
                bool writeColumnProperties = true;

                if ((origTable != null) && (origTable.GetField(col.strName, false) != null))
                {
                    columnOverwrite = "new ";

                    if (origTable.GetField(col.strName).iOrder == colOrder)
                    {
                        // same order number, save some lines of code by not writing them
                        writeColumnProperties = false;
                    }
                }

                if (writeColumnProperties && (columnOverwrite.Length == 0))
                {
                    tempTemplate = Template.GetSnippet("DATACOLUMN");
                    tempTemplate.SetCodeletComment("COLUMN_DESCRIPTION", col.strDescription);
                    tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                    snippet.InsertSnippet("DATACOLUMNS", tempTemplate);
                }

                if (writeColumnProperties)
                {
                    tempTemplate = Template.GetSnippet("COLUMNIDS");
                    tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                    tempTemplate.SetCodelet("COLUMNORDERNUMBER", colOrder.ToString());
                    tempTemplate.SetCodelet("NEW", columnOverwrite);
                    snippet.InsertSnippet("COLUMNIDS", tempTemplate);
                }

                if (origTable == null)
                {
                    tempTemplate = Template.GetSnippet("COLUMNINFO");
                    tempTemplate.SetCodelet("COLUMNORDERNUMBER", colOrder.ToString());
                    tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                    tempTemplate.SetCodelet("COLUMNDBNAME", col.strName);
                    tempTemplate.SetCodelet("COLUMNLABEL", col.strLabel);
                    tempTemplate.SetCodelet("COLUMNODBCTYPE", CodeGenerationPetra.ToOdbcTypeString(col));
                    tempTemplate.SetCodelet("COLUMNLENGTH", col.iLength.ToString());
                    tempTemplate.SetCodelet("COLUMNNOTNULL", col.bNotNull.ToString().ToLower());
                    tempTemplate.SetCodelet("COLUMNCOMMA", colOrder + 1 < currentTable.grpTableField.Count ? "," : "");
                    snippet.InsertSnippet("COLUMNINFO", tempTemplate);
                }

                tempTemplate = Template.GetSnippet("INITCLASSADDCOLUMN");
                tempTemplate.SetCodelet("COLUMNDBNAME", col.strName);
                tempTemplate.SetCodelet("COLUMNDOTNETTYPE", col.GetDotNetType());
                tempTemplate.SetCodelet("COLUMNDOTNETTYPENOTNULLABLE", col.GetDotNetType().Replace("?", ""));
                snippet.InsertSnippet("INITCLASSADDCOLUMN", tempTemplate);

                tempTemplate = Template.GetSnippet("INITVARSCOLUMN");
                tempTemplate.SetCodelet("COLUMNDBNAME", col.strName);
                tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                snippet.InsertSnippet("INITVARSCOLUMN", tempTemplate);

                if (col.bAvailableForCustomReport)
                {
                    tempTemplate = Template.GetSnippet("INITVARSCUSTOMREPORTFIELDLIST");

                    if (CustomReportFieldAdded)
                    {
                        tempTemplate.SetCodelet("LISTDELIMITER", ",");
                    }
                    else
                    {
                        tempTemplate.SetCodelet("LISTDELIMITER", "");
                    }

                    tempTemplate.SetCodelet("COLUMNDBNAME", col.strName);

                    snippet.InsertSnippet("INITVARSCUSTOMREPORTFIELDLIST", tempTemplate);

                    CustomReportFieldAdded = true;
                }

                if (writeColumnProperties)
                {
                    tempTemplate = Template.GetSnippet("STATICCOLUMNPROPERTIES");
                    tempTemplate.SetCodelet("COLUMNDBNAME", col.strName);
                    tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                    tempTemplate.SetCodelet("COLUMNHELP", col.strHelp.Replace("\"", "\\\""));
                    tempTemplate.SetCodelet("COLUMNLENGTH", col.iLength.ToString());
                    tempTemplate.SetCodelet("NEW", columnOverwrite);
                    snippet.InsertSnippet("STATICCOLUMNPROPERTIES", tempTemplate);
                }

                colOrder++;
            }

            if (!CustomReportFieldAdded)
            {
                // fill snippet if nothing was added yet
                tempTemplate = Template.GetSnippet("INITVARSCUSTOMREPORTFIELDLISTEMPTY");
                tempTemplate.SetCodelet("EMPTY", "");
                snippet.InsertSnippet("INITVARSCUSTOMREPORTFIELDLIST", tempTemplate);
            }

            Template.InsertSnippet(WhereToInsert, snippet);
        }

        /// <summary>
        /// write the definition for the code of a typed row
        /// </summary>
        /// <param name="Template"></param>
        /// <param name="currentTable"></param>
        /// <param name="origTable"></param>
        /// <param name="WhereToInsert"></param>
        public static void InsertRowDefinition(ProcessTemplate Template, TTable currentTable, TTable origTable, string WhereToInsert)
        {
            ProcessTemplate snippet = Template.GetSnippet("TYPEDROW");

            if (origTable != null)
            {
                snippet.SetCodelet("BASECLASSROW", TTable.NiceTableName(currentTable.strName) + "Row");
                snippet.SetCodelet("OVERRIDE", "override ");
            }
            else
            {
                snippet.SetCodelet("BASECLASSROW", "System.Data.DataRow");
                snippet.SetCodelet("OVERRIDE", "virtual ");
            }

            snippet.SetCodeletComment("TABLE_DESCRIPTION", currentTable.strDescription);
            snippet.SetCodelet("TABLENAME", currentTable.strDotNetName);

            foreach (TTableField col in currentTable.grpTableField)
            {
                ProcessTemplate tempTemplate = null;
                string columnOverwrite = "";

                if ((origTable != null) && (origTable.GetField(col.strName, false) != null))
                {
                    columnOverwrite = "new ";
                }

                if (columnOverwrite.Length == 0)
                {
                    tempTemplate = Template.GetSnippet("ROWCOLUMNPROPERTY");
                    tempTemplate.SetCodelet("COLUMNDBNAME", col.strName);
                    tempTemplate.SetCodelet("COLUMNNAME", col.strNameDotNet);
                    tempTemplate.SetCodelet("COLUMNHELP", col.strDescription.Replace(Environment.NewLine, " "));
                    tempTemplate.SetCodelet("COLUMNLABEL", col.strLabel);
                    tempTemplate.SetCodelet("COLUMNLENGTH", col.iLength.ToString());
                    tempTemplate.SetCodelet("COLUMNDOTNETTYPE", col.GetDotNetType());

                    if (!col.bNotNull)
                    {
                        if (col.GetDotNetType().Contains("DateTime?"))
                        {
                            tempTemplate.SetCodelet("TESTFORNULL", "!value.HasValue");
                        }
                        else if (col.GetDotNetType().Contains("String"))
                        {
                            tempTemplate.SetCodelet("TESTFORNULL", "(value == null) || (value.Length == 0)");
                        }
                    }

                    if (col.GetDotNetType().Contains("DateTime?"))
                    {
                        tempTemplate.SetCodelet("ACTIONGETNULLVALUE", "return null;");
                    }
                    else if (col.GetDotNetType().Contains("DateTime"))
                    {
                        tempTemplate.SetCodelet("ACTIONGETNULLVALUE", "return DateTime.MinValue;");
                    }
                    else if (col.GetDotNetType().ToLower().Contains("string"))
                    {
                        tempTemplate.SetCodelet("ACTIONGETNULLVALUE", "return String.Empty;");
                    }
                    else
                    {
                        tempTemplate.SetCodelet("ACTIONGETNULLVALUE", "throw new System.Data.StrongTypingException(\"Error: DB null\", null);");
                    }

                    tempTemplate.SetCodeletComment("COLUMN_DESCRIPTION", col.strDescription);
                    snippet.InsertSnippet("ROWCOLUMNPROPERTIES", tempTemplate);

                    tempTemplate = Template.GetSnippet("FUNCTIONSFORNULLVALUES");
                    tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                    snippet.InsertSnippet("FUNCTIONSFORNULLVALUES", tempTemplate);
                }

                if ((col.strDefault.Length > 0) && (col.strDefault != "NULL"))
                {
                    string defaultValue = col.strDefault;

                    if (defaultValue == "SYSDATE")
                    {
                        defaultValue = "DateTime.Today";
                    }
                    else if ((col.strType == "bit") || ((col.strTypeDotNet != null) && col.strTypeDotNet.ToLower().Contains("bool")))
                    {
                        defaultValue = (defaultValue == "1" || defaultValue.ToLower() == "true").ToString().ToLower();
                    }
                    else if ((col.strType == "varchar") || ((col.strTypeDotNet != null) && col.strTypeDotNet.ToLower().Contains("string")))
                    {
                        defaultValue = '"' + defaultValue + '"';
                    }

                    snippet.AddToCodelet("ROWSETNULLORDEFAULT", "this[this.myTable.Column" + TTable.NiceFieldName(
                            col) + ".Ordinal] = " + defaultValue + ";" + Environment.NewLine);
                }
                else
                {
                    snippet.AddToCodelet("ROWSETNULLORDEFAULT", "this.SetNull(this.myTable.Column" + TTable.NiceFieldName(
                            col) + ");" + Environment.NewLine);
                }
            }

            Template.InsertSnippet(WhereToInsert, snippet);
        }

        /// <summary>
        /// create the code for a typed table
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="strGroup"></param>
        /// <param name="AFilePath"></param>
        /// <param name="ANamespaceName"></param>
        /// <param name="AFileName"></param>
        /// <returns></returns>
        public static Boolean WriteTypedTable(TDataDefinitionStore AStore, string strGroup, string AFilePath, string ANamespaceName, string AFileName)
        {
            Console.WriteLine("processing namespace Typed Tables " + strGroup.Substring(0, 1).ToUpper() + strGroup.Substring(1));

            string templateDir = TAppSettingsManager.GetValue("TemplateDir", true);
            ProcessTemplate Template = new ProcessTemplate(templateDir + Path.DirectorySeparatorChar +
                "ORM" + Path.DirectorySeparatorChar +
                "DataTable.cs");

            Template.AddToCodelet("NAMESPACE", ANamespaceName);

            // load default header with license and copyright
            Template.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(templateDir));

            foreach (TTable currentTable in AStore.GetTables())
            {
                if (currentTable.strGroup == strGroup)
                {
                    if (!currentTable.HasPrimaryKey())
                    {
                        TLogging.Log("Warning: there is no primary key for table " + currentTable.strName);
                    }

                    InsertTableDefinition(Template, currentTable, null, "TABLELOOP", false);
                    InsertRowDefinition(Template, currentTable, null, "TABLELOOP");
                }
            }

            Template.FinishWriting(AFilePath + AFileName + "-generated.cs", ".cs", true);

            return true;
        }
    }
}