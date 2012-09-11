/* */

using System;
using NUnit.Framework;
using SourceGrid.Cells;
using SourceGrid.Extensions.PingGrids;
using SourceGrid.Utils;

namespace SourceGrid.Tests
{
	[TestFixture]
	public class TestPingGrid
	{
		[Test]
		public void CreateEmpty()
		{
			new PingGrid();
		}
	}
}
