//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2012 by OM International
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
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Ict.Common
{
    /// <summary>
    /// The TAppSettingsManager class allows reading of AppSettings section values
    /// in any .NET Configuration File (not just the one that has the same name as
    /// the Application itself).
    /// It also supports parameters on the command line which override the values in the config file.
    /// </summary>
    public class TAppSettingsManager
    {
        /// <summary>
        /// constant for undefined value
        /// </summary>
        public const String UNDEFINEDVALUE = "#UNDEFINED#";

        /// <summary>The path where the application is started from.</summary>
        private static String FApplicationDirectory = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

        /// <summary>The name of the Configuration File that should be read from; static so it can be manipulated manually once for all (remoting nunit etc.)</summary>
        private static String FConfigFileName = "";

        /// <summary>XML Element under which the AppSettings are found</summary>
        private static XmlElement FAppSettingsElement = null;
        private static TCmdOpts FCmdOpts;

        /// <summary>
        /// read only property for the filename of the current config file
        /// </summary>
        public static string ConfigFileName
        {
            get
            {
                return FConfigFileName;
            }
        }

        /// <summary>
        /// read only property for the directory where all the binary exe and dll files are
        /// </summary>
        public static string ApplicationDirectory
        {
            get
            {
                if (FApplicationDirectory.StartsWith("file:\\"))
                {
                    FApplicationDirectory = FApplicationDirectory.Substring("file:\\".Length);
                }
                else if (FApplicationDirectory.StartsWith("file:/"))
                {
                    FApplicationDirectory = FApplicationDirectory.Substring("file:".Length);
                }

                return FApplicationDirectory;
            }
        }

        #region TAppSettingsManager

        /// <summary>
        /// this should be called by the constructors only;
        /// in C# one constructor cannot call another constructor, so we have moved the code to a different method
        /// </summary>
        /// <param name="CustomConfigFileName">Name of the .NET Configuration File to read from</param>
        /// <param name="AFailOnMissingConfigFile">if this is true and there is no config file, an exception is raised</param>
        /// <returns>void</returns>
        private void Create(String CustomConfigFileName, bool AFailOnMissingConfigFile)
        {
            FCmdOpts = new TCmdOpts();

            if (CustomConfigFileName != "")
            {
                FConfigFileName = CustomConfigFileName;
            }
            else if (FCmdOpts.IsFlagSet("C") == true)
            {
                FConfigFileName = FCmdOpts.GetOptValue("C");
            }
            else if (FConfigFileName.Length == 0) // don't reset the name if it was set manually before
            {
                if (Environment.GetCommandLineArgs().Length == 0)
                {
                    // somehow the Environment commandlineargs are empty during debugging
                    throw new Exception("During debugging, please use the parameter -C for the config file");
                }
                else
                {
                    FConfigFileName = Environment.GetCommandLineArgs()[0] + ".config";
                }
            }

            LoadCustomAppSettingFile(AFailOnMissingConfigFile);
        }

        /// <summary>
        /// Create a TAppSettingsManager that reads AppSettings values from the
        /// specified .NET Configuration File.
        ///
        /// </summary>
        /// <param name="CustomConfigFileName">Name of the .NET Configuration File to read from
        /// </param>
        /// <returns>void</returns>
        public TAppSettingsManager(String CustomConfigFileName)
        {
            Create(CustomConfigFileName, true);
        }

        /// <summary>
        /// Create a TAppSettingsManager that reads AppSettings values from the
        /// Application's standard .NET Configuration File.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TAppSettingsManager()
        {
            Create("", true);
        }

        /// <summary>
        /// Create a TAppSettingsManager that reads AppSettings values from the
        /// Application's standard .NET Configuration File.
        ///
        /// </summary>
        /// <param name="AFailOnMissingConfigFile">if this is true and there is no config file, an exception is raised</param>
        /// <returns>void</returns>
        public TAppSettingsManager(bool AFailOnMissingConfigFile)
        {
            Create("", AFailOnMissingConfigFile);
        }

        /// <summary>
        /// Read AppSetting values from the a .NET Configuration File.
        ///
        /// </summary>
        /// <param name="AFailOnMissingConfigFile">if this is true and there is no config file, an exception is raised; otherwise it returns false</param>
        /// <returns>TRUE if reading of the .NET Configuration File succeeded.</returns>
        /// <exception cref="ApplicationException">Raised when it is not possible to load or
        /// XML-parse the .NET Configuration File.
        /// </exception>
        private bool LoadCustomAppSettingFile(bool AFailOnMissingConfigFile)
        {
            XmlDocument xml;

            if (!System.IO.File.Exists(FConfigFileName) && System.IO.File.Exists(FConfigFileName.Replace(".config", ".config.sample")))
            {
                System.IO.File.Copy(FConfigFileName.Replace(".config", ".config.sample"), FConfigFileName);
            }

            if (System.IO.File.Exists(FConfigFileName))
            {
                try
                {
                    xml = new XmlDocument();
                    xml.Load(FConfigFileName);
                    FAppSettingsElement = (XmlElement)xml.SelectSingleNode("//configuration/appSettings");

                    if (FAppSettingsElement == null)
                    {
                        FAppSettingsElement = xml.CreateElement("appSettings");
                        xml.DocumentElement.AppendChild(FAppSettingsElement);
                    }
                }
                catch (Exception exp)
                {
                    throw new ApplicationException("Could not XML-parse the configuration file '" + FConfigFileName + "'.", exp);
                }
            }
            else if (AFailOnMissingConfigFile)
            {
                throw new ApplicationException("Unable to load the configuration file '" + FConfigFileName + "'.");
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// returns true if the value is on the command line or in the config file
        /// </summary>
        /// <returns>void</returns>
        public static Boolean HasValue(String Key)
        {
            Boolean ReturnValue;
            XmlElement appsetting;

            ReturnValue = false;

            if (FCmdOpts.IsFlagSet(Key))
            {
                ReturnValue = true;
            }
            else if (FAppSettingsElement != null)
            {
                appsetting = (XmlElement)FAppSettingsElement.SelectSingleNode("add[@key='" + Key + "']");

                if (appsetting != null)
                {
                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the Value of a specified AppSetting key.
        ///
        /// </summary>
        /// <param name="AKey">Key of the AppSetting</param>
        /// <param name="ALogErrorIfNotPresent"></param>
        /// <returns>Value of the AppSetting
        /// </returns>
        public static String GetValue(String AKey, Boolean ALogErrorIfNotPresent)
        {
            String ReturnValue;
            XmlElement appsetting = null;

            ReturnValue = UNDEFINEDVALUE;

            if (FCmdOpts.IsFlagSet(AKey))
            {
                ReturnValue = FCmdOpts.GetOptValue(AKey);
            }
            else
            {
                if (FAppSettingsElement != null)
                {
                    appsetting = (XmlElement)FAppSettingsElement.SelectSingleNode("add[@key='" + AKey + "']");
                }

                if (appsetting != null)
                {
                    ReturnValue = appsetting.GetAttribute("value");
                    TLogging.Log("AppSettings: " + AKey + " = " + ReturnValue);
                }
                else
                {
                    if (ALogErrorIfNotPresent)
                    {
                        try
                        {
                            TLogging.Log("Cannot find " + AKey + " in command line options or in the config file " + FConfigFileName,
                                TLoggingType.ToConsole | TLoggingType.ToLogfile);
                        }
                        catch (TNoLoggingToFile_WrongConstructorUsedException)
                        {
                            // ignore this Exception; it is thrown if TLogging was not initialised yet, which means no Log file is specified.
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }

            ReturnValue = ReturnValue.Replace("{userappdata}",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            ReturnValue = ReturnValue.Replace("{applicationbindir}", ApplicationDirectory);

            return ReturnValue;
        }

        /// <summary>
        /// returns the string value of a parameter, either from command line or from the current config file
        /// </summary>
        /// <param name="AKey">the name of the parameter</param>
        /// <returns>returns the string value</returns>
        public static String GetValue(String AKey)
        {
            return GetValue(AKey, true);
        }

        /// <summary>
        /// returns the string value of a parameter or the default value
        /// </summary>
        /// <param name="AKey">the name of the parameter</param>
        /// <param name="ADefaultValue">the default value in case the parameter cannot be found</param>
        /// <returns>returns the string value or the default value</returns>
        public static String GetValue(String AKey, String ADefaultValue)
        {
            string ReturnValue = GetValue(AKey, false);

            if (ReturnValue == UNDEFINEDVALUE)
            {
                ReturnValue = ADefaultValue;
            }

            return ReturnValue;
        }

        /// <summary>
        /// returns the string value of a parameter or the default value
        /// </summary>
        /// <param name="AKey">the name of the parameter</param>
        /// <param name="ADefaultValue">the default value in case the parameter cannot be found</param>
        /// <param name="ALogErrorIfNotPresent"></param>
        /// <returns>returns the string value or the default value</returns>
        public static String GetValue(String AKey, String ADefaultValue, Boolean ALogErrorIfNotPresent)
        {
            string ReturnValue = GetValue(AKey, ALogErrorIfNotPresent);

            if (ReturnValue == UNDEFINEDVALUE)
            {
                ReturnValue = ADefaultValue;
            }

            return ReturnValue;
        }

        /// <summary>
        /// return the Integer value of a parameter or its default value
        /// </summary>
        /// <param name="AKey">the name of the parameter</param>
        /// <param name="ADefaultValue">the default value in case the parameter cannot be found</param>
        /// <returns>the value of the parameter, or the default value</returns>
        public static System.Int16 GetInt16(String AKey, System.Int16 ADefaultValue)
        {
            System.Int16 ReturnValue;
            ReturnValue = ADefaultValue;
            try
            {
                string str = GetValue(AKey, (ADefaultValue == -1));
                ReturnValue = Convert.ToInt16(str);
            }
            catch (Exception)
            {
                if (ADefaultValue == -1)
                {
                    // Caller wanted the Value and didn't specify a Default: log that
                    TLogging.Log("Problem reading int16 value from key " + AKey + " from config file.", TLoggingType.ToLogfile);
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// read an integer value
        /// </summary>
        /// <param name="AKey">the name of the parameter</param>
        /// <returns>the value of the parameter or -1 if the parameter does not exist on the command line or in the config file</returns>
        public static System.Int16 GetInt16(String AKey)
        {
            return GetInt16(AKey, -1);
        }

        /// <summary>
        /// Return the Integer value of a parameter or its default value
        /// </summary>
        /// <param name="AKey">the name of the parameter</param>
        /// <param name="ADefaultValue">the default value in case the parameter cannot be found</param>
        /// <returns>the value of the parameter, or the default value</returns>
        public static System.Int32 GetInt32(String AKey, System.Int32 ADefaultValue)
        {
            System.Int32 ReturnValue;
            ReturnValue = ADefaultValue;

            if (!Int32.TryParse(GetValue(AKey, (ADefaultValue == -1)), out ReturnValue))
            {
                if (ADefaultValue == -1)
                {
                    // Caller wanted the Value and didn't specify a Default: log that
                    TLogging.Log("Problem reading Int32 value from key " + AKey + " from config file.", TLoggingType.ToLogfile);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// read an integer value
        /// </summary>
        /// <param name="AKey">the name of the parameter</param>
        /// <returns>the value of the parameter or -1 if the parameter does not exist on the command line or in the config file</returns>
        public static System.Int32 GetInt32(String AKey)
        {
            return GetInt32(AKey, -1);
        }

        /// <summary>
        /// return the Integer value of a parameter or its default value
        /// </summary>
        /// <param name="AKey">the name of the parameter</param>
        /// <param name="ADefaultValue">the default value in case the parameter cannot be found</param>
        /// <returns>the value of the parameter, or the default value</returns>
        public static System.Int64 GetInt64(String AKey, System.Int64 ADefaultValue)
        {
            System.Int64 ReturnValue;
            ReturnValue = ADefaultValue;

            if (!Int64.TryParse(GetValue(AKey, (ADefaultValue == -1)), out ReturnValue))
            {
                if (ADefaultValue == -1)
                {
                    // Caller wanted the Value and didn't specify a Default: log that
                    TLogging.Log("Problem reading Int64 value from key " + AKey + " from config file.", TLoggingType.ToLogfile);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// read an integer value
        /// </summary>
        /// <param name="AKey">the name of the parameter</param>
        /// <returns>the value of the parameter or -1 if the parameter does not exist on the command line or in the config file</returns>
        public static System.Int64 GetInt64(String AKey)
        {
            return GetInt64(AKey, -1);
        }

        /// <summary>
        /// read a float value
        /// </summary>
        /// <param name="AKey">the name of the parameter</param>
        /// <returns>the value of the parameter or -1.0 if the parameter does not exist on the command line or in the config file</returns>
        public static float GetFloat(String AKey)
        {
            float ReturnValue = -1.0f;

            if (!float.TryParse(GetValue(AKey), out ReturnValue))
            {
                // Caller wanted the Value and didn't specify a Default: log that
                TLogging.Log("Problem reading Double value from key " + AKey + " from config file.", TLoggingType.ToLogfile);
            }

            return ReturnValue;
        }

        /// <summary>
        /// read a double value
        /// </summary>
        /// <returns>the value of the parameter or -1.0 if the parameter does not exist on the command line or in the config file</returns>
        public static double GetDouble(String AKey, double ADefault)
        {
            double ReturnValue = -1.0f;

            if (!double.TryParse(GetValue(AKey), out ReturnValue))
            {
                ReturnValue = ADefault;
            }

            return ReturnValue;
        }

        /// <summary>
        /// return the Boolean value of a parameter or its default value
        /// </summary>
        /// <param name="AKey">the name of the parameter</param>
        /// <param name="ADefaultValue">the default value in case the parameter cannot be found</param>
        /// <returns>the value of the parameter, or the default value</returns>
        public static bool GetBoolean(String AKey, bool ADefaultValue)
        {
            bool ReturnValue = ADefaultValue;

            try
            {
                // never complain about missing boolean parameters; there is always a default value
                ReturnValue = ToBoolean(GetValue(AKey, false), ADefaultValue);
            }
            catch (Exception)
            {
                TLogging.Log("Problem reading Boolean value from key " + AKey + " from config file.", TLoggingType.ToLogfile);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Converts a value given as a string to boolean.
        /// If the value cannot be interpreted, the default value is returned.
        ///
        /// </summary>
        /// <param name="AValue">the value as a string (true|false|yes|no|0|1)</param>
        /// <param name="ADefaultValue">default value for the situation that AValue does not have a valid value</param>
        /// <returns>boolean value
        /// </returns>
        public static bool ToBoolean(String AValue, bool ADefaultValue)
        {
            bool ReturnValue;

            AValue = AValue.ToLower();
            ReturnValue = ADefaultValue;

            if ((AValue == "true") || (AValue == "yes") || (AValue == "1"))
            {
                ReturnValue = true;
            }
            else if ((AValue == "false") || (AValue == "no") || (AValue == "0"))
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        #endregion

        #region FontSettingsI18N

        /// <summary>needed for internationalisation; we cannot hardcode the fonts</summary>
        private static Font defaultBoldFont;

        /// <summary>needed for internationalisation; we cannot hardcode the fonts</summary>
        private static Font defaultUIFont;

        /// <summary>needed for internationalisation; we cannot hardcode the fonts; this variable defines whether the fonts need to be changed at all</summary>
        private static Boolean changeFonts;

        /// <summary>
        /// needed for internationalisation; for western systems, we don't need to change the fonts
        /// other systems need different fonts, and also different behaviour (eg. no bold fonts to improve readability)
        /// </summary>
        /// <returns>whether the fonts need to be changed for internationalisation</returns>
        public static Boolean ChangeFontForLocalisation()
        {
            return changeFonts;
        }

        /// <summary>
        /// needed for internationalisation;
        /// some languages cannot be displayed in bold letters, because they have so detailed letters
        /// and the user would not be able to read them in bold letters
        /// </summary>
        /// <returns>which font to use for bold letters</returns>
        public static Font GetDefaultBoldFont()
        {
            return defaultBoldFont;
        }

        /// <summary>
        /// same as GetDefaultBoldFont, but with option to create the font in a certain size
        /// </summary>
        /// <param name="ASize">required size for the font</param>
        /// <returns>appropriate font for bold letters</returns>
        public static Font GetDefaultBoldFont(float ASize)
        {
            return new Font(defaultBoldFont.Name, ASize, System.Drawing.FontStyle.Bold);
        }

        /// <summary>
        /// Initialise the bold font for the current character setting
        /// </summary>
        /// <param name="AFont">the font that should be used for bold characters</param>
        public static void SetDefaultBoldFont(Font AFont)
        {
            defaultBoldFont = AFont;
        }

        /// <summary>
        /// return the font that should usually be used on the forms
        /// </summary>
        /// <returns>appropriate font for general UI</returns>
        public static Font GetDefaultUIFont()
        {
            return defaultUIFont;
        }

        /// <summary>
        /// set the default font that should be used in general
        /// </summary>
        /// <param name="AFont">appropriate font for the current character settings</param>
        public static void SetDefaultUIFont(Font AFont)
        {
            defaultUIFont = AFont;
        }

        /// <summary>
        /// test if we need to replace the given font with the default UI font
        /// </summary>
        /// <param name="AFont">the font that needs to be evaluated</param>
        /// <returns>true if this is the default font from the GUI designer, and we want to replace it</returns>
        public static Boolean ReplaceFont(Font AFont)
        {
            return (AFont.Name == "Verdana") || (AFont.Name == "Tahoma");
        }

        /// <summary>
        /// Initialises the font settings by using values from the config file,
        /// but also checking the Windows environment to find out whether this is a western or asian system
        /// </summary>
        /// <returns>the default font to be used</returns>
        public static Font InitFontI18N()
        {
            TextBox txt = new TextBox();
            Font WindowsFont = txt.Font;

            Font ReturnValue;

            // set the font in the config file; or try to figure it out from the Windows XP system font (WindowsFont)

            if (TAppSettingsManager.HasValue("FontName") && TAppSettingsManager.HasValue("FontSize"))
            {
                changeFonts = true;
                ReturnValue = new System.Drawing.Font(
                    TAppSettingsManager.GetValue("FontName"),
                    TAppSettingsManager.GetFloat("FontSize"));
                SetDefaultUIFont(ReturnValue);

                if (TAppSettingsManager.HasValue("FontAllowBold") && (TAppSettingsManager.GetBoolean("FontAllowBold", false) == true))
                {
                    SetDefaultBoldFont(new System.Drawing.Font(
                            TAppSettingsManager.GetValue("FontName"),
                            TAppSettingsManager.GetFloat("FontSize"),
                            System.Drawing.FontStyle.Bold));
                }
                else
                {
                    SetDefaultBoldFont(ReturnValue);
                }
            }
            // just set the font size in the config file, without specifying the font; will use the default Windows font
            else if ((!TAppSettingsManager.HasValue("FontName") && TAppSettingsManager.HasValue("FontSize")))
            {
                changeFonts = true;
                ReturnValue = new System.Drawing.Font(WindowsFont.Name, TAppSettingsManager.GetFloat("FontSize"));
                SetDefaultUIFont(ReturnValue);

                if (TAppSettingsManager.HasValue("FontAllowBold") && (TAppSettingsManager.GetBoolean("FontAllowBold", false) == true))
                {
                    SetDefaultBoldFont(new System.Drawing.Font(
                            TAppSettingsManager.GetValue("FontName"),
                            TAppSettingsManager.GetFloat("FontSize"),
                            System.Drawing.FontStyle.Bold));
                }
                else
                {
                    SetDefaultBoldFont(ReturnValue);
                }
            }
            else if (WindowsFont.Name == "Microsoft Sans Serif")
            {
                // japanese winxp: MS UI Gothic
                // english winxp: Microsoft Sans Serif

                // this is a western / english Windows XP
                changeFonts = false;
                ReturnValue = new System.Drawing.Font("Verdana", 8.25f);
                SetDefaultUIFont(ReturnValue);
                SetDefaultBoldFont(
                    new System.Drawing.Font("Verdana",
                        8.25f,
                        System.Drawing.FontStyle.Bold,
                        System.Drawing.GraphicsUnit.Point,
                        0));
            }
            else
            {
                // this might be an asian windows; no special font for bold because that makes it unreadable, e.g. chinese or japanese
                ReturnValue = new System.Drawing.Font(WindowsFont.Name, 12.0f);
                changeFonts = true;
                SetDefaultBoldFont(ReturnValue);
                SetDefaultUIFont(ReturnValue);
            }

            return ReturnValue;
        }

        #endregion
    }
}