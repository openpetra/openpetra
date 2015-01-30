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
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This test suite will delete all the existing rows in the Daily Exchange rate table
// It will not affect any other tables
///////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Tests.MFinance.Client.ExchangeRates
{
    /// Testing the Daily Exchange rate screen
    /// This part of the class tests the screen as a MODELESS dialog
    /// - See DailyRateModal.test.cs for MODAL tests
    [TestFixture]
    public partial class TDailyExchangeRateTest : NUnitFormTest
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

            // We use a special FIN. finance module error code which we need to register
            ErrorCodeInventory.RegisteredTypes.Add(new Ict.Petra.Shared.PetraErrorCodes().GetType());
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

            // We need to re-load the data set because since we had it open the modal form may have changed things
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.SaveChanges();

            FCorporateDS.DeleteAllCorporateRates();
            FLedgerDS.DeleteTestLedgerIfExists();
            FGiftAndJournal.Clean();

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

            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
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

            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
            mainScreen.Show();

            // Toolstrip
            ToolStripButton btnSave = (new ToolStripButtonTester("tbbSave", mainScreen)).Properties;

            // Modal controls
            Button btnClose = (new ButtonTester("btnClose", FModalFormName)).Properties;
            Button btnCancel = (new ButtonTester("btnCancel", FModalFormName)).Properties;

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
            Assert.IsFalse(btnClose.Visible, "The Accept button should not be visible on a modeless form");
            Assert.IsFalse(btnCancel.Visible, "The Cancel but should not be visible on a modeless form");

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

            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
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
            string baseCurrency = GetDefaultBaseCurrency();
            string expectedFromCurrency = "USD";

            if (baseCurrency == "USD")
            {
                expectedFromCurrency = "GBP";
            }

            DateTime expectedDate = DateTime.Today;

            // Check the details panel after adding the new row
            Assert.AreEqual(expectedFromCurrency, cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(baseCurrency, cmbToCurrency.GetSelectedString());
            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date);
            Assert.AreEqual(0.0m, txtExchangeRate.NumberValueDecimal);

            // Set a valid exchange rate and save
            txtExchangeRate.NumberValueDecimal = 2.0m;
            mainScreen.SaveChanges();
            Assert.IsFalse(btnSave.Enabled, "The Save button should be disabled after the new row has been saved");

            // Check the row count in the grid
            Assert.AreEqual(3, grdDetails.Rows.Count, "There should be 2 rows in the grid after saving a new row");

            // Even though an inverse row has been added we should still be highlighting the newly added row
            Assert.AreEqual(expectedFromCurrency, cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(baseCurrency, cmbToCurrency.GetSelectedString());
            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date);
            Assert.AreEqual(2.0m, txtExchangeRate.NumberValueDecimal);

            // Now select the inverese row
            SelectRowInGrid(2);

            // Check the details are, in fact, the inverse
            Assert.AreEqual(baseCurrency, cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(expectedFromCurrency, cmbToCurrency.GetSelectedString());
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

            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
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
            TextBox txtTimeEffective = (new TextBoxTester("txtDetailTimeEffectiveFrom", mainScreen)).Properties;

            // Select the bottom row - when we get a new row it should be based on StandardData[1]
            SelectRowInGrid(FAllRowCount, 1);

            // Check that the controls are disabled.  But the exchange rate itself can be edited because the rows are unused.
            Assert.IsFalse(cmbFromCurrency.Enabled);
            Assert.IsFalse(cmbToCurrency.Enabled);
            Assert.IsFalse(dtpEffectiveDate.Enabled);
            Assert.IsTrue(txtExchangeRate.Enabled);
            Assert.IsFalse(btnSave.Enabled, "The Save button should be disabled when the screen is loaded");

            // Click the 'New' button
            btnNew.Click();
            Assert.IsTrue(btnSave.Enabled, "The Save button should be enabled after adding a new row");
            Assert.IsTrue(cmbFromCurrency.Enabled);
            Assert.IsTrue(cmbToCurrency.Enabled);
            Assert.IsTrue(dtpEffectiveDate.Enabled);
            Assert.IsTrue(txtExchangeRate.Enabled);

            // The effective date should be 1st of current month
            DateTime expectedDate = DateTime.Today;

            Assert.AreEqual(EffectiveCurrency(FFromCurrencyId), cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(EffectiveCurrency(FToCurrencyId), cmbToCurrency.GetSelectedString());
            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date);
            Assert.AreEqual(EffectiveRate(), txtExchangeRate.NumberValueDecimal.Value);

            // The row number of the new row should be at row 7
            Assert.AreEqual(FAllRowCount - 1, mainScreen.GetSelectedRowIndex());

            // Change the rate to a new value - more than 10% different
            decimal newRate = 0.667m;
            txtExchangeRate.NumberValueDecimal = newRate;

            // click the 'New' button again - this time the date should be the same but the time should be 7800
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.No);
            };
            btnNew.Click();

            string expectedTime = "02:10";

            // The details should be the same as before except for the new date and the rate being what we just set
            Assert.AreEqual(EffectiveCurrency(FFromCurrencyId), cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(EffectiveCurrency(FToCurrencyId), cmbToCurrency.GetSelectedString());
            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date);
            Assert.AreEqual(expectedTime, txtTimeEffective.Text);
            Assert.AreEqual(newRate, txtExchangeRate.NumberValueDecimal.Value);
            Assert.AreEqual(FAllRowCount - 1, mainScreen.GetSelectedRowIndex());

            // Save the changes and check the number of rows now (we will get our rate alert warning dialog again)
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.No);
            };
            // We have added 2 new rows but the same rate at different times of day
            // When we save the duplicate row will get removed, so we will end up with 10 data rows
            mainScreen.SaveChanges();
            Assert.IsFalse(btnSave.Enabled, "The Save button should be disabled after the new row has been saved");
            Assert.AreEqual(11, grdDetails.Rows.Count, "There should be 10 rows in the grid after saving 2 new rows (but duplicates of each other)");

            mainScreen.Close();
        }

        #endregion

        #region Add row to an empty table but with data in the corporate rate table

        /// <summary>
        /// Test adding rows with no data in the Exchange Rate table, but with possible data in the Corporate Rate table
        /// </summary>
        [Test]
        public void AddRowFromCorporateToEmptyTable()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.SaveChanges();

            // Work out our currency expectations
            string baseCurrency = GetDefaultBaseCurrency();
            string expectedFromCurrency = "USD";

            if (baseCurrency == "USD")
            {
                expectedFromCurrency = "GBP";
            }

            // Create a corporate rate valid from yesterday
            FCorporateDS.CreateCorporateRate(expectedFromCurrency, baseCurrency, DateTime.Today.AddDays(-1), 2.0m);

            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
            mainScreen.Show();

            // Controls
            ButtonTester btnNew = new ButtonTester("btnNew", mainScreen);
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", mainScreen)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", mainScreen)).Properties;
            TtxtPetraDate dtpEffectiveDate = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", mainScreen)).Properties;
            TTxtNumericTextBox txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen)).Properties;

            // Create a new row
            btnNew.Click();

            Assert.AreEqual(expectedFromCurrency, cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(baseCurrency, cmbToCurrency.GetSelectedString());
            Assert.AreEqual(DateTime.Today, dtpEffectiveDate.Date);
            Assert.AreEqual(2.0m, txtExchangeRate.NumberValueDecimal, "The rate should be the rate from the corporate excahnge rate table");

            // change the date to an earlier date - the rate hould revert to 0.0,
            dtpEffectiveDate.Focus();
            dtpEffectiveDate.Date = DateTime.Today.AddYears(-1);
            txtExchangeRate.Focus();
            Assert.AreEqual(0.0m, txtExchangeRate.NumberValueDecimal, "After changing the date the rate should now be 0.0m");

            // Do not leave the rate on 0.0
            txtExchangeRate.NumberValueDecimal = 2.1m;
        }

        #endregion

        #region Load the screen from a table with existing data but with data in the corporate rate table

        /// <summary>
        /// Test adding rows with existing data in the Exchange Rate table, but with possible data in the Corporate Rate table
        /// </summary>
        [Test]
        public void AddRowFromCorporate()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.InsertStandardRows();
            FMainDS.SaveChanges();

            // Create a corporate rate valid from yesterday
            FCorporateDS.CreateCorporateRate("GBP", "USD", DateTime.Today.AddDays(-1), 0.515m);

            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
            mainScreen.Show();

            // Controls
            ButtonTester btnNew = new ButtonTester("btnNew", mainScreen);
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", mainScreen)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", mainScreen)).Properties;
            TtxtPetraDate dtpEffectiveDate = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", mainScreen)).Properties;
            TTxtNumericTextBox txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen)).Properties;

            // Create a new row based on the last row
            SelectRowInGrid(8);
            btnNew.Click();

            Assert.AreEqual(EffectiveCurrency(FFromCurrencyId), cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(EffectiveCurrency(FToCurrencyId), cmbToCurrency.GetSelectedString());
            Assert.AreEqual(DateTime.Today, dtpEffectiveDate.Date);
            Assert.AreEqual(0.515m, txtExchangeRate.NumberValueDecimal, "The rate should be the rate from the corporate exchange rate table");

            // change the date to an earlier date - the rate hould revert to the prior daily rate,
            dtpEffectiveDate.Focus();
            dtpEffectiveDate.Date = DateTime.Today.AddYears(-1);
            txtExchangeRate.Focus();
            Assert.AreEqual(StandardData[Row2DataId(
                                             7),
                                         FRateOfExchangeId], txtExchangeRate.NumberValueDecimal,
                "After changing the date the rate should now be based on previous daily rates");
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

            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
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
            TextBox txtTimeEffective = (new TextBoxTester("txtDetailTimeEffectiveFrom", mainScreen)).Properties;

            // Select the first row in the grid.  New rows should be based on data row 5
            SelectRowInGrid(1, 5);

            // Add three rows
            btnNew.Click();
            txtExchangeRate.NumberValueDecimal = 1.95m;
            btnNew.Click();
            txtExchangeRate.NumberValueDecimal = 1.94m;
            btnNew.Click();
            txtExchangeRate.NumberValueDecimal = 1.93m;

            DateTime expectedDate = DateTime.Today;
            string expectedT1 = "02:00";
            //string expectedT2 = "02:10";
            string expectedT3 = "02:20";

            // Check the data first
            Assert.AreEqual(EffectiveCurrency(FFromCurrencyId), cmbFromCurrency.GetSelectedString());
            Assert.AreEqual(EffectiveCurrency(FToCurrencyId), cmbToCurrency.GetSelectedString());
            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date);
            Assert.AreEqual(expectedT3, txtTimeEffective.Text);
            Assert.AreEqual(1.93m, txtExchangeRate.NumberValueDecimal.Value);
            Assert.AreEqual(3, mainScreen.GetSelectedRowIndex());

            // Focus on the from currency, then change it to 'BEF'
            cmbFromCurrency.Focus();
            cmbFromCurrency.SetSelectedString(STANDARD_TEST_CURRENCY);
            cmbToCurrency.Focus();

            // Now check the date and rate.  Time should be back to O2:00 and rate should be 0.00 because this currency has never been used
            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date.Value);
            Assert.AreEqual(expectedT1, txtTimeEffective.Text);
            Assert.AreEqual(0.0m, txtExchangeRate.NumberValueDecimal);
            Assert.AreEqual(1, mainScreen.GetSelectedRowIndex());

            // Reset the currency and confirm we go back to where we were
            cmbFromCurrency.Focus();
            cmbFromCurrency.SetSelectedString(EffectiveCurrency(FFromCurrencyId));
            cmbToCurrency.Focus();

            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date.Value);
            Assert.AreEqual(expectedT3, txtTimeEffective.Text);
            Assert.AreEqual(1.94m, txtExchangeRate.NumberValueDecimal);
            Assert.AreEqual(3, mainScreen.GetSelectedRowIndex());

            // Repeat for the To currency
            cmbToCurrency.Focus();
            cmbToCurrency.SetSelectedString(STANDARD_TEST_CURRENCY);
            dtpEffectiveDate.Focus();

            // Now check the date and rate.  Date should be back to this month and rate should be 0.00 because this currency has never been used
            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date.Value);
            Assert.AreEqual(expectedT1, txtTimeEffective.Text);
            Assert.AreEqual(0.0m, txtExchangeRate.NumberValueDecimal);
            Assert.AreEqual(1, mainScreen.GetSelectedRowIndex());

            // Reset the currency and confirm we go back to where we were
            cmbToCurrency.Focus();
            cmbToCurrency.SetSelectedString(EffectiveCurrency(FToCurrencyId));
            dtpEffectiveDate.Focus();

            Assert.AreEqual(expectedDate, dtpEffectiveDate.Date.Value);
            Assert.AreEqual(expectedT3, txtTimeEffective.Text);
            Assert.AreEqual(1.94m, txtExchangeRate.NumberValueDecimal);
            Assert.AreEqual(3, mainScreen.GetSelectedRowIndex());

            // That is a duplicate row now
            txtExchangeRate.NumberValueDecimal = 1.944m;

            // Finally check what happens when editing the date
            SelectRowInGrid(5);
            txtExchangeRate.NumberValueDecimal = 1.95m;             // 02:00 today
            FixUnvalidatedChanges();
            SelectRowInGrid(4);
            txtExchangeRate.NumberValueDecimal = 1.96m;
            dtpEffectiveDate.Date = DateTime.Today.AddDays(1);      // 02.00 tomorrow
            FixUnvalidatedChanges();
            SelectRowInGrid(3);
            txtExchangeRate.NumberValueDecimal = 1.97m;
            dtpEffectiveDate.Date = DateTime.Today.AddDays(2);     // 02:00 next day
            FixUnvalidatedChanges();

            SelectRowInGrid(5);
            Assert.AreEqual(1.95m, txtExchangeRate.NumberValueDecimal);
            dtpEffectiveDate.Focus();
            dtpEffectiveDate.Date = DateTime.Today.AddDays(10);
            grdDetails.Focus();
            Assert.AreEqual(1.97m, txtExchangeRate.NumberValueDecimal);
            Assert.AreEqual("02:00", txtTimeEffective.Text);

            mainScreen.SaveChanges();
            mainScreen.Close();
        }

        #endregion

        #region Delete Rows (including saved but not used rows)

        /// <summary>
        /// Add rows and then delete them.  Also delete rows that have been saved but are not used.
        /// </summary>
        [Test]
        public void DeleteRows()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.InsertStandardRows();
            FMainDS.SaveChanges();

            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
            mainScreen.Show();

            // Toolstrip
            ToolStripButtonTester btnSaveTester = new ToolStripButtonTester("tbbSave", mainScreen);
            ButtonWithFocusTester btnNewTester = new ButtonWithFocusTester("btnNew", mainScreen);
            ButtonWithFocusTester btnDeleteTester = new ButtonWithFocusTester("btnDelete", mainScreen);
            //ButtonWithFocusTester btnEnableEdit = new ButtonWithFocusTester("btnEnableEdit", mainScreen);
            TSgrdDataGridPagedTester grdTester = new TSgrdDataGridPagedTester("grdDetails", mainScreen);
            TSgrdDataGrid grdDetails = (TSgrdDataGrid)grdTester.Properties;
            TTxtNumericTextBox txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen)).Properties;
            TtxtPetraDate txtDateEffective = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", mainScreen)).Properties;
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", mainScreen)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", mainScreen)).Properties;

            // All rows in grid should be deletable because they are unused
            Assert.AreEqual(9, grdDetails.Rows.Count);
            Assert.IsTrue(btnDeleteTester.Properties.Enabled);
            Assert.IsTrue(txtDateEffective.Date.Value.Year < 1910 || txtDateEffective.Date.Value.Year > 2980);

            // Create 3 new rows
            btnNewTester.Click();
            txtExchangeRate.NumberValueDecimal = 1.95m;
            btnNewTester.Click();
            txtExchangeRate.NumberValueDecimal = 1.94m;
            btnNewTester.Click();
            txtExchangeRate.NumberValueDecimal = 1.93m;

            Assert.AreEqual(12, grdDetails.Rows.Count);
            Assert.AreEqual(3, mainScreen.GetSelectedRowIndex());

            // New rows should be deletable
            Assert.IsTrue(btnDeleteTester.Properties.Enabled);
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.Yes);
            };
            btnDeleteTester.Click();
            Assert.IsTrue(btnDeleteTester.Properties.Enabled);
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.Yes);
            };
            btnDeleteTester.Click();
            Assert.IsTrue(btnDeleteTester.Properties.Enabled);
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.Yes);
            };
            btnDeleteTester.Click();

            Assert.IsFalse(cmbFromCurrency.Enabled);
            Assert.IsFalse(cmbToCurrency.Enabled);
            Assert.IsFalse(txtDateEffective.Enabled);
            Assert.IsTrue(txtDateEffective.Date.Value.Year < 1910 || txtDateEffective.Date.Value.Year > 2980);

            Assert.AreEqual(3, mainScreen.GetSelectedRowIndex());

            // Now we should be back to being able to delete a unused row
            Assert.IsTrue(btnDeleteTester.Properties.Enabled);
            Assert.AreEqual(9, grdDetails.Rows.Count);
            Assert.IsTrue(txtDateEffective.Date.Value.Year < 1910 || txtDateEffective.Date.Value.Year > 2980);

            // Activate deletion of saved rows
            //btnEnableEdit.Click();
            Assert.IsTrue(txtDateEffective.Date.Value.Year < 1910 || txtDateEffective.Date.Value.Year > 2980);

            // Now we should be able to delete the row we could not delete before
            Assert.IsTrue(btnDeleteTester.Properties.Enabled);

            // Change to the first row
            Assert.AreEqual(3, mainScreen.GetSelectedRowIndex());
            Assert.IsTrue(txtDateEffective.Date.Value.Year < 1910 || txtDateEffective.Date.Value.Year > 2980);
            SelectRowInGrid(1);
            Assert.IsTrue(txtDateEffective.Date.Value.Year < 1910 || txtDateEffective.Date.Value.Year > 2980);
            Assert.AreEqual(1, mainScreen.GetSelectedRowIndex());

            // So now we should have our 2 rows far in the future at the top which are not used anywhere
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.Yes);
            };
            btnDeleteTester.Click();
            Assert.IsTrue(txtDateEffective.Date.Value.Year < 1910 || txtDateEffective.Date.Value.Year > 2980);
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.Yes);
            };
            btnDeleteTester.Click();
            Assert.IsTrue(txtDateEffective.Date.Value.Year < 1910 || txtDateEffective.Date.Value.Year > 2980);

            // Should still be on row 1 with 7 grid rows now that 2 have gone
            Assert.AreEqual(1, mainScreen.GetSelectedRowIndex());
            Assert.IsTrue(btnDeleteTester.Properties.Enabled);
            Assert.AreEqual(7, grdDetails.Rows.Count);

            // Delete rows starting at the bottom
            SelectRowInGrid(6);
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.Yes);
            };
            btnDeleteTester.Click();
            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.Yes);
            };
            btnDeleteTester.Click();

            Assert.AreEqual(4, mainScreen.GetSelectedRowIndex());
            Assert.IsTrue(btnDeleteTester.Properties.Enabled);
            Assert.AreEqual(5, grdDetails.Rows.Count);

            // Save the new settings - deleting does not remove inverse rows when saved
            Assert.IsTrue(btnSaveTester.Properties.Enabled);
            btnSaveTester.Click();
            Assert.IsFalse(btnSaveTester.Properties.Enabled);
            Assert.AreEqual(5, grdDetails.Rows.Count);
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

            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
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

            // Check that Invert is enabled, then add one new row and check it is also enabled
            Assert.IsTrue(btnInvert.Properties.Enabled);
            btnNew.Click();
            Assert.IsTrue(btnInvert.Properties.Enabled);
            txtExchangeRate.NumberValueDecimal = 2.0m;
            btnInvert.Click();
            Assert.AreEqual(0.5m, txtExchangeRate.NumberValueDecimal);
            btnInvert.Click();
            Assert.AreEqual(2.0m, txtExchangeRate.NumberValueDecimal);

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

            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
            mainScreen.Show();

            // Save and New buttons
            ToolStripButtonTester btnSave = new ToolStripButtonTester("tbbSave", mainScreen);
            ButtonTester btnNew = new ButtonTester("btnNew", mainScreen);
            TTxtNumericTextBox txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen)).Properties;

            // Add new row, save and close
            btnNew.Click();
            txtExchangeRate.NumberValueDecimal = 10m;
            btnSave.Click();
            mainScreen.Close();

            // Create the screen a second time
            TFrmSetupDailyExchangeRate mainScreen2 = new TFrmSetupDailyExchangeRate(null);
            mainScreen2.Show();

            // make sure the data really got saved
            TSgrdDataGrid grdDetails = (TSgrdDataGrid)(new TSgrdDataGridPagedTester("grdDetails", mainScreen2).Properties);
            Assert.AreEqual(3, grdDetails.Rows.Count);

            btnNew = new ButtonTester("btnNew", mainScreen2);
            txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen2)).Properties;

            // Add another row, but this time close without saving
            btnNew.Click();
            txtExchangeRate.NumberValueDecimal = 10.1m;

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.No);
            };

            mainScreen2.Close();

            // Create the screen a third time
            TFrmSetupDailyExchangeRate mainScreen3 = new TFrmSetupDailyExchangeRate(null);
            mainScreen3.Show();

            // make sure the data did not get saved
            grdDetails = (TSgrdDataGrid)(new TSgrdDataGridPagedTester("grdDetails", mainScreen3).Properties);
            Assert.AreEqual(3, grdDetails.Rows.Count);

            btnNew = new ButtonTester("btnNew", mainScreen3);
            txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen3)).Properties;

            // Add another row, but this time close AND save
            btnNew.Click();
            txtExchangeRate.NumberValueDecimal = 10.1m;

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                tester.SendCommand(MessageBoxTester.Command.Yes);
            };

            mainScreen3.Close();

            // Create the screen a fourth time!
            TFrmSetupDailyExchangeRate mainScreen4 = new TFrmSetupDailyExchangeRate(null);
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

            RunTestImport("daily-csv/GoodImport-NoTime.csv", ",", results, out resultText, out firstResultCode);
            Assert.AreEqual(String.Empty, resultText, "Errors during import...");
            Assert.AreEqual(8, FMainDS.ADailyExchangeRate.Rows.Count, "Wrong number of rows after successful import");

            FMainDS.DeleteAllRows();
            RunTestImport("daily-csv/GoodImport-WithTime.csv", ",", results, out resultText, out firstResultCode);
            Assert.AreEqual(String.Empty, resultText, "Errors during import...");
            Assert.AreEqual(8, FMainDS.ADailyExchangeRate.Rows.Count, "Wrong number of rows after successful import");

            FMainDS.DeleteAllRows();
            RunTestImport("daily-csv/BadCurrencyImport.csv", ",", results, out resultText, out firstResultCode);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(CommonErrorCodes.ERR_INCONGRUOUSSTRINGS, firstResultCode);

            FMainDS.DeleteAllRows();
            RunTestImport("daily-csv/BadDateImport.csv", ",", results, out resultText, out firstResultCode);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(CommonErrorCodes.ERR_INVALIDDATE, firstResultCode);

            FMainDS.DeleteAllRows();
            RunTestImport("daily-csv/BadRateImport.csv", ",", results, out resultText, out firstResultCode);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(CommonErrorCodes.ERR_INVALIDNUMBER, firstResultCode);

            FMainDS.DeleteAllRows();
            RunTestImport("daily-csv/BadTimeImport.csv", "\t", results, out resultText, out firstResultCode);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(CommonErrorCodes.ERR_INVALIDINTEGERTIME, firstResultCode);

            // Run the test(s) that have duplicates
            FMainDS.DeleteAllRows();
            FMainDS.InsertStandardRows();
            RunTestImport("daily-csv/GoodImport-WithDuplicates.csv", ",", results, out resultText, out firstResultCode);
            Assert.AreEqual(String.Empty, resultText, "Errors during import...");
            Assert.AreEqual(12, FMainDS.ADailyExchangeRate.Rows.Count, "Wrong number of rows after successful import");

            // And Headers
            FMainDS.DeleteAllRows();
            RunTestImport("daily-csv/GoodImport-WithTimeAndHeader.csv", ",", results, out resultText, out firstResultCode);
            Assert.AreEqual(String.Empty, resultText, "Errors during import...");
            Assert.AreEqual(8, FMainDS.ADailyExchangeRate.Rows.Count, "Wrong number of rows after successful import");

            // Test a date/rate only file - this is tab separated
            FMainDS.DeleteAllRows();
            RunTestImport("daily-csv/USD_EUR.csv", "\t", results, out resultText, out firstResultCode);
            Assert.AreEqual(String.Empty, resultText, "Errors during import...");
            Assert.AreEqual(8, FMainDS.ADailyExchangeRate.Rows.Count, "Wrong number of rows after successful import");

            // Test a file with its own inverses
            FMainDS.DeleteAllRows();
            RunTestImport("daily-csv/GoodImport-WithInverses.csv", ",", results, out resultText, out firstResultCode);
            Assert.AreEqual(String.Empty, resultText, "Errors during import...");
            Assert.AreEqual(4, FMainDS.ADailyExchangeRate.Rows.Count, "Wrong number of rows after successful import");
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

            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
            mainScreen.Show();

            ButtonTester btnNew = new ButtonTester("btnNew", mainScreen);
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", mainScreen)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", mainScreen)).Properties;
            TtxtPetraDate dtpEffectiveDate = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", mainScreen)).Properties;
            TTxtNumericTextBox txtExchangeRate = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", mainScreen)).Properties;
            TextBox txtTimeEffective = (new TextBoxTester("txtDetailTimeEffectiveFrom", mainScreen)).Properties;

            btnNew.Click();

            string dlgText = String.Empty;
            bool dlgDisplayed = false;

            // Set up some bad entries
            // We already have an invalid rate (it will be 0.0)

            // Click the New button and discover what validation errors we have
            // Note - we do not put assert's inside the delegate because we want the dialog to close.
            //   If the dialog is left hanging this might do bad stuff to automated testing
            // Also note that dialogs with long text content get their text clipped in Tester.Text.
            // For that reason we are going to have to run three tests here instead of just one to do them all
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
            Assert.IsTrue(dlgText.Contains(CommonErrorCodes.ERR_INVALIDNUMBER), dlgText);

            // Set up second bad entry
            txtExchangeRate.NumberValueDecimal = 2.0m;
            string prevDateText = dtpEffectiveDate.Text;
            dtpEffectiveDate.Text = "";
            dlgDisplayed = false;
            dlgText = String.Empty;

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                dlgText = tester.Text;
                dlgDisplayed = true;
                tester.SendCommand(MessageBoxTester.Command.OK);
            };

            btnNew.Click();

            Assert.IsTrue(dlgDisplayed);
            Assert.IsTrue(dlgText.Contains(CommonErrorCodes.ERR_NOUNDEFINEDDATE), dlgText);

            // Set up third bad entry
            dtpEffectiveDate.Text = prevDateText;
            txtTimeEffective.Text = "09:80";
            dlgDisplayed = false;
            dlgText = String.Empty;

            ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
            {
                MessageBoxTester tester = new MessageBoxTester(hWnd);
                dlgText = tester.Text;
                dlgDisplayed = true;
                tester.SendCommand(MessageBoxTester.Command.OK);
            };

            btnNew.Click();

            Assert.IsTrue(dlgDisplayed);
            Assert.IsTrue(dlgText.Contains(CommonErrorCodes.ERR_INVALIDINTEGERTIME), dlgText);

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
            out string AFirstResultCode)
        {
            string TestFile = Path.GetFullPath(TAppSettingsManager.GetValue("Testing.Path") + "/lib/MFinance/ExchangeRates/" + AFileName);

            Assert.IsTrue(File.Exists(TestFile), "File does not exist: " + TestFile);

            AResults.Clear();
            TImportExchangeRates.ImportCurrencyExRates(FMainDS.ADailyExchangeRate, TestFile, ACSVSeparator, "Daily", AResults);

            AResultText = String.Empty;

            for (int i = 0; i < AResults.Count; i++)
            {
                AResultText += String.Format("{0}: {1}{2}", i.ToString(), AResults[i].ResultText, Environment.NewLine);
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