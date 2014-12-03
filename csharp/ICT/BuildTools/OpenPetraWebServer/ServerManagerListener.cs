//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       >>>> Put your full name or just a shortname here <<<<
//
// Copyright 2004-2013 by OM International
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
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using Ict.Common;

namespace Ict.Tools.OpenPetraWebServer
{
    /// <summary>
    /// Class that listens for UDP messages broadcast by the Server Manager application
    /// These messages can, for example, instruct the OP Web Server to shut down
    /// </summary>
    public class ServerManagerListener
    {
        private Int32 _listenerBasePort = 0;
        private Int32 _listenerPort = 0;
        private UdpClient _udpClient = null;
        private IOPWebServerManagerActions _guiForm;
        private IAsyncResult _ar;

        private WebSites _webSites = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AListenerBasePort">Base port to listen on.  The actual port used will be the first available one in a range of ports starting here</param>
        /// <param name="AGuiForm">The form that owns this class and handles messages</param>
        /// <param name="AWebSiteList">A list of WebSite objects that are to be reported to the Manager</param>
        public ServerManagerListener(Int32 AListenerBasePort, IOPWebServerManagerActions AGuiForm, WebSites AWebSiteList)
        {
            _listenerBasePort = AListenerBasePort;
            _guiForm = AGuiForm;

            _webSites = AWebSiteList;
        }

        /// <summary>
        /// Call this to create a new instance of a listener and start listening
        /// </summary>
        public void StartListening()
        {
            const int MAX_TRIES = 32;

            Int32 tryPort = _listenerBasePort;
            int tryCount = 0;
            bool done = false;

            while (!done)
            {
                try
                {
                    _udpClient = new UdpClient(tryPort);
                    _listenerPort = tryPort;
                    done = true;
                }
                catch
                {
                    if (tryCount < MAX_TRIES)
                    {
                        tryCount++;
                        tryPort++;
                    }
                    else
                    {
                        done = true;
                    }
                }
            }

            if (tryCount == MAX_TRIES)
            {
                // There are too many server instances running!
            }
            else
            {
                Listen();
            }
        }

        private void Listen()
        {
            _ar = _udpClient.BeginReceive(OnReceive, new object());
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint receiveEp = new IPEndPoint(IPAddress.Any, _listenerPort);
                byte[] bytes = _udpClient.EndReceive(ar, ref receiveEp);
                string receivedMessage = Encoding.ASCII.GetString(bytes);

                // Unpack the message and send a response
                System.Diagnostics.Trace.WriteLine("Received message: " + receivedMessage + " from " + receiveEp.Port.ToString());

                int replyPort;
                string replyMessage = HandleResponse(receivedMessage, out replyPort);

                if (replyPort != 0)
                {
                    UdpClient manager = new UdpClient();
                    IPEndPoint replyEp = new IPEndPoint(IPAddress.Loopback, replyPort);
                    byte[] replyBytes = Encoding.ASCII.GetBytes(replyMessage);
                    manager.Send(replyBytes, replyBytes.Length, replyEp);
                }

                TakeAction(replyMessage);
            }
            catch (ObjectDisposedException)
            {
                // This sometimes happens when calling _udpClient.EndReceive()
            }
            catch (Exception ex)
            {
                // this should not happen
                MessageBox.Show("Receive exception: " + ex.Message, Program.ApplicationTitle);
            }
        }

        /// <summary>
        /// Stop listening for Manager messages.  Call this when the application closes
        /// </summary>
        public void StopListening()
        {
            try
            {
                _udpClient.Close();
            }
            catch
            {
                // Don't care
            }
        }

        private string HandleResponse(string IncomingMessage, out int ReplyPort)
        {
            string returnValue = String.Empty;

            ReplyPort = 0;

            // Is the string terminated?
            if (!IncomingMessage.EndsWith(";EOF=1"))
            {
                return returnValue;
            }

            int pos = IncomingMessage.IndexOf(';');

            if (pos == -1)
            {
                return returnValue;
            }

            string command = IncomingMessage.Substring(0, pos);
            string[] parameters = IncomingMessage.Split(';');

            Dictionary <string, string>paramDic = new Dictionary <string, string>();

            foreach (string p in parameters)
            {
                string[] kvp = p.Split('=');

                if (kvp.Length > 1)
                {
                    paramDic.Add(kvp[0], kvp[1]);
                }
            }

            string myWebPorts = String.Empty;
            string physicalPath = String.Empty;
            string started = String.Empty;

            foreach (WebSite ws in _webSites.Values)
            {
                if (myWebPorts.Length > 0)
                {
                    myWebPorts += "+";
                    physicalPath = "Various";
                }
                else
                {
                    physicalPath = ws.PhysicalPath;
                }

                myWebPorts += ws.Port.ToString();
            }

            string matchMyWebPorts = "+" + myWebPorts + "+";

            switch (command)
            {
                case "Is there a OP WebServer out there?":

                    if (paramDic.ContainsKey("ReplyTo"))
                    {
                        ReplyPort = Convert.ToInt32(paramDic["ReplyTo"]);
                    }

                    returnValue = String.Format("ServerInfo;ReplyTo={0};WebPort={1};CommandArgs={2};PhysicalPath={3};Status={4};EOF=1",
                    _listenerPort.ToString(),
                    myWebPorts,
                    Program.CommandLineParams,
                    physicalPath,
                    _guiForm.IsServerStarted() ? "Started" : "Stopped");

                    break;

                case "GeneralShutdownRequest":

                    if (paramDic.ContainsKey("ReplyTo"))
                    {
                        ReplyPort = Convert.ToInt32(paramDic["ReplyTo"]);
                    }

                    returnValue = String.Format("GeneralShutdownResponse;WebPort={0};EOF=1", myWebPorts);

                    break;

                case "ShutdownRequest":

                    if (paramDic.ContainsKey("WebPort"))
                    {
                        if (!matchMyWebPorts.Contains("+" + paramDic["WebPort"] + "+"))
                        {
                            break;
                        }
                    }

                    if (paramDic.ContainsKey("ReplyTo"))
                    {
                        ReplyPort = Convert.ToInt32(paramDic["ReplyTo"]);
                    }

                    returnValue = String.Format("ShutdownResponse;WebPort={0};EOF=1", myWebPorts);

                    break;

                case "StartRequest":

                    if (paramDic.ContainsKey("WebPort"))
                    {
                        if (!matchMyWebPorts.Contains("+" + paramDic["WebPort"] + "+"))
                        {
                            break;
                        }
                    }

                    if (paramDic.ContainsKey("ReplyTo"))
                    {
                        ReplyPort = Convert.ToInt32(paramDic["ReplyTo"]);
                    }

                    returnValue = String.Format("StartResponse;WebPort={0};EOF=1", myWebPorts);

                    break;

                case "StopRequest":

                    if (paramDic.ContainsKey("WebPort"))
                    {
                        if (!matchMyWebPorts.Contains("+" + paramDic["WebPort"] + "+"))
                        {
                            break;
                        }
                    }

                    if (paramDic.ContainsKey("ReplyTo"))
                    {
                        ReplyPort = Convert.ToInt32(paramDic["ReplyTo"]);
                    }

                    returnValue = String.Format("StopResponse;WebPort={0};EOF=1", myWebPorts);

                    break;
            }

            return returnValue;
        }

        private void TakeAction(string AResponse)
        {
            if (AResponse.StartsWith("GeneralShutdownResponse;"))
            {
                _guiForm.ManagerShutdown();
                return;
            }
            else if (AResponse.StartsWith("ShutdownResponse;"))
            {
                _guiForm.ManagerShutdown();
                return;
            }
            else if (AResponse.StartsWith("StartResponse;"))
            {
                _guiForm.ManagerStartAll();
            }
            else if (AResponse.StartsWith("StopResponse;"))
            {
                _guiForm.ManagerStopAll();
            }

            Listen();
        }
    }
}