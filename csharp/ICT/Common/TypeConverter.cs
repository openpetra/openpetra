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
using System.ComponentModel;
using System.Globalization;

namespace Ict.Common
{
    /// <summary>
    /// Contains Type-converting classes that inherit from and follow the model of System.ComponentModel.TypeConverter.
    /// </summary>
    public class TypeConverter
    {
        #region TDateConverter

        /// <summary>
        /// Converts a date into a common international data format, independent of a computer's date formatting settings.
        /// </summary>
        public class TDateConverter : System.ComponentModel.TypeConverter
        {
            public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override Boolean CanConvertTo(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

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

            public TDateConverter() : base()
            {
            }
        }

        #endregion


        #region TBooleanToYesNoConverter

        /// <summary>
        /// Converts 'true' and 'false' values to 'Yes' and 'No' values.
        /// </summary>
        /// <remarks>TODO: In need of I8N support!</remarks>
        public class TBooleanToYesNoConverter : System.ComponentModel.TypeConverter
        {
            public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override Boolean CanConvertTo(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value == typeof(string))
                {
                    return value;
                }

                return null;
            }

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

            public TBooleanToYesNoConverter() : base()
            {
            }
        }

        #endregion
    }
}