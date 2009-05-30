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
        this.grdEmails = new System.Windows.Forms.DataGridView();
        this.brwEmailContent = new System.Windows.Forms.WebBrowser();
        this.toolStrip1 = new System.Windows.Forms.ToolStrip();
        this.btnSendOneEmail = new System.Windows.Forms.ToolStripButton();
        this.btnSendAllEmails = new System.Windows.Forms.ToolStripButton();
        this.btnGenerateEmails = new System.Windows.Forms.ToolStripButton();
        ((System.ComponentModel.ISupportInitialize)(this.grdEmails)).BeginInit();
        this.toolStrip1.SuspendLayout();
        this.SuspendLayout();

        //
        // grdEmails
        //
        this.grdEmails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.grdEmails.Dock = System.Windows.Forms.DockStyle.Top;
        this.grdEmails.Location = new System.Drawing.Point(0, 25);
        this.grdEmails.Name = "grdEmails";
        this.grdEmails.Size = new System.Drawing.Size(568, 150);
        this.grdEmails.TabIndex = 3;
        this.grdEmails.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GrdEmailsCellEnter);

        //
        // brwEmailContent
        //
        this.brwEmailContent.Dock = System.Windows.Forms.DockStyle.Fill;
        this.brwEmailContent.Location = new System.Drawing.Point(0, 175);
        this.brwEmailContent.MinimumSize = new System.Drawing.Size(20, 20);
        this.brwEmailContent.Name = "brwEmailContent";
        this.brwEmailContent.Size = new System.Drawing.Size(568, 100);
        this.brwEmailContent.TabIndex = 4;

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
        this.toolStrip1.Size = new System.Drawing.Size(568, 25);
        this.toolStrip1.TabIndex = 5;
        this.toolStrip1.Text = "toolStrip1";

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
        // btnGenerateEmails
        //
        this.btnGenerateEmails.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnGenerateEmails.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerateEmails.Image")));
        this.btnGenerateEmails.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.btnGenerateEmails.Name = "btnGenerateEmails";
        this.btnGenerateEmails.Size = new System.Drawing.Size(88, 22);
        this.btnGenerateEmails.Text = "Generate Emails";
        this.btnGenerateEmails.Click += new System.EventHandler(this.BtnGenerateEmailsClick);

        //
        // MainForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(568, 275);
        this.Controls.Add(this.brwEmailContent);
        this.Controls.Add(this.grdEmails);
        this.Controls.Add(this.toolStrip1);
        this.Name = "MainForm";
        this.Text = "treasurerEmails";
        ((System.ComponentModel.ISupportInitialize)(this.grdEmails)).EndInit();
        this.toolStrip1.ResumeLayout(false);
        this.toolStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.ToolStripButton btnGenerateEmails;
    private System.Windows.Forms.ToolStripButton btnSendAllEmails;
    private System.Windows.Forms.ToolStripButton btnSendOneEmail;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.DataGridView grdEmails;
    private System.Windows.Forms.WebBrowser brwEmailContent;
}
}