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
                return -1;         // wrong lenght
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
    }
}