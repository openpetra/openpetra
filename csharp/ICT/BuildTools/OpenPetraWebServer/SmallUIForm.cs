//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Ict.Common;
using Ict.Tools.OpenPetraRuntimeHost;

namespace Ict.Tools.OpenPetraWebServer
{
    /// <summary>
    /// The is the Form that is used when command line parameters are used
    /// </summary>
    public partial class SmallUIForm : Form, IOPWebServerManagerActions
    {
        private bool _exitMenuWasSelected = false;

        private WebSites _webSites = null;
        private WebSite _webSite = new WebSite();
        private WebSite _helpSite = null;
        private bool _allowRemoteConnection = false;
        private bool _started = false;
        private bool _haveShownBalloon = false;

        private bool _helpServerStarted = false;

        private ServerLogForm serverLog = new ServerLogForm();
        private ServerManagerListener _managerListener = null;

        /// <summary>
        /// This is the constructor that is used when command line parameters are used
        /// </summary>
        /// <param name="PhysicalPath"></param>
        /// <param name="VirtualPath"></param>
        /// <param name="Port"></param>
        /// <param name="DefaultPage"></param>
        /// <param name="AllowRemoteConnection"></param>
        /// <param name="LogPageRequests"></param>
        public SmallUIForm(string PhysicalPath, string VirtualPath, int Port, string DefaultPage, bool AllowRemoteConnection, bool LogPageRequests)
        {
            InitializeComponent();

            _webSite.PhysicalPath = PhysicalPath;
            _webSite.VirtualPath = VirtualPath;
            _webSite.Port = Port;
            _webSite.DefaultPage = DefaultPage;
            _webSite.LogPageRequests = LogPageRequests;

            _allowRemoteConnection = AllowRemoteConnection;

            txtPhysicalPath.Text = _webSite.PhysicalPath;
            txtVirtualPath.Text = _webSite.VirtualPath.Substring(1);
            txtPortNumber.Text = _webSite.Port.ToString();
            txtDefaultPage.Text = _webSite.DefaultPage;

            linkLabel.Text = _webSite.Url;
            chkAllowRemoteConnection.Checked = _allowRemoteConnection;
            chkLogPageRequests.Checked = _webSite.LogPageRequests;

            // Create a listener for ServerManager messages
            _webSites = new WebSites();
            _webSites.Add("FormSite", _webSite);
            _managerListener = new ServerManagerListener(Program.SERVER_MANAGER_BASE_PORT, this, _webSites);
            _managerListener.StartListening();

            // Create the help site and start the server unless another instance of the app has already done it
            _helpSite = Program.CreateHelpWebSite();

            if (!Program.PortIsInUse(_helpSite.Port, false) && (_helpSite.PhysicalPath != String.Empty))
            {
                _helpSite.WebServer = new Server(_helpSite.Port, _helpSite.VirtualPath, _helpSite.PhysicalPath, _helpSite.DefaultPage, false, false);
            }

            // Set the main window title
            this.Text = String.Format("{0} [{1}]", Program.ApplicationTitle, _webSite.Port.ToString());
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////
        // Message handlers for the GUI

        private void SmallUIForm_Shown(object sender, EventArgs e)
        {
            StartOrStopServer(true);

            timer.Enabled = true;
        }

        private void SmallUIForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();

                if (!_haveShownBalloon)
                {
                    string title = String.Format("{0} [{1}]", Program.ApplicationTitle, _webSite.Port.ToString());
                    this.notifyIcon.ShowBalloonTip(5000, title, Program.BALLOON_MESSAGE, ToolTipIcon.Info);
                    _haveShownBalloon = true;
                }
            }
        }

        private void SmallUIForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_exitMenuWasSelected)
            {
                if (_started && (MessageBox.Show(Program.SHUTDOWN_MESSAGE,
                                     Program.ApplicationTitle,
                                     MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question) == DialogResult.No))
                {
                    e.Cancel = true;
                    _exitMenuWasSelected = false;
                }
            }
            else
            {
                WindowState = FormWindowState.Minimized;
                e.Cancel = (e.CloseReason != CloseReason.WindowsShutDown);
            }

            if (!e.Cancel)
            {
                if (_helpServerStarted)
                {
                    _helpSite.WebServer.Stop();
                }

                if (_managerListener != null)
                {
                    _managerListener.StopListening();
                }

                StartOrStopServer(false);  //Stop the server if it is running
                notifyIcon.Dispose();

                TLogging.Log(String.Format("Shutting down ... (Reason: {0})", e.CloseReason.ToString()));
            }
        }

        private void UpdateUI()
        {
            if (_webSite != null)
            {
                linkLabel.Text = _webSite.Url;
            }

            linkLabel.Enabled = _webSite != null && _webSite.Url != String.Empty && _started;

            lblServerStatus.Text = _started ? "The server is running" : "The server has been stopped";
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel.Text);
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            if (!_helpServerStarted && (_helpSite != null) && (_helpSite.WebServer != null))
            {
                _helpSite.WebServer.Start();
                _helpServerStarted = true;
            }

            if (_helpSite.PhysicalPath == String.Empty)
            {
                MessageBox.Show("Could not find the help files in the ServerHelp folder.",
                    Program.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _helpSite.DefaultPage = "SimpleGUI.htm";
                System.Diagnostics.Process.Start(_helpSite.Url);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;
            Close();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        // Message handlers for the context menu

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            bool bIsOpen = this.WindowState == FormWindowState.Normal;

            openToolStripMenuItem.Text = bIsOpen ? "Bring To Front" : "Open";
            startToolStripMenuItem.Text = _started ? "Stop" : "Start";
            browseToolStripMenuItem.Enabled = linkLabel.Enabled;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _exitMenuWasSelected = true;
            Close();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartOrStopServer(!_started);
        }

        private void browseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            linkLabel_LinkClicked(null, null);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(null, null);
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnHelp_Click(null, null);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutMe dlg = new AboutMe(this.WindowState == FormWindowState.Normal);

            dlg.ShowDialog(this);
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        // Server methods

        private void StartOrStopServer(bool Start)
        {
            notifyIcon.Text = String.Format("{0} [{1}]\r\n", Program.ApplicationTitle, _webSite.Port.ToString());

            if (Start)
            {
                try
                {
                    _webSite.WebServer = new Server(_webSite.Port,
                        _webSite.VirtualPath,
                        _webSite.PhysicalPath,
                        _webSite.DefaultPage,
                        _allowRemoteConnection,
                        _webSite.LogPageRequests);
                    _webSite.WebServer.Start();
                    TLogging.Log("The web site server has started ...");
                }
                catch (Exception ex)
                {
                    TLogging.Log(String.Format("Exception occurred while starting the server: {0}", ex.Message));
                }

                _started = true;
            }
            else
            {
                if (_webSite.WebServer != null)
                {
                    _webSite.WebServer.Stop();
                    _webSite.WebServer = null;
                    TLogging.Log("The web site server has stopped ...");
                }

                _started = false;
            }

            if (_started)
            {
                notifyIcon.Text += "Running";
            }
            else
            {
                notifyIcon.Text += "Stopped";
            }

            UpdateUI();
        }

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            serverLog.Text = String.Format("Server Log (Port {0})", _webSite.Port.ToString());
            serverLog.Refresh();

            if (serverLog.WindowState == FormWindowState.Minimized)
            {
                serverLog.WindowState = FormWindowState.Normal;
            }
            else
            {
                serverLog.Show();
            }

            serverLog.BringToFront();
        }

        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnShowLog_Click(sender, e);
        }

        /// <summary>
        /// Call this when the server log text changes
        /// </summary>
        public void OnNewLogMessage()
        {
            serverLog.OnNewLogMessage();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //  Server Manager calls

        /// <summary>
        /// Take action in response to server manager to shutdown, optionally only if we are working on a specified port
        /// </summary>
        /// <param name="AWebPort">An optional web site port to be tested for a match</param>
        public void ManagerShutdown(int AWebPort = -1)
        {
            if (AWebPort > 0)
            {
                if (_webSite.Port != AWebPort)
                {
                    // Not for us!
                    return;
                }
            }

            _exitMenuWasSelected = true;
            _started = false;
            Close();
        }

        /// <summary>
        /// Take action in response to server manager to start all sites
        /// </summary>
        public void ManagerStartAll()
        {
            StartOrStopServer(true);
        }

        /// <summary>
        /// Take action in response to server manager to stop all sites
        /// </summary>
        public void ManagerStopAll()
        {
            StartOrStopServer(false);
        }

        /// <summary>
        /// Respond to server manager with server status
        /// </summary>
        public bool IsServerStarted()
        {
            return _started;
        }
    }
}