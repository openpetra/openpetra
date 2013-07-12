//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Windows.Forms;

using Ict.Common;

namespace Ict.Petra.Client.MFinance.Logic
{
    /// <summary>
    /// Common logic for the two Exchange Rate Setup (Corporate and Daily) screens.
    /// </summary>
    public class TSetupExchangeRates
    {
        /// <summary>
        /// Displays how the Exchange Rate entered applies to the entered currencies in both directions.
        /// </summary>
        /// <param name="AFromCurrencyCode">Currency Code to convert from.</param>
        /// <param name="AToCurrencyCode">Currency Code to convert to.</param>
        /// <param name="AExchangeRateRow">DataRow that contains the data.</param>
        /// <param name="AExchangeRate">Exchange Rate.</param>
        /// <param name="AValueOneDirectionLabel">Label that shows the exchange rate information in one direction (gets updated).</param>
        /// <param name="AValueOtherDirectionLabel">Label that shows the exchange rate information in the other direction (gets updated).</param>
        public static void SetExchangeRateLabels(String AFromCurrencyCode, String AToCurrencyCode,
            DataRow AExchangeRateRow, decimal AExchangeRate, Label AValueOneDirectionLabel, Label AValueOtherDirectionLabel)
        {
            string StrLabelText = Catalog.GetString("For {0} {1} one will get {2} {3}.");

            if (AExchangeRateRow == null)
            {
                AValueOneDirectionLabel.Text = "-";
            }
            else
            {
                AValueOneDirectionLabel.Text =
                    String.Format(StrLabelText,
                        1.0m.ToString("N2"),
                        AToCurrencyCode,
                        AExchangeRate.ToString("N10"),
                        AFromCurrencyCode
                        );
            }

            if (AExchangeRate != 0)
            {
                AExchangeRate = 1 / AExchangeRate;
            }

            if (AExchangeRateRow == null)
            {
                AValueOtherDirectionLabel.Text = "-";
            }
            else
            {
                AValueOtherDirectionLabel.Text =
                    String.Format(StrLabelText,
                        1.0m.ToString("N2"),
                        AFromCurrencyCode,
                        AExchangeRate.ToString("N10"),
                        AToCurrencyCode
                        );
            }
        }
    }
}