using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SourceGrid.Cells.Views
{
	/// <summary>
	/// Summary description for VisualModelCheckBox.
	/// </summary>
	[Serializable]
	public class MultiImages : Cell
	{
		#region Constructors

		/// <summary>
		/// Use default setting
		/// </summary>
		public MultiImages()
		{
            ElementsDrawMode = DevAge.Drawing.ElementsDrawMode.Covering;
		}

		/// <summary>
		/// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
        /// <param name="other"></param>
        public MultiImages(MultiImages other)
            : base(other)
		{
            mImages = (DevAge.Drawing.VisualElements.VisualElementList)other.mImages.Clone();
		}
		#endregion

        private DevAge.Drawing.VisualElements.VisualElementList mImages = new DevAge.Drawing.VisualElements.VisualElementList();
		/// <summary>
		/// Images of the cells
		/// </summary>
        public DevAge.Drawing.VisualElements.VisualElementList SubImages
		{
            get { return mImages; }
		}

        protected override IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetElements()
        {
            foreach (DevAge.Drawing.VisualElements.IVisualElement v in GetBaseElements())
                yield return v;

            foreach (DevAge.Drawing.VisualElements.IVisualElement v in SubImages)
                yield return v;
        }
        private IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetBaseElements()
        {
            return base.GetElements();
        }

		#region Clone
		/// <summary>
		/// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			return new MultiImages(this);
		}
		#endregion
	}
}
