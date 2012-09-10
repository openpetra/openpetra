/*
 * Created by SharpDevelop.
 * User: darius.damalakas
 * Date: 2008-12-10
 * Time: 17:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;
using SourceGrid;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using SourceGrid.Selection;

namespace SourceGrid.Tests.Selection
{
	public class TestRowSelectionHelper
	{
		public Grid CreateGridWithRows(int rowCount)
		{
			Grid grid1 = new Grid();
			grid1.ColumnsCount = 1;
			grid1.SelectionMode = GridSelectionMode.Row;
	
			for (int r = 0; r < rowCount; r++)
			{
				grid1.Rows.Insert(r);
				grid1[r, 0] = new SourceGrid.Cells.Cell("Hello " + r.ToString(), typeof(string));
			}
			
			grid1.AutoSizeCells();
			grid1.SelectionMode = GridSelectionMode.Row;
			return grid1;
		}
	}
}
