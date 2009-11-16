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

        grdAllWorkers.Columns.Add("id", "ID");
        grdAllWorkers.Columns.Add("sendas", "Send As");
        grdAllWorkers.Columns.Add("treasurer", "Treasurer");
        grdAllWorkers.Columns.Add("treasurerkey", "Treasurer Key");
        grdAllWorkers.Columns.Add("worker", "Worker");
        grdAllWorkers.Columns.Add("workerkey", "Worker Key");
        grdAllWorkers.Columns.Add("transition", "Transition");
        grdAllWorkers.Columns.Add("error", "Error");
        grdAllWorkers.Columns.Add("LetterMessagePointer", "LetterMessagePointer");
        grdAllWorkers.Columns[0].Width = 30;
        grdAllWorkers.Columns[1].Width = 80;
        grdAllWorkers.Columns[2].Width = 150;
        grdAllWorkers.Columns[3].Width = 100;
        grdAllWorkers.Columns[4].Width = 150;
        grdAllWorkers.Columns[5].Width = 100;
        grdAllWorkers.Columns[6].Width = 30;
        grdAllWorkers.Columns[7].Width = 400;
        grdAllWorkers.Columns[8].Visible = false;
        grdAllWorkers.ReadOnly = true;
        grdAllWorkers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grdAllWorkers.MultiSelect = false;
        grdAllWorkers.AllowUserToAddRows = false;
        grdAllWorkers.RowHeadersVisible = false;

        grdEmails.Columns.Add("id", "ID");
        grdEmails.Columns.Add("sent", "Send Date");
        grdEmails.Columns.Add("recipient", "To");
        grdEmails.Columns.Add("recipientshortname", "RecipientName");
        grdEmails.Columns.Add("subject", "Subject");
        grdEmails.Columns.Add("LetterMessagePointer", "LetterMessagePointer");
        grdEmails.Columns[0].Width = 30;
        grdEmails.Columns[1].Width = 100;
        grdEmails.Columns[2].Width = 200;
        grdEmails.Columns[3].Width = 200;
        grdEmails.Columns[4].Width = 400;
        grdEmails.Columns[5].Visible = false;
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

    private List <LetterMessage>FLetters = null;

    void RefreshStatistics()
    {
        List <string>EmailRecipientsUnique = new List <string>();
        List <Int64>LetterRecipientsUnique = new List <Int64>();
        List <Int64>WorkersAlltogether = new List <Int64>();
        List <Int64>WorkersInTransition = new List <Int64>();
        List <Int64>ExWorkersWithGifts = new List <Int64>();
        Int32 CountInvalidAddressTreasurer = 0;
        Int32 CountMissingTreasurer = 0;
        Int32 CountPagesSent = 0;

        foreach (LetterMessage msg in FLetters)
        {
            if (!WorkersAlltogether.Contains(msg.SubjectKey))
            {
                WorkersAlltogether.Add(msg.SubjectKey);
            }

            if (msg.Transition && !WorkersInTransition.Contains(msg.SubjectKey))
            {
                WorkersInTransition.Add(msg.SubjectKey);
            }

            if (msg.ErrorMessage == "NOADDRESS")
            {
                CountInvalidAddressTreasurer++;
            }
            else if (msg.ErrorMessage == "NOTREASURER")
            {
                CountMissingTreasurer++;
            }
            else if (msg.ErrorMessage == "EXWORKER")
            {
                if (!ExWorkersWithGifts.Contains(msg.SubjectKey))
                {
                    ExWorkersWithGifts.Add(msg.SubjectKey);
                }
            }
            else
            {
                if (msg.SendAsEmail() && !EmailRecipientsUnique.Contains(msg.MessageRecipientShortName))
                {
                    EmailRecipientsUnique.Add(msg.MessageRecipientShortName);
                }

                if (msg.SendAsLetter() && !LetterRecipientsUnique.Contains(msg.MessageRecipientKey))
                {
                    LetterRecipientsUnique.Add(msg.MessageRecipientKey);
                }

                if (msg.SendAsLetter() || msg.SendAsEmail())
                {
                    CountPagesSent++;
                }
            }
        }

        txtTreasurersEmail.Text = EmailRecipientsUnique.Count.ToString();
        txtTreasurersLetter.Text = LetterRecipientsUnique.Count.ToString();
        txtNumberOfWorkersReceivingDonations.Text = WorkersAlltogether.Count.ToString();
        txtWorkersWithTreasurer.Text = (WorkersAlltogether.Count - CountMissingTreasurer).ToString();
        txtNumberOfUniqueTreasurers.Text = (EmailRecipientsUnique.Count + LetterRecipientsUnique.Count).ToString();
        txtWorkersWithoutTreasurer.Text = CountMissingTreasurer.ToString();
        txtTreasurerInvalidAddress.Text = CountInvalidAddressTreasurer.ToString();
        txtWorkersInTransition.Text = WorkersInTransition.Count.ToString();
        txtPagesSent.Text = CountPagesSent.ToString();
        txtExWorkersWithGifts.Text = ExWorkersWithGifts.Count.ToString();
    }

    void RefreshGridEmails()
    {
        grdEmails.Rows.Clear();

        foreach (LetterMessage email in FLetters)
        {
            if (email.SendAsEmail())
            {
                grdEmails.Rows.Add(new object[] { grdEmails.Rows.Count + 1,
                                                  email.EmailMessage.Headers.Get("Date-Sent"),
                                                  email.EmailMessage.To.ToString(),
                                                  email.MessageRecipientShortName,
                                                  email.EmailMessage.Subject,
                                                  email });
            }
        }
    }

    void RefreshGridLetters()
    {
        grdLetters.Rows.Clear();

        foreach (LetterMessage letter in FLetters)
        {
            if (letter.SendAsLetter())
            {
                grdLetters.Rows.Add(new object[] { grdLetters.Rows.Count + 1, letter.MessageRecipientShortName, letter.Subject });
            }
        }
    }

    void RefreshWorkers()
    {
        List <Int64>workers = new List <Int64>();

        grdAllWorkers.Rows.Clear();

        foreach (LetterMessage m in FLetters)
        {
            string localisedErrormessage = m.ErrorMessage;

            if (localisedErrormessage == "NOTREASURER")
            {
                localisedErrormessage = Catalog.GetString("No treasurer assigned to this worker");
            }
            else if (localisedErrormessage == "NOADDRESS")
            {
                localisedErrormessage = Catalog.GetString("There is no valid address for the treasurer");
            }
            else if (localisedErrormessage == "EXWORKER")
            {
                localisedErrormessage = Catalog.GetString("The Worker has left the organisation and is not in TRANSITION anymore");
            }

            grdAllWorkers.Rows.Add(new object[] { grdAllWorkers.Rows.Count + 1,
                                                  m.ErrorMessage.Length > 0 ? "Nothing" : ((m.EmailMessage != null) ? "Email" : "Letter"),
                                                  m.MessageRecipientShortName,
                                                  m.MessageRecipientKey,
                                                  m.SubjectShortName,
                                                  m.SubjectKey,
                                                  m.Transition,
                                                  localisedErrormessage,
                                                  m });
        }
    }

    /// <summary>
    /// display the email in the web browser control below the list
    /// </summary>
    void DisplayEmail(LetterMessage ALetterMessage)
    {
        MailMessage selectedMail = ALetterMessage.EmailMessage;

        if (selectedMail == null)
        {
            // should not get here
            return;
        }

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
                Catalog.GetString("Bcc"),
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
                if (letter.SendAsLetter())
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

    private TSmtpSender CreateConnection()
    {
        return new TSmtpSender();
    }

    void BtnSendOneEmailClick(object sender, EventArgs e)
    {
        TSmtpSender smtp = CreateConnection();

        if (grdEmails.SelectedRows.Count == 1)
        {
            DataGridViewRow row = grdEmails.SelectedRows[0];
            DataGridViewCell cell = row.Cells["LetterMessagePointer"];
            LetterMessage selectedMail = (LetterMessage)cell.Value;

            if (selectedMail.SendAsEmail())
            {
                smtp.SendMessage(ref selectedMail.EmailMessage);
                RefreshGridEmails();
            }
        }
    }

    void BtnSendAllEmailsClick(object sender, EventArgs e)
    {
        if (MessageBox.Show(Catalog.GetString("Do you really want to send the emails?"),
                Catalog.GetString("Confirm sending emails"),
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
        {
            return;
        }

        TSmtpSender smtp = CreateConnection();

        // TODO: dialog that allows to cancel???
        Cursor = Cursors.WaitCursor;

        foreach (LetterMessage mail in FLetters)
        {
            if (mail.SendAsEmail())
            {
//                mail.EmailMessage.To.Clear();
//                mail.EmailMessage.To.Add("justfortest@example.org");

                if (!smtp.SendMessage(ref mail.EmailMessage))
                {
                    RefreshGridEmails();
                    return;
                }

                // TODO: add email to p_partner_contact
                // TODO: add email to sent box???
            }
        }

        Cursor = Cursors.Default;

        RefreshGridEmails();
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

        FLetters = TGetTreasurerData.GenerateMessages(allTreasurerEmails, settings.GetValue(
                "senderemailaddress"), chkLettersOnly.Checked, dtpLastMonth.Value);

        if (FLetters.Count == 0)
        {
            MessageBox.Show(Catalog.GetString("There are no gifts in the given period of time"));
            return;
        }

        RefreshWorkers();
        RefreshGridLetters();
        RefreshGridEmails();
        PreparePrintLetters();
        RefreshStatistics();
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

    void GrdAllWorkersDoubleClick(object sender, System.EventArgs e)
    {
        if (grdAllWorkers.SelectedRows.Count > 0)
        {
            DataGridViewRow row = grdAllWorkers.SelectedRows[0];
            DataGridViewCell cell = row.Cells["LetterMessagePointer"];
            LetterMessage msg = (LetterMessage)cell.Value;
            MessageBox.Show(msg.ToString());
        }
    }

    void GrdEmailsSelectionChanged(object sender, System.EventArgs e)
    {
        if (grdEmails.SelectedRows.Count > 0)
        {
            DataGridViewRow row = grdEmails.SelectedRows[0];
            DataGridViewCell cell = row.Cells["LetterMessagePointer"];
            LetterMessage msg = (LetterMessage)cell.Value;
            DisplayEmail(msg);
        }
    }

    /// get the currently selected row, the appropriate ID, and scroll to the printed page
    void GrdLettersSelectionChanged(object sender, System.EventArgs e)
    {
        if (grdLetters.SelectedRows.Count > 0)
        {
            Int32 Id = Convert.ToInt32(grdLetters.SelectedRows[0].Cells["Id"].Value);
            preLetter.StartPage = Id - 1;
            RefreshPagePosition();
        }
    }

    /// select the appropriate row in the grid
    void SelectInGrid(Int32 AId)
    {
        foreach (DataGridViewRow rowview in grdLetters.Rows)
        {
            if (rowview.Cells["Id"].Value.ToString() == AId.ToString())
            {
                rowview.Cells["Id"].Selected = true;
            }
        }
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
            dlg.Document.Print();
        }
    }

    void TbbPrintClick(object sender, System.EventArgs e)
    {
        PrintDialog dlg = new PrintDialog();

        dlg.Document = FGfxPrinter.Document;
        dlg.AllowCurrentPage = true;
        dlg.AllowSomePages = true;

        if (dlg.ShowDialog() == DialogResult.OK)
        {
            dlg.Document.Print();
        }
    }
}
}