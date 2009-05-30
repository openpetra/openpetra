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
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;
using Ict.Common;


namespace Ict.Tools.CodeGeneration.DataStore
{
    /// This program generates the tables and datasets for the typed datasets
    public class generateTypedTables
    {
        public static TDataDefinitionParser parser;
        public static TDataDefinitionStore store;
        public static TCmdOpts cmdLine;

        public static Boolean WriteTypedTable(TDataDefinitionStore store, string strGroup, string AFilePath, string ANamespaceName, string AFilename)
        {
            FileStream outFile;
            TextWriter tw;
            string rowName;
            string tableName;
            String OutFileName;
            CodeNamespace cns;

            Console.WriteLine("processing namespace PetraTypedDataSet." + strGroup.Substring(0, 1).ToUpper() + strGroup.Substring(1));
            OutFileName = AFilePath + AFilename + ".cs";
            outFile = new  FileStream(OutFileName + ".new", FileMode.Create, FileAccess.Write);

            if (outFile == null)
            {
                return false;
            }

            CSharpCodeProvider gen = new CSharpCodeProvider();
            cns = new CodeNamespace(ANamespaceName.Substring(0, ANamespaceName.Length - ".Tables".Length));
            tw = new StreamWriter(outFile);
            tw.WriteLine("/* Auto generated with nant generateORM");
            tw.WriteLine(" * Do not modify this file manually!");
            tw.WriteLine(" */");
            codeGenerationPetra.AddImports(cns);

            foreach (TTable currentTable in store.GetTables())
            {
                rowName = TTable.NiceTableName(currentTable.strName) + "Row";
                tableName = TTable.NiceTableName(currentTable.strName) + "Table";

                if (currentTable.strGroup == strGroup)
                {
                    cns.Types.Add(codeGenerationTable.Table(currentTable, TTable.NiceTableName(currentTable.strName), tableName, rowName));
                    cns.Types.Add(codeGenerationRow.Row(currentTable, tableName, rowName));
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

        public static Boolean WriteSortedTableList(TDataDefinitionStore store, string AFilePath)
        {
            FileStream outFile;
            TextWriter tw;
            String OutFileName;
            CodeNamespace cns;
            CodeMemberMethod FillSortedList;
            CodeMemberField list;
            CodeTypeDeclaration t;
            String DLLName = "";

            Console.WriteLine("writing the SortedList of tables/dll association");
            OutFileName = AFilePath + "/TableList.cs";
            outFile = new FileStream(OutFileName + ".new", FileMode.Create, FileAccess.Write);

            if (outFile == null)
            {
                return false;
            }

            CSharpCodeProvider gen = new CSharpCodeProvider();
            cns = new CodeNamespace("Ict.Petra.Shared.DataStore.TableList");
            tw = new StreamWriter(outFile);
            tw.WriteLine("/* Auto generated with nant generateORM");
            tw.WriteLine(" * Do not modify this file manually!");
            tw.WriteLine(" */");
            cns.Imports.Add(new CodeNamespaceImport("System"));
            cns.Imports.Add(new CodeNamespaceImport("System.Collections"));

            // todo: add the other things in other units, for retrieving the type and calling methods on it
            FillSortedList = new CodeMemberMethod();
            FillSortedList.Name = "FillSortedListTables";
            FillSortedList.Comments.Add(new CodeCommentStatement("auto generated", true));
            FillSortedList.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            FillSortedList.Statements.Add(CodeDom.Let(CodeDom.Local("FTableList"), CodeDom._New("SortedList", new CodeExpression[] { })));

            foreach (TTable currentTable in store.GetTables())
            {
                if (currentTable.strGroup == "partner")
                {
                    DLLName = "MPartner.Partner";
                }
                else if (currentTable.strGroup == "mailroom")
                {
                    DLLName = "MPartner.Mailroom";
                }
                else if (currentTable.strGroup == "personnel")
                {
                    DLLName = "MPersonnel.Personnel";
                }
                else if (currentTable.strGroup == "units")
                {
                    DLLName = "MPersonnel.Units";
                }
                else if (currentTable.strGroup == "conference")
                {
                    DLLName = "MConference";
                }
                else if (currentTable.strGroup == "hospitality")
                {
                    DLLName = "MHospitality";
                }
                else if (currentTable.strGroup == "account")
                {
                    DLLName = "MFinance.Account";
                }
                else if (currentTable.strGroup == "ap")
                {
                    DLLName = "MFinance.AP";
                }
                else if (currentTable.strGroup == "ar")
                {
                    DLLName = "MFinance.AR";
                }
                else if (currentTable.strGroup == "gift")
                {
                    DLLName = "MFinance.Gift";
                }
                else if (currentTable.strGroup == "sysman")
                {
                    DLLName = "MSysMan";
                }
                else if (currentTable.strGroup == "main")
                {
                    DLLName = "MCommon";
                }

                FillSortedList.Statements.Add(CodeDom.MethodInvoke(CodeDom.Local("FTableList"), "Add", new CodeExpression[]
                        { CodeDom._Const(TTable.NiceTableName(currentTable.strName)),
                          CodeDom._Const(DLLName
                              ) }));
            }

            list = new CodeMemberField("SortedList", "FTableList");
            list.Comments.Add(new CodeCommentStatement("auto generated", true));
            list.Attributes = MemberAttributes.Family | MemberAttributes.Static;
            t = new CodeTypeDeclaration("TTableList");
            t.Comments.Add(new CodeCommentStatement("auto generated", true));
            t.IsClass = true;
            t.Members.Add(list);
            t.Members.Add(FillSortedList);
            cns.Types.Add(t);
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

        void run()
        {
            cmdLine = new TCmdOpts();

            if (!cmdLine.IsFlagSet("do")
                || (cmdLine.GetOptValue("do") == "dataset")
                && (!cmdLine.IsFlagSet("input") || !cmdLine.IsFlagSet("outputNamespace")))
            {
                System.Console.WriteLine("GenerateORM: generate Typed Tables and Datasets");
                System.Console.WriteLine("usage: GenerateORM -do:<operation> -petraxml:<xyz/petra.xml>");
                System.Console.WriteLine("operations available:");
                System.Console.WriteLine("  defaulttables ");
                System.Console.WriteLine("           with parameters: ");
                System.Console.WriteLine("                 -outputshared:<path to ICT\\Petra\\Shared>");
                System.Console.WriteLine("  dataaccess ");
                System.Console.WriteLine("           with parameters: ");
                System.Console.WriteLine("                 -outputshared:<path to ICT\\Petra\\Shared>");
                System.Console.WriteLine("  dataset ");
                System.Console.WriteLine("           with parameters: ");
                System.Console.WriteLine("                 -input:<dataset XML file>");
                System.Console.WriteLine("                 -outputNamespace:<Namespace of the file>");
                System.Console.WriteLine(
                    "       e.g. GenerateORM -do:dataset -petraxml:U:/sql/datadefinition/petra.xml -input:U:/sql/datadefinition/dataset.xml -outputNamespace:Ict.Petra.Shared.MCommon.Data.Dataset");
                return;
            }

            try
            {
                parser = new TDataDefinitionParser(cmdLine.GetOptValue("petraxml"));
                store = new TDataDefinitionStore();

                if (parser.ParseDocument(ref store))
                {
                    if ((cmdLine.GetOptValue("do") == "defaulttables") || (cmdLine.GetOptValue("do") == "datatables"))
                    {
                        WriteSortedTableList(store, cmdLine.GetOptValue("outputshared"));
                        WriteTypedTable(store, "partner", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MPartner\\data\\",
                            "Ict.Petra.Shared.MPartner.Partner.Data.Tables",
                            "Partner.Tables");
                        WriteTypedTable(store, "mailroom", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MPartner\\data\\",
                            "Ict.Petra.Shared.MPartner.Mailroom.Data.Tables",
                            "Mailroom.Tables");
                        WriteTypedTable(store, "personnel", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MPersonnel\\data\\",
                            "Ict.Petra.Shared.MPersonnel.Personnel.Data.Tables",
                            "Personnel.Tables");
                        WriteTypedTable(store, "units", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MPersonnel\\data\\",
                            "Ict.Petra.Shared.MPersonnel.Units.Data.Tables",
                            "Units.Tables");
                        WriteTypedTable(store, "conference", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MConference\\data\\",
                            "Ict.Petra.Shared.MConference.Data.Tables",
                            "Conference.Tables");
                        WriteTypedTable(store, "hospitality", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MHospitality\\data\\",
                            "Ict.Petra.Shared.MHospitality.Data.Tables",
                            "Hospitality.Tables");
                        WriteTypedTable(store, "account", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MFinance\\data\\",
                            "Ict.Petra.Shared.MFinance.Account.Data.Tables",
                            "Account.Tables");
                        WriteTypedTable(store, "ap", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MFinance\\data\\",
                            "Ict.Petra.Shared.MFinance.AP.Data.Tables",
                            "AP.Tables");
                        WriteTypedTable(store, "ar", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MFinance\\data\\",
                            "Ict.Petra.Shared.MFinance.AR.Data.Tables",
                            "AR.Tables");
                        WriteTypedTable(store, "gift", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MFinance\\data\\",
                            "Ict.Petra.Shared.MFinance.Gift.Data.Tables",
                            "Gift.Tables");
                        WriteTypedTable(store, "sysman", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MSysMan\\data\\",
                            "Ict.Petra.Shared.MSysMan.Data.Tables",
                            "SysMan.Tables");
                        WriteTypedTable(store, "main", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MCommon\\data\\",
                            "Ict.Petra.Shared.MCommon.Data.Tables",
                            "Common.Tables");
                    }
                    else if (cmdLine.GetOptValue("do") == "dataaccess")
                    {
                        codeGenerationAccess.WriteTypedDataAccess(store, "partner", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MPartner\\data\\",
                            "Ict.Petra.Shared.MPartner.Partner.Data.Access",
                            "Partner.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "mailroom", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MPartner\\data\\",
                            "Ict.Petra.Shared.MPartner.Mailroom.Data.Access",
                            "Mailroom.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "personnel", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MPersonnel\\data\\",
                            "Ict.Petra.Shared.MPersonnel.Personnel.Data.Access",
                            "Personnel.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "units", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MPersonnel\\data\\",
                            "Ict.Petra.Shared.MPersonnel.Units.Data.Access",
                            "Units.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "conference", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MConference\\data\\",
                            "Ict.Petra.Shared.MConference.Data.Access",
                            "Conference.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "hospitality", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MHospitality\\data\\",
                            "Ict.Petra.Shared.MHospitality.Data.Access",
                            "Hospitality.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "account", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MFinance\\data\\",
                            "Ict.Petra.Shared.MFinance.Account.Data.Access",
                            "Account.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "ap", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MFinance\\data\\",
                            "Ict.Petra.Shared.MFinance.AP.Data.Access",
                            "AP.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "ar", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MFinance\\data\\",
                            "Ict.Petra.Shared.MFinance.AR.Data.Access",
                            "AR.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "gift", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MFinance\\data\\",
                            "Ict.Petra.Shared.MFinance.Gift.Data.Access",
                            "Gift.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "sysman", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MSysMan\\data\\",
                            "Ict.Petra.Shared.MSysMan.Data.Access",
                            "SysMan.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "main", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MCommon\\data\\",
                            "Ict.Petra.Shared.MCommon.Data.Access",
                            "Common.Access");
                        codeGenerationCascading.WriteTypedDataCascading(store, cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MCommon\\data\\",
                            "Ict.Petra.Shared.MCommon.Data.Cascading",
                            "Cascading");
                    }
                    else if (cmdLine.GetOptValue("do") == "dataset")
                    {
                        string[] groups = new string[] {
                            "main", "mailroom", "sysman", "partner",
                            "account", "gift", "ap", "ar", "personnel", "units", "conference", "hospitality"
                        };

                        codeGenerationDataset.CreateTypedDataSets(cmdLine.GetOptValue("input"),
                            System.IO.Path.GetDirectoryName(cmdLine.GetOptValue("input")),
                            cmdLine.GetOptValue("outputNamespace"),
                            store, groups,
                            cmdLine.GetOptValue("outputFilename"));
                    }
                    else
                    {
                        Console.WriteLine("could not recognise: " + cmdLine.GetOptValue("do"));
                    }
                }
            }
            catch (Exception E)
            {
                System.Console.WriteLine(E.Message);
                System.Console.WriteLine(E.StackTrace);

                // System.Console.ReadLine();
            }
        }

        public static void Main(string[] args)
        {
            generateTypedTables myApp = new generateTypedTables();

            myApp.run();
        }
    }
}