SourceGrid\SourceGrid\Common\ColumnInfoCollection.cs

		/// <summary>
		/// Auto size the columns calculating the required size only on the rows currently visible
		/// </summary>
		public void AutoSizeView()
		{
            /*
             * Modification by ChristianK:
             * 
             * Include 'Fixed Rows' in the calculation of the Column's AutoSize (as SourceGrid 4.11 did it).
             * -- See file 'readme.txt' for details! --
             */
		    
//            List<int> list = Grid.Rows.RowsInsideRegion(Grid.DisplayRectangle.Y, Grid.DisplayRectangle.Height, true, false);           
            List<int> list = Grid.Rows.RowsInsideRegion(Grid.DisplayRectangle.Y, Grid.DisplayRectangle.Height);

            if (list.Count > 0)
			{
				AutoSize(false, list[0], list[list.Count - 1]);
			}
		}
