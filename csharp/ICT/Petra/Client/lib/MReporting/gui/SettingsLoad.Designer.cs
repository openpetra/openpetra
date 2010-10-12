//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System;
using System.Windows.Forms;
using System.Drawing.Printing;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MReporting.Gui
{
    partial class TFrmSettingsLoad
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
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.Label1 = new System.Windows.Forms.Label();
            this.LB_ExistingSettings = new System.Windows.Forms.ListBox();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_LoadFile = new System.Windows.Forms.Button();
            this.SuspendLayout();

            //
            // Label1
            //
            this.Label1.Location = new System.Drawing.Point(16, 8);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(128, 16);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Existing stored Settings:";

            //
            // LB_ExistingSettings
            //
            this.LB_ExistingSettings.Items.AddRange(new object[] {
                    "My default I&E statement",
                    "OM Standard I&E report",
                    "Board Month End Report"
                });
            this.LB_ExistingSettings.Location = new System.Drawing.Point(16, 24);
            this.LB_ExistingSettings.Name = "LB_ExistingSettings";
            this.LB_ExistingSettings.Size = new System.Drawing.Size(336, 134);
            this.LB_ExistingSettings.Sorted = true;
            this.LB_ExistingSettings.TabIndex = 1;
            this.LB_ExistingSettings.SelectedIndexChanged += new System.EventHandler(this.LB_ExistingSettings_SelectedIndexChanged);
            this.LB_ExistingSettings.DoubleClick += new System.EventHandler(this.LB_ExistingSettings_DoubleClick);

            //
            // Btn_Cancel
            //
            this.Btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Btn_Cancel.Location = new System.Drawing.Point(192, 168);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(72, 24);
            this.Btn_Cancel.TabIndex = 2;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);

            //
            // Btn_LoadFile
            //
            this.Btn_LoadFile.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Btn_LoadFile.Location = new System.Drawing.Point(280, 168);
            this.Btn_LoadFile.Name = "Btn_LoadFile";
            this.Btn_LoadFile.Size = new System.Drawing.Size(72, 24);
            this.Btn_LoadFile.TabIndex = 3;
            this.Btn_LoadFile.Text = "Load";
            this.Btn_LoadFile.Click += new System.EventHandler(this.Btn_LoadFile_Click);

            //
            // TFrmSettingsLoad
            //
            this.AcceptButton = this.Btn_LoadFile;
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.Btn_Cancel;
            this.ClientSize = new System.Drawing.Size(368, 205);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.LB_ExistingSettings);
            this.Controls.Add(this.Btn_Cancel);
            this.Controls.Add(this.Btn_LoadFile);
            this.KeyPreview = true;
            this.Name = "TFrmSettingsLoad";
            this.Text = "Load Report Settings";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TFrmSettingsLoadKeyUp);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ListBox LB_ExistingSettings;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_LoadFile;
    }
}