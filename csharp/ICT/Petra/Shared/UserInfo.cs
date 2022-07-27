//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2022 by OM International
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
using Ict.Common.Session;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;

using Newtonsoft.Json;


namespace Ict.Petra.Shared
{
    /// <summary>
    ///  Holds User Information (particularly security-related) in a global variable
    /// and allows refreshing of this information.
    /// </summary>
    public class UserInfo
    {
        /// <summary>get user information from the session</summary>
        public static TPetraPrincipal GetUserInfo()
        {
            try
            {
                object value = TSession.GetVariable("UserInfo");

                if (value == null)
                {
                    TLogging.Log("UserInfo is null");
                    return null;
                }

                return JsonConvert.DeserializeObject<TPetraPrincipal>(TSession.GetVariant("UserInfo").ToJson());
            }
            catch (Exception e)
            {
                TLogging.Log("Get user info " + e.ToString());
            }

            return null;
        }

        /// <summary>set user information in the session</summary>
        public static void SetUserInfo(TPetraPrincipal value)
        {
            TSession.SetVariable("UserInfo", value);
            TSession.SetVariable("UserID", value.UserID);
        }
    }
}
