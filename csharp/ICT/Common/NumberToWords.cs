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
using System.Globalization;

namespace Ict.Common
{
    /// <summary>
    /// This is to write a currency value in words, eg. 123 becomes onehundred twenty-three;
    /// this has to be implemented for each language.
    /// see also Java GPL code: NumericalChameleon: http://www.jonelo.de/java/nc/
    /// </summary>
    public class NumberToWords
    {
        private static string[] SingleDigitsDE =
        {
            "null", "ein", "zwei", "drei", "vier", "fünf", "sechs", "sieben",
            "acht", "neun"
        };
        private static string[] TenTo19DE =
        {
            "zehn", "elf", "zwölf", "dreizehn", "vierzehn", "fünfzehn", "sechzehn",
            "siebzehn", "achtzehn", "neunzehn"
        };
        private static string[] MultiplesOf10DE =
        {
            "zwanzig", "dreißig", "vierzig", "fünfzig", "sechzig", "siebzig",
            "achtzig", "neunzig"
        };

        private static string AmountToWordsInternalDE(Int64 IntValue)
        {
            string Result = "";

            if (IntValue < 0)
            {
                IntValue = Math.Abs(IntValue);
                Result = "minus";
            }

            if (IntValue < 10)
            {
                Result = SingleDigitsDE[IntValue];
            }
            else if (IntValue < 20)
            {
                Result = TenTo19DE[IntValue - 10];
            }
            else
            {
                // TODO millions?
                if (IntValue >= 1000)
                {
                    Result += AmountToWordsInternalDE(IntValue / 1000) + "tausend";
                    IntValue = IntValue % 1000;
                }

                if (IntValue >= 100)
                {
                    Result += AmountToWordsInternalDE(IntValue / 100) + "hundert";
                    IntValue = IntValue % 100;
                }

                if (IntValue >= 20)
                {
                    if (IntValue % 10 > 0)
                    {
                        Result += AmountToWordsInternalDE(IntValue % 10) + "und";
                    }

                    Result += MultiplesOf10DE[IntValue / 10 - 2];
                }
                else if (IntValue == 1)
                {
                    Result += "eins";
                }
                else if (IntValue > 0)
                {
                    Result += AmountToWordsInternalDE(IntValue);
                }
            }

            return Result;
        }

        private static string[] SingleDigitsUK =
        {
            "zero", "one", "two", "three", "four", "five", "six", "seven",
            "eight", "nine"
        };
        private static string[] TenTo19UK =
        {
            "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen",
            "seventeen", "eighteen", "nineteen"
        };
        private static string[] MultiplesOf10UK =
        {
            "twenty", "thirty", "fourty", "fifty", "sixty", "seventy",
            "eighty", "ninety"
        };

        private static string AmountToWordsInternalUK(Decimal AValue)
        {
            string Result = "";

            Int64 IntValue = Convert.ToInt64(Math.Floor(AValue));

            if (IntValue < 0)
            {
                IntValue = Math.Abs(IntValue);
                Result = "minus";
            }

            if (IntValue < 10)
            {
                Result += SingleDigitsUK[IntValue];
            }
            else if (IntValue < 20)
            {
                Result += TenTo19UK[IntValue - 10];
            }
            else
            {
                // TODO millions?
                if (IntValue >= 1000)
                {
                    Result += AmountToWordsInternalUK(IntValue / 1000) + " thousand";
                    IntValue = IntValue % 1000;

                    if (IntValue > 0)
                    {
                        Result += " ";
                    }
                }

                if (IntValue >= 100)
                {
                    Result += AmountToWordsInternalUK(IntValue / 100) + " hundred";
                    IntValue = IntValue % 100;

                    if (IntValue > 0)
                    {
                        Result += " and ";
                    }
                }

                if (IntValue >= 20)
                {
                    Result += MultiplesOf10UK[IntValue / 10 - 2];

                    if (IntValue % 10 > 0)
                    {
                        Result += "-" + AmountToWordsInternalUK(IntValue % 10);
                    }
                }
                else if (IntValue > 0)
                {
                    Result += AmountToWordsInternalUK(IntValue);
                }
            }

            return Result;
        }

        /// <summary>
        /// Convert currency amount to words.
        /// This uses the current culture to determine the language.
        /// </summary>
        /// <param name="AValue">Amount as a decimal</param>
        /// <param name="AMajorUnitSingular"></param>
        /// <param name="AMajorUnitPlural"></param>
        /// <param name="AMinorUnitSingular"></param>
        /// <param name="AMinorUnitPlural"></param>
        /// <returns></returns>
        public static string AmountToWords(decimal AValue, string AMajorUnitSingular, string AMajorUnitPlural, string AMinorUnitSingular, string AMinorUnitPlural)
        {
            Int64 IntValue = Convert.ToInt64(Math.Floor(AValue));
            Int32 Decimals = Convert.ToInt32(Math.Floor(AValue * 100)) % 100;

            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de")
            {
                string Result = AmountToWordsInternalDE(IntValue);
                Result = Result.Substring(0, 1).ToUpper() + Result.Substring(1);

                if (AMajorUnitSingular.Length > 0 && IntValue == 1)
                {
                    Result += " " + AMajorUnitSingular;
                }
                else if (AMajorUnitPlural.Length > 0 && IntValue != 1)
                {
                    Result += " " + AMajorUnitPlural;
                }

                if (Decimals > 0)
                {
                    string DecimalResult = AmountToWordsInternalDE(Decimals);
                    DecimalResult = DecimalResult.Substring(0, 1).ToUpper() + DecimalResult.Substring(1);
                    Result += " " + DecimalResult;

                    if (AMinorUnitSingular.Length > 0 && Decimals == 1)
                    {
                        Result += " " + AMinorUnitSingular;
                    }
                    else if (AMinorUnitPlural.Length > 0 && Decimals != 1)
                    {
                        Result += " " + AMinorUnitPlural;
                    }
                }

                return Result;
            }
            else
            {
                string Result = AmountToWordsInternalUK(IntValue);

                if (AMajorUnitSingular.Length > 0 && IntValue == 1)
                {
                    Result += " " + AMajorUnitSingular;
                }
                else if (AMajorUnitPlural.Length > 0 && IntValue != 1)
                {
                    Result += " " + AMajorUnitPlural;
                }

                if (Decimals > 0)
                {
                    string DecimalResult = AmountToWordsInternalUK(Decimals);
                    Result += " " + DecimalResult;

                    if (AMinorUnitSingular.Length > 0 && Decimals == 1)
                    {
                        Result += " " + AMinorUnitSingular;
                    }
                    else if (AMinorUnitPlural.Length > 0 && Decimals != 1)
                    {
                        Result += " " + AMinorUnitPlural;
                    }
                }

                return Result;
            }
        }
    }
}