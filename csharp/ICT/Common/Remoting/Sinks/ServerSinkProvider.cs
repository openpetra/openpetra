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
using System.Xml;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Collections;
using System.Security.Cryptography;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Common.Remoting.Sinks.Encryption
{
    /// <summary>
    /// the channel sink provider, hard coded for our encryption sink
    /// </summary>
    public class EncryptionServerSinkProvider : IServerChannelSinkProvider
    {
        private RSAParameters FPrivateKey;

        private IServerChannelSinkProvider FNextProvider;

        /// <summary>
        /// constructor
        /// </summary>
        public EncryptionServerSinkProvider()
        {
            Init();
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EncryptionServerSinkProvider(IDictionary properties, ICollection providerData)
        {
            Init();
        }

        private void Init()
        {
            string KeyFile = TAppSettingsManager.GetValue("Server.ChannelEncryption.PrivateKeyfile");

            // read the encryption key from the specified file
            FileInfo fi = new FileInfo(KeyFile);

            if (!fi.Exists)
            {
                throw new RemotingException(
                    String.Format("Specified keyfile {0} does not exist", KeyFile));
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(KeyFile);

            FPrivateKey = new RSAParameters();

            try
            {
                FPrivateKey.D = Convert.FromBase64String(TXMLParser.GetChild(doc.FirstChild, "D").InnerText);
                FPrivateKey.P = Convert.FromBase64String(TXMLParser.GetChild(doc.FirstChild, "P").InnerText);
                FPrivateKey.Q = Convert.FromBase64String(TXMLParser.GetChild(doc.FirstChild, "Q").InnerText);
                FPrivateKey.DP = Convert.FromBase64String(TXMLParser.GetChild(doc.FirstChild, "DP").InnerText);
                FPrivateKey.DQ = Convert.FromBase64String(TXMLParser.GetChild(doc.FirstChild, "DQ").InnerText);
                FPrivateKey.InverseQ = Convert.FromBase64String(TXMLParser.GetChild(doc.FirstChild, "InverseQ").InnerText);
                FPrivateKey.Modulus = Convert.FromBase64String(TXMLParser.GetChild(doc.FirstChild, "Modulus").InnerText);
                FPrivateKey.Exponent = Convert.FromBase64String(TXMLParser.GetChild(doc.FirstChild, "Exponent").InnerText);
            }
            catch (Exception)
            {
                throw new RemotingException(
                    String.Format("Problems reading the keyfile {0}. Cannot find all attributes of the key.", KeyFile));
            }
        }

        /// <summary>
        /// the next provider
        /// </summary>
        public IServerChannelSinkProvider Next
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
        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            // create other sinks in the chain
            IServerChannelSink next = FNextProvider.CreateSink(channel);

            // put our sink on top of the chain and return it
            return new EncryptionServerSink(next,
                FPrivateKey);
        }

        /// <summary>
        /// not implemented
        /// </summary>
        public void GetChannelData(IChannelDataStore channelData)
        {
            // not needed
        }
    }
}