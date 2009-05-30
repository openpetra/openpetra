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
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Collections;
using System.Collections.Specialized;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;
using Ict.Common;

namespace Ict.Tools.CodeGeneration.DataStore
{
    /// This will generate the cascading parts of the datastore;
    /// it references right across all table groups, therefore a single file is created.
    public class codeGenerationCascading
    {
        /// to avoid huge cascading deletes, which we will probably not allow anyways (e.g. s_user)
        public const Int32 CASCADING_DELETE_MAX_REFERENCES = 10;

        public static CodeMemberMethod CreateDeleteByPrimaryKeyCascading(CodeNamespace ACns,
            TDataDefinitionStore AStore,
            TTable ATable,
            string ATypedTableName)
        {
            CodeMemberMethod myCode;
            String MyTable;
            CodeExpression TheRow;

            CodeStatement[] loopContentTable;
            ArrayList StatementList;
            StringCollection LocalVariables;
            String LoadViaProcedureName;
            String DifferentField;
            myCode = new CodeMemberMethod();
            myCode.Name = "DeleteByPrimaryKey";
            myCode.Comments.Add(new CodeCommentStatement("cascading delete", true));
            myCode.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            codeGenerationAccess.AddPrimaryKeyParameters(myCode, ATable);
            myCode.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            myCode.Parameters.Add(CodeDom.Param("System.Boolean", "AWithCascDelete"));
            StatementList = new ArrayList();
            LocalVariables = new StringCollection();

            foreach (TConstraint constraint in ATable.FReferenced)
            {
                if (AStore.GetTable(constraint.strThisTable).HasPrimaryKey())
                {
                    codeGenerationAccess.AddDifferentNamespace(ACns, AStore.GetTable(constraint.strThisTable).strGroup, ATable.strGroup);
                    MyTable = "My" + TTable.NiceTableName(constraint.strThisTable) + "Table";

                    if (!LocalVariables.Contains(MyTable))
                    {
                        StatementList.Add(CodeDom.VarDecl(TTable.NiceTableName(constraint.strThisTable) + "Table", MyTable,
                                CodeDom._Const(null)));
                        LocalVariables.Add(MyTable);
                    }

                    // check if other foreign key exists that references the same table, e.g.
                    // PBankAccess.LoadViaPPartnerPartnerKey
                    // PBankAccess.LoadViaPPartnerContactPartnerKey
                    DifferentField = codeGenerationAccess.FindOtherConstraintSameOtherTable(AStore.GetTable(
                            constraint.strThisTable).
                        grpConstraint.List, constraint);
                    LoadViaProcedureName = ATypedTableName;

                    if (DifferentField.Length != 0)
                    {
                        LoadViaProcedureName = ATypedTableName + TTable.NiceFieldName(DifferentField);
                    }

                    // get all rows of the table that reference the row that should be deleted
                    StatementList.Add(CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local(TTable.NiceTableName(constraint.strThisTable) +
                                    "Access"), "LoadVia" + LoadViaProcedureName,
                                codeGenerationAccess.GetActualParameters(new
                                    CodeDirectionExpression(
                                        FieldDirection.
                                        Out,
                                        CodeDom
                                        .
                                        Local(MyTable)),
                                    CodeDom.
                                    GlobalMethodInvoke(
                                        "StringHelper.StrSplit", new CodeExpression[]
                                        {
                                            CodeDom
                                            .
                                            _Const(
                                                StringHelper
                                                .
                                                StrMerge(AStore.GetTable(constraint.strThisTable).GetPrimaryKey().
                                                    strThisFields,
                                                    ",")),
                                            CodeDom
                                            .
                                            _Const((
                                                    String)(",")
                                                )
                                        }),
                                    CodeDom.Local(
                                        "ATransaction"),
                                    ATable
                                    ))));
                    TheRow = CodeDom.IndexerRef(CodeDom.Local(MyTable), new CodeExpression[] { CodeDom.Local("countRow") });

                    if (AStore.GetTable(constraint.strThisTable).FReferenced.Count < CASCADING_DELETE_MAX_REFERENCES)
                    {
                        loopContentTable = new CodeStatement[]
                        {
                            CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local(TTable.NiceTableName(constraint.strThisTable) + "Cascading"),
                                    "DeleteUsingTemplate", new CodeExpression[]
                                    { TheRow, CodeDom._Const(null), CodeDom.Local("ATransaction"),
                                      CodeDom.Local("AWithCascDelete"
                                          ) }))
                        };
                    }
                    else
                    {
                        loopContentTable = new CodeStatement[]
                        {
                            CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local(TTable.NiceTableName(constraint.strThisTable) + "Access"),
                                    "DeleteUsingTemplate", new CodeExpression[]
                                    { TheRow, CodeDom._Const(null), CodeDom.Local("ATransaction") }))
                        };
                    }

                    // Result.Statements.Add(CodeSnippetStatement.Create('for theRow in ATable do'));
                    StatementList.Add(new CodeIterationStatement(CodeDom.Let(CodeDom.Local("countRow"),
                                CodeDom._Const((System.Object) 0)),
                            CodeDom.Inequals(CodeDom.Local("countRow"),
                                CodeDom.PropRef(CodeDom.PropRef(CodeDom.Local(MyTable),
                                        "Rows"),
                                    "Count")),
                            CodeDom.Let(CodeDom.Local("countRow"),
                                new CodeBinaryOperatorExpression(CodeDom.Local("countRow"),
                                    CodeBinaryOperatorType.Add,
                                    CodeDom._Const((System.
                                                    Object)(1)))),
                            loopContentTable));

                    // countRow := 0
                    // countRow <> Table.Rows.Count
                    // countRow := countRow + 1
                }
            }

            if (StatementList.Count > 0)
            {
                myCode.Statements.Add(CodeDom.VarDecl(typeof(System.Int32), "countRow", null));
                myCode.Statements.Add(new CodeConditionStatement(
                        CodeDom.Equals(CodeDom.Local("AWithCascDelete"),
                            CodeDom._Const((System.Object)true)),
                        CodeDom.MakeCodeStatementArray(StatementList), new CodeStatement[] { }));
            }

            myCode.Statements.Add(CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local(ATypedTableName + "Access"), "DeleteByPrimaryKey",
                        codeGenerationAccess.GetActualParameters(null, null,
                            CodeDom.Local("ATransaction"),
                            ATable))));
            return myCode;
        }

        public static CodeMemberMethod CreateDeleteUsingTemplateCascading(TDataDefinitionStore AStore, TTable ATable, string ATypedTableName)
        {
            CodeMemberMethod myCode;
            String MyTable;
            CodeExpression TheRow;

            CodeStatement[] loopContentTable;
            ArrayList StatementList;
            ArrayList DeleteAllReferencingThisPrimaryKey;
            StringCollection LocalVariables;
            String LoadViaProcedureName;
            String DifferentField;
            myCode = new CodeMemberMethod();
            myCode.Name = "DeleteUsingTemplate";
            myCode.Comments.Add(new CodeCommentStatement("cascading delete", true));
            myCode.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            myCode.Parameters.Add(CodeDom.Param(ATypedTableName + "Row", "ATemplateRow"));
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "ATemplateOperators"));
            myCode.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            myCode.Parameters.Add(CodeDom.Param("System.Boolean", "AWithCascDelete"));
            StatementList = new ArrayList();

            /*
             * // get the rows (primary key values only) that would be deleted
             * // loadusingtemplate
             * StatementList.Add (
             * VarDecl(ATypedTableName+'Table', 'MyTable',
             * CodeDom._Const(nil)));
             * StatementList.Add (
             * Eval(CodeDom.MethodInvoke(CodeDom.Local(ATypedTableName+'Access'),
             * 'LoadUsingTemplate',
             * CodeExpression[].Create(CodeDom.Local('MyTable'), CodeDom.Local('ATemplateRow'),
             *   CodeDom.GlobalMethodInvoke('StringHelper.StrSplit',
             *       CodeExpression[].Create(
             *           CodeDom._Const(strmerge(ATable.GetPrimaryKey().strThisFields, ',')),
             *           CodeDom._Const(','))),
             *   CodeDom.Local('ATransaction')))));
             * myCode.Statements.Add( VarDecl(typeof(System.Int32), 'countTemplateRow', nil));
             */
            LocalVariables = new StringCollection();

            // for each row, delete the rows in the depending tables
            foreach (TConstraint constraint in ATable.FReferenced)
            {
                if (AStore.GetTable(constraint.strThisTable).HasPrimaryKey())
                {
                    MyTable = "My" + TTable.NiceTableName(constraint.strThisTable) + "Table";

                    if (!LocalVariables.Contains(MyTable))
                    {
                        StatementList.Add(CodeDom.VarDecl(TTable.NiceTableName(constraint.strThisTable) + "Table", MyTable,
                                CodeDom._Const(null)));
                        LocalVariables.Add(MyTable);
                    }

                    DeleteAllReferencingThisPrimaryKey = new ArrayList();

                    // check if other foreign key exists that references the same table, e.g.
                    // PBankAccess.LoadViaPPartnerPartnerKey
                    // PBankAccess.LoadViaPPartnerContactPartnerKey
                    DifferentField = codeGenerationAccess.FindOtherConstraintSameOtherTable(AStore.GetTable(
                            constraint.strThisTable).
                        grpConstraint.List, constraint);
                    LoadViaProcedureName = ATypedTableName;

                    if (DifferentField.Length != 0)
                    {
                        LoadViaProcedureName = ATypedTableName + TTable.NiceFieldName(DifferentField);
                    }

                    // get all rows of the table that reference the row that should be deleted
                    // DeleteAllReferencingThisPrimaryKey
                    StatementList.Add(CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local(TTable.NiceTableName(constraint.strThisTable) +
                                    "Access"), "LoadVia" + LoadViaProcedureName +
                                "Template", new CodeExpression[]
                                { new CodeDirectionExpression(FieldDirection.Out,
                                      CodeDom.Local(MyTable)),
                                  CodeDom.Local("ATemplateRow"),
                                  CodeDom.GlobalMethodInvoke("StringHelper.StrSplit",
                                      new CodeExpression[]
                                      {
                                          CodeDom._Const(StringHelper.
                                              StrMerge(
                                                  AStore
                                                  .
                                                  GetTable(
                                                      constraint.strThisTable).GetPrimaryKey().
                                                  strThisFields,
                                                  ",")),
                                          CodeDom._Const((String)(",")
                                              )
                                      }),
                                  CodeDom.Local("ATransaction"
                                      ) })));

                    // IndexerRef(CodeDom.PropRef(CodeDom.Local('MyTable'), 'Row'), CodeExpression[].Create(CodeDom.Local('countRow'))),
                    TheRow = CodeDom.IndexerRef(CodeDom.Local(MyTable), new CodeExpression[] { CodeDom.Local("countRow") });

                    if (AStore.GetTable(constraint.strThisTable).FReferenced.Count < CASCADING_DELETE_MAX_REFERENCES)
                    {
                        loopContentTable =
                            new CodeStatement[] {
                            CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local(TTable.NiceTableName(constraint.
                                            strThisTable) +
                                        "Cascading"),
                                    "DeleteUsingTemplate",
                                    new CodeExpression[] { TheRow,
                                                           CodeDom._Const(null),
                                                           CodeDom.Local(
                                                               "ATransaction"),
                                                           CodeDom.Local(
                                                               "AWithCascDelete") }))
                        };
                    }
                    else
                    {
                        loopContentTable =
                            new CodeStatement[] {
                            CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local(TTable.NiceTableName(constraint.
                                            strThisTable) +
                                        "Access"),
                                    "DeleteUsingTemplate",
                                    new CodeExpression[] { TheRow,
                                                           CodeDom._Const(null),
                                                           CodeDom.Local(
                                                               "ATransaction") }))
                        };
                    }

                    // Result.Statements.Add(CodeSnippetStatement.Create('for theRow in ATable do'));
                    // DeleteAllReferencingThisPrimaryKey
                    StatementList.Add(new CodeIterationStatement(CodeDom.Let(CodeDom.Local("countRow"),
                                CodeDom._Const((System.Object) 0)),
                            CodeDom.Inequals(CodeDom.Local("countRow"),
                                CodeDom.PropRef(CodeDom.PropRef(CodeDom.Local(MyTable),
                                        "Rows"),
                                    "Count")),
                            CodeDom.Let(CodeDom.Local("countRow"),
                                new CodeBinaryOperatorExpression(CodeDom.Local("countRow"),
                                    CodeBinaryOperatorType.Add,
                                    CodeDom._Const((System.
                                                    Object)(1)))),
                            loopContentTable));

                    // countRow := 0
                    // countRow <> Table.Rows.Count
                    // countRow := countRow + 1

                    /* StatementList.Add(CodeIterationStatement.Create(
                     * Let(CodeDom.Local('countTemplateRow'), CodeDom._Const((System.Object)0)), // countTemplateRow := 0
                     * Inequals(CodeDom.Local('countTemplateRow'), CodeDom.PropRef(CodeDom.PropRef(CodeDom.Local('MyTable'), 'Rows'), 'Count')), // countRow <> Table.Rows.Count
                     *   Let(CodeDom.Local('countTemplateRow'),  // countRow := countRow + 1
                     *     CodeBinaryOperatorExpression.Create(CodeDom.Local('countTemplateRow'),
                     *     CodeBinaryOperatorType.Add,
                     *     CodeDom._Const(System.Object(1)))),
                     * MakeCodeStatementArray(DeleteAllReferencingThisPrimaryKey)
                     * ));
                     */
                }
            }

            if (StatementList.Count > 0)
            {
                myCode.Statements.Add(CodeDom.VarDecl(typeof(System.Int32), "countRow", null));
                myCode.Statements.Add(new CodeConditionStatement(CodeDom.Equals(CodeDom.Local("AWithCascDelete"),
                            CodeDom._Const((System.Object)true)),
                        CodeDom.MakeCodeStatementArray(StatementList), new CodeStatement[] { }));
            }

            // call normal deleteusingtemplate
            myCode.Statements.Add(CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local(ATypedTableName + "Access"), "DeleteUsingTemplate",
                        new CodeExpression[]
                        { CodeDom.Local("ATemplateRow"), CodeDom.Local("ATemplateOperators"),
                          CodeDom.Local("ATransaction") })));
            return myCode;
        }

        public static void AddDeleteTable(CodeNamespace ACns,
            TDataDefinitionStore AStore,
            TTable ATable,
            string ATypedTableName,
            string ATableName,
            ref CodeTypeDeclaration ATableClass)
        {
            if (ATable.HasPrimaryKey())
            {
                // for the moment, don't implement it for too big tables, e.g. s_user)
                if (ATable.FReferenced.Count < CASCADING_DELETE_MAX_REFERENCES)
                {
                    ATableClass.Members.Add(CreateDeleteByPrimaryKeyCascading(ACns, AStore, ATable, ATypedTableName));
                }

                // for the moment, don't implement it for too big tables, e.g. s_user)
                if (ATable.FReferenced.Count < CASCADING_DELETE_MAX_REFERENCES)
                {
                    ATableClass.Members.Add(CreateDeleteUsingTemplateCascading(AStore, ATable, ATypedTableName));
                }
            }
        }

        // create the class for accessing the database (datastore)

        public static CodeTypeDeclaration Access(CodeNamespace ACns,
            TDataDefinitionStore AStore,
            TTable table,
            string typedTableName,
            string tableName)
        {
            CodeTypeDeclaration Result;

            Result = new CodeTypeDeclaration(typedTableName + "Cascading");
            Result.IsClass = true;
            Result.Comments.Add(new CodeCommentStatement("auto generated", true));
            Result.BaseTypes.Add("TTypedDataAccess");
            AddDeleteTable(ACns, AStore, table, typedTableName, tableName, ref Result);
            return Result;
        }

        public static Boolean WriteTypedDataCascading(TDataDefinitionStore store, string AFilePath, string ANamespaceName, string AFileName)
        {
            FileStream outFile;
            String OutFileName;
            TextWriter tw;
            string tableName;
            CodeNamespace cns;

            Console.WriteLine("writing namespace " + ANamespaceName);
            OutFileName = AFilePath + AFileName + ".cs";
            outFile = new FileStream(OutFileName + ".new", FileMode.Create, FileAccess.Write);

            if (outFile == null)
            {
                return false;
            }

            CSharpCodeProvider gen = new CSharpCodeProvider();
            cns = new CodeNamespace(ANamespaceName);
            tw = new StreamWriter(outFile);
            tw.WriteLine("/* Auto generated with nant generateORM");
            tw.WriteLine(" * Do not modify this file manually!");
            tw.WriteLine(" */");
            cns.Imports.Add(new CodeNamespaceImport("System"));
            cns.Imports.Add(new CodeNamespaceImport("System.Collections.Specialized"));
            cns.Imports.Add(new CodeNamespaceImport("System.Data"));
            cns.Imports.Add(new CodeNamespaceImport("System.Data.Odbc"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Common"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Common.DB"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Common.Verification"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Common.Data"));

            // cns.Imports.Add (CodeNamespaceImport.Create ('Ict.Petra.Shared.UserInfo'));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MPartner.Partner.Data"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MPartner.Partner.Data.Access"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MPartner.Mailroom.Data"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MPartner.Mailroom.Data.Access"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MPersonnel.Personnel.Data"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MPersonnel.Personnel.Data.Access"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MPersonnel.Units.Data"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MPersonnel.Units.Data.Access"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MConference.Data"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MConference.Data.Access"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MFinance.Account.Data"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MFinance.Account.Data.Access"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MFinance.Gift.Data"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MFinance.Gift.Data.Access"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MFinance.AP.Data"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MFinance.AP.Data.Access"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MFinance.AR.Data"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MFinance.AR.Data.Access"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MSysMan.Data"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MSysMan.Data.Access"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MCommon.Data"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MCommon.Data.Access"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MHospitality.Data"));
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared.MHospitality.Data.Access"));

            foreach (TTable currentTable in store.GetTables())
            {
                tableName = TTable.NiceTableName(currentTable.strName) + "Table";
                cns.Types.Add(Access(cns, store, currentTable, TTable.NiceTableName(currentTable.strName), tableName));
            }

            CodeGeneratorOptions opt = new CodeGeneratorOptions();
            opt.BracingStyle = "C";
            gen.GenerateCodeFromNamespace(cns, tw, opt);
            tw.Close();

            if (TTextFile.UpdateFile(OutFileName) == true)
            {
                System.Console.WriteLine("   Writing file " + OutFileName);
            }

            return true;
        }
    }
}