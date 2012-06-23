//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Collections;
using System.Collections.Specialized;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Tools.CodeGeneration.DataStore
{
    /// This will generate the cascading parts of the datastore;
    /// it references right across all table groups, therefore a single file is created.
    public class CodeGenerationCascading
    {
        /// to avoid huge cascading deletes, which we will probably not allow anyways (e.g. s_user)
        public const Int32 CASCADING_DELETE_MAX_REFERENCES = 9;

        private static void PrepareCodeletsPrimaryKey(
            TTable ACurrentTable,
            out string csvListPrimaryKeyFields,
            out string formalParametersPrimaryKey,
            out string actualParametersPrimaryKey)
        {
            csvListPrimaryKeyFields = "";
            formalParametersPrimaryKey = "";
            actualParametersPrimaryKey = "";
            int counterPrimaryKeyField = 0;

            if (!ACurrentTable.HasPrimaryKey())
            {
                return;
            }

            foreach (string field in ACurrentTable.GetPrimaryKey().strThisFields)
            {
                if (counterPrimaryKeyField > 0)
                {
                    csvListPrimaryKeyFields += ",";
                    formalParametersPrimaryKey += ", ";
                    actualParametersPrimaryKey += ", ";
                }

                TTableField typedField = ACurrentTable.GetField(field);

                csvListPrimaryKeyFields += field;
                formalParametersPrimaryKey += typedField.GetDotNetType() + " A" + TTable.NiceFieldName(field);
                actualParametersPrimaryKey += "A" + TTable.NiceFieldName(field);

                counterPrimaryKeyField++;
            }
        }

        /// <summary>
        /// code for cascading functions
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="ACurrentTable"></param>
        /// <param name="ATemplate"></param>
        /// <param name="ASnippet"></param>
        /// <returns>false if no cascading available</returns>
        private static bool InsertMainProcedures(TDataDefinitionStore AStore,
            TTable ACurrentTable,
            ProcessTemplate ATemplate,
            ProcessTemplate ASnippet)
        {
            // for the moment, don't implement it for too big tables, e.g. s_user)
            if (!ACurrentTable.HasPrimaryKey() || (ACurrentTable.FReferenced.Count > CASCADING_DELETE_MAX_REFERENCES))
            {
                return false;
            }

            ASnippet.AddToCodelet("TABLENAME", TTable.NiceTableName(ACurrentTable.strName));

            string csvListPrimaryKeyFields;
            string formalParametersPrimaryKey;
            string actualParametersPrimaryKey;

            PrepareCodeletsPrimaryKey(ACurrentTable,
                out csvListPrimaryKeyFields,
                out formalParametersPrimaryKey,
                out actualParametersPrimaryKey);

            ASnippet.AddToCodelet("CSVLISTPRIMARYKEYFIELDS", csvListPrimaryKeyFields);
            ASnippet.AddToCodelet("FORMALPARAMETERSPRIMARYKEY", formalParametersPrimaryKey);
            ASnippet.AddToCodelet("ACTUALPARAMETERSPRIMARYKEY", actualParametersPrimaryKey);

            foreach (TConstraint constraint in ACurrentTable.FReferenced)
            {
                if (AStore.GetTable(constraint.strThisTable).HasPrimaryKey())
                {
                    string csvListOtherPrimaryKeyFields;
                    string notUsed;
                    TTable OtherTable = AStore.GetTable(constraint.strThisTable);
                    PrepareCodeletsPrimaryKey(OtherTable,
                        out csvListOtherPrimaryKeyFields,
                        out notUsed,
                        out notUsed);

                    // check if other foreign key exists that references the same table, e.g.
                    // PBankAccess.LoadViaPPartnerPartnerKey
                    // PBankAccess.LoadViaPPartnerContactPartnerKey
                    string DifferentField = CodeGenerationAccess.FindOtherConstraintSameOtherTable(
                        OtherTable.grpConstraint,
                        constraint);
                    string LoadViaProcedureName = TTable.NiceTableName(ACurrentTable.strName);
                    string MyOtherTableName = "My" + TTable.NiceTableName(constraint.strThisTable);

                    if (DifferentField.Length != 0)
                    {
                        LoadViaProcedureName += TTable.NiceFieldName(DifferentField);
                        MyOtherTableName += TTable.NiceFieldName(DifferentField);
                    }

                    ProcessTemplate snippetDelete = ASnippet.GetSnippet("DELETEBYPRIMARYKEYCASCADING");
                    snippetDelete.SetCodelet("OTHERTABLENAME", TTable.NiceTableName(constraint.strThisTable));
                    snippetDelete.SetCodelet("MYOTHERTABLENAME", MyOtherTableName);
                    snippetDelete.SetCodelet("VIAPROCEDURENAME", "Via" + LoadViaProcedureName);
                    snippetDelete.SetCodelet("CSVLISTOTHERPRIMARYKEYFIELDS", csvListOtherPrimaryKeyFields);

                    if (OtherTable.FReferenced.Count <= CASCADING_DELETE_MAX_REFERENCES)
                    {
                        snippetDelete.SetCodelet("OTHERTABLEALSOCASCADING", "true");
                    }

                    ASnippet.InsertSnippet("DELETEBYPRIMARYKEYCASCADING", snippetDelete);

                    snippetDelete = ASnippet.GetSnippet("DELETEBYTEMPLATECASCADING");
                    snippetDelete.SetCodelet("OTHERTABLENAME", TTable.NiceTableName(constraint.strThisTable));
                    snippetDelete.SetCodelet("MYOTHERTABLENAME", MyOtherTableName);
                    snippetDelete.SetCodelet("VIAPROCEDURENAME", "Via" + LoadViaProcedureName);
                    snippetDelete.SetCodelet("CSVLISTOTHERPRIMARYKEYFIELDS", csvListOtherPrimaryKeyFields);

                    if (OtherTable.FReferenced.Count <= CASCADING_DELETE_MAX_REFERENCES)
                    {
                        snippetDelete.SetCodelet("OTHERTABLEALSOCASCADING", "true");
                    }

                    ASnippet.InsertSnippet("DELETEBYTEMPLATECASCADING", snippetDelete);

                    ProcessTemplate snippetCount = ASnippet.GetSnippet("COUNTBYPRIMARYKEYCASCADING");
                    snippetCount.SetCodelet("OTHERTABLENAME", TTable.NiceTableName(constraint.strThisTable));
                    snippetCount.SetCodelet("MYOTHERTABLENAME", MyOtherTableName);
                    snippetCount.SetCodelet("VIAPROCEDURENAME", "Via" + LoadViaProcedureName);
                    snippetCount.SetCodelet("CSVLISTOTHERPRIMARYKEYFIELDS", csvListOtherPrimaryKeyFields);

                    if (OtherTable.FReferenced.Count <= CASCADING_DELETE_MAX_REFERENCES)
                    {
                        snippetCount.SetCodelet("OTHERTABLEALSOCASCADING", "true");
                    }

                    ASnippet.InsertSnippet("COUNTBYPRIMARYKEYCASCADING", snippetCount);

                    snippetCount = ASnippet.GetSnippet("COUNTBYTEMPLATECASCADING");
                    snippetCount.SetCodelet("OTHERTABLENAME", TTable.NiceTableName(constraint.strThisTable));
                    snippetCount.SetCodelet("MYOTHERTABLENAME", MyOtherTableName);
                    snippetCount.SetCodelet("VIAPROCEDURENAME", "Via" + LoadViaProcedureName);
                    snippetCount.SetCodelet("CSVLISTOTHERPRIMARYKEYFIELDS", csvListOtherPrimaryKeyFields);

                    if (OtherTable.FReferenced.Count <= CASCADING_DELETE_MAX_REFERENCES)
                    {
                        snippetCount.SetCodelet("OTHERTABLEALSOCASCADING", "true");
                    }

                    ASnippet.InsertSnippet("COUNTBYTEMPLATECASCADING", snippetCount);
                }
            }

            return true;
        }

        /// <summary>
        /// generate code for cascading deletions etc
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="AFilePath"></param>
        /// <param name="ANamespaceName"></param>
        /// <param name="AFileName"></param>
        /// <returns></returns>
        public static Boolean WriteTypedDataCascading(TDataDefinitionStore AStore, string AFilePath, string ANamespaceName, string AFileName)
        {
            Console.WriteLine("writing namespace " + ANamespaceName);

            string templateDir = TAppSettingsManager.GetValue("TemplateDir", true);
            ProcessTemplate Template = new ProcessTemplate(templateDir + Path.DirectorySeparatorChar +
                "ORM" + Path.DirectorySeparatorChar +
                "DataCascading.cs");

            Template.AddToCodelet("NAMESPACE", ANamespaceName);

            // load default header with license and copyright
            Template.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(templateDir));

            foreach (TTable currentTable in AStore.GetTables())
            {
                ProcessTemplate snippet = Template.GetSnippet("TABLECASCADING");

                if (InsertMainProcedures(AStore, currentTable, Template, snippet))
                {
                    Template.AddToCodelet("USINGNAMESPACES",
                        CodeGenerationAccess.GetNamespace(currentTable.strGroup), false);
                    Template.AddToCodelet("USINGNAMESPACES",
                        CodeGenerationAccess.GetNamespace(currentTable.strGroup).Replace(
                            ".Data;", ".Data.Access;").
                        Replace("Ict.Petra.Shared.", "Ict.Petra.Server."), false);

                    Template.InsertSnippet("TABLECASCADINGLOOP", snippet);
                }
            }

            Template.FinishWriting(AFilePath + AFileName + ".cs", ".cs", true);

            return true;
        }
    }
}