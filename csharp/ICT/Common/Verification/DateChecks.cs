//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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
using Ict.Common.Verification;
using GNU.Gettext;
using System.Windows.Forms;

namespace Ict.Common.Verification
{
    /// <summary>
    /// Class for date verification that are needed both on Server and Client side.
    /// </summary>
    /// <remark> None of the data verifications in here must access the database
    ///   since the Client doesn't have access to the database!
    /// </remark>
    public class TDateChecks : System.Object
    {
        /// <summary>
        /// error code constant X_0041 for invalid dates
        /// </summary>
        public const String ERRORCODE_INVALIDDATE = "X_0041";

        /// <summary>
        /// error code constant X_0029 for unappropriate future date
        /// </summary>
        public const String ERRORCODE_NOFUTUREDATE = "X_0029";

        /// <summary>
        /// Checks whether the first submitted data is earlier or equal than the second
        /// submitted date. Nil dates are accepted.
        ///
        /// </summary>
        /// <param name="ADate1">First date</param>
        /// <param name="ADate2">Second date</param>
        /// <param name="AFirstDateDescription">Description what the first date is about (for the
        /// error message)</param>
        /// <param name="ASecondDateDescription">Description what the second date is about (for
        /// the error message)</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.</returns>
        public static TVerificationResult FirstLesserOrEqualThanSecondDate(DateTime ADate1,
            DateTime ADate2,
            String AFirstDateDescription,
            String ASecondDateDescription)
        {
            TVerificationResult ReturnValue;

            if (ADate1 <= ADate2)
            {
                //MessageBox.Show('ADate1 <= ADate2');
                ReturnValue = null;
            }
            else
            {
                if (ADate2 != new DateTime(0001, 1, 1))
                {
                    ReturnValue = new TVerificationResult("",
                        Catalog.GetString("Invalid date entered.") +
                        Environment.NewLine +
                        String.Format(Catalog.GetString("'{0}' cannot be later than '{1}'."),
                            AFirstDateDescription, ASecondDateDescription),
                        Catalog.GetString("Invalid Data"),
                        ERRORCODE_INVALIDDATE,
                        TResultSeverity.Resv_Critical);
                }
                else
                {
                    ReturnValue = null;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether the first submitted data is earlier than the second submitted
        /// date. Nil dates are accepted.
        ///
        /// </summary>
        /// <param name="ADate1">First date</param>
        /// <param name="ADate2">Second date</param>
        /// <param name="AFirstDateDescription">Description what the first date is about (for the
        /// error message)</param>
        /// <param name="ASecondDateDescription">Description what the second date is about (for
        /// the error message)</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.</returns>
        public static TVerificationResult FirstLesserThanSecondDate(DateTime ADate1,
            DateTime ADate2,
            String AFirstDateDescription,
            String ASecondDateDescription)
        {
            TVerificationResult ReturnValue;

            if (ADate1 < ADate2)
            {
                //MessageBox.Show('ADate1 < ADate2');
                ReturnValue = null;
            }
            else
            {
                if (ADate2 != new DateTime(0001, 1, 1))
                {
                    ReturnValue = new TVerificationResult("",
                        Catalog.GetString("Invalid date entered.") +
                        Environment.NewLine +
                        String.Format(Catalog.GetString("'{0}' cannot be later or equal than '{1}'."),
                            AFirstDateDescription, ASecondDateDescription),
                        Catalog.GetString("Invalid Data"),
                        ERRORCODE_INVALIDDATE,
                        TResultSeverity.Resv_Critical);
                }
                else
                {
                    ReturnValue = null;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// this is called if the date is invalid
        /// </summary>
        /// <param name="ADescription">either a name for the date value or empty</param>
        /// <returns>a Verification Result with the error message</returns>
        public static TVerificationResult GetInvalidDateVerificationResult(String ADescription)
        {
            String Description;

            if (ADescription != "")
            {
                Description = "'" + ADescription + "'";
            }
            else
            {
                Description = "Value";
            }

            return new TVerificationResult("",
                Catalog.GetString("Invalid date entered.") +
                Environment.NewLine +
                String.Format(Catalog.GetString("{0} must be a date.'"),
                    Description),
                Catalog.GetString("Invalid Data"),
                ERRORCODE_INVALIDDATE,
                TResultSeverity.Resv_Critical);
        }

        /// <summary>
        /// make sure that the date is today or in the future
        /// </summary>
        /// <param name="ADate">date to check</param>
        /// <param name="ADateDescription">name of the date value</param>
        /// <returns>null if everything is ok, otherwise an error message in VerificationResult</returns>
        public static TVerificationResult IsCurrentOrFutureDate(DateTime ADate, String ADateDescription)
        {
            TVerificationResult ReturnValue;
            DateTime TheDate = TSaveConvert.ObjectToDate(ADate);

            if (TheDate >= DateTime.Today)
            {
                //MessageBox.Show('Date >= Today');
                ReturnValue = null;
            }
            else
            {
                if (TheDate != DateTime.MinValue)
                {
                    ReturnValue = new TVerificationResult("",
                        Catalog.GetString("Invalid date entered.") +
                        Environment.NewLine +
                        String.Format(Catalog.GetString("'{0}' may not be a past date.'"),
                            ADateDescription),
                        Catalog.GetString("Invalid Data"),
                        ERRORCODE_NOFUTUREDATE,
                        TResultSeverity.Resv_Critical);

                    // ERRORCODE_NOFUTUREDATE (X_0029) is defined in s_error_messages solely for future dates, so we should find a new X_??? code for past dates...
                }
                else
                {
                    ReturnValue = null;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Check that the date is today or in the past
        /// </summary>
        /// <param name="ADate">the date to check</param>
        /// <param name="ADateDescription">the name of the date value</param>
        /// <returns>null if everything is fine, otherwise error message in Verification Result</returns>
        public static TVerificationResult IsCurrentOrPastDate(DateTime ADate, String ADateDescription)
        {
            TVerificationResult ReturnValue;
            DateTime TheDate = TSaveConvert.ObjectToDate(ADate);

            if (TheDate <= DateTime.Today)
            {
                //MessageBox.Show('Date <= Today');
                ReturnValue = null;
            }
            else
            {
                if (TheDate != DateTime.MinValue)
                {
                    ReturnValue = new TVerificationResult("",
                        Catalog.GetString("Invalid date entered.") +
                        Environment.NewLine +
                        String.Format(Catalog.GetString("'{0}' may not be a future date.'"),
                            ADateDescription),
                        Catalog.GetString("Invalid Data"),
                        ERRORCODE_NOFUTUREDATE,
                        TResultSeverity.Resv_Critical);
                }
                else
                {
                    ReturnValue = null;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Check that the date is valid
        /// </summary>
        /// <param name="ADate">the date to check</param>
        /// <param name="ADateDescription">the name of the date value</param>
        /// <returns>null if everything is fine, otherwise error message in Verification Result</returns>
        public static TVerificationResult IsUndefinedDateTime(DateTime ADate, String ADateDescription)
        {
            TVerificationResult ReturnValue;
            DateTime TheDate = TSaveConvert.ObjectToDate(ADate);

            if (TheDate != DateTime.MinValue)
            {
                //MessageBox.Show('Date <> DateTime.MinValue');
                ReturnValue = null;
            }
            else
            {
                ReturnValue = new TVerificationResult("",
                    Catalog.GetString("Invalid date entered.") +
                    Environment.NewLine +
                    String.Format(Catalog.GetString("'{0}' may not be empty.'"),
                        ADateDescription),
                    Catalog.GetString("Invalid Data"),
                    ERRORCODE_INVALIDDATE,
                    TResultSeverity.Resv_Critical);

                // ERRORCODE_INVALIDDATE (X_0041) is defined in s_error_messages solely for invalid dates, so we should find a new X_??? code for empty dates...
            }

            return ReturnValue;
        }

        /// <summary>
        /// Finds out if the string s contains a valid DateTime; otherwise return a verification result with a message using ADescription
        /// </summary>
        /// <returns>void</returns>
        public static TVerificationResult IsValidDateTime(String s, String ADescription)
        {
            TVerificationResult ReturnValue = null;

            try
            {
                System.Convert.ToDateTime(s);
            }
            catch (System.Exception)
            {
                ReturnValue = GetInvalidDateVerificationResult(ADescription);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether the first submitted data is later or equal than the second
        /// submitted date. Nil dates are accepted.
        ///
        /// </summary>
        /// <param name="ADate1">First date</param>
        /// <param name="ADate2">Second date</param>
        /// <param name="AFirstDateDescription">Description what the first date is about (for the
        /// error message)</param>
        /// <param name="ASecondDateDescription">Description what the second date is about (for
        /// the error message)</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.</returns>
        public static TVerificationResult FirstGreaterOrEqualThanSecondDate(DateTime ADate1,
            DateTime ADate2,
            String AFirstDateDescription,
            String ASecondDateDescription)
        {
            TVerificationResult ReturnValue;

            if (ADate1 >= ADate2)
            {
                //MessageBox.Show('ADate1 <= ADate2');
                ReturnValue = null;
            }
            else
            {
                if (ADate1 != new DateTime(0001, 1, 1))
                {
                    ReturnValue = new TVerificationResult("",
                        Catalog.GetString("Invalid date entered.") +
                        Environment.NewLine +
                        String.Format(Catalog.GetString("'{0}' cannot be earlier than '{1}'."),
                            AFirstDateDescription, ASecondDateDescription),
                        Catalog.GetString("Invalid Data"),
                        ERRORCODE_INVALIDDATE,
                        TResultSeverity.Resv_Critical);
                }
                else
                {
                    ReturnValue = null;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether the first submitted date is later than the second submitted
        /// date. Nil dates are accepted.
        ///
        /// </summary>
        /// <param name="ADate1">First date</param>
        /// <param name="ADate2">Second date</param>
        /// <param name="AFirstDateDescription">Description what the first date is about (for the
        /// error message)</param>
        /// <param name="ASecondDateDescription">Description what the second date is about (for
        /// the error message)</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.</returns>
        public static TVerificationResult FirstGreaterThanSecondDate(DateTime ADate1,
            DateTime ADate2,
            String AFirstDateDescription,
            String ASecondDateDescription)
        {
            TVerificationResult ReturnValue;

            if (ADate1 > ADate2)
            {
                //MessageBox.Show('ADate1 <= ADate2');
                ReturnValue = null;
            }
            else
            {
                if (ADate1 != new DateTime(0001, 1, 1))
                {
                    ReturnValue = new TVerificationResult("",
                        Catalog.GetString("Invalid date entered.") +
                        Environment.NewLine +
                        String.Format(Catalog.GetString("'{0}' cannot be earlier or equal than '{1}'."),
                            AFirstDateDescription, ASecondDateDescription),
                        Catalog.GetString("Invalid Data"),
                        ERRORCODE_INVALIDDATE,
                        TResultSeverity.Resv_Critical);
                }
                else
                {
                    ReturnValue = null;
                }
            }

            return ReturnValue;
        }
    }
}