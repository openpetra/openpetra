using System;
using SourceGrid.Cells;
using SourceGrid.Cells.Editors;

namespace SourceGrid.Extensions.PingGrids
{
	public class PingGridColumns : ColumnInfoCollection
	{
		public PingGridColumns(PingGrid grid)
			: base(grid)
		{
		}
	
		public new PingGrid Grid
		{
			get { return (PingGrid)base.Grid; }
		}
	
		public new PingGridColumn this[int index]
		{
			get { return base[index] as PingGridColumn; }
		}
	
		/// <summary>
		/// Return the DataColumn object for a given grid column index. Return null if not applicable, for example if the column index requested is a FixedColumns of an unbound column
		/// </summary>
		/// <param name="gridColumnIndex"></param>
		/// <returns></returns>
		[Obsolete]
		public System.ComponentModel.PropertyDescriptor IndexToPropertyColumn(int gridColumnIndex)
		{
			return Grid.Columns[gridColumnIndex].PropertyColumn;
		}
		/// <summary>
		/// Returns the index for a given DataColumn. -1 if not valid.
		/// </summary>
		/// <returns></returns>
		[Obsolete]
		public int DataSourceColumnToIndex(System.ComponentModel.PropertyDescriptor propertyColumn)
		{
			for (int i = 0; i < Grid.Columns.Count; i++)
			{
				if (Grid.Columns[i].PropertyColumn == propertyColumn)
					return i;
			}
	
			return -1;
		}
	
		#region Add Helper methods
		public PingGridColumn Add(string property,
		                          string caption,
		                          Type propertyType)
		{
			var cell = SourceGrid.Extensions.PingGrids.Cells.Cell.Create(propertyType, true);
	
			return Add(property, caption, cell);
		}
	
		public PingGridColumn Add(string property,
		                          string caption,
		                          EditorBase editor)
		{
			var cell = new SourceGrid.Extensions.PingGrids.Cells.Cell();
			cell.Editor = editor;
	
			return Add(property, caption, cell);
		}
	
		public PingGridColumn Add(string property,
		                           string caption,
		                           ICellVirtual cell)
		{
			var col = new PingGridColumn(Grid,
			                              new SourceGrid.Extensions.PingGrids.Cells.ColumnHeader(caption),
			                              cell,
			                              property);
			Insert(Count, col);
	
			return col;
		}
		#endregion
	}
}
