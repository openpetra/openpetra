using System;

namespace SourceGrid.Tests.Performance
{
	public class RangeCreator
	{
		public int RowCount {get;set;}
		public int ColCount {get;set;}
		public int Span {get;set;}
		public SpannedRangesList SpannedRangesList{get;set;}
		
		public RangeCreator(int rowCount, int colCount, int span)
			:this(rowCount, colCount)
		{
			this.Span = span;
		}
		
		public RangeCreator(int rowCount, int colCount)
		{
			RowCount = rowCount;
			ColCount = colCount;
			Span = 3;
		}
		public RangeCreator CreateRanges()
		{
			var grid = new Grid();
			grid.Redim(RowCount, ColCount);
			SpannedRangesList = new SpannedRangesList();
			for (int r = 0; r < RowCount; r++)
				for (int c = 0; c < ColCount; c++)
			{
				if (c + Span < ColCount)
				{
					SpannedRangesList.Add(new Range(r, c, r, c + Span));
					c += Span;
				}
			}
			return this;
		}
	}
}
