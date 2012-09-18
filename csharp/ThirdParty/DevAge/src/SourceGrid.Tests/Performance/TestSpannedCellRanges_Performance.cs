using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuadTreeLib;
using SourceGrid.Cells;
using SourceGrid.Utils;

namespace SourceGrid.Tests.Performance
{
	[TestFixture]
	public class TestSpannedCellRanges_Performance
	{
		[Test, Explicit]
		public void SpannedCellCreation_Performance()
		{
			// originaly this test run in ~11900 ms
			// after implementing this takes 1350ms :)
			var grid = new Grid();
			var count = 10000;
			var columns = 500;
			grid.Redim(count, columns);
			
			
			using (var counter = new PerformanceCounter())
			{
				for (int i = 0; i < count; i++)
				{
					grid[i, 0] = new Cell();
					grid[i, 0].SetSpan(50, 500);
					
					i+=50;
				}
				Console.WriteLine("Test finished in {0} ms", counter.GetMilisec());
			}
		}
		
		
		[Test, Explicit]
		public void SpannedRangesList_SlowPerformance()
		{
			// this test proves that SpannedRangesList is really slow
			// instead use the QuadTreeRangesList
			
			// Total 12000 spanned ranges
			// Total result count 36000
			// Total time 31495 ms
			// Average time 0,3936875
			// Total queries 80000
			
			var ranges = new RangeCreator(2000, 20, 2).CreateRanges();
			Console.WriteLine("testing foreach");
			Console.WriteLine(string.Format(
				"Total {0} spanned ranges", ranges.SpannedRangesList.Count));
			var rangeGetter = new RangeGetter();
			rangeGetter.GetRanges(ranges);
			
			Console.WriteLine(string.Format(
				"Total result count {0}", rangeGetter.GetRanges(ranges).Count));
			Console.WriteLine(string.Format(
				"Total time {0} ms", rangeGetter.TotalTimeSpent));
			Console.WriteLine(string.Format(
				"Average time {0} ", rangeGetter.AverageTimeSpent));
			Console.WriteLine(string.Format(
				"Total queries {0} ", rangeGetter.TotalQueries));
		}
		
		private void PrintResult(RangeGetter rangeGetter, List<Range> result)
		{
			Console.WriteLine(string.Format(
				"Total result count {0}", result.Count));
			Console.WriteLine(string.Format(
				"Total time {0} ms", rangeGetter.TotalTimeSpent));
			Console.WriteLine(string.Format(
				"Average time {0} ", rangeGetter.AverageTimeSpent));
			Console.WriteLine(string.Format(
				"Total queries {0} ", rangeGetter.TotalQueries));
		}
		
		[Test, Explicit]
		public void TestSpannedRangesList_And_QuadTree()
		{
			int row = 5000;
			int column = 20;
			var ranges = new RangeCreator(row, column, 2).CreateRanges();
			Console.WriteLine();
			Console.WriteLine("testing foreach");
			Console.WriteLine(string.Format(
				"Total {0} spanned ranges", ranges.SpannedRangesList.Count));
			var rangeGetter = new RangeGetter();
			var resultForeach = rangeGetter.GetRanges(ranges);
			PrintResult(rangeGetter, resultForeach);
			
			
			ranges = new RangeCreator(row, column, 2).CreateRanges();
			Console.WriteLine();
			Console.WriteLine("testing quad tree");
			Console.WriteLine(string.Format(
				"Total {0} spanned ranges", ranges.SpannedRangesList.Count));
			var tree = new QuadTree(new Range(0, 0, ranges.RowCount - 1, ranges.ColCount - 1));
			tree.QuadTreeNodeDivider = new ProportioanteSizeNodeDivider();
			tree.Insert(ranges.SpannedRangesList);
			rangeGetter = new RangeGetter();
			var resultQuad = rangeGetter.GetRanges(tree);
			PrintResult(rangeGetter, resultQuad);
			Console.WriteLine(string.Format("max tree depth: {0}", tree.MaxDepth));
			
			Assert.AreEqual(resultQuad.Count, resultForeach.Count);
			
			for (int i = 0; i < resultQuad.Count; i++)
			{
				Assert.AreEqual(resultQuad[i], resultForeach[i]);
			}
		}
		
		[Test, Explicit]
		public void TestQuadTree()
		{
			// shows that the difference between space partitioners
			// is around 60 times  :) nice
			
			// with HalfSizeNodeDivider  -
			// Total 30000 spanned ranges
			// Total result count 90000
			// Total time 81812 ms
			// Average time 0,81812
			// Total queries 100000
			
			// with ProportioanteSizeNodeDivider  -
			// Total 30000 spanned ranges
			// Total result count 90000
			// Total time 1371 ms
			// Average time 0,01371
			// Total queries 100000
			
			
			var ranges = new RangeCreator(10000, 10, 2).CreateRanges();
			Console.WriteLine("testing quad tree");
			Console.WriteLine(string.Format(
				"Total {0} spanned ranges", ranges.SpannedRangesList.Count));
			var tree = new QuadTree(new Range(0, 0, ranges.RowCount - 1, ranges.ColCount - 1));
			tree.QuadTreeNodeDivider = new ProportioanteSizeNodeDivider();
			tree.Insert(ranges.SpannedRangesList);
			var rangeGetter = new RangeGetter();
			Console.WriteLine(string.Format(
				"Total result count {0}", rangeGetter.GetRanges(tree).Count));
			Console.WriteLine(string.Format(
				"Total time {0} ms", rangeGetter.TotalTimeSpent));
			Console.WriteLine(string.Format(
				"Average time {0} ", rangeGetter.AverageTimeSpent));
			Console.WriteLine(string.Format(
				"Total queries {0} ", rangeGetter.TotalQueries));
		}
		
		[Test, Explicit]
		public void TestQuadTree_ReturnFirstResult()
		{
			// shows that there is differnece between
			// returning only the first result, and traversin whole tree
			
			// speed difference is
			// for returning only first - 650 ms
			// returning all - 1450ms
			// around 2x :) nice
			
			var ranges = new RangeCreator(10000, 10, 2).CreateRanges();
			Console.WriteLine("testing quad tree");
			Console.WriteLine(string.Format(
				"Total {0} spanned ranges", ranges.SpannedRangesList.Count));
			var tree = new QuadTree(new Range(0, 0, ranges.RowCount - 1, ranges.ColCount - 1));
			tree.QuadTreeNodeDivider = new ProportioanteSizeNodeDivider();
			tree.Insert(ranges.SpannedRangesList);
			var rangeGetter = new RangeGetter();
			Console.WriteLine(string.Format(
				"Total result count {0}", rangeGetter.GetRangesFirst(tree).Count));
			Console.WriteLine(string.Format(
				"Total time {0} ms", rangeGetter.TotalTimeSpent));
			Console.WriteLine(string.Format(
				"Average time {0} ", rangeGetter.AverageTimeSpent));
			Console.WriteLine(string.Format(
				"Total queries {0} ", rangeGetter.TotalQueries));
		}
		
		[Test]
		public void Test_QuadTree_And_SpannedRangesList_Results_Are_Exactly_Same()
		{
			var ranges = new RangeCreator(1000, 4, 2).CreateRanges();
			var resultSpann = new RangeGetter().GetRanges(ranges);
			
			//var ranges = CreateRanges();
			var tree = new QuadTree(new Range(0, 0, ranges.RowCount, ranges.ColCount));
			tree.Insert(ranges.SpannedRangesList);
			var resultQuad = new RangeGetter().GetRanges(tree);
			
			Assert.AreEqual(resultSpann.Count, resultQuad.Count);
		}
	}
}
