using System;

namespace DevAge.Configuration
{
	public class PersistableItem
	{
		private ComponentModel.Validator.IValidator m_Validator;
		private Type m_Type;
		private string m_Name;
		private object m_Value;
		private object m_DefaultValue;

		public PersistableItem(Type pType, string pName, object pDefaultValue)
		{
			m_Validator = new ComponentModel.Validator.ValidatorTypeConverter(pType);
			//N.B. I have used invariant culture to ensure to correct parsing and transformation with a standard (english) format
			m_Validator.CultureInfo = System.Globalization.CultureInfo.InvariantCulture;

			m_Type = pType;
			m_Name = pName;
			m_Value = pDefaultValue;
			m_DefaultValue = pDefaultValue;
		}

		public ComponentModel.Validator.IValidator Validator
		{
			get{return m_Validator;}
		}
		public Type Type
		{
			get{return m_Type;}
		}
		public string Name
		{
			get{return m_Name;}
		}
		public object Value
		{
			get{return m_Value;}
			set{m_Value = value;}
		}
		public object DefaultValue
		{
			get{return m_DefaultValue;}
			set{m_DefaultValue = value;}
		}

		public bool IsChanged
		{
			get
			{
				if (m_Value == null && m_DefaultValue == null)
					return false;
				if (m_Value == null)
					return true;

				return !(m_Value.Equals(m_DefaultValue));
			}
		}

		public void AcceptAsDefault()
		{
			m_DefaultValue = m_Value;
		}

		public void Reset()
		{
			m_Value = m_DefaultValue;
		}

		public override string ToString()
		{
			return "PersistableItem: " + Name + "=" + Validator.ValueToDisplayString(Value);
		}

	}
}