//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
//
// Copyright 2004-2010 by OM International
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ict.Petra.Client.App.Core;
using GNU.Gettext;
using Ict.Common;

namespace Ict.Petra.Client.CommonDialogs
{
    /// <summary>
    /// UserControl that features a big Petra Logo and Version Infomation.
    /// </summary>
    public partial class TUCPetraLogoAndVersionInfo : UserControl
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TUCPetraLogoAndVersionInfo()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblPetra.Text = Catalog.GetString(" O P E N P E T R A ");
            this.lblPetraVersion.Text = Catalog.GetString("Version");
            this.lblCopyrightNotice.Text = Catalog.GetString("© 1995 - 2009 by OM International");
            this.lblInstallationKind.Text = Catalog.GetString("Standalone / Network / Remote");
            #endregion
        }

        /// <summary>todoComment</summary>
        public string PetraVersion
        {
            get
            {
                return lblPetraVersion.Text;
            }

            set
            {
                lblPetraVersion.Text = value;
            }
        }

        /// <summary>todoComment</summary>
        public string InstallationKind
        {
            get
            {
                return lblInstallationKind.Text;
            }

            set
            {
                lblInstallationKind.Text = value;
            }
        }

        /// <summary>
        /// Set the  status of the cursor for the whole user control
        /// </summary>
        /// <param name="newCursor">the cursor to show</param>
        public void SetCursor(Cursor newCursor)
        {
            this.Cursor = newCursor;
            lblInstallationKind.Cursor = newCursor;
            lblPetra.Cursor = newCursor;
            lblPetraVersion.Cursor = newCursor;
            lblCopyrightNotice.Cursor = newCursor;

            pnlTextBoxContainer.Cursor = newCursor;
            pbxPetraLogo.Cursor = newCursor;
        }
    }
}