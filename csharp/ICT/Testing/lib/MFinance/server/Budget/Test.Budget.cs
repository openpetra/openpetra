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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Data.Odbc;
using NUnit.Framework;
using Ict.Testing.NUnitTools;
using Ict.Testing.NUnitPetraServer;
using Ict.Petra.Server.MFinance.GL;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.Budget.WebConnectors;
using Ict.Petra.Server.MFinance.GL.Data.Access;
using Ict.Petra.Shared.MFinance;

namespace Ict.Testing.Petra.Server.MFinance.Budget
{
    /// <summary>
    /// a couple of tests for Budget
    /// </summary>
    [TestFixture]
    public class TestBudget
    {
        private const int FLedgerNumber = 43;
        private BudgetTDS FMainDS;

        /// <summary>
        /// TestFixtureSetUp
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect("../../etc/TestServer.config");
        }

        /// <summary>
        /// TearDown the test
        /// </summary>
        [TestFixtureTearDown]
        public void TearDownTest()
        {
            TPetraServerConnector.Disconnect();
        }

        /// <summary>
        /// test the budget consolidation
        /// </summary>
        [Test]
        public void T0_Consolidation()
        {
            // reset the database, so that there is no consolidated budget
            //CommonNUnitFunctions.ResetDatabase();

            string budgetTestFile = TAppSettingsManager.GetValue("GiftBatch.file",
                CommonNUnitFunctions.rootPath + "/csharp/ICT/Testing/lib/MFinance/SampleData/BudgetImport-All.csv");

            TVerificationResultCollection VerificationResult;

            BudgetTDS ImportDS = new BudgetTDS();

            // import budget from CSV
            decimal RowsImported = TBudgetMaintainWebConnector.ImportBudgets(
                FLedgerNumber,
                0,
                budgetTestFile,
                new string[] { ",", "dmy", "American" },
                ref ImportDS,
                out VerificationResult);

            Assert.AreNotEqual(0, RowsImported, "expect to import several rows");

            if (VerificationResult != null)
            {
                Assert.AreEqual(false,
                    VerificationResult.HasCriticalErrors,
                    "ImportBudgets has critical errors: " + VerificationResult.BuildVerificationResultString());
            }

            Assert.AreEqual(TSubmitChangesResult.scrOK, BudgetTDSAccess.SubmitChanges(ImportDS,
                    out VerificationResult), "submitchanges of imported budget");

            // check for value in budget table
            string sqlQueryBudget =
                String.Format(
                    "SELECT {0} FROM PUB_{1}, PUB_{2} WHERE {1}.a_budget_sequence_i = {2}.a_budget_sequence_i AND a_period_number_i = 1 AND " +
                    "a_ledger_number_i = {3} AND a_revision_i = 0 AND a_year_i = 0 AND a_account_code_c = '0300' AND a_cost_centre_code_c = '4300'",
                    ABudgetPeriodTable.GetBudgetBaseDBName(),
                    ABudgetTable.GetTableDBName(),
                    ABudgetPeriodTable.GetTableDBName(),
                    FLedgerNumber);

            decimal budgetValue = Convert.ToDecimal(DBAccess.GDBAccessObj.ExecuteScalar(sqlQueryBudget, IsolationLevel.ReadCommitted));
            Assert.AreEqual(250m, budgetValue, "problem with importing budget from CSV");

            // check for zero in glmperiod budget: that row does not even exist yet, so check that it does not exist
            string sqlQueryCheckEmptyConsolidatedBudget =
                String.Format(
                    "SELECT COUNT(*) FROM PUB_{1}, PUB_{2} WHERE {1}.a_glm_sequence_i = {2}.a_glm_sequence_i AND a_period_number_i = 1 AND " +
                    "a_ledger_number_i = {3} AND a_year_i = 0 AND a_account_code_c = '0300' AND a_cost_centre_code_c = '4300'",
                    AGeneralLedgerMasterPeriodTable.GetBudgetBaseDBName(),
                    AGeneralLedgerMasterPeriodTable.GetTableDBName(),
                    AGeneralLedgerMasterTable.GetTableDBName(),
                    FLedgerNumber);

            Assert.AreEqual(0, DBAccess.GDBAccessObj.ExecuteScalar(sqlQueryCheckEmptyConsolidatedBudget,
                    IsolationLevel.ReadCommitted), "budget should not be consolidated yet");

            // consolidate the budget
            TBudgetConsolidateWebConnector.LoadBudgetForConsolidate(FLedgerNumber);
            bool consolidated = TBudgetConsolidateWebConnector.ConsolidateBudgets(FLedgerNumber, true, out VerificationResult);

            if (VerificationResult != null)
            {
                Assert.AreEqual(false,
                    VerificationResult.HasCriticalErrors,
                    "ConsolidateBudget has critical errors: " + VerificationResult.BuildVerificationResultString());
            }

            Assert.AreEqual(true, consolidated, "consolidating the budgets");

            // check for correct value in glmperiod budget
            string sqlQueryConsolidatedBudget =
                String.Format(
                    "SELECT {0} FROM PUB_{1}, PUB_{2} WHERE {1}.a_glm_sequence_i = {2}.a_glm_sequence_i AND a_period_number_i = 1 AND " +
                    "a_ledger_number_i = {3} AND a_year_i = 0 AND a_account_code_c = '0300' AND a_cost_centre_code_c = '4300'",
                    AGeneralLedgerMasterPeriodTable.GetBudgetBaseDBName(),
                    AGeneralLedgerMasterPeriodTable.GetTableDBName(),
                    AGeneralLedgerMasterTable.GetTableDBName(),
                    FLedgerNumber);

            decimal consolidatedBudgetValue =
                Convert.ToDecimal(DBAccess.GDBAccessObj.ExecuteScalar(sqlQueryConsolidatedBudget, IsolationLevel.ReadCommitted));
            Assert.AreEqual(250m, consolidatedBudgetValue, "budget should now be consolidated");

            // TODO: also check some summary account and cost centre for summed up budget values

            // check how reposting a budget works
            string sqlChangeBudget = String.Format("UPDATE PUB_{0} SET {1} = 44 WHERE a_period_number_i = 1 AND " +
                "EXISTS (SELECT * FROM PUB_{2} WHERE {0}.a_budget_sequence_i = {2}.a_budget_sequence_i AND a_ledger_number_i = {3} " +
                "AND a_year_i = 0 AND a_revision_i = 0 AND a_account_code_c = '0300' AND a_cost_centre_code_c = '4300')",
                ABudgetPeriodTable.GetTableDBName(),
                ABudgetPeriodTable.GetBudgetBaseDBName(),
                ABudgetTable.GetTableDBName(),
                FLedgerNumber);

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            DBAccess.GDBAccessObj.ExecuteNonQuery(sqlChangeBudget, Transaction);

            DBAccess.GDBAccessObj.CommitTransaction();

            // post all budgets again
            TBudgetConsolidateWebConnector.LoadBudgetForConsolidate(FLedgerNumber);
            consolidated = TBudgetConsolidateWebConnector.ConsolidateBudgets(FLedgerNumber, true, out VerificationResult);

            if (VerificationResult != null)
            {
                Assert.AreEqual(false,
                    VerificationResult.HasCriticalErrors,
                    "2nd ConsolidateBudget has critical errors: " + VerificationResult.BuildVerificationResultString());
            }

            Assert.AreEqual(true, consolidated, "consolidating the budgets for the second time");

            consolidatedBudgetValue =
                Convert.ToDecimal(DBAccess.GDBAccessObj.ExecuteScalar(sqlQueryConsolidatedBudget, IsolationLevel.ReadCommitted));
            Assert.AreEqual(44.0m, consolidatedBudgetValue, "budget should be consolidated with the new value");

            // post only a modified budget (testing UnPostBudget)
            sqlChangeBudget = String.Format("UPDATE PUB_{0} SET {1} = 65 WHERE a_period_number_i = 1 AND " +
                "EXISTS (SELECT * FROM PUB_{2} WHERE {0}.a_budget_sequence_i = {2}.a_budget_sequence_i AND a_ledger_number_i = {3} " +
                "AND a_year_i = 0 AND a_revision_i = 0 AND a_account_code_c = '0300' AND a_cost_centre_code_c = '4300')",
                ABudgetPeriodTable.GetTableDBName(),
                ABudgetPeriodTable.GetBudgetBaseDBName(),
                ABudgetTable.GetTableDBName(),
                FLedgerNumber);

            string sqlMarkBudgetForConsolidation = String.Format("UPDATE PUB_{0} SET {1} = false WHERE " +
                "a_ledger_number_i = {2} " +
                "AND a_year_i = 0 AND a_revision_i = 0 AND a_account_code_c = '0300' AND a_cost_centre_code_c = '4300'",
                ABudgetTable.GetTableDBName(),
                ABudgetTable.GetBudgetStatusDBName(),
                FLedgerNumber);

            Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            DBAccess.GDBAccessObj.ExecuteNonQuery(sqlChangeBudget, Transaction);
            DBAccess.GDBAccessObj.ExecuteNonQuery(sqlMarkBudgetForConsolidation, Transaction);

            DBAccess.GDBAccessObj.CommitTransaction();

            // post only modified budget again
            TBudgetConsolidateWebConnector.LoadBudgetForConsolidate(FLedgerNumber);
            consolidated = TBudgetConsolidateWebConnector.ConsolidateBudgets(FLedgerNumber, false, out VerificationResult);

            if (VerificationResult != null)
            {
                Assert.AreEqual(false,
                    VerificationResult.HasCriticalErrors,
                    "3rd ConsolidateBudget has critical errors: " + VerificationResult.BuildVerificationResultString());
            }

            Assert.AreEqual(true, consolidated, "consolidating the budgets for the third time");

            consolidatedBudgetValue =
                Convert.ToDecimal(DBAccess.GDBAccessObj.ExecuteScalar(sqlQueryConsolidatedBudget, IsolationLevel.ReadCommitted));
            Assert.AreEqual(65.0m, consolidatedBudgetValue, "budget should be consolidated with the new value, after UnPostBudget");

            // TODO: test forwarding periods. what happens to next year values, when there is no next year glm record yet?
        }

        /// <summary>
        /// test the budget autogeneration
        /// </summary>
        [Test]
        public void T1_AutoGenerationLoadData()
        {
        	FMainDS = new BudgetTDS();
        	
        	FMainDS.Merge(TBudgetAutoGenerateWebConnector.LoadBudgetForAutoGenerate(FLedgerNumber));
        	
        	//Not needed
        	FMainDS.RemoveTable("AGeneralLedgerMasterPeriod");
        	
        	string emptyTables = string.Empty;
        	
        	foreach (DataTable tb in FMainDS.Tables)
        	{
        		if (FMainDS.Tables[tb.TableName].Rows.Count == 0)
        		{
        			emptyTables += tb.TableName + "; ";
        		}
        	}

        	Assert.IsEmpty(emptyTables, "Empty Budget Autogeneration Tables: " + emptyTables);
        }

        /// <summary>
        /// test the budget autogeneration
        /// </summary>
        [Test]
        public void T2_AutoGenerationGenBudget()
        {
			//FMainDS loaded in previous test
			int budgetSequence = FMainDS.ABudget.Count > 0 ? FMainDS.ABudget[0].BudgetSequence: 0;
        	string forecastType = MFinanceConstants.FORECAST_TYPE_BUDGET;
        	
        	if (FMainDS.ABudget.Count == 0)
			{
				return;
			}
        		    	
        	Assert.IsTrue(TBudgetAutoGenerateWebConnector.GenBudgetForNextYear(FLedgerNumber, budgetSequence, forecastType),"Budget Autogenerate failed!");
        }

        /// <summary>
        /// test the budget autogeneration
        /// </summary>
        [Test]
        public void T3_AutoGenerationGetBudgetAmount()
        {
			//FMainDS loaded in previous test
        	if (FMainDS.ABudget.Count == 0)
			{
				return;
			}
        	
			int budgetSequence = FMainDS.ABudget[0].BudgetSequence;
			int periodNo = 1;

			ABudgetPeriodRow bPRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] {budgetSequence, 1});
        	
        	if (bPRow == null)
        	{
        		Assert.IsNotNull(bPRow, String.Format("Cannot find budget period {0} value for budget sequence {1}", periodNo, budgetSequence));
        		return;
        	}
        	
        	decimal baseBudgetAmountFromTable = bPRow.BudgetBase;
        	decimal baseBudgetAmountFromFunction = TBudgetAutoGenerateWebConnector.GetBudgetPeriodAmount(budgetSequence, 1);

        	//Check if the value in the database equals that delivered by the function
        	Assert.IsTrue(baseBudgetAmountFromTable == baseBudgetAmountFromFunction, String.Format("GetBudgetPeriod Failed. Base Amount in Table is {0}. Function returns: {1}",
        		                                                                              baseBudgetAmountFromTable,
        		                                                                              baseBudgetAmountFromFunction));
        }

        /// <summary>
        /// test the budget autogeneration
        /// </summary>
        [Test]
        public void T4_AutoGenerationSetBudgetAmount()
        {
			//FMainDS loaded in previous test
        	if (FMainDS.ABudget.Count == 0)
			{
				return;
			}
        	
			int budgetSequence = FMainDS.ABudget[0].BudgetSequence;
        	
        	ABudgetPeriodRow bPRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] {budgetSequence, 1});
        	
        	if (bPRow == null)
        	{
        		Assert.IsNotNull(bPRow, String.Format("Cannot find budget period 1 value for budget sequence {0}", budgetSequence));
        		return;
        	}
        	
			//Add 10 to Budget base value and check if it is written
        	decimal budgetBase = bPRow.BudgetBase;
        	TBudgetAutoGenerateWebConnector.SetBudgetPeriodBaseAmount(budgetSequence, 1, (budgetBase + 10));
        	
			decimal budgetBaseNew = TBudgetAutoGenerateWebConnector.GetBudgetPeriodAmount(budgetSequence, 1);
			Assert.IsTrue(budgetBaseNew == (budgetBase + 10), String.Format("SetBudgetPeriod Failed. BudgetBase ({0}) has not been updated to: {1}",
			                                                         budgetBase,
			                                                        budgetBaseNew));
        }
    }
}