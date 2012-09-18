using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DevAge.Drawing.VisualElements
{
    [Serializable]
    public class SortIndicatorThemed : SortIndicator
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public SortIndicatorThemed()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public SortIndicatorThemed(SortIndicatorThemed other)
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
            return new SortIndicatorThemed(this);
        }

        protected VisualStyleElement GetSortElement()
        {
            if (SortStyle == HeaderSortStyle.Ascending)
                return VisualStyleElement.Header.SortArrow.SortedUp;
            else if (SortStyle == HeaderSortStyle.Descending)
                return VisualStyleElement.Header.SortArrow.SortedDown;
            else
                return null;
        }

        /// <summary>
        /// Gets the System.Windows.Forms.VisualStyles.VisualStyleRenderer to draw the specified element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public System.Windows.Forms.VisualStyles.VisualStyleRenderer GetRenderer(VisualStyleElement element)
        {
            return new System.Windows.Forms.VisualStyles.VisualStyleRenderer(element);
        }

        protected override void OnDraw(GraphicsCache graphics, System.Drawing.RectangleF area)
        {
            if (SortStyle != HeaderSortStyle.None && Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(GetSortElement()))
            {
                Rectangle sortArea = Rectangle.Round(area);

                VisualStyleRenderer renderer = GetRenderer(GetSortElement());
                Size sortSize = renderer.GetPartSize(graphics.Graphics, sortArea, ThemeSizeType.Draw);
                sortArea = new Rectangle(sortArea.Right - sortSize.Width, sortArea.Top, sortSize.Width, sortSize.Height);

                renderer.DrawBackground(graphics.Graphics, sortArea);
            }
            else
            {
                base.OnDraw(graphics, area);
            }
        }

        protected override SizeF OnMeasureContent(MeasureHelper measure, SizeF maxSize)
        {
            if (SortStyle != HeaderSortStyle.None && Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(GetSortElement()))
            {
                VisualStyleRenderer renderer = GetRenderer(GetSortElement());
                return renderer.GetPartSize(measure.Graphics, ThemeSizeType.Draw);
            }
            else
                return base.OnMeasureContent(measure, maxSize);
        }
    }
}
