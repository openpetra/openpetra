using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevAge.Drawing
{
    [Serializable]
    public struct Padding
    {
        public static Padding Empty = new Padding();

        public Padding(float all)
        {
            Left = all;
            Right = all;
            Top = all;
            Bottom = all;
        }

        public Padding(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public float Left;
        public float Right;
        public float Top;
        public float Bottom;

        public bool IsEmpty
        {
            get { return Left == 0 && Right == 0 && Top == 0 && Bottom == 0; }
        }

        public RectangleF GetContentRectangle(RectangleF backGroundArea)
        {
            if (IsEmpty)
                return backGroundArea;
            else
            {
                return new RectangleF(backGroundArea.X + Left, backGroundArea.Y + Top,
                                backGroundArea.Width - (Left + Right), backGroundArea.Height - (Top + Bottom));
            }
        }

        public SizeF GetExtent(SizeF contentSize)
        {
            if (IsEmpty)
                return contentSize;
            else
            {
                return new SizeF(contentSize.Width + (Left + Right), contentSize.Height + (Top + Bottom));
            }
        }

        public override string ToString()
        {
            return "Left:" + Left.ToString() + ",Right:" + Right.ToString() + ",Top:" + Top.ToString() + ",Bottom:" + Bottom.ToString();
        }

        /// <summary>
        /// Compare to current padding with another padding.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            Padding other = (Padding)obj;
            if (other.Top == Top && other.Bottom == Bottom &&
                other.Right == Right && other.Left == Left)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Top.GetHashCode();
        }

        public static bool operator ==(Padding a, Padding b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Padding a, Padding b)
        {
            return a.Equals(b) == false;
        }
    }
}
