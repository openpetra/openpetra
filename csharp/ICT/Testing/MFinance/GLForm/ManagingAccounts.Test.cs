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
//

using System;
using System.IO;
using System.Windows.Forms;

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
    /// <summary>
    /// Here the complete Management of the Accounts shall be tested ...
    /// </summary>
    [TestFixture]
    public class ManagingAccounts : NUnitFormTest
    {
        // Each account is defined by its LedgerNumber ...
        // Actually the value is read by the TAppSettingsManager
        private Int32 fLedgerNumber;


        private void WaitForMessageBox(NUnit.Extensions.Forms.MessageBoxTester.Command cmd)
        {
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);


                System.Console.WriteLine(tester.Title);
                System.Console.WriteLine(tester.Text);

                tester.SendCommand(cmd);
            };
        }

        [TestFixtureSetUp]
        public void Init()
        {
            // Before Execution of any Test we should do something like
            // nant stopPetraServer
            // nant ResetDatabase
            // nant startPetraServer
            // this may take some time ....
            new TLogging("PetraClient.log");

            TPetraConnector.Connect("../../../../../etc/TestClient.config");
            fLedgerNumber = Convert.ToInt32(TAppSettingsManager.GetValueStatic("LedgerNumber"));
        }

        /// <summary>
        /// clean up, disconnect from OpenPetra server
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            TPetraConnector.Disconnect();
        }

        /// <summary>
        /// This test runs in the very first step, because there is no account
        /// named "xxdd2". After this NANT will be shut down by executing the test.
        /// </summary>
        [Test]
        public void Test_01_CreateAccount()
        {
            TFrmGLAccountHierarchyTester hierarchyTester;

            hierarchyTester = new TFrmGLAccountHierarchyTester();
            String str;

            System.Console.WriteLine("-------Test_01_CreateLedger------------");

            TLogging.Log("CreateLedger");

            hierarchyTester.mainForm.Show();
            hierarchyTester.mainForm.LedgerNumber = fLedgerNumber;

            // Get a Node to operate ...
            int[] nodeList =
            {
                0, 2
            };
            //hierarchyTester.trvAccounts.SelectNode(nodeList);

            TreeNode node = hierarchyTester.trvAccounts.Properties.SelectedNode;
            str = node.GetNodeCount(false).ToString();

            System.Console.WriteLine("Anzahl Nodes: " + str);


            // WaitForMessageBox(MessageBoxTester.Command.Yes);
            hierarchyTester.tbbAddNewAccount.Click();

            System.Console.WriteLine("b");
            // hierarchyTester.tbbSave.Click();
            System.Console.WriteLine("c");
            hierarchyTester.txtDetailAccountCode.Properties.Focus();
            hierarchyTester.txtDetailAccountCode.Properties.Text = "xxdd2";
            Assert.AreEqual(true, true, "ddd");
            hierarchyTester.txtDetailAccountCodeLongDesc.Properties.Focus();
            Assert.AreEqual(true, true, "ddd");
            System.Console.WriteLine("d");

            str = hierarchyTester.txtDetailAccountCodeLongDesc.Properties.Text;

            // Assert.AreEqual(str,"xx","LostFocus");


            // System.Console.WriteLine(str);

            // System.Console.WriteLine(hierarchyTester.txtDetailAccountCode.Properties.Text);

            // node = hierarchyTester.trvAccounts.Properties.SelectedNode;
            // str = node.GetNodeCount(false).ToString();


            /// <summary>
            /// hier crasht es !!!
            /// </summary>
            ///
            throw new Exception();

            try
            {
                hierarchyTester.tbbSave.Click();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            System.Console.WriteLine("e");
            //hierarchyTester.mniClose.Click();
            System.Console.WriteLine("f");

            //hierarchyTester.mainForm.Close();

            System.Console.WriteLine("-------Test_01_CreateLedger-Bye------------");
        }

        [Test]
        public void Test_10_CreateLedger()
        {
            System.Console.WriteLine("-------Test_02_CreateLedger------------");


            TFrmGLCreateLedger tFrmGLCreateLedger = new TFrmGLCreateLedger(IntPtr.Zero);

            TextBoxTester ledgerName = new TextBoxTester("txtLedgerName", tFrmGLCreateLedger);
            // ComboBoxTester countryCode = new ComboBoxTester("cmbCountryCode",tFrmGLCreateLedger);

            // System.Windows.Forms

            TCmbAutoPopulatedTester cmbCountryCode = new TCmbAutoPopulatedTester("cmbCountryCode", tFrmGLCreateLedger);
            TCmbAutoPopulatedTester cmbBaseCurrency = new TCmbAutoPopulatedTester("cmbBaseCurrency", tFrmGLCreateLedger);
            TCmbAutoPopulatedTester cmbIntlCurrency = new TCmbAutoPopulatedTester("cmbIntlCurrency", tFrmGLCreateLedger);
            // countryCode.Properties.SelectedIndex = 3;

            TextBoxTester dtpCalendarStartDate = new TextBoxTester("dtpCalendarStartDate", tFrmGLCreateLedger);

            NumericUpDownTester nudLedgerNumber = new NumericUpDownTester("nudLedgerNumber", tFrmGLCreateLedger);
            NumericUpDownTester nudNumberOfPeriods = new NumericUpDownTester("nudNumberOfPeriods", tFrmGLCreateLedger);
            NumericUpDownTester nudCurrentPeriod = new NumericUpDownTester("nudCurrentPeriod", tFrmGLCreateLedger);
            NumericUpDownTester nudNumberOfFwdPostingPeriods = new NumericUpDownTester("nudNumberOfFwdPostingPeriods", tFrmGLCreateLedger);

            dtpCalendarStartDate.Properties.Text = "123";

            String str = nudLedgerNumber.Properties.Text;
            System.Console.WriteLine("nudLedgerNumber" + str);

            nudLedgerNumber.Properties.Text = "22";

            str = nudLedgerNumber.Properties.Text;
            System.Console.WriteLine("nudLedgerNumber" + str);

            // System.Console.WriteLine(countryCode.Properties.SelectedValue );
            // countryCode.Properties.SelectedItem = 12 ;
            // System.Console.WriteLine(countryCode.Properties.SelectedValue );
            // System.Console.WriteLine(countryCode.Properties.SelectedValue );
        }

        [Test]
        public void TestMethod()
        {
            TFrmGLAccountHierarchy tFrmMainWindow = new TFrmGLAccountHierarchy(IntPtr.Zero);

            tFrmMainWindow.LedgerNumber = fLedgerNumber;
            tFrmMainWindow.Show();


            TreeViewTester treeViewTester = new TreeViewTester("trvAccounts", tFrmMainWindow);


            System.Windows.Forms.TreeNode subTree;

            // subTree = treeViewTester.Properties.Nodes[1].NextNode;

            subTree = treeViewTester.Properties.SelectedNode;
            subTree.ExpandAll();

            for (int i = 0; i < subTree.Nodes.Count; i++)
            {
                System.Console.WriteLine(subTree.Nodes[i].Text);
            }

            System.Console.WriteLine("-------------------");

            subTree = subTree.Nodes[2];
            subTree.ExpandAll();

            for (int i = 0; i < subTree.Nodes.Count; i++)
            {
                System.Console.WriteLine(subTree.Nodes[i].Text);
            }

            System.Console.WriteLine("-------------------");

            treeViewTester.Properties.SelectedNode = subTree.Nodes[1];


            TreeView treeView = treeViewTester.Properties.SelectedNode.TreeView;

            int help = treeViewTester.Properties.GetNodeCount(true);
            System.Console.WriteLine(String.Concat("TVC: ", help));

            ToolStripMenuItemTester btnNewAccount =
                new ToolStripMenuItemTester("mniAddNewAccount");
            // btnNewAccount.Click();

            ToolStripMenuItemTester btnSave = new ToolStripMenuItemTester("mniFileSave");
            // btnSave.Click();

            System.Console.WriteLine("-------------------");

            TextBoxTester txtDetailAccountCode = new TextBoxTester("txtDetailAccountCode");
            String textDetail = txtDetailAccountCode.Text;

            System.Console.WriteLine(textDetail);


            // TableLayoutPanel

            // ToolStripPanel


            //Constraint constraint = TrueConstraint;


            //StringAssert.Contains(textDetail, "123", "Meldung");
            // Assert.AreEqual(2,2,"3");
        }

        [Test]
        public void TestLedgerNumber()
        {
            DelayedConstraint delayedConstraint = new DelayedConstraint(Is.True, 50000);

            Assert.That(delayedConstraint, Is.True);
            Assert.AreEqual(1, 2, "3");
            Assert.That(delayedConstraint, Is.True);
            Assert.AreEqual(1, 2, "3");
            Assert.AreEqual(fLedgerNumber, 43, "ln");
            System.Console.WriteLine("jaja");
        }
    }
}