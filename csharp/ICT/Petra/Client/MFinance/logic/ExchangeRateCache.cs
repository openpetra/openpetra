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
using System.Collections.Generic;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MFinance.Logic
{
    /// Cache for daily exchange rates
    /// Be sure to clear the cache regularly and always at the beginning of an operation, because there is no automatic update mechanism.
    public class TExchangeRateCache
    {
        private static SortedList <string, decimal>FCachedExchangeRates = new SortedList <string, decimal>();

        /// reset cache
        public static void ResetCache()
        {
            FCachedExchangeRates.Clear();
        }

        /// <summary>
        /// get exchange rate valid at the given date; uses cache to avoid too many roundtrips to the server.
        /// Be sure to clear the cache regularly and always at the beginning of an operation, because there is no automatic update mechanism.
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="ADateEffective"></param>
        /// <param name="AAllowFuzzyMatch">Set this to true to allow the database to return the rate on any date on or prior to ADateEffective.
        /// Set to false to force the database to apply the date strictly.</param>
        /// <returns></returns>
        public static decimal GetDailyExchangeRate(string ACurrencyFrom, string ACurrencyTo, DateTime ADateEffective, Boolean AAllowFuzzyMatch)
        {
            string key = ACurrencyFrom + ACurrencyTo + ADateEffective.ToShortDateString();

            if (FCachedExchangeRates.ContainsKey(key))
            {
                return FCachedExchangeRates[key];
            }

            if (ACurrencyFrom == ACurrencyTo)
            {
                return 1.0M;
            }

            decimal rate = TRemote.MFinance.GL.WebConnectors.GetDailyExchangeRate(ACurrencyFrom, ACurrencyTo, ADateEffective,
                AAllowFuzzyMatch ? -1 : 0, false);

            //
            // Don't cache a zero rate:
            //   If a zero rate is returned, the user will fix the problem and try again,
            //   so I also need to get a fresh value from the server.

            if (rate != 0)
            {
                FCachedExchangeRates.Add(key, rate);
            }

            return rate;
        }

        /// <summary>
        /// Puts a new value into the cache or updates an existing one.  Exchange rates of 0.0 and rates for currencies that are the same
        /// are ignored.
        /// </summary>
        public static void SetDailyExchangeRate(string ACurrencyFrom, string ACurrencyTo, DateTime ADateEffective, decimal AExchangeRate)
        {
            if ((ACurrencyFrom == ACurrencyTo) || (AExchangeRate == 0.0m))
            {
                return;
            }

            string key = ACurrencyFrom + ACurrencyTo + ADateEffective.ToShortDateString();

            if (FCachedExchangeRates.ContainsKey(key))
            {
                FCachedExchangeRates[key] = AExchangeRate;
            }
            else
            {
                FCachedExchangeRates.Add(key, AExchangeRate);
            }
        }
    }
}