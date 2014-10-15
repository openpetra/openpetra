//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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

using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Shared.MFinance
{
    /// <summary>
    /// Contains several functions which are specific to the Petra Finance Module.
    /// </summary>
    public static class TaxDeductibility
    {
        /// <summary>
        /// Calculate TaxDeductibleAmount and NonDeductibleAmount for a gift transaction using the tax deductible percentage
        /// </summary>
        /// <param name="ATaxDeductAmount"></param>
        /// <param name="ANonDeductAmount"></param>
        /// <param name="AGiftTransactionAmount"></param>
        /// <param name="ADeductiblePercentage"></param>
        public static void UpdateTaxDeductibilityAmounts(ref decimal ATaxDeductAmount, ref decimal ANonDeductAmount, decimal AGiftTransactionAmount, decimal ADeductiblePercentage)
        {
            ATaxDeductAmount =
                AGiftTransactionAmount * (ADeductiblePercentage / 100);
            ANonDeductAmount =
                AGiftTransactionAmount * (1 - (ADeductiblePercentage / 100));

            // correct rounding errors
            while (ATaxDeductAmount + ANonDeductAmount
                   < AGiftTransactionAmount)
            {
                if (ADeductiblePercentage >= 50)
                {
                    ATaxDeductAmount += (decimal)0.01;
                }
                else
                {
                    ANonDeductAmount += (decimal)0.01;
                }
            }

            while (ATaxDeductAmount + ANonDeductAmount
                   > AGiftTransactionAmount)
            {
                if (ADeductiblePercentage >= 50)
                {
                    ATaxDeductAmount -= (decimal)0.01;
                }
                else
                {
                    ANonDeductAmount -= (decimal)0.01;
                }
            }
        }
        
        /// <summary>
        /// Calculate the Base and Intl amounts for TaxDeductibleAmount and NonDeductibleAmount
        /// </summary>
        /// <param name="AGiftDetail"></param>
        /// <param name="ABatchExchangeRateToBase"></param>
        /// <param name="AIntlToBaseCurrencyExchRate"></param>
        /// <param name="AIsTransactionInIntlCurrency"></param>
        public static void UpdateTaxDeductibiltyCurrencyAmounts(ref GiftBatchTDSAGiftDetailRow AGiftDetail,
        	decimal ABatchExchangeRateToBase, decimal AIntlToBaseCurrencyExchRate, bool AIsTransactionInIntlCurrency)
        {
        	AGiftDetail.TaxDeductibleAmountBase = GLRoutines.Divide(AGiftDetail.TaxDeductibleAmount, ABatchExchangeRateToBase);
            AGiftDetail.NonDeductibleAmountBase = GLRoutines.Divide(AGiftDetail.NonDeductibleAmount, ABatchExchangeRateToBase);

            if (!AIsTransactionInIntlCurrency)
            {
                AGiftDetail.TaxDeductibleAmountIntl = (AIntlToBaseCurrencyExchRate == 0) ? 0 : GLRoutines.Divide(AGiftDetail.TaxDeductibleAmountBase,
                    AIntlToBaseCurrencyExchRate);
                AGiftDetail.NonDeductibleAmountIntl = (AIntlToBaseCurrencyExchRate == 0) ? 0 : GLRoutines.Divide(AGiftDetail.NonDeductibleAmountBase,
                    AIntlToBaseCurrencyExchRate);
            }
            else
            {
                AGiftDetail.TaxDeductibleAmountIntl = AGiftDetail.TaxDeductibleAmount;
                AGiftDetail.NonDeductibleAmountIntl = AGiftDetail.NonDeductibleAmount;
            }
        }
    }
}