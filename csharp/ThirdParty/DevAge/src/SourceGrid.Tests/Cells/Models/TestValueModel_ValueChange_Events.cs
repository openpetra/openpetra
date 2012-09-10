/* */

using System;
using NUnit.Framework;
using SourceGrid.Cells.Controllers;
using DevAge.ComponentModel;
using SourceGrid.Cells.Models;

namespace SourceGrid.Tests.Cells.Models
{

	
	[TestFixture]
	public class TestValueModel_ValueChange_Events
	{
		[Test]
		public void No_ValueChanged_Event_If_Value_Does_Not_Change()
		{
			
			CustomEvents events = new CustomEvents();
			events.ValueChanged += delegate
			{
				Assert.Fail("We did not change value, no event expected");
			};

			Grid grid = new Grid();
			grid.Redim(1, 1);
			grid[0, 0] = new SourceGrid.Cells.Cell(1, typeof(int));
			grid[0, 0].AddController(events);
			
			CellContext context = new CellContext(grid, new Position(0, 0));
			// assert that we have already 1 inside this cell
			Assert.AreEqual(1, context.Value);
			// change the value of the cell to the same value
			// this should not fire ValueChanged event handler
			context.Cell.Editor.SetCellValue(context, 1);
		}
		
		[Test]
		public void No_ValueChanging_Event_If_Value_Does_Not_Change()
		{
			CustomEvents events = new CustomEvents();
			events.ValueChanging += delegate(object sender, ValueChangeEventArgs e)
			{
				Assert.Fail("We did not change value, no event expected");
			};
			
			Grid grid = new Grid();
			grid.Redim(1, 1);
			grid[0, 0] = new SourceGrid.Cells.Cell(1, typeof(int));
			grid[0, 0].AddController(events);
			
			CellContext context = new CellContext(grid, new Position(0, 0));
			// assert that we have already 1 inside this cell
			Assert.AreEqual(1, context.Value);
			// change the value of the cell to the same value
			// this should not fire ValueChanged event handler
			context.Cell.Editor.SetCellValue(context, 1);
		}
		
		[TestFixtureSetUp]
		public void Init()
		{
		}
	}
}
