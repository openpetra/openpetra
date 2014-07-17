//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       tim-ingham
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
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// This is just an ordinary checkbox, but unlike the standard .NET one,
    /// the user can see when it has focus.
    /// </summary>
    public class TchkVisibleFocus : System.Windows.Forms.CheckBox
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TchkVisibleFocus() : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.FlatStyle = FlatStyle.Standard;
            
            this.GotFocus += TchkClearFocus_GotFocus;
            this.LostFocus += TchkClearFocus_LostFocus;
        }

        private void TchkClearFocus_GotFocus(object sender, System.EventArgs e)
        {
            this.FlatStyle = FlatStyle.Flat;
        }

        private void TchkClearFocus_LostFocus(object sender, System.EventArgs e)
        {            
            this.FlatStyle = FlatStyle.Standard;
        }
    }
}