using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceGrid
{
	/// <summary>
	/// Collection of RowInfo
	/// </summary>
	public abstract class RowInfoCollection : RowsBase, IEnumerable<RowInfo>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="grid"></param>
		public RowInfoCollection(GridVirtual grid):base(grid)
		{
			m_HiddenRowsCoordinator = new RowInfoCollectoinHiddenRowCoordinator(this);
		}

		private List<RowInfo> m_List = new List<RowInfo>();

		/// <summary>
		/// Returns true if the range passed is valid
		/// </summary>
		/// <param name="p_StartIndex"></param>
		/// <param name="p_Count"></param>
		/// <returns></returns>
		public bool IsValidRange(int p_StartIndex, int p_Count)
		{
			if (p_StartIndex < Count && p_StartIndex >= 0 &&
			    p_Count > 0 && (p_StartIndex+p_Count) <= Count)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Returns true if the range passed is valid for insert method
		/// </summary>
		/// <param name="p_StartIndex"></param>
		/// <param name="p_Count"></param>
		/// <returns></returns>
		public bool IsValidRangeForInsert(int p_StartIndex, int p_Count)
		{
			if (p_StartIndex <= Count && p_StartIndex >= 0 &&
			    p_Count > 0)
				return true;
			else
				return false;
		}


		#region Insert/Remove Methods

		/// <summary>
		/// Insert the specified number of rows at the specified position
		/// </summary>
		/// <param name="p_StartIndex"></param>
		/// <param name="rows"></param>
		protected void InsertRange(int p_StartIndex, RowInfo[] rows)
		{
			if (IsValidRangeForInsert(p_StartIndex, rows.Length) == false)
				throw new SourceGridException("Invalid index");

			for (int r = 0; r < rows.Length; r++)
			{
				m_List.Insert(p_StartIndex+r, rows[r]);
			}

			PerformLayout();

			OnRowsAdded(new IndexRangeEventArgs(p_StartIndex, rows.Length));
		}

		/// <summary>
		/// Remove a row at the speicifed position
		/// </summary>
		/// <param name="p_Index"></param>
		public void Remove(int p_Index)
		{
			RemoveRange(p_Index, 1);
		}
		/// <summary>
		/// Remove the RowInfo at the specified positions
		/// </summary>
		/// <param name="p_StartIndex"></param>
		/// <param name="p_Count"></param>
		public virtual void RemoveRange(int p_StartIndex, int p_Count)
		{
			if (IsValidRange(p_StartIndex, p_Count)==false)
				throw new SourceGridException("Invalid index");

			IndexRangeEventArgs eventArgs = new IndexRangeEventArgs(p_StartIndex, p_Count);
			OnRowsRemoving(eventArgs);

			m_List.RemoveRange(p_StartIndex, p_Count);

			OnRowsRemoved(eventArgs);

			PerformLayout();
		}

		#endregion

		/// <summary>
		/// Move a row from one position to another position
		/// </summary>
		/// <param name="p_CurrentRowPosition"></param>
		/// <param name="p_NewRowPosition"></param>
		public void Move(int p_CurrentRowPosition, int p_NewRowPosition)
		{
			if (p_CurrentRowPosition == p_NewRowPosition)
				return;

			if (p_CurrentRowPosition < p_NewRowPosition)
			{
				for (int r = p_CurrentRowPosition; r < p_NewRowPosition; r++)
				{
					Swap(r, r + 1);
				}
			}
			else
			{
				for (int r = p_CurrentRowPosition; r > p_NewRowPosition; r--)
				{
					Swap(r, r - 1);
				}
			}
		}

		/// <summary>
		/// Change the position of row 1 with row 2.
		/// </summary>
		/// <param name="p_RowIndex1"></param>
		/// <param name="p_RowIndex2"></param>
		public virtual void Swap(int p_RowIndex1, int p_RowIndex2)
		{
			if (p_RowIndex1 == p_RowIndex2)
				return;

			RowInfo row1 = this[p_RowIndex1];
			RowInfo row2 = this[p_RowIndex2];

			m_List[p_RowIndex1] = row2;
			m_List[p_RowIndex2] = row1;

			PerformLayout();
		}

		/// <summary>
		/// Fired when the number of rows change
		/// </summary>
		public event IndexRangeEventHandler RowsAdded;

		/// <summary>
		/// Fired when the number of rows change
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnRowsAdded(IndexRangeEventArgs e)
		{
			if (RowsAdded!=null)
				RowsAdded(this, e);

			RowsChanged();
		}

		/// <summary>
		/// Fired when some rows are removed
		/// </summary>
		public event IndexRangeEventHandler RowsRemoved;

		/// <summary>
		/// Fired when some rows are removed
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnRowsRemoved(IndexRangeEventArgs e)
		{
			if (RowsRemoved!=null)
				RowsRemoved(this, e);

			RowsChanged();
		}

		/// <summary>
		/// Fired before some rows are removed
		/// </summary>
		public event IndexRangeEventHandler RowsRemoving;

		/// <summary>
		/// Fired before some rows are removed
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnRowsRemoving(IndexRangeEventArgs e)
		{
			if (RowsRemoving!=null)
				RowsRemoving(this, e);

			//Grid.OnRowsRemoving(e);
		}

		/// <summary>
		/// Indexer. Returns a RowInfo at the specified position
		/// </summary>
		public RowInfo this[int p]
		{
			get
			{
				if (p < 0)
					return null;
				if (p >= m_List.Count)
					return null;
				return m_List[p];
			}
		}

		protected override void OnLayout()
		{
			base.OnLayout ();
		}

		/// <summary>
		/// Fired when the user change the Height property of one of the Row
		/// </summary>
		public event RowInfoEventHandler RowHeightChanged;

		/// <summary>
		/// Execute the RowHeightChanged event
		/// </summary>
		/// <param name="e"></param>
		public void OnRowHeightChanged(RowInfoEventArgs e)
		{
			PerformLayout();

			if (RowHeightChanged!=null)
				RowHeightChanged(this, e);
		}


		public int IndexOf(RowInfo p_Info)
		{
			return m_List.IndexOf(p_Info);
		}

		/// <summary>
		/// Auto size the rows calculating the required size only on the columns currently visible
		/// </summary>
		public void AutoSizeView()
		{
			List<int> list = Grid.Columns.ColumnsInsideRegion(Grid.DisplayRectangle.X, Grid.DisplayRectangle.Width, true, false);
			if (list.Count > 0)
			{
				AutoSize(false, list[0], list[list.Count - 1]);
			}
		}

		/// <summary>
		/// Remove all the columns
		/// </summary>
		public void Clear()
		{
			if (Count == 0)
				return;
			this.Grid.LinkedControls.Clear();
			RemoveRange(0, Count);
		}
		
		

		#region RowsBase
		public override int GetHeight(int row)
		{
			return this[row].Height;
		}
		public override void SetHeight(int row, int height)
		{
			this[row].Height = height;
		}
		public override AutoSizeMode GetAutoSizeMode(int row)
		{
			return this[row].AutoSizeMode;
		}
		#endregion

		public override int Count
		{
			get{return m_List.Count;}
		}

		#region IEnumerable<RowInfo> Members

		public IEnumerator<RowInfo> GetEnumerator()
		{
			return m_List.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_List.GetEnumerator();
		}

		#endregion
	}
}
