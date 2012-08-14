//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Security.Principal;
using System.Collections;
using Ict.Common;
using Ict.Common.Remoting.Server;
using Tests.IctCommonRemoting.Interface;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;

namespace Tests.IctCommonRemoting.Server
{
    /// <summary>
    /// Allows creation of and connection to a Client's AppDomain without causing
    /// the Assemblies which are loaded in the Client's AppDomain to be loaded into
    /// the Default AppDomain.
    ///
    /// @comment This class is used by TClientManager to create AppDomains for Clients
    /// and to communicate with them, using a TRemoteLoader object.
    ///
    /// </summary>
    public class TClientAppDomainConnection : TClientAppDomainConnectionBase
    {
        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MYSERVICE_DLLNAME = "Ict.Testing.exe.Remoting.IctCommonRemoting.Service";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String MYSERVICE_CLASSNAME = "Tests.IctCommonRemoting.Instantiator.TMyServiceNamespaceLoader";

        /// Load Petra Module DLLs into Clients AppDomain, initialise them and remote an Instantiator Object
        public override void LoadAssemblies(string AClientID, IPrincipal AUserInfo, ref Hashtable ARemotingURLs)
        {
            String RemotingURL_MyService;

            // Load MYSERVICE Module assembly
            LoadPetraModuleAssembly(AClientID, MYSERVICE_DLLNAME, MYSERVICE_CLASSNAME, out RemotingURL_MyService);
            ARemotingURLs.Add(SharedConstantsTest.REMOTINGURL_IDENTIFIER_MYSERVICE, RemotingURL_MyService);

            if (TLogging.DL >= 5)
            {
                Console.WriteLine("  TMyService instantiated. Remoting URL: " + RemotingURL_MyService);
            }
        }
    }
}