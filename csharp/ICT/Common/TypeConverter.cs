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
            /// <summary>
            /// Test if we can convert from a given type to Date
            /// </summary>
            /// <param name="context"></param>
            /// <param name="sourceType"></param>
            /// <returns></returns>
            public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            /// <summary>
            /// Test if we can convert the date to a given type
            /// </summary>
            /// <param name="context"></param>
            /// <param name="sourceType"></param>
            /// <returns></returns>
            public override Boolean CanConvertTo(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            /// <summary>
            /// convert an object into a date
            /// </summary>
            /// <param name="context"></param>
            /// <param name="culture"></param>
            /// <param name="value"></param>
            /// <returns></returns>
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
            /// convert a date into another type
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

            /// <summary>
            /// default constructor
            /// </summary>
            public TDateConverter() : base()
            {
            }
        }

        #endregion

        #region TTimeConverter

        /// <summary>
        /// Converts a time (in seconds or HH:MM:SS) to a short time string (HH:MM)
        /// </summary>
        public class TShortTimeConverter : System.ComponentModel.TypeConverter
        {
            /// <summary>
            /// Returns whether this converter can convert an object of the given type to the type of this converter (HH:MM:SS string).
            /// </summary>
            /// <param name="context"></param>
            /// <param name="sourceType"></param>
            /// <returns></returns>
            public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return TTimeConverterInternal.CanConvertTo(sourceType);
            }

            /// <summary>
            /// Returns whether this converter can convert the object to the specified type.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="destinationType"></param>
            /// <returns></returns>
            public override Boolean CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return TTimeConverterInternal.CanConvertTo(destinationType);
            }

            /// <summary>
            /// Converts the given object to the type of this converter (HH:MM string).
            /// </summary>
            /// <param name="context"></param>
            /// <param name="culture"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                return TTimeConverterInternal.ConvertFrom(context, culture, value, false);
            }

            /// <summary>
            /// Converts the given value object to the specified type.  This is the method that the grid calls to display the times
            /// </summary>
            /// <param name="context"></param>
            /// <param name="culture"></param>
            /// <param name="value"></param>
            /// <param name="destinationType"></param>
            /// <returns></returns>
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                return TTimeConverterInternal.ConvertTo(context, culture, value, destinationType, false);
            }

            /// <summary>
            /// default constructor
            /// </summary>
            public TShortTimeConverter()
                : base()
            {
            }
        }

        /// <summary>
        /// Converts a time (in seconds or HH:MM) to a long time string (HH:MM:SS)
        /// </summary>
        public class TLongTimeConverter : System.ComponentModel.TypeConverter
        {
            /// <summary>
            /// Returns whether this converter can convert an object of the given type to the type of this converter (HH:MM:SS string).
            /// </summary>
            /// <param name="context"></param>
            /// <param name="sourceType"></param>
            /// <returns></returns>
            public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return TTimeConverterInternal.CanConvertTo(sourceType);
            }

            /// <summary>
            /// Returns whether this converter can convert the object to the specified type.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="destinationType"></param>
            /// <returns></returns>
            public override Boolean CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return TTimeConverterInternal.CanConvertTo(destinationType);
            }

            /// <summary>
            /// Converts the given object to the type of this converter (HH:MM:SS string).
            /// </summary>
            /// <param name="context"></param>
            /// <param name="culture"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                return TTimeConverterInternal.ConvertFrom(context, culture, value, true);
            }

            /// <summary>
            /// Converts the given value object to the specified type.  This is the method that the grid calls to display the times
            /// </summary>
            /// <param name="context"></param>
            /// <param name="culture"></param>
            /// <param name="value"></param>
            /// <param name="destinationType"></param>
            /// <returns></returns>
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                return TTimeConverterInternal.ConvertTo(context, culture, value, destinationType, true);
            }

            /// <summary>
            /// default constructor
            /// </summary>
            public TLongTimeConverter()
                : base()
            {
            }
        }

        private class TTimeConverterInternal
        {
            public static bool CanConvertFrom(Type SourceType)
            {
                switch (SourceType.FullName)
                {
                    case "System.String":
                    case "System.Int32":
                    case "System.Int64":
                    case "System.UInt32":
                    case "System.UInt64":
                    case "System.Double":
                        return true;
                }

                return false;
            }

            public static bool CanConvertTo(Type DestinationType)
            {
                return DestinationType == typeof(int) || DestinationType == typeof(string);
            }

            /// <summary>
            /// This private static method is shared by Short and Long time string converters
            /// It converts a given value for time to our string type (short or long)
            /// </summary>
            /// <param name="context"></param>
            /// <param name="culture"></param>
            /// <param name="value"></param>
            /// <param name="bAsLongTimeString">If true, the result is a long time string, otherwise a short one</param>
            /// <returns></returns>
            public static object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value, bool bAsLongTimeString)
            {
                DateTime dt = new DateTime();

                switch (value.GetType().FullName)
                {
                    case "System.Int32":
                    case "System.Int64":
                    case "System.UInt32":
                    case "System.UInt64":
                    case "System.Double":
                        // Number to string
                        double dblValue = Convert.ToDouble(value);

                        if ((dblValue >= 0.0) && (dblValue < 86400.0))
                        {
                            dt = dt.AddMilliseconds(dblValue * 1000);
                            return (bAsLongTimeString) ? dt.ToLongTimeString() : dt.ToShortTimeString();
                        }

                        break;

                    case "System.String":

                        // String to string
                        if (DateTime.TryParse(value.ToString(), out dt))
                        {
                            return (bAsLongTimeString) ? dt.ToLongTimeString() : dt.ToShortTimeString();
                        }

                        break;

                    default:
                        break;
                }

                return (bAsLongTimeString) ? "??:??:??" : "??:??";
            }

            /// <summary>
            /// This private static method is shared by Short and Long time string converters
            /// It converts a given value for time to the destination type
            /// </summary>
            /// <param name="context"></param>
            /// <param name="culture"></param>
            /// <param name="value"></param>
            /// <param name="destinationType"></param>
            /// <param name="bAsLongTimeString"></param>
            /// <returns></returns>
            public static object ConvertTo(ITypeDescriptorContext context,
                CultureInfo culture,
                object value,
                Type destinationType,
                bool bAsLongTimeString)
            {
                if (value == null)
                {
                    return null;
                }

                if (destinationType == typeof(string))
                {
                    DateTime dt = new DateTime();

                    switch (value.GetType().FullName)
                    {
                        case "System.Int32":
                        case "System.Int64":
                        case "System.UInt32":
                        case "System.UInt64":
                        case "System.Double":
                            // Number to string
                            double dblValue = Convert.ToDouble(value);

                            if ((dblValue >= 0.0) && (dblValue < 86400.0))
                            {
                                dt = dt.AddMilliseconds(dblValue * 1000);
                                return (bAsLongTimeString) ? dt.ToLongTimeString() : dt.ToShortTimeString();
                            }

                            break;

                        case "System.String":

                            // String to string
                            if (DateTime.TryParse(value.ToString(), out dt))
                            {
                                return (bAsLongTimeString) ? dt.ToLongTimeString() : dt.ToShortTimeString();
                            }

                            break;

                        default:
                            break;
                    }

                    return (bAsLongTimeString) ? "??:??:??" : "??:??";
                }
                else if ((destinationType == typeof(int)) && (value != null))
                {
                    switch (value.GetType().FullName)
                    {
                        case "System.String":
                            // String to int
                            DateTime dt = new DateTime();

                            if (DateTime.TryParse(value.ToString(), out dt))
                            {
                                return (int)((dt.Hour * 3600) + (dt.Minute * 60) + dt.Second);
                            }

                            break;

                        default:
                            break;
                    }

                    return -1;      // negative numbers are failures
                }

                return null;
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
            /// <summary>
            /// Test if we can convert from a given type to Boolean
            /// </summary>
            /// <param name="context"></param>
            /// <param name="sourceType"></param>
            /// <returns></returns>
            public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            /// <summary>
            /// Test if we can convert the boolean to a given type
            /// </summary>
            /// <param name="context"></param>
            /// <param name="sourceType"></param>
            /// <returns></returns>
            public override Boolean CanConvertTo(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            /// <summary>
            /// convert an object into a boolean
            /// </summary>
            /// <param name="context"></param>
            /// <param name="culture"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value.GetType() == typeof(string))
                {
                    return value;
                }

                return null;
            }

            /// <summary>
            /// convert a boolean into another type
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

            /// <summary>
            /// default constructor
            /// </summary>
            public TBooleanToYesNoConverter() : base()
            {
            }
        }

        #endregion
    }
}