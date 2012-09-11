/* */

using System;
using NUnit.Framework;
using SourceGrid;
using SourceGrid.Cells;

namespace SourceGrid.Tests
{
	[TestFixture]
	public class TestRangeRegion
	{
		[Test]
		public void GetRowsIndex()
		{
			RangeRegion region = new RangeRegion();
			region.Add(new Range(0, 0, 0, 0));
			region.Add(new Range(2, 2, 2, 2));
			
			int[] indexes = region.GetRowsIndex();
			Assert.AreEqual(2, indexes.Length);
			Assert.AreEqual(0, indexes[0]);
			Assert.AreEqual(2, indexes[1]);
		}
		
		[Test]
		public void SelectionBug()
		{
			// Bug report http://www.devage.com/Forum/ViewTopic.aspx?id=8addb0016ae24d97acdef5183bc745c4#msgae7217a8cdc149d383cfdc207f1dc056
			
			// 1. create new RangeRegion with single Range(3, 3, 3, 3);
			// 2. Add new range witch extends our range by one cell to right 
			// 3. original RangeRegion should fire event Changed,
			// witch should report that a new region was added
			
			// The bug is that the AddedRange reports to be empty
			
			RangeRegion region = new RangeRegion(
				new Range(3, 3, 3, 3));
			region.Changed += delegate (object sender, RangeRegionChangedEventArgs e)
			{
				Assert.AreEqual(1, e.AddedRange.Count);
			};
			region.Add(new Range(3, 3, 3, 4));
		}
	}
}

