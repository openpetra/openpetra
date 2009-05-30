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
using System.Collections;
using System.Xml;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.CSharp;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;
using Ict.Common.IO;
using Ict.Common;

namespace Ict.Tools.CodeGeneration.DataStore
{
    public class codeGenerationDataset
    {
        public static CodeConstructor DataSetConstructor(String ADataSetName)
        {
            CodeConstructor ReturnValue;

            ReturnValue = new CodeConstructor();
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Overloaded;
            ReturnValue.BaseConstructorArgs.Add(CodeDom._Const(ADataSetName));
            ReturnValue.Comments.Add(new CodeCommentStatement("auto generated", true));
            return ReturnValue;
        }

        public static CodeConstructor DataSetConstructorWithName()
        {
            CodeConstructor ReturnValue;

            ReturnValue = new CodeConstructor();
            ReturnValue.Parameters.Add(CodeDom.Param(typeof(String), "ADatasetName"));
            ReturnValue.Comments.Add(new CodeCommentStatement("auto generated", true));
            ReturnValue.BaseConstructorArgs.Add(CodeDom.Local("ADatasetName"));
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Overloaded;
            return ReturnValue;
        }

        public static CodeConstructor DataSetConstructorSerialize()
        {
            CodeConstructor ReturnValue;

            ReturnValue = new CodeConstructor();
            ReturnValue.Comments.Add(new CodeCommentStatement("auto generated for serialization", true));
            ReturnValue.Parameters.Add(CodeDom.Param(typeof(SerializationInfo), "info"));
            ReturnValue.Parameters.Add(CodeDom.Param(typeof(StreamingContext), "context"));
            ReturnValue.BaseConstructorArgs.Add(CodeDom.Local("info"));
            ReturnValue.BaseConstructorArgs.Add(CodeDom.Local("context"));
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Overloaded;
            return ReturnValue;
        }

        /*
         * function DataSetGetChanges(): CodeMemberMethod;
         * var
         * loopContentRemoveEmptyTable: CodeStatementArray;
         * loopContentTable, loopContentRow, loopContentColumn: CodeStatementArray;
         * begin
         * Result := new  CodeMemberMethod();
         * result.Name := 'GetChanges';
         * result.ReturnType := CodeDom.TypeRef(typeof(System.Data.DataSet));
         * Result.Parameters.Add(CodeDom.Param(typeof(Boolean), 'removeEmptyTables'));
         * Result.Attributes := MemberAttributes.Public or MemberAttributes.Final or MemberAttributes.Overloaded or MemberAttributes.Override;
         * Result.Statements.Add( VarDecl(typeof(System.Data.DataSet), 'ds', nil));
         * Result.Statements.Add( VarDecl(typeof(System.Data.DataTable), 'tab', nil));
         * Result.Statements.Add( VarDecl(typeof(System.Data.DataRow), 'row', nil));
         * Result.Statements.Add( VarDecl(typeof(System.Int16), 'columnNr', nil));
         * Result.Statements.Add( VarDecl(typeof(System.Int16), 'countTab', nil));
         * Result.Statements.Add( VarDecl(typeof(System.Int16), 'countRow', nil));
         * Result.Statements.Add( VarDecl(typeof(System.Int16), 'countCol', nil));
         *
         * Result.Statements.Add( Let (CodeDom.Local('ds'),
         * BaseMethodInvoke('GetChanges', CodeExpression[].Create())));
         *
         *
         * loopContentRemoveEmptyTable := new  CodeStatementArray(
         *   // tab := ds.Tables[countTab] as System.Data.DataTable
         *   Let(CodeDom.Local('tab'),
         *     CodeDom.Cast(typeof(System.Data.DataTable), IndexerRef(CodeDom.PropRef(CodeDom.Local('ds'), 'Tables'),
         *       CodeExpression[].Create(CodeDom.Local('countTab'))))),
         *   // if (table.Rows.Count = 0) then
         *   CodeConditionStatement.Create (
         *     Equals (CodeDom.PropRef(CodeDom.PropRef(CodeDom.Local('tab'), 'Rows'), 'Count'), CodeDom._Const((System.Object)0)),
         *     CodeStatementArray.Create(CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.PropRef(CodeDom.Local('ds'), 'Tables'), 'Remove',
         *       CodeExpression[].Create(CodeDom.Local('tab')))),
         *       Let(CodeDom.Local('countTab'), CodeDom._Const((System.Object)0)) // countTab := 0
         *       ),
         *     CodeStatementArray.Create(CodeDom.Let(CodeDom.Local('countTab'),  // countTab := countTab + 1
         *       CodeBinaryOperatorExpression.Create(CodeDom.Local('countTab'),
         *       CodeBinaryOperatorType.Add,
         *       CodeDom._Const(System.Object(1))))))
         *   );
         *
         * Result.Statements.Add( CodeConditionStatement.Create (
         * Equals (CodeDom.Local('removeEmptyTables'), CodeDom._Const((System.Object)true)), // if (removeEmptyTables) then
         * CodeStatementArray.Create (
         * Let(CodeDom.Local('countTab'), CodeDom._Const((System.Object)0)), // countTab := 0
         * CodeIterationStatement.Create(
         *   nil,
         *   Inequals(CodeDom.Local('countTab'), CodeDom.PropRef(CodeDom.PropRef(CodeDom.Local('ds'), 'Tables'), 'Count')), // countTab <> ds.Tables.Count
         *   nil,
         *   loopContentRemoveEmptyTable
         * )),
         * // no else
         * CodeStatementArray.Create()
         * ));
         *
         * loopContentColumn := new  CodeStatementArray(
         * CodeConditionStatement.Create(
         *   Equals(
         *     IndexerRef(CodeDom.Local('row'),
         *       CodeExpression[].Create(CodeDom.Local('countCol'), CodeSnippetExpression.Create('DataRowVersion.Original'))),
         *     IndexerRef(CodeDom.Local('row'),
         *       CodeExpression[].Create(CodeDom.Local('countCol'), CodeSnippetExpression.Create('DataRowVersion.Current')))
         *   ),
         *   CodeStatementArray.Create(
         *     //  row[theColumn] := nil;
         *     Let (IndexerRef(CodeDom.PropRef(CodeDom.Local('row'), 'Item'), CodeExpression[].Create(CodeDom.Local('countCol'))), CodeDom._Const(nil))
         *   )
         * )
         * );
         *
         * loopContentRow := new  CodeStatementArray(
         * // row := tab.Rows[countRow] as System.Data.DataRow
         * Let(CodeDom.Local('row'),
         *   CodeDom.Cast(typeof(System.Data.DataRow), IndexerRef(CodeDom.PropRef(CodeDom.Local('tab'), 'Rows'),
         *     CodeExpression[].Create(CodeDom.Local('countRow'))))),
         * CodeIterationStatement.Create(
         *   Let(CodeDom.Local('countCol'), CodeDom._Const((System.Object)0)), // countCol := 0
         *   Inequals(CodeDom.Local('countCol'), CodeDom.PropRef(CodeDom.PropRef(CodeDom.Local('tab'), 'Columns'), 'Count')), // countCol <> tab.Columns.Count
         *   Let(CodeDom.Local('countCol'),  // countCol := countCol + 1
         *       CodeBinaryOperatorExpression.Create(CodeDom.Local('countCol'),
         *       CodeBinaryOperatorType.Add,
         *       CodeDom._Const(System.Object(1)))),
         *   loopContentColumn
         * )
         * );
         *
         * loopContentTable := new  CodeStatementArray(
         * // tab := ds.Tables[countTab] as System.Data.DataTable
         * Let(CodeDom.Local('tab'),
         *   CodeDom.Cast(typeof(System.Data.DataTable), IndexerRef(CodeDom.PropRef(CodeDom.Local('ds'), 'Tables'),
         *     CodeExpression[].Create(CodeDom.Local('countTab'))))),
         * CodeIterationStatement.Create(
         *   Let(CodeDom.Local('countRow'), CodeDom._Const((System.Object)0)), // countRow := 0
         *   Inequals(CodeDom.Local('countRow'), CodeDom.PropRef(CodeDom.PropRef(CodeDom.Local('tab'), 'Rows'), 'Count')), // countRow <> tab.Rows.Count
         *   Let(CodeDom.Local('countRow'),  // countRow := countRow + 1
         *       CodeBinaryOperatorExpression.Create(CodeDom.Local('countRow'),
         *       CodeBinaryOperatorType.Add,
         *       CodeDom._Const(System.Object(1)))),
         *   loopContentRow
         * )
         * );
         *
         * Result.Statements.Add(CodeIterationStatement.Create(
         * Let(CodeDom.Local('countTab'), CodeDom._Const((System.Object)0)), // countTab := 0
         * Inequals(CodeDom.Local('countTab'), CodeDom.PropRef(CodeDom.PropRef(CodeDom.Local('ds'), 'Tables'), 'Count')), // countTab <> ds.Tables.Count
         *     Let(CodeDom.Local('countTab'),  // countTab := countTab + 1
         *       CodeBinaryOperatorExpression.Create(CodeDom.Local('countTab'),
         *       CodeBinaryOperatorType.Add,
         *       CodeDom._Const(System.Object(1)))),
         * loopContentTable
         * ));
         *
         * Result.Statements.Add(CodeDom.Return(CodeDom.Local('ds')));
         * end;
         */

        public static CodeMemberMethod DataSetGetChanges(String datasetName)
        {
            CodeMemberMethod ReturnValue;

            ReturnValue = new CodeMemberMethod();
            ReturnValue.Name = "GetChangesTyped";
            ReturnValue.Comments.Add(new CodeCommentStatement("auto generated", true));
            ReturnValue.ReturnType = CodeDom.TypeRef(datasetName);
            ReturnValue.Parameters.Add(CodeDom.Param(typeof(Boolean), "removeEmptyTables"));
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.New;
            ReturnValue.Statements.Add(CodeDom.Return(CodeDom.Cast(datasetName, CodeDom.BaseMethodInvoke("GetChangesTyped", new CodeExpression[]
                            { CodeDom.Local("removeEmptyTables") }))));
            return ReturnValue;
        }

        /// add a property for the table to the dataset
        public static CodeMemberProperty DataSetTableProperty(string tableName, string tableAlias)
        {
            CodeMemberProperty ReturnValue;

            // property
            ReturnValue = new CodeMemberProperty();
            ReturnValue.Name = tableAlias;
            ReturnValue.Comments.Add(new CodeCommentStatement("auto generated", true));
            ReturnValue.Type = CodeDom.TypeRef(tableName + "Table");
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            ReturnValue.GetStatements.Add(CodeDom.Return(CodeDom.FieldRef("Table" + tableAlias)));

            // Result.GetStatements.Add (CodeDom.Return(
            // CodeDom.Cast(tableName + 'Table',
            // IndexerRef(FieldRef('Tables'), CodeExpression[].Create(CodeDom._Const(tableAlias))))
            // ));
            return ReturnValue;
        }

        /// add table to the Dataset
        public static CodeMemberField CreateDataSetTable(string tableName, string tableAlias)
        {
            CodeMemberField ReturnValue;

            ReturnValue = new CodeMemberField();
            ReturnValue.Name = "Table" + tableAlias;
            ReturnValue.Type = CodeDom.TypeRef(tableName + "Table");
            ReturnValue.Attributes = MemberAttributes.Private;
            return ReturnValue;
        }

        public static TDataSetTable GetTableInDataset(ArrayList datasetTables, string tablename)
        {
            foreach (TDataSetTable datasetTable in datasetTables)
            {
                if ((datasetTable.tableorig == tablename) || (datasetTable.tablealias == tablename))
                {
                    return datasetTable;
                }
            }

            return new TDataSetTable(tablename, "", "", null, new ArrayList(), new ArrayList());
        }

        /*
         * function addConstraintsCode(store: TDataDefinitionStore; datasetTables: ArrayList): CodeMemberMethod;
         * var
         * datasetTable: TDataSetTable;
         * constraint: TConstraint;
         * otherTable: TDataSetTable;
         *
         * begin
         * Result := new  CodeMemberMethod();
         * Result.Name := 'AddConstraints';
         *
         * for datasetTable in datasetTables do
         * begin
         * // todo??? add constraints also for custom keys?
         * if (datasetTable.tableorig.Length <> 0) then
         * begin
         * // todo: we need the constraints from the added fields
         * for constraint in datasetTable.Constraints do
         * begin
         *   if (constraint.strType = 'foreignkey') then
         *   begin
         *     otherTable := getTableInDataset(datasetTables, constraint.strOtherTable);
         *     if (otherTable.tablealias.Length > 0) then
         *     begin
         *       Result.Statements.Add(
         *         CodeDom.MethodInvoke(CodeDom.PropRef(CodeDom.PropRef('Table'+datasetTable.tablealias), 'Constraints'),
         *                      'Add',
         *                      CodeExpression[].Create(
         *                         CodeDom._Const(niceKeyName(constraint)),
         *                         CodeDom.PropRef(CodeDom.PropRef('Table'+otherTable.tableAlias),
         *                           niceKeyName(store.GetTable(constraint.strOtherTable).GetConstraint(constraint.strOtherFields ))),
         *                         CodeDom.PropRef(CodeDom.PropRef('Table'+datasetTable.tableAlias),
         *                           niceKeyName(constraint)))
         *                      )
         *         );
         *     end;
         *   end;
         * end;
         * end;
         * end;
         * end;
         */

        public static TDataSetTable CreateTableFromSql(CodeNamespace cns, TDataDefinitionStore store, string datasetName, XmlNode cur)
        {
            string tableName;
            string tableAlias;
            string tableOrig;
            XmlNode curFields;
            ArrayList constraints;
            TTableField field;
            ArrayList fields;

            fields = new ArrayList();
            tableOrig = TXMLParser.GetAttribute(cur, "sqltable");
            tableName = TTable.NiceTableName(tableOrig);

            if (TXMLParser.HasAttribute(cur, "alias") == true)
            {
                tableAlias = TXMLParser.GetAttribute(cur, "alias");
            }
            else if (TXMLParser.HasAttribute(cur, "name") == true)
            {
                tableAlias = TXMLParser.GetAttribute(cur, "name");
            }
            else
            {
                tableAlias = tableName;
            }

            constraints = new ArrayList();

            foreach (TConstraint constraint in store.GetTable(tableOrig).grpConstraint.List)
            {
                constraints.Add(constraint);
            }

            if ((cur.Name.ToLower() == "table") && (cur.FirstChild != null))
            {
                // there are customised or other sql fields added to the existing table.
                curFields = cur.FirstChild;

                while (curFields != null)
                {
                    if (curFields.Name.ToLower() == "field")
                    {
                        field = new TTableField();
                        field =
                            store.GetTable(TXMLParser.GetAttribute(curFields,
                                    "sqltable")).GetField(TXMLParser.GetAttribute(curFields,
                                    "sqlfield"));
                        fields.Add(field);
                    }
                    else if (curFields.Name.ToLower() == "customfield")
                    {
                        field = new TTableField();
                        field.strName = TXMLParser.GetAttribute(curFields, "name");
                        field.strType = TXMLParser.GetAttribute(curFields, "type").ToLower();
                        field.strTypeDotNet = null;
                        field.strNameDotNet = field.strName;
                        field.iLength = TXMLParser.GetIntAttribute(curFields, "length");
                        field.bNotNull = (TXMLParser.GetAttribute(curFields, "notnull") == "yes");
                        field.strTableName = "";
                        fields.Add(field);
                    }

                    curFields = curFields.NextSibling;
                }

                cns.Types.Add(codeGenerationTable.DerivedTable(store, fields, tableAlias, datasetName + tableAlias + "Table", datasetName +
                        tableAlias + "Row", tableName, ref constraints));
                cns.Types.Add(codeGenerationRow.DerivedRow(fields, datasetName + tableAlias + "Table", datasetName + tableAlias + "Row",
                        tableName));
                tableName = datasetName + tableAlias;
            }

            return new TDataSetTable(tableOrig, tableName, tableAlias, store.GetTable(tableOrig), constraints, fields);
        }

        public static TDataSetTable CreateCustomTable(CodeNamespace cns, TDataDefinitionStore store, string datasetName, XmlNode cur)
        {
            string tableName;
            string tableAlias;
            string tableOrig;
            XmlNode curFields;
            ArrayList fields;
            ArrayList constraints;
            TConstraint primaryKey;
            bool fieldExists;

            fields = new ArrayList();
            tableName = TXMLParser.GetAttribute(cur, "name");
            tableAlias = tableName;
            tableOrig = tableName;
            constraints = new ArrayList();
            curFields = cur.FirstChild;
            primaryKey = null;

            while (curFields != null)
            {
                if (curFields.Name.ToLower() == "field")
                {
                    // make a copy of the field, otherwise strNameDotNet below will change the field of the petraxml
                    TTableField field =
                        new TTableField(store.GetTable(TXMLParser.GetAttribute(curFields,
                                    "sqltable")).GetField(TXMLParser.GetAttribute(
                                    curFields, "sqlfield")));

                    if (TXMLParser.HasAttribute(curFields, "name"))
                    {
                        field.strNameDotNet = TXMLParser.GetAttribute(curFields, "name");
                    }

                    fields.Add(field);
                }
                else if (curFields.Name.ToLower() == "customfield")
                {
                    TTableField field = new TTableField();
                    field.strName = TXMLParser.GetAttribute(curFields, "name");
                    field.strType = TXMLParser.GetAttribute(curFields, "type").ToLower();
                    field.strTypeDotNet = null;
                    field.strNameDotNet = field.strName;
                    field.iLength = TXMLParser.GetIntAttribute(curFields, "length");
                    field.bNotNull = (TXMLParser.GetAttribute(curFields, "notnull") == "yes");
                    field.strTableName = "";
                    fields.Add(field);
                }
                else if (curFields.Name.ToLower() == "primarykey")
                {
                    primaryKey = new TConstraint();
                    primaryKey.strName = TXMLParser.GetAttribute(curFields, "name");
                    primaryKey.strType = curFields.Name;             /// foreignkey, uniquekey, primarykey
                    primaryKey.strThisFields = StringHelper.StrSplit(TXMLParser.GetAttribute(curFields, "thisFields"), ",");
                    primaryKey.strOtherTable = TXMLParser.GetAttribute(curFields, "otherTable");
                    primaryKey.strOtherFields = StringHelper.StrSplit(TXMLParser.GetAttribute(curFields, "otherFields"), ",");

                    // check if the fields of the primary key do exist!
                    foreach (String primKeyField in primaryKey.strThisFields)
                    {
                        fieldExists = false;

                        foreach (TTableField field in fields)
                        {
                            if ((field.strName == primKeyField) || (field.strNameDotNet == primKeyField)
                                || (TTable.NiceFieldName(field.strName) == primKeyField))
                            {
                                fieldExists = true;
                            }
                        }

                        if (fieldExists == false)
                        {
                            System.Console.WriteLine(
                                "primarykey in custom table " + tableName + " references non existing field " + primKeyField);
                            Environment.Exit(1);
                        }
                    }
                }

                curFields = curFields.NextSibling;
            }

            cns.Types.Add(codeGenerationTable.CustomTable(store, fields, tableAlias, datasetName + tableAlias + "Table", datasetName +
                    tableAlias + "Row", ref constraints, primaryKey));
            cns.Types.Add(codeGenerationRow.CustomRow(fields, datasetName + tableAlias + "Table", datasetName + tableAlias + "Row"));
            tableName = datasetName + tableAlias;
            return new TDataSetTable(tableOrig, tableName, tableAlias, null, constraints, fields);
        }

        public static CodeTypeDeclaration CreateDataSet(CodeNamespace cns, TDataDefinitionStore store, string datasetName, XmlNode cur)
        {
            CodeTypeDeclaration ReturnValue;
            CodeMemberMethod myInitVarsCode;
            CodeMemberMethod myInitTablesCode;
            CodeMemberMethod myInitTablesDSCode;
            CodeMemberMethod myMapTablesCode;
            CodeMemberMethod myInitConstraintsCode;
            ArrayList datasetTables;
            TDataSetTable otherTable;
            XmlNode FirstNode;
            CodeMethodInvokeExpression AddConstraint;

            FirstNode = cur;
            ReturnValue = new CodeTypeDeclaration(datasetName);
            ReturnValue.IsClass = true;
            Console.WriteLine(" ********* Generate DataSet " + datasetName + " ********* ");
            ReturnValue.CustomAttributes.Add(new CodeAttributeDeclaration("Serializable"));
            ReturnValue.BaseTypes.Add("TTypedDataSet");
            ReturnValue.Comments.Add(new CodeCommentStatement("auto generated", true));

            // InitTables
            myInitTablesCode = new CodeMemberMethod();
            myInitTablesCode.Attributes = MemberAttributes.Override | MemberAttributes.Family | MemberAttributes.Overloaded;
            myInitTablesCode.Name = "InitTables";
            myInitTablesCode.Comments.Add(new CodeCommentStatement("auto generated", true));

            // InitTables with Parameter dataset
            myInitTablesDSCode = new CodeMemberMethod();
            myInitTablesDSCode.Attributes = MemberAttributes.Override | MemberAttributes.Family | MemberAttributes.Overloaded;
            myInitTablesDSCode.Name = "InitTables";
            myInitTablesDSCode.Comments.Add(new CodeCommentStatement("auto generated", true));
            myInitTablesDSCode.Parameters.Add(CodeDom.Param(typeof(DataSet), "ds"));
            myMapTablesCode = new CodeMemberMethod();
            myMapTablesCode.Attributes = MemberAttributes.Override | MemberAttributes.Family;
            myMapTablesCode.Name = "MapTables";
            myMapTablesCode.Comments.Add(new CodeCommentStatement("auto generated", true));
            myMapTablesCode.Statements.Add(CodeDom.MethodInvoke("InitVars", new CodeExpression[] { }));
            myMapTablesCode.Statements.Add(CodeDom.BaseMethodInvoke("MapTables", new CodeExpression[] { }));

            // InitVars
            myInitVarsCode = new CodeMemberMethod();
            myInitVarsCode.Attributes = MemberAttributes.Override | MemberAttributes.Public;
            myInitVarsCode.Name = "InitVars";
            myInitVarsCode.Comments.Add(new CodeCommentStatement("auto generated", true));
            myInitVarsCode.Statements.Add(CodeDom.Let(CodeDom.FieldRef("DataSetName"), CodeDom._Const(datasetName)));

            // InitConstraints
            myInitConstraintsCode = new CodeMemberMethod();
            myInitConstraintsCode.Attributes = MemberAttributes.Override | MemberAttributes.Family;
            myInitConstraintsCode.Comments.Add(new CodeCommentStatement("auto generated", true));
            myInitConstraintsCode.Name = "InitConstraints";
            datasetTables = new ArrayList();

            while (cur != null)
            {
                if (cur.Name.ToLower() == "table")
                {
                    datasetTables.Add(CreateTableFromSql(cns, store, datasetName, cur));
                }

                if (cur.Name.ToLower() == "customtable")
                {
                    datasetTables.Add(CreateCustomTable(cns, store, datasetName, cur));
                }

                cur = TXMLParser.GetNextEntity(cur);
            }

            foreach (TDataSetTable datasetTable in datasetTables)
            {
                myInitTablesCode.Statements.Add(CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.PropRef("Tables"), "Add", new CodeExpression[]
                            { CodeDom._New(datasetTable.tablename + "Table",
                                  new CodeExpression[]
                                  { CodeDom._Const(datasetTable.tablealias) }) })));
                ReturnValue.Members.Add(CreateDataSetTable(datasetTable.tablename, datasetTable.tablealias));
                myInitTablesDSCode.Statements.Add(new CodeConditionStatement(CodeDom.Inequals(CodeDom.MethodInvoke(CodeDom.PropRef(CodeDom.
                                    Local("ds"),
                                    "Tables"),
                                "IndexOf",
                                new CodeExpression[]
                                { CodeDom._Const(
                                      datasetTable.
                                      tablealias) }),
                            CodeDom._Const((System.Object)(Int32) (-1))),
                        new CodeStatement[]
                        { CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.PropRef("Tables"),
                                  "Add", new CodeExpression[]
                                  { CodeDom._New(datasetTable.
                                        tablename +
                                        "Table",
                                        new
                                        CodeExpression
                                        []
                                        { CodeDom._Const(
                                              datasetTable
                                              .
                                              tablealias) }) })) },
                        new CodeStatement[] { }));
            }

            foreach (TDataSetTable datasetTable in datasetTables)
            {
                myInitVarsCode.Statements.Add(CodeDom.Let(CodeDom.FieldRef("Table" + datasetTable.tablealias),
                        CodeDom.Cast(datasetTable.tablename + "Table",
                            CodeDom.
                            IndexerRef(CodeDom.FieldRef("Tables"),
                                new CodeExpression[] { CodeDom._Const(datasetTable.
                                                           tablealias) }))));
                myMapTablesCode.Statements.Add(new CodeConditionStatement(CodeDom.Inequals(CodeDom.PropRef("Table" + datasetTable.tablealias),
                            CodeDom._Const(null)), new CodeStatement[]
                        { CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.PropRef("Table" +
                                      datasetTable.
                                      tablealias),
                                  "InitVars",
                                  new CodeExpression[] { }
                                  )) }, new CodeStatement[] { }
                        ));
                ReturnValue.Members.Add(DataSetTableProperty(datasetTable.tablename, datasetTable.tablealias));
            }

            // constraints
            foreach (TDataSetTable datasetTable in datasetTables)
            {
                // todo??? add constraints also for custom keys?
                if (datasetTable.tableorig.Length != 0)
                {
                    // todo: we need the constraints from the added fields
                    foreach (TConstraint constraint in datasetTable.Constraints)
                    {
                        if (constraint.strType == "foreignkey")
                        {
                            otherTable = GetTableInDataset(datasetTables, constraint.strOtherTable);

                            if (otherTable.tablealias.Length > 0)
                            {
                                AddConstraint = CodeDom.MethodInvoke(CodeDom.PropRef(
                                        "FConstraints"), "Add", new CodeExpression[]
                                    { CodeDom._New("TTypedConstraint", new CodeExpression[]
                                          { CodeDom._Const(TTable.NiceKeyName(
                                                    constraint)),
                                            CodeDom._Const(otherTable.tablealias),
                                            CodeDom.NewArray(typeof(String),
                                                codeGenerationPetra.
                                                FieldListToStringExpressionArray(
                                                    otherTable,
                                                    constraint
                                                    .strOtherFields)),
                                            CodeDom._Const(datasetTable.tablealias),
                                            CodeDom.NewArray(typeof(String),
                                                codeGenerationPetra.
                                                FieldListToStringExpressionArray(
                                                    datasetTable,
                                                    constraint
                                                    .strThisFields)
                                                ) }) });
                                myInitConstraintsCode.Statements.Add(new CodeConditionStatement(new
                                        CodeBinaryOperatorExpression(
                                            CodeDom.Inequals(
                                                CodeDom.
                                                PropRef(
                                                    "Table"
                                                    +
                                                    otherTable
                                                    .
                                                    tablealias),
                                                CodeDom
                                                .
                                                _Const(null)),
                                            CodeBinaryOperatorType
                                            .BooleanAnd,
                                            CodeDom
                                            .Inequals(CodeDom.
                                                PropRef(
                                                    "Table"
                                                    +
                                                    datasetTable.tablealias),
                                                CodeDom
                                                ._Const(
                                                    null))),
                                        new CodeStatement[]
                                        { CodeDom.Eval(AddConstraint) },
                                        new CodeStatement[] { }));
                            }
                        }
                    }
                }
            }

            // go back to beginning of dataset
            cur = FirstNode;

            while (cur != null)
            {
                if (cur.Name.ToLower() == "customconstraint")
                {
                    otherTable = GetTableInDataset(datasetTables, TXMLParser.GetAttribute(cur, "referencedTable"));
                    TDataSetTable datasetTable = GetTableInDataset(datasetTables, TXMLParser.GetAttribute(cur, "refererTable"));

                    if ((otherTable != null) && (datasetTable != null))
                    {
                        myInitConstraintsCode.Statements.Add(CodeDom.MethodInvoke(CodeDom.PropRef("FConstraints"), "Add",
                                new CodeExpression[]
                                { CodeDom._New("TTypedConstraint",
                                      new CodeExpression[]
                                      { CodeDom._Const(TXMLParser.
                                            GetAttribute(cur,
                                                "name")),
                                        CodeDom._Const(otherTable.tablealias),
                                        CodeDom.NewArray(typeof(String),
                                            codeGenerationPetra
                                            .
                                            FieldListToStringExpressionArray(
                                                otherTable,
                                                StringHelper
                                                .
                                                StrSplit(
                                                    TXMLParser
                                                    .
                                                    GetAttribute(
                                                        cur,
                                                        "referencedFields"),
                                                    ","))),
                                        CodeDom._Const(datasetTable.
                                            tablealias),
                                        CodeDom.NewArray(typeof(String),
                                            codeGenerationPetra
                                            .
                                            FieldListToStringExpressionArray(
                                                datasetTable,
                                                StringHelper
                                                .
                                                StrSplit(
                                                    TXMLParser
                                                    .
                                                    GetAttribute(cur,
                                                        "refererFields"), ",")
                                                )) }) }));
                    }
                }

                if (cur.Name.ToLower() == "customrelation")
                {
                    otherTable = GetTableInDataset(datasetTables, TXMLParser.GetAttribute(cur, "childTable"));
                    TDataSetTable datasetTable = GetTableInDataset(datasetTables, TXMLParser.GetAttribute(cur, "parentTable"));

                    if ((otherTable != null) && (datasetTable != null))
                    {
                        myInitConstraintsCode.Statements.Add(CodeDom.MethodInvoke(CodeDom.PropRef("FRelations"), "Add",
                                new CodeExpression[]
                                { CodeDom._New("TTypedRelation",
                                      new CodeExpression[]
                                      { CodeDom._Const(TXMLParser.
                                            GetAttribute(cur,
                                                "name")),
                                        CodeDom._Const(datasetTable.
                                            tablealias),
                                        CodeDom.NewArray(typeof(String),
                                            codeGenerationPetra
                                            .
                                            FieldListToStringExpressionArray(
                                                datasetTable,
                                                StringHelper
                                                .
                                                StrSplit(
                                                    TXMLParser
                                                    .
                                                    GetAttribute(cur,
                                                        "parentFields"),
                                                    ","))),
                                        CodeDom._Const(otherTable.tablealias),
                                        CodeDom.NewArray(typeof(String),
                                            codeGenerationPetra
                                            .
                                            FieldListToStringExpressionArray(
                                                otherTable,
                                                StringHelper
                                                .
                                                StrSplit(
                                                    TXMLParser
                                                    .
                                                    GetAttribute(cur,
                                                        "childFields"),
                                                    ","))),
                                        CodeDom._Const((System.Object)(
                                                TXMLParser.
                                                GetAttribute(
                                                    cur,
                                                    "createConstraints")
                                                == "true")
                                            ) }) }));
                    }
                }

                cur = TXMLParser.GetNextEntity(cur);
            }

            ReturnValue.Members.Add(DataSetConstructor(datasetName));
            ReturnValue.Members.Add(DataSetConstructorSerialize());
            ReturnValue.Members.Add(DataSetConstructorWithName());
            ReturnValue.Members.Add(DataSetGetChanges(datasetName));
            ReturnValue.Members.Add(myInitTablesCode);
            ReturnValue.Members.Add(myInitTablesDSCode);
            ReturnValue.Members.Add(myMapTablesCode);
            ReturnValue.Members.Add(myInitVarsCode);
            ReturnValue.Members.Add(myInitConstraintsCode);
            return ReturnValue;
        }

        public static void CreateTypedDataSets(String AInputXmlfile,
            String AOutputPath,
            String ANameSpace,
            TDataDefinitionStore store,
            string[] groups,
            string AFilename)
        {
            TXMLParser parserDataSet;
            XmlDocument myDoc;
            XmlNode startNode;
            XmlNode cur;
            string datasetName;
            FileStream outFile;
            String OutFileName;
            TextWriter tw;
            CodeNamespace cns;
            CodeTypeDeclaration myDataSet;

            Console.WriteLine("processing dataset " + ANameSpace);
            OutFileName = AOutputPath + '/' + AFilename + ".cs";
            outFile = new FileStream(OutFileName + ".new", FileMode.Create, FileAccess.Write);

            if (outFile == null)
            {
                return;
            }

            CSharpCodeProvider gen = new CSharpCodeProvider();
            cns = new CodeNamespace(ANameSpace.Substring(0, ANameSpace.Length - ".Datasets".Length));
            tw = new StreamWriter(outFile);
            tw.WriteLine("/* Auto generated with nant generateORM");
            tw.WriteLine(" * based on " + System.IO.Path.GetFileName(AInputXmlfile));
            tw.WriteLine(" * Do not modify this file manually!");
            tw.WriteLine(" */");
            cns.Imports.Add(new CodeNamespaceImport("Ict.Common"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Common.Data"));
            parserDataSet = new TXMLParser(AInputXmlfile, false);
            myDoc = parserDataSet.GetDocument();
            startNode = myDoc.DocumentElement;

            if (startNode.Name.ToLower() == "petradatasets")
            {
                cns.Imports.Add(new CodeNamespaceImport("System"));
                cns.Imports.Add(new CodeNamespaceImport("System.Data"));
                cns.Imports.Add(new CodeNamespaceImport("System.Data.Odbc"));
                cur = TXMLParser.NextNotBlank(startNode.FirstChild);

                while ((cur != null) && (cur.Name.ToLower() == "importunit"))
                {
                    cns.Imports.Add(new CodeNamespaceImport(TXMLParser.GetAttribute(cur, "name")));
                    cur = TXMLParser.GetNextEntity(cur);
                }

                while ((cur != null) && (cur.Name.ToLower() == "dataset"))
                {
                    datasetName = TXMLParser.GetAttribute(cur, "name");
                    myDataSet = CreateDataSet(cns, store, datasetName, cur.FirstChild);
                    cns.Types.Add(myDataSet);
                    cur = TXMLParser.GetNextEntity(cur);
                }
            }

            CodeGeneratorOptions opt = new CodeGeneratorOptions();
            opt.BracingStyle = "C";
            gen.GenerateCodeFromNamespace(cns, tw, opt);
            tw.Close();

            if (TTextFile.UpdateFile(OutFileName) == true)
            {
                System.Console.WriteLine("   Writing file " + OutFileName);
            }
        }
    }
}