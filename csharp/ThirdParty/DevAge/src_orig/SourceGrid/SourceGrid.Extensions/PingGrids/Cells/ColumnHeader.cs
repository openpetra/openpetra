
using System;

namespace SourceGrid.Extensions.PingGrids.Cells
{
	/// <summary>
	/// A cell header used for the columns. Usually used in the HeaderCell property of a DataGridColumn.
	/// </summary>
	public class ColumnHeader : SourceGrid.Cells.Virtual.ColumnHeader
	{
	    public ColumnHeader(string pCaption)
	    {
	        Model.AddModel(new SourceGrid.Cells.Models.ValueModel(pCaption));
	    }
	}
}
