
using System;

namespace SourceGrid.Extensions.PingGrids.Cells
{
	/// <summary>
	/// A cell used as left row selector. Usually used in the DataCell property of a DataGridColumn. If FixedColumns is grater than 0 and the columns are automatically created then the first column is created of this type.
	/// </summary>
	public class RowHeader : SourceGrid.Cells.Virtual.RowHeader
	{
	    public RowHeader()
	    {
	        Model.AddModel(new DataGridRowHeaderModel());
	    }
	}
}
