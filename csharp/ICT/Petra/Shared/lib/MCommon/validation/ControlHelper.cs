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
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;

namespace Ict.Petra.Shared.MCommon.Validation
{
    /// <summary>
    /// Contains helper functions for the shared validation of data, specific to Controls.
    /// </summary>
    public static class TSharedValidationControlHelper
    {
        /// <summary>
        /// Delegate for invoking the simple data reader.
        /// </summary>
        public delegate TVerificationResult TSharedGetDateVerificationResult(Control APetraDateControl);

        /// <summary>
        /// Reference to the Delegate for invoking the getting of a DateVerificationResult.
        /// </summary>
        private static TSharedGetDateVerificationResult FDelegateSharedGetDateVerificationResult;

        /// <summary>
        /// This property is used to provide a function which invokes the simple data reader.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TSharedGetDateVerificationResult SharedGetDateVerificationResultDelegate
        {
            get
            {
                return FDelegateSharedGetDateVerificationResult;
            }

            set
            {
                FDelegateSharedGetDateVerificationResult = value;
            }
        }

        /// <summary>
        /// Checks wheter a given DateTime is an invalid date. A check whether it is an undefined DateTime is always performed.
        /// If Delegate <see cref="SharedGetDateVerificationResultDelegate" /> is set up and Argument
        /// <paramref name="AResultControl" /> isn't null, the 'DateVerificationResult' of the TtxtPetraDate Control is
        /// returned by this Method through this Method if it isn't null. That way the Data Validation Framework can
        /// use the detailed Data Verification error that is held by the Control.
        /// </summary>
        /// <returns>Null if validation succeeded, otherwise a <see cref="TVerificationResult" /> is
        /// returned that contains details about the problem.</returns>
        public static TVerificationResult IsNotInvalidDate(DateTime? ADate, String ADescription,
            TVerificationResultCollection AVerificationResultCollection, bool ATreatNullAsInvalid = false,
            object AResultContext = null, System.Data.DataColumn AResultColumn = null,
            Control AResultControl = null)
        {
            TVerificationResult VerificationResult;

            if (FDelegateSharedGetDateVerificationResult != null)
            {
                if ((AResultControl != null))
                {
                    VerificationResult = FDelegateSharedGetDateVerificationResult(AResultControl);

                    if (VerificationResult == null)
                    {
                        VerificationResult = TDateChecks.IsNotUndefinedDateTime(ADate,
                            ADescription, ATreatNullAsInvalid, AResultContext, AResultColumn, AResultControl);
                    }
                    else
                    {
                        VerificationResult.OverrideResultContext(AResultContext);
                        VerificationResult = new TScreenVerificationResult(VerificationResult, AResultColumn, AResultControl);
                    }
                }
                else
                {
                    VerificationResult = TDateChecks.IsNotUndefinedDateTime(ADate,
                        ADescription, ATreatNullAsInvalid, AResultContext, AResultColumn, AResultControl);
                }

                // Remove Verification Result that would have been recorded earlier for the same DataColumn
                TVerificationResult OtherRecordedVerificationResult = AVerificationResultCollection.FindBy(AResultColumn);

                if (OtherRecordedVerificationResult != null)
                {
                    AVerificationResultCollection.Remove(OtherRecordedVerificationResult);
                }
            }
            else
            {
                VerificationResult = TDateChecks.IsNotUndefinedDateTime(ADate,
                    ADescription, ATreatNullAsInvalid, AResultContext, AResultColumn, AResultControl);
            }

            return VerificationResult;
        }
    }
}