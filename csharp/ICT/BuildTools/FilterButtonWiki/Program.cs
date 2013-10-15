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
using System.Xml;

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
        private static int FPnlFilterFindCount;
        private static int FNoFilterFindRequiredCount;
        private static int FCountWithAll;
        private static int FCountWithNoMantisCase;
        private static StreamWriter FLogFile;
        private static SortedList<int, int> FMantisCasesDone = new SortedList <int, int>();
        private static SortedList<int, int> FMantisCasesOpen = new SortedList <int, int>();

        // our metadata Xml
        private static XmlNode FMetaDataComments;
        private static XmlNode FMetaDataMantis;

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

            FLogFile.WriteLine(
                "!Filename !! Has Grid !! Has Details !! Has Buttons !! Has All !! Has Filter/Find !! Has Manual RowFilter !! Mantis !! Comments");

            foreach (string tryPath in AListToCheck)
            {
                CheckForIssues(tryPath);
            }

            FLogFile.WriteLine("|}");
        }

        private static void CheckForIssues(string AYAMLPath)
        {
            string manualPath = AYAMLPath.Replace(".yaml", ".ManualCode.cs");

            if (!File.Exists(manualPath))
            {
                return;
            }

            using (StreamReader srManual = new StreamReader(manualPath))
            using (StreamReader srYAML = new StreamReader(AYAMLPath))
            {
                string yml = srYAML.ReadToEnd();
                string manual = srManual.ReadToEnd();

                string shortYAMLPath = AYAMLPath.Substring(FBaseClientPath.Length + 1);

                bool bHasGrid = yml.Contains("grdDetails:");
                bool bHasDetails = yml.Contains("pnlDetails:");
                bool bHasButtons = yml.Contains("pnlButtons:") || yml.Contains("pnlDetailButtons");
                bool bHasFilterFind = yml.Contains("[pnlFilterAndFind,");
                bool bHasManualRowFilter = manual.Contains(".RowFilter = ");

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

                if (bHasFilterFind)
                {
                    FPnlFilterFindCount++;
                }

                if (bHasButtons && bHasDetails && bHasGrid)
                {
                    FCountWithAll++;
                }

                // Start new row: col1 is filename
                FLogFile.WriteLine("|-");
                FLogFile.WriteLine("|{0}", shortYAMLPath);

                // Col2: HasGrid, Col3: Haspnldetails, Col4: Has pnlButtons, Col5: HasAll, Col6: Has pnlFilterFind, Col7: Manual RowFilter, Col8: Comment
                bool bHasBlueColumn = false;
                string strColContent;
                FLogFile.WriteLine(bHasGrid ? "|Yes" : "|No");
                FLogFile.WriteLine(bHasDetails ? "|Yes" : "|No");
                FLogFile.WriteLine(bHasButtons ? "|Yes" : "|No");

                if (bHasGrid && bHasButtons && bHasDetails)
                {
                    strColContent = "|Yes";
                }
                else
                {
                    strColContent = "|style=\"background:LightSkyBlue\" |No";
                    bHasBlueColumn = true;
                }
                FLogFile.WriteLine(strColContent);

                if (bHasFilterFind)
                {
                    strColContent = "|style=\"background:LightSkyBlue\" |Yes";
                    bHasBlueColumn = true;
                }
                else
                {
                    strColContent = "|No";
                }

                FLogFile.WriteLine(strColContent);
                FLogFile.WriteLine(bHasManualRowFilter ? "|style=\"background:Yellow\" |Yes" : "|No");

                // is there a Mantis Bug?
                FLogFile.Write("|");
                string fileName = Path.GetFileName(AYAMLPath);
                XmlNode mantisFeatureNode = FMetaDataMantis.SelectSingleNode(String.Format("feature[@key='{0}']", fileName));
                string strMantisFeature = mantisFeatureNode == null ? String.Empty : mantisFeatureNode.Attributes["value"].Value;
                FLogFile.WriteLine(strMantisFeature);

                int iFeature;
                if (strMantisFeature == String.Empty)
                {
                    if (bHasGrid && bHasButtons && bHasDetails)
                    {
                        FCountWithNoMantisCase++;
                    }
                }
                else
                {
                    string[] items = strMantisFeature.Split(new char[] { ' ' });
                    foreach (string item in items)
                    {
                        if (Int32.TryParse(item, out iFeature))
                        {
                            if (bHasBlueColumn && !FMantisCasesDone.ContainsKey(iFeature))
                            {
                                FMantisCasesDone.Add(iFeature, iFeature);
                            }
                            else if (!bHasBlueColumn && !FMantisCasesOpen.ContainsKey(iFeature))
                            {
                                FMantisCasesOpen.Add(iFeature, iFeature);
                            }
                        }
                    }
                }

                // is there a comment for this file?
                FLogFile.Write("|");
                XmlNode commentNode = FMetaDataComments.SelectSingleNode(String.Format("comment[@key='{0}']", fileName));
                string strComment = commentNode == null ? String.Empty : commentNode.Attributes["value"].Value;
                FLogFile.WriteLine(strComment);

                if (strComment.Contains("background:LightSkyBlue"))
                {
                    FNoFilterFindRequiredCount++;
                    if (bHasGrid && bHasButtons && bHasDetails)
                    {
                        FCountWithNoMantisCase--;
                    }
                }
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

            XmlDocument xmlDoc = new XmlDocument();
            string metaDataPath = Path.Combine(rootPath, @"csharp\ICT\BuildTools\FilterButtonWiki\wiki.metadata.xml");

            if (File.Exists(metaDataPath))
            {
                xmlDoc.Load(metaDataPath);
            }
            else
            {
                xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><rootnode/>");
            }

            FMetaDataComments = xmlDoc.SelectSingleNode("//comments");
            FMetaDataMantis = xmlDoc.SelectSingleNode("//mantis");

            // The output file is in the OpenPetra log folder
            using (FLogFile = new StreamWriter(Path.Combine(rootPath, @"log\FilterButtonWiki.txt")))
            {
                FLogFile.WriteLine("== Most Recent Filter Button Implementation Information ==");
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
                FPnlFilterFindCount = 0;
                FNoFilterFindRequiredCount = 0;
                FCountWithNoMantisCase = 0;

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
                FLogFile.WriteLine("  {0} screens have Filter/Find", FPnlFilterFindCount);
                FLogFile.WriteLine("  {0} screens do not require Filter/Find", FNoFilterFindRequiredCount);
                FLogFile.WriteLine("  There are {0} screens that are still potential candidates for Filter/Find", FCountWithAll - FPnlFilterFindCount - FNoFilterFindRequiredCount);
                FLogFile.WriteLine();
                FLogFile.WriteLine();
                FLogFile.Write("Mantis cases fixed: ");

                for (int i = 0; i < FMantisCasesDone.Count; i++)
                {
                    FLogFile.Write(FMantisCasesDone.Values[i].ToString());
                    if (i < FMantisCasesDone.Count - 1)
                    {
                        FLogFile.Write(", ");
                    }
                    else
                    {
                        FLogFile.WriteLine();
                        FLogFile.WriteLine();
                    }
                }

                FLogFile.Write("Mantis cases open: ");

                for (int i = 0; i < FMantisCasesOpen.Count; i++)
                {
                    FLogFile.Write(FMantisCasesOpen.Values[i].ToString());
                    if (i < FMantisCasesOpen.Count - 1)
                    {
                        FLogFile.Write(", ");
                    }
                    else
                    {
                        FLogFile.WriteLine();
                        FLogFile.WriteLine();
                    }
                }

                FLogFile.WriteLine("There are {0} screens that have, or could have, Filter/Find that are not associated with a Mantis case", FCountWithNoMantisCase);
                FLogFile.WriteLine();

                // The number in Mantis when searching (Severity: feature + Hide: resolved + Filter: filter) less 4 cases that do not apply (note incl. DIALOG)
                int TotalMantisCases = 74;
                FLogFile.WriteLine("There are a total of {0} cases in Mantis, so there are {1} cases that do not apply to any of the screens shown above",
                    TotalMantisCases, TotalMantisCases - FMantisCasesDone.Count - FMantisCasesOpen.Count);
                FLogFile.WriteLine();

                FLogFile.WriteLine("This table shows the Mantis cases that cannot yet be assigned to an Open Petra screen");
                FLogFile.WriteLine("{| border=\"1\" cellpadding=\"5\" cellspacing=\"0\"");
                FLogFile.WriteLine("!Mantis Case !! Description");

                XmlNodeList noScreenList = FMetaDataMantis.SelectNodes("featurenoscreen");

                foreach (XmlNode noScreen in noScreenList)
                {
                    FLogFile.WriteLine("|-");
                    FLogFile.WriteLine("|{0}", noScreen.Attributes["key"].Value);
                    FLogFile.WriteLine("|{0}", noScreen.Attributes["value"].Value);
                }

                FLogFile.WriteLine("|}");
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