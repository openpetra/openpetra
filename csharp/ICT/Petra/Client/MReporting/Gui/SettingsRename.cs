//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using GNU.Gettext;
using Ict.Common;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// form that helps with renaming report settings
    /// </summary>
    public partial class TFrmSettingsRename : System.Windows.Forms.Form
    {
        /// <summary>
        /// the new name after renaming
        /// </summary>
        public string NewName
        {
            get
            {
                return txtNewName.Text;
            }

            set
            {
                txtNewName.Text = value;
            }
        }

        /// <summary>
        /// the old name before renaming
        /// </summary>
        public string OldName
        {
            get
            {
                return txtOldName.Text;
            }

            set
            {
                txtOldName.Text = value;
            }
        }

        /// <summary>
        /// Private Declarations
        /// </summary>
        /// <returns>void</returns>
        public TFrmSettingsRename() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblNewName.Text = Catalog.GetString("New Name") + ":";
            this.lblOldName.Text = Catalog.GetString("Old Name") + ":";
            this.txtNewName.Text = Catalog.GetString("New Name");
            this.txtOldName.Text = Catalog.GetString("OldName");
            this.BtnOk.Text = Catalog.GetString("&Ok");
            this.BtnCancel.Text = Catalog.GetString("&Cancel");
            this.Text = Catalog.GetString("Rename the Settings");
            #endregion
        }
    }
}