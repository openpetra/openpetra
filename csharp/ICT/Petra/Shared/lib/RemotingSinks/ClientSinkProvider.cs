/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Collections;

namespace Ict.Petra.Shared.RemotingSinks.Encryption
{
    /// <summary>
    /// the channel sink provider, hard coded for our encryption sink
    /// </summary>
    public class EncryptionClientSinkProvider : IClientChannelSinkProvider
    {
        private IClientChannelSinkProvider FNextProvider;

        private byte[] FEncryptionKey;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="providerData"></param>
        public EncryptionClientSinkProvider(IDictionary properties, ICollection providerData)
        {
            String keyfile = (String)properties["keyfile"];

            if (keyfile == null)
            {
                throw new RemotingException("'keyfile' has to " +
                    "be specified for EncryptionClientSinkProvider");
            }

            // read the encryption key from the specified fike
            FileInfo fi = new FileInfo(keyfile);

            if (!fi.Exists)
            {
                throw new RemotingException("Specified keyfile does not exist");
            }

            FEncryptionKey = Ict.Common.IO.EncryptionRijndael.ReadSecretKey(keyfile);
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
            return new EncryptionClientSink(next, FEncryptionKey);
        }
    }
}