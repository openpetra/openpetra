//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Data.Odbc;
using System.Threading;
using System.Security.Principal;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;

namespace Tests.IctCommonRemoting.Server
{
    /// <summary>
    /// The TUserManager class provides access to the security-related information
    /// of Users of a Petra DB.
    /// </summary>
    public class TUserManager : IUserManager
    {
        /// <summary>
        /// add a new user
        /// </summary>
        public bool AddUser(string AUserID, string APassword = "")
        {
            return false;
        }

        /// <summary>
        /// make sure the user can login with the correct password
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="APassword"></param>
        /// <param name="AProcessID"></param>
        /// <param name="ASystemEnabled"></param>
        /// <returns></returns>
        public IPrincipal PerformUserAuthentication(String AUserID,
            String APassword,
            out Int32 AProcessID,
            out Boolean ASystemEnabled)
        {
            AProcessID = -1;
            ASystemEnabled = true;
            return null;
        }
    }
}