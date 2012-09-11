/* */

using System;
using System.Drawing;
using NUnit.Framework;

namespace SourceGrid.Tests
{
	[TestFixture]
	public class TestGridVirtual_PositionToRectangle
	{
		[Test]
		public void PositionToCellRange()
		{
			Assert.AreEqual(Rectangle.Empty, new Grid().PositionToRectangle(new Position(10, 10)));
		}
	}
}
