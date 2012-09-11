using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGrid
{

	
	
	/// <summary>
	/// This class implements a RowsSimpleBase class using a DataView bound mode for row count.
	/// </summary>
	public class DataGridRows : RowsSimpleBase
	{
		public DataGridRows(DataGrid grid)
			: base(grid)
		{
			mHeaderHeight = grid.DefaultHeight;
		}

		public new DataGrid Grid
		{
			get { return (DataGrid)base.Grid; }
		}

		/// <summary>
		/// Gets the number of row of the current DataView. Usually this value is automatically calculated and cannot be changed manually.
		/// </summary>
		public override int Count
		{
			get
			{
				if (Grid.DataSource != null)
					if (Grid.DataSource.AllowNew)
						return Grid.DataSource.Count + Grid.FixedRows + 1;
					else
						return Grid.DataSource.Count + Grid.FixedRows;
					else
						return Grid.FixedRows;
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
		public int DataSourceIndexToGridRowIndex(int dataSourceIndex)
		{
			return dataSourceIndex + Grid.FixedRows;
		}


		/// <summary>
		/// Returns the DataRowView object for a given grid row index. Return null if not applicable, for example if the DataSource is null or if the row index requested is a FixedRows
		/// </summary>
		/// <param name="gridRowIndex"></param>
		/// <returns></returns>
		public object IndexToDataSourceRow(int gridRowIndex)
		{
			int dataIndex = IndexToDataSourceIndex(gridRowIndex);

			//Verifico che l'indice sia valido, perchè potrei essere in un caso in cui le righe non combaciano (magari a seguito di un refresh non ancora completo)
			if (Grid.DataSource != null &&
			    dataIndex >= 0 && dataIndex < Grid.DataSource.Count)
				return Grid.DataSource[dataIndex];
			else
				return null;
		}

		/// <summary>
		/// Returns the index for a given item row. -1 if not valid.
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public int DataSourceRowToIndex(object row)
		{
			if (Grid.DataSource != null)
			{
				return Grid.DataSource.IndexOf(row);
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

		private Dictionary<int, int> mRowHeights = new Dictionary<int, int>();

		public void ResetRowHeigth()
		{
			mRowHeights.Clear();
		}

		public void RowDeleted(object row)
		{
			if (row != null && mRowHeights.ContainsKey(row.GetHashCode()))
				mRowHeights.Remove(row.GetHashCode());
		}

		public override int GetHeight(int row)
		{
			if (row == 0)
				return HeaderHeight;
			object objectRow = IndexToDataSourceRow(row);
			if ( objectRow != null && mRowHeights.ContainsKey(objectRow.GetHashCode()) )
				return mRowHeights[objectRow.GetHashCode()];
			else
				return base.GetHeight(row);
		}

		public override void SetHeight(int row, int height)
		{
			if (row == 0)
				HeaderHeight = height;
			else if ( IndexToDataSourceRow(row) != null )
			{
				int hash = IndexToDataSourceRow(row).GetHashCode();
				if ( !mRowHeights.ContainsKey(hash) || mRowHeights[hash] != height )
				{
					mRowHeights[hash] = height;
					PerformLayout();
				}
			}
			else
				base.SetHeight(row, height);
		}
	}

}
