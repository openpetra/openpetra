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
using System.Data.Odbc;
using System.Collections;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;
using Ict.Common;
using Ict.Common.IO;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Ict.Tools.CodeGeneration.DataStore
{
    /// This will generate most of the datastore.
    /// Only here should SQL queries happen.
    public class CodeGenerationAccess
    {
        /// <summary>
        /// used for each table, to avoid duplicate loadvialink etc
        /// </summary>
        private static ArrayList DirectReferences;

        /// <summary>
        /// This function checks if there is already a similar constraint that connects the two tables on the same fields
        /// </summary>
        /// <returns>true if other constraint exists</returns>
        private static Boolean LoadViaHasAlreadyBeenImplemented(TConstraint AConstraint)
        {
            return DirectReferences.Contains(
                AConstraint.strOtherTable + "," +
                StringHelper.StrMerge(AConstraint.strThisFields, ',') + "," +
                StringHelper.StrMerge(AConstraint.strOtherFields, ','));
        }

        private static void AddDirectReference(TConstraint AConstraint)
        {
            DirectReferences.Add(AConstraint.strOtherTable + "," +
                StringHelper.StrMerge(AConstraint.strThisFields, ',') + "," +
                StringHelper.StrMerge(AConstraint.strOtherFields, ','));
        }

        /// <summary>
        /// do we want a special load via function for this foreign key?
        /// </summary>
        /// <param name="AConstraint"></param>
        /// <returns></returns>
        public static Boolean ValidForeignKeyConstraintForLoadVia(TConstraint AConstraint)
        {
            return (AConstraint.strType == "foreignkey") && (!AConstraint.strThisFields.Contains("s_created_by_c"))
                   && (!AConstraint.strThisFields.Contains("s_modified_by_c")) && (AConstraint.strThisTable != "a_ledger");
        }

        /// <summary>
        /// get the namespace for a given table group name
        /// </summary>
        public static string GetNamespace(string AStrGroup)
        {
            String NamespaceTable = TTable.GetNamespace(AStrGroup);

            return "using Ict.Petra.Shared." +
                   NamespaceTable + ".Data" +
                   ";" + Environment.NewLine;
        }

        /// <summary>
        /// get formal and actual parameters for a unique or primary key
        /// </summary>
        /// <param name="ACurrentTable"></param>
        /// <param name="AConstraint"></param>
        /// <param name="formalParametersKey"></param>
        /// <param name="actualParametersKey"></param>
        /// <param name="numberKeyColumns"></param>
        /// <param name="actualParametersToString"></param>
        private static void PrepareCodeletsKey(
            TTable ACurrentTable,
            TConstraint AConstraint,
            out string formalParametersKey,
            out string actualParametersKey,
            out string actualParametersToString,
            out int numberKeyColumns)
        {
            formalParametersKey = "";
            actualParametersKey = "";
            actualParametersToString = "";

            numberKeyColumns = AConstraint.strThisFields.Count;

            int counterKeyField = 0;

            foreach (string field in AConstraint.strThisFields)
            {
                if (counterKeyField > 0)
                {
                    formalParametersKey += ", ";
                    actualParametersKey += ", ";
                    actualParametersToString += " + \" \" + ";
                }

                TTableField typedField = ACurrentTable.GetField(field);

                formalParametersKey += typedField.GetDotNetType() + " A" + TTable.NiceFieldName(field);
                actualParametersKey += "A" + TTable.NiceFieldName(field);
                actualParametersToString += "A" + TTable.NiceFieldName(field) + ".ToString()";

                counterKeyField++;
            }

            // for partners, show their names as well. This is used in function AddOrModifyRecord to show the users which values are different
            foreach (TTableField column in ACurrentTable.grpTableField)
            {
                if (column.strName.Contains("_name_"))
                {
                    actualParametersToString += " + ExistingRecord[0]." + TTable.NiceFieldName(column) + ".ToString()";
                }
            }
        }

        private static void PrepareCodeletsPrimaryKey(
            TTable ACurrentTable,
            out string formalParametersPrimaryKey,
            out string actualParametersPrimaryKey,
            out string actualParametersPrimaryKeyToString,
            out int numberPrimaryKeyColumns)
        {
            formalParametersPrimaryKey = "";
            actualParametersPrimaryKey = "";
            actualParametersPrimaryKeyToString = "";
            numberPrimaryKeyColumns = 0;

            if (!ACurrentTable.HasPrimaryKey())
            {
                return;
            }

            PrepareCodeletsKey(ACurrentTable,
                ACurrentTable.GetPrimaryKey(),
                out formalParametersPrimaryKey,
                out actualParametersPrimaryKey,
                out actualParametersPrimaryKeyToString,
                out numberPrimaryKeyColumns);
        }

        private static void PrepareCodeletsUniqueKey(
            TTable ACurrentTable,
            out string formalParametersUniqueKey,
            out string actualParametersUniqueKey,
            out int numberUniqueKeyColumns)
        {
            formalParametersUniqueKey = "";
            actualParametersUniqueKey = "";
            numberUniqueKeyColumns = 0;
            string dummy = "";

            if (!ACurrentTable.HasUniqueKey())
            {
                return;
            }

            PrepareCodeletsKey(ACurrentTable,
                ACurrentTable.GetFirstUniqueKey(),
                out formalParametersUniqueKey,
                out actualParametersUniqueKey,
                out dummy,
                out numberUniqueKeyColumns);
        }

        private static void PrepareCodeletsForeignKey(
            TTable AOtherTable,
            TConstraint AConstraint,
            out string whereClauseForeignKey,
            out string whereClauseViaOtherTable,
            out string odbcParametersForeignKey,
            out int numberFields,
            out string namesOfThisTableFields)
        {
            whereClauseForeignKey = "";
            whereClauseViaOtherTable = "";
            numberFields = AConstraint.strThisFields.Count;
            namesOfThisTableFields = "";
            odbcParametersForeignKey =
                "OdbcParameter[] ParametersArray = new OdbcParameter[" +
                AConstraint.strThisFields.Count.ToString() + "];" +
                Environment.NewLine;

            int counterKeyField = 0;

            foreach (string field in AConstraint.strThisFields)
            {
                if (counterKeyField > 0)
                {
                    whereClauseForeignKey += " AND ";
                    whereClauseViaOtherTable += " AND ";
                    namesOfThisTableFields += ", ";
                }

                namesOfThisTableFields += '"' + AConstraint.strThisFields[counterKeyField] + '"';

                string otherfield = AConstraint.strOtherFields[counterKeyField];
                TTableField otherTypedField = AOtherTable.GetField(otherfield);

                whereClauseForeignKey += field + " = ?";
                whereClauseViaOtherTable += "PUB_" + AConstraint.strThisTable + "." + field +
                                            " = PUB_" + AConstraint.strOtherTable + "." + otherfield;

                odbcParametersForeignKey += "ParametersArray[" + counterKeyField.ToString() + "] = " +
                                            "new OdbcParameter(\"\", " + CodeGenerationPetra.ToOdbcTypeString(otherTypedField) +
                                            (otherTypedField.iLength != -1 ? ", " +
                                             otherTypedField.iLength.ToString() : "") + ");" + Environment.NewLine;
                odbcParametersForeignKey += "ParametersArray[" + counterKeyField.ToString() + "].Value = " +
                                            "((object)(A" + TTable.NiceFieldName(otherfield) + "));" + Environment.NewLine;

                counterKeyField++;
            }
        }

        /// <summary>
        /// prepare some code for the via other linking table (bridge)
        /// </summary>
        /// <param name="AConstraint">the constraint between current and linking table</param>
        /// <param name="AConstraintOther">the constraint between linking table and other</param>
        /// <param name="whereClauseViaLinkTable"></param>
        /// <param name="whereClauseAllViaTables"></param>
        private static void PrepareCodeletsViaLinkTable(
            TConstraint AConstraint,
            TConstraint AConstraintOther,
            out string whereClauseViaLinkTable,
            out string whereClauseAllViaTables)
        {
            whereClauseViaLinkTable = "";

            int counterKeyField = 0;

            foreach (string field in AConstraint.strThisFields)
            {
                if (counterKeyField > 0)
                {
                    whereClauseViaLinkTable += " AND ";
                }

                string otherfield = AConstraint.strOtherFields[counterKeyField];

                whereClauseViaLinkTable += "PUB_" + AConstraint.strThisTable + "." + field +
                                           " = PUB_" + AConstraint.strOtherTable + "." + otherfield;

                counterKeyField++;
            }

            whereClauseViaLinkTable += " AND ";
            whereClauseAllViaTables = whereClauseViaLinkTable;

            counterKeyField = 0;

            foreach (string field in AConstraintOther.strThisFields)
            {
                if (counterKeyField > 0)
                {
                    whereClauseViaLinkTable += " AND ";
                    whereClauseAllViaTables += " AND ";
                }

                string otherfield = AConstraintOther.strOtherFields[counterKeyField];

                whereClauseViaLinkTable += "PUB_" + AConstraintOther.strThisTable + "." + field + " = ?";
                whereClauseAllViaTables += "PUB_" + AConstraintOther.strThisTable + "." + field +
                                           " = PUB_" + AConstraintOther.strOtherTable + "." + otherfield;


                counterKeyField++;
            }
        }

        /// <summary>
        /// insert the viaothertable functions for one specific constraint
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="AConstraint"></param>
        /// <param name="ACurrentTable"></param>
        /// <param name="AOtherTable"></param>
        /// <param name="ATemplate"></param>
        /// <param name="ASnippet"></param>
        private static void InsertViaOtherTableConstraint(TDataDefinitionStore AStore,
            TConstraint AConstraint,
            TTable ACurrentTable,
            TTable AOtherTable,
            ProcessTemplate ATemplate,
            ProcessTemplate ASnippet)
        {
            ATemplate.AddToCodelet("USINGNAMESPACES",
                GetNamespace(AOtherTable.strGroup), false);

            ProcessTemplate snippetViaTable = ATemplate.GetSnippet("VIAOTHERTABLE");
            snippetViaTable.SetCodelet("OTHERTABLENAME", TTable.NiceTableName(AOtherTable.strName));
            snippetViaTable.SetCodelet("SQLOTHERTABLENAME", AOtherTable.strName);

            string ProcedureName = "Via" + TTable.NiceTableName(AOtherTable.strName);

            // check if other foreign key exists that references the same table, e.g.
            // PBankAccess.CountViaPPartnerPartnerKey
            // PBankAccess.CountViaPPartnerContactPartnerKey
            string DifferentField = FindOtherConstraintSameOtherTable(ACurrentTable.grpConstraint, AConstraint);

            if (DifferentField.Length != 0)
            {
                ProcedureName += TTable.NiceFieldName(DifferentField);
            }

            int notUsedInt;
            string formalParametersOtherPrimaryKey;
            string actualParametersOtherPrimaryKey;
            string dummy;

            PrepareCodeletsPrimaryKey(AOtherTable,
                out formalParametersOtherPrimaryKey,
                out actualParametersOtherPrimaryKey,
                out dummy,
                out notUsedInt);

            int numberFields;

            string namesOfThisTableFields;
            string notUsed;
            PrepareCodeletsForeignKey(
                AOtherTable,
                AConstraint,
                out notUsed,
                out notUsed,
                out notUsed,
                out numberFields,
                out namesOfThisTableFields);

            snippetViaTable.SetCodelet("VIAPROCEDURENAME", ProcedureName);
            snippetViaTable.SetCodelet("FORMALPARAMETERSOTHERPRIMARYKEY", formalParametersOtherPrimaryKey);
            snippetViaTable.SetCodelet("ACTUALPARAMETERSOTHERPRIMARYKEY", actualParametersOtherPrimaryKey);
            snippetViaTable.SetCodelet("NUMBERFIELDS", numberFields.ToString());
            snippetViaTable.SetCodelet("NUMBERFIELDSOTHER", (actualParametersOtherPrimaryKey.Split(',').Length).ToString());
            snippetViaTable.SetCodelet("THISTABLEFIELDS", namesOfThisTableFields);

            AddDirectReference(AConstraint);

            ASnippet.InsertSnippet("VIAOTHERTABLE", snippetViaTable);
        }

        /// <summary>
        /// this is for foreign keys, eg load all countries with currency EUR
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="ACurrentTable"></param>
        /// <param name="ATemplate"></param>
        /// <param name="ASnippet"></param>
        private static void InsertViaOtherTable(TDataDefinitionStore AStore,
            TTable ACurrentTable,
            ProcessTemplate ATemplate,
            ProcessTemplate ASnippet)
        {
            foreach (TConstraint myConstraint in ACurrentTable.grpConstraint)
            {
                if (ValidForeignKeyConstraintForLoadVia(myConstraint))
                {
                    TTable OtherTable = AStore.GetTable(myConstraint.strOtherTable);

                    if (!LoadViaHasAlreadyBeenImplemented(myConstraint))
                    {
                        InsertViaOtherTableConstraint(AStore,
                            myConstraint,
                            ACurrentTable,
                            OtherTable,
                            ATemplate,
                            ASnippet);
                    }

                    // AccountHierarchy: there is no constraint that references Ledger directly, but constraint referencing the Account table with a key that contains the ledger reference
                    // but because the key in Ledger is already the primary key, a LoadViaLedger is required.
                    // other way round: p_foundation_proposal_detail has 2 constraints for foundation and foundationproposal
                    foreach (string field in myConstraint.strOtherFields)
                    {
                        // get a constraint that is only based on that field
                        TConstraint OtherLinkConstraint = OtherTable.GetConstraint(StringHelper.StrSplit(field, ","));

                        if ((OtherLinkConstraint != null) && ValidForeignKeyConstraintForLoadVia(OtherLinkConstraint))
                        {
                            TConstraint NewConstraint = new TConstraint();
                            NewConstraint.strName = OtherLinkConstraint.strName + "forLoadVia";
                            NewConstraint.strType = "foreignkey";
                            NewConstraint.strThisTable = myConstraint.strThisTable;
                            NewConstraint.strThisFields =
                                StringHelper.StrSplit(myConstraint.strThisFields[myConstraint.strOtherFields.IndexOf(
                                                                                     field)], ",");
                            NewConstraint.strOtherTable = OtherLinkConstraint.strOtherTable;
                            NewConstraint.strOtherFields = OtherLinkConstraint.strOtherFields;

                            if (!LoadViaHasAlreadyBeenImplemented(NewConstraint))
                            {
                                InsertViaOtherTableConstraint(AStore,
                                    NewConstraint,
                                    ACurrentTable,
                                    AStore.GetTable(OtherLinkConstraint.strOtherTable),
                                    ATemplate,
                                    ASnippet);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function checks if there is another constraint in this ATable,
        /// that references the same other table
        /// It will find the field that has a different name, so that names can be unique
        /// </summary>
        /// <param name="AConstraints"></param>
        /// <param name="AConstraint"></param>
        /// <returns>the field that is different in the two keys; empty string if there is no other constraint or no different field</returns>
        public static String FindOtherConstraintSameOtherTable(List <TConstraint>AConstraints, TConstraint AConstraint)
        {
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
                            return s;
                        }
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// This function checks if there is another constraint from the same table
        /// that references the current table through a link table
        /// </summary>
        /// <param name="AConstraints"></param>
        /// <param name="AConstraint"></param>
        /// <returns>true if other constraint exists</returns>
        public static Boolean FindOtherConstraintSameOtherTable2(ArrayList AConstraints, TConstraint AConstraint)
        {
            foreach (TConstraint myConstraint in AConstraints)
            {
                if ((myConstraint != AConstraint)
                    && (myConstraint.strOtherTable == AConstraint.strOtherTable))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// situation 2: bridging tables
        /// for example p_location and p_partner are connected through p_partner_location
        /// it would be helpful, to be able to do:
        /// location.LoadViaPartner(partnerkey) to get all locations of the partner
        /// partner.loadvialocation(locationkey) to get all partners living at that location
        /// general solution: pick up all foreign keys from other tables (B) to the primary key of the current table (A),
        /// where the other table has a foreign key to another table (C), using other fields in the primary key of (B) than the link to (A).
        /// get all tables that reference the current table
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="ACurrentTable"></param>
        /// <param name="ATemplate"></param>
        /// <param name="ASnippet"></param>
        private static void InsertViaLinkTable(TDataDefinitionStore AStore, TTable ACurrentTable, ProcessTemplate ATemplate, ProcessTemplate ASnippet)
        {
            // step 1: collect all the valid constraints of such link tables
            ArrayList OtherLinkConstraints = new ArrayList();
            ArrayList References = new ArrayList();

            foreach (TConstraint Reference in ACurrentTable.GetReferences())
            {
                TTable LinkTable = AStore.GetTable(Reference.strThisTable);
                try
                {
                    TConstraint LinkPrimaryKey = LinkTable.GetPrimaryKey();

                    if (StringHelper.Contains(LinkPrimaryKey.strThisFields, Reference.strThisFields))
                    {
                        // check how many constraints from the link table are from fields in the primary key
                        // a link table only should have 2
                        // so find the other one
                        TConstraint OtherLinkConstraint = null;
                        bool IsLinkTable = false;

                        foreach (TConstraint LinkConstraint in LinkTable.grpConstraint)
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
            int Count = 0;

            foreach (TConstraint OtherLinkConstraint in OtherLinkConstraints)
            {
                TTable OtherTable = AStore.GetTable(OtherLinkConstraint.strOtherTable);
                string ProcedureName = "Via" + TTable.NiceTableName(OtherTable.strName);

                // check if other foreign key exists that references the same table, e.g.
                // PPartnerAccess.LoadViaSUserPRecentPartners
                // PPartnerAccess.LoadViaSUserPCustomisedGreeting
                // also PFoundationProposalAccess.LoadViaPFoundationPFoundationProposalDetail and
                // TODO AlreadyExistsProcedureSameName necessary for PPersonAccess.LoadViaPUnit (p_field_key_n) and PPersonAccess.LoadViaPUnitPmGeneralApplication
                // Question: does PPersonAccess.LoadViaPUnitPmGeneralApplication make sense?
                if (FindOtherConstraintSameOtherTable2(OtherLinkConstraints,
                        OtherLinkConstraint)
                    || LoadViaHasAlreadyBeenImplemented(OtherLinkConstraint)

                    // TODO || AlreadyExistsProcedureSameName(ProcedureName)
                    || ((ProcedureName == "ViaPUnit") && (OtherLinkConstraint.strThisTable == "pm_general_application"))
                    )
                {
                    ProcedureName += TTable.NiceTableName(OtherLinkConstraint.strThisTable);
                }

                ATemplate.AddToCodelet("USINGNAMESPACES",
                    GetNamespace(OtherTable.strGroup), false);

                ProcessTemplate snippetLinkTable = ATemplate.GetSnippet("VIALINKTABLE");

                snippetLinkTable.SetCodelet("VIAPROCEDURENAME", ProcedureName);
                snippetLinkTable.SetCodelet("OTHERTABLENAME", TTable.NiceTableName(OtherTable.strName));
                snippetLinkTable.SetCodelet("SQLOTHERTABLENAME", OtherTable.strName);
                snippetLinkTable.SetCodelet("SQLLINKTABLENAME", OtherLinkConstraint.strThisTable);

                string notUsed;
                int notUsedInt;
                string formalParametersOtherPrimaryKey;
                string actualParametersOtherPrimaryKey;

                PrepareCodeletsPrimaryKey(OtherTable,
                    out formalParametersOtherPrimaryKey,
                    out actualParametersOtherPrimaryKey,
                    out notUsed,
                    out notUsedInt);

                PrepareCodeletsPrimaryKey(ACurrentTable,
                    out notUsed,
                    out notUsed,
                    out notUsed,
                    out notUsedInt);

                string whereClauseForeignKey;
                string whereClauseViaOtherTable;
                string odbcParametersForeignKey;
                PrepareCodeletsForeignKey(
                    OtherTable,
                    OtherLinkConstraint,
                    out whereClauseForeignKey,
                    out whereClauseViaOtherTable,
                    out odbcParametersForeignKey,
                    out notUsedInt,
                    out notUsed);

                string whereClauseViaLinkTable;
                string whereClauseAllViaTables;
                PrepareCodeletsViaLinkTable(
                    (TConstraint)References[Count],
                    OtherLinkConstraint,
                    out whereClauseViaLinkTable,
                    out whereClauseAllViaTables);

                snippetLinkTable.SetCodelet("FORMALPARAMETERSOTHERPRIMARYKEY", formalParametersOtherPrimaryKey);
                snippetLinkTable.SetCodelet("ACTUALPARAMETERSOTHERPRIMARYKEY", actualParametersOtherPrimaryKey);
                snippetLinkTable.SetCodelet("ODBCPARAMETERSFOREIGNKEY", odbcParametersForeignKey);
                snippetLinkTable.SetCodelet("WHERECLAUSEVIALINKTABLE", whereClauseViaLinkTable);
                snippetLinkTable.SetCodelet("WHERECLAUSEALLVIATABLES", whereClauseAllViaTables);

                ASnippet.InsertSnippet("VIALINKTABLE", snippetLinkTable);

                Count = Count + 1;
            }
        }

        private static void InsertMainProcedures(TDataDefinitionStore AStore,
            TTable ACurrentTable,
            ProcessTemplate ATemplate,
            ProcessTemplate ASnippet)
        {
            ASnippet.SetCodelet("TABLENAME", TTable.NiceTableName(ACurrentTable.strName));
            ASnippet.SetCodeletComment("TABLE_DESCRIPTION", ACurrentTable.strDescription);
            ASnippet.SetCodelet("SQLTABLENAME", ACurrentTable.strName);
            ASnippet.SetCodelet("VIAOTHERTABLE", "");
            ASnippet.SetCodelet("VIALINKTABLE", "");

            string formalParametersPrimaryKey;
            string actualParametersPrimaryKey;
            string actualParametersPrimaryKeyToString;
            int numberPrimaryKeyColumns;

            PrepareCodeletsPrimaryKey(ACurrentTable,
                out formalParametersPrimaryKey,
                out actualParametersPrimaryKey,
                out actualParametersPrimaryKeyToString,
                out numberPrimaryKeyColumns);

            ASnippet.SetCodelet("FORMALPARAMETERSPRIMARYKEY", formalParametersPrimaryKey);
            ASnippet.SetCodelet("ACTUALPARAMETERSPRIMARYKEY", actualParametersPrimaryKey);
            ASnippet.SetCodelet("ACTUALPARAMETERSPRIMARYKEYTOSTRING", actualParametersPrimaryKeyToString);
            ASnippet.SetCodelet("PRIMARYKEYNUMBERCOLUMNS", numberPrimaryKeyColumns.ToString());

            string formalParametersUniqueKey;
            string actualParametersUniqueKey;
            int numberUniqueKeyColumns;

            PrepareCodeletsUniqueKey(ACurrentTable,
                out formalParametersUniqueKey,
                out actualParametersUniqueKey,
                out numberUniqueKeyColumns);

            ASnippet.SetCodelet("FORMALPARAMETERSUNIQUEKEY", formalParametersUniqueKey);
            ASnippet.SetCodelet("ACTUALPARAMETERSUNIQUEKEY", actualParametersUniqueKey);
            ASnippet.SetCodelet("UNIQUEKEYNUMBERCOLUMNS", numberUniqueKeyColumns.ToString());


            ASnippet.SetCodelet("SEQUENCENAMEANDFIELD", "");

            foreach (TTableField tablefield in ACurrentTable.grpTableField)
            {
                // is there a field filled by a sequence?
                // yes: get the next value of that sequence and assign to row
                if (tablefield.strSequence.Length > 0)
                {
                    ASnippet.SetCodelet("SEQUENCENAMEANDFIELD", ", \"" + tablefield.strSequence + "\", \"" + tablefield.strName + "\"");

                    // assume only one sequence per table
                    break;
                }
            }
        }

        /// <summary>
        /// generate code for reading and writing typed data tables from and to the database
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="strGroup"></param>
        /// <param name="AFilePath"></param>
        /// <param name="ANamespaceName"></param>
        /// <param name="AFilename"></param>
        /// <returns></returns>
        public static Boolean WriteTypedDataAccess(TDataDefinitionStore AStore,
            string strGroup,
            string AFilePath,
            string ANamespaceName,
            string AFilename)
        {
            Console.WriteLine("processing namespace PetraTypedDataAccess." + strGroup.Substring(0, 1).ToUpper() + strGroup.Substring(1));

            string templateDir = TAppSettingsManager.GetValue("TemplateDir", true);
            ProcessTemplate Template = new ProcessTemplate(templateDir + Path.DirectorySeparatorChar +
                "ORM" + Path.DirectorySeparatorChar +
                "DataAccess.cs");

            Template.SetCodelet("NAMESPACE", ANamespaceName);

            // load default header with license and copyright
            Template.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(templateDir));

            Template.AddToCodelet("USINGNAMESPACES", GetNamespace(strGroup), false);

            foreach (TTable currentTable in AStore.GetTables())
            {
                if (currentTable.strGroup == strGroup)
                {
                    DirectReferences = new ArrayList();

                    ProcessTemplate snippet = Template.GetSnippet("TABLEACCESS");

                    InsertMainProcedures(AStore, currentTable, Template, snippet);
                    InsertViaOtherTable(AStore, currentTable, Template, snippet);
                    InsertViaLinkTable(AStore, currentTable, Template, snippet);

                    Template.InsertSnippet("TABLEACCESSLOOP", snippet);
                }
            }

            Template.FinishWriting(AFilePath + AFilename + ".cs", ".cs", true);

            return true;
        }
    }
}