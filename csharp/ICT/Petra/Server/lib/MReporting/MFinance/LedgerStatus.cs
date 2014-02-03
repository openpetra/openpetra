//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.DB.DBCaching;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MReporting;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Server.MFinance.Common;

namespace Ict.Petra.Server.MReporting.MFinance
{
    /// <summary>
    /// holds some lists of cached values
    /// </summary>
    public class LedgerStatus
    {
        /// <summary>list of TGlmSequence System.Objects</summary>
        public static TGlmSequenceCache GlmSequencesCache = new TGlmSequenceCache();

        /// <summary>
        /// cache of exchange rates
        /// </summary>
        public static TCorporateExchangeRateCache ExchangeRateCache = new TCorporateExchangeRateCache();

        /// <summary>
        /// cache of sql results for exchange rates
        /// </summary>
        public static TSQLCache ExchangeRateCachedResultSets = new TSQLCache();
    }

    /// <summary>
    /// should help with forwarding periods, different financial years etc.
    /// </summary>
    public class TFinancialPeriod
    {
        /// <summary>todoComment</summary>
        public int diffPeriod;

        /// <summary>todoComment</summary>
        public int realYear;

        /// <summary>todoComment</summary>
        public int realPeriod;

        /// <summary>todoComment</summary>
        public TGlmSequence realGlmSequence;

        /// <summary>todoComment</summary>
        public decimal exchangeRateToIntl;

        /// <summary>todoComment</summary>
        public int FCurrentFinancialYear;

        /// <summary>todoComment</summary>
        public int FNumberAccountingPeriods;

        /// <summary>todoComment</summary>
        public int FCurrentPeriod;

        /// <summary>todoComment</summary>
        public int FNumberForwardingPeriods;

        /// <summary>
        /// called by the constructors
        /// </summary>
        /// <param name="databaseConnection"></param>
        /// <param name="period"></param>
        /// <param name="year">the selected year that the report should be on</param>
        /// <param name="diffPeriod"></param>
        /// <param name="ACurrentFinancialYear"></param>
        /// <param name="ACurrentPeriod"></param>
        /// <param name="ANumberAccountingPeriods"></param>
        /// <param name="ANumberForwardingPeriods"></param>
        /// <param name="glmSequence">in relation to year, not currentFinancialYear</param>
        public void MainConstructor(TDataBase databaseConnection,
            int period,
            int year,
            int diffPeriod,
            System.Int32 ACurrentFinancialYear,
            System.Int32 ACurrentPeriod,
            System.Int32 ANumberAccountingPeriods,
            System.Int32 ANumberForwardingPeriods,
            TGlmSequence glmSequence)
        {
            this.diffPeriod = diffPeriod;
            FCurrentFinancialYear = ACurrentFinancialYear;
            FNumberAccountingPeriods = ANumberAccountingPeriods;
            FCurrentPeriod = ACurrentPeriod;
            FNumberForwardingPeriods = ANumberForwardingPeriods;
            realPeriod = period + diffPeriod;
            realYear = year;

            if (glmSequence == null)
            {
                realGlmSequence = null;
            }
            else
            {
                realGlmSequence = LedgerStatus.GlmSequencesCache.GetOtherYearGlmSequence(databaseConnection, glmSequence, realYear);
            }

            // the period is in the last year
            // this treatment only applies to situations with different financial years.
            // in a financial year equals to the glm year, the period 0 represents the start balance
            if ((diffPeriod == 0) && (realPeriod == 0))
            {
                // leave it, period 0 represents the start balance
            }
            else if (realPeriod < 1)
            {
                realPeriod = FNumberAccountingPeriods + realPeriod;
                realYear = realYear - 1;
                realGlmSequence = LedgerStatus.GlmSequencesCache.GetOtherYearGlmSequence(databaseConnection, realGlmSequence, realYear);
            }

            // forwarding periods are only allowed in the current year
            if ((realPeriod > FNumberAccountingPeriods) && (realYear != FCurrentFinancialYear))
            {
                realPeriod = realPeriod - FNumberAccountingPeriods;
                realYear = realYear + 1;
                realGlmSequence = LedgerStatus.GlmSequencesCache.GetOtherYearGlmSequence(databaseConnection, realGlmSequence, realYear);
            }

            if (realGlmSequence != null)
            {
                exchangeRateToIntl = LedgerStatus.ExchangeRateCache.GetCorporateExchangeRate(databaseConnection,
                    realGlmSequence.ledger_number,
                    realYear,
                    realPeriod,
                    FCurrentFinancialYear);
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="databaseConnection"></param>
        /// <param name="period"></param>
        /// <param name="year"></param>
        /// <param name="diffPeriod"></param>
        /// <param name="ACurrentFinancialYear"></param>
        /// <param name="ACurrentPeriod"></param>
        /// <param name="ANumberAccountingPeriods"></param>
        /// <param name="ANumberForwardingPeriods"></param>
        /// <param name="glmSequence"></param>
        public TFinancialPeriod(TDataBase databaseConnection,
            int period,
            int year,
            int diffPeriod,
            System.Int32 ACurrentFinancialYear,
            System.Int32 ACurrentPeriod,
            System.Int32 ANumberAccountingPeriods,
            System.Int32 ANumberForwardingPeriods,
            TGlmSequence glmSequence)
        {
            MainConstructor(databaseConnection,
                period,
                year,
                diffPeriod,
                ACurrentFinancialYear,
                ACurrentPeriod,
                ANumberAccountingPeriods,
                ANumberForwardingPeriods,
                glmSequence);
        }

        /// <summary>
        /// if withExchangeRatesGLM is false, it
        /// will only set the correct values for realPeriod and realYear,
        /// </summary>
        /// <returns></returns>
        public TFinancialPeriod(TDataBase databaseConnection, int period, int year, TParameterList parameters, int column)
        {
            diffPeriod = 0;
            FCurrentFinancialYear = parameters.Get("param_current_financial_year_i", column).ToInt();
            FNumberAccountingPeriods = parameters.Get("param_number_of_accounting_periods_i", column).ToInt();
            FCurrentPeriod = parameters.Get("param_current_period_i", column).ToInt();
            FNumberForwardingPeriods = parameters.Get("param_number_fwd_posting_periods_i", column).ToInt();
            Int32 glmSequenceNumber = parameters.Get("glm_sequence_i", column).ToInt();
            realGlmSequence = LedgerStatus.GlmSequencesCache.GetGlmSequence(glmSequenceNumber);

            if ((realGlmSequence != null) && (realGlmSequence.year != year))
            {
                realGlmSequence = LedgerStatus.GlmSequencesCache.GetOtherYearGlmSequence(databaseConnection, realGlmSequence, year);
            }

            if (parameters.Exists("param_diff_period_i", column, -1))
            {
                diffPeriod = parameters.Get("param_diff_period_i", column).ToInt();
            }

            MainConstructor(databaseConnection,
                period,
                year,
                diffPeriod,
                FCurrentFinancialYear,
                FCurrentPeriod,
                FNumberAccountingPeriods,
                FNumberForwardingPeriods,
                realGlmSequence);
        }

        /// <summary>
        /// creates a period before or after the given period
        /// </summary>
        public TFinancialPeriod(TDataBase databaseConnection, TFinancialPeriod period, int diff, TParameterList parameters, int column)
            : this(databaseConnection, period.realPeriod + diff, period.realYear, parameters, column)
        {
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="APeriod"></param>
        public TFinancialPeriod(TFinancialPeriod APeriod)
        {
            diffPeriod = APeriod.diffPeriod;
            realYear = APeriod.realYear;
            realPeriod = APeriod.realPeriod;
            realGlmSequence = new TGlmSequence(APeriod.realGlmSequence);
            exchangeRateToIntl = APeriod.exchangeRateToIntl;
            FCurrentFinancialYear = APeriod.FCurrentFinancialYear;
            FNumberAccountingPeriods = APeriod.FNumberAccountingPeriods;
            FCurrentPeriod = APeriod.FCurrentPeriod;
            FNumberForwardingPeriods = APeriod.FNumberForwardingPeriods;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="databaseConnection"></param>
        /// <param name="period"></param>
        /// <param name="year"></param>
        /// <param name="AFinancialPeriod"></param>
        public TFinancialPeriod(TDataBase databaseConnection, int period, int year, TFinancialPeriod AFinancialPeriod) :
            this(databaseConnection, period, year, AFinancialPeriod.diffPeriod, AFinancialPeriod.FCurrentFinancialYear,
                 AFinancialPeriod.FCurrentPeriod, AFinancialPeriod.FNumberAccountingPeriods, AFinancialPeriod.FNumberForwardingPeriods,
                 AFinancialPeriod.realGlmSequence)
        {
        }

        /// the given period should not be changed with diffPeriod
        public TFinancialPeriod(TDataBase databaseConnection, int realPeriod, int year, TParameterList parameters, int column, bool real)
        {
            diffPeriod = 0;
            FCurrentFinancialYear = parameters.Get("param_current_financial_year_i", column).ToInt();
            FNumberAccountingPeriods = parameters.Get("param_number_of_accounting_periods_i", column).ToInt();
            FCurrentPeriod = parameters.Get("param_current_period_i", column).ToInt();
            FNumberForwardingPeriods = parameters.Get("param_number_of_accounting_periods_i", column).ToInt();
            Int32 glmSequenceNumber = parameters.Get("glm_sequence_i", column).ToInt();
            realGlmSequence = LedgerStatus.GlmSequencesCache.GetGlmSequence(glmSequenceNumber);
            MainConstructor(databaseConnection,
                realPeriod,
                year,
                diffPeriod,
                FCurrentFinancialYear,
                FCurrentPeriod,
                FNumberAccountingPeriods,
                FNumberForwardingPeriods,
                realGlmSequence);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public Boolean HasDifference()
        {
            return diffPeriod != 0;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public bool RealPeriodExists()
        {
            bool ReturnValue = true;

            if ((realYear == FCurrentFinancialYear) && (realPeriod > FCurrentPeriod + FNumberForwardingPeriods))
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TGlmSequence
    {
        /// <summary>todoComment</summary>
        public int glmSequence;

        /// <summary>todoComment</summary>
        public String account_code;

        /// <summary>todoComment</summary>
        public String cost_centre_code;

        /// <summary>
        /// is true if the account is of type Income or Expense;
        /// that means, the account has a start balance of 0 each year
        /// </summary>
        public Boolean incExpAccount;

        /// <summary>todoComment</summary>
        public Boolean postingAccount;

        /// <summary>todoComment</summary>
        public Boolean DebitCreditIndicator;

        /// <summary>todoComment</summary>
        public int ledger_number;

        /// <summary>todoComment</summary>
        public int year;

        /// <summary>todoComment</summary>
        public int currentFinancialYearSequence;

        /// <summary>todoComment</summary>
        public int currentFinancialYear;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ledgerNumber"></param>
        /// <param name="accountCode"></param>
        /// <param name="costCentreCode"></param>
        /// <param name="incExpAccount"></param>
        /// <param name="postingAccount"></param>
        /// <param name="debitCreditIndicator"></param>
        /// <param name="currentFinancialYearSequence"></param>
        /// <param name="currentFinancialYear"></param>
        public TGlmSequence(int ledgerNumber,
            String accountCode,
            String costCentreCode,
            Boolean incExpAccount,
            Boolean postingAccount,
            Boolean debitCreditIndicator,
            int currentFinancialYearSequence,
            int currentFinancialYear)
        {
            this.glmSequence = currentFinancialYearSequence;
            this.ledger_number = ledgerNumber;
            this.account_code = accountCode;
            this.cost_centre_code = costCentreCode;
            this.incExpAccount = incExpAccount;
            this.postingAccount = postingAccount;
            this.DebitCreditIndicator = debitCreditIndicator;
            this.currentFinancialYearSequence = currentFinancialYearSequence;
            this.currentFinancialYear = currentFinancialYear;
            this.year = currentFinancialYear;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="currentYearSequence"></param>
        /// <param name="glmsequence"></param>
        /// <param name="year"></param>
        public TGlmSequence(TGlmSequence currentYearSequence, int glmsequence, int year)
        {
            this.account_code = currentYearSequence.account_code;
            this.cost_centre_code = currentYearSequence.cost_centre_code;
            this.incExpAccount = currentYearSequence.incExpAccount;
            this.postingAccount = currentYearSequence.postingAccount;
            this.DebitCreditIndicator = currentYearSequence.DebitCreditIndicator;
            this.ledger_number = currentYearSequence.ledger_number;
            this.currentFinancialYearSequence = currentYearSequence.currentFinancialYearSequence;
            this.currentFinancialYear = currentYearSequence.currentFinancialYear;
            this.glmSequence = glmsequence;
            this.year = year;
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="glmSequence"></param>
        public TGlmSequence(TGlmSequence glmSequence)
        {
            this.account_code = glmSequence.account_code;
            this.cost_centre_code = glmSequence.cost_centre_code;
            this.incExpAccount = glmSequence.incExpAccount;
            this.postingAccount = glmSequence.postingAccount;
            this.DebitCreditIndicator = glmSequence.DebitCreditIndicator;
            this.ledger_number = glmSequence.ledger_number;
            this.currentFinancialYearSequence = glmSequence.currentFinancialYearSequence;
            this.currentFinancialYear = glmSequence.currentFinancialYear;
            this.glmSequence = glmSequence.glmSequence;
            this.year = glmSequence.year;
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TGlmSequenceCache
    {
        private ArrayList glmSequences;
        private Int32 NextNegativeSequence;

        /// <summary>
        /// constructor
        /// </summary>
        public TGlmSequenceCache() : base()
        {
            glmSequences = new ArrayList();
            NextNegativeSequence = -2;
        }

        private int GetGlmSequenceFromDB(TDataBase databaseConnection,
            int pv_ledger_number_i,
            String pv_cost_centre_code_c,
            String pv_account_code_c,
            int pv_year_i)
        {
            int ReturnValue = -1;
            String strSql = "SELECT a_glm_sequence_i FROM PUB_a_general_ledger_master WHERE a_ledger_number_i = " +
                            pv_ledger_number_i + " AND a_cost_centre_code_c = \"" + pv_cost_centre_code_c +
                            "\" AND a_account_code_c = \"" +
                            pv_account_code_c + "\" AND a_year_i = " + pv_year_i;
            DataTable tab = databaseConnection.SelectDT(strSql, "GetGlmSequenceFromDB_TempTable", databaseConnection.Transaction);

            if (tab.Rows.Count == 1)
            {
                ReturnValue = Convert.ToInt32(tab.Rows[0]["a_glm_sequence_i"]);
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="databaseConnection"></param>
        /// <param name="pv_ledger_number_i"></param>
        /// <param name="pv_account_code_c"></param>
        /// <param name="accountType">returns the type of account, ie. Income, Expense, Assets, Liab, Equity</param>
        /// <param name="postingAccount">returns true if the account is a posting account
        /// </param>
        /// <param name="debitCreditIndicator"></param>
        /// <returns></returns>
        private bool TAccountInfo(TDataBase databaseConnection,
            System.Int32 pv_ledger_number_i,
            String pv_account_code_c,
            out String accountType,
            out bool postingAccount,
            out bool debitCreditIndicator)
        {
            bool ReturnValue;
            string strSql;
            DataTable tab;

            ReturnValue = false;
            debitCreditIndicator = false;
            accountType = "";
            List <OdbcParameter>parameters = new List <OdbcParameter>();
            OdbcParameter param = new OdbcParameter("accountcode", OdbcType.VarChar);
            param.Value = pv_account_code_c;
            parameters.Add(param);
            postingAccount = false;
            strSql = "SELECT a_account_type_c, a_posting_status_l, a_debit_credit_indicator_l FROM PUB_a_account" +
                     " WHERE a_ledger_number_i = " + pv_ledger_number_i.ToString() +
                     " AND a_account_code_c = ?";
            tab =
                databaseConnection.SelectDT(strSql, "AccountType", databaseConnection.Transaction,
                    parameters.ToArray());

            if (tab.Rows.Count > 0)
            {
                accountType = Convert.ToString(tab.Rows[0]["a_account_type_c"]);
                postingAccount = (Boolean)tab.Rows[0]["a_posting_status_l"];
                debitCreditIndicator = (Boolean)tab.Rows[0]["a_debit_credit_indicator_l"];
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Get the sequence System.Object of a a_general_ledger_master
        /// </summary>
        /// <returns>the glm sequence System.Object of the a_general_ledger_master row of the given year; nil if there is no available a_general_ledger_master row
        /// </returns>
        public TGlmSequence GetGlmSequenceCurrentYear(TDataBase databaseConnection,
            int pv_ledger_number_i,
            String pv_cost_centre_code_c,
            String pv_account_code_c,
            int pv_current_financial_year_i)
        {
            TGlmSequence ReturnValue;
            int sequenceNumber;
            String accountType;
            bool postingAccount;
            bool debitCreditIndicator;

            // first check the local cache
            ReturnValue = null;

            foreach (TGlmSequence glmSequenceElement in glmSequences)
            {
                if ((glmSequenceElement.ledger_number == pv_ledger_number_i) && (glmSequenceElement.account_code == pv_account_code_c)
                    && (glmSequenceElement.cost_centre_code == pv_cost_centre_code_c) && (glmSequenceElement.year == pv_current_financial_year_i))
                {
                    ReturnValue = glmSequenceElement;
                    break;
                }
            }

            if (ReturnValue == null)
            {
                sequenceNumber = GetGlmSequenceFromDB(databaseConnection,
                    pv_ledger_number_i,
                    pv_cost_centre_code_c,
                    pv_account_code_c,
                    pv_current_financial_year_i);

                if (sequenceNumber == -1)
                {
                    // for the summary accounts from alternative account hierarchies,
                    // which don't have glm records.
                    sequenceNumber = NextNegativeSequence;
                    NextNegativeSequence = NextNegativeSequence - 1;
                }

                if (TAccountInfo(databaseConnection, pv_ledger_number_i, pv_account_code_c, out accountType, out postingAccount,
                        out debitCreditIndicator))
                {
                    TGlmSequence glmSequenceElement = new TGlmSequence(pv_ledger_number_i,
                        pv_account_code_c,
                        pv_cost_centre_code_c,
                        StringHelper.IsSame(accountType, "income") || StringHelper.IsSame(accountType, "expense"),
                        postingAccount,
                        debitCreditIndicator,
                        sequenceNumber,
                        pv_current_financial_year_i);
                    glmSequences.Add(glmSequenceElement);
                    ReturnValue = glmSequenceElement;
                }
                else
                {
                    // account could not be found
                    ReturnValue = null;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Get the sequence number of another year, using a known sequence number
        /// </summary>
        /// <param name="databaseConnection"></param>
        /// <param name="glmSequence">The known sequence number of a general_ledger_master year</param>
        /// <param name="year">The year of the required sequence</param>
        /// <returns>the glm sequence System.Object of the a_general_ledger_master row of the another year; null if there is no available a_general_ledger_master row
        /// </returns>
        ///
        public TGlmSequence GetOtherYearGlmSequence(TDataBase databaseConnection, TGlmSequence glmSequence, int year)
        {
            TGlmSequence ReturnValue;
            int sequenceNumber;

            if (glmSequence == null)
            {
                return null;
            }

            // first check the local cache
            ReturnValue = null;

            foreach (TGlmSequence glmSequenceElement in glmSequences)
            {
                if ((glmSequenceElement.currentFinancialYearSequence == glmSequence.currentFinancialYearSequence) && (glmSequenceElement.year == year))
                {
                    ReturnValue = glmSequenceElement;
                    break;
                }
            }

            if ((ReturnValue == null) && (glmSequence != null))
            {
                sequenceNumber = GetGlmSequenceFromDB(databaseConnection,
                    glmSequence.ledger_number,
                    glmSequence.cost_centre_code,
                    glmSequence.account_code,
                    year);

                if (sequenceNumber == -1)
                {
                    // for the summary accounts from alternative account hierarchies,
                    // which don't have glm records.
                    sequenceNumber = NextNegativeSequence;
                    NextNegativeSequence--;
                }

                TGlmSequence glmSequenceElement = new TGlmSequence(glmSequence, sequenceNumber, year);
                glmSequences.Add(glmSequenceElement);
                ReturnValue = glmSequenceElement;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Get the complete object to a known sequence number
        /// </summary>
        /// <param name="sequenceNumber">The known sequence number of a general_ledger_master year</param>
        /// <returns>the glm sequence object of the a_general_ledger_master row of the given sequence; nil if sequence is not in cache
        /// </returns>
        public TGlmSequence GetGlmSequence(int sequenceNumber)
        {
            foreach (TGlmSequence glmSequenceElement in glmSequences)
            {
                if (glmSequenceElement.glmSequence == sequenceNumber)
                {
                    return glmSequenceElement;
                }
            }

            return null;
        }
    }
}