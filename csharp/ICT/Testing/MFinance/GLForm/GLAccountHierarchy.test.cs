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
    public class GLAccountHierarchy_test : NUnitFormTest
    {
        // Each account is defined by its LedgerNumber ...
        // Actually the value is read by the TAppSettingsManager
        private Int32 fLedgerNumber;


        [Test]
        public void T01_TreeViewFocusColor()
        {
            System.Console.WriteLine("-------T01_TreeViewFocusColor------------");
            TFrmGLAccountHierarchyTester hierarchyTester
                = new TFrmGLAccountHierarchyTester();
            hierarchyTester.mainForm.Show();
            hierarchyTester.mainForm.LedgerNumber = fLedgerNumber;


            // Get a Node to operate ...
            int[] nodeList1 =
            {
                0
            };
            int[] nodeList2 =
            {
                0, 2
            };
            int[] nodeList3 =
            {
                0, 4
            };
            hierarchyTester.trvAccounts.SelectNode(nodeList1);
            hierarchyTester.trvAccounts.Properties.Focus();
            TreeNode node1 = hierarchyTester.trvAccounts.Properties.SelectedNode;

            Color colorBackNode1 = node1.BackColor;
            Color colorFontNode1 = node1.ForeColor;

            System.Console.WriteLine("Color1 : " + colorBackNode1.ToString());
            System.Console.WriteLine("Color1 : " + colorFontNode1.ToString());


            node1.Expand();

            System.Threading.Thread.Sleep(10000);

            //hierarchyTester.trvAccounts.SelectNode(nodeList2);
            hierarchyTester.trvAccounts.Properties.Focus();

            hierarchyTester.txtDetailAccountCode.Properties.Focus();

            TreeNode node2 = hierarchyTester.trvAccounts.Properties.SelectedNode;
            Color colorBackNode2 = node2.BackColor;
            Color colorFontNode2 = node2.ForeColor;

            System.Console.WriteLine("Color1 : " + colorBackNode1.ToString());
            System.Console.WriteLine("Color1 : " + colorFontNode1.ToString());

            System.Console.WriteLine("Color2 : " + colorBackNode2.ToString());
            System.Console.WriteLine("Color2 : " + colorFontNode2.ToString());
            System.Threading.Thread.Sleep(10000);


            //hierarchyTester.trvAccounts.SelectNode(nodeList3);
            hierarchyTester.trvAccounts.Properties.Focus();

            TreeNode node3 = hierarchyTester.trvAccounts.Properties.SelectedNode;
            Color colorBackNode3 = node3.BackColor;
            Color colorFontNode3 = node3.ForeColor;

            System.Console.WriteLine("Color1 : " + colorBackNode1.ToString());
            System.Console.WriteLine("Color1 : " + colorFontNode1.ToString());

            System.Console.WriteLine("Color2 : " + colorBackNode2.ToString());
            System.Console.WriteLine("Color2 : " + colorFontNode2.ToString());

            System.Console.WriteLine("Color3 : " + colorBackNode3.ToString());
            System.Console.WriteLine("Color3 : " + colorFontNode3.ToString());

            System.Console.WriteLine("Text1 : " + node1.Text);
            System.Console.WriteLine("Text2 : " + node2.Text);
            System.Console.WriteLine("Text3 : " + node3.Text);
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
    }
}