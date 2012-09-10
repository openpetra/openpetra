using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGrid
{
	public partial class Grid
	{
		public override bool EnableSort {get;set;}
		
	}
	
	public class GridColumn : ColumnInfo
	{
		public GridColumn(Grid grid)
			: base(grid)
		{
		}

		private Dictionary<GridRow, Cells.ICell> mCells = new Dictionary<GridRow, Cells.ICell>();

		public Cells.ICell this[GridRow row]
		{
			get
			{
				Cells.ICell cell;
				if (mCells.TryGetValue(row, out cell))
					return cell;
				else
					return null;
			}
			set
			{
				mCells[row] = value;
			}
		}
	}

	public class GridColumns : ColumnInfoCollection
	{
		public new Grid Grid
		{
			get { return base.Grid as Grid;}
		}
		
		public GridColumns(Grid grid)
			: base(grid)
		{
		}

		/// <summary>
		/// Insert a column at the specified position
		/// </summary>
		/// <param name="p_Index"></param>
		public void Insert(int p_Index)
		{
			InsertRange(p_Index, 1);
		}

		/// <summary>
		/// Insert the specified number of Columns at the specified position
		/// </summary>
		public void InsertRange(int startIndex, int count)
		{
			GridColumn[] columns = new GridColumn[count];
			for (int i = 0; i < columns.Length; i++)
				columns[i] = CreateColumn();

			InsertRange(startIndex, columns);
			this.Grid.SpannedCellReferences.MoveRightSpannedRanges(startIndex, count);
			this.Grid.SpannedCellReferences.ExpandSpannedColumns(startIndex, count);
		}
		
		public override void RemoveRange(int startIndex, int count)
		{
			this.Grid.SpannedCellReferences.RemoveSpannedCellReferencesInColumns(startIndex, count);
			base.RemoveRange(startIndex, count);
			this.Grid.SpannedCellReferences.ShrinkOrRemoveSpannedColumns(startIndex, count);
			this.Grid.SpannedCellReferences.MoveLeftSpannedRanges(startIndex, count);
		}
		
		protected GridColumn CreateColumn()
		{
			return new GridColumn((Grid)Grid);
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
