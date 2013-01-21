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
using System.Collections.Generic;
using System.Text;
using System.Security.Permissions;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// this class will manage access to services for each client, to its appdomain
    /// </summary>
    [CrossDomainContext]
    public class TCrossDomainMarshaller : ContextBoundObject
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TCrossDomainMarshaller()
        {
        }

        private static Dictionary <string, ICrossDomainService>FAvailableServicesPerClient
            = new Dictionary <string, ICrossDomainService>();

        /// <summary>
        /// add a new service, for the specified client
        /// </summary>
        public static void AddService(string AClientID, string AServiceID, ICrossDomainService AService)
        {
            FAvailableServicesPerClient.Add(AClientID + AServiceID, AService);

            if (TLogging.DebugLevel > 1)
            {
                TLogging.Log("CrossDomainMarshaller.AddService " + AClientID + " " + AServiceID + " " + FAvailableServicesPerClient.Count.ToString());
            }
        }

        /// <summary>
        /// get the service that should be served across appdomains
        /// </summary>
        public static ICrossDomainService GetService(string clientID, string AServiceID)
        {
            if (FAvailableServicesPerClient.ContainsKey(clientID + AServiceID))
            {
                return FAvailableServicesPerClient[clientID + AServiceID];
            }

            string message = "cannot find service " + AServiceID + " for client " + clientID + "; there are " +
                             FAvailableServicesPerClient.Count.ToString() + " registered services at the moment";
            TLogging.Log(message);
            throw new Exception(message);
        }

        /// <summary>
        /// lifetime service
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }

    /// <summary>
    /// attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    class CrossDomainContextAttribute : ContextAttribute
    {
        /// <summary>
        /// constructor
        /// </summary>
        public CrossDomainContextAttribute() : base("Interception")
        {
        }

        /// <summary>
        /// check context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public override bool IsContextOK(Context context, IConstructionCallMessage ctor)
        {
            return context.GetProperty("Interception") != null;
        }

        /// <summary>
        /// add context property
        /// </summary>
        /// <param name="ctor"></param>
        public override void GetPropertiesForNewContext(IConstructionCallMessage ctor)
        {
            ctor.ContextProperties.Add(new CrossDomainContextProperty());
        }
    }

    /// <summary>
    /// context property
    /// </summary>
    class CrossDomainContextProperty :
        IContextProperty,
        IContributeObjectSink
    {
        /// <summary>
        /// check context
        /// </summary>
        public bool IsNewContextOK(Context newContext)
        {
            return true;
        }

        /// <summary>
        /// freeze context
        /// </summary>
        /// <param name="newContext"></param>
        public void Freeze(Context newContext)
        {
        }

        /// <summary>
        /// get name
        /// </summary>
        public string Name
        {
            get
            {
                return "CrossDomain";
            }
        }

        /// <summary>
        /// create MessageSink
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nextSink"></param>
        /// <returns></returns>
        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            return new MessageSink(nextSink);
        }
    }

    /// <summary>
    /// MessageSink
    /// </summary>
    class MessageSink :
        IMessageSink
    {
        IMessageSink nextSink;

        /// <summary>
        /// constructor
        /// </summary>
        public MessageSink(IMessageSink nextSink)
        {
            this.nextSink = nextSink;
        }

        /// <summary>
        /// process message
        /// </summary>
        public IMessage SyncProcessMessage(IMessage msg)
        {
            if (msg.Properties["__Uri"] == null)
            {
                return this.nextSink.SyncProcessMessage(msg);
            }
            else
            {
                LogicalCallContext callContext = (LogicalCallContext)msg.Properties["__CallContext"];

                if ((callContext.GetData("__ClientID") != null) && (callContext.GetData("__ServiceID") != null))
                {
                    string clientID = (string)callContext.GetData("__ClientID");
                    string serviceID = (string)callContext.GetData("__ServiceID");

                    if (TLogging.DebugLevel >= 10)
                    {
                        TLogging.Log("SyncProcessMessage: " + clientID + " " + serviceID);
                    }

                    return TCrossDomainMarshaller.GetService(clientID, serviceID).Marshal(msg);
                }
                else
                {
                    // TLogging.Log("SyncProcessMessage: No __ClientID or __ServiceID");
                    return new ReturnMessage(
                        new ApplicationException("No __ClientID or __ServiceID"),
                        (IMethodCallMessage)msg);
                }
            }
        }

        /// <summary>
        /// Process Message
        /// </summary>
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return this.nextSink.AsyncProcessMessage(msg, replySink);
        }

        /// <summary>
        /// get next sink
        /// </summary>
        public IMessageSink NextSink
        {
            get
            {
                return this.nextSink;
            }
        }
    }
}