//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Matthiash
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
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using Ict.Common;
using Ict.Common.IO;

using Ict.Petra.Client.CommonForms;
using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;
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
        /// Attention! This resets your database
        /// there is only one test possible in this class (perhaps nunit parallelizes the tests?)
        /// </summary>
        public override void Setup()
        {
            new TLogging("TestClient.log");
            nant("stopPetraServer", false);
            nant("loadDatabase -D:LoadDB.file=csharp\\ICT\\Testing\\lib\\MFinance\\GiftForm\\TestData\\withpartners.sql", true);
            nant("startPetraServer", true);

            TPetraConnector.Connect("../../../../../etc/TestClient.config");
            FLedgerNumber = Convert.ToInt32(TAppSettingsManager.GetValueStatic("LedgerNumber"));
        }

        void nant(String argument, bool ignoreError)
        {
            Process NantProcess = new Process();

            NantProcess.EnableRaisingEvents = false;
            NantProcess.StartInfo.FileName = "cmd"; //if you have trouble and want to check the dos box try with cmd and /k nant xxx as arguments
            NantProcess.StartInfo.Arguments = "/c nant " + argument;
            NantProcess.StartInfo.CreateNoWindow = false;
            NantProcess.StartInfo.WorkingDirectory = "../../../../..";
            NantProcess.StartInfo.UseShellExecute = true;
            NantProcess.EnableRaisingEvents = true;
            NantProcess.StartInfo.ErrorDialog = true;

            if (!NantProcess.Start())
            {
                Debug.Print("failed to start " + NantProcess.StartInfo.FileName);
            }
            else
            {
                NantProcess.WaitForExit(60000);
                Debug.Print("OS says nant process ist finished");
            }
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

        /// <summary>
        /// test the import and export of gift batches
        /// </summary>
        [Test]
        public void TestImportExportGiftBatch()
        {
            // create two test batches, with some strange figures, to test problem with double values
            // export the 2 test batches, with summarize option
            // compare the exported text file

            string TestFile = TAppSettingsManager.GetValueStatic("Testing.Path") + "/MFinance/GiftForm/TestData/BatchImportTest.csv";

            TestFile = Path.GetFullPath(TestFile);
            Assert.IsTrue(File.Exists(TestFile), "File does not exist: " + TestFile);
            TFrmGiftBatch frmBatch = new TFrmGiftBatch(IntPtr.Zero);

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
            TabControlTester tabGiftBatch = new TabControlTester("tabGiftBatch");
            tabGiftBatch.SelectTab(1);
            TextBoxTester txtDetailGiftTransactionAmount = new TextBoxTester("txtDetailGiftTransactionAmount");
            Assert.AreEqual(Convert.ToDecimal(txtDetailGiftTransactionAmount.Properties.Text), 10000000000M);

            frmBatch.Close();
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