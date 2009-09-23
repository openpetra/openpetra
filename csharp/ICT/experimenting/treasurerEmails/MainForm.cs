/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Drawing.Printing;
using Mono.Unix;
using Ict.Common.IO;
using Ict.Common;
using Ict.Common.Printing;
using Ict.Common.Controls;

namespace treasurerEmails
{
/// <summary>
/// Description of MainForm.
/// </summary>
public partial class MainForm : Form
{
    public MainForm()
    {
        //
        // The InitializeComponent() call is required for Windows Forms designer support.
        //
        InitializeComponent();

        grdEmails.Columns.Add("id", "ID");
        grdEmails.Columns.Add("sent", "Send Date");
        grdEmails.Columns.Add("recipient", "To");
        grdEmails.Columns.Add("subject", "Subject");
        grdEmails.Columns[0].Width = 30;
        grdEmails.Columns[1].Width = 100;
        grdEmails.Columns[2].Width = 200;
        grdEmails.Columns[3].Width = 400;
        grdEmails.ReadOnly = true;
        grdEmails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grdEmails.MultiSelect = false;
        grdEmails.AllowUserToAddRows = false;
        grdEmails.RowHeadersVisible = false;

        grdLetters.Columns.Add("id", "ID");
        grdLetters.Columns.Add("recipient", "To");
        grdLetters.Columns.Add("subject", "Subject");
        grdLetters.Columns[0].Width = 30;
        grdLetters.Columns[1].Width = 200;
        grdLetters.Columns[2].Width = 400;
        grdLetters.ReadOnly = true;
        grdLetters.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grdLetters.MultiSelect = false;
        grdLetters.AllowUserToAddRows = false;
        grdLetters.RowHeadersVisible = false;

        dtpLastMonth.Value = DateTime.Now.AddMonths(-1);
    }

    private List <MailMessage>FEmails = null;
    private List <LetterMessage>FLetters = null;

    /// <summary>
    /// set the emails that are in the outbox
    /// </summary>
    /// <param name="AEmails"></param>
    public void SetEmails(List <MailMessage>AEmails)
    {
        FEmails = AEmails;
        RefreshGridEmails();

        if (FEmails.Count == 0)
        {
            tabOutput.SelectedTab = tpgLetters;
        }
    }

    /// <summary>
    /// set the letters to be printed
    /// </summary>
    /// <param name="AEmails"></param>
    public void SetLetters(List <LetterMessage>ALetters)
    {
        FLetters = ALetters;
        RefreshGridLetters();
        PreparePrintLetters();
    }

    void RefreshGridEmails()
    {
        grdEmails.Rows.Clear();

        foreach (MailMessage email in FEmails)
        {
            grdEmails.Rows.Add(new object[] { grdEmails.Rows.Count + 1, email.Headers.Get("Date-Sent"),
                                              email.To.ToString(), email.Subject });
        }
    }

    void RefreshGridLetters()
    {
        grdLetters.Rows.Clear();

        foreach (LetterMessage letter in FLetters)
        {
            grdLetters.Rows.Add(new object[] { grdLetters.Rows.Count + 1, letter.RecipientShortName, letter.Subject });
        }
    }

    /// <summary>
    /// select a row and display the email in the web browser control below the list
    /// </summary>
    /// <param name="ARow"></param>
    void SelectEmailRow(int ARow)
    {
        if ((FEmails == null) || (ARow < 0) || (ARow >= FEmails.Count))
        {
            // invalid row
            return;
        }

        MailMessage selectedMail = FEmails[ARow];
        string header = "<html><body>";
        header += String.Format("{0}: {1}<br/>",
            Catalog.GetString("From"),
            selectedMail.From.ToString());
        header += String.Format("{0}: {1}<br/>",
            Catalog.GetString("To"),
            selectedMail.To);

        if (selectedMail.CC.Count > 0)
        {
            header += String.Format("{0}: {1}<br/>",
                Catalog.GetString("Cc"),
                selectedMail.CC);
        }

        if (selectedMail.Bcc.Count > 0)
        {
            header += String.Format("{0}: {1}<br/>",
                Catalog.GetString("Cc"),
                selectedMail.Bcc);
        }

        header += String.Format("<b>{0}: {1}</b><br/>",
            Catalog.GetString("Subject"),
            selectedMail.Subject);
        header += "<hr></body></html>";

        if (selectedMail.IsBodyHtml)
        {
            brwEmailContent.DocumentText = header + selectedMail.Body;
        }
        else
        {
            brwEmailContent.DocumentText = header +
                                           "<html><body>" + selectedMail.Body +
                                           "</body></html>";
        }
    }

    private Int32 FNumberOfPages = 0;
    private TGfxPrinter FGfxPrinter = null;

    /// <summary>
    /// display all letters in the web browser control below the list
    /// </summary>
    /// <param name="ARow"></param>
    void PreparePrintLetters()
    {
        if (FLetters == null)
        {
            return;
        }

        System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
        bool printerInstalled = printDocument.PrinterSettings.IsValid;

        if (printerInstalled)
        {
            string AllLetters = String.Empty;

            foreach (LetterMessage letter in FLetters)
            {
                if (AllLetters.Length > 0)
                {
                    // AllLetters += "<div style=\"page-break-before: always;\"/>";
                    string body = letter.HtmlMessage.Substring(letter.HtmlMessage.IndexOf("<body"));
                    body = body.Substring(0, body.IndexOf("</html"));
                    AllLetters += body;
                }
                else
                {
                    // without closing html
                    AllLetters += letter.HtmlMessage.Substring(0, letter.HtmlMessage.IndexOf("</html"));
                }
            }

            if (AllLetters.Length > 0)
            {
                AllLetters += "</html>";
            }

            string letterTemplateFilename = TAppSettingsManager.GetValueStatic("LetterTemplate.File");
            FGfxPrinter = new TGfxPrinter(printDocument);
            try
            {
                TPrinterHtml htmlPrinter = new TPrinterHtml(AllLetters,
                    System.IO.Path.GetDirectoryName(letterTemplateFilename),
                    FGfxPrinter);
                FGfxPrinter.Init(eOrientation.ePortrait, htmlPrinter);
                this.preLetter.InvalidatePreview();
                this.preLetter.Document = FGfxPrinter.Document;
                this.preLetter.Zoom = 1;
                FGfxPrinter.Document.EndPrint += new PrintEventHandler(this.EndPrint);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }

    void EndPrint(object ASender, PrintEventArgs AEv)
    {
        FNumberOfPages = FGfxPrinter.NumberOfPages;
        RefreshPagePosition();
    }

    void GrdEmailsCellEnter(object sender, DataGridViewCellEventArgs e)
    {
        SelectEmailRow(e.RowIndex);
    }

    private TSmtpSender CreateConnection()
    {
        return new TSmtpSender();
    }

    void BtnSendOneEmailClick(object sender, EventArgs e)
    {
        TSmtpSender smtp = CreateConnection();

        if (grdEmails.SelectedRows.Count == 1)
        {
            MailMessage selectedMail = FEmails[grdEmails.SelectedRows[0].Index];
            smtp.SendMessage(ref selectedMail);
            RefreshGridEmails();
        }
    }

    void BtnSendAllEmailsClick(object sender, EventArgs e)
    {
        TSmtpSender smtp = CreateConnection();

        for (Int16 Count = 0; Count < FEmails.Count; Count++)
        {
            MailMessage mail = FEmails[Count];

            if (!smtp.SendMessage(ref mail))
            {
                return;
            }

            RefreshGridEmails();
        }
    }

    void BtnGenerateEmailsClick(object sender, EventArgs e)
    {
        new TLogging("treasurerEmails.log");

        if (grdEmails.Rows.Count > 0)
        {
            MessageBox.Show("will not reload because emails might have been sent already");
            return;
        }

        DataSet allTreasurerEmails;
#if DEBUG
        Cursor = Cursors.WaitCursor;

        TAppSettingsManager settings = new TAppSettingsManager();
        allTreasurerEmails = TGetTreasurerData.GetTreasurerData(
            settings.GetValue("odbc.username"),
            settings.GetValue("odbc.password"),
            settings.GetInt32("ledger"),
            settings.GetValue("motivationgroup"),
            settings.GetValue("motivationdetail"),
            dtpLastMonth.Value, Convert.ToInt16(nudNumberMonths.Value));

        Cursor = Cursors.Default;
#else
        TFrmDBLogin login = new TFrmDBLogin();

        if (login.ShowDialog() == DialogResult.OK)
        {
            Cursor = Cursors.WaitCursor;

            // TODO ledger number, motivation group and detail
            allTreasurerEmails = TGetTreasurerData.GetTreasurerData(
                login.Username, login.Password,
                dtpLastMonth.Value, nudNumberMonths.Value);
            Cursor = Cursors.Default;
        }
#endif
        SetEmails(TGetTreasurerData.GenerateEmails(allTreasurerEmails, settings.GetValue("senderemailaddress"), chkLettersOnly.Checked));
        SetLetters(TGetTreasurerData.GenerateLetters(allTreasurerEmails, chkLettersOnly.Checked));
    }

    void RefreshPagePosition()
    {
        lblTotalNumberPages.Text = String.Format(Catalog.GetString("of {0}"), FNumberOfPages);
        txtCurrentPage.Text = (this.preLetter.StartPage + 1).ToString();
    }

    void TbbPrevPageClick(object sender, EventArgs e)
    {
        if (this.preLetter.StartPage > 0)
        {
            this.preLetter.StartPage = this.preLetter.StartPage - 1;
            RefreshPagePosition();
        }
    }

    void TbbNextPageClick(object sender, EventArgs e)
    {
        if (this.preLetter.StartPage + 1 < FNumberOfPages)
        {
            this.preLetter.StartPage = this.preLetter.StartPage + 1;
            RefreshPagePosition();
        }
    }

    void TxtCurrentPageTextChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 NewCurrentPage = Convert.ToInt32(txtCurrentPage.Text);

            if ((NewCurrentPage > 0) && (NewCurrentPage <= FNumberOfPages))
            {
                this.preLetter.StartPage = NewCurrentPage - 1;
                SelectInGrid(NewCurrentPage);
            }
        }
        catch (Exception)
        {
        }
    }

    void GrdLettersSelectionChanged(object sender, System.EventArgs e)
    {
        //SelectLetterRow(e.RowIndex);
        // TODO: get the currently selected row, the appropriate ID, and scroll to the printed page
    }

    void SelectInGrid(Int32 AId)
    {
        // TODO select the appropriate row in the grid
    }

    Int32 GetIndexFromRow(DataGridViewRow ARow)
    {
        return ARow.Index;
    }

    void TbbPrintCurrentPageClick(object sender, EventArgs e)
    {
        PrintDialog dlg = new PrintDialog();

        dlg.Document = FGfxPrinter.Document;
        dlg.AllowCurrentPage = true;
        dlg.AllowSomePages = true;
        dlg.PrinterSettings.PrintRange = PrintRange.SomePages;
        dlg.PrinterSettings.FromPage = GetIndexFromRow(grdLetters.SelectedRows[0]) + 1;
        dlg.PrinterSettings.ToPage = dlg.PrinterSettings.FromPage;

        if (dlg.ShowDialog() == DialogResult.OK)
        {
            //FGfxPrinter.Document.PrinterSettings = dlg.PrinterSettings;
            dlg.Document.Print();
        }
    }
}
}