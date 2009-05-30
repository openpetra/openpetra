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
using Ict.Common;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.DataStore
{
    public class codeGenerationRow
    {
        /** Row Constructor
         */
        public static CodeConstructor RowConstructor(string tableName)
        {
            CodeConstructor Result = new CodeConstructor();

            Result.Comments.Add(new CodeCommentStatement("Constructor", true));
            Result.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            Result.BaseConstructorArgs.Add(CodeDom.Local("rb"));
            Result.Parameters.Add(CodeDom.Param(typeof(DataRowBuilder), "rb"));
            Result.Statements.Add(CodeDom.Let(CodeDom.PropRef("myTable"), CodeDom.Cast(tableName, CodeDom.PropRef("Table"))));
            return Result;
        }

        // will initialise the columns with NULL or the default value

        public static CodeMemberMethod InitRow(string tableName, ArrayList AFields, Boolean AOriginal)
        {
            CodeMemberMethod Result;
            CodeExpression ConstValue;

            Result = new CodeMemberMethod();
            Result.Comments.Add(new CodeCommentStatement("set default values", true));
            Result.Name = "InitValues";

            if (AOriginal)
            {
                Result.Attributes = MemberAttributes.Public;
            }
            else
            {
                Result.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            }

            foreach (TTableField tablefield in AFields)
            {
                ConstValue = null;

                if ((tablefield.ExistsStrInitialValue == true) && (tablefield.strDefault != "NULL") && (tablefield.strDefault != ""))
                {
                    if (tablefield.strType == "bit")
                    {
                        ConstValue = CodeDom._Const((System.Object)(tablefield.strDefault == "1"));
                    }
                    else if (tablefield.strType == "varchar")
                    {
                        ConstValue = CodeDom._Const((System.Object)(tablefield.strDefault));
                    }
                    else if (tablefield.strType == "integer")
                    {
                        ConstValue = CodeDom._Const((System.Object)(StringHelper.StrToInt(tablefield.strDefault)));
                    }
                    else if (tablefield.strType == "number")
                    {
                        // System.Console.WriteLine(tablefield.strType + ' ' + tablefield.strTableName +'.'+ tablefield.strName + ' ' +tableField.strDefault);
                        ConstValue = CodeDom._Const((System.Object)(StringHelper.TryStrToFloat(tablefield.strDefault, -1.0)));
                    }
                    else if ((tablefield.strType == "date") && (tablefield.strDefault == "SYSDATE"))
                    {
                        // System.Console.WriteLine(tablefield.strType + ' ' + tablefield.strTableName +'.'+ tablefield.strName + ' ' +tableField.strDefault);
                        ConstValue = CodeDom.PropRef(CodeDom.Local("DateTime"), "Today");
                    }
                    else
                    {
                        System.Console.WriteLine("RowConstructor Default value: forgot type " + tablefield.strType);
                    }

                    Result.Statements.Add(CodeDom.Let(CodeDom.IndexerRef(new CodeExpression[]
                                { CodeDom.PropRef(CodeDom.PropRef(CodeDom.PropRef("myTable"),
                                          "Column" +
                                          TTable.NiceFieldName(tablefield)),
                                      "Ordinal") }), ConstValue));
                }
                else
                {
                    Result.Statements.Add(CodeDom.MethodInvoke("SetNull", new CodeExpression[]
                            { CodeDom.PropRef(CodeDom.PropRef("myTable"), "Column" +
                                  TTable.NiceFieldName(tablefield)
                                  ) }));
                }
            }

            // Result.Statements.Add (
            // Eval(CodeDom.MethodInvoke('AcceptChanges', CodeExpression[].Create())));
            return Result;
        }

        // the row needs a typed reference to the table

        public static CodeMemberField RowTypedTable(string tableName)
        {
            CodeMemberField ReturnValue;

            ReturnValue = new CodeMemberField();
            ReturnValue.Name = "myTable";
            ReturnValue.Type = new CodeTypeReference(tableName);
            ReturnValue.Attributes = MemberAttributes.Private;
            return ReturnValue;
        }

        /* a method that will bind the correct Odbc Type
         * and the current value to an ODBC call */

        public static CodeMemberMethod RowODBCAddParam(TTableField tableField)
        {
            CodeMemberMethod ReturnValue;

            CodeExpression[] parameters;
            ReturnValue = new CodeMemberMethod();
            ReturnValue.Name = "AddParam" + TTable.NiceFieldName(tableField);
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            ReturnValue.ReturnType = CodeDom.TypeRef(typeof(OdbcParameter));
            ReturnValue.Parameters.Add(CodeDom.Param(typeof(OdbcParameterCollection), "odbcCmdParameters"));
            ReturnValue.Parameters.Add(CodeDom.Param(typeof(string), "paramName"));

            if (tableField.iLength != -1)
            {
                parameters =
                    new CodeExpression[] {
                    CodeDom.Local("paramName"), codeGenerationPetra.ToOdbcType(tableField),
                    CodeDom._Const((System.Object)(tableField.iLength))
                };
            }
            else
            {
                parameters = new CodeExpression[] {
                    CodeDom.Local("paramName"), codeGenerationPetra.ToOdbcType(tableField)
                };
            }

            ReturnValue.Statements.Add(CodeDom.VarDecl(typeof(OdbcParameter), "ret",
                    CodeDom.MethodInvoke(CodeDom.Local("odbcCmdParameters"), "Add", parameters)));
            ReturnValue.Statements.Add(CodeDom.Let(CodeDom.PropRef(CodeDom.Local("ret"),
                        "Value"), CodeDom.PropRef(TTable.NiceFieldName(tableField))));
            ReturnValue.Statements.Add(CodeDom.Return(CodeDom.Local("ret")));
            return ReturnValue;
        }

        // add a property for the column to the row

        public static CodeMemberProperty RowColumnProperty(TTableField tableField)
        {
            CodeMemberProperty Result;

            // property
            Result = new CodeMemberProperty();
            Result.Comments.Add(new CodeCommentStatement(HttpUtility.HtmlEncode(tableField.strDescription), true));
            Result.Name = TTable.NiceFieldName(tableField);
            Result.Type = CodeDom.TypeRef(codeGenerationPetra.ToDelphiType(tableField));
            Result.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            Result.GetStatements.Add(CodeDom.VarDecl(typeof(System.Object), "ret", null));

            /*
             * Problem: somehow the ordinal gets lost, when using the dataset.Getchanges method
             * if (self.myTable.ColumnPartnerKey.Ordinal <> -1) then
             * ret := self[self.myTable.ColumnPartnerKey.Ordinal]
             * else
             * ret := self['p_partner_key_n'];
             * hope to have solved it with overwriting dataset.clone
             */

            /*
             * Result.GetStatements.Add (CodeConditionStatement.Create (
             * Inequals(CodeDom.PropRef(CodeDom.PropRef(CodeDom.PropRef('myTable'), 'Column'+niceFieldName(tableField)), 'Ordinal'),
             * CodeDom._Const(System.Object(-1))),
             * CodeStatementArray.Create (CodeDom.Let (CodeDom.Local('ret'),
             * IndexerRef(
             *   CodeExpression[].Create(CodeDom.PropRef(CodeDom.PropRef(CodeDom.PropRef('myTable'), 'Column'+niceFieldName(tableField)), 'Ordinal'))))),
             * CodeStatementArray.Create (CodeDom.Let (CodeDom.Local('ret'),
             * IndexerRef(
             *   CodeExpression[].Create(CodeDom.PropRef(CodeDom.PropRef(CodeDom.PropRef('myTable'), 'Column'+niceFieldName(tableField)), 'ColumnName')))))));
             */
            Result.GetStatements.Add(CodeDom.Let(CodeDom.Local("ret"), CodeDom.IndexerRef(new CodeExpression[]
                        { CodeDom.PropRef(CodeDom.PropRef(CodeDom.PropRef(
                                      "myTable"),
                                  "Column" +
                                  TTable.NiceFieldName(
                                      tableField)),
                              "Ordinal") })));
            Result.GetStatements.Add(new CodeConditionStatement(CodeDom.Equals(CodeDom.Local("ret"),
                        CodeDom.PropRef(CodeDom.TypeRefExp(typeof(DBNull)),
                            "Value")), new CodeStatement[]
                    { (tableField.strType.ToLower() == "varchar" ? (CodeStatement)CodeDom.Return(CodeDom.Local("String.Empty")) :
                       CodeDom.Throw(CodeDom._New(typeof(StrongTypingException), new CodeExpression[]
                               { CodeDom._Const("Error: DB null"),
                                 CodeDom._Const(null) }))
                       ) }, new CodeStatement[]
                    { CodeDom.Return(codeGenerationPetra.ToPetraCast(tableField, CodeDom.Local("ret")
                              )) }));

            /* CodeBinaryOperatorExpression.Create(
             * Equals (
             * Local ('ret'),
             * CodeDom._Const (nil)),
             * CodeBinaryOperatorType.BooleanOr, */
            Result.SetStatements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(CodeDom.MethodInvoke("IsNull",
                            new CodeExpression[]
                            { CodeDom.PropRef(CodeDom.
                                  PropRef(
                                      "myTable"),
                                  "Column" +
                                  TTable.
                                  NiceFieldName(
                                      tableField)
                                  ) }),
                        CodeBinaryOperatorType.BooleanOr,
                        CodeDom.Inequals(
                            codeGenerationPetra.ToPetraCast(
                                tableField,
                                CodeDom.IndexerRef(new
                                    CodeExpression
                                    []
                                    {
                                        CodeDom
                                        .
                                        PropRef(
                                            CodeDom
                                            .
                                            PropRef("myTable"), "Column" + TTable.NiceFieldName(tableField)
                                            )
                                    })),
                            new
                            CodePropertySetValueReferenceExpression())),
                    new CodeStatement[]
                    { CodeDom.Let(CodeDom.IndexerRef(new CodeExpression[]
                              { CodeDom.PropRef(CodeDom.PropRef("myTable"),
                                    "Column" +
                                    TTable.NiceFieldName(tableField
                                        )) }),
                          new CodePropertySetValueReferenceExpression()
                          ) }));
            return Result;
        }

        // add high null property for the date column to the row

        public static CodeMemberProperty RowColumnDateHighNullProperty(TTableField tableField)
        {
            CodeMemberProperty ReturnValue;

            // property
            ReturnValue = new CodeMemberProperty();
            ReturnValue.Name = TTable.NiceFieldName(tableField) + "HighNull";
            ReturnValue.Comments.Add(new CodeCommentStatement("Returns the date value or the maximum date if the date is NULL", true));
            ReturnValue.Type = CodeDom.TypeRef(codeGenerationPetra.ToDelphiType(tableField));
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            ReturnValue.GetStatements.Add(CodeDom.Return(CodeDom.MethodInvoke(null, "TSaveConvert.ObjectToDate", new CodeExpression[]
                        { CodeDom.IndexerRef(new CodeExpression[]
                              { CodeDom.PropRef(CodeDom.PropRef(CodeDom.
                                        PropRef(
                                            "myTable"),
                                        "Column" +
                                        TTable.
                                        NiceFieldName(
                                            tableField)), "Ordinal"
                                    ) }),
                          CodeDom.PropRef(CodeDom.Local("TNullHandlingEnum"),
                              "nhReturnHighestDate"
                              ) })));
            return ReturnValue;
        }

        /// add low null property for the date column to the row
        public static CodeMemberProperty RowColumnDateLowNullProperty(TTableField tableField)
        {
            CodeMemberProperty ReturnValue;

            // property
            ReturnValue = new CodeMemberProperty();
            ReturnValue.Name = TTable.NiceFieldName(tableField) + "LowNull";
            ReturnValue.Comments.Add(new CodeCommentStatement("Returns the date value or the minimum date if the date is NULL", true));
            ReturnValue.Type = CodeDom.TypeRef(codeGenerationPetra.ToDelphiType(tableField));
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            ReturnValue.GetStatements.Add(CodeDom.Return(CodeDom.MethodInvoke(null, "TSaveConvert.ObjectToDate", new CodeExpression[]
                        { CodeDom.IndexerRef(new CodeExpression[]
                              { CodeDom.PropRef(CodeDom.PropRef("myTable"),
                                    "Column" +
                                    TTable.NiceFieldName(
                                        tableField
                                        )) }),
                          CodeDom.PropRef(CodeDom.Local("TNullHandlingEnum"),
                              "nhReturnLowestDate"
                              ) })));
            return ReturnValue;
        }

        // Row method to test if the column has a null value

        public static CodeMemberMethod RowTestNull(string niceFieldName)
        {
            CodeMemberMethod ReturnValue;

            ReturnValue = new CodeMemberMethod();
            ReturnValue.Comments.Add(new CodeCommentStatement("test for NULL value", true));
            ReturnValue.Name = "Is" + niceFieldName + "Null";
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            ReturnValue.ReturnType = CodeDom.TypeRef(typeof(Boolean));
            ReturnValue.Statements.Add(CodeDom.Return(CodeDom.MethodInvoke("IsNull", new CodeExpression[]
                        { CodeDom.PropRef(CodeDom.PropRef("myTable"), "Column" + niceFieldName) })));

            /* Result.Statements.Add (CodeConditionStatement.Create (
             * Inequals(CodeDom.PropRef(CodeDom.PropRef(CodeDom.PropRef('myTable'), 'Column'+niceFieldName), 'Ordinal'),
             * CodeDom._Const(System.Object(-1))),
             * CodeStatementArray.Create (CodeDom.Return(CodeDom.MethodInvoke('IsNull',
             *   CodeExpression[].Create(CodeDom.PropRef(CodeDom.PropRef(CodeDom.PropRef('myTable'),
             *     'Column'+niceFieldName), 'Ordinal'))))),
             * CodeStatementArray.Create (CodeDom.Return(CodeDom.MethodInvoke('IsNull',
             *   CodeExpression[].Create(CodeDom.PropRef(CodeDom.PropRef(CodeDom.PropRef('myTable'),
             *     'Column'+niceFieldName), 'ColumnName')))))));
             */
            return ReturnValue;
        }

        // Row method to set the column to NULL

        public static CodeMemberMethod RowSetIsNull(string niceFieldName)
        {
            CodeMemberMethod ReturnValue;

            // method to set a field to null
            ReturnValue = new CodeMemberMethod();
            ReturnValue.Comments.Add(new CodeCommentStatement("assign NULL value", true));
            ReturnValue.Name = "Set" + niceFieldName + "Null";
            ReturnValue.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            ReturnValue.Statements.Add(CodeDom.MethodInvoke("SetNull", new CodeExpression[]
                    { CodeDom.PropRef(CodeDom.PropRef("myTable"), "Column" + niceFieldName) }));
            return ReturnValue;
        }

        // create the row

        public static CodeTypeDeclaration Row(TTable ATable, string tableName, string rowName)
        {
            CodeTypeDeclaration Result;

            Result = new CodeTypeDeclaration(rowName);
            Result.Comments.Add(new CodeCommentStatement(HttpUtility.HtmlEncode(ATable.strDescription), true));
            Result.IsClass = true;
            Result.CustomAttributes.Add(new CodeAttributeDeclaration("Serializable"));
            Result.BaseTypes.Add(typeof(DataRow));
            Result.Members.Add(RowConstructor(tableName));
            Result.Members.Add(InitRow(tableName, ATable.grpTableField.List, true));
            Result.Members.Add(RowTypedTable(tableName));

            /*
             * dropped, now using TypedTable.CreateODBCParam
             * for tableField in grpFields.list do
             * begin
             * Result.Members.Add (RowODBCAddParam(tableField));
             * end;
             */
            foreach (TTableField tableField in ATable.grpTableField.List)
            {
                Result.Members.Add(RowColumnProperty(tableField));

                if (!tableField.bNotNull)
                {
                    Result.Members.Add(RowTestNull(TTable.NiceFieldName(tableField)));
                    Result.Members.Add(RowSetIsNull(TTable.NiceFieldName(tableField)));
                }

                if (codeGenerationPetra.ToDelphiType(tableField) == "System.DateTime")
                {
                    Result.Members.Add(RowColumnDateLowNullProperty(tableField));
                    Result.Members.Add(RowColumnDateHighNullProperty(tableField));
                }
            }

            return Result;
        }

        public static CodeTypeDeclaration DerivedRow(ArrayList fields, string tableName, string rowName, string superTable)
        {
            CodeTypeDeclaration Result;

            Result = new CodeTypeDeclaration(rowName);
            Result.IsClass = true;
            Result.Comments.Add(new CodeCommentStatement("DerivedRow from " + superTable + "Row", true));
            Result.CustomAttributes.Add(new CodeAttributeDeclaration("Serializable"));
            Result.BaseTypes.Add(superTable + "Row");
            Result.Members.Add(RowConstructor(tableName));
            Result.Members.Add(InitRow(tableName, fields, false));
            Result.Members.Add(RowTypedTable(tableName));

            /*
             * dropped, now using TypedTable.CreateODBCParam
             * for tableField in fields do
             * begin
             * Result.Members.Add (RowODBCAddParam(tableField));
             * end;
             */
            foreach (TTableField tableField in fields)
            {
                Result.Members.Add(RowColumnProperty(tableField));

                if (!tableField.bNotNull)
                {
                    Result.Members.Add(RowTestNull(TTable.NiceFieldName(tableField)));
                    Result.Members.Add(RowSetIsNull(TTable.NiceFieldName(tableField)));
                }
            }

            return Result;
        }

        public static CodeTypeDeclaration CustomRow(ArrayList fields, string tableName, string rowName)
        {
            CodeTypeDeclaration Result;

            Result = new CodeTypeDeclaration(rowName);
            Result.Comments.Add(new CodeCommentStatement("CustomRow auto generated", true));
            Result.IsClass = true;
            Result.CustomAttributes.Add(new CodeAttributeDeclaration("Serializable"));
            Result.BaseTypes.Add(typeof(DataRow));
            Result.Members.Add(RowConstructor(tableName));
            Result.Members.Add(InitRow(tableName, fields, true));
            Result.Members.Add(RowTypedTable(tableName));

            /*
             * dropped, now using TypedTable.CreateODBCParam
             * for tableField in fields do
             * begin
             * Result.Members.Add (RowODBCAddParam(tableField));
             * end;
             */
            foreach (TTableField tableField in fields)
            {
                Result.Members.Add(RowColumnProperty(tableField));

                if (!tableField.bNotNull)
                {
                    Result.Members.Add(RowTestNull(TTable.NiceFieldName(tableField)));
                    Result.Members.Add(RowSetIsNull(TTable.NiceFieldName(tableField)));
                }
            }

            return Result;
        }
    }
}