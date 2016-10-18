//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
//
// Copyright 2004-2014 by OM International
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
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace Tests.MFinance.GLBatches
{
    /// Testing the GL Batches screen
    [TestFixture]
    public class TGLBatchesTest : NUnitFormTest
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// start the gui program
        /// </summary>
        public override void Setup()
        {
            // Before Execution of any Test we should do something like
            // nant stopPetraServer
            // nant ResetDatabase
            // nant startPetraServer
            // this may take some time ....
            new TLogging("../../log/TestClient.log");
            TPetraConnector.Connect("../../etc/TestClient.config");
            FLedgerNumber = Convert.ToInt32(TAppSettingsManager.GetValue("LedgerNumber"));
        }

        /// <summary>
        /// clean up, disconnect from OpenPetra server
        /// </summary>
        public override void TearDown()
        {
            TPetraConnector.Disconnect();
        }

        /// <summary>
        /// simple test to create a batch and save it
        /// </summary>
        [Test]
        public void TestCreateBatchAndSave()
        {
            TFrmGLBatch frmBatch = new TFrmGLBatch(null);

            frmBatch.LedgerNumber = FLedgerNumber;
            frmBatch.Show();

            ToolStripButtonTester btnSave = new ToolStripButtonTester("tbbSave");
            ButtonTester btnNewBatch = new ButtonTester("ucoBatches.btnNew");

            Assert.AreEqual(false, btnSave.Properties.Enabled, "Save button should be disabled since there are no changes");
            btnNewBatch.Click();

            TextBoxTester txtDetailBatchDescription = new TextBoxTester("txtDetailBatchDescription");
            txtDetailBatchDescription.Properties.Text = "Created by test TestCreateBatchAndSave";

            Assert.AreEqual(true, btnSave.Properties.Enabled, "Save button should be enabled since there was a change");
            btnSave.Click();

            frmBatch.Close();
        }

        /// <summary>
        /// test for bug http://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=121
        /// </summary>
        [Test]
        public void TestCancelBatchBug121()
        {
            TFrmGLBatch frmBatch = new TFrmGLBatch(null);

            frmBatch.LedgerNumber = FLedgerNumber;
            frmBatch.Show();

            // create a new batch and save
            ToolStripButtonTester btnSave = new ToolStripButtonTester("tbbSave");
            ButtonTester btnNewBatch = new ButtonTester("ucoBatches.btnNew");
            btnNewBatch.Click();
            TextBoxTester txtDetailBatchDescription = new TextBoxTester("txtDetailBatchDescription");
            txtDetailBatchDescription.Properties.Text = "Created by test TestCancelBatchBug121";
            btnSave.Click();

            // cancel that batch. no saving necessary
            ButtonTester btnCancelBatch = new ButtonTester("ucoBatches.btnCancel");
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                Assert.AreEqual("Form Contains Invalid Data", tester.Title);

                // there is a second message box after confirming the cancellation, telling the user the cancellation was successful.
                // because the ModalFormHandler is reset after handling the first message box, we need to set up a new handler.
                ModalFormHandler = delegate(string name2, IntPtr hWnd2, Form form2)
                {
                    MessageBoxTester tester2 = new MessageBoxTester(hWnd2);
                    // Assert.AreEqual("Success", tester.Title);
                    tester2.SendCommand(MessageBoxTester.Command.Yes);
                };

                tester.SendCommand(MessageBoxTester.Command.Yes);
            };

            btnCancelBatch.Click();

            // add a new batch
            btnNewBatch.Click();
            txtDetailBatchDescription.Properties.Text = "Created by test TestCancelBatchBug121, not cancelled";

            // save: the bug caused exception "Forgot to call AcceptChanges"
            btnSave.Click();

            Assert.AreEqual(false, btnSave.Properties.Enabled, "Save button should be disabled because all changes have been saved");

            frmBatch.Close();
        }

        /// <summary>
        /// test for creating and posting batch
        /// </summary>
        [Test]
        public void TestCreateBatchAndPost()
        {
            TFrmGLBatch frmBatch = new TFrmGLBatch(null);

            frmBatch.LedgerNumber = FLedgerNumber;
            frmBatch.Show();

            // create a new batch and save
            ButtonTester btnNewBatch = new ButtonTester("ucoBatches.btnNew");
            btnNewBatch.Click();
            TextBoxTester txtDetailBatchDescription = new TextBoxTester("txtDetailBatchDescription");
            txtDetailBatchDescription.Properties.Text = "Created by test TestExportGLBatch";

            TabControlTester tabGLBatch = new TabControlTester("tabGLBatch");

            // go to Journal tab
            tabGLBatch.SelectTab(1);

            ButtonTester btnNewJournal = new ButtonTester("ucoJournals.btnAdd");
            btnNewJournal.Click();

            // go to transaction tab
            tabGLBatch.SelectTab(2);

            ButtonTester btnNewTransaction = new ButtonTester("ucoTransactions.btnNew");
            btnNewTransaction.Click();

            TextBoxTester txtDetailNarrative = new TextBoxTester("txtDetailNarrative");
            txtDetailNarrative.Properties.Text = "test";
            TextBoxTester txtDetailReference = new TextBoxTester("txtDetailReference");
            txtDetailReference.Properties.Text = "test";

            TTxtCurrencyTextBoxTester txtDebitAmount = new TTxtCurrencyTextBoxTester("txtDebitAmount");
            txtDebitAmount.Properties.Focus();
            decimal Amount = 1111.44M;
            txtDebitAmount.Properties.NumberValueDecimal = Amount;

            TCmbAutoPopulatedTester cmbDetailAccountCode = new TCmbAutoPopulatedTester("cmbDetailAccountCode");
            cmbDetailAccountCode.Properties.SetSelectedString("6000");

            TCmbAutoPopulatedTester cmbDetailCostCentreCode = new TCmbAutoPopulatedTester("cmbDetailCostCentreCode");
            cmbDetailCostCentreCode.Properties.SetSelectedString(FLedgerNumber.ToString("00") + "00");

            btnNewTransaction.Click();
            txtDetailNarrative.Properties.Text = "test";
            txtDetailReference.Properties.Text = "test";
            TTxtCurrencyTextBoxTester txtCreditAmount = new TTxtCurrencyTextBoxTester("txtCreditAmount");
            txtDebitAmount.Properties.NumberValueDecimal = 0;
            txtCreditAmount.Properties.Focus();
            txtCreditAmount.Properties.NumberValueDecimal = Amount;

            cmbDetailAccountCode.Properties.Focus();        // This will update the totals
            cmbDetailAccountCode.Properties.SetSelectedString("0200");
            cmbDetailCostCentreCode.Properties.SetSelectedString(FLedgerNumber.ToString("00") + "00");

            ToolStripButtonTester btnSave = new ToolStripButtonTester("tbbSave");
            btnSave.Click();

            // go to Batch tab
            tabGLBatch.SelectTab(0);

            // post this batch
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                Assert.IsTrue(tester.Text.StartsWith(
                        "Are you sure you want to post GL batch"),
                    "Should start with 'are you sure you want to post GL batch', but is '" +
                    tester.Text + "'");

                // there is a second message box after posting, telling the user about success.
                // because the ModalFormHandler is reset after handling the first message box, we need to set up a new handler.
                ModalFormHandler = delegate(string name2, IntPtr hWnd2, Form form2)
                {
                    MessageBoxTester tester2 = new MessageBoxTester(hWnd2);
                    Assert.AreEqual("Progress Dialog", tester2.Title);

                    //Wait for it to close
                    Thread.Sleep(1000);

                    // there is a second message box after posting, telling the user about success.
                    // because the ModalFormHandler is reset after handling the first message box, we need to set up a new handler.
                    ModalFormHandler = delegate(string name3, IntPtr hWnd3, Form form3)
                    {
                        MessageBoxTester tester3 = new MessageBoxTester(hWnd3);
                        Assert.AreEqual("Success", tester3.Title);

                        tester3.SendCommand(MessageBoxTester.Command.Yes);
                    };
                };

                tester.SendCommand(MessageBoxTester.Command.Yes);
            };

            frmBatch.EnablePostingReport = false;
            ButtonTester btnPost = new ButtonTester("ucoBatches.btnPostBatch");
            btnPost.Click();

            // and now try to create a new batch, bug https://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=1058
            btnNewBatch.Click();
            btnSave.Click();

            frmBatch.Close();
        }

        /// <summary>
        /// test the import and export of gl batches
        /// </summary>
        [Test]
        [Ignore("TODO this NUnit test needs to be fixed")]
        public void TestImportExportGLBatch()
        {
            // create two test batches, with some strange figures, to test problem with double values
            // export the 2 test batches, with summarize option
            // compare the exported text file

            string TestFile = TAppSettingsManager.GetValue("Testing.Path") + "/lib/MFinance/GLForm/TestData/BatchImportFloatTest.csv";

            TestFile = Path.GetFullPath(TestFile);
            Assert.IsTrue(File.Exists(TestFile), "File does not exist: " + TestFile);

            TFrmGLBatch frmBatch = new TFrmGLBatch(null);

            frmBatch.LedgerNumber = FLedgerNumber;
            frmBatch.Show();

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                OpenFileDialogTester tester = new OpenFileDialogTester(hWnd);

                ModalFormHandler = delegate(string name2, IntPtr hWnd2, Form form2)
                {
                    TDlgSelectCSVSeparatorTester tester2 = new TDlgSelectCSVSeparatorTester(hWnd2);
                    TextBoxTester txtDateFormat = new TextBoxTester("txtDateFormat");
                    txtDateFormat.Properties.Text = "MM/dd/yyyy";
                    RadioButtonTester rbtSemicolon = new RadioButtonTester("rbtSemicolon");
                    rbtSemicolon.Properties.Checked = true;

                    ButtonTester btnOK = new ButtonTester("btnOK", tester2.Properties.Name);
                    btnOK.Click();
                };

                tester.OpenFile(TestFile);
            };

            ToolStripButtonTester btnImport = new ToolStripButtonTester("tbbImportBatches");
            btnImport.Click();

            ToolStripButtonTester btnSave = new ToolStripButtonTester("tbbSave");
            Assert.IsTrue(btnSave.Properties.Enabled, "Save button has not been activated");
            btnSave.Click();

            // go to Journal tab
            TabControlTester tabGLBatch = new TabControlTester("tabGLBatch");
            tabGLBatch.SelectTab(1);
            TextBoxTester txtBatchNumber = new TextBoxTester("ucoJournals.txtBatchNumber");

            // get the batch number from the journal tab
            int ImportedBatchNumber = Convert.ToInt32(txtBatchNumber.Properties.Text);

            TFrmGLBatchExport frmBatchExport = new TFrmGLBatchExport(null);

            frmBatch.Close();

            // export that batch, summarize the transactions
            // compare the result with the expected file
            frmBatchExport.LedgerNumber = FLedgerNumber;
            frmBatchExport.Show();

            CheckBoxTester chkIncludeUnposted = new CheckBoxTester("chkIncludeUnposted");
            chkIncludeUnposted.Properties.Checked = true;

            RadioButtonTester rbtSummary = new RadioButtonTester("rbtSummary");
            rbtSummary.Properties.Checked = false;

            RadioButtonTester rbtBatchNumberSelection = new RadioButtonTester("rbtBatchNumberSelection");
            rbtBatchNumberSelection.Properties.Checked = true;

            TextBoxTester txtFilename = new TextBoxTester("txtFilename");

            TTxtNumericTextBoxTester txtBatchNumberStart = new TTxtNumericTextBoxTester("txtBatchNumberStart");
            txtBatchNumberStart.Properties.NumberValueInt = ImportedBatchNumber;
            TTxtNumericTextBoxTester txtBatchNumberEnd = new TTxtNumericTextBoxTester("txtBatchNumberEnd");
            txtBatchNumberEnd.Properties.NumberValueInt = ImportedBatchNumber;

            // Test simple export of batches, no summary
            TestFile = TAppSettingsManager.GetValue("Testing.Path") + "/MFinance/GLForm/TestData/BatchExportFloatTest.csv";
            TestFile = Path.GetFullPath(TestFile);
            Assert.IsTrue(File.Exists(TestFile), "File does not exist: " + TestFile);
            txtFilename.Properties.Text = TestFile + ".new";

            ModalFormHandler = delegate(string name2, IntPtr hWnd2, Form form2)
            {
                MessageBoxTester tester2 = new MessageBoxTester(hWnd2);
                // Assert.AreEqual("Success", tester.Title);
                tester2.SendCommand(MessageBoxTester.Command.OK);
            };

            frmBatchExport.ExportBatches(false);

            Assert.AreEqual(true, TTextFile.SameContent(TestFile,
                    TestFile + ".new"), "the files should be the same: " + TestFile);
            System.IO.File.Delete(TestFile + ".new");

            // Test export of batches, summarizing the transactions
            TestFile = TAppSettingsManager.GetValue("Testing.Path") + "/MFinance/GLForm/TestData/BatchExportFloatTestSummary.csv";
            TestFile = Path.GetFullPath(TestFile);
            Assert.IsTrue(File.Exists(TestFile), "File does not exist: " + TestFile);
            txtFilename.Properties.Text = TestFile + ".new";
            rbtSummary.Properties.Checked = true;

            ModalFormHandler = delegate(string name2, IntPtr hWnd2, Form form2)
            {
                MessageBoxTester tester2 = new MessageBoxTester(hWnd2);
                // Assert.AreEqual("Success", tester.Title);
                tester2.SendCommand(MessageBoxTester.Command.OK);
            };

            frmBatchExport.ExportBatches(false);

            Assert.AreEqual(true, TTextFile.SameContent(TestFile,
                    TestFile + ".new"), "the files should be the same: " + TestFile);
            System.IO.File.Delete(TestFile + ".new");

            frmBatchExport.Close();
        }

        /// <summary>
        /// test the import of gl batches
        /// </summary>
        [Test]
        public void TestImportGLBatch()
        {
            int NumberOfBatches = 0;

            string TestFile = TAppSettingsManager.GetValue("Testing.Path") + "/MFinance/GLForm/TestData/BatchImportFloatTest.csv";

            TestFile = Path.GetFullPath(TestFile);
            Assert.IsTrue(File.Exists(TestFile), "File does not exist: " + TestFile);

            TFrmGLBatch frmBatch = new TFrmGLBatch(null);
            TFrmGLBatch frmBatch1 = new TFrmGLBatch(null);

            //Open the batch form and count no. of batches
            frmBatch.LedgerNumber = FLedgerNumber;
            frmBatch.Show();
            TSgrdDataGridPagedTester grdDetails = new TSgrdDataGridPagedTester("grdDetails");
            NumberOfBatches = grdDetails.Count - 1;
            TLogging.Log("NumberOfBatches: " + NumberOfBatches.ToString());

            //Close the form
            frmBatch.Close();

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                OpenFileDialogTester tester = new OpenFileDialogTester(hWnd);

                ModalFormHandler = delegate(string name2, IntPtr hWnd2, Form form2)
                {
                    TDlgSelectCSVSeparatorTester tester2 = new TDlgSelectCSVSeparatorTester(hWnd2);
                    TextBoxTester txtDateFormat = new TextBoxTester("txtDateFormat");
                    txtDateFormat.Properties.Text = "MM/dd/yyyy";
                    RadioButtonTester rbtSemicolon = new RadioButtonTester("rbtSemicolon");
                    rbtSemicolon.Properties.Checked = true;

                    ButtonTester btnOK = new ButtonTester("btnOK", tester2.Properties.Name);
                    btnOK.Click();
                };

                tester.OpenFile(TestFile);
            };

            //Set the batch form to open with importing batches dialog
            frmBatch1.LoadForImport = true;
            frmBatch1.LedgerNumber = FLedgerNumber;
            frmBatch1.Show();

            TSgrdDataGridPagedTester grdDetails1 = new TSgrdDataGridPagedTester("grdDetails");
            TLogging.Log("grdDetails.Count after import: " + grdDetails1.Count.ToString());
            Assert.AreNotEqual(NumberOfBatches, grdDetails1.Count, "The grid should include imported batches");

            frmBatch1.Close();
        }

        /// <summary>
        /// test the import of gl transactions
        /// </summary>
        [Test]
        public void TestImportGLTransactions()
        {
            // create a test batch and journal and then import transactions

            string TestFile = TAppSettingsManager.GetValue("Testing.Path") + "/MFinance/GLForm/TestData/TransactionsImport.csv";

            TestFile = Path.GetFullPath(TestFile);
            Assert.IsTrue(File.Exists(TestFile), "File does not exist: " + TestFile);

            TFrmGLBatch frmBatch = new TFrmGLBatch(null);

            frmBatch.LedgerNumber = FLedgerNumber;
            frmBatch.Show();

            // create a new batch and save
            ButtonTester btnNewBatch = new ButtonTester("ucoBatches.btnNew");
            btnNewBatch.Click();
            TextBoxTester txtDetailBatchDescription = new TextBoxTester("txtDetailBatchDescription");
            txtDetailBatchDescription.Properties.Text = "Created by test TestImportGLTransactions";

            TabControlTester tabGLBatch = new TabControlTester("tabGLBatch");

            // go to Journal tab
            tabGLBatch.SelectTab(1);

            ButtonTester btnNewJournal = new ButtonTester("ucoJournals.btnAdd");
            btnNewJournal.Click();

            // go to transaction tab
            tabGLBatch.SelectTab(2);

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                OpenFileDialogTester tester = new OpenFileDialogTester(hWnd);

                ModalFormHandler = delegate(string name2, IntPtr hWnd2, Form form2)
                {
                    TDlgSelectCSVSeparatorTester tester2 = new TDlgSelectCSVSeparatorTester(hWnd2);
                    TextBoxTester txtDateFormat = new TextBoxTester("txtDateFormat");
                    txtDateFormat.Properties.Text = "dd/MM/yyyy";
                    RadioButtonTester rbtSemicolon = new RadioButtonTester("rbtSemicolon");
                    rbtSemicolon.Properties.Checked = true;

                    ButtonTester btnOK = new ButtonTester("btnOK", tester2.Properties.Name);
                    btnOK.Click();
                };

                tester.OpenFile(TestFile);
            };

            ToolStripButtonTester btnImport = new ToolStripButtonTester("tbbImportTransactions");
            btnImport.Click();

            TSgrdDataGridPagedTester grdDetails = new TSgrdDataGridPagedTester("grdDetails");
            Assert.AreNotEqual(1, grdDetails.Count, "The grid should be populated");
        }

        /// <summary>
        /// simple test to view the transactions of a posted batch and then add a new batch
        /// </summary>
        [Test]
        [Ignore("TODO fix this test: cannot find rbtPosting")]
        public void TestViewPostedBatchTransactionsAndAddBatch()
        {
            //This test adds a new batch, saves and posts it, then views it and then tries to add a new batch

            TFrmGLBatch frmBatch = new TFrmGLBatch(null);

            frmBatch.LedgerNumber = FLedgerNumber;
            frmBatch.Show();

            // create a new batch and save
            ButtonTester btnNewBatch = new ButtonTester("ucoBatches.btnNew");
            btnNewBatch.Click();
            TextBoxTester txtDetailBatchDescription = new TextBoxTester("txtDetailBatchDescription");
            txtDetailBatchDescription.Properties.Text = "Created by test TestExportGLBatch";

            TabControlTester tabGLBatch = new TabControlTester("tabGLBatch");

            // go to Journal tab
            tabGLBatch.SelectTab(1);

            ButtonTester btnNewJournal = new ButtonTester("ucoJournals.btnAdd");
            btnNewJournal.Click();

            // go to transaction tab
            tabGLBatch.SelectTab(2);

            ButtonTester btnNewTransaction = new ButtonTester("ucoTransactions.btnNew");
            btnNewTransaction.Click();

            TextBoxTester txtDetailNarrative = new TextBoxTester("txtDetailNarrative");
            txtDetailNarrative.Properties.Text = "test";
            TextBoxTester txtDetailReference = new TextBoxTester("txtDetailReference");
            txtDetailReference.Properties.Text = "test";

            TTxtCurrencyTextBoxTester txtDebitAmount = new TTxtCurrencyTextBoxTester("txtDebitAmount");
            decimal Amount = 1111.44M;
            txtDebitAmount.Properties.NumberValueDecimal = Amount;

            TCmbAutoPopulatedTester cmbDetailAccountCode = new TCmbAutoPopulatedTester("cmbDetailAccountCode");
            cmbDetailAccountCode.Properties.SetSelectedString("6000");

            TCmbAutoPopulatedTester cmbDetailCostCentreCode = new TCmbAutoPopulatedTester("cmbDetailCostCentreCode");
            cmbDetailCostCentreCode.Properties.SetSelectedString(FLedgerNumber.ToString("00") + "00");

            btnNewTransaction.Click();
            txtDetailNarrative.Properties.Text = "test";
            txtDetailReference.Properties.Text = "test";
            TTxtCurrencyTextBoxTester txtCreditAmount = new TTxtCurrencyTextBoxTester("txtCreditAmount");
            txtDebitAmount.Properties.NumberValueDecimal = 0;
            txtCreditAmount.Properties.Focus();
            txtCreditAmount.Properties.NumberValueDecimal = Amount;

            cmbDetailAccountCode.Properties.SetSelectedString("0200");
            cmbDetailCostCentreCode.Properties.SetSelectedString(FLedgerNumber.ToString("00") + "00");

            //ToolStripButtonTester btnSave = new ToolStripButtonTester("tbbSave");
            //btnSave.Click();

            // post this batch
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                Assert.IsTrue(tester.Text.StartsWith(
                        "Are you sure you want to post GL batch"),
                    "Should start with 'are you sure you want to post GL batch', but is '" +
                    tester.Text + "'");

                // there is a second message box after posting, telling the user about success.
                // because the ModalFormHandler is reset after handling the first message box, we need to set up a new handler.
                ModalFormHandler = delegate(string name2, IntPtr hWnd2, Form form2)
                {
                    MessageBoxTester tester2 = new MessageBoxTester(hWnd2);
                    Assert.AreEqual("Success", tester2.Title);
                    tester2.SendCommand(MessageBoxTester.Command.Yes);
                };

                tester.SendCommand(MessageBoxTester.Command.Yes);
            };

            // and now try to create a new batch, bug https://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=1058
            // go to Batch tab
            tabGLBatch.SelectTab(0);

            ButtonTester btnPostBatch = new ButtonTester("ucoBatches.btnPostBatch");
            btnPostBatch.Click();

            //Make sure the grid is clear
            // TODO NUnit.Extensions.Forms.NoSuchControlException : rbtPosting
            RadioButtonTester rbtPosting = new RadioButtonTester("rbtPosting");
            rbtPosting.Properties.Checked = true;

            //This will then select the first batch in the grid which needs to be posted
            RadioButtonTester rbtAll = new RadioButtonTester("rbtAll");
            rbtAll.Properties.Checked = true;

            //TabControlTester tabGLBatch = new TabControlTester("tabGLBatch");

            // go to Journal tab
            tabGLBatch.SelectTab(1);

            // go to Transaction Tab
            tabGLBatch.SelectTab(2);

            // go to Batch Tab
            tabGLBatch.SelectTab(0);

            //ButtonTester btnNewBatch = new ButtonTester("ucoBatches.btnNew");
            btnNewBatch.Click();
        }

        /// <summary>
        /// simple test to create a batch and save it
        /// </summary>
        [Test]
        [Ignore("TODO this NUnit test needs to be fixed: this.btnDelete")]
        public void TestAnalysisAttributes()
        {
            // At the moment the initial state is unknown so we make a relative test


            TFrmSetupAnalysisTypes frmAnalysistypes = new TFrmSetupAnalysisTypes(null);

            frmAnalysistypes.LedgerNumber = FLedgerNumber;
            frmAnalysistypes.Show();
            // Tests schould be repeateable but at the moment we make only a relative test

            // Press the new Button for the types
            String randomNewValueString = RandomString();
            ButtonTester btnNewType = new ButtonTester("btnNewType");
            btnNewType.Click();

            TextBoxTester txtDetailAnalysisTypeCode = new TextBoxTester("txtDetailAnalysisTypeCode");
            txtDetailAnalysisTypeCode.Properties.Text = randomNewValueString;
            TextBoxTester txtDetailAnalysisTypeDescription = new TextBoxTester("txtDetailAnalysisTypeDescription");
            txtDetailAnalysisTypeDescription.Properties.Text = "Description for " + randomNewValueString;

            // Press the new Button for the values
            ButtonTester btnNewValue = new ButtonTester("btnNew");
            btnNewValue.Click();

            TextBoxTester txtDetailAnalysisValue = new TextBoxTester("txtDetailAnalysisValue");
            txtDetailAnalysisValue.Properties.Text = randomNewValueString + "-1";
            btnNewValue.Click();

            txtDetailAnalysisValue.Properties.Text = randomNewValueString + "-2";
            CheckBoxTester chkDetailActive = new CheckBoxTester("chkDetailActive");
            Assert.IsTrue(chkDetailActive.Checked, "Active not set as default!");
            chkDetailActive.Properties.Checked = false;


            ToolStripButtonTester btnSave = new ToolStripButtonTester("tbbSave");
            // and save everything
            btnSave.Click();
            // Press the delete Button for the values
            ButtonTester btnDeleteValue = new ButtonTester("ucoValues.btnDelete");
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                Assert.AreEqual("Confirm Delete", tester.Title);
                tester.SendCommand(MessageBoxTester.Command.Yes);
            };
            btnDeleteValue.Click();

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                Assert.AreEqual("Confirm Delete", tester.Title);
                tester.SendCommand(MessageBoxTester.Command.Yes);
            };
            btnDeleteValue.Click();
            // Press the delete Button for the types
            // TODO: AmbigousNameException. Should we implement this.btnDelete in NUnitForms?
            ButtonTester btnDeleteType = new ButtonTester("btnDelete");
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                Assert.AreEqual("Confirm Delete", tester.Title);
                tester.SendCommand(MessageBoxTester.Command.Yes);
            };
            btnDeleteType.Click();
            btnSave.Click();
        }

        /// <summary>
        /// Generate a random string with 8 characters
        /// </summary>
        /// <returns>random string</returns>
        public String RandomString()
        {
            String s = "";
            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                s += Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            }

            return s;
        }
    }
}
