//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2014 by OM International
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
        static void Main(string[] args)
        {
            // Root of all C# directories for OpenPetra.org
            string RootOfCSharpDirectories = "../../csharp/";
            // We are only analysing C# Files
            string CSharpFileType = "*.cs";
            // Log file where to put output of the analysis to
            string LogFile = @"CodeChecker.log";
            Regex RegExpToFind;
            var RegExPatterns = new Dictionary<string, string>();
            var FalsePositives = new Dictionary<string, string>();
            string FalsePositiveValue;
            string FalsePositivesEncountered = String.Empty;
            int NumberOfRegExMatches = 0;
            
            // Application set-up
            new TAppSettingsManager("../../etc/Client.config");

            if (Directory.Exists(TAppSettingsManager.GetValue("OpenPetra.PathLog")))
            {
                new TLogging(TAppSettingsManager.GetValue("OpenPetra.PathLog") + "/" + LogFile);
            }
            else
            {
                new TLogging(LogFile);
            }
            
            // 'Discovery rules' set-up
            RegExPatterns = DeclareRegExpressions();           
            FalsePositives = DeclareFalsePositives();

            // Log that we have started a run
            TLogging.Log("'CODECHECKER' run started." + Environment.NewLine + 
                         "  (" + RegExPatterns.Count.ToString() +
                         " Regular Expression Patterns to search for, " + FalsePositives.Count.ToString() + 
                         " 'False Positives' excluded from search)");
                         
            // Obtain all the C# files we want and loop through them
            foreach (var file in Directory.GetFiles(RootOfCSharpDirectories
                        ,CSharpFileType
                        ,SearchOption.AllDirectories))
            {
                // Don't process the file that belongs to this project as it contains comments that would otherwise be found!
                if (file == @"../../csharp/ICT\BuildTools\CodeChecker\CodeChecker.cs") 
                {
                    continue;
                }
                
                Console.WriteLine("Processing file: " + file);
                
                // Open C# file and read its text
                string contents = File.ReadAllText(file);                

                //
                // Check if any of the RegEx matches!!!
                //
                // CAVEAT: At present only the first match of a matching RegEx Pattern is discovered in any given file; THAT OUGHT TO BE CHANGED!
                foreach (var RegExpItem in RegExPatterns) 
                {
                    RegExpToFind = new Regex(RegExpItem.Value);
                    
                    Match matchInfo = RegExpToFind.Match(contents);
                    
                    if (matchInfo.Success)
                    {
                        if(!FalsePositives.TryGetValue(matchInfo.Value, out FalsePositiveValue))
                        {
                            // RegEx Match is found and not a 'false positive', so log this file!
                            TLogging.Log(file + " -> " + RegExpItem.Key + " match = ''" + matchInfo.Value + "''");
                            
                            NumberOfRegExMatches++;
                        }
                        else
                        {
                            FalsePositivesEncountered += "   * " + FalsePositiveValue + " [" + 
                                matchInfo.Value + "]" + Environment.NewLine;
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
        }

        /// <summary>
        /// Recursive method that returns the files we want in all sub-directories of <paramref name="ADirectoryPath" />.
        /// </summary>
        /// <param name="ADirectoryPath">Path that the recursive file search should start at.</param>
        /// <param name="AFileExtension">File Extension of the files that should be searched for.</param>
        /// <returns>The files we want in all sub-directories of <paramref name="ADirectoryPath" /></returns>
        static List<string> GetFiles(string ADirectoryPath, string AFileExtension)
        {
            var FileList = new List<string>();
            
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
        private static Dictionary<string, string> DeclareRegExpressions()
        {
            var ReturnValue = new Dictionary<string, string>();
            
//            string patternToMatch = @"Access\.Load.*(\,\snull\))";      // Matches for example: 'Access.LoadViaPType(MPartnerConstants.PARTNERTYPE_LEDGER, null)'    in file \Server\lib\MFinance\Gift\Gift.Transactions.cs
//            string patternToMatch = @"DBAccess\.GDBAccessObj\.Select\(([^;]*\,)[\s]*([^;]*\,)[\s]*([^;]*\,)[\s]*null,([^;]*);";  // Matches for example: 'DBAccess.GDBAccessObj.Select(FBudgetTDS,sqlLoadBudgetForThisAndNextYear,FBudgetTDS.ABudget.TableName,null,parameters.ToArray());'    in file \Server\lib\MFinance\Budget\Budget.Consolidate.cs  
//            string patternToMatch = @"DBAccess\.GDBAccessObj\.Select(|DT)\(.*[\s]*null,([^;]*);";
            
            // All kinds of *Access.Load* Methods
            ReturnValue.Add("*Access.Load.*", @"Access\.Load.*(\,\snull\))");       // Matches for example: Access.LoadViaPType(MPartnerConstants.PARTNERTYPE_LEDGER, null)    in file \Server\lib\MFinance\Gift\Gift.Transactions.cs

            // DBAccess.GDBAccessObj.Select Methods and DBAccess.GDBAccessObj.SelectDT Methods
            ReturnValue.Add("DBAccess.GDBAccessObj.Select / SelectDT (no Argument after DB Transaction)", @"DBAccess\.GDBAccessObj\.Select(|DT)\(.*[\s]*null\)");    // Matches for example: 'DBAccess.GDBAccessObj.SelectDT(strSql, "GetLedgerName_TempTable", null)'    in file \Server\lib\MFinance\GL\Reporting.UIConnectors.cs
            ReturnValue.Add("DBAccess.GDBAccessObj.Select / SelectDT (1 to 3 Arguments after DB Transaction)", @"DBAccess\.GDBAccessObj\.Select(|DT)\(.*[\s]*null,[\s]*([^;]*)\)");     // Matches for example: 'DBAccess.GDBAccessObj.SelectDT(bank, sqlFindBankBySortCode, null, new OdbcParameter[]'    in file \Server\lib\MPartner\web\Partner.cs
            
            return ReturnValue;
        }
        
        /// <summary>
        /// Declares the 'false positive' matches that should be excluded from Regular Expressions matches.
        /// </summary>
        /// <returns>'False positive' matches that should be excluded from Regular Expressions matches.</returns>
        private static Dictionary<string, string> DeclareFalsePositives()
        {
            var ReturnValue = new Dictionary<string, string>();
            
            ReturnValue.Add("DBAccess.GDBAccessObj.SelectDT(ExtractDT, SqlStmt, Transaction, null, -1, -1)",
                @"AParametersArrary Argument mistaken for AReadTransaction Argument (in file ../../csharp/ICT\Petra\Server\lib\MPartner\web\ExtractMaster.cs)");
            
            ReturnValue.Add("DBAccess.GDBAccessObj.SelectDT(giftdetails, sql, Transaction, null, 0, 0)",
                @"AParametersArrary Argument mistaken for AReadTransaction Argument (in file ../../csharp/ICT\Petra\Tools\FinanceGDPdUExport\participants.cs)");
            
            ReturnValue.Add("DBAccess.GDBAccessObj.SelectDT(transactions, sql, Transaction, null, 0, 0)",
                @"AParametersArrary Argument mistaken for AReadTransaction Argument (in file ../../csharp/ICT\Petra\Tools\FinanceGDPdUExport\transactions.cs)");
            
            return ReturnValue;
        }        
    }
}