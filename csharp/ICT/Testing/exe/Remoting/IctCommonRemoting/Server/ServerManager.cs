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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using GNU.Gettext;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;

using Tests.IctCommonRemoting.Interface;

namespace Tests.IctCommonRemoting.Server
{
    /// <summary>
    /// see the documentation of the base class
    /// </summary>
    public class TServerManager : TServerManagerBase
    {
        /// <summary>
        /// Initialises Logging and parses Server settings from different sources.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TServerManager() : base()
        {
            TRemoteLoader.CLIENTDOMAIN_DLLNAME = "Ict.Testing.IctCommonRemoting.Server";
            TRemoteLoader.CLIENTDOMAIN_CLASSNAME = "Tests.IctCommonRemoting.Server.TClientDomainManager";
            TClientAppDomainConnectionBase.ClientAppDomainConnectionType = typeof(TClientAppDomainConnection);

            TClientManager.InitializeStaticVariables(null,
                null,
                new TUserManager(),
                null,
                null,
                new TClientAppDomainConnection());
        }
    }
}