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
    	this.tbbPrintSelectedLetters = new System.Windows.Forms.ToolStripButton();
    	this.tbbPrintAllLetters = new System.Windows.Forms.ToolStripButton();
    	this.tabOutput = new System.Windows.Forms.TabControl();
    	this.tpgEmails = new System.Windows.Forms.TabPage();
    	this.brwEmailContent = new System.Windows.Forms.WebBrowser();
    	this.grdEmails = new System.Windows.Forms.DataGridView();
    	this.tpgLetters = new System.Windows.Forms.TabPage();
    	this.brwLetterContent = new System.Windows.Forms.WebBrowser();
    	this.grdLetters = new System.Windows.Forms.DataGridView();
    	this.panel1 = new System.Windows.Forms.Panel();
    	this.label2 = new System.Windows.Forms.Label();
    	this.nudNumberMonths = new System.Windows.Forms.NumericUpDown();
    	this.label1 = new System.Windows.Forms.Label();
    	this.dtpLastMonth = new System.Windows.Forms.DateTimePicker();
    	this.chkLettersOnly = new System.Windows.Forms.CheckBox();
    	this.toolStrip1.SuspendLayout();
    	this.tabOutput.SuspendLayout();
    	this.tpgEmails.SuspendLayout();
    	((System.ComponentModel.ISupportInitialize)(this.grdEmails)).BeginInit();
    	this.tpgLetters.SuspendLayout();
    	((System.ComponentModel.ISupportInitialize)(this.grdLetters)).BeginInit();
    	this.panel1.SuspendLayout();
    	((System.ComponentModel.ISupportInitialize)(this.nudNumberMonths)).BeginInit();
    	this.SuspendLayout();
    	// 
    	// toolStrip1
    	// 
    	this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
    	    	    	this.btnGenerateEmails,
    	    	    	this.btnSendOneEmail,
    	    	    	this.btnSendAllEmails,
    	    	    	this.tbbPrintSelectedLetters,
    	    	    	this.tbbPrintAllLetters});
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
    	// tbbPrintSelectedLetters
    	// 
    	this.tbbPrintSelectedLetters.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
    	this.tbbPrintSelectedLetters.Image = ((System.Drawing.Image)(resources.GetObject("tbbPrintSelectedLetters.Image")));
    	this.tbbPrintSelectedLetters.ImageTransparentColor = System.Drawing.Color.Magenta;
    	this.tbbPrintSelectedLetters.Name = "tbbPrintSelectedLetters";
    	this.tbbPrintSelectedLetters.Size = new System.Drawing.Size(113, 22);
    	this.tbbPrintSelectedLetters.Text = "Print selected Letters";
    	// 
    	// tbbPrintAllLetters
    	// 
    	this.tbbPrintAllLetters.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
    	this.tbbPrintAllLetters.Image = ((System.Drawing.Image)(resources.GetObject("tbbPrintAllLetters.Image")));
    	this.tbbPrintAllLetters.ImageTransparentColor = System.Drawing.Color.Magenta;
    	this.tbbPrintAllLetters.Name = "tbbPrintAllLetters";
    	this.tbbPrintAllLetters.Size = new System.Drawing.Size(83, 22);
    	this.tbbPrintAllLetters.Text = "Print all Letters";
    	// 
    	// tabOutput
    	// 
    	this.tabOutput.Controls.Add(this.tpgEmails);
    	this.tabOutput.Controls.Add(this.tpgLetters);
    	this.tabOutput.Dock = System.Windows.Forms.DockStyle.Fill;
    	this.tabOutput.Location = new System.Drawing.Point(0, 53);
    	this.tabOutput.Name = "tabOutput";
    	this.tabOutput.SelectedIndex = 0;
    	this.tabOutput.Size = new System.Drawing.Size(601, 327);
    	this.tabOutput.TabIndex = 6;
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
    	this.tpgLetters.Controls.Add(this.brwLetterContent);
    	this.tpgLetters.Controls.Add(this.grdLetters);
    	this.tpgLetters.Location = new System.Drawing.Point(4, 22);
    	this.tpgLetters.Name = "tpgLetters";
    	this.tpgLetters.Padding = new System.Windows.Forms.Padding(3);
    	this.tpgLetters.Size = new System.Drawing.Size(593, 301);
    	this.tpgLetters.TabIndex = 1;
    	this.tpgLetters.Text = "Letters";
    	this.tpgLetters.UseVisualStyleBackColor = true;
    	// 
    	// brwLetterContent
    	// 
    	this.brwLetterContent.Dock = System.Windows.Forms.DockStyle.Fill;
    	this.brwLetterContent.Location = new System.Drawing.Point(3, 153);
    	this.brwLetterContent.MinimumSize = new System.Drawing.Size(20, 20);
    	this.brwLetterContent.Name = "brwLetterContent";
    	this.brwLetterContent.Size = new System.Drawing.Size(587, 145);
    	this.brwLetterContent.TabIndex = 8;
    	// 
    	// grdLetters
    	// 
    	this.grdLetters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    	this.grdLetters.Dock = System.Windows.Forms.DockStyle.Top;
    	this.grdLetters.Location = new System.Drawing.Point(3, 3);
    	this.grdLetters.Name = "grdLetters";
    	this.grdLetters.Size = new System.Drawing.Size(587, 150);
    	this.grdLetters.TabIndex = 7;
    	this.grdLetters.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GrdLettersCellEnter);
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
    	    	    	0});
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
    	// chkLettersOnly
    	// 
    	this.chkLettersOnly.Checked = true;
    	this.chkLettersOnly.CheckState = System.Windows.Forms.CheckState.Checked;
    	this.chkLettersOnly.Location = new System.Drawing.Point(428, 2);
    	this.chkLettersOnly.Name = "chkLettersOnly";
    	this.chkLettersOnly.Size = new System.Drawing.Size(166, 24);
    	this.chkLettersOnly.TabIndex = 12;
    	this.chkLettersOnly.Text = "no emails, just letters";
    	this.chkLettersOnly.UseVisualStyleBackColor = true;
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
    	this.tpgEmails.ResumeLayout(false);
    	((System.ComponentModel.ISupportInitialize)(this.grdEmails)).EndInit();
    	this.tpgLetters.ResumeLayout(false);
    	((System.ComponentModel.ISupportInitialize)(this.grdLetters)).EndInit();
    	this.panel1.ResumeLayout(false);
    	((System.ComponentModel.ISupportInitialize)(this.nudNumberMonths)).EndInit();
    	this.ResumeLayout(false);
    	this.PerformLayout();
    }
    private System.Windows.Forms.CheckBox chkLettersOnly;

    private System.Windows.Forms.DateTimePicker dtpLastMonth;
    private System.Windows.Forms.NumericUpDown nudNumberMonths;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.DataGridView grdLetters;
    private System.Windows.Forms.WebBrowser brwLetterContent;
    private System.Windows.Forms.ToolStripButton tbbPrintAllLetters;
    private System.Windows.Forms.ToolStripButton tbbPrintSelectedLetters;
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