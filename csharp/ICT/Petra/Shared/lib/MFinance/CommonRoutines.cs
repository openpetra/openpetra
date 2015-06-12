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
using System.Text.RegularExpressions;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.CrossLedger.Data;

namespace Ict.Petra.Shared.MFinance
{
    /// <summary>
    /// Contains several functions which are specific to the Petra Finance Module.
    /// </summary>
    public class CommonRoutines
    {
        /// <summary>
        /// Checks whether the submitted BIC (Bank Identifier Code) is valid.
        ///
        /// The Bank Identifier Code is a unique address which, in telecommunication
        /// messages, identifies precisely the financial institutions involved in
        /// financial transactions. This identification method was recognised by the
        /// International Organisation for Standardisation (ISO).
        ///
        /// BICs either have 8 or 11 characters. The 11-character version specifes the
        /// "Branch code" in addition to the other information.
        /// Here is an example of an BIC: "BANKCCLL" or "BANKCCLLMAR".
        /// BANK = Bank Code. "BANK" identifies the bank, here "banque BNP-Paribas".
        /// This four-character code is called the Bank Code. It is unique
        /// to each financial institution and can only be made up of letters.
        /// CC   = Country Code. "CC" is the ISO country code for France.
        /// The country code identifies the country in which the financial
        /// institution is located and can only be made up of letters.
        /// LL   = Location Code. "LL" stands for Paris. It is the Location Code.
        /// This 2-character code may be alphabetical or numerical.
        /// The location code provides geographical distinction within a country,
        /// eg., cities, states, provinces and time zones.
        /// MAR  = Branch Code. "BNP-Paribas" has several branches throughout France.
        /// "MAR" identifies the "banque BNP-Paribas" branch in "Marseille",
        /// a city in the South of France.
        /// This 3-character code is called the Branch Code. It identifies a
        /// specific branch, or, for example, a department in a bank within the same
        /// country as the 8-character SWIFT BIC. This code may be alphabetical or
        /// numerical. *The Branch code is optional for SWIFT users.*
        /// Example from of the BIC code of London Branch of the HSBC Bank PLC
        /// (CITY OF LONDON CORPORATE OFFICE): MIDLGB2110C
        ///
        /// There is a web site where BIC codes can be checked:
        /// http://www.swift.com/biconline/
        ///
        /// </summary>
        /// <param name="ABic">String that should be checked</param>
        /// <returns>True if ABic is a valid BIC or an empty String or nil, False if it is
        /// not valid.
        /// </returns>
        public static bool CheckBIC(System.String ABic)
        {
            bool ReturnValue = false;
            bool mCharDigitOK = true;
            bool mLetterOK = true;

            System.Int16 mTmpCount;
            System.String mSubset1_6;

            // If the passed in Argument is null or empty then this is regarded as a valid BIC
            if ((ABic == null) || (ABic.Length == 0))
            {
                return true;
            }
            // Check if the passed in Argument has a length that qualifies it for a BIC check
            else if ((ABic.Length == 8) || (ABic.Length == 11))
            {
                #region Check whether the BIC is correctly built

                // Check whether the bic contains only letters and digits
                for (mTmpCount = 0; mTmpCount <= ABic.Length - 1; mTmpCount += 1)
                {
                    if (!System.Char.IsLetterOrDigit(ABic, mTmpCount))
                    {
                        mCharDigitOK = false;
                    }
                }

                // Check whether the first 6 characters are letters
                mSubset1_6 = ABic.Substring(0, 6);

                // messagebox.show('Substring bic: ' + mSubset1_6);
                for (mTmpCount = 0; mTmpCount <= mSubset1_6.Length - 1; mTmpCount += 1)
                {
                    if (!System.Char.IsLetter(mSubset1_6, mTmpCount))
                    {
                        mLetterOK = false;
                    }
                }

                // Summarize findings
                if ((mCharDigitOK == true)
                    && (mLetterOK == true))
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }

                #endregion
            }
            else
            {
                // Since the length of the Bic is incorrect we exit at once.
                return false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether the submitted IBAN (International Bank Account Number) is valid.
        ///
        /// The IBAN is an ISO standard (ISO 13616) designed to ensure that account details are given in a
        /// standard format (bank plus account). IBAN was introduced to provide an international standard
        /// for payee account details and to make details more easy to check. IBAN is a standardised
        /// international format for entering account details which consists of the country code (bank which
        /// maintains the account), the local bank code (eg. in the UK this is the sortcode), the (payee)
        /// account number and a two check digits. Please note that IBANs do not start with "IBAN " and do not
        /// contain spaces when stored in computer systems.
        /// Here is an example of an IBAN: DE89370400440532013000
        ///     DE = ISO 3166-1 country code
        ///         (NOTE: The country code used in an IBAN can deviate from the country code used in a BIC!
        ///     89 = two check digits
        ///     37040044 = sortcode
        ///     0532013000 = account number.
        /// IBANs are only issued by the bank where the account it is issued for is held.
        ///
        /// </summary>
        /// <param name="AIban">String that should be checked</param>
        /// <param name="AResult"></param>
        /// <returns>True if AIban is a valid Iban or an empty String or nil, False if it is
        /// not valid.
        /// </returns>
        public static bool CheckIBAN(System.String AIban, out TVerificationResult AResult)
        {
            AResult = null;

            int IbanLength;
            string IbanCountryCode;
            int IbanCheckDigits;
            int Index = -1;
            bool ReturnValue = true;

            // this list was up-to-date as of Jan 2014 (http://www.swift.com/dsp/resources/documents/IBAN_Registry.pdf)
            string[, ] COUNTRY_DATA =
            {
                { "AD", "Andorra", "24" },
                { "AE", "United Arab Emirates", "23" },
                { "AL", "Albania", "28" },
                { "AT", "Austria", "20" },
                { "AZ", "Azerbaijan", "28" },
                { "BA", "Bosnia and Herzegovina", "20" },
                { "BE", "Belgium", "16" },
                { "BG", "Bulgaria", "22" },
                { "BH", "Bahrain", "22" },
                { "BR", "Brazil", "29" },
                { "CH", "Switzerland", "21" },
                { "CR", "Costa Rica", "21" },
                { "CY", "Cyprus", "28" },
                { "CZ", "Czech Republic", "24" },
                { "DE", "Germany", "22" },
                { "DK", "Denmark", "18" },
                { "DO", "Dominican Republic", "28" },
                { "EE", "Estonia", "20" },
                { "ES", "Spain", "24" },
                { "FI", "Finland", "18" },
                { "FO", "Faroe Islands", "18" },
                { "FR", "France", "27" },
                { "GE", "Georgia", "22" },
                { "GI", "Gibraltar", "23" },
                { "GB", "United Kingdom", "22" },
                { "GL", "Greenland", "18" },
                { "GR", "Greece", "27" },
                { "GT", "Guatemala", "28" },
                { "HR", "Croatia", "21" },
                { "HU", "Hungary", "28" },
                { "IE", "Republic of Ireland", "22" },
                { "IL", "Israel", "23" },
                { "IS", "Iceland", "26" },
                { "IT", "Italy", "27" },
                { "JO", "Jordan", "30" },
                { "KU", "Kuwait", "30" },
                { "KZ", "Kazakhstan", "20" },
                { "LB", "Lebanon", "28" },
                { "LI", "Lichtenstein", "21" },
                { "LU", "Luxembourg", "20" },
                { "LV", "Latvia", "21" },
                { "LT", "Lithuania", "20" },
                { "MC", "Monaco", "27" },
                { "MD", "Moldova", "24" },
                { "ME", "Montenegro", "22" },
                { "MK", "Macedonia", "19" },
                { "MR", "Mauritania", "27" },
                { "MT", "Malta", "31" },
                { "MU", "Mauritius", "30" },
                { "NL", "The Netherlands", "18" },
                { "NO", "Norway", "15" },
                { "PK", "Pakistan", "24" },
                { "PL", "Poland", "28" },
                { "PS", "Palestine", "29" },
                { "PT", "Portugal", "25" },
                { "QA", "Qatar", "29" },
                { "RO", "Romania", "24" },
                { "RS", "Serbia", "22" },
                { "SA", "Saudi Arabia", "24" },
                { "SE", "Sweden", "24" },
                { "SI", "Slovenia", "19" },
                { "SK", "Slovak Republic", "24" },
                { "SM", "San Marino", "27" },
                { "TN", "Tunisia", "24" },
                { "TR", "Turkey", "26" },
                { "VG", "Virgin Islands", "24" }
            };

            // remove all spaces
            AIban = AIban.Replace(" ", "");

            // make string uppercase for ease
            AIban = AIban.ToUpper();

            // get length
            IbanLength = AIban.Length;

            // check length (must be less or equal to 34 characters)
            if (IbanLength > 34)
            {
                AResult = new TVerificationResult(
                    "CommonRoutines.CheckIBAN",
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_IBAN_TOO_LONG));

                return false;
            }

            // check first and second character to be A-Z
            if ((IbanLength < 2) || !Regex.IsMatch(AIban.Substring(0, 2), @"^[A-Z]+$"))
            {
                AResult = new TVerificationResult(
                    "CommonRoutines.CheckIBAN",
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_IBAN_NOTBEGINWITHTWOLETTERS));

                return false;
            }
            else
            {
                IbanCountryCode = AIban.Substring(0, 2);
            }

            // check third and fourth character to form a number
            if ((IbanLength < 4) || !Regex.IsMatch(AIban.Substring(2, 2), @"^[0-9]+$"))
            {
                AResult = new TVerificationResult(
                    "CommonRoutines.CheckIBAN",
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_IBAN_THIRDANDFORTHNOTDIGITS));

                return false;
            }
            else
            {
                IbanCheckDigits = Convert.ToInt32(AIban.Substring(2, 2));
            }

            // verify check digits (must be within range 02-98)
            if ((IbanCheckDigits < 2) || (IbanCheckDigits == 99))
            {
                AResult = new TVerificationResult(
                    "CommonRoutines.CheckIBAN",
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_IBAN_CHECKDIGITSAREWRONG));

                return false;
            }

            // find the index of the country code (if it exists)
            for (int i = 0; i < COUNTRY_DATA.GetLength(0); i++)
            {
                if (COUNTRY_DATA[i, 0] == IbanCountryCode)
                {
                    Index = i;
                    break;
                }
            }

            if (Index == -1)
            {
                AResult = new TVerificationResult(
                    "CommonRoutines.CheckIBAN",
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_IBAN_COUNTRYNOTDEFINIED, IbanCountryCode));

                ReturnValue = false;
            }

            // check the length of the IBAN for defined countries
            if ((Index != -1) && (Convert.ToInt32(COUNTRY_DATA[Index, 2]) != IbanLength))
            {
                AResult = new TVerificationResult(
                    "CommonRoutines.CheckIBAN",
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_IBAN_WRONGLENGTH, new string[4]
                        {
                            COUNTRY_DATA[Index, 1], COUNTRY_DATA[Index, 0], COUNTRY_DATA[Index, 2].ToString(), IbanLength.ToString()
                        }));

                return false;
            }

            /* calculate and check the checksum */

            // put country and check digits to the end
            AIban = AIban.Substring(4) + AIban.Substring(0, 4);

            //replace each letter by numerical equivalent
            AIban = Regex.Replace(AIban, @"\D", x => ((int)x.Value[0] - 55).ToString());

            int Remainder = 0;

            // Interpret the IBAN as a decimal integer and compute the remainder of that number on division by 97
            while (AIban.Length >= 7)
            {
                Remainder = Convert.ToInt32(AIban.Substring(0, 7)) % 97;
                AIban = Remainder.ToString() + AIban.Substring(7);
            }

            if (AIban.Length > 0)
            {
                Remainder = Convert.ToInt32(AIban) % 97;
            }

            // checksum only valid if Remainder = 1
            if (Remainder != 1)
            {
                AResult = new TVerificationResult(
                    "CommonRoutines.CheckIBAN",
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_IBAN_CHECKSUMMISMATCH));

                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether the given account number is NOT an IBAN.
        ///
        /// This check is derived from a statement found in the 'EUROPEAN COMMITTEE FOR
        /// BANKING STANDARDS' (ECBS) document 'IBAN: STANDARD IMPLEMENTATION GUIDELINES'
        /// (SIG203 V3.2 - AUGUST 2003), Section 8.2., which says:
        /// "The presence of an IBAN can be detected from the two starting alpha
        /// characters which signify the ISO country code followed by two numeric digits
        /// which signify the IBAN check digits. *There are no domestic account numbers
        /// known which start with two alpha characters followed by two numeric digits*."
        ///
        /// </summary>
        /// <param name="AAccountNumber">String that should be checked.
        /// </param>
        /// <returns>True if the submitted account number does not start with two letters
        /// followed by two digits. If the submitted account number does start with two
        /// letters followed by two digits then it returns False.</returns>
        public static bool CheckAccountNumberIsNotIBAN(System.String AAccountNumber)
        {
            if ((System.Char.IsLetter(AAccountNumber[0]))
                && (System.Char.IsLetter(AAccountNumber[1]))
                && (System.Char.IsDigit(AAccountNumber[2]))
                && (System.Char.IsDigit(AAccountNumber[3])))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks the validity of a bank account number.
        /// This function checks the validity of a bank account number by performing a
        /// country-specific check on the submitted account number, if a check rountine for
        /// that country exists.
        /// </summary>
        /// <param name="AAccountNumber">Account number</param>
        /// <param name="ABankCountryCode">Country code of the bank</param>
        /// <returns> -1 = length check failed.
        ///            0 = invalid account number
        ///            1 = valid account number
        ///            2 = probably valid - account number cannot be validated by country-specific check
        ///            3 = account number could not be validated - no country-specific check implemented
        ///            4 = Bank partner could not be found
        /// </returns>
        public int CheckAccountNumber(String AAccountNumber, String ABankCountryCode)
        {
            // perform bank account number validation depending on the bank's country
            if (ABankCountryCode == "NL")
            {
                return CheckAccountNumber_NL(AAccountNumber);
            }
            else
            {
                return 3;
            }
        }

        /// <summary>
        /// Checks a Dutch (NL) account number ("Rekeningnummer") for validity.
        /// Explanation:  (1) Excludes "Postbank" account numbers (Postgiro-accounts) since
        ///                there is no way to verify them.
        ///            (2) Checks the length of the account number.
        ///            (3) Runs a checksum algorithm to check the Dutch bank account numbers
        ///                (modulo 11 proof).
        ///                Example with bank account number "123456789":
        ///                  1st digit bank-account-number = 1 9 * 1 =  9
        ///                  2nd digit bank-account-number = 2 8 * 2 = 16
        ///                  3rd digit bank-account-number = 3 7 * 3 = 21
        ///                  4th digit bank-account-number = 4 6 * 4 = 24
        ///                  5th digit bank-account-number = 5 5 * 5 = 25
        ///                  6th digit bank-account-number = 6 4 * 6 = 24
        ///                  7th digit bank-account-number = 7 3 * 7 = 21
        ///                  8th digit bank-account-number = 8 2 * 8 = 16
        ///                  9th digit bank-account-number = 9 1 * 9 =  9
        ///                                                 added up: 165
        ///
        ///                  Because 165 is devidable by 11, chances are big
        ///                  that the bank account number "123456789" is correct
        ///                  (165/11=15 rest 0).
        ///
        ///            NOTE: "Postbank" account numbers (Postgiro-accounts) cannot be validated!
        ///                  In this case the return value is "2".
        ///
        /// </summary>
        /// <param name="AAcountNumber">Account number</param>
        /// <returns> -1 = length check failed.
        ///            0 = invalid account number
        ///            1 = valid account number
        ///            2 = probably valid - account number cannot be validated
        ///                (is a "Postbank" account number [a Postgiro-account])
        /// </returns>
        private int CheckAccountNumber_NL(String AAcountNumber)
        {
            AAcountNumber = PruneAccountNumber(AAcountNumber);

            if (AAcountNumber.StartsWith("P"))
            {
                // All such numbers must be 7 characters or less
                if (AAcountNumber.Length <= 8)
                {
                    // no way to verify Dutch "Postbank"
                    return 2;
                }
                else
                {
                    return -1;
                }
            }
            else if (AAcountNumber.StartsWith("000"))
            {
                if (AAcountNumber.Length == 10)
                {
                    return 2;
                }
                else
                {
                    return -1;
                }
            }
            //All Dutch bank account numbers that are not "Postbank" numbers must be either 9 digits,
            // or 10 digits where the first digit needs to be 0 (this zero is a fill character)
            else if ((AAcountNumber.Length != 9)
                     && (AAcountNumber.Length != 10))
            {
                return -1;         // wrong length
            }

            if ((AAcountNumber.Length == 10)
                && (!AAcountNumber.StartsWith("0")))
            {
                return 0;
            }

            int CheckSum = ComputeChecksum(AAcountNumber);

            // compute the check sum
            if ((CheckSum % 11) == 0)
            {
                return 1;
            }

            return 0;
        }

        private int ComputeChecksum(String AAcountNumber)
        {
            int CheckSum = 0;
            int Multiplier = 1;

            for (int Counter = AAcountNumber.Length - 1; Counter >= 0; --Counter)
            {
                int tmp = AAcountNumber[Counter];

                if ((tmp < 0X30)
                    || (tmp > 0X39))
                {
                    CheckSum = -1;
                    break;
                }

                CheckSum += (tmp - 0X30) * Multiplier++;
            }

            return CheckSum;
        }

        /// <summary>
        /// Removes all characters that are not 0...9 and A...Z from the submitted account number.
        /// This function is used to be able to compare account numbers that are entered in
        /// a "fancy" style, eg. "1234-567.89" against ones that are entered in a "plain" style,
        /// eg. "123456789". This function always returns the plain stlye, no matter if the
        /// submitted account number is "fancy" style or not.
        /// </summary>
        /// <param name="AAcountNumber"></param>
        /// <returns>AccountNumber in plain style</returns>
        private String PruneAccountNumber(String AAcountNumber)
        {
            System.Text.StringBuilder SB = new System.Text.StringBuilder();

            for (int Counter = 0; Counter < AAcountNumber.Length; ++Counter)
            {
                Char CurrentChar = AAcountNumber[Counter];

                if (((CurrentChar >= 'a') && (CurrentChar <= 'z'))
                    || ((CurrentChar >= 'A') && (CurrentChar <= 'Z'))
                    || ((CurrentChar >= '0') && (CurrentChar <= '9')))
                {
                    SB.Append(CurrentChar);
                }
            }

            return SB.ToString();
        }

        /// <summary>
        /// Gets the best (latest) exchange rate for a pair of currencies from a table of rates.
        /// </summary>
        /// <param name="ADailyRateTable">A table of rates that has been obtained by calling LoadExchangeRateData for a period of interest.</param>
        /// <param name="ACurrencyFrom">The 'from' currency</param>
        /// <param name="ACurrencyTo">The 'to' currency</param>
        /// <param name="AEnforceUniqueRate">If true the method will only return true if there is a unique rate for the specified currencies on the 'latest' date.
        /// If false the method returns the rate for the latest 'time' on the latest date.</param>
        /// <param name="ARateOfExchange">The returned rate</param>
        /// <param name="AEffectiveDate">The date associated with the returned rate.</param>
        /// <returns>True if a matching rate was found.  The rate and date are in the out parameters.</returns>
        public static bool GetBestExchangeRate(ExchangeRateTDSADailyExchangeRateTable ADailyRateTable,
            string ACurrencyFrom,
            string ACurrencyTo,
            Boolean AEnforceUniqueRate,
            out decimal ARateOfExchange,
            out DateTime AEffectiveDate)
        {
            ARateOfExchange = 0.0m;
            AEffectiveDate = DateTime.MinValue;

            ADailyExchangeRateRow uniqueFittingRow = null;
            bool oppositeRate = false;

            // sort rates by date, look for rate just before the date we are looking for
            string rowFilter = String.Format("{0}='{1}' AND {2}='{3}'",
                ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                ACurrencyFrom,
                ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                ACurrencyTo);

            ADailyRateTable.DefaultView.RowFilter = rowFilter;
            ADailyRateTable.DefaultView.Sort = String.Format("{0} DESC, {1} DESC",
                ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                ADailyExchangeRateTable.GetTimeEffectiveFromDBName());

            // If we are after a unique row we need to remember the one from this view, if it exists
            if (AEnforceUniqueRate && (ADailyRateTable.DefaultView.Count == 1))
            {
                uniqueFittingRow = (ADailyExchangeRateRow)ADailyRateTable.DefaultView[0].Row;
            }

            if ((ADailyRateTable.DefaultView.Count == 0) || AEnforceUniqueRate)
            {
                // try other way round
                rowFilter = String.Format("{0}='{1}' AND {2}='{3}'",
                    ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                    ACurrencyFrom,
                    ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                    ACurrencyTo);

                ADailyRateTable.DefaultView.RowFilter = rowFilter;
                oppositeRate = true;

                if (AEnforceUniqueRate)
                {
                    if (ADailyRateTable.DefaultView.Count > 1)
                    {
                        // This way round does not have a unique rate
                        uniqueFittingRow = null;
                    }
                    else if ((ADailyRateTable.DefaultView.Count == 1) && (uniqueFittingRow == null))
                    {
                        // we will use this as our unique rate
                        uniqueFittingRow = (ADailyExchangeRateRow)ADailyRateTable.DefaultView[0].Row;
                    }
                    else
                    {
                        // put this variable back as it was
                        oppositeRate = false;
                    }
                }
            }

            ADailyExchangeRateRow fittingRate = null;

            if (AEnforceUniqueRate)
            {
                // Did we get a unique rate?
                if (uniqueFittingRow != null)
                {
                    fittingRate = uniqueFittingRow;
                }
            }
            else if (ADailyRateTable.DefaultView.Count > 0)
            {
                // Just return the first in the list
                fittingRate = (ADailyExchangeRateRow)ADailyRateTable.DefaultView[0].Row;
            }

            if (fittingRate != null)
            {
                if (oppositeRate)
                {
                    ARateOfExchange = GLRoutines.Divide(1.0m, fittingRate.RateOfExchange);
                }
                else
                {
                    ARateOfExchange = fittingRate.RateOfExchange;
                }

                AEffectiveDate = fittingRate.DateEffectiveFrom;
            }

            //Returning 0 causes a validation error to force the user to select an exchange rate:
            return ARateOfExchange != 0.0m;
        }
    }
}