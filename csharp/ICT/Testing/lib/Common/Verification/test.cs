//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;

using NUnit.Framework;

using Ict.Common;
using Ict.Common.Verification;

namespace Ict.Common.Verification.Testing
{
    ///  This is a testing program for Ict.Common.Verification.dll
    [TestFixture]
    public class TTestCommon
    {
        /// <summary>
        /// Test initialisation.
        /// </summary>
        [SetUp]
        public void Init()
        {
            Catalog.Init();
            new TLogging("test.log");
        }

        #region Helper Methods for Test Cases of this Unit Test

        private string EvaluateVerificationResults(TVerificationResult AExpectedResult, TVerificationResult ATestResult)
        {
            TVerificationResultCollection Tmp;

            if (!TVerificationHelper.AreVerificationResultsIdentical(ATestResult, AExpectedResult))
            {
                Tmp = new TVerificationResultCollection();

                if (AExpectedResult != null)
                {
                    Tmp.Add(AExpectedResult);
                }

                if (ATestResult != null)
                {
                    Tmp.Add(ATestResult);
                }

                return TVerificationHelper.FormatVerificationCollectionItems(Tmp);
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion


        /// <summary>
        /// Test for numerical verifications.
        /// </summary>
        [Test]
        public void TestNumericalChecks()
        {
            TVerificationResult ExpectedResult = null;
            TVerificationResult TestResult = null;
            string Testname;
            string ExpectedErrorText;
            string VerificationProblemListing;
            object TestContext = new Object();
            DataColumn TestColumn = new DataColumn("TestColumn");

            System.Windows.Forms.Control TestControl = new System.Windows.Forms.TextBox();

            #region IsValid...

            #region IsValidInteger

            ExpectedErrorText = "'{0}' must be an integer (= a number without a fraction).";

            Testname = "Test with 5";
            TestResult = TNumericalChecks.IsValidInteger("5", Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidInteger: " + Testname);

            Testname = "Test with 5";
            TestResult = TNumericalChecks.IsValidInteger("5", Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidInteger: " + Testname);

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsValidInteger("5.1", Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidInteger: " + Testname);

            Testname = "Test with 5.1 (TScreenVerificationResult)";
            TestResult = TNumericalChecks.IsValidInteger("5.1", Testname);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidInteger: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsValidInteger(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidInteger: " + Testname);

            Testname = "Test with String.Empty";
            TestResult = TNumericalChecks.IsValidInteger(String.Empty, Testname);
//            ExpectedResult = null;
//            ExpectedResult = new TVerificationResult(String.Empty, ErrorCodes.GetErrorInfo("GENC.00005V",
//                "'oooooooooooo' must be an integer (= a number without a fraction)."));
//            ExpectedResult = new TVerificationResult(String.Empty, ErrorCodes.GetErrorInfo("GENC.00005V",
//                "Invalid number entered." + Environment.NewLine + "'Test with String.Empty' must be an integer (= a number without a fraction)."));
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidInteger: " + Testname);

            #endregion


            #region IsValidDouble

            ExpectedErrorText = "'{0}' must be a decimal number (= a number that has a fraction).";

            Testname = "Test with -5";
            TestResult = TNumericalChecks.IsValidDouble("-5", Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDouble: " + Testname);

            Testname = "Test with -5";
            TestResult = TNumericalChecks.IsValidDouble("-5", Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDouble: " + Testname);

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsValidDouble("5.1", Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDouble: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsValidDouble(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDouble: " + Testname);

            Testname = "Test with String.Empty";
            TestResult = TNumericalChecks.IsValidDouble(String.Empty, Testname);
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDouble: " + Testname);

            Testname = "Test with String.Empty (TScreenVerificationResult)";
            TestResult = TNumericalChecks.IsValidDouble(String.Empty, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDouble: " + Testname);

            #endregion


            #region IsValidDecimal

            ExpectedErrorText = "'{0}' must be a decimal number (= a number that has a fraction).";

            Testname = "Test with -5";
            TestResult = TNumericalChecks.IsValidDecimal("-5", Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDecimal: " + Testname);

            Testname = "Test with -5";
            TestResult = TNumericalChecks.IsValidDecimal("-5", Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDecimal: " + Testname);

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsValidDecimal("5.1", Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDecimal: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsValidDecimal(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDecimal: " + Testname);

            Testname = "Test with String.Empty";
            TestResult = TNumericalChecks.IsValidDecimal(String.Empty, Testname);
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDecimal: " + Testname);

            Testname = "Test with String.Empty (TScreenVerificationResult)";
            TestResult = TNumericalChecks.IsValidDecimal(String.Empty, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDecimal: " + Testname);

            #endregion

            #endregion


            #region IsPositiveOrZero...

            #region IsPositiveOrZeroInteger

            ExpectedErrorText = "'{0}' must be a positive integer (= a number without a fraction), or 0.";

            Testname = "Test with 5";
            TestResult = TNumericalChecks.IsPositiveOrZeroInteger(5, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroInteger: " + Testname);

            Testname = "Test with 5";
            TestResult = TNumericalChecks.IsPositiveOrZeroInteger(5, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroInteger: " + Testname);

            Testname = "Test with 0";
            TestResult = TNumericalChecks.IsPositiveOrZeroInteger(0, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroInteger: " + Testname);

            Testname = "Test with -1";
            TestResult = TNumericalChecks.IsPositiveOrZeroInteger(-1, Testname);
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroInteger: " + Testname);

            Testname = "Test with -1 (TScreenVerificationResult)";
            TestResult = TNumericalChecks.IsPositiveOrZeroInteger(-1, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroInteger: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsPositiveOrZeroInteger(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroInteger: " + Testname);

            #endregion

            #region IsPositiveOrZeroDouble

            ExpectedErrorText = "'{0}' must be a positive decimal number (= a number that has a fraction), or 0.";

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsPositiveOrZeroDouble(5.1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDouble: " + Testname);

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsPositiveOrZeroDouble(5.1, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDouble: " + Testname);

            Testname = "Test with 0.0";
            TestResult = TNumericalChecks.IsPositiveOrZeroDouble(0.0, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDouble: " + Testname);

            Testname = "Test with -0.1";
            TestResult = TNumericalChecks.IsPositiveOrZeroDouble(-0.1, Testname);
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDouble: " + Testname);

            Testname = "Test with -0.1 (TScreenVerificationResult)";
            TestResult = TNumericalChecks.IsPositiveOrZeroDouble(-0.1, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDouble: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsPositiveOrZeroDouble(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDouble: " + Testname);

            #endregion

            #region IsPositiveOrZeroDecimal

            ExpectedErrorText = "'{0}' must be a positive decimal number (= a number that has a fraction), or 0.";

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsPositiveOrZeroDecimal((decimal)5.1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDecimal: " + Testname);

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsPositiveOrZeroDecimal((decimal)5.1, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDecimal: " + Testname);

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsPositiveOrZeroDecimal((decimal)5.1, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDecimal: " + Testname);

            Testname = "Test with 0.0";
            TestResult = TNumericalChecks.IsPositiveOrZeroDecimal((decimal)0.0, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDecimal: " + Testname);

            Testname = "Test with -0.1";
            TestResult = TNumericalChecks.IsPositiveOrZeroDecimal((decimal) - 0.1, Testname);
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDecimal: " + Testname);

            Testname = "Test with -0.1 (TScreenVerificationResult)";
            TestResult = TNumericalChecks.IsPositiveOrZeroDecimal((decimal) - 0.1, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDecimal: " + Testname);

            Testname = "Test with -0.1 (TScreenVerificationResult)";
            TestResult = TNumericalChecks.IsPositiveOrZeroDecimal((decimal) - 0.1, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDecimal: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsPositiveOrZeroDecimal(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDecimal: " + Testname);

            #endregion

            #endregion


            #region FirstLesserThanSecond...

            ExpectedErrorText = "'{0}' cannot be greater or equal to '{1}'.";

            #region FirstLesserThanSecondInteger

            Testname = "Test with 5 and 10";
            TestResult = TNumericalChecks.FirstLesserThanSecondInteger(5, 10, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondInteger: " + Testname);

            Testname = "Test with 5 and 10";
            TestResult = TNumericalChecks.FirstLesserThanSecondInteger(5, 10, "1st value", "2nd value", TestContext, TestColumn, TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondInteger: " + Testname);

            Testname = "Test with 6 and 5";
            TestResult = TNumericalChecks.FirstLesserThanSecondInteger(6, 5, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondInteger: " + Testname);

            Testname = "Test with 6 and 5 (TScreenVerificationResult)";
            TestResult = TNumericalChecks.FirstLesserThanSecondInteger(6, 5, "1st value", "2nd value", TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondInteger: " + Testname);

            Testname = "Test with 0 and 0";
            TestResult = TNumericalChecks.FirstLesserThanSecondInteger(0, 0, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondInteger: " + Testname);

            Testname = "Test with -1 and -2";
            TestResult = TNumericalChecks.FirstLesserThanSecondInteger(-1, -2, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondInteger: " + Testname);

            Testname = "Test with null and null";
            TestResult = TNumericalChecks.FirstLesserThanSecondInteger(null, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondInteger: " + Testname);

            Testname = "Test with 1 and null";
            TestResult = TNumericalChecks.FirstLesserThanSecondInteger(1, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondInteger: " + Testname);

            Testname = "Test with null and 1";
            TestResult = TNumericalChecks.FirstLesserThanSecondInteger(null, 1, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondInteger: " + Testname);

            #endregion

            #region FirstLesserThanSecondDouble

            Testname = "Test with 5.1 and 5.2";
            TestResult = TNumericalChecks.FirstLesserThanSecondDouble(5.1, 5.2, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDouble: " + Testname);

            Testname = "Test with 5.1 and 5.2";
            TestResult = TNumericalChecks.FirstLesserThanSecondDouble(5.1, 5.2, "1st value", "2nd value", TestContext, TestColumn, TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDouble: " + Testname);

            Testname = "Test with 5.2 and 5.1";
            TestResult = TNumericalChecks.FirstLesserThanSecondDouble(5.2, 5.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDouble: " + Testname);

            Testname = "Test with 5.2 and 5.1 (TScreenVerificationResult)";
            TestResult = TNumericalChecks.FirstLesserThanSecondDouble(5.2, 5.1, "1st value", "2nd value", TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDouble: " + Testname);

            Testname = "Test with 0.0 and 0.0";
            TestResult = TNumericalChecks.FirstLesserThanSecondDouble(0.0, 0.0, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDouble: " + Testname);

            Testname = "Test with -1.0 and -1.1";
            TestResult = TNumericalChecks.FirstLesserThanSecondDouble(-1.0, -1.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDouble: " + Testname);

            Testname = "Test with null and null";
            TestResult = TNumericalChecks.FirstLesserThanSecondDouble(null, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDouble: " + Testname);

            Testname = "Test with 1.0 and null";
            TestResult = TNumericalChecks.FirstLesserThanSecondDouble(1.0, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDouble: " + Testname);

            Testname = "Test with null and 1.0";
            TestResult = TNumericalChecks.FirstLesserThanSecondDouble(null, 1.0, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDouble: " + Testname);

            #endregion

            #region FirstLesserThanSecondDecimal

            Testname = "Test with 5.1 and 5.2";
            TestResult = TNumericalChecks.FirstLesserThanSecondDecimal((decimal)5.1, (decimal)5.2, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDecimal: " + Testname);

            Testname = "Test with 5.1 and 5.2";
            TestResult = TNumericalChecks.FirstLesserThanSecondDecimal((decimal)5.1,
                (decimal)5.2,
                "1st value",
                "2nd value",
                TestContext,
                TestColumn,
                TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDecimal: " + Testname);

            Testname = "Test with 5.2 and 5.1";
            TestResult = TNumericalChecks.FirstLesserThanSecondDecimal((decimal)5.2, (decimal)5.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDecimal: " + Testname);

            Testname = "Test with 5.2 and 5.1 (TScreenVerificationResult)";
            TestResult = TNumericalChecks.FirstLesserThanSecondDecimal((decimal)5.2,
                (decimal)5.1,
                "1st value",
                "2nd value",
                TestContext,
                TestColumn,
                TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDecimal: " + Testname);

            Testname = "Test with 0.0 and 0.0";
            TestResult = TNumericalChecks.FirstLesserThanSecondDecimal((decimal)0.0, (decimal)0.0, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDecimal: " + Testname);

            Testname = "Test with -1.0 and -1.1";
            TestResult = TNumericalChecks.FirstLesserThanSecondDecimal((decimal) - 1.0, (decimal) - 1.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDecimal: " + Testname);

            Testname = "Test with null and null";
            TestResult = TNumericalChecks.FirstLesserThanSecondDecimal(null, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDecimal: " + Testname);

            Testname = "Test with 1.0 and null";
            TestResult = TNumericalChecks.FirstLesserThanSecondDecimal((decimal)1.0, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDecimal: " + Testname);

            Testname = "Test with null and 1.0";
            TestResult = TNumericalChecks.FirstLesserThanSecondDecimal(null, (decimal)1.0, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDecimal: " + Testname);

            #endregion

            #endregion


            #region FirstLesserOrEqualThanSecond...

            ExpectedErrorText = "'{0}' cannot be greater than '{1}'.";

            #region FirstLesserOrEqualThanSecondInteger

            Testname = "Test with 5 and 10";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondInteger(5, 10, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondInteger: " + Testname);

            Testname = "Test with 5 and 10";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondInteger(5, 10, "1st value", "2nd value", TestContext, TestColumn, TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondInteger: " + Testname);

            Testname = "Test with 6 and 5";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondInteger(6, 5, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondInteger: " + Testname);

            Testname = "Test with 6 and 5 (TScreenVerificationResult)";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondInteger(6, 5, "1st value", "2nd value", TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondInteger: " + Testname);

            Testname = "Test with 6 and 6";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondInteger(6, 6, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondInteger: " + Testname);

            Testname = "Test with 0 and 0";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondInteger(0, 0, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondInteger: " + Testname);

            Testname = "Test with -1 and -2";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondInteger(-1, -2, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondInteger: " + Testname);

            Testname = "Test with -1 and -1";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondInteger(-1, -1, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondInteger: " + Testname);

            Testname = "Test with null and null";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondInteger(null, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondInteger: " + Testname);

            Testname = "Test with 1 and null";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondInteger(1, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondInteger: " + Testname);

            Testname = "Test with null and 1";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondInteger(null, 1, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondInteger: " + Testname);

            #endregion

            #region FirstLesserOrEqualThanSecondDouble

            Testname = "Test with 5.1 and 5.2";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDouble(5.1, 5.2, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDouble: " + Testname);

            Testname = "Test with 5.1 and 5.2";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDouble(5.1, 5.2, "1st value", "2nd value", TestContext, TestColumn, TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDouble: " + Testname);

            Testname = "Test with 5.2 and 5.1";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDouble(5.2, 5.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDouble: " + Testname);

            Testname = "Test with 5.2 and 5.1 (TScreenVerificationResult)";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDouble(5.2, 5.1, "1st value", "2nd value", TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDouble: " + Testname);

            Testname = "Test with 5.2 and 5.2";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDouble(5.2, 5.2, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDouble: " + Testname);

            Testname = "Test with 0.0 and 0.0";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDouble(0.0, 0.0, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDouble: " + Testname);

            Testname = "Test with -1.0 and -1.1";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDouble(-1.0, -1.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDouble: " + Testname);

            Testname = "Test with -1.0 and -1.0";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDouble(-1.0, -1.0, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDouble: " + Testname);

            Testname = "Test with null and null";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDouble(null, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDouble: " + Testname);

            Testname = "Test with 1.0 and null";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDouble(1.0, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDouble: " + Testname);

            Testname = "Test with null and 1.0";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDouble(null, 1.0, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDouble: " + Testname);

            #endregion

            #region FirstLesserOrEqualThanSecondDecimal

            Testname = "Test with 5.1 and 5.2";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal((decimal)5.1, (decimal)5.2, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDecimal: " + Testname);

            Testname = "Test with 5.1 and 5.2";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal((decimal)5.1,
                (decimal)5.2,
                "1st value",
                "2nd value",
                TestContext,
                TestColumn,
                TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDecimal: " + Testname);

            Testname = "Test with 5.2 and 5.1";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal((decimal)5.2, (decimal)5.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDecimal: " + Testname);

            Testname = "Test with 5.2 and 5.1 (TScreenVerificationResult)";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal((decimal)5.2,
                (decimal)5.1,
                "1st value",
                "2nd value",
                TestContext,
                TestColumn,
                TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDecimal: " + Testname);

            Testname = "Test with 5.2 and 5.2";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal((decimal)5.2, (decimal)5.2, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDecimal: " + Testname);

            Testname = "Test with 0.0 and 0.0";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal((decimal)0.0, (decimal)0.0, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDecimal: " + Testname);

            Testname = "Test with -1.0 and -1.1";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal((decimal) - 1.0, (decimal) - 1.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDecimal: " + Testname);

            Testname = "Test with -1.0 and -1.0";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal((decimal) - 1.0, (decimal) - 1.0, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDecimal: " + Testname);

            Testname = "Test with null and null";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal(null, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDecimal: " + Testname);

            Testname = "Test with 1.0 and null";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal((decimal)1.0, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDecimal: " + Testname);

            Testname = "Test with null and 1.0";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal(null, (decimal)1.0, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDecimal: " + Testname);

            #endregion

            #endregion
        }

        /// <summary>
        /// Test for string verifications.
        /// </summary>
        [Test]
        public void TestStringChecks()
        {
            TVerificationResult ExpectedResult = null;
            TVerificationResult TestResult = null;
            string Testname;
            string ExpectedErrorText;
            string VerificationProblemListing;
            object TestContext = new Object();
            DataColumn TestColumn = new DataColumn("TestColumn");

            System.Windows.Forms.Control TestControl = new System.Windows.Forms.TextBox();

            #region StringMustNotBeEmpty

            ExpectedErrorText = "A value must be entered for '{0}'.";

            Testname = "Test with 'blahblah'";
            TestResult = TStringChecks.StringMustNotBeEmpty("blahblah", Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "StringMustNotBeEmpty: " + Testname);

            Testname = "Test with 'blahblah' (TScreenVerificationResult)";
            TestResult = TStringChecks.StringMustNotBeEmpty("blahblah", Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "StringMustNotBeEmpty: " + Testname);

            Testname = "Test with String.Empty";
            TestResult = TStringChecks.StringMustNotBeEmpty(String.Empty, Testname);
            ExpectedResult = new TVerificationResult("", "Invalid value entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00008V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "StringMustNotBeEmpty: " + Testname);

            Testname = "Test with String.Empty (TScreenVerificationResult)";
            TestResult = TStringChecks.StringMustNotBeEmpty(String.Empty, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid value entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00008V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "StringMustNotBeEmpty: " + Testname);

            Testname = "Test with null";
            TestResult = TStringChecks.StringMustNotBeEmpty(null, Testname);
            ExpectedResult = new TVerificationResult("", "Invalid value entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00008V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "StringMustNotBeEmpty: " + Testname);

            #endregion


            #region FirstLesserOrEqualThanSecondString

            ExpectedErrorText = "Invalid Order.\r\n'{0}' cannot be greater than '{1}'.";

            Testname = "Test with 'aaa' and 'aab'";
            TestResult = TStringChecks.FirstLesserOrEqualThanSecondString("aaa", "aab", "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondString: " + Testname);

            Testname = "Test with 'aaa' and 'aab'";
            TestResult = TStringChecks.FirstLesserOrEqualThanSecondString("aaa",
                "aab",
                "1st value",
                "2nd value",
                TestContext,
                TestColumn,
                TestControl);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondString: " + Testname);

            Testname = "Test with 'aab' and 'aaa'";
            TestResult = TStringChecks.FirstLesserOrEqualThanSecondString("aab", "aaa", "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("",
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00007V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondString: " + Testname);

            Testname = "Test with 'aab' and 'aaa'";
            TestResult = TStringChecks.FirstLesserOrEqualThanSecondString("aab",
                "aaa",
                "1st value",
                "2nd value",
                TestContext,
                TestColumn,
                TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn,
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00007V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondString: " + Testname);

            Testname = "Test with 'aaa' and 'aaa'";
            TestResult = TStringChecks.FirstLesserOrEqualThanSecondString("aaa", "aaa", "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondString: " + Testname);

            Testname = "Test with String.Empty and String.Empty";
            TestResult = TStringChecks.FirstLesserOrEqualThanSecondString(String.Empty, String.Empty, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondString: " + Testname);

            Testname = "Test with null and null";
            TestResult = TStringChecks.FirstLesserOrEqualThanSecondString(null, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondString: " + Testname);

            #endregion


            #region ValidateEmail

            ExpectedErrorText = "The e-mail address '{0}' is not valid.";

            Testname = "Test with 'test@test.com'";
            TestResult = TStringChecks.ValidateEmail("test@test.com");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com.x'";
            TestResult = TStringChecks.ValidateEmail("test@test.com.x");
            ExpectedResult = new TVerificationResult("", String.Format(ExpectedErrorText, "test@test.com.x"),
                "GENC.00007V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com.x'";
            TestResult = TStringChecks.ValidateEmail("test@test.com.x", TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn,
                String.Format(ExpectedErrorText, "test@test.com.x"), "GENC.00007V", TestControl,
                TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com, blah@blah.org'";
            TestResult = TStringChecks.ValidateEmail("test@test.com, blah@blah.org", true);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com;blah@blah.org'";
            TestResult = TStringChecks.ValidateEmail("test@test.com;blah@blah.org", true);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com;;'";
            TestResult = TStringChecks.ValidateEmail("test@test.com;;", true);
            ExpectedResult = new TVerificationResult("", String.Format(ExpectedErrorText + Environment.NewLine +
                    "Note: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses.",
                    "test@test.com;;"),
                "GENC.00007V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com; ,'";
            TestResult = TStringChecks.ValidateEmail("test@test.com; ,", true);
            ExpectedResult = new TVerificationResult("", String.Format(ExpectedErrorText + Environment.NewLine +
                    "Note: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses.",
                    "test@test.com; ,"),
                "GENC.00007V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com;blah@blah.org'";
            TestResult = TStringChecks.ValidateEmail("test@test.com;blah@blah.org");
            ExpectedResult = new TVerificationResult("", String.Format(ExpectedErrorText + Environment.NewLine +
                    "Reason: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses. However, in this case only ONE e-mail address is allowed!",
                    "test@test.com;blah@blah.org"),
                "GENC.00007V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com;blah@blah.org'";
            TestResult = TStringChecks.ValidateEmail("test@test.com;blah@blah.org", TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, String.Format(ExpectedErrorText + Environment.NewLine +
                    "Reason: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses. However, in this case only ONE e-mail address is allowed!",
                    "test@test.com;blah@blah.org"),
                "GENC.00007V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);


            ExpectedErrorText = "The e-mail address '{0}' (or a part of it, '{1}') is not valid.";

            Testname = "Test with 'test@test.com.x;blah@blah.org'";
            TestResult = TStringChecks.ValidateEmail("test@test.com.x;blah@blah.org", true);
            ExpectedResult = new TVerificationResult("", String.Format(ExpectedErrorText + Environment.NewLine +
                    "Note: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses.",
                    "test@test.com.x;blah@blah.org", "test@test.com.x"),
                "GENC.00007V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com.x;blah@blah.org'";
            TestResult = TStringChecks.ValidateEmail("test@test.com.x;blah@blah.org", true, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, String.Format(ExpectedErrorText + Environment.NewLine +
                    "Note: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses.",
                    "test@test.com.x;blah@blah.org", "test@test.com.x"),
                "GENC.00007V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com;blah@blah.org.a'";
            TestResult = TStringChecks.ValidateEmail("test@test.com;blah@blah.org.a", true);
            ExpectedResult = new TVerificationResult("", String.Format(ExpectedErrorText + Environment.NewLine +
                    "Note: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses.",
                    "test@test.com;blah@blah.org.a", "blah@blah.org.a"),
                "GENC.00007V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com.x,'";
            TestResult = TStringChecks.ValidateEmail("test@test.com.x,", true, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, String.Format(ExpectedErrorText + Environment.NewLine +
                    "Note: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses.",
                    "test@test.com.x,", "test@test.com.x"),
                "GENC.00007V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            #endregion
        }

        /// <summary>
        /// Test for date verifications.
        /// </summary>
        [Test]
        public void TestDateChecks()
        {
            TVerificationResult ExpectedResult = null;
            TVerificationResult TestResult = null;
            string Testname;
            string ExpectedErrorText;
            string VerificationProblemListing;
            object TestContext = new Object();
            DataColumn TestColumn = new DataColumn("TestColumn");

            System.Windows.Forms.Control TestControl = new System.Windows.Forms.TextBox();

            #region IsNotUndefinedDateTime

            ExpectedErrorText = "'{0}' may not be empty.";

            Testname = "Test with DateTime.Now.Date";
            TestResult = TDateChecks.IsNotUndefinedDateTime(DateTime.Now.Date, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNotUndefinedDateTime: " + Testname);

            Testname = "Test with DateTime.MinValue";
            TestResult = TDateChecks.IsNotUndefinedDateTime(DateTime.MinValue, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TVerificationResult("", "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00002V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNotUndefinedDateTime: " + Testname);

            Testname = "Test with DateTime.MinValue (TScreenVerificationResult)";
            TestResult = TDateChecks.IsNotUndefinedDateTime(DateTime.MinValue, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00002V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNotUndefinedDateTime: " + Testname);

            Testname = "Test with null";
            TestResult = TDateChecks.IsNotUndefinedDateTime(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNotUndefinedDateTime: " + Testname);

            #endregion


            #region IsValidDateTime

            ExpectedErrorText = "'{0}' must be a date.";

            Testname = "Test with DateTime.Now.Date";
            TestResult = TDateChecks.IsValidDateTime(DateTime.Now.Date.ToString(), Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDateTime: " + Testname);

            Testname = "Test with String.Empty";
            TestResult = TDateChecks.IsValidDateTime(String.Empty, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TVerificationResult("", "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00001V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDateTime: " + Testname);

            Testname = "Test with String.Empty (TScreenVerificationResult)";
            TestResult = TDateChecks.IsValidDateTime(String.Empty, Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00001V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDateTime: " + Testname);

            Testname = "Test with null";
            TestResult = TDateChecks.IsValidDateTime(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDateTime: " + Testname);

            #endregion


            #region IsCurrentOr...Date

            #region IsCurrentOrFutureDate

            ExpectedErrorText = "'{0}' may not be a past date.";

            Testname = "Test with DateTime.Now.Date";
            TestResult = TDateChecks.IsCurrentOrFutureDate(DateTime.Now.Date, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrFutureDate: " + Testname);

            Testname = "Test with DateTime.MinValue";
            TestResult = TDateChecks.IsCurrentOrFutureDate(DateTime.MinValue, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrFutureDate: " + Testname);

            Testname = "Test with DateTime.Now.Date.AddDays(5)";
            TestResult = TDateChecks.IsCurrentOrFutureDate(DateTime.Now.Date.AddDays(5), Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrFutureDate: " + Testname);

            Testname = "Test with DateTime.Now.Date.AddDays(-1)";
            TestResult = TDateChecks.IsCurrentOrFutureDate(DateTime.Now.Date.AddDays(-1), Testname);
            ExpectedResult = new TVerificationResult("", "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00004V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrFutureDate: " + Testname);

            Testname = "Test with DateTime.Now.Date.AddDays(-1) (TScreenVerificationResult)";
            TestResult = TDateChecks.IsCurrentOrFutureDate(DateTime.Now.Date.AddDays(-1), Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00004V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrFutureDate: " + Testname);

            Testname = "Test with null";
            TestResult = TDateChecks.IsCurrentOrFutureDate(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrFutureDate: " + Testname);

            #endregion


            #region IsCurrentOrPastDate

            ExpectedErrorText = "'{0}' may not be a future date.";

            Testname = "Test with DateTime.Now.Date";
            TestResult = TDateChecks.IsCurrentOrPastDate(DateTime.Now.Date, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrPastDate: " + Testname);

            Testname = "Test with DateTime.Now.Date.AddDays(-5)";
            TestResult = TDateChecks.IsCurrentOrPastDate(DateTime.Now.Date.AddDays(-5), Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrPastDate: " + Testname);

            Testname = "Test with DateTime.Now.Date.AddDays(1)";
            TestResult = TDateChecks.IsCurrentOrPastDate(DateTime.Now.Date.AddDays(1), Testname);
            ExpectedResult = new TVerificationResult("", "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00003V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrPastDate: " + Testname);

            Testname = "Test with DateTime.Now.Date.AddDays(1) (TScreenVerificationResult)";
            TestResult = TDateChecks.IsCurrentOrPastDate(DateTime.Now.Date.AddDays(1), Testname, TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00003V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrPastDate: " + Testname);

            Testname = "Test with null";
            TestResult = TDateChecks.IsCurrentOrPastDate(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrPastDate: " + Testname);

            #endregion

            #endregion


            #region FirstLesser...ThanSecondDate

            #region FirstLesserOrEqualThanSecondDate

            ExpectedErrorText = "'{0}' cannot be later then '{1}'.";

            Testname = "Test with DateTime.Now.Date, DateTime.Now.Date";
            TestResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateTime.Now.Date, DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date, DateTime.MinValue";
            TestResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateTime.Now.Date, DateTime.MinValue, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date.AddDays(-1), DateTime.Now.Date";
            TestResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date.AddDays(1), DateTime.Now.Date";
            TestResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateTime.Now.Date.AddDays(1), DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00001V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date.AddDays(1), DateTime.Now.Date (TScreenVerificationResult)";
            TestResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateTime.Now.Date.AddDays(
                    1), DateTime.Now.Date, "1st value", "2nd value", TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00001V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDate: " + Testname);

            Testname = "Test with null, DateTime.Now.Date";
            TestResult = TDateChecks.FirstLesserOrEqualThanSecondDate(null, DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date, null";
            TestResult = TDateChecks.FirstLesserOrEqualThanSecondDate(DateTime.Now.Date, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDate: " + Testname);

            Testname = "Test with null, null";
            TestResult = TDateChecks.FirstLesserOrEqualThanSecondDate(null, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserOrEqualThanSecondDate: " + Testname);

            #endregion

            #region FirstLesserThanSecondDate

            ExpectedErrorText = "'{0}' cannot be later than or equal to '{1}'.";

            Testname = "Test with DateTime.Now.Date.AddDays(-1), DateTime.Now.Date";
            TestResult = TDateChecks.FirstLesserThanSecondDate(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date, DateTime.MinValue";
            TestResult = TDateChecks.FirstLesserThanSecondDate(DateTime.Now.Date, DateTime.MinValue, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date, DateTime.Now.Date";
            TestResult = TDateChecks.FirstLesserThanSecondDate(DateTime.Now.Date, DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00001V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date, DateTime.Now.Date (TScreenVerificationResult)";
            TestResult = TDateChecks.FirstLesserThanSecondDate(DateTime.Now.Date,
                DateTime.Now.Date,
                "1st value",
                "2nd value",
                TestContext,
                TestColumn,
                TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00001V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDate: " + Testname);

            Testname = "Test with null, DateTime.Now.Date";
            TestResult = TDateChecks.FirstLesserThanSecondDate(null, DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date, null";
            TestResult = TDateChecks.FirstLesserThanSecondDate(DateTime.Now.Date, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDate: " + Testname);

            Testname = "Test with null, null";
            TestResult = TDateChecks.FirstLesserThanSecondDate(null, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDate: " + Testname);

            #endregion

            #endregion


            #region FirstGreater...ThanSecondDate

            #region FirstGreaterOrEqualThanSecondDate

            ExpectedErrorText = "'{0}' cannot be earlier than '{1}'.";

            Testname = "Test with DateTime.Now.Date, DateTime.Now.Date";
            TestResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(DateTime.Now.Date, DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterOrEqualThanSecondDate: " + Testname);

            Testname = "Test with DateTime.MinValue, DateTime.Now.Date";
            TestResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(DateTime.MinValue, DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterOrEqualThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date.AddDays(1), DateTime.Now.Date";
            TestResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(DateTime.Now.Date.AddDays(1), DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterOrEqualThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date.AddDays(-1), DateTime.Now.Date";
            TestResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00001V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterOrEqualThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date.AddDays(-1), DateTime.Now.Date (TScreenVerificationResult)";
            TestResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(DateTime.Now.Date.AddDays(
                    -1), DateTime.Now.Date, "1st value", "2nd value", TestContext, TestColumn, TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00001V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterOrEqualThanSecondDate: " + Testname);

            Testname = "Test with null, DateTime.Now.Date";
            TestResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(null, DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterOrEqualThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date, null";
            TestResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(DateTime.Now.Date, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterOrEqualThanSecondDate: " + Testname);

            Testname = "Test with null, null";
            TestResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(null, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterOrEqualThanSecondDate: " + Testname);

            #endregion

            #region FirstGreaterThanSecondDate

            ExpectedErrorText = "'{0}' cannot be earlier than or equal to '{1}'.";

            Testname = "Test with DateTime.Now.Date.AddDays(1), DateTime.Now.Date";
            TestResult = TDateChecks.FirstGreaterThanSecondDate(DateTime.Now.Date.AddDays(1), DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterThanSecondDate: " + Testname);

            Testname = "Test with DateTime.MinValue, DateTime.Now.Date";
            TestResult = TDateChecks.FirstGreaterThanSecondDate(DateTime.MinValue, DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date, DateTime.Now.Date";
            TestResult = TDateChecks.FirstGreaterThanSecondDate(DateTime.Now.Date, DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult("", "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00001V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date, DateTime.Now.Date (TScreenVerificationResult)";
            TestResult = TDateChecks.FirstGreaterThanSecondDate(DateTime.Now.Date,
                DateTime.Now.Date,
                "1st value",
                "2nd value",
                TestContext,
                TestColumn,
                TestControl);
            ExpectedResult = new TScreenVerificationResult("", TestColumn, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00001V", TestControl, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterThanSecondDate: " + Testname);

            Testname = "Test with null, DateTime.Now.Date";
            TestResult = TDateChecks.FirstGreaterThanSecondDate(null, DateTime.Now.Date, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterThanSecondDate: " + Testname);

            Testname = "Test with DateTime.Now.Date, null";
            TestResult = TDateChecks.FirstGreaterThanSecondDate(DateTime.Now.Date, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterThanSecondDate: " + Testname);

            Testname = "Test with null, null";
            TestResult = TDateChecks.FirstGreaterThanSecondDate(null, null, "1st value", "2nd value");
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstGreaterThanSecondDate: " + Testname);

            #endregion

            #endregion


            #region Helper Methods

            #region TVerificationResult Constructors not covered by tests above

            Testname = "New TVerificationResult with ErrCodeInfo Constructor (Test #1)";
            TestResult = new TVerificationResult("ResultContext", new ErrCodeInfo(
                    "TEST.00001V", "Ict.Common.Verification.Testing", "ERR_TEST_ONLY", "ShortDescription", "FullDescription",
                    "ErrorMessageText", "ErrorMessageTitle", ErrCodeCategory.Validation, "HelpID"));
            ExpectedResult = new TVerificationResult("ResultContext", "ErrorMessageText", "ErrorMessageTitle",
                "TEST.00001V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "TVerificationResult Constructors: " + Testname);

            Testname = "New TVerificationResult with ErrCodeInfo Constructor (Test #2)";
            TestResult = new TVerificationResult("ResultContext", new ErrCodeInfo(
                    "TEST.00001V", "Ict.Common.Verification.Testing", "ERR_TEST_ONLY", "ShortDescription", "FullDescription",
                    String.Empty, String.Empty, ErrCodeCategory.Error, "HelpID"));
            ExpectedResult = new TVerificationResult("ResultContext", "ShortDescription", String.Empty,
                "TEST.00001V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "TVerificationResult Constructors: " + Testname);

            Testname = "New TVerificationResult with ErrCodeInfo Constructor (Test #3)";
            TestResult = new TVerificationResult("ResultContext", new ErrCodeInfo(
                    "TEST.00001V", "Ict.Common.Verification.Testing", "ERR_TEST_ONLY", "ShortDescription", "FullDescription",
                    String.Empty, String.Empty, ErrCodeCategory.NonCriticalError, "HelpID"));
            ExpectedResult = new TVerificationResult("ResultContext", "ShortDescription", String.Empty,
                "TEST.00001V", TResultSeverity.Resv_Noncritical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "TVerificationResult Constructors: " + Testname);

            #endregion


            #region AreVerificationResultsIdentical

            ExpectedResult = new TVerificationResult("ResultContext", "ResultText", "ResultTextCaption",
                "ResultCode", TResultSeverity.Resv_Critical);

            Testname = "AreVerificationResultsIdentical (all values equal)";
            Assert.IsTrue(TVerificationHelper.AreVerificationResultsIdentical(ExpectedResult, ExpectedResult), Testname);

            Testname = "AreVerificationResultsIdentical (all values equal but ResultCode)";
            Assert.IsFalse(TVerificationHelper.AreVerificationResultsIdentical(ExpectedResult,
                    new TVerificationResult("ResultContext", "ResultText", "ResultTextCaption",
                        "ResultCode2", TResultSeverity.Resv_Critical)), Testname);

            Testname = "AreVerificationResultsIdentical (all values equal but ResultContext)";
            Assert.IsFalse(TVerificationHelper.AreVerificationResultsIdentical(ExpectedResult,
                    new TVerificationResult("ResultContext2", "ResultText", "ResultTextCaption",
                        "ResultCode", TResultSeverity.Resv_Critical)), Testname);

            Testname = "AreVerificationResultsIdentical (all values equal but ResultSeverity)";
            Assert.IsFalse(TVerificationHelper.AreVerificationResultsIdentical(ExpectedResult,
                    new TVerificationResult("ResultContext", "ResultText", "ResultTextCaption",
                        "ResultCode", TResultSeverity.Resv_Noncritical)), Testname);

            Testname = "AreVerificationResultsIdentical (all values equal except ResultText)";
            Assert.IsFalse(TVerificationHelper.AreVerificationResultsIdentical(ExpectedResult,
                    new TVerificationResult("ResultContext", "ResultText2", "ResultTextCaption",
                        "ResultCode", TResultSeverity.Resv_Critical)), Testname);

            Testname = "AreVerificationResultsIdentical (all values equal except ResultTextCaption)";
            Assert.IsFalse(TVerificationHelper.AreVerificationResultsIdentical(ExpectedResult,
                    new TVerificationResult("ResultContext", "ResultText", "ResultTextCaption2",
                        "ResultCode", TResultSeverity.Resv_Critical)), Testname);

            Testname = "AreVerificationResultsIdentical with null, values)";
            Assert.IsFalse(TVerificationHelper.AreVerificationResultsIdentical(null, ExpectedResult), Testname);

            Testname = "AreVerificationResultsIdentical with values, null)";
            Assert.IsFalse(TVerificationHelper.AreVerificationResultsIdentical(ExpectedResult, null), Testname);

            Testname = "AreVerificationResultsIdentical with null, null)";
            Assert.IsTrue(TVerificationHelper.AreVerificationResultsIdentical(null, null), Testname);

            #endregion


            #region GetInvalidDateVerificationResult

            ExpectedErrorText = "'{0}' must be a date.";

            Testname = "Invalid date string formatting";
            TestResult = TDateChecks.GetInvalidDateVerificationResult(Testname);
            ExpectedResult = new TVerificationResult("", "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00001V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "GetInvalidDateVerificationResult: " + Testname);

            #endregion


            #region NiceValueDescription

            string TestResultString;

            ExpectedErrorText = "'NiceValueDescription'";

            Testname = "NiceValueDescription";
            TestResultString = THelper.NiceValueDescription(Testname);
            Assert.IsTrue(String.Compare(ExpectedErrorText, TestResultString) == 0, Testname);

            Testname = "NiceValueDescription:";
            TestResultString = THelper.NiceValueDescription(Testname);
            Assert.IsTrue(String.Compare(ExpectedErrorText, TestResultString) == 0, Testname);

            ExpectedErrorText = "Value";

            Testname = "NiceValueDescription with String.Empty";
            TestResultString = THelper.NiceValueDescription(String.Empty);
            Assert.IsTrue(String.Compare(ExpectedErrorText, TestResultString) == 0, Testname);

            #endregion

            #endregion


            #region This Test Case's Private Methods ('self-test')

            Testname = "EvaluateVerificationResults (identical values)";
            TestResult = new TVerificationResult("ResultContext", "ErrorMessageText", "ErrorMessageTitle",
                "TEST.00001V", TResultSeverity.Resv_Critical);
            ExpectedResult = new TVerificationResult("ResultContext", "ErrorMessageText", "ErrorMessageTitle",
                "TEST.00001V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "EvaluateVerificationResults: " + Testname);

            Testname = "EvaluateVerificationResults (non-identical values)";
            TestResult = new TVerificationResult("ResultContext", "ErrorMessageText", "ErrorMessageTitle",
                "TEST.00001V", TResultSeverity.Resv_Critical);
            ExpectedResult = new TVerificationResult("ResultContext2", "ErrorMessageText", "ErrorMessageTitle",
                "TEST.00001V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsTrue(String.Compare(
                    VerificationProblemListing,
                    "ResultContext: ResultContext2, ResultText: ErrorMessageText, ResultTextCaption: ErrorMessageTitle, ResultCode TEST.00001V, ResultSeverity: Resv_Critical."
                    + Environment.NewLine +
                    "ResultContext: ResultContext, ResultText: ErrorMessageText, ResultTextCaption: ErrorMessageTitle, ResultCode TEST.00001V, ResultSeverity: Resv_Critical.")
                == 0,
                "EvaluateVerificationResults: " + Testname);

            #endregion
        }
    }
}