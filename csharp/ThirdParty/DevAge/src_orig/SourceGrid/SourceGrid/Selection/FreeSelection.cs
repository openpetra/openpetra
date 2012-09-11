using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGrid.Selection
{
	/// <summary>
	/// Similar to <see cref="RangeMergerByRows"/>, this class merge adjancent
	/// ranges if they are differ only by row or column count.
	/// 
	/// See TestRangeMergerByCells for unit tests, which show how this class works
	/// </summary>
	public class RangeMergerByCells
	{
		private List<Range> m_ranges = new List<Range>();
		
		private bool IntersectWithCurrentRangesRecursive(RangeRegion rangeRegionToIntersect)
		{
			// check against each range
			foreach (Range range in m_ranges)
			{
				RangeRegion excludedResults = null;
				Range remove = Range.Empty;
				bool excluded = false;
				// loop through our new ranges
				foreach (Range rangeToIntersect in rangeRegionToIntersect)
				{
					// if we intersect with at least one range
					// get the exclusion range.
					// mark a flag, that we need to remove our 
					// range, and add an excluded range
					if (range.IntersectsWith(rangeToIntersect))
					{
						excludedResults = rangeToIntersect.Exclude(range);
						excluded = true;
						remove = rangeToIntersect;
						break;
					}
				}
				if (excluded)
				{
					rangeRegionToIntersect.Remove(remove);
					rangeRegionToIntersect.Add(excludedResults);
					return true;
				}
			}
			return false;
		}
		
		private RangeRegion IntersectWithCurrentRanges(RangeRegion rangeRegionToIntersect)
		{
			while (IntersectWithCurrentRangesRecursive(rangeRegionToIntersect))
			{
			}
			return rangeRegionToIntersect;
		}
		
		public RangeMergerByCells AddRange(Range rangeToAdd)
		{
			RangeRegion rangeRegionToAdd = IntersectWithCurrentRanges(new RangeRegion(rangeToAdd));
			foreach (Range range in rangeRegionToAdd)
			{
				m_ranges.Add(range);
			}
			JoinAdjanced();
			return this;
		}
		
		private void Join(Range range1, Range range2)
		{
			Range result = Range.GetBounds(range1, range2);
			m_ranges.Remove(range1);
			m_ranges.Remove(range2);
			m_ranges.Add(result);
		}
		
		
		private void JoinAdjanced()
		{
			while (JoinAdjancedRecursive())
			{
			}
		}
		
		
		/// <summary>
		/// Repeat this function until it returns false to recursively merge all ranges
		/// </summary>
		/// <returns>true, if at least two ranges were joined into single</returns>
		private bool JoinAdjancedRecursive()
		{
			List<Range> cloneRanges = new List<Range>(m_ranges);
			foreach (Range clonedRange in cloneRanges)
			{
				foreach (Range rangeToTest in m_ranges)
				{
					if (rangeToTest.Equals(clonedRange))
						continue;
					if ((rangeToTest.Start.Row == clonedRange.Start.Row) &&
					    (rangeToTest.End.Row == clonedRange.End.Row))
					{
						if (Math.Abs(rangeToTest.Start.Column - clonedRange.End.Column) == 1)
						{
							Join(rangeToTest, clonedRange);
							return true;
						}
						
					}
					
					if ((rangeToTest.Start.Column == clonedRange.Start.Column) &&
					    (rangeToTest.End.Column == clonedRange.End.Column))
					{
						if (Math.Abs(rangeToTest.Start.Row - clonedRange.End.Row) == 1)
						{
							Join(rangeToTest, clonedRange);
							return true;
						}
						
					}
				}
			}
			return false;
		}
		
		public List<Range> GetSelectedRowRegions()
		{
			return m_ranges;
		}
	}
	
	/// <summary>
	/// A selection class that support free selection of cells (ranges)
	/// </summary>
	public class FreeSelection : SelectionBase
	{
		private RangeRegion mRegion = new RangeRegion();

		public FreeSelection()
		{
			mRegion.Changed += delegate (object sender, RangeRegionChangedEventArgs e) { OnSelectionChanged(e); };
		}

		public override void BindToGrid(GridVirtual p_grid)
		{
			base.BindToGrid(p_grid);

			mDecorator = new Decorators.DecoratorSelection(this);
			Grid.Decorators.Add(mDecorator);
		}

		public override void UnBindToGrid()
		{
			Grid.Decorators.Remove(mDecorator);

			base.UnBindToGrid();
		}

		private Decorators.DecoratorSelection mDecorator;

		public override bool IsSelectedColumn(int column)
		{
			return mRegion.ContainsColumn(column);
		}

		public override void SelectColumn(int column, bool select)
		{
			Range rng = Grid.Columns.GetRange(column);

			SelectRange(rng, select);
		}

		public override bool IsSelectedRow(int row)
		{
			return mRegion.ContainsRow(row);
		}

		public override void SelectRow(int row, bool select)
		{
			Range rng = Grid.Rows.GetRange(row);

			SelectRange(rng, select);
		}

		public override bool IsSelectedCell(Position position)
		{
			return mRegion.Contains(position);
		}

		public override void SelectCell(Position position, bool select)
		{
			SelectRange(Grid.PositionToCellRange(position), select);
		}

		public override bool IsSelectedRange(Range range)
		{
			return mRegion.Contains(range);
		}

		public override void SelectRange(Range range, bool select)
		{
			Range newRange = Grid.RangeToCellRange(range);
			if (select)
				mRegion.Add(ValidateRange(newRange));
			else
				mRegion.Remove(newRange);
		}

		protected override void OnResetSelection()
		{
			mRegion.Clear();
		}

		/// <summary>
		/// Returns true if the selection is empty
		/// </summary>
		/// <returns></returns>
		public override bool IsEmpty()
		{
			return mRegion.IsEmpty();
		}

		/// <summary>
		/// Returns the selected region.
		/// </summary>
		/// <returns></returns>
		public override RangeRegion GetSelectionRegion()
		{
			return new RangeRegion(mRegion);
		}

		/// <summary>
		/// Returns true if the specified selection intersect with the range
		/// </summary>
		/// <param name="rng"></param>
		/// <returns></returns>
		public override bool IntersectsWith(Range rng)
		{
			return mRegion.IntersectsWith(rng);
		}
	}
}
