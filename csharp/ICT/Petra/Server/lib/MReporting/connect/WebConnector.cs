//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, ChristianK
//
// Copyright 2004-2019 by OM International
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
using Ict.Common.Data;
using Ict.Common.DB.Exceptions;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MReporting;
using Ict.Petra.Server.MReporting.Calculator;
using Ict.Petra.Server.MReporting.MFinance;
using Ict.Petra.Server.MSysMan.Common.WebConnectors;
using Ict.Petra.Server.MSysMan.Data.Access;
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
using OfficeOpenXml;
using HtmlAgilityPack;

namespace Ict.Petra.Server.MReporting.WebConnectors
{
    /// <summary>
    /// the connector for the report generation
    /// </summary>
    public static class TReportGeneratorWebConnector
    {
        /// <summary>
        /// to show the progress of the report calculation;
        /// prints the current id of the row that is being calculated;
        /// </summary>
        [RequireModulePermission("NONE")]
        public static TProgressState GetProgress(string AReportID)
        {
            return TProgressTracker.GetCurrentState(AReportID);
        }

        /// <summary>
        /// prepare a UID for the report
        /// </summary>
        /// <returns>ReportClientUID</returns>
        [RequireModulePermission("NONE")]
        public static string Create()
        {
            string session = TSession.GetSessionID();

            string ReportID = "ReportCalculation" + session + Guid.NewGuid();
            TProgressTracker.InitProgressTracker(ReportID, string.Empty, -1.0m);

            return ReportID;
        }

        /// <summary>
        /// Calculates the report, which is specified in the parameters table
        /// </summary>
        /// <returns>ReportClientUID</returns>
        [RequireModulePermission("NONE")]
        public static string Start(string AReportID, System.Data.DataTable AParameters)
        {
            string session = TSession.GetSessionID();

            TParameterList ParameterList = new TParameterList();
            ParameterList.LoadFromDataTable(AParameters);

            String PathStandardReports = TAppSettingsManager.GetValue("Reporting.PathStandardReports");
            String PathCustomReports = TAppSettingsManager.GetValue("Reporting.PathCustomReports");

            TRptDataCalculator Datacalculator = new TRptDataCalculator(PathStandardReports, PathCustomReports);

            ThreadStart myThreadStart = delegate {
                Run(session, AReportID, Datacalculator, ParameterList);
            };
            Thread TheThread = new Thread(myThreadStart);
            TheThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            TheThread.Name = AReportID + "_" + UserInfo.GetUserInfo().UserID + "__TReportGeneratorUIConnector.Start_Thread";
            TLogging.LogAtLevel(7, TheThread.Name + " starting.");
            TheThread.Start();

            return AReportID;
        }

        /// <summary>
        /// run the report
        /// </summary>
        private static void Run(string ASessionID, string AReportID, TRptDataCalculator ADatacalculator, TParameterList AParameterList)
        {
            // need to initialize the database session
            TSession.InitThread(ASessionID);

            TDataBase db = DBAccess.Connect("TReportGeneratorWebConnector");
            
            TDBTransaction Transaction = new TDBTransaction();
            bool Success = false;
            bool Submit = true;
            string HTMLOutput = String.Empty;
            HtmlDocument HTMLDocument = new HtmlDocument();
            string ErrorMessage = String.Empty;

            try
            {
                db.ReadTransaction(ref Transaction,
                    delegate
                    {
                        Exception myException = null;
                        if (ADatacalculator.GenerateResult(ref AParameterList, ref HTMLOutput, out HTMLDocument, ref ErrorMessage, ref myException, Transaction))
                        {
                            Success = true;
                        }
                        else
                        {
                            TLogging.Log(ErrorMessage);
                        }
                    });
            }
            catch (Exception Exc)
            {
                TLogging.Log("Problem calculating report: " + Exc.ToString());
                TLogging.Log(Exc.StackTrace, TLoggingType.ToLogfile);

                Success = false;
                ErrorMessage = Exc.Message;
            }

/*
            if (TDBExceptionHelper.IsTransactionSerialisationException(FException))
            {
                // do nothing - we want this exception to bubble up
            }
            else if (FException is Exception && FException.InnerException is EOPDBException)
            {
                EOPDBException DbExc = (EOPDBException)FException.InnerException;

                if (DbExc.InnerException is Exception)
                {
                    if (DbExc.InnerException is PostgresException)
                    {
                        PostgresException PgExc = (PostgresException)DbExc.InnerException;

                        if (PgExc.SqlState == "57014") // SQL statement timeout problem
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
*/

            try
            {
                // store the report result
                db.WriteTransaction(ref Transaction,
                    ref Submit,
                    delegate
                    {
                        // delete report results that are expired.
                        string sql = "DELETE FROM PUB_s_report_result WHERE s_valid_until_d < NOW()";
                        db.ExecuteNonQuery(sql, Transaction);
                        
                        // TODO: only keep maximum of 10 report results per user (s_created_by_c)

                        // store success, store parameter list, store html document
                        SReportResultTable table = new SReportResultTable();
                        SReportResultRow row = table.NewRowTyped();
                        row.ReportId = AReportID;
                        row.SessionId = TSession.GetSessionID();
                        row.ValidUntil = DateTime.Now.AddHours(12);
                        row.ParameterList = AParameterList.ToJson();
                        row.ResultHtml = HTMLOutput;
                        row.Success = Success;
                        row.ErrorMessage = ErrorMessage;
                        table.Rows.Add(row);
                        SReportResultAccess.SubmitChanges(table, Transaction);
                        Submit = true;
                    });
            }
            catch (Exception Exc)
            {
                TLogging.Log("Problem storing report result: " + Exc.ToString());
                TLogging.Log(Exc.StackTrace, TLoggingType.ToLogfile);

                Success = false;
                ErrorMessage = Exc.Message;
            }

            db.CloseDBConnection();

            TProgressTracker.FinishJob(AReportID);
        }

        private static SReportResultRow GetReportResult(string AReportID, TDataBase ADataBase)
        {
            SReportResultRow Row = null;
            TDBTransaction t = new TDBTransaction();
            TDataBase db = DBAccess.Connect("GetReportResult", ADataBase);

            db.ReadTransaction(ref t,
                delegate
                {
                    SReportResultTable resultTable = SReportResultAccess.LoadByPrimaryKey(AReportID, t);

                    if (resultTable.Rows.Count == 1)
                    {
                        Row = resultTable[0];
                    }
                });

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }

            return Row;
        }

        /// <summary>
        /// get the environment variables after report calculation
        /// </summary>
        [NoRemoting]
        public static TParameterList GetParameter(string AReportID, TDataBase ADataBase)
        {
            SReportResultRow Row = GetReportResult(AReportID, ADataBase);
            TParameterList ParameterList = new TParameterList();

            if (Row != null)
            {
                ParameterList.LoadFromJson(Row.ParameterList);
            }

            return ParameterList;
        }

        /// <summary>
        /// see if the report calculation finished successfully
        /// </summary>
        [RequireModulePermission("NONE")]
        public static Boolean GetSuccess(string AReportID, TDataBase ADataBase = null)
        {
            SReportResultRow Row = GetReportResult(AReportID, ADataBase);

            if (Row != null)
            {
                return Row.Success;
            }

            return false;
        }

        /// <summary>
        /// error message that happened during report calculation
        /// </summary>
        [RequireModulePermission("NONE")]
        public static String GetErrorMessage(string AReportID, TDataBase ADataBase = null)
        {
            SReportResultRow Row = GetReportResult(AReportID, ADataBase);

            if (Row != null)
            {
                return Row.ErrorMessage;
            }

            return String.Empty;
        }

        private static bool ExportToExcelFile(string AFilename, HtmlDocument AHTMLDocument)
        {
            // transform the HTML output to xlsx file
            ExcelPackage ExcelDoc = HTMLTemplateProcessor.HTMLToCalc(AHTMLDocument);

            if (ExcelDoc != null)
            {
                using (FileStream fs = new FileStream(AFilename, FileMode.Create))
                {
                    ExcelDoc.SaveAs(fs);
                    fs.Close();
                }

                return true;
            }

            return false;
        }

        private static bool PrintToPDF(string AFilename, HtmlDocument AHTMLDocument)
        {
            // transform the HTML output to pdf file
            HTMLTemplateProcessor.HTMLToPDF(AHTMLDocument, AFilename);

            return true;
        }

        /// Download the result of the report as HTML
        [RequireModulePermission("NONE")]
        public static string DownloadHTML(string AReportID, TDataBase ADataBase = null)
        {
            SReportResultRow Row = GetReportResult(AReportID, ADataBase);

            if (Row != null)
            {
                return Row.ResultHtml;
            }
            
            return String.Empty;
        }

        /// Download the result of the report as PDF File in base64 encoding
        [RequireModulePermission("NONE")]
        public static string DownloadPDF(string AReportID, TDataBase ADataBase = null)
        {
            SReportResultRow Row = GetReportResult(AReportID, ADataBase);

            if (Row != null)
            {
                HtmlDocument HTMLDocument = new HtmlDocument();
                HTMLDocument.LoadHtml(Row.ResultHtml);

                string PDFFile = TFileHelper.GetTempFileName(
                    AReportID,
                    ".pdf");

                if (PrintToPDF(PDFFile, HTMLDocument))
                {
                    byte[] data = System.IO.File.ReadAllBytes(PDFFile);
                    string result = Convert.ToBase64String(data);
                    System.IO.File.Delete(PDFFile);
                    return result;
                }
            }

            return String.Empty;
        }

        /// Download the result of the report as Excel File in base64 encoding
        [RequireModulePermission("NONE")]
        public static string DownloadExcel(string AReportID, TDataBase ADataBase = null)
        {
            SReportResultRow Row = GetReportResult(AReportID, ADataBase);

            if (Row != null)
            {
                HtmlDocument HTMLDocument = new HtmlDocument();
                HTMLDocument.LoadHtml(Row.ResultHtml);

                string ExcelFile = TFileHelper.GetTempFileName(
                    AReportID,
                    ".xls");

                if (ExportToExcelFile(ExcelFile, HTMLDocument))
                {
                    byte[] data = System.IO.File.ReadAllBytes(ExcelFile);
                    string result = Convert.ToBase64String(data);
                    System.IO.File.Delete(ExcelFile);
                    return result;
                }
            }

            return String.Empty;
        }

        /// <summary>
        /// send report as email
        /// </summary>
        [RequireModulePermission("NONE")]
        public static Boolean SendEmail(string AReportID, string AEmailAddresses,
            bool AAttachExcelFile,
            bool AAttachPDF,
            out TVerificationResultCollection AVerification)
        {
            TSmtpSender EmailSender = null;
            string EmailBody = "";

            SReportResultRow Row = GetReportResult(AReportID, null);
            TParameterList ParameterList = new TParameterList();
            AVerification = new TVerificationResultCollection();

            if (Row == null)
            {
                return false;
            }
            
            ParameterList.LoadFromJson(Row.ParameterList);
            HtmlDocument HTMLDocument = new HtmlDocument();
            HTMLDocument.LoadHtml(Row.ResultHtml);

            try
            {
                EmailSender = new TSmtpSender();

                List <string>FilesToAttach = new List <string>();

                if (AAttachExcelFile)
                {
                    string ExcelFile = TFileHelper.GetTempFileName(
                        ParameterList.Get("currentReport").ToString(),
                        ".xlsx");

                    if (ExportToExcelFile(ExcelFile, HTMLDocument))
                    {
                        FilesToAttach.Add(ExcelFile);
                    }
                }

                if (AAttachPDF)
                {
                    string PDFFile = TFileHelper.GetTempFileName(
                        ParameterList.Get("currentReport").ToString(),
                        ".pdf");

                    if (PrintToPDF(PDFFile, HTMLDocument))
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
                        ParameterList.Get("currentReport").ToString(),
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
