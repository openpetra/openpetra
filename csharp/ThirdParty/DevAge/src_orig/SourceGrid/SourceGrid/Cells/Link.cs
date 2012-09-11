using System;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Cells.Virtual
{
	/// <summary>
	/// A cell that contains a HTML style link. Use the click event to execute the link
	/// </summary>
	public abstract class Link : CellVirtual
	{
		/// <summary>
		/// Constructor, using VisualModels.Common.LinkStyle and BehaviorModels.Cursor.Default
		/// </summary>
		public Link()
		{
			View = Views.Link.Default;
			AddController(Controllers.MouseCursor.Hand);
		}
	}
}

namespace SourceGrid.Cells
{
	/// <summary>
	/// A cell that contains a HTML style link. Use the click event to execute the link
	/// </summary>
	public class Link : Cell
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Link():this(null)
		{
		}

		/// <summary>
		/// Constructor using VisualModels.Common.LinkStyle and BehaviorModels.Cursor.Default
		/// </summary>
		/// <param name="p_Value"></param>
		public Link(object p_Value):base(p_Value)
		{
			View = Views.Link.Default;
			AddController(Controllers.MouseCursor.Hand);
		}
	}
}
