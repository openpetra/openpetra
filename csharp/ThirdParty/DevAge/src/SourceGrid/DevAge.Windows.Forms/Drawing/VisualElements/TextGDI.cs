using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevAge.Drawing.VisualElements
{
    [Serializable]
    public class TextGDI : Text
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TextGDI()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public TextGDI(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public TextGDI(TextGDI other)
            : base(other)
        {
            if (other.StringFormat != null)
                StringFormat = (StringFormat)other.StringFormat.Clone();
            else
                StringFormat = null;
        }
        #endregion

        #region Properties
        private StringFormat mStringFormat = new StringFormat(StringFormat.GenericDefault);
        /// <summary>
        /// Gets or sets the StringFormat object. 
        /// Encapsulates text layout information (such as alignment, orientation and tab stops) display manipulations (such as ellipsis insertion and national digit substitution) and OpenType features.
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual StringFormat StringFormat
        {
            get { return mStringFormat; }
            set { mStringFormat = value; }
        }

        /// <summary>
        /// Gets or sets the alignment of the content.
        /// </summary>
        public virtual ContentAlignment Alignment
        {
            get { return Utilities.StringFormatToContentAlignment(StringFormat); }
            set { Utilities.ApplyContentAlignmentToStringFormat(value, StringFormat); }
        }
        protected virtual bool ShouldSerializeAlignment()
        {
            return false; //Always false because this property simply change the StringFormat
        }

        #endregion

        protected override void OnDraw(GraphicsCache graphics, RectangleF area)
        {
            //base.OnDrawContent(graphics, area); //Don't call the base method because this class draw the class in a different way

            if (Value == null || Value.Length == 0)
                return;

            SolidBrush brush;

            if (Enabled)
                brush = graphics.BrushsCache.GetBrush(ForeColor);
            else
                brush = graphics.BrushsCache.GetBrush(Color.FromKnownColor(KnownColor.GrayText));

            graphics.Graphics.DrawString(Value, Font, brush, area, StringFormat);
        }

        /// <summary>
        /// Measure the current content of the VisualElement.
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="maxSize">If empty is not used.</param>
        /// <returns></returns>
        protected override SizeF OnMeasureContent(MeasureHelper measure, System.Drawing.SizeF maxSize)
        {
            if (maxSize != System.Drawing.SizeF.Empty)
                return measure.Graphics.MeasureString(Value, Font, maxSize, StringFormat);
            else
                return measure.Graphics.MeasureString(Value, Font, new SizeF(5000, 5000), StringFormat);
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new TextGDI(this);
        }
    }
}
