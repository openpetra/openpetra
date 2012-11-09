//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Testing.NUnitForms;

namespace Tests.Common.Controls
{
    /// Testing the text edit control for dates
    [TestFixture]
    public class TTestPetraDate : NUnitFormTest
    {
        int DateChangedCalled = 0;
        int TextChangedCalled = 0;

        private void DateChanged(object sender, TPetraDateChangedEventArgs e)
        {
            TLogging.Log("DateChanged");
            DateChangedCalled++;
        }

        private void TextChanged(object sender, EventArgs e)
        {
            TLogging.Log("TextChanged");
            TextChangedCalled++;
        }

        /// <summary>
        /// Testing the text edit control for dates
        /// </summary>
        [Test]
        public void TestEvents()
        {
            TtxtPetraDate dtpDate = new TtxtPetraDate();

            dtpDate.Name = "dtpDate";
            dtpDate.DateChanged += new TPetraDateChangedEventHandler(DateChanged);
            dtpDate.TextChanged += new EventHandler(TextChanged);

            Form TestForm = new Form();
            TestForm.Controls.Add(dtpDate);

            TestForm.Show();

            TTxtPetraDateTester tester = new TTxtPetraDateTester("dtpDate");

            TextChangedCalled = 0;
            DateChangedCalled = 0;
            tester.Properties.Text = "31-12-2012";

            Assert.AreEqual(new DateTime(2012, 12, 31), tester.Properties.Date.Value, "date should be set");
            Assert.AreEqual(1, DateChangedCalled, "event DateChanged should have been called once");
            Assert.AreEqual(1, TextChangedCalled, "event TextChanged should have been called once");

            tester.Properties.Text = "30-12-2012";

            TextChangedCalled = 0;
            DateChangedCalled = 0;
            tester.Properties.Text = "31-DEC-2012";

            Assert.AreEqual(new DateTime(2012, 12, 31), tester.Properties.Date.Value, "date should be set, Test With DEC");
            Assert.AreEqual(1, DateChangedCalled, "event DateChanged should have been called once, Test With DEC");
            Assert.AreEqual(1, TextChangedCalled, "event TextChanged should have been called once, Test With DEC");

            TextChangedCalled = 0;
            DateChangedCalled = 0;
            tester.Properties.Text = "31-12-2012";

            Assert.AreEqual(new DateTime(2012, 12, 31), tester.Properties.Date.Value, "date should be set, test resetting");
            Assert.AreEqual(0, DateChangedCalled, "event DateChanged should not have been called, test resetting");
            Assert.AreEqual(0, TextChangedCalled, "event TextChanged should not have been called, test resetting");
        }

        /// <summary>
        /// Test assigning month names for different culture
        /// </summary>
        [Test]
        public void TestCulture()
        {
            TtxtPetraDate dtpDate = new TtxtPetraDate();

            dtpDate.Name = "dtpDate";
            dtpDate.DateChanged += new TPetraDateChangedEventHandler(DateChanged);
            dtpDate.TextChanged += new EventHandler(TextChanged);

            Form TestForm = new Form();
            TestForm.Controls.Add(dtpDate);

            TestForm.Show();

            TTxtPetraDateTester tester = new TTxtPetraDateTester("dtpDate");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            tester.Properties.Text = "30-JAN-2012";
            Assert.AreEqual(new DateTime(2012, 1, 30), tester.Properties.Date.Value, "date should be set, 30-JAN-2012");
            tester.Properties.Text = "30-JANUARY-2012";
            Assert.AreEqual(new DateTime(2012, 1, 30), tester.Properties.Date.Value, "date should be set, 30-JANUARY-2012");
            tester.Properties.Text = "30-SEPTEMBER-2012";
            Assert.AreEqual(new DateTime(2012, 9, 30), tester.Properties.Date.Value, "date should be set, 30-SEPTEMBER-2012");
            tester.Properties.Text = "30-APR-2012";
            Assert.AreEqual(new DateTime(2012, 4, 30), tester.Properties.Date.Value, "date should be set, 30-APR-2012");
            tester.Properties.Text = "30-NOV-2012";
            Assert.AreEqual(new DateTime(2012, 11, 30), tester.Properties.Date.Value, "date should be set, 30-NOV-2012");
        }

        Int32 NumberOfMessageBoxes = 0;

        private void HandleMessageBox(string name, IntPtr hWnd)
        {
            NumberOfMessageBoxes++;

            MessageBoxTester tester = new MessageBoxTester(hWnd);

            TLogging.Log("HandleMessageBox: " + NumberOfMessageBoxes.ToString() + ": " +
                tester.Text.Replace(Environment.NewLine, " "));

            DialogBoxHandler = HandleMessageBox;

            tester.SendCommand(MessageBoxTester.Command.OK);
        }

        /// <summary>
        /// what happens when we enter invalid dates
        /// </summary>
        [Test]
        public void EnterInvalidDates()
        {
            TtxtPetraDate dtpDate = new TtxtPetraDate();

            dtpDate.Name = "dtpDate";
            dtpDate.DateChanged += new TPetraDateChangedEventHandler(DateChanged);
            dtpDate.TextChanged += new EventHandler(TextChanged);

            Form TestForm = new Form();
            TestForm.Controls.Add(dtpDate);

            TestForm.Show();

            TTxtPetraDateTester tester = new TTxtPetraDateTester("dtpDate");

            TextChangedCalled = 0;
            DateChangedCalled = 0;
            NumberOfMessageBoxes = 0;

            DialogBoxHandler = HandleMessageBox;
            tester.Properties.Text = "30";

            Assert.AreEqual(1, NumberOfMessageBoxes, "entering an invalid date should only show a messagebox once");
            Assert.IsTrue(TVerificationHelper.AreVerificationResultsIdentical(tester.Properties.DateVerificationResult, Ict.Common.Verification.TDateChecks.GetInvalidDateVerificationResult("Date", null), false, false));
            
            TextChangedCalled = 0;
            DateChangedCalled = 0;
            NumberOfMessageBoxes = 0;

            DialogBoxHandler = HandleMessageBox;
            tester.Properties.Text = "301210000";

            Assert.AreEqual(1, NumberOfMessageBoxes, "entering an invalid date should only show a messagebox once: year 10000");

            DialogBoxHandler = null;
            
            NumberOfMessageBoxes = 0;
            
            DialogBoxHandler = HandleMessageBox;
            
            tester.Properties.AllowEmpty = false;
            tester.Properties.Text = "01-JAN-2010";
            tester.Properties.Text = "";
            
            Assert.AreEqual(1, NumberOfMessageBoxes, "entering an invalid date should only show a messagebox once: year 10000");
            Assert.IsTrue(TVerificationHelper.AreVerificationResultsIdentical(tester.Properties.DateVerificationResult, 
                new TVerificationResult(null, ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_NOUNDEFINEDDATE,
                            CommonResourcestrings.StrInvalidDateEntered + Environment.NewLine +
                            "{0} may not be empty.", new string[] { "'Date'" }))));
            
            DialogBoxHandler = null;
        }
    }
}