using System;
using System.Collections.Generic;
using System.ComponentModel;

using SourceGrid.Cells;

namespace SourceGrid.Extensions.PingGrids
{
	/// <summary>
	/// A ColumnInfo derived class used to store column informations for a PingGrid control.
	/// Mantains the cell used on this grid and manage the binding to the DataSource using a DataGridValueModel class.
	/// </summary>
	public class PingGridColumn : ColumnInfo
	{
		/// <summary>
		/// Constructor. Create a DataGridColumn class.
		/// </summary>
		/// <param name="grid"></param>
		public PingGridColumn(PingGrid grid)
			: base(grid)
		{
			mHeaderCell = new SourceGrid.Extensions.PingGrids.Cells.ColumnHeader(string.Empty);
			mDataCell = new SourceGrid.Extensions.PingGrids.Cells.Cell();
		}
		
		/// <summary>
		/// Constructor. Create a DataGridColumn class.
		/// </summary>
		public PingGridColumn(PingGrid grid,
		                      ICellVirtual headerCell,
		                      ICellVirtual dataCell,
		                      string propertyName)
			: base(grid)
		{
			mPropertyName = propertyName;
			mHeaderCell = headerCell;
			mDataCell = dataCell;
		}
		
		/// <summary>
		/// Create a DataGridColumn with special cells used for RowHeader, usually used when FixedColumns is 1 for the first column.
		/// </summary>
		/// <param name="grid"></param>
		/// <returns></returns>
		public static PingGridColumn CreateRowHeader(PingGrid grid)
		{
			return new PingGridColumn(grid,
			                          new SourceGrid.Extensions.PingGrids.Cells.Header(),
			                          new SourceGrid.Extensions.PingGrids.Cells.RowHeader(),
			                          null);
		}
		
		public new PingGrid Grid
		{
			get { return (PingGrid)base.Grid; }
		}
		
		private string mPropertyName;
		public string PropertyName
		{
			get { return mPropertyName; }
			set
			{
				mPropertyName = value;
				//mPropertyColumn = null;
			}
		}
		
		/// <summary>
		/// Clears any associated data with DataGridView.
		/// <remarks>PropertyColumn binds to DataTable, calling invalidate will remove
		/// this link</remarks>
		/// </summary>
		[Obsolete]
		public void Invalidate()
		{
			//mPropertyColumn = null;
		}
		
		[Obsolete]
		private PropertyDescriptor mPropertyColumn = null;
		/// <summary>
		/// Gets the property column. Can be null if not bound to a datasource Column.
		/// This field is used for example to support sorting.
		/// </summary>
		[Obsolete]
		public PropertyDescriptor PropertyColumn
		{
			get
			{
				//if (mPropertyColumn == null && Grid.DataSource != null)
				//	mPropertyColumn = Grid.DataSource.GetItemProperty(PropertyName, StringComparison.InvariantCultureIgnoreCase);
				
				return mPropertyColumn;
			}
		}
		
		private ICellVirtual mHeaderCell;
		/// <summary>
		/// Gets or sets the header cell for this column.
		/// Typically is an instance of SourceGrid.Cells.DataGrid.ColumnHeader
		/// </summary>
		public ICellVirtual HeaderCell
		{
			get { return mHeaderCell; }
			set { mHeaderCell = value; }
		}
		
		private ICellVirtual mDataCell;
		/// <summary>
		/// Gets or sets the cell used for this column for all the rows to disply the data
		/// Typically is an instance of SourceGrid.Cells.DataGrid.Cell or other classes of the same namespace
		/// </summary>
		public ICellVirtual DataCell
		{
			get { return mDataCell; }
			set { mDataCell = value; }
		}
		
		[Obsolete]
		private List<Conditions.ICondition> mConditions = new List<Conditions.ICondition>();
		/// <summary>
		/// Gets the conditions used to returns different cell based on the data of the row.
		/// </summary>
		[Obsolete]
		public List<Conditions.ICondition> Conditions
		{
			get { return mConditions; }
		}
		
		[Obsolete]
		private Dictionary<Conditions.ICondition, ICellVirtual> mConditionalCells = new Dictionary<Conditions.ICondition, ICellVirtual>();
		
		
		/// <summary>
		/// Gets the ICellVirtual for the current column and the specified row.
		/// Override this method to provide custom cells, based on the row informations.
		/// </summary>
		/// <param name="gridRow"></param>
		/// <returns></returns>
		public virtual ICellVirtual GetDataCell(int gridRow)
		{
			//object itemRow = Grid.Rows.IndexToDataSourceRow(gridRow);
			
			//Cells.ICellVirtual cell;
			//if (mConditionalCells.TryGetValue(con, out cell) == false)
			//{
			//	cell = con.ApplyCondition(DataCell);
			//	mConditionalCells.Add(con, cell);
			//}
			//
			//return cell;
			
			return DataCell;
		}
	}
}
