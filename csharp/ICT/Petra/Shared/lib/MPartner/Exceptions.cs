// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2024 by OM International
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
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ESecurityPartnerAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ESecurityPartnerAccessDeniedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ESecurityPartnerAccessDeniedException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        /// <summary>
        /// /// Initializes a new instance of this Exception Class with a specified error message and further data.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerShortName"></param>
        /// <param name="AAccessLevel"></param>
        public ESecurityPartnerAccessDeniedException(String AMessage, Int64 APartnerKey,
            string APartnerShortName,
            TPartnerAccessLevelEnum AAccessLevel) : base(AMessage)
        {
            FPartnerKey = APartnerKey;
            FPartnerShortName = APartnerShortName;
            FAccessLevel = (byte)AAccessLevel; // (byte)Enum.Parse(typeof(TPartnerAccessLevelEnum), Enum.GetName(typeof(TPartnerAccessLevelEnum), AAccessLevel));
        }
    }
}