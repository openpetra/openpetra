using QuadTreeLib;
using System;
using NUnit.Framework;

namespace SourceGrid.Tests.QuadTrees
{
	[TestFixture]
	public class TestProportioanteSizeNodeDivider
	{
		[Test]
		public void IsNodeEmpty_Bug()
		{
			var tree = new QuadTree(14, 14);
			
			var range = new Range(1, 1, 1, 1);
			tree.Insert(range);
			Assert.AreEqual(range, tree.QueryFirst(range));
		}
		
		[Test]
		public void CreateNewRoot()
		{
			var tree = new QuadTree(1, 1);
			tree.Grow();
			
			Assert.AreEqual(4, tree.Root.Nodes.Count);
			var node = tree.Root;
			Assert.AreEqual(new Range(1, 1, 2, 2), tree.Bounds);
			Assert.AreEqual(new Range(1, 1, 1, 1), node.Nodes[0].Bounds);
			Assert.AreEqual(new Range(1, 2, 1, 2), node.Nodes[1].Bounds);
			Assert.AreEqual(new Range(2, 1, 2, 1), node.Nodes[2].Bounds);
			Assert.AreEqual(new Range(2, 2, 2, 2), node.Nodes[3].Bounds);
			
			
			tree.Grow();
			Assert.AreEqual(4, tree.Root.Nodes.Count);
			node = tree.Root;
			Assert.AreEqual(new Range(1, 1, 4, 4), tree.Bounds);
			Assert.AreEqual(new Range(1, 1, 2, 2), node.Nodes[0].Bounds);
			Assert.AreEqual(new Range(1, 3, 2, 4), node.Nodes[1].Bounds);
			Assert.AreEqual(new Range(3, 1, 4, 2), node.Nodes[2].Bounds);
			Assert.AreEqual(new Range(3, 3, 4, 4), node.Nodes[3].Bounds);
		}
		
		
		[Test]
		public void CreateNotProportionateNodes()
		{
			var node = new QuadTreeNode(new Range(1, 1, 1000, 100));
			var divider = new ProportioanteSizeNodeDivider();
			divider.CreateSubNodes(node);
			Assert.AreEqual(2, node.Nodes.Count);
			Assert.AreEqual(new Range(1, 1, 500, 100), node.Nodes[0].Bounds);
			Assert.AreEqual(new Range(501, 1, 1000, 100), node.Nodes[1].Bounds);
		}
		
		[Test]
		public void CreateProportionateNodes()
		{
			var node = new QuadTreeNode(new Range(1, 1, 100, 100));
			var divider = new ProportioanteSizeNodeDivider();
			divider.CreateSubNodes(node);
			Assert.AreEqual(4, node.Nodes.Count);
			Assert.AreEqual(new Range(1, 1, 50, 50), node.Nodes[0].Bounds);
			Assert.AreEqual(new Range(1, 51, 50, 100), node.Nodes[1].Bounds);
			Assert.AreEqual(new Range(51, 1, 100, 50), node.Nodes[2].Bounds);
			Assert.AreEqual(new Range(51, 51, 100, 100), node.Nodes[3].Bounds);
		}
		
		
		[Test]
		public void IsProportionateFalse()
		{
			var node = new QuadTreeNode(new Range(0, 0, 1000, 1));
			var divider = new ProportioanteSizeNodeDivider();
			Assert.AreEqual(false, divider.IsProportionate(node));
		}
		
		[Test]
		public void IsProportionateTrue()
		{
			var node = new QuadTreeNode(new Range(0, 0, 1000, 500));
			var divider = new ProportioanteSizeNodeDivider();
			Assert.AreEqual(true, divider.IsProportionate(node));
		}
	}
}
