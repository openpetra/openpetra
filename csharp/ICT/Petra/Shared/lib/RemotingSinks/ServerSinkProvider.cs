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
    public class EncryptionServerSinkProvider : IServerChannelSinkProvider
    {
        private byte[] FEncryptionKey;

        private IServerChannelSinkProvider FNextProvider;

        /// <summary>
        /// constructor
        /// </summary>
        public EncryptionServerSinkProvider(string AKeyFile)
        {
            Init(AKeyFile);
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EncryptionServerSinkProvider(IDictionary properties, ICollection providerData)
        {
            String keyfile = (String)properties["keyfile"];

            if (keyfile == null)
            {
                throw new RemotingException("'keyfile' has to " +
                    "be specified for EncryptionServerSinkProvider");
            }

            Init(keyfile);
        }

        private void Init(string AKeyFile)
        {
            // read the encryption key from the specified file
            FileInfo fi = new FileInfo(AKeyFile);

            if (!fi.Exists)
            {
                throw new RemotingException("Specified keyfile does not exist");
            }

            FileStream fs = new FileStream(AKeyFile, FileMode.Open);
            FEncryptionKey = new Byte[fi.Length];
            fs.Read(FEncryptionKey, 0, FEncryptionKey.Length);
            fs.Close();
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
                FEncryptionKey);
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