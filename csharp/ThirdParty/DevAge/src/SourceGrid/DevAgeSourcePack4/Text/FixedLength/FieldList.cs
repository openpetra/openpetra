using System;

namespace DevAge.Text.FixedLength
{
	/// <summary>
	/// A dictionary with keys of type string and values of type IField
	/// </summary>
	public class FieldList: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the FieldList class
		/// </summary>
		public FieldList()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the IField associated with the given string
		/// </summary>
		/// <param name="key">
		/// The string whose value to get or set.
		/// </param>
		public virtual IField this[string key]
		{
			get
			{
				return (IField) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this FieldList.
		/// </summary>
		/// <param name="value">
		/// The IField value of the element to add.
		/// </param>
		public virtual void Add(IField value)
		{
			this.Dictionary.Add(value.Name, value);
		}

		/// <summary>
		/// Determines whether this FieldList contains a specific key.
		/// </summary>
		/// <param name="fieldName">
		/// The string key to locate in this FieldList.
		/// </param>
		/// <returns>
		/// true if this FieldList contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(string fieldName)
		{
			return this.Dictionary.Contains(fieldName);
		}

		/// <summary>
		/// Determines whether this FieldList contains a specific key.
		/// </summary>
		/// <returns>
		/// true if this FieldList contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(string fieldName)
		{
			return this.Dictionary.Contains(fieldName);
		}

		/// <summary>
		/// Determines whether this FieldList contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The IField value to locate in this FieldList.
		/// </param>
		/// <returns>
		/// true if this FieldList contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsValue(IField value)
		{
			foreach (IField item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this FieldList.
		/// </summary>
		/// <param name="fieldName">
		/// The string key of the element to remove.
		/// </param>
		public virtual void Remove(string fieldName)
		{
			this.Dictionary.Remove(fieldName);
		}

		/// <summary>
		/// Gets a collection containing the keys in this FieldList.
		/// </summary>
		public virtual System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this FieldList.
		/// </summary>
		public virtual System.Collections.ICollection Values
		{
			get
			{
				return this.Dictionary.Values;
			}
		}

		public IField[] GetSortedList()
		{
			IField[] fields = new IField[Count];
			for (int i = 0; i < fields.Length; i++)
			{
				foreach (IField fld in Values)
				{
					if (fld.Index == i)
					{
						fields[i] = fld;
						break;
					}
				}
				if (fields[i] == null)
					throw new FieldNotDefinedException(i);
			}

			return fields;
		}
	}

}
