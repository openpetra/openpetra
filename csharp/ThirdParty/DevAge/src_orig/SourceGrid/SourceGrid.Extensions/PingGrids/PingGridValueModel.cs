
using System;
using System.ComponentModel;
using DevAge.ComponentModel;
using SourceGrid.Cells.Models;
using SourceGrid.Selection;

namespace SourceGrid.Extensions.PingGrids
{
	/// <summary>
	/// A Model of type IValueModel used for binding the value to a specified property of the bound object.
	/// Used for the DataGrid control.
	/// </summary>
	public class PingGridValueModel : IValueModel
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public PingGridValueModel()
		{
		}
		#region IValueModel Members
	
		public object GetValue(CellContext cellContext)
		{
			var grid = cellContext.Grid as PingGrid;
	
			var propertyName = grid.Columns[cellContext.Position.Column].PropertyName;
	
			int dataIndex = grid.Rows.IndexToDataSourceIndex(cellContext.Position.Row);
	
			//Check if the row is not outside the valid range (for example to handle the new row)
			if (dataIndex >= grid.DataSource.Count)
				return null;
			else
				return grid.DataSource.GetItemValue(dataIndex, propertyName);
		}
	
		public void SetValue(CellContext cellContext, object value)
		{
			throw new NotImplementedException();
			/*DataGrid grid = (DataGrid)cellContext.Grid;
			PropertyDescriptor prop = grid.Columns[cellContext.Position.Column].PropertyColumn;
			object oldValue = GetValue(cellContext);
			
			ValueChangeEventArgs valArgs = new ValueChangeEventArgs(oldValue, value);
			if (cellContext.Grid != null)
				cellContext.Grid.Controller.OnValueChanging(cellContext, valArgs);
	
			
	
			
	
			grid.DataSource.SetEditValue(prop, valArgs.NewValue);
	
			if (cellContext.Grid != null)
				cellContext.Grid.Controller.OnValueChanged(cellContext, EventArgs.Empty);
				*/
		}
		#endregion
	}
}
