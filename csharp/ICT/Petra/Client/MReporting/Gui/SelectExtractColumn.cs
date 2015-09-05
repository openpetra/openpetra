﻿//
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