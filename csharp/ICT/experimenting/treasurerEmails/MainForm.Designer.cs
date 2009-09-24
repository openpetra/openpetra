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
namespace treasurerEmails
{
partial class MainForm
{
    /// <summary>
    /// Designer variable used to keep track of non-visual components.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Disposes resources used by the form.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (components != null)
            {
                components.Dispose();
            }
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// This method is required for Windows Forms designer support.
    /// Do not change the method contents inside the source code editor. The Forms designer might
    /// not be able to load this method if it was changed manually.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        this.toolStrip1 = new System.Windows.Forms.ToolStrip();
        this.btnGenerateEmails = new System.Windows.Forms.ToolStripButton();
        this.btnSendOneEmail = new System.Windows.Forms.ToolStripButton();
        this.btnSendAllEmails = new System.Windows.Forms.ToolStripButton();
        this.tabOutput = new System.Windows.Forms.TabControl();
        this.tpgAllWorkers = new System.Windows.Forms.TabPage();
        this.grdAllWorkers = new System.Windows.Forms.DataGridView();
        this.tpgEmails = new System.Windows.Forms.TabPage();
        this.brwEmailContent = new System.Windows.Forms.WebBrowser();
        this.grdEmails = new System.Windows.Forms.DataGridView();
        this.tpgLetters = new System.Windows.Forms.TabPage();
        this.preLetter = new System.Windows.Forms.PrintPreviewControl();
        this.toolStrip2 = new System.Windows.Forms.ToolStrip();
        this.txtCurrentPage = new System.Windows.Forms.ToolStripTextBox();
        this.lblTotalNumberPages = new System.Windows.Forms.ToolStripLabel();
        this.tbbPrevPage = new System.Windows.Forms.ToolStripButton();
        this.tbbNextPage = new System.Windows.Forms.ToolStripButton();
        this.tbbPrintCurrentPage = new System.Windows.Forms.ToolStripButton();
        this.tbbPrint = new System.Windows.Forms.ToolStripButton();
        this.grdLetters = new System.Windows.Forms.DataGridView();
        this.tpgStatistics = new System.Windows.Forms.TabPage();
        this.txtWorkersWithoutTreasurer = new System.Windows.Forms.TextBox();
        this.label9 = new System.Windows.Forms.Label();
        this.txtWorkersWithTreasurer = new System.Windows.Forms.TextBox();
        this.label8 = new System.Windows.Forms.Label();
        this.txtTreasurerInvalidAddress = new System.Windows.Forms.TextBox();
        this.label7 = new System.Windows.Forms.Label();
        this.txtTreasurersLetter = new System.Windows.Forms.TextBox();
        this.label6 = new System.Windows.Forms.Label();
        this.txtTreasurersEmail = new System.Windows.Forms.TextBox();
        this.label5 = new System.Windows.Forms.Label();
        this.txtNumberOfUniqueTreasurers = new System.Windows.Forms.TextBox();
        this.label4 = new System.Windows.Forms.Label();
        this.txtNumberOfWorkersReceivingDonations = new System.Windows.Forms.TextBox();
        this.label3 = new System.Windows.Forms.Label();
        this.panel1 = new System.Windows.Forms.Panel();
        this.chkLettersOnly = new System.Windows.Forms.CheckBox();
        this.label2 = new System.Windows.Forms.Label();
        this.nudNumberMonths = new System.Windows.Forms.NumericUpDown();
        this.label1 = new System.Windows.Forms.Label();
        this.dtpLastMonth = new System.Windows.Forms.DateTimePicker();
        this.toolStrip1.SuspendLayout();
        this.tabOutput.SuspendLayout();
        this.tpgAllWorkers.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grdAllWorkers)).BeginInit();
        this.tpgEmails.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grdEmails)).BeginInit();
        this.tpgLetters.SuspendLayout();
        this.toolStrip2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grdLetters)).BeginInit();
        this.tpgStatistics.SuspendLayout();
        this.panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudNumberMonths)).BeginInit();
        this.SuspendLayout();

        //
        // toolStrip1
        //
        this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.btnGenerateEmails,
                this.btnSendOneEmail,
                this.btnSendAllEmails
            });
        this.toolStrip1.Location = new System.Drawing.Point(0, 0);
        this.toolStrip1.Name = "toolStrip1";
        this.toolStrip1.Size = new System.Drawing.Size(601, 25);
        this.toolStrip1.TabIndex = 5;
        this.toolStrip1.Text = "toolStrip1";

        //
        // btnGenerateEmails
        //
        this.btnGenerateEmails.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnGenerateEmails.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerateEmails.Image")));
        this.btnGenerateEmails.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnGenerateEmails.Name = "btnGenerateEmails";
        this.btnGenerateEmails.Size = new System.Drawing.Size(135, 22);
        this.btnGenerateEmails.Text = "Generate Emails && Letters";
        this.btnGenerateEmails.Click += new System.EventHandler(this.BtnGenerateEmailsClick);

        //
        // btnSendOneEmail
        //
        this.btnSendOneEmail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnSendOneEmail.Image = ((System.Drawing.Image)(resources.GetObject("btnSendOneEmail.Image")));
        this.btnSendOneEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnSendOneEmail.Name = "btnSendOneEmail";
        this.btnSendOneEmail.Size = new System.Drawing.Size(105, 22);
        this.btnSendOneEmail.Text = "Send selected Email";
        this.btnSendOneEmail.Click += new System.EventHandler(this.BtnSendOneEmailClick);

        //
        // btnSendAllEmails
        //
        this.btnSendAllEmails.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnSendAllEmails.Image = ((System.Drawing.Image)(resources.GetObject("btnSendAllEmails.Image")));
        this.btnSendAllEmails.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnSendAllEmails.Name = "btnSendAllEmails";
        this.btnSendAllEmails.Size = new System.Drawing.Size(80, 22);
        this.btnSendAllEmails.Text = "Send all Emails";
        this.btnSendAllEmails.Click += new System.EventHandler(this.BtnSendAllEmailsClick);

        //
        // tabOutput
        //
        this.tabOutput.Controls.Add(this.tpgAllWorkers);
        this.tabOutput.Controls.Add(this.tpgEmails);
        this.tabOutput.Controls.Add(this.tpgLetters);
        this.tabOutput.Controls.Add(this.tpgStatistics);
        this.tabOutput.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabOutput.Location = new System.Drawing.Point(0, 53);
        this.tabOutput.Name = "tabOutput";
        this.tabOutput.SelectedIndex = 0;
        this.tabOutput.Size = new System.Drawing.Size(601, 327);
        this.tabOutput.TabIndex = 0;

        //
        // tpgAllWorkers
        //
        this.tpgAllWorkers.Controls.Add(this.grdAllWorkers);
        this.tpgAllWorkers.Location = new System.Drawing.Point(4, 22);
        this.tpgAllWorkers.Name = "tpgAllWorkers";
        this.tpgAllWorkers.Padding = new System.Windows.Forms.Padding(3);
        this.tpgAllWorkers.Size = new System.Drawing.Size(593, 301);
        this.tpgAllWorkers.TabIndex = 4;
        this.tpgAllWorkers.Text = "All";
        this.tpgAllWorkers.UseVisualStyleBackColor = true;

        //
        // grdAllWorkers
        //
        this.grdAllWorkers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.grdAllWorkers.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grdAllWorkers.Location = new System.Drawing.Point(3, 3);
        this.grdAllWorkers.Name = "grdAllWorkers";
        this.grdAllWorkers.Size = new System.Drawing.Size(587, 295);
        this.grdAllWorkers.TabIndex = 6;

        //
        // tpgEmails
        //
        this.tpgEmails.Controls.Add(this.brwEmailContent);
        this.tpgEmails.Controls.Add(this.grdEmails);
        this.tpgEmails.Location = new System.Drawing.Point(4, 22);
        this.tpgEmails.Name = "tpgEmails";
        this.tpgEmails.Padding = new System.Windows.Forms.Padding(3);
        this.tpgEmails.Size = new System.Drawing.Size(593, 301);
        this.tpgEmails.TabIndex = 0;
        this.tpgEmails.Text = "Emails";
        this.tpgEmails.UseVisualStyleBackColor = true;

        //
        // brwEmailContent
        //
        this.brwEmailContent.Dock = System.Windows.Forms.DockStyle.Fill;
        this.brwEmailContent.Location = new System.Drawing.Point(3, 153);
        this.brwEmailContent.MinimumSize = new System.Drawing.Size(20, 20);
        this.brwEmailContent.Name = "brwEmailContent";
        this.brwEmailContent.Size = new System.Drawing.Size(587, 145);
        this.brwEmailContent.TabIndex = 6;

        //
        // grdEmails
        //
        this.grdEmails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.grdEmails.Dock = System.Windows.Forms.DockStyle.Top;
        this.grdEmails.Location = new System.Drawing.Point(3, 3);
        this.grdEmails.Name = "grdEmails";
        this.grdEmails.Size = new System.Drawing.Size(587, 150);
        this.grdEmails.TabIndex = 5;
        this.grdEmails.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GrdEmailsCellEnter);

        //
        // tpgLetters
        //
        this.tpgLetters.Controls.Add(this.preLetter);
        this.tpgLetters.Controls.Add(this.toolStrip2);
        this.tpgLetters.Controls.Add(this.grdLetters);
        this.tpgLetters.Location = new System.Drawing.Point(4, 22);
        this.tpgLetters.Name = "tpgLetters";
        this.tpgLetters.Padding = new System.Windows.Forms.Padding(3);
        this.tpgLetters.Size = new System.Drawing.Size(593, 301);
        this.tpgLetters.TabIndex = 1;
        this.tpgLetters.Text = "Letters";
        this.tpgLetters.UseVisualStyleBackColor = true;

        //
        // preLetter
        //
        this.preLetter.Dock = System.Windows.Forms.DockStyle.Fill;
        this.preLetter.Location = new System.Drawing.Point(3, 178);
        this.preLetter.Name = "preLetter";
        this.preLetter.Size = new System.Drawing.Size(587, 120);
        this.preLetter.TabIndex = 8;

        //
        // toolStrip2
        //
        this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.txtCurrentPage,
                this.lblTotalNumberPages,
                this.tbbPrevPage,
                this.tbbNextPage,
                this.tbbPrintCurrentPage,
                this.tbbPrint
            });
        this.toolStrip2.Location = new System.Drawing.Point(3, 153);
        this.toolStrip2.Name = "toolStrip2";
        this.toolStrip2.Size = new System.Drawing.Size(587, 25);
        this.toolStrip2.TabIndex = 9;
        this.toolStrip2.Text = "toolStrip2";

        //
        // txtCurrentPage
        //
        this.txtCurrentPage.Name = "txtCurrentPage";
        this.txtCurrentPage.Size = new System.Drawing.Size(50, 25);
        this.txtCurrentPage.TextChanged += new System.EventHandler(this.TxtCurrentPageTextChanged);

        //
        // lblTotalNumberPages
        //
        this.lblTotalNumberPages.Name = "lblTotalNumberPages";
        this.lblTotalNumberPages.Size = new System.Drawing.Size(97, 22);
        this.lblTotalNumberPages.Text = "#Number of pages";

        //
        // tbbPrevPage
        //
        this.tbbPrevPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.tbbPrevPage.Image = ((System.Drawing.Image)(resources.GetObject("tbbPrevPage.Image")));
        this.tbbPrevPage.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.tbbPrevPage.Name = "tbbPrevPage";
        this.tbbPrevPage.Size = new System.Drawing.Size(79, 22);
        this.tbbPrevPage.Text = "Previous Page";
        this.tbbPrevPage.Click += new System.EventHandler(this.TbbPrevPageClick);

        //
        // tbbNextPage
        //
        this.tbbNextPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.tbbNextPage.Image = ((System.Drawing.Image)(resources.GetObject("tbbNextPage.Image")));
        this.tbbNextPage.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.tbbNextPage.Name = "tbbNextPage";
        this.tbbNextPage.Size = new System.Drawing.Size(61, 22);
        this.tbbNextPage.Text = "Next Page";
        this.tbbNextPage.Click += new System.EventHandler(this.TbbNextPageClick);

        //
        // tbbPrintCurrentPage
        //
        this.tbbPrintCurrentPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.tbbPrintCurrentPage.Image = ((System.Drawing.Image)(resources.GetObject("tbbPrintCurrentPage.Image")));
        this.tbbPrintCurrentPage.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.tbbPrintCurrentPage.Name = "tbbPrintCurrentPage";
        this.tbbPrintCurrentPage.Size = new System.Drawing.Size(100, 22);
        this.tbbPrintCurrentPage.Text = "Print Current Page";
        this.tbbPrintCurrentPage.Click += new System.EventHandler(this.TbbPrintCurrentPageClick);

        //
        // tbbPrint
        //
        this.tbbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.tbbPrint.Image = ((System.Drawing.Image)(resources.GetObject("tbbPrint.Image")));
        this.tbbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.tbbPrint.Name = "tbbPrint";
        this.tbbPrint.Size = new System.Drawing.Size(33, 22);
        this.tbbPrint.Text = "Print";
        this.tbbPrint.Click += new System.EventHandler(this.TbbPrintClick);

        //
        // grdLetters
        //
        this.grdLetters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.grdLetters.Dock = System.Windows.Forms.DockStyle.Top;
        this.grdLetters.Location = new System.Drawing.Point(3, 3);
        this.grdLetters.Name = "grdLetters";
        this.grdLetters.Size = new System.Drawing.Size(587, 150);
        this.grdLetters.TabIndex = 7;
        this.grdLetters.SelectionChanged += new System.EventHandler(this.GrdLettersSelectionChanged);

        //
        // tpgStatistics
        //
        this.tpgStatistics.Controls.Add(this.txtWorkersWithoutTreasurer);
        this.tpgStatistics.Controls.Add(this.label9);
        this.tpgStatistics.Controls.Add(this.txtWorkersWithTreasurer);
        this.tpgStatistics.Controls.Add(this.label8);
        this.tpgStatistics.Controls.Add(this.txtTreasurerInvalidAddress);
        this.tpgStatistics.Controls.Add(this.label7);
        this.tpgStatistics.Controls.Add(this.txtTreasurersLetter);
        this.tpgStatistics.Controls.Add(this.label6);
        this.tpgStatistics.Controls.Add(this.txtTreasurersEmail);
        this.tpgStatistics.Controls.Add(this.label5);
        this.tpgStatistics.Controls.Add(this.txtNumberOfUniqueTreasurers);
        this.tpgStatistics.Controls.Add(this.label4);
        this.tpgStatistics.Controls.Add(this.txtNumberOfWorkersReceivingDonations);
        this.tpgStatistics.Controls.Add(this.label3);
        this.tpgStatistics.Location = new System.Drawing.Point(4, 22);
        this.tpgStatistics.Name = "tpgStatistics";
        this.tpgStatistics.Padding = new System.Windows.Forms.Padding(3);
        this.tpgStatistics.Size = new System.Drawing.Size(593, 301);
        this.tpgStatistics.TabIndex = 3;
        this.tpgStatistics.Text = "Statistics";
        this.tpgStatistics.UseVisualStyleBackColor = true;

        //
        // txtWorkersWithoutTreasurer
        //
        this.txtWorkersWithoutTreasurer.Location = new System.Drawing.Point(24, 77);
        this.txtWorkersWithoutTreasurer.Name = "txtWorkersWithoutTreasurer";
        this.txtWorkersWithoutTreasurer.ReadOnly = true;
        this.txtWorkersWithoutTreasurer.Size = new System.Drawing.Size(100, 20);
        this.txtWorkersWithoutTreasurer.TabIndex = 13;

        //
        // label9
        //
        this.label9.Location = new System.Drawing.Point(130, 80);
        this.label9.Name = "label9";
        this.label9.Size = new System.Drawing.Size(364, 23);
        this.label9.TabIndex = 12;
        this.label9.Text = "workers have no treasurer";

        //
        // txtWorkersWithTreasurer
        //
        this.txtWorkersWithTreasurer.Location = new System.Drawing.Point(24, 50);
        this.txtWorkersWithTreasurer.Name = "txtWorkersWithTreasurer";
        this.txtWorkersWithTreasurer.ReadOnly = true;
        this.txtWorkersWithTreasurer.Size = new System.Drawing.Size(100, 20);
        this.txtWorkersWithTreasurer.TabIndex = 11;

        //
        // label8
        //
        this.label8.Location = new System.Drawing.Point(130, 53);
        this.label8.Name = "label8";
        this.label8.Size = new System.Drawing.Size(364, 23);
        this.label8.TabIndex = 10;
        this.label8.Text = "workers have at least one treasurer";

        //
        // txtTreasurerInvalidAddress
        //
        this.txtTreasurerInvalidAddress.Location = new System.Drawing.Point(24, 208);
        this.txtTreasurerInvalidAddress.Name = "txtTreasurerInvalidAddress";
        this.txtTreasurerInvalidAddress.ReadOnly = true;
        this.txtTreasurerInvalidAddress.Size = new System.Drawing.Size(100, 20);
        this.txtTreasurerInvalidAddress.TabIndex = 9;

        //
        // label7
        //
        this.label7.Location = new System.Drawing.Point(130, 211);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(364, 23);
        this.label7.TabIndex = 8;
        this.label7.Text = "treasurers have invalid address details";

        //
        // txtTreasurersLetter
        //
        this.txtTreasurersLetter.Location = new System.Drawing.Point(24, 182);
        this.txtTreasurersLetter.Name = "txtTreasurersLetter";
        this.txtTreasurersLetter.ReadOnly = true;
        this.txtTreasurersLetter.Size = new System.Drawing.Size(100, 20);
        this.txtTreasurersLetter.TabIndex = 7;

        //
        // label6
        //
        this.label6.Location = new System.Drawing.Point(130, 185);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(364, 23);
        this.label6.TabIndex = 6;
        this.label6.Text = "treasurers receive their update via printed letter";

        //
        // txtTreasurersEmail
        //
        this.txtTreasurersEmail.Location = new System.Drawing.Point(24, 156);
        this.txtTreasurersEmail.Name = "txtTreasurersEmail";
        this.txtTreasurersEmail.ReadOnly = true;
        this.txtTreasurersEmail.Size = new System.Drawing.Size(100, 20);
        this.txtTreasurersEmail.TabIndex = 5;

        //
        // label5
        //
        this.label5.Location = new System.Drawing.Point(130, 159);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(364, 23);
        this.label5.TabIndex = 4;
        this.label5.Text = "treasurers receive their update via email";

        //
        // txtNumberOfUniqueTreasurers
        //
        this.txtNumberOfUniqueTreasurers.Location = new System.Drawing.Point(24, 130);
        this.txtNumberOfUniqueTreasurers.Name = "txtNumberOfUniqueTreasurers";
        this.txtNumberOfUniqueTreasurers.ReadOnly = true;
        this.txtNumberOfUniqueTreasurers.Size = new System.Drawing.Size(100, 20);
        this.txtNumberOfUniqueTreasurers.TabIndex = 3;

        //
        // label4
        //
        this.label4.Location = new System.Drawing.Point(130, 133);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(364, 23);
        this.label4.TabIndex = 2;
        this.label4.Text = "unique treasurers";

        //
        // txtNumberOfWorkersReceivingDonations
        //
        this.txtNumberOfWorkersReceivingDonations.Location = new System.Drawing.Point(24, 21);
        this.txtNumberOfWorkersReceivingDonations.Name = "txtNumberOfWorkersReceivingDonations";
        this.txtNumberOfWorkersReceivingDonations.ReadOnly = true;
        this.txtNumberOfWorkersReceivingDonations.Size = new System.Drawing.Size(100, 20);
        this.txtNumberOfWorkersReceivingDonations.TabIndex = 1;

        //
        // label3
        //
        this.label3.Location = new System.Drawing.Point(130, 24);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(364, 23);
        this.label3.TabIndex = 0;
        this.label3.Text = "workers have received gifts in the last months";

        //
        // panel1
        //
        this.panel1.Controls.Add(this.chkLettersOnly);
        this.panel1.Controls.Add(this.label2);
        this.panel1.Controls.Add(this.nudNumberMonths);
        this.panel1.Controls.Add(this.label1);
        this.panel1.Controls.Add(this.dtpLastMonth);
        this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
        this.panel1.Location = new System.Drawing.Point(0, 25);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(601, 28);
        this.panel1.TabIndex = 8;

        //
        // chkLettersOnly
        //
        this.chkLettersOnly.Location = new System.Drawing.Point(428, 2);
        this.chkLettersOnly.Name = "chkLettersOnly";
        this.chkLettersOnly.Size = new System.Drawing.Size(166, 24);
        this.chkLettersOnly.TabIndex = 12;
        this.chkLettersOnly.Text = "no emails, just letters";
        this.chkLettersOnly.UseVisualStyleBackColor = true;

        //
        // label2
        //
        this.label2.Location = new System.Drawing.Point(134, 7);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(153, 23);
        this.label2.TabIndex = 11;
        this.label2.Text = "months before and including ";

        //
        // nudNumberMonths
        //
        this.nudNumberMonths.Location = new System.Drawing.Point(79, 5);
        this.nudNumberMonths.Name = "nudNumberMonths";
        this.nudNumberMonths.Size = new System.Drawing.Size(49, 20);
        this.nudNumberMonths.TabIndex = 10;
        this.nudNumberMonths.Value = new decimal(new int[] {
                14,
                0,
                0,
                0
            });

        //
        // label1
        //
        this.label1.Location = new System.Drawing.Point(9, 7);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(100, 23);
        this.label1.TabIndex = 9;
        this.label1.Text = "for the last ";

        //
        // dtpLastMonth
        //
        this.dtpLastMonth.CustomFormat = "MMM yyyy";
        this.dtpLastMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
        this.dtpLastMonth.Location = new System.Drawing.Point(293, 3);
        this.dtpLastMonth.Name = "dtpLastMonth";
        this.dtpLastMonth.Size = new System.Drawing.Size(83, 20);
        this.dtpLastMonth.TabIndex = 8;

        //
        // MainForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(601, 380);
        this.Controls.Add(this.tabOutput);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this.toolStrip1);
        this.Name = "MainForm";
        this.Text = "treasurerEmails";
        this.toolStrip1.ResumeLayout(false);
        this.toolStrip1.PerformLayout();
        this.tabOutput.ResumeLayout(false);
        this.tpgAllWorkers.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grdAllWorkers)).EndInit();
        this.tpgEmails.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.grdEmails)).EndInit();
        this.tpgLetters.ResumeLayout(false);
        this.tpgLetters.PerformLayout();
        this.toolStrip2.ResumeLayout(false);
        this.toolStrip2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.grdLetters)).EndInit();
        this.tpgStatistics.ResumeLayout(false);
        this.tpgStatistics.PerformLayout();
        this.panel1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.nudNumberMonths)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.DataGridView grdAllWorkers;
    private System.Windows.Forms.TabPage tpgAllWorkers;
    private System.Windows.Forms.TextBox txtTreasurersEmail;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox txtTreasurersLetter;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox txtTreasurerInvalidAddress;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox txtWorkersWithTreasurer;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.TextBox txtWorkersWithoutTreasurer;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtNumberOfWorkersReceivingDonations;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox txtNumberOfUniqueTreasurers;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TabPage tpgStatistics;

    private System.Windows.Forms.ToolStripTextBox txtCurrentPage;
    private System.Windows.Forms.ToolStripButton tbbPrint;
    private System.Windows.Forms.ToolStripButton tbbPrintCurrentPage;
    private System.Windows.Forms.ToolStripLabel lblTotalNumberPages;
    private System.Windows.Forms.ToolStripButton tbbNextPage;
    private System.Windows.Forms.ToolStripButton tbbPrevPage;
    private System.Windows.Forms.ToolStrip toolStrip2;

    private System.Windows.Forms.PrintPreviewControl preLetter;
    private System.Windows.Forms.CheckBox chkLettersOnly;

    private System.Windows.Forms.DateTimePicker dtpLastMonth;
    private System.Windows.Forms.NumericUpDown nudNumberMonths;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.DataGridView grdLetters;
    private System.Windows.Forms.TabPage tpgLetters;
    private System.Windows.Forms.TabPage tpgEmails;
    private System.Windows.Forms.TabControl tabOutput;

    private System.Windows.Forms.ToolStripButton btnGenerateEmails;
    private System.Windows.Forms.ToolStripButton btnSendAllEmails;
    private System.Windows.Forms.ToolStripButton btnSendOneEmail;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.DataGridView grdEmails;
    private System.Windows.Forms.WebBrowser brwEmailContent;
}
}