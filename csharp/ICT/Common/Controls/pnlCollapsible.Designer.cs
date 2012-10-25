//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
namespace Ict.Common.Controls
{
    partial class TPnlCollapsible
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPnlCollapsible));
            this.pnlTitle = new Ict.Common.Controls.TPnlGradient();
            this.pnlCollapsedInfoText = new Ict.Common.Controls.TPnlGradient();
            this.otlCollapsedInfoText = new CustomControl.OrientAbleTextControls.OrientedTextLabel();
            this.pnlContent = new Ict.Common.Controls.TPnlGradient();
            this.pnlTitleText = new Ict.Common.Controls.TPnlGradient();
            this.lblDetailHeading = new System.Windows.Forms.Label();
            this.btnToggle = new System.Windows.Forms.Button();
            this.imlUpDownArrows = new System.Windows.Forms.ImageList(this.components);
            this.tipCollapseExpandHints = new System.Windows.Forms.ToolTip(this.components);
            this.pnlTitle.SuspendLayout();
            this.pnlCollapsedInfoText.SuspendLayout();
            this.pnlTitleText.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTitle
            //
            this.pnlTitle.AutoSize = true;
            this.pnlTitle.Controls.Add(this.pnlCollapsedInfoText);
            this.pnlTitle.Controls.Add(this.pnlTitleText);
            this.pnlTitle.Controls.Add(this.pnlContent);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitle.DontDrawBottomLine = false;
            this.pnlTitle.GradientColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(203)))), ((int)(((byte)(231)))));
            this.pnlTitle.GradientColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.pnlTitle.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(424, 176);
            this.pnlTitle.TabIndex = 2;
            this.pnlTitle.MouseEnter += new System.EventHandler(this.BtnToggleMouseEnter);
            this.pnlTitle.MouseLeave += new System.EventHandler(this.BtnToggleMouseLeave);
            //
            // pnlCollapsedInfoText
            //
            this.pnlCollapsedInfoText.AutoSize = true;
            this.pnlCollapsedInfoText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(228)))), ((int)(((byte)(248)))));
            this.pnlCollapsedInfoText.Controls.Add(this.otlCollapsedInfoText);
            this.pnlCollapsedInfoText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCollapsedInfoText.GradientColorBottom =
                System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(228)))), ((int)(((byte)(248)))));
            this.pnlCollapsedInfoText.GradientColorTop =
                System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(228)))), ((int)(((byte)(248)))));
            this.pnlCollapsedInfoText.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.pnlCollapsedInfoText.Location = new System.Drawing.Point(0, 19);
            this.pnlCollapsedInfoText.Name = "pnlCollapsedInfoText";
            this.pnlCollapsedInfoText.Size = new System.Drawing.Size(39, 157);
            this.pnlCollapsedInfoText.TabIndex = 0;
            this.pnlCollapsedInfoText.Visible = false;
            //
            // otlCollapsedInfoText
            //
            this.otlCollapsedInfoText.BackColor = System.Drawing.Color.Transparent;
            this.otlCollapsedInfoText.Dock = System.Windows.Forms.DockStyle.Left;
            this.otlCollapsedInfoText.Font = new System.Drawing.Font("Verdana",
                12.75F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.otlCollapsedInfoText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.otlCollapsedInfoText.Location = new System.Drawing.Point(0, 0);
            this.otlCollapsedInfoText.Margin = new System.Windows.Forms.Padding(0);
            this.otlCollapsedInfoText.Name = "otlCollapsedInfoText";
            this.otlCollapsedInfoText.RotationAngle = 270D;
            this.otlCollapsedInfoText.Size = new System.Drawing.Size(39, 157);
            this.otlCollapsedInfoText.TabIndex = 0;
            this.otlCollapsedInfoText.Text = "Navigation Bar";
            this.otlCollapsedInfoText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.otlCollapsedInfoText.TextDirection = CustomControl.OrientAbleTextControls.Direction.Clockwise;
            this.otlCollapsedInfoText.TextOrientation = CustomControl.OrientAbleTextControls.Orientation.Rotate;
            this.tipCollapseExpandHints.SetToolTip(this.otlCollapsedInfoText, "Click here to expand the panel");
            this.otlCollapsedInfoText.Click += new System.EventHandler(CollapsedInfoTextClick);
            this.otlCollapsedInfoText.MouseEnter += new System.EventHandler(this.BtnCollapsedInfoTextMouseEnter);
            this.otlCollapsedInfoText.MouseLeave += new System.EventHandler(this.BtnCollapsedInfoTextMouseLeave);
            //
            // pnlContent
            //
            this.pnlContent.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContent.BackColor = System.Drawing.Color.Transparent;
            this.pnlContent.DontDrawBottomLine = false;
            this.pnlContent.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.pnlContent.GradientColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(203)))), ((int)(((byte)(231)))));
            this.pnlContent.GradientColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.pnlContent.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlContent.Location = new System.Drawing.Point(0, 14);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.pnlContent.Size = new System.Drawing.Size(424, 161);
            this.pnlContent.TabIndex = 2;
            //
            // pnlTitleText
            //
            this.pnlTitleText.AutoSize = true;
            this.pnlTitleText.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlTitleText.BackColor = System.Drawing.Color.Transparent;
            this.pnlTitleText.Controls.Add(this.lblDetailHeading);
            this.pnlTitleText.Controls.Add(this.btnToggle);
            this.pnlTitleText.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleText.DontDrawBottomLine = false;
            this.pnlTitleText.GradientColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(203)))), ((int)(((byte)(231)))));
            this.pnlTitleText.GradientColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.pnlTitleText.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlTitleText.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleText.Name = "pnlTitleText";
            this.pnlTitleText.Size = new System.Drawing.Size(424, 19);
            this.pnlTitleText.TabIndex = 1;
            this.tipCollapseExpandHints.SetToolTip(this.pnlTitleText, "Click here to expand / collapse the panel");
            this.pnlTitleText.Click += new System.EventHandler(this.BtnToggleClick);
            //
            // lblDetailHeading
            //
            this.lblDetailHeading.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.lblDetailHeading.AutoEllipsis = true;
            this.lblDetailHeading.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailHeading.ForeColor = System.Drawing.Color.MediumBlue;
            this.lblDetailHeading.Location = new System.Drawing.Point(2, 0);
            this.lblDetailHeading.Name = "lblDetailHeading";
            this.lblDetailHeading.Size = new System.Drawing.Size(395, 18);
            this.lblDetailHeading.TabIndex = 1;
            this.lblDetailHeading.Text = "Collapsible Panel";
            this.tipCollapseExpandHints.SetToolTip(this.lblDetailHeading, "Click here to expand / collapse the panel");
            this.lblDetailHeading.Click += new System.EventHandler(this.BtnToggleClick);
            this.lblDetailHeading.MouseEnter += new System.EventHandler(this.BtnToggleMouseEnter);
            this.lblDetailHeading.MouseLeave += new System.EventHandler(this.BtnToggleMouseLeave);
            //
            // btnToggle
            //
            this.btnToggle.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggle.BackColor = System.Drawing.Color.Transparent;
            this.btnToggle.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggle.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggle.ImageIndex = 0;
            this.btnToggle.ImageList = this.imlUpDownArrows;
            this.btnToggle.Location = new System.Drawing.Point(408, 0);
            this.btnToggle.Name = "btnToggle";
            this.btnToggle.Size = new System.Drawing.Size(16, 16);
            this.btnToggle.TabIndex = 0;
            this.btnToggle.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.tipCollapseExpandHints.SetToolTip(this.btnToggle, "Expands / collapses the panel");
            this.btnToggle.UseVisualStyleBackColor = false;
            this.btnToggle.Click += new System.EventHandler(this.BtnToggleClick);
            this.btnToggle.MouseEnter += new System.EventHandler(this.BtnToggleMouseEnter);
            this.btnToggle.MouseLeave += new System.EventHandler(this.BtnToggleMouseLeave);
            //
            // imlUpDownArrows
            //
            this.imlUpDownArrows.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlUpDownArrows.ImageStream")));
            this.imlUpDownArrows.TransparentColor = System.Drawing.Color.Transparent;
            this.imlUpDownArrows.Images.SetKeyName(0, "CollapseDown.ico");
            this.imlUpDownArrows.Images.SetKeyName(1, "CollapseDown_Hover.ico");
            this.imlUpDownArrows.Images.SetKeyName(2, "CollapseLeft.ico");
            this.imlUpDownArrows.Images.SetKeyName(3, "CollapseLeft_Hover.ico");
            this.imlUpDownArrows.Images.SetKeyName(4, "CollapseRight.ico");
            this.imlUpDownArrows.Images.SetKeyName(5, "CollapseRight_Hover.ico");
            this.imlUpDownArrows.Images.SetKeyName(6, "CollapseUp.ico");
            this.imlUpDownArrows.Images.SetKeyName(7, "CollapseUp_Hover.ico");
            this.imlUpDownArrows.Images.SetKeyName(8, "TaskPanel_CollapseDown.ico");
            this.imlUpDownArrows.Images.SetKeyName(9, "TaskPanel_CollapseDown_Hover.ico");
            this.imlUpDownArrows.Images.SetKeyName(10, "TaskPanel_CollapseUp.ico");
            this.imlUpDownArrows.Images.SetKeyName(11, "TaskPanel_CollapseUp_Hover.ico");
            //
            // TPnlCollapsible
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.pnlTitle);
            this.BackColor = System.Drawing.Color.FromArgb(150, 184, 228);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "TPnlCollapsible";
            this.Size = new System.Drawing.Size(424, 176);
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlCollapsedInfoText.ResumeLayout(false);
            this.pnlTitleText.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Ict.Common.Controls.TPnlGradient pnlCollapsedInfoText;
        private CustomControl.OrientAbleTextControls.OrientedTextLabel otlCollapsedInfoText;

        private System.Windows.Forms.ToolTip tipCollapseExpandHints;
        private System.Windows.Forms.ImageList imlUpDownArrows;
        private System.Windows.Forms.Button btnToggle;
        private System.Windows.Forms.Label lblDetailHeading;
        private Ict.Common.Controls.TPnlGradient pnlTitleText;
        private Ict.Common.Controls.TPnlGradient pnlTitle;
        private Ict.Common.Controls.TPnlGradient pnlContent;
    }
}