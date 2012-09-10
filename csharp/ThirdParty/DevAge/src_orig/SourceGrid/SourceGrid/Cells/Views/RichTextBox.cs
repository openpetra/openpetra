using System;
using System.Collections.Generic;
using System.Drawing;

namespace SourceGrid.Cells.Views
{
    [Serializable]
    public class RichTextBox : Cell
    {
        /// <summary>
        /// Represents a rich text box
        /// </summary>
        public new readonly static RichTextBox Default = new RichTextBox();

        #region Constructors

        /// <summary>
        /// Use default setting and construct a read and write VisualProperties
        /// </summary>
        public RichTextBox()
        {
            ElementRichText = new DevAge.Drawing.VisualElements.RichTextGDI();
        }

        /// <summary>
        /// Copy constructor. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
        /// </summary>
        /// <param name="p_Source"></param>
        public RichTextBox(RichTextBox p_Source)
            : base(p_Source)
        {
            ElementRichText = (DevAge.Drawing.VisualElements.IRichText)p_Source.ElementRichText.Clone();
        }

        #endregion

        #region Preparing

        protected override void PrepareView(CellContext context)
        {
            // Do not start base prepare view, as it will render the text as normal TextGDI.
            // base.PrepareView(context);

            PrepareVisualElementRichTextBox(context);
        }

        protected override IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetElements()
        {
            if (ElementRichText != null)
                yield return ElementRichText;

            foreach (DevAge.Drawing.VisualElements.IVisualElement v in GetBaseElements())
                yield return v;
        }
        private IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetBaseElements()
        {
            return base.GetElements();
        }

        protected virtual void PrepareVisualElementRichTextBox(CellContext context)
        {
            ElementRichText.Value = context.Cell.Model.ValueModel.GetValue(context) as DevAge.Windows.Forms.RichText;
            ElementRichText.ForeColor = ForeColor;
            ElementRichText.TextAlignment = TextAlignment;
            ElementRichText.Font = GetDrawingFont(context.Grid);
            ElementRichText.RotateFlipType = RotateFlipType;
        }

        #endregion

        #region Properties

        private DevAge.Drawing.VisualElements.IRichText m_ElementRichText = null;
        /// <summary>
        /// Gets or sets the IText visual element used to draw the cell rich text.
        /// </summary>
        public DevAge.Drawing.VisualElements.IRichText ElementRichText
        {
            get { return m_ElementRichText; }
            set { m_ElementRichText = value; }
        }

        /// <summary>
        /// Rotate flip type
        /// </summary>
        private RotateFlipType m_RotateFlipType = RotateFlipType.RotateNoneFlipNone;
        public RotateFlipType RotateFlipType
        {
            get { return m_RotateFlipType; }
            set { m_RotateFlipType = value; }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new RichTextBox(this);
        }

        #endregion
    }
}
