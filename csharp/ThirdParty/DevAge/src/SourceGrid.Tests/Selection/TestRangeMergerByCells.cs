using System;
using NUnit.Framework;
using SourceGrid.Selection;

namespace SourceGrid.Tests.Selection
{
	[TestFixture]
	public class TestRangeMergerByCells
	{
		[Test]
		public void AddTwoAdjancentRanges_ByColumn()
		{
			RangeMergerByCells list = new RangeMergerByCells()
				.AddRange(new Range(0, 0, 0, 4))
				.AddRange(new Range(0, 5, 0, 8));
			
			Assert.AreEqual(1, list.GetSelectedRowRegions().Count);
			Assert.AreEqual(new Range(0, 0, 0, 8), list.GetSelectedRowRegions()[0]);
		}
		
		[Test]
		public void AddTwoAdjancentRanges_ByRow()
		{
			RangeMergerByCells list = new RangeMergerByCells()
				.AddRange(new Range(0, 0, 4, 0))
				.AddRange(new Range(5, 0, 8, 0));
			
			Assert.AreEqual(1, list.GetSelectedRowRegions().Count);
			Assert.AreEqual(new Range(0, 0, 8, 0), list.GetSelectedRowRegions()[0]);
		}
		
		[Test]
		public void AddRanges_WithCascade()
		{
			RangeMergerByCells list = new RangeMergerByCells()
				.AddRange(new Range(0, 0, 5, 0))
				.AddRange(new Range(0, 1, 3, 1))
				.AddRange(new Range(4, 1, 5, 1));
			
			Assert.AreEqual(1, list.GetSelectedRowRegions().Count);
			Assert.AreEqual(new Range(0, 0, 5, 1), list.GetSelectedRowRegions()[0]);
		}
		
		[Test]
		public void AddIntersecting_ButNotAdjanced_Ranges()
		{
			// add two ranges, which can not be joined
			// one of the ranges must be intersected and shrinked
			RangeMergerByCells list = new RangeMergerByCells()
				.AddRange(new Range(0, 0, 5, 0))
				.AddRange(new Range(1, 0, 2, 4));
			
			Assert.AreEqual(2, list.GetSelectedRowRegions().Count);
			Assert.AreEqual(new Range(0, 0, 5, 0), list.GetSelectedRowRegions()[0]);
			Assert.AreEqual(new Range(1, 1, 2, 4), list.GetSelectedRowRegions()[1]);
		}
	}
}
