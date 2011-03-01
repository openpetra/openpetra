//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MSysMan.Data;

using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MSysMan.Security
{
    /// <summary>
    /// The TUserManager class provides access to the security-related information
    /// of Users of a Petra DB.
    /// </summary>
    /// <remarks>
    /// Calls methods that have the same name in the
    /// Ict.Petra.Server.App.Core.Security.UserManager Namespace to perform its
    /// functionality!
    ///
    /// This is required in two places,
    /// because it is needed before the appdomain is loaded and therefore cannot be in MSysMan;
    /// and it is needed here to make it available to the client via MSysMan remotely
    /// </remarks>
    public class TUserManager
    {
        /// <summary>
        /// use Server.App.Core.Security.TUserManager.LoadUser
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="APetraPrincipal"></param>
        /// <returns></returns>
        public static SUserRow LoadUser(String AUserID, ref TPetraPrincipal APetraPrincipal)
        {
            return Ict.Petra.Server.App.Core.Security.TUserManager.LoadUser(AUserID, out APetraPrincipal);
        }

        /// <summary>
        /// use Server.App.Core.Security.TUserManager.LoadUser
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="APetraIdentity"></param>
        /// <returns></returns>
        public static SUserRow LoadUser(String AUserID, ref Ict.Petra.Shared.Security.TPetraIdentity APetraIdentity)
        {
            return Ict.Petra.Server.App.Core.Security.TUserManager.LoadUser(AUserID, out APetraIdentity);
        }

        /// <summary>
        /// use Server.App.Core.Security.TUserManager.PerformUserAuthentication
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="APassword"></param>
        /// <param name="AProcessID"></param>
        /// <param name="ASystemEnabled"></param>
        /// <returns></returns>
        public static TPetraPrincipal PerformUserAuthentication(String AUserID, String APassword, ref Int32 AProcessID, ref Boolean ASystemEnabled)
        {
            return Ict.Petra.Server.App.Core.Security.TUserManager.PerformUserAuthentication(AUserID, APassword, out AProcessID, out ASystemEnabled);
        }

        /// <summary>
        /// Causes an immediately reload of the UserInfo that is stored in a global
        /// variable.
        ///
        /// </summary>
        /// <returns>void</returns>
        public static TPetraPrincipal ReloadCachedUserInfo()
        {
            return Ict.Petra.Server.App.Core.Security.TUserManager.ReloadCachedUserInfo();
        }

        /// <summary>
        /// use Server.App.Core.Security.TUserManager.SaveUser
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="AUserDataTable"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static Boolean SaveUser(String AUserID, SUserTable AUserDataTable, ref TVerificationResultCollection AVerificationResult)
        {
            return Ict.Petra.Server.App.Core.Security.TUserManager.SaveUser(AUserID, AUserDataTable, out AVerificationResult);
        }

        /// <summary>
        /// Queues a ClientTask for reloading of the UserInfo for all connected Clients
        /// with a certain UserID.
        ///
        /// </summary>
        /// <param name="AUserID">UserID for which the ClientTask should be queued
        /// </param>
        /// <returns>void</returns>
        public static void SignalReloadCachedUserInfo(String AUserID)
        {
            // $IFDEF DEBUGMODE if TSrvSetting.DL >= 7 then Console.WriteLine(this.GetType.FullName + '.SignalReloadCachedUserInfo: calling DomainManager.ClientTaskAddToOtherClient...'); $ENDIF
            Ict.Petra.Server.App.ClientDomain.DomainManager.ClientTaskAddToOtherClient(AUserID,
                SharedConstants.CLIENTTASKGROUP_USERINFOREFRESH,
                "",
                1);
        }
    }
}