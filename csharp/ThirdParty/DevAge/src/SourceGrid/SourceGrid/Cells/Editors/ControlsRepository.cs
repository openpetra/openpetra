using System;
using System.Windows.Forms;

namespace SourceGrid
{
#if !MINI
	//la classe DictionaryBase non esisteva nel Compact Framework e non sono riuscia a riprodurla

	/// <summary>
	/// A dictionary with keys of type Guid and values of type Control
	/// </summary>
	public class ControlsRepository : System.Collections.DictionaryBase
	{
		private Control m_ParentControl;
		/// <summary>
		/// Initializes a new empty instance of the ControlsRepository class
		/// </summary>
		public ControlsRepository(Control p_ParentControl)
		{
			m_ParentControl = p_ParentControl;
		}

		/// <summary>
		/// Gets or sets the Control associated with the given Guid
		/// </summary>
		/// <param name="key">
		/// The Guid whose value to get or set.
		/// </param>
		public virtual Control this[Guid key]
		{
			get
			{
				return (Control) this.Dictionary[key];
			}
/*			set
			{
				this.Dictionary[key] = value;
			}*/
		}

		/// <summary>
		/// Adds an element with the specified key and value to this ControlsRepository.
		/// </summary>
		/// <param name="key">
		/// The Guid key of the element to add.
		/// </param>
		/// <param name="value">
		/// The Control value of the element to add.
		/// </param>
		public virtual void Add(Guid key, Control value)
		{
			this.Dictionary.Add(key, value);
			m_ParentControl.Controls.Add(value);
		}

		/// <summary>
		/// Determines whether this ControlsRepository contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Guid key to locate in this ControlsRepository.
		/// </param>
		/// <returns>
		/// true if this ControlsRepository contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(Guid key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this ControlsRepository contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Guid key to locate in this ControlsRepository.
		/// </param>
		/// <returns>
		/// true if this ControlsRepository contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(Guid key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this ControlsRepository contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The Control value to locate in this ControlsRepository.
		/// </param>
		/// <returns>
		/// true if this ControlsRepository contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsValue(Control value)
		{
			foreach (Control item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this ControlsRepository.
		/// </summary>
		/// <param name="key">
		/// The Guid key of the element to remove.
		/// </param>
		public virtual void Remove(Guid key)
		{
			if (ContainsKey(key))
			{
				m_ParentControl.Controls.Remove(this[key]);
				this.Dictionary.Remove(key);
			}
		}

		/// <summary>
		/// Gets a collection containing the keys in this ControlsRepository.
		/// </summary>
		public virtual System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this ControlsRepository.
		/// </summary>
		public virtual System.Collections.ICollection Values
		{
			get
			{
				return this.Dictionary.Values;
			}
		}
	}
#else
//TODO vedere se si riesce a riprodurre DictionaryBase per MINI

	/// <summary>
	/// A dictionary with keys of type Guid and values of type Control
	/// </summary>
	public class ControlsRepository : System.Collections.IDictionary
	{
		public void Add(object key, object value)
		{
			Add((Control)key, (Position)value);
		}
		public void Clear()
		{
			m_HashTable.Clear();
		}
		public bool Contains(object key)
		{
			return Contains((Control)key);
		}
		System.Collections.IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
		{
			return m_HashTable.GetEnumerator();
		}
		public void Remove(object key)
		{
			Remove((Control)key);
		}
		public bool IsFixedSize
		{
			get{return m_HashTable.IsFixedSize;}
		}

		public bool IsReadOnly
		{
			get{return m_HashTable.IsReadOnly;}
		}
		public object this[object key]
		{
			get{return m_HashTable[key];}
			set{m_HashTable[key] = value;}
		}

		public void CopyTo(Array array, int index)
		{
			m_HashTable.CopyTo(array, index);
		}
		public int Count
		{
			get{return m_HashTable.Count;} 
		}
		public bool IsSynchronized
		{
			get{return m_HashTable.IsSynchronized;}
		}
		public object SyncRoot
		{
			get{return m_HashTable.SyncRoot;}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return m_HashTable.GetEnumerator();
		}


		private System.Collections.Hashtable m_HashTable = new System.Collections.Hashtable();

		private Control m_ParentControl;
		/// <summary>
		/// Initializes a new empty instance of the ControlsRepository class
		/// </summary>
		public ControlsRepository(Control p_ParentControl)
		{
			m_ParentControl = p_ParentControl;
		}

		/// <summary>
		/// Gets or sets the Control associated with the given Guid
		/// </summary>
		/// <param name="key">
		/// The Guid whose value to get or set.
		/// </param>
		public virtual Control this[Guid key]
		{
			get
			{
				return (Control) m_HashTable[key];
			}
			/*			set
						{
							this.Dictionary[key] = value;
						}*/
		}

		/// <summary>
		/// Adds an element with the specified key and value to this ControlsRepository.
		/// </summary>
		/// <param name="key">
		/// The Guid key of the element to add.
		/// </param>
		/// <param name="value">
		/// The Control value of the element to add.
		/// </param>
		public virtual void Add(Guid key, Control value)
		{
			m_HashTable.Add(key, value);
			m_ParentControl.Controls.Add(value);
		}

		/// <summary>
		/// Determines whether this ControlsRepository contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Guid key to locate in this ControlsRepository.
		/// </param>
		/// <returns>
		/// true if this ControlsRepository contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(Guid key)
		{
			return m_HashTable.Contains(key);
		}

		/// <summary>
		/// Determines whether this ControlsRepository contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Guid key to locate in this ControlsRepository.
		/// </param>
		/// <returns>
		/// true if this ControlsRepository contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(Guid key)
		{
			return m_HashTable.Contains(key);
		}

		/// <summary>
		/// Determines whether this ControlsRepository contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The Control value to locate in this ControlsRepository.
		/// </param>
		/// <returns>
		/// true if this ControlsRepository contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsValue(Control value)
		{
			foreach (Control item in m_HashTable.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this ControlsRepository.
		/// </summary>
		/// <param name="key">
		/// The Guid key of the element to remove.
		/// </param>
		public virtual void Remove(Guid key)
		{
			if (ContainsKey(key))
			{
				m_ParentControl.Controls.Remove(this[key]);
				m_HashTable.Remove(key);
			}
		}

		/// <summary>
		/// Gets a collection containing the keys in this ControlsRepository.
		/// </summary>
		public virtual System.Collections.ICollection Keys
		{
			get
			{
				return m_HashTable.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this ControlsRepository.
		/// </summary>
		public virtual System.Collections.ICollection Values
		{
			get
			{
				return m_HashTable.Values;
			}
		}
	}
#endif
}
