using System;
using System.ComponentModel;

namespace SourceGrid
{
	/// <summary>
	/// Column Information
	/// </summary>
	public class ColumnInfo
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_Grid"></param>
		public ColumnInfo(GridVirtual p_Grid)
		{
			m_Grid = p_Grid;
			m_width = Grid.DefaultWidth;
		}
		private int m_maximalWidth = -1;

		/// <summary>
		/// Maximal width of the current Column, -1 if unlimited
		/// </summary>
		public int MaximalWidth
		{
			get{return m_maximalWidth;}
			set
			{
				// Maximal width can not be smaller than minimal width
				if ( value < m_minimalWidth )
					value = m_minimalWidth;
				if ( value != m_maximalWidth && value >= -1 )
				{
					m_maximalWidth = value;
					if ( this.Width > m_maximalWidth && m_maximalWidth > -1 )
						this.Width = m_maximalWidth;
				}
			}
		}

		private int m_minimalWidth = 0;

		/// <summary>
		/// Minimal width of the current Column
		/// </summary>
		public int MinimalWidth
		{
			get { return m_minimalWidth; }
			set
			{
				if ( value < 0 )
					value = 0;
				// Minimal width can not be bigger than maximal width
				if ( value > m_maximalWidth && m_maximalWidth > -1 )
					value = m_maximalWidth;
				if ( value != m_minimalWidth )
				{
					m_minimalWidth = value;
					if ( this.Width < m_minimalWidth && this.Visible )
						this.Width = m_minimalWidth;
				}
			}
		}

		private int m_width;
		/// <summary>
		/// Width of the current Column
		/// </summary>
		public int Width
		{
			get{return m_width;}
			set
			{
				if ( value < m_minimalWidth )
					value = m_minimalWidth;
				if ( value > m_maximalWidth && m_maximalWidth > -1 )
					value = m_maximalWidth;
	
				if (m_width != value)
				{
					m_width = value;
					if ( Visible )
						((ColumnInfoCollection)Grid.Columns).OnColumnWidthChanged(new ColumnInfoEventArgs(this));
				}
			}
		}
	
		/// <summary>
		/// Index of the current Column
		/// </summary>
		public int Index
		{
			get
			{
				return ((ColumnInfoCollection)Grid.Columns).IndexOf(this);
			}
		}
	
		private GridVirtual m_Grid;
		/// <summary>
		/// Attached Grid
		/// </summary>
		[Browsable(false)]
		public GridVirtual Grid
		{
			get{return m_Grid;}
		}
	
	
		public Range Range
		{
			get
			{
				if (m_Grid == null)
					throw new SourceGridException("Invalid Grid object");
	
				return new Range(0, Index, Grid.Rows.Count - 1, Index);
			}
		}
	
		private object m_Tag;
		/// <summary>
		/// A property that the user can use to insert custom informations associated to a specific column
		/// </summary>
		[Browsable(false)]
		public object Tag
		{
			get{return m_Tag;}
			set{m_Tag = value;}
		}
	
		private AutoSizeMode m_AutoSizeMode = AutoSizeMode.Default;
		/// <summary>
		/// Flags for autosize and stretch
		/// </summary>
		public AutoSizeMode AutoSizeMode
		{
			get{return m_AutoSizeMode;}
			set{m_AutoSizeMode = value;}
		}

		private bool m_visible = true;

		/// <summary>
		/// Gets or sets if the column is visible.
		/// Internally set the width to 0 to hide a column.
		/// </summary>
		public bool Visible
		{
			get { return m_visible; }
			set
			{
				if ( value != m_visible )
				{
					m_visible = value;
					((ColumnInfoCollection)Grid.Columns).OnColumnWidthChanged(new ColumnInfoEventArgs(this));
				}
			}
		}
	}
}
