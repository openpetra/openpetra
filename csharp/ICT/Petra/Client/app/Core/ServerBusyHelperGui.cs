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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.DB.Exceptions;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Helper Class for dealing with 'server busy' state (which can occur due to the prevention of multi-threading DB access problems).
    /// </summary>
    /// <remarks>This Class can be used in connection with functionality that the the Class
    /// <see cref="Ict.Common.DB.TServerBusyHelper"/> provides for the provision of user interaction.</remarks>
    public static class TServerBusyHelperGui
    {
        #region Resourcestrings

        private static readonly string StrLoadingOfDataGotCancelled = Catalog.GetString(
            "The request for loading of data from the OpenPetra Server got cancelled.");

        private static readonly string StrLoadingOfDataGotCancelledTitle = Catalog.GetString(
            "Loading of Data Cancelled");

        #endregion

        /// <summary>
        /// Shows the 'Server is busy' Dialog.
        /// </summary>
        /// <returns>What the user chose: Either <see cref="System.Windows.Forms.DialogResult.Retry"/> or
        /// <see cref="System.Windows.Forms.DialogResult.Cancel"/>.</returns>
        public static DialogResult ShowServerBusyDialogGeneric()
        {
            return MessageBox.Show(String.Format(AppCoreResourcestrings.StrPetraServerTooBusyWaitAFewSecondsWithRetryCancel,
                    Catalog.GetString("process the request.")), AppCoreResourcestrings.StrPetraServerTooBusyTitle,
                MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Shows the 'Server is busy' Dialog.
        /// </summary>
        /// <remarks>When the opening of a Form is timing out then call <see cref="ShowServerBusyDialogWhenOpeningForm"/>
        /// instead!</remarks>
        /// <param name="ATooBusyForWhat">Explain *what* the server is to busy for (becomes part of the message that
        /// is shown to the user, so please 'Catalog.GetString' that.)</param>
        /// <returns>What the user chose: Either <see cref="System.Windows.Forms.DialogResult.Retry"/> or
        /// <see cref="System.Windows.Forms.DialogResult.Cancel"/>.</returns>
        public static DialogResult ShowServerBusyDialog(string ATooBusyForWhat)
        {
            return MessageBox.Show(String.Format(AppCoreResourcestrings.StrPetraServerTooBusyWaitAFewSecondsWithRetryCancel,
                    ATooBusyForWhat), AppCoreResourcestrings.StrPetraServerTooBusyTitle,
                MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Shows the 'Server is busy' Dialog. Call this Method when the opening of a Form is timing out.
        /// </summary>
        /// <param name="AFormTitle">Title of the Form that was about to be opened (becomes part of the message that
        /// is shown to the user, so please 'Catalog.GetString' that.)</param>
        /// <returns>What the user chose: Either <see cref="System.Windows.Forms.DialogResult.Retry"/> or
        /// <see cref="System.Windows.Forms.DialogResult.Cancel"/>.</returns>
        public static DialogResult ShowServerBusyDialogWhenOpeningForm(string AFormTitle)
        {
            return MessageBox.Show(String.Format(AppCoreResourcestrings.StrPetraServerTooBusyWaitAFewSecondsWithRetryCancel,
                    "open the " + AFormTitle + " screen"), AppCoreResourcestrings.StrPetraServerTooBusyTitle,
                MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Shows the 'Loading of Data Cancelled' Dialog.
        /// </summary>
        public static void ShowLoadingOfDataGotCancelledDialog()
        {
            MessageBox.Show(StrLoadingOfDataGotCancelled, StrLoadingOfDataGotCancelledTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows the 'Server too busy to perform the requested action' Dialog.
        /// </summary>
        /// <param name="ADerivedException">An Exception instance that is derived from
        /// <see cref="EDBAccessLackingCoordinationException"/>.</param>
        /// <param name="AReason">Contains the reason as to why the action was not possible.</param>
        public static void ShowDBAccessLackingActionNotPossibleDialog(
            EDBAccessLackingCoordinationException ADerivedException, out string AReason)
        {
            AReason = Catalog.GetString("Unknown.");

            if (ADerivedException as EDBTransactionBusyException != null)
            {
                AReason = Catalog.GetString("Waiting time for initiating exclusive data access exceeded.");
            }
            else if (ADerivedException as EDBTransactionIsolationLevelWrongException != null)
            {
                AReason = Catalog.GetString("Shared data access could not be initiated.");
            }
            else if (ADerivedException as EDBCoordinatedDBAccessWaitingTimeExceededException != null)
            {
                AReason = Catalog.GetString("Waiting time for data access exceeded.");
            }
            else if
            (
                (ADerivedException as EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException != null)
                || (ADerivedException as
                    EDBAttemptingToCreateCommandOnDifferentDBConnectionThanTheDBConnectionOfOfTheDBTransactionThatGotPassedException != null)
                || (ADerivedException as EDBAttemptingToCreateCommandEnlistedInDifferentDBTransactionThanTheCurrentDBTransactionException != null)
                || (ADerivedException as EDBAttemptingToCloseDBConnectionThatGotEstablishedOnDifferentThreadException != null)
            )
            {
                AReason = Catalog.GetString("Parallel data access could not be performed.");
            }

            MessageBox.Show(String.Format(AppCoreResourcestrings.StrPetraServerTooBusyWaitAFewSecondsNoAutomaticRetryCancel,
                    String.Format(Catalog.GetString("perform the requested action.  (Reason: {0})"), AReason)),
                AppCoreResourcestrings.StrPetraServerTooBusyTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}