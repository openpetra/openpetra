//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2013 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Ict.Tools.FilterButtonWiki
{
    class Program
    {
        // Lists of YAML files that use the specified WinForms template
        private static List <string>FListWindowEditUIConnector = new List <string>();
        private static List <string>FListWindowEditWebConnectorMasterDetail = new List <string>();
        private static List <string>FListWindowEdit = new List <string>();
        private static List <string>FListWindowTDS = new List <string>();
        private static List <string>FListWindowMaintainTable = new List <string>();
        private static List <string>FListWindowMaintainCacheableTable = new List <string>();
        private static List <string>FListControlMaintainTable = new List <string>();
        private static List <string>FListControlMaintainCacheableTable = new List <string>();
        private static List <string>FListWindowSingleTable = new List <string>();
        private static List <string>FListControlMaintainTableWithDataViewGrid = new List <string>();
        private static List <string>FListWindowBrowsePrint = new List <string>();
        private static List <string>FListMasterAndDetails = new List <string>();

        // various global variables
        private static string FBaseClientPath;
        private static int FGridCount;
        private static int FPnlDetailsCount;
        private static int FPnlButtonsCount;
        private static int FCountWithAll;
        private static StreamWriter FLogFile;

        private static void CheckForIssues(List <string>AListToCheck, string ATitle)
        {
            FLogFile.WriteLine("");
            FLogFile.WriteLine(
                "=== Screens Based on Template: {0} ({1} files use this template). ===",
                ATitle,
                AListToCheck.Count);
            FLogFile.WriteLine(
                "The table listings consist of only those files that have a grid, a details panel and a buttons panel.");
            FLogFile.WriteLine("{| border=\"1\" cellpadding=\"5\" cellspacing=\"0\"");

            FLogFile.WriteLine("!Filename !! Has Grid !! Has Details !! Has Buttons !! Has All !! Comments");

            foreach (string tryPath in AListToCheck)
            {
                CheckForIssues(tryPath);
            }

            FLogFile.WriteLine("|}");
        }

        private static void CheckForIssues(string AYAMLPath)
        {
            using (StreamReader srYAML = new StreamReader(AYAMLPath))
            {
                string yml = srYAML.ReadToEnd();

                string shortYAMLPath = AYAMLPath.Substring(FBaseClientPath.Length + 1);

                bool bHasGrid = yml.Contains("grdDetails:");
                bool bHasDetails = yml.Contains("pnlDetails:");
                bool bHasButtons = yml.Contains("pnlButtons:") || yml.Contains("pnlDetailButtons");

                if (!bHasGrid && !bHasDetails && !bHasButtons)
                {
                    return;
                }

                if (bHasGrid)
                {
                    FGridCount++;
                }

                if (bHasDetails)
                {
                    FPnlDetailsCount++;
                }

                if (bHasButtons)
                {
                    FPnlButtonsCount++;
                }

                if (bHasButtons && bHasDetails && bHasGrid)
                {
                    FCountWithAll++;
                }

                // Start new row: col1 is filename
                FLogFile.WriteLine("|-");
                FLogFile.WriteLine("|{0}", shortYAMLPath);

                // Col2: HasGrid, Col3: Haspnldetails, Col4: Has pnlButtons, Col5: HasAll
                FLogFile.WriteLine(bHasGrid ? "|Yes" : "|No");
                FLogFile.WriteLine(bHasDetails ? "|Yes" : "|No");
                FLogFile.WriteLine(bHasButtons ? "|Yes" : "|No");
                FLogFile.WriteLine(bHasGrid && bHasButtons && bHasDetails ? "|Yes" : "|No");
                FLogFile.WriteLine("|");
            }
        }

        /// <summary>
        /// Main entry point for the application
        /// </summary>
        static void Main(/* string[] args */)
        {
            // Get the path to me
            string PathToMe = System.Reflection.Assembly.GetExecutingAssembly().Location;
            int pos = PathToMe.IndexOf(@"\delivery");
            string rootPath = PathToMe.Substring(0, pos);

            // The output file is in the OpenPetra log folder
            using (FLogFile = new StreamWriter(Path.Combine(rootPath, @"log\FilterButtonWiki.txt")))
            {
                FLogFile.WriteLine("== Filter Button Implementation ==");
                FLogFile.WriteLine(
                    "Analysis run at {0} on {1}.<br/>All the files listed below have at least one of: a Details Grid, a Details Panel or a Buttons Panel",
                    DateTime.Now.ToShortTimeString(),
                    DateTime.Now.ToShortDateString());

                FBaseClientPath = Path.Combine(rootPath, @"csharp\ICT\Petra\Client");
                string[] allYAML = Directory.GetFiles(FBaseClientPath, "*.yaml", SearchOption.AllDirectories);

                foreach (string tryPath in allYAML)
                {
                    using (StreamReader sr = new StreamReader(tryPath))
                    {
                        string content = sr.ReadToEnd();

                        if (content.Contains("windowEditUIConnector"))
                        {
                            FListWindowEditUIConnector.Add(tryPath);
                        }
                        else if (content.Contains("windowEditWebConnectorMasterDetail"))
                        {
                            FListWindowEditWebConnectorMasterDetail.Add(tryPath);
                        }
                        else if (content.Contains("windowEdit"))
                        {
                            FListWindowEdit.Add(tryPath);
                        }
                        else if (content.Contains("windowTDS"))
                        {
                            FListWindowTDS.Add(tryPath);
                        }
                        else if (content.Contains("windowMaintainTable"))
                        {
                            FListWindowMaintainTable.Add(tryPath);
                        }
                        else if (content.Contains("windowMaintainCacheableTable"))
                        {
                            FListWindowMaintainCacheableTable.Add(tryPath);
                        }
                        else if (content.Contains("controlMaintainTable"))
                        {
                            FListControlMaintainTable.Add(tryPath);
                        }
                        else if (content.Contains("controlMaintainCachableTable"))
                        {
                            FListControlMaintainCacheableTable.Add(tryPath);
                        }
                        else if (content.Contains("windowSingleTable"))
                        {
                            FListWindowSingleTable.Add(tryPath);
                        }
                        else if (content.Contains("controlMaintainTableWithDataViewGrid"))
                        {
                            FListControlMaintainTableWithDataViewGrid.Add(tryPath);
                        }
                        else if (content.Contains("windowBrowsePrint"))
                        {
                            FListWindowBrowsePrint.Add(tryPath);
                        }
                        else if (content.Contains("MasterTable") && content.Contains("DetailTable"))
                        {
                            FListMasterAndDetails.Add(tryPath);
                        }

                        sr.Close();
                    }
                }

                // Now do each one in turn, looking for issues in the manual code
                FCountWithAll = 0;
                FGridCount = 0;
                FPnlDetailsCount = 0;
                FPnlButtonsCount = 0;

                CheckForIssues(FListWindowEdit, "WindowEdit");
                CheckForIssues(FListWindowTDS, "WindowTDS");
                CheckForIssues(FListWindowMaintainTable, "WindowMaintainTable");
                CheckForIssues(FListWindowMaintainCacheableTable, "WindowMaintainCacheableTable");
                CheckForIssues(FListWindowEditUIConnector, "WindowEditUIConnector");
                CheckForIssues(FListWindowEditWebConnectorMasterDetail, "WindowEditWebConnectorMasterDetail");
                CheckForIssues(FListControlMaintainTable, "ControlMaintainTable");
                CheckForIssues(FListControlMaintainCacheableTable, "ControlMaintainCacheableTable");
                CheckForIssues(FListWindowSingleTable, "WindowSingleTable");
                CheckForIssues(FListWindowBrowsePrint, "WindowBrowsePrint");
                CheckForIssues(FListControlMaintainTableWithDataViewGrid, "ControlMaintainTableWithDataViewGrid");

                FLogFile.WriteLine("");
                FLogFile.WriteLine("=== Summary ===");
                FLogFile.WriteLine("  {0} screens with grdDetails", FGridCount);
                FLogFile.WriteLine("  {0} screens with pnlDetails", FPnlDetailsCount);
                FLogFile.WriteLine("  {0} screens with pnlButtons", FPnlButtonsCount);
                FLogFile.WriteLine("  {0} screens have all the above", FCountWithAll);

                FLogFile.Close();
            }
        }

        private static string GetYAMLAttribute(string AYAMLText, string AAttributeName)
        {
            string ret = String.Empty;

            int pos = AYAMLText.IndexOf(AAttributeName + ":");

            if (pos >= 0)
            {
                int posValue = pos + AAttributeName.Length + 1;
                int posCrlf = AYAMLText.IndexOf(Environment.NewLine, posValue);
                ret = AYAMLText.Substring(posValue, posCrlf - posValue).Trim();
            }

            return ret;
        }
    }
}