//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
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

using System;
using System.Data;
using System.Data.Odbc;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// Base on the idea to reduce the number of database request to it's minimum, this object reads
    /// the complete a_currency table. Two currency slots are provided, a base currency slot and a foreigen
    /// currency slot. The the base currency slot can only set in one of the constructors one time and the
    /// foreign currency slot can easily be switched to an other currency by using the
    /// ForeignCurrencyCode property without and any more database request.
    ///
    /// The petra table a_currency contains a "hidden information" about the number of digits and this
    /// value will be used either to calculate the number of value dependet rounding digits.
    /// Furthermore there are some servic routines base on this information.
    /// </summary>

    public class TCurrencyInfo
    {
        private ACurrencyTable currencyTable = null;
        private ACurrencyRow baseCurrencyRow = null;
        private ACurrencyRow foreignCurrencyRow = null;
        private int intBaseCurrencyDigits;
        private int intForeignCurrencyDigits;
        private const int DIGIT_INIT_VALUE = -1;

        /// <summary>
        /// Constructor which automatically loads the table and sets the value of the
        /// currency table.
        /// </summary>
        /// <param name="ACurrencyCode">Three digit description to define the
        /// base currency.</param>
        public TCurrencyInfo(string ACurrencyCode)
        {
            LoadDatabase();
            baseCurrencyRow = SetRowToCode(ACurrencyCode);
        }

        /// <summary>
        ///
        /// </summary>
        public TCurrencyInfo(string ACurrencyCode, TDBTransaction AMasterTransaction)
        {
            LoadDatabase();
            baseCurrencyRow = SetRowToCode(ACurrencyCode);
        }

        /// <summary>
        /// Constructor which automatically loads the table and sets the value of
        /// the base currency table and foreign currency table.
        /// </summary>
        /// <param name="ABaseCurrencyCode">Base currency code</param>
        /// <param name="AForeignCurrencyCode">foreign Currency Code</param>
        public TCurrencyInfo(string ABaseCurrencyCode, string AForeignCurrencyCode)
        {
            LoadDatabase();
            baseCurrencyRow = SetRowToCode(ABaseCurrencyCode);
            foreignCurrencyRow = SetRowToCode(AForeignCurrencyCode);
        }

        private void LoadDatabase()
        {
            intBaseCurrencyDigits = DIGIT_INIT_VALUE;
            intForeignCurrencyDigits = DIGIT_INIT_VALUE;

            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            currencyTable = ACurrencyAccess.LoadAll(transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            if (currencyTable.Rows.Count == 0)
            {
                TVerificationException terminate = new TVerificationException(
                    Catalog.GetString("The table a_currency is empty!"));
                terminate.Context = "Common Accountig";
                terminate.ErrorCode = "TCurrencyInfo01";
                throw terminate;
            }
        }

        private ACurrencyRow SetRowToCode(string ACurrencyCode)
        {
            if (ACurrencyCode.Equals(String.Empty))
            {
                return null;
            }

            for (int i = 0; i < currencyTable.Rows.Count; ++i)
            {
                if (ACurrencyCode.Equals(((ACurrencyRow)currencyTable[i]).CurrencyCode))
                {
                    return (ACurrencyRow)currencyTable[i];
                }
            }

            TVerificationException terminate = new TVerificationException(
                Catalog.GetString(String.Format(
                        "No Data for curency {0} found", ACurrencyCode)));
            terminate.Context = "Common Accountig";
            terminate.ErrorCode = "TCurrencyInfo02";
            throw terminate;
        }

        /// <summary>
        /// Property to read the base currency code value.
        /// </summary>
        public string CurrencyCode
        {
            get
            {
                return baseCurrencyRow.CurrencyCode;
            }
        }

        /// <summary>
        /// Property to read and to set the foreign currency code value.
        /// </summary>
        public string ForeignCurrencyCode
        {
            get
            {
                return foreignCurrencyRow.CurrencyCode;
            }
            set
            {
                intForeignCurrencyDigits = DIGIT_INIT_VALUE;
                foreignCurrencyRow = SetRowToCode(value);
            }
        }

        /// <summary>
        /// This rotine handles a correct roundig by using the number of digits in a_currency
        /// </summary>
        /// <param name="AValueToRound"></param>
        /// <returns></returns>
        public decimal RoundBaseCurrencyValue(decimal AValueToRound)
        {
            return Math.Round(AValueToRound, digits);
        }

        /// <summary>
        /// This rotine handles a correct roundig by using the number of digits in a_currency
        /// </summary>
        /// <param name="AValueToRound"></param>
        /// <returns></returns>
        public decimal RoundForeignCurrencyValue(decimal AValueToRound)
        {
            return Math.Round(AValueToRound, foreignDigits);
        }

        /// <summary>
        /// Here you can calculate the base value amount by defining the parameters below.
        /// Because of the rounding the ToBaseValue(ToForeignValue(someValue)) != someValue
        /// </summary>
        /// <param name="AForeignValue"></param>
        /// <param name="AExchangeRate"></param>
        /// <returns></returns>
        public decimal ToBaseValue(decimal AForeignValue, decimal AExchangeRate)
        {
            return RoundBaseCurrencyValue(AForeignValue * AExchangeRate);
        }

        /// <summary>
        /// Here you can calculate the base value amount by defining the parameters below.
        /// Because of the rounding the ToBaseValue(ToForeignValue(someValue)) != someValue
        /// </summary>
        /// <param name="ABaseValue"></param>
        /// <param name="AExchangeRate"></param>
        /// <returns></returns>
        public decimal ToForeignValue(decimal ABaseValue, decimal AExchangeRate)
        {
            return RoundForeignCurrencyValue(ABaseValue / AExchangeRate);
        }

        /// <summary>
        /// Calculates the number of digits by reading the row.DisplayFormat
        /// Entry of the currency table and convert the old petra string to an
        /// integer response.
        /// </summary>
        public int digits
        {
            get
            {
                if (intBaseCurrencyDigits == DIGIT_INIT_VALUE)
                {
                    intBaseCurrencyDigits = new FormatConverter(baseCurrencyRow.DisplayFormat).digits;
                }

                return intBaseCurrencyDigits;
            }
        }

        /// <summary>
        /// Calculates the number of digits for the foreign currency.
        /// </summary>
        public int foreignDigits
        {
            get
            {
                if (intForeignCurrencyDigits == DIGIT_INIT_VALUE)
                {
                    intForeignCurrencyDigits = new FormatConverter(foreignCurrencyRow.DisplayFormat).digits;
                }

                return intForeignCurrencyDigits;
            }
        }
    }

    /// <summary>
    /// This class is a local Format converter <br />
    ///  Console.WriteLine(new FormatConverter("->>>,>>>,>>>,>>9.99").digits.ToString());<br />
    ///  Console.WriteLine(new FormatConverter("->>>,>>>,>>>,>>9.9").digits.ToString());<br />
    ///  Console.WriteLine(new FormatConverter("->>>,>>>,>>>,>>9").digits.ToString());<br />
    /// The result is 2,1 and 0 digits ..
    /// </summary>
    public class FormatConverter
    {
        string sRegex;
        Regex reg;
        MatchCollection matchCollection;
        int intDigits;

        /// <summary>
        ///
        /// </summary>
        public FormatConverter(string strFormat)
        {
            sRegex = ">9.(9)+|>9$";
            reg = new Regex(sRegex);
            matchCollection = reg.Matches(strFormat);

            if (matchCollection.Count != 1)
            {
                TVerificationException terminate = new TVerificationException(
                    String.Format(Catalog.GetString("The regular expression {0} does not fit for a match in {1}"),
                        sRegex, strFormat));

                terminate.Context = "Common Accountig";
                terminate.ErrorCode = "TCurrencyInfo03";
                throw terminate;
            }

            intDigits = (matchCollection[0].Value).Length - 3;

            if (intDigits == -1)
            {
                intDigits = 0;
            }

            if (intDigits < -1)
            {
                intDigits = 2;
            }
        }

        /// <summary>
        /// Property to report the number of digits
        /// </summary>
        public int digits
        {
            get
            {
                return intDigits;
            }
        }
    }
}