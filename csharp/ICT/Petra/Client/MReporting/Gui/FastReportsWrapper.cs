//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//     Tim Ingham
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

//using FastReport;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.IO;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.MSysMan.Gui;

using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using System.Text;
using System.Security;
using System.Net.Mail;
using Ict.Petra.Client.MFastReport;
using Ict.Petra.Client.MFastReport.Gui;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// If the FastReports DLL can be loaded, this object insinuates FastReports into the GUI via the PetraUtilsObject,
    /// otherwise it does nothing.
    /// </summary>
    public class FastReportsWrapper
    {
        TFrmPetraReportingUtils FPetraUtilsObject;
        /// <summary>
        /// Delegate for getting data from the server and into the report
        /// </summary>
        /// <param name="ACalc"></param>
        /// <returns>true if the data is OK. (If it's not OK, the method should have told the user why not!)</returns>
        public delegate bool TDataGetter (TRptCalculator ACalc);
        private TDataGetter FDataGetter;
        String FExtractPartnerKeyName = "";
        private DataTable FClientDataTable = null;
        private Assembly FastReportsDll;
        private object FfastReportInstance;
        /// <summary>Specified with constructor, this can be accessed afterwards.</summary>
        public String FReportName;

        Type FFastReportType;
        /// <summary>
        /// Use this to check whether loading the FastReports DLL worked.
        /// </summary>
        public Boolean LoadedOK;

        private SReportTemplateRow FSelectedTemplate = null;

        private enum TInitState
        {
            Unknown, LoadTemplate, InitSystem, LoadedOK
        };
        private TInitState FInitState;

        /// <summary>
        /// Use This template for the report
        /// </summary>
        /// <param name="ATemplate"></param>
        public void SetTemplate(SReportTemplateRow ATemplate)
        {
            FSelectedTemplate = ATemplate;

            if (FPetraUtilsObject != null)
            {
                FPetraUtilsObject.SetWindowTitle();
            }
        }

        /// <summary>The Id of the currently selected FastReport template (or 0)</summary>
        public Int32 SelectedTemplateId
        {
            get
            {
                return (FSelectedTemplate == null) ?
                       0
                       :
                       FSelectedTemplate.TemplateId;
            }
        }

        /// <summary>The Template Name will be written to the UI title bar</summary>
        public string SelectedTemplateName
        {
            get
            {
                if (!LoadedOK || (FSelectedTemplate == null))
                {
                    return "";
                }

                return String.Format(" [{0}]", FSelectedTemplate.ReportVariant);
            }
        }

        /// <summary>
        /// FastReports uses this function to prepare the Dataset.
        /// The DataGetter function will be called for "GenerateReport" or "DesignReport".
        /// It should make calls back to RegisterData, below.
        /// </summary>
        /// <param name="DataGetter"></param>
        public void SetDataGetter(TDataGetter DataGetter)
        {
            if (LoadedOK)
            {
                FDataGetter = DataGetter;
            }
            else
            {
                ShowErrorPopup();
            }
        }

        private Boolean LoadDefaultTemplate()
        {
            FInitState = TInitState.LoadTemplate;
            SReportTemplateTable TemplateTable = TRemote.MReporting.WebConnectors.GetTemplateVariants(FReportName,
                UserInfo.GUserInfo.UserID,
                true);

            if (TemplateTable.Rows.Count == 0)
            {
//              TLogging.Log("No FastReports template for " + FReportName);
                return false;
            }

            SetTemplate(TemplateTable[0]);
            FInitState = TInitState.LoadedOK;
            return true;
        }

        /// <summary>
        /// Instance this object and it changes the behaviour of the ReportForm UI to use FastReports if the DLL is installed.
        /// </summary>
        /// <param name="PetraUtilsObject"></param>
        public FastReportsWrapper(TFrmPetraReportingUtils PetraUtilsObject)
        {
            try
            {
                LoadedOK = false;
                FDataGetter = null;
                FPetraUtilsObject = PetraUtilsObject;

                // we do not support FastReports in the Open Source fork of OpenPetra
                return;
            }
            catch (Exception e)
            {
                TLogging.Log("FastReports Wrapper (" + FReportName + ") Not loaded: " + e.Message);
            }
        }

        /// <summary>Call with true to include the facility to generate an extract</summary>
        /// <param name="ACanDoExtract"></param>
        /// <param name="APartnerKeyName"></param>
        public void AllowExtractGeneration(Boolean ACanDoExtract, String APartnerKeyName = "")
        {
            FPetraUtilsObject.DelegateGenerateExtract = ACanDoExtract ? GenerateExtract
                                                        : (TFrmPetraReportingUtils.TDelegateGenerateReportOverride) null;
            FExtractPartnerKeyName = APartnerKeyName;
        }

        /// <summary>
        /// Constructor used when there's no Reporting UI
        /// </summary>
        /// <param name="AReportName"></param>
        public FastReportsWrapper(String AReportName)
        {
            try
            {
                LoadedOK = false;
                FDataGetter = null;

                // we do not support FastReports in the Open Source fork of OpenPetra
                return;
            }
            catch (Exception e)
            {
                TLogging.Log("FastReports Wrapper (" + FReportName + ") Not loaded: " + e.Message);
            }
        }

        /// <summary>
        /// If the wrapper didn't initialise, the caller can call this.
        /// </summary>
        public void ShowErrorPopup()
        {
            // in OpenPetra OpenSource, we do not use the FastReport DLLs
            return;

            // Note from AlanP: This method will show an appropriate Fast Reports error message box depending on the InitState
            // We want to show this error if FastReports is supposed to be used but is not installed.
            // We do not want to show it if FastReports is NOT supposed to be used....
            // If FastReports is not supposed to be used the database will have an entry: USEXMLREPORTS (in system defaults table)
            // However the only quirk to this simple arrangement is that in OM we do want to use FastReports - but it does not HAVE to be installed
            //   in many circumstances.  It may well not be installed on our continuous integration server for example.
            // When the CI server runs the test suite, one of the tests is to open all the main screens from the main menu.
            // It does this on a 'new', 'blank' database which will not have 'USEXMLREPORTS' in the defaults table.
            // As a result the test fails because an unexpected modal dialog appears that is not dealt with
            // So we would like to know if the method is being run in the context of a test or not.
            // We achieve this by the first line of code in the method.
            // If FPetraUtilsObject is null, this wrapper has been created using the constructor that specifies the ReportName.
            //   This is never used (at present anyway) in the context of a test.
            // If FPetraUtilsObject is non-null, we can always (at present) detect the difference between a screen opened from the main menu and a screen opened for a test
            //   because, in the latter case, the CallerForm will be null.
            // If, in the future, this 'workaround' can no longer be used (because we start to use a FastReports test?), we will have to do things differently.

            Boolean IsTestContext = (FPetraUtilsObject != null) && (FPetraUtilsObject.GetCallerForm() == null);

            if ((TSystemDefaults.GetStringDefault("USEXMLREPORTS", "Not Specified") == "Not Specified") && !IsTestContext)
            {
                String Msg = "";

                switch (FInitState)
                {
                    case TInitState.LoadTemplate:
                    {
                        Msg = String.Format("no reporting template found for {0}.", FReportName);
                        break;
                    }

                    case TInitState.InitSystem:
                    {
                        Msg = "the DLL failed to initialise.";
                        break;
                    }

                    default:
                    {
                        return;     // Anything else is not an error...
                    }
                }

                MessageBox.Show("The FastReports subsystem did not initialise:\r\n" +
                    Msg +
                    "\r\n(To suppress this message, set USEXMLREPORTS in SystemDefaults.)",
                    "Reporting engine");
            }
        }

        //
        // Called on Cancel button:
        private void CancelReportGeneration(TRptCalculator ACalc)
        {
            TRemote.MReporting.WebConnectors.CancelDataTableGeneration();

            if (FPetraUtilsObject != null)
            {
                FPetraUtilsObject.AbortStatusThread();
            }
        }

        /// <summary>
        /// Call the FastReport method of the same name
        /// This should only be called from the "DataGetter" method. It can only succeed if FastReports initialised correctly.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        public void RegisterData(DataTable data, string name)
        {
            FFastReportType.GetMethod("RegisterData", new Type[] { data.GetType(), name.GetType() }).Invoke(FfastReportInstance,
                new object[] { data, name });
            FClientDataTable = data;
        }

        private void LoadReportParams(TRptCalculator ACalc)
        {
            // Add standard parameters for the report header
            ACalc.GetParameters().Add("param_requested_by", UserInfo.GUserInfo.UserID);
            Version ClientVersion = Assembly.GetAssembly(typeof(FastReportsWrapper)).GetName().Version;
            ACalc.GetParameters().Add("param_version", ClientVersion.Major.ToString() + "." +
                ClientVersion.Minor.ToString() + "." +
                ClientVersion.Build.ToString() + "." +
                ClientVersion.Revision.ToString());
            //
            // Some params are always provided for reports:
            bool TaxDeductiblePercentageEnabled =
                TSystemDefaults.GetBooleanDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, false);

            ACalc.AddParameter("param_tax_deductible_pct", TaxDeductiblePercentageEnabled);


            ArrayList reportParam = ACalc.GetParameters().Elems;
            MethodInfo FastReport_SetParameterValue = FFastReportType.GetMethod("SetParameterValue");

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation"))
                {
                    FastReport_SetParameterValue.Invoke(FfastReportInstance, new object[] { p.name, p.value.ToObject() });
                }
            }
        }

        /// <summary>
        /// Called from a delegate set up by my constructor.
        /// Or if you're not using a reporting UI, you can call this directly, once the data and params have been set up.
        /// </summary>
        /// <param name="ACalc"></param>
        public void DesignReport(TRptCalculator ACalc)
        {
            ACalc.GetParameters().Add("param_design_template", true);

            if (FSelectedTemplate != null)
            {
                if (FDataGetter != null)
                {
                    if (!FDataGetter(ACalc))
                    {
                        return;
                    }
                }

                ACalc.GetParameters().Add("param_design_template_id", FSelectedTemplate.TemplateId);
                FFastReportType.GetMethod("LoadFromString", new Type[] { FSelectedTemplate.XmlText.GetType() }).Invoke(FfastReportInstance,
                    new object[] { FSelectedTemplate.XmlText });

                LoadReportParams(ACalc);

                // The FastReportsWrapper constructor has called IctConfig.InitIctConfig(), connecting the Save methods of the Designer to
                // our own Ict.Petra.Client.MFastReport.DesignerSettings_CustomSaveReport() handler to save the report design to the
                // OpenPetra database and Backup_*.sql file from within Designer itself.
                FfastReportInstance.Report.FileName = FReportName;
                FFastReportType.GetMethod("Design", new Type[0]).Invoke(FfastReportInstance, null);
 
               // Re-fetch the template id from the report, in case a new template was created, and make this the current template.
                var Tbl = TRemote.MReporting.WebConnectors.GetTemplateById((int)FfastReportInstance.GetParameterValue("param_design_template_id"));
                SetTemplate(Tbl[0]);
            }

            if (FPetraUtilsObject != null)
            {
                FPetraUtilsObject.UpdateParentFormEndOfReport();
            }
        }

        /// <summary>
        /// Must be specified for ExportToStream
        /// </summary>
        public enum ReportExportType
        {
            /// <summary>
            /// </summary>
            Pdf,
            /// <summary>
            /// Allows text formatting but not external assets
            /// </summary>
            Html,
            /// <summary>
            /// Plain text
            /// </summary>
            Text
        };

        /// <summary>
        /// The report will be generated, but not shown to the user.
        /// </summary>
        /// <param name="ACalc"></param>
        /// <param name="Format"></param>
        public MemoryStream ExportToStream(TRptCalculator ACalc, ReportExportType Format)
        {
            MemoryStream HtmlStream = new MemoryStream();
            object exporter;
            Type ExporterType;

            if (Format == ReportExportType.Pdf)
            {
                exporter = FastReportsDll.CreateInstance("FastReport.Export.Pdf.PDFExport");
                ExporterType = exporter.GetType();
                ExporterType.GetProperty("EmbeddingFonts").SetValue(exporter, false, null);
            }
            else // otherwise do HTML - text is not yet supported.
            {
                exporter = FastReportsDll.CreateInstance("FastReport.Export.Html.HTMLExport");
                ExporterType = exporter.GetType();
            }

            FFastReportType.GetMethod("LoadFromString", new Type[] { FSelectedTemplate.XmlText.GetType() }).Invoke(FfastReportInstance,
                new object[] { FSelectedTemplate.XmlText });
            LoadReportParams(ACalc);
            FFastReportType.GetMethod("Prepare", new Type[0]).Invoke(FfastReportInstance, null);
            FFastReportType.GetMethod("Export", new Type[] { ExporterType, HtmlStream.GetType() }).Invoke(FfastReportInstance,
                new Object[] { exporter, HtmlStream });
            return HtmlStream;
        }

        private String SelectColumnNameForExract(DataTable ATbl, String ADefaultField)
        {
            String Res = "";

            TFrmSelectExtractColumn SelectForm = new TFrmSelectExtractColumn();
            Boolean FoundInt64Field = false;

            if (ATbl.Rows.Count < 1)
            {
                return Res;
            }

            foreach (DataColumn Col in ATbl.Columns)
            {
                if (Col.DataType == typeof(Int64))
                {
                    FoundInt64Field = true;
                    SelectForm.AddOption(Col.ColumnName);
                }
            }

            SelectForm.SelectedOption = ADefaultField;

            if (FoundInt64Field && (SelectForm.ShowDialog() == System.Windows.Forms.DialogResult.OK))
            {
                Res = SelectForm.SelectedOption;
            }

            return Res;
        }

        /// <summary>
        /// Called from a delegate set up by me.
        /// </summary>
        /// <param name="ACalc"></param>
        public void GenerateExtract(TRptCalculator ACalc)
        {
            ACalc.GetParameters().Add("param_design_template", false);

            if (FDataGetter == null)
            {
                MessageBox.Show(Catalog.GetString("Fault: No Data Table available."), Catalog.GetString("GenerateExtract"));
                return;
            }

            if (!FDataGetter(ACalc))
            {
                return;
            }

            FExtractPartnerKeyName = SelectColumnNameForExract(FClientDataTable, FExtractPartnerKeyName);

            if (FExtractPartnerKeyName == "")
            {
                return;
            }

            Int32 partnerKeyColumnNum = FClientDataTable.Columns[FExtractPartnerKeyName].Ordinal;

            TFrmExtractNamingDialog ExtractNameDialog = new TFrmExtractNamingDialog(FPetraUtilsObject.GetForm());
            string ExtractName;
            string ExtractDescription;

            ExtractNameDialog.ShowDialog();

            if (ExtractNameDialog.DialogResult != System.Windows.Forms.DialogResult.Cancel)
            {
                /* Get values from the Dialog */
                ExtractNameDialog.GetReturnedParameters(out ExtractName, out ExtractDescription);
            }
            else
            {
                // dialog was cancelled, do not continue with extract generation
                return;
            }

            ExtractNameDialog.Dispose();

            FPetraUtilsObject.GetForm().UseWaitCursor = true;

            // Create extract with given name and description and store it
            int ExtractId = 0;
            IPartnerUIConnectorsPartnerNewExtract PartnerExtractObject = TRemote.MPartner.Extracts.UIConnectors.PartnerNewExtract();
            Boolean CreateOk = PartnerExtractObject.CreateExtractFromListOfPartnerKeys(
                ExtractName, ExtractDescription, out ExtractId, FClientDataTable, partnerKeyColumnNum, false);
            FPetraUtilsObject.GetForm().UseWaitCursor = false;

            if (CreateOk)
            {
                MessageBox.Show(String.Format(Catalog.GetString("Extract Created with {0} Partners."),
                        FClientDataTable.Rows.Count),
                    Catalog.GetString("Generate Extract"));
            }
            else
            {
                MessageBox.Show(Catalog.GetString("Creation of extract failed"),
                    Catalog.GetString("Generate Extract"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// Called from a delegate set up by me.
        /// Or if you're not using a reporting UI, you can call this directly, once the data and params have been set up.
        /// </summary>
        /// <param name="ACalc"></param>
        public void GenerateReport(TRptCalculator ACalc)
        {
            ACalc.GetParameters().Add("param_design_template", false);

            if (FSelectedTemplate != null)
            {
                if (FDataGetter != null)
                {
                    if (!FDataGetter(ACalc))
                    {
                        return;
                    }
                }

                FFastReportType.GetMethod("LoadFromString", new Type[] { FSelectedTemplate.XmlText.GetType() }).Invoke(FfastReportInstance,
                    new object[] { FSelectedTemplate.XmlText });
                LoadReportParams(ACalc);
                FFastReportType.GetMethod("Show", new Type[0]).Invoke(FfastReportInstance, null);
            }

            if (FPetraUtilsObject != null)
            {
                FPetraUtilsObject.UpdateParentFormEndOfReport();
            }
        }

        /// <summary>
        /// Takes a report and a list of cost centres; it runs the report for each of the cost centres and emails the results to the address(es)
        /// associated with that cost centre. Used by Account Detail, Income Expense reports and HOSAs.
        /// </summary>
        /// <remarks>If ACostCentreFilter is an SQL clause selecting cost centres, the associated email address is the Primary E-Mail Address of
        /// every Partner linked to the cost centre. If ACostCentreFilter is "Foreign", the list is every cost centre in the a_email_destination
        /// table with a File Code of "HOSA".</remarks>
        /// <param name="FormUtils">The report selection form, to write to the status bar.</param>
        /// <param name="ReportEngine">FastReport wrapper object.</param>
        /// <param name="ACalc">The report parameters.</param>
        /// <param name="ALedgerNumber">The ledger number.</param>
        /// <param name="ACostCentreFilter">SQL clause to select the list of cost centres to run the report for, or "Foreign" </param>
        /// <returns>List of status strings that should be shown to the user.</returns>
        public static List <String>AutoEmailReports(TFrmPetraReportingUtils FormUtils, FastReportsWrapper ReportEngine,
            TRptCalculator ACalc, Int32 ALedgerNumber, String ACostCentreFilter)
        {
            TSmtpSender EmailSender;
            Int32 SuccessfulCount = 0;
            var NoEmailAddr = new List <String>();
            var FailedAddresses = new List <String>();
            var SendReport = new List <String>();

            // FastReport will use a temporary folder to store HTML files.
            // I need to ensure that the CurrectDirectory is somewhere writable:
            String prevCurrentDir = Directory.GetCurrentDirectory();


            //Get a path in the Public Documents folder
            String newDir = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.CommonDocuments), "OpenPetraOrg");

            //Check it exists, and if not create it
            if (!Directory.Exists(newDir))
            {
                try
                {
                    Directory.CreateDirectory(newDir);
                }
                catch (Exception ex)
                {
                    //Could not create the path so return useful debugging information:
                    SendReport.Add(Catalog.GetString("Error - could not create directory: "));
                    SendReport.Add(newDir);
                    SendReport.Add(ex.Message);

                    return SendReport;
                }
            }

            Directory.SetCurrentDirectory(newDir);

            // This gets email defaults from the user settings table
            //TUC_EmailPreferences.LoadEmailDefaults();

            try
            {
                EmailSender = new TSmtpSender();

                EmailSender.SetSender(TUserDefaults.GetStringDefault("SmtpFromAccount"), TUserDefaults.GetStringDefault("SmtpDisplayName"));
                EmailSender.CcEverythingTo = TUserDefaults.GetStringDefault("SmtpCcTo");
                EmailSender.ReplyTo = TUserDefaults.GetStringDefault("SmtpReplyTo");
            }
            catch (ESmtpSenderInitializeException e)
            {
                if (e.InnerException != null)
                {
                    // I'd write the full exception to the log file, but it still gets transferred to the client window status bar and is _really_ ugly.
                    //TLogging.Log("AutoEmailReports: " + e.InnerException.ToString());
                    TLogging.Log("AutoEmailReports: " + e.InnerException.Message);
                }

                SendReport.Add(e.Message);

                if (e.ErrorClass == TSmtpErrorClassEnum.secClient)
                {
                    SendReport.Add(Catalog.GetString("Check the Email tab in User Settings >> Preferences."));
                }

                return SendReport;
            }

            // I need to find the email addresses for the linked partners I'm sending to.
            DataTable LinkedPartners = null;

            LinkedPartners = TRemote.MFinance.Setup.WebConnectors.GetLinkedPartners(ALedgerNumber, ACostCentreFilter);
            LinkedPartners.DefaultView.Sort = "CostCentreCode";

            foreach (DataRowView rv in LinkedPartners.DefaultView)
            {
                DataRow LinkedPartner = rv.Row;
                Boolean reportAsAttachment = TUserDefaults.GetBooleanDefault("SmtpSendAsAttachment", true);

                if (LinkedPartner["EmailAddress"].ToString() != "")
                {
                    ACalc.AddStringParameter("param_linked_partner_cc", LinkedPartner["CostCentreCode"].ToString());
                    FormUtils.WriteToStatusBar("Generate " + ReportEngine.FReportName + " Report for " + LinkedPartner["PartnerShortName"]);
                    MemoryStream ReportStream = ReportEngine.ExportToStream(ACalc,
                        reportAsAttachment ? FastReportsWrapper.ReportExportType.Pdf : FastReportsWrapper.ReportExportType.Html);

                    // in OpenSource OpenPetra, we do not have and use the FastReport dlls
                    // if (ReportEngine.FfastReportInstance.ReportInfo.Description == "Empty")
                    // {
                    //    continue; // Don't send an empty report
                    // }

                    ReportStream.Position = 0;

                    String EmailBody = "";

                    if (reportAsAttachment)
                    {
                        EmailBody = TUserDefaults.GetStringDefault("SmtpEmailBody");
                        EmailSender.AttachFromStream(ReportStream, ReportEngine.FReportName + ".pdf");
                    }
                    else
                    {
                        StreamReader sr = new StreamReader(ReportStream);
                        EmailBody = sr.ReadToEnd();
                    }

                    Boolean SentOk = EmailSender.SendEmail(
                        LinkedPartner["EmailAddress"].ToString(),
                        ReportEngine.FReportName + " Report for " + LinkedPartner["PartnerShortName"] + ", Address=" + LinkedPartner["EmailAddress"],
                        EmailBody);

                    if (SentOk)
                    {
                        SuccessfulCount++;
                    }
                    else // Email didn't send for some reason
                    {
                        SendReport.Add(String.Format(
                                Catalog.GetString("Failed to send to {0}. Message returned: \"{1}\"."),
                                LinkedPartner["EmailAddress"],
                                EmailSender.ErrorStatus
                                ));

                        FailedAddresses.Add("    " + LinkedPartner["EmailAddress"]);
                    }
                }
                else // No Email Address for this Partner
                {
                    NoEmailAddr.Add("    " + String.Format("{0:D10}", LinkedPartner["PartnerKey"]) + " " + LinkedPartner["PartnerShortName"]);
                }
            }

            if (SuccessfulCount == 1)
            {
                SendReport.Add(
                    String.Format(Catalog.GetString("{0} emailed to {1} address."), ReportEngine.FReportName, SuccessfulCount));
            }
            else if (SuccessfulCount > 1)
            {
                SendReport.Add(
                    String.Format(Catalog.GetString("{0} emailed to {1} addresses."), ReportEngine.FReportName, SuccessfulCount));
            }
            else
            {
                SendReport.Add(Catalog.GetString(
                        "Error – no cost centre in the report was linked to a Partner with a valid Primary E-mail Address."));
            }

            if (NoEmailAddr.Count > 0)
            {
                SendReport.Add(Catalog.GetString("These Partners have no Primary E-mail Address:"));
                SendReport.AddRange(NoEmailAddr);
            }

            if (FailedAddresses.Count > 0)
            {
                SendReport.Add(Catalog.GetString("Failed to send email to these addresses:"));
                SendReport.AddRange(FailedAddresses);
            }

            FormUtils.WriteToStatusBar("");
            Directory.SetCurrentDirectory(prevCurrentDir);
            EmailSender.Dispose();
            return SendReport;
        } // AutoEmailReports

        /// <summary>Helper for the report printing ClientTask</summary>
        /// <param name="ReportName"></param>
        /// <param name="paramStr"></param>
        public static void PrintReportNoUi(String ReportName, String paramStr)
        {
            String[] Params = paramStr.Split(',');
            Int32 ledgerNumber = -1;
            Int32 batchNumber = -1;

            FastReportsWrapper ReportingEngine = new FastReportsWrapper(ReportName);

            if (!ReportingEngine.LoadedOK)
            {
                ReportingEngine.ShowErrorPopup();
                return;
            }

            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();
            TRptCalculator Calc = new TRptCalculator();

            //
            // Copy paramters to report:
            foreach (String param in Params)
            {
                String[] term = param.Split('=');

                if (term.Length > 1)
                {
                    if (term[1][0] == '"') // This is a string
                    {
                        String val = term[1].Substring(1, term[1].Length - 2);
                        Calc.AddStringParameter(term[0], val);
                        paramsDictionary.Add(term[0], new TVariant(val));
                    }
                    else // This is a number - Int32 assumed.
                    {
                        Int32 IntTerm;

                        if (Int32.TryParse(term[1], out IntTerm))
                        {
                            Calc.AddParameter(term[0], IntTerm);
                            paramsDictionary.Add(term[0], new TVariant(IntTerm));

                            //
                            // As I'm adding these values, I'll keep a note of any that may be useful later..
                            switch (term[0])
                            {
                                case "param_ledger_number_i":
                                {
                                    ledgerNumber = IntTerm;
                                    break;
                                }

                                case "param_batch_number_i":
                                {
                                    batchNumber = IntTerm;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error: Parameter not recognised: " + param, "FastReportWrapper.PrintReportNoUi");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error: malformed Parameter: " + param, "FastReportWrapper.PrintReportNoUi");
                }
            } // foreach param

            //
            // Get Data for report:
            switch (ReportName)
            {
                case "Gift Batch Detail":
                {
                    DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("GiftBatchDetail", paramsDictionary);
                    ReportingEngine.RegisterData(ReportTable, "GiftBatchDetail");
                    break;
                }
            } // switch

            // I'm not in the User Interface thread, so I can use an invoke here:

            TFormsList.GFormsList.MainMenuForm.Invoke((ThreadStart) delegate { ReportingEngine.GenerateReport(Calc); });
            //Application.OpenForms[0].Invoke((ThreadStart) delegate { ReportingEngine.GenerateReport(Calc); });
        } // PrintReportNoUi
    }
}
