using SourceGrid.Utils;
using System;
using System.Collections.Generic;
using QuadTreeLib;

namespace SourceGrid.Tests.Performance
{
	public class RangeGetter
	{
		public int TotalQueries = 0;
		public double TotalTimeSpent = 0;
		public double AverageTimeSpent = 0;
		
		public RangeGetter()
		{
		}
		
		public List<Range> GetRangesFirst(QuadTree tree)
		{
			List<Range> results = new List<Range>();
			
			for (var col = 0; col < tree.Bounds.ColumnsCount; col++)
				for (var row = 0; row < tree.Bounds.RowsCount; row++)
			{
				Range? range = null;
				IPerformanceCounter counter = null;
				using (counter = new PerformanceCounter())
				{
					range = tree.QueryFirst(new Position(row, col));
					
				}
				TotalQueries ++;
				TotalTimeSpent += counter.GetMilisec();
				if (range != null)
					results.Add(range.Value);
			}
			AverageTimeSpent = TotalTimeSpent / TotalQueries;
			return results;
		}
		
		public List<Range> GetRanges(QuadTree tree)
		{
			List<Range> results = new List<Range>();
			
			for (var col = 0; col < tree.Bounds.ColumnsCount; col++)
				for (var row = 0; row < tree.Bounds.RowsCount; row++)
			{
				List<Range> ranges = null;
				IPerformanceCounter counter = null;
				using (counter = new PerformanceCounter())
				{
					ranges = tree.Query(new Position(row, col));
					
				}
				TotalQueries ++;
				TotalTimeSpent += counter.GetMilisec();
				results.AddRange(ranges);
			}
			AverageTimeSpent = TotalTimeSpent / TotalQueries;
			return results;
		}
		
		public List<Range> GetRanges(RangeCreator ranges)
		{
			List<Range> results = new List<Range>();
			for (var col = 0; col < ranges.ColCount; col++)
				for (var row = 0; row < ranges.RowCount; row++)
			{
				Range? index;
				IPerformanceCounter counter = null;
				using (counter = new PerformanceCounter())
				{
					index = ranges.SpannedRangesList.GetFirstIntersectedRange(new Position(row, col));
				}
				TotalQueries ++;
				TotalTimeSpent += counter.GetMilisec();
				if (index != null)
					results.Add(index.Value);
			}
			AverageTimeSpent = TotalTimeSpent / TotalQueries;
			return results;
		}
		
		
	}
}
