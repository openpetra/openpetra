//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Reflection;
using System.IO;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Common.Remoting.Client
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
            UClientAssemblyVersion = TFileVersionInfo.GetApplicationVersion().ToString();

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
                return UClientOS;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static String ClientIPAddress
        {
            get
            {
                return UClientIPAddress;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static String ClientComputerName
        {
            get
            {
                return UClientComputerName;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static String ClientAssemblyVersion
        {
            get
            {
                return UClientAssemblyVersion;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static String InstallationKind
        {
            get
            {
                return UInstallationKind;
            }
        }
    }
}