using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace SourceGrid.Cells.Views
{
	/// <summary>
	/// Class to manage the visual aspect of a cell. This class can be shared beetween multiple cells.
	/// </summary>
	[Serializable]
	public class Link : Cell
	{
		/// <summary>
		/// Represents a model with a link style font and forecolor.
		/// </summary>
		public new readonly static Link Default;

		static Link()
		{
			Default = new Link();
		}

		#region Constructors
		/// <summary>
		/// Use default setting and construct a read and write VisualProperties
		/// </summary>
		public Link()
		{
			Font = new Font(Control.DefaultFont, FontStyle.Underline);
			ForeColor = Color.Blue;
		}
		/// <summary>
		/// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <param name="p_Source"></param>
		public Link(Link p_Source):base(p_Source)
		{
		}
		#endregion

		#region Clone
		/// <summary>
		/// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			return new Link(this);
		}
		#endregion
	}
}
