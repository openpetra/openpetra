using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace SourceGrid.Selection
{
	public class RangeComparerByRows : IComparer<Range>
	{
		public int Compare(Range x, Range y)
		{
			if (x.Start.Row == y.Start.Row)
				return 0;
			if (x.Start.Row > y.Start.Row)
				return 1;
			return -1;
		}
	}
}

