using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DevAge.Drawing.VisualElements
{
    [Serializable]
    public class RowHeaderThemed : RowHeaderBase
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public RowHeaderThemed()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public RowHeaderThemed(RowHeaderThemed other)
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
            return new RowHeaderThemed(this);
        }

        #region Properties
        /// <summary>
        /// Standard header used when the XP style are disabled.
        /// </summary>
        private RowHeader mStandardRowHeader = new RowHeader();
        private ColumnHeaderThemed mColHeaderThemed = new ColumnHeaderThemed();

        public override ControlDrawStyle Style
        {
            get { return base.Style; }
            set { base.Style = value; mStandardRowHeader.Style = value; mColHeaderThemed.Style = value; }
        }

        private bool mRotateColHeaderIfNotDefined = true;
        /// <summary>
        /// Gets or sets a property to indicate to rotate the standard column header if the row header is not defined in the current theme.
        /// This is expecially usefull because for many themes the row header is not defined. Default is true.
        /// </summary>
        public virtual bool RotateColHeaderIfNotDefined
        {
            get { return mRotateColHeaderIfNotDefined; }
            set { mRotateColHeaderIfNotDefined = value; }
        }
        #endregion

        #region Helper methods
        protected VisualStyleElement GetBackgroundElement()
        {
            if (Style == ControlDrawStyle.Hot)
                return VisualStyleElement.Header.ItemLeft.Hot;
            else if (Style == ControlDrawStyle.Pressed)
                return VisualStyleElement.Header.ItemLeft.Pressed;
            else
                return VisualStyleElement.Header.ItemLeft.Normal;
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
            base.OnDraw(graphics, area);

            if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(GetBackgroundElement()))
                GetRenderer(GetBackgroundElement()).DrawBackground(graphics.Graphics, Rectangle.Round(area));
            else
            {
                if (RotateColHeaderIfNotDefined)
                {
	                //Swap Height and Width to rotate the bitmap (note I can't use the Graphics transformation matrix because the theme are drawed without using GDI+ I think and the trasformation matrix has no effect)
                    Bitmap bmpBuffer = new Bitmap((int)area.Height, (int)area.Width);
	                Graphics bmpGraphics = Graphics.FromImage(bmpBuffer);
	                try
	                {
                        Rectangle bmpRect = new Rectangle(0, 0, (int)area.Height, (int)area.Width);
                        using (GraphicsCache cache = new GraphicsCache(bmpGraphics))
                        {
		                    mColHeaderThemed.Draw(cache, bmpRect);
                        }

		                bmpBuffer.RotateFlip(RotateFlipType.Rotate270FlipY);

		                graphics.Graphics.DrawImage(bmpBuffer, area);
	                }
	                finally
	                {
		                bmpGraphics.Dispose();
		                bmpBuffer.Dispose();
	                }
                }
                else
                {
                    mStandardRowHeader.Draw(graphics, area);
                }
            }
        }

        public override RectangleF GetBackgroundContentRectangle(MeasureHelper measure, RectangleF backGroundArea)
        {
            backGroundArea = base.GetBackgroundContentRectangle(measure, backGroundArea);

            if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(GetBackgroundElement()))
                return GetRenderer(GetBackgroundElement()).GetBackgroundContentRectangle(measure.Graphics, Rectangle.Round(backGroundArea));
            else
                return mStandardRowHeader.GetBackgroundContentRectangle(measure, backGroundArea);
        }

        public override SizeF GetBackgroundExtent(MeasureHelper measure, SizeF contentSize)
        {
            if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(GetBackgroundElement()))
            {
                Rectangle content = new Rectangle(new Point(0, 0), Size.Ceiling(contentSize));
                contentSize = GetRenderer(GetBackgroundElement()).GetBackgroundExtent(measure.Graphics, content).Size;
            }
            else
                contentSize = mStandardRowHeader.GetBackgroundExtent(measure, contentSize);

            return base.GetBackgroundExtent(measure, contentSize);
        }
    }
}
