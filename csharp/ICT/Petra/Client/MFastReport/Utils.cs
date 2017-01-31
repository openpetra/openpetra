//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Moray
//
// Copyright 2004-2013 by OM International
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
using System.Globalization;

namespace Ict.Petra.Client.MFastReport
{
    /// <summary>
    /// Utility class for FastReport
    /// </summary>
    public class IctUtils
    {
        private NumberFormatInfo FCurrencyFormat = null;

        /// <summary>Partner keys always have 10 digits.</summary>
        protected String PartnerKey(Int64 PartnerKey)
        {
            return PartnerKey.ToString("0000000000");
        }

        /// <summary>
        /// OM International date format.
        /// </summary>
        /// <param name="fld"></param>
        /// <returns></returns>
        protected String OmDate(DateTime fld)
        {
            return fld.ToString("dd-MMM-yyyy");
        }

        /// <summary>
        ///  Get the standard report currency format: no currency symbol.
        /// </summary>
        /// <example>[[Donors.totalamount].ToString("C", CurrencyFormat)]</example>
        protected NumberFormatInfo CurrencyFormat
        {
            get
            {
                if (FCurrencyFormat == null)
                {
                    FCurrencyFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
                    FCurrencyFormat.CurrencySymbol = "";
                }

                return FCurrencyFormat;
            }
        }
    }
}