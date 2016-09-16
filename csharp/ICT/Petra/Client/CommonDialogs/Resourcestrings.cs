//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2016 by OM International
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

namespace Ict.Petra.Client.CommonDialogs
{
    /// <summary>
    /// Contains resourcetexts that are used in Common Dialogs.
    /// </summary>
    public class CommonDialogsResourcestrings
    {
        #region General resourcestrings

        #endregion

        #region Password Management resourcestrings

        /// <summary>todoComment</summary>
        public static readonly string StrChangePasswordSuccess = Catalog.GetString("The password was successfully changed for user {0}.");

        /// <summary>todoComment</summary>
        public static readonly string StrChangePasswordError = Catalog.GetString(
            "The password could not be changed for user {0} for the following reason:");

        /// <summary>todoComment</summary>
        public static readonly string StrChangePasswordTitle =
            Catalog.GetString("Change Password");

        /// <summary>todoComment</summary>
        public static readonly string StrChangeUserPasswordTitle =
            Catalog.GetString("Change User Password");

        #endregion
    }
}