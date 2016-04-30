//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2015 by OM International
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
using System.Threading;

using Ict.Common;
using Ict.Common.DB.Exceptions;

namespace Ict.Common.DB
{
    /// <summary>
    /// Helper Class for dealing with 'server busy' state (which can occur due to the prevention of multi-threading DB access problems).
    /// </summary>
    /// <remarks>Another Class, TServerBusyHelperGui in Namespace Ict.Petra.Client.App.Core, derives from this Class and extends it with
    /// GUI components related to the 'server-busy'-state.</remarks>
    public static class TServerBusyHelper
    {
        #region Resourcestrings

        /// <summary>todoComment</summary>
        public static readonly string StrPleaseWaitAFewSecondsManualRetryRequiredGeneric = Catalog.GetString(
            "Please wait a few seconds, then retry the action that you wanted to perform (if you had previously started a long-running action and this is not finished yet, you might need to wait until this is finished).");

        /// <summary>todoComment</summary>
        public static readonly string StrPleaseWaitAFewSecondsManualRetryRequiredCustom = Catalog.GetString(
            "Please wait a few seconds, then {0} (if you had previously started a long-running action and this is not finished yet, you might need to wait until this is finished).");

        #endregion

        /// <summary>
        /// <em>Automatic Retry Handling for Co-ordinated DB Access</em>: Calls the C# Delegate that encapsulates C# code that
        /// should be run inside the automatic retry handling scope that this Method provides. It does this a number of times
        /// in case the first call didn't work out, and once a set number of retries has been tried it returns (optionally,
        /// an Exception can be thrown if the number of retries was exceeded - this gets enabled by setting
        /// <paramref name="AWhenNotSuccessfulThrowSpecialException"/> to 'true').
        /// </summary>
        /// <param name="AContext">Context that the Method gets called in, eg. name of screen (used only for logging, so
        /// don't Catalog.GetString it).</param>
        /// <param name="AServerCallSuccessful">Controls whether a retry (when false) or no retry (when true) gets
        /// performed.</param>
        /// <param name="AEncapsulatedServerCallCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic server call retry handling scope that this Method provides.</param>
        /// <param name="AWhenNotSuccessfulThrowSpecialException">Whether
        /// <see cref="EDBAutoServerCallRetriesExceededException"/> should be thrown when the maximum number of retries
        /// has been exceeded, or not (Default=false). Should only be set to true in specific circumstances - the caller should
        /// normally just inspect its Variable that gets passed in into <paramref name="AServerCallSuccessful"/> to evaluate
        /// whether the call to this Method was within the retry attempts, or not.</param>
        /// <param name="AExceptionMessage">Optional message that the an <see cref="EDBAutoServerCallRetriesExceededException"/>
        /// Exception gets raised with. (Default = "").</param>
        public static void CoordinatedAutoRetryCall(string AContext, ref bool AServerCallSuccessful,
            Action AEncapsulatedServerCallCode, bool AWhenNotSuccessfulThrowSpecialException = false,
            string AExceptionMessage = null)
        {
            int MaxRetries = TAppSettingsManager.GetInt16("CoordinatedAutoRetryCallMaxRetries", 3);
            int ServerCallRetries = 0;

            do
            {
                try
                {
                    // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedServerCallCode
                    // Action delegate (0..n lines of code!)
                    AEncapsulatedServerCallCode();
                }
                catch (EDBTransactionBusyException)
                {
                    ServerCallRetries++;

                    if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_COORDINATED_DB_ACCESS)
                    {
                        TLogging.Log(TLogging.LOG_PREFIX_INFO + AContext +
                            String.Format(": Server is busy, retrying acquisition of DB Transaction... (Retry #{0} of {1})",
                                ServerCallRetries, MaxRetries));
                    }
                }
                catch (EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException)
                {
                    ServerCallRetries++;

                    if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_COORDINATED_DB_ACCESS)
                    {
                        TLogging.Log(TLogging.LOG_PREFIX_INFO + AContext +
                            String.Format(
                                ": Server is busy (DB Transaction in use by another Thread), retrying acquisition of DB Transaction... (Retry #{0} of {1})",
                                ServerCallRetries, MaxRetries));
                    }

                    Thread.Sleep(400);
                }
                catch (EDBCoordinatedDBAccessWaitingTimeExceededException)
                {
                    ServerCallRetries++;

                    if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_COORDINATED_DB_ACCESS)
                    {
                        TLogging.Log(TLogging.LOG_PREFIX_INFO + AContext +
                            String.Format(": Server is busy, retrying... (Retry #{0} of {1})",
                                ServerCallRetries, MaxRetries));
                    }
                }
            } while ((ServerCallRetries < MaxRetries) && (!(AServerCallSuccessful)));

            if (!AServerCallSuccessful)
            {
                if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_COORDINATED_DB_ACCESS)
                {
                    TLogging.Log(TLogging.LOG_PREFIX_INFO + AContext +
                        String.Format(": Server was too busy to service the request; the maximum number of retry attempts ({0}) were exhausted.",
                            MaxRetries));
                }

                if (AWhenNotSuccessfulThrowSpecialException)
                {
                    throw new EDBAutoServerCallRetriesExceededException(AExceptionMessage ?? String.Empty);
                }
            }
        }
    }
}