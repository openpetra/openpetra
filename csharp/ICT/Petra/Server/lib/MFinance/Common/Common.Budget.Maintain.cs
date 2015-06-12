//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//		 cthomas
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
using System.Data;

using Ict.Common.DB;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// </summary>
    public static class TCommonBudgetMaintain
    {
        /// <summary>
        /// GetGLMSequence
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACostCentreCode"></param>
        /// <param name="AYear"></param>
        /// <returns>GLM Sequence no</returns>
        [RequireModulePermission("FINANCE-3")]
        public static int GetGLMSequenceForBudget(int ALedgerNumber, string AAccountCode, string ACostCentreCode, int AYear)
        {
            int retVal;
            AGeneralLedgerMasterTable GeneralLedgerMasterTable = null;

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    GeneralLedgerMasterTable = AGeneralLedgerMasterAccess.LoadByUniqueKey(ALedgerNumber,
                        AYear,
                        AAccountCode,
                        ACostCentreCode,
                        Transaction);
                });

            if (GeneralLedgerMasterTable.Count > 0)
            {
                retVal = (int)GeneralLedgerMasterTable.Rows[0].ItemArray[0];
            }
            else
            {
                retVal = -1;
            }

            return retVal;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------
        /// Description: GetActual retrieves the actuals value of the given period, no matter if it is in a forwarding period.
        ///  GetActual is similar to GetBudget. The main difference is, that forwarding periods are saved in the current year.
        ///  You still need the sequence_next_year, because this_year can be older than current_financial_year of the ledger.
        ///  So you need to give number_accounting_periods and current_financial_year of the ledger.
        ///  You also need to give the number of the year from which you want the data.
        ///  Currency_select is either "B" for base or "I" for international currency or "T" for transaction currency
        ///  You want e.g. the actual data of period 13 in year 2, the current financial year is 3.
        ///  The call would look like: GetActual(sequence_year_2, sequence_year_3, 13, 12, 3, 2, false, "B");
        ///  That means, the function has to return the difference between year 3 period 1 and the start balance of year 3.
        /// ------------------------------------------------------------------------------
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AGLMSeqThisYear"></param>
        /// <param name="AGLMSeqNextYear"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="ANumberAccountingPeriods"></param>
        /// <param name="ACurrentFinancialYear"></param>
        /// <param name="AThisYear"></param>
        /// <param name="AYTD"></param>
        /// <param name="ACurrencySelect"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static decimal GetActual(int ALedgerNumber,
            int AGLMSeqThisYear,
            int AGLMSeqNextYear,
            int APeriodNumber,
            int ANumberAccountingPeriods,
            int ACurrentFinancialYear,
            int AThisYear,
            bool AYTD,
            string ACurrencySelect)
        {
            decimal retVal = 0;

            retVal = GetActualInternal(ALedgerNumber,
                AGLMSeqThisYear,
                AGLMSeqNextYear,
                APeriodNumber,
                ANumberAccountingPeriods,
                ACurrentFinancialYear,
                AThisYear,
                AYTD,
                false,
                ACurrencySelect);

            return retVal;
        }

        /// <summary>
        /// Get the actual amount
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AGLMSeqThisYear"></param>
        /// <param name="AGLMSeqNextYear"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="ANumberAccountingPeriods"></param>
        /// <param name="ACurrentFinancialYear"></param>
        /// <param name="AThisYear"></param>
        /// <param name="AYTD"></param>
        /// <param name="ABalSheetForwardPeriods"></param>
        /// <param name="ACurrencySelect"></param>
        /// <returns></returns>
        private static decimal GetActualInternal(int ALedgerNumber,
            int AGLMSeqThisYear,
            int AGLMSeqNextYear,
            int APeriodNumber,
            int ANumberAccountingPeriods,
            int ACurrentFinancialYear,
            int AThisYear,
            bool AYTD,
            bool ABalSheetForwardPeriods,
            string ACurrencySelect)
        {
            decimal retVal = 0;

            decimal currencyAmount = 0;
            bool incExpAccountFwdPeriod = false;

            //DEFINE BUFFER a_glm_period FOR a_general_ledger_master_period.
            //DEFINE BUFFER a_glm FOR a_general_ledger_master.
            //DEFINE BUFFER buf_account FOR a_account.

            if (AGLMSeqThisYear == -1)
            {
                return retVal;
            }

            bool newTransaction = false;
            TDBTransaction dBTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out newTransaction);

            AGeneralLedgerMasterTable generalLedgerMasterTable = null;
            AGeneralLedgerMasterRow generalLedgerMasterRow = null;

            AGeneralLedgerMasterPeriodTable generalLedgerMasterPeriodTable = null;
            AGeneralLedgerMasterPeriodRow generalLedgerMasterPeriodRow = null;

            AAccountTable AccountTable = null;
            AAccountRow AccountRow = null;

            try
            {
                if (APeriodNumber == 0)             /* start balance */
                {
                    generalLedgerMasterTable = AGeneralLedgerMasterAccess.LoadByPrimaryKey(AGLMSeqThisYear, dBTransaction);
                    generalLedgerMasterRow = (AGeneralLedgerMasterRow)generalLedgerMasterTable.Rows[0];

                    switch (ACurrencySelect)
                    {
                        case MFinanceConstants.CURRENCY_BASE:
                            currencyAmount = generalLedgerMasterRow.StartBalanceBase;
                            break;

                        case MFinanceConstants.CURRENCY_INTERNATIONAL:
                            currencyAmount = generalLedgerMasterRow.StartBalanceIntl;
                            break;

                        default:
                            currencyAmount = generalLedgerMasterRow.StartBalanceForeign;
                            break;
                    }
                }
                else if (APeriodNumber > ANumberAccountingPeriods)             /* forwarding periods only exist for the current financial year */
                {
                    if (ACurrentFinancialYear == AThisYear)
                    {
                        generalLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSeqThisYear,
                            APeriodNumber,
                            dBTransaction);
                        generalLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)generalLedgerMasterPeriodTable.Rows[0];
                    }
                    else
                    {
                        generalLedgerMasterPeriodTable =
                            AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSeqNextYear,
                                (APeriodNumber - ANumberAccountingPeriods),
                                dBTransaction);
                        generalLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)generalLedgerMasterPeriodTable.Rows[0];
                    }
                }
                else             /* normal period */
                {
                    generalLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSeqThisYear, APeriodNumber, dBTransaction);
                    generalLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)generalLedgerMasterPeriodTable.Rows[0];
                }

                if (generalLedgerMasterPeriodRow != null)
                {
                    switch (ACurrencySelect)
                    {
                        case MFinanceConstants.CURRENCY_BASE:
                            currencyAmount = generalLedgerMasterPeriodRow.ActualBase;
                            break;

                        case MFinanceConstants.CURRENCY_INTERNATIONAL:
                            currencyAmount = generalLedgerMasterPeriodRow.ActualIntl;
                            break;

                        default:
                            currencyAmount = generalLedgerMasterPeriodRow.ActualForeign;
                            break;
                    }
                }

                if ((APeriodNumber > ANumberAccountingPeriods) && (ACurrentFinancialYear == AThisYear))
                {
                    generalLedgerMasterTable = AGeneralLedgerMasterAccess.LoadByPrimaryKey(AGLMSeqThisYear, dBTransaction);
                    generalLedgerMasterRow = (AGeneralLedgerMasterRow)generalLedgerMasterTable.Rows[0];

                    AccountTable = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, generalLedgerMasterRow.AccountCode, dBTransaction);
                    AccountRow = (AAccountRow)AccountTable.Rows[0];

                    if ((AccountRow.AccountCode.ToUpper() == MFinanceConstants.ACCOUNT_TYPE_INCOME.ToUpper())
                        || (AccountRow.AccountCode.ToUpper() == MFinanceConstants.ACCOUNT_TYPE_EXPENSE.ToUpper())
                        && !ABalSheetForwardPeriods)
                    {
                        incExpAccountFwdPeriod = true;
                        currencyAmount -= GetActualInternal(ALedgerNumber,
                            AGLMSeqThisYear,
                            AGLMSeqNextYear,
                            ANumberAccountingPeriods,
                            ANumberAccountingPeriods,
                            ACurrentFinancialYear,
                            AThisYear,
                            true,
                            ABalSheetForwardPeriods,
                            ACurrencySelect);
                    }
                }

                if (!AYTD)
                {
                    if (!((APeriodNumber == (ANumberAccountingPeriods + 1)) && incExpAccountFwdPeriod)
                        && !((APeriodNumber == (ANumberAccountingPeriods + 1)) && (ACurrentFinancialYear > AThisYear)))
                    {
                        /* if it is an income expense acount, and we are in period 13, nothing needs to be subtracted,
                         * because that was done in correcting the amount in the block above;
                         * if we are in a previous year, in period 13, don't worry about subtracting.
                         *
                         * THIS IS CLEARLY INCORRECT - THE CONDITION ABOVE APPLIES *ONLY* IN THE FIRST FORWARDING PERIOD, NOT IN EVERY FORWARDING PERIOD.
                         * IF THE METHOD IS ONLY CALLED FROM AUTOGENERATE BUDGETS, THIS IS PROBABLY INCONSEQUENTIAL.
                         */
                        currencyAmount -= GetActualInternal(ALedgerNumber,
                            AGLMSeqThisYear,
                            AGLMSeqNextYear,
                            (APeriodNumber - 1),
                            ANumberAccountingPeriods,
                            ACurrentFinancialYear,
                            AThisYear,
                            true,
                            ABalSheetForwardPeriods,
                            ACurrencySelect);
                    }
                }

                retVal = currencyAmount;
            }
            finally
            {
                if (newTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return retVal;
        }

        /// <summary>
        /// Retrieves a budget value
        /// </summary>
        /// <param name="AGLMSeqThisYear"></param>
        /// <param name="AGLMSeqNextYear"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="ANumberAccountingPeriods"></param>
        /// <param name="AYTD"></param>
        /// <param name="ACurrencySelect"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static decimal GetBudget(int AGLMSeqThisYear,
            int AGLMSeqNextYear,
            int APeriodNumber,
            int ANumberAccountingPeriods,
            bool AYTD,
            string ACurrencySelect)
        {
            decimal retVal = 0;

            if (APeriodNumber > ANumberAccountingPeriods)
            {
                retVal = CalculateBudget(AGLMSeqNextYear, 1, (APeriodNumber - ANumberAccountingPeriods), AYTD, ACurrencySelect);
            }
            else
            {
                retVal = CalculateBudget(AGLMSeqThisYear, 1, APeriodNumber, AYTD, ACurrencySelect);
            }

            return retVal;
        }

        /// <summary>
        ///Description: CalculateBudget is only used internally as a helper function for GetBudget.
        ///Returns the budget for the given period of time,
        ///  if ytd is set, this period is from start_period to end_period,
        ///  otherwise it is only the value of the end_period.
        ///  currency_select is either "B" for base or "I" for international currency
        /// </summary>
        /// <param name="AGLMSeq"></param>
        /// <param name="AStartPeriod"></param>
        /// <param name="AEndPeriod"></param>
        /// <param name="AYTD"></param>
        /// <param name="ACurrencySelect"></param>
        /// <returns>Budget Amount</returns>
        private static decimal CalculateBudget(int AGLMSeq, int AStartPeriod, int AEndPeriod, bool AYTD, string ACurrencySelect)
        {
            decimal retVal = 0;

            decimal lv_currency_amount_n = 0;
            int lv_ytd_period_i;

            if (AGLMSeq == -1)
            {
                return retVal;
            }

            AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = null;
            AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow = null;

            if (!AYTD)
            {
                AStartPeriod = AEndPeriod;
            }

            TDBTransaction transaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref transaction,
                delegate
                {
                    for (lv_ytd_period_i = AStartPeriod; lv_ytd_period_i <= AEndPeriod; lv_ytd_period_i++)
                    {
                        GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSeq, lv_ytd_period_i, transaction);
                        GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];

                        if (GeneralLedgerMasterPeriodRow != null)
                        {
                            if (ACurrencySelect == MFinanceConstants.CURRENCY_BASE)
                            {
                                lv_currency_amount_n += GeneralLedgerMasterPeriodRow.BudgetBase;
                            }
                            else if (ACurrencySelect == MFinanceConstants.CURRENCY_INTERNATIONAL)
                            {
                                lv_currency_amount_n += GeneralLedgerMasterPeriodRow.BudgetIntl;
                            }
                        }
                    }
                });

            retVal = lv_currency_amount_n;

            return retVal;
        }
    }
}