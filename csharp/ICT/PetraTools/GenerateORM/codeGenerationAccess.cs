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
using System.Data.Odbc;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Collections;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;
using Ict.Common;
using System.Collections.Specialized;

namespace Ict.Tools.CodeGeneration.DataStore
{
    /// This will generate most of the datastore.
    /// Only here should SQL queries happen.
    public class codeGenerationAccess
    {
        public static void AddPrimaryKeyParameters(CodeMemberMethod m, TTable table)
        {
            TTableField tablefield;
            TConstraint constr;

            try
            {
                constr = table.GetPrimaryKey();
            }
            catch (Exception)
            {
                // Console.WriteLine(e.Message);
                return;
            }

            foreach (string fieldname in constr.strThisFields)
            {
                tablefield = table.GetField(fieldname);
                m.Parameters.Add(CodeDom.Param(codeGenerationPetra.ToDelphiType(tablefield), 'A' + TTable.NiceFieldName(tablefield)));
            }
        }

        public static System.Int32 MakeFormalParameters(CodeMemberMethod AMethod,
            out TConstraint AConstr,
            TTable ATableToLoad,
            TTable ATableForPrimaryKey,
            String AParamDataSetOrDataTableType,
            String AParamTemplateDataRowType,
            bool AWithFieldList,
            bool AWithTransaction,
            bool AWithOrderBy,
            bool AWithLimitRecords,
            bool AWithTemplateOperators)
        {
            Int32 NumberOfParameters;

            NumberOfParameters = 0;
            AConstr = null;

            if (AParamDataSetOrDataTableType.Length != 0)
            {
                if (AParamDataSetOrDataTableType == "DataTable")
                {
                    AMethod.Parameters.Add(CodeDom.Param(TTable.NiceTableName(ATableToLoad.strName) + "Table", "AData",
                            FieldDirection.Out));
                }
                else
                {
                    AMethod.Parameters.Add(CodeDom.Param(AParamDataSetOrDataTableType, "AData"));
                }

                NumberOfParameters = NumberOfParameters + 1;
            }

            if (ATableForPrimaryKey != null)
            {
                try
                {
                    AConstr = ATableForPrimaryKey.GetPrimaryKey();
                }
                catch (Exception)
                {
                    // Console.WriteLine(e.Message);
                    AConstr = new TConstraint();
                }
                AddPrimaryKeyParameters(AMethod, ATableForPrimaryKey);
                NumberOfParameters = NumberOfParameters + AConstr.strThisFields.Count;
            }
            else if (AParamTemplateDataRowType != null)
            {
                AMethod.Parameters.Add(CodeDom.Param(AParamTemplateDataRowType, "ATemplateRow"));
                NumberOfParameters = NumberOfParameters + 1;

                if (AWithTemplateOperators == true)
                {
                    AMethod.Parameters.Add(CodeDom.Param("StringCollection", "ATemplateOperators"));
                }

                NumberOfParameters = NumberOfParameters + 1;
            }

            if (AWithFieldList == true)
            {
                AMethod.Parameters.Add(CodeDom.Param("StringCollection", "AFieldList"));
            }

            if (AWithTransaction == true)
            {
                AMethod.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            }

            if (AWithOrderBy == true)
            {
                AMethod.Parameters.Add(CodeDom.Param("StringCollection", "AOrderBy"));
            }

            NumberOfParameters = NumberOfParameters + 3;     /// FieldList, Transaction, Orderby

            if (AWithLimitRecords == true)
            {
                AMethod.Parameters.Add(CodeDom.Param("System.Int32", "AStartRecord"));
                AMethod.Parameters.Add(CodeDom.Param("System.Int32", "AMaxRecords"));
            }

            NumberOfParameters = NumberOfParameters + 2;     /// AStartRecord, AMaxRecords
            return NumberOfParameters;
        }

        public static void InsertActualParametersPrimaryKey(ref System.Int32 Counter,
            ref CodeExpression[] AActualParameters,
            TTable ATableForPrimaryKey,
            TConstraint AConstr)
        {
            foreach (String fieldname in AConstr.strThisFields)
            {
                TTableField tablefield = ATableForPrimaryKey.GetField(fieldname);
                AActualParameters[Counter] = CodeDom.Local('A' + TTable.NiceFieldName(tablefield));
                Counter = Counter + 1;
            }
        }

        public static CodeMemberMethod CreateOverLoad(String AProcedureName,
            TTable ATableToLoad,
            TTable ATableForPrimaryKey,
            String AParamDataSetOrDataTableType,
            String AParamTemplateDataRowType,
            bool AWithFieldList,
            bool AWithTransaction,
            bool AWithOrderBy,
            bool AWithTemplateOperators)
        {
            CodeMemberMethod ReturnValue;
            TConstraint constr;
            Int32 NumberOfParameters;

            CodeExpression[] ActualParameters;
            Int32 Counter;
            ReturnValue = new CodeMemberMethod();
            ReturnValue.Comments.Add(new CodeCommentStatement("auto generated", true));
            ReturnValue.Name = AProcedureName;
            ReturnValue.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            NumberOfParameters = MakeFormalParameters(ReturnValue,
                out constr,
                ATableToLoad,
                ATableForPrimaryKey,
                AParamDataSetOrDataTableType,
                AParamTemplateDataRowType,
                AWithFieldList,
                AWithTransaction,
                AWithOrderBy,
                false,
                AWithTemplateOperators);

            // prepare the call statement of the long version of the procedure
            ActualParameters = new CodeExpression[NumberOfParameters];
            Counter = 0;

            if (AParamDataSetOrDataTableType.Length != 0)
            {
                if (AParamDataSetOrDataTableType == "DataSet")
                {
                    ActualParameters[Counter] = CodeDom.Local("AData");
                }
                else
                {
                    ActualParameters[Counter] = new CodeDirectionExpression(FieldDirection.Out, CodeDom.Local("AData"));
                }

                Counter = Counter + 1;
            }

            if (ATableForPrimaryKey != null)
            {
                InsertActualParametersPrimaryKey(ref Counter, ref ActualParameters, ATableForPrimaryKey, constr);
            }
            else if (AParamTemplateDataRowType != null)
            {
                ActualParameters[Counter] = CodeDom.Local("ATemplateRow");
                Counter = Counter + 1;

                if (AWithTemplateOperators == true)
                {
                    ActualParameters[Counter] = CodeDom.Local("ATemplateOperators");
                }
                else
                {
                    ActualParameters[Counter] = CodeDom._Const(null);
                }

                Counter = Counter + 1;
            }

            if (AWithFieldList == true)
            {
                ActualParameters[Counter] = CodeDom.Local("AFieldList");
            }
            else
            {
                ActualParameters[Counter] = CodeDom._Const(null);
            }

            Counter = Counter + 1;

            if (AWithTransaction == true)
            {
                ActualParameters[Counter] = CodeDom.Local("ATransaction");
            }
            else
            {
                ActualParameters[Counter] = CodeDom._Const(null);
            }

            Counter = Counter + 1;

            if (AWithOrderBy == true)
            {
                ActualParameters[Counter] = CodeDom.Local("AOrderBy");
            }
            else
            {
                ActualParameters[Counter] = CodeDom._Const(null);
            }

            Counter = Counter + 1;

            // AStartRecord
            ActualParameters[Counter] = CodeDom._Const((System.Object) 0);
            Counter = Counter + 1;

            // AMaxRecords
            ActualParameters[Counter] = CodeDom._Const((System.Object) 0);
            Counter = Counter + 1;
            ReturnValue.Statements.Add(CodeDom.Eval(CodeDom.GlobalMethodInvoke(AProcedureName, ActualParameters)));
            return ReturnValue;
        }

        public static CodeMemberMethod CreateOverLoadTable(TTable ATable,
            String AProcedureName,
            TTable ATableForPrimaryKey,
            String AParamTemplateDataRowType,
            bool AWithFieldList,
            bool AWithTransaction,
            bool AWithOrderBy,
            bool AWithTemplateOperators)
        {
            CodeMemberMethod ReturnValue;
            TConstraint constr;
            Int32 NumberOfParameters;

            CodeExpression[] ActualParameters;
            Int32 Counter;
            ReturnValue = new CodeMemberMethod();
            ReturnValue.Name = AProcedureName;
            ReturnValue.Comments.Add(new CodeCommentStatement("auto generated", true));
            ReturnValue.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            NumberOfParameters = MakeFormalParameters(ReturnValue,
                out constr,
                ATable,
                ATableForPrimaryKey,
                "DataTable",
                AParamTemplateDataRowType,
                AWithFieldList,
                AWithTransaction,
                AWithOrderBy,
                true,
                AWithTemplateOperators);

            /*
             * FillDataSet := new  DataSet();
             * DataSet.Tables.Add(ADataTable);
             */
            ReturnValue.Statements.Add(CodeDom.VarDecl("DataSet", "FillDataSet", CodeDom._New("DataSet", new CodeExpression[] { })));
            ReturnValue.Statements.Add(CodeDom.Let(CodeDom.Local("AData"),
                    CodeDom._New(TTable.NiceTableName(ATable.strName) + "Table", new CodeExpression[] { })));
            ReturnValue.Statements.Add(CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.PropRef(CodeDom.Local("FillDataSet"), "Tables"),
                        "Add", new CodeExpression[] { CodeDom.Local("AData") })));

            // prepare the call statement of the long version of the procedure
            ActualParameters = new CodeExpression[NumberOfParameters];
            ActualParameters[0] = CodeDom.Local("FillDataSet");
            Counter = 1;

            if (ATableForPrimaryKey != null)
            {
                InsertActualParametersPrimaryKey(ref Counter, ref ActualParameters, ATableForPrimaryKey, constr);
            }
            else if (AParamTemplateDataRowType != null)
            {
                ActualParameters[Counter] = CodeDom.Local("ATemplateRow");
                Counter = Counter + 1;

                if (AWithTemplateOperators == true)
                {
                    ActualParameters[Counter] = CodeDom.Local("ATemplateOperators");
                }
                else
                {
                    ActualParameters[Counter] = CodeDom._Const(null);
                }

                Counter = Counter + 1;
            }

            if (AWithFieldList == true)
            {
                ActualParameters[Counter] = CodeDom.Local("AFieldList");
            }
            else
            {
                ActualParameters[Counter] = CodeDom._Const(null);
            }

            Counter = Counter + 1;

            if (AWithTransaction == true)
            {
                ActualParameters[Counter] = CodeDom.Local("ATransaction");
            }
            else
            {
                ActualParameters[Counter] = CodeDom._Const(null);
            }

            Counter = Counter + 1;

            if (AWithOrderBy == true)
            {
                ActualParameters[Counter] = CodeDom.Local("AOrderBy");
            }
            else
            {
                ActualParameters[Counter] = CodeDom._Const(null);
            }

            Counter = Counter + 1;

            // AStartRecord
            ActualParameters[Counter] = CodeDom.Local("AStartRecord");
            Counter = Counter + 1;

            // AMaxRecords
            ActualParameters[Counter] = CodeDom.Local("AMaxRecords");
            Counter = Counter + 1;
            ReturnValue.Statements.Add(CodeDom.Eval(
                    CodeDom.GlobalMethodInvoke(AProcedureName, ActualParameters)));

            /*
             * need to remove the table again from the dataset, in order to be able to move it to another dataset
             * (e.g. used in Cache)
             * DataSet.Tables.Add(ADataTable);
             */
            ReturnValue.Statements.Add(
                CodeDom.Eval(
                    CodeDom.MethodInvoke(CodeDom.PropRef(
                            CodeDom.Local("FillDataSet"), "Tables"), "Remove",
                        new CodeExpression[] { CodeDom.Local("AData") })));
            return ReturnValue;
        }

        public static void CreateOverLoads(ref CodeTypeDeclaration ATableClass,
            String AProcedureName,
            TTable ATableToLoad,
            TTable ATableForPrimaryKey,
            String AParamTemplateDataRowType)
        {
            ATableClass.Members.Add(CreateOverLoad(AProcedureName, ATableToLoad, ATableForPrimaryKey, "DataSet", AParamTemplateDataRowType, false,
                    true, false, false));
            ATableClass.Members.Add(CreateOverLoad(AProcedureName, ATableToLoad, ATableForPrimaryKey, "DataSet", AParamTemplateDataRowType, true,
                    true, false, false));

            // ATableClass.Members.Add(CreateOverLoad(AProcedureName, ATableToLoad, ATableForPrimaryKey,
            // 'DataSet', AParamTemplateDataRowType, false, true, false));
            ATableClass.Members.Add(CreateOverLoadTable(ATableToLoad, AProcedureName, ATableForPrimaryKey, AParamTemplateDataRowType, true, true,
                    true, true));
            ATableClass.Members.Add(CreateOverLoad(AProcedureName, ATableToLoad, ATableForPrimaryKey, "DataTable", AParamTemplateDataRowType,
                    false, true, false, false));
            ATableClass.Members.Add(CreateOverLoad(AProcedureName, ATableToLoad, ATableForPrimaryKey, "DataTable", AParamTemplateDataRowType,
                    true, true, false, false));

            if (AParamTemplateDataRowType != null)
            {
                // for the ATemplateOperators
                // cannot have either ATemplateOperators or AFieldList, because they have the same type; so need both
                ATableClass.Members.Add(CreateOverLoad(AProcedureName, ATableToLoad, ATableForPrimaryKey, "DataTable",
                        AParamTemplateDataRowType, true, true, false, true));
            }

            // ATableClass.Members.Add(CreateOverLoad(AProcedureName, ATableToLoad, ATableForPrimaryKey,
            // 'DataTable', AParamTemplateDataRowType, false, true, false));
        }

        public static void CreateCountOverLoads(ref CodeTypeDeclaration ATableClass,
            String AProcedureName,
            TTable ATableToLoad,
            TTable ATableForPrimaryKey,
            String AParamTemplateDataRowType)
        {
            // would only be without transaction, not necessary, just use nil
            // ATableClass.Members.Add(CreateCountOverLoad(AProcedureName, ATableToLoad, ATableForPrimaryKey,
            // '', AParamTemplateDataRowType, false));
        }

        public static CodeExpression[] GetActualParameters(CodeExpression AExpFirst,
            CodeExpression AExpBeforeLast,
            CodeExpression AExpLast,
            TTable table)
        {
            TTableField tablefield;
            TConstraint constr;
            Int32 i;
            Int32 Length;

            try
            {
                constr = table.GetPrimaryKey();
            }
            catch (Exception)
            {
                // Console.WriteLine(e.Message);
                constr = new TConstraint();
            }
            Length = constr.strThisFields.Count;

            if (AExpFirst != null)
            {
                Length = Length + 1;
            }

            if (AExpBeforeLast != null)
            {
                Length = Length + 1;
            }

            if (AExpLast != null)
            {
                Length = Length + 1;
            }

            CodeExpression[] ReturnValue = new CodeExpression[Length];
            i = 0;

            if (AExpFirst != null)
            {
                ReturnValue[i] = AExpFirst;
                i = i + 1;
            }

            foreach (string fieldname in constr.strThisFields)
            {
                tablefield = table.GetField(fieldname);
                ReturnValue[i] = CodeDom.Local('A' + TTable.NiceFieldName(tablefield));
                i = i + 1;
            }

            if (AExpBeforeLast != null)
            {
                ReturnValue[i] = AExpBeforeLast;
                i = i + 1;
            }

            if (AExpLast != null)
            {
                ReturnValue[i] = AExpLast;
            }

            return ReturnValue;
        }

        public static CodeExpression[] GetOriginalParametersFromTypedRow(String ARowVariable,
            String ATableVariable,
            CodeExpression AExpFirst,
            CodeExpression AExpBeforeLast,
            CodeExpression AExpLast,
            TTable table)
        {
            TTableField tablefield;
            TConstraint constr;
            Int32 i;
            Int32 Length;

            try
            {
                constr = table.GetPrimaryKey();
            }
            catch (Exception)
            {
                // Console.WriteLine(e.Message);
                constr = new TConstraint();
            }
            Length = constr.strThisFields.Count;

            if (AExpFirst != null)
            {
                Length = Length + 1;
            }

            if (AExpBeforeLast != null)
            {
                Length = Length + 1;
            }

            if (AExpLast != null)
            {
                Length = Length + 1;
            }

            CodeExpression[] ReturnValue = new CodeExpression[Length];
            i = 0;

            if (AExpFirst != null)
            {
                ReturnValue[i] = AExpFirst;
                i = i + 1;
            }

            foreach (string fieldname in constr.strThisFields)
            {
                tablefield = table.GetField(fieldname);

                // ReturnValue[i] := CodeDom.PropRef(CodeDom.Local(ARowVariable), TTable.NiceFieldName(tablefield.strName));
                ReturnValue[i] = CodeDom.MethodInvoke(
                    CodeDom.Local("Convert"), "To" + codeGenerationPetra.ToDelphiType(tablefield),
                    new CodeExpression[] { CodeDom.IndexerRef(
                                               CodeDom.Local(ARowVariable), new CodeExpression[] {
                                                   CodeDom.MethodInvoke(CodeDom.Local(ATableVariable),
                                                       "Get" + TTable.NiceFieldName(tablefield.strName) +
                                                       "DBName",
                                                       new CodeExpression[] { }),
                                                   CodeDom.PropRef(CodeDom.Local("DataRowVersion"), "Original")
                                               }) });
                i = i + 1;
            }

            if (AExpBeforeLast != null)
            {
                ReturnValue[i] = AExpBeforeLast;
                i = i + 1;
            }

            if (AExpLast != null)
            {
                ReturnValue[i] = AExpLast;
            }

            return ReturnValue;
        }

        public static CodeExpression[] GetActualParametersFromTypedRow(String ARowVariable,
            CodeExpression AExpFirst,
            CodeExpression AExpBeforeLast,
            CodeExpression AExpLast,
            TTable table)
        {
            TTableField tablefield;
            TConstraint constr;
            Int32 i;
            Int32 Length;

            try
            {
                constr = table.GetPrimaryKey();
            }
            catch (Exception)
            {
                // Console.WriteLine(e.Message);
                constr = new TConstraint();
            }
            Length = constr.strThisFields.Count;

            if (AExpFirst != null)
            {
                Length = Length + 1;
            }

            if (AExpBeforeLast != null)
            {
                Length = Length + 1;
            }

            if (AExpLast != null)
            {
                Length = Length + 1;
            }

            CodeExpression[] result = new CodeExpression[Length];
            i = 0;

            if (AExpFirst != null)
            {
                result[i] = AExpFirst;
                i = i + 1;
            }

            foreach (string fieldname in constr.strThisFields)
            {
                tablefield = table.GetField(fieldname);
                result[i] = CodeDom.PropRef(CodeDom.Local(ARowVariable), TTable.NiceFieldName(tablefield.strName));

                /* result[i] := CodeDom.MethodInvoke(CodeDom.Local('Convert'),
                 * 'To'+toDelphiType(tableField),
                 * CodeExpression[].Create(
                 * IndexerRef(CodeDom.PropRef(CodeDom.Local(ARowVariable), 'Item'), CodeExpression[].Create(CodeDom._Const(tableField.strName)))
                 * ));
                 */
                i = i + 1;
            }

            if (AExpBeforeLast != null)
            {
                result[i] = AExpBeforeLast;
                i = i + 1;
            }

            if (AExpLast != null)
            {
                result[i] = AExpLast;
            }

            return result;
        }

        public static String GenerateWhereStatement(TTable ATable, StringCollection AThisFields)
        {
            String ReturnValue;
            TTableField tablefield;
            Int32 Count;

            /*
             * SqlQuery := SqlQuery + ' WHERE ';
             * SqlQuery := SqlQuery + 'p_partner_key_n = ? '
             * SqlQuery := SqlQuery + ' AND ';
             * SqlQuery := SqlQuery + 'p_location_key_n = ?'
             */
            ReturnValue = "";

            if (AThisFields.Count > 0)
            {
                ReturnValue = ReturnValue + " WHERE ";

                for (Count = 0; Count <= AThisFields.Count - 1; Count += 1)
                {
                    tablefield = ATable.GetField(AThisFields[Count]);

                    if (Count > 0)
                    {
                        ReturnValue = ReturnValue + " AND ";
                    }

                    ReturnValue = ReturnValue + tablefield.strName + " = ?";
                }
            }

            return ReturnValue;
        }

        public static String GenerateWhereStatementTemplate(TConstraint AConstraint)
        {
            String ReturnValue;
            Int32 Count;

            /*
             * // e.g. AGiftDetailAccess.LoadViaAGiftTemplate: load all gift details of gifts that are from donor p_donor_key_n
             * SELECT a_gift_detail.*
             * FROM a_gift_detail, a_gift
             * => ***      WHERE gift.ledger,batch,giftnumber = giftdetail.ledger,batch,giftnumber // ADirectReference ****
             * AND a_gift.p_donor_key_n = ?      // depending on values in DataRow
             */
            ReturnValue = "";
            ReturnValue = ReturnValue + " WHERE ";

            for (Count = 0; Count <= AConstraint.strThisFields.Count - 1; Count += 1)
            {
                if (Count > 0)
                {
                    ReturnValue = ReturnValue + " AND ";
                }

                ReturnValue = ReturnValue + AConstraint.strThisTable + '.' + AConstraint.strThisFields[Count] + " = " +
                              AConstraint.strOtherTable + '.' + AConstraint.strOtherFields[Count];
            }

            return ReturnValue;
        }

        public static String GenerateWhereStatement(TTable ALinkTable, TConstraint AOtherConstraint, TConstraint ADirectReference)
        {
            String ReturnValue;
            Int32 Count;

            /*
             * WHERE p_partner.p_partner_key_n = p_partner_location.p_partner_key_n // ADirectReference
             * AND p_partner_location.p_site_key_n = ?      // AOtherConstraint
             * AND p_partner_location.p_location_key_i = ?  // AOtherConstraint
             */
            ReturnValue = " WHERE ";

            for (Count = 0; Count <= ADirectReference.strThisFields.Count - 1; Count += 1)
            {
                if (Count > 0)
                {
                    ReturnValue = ReturnValue + " AND ";
                }

                ReturnValue = ReturnValue + "PUB_" + ADirectReference.strThisTable + '.' + ADirectReference.strThisFields[Count] + " = " +
                              "PUB_" + ADirectReference.strOtherTable + '.' + ADirectReference.strOtherFields[Count];
            }

            foreach (string s in AOtherConstraint.strThisFields)
            {
                ReturnValue = ReturnValue + " AND PUB_" + AOtherConstraint.strThisTable + '.' + s + " = ?";
            }

            return ReturnValue;
        }

        public static String GenerateWhereStatementTemplate(TTable ALinkTable, TConstraint AOtherConstraint, TConstraint ADirectReference)
        {
            String ReturnValue;
            Int32 Count;

            /*
             * // e.g. PPartner.LoadViaPLocationTemplate: load all partners that live in a specific road
             * SELECT p_partner.*
             * FROM p_partner_location, p_partner, p_location
             * WHERE p_partner.p_partner_key_n = p_partner_location.p_partner_key_n // ADirectReference
             * AND p_partner_location.p_location_key_i = p_location.p_location_key_i // AOtherConstraint
             * AND p_location.p_street_name_c = ?      // depending on values in DataRow
             * AND p_location.??? = ?  // depending on values in DataRow
             */
            ReturnValue = " WHERE ";

            for (Count = 0; Count <= ADirectReference.strThisFields.Count - 1; Count += 1)
            {
                if (Count > 0)
                {
                    ReturnValue = ReturnValue + " AND ";
                }

                ReturnValue = ReturnValue + "PUB_" + ADirectReference.strThisTable + '.' + ADirectReference.strThisFields[Count] + " = " +
                              "PUB_" + ADirectReference.strOtherTable + '.' + ADirectReference.strOtherFields[Count];
            }

            for (Count = 0; Count <= AOtherConstraint.strThisFields.Count - 1; Count += 1)
            {
                ReturnValue = ReturnValue + " AND ";
                ReturnValue = ReturnValue + "PUB_" + AOtherConstraint.strThisTable + '.' + AOtherConstraint.strThisFields[Count] + " = " +
                              "PUB_" + AOtherConstraint.strOtherTable + '.' + AOtherConstraint.strOtherFields[Count];
            }

            return ReturnValue;
        }

        public static void GenerateParameterArrayActualValues(ref CodeMemberMethod AMethod, TTable ATable, StringCollection AThisFields)
        {
            TTableField tablefield;

            CodeExpression[] parameters;
            Int32 Count;

            /*
             * var
             * ParametersArray: array of OdbcParameter;
             * ParametersArray = new SomeType[2];
             */
            AMethod.Statements.Add(CodeDom.VarDecl("OdbcParameter[]", "ParametersArray",
                    new CodeArrayCreateExpression("OdbcParameter", AThisFields.Count)));

            /*
             * ParametersArray[0] := new  OdbcParameter('', OdbcType.Decimal, 10);
             * ParametersArray[0].Value := APartnerKey as System.Object;
             * ParametersArray[0] := new  OdbcParameter('', OdbcType.Decimal, 10);
             * ParametersArray[0].Value := ALocationKey as System.Object;
             */
            for (Count = 0; Count <= AThisFields.Count - 1; Count += 1)
            {
                tablefield = ATable.GetField(AThisFields[Count]);

                if (tablefield.iLength != -1)
                {
                    parameters = new CodeExpression[] {
                        CodeDom._Const(""),
                        codeGenerationPetra.ToOdbcType(tablefield),
                        CodeDom._Const((System.Object)tablefield.iLength)
                    };
                }
                else
                {
                    parameters = new CodeExpression[] {
                        CodeDom._Const(""),
                        codeGenerationPetra.ToOdbcType(tablefield)
                    };
                }

                AMethod.Statements.Add(CodeDom.Let(
                        CodeDom.IndexerRef(CodeDom.Local("ParametersArray"),
                            new CodeExpression[] { CodeDom._Const((System.Object)Count) }),
                        CodeDom._New("OdbcParameter", parameters)));
                AMethod.Statements.Add(CodeDom.Let(
                        CodeDom.PropRef(
                            CodeDom.IndexerRef(CodeDom.Local("ParametersArray"),
                                new CodeExpression[] {
                                    CodeDom._Const((System.Object)Count)
                                }),
                            "Value"),
                        CodeDom.Cast("System.Object",
                            CodeDom.Local('A' +
                                TTable.NiceFieldName(tablefield)))));
            }
        }

        public static CodeExpression GetPrimaryKeyFields(TTable ATable)
        {
            CodeExpression ReturnValue;

            CodeExpression[] primarykeyfields;
            TConstraint constrPrimKey;
            System.Int32 Counter;
            try
            {
                constrPrimKey = ATable.GetPrimaryKey();
                primarykeyfields = new CodeExpression[constrPrimKey.strThisFields.Count];
                Counter = 0;

                foreach (string field in constrPrimKey.strThisFields)
                {
                    primarykeyfields[Counter] = CodeDom._Const(field);
                    Counter = Counter + 1;
                }

                ReturnValue = CodeDom.NewArray(typeof(String), primarykeyfields);
            }
            catch (Exception)
            {
                primarykeyfields = new CodeExpression[0];
                ReturnValue = CodeDom._Const(null);
            }
            return ReturnValue;
        }

        public static CodeConditionStatement CreateCheckEmtpyResultTableInDataset(String ATypedTableName)
        {
            /*
             * if ADataSet.Tables[PPartnerTable.GetTableName].Rows.Count = 0 then
             * begin
             * raise EDBNoRecordsReturnedWarningException.Create();
             * end;
             */
            return new CodeConditionStatement(
                CodeDom.Equals(CodeDom.PropRef(CodeDom.PropRef(CodeDom.IndexerRef(
                                CodeDom.PropRef(CodeDom.Local("ADataSet"), "Tables"),
                                new CodeExpression[] { CodeDom.StaticMethodInvoke(
                                                           ATypedTableName + "Table",
                                                           "GetTableName",
                                                           new CodeExpression[] { }) }),
                            "Rows"), "Count"),
                    CodeDom._Const((System.Object) 0)),
                new CodeStatement[] {
                    CodeDom.Throw(CodeDom._New("EDBNoRecordsReturnedWarningException",
                            new CodeExpression[] {
                                CodeDom._Const("no filtered rows in " + ATypedTableName + "Table")
                            }))
                },
                new CodeStatement[] { });
        }

        public static CodeConditionStatement CreateCheckEmtpyResultTable(String ATypedTableName)
        {
            /*
             * if ADataTable.Rows.Count = 0 then
             * begin
             * raise EDBNoRecordsReturnedWarningException.Create();
             * end;
             */
            return new CodeConditionStatement
                       (CodeDom.Equals(CodeDom.PropRef(CodeDom.PropRef(CodeDom.Local("ADataTable"),
                                   "Rows"), "Count"), CodeDom._Const((System.Object) 0)),
                       new CodeStatement[] {
                           CodeDom.Throw(CodeDom._New("EDBNoRecordsReturnedWarningException",
                                   new CodeExpression[] { }))
                       },
                       new CodeStatement[] { });
        }

        public static CodeMemberMethod CreateLoadAllLongCode(TTable ATable, String ATypedTableName)
        {
            String WhereStmt;
            CodeExpression SqlQuery;
            CodeMemberMethod result = new CodeMemberMethod();

            result.Name = "LoadAll";
            result.Comments.Add(new CodeCommentStatement("this method is called by all overloads", true));
            result.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            result.Parameters.Add(CodeDom.Param("DataSet", "ADataSet"));
            result.Parameters.Add(CodeDom.Param("StringCollection", "AFieldList"));
            result.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            result.Parameters.Add(CodeDom.Param("StringCollection", "AOrderBy"));
            result.Parameters.Add(CodeDom.Param("System.Int32", "AStartRecord"));
            result.Parameters.Add(CodeDom.Param("System.Int32", "AMaxRecords"));
            WhereStmt = " FROM PUB_" + ATable.strName;
            SqlQuery = new CodeBinaryOperatorExpression(
                new CodeBinaryOperatorExpression(CodeDom.GlobalMethodInvoke("GenerateSelectClause",
                        new CodeExpression[] { CodeDom.Local("AFieldList"),
                                               GetPrimaryKeyFields(ATable) }),
                    CodeBinaryOperatorType.Add,
                    CodeDom._Const(WhereStmt)),
                CodeBinaryOperatorType.Add,
                CodeDom.GlobalMethodInvoke("GenerateOrderByClause",
                    new CodeExpression[] { CodeDom.Local("AOrderBy") }));

            /*
             * ADataSet := DBAccess.GDBAccessObj.Select(
             * ADataSet,
             * 'SELECT * FROM PUB_p_partner',
             * PPartnerTable.GetTableName,
             * ATransaction);
             */
            result.Statements.Add(CodeDom.Let(CodeDom.Local("ADataSet"), CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "Select",
                        new CodeExpression[] { CodeDom.Local("ADataSet"),
                                               SqlQuery,
                                               CodeDom.StaticMethodInvoke(
                                                   ATypedTableName +
                                                   "Table",
                                                   "GetTableName",
                                                   new
                                                   CodeExpression[] { }),
                                               CodeDom.Local("ATransaction"),
                                               CodeDom.Local("AStartRecord"),
                                               CodeDom.Local("AMaxRecords") })));
            return result;
        }

        public static CodeMemberMethod CreateLoadByPrimaryKeyLongCode(TTable table, String typedTableName)
        {
            CodeMemberMethod result;
            TConstraint PrimaryKeyConstraint;
            String WhereStmt;
            CodeExpression SqlQuery;

            result = new CodeMemberMethod();
            result.Name = "LoadByPrimaryKey";
            result.Comments.Add(new CodeCommentStatement("this method is called by all overloads", true));
            result.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            result.Parameters.Add(CodeDom.Param("DataSet", "ADataSet"));
            AddPrimaryKeyParameters(result, table);
            result.Parameters.Add(CodeDom.Param("StringCollection", "AFieldList"));
            result.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            result.Parameters.Add(CodeDom.Param("StringCollection", "AOrderBy"));
            result.Parameters.Add(CodeDom.Param("System.Int32", "AStartRecord"));
            result.Parameters.Add(CodeDom.Param("System.Int32", "AMaxRecords"));
            try
            {
                PrimaryKeyConstraint = table.GetPrimaryKey();
            }
            catch (Exception)
            {
                PrimaryKeyConstraint = new TConstraint();
            }
            GenerateParameterArrayActualValues(ref result, table, PrimaryKeyConstraint.strThisFields);
            WhereStmt = " FROM PUB_" + table.strName;
            WhereStmt = WhereStmt + GenerateWhereStatement(table, PrimaryKeyConstraint.strThisFields);
            SqlQuery =
                new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(CodeDom.GlobalMethodInvoke("GenerateSelectClause",
                            new CodeExpression[]
                            { CodeDom.Local("AFieldList"),
                              GetPrimaryKeyFields(table) }),
                        CodeBinaryOperatorType.Add, CodeDom._Const(
                            WhereStmt)), CodeBinaryOperatorType.Add,
                    CodeDom.GlobalMethodInvoke("GenerateOrderByClause",
                        new
                        CodeExpression
                        [] { CodeDom.Local("AOrderBy") }));

            /*
             * ADataSet := DBAccess.GDBAccessObj.Select(
             * ADataSet,
             * SqlQuery,
             * PPartnerTable.GetTableName,
             * ATransaction,
             * ParametersArray);
             */
            result.Statements.Add(CodeDom.Let(CodeDom.Local("ADataSet"),
                    CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "Select",
                        new CodeExpression[] { CodeDom.Local("ADataSet"), SqlQuery,
                                               CodeDom
                                               .
                                               StaticMethodInvoke(typedTableName + "Table",
                                                   "GetTableName",
                                                   new CodeExpression[] { }),
                                               CodeDom
                                               .Local(
                                                   "ATransaction"),
                                               CodeDom
                                               .Local(
                                                   "ParametersArray"),
                                               CodeDom
                                               .Local(
                                                   "AStartRecord"),
                                               CodeDom
                                               .Local(
                                                   "AMaxRecords") })));
            return result;
        }

        public static CodeMemberMethod CreateLoadUsingTemplateLongCode(TTable table, String TypedTableName)
        {
            CodeMemberMethod result;
            CodeExpression SqlQuery;

            result = new CodeMemberMethod();
            result.Comments.Add(new CodeCommentStatement("this method is called by all overloads", true));
            result.Name = "LoadUsingTemplate";
            result.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            result.Parameters.Add(CodeDom.Param("DataSet", "ADataSet"));
            result.Parameters.Add(CodeDom.Param(TypedTableName + "Row", "ATemplateRow"));
            result.Parameters.Add(CodeDom.Param("StringCollection", "ATemplateOperators"));
            result.Parameters.Add(CodeDom.Param("StringCollection", "AFieldList"));
            result.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            result.Parameters.Add(CodeDom.Param("StringCollection", "AOrderBy"));
            result.Parameters.Add(CodeDom.Param("System.Int32", "AStartRecord"));
            result.Parameters.Add(CodeDom.Param("System.Int32", "AMaxRecords"));
            SqlQuery =
                new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(CodeDom.GlobalMethodInvoke(
                                "GenerateSelectClause",
                                new CodeExpression
                                []
                                {
                                    CodeDom.
                                    Local(
                                        "AFieldList"),
                                    GetPrimaryKeyFields(
                                        table)
                                }),
                            CodeBinaryOperatorType.Add,
                            CodeDom._Const(
                                " FROM PUB_" +
                                table.strName)),
                        CodeBinaryOperatorType.Add,
                        CodeDom.GlobalMethodInvoke("GenerateWhereClause",
                            new CodeExpression[] {
                                CodeDom
                                .StaticMethodInvoke(
                                    TypedTableName +
                                    "Table",
                                    "GetColumnStringList",
                                    new
                                    CodeExpression
                                    [] { }),
                                CodeDom
                                .Local("ATemplateRow"),
                                CodeDom
                                .Local(
                                    "ATemplateOperators")
                            })),
                    CodeBinaryOperatorType.Add,
                    CodeDom.GlobalMethodInvoke("GenerateOrderByClause",

                        new CodeExpression[] { CodeDom.Local("AOrderBy") }));

            /*
             * ADataSet := DBAccess.GDBAccessObj.Select(
             * ADataSet,
             * SqlQuery,
             * PPartnerTable.GetTableName,
             * ATransaction,
             * nil);
             */
            result.Statements.Add(CodeDom.Let(CodeDom.Local("ADataSet"),
                    CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "Select",
                        new CodeExpression[] { CodeDom.Local("ADataSet"), SqlQuery,
                                               CodeDom.StaticMethodInvoke(TypedTableName + "Table",
                                                   "GetTableName",
                                                   new CodeExpression[] { }),
                                               CodeDom.Local("ATransaction"),
                                               CodeDom.GlobalMethodInvoke("GetParametersForWhereClause",
                                                   new CodeExpression[] {
                                                       CodeDom
                                                       .Local("ATemplateRow")
                                                   }),
                                               CodeDom.Local("AStartRecord"),
                                               CodeDom.Local("AMaxRecords") })));

            return result;
        }

        public static CodeMemberMethod CreateCountAllLongCode(TTable ATable, String ATypedTableName)
        {
            CodeMemberMethod Result;
            String WhereStmt;
            CodeExpression SqlQuery;

            Result = new CodeMemberMethod();
            Result.Name = "CountAll";
            Result.Comments.Add(new CodeCommentStatement("this method is called by all overloads", true));
            Result.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            Result.ReturnType = CodeDom.TypeRef("System.Int32");
            Result.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            WhereStmt = "SELECT COUNT(*) FROM PUB_" + ATable.strName;
            SqlQuery = CodeDom._Const(WhereStmt);

            /*
             * result := DBAccess.GDBAccessObj.ExecuteScalar(
             * 'SELECT Count(*) FROM PUB_p_partner',
             * ATransaction, false);
             */
            Result.Statements.Add(
                CodeDom.Return(
                    CodeDom.MethodInvoke(CodeDom.Local("Convert"), "ToInt32",
                        new CodeExpression[] {
                            CodeDom.MethodInvoke(
                                CodeDom.Local("DBAccess.GDBAccessObj"),
                                "ExecuteScalar",
                                new CodeExpression[] {
                                    SqlQuery,
                                    CodeDom.Local("ATransaction"),
                                    CodeDom._Const((System.Object)false)
                                })
                        })));
            return Result;
        }

        public static CodeMemberMethod CreateCountByPrimaryKeyLongCode(TTable table, String typedTableName)
        {
            CodeMemberMethod Result;
            TConstraint PrimaryKeyConstraint;
            String WhereStmt;
            CodeExpression SqlQuery;

            Result = new CodeMemberMethod();
            Result.Comments.Add(new CodeCommentStatement("this method is called by all overloads", true));
            Result.Name = "CountByPrimaryKey";
            Result.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            Result.ReturnType = CodeDom.TypeRef("System.Int32");
            AddPrimaryKeyParameters(Result, table);
            Result.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            try
            {
                PrimaryKeyConstraint = table.GetPrimaryKey();
            }
            catch (Exception)
            {
                PrimaryKeyConstraint = new TConstraint();
            }
            GenerateParameterArrayActualValues(ref Result, table, PrimaryKeyConstraint.strThisFields);
            WhereStmt = "SELECT COUNT(*) FROM PUB_" + table.strName;
            WhereStmt = WhereStmt + GenerateWhereStatement(table, PrimaryKeyConstraint.strThisFields);
            SqlQuery = CodeDom._Const(WhereStmt);
            Result.Statements.Add(CodeDom.Return(
                    CodeDom.MethodInvoke(CodeDom.Local("Convert"), "ToInt32",
                        new CodeExpression[] {
                            CodeDom.MethodInvoke(
                                CodeDom.Local("DBAccess.GDBAccessObj"),
                                "ExecuteScalar",
                                new CodeExpression[] {
                                    SqlQuery,
                                    CodeDom.Local("ATransaction"),
                                    CodeDom._Const((System.Object)false),
                                    CodeDom.Local("ParametersArray")
                                })
                        })));
            return Result;
        }

        public static CodeMemberMethod CreateCountUsingTemplateLongCode(TTable table, String TypedTableName)
        {
            CodeMemberMethod Result;
            CodeExpression SqlQuery;

            Result = new CodeMemberMethod();
            Result.Name = "CountUsingTemplate";
            Result.Comments.Add(new CodeCommentStatement("this method is called by all overloads", true));
            Result.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            Result.ReturnType = CodeDom.TypeRef("System.Int32");
            Result.Parameters.Add(CodeDom.Param(TypedTableName + "Row", "ATemplateRow"));
            Result.Parameters.Add(CodeDom.Param("StringCollection", "ATemplateOperators"));
            Result.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            SqlQuery = new CodeBinaryOperatorExpression(
                CodeDom._Const("SELECT COUNT(*) FROM PUB_" + table.strName),
                CodeBinaryOperatorType.Add,
                CodeDom.GlobalMethodInvoke("GenerateWhereClause",
                    new CodeExpression[] {
                        CodeDom.StaticMethodInvoke(TypedTableName + "Table",
                            "GetColumnStringList",
                            new CodeExpression[] { }),
                        CodeDom.Local("ATemplateRow"),
                        CodeDom.Local("ATemplateOperators")
                    }));
            Result.Statements.Add(CodeDom.Return(
                    CodeDom.MethodInvoke(CodeDom.Local("Convert"), "ToInt32",
                        new CodeExpression[] {
                            CodeDom.MethodInvoke(
                                CodeDom.Local("DBAccess.GDBAccessObj"),
                                "ExecuteScalar",
                                new CodeExpression[] {
                                    SqlQuery,
                                    CodeDom.Local("ATransaction"),
                                    CodeDom._Const((System.Object)(false)),
                                    CodeDom.GlobalMethodInvoke(
                                        "GetParametersForWhereClause",
                                        new
                                        CodeExpression
                                        [] { CodeDom.Local("ATemplateRow") })
                                })
                        })));
            return Result;
        }

        public static CodeMemberMethod CreateSubmitChangesCode(TTable table, String TypedTableName)
        {
            CodeMemberMethod Result;
            CodeCatchClause CatchClause;
            CodeCatchClause CatchClauseFailedUpdate;
            CodeCatchClause RaiseException;

//		Int32 CounterConstraints;
            CodeStatement[] loopContentTable;
            CodeStatement[] DealWithRow;
            CodeStatement UpdateRow;
            CodeStatement InsertRow;
            CodeStatement DeleteRow;
            TConstraint PrimaryKeyConstraint;
            CodeStatement InsertCall;
            CodeStatement UpdateCall;
            CodeStatement DeleteCall;
            CodeStatement[] InsertStmts;
            CodeExpression sequence;
            Result = new CodeMemberMethod();
            Result.Name = "SubmitChanges";
            Result.Comments.Add(new CodeCommentStatement("auto generated", true));
            Result.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            Result.ReturnType = CodeDom.TypeRef(typeof(bool));
            Result.Parameters.Add(CodeDom.Param(TypedTableName + "Table", "ATable"));
            Result.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            Result.Parameters.Add(CodeDom.Param("TVerificationResultCollection", "AVerificationResult", FieldDirection.Out));
            Result.Statements.Add(CodeDom.VarDecl("System.Boolean", "ResultValue", CodeDom._Const((System.Object)true)));
            Result.Statements.Add(CodeDom.VarDecl("System.Boolean", "ExceptionReported", CodeDom._Const((System.Object)false)));
            Result.Statements.Add(CodeDom.VarDecl("DataRow", "TheRow", CodeDom._Const(null)));
            Result.Statements.Add(CodeDom.Let(CodeDom.Local("AVerificationResult"),
                    CodeDom._New("TVerificationResultCollection", new CodeExpression[] { })));
            InsertCall = CodeDom.Eval(CodeDom.StaticMethodInvoke("TTypedDataAccess", "InsertRow", new CodeExpression[]
                    { CodeDom._Const(table.strName),
                      CodeDom.StaticMethodInvoke(TypedTableName + "Table",
                          "GetColumnStringList",
                          new CodeExpression[] { }),
                      new CodeDirectionExpression(FieldDirection.Ref,
                          CodeDom.Local("TheRow")),
                      CodeDom.Local("ATransaction"),
                      CodeDom.PropRef(CodeDom.PropRef(CodeDom.Local("UserInfo"),
                              "GUserInfo"),
                          "UserID") }));
            InsertStmts = new CodeStatement[] {
                InsertCall
            };

            foreach (TTableField tablefield in table.grpTableField.List)
            {
                // is there a field filled by a sequence?
                // yes: get the next value of that sequence and assign to row
                if (tablefield.strSequence.Length > 0)
                {
                    if (codeGenerationPetra.ToDelphiType(tablefield) != "Int64")
                    {
                        sequence =
                            CodeDom.Cast(codeGenerationPetra.ToDelphiType(tablefield),
                                CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "GetNextSequenceValue",
                                    new CodeExpression[]
                                    { CodeDom._Const(
                                          tablefield
                                          .strSequence), CodeDom.Local("ATransaction") }));
                    }
                    else
                    {
                        sequence = CodeDom.MethodInvoke(CodeDom.Local(
                                "DBAccess.GDBAccessObj"), "GetNextSequenceValue", new CodeExpression[]
                            { CodeDom._Const(tablefield.strSequence), CodeDom.Local("ATransaction") });
                    }

                    InsertStmts =
                        new CodeStatement[] {
                        CodeDom.Let(CodeDom.PropRef(CodeDom.Cast(TypedTableName + "Row",
                                    CodeDom.Local("TheRow")),
                                TTable.NiceFieldName(tablefield)), sequence), InsertCall
                    };

                    // assume only one sequence per table
                    break;
                }
            }

            InsertRow =
                new CodeConditionStatement(CodeDom.Equals(CodeDom.PropRef(CodeDom.Local("TheRow"),
                            "RowState"), CodeDom.PropRef(CodeDom.Local(
                                "DataRowState"),
                            "Added")), InsertStmts);
            try
            {
                PrimaryKeyConstraint = table.GetPrimaryKey();
                UpdateCall = CodeDom.Eval(CodeDom.StaticMethodInvoke("TTypedDataAccess", "UpdateRow", new CodeExpression[]
                        { CodeDom._Const(table.strName),
                          CodeDom.StaticMethodInvoke(TypedTableName + "Table",
                              "GetColumnStringList",
                              new CodeExpression[] { }),
                          CodeDom.StaticMethodInvoke(TypedTableName + "Table",
                              "GetPrimKeyColumnOrdList",
                              new CodeExpression[] { }),
                          new CodeDirectionExpression(FieldDirection.Ref,
                              CodeDom.Local("TheRow")),
                          CodeDom.Local("ATransaction"),
                          CodeDom.PropRef(CodeDom.PropRef(CodeDom.Local("UserInfo"), "GUserInfo"),
                              "UserID") }));
                UpdateRow =
                    new CodeConditionStatement(CodeDom.Equals(CodeDom.PropRef(CodeDom.Local("TheRow"),
                                "RowState"),
                            CodeDom.PropRef(CodeDom.Local(
                                    "DataRowState"), "Modified")), new CodeStatement[]
                        { UpdateCall });

                if (table.bCatchUpdateException == true)
                {
                    CatchClauseFailedUpdate = new CodeCatchClause("", new CodeTypeReference("EDBConcurrencyRowDeletedException"));
                    CatchClauseFailedUpdate.Statements.Add(InsertCall);
                    RaiseException = new CodeCatchClause("ex", new CodeTypeReference("Exception"));
                    RaiseException.Statements.Add(CodeDom.Throw(CodeDom.Local("ex")));
                    UpdateRow = new CodeTryCatchFinallyStatement(new CodeStatement[] { UpdateRow },
                        new CodeCatchClause[] { CatchClauseFailedUpdate, RaiseException });
                }

                DeleteCall = CodeDom.Eval(CodeDom.StaticMethodInvoke("TTypedDataAccess", "DeleteRow", new CodeExpression[]
                        { CodeDom._Const(table.strName),
                          CodeDom.StaticMethodInvoke(TypedTableName + "Table",
                              "GetColumnStringList",
                              new CodeExpression[] { }),
                          CodeDom.StaticMethodInvoke(TypedTableName + "Table",
                              "GetPrimKeyColumnOrdList",
                              new CodeExpression[] { }),
                          CodeDom.Local("TheRow"), CodeDom.Local("ATransaction") }));
                DeleteRow =
                    new CodeConditionStatement(CodeDom.Equals(CodeDom.PropRef(CodeDom.Local("TheRow"),
                                "RowState"),
                            CodeDom.PropRef(CodeDom.Local(
                                    "DataRowState"), "Deleted")), new CodeStatement[]
                        { DeleteCall });
            }
            catch (Exception)
            {
                // cannot create an update statement without a primary key
                UpdateRow =
                    new CodeConditionStatement(CodeDom.Equals(CodeDom.PropRef(CodeDom.Local("TheRow"),
                                "RowState"),
                            CodeDom.PropRef(CodeDom.Local(
                                    "DataRowState"), "Modified")), new CodeStatement[]
                        { CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local("AVerificationResult"), "Add",
                                  new CodeExpression[]
                                  { CodeDom._New("TVerificationResult",
                                        new CodeExpression[]
                                        { CodeDom._Const("[DB] NO PRIMARY KEY"),
                                          CodeDom._Const(
                                              "Cannot update record because table " +
                                              TTable.NiceTableName(table.
                                                  strName)
                                              +
                                              " has no primary key."),
                                          CodeDom._Const("Primary Key missing"),
                                          CodeDom._Const(table.strName),
                                          CodeDom.PropRef(CodeDom.Local(
                                                  "TResultSeverity"),
                                              "Resv_Critical"
                                              ) }) })) });
                DeleteRow =
                    new CodeConditionStatement(CodeDom.Equals(CodeDom.PropRef(CodeDom.Local("TheRow"),
                                "RowState"),
                            CodeDom.PropRef(CodeDom.Local(
                                    "DataRowState"), "Deleted")), new CodeStatement[]
                        {
                            CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local("AVerificationResult"), "Add",
                                    new CodeExpression[]
                                    { CodeDom._New("TVerificationResult",
                                          new CodeExpression[]
                                          { CodeDom._Const(
                                                "[DB] NO PRIMARY KEY"),
                                            CodeDom._Const(
                                                "Cannot delete record because table "
                                                +
                                                TTable.NiceTableName(
                                                    table.strName) +
                                                " has no primary key."),
                                            CodeDom._Const(
                                                "Primary Key missing"),
                                            CodeDom._Const(table.strName),
                                            CodeDom.PropRef(CodeDom.Local(
                                                    "TResultSeverity"),
                                                "Resv_Critical"
                                                ) }) }))
                        });
            }
            CatchClause = new CodeCatchClause("ex", new CodeTypeReference("OdbcException"));
            CatchClause.Statements.Add(CodeDom.Let(CodeDom.Local("ResultValue"), CodeDom._Const((System.Object)false)));
            CatchClause.Statements.Add(CodeDom.Let(CodeDom.Local("ExceptionReported"), CodeDom._Const((System.Object)false)));
#if NOTNEEDEDANYMORE_JAVATRIGGEREXCEPTIONS_CONSTRAINTCHECKING
            CounterConstraints = -1000;

            foreach (TTableField tablefield in table.grpTableField.List)
            {
                if (tablefield.bNotNull)
                {
                    CatchClause.Statements.Add(new CodeConditionStatement(CodeDom.Equals(CodeDom.PropRef(CodeDom.IndexerRef(CodeDom.
                                        PropRef(
                                            CodeDom
                                            .
                                            Local(
                                                "ex"),
                                            "Errors"), new CodeExpression[]
                                        { CodeDom.
                                          _Const((
                                                  System
                                                  .
                                                  Object)
                                              0) }),
                                    "NativeError"),
                                CodeDom._Const((System.Object)(
                                        CounterConstraints -
                                        (tablefield.iOrder + 1)))),
                            new CodeStatement[]
                            { CodeDom.Let(CodeDom.Local("ExceptionReported"),
                                  CodeDom._Const((System.Object)true)),
                              CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local(
                                          "AVerificationResult"),
                                      "Add", new CodeExpression[]
                                      { CodeDom._New(
                                            "TVerificationResult",
                                            new CodeExpression
                                            []
                                            { CodeDom.
                                              _Const(
                                                  "[DB] NOT NULL"),
                                              CodeDom.
                                              _Const(
                                                  "Cannot save because field "
                                                  +
                                                  TTable.
                                                  NiceTableName(
                                                      table.
                                                      strName) +
                                                  '.' +
                                                  TTable.
                                                  NiceFieldName(
                                                      tablefield
                                                      .
                                                      strName) +
                                                  " is NULL."),
                                              CodeDom.
                                              _Const(
                                                  "Value has to be set"),
                                              CodeDom.
                                              _Const(table.
                                                  strName +
                                                  '.' +
                                                  tablefield.
                                                  strName),
                                              CodeDom.
                                              PropRef(CodeDom.
                                                  Local(
                                                      "TResultSeverity"),
                                                  "Resv_Critical"
                                                  ) }) })) }));

                    // Throw(CodeDom._New('EDBValueIsNullException',
                    // CodeExpression[].Create(CodeDom._Const('NOT NULL constraint on field '+ tablefield.strName + ' prevents insert'), CodeDom.Local('ex')))))
                }
            }

            CounterConstraints = -2000;

            foreach (TConstraint constraint in table.grpConstraint.List)
            {
                if (constraint.strName.IndexOf("forLoadVia") == -1)
                {
                    if (constraint.strType == "foreignkey")
                    {
                        CatchClause.Statements.Add(new CodeConditionStatement(CodeDom.Equals(CodeDom.PropRef(CodeDom.IndexerRef(
                                            CodeDom.PropRef(
                                                CodeDom.
                                                Local(
                                                    "ex"),
                                                "Errors"),
                                            new CodeExpression[]
                                            {
                                                CodeDom.
                                                _Const((
                                                        System
                                                        .
                                                        Object)
                                                    0)
                                            }),
                                        "NativeError"),
                                    CodeDom._Const((System.Object)(
                                            CounterConstraints))),
                                new CodeStatement[]
                                { CodeDom.Let(CodeDom.Local("ExceptionReported"),
                                      CodeDom._Const((System.Object)true)),
                                  CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local(
                                              "AVerificationResult"),
                                          "Add",
                                          new CodeExpression[]
                                          {
                                              CodeDom._New(
                                                  "TVerificationResult",
                                                  new
                                                  CodeExpression
                                                  []
                                                  {
                                                      CodeDom
                                                      .
                                                      _Const(
                                                          "[DB] Constraint Violation"),
                                                      CodeDom
                                                      .
                                                      _Const(
                                                          "Cannot save because foreign key "
                                                          +
                                                          constraint.strName +
                                                          " is violated."),
                                                      CodeDom
                                                      .
                                                      _Const(
                                                          "Constraint Violation"),
                                                      CodeDom
                                                      .
                                                      _Const(
                                                          constraint
                                                          .
                                                          strName),
                                                      CodeDom
                                                      .
                                                      PropRef(
                                                          CodeDom
                                                          .
                                                          Local("TResultSeverity"), "Resv_Critical"
                                                          )
                                                  })
                                          })) }));

                        // CodeStatementArray.Create(Throw(CodeDom._New('EDBSubmitException',
                        // CodeExpression[].Create(CodeDom._Const('Constraint '+ constraint.strName+ ' prevents insert'), CodeDom.Local('ex')))))
                    }

                    CounterConstraints = CounterConstraints - 1;
                }
            }

            foreach (TConstraint constraint in table.FReferenced)
            {
                if (constraint.strType == "foreignkey")
                {
                    CatchClause.Statements.Add(new CodeConditionStatement(CodeDom.Equals(CodeDom.PropRef(CodeDom.IndexerRef(CodeDom.
                                        PropRef(
                                            CodeDom
                                            .
                                            Local(
                                                "ex"),
                                            "Errors"), new CodeExpression[]
                                        { CodeDom.
                                          _Const((
                                                  System
                                                  .
                                                  Object)
                                              0) }),
                                    "NativeError"),
                                CodeDom._Const((System.Object)(
                                        CounterConstraints))),
                            new CodeStatement[]
                            { CodeDom.Let(CodeDom.Local("ExceptionReported"),
                                  CodeDom._Const((System.Object)true)),
                              CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local(
                                          "AVerificationResult"),
                                      "Add", new CodeExpression[]
                                      {
                                          CodeDom._New(
                                              "TVerificationResult",
                                              new
                                              CodeExpression
                                              []
                                              {
                                                  CodeDom
                                                  .
                                                  _Const(
                                                      "[DB] Constraint Violation"),
                                                  CodeDom
                                                  .
                                                  _Const(
                                                      "Cannot save because foreign key "
                                                      +
                                                      constraint.strName +
                                                      " is violated."),
                                                  CodeDom
                                                  .
                                                  _Const(
                                                      "Constraint Violation"),
                                                  CodeDom
                                                  .
                                                  _Const(
                                                      constraint
                                                      .
                                                      strName),
                                                  CodeDom
                                                  .
                                                  PropRef(
                                                      CodeDom
                                                      .
                                                      Local("TResultSeverity"), "Resv_Critical"
                                                      )
                                              })
                                      })) }));

                    // CodeStatementArray.Create(Throw(CodeDom._New('EDBSubmitException',
                    // CodeExpression[].Create(CodeDom._Const('Constraint '+ constraint.strName+ ' prevents insert'), CodeDom.Local('ex')))))
                }

                CounterConstraints = CounterConstraints - 1;
            }
#endif

            // CatchClause.Statements.Add(Throw(CodeDom.Local('ex')));
            // do something for unexpected odbc exception
            CatchClause.Statements.Add(new CodeConditionStatement(CodeDom.Equals(CodeDom.Local("ExceptionReported"),
                        CodeDom._Const((System.Object)false)), new CodeStatement[]
                    { CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local("AVerificationResult"), "Add",
                              new CodeExpression[]
                              { CodeDom._New("TVerificationResult",
                                    new CodeExpression[]
                                    { CodeDom._Const("[ODBC]"),
                                      CodeDom.PropRef(CodeDom.
                                          IndexerRef(
                                              CodeDom
                                              .
                                              PropRef(
                                                  CodeDom.Local("ex"),
                                                  "Errors"),
                                              new
                                              CodeExpression
                                              []
                                              {
                                                  CodeDom
                                                  .
                                                  _Const((
                                                          System
                                                          .
                                                          Object) 0)
                                              }),
                                          "Message"),
                                      CodeDom._Const(
                                          "ODBC error for table " +
                                          TTable.
                                          NiceTableName(table.
                                              strName)),
                                      CodeDom.MethodInvoke(CodeDom.
                                          PropRef(
                                              CodeDom
                                              .
                                              IndexerRef(
                                                  CodeDom.PropRef(CodeDom.
                                                      Local("ex"),
                                                      "Errors"),
                                                  new
                                                  CodeExpression
                                                  []
                                                  {
                                                      CodeDom
                                                      .
                                                      _Const((
                                                              System
                                                              .
                                                              Object) 0)
                                                  }),
                                              "NativeError"),
                                          "ToString",
                                          new
                                          CodeExpression
                                          [] { }),
                                      CodeDom.PropRef(CodeDom.Local(
                                              "TResultSeverity"),
                                          "Resv_Critical"
                                          ) }) })) }));

            if (UpdateRow == null)
            {
                DealWithRow = new CodeStatement[] {
                    InsertRow
                };
            }
            else
            {
                DealWithRow = new CodeStatement[] {
                    InsertRow, UpdateRow, DeleteRow
                };
            }

            loopContentTable = new CodeStatement[]
            {
                CodeDom.Let(CodeDom.Local("TheRow"), CodeDom.IndexerRef(CodeDom.Local("ATable"), new CodeExpression[]
                        { CodeDom.Local("RowCount") })), new CodeTryCatchFinallyStatement(
                    DealWithRow,
                    new
                    CodeCatchClause[] { CatchClause }
                    )
            };

            // Result.Statements.Add(CodeSnippetStatement.Create('for theRow in ATable do'));
            Result.Statements.Add(new CodeIterationStatement(CodeDom.Let(CodeDom.Local("RowCount"),
                        CodeDom._Const((System.Object) 0)),
                    CodeDom.Inequals(CodeDom.Local("RowCount"),
                        CodeDom.PropRef(CodeDom.PropRef(CodeDom.Local("ATable"),
                                "Rows"),
                            "Count")),
                    CodeDom.Let(CodeDom.Local("RowCount"),
                        new CodeBinaryOperatorExpression(CodeDom.Local("RowCount"),
                            CodeBinaryOperatorType.Add,
                            CodeDom._Const(
                                (
                                    System.Object)(1)))),
                    loopContentTable));

            // countRow := 0
            // countRow <> Table.Rows.Count
            // RowCount := RowCount + 1
            Result.Statements.Add(CodeDom.Return(CodeDom.Local("ResultValue")));
            return Result;
        }

        public static Boolean ValidForeignKeyConstraintForLoadVia(TConstraint AConstraint)
        {
            return (AConstraint.strType == "foreignkey") && (!AConstraint.strThisFields.Contains("s_created_by_c"))
                   && (!AConstraint.strThisFields.Contains("s_modified_by_c")) && (AConstraint.strThisTable != "a_ledger");
        }

        /*
         * This function checks if there is another constraint in this ATable,
         * that references the same other table
         * It will find the field that has a different name, so that names can be unique
         *
         * @return the field that is different in the two keys; empty string if there is no other constraint or no different field
         */

        public static String FindOtherConstraintSameOtherTable(ArrayList AConstraints, TConstraint AConstraint)
        {
            String ReturnValue;

            ReturnValue = "";

            foreach (TConstraint myConstraint in AConstraints)
            {
                if (ValidForeignKeyConstraintForLoadVia(myConstraint) && (myConstraint != AConstraint)
                    && (myConstraint.strOtherTable == AConstraint.strOtherTable))
                {
                    // find the field that is different in AConstraint and myConstraint
                    foreach (string s in AConstraint.strThisFields)
                    {
                        if (!myConstraint.strThisFields.Contains(s))
                        {
                            ReturnValue = s;
                        }
                    }
                }
            }

            return ReturnValue;
        }

        /*
         * This function checks if there is another constraint from the same table
         * that references the current table through a link table
         *
         * @return true if other constraint exists
         */

        public static Boolean FindOtherConstraintSameOtherTable2(ArrayList AConstraints, TConstraint AConstraint)
        {
            Boolean ReturnValue;

            ReturnValue = false;

            foreach (TConstraint myConstraint in AConstraints)
            {
                if ((myConstraint != AConstraint) && (myConstraint.strOtherTable == AConstraint.strOtherTable))
                {
                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }

        public static void AddDifferentNamespace(CodeNamespace ACns, String AOtherTableGroup, String ATableGroup)
        {
            String NamespaceOtherTable;

            if (AOtherTableGroup != ATableGroup)
            {
                if (AOtherTableGroup == "partner")
                {
                    NamespaceOtherTable = "MPartner.Partner";
                }
                else if (AOtherTableGroup == "mailroom")
                {
                    NamespaceOtherTable = "MPartner.Mailroom";
                }
                else if (AOtherTableGroup == "account")
                {
                    NamespaceOtherTable = "MFinance.Account";
                }
                else if (AOtherTableGroup == "gift")
                {
                    NamespaceOtherTable = "MFinance.Gift";
                }
                else if (AOtherTableGroup == "ap")
                {
                    NamespaceOtherTable = "MFinance.AP";
                }
                else if (AOtherTableGroup == "ar")
                {
                    NamespaceOtherTable = "MFinance.AR";
                }
                else if (AOtherTableGroup == "personnel")
                {
                    NamespaceOtherTable = "MPersonnel.Personnel";
                }
                else if (AOtherTableGroup == "units")
                {
                    NamespaceOtherTable = "MPersonnel.Units";
                }
                else if (AOtherTableGroup == "main")
                {
                    NamespaceOtherTable = "MCommon";
                }
                else
                {
                    NamespaceOtherTable = 'M' + AOtherTableGroup;
                }

                ACns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared." +
                        NamespaceOtherTable.Replace("Msysman", "MSysMan").
                        Replace("Mconference", "MConference").
                        Replace("Mhospitality", "MHospitality") + ".Data"));
            }
        }

        public static void GenerateLoadViaDirect(ref CodeTypeDeclaration ATableClass,
            TTable ATable,
            String ATypedTableName,
            TTable AOtherTable,
            TConstraint AConstraint)
        {
            String WhereStmt;
            String DifferentField;
            CodeMemberMethod myCode;
            String ProcedureName;
            CodeExpression SqlQuery;
            String OtherTypedTableName;

            // LoadViaOtherTable
            myCode = new CodeMemberMethod();
            myCode.Comments.Add(new CodeCommentStatement("auto generated", true));
            myCode.Name = "LoadVia" + TTable.NiceTableName(AOtherTable.strName);

            // check if other foreign key exists that references the same table, e.g.
            // PBankAccess.LoadViaPPartnerPartnerKey
            // PBankAccess.LoadViaPPartnerContactPartnerKey
            DifferentField = FindOtherConstraintSameOtherTable(ATable.grpConstraint.List, AConstraint);

            if (DifferentField.Length != 0)
            {
                myCode.Name = myCode.Name + TTable.NiceFieldName(DifferentField);

                // System.Console.WriteLine(TTable.NiceTableName(ATable.strName) + 'Access.' + myCode.Name);
            }

            ProcedureName = myCode.Name;
            myCode.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            myCode.Parameters.Add(CodeDom.Param("DataSet", "ADataSet"));
            AddPrimaryKeyParameters(myCode, AOtherTable);
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "AFieldList"));
            myCode.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "AOrderBy"));
            myCode.Parameters.Add(CodeDom.Param("System.Int32", "AStartRecord"));
            myCode.Parameters.Add(CodeDom.Param("System.Int32", "AMaxRecords"));

            /*
             * PPartnerLocationTableAccess.LoadViaPLocation(ADataSet: DataSet; ASiteKey: Int64; ALocationKey: integer);
             * SELECT *
             * FROM p_partner_location
             * WHERE ASiteKey = ?
             * AND ALocationKey = ?
             */
            GenerateParameterArrayActualValues(ref myCode, AOtherTable, AOtherTable.GetPrimaryKey().strThisFields);
            WhereStmt = " FROM PUB_" + ATable.strName;
            WhereStmt = WhereStmt + GenerateWhereStatement(ATable, AConstraint.strThisFields);
            SqlQuery = new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(CodeDom.GlobalMethodInvoke("GenerateSelectClause",
                        new CodeExpression[] { CodeDom
                                               .Local(
                                                   "AFieldList"),
                                               GetPrimaryKeyFields(
                                                   ATable) }),
                    CodeBinaryOperatorType.Add,
                    CodeDom._Const(
                        WhereStmt)), CodeBinaryOperatorType.Add,
                CodeDom.GlobalMethodInvoke("GenerateOrderByClause",
                    new CodeExpression[] { CodeDom.Local("AOrderBy"
                                               ) }));
            myCode.Statements.Add(CodeDom.Let(CodeDom.Local("ADataSet"),
                    CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "Select", new CodeExpression[]
                        { CodeDom.Local("ADataSet"), SqlQuery,
                          CodeDom.StaticMethodInvoke(ATypedTableName +
                              "Table", "GetTableName",
                              new CodeExpression[] { }),
                          CodeDom.Local("ATransaction"), CodeDom.Local("ParametersArray"),
                          CodeDom.Local("AStartRecord"),
                          CodeDom.Local("AMaxRecords"
                              ) })));
            ATableClass.Members.Add(myCode);

            // Create Overloads
            CreateOverLoads(ref ATableClass, ProcedureName, ATable, AOtherTable, null);
            OtherTypedTableName = TTable.NiceTableName(AOtherTable.strName);

            // Load via template
            myCode = new CodeMemberMethod();
            myCode.Name = ProcedureName + "Template";
            myCode.Comments.Add(new CodeCommentStatement("auto generated", true));
            myCode.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            myCode.Parameters.Add(CodeDom.Param("DataSet", "ADataSet"));
            myCode.Parameters.Add(CodeDom.Param(OtherTypedTableName + "Row", "ATemplateRow"));
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "ATemplateOperators"));
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "AFieldList"));
            myCode.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "AOrderBy"));
            myCode.Parameters.Add(CodeDom.Param("System.Int32", "AStartRecord"));
            myCode.Parameters.Add(CodeDom.Param("System.Int32", "AMaxRecords"));

            /*
             * // e.g. AGiftDetailAccess.LoadViaAGiftTemplate: load all gift details of gifts that are from donor p_donor_key_n
             * SELECT a_gift_detail.*
             * FROM a_gift_detail, a_gift
             * WHERE gift.ledger,batch,giftnumber = giftdetail.ledger,batch,giftnumber // ADirectReference
             * AND a_gift.p_donor_key_n = ?      // depending on values in DataRow
             */
            WhereStmt = " FROM PUB_" + ATable.strName + ", PUB_" + AOtherTable.strName;
            WhereStmt = WhereStmt + GenerateWhereStatementTemplate(AConstraint);
            SqlQuery =
                new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(CodeDom.GlobalMethodInvoke(
                                "GenerateSelectClauseLong",
                                new CodeExpression
                                []
                                {
                                    CodeDom.
                                    _Const(
                                        "PUB_"
                                        +
                                        ATable
                                        .strName), CodeDom.Local("AFieldList"),
                                    GetPrimaryKeyFields(
                                        ATable)
                                }),
                            CodeBinaryOperatorType.
                            Add,
                            CodeDom._Const(WhereStmt)),
                        CodeBinaryOperatorType.Add,
                        CodeDom.GlobalMethodInvoke("GenerateWhereClauseLong",
                            new CodeExpression[]
                            {
                                CodeDom._Const("PUB_" +
                                    AOtherTable
                                    .strName),
                                CodeDom.
                                StaticMethodInvoke(
                                    OtherTypedTableName +
                                    "Table",
                                    "GetColumnStringList",
                                    new CodeExpression[] { }
                                    ),
                                CodeDom
                                .Local("ATemplateRow"),
                                CodeDom.Local(
                                    "ATemplateOperators")
                            })),
                    CodeBinaryOperatorType.Add,
                    CodeDom.GlobalMethodInvoke("GenerateOrderByClause", new CodeExpression[]
                        { CodeDom.Local("AOrderBy") }));
            myCode.Statements.Add(CodeDom.Let(CodeDom.Local("ADataSet"),
                    CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "Select", new CodeExpression[]
                        { CodeDom.Local("ADataSet"), SqlQuery,
                          CodeDom.StaticMethodInvoke(ATypedTableName +
                              "Table", "GetTableName", new CodeExpression[] { }),
                          CodeDom.Local("ATransaction"),
                          CodeDom.GlobalMethodInvoke(
                              "GetParametersForWhereClause",
                              new CodeExpression[] {
                                  CodeDom.Local("ATemplateRow")
                              }),
                          CodeDom.Local("AStartRecord"),
                          CodeDom.Local("AMaxRecords") })));
            ATableClass.Members.Add(myCode);
            CreateOverLoads(ref ATableClass, ProcedureName + "Template", ATable, null, OtherTypedTableName + "Row");
        }

        public static void GenerateCountViaDirect(ref CodeTypeDeclaration ATableClass,
            TTable ATable,
            String ATypedTableName,
            TTable AOtherTable,
            TConstraint AConstraint)
        {
            String WhereStmt;
            String DifferentField;
            CodeMemberMethod myCode;
            String ProcedureName;
            CodeExpression SqlQuery;
            String OtherTypedTableName;

            // CountViaOtherTable
            myCode = new CodeMemberMethod();
            myCode.Comments.Add(new CodeCommentStatement("auto generated", true));
            myCode.Name = "CountVia" + TTable.NiceTableName(AOtherTable.strName);

            // check if other foreign key exists that references the same table, e.g.
            // PBankAccess.CountViaPPartnerPartnerKey
            // PBankAccess.CountViaPPartnerContactPartnerKey
            DifferentField = FindOtherConstraintSameOtherTable(ATable.grpConstraint.List, AConstraint);

            if (DifferentField.Length != 0)
            {
                myCode.Name = myCode.Name + TTable.NiceFieldName(DifferentField);

                // System.Console.WriteLine(TTable.NiceTableName(ATable.strName) + 'Access.' + myCode.Name);
            }

            ProcedureName = myCode.Name;
            myCode.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            myCode.ReturnType = CodeDom.TypeRef("System.Int32");
            AddPrimaryKeyParameters(myCode, AOtherTable);
            myCode.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));

            /*
             * PPartnerLocationTableAccess.CountViaPLocation(ASiteKey: Int64; ALocationKey: integer);
             * SELECT COUNT(*)
             * FROM p_partner_location
             * WHERE ASiteKey = ?
             * AND ALocationKey = ?
             */
            GenerateParameterArrayActualValues(ref myCode, AOtherTable, AOtherTable.GetPrimaryKey().strThisFields);
            WhereStmt = "SELECT COUNT(*) FROM PUB_" + ATable.strName;
            WhereStmt = WhereStmt + GenerateWhereStatement(ATable, AConstraint.strThisFields);
            SqlQuery = CodeDom._Const(WhereStmt);
            myCode.Statements.Add(CodeDom.Return(
                    CodeDom.MethodInvoke(CodeDom.Local("Convert"), "ToInt32",
                        new CodeExpression[] {
                            CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "ExecuteScalar",
                                new CodeExpression[]
                                { SqlQuery, CodeDom.Local("ATransaction"),
                                  CodeDom._Const((System.Object)false),
                                  CodeDom.Local("ParametersArray") })
                        })));
            ATableClass.Members.Add(myCode);

            // Create Overloads
            CreateCountOverLoads(ref ATableClass, ProcedureName, ATable, AOtherTable, null);
            OtherTypedTableName = TTable.NiceTableName(AOtherTable.strName);

            // Count via template
            myCode = new CodeMemberMethod();
            myCode.Name = ProcedureName + "Template";
            myCode.Comments.Add(new CodeCommentStatement("auto generated", true));
            myCode.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            myCode.ReturnType = CodeDom.TypeRef("System.Int32");
            myCode.Parameters.Add(CodeDom.Param(OtherTypedTableName + "Row", "ATemplateRow"));
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "ATemplateOperators"));
            myCode.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));

            /*
             * // e.g. AGiftDetailAccess.CountViaAGiftTemplate: load all gift details of gifts that are from donor p_donor_key_n
             * SELECT COUNT(*)
             * FROM a_gift_detail, a_gift
             * WHERE gift.ledger,batch,giftnumber = giftdetail.ledger,batch,giftnumber // ADirectReference
             * AND a_gift.p_donor_key_n = ?      // depending on values in DataRow
             */
            WhereStmt = "SELECT COUNT(*) FROM PUB_" + ATable.strName + ", PUB_" + AOtherTable.strName;
            WhereStmt = WhereStmt + GenerateWhereStatementTemplate(AConstraint);
            SqlQuery =
                new CodeBinaryOperatorExpression(CodeDom._Const(WhereStmt), CodeBinaryOperatorType.Add,
                    CodeDom.GlobalMethodInvoke("GenerateWhereClauseLong", new CodeExpression[]
                        {
                            CodeDom._Const("PUB_" + AOtherTable.strName),
                            CodeDom.StaticMethodInvoke(OtherTypedTableName + "Table",
                                "GetColumnStringList",
                                new CodeExpression[] { }),
                            CodeDom.Local("ATemplateRow"),
                            CodeDom.Local("ATemplateOperators"
                                )
                        }));
            myCode.Statements.Add(CodeDom.Return(
                    CodeDom.MethodInvoke(CodeDom.Local("Convert"), "ToInt32",
                        new CodeExpression[] {
                            CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "ExecuteScalar",
                                new CodeExpression[]
                                { SqlQuery, CodeDom.Local("ATransaction"),
                                  CodeDom._Const((System.Object)false),
                                  CodeDom.GlobalMethodInvoke("GetParametersForWhereClause",
                                      new CodeExpression[]
                                      {
                                          CodeDom
                                          .Local("ATemplateRow"),
                                          CodeDom
                                          .StaticMethodInvoke(
                                              OtherTypedTableName +
                                              "Table",
                                              "GetPrimKeyColumnOrdList",
                                              new CodeExpression[] { }
                                              )
                                      }
                                      ) })
                        })));
            ATableClass.Members.Add(myCode);
            CreateCountOverLoads(ref ATableClass, ProcedureName + "Template", ATable, null, OtherTypedTableName + "Row");
        }

        public static void GenerateLoadViaLinkTable(ref CodeTypeDeclaration ATableClass,
            TTable ATable,
            TTable ALinkTable,
            String ATypedTableName,
            String AProcedureName,
            TTable AOtherTable,
            TConstraint AOtherConstraint,
            TConstraint ADirectReference)
        {
            String WhereStmt;
            CodeExpression SqlQuery;
            CodeMemberMethod myCode;
            String OtherTypedTableName;

            // LoadViaOtherTable
            myCode = new CodeMemberMethod();
            myCode.Name = AProcedureName;
            myCode.Comments.Add(new CodeCommentStatement("auto generated LoadViaLinkTable", true));
            myCode.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            myCode.Parameters.Add(CodeDom.Param("DataSet", "ADataSet"));
            AddPrimaryKeyParameters(myCode, AOtherTable);
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "AFieldList"));
            myCode.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "AOrderBy"));
            myCode.Parameters.Add(CodeDom.Param("System.Int32", "AStartRecord"));
            myCode.Parameters.Add(CodeDom.Param("System.Int32", "AMaxRecords"));

            /*
             * PPartnerTableAccess.LoadViaPLocation(ADataSet: DataSet; ASiteKey: Int64; ALocationKey: integer);
             * SELECT p_partner.*
             * FROM p_partner_location, p_partner
             * WHERE p_partner.p_partner_key_n = p_partner_location.p_partner_key_n // ADirectReference
             * AND p_partner_location.p_site_key_n = ?      // AOtherConstraint
             * AND p_partner_location.p_location_key_i = ?  // AOtherConstraint
             */
            OtherTypedTableName = TTable.NiceTableName(AOtherTable.strName);
            GenerateParameterArrayActualValues(ref myCode, AOtherTable, AOtherTable.GetPrimaryKey().strThisFields);
            WhereStmt = " FROM PUB_" + ATable.strName + ", PUB_" + ALinkTable.strName;
            WhereStmt = WhereStmt + GenerateWhereStatement(ALinkTable, AOtherConstraint, ADirectReference);
            SqlQuery =
                new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(CodeDom.GlobalMethodInvoke("GenerateSelectClauseLong",
                            new CodeExpression[]
                            { CodeDom._Const("PUB_" +
                                  ATable.strName),
                              CodeDom.Local("AFieldList"),
                              GetPrimaryKeyFields(ATable
                                  ) }),
                        CodeBinaryOperatorType.Add,
                        CodeDom._Const(WhereStmt)), CodeBinaryOperatorType.Add,
                    CodeDom.GlobalMethodInvoke("GenerateOrderByClause", new CodeExpression[]
                        {
                            CodeDom.Local("AOrderBy")
                        }));
            myCode.Statements.Add(CodeDom.Let(CodeDom.Local("ADataSet"),
                    CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "Select", new CodeExpression[]
                        { CodeDom.Local("ADataSet"), SqlQuery,
                          CodeDom.StaticMethodInvoke(ATypedTableName +
                              "Table", "GetTableName",
                              new CodeExpression[] { }),
                          CodeDom.Local("ATransaction"), CodeDom.Local("ParametersArray"),
                          CodeDom.Local("AStartRecord"),
                          CodeDom.Local("AMaxRecords"
                              ) })));
            ATableClass.Members.Add(myCode);

            // Create Overloads
            CreateOverLoads(ref ATableClass, AProcedureName, ATable, AOtherTable, null);
            OtherTypedTableName = TTable.NiceTableName(AOtherTable.strName);
            myCode = new CodeMemberMethod();
            myCode.Name = AProcedureName + "Template";
            myCode.Comments.Add(new CodeCommentStatement("auto generated", true));
            myCode.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            myCode.Parameters.Add(CodeDom.Param("DataSet", "ADataSet"));
            myCode.Parameters.Add(CodeDom.Param(OtherTypedTableName + "Row", "ATemplateRow"));
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "ATemplateOperators"));
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "AFieldList"));
            myCode.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "AOrderBy"));
            myCode.Parameters.Add(CodeDom.Param("System.Int32", "AStartRecord"));
            myCode.Parameters.Add(CodeDom.Param("System.Int32", "AMaxRecords"));

            /*
             * // e.g. PPartnerAccess.LoadViaPLocationTemplate: load all partners that live in a specific road
             * SELECT p_partner.*
             * FROM p_partner_location, p_partner, p_location
             * WHERE p_partner.p_partner_key_n = p_partner_location.p_partner_key_n // ADirectReference
             * AND p_partner_location.p_location_key_i = p_location.p_location_key_i // AOtherConstraint
             * AND p_location.p_street_name_c = ?      // depending on values in DataRow
             * AND p_location.??? = ?  // depending on values in DataRow
             */
            WhereStmt = " FROM PUB_" + ATable.strName + ", PUB_" + ALinkTable.strName + ", PUB_" + AOtherTable.strName;
            WhereStmt = WhereStmt + GenerateWhereStatementTemplate(ALinkTable, AOtherConstraint, ADirectReference);
            SqlQuery =
                new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(CodeDom.GlobalMethodInvoke(
                                "GenerateSelectClauseLong",
                                new CodeExpression
                                []
                                {
                                    CodeDom.
                                    _Const(
                                        "PUB_"
                                        +
                                        ATable
                                        .strName), CodeDom.Local("AFieldList"),
                                    GetPrimaryKeyFields(
                                        ATable)
                                }),
                            CodeBinaryOperatorType.Add,
                            CodeDom._Const(WhereStmt)
                            ),
                        CodeBinaryOperatorType.Add,
                        CodeDom.GlobalMethodInvoke("GenerateWhereClauseLong",
                            new CodeExpression[]
                            {
                                CodeDom
                                ._Const("PUB_" +
                                    AOtherTable.
                                    strName),
                                CodeDom
                                .StaticMethodInvoke(
                                    OtherTypedTableName +
                                    "Table",
                                    "GetColumnStringList",
                                    new
                                    CodeExpression
                                    [] { }),
                                CodeDom.Local(
                                    "ATemplateRow"),
                                CodeDom.Local("ATemplateOperators"
                                    )
                            })),
                    CodeBinaryOperatorType.Add,
                    CodeDom.GlobalMethodInvoke("GenerateOrderByClause", new CodeExpression[]
                        {
                            CodeDom.Local("AOrderBy")
                        }));
            myCode.Statements.Add(CodeDom.Let(CodeDom.Local("ADataSet"),
                    CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "Select", new CodeExpression[]
                        { CodeDom.Local("ADataSet"), SqlQuery,
                          CodeDom.StaticMethodInvoke(ATypedTableName +
                              "Table", "GetTableName",
                              new CodeExpression[] { }),
                          CodeDom.Local("ATransaction"),
                          CodeDom.GlobalMethodInvoke(
                              "GetParametersForWhereClause", new CodeExpression[]
                              {
                                  CodeDom.Local(
                                      "ATemplateRow")
                              }), CodeDom.Local("AStartRecord"),
                          CodeDom.Local("AMaxRecords") })));
            ATableClass.Members.Add(myCode);
            CreateOverLoads(ref ATableClass, AProcedureName + "Template", ATable, null, OtherTypedTableName + "Row");
        }

        public static void GenerateCountViaLinkTable(ref CodeTypeDeclaration ATableClass,
            TTable ATable,
            TTable ALinkTable,
            String ATypedTableName,
            String AProcedureName,
            TTable AOtherTable,
            TConstraint AOtherConstraint,
            TConstraint ADirectReference)
        {
            String WhereStmt;
            CodeExpression SqlQuery;
            CodeMemberMethod myCode;
            String OtherTypedTableName;

            // CountViaOtherTable
            myCode = new CodeMemberMethod();
            myCode.Name = AProcedureName;
            myCode.Comments.Add(new CodeCommentStatement("auto generated CountViaLinkTable", true));
            myCode.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            myCode.ReturnType = CodeDom.TypeRef("System.Int32");
            AddPrimaryKeyParameters(myCode, AOtherTable);
            myCode.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));

            /*
             * PPartnerTableAccess.CountViaPLocation(ADataSet: DataSet; ASiteKey: Int64; ALocationKey: integer);
             * SELECT COUNT(*)
             * FROM p_partner_location, p_partner
             * WHERE p_partner.p_partner_key_n = p_partner_location.p_partner_key_n // ADirectReference
             * AND p_partner_location.p_site_key_n = ?      // AOtherConstraint
             * AND p_partner_location.p_location_key_i = ?  // AOtherConstraint
             */
            OtherTypedTableName = TTable.NiceTableName(AOtherTable.strName);
            GenerateParameterArrayActualValues(ref myCode, AOtherTable, AOtherTable.GetPrimaryKey().strThisFields);
            WhereStmt = "SELECT COUNT(*) FROM PUB_" + ATable.strName + ", PUB_" + ALinkTable.strName;
            WhereStmt = WhereStmt + GenerateWhereStatement(ALinkTable, AOtherConstraint, ADirectReference);
            SqlQuery = CodeDom._Const(WhereStmt);
            myCode.Statements.Add(CodeDom.Return(
                    CodeDom.MethodInvoke(CodeDom.Local("Convert"), "ToInt32",
                        new CodeExpression[] {
                            CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "ExecuteScalar",
                                new CodeExpression[]
                                { SqlQuery, CodeDom.Local("ATransaction"),
                                  CodeDom._Const((System.Object)false),
                                  CodeDom.Local("ParametersArray"
                                      ) })
                        })));
            ATableClass.Members.Add(myCode);

            // Create Overloads
            CreateCountOverLoads(ref ATableClass, AProcedureName, ATable, AOtherTable, null);
            OtherTypedTableName = TTable.NiceTableName(AOtherTable.strName);
            myCode = new CodeMemberMethod();
            myCode.Name = AProcedureName + "Template";
            myCode.Comments.Add(new CodeCommentStatement("auto generated", true));
            myCode.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            myCode.ReturnType = CodeDom.TypeRef("System.Int32");
            myCode.Parameters.Add(CodeDom.Param(OtherTypedTableName + "Row", "ATemplateRow"));
            myCode.Parameters.Add(CodeDom.Param("StringCollection", "ATemplateOperators"));
            myCode.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));

            /*
             * // e.g. PPartnerAccess.CountViaPLocationTemplate: count all partners that live in a specific road
             * SELECT COUNT(*)
             * FROM p_partner_location, p_partner, p_location
             * WHERE p_partner.p_partner_key_n = p_partner_location.p_partner_key_n // ADirectReference
             * AND p_partner_location.p_location_key_i = p_location.p_location_key_i // AOtherConstraint
             * AND p_location.p_street_name_c = ?      // depending on values in DataRow
             * AND p_location.??? = ?  // depending on values in DataRow
             */
            WhereStmt = "SELECT COUNT(*) FROM PUB_" + ATable.strName + ", PUB_" + ALinkTable.strName + ", PUB_" + AOtherTable.strName;
            WhereStmt = WhereStmt + GenerateWhereStatementTemplate(ALinkTable, AOtherConstraint, ADirectReference);
            SqlQuery =
                new CodeBinaryOperatorExpression(CodeDom._Const(WhereStmt), CodeBinaryOperatorType.Add,
                    CodeDom.GlobalMethodInvoke("GenerateWhereClauseLong", new CodeExpression[]
                        {
                            CodeDom._Const("PUB_" + AOtherTable.strName),
                            CodeDom.StaticMethodInvoke(OtherTypedTableName + "Table",
                                "GetColumnStringList",
                                new CodeExpression[] { }),
                            CodeDom.Local("ATemplateRow"),
                            CodeDom.Local("ATemplateOperators"
                                )
                        }));
            myCode.Statements.Add(CodeDom.Return(
                    CodeDom.MethodInvoke(CodeDom.Local("Convert"), "ToInt32",
                        new CodeExpression[] {
                            CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "ExecuteScalar",
                                new CodeExpression[]
                                { SqlQuery, CodeDom.Local("ATransaction"),
                                  CodeDom._Const((System.Object)false),
                                  CodeDom.GlobalMethodInvoke("GetParametersForWhereClause",
                                      new CodeExpression[]
                                      {
                                          CodeDom
                                          .Local("ATemplateRow"),
                                          CodeDom
                                          .StaticMethodInvoke(
                                              OtherTypedTableName +
                                              "Table",
                                              "GetPrimKeyColumnOrdList",
                                              new CodeExpression[] { }
                                              )
                                      }) })
                        })));
            ATableClass.Members.Add(myCode);
            CreateCountOverLoads(ref ATableClass, AProcedureName + "Template", ATable, null, OtherTypedTableName + "Row");
        }

        public static void AddLoadViaOtherTable(CodeNamespace ACns,
            TDataDefinitionStore AStore,
            TTable ATable,
            string ATypedTableName,
            string ATableName,
            ref CodeTypeDeclaration ATableClass)
        {
            TConstraint NewConstraint;
            TConstraint PrimaryKey;
            TConstraint LinkPrimaryKey;
            TTable LinkTable;
            TTable OtherTable;
            String ProcedureName;
            Boolean IsLinkTable;
            ArrayList OtherLinkConstraints;
            ArrayList References;
            ArrayList DirectReferences;

            System.Int32 ConstraintCounter;
            System.Int32 NumberOfConstraints;
            System.Int32 Count;

            // LoadViaOtherTablePrimaryKey
            // example: we want to get a list of all a_ap_document_details, via the primary key of table a_ap_document
            // the current table is a_ap_document_details
            // via table is a_ap_document
            // the foreign key this relation is based on is a_ap_document_detail_fk2: a_ledger_number_i, a_ap_number_i => a_ap_document
            // general solution: pick up all foreign keys from this table to other tables
            // I thought: that are part of the primary key of this table
            // but this would exclude a_ap_document.p_partner_key_n=>supplier, because it is not part of the primary key
            try
            {
                PrimaryKey = ATable.GetPrimaryKey();
            }
            catch (Exception)
            {
                // Console.WriteLine(e.Message);
                return;
            }
            DirectReferences = new ArrayList();
            NumberOfConstraints = ATable.grpConstraint.List.Count;

            for (ConstraintCounter = 0; ConstraintCounter <= NumberOfConstraints - 1; ConstraintCounter += 1)
            {
                TConstraint myConstraint = (TConstraint)ATable.grpConstraint.List[ConstraintCounter];

                if (ValidForeignKeyConstraintForLoadVia(myConstraint))
                {
                    OtherTable = AStore.GetTable(myConstraint.strOtherTable);

                    if ((myConstraint.strName == "a_account_hierarchy_fk1") || (myConstraint.strName == "a_motivation_detail_fk1"))
                    {
                        // AccountHierarchy: there is no constraint that only references Ledger, but only a constraint that references ledger and account
                        // but because the key in Ledger is already the primary key, a LoadViaLedger is required.
                        foreach (string field in myConstraint.strOtherFields)
                        {
                            // get a constraint that is only based on that field
                            TConstraint OtherLinkConstraint = OtherTable.GetConstraint(StringHelper.StrSplit(field, ","));

                            if ((OtherLinkConstraint != null) && ValidForeignKeyConstraintForLoadVia(OtherLinkConstraint))
                            {
                                NewConstraint = new TConstraint();
                                NewConstraint.strName = OtherLinkConstraint.strName + "forLoadVia";
                                NewConstraint.strType = "foreignkey";
                                NewConstraint.strThisTable = myConstraint.strThisTable;
                                NewConstraint.strThisFields =
                                    StringHelper.StrSplit(myConstraint.strThisFields[myConstraint.strOtherFields.IndexOf(
                                                                                         field)], ",");
                                NewConstraint.strOtherTable = OtherLinkConstraint.strOtherTable;
                                NewConstraint.strOtherFields = OtherLinkConstraint.strOtherFields;
                                ATable.grpConstraint.List.Add(NewConstraint);
                            }
                        }
                    }
                }
            }

            foreach (TConstraint myConstraint in ATable.grpConstraint.List)
            {
                if (ValidForeignKeyConstraintForLoadVia(myConstraint))
                {
                    OtherTable = AStore.GetTable(myConstraint.strOtherTable);
                    AddDifferentNamespace(ACns, OtherTable.strGroup, ATable.strGroup);
                    GenerateLoadViaDirect(ref ATableClass, ATable, ATypedTableName, OtherTable, myConstraint);
                    GenerateCountViaDirect(ref ATableClass, ATable, ATypedTableName, OtherTable, myConstraint);
                    DirectReferences.Add(myConstraint);
                }
            }

            // situation 2: bridging tables
            // for example p_location and p_partner are connected through p_partner_location
            // it would be helpful, to be able to do:
            // location.LoadViaPartner(partnerkey) to get all locations of the partner
            // partner.loadvialocation(locationkey) to get all partners living at that location
            // general solution: pick up all foreign keys from other tables (B) to the primary key of the current table (A),
            // where the other table has a foreign key to another table (C), using other fields in the primary key of (B) than the link to (A).
            // get all tables that reference the current table
            // step 1: collect all the valid constraints of such link tables
            OtherLinkConstraints = new ArrayList();
            References = new ArrayList();

            foreach (TConstraint Reference in ATable.GetReferences())
            {
                LinkTable = AStore.GetTable(Reference.strThisTable);
                try
                {
                    LinkPrimaryKey = LinkTable.GetPrimaryKey();

                    if (StringHelper.Contains(LinkPrimaryKey.strThisFields, Reference.strThisFields))
                    {
                        // check how many constraints from the link table are from fields in the primary key
                        // a link table only should have 2
                        // so find the other one
                        TConstraint OtherLinkConstraint = null;
                        IsLinkTable = false;

                        foreach (TConstraint LinkConstraint in LinkTable.grpConstraint.List)
                        {
                            // check if there is another constraint for the primary key of the link table.
                            if (ValidForeignKeyConstraintForLoadVia(LinkConstraint) && (LinkConstraint != Reference)
                                && (StringHelper.Contains(LinkPrimaryKey.strThisFields, LinkConstraint.strThisFields)))
                            {
                                if (OtherLinkConstraint == null)
                                {
                                    OtherLinkConstraint = LinkConstraint;
                                    IsLinkTable = true;
                                }
                                else
                                {
                                    IsLinkTable = false;
                                }
                            }
                            else if (ValidForeignKeyConstraintForLoadVia(LinkConstraint) && (LinkConstraint != Reference)
                                     && (!StringHelper.Contains(LinkPrimaryKey.strThisFields, LinkConstraint.strThisFields))
                                     && StringHelper.ContainsSome(LinkPrimaryKey.strThisFields, LinkConstraint.strThisFields))
                            {
                                // if there is a key that partly is in the primary key, then this is not a link table.
                                OtherLinkConstraint = LinkConstraint;
                                IsLinkTable = false;
                            }
                        }

                        if ((IsLinkTable) && (OtherLinkConstraint.strOtherTable != Reference.strOtherTable))
                        {
                            // collect the links. then we are able to name them correctly, once we need them
                            OtherLinkConstraints.Add(OtherLinkConstraint);
                            References.Add(Reference);
                        }
                    }
                }
                catch (Exception)
                {
                    // Console.WriteLine(e.Message);
                }
            }

            // step2: implement the link tables, using correct names for the procedures
            Count = 0;

            foreach (TConstraint OtherLinkConstraint in OtherLinkConstraints)
            {
                OtherTable = AStore.GetTable(OtherLinkConstraint.strOtherTable);
                ProcedureName = "Via" + TTable.NiceTableName(OtherTable.strName);

                // check if other foreign key exists that references the same table, e.g.
                // PPartnerAccess.LoadViaSUserPRecentPartners
                // PPartnerAccess.LoadViaSUserPCustomisedGreeting
                // DirectReferences necessary for PPersonAccess.LoadViaPUnit (p_om_field_key_n) and PPersonAccess.LoadViaPUnitPmGeneralApplication
                if (FindOtherConstraintSameOtherTable2(OtherLinkConstraints,
                        OtherLinkConstraint)
                    || FindOtherConstraintSameOtherTable2(DirectReferences, OtherLinkConstraint))
                {
                    ProcedureName = ProcedureName + TTable.NiceTableName(OtherLinkConstraint.strThisTable);

                    // System.Console.WriteLine(TTable.NiceTableName(ATable.strName) + 'Access.' + ProcedureName);
                }

                AddDifferentNamespace(ACns, OtherTable.strGroup, ATable.strGroup);
                GenerateLoadViaLinkTable(ref ATableClass, ATable, AStore.GetTable(
                        ((TConstraint)References[Count]).strThisTable), ATypedTableName, "Load" + ProcedureName,
                    OtherTable,
                    OtherLinkConstraint, (TConstraint)References[Count]);
                GenerateCountViaLinkTable(ref ATableClass, ATable, AStore.GetTable(
                        ((TConstraint)References[Count]).strThisTable), ATypedTableName, "Count" + ProcedureName,
                    OtherTable,
                    OtherLinkConstraint, (TConstraint)References[Count]);
                Count = Count + 1;
            }
        }

        public static void AddDeleteTable(CodeNamespace ACns,
            TDataDefinitionStore AStore,
            TTable ATable,
            string ATypedTableName,
            string ATableName,
            ref CodeTypeDeclaration ATableClass)
        {
            CodeMemberMethod myCode;
            TConstraint PrimaryKeyConstraint;
            String DeleteStmt;

            if (ATable.HasPrimaryKey())
            {
                myCode = new CodeMemberMethod();
                myCode.Comments.Add(new CodeCommentStatement("auto generated", true));
                myCode.Name = "DeleteByPrimaryKey";
                myCode.Attributes = MemberAttributes.Static | MemberAttributes.Public;
                AddPrimaryKeyParameters(myCode, ATable);
                myCode.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
                PrimaryKeyConstraint = ATable.GetPrimaryKey();
                GenerateParameterArrayActualValues(ref myCode, ATable, PrimaryKeyConstraint.strThisFields);
                DeleteStmt = "DELETE FROM PUB_" + ATable.strName;
                DeleteStmt = DeleteStmt + GenerateWhereStatement(ATable, PrimaryKeyConstraint.strThisFields);
                myCode.Statements.Add(CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "ExecuteNonQuery",
                            new CodeExpression[]
                            { CodeDom._Const(DeleteStmt), CodeDom.Local("ATransaction"),
                              CodeDom._Const((System.Object)false),
                              CodeDom.Local("ParametersArray") })));

                // ACommitTransaction
                ATableClass.Members.Add(myCode);

                // removed overload without transaction: we don't want that anymore
                // DeleteUsingTemplate
                myCode = new CodeMemberMethod();
                myCode.Comments.Add(new CodeCommentStatement("auto generated", true));
                myCode.Name = "DeleteUsingTemplate";
                myCode.Attributes = MemberAttributes.Static | MemberAttributes.Public;
                myCode.Parameters.Add(CodeDom.Param(ATypedTableName + "Row", "ATemplateRow"));
                myCode.Parameters.Add(CodeDom.Param("StringCollection", "ATemplateOperators"));
                myCode.Parameters.Add(CodeDom.Param("TDBTransaction", "ATransaction"));
                myCode.Statements.Add(CodeDom.Eval(CodeDom.MethodInvoke(CodeDom.Local("DBAccess.GDBAccessObj"), "ExecuteNonQuery",
                            new CodeExpression[]
                            { new CodeBinaryOperatorExpression(CodeDom._Const("DELETE FROM PUB_" +
                                      ATable.strName),
                                  CodeBinaryOperatorType.Add,
                                  CodeDom.GlobalMethodInvoke(
                                      "GenerateWhereClause",
                                      new CodeExpression[]
                                      {
                                          CodeDom
                                          .StaticMethodInvoke(
                                              ATypedTableName +
                                              "Table",
                                              "GetColumnStringList",
                                              new
                                              CodeExpression
                                              [] { }),
                                          CodeDom.Local(
                                              "ATemplateRow"),
                                          CodeDom
                                          .Local(
                                              "ATemplateOperators")
                                      })),
                              CodeDom.Local("ATransaction"),
                              CodeDom._Const((System.Object)false),
                              CodeDom.GlobalMethodInvoke("GetParametersForWhereClause",
                                  new CodeExpression[]
                                  { CodeDom.Local("ATemplateRow") }) })));

                // ACommitTransaction
                ATableClass.Members.Add(myCode);

                // removed overload without transaction: we don't want that anymore
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
            CodeMemberField constant;

            Result = new CodeTypeDeclaration(typedTableName + "Access");
            Result.IsClass = true;
            Result.BaseTypes.Add("TTypedDataAccess");
            Result.Comments.Add(new CodeCommentStatement(table.strDescription, true));

            // const DATATABLENAME = 'Partner'; DBTABLENAME = 'p_partner';
            constant = new CodeMemberField("System.String", "DATATABLENAME");
            constant.Comments.Add(new CodeCommentStatement("CamelCase version of table name", true));
            constant.InitExpression = new CodePrimitiveExpression(typedTableName);
            constant.Attributes = MemberAttributes.Const | MemberAttributes.Public;
            Result.Members.Add(constant);
            constant = new CodeMemberField("System.String", "DBTABLENAME");
            constant.Comments.Add(new CodeCommentStatement("original table name in database", true));
            constant.InitExpression = new CodePrimitiveExpression(table.strName);
            constant.Attributes = MemberAttributes.Const | MemberAttributes.Public;
            Result.Members.Add(constant);
            Result.Members.Add(CreateLoadAllLongCode(table, typedTableName));
            CreateOverLoads(ref Result, "LoadAll", table, null, null);
            Result.Members.Add(CreateLoadByPrimaryKeyLongCode(table, typedTableName));
            CreateOverLoads(ref Result, "LoadByPrimaryKey", table, table, null);
            Result.Members.Add(CreateLoadUsingTemplateLongCode(table, typedTableName));
            CreateOverLoads(ref Result, "LoadUsingTemplate", table, null, typedTableName + "Row");
            Result.Members.Add(CreateCountAllLongCode(table, typedTableName));
            CreateCountOverLoads(ref Result, "CountAll", table, null, null);
            Result.Members.Add(CreateCountByPrimaryKeyLongCode(table, typedTableName));
            CreateCountOverLoads(ref Result, "CountByPrimaryKey", table, table, null);
            Result.Members.Add(CreateCountUsingTemplateLongCode(table, typedTableName));
            CreateCountOverLoads(ref Result, "CountUsingTemplate", table, null, typedTableName + "Row");
            AddLoadViaOtherTable(ACns, AStore, table, typedTableName, tableName, ref Result);
            AddDeleteTable(ACns, AStore, table, typedTableName, tableName, ref Result);
            Result.Members.Add(CreateSubmitChangesCode(table, typedTableName));
            return Result;
        }

        public static Boolean WriteTypedDataAccess(TDataDefinitionStore store,
            string strGroup,
            string AFilePath,
            string ANamespaceName,
            string AFilename)
        {
            FileStream outFile;
            String OutFileName;
            TextWriter tw;
            string tableName;
            CodeNamespace cns;

            Console.WriteLine("processing namespace PetraTypedDataAccess." + strGroup.Substring(0, 1).ToUpper() + strGroup.Substring(1));
            OutFileName = AFilePath + AFilename + ".cs";
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
            cns.Imports.Add(new CodeNamespaceImport("Ict.Petra.Shared"));
            cns.Imports.Add(new CodeNamespaceImport(ANamespaceName.Replace(".Access", "")));

            foreach (TTable currentTable in store.GetTables())
            {
                if (currentTable.strGroup == strGroup)
                {
                    tableName = TTable.NiceTableName(currentTable.strName) + "Table";
                    cns.Types.Add(Access(cns, store, currentTable, TTable.NiceTableName(currentTable.strName), tableName));
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

            return true;
        }
    }
}