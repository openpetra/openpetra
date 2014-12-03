//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2014 by OM International
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
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Ict.Tools.OPWebServerManager
{
    /// <summary>
    /// This class listens for responses from OpenPetra development web servers
    /// </summary>
    class WebServerListener
    {
        private static Int32 _listenerPort = 0;

        private UdpClient _udpClient = null;
        IAsyncResult _ar;
        private Form _guiForm;

        /// <summary>
        /// Gets the port that we are listening on (which was assigned by the OS)
        /// </summary>
        public Int32 Port
        {
            get
            {
                return _listenerPort;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AGuiForm">The 'owner' form that created us</param>
        public WebServerListener(Form AGuiForm)
        {
            _guiForm = AGuiForm;
        }

        /// <summary>
        /// Call this method to start listening for responses.  The listener will use a port assigned by the OS
        /// </summary>
        public void StartListening()
        {
            bool bFirstUse = (_listenerPort == 0);

            // Create a client.  If this is our first time we get a port assigned by the system.
            // After that we continue to use the original port.
            _udpClient = new UdpClient(_listenerPort);

            if (bFirstUse)
            {
                _listenerPort = ((IPEndPoint)_udpClient.Client.LocalEndPoint).Port;
            }

            Listen();
        }

        /// <summary>
        /// Begin an asynchronous receive
        /// </summary>
        private void Listen()
        {
            _ar = _udpClient.BeginReceive(OnReceive, new object());
        }

        /// <summary>
        /// We have received a response
        /// </summary>
        /// <param name="ar"></param>
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, _listenerPort);
                byte[] bytes = _udpClient.EndReceive(ar, ref ep);
                string message = Encoding.ASCII.GetString(bytes);

                // Pass the response to the caller form so that it will unpack the message (it will know how to do this because it sent the outgoing request)
                if (_guiForm is MainForm)
                {
                    ((MainForm)_guiForm).OnWebServerResponse(message);
                }
                else if (_guiForm is ImmediateExecuteForm)
                {
                    ((ImmediateExecuteForm)_guiForm).OnWebServerResponse(message);
                }

                Listen();
            }
            catch (ObjectDisposedException)
            {
                // This sometimes happens when calling _udpClient.EndReceive()
            }
            catch (Exception ex)
            {
                // this should not happen
                MessageBox.Show("Receive exception: " + ex.Message, _guiForm.Text);
            }
        }

        /// <summary>
        /// Call this when we can stop listening
        /// </summary>
        public void StopListening()
        {
            if (_udpClient != null)
            {
                _udpClient.Close();
            }
        }
    }
}