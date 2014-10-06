using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGrid.Decorators
{
	public class DecoratorSelection : DecoratorBase
	{
		public DecoratorSelection(Selection.SelectionBase selection)
		{
			mSelection = selection;
		}

		private Selection.SelectionBase mSelection;

		public override bool IntersectWith(Range range)
		{
			return mSelection.IntersectsWith(range);
		}

		public override void Draw(RangePaintEventArgs e)
		{
			RangeRegion region = mSelection.GetSelectionRegion();

			if (region.IsEmpty())
				return;

			// get visible range for scrollable area
            // AlanP: June 2014.  Changed the call from RangeAtAreaExpanded to RangeAtArea to fix a bug that occurred on Partner/Find screen
            // The grid on this screen has 3 fixed columns on the left but the highlight was always applied to the cell 'behind' the last fixed column
            //   as well as the rest of the row.  This is because of getting the 'expanded' range.
            // To be honest I don't understand what the expanded range is for.  The comment for it is not helpful (to me anyway).
            // It may be something to do with drawing the focus cell?  If we see a problem later we could revies this code
            //Range visibleScrollabeRange = mSelection.Grid.RangeAtAreaExpanded(CellPositionType.Scrollable);
            Range visibleScrollableRange = mSelection.Grid.RangeAtArea(CellPositionType.Scrollable);
			
			System.Drawing.Brush brush = e.GraphicsCache.BrushsCache.GetBrush(mSelection.BackColor);

            // In OP we would like to have the rows of fixed columns highlighted as well - this seems non-standard
            // This code is the same as is used in the standard code below, but for the FixedLeft Range
            Range visibleFixedLeftRange = mSelection.Grid.RangeAtArea(CellPositionType.FixedLeft);
            foreach (Range rangeToLoop in region)
            {
                Range rng = visibleFixedLeftRange.Intersect(rangeToLoop);
                System.Drawing.Rectangle rectToDraw = e.Grid.RangeToRectangle(rng);
                if (rectToDraw == System.Drawing.Rectangle.Empty)
                    continue;

                System.Drawing.Region regionToDraw = new System.Drawing.Region(rectToDraw);
                e.GraphicsCache.Graphics.FillRegion(brush, regionToDraw);
            }

			// Deal with the focus rectangle...  The original grid code does not seem to support the focus being on a fixed column, which seems like a bug
            //  but does not affect OpenPetra
            CellContext focusContext = new CellContext(e.Grid, mSelection.ActivePosition);
			// get focus rectangle
			// clipped to visible range
            Range focusClippedRange = visibleScrollableRange.Intersect(new Range(mSelection.ActivePosition, mSelection.ActivePosition));
			System.Drawing.Rectangle focusRect = e.Grid.PositionToRectangle(focusClippedRange.Start);

			//Draw each selection range
			foreach (Range rangeToLoop in region)
			{
				// intersect given range with visible range
				// this way we ensure we don't loop through thousands
				// of rows to calculate rectToDraw
                Range rng = visibleScrollableRange.Intersect(rangeToLoop);

				System.Drawing.Rectangle rectToDraw = e.Grid.RangeToRectangle(rng);
				if (rectToDraw == System.Drawing.Rectangle.Empty)
					continue;

				System.Drawing.Region regionToDraw = new System.Drawing.Region(rectToDraw);

				if (rectToDraw.IntersectsWith(focusRect))
					regionToDraw.Exclude(focusRect);

				e.GraphicsCache.Graphics.FillRegion(brush, regionToDraw);

				//Draw the border only if there isn't a editing cell
				// and is the range that contains the focus or there is a single range
				if (rng.Contains(mSelection.ActivePosition) || region.Count == 1)
				{
					if (focusContext.IsEditing() == false)
						mSelection.Border.Draw(e.GraphicsCache, rectToDraw);
				}
			}

			//Draw Focus
			System.Drawing.Brush brushFocus = e.GraphicsCache.BrushsCache.GetBrush(mSelection.FocusBackColor);
			e.GraphicsCache.Graphics.FillRectangle(brushFocus, focusRect);
		}
	}
}
