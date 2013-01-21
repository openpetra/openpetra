		/// <summary>
		/// Recalculate the scrollbars position and size.
		/// Use this to refresh scroll bars
		/// </summary>
		public void RecalcCustomScrollBars()
		{
			SuspendLayout();

            /////////////////////////////////////////////////////////////////////
            //ALAN
            if (GetScrollColumns(base.DisplayRectangle.Width) > 0)
            {
                // we definitely need a HScroll based on the base display rectangle
                PrepareScrollBars(true, false);
                if (GetScrollRows(DisplayRectangle.Height) > 0)
                {
                    // we need a VScroll too
                    PrepareScrollBars(true, true);
                }
            }
            else
            {
                // we don't need an HScroll (yet)
                if (GetScrollRows(base.DisplayRectangle.Height) > 0)
                {
                    // we definitely need a VScroll based on the base display rectangle
                    PrepareScrollBars(false, true);
                    if (GetScrollColumns(DisplayRectangle.Width) > 0)
                    {
                        // actually now we need an HScroll after all, because the VScroll has taken up space
                        PrepareScrollBars(true, true);
                    }
                }
                else
                {
                    // No scrolls needed - everything fits in the base display rectangle
                    PrepareScrollBars(false, false);
                }
            }

            //Finally I read the actual values to use (that can be changed because I have called PrepareScrollBars)
            if (VScrollBarVisible)
            {
                int scrollRows = GetScrollRows(DisplayRectangle.Height);
			    scrollRows = scrollRows - GetActualFixedRows();
			    RecalcVScrollBar(scrollRows);
            }
            if (HScrollBarVisible)
            {
                int scrollCols = GetScrollColumns(DisplayRectangle.Width);
			    RecalcHScrollBar(scrollCols);
            }

			//forzo un ridisegno
			InvalidateScrollableArea();

			ResumeLayout(true);
		}

