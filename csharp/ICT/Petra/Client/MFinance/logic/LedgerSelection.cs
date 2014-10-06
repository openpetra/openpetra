//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.Collections.Generic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MFinance.Logic
{
    /// <summary>
    /// functionality for the Ledger Selection screen
    /// </summary>
    public class TLedgerSelection
    {
        private static SortedList <Int32, DateTime[]>FValidPostingDates = new SortedList <int, DateTime[]>();

        /// <summary>
        /// to be called if no valid dates exist yet in the cache;
        /// also to be called by period end to reset the cache
        /// </summary>
        public static void ResetValidDates(Int32 ALedgerNumber)
        {
            DateTime StartDateCurrentPeriod;
            DateTime EndDateLastForwardingPeriod;

            TRemote.MFinance.GL.WebConnectors.GetCurrentPostingRangeDates(ALedgerNumber, out StartDateCurrentPeriod, out EndDateLastForwardingPeriod);

            if (FValidPostingDates.ContainsKey(ALedgerNumber))
            {
                FValidPostingDates.Remove(ALedgerNumber);
            }

            FValidPostingDates.Add(ALedgerNumber, new DateTime[] { StartDateCurrentPeriod, EndDateLastForwardingPeriod });
        }

        /// <summary>
        /// Get the valid dates for posting;
        /// based on current period and number of forwarding periods
        /// </summary>
        public static bool GetCurrentPostingRangeDates(Int32 ALedgerNumber,
            out DateTime AStartDateCurrentPeriod,
            out DateTime AEndDateLastForwardingPeriod)
        {
            bool RetVal = false;

            AStartDateCurrentPeriod = DateTime.MinValue;
            AEndDateLastForwardingPeriod = DateTime.MinValue;

            if (!FValidPostingDates.ContainsKey(ALedgerNumber))
            {
                ResetValidDates(ALedgerNumber);
            }

            if (FValidPostingDates.ContainsKey(ALedgerNumber))
            {
                AStartDateCurrentPeriod = FValidPostingDates[ALedgerNumber][0];
                AEndDateLastForwardingPeriod = FValidPostingDates[ALedgerNumber][1];

                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Get the valid dates for posting;
        /// based on current period and number of forwarding periods
        /// </summary>
        public static bool GetCurrentPostingRangeDates(Int32 ALedgerNumber,
            out DateTime AStartDateCurrentPeriod,
            out DateTime AEndDateLastForwardingPeriod,
            out DateTime ADefaultDate)
        {
            if (GetCurrentPostingRangeDates(ALedgerNumber,
                    out AStartDateCurrentPeriod,
                    out AEndDateLastForwardingPeriod))
            {
                if ((DateTime.Now >= AStartDateCurrentPeriod) && (DateTime.Now <= AEndDateLastForwardingPeriod))
                {
                    ADefaultDate = DateTime.Now;
                }
                else
                {
                    ADefaultDate = AEndDateLastForwardingPeriod;
                }

                return true;
            }

            ADefaultDate = DateTime.MinValue;
            return false;
        }
    }
}