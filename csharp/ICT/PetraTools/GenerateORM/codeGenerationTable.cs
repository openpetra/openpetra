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
using System.CodeDom;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Web;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.DataStore
{
    public class codeGenerationTable
    {
        public static CodeConstructor TableConstructorSerialize()
        {
            CodeConstructor ReturnValue;

            ReturnValue = new CodeConstructor();
            ReturnValue.Comments.Add(new CodeCommentStatement("constructor for serialization", true));
            ReturnValue.Parameters.Add(CodeDom.Param(typeof(SerializationInfo), "info"));
            ReturnValue.Parameters.Add(CodeDom.Param(typeof(StreamingContext), "context"));
            ReturnValue.BaseConstructorArgs.Add(CodeDom.Local("info"));
            ReturnValue.BaseConstructorArgs.Add(CodeDom.Local("context"));
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Overloaded;
            return ReturnValue;
        }

        // add column to the Table

        public static CodeMemberField TableColumn(TTableField field)
        {
            CodeMemberField Result = new CodeMemberField();

            Result.Name = "Column" + TTable.NiceFieldName(field);
            Result.Type = new CodeTypeReference("DataColumn");
            Result.Comments.Add(new CodeCommentStatement(HttpUtility.HtmlEncode(field.strDescription), true));
            Result.Attributes = MemberAttributes.Public;
            return Result;
        }

        /// the row indexer in the table
        public static CodeMemberProperty TableRowIndexer(string rowName, Boolean needNew)
        {
            CodeMemberProperty Result = new CodeMemberProperty();

            // http:msdn2.microsoft.com/enus/library/system.codedom.codememberproperty.parameters.aspx
            // In general, properties do not have parameters. CodeDom supports an exception to this. For any property that has the special name "Item" and one or more parameters, it will declare an indexer property for the class. However, not all
            // languages support the declaration of indexers.
            Result.Name = "Item";     // indexer
            Result.Comments.Add(new CodeCommentStatement("Access a typed row by index", true));

            // need to make it the default property
            // http:forums.microsoft.com/MSDN/ShowPost.aspx?PostID=707143&SiteID=1
            if (needNew)
            {
                Result.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.New;
            }
            else
            {
                Result.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            }

            Result.Type = CodeDom.TypeRef(rowName);
            Result.Parameters.Add(CodeDom.Param(typeof(Int32), "i"));
            Result.HasSet = false;
            Result.GetStatements.Add(CodeDom.Return(CodeDom.Cast(rowName, CodeDom.IndexerRef(CodeDom.PropRef("Rows"), new CodeExpression[]
                            { CodeDom.ParamRef("i") }))));
            return Result;
        }

        public static CodeMemberProperty TableRowIndexer(string rowName)
        {
            return TableRowIndexer(rowName, true);
        }

        // creates a method that returns a new row System.Object for the table

        public static CodeMemberMethod TableNewRowFromBuilder(string rowName)
        {
            CodeMemberMethod ReturnValue;

            ReturnValue = new CodeMemberMethod();
            ReturnValue.Name = "NewRowFromBuilder";
            ReturnValue.Comments.Add(new CodeCommentStatement("new typed row using DataRowBuilder", true));
            ReturnValue.Attributes = MemberAttributes.Family | MemberAttributes.Override;
            ReturnValue.ReturnType = CodeDom.TypeRef(typeof(DataRow));
            ReturnValue.Parameters.Add(CodeDom.Param(typeof(DataRowBuilder), "builder"));
            ReturnValue.Statements.Add(CodeDom.Return(CodeDom._New(rowName, new CodeExpression[]
                        { CodeDom.ParamRef("builder") })));
            return ReturnValue;
        }

        public static CodeMemberMethod TableNewRowShort(string rowName)
        {
            CodeMemberMethod Result;

            Result = new CodeMemberMethod();
            Result.Name = "NewRowTyped";
            Result.Comments.Add(new CodeCommentStatement("create a new typed row, always with default values", true));
            Result.Attributes = MemberAttributes.Final | MemberAttributes.Public | MemberAttributes.Overloaded;
            Result.ReturnType = CodeDom.TypeRef(rowName);
            Result.Statements.Add(CodeDom.Return(CodeDom.MethodInvoke("NewRowTyped", new CodeExpression[]
                        { CodeDom._Const((System.Object)true) })));
            return Result;
        }

        public static CodeMemberMethod TableNewRow(string rowName, Boolean overriding)
        {
            CodeMemberMethod Result;

            Result = new CodeMemberMethod();
            Result.Name = "NewRowTyped";
            Result.Comments.Add(new CodeCommentStatement("create a new typed row", true));
            Result.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Overloaded;

            if (overriding)
            {
                Result.Attributes = Result.Attributes | MemberAttributes.New;
            }

            Result.ReturnType = CodeDom.TypeRef(rowName);
            Result.Parameters.Add(CodeDom.Param(typeof(bool), "AWithDefaultValues"));
            Result.Statements.Add(CodeDom.VarDecl(rowName, "ret", CodeDom.Cast(rowName, CodeDom.MethodInvoke("NewRow", new CodeExpression[] { }))));
            Result.Statements.Add(new CodeConditionStatement(CodeDom.Equals(CodeDom.Local("AWithDefaultValues"),
                        CodeDom._Const((System.Object)true)), new CodeStatement[]
                    { CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local("ret"), "InitValues",
                              new CodeExpression[] { }
                              )) }, new CodeStatement[] { }
                    ));
            Result.Statements.Add(CodeDom.Return(CodeDom.Local("ret")));
            return Result;
        }

        public static CodeMemberMethod TableNewRow(string rowName)
        {
            return TableNewRow(rowName, false);
        }

        // an attribute for a key of the table

        public static CodeMemberField TableKey(TConstraint constraint)
        {
            CodeMemberField ReturnValue;

            ReturnValue = new CodeMemberField();
            ReturnValue.Name = TTable.NiceKeyName(constraint);
            ReturnValue.Comments.Add(new CodeCommentStatement("auto generated", true));

            // nonDelphi specific:
            // Result.Type := new  CodeTypeReference(typeof(TDataColumnArray));
            ReturnValue.Attributes = MemberAttributes.Public;
            ReturnValue.Type = new CodeTypeReference("DataColumn[]");
            return ReturnValue;
        }

        /// add primary and foreign keys to the table;
        /// using the constraints from the datadefinition xml
        public static void TableAddKeys(TTable table, CodeStatementCollection InitClassStatements, CodeTypeMemberCollection TableMembers)
        {
            foreach (TConstraint myConstraint in table.grpConstraint.List)
            {
                // don't need the foreign keys as variables, the references are lost often anyway
                if (myConstraint.strType == "primarykey")
                {
                    InitClassStatements.Add(CodeDom.Let(CodeDom.PropRef(TTable.NiceKeyName(myConstraint)),
                            CodeDom.NewArray(typeof(DataColumn),
                                codeGenerationPetra
                                .FieldListToExpressionArray(table, myConstraint.strThisFields))));
                }

                // if (myConstraint.strType <> 'primarykey') then
                // TableMembers.Add (TableKey(myConstraint));
            }
        }

        public static void TableAddKeys(TDataDefinitionStore store,
            ArrayList fields,
            CodeStatementCollection InitClassStatements,
            CodeTypeMemberCollection TableMembers,
            ref ArrayList constraints,
            TConstraint primaryKey)
        {
            TTable tableOfField;
            bool allFieldExist;
            bool fieldExists;
            StringCollection constraintLocalNames;

            if (primaryKey != null)
            {
                InitClassStatements.Add(CodeDom.Let(CodeDom.PropRef("PrimaryKey"), CodeDom.NewArray(typeof(DataColumn),
                            codeGenerationPetra.
                            FieldListToExpressionArray(primaryKey.
                                strThisFields))));
            }

            foreach (TTableField field in fields)
            {
                // constraints of existing fields in Petra SQL tables
                if (field.strTableName.Length != 0)
                {
                    tableOfField = store.GetTable(field.strTableName);

                    foreach (TConstraint myConstraint in tableOfField.grpConstraint.List)
                    {
                        // try to see if the constraint is for the field.
                        if (myConstraint.strThisFields.IndexOf(field.strName) != -1)
                        {
                            // check if all other fields of the constraint are present in this (custom) table
                            allFieldExist = true;
                            constraintLocalNames = new StringCollection();

                            foreach (string keyField in myConstraint.strThisFields)
                            {
                                fieldExists = false;

                                foreach (TTableField field2 in fields)
                                {
                                    if ((field2.strName == keyField) || (field2.strNameDotNet == keyField)
                                        || (TTable.NiceFieldName(field2.strName) == keyField))
                                    {
                                        if (field2.strNameDotNet.Length > 0)
                                        {
                                            constraintLocalNames.Add(field2.strNameDotNet);
                                        }
                                        else
                                        {
                                            constraintLocalNames.Add(TTable.NiceFieldName(field2));
                                        }

                                        fieldExists = true;
                                    }
                                }

                                if (fieldExists == false)
                                {
                                    allFieldExist = false;
                                }
                            }

                            // don't add a constraint twice
                            if ((allFieldExist == true) && (!constraints.Contains(myConstraint)))
                            {
                                // don't add primary keys from additional fields
                                if (myConstraint.strType != "primarykey")
                                {
                                    InitClassStatements.Add(CodeDom.Let(CodeDom.PropRef(TTable.NiceKeyName(myConstraint)),
                                            CodeDom.NewArray(typeof(DataColumn),
                                                codeGenerationPetra.
                                                FieldListToExpressionArray(
                                                    constraintLocalNames))));

                                    if (myConstraint.strType != "primarykey")
                                    {
                                        TableMembers.Add(TableKey(myConstraint));
                                    }

                                    constraints.Add(myConstraint);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static CodeMemberMethod TableGetColumnDBName(TTableField field)
        {
            CodeMemberMethod ReturnValue;

            ReturnValue = new CodeMemberMethod();
            ReturnValue.Comments.Add(new CodeCommentStatement("get the name of the field in the database for this column", true));
            ReturnValue.Name = "Get" + TTable.NiceFieldName(field) + "DBName";
            ReturnValue.ReturnType = CodeDom.TypeRef(typeof(String));
            ReturnValue.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            ReturnValue.Statements.Add(CodeDom.Return(CodeDom._Const(field.strName)));
            return ReturnValue;
        }

        public static CodeMemberMethod TableGetColumnHelp(TTableField field)
        {
            CodeMemberMethod ReturnValue;
            string help;

            ReturnValue = new CodeMemberMethod();
            ReturnValue.Comments.Add(new CodeCommentStatement("get help text for column", true));
            ReturnValue.Name = "Get" + TTable.NiceFieldName(field) + "Help";
            ReturnValue.ReturnType = CodeDom.TypeRef(typeof(String));
            ReturnValue.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            help = field.strHelp;

            if (help.Length == 0)
            {
                help = field.strDescription;
            }

            ReturnValue.Statements.Add(CodeDom.Return(CodeDom._Const(help)));
            return ReturnValue;
        }

        public static CodeMemberMethod TableGetColumnCharLength(TTableField field)
        {
            CodeMemberMethod ReturnValue;

            ReturnValue = new CodeMemberMethod();
            ReturnValue.Comments.Add(new CodeCommentStatement("get character length for column", true));
            ReturnValue.Name = "Get" + TTable.NiceFieldName(field) + "Length";
            ReturnValue.ReturnType = CodeDom.TypeRef(typeof(System.Int16));
            ReturnValue.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            ReturnValue.Statements.Add(CodeDom.Return(CodeDom._Const((System.Object)(field.iCharLength))));
            return ReturnValue;
        }

        public static CodeMemberMethod TableGetColumnFormatLength(TTableField field)
        {
            CodeMemberMethod ReturnValue;
            Int32 length;

            ReturnValue = new CodeMemberMethod();
            ReturnValue.Comments.Add(new CodeCommentStatement("get display format for column", true));
            ReturnValue.Name = "Get" + TTable.NiceFieldName(field) + "Length";
            ReturnValue.ReturnType = CodeDom.TypeRef(typeof(System.Int16));
            ReturnValue.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            length = field.strFormat.Length;

            if ((field.strType == "date") && (field.strFormat.Length == 10))
            {
                // for ddMMMyyyy
                length = 11;
            }

            ReturnValue.Statements.Add(CodeDom.Return(CodeDom._Const((System.Object)(length))));
            return ReturnValue;
        }

        public static CodeMemberMethod TableGetColumnLabel(TTableField field)
        {
            CodeMemberMethod ReturnValue;

            ReturnValue = new CodeMemberMethod();
            ReturnValue.Comments.Add(new CodeCommentStatement("get label of column", true));
            ReturnValue.Name = "Get" + TTable.NiceFieldName(field) + "Label";
            ReturnValue.ReturnType = CodeDom.TypeRef(typeof(String));
            ReturnValue.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            ReturnValue.Statements.Add(CodeDom.Return(CodeDom._Const(field.strLabel)));
            return ReturnValue;
        }

        public static CodeStatement TableCreateOdbcParameter(TTableField tableField)
        {
            CodeExpression[] parameters;

            if (tableField.iLength != -1)
            {
                parameters =
                    new CodeExpression[] {
                    CodeDom._Const(""), codeGenerationPetra.ToOdbcType(tableField),
                    CodeDom._Const((System.Object)(tableField.iLength))
                };
            }
            else
            {
                parameters = new CodeExpression[] {
                    CodeDom._Const(""), codeGenerationPetra.ToOdbcType(tableField)
                };
            }

            return new CodeConditionStatement(CodeDom.Equals(CodeDom.Local("ACol"), CodeDom.Local("Column" + TTable.NiceFieldName(
                            tableField))), new CodeStatement[]
                { CodeDom.Return(CodeDom._New(typeof(OdbcParameter), parameters)) }, new CodeStatement[] { });
        }

        public static CodeMemberMethod StaticTableName(String name, Boolean overriding)
        {
            CodeMemberMethod ReturnValue;

            ReturnValue = new CodeMemberMethod();
            ReturnValue.Name = "GetTableName";
            ReturnValue.Comments.Add(new CodeCommentStatement("CamelCase version of the tablename", true));
            ReturnValue.ReturnType = CodeDom.TypeRef(typeof(String));
            ReturnValue.Attributes = MemberAttributes.Static | MemberAttributes.Public;

            if (overriding)
            {
                ReturnValue.Attributes = ReturnValue.Attributes | MemberAttributes.New;
            }

            ReturnValue.Statements.Add(CodeDom.Return(CodeDom._Const(name)));
            return ReturnValue;
        }

        public static CodeMemberMethod StaticTableName(String name)
        {
            return StaticTableName(name, false);
        }

        public static CodeMemberMethod StaticDBTableName(String name, Boolean overriding)
        {
            CodeMemberMethod ReturnValue;

            ReturnValue = new CodeMemberMethod();
            ReturnValue.Comments.Add(new CodeCommentStatement("original name of table in the database", true));
            ReturnValue.Name = "GetTableDBName";
            ReturnValue.ReturnType = CodeDom.TypeRef(typeof(String));
            ReturnValue.Attributes = MemberAttributes.Static | MemberAttributes.Public;

            if (overriding)
            {
                ReturnValue.Attributes = ReturnValue.Attributes | MemberAttributes.New;
            }

            ReturnValue.Statements.Add(CodeDom.Return(CodeDom._Const(name)));
            return ReturnValue;
        }

        public static CodeMemberMethod StaticDBTableName(String name)
        {
            return StaticDBTableName(name, false);
        }

        public static CodeMemberMethod StaticTableLabel(String name)
        {
            CodeMemberMethod ReturnValue;

            ReturnValue = new CodeMemberMethod();
            ReturnValue.Name = "GetTableLabel";
            ReturnValue.Comments.Add(new CodeCommentStatement("get table label for messages etc", true));
            ReturnValue.ReturnType = CodeDom.TypeRef(typeof(String));
            ReturnValue.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            ReturnValue.Statements.Add(CodeDom.Return(CodeDom._Const(name)));
            return ReturnValue;
        }

        // add a column in the initClass method of the Table

        public static void TableInitClassAddColumn(CodeStatementCollection AStatementsClass,
            CodeStatementCollection AStatementsVars,
            TTableField tableField)
        {
            /* myInitClassCode.Statements.Add(
             * Eval (CodeDom.MethodInvoke (
             * CodeDom.PropRef ('Columns'),
             * 'Add', CodeExpression[].Create(CodeDom._New (typeof (DataColumn),
             *   CodeExpression[].Create(CodeDom._Const (tableField.strName),
             *     CodetypeofExpression.Create(
             *     CodeTypeReference.Create(toDelphiType(tableField.strName, tableField.strType)))))
             * ))));
             */
            AStatementsClass.Add(CodeDom.MethodInvoke(CodeDom.PropRef("Columns"), "Add", new CodeExpression[]
                    { CodeDom._New(typeof(DataColumn), new CodeExpression[]
                          { CodeDom._Const(tableField.strName),
                            new CodeTypeOfExpression(new CodeTypeReference(
                                    codeGenerationPetra
                                    .ToDelphiType(tableField)
                                    )) }) }));
            AStatementsVars.Add(CodeDom.Let(CodeDom.FieldRef("Column" + TTable.NiceFieldName(tableField)),
                    CodeDom.IndexerRef(CodeDom.FieldRef("Columns"), new CodeExpression[]
                        { CodeDom._Const(
                              tableField
                              .strName) })));

            /* if query does not include the field, we are in trouble, because the field will be null
             * if (tableField.bNotNull) then
             * begin
             * myInitClassCode.Statements.Add( Let (
             *     CodeDom.PropRef(FieldRef('Column'+niceFieldName(tableField.strName)), 'AllowDBNull'),
             *     CodeDom._Const(false as System.Object)));
             * end;
             */
        }

        // Constructor for the typed table (with parameter name)

        public static CodeConstructor TableConstructorWithoutName(string tablename)
        {
            CodeConstructor ReturnValue;

            ReturnValue = new CodeConstructor();
            ReturnValue.Comments.Add(new CodeCommentStatement("constructor", true));
            ReturnValue.BaseConstructorArgs.Add(CodeDom._Const(tablename));
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Overloaded;
            return ReturnValue;
        }

        public static CodeConstructor TableConstructorWithName()
        {
            CodeConstructor ReturnValue;

            ReturnValue = new CodeConstructor();
            ReturnValue.Comments.Add(new CodeCommentStatement("constructor", true));
            ReturnValue.Parameters.Add(CodeDom.Param(typeof(String), "ATablename"));
            ReturnValue.BaseConstructorArgs.Add(CodeDom.Local("ATablename"));
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Overloaded;
            return ReturnValue;
        }

        // GetPrimKeyColumnOrdList

        public static CodeMemberMethod TableGetPrimKeyColumnOrdList(CodeTypeDeclaration AClass, TTable ATable)
        {
            CodeMemberMethod ReturnValue;

            CodeExpression[] elements;
            TConstraint PrimaryKey;
            TTableField field;
            Int32 Counter;
            try
            {
                PrimaryKey = ATable.GetPrimaryKey();
            }
            catch (Exception)
            {
                return null;
            }
            ReturnValue = new CodeMemberMethod();
            ReturnValue.Name = "GetPrimKeyColumnOrdList";
            ReturnValue.Comments.Add(new CodeCommentStatement("get the index number of fields that are part of the primary key", true));
            ReturnValue.ReturnType = CodeDom.TypeRef("Int32[]");
            ReturnValue.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            elements = new CodeExpression[PrimaryKey.strThisFields.Count];
            Counter = 0;

            foreach (string fieldname in PrimaryKey.strThisFields)
            {
                field = ATable.GetField(fieldname);
                elements[Counter] = CodeDom._Const((System.Object)(field.iOrder - 1));
                Counter = Counter + 1;
            }

            ReturnValue.Statements.Add(CodeDom.Return(CodeDom.NewArray(typeof(Int32), elements)));
            AClass.Members.Add(ReturnValue);
            return ReturnValue;
        }

        // GetColumnStringList

        public static CodeMemberMethod TableGetColumnStringList(CodeTypeDeclaration AClass, TTable ATable)
        {
            CodeMemberMethod ReturnValue;

            CodeExpression[] elements;
            Int32 Counter;
            ReturnValue = new CodeMemberMethod();
            ReturnValue.Name = "GetColumnStringList";
            ReturnValue.ReturnType = CodeDom.TypeRef("String[]");
            ReturnValue.Comments.Add(new CodeCommentStatement("get the names of the columns", true));
            ReturnValue.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            elements = new CodeExpression[ATable.grpTableField.List.Count];
            Counter = 0;

            foreach (TTableField field in ATable.grpTableField.List)
            {
                elements[Counter] = CodeDom._Const(field.strName);
                Counter = Counter + 1;
            }

            ReturnValue.Statements.Add(CodeDom.Return(CodeDom.NewArray(typeof(string), elements)));
            AClass.Members.Add(ReturnValue);
            return ReturnValue;
        }

        // create the table

        public static CodeTypeDeclaration Table(TTable table, string typedTableName, string tableName, string rowName)
        {
            CodeTypeDeclaration Result;
            CodeMemberMethod myInitClassCode;
            CodeMemberMethod myInitVarsCode;
            CodeMemberMethod myGetChangesCode;
            CodeMemberMethod myCreateOdbcParameter;

            Result = new CodeTypeDeclaration(tableName);
            Result.IsClass = true;
            Result.Comments.Add(new CodeCommentStatement(HttpUtility.HtmlEncode(table.strDescription), true));
            Result.CustomAttributes.Add(new CodeAttributeDeclaration("Serializable"));
            Result.BaseTypes.Add("TTypedDataTable");
            Result.Members.Add(TableConstructorWithoutName(typedTableName));

            // CSharp seems to need to derive each constructor
            Result.Members.Add(TableConstructorWithName());

            // InitClass
            myInitClassCode = new CodeMemberMethod();
            myInitClassCode.Name = "InitClass";
            myInitClassCode.Comments.Add(new CodeCommentStatement("create the columns", true));
            myInitClassCode.Attributes = MemberAttributes.Family | MemberAttributes.Override;

            // InitVars
            myInitVarsCode = new CodeMemberMethod();
            myInitVarsCode.Name = "InitVars";
            myInitVarsCode.Comments.Add(new CodeCommentStatement("assign columns to properties, set primary key", true));
            myInitVarsCode.Attributes = MemberAttributes.Public | MemberAttributes.Override;

            // GetChanges
            myGetChangesCode = new CodeMemberMethod();
            myGetChangesCode.Name = "GetChangesTyped";
            myGetChangesCode.Comments.Add(new CodeCommentStatement("get typed set of changes", true));
            myGetChangesCode.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            myGetChangesCode.ReturnType = CodeDom.TypeRef(tableName);
            myGetChangesCode.Statements.Add(CodeDom.Return(CodeDom.Cast(tableName,
                        CodeDom.BaseMethodInvoke("GetChangesTypedInternal",
                            new CodeExpression[] { }))));

            // CreateOdbcParameter
            myCreateOdbcParameter = new CodeMemberMethod();
            myCreateOdbcParameter.Name = "CreateOdbcParameter";
            myCreateOdbcParameter.Comments.Add(new CodeCommentStatement("prepare odbc parameters for given column", true));
            myCreateOdbcParameter.ReturnType = CodeDom.TypeRef("OdbcParameter");
            myCreateOdbcParameter.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            myCreateOdbcParameter.Parameters.Add(CodeDom.Param("DataColumn", "ACol"));

            foreach (TTableField tableField in table.grpTableField.List)
            {
                Result.Members.Add(TableColumn(tableField));
                Result.Members.Add(TableGetColumnDBName(tableField));
                Result.Members.Add(TableGetColumnHelp(tableField));
                Result.Members.Add(TableGetColumnLabel(tableField));

                if (tableField.strType == "varchar")
                {
                    Result.Members.Add(TableGetColumnCharLength(tableField));
                }
                else if (tableField.strFormat.Length != 0)
                {
                    Result.Members.Add(TableGetColumnFormatLength(tableField));
                }

                TableInitClassAddColumn(myInitClassCode.Statements, myInitVarsCode.Statements, tableField);
                myCreateOdbcParameter.Statements.Add(TableCreateOdbcParameter(tableField));
            }

            myCreateOdbcParameter.Statements.Add(CodeDom.Return(CodeDom._Const(null)));
            TableAddKeys(table, myInitVarsCode.Statements, Result.Members);
            Result.Members.Add(TableConstructorSerialize());
            Result.Members.Add(StaticTableName(typedTableName));
            Result.Members.Add(StaticDBTableName(table.strName));
            Result.Members.Add(StaticTableLabel(table.strLabel));
            TableGetPrimKeyColumnOrdList(Result, table);
            TableGetColumnStringList(Result, table);
            Result.Members.Add(myInitVarsCode);
            Result.Members.Add(myGetChangesCode);
            Result.Members.Add(TableRowIndexer(rowName, false));
            Result.Members.Add(TableNewRow(rowName));
            Result.Members.Add(TableNewRowShort(rowName));
            Result.Members.Add(TableNewRowFromBuilder(rowName));
            Result.Members.Add(myInitClassCode);
            Result.Members.Add(myCreateOdbcParameter);
            return Result;
        }

        // create a table, that is derived from another table

        public static CodeTypeDeclaration DerivedTable(TDataDefinitionStore store,
            ArrayList fields,
            string tableTypedName,
            string tableName,
            string rowName,
            string superTable,
            ref ArrayList constraints)
        {
            CodeTypeDeclaration Result;
            CodeMemberMethod myInitClassCode;
            CodeMemberMethod myInitVarsCode;
            CodeMemberMethod myCreateOdbcParameter;

            Result = new CodeTypeDeclaration(tableName);
            Result.IsClass = true;
            Result.Comments.Add(new CodeCommentStatement("auto generated table derived from " + superTable, true));
            Result.CustomAttributes.Add(new CodeAttributeDeclaration("Serializable"));
            Result.BaseTypes.Add(superTable + "Table");
            Result.Members.Add(TableConstructorWithoutName(tableTypedName));

            // CSharp seems to need to derive each constructor
            Result.Members.Add(TableConstructorWithName());

            // InitClass
            myInitClassCode = new CodeMemberMethod();
            myInitClassCode.Attributes = MemberAttributes.Family | MemberAttributes.Override;
            myInitClassCode.Name = "InitClass";
            myInitClassCode.Comments.Add(new CodeCommentStatement("create the columns", true));
            myInitClassCode.Statements.Add(CodeDom.BaseMethodInvoke("InitClass", new CodeExpression[] { }));

            // InitVars
            // assigns the columns to the typed columnnames, generates the keys
            myInitVarsCode = new CodeMemberMethod();
            myInitVarsCode.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            myInitVarsCode.Name = "InitVars";
            myInitVarsCode.Comments.Add(new CodeCommentStatement("assign columns to properties, set primary key", true));
            myInitVarsCode.Statements.Add(CodeDom.BaseMethodInvoke("InitVars", new CodeExpression[] { }));

            // CreateOdbcParameter
            myCreateOdbcParameter = new CodeMemberMethod();
            myCreateOdbcParameter.Name = "CreateOdbcParameter";
            myCreateOdbcParameter.Comments.Add(new CodeCommentStatement("prepare odbc parameters for given column", true));
            myCreateOdbcParameter.ReturnType = CodeDom.TypeRef("OdbcParameter");
            myCreateOdbcParameter.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            myCreateOdbcParameter.Parameters.Add(CodeDom.Param("DataColumn", "ACol"));

            foreach (TTableField tableField in fields)
            {
                Result.Members.Add(TableColumn(tableField));
                Result.Members.Add(TableGetColumnDBName(tableField));
                Result.Members.Add(TableGetColumnHelp(tableField));
                Result.Members.Add(TableGetColumnLabel(tableField));
                TableInitClassAddColumn(myInitClassCode.Statements, myInitVarsCode.Statements, tableField);
                myCreateOdbcParameter.Statements.Add(TableCreateOdbcParameter(tableField));
            }

            myCreateOdbcParameter.Statements.Add(CodeDom.Return(CodeDom.BaseMethodInvoke("CreateOdbcParameter", new CodeExpression[]
                        { CodeDom.Local("ACol") })));
            TableAddKeys(store, fields, myInitVarsCode.Statements, Result.Members, ref constraints, null);
            Result.Members.Add(TableConstructorSerialize());
            Result.Members.Add(StaticTableName(tableTypedName, true));
            Result.Members.Add(StaticDBTableName(tableTypedName, true));
            Result.Members.Add(myInitVarsCode);
            Result.Members.Add(TableRowIndexer(rowName));
            Result.Members.Add(TableNewRow(rowName, true));
            Result.Members.Add(TableNewRowFromBuilder(rowName));
            Result.Members.Add(myInitClassCode);
            Result.Members.Add(myCreateOdbcParameter);
            return Result;
        }

        // create a custom table, consisting of custom fields and fields of sql tables

        public static CodeTypeDeclaration CustomTable(TDataDefinitionStore store,
            ArrayList fields,
            string tableTypedName,
            string tableName,
            string rowName,
            ref ArrayList constraints,
            TConstraint primaryKey)
        {
            CodeTypeDeclaration Result;
            CodeMemberMethod myInitClassCode;
            CodeMemberMethod myInitVarsCode;
            CodeMemberMethod myCreateOdbcParameter;

            Result = new CodeTypeDeclaration(tableName);
            Result.Comments.Add(new CodeCommentStatement("auto generated custom table", true));
            Result.IsClass = true;
            Result.CustomAttributes.Add(new CodeAttributeDeclaration("Serializable"));
            Result.BaseTypes.Add("TTypedDataTable");
            Result.Members.Add(TableConstructorWithoutName(tableTypedName));

            // CSharp seems to need to derive each constructor
            Result.Members.Add(TableConstructorWithName());

            // InitClass
            myInitClassCode = new CodeMemberMethod();
            myInitClassCode.Attributes = MemberAttributes.Family | MemberAttributes.Override;
            myInitClassCode.Name = "InitClass";
            myInitClassCode.Comments.Add(new CodeCommentStatement("create the columns", true));

            // InitVars
            // assigns the columns to the typed columnnames, generates the keys
            myInitVarsCode = new CodeMemberMethod();
            myInitVarsCode.Comments.Add(new CodeCommentStatement("assign columns to properties, set primary key", true));
            myInitVarsCode.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            myInitVarsCode.Name = "InitVars";

            // CreateOdbcParameter
            myCreateOdbcParameter = new CodeMemberMethod();
            myCreateOdbcParameter.Name = "CreateOdbcParameter";
            myCreateOdbcParameter.Comments.Add(new CodeCommentStatement("prepare odbc parameters for given column", true));
            myCreateOdbcParameter.ReturnType = CodeDom.TypeRef("OdbcParameter");
            myCreateOdbcParameter.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            myCreateOdbcParameter.Parameters.Add(CodeDom.Param("DataColumn", "ACol"));

            foreach (TTableField tableField in fields)
            {
                Result.Members.Add(TableColumn(tableField));
                Result.Members.Add(TableGetColumnDBName(tableField));
                Result.Members.Add(TableGetColumnHelp(tableField));
                Result.Members.Add(TableGetColumnLabel(tableField));
                TableInitClassAddColumn(myInitClassCode.Statements, myInitVarsCode.Statements, tableField);
                myCreateOdbcParameter.Statements.Add(TableCreateOdbcParameter(tableField));
            }

            myCreateOdbcParameter.Statements.Add(CodeDom.Return(CodeDom._Const(null)));
            TableAddKeys(store, fields, myInitVarsCode.Statements, Result.Members, ref constraints, primaryKey);
            Result.Members.Add(TableConstructorSerialize());
            Result.Members.Add(StaticTableName(tableTypedName));
            Result.Members.Add(StaticDBTableName(tableTypedName));
            Result.Members.Add(myInitVarsCode);
            Result.Members.Add(TableRowIndexer(rowName, false));
            Result.Members.Add(TableNewRow(rowName));
            Result.Members.Add(TableNewRowFromBuilder(rowName));
            Result.Members.Add(myInitClassCode);
            Result.Members.Add(myCreateOdbcParameter);
            return Result;
        }
    }
}