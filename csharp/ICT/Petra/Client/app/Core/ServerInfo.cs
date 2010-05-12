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
    /// The TServerInfo class holds information about the PetraServer that the
    /// Client is connected to.
    ///
    /// Currently hold only very basic information, will be extended in the future.
    ///
    /// @Comment Change the class to retrieve the Server info on its own on
    ///          constructor call.
    /// </summary>
    public class TServerInfo
    {
        private static TExecutingOSEnum PServerOS;

        /// <summary>class procedure set_ServerOS(const Value: TExecutingOS); static;write set_ServerOS</summary>
        public static TExecutingOSEnum ServerOS
        {
            get
            {
                return PServerOS;
            }
        }

        /// <summary>
        /// Constructor that initialises the property values.
        ///
        /// @todo Change the class to retrieve the Server info on its own on
        ///
        /// </summary>
        /// <param name="ServerOS">Operating System of the PetraServer that the Client is
        /// connected to
        /// </param>
        /// <returns>void</returns>
        public TServerInfo(TExecutingOSEnum ServerOS)
        {
            // TODO ochristiank cServer_Info : Load Server info from Server.
            PServerOS = ServerOS;
        }
    }
}