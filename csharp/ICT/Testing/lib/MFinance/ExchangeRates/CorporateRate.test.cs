//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       AlanP
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
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;

using NUnit.Extensions.Forms;
using NUnit.Framework;

using SourceGrid;

using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;
using Ict.Testing.NUnitTools;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance.Account.Data;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This test suite will delete all the existing rows in the Corporate Exchange rate table
// It will not affect any other tables
///////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Tests.MFinance.Client.ExchangeRates
{
    /// Testing the Corporate Exchange rate screen
    [TestFixture]
    public partial class TCorporateExchangeRateTest : NUnitFormTest
    {
        private Boolean FConnectedToServer = false;

        #region Setup and TearDown

        /// <summary>
        /// Set up test for corporate exchange rate screen
        /// </summary>
        public override void Setup()
        {
            new TLogging("../../log/TestClient.log");

            FConnectedToServer = false;
            try
            {
                TPetraConnector.Connect("../../etc/TestClient.config");
                FConnectedToServer = true;
            }
            catch (Exception Exc)
            {
                Assert.Fail("Failed to connect to the Petra Server.  Have you forgotten to launch the Server Console? Exception: \r\n" + Exc.ToString());
            }
        }

        /// <summary>
        /// clean up, disconnect from OpenPetra server
        /// </summary>
        public override void TearDown()
        {
            if (!FConnectedToServer)
            {
                return;
            }

            FMainDS.DeleteAllRows();
            FMainDS.SaveChanges();

            TPetraConnector.Disconnect();
        }

        #endregion

        #region Load the screen with an empty table

        /// <summary>
        /// Test loading the screen with no data in the Exchange Rate table
        /// </summary>
        [Test]
        public void LoadEmptyTable()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.SaveChanges();

            TFrmSetupCorporateExchangeRate mainScreen = new TFrmSetupCorporateExchangeRate(null);
            mainScreen.Show();

            // Toolstrip
            ToolStripButton btnSave = (new ToolStripButtonTester("tbbSave", mainScreen)).Properties;

            // Grid
            TSgrdDataGrid grdDetails = (TSgrdDataGrid)(new TSgrdDataGridPagedTester("grdDetails", mainScreen)).Properties;

            // Panel and controls
            Panel pnlDetails = (new PanelTester("pnlDetails", mainScreen)).Properties;
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", mainScreen)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", mainScreen)).Properties;
            TtxtPetraDate dtpEffectiveDate = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", mainScreen)).Properties;
            CheckBox chkHideOthers = (new CheckBoxTester("chkHideOthers", mainScreen)).Properties;

            Assert.IsFalse(btnSave.Enabled, "The Save button should be disabled when the screen is loaded");
            Assert.IsFalse(pnlDetails.Enabled, "The Details Panel should be disabled on initial load");
            Assert.IsFalse(dtpEffectiveDate.Date.HasValue, "The date control should be empty on initial load");

            Assert.AreEqual(1, grdDetails.Rows.Count, "The grid should be empty");

            mainScreen.Close();
        }

        #endregion

        #region Load the screen from a table with existing data

        /// <summary>
        /// Test loading the screen with data in the Exchange Rate table
        /// </summary>
        [Test]
        public void LoadTableContainingData()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.InsertStandardRows();
            FMainDS.SaveChanges();

            TFrmSetupCorporateExchangeRate mainScreen = new TFrmSetupCorporateExchangeRate(null);
            mainScreen.Show();

            // Toolstrip
            ToolStripButton btnSave = (new ToolStripButtonTester("tbbSave", mainScreen)).Properties;

            // Grid
            TSgrdDataGrid grdDetails = (TSgrdDataGrid)(new TSgrdDataGridPagedTester("grdDetails", mainScreen)).Properties;

            // Panel and controls
            Panel pnlDetails = (new PanelTester("pnlDetails", mainScreen)).Properties;
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", mainScreen)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", mainScreen)).Properties;
            TtxtPetraDate dtpEffectiveDate = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", mainScreen)).Properties;
            CheckBox chkHideOthers = (new CheckBoxTester("chkHideOthers", mainScreen)).Properties;

            // Start of testing...
            Assert.IsFalse(btnSave.Enabled, "The Save button should be disabled when the screen is loaded");
            Assert.IsTrue(pnlDetails.Enabled, "The Details Panel should be enabled on initial load");

            // Check the number of rows in the grid
            Assert.AreEqual(FAllRowCount + 1, grdDetails.Rows.Count);

            FCurrentDataId = Row2DataId(1);

            // Check the content of the details panel matches the last item in standard data (because sorting will have put it first)
            Assert.AreEqual(EffectiveCurrency(FFromCurrencyId),
                cmbFromCurrency.GetSelectedString(), "The From currency on row 1 should be {0}", EffectiveCurrency(FFromCurrencyId));
            Assert.AreEqual(EffectiveCurrency(FToCurrencyId),
                cmbToCurrency.GetSelectedString(), "The To currency on row 1 should be {0}", EffectiveCurrency(FToCurrencyId));
            Assert.AreEqual(EffectiveDate(), dtpEffectiveDate.Date, "The effective date on row 1 should be {0}", EffectiveDate().ToString());

            // Select the second row - which will be the last but one item of standard data
            SelectRowInGrid(2);

            // Check the details again for this row
            Assert.AreEqual(EffectiveCurrency(FFromCurrencyId),
                cmbFromCurrency.GetSelectedString(), "The From currency on row 2 should be {0}", EffectiveCurrency(FFromCurrencyId));
            Assert.AreEqual(EffectiveCurrency(FToCurrencyId),
                cmbToCurrency.GetSelectedString(), "The To currency on row 2 should be {0}", EffectiveCurrency(FToCurrencyId));
            Assert.AreEqual(EffectiveDate(), dtpEffectiveDate.Date, "The effective date on row 2 should be {0}", EffectiveDate().ToString());

            // Now hide the other currencies
            chkHideOthers.Checked = true;

            // The number of rows in the grid should have changed
            Assert.AreEqual(FHiddenRowCount + 1,
                grdDetails.Rows.Count,
                "The grid should have {0} rows when the checkbox is checked",
                FHiddenRowCount + 1);

            // But the details should still be the same
            Assert.AreEqual(EffectiveCurrency(FFromCurrencyId),
                cmbFromCurrency.GetSelectedString(), "The From currency on row 2 should be {0}", EffectiveCurrency(FFromCurrencyId));
            Assert.AreEqual(EffectiveCurrency(FToCurrencyId),
                cmbToCurrency.GetSelectedString(), "The To currency on row 2 should be {0}", EffectiveCurrency(FToCurrencyId));
            Assert.AreEqual(EffectiveDate(), dtpEffectiveDate.Date, "The effective date on row 2 should be {0}", EffectiveDate().ToString());
            Assert.IsFalse(cmbToCurrency.Enabled, "The To Currency should be disabled when the checkbox is checked");

            // Uncheck the box and select the last row
            chkHideOthers.Checked = false;
            SelectRowInGrid(FAllRowCount);

            // Check the details - should be the first item of standard data
            Assert.AreEqual(EffectiveCurrency(FFromCurrencyId),
                cmbFromCurrency.GetSelectedString(), "The From currency on row {0} should be {1}", FAllRowCount, EffectiveCurrency(FFromCurrencyId));
            Assert.AreEqual(EffectiveCurrency(FToCurrencyId),
                cmbToCurrency.GetSelectedString(), "The To currency on row {0} should be {1}", FAllRowCount, EffectiveCurrency(FToCurrencyId));
            Assert.AreEqual(EffectiveDate(), dtpEffectiveDate.Date, "The effective date on row {0} should be {1}", FAllRowCount,
                EffectiveDate().ToString());

            // Hide other To currencies again - now the selected row will have jumped higher
            chkHideOthers.Checked = true;
            Assert.AreEqual(FHiddenRowCount,
                mainScreen.GetSelectedRowIndex(), "When the checkbox is checked the selected row should be {0}", FHiddenRowCount);

            // But the details should again be the same as before the checkbox check
            Assert.AreEqual(EffectiveCurrency(FFromCurrencyId),
                cmbFromCurrency.GetSelectedString(), "The From currency on row {0} should be {1}", FHiddenRowCount, EffectiveCurrency(FFromCurrencyId));
            Assert.AreEqual(EffectiveCurrency(FToCurrencyId),
                cmbToCurrency.GetSelectedString(), "The To currency on row {0} should be {1}", FHiddenRowCount, EffectiveCurrency(FToCurrencyId));
            Assert.AreEqual(EffectiveDate(), dtpEffectiveDate.Date, "The effective date on row {0} should be {1}", FHiddenRowCount,
                EffectiveDate().ToString());

            mainScreen.Close();
        }

        #endregion

        #region Add a first row to an empty table

        /// <summary>
        /// Test adding the first row to a table
        /// </summary>
        [Test]
        public void AddRowToEmptyTable()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.SaveChanges();

            TFrmSetupCorporateExchangeRate mainScreen = new TFrmSetupCorporateExchangeRate(null);
            mainScreen.RunOnceOnActivation();
            mainScreen.Show();

            // Toolstrip
            ToolStripButton btnSave = (new ToolStripButtonTester("tbbSave", mainScreen)).Properties;

            // Grid
            TSgrdDataGrid grdDetails = (TSgrdDataGrid)(new TSgrdDataGridPagedTester("grdDetails", mainScreen)).Properties;

            // Panel and controls
            Panel pnlDetails = (new PanelTester("pnlDetails", mainScreen)).Properties;
            ButtonTester btnNew = new ButtonTester("btnNew", mainScreen);
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", mainScreen)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", mainScreen)).Properties;
            TtxtPetraDate dtpEffectiveDate = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", mainScreen)).Properties;
            TTxtNumericTextBox txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen)).Properties;

            Assert.IsFalse(btnSave.Enabled, "The Save button should be disabled when the screen is loaded");
            btnNew.Click();
            Assert.IsTrue(btnSave.Enabled, "The Save button should be enabled after adding a new row");

            // Work out our expectations
            string expectedToCurrency = "USD";
            string baseCurrency = GetDefaultBaseCurrency();

            if (baseCurrency == "USD")
            {
                baseCurrency = "GBP";
            }

            DateTime expectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Check the details panel after adding the new row
            Assert.AreEqual(baseCurrency, cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(expectedToCurrency, cmbToCurrency.GetSelectedString());
            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date);
            Assert.AreEqual(0.0m, txtExchangeRate.NumberValueDecimal);

            // Set a valid exchange rate and save
            txtExchangeRate.NumberValueDecimal = 2.0m;

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.OK);
            };

            mainScreen.SaveChanges();
            Assert.IsFalse(btnSave.Enabled, "The Save button should be disabled after the new row has been saved");

            // Check the row count in the grid
            Assert.AreEqual(3, grdDetails.Rows.Count, "There should be 2 rows in the grid after saving a new row");

            // Even though an inverse row has been added we should still be highlighting the newly added row
            Assert.AreEqual(baseCurrency, cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(expectedToCurrency, cmbToCurrency.GetSelectedString());
            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date);
            Assert.AreEqual(2.0m, txtExchangeRate.NumberValueDecimal);

            // Now select the inverese row
            SelectRowInGrid(1);

            // Check the details are, in fact, the inverse
            Assert.AreEqual(expectedToCurrency, cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(baseCurrency, cmbToCurrency.GetSelectedString());
            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date);
            Assert.AreEqual(0.5m, txtExchangeRate.NumberValueDecimal);

            mainScreen.Close();
        }

        #endregion

        #region Add a new row to a table that already contains data

        /// <summary>
        /// Test adding a new row to a table that already has rows
        /// </summary>
        [Test]
        public void AddRowToTable()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.InsertStandardRows();
            FMainDS.SaveChanges();

            TFrmSetupCorporateExchangeRate mainScreen = new TFrmSetupCorporateExchangeRate(null);
            mainScreen.Show();

            // Toolstrip
            ToolStripButton btnSave = (new ToolStripButtonTester("tbbSave", mainScreen)).Properties;

            // Grid
            TSgrdDataGrid grdDetails = (TSgrdDataGrid)(new TSgrdDataGridPagedTester("grdDetails", mainScreen)).Properties;

            // Panel and controls
            Panel pnlDetails = (new PanelTester("pnlDetails", mainScreen)).Properties;
            ButtonTester btnNew = new ButtonTester("btnNew", mainScreen);
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", mainScreen)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", mainScreen)).Properties;
            TtxtPetraDate dtpEffectiveDate = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", mainScreen)).Properties;
            TTxtNumericTextBox txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen)).Properties;

            // Select the bottom row - when we get a new row it should be based on StandardData[1]
            SelectRowInGrid(FAllRowCount, 1);

            // Check that the controls are disabled
            Assert.IsFalse(cmbFromCurrency.Enabled);
            Assert.IsFalse(cmbToCurrency.Enabled);
            Assert.IsTrue(dtpEffectiveDate.ReadOnly);

            // Check that the controls are enabled
            Assert.IsTrue(txtExchangeRate.Enabled);

            // Click the 'New' button
            Assert.IsFalse(btnSave.Enabled, "The Save button should be disabled when the screen is loaded");
            btnNew.Click();
            Assert.IsTrue(btnSave.Enabled, "The Save button should be enabled after adding a new row");
            Assert.IsTrue(cmbFromCurrency.Enabled);
            Assert.IsTrue(cmbToCurrency.Enabled);
            Assert.IsTrue(dtpEffectiveDate.Enabled);
            Assert.IsTrue(txtExchangeRate.Enabled);

            // The effective date should be 1st of current month
            DateTime expectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            Assert.AreEqual(EffectiveCurrency(FFromCurrencyId), cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(EffectiveCurrency(FToCurrencyId), cmbToCurrency.GetSelectedString());
            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date);
            Assert.AreEqual(EffectiveRate(), txtExchangeRate.NumberValueDecimal.Value);

            // The row number of the new row should be at row 7
            Assert.AreEqual(FAllRowCount - 1, mainScreen.GetSelectedRowIndex());

            // Change the rate to a new value
            decimal newRate = 0.667m;
            txtExchangeRate.NumberValueDecimal = newRate;

            // click the 'New' button again - this time the date should be first of next month
            btnNew.Click();
            expectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(1);

            // The details should be the same as before except for the new date and the rate being what we just set
            Assert.AreEqual(EffectiveCurrency(FFromCurrencyId), cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(EffectiveCurrency(FToCurrencyId), cmbToCurrency.GetSelectedString());
            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date);
            Assert.AreEqual(newRate, txtExchangeRate.NumberValueDecimal.Value);
            Assert.AreEqual(FAllRowCount - 1, mainScreen.GetSelectedRowIndex());

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.OK);
            };

            // Save the changes and check the number of rows now
            mainScreen.SaveChanges();
            Assert.IsFalse(btnSave.Enabled, "The Save button should be disabled after the new row has been saved");
            Assert.AreEqual(13, grdDetails.Rows.Count, "There should be 12 rows in the grid after saving 2 new rows");

            mainScreen.Close();
        }

        #endregion

        #region Edit a New Row

        /// <summary>
        /// Create new rows and test the inter-actions as you edit currency and date
        /// </summary>
        [Test]
        public void EditRow()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.InsertStandardRows();
            FMainDS.SaveChanges();

            TFrmSetupCorporateExchangeRate mainScreen = new TFrmSetupCorporateExchangeRate(null);
            mainScreen.Show();

            // Toolstrip
            ToolStripButton btnSave = (new ToolStripButtonTester("tbbSave", mainScreen)).Properties;

            // Grid
            TSgrdDataGrid grdDetails = (TSgrdDataGrid)(new TSgrdDataGridPagedTester("grdDetails", mainScreen)).Properties;

            // Panel and controls
            Panel pnlDetails = (new PanelTester("pnlDetails", mainScreen)).Properties;
            ButtonTester btnNew = new ButtonTester("btnNew", mainScreen);
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", mainScreen)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", mainScreen)).Properties;
            TtxtPetraDate dtpEffectiveDate = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", mainScreen)).Properties;
            TTxtNumericTextBox txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen)).Properties;

            // Select the first row in the grid.  New rows should be based on data row 5
            SelectRowInGrid(1, 5);

            // Add three rows
            btnNew.Click();
            btnNew.Click();
            btnNew.Click();

            DateTime dt1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //DateTime dt2 = dt1.AddMonths(1);
            DateTime dt3 = dt1.AddMonths(2);

            // Check the data first
            Assert.AreEqual(EffectiveCurrency(FFromCurrencyId), cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(EffectiveCurrency(FToCurrencyId), cmbToCurrency.GetSelectedString());
            Assert.AreEqual(dt3, dtpEffectiveDate.Date);
            Assert.AreEqual(EffectiveRate(), txtExchangeRate.NumberValueDecimal.Value);
            Assert.AreEqual(3, mainScreen.GetSelectedRowIndex());

            // Focus on the from currency, then change it to 'BEF'
            cmbFromCurrency.Focus();
            cmbFromCurrency.SetSelectedString("BEF");
            cmbToCurrency.Focus();

            // Now check the date and rate.  Date should be back to this month and rate should be 0.00 because this currency has never been used
            Assert.AreEqual(dt1, dtpEffectiveDate.Date.Value);
            Assert.AreEqual(0.0m, txtExchangeRate.NumberValueDecimal);
            Assert.AreEqual(1, mainScreen.GetSelectedRowIndex());

            // Reset the currency and confirm we go back to where we were
            cmbFromCurrency.Focus();
            cmbFromCurrency.SetSelectedString(EffectiveCurrency(FFromCurrencyId));
            cmbToCurrency.Focus();

            Assert.AreEqual(dt3, dtpEffectiveDate.Date.Value);
            Assert.AreEqual(EffectiveRate(), txtExchangeRate.NumberValueDecimal);
            Assert.AreEqual(3, mainScreen.GetSelectedRowIndex());

            // Repeat for the To currency
            cmbToCurrency.Focus();
            cmbToCurrency.SetSelectedString("BEF");
            dtpEffectiveDate.Focus();

            // Now check the date and rate.  Date should be back to this month and rate should be 0.00 because this currency has never been used
            Assert.AreEqual(dt1, dtpEffectiveDate.Date.Value);
            Assert.AreEqual(0.0m, txtExchangeRate.NumberValueDecimal);
            Assert.AreEqual(1, mainScreen.GetSelectedRowIndex());

            // Reset the currency and confirm we go back to where we were
            cmbToCurrency.Focus();
            cmbToCurrency.SetSelectedString(EffectiveCurrency(FToCurrencyId));
            dtpEffectiveDate.Focus();

            Assert.AreEqual(dt3, dtpEffectiveDate.Date.Value);
            Assert.AreEqual(EffectiveRate(), txtExchangeRate.NumberValueDecimal);
            Assert.AreEqual(3, mainScreen.GetSelectedRowIndex());

            // Finally check what happens when editing the date
            SelectRowInGrid(5);
            txtExchangeRate.NumberValueDecimal = 8.0m;      // Today
            FixUnvalidatedChanges();
            SelectRowInGrid(4);
            txtExchangeRate.NumberValueDecimal = 9.0m;      // Today + 1m
            FixUnvalidatedChanges();
            SelectRowInGrid(3);
            txtExchangeRate.NumberValueDecimal = 10.0m;     // Today +2m
            FixUnvalidatedChanges();

            SelectRowInGrid(5);
            Assert.AreEqual(8.0m, txtExchangeRate.NumberValueDecimal);
            dtpEffectiveDate.Focus();
            dtpEffectiveDate.Date = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(6);
            grdDetails.Focus();
            Assert.AreEqual(10.0m, txtExchangeRate.NumberValueDecimal);

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.OK);
            };

            mainScreen.SaveChanges();
            mainScreen.Close();
        }

        #endregion

        #region Invert Rate Test

        /// <summary>
        /// Test the Invert Rate button functionality
        /// </summary>
        [Test]
        public void InvertRate()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.InsertStandardRows();
            FMainDS.SaveChanges();

            TFrmSetupCorporateExchangeRate mainScreen = new TFrmSetupCorporateExchangeRate(null);
            mainScreen.Show();

            // Toolstrip
            ToolStripButton btnSave = (new ToolStripButtonTester("tbbSave", mainScreen)).Properties;

            // Grid
            TSgrdDataGrid grdDetails = (TSgrdDataGrid)(new TSgrdDataGridPagedTester("grdDetails", mainScreen)).Properties;

            // Panel and controls
            Panel pnlDetails = (new PanelTester("pnlDetails", mainScreen)).Properties;
            ButtonTester btnNew = new ButtonTester("btnNew", mainScreen);
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", mainScreen)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", mainScreen)).Properties;
            TtxtPetraDate dtpEffectiveDate = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", mainScreen)).Properties;
            TTxtNumericTextBox txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen)).Properties;
            ButtonTester btnInvert = new ButtonTester("btnInvertExchangeRate", mainScreen);

            // Select the first row in the grid.  New rows should be based on data row 5
            SelectRowInGrid(1, 5);

            // Check that Invert enabled and test that it works
            btnNew.Click();
            Assert.IsTrue(btnInvert.Properties.Enabled);
            txtExchangeRate.NumberValueDecimal = 5.0m;
            btnInvert.Click();
            Assert.AreEqual(0.2m, txtExchangeRate.NumberValueDecimal);
            btnInvert.Click();
            Assert.AreEqual(5.0m, txtExchangeRate.NumberValueDecimal);

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.OK);
            };

            mainScreen.SaveChanges();
            mainScreen.Close();
        }

        #endregion

        #region Save and Cancel

        /// <summary>
        /// Test the save and Cancel functionality
        /// </summary>
        [Test]
        public void SaveAndCancel()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.SaveChanges();

            TFrmSetupCorporateExchangeRate mainScreen = new TFrmSetupCorporateExchangeRate(null);
            mainScreen.Show();

            // Save and New buttons
            ToolStripButtonTester btnSave = new ToolStripButtonTester("tbbSave", mainScreen);
            ButtonTester btnNew = new ButtonTester("btnNew", mainScreen);
            TTxtNumericTextBox txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen)).Properties;

            // Add new row, save and close
            btnNew.Click();
            txtExchangeRate.NumberValueDecimal = 10m;

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.OK);
            };

            btnSave.Click();
            mainScreen.Close();

            // Create the screen a second time
            TFrmSetupCorporateExchangeRate mainScreen2 = new TFrmSetupCorporateExchangeRate(null);
            mainScreen2.Show();

            // make sure the data really got saved
            TSgrdDataGrid grdDetails = (TSgrdDataGrid)(new TSgrdDataGridPagedTester("grdDetails", mainScreen2).Properties);
            Assert.AreEqual(3, grdDetails.Rows.Count);

            btnNew = new ButtonTester("btnNew", mainScreen2);
            txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen2)).Properties;

            // Add another row, but this time close without saving
            btnNew.Click();

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.No);
            };

            mainScreen2.Close();

            // Create the screen a third time
            TFrmSetupCorporateExchangeRate mainScreen3 = new TFrmSetupCorporateExchangeRate(null);
            mainScreen3.Show();

            // make sure the data did not get saved
            grdDetails = (TSgrdDataGrid)(new TSgrdDataGridPagedTester("grdDetails", mainScreen3).Properties);
            Assert.AreEqual(3, grdDetails.Rows.Count);

            btnNew = new ButtonTester("btnNew", mainScreen3);
            txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen3)).Properties;

            // Add another row, but this time close AND save
            btnNew.Click();

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.Yes);

                ModalFormHandler = delegate(string name2, IntPtr hWnd2, Form form2)
                {
                    MessageBoxTester tester2 = new MessageBoxTester(hWnd2);
                    tester2.SendCommand(MessageBoxTester.Command.OK);
                };
            };

            mainScreen3.Close();

            // Create the screen a fourth time!
            TFrmSetupCorporateExchangeRate mainScreen4 = new TFrmSetupCorporateExchangeRate(null);
            mainScreen4.Show();

            // make sure the data was saved last time
            grdDetails = (TSgrdDataGrid)(new TSgrdDataGridPagedTester("grdDetails", mainScreen4).Properties);
            Assert.AreEqual(5, grdDetails.Rows.Count);

            mainScreen4.Close();
        }

        #endregion

        #region Import

        /// <summary>
        /// Test the Import functionality
        /// </summary>
        [Test]
        public void Import()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.SaveChanges();

            TVerificationResultCollection results = new TVerificationResultCollection();
            string resultText;
            string firstResultCode;
            Boolean gotDateWarning;

            RunTestImport("corporate-csv/GoodImport.csv", ",", results, out resultText, out firstResultCode, out gotDateWarning);
            Assert.IsTrue(gotDateWarning, "Expected a warning about dates");
            Assert.AreEqual(String.Empty, resultText, "Errors during import...");
            Assert.AreEqual(8, FMainDS.ACorporateExchangeRate.Rows.Count, "Wrong number of rows after successful import");

            FMainDS.DeleteAllRows();
            RunTestImport("corporate-csv/BadCurrencyImport.csv", ",", results, out resultText, out firstResultCode, out gotDateWarning);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(CommonErrorCodes.ERR_INCONGRUOUSSTRINGS, firstResultCode);

            FMainDS.DeleteAllRows();
            RunTestImport("corporate-csv/BadDateImport.csv", ",", results, out resultText, out firstResultCode, out gotDateWarning);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(CommonErrorCodes.ERR_INCONGRUOUSSTRINGS, firstResultCode);

            FMainDS.DeleteAllRows();
            RunTestImport("corporate-csv/BadRateImport.csv", ",", results, out resultText, out firstResultCode, out gotDateWarning);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(CommonErrorCodes.ERR_INCONGRUOUSSTRINGS, firstResultCode);

            // Test for a missing column
            FMainDS.DeleteAllRows();
            RunTestImport("corporate-csv/MissingColumn.csv", ",", results, out resultText, out firstResultCode, out gotDateWarning);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(CommonErrorCodes.ERR_INCONGRUOUSSTRINGS, firstResultCode);

            // Run the test(s) that have duplicates
            FMainDS.DeleteAllRows();
            FMainDS.InsertStandardRows();
            RunTestImport("corporate-csv/GoodImport-WithDuplicates.csv", ",", results, out resultText, out firstResultCode, out gotDateWarning);
            Assert.AreEqual(String.Empty, resultText, "Errors during import...");
            Assert.AreEqual(12, FMainDS.ACorporateExchangeRate.Rows.Count, "Wrong number of rows after successful import");

            // And Headers
            FMainDS.DeleteAllRows();
            RunTestImport("corporate-csv/GoodImport-WithHeader.csv", ",", results, out resultText, out firstResultCode, out gotDateWarning);
            Assert.AreEqual(String.Empty, resultText, "Errors during import...");
            Assert.AreEqual(8, FMainDS.ACorporateExchangeRate.Rows.Count, "Wrong number of rows after successful import");

            // Test a date/rate only file - this is tab separated
            FMainDS.DeleteAllRows();
            RunTestImport("corporate-csv/USD_EUR.csv", "\t", results, out resultText, out firstResultCode, out gotDateWarning);
            Assert.AreEqual(String.Empty, resultText, "Errors during import...");
            Assert.AreEqual(8, FMainDS.ACorporateExchangeRate.Rows.Count, "Wrong number of rows after successful import");

            // Test a file with its own inverses
            FMainDS.DeleteAllRows();
            RunTestImport("corporate-csv/GoodImport-WithInverses.csv", ",", results, out resultText, out firstResultCode, out gotDateWarning);
            Assert.AreEqual(String.Empty, resultText, "Errors during import...");
            Assert.AreEqual(4, FMainDS.ACorporateExchangeRate.Rows.Count, "Wrong number of rows after successful import");
        }

        #endregion

        #region Validation

        /// <summary>
        /// Test the input validation
        /// </summary>
        [Test]
        public void Validation()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.SaveChanges();

            TFrmSetupCorporateExchangeRate mainScreen = new TFrmSetupCorporateExchangeRate(null);
            mainScreen.Show();

            ButtonTester btnNew = new ButtonTester("btnNew", mainScreen);
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", mainScreen)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", mainScreen)).Properties;
            TtxtPetraDate dtpEffectiveDate = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", mainScreen)).Properties;
            TTxtNumericTextBox txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen)).Properties;

            btnNew.Click();

            // Set up some bad entries
            dtpEffectiveDate.Text = "";
            txtExchangeRate.NumberValueDecimal = 0.0m;

            string dlgText = String.Empty;
            bool dlgDisplayed = false;

            // Click the New button and discover what validation errors we have
            // Note - we do not put assert's inside the delegate because we want the dialog to close.
            //   If the dialog is left hanging this might do bad stuff to automated testing
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                dlgText = tester.Text;
                dlgDisplayed = true;
                tester.SendCommand(MessageBoxTester.Command.OK);
            };

            btnNew.Click();

            // Check that we did display the dialog and that we picked up the validation errors we predicted
            Assert.IsTrue(dlgDisplayed);
            Assert.IsTrue(dlgText.Contains(CommonErrorCodes.ERR_NOUNDEFINEDDATE));
            Assert.IsTrue(dlgText.Contains(CommonErrorCodes.ERR_INVALIDNUMBER));

            // Close without saving
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.No);
            };

            mainScreen.Close();
        }

        #endregion

        #region Helper methods
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Helper functions

        private void FixUnvalidatedChanges()
        {
            ((TCmbAutoPopulated)(new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode")).Properties).Focus();
            ((TCmbAutoPopulated)(new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode")).Properties).Focus();
        }

        private string GetDefaultBaseCurrency()
        {
            ALedgerTable ledgers = TRemote.MFinance.Setup.WebConnectors.GetAvailableLedgers();
            DataView ledgerView = ledgers.DefaultView;

            ledgerView.RowFilter = "a_ledger_status_l = 1";     // Only view 'in use' ledgers

            if (ledgerView.Count > 0)
            {
                // There is at least one - so default to the currency of the first one
                return ((ALedgerRow)ledgerView.Table.Rows[0]).BaseCurrency;
            }

            return null;
        }

        private void RunTestImport(string AFileName,
            string ACSVSeparator,
            TVerificationResultCollection AResults,
            out string AResultText,
            out string AFirstResultCode,
            out Boolean AGotDateWarning)
        {
            string TestFile = Path.GetFullPath(TAppSettingsManager.GetValue("Testing.Path") + "/lib/MFinance/ExchangeRates/" + AFileName);

            Assert.IsTrue(File.Exists(TestFile), "File does not exist: " + TestFile);

            AResults.Clear();
            TImportExchangeRates.ImportCurrencyExRates(FMainDS.ACorporateExchangeRate, TestFile, ACSVSeparator, "Corporate", AResults);

            AResultText = String.Empty;
            AGotDateWarning = false;

            for (int i = 0; i < AResults.Count; i++)
            {
                if (AResults[i].ResultText.Contains("Warning:") && AResults[i].ResultText.Contains("before the current accounting period"))
                {
                    AGotDateWarning = true;
                }
                else
                {
                    AResultText += String.Format("{0}: {1}{2}", i.ToString(), AResults[i].ResultText, Environment.NewLine);
                }
            }

            if (AResultText.Length > 0)
            {
                Console.WriteLine(AResultText);
            }

            AFirstResultCode = String.Empty;

            if (AResults.Count > 0)
            {
                AFirstResultCode = AResults[0].ResultCode;
            }
        }

        #endregion
    }
}