using SourceGrid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace QuadTreeLib
{
	public interface IQuadTreeNodeDivider
	{
		void CreateSubNodes(QuadTreeNode parentNode);
		
		/// <summary>
		/// Expand tree by creating new root.
		/// Add old root under new root, and create any
		/// additional tree nodes that are needed
		/// </summary>
		/// <param name="currentRoot"></param>
		/// <returns></returns>
		QuadTreeNode CreateNewRoot(QuadTreeNode currentRoot);
	}
	
	/// <summary>
	/// Tries to divide into nodes, that are more suited for grids,
	/// which expand more into rows than into columns.
	/// In this case, quad tree does not end up divided
	/// into regions where width is very very small.
	/// 
	/// This is much more efficien space partitioning
	/// </summary>
	public class ProportioanteSizeNodeDivider : IQuadTreeNodeDivider
	{
		HalfSizeNodeDivider HalfSizeNodeDivider = new HalfSizeNodeDivider();

		public QuadTreeNode CreateNewRoot(QuadTreeNode currentRoot)
		{
			var bounds = currentRoot.Bounds;
			int startRow = bounds.Start.Row;
			int startCol = bounds.Start.Column;
			int halfCol = bounds.ColumnsCount ;
			int halfRow = bounds.RowsCount;
			var Depth = currentRoot.Depth;
			var QuadTree = currentRoot.QuadTree;
			
			var newRoot = new QuadTreeNode(
				new Range(startRow, startCol, halfCol * 2, halfRow * 2),
				currentRoot.Depth, currentRoot.QuadTree);
			
			
			newRoot.Nodes.Add(currentRoot);
			newRoot.Nodes.Add(new QuadTreeNode(Range.From(
				new Position(startRow, startCol + halfCol),
				halfRow, halfCol), Depth, QuadTree));
			newRoot.Nodes.Add(new QuadTreeNode(Range.From(
				new Position(startRow + halfRow, startCol),
				halfRow, halfCol), Depth, QuadTree));
			newRoot.Nodes.Add(new QuadTreeNode(Range.From(
				new Position(startRow + halfRow, startCol + halfCol),
				halfRow, halfCol), Depth, QuadTree));
			return newRoot;
		}
		
		public bool IsProportionate(QuadTreeNode node)
		{
			if (node.Bounds.ColumnsCount * 2 < node.Bounds.RowsCount)
				return false;
			return true;
		}
		
		public void CreateSubNodes(QuadTreeNode parentNode)
		{
			// the smallest subnode has an area
			if ((parentNode.Bounds.ColumnsCount * parentNode.Bounds.RowsCount) <= 10)
				return;
			
			if (IsProportionate(parentNode) == false)
				CreateNotProportionate(parentNode);
			else
				CreateProportionate(parentNode);
		}
		
		private void CreateNotProportionate(QuadTreeNode parentNode)
		{
			var m_bounds = parentNode.Bounds;
			int startRow = m_bounds.Start.Row;
			int startCol = m_bounds.Start.Column;
			int halfCol = m_bounds.ColumnsCount;
			int halfRow = (m_bounds.RowsCount/ 2);

			var Depth = parentNode.Depth;
			var QuadTree = parentNode.QuadTree;
			
			parentNode.Nodes.Add(new QuadTreeNode(Range.From(m_bounds.Start, halfRow, halfCol), Depth, QuadTree));
			parentNode.Nodes.Add(new QuadTreeNode(Range.From(
				new Position(startRow + halfRow, startCol),
				halfRow, halfCol), Depth, QuadTree));
		}
		
		private void CreateProportionate(QuadTreeNode parentNode)
		{
			var m_bounds = parentNode.Bounds;
			int startRow = m_bounds.Start.Row;
			int startCol = m_bounds.Start.Column;
			int halfCol = (m_bounds.ColumnsCount / 2);
			int halfRow = (m_bounds.RowsCount/ 2);

			var Depth = parentNode.Depth;
			var QuadTree = parentNode.QuadTree;
			
			parentNode.Nodes.Add(new QuadTreeNode(Range.From(m_bounds.Start, halfRow, halfCol), Depth, QuadTree));
			parentNode.Nodes.Add(new QuadTreeNode(Range.From(
				new Position(startRow, startCol + halfCol),
				halfRow, halfCol), Depth, QuadTree));
			parentNode.Nodes.Add(new QuadTreeNode(Range.From(
				new Position(startRow + halfRow, startCol),
				halfRow, halfCol), Depth, QuadTree));
			parentNode.Nodes.Add(new QuadTreeNode(Range.From(
				new Position(startRow + halfRow, startCol + halfCol),
				halfRow, halfCol), Depth, QuadTree));
		}
	}
	
	
	/// <summary>
	/// Creates sub-nodes simply by dividing current node
	/// by 2 and creating 4 smaller regions.
	/// 
	/// This is not efficient if the area is very rectangular,
	/// for example, row count 10 000 and column count 10
	/// 
	/// Use ProportioanteSizeNodeDivider instead
	/// </summary>
	public class HalfSizeNodeDivider : IQuadTreeNodeDivider
	{
		public QuadTreeNode CreateNewRoot(QuadTreeNode currentRoot)
		{
			return new ProportioanteSizeNodeDivider().CreateNewRoot(currentRoot);
		}
		
		
		public void CreateSubNodes(QuadTreeNode parentNode)
		{
			var m_bounds = parentNode.Bounds;
			// the smallest subnode has an area
			if ((parentNode.Bounds.ColumnsCount * parentNode.Bounds.RowsCount) <= 10)
				return;

			int startRow = m_bounds.Start.Row;
			int startCol = m_bounds.Start.Column;
			int halfCol = (m_bounds.ColumnsCount / 2);
			int halfRow = (m_bounds.RowsCount/ 2);

			var Depth = parentNode.Depth;
			var QuadTree = parentNode.QuadTree;
			
			parentNode.Nodes.Add(new QuadTreeNode(Range.From(m_bounds.Start, halfRow, halfCol), Depth, QuadTree));
			parentNode.Nodes.Add(new QuadTreeNode(Range.From(
				new Position(startRow, startCol + halfCol),
				halfRow, halfCol), Depth, QuadTree));
			parentNode.Nodes.Add(new QuadTreeNode(Range.From(
				new Position(startRow + halfRow, startCol),
				halfRow, halfCol), Depth, QuadTree));
			parentNode.Nodes.Add(new QuadTreeNode(Range.From(
				new Position(startRow + halfRow, startCol + halfCol),
				halfRow, halfCol), Depth, QuadTree));
		}
	}
	
	
	/// <summary>
	/// The QuadTreeNode
	/// </summary>
	public class QuadTreeNode
	{
		public int Depth {get;set;}
		public QuadTree QuadTree{get;set;}
		public List<QuadTreeNode> Nodes {
			get { return m_nodes; }
		}
		
		public QuadTreeNode(Range bounds)
		{
			this.m_bounds = bounds;
		}
		
		/// <summary>
		/// Construct a quadtree node with the given bounds
		/// </summary>
		public QuadTreeNode(Range bounds, int currentDepth, QuadTree quadTree)
			:this(bounds)
		{
			m_bounds = bounds;
			QuadTree = quadTree;
			Depth = currentDepth + 1;
		}

		/// <summary>
		/// The area of this node
		/// </summary>
		Range m_bounds;

		/// <summary>
		/// The contents of this node.
		/// Note that the contents have no limit: this is not the standard way to impement a QuadTree
		/// </summary>
		List<Range> m_contents = new List<Range>();

		/// <summary>
		/// The child nodes of the QuadTree
		/// </summary>
		List<QuadTreeNode> m_nodes = new List<QuadTreeNode>(4);

		/// <summary>
		/// Is the node empty
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return m_nodes.Count == 0 && m_contents.Count == 0 ;
			}
		}

		/// <summary>
		/// Area of the quadtree node
		/// </summary>
		public Range Bounds { get { return m_bounds; } }

		public int MaxDepth
		{
			get
			{
				int currentDepth = Depth;
				foreach (QuadTreeNode node in m_nodes)
				{
					int depth = node.MaxDepth;
					if (depth > currentDepth)
						currentDepth = depth;
				}
				return currentDepth;
			}
		}
		
		/// <summary>
		/// Total number of nodes in the this node and all SubNodes
		/// </summary>
		public int Count
		{
			get
			{
				int count = 0;

				foreach (QuadTreeNode node in m_nodes)
					count += node.Count;

				count += this.Contents.Count;

				return count;
			}
		}

		/// <summary>
		/// Return the contents of this node and all subnodes in the true below this one.
		/// </summary>
		public List<Range> SubTreeContents
		{
			get
			{
				List<Range> results = new List<Range>();

				foreach (QuadTreeNode node in m_nodes)
					results.AddRange(node.SubTreeContents);

				results.AddRange(this.Contents);
				return results;
			}
		}

		public List<Range> Contents { get { return m_contents; } }

		
		public List<Range> Query(Range queryArea)
		{
			return QueryInternal(queryArea, false);
		}
		
		public Range? QueryFirst(Range queryArea)
		{
			var results = QueryInternal(queryArea, true);
			if (results.Count == 0)
				return null;
			return results[0];
		}
		
		/// <summary>
		/// Query the QuadTree for items that are in the given area
		/// </summary>
		/// <returns></returns>
		public List<Range> QueryInternal(Range queryArea, bool stopOnFirst)
		{
			// create a list of the items that are found
			List<Range> results = new List<Range>();

			// this quad contains items that are not entirely contained by
			// it's four sub-quads. Iterate through the items in this quad
			// to see if they intersect.
			foreach (var item in this.Contents)
			{
				if (queryArea.IntersectsWith(item))
				{
					results.Add(item);
					if (stopOnFirst == true)
						return results;
				}
			}

			foreach (QuadTreeNode node in m_nodes)
			{
				if (node.IsEmpty)
					continue;

				// Case 1: search area completely contained by sub-quad
				// if a node completely contains the query area, go down that branch
				// and skip the remaining nodes (break this loop)
				if (node.Bounds.Contains(queryArea))
				{
					results.AddRange(node.QueryInternal(queryArea, stopOnFirst));
					break;
				}

				// Case 2: Sub-quad completely contained by search area
				// if the query area completely contains a sub-quad,
				// just add all the contents of that quad and it's children
				// to the result set. You need to continue the loop to test
				// the other quads
				if (queryArea.Contains(node.Bounds))
				{
					results.AddRange(node.SubTreeContents);
					continue;
				}

				// Case 3: search area intersects with sub-quad
				// traverse into this quad, continue the loop to search other
				// quads
				if (node.Bounds.IntersectsWith(queryArea))
				{
					results.AddRange(node.QueryInternal(queryArea, stopOnFirst));
				}
			}


			return results;
		}
		
		public List<Range> Query(Position queryArea)
		{
			// create a list of the items that are found
			List<Range> results = new List<Range>();

			// this quad contains items that are not entirely contained by
			// it's four sub-quads. Iterate through the items in this quad
			// to see if they intersect.
			foreach (var item in this.Contents)
			{
				if (item.Contains(queryArea))
					results.Add(item);
			}

			foreach (QuadTreeNode node in m_nodes)
			{
				if (node.IsEmpty)
					continue;

				// Case 1: search area completely contained by sub-quad
				// if a node completely contains the query area, go down that branch
				// and skip the remaining nodes (break this loop)
				if (node.Bounds.Contains(queryArea))
				{
					results.AddRange(node.Query(queryArea));
					break;
				}
			}

			return results;
		}
		
		public Range? QueryFirst(Position queryArea)
		{
			// this quad contains items that are not entirely contained by
			// it's four sub-quads. Iterate through the items in this quad
			// to see if they intersect.
			foreach (var item in this.Contents)
			{
				if (item.Contains(queryArea))
					return item;
			}

			foreach (QuadTreeNode node in m_nodes)
			{
				if (node.IsEmpty)
					continue;

				// Case 1: search area completely contained by sub-quad
				// if a node completely contains the query area, go down that branch
				// and skip the remaining nodes (break this loop)
				if (node.Bounds.Contains(queryArea))
					return node.QueryFirst(queryArea);
			}
			return null;
		}

		public bool Remove(Range range)
		{
			// if the item is not contained in this quad, there's a problem
			if (!m_bounds.Contains(range))
			{
				throw new ArgumentException("range is out of the bounds of this quadtree node");
			}

			// for each subnode:
			// if the node contains the item, add the item to that node and return
			// this recurses into the node that is just large enough to fit this item
			foreach (QuadTreeNode node in m_nodes)
			{
				if (node.Bounds.Contains(range))
				{
					return node.Remove(range);
				}
			}

			for (int i = 0; i < Contents.Count; i++ )
			{
				if (Contents[i].Equals(range))
				{
					Contents.RemoveAt(i);
					return true;
				}
			}
			return false;
		}
		
		/// <summary>
		/// Insert an item to this node
		/// </summary>
		/// <param name="item"></param>
		public void Insert(Range item)
		{
			// if the item is not contained in this quad, there's a problem
			if (!m_bounds.Contains(item))
			{
				throw new ArgumentException("range is out of the bounds of this quadtree node");
			}

			// if the subnodes are null create them. may not be sucessfull: see below
			// we may be at the smallest allowed size in which case the subnodes will not be created
			if (m_nodes.Count == 0)
				QuadTree.QuadTreeNodeDivider.CreateSubNodes(this);

			// for each subnode:
			// if the node contains the item, add the item to that node and return
			// this recurses into the node that is just large enough to fit this item
			foreach (QuadTreeNode node in m_nodes)
			{
				if (node.Bounds.Contains(item))
				{
					node.Insert(item);
					return;
				}
			}

			// if we make it to here, either
			// 1) none of the subnodes completely contained the item. or
			// 2) we're at the smallest subnode size allowed
			// add the item to this node's contents.
			this.Contents.Add(item);
		}

		public void ForEach(QuadTree.QTAction action)
		{
			action(this);

			// draw the child quads
			foreach (QuadTreeNode node in this.m_nodes)
				node.ForEach(action);
		}
	}
}
