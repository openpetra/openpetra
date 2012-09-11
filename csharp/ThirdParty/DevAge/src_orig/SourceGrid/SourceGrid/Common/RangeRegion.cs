using System;
using System.Collections.Generic;
using SourceGrid.Selection;

namespace SourceGrid
{
	/// <summary>
	/// RangeRegion is a collection of range that are never overlying each other.
	/// </summary>
	[Serializable]
	public class RangeRegion : ICollection<Range>
	{
		#region Constructors
		public RangeRegion()
		{
		}
		public RangeRegion(Position position)
		{
			if (position.IsEmpty() == false)
				m_RangeCollection.Add(new Range(position));
		}
		public RangeRegion(Range range)
		{
			if (range.IsEmpty() == false)
				m_RangeCollection.Add(range);
		}
		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="other"></param>
		public RangeRegion(RangeRegion other)
		{
			m_RangeCollection.AddRange(other.m_RangeCollection);
		}
		#endregion

		//N.B. Il codice di questa classe è scritto in maniera tale da non avere mai sovrapposizioni.
		// Quindi una cella potrà essere contenuta in un solo range.
		// Nel caso vengano inseririti range sovrapposti questi vengono spezzati e reinseriti.
		private RangeCollection m_RangeCollection = new RangeCollection();

		#region IsEmpty
		public virtual bool IsEmpty()
		{
			if (m_RangeCollection.Count == 0)
				return true;
			else
				return false;
		}
		#endregion

		#region GetCellsPositions, GetRanges

		/// <summary>
		/// Returns the range at the specific position
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Range this[int index]
		{
			get { return m_RangeCollection[index]; }
		}

		/// <summary>
		/// Returns a Collection of cells that represents the current class
		/// </summary>
		/// <returns></returns>
		public virtual PositionCollection GetCellsPositions()
		{
			PositionCollection positions = new PositionCollection();

			//Range
			for (int i = 0; i < m_RangeCollection.Count; i++)
			{
				positions.AddRange( m_RangeCollection[i].GetCellsPositions() );
			}

			return positions;
		}
		#endregion

		#region GetRows/GetColumns
		/// <summary>
		/// Returns all the selected rows index
		/// </summary>
		/// <returns></returns>
		public virtual int[] GetRowsIndex()
		{
			RangeMergerByRows merger = new RangeMergerByRows();
			foreach (Range range in this)
			{
				merger.AddRange(range);
			}
			
			IList<int> indexes = merger.GetRowsIndex();
			int[] ret = new int[indexes.Count];
			indexes.CopyTo(ret, 0);
			
			return ret;
		}
		/// <summary>
		/// Returns all the selected columns index
		/// </summary>
		/// <returns></returns>
		public virtual int[] GetColumnsIndex()
		{
			System.Collections.ArrayList indexList = new System.Collections.ArrayList();

			for (int iRange = 0; iRange < this.Count; iRange++)
			{
				for (int c = this[iRange].Start.Column; c <= this[iRange].End.Column; c++)
				{
					if (indexList.Contains(c) == false)
						indexList.Add(c);
				}
			}
			int[] ret = new int[indexList.Count];
			indexList.CopyTo(ret, 0);

			return ret;
		}
		#endregion

		#region Contains
		/// <summary>
		/// Indicates if the specified cell is selected
		/// </summary>
		/// <param name="p_Cell"></param>
		/// <returns></returns>
		public virtual bool Contains(Position p_Cell)
		{
			if (p_Cell.IsEmpty() || IsEmpty())
				return false;

			//Range
			for (int i = 0; i < m_RangeCollection.Count; i++)
			{
				if (m_RangeCollection[i].Contains(p_Cell))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Indicates if the specified range of cells is selected
		/// </summary>
		/// <param name="p_Range"></param>
		/// <returns></returns>
		public virtual bool Contains(Range p_Range)
		{
			if (p_Range.IsEmpty() || IsEmpty())
				return false;

			if (p_Range.ColumnsCount == 1 && p_Range.RowsCount == 1)
				return Contains(p_Range.Start);

			//Range
			for (int i = 0; i < m_RangeCollection.Count; i++)
			{
				if (m_RangeCollection[i].Contains(p_Range))
					return true;
			}

			PositionCollection positions = p_Range.GetCellsPositions();
			for (int iPos = 0; iPos < positions.Count; iPos++)
			{
				if (Contains(positions[iPos]) == false)
					return false;
			}
			return true;
		}

		/// <summary>
		/// Indicates if the specified range of cells is selected
		/// </summary>
		/// <param name="p_Range"></param>
		/// <returns></returns>
		public virtual bool Contains(RangeRegion p_Range)
		{
			if (p_Range.IsEmpty() || IsEmpty())
				return false;

			PositionCollection positions = p_Range.GetCellsPositions();
			for (int i = 0; i < positions.Count; i++)
			{
				if ( Contains(positions[i]) == false)
					return false;
			}

			return true;
		}

		/// <summary>
		/// Indicates if the specified row is selected
		/// </summary>
		/// <param name="p_Row"></param>
		/// <returns></returns>
		public virtual bool ContainsRow(int p_Row)
		{
			if (IsEmpty())
				return false;

			//Range
			for (int i = 0; i < m_RangeCollection.Count; i++)
			{
				if (m_RangeCollection[i].ContainsRow(p_Row))
					return true;
			}

			return false;
		}
		/// <summary>
		/// Indicates if the specified column is selected
		/// </summary>
		/// <param name="p_Column"></param>
		/// <returns></returns>
		public virtual bool ContainsColumn(int p_Column)
		{
			if (IsEmpty())
				return false;

			//Range
			for (int i = 0; i < m_RangeCollection.Count; i++)
			{
				if (m_RangeCollection[i].ContainsColumn(p_Column))
					return true;
			}

			return false;
		}
		#endregion

		#region Intersect
		/// <summary>
		/// Indicates if the specified range of cells is selected
		/// </summary>
		/// <param name="p_Range"></param>
		/// <returns></returns>
		public virtual bool IntersectsWith(Range p_Range)
		{
			if (p_Range.IsEmpty() || IsEmpty())
				return false;

			RangeRegion range = Intersect(p_Range);
			return !range.IsEmpty();
		}

		/// <summary>
		/// Returns a non contiguous range of cells of the intersection between the current range and the specified range.
		/// </summary>
		/// <param name="p_Range"></param>
		/// <returns></returns>
		public virtual RangeRegion Intersect(Range p_Range)
		{
			RangeRegion range = new RangeRegion();

			if (p_Range.IsEmpty() == false && IsEmpty() == false)
			{
				//Range
				for (int i = 0; i < m_RangeCollection.Count; i++)
				{
					Range intersectRange = p_Range.Intersect(m_RangeCollection[i]);
					if (intersectRange.IsEmpty() == false)
						range.m_RangeCollection.Add(intersectRange);
				}
			}

			return range;
		}
		public RangeRegion Intersect(RangeRegion pRange)
		{
			RangeRegion ret = new RangeRegion();
			for (int rToCheck = 0; rToCheck < pRange.m_RangeCollection.Count; rToCheck++)
			{
				Range rangeToCheck = pRange.m_RangeCollection[rToCheck];
				RangeRegion intersect = Intersect(rangeToCheck);
				ret.m_RangeCollection.AddRange(intersect.m_RangeCollection);
			}
			return ret;
		}

		#endregion

		#region Exclude
		/// <summary>
		/// Returns the cells of the current range that don't intersect with the specified range
		/// </summary>
		/// <param name="pRange"></param>
		/// <returns></returns>
		public RangeRegion Exclude(Range pRange)
		{
			RangeRegion range = new RangeRegion();

			for (int i = 0; i < m_RangeCollection.Count; i++)
			{
				RangeRegion excludedSubRange = m_RangeCollection[i].Exclude(pRange);
				range.m_RangeCollection.AddRange(excludedSubRange.m_RangeCollection);
			}

			return range;
		}

		public RangeRegion Exclude(RangeRegion pRange)
		{
			RangeRegion excludedRange = new RangeRegion(this);
			if (excludedRange.IsEmpty() == false)
			{
				for (int rToCheck = 0; rToCheck < pRange.m_RangeCollection.Count; rToCheck++)
				{
					excludedRange = excludedRange.Exclude( pRange.m_RangeCollection[rToCheck] );
				}
			}

			return excludedRange;
		}
		#endregion

		#region Add/Remove/Clear
		
		/// <summary>
		/// Remove all the cells
		/// </summary>
		public void Clear()
		{
			Remove(new RangeRegion(this));
		}

		/// <summary>
		/// Remove all the cells excluse the specified range
		/// </summary>
		/// <param name="pRangeToLeave"></param>
		public void Clear(Range pRangeToLeave)
		{
			RangeRegion region = new RangeRegion(this);
			region.Remove(pRangeToLeave);
			Remove(region);
		}

		/// <summary>
		/// Reset the object to its original state. It is similar to the Clear method but doesn't call any events when removeing the saved positions, usually used when refreshing the cells with new data.
		/// To simply clear the object use the Clear method, only use this method when you want to force a reset of the object without calling additional methods.
		/// </summary>
		protected virtual void ResetRange()
		{
			m_bValidated = false;
			m_RangeCollection.Clear();
		}

		/// <summary>
		/// Add the specified cell and add the cell to the collection.
		/// </summary>
		/// <param name="pCell"></param>
		/// <returns>Returns true if sucesfully added</returns>
		public bool Add(Position pCell)
		{
			return Add(new Range(pCell));
		}

		/// <summary>
		/// Remove from the collection the specified cell
		/// </summary>
		/// <param name="pCell"></param>
		/// <returns>Returns true if sucesfully removed</returns>
		public bool Remove(Position pCell)
		{
			return Remove(new Range(pCell));
		}

		/// <summary>
		/// Add the specified Range of cells
		/// </summary>
		/// <param name="pRange"></param>
		/// <returns>Returns true if sucesfully added</returns>
		public bool Add(Range pRange)
		{
			return InternalAdd(new RangeRegion(pRange));
		}

		/// <summary>
		/// Remove from the collection the specified range of cells
		/// </summary>
		/// <param name="pRange"></param>
		/// <returns>Returns true if sucesfully removed</returns>
		public bool Remove(Range pRange)
		{
			return InternalRemove(new RangeRegion(pRange));
		}

		/// <summary>
		/// Add the specified ranges of cells
		/// </summary>
		/// <param name="pRange"></param>
		/// <returns></returns>
		public bool Add(RangeRegion pRange)
		{
			return InternalAdd(pRange);
		}

		/// <summary>
		/// Remove the specified ranges of cells
		/// </summary>
		/// <param name="pRange"></param>
		/// <returns></returns>
		public bool Remove(RangeRegion pRange)
		{
			return InternalRemove(pRange);
		}

		/// <summary>
		/// Prende un range che è già stato filtrato con solo le celle non presenti nell'attuale range
		/// </summary>
		/// <param name="pRange"></param>
		/// <returns></returns>
		private bool InternalAdd(RangeRegion pRange)
		{
			if (Contains(pRange))
				return true;

			if (pRange.Contains(this)) //change all the contents with the new range
			{
				RangeRegion existingRange = new RangeRegion(this);

				m_RangeCollection.Clear();
				m_RangeCollection.AddRange(pRange.m_RangeCollection);

				pRange = pRange.Exclude(existingRange);
			}
			else
			{
				pRange = pRange.Exclude(this);
				if (pRange.IsEmpty())
					return true; //il range è vuoto
				pRange.m_bValidated = true;

				RangeRegionCancelEventArgs e = new RangeRegionCancelEventArgs(pRange);
				OnAddingRange(e); //calling this method the range can change
				if (e.Cancel)
					return false;

				if (pRange.m_bValidated == false)
				{
					pRange = pRange.Exclude(this);
					if (pRange.IsEmpty())
						return true; //il range è vuoto
				}

				for (int rToAdd = 0; rToAdd < pRange.m_RangeCollection.Count; rToAdd++)
				{
					Range rangeToAdd = pRange.m_RangeCollection[rToAdd];
					m_RangeCollection.Add(rangeToAdd);
				}
			}

			OnAddedRange(new RangeRegionCancelEventArgs(pRange));

			m_bValidated = false;

			return true;
		}

		/// <summary>
		/// Prende un range che è già stato filtrato con solo le celle presenti nell'attuale range
		/// </summary>
		/// <param name="pRange"></param>
		/// <returns></returns>
		private bool InternalRemove(RangeRegion pRange)
		{
			pRange = Intersect(pRange);
			if (pRange.IsEmpty())
				return true; //il range non è presente
			pRange.m_bValidated = true;

			RangeRegionCancelEventArgs e = new RangeRegionCancelEventArgs(pRange);
			OnRemovingRange(e); //calling this method the range can change
			if (e.Cancel)
				return false;

			if (pRange.m_bValidated == false)
			{
				pRange = Intersect(pRange);
				if (pRange.IsEmpty())
					return true; //il range non è presente
			}

			m_RangeCollection = Exclude(pRange).m_RangeCollection;

			OnRemovedRange(e);

			m_bValidated = false;

			return true;
		}

		#endregion

		/// <summary>
		/// Variabile che indica se lo stato interno della classe è stato modificato, serve per poter ottimizzare alcune verifiche.
		/// </summary>
		private bool m_bValidated = false;

		#region Events
		public virtual void OnAddingRange(RangeRegionCancelEventArgs e)
		{
			if (AddingRange != null)
				AddingRange(this, e);
		}
		public virtual void OnRemovingRange(RangeRegionCancelEventArgs e)
		{
			if (RemovingRange != null)
				RemovingRange(this, e);
		}
		public virtual void OnAddedRange(RangeRegionEventArgs e)
		{
			if (AddedRange != null)
				AddedRange(this, e);

			OnChanged(new RangeRegionChangedEventArgs(e.RangeRegion, null));
		}
		public virtual void OnRemovedRange(RangeRegionEventArgs e)
		{
			if (RemovedRange != null)
				RemovedRange(this, e);

			OnChanged(new RangeRegionChangedEventArgs(null, e.RangeRegion));
		}

		public virtual void OnChanged(RangeRegionChangedEventArgs e)
		{
			if (Changed != null)
				Changed(this, e);
		}
		public event RangeRegionCancelEventHandler AddingRange;
		public event RangeRegionCancelEventHandler RemovingRange;
		public event RangeRegionEventHandler AddedRange;
		public event RangeRegionEventHandler RemovedRange;
		public event RangeRegionChangedEventHandler Changed;
		#endregion

		public override string ToString()
		{
			String buffer = "RangeRegion";
			for (int i = 0; i < m_RangeCollection.Count; i++)
				buffer += " | " + m_RangeCollection[i].ToString();
			return buffer;
		}


		#region ICollection<Range> Members
		void ICollection<Range>.Add(Range item)
		{
			this.Add(item);
		}

		public void CopyTo(Range[] array, int arrayIndex)
		{
			m_RangeCollection.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return m_RangeCollection.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		#endregion

		#region IEnumerable<Range> Members

		public IEnumerator<Range> GetEnumerator()
		{
			return m_RangeCollection.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return m_RangeCollection.GetEnumerator();
		}

		#endregion
	}
}
