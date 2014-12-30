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
    /// Partial Class for testing the Daily Exchange rate screen as a Modal Dialog
    public partial class TDailyExchangeRateTest
    {
        #region Help on NUnit Testing of Modal Dialogs

        /// <summary>
        /// Performing tests on a modal dialog needs a different approach from tests on a simple class or a modeless screen.
        /// Here are some tips to help design your test.
        ///
        /// NUnitForms has a recommended method for handling modal dialogs.  It involves specifying a one-shot delegate to that gets invoked
        /// when the modal screen is displayed.  This works well for Message Boxes and the like but I could not get it to work with my
        /// daily exchange rate screen.  Fortunately NUnitForms previously used a different method for working with dialogs, and although it
        /// has been marked as [Obsolete], it still works!  So the modal tests written here all use this 'old' method successfully.
        ///
        /// The [Test] code involves creating a public void method as usual.  In this method you
        ///    perform some initialisation
        ///    call the NUnitForms method:  ExpectModal("AFormName", AHandlerMethodName)
        ///    instantiate your modal form: DialogResult result = myDialog.ShowModal()
        ///    test the return values after the dialog has closed.
        ///
        /// Then you create another public void method (AHandlerMethodName()) and in there you do the Assert's and button pushes to test the
        /// behaviour of the dialog itself.
        ///
        /// There are some things you need to know about coding this dialog handler:
        /// * Problems with the Activation Event (RunOnceOnActivation). On Windows Jenkins, this is not run at the right time.
        ///     We are now calling the method RunBeforeActivation() before ShowDialog inside the dialog.
        /// * You need to be sure that you close the dialog so that control returns to the [Test] method.
        ///     If the dialog does not close the whole Unit Test will hang indefinitely.  The effect of this requirement is that, if any of
        ///     your Assert tests fail the dialog remains open and needs you to manually close it.  (Possibly this is a Debug build behaviour
        ///     and a release build of the DLL may behave differently, but I have not checked that.)
        ///
        ///     Your code will probably have a few button clicks and a number of Assert's and then will close the dialog with a button click.
        ///     The requirement to ensure that the dialog closes even if an Assert fires means that you need to enclose all the actions in the dialog
        ///     inside a try/catch block, so that you can close the dialog in the catch or a finally block.
        /// * In order to return the Assert to the main [Test] routine, your catch block needs to save the exception information in a global variable.
        ///     The catch block then needs to return a different DialogResult (I use Abort).  Then the main [Test] method knows whether the dialog
        ///     closed with an error or not.
        ///
        /// If you look at the code here you will see that I use a couple of helper methods to
        /// * Call the modal handler method.  Since this method is marked as [Obsolete] I use a #pragma statement to turn off the resultant compiler warning.
        /// * Handle the exception message from the try/catch block.  This formats the message ready for the main method to use in an Assert.Fail().
        ///    Note that the message is constructed differently depending on whether the exception arose from an Assert or from a generic error in your code.
        /// </summary>

        #endregion

        // Special variables used in Modal Dialog testing
        const string FModalFormName = "TFrmSetupDailyExchangeRate";                 // The name of the modal form being tested
        const int STANDARD_TEST_LEDGER_NUMBER = 9997;                               // On modal form tests we create a special ledger #9997 for test use
        const string STANDARD_TEST_CURRENCY = "BEF";                                // On modal form tests our ledger currency is always BEF
        const decimal STANDARD_RATE_OF_EXCHANGE = 1.50m;                            // A rate of exchange we use in testing

        /// <summary>
        /// An effective date we often use in testing.  It is not the earliest date in the table, but it is earlier than 'Today'.
        /// The standard modal data also has two dates much later than 'Today' (the year 2999 actually!)
        /// </summary>
        private DateTime FStandardEffectiveDate = new DateTime(2000, 12, 31);


        // The class variable we use to pass exceptions in the dialog back to this test class thread
        private String FModalAssertResult = String.Empty;

        #region GetRateForDate test (does not show screen)

        /// <summary>
        /// Actually this test does not show a screen at all.  It is neither modal nor modeless
        /// We include it here because it makes use of our modal data to return a value without a GUI.
        /// It is used by Revaluation
        /// </summary>
        [Test]
        public void GetRateForDate()
        {
            // First test is with empty data
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.SaveChanges();
            FLedgerDS.CreateTestLedger();

            // define our working date range
            DateTime dtStart = new DateTime(2000, 01, 01);
            DateTime dtEnd = new DateTime(2000, 12, 31);

            // First test is with empty data - should return 1.0m
            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
            decimal result = mainScreen.GetLastExchangeValueOfInterval(STANDARD_TEST_LEDGER_NUMBER, dtStart, dtEnd, "GBP");
            Assert.AreEqual(1.0m, result, "The result should be 1.0m when the table contains no data");

            // Repeat test with data but outside the date range - again should return 1.0m
            FMainDS.InsertStandardModalRows();
            FMainDS.SaveChanges();

            mainScreen = new TFrmSetupDailyExchangeRate(null);
            result = mainScreen.GetLastExchangeValueOfInterval(STANDARD_TEST_LEDGER_NUMBER, dtStart, dtEnd, "GBP");
            Assert.AreEqual(1.0m, result, "The result should be 1.0m because there is no data in the date range");

            // Repeat again with data inside the range
            FMainDS.AddARow("GBP", STANDARD_TEST_CURRENCY, new DateTime(2000, 6, 1), 2.0m);
            FMainDS.AddARow("GBP", STANDARD_TEST_CURRENCY, new DateTime(2000, 6, 10), 2.05m);
            FMainDS.AddARow("GBP", STANDARD_TEST_CURRENCY, new DateTime(2000, 6, 30), 2.15m);           // This is the latest
            FMainDS.AddARow("GBP", STANDARD_TEST_CURRENCY, new DateTime(2000, 6, 20), 2.10m);
            FMainDS.SaveChanges();

            mainScreen = new TFrmSetupDailyExchangeRate(null);
            result = mainScreen.GetLastExchangeValueOfInterval(STANDARD_TEST_LEDGER_NUMBER, dtStart, dtEnd, "GBP");
            Assert.AreEqual(2.15m, result);
        }

        #endregion

        #region Load screen as a Modal dialog from Empty Table

        /// <summary>
        /// Run the screen modally on an empty table
        /// </summary>
        [Test]
        public void LoadModalEmptyTable()
        {
            // Initialise data - create an empty table and our test ledger
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.SaveChanges();
            FLedgerDS.CreateTestLedger();

            // variables to hold the dialog result output
            decimal selectedRate;
            DateTime selectedDate;
            int selectedTime;

            // Open the screen modally on our test ledger using a 'from' currency of GBP
            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);

            DialogBoxHandler = delegate(string name, IntPtr hWnd)
            {
                LoadModalEmptyTableHandler();
            };

            DialogResult dlgResult = mainScreen.ShowDialog(STANDARD_TEST_LEDGER_NUMBER,
                FStandardEffectiveDate,
                "GBP",
                1.0m,
                out selectedRate,
                out selectedDate,
                out selectedTime);

            // Check the result for any assertions
            if (dlgResult == DialogResult.Abort)
            {
                Assert.Fail(FModalAssertResult);
            }

            // Check we returned the correct data to the caller
            Assert.AreEqual(DialogResult.OK, dlgResult);
            Assert.AreEqual(STANDARD_RATE_OF_EXCHANGE, selectedRate);
            Assert.AreEqual(FStandardEffectiveDate, selectedDate);
            Assert.AreEqual(7200, selectedTime);

            // Check we did also save the result
            FMainDS.LoadAll();
            ADailyExchangeRateRow row =
                (ADailyExchangeRateRow)FMainDS.ADailyExchangeRate.Rows.Find(new object[] { "GBP", STANDARD_TEST_CURRENCY, FStandardEffectiveDate,
                                                                                           7200 });
            Assert.IsNotNull(row, "The selected exchange rate was not saved");
            Assert.AreEqual(STANDARD_RATE_OF_EXCHANGE, row.RateOfExchange);
        }

        /// <summary>
        /// Handler for ModalEmptyTable test
        /// </summary>
        public void LoadModalEmptyTableHandler()
        {
            // Controls
            ButtonTester btnNewTester = new ButtonTester("btnNew", FModalFormName);
            ButtonTester btnCloseTester = new ButtonTester("btnClose", FModalFormName);
            ButtonTester btnCancelTester = new ButtonTester("btnCancel", FModalFormName);

            Button btnNew = btnNewTester.Properties;
            Button btnClose = btnCloseTester.Properties;
            Button btnDelete = (new ButtonTester("btnDelete", FModalFormName)).Properties;
            TSgrdDataGrid grdDetails = (TSgrdDataGrid)((new TSgrdDataGridPagedTester("grdDetails", FModalFormName)).Properties);
            Panel pnlDetails = (new PanelTester("pnlDetails", FModalFormName)).Properties;
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", FModalFormName)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", FModalFormName)).Properties;
            TtxtPetraDate dtpDateEffective = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", FModalFormName)).Properties;
            TTxtNumericTextBox txtRateOfExchange = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", FModalFormName)).Properties;

            // These should be the states on an empty modal screen (as loaded)
            try
            {
                Assert.IsFalse(btnClose.Enabled);
                Assert.IsFalse(btnDelete.Enabled);
                Assert.IsFalse(dtpDateEffective.Date.HasValue);
                Assert.IsFalse(pnlDetails.Enabled);
                Assert.AreEqual(1, grdDetails.Rows.Count);

                // Now create a new row
                btnNewTester.Click();

                // These should be the states after adding a new row
                Assert.AreEqual("GBP", cmbFromCurrency.GetSelectedString());                    // GBP passed in as a ShowDialog parameter
                Assert.AreEqual(STANDARD_TEST_CURRENCY, cmbToCurrency.GetSelectedString());
                Assert.AreEqual(FStandardEffectiveDate, dtpDateEffective.Date);
                Assert.IsFalse(cmbFromCurrency.Enabled);
                Assert.IsFalse(cmbToCurrency.Enabled);
                Assert.IsTrue(btnClose.Enabled);
                Assert.IsTrue(btnDelete.Enabled);
                Assert.IsTrue(pnlDetails.Enabled);
                Assert.AreEqual(2, grdDetails.Rows.Count);

                // Rate should be 0.0m - we will need to set it to something else
                Assert.AreEqual(0.0m, txtRateOfExchange.NumberValueDecimal);
                txtRateOfExchange.NumberValueDecimal = STANDARD_RATE_OF_EXCHANGE;

                // Save this as our rate and quit
                btnCloseTester.Click();
            }
            catch (Exception ex)
            {
                // Handle the exception and abort without saving
                HandleModalException(ex);
                btnCancelTester.Properties.DialogResult = DialogResult.Abort;
                btnCancelTester.Click();
            }
        }

        #endregion

        #region Load screen as a Modal dialog from table with existing data

        /// <summary>
        /// Run the screen modally on an existing table
        /// </summary>
        [Test]
        public void LoadModalTableWithData()
        {
            // Initialse data
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.InsertStandardModalRows();
            FMainDS.SaveChanges();
            FLedgerDS.CreateTestLedger();

            decimal selectedRate;
            DateTime selectedDate;
            int selectedTime;

            // Open the screen modally on our test ledger and a from currency of GBP
            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);

            DialogBoxHandler = delegate(string name, IntPtr hWnd)
            {
                LoadModalTableHandler();
            };

            DialogResult dlgResult = mainScreen.ShowDialog(STANDARD_TEST_LEDGER_NUMBER,
                FStandardEffectiveDate,
                "GBP",
                1.0m,
                out selectedRate,
                out selectedDate,
                out selectedTime);

            if (dlgResult == DialogResult.Abort)
            {
                Assert.Fail(FModalAssertResult);
            }

            Assert.AreEqual(DialogResult.OK, dlgResult);
            Assert.AreEqual(new DateTime(1900, 7, 1), selectedDate);
            Assert.AreEqual(0.51m, selectedRate);
        }

        /// <summary>
        /// Handler for LoadModalTableWithData test
        /// </summary>
        public void LoadModalTableHandler()
        {
            // Controls
            ButtonTester btnCloseTester = new ButtonTester("btnClose", FModalFormName);
            ButtonTester btnCancelTester = new ButtonTester("btnCancel", FModalFormName);

            Button btnClose = btnCloseTester.Properties;
            Button btnDelete = (new ButtonTester("btnDelete", FModalFormName)).Properties;
            TSgrdDataGrid grdDetails = (TSgrdDataGrid)((new TSgrdDataGridPagedTester("grdDetails", FModalFormName)).Properties);
            Panel pnlDetails = (new PanelTester("pnlDetails", FModalFormName)).Properties;
            TCmbAutoPopulated cmbFromCurrency = (new TCmbAutoPopulatedTester("cmbDetailFromCurrencyCode", FModalFormName)).Properties;
            TCmbAutoPopulated cmbToCurrency = (new TCmbAutoPopulatedTester("cmbDetailToCurrencyCode", FModalFormName)).Properties;
            TtxtPetraDate dtpDateEffective = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", FModalFormName)).Properties;
            TTxtNumericTextBox txtRateOfExchange = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", FModalFormName)).Properties;

            // These should be the states given the standard modal data we loaded
            try
            {
                Assert.AreEqual("GBP", cmbFromCurrency.GetSelectedString());                    // GBP passed in as a ShowDialog parameter
                Assert.AreEqual(STANDARD_TEST_CURRENCY, cmbToCurrency.GetSelectedString());     // The ledger currency for the ledger passed in as parameter
                Assert.AreEqual(new DateTime(1900, 7, 1), dtpDateEffective.Date);
                Assert.AreEqual(0.51m, txtRateOfExchange.NumberValueDecimal);
                Assert.IsFalse(cmbFromCurrency.Enabled);
                Assert.IsFalse(cmbToCurrency.Enabled);
                Assert.IsTrue(btnClose.Enabled);
                Assert.IsTrue(btnClose.Visible);
                Assert.IsTrue(btnCancelTester.Properties.Visible);
                Assert.IsTrue(btnDelete.Enabled);
                Assert.IsTrue(pnlDetails.Enabled);
                Assert.AreEqual(3, grdDetails.Rows.Count);

                // select this item and close
                btnCloseTester.Click();
            }
            catch (Exception ex)
            {
                // Handle the exception and abort without saving
                HandleModalException(ex);
                btnCancelTester.Properties.DialogResult = DialogResult.Abort;
                btnCancelTester.Click();
            }
        }

        #endregion

        #region Modal Validation

        /// <summary>
        /// Test for usage of a specific Gift Batch
        /// </summary>
        [Test]
        public void ModalValidation()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.SaveChanges();
            FLedgerDS.CreateTestLedger();

            decimal selectedRate;
            DateTime selectedDate;
            int selectedTime;

            // Open the screen modally on our test ledger and a from currency of GBP
            // This test sets up a date range
            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);

            DialogBoxHandler = delegate(string name, IntPtr hWnd)
            {
                ModalValidationHandler();
            };

            DialogResult dlgResult = mainScreen.ShowDialog(STANDARD_TEST_LEDGER_NUMBER, FStandardEffectiveDate.AddDays(
                    -10), FStandardEffectiveDate, "GBP", 1.0m, out selectedRate, out selectedDate, out selectedTime);

            if (dlgResult == DialogResult.Abort)
            {
                Assert.Fail(FModalAssertResult);
            }

            // Make sure we did save
            Assert.AreEqual(DialogResult.OK, dlgResult);
            Assert.IsFalse((new ToolStripButtonTester("tbbSave", mainScreen)).Properties.Enabled);
            FMainDS.LoadAll();
            Assert.AreEqual(2, FMainDS.ADailyExchangeRate.Rows.Count, "The data table should have 2 rows after a successful save operation");
        }

        /// <summary>
        /// Handler for the GiftBatchUsage test
        /// </summary>
        public void ModalValidationHandler()
        {
            // Controls
            ButtonTester btnNewTester = new ButtonTester("btnNew", FModalFormName);
            ButtonTester btnCloseTester = new ButtonTester("btnClose", FModalFormName);
            ButtonTester btnCancelTester = new ButtonTester("btnCancel", FModalFormName);

            TtxtPetraDate dtpDateEffective = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", FModalFormName)).Properties;
            TTxtNumericTextBox txtRateOfExchange = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", FModalFormName)).Properties;

            try
            {
                // Create a new row and set the rate
                btnNewTester.Click();
                txtRateOfExchange.NumberValueDecimal = 2.0m;
                string dlgText = String.Empty;
                bool dlgDisplayed = false;

                // Set up a popup handler
                ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
                {
                    MessageBoxTester tester = new MessageBoxTester(hWnd);
                    dlgText = tester.Text;
                    dlgDisplayed = true;
                    tester.SendCommand(MessageBoxTester.Command.OK);
                };

                // Try setting the date to the day after the end and then clicking New
                dtpDateEffective.Date = FStandardEffectiveDate.AddDays(1.0);
                btnNewTester.Click();
                Assert.IsTrue(dlgDisplayed, "The date chosen for the test should have raised a validation errror");
                Assert.IsTrue(dlgText.Contains(CommonErrorCodes.ERR_DATENOTINDATERANGE));

                // Set up another popup handler
                dlgText = String.Empty;
                dlgDisplayed = false;
                ModalFormHandler = delegate(string name, IntPtr hWnd, Form form)
                {
                    MessageBoxTester tester = new MessageBoxTester(hWnd);
                    dlgText = tester.Text;
                    dlgDisplayed = true;
                    tester.SendCommand(MessageBoxTester.Command.OK);
                };

                // Try setting the date to the day before the start and then clicking New
                dtpDateEffective.Date = FStandardEffectiveDate.AddDays(-11.0);
                btnNewTester.Click();
                Assert.IsTrue(dlgDisplayed, "The date chosen for the test should have raised a validation errror");
                Assert.IsTrue(dlgText.Contains(CommonErrorCodes.ERR_DATENOTINDATERANGE));

                // Set the date to a valid date and close (and save)
                dtpDateEffective.Date = FStandardEffectiveDate;
                btnCloseTester.Click();
            }
            catch (Exception ex)
            {
                HandleModalException(ex);
                btnCancelTester.Properties.DialogResult = DialogResult.Abort;
                btnCancelTester.Click();
            }
        }

        #endregion

        #region GiftBatch and Journal rate usage test

        /// <summary>
        /// Test for usage of a specific rate in Journal and Gift Batch
        /// </summary>
        [Test]
        public void ExchangeRateUsage()
        {
            FMainDS.LoadAll();
            FMainDS.DeleteAllRows();
            FMainDS.InsertStandardModalRows();
            FMainDS.SaveChanges();

            FGiftAndJournal.InitialiseData("load-data.sql");

            decimal selectedRate;
            DateTime selectedDate;
            int selectedTime;

            // Open the screen modally on our test ledger and a from currency of GBP
            TFrmSetupDailyExchangeRate mainScreen = new TFrmSetupDailyExchangeRate(null);
            mainScreen.ShowUsedRates();

            DialogBoxHandler = delegate(string name, IntPtr hWnd)
            {
                ExchangeRateUsageHandler();
            };

            DialogResult dlgResult = mainScreen.ShowDialog(STANDARD_TEST_LEDGER_NUMBER,
                FStandardEffectiveDate,
                "GBP",
                1.0m,
                out selectedRate,
                out selectedDate,
                out selectedTime);

            if (dlgResult == DialogResult.Abort)
            {
                Assert.Fail(FModalAssertResult);
            }

            Assert.AreEqual(DialogResult.OK, dlgResult);
        }

        /// <summary>
        /// Handler for the ExchangeRateUsage1 test
        /// </summary>
        public void ExchangeRateUsageHandler()
        {
            FormTester formTester = new FormTester(FModalFormName);

            // Controls
            //ButtonTester btnNewTester = new ButtonTester("btnNew", FModalFormName);
            ButtonTester btnCloseTester = new ButtonTester("btnClose", FModalFormName);
            ButtonTester btnCancelTester = new ButtonTester("btnCancel", FModalFormName);

            TtxtPetraDate dtpDateEffective = (new TTxtPetraDateTester("dtpDetailDateEffectiveFrom", FModalFormName)).Properties;
            TTxtNumericTextBox txtRateOfExchange = (new TTxtNumericTextBoxTester("txtDetailRateOfExchange", FModalFormName)).Properties;

            try
            {
                for (int i = 7; i > 0; i--)
                {
                    SelectRowInGrid(i);
                    string Usage = ((TFrmSetupDailyExchangeRate)formTester.Properties).Usage;
                    Console.WriteLine("Grid row {0}: rate: {1}: {2}", i, txtRateOfExchange.NumberValueDecimal.Value.ToString(), Usage);
                    // Usage is of form "Ledger:{0} Batch:{1} Journal:{2} Status:{3} at Rate:{4} on Date:{5} at Time:{6}"

                    Assert.IsTrue(Usage.Contains("Ledger:9997"), "Expected a reference to Ledger #9997, but was: " + Usage);

                    switch (i)
                    {
                        case 1:             // 0.5225
                            Assert.IsTrue(Usage.ToLower().Contains(
                                "status:unposted"), "Expected an unposted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:113"), "Expected a reference to Batch 113, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:1"), "Expected a reference to Journal 1, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5225"), "Expected a reference to a rate of 0.5225, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-10-30"), "Expected a reference to a date of 2000-10-30, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:0"), "Expected a reference to Time 0, but was: " + Usage);
                            break;

                        case 2:             // 0.5225
                            Assert.IsTrue(Usage.ToLower().Contains(
                                "status:unposted"), "Expected an unposted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:112"), "Expected a reference to Batch 112, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:1"), "Expected a reference to Journal 1, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5225"), "Expected a reference to a rate of 0.5225, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-10-26"), "Expected a reference to a date of 2000-10-26, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:0"), "Expected a reference to Time 0, but was: " + Usage);
                            break;

                        case 3:             // 0.5225
                            Assert.IsTrue(Usage.ToLower().Contains(
                                "status:unposted"), "Expected an unposted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:111"), "Expected a reference to Batch 111, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:1"), "Expected a reference to Journal 1, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5225"), "Expected a reference to a rate of 0.5225, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-10-22"), "Expected a reference to a date of 2000-10-22, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:0"), "Expected a reference to Time 0, but was: " + Usage);
                            break;

                        case 4:             // 0.5225
                            Assert.IsTrue(Usage.ToLower().Contains(
                                "status:unposted"), "Expected an unposted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:24"), "Expected a reference to Batch 24, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:0"), "Expected a reference to Journal 0, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5225"), "Expected a reference to a rate of 0.5225, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-10-15"), "Expected a reference to a date of 2000-10-15, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:0"), "Expected a reference to Time 0, but was: " + Usage);
                            break;

                        case 5:             // 0.5225
                            Assert.IsTrue(Usage.ToLower().Contains(
                                "status:unposted"), "Expected an unposted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:23"), "Expected a reference to Batch 23, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:0"), "Expected a reference to Journal 0, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5225"), "Expected a reference to a rate of 0.5225, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-10-09"), "Expected a reference to a date of 2000-10-09, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:0"), "Expected a reference to Time 0, but was: " + Usage);
                            break;

                        case 6:             // 0.5225
                            Assert.IsTrue(Usage.ToLower().Contains(
                                "status:unposted"), "Expected an unposted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:22"), "Expected a reference to Batch 22, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:0"), "Expected a reference to Journal 0, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5225"), "Expected a reference to a rate of 0.5225, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-10-05"), "Expected a reference to a date of 2000-10-05, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:0"), "Expected a reference to Time 0, but was: " + Usage);
                            break;

                        case 7:             // 0.5155
                            Assert.IsTrue(Usage.ToLower().Contains(
                                "status:unposted"), "Expected an unposted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:21"), "Expected a reference to Batch 22, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5155"), "Expected a reference to a rate of 0.5155, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:0"), "Expected a reference to Journal 0, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-10-01"), "Expected a reference to a date of 2000-10-01, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:0"), "Expected a reference to Time 0, but was: " + Usage);
                            break;

                        case 8:             // 0.5155
                            Assert.IsTrue(Usage.ToLower().Contains(
                                "status:unposted"), "Expected an unposted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:103"), "Expected a reference to Batch 103, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:1"), "Expected a reference to Journal 1, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5155"), "Expected a reference to a rate of 0.5155, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-08-28"), "Expected a reference to a date of 2000-08-28, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:60"), "Expected a reference to Time 60, but was: " + Usage);
                            break;

                        case 9:             // 0.5155
                            Assert.IsTrue(Usage.ToLower().Contains(
                                "status:unposted"), "Expected an unposted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:17"), "Expected a reference to Batch 17, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:0"), "Expected a reference to Journal 0, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5155"), "Expected a reference to a rate of 0.5155, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-08-28"), "Expected a reference to a date of 2000-08-28, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:0"), "Expected a reference to Time 0, but was: " + Usage);
                            break;

                        case 10:             // 0.5155
                            Assert.IsTrue(Usage.ToLower().Contains(
                                "status:unposted"), "Expected an unposted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:102"), "Expected a reference to Batch 102, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:1"), "Expected a reference to Journal 1, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5155"), "Expected a reference to a rate of 0.5155, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-08-08"), "Expected a reference to a date of 2000-08-08, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:60"), "Expected a reference to Time 60, but was: " + Usage);
                            break;

                        case 11:             // 0.5155
                            Assert.IsTrue(Usage.ToLower().Contains("status:posted"), "Expected a posted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:16"), "Expected a reference to Batch 16, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:0"), "Expected a reference to Journal 0, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5155"), "Expected a reference to a rate of 0.5155, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-08-08"), "Expected a reference to a date of 2000-08-08, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:0"), "Expected a reference to Time 0, but was: " + Usage);
                            break;

                        case 12:             // 0.5155
                            Assert.IsTrue(Usage.ToLower().Contains(
                                "status:unposted"), "Expected an unposted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:101"), "Expected a reference to Batch 101, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:1"), "Expected a reference to Journal 1, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5155"), "Expected a reference to a rate of 0.5155, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-08-08"), "Expected a reference to a date of 2000-08-08, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:60"), "Expected a reference to Time 60, but was: " + Usage);
                            break;

                        case 13:             // 0.5155
                            Assert.IsTrue(Usage.ToLower().Contains("status:posted"), "Expected a posted row in Gift Batch table, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Batch:15"), "Expected a reference to Batch 15, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Journal:0"), "Expected a reference to Journal 0, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Rate:0.5155"), "Expected a reference to a rate of 0.5155, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Date:2000-08-01"), "Expected a reference to a date of 2000-08-01, but was: " + Usage);
                            Assert.IsTrue(Usage.Contains("Time:0"), "Expected a reference to Time 0, but was: " + Usage);
                            break;

                        default:
                            break;
                    }
                }

                btnCloseTester.Click();
            }
            catch (Exception ex)
            {
                HandleModalException(ex);
                btnCancelTester.Properties.DialogResult = DialogResult.Abort;
                btnCancelTester.Click();
            }
        }

        #endregion

        #region Helper methods for Modal tests

        private void HandleModalException(Exception ex)
        {
            if (ex.StackTrace.Contains("Framework.Assert.That"))
            {
                // The exception is from an Assert
                // Construct a message that will show the Assert message from NUnitForms and then append the file and line number where the Assert occurred
                FModalAssertResult = ex.Message.Replace("\r\n", ".").TrimStart();
                FModalAssertResult += "\nAssertion occurred at: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf('\\') + 1);
            }
            else
            {
                // Ooops!  We got a general exception (coding error).
                // Show the message on the Results tab and write the stack trace info to the Text Output tab.
                FModalAssertResult = "General exception occurred on modal form:\n" + ex.Message + "\nSee Text Output tab for more details";
                Console.WriteLine(ex.StackTrace);
            }
        }

        #endregion
    }
}