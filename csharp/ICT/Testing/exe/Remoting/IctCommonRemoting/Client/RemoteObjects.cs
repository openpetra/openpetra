//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Tests.IctCommonRemoting.Interface;

namespace Tests.IctCommonRemoting.Client
{
    /// <summary>
    /// Holds all references to instantiated Serverside objects that are remoted by the Server.
    /// These objects can get remoted either statically (the same Remoting URL all
    /// the time) or dynamically (on-the-fly generation of Remoting URL).
    ///
    /// The TRemote class is used by the Client for all communication with the Server
    /// after the initial communication at Client start-up is done.
    ///
    /// The properties MPartner, MFinance, etc. represent the top-most level of the
    /// Petra Partner, Finance, etc. Petra Module Namespaces in the PetraServer.
    /// </summary>
    public class TRemote : TRemoteBase
    {
        /// <summary>Reference to the topmost level of the Petra Common Module Namespace</summary>
        public static IMyService MyService
        {
            get
            {
                return UMyServiceObject;
            }
        }

        private static IMyService UMyServiceObject;

        /// <summary>
        /// References to the rest of the topmost level of the other Petra Module Namespaces will go here
        /// </summary>
        /// <returns>void</returns>
        public TRemote(IClientManagerInterface AClientManager,
            IMyService AMyServiceObject)
            : base(AClientManager)
        {
            UMyServiceObject = AMyServiceObject;
        }
    }
}