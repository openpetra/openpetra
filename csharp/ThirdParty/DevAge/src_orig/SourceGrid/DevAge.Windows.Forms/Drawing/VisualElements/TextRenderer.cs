using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevAge.Drawing.VisualElements
{
    [Serializable]
    public class TextRenderer : Text
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public TextRenderer()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public TextRenderer(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public TextRenderer(TextRenderer other)
            : base(other)
        {
            TextFormatFlags = other.TextFormatFlags;
        }
        #endregion
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new TextRenderer(this);
        }

        #region Properties
        private System.Windows.Forms.TextFormatFlags mTextFormatFlags = System.Windows.Forms.TextFormatFlags.Default | System.Windows.Forms.TextFormatFlags.NoPrefix;
        /// <summary>
        /// Gets or sets the TextFormatFlags enum. 
        /// </summary>
        public virtual System.Windows.Forms.TextFormatFlags TextFormatFlags
        {
            get { return mTextFormatFlags; }
            set { mTextFormatFlags = value; }
        }
        protected virtual bool ShouldSerializeTextFormatFlags()
        {
            return TextFormatFlags != (System.Windows.Forms.TextFormatFlags.Default | System.Windows.Forms.TextFormatFlags.NoPrefix);
        }

        #endregion

        protected override void OnDraw(GraphicsCache graphics, RectangleF area)
        {
            // base.OnDraw(graphics, area); //Don't call the base method because this class draw the class in a different way

            if (Value == null || Value.Length == 0)
                return;

            if (Enabled)
                System.Windows.Forms.TextRenderer.DrawText(graphics.Graphics, Value, Font, Rectangle.Round(area), ForeColor, TextFormatFlags);
            else
                System.Windows.Forms.TextRenderer.DrawText(graphics.Graphics, Value, Font, Rectangle.Round(area), Color.FromKnownColor(KnownColor.GrayText), TextFormatFlags);
        }

        /// <summary>
        /// Measure the current content of the VisualElement.
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="maxSize">If empty is not used.</param>
        /// <returns></returns>
        protected override SizeF OnMeasureContent(MeasureHelper measure, System.Drawing.SizeF maxSize)
        {
            Size proposedSize;

            if (maxSize != System.Drawing.SizeF.Empty)
            {
                proposedSize = Size.Ceiling(maxSize);

                //Remove the 0 because cause some auto size problems expecially when used with the EndEllipses flag.
                if (proposedSize.Width == 0)
                    proposedSize.Width = int.MaxValue;
                if (proposedSize.Height == 0)
                    proposedSize.Height = int.MaxValue;
            }
            else
                // Declare a proposed size with dimensions set to the maximum integer value.
                proposedSize = new Size(int.MaxValue, int.MaxValue);

            return System.Windows.Forms.TextRenderer.MeasureText(measure.Graphics, Value, Font, proposedSize, TextFormatFlags);
        }
    }
}
