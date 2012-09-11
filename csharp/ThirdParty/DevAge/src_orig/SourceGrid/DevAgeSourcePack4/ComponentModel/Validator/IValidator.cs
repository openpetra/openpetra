using System;

namespace DevAge.ComponentModel.Validator
{
	/// <summary>
	/// An interface to support value conversion and validation. 
	/// Naming Legend:
	/// Object = an object not yet converted for the current validator, 
	/// Value = an object already converted and valid for the current validator, 
	/// String = a string that can be used for conversion to and from Value, 
	/// DisplayString = a string representation of the Value
	/// </summary>
	public interface IValidator
	{
		#region Null
		/// <summary>
		/// True to allow null object value or NullString string Value
		/// </summary>
		bool AllowNull
		{
			get;
			set;
		}
		/// <summary>
		/// Null string representation. A string is null when is null or when is equals to this string. Default is empty string.
		/// Used by ValueToString and StringToValue
		/// </summary>
		string NullString
		{
			get;
			set;
		}
		/// <summary>
		/// Null string representation. A string is null when is null or when is equals to this string. Default is empty string.
		/// Used by ValueToDisplayString
		/// </summary>
		string NullDisplayString
		{
			get;
			set;
		}
		/// <summary>
		/// Returns true if the string is null or if is equals to the NullString
		/// </summary>
		/// <param name="p_str"></param>
		/// <returns></returns>
		bool IsNullString(string p_str);
		#endregion

		#region Conversion
		/// <summary>
		/// Convert an object according to the current ValueType of the validator
		/// </summary>
		/// <param name="p_Object"></param>
		/// <returns></returns>
		object ObjectToValue(object p_Object);
		/// <summary>
		/// Convert a value valid for the current validator ValueType to an object with the Type specified. Throw an exception on error.
		/// </summary>
		/// <param name="p_Value"></param>
		/// <param name="p_ReturnObjectType"></param>
		/// <returns></returns>
		object ValueToObject(object p_Value, Type p_ReturnObjectType);
		/// <summary>
		/// Convert a value valid for the current validator ValueType to a string that can be used for other conversions, for example StringToValue method.
		/// </summary>
		/// <param name="p_Value"></param>
		/// <returns></returns>
		string ValueToString(object p_Value);
		/// <summary>
		/// Converts a string to an object according to the type of the string editor
		/// </summary>
		/// <param name="p_str"></param>
		/// <returns></returns>
		object StringToValue(string p_str);
		/// <summary>
		/// Returns true if string conversion is suported. AllowStringConversion must be true and the current Validator must support string conversion.
		/// </summary>
		bool IsStringConversionSupported();
		/// <summary>
		/// Gets or Sets if the string conversion is allowed.
		/// </summary>
		bool AllowStringConversion
		{
			get;
			set;
		}
		#endregion

		#region DisplayString
		/// <summary>
		/// Converts a value valid for this validator valuetype to a string representation. The string cannot be used for conversion.
		/// If the validator support string conversion this method simply call ValueToString otherwise call Value.ToString()
		/// </summary>
		/// <param name="p_Value"></param>
		/// <returns></returns>
		string ValueToDisplayString(object p_Value);
		#endregion

		#region Validating
		/// <summary>
		/// Returns true if the value is valid for this type of editor without any conversion.
		/// </summary>
		/// <param name="p_Value"></param>
		/// <returns></returns>
		bool IsValidValue(object p_Value);
		/// <summary>
		/// Returns true if the object is valid for this type of validator, using conversion functions.
		/// </summary>
		/// <param name="p_Object"></param>
		/// <returns></returns>
		bool IsValidObject(object p_Object);
		/// <summary>
		/// Returns true if the object is valid for this type of validator, using conversion functions. Returns as parameter the value converted.
		/// </summary>
		/// <param name="p_Object"></param>
		/// <param name="p_ValueConverted"></param>
		/// <returns></returns>
		bool IsValidObject(object p_Object, out object p_ValueConverted);
		/// <summary>
		/// Returns true if the string is valid for this type of editor, using string conversion function.
		/// </summary>
		/// <param name="p_strValue"></param>
		/// <returns></returns>
		bool IsValidString(string p_strValue);
		/// <summary>
		/// Returns true if the string is valid for this type of editor, using string conversion function. An returns the object converted.
		/// </summary>
		/// <param name="p_strValue"></param>
		/// <param name="p_ValueConverted"></param>
		/// <returns></returns>
		bool IsValidString(string p_strValue, out object p_ValueConverted);
		#endregion

		#region Maximum/Minimum
		/// <summary>
		/// Minimum value allowed. If null no check is performed. The value must derive from IComparable interface to use Minimum or Maximum feature.
		/// </summary>
		object MinimumValue
		{
			get;
			set;
		}
		/// <summary>
		/// Maximum value allowed. If null no check is performed. The value must derive from IComparable interface to use Minimum or Maximum feature.
		/// </summary>
		object MaximumValue
		{
			get;
			set;
		}
		#endregion

		#region Type
		/// <summary>
		/// Type allowed for the current editor. Cannot be null.
		/// </summary>
		Type ValueType
		{
			get;
		}
		#endregion

		#region Events
		/// <summary>
		/// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
		/// </summary>
		event ConvertingObjectEventHandler ConvertingObjectToValue;
		/// <summary>
		/// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
		/// </summary>
		event ConvertingObjectEventHandler ConvertingValueToObject;
		/// <summary>
		/// Fired when converting a value to a display string. Called from method ValueToDisplayString
		/// </summary>
		event ConvertingObjectEventHandler ConvertingValueToDisplayString;
		#endregion

		#region Default Value
		/// <summary>
		/// Default value for this editor, usually is the default value for the specified type.
		/// </summary>
		object DefaultValue
		{
			get;
			set;
		}
		#endregion

		#region Standard Value Exclusive
		/// <summary>
		/// A list of values that this editor can support. If StandardValuesExclusive is true then the editor can only support one of these values.
		/// </summary>
		System.Collections.ICollection StandardValues
		{
			get;
			set;
		}
		/// <summary>
		/// If StandardValuesExclusive is true then the editor can only support the list specified in StandardValues.
		/// </summary>
		bool StandardValuesExclusive
		{
			get;
			set;
		}
		/// <summary>
		/// Returns true if the value specified is presents in the list StandardValues.
		/// </summary>
		/// <param name="p_Value"></param>
		/// <returns></returns>
		bool IsInStandardValues(object p_Value);

		/// <summary>
		/// Returns the standard values at the specified index. If StandardValues support IList use simple the indexer method otherwise loop troght the collection.
		/// </summary>
		/// <param name="p_Index"></param>
		/// <returns></returns>
		object StandardValueAtIndex(int p_Index);

		/// <summary>
		/// Returns the index of the specified standard value. -1 if not found. If StandardValues support IList use simple the indexer method otherwise loop troght the collection.
		/// </summary>
		/// <param name="p_StandardValue"></param>
		/// <returns></returns>
		int StandardValuesIndexOf(object p_StandardValue);
		#endregion

		#region Culture
		/// <summary>
		/// Culture for conversion. If null the default user culture is used. Default is null.
		/// </summary>
		System.Globalization.CultureInfo CultureInfo
		{
			get;
			set;
		}
		#endregion	

        #region Changed event
        /// <summary>
        /// Fired when one of the properties of the Validator change.
        /// </summary>
        event EventHandler Changed;
        #endregion
    }
}