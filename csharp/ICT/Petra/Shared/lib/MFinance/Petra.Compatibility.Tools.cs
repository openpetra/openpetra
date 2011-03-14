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
using System.Text.RegularExpressions;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;

namespace Ict.Petra.Shared.MFinance
{
    /// <summary>
    /// Get currency info is intended to be used to get some some specific infos
    /// using the old petra data base entries. GetCurrencyInfon is designed to get
    /// a rough set of information which shall be used for foreign currency
    /// calculations and presentations. In normal cases open petra uses the
    /// user defined localisation ie the presentation and rounding rules.
    /// But if you have to work with JPY (Japanese Yen) you have to know that
    /// you have to round to 0 digits even if your user settings have selected the
    /// USD and two digits rounding.
    ///
    /// The routine works error regressive that means that invalid data (currency codes)
    /// and damaged format strings will result in 2 digit rounding as a default.
    /// </summary>
    public class GetCurrencyInfo
    {
        private ACurrencyTable currencyTable = null;
        private ACurrencyRow currencyRow = null;

        /// <summary>
        /// Constructor which automatically loads one CurrencyTable Entry defined
        /// by the parameter.
        /// </summary>
        /// <param name="ACurrencyCode">Three digit description to define the
        /// currency.</param>
        public GetCurrencyInfo(string ACurrencyCode)
        {
            currencyTable = ACurrencyAccess.LoadByPrimaryKey(ACurrencyCode, null);

            if (currencyTable.Rows.Count == 1)
            {
                currencyRow = (ACurrencyRow)currencyTable[0];
            }
            else
            {
                throw new GetCurrencyInfoException(
                    "GetCurrencyInfo-Constructor: currencyTable.Rows.Count = " +
                    currencyTable.Rows.Count.ToString());
            }
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
                return new FormatConverter(currencyRow.DisplayFormat).digits;
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
    class FormatConverter
    {
        string sRegex;
        Regex reg;
        MatchCollection matchCollection;
        int intDigits;
        public FormatConverter(string strFormat)
        {
            sRegex = ">9.(9)+|>9$";
            reg = new Regex(sRegex);
            matchCollection = reg.Matches(strFormat);

            if (matchCollection.Count != 1)
            {
                throw new GetCurrencyInfoException(
                    String.Format("The regular expression {0} does not fit for a match in {1}",
                        sRegex, strFormat));
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

    /// <summary>
    /// This exception shall be thrown if the value of a_currency:a_display_format_c
    /// is changed and cannot be used to calculate the number of currency digits anymore.
    /// </summary>
    public class GetCurrencyInfoException : System.Exception
    {
        public GetCurrencyInfoException()
        {
        }

        public GetCurrencyInfoException(string message) : base(message)
        {
        }
    }
}