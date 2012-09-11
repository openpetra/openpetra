using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

using SourceGrid.Utils;

namespace DevAge.ComponentModel.Validator
{
	/// <summary>
	/// A string editor that use a TypeConverter for conversion.
	/// </summary>
	public class ValidatorTypeConverter : ValidatorBase
	{
		#region Constructor
		/// <summary>
		/// Constructor. Initialize the Validator with a null TypeConverter.
		/// </summary>
		public ValidatorTypeConverter()
		{
			m_TypeConverter = null;
		}

		/// <summary>
		/// Constructor. If the Type doesn't implements a TypeConverter no conversion is made.
		/// </summary>
		/// <param name="p_Type"></param>
		public ValidatorTypeConverter(Type p_Type):base(p_Type)
		{
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_Type">Cannot be null.</param>
		/// <param name="p_TypeConverter">Can be null to don't allow any conversion.</param>
		public ValidatorTypeConverter(Type p_Type, System.ComponentModel.TypeConverter p_TypeConverter):base(p_Type)
		{
			TypeConverter = p_TypeConverter;
		}
		#endregion

		#region Conversion
		/// <summary>
		/// Returns true if string conversion is suported. AllowStringConversion must be true and the current Validator must support string conversion.
		/// </summary>
		public override bool IsStringConversionSupported()
		{
			if (typeof(string).IsAssignableFrom(ValueType))
				return AllowStringConversion;

			if (m_TypeConverter != null)
				return AllowStringConversion && m_TypeConverter.CanConvertFrom(typeof(string)) && m_TypeConverter.CanConvertTo(typeof(string));
			else
				return base.AllowStringConversion;
		}

		/// <summary>
		/// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
		/// </summary>
		/// <param name="e"></param>
		protected override void OnConvertingObjectToValue(ConvertingObjectEventArgs e)
		{
			base.OnConvertingObjectToValue(e);

			if (e.ConvertingStatus == ConvertingStatus.Error)
				throw new ApplicationException("Invalid conversion");
			else if (e.ConvertingStatus == ConvertingStatus.Completed)
				return;

			if (e.Value == null)
			{
			}
			else if (e.Value is string) //è importante fare prima il caso stringa per gestire correttamente il null
			{
				string tmp = (string)e.Value;
				if (IsNullString(tmp))
					e.Value = null;
				else if (e.DestinationType.IsAssignableFrom(e.Value.GetType())) //se la stringa non è nulla e il tipo di destinazione è sempre una string allora non faccio nessuna conversione
				{
				}
				else if (IsStringConversionSupported())
					e.Value = m_TypeConverter.ConvertFromString(EmptyTypeDescriptorContext.Empty, CultureInfo, tmp);
				else
					throw new ApplicationException("String conversion not supported for this type of Validator.");
			}
			else if (e.DestinationType.IsAssignableFrom(e.Value.GetType()))
			{
			}
			else if (m_TypeConverter != null)
			{
				// For some reason string converter does not allow converting from
				// double to string. So here is just override with simple if statemenet
				if (m_TypeConverter is StringConverter)
					e.Value = SourceGridConvert.To<string>(e.Value);
				else
					// otherwise just do normal conversion
					e.Value = m_TypeConverter.ConvertFrom(EmptyTypeDescriptorContext.Empty, CultureInfo, e.Value);
			}
		}
		/// <summary>
		/// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
		/// </summary>
		/// <param name="e"></param>
		protected override void OnConvertingValueToObject(ConvertingObjectEventArgs e)
		{
			base.OnConvertingValueToObject(e);

			if (e.ConvertingStatus == ConvertingStatus.Error)
				throw new ApplicationException("Invalid conversion");
			else if (e.ConvertingStatus == ConvertingStatus.Completed)
				return;

			if (e.Value == null)
			{
			}
			else if (e.DestinationType.IsAssignableFrom(e.Value.GetType()))
			{
			}
			else if (e.DestinationType == typeof(string) && IsStringConversionSupported() == false)
			{
				throw new ApplicationException("String conversion not supported for this type of Validator.");
			}
			else if (m_TypeConverter != null)
			{
				e.Value = m_TypeConverter.ConvertTo(EmptyTypeDescriptorContext.Empty, CultureInfo, e.Value, e.DestinationType);
			}
		}
		#endregion

		#region TypeConverter

		private System.ComponentModel.TypeConverter m_TypeConverter;
		/// <summary>
		/// TypeConverter used for this type editor, cannot be null.
		/// </summary>
		[DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public System.ComponentModel.TypeConverter TypeConverter
		{
			get{return m_TypeConverter;}
			set
			{
				if (m_TypeConverter != value)
				{
					m_TypeConverter = value;
					OnLoadingTypeConverter();
					OnChanged(EventArgs.Empty);
				}
			}
		}

		private void OnLoadingTypeConverter()
		{
			StandardValues = null;
			StandardValuesExclusive = false;

			//Populate properties using TypeConverter
			if (m_TypeConverter != null)
			{
				StandardValues = m_TypeConverter.GetStandardValues();
				if (StandardValues != null && StandardValues.Count > 0)
					StandardValuesExclusive = m_TypeConverter.GetStandardValuesExclusive();
				else
					StandardValuesExclusive = false;
			}
		}

		protected override void OnLoadingValueType()
		{
			base.OnLoadingValueType ();

			if (ValueType != null)
				TypeConverter = System.ComponentModel.TypeDescriptor.GetConverter(ValueType);
			else
				TypeConverter = null;
		}
		#endregion
	}
}
