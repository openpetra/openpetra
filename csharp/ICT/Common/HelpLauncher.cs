//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Diagnostics;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Runtime.Serialization;

using Ict.Common.Exceptions;

namespace Ict.Common
{
    /// <summary>
    /// Allows the opening of a help topic in the OS's Default Web Browser.
    /// </summary>
    public static class HelpLauncher
    {
        private static TDetermineHelpTopic FDetermineHelpTopicDelegate;
        private static string FHelpHTMLBaseURL = String.Empty;
        private static string FHelpLanguage = "en";
        private static bool FLocalHTMLHelp;

        /// <summary>
        /// Delegate for determinig a help topic for a given Form and Control.
        /// </summary>
        public delegate string TDetermineHelpTopic(System.Object Sender, TDetermineHelpTopicEventArgs AEventArgs);

        /// <summary>
        /// Event Arguments for a TDetermineHelpTopic Delegate
        /// </summary>
        public class TDetermineHelpTopicEventArgs : System.EventArgs
        {
            private Form FHelpContextForm;
            private Control FHelpContextControl;

            /// <summary>
            /// Form that specifies the Help Context.
            /// </summary>
            public Form HelpContextForm
            {
                get
                {
                    return FHelpContextForm;
                }

                set
                {
                    FHelpContextForm = value;
                }
            }

            /// <summary>
            /// Control that specifies the Help Context.
            /// </summary>
            public Control HelpContextControl
            {
                get
                {
                    return FHelpContextControl;
                }

                set
                {
                    FHelpContextControl = value;
                }
            }

            /// <summary>
            /// constructor
            /// </summary>
            /// <returns>void</returns>
            public TDetermineHelpTopicEventArgs() : base()
            {
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="AHelpContextForm">Form that specifies the Help Context.</param>
            /// <param name="AHelpContextControl">Control that specifies the Help Context.</param>
            public TDetermineHelpTopicEventArgs(Form AHelpContextForm, Control AHelpContextControl) : base()
            {
                FHelpContextForm = AHelpContextForm;
                FHelpContextControl = AHelpContextControl;
            }
        }

        /// <summary>
        /// Delegate for determinig a help topic for a given Form and Control.
        /// </summary>
        public static TDetermineHelpTopic DetermineHelpTopic
        {
            get
            {
                return FDetermineHelpTopicDelegate;
            }

            set
            {
                FDetermineHelpTopicDelegate = value;
            }
        }

        /// <summary>
        /// URL to the HTML pages for the Application Help.
        /// </summary>
        public static string HelpHTMLBaseURL
        {
            get
            {
                return FHelpHTMLBaseURL;
            }

            set
            {
                FHelpHTMLBaseURL = value;
            }
        }

        /// <summary>
        /// Language that the help should be displayed in (default='en' [English]).
        /// </summary>
        public static string HelpLanguage
        {
            get
            {
                return FHelpLanguage;
            }

            set
            {
                // Only use the main denominator of a Language (e.g. 'en' if the language passed in is 'en-GB')
                if (value.IndexOf('-') == -1)
                {
                    FHelpLanguage = value;
                }
                else
                {
                    FHelpLanguage = value.Substring(0, value.IndexOf('-'));
                }

                // TODO: Somehow we need to prevent setting this value to a language in which no application help exists... In such a case, the fallback should be 'en'.
            }
        }

        /// <summary>
        /// Set to true to lauch the help topics from local HTML files, or to false to lauch them from an online location.
        /// </summary>
        public static bool LocalHTMLHelp
        {
            get
            {
                return FLocalHTMLHelp;
            }

            set
            {
                FLocalHTMLHelp = value;
            }
        }

        /// <summary>
        /// Opens a help topic in a Web Browser.
        /// </summary>
        /// <remarks>
        /// The help topic that should be openend is determined by
        /// <paramref name="AForm"/> and <paramref name="AControl"/>, except if <paramref name="AManualTopic" />
        /// is specified.
        /// </remarks>
        /// <param name="AForm">Form that specifies the Help Context.</param>
        /// <param name="AControl">Control that specifies the Help Context.</param>
        /// <param name="AManualTopic">If specified, this overrides the detection of a help topic by
        /// <paramref name="AForm"/> and <paramref name="AControl"/>. Instead, the passed in value
        /// will be taken as the help topic.</param>
        /// <returns>True if a help topic was found, false if not.</returns>
        public static bool ShowHelp(Form AForm, Control AControl, string AManualTopic = "")
        {
            bool ReturnValue = true;
            string HelpTopic = AManualTopic;

            if ((AForm == null)
                && (AManualTopic == String.Empty))
            {
                throw new ArgumentException("AForm must not be null if AManualTopic isn't specified");
            }

            if ((AControl == null)
                && (AForm == null)
                && (AManualTopic == String.Empty))
            {
                throw new ArgumentException("AControl must not be null if AForm and AManualTopic aren't specified");
            }

            if (HelpTopic != String.Empty)
            {
                LauchHelpTopicInBrowser(AForm, HelpTopic);
            }
            else
            {
                HelpTopic = OnDetermineHelpTopic(new TDetermineHelpTopicEventArgs(AForm, AControl));

                if (HelpTopic != String.Empty)
                {
                    if (!LauchHelpTopicInBrowser(AForm, HelpTopic))
                    {
                        throw new EHelpLauncherException(Catalog.GetString("Application Help could not be opened.\r\n\r\n" +
                                "Reason: The URL that points to the help files is not configured in the\r\nClient Config file ('HTMLHelpBaseURLLocal' and 'HTMLHelpBaseURLOnInternet' settings)."));
                    }
                }
                else
                {
                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Launches the DetermineHelpTopic Delegate, if it is set.
        /// </summary>
        /// <param name="e">Event Arguments.</param>
        /// <returns>Return value from Delegate call.</returns>
        private static string OnDetermineHelpTopic(TDetermineHelpTopicEventArgs e)
        {
            if (FDetermineHelpTopicDelegate != null)
            {
                return FDetermineHelpTopicDelegate(null, e);
            }
            else
            {
                throw new EOPAppException("DEVELOPER NEEDS TO FIX THIS: Delegate for determining a Help Topic is not set!");
            }
        }

        /// <summary>
        /// Opens an URL in the OS's 'Default Browser'.
        /// </summary>
        /// <param name="AForm">Instance of a Form that is trying to launch the Help Topic.</param>
        /// <param name="AURL">URL to open (can include Anchors!).</param>
        private static bool LauchHelpTopicInBrowser(Form AForm, string AURL)
        {
            const char PathSeparator = '/';
            Cursor FormCursorAtTimeOfCall = null;
            Process WebBrowserProcess = new Process();
            Version ApplicationVersion = new Version(Application.ProductVersion);

            if (FHelpHTMLBaseURL == String.Empty)
            {
                return false;
            }

            if (FLocalHTMLHelp)
            {
                AURL = FHelpHTMLBaseURL + PathSeparator + FHelpLanguage + PathSeparator +
                       "content" + PathSeparator + AURL;
            }
            else
            {
                AURL = FHelpHTMLBaseURL + PathSeparator +
                       ApplicationVersion.Major + "_" + ApplicationVersion.Minor + "_" + ApplicationVersion.Build + PathSeparator +
                       FHelpLanguage + PathSeparator +
                       "content" + PathSeparator + AURL;
            }

//			MessageBox.Show("AURL: " + AURL);

            // Show 'Application Staring' Cursor
            FormCursorAtTimeOfCall = AForm.Cursor;
            AForm.Cursor = Cursors.AppStarting;

            // Start .EXE of the Browser, passing in the URL as startup arguments
            WebBrowserProcess.StartInfo.FileName = GetDefaultBrowser();
            WebBrowserProcess.StartInfo.Arguments = AURL;
            WebBrowserProcess.Start();

            // Reset to originally shown Cursor
            AForm.Cursor = FormCursorAtTimeOfCall;

            return true;
        }

        /// <summary>
        /// Determines the OS's 'Default Browser' Executable.
        /// </summary>
        /// <remarks>TODO: Make this Method work on OS's other than Windows!</remarks>
        /// <returns>File name of the OS's 'Default Browser'.</returns>
        private static string GetDefaultBrowser()
        {
            string BrowserExe = string.Empty;
            RegistryKey RegKey = null;

            try
            {
                // TODO: Make this Method work on OS's other than Windows!
                RegKey = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                // Trim off quotes
                BrowserExe = RegKey.GetValue(null).ToString().ToLower().Replace("\"", "");

                if (!BrowserExe.EndsWith("exe"))
                {
                    // Get rid of everything after the ".exe"
                    BrowserExe = BrowserExe.Substring(0, BrowserExe.LastIndexOf(".exe") + 4);
                }
            }
            finally
            {
                if (RegKey != null)
                {
                    RegKey.Close();
                }
            }

            return BrowserExe;
        }
    }

    #region Exceptions

    /// <summary>
    /// Thrown by ShowHelp Method if the Application Help could not be opened.
    /// commands.
    /// </summary>
    [Serializable()]
    public class EHelpLauncherException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EHelpLauncherException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EHelpLauncherException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EHelpLauncherException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EHelpLauncherException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion
}