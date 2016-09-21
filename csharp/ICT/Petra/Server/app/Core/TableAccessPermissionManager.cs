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
using System.Data;

using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;

namespace Ict.Petra.Server.App.Core.Security
{
    /// <summary>
    /// The TTableAccessPermissionManager class provides functions to work with the
    /// Table Access Permissions of a User of a Petra DB.
    /// </summary>
    public class TTableAccessPermissionManager
    {
        /// <summary>
        /// get the list of access permissions to database tables for the user
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        /// <returns></returns>
        public static SUserTableAccessPermissionTable LoadTableAccessPermissions(String AUserID, TDBTransaction ATransaction)
        {
            SUserTableAccessPermissionTable ReturnValue;

            if (SUserTableAccessPermissionAccess.CountViaSUser(AUserID, ATransaction) > 0)
            {
                ReturnValue = SUserTableAccessPermissionAccess.LoadViaSUser(AUserID, ATransaction);
            }
            else
            {
                ReturnValue = new SUserTableAccessPermissionTable();
            }

            return ReturnValue;
        }
    }
}