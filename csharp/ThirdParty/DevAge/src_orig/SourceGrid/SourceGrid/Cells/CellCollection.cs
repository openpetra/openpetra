using System;

namespace SourceGrid
{
	/// <summary>
	/// A collection of elements of type Cells.ICellVirtual
	/// </summary>
	public class CellCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the CellBaseCollection class.
		/// </summary>
		public CellCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the CellBaseCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new CellBaseCollection.
		/// </param>
		public CellCollection(Cells.ICellVirtual[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the CellBaseCollection class, containing elements
		/// copied from another instance of CellBaseCollection
		/// </summary>
		/// <param name="items">
		/// The CellBaseCollection whose elements are to be added to the new CellBaseCollection.
		/// </param>
		public CellCollection(CellCollection items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this CellBaseCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this CellBaseCollection.
		/// </param>
		public virtual void AddRange(Cells.ICellVirtual[] items)
		{
			foreach (Cells.ICellVirtual item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another CellBaseCollection to the end of this CellBaseCollection.
		/// </summary>
		/// <param name="items">
		/// The CellBaseCollection whose elements are to be added to the end of this CellBaseCollection.
		/// </param>
		public virtual void AddRange(CellCollection items)
		{
			foreach (Cells.ICellVirtual item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type Cells.ICellVirtual to the end of this CellBaseCollection.
		/// </summary>
		/// <param name="value">
		/// The Cells.ICellVirtual to be added to the end of this CellBaseCollection.
		/// </param>
		public virtual void Add(Cells.ICellVirtual value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic Cells.ICellVirtual value is in this CellBaseCollection.
		/// </summary>
		/// <param name="value">
		/// The Cells.ICellVirtual value to locate in this CellBaseCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this CellBaseCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(Cells.ICellVirtual value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this CellBaseCollection
		/// </summary>
		/// <param name="value">
		/// The Cells.ICellVirtual value to locate in the CellBaseCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(Cells.ICellVirtual value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the CellBaseCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the Cells.ICellVirtual is to be inserted.
		/// </param>
		/// <param name="value">
		/// The Cells.ICellVirtual to insert.
		/// </param>
		public virtual void Insert(int index, Cells.ICellVirtual value)
		{
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the Cells.ICellVirtual at the given index in this CellBaseCollection.
		/// </summary>
		public virtual Cells.ICellVirtual this[int index]
		{
			get
			{
				return (Cells.ICellVirtual) this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific Cells.ICellVirtual from this CellBaseCollection.
		/// </summary>
		/// <param name="value">
		/// The Cells.ICellVirtual value to remove from this CellBaseCollection.
		/// </param>
		public virtual void Remove(Cells.ICellVirtual value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by CellBaseCollection.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="collection"></param>
			public Enumerator(CellCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			/// <summary>
			/// 
			/// </summary>
			public Cells.ICellVirtual Current
			{
				get
				{
					return (Cells.ICellVirtual) (this.wrapped.Current);
				}
			}

			/// <summary>
			/// 
			/// </summary>
			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (Cells.ICellVirtual) (this.wrapped.Current);
				}
			}

			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public bool MoveNext()
			{
				return this.wrapped.MoveNext();
			}

			/// <summary>
			/// 
			/// </summary>
			public void Reset()
			{
				this.wrapped.Reset();
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this CellBaseCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual CellCollection.Enumerator GetEnumerator()
		{
			return new CellCollection.Enumerator(this);
		}
	}
}
