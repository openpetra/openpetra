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

using Ict.Common.DB;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MSysMan.Security
{
    /// <summary>
    /// The TTableAccessPermissionManager class provides functions to work with the
    /// Table Access Permissions of a User of a Petra DB.
    /// </summary>
    /// <remarks>
    /// Calls methods that have the same name in the
    /// Ict.Petra.Server.App.Core.Security.TableAccessPermissionManager Namespace
    /// to perform its functionality!
    /// </remarks>
    public class TTableAccessPermissionManager
    {
        /// <summary>
        /// call Server.App.Core.Security.TTableAccessPermissionManager.LoadTableAccessPermissions
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        /// <returns></returns>
        public static SUserTableAccessPermissionTable LoadTableAccessPermissions(String AUserID, TDBTransaction ATransaction)
        {
            return Ict.Petra.Server.App.Core.Security.TTableAccessPermissionManager.LoadTableAccessPermissions(AUserID,
                ATransaction);
        }
    }
}