//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.GL.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.GL.WebConnectors;

namespace Ict.Petra.Server.MFinance.Budget.WebConnectors
{
    /// <summary>
    /// maintain the budget
    /// </summary>
    public class TBudgetAutoGenerateWebConnector
    {
        /// <summary>
        /// Main Budget tables dataset
        /// </summary>
        public static BudgetTDS FMainDS = new BudgetTDS();

        /// <summary>
        /// load budget tables and return the budget table
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static BudgetTDS LoadBudgetForAutoGenerate(Int32 ALedgerNumber)
        {
            //TODO: need to filter on Year
            ABudgetAccess.LoadViaALedger(FMainDS, ALedgerNumber, null);
            ABudgetRevisionAccess.LoadViaALedger(FMainDS, ALedgerNumber, null);
            //TODO: need to filter on ABudgetPeriod using LoadViaBudget or LoadViaUniqueKey
            ABudgetPeriodAccess.LoadAll(FMainDS, null);
            ALedgerAccess.LoadByPrimaryKey(FMainDS, ALedgerNumber, null);

            // Accept row changes here so that the Client gets 'unmodified' rows
            FMainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            FMainDS.RemoveEmptyTables();

            return FMainDS;
        }

        /// <summary>
        /// Generate the budget for next year
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABudgetSeq"></param>
        /// <param name="AForecastType"></param>
        [RequireModulePermission("FINANCE-3")]
        public static bool GenBudgetForNextYear(int ALedgerNumber, int ABudgetSeq, string AForecastType)
        {
            bool retVal = false;

            decimal BudgetSum;
            decimal PriorAmount = 0;
            decimal AfterAmount = 0;
            int PeriodOfChange = 0;
            int GLMSequenceThisYear = 0;
            int GLMSequenceLastYear = 0;

            ABudgetTable BudgetTable = FMainDS.ABudget;
            ABudgetRow BudgetRow = (ABudgetRow)BudgetTable.Rows.Find(new object[] { ABudgetSeq });

            ALedgerTable LedgerTable = FMainDS.ALedger;
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            string AccountCode = BudgetRow.AccountCode;
            string CostCentreCode = BudgetRow.CostCentreCode;
            int CurrentFinancialYear = LedgerRow.CurrentFinancialYear;
            int CurrentPeriod = LedgerRow.CurrentPeriod;
            int NumAccPeriods = LedgerRow.NumberOfAccountingPeriods;

            GLMSequenceThisYear = TBudgetMaintainWebConnector.GetGLMSequenceForBudget(ALedgerNumber,
                AccountCode,
                CostCentreCode,
                CurrentFinancialYear);

            GLMSequenceLastYear = TBudgetMaintainWebConnector.GetGLMSequenceForBudget(ALedgerNumber,
                AccountCode,
                CostCentreCode,
                (CurrentFinancialYear - 1));
            try
            {
                //Update the budget status
                BudgetRow.BeginEdit();
                BudgetRow.BudgetStatus = false;
                BudgetRow.EndEdit();

                string BudgetType = BudgetRow.BudgetTypeCode.ToUpper();


                decimal BudgetAmount = 0;
                decimal ActualAmount = 0;
                bool ValidBudgetType = true;

                switch (BudgetType)
                {
                    case MFinanceConstants.BUDGET_ADHOC_U:
                    case MFinanceConstants.BUDGET_INFLATE_BASE_U:

                        for (int i = 1; i < CurrentPeriod; i++)
                        {
                            //Set budget period
                            ActualAmount = TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                GLMSequenceLastYear,
                                GLMSequenceThisYear,
                                i,
                                NumAccPeriods,
                                CurrentFinancialYear,
                                (CurrentFinancialYear - 1),
                                false,
                                MFinanceConstants.CURRENCY_BASE);
                            SetBudgetPeriod(ABudgetSeq, i, ActualAmount);
                        }

                        for (int j = CurrentPeriod; j <= MFinanceConstants.MAX_PERIODS; j++)
                        {
                            if (AForecastType == MFinanceConstants.FORECAST_TYPE_BUDGET)
                            {
                                BudgetAmount =
                                    Math.Round(TBudgetMaintainWebConnector.GetBudget(GLMSequenceThisYear, -1, j, NumAccPeriods,
                                            false,
                                            MFinanceConstants.CURRENCY_BASE));
                                SetBudgetPeriod(ABudgetSeq, j, BudgetAmount);
                            }
                            else
                            {
                                ActualAmount = TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceLastYear,
                                    GLMSequenceThisYear,
                                    j,
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    (CurrentFinancialYear - 1),
                                    false,
                                    MFinanceConstants.CURRENCY_BASE);
                                SetBudgetPeriod(ABudgetSeq, j, ActualAmount);
                            }
                        }

                        break;

                    case MFinanceConstants.BUDGET_SAME_U:                      //because this case has no code it will fall through to the next case until it finds code.
                    case MFinanceConstants.BUDGET_SPLIT_U:

                        if ((CurrentPeriod - 1) != 0)
                        {
                            BudgetSum =
                                TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceThisYear,
                                    -1,
                                    (CurrentPeriod - 1),
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    CurrentFinancialYear,
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);
                        }
                        else
                        {
                            BudgetSum = 0;
                        }

                        if (AForecastType == MFinanceConstants.FORECAST_TYPE_BUDGET)
                        {
                            for (int i = CurrentPeriod; i <= NumAccPeriods; i++)
                            {
                                BudgetSum += TBudgetMaintainWebConnector.GetBudget(GLMSequenceThisYear,
                                    -1,
                                    i,
                                    NumAccPeriods,
                                    false,
                                    MFinanceConstants.CURRENCY_BASE);
                            }
                        }
                        else
                        {
                            if (CurrentPeriod > 1)
                            {
                                BudgetSum += TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceLastYear,
                                    GLMSequenceThisYear,
                                    NumAccPeriods,
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    (CurrentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE) -
                                             TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceLastYear,
                                    GLMSequenceThisYear,
                                    (CurrentPeriod - 1),
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    (CurrentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);
                            }
                            else
                            {
                                BudgetSum += TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceLastYear,
                                    GLMSequenceThisYear,
                                    NumAccPeriods,
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    (CurrentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);
                            }
                        }

                        BudgetSum = BudgetSum / NumAccPeriods;

                        for (int i = 1; i <= NumAccPeriods; i++)
                        {
                            SetBudgetPeriod(ABudgetSeq, i, Math.Round(BudgetSum));
                        }

                        break;

                    case MFinanceConstants.BUDGET_INFLATE_N_U:

                        for (int i = 1; i <= NumAccPeriods; i++)
                        {
                            if (GetBudgetPeriodAmount(ABudgetSeq, i) != GetBudgetPeriodAmount(ABudgetSeq, 1))
                            {
                                PeriodOfChange = i - 1;
                                break;
                            }
                        }

                        /* Calculate average prior to change and after change. */
                        if (PeriodOfChange < (CurrentPeriod - 1))
                        {
                            PriorAmount = TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                GLMSequenceThisYear,
                                -1,
                                PeriodOfChange,
                                NumAccPeriods,
                                CurrentFinancialYear,
                                CurrentFinancialYear,
                                true,
                                MFinanceConstants.CURRENCY_BASE);

                            AfterAmount =
                                TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceThisYear,
                                    -1,
                                    (CurrentPeriod - 1),
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    CurrentFinancialYear,
                                    true,
                                    MFinanceConstants.CURRENCY_BASE) -
                                TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceThisYear,
                                    -1,
                                    PeriodOfChange + 1,
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    CurrentFinancialYear,
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);

                            if (AForecastType == MFinanceConstants.FORECAST_TYPE_BUDGET)
                            {
                                for (int i = CurrentPeriod; i <= NumAccPeriods; i++)
                                {
                                    AfterAmount += TBudgetMaintainWebConnector.GetBudget(GLMSequenceThisYear,
                                        -1,
                                        i,
                                        NumAccPeriods,
                                        false,
                                        MFinanceConstants.CURRENCY_BASE);
                                }
                            }
                            else
                            {
                                AfterAmount += TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceLastYear,
                                    GLMSequenceThisYear,
                                    NumAccPeriods,
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    (CurrentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE) -
                                               TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceLastYear,
                                    GLMSequenceThisYear,
                                    CurrentPeriod,
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    (CurrentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);
                            }
                        }
                        else                          /* Period of change HAS NOT taken place. */
                        {
                            if ((CurrentPeriod - 1) != 0)
                            {
                                PriorAmount =
                                    TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                        GLMSequenceThisYear,
                                        -1,
                                        (CurrentPeriod - 1),
                                        NumAccPeriods,
                                        CurrentFinancialYear,
                                        CurrentFinancialYear,
                                        true,
                                        MFinanceConstants.CURRENCY_BASE);
                            }
                            else
                            {
                                PriorAmount = 0;
                            }

                            if (AForecastType == MFinanceConstants.FORECAST_TYPE_BUDGET)
                            {
                                for (int i = CurrentPeriod; i <= PeriodOfChange; i++)
                                {
                                    PriorAmount += TBudgetMaintainWebConnector.GetBudget(GLMSequenceThisYear,
                                        -1,
                                        i,
                                        NumAccPeriods,
                                        false,
                                        MFinanceConstants.CURRENCY_BASE);
                                }
                            }
                            else
                            {
                                PriorAmount = TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceLastYear,
                                    GLMSequenceThisYear,
                                    PeriodOfChange,
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    (CurrentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE) -
                                              TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceLastYear,
                                    GLMSequenceThisYear,
                                    CurrentPeriod,
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    (CurrentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);
                            }

                            if (AForecastType == MFinanceConstants.FORECAST_TYPE_BUDGET)
                            {
                                for (int i = (PeriodOfChange + 1); i <= NumAccPeriods; i++)
                                {
                                    AfterAmount += TBudgetMaintainWebConnector.GetBudget(GLMSequenceThisYear,
                                        -1,
                                        i,
                                        NumAccPeriods,
                                        false,
                                        MFinanceConstants.CURRENCY_BASE);
                                }
                            }
                            else
                            {
                                AfterAmount = TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceLastYear,
                                    GLMSequenceThisYear,
                                    NumAccPeriods,
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    (CurrentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE) -
                                              TBudgetMaintainWebConnector.GetActual(ALedgerNumber,
                                    GLMSequenceLastYear,
                                    GLMSequenceThisYear,
                                    (PeriodOfChange + 1),
                                    NumAccPeriods,
                                    CurrentFinancialYear,
                                    (CurrentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);
                            }

                            /* Dividing after sum by prior sum gives rate of inflation. */
                            PriorAmount = PriorAmount / PeriodOfChange;
                            AfterAmount = AfterAmount / (NumAccPeriods - PeriodOfChange);

                            for (int i = 1; i <= PeriodOfChange; i++)
                            {
                                SetBudgetPeriod(ABudgetSeq, i, Math.Round(PriorAmount, 0));
                            }

                            for (int i = (PeriodOfChange + 1); i <= NumAccPeriods; i++)
                            {
                                SetBudgetPeriod(ABudgetSeq, i, Math.Round(AfterAmount, 0));
                            }
                        }

                        break;

                    default:
                        ValidBudgetType = false;
                        break;
                }

                if (!ValidBudgetType)
                {
                    throw new InvalidOperationException(String.Format("Invalid budget type of: {0} for Budget Seq.: {1}",
                            BudgetType,
                            BudgetRow.BudgetSequence));
                }

                retVal = true;
            }
            catch (Exception)
            {
                throw;
            }

            return retVal;
        }

        /// <summary>
        /// Description: set the budget of a period.
        /// </summary>
        /// <param name="ABudgetSequence"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="ABudgetAmount"></param>
        /// <returns></returns>
        private static decimal SetBudgetPeriod(int ABudgetSequence, int APeriodNumber, decimal ABudgetAmount)
        {
            decimal retVal = 0;

            ABudgetPeriodTable BudgetPeriodTable = FMainDS.ABudgetPeriod;
            ABudgetPeriodRow BudgetPeriodRow = (ABudgetPeriodRow)BudgetPeriodTable.Rows.Find(new object[] { ABudgetSequence, APeriodNumber });

            if (BudgetPeriodRow != null)
            {
                BudgetPeriodRow.BeginEdit();
                BudgetPeriodRow.BudgetBase = ABudgetAmount;
                BudgetPeriodRow.EndEdit();

                retVal = ABudgetAmount;
            }

            return retVal;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ABudgetSequence"></param>
        /// <param name="APeriodNumber"></param>
        /// <returns></returns>
        private static decimal GetBudgetPeriodAmount(int ABudgetSequence, int APeriodNumber)
        {
            decimal retVal = 0;

            decimal BudgetAmount = 0;

            ABudgetPeriodTable BudgetPeriodTable = FMainDS.ABudgetPeriod;
            ABudgetPeriodRow BudgetPeriodRow = (ABudgetPeriodRow)BudgetPeriodTable.Rows.Find(new object[] { ABudgetSequence, APeriodNumber });

            if (BudgetPeriodRow != null)
            {
                BudgetAmount = BudgetPeriodRow.BudgetBase;

                retVal = BudgetAmount;
            }

            return retVal;
        }
    }
}