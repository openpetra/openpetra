//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Matthiash, timop
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
//
using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using Ict.Common;
using Ict.Common.IO;

using Ict.Petra.Client.CommonForms;
using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;
using Ict.Testing.NUnitTools;
using NUnit.Extensions.Forms;
using NUnit.Framework;
using Ict.Petra.Client.MFinance.Gui.Gift;

namespace Tests.MFinance.Client.Gift
{
    /// Testing the GL Batches screen
    [TestFixture]
    public class TGiftBatchesTest : NUnitFormTest
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// test for gift batch import
        /// </summary>
        public override void Setup()
        {
            new TLogging("../../log/TestClient.log");

            TPetraConnector.Connect("../../etc/TestClient.config");
            FLedgerNumber = Convert.ToInt32(TAppSettingsManager.GetValue("LedgerNumber", "43"));
        }

        /// <summary>
        /// clean up, disconnect from OpenPetra server
        /// </summary>
        public override void TearDown()
        {
            TPetraConnector.Disconnect();
        }

//        /// <summary>
//        /// simple test to create a batch and save it
//        /// </summary>
//        [Test]
//        public void TestCreateBatchAndSave()
//        {
//            TFrmGiftBatch frmBatch = new TFrmGiftBatch(IntPtr.Zero);
//
//            frmBatch.LedgerNumber = FLedgerNumber;
//            frmBatch.Show();
//
//            ToolStripButtonTester btnSave = new ToolStripButtonTester("tbbSave");
//            ButtonTester btnNewBatch = new ButtonTester("ucoBatches.btnNew");
//
//            Assert.AreEqual(false, btnSave.Properties.Enabled, "Save button should be disabled since there are no changes");
//            btnNewBatch.Click();
//
//            TextBoxTester txtDetailBatchDescription = new TextBoxTester("txtDetailBatchDescription");
//            txtDetailBatchDescription.Properties.Text = "Created by test TestCreateBatchAndSave";
//
//            Assert.AreEqual(true, btnSave.Properties.Enabled, "Save button should be enabled since there was a change");
//            btnSave.Click();
//        }

        private void ImportGiftBatch(string TestFile)
        {
            TestFile = Path.GetFullPath(TestFile);
            Assert.IsTrue(File.Exists(TestFile), "File does not exist: " + TestFile);

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
                    ComboBoxTester cmbNumberFormat = new ComboBoxTester("cmbNumberFormat");
                    cmbNumberFormat.Select(0);
                    ButtonTester btnOK = new ButtonTester("btnOK", tester2.Properties.Name);
                    ModalFormHandler = delegate(string name3, IntPtr hWnd3, Form form3)
                    {
                        MessageBoxTester tester3 = new MessageBoxTester(hWnd3);
                        Assert.AreEqual("Success", tester3.Title);
                        tester3.SendCommand(MessageBoxTester.Command.OK);
                    };
                    btnOK.Click();
                };
                tester.OpenFile(TestFile);
            };

            ToolStripButtonTester btnImport = new ToolStripButtonTester("tbbImportBatches");

            btnImport.Click();
        }

        /// <summary>
        /// test the import and export of gift batches
        /// </summary>
        [Test]
        public void TestImportExportGiftBatch()
        {
            // create two test batches, with some strange figures, to test problem with double values
            // TODO export the 2 test batches, with summarize option
            // TODO compare the exported text file

            string TestFile = CommonNUnitFunctions.rootPath + "/csharp/ICT/Testing/lib/MFinance/GiftForm/TestData/BatchImportTest.csv";

            TFrmGiftBatch frmBatch = new TFrmGiftBatch(null);

            frmBatch.LedgerNumber = FLedgerNumber;
            frmBatch.Show();

            ImportGiftBatch(TestFile);

            TabControlTester tabGiftBatch = new TabControlTester("tabGiftBatch");
            tabGiftBatch.SelectTab(1);
            TextBoxTester txtDetailGiftTransactionAmount = new TextBoxTester("txtDetailGiftTransactionAmount");
            Assert.AreEqual(Convert.ToDecimal(txtDetailGiftTransactionAmount.Properties.Text), 10000000000M);

            frmBatch.Close();
        }

        /// <summary>
        /// test a problem with saving after you have posted a batch
        /// </summary>
        [Test]
        public void TestPostAndSaveAfterwards()
        {
            string TestFile = CommonNUnitFunctions.rootPath + "/csharp/ICT/Testing/lib/MFinance/GiftForm/TestData/BatchImportTest.csv";

            TFrmGiftBatch frmBatch = new TFrmGiftBatch(null);

            frmBatch.LedgerNumber = FLedgerNumber;
            frmBatch.Show();

            ImportGiftBatch(TestFile);

            TabControlTester tabGiftBatch = new TabControlTester("tabGiftBatch");
            tabGiftBatch.SelectTab(1);
            TextBoxTester txtDetailGiftTransactionAmount = new TextBoxTester("txtDetailGiftTransactionAmount");
            Assert.AreEqual(Convert.ToDecimal(txtDetailGiftTransactionAmount.Properties.Text), 10000000000M);

            frmBatch.Close();
        }
    }
}