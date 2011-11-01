//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Christophert
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

namespace Ict.Common
{
    /// <summary>
    /// Helper class for all things numeric
    /// </summary>
    public class THelperNumeric
    {
        /// <summary>
        ///  Returns the number of decimal places
        ///  in a OpenPetra numeric format. Relates to x_dp.p
        /// </summary>
        /// <param name="ANumericFormat">The numeric format to parse</param>
        /// <returns>Integer number of decimal places</returns>
        public static int CalcNumericFormatDecimalPlaces(string ANumericFormat)
        {
            int DecimalPos = ANumericFormat.IndexOf(".");

            if ((DecimalPos < 0) || (DecimalPos == (ANumericFormat.Length - 1)))
            {
                return 0;
            }
            else
            {
                return ANumericFormat.Substring(DecimalPos + 1).Length;
            }
        }
    }
}