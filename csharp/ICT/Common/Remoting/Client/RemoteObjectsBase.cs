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
using System.Collections.Generic;
using System.Runtime.Remoting;
using Ict.Common;
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Client
{
    /// <summary>
    /// base class for TRemote
    /// </summary>
    public class TRemoteBase
    {
        /// <summary>Reference to the ClientManager</summary>
        public static IClientManagerInterface ClientManager
        {
            get
            {
                return UClientManager;
            }

            set
            {
                UClientManager = value;
            }
        }

        private static IClientManagerInterface UClientManager;

        /// <summary>
        /// list of the main remoted objects
        /// </summary>
        protected static List <MarshalByRefObject>FRemoteObjects = new List <MarshalByRefObject>();

        /// <summary>
        /// constructor
        /// </summary>
        public TRemoteBase(IClientManagerInterface AClientManager)
        {
            UClientManager = AClientManager;
        }

        /// <summary>
        /// disconnect the remoted objects
        /// </summary>
        public static void Disconnect()
        {
            /* These are proxies - we don't disconnect them.
            RemotingServices.Disconnect((MarshalByRefObject)UClientManager);

            foreach (MarshalByRefObject obj in FRemoteObjects)
            {
                RemotingServices.Disconnect(obj);
            }
             */
        }
    }
}