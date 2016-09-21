//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Data;
using System.Security.Principal;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared.MSysMan.Data;

namespace Ict.Petra.Server.App.Core
{
    /// <summary>
    /// Reads and saves the Logon Message.
    /// </summary>
    public class TMaintenanceLogonMessage : IMaintenanceLogonMessage
    {
        /// <summary>
        /// Returns the Logon Message for a certain LanguageCode.
        /// </summary>
        /// <param name="ALanguageCode">LanguageCode for which the LogonMessage should be returned</param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        /// <returns>The LogonMessage, or an empty String if no LogonMessage was found for
        /// the specified LanguageCode
        /// </returns>
        public String GetLogonMessage(String ALanguageCode, TDBTransaction ATransaction)
        {
            return GetLogonMessage(ALanguageCode, false, ATransaction);
        }

        /// <summary>
        /// overload. language code is part of TPetraPrincipal
        /// </summary>
        /// <param name="AUserInfo"></param>
        /// <param name="AReturnEnglishIfNotFound"></param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        /// <returns></returns>
        public String GetLogonMessage(IPrincipal AUserInfo, Boolean AReturnEnglishIfNotFound, TDBTransaction ATransaction)
        {
            return GetLogonMessage(((TPetraPrincipal)AUserInfo).PetraIdentity.LanguageCode, false, ATransaction);
        }

        /// <summary>
        /// Returns the Logon Message for a certain LanguageCode.
        /// </summary>
        /// <param name="ALanguageCode">LanguageCode for which the LogonMessage should be returned</param>
        /// <param name="AReturnEnglishIfNotFound">Returns the LogonMessage in English if no
        /// LogonMessage was found for the specified LanguageCode</param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        /// <returns>The LogonMessage
        /// </returns>
        public String GetLogonMessage(String ALanguageCode, Boolean AReturnEnglishIfNotFound, TDBTransaction ATransaction)
        {
            String ReturnValue;
            SLogonMessageTable LogonMessageTable;

            try
            {
                if (SLogonMessageAccess.Exists(ALanguageCode, ATransaction))
                {
                    LogonMessageTable = SLogonMessageAccess.LoadByPrimaryKey(ALanguageCode, ATransaction);
                    ReturnValue = LogonMessageTable[0].LogonMessage;
                }
                else
                {
                    if (AReturnEnglishIfNotFound)
                    {
                        LogonMessageTable = SLogonMessageAccess.LoadByPrimaryKey("EN", ATransaction);
                        ReturnValue = LogonMessageTable[0].LogonMessage;
                    }
                    else
                    {
                        ReturnValue = "";
                    }
                }
            }
            catch (Exception exp)
            {
                TLogging.Log("TMaintenanceLogonMessage.GetLogonMessage: Error loading Logon Message! " + "Possible cause: " + exp.ToString());
                return "";
            }

            return ReturnValue;
        }
    }
}