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
    /// this object is needed because we need another remoted object for sub namespaces
    /// </summary>
    public class TMySubNamespace : TConfigurableMBRObject, IMySubNamespace
    {
        /// print hello sub world
        public string HelloSubWorld(string msg)
        {
            TLogging.Log(msg);
            return "HelloSubWorld from the server!!!";
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
//          TLogging.LogAtLevel (9, "TMyServiceNamespaceLoader created in application domain: " + Thread.GetDomain().FriendlyName);
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