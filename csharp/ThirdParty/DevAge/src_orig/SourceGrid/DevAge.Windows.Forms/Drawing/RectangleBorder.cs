using System;
using System.Drawing;
using System.ComponentModel;

namespace DevAge.Drawing
{
    /// <summary>
    /// A struct that represents the borders of a cell. Contains 4 borders: Right, Left, Top, Bottom.
    /// If you have 2 adjacent cells and want to create a 1 pixel width border, you must set width 1 for one cell and width 0 for the other. Usually a cell has only Right and Bottom border.
    /// The bottom and top border lines are drawed over the right and left lines.
    /// </summary>
    [Serializable]
    public struct RectangleBorder : IBorder
    {
        public readonly static RectangleBorder NoBorder = new RectangleBorder(BorderLine.NoBorder);
        public readonly static RectangleBorder RectangleBlack1Width = new RectangleBorder(new BorderLine(Color.Black, 1));

        #region Constructor
        /// <summary>
        /// Construct a RectangleBorder with the same border on all the side
        /// </summary>
        /// <param name="p_Border"></param>
        public RectangleBorder(BorderLine p_Border)
        {
            Top = p_Border;
            Bottom = p_Border;
            Left = p_Border;
            Right = p_Border;
        }

        /// <summary>
        /// Construct a RectangleBorder with the specified Right and Bottom border and a 0 Left and Top border
        /// </summary>
        /// <param name="p_Right"></param>
        /// <param name="p_Bottom"></param>
        public RectangleBorder(BorderLine p_Right, BorderLine p_Bottom)
        {
            Right = p_Right;
            Bottom = p_Bottom;
            Top = new BorderLine(Color.White, 0);
            Left = new BorderLine(Color.White, 0);
        }

        /// <summary>
        /// Construct a RectangleBorder with the specified borders
        /// </summary>
        /// <param name="p_Top"></param>
        /// <param name="p_Bottom"></param>
        /// <param name="p_Left"></param>
        /// <param name="p_Right"></param>
        public RectangleBorder(BorderLine p_Top, BorderLine p_Bottom, BorderLine p_Left, BorderLine p_Right)
        {
            Top = p_Top;
            Bottom = p_Bottom;
            Left = p_Left;
            Right = p_Right;
        }
        #endregion

        #region Properties
        public BorderLine Top;
        public BorderLine Bottom;
        public BorderLine Left;
        public BorderLine Right;
        #endregion

        #region Helper methods
        /// <summary>
        /// Change the color of the current struct instance and return a copy of the modified struct.
        /// </summary>
        /// <param name="p_Color"></param>
        /// <returns></returns>
        public RectangleBorder SetColor(Color p_Color)
        {
            Top = new BorderLine(p_Color, Top.Width, Top.DashStyle, Top.Padding);
            Bottom = new BorderLine(p_Color, Bottom.Width, Bottom.DashStyle, Bottom.Padding);
            Left = new BorderLine(p_Color, Left.Width, Left.DashStyle, Left.Padding);
            Right = new BorderLine(p_Color, Right.Width, Right.DashStyle, Right.Padding);

            return this;
        }

        /// <summary>
        /// Change the dashStyle of the current struct instance and return a copy of the modified struct.
        /// </summary>
        /// <param name="dashStyle"></param>
        /// <returns></returns>
        public RectangleBorder SetDashStyle(System.Drawing.Drawing2D.DashStyle dashStyle)
        {
            Top = new BorderLine(Top.Color, Top.Width, dashStyle, Top.Padding);
            Bottom = new BorderLine(Bottom.Color, Bottom.Width, dashStyle, Bottom.Padding);
            Left = new BorderLine(Left.Color, Left.Width, dashStyle, Left.Padding);
            Right = new BorderLine(Right.Color, Right.Width, dashStyle, Right.Padding);

            return this;
        }

        /// <summary>
        /// Change the width of the current struct instance and return a copy of the modified struct.
        /// </summary>
        /// <param name="p_Width"></param>
        /// <returns></returns>
        public RectangleBorder SetWidth(int p_Width)
        {
            Top = new BorderLine(Top.Color, p_Width, Top.DashStyle, Top.Padding);
            Bottom = new BorderLine(Bottom.Color, p_Width, Bottom.DashStyle, Bottom.Padding);
            Left = new BorderLine(Left.Color, p_Width, Left.DashStyle, Left.Padding);
            Right = new BorderLine(Right.Color, p_Width, Right.DashStyle, Right.Padding);

            return this;
        }

        /// <summary>
        /// Change the width of the current struct instance and return a copy of the modified struct.
        /// </summary>
        /// <returns></returns>
        public RectangleBorder SetPadding(int padding)
        {
            Top = new BorderLine(Top.Color, Top.Width, Top.DashStyle, padding);
            Bottom = new BorderLine(Bottom.Color, Bottom.Width, Bottom.DashStyle, padding);
            Left = new BorderLine(Left.Color, Left.Width, Left.DashStyle, padding);
            Right = new BorderLine(Right.Color, Right.Width, Right.DashStyle, padding);

            return this;
        }
        #endregion

        #region .NET override methods (Equals, GetHashCode, operators, ToString, ...)

        public override string ToString()
        {
            return "Top:" + Top.ToString() + " Bottom:" + Bottom.ToString() + " Left:" + Left.ToString() + " Right:" + Right.ToString();
        }

        /// <summary>
        /// Compare to current border with another border.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            RectangleBorder l_Other = (RectangleBorder)obj;
            if (l_Other.Left == Left && l_Other.Bottom == Bottom &&
                l_Other.Top == Top && l_Other.Right == Right)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode();
        }

        public static bool operator ==(RectangleBorder a, RectangleBorder b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(RectangleBorder a, RectangleBorder b)
        {
            return a.Equals(b) == false;
        }

        #endregion

        #region Factory methods
        /// <summary>
        /// Create an Inset border
        /// </summary>
        /// <param name="p_width"></param>
        /// <param name="p_ShadowColor"></param>
        /// <param name="p_LightColor"></param>
        public static RectangleBorder CreateInsetBorder(int p_width, Color p_ShadowColor, Color p_LightColor)
        {
            RectangleBorder l_Border = new RectangleBorder(new BorderLine(Color.White)); ;

            l_Border.Top = new BorderLine(p_ShadowColor, p_width);
            l_Border.Left = new BorderLine(p_ShadowColor, p_width);
            l_Border.Bottom = new BorderLine(p_LightColor, p_width);
            l_Border.Right = new BorderLine(p_LightColor, p_width);

            return l_Border;
        }

        /// <summary>
        /// Create a Raised border
        /// </summary>
        /// <param name="p_width"></param>
        /// <param name="p_ShadowColor"></param>
        /// <param name="p_LightColor"></param>
        public static RectangleBorder CreateRaisedBorder(int p_width, Color p_ShadowColor, Color p_LightColor)
        {
            RectangleBorder l_Border = new RectangleBorder(new BorderLine(Color.White)); ;

            l_Border.Top = new BorderLine(p_LightColor, p_width);
            l_Border.Left = new BorderLine(p_LightColor, p_width);
            l_Border.Bottom = new BorderLine(p_ShadowColor, p_width);
            l_Border.Right = new BorderLine(p_ShadowColor, p_width);

            return l_Border;
        }

        #endregion

        #region IBorder Members

        public RectangleF GetContentRectangle(RectangleF backGroundArea)
        {
            backGroundArea.Y += Top.Width + Top.Padding;
            backGroundArea.X += Left.Width + Left.Padding;
            backGroundArea.Width -= Left.Width + Right.Width + Left.Padding + Right.Padding;
            backGroundArea.Height -= Top.Width + Bottom.Width + Top.Padding + Bottom.Padding;

            return backGroundArea;
        }

        public SizeF GetExtent(SizeF contentSize)
        {
            //Add Border Width
            contentSize.Width += Left.Width + Right.Width + Left.Padding + Right.Padding;
            contentSize.Height += Top.Width + Bottom.Width + Top.Padding + Bottom.Padding;

            return contentSize;
        }

        //public void Draw(GraphicsCache graphics, RectangleF rectangle)
        //{
        //    RectangleBorder border = this;

        //    //Calculate the padding
        //    rectangle = new RectangleF(rectangle.X + Left.Padding, rectangle.Y + Top.Padding, rectangle.Width - (Left.Padding + Right.Padding), rectangle.Height - (Top.Padding + Bottom.Padding));

        //    if (rectangle.Width <= 0 || rectangle.Height <= 0)
        //        return;

        //    PensCache pens = graphics.PensCache;
        //    if (border.Left.Width > 0)
        //    {
        //        Pen leftPen = pens.GetPen(border.Left.Color, 1, border.Left.DashStyle);
        //        for (int i = 0; i < border.Left.Width; i++)
        //            graphics.Graphics.DrawLine(leftPen, rectangle.X + i, rectangle.Y, rectangle.X + i, rectangle.Bottom - 1);
        //    }

        //    if (border.Bottom.Width > 0)
        //    {
        //        Pen bottomPen = pens.GetPen(border.Bottom.Color, 1, border.Bottom.DashStyle);
        //        for (int i = 1; i <= border.Bottom.Width; i++)
        //            graphics.Graphics.DrawLine(bottomPen, rectangle.X, rectangle.Bottom - i, rectangle.Right - 1, rectangle.Bottom - i);
        //    }

        //    if (border.Right.Width > 0)
        //    {
        //        Pen rightPen = pens.GetPen(border.Right.Color, 1, border.Right.DashStyle);
        //        for (int i = 1; i <= border.Right.Width; i++)
        //            graphics.Graphics.DrawLine(rightPen, rectangle.Right - i, rectangle.Y, rectangle.Right - i, rectangle.Bottom - 1);
        //    }

        //    if (border.Top.Width > 0)
        //    {
        //        Pen topPen = pens.GetPen(border.Top.Color, 1, border.Top.DashStyle);
        //        for (int i = 0; i < border.Top.Width; i++)
        //            graphics.Graphics.DrawLine(topPen, rectangle.X, rectangle.Y + i, rectangle.Right - 1, rectangle.Y + i);
        //    }
        //}

        public void Draw(GraphicsCache graphics, RectangleF rectangle)
        {
            RectangleBorder border = this;

            //Calculate the padding
            rectangle = new RectangleF(rectangle.X + Left.Padding, rectangle.Y + Top.Padding, rectangle.Width - (Left.Padding + Right.Padding), rectangle.Height - (Top.Padding + Bottom.Padding));

            if (rectangle.Width <= 0 || rectangle.Height <= 0)
                return;

            PensCache pens = graphics.PensCache;
            if (border.Left.Width > 0)
            {
                Pen leftPen = pens.GetPen(border.Left.Color, border.Left.Width, border.Left.DashStyle);
                float x;
                float bottom;
                if (border.Left.Width > 1)
                {
                    x = rectangle.X + ((float)border.Left.Width) / 2;
                    bottom = rectangle.Bottom;
                }
                else
                {
                    x = rectangle.X;
                    bottom = rectangle.Bottom - 1;
                }
                graphics.Graphics.DrawLine(leftPen, new PointF(x, rectangle.Y),
                                                    new PointF(x, bottom));
            }

            if (border.Right.Width > 0)
            {
                Pen rightPen = pens.GetPen(border.Right.Color, border.Right.Width, border.Right.DashStyle);
                float x;
                float bottom;
                if (border.Right.Width > 1)
                {
                    x = rectangle.Right - border.Right.Width / 2;
                    bottom = rectangle.Bottom;
                }
                else
                {
                    x = rectangle.Right - 1;
                    bottom = rectangle.Bottom - 1;
                }
                graphics.Graphics.DrawLine(rightPen, new PointF(x, rectangle.Y),
                                                    new PointF(x, bottom));
            }

            if (border.Top.Width > 0)
            {
                Pen topPen = pens.GetPen(border.Top.Color, border.Top.Width, border.Top.DashStyle);
                float y;
                float right;
                if (border.Top.Width > 1)
                {
                    y = rectangle.Y + border.Top.Width / 2;
                    right = rectangle.Right;
                }
                else
                {
                    y = rectangle.Y;
                    right = rectangle.Right - 1;
                }
                graphics.Graphics.DrawLine(topPen, new PointF(rectangle.X, y),
                                                   new PointF(right, y));
            }

            if (border.Bottom.Width > 0)
            {
                Pen bottomPen = pens.GetPen(border.Bottom.Color, border.Bottom.Width, border.Bottom.DashStyle);
                float y;
                float right;
                if (border.Bottom.Width > 1)
                {
                    y = rectangle.Bottom - border.Bottom.Width / 2;
                    right = rectangle.Right;
                }
                else
                {
                    y = rectangle.Bottom - 1;
                    right = rectangle.Right - 1;
                }
                graphics.Graphics.DrawLine(bottomPen, new PointF(rectangle.X, y),
                                                      new PointF(right, y));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="point"></param>
        /// <param name="distanceFromBorder">Returns the distance of the specified point from the border rectangle. -1 if is not inside the border. Returns a positive value or 0 if inside the border. Consider always the distance from the outer border.</param>
        /// <returns></returns>
        public RectanglePartType GetPointPartType(RectangleF area, PointF point, out float distanceFromBorder)
        {
            if (area.Contains(point) == false)
            {
                distanceFromBorder = -1;
                return RectanglePartType.None;
            }

            RectangleF contentRect = GetContentRectangle(area);

            if (contentRect.Contains(point))
            {
                distanceFromBorder = -1;
                return RectanglePartType.ContentArea;
            }

            if (point.X >= area.Left && point.X < contentRect.Left)
            {
                distanceFromBorder = point.X - area.Left;
                return RectanglePartType.LeftBorder;
            }

            if (point.X < area.Right && point.X >= contentRect.Right)
            {
                distanceFromBorder = area.Right - point.X;
                return RectanglePartType.RightBorder;
            }

            if (point.Y >= area.Top && point.Y < contentRect.Top)
            {
                distanceFromBorder = point.Y - area.Top;
                return RectanglePartType.TopBorder;
            }

            if (point.Y < area.Bottom && point.Y >= contentRect.Bottom)
            {
                distanceFromBorder = area.Bottom - point.Y;
                return RectanglePartType.BottomBorder;
            }

            distanceFromBorder = -1;
            return RectanglePartType.None;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
        #endregion
    }
}
