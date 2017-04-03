//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, ChristianK
//
// Copyright 2004-2015 by OM International
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
using Ict.Common.DB.Exceptions;
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
using Ict.Petra.Server.MSysMan.Common.WebConnectors;
using System.Threading;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Printing;
using Ict.Common.Verification;
using Ict.Common.Session;
using Ict.Petra.Shared.MCommon;
using Ict.Common.Exceptions;
using Npgsql;

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
        private Exception FException = null;
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
            FProgressID = "ReportCalculation" + Guid.NewGuid();
            TProgressTracker.InitProgressTracker(FProgressID, string.Empty, -1.0m);

            // First check whether the 'globally available' DB Connection isn't busy - we must not allow the start of a
            // Report Calculation when that is the case (as this would lead to a nested DB Transaction exception,
            // EDBTransactionBusyException).
            if (DBAccess.GDBAccessObj.Transaction != null)
            {
                FErrorMessage = Catalog.GetString(SharedConstants.NO_PARALLEL_EXECUTION_OF_XML_REPORTS_PREFIX +
                    "The OpenPetra Server is currently too busy to prepare this Report. " +
                    "Please retry once other running tasks (eg. a Report) are finished!");
                TProgressTracker.FinishJob(FProgressID);
                FSuccess = false;

                // Return to the Client immediately!
                return;
            }

            TRptUserFunctionsFinance.FlushSqlCache();

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
            TheThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            TheThread.Name = FProgressID + "_" + UserInfo.GUserInfo.UserID + "__TReportGeneratorUIConnector.Start_Thread";
            TLogging.LogAtLevel(7, TheThread.Name + " starting.");
            TheThread.Start();
        }

        /// <summary>
        /// Signal that the Report calculation should be cancelled.
        /// </summary>
        public void Cancel()
        {
            if (FParameterList != null)
            {
                // This variable will be picked up regularly during generation, in TRptDataCalcLevel.calculate in
                // Ict.Petra.Server.MReporting.Calculation. (It can be null if Cancel gets called before FParameterList is
                // available.)
                FParameterList.Add("CancelReportCalculation", new TVariant(true));
            }
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
                        if (FDatacalculator.GenerateResult(ref FParameterList, ref FResultList, ref FErrorMessage, ref FException))
                        {
                            FSuccess = true;
                            SubmissionOK = true;
                        }
                        else
                        {
                            TLogging.Log(FErrorMessage);
                        }
                    });
            }
            catch (Exception Exc)
            {
                TLogging.Log("Problem calculating report: " + Exc.ToString());
                TLogging.Log(Exc.StackTrace, TLoggingType.ToLogfile);

                FSuccess = false;
                FErrorMessage = Exc.Message;
                FException = Exc;
            }

            if (TDBExceptionHelper.IsTransactionSerialisationException(FException))
            {
                // do nothing - we want this exception to bubble up
            }
            else if (FException is Exception && FException.InnerException is EOPDBException)
            {
                EOPDBException DbExc = (EOPDBException)FException.InnerException;

                if (DbExc.InnerException is Exception)
                {
                    if (DbExc.InnerException is NpgsqlException)
                    {
                        NpgsqlException PgExc = (NpgsqlException)DbExc.InnerException;

                        if (PgExc.Code == "57014") // SQL statement timeout problem
                        {
                            FErrorMessage = Catalog.GetString(
                                "Error - Database took too long to respond. Try different parameters to return fewer results.");
                        }
                    }
                    else
                    {
                        FErrorMessage = DbExc.InnerException.Message;
                    }

                    FException = null;
                }
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
        public String GetErrorMessage(out Exception AException)
        {
            AException = FException;

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

            eOrientation Orientation;

            if (pdfPrinter.Document.DefaultPageSettings.Landscape)
            {
                Orientation = eOrientation.eLandscape;
            }
            else
            {
                Orientation = eOrientation.ePortrait;
            }

            pdfPrinter.Init(Orientation, layout, eMarginType.ePrintableArea);

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
            TSmtpSender EmailSender = null;
            string EmailBody = "";

            AVerification = new TVerificationResultCollection();

            try
            {
                EmailSender = new TSmtpSender();

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

                try
                {
                    EmailSender.SetSender(TUserDefaults.GetStringDefault("SmtpFromAccount"),
                        TUserDefaults.GetStringDefault("SmtpDisplayName"));
                    EmailSender.CcEverythingTo = TUserDefaults.GetStringDefault("SmtpCcTo");
                    EmailSender.ReplyTo = TUserDefaults.GetStringDefault("SmtpReplyTo");
                    EmailBody = TUserDefaults.GetStringDefault("SmtpEmailBody");
                }
                catch (ESmtpSenderInitializeException e)
                {
                    AVerification.Add(new TVerificationResult(
                            Catalog.GetString("Sending Email"),
                            String.Format("{0}\n{1}", e.Message, Catalog.GetString("Check the Email tab in User Settings >> Preferences.")),
                            CommonErrorCodes.ERR_MISSINGEMAILCONFIGURATION,
                            TResultSeverity.Resv_Critical,
                            new System.Guid()));

                    if (e.InnerException != null)
                    {
                        TLogging.Log("Email XML Report: " + e.InnerException);
                    }

                    return false;
                }

                if (EmailBody == "")
                {
                    EmailBody = Catalog.GetString("OpenPetra report attached.");
                }

                if (EmailSender.SendEmail(
                        AEmailAddresses,
                        FParameterList.Get("currentReport").ToString(),
                        EmailBody,
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
                        "Server problems",
                        TResultSeverity.Resv_Critical,
                        new System.Guid()));

                return false;
            } // try
            catch (ESmtpSenderInitializeException e)
            {
                AVerification.Add(new TVerificationResult(
                        Catalog.GetString("Sending Email"),
                        e.Message,
                        CommonErrorCodes.ERR_MISSINGEMAILCONFIGURATION,
                        TResultSeverity.Resv_Critical,
                        new System.Guid()));

                if (e.InnerException != null)
                {
                    TLogging.Log("Email XML Report: " + e.InnerException);
                }

                return false;
            }
            finally
            {
                if (EmailSender != null)
                {
                    EmailSender.Dispose();
                }
            }
        }
    }
}
