using System;

namespace DevAge.ComponentModel.Validator
{
	/// <summary>
	/// The ValueMapping class can be used to easily map a value to a string value or a display string for conversion
	/// </summary>
	public class ValueMapping
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ValueMapping()
		{
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="validator"></param>
		/// <param name="valueList">A list of valid value. If null an error occurred. The index must match the index of ValueList, ObjectList and DisplayStringList</param>
		/// <param name="displayStringList">A list of displayString. Can be null. The index must match the index of ValueList, ObjectList and DisplayStringList</param>
		/// <param name="specialList">A list of object that can be converted to value. Can be null. The index must match the index of ValueList, ObjectList and DisplayStringList</param>
		/// <param name="specialType">The type of object stored in the specialList collection.</param>
		public ValueMapping(IValidator validator, System.Collections.IList valueList, System.Collections.IList displayStringList, System.Collections.IList specialList, Type specialType)
		{
			ValueList = valueList;
			DisplayStringList = displayStringList;
			SpecialList = specialList;
			if (validator != null)
				BindValidator(validator);
		}

		/// <summary>
		/// Bind the specified validator
		/// </summary>
		/// <param name="p_Validator"></param>
		public void BindValidator(IValidator p_Validator)
		{
			p_Validator.ConvertingValueToDisplayString += new ConvertingObjectEventHandler(p_Validator_ConvertingValueToDisplayString);
			p_Validator.ConvertingObjectToValue += new ConvertingObjectEventHandler(p_Validator_ConvertingObjectToValue);
			p_Validator.ConvertingValueToObject += new ConvertingObjectEventHandler(p_Validator_ConvertingValueToObject);
		}

		/// <summary>
		/// Unbind the specified validator
		/// </summary>
		/// <param name="p_Validator"></param>
		public void UnBindValidator(IValidator p_Validator)
		{
			p_Validator.ConvertingValueToDisplayString -= new ConvertingObjectEventHandler(p_Validator_ConvertingValueToDisplayString);
			p_Validator.ConvertingObjectToValue -= new ConvertingObjectEventHandler(p_Validator_ConvertingObjectToValue);
			p_Validator.ConvertingValueToObject -= new ConvertingObjectEventHandler(p_Validator_ConvertingValueToObject);
		}

		private System.Collections.IList m_ValueList;
		private Type mSpecialType = typeof(string);
		private System.Collections.IList mSpecialList;
		private System.Collections.IList m_DisplayStringList;

		/// <summary>
		/// A list of valid value. If null an error occurred. The index must match the index of ValueList and DisplayStringList
		/// </summary>
		public System.Collections.IList ValueList
		{
			get{return m_ValueList;}
			set{m_ValueList = value;}
		}
		/// <summary>
		/// A list of object that can be converted to value. Can be null. The index must match the index of ValueList and DisplayStringList. Must be a list of object of the type specified in the SpecialType property.
		/// Usually this property can be used when performing special conversion of specific type. For example if you want to map an enum value or an id value to a string for a better user experience.
		/// </summary>
		public System.Collections.IList SpecialList
		{
			get{return mSpecialList;}
			set{mSpecialList = value;}
		}

		/// <summary>
		/// Gets or sets the type used for converting an object to a value and a value to an object when populating the SpecialList property. Default is System.String.
		/// </summary>
		public Type SpecialType
		{
			get{return mSpecialType;}
			set{mSpecialType = value;}
		}

		/// <summary>
		/// A list of displayString. Can be null. The index must match the index of ValueList and DisplayStringList
		/// </summary>
		public System.Collections.IList DisplayStringList
		{
			get{return m_DisplayStringList;}
			set{m_DisplayStringList = value;}
		}

		private bool m_bThrowErrorIfNotFound = true;

		/// <summary>
		/// Gets or sets, if throw an error when the value if not found in one of the collections.
        /// Default true.
		/// </summary>
		public bool ThrowErrorIfNotFound
		{
			get{return m_bThrowErrorIfNotFound;}
			set{m_bThrowErrorIfNotFound = value;}
		}

		private void p_Validator_ConvertingValueToDisplayString(object sender, ConvertingObjectEventArgs e)
		{
			if (m_DisplayStringList != null)
			{
				if (m_ValueList == null)
					throw new ApplicationException("ValueList cannot be null");
				
				int l_Index = m_ValueList.IndexOf(e.Value);
				if (l_Index >= 0)
				{
					e.Value = m_DisplayStringList[l_Index];
					e.ConvertingStatus = ConvertingStatus.Completed;
				}
				else
				{
					if (m_bThrowErrorIfNotFound)
						e.ConvertingStatus = ConvertingStatus.Error;
				}
			}
		}

		private void p_Validator_ConvertingObjectToValue(object sender, ConvertingObjectEventArgs e)
		{
			if (mSpecialList != null && e.Value != null && e.Value.GetType() == SpecialType)
			{
				if (m_ValueList == null)
					throw new ApplicationException("ValueList cannot be null");
				
				//Verifico se fa parte della lista di valori
				int index = m_ValueList.IndexOf(e.Value);
				if (index >= 0)
				{
					e.Value = m_ValueList[index];
					e.ConvertingStatus = ConvertingStatus.Completed;
				}
				else
				{
					//Verifico se fa parte della lista di oggetti, in questo caso restituisco il valore corrispondente
					index = mSpecialList.IndexOf(e.Value);
					if (index >= 0)
					{
						e.Value = m_ValueList[index];
						e.ConvertingStatus = ConvertingStatus.Completed;
					}
					else if (m_bThrowErrorIfNotFound)
						e.ConvertingStatus = ConvertingStatus.Error;
				}
			}
		}

		private void p_Validator_ConvertingValueToObject(object sender, ConvertingObjectEventArgs e)
		{
			if (mSpecialList != null && e.DestinationType == SpecialType)
			{
				if (m_ValueList == null)
					throw new ApplicationException("ValueList cannot be null");
				
				//Verifico se il valore è presente nella list, in questo caso restituisco l'oggetto associato
				int l_Index = m_ValueList.IndexOf(e.Value);
				if (l_Index >= 0)
				{
					e.Value = mSpecialList[l_Index];
					e.ConvertingStatus = ConvertingStatus.Completed;
				}
				else
				{
					if (m_bThrowErrorIfNotFound)
						e.ConvertingStatus = ConvertingStatus.Error;
				}
			}
		}
	}
}
