//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
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
using FastReport.Export;
using FastReport.Utils;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace Ict.Petra.Client.MFastReport.Gui
{
    public partial class TFrmEmailDialog
    {
        private TSmtpSender FSender;
        private bool FSendAsAttachment;
        private const string LASTFILTER = "EmailLastFilter";
        private const string OPTIONS = "EmailOptions";

        /// <summary>
        /// Get or set the report that is to be emailed. Needs to be set before the dialog is shown.
        /// </summary>
        public FastReport.Report ReportInstance {
            get; set;
        }

        private void InitializeManualCode()
        {
            FSender = InitialiseEmailSettings();

            FSendAsAttachment = TUserDefaults.GetBooleanDefault("SmtpSendAsAttachment");

            cmbTo.AcceptNewValues = true;
            var RecentAddrs = new StringCollection();
            RecentAddrs.AddRange(TUserDefaults.GetStringDefault(TSmtpSender.RECENTADDRS).Split('|'));
            cmbTo.SetDataSourceStringList(RecentAddrs);
            txtCC.Text = TSmtpSender.ConvertAddressList(TUserDefaults.GetStringDefault("SmtpCcTo"));

            GetOptions();

            cmbType.SuspendLayout();
            cmbType.DisplayMember = "Description";
            cmbType.ValueMember = "Exporter";

            if (FSendAsAttachment)
            {
                txtMessage.Text = TUserDefaults.GetStringDefault("SmtpEmailBody");

                cmbType.DataSource = GetExportItemsView(new Type[] {  typeof(FastReport.Export.Pdf.PDFExport),
                                                                      typeof(FastReport.Export.Csv.CSVExport),
                                                                      typeof(FastReport.Export.OoXML.Excel2007Export),
                                                                      typeof(FastReport.Export.OoXML.Word2007Export),
                                                                      typeof(FastReport.Export.Html.HTMLExport) });
            }
            else
            {
                lblMessage.Visible = txtMessage.Visible = false;
                this.ClientSize = new System.Drawing.Size(436, 198);
                cmbType.DataSource = GetExportItemsView(new Type[] { typeof(FastReport.Export.Html.HTMLExport),
                                                                     typeof(FastReport.Export.Csv.CSVExport) });
            }

            cmbType.SelectedIndex = cmbType.FindExactString(TUserDefaults.GetStringDefault(LASTFILTER), 0);

            if ((cmbType.SelectedIndex < 0) && (cmbType.Items.Count > 0))
            {
                cmbType.SelectedIndex = 0;
            }

            cmbType.ResumeLayout(false);
        }

        private TSmtpSender InitialiseEmailSettings()
        {
            TSmtpSender Sender = null;

            try
            {
                Sender = new TSmtpSender();

                string Address = TUserDefaults.GetStringDefault("SmtpFromAccount");
                string DisplayName = TUserDefaults.GetStringDefault("SmtpDisplayName");
                Sender.SetSender(Address, DisplayName);

                Sender.ReplyTo = TUserDefaults.GetStringDefault("SmtpReplyTo");
            }
            catch (ESmtpSenderInitializeException e)
            {
                var Message = new StringBuilder();

                Message.AppendLine(e.Message);

                if (e.ErrorClass == TSmtpErrorClassEnum.secClient)
                {
                    Message.AppendLine();
                    Message.AppendLine(Catalog.GetString("Check the Email tab in User Settings >> Preferences."));
                }

                MessageBox.Show(Message.ToString(), "E-Mail Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TLogging.Log("FastReport email initialization failed: " + e.Message);

                if (e.InnerException != null)
                {
                    //TLogging.Log("FastReport: " + e.InnerException.ToString());
                    TLogging.Log("FastReport: " + e.InnerException.Message);
                }

                DisposeOf(Sender);

                throw;
            }
            return Sender;
        }

        private void Form_Shown(Object sender, EventArgs e)
        {
            if (ReportInstance == null)
            {
                throw new ArgumentNullException("Report", "Report property must be set before showing this dialog.");
            }

            txtSubject.Text = ReportInstance.ReportInfo.Name;
        }

        private void Form_FormClosing(Object sender, EventArgs e)
        {
            DisposeOf(FSender);
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            MailMessage Email = null;
            string ErrorMessage = null;
            var ErrorTitle = Catalog.GetString("E-Mail Error");

            var Exporter = (ExportBase)Activator.CreateInstance(((ObjectInfo)cmbType.SelectedValue).Object);

            SetOptions(Exporter);

            try
            {
                Email = FSender.GetNewMailMessage();

                ErrorMessage = "To";    // Used by the exception handler
                Email.To.Add(TSmtpSender.ConvertAddressList(cmbTo.Text));

                ErrorMessage = "CC";

                if (txtCC.Text != "")
                {
                    Email.CC.Add(TSmtpSender.ConvertAddressList(txtCC.Text));
                }

                Email.Subject = txtSubject.Text;

                ErrorMessage = Catalog.GetString("Send failed");

                Cursor = Cursors.WaitCursor;

                using (var Reader = new StreamReader(new MemoryStream()))
                {
                    ReportInstance.Export(Exporter, Reader.BaseStream);
                    Reader.BaseStream.Position = 0;

                    if (FSendAsAttachment)
                    {
                        Email.Body = txtMessage.Text;

                        Email.Attachments.Add(new Attachment(Reader.BaseStream, ReportInstance.ReportInfo.Name +
                                Path.GetExtension(Exporter.FileFilter.Split('|')[1])));
                    }
                    else
                    {
                        Email.Body = Reader.ReadToEnd();
                    }

                    FSender.SendMessage(Email);
                }

                if (!cmbTo.Items.Contains(cmbTo.Text))
                {
                    cmbTo.Items.Insert(0, cmbTo.Text);
                }

                TUserDefaults.SetDefault(TSmtpSender.RECENTADDRS, String.Join("|", cmbTo.Items.Cast <String>().Take(10).ToArray()));
                TUserDefaults.SetDefault(LASTFILTER, cmbType.Text);

                this.DialogResult = DialogResult.OK;
            }
            catch (ESmtpSenderInitializeException ex)
            {
                MessageBox.Show(String.Format("{0}: {1}\n\n{2}", ErrorMessage, ex.Message,
                        Catalog.GetString(
                            "Check the Email tab in User Settings >> Preferences.")), ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (ex.InnerException != null)
                {
                    // I'd write the full exception to the log file, but it still gets transferred to the client window status bar and is _really_ ugly.
                    //TLogging.Log("FastReport: " + ex.InnerException.ToString());
                    TLogging.Log("FastReport: " + ex.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}: {1}", ErrorMessage, ex.Message), ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (ex.InnerException != null)
                {
                    //TLogging.Log("FastReport: " + ex.InnerException.ToString());
                    TLogging.Log("FastReport: " + ex.InnerException.Message);
                }
            }
            finally
            {
                Cursor = Cursors.Default;
                DisposeOf(Email);
                DisposeOf(Exporter);
            }
        }

        private void cmbType_Changed(Object sender, EventArgs e)
        {
            if (cmbType.SelectedValue == null)
            {
                return;
            }

            pnlPDFExport.Visible = false;
            pnlExcel2007Export.Visible = false;

            switch (((ObjectInfo)cmbType.SelectedValue).Object.Name)
            {
                case "PDFExport":
                    pnlPDFExport.Visible = true;
                    break;

                case "Excel2007Export":
                    pnlExcel2007Export.Visible = true;
                    break;
            }
        }

        private void txtMessage_Enter(Object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }

        private void txtMessage_Leave(Object sender, EventArgs e)
        {
            this.AcceptButton = btnOK;
        }

        private DataView GetExportItemsView(Type[] ATypes)
        {
            var ItemTable = new DataTable();

            ItemTable.Columns.Add(new DataColumn("Description", typeof(string)));
            ItemTable.Columns.Add(new DataColumn("Exporter", typeof(ObjectInfo)));

            foreach (var Exporter in ATypes)
            {
                var ExportObject = RegisteredObjects.FindObject(Exporter);

                if (ExportObject == null)
                {
                    continue;
                }

                ItemTable.Rows.Add(new object[] { Res.TryGet(ExportObject.Text), ExportObject });
            }

            return ItemTable.DefaultView;
        }

        private void DisposeOf(IDisposable AThing)
        {
            if (AThing != null)
            {
                AThing.Dispose();
            }
        }

        /// <summary>
        /// Get the export options from user defaults and sets the values of the option controls.
        /// </summary>
        private void GetOptions()
        {
            var Opts = TUserDefaults.GetStringDefault(OPTIONS, "PDFExport|True|Excel2007Export|False").Split('|');
            var counter = 0;

            while (counter < Opts.Length)
            {
                switch (Opts[counter])
                {
                    case "PDFExport":
                        chkEmbedFonts.Checked = (Opts[++counter] != "False");         // On unless explicitly set to "False"
                        break;

                    case "Excel2007Export":
                        chkPageBreaks.Checked = (Opts[++counter] == "True");    // Off unless explicitly set to "True"
                        break;

                    default:
                        ++counter;
                        break;
                }

                ++counter;
            }
        }

        /// <summary>
        /// Set the export options from the values in the option controls, and save them back to the database.
        /// </summary>
        private void SetOptions(ExportBase Exporter)
        {
            var Opts = new List <string>();

            Opts.Add("PDFExport");

            if (Exporter is FastReport.Export.Pdf.PDFExport)
            {
                ((FastReport.Export.Pdf.PDFExport)Exporter).EmbeddingFonts = chkEmbedFonts.Checked;
            }

            Opts.Add(chkEmbedFonts.Checked.ToString());

            Opts.Add("Excel2007Export");

            if (Exporter is FastReport.Export.OoXML.Excel2007Export)
            {
                ((FastReport.Export.OoXML.Excel2007Export)Exporter).PageBreaks = chkPageBreaks.Checked;
            }

            Opts.Add(chkPageBreaks.Checked.ToString());

            TUserDefaults.SetDefault(OPTIONS, String.Join("|", Opts));
        }
    }
}