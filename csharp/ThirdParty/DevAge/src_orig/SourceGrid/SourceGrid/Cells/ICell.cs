using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Cells
{
	/// <summary>
	/// Represents a Cell to use with Grid control.
	/// </summary>
	public interface ICell : ICellVirtual
	{
		#region Value, DisplayText, ToolTipText, Image, Tag

		/// <summary>
		/// Gets the string representation of the Cell.Value property (default Value.ToString())
		/// </summary>
		string DisplayText
		{
			get;
		}

		/// <summary>
		/// Gets or sets the value of the cell 
		/// </summary>
		object Value
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets additional info for this cell
		/// </summary>
		object Tag
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the ToolTipText
		/// </summary>
		string ToolTipText
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or set the image of the cell.
		/// </summary>
		System.Drawing.Image Image
		{
			get;
			set;
		}
		#endregion

        #region LinkToGrid
        /// <summary>
        /// The Grid object
        /// </summary>
        Grid Grid
        {
            get;
        }

        /// <summary>
        /// Link the cell at the specified grid.
        /// For internal use only.
        /// </summary>
        /// <param name="p_grid"></param>
        /// <param name="p_Position"></param>
        void BindToGrid(Grid p_grid, Position p_Position);

        /// <summary>
        /// Remove the link of the cell from the grid.
        /// For internal use only.
        /// </summary>
        void UnBindToGrid();

        /// <summary>
        /// Gets the column of the specified cell
        /// </summary>
        GridColumn Column
        {
            get;
        }

        /// <summary>
        /// Gets the row of the specified cell
        /// </summary>
        GridRow Row
        {
            get;
        }

        /// <summary>
        /// Gets the range of the cell
        /// </summary>
        Range Range
        {
            get;
        }
        #endregion

		#region Row/Col Span
		/// <summary>
		/// ColSpan for merge operation, calculated using the current range.
		/// </summary>
		int ColumnSpan
		{
			get;
			set;
		}
		/// <summary>
		/// RowSpan for merge operation, calculated using the current range.
		/// </summary>
		int RowSpan
		{
			get;
			set;
		}
		
		/// <summary>
		/// Setting a col/row spann is a costly operation, so it's better
		/// if you set these two at the same time.
		/// Prefer this method to <c>RowSpan</c> and <c>ColSpan</c> property setters
		/// </summary>
		/// <param name="rowSpan"></param>
		/// <param name="colSpan"></param>
		void SetSpan(int rowSpan, int colSpan);
		#endregion
	}
}
