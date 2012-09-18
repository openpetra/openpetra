using System;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Cells.Views
{
	/// <summary>
	/// Summary description for a 3D themed Header.
	/// </summary>
	[Serializable]
	public class RowHeader : Header
	{
		/// <summary>
		/// Represents a Column Header with the ability to draw an Image in the right to indicates the sort operation. You must use this model with a cell of type ICellSortableHeader.
		/// </summary>
		public new readonly static RowHeader Default;

		#region Constructors

		static RowHeader()
		{
			Default = new RowHeader();
		}

		/// <summary>
		/// Use default setting
		/// </summary>
		public RowHeader()
		{
            Background = new DevAge.Drawing.VisualElements.RowHeaderThemed();
		}

		/// <summary>
		/// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <param name="p_Source"></param>
		public RowHeader(RowHeader p_Source):base(p_Source)
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
			return new RowHeader(this);
		}
		#endregion

        #region Visual Elements

        public new DevAge.Drawing.VisualElements.IRowHeader Background
        {
            get { return (DevAge.Drawing.VisualElements.IRowHeader)base.Background; }
            set { base.Background = value; }
        }

        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);
        }
        #endregion

	}
}
