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
        /// </summary>
        [Test]
        public void Test_01_BaseCurrencyAccounting()
        {
        	string strAccountStart = "9800";
        	string strAccountEnd = "6000";
        	string strCostCentre = "4300";

        	// Get the glm-values before and after the test and taking the differences enables 
        	// to run the test several times
        	Get_GLM_Info getGLM_InfoBeforeStart = new Get_GLM_Info(LedgerNumber,strAccountStart,strCostCentre);
        	Get_GLM_Info getGLM_InfoBeforeEnd = new Get_GLM_Info(LedgerNumber,strAccountEnd,strCostCentre);
        	
        	CommonAccountingTool commonAccountingTool = 
        		new CommonAccountingTool(LedgerNumber, "Test-Description");

        	commonAccountingTool.AddBaseCurrencyJournal();

        	commonAccountingTool.AddBaseCurrencyTransaction(
        		strAccountStart, strCostCentre, "Narrative", "Reference", 
        		CommonAccountingConstants.IS_CREDIT, 10);
        	commonAccountingTool.AddBaseCurrencyTransaction(
        		strAccountEnd, strCostCentre, "Narrative", "Reference", 
        		CommonAccountingConstants.IS_DEBIT, 10);
        	int intBatchNumber = commonAccountingTool.CloseSaveAndPost();        	

        	Get_GLM_Info getGLM_InfoAfterStart = new Get_GLM_Info(LedgerNumber,strAccountStart,strCostCentre);
        	Get_GLM_Info getGLM_InfoAfterEnd = new Get_GLM_Info(LedgerNumber,strAccountEnd,strCostCentre);

        	// strAccountStart is a debit account -> in this case "-"
        	Assert.AreEqual(getGLM_InfoBeforeStart.YtdActual - 10, getGLM_InfoAfterStart.YtdActual,
        	                "Check if 10 has been accounted");
        	// strAccountEnd is a credit acount -> in this case "-" too!
        	Assert.AreEqual(getGLM_InfoBeforeEnd.YtdActual - 10, getGLM_InfoAfterEnd.YtdActual, 
        	                "Check if 10 has been accounted");
        }

        [Test]
        public void Test_02_ForeignCurrencyAccounting()
        {
        	string strAccountStart = "9800";
        	string strAccountEnd = "6001";
        	string strCostCentre = "4300";

        	// Get the glm-values before and after the test and taking the differences enables 
        	// to run the test several times
        	Get_GLM_Info getGLM_InfoBeforeStart = new Get_GLM_Info(LedgerNumber,strAccountStart,strCostCentre);
        	Get_GLM_Info getGLM_InfoBeforeEnd = new Get_GLM_Info(LedgerNumber,strAccountEnd,strCostCentre);
        	
        	CommonAccountingTool commonAccountingTool = 
        		new CommonAccountingTool(LedgerNumber, "Test-Description");

        	commonAccountingTool.AddForeignCurrencyJournal("GBP", 2);

        	commonAccountingTool.AddForeignCurrencyTransaction(
        		strAccountStart, strCostCentre, "Narrative", "Reference", 
        		CommonAccountingConstants.IS_CREDIT, 10);
        	commonAccountingTool.AddForeignCurrencyTransaction(
        		strAccountEnd, strCostCentre, "Narrative", "Reference", 
        		CommonAccountingConstants.IS_DEBIT, 10);
        	
        	int intBatchNumber = commonAccountingTool.CloseSaveAndPost();        	

        	Get_GLM_Info getGLM_InfoAfterStart = new Get_GLM_Info(LedgerNumber,strAccountStart,strCostCentre);
        	Get_GLM_Info getGLM_InfoAfterEnd = new Get_GLM_Info(LedgerNumber,strAccountEnd,strCostCentre);

        	// strAccountStart is a debit account -> in this case "-"
        	Assert.AreEqual(getGLM_InfoBeforeStart.YtdActual - 10, getGLM_InfoAfterStart.YtdActual,
        	                "Check if 10 has been accounted");
        	// strAccountEnd is a credit acount -> in this case "-" too!
        	Assert.AreEqual(getGLM_InfoBeforeEnd.YtdActual - 10, getGLM_InfoAfterEnd.YtdActual, 
        	                "Check if 10 has been accounted");

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