//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using System.Threading;
using System.Globalization;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Controls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.App.Core;
using Ict.Testing.NUnitPetraClient;
using Ict.Testing.NUnitForms;
using NUnit.Extensions.Forms;

namespace Tests.Common.Controls
{
    /// TODO write your comment here
    [TestFixture]
    public class TTestComboboxAutoPopulated
    {
        private Int32 FLedgerNumber = -1;

        /// <summary>
        /// TestFixtureSetUp
        /// </summary>
        [SetUp]
        public void Init()
        {
            new TLogging("TestCommonControls.log");

            TPetraConnector.Connect("../../etc/TestClient.config");
            FLedgerNumber = Convert.ToInt32(TAppSettingsManager.GetValue("LedgerNumber"));
        }

        /// <summary>
        /// clean up, disconnect from OpenPetra server
        /// </summary>
        public void TearDown()
        {
            TPetraConnector.Disconnect();
        }

        /// <summary>
        /// testing auto populated combobox. bug #411, problem with description of first row
        /// </summary>
        [Test]
        public void TestComboboxEmptyDescriptionBug()
        {
            TCmbAutoPopulated cmb = new TCmbAutoPopulated();

            DataTable detailTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationList, FLedgerNumber);

            Assert.AreNotEqual(0, detailTable.Rows.Count, "There should be a motivation detail in the cached table");

            cmb.Name = "TestCombobox";
            cmb.InitialiseUserControl(detailTable,
                AMotivationDetailTable.GetMotivationDetailCodeDBName(),
                AMotivationDetailTable.GetMotivationDetailDescDBName(),
                null);
            cmb.AppearanceSetup(new int[] { -1, 150 }, -1);

            Form TestForm = new Form();
            TestForm.Controls.Add(cmb);

            TestForm.Show();

            cmb.SelectedIndex = 1;
            Assert.AreEqual(cmb.GetSelectedString(), "KEYMIN");
            Assert.AreEqual(cmb.GetSelectedDescription(), "Key Ministry Gift");

            cmb.SelectedIndex = 0;
            Assert.AreEqual(cmb.GetSelectedString(), "FIELD");
            Assert.AreEqual(cmb.GetSelectedDescription(), "Field Gift");
        }

        /// <summary>
        /// testing labelled combobox. bug #918, SelectedText is not reset when DataSource is changed
        /// </summary>
        [Test]
        public void TestComboboxResettingDataSourceSelectedText()
        {
            TCmbLabelled cmb = new TCmbLabelled();

            cmb.cmbCombobox.SetDataSourceStringList("test1,test2,test3,test4");

            Form TestForm = new Form();
            TestForm.Controls.Add(cmb);

            TestForm.Show();

            Assert.AreEqual(string.Empty, cmb.cmbCombobox.SelectedText, "initially no text is selected");

            cmb.SetSelectedString("test3");
            Assert.AreEqual("test3", cmb.cmbCombobox.Text, "test3 should be displayed");

            cmb.cmbCombobox.SetDataSourceStringList("test8,test9,test10");
            Assert.AreEqual(string.Empty, cmb.cmbCombobox.Text, "after resetting of datasource, no value should be selected");

            cmb.SetSelectedString("test3");
            cmb.cmbCombobox.DataSource = new DataTable().DefaultView;
            Assert.AreEqual(-1, cmb.cmbCombobox.SelectedIndex, "after resetting of datasource with dataview, selectedindex should be -1");
            Assert.AreEqual(string.Empty, cmb.cmbCombobox.Text, "after resetting of datasource to dataview, no value should be selected");
        }
    }
}