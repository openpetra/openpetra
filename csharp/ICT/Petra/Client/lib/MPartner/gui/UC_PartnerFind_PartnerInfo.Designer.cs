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
namespace Ict.Petra.Client.MPartner.Gui
{
    /// UserControl for displaying Partner Info data in the Partner Find screen.
    partial class TUC_PartnerFind_PartnerInfo
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
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerFind_PartnerInfo));
            this.pnlPartnerDetails = new System.Windows.Forms.Panel();
            this.pnlPartnerDetailsTitle = new System.Windows.Forms.Panel();
            this.lblDetailHeading = new System.Windows.Forms.Label();
            this.btnTogglePartnerDetails = new System.Windows.Forms.Button();
            this.imlUpDownArrows = new System.Windows.Forms.ImageList(this.components);
            this.ucoPartnerInfo = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerInfo();
            this.tipCollapseExpandHints = new System.Windows.Forms.ToolTip(this.components);
            this.pnlPartnerDetails.SuspendLayout();
            this.pnlPartnerDetailsTitle.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlPartnerDetails
            //
            this.pnlPartnerDetails.AutoSize = true;
            this.pnlPartnerDetails.BackColor = System.Drawing.Color.Red;
            this.pnlPartnerDetails.Controls.Add(this.pnlPartnerDetailsTitle);
            this.pnlPartnerDetails.Controls.Add(this.ucoPartnerInfo);
            this.pnlPartnerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPartnerDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerDetails.Name = "pnlPartnerDetails";
            this.pnlPartnerDetails.Size = new System.Drawing.Size(424, 150);
            this.pnlPartnerDetails.TabIndex = 2;
            //
            // pnlPartnerDetailsTitle
            //
            this.pnlPartnerDetailsTitle.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlPartnerDetailsTitle.Controls.Add(this.lblDetailHeading);
            this.pnlPartnerDetailsTitle.Controls.Add(this.btnTogglePartnerDetails);
            this.pnlPartnerDetailsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPartnerDetailsTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerDetailsTitle.Name = "pnlPartnerDetailsTitle";
            this.pnlPartnerDetailsTitle.Size = new System.Drawing.Size(424, 24);
            this.pnlPartnerDetailsTitle.TabIndex = 1;
            this.tipCollapseExpandHints.SetToolTip(this.pnlPartnerDetailsTitle, "Click here to expand / collapse the Partner Info panel");
            this.pnlPartnerDetailsTitle.MouseLeave += new System.EventHandler(this.BtnTogglePartnerDetailsMouseLeave);
            this.pnlPartnerDetailsTitle.Click += new System.EventHandler(this.BtnTogglePartnerDetailsClick);
            this.pnlPartnerDetailsTitle.MouseEnter += new System.EventHandler(this.BtnTogglePartnerDetailsMouseEnter);
            //
            // lblDetailHeading
            //
            this.lblDetailHeading.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailHeading.ForeColor = System.Drawing.Color.MediumBlue;
            this.lblDetailHeading.Location = new System.Drawing.Point(2, 5);
            this.lblDetailHeading.Name = "lblDetailHeading";
            this.lblDetailHeading.Size = new System.Drawing.Size(144, 18);
            this.lblDetailHeading.TabIndex = 1;
            this.lblDetailHeading.Text = "Partner Info";
            this.tipCollapseExpandHints.SetToolTip(this.lblDetailHeading, "Click here to expand / collapse the Partner Info panel");
            this.lblDetailHeading.Click += new System.EventHandler(this.BtnTogglePartnerDetailsClick);
            //
            // btnTogglePartnerDetails
            //
            this.btnTogglePartnerDetails.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTogglePartnerDetails.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnTogglePartnerDetails.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnTogglePartnerDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTogglePartnerDetails.Font = new System.Drawing.Font("Verdana",
                7F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.btnTogglePartnerDetails.ImageIndex = 0;
            this.btnTogglePartnerDetails.ImageList = this.imlUpDownArrows;
            this.btnTogglePartnerDetails.Location = new System.Drawing.Point(403, 2);
            this.btnTogglePartnerDetails.Name = "btnTogglePartnerDetails";
            this.btnTogglePartnerDetails.Size = new System.Drawing.Size(18, 18);
            this.btnTogglePartnerDetails.TabIndex = 0;
            this.btnTogglePartnerDetails.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.tipCollapseExpandHints.SetToolTip(this.btnTogglePartnerDetails, "Expands / collapses the Partner Info panel");
            this.btnTogglePartnerDetails.UseVisualStyleBackColor = false;
            this.btnTogglePartnerDetails.MouseLeave += new System.EventHandler(this.BtnTogglePartnerDetailsMouseLeave);
            this.btnTogglePartnerDetails.Click += new System.EventHandler(this.BtnTogglePartnerDetailsClick);
            this.btnTogglePartnerDetails.MouseEnter += new System.EventHandler(this.BtnTogglePartnerDetailsMouseEnter);
            //
            // imlUpDownArrows
            //
            this.imlUpDownArrows.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlUpDownArrows.ImageStream")));
            this.imlUpDownArrows.TransparentColor = System.Drawing.Color.Transparent;
            this.imlUpDownArrows.Images.SetKeyName(0, "HCollapseDown.ico");
            this.imlUpDownArrows.Images.SetKeyName(1, "HCollapseUp.ico");
            this.imlUpDownArrows.Images.SetKeyName(2, "HCollapseUpHover.ico");
            this.imlUpDownArrows.Images.SetKeyName(3, "HCollapseDownHover.ico");
            //
            // ucoPartnerInfo
            //
            this.ucoPartnerInfo.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.ucoPartnerInfo.BackColor = System.Drawing.SystemColors.Info;
            this.ucoPartnerInfo.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.ucoPartnerInfo.Location = new System.Drawing.Point(0, 24);
            this.ucoPartnerInfo.MainDS = null;
            this.ucoPartnerInfo.Margin = new System.Windows.Forms.Padding(0);
            this.ucoPartnerInfo.Name = "ucoPartnerInfo";
            this.ucoPartnerInfo.Size = new System.Drawing.Size(424, 126);
            this.ucoPartnerInfo.TabIndex = 2;
            this.ucoPartnerInfo.VerificationResultCollection = null;
            //
            // TUC_PartnerFind_PartnerInfo
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.pnlPartnerDetails);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TUC_PartnerFind_PartnerInfo";
            this.Size = new System.Drawing.Size(424, 150);
            this.pnlPartnerDetails.ResumeLayout(false);
            this.pnlPartnerDetailsTitle.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ToolTip tipCollapseExpandHints;
        private System.Windows.Forms.ImageList imlUpDownArrows;
        private Ict.Petra.Client.MPartner.Gui.TUC_PartnerInfo ucoPartnerInfo;
        private System.Windows.Forms.Button btnTogglePartnerDetails;
        private System.Windows.Forms.Label lblDetailHeading;
        private System.Windows.Forms.Panel pnlPartnerDetailsTitle;
        private System.Windows.Forms.Panel pnlPartnerDetails;
    }
}