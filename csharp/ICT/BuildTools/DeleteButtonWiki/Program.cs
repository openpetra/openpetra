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

namespace Ict.Tools.DeleteButtonWiki
{
    /// <summary>
    /// This application reads YAML, manual and auto-generated files in the entire OpenPetra suite
    /// It examines the content of these files looking for 'issues'
    /// In this case the issues are to do with the implementation of the Delete Button functionality
    /// The output from the application is a text file in the 'log' folder
    /// This text is formatted so that it can be inserted directly onto a wiki page to give a tabular page content.
    /// </summary>
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
        //private static List <string>FListWindowSingleTable = new List <string>();
        //private static List <string>FListControlMaintainTableWithDataViewGrid = new List <string>();
        //private static List<string> FListWindowBrowsePrint = new List<string>();
        private static List <string>FListMasterAndDetails = new List <string>();

        // Content of the auto-generated -ReferenceCount.cs files
        private static string FTxtCommonRefCount;
        private static string FTxtConferenceRefCount;
        private static string FTxtFinanceRefCount;
        private static string FTxtPartnerRefCount;
        private static string FTxtPersonnelRefCount;
        private static string FTxtSysManRefCount;
        private static List <string>FListActDelete = new List <string>();

        // various global variables
        private static string FBaseClientPath;
        private static int FIssueCount;
        private static int FIssueFileCount;
        private static int FDeleteButtonCount;
        private static int FNewButtonCount;
        private static int FAutoDeleteCount;
        private static int FManualDeleteCount;
        private static int FManualCallsAutoCount;
        private static StreamWriter FLogFile;


        private static void CheckForIssues(List <string>AListToCheck, string ATitle)
        {
            FLogFile.WriteLine("");
            FLogFile.WriteLine("=== Screens Based on Template: {0} ({1} files use this template. The following have Add/Delete functionality) ===",
                ATitle,
                AListToCheck.Count);
            FLogFile.WriteLine("{| border=\"1\" cellpadding=\"5\" cellspacing=\"0\" style=\"font-size:x-small\"");

            FLogFile.Write("!Filename !! DetailTable !! ");
            bool AIsCacheable = ATitle.Contains("Cacheable") || ATitle.Contains("Cachable");

            if (AIsCacheable)
            {
                FLogFile.Write("CacheableTable !! ");
            }

            FLogFile.WriteLine(
                "Has New Button !! Has Delete Button !! Has Auto-delete !! Has ReferenceCount !! Has Deletable Flag !! Has Enable Delete !! Calls .Delete() !! Manual Calls Auto !! PreDelete !! DeleteRow !! PostDelete !! Client/Server Match !! Multi Select");

            foreach (string tryPath in AListToCheck)
            {
                CheckForIssues(tryPath, AIsCacheable);
            }

            FLogFile.WriteLine("|}");
        }

        private static void CheckForIssues(string AYAMLPath, bool AIsCacheable)
        {
            string manualPath = AYAMLPath.Replace(".yaml", ".ManualCode.cs");
            string generatedPath = AYAMLPath.Replace(".yaml", "-generated.cs");

            if (!File.Exists(manualPath))
            {
                return;
            }

            using (StreamReader srManual = new StreamReader(manualPath))
                using (StreamReader srGenerated = new StreamReader(generatedPath))
                    using (StreamReader srYAML = new StreamReader(AYAMLPath))
                    {
                        string yml = srYAML.ReadToEnd();
                        string manual = srManual.ReadToEnd();
                        string generated = srGenerated.ReadToEnd();

                        //string shortManualPath = manualPath.Substring(FBaseClientPath.Length + 1);
                        //string shortGeneratedPath = generatedPath.Substring(FBaseClientPath.Length + 1);
                        string shortYAMLPath = AYAMLPath.Substring(FBaseClientPath.Length + 1);
                        int startCount = FIssueCount;

                        bool bHasDelete = yml.Contains("btnDelete") || yml.Contains("btnRemove");
                        bool bHasNew = yml.Contains("btnNew") || yml.Contains("btnAdd");

                        if (!bHasDelete && !bHasNew)
                        {
                            return;
                        }

                        if (bHasNew)
                        {
                            FNewButtonCount++;
                        }

                        if (bHasDelete)
                        {
                            FDeleteButtonCount++;
                        }

                        string detailTable = GetYAMLAttribute(yml, "DetailTable");
                        string cacheableTable = GetYAMLAttribute(yml, "CacheableTable");

                        // Start new row: col1 is filename, col2 is DetailTable
                        FLogFile.WriteLine("|-");
                        FLogFile.WriteLine("|{0}", shortYAMLPath);
                        FLogFile.WriteLine("|{0}", detailTable);

                        if (AIsCacheable)
                        {
                            // Col2a is chacheable table
                            FLogFile.WriteLine("|{0}", cacheableTable);
                        }

                        // Col3: HasNew, Col4: HasDelete
                        bool bHasNoDeleteByDesign = false;

                        if (bHasNew && bHasDelete)
                        {
                            FLogFile.WriteLine("|Yes");
                            FLogFile.WriteLine("|Yes");
                        }
                        else if (bHasDelete)
                        {
                            FLogFile.WriteLine("|style=\"background:LightSkyBlue\" |No");
                            FLogFile.WriteLine("|Yes");
                        }
                        else if (bHasNew)
                        {
                            FLogFile.WriteLine("|Yes");

                            if (shortYAMLPath.Contains("MaintainUsers.") || shortYAMLPath.Contains("UC_GLBatches.")
                                || shortYAMLPath.Contains("UC_GLJournals."))
                            {
                                // These three have no delete button by design
                                bHasNoDeleteByDesign = true;
                                FLogFile.WriteLine("|style=\"background:LightSkyBlue\" |No");
                            }
                            else
                            {
                                FLogFile.WriteLine("|style=\"background:yellow\" |No");
                                FIssueCount++;
                            }
                        }

                        // Col5: Has auto-delete
                        bool bHasAutoDelete = false;

                        if (generated.Contains("void DeleteRecord(") /*|| generated.Contains("void DeleteRow(") */)
                        {
                            FLogFile.WriteLine("|Yes");
                            bHasAutoDelete = true;
                            FAutoDeleteCount++;
                        }
                        else if (bHasDelete)
                        {
                            if (manual.Contains("void DeleteRecord(") || manual.Contains("void DeleteRow(")
                                || manual.Contains("void DeleteDetail(") || manual.Contains("void Remove"))
                            {
                                FLogFile.WriteLine("|style=\"background:yellow\" |No");
                                FManualDeleteCount++;
                            }
                            else
                            {
                                FLogFile.WriteLine("|style=\"background:yellow\" |???");
                            }

                            FIssueCount++;
                        }
                        else
                        {
                            FLogFile.WriteLine("|n/a");
                        }

                        if (bHasAutoDelete && !yml.Contains("actDelete:"))
                        {
                            FListActDelete.Add(shortYAMLPath);
                            FIssueCount++;
                        }

                        // Col6: HasReferenceCount code to check for usage
                        bool bRefCountingEnabled = true;

                        if (bHasDelete && !generated.Contains("CacheableRecordReferenceCount"))
                        {
                            if (yml.Contains("SkipReferenceCheck=true"))
                            {
                                FLogFile.WriteLine("|style=\"background:LightSkyBlue\" |Disabled");
                                bRefCountingEnabled = false;
                            }
                            else
                            {
                                FLogFile.WriteLine("|style=\"background:yellow\" |No");
                                FIssueCount++;
                            }
                        }
                        else
                        {
                            FLogFile.WriteLine("|Yes");
                        }

                        // Col7 Has deletable flag
                        bool bHasDeletable = false;

                        if (yml.Contains("chkDetailDeletable") || yml.Contains("chkDetailTypeDeletable"))
                        {
                            FLogFile.WriteLine("|Yes");
                            bHasDeletable = true;
                        }
                        else
                        {
                            FLogFile.WriteLine("|No");
                        }

                        // Col8: Has Enable Delete
                        if (bHasDeletable)
                        {
                            if (generated.Contains("btnDelete.Enabled = ((grdDetails.")
                                || generated.Contains("btnDelete.Enabled = pnlDetails.Enabled;"))
                            {
                                FLogFile.WriteLine("|Yes");
                            }
                            else
                            {
                                FLogFile.WriteLine("|style=\"background:yellow\" |No");
                                FIssueCount++;
                            }
                        }
                        else
                        {
                            if (!bHasDelete)
                            {
                                FLogFile.WriteLine("|style=\"background:LightSkyBlue\" |n/a");
                            }
                            else if (generated.Contains("btnDelete.Enabled = pnlDetails.Enabled;"))
                            {
                                FLogFile.WriteLine("|Yes");
                            }
                            else
                            {
                                FLogFile.WriteLine("|style=\"background:yellow\" |No");
                                FIssueCount++;
                            }
                        }

                        // Col9: Calls .Delete() in manual code without a DeleteRowManual()
                        if (manual.Contains(".Delete()") && !manual.Contains(" DeleteRowManual("))
                        {
                            if (bHasAutoDelete || bHasNoDeleteByDesign)
                            {
                                FLogFile.WriteLine("|style=\"background:LightSkyBlue\" |Yes");
                            }
                            else
                            {
                                FLogFile.WriteLine("|style=\"background:yellow\" |Yes");
                                FIssueCount++;
                            }
                        }
                        else
                        {
                            FLogFile.WriteLine("|No");
                        }

                        // Col10: Manual code calls standard auto delete code
                        if (manual.Contains("Delete" + detailTable + "();"))
                        {
                            if (bHasAutoDelete)
                            {
                                FLogFile.WriteLine("|style=\"background:LightSkyBlue\" |Yes");
                            }
                            else
                            {
                                FLogFile.WriteLine("|style=\"background:yellow\" |Yes");
                                FIssueCount++;
                            }

                            FManualCallsAutoCount++;
                        }
                        else
                        {
                            FLogFile.WriteLine("|No");
                        }

                        // Col11: Has PreDeleteManual
                        if (manual.Contains("bool PreDeleteManual"))
                        {
                            FLogFile.WriteLine("|style=\"background:LightSkyBlue\" |Yes");
                        }
                        else
                        {
                            FLogFile.WriteLine("|No");
                        }

                        // Col12: Has DeleteRowManual
                        if (manual.Contains("bool DeleteRowManual"))
                        {
                            FLogFile.WriteLine("|style=\"background:LightSkyBlue\" |Yes");
                        }
                        else
                        {
                            FLogFile.WriteLine("|No");
                        }

                        // Col13: Has PostDeleteManual
                        if (manual.Contains("PostDeleteManual("))
                        {
                            FLogFile.WriteLine("|style=\"background:LightSkyBlue\" |Yes");
                        }
                        else
                        {
                            FLogFile.WriteLine("|No");
                        }

                        // Col14: Client/server tablename match
                        if (bHasDelete && bRefCountingEnabled)
                        {
                            bool bOk = false;
                            string searchForInServer = String.Empty;
                            string searchForInClient = String.Empty;

                            if (AIsCacheable)
                            {
                                searchForInServer = String.Format("case {0}{1}{0}", "\"", cacheableTable);
                                searchForInClient = String.Format("{0}{1}{0}", "\"", cacheableTable);
                                bOk = generated.Contains("WebConnectors.GetCacheableRecordReferenceCount(");
                            }
                            else
                            {
                                searchForInServer = String.Format("ADataTable is {0}Table)", detailTable);
                                searchForInClient = String.Format("FMainDS.{0}", detailTable);
                                bOk = generated.Contains("WebConnectors.GetNonCacheableRecordReferenceCount(");
                            }

                            bOk = bOk && generated.Contains(searchForInClient);

                            if (shortYAMLPath.StartsWith("MCommon"))
                            {
                                bOk = bOk && FTxtCommonRefCount.Contains(searchForInServer);
                            }
                            else if (shortYAMLPath.StartsWith("MConference"))
                            {
                                bOk = bOk && FTxtConferenceRefCount.Contains(searchForInServer);
                            }
                            else if (shortYAMLPath.StartsWith("MFinance"))
                            {
                                bOk = bOk && FTxtFinanceRefCount.Contains(searchForInServer);
                            }
                            else if (shortYAMLPath.StartsWith("MPartner"))
                            {
                                bOk = bOk && FTxtPartnerRefCount.Contains(searchForInServer);
                            }
                            else if (shortYAMLPath.StartsWith("MPersonnel"))
                            {
                                bOk = bOk && FTxtPersonnelRefCount.Contains(searchForInServer);
                            }
                            else if (shortYAMLPath.StartsWith("MSysMan"))
                            {
                                bOk = bOk && FTxtSysManRefCount.Contains(searchForInServer);
                            }
                            else
                            {
                                // Not a module we were expecting
                                bOk = false;
                            }

                            FLogFile.WriteLine(bOk ? "|Yes" : "|style=\"background:yellow\" |No");

                            if (!bOk)
                            {
                                FIssueCount++;
                            }
                        }
                        else
                        {
                            FLogFile.WriteLine("|n/a");
                        }

                        // Col15: Multi-select
                        if (generated.Contains("EnableMultiSelection = true"))
                        {
                            FLogFile.WriteLine("|Yes");
                        }
                        else
                        {
                            FLogFile.WriteLine("|No");
                        }

                        if (FIssueCount != startCount)
                        {
                            FIssueFileCount++;
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

            // The output file is in the OpenPetra log folder
            using (FLogFile = new StreamWriter(Path.Combine(rootPath, @"log\DeleteButtonWiki.txt")))
            {
                FLogFile.WriteLine("== Delete Button Implementation ==");
                FLogFile.WriteLine(
                    "Analysis run at {0} on {1}.<br/>All the files listed below have either a 'New' button or an 'Add' button with a name '''containing''' btnNew or btnAdd",
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
                        //else if (content.Contains("windowSingleTable"))
                        //{
                        //    FListWindowSingleTable.Add(tryPath);
                        //}
                        //else if (content.Contains("controlMaintainTableWithDataViewGrid"))
                        //{
                        //    FListControlMaintainTableWithDataViewGrid.Add(tryPath);
                        //}
                        //else if (content.Contains("windowBrowsePrint"))
                        //{
                        //    FListWindowBrowsePrint.Add(tryPath);
                        //}
                        else if (content.Contains("MasterTable") && content.Contains("DetailTable"))
                        {
                            FListMasterAndDetails.Add(tryPath);
                        }

                        sr.Close();
                    }
                }

                string path = Path.Combine(rootPath, @"csharp\ICT\Petra\Server");
                string[] allRefCounts = Directory.GetFiles(path, "*ReferenceCount-generated.cs", SearchOption.AllDirectories);

                foreach (string tryPath in allRefCounts)
                {
                    using (StreamReader sr = new StreamReader(tryPath))
                    {
                        if (tryPath.Contains("MCommon"))
                        {
                            FTxtCommonRefCount = sr.ReadToEnd();
                        }
                        else if (tryPath.Contains("MConference"))
                        {
                            FTxtConferenceRefCount = sr.ReadToEnd();
                        }
                        else if (tryPath.Contains("MFinance"))
                        {
                            FTxtFinanceRefCount = sr.ReadToEnd();
                        }
                        else if (tryPath.Contains("MPartner"))
                        {
                            FTxtPartnerRefCount = sr.ReadToEnd();
                        }
                        else if (tryPath.Contains("MPersonnel"))
                        {
                            FTxtPersonnelRefCount = sr.ReadToEnd();
                        }
                        else if (tryPath.Contains("MSysMan"))
                        {
                            FTxtSysManRefCount = sr.ReadToEnd();
                        }

                        sr.Close();
                    }
                }

                // Now do each one in turn, looking for issues in the manual code
                FIssueCount = 0;
                FIssueFileCount = 0;
                FDeleteButtonCount = 0;
                FNewButtonCount = 0;
                FAutoDeleteCount = 0;
                FManualCallsAutoCount = 0;
                FManualDeleteCount = 0;
                CheckForIssues(FListWindowEdit, "WindowEdit");
                CheckForIssues(FListWindowTDS, "WindowTDS");
                CheckForIssues(FListWindowMaintainTable, "WindowMaintainTable");
                CheckForIssues(FListWindowMaintainCacheableTable, "WindowMaintainCacheableTable");
                CheckForIssues(FListWindowEditUIConnector, "WindowEditUIConnector");
                CheckForIssues(FListWindowEditWebConnectorMasterDetail, "WindowEditWebConnectorMasterDetail");
                //CheckForIssues(FListWindowSingleTable, "WindowSingleTable");
                //CheckForIssues(FListWindowBrowsePrint, "WindowBrowsePrint");
                CheckForIssues(FListControlMaintainTable, "ControlMaintainTable");
                CheckForIssues(FListControlMaintainCacheableTable, "ControlMaintainCacheableTable");
                //CheckForIssues(FListControlMaintainTableWithDataViewGrid, "ControlMaintainTableWithDataViewGrid");

                FLogFile.WriteLine("");
                FLogFile.WriteLine("=== Summary ===");
                FLogFile.WriteLine("  {0} potential issues found in {1} files", FIssueCount, FIssueFileCount);
                FLogFile.WriteLine("  There are {0} New/Add buttons in the application", FNewButtonCount);
                FLogFile.WriteLine("  There are {0} Delete/Remove buttons in the application", FDeleteButtonCount);
                FLogFile.WriteLine("   {0} of these call the auto-delete function", FAutoDeleteCount);
                FLogFile.WriteLine("   {0} of these call a manual delete function", FManualDeleteCount);
                FLogFile.WriteLine("     {0} of these call the auto-delete function from manual code", FManualCallsAutoCount);

                FLogFile.WriteLine("<span style=\"background:LightSkyBlue\">A Blue cell</span> indicates an unusual entry that is 'by design'.<br/>");
                FLogFile.WriteLine("<span style=\"background:yellow\">A Yellow cell</span> indicates an unusual entry that requires investigation.");
                //FLogFile.WriteLine("________________________________________________________________________________");
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