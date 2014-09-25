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
    public class TBudgetConsolidateWebConnector
    {
        /// <summary>
        /// Main Budget tables dataset
        /// </summary>
        private static BudgetTDS FBudgetTDS = null;
        private static GLPostingTDS GLPostingDS = null;

        /// <summary>
        /// load budgets
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool LoadBudgetForConsolidate(Int32 ALedgerNumber)
        {
            FBudgetTDS = new BudgetTDS();

            TDBTransaction Transaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    ALedgerAccess.LoadByPrimaryKey(FBudgetTDS, ALedgerNumber, Transaction);

                    string sqlLoadBudgetForThisAndNextYear =
                        string.Format("SELECT * FROM PUB_{0} WHERE {1}=? AND ({2} = ? OR {2} = ?)",
                            ABudgetTable.GetTableDBName(),
                            ABudgetTable.GetLedgerNumberDBName(),
                            ABudgetTable.GetYearDBName());

                    List <OdbcParameter>parameters = new List <OdbcParameter>();
                    OdbcParameter param = new OdbcParameter("ledgernumber", OdbcType.Int);
                    param.Value = ALedgerNumber;
                    parameters.Add(param);
                    param = new OdbcParameter("thisyear", OdbcType.Int);
                    param.Value = FBudgetTDS.ALedger[0].CurrentFinancialYear;
                    parameters.Add(param);
                    param = new OdbcParameter("nextyear", OdbcType.Int);
                    param.Value = FBudgetTDS.ALedger[0].CurrentFinancialYear + 1;
                    parameters.Add(param);

                    DBAccess.GDBAccessObj.Select(FBudgetTDS, sqlLoadBudgetForThisAndNextYear, FBudgetTDS.ABudget.TableName, Transaction,
                        parameters.ToArray());

                    string sqlLoadBudgetPeriodForThisAndNextYear =
                        string.Format("SELECT {0}.* FROM PUB_{0}, PUB_{1} WHERE {0}.a_budget_sequence_i = {1}.a_budget_sequence_i AND " +
                            "{2}=? AND ({3} = ? OR {3} = ?)",
                            ABudgetPeriodTable.GetTableDBName(),
                            ABudgetTable.GetTableDBName(),
                            ABudgetTable.GetLedgerNumberDBName(),
                            ABudgetTable.GetYearDBName());

                    DBAccess.GDBAccessObj.Select(FBudgetTDS,
                        sqlLoadBudgetPeriodForThisAndNextYear,
                        FBudgetTDS.ABudgetPeriod.TableName,
                        Transaction,
                        parameters.ToArray());

                    // Accept row changes here so that the Client gets 'unmodified' rows
                    FBudgetTDS.AcceptChanges();

                    GLPostingDS = new GLPostingTDS();
                    AAccountAccess.LoadViaALedger(GLPostingDS, ALedgerNumber, Transaction);
                    AAccountHierarchyDetailAccess.LoadViaALedger(GLPostingDS, ALedgerNumber, Transaction);
                    ACostCentreAccess.LoadViaALedger(GLPostingDS, ALedgerNumber, Transaction);
                    ALedgerAccess.LoadByPrimaryKey(GLPostingDS, ALedgerNumber, Transaction);

                    // get the glm sequences for this year and next year
                    for (int i = 0; i <= 1; i++)
                    {
                        int Year = GLPostingDS.ALedger[0].CurrentFinancialYear + i;

                        AGeneralLedgerMasterRow TemplateRow = (AGeneralLedgerMasterRow)GLPostingDS.AGeneralLedgerMaster.NewRowTyped(false);

                        TemplateRow.LedgerNumber = ALedgerNumber;
                        TemplateRow.Year = Year;

                        GLPostingDS.AGeneralLedgerMaster.Merge(AGeneralLedgerMasterAccess.LoadUsingTemplate(TemplateRow, Transaction));
                    }

                    string sqlLoadGlmperiodForThisAndNextYear =
                        string.Format("SELECT {0}.* FROM PUB_{0}, PUB_{1} WHERE {0}.a_glm_sequence_i = {1}.a_glm_sequence_i AND " +
                            "{2}=? AND ({3} = ? OR {3} = ?)",
                            AGeneralLedgerMasterPeriodTable.GetTableDBName(),
                            AGeneralLedgerMasterTable.GetTableDBName(),
                            AGeneralLedgerMasterTable.GetLedgerNumberDBName(),
                            AGeneralLedgerMasterTable.GetYearDBName());

                    DBAccess.GDBAccessObj.Select(GLPostingDS,
                        sqlLoadGlmperiodForThisAndNextYear,
                        GLPostingDS.AGeneralLedgerMasterPeriod.TableName,
                        Transaction,
                        parameters.ToArray());
                });

            GLPostingDS.AcceptChanges();

            return true;
        }

        /// <summary>
        /// Consolidate Budgets.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AConsolidateAll"></param>
        [RequireModulePermission("FINANCE-3")]
        public static void ConsolidateBudgets(Int32 ALedgerNumber, bool AConsolidateAll)
        {
            bool NewTransaction = false;
            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                out NewTransaction);

            ALedgerRow LedgerRow = FBudgetTDS.ALedger[0];

            try
            {
                // first clear the old budget from GLMPeriods
                if (AConsolidateAll)
                {
                    foreach (ABudgetRow BudgetRow in FBudgetTDS.ABudget.Rows)
                    {
                        BudgetRow.BudgetStatus = false;
                    }

                    foreach (AGeneralLedgerMasterRow GeneralLedgerMasterRow in GLPostingDS.AGeneralLedgerMaster.Rows)
                    {
                        for (int Period = 1; Period <= LedgerRow.NumberOfAccountingPeriods; Period++)
                        {
                            ClearAllBudgetValues(GeneralLedgerMasterRow.GlmSequence, Period);
                        }
                    }
                }
                else
                {
                    foreach (ABudgetRow BudgetRow in FBudgetTDS.ABudget.Rows)
                    {
                        if (!BudgetRow.BudgetStatus)
                        {
                            UnPostBudget(BudgetRow, ALedgerNumber);
                        }
                    }
                }

                foreach (ABudgetRow BudgetRow in FBudgetTDS.ABudget.Rows)
                {
                    if (!BudgetRow.BudgetStatus || AConsolidateAll)
                    {
                        List <ABudgetPeriodRow>budgetPeriods = new List <ABudgetPeriodRow>();

                        FBudgetTDS.ABudgetPeriod.DefaultView.RowFilter = ABudgetPeriodTable.GetBudgetSequenceDBName() + " = " +
                                                                         BudgetRow.BudgetSequence.ToString();

                        foreach (DataRowView rv in FBudgetTDS.ABudgetPeriod.DefaultView)
                        {
                            budgetPeriods.Add((ABudgetPeriodRow)rv.Row);
                        }

                        PostBudget(ALedgerNumber, BudgetRow, budgetPeriods);
                    }
                }

                FinishConsolidateBudget(SubmitChangesTransaction);


                GLPostingDS.ThrowAwayAfterSubmitChanges = true;
                GLPostingTDSAccess.SubmitChanges(GLPostingDS);
                GLPostingDS.Clear();

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the consolidation of Budgets:" + Environment.NewLine + Exc.ToString());

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw;
            }
        }

        /// <summary>
        /// Complete the Budget consolidation process
        /// </summary>
        /// <param name="ATransaction"></param>
        private static void FinishConsolidateBudget(
            TDBTransaction ATransaction)
        {
            /*Consolidate_Budget*/
            foreach (ABudgetRow BudgetRow in FBudgetTDS.ABudget.Rows)
            {
                BudgetRow.BudgetStatus = true;
            }

            ABudgetAccess.SubmitChanges(FBudgetTDS.ABudget, ATransaction);
        }

        /// <summary>
        /// Return the budget amount from the temp table APeriodDataTable.
        ///   if the record is not already in the temp table, it is fetched
        /// </summary>
        /// <param name="APeriodDataTable"></param>
        /// <param name="AGLMSequence"></param>
        /// <param name="APeriodNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static decimal GetBudgetValue(ref DataTable APeriodDataTable, int AGLMSequence, int APeriodNumber)
        {
            decimal GetBudgetValue = 0;

            DataRow TempRow = (DataRow)APeriodDataTable.Rows.Find(new object[] { AGLMSequence, APeriodNumber });

            if (TempRow == null)
            {
                AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = null;
                AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow = null;

                TDBTransaction transaction = null;
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref transaction,
                delegate
                {
                    GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSequence, APeriodNumber, transaction);
                });

                if (GeneralLedgerMasterPeriodTable.Count > 0)
                {
                    GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];

                    DataRow DR = (DataRow)APeriodDataTable.NewRow();
                    DR["GLMSequence"] = AGLMSequence;
                    DR["PeriodNumber"] = APeriodNumber;
                    DR["BudgetBase"] = GeneralLedgerMasterPeriodRow.BudgetBase;

                    APeriodDataTable.Rows.Add(DR);
                }
            }
            else
            {
                //Set to budget base
                GetBudgetValue = Convert.ToDecimal(TempRow["BudgetBase"]);
            }

            return GetBudgetValue;
        }

        /// <summary>
        /// Unpost a budget
        /// </summary>
        /// <param name="ABudgetRow"></param>
        /// <param name="ALedgerNumber"></param>
        /// <returns>true if it seemed to go OK</returns>
        private static bool UnPostBudget(ABudgetRow ABudgetRow, int ALedgerNumber)
        {
            /* post the negative budget, which will result in an empty a_glm_period.budget */

            // get the current budget value for each GLM Period, and unpost that budget

            GLPostingDS.AGeneralLedgerMaster.DefaultView.Sort = String.Format("{0},{1},{2},{3}",
                AGeneralLedgerMasterTable.GetLedgerNumberDBName(),
                AGeneralLedgerMasterTable.GetYearDBName(),
                AGeneralLedgerMasterTable.GetAccountCodeDBName(),
                AGeneralLedgerMasterTable.GetCostCentreCodeDBName());

            int glmIndex = GLPostingDS.AGeneralLedgerMaster.DefaultView.Find(
                new object[] { ALedgerNumber, ABudgetRow.Year, ABudgetRow.AccountCode, ABudgetRow.CostCentreCode });

            if (glmIndex != -1)
            {
                AGeneralLedgerMasterRow glmRow = (AGeneralLedgerMasterRow)GLPostingDS.AGeneralLedgerMaster.DefaultView[glmIndex].Row;

                List <ABudgetPeriodRow>budgetPeriods = new List <ABudgetPeriodRow>();

                for (int Period = 1; Period <= GLPostingDS.ALedger[0].NumberOfAccountingPeriods; Period++)
                {
                    AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow =
                        (AGeneralLedgerMasterPeriodRow)GLPostingDS.AGeneralLedgerMasterPeriod.Rows.Find(
                            new object[] { glmRow.GlmSequence, Period });

                    ABudgetPeriodRow budgetPeriodRow = FBudgetTDS.ABudgetPeriod.NewRowTyped(true);
                    budgetPeriodRow.PeriodNumber = Period;
                    budgetPeriodRow.BudgetSequence = ABudgetRow.BudgetSequence;

                    // use negative amount for unposting
                    budgetPeriodRow.BudgetBase = -1 * GeneralLedgerMasterPeriodRow.BudgetBase;

                    // do not add to the budgetperiod table, but to our local list
                    budgetPeriods.Add(budgetPeriodRow);
                }

                PostBudget(ALedgerNumber, ABudgetRow, budgetPeriods);
            }

            ABudgetRow.BudgetStatus = false;                 //i.e. unposted

            return true;
        }

        /// <summary>
        /// Post a budget
        /// </summary>
        private static void PostBudget(int ALedgerNumber, ABudgetRow ABudgetRow, List <ABudgetPeriodRow>ABudgetPeriodRows)
        {
            //gb5300.p
            string AccountCode = ABudgetRow.AccountCode;

            string CostCentreList = ABudgetRow.CostCentreCode;              /* posting CC and parents */

            //Populate list of affected Cost Centres
            CostCentreParentsList(ALedgerNumber, ref CostCentreList);

            //Locate the row for the current account
            AAccountRow AccountRow = (AAccountRow)GLPostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, AccountCode });

            GLPostingDS.AGeneralLedgerMaster.DefaultView.Sort = String.Format("{0},{1},{2},{3}",
                AGeneralLedgerMasterTable.GetLedgerNumberDBName(),
                AGeneralLedgerMasterTable.GetYearDBName(),
                AGeneralLedgerMasterTable.GetAccountCodeDBName(),
                AGeneralLedgerMasterTable.GetCostCentreCodeDBName());

            /* calculate values for budgets and store them in a temp table; uses lb_budget */
            ProcessAccountParent(
                ALedgerNumber,
                AccountCode,
                AccountRow.DebitCreditIndicator,
                CostCentreList,
                ABudgetRow,
                ABudgetPeriodRows);
        }

        /// <summary>
        /// Process the account code parent codes
        /// </summary>
        private static void ProcessAccountParent(
            int ALedgerNumber,
            string CurrAccountCode,
            bool ADebitCreditIndicator,
            string ACostCentreList,
            ABudgetRow ABudgetRow,
            List <ABudgetPeriodRow>ABudgetPeriods)
        {
            AAccountRow AccountRow = (AAccountRow)GLPostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, CurrAccountCode });

            AAccountHierarchyDetailRow AccountHierarchyDetailRow = (AAccountHierarchyDetailRow)GLPostingDS.AAccountHierarchyDetail.Rows.Find(
                new object[] { ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, CurrAccountCode });

            if (AccountHierarchyDetailRow != null)
            {
                string AccountCodeToReportTo = AccountHierarchyDetailRow.AccountCodeToReportTo;

                if ((AccountCodeToReportTo != null) && (AccountCodeToReportTo != string.Empty))
                {
                    /* Recursively call this procedure. */
                    ProcessAccountParent(
                        ALedgerNumber,
                        AccountCodeToReportTo,
                        ADebitCreditIndicator,
                        ACostCentreList,
                        ABudgetRow,
                        ABudgetPeriods);
                }
            }

            int DebitCreditMultiply = 1;             /* needed if the debit credit indicator is not the same */

            /* If the account has the same db/cr indicator as the original
             *         account for which the budget was created, add the budget amount.
             *         Otherwise, subtract. */
            if (AccountRow.DebitCreditIndicator != ADebitCreditIndicator)
            {
                DebitCreditMultiply = -1;
            }

            string[] CostCentres = ACostCentreList.Split(':');
            string AccCode = AccountRow.AccountCode;

            /* For each associated Cost Centre, update the General Ledger Master. */
            foreach (string CostCentreCode in CostCentres)
            {
                int glmRowIndex = GLPostingDS.AGeneralLedgerMaster.DefaultView.Find(new object[] { ALedgerNumber, ABudgetRow.Year, AccCode,
                                                                                                   CostCentreCode });

                if (glmRowIndex == -1)
                {
                    TGLPosting.CreateGLMYear(ref GLPostingDS, ALedgerNumber, ABudgetRow.Year, AccCode, CostCentreCode);
                    glmRowIndex = GLPostingDS.AGeneralLedgerMaster.DefaultView.Find(new object[] { ALedgerNumber, ABudgetRow.Year, AccCode,
                                                                                                   CostCentreCode });
                }

                int GLMSequence = ((AGeneralLedgerMasterRow)GLPostingDS.AGeneralLedgerMaster.DefaultView[glmRowIndex].Row).GlmSequence;

                /* Update totals for the General Ledger Master period record. */
                foreach (ABudgetPeriodRow BPR in ABudgetPeriods)
                {
                    AddBudgetValue(GLMSequence, BPR.PeriodNumber, DebitCreditMultiply * BPR.BudgetBase);
                }
            }
        }

        /// <summary>
        /// Return the list of parent cost centre codes
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACurrentCostCentreList"></param>
        private static void CostCentreParentsList(int ALedgerNumber, ref string ACurrentCostCentreList)
        {
            string ParentCostCentre;
            string CostCentreList = ACurrentCostCentreList;

            ACostCentreRow CostCentreRow = (ACostCentreRow)GLPostingDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, ACurrentCostCentreList });

            ParentCostCentre = CostCentreRow.CostCentreToReportTo;

            while (ParentCostCentre != string.Empty)
            {
                ACostCentreRow CCRow = (ACostCentreRow)GLPostingDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, ParentCostCentre });

                CostCentreList += ":" + CCRow.CostCentreCode;
                ParentCostCentre = CCRow.CostCentreToReportTo;
            }
        }

        /// <summary>
        /// Write a budget value to the temporary table
        /// </summary>
        /// <param name="AGLMSequence"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="APeriodAmount"></param>
        private static void AddBudgetValue(int AGLMSequence, int APeriodNumber, decimal APeriodAmount)
        {
            AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow =
                (AGeneralLedgerMasterPeriodRow)GLPostingDS.AGeneralLedgerMasterPeriod.Rows.Find(
                    new object[] { AGLMSequence, APeriodNumber });

            if (GeneralLedgerMasterPeriodRow != null)
            {
                GeneralLedgerMasterPeriodRow.BudgetBase += APeriodAmount;
            }
            else
            {
                throw new Exception("AddBudgetValue: cannot find glmp record for " + AGLMSequence.ToString() + " " + APeriodNumber.ToString());
            }
        }

        /// <summary>
        /// Reset the budget amount in the temp table wtPeriodData.
        ///   if the record is not already in the temp table, it is created empty
        /// </summary>
        /// <param name="AGLMSequence"></param>
        /// <param name="APeriodNumber"></param>
        private static void ClearAllBudgetValues(int AGLMSequence, int APeriodNumber)
        {
            AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow =
                (AGeneralLedgerMasterPeriodRow)GLPostingDS.AGeneralLedgerMasterPeriod.Rows.Find(new object[] { AGLMSequence, APeriodNumber });

            if (GeneralLedgerMasterPeriodRow != null)
            {
                GeneralLedgerMasterPeriodRow.BudgetBase = 0;
            }
        }
    }
}