using System;
using System.Windows.Forms;
using SourceGrid;
using SourceGrid.Cells;

namespace WindowsFormsSample
{
	public class FrmSample21Events
	{
		public Position LastPosition {get;set;}
		public Grid Grid{get;set;}
		
		public FrmSample21Events(Grid grid)
		{
			this.Grid = grid;
		}
		
		public void AddEmptyCells(Range range)
		{
			for (int i = range.Start.Row; i <= range.End.Row; i++)
				for (int i1 = range.Start.Column; i1 <= range.End.Column; i1++)
			{
				if (this.Grid[i, i1] == null)
					Grid[i, i1] = new Cell();
			}
		}
		
		public ToolStripMenuItem GetInsertRowItem()
		{
			var item = new ToolStripMenuItem("Insert row");
			
			item.Click += delegate
			{
				if (LastPosition == Position.Empty)
					return;
				var row = LastPosition.Row;
				Grid.Rows.Insert(row);
				AddEmptyCells(new Range(row, 0, row, Grid.Columns.Count - 1));
				Grid.Selection.ResetSelection(true);
			};
			return item;
		}
		
		public ToolStripMenuItem GetRemoveRowItem()
		{
			var item = new ToolStripMenuItem("Remove row");
			
			item.Click += delegate
			{
				if (LastPosition == Position.Empty)
					return;
				Grid.Rows.Remove(LastPosition.Row);
				Grid.Selection.ResetSelection(true);
			};
			return item;
		}
		
		public ToolStripMenuItem GetInsertColItem()
		{
			var item = new ToolStripMenuItem("Insert colum");
			
			item.Click += delegate
			{
				if (LastPosition == Position.Empty)
					return;
				var col = LastPosition.Column;
				Grid.Columns.Insert(col);
				AddEmptyCells(new Range(0, col, Grid.Rows.Count - 1, col));
				Grid.Selection.ResetSelection(true);
			};
			return item;
		}
		
		public ToolStripMenuItem GetRemoveColItem()
		{
			var item = new ToolStripMenuItem("Remove column");
			
			item.Click += delegate
			{
				if (LastPosition == Position.Empty)
					return;
				Grid.Columns.Remove(LastPosition.Column);
				Grid.Selection.ResetSelection(true);
			};
			return item;
		}
	}
}
