/*
 * Created by SharpDevelop.
 * User: Ophucius
 * Date: 9/6/2009
 * Time: 12:54 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;

namespace SourceGrid.Tests.ClipBoard
{
	[TestFixture]
	public class TestClipboard
	{
		[Test]
		public void ShouldNotCopyColumnsWith_VisibleFalse()
		{
			Grid grid1 = new Grid();
			grid1.Redim(6, 2);
			
			grid1[0, 0] = new SourceGrid.Cells.Cell("a");
			grid1[0, 1] = new SourceGrid.Cells.Cell("b");
			grid1.Columns[1].Visible = false;
			
			RangeData data = RangeData.LoadData(grid1, new Range(0, 0, 0, 1), CutMode.None);
			Assert.AreEqual(1, data.SourceValues.Length);
			Assert.AreEqual("a", data.SourceValues[0, 0]);
		}
		
		[Test]
		public void ShouldNotCopyVisible_AndShouldPaseIntoColumnsNotVisible()
		{
			Grid grid1 = new Grid();
			grid1.Redim(6, 3);
			
			grid1[0, 0] = new SourceGrid.Cells.Cell("a");
			grid1[0, 1] = new SourceGrid.Cells.Cell("b");
			grid1[0, 2] = new SourceGrid.Cells.Cell("c");
			grid1.Columns[1].Visible = false;
			
			RangeData data = RangeData.LoadData(grid1, new Range(0, 0, 0, 2), CutMode.None);
			
			
			
			grid1[1, 0] = new SourceGrid.Cells.Cell("x", typeof(string));
			grid1[1, 1] = new SourceGrid.Cells.Cell("x", typeof(string));
			grid1[1, 2] = new SourceGrid.Cells.Cell("x", typeof(string));
			
			data.WriteData(grid1, new Position(1, 0));
			Assert.AreEqual("a", grid1[1, 0].Value);
			Assert.AreEqual("c", grid1[1, 1].Value);
			Assert.AreEqual("x", grid1[1, 2].Value);
		}
		
		[Test]
		public void ShouldPaseIntoColumnsWith_VisibleFalse()
		{
			Grid grid1 = new Grid();
			grid1.Redim(6, 4);
			
			grid1[0, 0] = new SourceGrid.Cells.Cell("a");
			grid1[0, 1] = new SourceGrid.Cells.Cell("b");
			grid1[0, 2] = new SourceGrid.Cells.Cell("c");
			grid1[0, 3] = new SourceGrid.Cells.Cell("d");
			
			
			RangeData data = RangeData.LoadData(grid1, new Range(0, 0, 0, 3), CutMode.None);
			
			
			grid1.Columns[1].Visible = false;
			grid1[1, 0] = new SourceGrid.Cells.Cell("x", typeof(string));
			grid1[1, 1] = new SourceGrid.Cells.Cell("x", typeof(string));
			grid1[1, 2] = new SourceGrid.Cells.Cell("x", typeof(string));
			grid1[1, 3] = new SourceGrid.Cells.Cell("x", typeof(string));
			
			data.WriteData(grid1, new Position(1, 0));
			Assert.AreEqual("a", grid1[1, 0].Value);
			Assert.AreEqual("b", grid1[1, 1].Value);
			Assert.AreEqual("c", grid1[1, 2].Value);
			Assert.AreEqual("d", grid1[1, 3].Value);
		}
		
		[Test]
		public void PasteVertically()
		{
			Grid grid1 = new Grid();
			grid1.Redim(6, 3);
			
			grid1[0, 0] = new SourceGrid.Cells.Cell("a");
			grid1[1, 0] = new SourceGrid.Cells.Cell("b");
			grid1[2, 0] = new SourceGrid.Cells.Cell("c");
			
			
			RangeData data = RangeData.LoadData(grid1, new Range(0, 0, 2, 0), CutMode.None);
			
			
			grid1.Columns[1].Visible = false;
			grid1[0, 1] = new SourceGrid.Cells.Cell("x", typeof(string));
			grid1[1, 1] = new SourceGrid.Cells.Cell("x", typeof(string));
			grid1[2, 1] = new SourceGrid.Cells.Cell("x", typeof(string));
			
			data.WriteData(grid1, new Position(0, 1));
			Assert.AreEqual("a", grid1[0, 1].Value);
			Assert.AreEqual("b", grid1[1, 1].Value);
			Assert.AreEqual("c", grid1[2, 1].Value);
		}
	}
}
