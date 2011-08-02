//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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


namespace Ict.Tools.CodeGeneration.DataStore
{
    /// This program generates the tables and datasets for the typed datasets
    public class generateTypedTables
    {
        public static TDataDefinitionParser parser;
        public static TDataDefinitionStore store;
        public static TCmdOpts cmdLine;

        int run()
        {
            new TAppSettingsManager(false);

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
                System.Console.WriteLine("  cachedtables ");
                System.Console.WriteLine("           with parameters: ");
                System.Console.WriteLine("                 -cachedef:<dataset XML file>");
                System.Console.WriteLine("                 -outputshared:<path to ICT\\Petra\\Shared>");
                System.Console.WriteLine(
                    "       e.g. GenerateORM -do:dataset -petraxml:U:/sql/datadefinition/petra.xml -input:U:/sql/datadefinition/dataset.xml -outputNamespace:Ict.Petra.Shared.MCommon.Data.Dataset");
                return 100;
            }

            try
            {
                parser = new TDataDefinitionParser(cmdLine.GetOptValue("petraxml"));
                store = new TDataDefinitionStore();

                if (parser.ParseDocument(ref store))
                {
                    if ((cmdLine.GetOptValue("do") == "defaulttables") || (cmdLine.GetOptValue("do") == "datatables"))
                    {
                        codeGenerationTable.WriteTypedTable(store, "partner", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MPartner.Partner.Data",
                            "Partner.Tables");
                        codeGenerationTable.WriteTypedTable(store, "mailroom", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MPartner.Mailroom.Data",
                            "Mailroom.Tables");
                        codeGenerationTable.WriteTypedTable(store, "personnel", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MPersonnel.Personnel.Data",
                            "Personnel.Tables");
                        codeGenerationTable.WriteTypedTable(store, "units", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MPersonnel.Units.Data",
                            "Units.Tables");
                        codeGenerationTable.WriteTypedTable(store, "conference", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MConference.Data",
                            "Conference.Tables");
                        codeGenerationTable.WriteTypedTable(store, "hospitality", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MHospitality.Data",
                            "Hospitality.Tables");
                        codeGenerationTable.WriteTypedTable(store, "account", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MFinance.Account.Data",
                            "Account.Tables");
                        codeGenerationTable.WriteTypedTable(store, "ap", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MFinance.AP.Data",
                            "AP.Tables");
                        codeGenerationTable.WriteTypedTable(store, "ar", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MFinance.AR.Data",
                            "AR.Tables");
                        codeGenerationTable.WriteTypedTable(store, "gift", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MFinance.Gift.Data",
                            "Gift.Tables");
                        codeGenerationTable.WriteTypedTable(store, "sysman", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MSysMan.Data",
                            "SysMan.Tables");
                        codeGenerationTable.WriteTypedTable(store, "common", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Shared.MCommon.Data",
                            "Common.Tables");
                        TGenerateTableList.WriteTableList(store, cmdLine.GetOptValue("outputshared") + Path.DirectorySeparatorChar + "TableList.cs");
                        TGenerateTableList.WriteDBClean(store, Path.GetDirectoryName(cmdLine.GetOptValue(
                                    "petraxml")) + Path.DirectorySeparatorChar + "basedata" + Path.DirectorySeparatorChar + "clean.sql");
                    }
                    else if (cmdLine.GetOptValue("do") == "dataaccess")
                    {
                        codeGenerationAccess.WriteTypedDataAccess(store, "partner", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MPartner.Partner.Data.Access",
                            "Partner.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "mailroom", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MPartner.Mailroom.Data.Access",
                            "Mailroom.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "personnel", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MPersonnel.Personnel.Data.Access",
                            "Personnel.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "units", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MPersonnel.Units.Data.Access",
                            "Units.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "conference", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MConference.Data.Access",
                            "Conference.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "hospitality", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MHospitality.Data.Access",
                            "Hospitality.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "account", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MFinance.Account.Data.Access",
                            "Account.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "ap", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MFinance.AP.Data.Access",
                            "AP.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "ar", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MFinance.AR.Data.Access",
                            "AR.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "gift", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MFinance.Gift.Data.Access",
                            "Gift.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "sysman", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MSysMan.Data.Access",
                            "SysMan.Access");
                        codeGenerationAccess.WriteTypedDataAccess(store, "common", cmdLine.GetOptValue(
                                "outputshared") + "\\lib\\data\\",
                            "Ict.Petra.Server.MCommon.Data.Access",
                            "Common.Access");
                        codeGenerationCascading.WriteTypedDataCascading(store, cmdLine.GetOptValue(
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

                        codeGenerationDataset.CreateTypedDataSets(cmdLine.GetOptValue("input"),
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

                        codeGenerationDatasetAccess.CreateTypedDataSets(cmdLine.GetOptValue("input"),
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

        public static int Main(string[] args)
        {
            generateTypedTables myApp = new generateTypedTables();

            return myApp.run();
        }
    }
}