/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MSysMan.Security
{
    /// <summary>
    /// Reads and saves entries in the Error Log table.
    /// </summary>
    /// <remarks>
    /// Calls methods that have the same name in the
    /// Ict.Petra.Server.App.Core.Security.ErrorLog Namespace to perform its
    /// functionality!
    /// </remarks>
    public class TErrorLog
    {
        /// <summary>
        /// facade call
        /// </summary>
        /// <param name="AErrorCode"></param>
        /// <param name="AContext"></param>
        /// <param name="AMessageLine1"></param>
        /// <param name="AMessageLine2"></param>
        /// <param name="AMessageLine3"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static Boolean AddErrorLogEntry(String AErrorCode,
            String AContext,
            String AMessageLine1,
            String AMessageLine2,
            String AMessageLine3,
            ref TVerificationResultCollection AVerificationResult)
        {
            return TErrorLog.AddErrorLogEntry(AErrorCode, AContext, AMessageLine1, AMessageLine2, AMessageLine3, ref AVerificationResult);
        }

        /// <summary>
        /// facade call
        /// </summary>
        /// <param name="AErrorCode"></param>
        /// <param name="AContext"></param>
        /// <param name="AMessageLine1"></param>
        /// <param name="AMessageLine2"></param>
        /// <param name="AMessageLine3"></param>
        /// <param name="AUserID"></param>
        /// <param name="AProcessID"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static Boolean AddErrorLogEntry(String AErrorCode,
            String AContext,
            String AMessageLine1,
            String AMessageLine2,
            String AMessageLine3,
            String AUserID,
            Int32 AProcessID,
            ref TVerificationResultCollection AVerificationResult)
        {
            return TErrorLog.AddErrorLogEntry(AErrorCode,
                AContext,
                AMessageLine1,
                AMessageLine2,
                AMessageLine3,
                AUserID,
                AProcessID,
                ref AVerificationResult);
        }
    }
}