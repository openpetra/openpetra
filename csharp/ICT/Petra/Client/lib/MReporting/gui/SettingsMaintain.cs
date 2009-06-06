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
	/// form that helps with mainting the report settings;
	/// allows to delete and to rename settings
	/// </summary>
    public class TFrmSettingsMaintain : System.Windows.Forms.Form
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ListBox LB_ExistingSettings;
        private System.Windows.Forms.Button Btn_Close;
        private System.Windows.Forms.Button BtnRename;
        private System.Windows.Forms.Button BtnDelete;
        private TStoredSettings FStoredSettings;

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
        public TFrmSettingsMaintain(TStoredSettings AStoredSettings)
        {
            //  
            // Required for Windows Form Designer support 
            //  
            InitializeComponent();
            FStoredSettings = AStoredSettings;
            LoadSettingsList();
        }

        private void BtnRename_Click(System.Object sender, System.EventArgs e)
        {
            TFrmSettingsRename frmRename;
            String NewName;
            String OldName;

            if (FStoredSettings.IsSystemSettings(LB_ExistingSettings.SelectedItem.ToString()))
            {
                MessageBox.Show(
                    "'" + LB_ExistingSettings.SelectedItem.ToString() + "'" + Environment.NewLine +
                    "is a predefined set of settings, and therefore cannot be renamed. " + Environment.NewLine +
                    "You should load it and then save it with a different name!", "Cannot rename System Settings");
                return;
            }

            frmRename = new TFrmSettingsRename();
            frmRename.NewName = LB_ExistingSettings.SelectedItem.ToString();
            frmRename.OldName = frmRename.NewName;

            if (frmRename.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NewName = frmRename.NewName;
                OldName = frmRename.OldName;

                if (NewName != OldName)
                {
                    // don't allow empty name 
                    if (NewName.Length == 0)
                    {
                        return;
                    }

                    // check if the name already exists in the list 
                    if (this.LB_ExistingSettings.FindStringExact(NewName) != ListBox.NoMatches)
                    {
                        if (FStoredSettings.IsSystemSettings(NewName))
                        {
                            MessageBox.Show(
                                "\"" + NewName + "\"" + Environment.NewLine +
                                "is a predefined set of settings, and therefore cannot be overwritten. " +
                                Environment.NewLine + "Please choose another name!",
                                "Cannot overwrite System Settings");
                            return;
                        }

                        // ask if it should be overwritten 
                        if (MessageBox.Show("This name already exists. Do you still want to use this name and overwrite the existing settings?",
                                "Overwrite existing Settings?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                    }

                    // do the renaming 
                    FStoredSettings.RenameSettings(OldName, NewName);
                    LoadSettingsList();
                }
            }
        }

        private void BtnDelete_Click(System.Object sender, System.EventArgs e)
        {
            if (LB_ExistingSettings.SelectedItem != null)
            {
                if (FStoredSettings.IsSystemSettings(LB_ExistingSettings.SelectedItem.ToString()))
                {
                    MessageBox.Show(
                        "\"" + LB_ExistingSettings.SelectedItem.ToString() + "\"" + Environment.NewLine +
                        "is a predefined set of settings, and therefore cannot be deleted.", "Cannot delete System Settings");
                    return;
                }

                if ((MessageBox.Show("Do you really want to delete the settings " + LB_ExistingSettings.SelectedItem.ToString() + '?',
                         "Delete Settings?", MessageBoxButtons.YesNo)) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                FStoredSettings.DeleteSettings(LB_ExistingSettings.SelectedItem.ToString());
                LoadSettingsList();
            }
        }

        /// <summary>
        /// load available settings into a listbox
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
