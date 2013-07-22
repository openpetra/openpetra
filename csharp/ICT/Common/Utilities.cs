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
using Ict.Common;
using System.Reflection;
using System.Windows.Forms;

namespace Ict.Common
{
    /// <summary>
    /// General utility functions for ICT applications that don't fall into other
    /// Units of the Ict.Common namespace.
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// Shorthand function that returns the current time in the HH:mm:ss format
        /// (24 hrs).
        ///
        /// </summary>
        /// <returns>String containing the formatted current time.
        /// </returns>
        public static String CurrentTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// Determines the Operating System on which this function is executed.
        ///
        /// </summary>
        /// <returns>A value of the TExecutingOS enumeration.</returns>
        public static TExecutingOSEnum DetermineExecutingOS()
        {
            System.Int32 PlatformIdentifier = (Int32)Environment.OSVersion.Platform;
            Version OSVersion = Environment.OSVersion.Version;

            switch (PlatformIdentifier)
            {
                case 4:
                case 128:
                    return TExecutingOSEnum.eosLinux;

                case (int)PlatformID.Win32Windows:
                    return TExecutingOSEnum.eosWin98ToWinME;

                case (int)PlatformID.Win32NT:

                    if (OSVersion.Major == 5)
                    {
                        return TExecutingOSEnum.eosWinXP;
                    }
                    else if ((OSVersion.Major == 6) && (OSVersion.Minor == 0))
                    {
                        return TExecutingOSEnum.eosWinVista;
                    }
                    else if ((OSVersion.Major == 6) && (OSVersion.Minor == 1))
                    {
                        return TExecutingOSEnum.eosWin7;
                    }
                    else if ((OSVersion.Major == 6) && (OSVersion.Minor == 2))
                    {
                        return TExecutingOSEnum.eosWin8;
                    }

                    return TExecutingOSEnum.eosWinNTOrLater;

                default:
                    return TExecutingOSEnum.oesUnsupportedPlatform;
            }
        }

        /// <summary>
        /// this discovers if we are running on Microsoft .net or Mono etc
        /// Common Language Runtime
        /// </summary>
        /// <returns>the runtime environment for .net</returns>
        public static TExecutingCLREnum DetermineExecutingCLR()
        {
            TExecutingCLREnum ReturnValue;
            Assembly mscorlibAssembly;

            mscorlibAssembly = typeof(System.Object).Assembly;

            if (mscorlibAssembly.GetType("Microsoft.Win32.Fusion") != null)
            {
                ReturnValue = TExecutingCLREnum.eclrMicrosoftDotNetFramework;
            }
            else if (mscorlibAssembly.GetType("Mono.Runtime") != null)
            {
                ReturnValue = TExecutingCLREnum.eclrMono;
            }
            else if (mscorlibAssembly.GetType("Platform.TaskMethods") != null)
            {
                ReturnValue = TExecutingCLREnum.eclrDotGNUPortableNet;
            }
            else
            {
                ReturnValue = TExecutingCLREnum.eclrUnknown;
            }

            return ReturnValue;
        }

        /// <summary>
        /// add a value to an existing array
        /// this will create a new array and copy all the elements and add the new value
        /// </summary>
        /// <param name="currentArray">existing array</param>
        /// <param name="newValue">new value</param>
        /// <returns>a new array with old values and the new value</returns>
        static public Object[] AddToArray(Object[] currentArray, Object newValue)
        {
            Object[] newArray = new Object[currentArray.Length + 1];

            for (Int32 counter = 0; counter < currentArray.Length; counter++)
            {
                newArray[counter] = currentArray[counter];
            }

            newArray[currentArray.Length] = newValue;
            return newArray;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="HtmlString">May be a whole document, or just a fragment.</param>
        public static void CopyHtmlToClipboard(String HtmlString)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // Build the CF_HTML header. See format specification here:
            // http://msdn.microsoft.com/library/default.asp?url=/workshop/networking/clipboard/htmlclipboard.asp

            // The string contains index references to other spots in the string, so we need placeholders so we can compute the offsets.
            // The <<<<<<<_ strings are just placeholders. I'll backpatch the actual values afterwards.
            sb.Append(
                @"Format:HTML Format
Version:1.0
StartHTML:<<<<<<<1
EndHTML:<<<<<<<2
StartFragment:<<<<<<<3
EndFragment:<<<<<<<4
StartSelection:<<<<<<<3
EndSelection:<<<<<<<4
SourceURL:OpenPetra
"                                                                                                                                                                                                            );
            int startHTML = sb.Length;
            int fragmentEnd;
            int fragmentStart = HtmlString.ToLower().IndexOf("<body>");

            if (fragmentStart < 0)
            {
                sb.Append(
                    @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN""><HTML><HEAD><TITLE>From clipboard</TITLE></HEAD><BODY><!--StartFragment-->");
                fragmentStart = sb.Length;

                fragmentEnd = sb.Length;

                sb.Append(@"<!--EndFragment--></BODY></HTML>");
            }
            else
            {
                fragmentStart = fragmentStart + 6 + sb.Length;
                fragmentEnd = HtmlString.ToLower().IndexOf("</body>") + sb.Length;
                sb.Append(HtmlString);
            }

            int endHTML = sb.Length;


            // Backpatch offsets
            sb.Replace("<<<<<<<1", startHTML.ToString("D8"));
            sb.Replace("<<<<<<<2", endHTML.ToString("D8"));
            sb.Replace("<<<<<<<3", fragmentStart.ToString("D8"));
            sb.Replace("<<<<<<<4", fragmentEnd.ToString("D8"));


            // Finally copy to clipboard.
            string data = sb.ToString();
            Clipboard.Clear();
            Clipboard.SetText(data, TextDataFormat.Html);
        }
    }
}