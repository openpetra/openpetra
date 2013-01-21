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
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Collections;
using System.Security.Cryptography;
using System.Xml;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Common.Remoting.Sinks.Encryption
{
    /// <summary>
    /// the channel sink provider, hard coded for our encryption sink
    /// </summary>
    public class EncryptionClientSinkProvider : IClientChannelSinkProvider
    {
        private IClientChannelSinkProvider FNextProvider;
        private RSAParameters FPublicKeyServer;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="providerData"></param>
        public EncryptionClientSinkProvider(IDictionary properties, ICollection providerData)
        {
            // do not use property, but create local symmetric key, and send to the server, encrypted with the public key of the server
            try
            {
                XmlDocument doc = new XmlDocument();

                // get the public key from the server, from a secure site. if the SSL certificate is self signed or not valid, this will fail.
                // publicKeyXml will contain: <RSAKeyValue><Modulus>w7/g+...+sU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>
                if (properties["HttpsPublicKeyXml"] != null)
                {
                    string publicKeyXml = THTTPUtils.ReadWebsite((string)properties["HttpsPublicKeyXml"]);
                    doc.LoadXml(publicKeyXml);
                }
                else
                {
                    string publicKeyXml = (string)properties["FilePublicKeyXml"];
                    doc.Load(publicKeyXml);
                }

                try
                {
                    FPublicKeyServer = new RSAParameters();
                    FPublicKeyServer.Modulus = Convert.FromBase64String(TXMLParser.GetChild(doc.FirstChild, "Modulus").InnerText);
                    FPublicKeyServer.Exponent = Convert.FromBase64String(TXMLParser.GetChild(doc.FirstChild, "Exponent").InnerText);
                }
                catch
                {
                    throw new Exception("Invalid public key XML file, cannot find Modulus or Exponent");
                }
            }
            catch (Exception)
            {
                TLogging.Log("Cannot get the public key of the OpenPetra server");
                throw;
            }
        }

        /// <summary>
        /// next sink provider
        /// </summary>
        public IClientChannelSinkProvider Next
        {
            get
            {
                return FNextProvider;
            }
            set
            {
                FNextProvider = value;
            }
        }

        /// <summary>
        /// create a sink, hard coded with our encryption sink
        /// </summary>
        public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
        {
            // create other sinks in the chain
            IClientChannelSink next = FNextProvider.CreateSink(channel,
                url,
                remoteChannelData);

            // put our sink on top of the chain and return it
            return new EncryptionClientSink(next, FPublicKeyServer);
        }
    }
}