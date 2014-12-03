//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System;
using Ict.Common;
using Ict.Common.Session;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;

namespace Ict.Petra.Shared
{
    /// <summary>
    ///  Holds User Information (particularly security-related) in a global variable
    /// and allows refreshing of this information.
    /// </summary>
    public class UserInfo
    {
        /// <summary>used internally to hold User Information - but only when this class is used client-side!</summary>
        private static TPetraPrincipal MUserInfo = null;

        /// <summary>
        /// True if the <see cref="UserInfo" /> Class is used on the client side, false otherwise.
        /// </summary>
        private static bool FRunningOnClientSide = false;

        /// <summary>
        /// True if the <see cref="UserInfo" /> Class is used on the client side, false otherwise.
        /// </summary>
        static public bool RunningOnClientSide
        {
            get
            {
                return FRunningOnClientSide;
            }

            set
            {
                FRunningOnClientSide = value;
            }
        }

        /// <summary>used internally to hold User Information</summary>
        public static TPetraPrincipal GUserInfo
        {
            set
            {
                if (FRunningOnClientSide)
                {
//                    TLogging.Log("GUserInfo gets written to from server-side");
                    MUserInfo = value;
                }
                else
                {
                    TSession.SetVariable("UserInfo", value);
                }
            }
            get
            {
                if (FRunningOnClientSide)
                {
                    return MUserInfo;
                }
                else
                {
//                    TLogging.Log("GUserInfo requested from server-side");
                    return (TPetraPrincipal)TSession.GetVariable("UserInfo");
                }
            }
        }
    }
}