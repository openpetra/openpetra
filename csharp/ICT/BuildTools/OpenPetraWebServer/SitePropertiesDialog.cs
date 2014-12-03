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
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Ict.Tools.OpenPetraWebServer
{
    public partial class SitePropertiesDialog : Form
    {
        /// <summary>
        /// Modes in which this screen may be used
        /// </summary>
        public enum OpenMode
        {
            /// <summary>
            /// Open in add mode
            /// </summary>
            MODE_ADD,

            /// <summary>
            /// Open in edit mode
            /// </summary>
            MODE_EDIT,

            /// <summary>
            /// Open in readonly mode
            /// </summary>
            MODE_READONLY
        }

        private OpenMode _openMode = OpenMode.MODE_EDIT;
        private WebSites _webSites = null;
        private string _activeSitekey = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public SitePropertiesDialog(OpenMode mode, WebSites Sites, string ActiveSiteKey)
        {
            _openMode = mode;
            _webSites = Sites;
            _activeSitekey = ActiveSiteKey;

            InitializeComponent();
        }

        private void SitePropertiesDialog_Load(object sender, EventArgs e)
        {
            btnOk.Text = (_openMode == OpenMode.MODE_ADD) ? "Add" : "Modify";
            btnOk.Enabled = (_openMode != OpenMode.MODE_READONLY);

            if (_openMode == OpenMode.MODE_ADD)
            {
                lblInfo.Text = "Enter the properties for the new web site.";
            }

            btnBrowse.Enabled = (_openMode != OpenMode.MODE_READONLY);
            btnPageBrowse.Enabled = btnBrowse.Enabled && txtPhysicalPath.Text != "";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                DialogResult = DialogResult.None;
            }
        }

        private bool ValidateInput()
        {
            string sTry = txtKeyName.Text.Trim();

            if (sTry.Length == 0)
            {
                MessageBox.Show("You must enter a friendly name for this web site.",
                    Program.ApplicationTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtKeyName.Focus();
                txtKeyName.SelectAll();
                return false;
            }

            if (sTry.IndexOf('(') >= 0)
            {
                // Not allowed because we split the list items on open-brace
                MessageBox.Show("The friendly name cannot contain brackets ().",
                    Program.ApplicationTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtKeyName.Focus();
                txtKeyName.SelectAll();
                return false;
            }

            if ((_activeSitekey != null) && (sTry != _activeSitekey) && _webSites.ContainsKey(sTry))
            {
                MessageBox.Show("The friendly name you have chosen has already been used for a different web site.",
                    Program.ApplicationTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtKeyName.Focus();
                txtKeyName.SelectAll();
                return false;
            }

            sTry = txtVirtualPath.Text.Trim();

            char[] chars = sTry.ToCharArray();
            bool bOk = true;

            foreach (char c in chars)
            {
                if (((c >= 'A') && (c <= 'Z')) || ((c >= 'a') && (c <= 'z')) || ((c >= '0') && (c <= '9')) || (c == '/'))
                {
                    continue;
                }
                else
                {
                    bOk = false;
                    break;
                }
            }

            if (!bOk)
            {
                MessageBox.Show("The format of the virtual path is not valid.  It must be made up of alphanumeric characters only.",
                    Program.ApplicationTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtVirtualPath.Focus();
                txtVirtualPath.SelectAll();
                return false;
            }

            sTry = txtPortNumber.Text.Trim();
            int n;

            if (!Int32.TryParse(sTry, out n))
            {
                MessageBox.Show("Please enter a port number.",
                    Program.ApplicationTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtPortNumber.Focus();
                txtPortNumber.SelectAll();
                return false;
            }

            if ((n < 80) || (n > 65535))
            {
                MessageBox.Show("The port number is not valid.  It should be between 80 and 65535.",
                    Program.ApplicationTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtPortNumber.Focus();
                txtPortNumber.SelectAll();
                return false;
            }

            if (n == Program.HELP_PORT)
            {
                MessageBox.Show("The selected port number is used by the Help System.  Please choose a different port",
                    Program.ApplicationTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtPortNumber.Focus();
                txtPortNumber.SelectAll();
                return false;
            }

            sTry = txtPhysicalPath.Text.ToLower().Trim();

            if (sTry == "")
            {
                MessageBox.Show("Please enter a physical path.",
                    Program.ApplicationTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtPhysicalPath.Focus();
                txtPhysicalPath.SelectAll();
                return false;
            }

            if (!Directory.Exists(sTry))
            {
                MessageBox.Show("The physical path does not exist.",
                    Program.ApplicationTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtPhysicalPath.Focus();
                txtPhysicalPath.SelectAll();
                return false;
            }

            foreach (KeyValuePair <string, WebSite>kvp in _webSites)
            {
                //the active key will be gone so we don't need to validate against that one
                if ((_activeSitekey != null) && kvp.Key.Equals(_activeSitekey))
                {
                    continue;
                }

                n = Convert.ToInt32(txtPortNumber.Text);

                if (kvp.Value.Port == n)
                {
                    MessageBox.Show("The port number is the same as another site that is already in use.",
                        Program.ApplicationTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    txtPortNumber.Focus();
                    txtPortNumber.SelectAll();
                    return false;
                }

                sTry = txtPhysicalPath.Text;

                if (kvp.Value.PhysicalPath.ToLower().Equals(sTry))
                {
                    MessageBox.Show("The physical path is the same as another site that is already in use.",
                        Program.ApplicationTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    txtPhysicalPath.Focus();
                    txtPhysicalPath.SelectAll();
                    return false;
                }
            }

            return true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            dlg.RootFolder = Environment.SpecialFolder.MyComputer;
            dlg.Description = "Select the physical location for the " + txtVirtualPath.Text + " website.";
            dlg.ShowNewFolderButton = false;

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            txtPhysicalPath.Text = dlg.SelectedPath;
            btnPageBrowse.Enabled = txtPhysicalPath.Text != "";
        }

        private void btnPageBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.CheckFileExists = true;
            dlg.InitialDirectory = txtPhysicalPath.Text;
            dlg.Title = "Select the Default Page for the Site";

            if (txtDefaultPage.Text != "")
            {
                dlg.FileName = txtPhysicalPath.Text + "\\" + txtDefaultPage.Text;
            }

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            if (Path.GetDirectoryName(dlg.FileName).ToLower() != txtPhysicalPath.Text.ToLower())
            {
                MessageBox.Show("You must choose a file that is in the directory specified above.", Program.ApplicationTitle, MessageBoxButtons.OK);
            }
            else
            {
                txtDefaultPage.Text = Path.GetFileName(dlg.FileName);
            }
        }

        private void txtPhysicalPath_TextChanged(object sender, EventArgs e)
        {
            btnPageBrowse.Enabled = txtPhysicalPath.Text != "";
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            ((MainForm) this.Owner).ShowHelpWindow(this, null);
        }
    }
}