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
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.MFinance.Gui;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;
using NUnit.Extensions.Forms;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Tests.MFinance.GLBatches
{
    [TestFixture]
    public class GLBatch_test : CommonNUnitFunctions
    {
        // Each account is defined by its LedgerNumber ...
        // Actually the value is read by the TAppSettingsManager
        private Int32 fLedgerNumber;

        private void CheckControlStatusEnabled(
            string message, bool shallBeStatus, bool isStatus)
        {
            Assert.AreEqual(shallBeStatus, isStatus, "Control.enabled(" + message + ")");
        }

        private void CheckCurrencySetting(String propertyText)
        {
            Assert.That(propertyText, new NotConstraint(new SubstringConstraint("#")),
                "Problems with CurrencySymbol settings");
        }

        [Test]
        public void Test_01_AddBatch()
        {
            TFrmGLBatchTester tFrmGLBatchTester = new TFrmGLBatchTester();

            tFrmGLBatchTester.tFrmGLBatch.Show();
            tFrmGLBatchTester.tFrmGLBatch.LedgerNumber = fLedgerNumber;

            String strH;
            Boolean boolH;
            strH = tFrmGLBatchTester.ttpgBatches.txtLedgerNumber.Properties.Text;
            StringAssert.Contains(fLedgerNumber.ToString(), strH, "Invalid LedgerNumber");

            tFrmGLBatchTester.ttpgBatches.rbtPosting.Click();

            if (tFrmGLBatchTester.ttpgBatches.grdDetails.Properties.Rows.Count == 1)
            {
                //                                                      Headline is a row!

                // Check the stati of some controls ...
                CheckControlStatusEnabled("btnPostBatch", false,
                    tFrmGLBatchTester.ttpgBatches.btnPostBatch.Properties.Enabled);
                CheckControlStatusEnabled("btnCancel", false,
                    tFrmGLBatchTester.ttpgBatches.btnCancel.Properties.Enabled);
                CheckControlStatusEnabled("btnNew", false,
                    tFrmGLBatchTester.ttpgBatches.btnNew.Properties.Enabled);

                CheckControlStatusEnabled("txtDetailBatchDescription", false,
                    tFrmGLBatchTester.ttpgBatches.txtDetailBatchDescription.Properties.Enabled);
                CheckControlStatusEnabled("txtDetailBatchControlTotal", false,
                    tFrmGLBatchTester.ttpgBatches.txtDetailBatchControlTotal.Properties.Enabled);
                CheckControlStatusEnabled("dtpDetailDateEffective", false,
                    tFrmGLBatchTester.ttpgBatches.dtpDetailDateEffective.Properties.Enabled);

                tFrmGLBatchTester.ttpgBatches.rbtEditing.Click();

                tFrmGLBatchTester.ttpgBatches.btnNew.Click();

                tFrmGLBatchTester.ttpgBatches.txtDetailBatchDescription.Properties.Text =
                    "NUnit-Forms gnerated batch";

                tFrmGLBatchTester.ttpgBatches.txtDetailBatchControlTotal.Properties.NumberValueDecimal = 555;

                CheckCurrencySetting(tFrmGLBatchTester.ttpgBatches.txtDetailBatchControlTotal.Properties.Text);

                String strValidRange = tFrmGLBatchTester.ttpgBatches.lblValidDateRange.Properties.Text;
                DateConverter dateConverter = new DateConverter();

                DateTime dteStart = dateConverter.GetNthDate(strValidRange, 0);
                DateTime dteEnd = dateConverter.GetNthDate(strValidRange, 1);

                TimeSpan timeDiff = dteEnd.Subtract(dteStart);
                Int16 dayDiff = Convert.ToInt16(timeDiff.TotalDays);

                if (dayDiff <= 10)
                {
                    Assert.Less(10, dayDiff, "Time Intervall to small to run a test");
                }

                DateTime dteToUse = dteEnd.Subtract(new TimeSpan(5, 0, 0, 0));

                tFrmGLBatchTester.ttpgBatches.dtpDetailDateEffective.Properties.Text =
                    dateConverter.GetDateString(dteToUse);
                tFrmGLBatchTester.ttpgBatches.dtpDetailDateEffective.FireEvent("Validating");

                tFrmGLBatchTester.tbbSave.Click();
            }
            else
            {
                Assert.AreEqual(1, tFrmGLBatchTester.ttpgBatches.grdDetails.Properties.Rows.Count,
                    "TODO: Insert Code for this case!");
            }

            tFrmGLBatchTester.tFrmGLBatch.Close();
        }

        [Test]
        public void Test_02_AddJournals()
        {
            TFrmGLBatchTester tFrmGLBatchTester = new TFrmGLBatchTester();

            tFrmGLBatchTester.tFrmGLBatch.Show();
            tFrmGLBatchTester.tFrmGLBatch.LedgerNumber = fLedgerNumber;
            tFrmGLBatchTester.ttpgJournals.SelectThisTab();

            CheckCurrencySetting(tFrmGLBatchTester.ttpgJournals.txtDebit.Properties.Text);
            CheckCurrencySetting(tFrmGLBatchTester.ttpgJournals.txtCredit.Properties.Text);
            CheckCurrencySetting(tFrmGLBatchTester.ttpgJournals.txtControl.Properties.Text);

            CheckControlStatusEnabled("btnCancel", false,
                tFrmGLBatchTester.ttpgJournals.btnCancel.Properties.Enabled);
            CheckControlStatusEnabled("btnNew", true,
                tFrmGLBatchTester.ttpgJournals.btnAdd.Properties.Enabled);

            tFrmGLBatchTester.ttpgJournals.btnAdd.Click();
            tFrmGLBatchTester.ttpgJournals.txtDetailJournalDescription.Properties.Text =
                "Auto generated Journal 1";

            tFrmGLBatchTester.ttpgJournals.btnAdd.Click();
            tFrmGLBatchTester.ttpgJournals.txtDetailJournalDescription.Properties.Text =
                "Auto generated Journal 2";
            tFrmGLBatchTester.ttpgJournals.cmbDetailTransactionCurrency.Properties.SetSelectedString("GTQ");
            tFrmGLBatchTester.ttpgJournals.txtDetailExchangeRateToBase.Properties.Text = "2.5";
            tFrmGLBatchTester.tbbSave.Click();
        }

        [Test]
        public void Test_03_CheckJournals()
        {
            TFrmGLBatchTester tFrmGLBatchTester = new TFrmGLBatchTester();

            tFrmGLBatchTester.tFrmGLBatch.Show();
            tFrmGLBatchTester.tFrmGLBatch.LedgerNumber = fLedgerNumber;

            tFrmGLBatchTester.ttpgBatches.grdDetails.Properties.i

            tFrmGLBatchTester.ttpgJournals.SelectThisTab();
        }

        [Test]
        public void Test_102_AddJournals()
        {
            Assert.AreEqual(true, true, "...");
        }

        [TestFixtureSetUp]
        public void Init()
        {
            new TLogging("PetraClient.log");
            TPetraConnector.Connect("../../../../../etc/TestClient.config");
            fLedgerNumber = Convert.ToInt32(TAppSettingsManager.GetValueStatic("LedgerNumber"));
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
            TPetraConnector.Disconnect();
        }

//            System.Console.WriteLine(tFrmGLBatchTester.tabGLBatch.Properties.SelectedTab.Name );
//
//			System.Console.WriteLine(tFrmGLBatchTester.ttpgBatches.message());
//			System.Console.WriteLine(tFrmGLBatchTester.ttpgBatches.btnNew.Properties.Name);
//			// tFrmGLBatchTester.ttpgBatches.btnNew.Click();
//			tFrmGLBatchTester.ttpgBatches.rbtPosting.Click();
//			tFrmGLBatchTester.ttpgJournals.btnCancel.Click();
//			tFrmGLBatchTester.ttpgBatches.btnCancel.Click();
//			System.Console.WriteLine(tFrmGLBatchTester.ttpgBatches.lblValidDateRange.Properties.Text);
//
//			System.Console.WriteLine(tFrmGLBatchTester.ttpgBatches.grdDetails.Properties.Columns.Count.ToString());


//			tFrmGLBatchTester.tFrmGLBatch.LedgerNumber = fLedgerNumber;
//			tFrmGLBatchTester.tFrmGLBatch.Show();
//
//			// Assert.AreEqual(true, tFrmGLBatchTester.Properties.Visible, "....");
//
//			Assert.AreEqual(true, tFrmGLBatchTester.tbbSave.Properties.Enabled, "xxx");
//			tFrmGLBatchTester.tbbSave.Click();
//
//			tFrmGLBatchTester.tUC_GLBatchesTester.btnNew1.Click();
//
//			tFrmGLBatchTester.tUC_GLTransactionsTester.btnNew2.Click();
//
//			// tFrmGLBatchTester.tUC_GLTransactions_Tester.btnNew.Click();
    }
}