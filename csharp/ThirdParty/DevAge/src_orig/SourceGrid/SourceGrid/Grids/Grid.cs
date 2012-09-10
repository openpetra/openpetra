using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using SourceGrid.Cells;

namespace SourceGrid
{
	/// <summary>
	/// The main grid control with static data.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Grid : GridVirtual
	{
		private ISpannedCellRangesController spannedCellReferences = null;
		
		#region properties
		/// <summary>
		/// Allows to check what spanned cells we have.
		/// Do not use this property directly from client code
		/// </summary>
		public ISpannedCellRangesController SpannedCellReferences {
			get { return spannedCellReferences; }
		}
		#endregion
		
		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public Grid()
		{
			this.SuspendLayout();
			this.Name = "Grid";
			spannedCellReferences = new SpannedCellRangesController(
				this,
				new QuadTreeRangesList(Range.From(new Position(0, 0), 4, 4)));

			this.ResumeLayout(false);
		}

		/// <summary>
		/// Method used to create the rows object, in this class of type RowInfoCollection.
		/// </summary>
		protected override RowsBase CreateRowsObject()
		{
			return new GridRows(this);
		}

		/// <summary>
		/// Method used to create the columns object, in this class of type ColumnInfoCollection.
		/// </summary>
		protected override ColumnsBase CreateColumnsObject()
		{
			return new GridColumns(this);
		}

		#endregion

		#region Rows/Columns
		/// <summary>
		/// Gets or Sets the number of columns
		/// </summary>
		[DefaultValue(0)]
		public int ColumnsCount
		{
			get{return Columns.Count;}
			set
			{
				Columns.SetCount(value);
			}
		}

		/// <summary>
		/// Gets or Sets the number of rows
		/// </summary>
		[DefaultValue(0)]
		public int RowsCount
		{
			get{return Rows.Count;}
			set
			{
				Rows.SetCount(value);
			}
		}

		/// <summary>
		/// RowsCount informations
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new GridRows Rows
		{
			get { return (GridRows)base.Rows; }
		}

		/// <summary>
		/// Columns informations
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new GridColumns Columns
		{
			get { return (GridColumns)base.Columns; }
		}

		private CellOptimizeMode mOptimizeMode = CellOptimizeMode.ForRows;
		/// <summary>
		/// Gets or sets the optimize mode. Default is ForRows
		/// </summary>
		public CellOptimizeMode OptimizeMode
		{
			get { return mOptimizeMode; }
			set { mOptimizeMode = value; }
		}
		#endregion

		#region AllowOverlappingCells
		private bool mAllowOverlappingCells = false;
		/// <summary>
		/// Indicates that overlapping cells are not allowed and will be thrown an exception.
		/// This is needed due to the fact that when there are two overlapping spanned cells,
		/// SourceGrid can not reliably draw them, since the call to function GetCell(x,y)
		/// returns random results. However, some users epxressed the need to allow such
		/// behaviour. So this flag forces Grid to not check for overlapping cells
		/// </summary>
		[DefaultValue(false)]
		public bool AllowOverlappingCells
		{
			get { return mAllowOverlappingCells; }
			set { mAllowOverlappingCells = value; }
		}
		#endregion
		
		#region GetCell methods
		/// <summary>
		/// Set the specified cell int he specified position. Abstract method of the GridVirtual control
		/// </summary>
		/// <param name="p_iRow"></param>
		/// <param name="p_iCol"></param>
		/// <param name="p_Cell"></param>
		public virtual void SetCell(int p_iRow, int p_iCol, Cells.ICellVirtual p_Cell)
		{
			if (p_Cell is Cells.ICell)
				InsertCell(p_iRow, p_iCol, (Cells.ICell)p_Cell);
			else if (p_Cell == null)
				InsertCell(p_iRow, p_iCol, null);
			else
				throw new SourceGridException("Expected ICell class");
		}
		
		/// <summary>
		/// Set the specified cell int he specified position. This method calls SetCell(int p_iRow, int p_iCol, Cells.ICellVirtual p_Cell)
		/// </summary>
		/// <param name="p_Position"></param>
		/// <param name="p_Cell"></param>
		public void SetCell(Position p_Position, Cells.ICellVirtual p_Cell)
		{
			SetCell(p_Position.Row, p_Position.Column, p_Cell);
		}

		/// <summary>
		/// Return the Cell at the specified Row and Col position.
		/// </summary>
		/// <param name="p_iRow"></param>
		/// <param name="p_iCol"></param>
		/// <returns></returns>
		public override Cells.ICellVirtual GetCell(int p_iRow, int p_iCol)
		{
			return this[p_iRow, p_iCol];
		}


		private void DirectSetCell(Position position, Cells.ICell cell)
		{
			if (OptimizeMode == CellOptimizeMode.ForRows)
			{
				GridRow row = Rows[position.Row];
				if (position.Column >= Columns.Count)
					throw new ArgumentException(string.Format(
						"Grid has only {0} columns, you tried to insert cell into position {1}." +
						"You should probably call Redim on grid to increase it's column size",
						Columns.Count, position.ToString()));

				row[Columns[position.Column] as GridColumn] = cell;
			}
			else if (OptimizeMode == CellOptimizeMode.ForColumns)
			{
				GridColumn col = Columns[position.Column] as GridColumn;

				col[Rows[position.Row]] = cell;
			}
			else
				throw new SourceGridException("Invalid OptimizeMode");
		}

		private bool IsInGrid(Position position)
		{
			if (position.Column < 0)
				return false;
			if (position.Row < 0 )
				return false;
			if (position.Column >= this.Columns.Count)
				return false;
			if (position.Row >= this.Rows.Count)
				return false;
			return true;
		}
		
		private Cells.ICell DirectGetCell(int row, int col)
		{
			return DirectGetCell(new Position(row, col));
		}
		
		private Cells.ICell DirectGetCell(Position position)
		{
			if (IsInGrid(position) == false)
				return null;
			if (OptimizeMode == CellOptimizeMode.ForRows)
			{
				GridRow row = Rows[position.Row];
				if (row == null)
					return null;

				return row[Columns[position.Column] as GridColumn];
			}
			else if (OptimizeMode == CellOptimizeMode.ForColumns)
			{
				GridColumn col = Columns[position.Column] as GridColumn;
				if (col == null)
					return null;
				
				return col[Rows[position.Row]];
			}
			else
				throw new SourceGridException("Invalid OptimizeMode");
		}

		public Cells.ICell this[Position position]
		{
			get
			{
				Cells.ICell cell = DirectGetCell(position);
				if ( cell != null )
					return cell;
				return GetSpannedCell(position);
			}
			set
			{
				InsertCell(position.Row, position.Column, value);
			}
		}
		
		/// <summary>
		/// Returns or set a cell at the specified row and col.
		/// If you get a ICell position occupied by a row/col span cell,
		/// and EnableRowColSpan is true, this method returns the cell with Row/Col span.
		/// </summary>
		public Cells.ICell this[int row, int col]
		{
			get
			{
				return this[new Position(row, col)];
			}
			set
			{
				this[new Position(row, col)] = value;
			}
		}

		/// <summary>
		/// Check if a cell exists in spanned cells.
		/// If yes, returns. If no, returns null
		/// </summary>
		private Cells.ICell GetSpannedCell(Position pos)
		{
			var range = spannedCellReferences.SpannedRangesCollection.GetFirstIntersectedRange(pos);
			if (range == null)
				return null;
			Position cellPos = range.Value.Start;
			Cells.ICell cell = DirectGetCell(cellPos);
			if (cell == null)
				throw new ArgumentException(string.Format(
					"Invalid grid state. Grid should contain a spanned cell at position {0}, " +
					" but apparently it does not!", cellPos));
			return cell;
		}
		
		/// <summary>
		/// Remove the specified cell
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		private void RemoveCell(int row, int col)
		{
			Cells.ICell cell = DirectGetCell(new Position(row, col));

			if (cell == null)
				return;
			cell.UnBindToGrid();
			
			RemoveSpannedCell(new Position(row, col));
			DirectSetCell(new Position(row, col), null);
		}
		
		/// <summary>
		/// Removes a spanned cell internal reference.
		/// This effectively removes any data in spannedCellReferences collection
		/// </summary>
		/// <param name="pos"></param>
		private void RemoveSpannedCell(Position pos)
		{
			var range = spannedCellReferences.SpannedRangesCollection.GetFirstIntersectedRange(pos);
			if (range == null )
				return;
			spannedCellReferences.SpannedRangesCollection.Remove(range.Value);
		}
		
		#region Spanned Cells support
		
		
		private void EnsureNoSpannedCellsExist(int row, int col, Cells.ICell p_cell)
		{
			var spanRange = new Range(row, col, row + p_cell.RowSpan - 1, col + p_cell.ColumnSpan - 1);
			var ranges = spannedCellReferences.SpannedRangesCollection.GetRanges(
				spanRange);
			if (ranges.Count == 0)
				return;
			
			var start = new Position(row, col);
			foreach (var range in ranges)
			{
				if (start.Equals(range.Start) == true)
					continue;
				Cells.ICell existingSpannedCell = this[range.Start];
				if (existingSpannedCell == null)
					throw new ArgumentException("internal error. please report this bug to developers");
				throw new OverlappingCellException(string.Format(
					"Given cell at position ({0}, {1}), " +
					"intersects with another cell " +
					"at position ({2}, {3}) '{4}'",
					row, col,
					existingSpannedCell.Row.Index,
					existingSpannedCell.Column.Index,
					existingSpannedCell.DisplayText));
			}
			
		}
		
		/// <summary>
		/// Check
		/// </summary>
		private void EnsureNoOtherCellsExist(int row, int col, Cells.ICell p_cell)
		{
			for (int y = row; y < row + p_cell.RowSpan; y++)
			{
				for (int x = col; x < col + p_cell.ColumnSpan; x++)
				{
					if (x == col && y == row)
						continue;
					Cells.ICell existingSpannedCell = this.DirectGetCell(y, x);
					if (existingSpannedCell != null)
						if (existingSpannedCell != p_cell)
							throw new OverlappingCellException(string.Format(
								"Given cell at position ({0}, {1}), " +
								"intersects with another cell " +
								"at position ({2}, {3}) '{4}'",
								row, col,
								existingSpannedCell.Row.Index,
								existingSpannedCell.Column.Index,
								existingSpannedCell.DisplayText));
				}
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="p_cell">except this cell</param>
		private void EnsureDestinationSpannedAreaIsEmptyExceptOriginalCell(int row, int col, Cells.ICell p_cell)
		{
			if (AllowOverlappingCells)
				return;
			
			EnsureNoSpannedCellsExist(row, col, p_cell);
			EnsureNoOtherCellsExist(row, col, p_cell);
			
			/*
			for (int y = row; y < row + p_cell.RowSpan; y++)
			{
				for (int x = col; x < col + p_cell.ColumnSpan; x++)
				{
					if (x == col && y == row)
						continue;
					Cells.ICell existingSpannedCell = this[y, x];
					if (existingSpannedCell != null)
						if (existingSpannedCell != p_cell)
							throw new OverlappingCellException(string.Format(
								"Given cell at position ({0}, {1}), " +
								"intersects with another cell " +
								"at position ({2}, {3}) '{4}'",
								row, col,
								existingSpannedCell.Row.Index,
								existingSpannedCell.Column.Index,
								existingSpannedCell.DisplayText));
				}
			}*/
		}
		
		private void EnsureDestinationSpannedAreaisCompletelyEmpty(int row, int col, int rowSpan, int colSpan)
		{
			if (AllowOverlappingCells)
				return;

			for (int y = row; y < row + rowSpan; y++)
			{
				for (int x = col; x < col + colSpan; x++)
				{
					Cells.ICell existingSpannedCell = this[y, x];
					if (existingSpannedCell != null)
						throw new OverlappingCellException(string.Format(
							"Given cell at position ({0}, {1}), " +
							"intersects with another cell " +
							"at position ({2}, {3}) '{4}'",
							row, col,
							(existingSpannedCell.Row!=null)?existingSpannedCell.Row.Index : -1,
							(existingSpannedCell.Column!=null)?existingSpannedCell.Column.Index : -1,
							existingSpannedCell.DisplayText));
				}
			}
		}

		public void OccupySpannedArea(int row, int col, ICell p_cell)
		{
			if (p_cell == null)
				throw new ArgumentNullException();
			EnsureDestinationSpannedAreaIsEmptyExceptOriginalCell(row, col, p_cell);
			spannedCellReferences.UpdateOrAdd(p_cell.Range);
		}
		
		public void UpdateSpannedArea(int row, int col, ICell p_cell)
		{
			if (p_cell == null)
				throw new ArgumentNullException();
			if (p_cell.RowSpan == 1 && p_cell.ColumnSpan == 1)
				throw new ArgumentException("Cell is not spanned! Can not update it!. " +
				                            "You should delete it manually, and ensure that foreach statement" +
				                            " will not be broken");
			EnsureDestinationSpannedAreaIsEmptyExceptOriginalCell(row, col, p_cell);
			spannedCellReferences.Update(p_cell.Range);
		}
		
		#endregion
		
		/// <summary>
		/// Insert the specified cell
		/// </summary>
		private void InsertCell(int row, int col, ICell p_cell)
		{
			RemoveCell(row, col);

			if (p_cell != null && p_cell.Grid != null)
				throw new ArgumentException("This cell already have a linked grid", "p_cell");

			if (p_cell != null)
				EnsureDestinationSpannedAreaisCompletelyEmpty(row, col, p_cell.RowSpan, p_cell.ColumnSpan);
			
			if (p_cell != null)
			{
				p_cell.BindToGrid(this,new Position(row, col));
			}
			
			if ((p_cell != null) && ((p_cell.RowSpan > 1) || (p_cell.ColumnSpan > 1)))
			{
				// occupy space if it is spanned cell
				OccupySpannedArea(row, col, p_cell);
			}
			
			DirectSetCell(new Position(row, col), p_cell);
		}
		#endregion

		#region AddRow/Col, RemoveRow/Col
		
		/// <summary>
		/// Call this method to update internal grid structures.
		/// This is used to update QuadTree space at the moment.
		/// </summary>
		internal void GrowGrid()
		{
			this.spannedCellReferences.SpannedRangesCollection.Redim(RowsCount, ColumnsCount);
		}
		
		/// <summary>
		/// Set the number of columns and rows
		/// </summary>
		public void Redim(int p_Rows, int p_Cols)
		{
			base.SuspendLayout();
			RowsCount = p_Rows;
			ColumnsCount = p_Cols;
			base.ResumeLayout();
			GrowGrid();
		}

		#endregion

		#region Row/Col Span
		[Obsolete]
		private static int m_MaxSpan = 100;
		/// <summary>
		/// Gets the maximum rows or columns number to search when using Row/Col Span.
		/// This is a static property.
		/// This value is automatically calculated based on the current cells. Do not change this value manually.
		/// Default is 100.
		/// </summary>
		[Obsolete("This property is not needed anymore. It has completely no effect")]
		public static int MaxSpan
		{
			get { return m_MaxSpan; }
			set { m_MaxSpan = value; }
		}

		/// <summary>
		/// This method converts a Position to the real range of the cell. This is usefull when RowSpan or ColumnSpan is greater than 1.
		/// </summary>
		/// <returns></returns>
		public override Range RangeToCellRange(Range range)
		{
			int x = range.Start.Column;
			int x1 = range.End.Column;
			int y = range.Start.Row;
			int y1 = range.End.Row;
			for (int x2 = range.Start.Column; x2 <= range.End.Column; x2++)
			{
				for (int y2 = range.Start.Row; y2 <= range.End.Row; y2++)
				{
					var p = new Position(y2, x2);
					Range range2 = PositionToCellRange(p);
					if (range2.IsEmpty())
						range2 = new Range(p, p);
					if (range2.Start.Column < x)
						x = range2.Start.Column;
					if (range2.End.Column > x1)
						x1 = range2.End.Column;
					
					if (range2.Start.Row < y)
						y = range2.Start.Row;
					if (range2.End.Row > y1)
						y1 = range2.End.Row;
				}
			}
			return new Range(y, x, y1, x1);
		}
		
		/// <summary>
		/// This method converts a Position to the real range of the cell. This is usefull when RowSpan or ColumnSpan is greater than 1.
		/// For example suppose to have at grid[0,0] a cell with ColumnSpan equal to 2. If you call this method with the position 0,0 returns 0,0-0,1 and if you call this method with 0,1 return again 0,0-0,1.
		/// </summary>
		/// <param name="pPosition"></param>
		/// <returns></returns>
		public override Range PositionToCellRange(Position pPosition)
		{
			if (pPosition.IsEmpty())
				return Range.Empty;

			Cells.ICell l_Cell = this[pPosition.Row, pPosition.Column];
			if (l_Cell == null)
				return Range.Empty;
			else
				return l_Cell.Range;
		}
		#endregion

		#region InvalidateCell
		/// <summary>
		/// Force a redraw of the specified cell
		/// </summary>
		/// <param name="p_Cell"></param>
		public virtual void InvalidateCell(Cells.ICell p_Cell)
		{
			if (p_Cell!=null)
				base.InvalidateRange(p_Cell.Range);
		}
		
		/// <summary>
		/// Force a cell to redraw. If ColSpan or RowSpan is greater than 0 this function invalidate the complete range with InvalidateRange
		/// </summary>
		/// <param name="p_Position"></param>
		public override void InvalidateCell(Position p_Position)
		{
			Cells.ICell cell = this[p_Position.Row, p_Position.Column];
			if (cell == null || (cell.Range.ColumnsCount == 1 && cell.Range.RowsCount == 1))
				base.InvalidateCell(p_Position);
			else
				InvalidateRange(cell.Range);
		}

		#endregion

		#region PaintCell
		/// <summary>
		/// List of the ranges already drawn
		/// </summary>
		private RangeCollection mDrawnRange = new RangeCollection();
		protected override void OnRangePaint(RangePaintEventArgs e)
		{
			mDrawnRange.Clear();

			base.OnRangePaint(e);
		}

		protected override void PaintCell(DevAge.Drawing.GraphicsCache graphics, CellContext cellContext, RectangleF drawRectangle)
		{
			Range cellRange = PositionToCellRange(cellContext.Position);
			if (cellRange.ColumnsCount == 1 && cellRange.RowsCount == 1)
			{
				base.PaintCell(graphics, cellContext, drawRectangle);
			}
			else //Row/Col Span > 1
			{
				// I draw the merged cell only if not already drawn otherwise
				// drawing the same cell can cause some problem when using
				// special drawing code (for example semi transparent background)
				if (mDrawnRange.Contains(cellRange) == false)
				{
					Rectangle spanRect = RangeToRectangle(cellRange);
					base.PaintCell(graphics, cellContext, spanRect);

					mDrawnRange.Add(cellRange);
				}
			}
		}
		#endregion

		#region Sort

		private bool m_CustomSort = false;

		/// <summary>
		/// Gets or sets if when calling SortRangeRows method use a custom sort or an automatic sort. Default = false (automatic)
		/// </summary>
		[DefaultValue(false)]
		public bool CustomSort
		{
			get{return m_CustomSort;}
			set{m_CustomSort = value;}
		}

		/// <summary>
		/// Fired when calling SortRangeRows method. If the range contains all the columns this method move directly the row object otherwise move each cell.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSortingRangeRows(SortRangeRowsEventArgs e)
		{
			base.OnSortingRangeRows(e);

			if (CustomSort)
				return;

			if (e.KeyColumn > e.Range.End.Column && e.KeyColumn < e.Range.Start.Column)
				throw new ArgumentException("Invalid range", "e.KeyColumn");

			System.Collections.IComparer cellComparer = e.CellComparer;
			if (cellComparer == null)
				cellComparer = new ValueCellComparer();

			//Sort all the columns (in this case I move directly the row object)
			if (e.Range.ColumnsCount == ColumnsCount)
			{
				RowInfo[] rowInfoToSort = new RowInfo[e.Range.End.Row-e.Range.Start.Row+1];
				Cells.ICell[] cellKeys = new Cells.ICell[e.Range.End.Row-e.Range.Start.Row+1];

				int zeroIndex = 0;
				for (int r = e.Range.Start.Row; r <= e.Range.End.Row;r++)
				{
					cellKeys[zeroIndex] = this[r, e.KeyColumn];

					rowInfoToSort[zeroIndex] = Rows[r];
					zeroIndex++;
				}

				Array.Sort(cellKeys, rowInfoToSort, 0, cellKeys.Length, cellComparer);

				//Apply sort
				if (e.Ascending)
				{
					for (zeroIndex = 0; zeroIndex < rowInfoToSort.Length; zeroIndex++)
					{
						Rows.Swap( rowInfoToSort[zeroIndex].Index, e.Range.Start.Row + zeroIndex);
					}
				}
				else //desc
				{
					for (zeroIndex = rowInfoToSort.Length-1; zeroIndex >= 0; zeroIndex--)
					{
						Rows.Swap( rowInfoToSort[zeroIndex].Index, e.Range.End.Row - zeroIndex);
					}
				}
			}
			else //sort only the specified range
			{
				Cells.ICell[][] l_RangeSort = new Cells.ICell[e.Range.End.Row-e.Range.Start.Row+1][];
				Cells.ICell[] l_CellsKeys = new Cells.ICell[e.Range.End.Row-e.Range.Start.Row+1];

				int zeroRowIndex = 0;
				for (int r = e.Range.Start.Row; r <= e.Range.End.Row;r++)
				{
					l_CellsKeys[zeroRowIndex] = this[r, e.KeyColumn];

					int zeroColIndex = 0;
					l_RangeSort[zeroRowIndex] = new Cells.ICell[e.Range.End.Column-e.Range.Start.Column+1];
					for (int c = e.Range.Start.Column; c <= e.Range.End.Column; c++)
					{
						l_RangeSort[zeroRowIndex][zeroColIndex] = this[r,c];
						zeroColIndex++;
					}
					zeroRowIndex++;
				}

				Array.Sort(l_CellsKeys, l_RangeSort, 0, l_CellsKeys.Length, cellComparer);

				//Apply sort
				zeroRowIndex = 0;
				if (e.Ascending)
				{
					for (int r = e.Range.Start.Row; r <= e.Range.End.Row;r++)
					{
						int zeroColIndex = 0;
						for (int c = e.Range.Start.Column; c <= e.Range.End.Column; c++)
						{
							RemoveCell(r,c);//rimuovo qualunque cella nella posizione corrente
							Cells.ICell tmp = l_RangeSort[zeroRowIndex][zeroColIndex];

							if (tmp!=null && tmp.Grid!=null && tmp.Range.Start.Row>=0 && tmp.Range.Start.Column>=0) //verifico che la cella sia valida
								RemoveCell(tmp.Range.Start.Row, tmp.Range.Start.Column);//la rimuovo dalla posizione precedente

							this[r,c] = tmp;
							zeroColIndex++;
						}
						zeroRowIndex++;
					}
				}
				else //desc
				{
					for (int r = e.Range.End.Row; r >= e.Range.Start.Row;r--)
					{
						int zeroColIndex = 0;
						for (int c = e.Range.Start.Column; c <= e.Range.End.Column; c++)
						{
							RemoveCell(r,c);//rimuovo qualunque cella nella posizione corrente
							Cells.ICell tmp = l_RangeSort[zeroRowIndex][zeroColIndex];

							if (tmp!=null && tmp.Grid!=null && tmp.Range.Start.Row >= 0 && tmp.Range.Start.Column >= 0) //verifico che la cella sia valida
								RemoveCell(tmp.Range.Start.Row, tmp.Range.Start.Column);//la rimuovo dalla posizione precedente

							this[r,c] = tmp;
							zeroColIndex++;
						}
						zeroRowIndex++;
					}
				}
			}
		}

		#endregion
	}
}
