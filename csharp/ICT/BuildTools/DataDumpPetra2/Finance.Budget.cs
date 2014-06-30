//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Tools.DBXML;
using Ict.Common;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// this class helps with upgrading tables in Budget
    /// </summary>
    public class TFinanceBudgetUpgrader : TFixData
    {
        /// <summary>
        /// fixing table a_budget_type
        /// </summary>
        public static bool FixABudgetType(StringCollection AColumnNames, ref string[] ANewRow)
        {
            string BudgetTypeCode = GetValue(AColumnNames, ANewRow, "a_budget_type_code_c");

            if (BudgetTypeCode == "INF.BASE")
            {
                BudgetTypeCode = "INFLATE_BASE";
            }
            else if (BudgetTypeCode == "INF. N")
            {
                BudgetTypeCode = "INFLATE_N";
            }

            SetValue(AColumnNames, ref ANewRow, "a_budget_type_code_c", BudgetTypeCode.ToUpper());

            return true;
        }

        /// <summary>
        /// Populate the empty table ABudgetRevision using tables ABudget and ABudgetPeriod
        /// </summary>
        public static int PopulateABudgetRevision(StringCollection AColumnNames,
            ref string[] ANewRow,
            StreamWriter AWriter,
            StreamWriter AWriterTest)
        {
            // in Petra 2.x, there never has been a record in this table.
            // so if there is a budget, we need to create a revision 0 for each year

            // load the file a_budget.d.gz so that we can access the values for each budget
            TTable BudgetTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_budget");

            TParseProgressCSV BudgetParser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_budget.d.gz",
                BudgetTableOld.grpTableField.Count);

            StringCollection BudgetColumnNames = GetColumnNames(BudgetTableOld);

            // load the file a_budget_period.d.gz
            TTable BudgetPeriodTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_budget_period");

            TParseProgressCSV BudgetPeriodParser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_budget_period.d.gz",
                BudgetPeriodTableOld.grpTableField.Count);

            StringCollection BudgetPeriodColumnNames = GetColumnNames(BudgetPeriodTableOld);

            // read first row in ABudgetPeriod
            string[] OldBudgetPeriodRow = BudgetPeriodParser.ReadNextRow();

            // [0] last year, [1] current year, [2] next year. Null if budget does not exist for a year.
            string[] BudgetYears = new string[3];

            List <string[]>BudgetPeriodRows = new List <string[]>();
            List <string>Revisions = new List <string>();
            string LedgerNumber = string.Empty;
            int RowCounter = 0;

            SetValue(AColumnNames, ref ANewRow, "a_revision_i", "0");
            SetValue(AColumnNames, ref ANewRow, "a_description_c", "default");
            SetValue(AColumnNames, ref ANewRow, "s_date_created_d", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_created_by_c", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_date_modified_d", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_modified_by_c", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_modification_id_t", "\\N");

            // one a_budget_period record for each year of each ledger
            while (true)
            {
                BudgetYears[0] = null;
                BudgetYears[2] = null;
                string[] OldRow = BudgetParser.ReadNextRow();

                if (OldRow == null)
                {
                    break;
                }

                if (LedgerNumber != GetValue(BudgetColumnNames, OldRow, "a_ledger_number_i"))
                {
                    LedgerNumber = GetValue(BudgetColumnNames, OldRow, "a_ledger_number_i");

                    // gets the current financial year for the current ledger
                    GetLedgerCurrentYear(LedgerNumber, out BudgetYears[1]);
                }

                // Only three possible years for each ledger. Hence only three a_budget_revision records per ledger.
                // Move on to next ledger if data for all three years is found.
                if (!Revisions.Contains(LedgerNumber + "_" + (Convert.ToInt32(BudgetYears[1]) - 1).ToString())
                    && !Revisions.Contains(LedgerNumber + "_" + (Convert.ToInt32(BudgetYears[1])).ToString())
                    && !Revisions.Contains(LedgerNumber + "_" + (Convert.ToInt32(BudgetYears[1]) + 1).ToString()))
                {
                    GetBudgetPeriods(BudgetPeriodParser, BudgetPeriodColumnNames, ref OldBudgetPeriodRow,
                        GetValue(BudgetColumnNames, OldRow, "a_budget_sequence_i"), out BudgetPeriodRows);
                    GetBudgetYears(BudgetPeriodColumnNames, BudgetPeriodRows, ref BudgetYears);

                    foreach (string BudgetYear in BudgetYears)
                    {
                        if ((BudgetYear != null) && !Revisions.Contains(LedgerNumber + "_" + BudgetYear))
                        {
                            SetValue(AColumnNames, ref ANewRow, "a_ledger_number_i", LedgerNumber);
                            SetValue(AColumnNames, ref ANewRow, "a_year_i", BudgetYear);

                            AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());

                            if (AWriterTest != null)
                            {
                                AWriterTest.WriteLine("BEGIN; " + "COPY a_budget_revision FROM stdin;");
                                AWriterTest.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                                AWriterTest.WriteLine("\\.");
                                AWriterTest.WriteLine("ROLLBACK;");
                            }

                            RowCounter++;

                            Revisions.Add(LedgerNumber + "_" + BudgetYear);
                        }
                    }
                }
            }

            return RowCounter;
        }

        /// <summary>
        /// Fix table a_budget
        /// </summary>
        /// <remarks>
        /// In Petra, one ABudget could cover three years. These must be split up for OP with one record for each year.
        /// </remarks>
        public static int FixABudget(StringCollection AColumnNames,
            ref string[] ANewRow,
            StreamWriter AWriter,
            StreamWriter AWriterTest)
        {
            // load the file a_budget.d.gz so that we can access the values for each budget
            TTable BudgetTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_budget");

            TParseProgressCSV BudgetParser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_budget.d.gz",
                BudgetTableOld.grpTableField.Count);

            StringCollection BudgetColumnNames = GetColumnNames(BudgetTableOld);

            // load the file a_budget_period.d.gz
            TTable BudgetPeriodTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_budget_period");

            TParseProgressCSV BudgetPeriodParser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_budget_period.d.gz",
                BudgetPeriodTableOld.grpTableField.Count);

            StringCollection BudgetPeriodColumnNames = GetColumnNames(BudgetPeriodTableOld);

            // read first row in ABudgetPeriod
            string[] OldBudgetPeriodRow = BudgetPeriodParser.ReadNextRow();

            // [0] last year, [1] current year, [2] next year. Null if budget does not exist for a year.
            string[] BudgetYears = new string[3];

            List <string[]>BudgetPeriodRows = new List <string[]>();
            string LedgerNumber = string.Empty;
            int RowCounter = 0;

            SetValue(AColumnNames, ref ANewRow, "a_revision_i", "0");
            SetValue(AColumnNames, ref ANewRow, "s_date_created_d", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_created_by_c", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_date_modified_d", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_modified_by_c", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_modification_id_t", "\\N");

            while (true)
            {
                BudgetYears[0] = null;
                BudgetYears[2] = null;
                string[] OldRow = BudgetParser.ReadNextRow();

                if (OldRow == null)
                {
                    break;
                }

                if (LedgerNumber != GetValue(BudgetColumnNames, OldRow, "a_ledger_number_i"))
                {
                    LedgerNumber = GetValue(BudgetColumnNames, OldRow, "a_ledger_number_i");

                    // gets the current financial year for the current ledger
                    GetLedgerCurrentYear(LedgerNumber, out BudgetYears[1]);
                }

                GetBudgetPeriods(BudgetPeriodParser, BudgetPeriodColumnNames, ref OldBudgetPeriodRow,
                    GetValue(BudgetColumnNames, OldRow, "a_budget_sequence_i"), out BudgetPeriodRows);
                GetBudgetYears(BudgetPeriodColumnNames, BudgetPeriodRows, ref BudgetYears);

                foreach (string BudgetYear in BudgetYears)
                {
                    if (BudgetYear != null)
                    {
                        RowCounter++;

                        SetValue(AColumnNames, ref ANewRow, "a_budget_sequence_i", RowCounter.ToString());
                        SetValue(AColumnNames, ref ANewRow, "a_ledger_number_i", LedgerNumber);
                        SetValue(AColumnNames, ref ANewRow, "a_year_i", BudgetYear);
                        SetValue(AColumnNames, ref ANewRow, "a_cost_centre_code_c", GetValue(BudgetColumnNames, OldRow, "a_cost_centre_code_c"));
                        SetValue(AColumnNames, ref ANewRow, "a_account_code_c", GetValue(BudgetColumnNames, OldRow, "a_account_code_c"));
                        SetValue(AColumnNames, ref ANewRow, "a_budget_type_code_c", GetValue(BudgetColumnNames, OldRow, "a_budget_type_code_c"));
                        SetValue(AColumnNames, ref ANewRow, "a_budget_status_l", GetValue(BudgetColumnNames, OldRow, "a_budget_status_l"));
                        SetValue(AColumnNames, ref ANewRow, "a_comment_c", GetValue(BudgetColumnNames, OldRow, "a_comment_c"));

                        FixABudgetType(AColumnNames, ref ANewRow);

                        AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());

                        if (AWriterTest != null)
                        {
                            AWriterTest.WriteLine("BEGIN; " + "COPY a_budget FROM stdin;");
                            AWriterTest.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                            AWriterTest.WriteLine("\\.");
                            AWriterTest.WriteLine("ROLLBACK;");
                        }
                    }
                }
            }

            // set a new sequence number for a_budget
            TDumpProgressToPostgresql.BudgetSequenceNumber = RowCounter + 1;

            return RowCounter;
        }

        /// <summary>
        /// Fix table a_budget_period
        /// </summary>
        /// <remarks>
        /// In Petra, one ABudgetPeriod could cover three years. These must be split up for OP with one record for each year.
        /// </remarks>
        public static int FixABudgetPeriod(StringCollection AColumnNames,
            ref string[] ANewRow,
            StreamWriter AWriter,
            StreamWriter AWriterTest)
        {
            // load the file a_budget.d.gz so that we can access the values for each budget
            TTable BudgetTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_budget");

            TParseProgressCSV BudgetParser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_budget.d.gz",
                BudgetTableOld.grpTableField.Count);

            StringCollection BudgetColumnNames = GetColumnNames(BudgetTableOld);

            // load the file a_budget_period.d.gz
            TTable BudgetPeriodTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_budget_period");

            TParseProgressCSV BudgetPeriodParser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_budget_period.d.gz",
                BudgetPeriodTableOld.grpTableField.Count);

            StringCollection BudgetPeriodColumnNames = GetColumnNames(BudgetPeriodTableOld);

            // read first row in ABudgetPeriod
            string[] OldBudgetPeriodRow = BudgetPeriodParser.ReadNextRow();

            // [0] last year, [1] current year, [2] next year. Null if budget does not exist for a year.
            string[] BudgetYears = new string[3];

            List <string[]>BudgetPeriodRows = new List <string[]>();
            string LedgerNumber = string.Empty;
            int RowCounter = 0;
            int SequenceNumber = 0;

            SetValue(AColumnNames, ref ANewRow, "s_date_created_d", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_created_by_c", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_date_modified_d", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_modified_by_c", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_modification_id_t", "\\N");

            while (true)
            {
                BudgetYears[0] = null;
                BudgetYears[2] = null;
                string[] OldRow = BudgetParser.ReadNextRow();

                if (OldRow == null)
                {
                    break;
                }

                if (LedgerNumber != GetValue(BudgetColumnNames, OldRow, "a_ledger_number_i"))
                {
                    LedgerNumber = GetValue(BudgetColumnNames, OldRow, "a_ledger_number_i");

                    // gets the current financial year for the current ledger
                    GetLedgerCurrentYear(LedgerNumber, out BudgetYears[1]);
                }

                GetBudgetPeriods(BudgetPeriodParser, BudgetPeriodColumnNames, ref OldBudgetPeriodRow,
                    GetValue(BudgetColumnNames, OldRow, "a_budget_sequence_i"), out BudgetPeriodRows);
                GetBudgetYears(BudgetPeriodColumnNames, BudgetPeriodRows, ref BudgetYears);

                for (int i = 0; i < 3; i++)
                {
                    if (BudgetYears[i] != null)
                    {
                        SequenceNumber++;

                        foreach (string[] BudgetPeriodRow in BudgetPeriodRows)
                        {
                            RowCounter++;

                            SetValue(AColumnNames, ref ANewRow, "a_budget_sequence_i", SequenceNumber.ToString());
                            SetValue(AColumnNames, ref ANewRow, "a_period_number_i",
                                GetValue(BudgetPeriodColumnNames, BudgetPeriodRow, "a_period_number_i"));

                            if (i == 0)
                            {
                                SetValue(AColumnNames, ref ANewRow, "a_budget_base_n",
                                    GetValue(BudgetPeriodColumnNames, BudgetPeriodRow, "a_budget_last_year_n"));
                            }
                            else if (i == 1)
                            {
                                SetValue(AColumnNames, ref ANewRow, "a_budget_base_n",
                                    GetValue(BudgetPeriodColumnNames, BudgetPeriodRow, "a_budget_this_year_n"));
                            }
                            else if (i == 2)
                            {
                                SetValue(AColumnNames, ref ANewRow, "a_budget_base_n",
                                    GetValue(BudgetPeriodColumnNames, BudgetPeriodRow, "a_budget_next_year_n"));
                            }

                            AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());

                            if (AWriterTest != null)
                            {
                                AWriterTest.WriteLine("BEGIN; " + "COPY a_budget_period FROM stdin;");
                                AWriterTest.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                                AWriterTest.WriteLine("\\.");
                                AWriterTest.WriteLine("ROLLBACK;");
                            }
                        }
                    }
                }
            }

            return RowCounter;
        }

        /// Get all ABudgetPeriod records for a budget sequence number
        private static void GetBudgetPeriods(TParseProgressCSV AParser,
            StringCollection ABudgetPeriodColumnNames,
            ref string[] AOldRow,
            string AOldSequenceNumber,
            out List <string[]>ABudgetPeriods)
        {
            // true once a record with the budget sequence number has been found
            bool BudgetSequenceFound = false;

            ABudgetPeriods = new List <string[]>();

            while (true)
            {
                if (GetValue(ABudgetPeriodColumnNames, AOldRow, "a_budget_sequence_i") == AOldSequenceNumber)
                {
                    ABudgetPeriods.Add(AOldRow);

                    BudgetSequenceFound = true;
                }
                else if (BudgetSequenceFound)
                {
                    // we have now parsed through all records for the budget sequence number
                    return;
                }

                AOldRow = AParser.ReadNextRow();

                if ((AOldRow == null) && !BudgetSequenceFound)
                {
                    throw new Exception(
                        "TFinanceBudgetUpgrader.GetBudgetYears: No ABudgetPeriod records found for ABudget sequence number: " + AOldSequenceNumber);
                }
                else if (AOldRow == null)
                {
                    // reached end of table
                    break;
                }
            }
        }

        /// Find if data exists for last year and next year in a list of ABudgetPeriods.
        private static void GetBudgetYears(StringCollection ABudgetPeriodColumnNames, List <string[]>ABudgetPeriodRows, ref string[] ABudgetYears)
        {
            foreach (string[] BudgetPeriodRow in ABudgetPeriodRows)
            {
                if ((ABudgetYears[0] == null) && (GetValue(ABudgetPeriodColumnNames, BudgetPeriodRow, "a_budget_last_year_n") != "0"))
                {
                    ABudgetYears[0] = (Convert.ToInt32(ABudgetYears[1]) - 1).ToString();

                    // break once budget is found to have last year and next year data
                    if (ABudgetYears[2] != null)
                    {
                        return;
                    }
                }

                if ((ABudgetYears[2] == null) && (GetValue(ABudgetPeriodColumnNames, BudgetPeriodRow, "a_budget_next_year_n") != "0"))
                {
                    ABudgetYears[2] = (Convert.ToInt32(ABudgetYears[1]) + 1).ToString();

                    // break once budget is found to have last year and next year data
                    if ((ABudgetYears[0] != null) || (ABudgetYears[1] == "0"))
                    {
                        return;
                    }
                }
            }
        }

        private static void GetLedgerCurrentYear(string ALedgerNumber, out string ALedgerYearCurrentYear)
        {
            // load the file a_ledger.d.gz
            TTable LedgerTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_ledger");

            TParseProgressCSV Parser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_ledger.d.gz",
                LedgerTableOld.grpTableField.Count);

            StringCollection LedgerColumnNames = GetColumnNames(LedgerTableOld);

            ALedgerYearCurrentYear = "0";

            while (true)
            {
                string[] OldRow = Parser.ReadNextRow();

                if (OldRow == null)
                {
                    break;
                }

                if (GetValue(LedgerColumnNames, OldRow, "a_ledger_number_i") == ALedgerNumber)
                {
                    ALedgerYearCurrentYear = GetValue(LedgerColumnNames, OldRow, "a_current_financial_year_i");
                    break;
                }
            }
        }
    }
}