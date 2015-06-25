//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2012 by OM International
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
using Ict.Common.Verification;

namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// Contains helper Methods for data validation.
    /// </summary>
    public static class TDataValidation
    {
        /// <summary>
        /// Checks for data verification errors and displays appropriate error messages. Returns true in case there
        /// were no data verification errors, otherwise false.
        /// </summary>
        /// <param name="ARecordChangeVerification">Set this to true if this Method is called in the context of a record change
        /// in a Grid, otherwise set it to false.</param>
        /// <param name="AVerificationResultCollection">A <see cref="TVerificationResultCollection" /> to inspect for
        /// data verification errors.</param>
        /// <param name="ATypeWhichRaisesError">Instance of the calling WinForm.</param>
        /// <param name="ARestrictToTypeWhichRaisesError">Restricts the <see cref="TVerificationResult" />s that
        /// are considered by this Method to those whose <see cref="TVerificationResult.ResultContext" /> matches
        /// <paramref name="ARestrictToTypeWhichRaisesError"></paramref> (defaults to null).</param>
        /// <param name="AIgnoreWarnings">Set to true if Warnings are to be ignored (defaults to false).</param>
        /// <returns>True in case there were no data verification errors, otherwise false.</returns>
        public static bool ProcessAnyDataValidationErrors(bool ARecordChangeVerification,
            TVerificationResultCollection AVerificationResultCollection,
            Type ATypeWhichRaisesError, Type ARestrictToTypeWhichRaisesError = null, bool AIgnoreWarnings = false)
        {
            bool ReturnValue = false;
            string ErrorMessages;
            Control FirstErrorControl;
            object FirstErrorContext;
            bool RecordDeletionErrorsBecauseOfReference = false;

            // In case there were only warnings, we return true and record change/saving of data can go ahead,
            // otherwise false is returned to prevent record change/saving of data.
            if (TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResultCollection))
            {
                ReturnValue = true;
            }

            if (AVerificationResultCollection.HasCriticalOrNonCriticalErrors)
            {
                // Determine data validation message, and more
                AVerificationResultCollection.BuildScreenVerificationResultList(out ErrorMessages,
                    out FirstErrorControl, out FirstErrorContext, true, ARestrictToTypeWhichRaisesError, AIgnoreWarnings);

                // Tell user that there are data validation errors if there are any
                if (ErrorMessages != String.Empty)
                {
                    if (ARecordChangeVerification)
                    {
                        TMessages.MsgRecordChangeVerificationError(ErrorMessages, ReturnValue, ATypeWhichRaisesError);
                    }
                    else
                    {
                        for (int Counter = 0; Counter < AVerificationResultCollection.Count; Counter++)
                        {
                            if (AVerificationResultCollection[Counter].ResultCode == CommonErrorCodes.ERR_RECORD_DELETION_NOT_POSSIBLE_REFERENCED)
                            {
                                RecordDeletionErrorsBecauseOfReference = true;
                                break;
                            }
                        }

                        if (!RecordDeletionErrorsBecauseOfReference)
                        {
                            TMessages.MsgFormSaveVerificationError(ErrorMessages, ReturnValue, ATypeWhichRaisesError);
                        }
                        else
                        {
                            TMessages.MsgFormSaveVerificationError(ErrorMessages,
                                CommonErrorCodes.ERR_RECORD_DELETION_NOT_POSSIBLE_REFERENCED,
                                ReturnValue, ATypeWhichRaisesError,
                                true);
                        }
                    }

                    // Put Focus on first Control that an error was recorded for
                    if (FirstErrorControl != null)
                    {
                        FirstErrorControl.Focus();
                    }
                }
                else
                {
                    ReturnValue = true;
                }
            }
            else
            {
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Processes validation warnings by showing a Yes/No message box to the client.  The client can choose to ignore
        /// the warnings and proceed with, for example, saving the data - or to cancel saving and adjust the entered data.
        /// The method should only be used when it has been established that there are no validation errors but there is at
        /// least one warning. Returns true in case the user answers Yes to the question, false otherwise.
        /// </summary>
        /// <param name="AVerificationResultCollection">A <see cref="TVerificationResultCollection" /> to inspect for
        /// data verification errors.</param>
        /// <param name="AQuestion">A Yes/No question to the client that is appended to the warning list.</param>
        /// <param name="ATypeWhichRaisesError">Instance of the calling WinForm.</param>
        /// <returns>True (success) in case the user answers Yes, otherwise false.</returns>
        public static bool ProcessAnyDataValidationWarnings(TVerificationResultCollection AVerificationResultCollection,
            String AQuestion, Type ATypeWhichRaisesError)
        {
            bool ReturnValue = true;

            if (AVerificationResultCollection.HasCriticalOrNonCriticalErrors && AVerificationResultCollection.HasOnlyNonCriticalErrors)
            {
                ReturnValue = false;

                string errorMessages;
                Control firstControl;
                object context;
                AVerificationResultCollection.BuildScreenVerificationResultList(out errorMessages, out firstControl, out context);

                if (TMessages.MsgFormSaveVerificationWarning(errorMessages, AVerificationResultCollection[0].ResultCode,
                        ATypeWhichRaisesError, AQuestion, false) == System.Windows.Forms.DialogResult.Yes)
                {
                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }
    }
}