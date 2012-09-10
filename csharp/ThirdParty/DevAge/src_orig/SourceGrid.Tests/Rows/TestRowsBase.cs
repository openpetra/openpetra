using System;
using NUnit.Framework;
using System.Windows.Forms;

namespace SourceGrid.Tests.Rows
{
	[TestFixture]
	public class TestRowsBase
	{
		[Test]
		public void ClearRows_ShouldClearLinkedControls()
		{
			Grid grid1 = new Grid();
			grid1.Redim(10, 1);
			grid1[0, 0] = new SourceGrid.Cells.Cell();
			RichTextBox rtb = new RichTextBox();
			SourceGrid.LinkedControlValue lk = new SourceGrid.LinkedControlValue(rtb, new SourceGrid.Position(0, 0));
			grid1.LinkedControls.Add(lk);
			grid1.Rows.Clear();
		}
	}
}
