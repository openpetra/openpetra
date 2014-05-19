//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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
using System.Data;
using System.Windows.Forms;
using Ict.Common.Controls;

namespace Ict.Petra.Client.CommonDialogs
{
    /// manual methods for the generated window
    public partial class TUC_Shortcuts
    {
        private bool FDoneInitialise = false;

        /// <summary>
        /// Returns a reference to the Help Grid, or null if the property has already been 'got' once (indicating that the grid has already been initialised)
        /// </summary>
        public TSgrdDataGrid HelpGrid
        {
            get
            {
                if (FDoneInitialise)
                {
                    return null;
                }
                else
                {
                    FDoneInitialise = true;
                    return grdHelp;
                }
            }
        }

        /// <summary>
        /// Returns a reference to the Description Label
        /// </summary>
        public Label DescriptionLabel
        {
            get
            {
                return lblDescription;
            }
        }

        /// <summary>
        /// implement dummy function so that we can use this control on a yaml form
        /// </summary>
        public void GetDataFromControls()
        {
            // not implemented
        }
    }
}