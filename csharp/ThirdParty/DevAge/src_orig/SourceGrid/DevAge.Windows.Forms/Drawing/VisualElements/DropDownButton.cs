using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DevAge.Drawing.VisualElements
{
    [Serializable]
    public class DropDownButton : DropDownButtonBase
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public DropDownButton()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public DropDownButton(DropDownButton other)
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
            return new DropDownButton(this);
        }

        protected override void OnDraw(GraphicsCache graphics, System.Drawing.RectangleF area)
        {
            ButtonState state;
            if (Style == ButtonStyle.Disabled)
                state = ButtonState.Inactive;
            else if (Style == ButtonStyle.Pressed)
                state = ButtonState.Pushed;
            else if (Style == ButtonStyle.Hot)
                state = ButtonState.Normal;
            else
                state = ButtonState.Normal;

            ControlPaint.DrawComboButton(graphics.Graphics, Rectangle.Round(area), state);

            if (Style == ButtonStyle.NormalDefault)
            {
                graphics.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(area));
            }

            if (Style == ButtonStyle.Focus)
            {
                using (MeasureHelper measure = new MeasureHelper(graphics))
                {
                    ControlPaint.DrawFocusRectangle(graphics.Graphics, Rectangle.Round(area));
                }
            }
        }

        protected override SizeF OnMeasureContent(MeasureHelper measure, SizeF maxSize)
        {
            //TODO Check to see if it is possible to get the real default size...
            return new SizeF(16, 16);
        }

        //public override System.Drawing.RectangleF GetBackgroundContentRectangle(MeasureHelper measure, System.Drawing.RectangleF backGroundArea)
        //{
        //    backGroundArea = base.GetBackgroundContentRectangle(measure, backGroundArea);

        //    if (backGroundArea.Width > 4)
        //    {
        //        backGroundArea.X += 2;
        //        backGroundArea.Width -= 4;
        //    }

        //    if (backGroundArea.Height > 4)
        //    {
        //        backGroundArea.Y += 2;
        //        backGroundArea.Height -= 4;
        //    }

        //    return backGroundArea;
        //}

        //public override System.Drawing.SizeF GetBackgroundExtent(MeasureHelper measure, System.Drawing.SizeF contentSize)
        //{
        //    contentSize = new SizeF(contentSize.Width + 4, contentSize.Height + 4);

        //    return base.GetBackgroundExtent(measure, contentSize);
        //}
    }
}
