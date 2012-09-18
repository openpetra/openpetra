using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SourceGrid.Cells.Views
{
	/// <summary>
	/// Class to manage the visual aspect of a cell. This class can be shared beetween multiple cells.
	/// </summary>
	[Serializable]
	public class Cell : ViewBase
	{
		/// <summary>
		/// Represents a default Model
		/// </summary>
		public readonly static Cell Default = new Cell();

		#region Constructors

		/// <summary>
		/// Use default setting and construct a read and write VisualProperties
		/// </summary>
		public Cell()
		{
            ElementsDrawMode = DevAge.Drawing.ElementsDrawMode.Align;
		}

		/// <summary>
		/// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <param name="p_Source"></param>
		public Cell(Cell p_Source):base(p_Source)
		{
            ElementImage = (DevAge.Drawing.VisualElements.IImage)p_Source.ElementImage.Clone();
            ElementText = (DevAge.Drawing.VisualElements.IText)p_Source.ElementText.Clone();
		}
		#endregion

		#region Clone
		/// <summary>
		/// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			return new Cell(this);
		}
		#endregion

        #region Visual elements

        protected override IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetElements()
        {
            if (ElementImage != null)
                yield return ElementImage;

            if (ElementText != null)
                yield return ElementText;
        }

        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);

            PrepareVisualElementText(context);

            PrepareVisualElementImage(context);
        }

        private DevAge.Drawing.VisualElements.IText mElementText = new DevAge.Drawing.VisualElements.TextGDI();
        /// <summary>
        /// Gets or sets the IText visual element used to draw the cell text.
        /// Default is DevAge.Drawing.VisualElements.TextGDI
        /// </summary>
        public DevAge.Drawing.VisualElements.IText ElementText
        {
            get { return mElementText; }
            set { mElementText = value; }
        }

        /// <summary>
        /// Apply to the VisualElement specified the Image properties of the current View.
        /// Derived class can call this method to apply the settings to custom VisualElement.
        /// </summary>
        protected virtual void PrepareVisualElementText(CellContext context)
        {
            if (ElementText is DevAge.Drawing.VisualElements.TextRenderer)
            {
                DevAge.Drawing.VisualElements.TextRenderer elementText = (DevAge.Drawing.VisualElements.TextRenderer)ElementText;

                elementText.TextFormatFlags = TextFormatFlags.Default | TextFormatFlags.NoPrefix;
                if (WordWrap)
                    elementText.TextFormatFlags |= TextFormatFlags.WordBreak;
                if (TrimmingMode == TrimmingMode.Char)
                    elementText.TextFormatFlags |= TextFormatFlags.EndEllipsis;
                else if (TrimmingMode == TrimmingMode.Word)
                    elementText.TextFormatFlags |= TextFormatFlags.WordEllipsis;
                elementText.TextFormatFlags |= DevAge.Windows.Forms.Utilities.ContentAligmentToTextFormatFlags(TextAlignment);
            }
            else if (ElementText is DevAge.Drawing.VisualElements.TextGDI)
            {
                DevAge.Drawing.VisualElements.TextGDI elementTextGDI = (DevAge.Drawing.VisualElements.TextGDI)ElementText;

                if (WordWrap)
                    elementTextGDI.StringFormat.FormatFlags = (StringFormatFlags)0;
                else
                    elementTextGDI.StringFormat.FormatFlags = StringFormatFlags.NoWrap;
                if (TrimmingMode == TrimmingMode.Char)
                    elementTextGDI.StringFormat.Trimming = StringTrimming.EllipsisCharacter;
                else if (TrimmingMode == TrimmingMode.Word)
                    elementTextGDI.StringFormat.Trimming = StringTrimming.EllipsisWord;
                else
                    elementTextGDI.StringFormat.Trimming = StringTrimming.None;
                elementTextGDI.Alignment = TextAlignment;
            }

            ElementText.Font = GetDrawingFont(context.Grid);
            ElementText.ForeColor = ForeColor;
            //I have already set the TextFormatFlags for the alignment so the Anchor is not necessary. I have removed this code for performance reasons.
            //element.AnchorArea = new DevAge.Drawing.AnchorArea(TextAlignment, false);

            ElementText.Value = context.DisplayText;
        }

        private DevAge.Drawing.VisualElements.IImage mElementImage = new DevAge.Drawing.VisualElements.Image();
        /// <summary>
        /// Gets or sets the IImage visual element used to draw the cell image.
        /// Default is DevAge.Drawing.VisualElements.Image
        /// </summary>
        public DevAge.Drawing.VisualElements.IImage ElementImage
        {
            get { return mElementImage; }
            set { mElementImage = value; }
        }

        /// <summary>
        /// Apply to the VisualElement specified the Image properties of the current View.
        /// Derived class can call this method to apply the settings to custom VisualElement.
        /// </summary>
        protected virtual void PrepareVisualElementImage(CellContext context)
        {
            ElementImage.AnchorArea = new DevAge.Drawing.AnchorArea(ImageAlignment, ImageStretch);

            //Read the image
            System.Drawing.Image img = null;
            Models.IImage imgModel = (Models.IImage)context.Cell.Model.FindModel(typeof(Models.IImage));
            if (imgModel != null)
                img = imgModel.GetImage(context);
            ElementImage.Value = img;
            //ElementImage.AnchorArea = AnchorArea.HasBottom
        }
        #endregion  
	}


}
