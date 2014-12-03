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
    /// Main GUI Window class
    /// </summary>
    public partial class MainForm : Form
    {
        private int _tickCount = 0;
        private bool _awaitingResponses = false;
        private bool _refreshOnCompletion = false;

        private WebServerListener _webServerListener = null;
        private List <string>_responses = new List <string>();
        private Dictionary <string, Dictionary <string, string>>_availableCommands;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            lblResponse.Top = listWebServers.Top;
            lblResponse.Text = "The application is searching for Open Petra web servers ...";

            btnFindServers_Click(null, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //
        /// <summary>
        /// Main method for sending a management message to Open Petra Web Servers.  Responses are handled by OnWebServerResponse.
        /// </summary>
        /// <param name="AMessage">Message to send</param>
        /// <param name="ASendToPort">Port to send to.  If -1 the message will be sent to our complete range of ports</param>
        private void SendMessage(string AMessage, int ASendToPort = -1)
        {
            PrepareToListen();

            _availableCommands = new Dictionary <string, Dictionary <string, string>>();

            // Create a new listener to handle the responses
            _webServerListener = new WebServerListener(this);
            _webServerListener.StartListening();

            // Check if the message needs completing, now that we know the listener port
            if (AMessage.Contains("{0}"))
            {
                AMessage = String.Format(AMessage, _webServerListener.Port);
            }

            System.Diagnostics.Trace.WriteLine("Starting listener ... Message sent: " + AMessage);

            // Send a message on the loopback address to all the ports in our range (because there may be multiple web servers listening on different ports)
            UdpClient udpClient = new UdpClient();
            byte[] bytes = Encoding.ASCII.GetBytes(AMessage);

            if (ASendToPort == -1)
            {
                for (int i = 0; i < Program.WEB_SERVER_TARGET_PORT_RANGE; i++)
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, Program.WEB_SERVER_TARGET_PORT_BASE + i);
                    udpClient.Send(bytes, bytes.Length, ep);
                }
            }
            else
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, ASendToPort);
                udpClient.Send(bytes, bytes.Length, ep);
            }

            // Finished sending
            udpClient.Close();

            System.Diagnostics.Trace.WriteLine("Finished sending...");
        }

        /// <summary>
        /// This ticks when our response timer is active
        /// </summary>
        private void responseTimer_Tick(object sender, EventArgs e)
        {
            _tickCount += responseTimer.Interval;

            // Wait for 2.5 seconds to get all the responses
            if (_tickCount > 2500)
            {
                OnResponseTimerCompleted();
            }
            else
            {
                progressResponses.Value = _tickCount;
            }
        }

        /// <summary>
        /// This is where we handle an individual response from a server and place it in our cache
        /// </summary>
        /// <param name="Response">Response string from server</param>
        public void OnWebServerResponse(string Response)
        {
            if (_awaitingResponses)
            {
                _responses.Add(Response);
                System.Diagnostics.Trace.WriteLine("Adding response: " + Response);
            }
        }

        /// <summary>
        /// Main method where we work out the meaning of the responses received
        /// </summary>
        private void OnResponseTimerCompleted()
        {
            _webServerListener.StopListening();
            lblResponse.Text = String.Empty;
            listWebServers.Items.Clear();

            if (_responses.Count > 0)
            {
                foreach (string response in _responses)
                {
                    string info;

                    if (!ParseIncomingMessage(response, out info))
                    {
                        lblResponse.Text += info;
                    }
                }
            }
            else
            {
                lblResponse.Text = "There are no Open Petra web server applications in the system tray.";
            }

            bool canShowList = (lblResponse.Text.Length == 0);
            listWebServers.Visible = canShowList;
            lblResponse.Visible = !canShowList;

            EndOfFind();
            System.Diagnostics.Trace.WriteLine("All responses handled and acted upon ...");

            if (_refreshOnCompletion)
            {
                RefreshServerList();
            }
        }

        /// <summary>
        /// Sets up the screen controls ready for listening
        /// </summary>
        private void PrepareToListen()
        {
            responseTimer.Start();
            _tickCount = 0;
            _awaitingResponses = true;
            progressResponses.Value = 0;
            _responses.Clear();

            this.Cursor = Cursors.WaitCursor;
            btnFindServers.Enabled = false;
        }

        /// <summary>
        /// Sets the screen controls in the light of responses received
        /// </summary>
        private void EndOfFind()
        {
            responseTimer.Stop();
            _awaitingResponses = false;
            progressResponses.Value = 0;

            bool canShowMenu = listWebServers.Visible;

            shutdownAllMainMenuItem.Enabled = canShowMenu;
            shutdownMainMenuItem.Enabled = canShowMenu;
            startMainMenuItem.Enabled = canShowMenu;
            stopMainMenuItem.Enabled = canShowMenu;

            if (canShowMenu)
            {
                listWebServers.Focus();
            }

            btnFindServers.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Parses an individual incoming message
        /// </summary>
        /// <param name="IncomingMessage">The message text</param>
        /// <param name="ExtraInfo">Sets any extra info that the GUI should display</param>
        /// <returns>True if the message can lead to displaying the list.  False if the GUI needs to act on ExtraInfo</returns>
        private bool ParseIncomingMessage(string IncomingMessage, out string ExtraInfo)
        {
            System.Diagnostics.Trace.WriteLine("Parsing; " + IncomingMessage);

            // Is the string terminated?
            if (!IncomingMessage.EndsWith(";EOF=1"))
            {
                ExtraInfo = "Incomplete response";
                return false;
            }

            int pos = IncomingMessage.IndexOf(';');

            if (pos == -1)
            {
                ExtraInfo = "Badly formed response";
                return false;
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
                case "ServerInfo":
                    ListViewItem row = new ListViewItem(dicParams["WebPort"]);
                    row.SubItems.Add(dicParams["Status"]);
                    row.SubItems.Add(dicParams["PhysicalPath"]);
                    row.Tag = dicParams["ReplyTo"];

                    if (listWebServers.Items.Count == 0)
                    {
                        row.Selected = true;
                    }

                    listWebServers.Items.Add(row);

                    Dictionary <string, string>dicCommands = new Dictionary <string, string>();
                    string shutdown = string.Format("ShutdownRequest;WebPort={0};ReplyTo={1};EOF=1", dicParams["WebPort"], _webServerListener.Port);
                    dicCommands.Add("ShutdownRequest", shutdown);
                    string stop = string.Format("StopRequest;WebPort={0};ReplyTo={1};EOF=1", dicParams["WebPort"], _webServerListener.Port);
                    dicCommands.Add("StopRequest", stop);
                    string start = string.Format("StartRequest;WebPort={0};ReplyTo={1};EOF=1", dicParams["WebPort"], _webServerListener.Port);
                    dicCommands.Add("StartRequest", start);

                    _availableCommands.Add(dicParams["ReplyTo"], dicCommands);
                    ExtraInfo = String.Empty;
                    return true;

                case "GeneralShutdownResponse":
                    ExtraInfo = string.Format("Web server monitoring port {0} has shut down" + Environment.NewLine, dicParams["WebPort"]);
                    return false;

                case "ShutdownResponse":
                    ExtraInfo = string.Format("Web server monitoring port {0} has shut down" + Environment.NewLine, dicParams["WebPort"]);
                    return false;

                case "StartResponse":
                    ExtraInfo = string.Format("Web server monitoring port {0} has started" + Environment.NewLine, dicParams["WebPort"]);
                    return false;

                case "StopResponse":
                    ExtraInfo = string.Format("Web server monitoring port {0} has stopped" + Environment.NewLine, dicParams["WebPort"]);
                    return false;
            }

            ExtraInfo = "Unknown response";
            return false;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Menu actions and other event handlers

        private void RefreshServerList()
        {
            btnFindServers_Click(null, null);
        }

        private void btnFindServers_Click(object sender, EventArgs e)
        {
            string message = "Is there a OP WebServer out there?;ReplyTo={0};EOF=1";

            _refreshOnCompletion = false;
            SendMessage(message, -1);
        }

        private void shutdownAllMainMenuItem_Click(object sender, EventArgs e)
        {
            string confirmMessage =
                "This action will shutdown all Open Petra web server applications that are running in the system tray.  Do you want to proceeed?";

            if (MessageBox.Show(confirmMessage, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string command = "GeneralShutdownRequest;ReplyTo={0};EOF=1";
            _refreshOnCompletion = true;
            SendMessage(command, -1);
        }

        private void shutdownMainMenuItem_Click(object sender, EventArgs e)
        {
            if (listWebServers.SelectedItems.Count == 0)
            {
                return;
            }

            string confirmMessage =
                "This action will shutdown the selected Open Petra web server application that is running in the system tray.  Do you want to proceeed?";

            if (MessageBox.Show(confirmMessage, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ListViewItem lvi = listWebServers.SelectedItems[0];
            string sendTo = lvi.Tag.ToString();
            string command = _availableCommands[sendTo]["ShutdownRequest"];

            _refreshOnCompletion = true;
            SendMessage(command, Convert.ToInt32(sendTo));
        }

        private void startMainMenuItem_Click(object sender, EventArgs e)
        {
            if (listWebServers.SelectedItems.Count == 0)
            {
                return;
            }

            ListViewItem lvi = listWebServers.SelectedItems[0];
            string sendTo = lvi.Tag.ToString();
            string command = _availableCommands[sendTo]["StartRequest"];

            _refreshOnCompletion = true;
            SendMessage(command, Convert.ToInt32(sendTo));
        }

        private void stopMainMenuItem_Click(object sender, EventArgs e)
        {
            if (listWebServers.SelectedItems.Count == 0)
            {
                return;
            }

            ListViewItem lvi = listWebServers.SelectedItems[0];
            string sendTo = lvi.Tag.ToString();
            string command = _availableCommands[sendTo]["StopRequest"];

            _refreshOnCompletion = true;
            SendMessage(command, Convert.ToInt32(sendTo));
        }

        private void shutdownAllContextMenuItem_Click(object sender, EventArgs e)
        {
            shutdownAllMainMenuItem_Click(null, null);
        }

        private void shutdownContextMenuItem_Click(object sender, EventArgs e)
        {
            shutdownMainMenuItem_Click(null, null);
        }

        private void startContextMenuItem_Click(object sender, EventArgs e)
        {
            startMainMenuItem_Click(null, null);
        }

        private void stopContextMenuItem_Click(object sender, EventArgs e)
        {
            stopMainMenuItem_Click(null, null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                string confirmMessage = "Are you sure you want to close the application?";

                if (MessageBox.Show(confirmMessage, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void listWebServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listWebServers.SelectedItems.Count == 0)
            {
                return;
            }

            ListViewItem lvi = listWebServers.SelectedItems[0];
            startMainMenuItem.Enabled = lvi.SubItems[1].Text == "Stopped";
            stopMainMenuItem.Enabled = lvi.SubItems[1].Text == "Started";

            startContextMenuItem.Enabled = startMainMenuItem.Enabled;
            stopContextMenuItem.Enabled = stopMainMenuItem.Enabled;
        }
    }
}