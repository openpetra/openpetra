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
using System.Security.Principal;

namespace Ict.Petra.Shared.Security
{
    /// The TPetraIdentity class is a .NET IIdentity-derived representation of a
    /// User in the Petra DB.
    [Serializable()]
    public class TPetraIdentity : System.Security.Principal.IIdentity
    {
        private String FUserID;
        private String FLastName;
        private String FFirstName;
        private String FLanguageCode;
        private String FAcquisitionCode;
        private DateTime FCurrentLogin;
        private DateTime FLastLogin;
        private DateTime FFailedLogin;
        private Int32 FFailedLogins;
        private Int64 FPartnerKey;
        private Int64 FDefaultLedgerNumber;
        private Boolean FAccountLocked;
        private Boolean FRetired;
        private Boolean FModifiableUser;

        /// <summary>
        /// the user id of the petra user
        /// </summary>
        public String UserID
        {
            get
            {
                return FUserID;
            }
        }

        /// <summary>
        /// is the user account available for log-in or locked
        /// </summary>
        public Boolean AccountLocked
        {
            get
            {
                return FAccountLocked;
            }
        }
        /// <summary>
        /// is the user still active or retired
        /// </summary>
        public Boolean Retired
        {
            get
            {
                return FRetired;
            }
        }

        /// <summary>
        /// partner key of the user
        /// </summary>
        public Int64 PartnerKey
        {
            get
            {
                return FPartnerKey;
            }
        }

        /// <summary>
        /// when was the login time of the user
        /// </summary>
        public DateTime CurrentLogin
        {
            get
            {
                return FCurrentLogin;
            }

            set
            {
                FCurrentLogin = value;
            }
        }

        /// <summary>
        /// when was the last login time before this session for this user
        /// </summary>
        public DateTime LastLogin
        {
            get
            {
                return FLastLogin;
            }

            set
            {
                FLastLogin = value;
            }
        }

        /// <summary>
        /// when was the last failed login for this user
        /// </summary>
        public DateTime FailedLogin
        {
            get
            {
                return FFailedLogin;
            }
        }

        /// <summary>
        /// number of failed logins for this user
        /// </summary>
        public Int32 FailedLogins
        {
            get
            {
                return FFailedLogins;
            }
        }

        /// <summary>
        /// prefered language of the user
        /// </summary>
        public String LanguageCode
        {
            get
            {
                return FLanguageCode;
            }
        }

        /// <summary>
        /// which acquisition code should be used by default when this user creates new partners
        /// </summary>
        public String AcquisitionCode
        {
            get
            {
                return FAcquisitionCode;
            }
        }

        /// <summary>
        /// the default ledger that should be selected for this user
        /// </summary>
        public Int64 DefaultLedgerNumber
        {
            get
            {
                return FDefaultLedgerNumber;
            }
        }

        /// <summary>
        /// can this user be changed
        /// </summary>
        public Boolean ModifiableUser
        {
            get
            {
                return FModifiableUser;
            }
        }

        /// Inherited from IIdentity
        public String AuthenticationType
        {
            get
            {
                return "Custom";
            }
        }

        /// Inherited from IIdentity
        public Boolean IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        /// Inherited from IIdentity
        public String Name
        {
            get
            {
                return FFirstName + ' ' + FLastName;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ALastName"></param>
        /// <param name="AFirstName"></param>
        /// <param name="ALanguageCode"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="ACurrentLogin"></param>
        /// <param name="ALastLogin"></param>
        /// <param name="AFailedLogin"></param>
        /// <param name="AFailedLogins"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ADefaultLedgerNumber"></param>
        /// <param name="AAccountLocked"></param>
        /// <param name="ARetired"></param>
        /// <param name="AModifiableUser"></param>
        public TPetraIdentity(String AUserID,
            String ALastName,
            String AFirstName,
            String ALanguageCode,
            String AAcquisitionCode,
            DateTime ACurrentLogin,
            DateTime ALastLogin,
            DateTime AFailedLogin,
            Int32 AFailedLogins,
            Int64 APartnerKey,
            Int64 ADefaultLedgerNumber,
            Boolean AAccountLocked,
            Boolean ARetired,
            Boolean AModifiableUser) : base()
        {
            FUserID = AUserID;
            FLastName = ALastName;
            FFirstName = AFirstName;
            FLanguageCode = ALanguageCode;
            FAcquisitionCode = AAcquisitionCode;
            FCurrentLogin = ACurrentLogin;
            FLastLogin = ALastLogin;
            FFailedLogin = AFailedLogin;
            FFailedLogins = AFailedLogins;
            FPartnerKey = APartnerKey;
            FDefaultLedgerNumber = ADefaultLedgerNumber;
            FAccountLocked = AAccountLocked;
            FRetired = ARetired;
            FModifiableUser = AModifiableUser;
        }
    }
}