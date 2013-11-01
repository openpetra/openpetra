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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.IO;
using System.Data;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;
using Ict.Common.IO;
using Ict.Common;

namespace Ict.Tools.CodeGeneration.DataStore
{
    /// <summary>
    /// create code for reading and writing datasets from and to the database
    /// </summary>
    public class CodeGenerationDatasetAccess
    {
        /// <summary>
        /// create code for a table that is part of a dataset
        /// </summary>
        /// <param name="ATables"></param>
        /// <param name="ASqltable"></param>
        /// <param name="tabletype"></param>
        /// <param name="variablename"></param>
        /// <param name="snippetDataset"></param>
        /// <param name="ASnippetSubmitChanges"></param>
        private static void AddTableToDataset(
            List <TDataSetTable>ATables,
            TTable ASqltable,
            string tabletype,
            string variablename,
            ProcessTemplate snippetDataset,
            ProcessTemplate ASnippetSubmitChanges)
        {
            ProcessTemplate tempSnippet;

            if (ASqltable != null)
            {
                string SequenceColumn = "";
                string SequenceName = "";

                foreach (TTableField tablefield in ASqltable.grpTableField)
                {
                    // is there a field filled by a sequence?
                    // yes: get the next value of that sequence and assign to row
                    if (tablefield.strSequence.Length > 0)
                    {
                        SequenceName = tablefield.strSequence;
                        SequenceColumn = tablefield.strName;

                        // assume only one sequence per table
                        break;
                    }
                }

                tempSnippet = snippetDataset.GetSnippet("SUBMITCHANGES");
                tempSnippet.SetCodelet("ORIGTABLENAME", TTable.NiceTableName(ASqltable.strName));
                tempSnippet.SetCodelet("TABLETYPENAME", tabletype);
                tempSnippet.SetCodelet("TABLEVARIABLENAME", variablename);
                tempSnippet.SetCodelet("SQLOPERATION", "eDelete");
                tempSnippet.SetCodelet("SEQUENCENAMEANDFIELD", "");
                ASnippetSubmitChanges.InsertSnippetPrepend("SUBMITCHANGESDELETE", tempSnippet);

                tempSnippet = snippetDataset.GetSnippet("SUBMITCHANGES");
                tempSnippet.SetCodelet("ORIGTABLENAME", TTable.NiceTableName(ASqltable.strName));
                tempSnippet.SetCodelet("TABLETYPENAME", tabletype);
                tempSnippet.SetCodelet("TABLEVARIABLENAME", variablename);
                tempSnippet.SetCodelet("SQLOPERATION", "eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate");
                tempSnippet.SetCodelet("SEQUENCENAMEANDFIELD", "");

                if (SequenceName.Length > 0)
                {
                    tempSnippet.SetCodelet("SEQUENCENAMEANDFIELD", ", \"" + SequenceName + "\", \"" + SequenceColumn + "\"");

                    // look for other tables in the dataset that have a foreign key on the sequenced column
                    // eg. p_location_key_i is set when storing p_location.
                    //     now we should update p_partner_location, which still has negative values in p_location_key_i
                    foreach (TDataSetTable table in ATables)
                    {
                        foreach (TConstraint constraint in table.grpConstraint)
                        {
                            if ((constraint.strType == "foreignkey")
                                && (constraint.strOtherTable == ASqltable.strName)
                                && constraint.strOtherFields.Contains(SequenceColumn))
                            {
                                tempSnippet.SetCodelet("TABLEROWTYPE", TTable.NiceTableName(ASqltable.strName) + "Row");
                                tempSnippet.SetCodelet("SEQUENCEDCOLUMNNAME", TTable.NiceFieldName(SequenceColumn));

                                ProcessTemplate updateSnippet = snippetDataset.GetSnippet("UPDATESEQUENCEINOTHERTABLES");
                                bool canbeNull =
                                    !table.GetField(constraint.strThisFields[constraint.strOtherFields.IndexOf(SequenceColumn)]).bNotNull;
                                updateSnippet.SetCodelet("TESTFORNULL", canbeNull ? "!otherRow.Is{#REFCOLUMNNAME}Null() && " : "");
                                updateSnippet.SetCodelet("REFERENCINGTABLEROWTYPE", TTable.NiceTableName(table.tablename) + "Row");
                                updateSnippet.SetCodelet("REFERENCINGTABLENAME", table.tablealias);
                                updateSnippet.SetCodelet("REFCOLUMNNAME",
                                    TTable.NiceFieldName(constraint.strThisFields[constraint.strOtherFields.IndexOf(SequenceColumn)]));
                                updateSnippet.SetCodelet("SEQUENCEDCOLUMNNAME", TTable.NiceFieldName(SequenceColumn));

                                tempSnippet.InsertSnippet("UPDATESEQUENCEINOTHERTABLES", updateSnippet);
                            }
                        }
                    }
                }

                ASnippetSubmitChanges.InsertSnippet("SUBMITCHANGESINSERT", tempSnippet);

                ASnippetSubmitChanges.AddToCodelet("SUBMITCHANGESUPDATE", "");
            }
        }

        /// <summary>
        /// main function for generating access methods for typed data sets
        /// </summary>
        /// <param name="AInputXmlfile"></param>
        /// <param name="AOutputPath"></param>
        /// <param name="ANameSpace"></param>
        /// <param name="store"></param>
        /// <param name="groups"></param>
        /// <param name="AFilename"></param>
        public static void CreateTypedDataSets(String AInputXmlfile,
            String AOutputPath,
            String ANameSpace,
            TDataDefinitionStore store,
            string[] groups,
            string AFilename)
        {
            Console.WriteLine("processing dataset " + ANameSpace);

            string templateDir = TAppSettingsManager.GetValue("TemplateDir", true);
            ProcessTemplate Template = new ProcessTemplate(templateDir + Path.DirectorySeparatorChar +
                "ORM" + Path.DirectorySeparatorChar +
                "DataSetAccess.cs");

            // load default header with license and copyright
            Template.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(templateDir));

            Template.SetCodelet("NAMESPACE", ANameSpace);

            // if no dataset is defined yet in the xml file, the following variables can be empty
            Template.AddToCodelet("USINGNAMESPACES", "");
            Template.AddToCodelet("CONTENTDATASETSANDTABLESANDROWS", "");

            Template.AddToCodelet("USINGNAMESPACES", "using " + ANameSpace.Replace(".Server.", ".Shared.") + ";" + Environment.NewLine, false);

            TXMLParser parserDataSet = new TXMLParser(AInputXmlfile, false);
            XmlDocument myDoc = parserDataSet.GetDocument();
            XmlNode startNode = myDoc.DocumentElement;

            if (startNode.Name.ToLower() == "petradatasets")
            {
                XmlNode cur = TXMLParser.NextNotBlank(startNode.FirstChild);

                while ((cur != null) && (cur.Name.ToLower() == "importunit"))
                {
                    Template.AddToCodelet("USINGNAMESPACES", "using " + TXMLParser.GetAttribute(cur, "name") + ";" + Environment.NewLine, false);
                    Template.AddToCodelet("USINGNAMESPACES", "using " + TXMLParser.GetAttribute(cur, "name").Replace(".Shared.",
                            ".Server.") + ".Access;" + Environment.NewLine, false);
                    cur = TXMLParser.GetNextEntity(cur);
                }

                while ((cur != null) && (cur.Name.ToLower() == "dataset"))
                {
                    ProcessTemplate snippetDataset = Template.GetSnippet("TYPEDDATASET");
                    string datasetname = TXMLParser.GetAttribute(cur, "name");
                    snippetDataset.SetCodelet("DATASETNAME", datasetname);

                    ProcessTemplate snippetSubmitChanges = snippetDataset.GetSnippet("SUBMITCHANGESFUNCTION");
                    snippetSubmitChanges.AddToCodelet("DATASETNAME", datasetname);

                    List <TDataSetTable>tables = new List <TDataSetTable>();

                    XmlNode curChild = cur.FirstChild;

                    // first collect the tables
                    while (curChild != null)
                    {
                        if (curChild.Name.ToLower() == "table")
                        {
                            string tabletype = TTable.NiceTableName(TXMLParser.GetAttribute(curChild, "sqltable"));
                            string variablename = (TXMLParser.HasAttribute(curChild, "name") ?
                                                   TXMLParser.GetAttribute(curChild, "name") :
                                                   tabletype);

                            TDataSetTable table = new TDataSetTable(
                                TXMLParser.GetAttribute(curChild, "sqltable"),
                                tabletype,
                                variablename,
                                store.GetTable(tabletype));

                            tables.Add(table);
                        }

                        curChild = curChild.NextSibling;
                    }

                    foreach (TDataSetTable table in tables)
                    {
                        AddTableToDataset(tables,
                            store.GetTable(table.tableorig),
                            table.tablename,
                            table.tablealias,
                            snippetDataset,
                            snippetSubmitChanges);
                    }

                    // there is one codelet for the dataset name.
                    // only add the full submitchanges function if there are any table to submit
                    if (snippetSubmitChanges.FCodelets.Count > 1)
                    {
                        snippetDataset.InsertSnippet("SUBMITCHANGESFUNCTION", snippetSubmitChanges);
                    }
                    else
                    {
                        snippetDataset.AddToCodelet("SUBMITCHANGESFUNCTION", "");
                    }

                    Template.InsertSnippet("CONTENTDATASETSANDTABLESANDROWS", snippetDataset);

                    cur = TXMLParser.GetNextEntity(cur);
                }
            }

            Template.FinishWriting(AOutputPath + Path.DirectorySeparatorChar + AFilename + "-generated.cs", ".cs", true);
        }
    }
}