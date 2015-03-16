//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.Xml;
using System.Drawing.Printing;
using System.Collections.Generic;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MReporting;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MReporting;
using Ict.Petra.Server.MReporting.Calculator;
using Ict.Petra.Server.MReporting.MFinance;
using System.Threading;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Printing;
using Ict.Common.Verification;
using Ict.Common.Session;
using Ict.Petra.Shared.MCommon;

namespace Ict.Petra.Server.MReporting.UIConnectors
{
    /// <summary>
    /// the connector for the report generation
    /// </summary>
    public class TReportGeneratorUIConnector : IReportingUIConnectorsReportGenerator
    {
        private TRptDataCalculator FDatacalculator;
        private TResultList FResultList;
        private TParameterList FParameterList;
        private String FErrorMessage = string.Empty;
        private Boolean FSuccess;
        private String FProgressID;

        /// constructor needed for the interface
        public TReportGeneratorUIConnector()
        {
        }

        /// <summary>
        /// to show the progress of the report calculation;
        /// prints the current id of the row that is being calculated;
        /// </summary>
        public TProgressState Progress
        {
            get
            {
                return TProgressTracker.GetCurrentState(FProgressID);
            }
        }

        /// <summary>
        /// Calculates the report, which is specified in the parameters table
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Start(System.Data.DataTable AParameters)
        {
            TRptUserFunctionsFinance.FlushSqlCache();
            FProgressID = "ReportCalculation" + Guid.NewGuid();
            TProgressTracker.InitProgressTracker(FProgressID, string.Empty, -1.0m);
            FParameterList = new TParameterList();
            FParameterList.LoadFromDataTable(AParameters);
            FSuccess = false;
            String PathStandardReports = TAppSettingsManager.GetValue("Reporting.PathStandardReports");
            String PathCustomReports = TAppSettingsManager.GetValue("Reporting.PathCustomReports");
            FDatacalculator = new TRptDataCalculator(DBAccess.GDBAccessObj, PathStandardReports, PathCustomReports);

            // setup the logging to go to the TProgressTracker
            TLogging.SetStatusBarProcedure(new TLogging.TStatusCallbackProcedure(WriteToStatusBar));
            string session = TSession.GetSessionID();
            ThreadStart myThreadStart = delegate {
                Run(session);
            };
            Thread TheThread = new Thread(myThreadStart);
            TheThread.Name = FProgressID;
            TheThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            TheThread.Start();
        }

        /// <summary>
        /// cancel the report calculation
        /// </summary>
        public void Cancel()
        {
            // This variable will be picked up regularly during generation, in TRptDataCalcLevel.calculate in Ict.Petra.Server.MReporting.Calculation
            FParameterList.Add("CancelReportCalculation", new TVariant(true));
        }

        /// <summary>
        /// run the report
        /// </summary>
        private void Run(string ASessionID)
        {
            // need to initialize the database session
            TSession.InitThread(ASessionID);
            IsolationLevel Level;
            if (FParameterList.Get("IsolationLevel").ToString().ToLower() == "readuncommitted")
            {
                // for long reports, that should not take out locks;
                // the data does not need to be consistent or will most likely not be changed during the generation of the report
                Level = IsolationLevel.ReadUncommitted;
            }
            else if (FParameterList.Get("IsolationLevel").ToString().ToLower() == "repeatableread")
            {
                // for financial reports: it is important to have consistent data; e.g. for totals
                Level = IsolationLevel.RepeatableRead;
            }
            else if (FParameterList.Get("IsolationLevel").ToString().ToLower() == "serializable")
            {
                // for creating extracts: we need to write to the database
                Level = IsolationLevel.Serializable;
            }
            else
            {
                // default behaviour for normal reports
                Level = IsolationLevel.ReadCommitted;
            }

            FSuccess = false;

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            try
            {
                DBAccess.GDBAccessObj.BeginAutoTransaction(Level, ref Transaction,
                    ref SubmissionOK,
                    delegate
                    {
                        if (FDatacalculator.GenerateResult(ref FParameterList, ref FResultList, ref FErrorMessage))
                        {
                            FSuccess = true;
                        }
                        else
                        {
                            TLogging.Log(FErrorMessage);
                        }
                    });
            }
            catch (Exception e)
            {
                TLogging.Log("problem calculating report: " + e.Message);
                TLogging.Log(e.StackTrace, TLoggingType.ToLogfile);
            }

            TProgressTracker.FinishJob(FProgressID);
        }

        /// <summary>
        /// get the result of the report calculation
        /// </summary>
        public DataTable GetResult()
        {
            return FResultList.ToDataTable(FParameterList);
        }

        /// <summary>
        /// get the environment variables after report calculation
        /// </summary>
        public DataTable GetParameter()
        {
            return FParameterList.ToDataTable();
        }

        /// <summary>
        /// see if the report calculation finished successfully
        /// </summary>
        public Boolean GetSuccess()
        {
            return FSuccess;
        }

        /// <summary>
        /// error message that happened during report calculation
        /// </summary>
        public String GetErrorMessage()
        {
            return FErrorMessage;
        }

        /// <summary>
        /// for displaying the progress
        /// </summary>
        /// <returns>void</returns>
        private void WriteToStatusBar(String s)
        {
            TProgressTracker.SetCurrentState(FProgressID, s, -1.0m);
        }

        private bool ExportToExcelFile(string AFilename)
        {
            bool ExportOnlyLowestLevel = false;

            // Add the parameter export_only_lowest_level to the Parameters if you don't want to export the
            // higher levels. In some reports (Supporting Churches Report or Partner Contact Report) the csv
            // output looks much nicer if it doesn't contain the unnecessary higher levels.
            if (FParameterList.Exists("csv_export_only_lowest_level"))
            {
                ExportOnlyLowestLevel = FParameterList.Get("csv_export_only_lowest_level").ToBool();
            }

            XmlDocument doc = FResultList.WriteXmlDocument(FParameterList, ExportOnlyLowestLevel);

            if (doc != null)
            {
                using (FileStream fs = new FileStream(AFilename, FileMode.Create))
                {
                    if (TCsv2Xml.Xml2ExcelStream(doc, fs, false))
                    {
                        fs.Close();
                    }
                }

                return true;
            }

            return false;
        }

        private bool PrintToPDF(string AFilename, bool AWrapColumn)
        {
            PrintDocument doc = new PrintDocument();

            TPdfPrinter pdfPrinter = new TPdfPrinter(doc, TGfxPrinter.ePrinterBehaviour.eReport);
            TReportPrinterLayout layout = new TReportPrinterLayout(FResultList, FParameterList, pdfPrinter, AWrapColumn);

            pdfPrinter.Init(eOrientation.ePortrait, layout, eMarginType.ePrintableArea);

            pdfPrinter.SavePDF(AFilename);

            return true;
        }

        private bool ExportToCSVFile(string AFilename)
        {
            bool ExportOnlyLowestLevel = false;

            // Add the parameter export_only_lowest_level to the Parameters if you don't want to export the
            // higher levels. In some reports (Supporting Churches Report or Partner Contact Report) the csv
            // output looks much nicer if it doesn't contain the unnecessary higher levels.
            if (FParameterList.Exists("csv_export_only_lowest_level"))
            {
                ExportOnlyLowestLevel = FParameterList.Get("csv_export_only_lowest_level").ToBool();
            }

            return FResultList.WriteCSV(FParameterList, AFilename, ExportOnlyLowestLevel);
        }

        /// <summary>
        /// send report as email
        /// </summary>
        public Boolean SendEmail(string AEmailAddresses,
            bool AAttachExcelFile,
            bool AAttachCSVFile,
            bool AAttachPDF,
            bool AWrapColumn,
            out TVerificationResultCollection AVerification)
        {
            TSmtpSender EmailSender = new TSmtpSender();

            AVerification = new TVerificationResultCollection();

            if (!EmailSender.ValidateEmailConfiguration())
            {
                AVerification.Add(new TVerificationResult(
                        Catalog.GetString("Sending Email"),
                        Catalog.GetString("Missing configuration for sending emails. Please edit your server configuration file"),
                        CommonErrorCodes.ERR_MISSINGEMAILCONFIGURATION,
                        TResultSeverity.Resv_Critical,
                        new System.Guid()));
                return false;
            }

            List <string>FilesToAttach = new List <string>();

            if (AAttachExcelFile)
            {
                string ExcelFile = TFileHelper.GetTempFileName(
                    FParameterList.Get("currentReport").ToString(),
                    ".xlsx");

                if (ExportToExcelFile(ExcelFile))
                {
                    FilesToAttach.Add(ExcelFile);
                }
            }

            if (AAttachCSVFile)
            {
                string CSVFile = TFileHelper.GetTempFileName(
                    FParameterList.Get("currentReport").ToString(),
                    ".csv");

                if (ExportToCSVFile(CSVFile))
                {
                    FilesToAttach.Add(CSVFile);
                }
            }

            if (AAttachPDF)
            {
                string PDFFile = TFileHelper.GetTempFileName(
                    FParameterList.Get("currentReport").ToString(),
                    ".pdf");

                if (PrintToPDF(PDFFile, AWrapColumn))
                {
                    FilesToAttach.Add(PDFFile);
                }
            }

            if (FilesToAttach.Count == 0)
            {
                AVerification.Add(new TVerificationResult(
                        Catalog.GetString("Sending Email"),
                        Catalog.GetString("Missing any attachments, not sending the email"),
                        "Missing Attachments",
                        TResultSeverity.Resv_Critical,
                        new System.Guid()));
                return false;
            }

            // TODO use the email address of the user, from s_user
            if (EmailSender.SendEmail("<" + TAppSettingsManager.GetValue("Reports.Email.Sender") + ">",
                    "OpenPetra Reports",
                    AEmailAddresses,
                    FParameterList.Get("currentReport").ToString(),
                    Catalog.GetString("Please see attachment!"),
                    FilesToAttach.ToArray()))
            {
                foreach (string file in FilesToAttach)
                {
                    File.Delete(file);
                }

                return true;
            }

            AVerification.Add(new TVerificationResult(
                    Catalog.GetString("Sending Email"),
                    Catalog.GetString("Problem sending email"),
                    "server problems",
                    TResultSeverity.Resv_Critical,
                    new System.Guid()));

            return false;
        }
    }
}
