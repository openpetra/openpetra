/* */

using System;
using NUnit.Framework;

namespace SourceGrid.Tests
{
	[TestFixture]
	public class TestValueCellComparer
	{
		[Test]
		public void Bug_3424()
		{
			// if compared values are incompatible,
			// return -1
			Assert.AreEqual(-1, new ValueCellComparer()
			                .Compare(true, DateTime.Now));
		}
	}
}
