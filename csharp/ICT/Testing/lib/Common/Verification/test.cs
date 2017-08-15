//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2017 by OM International
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
            new TLogging("../../log/test.log");
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

            #region IsValid...

            #region IsValidInteger

            ExpectedErrorText = "'{0}' must be a number without a decimal point.";

            Testname = "Test with 5";
            TestResult = TNumericalChecks.IsValidInteger("5", Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidInteger: " + Testname);

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsValidInteger("5.1", Testname);
            ExpectedResult = new TVerificationResult(TestContext, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidInteger: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsValidInteger(null, Testname);
            ExpectedResult = new TScreenVerificationResult(null, null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", null, TResultSeverity.Resv_Critical);;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidInteger: " + Testname);

            Testname = "Test with String.Empty";
            TestResult = TNumericalChecks.IsValidInteger(String.Empty, Testname);
//            ExpectedResult = null;
//            ExpectedResult = new TVerificationResult(String.Empty, ErrorCodes.GetErrorInfo("GENC.00005V",
//                "'oooooooooooo' must be an integer (= a number without a fraction)."));
//            ExpectedResult = new TVerificationResult(String.Empty, ErrorCodes.GetErrorInfo("GENC.00005V",
//                "Invalid number entered." + Environment.NewLine + "'Test with String.Empty' must be an integer (= a number without a fraction)."));
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidInteger: " + Testname);

            #endregion


            #region IsValidDouble

            ExpectedErrorText = "'{0}' must be a number with a decimal point.";

            Testname = "Test with -5";
            TestResult = TNumericalChecks.IsValidDouble("-5", Testname);
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
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDouble: " + Testname);

            Testname = "Test with String.Empty";
            TestResult = TNumericalChecks.IsValidDouble(String.Empty, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDouble: " + Testname);

            #endregion


            #region IsValidDecimal

            ExpectedErrorText = "'{0}' must be a number with a decimal point.";

            Testname = "Test with -5";
            TestResult = TNumericalChecks.IsValidDecimal("-5", Testname);
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
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDecimal: " + Testname);

            Testname = "Test with String.Empty";
            TestResult = TNumericalChecks.IsValidDecimal(String.Empty, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDecimal: " + Testname);

            #endregion

            #endregion


            #region IsPositive...

            #region IsPositiveInteger

            ExpectedErrorText = "'{0}' must be a positive number without a decimal point.";

            Testname = "Test with 5";
            TestResult = TNumericalChecks.IsPositiveInteger(5, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveInteger: " + Testname);

            Testname = "Test with 0";
            TestResult = TNumericalChecks.IsPositiveInteger(0, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveInteger: " + Testname);

            Testname = "Test with -1";
            TestResult = TNumericalChecks.IsPositiveInteger(-1, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveInteger: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsPositiveInteger(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveInteger: " + Testname);

            #endregion

            #region IsPositiveDouble

            ExpectedErrorText = "'{0}' must be a positive number with a decimal point.";

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsPositiveDouble(5.1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveDouble: " + Testname);

            Testname = "Test with 0.0";
            TestResult = TNumericalChecks.IsPositiveDouble(0.0, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveDouble: " + Testname);

            Testname = "Test with -0.1";
            TestResult = TNumericalChecks.IsPositiveDouble(-0.1, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveDouble: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsPositiveDouble(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveDouble: " + Testname);

            #endregion

            #region IsPositiveDecimal

            ExpectedErrorText = "'{0}' must be a positive number with a decimal point.";

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsPositiveDecimal((decimal)5.1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveDecimal: " + Testname);

            Testname = "Test with 0.0";
            TestResult = TNumericalChecks.IsPositiveDecimal((decimal)0.0, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveDecimal: " + Testname);

            Testname = "Test with -0.1";
            TestResult = TNumericalChecks.IsPositiveDecimal((decimal) - 0.1, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveDecimal: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsPositiveDecimal(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveDecimal: " + Testname);

            #endregion

            #endregion


            #region IsPositiveOrZero...

            #region IsPositiveOrZeroInteger

            ExpectedErrorText = "'{0}' must be a positive number without a decimal point, or 0.";

            Testname = "Test with 5";
            TestResult = TNumericalChecks.IsPositiveOrZeroInteger(5, Testname);
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
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroInteger: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsPositiveOrZeroInteger(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroInteger: " + Testname);

            #endregion

            #region IsPositiveOrZeroDouble

            ExpectedErrorText = "'{0}' must be a positive number with a decimal point, or 0.";

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsPositiveOrZeroDouble(5.1, Testname);
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
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDouble: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsPositiveOrZeroDouble(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDouble: " + Testname);

            #endregion

            #region IsPositiveOrZeroDecimal

            ExpectedErrorText = "'{0}' must be a positive number with a decimal point, or 0.";

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsPositiveOrZeroDecimal((decimal)5.1, Testname);
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
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDecimal: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsPositiveOrZeroDecimal(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsPositiveOrZeroDecimal: " + Testname);

            #endregion

            #endregion


            #region IsNegative...

            #region IsNegativeInteger

            ExpectedErrorText = "'{0}' must be a negative number without a decimal point.";

            Testname = "Test with 5";
            TestResult = TNumericalChecks.IsNegativeInteger(-5, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeInteger: " + Testname);

            TestResult = TNumericalChecks.IsNegativeInteger(0, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeInteger: " + Testname);

            Testname = "Test with -1";
            TestResult = TNumericalChecks.IsNegativeInteger(1, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeInteger: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsNegativeInteger(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeInteger: " + Testname);

            #endregion

            #region IsNegativeDouble

            ExpectedErrorText = "'{0}' must be a negative number with a decimal point.";

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsNegativeDouble(-5.1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeDouble: " + Testname);

            Testname = "Test with 0.0";
            TestResult = TNumericalChecks.IsNegativeDouble(0.0, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeDouble: " + Testname);

            Testname = "Test with -0.1";
            TestResult = TNumericalChecks.IsNegativeDouble(0.1, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeDouble: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsNegativeDouble(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeDouble: " + Testname);

            #endregion

            #region IsNegativeDecimal

            ExpectedErrorText = "'{0}' must be a negative number with a decimal point.";

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsNegativeDecimal((decimal) - 5.1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeDecimal: " + Testname);

            Testname = "Test with 0.0";
            TestResult = TNumericalChecks.IsNegativeDecimal((decimal)0.0, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeDecimal: " + Testname);

            Testname = "Test with -0.1";
            TestResult = TNumericalChecks.IsNegativeDecimal((decimal)0.1, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeDecimal: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsNegativeDecimal(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeDecimal: " + Testname);

            #endregion

            #endregion


            #region IsNegativeOrZero...

            #region IsNegativeOrZeroInteger

            ExpectedErrorText = "'{0}' must be a negative number without a decimal point, or 0.";

            Testname = "Test with 5";
            TestResult = TNumericalChecks.IsNegativeOrZeroInteger(-5, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeOrZeroInteger: " + Testname);

            Testname = "Test with 0";
            TestResult = TNumericalChecks.IsNegativeOrZeroInteger(0, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeOrZeroInteger: " + Testname);

            Testname = "Test with -1";
            TestResult = TNumericalChecks.IsNegativeOrZeroInteger(1, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeOrZeroInteger: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsNegativeOrZeroInteger(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeOrZeroInteger: " + Testname);

            #endregion

            #region IsNegativeOrZeroDouble

            ExpectedErrorText = "'{0}' must be a negative number with a decimal point, or 0.";

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsNegativeOrZeroDouble(-5.1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeOrZeroDouble: " + Testname);

            TestResult = TNumericalChecks.IsNegativeOrZeroDouble(0.0, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeOrZeroDouble: " + Testname);

            Testname = "Test with -0.1";
            TestResult = TNumericalChecks.IsNegativeOrZeroDouble(0.1, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeOrZeroDouble: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsNegativeOrZeroDouble(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeOrZeroDouble: " + Testname);

            #endregion

            #region IsNegativeOrZeroDecimal

            ExpectedErrorText = "'{0}' must be a negative number with a decimal point, or 0.";

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsNegativeOrZeroDecimal((decimal) - 5.1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeOrZeroDecimal: " + Testname);

            Testname = "Test with 0.0";
            TestResult = TNumericalChecks.IsNegativeOrZeroDecimal((decimal)0.0, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeOrZeroDecimal: " + Testname);

            Testname = "Test with -0.1";
            TestResult = TNumericalChecks.IsNegativeOrZeroDecimal((decimal)0.1, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeOrZeroDecimal: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsNegativeOrZeroDecimal(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNegativeOrZeroDecimal: " + Testname);

            #endregion

            #endregion

            #region IsNonZero...

            #region IsNonZeroInteger

            ExpectedErrorText = "'{0}' must be a positive or negative number but not zero.";

            Testname = "Test with 5";
            TestResult = TNumericalChecks.IsNonZeroInteger(5, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNonZeroInteger: " + Testname);

            Testname = "Test with 0";
            TestResult = TNumericalChecks.IsNonZeroInteger(0, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNonZeroInteger: " + Testname);

            Testname = "Test with -1";
            TestResult = TNumericalChecks.IsNonZeroInteger(-1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNonZeroInteger: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsNonZeroInteger(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNonZeroInteger: " + Testname);

            #endregion

            #region IsNonZeroDouble

            ExpectedErrorText = "'{0}' must be a positive or negative number but not zero.";

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsNonZeroDouble(-5.1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNonZeroDouble: " + Testname);

            Testname = "Test with 0.0";
            TestResult = TNumericalChecks.IsNonZeroDouble(0.0, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNonZeroDouble: " + Testname);

            Testname = "Test with -0.1";
            TestResult = TNumericalChecks.IsNonZeroDouble(0.1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNonZeroDouble: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsNonZeroDouble(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNonZeroDouble: " + Testname);

            #endregion

            #region IsNonZeroDecimal

            ExpectedErrorText = "'{0}' must be a positive or negative number but not zero.";

            Testname = "Test with 5.1";
            TestResult = TNumericalChecks.IsNonZeroDecimal((decimal) - 5.1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNonZeroDecimal: " + Testname);

            Testname = "Test with 0.0";
            TestResult = TNumericalChecks.IsNonZeroDecimal((decimal)0.0, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00005V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNonZeroDecimal: " + Testname);

            Testname = "Test with -0.1";
            TestResult = TNumericalChecks.IsNonZeroDecimal((decimal)0.1, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNonZeroDecimal: " + Testname);

            Testname = "Test with null";
            TestResult = TNumericalChecks.IsNonZeroDecimal(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNonZeroDecimal: " + Testname);

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

            Testname = "Test with 6 and 5";
            TestResult = TNumericalChecks.FirstLesserThanSecondInteger(6, 5, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondInteger: " + Testname);

            Testname = "Test with 0 and 0";
            TestResult = TNumericalChecks.FirstLesserThanSecondInteger(0, 0, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondInteger: " + Testname);

            Testname = "Test with -1 and -2";
            TestResult = TNumericalChecks.FirstLesserThanSecondInteger(-1, -2, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
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

            Testname = "Test with 5.2 and 5.1";
            TestResult = TNumericalChecks.FirstLesserThanSecondDouble(5.2, 5.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDouble: " + Testname);

            Testname = "Test with 0.0 and 0.0";
            TestResult = TNumericalChecks.FirstLesserThanSecondDouble(0.0, 0.0, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDouble: " + Testname);

            Testname = "Test with -1.0 and -1.1";
            TestResult = TNumericalChecks.FirstLesserThanSecondDouble(-1.0, -1.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
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

            Testname = "Test with 5.2 and 5.1";
            TestResult = TNumericalChecks.FirstLesserThanSecondDecimal((decimal)5.2, (decimal)5.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDecimal: " + Testname);

            Testname = "Test with 0.0 and 0.0";
            TestResult = TNumericalChecks.FirstLesserThanSecondDecimal((decimal)0.0, (decimal)0.0, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "FirstLesserThanSecondDecimal: " + Testname);

            Testname = "Test with -1.0 and -1.1";
            TestResult = TNumericalChecks.FirstLesserThanSecondDecimal((decimal) - 1.0, (decimal) - 1.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
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

            Testname = "Test with 6 and 5";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondInteger(6, 5, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
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
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
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

            Testname = "Test with 5.2 and 5.1";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDouble(5.2, 5.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
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
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
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

            Testname = "Test with 5.2 and 5.1";
            TestResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal((decimal)5.2, (decimal)5.1, "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00006V", TResultSeverity.Resv_Critical);
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
            ExpectedResult = new TVerificationResult(null, "Invalid number entered." + Environment.NewLine +
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

            #region StringMustNotBeEmpty

            ExpectedErrorText = "A value must be entered for '{0}'.";

            Testname = "Test with 'blahblah'";
            TestResult = TStringChecks.StringMustNotBeEmpty("blahblah", Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "StringMustNotBeEmpty: " + Testname);

            Testname = "Test with String.Empty";
            TestResult = TStringChecks.StringMustNotBeEmpty(String.Empty, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid value entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00008V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "StringMustNotBeEmpty: " + Testname);

            Testname = "Test with null";
            TestResult = TStringChecks.StringMustNotBeEmpty(null, Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid value entered." + Environment.NewLine +
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

            Testname = "Test with 'aab' and 'aaa'";
            TestResult = TStringChecks.FirstLesserOrEqualThanSecondString("aab", "aaa", "1st value", "2nd value");
            ExpectedResult = new TVerificationResult(null,
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00007V", TResultSeverity.Resv_Critical);
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
            ExpectedResult = new TVerificationResult(null, String.Format(ExpectedErrorText, "test@test.com.x"),
                "GENC.00017V", TResultSeverity.Resv_Critical);
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
            ExpectedResult = new TVerificationResult(null, String.Format(ExpectedErrorText + Environment.NewLine +
                    "Note: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses.",
                    "test@test.com;;"),
                "GENC.00017V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com; ,'";
            TestResult = TStringChecks.ValidateEmail("test@test.com; ,", true);
            ExpectedResult = new TVerificationResult(null, String.Format(ExpectedErrorText + Environment.NewLine +
                    "Note: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses.",
                    "test@test.com; ,"),
                "GENC.00017V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com;blah@blah.org'";
            TestResult = TStringChecks.ValidateEmail("test@test.com;blah@blah.org");
            ExpectedResult = new TVerificationResult(null, String.Format(ExpectedErrorText + Environment.NewLine +
                    "Reason: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses. However, in this case only ONE e-mail address is allowed!",
                    "test@test.com;blah@blah.org"),
                "GENC.00017V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            ExpectedErrorText = "The e-mail address '{0}' (or a part of it, '{1}') is not valid.";

            Testname = "Test with 'test@test.com.x;blah@blah.org'";
            TestResult = TStringChecks.ValidateEmail("test@test.com.x;blah@blah.org", true);
            ExpectedResult = new TVerificationResult(null, String.Format(ExpectedErrorText + Environment.NewLine +
                    "Note: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses.",
                    "test@test.com.x;blah@blah.org", "test@test.com.x"),
                "GENC.00017V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "ValidateEmail: " + Testname);

            Testname = "Test with 'test@test.com;blah@blah.org.a'";
            TestResult = TStringChecks.ValidateEmail("test@test.com;blah@blah.org.a", true);
            ExpectedResult = new TVerificationResult(null, String.Format(ExpectedErrorText + Environment.NewLine +
                    "Note: The submitted e-mail address contains a comma (',') or a semicolon (';'). These characters are not valid in an e-mail address, but they can be used to separate several e-mail addresses.",
                    "test@test.com;blah@blah.org.a", "blah@blah.org.a"),
                "GENC.00017V", TResultSeverity.Resv_Critical);
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

            #region IsNotUndefinedDateTime

            ExpectedErrorText = "'{0}' must not be empty.";

            Testname = "Test with DateTime.Now.Date";
            TestResult = TDateChecks.IsNotUndefinedDateTime(DateTime.Now.Date, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsNotUndefinedDateTime: " + Testname);

            Testname = "Test with null #1";
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

            Testname = "Test with null";
            TestResult = TDateChecks.IsValidDateTime(null, Testname);
            ExpectedResult = new TScreenVerificationResult(null, TestColumn, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00001V", null, TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsValidDateTime: " + Testname);

            #endregion


            #region IsCurrentOr...Date

            #region IsCurrentOrFutureDate

            ExpectedErrorText = "'{0}' must not be a past date.";

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
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00004V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrFutureDate: " + Testname);

            Testname = "Test with null";
            TestResult = TDateChecks.IsCurrentOrFutureDate(null, Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsCurrentOrFutureDate: " + Testname);

            #endregion


            #region IsCurrentOrPastDate

            ExpectedErrorText = "'{0}' must not be a future date.";

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
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00003V", TResultSeverity.Resv_Critical);
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
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00001V", TResultSeverity.Resv_Critical);
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
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00001V", TResultSeverity.Resv_Critical);
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
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00001V", TResultSeverity.Resv_Critical);
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
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, "1st value", "2nd value"), "GENC.00001V", TResultSeverity.Resv_Critical);
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


            #region IsDateBetweenDates

            Testname = "Test with null, Date, Date";
            TestResult = TDateChecks.IsDateBetweenDates(null, new DateTime(2000, 1, 1), new DateTime(2099, 12, 1),
                Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            Testname = "Test with DateTime.Now.Date, null, Date";
            TestResult = TDateChecks.IsDateBetweenDates(DateTime.Now.Date, null, new DateTime(2099, 12, 1),
                Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            Testname = "Test with DateTime.Now.Date, null, null";
            TestResult = TDateChecks.IsDateBetweenDates(DateTime.Now.Date, null, null,
                Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            Testname = "Test with null, null, null";
            TestResult = TDateChecks.IsDateBetweenDates(null, null, null,
                Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            Testname = "Test with DateTime.Now.Date";
            TestResult = TDateChecks.IsDateBetweenDates(DateTime.Now.Date, new DateTime(2000, 1, 1), new DateTime(2099, 12, 1),
                Testname);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            Testname = "Test with Jan. 1st, 1850 for lower date and DateTime.Today.Year for upper date: validated date is Jan. 1st, 1850";
            TestResult = TDateChecks.IsDateBetweenDates(new DateTime(1850, 1, 1), new DateTime(1850, 1, 1), new DateTime(DateTime.Today.Year, 12, 31),
                Testname, TDateBetweenDatesCheckType.dbdctUnrealisticDate, TDateBetweenDatesCheckType.dbdctNoFutureDate);
            ExpectedResult = null;
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            ExpectedErrorText = "'{0}' is not allowed as it does not lie within the required date range. It must lie between {1} and {2}.";

            Testname = "Test with Jan. 1st, 1850 for lower date and DateTime.Today.Year for upper date: validated date is Dec. 31st, 1849";
            TestResult = TDateChecks.IsDateBetweenDates(new DateTime(1849, 1, 1), new DateTime(1850, 1, 1), new DateTime(DateTime.Today.Year, 12, 31),
                Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname, StringHelper.DateToLocalizedString(new DateTime(1850, 1, 1)),
                    StringHelper.DateToLocalizedString(new DateTime(DateTime.Today.Year, 12, 31))), "GENC.00013V",
                TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            ExpectedErrorText = "'{0}' is not allowed as it does not lie within the required date range. It must lie between {1} and {2}.";

            Testname = "Test with Jan. 1st, 1850 for lower date and DateTime.Today.Year for upper date: validated date is Jan. 1st of next year.";
            TestResult =
                TDateChecks.IsDateBetweenDates(new DateTime(DateTime.Today.Year, 12,
                        31).AddDays(1), new DateTime(1850, 1, 1), new DateTime(DateTime.Today.Year, 12, 31),
                    Testname);
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname, StringHelper.DateToLocalizedString(new DateTime(1850, 1, 1)),
                    StringHelper.DateToLocalizedString(new DateTime(DateTime.Today.Year, 12, 31))), "GENC.00013V",
                TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            ExpectedErrorText = "'{0}' is not a sensible value in this case.";

            Testname = "Test with Jan. 1st, 1850 for lower date and DateTime.Today.Year for upper date: validated date is Dec. 31st, 1849";
            TestResult =
                TDateChecks.IsDateBetweenDates(new DateTime(1849, 12, 31), new DateTime(1850, 1, 1), new DateTime(DateTime.Today.Year, 12, 31),
                    Testname, TDateBetweenDatesCheckType.dbdctUnrealisticDate, TDateBetweenDatesCheckType.dbdctNoFutureDate);
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00014V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            ExpectedErrorText = "'{0}' must not be a future date.";

            Testname =
                "Test with Jan. 1st, 1850 for lower date and DateTime.Today for upper date: validated date is today plus one day (future date)";
            TestResult = TDateChecks.IsDateBetweenDates(DateTime.Today.AddDays(1), new DateTime(1850, 1, 1), DateTime.Today,
                Testname, TDateBetweenDatesCheckType.dbdctUnrealisticDate, TDateBetweenDatesCheckType.dbdctNoFutureDate);
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00003V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            ExpectedErrorText = "'{0}' must not be a past date.";

            Testname = "Test with today for lower date and Jan 1st, 2099 for upper date: validated date is today minus one day (past date)";
            TestResult = TDateChecks.IsDateBetweenDates(DateTime.Today.AddDays(-1), DateTime.Today, new DateTime(2099, 1, 1),
                Testname, TDateBetweenDatesCheckType.dbdctNoPastDate, TDateBetweenDatesCheckType.dbdctUnspecific);
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00004V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            ExpectedErrorText = "'{0}' is not a sensible value in this case.";

            Testname =
                "Test with Jan. 1st, 1850 for lower date and DateTime.Today for upper date: validated date is today plus eleven years (unrealistic date)";
            TestResult = TDateChecks.IsDateBetweenDates(DateTime.Today.AddYears(11), new DateTime(1850, 1, 1), DateTime.Today,
                Testname, TDateBetweenDatesCheckType.dbdctUnrealisticDate, TDateBetweenDatesCheckType.dbdctUnrealisticDate);
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname), "GENC.00014V", TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            ExpectedErrorText = "'{0}' is not allowed as it does not lie within the required date range. It must lie between {1} and {2}.";

            Testname = "Test with Jan. 1st, 1850 for lower date and DateTime.Today.Year for upper date: validated date is Dec. 31st, 1849";
            TestResult = TDateChecks.IsDateBetweenDates(new DateTime(1849, 1, 1), new DateTime(1850, 1, 1), new DateTime(DateTime.Today.Year, 12, 31),
                Testname, TDateBetweenDatesCheckType.dbdctUnspecific, TDateBetweenDatesCheckType.dbdctNoPastDate);
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname, StringHelper.DateToLocalizedString(new DateTime(1850, 1, 1)),
                    StringHelper.DateToLocalizedString(new DateTime(DateTime.Today.Year, 12, 31))), "GENC.00013V",
                TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

            Testname = "Test with Jan. 1st, 1850 for lower date and DateTime.Today for upper date: validated date is Jan. 1st, 2099(future date)";
            TestResult = TDateChecks.IsDateBetweenDates(new DateTime(2099, 1, 1), new DateTime(1850, 1, 1), new DateTime(2012, 1, 1),
                Testname, TDateBetweenDatesCheckType.dbdctUnrealisticDate, TDateBetweenDatesCheckType.dbdctUnspecific);
            ExpectedResult = new TVerificationResult(null, "Invalid date entered." + Environment.NewLine +
                String.Format(ExpectedErrorText, Testname, StringHelper.DateToLocalizedString(new DateTime(1850, 1, 1)),
                    StringHelper.DateToLocalizedString(new DateTime(2012, 1, 1))), "GENC.00013V",
                TResultSeverity.Resv_Critical);
            VerificationProblemListing = EvaluateVerificationResults(ExpectedResult, TestResult);
            Assert.IsEmpty(VerificationProblemListing, "IsDateBetweenDates: " + Testname);

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
            TestResult = TDateChecks.GetInvalidDateVerificationResult(Testname, TestContext);
            ExpectedResult = new TVerificationResult(TestContext, "Invalid date entered." + Environment.NewLine +
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
                    +
                    Environment.NewLine +
                    "ResultContext: ResultContext, ResultText: ErrorMessageText, ResultTextCaption: ErrorMessageTitle, ResultCode TEST.00001V, ResultSeverity: Resv_Critical.")
                == 0,
                "EvaluateVerificationResults: " + Testname);

            #endregion
        }
    }
}
