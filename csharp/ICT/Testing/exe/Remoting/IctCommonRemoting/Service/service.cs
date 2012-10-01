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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Runtime.Remoting;
using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Tests.IctCommonRemoting.Service;
using Tests.IctCommonRemoting.Interface;

namespace Tests.IctCommonRemoting.Service
{
    /// <summary>
    /// the test service
    /// </summary>
    public class TMyService : TConfigurableMBRObject, IMyService
    {
        /// make sure that the TMyService object exists until this AppDomain is unloaded!
        public override object InitializeLifetimeService()
        {
            return null;
        }

        private TMySubNamespaceRemote FSubNamespace = null;

        /// <summary>
        /// print hello world
        /// </summary>
        /// <param name="msg"></param>
        public string HelloWorld(string msg)
        {
            TLogging.Log(msg);
            return "Hello from the server!!!";
        }

        /// <summary>
        /// some tests for remoting DateTime objects
        /// </summary>
        /// <param name="date"></param>
        /// <param name="outDate"></param>
        /// <returns></returns>
        public DateTime TestDateTime(DateTime date, out DateTime outDate)
        {
            Console.WriteLine("ToShortDateString(): " + date.ToShortDateString());
            Console.WriteLine("ToUniversalTime(): " + date.ToUniversalTime());
            Console.WriteLine("ToLocalTime(): " + date.ToLocalTime());

            date = new DateTime(date.Year, date.Month, date.Day);
            Console.WriteLine("ToShortDateString(): " + date.ToShortDateString());
            Console.WriteLine("ToUniversalTime(): " + date.ToUniversalTime());
            Console.WriteLine("ToLocalTime(): " + date.ToLocalTime());

            outDate = date;
            return date;
        }

        /// <summary>
        /// test
        /// </summary>
        public IMySubNamespace SubNamespace
        {
            get
            {
                if (FSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TMySubNamespace");
                    TMySubNamespace ObjectToRemote = new TMySubNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FSubNamespace = new TMySubNamespaceRemote(ObjectURI);
                }

                return FSubNamespace;
            }
        }
    }

    /// <summary>
    /// test of UIConnector
    /// </summary>
    public class TMyUIConnector : TConfigurableMBRObject, IMyUIConnector
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TMyUIConnector()
        {
        }

        private int FCounter = 0;

        /// <summary>
        /// test
        /// </summary>
        public string HelloWorldUIConnector()
        {
            FCounter++;
            string s = "HelloWorldUIConnector called this many times: " + FCounter.ToString();
            TLogging.Log(s);
            return s;
        }
    }

    /// <summary>
    /// this object is needed because we need another remoted object for sub namespaces
    /// </summary>
    public class TMySubNamespace : TConfigurableMBRObject, IMySubNamespace
    {
        /// <summary>Constructor</summary>
        public TMySubNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMySubNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>
        /// return the UIConnector object
        /// </summary>
        public IMyUIConnector MyUIConnector()
        {
            return (IMyUIConnector)TCreateRemotableObject.CreateRemotableObject(
                typeof(IMyUIConnector),
                typeof(TMyUIConnectorRemote),
                new TMyUIConnector());
        }

        /// print hello sub world
        public string HelloSubWorld(string msg)
        {
            TLogging.Log(msg);
            return "HelloSubWorld from the server!!!";
        }

        /// object that will be serialized to the client.
        /// it opens a new channel for each new object.
        /// this is needed for cross domain marshalling.
        [Serializable]
        public class TMyUIConnectorRemote : IMyUIConnector, IKeepAlive
        {
            private IMyUIConnector RemoteObject = null;
            private string FObjectURI;
            /// constructor
            public TMyUIConnectorRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IMyUIConnector)
                               TConnector.TheConnector.GetRemoteObject(FObjectURI,
                    typeof(IMyUIConnector));
            }

            /// keep the object alive on the server
            public void KeepAlive()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                // The following call is the key to the whole concept of keeping
                // the remoted server-side Objects alive:
                // Calling 'GetLifeTimeService' is sufficient to 'tickle' the
                // server-side Object and for its Lease to be renewed!
                try
                {
                    ((MarshalByRefObject)RemoteObject).InitializeLifetimeService();
                }
                catch (Exception e)
                {
                    TLogging.Log(e.ToString());
                }
            }

            /// forward the method call
            public string HelloWorldUIConnector()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.HelloWorldUIConnector();
            }
        }
    }

    /// <summary>
    /// serializable, which means that this object is executed on the client side
    /// </summary>
    [Serializable]
    public class TMySubNamespaceRemote : IMySubNamespace
    {
        private IMySubNamespace RemoteObject = null;
        private string FObjectURI;

        /// <summary>
        /// constructor. get remote object
        /// </summary>
        public TMySubNamespaceRemote(string AObjectURI)
        {
            FObjectURI = AObjectURI;
            TLogging.Log(" in appdomain " + Thread.GetDomain().FriendlyName);
        }

        private void InitRemoteObject()
        {
            TLogging.Log("InitRemoteObject in appdomain " + Thread.GetDomain().FriendlyName);
            RemoteObject = (IMySubNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IMySubNamespace));
        }

        /// get the UIConnector
        public IMyUIConnector MyUIConnector()
        {
            if (RemoteObject == null)
            {
                InitRemoteObject();
            }

            return RemoteObject.MyUIConnector();
        }

        /// print hello sub world
        public string HelloSubWorld(string msg)
        {
            if (RemoteObject == null)
            {
                InitRemoteObject();
            }

            return RemoteObject.HelloSubWorld(msg);
        }
    }
}

namespace Tests.IctCommonRemoting.Instantiator
{
    /// <summary>
    /// LOADER CLASS. Creates and dynamically exposes an instance of the remoteable
    /// class to make it callable remotely from the Client.
    /// </summary>
    public class TMyServiceNamespaceLoader : TConfigurableMBRObject
    {
        /// <summary>URL at which the remoted object can be reached</summary>
        private String FRemotingURL;

        private ICrossDomainService FRemotedObject;

        /// <summary>Constructor</summary>
        public TMyServiceNamespaceLoader()
        {
            TLogging.LogAtLevel(9, "TMyServiceNamespaceLoader created in application domain: " + Thread.GetDomain().FriendlyName);
        }

        /// <summary>
        /// Creates and dynamically exposes an instance of the remoteable TMyService
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
                Console.WriteLine("TMyServiceNamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
            }

            FRemotedObject = new TMyService();
            FRemotingURL = BuildRandomURI("TMyService");

            return FRemotingURL;
        }

        /// <summary>
        /// get the object to be remoted
        /// </summary>
        public ICrossDomainService GetRemotedObject()
        {
            return FRemotedObject;
        }
    }
}