//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
//
namespace Ict.Common.Controls
{
    partial class TUcoFilterAndFind
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the control.
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUcoFilterAndFind));
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.btnCloseFilter = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFilterControls = new Owf.Controls.A1Panel();
            this.pnlExtraFilterControls = new Owf.Controls.A1Panel();
            this.imlButtonIcons = new System.Windows.Forms.ImageList(this.components);
            this.tipGeneral = new System.Windows.Forms.ToolTip(this.components);
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTitle
            // 
            this.pnlTitle.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pnlTitle.Controls.Add(this.btnCloseFilter);
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(150, 22);
            this.pnlTitle.TabIndex = 0;
            // 
            // btnCloseFilter
            // 
            this.btnCloseFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseFilter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCloseFilter.Font = new System.Drawing.Font("Verdana", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseFilter.Location = new System.Drawing.Point(130, 3);
            this.btnCloseFilter.Name = "btnCloseFilter";
            this.btnCloseFilter.Size = new System.Drawing.Size(18, 18);
            this.btnCloseFilter.TabIndex = 9999;
            this.btnCloseFilter.Text = "X";
            this.btnCloseFilter.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.tipGeneral.SetToolTip(this.btnCloseFilter, "Closes this panel");
            this.btnCloseFilter.UseVisualStyleBackColor = true;
            this.btnCloseFilter.Click += new System.EventHandler(this.BtnCloseFilterClick);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(3, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(48, 13);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "List Filter";
            // 
            // pnlFilterControls
            // 
            this.pnlFilterControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFilterControls.BorderColor = System.Drawing.Color.PapayaWhip;
            this.pnlFilterControls.GradientEndColor = System.Drawing.Color.LightSkyBlue;
            this.pnlFilterControls.GradientStartColor = System.Drawing.Color.LightBlue;
            this.pnlFilterControls.Image = null;
            this.pnlFilterControls.ImageLocation = new System.Drawing.Point(4, 4);
            this.pnlFilterControls.Location = new System.Drawing.Point(7, 30);
            this.pnlFilterControls.Name = "pnlFilterControls";
            this.pnlFilterControls.ShadowOffSet = 4;
            this.pnlFilterControls.Size = new System.Drawing.Size(139, 124);
            this.pnlFilterControls.TabIndex = 1;
            // 
            // pnlExtraFilterControls
            // 
            this.pnlExtraFilterControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlExtraFilterControls.BorderColor = System.Drawing.Color.CadetBlue;
            this.pnlExtraFilterControls.GradientEndColor = System.Drawing.Color.LightSkyBlue;
            this.pnlExtraFilterControls.GradientStartColor = System.Drawing.Color.LightBlue;
            this.pnlExtraFilterControls.Image = null;
            this.pnlExtraFilterControls.ImageLocation = new System.Drawing.Point(4, 4);
            this.pnlExtraFilterControls.Location = new System.Drawing.Point(7, 250);
            this.pnlExtraFilterControls.Name = "pnlExtraFilterControls";
            this.pnlExtraFilterControls.ShadowOffSet = 4;
            this.pnlExtraFilterControls.Size = new System.Drawing.Size(139, 124);
            this.pnlExtraFilterControls.TabIndex = 2;
            // 
            // imlButtonIcons
            // 
            this.imlButtonIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlButtonIcons.ImageStream")));
            this.imlButtonIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imlButtonIcons.Images.SetKeyName(0, "X.ico");
            this.imlButtonIcons.Images.SetKeyName(1, "X_red.ico");
            this.imlButtonIcons.Images.SetKeyName(2, "Go.ico");
            this.imlButtonIcons.Images.SetKeyName(3, "Find.ico");
            // 
            // TUcoFilterAndFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Controls.Add(this.pnlExtraFilterControls);
            this.Controls.Add(this.pnlFilterControls);
            this.Controls.Add(this.pnlTitle);
            this.Name = "TUcoFilterAndFind";
            this.Size = new System.Drawing.Size(150, 439);
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.ToolTip tipGeneral;
        private System.Windows.Forms.ImageList imlButtonIcons;
        private System.Windows.Forms.Button btnCloseFilter;
        private Owf.Controls.A1Panel pnlExtraFilterControls;
        private Owf.Controls.A1Panel pnlFilterControls;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlTitle;
    }
}