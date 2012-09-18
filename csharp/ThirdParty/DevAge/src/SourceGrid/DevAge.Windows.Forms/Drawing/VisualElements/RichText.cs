using System;
using System.ComponentModel;
using System.Drawing;

namespace DevAge.Drawing.VisualElements
{
    /// <summary>
    /// Default interface for RichText implementations
    /// </summary>
    public interface IRichText : IVisualElement
    {
        /// <summary>
        /// Text of format RichText
        /// </summary>
        DevAge.Windows.Forms.RichText Value
        {
            get;
            set;
        }

        /// <summary>
        /// ForeColor of text
        /// </summary>
        Color ForeColor
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
        /// Text Font
        /// </summary>
        Font Font
        {
            get;
            set;
        }

        /// <summary>
        /// Rotate flip type
        /// </summary>
        RotateFlipType RotateFlipType
        {
            get;
            set;
        }
    }

    [Serializable]
    public class RichText : VisualElementBase, IRichText
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RichText()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public RichText(DevAge.Windows.Forms.RichText value)
        {
            Value = value;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other"></param>
        public RichText(RichText other)
            : base(other)
        {
            Value = other.Value;
            ForeColor = other.ForeColor;
            TextAlignment = other.TextAlignment;
            Font = other.Font;
        }

        #endregion

        #region Properties

        private DevAge.Windows.Forms.RichText m_Value = null;
        /// <summary>
        /// Gets or sets the string of format rich text to draw. Default is null.
        /// </summary>
        [DefaultValue(null)]
        public virtual DevAge.Windows.Forms.RichText Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }

        private Color m_ForeColor = System.Windows.Forms.Control.DefaultForeColor;
        /// <summary>
        /// Gets or sets the fore color of the content. System.Windows.Forms.Control.DefaultForeColor
        /// </summary>
        public virtual Color ForeColor
        {
            get { return m_ForeColor; }
            set { m_ForeColor = value; }
        }

        /// <summary>
        /// Text Alignment
        /// </summary>
        private DevAge.Drawing.ContentAlignment m_TextAlignment;
        public DevAge.Drawing.ContentAlignment TextAlignment
        {
            get { return m_TextAlignment; }
            set { m_TextAlignment = value; }
        }

        /// <summary>
        /// Text Font
        /// </summary>
        private Font m_Font = System.Windows.Forms.Control.DefaultFont;
        public virtual Font Font
        {
            get { return m_Font; }
            set { m_Font = value; }
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

        #region Measure

        /// <summary>
        /// Measure the current content of the VisualElement.
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="maxSize">If empty is not used.</param>
        /// <returns></returns>
        protected override System.Drawing.SizeF OnMeasureContent(MeasureHelper measure, System.Drawing.SizeF maxSize)
        {
            Font font = System.Windows.Forms.Control.DefaultFont;

            if (maxSize != System.Drawing.SizeF.Empty)
                return measure.Graphics.MeasureString(Value.Rtf, font, maxSize);
            else
                return measure.Graphics.MeasureString(Value.Rtf, font);
        }

        #endregion

        #region Draw

        protected override void OnDraw(GraphicsCache graphics, System.Drawing.RectangleF area)
        {
            SolidBrush brush;
            Color color = System.Windows.Forms.Control.DefaultForeColor;
            Font font = System.Windows.Forms.Control.DefaultFont;

            brush = graphics.BrushsCache.GetBrush(color);
            graphics.Graphics.DrawString(Value.Rtf, font, brush, area);
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new RichText(this);
        }

        #endregion
    }
}
