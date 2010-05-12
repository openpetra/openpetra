//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Common;
using System.Reflection;

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
            TExecutingOSEnum ReturnValue;

            System.Int32 PlatformIdentifier;
            PlatformIdentifier = (Int32)Environment.OSVersion.Platform;

            switch (PlatformIdentifier)
            {
                case 4:
                case 128:
                    ReturnValue = TExecutingOSEnum.eosLinux;
                    break;

                case (int)PlatformID.Win32Windows:
                    ReturnValue = TExecutingOSEnum.eosWin98ToWinME;
                    break;

                case (int)PlatformID.Win32NT:
                    ReturnValue = TExecutingOSEnum.eosWinNTOrLater;
                    break;

                default:
                    ReturnValue = TExecutingOSEnum.oesUnsupportedPlatform;
                    break;
            }

            return ReturnValue;
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
    }
}