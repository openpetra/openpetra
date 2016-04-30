//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Common;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// Contains resourcetexts that are used in Forms and UserControls right across the OpenPetra Modules.
    /// </summary>
    public class CommonFormsResourcestrings
    {
        #region Find/Filter resourcestrings

        /// <summary>todoComment</summary>
        public static readonly string StrFilterIsHidden = Catalog.GetString("Filter is hidden.");

        /// <summary>todoComment</summary>
        public static readonly string StrFilterClickToTurnOn = Catalog.GetString("-> Click button to show the Filter Panel.");

        /// <summary>todoComment</summary>
        public static readonly string StrFilterClickToTurnOff = Catalog.GetString("-> Click button to hide the Filter Panel.");

        /// <summary>todoComment</summary>
        public static readonly string StrFilterAllRecordsShown = Catalog.GetString("The current filter is showing all the relevant records.");

        /// <summary>todoComment</summary>
        public static readonly string StrFilterSomeRecordsHidden = Catalog.GetString("The current filter may be hiding some relevant records.");

        #endregion

        #region Petra Forms Utilities Strings

        /// <summary>Our 'Company Name' in the folder structure</summary>
        public static readonly string StrFolderOrganisationName = "OpenPetraOrg";

        /// <summary>Our screen positions file name.  The replaceable parameter is the userID</summary>
        public static readonly string StrScreenPositionsFileName = "{0}.ScreenPositions.cfg";

        /// <summary>Dialog message title</summary>
        public static readonly string StrReuseScreenPositionsTitle = Catalog.GetString("Saving Window Positions");

        /// <summary>First part of the dialog message</summary>
        public static readonly string StrReuseScreenPositionsMessage1 = Catalog.GetString(
            "You have chosen to start storing window positions and sizes again.  OpenPetra has saved this information in the past.");

        /// <summary>Second part of the dialog message</summary>
        public static readonly string StrReuseScreenPositionsMessage2 = Catalog.GetString(
            "Do you want to keep this 'old' information and re-use the sizes and positions that you saved previously or discard this information and start over?");


        #endregion
    }
}