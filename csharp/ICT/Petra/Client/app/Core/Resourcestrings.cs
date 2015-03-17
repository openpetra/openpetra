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

using Ict.Common;
using Ict.Common.DB;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Contains resourcetexts that are used in the App.Core Namespace (and beyond).
    /// </summary>
    public static class AppCoreResourcestrings
    {
        #region Resourcestrings

        /// <summary>todoComment</summary>
        public static readonly string StrFunctionalityNotAvailableYet = Catalog.GetString(
            "This functionality is not yet implemented in OpenPetra.");

        /// <summary>todoComment</summary>
        public static readonly string StrFunctionalityNotAvailableYetTitle = Catalog.GetString(
            "Not Yet Implemented in OpenPetra");

        /// <summary>todoComment</summary>
        public static readonly string StrSystemAdministratorLogFileHint = Catalog.GetString(
            "Your system administator can find details about this in the log file.");

        /// <summary>todoComment</summary>
        public static readonly string StrPetraServerTooBusyGeneric = Catalog.GetString(
            "The OpenPetra Server is currently too busy to {0}");

        /// <summary>todoComment</summary>
        public static readonly string StrPetraServerTooBusyTitle = Catalog.GetString("OpenPetra Server Too Busy");

        /// <summary>todoComment</summary>
        public static readonly string StrPleaseWaitAFewSecondsWithRetryCancel = Catalog.GetString(
            "Please wait a few seconds, then press 'Retry' to retry, or press 'Cancel' to abort.");

        /// <summary>todoComment</summary>
        public static readonly string StrServerTooBusyScreenMightBeBroken = Catalog.GetString(
            "If you have received this message while a screen was opened and that screen is left open then please don't try to use that screen! Rather, close it and try to open it again.");

        /// <summary>todoComment</summary>
        public static readonly string StrPetraServerTooBusyWaitAFewSecondsWithRetryCancel =
            StrPetraServerTooBusyGeneric + ".\r\n\r\n" +
            StrPleaseWaitAFewSecondsWithRetryCancel;

        /// <summary>todoComment</summary>
        public static readonly string StrPetraServerTooBusyWaitAFewSecondsNoAutomaticRetryCancel =
            StrPetraServerTooBusyGeneric + ".\r\n\r\n" +
            TServerBusyHelper.StrPleaseWaitAFewSecondsManualRetryRequiredGeneric + "\r\n\r\n" +
            StrServerTooBusyScreenMightBeBroken;

        /// <summary>todoComment</summary>
        public static readonly string StrOpeningCancelledByUser = Catalog.GetString("Opening of {0} screen got cancelled by user.");

        /// <summary>todoComment</summary>
        public static readonly string StrOpeningCancelledByUserTitle = Catalog.GetString("Screen Opening Cancelled");

        /// <summary>todoComment</summary>
        public static readonly string StrOpenPetraClientNeedsToBeRestarted =
            Catalog.GetString("We are very sorry, but THE OPENPETRA CLIENT NEEDS TO BE RESTARTED!{0}\r\n\r\n       " +
                "--->   Press 'OK' to restart the OpenPetra Client now!   <---");

        /// <summary>todoComment</summary>
        public static readonly string StrOpenPetraClientNeedsToBeRestartedTitle =
            Catalog.GetString("Sorry: OpenPetra Client RESTART REQUIRED!");

//        public static readonly string StrConnectionBroken = Catalog.GetString(
//            "The connection to the OpenPetra Server has broken.\r\n\r\n==> Unfortunately you will need to close OpenPetra and log in again. <==");

//        public static readonly string StrConnectionBrokenTitle = Catalog.GetString("SERVER CONNECTION BROKEN!");

//        public static readonly string StrConnectionClosed = Catalog.GetString(
//            "The connection to the OpenPetra Server has been closed by the OpenPetra Server.\r\n\r\n==> Unfortunately you will need to close OpenPetra and log in again. <==");

//        public static readonly string StrConnectionClosedTitle = Catalog.GetString("SERVER CONNECTION CLOSED BY OPENPETRA SERVER!");

//        public static readonly string StrConnectionUnavailableCause = Catalog.GetString("\r\n\r\nDEBUG INFORMATION: Actual cause for the problem: \r\n");

        #endregion
    }
}