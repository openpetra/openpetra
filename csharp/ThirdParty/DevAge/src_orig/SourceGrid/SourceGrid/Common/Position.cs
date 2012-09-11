using System;

namespace SourceGrid
{
	/// <summary>
	/// Represents a cell position (Row, Col). Once created connot be modified
	/// </summary>
	[Serializable]
	public struct Position
	{
		/// <summary>
		/// Constructor
		/// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
		public Position(int row, int col)
		{
            m_Row = row;
            m_Col = col;
		}

		static Position()
		{
			Empty = new Position(c_EmptyIndex, c_EmptyIndex);
		}

		private int m_Row;

		private int m_Col;

		/// <summary>
		/// Row
		/// </summary>
		public int Row
		{
			get{return m_Row;}
		}
		/// <summary>
		/// Column
		/// </summary>
		public int Column
		{
			get{return m_Col;}
		}

		/// <summary>
		/// Empty position
		/// </summary>
		public readonly static Position Empty;

		/// <summary>
		/// Returns true if the current struct is empty
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty()
		{
			return this.Equals(Empty);
		}

		/// <summary>
		/// An empty index constant
		/// </summary>
		public const int c_EmptyIndex = -1;

		/// <summary>
		/// GetHashCode
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return Row;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p_Position"></param>
		/// <returns></returns>
		public bool Equals(Position p_Position)
		{
			return (m_Col == p_Position.m_Col && m_Row == p_Position.m_Row);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return Equals((Position)obj);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		/// <returns></returns>
		public static bool operator == (Position Left, Position Right)
		{
			return Left.Equals(Right);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		/// <returns></returns>
		public static bool operator != (Position Left, Position Right)
		{
			return !Left.Equals(Right);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Row.ToString() + ";" + Column.ToString();
		}

		/// <summary>
		/// Returns a position with the smaller Row and the smaller column
		/// </summary>
		/// <param name="p_Position1"></param>
		/// <param name="p_Position2"></param>
		/// <returns></returns>
		public static Position Min(Position p_Position1, Position p_Position2)
		{
			int l_Row, l_Col;
			if (p_Position1.Row < p_Position2.Row)
				l_Row = p_Position1.Row;
			else
				l_Row = p_Position2.Row;
			if (p_Position1.Column < p_Position2.Column)
				l_Col = p_Position1.Column;
			else
				l_Col = p_Position2.Column;
			return new Position(l_Row, l_Col);
		}
		/// <summary>
		/// Returns a position with the bigger Row and the bigger column
		/// </summary>
		/// <param name="p_Position1"></param>
		/// <param name="p_Position2"></param>
		/// <returns></returns>
		public static Position Max(Position p_Position1, Position p_Position2)
		{
			int l_Row, l_Col;
			if (p_Position1.Row > p_Position2.Row)
				l_Row = p_Position1.Row;
			else
				l_Row = p_Position2.Row;
			if (p_Position1.Column > p_Position2.Column)
				l_Col = p_Position1.Column;
			else
				l_Col = p_Position2.Column;
			return new Position(l_Row, l_Col);
		}
	}


}
