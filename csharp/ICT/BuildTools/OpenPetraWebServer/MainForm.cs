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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Ict.Common;
using Ict.Tools.OpenPetraRuntimeHost;

namespace Ict.Tools.OpenPetraWebServer
{
    /// <summary>
    /// This class is the main multi-site UI for the Open Petra Web Server application.
    ///
    /// The application as a whole integrates with the Microsoft Cassini web server code
    /// which is preserved more or less in tact with the exception that 'cosmetic' options
    /// to allow remote connections and to specify a default page have been added.
    ///
    /// Unlike IIS, multiple web sites must be on different ports - a separate instance of the
    /// Cassini web server is created for each site.
    /// </summary>
    public partial class MainForm : Form, IOPWebServerManagerActions
    {
        private WebSites _webSites = null;
        private WebSite _helpSite = null;
        private bool _exitMenuWasSelected = false;
        private bool _started = false;
        private bool _startAutomatically = true;
        private bool _hideAtStartup = true;
        private bool _allowRemoteConnections = false;
        private bool _haveShownBalloon = false;
        private bool _helpServerStarted = false;

        private ServerLogForm serverLog = new ServerLogForm();
        private ServerManagerListener _managerListener = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            //read in the site info from the settings xml file
            _webSites = new WebSites();
            _webSites.Read(out _startAutomatically, out _hideAtStartup, out _allowRemoteConnections);

            // Create a listener for ServerManager messages
            _managerListener = new ServerManagerListener(Program.SERVER_MANAGER_BASE_PORT, this, _webSites);
            _managerListener.StartListening();

            // Create the help site and start the server unless another instance of the app has already done it
            _helpSite = Program.CreateHelpWebSite();

            if (!Program.PortIsInUse(_helpSite.Port, false) && (_helpSite.PhysicalPath != String.Empty))
            {
                _helpSite.WebServer = new Server(_helpSite.Port, _helpSite.VirtualPath, _helpSite.PhysicalPath, _helpSite.DefaultPage, false, false);
            }

            //Populate the list box and update the link label
            UpdateUI();

            //If we are set to start the servers automatically, do it now
            if (_startAutomatically)
            {
                toolStripStatusLabel.Text = StartOrStopAll(true);
            }
            else
            {
                //otherwise just update the tray icon text
                notifyIcon.Text = Program.ApplicationTitleAndVersion;
                notifyIcon.Text += "\r\nStopped";
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Left = Screen.PrimaryScreen.Bounds.Width - Width - 50;
            Top = Screen.PrimaryScreen.Bounds.Height - Height - 50;

            if (_hideAtStartup)
            {
                startTimer.Enabled = true;
            }
        }

        private void startTimer_Tick(object sender, EventArgs e)
        {
            // Minimise the form to the system tray
            WindowState = FormWindowState.Minimized;
            startTimer.Enabled = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
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

                StartOrStopAll(false);  //Stop all servers
                notifyIcon.Dispose();
                _webSites.Save(_allowRemoteConnections, _hideAtStartup, _startAutomatically);

                TLogging.Log(String.Format("Shutting down ... (Reason: {0})", e.CloseReason.ToString()));
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();

                if (!_haveShownBalloon)
                {
                    this.notifyIcon.ShowBalloonTip(5000, Program.ApplicationTitle, Program.BALLOON_MESSAGE, ToolTipIcon.Info);
                    _haveShownBalloon = true;
                }
            }
        }

/**********************************************************************************
 * Server Code
 * *******************************************************************************/

        private string StartOrStopAll(bool Start)
        {
            // The main function to start or stop the web site servers

            //Make a start on the return text and setting up the icon text
            string s = (Start ? "Started" : "Stopped");

            if (_webSites.Count == 0)
            {
                s = "Click 'Sites' and 'Add site...'";
            }

            // NB:  Only 64 characters allowed for this
            // AppTitle has 21, version adds 10
            notifyIcon.Text = Program.ApplicationTitleAndVersion;
            notifyIcon.Text += "\r\n";

            try
            {
                //Start or stop each site in turn
                foreach (KeyValuePair <string, WebSite>kvp in _webSites)
                {
                    if (Start)
                    {
                        kvp.Value.WebServer = new Server(kvp.Value.Port,
                            kvp.Value.VirtualPath,
                            kvp.Value.PhysicalPath,
                            kvp.Value.DefaultPage,
                            _allowRemoteConnections,
                            kvp.Value.LogPageRequests);
                        kvp.Value.WebServer.Start();
                        TLogging.Log("Server started on port " + kvp.Value.Port.ToString());
                    }
                    else
                    {
                        if (kvp.Value.WebServer != null)
                        {
                            kvp.Value.WebServer.Stop();
                        }

                        kvp.Value.WebServer = null;
                        TLogging.Log("Server stopped on port " + kvp.Value.Port.ToString());
                    }
                }

                //set our private _started variable
                _started = Start && (_webSites.Count > 0);

                //complete the icon text for a successful outcome
                if (_webSites.Count == 0)
                {
                    notifyIcon.Text += "No web sites specified";
                }
                else if (_started)
                {
                    notifyIcon.Text += "Running";
                }
                else
                {
                    notifyIcon.Text += "Stopped";
                }
            }
            catch (Exception)
            {
                //Handle an error in start/stop code
                if (Start)
                {
                    s = "Failed to start all sites.";
                    TLogging.Log(s);
                }
                else
                {
                    s = "Error closing all sites.";
                    TLogging.Log(s);
                }

                notifyIcon.Text += s;

                // We will say that we have started then the menu options will be 'stop'
                _started = true;
            }

            //Update the menu options
            if (_started)
            {
                startStopAllMenuItem.Text = "&Stop All";
            }
            else
            {
                startStopAllMenuItem.Text = "&Start All";
            }

            if (_started)
            {
                startStopNotifyMenuItem.Text = "Stop All";
            }
            else
            {
                startStopNotifyMenuItem.Text = "Start All";
            }

            UpdateUI();

            return s;
        }

/************************************************************************************
 * Message handlers
 * *********************************************************************************/

        private void openNotifyMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            this.Activate();                    // we need this when we are activated by another potential app instance
        }

        private void exitNotifyMenuItem_Click(object sender, EventArgs e)
        {
            _exitMenuWasSelected = true;
            Close();
        }

        private void closeNotifyMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            openNotifyMenuItem_Click(null, null);
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel.Text);
        }

        private void startStopAllMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = StartOrStopAll(!_started);
        }

        private void startStopAllNotifyMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = StartOrStopAll(!_started);
        }

        private void allowRemoteConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _allowRemoteConnections = !_allowRemoteConnections;
        }

        private void allowRemoteConnectionsNotifyMenuItem_Click(object sender, EventArgs e)
        {
            _allowRemoteConnections = !_allowRemoteConnections;
        }

        private void startAutomaticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _startAutomatically = !_startAutomatically;
        }

        private void startAutomaticallyNotifyStripMenuItem_Click(object sender, EventArgs e)
        {
            _startAutomatically = !_startAutomatically;
        }

        private void hideWindowAtStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _hideAtStartup = !_hideAtStartup;
        }

        private void hideWindowAtStartupNotifyMenuItem_Click(object sender, EventArgs e)
        {
            _hideAtStartup = !_hideAtStartup;
        }

        private void propertiesNotifyMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            ShowSiteProperties(item.Text);
        }

        private void listSites_DoubleClick(object sender, EventArgs e)
        {
            propertiesToolStripMenuItem_Click(null, null);
        }

        private void browseSiteNotifyMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            string sKey = GetKeyForListEntry(item.Text);

            System.Diagnostics.Process.Start(_webSites[sKey].Url);
        }

        private void listSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listSites.SelectedIndex < 0)
            {
                return;
            }

            string sKey = GetKeyForListEntry();
            linkLabel.Text = _webSites[sKey].Url;
            linkLabel.LinkVisited = false;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sKey = GetKeyForListEntry();
            string s = "Are you sure that you want to remove the " +
                       sKey +
                       " site?";

            if (MessageBox.Show(s,
                    Program.ApplicationTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            _webSites.Remove(sKey);
            _webSites.Save(_allowRemoteConnections, _hideAtStartup, _startAutomatically);

            UpdateUI();
        }

/***********************************************************************************************
 * Menu updates
 * ********************************************************************************************/

        private void notifyIconMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            bool bIsOpen = this.WindowState == FormWindowState.Normal;

            openNotifyMenuItem.Text = bIsOpen ? "Bring To Front" : "Open";
            allowRemoteConnectionsNotifyMenuItem.Checked = _allowRemoteConnections;
            allowRemoteConnectionsNotifyMenuItem.Enabled = !_started;
            startAutomaticallyNotifyStripMenuItem.Checked = _startAutomatically;
            startStopNotifyMenuItem.Enabled = (_webSites.Count > 0);
            hideWindowAtStartupNotifyMenuItem.Checked = _hideAtStartup;

            browseNotifyMenuItemPopup.Visible = _webSites.Count > 0;
            propertiesNotifyMenuItemPopup.Visible = _webSites.Count > 0;

            if (_webSites.Count == 0)
            {
                return;
            }

            browseNotifyMenuItemPopup.DropDownItems.Clear();
            propertiesNotifyMenuItemPopup.DropDownItems.Clear();
            int i = 0;

            foreach (KeyValuePair <string, WebSite>kvp in _webSites)
            {
                i++;
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = "browseMenuItem" + i;
                item.Text = kvp.Key;
                item.Size = new Size(150, 22);
                item.Click += new EventHandler(browseSiteNotifyMenuItem_Click);

                browseNotifyMenuItemPopup.DropDownItems.Add(item);

                item = new ToolStripMenuItem();
                item.Name = "propertiesMenuItem" + i;
                item.Text = kvp.Key;
                item.Size = new Size(150, 22);
                item.Click += new EventHandler(propertiesNotifyMenuItem_Click);

                propertiesNotifyMenuItemPopup.DropDownItems.Add(item);
            }
        }

        private void sitesToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            bool bEnable = (listSites.SelectedIndex >= 0);

            propertiesToolStripMenuItem.Enabled = bEnable;
            removeToolStripMenuItem.Enabled = bEnable && !_started;
            startAutomaticallyToolStripMenuItem.Checked = _startAutomatically;
            allowRemoteConnectionsToolStripMenuItem.Enabled = !_started;
            allowRemoteConnectionsToolStripMenuItem.Checked = _allowRemoteConnections;
            startStopAllMenuItem.Enabled = (_webSites.Count > 0);
            hideWindowAtStartupToolStripMenuItem.Checked = _hideAtStartup;
            addSiteToolStripMenuItem.Enabled = !_started;
        }

/********************************************************************************************
 * Helper functions relating to the UI
 * *****************************************************************************************/

        private void UpdateUI()
        {
            //populate the list box
            listSites.Items.Clear();

            foreach (KeyValuePair <string, WebSite>kvp in _webSites)
            {
                string s = String.Format("{0} ( on port {1} )", kvp.Key, kvp.Value.Port.ToString());
                listSites.Items.Add(s);
            }

            bool bHaveSites = listSites.Items.Count > 0;

            //update the link label
            linkLabel.Visible = bHaveSites;
            linkLabel.Enabled = _started;

            if (bHaveSites)
            {
                listSites.SelectedIndex = 0;
            }

            startButton.Enabled = bHaveSites && !_started;
            stopButton.Enabled = bHaveSites && _started;
            addButton.Enabled = !_started;
            removeButton.Enabled = bHaveSites && !_started;
            propertiesButton.Enabled = bHaveSites;
        }

        private string GetKeyForListEntry(string EntryText)
        {
            return EntryText.Split(new char[] { '(' })[0].Trim();
        }

        private string GetKeyForListEntry()
        {
            Trace.Assert(listSites.SelectedIndex >= 0);
            return listSites.Items[listSites.SelectedIndex].ToString().Split(new char[] { '(' })[0].Trim();
        }

        /// <summary>
        /// Shows the help in a browser
        /// </summary>
        public void ShowHelpWindow(object sender, EventArgs e)
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
                if (sender is SitePropertiesDialog)
                {
                    _helpSite.DefaultPage = "EditSiteProperties.htm";
                }
                else
                {
                    _helpSite.DefaultPage = "FullGUI.htm";
                }

                System.Diagnostics.Process.Start(_helpSite.Url);
            }
        }

/*******************************************************************************************
 * Dialog actions
 * ****************************************************************************************/

        private void aboutOpenPetraWebServerMenuItem_Click(object sender, EventArgs e)
        {
            AboutMe dlg = new AboutMe(true);

            dlg.ShowDialog(this);
        }

        private void aboutNotifyMenuItem_Click(object sender, EventArgs e)
        {
            AboutMe dlg = new AboutMe(this.WindowState == FormWindowState.Normal);

            dlg.ShowDialog(this);
        }

        private void addSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SitePropertiesDialog dlg = new SitePropertiesDialog(SitePropertiesDialog.OpenMode.MODE_ADD,
                _webSites,
                null);

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            WebSite ws = new WebSite();
            ws.DefaultPage = dlg.txtDefaultPage.Text;
            ws.PhysicalPath = dlg.txtPhysicalPath.Text;
            ws.Port = Convert.ToInt32(dlg.txtPortNumber.Text);
            ws.VirtualPath = dlg.txtVirtualPath.Text;

            _webSites.Add(dlg.txtKeyName.Text, ws);
            _webSites.Save(_allowRemoteConnections, _hideAtStartup, _startAutomatically);

            UpdateUI();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sKey = GetKeyForListEntry();

            ShowSiteProperties(sKey);
        }

        private void ShowSiteProperties(string sKey)
        {
            SitePropertiesDialog dlg = new SitePropertiesDialog(
                _started ? SitePropertiesDialog.OpenMode.MODE_READONLY : SitePropertiesDialog.OpenMode.MODE_EDIT,
                _webSites,
                sKey);

            dlg.txtKeyName.Text = sKey;
            dlg.txtDefaultPage.Text = _webSites[sKey].DefaultPage;
            dlg.txtPhysicalPath.Text = _webSites[sKey].PhysicalPath;
            dlg.txtPortNumber.Text = _webSites[sKey].Port.ToString();
            dlg.txtVirtualPath.Text = _webSites[sKey].VirtualPath.Substring(1);
            dlg.chkLogPageRequests.Checked = _webSites[sKey].LogPageRequests;
            dlg.txtKeyName.Enabled = !_started;
            dlg.txtDefaultPage.Enabled = !_started;
            dlg.txtPhysicalPath.Enabled = !_started;
            dlg.txtPortNumber.Enabled = !_started;
            dlg.txtVirtualPath.Enabled = !_started;
            dlg.btnOk.Enabled = !_started;

            if (dlg.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }

            _webSites.Remove(sKey);

            WebSite ws = new WebSite();
            ws.DefaultPage = dlg.txtDefaultPage.Text;
            ws.PhysicalPath = dlg.txtPhysicalPath.Text;
            ws.Port = Convert.ToInt32(dlg.txtPortNumber.Text);
            ws.VirtualPath = dlg.txtVirtualPath.Text;
            ws.LogPageRequests = dlg.chkLogPageRequests.Checked;

            _webSites.Add(dlg.txtKeyName.Text, ws);
            _webSites.Save(_allowRemoteConnections, _hideAtStartup, _startAutomatically);

            UpdateUI();
        }

        private void helpContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowHelpWindow(this, null);
        }

        private void helpNotifyMenuItem_Click(object sender, EventArgs e)
        {
            ShowHelpWindow(this, null);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = StartOrStopAll(true);
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = StartOrStopAll(false);
        }

        private void propertiesButton_Click(object sender, EventArgs e)
        {
            propertiesToolStripMenuItem_Click(null, null);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            addSiteToolStripMenuItem_Click(null, null);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            removeToolStripMenuItem_Click(null, null);
        }

        /// <summary>
        /// Override of message handler
        /// </summary>
        /// <param name="message"></param>
        protected override void WndProc(ref Message message)
        {
            // We just check for our magic number to see if we have been activated by another instance
            if (message.Msg == Program.UM_ACTIVATE_APP)
            {
                openNotifyMenuItem_Click(null, null);
            }
            else if (message.Msg == Program.UM_CLOSE_APP)
            {
                exitNotifyMenuItem_Click(null, null);
            }

            base.WndProc(ref message);
        }

        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serverLog.Text = "Server Log";
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
                bool foundMatch = false;

                foreach (WebSite ws in _webSites.Values)
                {
                    foundMatch = (AWebPort == ws.Port);

                    if (foundMatch)
                    {
                        break;
                    }
                }

                if (!foundMatch)
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
            StartOrStopAll(true);
        }

        /// <summary>
        /// Take action in response to server manager to stop all sites
        /// </summary>
        public void ManagerStopAll()
        {
            StartOrStopAll(false);
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