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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using Ict.Common;

namespace Ict.Common.Remoting.Client
{
    /// <summary>
    /// custom proxy, for each remoted object, to tell the server which client and which service should be served
    /// </summary>
    public class CustomProxy : RealProxy
    {
        string CrossDomainURL;
        string clientID;
        string serviceID;
        IMessageSink messageSink;

        /// <summary>
        /// constructor
        /// </summary>
        public CustomProxy(string ACrossDomainURL, string AServiceURI, string AClientID, Type type) : base(type)
        {
            this.CrossDomainURL = ACrossDomainURL;

            this.clientID = AClientID;

            this.serviceID = AServiceURI;

            foreach (IChannel channel in ChannelServices.RegisteredChannels)
            {
                if (channel is IChannelSender)
                {
                    IChannelSender sender = (IChannelSender)channel;

                    if (string.Compare(sender.ChannelName, "tcp") == 0)
                    {
                        string objectUri;
                        this.messageSink = sender.CreateMessageSink(this.CrossDomainURL, null, out objectUri);

                        if (this.messageSink != null)
                        {
                            break;
                        }
                    }
                }
            }

            if (this.messageSink == null)
            {
                throw new Exception("No channel found for " + this.CrossDomainURL);
            }
        }

        /// <summary>
        /// tell the server which client we are, so that it can provide the right service
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override IMessage Invoke(IMessage msg)
        {
            msg.Properties["__Uri"] = this.CrossDomainURL;

            LogicalCallContext callContext = (LogicalCallContext)msg.Properties["__CallContext"];
            callContext.SetData("__ClientID", this.clientID);
            callContext.SetData("__ServiceID", this.serviceID);

            return this.messageSink.SyncProcessMessage(msg);
        }
    }
}