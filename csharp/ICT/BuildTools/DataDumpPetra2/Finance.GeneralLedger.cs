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
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Tools.DBXML;
using Ict.Common;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// this class helps with upgrading tables in GL
    /// </summary>
    public class TFinanceGeneralLedgerUpgrader : TFixData
    {
        private static SortedList <string, string>LedgersCurrentFinancialYear = null;

        private static void LoadLedgersCurrentFinancialYear()
        {
            LedgersCurrentFinancialYear = new SortedList <string, string>();

            // load a_ledger table and a_accounting_period
            TTable ledgerTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_ledger");

            TParseProgressCSV Parser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_ledger.d.gz",
                ledgerTableOld.grpTableField.Count);

            StringCollection LedgerColumnNames = GetColumnNames(ledgerTableOld);

            while (true)
            {
                string[] OldRow = Parser.ReadNextRow();

                if (OldRow == null)
                {
                    break;
                }

                string LedgerNumber = GetValue(LedgerColumnNames, OldRow, "a_ledger_number_i");
                string CurrentFinancialYear = GetValue(LedgerColumnNames, OldRow, "a_current_financial_year_i");
                LedgersCurrentFinancialYear.Add(LedgerNumber, CurrentFinancialYear);
            }

            TTable accountingPeriodTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_accounting_period");

            Parser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_accounting_period.d.gz",
                accountingPeriodTableOld.grpTableField.Count);

            StringCollection PeriodColumnNames = GetColumnNames(accountingPeriodTableOld);

            while (true)
            {
                string[] OldRow = Parser.ReadNextRow();

                if (OldRow == null)
                {
                    break;
                }

                string PeriodNumber = GetValue(PeriodColumnNames, OldRow, "a_accounting_period_number_i");

                if (PeriodNumber == "1")
                {
                    string LedgerNumber = GetValue(PeriodColumnNames, OldRow, "a_ledger_number_i");
                    string RealYear = GetValue(PeriodColumnNames, OldRow, "a_period_start_date_d").Substring(6, 4);
                    LedgersCurrentFinancialYear[LedgerNumber] += "," + RealYear;
                }
            }
        }

        /// <summary>
        /// fixing table a_batch
        /// </summary>
        public static bool FixABatch(StringCollection AColumnNames, ref string[] ANewRow)
        {
            // if no batch year has been set, make sure to set the right year
            string val = GetValue(AColumnNames, ANewRow, "a_batch_year_i");

            if ((val.Length == 0) || (val == "\\N") || (val == "0"))
            {
                string LedgerNumber = GetValue(AColumnNames, ANewRow, "a_ledger_number_i");
                int BatchRealYear = Convert.ToInt32(GetValue(AColumnNames, ANewRow, "a_date_effective_d").Substring(0, 4));

                if (LedgersCurrentFinancialYear == null)
                {
                    LoadLedgersCurrentFinancialYear();
                }

                string[] info = LedgersCurrentFinancialYear[LedgerNumber].Split(new char[] { ',' });
                int CurrentFinancialYear = Convert.ToInt32(info[0]);
                int CurrentRealYear = Convert.ToInt32(info[1]);

                int BatchFinancialYear = CurrentFinancialYear - (CurrentRealYear - BatchRealYear);

                // forwarding periods
                if (BatchFinancialYear > CurrentFinancialYear)
                {
                    BatchFinancialYear = CurrentFinancialYear;
                }

                SetValue(AColumnNames, ref ANewRow, "a_batch_year_i", BatchFinancialYear.ToString());
            }

            return true;
        }

        /// <summary>
        /// fixing table a_budget_type
        /// </summary>
        public static bool FixABudgetType(StringCollection AColumnNames, ref string[] ANewRow)
        {
            string BudgetTypeCode = GetValue(AColumnNames, ANewRow, "a_budget_type_code_c");

            if (BudgetTypeCode == "Same")
            {
                BudgetTypeCode = "SAME";
            }
            else if (BudgetTypeCode == "Split")
            {
                BudgetTypeCode = "SPLIT";
            }
            else if (BudgetTypeCode == "Adhoc")
            {
                BudgetTypeCode = "ADHOC";
            }
            else if (BudgetTypeCode == "Inf.Base")
            {
                BudgetTypeCode = "INFLATE_BASE";
            }
            else if (BudgetTypeCode == "Inf. n")
            {
                BudgetTypeCode = "INFLATE_N";
            }

            return true;
        }

        /// <summary>
        /// Supply the new a_report_column_c field
        /// </summary>
        /// <param name="AColumnNames"></param>
        /// <param name="ANewRow"></param>
        /// <returns>(false if the row should be dropped) - ALWAYS TRUE!</returns>
        public static bool FixAMotivationDetail(StringCollection AColumnNames, ref string[] ANewRow)
        {
            String motivationDetailCode = GetValue(AColumnNames, ANewRow, "a_motivation_detail_code_c");
            String ReportColumn = "Field";

            if ((motivationDetailCode == "SUPPORT") || (motivationDetailCode == "PERSONAL"))
            {
                ReportColumn = "Worker";
            }

            SetValue(AColumnNames, ref ANewRow, "a_report_column_c", ReportColumn);

            return true;
        }
    }
}