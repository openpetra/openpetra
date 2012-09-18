using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DevAge.Drawing.VisualElements
{
    [Serializable]
    public class DropDownButtonThemed : DropDownButtonBase
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public DropDownButtonThemed()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public DropDownButtonThemed(DropDownButtonThemed other)
            : base(other)
        {
        }
        #endregion
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new DropDownButtonThemed(this);
        }

        #region Properties
        /// <summary>
        /// Standard button used when the XP style are disabled.
        /// </summary>
        private DropDownButton mStandardButton = new DropDownButton();

        public override ButtonStyle Style
        {
            get { return base.Style; }
            set { base.Style = value; mStandardButton.Style = value; }
        }
        #endregion

        #region Helper methods
        protected VisualStyleElement GetBackgroundElement()
        {
            if (Style == ButtonStyle.Hot)
                return VisualStyleElement.ComboBox.DropDownButton.Hot;
            else if (Style == ButtonStyle.Pressed)
                return VisualStyleElement.ComboBox.DropDownButton.Pressed;
            else if (Style == ButtonStyle.Disabled)
                return VisualStyleElement.ComboBox.DropDownButton.Disabled;
            else
                return VisualStyleElement.ComboBox.DropDownButton.Normal;
        }

        /// <summary>
        /// Gets the System.Windows.Forms.VisualStyles.VisualStyleRenderer to draw the specified element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected System.Windows.Forms.VisualStyles.VisualStyleRenderer GetRenderer(VisualStyleElement element)
        {
            return new System.Windows.Forms.VisualStyles.VisualStyleRenderer(element);
        }
        #endregion

        protected override void OnDraw(GraphicsCache graphics, RectangleF area)
        {
            if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(GetBackgroundElement()))
            {
                GetRenderer(GetBackgroundElement()).DrawBackground(graphics.Graphics, Rectangle.Round(area));

                if (Style == ButtonStyle.Focus)
                {
                    using (MeasureHelper measure = new MeasureHelper(graphics))
                    {
                        ControlPaint.DrawFocusRectangle(graphics.Graphics, Rectangle.Round(area));
                    }
                }
            }
            else
                mStandardButton.Draw(graphics, area);
        }

        protected override SizeF OnMeasureContent(MeasureHelper measure, SizeF maxSize)
        {
            if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(GetBackgroundElement()))
                return GetRenderer(GetBackgroundElement()).GetPartSize(measure.Graphics, ThemeSizeType.True);
            else
                return mStandardButton.Measure(measure, Size.Empty, maxSize);
        }

        //public override RectangleF GetBackgroundContentRectangle(MeasureHelper measure, RectangleF backGroundArea)
        //{
        //    backGroundArea = base.GetBackgroundContentRectangle(measure, backGroundArea);

        //    if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(GetBackgroundElement()))
        //        return GetRenderer(GetBackgroundElement()).GetBackgroundContentRectangle(measure.Graphics, Rectangle.Round(backGroundArea));
        //    else
        //        return mStandardButton.GetBackgroundContentRectangle(measure, backGroundArea);
        //}

        //public override SizeF GetBackgroundExtent(MeasureHelper measure, SizeF contentSize)
        //{
        //    if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(GetBackgroundElement()))
        //    {
        //        Rectangle content = new Rectangle(new Point(0, 0), Size.Ceiling(contentSize));
        //        contentSize = GetRenderer(GetBackgroundElement()).GetBackgroundExtent(measure.Graphics, content).Size;
        //    }
        //    else
        //        contentSize = mStandardButton.GetBackgroundExtent(measure, contentSize);

        //    return base.GetBackgroundExtent(measure, contentSize);
        //}
    }
}
