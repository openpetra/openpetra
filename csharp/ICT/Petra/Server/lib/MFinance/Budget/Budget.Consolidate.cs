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
        public static BudgetTDS FBudgetTDS = new BudgetTDS();
        private static GLBatchTDS GLBatchDS = null;

        /// <summary>
        /// load budgets
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool LoadBudgetForConsolidate(Int32 ALedgerNumber)
        {
            ALedgerAccess.LoadByPrimaryKey(FBudgetTDS, ALedgerNumber, null);

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

            DBAccess.GDBAccessObj.Select(FBudgetTDS, sqlLoadBudgetForThisAndNextYear, FBudgetTDS.ABudget.TableName, null, parameters.ToArray());

            ABudgetRevisionAccess.LoadViaALedger(FBudgetTDS, ALedgerNumber, null);
            //TODO: need to filter on ABudgetPeriod using LoadViaBudget or LoadViaUniqueKey
            ABudgetPeriodAccess.LoadAll(FBudgetTDS, null);

            // Accept row changes here so that the Client gets 'unmodified' rows
            FBudgetTDS.AcceptChanges();

            GLBatchDS = new GLBatchTDS();
            AAccountAccess.LoadViaALedger(GLBatchDS, ALedgerNumber, null);
            AAccountHierarchyDetailAccess.LoadViaALedger(GLBatchDS, ALedgerNumber, null);
            ACostCentreAccess.LoadViaALedger(GLBatchDS, ALedgerNumber, null);
            ALedgerAccess.LoadByPrimaryKey(GLBatchDS, ALedgerNumber, null);

            // get the glm sequences for this year and next year
            for (int i = 0; i <= 1; i++)
            {
                int Year = GLBatchDS.ALedger[0].CurrentFinancialYear + i;

                AGeneralLedgerMasterRow TemplateRow = (AGeneralLedgerMasterRow)GLBatchDS.AGeneralLedgerMaster.NewRowTyped(false);

                TemplateRow.LedgerNumber = ALedgerNumber;
                TemplateRow.Year = Year;

                GLBatchDS.AGeneralLedgerMaster.Merge(AGeneralLedgerMasterAccess.LoadUsingTemplate(TemplateRow, null));
            }

            GLBatchDS.AcceptChanges();

            return true;
        }

        /// <summary>
        /// Consolidate Budgets.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AConsolidateAll"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>false (always!)</returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool ConsolidateBudgets(Int32 ALedgerNumber, bool AConsolidateAll,
            out TVerificationResultCollection AVerificationResult)
        {
            bool retVal = true;

            bool IsMyOwnTransaction; // If I create a transaction here, then I need to rollback when I'm done.
            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                out IsMyOwnTransaction);

            AVerificationResult = null;

            ALedgerRow LedgerRow = FBudgetTDS.ALedger[0];

            if (AConsolidateAll)
            {
                foreach (ABudgetRow BudgetRow in FBudgetTDS.ABudget.Rows)
                {
                    BudgetRow.BudgetStatus = false;
                }

                foreach (AGeneralLedgerMasterRow GeneralLedgerMasterRow in GLBatchDS.AGeneralLedgerMaster.Rows)
                {
                    for (int Period = 1; Period <= LedgerRow.NumberOfAccountingPeriods; Period++)
                    {
                        ClearAllBudgetValues(GeneralLedgerMasterRow.GlmSequence, Period);
                    }
                }
            }

            foreach (ABudgetRow BudgetRow in FBudgetTDS.ABudget.Rows)
            {
                if (!BudgetRow.BudgetStatus || AConsolidateAll)
                {
                    if (!AConsolidateAll)
                    {
                        UnPostBudget(BudgetRow, ALedgerNumber);
                    }

                    PostBudget(BudgetRow, ALedgerNumber);
                }
            }

            FinishConsolidateBudget(SubmitChangesTransaction, out AVerificationResult);

            if (AVerificationResult.HasCriticalErrors)
            {
                retVal = false;
                TLogging.Log(AVerificationResult.BuildVerificationResultString());
            }
            else
            {
                GLBatchTDSAccess.SubmitChanges(GLBatchDS, out AVerificationResult);

                if (AVerificationResult.HasCriticalErrors)
                {
                    retVal = false;
                    TLogging.Log(AVerificationResult.BuildVerificationResultString());
                }
            }

            if (IsMyOwnTransaction)
            {
                if (retVal)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return retVal;
        }

        /// <summary>
        /// Complete the Budget consolidation process
        /// </summary>
        /// <param name="ATransaction"></param>
        /// <param name="AVerificationResult"></param>
        private static void FinishConsolidateBudget(
            TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            /*Consolidate_Budget*/
            foreach (ABudgetRow BudgetRow in FBudgetTDS.ABudget.Rows)
            {
                BudgetRow.BudgetStatus = true;
            }

            ABudgetAccess.SubmitChanges(FBudgetTDS.ABudget, ATransaction, out AVerificationResult);
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
                AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSequence,
                    APeriodNumber,
                    null);
                AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow = null;

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
#if todo
            decimal[] TempAmountsThisYear = new decimal[19];                 // AS DECIMAL EXTENT {&MAX-PERIODS} NO-UNDO. Max-Periods = 20
            decimal[] TempAmountsNextYear = new decimal[19];                 // AS DECIMAL EXTENT {&MAX-PERIODS} NO-UNDO. Max-Periods = 20
            //bool lv_answer_l = false;
            int GLMSequenceThisYear;
            int GLMSequenceNextYear;

            ABudgetPeriodTable BPT = ABudgetPeriodAccess.LoadViaABudget(ABudgetRow.BudgetSequence, null);

            ALedgerRow LedgerRow = (ALedgerRow)FBudgetTDS.ALedger.Rows[0];

            GLMSequenceThisYear = TBudgetMaintainWebConnector.GetGLMSequenceForBudget(ALedgerNumber,
                ABudgetRow.AccountCode,
                ABudgetRow.CostCentreCode,
                LedgerRow.CurrentFinancialYear);
            GLMSequenceNextYear = TBudgetMaintainWebConnector.GetGLMSequenceForBudget(ALedgerNumber,
                ABudgetRow.AccountCode,
                ABudgetRow.CostCentreCode,
                LedgerRow.CurrentFinancialYear + 1);

            if ((GLMSequenceThisYear != -1) || (GLMSequenceNextYear != -1))
            {
                AGeneralLedgerMasterPeriodTable GenLedgerMasterPeriodTable = new AGeneralLedgerMasterPeriodTable();
                AGeneralLedgerMasterPeriodRow TemplateRow = (AGeneralLedgerMasterPeriodRow)GenLedgerMasterPeriodTable.NewRowTyped(false);

                TemplateRow.GlmSequence = GLMSequenceThisYear;
                TemplateRow.BudgetBase = 0;

                StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "<>" });

                AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadUsingTemplate(TemplateRow,
                    operators,
                    null,
                    null,
                    null,
                    0,
                    0);


                bool BudgetValueExists = false;

                if (GeneralLedgerMasterPeriodTable.Count > 0)
                {
                    BudgetValueExists = true;
                }
                else
                {
                    AGeneralLedgerMasterPeriodTable GenLedgerMasterPeriodTable2 = new AGeneralLedgerMasterPeriodTable();
                    AGeneralLedgerMasterPeriodRow TemplateRow2 = (AGeneralLedgerMasterPeriodRow)GenLedgerMasterPeriodTable2.NewRowTyped(false);

                    TemplateRow2.GlmSequence = GLMSequenceNextYear;
                    TemplateRow2.BudgetBase = 0;

                    StringCollection operators2 = StringHelper.InitStrArr(new string[] { "=", "<>" });

                    AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable2 = AGeneralLedgerMasterPeriodAccess.LoadUsingTemplate(TemplateRow,
                        operators2,
                        null,
                        null,
                        null,
                        0,
                        0);

                    if (GeneralLedgerMasterPeriodTable2.Count > 0)
                    {
                        BudgetValueExists = true;
                    }
                }

                if (BudgetValueExists)
                {
                    bool IsMyOwnTransaction; // If I create a transaction here, then I need to rollback when I'm done.
                    TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                        out IsMyOwnTransaction);
                    TVerificationResultCollection VerificationResult = null;

                    for (int i = 0; i < BPT.Count; i++)
                    {
                        ABudgetPeriodRow BPR = (ABudgetPeriodRow)BPT.Rows[i];

                        TempAmountsThisYear[BPR.PeriodNumber] = BPR.BudgetThisYear;
                        TempAmountsNextYear[BPR.PeriodNumber] = BPR.BudgetNextYear;
                        BPR.BudgetThisYear = -1 * GetBudgetValue(ref APeriodDataTable, GLMSequenceThisYear, BPR.PeriodNumber);
                        BPR.BudgetNextYear = -1 * GetBudgetValue(ref APeriodDataTable, GLMSequenceNextYear, BPR.PeriodNumber);
                    }

                    ABudgetPeriodAccess.SubmitChanges(BPT, SubmitChangesTransaction, out VerificationResult);

                    /* post the negative budget, which will result in an empty a_glm_period.budget */
                    PostBudget(ref APeriodDataTable, ABudgetRow, ALedgerNumber);

                    BPT = ABudgetPeriodAccess.LoadViaABudget(ABudgetRow.BudgetSequence, null);

                    for (int i = 0; i < BPT.Count; i++)
                    {
                        ABudgetPeriodRow BPR = (ABudgetPeriodRow)BPT.Rows[i];

                        BPR.BudgetThisYear = TempAmountsThisYear[BPR.PeriodNumber];
                        BPR.BudgetNextYear = TempAmountsNextYear[BPR.PeriodNumber];
                    }

                    ABudgetPeriodAccess.SubmitChanges(BPT, SubmitChangesTransaction, out VerificationResult);

                    if (IsMyOwnTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                }

                ABudgetRow.BudgetStatus = false;                 //i.e. unposted
                return true;
            }
#endif
            return false;
        }

        /// <summary>
        /// Post a budget
        /// </summary>
        /// <param name="ABudgetRow"></param>
        /// <param name="ALedgerNumber"></param>
        private static void PostBudget(ABudgetRow ABudgetRow, int ALedgerNumber)
        {
            /* post the negative budget, which will result in an empty a_glm_period.budget */
            //gb5300.p
            string AccountCode = ABudgetRow.AccountCode;

            string CostCentreList = ABudgetRow.CostCentreCode;              /* posting CC and parents */

            //Populate list of affected Cost Centres
            CostCentreParentsList(ALedgerNumber, ref CostCentreList);

            //Locate the row for the current account
            AAccountRow AccountRow = (AAccountRow)GLBatchDS.AAccount.Rows.Find(new object[] { ALedgerNumber, AccountCode });

            /* calculate values for budgets and store them in a temp table; uses lb_budget */
            ProcessAccountParent(
                ALedgerNumber,
                AccountCode,
                AccountRow.DebitCreditIndicator,
                CostCentreList,
                ABudgetRow);
        }

        /// <summary>
        /// Process the account code parent codes
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="CurrAccountCode"></param>
        /// <param name="ADebitCreditIndicator"></param>
        /// <param name="ACostCentreList"></param>
        /// <param name="ABudgetRow"></param>
        private static void ProcessAccountParent(
            int ALedgerNumber,
            string CurrAccountCode,
            bool ADebitCreditIndicator,
            string ACostCentreList,
            ABudgetRow ABudgetRow)
        {
            AAccountRow AccountRow = (AAccountRow)GLBatchDS.AAccount.Rows.Find(new object[] { ALedgerNumber, CurrAccountCode });

            AAccountHierarchyDetailRow AccountHierarchyDetailRow = (AAccountHierarchyDetailRow)GLBatchDS.AAccountHierarchyDetail.Rows.Find(
                new object[] { ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, CurrAccountCode });

            if (AccountHierarchyDetailRow != null)
            {
                string AccountCodeToReportTo = AccountHierarchyDetailRow.AccountCodeToReportTo;

                if ((AccountCodeToReportTo != null) && (AccountCodeToReportTo != string.Empty))
                {
                    //AAccountTable a_parent_account_b = GLBatchDS.AAccount;
                    //AAccountRow AccountRowP = (AAccountRow)a_parent_account_b.Rows.Find(new object[] {ALedgerNumber, AccountCodeToReportTo});

                    /* Recursively call this procedure. */
                    ProcessAccountParent(
                        ALedgerNumber,
                        AccountCodeToReportTo,
                        ADebitCreditIndicator,
                        ACostCentreList,
                        ABudgetRow);
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

            GLBatchDS.AGeneralLedgerMaster.DefaultView.Sort = String.Format("{0},{1},{2},{3}",
                AGeneralLedgerMasterTable.GetLedgerNumberDBName(),
                AGeneralLedgerMasterTable.GetYearDBName(),
                AGeneralLedgerMasterTable.GetAccountCodeDBName(),
                AGeneralLedgerMasterTable.GetCostCentreCodeDBName());

            /* For each associated Cost Centre, update the General Ledger Master. */
            foreach (string CostCentreCode in CostCentres)
            {
                int glmRowIndex = GLBatchDS.AGeneralLedgerMaster.DefaultView.Find(new object[] { ALedgerNumber, ABudgetRow.Year, AccCode,
                                                                                                 CostCentreCode });

                if (glmRowIndex == -1)
                {
                    TGLPosting.CreateGLMYear(ref GLBatchDS, ALedgerNumber, ABudgetRow.Year, AccCode, CostCentreCode);
                    glmRowIndex = GLBatchDS.AGeneralLedgerMaster.DefaultView.Find(new object[] { ALedgerNumber, ABudgetRow.Year, AccCode,
                                                                                                 CostCentreCode });
                }

                int GLMThisYear = ((AGeneralLedgerMasterRow)GLBatchDS.AGeneralLedgerMaster.DefaultView[glmRowIndex].Row).GlmSequence;

                /* Update totals for the General Ledger Master record. */
                FBudgetTDS.ABudgetPeriod.DefaultView.RowFilter = ABudgetPeriodTable.GetBudgetSequenceDBName() + " = " +
                                                                 ABudgetRow.BudgetSequence.ToString();

                foreach (DataRowView rv in FBudgetTDS.ABudgetPeriod.DefaultView)
                {
                    ABudgetPeriodRow BPR = (ABudgetPeriodRow)rv.Row;

                    AddBudgetValue(GLMThisYear, BPR.PeriodNumber, DebitCreditMultiply * BPR.BudgetBase);
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

            ACostCentreRow CostCentreRow = (ACostCentreRow)GLBatchDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, ACurrentCostCentreList });

            ParentCostCentre = CostCentreRow.CostCentreToReportTo;

            while (ParentCostCentre != string.Empty)
            {
                ACostCentreRow CCRow = (ACostCentreRow)GLBatchDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, ParentCostCentre });

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
                (AGeneralLedgerMasterPeriodRow)GLBatchDS.AGeneralLedgerMasterPeriod.Rows.Find(
                    new object[] { AGLMSequence, APeriodNumber });

            if (GeneralLedgerMasterPeriodRow != null)
            {
                GeneralLedgerMasterPeriodRow.BudgetBase += APeriodAmount;
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
                (AGeneralLedgerMasterPeriodRow)GLBatchDS.AGeneralLedgerMaster.Rows.Find(new object[] { AGLMSequence, APeriodNumber });

            if (GeneralLedgerMasterPeriodRow != null)
            {
                GeneralLedgerMasterPeriodRow.BudgetBase = 0;
            }
        }
    }
}