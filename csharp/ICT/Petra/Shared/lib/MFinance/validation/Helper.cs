//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2017 by OM International
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
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;

namespace Ict.Petra.Shared.MFinance.Validation
{
    /// <summary>
    /// Contains helper functions for the shared validation of Finance data.
    /// </summary>
    public static class TSharedFinanceValidationHelper
    {
        /// <summary>
        /// Delegate for invoking the process of finding the valid start and end dates for the specified Ledger.
        /// </summary>
        public delegate bool TGetValidPostingDateRange(Int32 ALedgerNumber,
            out DateTime AStartDateCurrentPeriod,
            out DateTime AEndDateLastForwardingPeriod);

        /// <summary>
        /// Delegate for invoking the process of finding the valid start and end dates for the specified period.
        /// </summary>
        public delegate bool TGetValidPeriodDates(Int32 ALedgerNumber,
            Int32 AYearNumber,
            Int32 ADiffPeriod,
            Int32 APeriodNumber,
            out DateTime AStartDatePeriod,
            out DateTime AEndDatePeriod);

        /// <summary>
        /// Delegate for invoking the process of finding the first day in the accounting period for the specified Ledger and date.
        /// </summary>
        public delegate bool TGetFirstDayOfAccountingPeriod(Int32 ALedgerNumber,
            DateTime ADateInAPeriod,
            out DateTime AFirstDayOfPeriod);

        /// <summary>
        /// Reference to the Delegate for invoking the verification of the existence of a Finance.
        /// </summary>
        private static TGetValidPostingDateRange FDelegateGetValidPostingDateRange;

        /// <summary>
        /// Reference to the Delegate for invoking the verification of the existence of a Finance.
        /// </summary>
        private static TGetValidPeriodDates FDelegateGetValidPeriodDates;

        /// <summary>
        /// Reference to the Delegate for discovering the first day of the accounting period for a date.
        /// </summary>
        private static TGetFirstDayOfAccountingPeriod FDelegateGetFirstDayOfAccountingPeriod;

        /// <summary>
        /// This property is used to provide a function which invokes the verification of the existence of a Finance.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TGetValidPostingDateRange GetValidPostingDateRangeDelegate
        {
            get
            {
                return FDelegateGetValidPostingDateRange;
            }

            set
            {
                FDelegateGetValidPostingDateRange = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which invokes the verification of the existence of a Finance.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TGetValidPeriodDates GetValidPeriodDatesDelegate
        {
            get
            {
                return FDelegateGetValidPeriodDates;
            }

            set
            {
                FDelegateGetValidPeriodDates = value;
            }
        }

        /// <summary>
        /// This property is used to provide a function which determines the first day of the accounting period for the ledger and date specified.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TGetFirstDayOfAccountingPeriod GetFirstDayOfAccountingPeriodDelegate
        {
            get
            {
                return FDelegateGetFirstDayOfAccountingPeriod;
            }

            set
            {
                FDelegateGetFirstDayOfAccountingPeriod = value;
            }
        }

        /// <summary>
        /// Get the valid posting date range for the specified ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AStartDateCurrentPeriod"></param>
        /// <param name="AEndDateLastForwardingPeriod"></param>
        /// <returns>true if dates are returned OK</returns>
        public static bool GetValidPostingDateRange(Int32 ALedgerNumber,
            out DateTime AStartDateCurrentPeriod,
            out DateTime AEndDateLastForwardingPeriod)
        {
            if (FDelegateGetValidPostingDateRange != null)
            {
                return FDelegateGetValidPostingDateRange(ALedgerNumber, out AStartDateCurrentPeriod, out AEndDateLastForwardingPeriod);
            }
            else
            {
                throw new InvalidOperationException("Delegate 'TGetValidPostingDateRange' must be initialised before calling this Method");
            }
        }

        /// <summary>
        /// Get the valid date range for the specified period
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AYearNumber"></param>
        /// <param name="ADiffPeriod"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="AStartDatePeriod"></param>
        /// <param name="AEndDatePeriod"></param>
        /// <returns>true if dates are returned OK</returns>
        public static bool GetValidPeriodDates(Int32 ALedgerNumber,
            Int32 AYearNumber,
            Int32 ADiffPeriod,
            Int32 APeriodNumber,
            out DateTime AStartDatePeriod,
            out DateTime AEndDatePeriod)
        {
            if (FDelegateGetValidPeriodDates != null)
            {
                return FDelegateGetValidPeriodDates(ALedgerNumber, AYearNumber, ADiffPeriod, APeriodNumber, out AStartDatePeriod, out AEndDatePeriod);
            }
            else
            {
                throw new InvalidOperationException("Delegate 'TGetValidPostingDateRange' must be initialised before calling this Method");
            }
        }

        /// <summary>
        /// Get the first day in the accounting period for any specified date/ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADateInAPeriod"></param>
        /// <param name="AFirstDayOfPeriod"></param>
        /// <returns>The first day in the accounting period for the date specified.</returns>
        public static bool GetFirstDayOfAccountingPeriod(Int32 ALedgerNumber, DateTime ADateInAPeriod, out DateTime AFirstDayOfPeriod)
        {
            if (FDelegateGetFirstDayOfAccountingPeriod != null)
            {
                return FDelegateGetFirstDayOfAccountingPeriod(ALedgerNumber, ADateInAPeriod, out AFirstDayOfPeriod);
            }
            else
            {
                throw new InvalidOperationException("Delegate 'TGetFirstDayOfAccountingPeriod' must be initialised before calling this Method");
            }
        }
    }
}
