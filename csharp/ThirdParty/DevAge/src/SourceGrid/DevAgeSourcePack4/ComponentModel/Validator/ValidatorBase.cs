using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace DevAge.ComponentModel.Validator
{
	/// <summary>
	/// A base class to support value conversion and validation. This class is used if no conversion is required or as a base class for specialized validator.
	/// Naming Legend:
	/// Object = an object not yet converted for the current validator, 
	/// Value = an object already converted and valid for the current validator, 
	/// String = a string that can be used for conversion to and from Value, 
	/// DisplayString = a string representation of the Value
	/// </summary>
    public class ValidatorBase : ComponentLight, IValidator
	{
		#region Constructor
		/// <summary>
		/// Constructor. Initialize the class using a null type.
		/// </summary>
		public ValidatorBase()
		{
			ValueType = null;
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type">The type used to validate the values. If null no validation is made.</param>
		public ValidatorBase(Type type)
		{
			ValueType = type;
		}

		protected virtual void OnLoadingValueType()
		{
			if (ValueType != null)
			{
				if (ValueType.IsValueType)
				{
					m_bAllowNull = false;
					if (ValueType.IsEnum)
					{
						System.Reflection.FieldInfo[] fields = ValueType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
						m_DefaultValue = fields[0].GetValue(null);
					}
					else
					{
						m_DefaultValue = Activator.CreateInstance(ValueType);
					}
				}
				else
				{
					m_DefaultValue = null;
					m_bAllowNull = true;
				}

				m_StandardValues = null;
				m_bStandardValuesExclusive = false;

				m_MaximumValue = null;
				m_MinimumValue = null;
				m_sNullString = "";
				m_sNullDisplayString = "";
			}
			else
			{
				m_bAllowNull = true;
				m_DefaultValue = null;
				m_StandardValues = null;
				m_bStandardValuesExclusive = false;
				m_MaximumValue = null;
				m_MinimumValue = null;
				m_sNullString = "";
				m_sNullDisplayString = "";
			}
		}
		#endregion

		#region Null
		private bool m_bAllowNull;
		/// <summary>
		/// True to allow null object value or NullString string Value
		/// </summary>
		[DefaultValue(true), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AllowNull
		{
			get{return m_bAllowNull;}
			set
            {
                if (m_bAllowNull != value)
                {
                    m_bAllowNull = value;
                    OnChanged(EventArgs.Empty);
                }
            }
		}
		private string m_sNullString;
		/// <summary>
		/// Null string representation. A string is null when is null or when is equals to this string. Default is empty string.
		/// Used by ValueToString and StringToValue
		/// </summary>
		[DefaultValue(""), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string NullString
		{
			get{return m_sNullString;}
			set
            {
                if (m_sNullString != value)
                {
                    m_sNullString = value;
                    OnChanged(EventArgs.Empty);
                }
            }
		}
		private string m_sNullDisplayString;
		/// <summary>
		/// Null string representation. Default is empty string.
		/// Used by ValueToDisplayString
		/// </summary>
		[DefaultValue(""), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string NullDisplayString
		{
			get{return m_sNullDisplayString;}
			set
            {
                if (m_sNullDisplayString != value)
                {
                    m_sNullDisplayString = value;
                    OnChanged(EventArgs.Empty);
                }
            }
		}
		/// <summary>
		/// Returns true if the string is null or if is equals to the NullString
		/// </summary>
		/// <param name="p_str"></param>
		/// <returns></returns>
		public virtual bool IsNullString(string p_str)
		{
			return (p_str == null || p_str == m_sNullString);
		}
		#endregion

		#region Error string
		/// <summary>
		/// Returns a string used for error description for a specified object. Usually used when printing the object for the error message when there is a conversion error.
		/// </summary>
		/// <param name="val"></param>
		public virtual string ObjectToStringForError(object val)
		{
			try
			{
				if (val == null)
					return "<null>";
				else
					return val.ToString();
			}
			catch(Exception)
			{
				return "<object>";
			}
		}
		#endregion

		#region Conversion
		/// <summary>
		/// Convert an object according to the current ValueType of the validator
		/// </summary>
		/// <param name="p_Object"></param>
		/// <returns></returns>
		public object ObjectToValue(object p_Object)
		{
			Type destinationType = ValueType;
			if (destinationType == null && p_Object != null)
				destinationType = p_Object.GetType();

			ConvertingObjectEventArgs l_Converting = new ConvertingObjectEventArgs(p_Object, destinationType);
			OnConvertingObjectToValue(l_Converting);
			if (l_Converting.ConvertingStatus == ConvertingStatus.Error)
				throw new ConversionErrorException(ValueTypeName, ObjectToStringForError(l_Converting.Value) );

			if (IsValidValue(l_Converting.Value))
				return l_Converting.Value;
			else
				throw new ConversionErrorException(ValueTypeName, ObjectToStringForError(l_Converting.Value) );
		}

		/// <summary>
		/// Convert a value according to the current ValueType to an object with the Type specified. Throw an exception on error.
		/// </summary>
		/// <param name="p_Value"></param>
		/// <param name="p_ReturnObjectType"></param>
		/// <returns></returns>
		public object ValueToObject(object p_Value, Type p_ReturnObjectType)
		{
			if (p_ReturnObjectType == null)
				throw new DevAgeApplicationException("Invalid parameter returnObjectType cannot be null");

			ConvertingObjectEventArgs l_Converting = new ConvertingObjectEventArgs(p_Value, p_ReturnObjectType);
			OnConvertingValueToObject(l_Converting);
			if (l_Converting.ConvertingStatus == ConvertingStatus.Error)
				throw new ConversionErrorException(p_ReturnObjectType.Name, ObjectToStringForError(l_Converting.Value) );

			if (l_Converting.Value == null)
				return null;
			else if (l_Converting.DestinationType.IsAssignableFrom(l_Converting.Value.GetType()))
				return l_Converting.Value;
			else
				throw new ConversionErrorException(p_ReturnObjectType.Name, ObjectToStringForError(l_Converting.Value) );
		}
		/// <summary>
		/// Converts a value object to a string representation
		/// </summary>
		/// <param name="p_Value"></param>
		/// <returns></returns>
		public string ValueToString(object p_Value)
		{
			object tmp = ValueToObject(p_Value, typeof(string));
			if (tmp == null)
				return null;
			else
				return (string)tmp;
		}
		/// <summary>
		/// Converts a string to an object according to the type of the string editor
		/// </summary>
		/// <param name="p_str"></param>
		/// <returns></returns>
		public object StringToValue(string p_str)
		{
			return ObjectToValue(p_str);
		}

		private bool m_bAllowStringConversion = true;

		/// <summary>
		/// Gets or Sets if the string conversion is allowed.
		/// </summary>
		[DefaultValue(true), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AllowStringConversion
		{
			get{return m_bAllowStringConversion;}
			set
            {
                if (m_bAllowStringConversion != value)
                {
                    m_bAllowStringConversion = value;
                    OnChanged(EventArgs.Empty);
                }
            }
		}

		/// <summary>
		/// Returns true if string conversion is suported. AllowStringConversion must be true and the current Validator must support string conversion.
		/// </summary>
		public virtual bool IsStringConversionSupported()
		{
			return (AllowStringConversion && typeof(string) == ValueType) ||
					ValueType == null;
		}
		#endregion

		#region DisplayString
		/// <summary>
		/// Converts a value valid for this validator valuetype to a string representation. The string cannot be used for conversion.
		/// If the validator support string conversion this method simply call ValueToString otherwise call Value.ToString()
		/// </summary>
		/// <param name="p_Value"></param>
		/// <returns></returns>
		public virtual string ValueToDisplayString(object p_Value)
		{
			ConvertingObjectEventArgs l_Converting = new ConvertingObjectEventArgs(p_Value, typeof(string));
			OnConvertingValueToDisplayString(l_Converting);
			if (l_Converting.ConvertingStatus == ConvertingStatus.Error)
				throw new ConversionErrorException("display String", ObjectToStringForError(l_Converting.Value) );

			if (l_Converting.Value == null)
				return NullDisplayString;
			else if (l_Converting.Value is string)
				return (string)l_Converting.Value;
			else
				throw new ConversionErrorException("display String", ObjectToStringForError(l_Converting.Value) );
		}
		#endregion

		#region Events
		/// <summary>
		/// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
		/// </summary>
		private ConvertingObjectEventHandler m_ConvertingObjectToValue;
		/// <summary>
		/// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
		/// </summary>
        private ConvertingObjectEventHandler m_ConvertingValueToObject;
		/// <summary>
		/// Fired when converting a value to a display string. Called from method ValueToDisplayString
		/// </summary>
        private ConvertingObjectEventHandler m_ConvertingValueToDisplayString;

		/// <summary>
		/// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
		/// </summary>
		public event ConvertingObjectEventHandler ConvertingObjectToValue
		{
			add{m_ConvertingObjectToValue += value;}
			remove{m_ConvertingObjectToValue -= value;}
		}
		/// <summary>
		/// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
		/// </summary>
		public event ConvertingObjectEventHandler ConvertingValueToObject
		{
			add{m_ConvertingValueToObject += value;}
			remove{m_ConvertingValueToObject -= value;}
		}
		/// <summary>
		/// Fired when converting a value to a display string. Called from method ValueToDisplayString
		/// </summary>
		public event ConvertingObjectEventHandler ConvertingValueToDisplayString
		{
			add{m_ConvertingValueToDisplayString += value;}
			remove{m_ConvertingValueToDisplayString -= value;}
		}


		/// <summary>
		/// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnConvertingObjectToValue(ConvertingObjectEventArgs e)
		{
			if (m_ConvertingObjectToValue != null)
				m_ConvertingObjectToValue(this, e);

			if (e.ConvertingStatus == ConvertingStatus.Error)
				throw new ConversionErrorException(e.DestinationType.Name, ObjectToStringForError(e.Value) );
			else if (e.ConvertingStatus == ConvertingStatus.Completed)
				return;

			if (e.Value is string) //è importante fare prima il caso stringa per gestire correttamente il null
			{
				string tmp = (string)e.Value;
				if (IsNullString(tmp))
					e.Value = null;
			}			
		}
		/// <summary>
		/// Fired when converting a object to the value specified. Called from method ObjectToValue and IsValidObject
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnConvertingValueToObject(ConvertingObjectEventArgs e)
		{
			if (m_ConvertingValueToObject != null)
				m_ConvertingValueToObject(this, e);

			if (e.ConvertingStatus == ConvertingStatus.Error)
				throw new ConversionErrorException(e.DestinationType.Name, ObjectToStringForError(e.Value) );
			else if (e.ConvertingStatus == ConvertingStatus.Completed)
				return;
		}

		/// <summary>
		/// Fired when converting a value to a display string. Called from method ValueToDisplayString
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnConvertingValueToDisplayString(ConvertingObjectEventArgs e)
		{
			if (m_ConvertingValueToDisplayString != null)
				m_ConvertingValueToDisplayString(this, e);

			if (e.ConvertingStatus == ConvertingStatus.Error)
				throw new ConversionErrorException("display String", ObjectToStringForError(e.Value));
			else if (e.ConvertingStatus == ConvertingStatus.Completed)
				return;

			if (e.Value == null)
				e.Value = NullDisplayString;
			else if (IsStringConversionSupported())
				e.Value = ValueToString(e.Value);
			else
				e.Value = e.Value.ToString();
		}
		#endregion

		#region Validating
		/// <summary>
		/// Returns true if the value is valid for this type of editor without any conversion.
		/// </summary>
		/// <param name="p_Value"></param>
		/// <returns></returns>
		public bool IsValidValue(object p_Value)
		{
			try
			{
				if (IsInStandardValues(p_Value))
					return true;

				if (p_Value == null)
				{
					if (AllowNull)
						return true;
					else
						return false;
				}

				if (m_bStandardValuesExclusive)
					return false;

				if (m_MaximumValue != null)
				{
					IComparable l_Max = (IComparable)m_MaximumValue;
					if (l_Max.CompareTo(p_Value) < 0)
						return false;
				}

				if (m_MinimumValue != null)
				{
					IComparable l_Min = (IComparable)m_MinimumValue;
					if (l_Min.CompareTo(p_Value) > 0)
						return false;
				}

				if (ValueType != null)
					return ValueType.IsAssignableFrom(p_Value.GetType());
				else
					return true;
			}
			catch(Exception)
			{
				return false;
			}
		}
		/// <summary>
		/// Returns true if the object is valid for this type of validator, using conversion functions.
		/// </summary>
		/// <param name="p_Object"></param>
		/// <returns></returns>
		public bool IsValidObject(object p_Object)
		{
			object dummy;
			return IsValidObject(p_Object, out dummy);
		}
		/// <summary>
		/// Returns true if the object is valid for this type of validator, using conversion functions. Returns as parameter the value converted.
		/// </summary>
		/// <param name="p_Object"></param>
		/// <param name="p_ValueConverted"></param>
		/// <returns></returns>
		public bool IsValidObject(object p_Object, out object p_ValueConverted)
		{
			p_ValueConverted = null;
			try
			{
				p_ValueConverted = ObjectToValue(p_Object);
				return IsValidValue(p_ValueConverted);
			}
			catch(Exception)
			{
				return false;
			}		
		}
		/// <summary>
		/// Returns true if the string is valid for this type of editor, using string conversion function.
		/// </summary>
		/// <param name="p_strValue"></param>
		/// <returns></returns>
		public bool IsValidString(string p_strValue)
		{
			object dummy;
			return IsValidString(p_strValue, out dummy);
		}
		/// <summary>
		/// Returns true if the string is valid for this type of editor, using string conversion function. Returns as out parameter the object converted.
		/// </summary>
		/// <param name="p_strValue"></param>
		/// <param name="p_ValueConverted"></param>
		/// <returns></returns>
		public bool IsValidString(string p_strValue, out object p_ValueConverted)
		{
			return IsValidObject(p_strValue, out p_ValueConverted);
		}
		#endregion

		#region Maximum/Minimum
		private object m_MinimumValue = null;
		/// <summary>
		/// Minimum value allowed. If null no check is performed. The value must derive from IComparable interface to use Minimum or Maximum feature.
		/// </summary>
		[DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object MinimumValue
		{
			get{return m_MinimumValue;}
			set
            {
                if (m_MinimumValue != value)
                {
                    m_MinimumValue = value;
                    OnChanged(EventArgs.Empty);
                }
            }
		}
		private object m_MaximumValue = null;
		/// <summary>
		/// Maximum value allowed. If null no check is performed. The value must derive from IComparable interface to use Minimum or Maximum feature.
		/// </summary>
		[DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object MaximumValue
		{
			get{return m_MaximumValue;}
			set
            {
                if (m_MaximumValue != value)
                {
                    m_MaximumValue = value;
                    OnChanged(EventArgs.Empty);
                }
            }
		}
		#endregion

		#region Type
		private Type m_ValueType;
		/// <summary>
		/// Type allowed for the current editor. Cannot be null.
		/// </summary>
		[DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Type ValueType
		{
			get{return m_ValueType;}
			set
            {
                if (m_ValueType != value)
                {
                    m_ValueType = value;
                    OnLoadingValueType();
                    OnChanged(EventArgs.Empty);
                }
            }
		}

		/// <summary>
		/// Set the ValueType property using a string value, usually used by designer code generation.
		/// The designer for this type is configured to use only this property.
		/// </summary>
		[DefaultValue(""), Browsable(true)]
		public string ValueTypeName
		{
			get
			{
				if (ValueType == null)
					return string.Empty;
				else
					return ValueType.AssemblyQualifiedName;
			}
			set
			{
				if (value == null || value.Trim().Length == 0)
					ValueType = null;
				else
					ValueType = Type.GetType(value, true, true);
			}
		}
		#endregion

		#region Default Value
		private object m_DefaultValue;
		/// <summary>
		/// Default value for this editor, usually is the default value for the specified type.
		/// </summary>
		[DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object DefaultValue
		{
			get{return m_DefaultValue;}
			set
            {
                if (m_DefaultValue != value)
                {
                    m_DefaultValue = value;
                    OnChanged(EventArgs.Empty);
                }
            }
		}
		#endregion

		#region Standard Value Exclusive
		private System.Collections.ICollection m_StandardValues;
		/// <summary>
		/// A list of values that this editor can support. If StandardValuesExclusive is true then the editor can only support one of these values.
		/// </summary>
		[DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public System.Collections.ICollection StandardValues
		{
			get{return m_StandardValues;}
			set
            {
                if (m_StandardValues != value)
                {
                    m_StandardValues = value;
                    OnChanged(EventArgs.Empty);
                }
            }
		}
		private bool m_bStandardValuesExclusive;
		/// <summary>
		/// If StandardValuesExclusive is true then the editor can only support the list specified in StandardValues.
		/// </summary>
		[DefaultValue(false), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool StandardValuesExclusive
		{
			get{return m_bStandardValuesExclusive;}
			set
            {
                if (m_bStandardValuesExclusive != value)
                {
                    m_bStandardValuesExclusive = value;
                    OnChanged(EventArgs.Empty);
                }
            }
		}
		/// <summary>
		/// Returns true if the value specified is presents in the list StandardValues.
		/// </summary>
		/// <param name="p_Value"></param>
		/// <returns></returns>
		public virtual bool IsInStandardValues(object p_Value)
		{
			if (m_StandardValues == null)
				return false;

			foreach (object l_ListVal in m_StandardValues)
			{
				if ( (l_ListVal == null && p_Value == null) || 
					(l_ListVal != null && l_ListVal.Equals(p_Value))  )
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the standard values at the specified index. If StandardValues support IList use simple the indexer method otherwise loop troght the collection.
		/// </summary>
		/// <param name="p_Index"></param>
		/// <returns></returns>
		public virtual object StandardValueAtIndex(int p_Index)
		{
			if (m_StandardValues == null)
				throw new DevAgeApplicationException("StandardValues is null");

			System.Collections.IList l_List = m_StandardValues as System.Collections.IList;
			if (l_List != null)
				return l_List[p_Index];
			else
			{
				int l_CurrentIndex = 0;
				foreach (object o in m_StandardValues)
				{
					if (l_CurrentIndex == p_Index)
						return o;

					l_CurrentIndex++;
				}

				throw new DevAgeApplicationException("Invalid Index");
			}
		}

		/// <summary>
		/// Returns the index of the specified standard value. -1 if not found. If StandardValues support IList use simple the indexer method otherwise loop troght the collection.
		/// </summary>
		/// <param name="p_StandardValue"></param>
		/// <returns></returns>
		public virtual int StandardValuesIndexOf(object p_StandardValue)
		{
			if (m_StandardValues == null)
				throw new DevAgeApplicationException("StandardValues is null");

			System.Collections.IList l_List = m_StandardValues as System.Collections.IList;
			if (l_List != null)
				return l_List.IndexOf(p_StandardValue);
			else
			{
				int l_CurrentIndex = 0;
				foreach (object o in m_StandardValues)
				{
					if (o == null && p_StandardValue == null)
						return l_CurrentIndex;
					else if (o != null)
					{
						if (o.Equals(p_StandardValue))
							return l_CurrentIndex;
					}

					l_CurrentIndex++;
				}

				return -1;
			}
		}
		#endregion

		#region Culture
		private System.Globalization.CultureInfo m_CultureInfo = null;

		/// <summary>
		/// Culture for conversion. If null the default user culture is used. Default is null.
		/// </summary>
		[DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public System.Globalization.CultureInfo CultureInfo
		{
			get{return m_CultureInfo;}
			set
            {
                if (m_CultureInfo != value)
                {
                    m_CultureInfo = value;
                    OnChanged(EventArgs.Empty);
                }
            }
		}
		#endregion

		#region Component methods
		protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);
		}
		#endregion

        #region Changed event
        /// <summary>
        /// Fired when one of the properties of the Validator change.
        /// Call the Changed event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChanged(EventArgs e)
        {
            if (mChanged != null)
                mChanged(this, e);
        }

        private EventHandler mChanged;
        /// <summary>
        /// Fired when one of the properties of the Validator change.
        /// </summary>
        public event EventHandler Changed
        {
            add { mChanged += value; }
            remove { mChanged -= value; }
        }
        #endregion
    }
}
