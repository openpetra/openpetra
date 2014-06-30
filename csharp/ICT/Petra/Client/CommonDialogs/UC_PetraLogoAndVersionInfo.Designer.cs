//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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

namespace Ict.Petra.Client.CommonDialogs
{
    /// <summary>
    /// UserControl that features a big Petra Logo and Version Infomation.
    /// </summary>
    partial class TUCPetraLogoAndVersionInfo
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
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUCPetraLogoAndVersionInfo));
            this.lblTopHorizontalLine = new System.Windows.Forms.Label();
            this.lblPetra = new System.Windows.Forms.Label();
            this.lblPetraVersion = new System.Windows.Forms.Label();
            this.pnlTextBoxContainer = new System.Windows.Forms.Panel();
            this.lblCopyrightNotice = new System.Windows.Forms.Label();
            this.lblInstallationKind = new System.Windows.Forms.Label();
            this.pbxPetraLogo = new System.Windows.Forms.PictureBox();
            this.pnlTextBoxContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPetraLogo)).BeginInit();
            this.SuspendLayout();
            //
            // lblTopHorizontalLine
            //
            this.lblTopHorizontalLine.Font = new System.Drawing.Font("Times New Roman",
                20F,
                System.Drawing.FontStyle.Underline,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblTopHorizontalLine.Location = new System.Drawing.Point(42, -26);
            this.lblTopHorizontalLine.Name = "lblTopHorizontalLine";
            this.lblTopHorizontalLine.Size = new System.Drawing.Size(163, 31);
            this.lblTopHorizontalLine.TabIndex = 5;
            this.lblTopHorizontalLine.Text = "                  ";
            this.lblTopHorizontalLine.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //
            // lblPetra
            //
            this.lblPetra.Font = new System.Drawing.Font("Times New Roman",
                20F,
                System.Drawing.FontStyle.Underline,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblPetra.Location = new System.Drawing.Point(42, -1);
            this.lblPetra.Name = "lblPetra";
            this.lblPetra.Size = new System.Drawing.Size(163, 31);
            this.lblPetra.TabIndex = 0;
            this.lblPetra.Text = "OpenPetra";
            this.lblPetra.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //
            // lblPetraVersion
            //
            this.lblPetraVersion.Font = new System.Drawing.Font("Microsoft Sans Serif",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblPetraVersion.Location = new System.Drawing.Point(3, 29);
            this.lblPetraVersion.Name = "lblPetraVersion";
            this.lblPetraVersion.Size = new System.Drawing.Size(238, 18);
            this.lblPetraVersion.TabIndex = 2;
            this.lblPetraVersion.Text = "Version";
            this.lblPetraVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblPetraVersion.Click += new System.EventHandler(this.LblPetraVersionClick);
            //
            // pnlTextBoxContainer
            //
            this.pnlTextBoxContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.pnlTextBoxContainer.Controls.Add(this.lblTopHorizontalLine);
            this.pnlTextBoxContainer.Controls.Add(this.lblPetra);
            this.pnlTextBoxContainer.Controls.Add(this.lblCopyrightNotice);
            this.pnlTextBoxContainer.Controls.Add(this.lblPetraVersion);
            this.pnlTextBoxContainer.Controls.Add(this.lblInstallationKind);
            this.pnlTextBoxContainer.Location = new System.Drawing.Point(0, 97);
            this.pnlTextBoxContainer.Name = "pnlTextBoxContainer";
            this.pnlTextBoxContainer.Size = new System.Drawing.Size(244, 90);
            this.pnlTextBoxContainer.TabIndex = 3;
            //
            // lblCopyrightNotice
            //
            this.lblCopyrightNotice.Font = new System.Drawing.Font("Microsoft Sans Serif",
                8.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblCopyrightNotice.Location = new System.Drawing.Point(3, 47);
            this.lblCopyrightNotice.Name = "lblCopyrightNotice";
            this.lblCopyrightNotice.Size = new System.Drawing.Size(238, 18);
            this.lblCopyrightNotice.TabIndex = 8;
            this.lblCopyrightNotice.Text = "© 1995 - 2013 by OM International";
            this.lblCopyrightNotice.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //
            // lblInstallationKind
            //
            this.lblInstallationKind.Font = new System.Drawing.Font("Microsoft Sans Serif",
                8.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblInstallationKind.Location = new System.Drawing.Point(3, 67);
            this.lblInstallationKind.Name = "lblInstallationKind";
            this.lblInstallationKind.Size = new System.Drawing.Size(238, 18);
            this.lblInstallationKind.TabIndex = 3;
            this.lblInstallationKind.Text = "Standalone / Network / Remote";
            this.lblInstallationKind.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //
            // pbxPetraLogo
            //
            this.pbxPetraLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbxPetraLogo.Image")));
            this.pbxPetraLogo.Location = new System.Drawing.Point(70, 1);
            this.pbxPetraLogo.Name = "pbxPetraLogo";
            this.pbxPetraLogo.Size = new System.Drawing.Size(99, 96);
            this.pbxPetraLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxPetraLogo.TabIndex = 4;
            this.pbxPetraLogo.TabStop = false;
            //
            // TUCPetraLogoAndVersionInfo
            //
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.pnlTextBoxContainer);
            this.Controls.Add(this.pbxPetraLogo);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TUCPetraLogoAndVersionInfo";
            this.Size = new System.Drawing.Size(241, 190);
            this.pnlTextBoxContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxPetraLogo)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.PictureBox pbxPetraLogo;
        private System.Windows.Forms.Label lblInstallationKind;
        private System.Windows.Forms.Label lblCopyrightNotice;
        private System.Windows.Forms.Panel pnlTextBoxContainer;
        private System.Windows.Forms.Label lblPetraVersion;
        private System.Windows.Forms.Label lblPetra;
        private System.Windows.Forms.Label lblTopHorizontalLine;
    }
}