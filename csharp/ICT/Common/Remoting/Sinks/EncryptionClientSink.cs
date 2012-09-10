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
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using Ict.Common.IO;
using Ict.Common;

namespace Ict.Common.Remoting.Sinks.Encryption
{
    internal class EncryptionClientSink : BaseChannelSinkWithProperties, IClientChannelSink
    {
        private static RSAParameters FPublicKeyServer;
        private static byte[] FEncryptionKey = null;
        private IClientChannelSink FNextSink;
        private bool SendKeyAgain = true;

        /// <summary>
        /// constructor
        /// </summary>
        public EncryptionClientSink(IClientChannelSink ANextSink, RSAParameters APublicKeyServer)
        {
            FPublicKeyServer = APublicKeyServer;
            FNextSink = ANextSink;
            SendKeyAgain = true;
        }

        public IClientChannelSink NextChannelSink
        {
            get
            {
                return FNextSink;
            }
        }

        /// <summary>
        /// Requests asynchronous processing of a method call on the current sink.
        /// </summary>
        public void AsyncProcessRequest(IClientChannelSinkStack sinkStack,
            IMessage msg, ITransportHeaders headers, Stream stream)
        {
            // process request
            object state = null;

            ProcessRequest(msg, headers, ref stream, ref state);

            // push to stack (to get a call to handle response)
            // and forward to the next
            sinkStack.Push(this, state);
            this.FNextSink.AsyncProcessRequest(sinkStack, msg, headers, stream);
        }

        /// <summary>
        /// Requests asynchronous processing of a response to a method call on the current sink.
        /// </summary>
        public void AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack, object state,
            ITransportHeaders headers, Stream stream)
        {
            // process response
            ProcessResponse(null, headers, ref stream, state);

            // forward to the next
            sinkStack.AsyncProcessResponse(headers, stream);
        }

        /// <summary>
        /// Returns the Stream onto which the provided message is to be serialized.
        /// </summary>
        public Stream GetRequestStream(IMessage msg,
            ITransportHeaders headers)
        {
            return null;
        }

        /// <summary>
        /// Requests message processing from the current sink.
        /// </summary>
        public void ProcessMessage(IMessage msg, ITransportHeaders requestHeaders,
            Stream requestStream, out ITransportHeaders responseHeaders, out Stream responseStream)
        {
            // process request
            object state = null;

            ProcessRequest(msg, requestHeaders, ref requestStream, ref state);

            // forward to the next
            this.FNextSink.ProcessMessage(msg, requestHeaders, requestStream,
                out responseHeaders, out responseStream);

            // process response
            ProcessResponse(null, responseHeaders, ref responseStream, state);
        }

        public string CurrentClientGuid = Guid.NewGuid().ToString();

        /// <summary>
        /// encrypt the request
        /// </summary>
        protected void ProcessRequest(IMessage message, ITransportHeaders headers, ref Stream stream, ref object state)
        {
            if (FEncryptionKey == null)
            {
                // create a symmetric key
                Rijndael alg = new RijndaelManaged();
                alg.GenerateKey();
                FEncryptionKey = alg.Key;
                SendKeyAgain = true;
            }

            if (SendKeyAgain)
            {
                // tell the server the symmetric key,
                // but encrypt with the public key of the server.
                // this means that only the server can read the secret key.
                RSACryptoServiceProvider serverRSA = new RSACryptoServiceProvider();
                serverRSA.ImportParameters(FPublicKeyServer);
                string encryptedSymmetricKey = Convert.ToBase64String(serverRSA.Encrypt(FEncryptionKey, false));
                headers[EncryptionRijndael.GetEncryptionName() + "KEY"] = encryptedSymmetricKey;
                SendKeyAgain = false;
            }

            headers["ClientGuid"] = CurrentClientGuid;

            byte[] EncryptionIV;
            stream = EncryptionRijndael.Encrypt(FEncryptionKey, stream, out EncryptionIV);
            headers[EncryptionRijndael.GetEncryptionName()] = "Yes";

            // the initialisation vector is no secret, but we need to generate it for each encryption, and it is needed for decryption
            headers[EncryptionRijndael.GetEncryptionName() + "IV"] = Convert.ToBase64String(EncryptionIV);
        }

        /// <summary>
        /// decrypt the response
        /// </summary>
        protected void ProcessResponse(IMessage message, ITransportHeaders headers, ref Stream stream, object state)
        {
            if (headers[EncryptionRijndael.GetEncryptionName()] != null)
            {
                byte[] EncryptionIV = Convert.FromBase64String((String)headers[EncryptionRijndael.GetEncryptionName() + "IV"]);
                stream = EncryptionRijndael.Decrypt(FEncryptionKey, stream, EncryptionIV);
            }
        }
    }
}