//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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

using System;
using System.Diagnostics;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public class CurrencyCalculationConstantss
    {
        static public int NoRounding = 0;
        static public int CurrencyRounding = 1;
        static public int SummationRounding = 2;
    }

    /// <summary>
    /// Here all ForeigenCurrency calulation shall be done.
    /// </summary>
    public class ForeignCurrencyCalculationss
    {
        private Ict.Petra.Shared.MFinance.GL.Data.GLBatchTDS FMainDS;

        private Decimal baseCurrency;
        private Decimal foreignCurrency;
        private Decimal exchangeRate;

        /// <summary>
        /// This constructur requires the currency type and the date for the
        /// Exchange calculations.
        /// </summary>
        /// <param name="currencyCode">The Code is a valid value of the column
        /// a_currency_code_c in the table a_currency</param>
        /// <param name="date">The date is a valid entry in the table
        /// a_daily_exchange_rate</param>
        public ForeignCurrencyCalculationss(String currencyCode, DateTime date)
        {
//			System.Diagnostics.Debug.WriteLine("currencyCode: " + currencyCode);
//			System.Diagnostics.Debug.WriteLine("date: " + date.ToString());

            ADailyExchangeRateTable ADailyExchangeRateTable;
            ACurrencyTable ACurrencyTable;

            // ACurrencyLanguageTable

            //public static ACurrencyTable ACurrency;

            // FMainDS
        }

        /// <summary>
        /// This constructor may be used for the cases if the "foreign currency"
        /// is the base curency.
        /// </summary>
        public ForeignCurrencyCalculationss()
        {
            exchangeRate = 1.0m;
        }

        public void NewBaseCurrencyValue(Decimal newValue)
        {
            baseCurrency = newValue;
            foreignCurrency = baseCurrency / exchangeRate;
        }

        public Decimal ToForeign(int roundingTpye)
        {
            if (roundingTpye == CurrencyCalculationConstantss.SummationRounding)
            {
                return Math.Round(foreignCurrency, 6);
            }
            else if (roundingTpye == CurrencyCalculationConstantss.CurrencyRounding)
            {
                return Math.Round(foreignCurrency, 2);
            }

            return foreignCurrency;
        }

        /// <summary>
        /// Based on the Value of the foreign currency here a revers calculation is done.
        /// This is done because the value of the foreign currency is the only value which
        /// may be refered to calculate a balance sheet.
        /// </summary>
        /// <returns></returns>
        public Decimal RevertToBaseCurrency()
        {
            return Math.Round(
                ToForeign(CurrencyCalculationConstantss.CurrencyRounding) * exchangeRate, 2);
        }

        public decimal GetExchangeRate()
        {
            return exchangeRate;
        }
    }
}