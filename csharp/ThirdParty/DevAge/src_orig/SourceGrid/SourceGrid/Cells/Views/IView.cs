using System;
using System.Drawing;

namespace SourceGrid.Cells.Views
{
	/// <summary>
	/// A interface that represents the visual aspect of a cell. Contains the Draw method and the common properties
	/// Support a deep cloning.
	/// </summary>
	public interface IView : ICloneable
	{
		#region Format
		Font Font
		{
			get;
			set;
		}
		/// <summary>
		/// Get the font of the cell, check if the current font is null and in this case return the grid font
		/// </summary>
		/// <param name="grid"></param>
		/// <returns></returns>
		Font GetDrawingFont(GridVirtual grid);

		/// <summary>
		/// Word Wrap.
		/// </summary>
		bool WordWrap
		{
			get;
			set;
		}

		/// <summary>
		/// Text Alignment.
		/// </summary>
		DevAge.Drawing.ContentAlignment TextAlignment
		{
			get;
			set;
		}


		/// <summary>
        /// The normal border of a cell. Usually it is an instance of a DevAge.Drawing.RectangleBorder structure
		/// </summary>
		DevAge.Drawing.IBorder Border
		{
			get;
			set;
		}

		/// <summary>
		/// The BackColor of a cell
		/// </summary>
		Color BackColor
		{
			get;
			set;
		}
		/// <summary>
		/// The ForeColor of a cell
		/// </summary>
		Color ForeColor
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// Draw the cell specified
		/// </summary>
		/// <param name="cellContext"></param>
        /// <param name="graphics">Paint arguments</param>
        /// <param name="rectangle">Rectangle where draw the current cell</param>
		void DrawCell(CellContext cellContext,
			DevAge.Drawing.GraphicsCache graphics, 
			System.Drawing.RectangleF rectangle);


		/// <summary>
		/// Returns the minimum required size of the current cell, calculating using the current DisplayString, Image and Borders informations.
		/// </summary>
		/// <param name="cellContext"></param>
		/// <param name="maxLayoutArea">SizeF structure that specifies the maximum layout area for the text. If width or height are zero the value is set to a default maximum value.</param>
		/// <returns></returns>
		Size Measure(CellContext cellContext,
									Size maxLayoutArea);
	}
}
