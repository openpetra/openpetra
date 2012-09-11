using System;
using NUnit.Framework;
using QuadTreeLib;

namespace SourceGrid.Tests.Performance
{
	[TestFixture]
	public class TestQuadTree
	{
		[Test]
		public void Remove()
		{
			var range = new Range(1, 1, 5, 5);
			var tree = new QuadTree(100, 100)
				.Insert(range)
				.Remove(range);
			Assert.AreEqual(0, tree.Count);
		}
		
		
		[Test]
		public void SinglePosition()
		{
			var range = new Range(1, 1, 5, 5);
			var results = new QuadTree(100, 100)
				.Insert(range)
				.Query(range);
			Assert.AreEqual(1, results.Count);
			Assert.AreEqual(range, results[0]);
		}
		
		[Test]
		public void MultiPosition()
		{
			var range = new Range(1, 1, 1, 5);
			var results = new QuadTree(100, 100)
				.Insert(range)
				.Insert(new Range(3, 1, 4, 4))
				.Query(range);
			Assert.AreEqual(1, results.Count);
			Assert.AreEqual(range, results[0]);
		}
	}
}
