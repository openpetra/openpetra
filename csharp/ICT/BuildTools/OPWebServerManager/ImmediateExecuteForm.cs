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
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Ict.Tools.OPWebServerManager
{
    /// <summary>
    /// Enumeration of actions that can be specified on the command line
    /// </summary>
    public enum ImmediateActions
    {
        /// <summary>
        /// No action specified
        /// </summary>
        None,

        /// <summary>
        /// Shut down all OP development web servers
        /// </summary>
        ShutdownAll,

        /// <summary>
        /// Shut down a specific OP development web server identified by the web port it is associated with
        /// </summary>
        Shutdown,

        /// <summary>
        /// Start a specific OP development web server identified by the web port it is associated with
        /// </summary>
        Start,

        /// <summary>
        /// Stop a specific OP development web server identified by the web port it is associated with
        /// </summary>
        Stop
    };

    /// <summary>
    /// The class that is the window displayed when executing an action from the command line
    /// </summary>
    public partial class ImmediateExecuteForm : Form
    {
        private ImmediateActions _runtimeAction = ImmediateActions.None;
        private int _port = 0;
        private WebServerListener _webServerListener = null;
        private int _tickCount = 0;
        private bool _awaitingResponses = false;
        private List <string>_responses = null;

        /// <summary>
        /// The main constructor
        /// </summary>
        /// <param name="ARuntimeAction">Action to perform</param>
        /// <param name="APort">Port number to act on, where relevant</param>
        public ImmediateExecuteForm(ImmediateActions ARuntimeAction, int APort)
        {
            InitializeComponent();

            _runtimeAction = ARuntimeAction;
            _port = APort;
        }

        /// <summary>
        /// The form has been displayed so we take initial actions here
        /// </summary>
        private void ImmediateExecuteForm_Shown(object sender, EventArgs e)
        {
            progressResponses.Value = 0;
            _tickCount = 0;
            _responses = new List <string>();
            responseTimer.Start();

            System.Diagnostics.Trace.WriteLine("Starting listener...");

            // Create a new listener to handle the responses
            _webServerListener = new WebServerListener(this);
            _webServerListener.StartListening();

            string messageToSend = String.Empty;

            switch (_runtimeAction)
            {
                case ImmediateActions.ShutdownAll:
                    messageToSend = String.Format("GeneralShutdownRequest;ReplyTo={0};EOF=1", _webServerListener.Port.ToString());
                    lblStatus.Text = "Trying to shutdown all Open Petra web servers";
                    break;

                case ImmediateActions.Shutdown:
                    messageToSend = String.Format("ShutdownRequest;WebPort={0};ReplyTo={1};EOF=1", _port, _webServerListener.Port.ToString());
                    lblStatus.Text = "Trying to shutdown the Open Petra web server on port " + _port.ToString();
                    break;

                case ImmediateActions.Start:
                    messageToSend = String.Format("StartRequest;WebPort={0};ReplyTo={1};EOF=1", _port, _webServerListener.Port.ToString());
                    lblStatus.Text = "Trying to start the Open Petra web server on port " + _port.ToString();
                    break;

                case ImmediateActions.Stop:
                    messageToSend = String.Format("StopRequest;WebPort={0};ReplyTo={1};EOF=1", _port, _webServerListener.Port.ToString());
                    lblStatus.Text = "Trying to stop the Open Petra web server on port " + _port.ToString();
                    break;
            }

            if (messageToSend == String.Empty)
            {
                lblStatus.Text = "Unknown command.  Cannot send message ...";
                _tickCount = 2000;
            }
            else
            {
                // Send a message on the loopback address to all the ports in our range (because there may be multiple web servers listening on different ports)
                UdpClient udpClient = new UdpClient();
                byte[] bytes = Encoding.ASCII.GetBytes(messageToSend);
                _awaitingResponses = true;

                for (int i = 0; i < Program.WEB_SERVER_TARGET_PORT_RANGE; i++)
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, Program.WEB_SERVER_TARGET_PORT_BASE + i);
                    udpClient.Send(bytes, bytes.Length, ep);
                }

                // Finished sending
                udpClient.Close();
            }

            System.Diagnostics.Trace.WriteLine("Finished sending...");
        }

        /// <summary>
        /// Called when there has been a response from an OP web server
        /// </summary>
        /// <param name="Response"></param>
        public void OnWebServerResponse(string Response)
        {
            _responses.Add(Response);
            System.Diagnostics.Trace.WriteLine("Adding response: " + Response);
        }

        /// <summary>
        /// Fired by the response timer every 200 milliseconds
        /// </summary>
        private void responseTimer_Tick(object sender, EventArgs e)
        {
            _tickCount += responseTimer.Interval;

            if (_tickCount > 4500)
            {
                responseTimer.Stop();
                Close();
            }
            else if (_tickCount > 2500)
            {
                if (_awaitingResponses)
                {
                    // Wait for 2.5 seconds to get all the responses
                    OnResponseTimerCompleted();
                    _awaitingResponses = false;
                }
            }
            else
            {
                progressResponses.Value = _tickCount;
            }
        }

        /// <summary>
        /// Called when the time is up for receiving responses
        /// </summary>
        private void OnResponseTimerCompleted()
        {
            _webServerListener.StopListening();

            // Check the responses
            if (_responses.Count > 0)
            {
                lblStatus.Text = String.Empty;

                foreach (string response in _responses)
                {
                    string info;
                    ParseIncomingMessage(response, out info);
                    lblStatus.Text += info;
                }
            }
            else
            {
                lblStatus.Text = "No responses recieved";
            }
        }

        /// <summary>
        /// Parse a response from the OP web server
        /// </summary>
        /// <param name="IncomingMessage">Message to parse</param>
        /// <param name="ExtraInfo">Resulting output message to user</param>
        private void ParseIncomingMessage(string IncomingMessage, out string ExtraInfo)
        {
            System.Diagnostics.Trace.WriteLine("Parsing; " + IncomingMessage);

            // Is the string terminated?
            if (!IncomingMessage.EndsWith(";EOF=1"))
            {
                ExtraInfo = "Incomplete response" + Environment.NewLine;
                return;
            }

            int pos = IncomingMessage.IndexOf(';');

            if (pos == -1)
            {
                ExtraInfo = "Badly formed response" + Environment.NewLine;
                return;
            }

            string command = IncomingMessage.Substring(0, pos);
            string[] parameters = IncomingMessage.Split(';');

            Dictionary <string, string>dicParams = new Dictionary <string, string>();

            foreach (string p in parameters)
            {
                string[] kvp = p.Split('=');

                if (kvp.Length > 1)
                {
                    dicParams.Add(kvp[0], kvp[1]);
                }
            }

            switch (command)
            {
                case "GeneralShutdownResponse":
                    ExtraInfo = string.Format("Web server monitoring port {0} has shut down" + Environment.NewLine, dicParams["WebPort"]);
                    return;

                case "ShutdownResponse":
                    ExtraInfo = string.Format("Web server monitoring port {0} has shut down" + Environment.NewLine, dicParams["WebPort"]);
                    return;

                case "StartResponse":
                    ExtraInfo = string.Format("Web server monitoring port {0} has started" + Environment.NewLine, dicParams["WebPort"]);
                    return;

                case "StopResponse":
                    ExtraInfo = string.Format("Web server monitoring port {0} has stopped" + Environment.NewLine, dicParams["WebPort"]);
                    return;
            }

            ExtraInfo = "Unknown response" + Environment.NewLine;
            return;
        }
    }
}