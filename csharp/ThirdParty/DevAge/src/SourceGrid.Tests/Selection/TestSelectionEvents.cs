/*
 * Created by SharpDevelop.
 * User: darius.damalakas
 * Date: 2008-12-10
 * Time: 17:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;
using SourceGrid;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using SourceGrid.Selection;

namespace SourceGrid.Tests.Selection
{
	[TestFixture, IgnoreAttribute("Not yet implemented")]
	public class TestSelectionEvents
	{
		[Test]
		public void SubscriptionToEventsAreNotChanged_After_SelectionIsChanged()
		{
			int selectionChangedCount = 0;
			Grid grid = new Grid();
			grid.Redim(5, 5);
			grid[1, 1] = new SourceGrid.Cells.Cell("b");
			
			grid.Selection.SelectionChanged += delegate { selectionChangedCount++; };
			grid.SelectionMode = GridSelectionMode.Row;
			grid.Selection.SelectCell(new Position(1, 1), true);
			Assert.AreEqual(1, selectionChangedCount);
			
		}
	}
}
