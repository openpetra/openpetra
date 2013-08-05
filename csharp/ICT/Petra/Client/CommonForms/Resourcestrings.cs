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
        public static readonly string StrFilterIsTurnedOff = Catalog.GetString("Filter is off.\r\n-> Click button to show the Filter Panel.");

        /// <summary>todoComment</summary>
        public static readonly string StrFilterIsTurnedOn = Catalog.GetString(
            "Filter is on.\r\nIf Filter Crieria are entered then the list will\r\nonly display records that match them.\r\n-> Click button to turn the Filter off.");

        #endregion
    }
}