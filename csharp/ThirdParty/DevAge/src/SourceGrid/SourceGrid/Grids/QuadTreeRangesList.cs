using System;
using QuadTreeLib;

namespace SourceGrid
{
	/// <summary>
	/// This is a QuadTree implementation for cell ranges.
	/// Performance is definetely not O(n*n) as in simple
	/// list of ranges
	/// 
	/// Have a look at these numbers to see the difference:
	/// 
	/// with simple List of Ranges
	/// Total 30000 spanned ranges
	/// Total result count 90000
	/// Total time 98541 ms
	/// Average time 0,98541
	/// Total queries 100000
	/// 
	/// testing quad tree
	/// Total 30000 spanned ranges
	/// Total result count 90000
	/// Total time 1495 ms
	/// Average time 0,01495
	/// Total queries 100000
	/// max tree depth: 11
	/// 
	/// </summary>
	public class QuadTreeRangesList : QuadTree, ISpannedRangesCollection
	{
		public QuadTreeRangesList(Range bounds) : base(bounds)
		{
			
		}
		
		public Range[] ToArray()
		{
			return base.Contents.ToArray();
		}
		
		
		public new void Remove(Range range)
		{
			base.Remove(range);
		}
		
		public void Redim(int rowCount, int colCount)
		{
			while (this.Bounds.RowsCount <= rowCount)
			{
				base.Grow();
			}
			while (this.Bounds.ColumnsCount <= colCount)
			{
				base.Grow();
			}
		}
		
		public void Update(Range oldRange, Range newRange)
		{
			var range = base.QueryFirst(oldRange.Start);
			if (range == null)
				throw new RangeNotFoundException();
			Remove(oldRange);
			Insert(newRange);
		}
		
		public void Add(Range range)
		{
			base.Insert(range);
		}
		
		public System.Collections.Generic.List<Range> GetRanges(Range range)
		{
			return base.Query(range);
		}
		
		
		public Range? GetFirstIntersectedRange(Position pos)
		{
			var result = base.QueryFirst(pos);
			if (result == null)
				return null;
			return result;
		}
		
		public Range? FindRangeWithStart(Position start)
		{
			var result = base.QueryFirst(start);
			return result;
		}
	}
}
