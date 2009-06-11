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
using System.Collections.Specialized;
using System.Windows.Forms;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui
{
    public delegate void TSetControlsEventHandler(TParameterList AParameters);

    /// <summary>
    /// todoComment
    /// </summary>
    public class TSettingsGui
    {
        /// <summary>this System.Object manages the stored settings of the current user and current report</summary>
        protected TStoredSettings FStoredSettings;

        /// the name of the current settings, if they have been loaded or already saved under this name
        protected string FCurrentSettingsName;


        public TSettingsGui()
        {
            FCurrentSettingsName = "";
        }

        private TRptCalculator FCalculator = null;
        private UC_Columns FUcoReportColumns = null;
        private MenuItem FMniLoadSettings = null;
        private ContextMenu FMnuLoadSettings = null;

        /// <param name="AMniLoadSettings">this is the File/Load settings submenu</param>
        /// <param name="AMnuLoadSettings">this is the context menu from the toolbar button Load settings</param>
        public void InitialiseData(TRptCalculator ACalculator,
            string AReportName,
            UC_Columns AUcoReportColumns,
            MenuItem AMniLoadSettings,
            ContextMenu AMnuLoadSettings)
        {
            FCalculator = ACalculator;
            FUcoReportColumns = AUcoReportColumns;
            FMniLoadSettings = AMniLoadSettings;
            FMnuLoadSettings = AMnuLoadSettings;

            string SettingsDirectory = TClientSettings.ReportingPathReportSettings;
            this.FStoredSettings = new TStoredSettings(AReportName, SettingsDirectory);
            UpdateLoadingMenu(this.FStoredSettings.GetRecentlyUsedSettings());
        }

        public string LoadSettingsDialog(string AWindowCaption)
        {
            TFrmSettingsLoad SettingsDialog = new TFrmSettingsLoad(FStoredSettings);

            if (SettingsDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return LoadSettings(FUcoReportColumns, SettingsDialog.GetNewName());
            }

            return AWindowCaption;
        }

        public string SaveSettingsAs(string AWindowCaption, string ACurrentReport)
        {
            TFrmSettingsSave SettingsDialog;

            if (FCurrentSettingsName == "")
            {
                SettingsDialog = new TFrmSettingsSave(FStoredSettings, ACurrentReport);
            }
            else
            {
                SettingsDialog = new TFrmSettingsSave(FStoredSettings, FCurrentSettingsName);
            }

            if (SettingsDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FCurrentSettingsName = SettingsDialog.GetNewName();

                // set the title of the window
                string newWindowCaption = AWindowCaption + ": " + FCurrentSettingsName;
                StringCollection RecentlyUsedSettings = null;
                try
                {
                    RecentlyUsedSettings = this.FStoredSettings.SaveSettings(FCurrentSettingsName, FCalculator.GetParameters());
                }
                catch (Exception)
                {
                    MessageBox.Show("Not a valid name. Please use letters numbers and underscores etc, values not saved");
                    newWindowCaption = AWindowCaption + ": Not a valid name, values not saved!";
                }

                if (RecentlyUsedSettings != null)
                {
                    UpdateLoadingMenu(RecentlyUsedSettings);
                }

                return newWindowCaption;
            }

            return AWindowCaption;
        }

        public string SaveSettings(string AWindowCaption, string ACurrentReport)
        {
            if ((FCurrentSettingsName.Length == 0) || (FStoredSettings.IsSystemSettings(FCurrentSettingsName)))
            {
                return SaveSettingsAs(AWindowCaption, ACurrentReport);
            }
            else
            {
                StringCollection RecentlyUsedSettings =
                    this.FStoredSettings.SaveSettings(FCurrentSettingsName, FCalculator.GetParameters());

                if (RecentlyUsedSettings != null)
                {
                    UpdateLoadingMenu(RecentlyUsedSettings);
                }
            }

            return AWindowCaption;
        }

        public void MaintainSettings()
        {
            TFrmSettingsMaintain SettingsDialog = new TFrmSettingsMaintain(FStoredSettings);

            SettingsDialog.ShowDialog();
            UpdateLoadingMenu(this.FStoredSettings.GetRecentlyUsedSettings());
        }

        /**
         * This procedure loads the available saved settings into the Load menu
         *
         */
        protected void UpdateLoadingMenu(StringCollection ARecentlyUsedSettings)
        {
            System.Int32 Counter;

            if (FMnuLoadSettings == null)
            {
                return;
            }

            Counter = 0;

            foreach (MenuItem item in FMnuLoadSettings.MenuItems)
            {
                if (Counter <= ARecentlyUsedSettings.Count - 1)
                {
                    item.Text = ARecentlyUsedSettings[Counter];
                    item.Visible = true;
                }
                else
                {
                    item.Visible = false;
                }

                Counter++;
            }

            Counter = 0;

            foreach (MenuItem item in FMniLoadSettings.MenuItems)
            {
                if (Counter <= ARecentlyUsedSettings.Count - 1)
                {
                    item.Text = ARecentlyUsedSettings[Counter];
                    item.Visible = true;
                }
                else
                {
                    item.Visible = false;
                }

                Counter++;
            }
        }

        /// Custom Event that can be triggered in order for the controls on the screen to be set with the given parameters
        public event TSetControlsEventHandler SetControlsEventHandler;

        /**
         * Raises Event SetControls.
         */
        protected void OnSetControls(TParameterList AParameters)
        {
            if (SetControlsEventHandler != null)
            {
                SetControlsEventHandler(AParameters);
            }
        }

        /// <summary>
        /// This procedure loads the parameters of the given settings
        /// </summary>
        /// <returns>the new title for the window containing the name of the settings</returns>
        public string LoadSettings(Object sender, string AWindowCaption)
        {
            if (FMnuLoadSettings == null)
            {
                return "";
            }

            string SettingsName = "";

            // do a loop, just need the main menu item for load settings, or the context menu
            foreach (MenuItem item in FMnuLoadSettings.MenuItems)
            {
                if (sender == item)
                {
                    SettingsName = item.Text;
                }
            }

            foreach (MenuItem item in FMniLoadSettings.MenuItems)
            {
                if (sender == item)
                {
                    SettingsName = item.Text;
                }
            }

            return LoadSettings(SettingsName, AWindowCaption);
        }

        protected string LoadSettings(string ASettingsName, string AWindowCaption)
        {
            FCurrentSettingsName = ASettingsName;

            TParameterList Parameters = new TParameterList();
            StringCollection RecentlyUsedSettings = FStoredSettings.LoadSettings(ref FCurrentSettingsName, ref Parameters);

            string newWindowCaption = AWindowCaption;

            // set the title of the window
            if (FCurrentSettingsName.Length > 0)
            {
                newWindowCaption = AWindowCaption + ": " + FCurrentSettingsName;
            }
            else
            {
                newWindowCaption = AWindowCaption;
            }

            OnSetControls(Parameters);
            UpdateLoadingMenu(RecentlyUsedSettings);
            return newWindowCaption;
        }

        /**
         * This procedure loads the parameters of the default settings;
         * at the moment this is implemented to use the last used settings
         *
         */
        public string LoadDefaultSettings(string AWindowCaption)
        {
            StringCollection RecentlyUsedSettings = FStoredSettings.GetRecentlyUsedSettings();

            if (RecentlyUsedSettings.Count > 0)
            {
                return LoadSettings(RecentlyUsedSettings[0], AWindowCaption);
            }

            return AWindowCaption;
        }
    }
}