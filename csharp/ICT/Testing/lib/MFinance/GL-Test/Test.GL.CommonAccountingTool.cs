//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
//
// Copyright 2004-2010 by OM International
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
using NUnit.Framework;
using Ict.Testing.NUnitForms;
using Ict.Petra.Server.MFinance.GL;


using Ict.Common;


namespace Ict.Testing.Petra.Server.MFinance.GL
{
    public partial class TestCommonAccountingTool : CommonNUnitFunctions
    {
        int LedgerNumber = 43;


        /// <summary>
        /// This routine tests the TLedgerInitFlagHandler completely. It's the routine
        /// which writes "boolean" values to a data base table. The class Get_GLM_Info is
        /// tested indirect too.
        ///
        /// Be careful by changing this routine. The behaviour has been compared to this of
        /// petra and it has been verfied that the same database entries of the table
        /// a_transaction has been created and the same changes in glm.
        /// </summary>
        [Test]
        public void Test_01_BaseCurrencyAccounting()
        {
            // <summary>
            // 9800 is definde as credit Account and so an accouting in "credit direction" is
            // added as a postive value to GLM.
            // 9800 is definde as debit Account and so an accouting in "debit direction" is
            // added as a postive value to GLM.
            // </summary>

            string strAccountStart = "6000";
            string strAccountEnd = "9800";
            string strCostCentre = "4300";

            // Get the glm-values before and after the test and taking the differences enables
            // to run the test several times
            Get_GLM_Info getGLM_InfoBeforeStart = new Get_GLM_Info(LedgerNumber, strAccountStart, strCostCentre);
            Get_GLM_Info getGLM_InfoBeforeEnd = new Get_GLM_Info(LedgerNumber, strAccountEnd, strCostCentre);

            CommonAccountingTool commonAccountingTool =
                new CommonAccountingTool(LedgerNumber, "NUNIT");

            commonAccountingTool.AddBaseCurrencyJournal();

            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountStart, strCostCentre, "Debit-Test-10", "NUNIT",
                CommonAccountingConstants.IS_DEBIT, 10);
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountEnd, strCostCentre, "Credit-Test 10", "NUNIT",
                CommonAccountingConstants.IS_CREDIT, 10);
            int intBatchNumber = commonAccountingTool.CloseSaveAndPost();

            Get_GLM_Info getGLM_InfoAfterStart = new Get_GLM_Info(LedgerNumber, strAccountStart, strCostCentre);
            Get_GLM_Info getGLM_InfoAfterEnd = new Get_GLM_Info(LedgerNumber, strAccountEnd, strCostCentre);

            // strAccountStart is a debit account -> in this case "+"
            Assert.AreEqual(getGLM_InfoBeforeStart.YtdActual + 10, getGLM_InfoAfterStart.YtdActual,
                "Check if 10 has been accounted");
            // strAccountEnd is a credit acount -> in this case "+" too!
            Assert.AreEqual(getGLM_InfoBeforeEnd.YtdActual + 10, getGLM_InfoAfterEnd.YtdActual,
                "Check if 10 has been accounted");

            commonAccountingTool =
                new CommonAccountingTool(LedgerNumber, "NUNIT");

            commonAccountingTool.AddBaseCurrencyJournal();

            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountStart, strCostCentre, "Debit-Test-5", "NUNIT",
                CommonAccountingConstants.IS_CREDIT, 5);
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountEnd, strCostCentre, "Credit-Test 5", "NUNIT",
                CommonAccountingConstants.IS_DEBIT, 5);
            intBatchNumber = commonAccountingTool.CloseSaveAndPost();

            getGLM_InfoAfterStart = new Get_GLM_Info(LedgerNumber, strAccountStart, strCostCentre);
            getGLM_InfoAfterEnd = new Get_GLM_Info(LedgerNumber, strAccountEnd, strCostCentre);
            // now both directions are "-" and so the difference is reduced to 5
            Assert.AreEqual(getGLM_InfoBeforeStart.YtdActual + 5, getGLM_InfoAfterStart.YtdActual,
                "Check if 10 has been accounted");
            // strAccountEnd is a credit acount -> in this case "+" too!
            Assert.AreEqual(getGLM_InfoBeforeEnd.YtdActual + 5, getGLM_InfoAfterEnd.YtdActual,
                "Check if 10 has been accounted");
        }

        /// <summary>
        /// Tests the foreign Currency part of the CommonAccountingTool.
        /// </summary>
        [Test]
        public void Test_02_ForeignCurrencyAccounting()
        {
            string strAccountStart = "6001";      // Use an foreign currency account in GBP only
            string strAccountEnd = "9800";        // Use a base currency account only
            string strCostCentre = "4300";

            // Check if some special test data are availiable - otherwise load ...
            if (!new GetAccountInfo(null, new GetLedgerInfo(LedgerNumber), "6001").IsValid)
            {
                LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL-Test\\" +
                    "test-sql\\gl-test-account-data.sql");
            }

            // Check if some special test data are availiable - otherwise load ...
            if (!new GetCostCenterInfo(LedgerNumber, "4301").IsValid)
            {
                LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL-Test\\" +
                    "test-sql\\gl-test-costcentre-data.sql");
            }

            // Get the glm-values before and after the test and taking the differences enables
            // to run the test several times
            Get_GLM_Info getGLM_InfoBeforeStart = new Get_GLM_Info(LedgerNumber, strAccountStart, strCostCentre);
            Get_GLM_Info getGLM_InfoBeforeEnd = new Get_GLM_Info(LedgerNumber, strAccountEnd, strCostCentre);

            CommonAccountingTool commonAccountingTool =
                new CommonAccountingTool(LedgerNumber, "NUNIT");

            commonAccountingTool.AddForeignCurrencyJournal("GBP", 0.3m);

            commonAccountingTool.AddForeignCurrencyTransaction(
                strAccountStart, strCostCentre, "Debit GBP 100", "NUNIT",
                CommonAccountingConstants.IS_DEBIT, 100, 333.33m);
            commonAccountingTool.AddForeignCurrencyTransaction(
                strAccountEnd, strCostCentre, "Credit GBP 100", "NUNIT",
                CommonAccountingConstants.IS_CREDIT, 100, 333.33m);

            int intBatchNumber = commonAccountingTool.CloseSaveAndPost();

            Get_GLM_Info getGLM_InfoAfterStart = new Get_GLM_Info(LedgerNumber, strAccountStart, strCostCentre);
            Get_GLM_Info getGLM_InfoAfterEnd = new Get_GLM_Info(LedgerNumber, strAccountEnd, strCostCentre);

            Assert.AreEqual(getGLM_InfoBeforeStart.YtdActual + 100, getGLM_InfoAfterStart.YtdActual,
                "Check if 100 has been accounted");
            Assert.AreEqual(getGLM_InfoBeforeEnd.YtdActual + 100, getGLM_InfoAfterEnd.YtdActual,
                "Check if 100 has been accounted");

            Assert.AreEqual(getGLM_InfoBeforeStart.YtdForeign +  + 333.33m, getGLM_InfoAfterStart.YtdForeign,
                "Check if 333.33 has been accounted");
            Assert.AreEqual(getGLM_InfoBeforeEnd.YtdForeign, getGLM_InfoAfterEnd.YtdForeign,
                "Check if nothing has been accounted");
        }

        /// <summary>
        /// Check an spefic error ...
        /// </summary>
        [Test]
        public void Test_03_ForeignCurrencyAccountingWithWrongForeignValue()
        {
            string strAccountStart = "6001";      // Use an foreign currency account in GBP only
            string strAccountEnd = "9800";        // Use a base currency account only
            string strCostCentre = "4300";

            // Check if some special test data are availiable - otherwise load ...
            if (!new GetAccountInfo(null, new GetLedgerInfo(LedgerNumber), "6001").IsValid)
            {
                LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL-Test\\" +
                    "test-sql\\gl-test-account-data.sql");
            }

            // Check if some special test data are availiable - otherwise load ...
            if (!new GetCostCenterInfo(LedgerNumber, "4301").IsValid)
            {
                LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL-Test\\" +
                    "test-sql\\gl-test-costcentre-data.sql");
            }

            CommonAccountingTool commonAccountingTool =
                new CommonAccountingTool(LedgerNumber, "NUNIT");
            try
            {
                commonAccountingTool.AddForeignCurrencyJournal("JPY", 0.3m);
                commonAccountingTool.AddForeignCurrencyTransaction(
                    strAccountStart, strCostCentre, "Credit GBP 100", "NUNIT",
                    CommonAccountingConstants.IS_DEBIT, 100, 333.33m);
                commonAccountingTool.AddForeignCurrencyTransaction(
                    strAccountEnd, strCostCentre, "Debit GBP 100", "NUNIT",
                    CommonAccountingConstants.IS_CREDIT, 100, 333.33m);
                Assert.Fail("Exception does not appear!");
            }
            catch (TerminateException)
            {
                Assert.Pass("Exception was thrown");
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.ToString());
                Assert.Fail("Wrong exception thrown");
            }
        }

        [Test]
        public void Test_04_ENum()
        {
            string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            System.Diagnostics.Debug.WriteLine(username);

            TLedgerInitFlagHandler tifh = new TLedgerInitFlagHandler(
                LedgerNumber, TLedgerInitFlagEnum.DatabaseAllocation);
            System.Diagnostics.Debug.WriteLine(tifh.Flag.ToString());
            tifh.SetFlagAndName(username);
            System.Diagnostics.Debug.WriteLine(tifh.Flag.ToString());
        }

        [TestFixtureSetUp]
        public void Init()
        {
            InitServerConnection();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            DisconnectServerConnection();
        }
    }
}