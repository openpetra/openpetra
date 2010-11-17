//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timh
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
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace Ict.Petra.Shared
{
    /// <summary>
    /// the date converter allows short strings to be converted to DateTime objects
    /// </summary>
    public class TPetraDateConverter : System.ComponentModel.TypeConverter
    {
        /// <summary>
        /// we can convert from string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <summary>
        /// we can convert to string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override Boolean CanConvertTo(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <summary>
        /// convert from string to DateTime
        /// equal sign is short for today, +/- will add or substract given number of days
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>�
        /// <param name="value"></param>
        /// <returns>DateTime object</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                if (value.ToString() == "=")
                {
                    return DateTime.Today;
                }

                if ((value.ToString().StartsWith("+")) || (value.ToString().StartsWith("-")))
                {
                    return DateTime.Today.AddDays(Convert.ToDouble(value));
                }

                if (value != null)
                {
                    return DateTime.Parse((string)value);
                }
            }

            return null;
        }

        /// <summary>
        /// convert DateTime to string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((destinationType == typeof(string)) && (value != null))
            {
                if (((DateTime)value).CompareTo(DateTime.MinValue) == 0)
                {
                    return "";
                }
                else
                {
                    return ((DateTime)value).ToString("dd-MMM-yyyy").ToUpper();
                }
            }

            return null;
        }
    }


    /// <summary>
    /// convert boolean to string and vice versa
    /// </summary>
    public class TPetraBooleanToYesNoConverter : System.ComponentModel.TypeConverter
    {
        /// <summary>
        /// can convert from string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <summary>
        /// can convert to string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override Boolean CanConvertTo(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <summary>
        /// convert string to boolean
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == typeof(string))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// convert from boolean to string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value.GetType() == typeof(Boolean))
            {
                if (value.ToString().ToLower() == "true")
                {
                    return "Yes";
                }

                if (value.ToString().ToLower() == "false")
                {
                    return "No";
                }
            }

            return null;
        }
    }
}