//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
//
using System;
using System.Data;
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using System.Threading;
using System.Globalization;
using Ict.Common.Controls;

namespace Tests.Common.Controls
{
    /// TODO write your comment here
    [TestFixture]
    public class TTestNumericTextBox
    {
        /// <summary>
        /// testing decimal currency values in numeric text box
        /// </summary>
        [Test]
        public void TestCurrencyValues()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            TTxtCurrencyTextBox txtBox = new TTxtCurrencyTextBox();
            txtBox.DecimalPlaces = 2;

            txtBox.NumberValueDecimal = 1410.95M;
            Assert.AreEqual(1410.95M, txtBox.NumberValueDecimal, "decimal value stored in british culture");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            txtBox.NumberValueDecimal = 0.0M;
            txtBox.NumberValueDecimal = 1410.95M;
            Assert.AreEqual(1410.95M, txtBox.NumberValueDecimal, "decimal value stored in german culture, switching culture, same txt object");
            Assert.AreEqual("1,410.95",
                txtBox.Text,
                "text value stored in german culture, switching culture, same txt object, therefore still british format");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            TTxtCurrencyTextBox txtDEBox = new TTxtCurrencyTextBox();
            txtDEBox.DecimalPlaces = 2;
            txtDEBox.NumberValueDecimal = 0.0M;
            txtDEBox.NumberValueDecimal = 1410.95M;
            Assert.AreEqual("1.410,95", txtDEBox.Text, "text value stored in german culture");
            Assert.AreEqual(1410.95M, txtDEBox.NumberValueDecimal, "decimal value stored in german culture");
            txtDEBox.NumberValueDecimal = 1234410.95M;
            Assert.AreEqual("1.234.410,95", txtDEBox.Text, "huge number text value stored in german culture");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            txtBox.NumberValueDecimal = 0.0M;
            txtBox.NumberValueDecimal = 30.00M;
            Assert.AreEqual(30.00M, txtBox.NumberValueDecimal, "decimal value stored in english culture, with english UI");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            txtBox.NumberValueDecimal = 0.0M;
            txtBox.NumberValueDecimal = 770.00M;
            Assert.AreEqual(770.00M,
                txtBox.NumberValueDecimal,
                "decimal value stored in english culture, with english UI, thousand separator problem");
            Assert.AreEqual("770.00", txtBox.Text, "testing problem with thousand separator");
            txtBox.NumberValueDecimal = 1234410.95M;
            Assert.AreEqual("1,234,410.95", txtBox.Text, "huge number text value stored in british culture");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            txtBox.NumberValueDecimal = 0.0M;
            txtBox.NumberValueDecimal = 1410.95M;
            Assert.AreEqual(1410.95M, txtBox.NumberValueDecimal, "decimal value stored in german culture, but english UI");
        }

        /// <summary>
        /// testing decimal values in numeric text box
        /// </summary>
        [Test]
        public void TestDecimalValues()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            TTxtNumericTextBox txtBox = new TTxtNumericTextBox();
            txtBox.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Decimal;
            txtBox.DecimalPlaces = 2;

            txtBox.NumberValueDecimal = 1410.95M;
            Assert.AreEqual(1410.95M, txtBox.NumberValueDecimal, "decimal value stored in british culture");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            txtBox.NumberValueDecimal = 0.0M;
            txtBox.NumberValueDecimal = 1410.95M;
            Assert.AreEqual(1410.95M, txtBox.NumberValueDecimal, "decimal value stored in german culture, switching culture, same txt object");
            Assert.AreEqual("1,410.95",
                txtBox.Text,
                "text value stored in german culture, switching culture, same txt object, therefore still british format");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            TTxtNumericTextBox txtDEBox = new TTxtNumericTextBox();
            txtDEBox.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Decimal;
            txtDEBox.DecimalPlaces = 2;
            txtDEBox.NumberValueDecimal = 0.0M;
            txtDEBox.NumberValueDecimal = 1410.95M;
            Assert.AreEqual("1.410,95", txtDEBox.Text, "text value stored in german culture");
            Assert.AreEqual(1410.95M, txtDEBox.NumberValueDecimal, "decimal value stored in german culture");
            txtDEBox.NumberValueDecimal = 1234410.95M;
            Assert.AreEqual("1.234.410,95", txtDEBox.Text, "huge number text value stored in german culture");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            txtBox.NumberValueDecimal = 0.0M;
            txtBox.NumberValueDecimal = 30.00M;
            Assert.AreEqual(30.00M, txtBox.NumberValueDecimal, "decimal value stored in english culture, with english UI");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            txtBox.NumberValueDecimal = 0.0M;
            txtBox.NumberValueDecimal = 1410.95M;
            Assert.AreEqual(1410.95M, txtBox.NumberValueDecimal, "decimal value stored in german culture, but english UI");
        }

        /// <summary>
        /// testing double values in numeric text box
        /// </summary>
        [Test]
        public void TestDoubleValues()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            TTxtNumericTextBox txtBox = new TTxtNumericTextBox();
            txtBox.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Decimal;
            txtBox.DecimalPlaces = 2;

            txtBox.NumberValueDouble = 1410.95;
            Assert.AreEqual(1410.95, txtBox.NumberValueDouble, "double value stored in british culture");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            txtBox.NumberValueDouble = 0.0;
            txtBox.NumberValueDouble = 1410.95;
            Assert.AreEqual(1410.95, txtBox.NumberValueDouble, "double value stored in german culture, switching culture, same txt object");
            Assert.AreEqual("1,410.95",
                txtBox.Text,
                "text value stored in german culture, switching culture, same txt object, therefore still british format");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            TTxtNumericTextBox txtDEBox = new TTxtNumericTextBox();
            txtDEBox.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Decimal;
            txtDEBox.DecimalPlaces = 2;
            txtDEBox.NumberValueDouble = 0.0;
            txtDEBox.NumberValueDouble = 1410.95;
            Assert.AreEqual("1.410,95", txtDEBox.Text, "text value stored in german culture");
            Assert.AreEqual(1410.95, txtDEBox.NumberValueDouble, "double value stored in german culture");
            txtDEBox.NumberValueDouble = 1234410.95;
            Assert.AreEqual("1.234.410,95", txtDEBox.Text, "huge number text value stored in german culture");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            txtBox.NumberValueDouble = 0.0;
            txtBox.NumberValueDouble = 30.00;
            Assert.AreEqual(30.00, txtBox.NumberValueDouble, "double value stored in english culture, with english UI");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            txtBox.NumberValueDouble = 0.0;
            txtBox.NumberValueDouble = 1410.95;
            Assert.AreEqual(1410.95, txtBox.NumberValueDouble, "double value stored in german culture, but english UI");
        }

        /// <summary>
        /// testing integer values in numeric text box
        /// </summary>
        [Test]
        public void TestIntegerValues()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            TTxtNumericTextBox txtBox = new TTxtNumericTextBox();
            txtBox.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Integer;

            txtBox.NumberValueInt = 1410;
            Assert.AreEqual(1410, txtBox.NumberValueInt, "integer value stored in british culture");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            txtBox.NumberValueInt = 0;
            txtBox.NumberValueInt = 1410;
            Assert.AreEqual(1410, txtBox.NumberValueInt, "integer value stored in german culture, switching culture, same txt object");
            Assert.AreEqual("1410",
                txtBox.Text,
                "text value stored in german culture, switching culture, same txt object, therefore still british format");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            TTxtNumericTextBox txtDEBox = new TTxtNumericTextBox();
            txtDEBox.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Integer;
            txtDEBox.NumberValueInt = 0;
            txtDEBox.NumberValueInt = 1410;
            Assert.AreEqual("1410", txtDEBox.Text, "text value stored in german culture");
            Assert.AreEqual(1410, txtDEBox.NumberValueInt, "integer value stored in german culture");
            txtDEBox.NumberValueInt = 1234410;
            Assert.AreEqual("1234410", txtDEBox.Text, "huge number text value stored in german culture");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            txtBox.NumberValueInt = 0;
            txtBox.NumberValueInt = 30;
            Assert.AreEqual(30, txtBox.NumberValueInt, "integer value stored in english culture, with english UI");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            txtBox.NumberValueInt = 0;
            txtBox.NumberValueInt = 1410;
            Assert.AreEqual(1410, txtBox.NumberValueInt, "integer value stored in german culture, but english UI");
        }
    }
}