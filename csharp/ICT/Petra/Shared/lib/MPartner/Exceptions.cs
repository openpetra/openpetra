// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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
using System.Runtime.Serialization;
using Ict.Common.Exceptions;
using Ict.Petra.Shared.Security;

namespace Ict.Petra.Shared.MPartner
{
    /// <summary>
    /// Thrown if a Partner cannot be opened for security reasons
    /// </summary>
    [Serializable()]
    public class ESecurityPartnerAccessDeniedException : ESecurityAccessDeniedException
    {
        /// <summary>PartnerKey of Partner to which access is denied.</summary>
        private Int64 FPartnerKey;

        /// <summary>ShortName of Partner to which access is denied.</summary>
        private String FPartnerShortName;

        private byte FAccessLevel;

        /// <summary>PartnerKey of Partner to which access is denied.</summary>
        public Int64 PartnerKey
        {
            get
            {
                return FPartnerKey;
            }

            set
            {
                FPartnerKey = value;
            }
        }

        /// <summary>ShortName of Partner to which access is denied.</summary>
        public String PartnerShortName
        {
            get
            {
                return FPartnerShortName;
            }

            set
            {
                FPartnerShortName = value;
            }
        }

        /// <summary>
        /// level of access that is denied
        /// </summary>
        public byte AccessLevel
        {
            get
            {
                return FAccessLevel;
            }

            set
            {
                FAccessLevel = value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ESecurityPartnerAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public ESecurityPartnerAccessDeniedException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerShortName"></param>
        /// <param name="AAccessLevel"></param>
        public ESecurityPartnerAccessDeniedException(String msg, Int64 APartnerKey,
            string APartnerShortName,
            TPartnerAccessLevelEnum AAccessLevel) : base(msg)
        {
            FPartnerKey = APartnerKey;
            FPartnerShortName = APartnerShortName;
            FAccessLevel = (byte)AAccessLevel; // (byte)Enum.Parse(typeof(TPartnerAccessLevelEnum), Enum.GetName(typeof(TPartnerAccessLevelEnum), AAccessLevel));
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ESecurityPartnerAccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            FPartnerKey = info.GetInt64("PartnerKey");
            FPartnerShortName = info.GetString("PartnerShortName");
            FAccessLevel = info.GetByte("AccessLevel");
        }

        /// <summary>
        /// needed for remoting, serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("PartnerKey", FPartnerKey);
            info.AddValue("PartnerShortName", FPartnerShortName);
            info.AddValue("AccessLevel", FAccessLevel);
        }
    }
}