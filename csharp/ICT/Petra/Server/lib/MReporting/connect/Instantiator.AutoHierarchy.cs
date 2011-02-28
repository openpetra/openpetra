// Auto generated with nant generateGlue
// based on csharp\ICT\Petra\Definitions\NamespaceHierarchy.yml
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
        /// <summary>holds reference to the TMReporting object</summary>
        private ObjRef FRemotedObject;

        /// <summary>Constructor</summary>
        public TMReportingNamespaceLoader()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created in application domain: " + Thread.GetDomain().FriendlyName);
            }

#endif
        }

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
            TMReporting RemotedObject;
            DateTime RemotingTime;
            String RemoteAtURI;
            String RandomString;
            System.Security.Cryptography.RNGCryptoServiceProvider rnd;
            Byte rndbytespos;
            Byte[] rndbytes = new Byte[5];

#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMReportingNamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
            }

#endif

            RandomString = "";
            rnd = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rnd.GetBytes(rndbytes);

            for (rndbytespos = 1; rndbytespos <= 4; rndbytespos += 1)
            {
                RandomString = RandomString + rndbytes[rndbytespos].ToString();
            }

            RemotingTime = DateTime.Now;
            RemotedObject = new TMReporting();
            RemoteAtURI = (RemotingTime.Day).ToString() + (RemotingTime.Hour).ToString() + (RemotingTime.Minute).ToString() +
                          (RemotingTime.Second).ToString() + '_' + RandomString.ToString();
            FRemotedObject = RemotingServices.Marshal(RemotedObject, RemoteAtURI, typeof(IMReportingNamespace));
            FRemotingURL = RemoteAtURI; // FRemotedObject.URI;

#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMReporting.URI: " + FRemotedObject.URI);
            }

#endif

            return FRemotingURL;
        }

    }

    /// <summary>
    /// REMOTEABLE CLASS. MReporting Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMReporting : MarshalByRefObject, IMReportingNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TLogicConnectorsNamespace FLogicConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TMReporting()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TMReporting()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMReporting object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

        /// <summary>The 'LogicConnectors' subnamespace contains further subnamespaces.</summary>
        public ILogicConnectorsNamespace LogicConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MReporting.LogicConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MReporting.LogicConnectors' sub-namespace
                //

                // accessing TLogicConnectorsNamespace the first time? > instantiate the object
                if (FLogicConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TLogicConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MReporting.Instantiator.LogicConnectors') should be automatically contructable.
                    FLogicConnectorsSubNamespace = new TLogicConnectorsNamespace();
                }

                return FLogicConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MReporting.Instantiator.LogicConnectors
{
    /// <summary>auto generated class </summary>
    public class TLogicConnectorsNamespace : MarshalByRefObject, ILogicConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TLogicConnectorsNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TLogicConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TLogicConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public IReportGeneratorLogicConnector ReportGenerator()
        {
            return new TReportGeneratorLogicConnector();
        }
    }
}

