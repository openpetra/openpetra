//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Bernardo Castilho
//
// Copyright 2009-2014 by Bernardo Castilho
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

namespace Ict.Common.Printing
{
    public partial class CoolPrintPreviewDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoolPrintPreviewDialog));
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnPrint = new System.Windows.Forms.ToolStripButton();
            this._btnPageSetup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._btnZoom = new System.Windows.Forms.ToolStripSplitButton();
            this._itemActualSize = new System.Windows.Forms.ToolStripMenuItem();
            this._itemFullPage = new System.Windows.Forms.ToolStripMenuItem();
            this._itemPageWidth = new System.Windows.Forms.ToolStripMenuItem();
            this._itemTwoPages = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this._item500 = new System.Windows.Forms.ToolStripMenuItem();
            this._item200 = new System.Windows.Forms.ToolStripMenuItem();
            this._item150 = new System.Windows.Forms.ToolStripMenuItem();
            this._item100 = new System.Windows.Forms.ToolStripMenuItem();
            this._item75 = new System.Windows.Forms.ToolStripMenuItem();
            this._item50 = new System.Windows.Forms.ToolStripMenuItem();
            this._item25 = new System.Windows.Forms.ToolStripMenuItem();
            this._item10 = new System.Windows.Forms.ToolStripMenuItem();
            this._btnFirst = new System.Windows.Forms.ToolStripButton();
            this._btnPrev = new System.Windows.Forms.ToolStripButton();
            this._txtStartPage = new System.Windows.Forms.ToolStripTextBox();
            this._lblPageCount = new System.Windows.Forms.ToolStripLabel();
            this._btnNext = new System.Windows.Forms.ToolStripButton();
            this._btnLast = new System.Windows.Forms.ToolStripButton();
            this._separator = new System.Windows.Forms.ToolStripSeparator();
            this._btnCancel = new System.Windows.Forms.ToolStripButton();
            this._preview = new CoolPrintPreviewControl();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnPrint,
            this._btnPageSetup,
            this.toolStripSeparator2,
            this._btnZoom,
            this._btnFirst,
            this._btnPrev,
            this._txtStartPage,
            this._lblPageCount,
            this._btnNext,
            this._btnLast,
            this._separator,
            this._btnCancel});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(532, 25);
            this._toolStrip.TabIndex = 0;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _btnPrint
            // 
            this._btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("_btnPrint.Image")));
            this._btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnPrint.Name = "_btnPrint";
            this._btnPrint.Size = new System.Drawing.Size(23, 22);
            this._btnPrint.Text = "Print Document";
            this._btnPrint.Click += new System.EventHandler(this._btnPrint_Click);
            // 
            // _btnPageSetup
            // 
            this._btnPageSetup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPageSetup.Image = ((System.Drawing.Image)(resources.GetObject("_btnPageSetup.Image")));
            this._btnPageSetup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnPageSetup.Name = "_btnPageSetup";
            this._btnPageSetup.Size = new System.Drawing.Size(23, 23);
            this._btnPageSetup.Text = "Page Setup";
            this._btnPageSetup.Click += new System.EventHandler(this._btnPageSetup_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 26);
            // 
            // _btnZoom
            // 
            this._btnZoom.AutoToolTip = false;
            this._btnZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._itemActualSize,
            this._itemFullPage,
            this._itemPageWidth,
            this._itemTwoPages,
            this.toolStripMenuItem1,
            this._item500,
            this._item200,
            this._item150,
            this._item100,
            this._item75,
            this._item50,
            this._item25,
            this._item10});
            this._btnZoom.Image = ((System.Drawing.Image)(resources.GetObject("_btnZoom.Image")));
            this._btnZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnZoom.Name = "_btnZoom";
            this._btnZoom.Size = new System.Drawing.Size(77, 23);
            this._btnZoom.Text = "&Zoom";
            this._btnZoom.ButtonClick += new System.EventHandler(this._btnZoom_ButtonClick);
            this._btnZoom.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this._btnZoom_DropDownItemClicked);
            // 
            // _itemActualSize
            // 
            this._itemActualSize.Image = ((System.Drawing.Image)(resources.GetObject("_itemActualSize.Image")));
            this._itemActualSize.Name = "_itemActualSize";
            this._itemActualSize.Size = new System.Drawing.Size(150, 22);
            this._itemActualSize.Text = "Actual Size";
            // 
            // _itemFullPage
            // 
            this._itemFullPage.Image = ((System.Drawing.Image)(resources.GetObject("_itemFullPage.Image")));
            this._itemFullPage.Name = "_itemFullPage";
            this._itemFullPage.Size = new System.Drawing.Size(150, 22);
            this._itemFullPage.Text = "Full Page";
            // 
            // _itemPageWidth
            // 
            this._itemPageWidth.Image = ((System.Drawing.Image)(resources.GetObject("_itemPageWidth.Image")));
            this._itemPageWidth.Name = "_itemPageWidth";
            this._itemPageWidth.Size = new System.Drawing.Size(150, 22);
            this._itemPageWidth.Text = "Page Width";
            // 
            // _itemTwoPages
            // 
            this._itemTwoPages.Image = ((System.Drawing.Image)(resources.GetObject("_itemTwoPages.Image")));
            this._itemTwoPages.Name = "_itemTwoPages";
            this._itemTwoPages.Size = new System.Drawing.Size(150, 22);
            this._itemTwoPages.Text = "Two Pages";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(147, 6);
            // 
            // _item500
            // 
            this._item500.Name = "_item500";
            this._item500.Size = new System.Drawing.Size(150, 22);
            this._item500.Text = "500%";
            // 
            // _item200
            // 
            this._item200.Name = "_item200";
            this._item200.Size = new System.Drawing.Size(150, 22);
            this._item200.Text = "200%";
            // 
            // _item150
            // 
            this._item150.Name = "_item150";
            this._item150.Size = new System.Drawing.Size(150, 22);
            this._item150.Text = "150%";
            // 
            // _item100
            // 
            this._item100.Name = "_item100";
            this._item100.Size = new System.Drawing.Size(150, 22);
            this._item100.Text = "100%";
            // 
            // _item75
            // 
            this._item75.Name = "_item75";
            this._item75.Size = new System.Drawing.Size(150, 22);
            this._item75.Text = "75%";
            // 
            // _item50
            // 
            this._item50.Name = "_item50";
            this._item50.Size = new System.Drawing.Size(150, 22);
            this._item50.Text = "50%";
            // 
            // _item25
            // 
            this._item25.Name = "_item25";
            this._item25.Size = new System.Drawing.Size(150, 22);
            this._item25.Text = "25%";
            // 
            // _item10
            // 
            this._item10.Name = "_item10";
            this._item10.Size = new System.Drawing.Size(150, 22);
            this._item10.Text = "10%";
            // 
            // _btnFirst
            // 
            this._btnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnFirst.Image = ((System.Drawing.Image)(resources.GetObject("_btnFirst.Image")));
            this._btnFirst.ImageTransparentColor = System.Drawing.Color.Red;
            this._btnFirst.Name = "_btnFirst";
            this._btnFirst.Size = new System.Drawing.Size(23, 23);
            this._btnFirst.Text = "First Page";
            this._btnFirst.Click += new System.EventHandler(this._btnFirst_Click);
            // 
            // _btnPrev
            // 
            this._btnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnPrev.Image = ((System.Drawing.Image)(resources.GetObject("_btnPrev.Image")));
            this._btnPrev.ImageTransparentColor = System.Drawing.Color.Red;
            this._btnPrev.Name = "_btnPrev";
            this._btnPrev.Size = new System.Drawing.Size(23, 23);
            this._btnPrev.Text = "Previous Page";
            this._btnPrev.Click += new System.EventHandler(this._btnPrev_Click);
            // 
            // _txtStartPage
            // 
            this._txtStartPage.AutoSize = false;
            this._txtStartPage.Name = "_txtStartPage";
            this._txtStartPage.Size = new System.Drawing.Size(39, 24);
            this._txtStartPage.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._txtStartPage.Validating += new System.ComponentModel.CancelEventHandler(this._txtStartPage_Validating);
            this._txtStartPage.Enter += new System.EventHandler(this._txtStartPage_Enter);
            this._txtStartPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._txtStartPage_KeyPress);
            // 
            // _lblPageCount
            // 
            this._lblPageCount.Name = "_lblPageCount";
            this._lblPageCount.Size = new System.Drawing.Size(13, 23);
            this._lblPageCount.Text = " ";
            // 
            // _btnNext
            // 
            this._btnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnNext.Image = ((System.Drawing.Image)(resources.GetObject("_btnNext.Image")));
            this._btnNext.ImageTransparentColor = System.Drawing.Color.Red;
            this._btnNext.Name = "_btnNext";
            this._btnNext.Size = new System.Drawing.Size(23, 23);
            this._btnNext.Text = "Next Page";
            this._btnNext.Click += new System.EventHandler(this._btnNext_Click);
            // 
            // _btnLast
            // 
            this._btnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._btnLast.Image = ((System.Drawing.Image)(resources.GetObject("_btnLast.Image")));
            this._btnLast.ImageTransparentColor = System.Drawing.Color.Red;
            this._btnLast.Name = "_btnLast";
            this._btnLast.Size = new System.Drawing.Size(23, 23);
            this._btnLast.Text = "Last Page";
            this._btnLast.Click += new System.EventHandler(this._btnLast_Click);
            // 
            // _separator
            // 
            this._separator.Name = "_separator";
            this._separator.Size = new System.Drawing.Size(6, 25);
            this._separator.Visible = false;
            // 
            // _btnCancel
            // 
            this._btnCancel.AutoToolTip = false;
            this._btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("_btnCancel.Image")));
            this._btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(70, 23);
            this._btnCancel.Text = "Cancel";
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // _preview
            // 
            this._preview.AutoScroll = true;
            this._preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this._preview.Document = null;
            this._preview.Location = new System.Drawing.Point(0, 25);
            this._preview.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._preview.Name = "_preview";
            this._preview.Size = new System.Drawing.Size(532, 410);
            this._preview.TabIndex = 1;
            this._preview.PageCountChanged += new System.EventHandler(this._preview_PageCountChanged);
            this._preview.StartPageChanged += new System.EventHandler(this._preview_StartPageChanged);
            // 
            // CoolPrintPreviewDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(532, 435);
            this.Controls.Add(this._preview);
            this.Controls.Add(this._toolStrip);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CoolPrintPreviewDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Preview";
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnPrint;
        private System.Windows.Forms.ToolStripButton _btnPageSetup;
        private CoolPrintPreviewControl _preview;
        private System.Windows.Forms.ToolStripSplitButton _btnZoom;
        private System.Windows.Forms.ToolStripMenuItem _itemActualSize;
        private System.Windows.Forms.ToolStripMenuItem _itemFullPage;
        private System.Windows.Forms.ToolStripMenuItem _itemPageWidth;
        private System.Windows.Forms.ToolStripMenuItem _itemTwoPages;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem _item500;
        private System.Windows.Forms.ToolStripMenuItem _item200;
        private System.Windows.Forms.ToolStripMenuItem _item150;
        private System.Windows.Forms.ToolStripMenuItem _item100;
        private System.Windows.Forms.ToolStripMenuItem _item75;
        private System.Windows.Forms.ToolStripMenuItem _item50;
        private System.Windows.Forms.ToolStripMenuItem _item25;
        private System.Windows.Forms.ToolStripMenuItem _item10;
        private System.Windows.Forms.ToolStripButton _btnFirst;
        private System.Windows.Forms.ToolStripButton _btnPrev;
        private System.Windows.Forms.ToolStripTextBox _txtStartPage;
        private System.Windows.Forms.ToolStripLabel _lblPageCount;
        private System.Windows.Forms.ToolStripButton _btnNext;
        private System.Windows.Forms.ToolStripButton _btnLast;
        private System.Windows.Forms.ToolStripSeparator _separator;
        private System.Windows.Forms.ToolStripButton _btnCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}