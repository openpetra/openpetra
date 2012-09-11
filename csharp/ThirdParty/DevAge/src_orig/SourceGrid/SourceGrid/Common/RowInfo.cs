using System;
using System.ComponentModel;

namespace SourceGrid
{
	/// <summary>
	/// Row Information
	/// </summary>
	public class RowInfo
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_Grid"></param>
		public RowInfo(GridVirtual p_Grid)
		{
			m_Grid = p_Grid;
			m_Height = Grid.DefaultHeight;
		}
	
		private int m_Height;
		/// <summary>
		/// Height of the current row
		/// </summary>
		public int Height
		{
			get{return m_Height;}
			set
			{
				if (value < 0)
					value = 0;
	
				if (m_Height != value)
				{
					m_Height = value;
					((RowInfoCollection)m_Grid.Rows).OnRowHeightChanged(new RowInfoEventArgs(this));
				}
			}
		}
	
		//private int m_Index;
		/// <summary>
		/// Index of the current row
		/// </summary>
		public int Index
		{
			get{return ((RowInfoCollection)Grid.Rows).IndexOf(this);}
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
	
				return new Range(Index, 0, Index, Grid.Columns.Count - 1);
			}
		}
		private object m_Tag;
		/// <summary>
		/// A property that the user can use to insert custom informations associated to a specific row
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
	
		/// <summary>
		/// Gets or sets if the row is visible.
		/// Internally set the height to 0 to hide a row.
		/// </summary>
		public bool Visible
		{
			get 
			{ 
				return Grid.Rows.IsRowVisible(this.Index);
			}
			set
			{
				Grid.Rows.ShowRow(this.Index, value);
			}
		}
	}
}
