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

                FFastReportType.GetMethod("LoadFromString", new Type[] { FSelectedTemplate.XmlText.GetType() }).Invoke(FfastReportInstance,
                    new object[] { FSelectedTemplate.XmlText });

                LoadReportParams(ACalc);
                FFastReportType.GetMethod("Design", new Type[0]).Invoke(FfastReportInstance, null);

                //
                // The user can change the report template - if it's changed I'll update the server
                // (unless the template is read-only, in which case I'll need to make a copy.)
                object ret = FFastReportType.GetMethod("SaveToString", new Type[0]).Invoke(FfastReportInstance, null);
                String XmlString = (String)ret;
                //
                // I only want to check part of the report to assess whether it's changed, otherwise it always detects a change
                // (the modified date is changed, and the parameters may also be different.)

                Boolean TemplateChanged = false;
                Int32 Page1Pos = XmlString.IndexOf("<ReportPage");
                Int32 PrevPage1Pos = FSelectedTemplate.XmlText.IndexOf("<ReportPage");

                if ((Page1Pos < 1) || (PrevPage1Pos < 1))
                {
                    TemplateChanged = true;
                }
                else
                {
                    if (XmlString.Substring(Page1Pos) != FSelectedTemplate.XmlText.Substring(PrevPage1Pos))
                    {
                        TemplateChanged = true;
                    }
                }

                if (TemplateChanged)
                {
                    Boolean MakeACopy = false;

                    if (FSelectedTemplate.Readonly)
                    {
                        if (MessageBox.Show(
                                String.Format(Catalog.GetString("{0} cannot be ovewritten.\r\nMake a copy instead?"), FSelectedTemplate.ReportVariant),
                                Catalog.GetString("Design Template"),
                                MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }

                        MakeACopy = true;
                    }
                    else
                    {
                        if (MessageBox.Show(
                                String.Format(Catalog.GetString("Save changes to {0}?"), FSelectedTemplate.ReportVariant),
                                Catalog.GetString("Design Template"),
                                MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    SReportTemplateTable TemplateTable = new SReportTemplateTable();
                    SReportTemplateRow NewRow = TemplateTable.NewRowTyped();
                    DataUtilities.CopyAllColumnValues(FSelectedTemplate, NewRow);
                    TemplateTable.Rows.Add(NewRow);

                    if (MakeACopy)
                    {
                        String currentUser = UserInfo.GUserInfo.UserID;
                        NewRow.TemplateId = -1; // The value will come from the sequence
                        NewRow.ReportVariant = String.Format(Catalog.GetString("{0} copy of {1}"), currentUser, TemplateTable[0].ReportVariant);
                        NewRow.Author = currentUser;
                        NewRow.Readonly = false;
                        NewRow.Default = false;
                        NewRow.PrivateDefault = false;
                    }
                    else
                    {
                        TemplateTable.AcceptChanges(); // Don't allow this one-row table to be seen as "new"
                    }

                    NewRow.XmlText = XmlString;
                    SReportTemplateTable Tbl = TRemote.MReporting.WebConnectors.SaveTemplates(TemplateTable);
                    Tbl.AcceptChanges();
                    SetTemplate(Tbl[0]);
                }
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
            object HtmlExport = FastReportsDll.CreateInstance("FastReport.Export.Html.HTMLExport");
            Type ExporterType = HtmlExport.GetType();
            MemoryStream HtmlStream = new MemoryStream();

            FFastReportType.GetMethod("LoadFromString", new Type[] { FSelectedTemplate.XmlText.GetType() }).Invoke(FfastReportInstance,
                new object[] { FSelectedTemplate.XmlText });
            LoadReportParams(ACalc);
            FFastReportType.GetMethod("Prepare", new Type[0]).Invoke(FfastReportInstance, null);
            FFastReportType.GetMethod("Export", new Type[] { ExporterType, HtmlStream.GetType() }).Invoke(FfastReportInstance,
                new Object[] { HtmlExport, HtmlStream });
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
        /// The report will be sent to a list of email addresses derived from the Cost Centres in the supplied CostCentreFilter.
        /// </summary>
        /// <returns>Status string that should be shown to the user</returns>
        public static String AutoEmailReports(TFrmPetraReportingUtils FormUtils, FastReportsWrapper ReportEngine,
            TRptCalculator ACalc, Int32 ALedgerNumber, String ACostCentreFilter)
        {
            Int32 SuccessfulCount = 0;
            String NoEmailAddr = "";
            String FailedAddresses = "";
            String SendReport = "Auto Email\r\n";

            //
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
                    //could not create the path so return useful debugging information:
                    SendReport += Catalog.GetString("\r\nError - could not create directory: " + newDir);
                    SendReport += Catalog.GetString("\r\n" + newDir);
                    SendReport += ex.Message;

                    return SendReport;
                }
            }

            Directory.SetCurrentDirectory(newDir);

            //
            // I need to find the email addresses for the linked partners I'm sending to.
            DataTable LinkedPartners = null;

            LinkedPartners = TRemote.MFinance.Setup.WebConnectors.GetLinkedPartners(ALedgerNumber, ACostCentreFilter);
            LinkedPartners.DefaultView.Sort = "CostCentreCode";

            foreach (DataRowView rv in LinkedPartners.DefaultView)
            {
                DataRow LinkedPartner = rv.Row;

                if (LinkedPartner["EmailAddress"].ToString() != "")
                {
                    ACalc.AddStringParameter("param_linked_partner_cc", LinkedPartner["CostCentreCode"].ToString());
                    FormUtils.WriteToStatusBar("Generate " + ReportEngine.FReportName + " Report for " + LinkedPartner["PartnerShortName"]);
                    MemoryStream ReportStream = ReportEngine.ExportToStream(ACalc, FastReportsWrapper.ReportExportType.Html);

                    // in OpenSource OpenPetra, we do not have and use the FastReport dlls
                    // if (ReportEngine.FfastReportInstance.ReportInfo.Description == "Empty")
                    // {
                    //    continue; // Don't send an empty report
                    // }

                    ReportStream.Position = 0;

                    // This gets email defaults from the user settings table
                    TUC_EmailPreferences.LoadEmailDefaults();

                    // This gets some of the settings from the server configuration.  We no longer get these items from local PC.
                    // SmtpUsername and SmtpPassword will usually be null
                    string smtpHost, smtpUsername, smtpPassword;
                    int smtpPort;
                    bool smtpUseSSL;
                    TRemote.MSysMan.Application.WebConnectors.GetServerSmtpSettings(out smtpHost,
                        out smtpPort,
                        out smtpUseSSL,
                        out smtpUsername,
                        out smtpPassword);

                    if ((smtpHost == string.Empty) || (smtpPort < 0))
                    {
                        return Catalog.GetString(
                            "Cannot send email because 'smtpHost' and/or 'smtpPort' are not configured in the OP server configuration file.");
                    }

                    TSmtpSender EmailSender = new TSmtpSender(smtpHost, smtpPort, smtpUseSSL, smtpUsername, smtpPassword, "");

                    EmailSender.CcEverythingTo = TUserDefaults.GetStringDefault("SmtpCcTo");
                    EmailSender.ReplyTo = TUserDefaults.GetStringDefault("SmtpReplyTo");

                    if (!EmailSender.FInitOk)
                    {
                        return String.Format(
                            Catalog.GetString(
                                "Failed to set up the email server.\n    Please check the settings in Preferences / Email.\n    Message returned : \"{0}\""),
                            EmailSender.FErrorStatus);
                    }

                    String EmailBody = "";

                    if (TUserDefaults.GetBooleanDefault("SmtpSendAsAttachment"))
                    {
                        EmailBody = TUserDefaults.GetStringDefault("SmtpEmailBody");
                        EmailSender.AttachFromStream(ReportStream, ReportEngine.FReportName + ".html");
                    }
                    else
                    {
                        StreamReader sr = new StreamReader(ReportStream);
                        EmailBody = sr.ReadToEnd();
                    }

                    Boolean SentOk = EmailSender.SendEmail(
                        TUserDefaults.GetStringDefault("SmtpFromAccount"),
                        TUserDefaults.GetStringDefault("SmtpDisplayName"),
                        LinkedPartner["EmailAddress"].ToString(),
                        ReportEngine.FReportName + " Report for " + LinkedPartner["PartnerShortName"] + ", Address=" + LinkedPartner["EmailAddress"],
                        EmailBody);

                    if (SentOk)
                    {
                        SuccessfulCount++;
                    }
                    else // Email didn't send for some reason
                    {
                        SendReport += String.Format(
                            Catalog.GetString("\r\nFailed to send to {0}. Message returned: \"{1}\"."),
                            LinkedPartner["EmailAddress"],
                            EmailSender.FErrorStatus
                            );

                        FailedAddresses += ("\r\n    " + LinkedPartner["EmailAddress"]);
                    }
                }
                else // No Email Address for this Partner
                {
                    NoEmailAddr += ("\r\n    " + LinkedPartner["PartnerKey"] + " " + LinkedPartner["PartnerShortName"]);
                }
            }

            if (SuccessfulCount > 0)
            {
                SendReport +=
                    String.Format(Catalog.GetString("\r\n{0} emailed to {1} addresses."), ReportEngine.FReportName, SuccessfulCount) + "\r\n\r\n";
            }
            else
            {
                SendReport += Catalog.GetString("\r\nError - no page had a linked email address.");
            }

            if (NoEmailAddr != "")
            {
                SendReport += (Catalog.GetString("\r\nThese Partners have no email addresses:") + NoEmailAddr + "\r\n");
            }

            if (FailedAddresses != "")
            {
                SendReport += (Catalog.GetString("Failed to send email to these addresses:") + FailedAddresses + "\r\n\r\n");
            }

            FormUtils.WriteToStatusBar("");
            Directory.SetCurrentDirectory(prevCurrentDir);
            return SendReport;
        } // AutoEmailReports

        private void ShowBadBatchNumMessageInUiThread(Int32 ABatchNumber)
        {
            if (FPetraUtilsObject == null)
            {
                return;
            }

            Form ParentForm = FPetraUtilsObject.GetCallerForm();

            if (ParentForm.InvokeRequired)
            {
                ParentForm.Invoke((MethodInvoker) delegate
                    {
                        ShowBadBatchNumMessageInUiThread(ABatchNumber);
                        return;
                    });;
            }
            else
            {
                MessageBox.Show(String.Format(Catalog.GetString("Batch {0} not found"), ABatchNumber),
                    Catalog.GetString("Batch Posting Register"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>Get all the data for the report</summary>
        /// <remarks>Called from the server during batch posting, and also from File/Print gui</remarks>
        /// <param name="ACalc"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        public Boolean RegisterBatchPostingData(TRptCalculator ACalc, Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GLBatchTDS BatchTDS = null;

            try
            {
                BatchTDS = TRemote.MFinance.GL.WebConnectors.LoadABatchAndRelatedTablesUsingPrivateDb(ALedgerNumber, ABatchNumber);
            }
            catch
            {
            }         // Ignore this error and instead detect the empty batch, below:

            if ((BatchTDS == null) || (BatchTDS.ABatch == null) || (BatchTDS.ABatch.Rows.Count < 1))
            {
                ShowBadBatchNumMessageInUiThread(ABatchNumber);
                return false;
            }

            //Call RegisterData to give the data to the template
            RegisterData(BatchTDS.ABatch, "ABatch");
            RegisterData(BatchTDS.AJournal, "AJournal");
            RegisterData(BatchTDS.ATransaction, "ATransaction");
            RegisterData(TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList,
                    ALedgerNumber), "AAccount");
            RegisterData(TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList,
                    ALedgerNumber), "ACostCentre");

            ACalc.AddParameter("param_batch_number_i", ABatchNumber);
            ACalc.AddParameter("param_ledger_number_i", ALedgerNumber);
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(ALedgerNumber);
            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ALedgerTable LedgerTable = (ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails);

            ACalc.AddStringParameter("param_linked_partner_cc", ""); // I may want to use this for auto_email, but usually it's unused.
            ACalc.AddParameter("param_currency_name", LedgerTable[0].BaseCurrency);
            return true;
        }

        /// <summary>Helper for the report printing ClientTask</summary>
        /// <param name="ReportName"></param>
        /// <param name="paramStr"></param>
        public static void PrintReportNoUi(String ReportName, String paramStr)
        {
            String[] Params = paramStr.Split(',');
            Int32 LedgerNumber = -1;
            Int32 BatchNumber = -1;

/*
 *          String Msg = ReportName + "\n";
 *          foreach (String param in Params)
 *          {
 *              Msg += (param + "\n");
 *          }
 *          MessageBox.Show(Msg, "FastReportWrapper.PrintReportNoUi");
 */
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
                                    LedgerNumber = IntTerm;
                                    break;
                                }

                                case "param_batch_number_i":
                                {
                                    BatchNumber = IntTerm;
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
                case "Batch Posting Register":
                {
                    if ((LedgerNumber != -1) && (BatchNumber != -1))
                    {
                        ReportingEngine.RegisterBatchPostingData(Calc, LedgerNumber, BatchNumber);
                    }
                    else
                    {
                        MessageBox.Show("Error: Can't get data for Batch Posting Register", "FastReportWrapper.PrintReportNoUi");
                    }

                    break;
                }

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
