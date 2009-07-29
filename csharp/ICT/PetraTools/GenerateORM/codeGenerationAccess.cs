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
        /// <summary>
        /// used for each table, to avoid duplicate loadvialink etc
        /// </summary>
        private static ArrayList DirectReferences;
        
        public static Boolean ValidForeignKeyConstraintForLoadVia(TConstraint AConstraint)
        {
            return (AConstraint.strType == "foreignkey") && (!AConstraint.strThisFields.Contains("s_created_by_c"))
                   && (!AConstraint.strThisFields.Contains("s_modified_by_c")) && (AConstraint.strThisTable != "a_ledger");
        }

        public static string GetNamespace(String ATableGroup)
        {
            String NamespaceTable;

            if (ATableGroup == "partner")
            {
                NamespaceTable = "MPartner.Partner";
            }
            else if (ATableGroup == "mailroom")
            {
                NamespaceTable = "MPartner.Mailroom";
            }
            else if (ATableGroup == "account")
            {
                NamespaceTable = "MFinance.Account";
            }
            else if (ATableGroup == "gift")
            {
                NamespaceTable = "MFinance.Gift";
            }
            else if (ATableGroup == "ap")
            {
                NamespaceTable = "MFinance.AP";
            }
            else if (ATableGroup == "ar")
            {
                NamespaceTable = "MFinance.AR";
            }
            else if (ATableGroup == "personnel")
            {
                NamespaceTable = "MPersonnel.Personnel";
            }
            else if (ATableGroup == "units")
            {
                NamespaceTable = "MPersonnel.Units";
            }
            else if (ATableGroup == "main")
            {
                NamespaceTable = "MCommon";
            }
            else
            {
                NamespaceTable = 'M' + ATableGroup;
            }

            return "using Ict.Petra.Shared." +
                    NamespaceTable.Replace("Msysman", "MSysMan").
                    Replace("Mconference", "MConference").
                    Replace("Mhospitality", "MHospitality") + ".Data" +
                    ";" + Environment.NewLine;
        }

        private static void PrepareCodeletsPrimaryKey(
                TTable ACurrentTable,
                out string csvListPrimaryKeyFields,
                out string formalParametersPrimaryKey,
                out string actualParametersPrimaryKey,
                out string whereClausePrimaryKey,
                out string odbcParametersPrimaryKey)
        {
            csvListPrimaryKeyFields = "";
            formalParametersPrimaryKey = "";
            actualParametersPrimaryKey = "";
            whereClausePrimaryKey = "";
            odbcParametersPrimaryKey = "";
            
            if (!ACurrentTable.HasPrimaryKey())
            {
                return;
            }
            
            odbcParametersPrimaryKey =
                "OdbcParameter[] ParametersArray = new OdbcParameter[" + 
                ACurrentTable.GetPrimaryKey().strThisFields.Count.ToString() + "];" + 
                Environment.NewLine;
            int counterPrimaryKeyField = 0;

            foreach (string field in ACurrentTable.GetPrimaryKey().strThisFields)
            {
                if (counterPrimaryKeyField > 0)
                {
                    csvListPrimaryKeyFields += ", ";
                    formalParametersPrimaryKey += ", ";
                    actualParametersPrimaryKey += ", ";
                    whereClausePrimaryKey += " AND ";
                }
                
                TTableField typedField = ACurrentTable.GetField(field);
                
                csvListPrimaryKeyFields += "\"" + field + "\"";
                formalParametersPrimaryKey += typedField.GetDotNetType() + " A" + TTable.NiceFieldName(field);
                actualParametersPrimaryKey += "A" + TTable.NiceFieldName(field);
                whereClausePrimaryKey += field + " = ?";
                
                odbcParametersPrimaryKey += "ParametersArray[" + counterPrimaryKeyField.ToString() + "] = " +
                    "new OdbcParameter(\"\", " + codeGenerationPetra.ToOdbcTypeString(typedField) + 
                    (typedField.iLength != -1? ", " + typedField.iLength.ToString():"") + ");" + Environment.NewLine;
                odbcParametersPrimaryKey += "ParametersArray[" + counterPrimaryKeyField.ToString() + "].Value = " +
                    "((object)(A" + TTable.NiceFieldName(field) + "));" + Environment.NewLine;
                
                counterPrimaryKeyField++;
            }
        }
        
        private static void PrepareCodeletsForeignKey(
                TTable AOtherTable,
                TConstraint AConstraint,
                out string whereClauseForeignKey,
                out string whereClauseViaOtherTable,
                out string odbcParametersForeignKey)
        {
            whereClauseForeignKey = "";
            whereClauseViaOtherTable = "";
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
                }
                
                string otherfield = AConstraint.strOtherFields[counterKeyField];
                TTableField otherTypedField = AOtherTable.GetField(otherfield);
                
                whereClauseForeignKey += field + " = ?";
                whereClauseViaOtherTable += "PUB_" + AConstraint.strThisTable + "." + field +
                    " = PUB_" + AConstraint.strOtherTable + "." + otherfield;

                odbcParametersForeignKey += "ParametersArray[" + counterKeyField.ToString() + "] = " +
                    "new OdbcParameter(\"\", " + codeGenerationPetra.ToOdbcTypeString(otherTypedField) + 
                    (otherTypedField.iLength != -1? ", " + otherTypedField.iLength.ToString():"") + ");" + Environment.NewLine;
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
        /// this is for foreign keys, eg load all countries with currency EUR
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="ACurrentTable"></param>
        /// <param name="ATemplate"></param>
        /// <param name="ASnippet"></param>
        private static void InsertViaOtherTable(TDataDefinitionStore AStore, TTable ACurrentTable, ProcessTemplate ATemplate, ProcessTemplate ASnippet)
        {
            foreach (TConstraint myConstraint in ACurrentTable.grpConstraint.List)
            {
                if (ValidForeignKeyConstraintForLoadVia(myConstraint))
                {
                    TTable OtherTable = AStore.GetTable(myConstraint.strOtherTable);
                    ATemplate.AddToCodelet("USINGNAMESPACES", 
                                          GetNamespace(OtherTable.strGroup), false);
                    
                    ProcessTemplate snippetViaTable = ATemplate.GetSnippet("VIAOTHERTABLE");
                    snippetViaTable.SetCodelet("OTHERTABLENAME", TTable.NiceTableName(OtherTable.strName));
                    snippetViaTable.SetCodelet("SQLOTHERTABLENAME", OtherTable.strName);

                    string ProcedureName = "Via" + TTable.NiceTableName(OtherTable.strName);
                    
                    // check if other foreign key exists that references the same table, e.g.
                    // PBankAccess.CountViaPPartnerPartnerKey
                    // PBankAccess.CountViaPPartnerContactPartnerKey
                    string DifferentField = FindOtherConstraintSameOtherTable(ACurrentTable.grpConstraint.List, myConstraint);
        
                    if (DifferentField.Length != 0)
                    {
                        ProcedureName += TTable.NiceFieldName(DifferentField);
                    }
                    
                    string notUsed;
                    string formalParametersOtherPrimaryKey;
                    string actualParametersOtherPrimaryKey;
                    
                    PrepareCodeletsPrimaryKey(OtherTable,
                            out notUsed,
                            out formalParametersOtherPrimaryKey,
                            out actualParametersOtherPrimaryKey,
                            out notUsed,
                            out notUsed);

                    string whereClauseForeignKey;
                    string whereClauseViaOtherTable;
                    string odbcParametersForeignKey;
                    PrepareCodeletsForeignKey(
                            OtherTable,
                            myConstraint,
                            out whereClauseForeignKey,
                            out whereClauseViaOtherTable,
                            out odbcParametersForeignKey);

                    snippetViaTable.SetCodelet("VIAPROCEDURENAME", ProcedureName);
                    snippetViaTable.SetCodelet("FORMALPARAMETERSOTHERPRIMARYKEY", formalParametersOtherPrimaryKey);
                    snippetViaTable.SetCodelet("ACTUALPARAMETERSOTHERPRIMARYKEY", actualParametersOtherPrimaryKey);
                    snippetViaTable.SetCodelet("WHERECLAUSEFOREIGNKEY", whereClauseForeignKey);
                    snippetViaTable.SetCodelet("ODBCPARAMETERSFOREIGNKEY", odbcParametersForeignKey);
                    snippetViaTable.SetCodelet("WHERECLAUSEVIAOTHERTABLE", whereClauseViaOtherTable);
                    
                    DirectReferences.Add(myConstraint);
                    
                    ASnippet.InsertSnippet("VIAOTHERTABLE", snippetViaTable);
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
        public static String FindOtherConstraintSameOtherTable(ArrayList AConstraints, TConstraint AConstraint)
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
                if ((myConstraint != AConstraint) && (myConstraint.strOtherTable == AConstraint.strOtherTable))
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
            int Count = 0;

            foreach (TConstraint OtherLinkConstraint in OtherLinkConstraints)
            {
                TTable OtherTable = AStore.GetTable(OtherLinkConstraint.strOtherTable);
                string ProcedureName = "Via" + TTable.NiceTableName(OtherTable.strName);

                // check if other foreign key exists that references the same table, e.g.
                // PPartnerAccess.LoadViaSUserPRecentPartners
                // PPartnerAccess.LoadViaSUserPCustomisedGreeting
                // DirectReferences necessary for PPersonAccess.LoadViaPUnit (p_om_field_key_n) and PPersonAccess.LoadViaPUnitPmGeneralApplication
                if (FindOtherConstraintSameOtherTable2(OtherLinkConstraints,
                        OtherLinkConstraint)
                    || FindOtherConstraintSameOtherTable2(DirectReferences, OtherLinkConstraint))
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
                string csvListPrimaryKeyFields;
                string formalParametersOtherPrimaryKey;
                string actualParametersOtherPrimaryKey;
                
                PrepareCodeletsPrimaryKey(OtherTable,
                        out notUsed,
                        out formalParametersOtherPrimaryKey,
                        out actualParametersOtherPrimaryKey,
                        out notUsed,
                        out notUsed);

                PrepareCodeletsPrimaryKey(ACurrentTable,
                        out csvListPrimaryKeyFields,
                        out notUsed,
                        out notUsed,
                        out notUsed,
                        out notUsed);
                
                string whereClauseForeignKey;
                string whereClauseViaOtherTable;
                string odbcParametersForeignKey;
                PrepareCodeletsForeignKey(
                        OtherTable,
                        OtherLinkConstraint,
                        out whereClauseForeignKey,
                        out whereClauseViaOtherTable,
                        out odbcParametersForeignKey);
                
                string whereClauseViaLinkTable;
                string whereClauseAllViaTables;
                PrepareCodeletsViaLinkTable(
                    (TConstraint)References[Count],
                    OtherLinkConstraint,
                    out whereClauseViaLinkTable,
                    out whereClauseAllViaTables);

                snippetLinkTable.SetCodelet("CSVLISTPRIMARYKEYFIELDS", csvListPrimaryKeyFields);
                snippetLinkTable.SetCodelet("FORMALPARAMETERSOTHERPRIMARYKEY", formalParametersOtherPrimaryKey);
                snippetLinkTable.SetCodelet("ACTUALPARAMETERSOTHERPRIMARYKEY", actualParametersOtherPrimaryKey);
                snippetLinkTable.SetCodelet("ODBCPARAMETERSFOREIGNKEY", odbcParametersForeignKey);
                snippetLinkTable.SetCodelet("WHERECLAUSEVIALINKTABLE", whereClauseViaLinkTable);
                snippetLinkTable.SetCodelet("WHERECLAUSEALLVIATABLES", whereClauseAllViaTables);
                 
                ASnippet.InsertSnippet("VIALINKTABLE", snippetLinkTable);
                
                Count = Count + 1;
            }
        }

        private static void InsertMainProcedures(TDataDefinitionStore AStore, TTable ACurrentTable, ProcessTemplate ATemplate, ProcessTemplate ASnippet)
        {
            ASnippet.AddToCodelet("TABLENAME", TTable.NiceTableName(ACurrentTable.strName));
            ASnippet.AddToCodelet("TABLE_DESCRIPTION", ACurrentTable.strDescription);
            ASnippet.AddToCodelet("SQLTABLENAME", ACurrentTable.strName);
            ASnippet.AddToCodelet("VIAOTHERTABLE", "");
            ASnippet.AddToCodelet("VIALINKTABLE", "");

            string csvListPrimaryKeyFields;
            string formalParametersPrimaryKey;
            string actualParametersPrimaryKey;
            string whereClausePrimaryKey;
            string odbcParametersPrimaryKey;

            PrepareCodeletsPrimaryKey(ACurrentTable,
                    out csvListPrimaryKeyFields,
                    out formalParametersPrimaryKey,
                    out actualParametersPrimaryKey,
                    out whereClausePrimaryKey,
                    out odbcParametersPrimaryKey);
            
            ASnippet.AddToCodelet("CSVLISTPRIMARYKEYFIELDS", csvListPrimaryKeyFields);
            ASnippet.AddToCodelet("FORMALPARAMETERSPRIMARYKEY", formalParametersPrimaryKey);
            ASnippet.AddToCodelet("ACTUALPARAMETERSPRIMARYKEY", actualParametersPrimaryKey);
            ASnippet.AddToCodelet("WHERECLAUSEPRIMARYKEY", whereClausePrimaryKey);
            ASnippet.AddToCodelet("ODBCPARAMETERSPRIMARYKEY", odbcParametersPrimaryKey);
                    
            foreach (TTableField tablefield in ACurrentTable.grpTableField.List)
            {
                // is there a field filled by a sequence?
                // yes: get the next value of that sequence and assign to row
                if (tablefield.strSequence.Length > 0)
                {
                    ASnippet.AddToCodelet("SEQUENCECAST", "");
                    if (codeGenerationPetra.ToDelphiType(tablefield) != "Int64")
                    {
                        ASnippet.AddToCodelet("SEQUENCECAST", "(" + codeGenerationPetra.ToDelphiType(tablefield) + ")");
                    }
                    ASnippet.AddToCodelet("SEQUENCEFIELD", TTable.NiceFieldName(tablefield));
                    ASnippet.AddToCodelet("SEQUENCENAME", tablefield.strSequence);

                    // assume only one sequence per table
                    break;
                }
            }
        }
        
        public static Boolean WriteTypedDataAccess(TDataDefinitionStore AStore,
            string strGroup,
            string AFilePath,
            string ANamespaceName,
            string AFilename)
        {
            Console.WriteLine("processing namespace PetraTypedDataAccess." + strGroup.Substring(0, 1).ToUpper() + strGroup.Substring(1));

            TAppSettingsManager opts = new TAppSettingsManager(false);
            string templateDir = opts.GetValue("TemplateDir", true);
            ProcessTemplate Template = new ProcessTemplate(templateDir + Path.DirectorySeparatorChar +
                                                           "ORM" + Path.DirectorySeparatorChar + 
                                                           "DataAccess.cs");

            Template.InsertSnippet("FILEHEADER", Template.GetSnippet("FILEHEADER"));

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
                    Template.AddToCodelet("USINGNAMESPACES", 
                                  GetNamespace(currentTable.strGroup), false);

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