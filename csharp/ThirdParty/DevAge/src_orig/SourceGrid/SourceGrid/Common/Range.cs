using System;
using System.Drawing;

namespace SourceGrid
{
	/// <summary>
	/// Represents a range of cells. Once created cannot be modified. This Range has always Start in the Top-Left, and End in the Bottom-Right (see Normalize method).
	/// </summary>
	[Serializable]
	public struct Range
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_Start"></param>
		/// <param name="p_End"></param>
		public Range(Position p_Start, Position p_End):this(p_Start, p_End, true)
		{
		}
		
		public static Range FromPosition(Position startPosition)
		{
			return new Range(startPosition);
		}

		
		public static Range From(Position startPosition, int rowCount, int colCount)
		{
			var range = new Range(startPosition);
			range.RowsCount = rowCount;
			range.ColumnsCount = colCount;
			return range;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_StartRow"></param>
		/// <param name="p_StartCol"></param>
		/// <param name="p_EndRow"></param>
		/// <param name="p_EndCol"></param>
		public Range(int p_StartRow, int p_StartCol, int p_EndRow, int p_EndCol)
		{
			m_Start = new Position(p_StartRow, p_StartCol);
			m_End = new Position(p_EndRow, p_EndCol);

			Normalize();
		}

		private Range(Position p_Start, Position p_End, bool p_bCheck)
		{
			m_Start = p_Start;
			m_End = p_End;

			if (p_bCheck)
				Normalize();
		}

		static Range()
		{
			Empty = new Range(Position.Empty, Position.Empty, false);
		}

		private Position m_Start, m_End;

		public Position Start
		{
			get{return m_Start;}
			//set{m_Start = value; Normalize();}
		}

		public Position End
		{
			get{return m_End;}
			//set{m_End = value; Normalize();}
		}


		/// <summary>
		/// Move the current range to the specified position, leaving the current ColumnsCount and RowsCount
		/// </summary>
		/// <param name="p_StartPosition"></param>
		public void MoveTo(Position p_StartPosition)
		{
			int l_ColCount = ColumnsCount;
			int l_RowCount = RowsCount;
			m_Start = p_StartPosition;
			RowsCount = l_RowCount;
			ColumnsCount = l_ColCount;
		}

		/// <summary>
		/// Sets or Gets the columns count (End.Column-Start.Column)
		/// </summary>
		public int ColumnsCount
		{
			get
			{
				return (m_End.Column - m_Start.Column)+1;
			}
			set
			{
				if (value<=0) //facendo questo controllo non serve richiamare Normalize
					throw new SourceGridException("Invalid columns count");
				m_End = new Position(m_End.Row, m_Start.Column+value-1);
			}
		}

		/// <summary>
		/// Sets or Gets the rows count (End.Row-Start.Row)
		/// </summary>
		public int RowsCount
		{
			get
			{
				return (m_End.Row - m_Start.Row)+1;
			}
			set
			{
				if (value<=0) //facendo questo controllo non serve richiamare Normalize
					throw new SourceGridException("Invalid columns count");
				m_End = new Position(m_Start.Row+value-1, m_End.Column);
			}
		}

		/// <summary>
		/// Construct a Range of a single cell
		/// </summary>
		/// <param name="p_SinglePosition"></param>
		public Range(Position p_SinglePosition):this(p_SinglePosition, p_SinglePosition, false)
		{
		}

		/// <summary>
		/// Represents an empty range
		/// </summary>
		public readonly static Range Empty;

		/// <summary>
		/// Check and fix the range to always have Start smaller than End
		/// </summary>
		private void Normalize()
		{
			int l_MinRow, l_MinCol, l_MaxRow, l_MaxCol;
			
			if (m_Start.Row < m_End.Row)
				l_MinRow = m_Start.Row;
			else
				l_MinRow = m_End.Row;

			if (m_Start.Column < m_End.Column)
				l_MinCol = m_Start.Column;
			else
				l_MinCol = m_End.Column;


			if (m_Start.Row > m_End.Row)
				l_MaxRow = m_Start.Row;
			else
				l_MaxRow = m_End.Row;

			if (m_Start.Column > m_End.Column)
				l_MaxCol = m_Start.Column;
			else
				l_MaxCol = m_End.Column;

			m_Start = new Position(l_MinRow, l_MinCol);
			m_End = new Position(l_MaxRow, l_MaxCol);
		}
		/// <summary>
		/// Returns true if the specified row is present in the current range.
		/// </summary>
		/// <param name="p_Row"></param>
		/// <returns></returns>
		public bool ContainsRow(int p_Row)
		{
			return (p_Row >= m_Start.Row &&
				p_Row <= m_End.Row);
		}
		/// <summary>
		/// Returns true if the specified column is present in the current range.
		/// </summary>
		/// <param name="p_Col"></param>
		/// <returns></returns>
		public bool ContainsColumn(int p_Col)
		{
			return (p_Col >= m_Start.Column &&
				p_Col <= m_End.Column);
		}
		/// <summary>
		/// Returns true if the specified cell position is present in the current range.
		/// </summary>
		/// <param name="p_Position"></param>
		/// <returns></returns>
		public bool Contains(Position p_Position)
		{
			return (p_Position.Row >= m_Start.Row &&
					p_Position.Column >= m_Start.Column &&
					p_Position.Row <= m_End.Row &&
					p_Position.Column <= m_End.Column);
		}

		/// <summary>
		/// Returns true if the specified range is present in the current range.
		/// </summary>
		/// <param name="p_Range"></param>
		/// <returns></returns>
		public bool Contains(Range p_Range)
		{
			return (Contains(p_Range.Start) &&
					Contains(p_Range.End));
		}

		/// <summary>
		/// Determines if the current range is empty
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty()
		{
			return (Start.IsEmpty() || End.IsEmpty());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		/// <returns></returns>
		public static bool operator == (Range Left, Range Right)
		{
			return Left.Equals(Right);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		/// <returns></returns>
		public static bool operator != (Range Left, Range Right)
		{
			return !Left.Equals(Right);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return m_Start.Row;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p_Range"></param>
		/// <returns></returns>
		public bool Equals(Range p_Range)
		{
			return (Start.Equals(p_Range.Start) && End.Equals(p_Range.End));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return Equals((Range)obj);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public PositionCollection GetCellsPositions()
		{
			PositionCollection l_List = new PositionCollection();
			for (int r = Start.Row; r <= End.Row; r++)
				for (int c = Start.Column; c <= End.Column; c++)
					l_List.Add(new Position(r,c));

			return l_List;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Start.ToString() + " to " + End.ToString();
		}


		/// <summary>
		/// Returns a range with the smaller Start and the bigger End. The Union of the 2 Range. If one of the range is empty then the return is the other range.
		/// </summary>
		/// <param name="p_Range1"></param>
		/// <param name="p_Range2"></param>
		/// <returns></returns>
		public static RangeRegion Union(Range p_Range1, Range p_Range2)
		{
			RangeRegion range = new RangeRegion();
			range.Add(p_Range1);
			range.Add(p_Range2);
			return range;
		}

		/// <summary>
		/// Returns a range with the smaller Start and the bigger End. The Union of the 2 Range. If one of the range is empty then the return is the other range.
		/// </summary>
		/// <param name="p_Range1"></param>
		/// <param name="p_Range2"></param>
		/// <returns></returns>
		public static Range GetBounds(Range p_Range1, Range p_Range2)
		{
			if (p_Range1.IsEmpty())
				return p_Range2;
			else if (p_Range2.IsEmpty())
				return p_Range1;
			else
				return new Range(Position.Min(p_Range1.Start, p_Range2.Start),
					Position.Max(p_Range1.End, p_Range2.End), false);
		}

		/// <summary>
		/// Returns the intersection between the 2 Range. If one of the range is empty then the return is empty.
		/// </summary>
		/// <param name="p_Range1"></param>
		/// <param name="p_Range2"></param>
		/// <returns></returns>
		public static Range Intersect(Range p_Range1, Range p_Range2)
		{
			if (p_Range1.IsEmpty() || p_Range2.IsEmpty())
				return Range.Empty;

			Position startNew = Position.Max(p_Range1.Start, p_Range2.Start);
			Position endNew = Position.Min(p_Range1.End, p_Range2.End);

			if (startNew.Column > endNew.Column ||
				startNew.Row > endNew.Row)
				return Range.Empty;
			else
				return new Range(startNew, endNew, false);
		}

		/// <summary>
		/// Returns the intersection between the 2 Range. If one of the range is empty then the return is empty.
		/// </summary>
		/// <param name="p_Range"></param>
		/// <returns></returns>
		public Range Intersect(Range p_Range)
		{
			return Intersect(this, p_Range);
		}

		/// <summary>
		/// Returns true if the specified range intersects (one or more cells) with the current range.
		/// If one of the range is empty then the return is false.
		/// </summary>
		/// <param name="p_Range1"></param>
		/// <param name="p_Range2"></param>
		/// <returns></returns>
		public static bool IntersectsWith(Range p_Range1, Range p_Range2)
		{
			return Intersect(p_Range1, p_Range2).IsEmpty() == false;
		}

		/// <summary>
		/// Returns true if the specified range intersects (one or more cells) with the current range.
		/// If one of the range is empty then the return is false.
		/// </summary>
		/// <param name="p_Range"></param>
		/// <returns></returns>
		public bool IntersectsWith(Range p_Range)
		{
			return IntersectsWith(this, p_Range);
		}

		/// <summary>
		/// Return all the cells that don't intersect with the specified cells. (Remove the specified cells from the current cells ad returns the remaining cells)
		/// </summary>
		/// <param name="range"></param>
		/// <returns></returns>
		public RangeRegion Exclude(Range range)
		{
			RangeRegion excluded;

			Range intersection = Intersect(range);
			if (intersection.IsEmpty())
			{
				excluded = new RangeRegion(this);
			}
			else
			{
				excluded = new RangeRegion();

				//Top Left
				if (this.Start.Row < intersection.Start.Row && 
					this.Start.Column < intersection.Start.Column)
					excluded.Add( new Range(this.Start.Row, this.Start.Column, intersection.Start.Row - 1, intersection.Start.Column - 1) );

				//Top
				if (this.Start.Row < intersection.Start.Row)
					excluded.Add( new Range(this.Start.Row, intersection.Start.Column, intersection.Start.Row - 1, intersection.End.Column) );

				//Top Right
				if (this.Start.Row < intersection.Start.Row && 
					this.End.Column > intersection.End.Column)
					excluded.Add( new Range(this.Start.Row, intersection.End.Column + 1, intersection.Start.Row -1, this.End.Column) );

				//----------

				//Left
				if (this.Start.Column < intersection.Start.Column)
					excluded.Add( new Range(intersection.Start.Row, this.Start.Column, intersection.End.Row, intersection.Start.Column -1) );

				//Right
				if (this.End.Column > intersection.End.Column)
					excluded.Add( new Range(intersection.Start.Row, intersection.End.Column + 1, intersection.End.Row, this.End.Column) );

				//--------

				//Bottom Left
				if (this.End.Row > intersection.End.Row &&
					this.Start.Column < intersection.Start.Column)
					excluded.Add( new Range(intersection.End.Row + 1, this.Start.Column, this.End.Row, intersection.Start.Column - 1) );

				//Bottom
				if (this.End.Row > intersection.End.Row)
					excluded.Add( new Range(intersection.End.Row + 1, intersection.Start.Column, this.End.Row, intersection.End.Column) );

				//Bottom Right
				if (this.End.Row > intersection.End.Row &&
					this.End.Column > intersection.End.Column)
					excluded.Add( new Range(intersection.End.Row + 1, intersection.End.Column + 1, this.End.Row, this.End.Column) );
			}

			return excluded;
		}
	}

	/// <summary>
	/// Interface that rappresent a range of the grid. (RangeFullGridNoFixedRows, RangeFullGridNoFixedCols, RangeFixedRows, RangeFixedCols, Range)
	/// This class is used to calculate a real Range structure at runtime.
	/// </summary>
	public interface IRangeLoader
	{
		/// <summary>
		/// Rectangle that contains the range.
		/// </summary>
		Range GetRange(GridVirtual p_grid);
	}

	/// <summary>
	/// Represents a range that contains all the grid
	/// </summary>
	public class RangeFullGrid : IRangeLoader 
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RangeFullGrid()
		{
		}

		/// <summary>
		/// Returns the Range struct from the specific instance
		/// </summary>
		/// <param name="p_Grid"></param>
		/// <returns></returns>
		public Range GetRange(GridVirtual p_Grid)
		{
			return p_Grid.CompleteRange;
		}
	}

	/// <summary>
	/// Represents a range that contains all the grid with no fixed rows
	/// </summary>
	public class RangeFullGridNoFixedRows : IRangeLoader 
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RangeFullGridNoFixedRows()
		{
		}

		/// <summary>
		/// Returns the Range struct from the specific instance
		/// </summary>
		/// <param name="p_Grid"></param>
		/// <returns></returns>
		public Range GetRange(GridVirtual p_Grid)
		{
			if (p_Grid.Rows.Count>=p_Grid.FixedRows)
				return new Range(p_Grid.FixedRows,0,p_Grid.Rows.Count-1,p_Grid.Columns.Count-1);
			else
				return Range.Empty;
		}
	}
	/// <summary>
	/// Represents a range that contains all the grid with no fixed cols
	/// </summary>
	public class RangeFullGridNoFixedCols : IRangeLoader 
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RangeFullGridNoFixedCols()
		{
		}

		/// <summary>
		/// Returns the Range struct from the specific instance
		/// </summary>
		/// <param name="p_Grid"></param>
		/// <returns></returns>
		public Range GetRange(GridVirtual p_Grid)
		{
			if (p_Grid.Columns.Count >= p_Grid.FixedColumns)
				return new Range(0,p_Grid.FixedColumns,p_Grid.Rows.Count-1, p_Grid.Columns.Count-1);
			else
				return Range.Empty;
		}
	}


	/// <summary>
	/// Represents a range that contains only fixed rows
	/// </summary>
	public class RangeFixedRows : IRangeLoader 
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RangeFixedRows()
		{
		}

		/// <summary>
		/// Returns the Range struct from the specific instance
		/// </summary>
		/// <param name="p_Grid"></param>
		/// <returns></returns>
		public Range GetRange(GridVirtual p_Grid)
		{
			if (p_Grid.Rows.Count>=p_Grid.FixedRows)
				return new Range(0,0,p_Grid.FixedRows,p_Grid.Columns.Count-1);
			else
				return Range.Empty;
		}
	}
	/// <summary>
	/// Represents a range that contains only fixed cols
	/// </summary>
	public class RangeFixedCols : IRangeLoader 
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RangeFixedCols()
		{
		}

		/// <summary>
		/// Returns the Range struct from the specific instance
		/// </summary>
		/// <param name="p_Grid"></param>
		/// <returns></returns>
		public Range GetRange(GridVirtual p_Grid)
		{
			if (p_Grid.Columns.Count >= p_Grid.FixedColumns)
				return new Range(0, 0, p_Grid.Rows.Count-1, p_Grid.FixedColumns);
			else
				return Range.Empty;
		}
	}

	/// <summary>
	/// Range custom
	/// </summary>
	public class RangeLoader : IRangeLoader
	{
		private Range mRange;
		/// <summary>
		/// Constructor
		/// </summary>
		public RangeLoader(Range range)
		{
			mRange = range;
		}

		/// <summary>
		/// Gets or sets the Range loaded in the class.
		/// </summary>
		public Range Range
		{
			get{return mRange;}
			set{mRange = value;}
		}

		/// <summary>
		/// Returns the Range struct from the specific instance
		/// </summary>
		/// <param name="p_Grid"></param>
		/// <returns></returns>
		public virtual Range GetRange(GridVirtual p_Grid)
		{
			return mRange;
		}
	}
}
