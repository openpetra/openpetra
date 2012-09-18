using System;
using System.Drawing;
using System.ComponentModel;

namespace DevAge.Drawing
{
    /// <summary>
	/// A struct that represents a single border line.
	/// </summary>
    [Serializable]
    public struct BorderLine
	{
		public readonly static BorderLine NoBorder = new BorderLine(Color.White, 0);
        public readonly static BorderLine Black1Width = new BorderLine(Color.Black, 1);

		public BorderLine(Color p_Color)
		{
			Color = p_Color;
			Width = 1;
			DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            Padding = 0;
        }
        public BorderLine(Color p_Color, float p_Width)
		{
			Width = p_Width;
			Color = p_Color;
			DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            Padding = 0;
        }
		public BorderLine(Color p_Color, float p_Width, System.Drawing.Drawing2D.DashStyle dashStyle)
		{
			Width = p_Width;
        	Color = p_Color;
			DashStyle = dashStyle;
            Padding = 0;
		}
        public BorderLine(Color p_Color, float p_Width, System.Drawing.Drawing2D.DashStyle dashStyle, float padding)
        {
            Width = p_Width;
            Color = p_Color;
            DashStyle = dashStyle;
            Padding = padding;
        }

        [DefaultValue(0)]
        public float Width;

        public Color Color;
        private bool ShouldSerializeColor()
        {
            return Color.IsEmpty == false;
        }

        [DefaultValue(System.Drawing.Drawing2D.DashStyle.Solid)]
        public System.Drawing.Drawing2D.DashStyle DashStyle;

        [DefaultValue(0)]
        public float Padding;

		public override string ToString()
		{
			return Color.ToString() + ", Width= " + Width.ToString() + ", DashStyle= " + DashStyle.ToString();
		}

		/// <summary>
		/// Compare to current border with another border.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj==null)
				return false;
			if (obj.GetType() != GetType())
				return false;
			BorderLine l_Other = (BorderLine)obj;
			if (l_Other.Width == Width && l_Other.Color == Color && l_Other.DashStyle == DashStyle && l_Other.Padding == Padding)
				return true;
			else
				return false;
		}

		public override int GetHashCode()
		{
			return Color.GetHashCode();
		}

		public static bool operator == (BorderLine a, BorderLine b)
		{
			return a.Equals(b);
		}

		public static bool operator != (BorderLine a, BorderLine b)
		{
			return a.Equals(b) == false;
		}
	}
}
