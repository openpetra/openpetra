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
using System.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;

namespace Ict.Petra.Server.App.Core.Security
{
    /// <summary>
    /// Reads and saves entries in the Error Log table.
    /// </summary>
    public class TErrorLog : IErrorLog
    {
        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AErrorCode"></param>
        /// <param name="AContext"></param>
        /// <param name="AMessageLine1"></param>
        /// <param name="AMessageLine2"></param>
        /// <param name="AMessageLine3"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public Boolean AddErrorLogEntry(String AErrorCode,
            String AContext,
            String AMessageLine1,
            String AMessageLine2,
            String AMessageLine3,
            out TVerificationResultCollection AVerificationResult)
        {
            return AddErrorLogEntry(AErrorCode,
                AContext,
                AMessageLine1,
                AMessageLine2,
                AMessageLine3,
                UserInfo.GUserInfo.UserID,
                UserInfo.GUserInfo.ProcessID,
                out AVerificationResult);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public const String UNKNOWN_CONTEXT = "File Name/Context is unknown.";

        /// <summary>
        /// todoComment
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
        public Boolean AddErrorLogEntry(String AErrorCode,
            String AContext,
            String AMessageLine1,
            String AMessageLine2,
            String AMessageLine3,
            String AUserID,
            Int32 AProcessID,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction WriteTransaction;
            Boolean SubmissionOK;
            SErrorLogTable ErrorLogTable;
            SErrorLogRow NewErrorLogRow;
            DateTime ErrorLogDateTime;
            Boolean NewTransaction;
            String Context;

            ErrorLogTable = new SErrorLogTable();
            NewErrorLogRow = ErrorLogTable.NewRowTyped(false);
            ErrorLogDateTime = DateTime.Now;

            if (AContext != "")
            {
                Context = AContext;
            }
            else
            {
                Context = UNKNOWN_CONTEXT;
            }

            // Set DataRow values
            NewErrorLogRow.ErrorCode = AErrorCode;
            NewErrorLogRow.UserId = AUserID.ToUpper();
            NewErrorLogRow.Date = ErrorLogDateTime;
            NewErrorLogRow.Time = Conversions.DateTimeToInt32Time(ErrorLogDateTime);
            NewErrorLogRow.ReleaseNumber = TSrvSetting.ApplicationVersion.ToString();
            NewErrorLogRow.FileName = Context;
            NewErrorLogRow.ProcessId = AProcessID.ToString();
            NewErrorLogRow.MessageLine1 = AMessageLine1;
            NewErrorLogRow.MessageLine2 = AMessageLine2;
            NewErrorLogRow.MessageLine3 = AMessageLine3;
            ErrorLogTable.Rows.Add(NewErrorLogRow);

            // Save DataRow
            WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            SubmissionOK = SErrorLogAccess.SubmitChanges(ErrorLogTable, WriteTransaction, out AVerificationResult);

            if (SubmissionOK)
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            else
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return SubmissionOK;
        }
    }
}