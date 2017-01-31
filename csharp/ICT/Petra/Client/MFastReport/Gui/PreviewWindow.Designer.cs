//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       markusm, timop
//
// Copyright 2004-2010 by OM International
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
using Ict.Common.Controls;
using System.Windows.Forms;

namespace Ict.Petra.Client.MFastReport.Gui
{
    partial class TFrmPreviewWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmPreviewWindow));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbbPrint = new System.Windows.Forms.ToolStripButton();
            this.tbbSave = new System.Windows.Forms.ToolStripDropDownButton();
            this.tbbEmail = new System.Windows.Forms.ToolStripButton();
            this.tbbFind = new System.Windows.Forms.ToolStripButton();
            this.tbbFirst = new System.Windows.Forms.ToolStripButton();
            this.tbbPrev = new System.Windows.Forms.ToolStripButton();
            this.tbtPageNo = new System.Windows.Forms.ToolStripTextBox();
            this.tblOfPages = new System.Windows.Forms.ToolStripLabel();
            this.tbbNext = new System.Windows.Forms.ToolStripButton();
            this.tbbLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbClose = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            // toolStrip1
            //
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.tbbPrint,
                    this.tbbSave,
                    this.tbbEmail,
                    this.tbbFind,
                    this.tbbFirst,
                    this.tbbPrev,
                    this.tbtPageNo,
                    this.tblOfPages,
                    this.tbbNext,
                    this.tbbLast,
                    this.toolStripSeparator1,
                    this.tbbClose
                });
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(841, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            //
            // tbbPrint
            //
            this.tbbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbPrint.Name = "tbbPrint";
            this.tbbPrint.Size = new System.Drawing.Size(36, 22);
            this.tbbPrint.Text = "Print";
            this.tbbPrint.ToolTipText = "Print (Ctrl-P)";
            this.tbbPrint.Click += new System.EventHandler(this.tbbPrint_Click);
            //
            // tbbSave
            //
            this.tbbSave.Image = ((System.Drawing.Image)(resources.GetObject("tbbSave.Image")));
            this.tbbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbSave.Name = "tbbSave";
            this.tbbSave.Size = new System.Drawing.Size(60, 22);
            this.tbbSave.Text = "Save";
            this.tbbSave.ToolTipText = "Save (Ctrl-S)";
            //
            // tbbEmail
            //
            this.tbbEmail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbEmail.Image = ((System.Drawing.Image)(resources.GetObject("tbbEmail.Image")));
            this.tbbEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbEmail.Name = "tbbEmail";
            this.tbbEmail.Size = new System.Drawing.Size(23, 22);
            this.tbbEmail.Text = "E-mail";
            this.tbbEmail.ToolTipText = "E-mail (Ctrl-M)";
            this.tbbEmail.Click += new System.EventHandler(this.tbbEmail_Click);
            //
            // tbbFind
            //
            this.tbbFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbFind.Image = ((System.Drawing.Image)(resources.GetObject("tbbFind.Image")));
            this.tbbFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbFind.Name = "tbbFind";
            this.tbbFind.Size = new System.Drawing.Size(23, 22);
            this.tbbFind.Text = "Find";
            this.tbbFind.ToolTipText = "Find (Ctrl-F)";
            this.tbbFind.Click += new System.EventHandler(this.tbbFind_Click);
            //
            // tbbFirst
            //
            this.tbbFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbFirst.Image = ((System.Drawing.Image)(resources.GetObject("tbbFirst.Image")));
            this.tbbFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbFirst.Name = "tbbFirst";
            this.tbbFirst.Size = new System.Drawing.Size(23, 22);
            this.tbbFirst.Text = "First";
            this.tbbFirst.Click += new System.EventHandler(this.tbbFirst_Click);
            //
            // tbbPrev
            //
            this.tbbPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbPrev.Image = ((System.Drawing.Image)(resources.GetObject("tbbPrev.Image")));
            this.tbbPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbPrev.Name = "tbbPrev";
            this.tbbPrev.Size = new System.Drawing.Size(23, 22);
            this.tbbPrev.Text = "Prev";
            this.tbbPrev.Click += new System.EventHandler(this.tbbPrev_Click);
            //
            // tbtPageNo
            //
            this.tbtPageNo.Name = "tbtPageNo";
            this.tbtPageNo.Size = new System.Drawing.Size(50, 25);
            this.tbtPageNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbtPageNo_KeyDown);
            //
            // tblOfPages
            //
            this.tblOfPages.Name = "tblOfPages";
            this.tblOfPages.Size = new System.Drawing.Size(21, 22);
            this.tblOfPages.Text = "of ";
            //
            // tbbNext
            //
            this.tbbNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbNext.Image = ((System.Drawing.Image)(resources.GetObject("tbbNext.Image")));
            this.tbbNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbNext.Name = "tbbNext";
            this.tbbNext.Size = new System.Drawing.Size(23, 22);
            this.tbbNext.Text = "Next";
            this.tbbNext.Click += new System.EventHandler(this.tbbNext_Click);
            //
            // tbbLast
            //
            this.tbbLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbbLast.Image = ((System.Drawing.Image)(resources.GetObject("tbbLast.Image")));
            this.tbbLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbLast.Name = "tbbLast";
            this.tbbLast.Size = new System.Drawing.Size(23, 22);
            this.tbbLast.Text = "Last";
            this.tbbLast.Click += new System.EventHandler(this.tbbLast_Click);
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            //
            // tbbClose
            //
            this.tbbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbbClose.Image = ((System.Drawing.Image)(resources.GetObject("tbbClose.Image")));
            this.tbbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbClose.Name = "tbbClose";
            this.tbbClose.Size = new System.Drawing.Size(40, 22);
            this.tbbClose.Text = "&Close";
            this.tbbClose.Click += new System.EventHandler(this.tbbClose_Click);
            //
            // TFrmPreviewWindow
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 461);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TFrmPreviewWindow";
            this.Text = "Report Preview";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.TFrmPreviewWindow_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tbbClose;
        private System.Windows.Forms.ToolStripButton tbbPrint;
        private System.Windows.Forms.ToolStripDropDownButton tbbSave;
        private System.Windows.Forms.ToolStripButton tbbEmail;
        private System.Windows.Forms.ToolStripButton tbbFind;
        private System.Windows.Forms.ToolStripButton tbbFirst;
        private System.Windows.Forms.ToolStripButton tbbPrev;
        private System.Windows.Forms.ToolStripTextBox tbtPageNo;
        private System.Windows.Forms.ToolStripLabel tblOfPages;
        private System.Windows.Forms.ToolStripButton tbbNext;
        private System.Windows.Forms.ToolStripButton tbbLast;
    }
}