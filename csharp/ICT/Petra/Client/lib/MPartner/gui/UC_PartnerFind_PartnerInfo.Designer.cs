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
            this.ucoPartnerInfo = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerInfo();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            //
            // ucoPartnerInfo
            //
            this.ucoPartnerInfo.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.ucoPartnerInfo.BackColor = System.Drawing.SystemColors.Info;
            this.ucoPartnerInfo.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.ucoPartnerInfo.Location = new System.Drawing.Point(0, 0);
            this.ucoPartnerInfo.MainDS = null;
            this.ucoPartnerInfo.Margin = new System.Windows.Forms.Padding(0);
            this.ucoPartnerInfo.Name = "ucoPartnerInfo";
            this.ucoPartnerInfo.Size = new System.Drawing.Size(424, 126);
            this.ucoPartnerInfo.TabIndex = 2;
            this.ucoPartnerInfo.VerificationResultCollection = null;

            this.pnlContent.Controls.Add(ucoPartnerInfo);
            //
            // TUC_PartnerFind_PartnerInfo
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TUC_PartnerFind_PartnerInfo";
            this.Size = new System.Drawing.Size(424, 150);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Ict.Petra.Client.MPartner.Gui.TUC_PartnerInfo ucoPartnerInfo;
    }
}