using System;
using System.ComponentModel;

namespace SourceGrid.Utils
{

	
	/// <summary>
	/// A generic converter class used to convert values from
	/// one type to another
	/// </summary>
	public static class SourceGridConvert
	{
		/// <summary>
		/// Converts specified value to type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">Type to convert to.</typeparam>
		/// <param name="value">Value to convert.</param>
		/// <returns>Converted value.</returns>
		public static T To<T>(object value)
		{
			try
			{
				object convertedValue = To(value, typeof(T));
				return convertedValue != null ? (T)convertedValue : default(T);
			}
			catch (FormatException)
			{
				return default(T);
			}
		}
		
		/// <summary>
		/// Converts specified value to the specified type.
		/// </summary>
		/// <param name="value">Value to convert.</param>
		/// <param name="type">Type to convert to.</param>
		/// <returns>Converted value.</returns>
		public static object To(object value, Type type)
		{
			// if a value is null - just return null
			if (value == null)
				return null;
			
			// if a value is of the specified type - just return it
			if (value.GetType() == type)
				return value;
			
			// do we have a nullable type?
			if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				// if the value is of type string, we handle empty string like null value
				if (value is string && value.ToString() == string.Empty)
					return null;
				
				var nullableConverter = new NullableConverter(type);
				type = nullableConverter.UnderlyingType;
			}
			
			// if we have an enumeration, need to use the Parse()
			if (type.IsEnum && Enum.IsDefined(type, value))
				return Enum.Parse(type, value.ToString(), false);
			
			TypeConverter typeConverter = TypeDescriptor.GetConverter(type);
			
			// if we have a custom type converter then use it
			if (typeConverter.CanConvertFrom(value.GetType()))
				return typeConverter.ConvertFrom(value);
			
			typeConverter = TypeDescriptor.GetConverter(value.GetType());
			
			// if ConvertFrom is not usable, try the other way around
			if (typeConverter.CanConvertTo(type))
				return typeConverter.ConvertTo(value, type);
			
			// otherwise use the ChangeType()
			return System.Convert.ChangeType(value, type);
		}
	}
}
