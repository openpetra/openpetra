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
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Ict.Petra.Client.MReporting.Logic;
using Tests.Reporting;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Printing;
using Ict.Petra.Shared.MReporting;
using Ict.Testing.NUnitPetraClient;

namespace Tests.Reporting
{
    /// This is a test for the reports.
    /// It runs as a NUnit test, and the login is defined in the config file.
    [TestFixture]
    public class TReportingTest
    {
        private CultureInfo OrigCulture;

        /// the object that is able to deal with all the parameters, and can calculate a report
        public TRptCalculator FCalculator;

        /// <summary>
        /// ...
        /// </summary>
        public String PathToTestData;
        /// <summary>
        /// ...
        /// </summary>
        public String PathToSettingsData;

        /// <summary>
        /// ...
        /// </summary>
        [SetUp]
        public void Init()
        {
            new TLogging("../../log/test.log");

            // TODO: what about different cultures?
            OrigCulture = new CultureInfo("en-GB", false);
            Thread.CurrentThread.CurrentCulture = OrigCulture;
            TPetraConnector.Connect("../../etc/TestClient.config");
            FCalculator = new TRptCalculator();
            PathToTestData = TAppSettingsManager.GetValue("Testing.Path") + "/lib/Reporting/TestData/".Replace("/",
                System.IO.Path.DirectorySeparatorChar.ToString());
            PathToSettingsData = TAppSettingsManager.GetValue("Reporting.PathReportSettings") + "/".Replace("/",
                System.IO.Path.DirectorySeparatorChar.ToString());
        }

        /// <summary>
        /// ...
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            TPetraConnector.Disconnect();
        }

        /// <summary>
        /// ...
        /// </summary>
        public static void PrintTxt(TResultList results, TParameterList parameters, string output)
        {
            TReportPrinterLayout reportTxtPrinter;
            TTxtPrinter txtPrinter;

            txtPrinter = new TTxtPrinter();
            reportTxtPrinter = new TReportPrinterLayout(results, parameters, txtPrinter, true);
            reportTxtPrinter.PrintReport();
            txtPrinter.WriteToFile(output);
        }

        /// <summary>
        /// ...
        /// </summary>
        public void TestDetailReport(String ATestPath, String ASettingsPath)
        {
            String detailReportCSV;
            String action;
            String query;
            String paramName;
            String paramValue;
            int SelectedRow;
            int columnCounter;

            detailReportCSV = FCalculator.GetParameters().Get("param_detail_report_0").ToString();
            TLogging.Log("detail report: " + StringHelper.GetNextCSV(ref detailReportCSV, ","));
            action = StringHelper.GetNextCSV(ref detailReportCSV, ",");

            if (action == "PartnerEditScreen")
            {
                /* get the partner key from the parameter */
                SelectedRow = 1;

                if (FCalculator.GetResults().GetResults().Count > 0)
                {
                    TResult row = (TResult)FCalculator.GetResults().GetResults()[SelectedRow];
                    Console.WriteLine("detailReportCSV " + detailReportCSV.ToString());
                    Console.WriteLine(FCalculator.GetResults().GetResults().Count.ToString());

                    if (row.column.Length > 0)
                    {
                        TLogging.Log("Open Partner Edit Screen for partner " + row.column[Convert.ToInt32(detailReportCSV)].ToString());
                    }
                }
            }
            else if (action.IndexOf(".xml") != -1)
            {
                query = StringHelper.GetNextCSV(ref detailReportCSV, ",");
                FCalculator.GetParameters().Add("param_whereSQL", query);

                /* get the parameter names and values */
                while (detailReportCSV.Length > 0)
                {
                    paramName = StringHelper.GetNextCSV(ref detailReportCSV, ",");
                    paramValue = StringHelper.GetNextCSV(ref detailReportCSV, ",");
                    FCalculator.GetParameters().Add(paramName, paramValue);
                }

                /* add the values of the selected column (in this example the first one) */
                SelectedRow = 1;

                for (columnCounter = 0; columnCounter <= FCalculator.GetParameters().Get("MaxDisplayColumns").ToInt32() - 1; columnCounter += 1)
                {
                    FCalculator.GetParameters().Add("param_" +
                        FCalculator.GetParameters().Get("param_calculation",
                            columnCounter).ToString(), ((TResult)FCalculator.GetResults().GetResults()[SelectedRow]).column[columnCounter]);
                }

                /* action is a link to a settings file; it contains e.g. xmlfiles, currentReport, and column settings */
                /* TParameterList.Load adds the new parameters to the existing parameters */
                FCalculator.GetParameters().Load(ASettingsPath + '/' + action);

                if (FCalculator.GenerateResultRemoteClient())
                {
                    FCalculator.GetParameters().Save(ATestPath + "LogParametersAfterCalcDetail.log", true);
                    FCalculator.GetResults().WriteCSV(FCalculator.GetParameters(), ATestPath + "LogResultsDetail.log", "FIND_BEST_SEPARATOR", true);

                    /* test if there is a detail report */
                    if (FCalculator.GetParameters().Exists("param_detail_report_0") == true)
                    {
                        TestDetailReport(ATestPath, ASettingsPath);
                    }
                }
            }
        }

        /// <summary>
        /// common procedure that will load all settings in the given directory, and run a report and
        /// compare the result with results
        /// from previous, using the csv and the txt output
        /// </summary>
        public void TestReport(String ASettingsDirectory)
        {
            String[] fileEntries;
            string fileName;

            if (!Directory.Exists(".." + System.IO.Path.DirectorySeparatorChar + ".." + System.IO.Path.DirectorySeparatorChar + "Reporting" +
                    System.IO.Path.DirectorySeparatorChar + "TestData" + System.IO.Path.DirectorySeparatorChar + ASettingsDirectory))
            {
                TLogging.Log("Test for " + ASettingsDirectory + " does not exist yet!");
                return;
            }

            try
            {
                /* get all xml files in the given directory (assume we are starting it from testing\_bin\debug */
                fileEntries = Directory.GetFiles(PathToTestData + ASettingsDirectory, "*.xml");

                foreach (string s in fileEntries)
                {
                    fileName = s.Substring(0, s.IndexOf(".xml"));
                    System.Console.Write(Path.GetFileName(fileName) + ' ');
                    FCalculator.ResetParameters();
                    FCalculator.GetParameters().Load(fileName + ".xml");

                    if (FCalculator.GenerateResultRemoteClient())
                    {
                        FCalculator.GetResults().WriteBinaryFile(FCalculator.GetParameters(), "report.bin");

                        FCalculator.GetParameters().Save(
                            PathToTestData + ASettingsDirectory + System.IO.Path.DirectorySeparatorChar + "LogParametersAfterCalc.log",
                            true);
                        FCalculator.GetResults().WriteCSV(
                            FCalculator.GetParameters(), PathToTestData + ASettingsDirectory + System.IO.Path.DirectorySeparatorChar +
                            "LogResults.log",
                            "FIND_BEST_SEPARATOR", true);

                        if (!System.IO.File.Exists(fileName + ".csv"))
                        {
                            /* create a new reference file */
                            FCalculator.GetResults().WriteCSV(FCalculator.GetParameters(), fileName + ".csv");
                        }
                        else
                        {
                            FCalculator.GetResults().WriteCSV(FCalculator.GetParameters(), fileName + ".csv.new");
                        }

                        if (!System.IO.File.Exists(fileName + ".txt"))
                        {
                            /* create a new reference file */
                            PrintTxt(FCalculator.GetResults(), FCalculator.GetParameters(), fileName + ".txt");
                        }
                        else
                        {
                            PrintTxt(FCalculator.GetResults(), FCalculator.GetParameters(), fileName + ".txt.new");
                        }

                        if (System.IO.File.Exists(fileName + ".csv.new"))
                        {
                            /* compare the files */
                            Assert.AreEqual(true, TTextFile.SameContent(fileName + ".csv",
                                    fileName + ".csv.new"), "the csv files should be the same: " + fileName);
                            System.IO.File.Delete(fileName + ".csv.new");
                        }

                        if (System.IO.File.Exists(fileName + ".txt.new"))
                        {
                            /* compare the files */
                            /* requires compilation with directive TESTMODE being set, so that the date of the report printout is constant */
                            // TODO: ignore the date, and also ignore the version number
                            // TODO: define sections which should be compared, and which should be ignored. Overwrite with blanks?
                            Assert.AreEqual(true, TTextFile.SameContent(fileName + ".txt",
                                    fileName + ".txt.new"), "the txt files should be the same: " + fileName);
                            System.IO.File.Delete(fileName + ".txt.new");
                        }

                        /* todo: test if something was written to the log file (e.g. parameter not found, etc); */
                        /* make a copy of the file before running the report, and compare with the new version */
                        /* this proves difficult, because it runs against the server */
                        /* once the progress of the report is fed back, we can compare the two output files */
                        /* test if there is a detail report */
                        if (FCalculator.GetParameters().Exists("param_detail_report_0") == true)
                        {
                            TestDetailReport(PathToTestData + ASettingsDirectory + System.IO.Path.DirectorySeparatorChar, PathToSettingsData);
                        }
                    }
                    else
                    {
                        Assert.Fail("Report calculation did not finish, was cancelled on the server");
                    }
                }
            }
            catch (Exception e)
            {
                if (e.GetType() != typeof(NUnit.Framework.AssertionException))
                {
                    System.Console.WriteLine(e.Message);
                    System.Console.WriteLine(e.StackTrace);
                }

                throw;
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestFDDonorsPerRecipient()
        {
            TestReport("FDDonorsPerRecipient");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestPassportExpiry()
        {
            TestReport("Passport Expiry");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestAccountDetail()
        {
            TestReport("Account Detail");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestAccountDetailAnalysisAttr()
        {
            TestReport("AccountDetailAnalysisAttr");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestAPPaymentExport()
        {
            TestReport("APPaymentExport");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestBalSheet()
        {
            TestReport("BalanceSheet");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestBalSheetMultiLedger()
        {
            TestReport("BalSheet MultiLedger");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestBirthdayList()
        {
            TestReport("Birthday List");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestFDIncomeByFund()
        {
            TestReport("FDIncomeByFund");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestGiftBatchExport()
        {
            TestReport("GiftBatchExport");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestIncExpMultiLedger()
        {
            TestReport("IncExp MultiLedger");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestIncExpMultiPeriod()
        {
            TestReport("IncExp MultiPeriod");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestIncExpStatement()
        {
            TestReport("Income Expense Statement");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestSurplusDeficit()
        {
            TestReport("SurplusDeficit");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestTrialBalance()
        {
            TestReport("TrialBalance");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestPartnerFindByEmail()
        {
            TestReport("PartnerFindByEmail");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestGiftExportByMotivation()
        {
            TestReport("GiftExportByMotivation");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestFDIncomeLocalSplit()
        {
            TestReport("FDIncomeLocalSplit");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestTotalGiftsPerDonor()
        {
            TestReport("TotalGiftsPerDonor");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestGiftMethodGiving()
        {
            TestReport("GiftMethodGiving");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestAddressesOfRelationships()
        {
            TestReport("AddressesOfRelationships");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestGiftDataExport()
        {
            TestReport("GiftDataExport");
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestCurrentAccountsPayable()
        {
            TestReport("CurrentAccountsPayable");
        }

        /// <summary>
        /// for testing csv files and printouts
        /// </summary>
        public void RunLocalizedReport(String OutputCSVFile)
        {
            if (!System.IO.File.Exists(OutputCSVFile + ".csv"))
            {
                /* create a new reference file */
                FCalculator.GetResults().WriteCSV(FCalculator.GetParameters(), OutputCSVFile + ".csv");
            }
            else
            {
                FCalculator.GetResults().WriteCSV(FCalculator.GetParameters(), OutputCSVFile + ".csv.new");
            }

            if (!System.IO.File.Exists(OutputCSVFile + ".txt"))
            {
                /* create a new reference file */
                PrintTxt(FCalculator.GetResults(), FCalculator.GetParameters(), OutputCSVFile + ".txt");
            }
            else
            {
                PrintTxt(FCalculator.GetResults(), FCalculator.GetParameters(), OutputCSVFile + ".txt.new");
            }

            if (System.IO.File.Exists(OutputCSVFile + ".csv.new"))
            {
                Assert.AreEqual(true, TTextFile.SameContent(OutputCSVFile + ".csv",
                        OutputCSVFile + ".csv.new"), "the csv files should be the same: " + OutputCSVFile);
                System.IO.File.Delete(OutputCSVFile + ".csv.new");
            }

            if (System.IO.File.Exists(OutputCSVFile + ".txt.new"))
            {
                Assert.AreEqual(true, TTextFile.SameContent(OutputCSVFile + ".txt",
                        OutputCSVFile + ".txt.new"), "the txt files should be the same: " + OutputCSVFile);
                System.IO.File.Delete(OutputCSVFile + ".txt.new");
            }
        }

        /// <summary>
        /// for testing csv files and printouts
        /// </summary>
        /// <param name="Prefix"></param>
        public void ExportLocalizationReports(String Prefix)
        {
            /* test which csv separator is used, and how dates and currency values are formatted */
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB", false);
            FCalculator.GetParameters().Add("param_currency_format", "CurrencyWithoutDecimals");
            RunLocalizedReport(
                PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + Thread.CurrentThread.CurrentCulture.Name + Prefix +
                "WODecimals");
            FCalculator.GetParameters().Add("param_currency_format", "CurrencyThousands");
            FCalculator.GetParameters().Add("ControlSource", new TVariant("Currency: EUR (in Thousands)"), ReportingConsts.HEADERDESCR2);
            RunLocalizedReport(
                PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + Thread.CurrentThread.CurrentCulture.Name + Prefix +
                "Thousands");
            FCalculator.GetParameters().Add("ControlSource", new TVariant("Currency: EUR"), ReportingConsts.HEADERDESCR2);
            FCalculator.GetParameters().RemoveVariable("param_currency_format");
            RunLocalizedReport(
                PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + Thread.CurrentThread.CurrentCulture.Name + Prefix +
                "Standard");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE", false);
            FCalculator.GetParameters().Add("param_currency_format", "CurrencyWithoutDecimals");
            RunLocalizedReport(
                PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + Thread.CurrentThread.CurrentCulture.Name + Prefix +
                "WODecimals");
            FCalculator.GetParameters().Add("param_currency_format", "CurrencyThousands");
            FCalculator.GetParameters().Add("ControlSource", new TVariant("Currency: EUR (in Thousands)"), ReportingConsts.HEADERDESCR2);
            RunLocalizedReport(
                PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + Thread.CurrentThread.CurrentCulture.Name + Prefix +
                "Thousands");
            FCalculator.GetParameters().Add("ControlSource", new TVariant("Currency: EUR"), ReportingConsts.HEADERDESCR2);
            FCalculator.GetParameters().RemoveVariable("param_currency_format");
            RunLocalizedReport(
                PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + Thread.CurrentThread.CurrentCulture.Name + Prefix +
                "Standard");
            Thread.CurrentThread.CurrentCulture = OrigCulture;
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestLocalizationCurrency()
        {
            String XMLFile;

            FCalculator.ResetParameters();
            XMLFile = PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + "TestStandard_Bal.xml";
            FCalculator.GetParameters().Load(XMLFile);

            if (FCalculator.GenerateResultRemoteClient())
            {
                FCalculator.GetParameters().Save(
                    PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + "LogParametersAfterCalc.log",
                    true);
                FCalculator.GetResults().WriteCSV(
                    FCalculator.GetParameters(), PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + "LogResults.log",
                    "FIND_BEST_SEPARATOR", true);
                ExportLocalizationReports("Bal");
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void TestLocalizationDates()
        {
            String XMLFile;

            FCalculator.ResetParameters();
            XMLFile = PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + "TestStandard_Acc.xml";
            FCalculator.GetParameters().Load(XMLFile);

            if (FCalculator.GenerateResultRemoteClient())
            {
                FCalculator.GetParameters().Save(
                    PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + "LogParametersAfterCalc.log",
                    true);
                FCalculator.GetResults().WriteCSV(
                    FCalculator.GetParameters(), PathToTestData + "Localization" + System.IO.Path.DirectorySeparatorChar + "LogResults.log",
                    "FIND_BEST_SEPARATOR", true);
                ExportLocalizationReports("Acc");
            }
        }
    }
}