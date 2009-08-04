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
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Web;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.DataStore
{
    public class codeGenerationTable
    {
        public static void InsertTableDefinition(ProcessTemplate Template, TTable currentTable, string WhereToInsert)
        {
            ProcessTemplate snippet = Template.GetSnippet("TYPEDTABLE");

            snippet.SetCodeletComment("TABLE_DESCRIPTION", currentTable.strDescription);
            snippet.SetCodelet("TABLENAME", currentTable.strDotNetName);
            snippet.SetCodelet("DBTABLENAME", currentTable.strName);
            snippet.SetCodelet("TABLEID", currentTable.order.ToString());

            if (currentTable.HasPrimaryKey())
            {
                TConstraint primKey = currentTable.GetPrimaryKey();
                bool first = true;

                foreach (string columnName in primKey.strThisFields)
                {
                    string toAdd = currentTable.grpTableField.List.IndexOf(currentTable.GetField(columnName)).ToString();

                    if (!first)
                    {
                        toAdd = ", " + toAdd;
                    }

                    first = false;

                    snippet.AddToCodelet("COLUMNPRIMARYKEYORDER", toAdd);
                }
            }
            else
            {
                snippet.AddToCodelet("COLUMNPRIMARYKEYORDER", "");
            }

            int colOrder = 0;

            foreach (TTableField col in currentTable.grpTableField.List)
            {
                ProcessTemplate tempTemplate = Template.GetSnippet("DATACOLUMN");
                tempTemplate.SetCodeletComment("COLUMN_DESCRIPTION", col.strDescription);
                tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                snippet.InsertSnippet("DATACOLUMNS", tempTemplate);
                
                tempTemplate = Template.GetSnippet("COLUMNIDS");
                tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                tempTemplate.SetCodelet("COLUMNORDERNUMBER", colOrder.ToString());
                snippet.InsertSnippet("COLUMNIDS", tempTemplate);

                tempTemplate = Template.GetSnippet("COLUMNINFO");
                tempTemplate.SetCodelet("COLUMNORDERNUMBER", (colOrder++).ToString());
                tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                tempTemplate.SetCodelet("COLUMNDBNAME", col.strName);
                tempTemplate.SetCodelet("COLUMNLABEL", col.strLabel);
                tempTemplate.SetCodelet("COLUMNODBCTYPE", codeGenerationPetra.ToOdbcTypeString(col));
                tempTemplate.SetCodelet("COLUMNLENGTH", col.iLength.ToString());
                tempTemplate.SetCodelet("COLUMNNOTNULL", col.bNotNull.ToString().ToLower());
                tempTemplate.SetCodelet("COLUMNCOMMA", colOrder < currentTable.grpTableField.List.Count ? "," : "");
                snippet.InsertSnippet("COLUMNINFO", tempTemplate);

                tempTemplate = Template.GetSnippet("INITCLASSADDCOLUMN");
                tempTemplate.SetCodelet("COLUMNDBNAME", col.strName);
                tempTemplate.SetCodelet("COLUMNDOTNETTYPE", col.GetDotNetType());
                snippet.InsertSnippet("INITCLASSADDCOLUMN", tempTemplate);

                tempTemplate = Template.GetSnippet("INITVARSCOLUMN");
                tempTemplate.SetCodelet("COLUMNDBNAME", col.strName);
                tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                snippet.InsertSnippet("INITVARSCOLUMN", tempTemplate);

                tempTemplate = Template.GetSnippet("STATICCOLUMNPROPERTIES");
                tempTemplate.SetCodelet("COLUMNDBNAME", col.strName);
                tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                tempTemplate.SetCodelet("COLUMNLENGTH", col.iLength.ToString());
                snippet.InsertSnippet("STATICCOLUMNPROPERTIES", tempTemplate);
            }

            Template.InsertSnippet(WhereToInsert, snippet);
        }

        public static void InsertRowDefinition(ProcessTemplate Template, TTable currentTable, string WhereToInsert)
        {
            ProcessTemplate snippet = Template.GetSnippet("TYPEDROW");

            snippet.SetCodeletComment("TABLE_DESCRIPTION", currentTable.strDescription);
            snippet.SetCodelet("TABLENAME", currentTable.strDotNetName);

            foreach (TTableField col in currentTable.grpTableField.List)
            {
                ProcessTemplate tempTemplate = Template.GetSnippet("ROWCOLUMNPROPERTY");
                tempTemplate.SetCodelet("COLUMNDBNAME", col.strName);
                tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                tempTemplate.SetCodelet("COLUMNHELP", col.strDescription.Replace(Environment.NewLine, " "));
                tempTemplate.SetCodelet("COLUMNLABEL", col.strLabel);
                tempTemplate.SetCodelet("COLUMNLENGTH", col.iLength.ToString());
                tempTemplate.SetCodelet("COLUMNDOTNETTYPE", col.GetDotNetType());
                tempTemplate.SetCodeletComment("COLUMN_DESCRIPTION", col.strDescription);
                snippet.InsertSnippet("ROWCOLUMNPROPERTIES", tempTemplate);

                tempTemplate = Template.GetSnippet("FUNCTIONSFORNULLVALUES");
                tempTemplate.SetCodelet("COLUMNNAME", TTable.NiceFieldName(col));
                snippet.InsertSnippet("FUNCTIONSFORNULLVALUES", tempTemplate);

                if ((col.strDefault.Length > 0) && (col.strDefault != "NULL"))
                {
                    string defaultValue = col.strDefault;

                    if (defaultValue == "SYSDATE")
                    {
                        defaultValue = "DateTime.Today";
                    }
                    else if (col.strType == "bit")
                    {
                        defaultValue = (defaultValue == "1").ToString().ToLower();
                    }
                    else if (col.strType == "varchar")
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

        public static Boolean WriteTypedTable(TDataDefinitionStore AStore, string strGroup, string AFilePath, string ANamespaceName, string AFileName)
        {
            Console.WriteLine("processing namespace Typed Tables " + strGroup.Substring(0, 1).ToUpper() + strGroup.Substring(1));

            TAppSettingsManager opts = new TAppSettingsManager(false);
            string templateDir = opts.GetValue("TemplateDir", true);
            ProcessTemplate Template = new ProcessTemplate(templateDir + Path.DirectorySeparatorChar +
                "ORM" + Path.DirectorySeparatorChar +
                "DataTable.cs");

            Template.AddToCodelet("NAMESPACE", ANamespaceName);

            // load default header with license and copyright
            StreamReader sr = new StreamReader(templateDir + Path.DirectorySeparatorChar + "EmptyFileComment.txt");
            string fileheader = sr.ReadToEnd();
            sr.Close();
            fileheader = fileheader.Replace(">>>> Put your full name or just a shortname here <<<<", "auto generated");
            Template.AddToCodelet("GPLFILEHEADER", fileheader);

            foreach (TTable currentTable in AStore.GetTables())
            {
                if (currentTable.strGroup == strGroup)
                {
                    InsertTableDefinition(Template, currentTable, "TABLELOOP");
                    InsertRowDefinition(Template, currentTable, "TABLELOOP");
                }
            }

            Template.FinishWriting(AFilePath + AFileName + ".cs", ".cs", true);

            return true;
        }
    }
}