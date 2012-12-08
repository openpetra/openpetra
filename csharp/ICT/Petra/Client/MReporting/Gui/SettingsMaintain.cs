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
    /// form that helps with mainting the report settings;
    /// allows to delete and to rename settings
    /// </summary>
    public partial class TFrmSettingsMaintain : System.Windows.Forms.Form
    {
        private TStoredSettings FStoredSettings;

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
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.Label1.Text = Catalog.GetString("Existing stored Settings") + ":" +;
            this.Btn_Close.Text = Catalog.GetString("Close");
            this.BtnRename.Text = Catalog.GetString("Rename");
            this.BtnDelete.Text = Catalog.GetString("Delete");
            this.Text = Catalog.GetString("Maintain Report Settings");
            #endregion
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