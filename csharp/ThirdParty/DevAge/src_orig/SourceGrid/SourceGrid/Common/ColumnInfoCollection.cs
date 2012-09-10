using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceGrid
{
	/// <summary>
	/// Collection of ColumnInfo
	/// </summary>
	public class ColumnInfoCollection : ColumnsBase, IEnumerable
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="grid"></param>
		public ColumnInfoCollection(GridVirtual grid):base(grid)
		{
		}

		private List<ColumnInfo> m_List = new List<ColumnInfo>();

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

		public void Add(ColumnInfo column)
		{
			Insert(Count, column);
		}
		
		public void Insert(int index, ColumnInfo dataGridColumn)
		{
			InsertRange(index, dataGridColumn);
		}
		
		/// <summary>
		/// Insert the specified number of Columns at the specified position
		/// </summary>
		/// <param name="p_StartIndex"></param>
		/// <param name="columns"></param>
		public void InsertRange(int p_StartIndex, params ColumnInfo[] columns)
		{
			if (IsValidRangeForInsert(p_StartIndex, columns.Length)==false)
				throw new SourceGridException("Invalid index");

			for (int c = 0; c < columns.Length; c++)
			{
				m_List.Insert(p_StartIndex + c, columns[c]);
			}

			PerformLayout();

			OnColumnsAdded(new IndexRangeEventArgs(p_StartIndex, columns.Length));
		}

		/// <summary>
		/// Remove a column at the speicifed position
		/// </summary>
		/// <param name="p_Index"></param>
		public void Remove(int p_Index)
		{
			RemoveRange(p_Index, 1);
		}

		/// <summary>
		/// Remove the ColumnInfo at the specified positions
		/// </summary>
		/// <param name="p_StartIndex"></param>
		/// <param name="p_Count"></param>
		public virtual void RemoveRange(int p_StartIndex, int p_Count)
		{
			if (IsValidRange(p_StartIndex, p_Count)==false)
				throw new SourceGridException("Invalid index");

			IndexRangeEventArgs eventArgs = new IndexRangeEventArgs(p_StartIndex, p_Count);
			OnColumnsRemoving(eventArgs);

			m_List.RemoveRange(p_StartIndex, p_Count);

			OnColumnsRemoved(eventArgs);

			PerformLayout();
		}


		#endregion

		/// <summary>
		/// Move a column from one position to another position
		/// </summary>
		/// <param name="p_CurrentColumnPosition"></param>
		/// <param name="p_NewColumnPosition"></param>
		public void Move(int p_CurrentColumnPosition, int p_NewColumnPosition)
		{
			if (p_CurrentColumnPosition == p_NewColumnPosition)
				return;

			if (p_CurrentColumnPosition < p_NewColumnPosition)
			{
				for (int r = p_CurrentColumnPosition; r < p_NewColumnPosition; r++)
				{
					Swap(r, r + 1);
				}
			}
			else
			{
				for (int r = p_CurrentColumnPosition; r > p_NewColumnPosition; r--)
				{
					Swap(r, r - 1);
				}
			}
		}

		/// <summary>
		/// Change the position of column 1 with column 2.
		/// </summary>
		/// <param name="p_ColumnIndex1"></param>
		/// <param name="p_ColumnIndex2"></param>
		public void Swap(int p_ColumnIndex1, int p_ColumnIndex2)
		{
			if (p_ColumnIndex1 == p_ColumnIndex2)
				return;

			ColumnInfo column1 = this[p_ColumnIndex1];
			ColumnInfo column2 = this[p_ColumnIndex2];

			m_List[p_ColumnIndex1] = column2;
			m_List[p_ColumnIndex2] = column1;

			PerformLayout();
		}

		/// <summary>
		/// Fired when the number of columns change
		/// </summary>
		public event IndexRangeEventHandler ColumnsAdded;

		/// <summary>
		/// Fired when the number of columns change
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnColumnsAdded(IndexRangeEventArgs e)
		{
			if (ColumnsAdded!=null)
				ColumnsAdded(this, e);

			ColumnsChanged();
		}

		/// <summary>
		/// Fired when some columns are removed
		/// </summary>
		public event IndexRangeEventHandler ColumnsRemoved;

		/// <summary>
		/// Fired when some columns are removed
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnColumnsRemoved(IndexRangeEventArgs e)
		{
			if (ColumnsRemoved!=null)
				ColumnsRemoved(this, e);

			ColumnsChanged();
		}

		/// <summary>
		/// Fired before some columns are removed
		/// </summary>
		public event IndexRangeEventHandler ColumnsRemoving;

		/// <summary>
		/// Fired before some columns are removed
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnColumnsRemoving(IndexRangeEventArgs e)
		{
			if (ColumnsRemoving!=null)
				ColumnsRemoving(this, e);

			//Grid.OnColumnsRemoving(e);
		}

		/// <summary>
		/// Indexer. Returns a ColumnInfo at the specified position
		/// </summary>
		public ColumnInfo this[int p]
		{
			get
			{
				if (p < 0 )
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
		/// Fired when the user change the Width property of one of the Column
		/// </summary>
		public event ColumnInfoEventHandler ColumnWidthChanged;

		/// <summary>
		/// Execute the RowHeightChanged event
		/// </summary>
		/// <param name="e"></param>
		public void OnColumnWidthChanged(ColumnInfoEventArgs e)
		{
			PerformLayout();

			if (ColumnWidthChanged!=null)
				ColumnWidthChanged(this, e);
		}


		public int IndexOf(ColumnInfo p_Info)
		{
			return m_List.IndexOf(p_Info);
		}

		/// <summary>
		/// Auto size the columns calculating the required size only on the rows currently visible
		/// </summary>
		public void AutoSizeView()
		{
			List<int> list = Grid.Rows.RowsInsideRegion(Grid.DisplayRectangle.Y, Grid.DisplayRectangle.Height, true, false);
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
			if (Count > 0)
				RemoveRange(0, Count);
		}

		#region ColumnsBase
		public override int GetWidth(int column)
		{
			return (IsColumnVisible(column) ? this[column].Width : 0);
		}
		public override void SetWidth(int column, int width)
		{
			this[column].Width = width;
		}
		public override AutoSizeMode GetAutoSizeMode(int column)
		{
			return this[column].AutoSizeMode;
		}

		public override bool IsColumnVisible(int column)
		{
			return this[column].Visible;
		}
		public override void HideColumn(int column)
		{
			this[column].Visible = false;
		}
		public override void ShowColumn(int column)
		{
			this[column].Visible = true;
		}
		#endregion


		public override int Count
		{
			get{return m_List.Count;}
		}


		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_List.GetEnumerator();
		}

		#endregion
	}
}
