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
    partial class TFrmSettingsSave
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
        /// this is InitializeComponent from PetraForm
        /// this will not be needed anymore when the code generation works
        /// </summary>
        private void InitializeComponent()
        {
            this.Label1 = new System.Windows.Forms.Label();
            this.LB_ExistingSettings = new System.Windows.Forms.ListBox();
            this.TBx_NewName = new System.Windows.Forms.TextBox();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_SaveFile = new System.Windows.Forms.Button();
            this.Lbl_SavedFileName = new System.Windows.Forms.Label();
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
            this.LB_ExistingSettings.Items.AddRange(new object[] { "My d" + "efault I&E statement", "OM Standard I&E report",
                                                                   "Board Month End R" + "eport" });
            this.LB_ExistingSettings.Location = new System.Drawing.Point(16, 24);
            this.LB_ExistingSettings.Name = "LB_ExistingSettings";
            this.LB_ExistingSettings.Size = new System.Drawing.Size(336, 95);
            this.LB_ExistingSettings.Sorted = true;
            this.LB_ExistingSettings.TabIndex = 1;
            this.LB_ExistingSettings.DoubleClick += new System.EventHandler(this.LB_ExistingSettings_DoubleClick);
            this.LB_ExistingSettings.SelectedIndexChanged += new System.EventHandler(this.LB_ExistingSettings_SelectedIndexChanged);

            //
            // TBx_NewName
            //
            this.TBx_NewName.Location = new System.Drawing.Point(136, 136);
            this.TBx_NewName.Name = "TBx_NewName";
            this.TBx_NewName.Size = new System.Drawing.Size(216, 20);
            this.TBx_NewName.TabIndex = 3;
            this.TBx_NewName.Text = "";

            //
            // Btn_Cancel
            //
            this.Btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Btn_Cancel.Location = new System.Drawing.Point(192, 168);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(72, 24);
            this.Btn_Cancel.TabIndex = 4;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);

            //
            // Btn_SaveFile
            //
            this.Btn_SaveFile.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Btn_SaveFile.Location = new System.Drawing.Point(280, 168);
            this.Btn_SaveFile.Name = "Btn_SaveFile";
            this.Btn_SaveFile.Size = new System.Drawing.Size(72, 24);
            this.Btn_SaveFile.TabIndex = 5;
            this.Btn_SaveFile.Text = "Save";
            this.Btn_SaveFile.Click += new System.EventHandler(this.Btn_SaveFile_Click);

            //
            // Lbl_SavedFileName
            //
            this.Lbl_SavedFileName.Location = new System.Drawing.Point(16, 136);
            this.Lbl_SavedFileName.Name = "Lbl_SavedFileName";
            this.Lbl_SavedFileName.Size = new System.Drawing.Size(112, 23);
            this.Lbl_SavedFileName.TabIndex = 2;
            this.Lbl_SavedFileName.Text = "Please enter a name:";

            //
            // TFrmSettingsSave
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(368, 205);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.LB_ExistingSettings);
            this.Controls.Add(this.TBx_NewName);
            this.Controls.Add(this.Btn_Cancel);
            this.Controls.Add(this.Btn_SaveFile);
            this.Controls.Add(this.Lbl_SavedFileName);
            this.Name = "TFrmSettingsSave";
            this.Text = "Save Report Settings";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ListBox LB_ExistingSettings;
        private System.Windows.Forms.TextBox TBx_NewName;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_SaveFile;
        private System.Windows.Forms.Label Lbl_SavedFileName;
    }
}