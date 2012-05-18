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

        /// <summary>
        /// load budgets
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool LoadBudgetForConsolidate(Int32 ALedgerNumber)
        {
            //TODO: need to filter on Year
            ABudgetAccess.LoadViaALedger(FBudgetTDS, ALedgerNumber, null);
            ABudgetRevisionAccess.LoadViaALedger(FBudgetTDS, ALedgerNumber, null);
            //TODO: need to filter on ABudgetPeriod using LoadViaBudget or LoadViaUniqueKey
            ABudgetPeriodAccess.LoadAll(FBudgetTDS, null);
            ALedgerAccess.LoadByPrimaryKey(FBudgetTDS, ALedgerNumber, null);

            // Accept row changes here so that the Client gets 'unmodified' rows
            FBudgetTDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            FBudgetTDS.RemoveEmptyTables();

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
            bool retVal = false;

            int Year;
            int Period;

            string PreviousAccount = string.Empty;

            string CurrentGLMAccountCode;
            int CurrentGLMSequence;

            bool IsMyOwnTransaction; // If I create a transaction here, then I need to rollback when I'm done.
            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out IsMyOwnTransaction);
            AVerificationResult = null;

            //Create the temp table
            // Same as the wtPeriodData table in 4GL
            DataTable PeriodDataTempTable = CreateTempTable();

            ABudgetTable BudgetTable = FBudgetTDS.ABudget;
            ABudgetRow BudgetRow = null;

            ALedgerTable LedgerTable = FBudgetTDS.ALedger;
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            if (AConsolidateAll)
            {
                for (int i = 0; i < BudgetTable.Count; i++)
                {
                    BudgetRow = (ABudgetRow)BudgetTable.Rows[i];
                    BudgetRow.BudgetStatus = false;
                }
                ABudgetAccess.SubmitChanges(BudgetTable, SubmitChangesTransaction, out AVerificationResult); // Do I need to be doing this?

                for (int i = 0; i <= 1; i++)
                {
                    Year = LedgerRow.CurrentFinancialYear + i;

                    AGeneralLedgerMasterTable GenLedgerMasterTable = new AGeneralLedgerMasterTable();
                    AGeneralLedgerMasterRow TemplateRow = (AGeneralLedgerMasterRow)GenLedgerMasterTable.NewRowTyped(false);

                    TemplateRow.LedgerNumber = ALedgerNumber;
                    TemplateRow.Year = Year;

                    StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=" });
                    StringCollection OrderList = new StringCollection();

                    OrderList.Add("ORDER BY");
                    OrderList.Add(AGeneralLedgerMasterTable.GetLedgerNumberDBName() + " ASC");
                    OrderList.Add(AGeneralLedgerMasterTable.GetYearDBName() + " ASC");

                    AGeneralLedgerMasterTable GeneralLedgerMasterTable = AGeneralLedgerMasterAccess.LoadUsingTemplate(TemplateRow,
                        operators,
                        null,
                        null,
                        OrderList,
                        0,
                        0);

                    AGeneralLedgerMasterRow GeneralLedgerMasterRow = null;

                    for (int j = 0; j < GeneralLedgerMasterTable.Count; j++)
                    {
                        GeneralLedgerMasterRow = (AGeneralLedgerMasterRow)GeneralLedgerMasterTable.Rows[j];

                        CurrentGLMAccountCode = GeneralLedgerMasterRow.AccountCode;
                        CurrentGLMSequence = GeneralLedgerMasterRow.GlmSequence;

                        if (PreviousAccount != CurrentGLMAccountCode)
                        {
                            PreviousAccount = CurrentGLMAccountCode;
                        }

                        for (Period = 1; Period <= LedgerRow.NumberOfAccountingPeriods; Period++)
                        {
                            ClearAllBudgetValues(ref PeriodDataTempTable, CurrentGLMSequence, Period);
                        }
                    }
                }
            }

            for (int k = 0; k < BudgetTable.Count; k++)
            {
                BudgetRow = (ABudgetRow)BudgetTable.Rows[k];

                if (!BudgetRow.BudgetStatus || AConsolidateAll)
                {
                    if (!AConsolidateAll)
                    {
                        UnPostBudget(ref PeriodDataTempTable, ref BudgetRow, ALedgerNumber);
                    }

                    PostBudget(ref PeriodDataTempTable, ref BudgetRow, ALedgerNumber);
                }
            }

            FinishConsolidateBudget(ALedgerNumber, ref PeriodDataTempTable, ref BudgetTable, SubmitChangesTransaction, out AVerificationResult);
            if (IsMyOwnTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            return retVal;
        }

        /// <summary>
        /// Complete the Budget consolidation process
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APeriodDataTable"></param>
        /// <param name="ABudgetTable"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AVerificationResult"></param>
        private static void FinishConsolidateBudget(int ALedgerNumber, 
            ref DataTable APeriodDataTable, ref ABudgetTable ABudgetTable, 
            TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            decimal IntlExchangeRate;
            int PreviousSequence = 0;
            int CurrentSequence;
            Int32 PeriodNumber;
            Decimal BudgetBase;
            AVerificationResult = null;

            bool IntlExchRateOk = TExchangeRateTools.GetLatestIntlCorpExchangeRate(ALedgerNumber, out IntlExchangeRate);
            /*Consolidate_Budget*/

            AGeneralLedgerMasterPeriodTable GLMPTable = null;
            AGeneralLedgerMasterPeriodRow GLMPRow = null;
            DataRow DR = null;

            for (int i = 0; i < APeriodDataTable.Rows.Count; i++)
            {
                DR = (DataRow)APeriodDataTable.Rows[i];
                CurrentSequence = Convert.ToInt32(DR["GLMSequence"]);
                if (PreviousSequence != CurrentSequence)
                {
                    PreviousSequence = CurrentSequence;
                }
                PeriodNumber = Convert.ToInt32(DR["PeriodNumber"]);
                BudgetBase = Convert.ToDecimal(DR["BudgetBase"]);

                AGeneralLedgerMasterPeriodTable TempGLMPTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey
                    (PreviousSequence, PeriodNumber, ATransaction);
                GLMPRow = (AGeneralLedgerMasterPeriodRow)TempGLMPTable.Rows[0];

                GLMPRow.BeginEdit();
                GLMPRow.BudgetBase = BudgetBase;
                if (IntlExchRateOk)
                {
                    GLMPRow.BudgetIntl = Math.Round(BudgetBase / IntlExchangeRate, 2);
                }
                GLMPRow.EndEdit();
                if (GLMPTable == null)
                {
                    GLMPTable = TempGLMPTable;
                }
                else
                {
                    GLMPTable.Merge(TempGLMPTable);
                }
            }
            AGeneralLedgerMasterPeriodAccess.SubmitChanges(GLMPTable, ATransaction, out AVerificationResult);

            ABudgetRow BudgetRow = null;

            for (int i = 0; i < ABudgetTable.Count; i++)
            {
                BudgetRow = (ABudgetRow)ABudgetTable.Rows[i];

                BudgetRow.BeginEdit();
                BudgetRow.BudgetStatus = true;
                BudgetRow.EndEdit();
            }
            ABudgetAccess.SubmitChanges(ABudgetTable, ATransaction, out AVerificationResult);
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
        /// <param name="APeriodDataTable"></param>
        /// <param name="ABudgetRow"></param>
        /// <param name="ALedgerNumber"></param>
        /// <returns>true if it seemed to go OK</returns>
        private static bool UnPostBudget(ref DataTable APeriodDataTable, ref ABudgetRow ABudgetRow, int ALedgerNumber)
        {
            decimal[] TempAmountsThisYear = new decimal[19];                 // AS DECIMAL EXTENT {&MAX-PERIODS} NO-UNDO. Max-Periods = 20
            decimal[] TempAmountsNextYear = new decimal[19];                 // AS DECIMAL EXTENT {&MAX-PERIODS} NO-UNDO. Max-Periods = 20
            //bool lv_answer_l = false;
            int GLMSequenceThisYear;
            int GLMSequenceNextYear;

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
                    ABudgetPeriodTable BPT = ABudgetPeriodAccess.LoadViaABudget(ABudgetRow.BudgetSequence, null);
                    bool IsMyOwnTransaction; // If I create a transaction here, then I need to rollback when I'm done.
                    TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out IsMyOwnTransaction);
                    TVerificationResultCollection VerificationResult = null;

                    for (int i = 0; i < BPT.Count; i++)
                    {
                        ABudgetPeriodRow BPR = (ABudgetPeriodRow)BPT.Rows[i];

                        TempAmountsThisYear[BPR.PeriodNumber] = BPR.BudgetThisYear;
                        TempAmountsNextYear[BPR.PeriodNumber] = BPR.BudgetNextYear;
                        BPR.BeginEdit();
                        BPR.BudgetThisYear = -1 * GetBudgetValue(ref APeriodDataTable, GLMSequenceThisYear, BPR.PeriodNumber);
                        BPR.BudgetNextYear = -1 * GetBudgetValue(ref APeriodDataTable, GLMSequenceNextYear, BPR.PeriodNumber);
                        BPR.EndEdit();
                    }
                    ABudgetPeriodAccess.SubmitChanges(BPT, SubmitChangesTransaction, out VerificationResult);

                    /* post the negative budget, which will result in an empty a_glm_period.budget */
                    PostBudget(ref APeriodDataTable, ref ABudgetRow, ALedgerNumber);

                    BPT = null;
                    BPT = ABudgetPeriodAccess.LoadViaABudget(ABudgetRow.BudgetSequence, null);

                    for (int i = 0; i < BPT.Count; i++)
                    {
                        ABudgetPeriodRow BPR = (ABudgetPeriodRow)BPT.Rows[i];

                        BPR.BeginEdit();
                        BPR.BudgetThisYear = TempAmountsThisYear[BPR.PeriodNumber];
                        BPR.BudgetNextYear = TempAmountsNextYear[BPR.PeriodNumber];
                        BPR.EndEdit();
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

            return false;
        }

        /// <summary>
        /// Post a budget
        /// </summary>
        /// <param name="APeriodDataTable"></param>
        /// <param name="ABudgetRow"></param>
        /// <param name="ALedgerNumber"></param>
        private static void PostBudget(ref DataTable APeriodDataTable, ref ABudgetRow ABudgetRow, int ALedgerNumber)
        {
            /* post the negative budget, which will result in an empty a_glm_period.budget */
            //gb5300.p
            string AccountCode = ABudgetRow.AccountCode;

            string CostCentreList = ABudgetRow.CostCentreCode;              /* posting CC and parents */

            //Populate list of affected Cost Centres
            CostCentreParentsList(ALedgerNumber, ref CostCentreList);

            //Locate the row for the current account
            GLBatchTDS GLBatchDS = new GLBatchTDS();

            AAccountAccess.LoadViaALedger(GLBatchDS, ALedgerNumber, null);

            AAccountRow AccountRow = (AAccountRow)GLBatchDS.AAccount.Rows.Find(new object[] { ALedgerNumber, AccountCode });

            /* calculate values for budgets and store them in a temp table; uses lb_budget */
            ProcessAccountParent(ref APeriodDataTable,
                ALedgerNumber,
                AccountCode,
                AccountRow.DebitCreditIndicator,
                CostCentreList,
                ABudgetRow.BudgetSequence);
        }

        /// <summary>
        /// Process the account code parent codes
        /// </summary>
        /// <param name="APeriodDataTable"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="CurrAccountCode"></param>
        /// <param name="ADebitCreditIndicator"></param>
        /// <param name="ACostCentreList"></param>
        /// <param name="ABudgetSequence"></param>
        private static void ProcessAccountParent(ref DataTable APeriodDataTable,
            int ALedgerNumber,
            string CurrAccountCode,
            bool ADebitCreditIndicator,
            string ACostCentreList,
            int ABudgetSequence)
        {
            int GLMThisYear;
            int GLMNextYear;

            int DebitCreditMultiply;             /* needed if the debit credit indicator is not the same */
            string CostCentreCode;

            AAccountHierarchyDetailTable AccountHierarchyDetailTable = null;
            AAccountHierarchyDetailRow AccountHierarchyDetailRow = null;

            //Locate the row for the current account
            GLBatchTDS GLBatchDS = new GLBatchTDS();

            AAccountAccess.LoadViaALedger(GLBatchDS, ALedgerNumber, null);     //transaction);

            ALedgerAccess.LoadByPrimaryKey(GLBatchDS, ALedgerNumber, null);     //transaction);
            ALedgerRow LedgerRow = (ALedgerRow)GLBatchDS.ALedger.Rows[0];

            AAccountTable a_current_account_b = GLBatchDS.AAccount;
            AAccountRow AccountRow = (AAccountRow)a_current_account_b.Rows.Find(new object[] { ALedgerNumber, CurrAccountCode });

            try
            {
                AccountHierarchyDetailTable = GLBatchDS.AAccountHierarchyDetail;
                AccountHierarchyDetailRow = (AAccountHierarchyDetailRow)AccountHierarchyDetailTable.Rows.Find(
                    new object[] { ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, CurrAccountCode });

                if (AccountHierarchyDetailRow != null)
                {
                    string AccountCodeToReportTo = AccountHierarchyDetailRow.AccountCodeToReportTo;

                    if ((AccountCodeToReportTo != null) && (AccountCodeToReportTo != string.Empty))
                    {
                        //AAccountTable a_parent_account_b = GLBatchDS.AAccount;
                        //AAccountRow AccountRowP = (AAccountRow)a_parent_account_b.Rows.Find(new object[] {ALedgerNumber, AccountCodeToReportTo});

                        /* Recursively call this procedure. */
                        ProcessAccountParent(ref APeriodDataTable,
                            ALedgerNumber,
                            AccountCodeToReportTo,
                            ADebitCreditIndicator,
                            ACostCentreList,
                            ABudgetSequence);
                    }
                }

                /* If the account has the same db/cr indicator as the original
                 *         account for which the budget was created, add the budget amount.
                 *         Otherwise, subtract. */
                if (AccountRow.DebitCreditIndicator = ADebitCreditIndicator)
                {
                    DebitCreditMultiply = 1;
                }
                else
                {
                    DebitCreditMultiply = -1;
                }

                string[] CostCentres = ACostCentreList.Split(':');
                string AccCode = AccountRow.AccountCode;
                int CurrYear = LedgerRow.CurrentFinancialYear;
                TVerificationResultCollection Verifications = null;

                /* For each associated Cost Centre, update the General Ledger Master. */
                for (int i = 0; i < CostCentres.Length; i++)
                {
                    CostCentreCode = CostCentres[i];

                    GLMThisYear = TBudgetMaintainWebConnector.GetGLMSequenceForBudget(ALedgerNumber, AccCode, CostCentreCode, CurrYear);
                    if (GLMThisYear == -1)
                    {
                        TGLPosting.CreateGLMYear(ref GLBatchDS, ALedgerNumber, CurrYear, AccCode, CostCentreCode);
                        GLBatchTDSAccess.SubmitChanges(GLBatchDS, out Verifications);
                        GLMThisYear = TBudgetMaintainWebConnector.GetGLMSequenceForBudget(ALedgerNumber, AccCode, CostCentreCode, CurrYear);
                        GLBatchDS.AGeneralLedgerMaster.Rows.Clear();            // Leaving these new rows ought to be fine
                        GLBatchDS.AGeneralLedgerMasterPeriod.Rows.Clear();      // but it causes problems below so I'm removing them.
                    }

                    GLMNextYear = TBudgetMaintainWebConnector.GetGLMSequenceForBudget(ALedgerNumber, AccCode, CostCentreCode, CurrYear + 1);
                    if (GLMNextYear == -1)
                    {
                        TGLPosting.CreateGLMYear(ref GLBatchDS, ALedgerNumber, CurrYear + 1, AccCode, CostCentreCode);
                        GLBatchTDSAccess.SubmitChanges(GLBatchDS, out Verifications);
                        GLMNextYear = TBudgetMaintainWebConnector.GetGLMSequenceForBudget(ALedgerNumber, AccCode, CostCentreCode, CurrYear + 1);
                    }


                    /* Update totals for the General Ledger Master record. */
                    ABudgetPeriodTable BPT = ABudgetPeriodAccess.LoadViaABudget(ABudgetSequence, null);
                    ABudgetPeriodRow BPR = null;

                    for (int j = 0; j < BPT.Count; j++)
                    {
                        BPR = (ABudgetPeriodRow)BPT.Rows[j];
                        AddBudgetValue(ref APeriodDataTable, GLMThisYear, BPR.PeriodNumber, DebitCreditMultiply * BPR.BudgetThisYear);
                        AddBudgetValue(ref APeriodDataTable, GLMNextYear, BPR.PeriodNumber, DebitCreditMultiply * BPR.BudgetNextYear);
                    }
                }
            }
            finally
            {
//                if (NewTransaction)
//                {
//                    DBAccess.GDBAccessObj.RollbackTransaction();
//                }
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

            bool NewTransaction = false;
            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            GLBatchTDS GLBatchDS = new GLBatchTDS();

            ACostCentreAccess.LoadViaALedger(GLBatchDS, ALedgerNumber, DBTransaction);

            //ACostCentreAccess.LoadByPrimaryKey(GLBatchDS, ALedgerNumber, ACurrentCostCentre, null);
            ACostCentreRow CostCentreRow = (ACostCentreRow)GLBatchDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, ACurrentCostCentreList });

            ParentCostCentre = CostCentreRow.CostCentreToReportTo;

            while (ParentCostCentre != string.Empty)
            {
                ACostCentreRow CCRow = (ACostCentreRow)GLBatchDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, ParentCostCentre });

                CostCentreList += ":" + CCRow.CostCentreCode;
                ParentCostCentre = CCRow.CostCentreToReportTo;
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }

        /// <summary>
        /// Create the temp table that stores the GLMSequence and budget data
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateTempTable()
        {
            DataTable wtPeriodData = new DataTable();

            wtPeriodData.Columns.Add("GLMSequence", typeof(int));
            wtPeriodData.Columns.Add("PeriodNumber", typeof(int));
            wtPeriodData.Columns.Add("BudgetBase", typeof(decimal));
            wtPeriodData.PrimaryKey = new DataColumn[2] {
                wtPeriodData.Columns[0], wtPeriodData.Columns[1]
            };

            return wtPeriodData;
        }

        /// <summary>
        /// Write a budget value to the temporary table
        /// </summary>
        /// <param name="APeriodDataTable"></param>
        /// <param name="AGLMSequence"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="APeriodAmount"></param>
        private static void AddBudgetValue(ref DataTable APeriodDataTable, int AGLMSequence, int APeriodNumber, decimal APeriodAmount)
        {
            /*  Add a budget amount to the temp table APeriodDataTable.
             *          If the record is not already in the temp table, it is created.
             */
            DataRow TempRow = (DataRow)APeriodDataTable.Rows.Find(new object[] { AGLMSequence, APeriodNumber });

            if (TempRow == null)
            {
                AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = 
                    AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSequence, APeriodNumber, null);

                if (GeneralLedgerMasterPeriodTable.Count > 0)
                {
                    AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];

                    TempRow = (DataRow)APeriodDataTable.NewRow();
                    TempRow["GLMSequence"] = AGLMSequence;
                    TempRow["PeriodNumber"] = APeriodNumber;
                    TempRow["BudgetBase"] = GeneralLedgerMasterPeriodRow.BudgetBase;

                    APeriodDataTable.Rows.Add(TempRow);
                }
            }

            TempRow["BudgetBase"] = Convert.ToDecimal(TempRow["BudgetBase"]) + APeriodAmount;
        }

        /// <summary>
        /// Reset the budget amount in the temp table wtPeriodData.
        ///   if the record is not already in the temp table, it is created empty
        /// </summary>
        /// <param name="ATempTable"></param>
        /// <param name="AGLMSequence"></param>
        /// <param name="APeriodNumber"></param>
        private static void ClearAllBudgetValues(ref DataTable ATempTable, int AGLMSequence, int APeriodNumber)
        {
            DataRow TempRow = (DataRow)ATempTable.Rows.Find(new object[] { AGLMSequence, APeriodNumber });

            try
            {
                if (TempRow == null)
                {
                    AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = 
                        AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey (AGLMSequence, APeriodNumber, null);

                    if (GeneralLedgerMasterPeriodTable.Count > 0)
                    {
                        AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];

                        /* Only create records for periods which have a value. 
                         * Try to keep the number of records low,
                         * to make the lock count in the write transaction smaller.
                         */
                        if (GeneralLedgerMasterPeriodRow.BudgetBase != 0)
                        {
                            DataRow DR = (DataRow)ATempTable.NewRow();
                            DR["GLMSequence"] = AGLMSequence;
                            DR["PeriodNumber"] = APeriodNumber;
                            DR["BudgetBase"] = 0;

                            ATempTable.Rows.Add(DR);
                        }
                    }
                }
                else
                {
                    TempRow["BudgetBase"] = 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}