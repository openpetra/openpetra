using SourceGrid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace QuadTreeLib
{
	/// <summary>
	/// A Quadtree is a structure designed to partition space so
	/// that it's faster to find out what is inside or outside a given
	/// area. See http://en.wikipedia.org/wiki/Quadtree
	/// This QuadTree contains items that have an area (RectangleF)
	/// it will store a reference to the item in the quad
	/// that is just big enough to hold it. Each quad has a bucket that
	/// contain multiple items.
	/// </summary>
	public class QuadTree
	{
		/// <summary>
		/// The root QuadTreeNode
		/// </summary>
		QuadTreeNode m_root;

		/// <summary>
		/// The bounds of this QuadTree
		/// </summary>
		Range m_rectangle;

		public IQuadTreeNodeDivider QuadTreeNodeDivider {get;set;}
		
		public Range Bounds {
			get { return m_rectangle; }
		}
		
		/// <summary>
		/// Returns the root node
		/// </summary>
		public QuadTreeNode Root {
			get { return m_root; }
		}
		
		/// <summary>
		/// Returns all the content 
		/// </summary>
		public List<Range> Contents {
			get 
			{ 
				return this.Query(this.Bounds);
			}
		}
		
		/// <summary>
		/// Double ocuppied space by adding one more root level node.
		/// Current root will go under the new root
		/// </summary>
		public void Grow()
		{
			this.m_root = this.QuadTreeNodeDivider.CreateNewRoot(m_root);
			this.m_rectangle = m_root.Bounds;
		}
		
		/// <summary>
		/// An delegate that performs an action on a QuadTreeNode
		/// </summary>
		/// <param name="obj"></param>
		public delegate void QTAction(QuadTreeNode obj);

		
		public QuadTree(int rows, int columns)
			:this(new Range(1, 1, rows, columns))
		{
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="rectangle"></param>
		public QuadTree(Range rectangle)
		{
			m_rectangle = rectangle;
			m_root = new QuadTreeNode(m_rectangle, 0, this);
			QuadTreeNodeDivider = new ProportioanteSizeNodeDivider();
		}

		/// <summary>
		/// Get the count of items in the QuadTree
		/// </summary>
		public int Count { get { return m_root.Count; } }

		
		/// <summary>
		/// Insert the feature into the QuadTree
		/// </summary>
		/// <param name="item"></param>
		public QuadTree Insert(Range item)
		{
			m_root.Insert(item);
			return this;
		}
		
		public QuadTree Remove(Range range)
		{
			m_root.Remove(range);
			return this;
		}
		
		/// <summary>
		/// returns
		/// </summary>
		/// <returns></returns>
		public int MaxDepth
		{
			get
			{
				return m_root.MaxDepth;
			}
		}
		
		/// <summary>
		/// Insert the feature into the QuadTree
		/// </summary>
		public QuadTree Insert(IEnumerable<Range> items)
		{
			foreach (var range in items)
			{
				m_root.Insert(range);
			}
			return this;
		}

		/// <summary>
		/// Query the QuadTree, returning the items that are in the given area
		/// </summary>
		/// <param name="area"></param>
		/// <returns></returns>
		public List<Range> Query(Range area)
		{
			return m_root.Query(area);
		}
		
		public List<Range> Query(Position area)
		{
			return m_root.Query(area);
		}
		
		/// <summary>
		/// Return first matching range
		/// </summary>
		/// <param name="area"></param>
		/// <returns></returns>
		public Range? QueryFirst(Position area)
		{
			return m_root.QueryFirst(area);
		}
		
		public Range? QueryFirst(Range area)
		{
			return m_root.QueryFirst(area);
		}
		
		/// <summary>
		/// Do the specified action for each item in the quadtree
		/// </summary>
		/// <param name="action"></param>
		public void ForEach(QTAction action)
		{
			m_root.ForEach(action);
		}

	}

}
