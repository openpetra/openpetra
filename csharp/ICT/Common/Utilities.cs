//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2015 by OM International
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
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
        /// String that gets utilised by Method <see cref="GetThreadAndAppDomainCallInfo"/> but can also be utilised
        /// elsewhere.
        /// </summary>
        public const string StrThreadAndAppDomainCallInfo = "(Call performed in Thread {0} in AppDomain '{1}')";

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
                    //Applications not manifested for Windows 8.1 or Windows 10 will return the Windows 8 OS version value (6.2)
                    else if ((OSVersion.Major == 6) && (OSVersion.Minor == 2))
                    {
                        return TExecutingOSEnum.eosWin8Plus;
                    }
                    //The next two only occur if the application is manifested for Win8.1 and Win10 respectively
                    else if ((OSVersion.Major == 6) && (OSVersion.Minor == 3))
                    {
                        return TExecutingOSEnum.eosWin81;
                    }
                    else if ((OSVersion.Major == 10) && (OSVersion.Minor == 0))
                    {
                        return TExecutingOSEnum.eosWin10;
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
        /// Return the name of the calling method
        /// </summary>
        /// <param name="ASplitPascalName"></param>
        /// <returns></returns>
        public static string GetMethodName(bool ASplitPascalName = false)
        {
            string RetVal = string.Empty;

            try
            {
                StackTrace stackTrace = new StackTrace();
                MethodBase methodBase = stackTrace.GetFrame(1).GetMethod();

                RetVal = "[" + methodBase.Name + "]";
            }
            catch
            {
                //Do nothing as this may not always work at run time
            }

            if (ASplitPascalName && (RetVal.Length > 0))
            {
                SplitPascalName(ref RetVal);
            }

            return RetVal;
        }

        /// <summary>
        /// Returns a formatted string that contains information about the current Thread and the AppDomain in which
        /// the code gets executed. Useful for logging!
        /// </summary>
        /// <returns>Formatted string that contains information about the current Thread and the AppDomain in which
        /// the code gets executed.</returns>
        public static string GetThreadAndAppDomainCallInfo()
        {
            return String.Format(StrThreadAndAppDomainCallInfo, ThreadingHelper.GetCurrentThreadIdentifier(),
                AppDomain.CurrentDomain.FriendlyName);
        }

        private static void SplitPascalName(ref string APascalCaseString)
        {
            //APascalCaseString = Regex.Replace(APascalCaseString, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
            var arr = Regex.Matches(APascalCaseString, @"[A-Z]+(?=[A-Z][a-z]+)|\d|[A-Z][a-z]+").Cast <Match>().Select(m => m.Value).ToArray();

            APascalCaseString = String.Join(" ", arr);
        }

        /// <summary>
        /// Return the name and signature of the calling method
        /// </summary>
        /// <returns></returns>
        public static string GetMethodSignature()
        {
            string RetVal = string.Empty;

            try
            {
                StackTrace stackTrace = new StackTrace();
                MethodBase methodBase = stackTrace.GetFrame(1).GetMethod();
                ParameterInfo[] arguments = methodBase.GetParameters();

                string argumentList = string.Empty;

                if (arguments.Length > 0)
                {
                    argumentList = string.Format("{0}({1})",
                        methodBase.Name,
                        string.Join(", ", (arguments.Select(x => (!x.ParameterType.IsByRef ? (x.Attributes + " ") : (x.IsOut ? "out " : "ref ")) +
                                               x.ParameterType.ToString().Substring(x.ParameterType.ToString().LastIndexOf(".") + 1) + " " + x.Name))));

                    argumentList = argumentList.Replace("(None ", "(").Replace(", None ", ", ").Replace("& ", " ");

                    MethodInfo methodInfo = (MethodInfo)methodBase;

                    if (methodInfo != null)
                    {
                        RetVal = (methodInfo.ReturnType.IsPublic ? "public " : "private ") + (methodBase.IsStatic ? "static " : string.Empty) +
                                 methodInfo.ReturnType.Name.Replace("Void", "void") + " " + argumentList;
                    }
                    else
                    {
                        RetVal = argumentList;
                    }
                }
                else
                {
                    RetVal = methodBase.ToString();

                    //Don't do a String.Replace just in case the method name contains the word Void, e.g. VoidTransaction
                    if (RetVal.StartsWith("Void "))
                    {
                        RetVal = "void " + RetVal.Substring(5);
                    }
                }
            }
            catch
            {
                RetVal = "**Unable to indentify**";
            }

            RetVal = "[" + RetVal + "]";

            return RetVal;
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
                "Format:HTML Format" + Environment.NewLine +
                "Version:1.0" + Environment.NewLine +
                "StartHTML:<<<<<<<1" + Environment.NewLine +
                "EndHTML:<<<<<<<2" + Environment.NewLine +
                "StartFragment:<<<<<<<3" + Environment.NewLine +
                "EndFragment:<<<<<<<4" + Environment.NewLine +
                "StartSelection:<<<<<<<3" + Environment.NewLine +
                "EndSelection:<<<<<<<4" + Environment.NewLine +
                "SourceURL:OpenPetra" + Environment.NewLine);

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