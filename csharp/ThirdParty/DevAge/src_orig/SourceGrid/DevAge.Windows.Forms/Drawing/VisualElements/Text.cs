using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DevAge.Drawing.VisualElements
{
    public interface IText : IVisualElement
    {
        string Value
        {
            get;
            set;
        }

        Color ForeColor
        {
            get;
            set;
        }

        Font Font
        {
            get;
            set;
        }
    }

    [Serializable]
    public class Text : VisualElementBase, IText
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Text()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public Text(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other"></param>
        public Text(Text other):base(other)
        {
            Value = other.Value;
            Font = other.Font;
            ForeColor = other.ForeColor;
            Enabled = other.Enabled;
        }
        #endregion

        #region Properties
        private Font mFont = System.Windows.Forms.Control.DefaultFont;
        /// <summary>
        /// Gets or sets the Font of the content. Default is System.Windows.Forms.Control.DefaultFont.
        /// </summary>
        public virtual Font Font
        {
            get { return mFont; }
            set { mFont = value; }
        }
        private bool ShouldSerializeFont()
        {
            return Font.ToString() != System.Windows.Forms.Control.DefaultFont.ToString();
        }

        private Color mForeColor = System.Windows.Forms.Control.DefaultForeColor;
        /// <summary>
        /// Gets or sets the fore color of the content. System.Windows.Forms.Control.DefaultForeColor
        /// </summary>
        public virtual Color ForeColor
        {
            get { return mForeColor; }
            set { mForeColor = value; }
        }
        private bool ShouldSerializeForeColor()
        {
            return ForeColor != System.Windows.Forms.Control.DefaultForeColor;
        }

        private string mValue = null;
        /// <summary>
        /// Gets or sets the string to draw. Default is null.
        /// </summary>
        [DefaultValue(null)]
        public virtual string Value
        {
            get { return mValue; }
            set { mValue = value; }
        }

        private bool mEnabled = true;
        public bool Enabled
        {
            get { return mEnabled; }
            set { mEnabled = value; }
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
            if (maxSize != System.Drawing.SizeF.Empty)
                return measure.Graphics.MeasureString(Value, Font, maxSize);
            else
                return measure.Graphics.MeasureString(Value, Font);
        }
        #endregion

        #region Draw
        protected override void OnDraw(GraphicsCache graphics, System.Drawing.RectangleF area)
        {
            SolidBrush brush;

            if (Enabled)
                brush = graphics.BrushsCache.GetBrush(ForeColor);
            else
                brush = graphics.BrushsCache.GetBrush(Color.FromKnownColor(KnownColor.GrayText));

            graphics.Graphics.DrawString(Value, Font, brush, area);
        }
        #endregion

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Text(this);
        }
    }
}
