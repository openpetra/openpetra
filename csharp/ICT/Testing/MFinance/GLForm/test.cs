//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
//
using System;
using System.Data;
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Extensions.Forms;
using System.IO;
using System.Windows.Forms;
using Ict.Common;
using Ict.Testing.NUnitPetraClient;
using Ict.Petra.Client.MFinance.Gui.GL;

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
            new TLogging("TestClient.log");
            TPetraConnector.Connect("../../../../../etc/TestClient.config");
            FLedgerNumber = Convert.ToInt32(TAppSettingsManager.GetValueStatic("LedgerNumber"));
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
            TFrmGLBatch frmBatch = new TFrmGLBatch(IntPtr.Zero);

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
        }

        /// <summary>
        /// test for bug http://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=121
        /// </summary>
        [Test]
        public void TestCancelBatchBug121()
        {
            TFrmGLBatch frmBatch = new TFrmGLBatch(IntPtr.Zero);

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
                Assert.AreEqual("Confirm Cancel", tester.Title);

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
        }

        /// <summary>
        /// test for creating and posting batch
        /// </summary>
        [Test]
        public void TestCreateBatchAndPost()
        {
            TFrmGLBatch frmBatch = new TFrmGLBatch(IntPtr.Zero);

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

            TextBoxTester txtDebitAmount = new TextBoxTester("txtDebitAmount");
            decimal Amount = 1111.44M;
            txtDebitAmount.Properties.Text = Amount.ToString();

            TCmbAutoPopulatedTester cmbDetailAccountCode = new TCmbAutoPopulatedTester("cmbDetailAccountCode");
            cmbDetailAccountCode.Properties.SetSelectedString("6000");

            TCmbAutoPopulatedTester cmbDetailCostCentreCode = new TCmbAutoPopulatedTester("cmbDetailCostCentreCode");
            cmbDetailCostCentreCode.Properties.SetSelectedString(FLedgerNumber.ToString("00") + "00");

            btnNewTransaction.Click();
            txtDetailNarrative.Properties.Text = "test";
            txtDetailReference.Properties.Text = "test";
            TextBoxTester txtCreditAmount = new TextBoxTester("txtCreditAmount");
            txtCreditAmount.Properties.Text = Amount.ToString();

            cmbDetailAccountCode.Properties.SetSelectedString("0200");
            cmbDetailCostCentreCode.Properties.SetSelectedString(FLedgerNumber.ToString("00") + "00");

            ToolStripButtonTester btnSave = new ToolStripButtonTester("tbbSave");
            btnSave.Click();

            // post this batch
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                Assert.IsTrue(tester.Text.StartsWith(
                        "Are you sure you want to post batch"),
                    "Should start with 'are you sure you want to post batch', but is '" +
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

            ToolStripButtonTester btnPost = new ToolStripButtonTester("tbbPostBatch");
            btnPost.Click();
        }

        /// <summary>
        /// test the import and export of gl batches
        /// </summary>
        [Test]
        public void TestImportExportGLBatch()
        {
            // create two test batches, with some strange figures, to test problem with double values
            // export the 2 test batches, with summarize option
            // compare the exported text file

            TFrmGLBatch frmBatch = new TFrmGLBatch(IntPtr.Zero);

            frmBatch.LedgerNumber = FLedgerNumber;
            frmBatch.Show();

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                OpenFileDialogTester tester = new OpenFileDialogTester(hWnd);

                ModalFormHandler = delegate(string name2, IntPtr hWnd2, Form form2)
                {
                    TDlgSelectCSVSeparatorTester tester2 = new TDlgSelectCSVSeparatorTester(hWnd2);
                    TextBoxTester txtDateFormat = new TextBoxTester("txtDateFormat");
                    txtDateFormat.Properties.Text = "MM.dd.yyyy";
                    RadioButtonTester rbtSemicolon = new RadioButtonTester("rbtSemicolon");
                    rbtSemicolon.Properties.Checked = true;

                    ButtonTester btnOK = new ButtonTester("btnOK", tester2.Properties.Name);
                    btnOK.Click();
                };

                string p = Path.GetFullPath(Directory.GetCurrentDirectory() + "/../../MFinance/GLForm/TestData/BatchImportFloatTest.csv");
                Assert.IsTrue(File.Exists(p), "File does not exist: " + p);
                tester.OpenFile(p);
            };

            ToolStripButtonTester btnImport = new ToolStripButtonTester("tbbImportBatches");
            btnImport.Click();

            ToolStripButtonTester btnSave = new ToolStripButtonTester("tbbSave");
            Assert.IsTrue(btnSave.Properties.Enabled, "Save button has not been activated");
            btnSave.Click();

            // TODO: select the new batch, get the batch number from the journal tab
            // export that batch, summarize the transactions
            // compare the result with the expected file

            //TSgrdDataGridPagedTester grdBatches = new TSgrdDataGridPagedTester("ucoBatches.grdDetails");
        }
    }
}