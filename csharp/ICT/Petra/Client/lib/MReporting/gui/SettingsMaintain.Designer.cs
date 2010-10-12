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
    partial class TFrmSettingsMaintain
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
            this.Btn_Close = new System.Windows.Forms.Button();
            this.BtnRename = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
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
            this.LB_ExistingSettings.Size = new System.Drawing.Size(336, 134);
            this.LB_ExistingSettings.Sorted = true;
            this.LB_ExistingSettings.TabIndex = 1;

            //
            // Btn_Close
            //
            this.Btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Btn_Close.Location = new System.Drawing.Point(280, 168);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(72, 24);
            this.Btn_Close.TabIndex = 4;
            this.Btn_Close.Text = "Close";

            //
            // BtnRename
            //
            this.BtnRename.Location = new System.Drawing.Point(192, 168);
            this.BtnRename.Name = "BtnRename";
            this.BtnRename.TabIndex = 3;
            this.BtnRename.Text = "Rename";
            this.BtnRename.Click += new System.EventHandler(this.BtnRename_Click);

            //
            // BtnDelete
            //
            this.BtnDelete.Location = new System.Drawing.Point(104, 168);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.TabIndex = 2;
            this.BtnDelete.Text = "Delete";
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);

            //
            // TFrmSettingsMaintain
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(368, 205);
            this.Controls.Add(this.BtnDelete);
            this.Controls.Add(this.BtnRename);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.LB_ExistingSettings);
            this.Controls.Add(this.Btn_Close);
            this.Name = "TFrmSettingsMaintain";
            this.Text = "Maintain Report Settings";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ListBox LB_ExistingSettings;
        private System.Windows.Forms.Button Btn_Close;
        private System.Windows.Forms.Button BtnRename;
        private System.Windows.Forms.Button BtnDelete;
    }
}