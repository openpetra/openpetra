using System;
using System.Collections.Generic;

namespace SourceGrid
{
	/// <summary>
	/// This interface helps work with spanned ranges collection
	/// 
	/// There are two implementations at the moment.
	/// One is SpannedRangesList,
	/// another is QuadTree implementation, which is much faster.
	/// Look at unit tests for speed comparisons:
	/// TestSpannedCellRnages_Performance: TestBoth.
	/// </summary>
	public interface ISpannedRangesCollection
	{
		/// <summary>
		/// Returns the number of ranges contained
		/// </summary>
		int Count {get;}
		
		void Add(Range range);
		
		/// <summary>
		/// Searches for an old range. If finds, updates 
		/// found region. Else throws RangeNotFoundException
		/// </summary>
		void Update(Range oldRange, Range newRange);
		
		/// <summary>
		/// Increase size up to specified values.
		/// Note that shrinking is not possible
		/// </summary>
		/// <param name="rowCount"></param>
		/// <param name="colCount"></param>
		void Redim(int rowCount, int colCount);
		
		/// <summary>
		/// If does not find, throws RangeNotFoundException
		/// </summary>
		void Remove(Range range);
		
		Range? GetFirstIntersectedRange(Position pos);
		
		List<Range> GetRanges(Range range);
		
		/// <summary>
		/// Returns range which has exactly the same start position
		/// as indicated
		/// </summary>
		/// <param name="start"></param>
		/// <returns></returns>
		Range? FindRangeWithStart(Position start);
		
		Range[] ToArray();
	}
}
