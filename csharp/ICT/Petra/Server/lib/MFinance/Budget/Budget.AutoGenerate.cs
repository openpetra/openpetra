//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
//
// Copyright 2004-2024 by OM International
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
using Ict.Common;
using Ict.Common.Exceptions;
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
        /// load budget tables and return the budget table
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static BudgetTDS LoadBudgetForAutoGenerate(Int32 ALedgerNumber)
        {
            BudgetTDS FMainDS = new BudgetTDS();

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("LoadBudgetForAutoGenerate");

            db.ReadTransaction(
                ref Transaction,
                delegate
                {
                    ALedgerAccess.LoadByPrimaryKey(FMainDS, ALedgerNumber, Transaction);

                    #region Validate Data

                    if ((FMainDS.ALedger == null) || (FMainDS.ALedger.Count == 0))
                    {
                        throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                    "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                Utilities.GetMethodName(true),
                                ALedgerNumber));
                    }

                    #endregion Validate Data

                    int CurrentFinancialYear = FMainDS.ALedger[0].CurrentFinancialYear;

                    //Load all by Ledger/Year but none may exist
                    ABudgetTable BudgetTable = new ABudgetTable();
                    ABudgetRow TemplateRow = (ABudgetRow)BudgetTable.NewRow();

                    TemplateRow.LedgerNumber = ALedgerNumber;
                    TemplateRow.Year = CurrentFinancialYear;

                    StringCollection Operators = StringHelper.InitStrArr(new string[] { "=", "=" });
                    StringCollection OrderList = new StringCollection();

                    OrderList.Add("ORDER BY");
                    OrderList.Add(ABudgetTable.GetCostCentreCodeDBName() + " ASC");
                    OrderList.Add(ABudgetTable.GetAccountCodeDBName() + " ASC");

                    ABudgetAccess.LoadUsingTemplate(FMainDS, TemplateRow, Operators, null, Transaction, OrderList, 0, 0);
                    ABudgetPeriodAccess.LoadViaABudgetTemplate(FMainDS, TemplateRow, Operators, null, Transaction, OrderList, 0, 0);

                    ABudgetRevisionAccess.LoadByPrimaryKey(FMainDS, ALedgerNumber, CurrentFinancialYear, 0, Transaction);

                    ABudgetTypeAccess.LoadAll(FMainDS, Transaction);
                });

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
            BudgetTDS FMainDS = LoadBudgetForAutoGenerate(ALedgerNumber);

            bool retVal = false;

            decimal budgetSum;
            decimal priorAmount = 0;
            decimal afterAmount = 0;
            int periodOfChange = 0;
            int gLMSequenceThisYear = 0;
            int gLMSequenceLastYear = 0;

            ALedgerTable ledgerTable = FMainDS.ALedger;
            ALedgerRow ledgerRow = (ALedgerRow)ledgerTable.Rows[0];
            int currentFinancialYear = ledgerRow.CurrentFinancialYear;
            int currentPeriod = ledgerRow.CurrentPeriod;
            int numAccPeriods = ledgerRow.NumberOfAccountingPeriods;

            ABudgetTable budgetTable = FMainDS.ABudget;
            ABudgetRow budgetRow = (ABudgetRow)budgetTable.Rows.Find(new object[] { ABudgetSeq });

            #region  Validate Method Parameters

            if ((budgetRow == null) || (budgetRow.LedgerNumber != ledgerRow.LedgerNumber) || (budgetRow.Year != currentFinancialYear))
            {
                TLogging.Log("GenBudgetForNextYear: either could not find budget sequence " + ABudgetSeq.ToString() +
                    "or the sequence does not apply to ledger " + ledgerRow.LedgerNumber.ToString() +
                    "or to ledger current year " + currentFinancialYear.ToString());
                return retVal;
            }

            if (ALedgerNumber != ledgerRow.LedgerNumber)
            {
                TLogging.Log("GenBudgetForNextYear: method parameter ALedgerNumber is " + ALedgerNumber.ToString() +
                    " but the data set expected the ledger to be " + ledgerRow.LedgerNumber.ToString());
            }

            #endregion

            string accountCode = budgetRow.AccountCode;
            string costCentreCode = budgetRow.CostCentreCode;

            gLMSequenceThisYear = TCommonBudgetMaintain.GetGLMSequenceForBudget(ALedgerNumber,
                accountCode,
                costCentreCode,
                currentFinancialYear);

            gLMSequenceLastYear = TCommonBudgetMaintain.GetGLMSequenceForBudget(ALedgerNumber,
                accountCode,
                costCentreCode,
                (currentFinancialYear - 1));
            try
            {
                //Update the budget status
                budgetRow.BeginEdit();
                budgetRow.BudgetStatus = false;
                budgetRow.EndEdit();

                string budgetType = budgetRow.BudgetTypeCode;

                decimal budgetAmount = 0;
                decimal actualAmount = 0;
                bool validBudgetType = true;

                switch (budgetType)
                {
                    case MFinanceConstants.BUDGET_ADHOC:
                    case MFinanceConstants.BUDGET_INFLATE_BASE:

                        for (int i = 1; i < currentPeriod; i++)
                        {
                            //Set budget period
                            actualAmount = TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                gLMSequenceLastYear,
                                gLMSequenceThisYear,
                                i,
                                numAccPeriods,
                                currentFinancialYear,
                                (currentFinancialYear - 1),
                                false,
                                MFinanceConstants.CURRENCY_BASE);
                            SetBudgetPeriodBaseAmount(FMainDS, ABudgetSeq, i, actualAmount);
                        }

                        for (int j = currentPeriod; j <= MFinanceConstants.MAX_PERIODS; j++)
                        {
                            if (AForecastType == MFinanceConstants.FORECAST_TYPE_BUDGET)
                            {
                                budgetAmount =
                                    Math.Round(TCommonBudgetMaintain.GetBudget(gLMSequenceThisYear, -1, j, numAccPeriods,
                                            false,
                                            MFinanceConstants.CURRENCY_BASE));
                                SetBudgetPeriodBaseAmount(FMainDS, ABudgetSeq, j, budgetAmount);
                            }
                            else
                            {
                                actualAmount = TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceLastYear,
                                    gLMSequenceThisYear,
                                    j,
                                    numAccPeriods,
                                    currentFinancialYear,
                                    (currentFinancialYear - 1),
                                    false,
                                    MFinanceConstants.CURRENCY_BASE);
                                SetBudgetPeriodBaseAmount(FMainDS, ABudgetSeq, j, actualAmount);
                            }
                        }

                        break;

                    case MFinanceConstants.BUDGET_SAME:                      //because this case has no code it will fall through to the next case until it finds code.
                    case MFinanceConstants.BUDGET_SPLIT:

                        if ((currentPeriod - 1) != 0)
                        {
                            budgetSum =
                                TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceThisYear,
                                    -1,
                                    (currentPeriod - 1),
                                    numAccPeriods,
                                    currentFinancialYear,
                                    currentFinancialYear,
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);
                        }
                        else
                        {
                            budgetSum = 0;
                        }

                        if (AForecastType == MFinanceConstants.FORECAST_TYPE_BUDGET)
                        {
                            for (int i = currentPeriod; i <= numAccPeriods; i++)
                            {
                                budgetSum += TCommonBudgetMaintain.GetBudget(gLMSequenceThisYear,
                                    -1,
                                    i,
                                    numAccPeriods,
                                    false,
                                    MFinanceConstants.CURRENCY_BASE);
                            }
                        }
                        else
                        {
                            if (currentPeriod > 1)
                            {
                                budgetSum += TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceLastYear,
                                    gLMSequenceThisYear,
                                    numAccPeriods,
                                    numAccPeriods,
                                    currentFinancialYear,
                                    (currentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE) -
                                             TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceLastYear,
                                    gLMSequenceThisYear,
                                    (currentPeriod - 1),
                                    numAccPeriods,
                                    currentFinancialYear,
                                    (currentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);
                            }
                            else
                            {
                                budgetSum += TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceLastYear,
                                    gLMSequenceThisYear,
                                    numAccPeriods,
                                    numAccPeriods,
                                    currentFinancialYear,
                                    (currentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);
                            }
                        }

                        budgetSum = budgetSum / numAccPeriods;

                        for (int i = 1; i <= numAccPeriods; i++)
                        {
                            SetBudgetPeriodBaseAmount(FMainDS, ABudgetSeq, i, Math.Round(budgetSum));
                        }

                        break;

                    case MFinanceConstants.BUDGET_INFLATE_N:

                        for (int i = 1; i <= numAccPeriods; i++)
                        {
                            if (GetBudgetPeriodAmount(FMainDS, ABudgetSeq, i) != GetBudgetPeriodAmount(FMainDS, ABudgetSeq, 1))
                            {
                                periodOfChange = i - 1;
                                break;
                            }
                        }

                        /* Calculate average prior to change and after change. */
                        if (periodOfChange < (currentPeriod - 1))
                        {
                            priorAmount = TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                gLMSequenceThisYear,
                                -1,
                                periodOfChange,
                                numAccPeriods,
                                currentFinancialYear,
                                currentFinancialYear,
                                true,
                                MFinanceConstants.CURRENCY_BASE);

                            afterAmount =
                                TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceThisYear,
                                    -1,
                                    (currentPeriod - 1),
                                    numAccPeriods,
                                    currentFinancialYear,
                                    currentFinancialYear,
                                    true,
                                    MFinanceConstants.CURRENCY_BASE) -
                                TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceThisYear,
                                    -1,
                                    periodOfChange + 1,
                                    numAccPeriods,
                                    currentFinancialYear,
                                    currentFinancialYear,
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);

                            if (AForecastType == MFinanceConstants.FORECAST_TYPE_BUDGET)
                            {
                                for (int i = currentPeriod; i <= numAccPeriods; i++)
                                {
                                    afterAmount += TCommonBudgetMaintain.GetBudget(gLMSequenceThisYear,
                                        -1,
                                        i,
                                        numAccPeriods,
                                        false,
                                        MFinanceConstants.CURRENCY_BASE);
                                }
                            }
                            else
                            {
                                afterAmount += TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceLastYear,
                                    gLMSequenceThisYear,
                                    numAccPeriods,
                                    numAccPeriods,
                                    currentFinancialYear,
                                    (currentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE) -
                                               TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceLastYear,
                                    gLMSequenceThisYear,
                                    currentPeriod,
                                    numAccPeriods,
                                    currentFinancialYear,
                                    (currentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);
                            }
                        }
                        else                          /* Period of change HAS NOT taken place. */
                        {
                            if ((currentPeriod - 1) != 0)
                            {
                                priorAmount =
                                    TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                        gLMSequenceThisYear,
                                        -1,
                                        (currentPeriod - 1),
                                        numAccPeriods,
                                        currentFinancialYear,
                                        currentFinancialYear,
                                        true,
                                        MFinanceConstants.CURRENCY_BASE);
                            }
                            else
                            {
                                priorAmount = 0;
                            }

                            if (AForecastType == MFinanceConstants.FORECAST_TYPE_BUDGET)
                            {
                                for (int i = currentPeriod; i <= periodOfChange; i++)
                                {
                                    priorAmount += TCommonBudgetMaintain.GetBudget(gLMSequenceThisYear,
                                        -1,
                                        i,
                                        numAccPeriods,
                                        false,
                                        MFinanceConstants.CURRENCY_BASE);
                                }
                            }
                            else
                            {
                                priorAmount = TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceLastYear,
                                    gLMSequenceThisYear,
                                    periodOfChange,
                                    numAccPeriods,
                                    currentFinancialYear,
                                    (currentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE) -
                                              TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceLastYear,
                                    gLMSequenceThisYear,
                                    currentPeriod,
                                    numAccPeriods,
                                    currentFinancialYear,
                                    (currentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);
                            }

                            if (AForecastType == MFinanceConstants.FORECAST_TYPE_BUDGET)
                            {
                                for (int i = (periodOfChange + 1); i <= numAccPeriods; i++)
                                {
                                    afterAmount += TCommonBudgetMaintain.GetBudget(gLMSequenceThisYear,
                                        -1,
                                        i,
                                        numAccPeriods,
                                        false,
                                        MFinanceConstants.CURRENCY_BASE);
                                }
                            }
                            else
                            {
                                afterAmount = TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceLastYear,
                                    gLMSequenceThisYear,
                                    numAccPeriods,
                                    numAccPeriods,
                                    currentFinancialYear,
                                    (currentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE) -
                                              TCommonBudgetMaintain.GetActual(ALedgerNumber,
                                    gLMSequenceLastYear,
                                    gLMSequenceThisYear,
                                    (periodOfChange + 1),
                                    numAccPeriods,
                                    currentFinancialYear,
                                    (currentFinancialYear - 1),
                                    true,
                                    MFinanceConstants.CURRENCY_BASE);
                            }

                            /* Dividing after sum by prior sum gives rate of inflation. */
                            priorAmount = priorAmount / periodOfChange;
                            afterAmount = afterAmount / (numAccPeriods - periodOfChange);

                            for (int i = 1; i <= periodOfChange; i++)
                            {
                                SetBudgetPeriodBaseAmount(FMainDS, ABudgetSeq, i, Math.Round(priorAmount, 0));
                            }

                            for (int i = (periodOfChange + 1); i <= numAccPeriods; i++)
                            {
                                SetBudgetPeriodBaseAmount(FMainDS, ABudgetSeq, i, Math.Round(afterAmount, 0));
                            }
                        }

                        break;

                    default:
                        validBudgetType = false;
                        break;
                }

                if (!validBudgetType)
                {
                    throw new InvalidOperationException(String.Format("Invalid budget type of: {0} for Budget Seq.: {1}",
                            budgetType,
                            budgetRow.BudgetSequence));
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
        /// set the budget of a period.
        /// </summary>
        /// <param name="FMainDS"></param>
        /// <param name="ABudgetSequence"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="ABudgetAmount"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static decimal SetBudgetPeriodBaseAmount(BudgetTDS FMainDS, int ABudgetSequence, int APeriodNumber, decimal ABudgetAmount)
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
        /// Get the amount for a certain period and budget
        /// </summary>
        /// <param name="FMainDS"></param>
        /// <param name="ABudgetSequence"></param>
        /// <param name="APeriodNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static decimal GetBudgetPeriodAmount(BudgetTDS FMainDS, int ABudgetSequence, int APeriodNumber)
        {
            decimal retVal = 0m;

            ABudgetPeriodTable BudgetPeriodTable = FMainDS.ABudgetPeriod;
            ABudgetPeriodRow BudgetPeriodRow = (ABudgetPeriodRow)BudgetPeriodTable.Rows.Find(new object[] { ABudgetSequence, APeriodNumber });

            if (BudgetPeriodRow != null)
            {
                retVal = (decimal)BudgetPeriodRow.BudgetBase;
            }

            return retVal;
        }
    }
}
