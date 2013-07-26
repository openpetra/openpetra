//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Collections;
using System.Globalization;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using Ict.Common;


namespace Ict.Common
{
    /// <summary>
    /// enum for the supported data types in the TVariant class
    /// </summary>
    public enum eVariantTypes
    {
        /// <summary>
        /// no value at all
        /// </summary>
        eEmpty,

        /// <summary>
        /// date/time
        /// </summary>
        eDateTime,

        /// <summary>
        /// decimal
        /// </summary>
        eDecimal,

        /// <summary>
        /// currency (decimal, but with 2 fixed decimals)
        /// </summary>
        eCurrency,

        /// <summary>
        /// integer
        /// </summary>
        eInteger,

        /// <summary>
        /// long integer
        /// </summary>
        eInt64,

        /// <summary>
        /// string
        /// </summary>
        eString,

        /// <summary>
        /// boolean
        /// </summary>
        eBoolean,

        /// <summary>
        /// composite: several TVariants concatenated
        /// </summary>
        eComposite
    };

    /// <summary>
    ///  a class for the storage of values in different representations;
    /// Conversion functions are provided.
    /// </summary>
    [Serializable]
    public class TVariant : System.Runtime.Serialization.ISerializable
    {
        private const String DATETIME_UNAMBIGUOUS_FORMAT = @"yyyy-MM-ddTHH:mm:ss";

        /// <summary>
        /// remove all trailing zeros, and the decimal point, if there are no decimals left
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string StripDecimalAndZeros(string s)
        {
            string ReturnValue = s;

            if (ReturnValue.IndexOf(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) != -1)
            {
                while (ReturnValue[ReturnValue.Length - 1] == '0')
                {
                    ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 1);
                }

                if (ReturnValue[ReturnValue.Length - 1].ToString() == CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator)
                {
                    ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 1);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// the type of the value stored in this instance
        /// </summary>
        public eVariantTypes TypeVariant;

        /// <summary>for currencies and dayofyear (date)</summary>
        public String FormatString = "";
        private System.Object EmptyValue;
        private System.DateTime DateValue;
        private decimal DecimalValue;
        private System.Int32 IntegerValue;
        private System.Int64 Int64Value;
        private String StringValue;
        private bool BooleanValue;
        private ArrayList CompositeValue;

        /// <summary>
        /// constructor from any object
        /// </summary>
        /// <param name="value">any type of object</param>
        public TVariant(System.Object value)
        {
            if (value == null)
            {
                this.Assign(new TVariant());
            }
            else if (value.GetType() == typeof(bool))
            {
                this.Assign(new TVariant((bool)value));
            }
            else if (value.GetType() == typeof(System.DateTime))
            {
                this.Assign(new TVariant((System.DateTime)value));
            }
            else if (value.GetType() == typeof(double))
            {
                this.Assign(new TVariant(Convert.ToDecimal(value), "Currency"));
            }
            else if (value.GetType() == typeof(System.Decimal))
            {
                this.Assign(new TVariant((decimal)value, "Currency"));
            }
            else if (value.GetType() == typeof(System.Int16))
            {
                this.Assign(new TVariant(Convert.ToInt32(value)));
            }
            else if (value.GetType() == typeof(System.Int32))
            {
                this.Assign(new TVariant((System.Int32)value));
            }
            else if (value.GetType() == typeof(System.Int64))
            {
                this.Assign(new TVariant(Convert.ToInt64(value)));
            }
            else if (value.GetType() == typeof(TVariant))
            {
                this.Assign(new TVariant((TVariant)value));
            }
            else
            {
                this.Assign(new TVariant(value.ToString()));
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public TVariant()
        {
            TypeVariant = eVariantTypes.eEmpty;
            EmptyValue = null;
            FormatString = "";
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="value"></param>
        public TVariant(TVariant value)
        {
            if (value != null)
            {
                Assign(value);
            }
            else
            {
                TypeVariant = eVariantTypes.eEmpty;
                EmptyValue = null;
                FormatString = "";
            }
        }

        /// <summary>
        /// constructor for dates
        /// </summary>
        /// <param name="value">date time value</param>
        /// <param name="AFormat">can be dayofyear (for birthdays)</param>
        /// <returns>void</returns>
        ///
        public TVariant(System.DateTime value, String AFormat)
        {
            TypeVariant = eVariantTypes.eDateTime;
            DateValue = value;
            FormatString = AFormat;
        }

        /// <summary>
        /// constructor for dates
        /// </summary>
        /// <param name="value">date time value</param>
        public TVariant(System.DateTime value)
            : this(value, "")
        {
        }

        /// <summary>
        /// constructor for decimal
        /// </summary>
        /// <param name="value">decimal value</param>
        /// <param name="AFormat">requested format</param>
        public TVariant(decimal value, String AFormat)
        {
            TypeVariant = eVariantTypes.eCurrency;
            DecimalValue = value;
            FormatString = AFormat;
        }

        /// <summary>
        /// constructor for decimal
        /// </summary>
        /// <param name="value">decimal value</param>
        public TVariant(decimal value)
        {
            TypeVariant = eVariantTypes.eDecimal;
            DecimalValue = (decimal)value;
            FormatString = "";
        }

        /// <summary>
        /// constructor for double (will be converted to decimal)
        /// </summary>
        /// <param name="value">double value</param>
        public TVariant(double value)
        {
            TypeVariant = eVariantTypes.eDecimal;
            DecimalValue = (decimal)value;
            FormatString = "";
        }

        /// <summary>
        /// constructor for single characters
        /// </summary>
        /// <param name="value">character value</param>
        public TVariant(char value)
            : this(value.ToString())
        {
        }

        /// <summary>
        /// constructor for integer
        /// </summary>
        /// <param name="value">integer value</param>
        public TVariant(System.Int32 value)
        {
            TypeVariant = eVariantTypes.eInteger;
            IntegerValue = value;
            FormatString = "";
        }

        /// <summary>
        /// constructor for long integer
        /// </summary>
        /// <param name="value">long integer value</param>
        public TVariant(System.Int64 value)
        {
            if (value < System.Int32.MaxValue)
            {
                TypeVariant = eVariantTypes.eInteger;
                IntegerValue = (int)value;
            }
            else
            {
                TypeVariant = eVariantTypes.eInt64;
                Int64Value = value;
            }

            FormatString = "";
        }

        /// <summary>
        /// constructor for string
        /// will look at the content of the string and generate a typed representation
        /// if you want to force a string value, use the other overloaded constructor below
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="AFormat">requested format</param>
        public TVariant(String value, String AFormat)
        {
            FormatString = AFormat;

            if (value == null)
            {
                TypeVariant = eVariantTypes.eEmpty;
                return;
            }

            TypeVariant = eVariantTypes.eString;
            StringValue = value;

            if (this.ToBool().ToString() == StringValue)
            {
                BooleanValue = this.ToBool();
                TypeVariant = eVariantTypes.eBoolean;
            }

            if (this.ToDate() != DateTime.MinValue)
            {
                // this is needed for SQLite, which returns date values as a string
                DateValue = this.ToDate();
                TypeVariant = eVariantTypes.eDateTime;
            }
            else if ((value.Length > 0)
                     && ((value[0] == '-')
                         || ((value[0] >= '0')
                             && (value[0] <= '9')))
                     && (((this.ToInt64() ==
                           StringHelper.TryStrToInt(StringValue,
                               -1)) && (this.ToInt64() != -1) && (this.ToInt64() == 0)) || (this.ToInt64().ToString() == StringValue)))
            {
                // prevent unnecessary Exceptions from TryStrToInt
                // we cannot do the following (convert in both directions to catch numbers with leading zeros, e.g. Partnerkey)
                // this would cause trouble with eg. Account codes that should start with 0
                // I will limit it to just zeros, which would catch an empty partner key to become a zero
                // will create Int32 if the value is small enough
                Int64Value = this.ToInt64();
                IntegerValue = this.ToInt();
                TypeVariant = eVariantTypes.eInt64;

                if (Int64Value == IntegerValue)
                {
                    TypeVariant = eVariantTypes.eInteger;
                }
            }
            else if (this.ToDecimal().ToString() == StripDecimalAndZeros(StringValue))
            {
                // has to work for 0.0 as well!
                DecimalValue = this.ToDecimal();
                TypeVariant = eVariantTypes.eDecimal;
            }
            else if ((StringValue.Length == 10) && (StringValue[0] == '#') && (StringValue[9] == '#'))
            {
                DateValue =
                    new DateTime(Convert.ToInt32(StringValue.Substring(1,
                                4)), Convert.ToInt32(StringValue.Substring(5, 2)), Convert.ToInt32(StringValue.Substring(7, 2)));
                TypeVariant = eVariantTypes.eDateTime;
            }
        }

        /// <summary>
        /// constructor for string
        /// </summary>
        /// <param name="value">string value</param>
        public TVariant(String value) : this(value, "")
        {
        }

        /// <summary>
        /// constructor for string
        /// if you want the content to be kept as a string, and not converted to int (e.g. cost centre: 0200)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ExplicitString"></param>
        public TVariant(String value, bool ExplicitString) : this(value, "")
        {
            if (ExplicitString)
            {
                TypeVariant = eVariantTypes.eString;
                StringValue = value;
                FormatString = "text";
            }
        }

        /// <summary>
        /// constructor for boolean
        /// </summary>
        /// <param name="value">boolean value</param>
        public TVariant(bool value)
        {
            TypeVariant = eVariantTypes.eBoolean;
            BooleanValue = value;
            FormatString = "";
        }

        /// <summary>
        /// This either adds the new value to an already existing composite (list of several values),
        /// or it changes the current non composite variable to be a composite,
        /// by adding the current value as the first member to the list, and then adding the new value;
        /// ToString will concatenate the values, but ToDecimal, ToBoolean etc will only convert the first value in the composite
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Add(TVariant value, String AFormatString, Boolean AConcatenateStrings)
        {
            TVariant lastValue;

            value.ApplyFormatString(AFormatString);

            if (TypeVariant == eVariantTypes.eComposite)
            {
                if ((value.TypeVariant == eVariantTypes.eComposite) && (value.CompositeValue.Count == 1))
                {
                    // if there is only one element in the other composite, reduce to the element
                    value = (TVariant)value.CompositeValue[0];
                }

                // try to concatenate strings
                if ((AConcatenateStrings == true) && (value.TypeVariant == eVariantTypes.eString) && (CompositeValue.Count > 0))
                {
                    lastValue = (TVariant)CompositeValue[CompositeValue.Count - 1];

                    if (lastValue.TypeVariant == eVariantTypes.eString)
                    {
                        // don't create a new value in the list, but add the string to the last element
                        lastValue.StringValue = lastValue.StringValue + value.StringValue;
                    }
                    else
                    {
                        CompositeValue.Add(value);
                    }
                }
                else
                {
                    CompositeValue.Add(value);
                }
            }
            else
            {
                if ((!IsNil()))
                {
                    if ((AConcatenateStrings == true) && (value.TypeVariant == eVariantTypes.eString) && (this.TypeVariant == eVariantTypes.eString))
                    {
                        // don't create a new value in the list, but add the string to the last element
                        this.StringValue = this.StringValue + value.StringValue;
                    }
                    else
                    {
                        CompositeValue = new ArrayList();

                        // if this has already a value, then move the value over into the arraylist
                        CompositeValue.Add(new TVariant(this));
                        TypeVariant = eVariantTypes.eComposite;
                        Add(value, AFormatString, AConcatenateStrings);

                        // recursive call so that we only need to code once for adding the strings directly
                    }
                }
                else
                {
                    // don't create a list at the moment, just copy the value
                    this.Assign(value);
                }
            }
        }

        /// <summary>
        /// overload for Add
        /// </summary>
        /// <param name="value">value to be added</param>
        /// <param name="AFormatString">requested format for the value</param>
        public void Add(TVariant value, String AFormatString)
        {
            Add(value, AFormatString, true);
        }

        /// <summary>
        /// overload for Add
        /// </summary>
        /// <param name="value">value to be added</param>
        public void Add(TVariant value)
        {
            Add(value, "", true);
        }

        /// <summary>
        /// returns the first value of the list, or an empty value, if the list is empty
        /// </summary>
        /// <returns>void</returns>
        public TVariant FirstCompositeValue()
        {
            TVariant ReturnValue;

            if ((TypeVariant == eVariantTypes.eComposite) && (CompositeValue != null) && (CompositeValue.Count > 0))
            {
                ReturnValue = (TVariant)CompositeValue[0];
            }
            else
            {
                ReturnValue = this;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This static function is used to create a variant with the correct type,
        /// when the variant is restored from an encoded string,
        /// that could e.g. be stored in a datatable (only strings)
        ///
        /// </summary>
        /// <returns>void</returns>
        public static TVariant DecodeFromString(String encodedValue)
        {
            TVariant ReturnValue;
            TVariant value;
            String typestr;
            String valuestr;
            String currencyFormat;
            String originalEncodedValue;
            String BeforeTryingFormat;
            String compositeEncodedValue;

            ReturnValue = new TVariant();
            originalEncodedValue = encodedValue;
            try
            {
                typestr = StringHelper.GetNextCSV(ref encodedValue, ":");
                currencyFormat = "";

                if (typestr == eVariantTypes.eComposite.ToString())
                {
                    currencyFormat = StringHelper.GetNextCSV(ref encodedValue, ":");
                    valuestr = StringHelper.GetNextCSV(ref encodedValue, ":");
                    value = new TVariant();

                    while (valuestr.Length > 0)
                    {
                        compositeEncodedValue = StringHelper.GetNextCSV(ref valuestr, "|");
                        value.Add(DecodeFromString(compositeEncodedValue), "", false);

                        // don't add strings up when decoding
                    }
                }
                else
                {
                    BeforeTryingFormat = encodedValue;
                    currencyFormat = StringHelper.GetNextCSV(ref encodedValue, ":");

                    // if there was no format in the encoded value, undo the previous step
                    if (encodedValue.Length == 0)
                    {
                        // there was no format
                        encodedValue = BeforeTryingFormat;
                        currencyFormat = "";
                    }

                    valuestr = StringHelper.GetNextCSV(ref encodedValue, ":");

                    if (typestr == eVariantTypes.eDateTime.ToString())
                    {
                        try
                        {
                            value =
                                new TVariant(DateTime.ParseExact(valuestr, DATETIME_UNAMBIGUOUS_FORMAT, DateTimeFormatInfo.InvariantInfo,
                                        DateTimeStyles.AssumeLocal));
                        }
                        catch (Exception)
                        {
                            value = new TVariant(DateTime.MinValue);
                        }
                        value.FormatString = currencyFormat;
                    }
                    else if ((typestr == eVariantTypes.eDecimal.ToString()) || (typestr == "eDouble"))
                    {
                        value = new TVariant(BitConverter.Int64BitsToDouble(Convert.ToInt64(valuestr)));
                    }
                    else if (typestr == eVariantTypes.eCurrency.ToString())
                    {
                        value = new TVariant((decimal)BitConverter.Int64BitsToDouble(Convert.ToInt64(valuestr)), "Currency");
                    }
                    else if (typestr == eVariantTypes.eInteger.ToString())
                    {
                        value = new TVariant(Convert.ToInt32(valuestr));
                    }
                    else if (typestr == eVariantTypes.eInt64.ToString())
                    {
                        value = new TVariant(Convert.ToInt64(valuestr));
                    }
                    else if (typestr == eVariantTypes.eBoolean.ToString())
                    {
                        value = new TVariant(Convert.ToBoolean(valuestr));
                    }
                    else if (typestr == eVariantTypes.eString.ToString())
                    {
                        value = new TVariant((String)valuestr, true);
                    }
                    else if (typestr == eVariantTypes.eEmpty.ToString())
                    {
                        value = new TVariant();
                    }
                    else
                    {
                        value = new TVariant(valuestr);
                    }
                }

                value.FormatString = currencyFormat;
                ReturnValue = value;
            }
            catch (Exception e)
            {
                TLogging.Log("problem decoding " + originalEncodedValue + Environment.NewLine + e.Message);
                ReturnValue = new TVariant();
            }
            return ReturnValue;
        }

        /// <summary>
        /// This function creates an encoded string, that holds the value and the type
        ///
        /// </summary>
        /// <returns>void</returns>
        public String EncodeToString()
        {
            String ReturnValue = "";

            try
            {
                ReturnValue = this.TypeVariant.ToString();

                if ((this.TypeVariant == eVariantTypes.eCurrency) || (this.TypeVariant == eVariantTypes.eComposite) || (FormatString.Length > 0))
                {
                    // make sure that it is put into quotes if there appear to be any separators in the string
                    ReturnValue = StringHelper.AddCSV(ReturnValue, FormatString, ":");
                }

                if ((this.TypeVariant == eVariantTypes.eDecimal) || (TypeVariant == eVariantTypes.eCurrency))
                {
                    // what about decimal point/comma? BitConverter saves it as int; that way no trouble with decimal point
                    ReturnValue = StringHelper.AddCSV(ReturnValue, BitConverter.DoubleToInt64Bits(this.ToDouble()).ToString(), ":");
                }
                else if (this.TypeVariant == eVariantTypes.eDateTime)
                {
                    // Force encoding into a well-defined UTC-grounded format
                    ReturnValue =
                        StringHelper.AddCSV(ReturnValue,
                            DateValue.ToString(DATETIME_UNAMBIGUOUS_FORMAT, DateTimeFormatInfo.InvariantInfo), ":");
                }
                else if (this.TypeVariant == eVariantTypes.eComposite)
                {
                    String CompositeEncodedLine = "";
                    Boolean first = true;

                    foreach (TVariant v in this.CompositeValue)
                    {
                        String CompositeEncoded = v.EncodeToString();

                        /*
                         * StringHelper.AddCSV() was not used for the first
                         * entry which was an empty string because
                         * StringHelper.AddCSV() will use a space in that
                         * circumstance. Therefore, we must manually handle the
                         * case where the length of the first member of the
                         * composite value is an empty string.
                         */
                        if ((CompositeEncodedLine.Length == 0) && !first)
                        {
                            CompositeEncodedLine += "|";
                        }

                        if (!first || (CompositeEncoded.Length > 0))
                        {
                            CompositeEncodedLine = StringHelper.AddCSV(CompositeEncodedLine, CompositeEncoded, "|");
                        }

                        first = false;
                    }

                    ReturnValue = StringHelper.AddCSV(ReturnValue, CompositeEncodedLine, ":");
                }
                else if (this.TypeVariant == eVariantTypes.eString)
                {
                    // make sure that enough quotes are around the value
                    ReturnValue = StringHelper.AddCSV(ReturnValue, this.ToString(), ":");
                }
                else
                {
                    ReturnValue = StringHelper.AddCSV(ReturnValue, this.ToString(), ":");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TLogging.LogStackTrace(TLoggingType.ToConsole);
            }
            return ReturnValue;
        }

        /// <summary>
        /// copy the value to the current object instance
        /// </summary>
        /// <param name="value">value to be copied</param>
        public void Assign(TVariant value)
        {
            TypeVariant = value.TypeVariant;
            EmptyValue = value.EmptyValue;
            DateValue = value.DateValue;
            DecimalValue = value.DecimalValue;
            FormatString = value.FormatString;
            IntegerValue = value.IntegerValue;
            Int64Value = value.Int64Value;
            StringValue = value.StringValue;
            BooleanValue = value.BooleanValue;
            CompositeValue = new ArrayList();

            if (value.CompositeValue != null)
            {
                foreach (TVariant v in value.CompositeValue)
                {
                    CompositeValue.Add(new TVariant(v));
                }
            }
        }

        /// <summary>
        /// Convert TVariant to Object
        /// </summary>
        /// <returns>a representation of this TVariant instance as a typed Object</returns>
        public System.Object ToObject()
        {
            System.Object ReturnValue;
            ReturnValue = "NOTFOUND";

            if (TypeVariant == eVariantTypes.eEmpty)
            {
                ReturnValue = new System.Object();
            }
            else if (TypeVariant == eVariantTypes.eComposite)
            {
                ReturnValue = FirstCompositeValue().ToObject();
            }
            else if (TypeVariant == eVariantTypes.eDateTime)
            {
                ReturnValue = DateValue;
            }
            else if ((TypeVariant == eVariantTypes.eDecimal) || (TypeVariant == eVariantTypes.eCurrency))
            {
                ReturnValue = (System.Object)(DecimalValue);
            }
            else if (TypeVariant == eVariantTypes.eInteger)
            {
                ReturnValue = (System.Object)(IntegerValue);
            }
            else if (TypeVariant == eVariantTypes.eInt64)
            {
                ReturnValue = (System.Object)(Int64Value);
            }
            else if (TypeVariant == eVariantTypes.eString)
            {
                ReturnValue = StringValue;
            }
            else if (TypeVariant == eVariantTypes.eBoolean)
            {
                ReturnValue = (System.Object)(BooleanValue);
            }

            return ReturnValue;
        }

        /// <summary>
        /// convert to Integer
        /// </summary>
        /// <returns>an integer representation</returns>
        public System.Int32 ToInt()
        {
            System.Int32 ReturnValue;

            if (TypeVariant == eVariantTypes.eComposite)
            {
                ReturnValue = FirstCompositeValue().ToInt();
            }
            else if (TypeVariant == eVariantTypes.eInteger)
            {
                ReturnValue = IntegerValue;
            }
            else if (TypeVariant == eVariantTypes.eInt64)
            {
                ReturnValue = Convert.ToInt32(Int64Value);
            }
            else if (TypeVariant == eVariantTypes.eString)
            {
                ReturnValue = StringHelper.TryStrToInt32(StringValue, -1);
            }
            else if ((TypeVariant == eVariantTypes.eDecimal) || (TypeVariant == eVariantTypes.eCurrency))
            {
                ReturnValue = Convert.ToInt32(DecimalValue);
            }
            else
            {
                ReturnValue = -1;
            }

            return ReturnValue;
        }

        /// <summary>
        /// convert to Integer
        /// </summary>
        /// <returns>an integer representation</returns>
        public System.Int32 ToInt32()
        {
            return ToInt();
        }

        /// <summary>
        /// convert to Integer
        /// </summary>
        /// <returns>an integer representation</returns>
        public System.Int64 ToInt64()
        {
            System.Int64 ReturnValue;

            if (TypeVariant == eVariantTypes.eComposite)
            {
                ReturnValue = FirstCompositeValue().ToInt64();
            }
            else if (TypeVariant == eVariantTypes.eInteger)
            {
                ReturnValue = Convert.ToInt64(IntegerValue);
            }
            else if (TypeVariant == eVariantTypes.eInt64)
            {
                ReturnValue = Int64Value;
            }
            else if (TypeVariant == eVariantTypes.eString)
            {
                ReturnValue = StringHelper.TryStrToInt(StringValue, -1);
            }
            else if ((TypeVariant == eVariantTypes.eDecimal) || (TypeVariant == eVariantTypes.eCurrency))
            {
                ReturnValue = Convert.ToInt64(DecimalValue);
            }
            else
            {
                ReturnValue = -1;
            }

            return ReturnValue;
        }

        /// <summary>
        /// convert to Boolean
        /// </summary>
        /// <returns>a boolean representation</returns>
        public bool ToBool()
        {
            bool ReturnValue;

            ReturnValue = false;

            if (TypeVariant == eVariantTypes.eComposite)
            {
                ReturnValue = FirstCompositeValue().ToBool();
            }
            else if (TypeVariant == eVariantTypes.eBoolean)
            {
                ReturnValue = BooleanValue;
            }
            else if (TypeVariant == eVariantTypes.eInteger)
            {
                ReturnValue = IntegerValue != 0;
            }
            else if (TypeVariant == eVariantTypes.eInt64)
            {
                ReturnValue = Int64Value != 0;
            }
            else if (TypeVariant == eVariantTypes.eString)
            {
                ReturnValue = (StringValue.CompareTo("1") == 0) || (StringValue.ToUpper().CompareTo("TRUE") == 0)
                              || (StringValue.ToUpper().CompareTo("YES") == 0);
            }

            return ReturnValue;
        }

        /// <summary>
        /// convert to Double
        /// </summary>
        /// <returns>a double representation</returns>
        public double ToDouble()
        {
            double ReturnValue;

            ReturnValue = 0.0;

            if (TypeVariant == eVariantTypes.eComposite)
            {
                ReturnValue = FirstCompositeValue().ToDouble();
            }
            else if ((TypeVariant == eVariantTypes.eDecimal) || (TypeVariant == eVariantTypes.eCurrency))
            {
                ReturnValue = (double)DecimalValue;
            }
            else if (TypeVariant == eVariantTypes.eInteger)
            {
                ReturnValue = IntegerValue;
            }
            else if (TypeVariant == eVariantTypes.eInt64)
            {
                ReturnValue = Int64Value;
            }
            else if (TypeVariant == eVariantTypes.eString)
            {
                ReturnValue = (double)StringHelper.TryStrToDecimal(StringValue, 0.0M);
            }

            return ReturnValue;
        }

        /// <summary>
        /// convert to Decimal
        /// </summary>
        /// <returns>a decimal representation</returns>
        public decimal ToDecimal()
        {
            decimal ReturnValue;

            ReturnValue = 0.0M;

            if (TypeVariant == eVariantTypes.eComposite)
            {
                ReturnValue = FirstCompositeValue().ToDecimal();
            }
            else if ((TypeVariant == eVariantTypes.eDecimal) || (TypeVariant == eVariantTypes.eCurrency))
            {
                ReturnValue = DecimalValue;
            }
            else if (TypeVariant == eVariantTypes.eInteger)
            {
                ReturnValue = IntegerValue;
            }
            else if (TypeVariant == eVariantTypes.eInt64)
            {
                ReturnValue = Int64Value;
            }
            else if (TypeVariant == eVariantTypes.eString)
            {
                ReturnValue = StringHelper.TryStrToDecimal(StringValue, 0.0M);
            }

            return ReturnValue;
        }

        /// <summary>
        /// convert to list of TVariant
        /// </summary>
        /// <returns>an array of TVariant</returns>
        public ArrayList ToComposite()
        {
            ArrayList ReturnValue;

            if (TypeVariant == eVariantTypes.eComposite)
            {
                ReturnValue = CompositeValue;
            }
            else
            {
                ReturnValue = new ArrayList(new Object[] { this });
            }

            return ReturnValue;
        }

        /// <summary>
        /// will call ToString(false),
        /// which means it will not print "NOTFOUND" for an empty value,
        /// for debugging call toString(true)
        /// </summary>
        /// <returns>void</returns>
        public override String ToString()
        {
            return ToString(false);
        }

        /// <summary>
        /// print to string
        /// </summary>
        /// <param name="printNotFound">print something so that it is clear that the value was invalid</param>
        /// <returns>a string representation</returns>
        public String ToString(bool printNotFound)
        {
            String ReturnValue;

            ReturnValue = "NOTFOUND";

            if (TypeVariant == eVariantTypes.eEmpty)
            {
                if (printNotFound)
                {
                    ReturnValue = "NOTFOUND";
                }
                else
                {
                    ReturnValue = "";
                }
            }
            else if (TypeVariant == eVariantTypes.eDateTime)
            {
                if ((DateValue.Hour == 0) && (DateValue.Minute == 0) && (DateValue.Second == 0))
                {
                    // don't use DateToLocalizedString, because the server might not understand the format of the client
                    ReturnValue = DateValue.ToString("dd/MM/yyyy");
                }
                else
                {
                    ReturnValue = DateValue.ToString("dd/MM/yyyy/HH/mm/ss");

                    // don't use DateToLocalizedString, because the server might not understand the format of the client
                }
            }
            else if ((TypeVariant == eVariantTypes.eDecimal) || (TypeVariant == eVariantTypes.eCurrency))
            {
                ReturnValue = DecimalValue.ToString();
            }
            else if (TypeVariant == eVariantTypes.eInteger)
            {
                ReturnValue = IntegerValue.ToString();
            }
            else if (TypeVariant == eVariantTypes.eInt64)
            {
                ReturnValue = Int64Value.ToString();
            }
            else if (TypeVariant == eVariantTypes.eString)
            {
                ReturnValue = StringValue;
            }
            else if (TypeVariant == eVariantTypes.eBoolean)
            {
                ReturnValue = BooleanValue.ToString().Replace("True", "true").Replace("False", "false");
            }
            else if (TypeVariant == eVariantTypes.eComposite)
            {
                ReturnValue = "";

                foreach (TVariant tv in this.CompositeValue)
                {
                    ReturnValue = ReturnValue + tv.ToString();
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// convert to Date
        /// </summary>
        /// <returns>a date representation</returns>
        public System.DateTime ToDate()
        {
            System.DateTime ReturnValue;

            if (TypeVariant == eVariantTypes.eComposite)
            {
                ReturnValue = FirstCompositeValue().ToDate();
            }
            else if (TypeVariant == eVariantTypes.eDateTime)
            {
                ReturnValue = DateValue;
            }
            else if (TypeVariant == eVariantTypes.eString)
            {
                // todo: need to decode, similar to ToDecimal?
                // should we raise an Exception?
                ReturnValue = System.DateTime.MinValue;

                // for SQLite, attempt to match eg. 2009-02-28 00:00:00
                Regex exp = new Regex(@"(\d\d\d\d)-(\d\d)-(\d\d) (\d\d):(\d\d):(\d\d)");
                MatchCollection matches = exp.Matches(StringValue);

                if (matches.Count != 0)
                {
                    return new DateTime(Convert.ToInt32(matches[0].Groups[1].ToString()),
                        Convert.ToInt32(matches[0].Groups[2].ToString()),
                        Convert.ToInt32(matches[0].Groups[3].ToString()),
                        Convert.ToInt32(matches[0].Groups[4].ToString()),
                        Convert.ToInt32(matches[0].Groups[5].ToString()),
                        Convert.ToInt32(matches[0].Groups[6].ToString()));
                }
            }
            else
            {
                // should we raise an Exception?
                ReturnValue = System.DateTime.MinValue;
            }

            return ReturnValue;
        }

        /// <summary>
        /// print date to string
        /// </summary>
        /// <param name="format">the requested format to be used</param>
        /// <returns>a string representation</returns>
        public String DateToString(String format)
        {
            return ToDate().ToString(format);
        }

        /// <summary>
        /// format a currency value and print to string
        /// </summary>
        /// <param name="ACurrencyFormat">which format to use</param>
        /// <param name="AOutputType">print to CSV, or for localised output</param>
        /// <returns>a string with the formatted date</returns>
        private String CurrencyToFormattedString(String ACurrencyFormat, String AOutputType)
        {
            String ReturnValue = "";

            if (AOutputType == "Localized")
            {
                ReturnValue = StringHelper.FormatCurrency(this, ACurrencyFormat);
            }
            else if (AOutputType == "CSV")
            {
                if (this.TypeVariant == eVariantTypes.eEmpty)
                {
                    ReturnValue = "";
                }
                else
                {
                    if (StringHelper.IsCurrencyFormatString(this.FormatString) == true)
                    {
                        // don't bother with format, print the whole number
                        ReturnValue = StringHelper.FormatCurrency(ToDecimal(), "#,##0.00;-#,##0.00;0.00;0");
                    }
                    else
                    {
                        ReturnValue = StringHelper.FormatCurrency(this, "");
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// overload for CurrencyToFormattedString, for localized output
        /// </summary>
        /// <param name="ACurrencyFormat">which format to use</param>
        /// <returns>formatted string</returns>
        private String CurrencyToFormattedString(String ACurrencyFormat)
        {
            return CurrencyToFormattedString(ACurrencyFormat, "Localized");
        }

        /// <summary>
        /// Format the current value to be exported to CSV or to be printed with the local culture format settings (for currencies and dates);
        /// </summary>
        /// <param name="ACurrencyFormat">CurrencyThousands or CurrencyWithoutDecimals</param>
        /// <param name="AOutputType">Localized or CSV
        /// </param>
        /// <returns>void</returns>
        public String ToFormattedString(String ACurrencyFormat, String AOutputType)
        {
            String ReturnValue;

            DateTime ThisYearDate;
            Boolean first;
            Boolean useSeparator;

            ReturnValue = "";

            if (TypeVariant == eVariantTypes.eDateTime)
            {
                if (AOutputType == "Localized")
                {
                    if ((ACurrencyFormat == "dayofyear") || (this.FormatString == "dayofyear"))
                    {
                        ReturnValue = StringHelper.FormatCurrency(this, "dayofyear");
                    }
                    else // formatteddate
                    {
                        ReturnValue = StringHelper.DateToLocalizedString(DateValue);
                    }
                }
                else if (AOutputType == "CSV")
                {
                    if ((ACurrencyFormat == "dayofyear") || (this.FormatString == "dayofyear"))
                    {
                        ThisYearDate = new DateTime(DateTime.Today.Year, DateValue.Month, DateValue.Day);

                        // for Excel: 'd';
                        ReturnValue = ThisYearDate.ToString("d");
                    }
                    else
                    {
                        // for Excel: 'd'
                        ReturnValue = DateValue.ToString("d");
                    }
                }
            }
            // special treatment for partnerkey, it should be formatted for both csv and text output
            else if ((ACurrencyFormat.ToLower() == "partnerkey") || (this.FormatString.ToLower() == "partnerkey"))
            {
                ReturnValue = StringHelper.FormatCurrency(this, ACurrencyFormat);
            }
            else if (TypeVariant == eVariantTypes.eCurrency)
            {
                ReturnValue = CurrencyToFormattedString(ACurrencyFormat, AOutputType);
            }
            else if (TypeVariant == eVariantTypes.eComposite)
            {
                ReturnValue = "";
                first = true;
                useSeparator = (ACurrencyFormat.IndexOf("csvlistslash") == 0) || (this.FormatString.IndexOf("csvlistslash") == 0);

                foreach (TVariant tv in this.CompositeValue)
                {
                    if ((useSeparator) && ((!first)))
                    {
                        ReturnValue = ReturnValue + '/';
                    }

                    first = false;
                    ReturnValue = ReturnValue + tv.ToFormattedString("", AOutputType);
                }
            }
            else if (this.TypeVariant == eVariantTypes.eString)
            {
                ReturnValue = this.ToString();
            }
            else if ((ACurrencyFormat.Length != 0) || (this.FormatString.Length != 0))
            {
                // format also other types, e.g. integer and decimal, for percentage or partnerkey etc.
                ReturnValue = CurrencyToFormattedString(ACurrencyFormat, AOutputType);
            }
            else
            {
                ReturnValue = this.ToString();
            }

            return ReturnValue;
        }

        /// <summary>
        /// overload of ToFormattedString
        /// </summary>
        /// <param name="ACurrencyFormat">format to use</param>
        /// <returns>formatted string</returns>
        public String ToFormattedString(String ACurrencyFormat)
        {
            return ToFormattedString(ACurrencyFormat, "Localized");
        }

        /// <summary>
        /// overload of ToFormattedString
        /// </summary>
        /// <returns>formatted string</returns>
        public String ToFormattedString()
        {
            return ToFormattedString("", "Localized");
        }

        /// <summary>
        /// apply the string format to the current variable
        /// </summary>
        /// <param name="AFormatString">format to be used</param>
        public void ApplyFormatString(String AFormatString)
        {
            String columnformat;
            int Counter;

            columnformat = AFormatString.ToLower();;

            if (columnformat.IndexOf("csvlistslash") == 0)
            {
                StringHelper.GetNextCSV(ref columnformat, ":");

                // this will leave the last element in columnFormat
                // if there is only one element in the list, we don't want to overwrite the format of that element
                // but we need to apply the format to each member of the list
                if (this.TypeVariant == eVariantTypes.eComposite)
                {
                    FormatString = "csvlistslash";

                    for (Counter = 0; Counter <= this.CompositeValue.Count - 1; Counter += 1)
                    {
                        ((TVariant) this.CompositeValue[Counter]).ApplyFormatString(columnformat);
                    }

                    // don't proceed, it might only cause trouble, e.g. with partnerkey being assigned to the composite
                    return;
                }
            }
            else if (columnformat.IndexOf("quotes") == 0)
            {
                // apply the quotes immediately
                this.StringValue = "'" + this.ToFormattedString() + "'";
                this.TypeVariant = eVariantTypes.eString;
                this.FormatString = "text";
            }

            if (this.ToFormattedString(columnformat).Length == 0)
            {
                // e.g. empty partnerkey should print 0000000000
                this.TypeVariant = eVariantTypes.eEmpty;

                if (columnformat != "")
                {
                    this.FormatString = columnformat;
                }
            }
            else
            {
                if (columnformat == "partnerkey")
                {
                    // this value comes from the database as a decimal, and therefore is treated like a currency;
                    // that is not good for csv file export; so we convert it to Int64
                    this.Int64Value = ToInt64();
                    this.TypeVariant = eVariantTypes.eInt64;
                    this.FormatString = columnformat;
                }
                else if (columnformat == "text")
                {
                    // this value comes from the database as some type, but should really be treated as a text string, without formatting
                    // so we convert it to String
                    this.StringValue = ToString();
                    this.TypeVariant = eVariantTypes.eString;
                    this.FormatString = columnformat;
                }
                else if (columnformat.StartsWith("numberformat"))
                {
                    this.StringValue = this.DecimalValue.ToString(columnformat.Substring(12));
                    this.TypeVariant = eVariantTypes.eString;
                    this.FormatString = columnformat;
                }
                else if (StringHelper.GetFormatString(columnformat, "").Length > 0)
                {
                    // this should filter out 'row', etc.
                    this.FormatString = columnformat;
                }
            }
        }

        /// <summary>
        /// evaluate if the current value is zero or null
        /// </summary>
        /// <returns>s true if the value is empty or has an empty string or 0 or 0.0</returns>
        public bool IsZeroOrNull()
        {
            bool ReturnValue;

            ReturnValue = (TypeVariant == eVariantTypes.eEmpty)
                          || ((TypeVariant == eVariantTypes.eInteger)
                              && (IntegerValue == 0))
                          || ((TypeVariant == eVariantTypes.eInt64)
                              && (Int64Value == 0))
                          || ((TypeVariant == eVariantTypes.eDecimal || TypeVariant == eVariantTypes.eCurrency)
                              && (DecimalValue == 0.0M))
                          || ((TypeVariant == eVariantTypes.eString)
                              && ((StringValue.CompareTo("NOTFOUND") == 0)
                                  || (StringValue.Length == 0)))
                          || ((TypeVariant == eVariantTypes.eDateTime) && (DateValue.Equals(System.DateTime.MinValue)));

            if (TypeVariant == eVariantTypes.eComposite)
            {
                ReturnValue = true;

                foreach (TVariant v in this.CompositeValue)
                {
                    if ((!v.IsZeroOrNull()))
                    {
                        ReturnValue = false;
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// check if the value is empty
        /// </summary>
        /// <returns>s true if the value is empty</returns>
        public bool IsNil()
        {
            return (TypeVariant == eVariantTypes.eEmpty) || ((TypeVariant == eVariantTypes.eString) && (StringValue.CompareTo("NOTFOUND") == 0));
        }

        private bool ComparingBooleanValues(TVariant v)
        {
            return ((ToString().ToLower() == "true") || (ToString().ToLower() == "yes") || (ToString().ToLower() == "false")
                    || (ToString().ToLower() == "no"))
                   && ((v.ToString().ToLower() == "true") || (v.ToString().ToLower() == "yes") || (v.ToString().ToLower() == "false")
                       || (v.ToString().ToLower() == "no"));
        }

        /// <summary>
        /// </summary>
        /// <returns>s 0 if equal, -1 if this object is less than the parameter, +1 if it is greater
        /// </returns>
        public System.Int16 CompareTo(TVariant v)
        {
            System.Int16 ReturnValue;
            ReturnValue = 0;

            if ((TypeVariant == eVariantTypes.eDecimal) || (TypeVariant == eVariantTypes.eInteger) || (TypeVariant == eVariantTypes.eInt64)
                || (TypeVariant == eVariantTypes.eCurrency))
            {
                if (ToDecimal() == v.ToDecimal())
                {
                    ReturnValue = 0;
                }
                else if (ToDecimal() < v.ToDecimal())
                {
                    ReturnValue = -1;
                }
                else
                {
                    ReturnValue = +1;
                }
            }
            else if (TypeVariant == eVariantTypes.eDateTime)
            {
                ReturnValue = (short)System.DateTime.Compare(ToDate(), v.ToDate());
            }
            else if (TypeVariant == eVariantTypes.eString)
            {
                // test if perhaps boolean values are compared; trouble is, yes and no and true and false are used
                if (ComparingBooleanValues(v))
                {
                    if (ToBool() == v.ToBool())
                    {
                        ReturnValue = 0;
                    }
                    else if (ToBool() == false)
                    {
                        ReturnValue = -1;
                    }
                    else
                    {
                        ReturnValue = 1;
                    }
                }
                else
                {
                    ReturnValue = (short)String.Compare(ToString(), v.ToString());
                }
            }
            else if (TypeVariant == eVariantTypes.eBoolean)
            {
                if (ToBool() == v.ToBool())
                {
                    ReturnValue = 0;
                }
                else if (ToBool() == false)
                {
                    ReturnValue = -1;
                }
                else
                {
                    ReturnValue = 1;
                }
            }
            else if (TypeVariant == eVariantTypes.eComposite)
            {
                // just compare the first elements
                ReturnValue = FirstCompositeValue().CompareTo(v.FirstCompositeValue());
            }

            return ReturnValue;
        }

        /// <summary>
        /// compare case insenstive
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public System.Int16 CompareToI(TVariant v)
        {
            if (TypeVariant == eVariantTypes.eString)
            {
                // test if perhaps boolean values are compared; trouble is, yes and no and true and false are used
                if (ComparingBooleanValues(v))
                {
                    return CompareTo(v);
                }
                else
                {
                    return (short)String.Compare(ToString().ToLower(), v.ToString().ToLower());
                }
            }
            else
            {
                return CompareTo(v);
            }
        }

        /// <summary>
        /// serialize TVariant as a string. This helps to avoid problems with .net Remoting and DateTime
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctx"></param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("encoded", this.EncodeToString());
        }

        /// <summary>
        /// serialize TVariant as a string. This helps to avoid problems with .net Remoting and DateTime
        /// </summary>
        protected TVariant(SerializationInfo info, StreamingContext ctx)
        {
            this.Assign(DecodeFromString(info.GetString("encoded")));
        }
    }
}