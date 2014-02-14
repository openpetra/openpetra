//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, peters
//
// Copyright 2004-2013 by OM International
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
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MSysMan;

namespace Ict.Petra.Client.MSysMan.Gui
{
    public partial class TUC_GeneralPreferences
    {
        private bool LanguageChanged = false;

        private void InitializeManualCode()
        {
            DataTable CultureTable = new DataTable();

            CultureTable.Columns.Add("Value", typeof(string));
            CultureTable.Columns.Add("Display", typeof(string));

            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                if (!ci.IsNeutralCulture)
                {
                    DataRow row = CultureTable.NewRow();
                    row[0] = ci.Name;
                    row[1] = ci.EnglishName;
                    CultureTable.Rows.Add(row);
                }
            }

            CultureTable.DefaultView.Sort = "Display";

            cmbCulture.DisplayMember = "Display";
            cmbCulture.ValueMember = "Value";
            cmbCulture.DataSource = CultureTable.DefaultView;

            // load languages from names of language sub directories
            string[] LanguageDirectories = Directory.GetDirectories(TAppSettingsManager.ApplicationDirectory);

            string LanguagesAvailable = "en-EN";

            foreach (String directory in LanguageDirectories)
            {
                if (File.Exists(directory + Path.DirectorySeparatorChar + "OpenPetra.resources.dll"))
                {
                    LanguagesAvailable = StringHelper.AddCSV(
                        LanguagesAvailable,
                        directory.Substring(
                            directory.LastIndexOf(Path.DirectorySeparatorChar) + 1));
                }
            }

            cmbLanguage.SetDataSourceStringList(LanguagesAvailable);

            // for the moment default to english, because translations are not fully supported, and the layout does not adjust
            string LanguageCode = "en-EN";
            string CultureCode = CultureInfo.CurrentCulture.Name;
            TRemote.MSysMan.Maintenance.WebConnectors.GetLanguageAndCulture(ref LanguageCode, ref CultureCode);

            cmbCulture.SetSelectedString(CultureCode);
            cmbLanguage.SetSelectedString(LanguageCode);

            llbLaunchpadLink.Click += LaunchpadLinkClicked;

            // Get the number of recent partners that the user has set, if not found take 10 as default value.
            nudNumberOfPartners.Value = TUserDefaults.GetInt16Default(MSysManConstants.USERDEFAULT_NUMBEROFRECENTPARTNERS, 10);
            nudNumberOfPartners.Maximum = 10;

            // Other preferences
            chkEscClosesScreen.Checked = TUserDefaults.GetBooleanDefault(TUserDefaults.NamedDefaults.USERDEFAULT_ESC_CLOSES_SCREEN, true);
            chkSaveWindowProperties.Checked = TUserDefaults.GetBooleanDefault(TUserDefaults.NamedDefaults.USERDEFAULT_SAVE_WINDOW_POS_AND_SIZE, true);
        }

        /// <summary>
        /// Gets the data from all UserControls on this TabControl.
        /// </summary>
        /// <returns>void</returns>
        public void GetDataFromControls()
        {
        }

        /// make sure that the primary key cannot be edited anymore
        public void SetPrimaryKeyReadOnly(bool AReadOnly)
        {
        }

        private void Language_Click(Object Sender, EventArgs e)
        {
            LanguageChanged = true;
        }

        /// <summary>
        /// Saves any changed preferences to s_user_defaults
        /// </summary>
        /// <returns>void</returns>
        public void SaveGeneralTab()
        {
            if (LanguageChanged)
            {
                string LanguageCode = cmbLanguage.GetSelectedString();
                string CultureCode = cmbCulture.GetSelectedString();

                // send to server
                TRemote.MSysMan.Maintenance.WebConnectors.SetLanguageAndCulture(LanguageCode, CultureCode);

                // set local settings for client
                Catalog.Init(LanguageCode, CultureCode);

                // TODO: can we reload the main window with the new language?
                MessageBox.Show(Catalog.GetString("Please restart the OpenPetra client to see the new language"),
                    Catalog.GetString("Restart the client"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            TUserDefaults.SetDefault(MSysManConstants.USERDEFAULT_NUMBEROFRECENTPARTNERS, nudNumberOfPartners.Value);
            TUserDefaults.SetDefault(TUserDefaults.NamedDefaults.USERDEFAULT_ESC_CLOSES_SCREEN, chkEscClosesScreen.Checked);
            TUserDefaults.SetDefault(TUserDefaults.NamedDefaults.USERDEFAULT_SAVE_WINDOW_POS_AND_SIZE, chkSaveWindowProperties.Checked);
        }

        private Boolean ViewMode
        {
            get
            {
                return ((TFrmUserPreferencesDialog)ParentForm).ViewMode;
            }
        }

        /// <summary>
        /// to be called when the client has logged in
        /// </summary>
        public static void InitLanguageAndCulture()
        {
            TRemote.MSysMan.Maintenance.WebConnectors.LoadLanguageAndCultureFromUserDefaults();

            // for the moment default to english, because translations are not fully supported, and the layout does not adjust
            string LanguageCode = "en-EN";
            string CultureCode = CultureInfo.CurrentCulture.Name;
            TRemote.MSysMan.Maintenance.WebConnectors.GetLanguageAndCulture(ref LanguageCode, ref CultureCode);

            // set local settings for client
            Catalog.Init(LanguageCode, CultureCode);
        }

        /// <summary>
        /// Event is fired when the Launchpad Translation Platform LinkLabel is 'clicked'.
        /// </summary>
        /// <param name="ASender">The Launchpad Translation Platform LinkLabel.</param>
        /// <param name="e">Not evaluated.</param>
        /// <returns>void</returns>
        private void LaunchpadLinkClicked(object ASender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://translations.launchpad.net/openpetraorg/trunk/+pots/template1");
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing
        /// to another record, otherwise set it to false.</param>
        /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
        /// user, otherwise set it to false.</param>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        public bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors)
        {
            return true;
        }
    }
}