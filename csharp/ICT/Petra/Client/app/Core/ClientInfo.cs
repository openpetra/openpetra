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

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// The TClientInfo class holds information about the currently running instance
    /// of the Petra Client.
    ///
    /// Currently hold only very basic information, will be extended in the future.
    /// </summary>
    public class TClientInfo
    {
        private static TExecutingOSEnum UClientOS;
        private static String UClientIPAddress;
        private static String UClientComputerName;
        private static String UClientAssemblyVersion;
        private static String UInstallationKind;

        /// <summary>
        /// todoComment
        /// </summary>
        public static void InitializeUnit()
        {
            Networking.DetermineNetworkConfig(out UClientComputerName, out UClientIPAddress);
            UClientOS = Utilities.DetermineExecutingOS();
            UClientAssemblyVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString(3);              // leave out 'Build'

            if (TClientSettings.RunAsRemote)
            {
                UInstallationKind = "Remote Client";
            }
            else
            {
                if (TClientSettings.RunAsStandalone)
                {
                    UInstallationKind = "Standalone Installation";
                }
                else
                {
                    UInstallationKind = "Network Client";
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static TExecutingOSEnum ClientOS
        {
            get
            {
                return Get_ClientOS();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static String ClientIPAddress
        {
            get
            {
                return Get_ClientIPAddress();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static String ClientComputerName
        {
            get
            {
                return Get_ClientComputerName();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static String ClientAssemblyVersion
        {
            get
            {
                return Get_ClientAssemblyVersion();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static String InstallationKind
        {
            get
            {
                return Get_InstallationKind();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public static String Get_ClientComputerName()
        {
            return UClientComputerName;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public static String Get_ClientIPAddress()
        {
            return UClientIPAddress;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public static String Get_ClientAssemblyVersion()
        {
            return UClientAssemblyVersion;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public static String Get_InstallationKind()
        {
            return UInstallationKind;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public static TExecutingOSEnum Get_ClientOS()
        {
            return UClientOS;
        }
    }
}