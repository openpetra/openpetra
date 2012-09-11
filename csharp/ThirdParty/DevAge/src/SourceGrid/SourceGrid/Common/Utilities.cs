using System;
using System.Collections;
using System.Runtime.Serialization;

namespace SourceGrid
{
	/// <summary>
	/// A comparer used to sort more than one columns.
	/// </summary>
	public class MultiColumnsComparer : IComparer
	{
		private IComparer m_defaultComparer = new SourceGrid.ValueCellComparer();
		private int[] m_SecondarySortColumns;

		public MultiColumnsComparer(params int[] secondarySortColumns)
		{
			m_SecondarySortColumns = secondarySortColumns;
		}

		public virtual System.Int32 Compare(System.Object x, System.Object y)
		{
			int ret = m_defaultComparer.Compare(x, y);
			if (ret == 0)
			{
				SourceGrid.Grid grid = ((SourceGrid.Cells.ICell)x).Grid;
				int rowX = ((SourceGrid.Cells.ICell)x).Range.Start.Row;
				int rowY = ((SourceGrid.Cells.ICell)y).Range.Start.Row;


				int indexOrder = 0;
				while (indexOrder < m_SecondarySortColumns.Length)
				{
					SourceGrid.Cells.ICellVirtual otherX = grid.GetCell(rowX, m_SecondarySortColumns[indexOrder]);
					SourceGrid.Cells.ICellVirtual otherY = grid.GetCell(rowY, m_SecondarySortColumns[indexOrder]);

					int subRet = m_defaultComparer.Compare(otherX, otherY);
					if (subRet != 0)
						return subRet;

					indexOrder++;
				}

				return 0;
			}
			else
				return ret;
		}
	}


	/// <summary>
	/// A comparer for the Cell class. (Not for CellVirtual). Using the DisplayString of the cell.
	/// </summary>
	public class DisplayStringCellComparer : IComparer
	{
		public virtual System.Int32 Compare ( System.Object x , System.Object y )
		{
			//Cell object
			if (x==null && y==null)
				return 0;
			if (x==null)
				return -1;
			if (y==null)
				return 1;

			if (x is IComparable)
				return ((IComparable)x).CompareTo(y);
			if (y is IComparable)
				return (-1* ((IComparable)y).CompareTo(x));

			//Cell.Value object
			string vx = ((Cells.ICell)x).DisplayText;
			string vy = ((Cells.ICell)y).DisplayText;
			if (vx==null && vy==null)
				return 0;
			if (vx==null)
				return -1;
			if (vy==null)
				return 1;

			return vx.CompareTo(vy);
		}
	}

	[Serializable]
	public class SourceGridException : ApplicationException
	{
		public SourceGridException(string p_strErrDescription):
			base(p_strErrDescription)
		{
		}
		public SourceGridException(string p_strErrDescription, Exception p_InnerException):
			base(p_strErrDescription, p_InnerException)
		{
		}
		protected SourceGridException(SerializationInfo p_Info, StreamingContext p_StreamingContext):
			base(p_Info, p_StreamingContext)
		{
		}
	}

	[Serializable]
	public class EditingCellException : SourceGridException
	{
		public EditingCellException(Exception innerException):
			base(innerException.Message, innerException)
		{
		}
		protected EditingCellException(SerializationInfo p_Info, StreamingContext p_StreamingContext):
			base(p_Info, p_StreamingContext)
		{
		}
	}
}
