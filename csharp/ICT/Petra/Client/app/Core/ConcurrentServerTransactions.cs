//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.DB.Exceptions;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Helper Class for dealing with exceptions caused by concurrent server transactions
    /// </summary>
    public static class TConcurrentServerTransactions
    {
        #region Resource Strings

        private static readonly string StrConcurrentServerTransactionMessage1 =
            Catalog.GetString("Sorry: At the same time that you needed to read or write database records another user was changing them.") +
            Environment.NewLine + Environment.NewLine;

        private static readonly string StrConcurrentServerTransactionMessage2a =
            Catalog.GetString("If you wait a few seconds and then repeat your last action, it may succeed.") +
            Environment.NewLine + Environment.NewLine;

        private static readonly string StrConcurrentServerTransactionMessage2b =
            Catalog.GetString("The program needs to close now in order to recover from this error.  However, " +
                "if you repeat your last action when the program restarts, it will almost certainly succeed.") +
            Environment.NewLine + Environment.NewLine;

        private static readonly string StrConcurrentServerTransactionMessage3 =
            Catalog.GetString("If the action fails again, it may be necessary to close the screen you are working on " +
                "and then open it again in order to get the changes that were made to the database by the other user.") +
            Environment.NewLine + Environment.NewLine;

        private static readonly string StrConcurrentServerTransactionMessage4 =
            Catalog.GetString("We are sorry for this inconvenience.");

        /// <summary>todoComment</summary>
        private static readonly string StrConcurrentServerTransactionMessage =
            StrConcurrentServerTransactionMessage1 + StrConcurrentServerTransactionMessage2a +
            StrConcurrentServerTransactionMessage3 + StrConcurrentServerTransactionMessage4;

        /// <summary>todoComment</summary>
        private static readonly string StrConcurrentServerTransactionMessageWithShutdown =
            StrConcurrentServerTransactionMessage1 + StrConcurrentServerTransactionMessage2b +
            StrConcurrentServerTransactionMessage4;

        /// <summary>todoComment</summary>
        private static readonly string StrConcurrentServerTransactionTitle = Catalog.GetString("Message From the OpenPetra Server");


        #endregion

        /// <summary>
        /// Shows a message box apologising that a user's action failed due to conncurrent serialisable transactions
        /// </summary>
        public static void ShowTransactionSerializationExceptionDialog(bool AIsTerminating = false)
        {
            if (AIsTerminating)
            {
                MessageBox.Show(StrConcurrentServerTransactionMessageWithShutdown,
                    StrConcurrentServerTransactionTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(StrConcurrentServerTransactionMessage,
                    StrConcurrentServerTransactionTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}