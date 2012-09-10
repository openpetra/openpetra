using System;
using System.Collections.Generic;

namespace SourceGrid.Extensions.PingGrids
{
	/// <summary>
	/// This class implements a RowsSimpleBase class using a DataView bound mode for row count.
	/// </summary>
	public class PingGridRows : RowsSimpleBase
	{
		public PingGridRows(PingGrid grid)
			: base(grid)
		{
			mHeaderHeight = grid.DefaultHeight;
		}
		
		public new PingGrid Grid
		{
			get { return (PingGrid)base.Grid; }
		}
		
		/// <summary>
		/// Gets the number of row of the current DataView. Usually this value is automatically calculated and cannot be changed manually.
		/// </summary>
		public override int Count
		{
			get
			{
				if (Grid.DataSource == null)
					return Grid.FixedRows;
				return Grid.DataSource.Count + Grid.FixedRows;
			}
		}
		
		/// <summary>
		/// Returns the DataSource index for the specified grid row index.
		/// </summary>
		/// <param name="gridRowIndex"></param>
		/// <returns></returns>
		public int IndexToDataSourceIndex(int gridRowIndex)
		{
			int dataIndex = gridRowIndex - Grid.FixedRows;
			return dataIndex;
		}
		
		/// <summary>
		/// Returns the grid index for the specified DataSource index
		/// </summary>
		/// <returns></returns>
		[Obsolete]
		public int DataSourceIndexToGridRowIndex(int dataSourceIndex)
		{
			return dataSourceIndex + Grid.FixedRows;
		}
		
		
		/// <summary>
		/// Returns the DataRowView object for a given grid row index. Return null if not applicable, for example if the DataSource is null or if the row index requested is a FixedRows
		/// </summary>
		/// <param name="gridRowIndex"></param>
		/// <returns></returns>
		[Obsolete]
		public object IndexToDataSourceRow(int gridRowIndex)
		{
			int dataIndex = IndexToDataSourceIndex(gridRowIndex);
			
			//Verifico che l'indice sia valido, perchè potrei essere in un caso in cui le righe non combaciano (magari a seguito di un refresh non ancora completo)
			//if (Grid.DataSource != null &&
			//    dataIndex >= 0 && dataIndex < Grid.DataSource.Count)
			//	return Grid.DataSource[dataIndex];
			//else
				return null;
		}
		
		/// <summary>
		/// Returns the index for a given item row. -1 if not valid.
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		[Obsolete]
		public int DataSourceRowToIndex(object row)
		{
			if (Grid.DataSource != null)
			{
				//return Grid.DataSource.IndexOf(row);
			}
			
			return -1;
		}
		
		private AutoSizeMode mAutoSizeMode = AutoSizeMode.Default;
		public AutoSizeMode AutoSizeMode
		{
			get { return mAutoSizeMode; }
			set { mAutoSizeMode = value; }
		}
		
		public override AutoSizeMode GetAutoSizeMode(int row)
		{
			return mAutoSizeMode;
		}
		
		private int mHeaderHeight;
		/// <summary>
		/// Gets or sets the header height (row 0)
		/// </summary>
		public int HeaderHeight
		{
			get { return mHeaderHeight; }
			set
			{
				if (mHeaderHeight != value)
				{
					mHeaderHeight = value;
					PerformLayout();
				}
			}
		}
		
		[Obsolete]
		private Dictionary<int, int> mRowHeights = new Dictionary<int, int>();
		
		[Obsolete]
		public void ResetRowHeigth()
		{
			mRowHeights.Clear();
		}
		
		[Obsolete]
		public void RowDeleted(object row)
		{
			if (row != null && mRowHeights.ContainsKey(row.GetHashCode()))
				mRowHeights.Remove(row.GetHashCode());
		}
		
		public override int GetHeight(int row)
		{
			if (row == 0)
				return HeaderHeight;
			return base.GetHeight(row);
		}
		
		public override void SetHeight(int row, int height)
		{
			if (row == 0)
				HeaderHeight = height;
			base.SetHeight(row, height);
		}
	}
}
