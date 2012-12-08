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
    /// load screen for report settings
    /// </summary>
    public partial class TFrmSettingsLoad : System.Windows.Forms.Form
    {
        /// <summary>
        /// the stored settings for the report
        /// </summary>
        protected TStoredSettings FStoredSettings;

        /// <summary>
        /// name of the selected settings
        /// </summary>
        protected String FSettingsName;

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
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.Label1.Text = Catalog.GetString("Existing stored Settings") + ":" +;
            this.Btn_Cancel.Text = Catalog.GetString("Cancel");
            this.Btn_LoadFile.Text = Catalog.GetString("Load");
            this.Text = Catalog.GetString("Load Report Settings");
            #endregion
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