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
using System.Net;

namespace Ict.Common
{
    /// <summary>
    /// Contains general networking procedures and functions for ICT Applications.
    /// </summary>
    public class Networking
    {
        /// <summary>
        /// Examines the network configuration of the computer where this procedure is
        /// executed.
        ///
        /// </summary>
        /// <param name="ComputerName">Network name of the computer</param>
        /// <param name="IPAddresses">IP Address(es) of the computer (separated by semicolons if
        /// there is more than one IP Address for this computer)
        /// </param>
        /// <returns>void</returns>
        public static void DetermineNetworkConfig(out String ComputerName, out String IPAddresses)
        {
            IPHostEntry HostInfo;

            // Get ComputerName and IPAddress(es) of the local computer
            ComputerName = Dns.GetHostName();
            HostInfo = Dns.GetHostEntry(ComputerName);

            // Loop through all IPAddressses of the local computer
            IPAddresses = "";

            foreach (IPAddress ip in HostInfo.AddressList)
            {
                IPAddresses = IPAddresses + ip.ToString() + "; ";
            }

            IPAddresses = IPAddresses.Substring(0, IPAddresses.Length - 2); // remove last '; '

            // on virtual servers, we have to tell the IP address manually, iptables forwarding hides the IP address
            IPAddresses = TAppSettingsManager.GetValue("ListenOnIPAddress", IPAddresses, false);
        }
    }
}