/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.MReporting.Logic;
using System.IO;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// load screen for report settings
    /// </summary>
    public class TFrmSettingsLoad : System.Windows.Forms.Form
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ListBox LB_ExistingSettings;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_LoadFile;

        /// <summary>
        /// the stored settings for the report
        /// </summary>
        protected TStoredSettings FStoredSettings;

        /// <summary>
        /// name of the selected settings
        /// </summary>
        protected String FSettingsName;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
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

        #endregion

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
        /// </summary>
        /// <returns>void</returns>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        /// <summary>
        /// Private Declarations
        /// </summary>
        /// <returns>void</returns>
        public TFrmSettingsLoad(TStoredSettings AStoredSettings)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            Btn_LoadFile.Enabled = false;
            FSettingsName = "";
            FStoredSettings = AStoredSettings;
            LoadSettingsList();
        }

        /// <summary>
        /// get the name of the selected settings
        /// </summary>
        /// <returns></returns>
        public String GetNewName()
        {
            return FSettingsName;
        }

        private void LB_ExistingSettings_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if (this.LB_ExistingSettings.SelectedIndex < 0)
            {
                Btn_LoadFile.Enabled = false;
            }
            else
            {
                Btn_LoadFile.Enabled = true;
            }
        }

        private void LB_ExistingSettings_DoubleClick(System.Object sender, System.EventArgs e)
        {
            Btn_LoadFile_Click(sender, e);
        }

        private void Btn_Cancel_Click(System.Object sender, System.EventArgs e)
        {
            FSettingsName = "";
        }

        private void Btn_LoadFile_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.None;

            if (LB_ExistingSettings.SelectedItem != null)
            {
                FSettingsName = LB_ExistingSettings.SelectedItem.ToString();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        /// <summary>
        /// load the available settings and add them to the listbox
        /// </summary>
        protected void LoadSettingsList()
        {
            StringCollection AvailableSettings;

            AvailableSettings = FStoredSettings.GetAvailableSettings();
            LB_ExistingSettings.Items.Clear();

            foreach (string SettingName in AvailableSettings)
            {
                LB_ExistingSettings.Items.Add(SettingName);
            }
        }

        void TFrmSettingsLoadKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}