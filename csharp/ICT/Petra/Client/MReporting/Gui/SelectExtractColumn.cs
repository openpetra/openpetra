//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2004-2015 by OM International
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
using Ict.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    ///
    /// </summary>
    public partial class TFrmSelectExtractColumn : Form
    {
        /// <summary>
        ///
        /// </summary>
        public TFrmSelectExtractColumn()
        {
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnOK.Text = Catalog.GetString("OK");
            this.button1.Text = Catalog.GetString("Cancel");
            this.label1.Text = Catalog.GetString("Select the column containing a partner key, which will be used for the extract.");
            this.Text = Catalog.GetString("Select Extract Column");
            #endregion
        }

        /// <summary>
        ///
        /// </summary>
        public void AddOption(String AOpt)
        {
            cmbOptions.Items.Add(AOpt);
        }

        /// <summary>
        ///
        /// </summary>
        public String SelectedOption
        {
            get
            {
                return (String)(cmbOptions.SelectedItem);
            }
            set
            {
                cmbOptions.SelectedItem = value;
            }
        }
    }
}
