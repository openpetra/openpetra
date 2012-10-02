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
using System.IO;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Common;
using Ict.Testing.NUnitForms;

namespace Tests.Common.Controls
{
    /// Testing the text edit control for dates
    [TestFixture]
    public class TTestPetraDate
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
            Assert.AreEqual(2, TextChangedCalled, "event TextChanged should have been called twice");
            Assert.AreEqual(1, DateChangedCalled, "event DateChanged should have been called once");
        }
    }
}