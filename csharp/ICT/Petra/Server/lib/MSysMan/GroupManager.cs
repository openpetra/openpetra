//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2019 by OM International
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
using System.Collections;

using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;

namespace Ict.Petra.Server.MSysMan.Security
{
    /// <summary>
    /// The TGroupManager class provides functions to work with the Security Groups
    /// and Users' Security Groups of a Petra DB.
    /// </summary>
    public class TGroupManager
    {
        /// <summary>
        /// load the groups of the given user
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        /// <returns></returns>
        public static string[] LoadUserGroups(String AUserID, TDBTransaction ATransaction)
        {
            string[] ReturnValue;
            SUserGroupTable table;
            Int32 Counter;
            ArrayList groups;

            if (SUserGroupAccess.CountViaSUser(AUserID, ATransaction) > 0)
            {
                table = SUserGroupAccess.LoadViaSUser(AUserID, ATransaction);

                // Dimension the ArrayList with the maximum number of Groups first
                groups = new ArrayList(table.Rows.Count - 1);

                for (Counter = 0; Counter <= table.Rows.Count - 1; Counter += 1)
                {
                    groups.Add(table[Counter].GroupId);
                }

                if (table.Rows.Count != 0)
                {
                    // Copy contents of the ArrayList into the ReturnValue
                    ReturnValue = new string[table.Rows.Count];
                    Array.Copy(groups.ToArray(), ReturnValue, table.Rows.Count);
                }
                else
                {
                    ReturnValue = new string[0];
                }
            }
            else
            {
                ReturnValue = new string[0];
            }

            return ReturnValue;
        }
    }
}
