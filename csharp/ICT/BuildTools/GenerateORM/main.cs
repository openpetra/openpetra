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
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;
using Ict.Common;
using Ict.Tools.CodeGeneration.DataStore;
using Ict.Tools.CodeGeneration.ReferenceCountConnectors;

namespace Ict.Tools.GenerateORM
{
    /// This program generates the tables and datasets for the typed datasets
    public class generateTypedTables
    {
        /// <summary>
        /// a parser for the petra.xml, which contains the database definition for OpenPetra
        /// </summary>
        public static TDataDefinitionParser parser;
        /// <summary>
        /// the data from the petra.xml with the database definition
        /// </summary>
        public static TDataDefinitionStore store;
        /// <summary>
        /// the options from the command line
        /// </summary>
        public static TCmdOpts cmdLine;

        int run()
        {
            new TAppSettingsManager(false);

            cmdLine = new TCmdOpts();

            if (!cmdLine.IsFlagSet("do")
                || ((cmdLine.GetOptValue("do") == "dataset") && (!cmdLine.IsFlagSet("input") || !cmdLine.IsFlagSet("outputNamespace")))
                || ((cmdLine.GetOptValue("do") == "referencecount")
                    && (!cmdLine.IsFlagSet("inputclient") || !cmdLine.IsFlagSet("outputserver") || !cmdLine.IsFlagSet("templatedir"))))
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
                System.Console.WriteLine("  cachedtables ");
                System.Console.WriteLine("           with parameters: ");
                System.Console.WriteLine("                 -cachedef:<dataset XML file>");
                System.Console.WriteLine("                 -outputshared:<path to ICT\\Petra\\Shared>");
                System.Console.WriteLine("  referencecount ");
                System.Console.WriteLine("           with parameters: ");
                System.Console.WriteLine("                 -inputclient:<path to ICT\\Petra\\Client>");
                System.Console.WriteLine("                 -outputserver:<path to ICT\\Petra\\Server>");
                System.Console.WriteLine("                 -templatedir:<path to inc\\template\\src>");
                System.Console.WriteLine(
                    "       e.g. GenerateORM -do:dataset -petraxml:U:/sql/datadefinition/petra.xml -input:U:/sql/datadefinition/dataset.xml -outputNamespace:Ict.Petra.Shared.MCommon.Data.Dataset");
                return 100;
            }

            try
            {
                if (cmdLine.GetOptValue("do") == "referencecount")
                {
                    // No need to parse the petra.xml document for this task - so we just run and exit
                    new TLogging();
                    TCreateReferenceCountConnectors createConnectors = new TCreateReferenceCountConnectors();
                    createConnectors.CreateFiles(cmdLine.GetOptValue("outputserver"), cmdLine.GetOptValue("inputclient"),
                        cmdLine.GetOptValue("templatedir"));

                    return 0;
                }

                parser = new TDataDefinitionParser(cmdLine.GetOptValue("petraxml"));
                store = new TDataDefinitionStore();

                if (parser.ParseDocument(ref store))
                {
                    if ((cmdLine.GetOptValue("do") == "defaulttables") || (cmdLine.GetOptValue("do") == "datatables"))
                    {
                        CodeGenerationTable.WriteTypedTable(store, "partner", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MPartner.Partner.Data",
                            "Partner.Tables");
                        CodeGenerationTable.WriteTypedTable(store, "mailroom", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MPartner.Mailroom.Data",
                            "Mailroom.Tables");
                        CodeGenerationTable.WriteTypedTable(store, "personnel", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MPersonnel.Personnel.Data",
                            "Personnel.Tables");
                        CodeGenerationTable.WriteTypedTable(store, "units", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MPersonnel.Units.Data",
                            "Units.Tables");
                        CodeGenerationTable.WriteTypedTable(store, "conference", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MConference.Data",
                            "Conference.Tables");
                        CodeGenerationTable.WriteTypedTable(store, "hospitality", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MHospitality.Data",
                            "Hospitality.Tables");
                        CodeGenerationTable.WriteTypedTable(store, "account", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MFinance.Account.Data",
                            "Account.Tables");
                        CodeGenerationTable.WriteTypedTable(store, "ap", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MFinance.AP.Data",
                            "AP.Tables");
                        CodeGenerationTable.WriteTypedTable(store, "ar", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MFinance.AR.Data",
                            "AR.Tables");
                        CodeGenerationTable.WriteTypedTable(store, "gift", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MFinance.Gift.Data",
                            "Gift.Tables");
                        CodeGenerationTable.WriteTypedTable(store, "sysman", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MSysMan.Data",
                            "SysMan.Tables");
                        CodeGenerationTable.WriteTypedTable(store, "common", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MCommon.Data",
                            "Common.Tables");

                        CodeGenerationTableValidation.WriteValidation(store, "partner", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MPartner\\validation\\",
                            "Ict.Petra.Shared.MPartner.Partner.Validation",
                            "Partner.Validation");
                        CodeGenerationTableValidation.WriteValidation(store, "mailroom", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MPartner\\validation\\",
                            "Ict.Petra.Shared.MPartner.Mailroom.Validation",
                            "Mailroom.Validation");
                        CodeGenerationTableValidation.WriteValidation(store, "personnel", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MPersonnel\\validation\\",
                            "Ict.Petra.Shared.MPersonnel.Personnel.Validation",
                            "Personnel.Validation");
                        CodeGenerationTableValidation.WriteValidation(store, "units", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MPersonnel\\validation\\",
                            "Ict.Petra.Shared.MPersonnel.Units.Validation",
                            "Units.Validation");
                        CodeGenerationTableValidation.WriteValidation(store, "conference", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MConference\\validation\\",
                            "Ict.Petra.Shared.MConference.Validation",
                            "Conference.Validation");
                        CodeGenerationTableValidation.WriteValidation(store, "hospitality", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MHospitality\\validation\\",
                            "Ict.Petra.Shared.MHospitality.Validation",
                            "Hospitality.Validation");
                        CodeGenerationTableValidation.WriteValidation(store, "account", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MFinance\\validation\\",
                            "Ict.Petra.Shared.MFinance.Account.Validation",
                            "Account.Validation");
                        CodeGenerationTableValidation.WriteValidation(store, "ap", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MFinance\\validation\\",
                            "Ict.Petra.Shared.MFinance.AP.Validation",
                            "AP.Validation");
                        CodeGenerationTableValidation.WriteValidation(store, "ar", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MFinance\\validation\\",
                            "Ict.Petra.Shared.MFinance.AR.Validation",
                            "AR.Validation");
                        CodeGenerationTableValidation.WriteValidation(store, "gift", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MFinance\\validation\\",
                            "Ict.Petra.Shared.MFinance.Gift.Validation",
                            "Gift.Validation");
                        CodeGenerationTableValidation.WriteValidation(store, "sysman", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MSysMan\\validation\\",
                            "Ict.Petra.Shared.MSysMan.Validation",
                            "SysMan.Validation");
                        CodeGenerationTableValidation.WriteValidation(store, "common", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\MCommon\\validation\\",
                            "Ict.Petra.Shared.MCommon.Validation",
                            "Common.Validation");

                        TGenerateTableList.WriteTableList(store, cmdLine.GetOptValue("outputshared") + Path.DirectorySeparatorChar + "TableList.cs");
                        TGenerateTableList.WriteDBClean(store, Path.GetDirectoryName(cmdLine.GetOptValue(
                                    "petraxml")) + Path.DirectorySeparatorChar + "basedata" + Path.DirectorySeparatorChar + "clean.sql");
                    }
                    else if (cmdLine.GetOptValue("do") == "dataaccess")
                    {
                        CodeGenerationAccess.WriteTypedDataAccess(store, "partner", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MPartner.Partner.Data.Access",
                            "Partner.Access");
                        CodeGenerationAccess.WriteTypedDataAccess(store, "mailroom", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MPartner.Mailroom.Data.Access",
                            "Mailroom.Access");
                        CodeGenerationAccess.WriteTypedDataAccess(store, "personnel", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MPersonnel.Personnel.Data.Access",
                            "Personnel.Access");
                        CodeGenerationAccess.WriteTypedDataAccess(store, "units", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MPersonnel.Units.Data.Access",
                            "Units.Access");
                        CodeGenerationAccess.WriteTypedDataAccess(store, "conference", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MConference.Data.Access",
                            "Conference.Access");
                        CodeGenerationAccess.WriteTypedDataAccess(store, "hospitality", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MHospitality.Data.Access",
                            "Hospitality.Access");
                        CodeGenerationAccess.WriteTypedDataAccess(store, "account", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MFinance.Account.Data.Access",
                            "Account.Access");
                        CodeGenerationAccess.WriteTypedDataAccess(store, "ap", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MFinance.AP.Data.Access",
                            "AP.Access");
                        CodeGenerationAccess.WriteTypedDataAccess(store, "ar", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MFinance.AR.Data.Access",
                            "AR.Access");
                        CodeGenerationAccess.WriteTypedDataAccess(store, "gift", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MFinance.Gift.Data.Access",
                            "Gift.Access");
                        CodeGenerationAccess.WriteTypedDataAccess(store, "sysman", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MSysMan.Data.Access",
                            "SysMan.Access");
                        CodeGenerationAccess.WriteTypedDataAccess(store, "common", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MCommon.Data.Access",
                            "Common.Access");
                        CodeGenerationCascading.WriteTypedDataCascading(store, cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MCommon.Data.Cascading",
                            "Cascading");
                    }
                    else if (cmdLine.GetOptValue("do") == "dataset")
                    {
                        string[] groups = new string[] {
                            "common", "mailroom", "sysman", "partner",
                            "account", "gift", "ap", "ar", "personnel", "units", "conference", "hospitality"
                        };

                        CodeGenerationDataset.CreateTypedDataSets(cmdLine.GetOptValue("input"),
                            cmdLine.GetOptValue("outputdir"),
                            cmdLine.GetOptValue("outputNamespace"),
                            store, groups,
                            cmdLine.GetOptValue("outputFilename"));
                    }
                    else if (cmdLine.GetOptValue("do") == "datasetaccess")
                    {
                        string[] groups = new string[] {
                            "common", "mailroom", "sysman", "partner",
                            "account", "gift", "ap", "ar", "personnel", "units", "conference", "hospitality"
                        };

                        CodeGenerationDatasetAccess.CreateTypedDataSets(cmdLine.GetOptValue("input"),
                            cmdLine.GetOptValue("outputdir"),
                            cmdLine.GetOptValue("outputNamespace"),
                            store, groups,
                            cmdLine.GetOptValue("outputFilename"));
                    }
                    else if (cmdLine.GetOptValue("do") == "cachedtables")
                    {
                        Ict.Tools.CodeGeneration.CachedTables.TGenerateCachedTables.WriteCachedTables(
                            store,
                            cmdLine.GetOptValue("cachedef"),
                            cmdLine.GetOptValue("outputshared"),
                            cmdLine.GetOptValue("TemplateDir"));
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
                return 100;
                // System.Console.ReadLine();
            }
            return 0;
        }

        /// <summary>
        /// main function for program
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Main(string[] args)
        {
            generateTypedTables myApp = new generateTypedTables();

            return myApp.run();
        }
    }
}