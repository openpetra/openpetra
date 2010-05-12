//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
        /// <summary>
        /// Try to find out the ledger for the user;
        /// does check for user permissions and user defaults
        /// </summary>
        /// <returns>-1 if no ledger is available to the user,
        /// -2 if there are several ledgers available and no default ledger is set,
        /// otherwise the default ledger number for the user</returns>
        public static Int32 DetermineDefaultLedger()
        {
            DataTable ledgerTable;

            // TODO: use App.Core.Cache, GetCacheableDataTableFromPetraServer???
            TRemote.MFinance.Cacheable.RefreshCacheableTable(TCacheableFinanceTablesEnum.LedgerNameList, out ledgerTable);

            Int32 countLedgersWithPermissions = 0;
            Int32 defaultLedgerNumber = -1;

            foreach (DataRow row in ledgerTable.Rows)
            {
                Int32 ledgerNumber = Convert.ToInt32(row["LedgerNumber"]);

                if (UserInfo.GUserInfo.IsInModule("LEDGER" + String.Format("{0:0000}", ledgerNumber)))
                {
                    countLedgersWithPermissions++;
                    defaultLedgerNumber = ledgerNumber;
                }
            }

            // remove ledgers that the user does not have access to
            if (countLedgersWithPermissions == 1)
            {
                return defaultLedgerNumber;
            }
            else if (countLedgersWithPermissions == 0)
            {
                return -1;
            }

            // TODO: check user default for ledger selection
            // if user has default ledger, return that ledger

            // several ledgers are available to the user
            // this function is called from TDlgSelectLedger, and it will show a list of ledgers to the user to choose
            return -2;
        }

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
            out DateTime AEndDateLastForwardingPeriod,
            out DateTime ADefaultDate)
        {
            if (!FValidPostingDates.ContainsKey(ALedgerNumber))
            {
                ResetValidDates(ALedgerNumber);
            }

            if (FValidPostingDates.ContainsKey(ALedgerNumber))
            {
                AStartDateCurrentPeriod = FValidPostingDates[ALedgerNumber][0];
                AEndDateLastForwardingPeriod = FValidPostingDates[ALedgerNumber][1];

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

            AStartDateCurrentPeriod = DateTime.MinValue;
            AEndDateLastForwardingPeriod = DateTime.MinValue;
            ADefaultDate = DateTime.MinValue;
            return false;
        }
    }
}