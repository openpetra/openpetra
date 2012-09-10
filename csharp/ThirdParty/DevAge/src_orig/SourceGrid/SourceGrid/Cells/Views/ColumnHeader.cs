using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Cells.Views
{
	/// <summary>
	/// Summary description for a 3D Header.
    /// This is a standard header without theme support. Use the ColumnHeaderThemed for theme support.
	/// </summary>
	[Serializable]
	public class ColumnHeader : Header
	{
		/// <summary>
		/// Represents a Column Header with the ability to draw an Image in the right to indicates the sort operation. You must use this model with a cell of type ICellSortableHeader.
		/// </summary>
		public new readonly static ColumnHeader Default;

		#region Constructors

		static ColumnHeader()
		{
			Default = new ColumnHeader();
		}

		/// <summary>
		/// Use default setting
		/// </summary>
		public ColumnHeader()
		{
            Background = new DevAge.Drawing.VisualElements.ColumnHeaderThemed();
		}

		/// <summary>
		/// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <param name="p_Source"></param>
		public ColumnHeader(ColumnHeader p_Source):base(p_Source)
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
			return new ColumnHeader(this);
		}
		#endregion

        #region Visual Elements

        public new DevAge.Drawing.VisualElements.IColumnHeader Background
        {
            get { return (DevAge.Drawing.VisualElements.IColumnHeader)base.Background; }
            set { base.Background = value; }
        }

        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);

            PrepareVisualElementSortIndicator(context);
        }

        protected override IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetElements()
        {
            if (ElementSort != null)
                yield return ElementSort;

            foreach (DevAge.Drawing.VisualElements.IVisualElement v in GetBaseElements())
                yield return v;
        }
        private IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetBaseElements()
        {
            return base.GetElements();
        }

        private DevAge.Drawing.VisualElements.ISortIndicator mElementSort = new DevAge.Drawing.VisualElements.SortIndicator();
        /// <summary>
        /// Gets or sets the visual element used to draw the sort indicator. Default is DevAge.Drawing.VisualElements.SortIndicator
        /// </summary>
        public DevAge.Drawing.VisualElements.ISortIndicator ElementSort
        {
            get { return mElementSort; }
            set { mElementSort = value; }
        }

        protected virtual void PrepareVisualElementSortIndicator(CellContext context)
        {
            Models.ISortableHeader sortModel = (Models.ISortableHeader)context.Cell.Model.FindModel(typeof(Models.ISortableHeader));
            if (sortModel != null)
            {
                Models.SortStatus status = sortModel.GetSortStatus(context);
                ElementSort.SortStyle = status.Style;
            }
            else
                ElementSort.SortStyle = DevAge.Drawing.HeaderSortStyle.None;

        }
        #endregion

	}
}
