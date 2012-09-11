using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGrid
{
	public class GridRow : RowInfo
	{
		public GridRow(Grid grid)
			: base(grid)
		{
		}

		private Dictionary<GridColumn, Cells.ICell> mCells = new Dictionary<GridColumn, Cells.ICell>();

		public Cells.ICell this[GridColumn column]
		{
			get
			{
				Cells.ICell cell;
				if (mCells.TryGetValue(column, out cell))
					return cell;
				else
					return null;
			}
			set
			{
				mCells[column] = value;
			}
		}
	}

	public class GridRows : RowInfoCollection
	{
		public GridRows(Grid grid)
			: base(grid)
		{
		}
		
		public override void Swap(int p_RowIndex1, int p_RowIndex2)
		{
			base.Swap(p_RowIndex1, p_RowIndex2);
			this.Grid.SpannedCellReferences.Swap(p_RowIndex1, p_RowIndex2);
		}
		

		/// <summary>
		/// Insert a row at the specified position
		/// </summary>
		/// <param name="p_Index"></param>
		public void Insert(int p_Index)
		{
			InsertRange(p_Index, 1);
		}

		/// <summary>
		/// Insert the specified number of rows at the specified position
		/// </summary>
		public void InsertRange(int startIndex, int count)
		{
			RowInfo[] rows = new RowInfo[count];
			for (int i = 0; i < rows.Length; i++)
				rows[i] = CreateRow();

			base.InsertRange(startIndex, rows);
			// Ensure that grid grows when rows are inserted
			this.Grid.GrowGrid();
			
			this.Grid.SpannedCellReferences.MoveDownSpannedRanges(startIndex, count);
			this.Grid.SpannedCellReferences.ExpandSpannedRows(startIndex, count);
			
			
		}
		
		public new Grid Grid
		{
			get { return base.Grid as Grid;}
		}
		
		public override void RemoveRange(int startIndex, int count)
		{
			this.Grid.SpannedCellReferences.RemoveSpannedCellReferencesInRows(startIndex, count);
			base.RemoveRange(startIndex, count);
			this.Grid.SpannedCellReferences.ShrinkOrRemoveSpannedRows(startIndex, count);
			this.Grid.SpannedCellReferences.MoveUpSpannedRanges(startIndex, count);
		}

		protected GridRow CreateRow()
		{
			return new GridRow((Grid)Grid);
		}

		public new GridRow this[int index]
		{
			get { return (GridRow)base[index]; }
		}

		public void SetCount(int value)
		{
			this.Grid.GrowGrid();
			if (Count < value)
				InsertRange(Count, value - Count);
			else if (Count > value)
				RemoveRange(value, Count - value);
		}
	}
}
