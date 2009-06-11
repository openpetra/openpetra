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
    /// form for saving the current set of settings
    /// </summary>
    public class TFrmSettingsSave : System.Windows.Forms.Form
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ListBox LB_ExistingSettings;
        private System.Windows.Forms.TextBox TBx_NewName;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_SaveFile;
        private System.Windows.Forms.Label Lbl_SavedFileName;
        private TStoredSettings FStoredSettings;
        private String FSettingsName;

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
        public TFrmSettingsSave(TStoredSettings AStoredSettings, String ASettingsName)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            FStoredSettings = AStoredSettings;
            FSettingsName = ASettingsName;
            this.TBx_NewName.Text = FSettingsName;
            LoadSettingsList();
        }

        /// <summary>
        /// the name that has be assigned to the current set of settings
        /// </summary>
        /// <returns></returns>
        public String GetNewName()
        {
            return FSettingsName;
        }

        private void LB_ExistingSettings_DoubleClick(System.Object sender, System.EventArgs e)
        {
            this.TBx_NewName.Text = LB_ExistingSettings.SelectedItem.ToString();
            Btn_SaveFile_Click(sender, e);
        }

        private void LB_ExistingSettings_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            this.TBx_NewName.Text = LB_ExistingSettings.SelectedItem.ToString();
        }

        private void Btn_Cancel_Click(System.Object sender, System.EventArgs e)
        {
            FSettingsName = "";
        }

        private void Btn_SaveFile_Click(System.Object sender, System.EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.None;

            // don't allow empty name
            if (this.TBx_NewName.Text.Length == 0)
            {
                return;
            }

            // check if the name already exists in the list
            if (this.LB_ExistingSettings.FindStringExact(this.TBx_NewName.Text) == ListBox.NoMatches)
            {
                FSettingsName = this.TBx_NewName.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                if (FStoredSettings.IsSystemSettings(this.TBx_NewName.Text))
                {
                    MessageBox.Show(
                        "'" + this.TBx_NewName.Text + "'" + Environment.NewLine +
                        "is a predefined set of settings, and therefore cannot be overwritten. " + Environment.NewLine +
                        "Please choose another name!",
                        "Cannot overwrite System Settings");
                    FSettingsName = "";
                }
                else
                {
                    // ask if it should be overwritten
                    if (MessageBox.Show("This name already exists. Do you still want to use this name and overwrite the existing settings?",
                            "Overwrite existing Settings?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        FSettingsName = this.TBx_NewName.Text;
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        // cancel overwriting
                        FSettingsName = "";
                    }
                }
            }
        }

        /// <summary>
        /// load the settings into the listbox
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
    }
}