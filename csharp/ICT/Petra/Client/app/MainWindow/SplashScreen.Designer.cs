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
using Ict.Petra.Client.CommonDialogs;

namespace SplashScreen
{
    partial class frmSplashScreen
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSplashScreen));
            this.lblProgressText = new System.Windows.Forms.Label();
            this.lblCustomText = new System.Windows.Forms.Label();
            this.pnlTextBoxContainer = new System.Windows.Forms.Panel();
            this.ucoPetraLogoAndVersionInfo = new Ict.Petra.Client.CommonDialogs.TUCPetraLogoAndVersionInfo();
            this.timerClose = new System.Windows.Forms.Timer(this.components);
            this.pnlTextBoxContainer.SuspendLayout();
            this.SuspendLayout();

            //
            // lblProgressText
            //
            this.lblProgressText.Font = new System.Drawing.Font("Microsoft Sans Serif",
                8.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblProgressText.ForeColor = System.Drawing.Color.DimGray;
            this.lblProgressText.Location = new System.Drawing.Point(3, 110);
            this.lblProgressText.Name = "lblProgressText";
            this.lblProgressText.Size = new System.Drawing.Size(374, 18);
            this.lblProgressText.TabIndex = 4;
            this.lblProgressText.Text = "Loading...";
            this.lblProgressText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblProgressText.UseWaitCursor = true;

            //
            // lblCustomText
            //
            this.lblCustomText.Font = new System.Drawing.Font("Microsoft Sans Serif",
                8.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblCustomText.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblCustomText.Location = new System.Drawing.Point(3, 87);
            this.lblCustomText.Name = "lblCustomText";
            this.lblCustomText.Size = new System.Drawing.Size(374, 18);
            this.lblCustomText.TabIndex = 1;
            this.lblCustomText.Text = "Custom Text can be displayed here...";
            this.lblCustomText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblCustomText.UseWaitCursor = true;

            //
            // pnlTextBoxContainer
            //
            this.pnlTextBoxContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.pnlTextBoxContainer.Controls.Add(this.lblProgressText);
            this.pnlTextBoxContainer.Controls.Add(this.lblCustomText);
            this.pnlTextBoxContainer.Location = new System.Drawing.Point(4, 96);
            this.pnlTextBoxContainer.Name = "pnlTextBoxContainer";
            this.pnlTextBoxContainer.Size = new System.Drawing.Size(380, 130);
            this.pnlTextBoxContainer.TabIndex = 1;
            this.pnlTextBoxContainer.UseWaitCursor = true;

            //
            // ucoPetraLogoAndVersionInfo
            //
            this.ucoPetraLogoAndVersionInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ucoPetraLogoAndVersionInfo.InstallationKind = "Standalone / Network / Remote";
            this.ucoPetraLogoAndVersionInfo.Location = new System.Drawing.Point(66, -2);
            this.ucoPetraLogoAndVersionInfo.Name = "ucoPetraLogoAndVersionInfo";
            this.ucoPetraLogoAndVersionInfo.PetraVersion = "Version";
            this.ucoPetraLogoAndVersionInfo.Size = new System.Drawing.Size(246, 179);
            this.ucoPetraLogoAndVersionInfo.TabIndex = 2;
            this.ucoPetraLogoAndVersionInfo.UseWaitCursor = true;

            //
            // timerClose
            //
            this.timerClose.Tick += new System.EventHandler(this.timerCloseTick);

            //
            // frmSplashScreen
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(368, 222);
            this.ControlBox = false;
            this.Controls.Add(this.ucoPetraLogoAndVersionInfo);
            this.Controls.Add(this.pnlTextBoxContainer);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSplashScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.White;
            this.UseWaitCursor = true;
            this.Load += new System.EventHandler(this.FrmSplashScreenLoad);
            this.Click += new System.EventHandler(this.FrmSplashScreenClick);
            this.pnlTextBoxContainer.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Timer timerClose;
        private TUCPetraLogoAndVersionInfo ucoPetraLogoAndVersionInfo;
        private System.Windows.Forms.Panel pnlTextBoxContainer;
        private System.Windows.Forms.Label lblProgressText;
        private System.Windows.Forms.Label lblCustomText;
    }
}