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
                }

                tester.SendCommand(MessageBoxTester.Command.Yes);
            }
            btnCancelBatch.Click();

            // add a new batch
            btnNewBatch.Click();
            txtDetailBatchDescription.Properties.Text = "Created by test TestCancelBatchBug121, not cancelled";

            // save: the bug caused exception "Forgot to call AcceptChanges"
            btnSave.Click();

            Assert.AreEqual(false, btnSave.Properties.Enabled, "Save button should be disabled because all changes have been saved");
        }
    }
}