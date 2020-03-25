//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2020 by OM International
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.ObjectModel;

using Ict.Common;

namespace Ict.Tools.CodeChecker
{
    class Program
    {
        static int Main(string[] args)
        {
            // Root of all C# directories for OpenPetra.org
            string RootOfCSharpDirectories = "csharp/";
            if (!Directory.Exists(RootOfCSharpDirectories))
            {
                // if this exe is run from delivery/bin
                RootOfCSharpDirectories = "../../csharp/";
            }

            // We are only analysing C# Files
            string CSharpFileType = "*.cs";
            // Log file where to put output of the analysis to
            string LogFile = "CodeChecker.log";
            Regex RegExpToFind;
            var RegExPatterns = new Dictionary <string, string>();

            Dictionary <string, string>FalsePositivesFullMatch = new Dictionary <string, string>();
            Dictionary <string, string>FalsePositivesEndMatch = new Dictionary <string, string>();
            string FalsePositiveValue;
            string FalsePositiveKey = String.Empty;
            string FalsePositivesEncountered = String.Empty;
            int NumberOfRegExMatches = 0;
            bool IsFalsePositive;

            try
            {
                // Application set-up
                new TAppSettingsManager(false);
            }
            catch (Exception Exc)
            {
                Console.WriteLine("Error setting up TAppSettingsManager:  " + Exc.ToString());

                // We return -1 to the operating system, indicating an error.
                return -1;
            }

            string action = TAppSettingsManager.GetValue("Action", "help");

            if (action == "help")
            {
                Console.WriteLine("sample calls: ");
                Console.WriteLine("   mono Ict.Tools.CodeChecker -Action:data");
                Console.WriteLine("   mono Ict.Tools.CodeChecker -Action:static");
                return -1;
            }

            try
            {
                new TLogging(LogFile, true);
            }
            catch (Exception Exc)
            {
                Console.WriteLine("Error setting up logging:  " + Exc.ToString());

                // We return -1 to the operating system, indicating an error.
                return -1;
            }

            try
            {
                // 'Discovery rules' set-up
                if (action == "data")
                {
                    RegExPatterns = DeclareRegExpressions();
                    DeclareFalsePositives(out FalsePositivesFullMatch, out FalsePositivesEndMatch);
                }

                bool firstMatchStatic = true;
                string StaticMessage = "Static Variables are not allowed in ASP.net";

                // Log that we have started a run
                TLogging.Log("'CODECHECKER' run started at " + DateTime.Now.ToString("dddd, dd-MMM-yyyy, HH:mm:ss.ff") + Environment.NewLine +
                    "  (" + RegExPatterns.Count.ToString() +
                    " Regular Expression Patterns to search for, " + (FalsePositivesFullMatch.Count + FalsePositivesEndMatch.Count).ToString() +
                    " 'False Positives' excluded from search)");

                // Obtain all the C# files we want and loop through them
                foreach (var file in Directory.GetFiles(RootOfCSharpDirectories,
                             CSharpFileType,
                             SearchOption.AllDirectories))
                {
                    // Don't process the file that belongs to this project as it contains comments that would otherwise be found!
                    if (file.Contains(@"csharp/ICT/BuildTools/CodeChecker/CodeChecker.cs"))
                    {
                        continue;
                    }

                    TLogging.LogAtLevel(4, "Processing file: " + file);

                    // Open C# file and read its text
                    string contents = File.ReadAllText(file);

                    // check for static variables
                    // does the filename match?
                    if ((action == "static") && (file.Contains("csharp/ICT/Common") || file.Contains("csharp/ICT/Petra"))
                        && !file.Contains("csharp/ICT/Petra/Tools") && !file.Contains("csharp/ICT/Petra/ServerAdmin"))
                    {
                        bool firstFileMatch = true;
                        RegExpToFind = new Regex(@"(?m)^.*\n.*static (?!readonly)(?!partial)(?!extern)[^(\n]*[;=][^\n]*\n");

                        foreach (Match matchInfo in RegExpToFind.Matches(contents))
                        {
                            string[] lines = matchInfo.Value.Split(new char[] {'\n'});
                            string staticLine = matchInfo.Value.Trim();

                            if (lines.Length >= 2)
                            {
                                staticLine = lines[1].Trim();

                                if (lines[0].Trim() == "[ThreadStatic]")
                                {
                                    continue;
                                }
                            }
                            
                            if (staticLine.StartsWith("//"))
                            {
                                continue;
                            }

                            if (staticLine.EndsWith("// STATIC_OK: Mutex"))
                            {
                                // here we actually want a static variable shared between the threads
                                continue;
                            }

                            if (staticLine.EndsWith("// STATIC_OK: Global"))
                            {
                                // here we actually want a static variable shared between the threads
                                continue;
                            }

                            // RegEx Match is found, so log this file!
                            if (firstMatchStatic)
                            {
                                firstMatchStatic = false;
                                TLogging.Log("*****   " + StaticMessage + "   *****");
                            }

                            if (firstFileMatch)
                            {
                                firstFileMatch = false;
                                TLogging.Log("file " + file);
                            }

                            TLogging.Log("                           " + matchInfo.Value.Trim());

                            NumberOfRegExMatches++;
                        }
                    }

                    //
                    // Check if any of the RegEx matches!!!
                    //
                    foreach (var RegExpItem in RegExPatterns)
                    {
                        RegExpToFind = new Regex(RegExpItem.Value);

                        foreach (Match matchInfo in RegExpToFind.Matches(contents))
                        {
                            IsFalsePositive = false;

                            if (!FalsePositivesFullMatch.TryGetValue(matchInfo.Value, out FalsePositiveValue))
                            {
                                foreach (var FalsePositivesEnding in FalsePositivesEndMatch)
                                {
                                    if (matchInfo.Value.EndsWith(FalsePositivesEnding.Key))
                                    {
                                        IsFalsePositive = true;
                                        FalsePositiveValue = FalsePositivesEnding.Value.Contains("{0}") ? String.Format(FalsePositivesEnding.Value,
                                            file) : FalsePositivesEnding.Value;
                                        FalsePositiveKey = FalsePositivesEnding.Key;

                                        break;
                                    }
                                }

                                if (!IsFalsePositive)
                                {
                                    // RegEx Match is found and not a 'false positive', so log this file!
                                    TLogging.Log(file + " -> " + RegExpItem.Key + " match = ''" + matchInfo.Value + "''");

                                    NumberOfRegExMatches++;
                                }
                                else
                                {
                                    FalsePositiveKey = matchInfo.Value;
                                }
                            }
                            else
                            {
                                IsFalsePositive = true;
                                FalsePositiveKey = matchInfo.Value;
                            }

                            if (IsFalsePositive)
                            {
                                FalsePositivesEncountered += "   * " + FalsePositiveValue + " [" +
                                                             FalsePositiveKey + "]" + Environment.NewLine;
                            }
                        }
                    }
                }

                if (NumberOfRegExMatches > 0)
                {
                    TLogging.Log(Environment.NewLine +
                        "  * " + NumberOfRegExMatches.ToString() + " Matches were found!");
                }
                else
                {
                    TLogging.Log(Environment.NewLine +
                        "  * No Matches were found!  :-)");
                }

                if (FalsePositivesEncountered != String.Empty)
                {
                    // Strip trailing 'Environment.NewLine'
                    FalsePositivesEncountered = FalsePositivesEncountered.Substring(0, FalsePositivesEncountered.Length - Environment.NewLine.Length);

                    TLogging.Log(Environment.NewLine +
                        "  * The following 'false positives' were found (please ignore those!!!)" + Environment.NewLine + FalsePositivesEncountered);
                }

                // We return to the operating system:
                // * 0 in case no Matches were found;
                // * otherwise the number of Matches.
                // This can be evaluated e.g. with a Build server (e.g. Jenkins).
                return NumberOfRegExMatches;
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception was thrown:" + Environment.NewLine + Exc.ToString());

                // We return -1 to the operating system, indicating an error.
                return -1;
            }
        }

        /// <summary>
        /// Recursive method that returns the files we want in all sub-directories of <paramref name="ADirectoryPath" />.
        /// </summary>
        /// <param name="ADirectoryPath">Path that the recursive file search should start at.</param>
        /// <param name="AFileExtension">File Extension of the files that should be searched for.</param>
        /// <returns>The files we want in all sub-directories of <paramref name="ADirectoryPath" /></returns>
        static List <string>GetFiles(string ADirectoryPath, string AFileExtension)
        {
            var FileList = new List <string>();

            foreach (var SubDirectory in Directory.GetDirectories(ADirectoryPath))
            {
                FileList.AddRange(GetFiles(SubDirectory, AFileExtension));
            }

            FileList.AddRange(Directory.GetFiles(ADirectoryPath, AFileExtension));

            return FileList;
        }

        /// <summary>
        /// Declares the Regular Expressions that we are looking for in the C# files.
        /// </summary>
        /// <remarks>When new Regular Expressions are added: <em>Take care to add any 'false positives'</em>
        /// to Method <see cref="DeclareFalsePositives"/>!!!</remarks>
        /// <returns>Regular Expressions that we are looking for in the C# files.</returns>
        private static Dictionary <string, string>DeclareRegExpressions()
        {
            var ReturnValue = new Dictionary <string, string>();

            // All kinds of *Access.Load* Methods
            ReturnValue.Add("*Access.Load.* (no Argument after DB Transaction)", @"Access\.Load.*[\s]*\((([^;]*)[\s]*null\))");       // Matches for example: Access.LoadViaPType(MPartnerConstants.PARTNERTYPE_LEDGER, null)    in file
                                                                                                                                      // \Server\lib\MFinance\Gift\Gift.Transactions.cs
            ReturnValue.Add("*Access.Load.* (n Arguments after DB Transaction [unsharp!])",
                @"Access\.Load.*[\s]*\((([^;]*)[\s]*null,[\s]*([^;]*)[\s]*,[\s]*([^;]*)[\s]*,[\s]*([^;]*)[\s]*\))");                  // Matches for example:
                                                                                                                                      // Access.LoadByPrimaryKey(partnerKey,\r\n
                                                                                                                                      //                    StringHelper.InitStrArr(new String[] {
                                                                                                                                      // PPartnerTable.GetPartnerShortNameDBName() }), null, null, 0, 0)
                                                                                                                                      // in file ../../csharp/ICT\Petra\Server\lib\MFinance\Gift\Gift.Exporting.cs

            ReturnValue.Add("*Access.CountAll", @"Access\.CountAll[\s]*\((null\))");    // would match for example (*if* the DB Transaction would be null): Access.ALedgerAccess.CountAll(Transaction)
                                                                                        // in file \Testing\lib\MFinance\server\Gift\test.cs

            ReturnValue.Add("*Access.Exists / ExistsUniqueKey", @"Access\.Exists(|UniqueKey)[\s]*\((([^;]*)[\s]*null\))");    // would match for example (*if* the DB Transaction would be null): SLoginAccess.Exists(NewLoginRow.UserId,
                                                                                                                              // NewLoginRow.LoginDate,
                                                                                                                              // NewLoginRow.LoginTime, WriteTransaction)    in file \Server\app\Core\LoginLog.cs

            ReturnValue.Add("*Access.CountUsingTemplate", @"Access\.CountUsingTemplate[\s]*\((([^;]*)[\s]*null\))");         // would match for example (*if* the DB Transaction would be null): PDataLabelUseAccess.CountUsingTemplate(TemplateRow, null,
                                                                                                                             // AReadTransaction)    in file \Server\lib\MCommon\OfficeSpecificDataLabels.cs

            ReturnValue.Add("*Access.DeleteByPrimaryKey", @"Access\.DeleteByPrimaryKey[\s]*\((([^;]*)[\s]*null\))");         // would match for example (*if* the DB Transaction would be null):
                                                                                                                             // PPartnerRelationshipAccess.DeleteByPrimaryKey(AOldFamilyKey, "FAMILY", APersonKey, Transaction)
                                                                                                                             // in file \Server\lib\MPartner\Common\Partner.cs

            ReturnValue.Add("*Access.DeleteUsingTemplate", @"Access\.DeleteUsingTemplate[\s]*\((([^;]*)[\s]*null\))");      // would match for example (*if* the DB Transaction would be null): PPartnerLocationAccess.DeleteUsingTemplate(TemplateRow,
                                                                                                                            // null, ATransaction)    in file \Server\lib\MPartner\Common\DataAggregates.PPartnerAddress.cs

            ReturnValue.Add("*Access.AddOrModifyRecord", @"Access\.AddOrModifyRecord[\s]*\((([^;]*)[\s]*null\))");          // would match for example (*if* the DB Transaction would be null): PBankAccess.AddOrModifyRecord(BankRow.PartnerKey,
                                                                                                                            // FMainDS.PBank, BankRow, FDoNotOverwrite, ATransaction);    in file \Server\lib\MPartner\web\ImportExt.cs


            ReturnValue.Add("*Cascading.DeleteByPrimaryKey (1 Argument after DB Transaction)",
                @"Cascading\.DeleteByPrimaryKey[\s]*\(([^;]*)[\s]*null,[\s]*([^;]*)\)");                                    // would match for example (*if* the DB Transaction would be null):
                                                                                                                            // MExtractMasterCascading.DeleteByPrimaryKey(ExtractId,
                                                                                                                            // SubmitChangesTransaction, true)
                                                                                                                            // in file \Server\lib\MPartner\web\ExtractMaster.cs
            ReturnValue.Add("*Cascading.DeleteUsingTemplate (1 Argument after DB Transaction)",
                @"Cascading\.DeleteUsingTemplate[\s]*\(([^;]*)[\s]*null,[\s]*([^;]*)\)");                                   // theoretic possibility, such call is not in use at all at the time of
                                                                                                                            // writing (Sept. 2014)

            ReturnValue.Add("*Cascading.CountByPrimaryKey (n Arguments after DB Transaction)",
                @"Cascading\.CountByPrimaryKey[\s]*\(([^;]*)[\s]*null,[\s]*([^;]*)\)");                                     // would match for example (*if* the DB Transaction would be null):
                                                                                                                            // AAccountCascading.CountByPrimaryKey(ALedgerNumber, AAccountCode, 50,
                                                                                                                            // Transaction, false, out ReferenceResults)
                                                                                                                            // in file \Server\lib\MFinance\setup\GL.Setup.cs
            ReturnValue.Add("*Cascading.CountUsingTemplate (n Arguments after DB Transaction)",
                @"Cascading\.CountUsingTemplate[\s]*\(([^;]*)[\s]*null,[\s]*([^;]*)\)");                                    // theoretic possibility, such call is not in use at all at the time of writing (Sept. 2014)

            return ReturnValue;
        }

        /// <summary>
        /// Declares the 'false positive' matches that should be excluded from Regular Expressions matches.
        /// </summary>
        /// <returns>'False positive' matches that should be excluded from Regular Expressions matches.</returns>
        private static void DeclareFalsePositives(out Dictionary <string, string>AFalsePositivesFullMatch,
            out Dictionary <string, string>AFalsePositivesEndMatch)
        {
            AFalsePositivesFullMatch = new Dictionary <string, string>();
            AFalsePositivesEndMatch = new Dictionary <string, string>();

            AFalsePositivesFullMatch.Add("Access.LoadUsingTemplate(TemplateRow, null, null, ReadTransaction,\r\n                        null, 0, 0)",
                @"Other Argument mistaken for AReadTransaction Argument (in file ../../csharp/ICT\Petra\Server\lib\MConference\ConferenceOptions.cs)");

            AFalsePositivesFullMatch.Add(
                "Access.LoadViaPPartner(LastGiftDS, APartnerKey, null, ReadTransaction,\r\n                        StringHelper.InitStrArr(new String[] { \"ORDER BY\", AGiftTable.GetDateEnteredDBName() + \" DESC\" }), 0, 1)",
                @"AFieldList Argument mistaken for AReadTransaction Argument (in file ../../csharp/ICT\Petra\Server\lib\MFinance\Gift\Gift.cs)");

            AFalsePositivesFullMatch.Add(
                "Access.LoadUsingTemplate(TemplateRow0,\r\n                    Operators0,\r\n                    null,\r\n                    DBTransaction,\r\n                    OrderList0,\r\n                    0,\r\n                    0)",
                @"AFieldList  Argument mistaken for AReadTransaction Argument (in file ../../csharp/ICT\Petra\Server\lib\MFinance\ICH\StewardshipCalculation.cs)");

            AFalsePositivesFullMatch.Add(
                "Access.LoadUsingTemplate(TemplateRow1,\r\n                                Operators1,\r\n                                null,\r\n                                DBTransaction,\r\n                                OrderList1,\r\n                                0,\r\n                                0)",
                @"AFieldList  Argument mistaken for AReadTransaction Argument (in file ../../csharp/ICT\Petra\Server\lib\MFinance\ICH\StewardshipCalculation.cs)");

            AFalsePositivesFullMatch.Add(
                "Access.LoadUsingTemplate(TemplateRow, null, null,\r\n                ASituation.GetDatabaseConnection().Transaction, OrderList, 0, 0)",
                @"AFieldList Argument mistaken for AReadTransaction Argument (in files ../../csharp/ICT\Petra\Server\lib\MReporting\MConference\AccommodationReportCalculation.cs AND ../../csharp/ICT\Petra\Server\lib\MReporting\MConference\ConferenceFieldCalculation.cs)");

            AFalsePositivesFullMatch.Add(
                "Access.LoadViaSUser(AUserName, null, ReadTransaction,\r\n                                StringHelper.InitStrArr(new string[] { \"ORDER BY\", SUserDefaultsTable.GetDefaultCodeDBName() }), 0, 0)",
                @"AFieldList Argument mistaken for AReadTransaction Argument (in file ../../csharp/ICT\Petra\Server\lib\MSysMan\UserDefaults.cs)");

            AFalsePositivesFullMatch.Add(
                "Access.Exists(ProcessedPersonRow.PartnerKey, SubmittedLocationPK.SiteKey,\r\n                                    SubmittedLocationPK.LocationKey, ASubmitChangesTransaction))\r\n                            {\r\n                                /*\r\n                                 * PartnerLocation records for family members are added to APartnerLocationTable for easier data handling and\r\n                                 * will be removed again after SubmitChanges of whole dataset but before returning to client as otherwise\r\n                                 * they would confusingly show up on client side.\r\n                                 */\r\n\r\n                                // Make sure record is not added more than once to APartnerLocationTable (in case it is not yet in database).\r\n                                if (APartnerLocationTable.Rows.Find(new System.Object[] { ProcessedPersonRow.PartnerKey, SubmittedLocationPK.SiteKey,\r\n                                                                                          SubmittedLocationPK.LocationKey }) == null)",
                @"null accidentally found that isn't an Argurment of the searched-for Method call anymore [multi-line continuation situation] (in file ../../csharp/ICT\Petra\Server\lib\MPartner\Common\DataAggregates.PPartnerAddress.cs)");

            // 'String-ending-with' matches
            AFalsePositivesEndMatch.Add(
                "\" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords)",
                @"AParametersArrary Argument mistaken for AReadTransaction Argument (this is the case in all *.Access-generated.cs files, here in file {0})");
            AFalsePositivesEndMatch.Add("Table[countRow], null, ATransaction, AWithCascDelete)",
                @"ATemplateOperators Argument mistaken for AReadTransaction Argument (this is the case in the Cascading-generated.cs file)");
            AFalsePositivesEndMatch.Add("Table[countRow], null, AMaxReferences, ATransaction, AWithCascCount, ref AReferences, ANestingDepth + 1)",
                @"ATemplateOperators Argument mistaken for AReadTransaction Argument (this is the case in the Cascading-generated.cs file");
        }
    }
}
