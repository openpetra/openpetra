//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       >>>> Put your full name or just a shortname here <<<<
//
// Copyright 2004-2011 by OM International
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
using System.Windows.Forms;

using Ict.Common;

namespace Ict.Petra.Client.CommonForms.Logic
{
    /// <summary>
    /// Shepherd Navigation Helper
    /// </summary>
    /// <remarks>TODO what was this Class about? Is it still needed?</remarks>
    public class TShepherdNavigationHelper
    {
        /// <summary>Total visible Panels</summary>
        public int TotalPages {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="AShepherdPages"></param>
        /// <param name="APanelCollapsible"></param>
        public TShepherdNavigationHelper(TPetraShepherdPagesList AShepherdPages, Panel APanelCollapsible)
        {
            TLogging.Log("Entering TShepherdNavigationHelper Constructor...");

            TLogging.Log("TShepherdNavigationHelper Constructor ran.");
        }

        /// <summary>Redraws the Navigation Panel only</summary>
        public void Refresh()
        {
            TLogging.Log("Refresh");
        }
    }
}