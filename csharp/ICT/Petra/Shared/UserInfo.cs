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
        /// <summary>used internally to hold User Information</summary>
        private static TPetraPrincipal MUserInfo = null;

        /// <summary>
        /// delegate for setting the object for this current session
        /// </summary>
        public delegate void ObjectSetter(TPetraPrincipal value);
        /// <summary>
        /// delegate for getting the object for this current session
        /// </summary>
        public delegate TPetraPrincipal ObjectGetter();

        private static ObjectSetter ObjDelegateSet = null;
        private static ObjectGetter ObjDelegateGet = null;

        /// we cannot have a reference to System.Web for Session here, so we use a delegate
        public static void SetFunctionForRetrievingCurrentObjectFromWebSession(
            ObjectSetter setter,
            ObjectGetter getter)
        {
            ObjDelegateSet = setter;
            ObjDelegateGet = getter;
        }

        /// <summary>used internally to hold User Information</summary>
        public static TPetraPrincipal GUserInfo
        {
            set
            {
                if (ObjDelegateSet == null)
                {
                    MUserInfo = value;
                }
                else
                {
                    ObjDelegateSet(value);
                }
            }
            get
            {
                if (ObjDelegateGet == null)
                {
                    return MUserInfo;
                }
                else
                {
                    return ObjDelegateGet();
                }
            }
        }
    }
}