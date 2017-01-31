//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Specialized;
using System.Windows.Forms;
using Ict.Petra.Shared.MReporting;
using System.IO;
using System.Data.Odbc;
using System.Data;
using Ict.Common;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MReporting.Logic
{
    /// <summary>
    /// provides methods to deal with stored settings from the reports
    /// </summary>
    public class TStoredSettings
    {
        /// <summary>constant for the number of how many settings should be in the menu</summary>
        public const Int32 MAX_NUMBER_OF_RECENT_SETTINGS = 5;

        /// <summary>the path where the application is started from.</summary>
        private static String FApplicationDirectory = Environment.CurrentDirectory;

        /// <summary>Private Declarations</summary>
        private String FReportName;
        private String FSettingsDirectory;
        private String FUserSettingsDirectory;

        /// <summary>
        /// The constructor sets the name of the report and the name of the user
        ///
        /// </summary>
        /// <param name="AReportName">the name of the report (to know where to search/store settings)</param>
        /// <param name="ASettingsDirectory">this is where the default settings are stored (in subdirectories for each report type)</param>
        /// <param name="AUserSettingsDirectory">this is where the user settings are stored (in subdirectories for each report type)</param>
        /// <returns>void</returns>
        public TStoredSettings(String AReportName, String ASettingsDirectory, String AUserSettingsDirectory)
        {
            if (AReportName == "")
            {
                // An empty string as report name causes various issues with this class like wrongly set path variables.
                throw new ArgumentException("The given report name must not be an empty String.", "AReportName");
            }

            this.FReportName = AReportName;
            this.FSettingsDirectory = ASettingsDirectory + System.IO.Path.DirectorySeparatorChar;
            this.FUserSettingsDirectory = AUserSettingsDirectory + System.IO.Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Get a StringCollection with stored settings for the report
        /// </summary>
        public StringCollection GetAvailableSettings()
        {
            String[] FilenameList = null;
            String SettingName;
            StringCollection ReturnValue = new StringCollection();

            // Switching back to the application directory, because the path names might be relative.
            Environment.CurrentDirectory = FApplicationDirectory;

            String Path = FSettingsDirectory + FReportName;
            try
            {
                FilenameList = System.IO.Directory.GetFiles(Path, "*.xml");
            }
            catch (System.Exception)
            {
                // Messagebox.show('Error: Xmlfiles could not be found!!!');
            }

            if (FilenameList != null)
            {
                foreach (string FileName in FilenameList)
                {
                    SettingName = System.IO.Path.GetFileNameWithoutExtension(FileName);
                    ReturnValue.Add(SettingName);
                }
            }

            // Now get the user settings
            Path = FUserSettingsDirectory + FReportName;

            if (System.IO.Directory.Exists(Path))
            {
                FilenameList = System.IO.Directory.GetFiles(Path, "*.xml");
            }

            if (FilenameList != null)
            {
                foreach (string FileName in FilenameList)
                {
                    SettingName = System.IO.Path.GetFileNameWithoutExtension(FileName);

                    if (!ReturnValue.Contains(SettingName))
                    {
                        ReturnValue.Add(SettingName);
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// This returns a StringCollection with the names of recently used settings
        /// for the given report
        ///
        /// </summary>
        /// <returns>the list of names of recently used settings, which exist at the moment
        /// </returns>
        public StringCollection GetRecentlyUsedSettings()
        {
            StringCollection ReturnValue;
            String SettingName;
            StringCollection AvailableSettings;

            System.Int32 Counter;
            ReturnValue = new StringCollection();

            if (("RptStg" + FReportName).Length > 32)
            {
                throw new Exception(String.Format("Report name ({0}) is too long for the settings",
                        FReportName));
            }

            // get names of recently used settings from the database
            ReturnValue = StringHelper.StrSplit(TUserDefaults.GetStringDefault("RptStg" + FReportName, ""), ",");

            // remove settings that are not available anymore
            AvailableSettings = GetAvailableSettings();
            Counter = 0;

            while (Counter < ReturnValue.Count)
            {
                SettingName = ReturnValue[Counter];

                if (!AvailableSettings.Contains(SettingName))
                {
                    ReturnValue.Remove(SettingName);
                }
                else
                {
                    Counter = Counter + 1;
                }
            }

            // we might to fill up with reports from the directory, that have not been used yet
            Counter = 0;

            while ((Counter < AvailableSettings.Count) && (ReturnValue.Count < MAX_NUMBER_OF_RECENT_SETTINGS))
            {
                SettingName = AvailableSettings[Counter];

                if (!ReturnValue.Contains(SettingName))
                {
                    ReturnValue.Add(SettingName);
                }

                Counter = Counter + 1;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This adds a setting to the top of the list of recently used settings.
        ///
        /// </summary>
        /// <param name="ASettingsName">the name of the setting, which should be most recent</param>
        /// <returns>the list of names of recently used settings, which exist at the moment
        /// </returns>
        public StringCollection UpdateRecentlyUsedSettings(String ASettingsName)
        {
            StringCollection RecentlyUsedSettings;

            RecentlyUsedSettings = GetRecentlyUsedSettings();

            // is the setting already the most recent one? then exit
            if (RecentlyUsedSettings.IndexOf(ASettingsName) == 0)
            {
                return RecentlyUsedSettings;
            }

            // remove the current setting if it is already there
            if (RecentlyUsedSettings.Contains(ASettingsName))
            {
                RecentlyUsedSettings.Remove(ASettingsName);
            }

            // add the current setting at the front
            RecentlyUsedSettings.Insert(0, ASettingsName);

            // if too many settings, then remove the last one
            if (RecentlyUsedSettings.Count > MAX_NUMBER_OF_RECENT_SETTINGS)
            {
                RecentlyUsedSettings.RemoveAt(RecentlyUsedSettings.Count - 1);
            }

            TUserDefaults.SetDefault("RptStg" + FReportName, StringHelper.StrMerge(RecentlyUsedSettings, ','));
            return RecentlyUsedSettings;
        }

        /// <summary>
        /// This will check, if the set of settings with the given name is already existing
        /// and if it is a system settings; ie. it is provided by the organisation/OpenPetra.org and should not be overwritten.
        /// all settings in the user directory are non system settings, all in the {app}/reports30/Settings are system settings
        /// </summary>
        /// <returns>void</returns>
        public bool IsSystemSettings(String ASettingsName)
        {
            bool ReturnValue = false;

            // need to switch back to the application directory, because the path names might be relative to the application
            Environment.CurrentDirectory = FApplicationDirectory;
            String Filename = FSettingsDirectory + FReportName + System.IO.Path.DirectorySeparatorChar + ASettingsName + ".xml";

            if (System.IO.File.Exists(Filename))
            {
                TParameterList Parameters = new TParameterList();

                try
                {
                    Parameters.Load(Filename);

                    if (Parameters.Get("systemsettings").ToBool())
                    {
                        ReturnValue = true;
                    }
                }
                finally
                {
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Save the currently selected options and parameters to a file;
        /// this will automatically make this setting the most recent
        /// </summary>
        /// <returns>null if something went wrong (eg. overwrite a system settings file);
        /// otherwise a collection with the recently used settings, including the settings just saved
        /// </returns>
        public StringCollection SaveSettings(String ASettingsName, TParameterList AParameters)
        {
            if (IsSystemSettings(ASettingsName))
            {
                // cannot store/modify system settings, they are only provided by ICT
                // they can be saved as another setting
                return null;
            }

            string pathToFile = FUserSettingsDirectory + FReportName + System.IO.Path.DirectorySeparatorChar + ASettingsName + ".xml";

            try
            {
                if (!System.IO.Directory.Exists(FUserSettingsDirectory + FReportName))
                {
                    System.IO.Directory.CreateDirectory(FUserSettingsDirectory + FReportName);
                }

                AParameters.Save(pathToFile);
            }
            catch (Exception ex)
            {
                string msg = string.Format(Catalog.GetString(
                        "An error occurred while trying to save the settings to file location:{0}  {1}{0}The message from the system was: {2}"),
                    Environment.NewLine,
                    pathToFile,
                    ex.Message);
                MessageBox.Show(msg, Catalog.GetString("Save Settings"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return UpdateRecentlyUsedSettings(ASettingsName);
        }

        /// <summary>
        /// Load stored options and parameters from a file;
        /// this will automatically make this setting the most recent
        /// if the setting does not exist, it is removed from the list of recent settings, and ASettingsName is cleared
        ///
        /// </summary>
        /// <returns>void</returns>
        public StringCollection LoadSettings(ref String ASettingsName, ref TParameterList AParameters)
        {
            StringCollection ReturnValue;
            String path;

            // need to switch back to the application directory, because the path names might be relative to the application
            Environment.CurrentDirectory = FApplicationDirectory;
            path = FSettingsDirectory + FReportName + System.IO.Path.DirectorySeparatorChar + ASettingsName + ".xml";

            if (!System.IO.File.Exists(path))
            {
                path = FUserSettingsDirectory + FReportName + System.IO.Path.DirectorySeparatorChar + ASettingsName + ".xml";
            }

            if (System.IO.File.Exists(path))
            {
                AParameters.Load(path);

                // nobody needs to see this variable
                AParameters.RemoveVariable("systemsettings");

                ReturnValue = UpdateRecentlyUsedSettings(ASettingsName);
            }
            else
            {
                // setting does not exist anymore, and should disappear from the menu
                ReturnValue = UpdateRecentlyUsedSettings(ASettingsName);
                ReturnValue.Remove(ASettingsName);
                ASettingsName = "";
            }

            return ReturnValue;
        }

        /// <summary>
        /// Delete a setting
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DeleteSettings(String ASettingsName)
        {
            String Filename;

            if (IsSystemSettings(ASettingsName))
            {
                return;
            }

            Filename = FUserSettingsDirectory + FReportName + System.IO.Path.DirectorySeparatorChar + ASettingsName + ".xml";
            try
            {
                if (System.IO.File.Exists(Filename))
                {
                    System.IO.File.Delete(Filename);
                }
            }
            catch (System.Exception)
            {
                TLogging.Log("Could not delete " + Filename);

                // Messagebox.show('Error: Setting file could not be deleted!!!');
            }
        }

        /// <summary>
        /// Rename a setting
        ///
        /// </summary>
        /// <returns>void</returns>
        public bool RenameSettings(String AOldSettingsName, String ANewSettingsName)
        {
            bool ReturnValue;
            String OldFilename;
            String NewFilename;

            ReturnValue = true;

            if (IsSystemSettings(AOldSettingsName) || IsSystemSettings(ANewSettingsName))
            {
                return false;
            }

            OldFilename = FUserSettingsDirectory + FReportName + System.IO.Path.DirectorySeparatorChar + AOldSettingsName + ".xml";
            NewFilename = FUserSettingsDirectory + FReportName + System.IO.Path.DirectorySeparatorChar + ANewSettingsName + ".xml";
            try
            {
                System.IO.File.Move(OldFilename, NewFilename);
            }
            catch (System.Exception e)
            {
                TLogging.Log("Could not rename " + AOldSettingsName + ": " + e.Message);
                ReturnValue = false;
            }
            return ReturnValue;
        }

        #region settings for wrap column

        /// <summary>
        /// Get the user default if the Menu Item "Wrap Column" is checked
        /// </summary>
        /// <returns></returns>
        public bool GetWrapOption()
        {
            return TUserDefaults.GetBooleanDefault("RptSettingWrapColumn", true);
        }

        /// <summary>
        /// Set the user default if the Menu Item "Wrap Column" is checked
        /// </summary>
        /// <param name="AWrap"></param>
        public void SaveWrapOption(bool AWrap)
        {
            TUserDefaults.SetDefault("RptSettingWrapColumn", AWrap);
        }

        #endregion
    }
}