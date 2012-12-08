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
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.MReporting.Logic;
using System.IO;
using GNU.Gettext;
using Ict.Common;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// form for saving the current set of settings
    /// </summary>
    public partial class TFrmSettingsSave : System.Windows.Forms.Form
    {
        private TStoredSettings FStoredSettings;
        private String FSettingsName;

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
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.Label1.Text = Catalog.GetString("Existing stored Settings") + ":" +;
            this.Btn_Cancel.Text = Catalog.GetString("Cancel");
            this.Btn_SaveFile.Text = Catalog.GetString("Save");
            this.Lbl_SavedFileName.Text = Catalog.GetString("Please enter a name") + ":" +;
            this.Text = Catalog.GetString("Save Report Settings");
            #endregion
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