using System;
using System.Drawing;
using System.Windows.Forms;
using DevAge.Drawing;

namespace SourceGrid.Cells.Views
{
	/// <summary>
	/// Summary description for a 3D Header.
	/// </summary>
	[Serializable]
	public class Header : Cell
	{
        public new static RectangleBorder DefaultBorder = RectangleBorder.NoBorder;

		/// <summary>
		/// Represents a default Header, with a 3D border and a LightGray BackColor
		/// </summary>
		public new readonly static Header Default;

		#region Constructors

		static Header()
		{
			Default = new Header();
		}

		/// <summary>
		/// Use default setting
		/// </summary>
		public Header()
		{
            Background = new DevAge.Drawing.VisualElements.HeaderThemed();
            Border = Header.DefaultBorder;
		}

		/// <summary>
		/// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <param name="p_Source"></param>
		public Header(Header p_Source):base(p_Source)
		{
            Background = (DevAge.Drawing.VisualElements.IHeader)p_Source.Background.Clone();
        }
		#endregion

		#region Clone
		/// <summary>
		/// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			return new Header(this);
		}
		#endregion

        #region Visual Elements

        public new DevAge.Drawing.VisualElements.IHeader Background
        {
            get { return (DevAge.Drawing.VisualElements.IHeader)base.Background; }
            set { base.Background = value; }
        }

        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);

            if (context.CellRange.Contains(context.Grid.MouseDownPosition))
                Background.Style = DevAge.Drawing.ControlDrawStyle.Pressed;
            else if (context.CellRange.Contains(context.Grid.MouseCellPosition))
                Background.Style = DevAge.Drawing.ControlDrawStyle.Hot;
            else
                Background.Style = DevAge.Drawing.ControlDrawStyle.Normal;
        }
        #endregion
	}
}
