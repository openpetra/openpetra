//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MReporting;
using Ict.Common;
using Ict.Common.IO; // Implicit reference
using Ict.Common.Data; // Implicit reference
using Ict.Common.DB.DBCaching;
using System.Data;
using System.Collections;
using System.Data.Odbc;

namespace Ict.Petra.Server.MReporting.MFinance
{
    /// <summary>
    /// functions for the finance module
    /// </summary>
    public class TRptUserFunctionsFinance : TRptUserFunctions
    {
        private static TSQLCache ActualsCache = new TSQLCache();
        private static TSQLCache AccountDescendantsCache = new TSQLCache();

        /// <summary>
        /// constructor
        /// </summary>
        public TRptUserFunctionsFinance() : base()
        {
        }

        /// <summary>
        /// Don't remember anything from the last report...
        /// </summary>
        public static void FlushSqlCache()
        {
            ActualsCache.Invalidate();
        }

        private string UnitKeyToForeignCostCentre(Int64 pv_unit_partner_key_n)
        {
            string ReturnValue;
            Int64 ledgerNumber;

            ledgerNumber = pv_unit_partner_key_n / 1000000;

            if (ledgerNumber < 10)
            {
                ReturnValue = '0' + ledgerNumber.ToString() + "00";
            }
            else
            {
                ReturnValue = ledgerNumber.ToString() + "00";
            }

            return ReturnValue;
        }

        /// <summary>
        /// functions need to be registered here
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="f"></param>
        /// <param name="ops"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override Boolean FunctionSelector(TRptSituation ASituation, String f, TVariant[] ops, out TVariant value)
        {
            if (base.FunctionSelector(ASituation, f, ops, out value))
            {
                return true;
            }

            if (StringHelper.IsSame(f, "getAccountDetailAmount"))
            {
                value = GetAccountDetailAmount(ops[1].ToDecimal(), ops[2].ToBool());
                return true;
            }

            if (StringHelper.IsSame(f, "getTransactionAmount"))
            {
                value = new TVariant(GetTransactionAmount(ops[1].ToString()), "currency");
                return true;
            }

            if (StringHelper.IsSame(f, "getAssetsMinusLiabs"))
            {
                value = new TVariant(GetAssetsMinusLiabs(ops[1].ToInt(), ops[2].ToInt()), "currency");
                return true;
            }

            if (StringHelper.IsSame(f, "getNetBalance"))
            {
                value = new TVariant(GetNetBalance(ops[1].ToInt()), "currency");
                return true;
            }

            if (StringHelper.IsSame(f, "getGLMSequences"))
            {
                GetGlmSequences(ops[1].ToInt(), ops[2].ToString(), ops[3].ToString(), ops[4].ToInt());
                value = new TVariant();
                return true;
            }

            if (StringHelper.IsSame(f, "getActual"))
            {
                value = new TVariant(GetActual(ops[1].ToInt(), ops[2].ToInt(), ops[3].ToBool(), ops[4].ToString()), "currency");
                return true;
            }

            if (StringHelper.IsSame(f, "getActualPeriods"))
            {
                value = new TVariant(GetActualPeriods(ops[1].ToInt(), ops[2].ToInt(), ops[3].ToInt(), ops[4].ToBool(),
                        ops[5].ToString()), "currency");
                return true;
            }

            if (StringHelper.IsSame(f, "getActualPeriodsIE"))
            {
                value = new TVariant(GetActualPeriodsIE(ops[1].ToInt(), ops[2].ToInt(), ops[3].ToInt(), ops[4].ToBool(),
                        ops[5].ToString()), "currency");
                return true;
            }

            if (StringHelper.IsSame(f, "getActualEndOfLastYear"))
            {
                value = new TVariant(GetActualEndOfLastYear(ops[1].ToInt(), ops[2].ToInt(), ops[3].ToString()), "currency");
                return true;
            }

            if (StringHelper.IsSame(f, "GetBudgetPeriods"))
            {
                value = new TVariant(GetBudgetPeriods(ops[1].ToInt(), ops[2].ToInt(), ops[3].ToInt(), ops[4].ToBool(),
                        ops[5].ToString()), "currency");
                return true;
            }

            if (StringHelper.IsSame(f, "getLedgerName"))
            {
                value = new TVariant(GetLedgerName(ops[1].ToInt()));
                return true;
            }

            if (StringHelper.IsSame(f, "UnitKeyToForeignCostCentre"))
            {
                value = new TVariant(UnitKeyToForeignCostCentre(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "getCurrency"))
            {
                value = new TVariant(GetCurrency(ops[1].ToInt(), ops[2].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "getBalanceSheetType"))
            {
                value = new TVariant(GetBalanceSheetType(ops[1].ToString(), ops[2].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "getIncExpStmtType"))
            {
                value = new TVariant(GetIncExpStmtType(ops[1].ToString(), ops[2].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "getAccountingHierarchy"))
            {
                value = new TVariant(GetAccountingHierarchy(ops[1].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetLedgerPerColumn"))
            {
                value = new TVariant(GetLedgerPerColumn());
                return true;
            }

            if (StringHelper.IsSame(f, "getCurrencyPerColumn"))
            {
                value = new TVariant(GetCurrencyPerColumn());
                return true;
            }

            if (StringHelper.IsSame(f, "GetYTDPerColumn"))
            {
                value = new TVariant(GetYTDPerColumn());
                return true;
            }

            if (StringHelper.IsSame(f, "getAllAccountDescendants"))
            {
                value = new TVariant("CSV:" + GetAllAccountDescendants(ops[1].ToInt(), ops[2].ToString(), ops[3].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "ExtractPaymentNumberFromTransactionNarrative"))
            {
                value = new TVariant("CSV:" + ExtractPaymentNumberFromTransactionNarrative(ops[1].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetMonthName"))
            {
                value = new TVariant(StringHelper.GetLongMonthName(ops[1].ToInt32()));
                return true;
            }

            value = new TVariant();
            return false;
        }

        private decimal GetAssetsMinusLiabs(int master, int column)
        {
            decimal ReturnValue;
            decimal assets;
            decimal liabs;
            Boolean notnull;
            ArrayList list;

            ReturnValue = 0;
            list = new ArrayList();
            notnull = false;
            assets = 0;
            liabs = 0;
            situation.GetResults().GetChildRows(master, ref list);

            foreach (TResult element in list)
            {
                if (element.code.IndexOf("ASSETS") != -1)
                {
                    assets = element.column[column].ToDecimal();
                }

                if (element.code.IndexOf("LIABS") != -1)
                {
                    liabs = element.column[column].ToDecimal();
                }

                notnull = true;
            }

            list = null;

            if (notnull)
            {
                ReturnValue = (assets - liabs);
            }

            return ReturnValue;
        }

        private string GetLedgerPerColumn()
        {
            string ReturnValue;

            ReturnValue = "";

            if ((!parameters.Get("param_selected_ledgers", situation.GetColumn(), -1, eParameterFit.eExact).IsZeroOrNull()))
            {
                ReturnValue = ReturnValue + "Ledger " + parameters.Get("param_selected_ledgers",
                    situation.GetColumn(), -1, eParameterFit.eExact).ToString();
            }
            else if (parameters.Exists("param_ledger_number_i", situation.GetColumn(), -1, eParameterFit.eExact))
            {
                ReturnValue = ReturnValue + "Ledger " +
                              parameters.Get("param_ledger_number_i", situation.GetColumn(), -1, eParameterFit.eExact).ToString();
            }

            return ReturnValue;
        }

        private string GetCurrencyPerColumn()
        {
            string ReturnValue;
            int ledgernumber;
            String reportcurrency;
            String columncurrency;

            ReturnValue = "";
            ledgernumber = parameters.Get("param_ledger_number_i", situation.GetColumn()).ToInt();
            reportcurrency = parameters.Get("param_currency", -1).ToString().ToLower();
            columncurrency = parameters.Get("param_currency", situation.GetColumn()).ToString().ToLower();

            if ((reportcurrency == "mixed") || (reportcurrency == "depends on column"))
            {
                ReturnValue = GetCurrency(ledgernumber, columncurrency);
            }

            return ReturnValue;
        }

        private string GetYTDPerColumn()
        {
            string ReturnValue;
            String reportytd;
            Boolean columnytd;

            System.Int32 Quarter;
            TRptUserFunctionsDate FnDate;
            ReturnValue = "";
            reportytd = parameters.Get("param_ytd", -1).ToString().ToLower();
            columnytd = parameters.Get("param_ytd", situation.GetColumn()).ToBool();

            if ((reportytd == "mixed") && columnytd)
            {
                ReturnValue = ReturnValue + "YTD";
            }

            if ((reportytd == "mixed") && (!columnytd))
            {
                // result := result + 'non YTD';
                if (parameters.Get("param_quarter").ToBool())
                {
                    Quarter = (parameters.Get("param_end_period_i", this.situation.GetColumn()).ToInt() - 1) / 3 + 1;
                    ReturnValue = "Quarter " + Quarter.ToString();
                }
                else
                {
                    FnDate = new TRptUserFunctionsDate(this.situation);
                    ReturnValue =
                        FnDate.GetMonthName(parameters.Get("param_ledger_number_i",
                                this.situation.GetColumn()).ToInt(),
                            parameters.Get("param_end_period_i", this.situation.GetColumn()).ToInt());
                }
            }

            // print the name of the month instead of nonytd, on multi period reports
            if (parameters.Get("param_multiperiod").ToBool() && (columnytd == false))
            {
                FnDate = new TRptUserFunctionsDate(this.situation);
                ReturnValue =
                    FnDate.GetMonthName(parameters.Get("param_ledger_number_i",
                            this.situation.GetColumn()).ToInt(),
                        parameters.Get("param_column_period_i", this.situation.GetColumn()).ToInt());
            }

            return ReturnValue;
        }

        private string GetCurrency(int ledgernumber, String param_currency)
        {
            string ReturnValue;
            string strSql;
            DataTable tab;
            String currencyFormat;

            ReturnValue = "";

            if (param_currency == "Transaction")
            {
                ReturnValue = "All";
            }
            else if ((param_currency.ToLower() == "mixed") || (param_currency.ToLower() == "depends on column"))
            {
                ReturnValue = "mixed";
            }
            else
            {
                strSql = "SELECT a_base_currency_c, a_intl_currency_c FROM PUB_a_ledger WHERE a_ledger_number_i = ?";
                tab = situation.GetDatabaseConnection().SelectDT(strSql, "currencies",
                    situation.GetDatabaseConnection().Transaction, new OdbcParameter[] { new OdbcParameter("ledger", (System.Object)ledgernumber) });

                if (tab.Rows.Count > 0)
                {
                    if (param_currency.ToLower() == "base")
                    {
                        ReturnValue = Convert.ToString(tab.Rows[0]["a_base_currency_c"]);
                    }
                    else if ((param_currency.ToLower() == "intl") || (param_currency.ToLower() == "international"))
                    {
                        ReturnValue = Convert.ToString(tab.Rows[0]["a_intl_currency_c"]);
                    }
                }
            }

            if (parameters.Exists("param_currency_format"))
            {
                currencyFormat = parameters.Get("param_currency_format").ToString();

                if (StringHelper.IsSame(currencyFormat, "CurrencyThousands"))
                {
                    ReturnValue = ReturnValue + " (in Thousands)";
                }

                // else if (StringHelper.IsSame(currencyFormat, 'CurrencyWithoutDecimals')) then
                // result := result + ' (without decimals)'
            }

            return ReturnValue;
        }

        private String GetBalanceSheetType(String param_type, String param_currency)
        {
            return GetIncExpStmtType(param_type, param_currency);
        }

        private String GetIncExpStmtType(String param_type, String param_currency)
        {
            String ReturnValue = "";

            if (param_type.ToLower().CompareTo("summary") == 0)
            {
                ReturnValue = "Summary Report";
            }
            else if (param_type.ToLower().CompareTo("standard") == 0)
            {
                ReturnValue = "Standard Report";
            }
            else if (param_type.ToLower().CompareTo("detail") == 0)
            {
                ReturnValue = "Detailed Report";
            }

            ReturnValue = ReturnValue + "    (";

            if (param_currency.ToLower().CompareTo("base") == 0)
            {
                ReturnValue = ReturnValue + "Base";
            }
            else if ((param_currency.ToLower().CompareTo("intl") == 0) || (param_currency.ToLower().CompareTo("international") == 0))
            {
                ReturnValue = ReturnValue + "International";
            }
            else if ((param_currency.ToLower().CompareTo("mixed") == 0) || (param_currency.ToLower().CompareTo("depends on column") == 0))
            {
                ReturnValue = ReturnValue + "Mixed Currencies";
            }

            return ReturnValue + ")";
        }

        private String GetAccountingHierarchy(String param_accounting_hierarchy)
        {
            String ReturnValue;

            ReturnValue = "";

            if (param_accounting_hierarchy.CompareTo("STANDARD") != 0)
            {
                ReturnValue = ReturnValue + "Accounting Hierarchy: " + param_accounting_hierarchy;
            }

            return ReturnValue;
        }

        private decimal GetTransactionAmount(String currency_s)
        {
            decimal ReturnValue;

            ReturnValue = 0;
            currency_s = currency_s.ToLower();

            if (currency_s.ToLower().CompareTo("transaction") == 0)
            {
                ReturnValue = parameters.Get("a_transaction_amount_n", situation.GetColumn(), situation.GetDepth()).ToDecimal();
            }
            else
            {
                if (currency_s.ToLower().CompareTo("base") == 0)
                {
                    ReturnValue = parameters.Get("a_amount_in_base_currency_n", situation.GetColumn(), situation.GetDepth()).ToDecimal();
                }
                else
                {
                    if (currency_s.ToLower().CompareTo("international") == 0)
                    {
                        ReturnValue = parameters.Get("a_amount_in_intl_currency_n", situation.GetColumn(), situation.GetDepth()).ToDecimal();
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// if the current account is a debit account xor amount is negative, then return the negative amount;
        /// else returns the positive amount
        /// </summary>
        /// <returns>void</returns>
        public TVariant GetAccountDetailAmount(decimal amount, Boolean debit_credit_indicator)
        {
            TVariant ReturnValue;

            if (debit_credit_indicator)
            {
                ReturnValue = new TVariant(amount, "currency");
            }
            else
            {
                ReturnValue = new TVariant(amount * -1, "currency");
            }

            return ReturnValue;
        }

        private decimal GetNetBalance(int line)
        {
            decimal ReturnValue;
            decimal col1;
            decimal col2;
            TResult element;

            ReturnValue = 0.0M;
            element = situation.GetResults().GetFirstChildRow(line);

            if (element != null)
            {
                col1 = element.column[0].ToDecimal();
                col2 = element.column[1].ToDecimal();
                ReturnValue = col1 + col2;
            }

            return ReturnValue;
        }

        /// <summary>
        /// calls getGlmSequence for the given year and the year before and the year after.
        /// saves the glm sequence numbers in the variables glm_seq_last_year_i, glm_seq_this_year_i and glm_seq_next_year_i
        /// if several ledgers are used, it selects the data from the other ledgers: well, in the same year
        /// </summary>
        /// <param name="pv_ledger_number_i"></param>
        /// <param name="pv_cost_centre_code_c">If it is empty, it is filled with the main costcentre, e.g. [10]
        /// </param>
        /// <param name="pv_account_code_c"></param>
        /// <param name="pv_year_i"></param>
        public void GetGlmSequences(int pv_ledger_number_i, String pv_cost_centre_code_c, String pv_account_code_c, int pv_year_i)
        {
            int col;
            int ledgernr;
            string cost_centre_other;
            Boolean use_several_ledgers;
            TGlmSequence glmSequence;

            if (pv_cost_centre_code_c.Length == 0)
            {
                pv_cost_centre_code_c = GetMainCostCentre(pv_ledger_number_i);
            }

            col = situation.GetColumn();

            if (col == ReportingConsts.ALLCOLUMNS)
            {
                col = -1;
            }

            glmSequence = LedgerStatus.GlmSequencesCache.GetGlmSequenceCurrentYear(
                situation.GetDatabaseConnection(), pv_ledger_number_i, pv_cost_centre_code_c, pv_account_code_c,
                situation.GetParameters().Get("param_current_financial_year_i", -1).ToInt());

            if ((glmSequence != null) && (glmSequence.glmSequence != -1))
            {
                situation.GetParameters().Add("glm_sequence_i", glmSequence.glmSequence, col, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                situation.GetParameters().Add("debit_credit_indicator",
                    glmSequence.DebitCreditIndicator,
                    col,
                    -1,
                    null,
                    null,
                    ReportingConsts.CALCULATIONPARAMETERS);
            }
            else
            {
                situation.GetParameters().Add("glm_sequence_i", new TVariant(), col, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                situation.GetParameters().Add("debit_credit_indicator", new TVariant(), col, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
            }

            use_several_ledgers = (situation.GetParameters().Get("use_several_ledgers").ToBool() == true);

            if (use_several_ledgers)
            {
                col = 1;

                while (situation.GetParameters().Exists("param_calculation", col, -1, eParameterFit.eExact))
                {
                    ledgernr = situation.GetParameters().Get("param_ledger_number_i", col, -1, eParameterFit.eExact).ToInt();

                    if (ledgernr != -1)
                    {
                        cost_centre_other = pv_cost_centre_code_c.Replace(pv_ledger_number_i.ToString(), ledgernr.ToString());
                        glmSequence = LedgerStatus.GlmSequencesCache.GetGlmSequenceCurrentYear(
                            situation.GetDatabaseConnection(), ledgernr, cost_centre_other, pv_account_code_c,
                            situation.GetParameters().Get("param_current_financial_year_i", col).ToInt());

                        if ((glmSequence != null) && (glmSequence.glmSequence != -1))
                        {
                            situation.GetParameters().Add("glm_sequence_i",
                                glmSequence.glmSequence,
                                col,
                                -1,
                                null,
                                null,
                                ReportingConsts.CALCULATIONPARAMETERS);
                            situation.GetParameters().Add("debit_credit_indicator",
                                glmSequence.DebitCreditIndicator,
                                col,
                                -1,
                                null,
                                null,
                                ReportingConsts.CALCULATIONPARAMETERS);
                        }
                        else
                        {
                            situation.GetParameters().Add("glm_sequence_i",
                                new TVariant(), col, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                            situation.GetParameters().Add("debit_credit_indicator",
                                new TVariant(), col, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                        }
                    }

                    col = col + 1;
                }
            }
        }

        /// <summary>
        /// get the main (root) cost centre of the ledger
        /// </summary>
        /// <returns>the main cost centre of the given ledger, e.g. [10]</returns>
        private string GetMainCostCentre(int pv_ledger_number_i)
        {
            string ReturnValue;
            string strSql;
            DataTable tab;

            ReturnValue = "";
            strSql = "SELECT a_cost_centre_code_c " + "FROM PUB_a_cost_centre " + "WHERE a_ledger_number_i = " + StringHelper.IntToStr(
                pv_ledger_number_i) + " AND a_cost_centre_to_report_to_c = \"\"";
            tab = situation.GetDatabaseConnection().SelectDT(strSql, "", situation.GetDatabaseConnection().Transaction);

            if (tab.Rows.Count > 0)
            {
                ReturnValue = Convert.ToString(tab.Rows[0]["a_cost_centre_code_c"]);
            }

            return ReturnValue;
        }

        private decimal GetActualValue(TFinancialPeriod period, String pv_currency_select_c)
        {
            string strSql = "SELECT a_actual_base_n, a_actual_intl_n, a_actual_foreign_n " + "FROM PUB_a_general_ledger_master_period " +
                            "WHERE a_glm_sequence_i = " + StringHelper.IntToStr(period.realGlmSequence.glmSequence) + ' ' +
                            "AND a_period_number_i = " +
                            StringHelper.IntToStr(period.realPeriod);
            DataTable tab = ActualsCache.GetDataTable(situation.GetDatabaseConnection(), strSql);

            if (tab.Rows.Count > 0)
            {
                if (StringHelper.IsSame(pv_currency_select_c, "Base"))
                {
                    return Convert.ToDecimal(tab.Rows[0]["a_actual_base_n"]);
                }
                else if (StringHelper.IsSame(pv_currency_select_c, "Intl") || StringHelper.IsSame(pv_currency_select_c, "International"))
                {
                    return Convert.ToDecimal(tab.Rows[0]["a_actual_base_n"]) * period.exchangeRateToIntl;
                }
                else if (StringHelper.IsSame(pv_currency_select_c, "Transaction"))
                {
                    if (tab.Rows[0].IsNull("a_actual_foreign_n"))
                    {
                        // this is not a foreign currency account, so it must be base currency
                        return Convert.ToDecimal(tab.Rows[0]["a_actual_base_n"]);
                    }

                    return Convert.ToDecimal(tab.Rows[0]["a_actual_foreign_n"]);
                }
            }

            return 0.0M;
        }

        /// <summary>
        /// This will get the amount of the given summary account, by going through
        /// all the reporting posting accounts, and using the given account hierarchy;
        /// This is done even for the STANDARD hierarchy,
        /// though that values are stored in the database, they are calculated from the bottom again.
        ///
        /// </summary>
        /// <returns>void</returns>
        private decimal GetActualSummary(TFinancialPeriod periodParent,
            int pv_period_number_i,
            int pv_year_i,
            Boolean pv_ytd_l,
            String pv_currency_select_c,
            String accountHierarchy)
        {
            decimal ReturnValue;
            string accountChildren;
            string accountChild;
            decimal subAccountAmount;
            TFinancialPeriod subAccountPeriod;
            bool childDebitCreditIndicator;

            // get all the posting accounts that report to this account in the selected account hierarchy
            accountChildren = GetAllAccountDescendants(periodParent.realGlmSequence.ledger_number,
                periodParent.realGlmSequence.account_code,
                accountHierarchy);
            ReturnValue = 0;

            while (accountChildren.Length > 0)
            {
                accountChild = StringHelper.GetNextCSV(ref accountChildren);

                StringHelper.GetNextCSV(ref accountChildren);
                // alias
                StringHelper.GetNextCSV(ref accountChildren);

                // accountChildAccountDescr
                childDebitCreditIndicator = (StringHelper.GetNextCSV(ref accountChildren).ToUpper() == "TRUE");
                subAccountPeriod = new TFinancialPeriod(
                    situation.GetDatabaseConnection(), pv_period_number_i, pv_year_i, periodParent.diffPeriod, periodParent.FCurrentFinancialYear,
                    periodParent.FCurrentPeriod, periodParent.FNumberAccountingPeriods, periodParent.FNumberForwardingPeriods,
                    LedgerStatus.GlmSequencesCache.GetGlmSequenceCurrentYear(situation.GetDatabaseConnection(),
                        periodParent.realGlmSequence.ledger_number, periodParent.realGlmSequence.cost_centre_code, accountChild,
                        periodParent.FCurrentFinancialYear));
                subAccountAmount = GetActual(subAccountPeriod, pv_period_number_i, pv_year_i, pv_ytd_l, pv_currency_select_c);

                if (childDebitCreditIndicator != periodParent.realGlmSequence.DebitCreditIndicator)
                {
                    subAccountAmount = (-1) * subAccountAmount;
                }

                ReturnValue = ReturnValue + subAccountAmount;
            }

            return ReturnValue;
        }

        /// <summary>
        /// GetActual retrieves the actuals value of the given period, no matter if it is in a forwarding period.
        /// It will use the current glm_sequence_i.
        /// GetActual is similar to GetBudget. The main difference is, that forwarding periods are saved in the current year.
        /// You want e.g. the actual data of period 13 in year 2, the current financial year is 3.
        /// The call would look like: GetActual(sequence_year_2, sequence_year_3, 13, 12, 3, 2, false, "B");
        /// That means, the function has to return the difference between year 3 period 1 and the start balance of year 3.
        /// </summary>
        /// <param name="pv_period_number_i">If 0, then the start balance of the year (pv_this_year_i) is returned,
        /// otherwise the actual amount of the given period
        /// If it is a forwarding period, pv_this_year_i is checked against the current financial year,
        /// if not equal then the data of the next year is used</param>
        /// <param name="pv_year_i"></param>
        /// <param name="pv_ytd_l"></param>
        /// <param name="pv_currency_select_c"></param>
        /// <returns></returns>
        public decimal GetActual(int pv_period_number_i, int pv_year_i, Boolean pv_ytd_l, String pv_currency_select_c)
        {
            return GetActual(new TFinancialPeriod(situation.GetDatabaseConnection(), pv_period_number_i, pv_year_i, situation.GetParameters(),
                    situation.GetColumn()), pv_period_number_i, pv_year_i, pv_ytd_l, pv_currency_select_c);
        }

        /// <summary>
        /// GetActual retrieves the actuals value of the given period, no matter if it is in a forwarding period.
        /// GetActual is similar to GetBudget. The main difference is, that forwarding periods are saved in the current year.
        /// You want e.g. the actual data of period 13 in year 2, the current financial year is 3.
        /// The call would look like: GetActual(sequence_year_2, sequence_year_3, 13, 12, 3, 2, false, "B");
        /// That means, the function has to return the difference between year 3 period 1 and the start balance of year 3.
        /// </summary>
        /// <param name="period"></param>
        /// <param name="pv_period_number_i">If 0, then the start balance of the year (pv_this_year_i) is returned,
        /// otherwise the actual amount of the given period
        /// If it is a forwarding period, pv_this_year_i is checked against the current financial year,
        /// if not equal then the data of the next year is used</param>
        /// <param name="pv_year_i"></param>
        /// <param name="pv_ytd_l"></param>
        /// <param name="pv_currency_select_c"></param>
        /// <returns></returns>
        private decimal GetActual(TFinancialPeriod period, int pv_period_number_i, int pv_year_i, Boolean pv_ytd_l, String pv_currency_select_c)
        {
            TFinancialPeriod firstPeriod;
            TFinancialPeriod beforeFirstPeriodOfYear;
            TFinancialPeriod lastPeriodOfPrevYear;
            decimal lv_currency_amount_n;
            decimal lv_prev_year_amount_n;
            string strSql;
            DataTable tab;

            if ((period == null) || (period.realGlmSequence == null))
            {
                return 0.0M;
            }

            if (!period.RealPeriodExists())
            {
                if (pv_ytd_l)
                {
                    // go back to the last valid period
                    period.realPeriod = period.FCurrentPeriod + period.FNumberForwardingPeriods;
                }
                else
                {
                    return 0.0M;
                }
            }

            // for summary accounts, need to get the correct summary amount
            string accountHierarchy = parameters.Get("param_account_hierarchy_c").ToString();

            if (!period.realGlmSequence.postingAccount)
            {
                return GetActualSummary(period, pv_period_number_i, pv_year_i, pv_ytd_l, pv_currency_select_c, accountHierarchy);
            }

            lv_currency_amount_n = 0;

            if (period.realPeriod == 0)
            {
                // start balance
                strSql = "SELECT a_start_balance_base_n, a_start_balance_intl_n, a_start_balance_foreign_n " + "FROM PUB_a_general_ledger_master " +
                         "WHERE a_glm_sequence_i = " + StringHelper.IntToStr(period.realGlmSequence.glmSequence);
                tab = ActualsCache.GetDataTable(situation.GetDatabaseConnection(), strSql);

                if (tab.Rows.Count > 0)
                {
                    if (pv_currency_select_c == "Base")
                    {
                        lv_currency_amount_n = Convert.ToDecimal(tab.Rows[0]["a_start_balance_base_n"]);
                    }
                    else
                    {
                        if (pv_currency_select_c == "Intl")
                        {
                            lv_currency_amount_n = Convert.ToDecimal(tab.Rows[0]["a_start_balance_base_n"]) * period.exchangeRateToIntl;
                        }
                        else
                        {
                            if (tab.Rows[0].IsNull("a_start_balance_foreign_n"))
                            {
                                // there is no foreign currency, this is an account in base currency
                                lv_currency_amount_n = Convert.ToDecimal(tab.Rows[0]["a_start_balance_base_n"]);
                            }
                            else
                            {
                                lv_currency_amount_n = Convert.ToDecimal(tab.Rows[0]["a_start_balance_foreign_n"]);
                            }
                        }
                    }
                }
            }
            else
            {
                lv_currency_amount_n = GetActualValue(period, pv_currency_select_c);

                if (pv_ytd_l)
                {
/*
 * is this something about 13 accounting periods?
 * it caused bug 833, and the unit tests for forwarding periods now work as in Progress reports
 * This code has been there since delphi.net;
 * delphi for win32 did not have it, but there was something else
 * where the amount from period 12 was subtracted
 *
 *        if ((pv_period_number_i > period.FNumberAccountingPeriods) && (period.realGlmSequence.incExpAccount) && (pv_year_i == period.FCurrentFinancialYear))
 *        {
 *          previousPeriod = new TFinancialPeriod(situation.GetDatabaseConnection(), period, -1, situation.GetParameters(), situation.GetColumn());
 *          lv_currency_amount_n = GetActualValue(previousPeriod, pv_currency_select_c);
 *          previousPeriod = null;
 *        }
 */
                    if ((period.diffPeriod != 0) && (period.realGlmSequence.incExpAccount))
                    {
                        // start balance starts with 0
                        firstPeriod = new TFinancialPeriod(situation.GetDatabaseConnection(), 1, pv_year_i, period);

                        if (firstPeriod.realYear != period.realYear)
                        {
                            // add the amount from the last year (real)
                            lastPeriodOfPrevYear = new TFinancialPeriod(
                                situation.GetDatabaseConnection(), period.FNumberAccountingPeriods - period.diffPeriod, pv_year_i - 1, period);
                            lv_prev_year_amount_n = 0;

                            if (lastPeriodOfPrevYear.realGlmSequence != null)
                            {
                                lv_prev_year_amount_n = GetActualValue(lastPeriodOfPrevYear, pv_currency_select_c);
                            }

                            lastPeriodOfPrevYear = null;
                            lv_currency_amount_n = lv_currency_amount_n + lv_prev_year_amount_n;
                        }

                        // substract the value that is before the first period of the (artificial) year
                        beforeFirstPeriodOfYear = new TFinancialPeriod(situation.GetDatabaseConnection(), 0, pv_year_i, period);
                        lv_prev_year_amount_n = 0;

                        if (beforeFirstPeriodOfYear.realGlmSequence != null)
                        {
                            lv_prev_year_amount_n = GetActualValue(beforeFirstPeriodOfYear, pv_currency_select_c);
                        }

                        beforeFirstPeriodOfYear = null;
                        lv_currency_amount_n = lv_currency_amount_n - lv_prev_year_amount_n;
                    }
                }
                else
                {
                    // not pv_ytd_l
                    lv_currency_amount_n = lv_currency_amount_n - GetActual(pv_period_number_i - 1, pv_year_i, true, pv_currency_select_c);
                }
            }

            period = null;
            return lv_currency_amount_n;
        }

        private string GetLedgerName(int ledgernumber)
        {
            string ReturnValue;
            string strSql;
            DataTable tab;

            ReturnValue = "";
            strSql = "SELECT p_partner_short_name_c FROM PUB_a_ledger, PUB_p_partner WHERE a_ledger_number_i=" +
                     StringHelper.IntToStr(ledgernumber) + " and PUB_a_ledger.p_partner_key_n = PUB_p_partner.p_partner_key_n";
            tab = situation.GetDatabaseConnection().SelectDT(strSql, "", situation.GetDatabaseConnection().Transaction);

            if (tab.Rows.Count > 0)
            {
                ReturnValue = Convert.ToString(tab.Rows[0]["p_partner_short_name_c"]);
            }

            return ReturnValue;
        }

        /// <summary>
        /// If you want the data of year 2 and period 10, you would call GetActualEndOfLastYear(sequence_year_1, sequence_year_2, 10, 12, 2, 2, "B");
        /// ' this would return the value of year 1 period 12.
        /// ' If you want the data of year 2 and period 14, you would call GetActualEndOfLastYear(sequence_year_1, sequence_year_2, 14, 12, 2, 2, "B");
        /// ' this would return the value of year 2 period 12.
        ///
        /// </summary>
        /// <returns>void</returns>
        private decimal GetActualEndOfLastYear(int pv_period_number_i, int pv_year_i, String pv_currency_select_c)
        {
            int numberAccountingPeriods;

            numberAccountingPeriods = situation.GetParameters().Get("param_number_of_accounting_periods_i", situation.GetColumn()).ToInt();

            // for forwarding periods, the last year is actually the current year...
            if (pv_period_number_i > numberAccountingPeriods)
            {
                pv_year_i = pv_year_i + 1;
            }

            return GetActual(numberAccountingPeriods, pv_year_i - 1, true, pv_currency_select_c);
        }

        /// <summary>
        /// calls getActual, and calculates the correct ytd or nonytd amount for the given period(s)
        /// </summary>
        /// <returns>void</returns>
        private decimal GetActualPeriods(int pv_start_period_number_i,
            int pv_end_period_number_i,
            int pv_year_i,
            Boolean pv_ytd_l,
            String pv_currency_select_c)
        {
            decimal ReturnValue;

            if (pv_ytd_l)
            {
                ReturnValue = GetActual(pv_end_period_number_i, pv_year_i, true, pv_currency_select_c);
            }
            else
            {
                ReturnValue = GetActual(pv_end_period_number_i, pv_year_i, true, pv_currency_select_c) - GetActual(pv_start_period_number_i - 1,
                    pv_year_i,
                    true,
                    pv_currency_select_c);
            }

            return ReturnValue;
        }

        /// <summary>
        /// calls getActual, and calculates the correct ytd or nonytd amount for the given period(s)
        /// special version for Income &amp; Expense: the values are reset at the year end
        /// </summary>
        /// <returns>void</returns>
        private decimal GetActualPeriodsIE(int pv_start_period_number_i,
            int pv_end_period_number_i,
            int pv_year_i,
            Boolean pv_ytd_l,
            String pv_currency_select_c)
        {
            decimal ReturnValue = GetActualPeriods(pv_start_period_number_i, pv_end_period_number_i, pv_year_i, pv_ytd_l, pv_currency_select_c);

            if (pv_ytd_l)
            {
                int numberAccountingPeriods = situation.GetParameters().Get("param_number_of_accounting_periods_i", situation.GetColumn()).ToInt();

                if (pv_end_period_number_i > numberAccountingPeriods)
                {
                    ReturnValue = ReturnValue - GetActual(numberAccountingPeriods, pv_year_i, true, pv_currency_select_c);
                }
            }

            return ReturnValue;
        }

        private decimal GetBudgetSummary(TFinancialPeriod StartPeriodParent,
            TFinancialPeriod EndPeriodParent,
            int pv_period_number_i,
            int pv_year_i,
            String accountHierarchy)
        {
            decimal ReturnValue;
            string accountChildren;
            string accountChild;
            decimal subAccountAmount;
            TFinancialPeriod subAccountEndPeriod;
            TFinancialPeriod subAccountStartPeriod;
            bool childDebitCreditIndicator;

            // get all the posting accounts that report to this account in the selected account hierarchy
            accountChildren = GetAllAccountDescendants(StartPeriodParent.realGlmSequence.ledger_number,
                StartPeriodParent.realGlmSequence.account_code,
                accountHierarchy);
            ReturnValue = 0;

            // precondition: the parent periods must be in the same real year; that is taken care of in CalculateBudget
            if (StartPeriodParent.realYear != EndPeriodParent.realYear)
            {
                return 0;
            }

            while (accountChildren.Length > 0)
            {
                accountChild = StringHelper.GetNextCSV(ref accountChildren);

                StringHelper.GetNextCSV(ref accountChildren);
                // alias
                StringHelper.GetNextCSV(ref accountChildren);

                // accountChildAccountDescr
                childDebitCreditIndicator = (StringHelper.GetNextCSV(ref accountChildren).ToUpper() == "TRUE");
                subAccountStartPeriod = new TFinancialPeriod(
                    situation.GetDatabaseConnection(), pv_period_number_i, pv_year_i, StartPeriodParent.diffPeriod,
                    StartPeriodParent.FCurrentFinancialYear, StartPeriodParent.FCurrentPeriod, StartPeriodParent.FNumberAccountingPeriods,
                    StartPeriodParent.FNumberForwardingPeriods,
                    LedgerStatus.GlmSequencesCache.GetGlmSequenceCurrentYear(situation.GetDatabaseConnection(),
                        StartPeriodParent.realGlmSequence.ledger_number, StartPeriodParent.realGlmSequence.cost_centre_code, accountChild,
                        StartPeriodParent.FCurrentFinancialYear));
                subAccountEndPeriod = new TFinancialPeriod(subAccountStartPeriod);
                subAccountEndPeriod.realPeriod = EndPeriodParent.realPeriod;
                subAccountAmount = GetBudget(subAccountStartPeriod, subAccountEndPeriod, pv_period_number_i, pv_year_i);

                if (childDebitCreditIndicator != StartPeriodParent.realGlmSequence.DebitCreditIndicator)
                {
                    subAccountAmount = -1 * subAccountAmount;
                }

                ReturnValue = ReturnValue + subAccountAmount;
            }

            return ReturnValue;
        }

        private decimal GetBudget(TFinancialPeriod startperiod, TFinancialPeriod endperiod, Int32 periodNr, Int32 yearNr)
        {
            decimal ReturnValue;
            string strSql;
            DataTable tab;
            string accountHierarchy;

            ReturnValue = 0;

            if ((startperiod == null) || (startperiod.realGlmSequence == null) || (startperiod.realGlmSequence.glmSequence <= -1))
            {
                return 0;
            }

            // precondition: the periods must be in the same real year; that is taken care of in CalculateBudget
            if (startperiod.realYear != endperiod.realYear)
            {
                return 0;
            }

            // for summary accounts, when using a different than the STANDARD accounting hierarchy,
            // need to get the correct summary amount
            accountHierarchy = parameters.Get("param_account_hierarchy_c").ToString();

            if ((!startperiod.realGlmSequence.postingAccount) && (!(accountHierarchy.ToUpper().CompareTo("STANDARD") == 0)))
            {
                return GetBudgetSummary(startperiod, endperiod, periodNr, yearNr, accountHierarchy);
            }

            strSql = "SELECT SUM(a_budget_base_n) " + "FROM PUB_a_general_ledger_master_period " + "WHERE a_glm_sequence_i = " +
                     StringHelper.IntToStr(startperiod.realGlmSequence.glmSequence) + ' ' + "AND a_period_number_i >= " + StringHelper.IntToStr(
                startperiod.realPeriod) + ' ' + "AND a_period_number_i <= " + StringHelper.IntToStr(endperiod.realPeriod);
            tab = situation.GetDatabaseConnection().SelectDT(strSql, "", situation.GetDatabaseConnection().Transaction);

            if (tab.Rows.Count > 0)
            {
                ReturnValue = ReturnValue + Convert.ToDecimal(tab.Rows[0][0]);
            }

            return ReturnValue;
        }

        private decimal CalculateBudget(int pv_start_period_i, int pv_end_period_i, int pv_year_i, String pv_currency_select_c)
        {
            decimal ReturnValue;

            //int lv_ytd_period_i;
            TFinancialPeriod startPeriod;
            TFinancialPeriod endPeriod;
            TFinancialPeriod endOfYearPeriod;
            TFinancialPeriod beginOfYearPeriod;
            decimal lastExchangeRate;

            System.Int32 Counter;
            ReturnValue = 0;
            lastExchangeRate = -1;
            startPeriod = new TFinancialPeriod(situation.GetDatabaseConnection(), pv_start_period_i, pv_year_i,
                situation.GetParameters(), situation.GetColumn());

            if (startPeriod.realPeriod > startPeriod.FNumberAccountingPeriods)
            {
                // budgets are not stored in the forwarding periods, but in the next year
                startPeriod = new TFinancialPeriod(
                    situation.GetDatabaseConnection(), pv_start_period_i - startPeriod.FNumberAccountingPeriods, pv_year_i + 1,
                    situation.GetParameters(), situation.GetColumn());
            }

            endPeriod = new TFinancialPeriod(situation.GetDatabaseConnection(), pv_end_period_i, pv_year_i,
                situation.GetParameters(), situation.GetColumn());

            if (endPeriod.realPeriod > endPeriod.FNumberAccountingPeriods)
            {
                // budgets are not stored in the forwarding periods, but in the next year
                endPeriod = new TFinancialPeriod(
                    situation.GetDatabaseConnection(), pv_end_period_i - endPeriod.FNumberAccountingPeriods, pv_year_i + 1,
                    situation.GetParameters(), situation.GetColumn());
            }

            // make sure, endperiod is valid, ie. not beyond the existing periods
            Counter = pv_end_period_i - 1;

            while (((endPeriod == null) || (endPeriod.realGlmSequence == null)) && (Counter >= pv_start_period_i))
            {
                endPeriod = new TFinancialPeriod(situation.GetDatabaseConnection(), Counter, pv_year_i,
                    situation.GetParameters(), situation.GetColumn());
                Counter = Counter - 1;
            }

            // GetBudget can only deal with a range of periods in the same year (same glm sequence)
            if (startPeriod.realYear != endPeriod.realYear)
            {
                endOfYearPeriod = new TFinancialPeriod(startPeriod);
                endOfYearPeriod.realPeriod = endOfYearPeriod.FNumberAccountingPeriods;
                ReturnValue = ReturnValue + GetBudget(startPeriod, endOfYearPeriod, pv_start_period_i, pv_year_i);
                beginOfYearPeriod = new TFinancialPeriod(endPeriod);
                endOfYearPeriod.realPeriod = 1;
                ReturnValue = ReturnValue + GetBudget(beginOfYearPeriod, endPeriod, pv_end_period_i, pv_year_i);
            }
            else
            {
                ReturnValue = ReturnValue + GetBudget(startPeriod, endPeriod, pv_start_period_i, pv_year_i);
            }

            if (StringHelper.IsSame(pv_currency_select_c, "Intl"))
            {
                if ((endPeriod != null) && (endPeriod.realGlmSequence != null))
                {
                    lastExchangeRate = endPeriod.exchangeRateToIntl;
                    ReturnValue = ReturnValue * lastExchangeRate;
                }
            }

            return ReturnValue;
        }

        private decimal GetBudgetPeriods(int pv_start_period_i, int pv_end_period_i, int pv_year_i, Boolean pv_ytd_l, String pv_currency_select_c)
        {
            int numberAccountingPeriods;

            if (pv_ytd_l)
            {
                pv_start_period_i = 1;
                numberAccountingPeriods = situation.GetParameters().Get("param_number_of_accounting_periods_i", situation.GetColumn()).ToInt();

                if (pv_end_period_i > numberAccountingPeriods)
                {
                    pv_start_period_i = numberAccountingPeriods + 1;
                }
            }

            return CalculateBudget(pv_start_period_i, pv_end_period_i, pv_year_i, pv_currency_select_c);
        }

        private string GetAllAccountDescendants(int pv_ledger_number_i, string pv_account_code_c, string pv_account_hierarchy_c)
        {
            string ReturnValue;
            DataTable tab;
            TRptCalculation rptCalculation;
            TRptDataCalcCalculation rptDataCalcCalculation;
            TVariant calculationResult;

            ReturnValue = "";
            parameters.Add("param_parentaccountcode", pv_account_code_c);
            rptCalculation = situation.GetReportStore().GetCalculation(situation.GetCurrentReport(), "Select AllAccountDescendants");
            rptDataCalcCalculation = new TRptDataCalcCalculation(situation);
            calculationResult = rptDataCalcCalculation.EvaluateCalculationAll(rptCalculation,
                null,
                rptCalculation.rptGrpTemplate,
                rptCalculation.rptGrpQuery);

            if (calculationResult.IsZeroOrNull())
            {
                return ReturnValue;
            }

            if ((calculationResult.ToString().ToUpper().IndexOf("SELECT") >= 0) && (calculationResult.ToString().ToUpper().IndexOf("SELECT") <= 3))
            {
                // this is an sql statement and not a function result
                tab = AccountDescendantsCache.GetDataTable(situation.GetDatabaseConnection(), calculationResult.ToString());

                foreach (DataRow row in tab.Rows)
                {
                    if (!Convert.ToBoolean(row["a_posting_status_l"]))
                    {
                        ReturnValue =
                            StringHelper.ConcatCSV(ReturnValue,
                                GetAllAccountDescendants(pv_ledger_number_i, Convert.ToString(row["line_a_account_code_c"]), pv_account_hierarchy_c));
                    }
                    else
                    {
                        ReturnValue =
                            StringHelper.AddCSV(ReturnValue,
                                Convert.ToString(row["line_a_account_code_c"]));
                        ReturnValue =
                            StringHelper.AddCSV(ReturnValue,
                                Convert.ToString(row["line_a_account_alias_c"]));
                        ReturnValue =
                            StringHelper.AddCSV(ReturnValue,
                                Convert.ToString(row["account_code_short_desc"]));
                        ReturnValue = StringHelper.AddCSV(ReturnValue, Convert.ToString(row["debit_credit_indicator"]));
                    }
                }
            }

            return ReturnValue;
        }

        private String ExtractPaymentNumberFromTransactionNarrative(String ATransactionNarrativesCSV)
        {
            String ReturnValue;
            String list;
            String narrative;

            list = ATransactionNarrativesCSV;

            // TLogging.Log('before reading payment numbers: ' + list);
            ReturnValue = "";

            while (list.Length > 0)
            {
                narrative = StringHelper.GetNextCSV(ref list, "/");

                // a_transaction.narrative: AP Payment: 53 AP: 30 31  British Telecom
                if ((narrative.Substring(0, 12) == "AP Payment: ") && (narrative.IndexOf("AP:") > 12))
                {
                    narrative = narrative.Substring(12);
                    ReturnValue = StringHelper.AddCSV(ReturnValue, narrative.Substring(0, -1));
                }
            }

            // TLogging.Log('Result: ' + result);
            return ReturnValue;
        }
    }
}