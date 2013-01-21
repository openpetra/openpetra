//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
//
// Copyright 2004-2011 by OM International
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
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
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
    /// ...
    /// </summary>
    [TestFixture]
    public class GLAccountHierarchy_test : CommonNUnitFormFunctions
    {
        // Each account is defined by its LedgerNumber ...
        // Actually the value is read by the TAppSettingsManager
        private Int32 fLedgerNumber;

        /// <summary>
        /// Tests if the LostFocus of the TreeView will show the Node which hast
        /// Focus last time - cfg. Mantis 217
        /// </summary>
        [Test]
        public void T01_TreeViewFocusColor()
        {
            System.Console.WriteLine("-------T01_TreeViewFocusColor------------");
            TFrmGLAccountHierarchyTester hierarchyTester
                = new TFrmGLAccountHierarchyTester();
            hierarchyTester.mainForm.Show();
            hierarchyTester.mainForm.LedgerNumber = fLedgerNumber;

            // Focus on the TreeView ..
            hierarchyTester.trvAccounts.Properties.Focus();

            // Get a Node to operate ...
            int[] nodeList =
            {
                0, 2
            };
            hierarchyTester.trvAccounts.SelectNode(nodeList);
            TreeNode node = hierarchyTester.trvAccounts.Properties.SelectedNode;

            // Colors form the node ...
            Color colorBackNode1 = node.BackColor;
            Color colorFontNode1 = node.ForeColor;

            // Focus to somewhere else
            hierarchyTester.txtDetailAccountCode.Properties.Focus();

            // Colors from the node
            Color colorBackNode2 = node.BackColor;
            Color colorFontNode2 = node.ForeColor;

            // Focus back to the treeview
            hierarchyTester.trvAccounts.Properties.Focus();

            // Colors from the node
            Color colorBackNode3 = node.BackColor;
            Color colorFontNode3 = node.ForeColor;

            // Different checks
            // 1. Colors 1&3 must be equal ...

            Assert.AreEqual(colorBackNode1, colorBackNode3, "Back-Color 1&3");
            Assert.AreEqual(colorFontNode1, colorFontNode3, "Font-Color 1&3");

            // 1. Colors 1&2 must be different ...
            Assert.AreNotEqual(colorBackNode1, colorBackNode2, "Back-Color 1&2");
            Assert.AreNotEqual(colorFontNode1, colorFontNode2, "Font-Color 1&2");
        }

        /// <summary>
        /// This routine sets and unsets an account to and from a foreign currency ...
        /// </summary>
        [Test]
        public void T02_SetAccountToForeigenCurrency()
        {
            System.Console.WriteLine("-------T02_SetAccountToForeigenCurrency------------");
            TFrmGLAccountHierarchyTester hierarchyTester
                = new TFrmGLAccountHierarchyTester();
            hierarchyTester.mainForm.Show();
            hierarchyTester.mainForm.LedgerNumber = fLedgerNumber;

            // Select a node ...
            int[] nodeList1 =
            {
                0, 0, 0, 0, 0
            };
            int[] nodeList2 =
            {
                0, 2
            };
            hierarchyTester.trvAccounts.SelectNode(nodeList1);

            //TreeNode node = hierarchyTester.trvAccounts.Properties.SelectedNode;

            Boolean blnSaveBtn = hierarchyTester.tbbSave.Properties.Enabled;
            Assert.AreEqual(blnSaveBtn, false, "Save button shall be not enabled!");

            String str1;
            String str2;
            Boolean blnCheckBoxSet;
            Boolean blnCheckBoxSet2;

            for (int i = 0; i <= 1; i++)
            {
                blnCheckBoxSet =
                    (hierarchyTester.chkDetailForeignCurrencyFlag.Properties.CheckState
                     == CheckState.Checked);

                if (blnCheckBoxSet)
                {
                    // Actually it is not possiblie to write SetSelectedString("ALL") ín order to
                    // reset a combobox. This is to be fixed with id 216
                    hierarchyTester.cmbDetailForeignCurrencyCode.Properties.SetSelectedString("ALL");
                    hierarchyTester.chkDetailForeignCurrencyFlag.Properties.CheckState =
                        CheckState.Unchecked;
                }
                else
                {
                    hierarchyTester.cmbDetailForeignCurrencyCode.Properties.SetSelectedString("CNY");
                    hierarchyTester.chkDetailForeignCurrencyFlag.Properties.CheckState =
                        CheckState.Checked;
                }

                blnSaveBtn = hierarchyTester.tbbSave.Properties.Enabled;
                Assert.AreEqual(blnSaveBtn, true, "Save button must be enabled now!");
                hierarchyTester.tbbSave.Click();
                blnSaveBtn = hierarchyTester.tbbSave.Properties.Enabled;
                Assert.AreEqual(blnSaveBtn, false, "Save button must be disabled again!");

                // Select an other node and switch back ...
                str1 = hierarchyTester.txtDetailAccountCode.Text;
                hierarchyTester.trvAccounts.SelectNode(nodeList2);
                str2 = hierarchyTester.txtDetailAccountCode.Text;
                Assert.AreNotEqual(str1, str2, "Value must change because node has changed");
                hierarchyTester.trvAccounts.SelectNode(nodeList1);
                str2 = hierarchyTester.txtDetailAccountCode.Text;
                Assert.AreEqual(str1, str2, "Value must be equal because node has changed back");

                // Get back the stored data ...
                blnCheckBoxSet2 =
                    (hierarchyTester.chkDetailForeignCurrencyFlag.Properties.CheckState
                     == CheckState.Checked);
                str1 = hierarchyTester.cmbDetailForeignCurrencyCode.Properties.GetSelectedString();

                if (blnCheckBoxSet)
                {
                    Assert.AreEqual("ALL", str1, "We shall find the stored data ...");
                    Assert.AreEqual(blnCheckBoxSet2, false, "Checkbox shall be unchecked now");
                }
                else
                {
                    Assert.AreEqual("CNY", str1, "We shall find the stored data ...");
                    Assert.AreEqual(blnCheckBoxSet2, true, "Checkbox shall be checked now");
                }
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void T03_CreateANewAccount()
        {
            System.Console.WriteLine("-------T03_CreateANewAccount------------");
            TFrmGLAccountHierarchyTester hierarchyTester
                = new TFrmGLAccountHierarchyTester();
            hierarchyTester.mainForm.Show();
            hierarchyTester.mainForm.LedgerNumber = fLedgerNumber;

            int[] nodeList1 =
            {
                0, 1, 1
            };
            hierarchyTester.trvAccounts.SelectNode(nodeList1);

            // Create a first Account ...
            hierarchyTester.tbbAddNewAccount.Click();
            String strName1 = hierarchyTester.txtDetailAccountCode.Properties.Text;
            // Enable tbbSave
            hierarchyTester.txtDetailEngAccountCodeLongDesc.Properties.Text = "x";
            hierarchyTester.tbbSave.Click();

            // Create a second Account ...
            hierarchyTester.trvAccounts.SelectNode(nodeList1);
            hierarchyTester.tbbAddNewAccount.Click();
            String strName2 = hierarchyTester.txtDetailAccountCode.Properties.Text;
            hierarchyTester.txtDetailEngAccountCodeLongDesc.Properties.Text = "y";
            hierarchyTester.tbbSave.Click();

            Assert.AreNotEqual(strName1, strName2, "Two Accounts must not have the same name");

            // Create a third Account ...
            hierarchyTester.trvAccounts.SelectNode(nodeList1);
            hierarchyTester.tbbAddNewAccount.Click();
            // String strName3 = hierarchyTester.txtDetailAccountCode.Properties.Text;
            hierarchyTester.txtDetailEngAccountCodeLongDesc.Properties.Text = "z";
            // use an invalid name
            hierarchyTester.txtDetailAccountCode.Properties.Text = strName1;

            WaitForMessageBox(MessageBoxTester.Command.Yes);
            hierarchyTester.tbbSave.Click();

            // Create a fourth Account ...
            hierarchyTester.trvAccounts.SelectNode(nodeList1);
            hierarchyTester.tbbAddNewAccount.Click();
            // String strName4 = hierarchyTester.txtDetailAccountCode.Properties.Text;
            hierarchyTester.txtDetailEngAccountCodeLongDesc.Properties.Text = "zz";
            // use an invalid name
            hierarchyTester.txtDetailAccountCode.Properties.Text = strName1;
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void T04_CreateNewAccountAndChangeTreeViewSelection()
        {
            System.Console.WriteLine("-------T04_CreateNewAccountAndChangeTreeViewSelection------------");
            TFrmGLAccountHierarchyTester hierarchyTester
                = new TFrmGLAccountHierarchyTester();
            hierarchyTester.mainForm.Show();
            hierarchyTester.mainForm.LedgerNumber = fLedgerNumber;

            int[] nodeList1 =
            {
                0, 2, 1
            };
            hierarchyTester.trvAccounts.SelectNode(nodeList1);

            // Create a first Account ...
            hierarchyTester.tbbAddNewAccount.Click();
            // String strName1 = hierarchyTester.txtDetailAccountCode.Properties.Text;
            // Invalid Name resp. the name "BAL SHT" exists in the test db ...
            hierarchyTester.txtDetailAccountCode.Properties.Text = "BAL SHT";

            // simulate a leave of the message box. It seems selecting another node in the tree would not trigger the leave event in the test
            WaitForMessageBox(MessageBoxTester.Command.OK);
            hierarchyTester.txtDetailAccountCode.FireEvent("Leave");

            Assert.That(lastMessageTitle,
                Is.StringContaining("You cannot use an account name twice!"),
                "Error Message shall appear");
            Assert.That(hierarchyTester.trvAccounts.Properties.SelectedNode.Text,
                Is.StringContaining("NewAccount"),
                "New Selection shall have been canceled");

            WaitForMessageBox(MessageBoxTester.Command.No);
            hierarchyTester.mniClose.Click();
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void T05_CreateBankAccount()
        {
            System.Console.WriteLine("-------T05_CreateBankAccount------------");
            TFrmGLAccountHierarchyTester hierarchyTester
                = new TFrmGLAccountHierarchyTester();
            hierarchyTester.mainForm.LedgerNumber = fLedgerNumber;
            hierarchyTester.mainForm.Show();

            int[] nodeList1 =
            {
                0, 1, 1
            };
            hierarchyTester.trvAccounts.SelectNode(nodeList1);

            // Create a new bank Account ...
            hierarchyTester.tbbAddNewAccount.Click();
            string newAccountName = hierarchyTester.txtDetailAccountCode.Properties.Text;

            hierarchyTester.chkDetailBankAccountFlag.Properties.Checked = true;
            Assert.IsTrue(hierarchyTester.tbbSave.Properties.Enabled, "Save button should be enabled");
            hierarchyTester.tbbSave.Click();
            hierarchyTester.mainForm.Close();

            // reopen the screen, check if the account is a bank account, and remove the bank account flag
            hierarchyTester = new TFrmGLAccountHierarchyTester();
            hierarchyTester.mainForm.LedgerNumber = fLedgerNumber;
            hierarchyTester.mainForm.Show();

            TreeNode[] nodes = hierarchyTester.trvAccounts.Properties.Nodes.Find(newAccountName, true);
            hierarchyTester.trvAccounts.Properties.SelectedNode = nodes[0];

            Assert.AreEqual(newAccountName, hierarchyTester.txtDetailAccountCode.Properties.Text, "we want to look at the newly created account");
            Assert.AreEqual(true, hierarchyTester.chkDetailBankAccountFlag.Checked, "this should have been stored as a bank account");
            hierarchyTester.chkDetailBankAccountFlag.Properties.Checked = false;
            hierarchyTester.tbbSave.Click();
            hierarchyTester.mainForm.Close();

            // reopen the screen, check if the account is not a bank account anymore
            hierarchyTester = new TFrmGLAccountHierarchyTester();
            hierarchyTester.mainForm.Show();
            hierarchyTester.mainForm.LedgerNumber = fLedgerNumber;

            nodes = hierarchyTester.trvAccounts.Properties.Nodes.Find(newAccountName, true);
            hierarchyTester.trvAccounts.Properties.SelectedNode = nodes[0];

            Assert.AreEqual(newAccountName,
                hierarchyTester.txtDetailAccountCode.Properties.Text,
                "we want to look at the newly created account, the second time");
            Assert.AreEqual(false, hierarchyTester.chkDetailBankAccountFlag.Checked, "this should have been stored not as a bank account");
            hierarchyTester.mainForm.Close();
        }

        /// <summary>
        /// ...
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            new TLogging("PetraClient.log");
            TPetraConnector.Connect("../../../../../etc/TestClient.config");
            fLedgerNumber = Convert.ToInt32(TAppSettingsManager.GetValue("LedgerNumber"));
        }

        /// <summary>
        /// ...
        /// </summary>
        [TestFixtureTearDown]
        public void Dispose()
        {
            TPetraConnector.Disconnect();
        }
    }
}