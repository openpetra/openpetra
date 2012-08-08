// Auto generated with nant generateGlue
// based on csharp\ICT\Petra\Definitions\NamespaceHierarchy.yml
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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

//
// Contains a remotable class that instantiates an Object which gives access to
// the MPartner Namespace (from the Client's perspective).
//
// The purpose of the remotable class is to present other classes which are
// Instantiators for sub-namespaces to the Client. The instantiation of the
// sub-namespace objects is completely transparent to the Client!
// The remotable class itself gets instantiated and dynamically remoted by the
// loader class, which in turn gets called when the Client Domain is set up.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Server.App.Core.Security;

using Ict.Petra.Shared.Interfaces.MReporting;
using Ict.Petra.Shared.Interfaces.MReporting.LogicConnectors;
using Ict.Petra.Server.MReporting.Instantiator.LogicConnectors;
using Ict.Petra.Server.MReporting.LogicConnectors;

namespace Ict.Petra.Server.MReporting.Instantiator
{
    /// <summary>
    /// LOADER CLASS. Creates and dynamically exposes an instance of the remoteable
    /// class to make it callable remotely from the Client.
    /// </summary>
    public class TMReportingNamespaceLoader : TConfigurableMBRObject
    {
        /// <summary>URL at which the remoted object can be reached</summary>
        private String FRemotingURL;
        /// <summary>the remoted object</summary>
        private TMReporting FRemotedObject;

        /// <summary>
        /// Creates and dynamically exposes an instance of the remoteable TMReporting
        /// class to make it callable remotely from the Client.
        ///
        /// @comment This function gets called from TRemoteLoader.LoadPetraModuleAssembly.
        /// This call is done late-bound through .NET Reflection!
        ///
        /// WARNING: If the name of this function or its parameters should change, this
        /// needs to be reflected in the call to this function in
        /// TRemoteLoader.LoadPetraModuleAssembly!!!
        ///
        /// </summary>
        /// <returns>The URL at which the remoted object can be reached.</returns>
        public String GetRemotingURL()
        {
            if (TLogging.DL >= 9)
            {
                Console.WriteLine("TMReportingNamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
            }

            FRemotedObject = new TMReporting();
            FRemotingURL = TConfigurableMBRObject.BuildRandomURI("TMReportingNamespaceLoader");

            return FRemotingURL;
        }

        /// <summary>
        /// get the object to be remoted
        /// </summary>
        public TMReporting GetRemotedObject()
        {
            return FRemotedObject;
        }
    }

    /// <summary>
    /// REMOTEABLE CLASS. MReporting Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMReporting : TConfigurableMBRObject, IMReportingNamespace
    {
        /// <summary>Constructor</summary>
        public TMReporting()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMReporting object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'LogicConnectors' subnamespace contains further subnamespaces.</summary>
        public ILogicConnectorsNamespace LogicConnectors
        {
            get
            {
                return (ILogicConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(ILogicConnectorsNamespace),
                        new TLogicConnectorsNamespace());
            }
        }
    }
}

namespace Ict.Petra.Server.MReporting.Instantiator.LogicConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. LogicConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TLogicConnectorsNamespace : TConfigurableMBRObject, ILogicConnectorsNamespace
    {
        /// <summary>Constructor</summary>
        public TLogicConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TLogicConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public IReportGeneratorLogicConnector ReportGenerator()
        {
            return (IReportGeneratorLogicConnector) TCreateRemotableObject.CreateRemotableObject(
                    typeof(IReportGeneratorLogicConnector),
                    new TReportGeneratorLogicConnector());
        }
    }
}

