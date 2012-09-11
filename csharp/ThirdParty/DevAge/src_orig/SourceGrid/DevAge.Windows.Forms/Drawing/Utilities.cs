using System;
using System.Drawing;

namespace DevAge.Drawing
{
	/// <summary>
	/// A static class with drawing utilities functions
	/// </summary>
	public static class Utilities
	{
		/// <summary>
		/// Draw a rounded rectangle with the specified pen.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="roundRect"></param>
		/// <param name="pen"></param>
		public static void DrawRoundedRectangle(Graphics g, RoundedRectangle roundRect, Pen pen)
		{
			//Remove from the rectangle the border width
			int penWidth = (int)pen.Width;
			Rectangle rectangleBorder = new Rectangle(roundRect.Rectangle.X + penWidth / 2, 
				roundRect.Rectangle.Y + penWidth / 2,
				roundRect.Rectangle.Width - penWidth,
				roundRect.Rectangle.Height - penWidth);

			RoundedRectangle roundToDraw = new RoundedRectangle(rectangleBorder, roundRect.RoundValue);
			g.DrawPath(pen, roundToDraw.ToGraphicsPath());
		}

		/// <summary>
		/// Fill a rounded rectangle with the specified brush.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="roundRect"></param>
		/// <param name="brush"></param>
		public static void FillRoundedRectangle(Graphics g, RoundedRectangle roundRect, Brush brush)
		{
			Rectangle rectangleBorder;
			rectangleBorder = new Rectangle(roundRect.Rectangle.X, 
				roundRect.Rectangle.Y,
				roundRect.Rectangle.Width - 1,
				roundRect.Rectangle.Height - 1);

			RoundedRectangle roundToDraw = new RoundedRectangle(rectangleBorder, roundRect.RoundValue);
			g.FillRegion(brush, new Region( roundToDraw.ToGraphicsPath() ));
		}

		/// <summary>
		/// Draw a 3D border inside the specified rectangle using a linear gradient border color.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="p_HeaderRectangle"></param>
		/// <param name="p_BackColor"></param>
		/// <param name="p_DarkColor"></param>
		/// <param name="p_LightColor"></param>
		/// <param name="p_DarkGradientNumber">The width of the dark border</param>
		/// <param name="p_LightGradientNumber">The width of the light border</param>
		/// <param name="p_Style"></param>
		public static void DrawGradient3DBorder(Graphics g, 
			Rectangle p_HeaderRectangle, 
			Color p_BackColor, 
			Color p_DarkColor, 
			Color p_LightColor,
			int p_DarkGradientNumber,
			int p_LightGradientNumber,
			Gradient3DBorderStyle p_Style)
		{
			Color l_TopLeft, l_BottomRight;
			int l_TopLeftWidth, l_BottomRightWidth;
			if (p_Style == Gradient3DBorderStyle.Raised)
			{
				l_TopLeft = p_LightColor;
				l_TopLeftWidth = p_LightGradientNumber;
				l_BottomRight = p_DarkColor;
				l_BottomRightWidth = p_DarkGradientNumber;
			}
			else
			{
				l_TopLeft = p_DarkColor;
				l_TopLeftWidth = p_DarkGradientNumber;
				l_BottomRight = p_LightColor;
				l_BottomRightWidth = p_LightGradientNumber;
			}

			//TopLeftBorder
			Color[] l_TopLeftGradient = CalculateColorGradient(p_BackColor, l_TopLeft, l_TopLeftWidth);
			using (Pen l_Pen = new Pen(l_TopLeftGradient[0]))
			{
				for (int i = 0; i < l_TopLeftGradient.Length; i++)
				{
					l_Pen.Color = l_TopLeftGradient[l_TopLeftGradient.Length - (i+1)];

					//top
					g.DrawLine(l_Pen, p_HeaderRectangle.Left+i, p_HeaderRectangle.Top+i, p_HeaderRectangle.Right-(i+1), p_HeaderRectangle.Top+i);

					//Left
					g.DrawLine(l_Pen, p_HeaderRectangle.Left+i, p_HeaderRectangle.Top+i, p_HeaderRectangle.Left+i, p_HeaderRectangle.Bottom-(i+1));
				}
			}

			//BottomRightBorder
			Color[] l_BottomRightGradient = CalculateColorGradient(p_BackColor, l_BottomRight, l_BottomRightWidth);
			using (Pen l_Pen = new Pen(l_BottomRightGradient[0]))
			{
				for (int i = 0; i < l_BottomRightGradient.Length; i++)
				{
					l_Pen.Color = l_BottomRightGradient[l_BottomRightGradient.Length - (i+1)];

					//bottom
					g.DrawLine(l_Pen, p_HeaderRectangle.Left+i, p_HeaderRectangle.Bottom-(i+1), p_HeaderRectangle.Right-(i+1), p_HeaderRectangle.Bottom-(i+1));

					//right
					g.DrawLine(l_Pen, p_HeaderRectangle.Right-(i+1), p_HeaderRectangle.Top+i, p_HeaderRectangle.Right-(i+1), p_HeaderRectangle.Bottom-(i+1));
				}
			}
		}


		/// <summary>
		/// Interpolate the specified number of times between start and end color
		/// </summary>
		/// <param name="p_StartColor"></param>
		/// <param name="p_EndColor"></param>
		/// <param name="p_NumberOfGradients"></param>
		/// <returns></returns>
		public static Color[] CalculateColorGradient(Color p_StartColor, Color p_EndColor, int p_NumberOfGradients)
		{
			if (p_NumberOfGradients<2)
				throw new ArgumentException("Invalid Number of gradients, must be 2 or more");
			Color[] l_Colors = new Color[p_NumberOfGradients];
			l_Colors[0] = p_StartColor;
			l_Colors[l_Colors.Length-1] = p_EndColor;

            float l_IncrementA = ((float)(p_EndColor.A - p_StartColor.A)) / (float)p_NumberOfGradients;
            float l_IncrementR = ((float)(p_EndColor.R - p_StartColor.R)) / (float)p_NumberOfGradients;
			float l_IncrementG = ((float)(p_EndColor.G - p_StartColor.G)) / (float)p_NumberOfGradients;
			float l_IncrementB = ((float)(p_EndColor.B - p_StartColor.B)) / (float)p_NumberOfGradients;

			for (int i = 1; i < (l_Colors.Length-1); i++)
			{
                l_Colors[i] = Color.FromArgb( (int)(p_StartColor.A + l_IncrementA * (float)i), 
                    (int)(p_StartColor.R + l_IncrementR * (float)i), 
					(int) (p_StartColor.G + l_IncrementG * (float)i ),
					(int) (p_StartColor.B + l_IncrementB * (float)i ) );
			}

			return l_Colors;
		}

        /// <summary>
        /// Calculate the middle color between the start and the end color.
        /// </summary>
        /// <param name="p_StartColor"></param>
        /// <param name="p_EndColor"></param>
        /// <returns></returns>
        public static Color CalculateMiddleColor(Color p_StartColor, Color p_EndColor)
        {
            return CalculateColorGradient(p_StartColor, p_EndColor, 3)[1];
        }

        /// <summary>
        /// Calculate a darker or lighter color using the source specified.
        /// A light of 1 is White, a light of -1 is black. All the other values are an interpolation from the source color.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="light"></param>
        /// <returns></returns>
        public static Color CalculateLightDarkColor(Color source, float light)
        {
            if (light == 0)
                return source;
            if (light > 1 || light < -1)
                throw new ArgumentException("Must be between 1 and -1", "light");

            float _IncrementR, _IncrementG, _IncrementB;

            if (light < 0)
            {
                _IncrementR = ((float)(source.R)) / (float)100;
                _IncrementG = ((float)(source.G)) / (float)100;
                _IncrementB = ((float)(source.B)) / (float)100;
            }
            else
            {
                _IncrementR = ((float)(255 - source.R)) / (float)100;
                _IncrementG = ((float)(255 - source.G)) / (float)100;
                _IncrementB = ((float)(255 - source.B)) / (float)100;
            }

            int newR, newG, newB;

            newR = source.R + (int)(_IncrementR * light * (float)100);
            newG = source.G + (int)(_IncrementG * light * (float)100);
            newB = source.B + (int)(_IncrementB * light * (float)100);

            if (newR > 255)
                newR = 255;
            else if (newR < 0)
                newR = 0;
            if (newG > 255)
                newG = 255;
            else if (newG < 0)
                newG = 0;
            if (newB > 255)
                newB = 255;
            else if (newB < 0)
                newB = 0;
            
            return Color.FromArgb(source.A, newR, newG, newB);
        }

        ///// <summary>
        ///// Calculates the location of an object inside a client rectangle using the specified alignment
        ///// </summary>
        ///// <param name="align"></param>
        ///// <param name="clientRect"></param>
        ///// <param name="objectSize"></param>
        ///// <returns></returns>
        //public static Point CalculateContentLocation(DevAge.Drawing.ContentAlignment align, 
        //                                            Rectangle clientRect, 
        //                                            Size objectSize)
        //{
        //    //default X left
        //    PointF pointf = Point.Empty;

        //    if ( IsTop(align) ) //Y Top
        //        pointf.Y = (float)clientRect.Top;
        //    else if ( IsBottom(align) ) //Y bottom
        //        pointf.Y = (float)clientRect.Bottom - objectSize.Height;
        //    else //Y middle
        //        pointf.Y = (float)clientRect.Top + ((float)clientRect.Height)/2.0F -  ((float)objectSize.Height)/2.0F;

        //    if ( IsCenter(align) )//X Center
        //        pointf.X = (float)clientRect.Left + ((float)clientRect.Width)/2.0F - ((float)objectSize.Width)/2.0F;
        //    else if ( IsRight(align) )//X Right
        //        pointf.X = (float)clientRect.Left + (float)clientRect.Width - ((float)objectSize.Width);
        //    else //X left
        //        pointf.X = (float)clientRect.Left;

        //    return Point.Round( pointf );
        //}

        ///// <summary>
        ///// Calculate the rectangle of the content specified
        ///// </summary>
        ///// <param name="align"></param>
        ///// <param name="clientRect"></param>
        ///// <param name="objectSize"></param>
        ///// <returns></returns>
        //public static Rectangle CalculateContentRectangle(DevAge.Drawing.ContentAlignment align, 
        //    Rectangle clientRect, 
        //    Size objectSize)
        //{
        //    Point contentPoint = CalculateContentLocation(align, clientRect, objectSize);
        //    Rectangle rect = new Rectangle(contentPoint, objectSize);
        //    rect.Intersect(clientRect);
        //    return rect;
        //}

		public static DevAge.Drawing.ContentAlignment StringFormatToContentAlignment(System.Drawing.StringFormat p_StringFormat)
		{
			if (IsBottom(p_StringFormat) && IsLeft(p_StringFormat))
				return DevAge.Drawing.ContentAlignment.BottomLeft;
			else if (IsBottom(p_StringFormat) && IsRight(p_StringFormat))
				return DevAge.Drawing.ContentAlignment.BottomRight;
			else if (IsBottom(p_StringFormat) && IsCenter(p_StringFormat))
				return DevAge.Drawing.ContentAlignment.BottomCenter;

			else if (IsTop(p_StringFormat) && IsLeft(p_StringFormat))
				return DevAge.Drawing.ContentAlignment.TopLeft;
			else if (IsTop(p_StringFormat) && IsRight(p_StringFormat))
				return DevAge.Drawing.ContentAlignment.TopRight;
			else if (IsTop(p_StringFormat) && IsCenter(p_StringFormat))
				return DevAge.Drawing.ContentAlignment.TopCenter;

			else if (IsMiddle(p_StringFormat) && IsLeft(p_StringFormat))
				return DevAge.Drawing.ContentAlignment.MiddleLeft;
			else if (IsMiddle(p_StringFormat) && IsRight(p_StringFormat))
				return DevAge.Drawing.ContentAlignment.MiddleRight;
			else //if (Utility.IsMiddle(StringFormat) && Utility.IsCenter(StringFormat))
				return DevAge.Drawing.ContentAlignment.MiddleCenter;
		}

		public static void ApplyContentAlignmentToStringFormat(DevAge.Drawing.ContentAlignment pAlignment, StringFormat stringFormat)
		{
			if (IsBottom(pAlignment))
				stringFormat.LineAlignment = StringAlignment.Far;
			else if (IsMiddle(pAlignment))
				stringFormat.LineAlignment = StringAlignment.Center;
			else //if (IsTop(pAlignment))
				stringFormat.LineAlignment = StringAlignment.Near;

			if (IsRight(pAlignment))
				stringFormat.Alignment = StringAlignment.Far;
			else if (IsCenter(pAlignment))
				stringFormat.Alignment = StringAlignment.Center;
			else //if (IsLeft(pAlignment))
				stringFormat.Alignment = StringAlignment.Near;
		}

		#region ContentAlign Utility
		public static bool IsBottom(DevAge.Drawing.ContentAlignment a)
		{
			return (a == DevAge.Drawing.ContentAlignment.BottomCenter ||
				a == DevAge.Drawing.ContentAlignment.BottomLeft ||
				a == DevAge.Drawing.ContentAlignment.BottomRight);
		}
		public static bool IsTop(DevAge.Drawing.ContentAlignment a)
		{
			return (a == DevAge.Drawing.ContentAlignment.TopCenter ||
				a == DevAge.Drawing.ContentAlignment.TopLeft ||
				a == DevAge.Drawing.ContentAlignment.TopRight);
		}
		public static bool IsMiddle(DevAge.Drawing.ContentAlignment a)
		{
			return (a == DevAge.Drawing.ContentAlignment.MiddleCenter ||
				a == DevAge.Drawing.ContentAlignment.MiddleLeft ||
				a == DevAge.Drawing.ContentAlignment.MiddleRight);
		}
		public static bool IsCenter(DevAge.Drawing.ContentAlignment a)
		{
			return (a == DevAge.Drawing.ContentAlignment.BottomCenter ||
				a == DevAge.Drawing.ContentAlignment.MiddleCenter ||
				a == DevAge.Drawing.ContentAlignment.TopCenter);
		}
		public static bool IsLeft(DevAge.Drawing.ContentAlignment a)
		{
			return (a == DevAge.Drawing.ContentAlignment.BottomLeft ||
				a == DevAge.Drawing.ContentAlignment.MiddleLeft ||
				a == DevAge.Drawing.ContentAlignment.TopLeft);
		}
		public static bool IsRight(DevAge.Drawing.ContentAlignment a)
		{
			return (a == DevAge.Drawing.ContentAlignment.BottomRight ||
				a == DevAge.Drawing.ContentAlignment.MiddleRight ||
				a == DevAge.Drawing.ContentAlignment.TopRight);
		}

		public static bool IsBottom(StringFormat a)
		{
			return (a.LineAlignment == StringAlignment.Far);
		}
		public static bool IsTop(StringFormat a)
		{
			return (a.LineAlignment == StringAlignment.Near);
		}
		public static bool IsMiddle(StringFormat a)
		{
			return (a.LineAlignment == StringAlignment.Center);
		}
		public static bool IsCenter(StringFormat a)
		{
			return (a.Alignment == StringAlignment.Center);
		}
		public static bool IsLeft(StringFormat a)
		{
			return (a.Alignment == StringAlignment.Near);
		}
		public static bool IsRight(StringFormat a)
		{
			return (a.Alignment == StringAlignment.Far);
		}
		#endregion

		/// <summary>
		/// Converts the specified image to an array of byte using the specified format.
		/// </summary>
		/// <param name="img"></param>
		/// <param name="imgFormat"></param>
		/// <returns></returns>
		public static byte[] ImageToBytes(System.Drawing.Image img, System.Drawing.Imaging.ImageFormat imgFormat)
		{
			if (img == null)
				return new byte[0];

			byte[] bytes;
			using (System.IO.MemoryStream mem = new System.IO.MemoryStream())
			{
				img.Save(mem, imgFormat);
				bytes = mem.ToArray();
			}
			return bytes;
		}

		/// <summary>
		/// Converts the specified byte array to an Image object.
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static System.Drawing.Image BytesToImage(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
				return null;

			System.Drawing.Image img;
			using (System.IO.MemoryStream mem = new System.IO.MemoryStream(bytes))
			{
				img = System.Drawing.Image.FromStream(mem);
			}
			return img;
		}

		private static System.Drawing.Imaging.ImageAttributes s_DisabledImageAttr;
		/// <summary>
		/// Create a disabled version of the image.
		/// </summary>
		/// <param name="image">The image to convert</param>
		/// <param name="background">The Color of the background behind the image. The background parameter is used to calculate the fill color of the disabled image so that it is always visible against the background.</param>
		/// <returns></returns>
		public static Image CreateDisabledImage(Image image, Color background)
		{
			if (image == null)
				return null;

			Size imgSize = image.Size;
			if (s_DisabledImageAttr == null)
			{
				float[][] arrayJagged = new float[5][];
				arrayJagged[0] = new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f } ;
				arrayJagged[1] = new float[5] { 0.2577f, 0.2577f, 0.2577f, 0f, 0f } ;
				arrayJagged[2] = new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f } ;
				float[] arraySingle = new float[5];
				arraySingle[3] = 1f;
				arrayJagged[3] = arraySingle;
				arrayJagged[4] = new float[5] { 0.38f, 0.38f, 0.38f, 0f, 1f } ;
				System.Drawing.Imaging.ColorMatrix matrix = new System.Drawing.Imaging.ColorMatrix(arrayJagged);
				s_DisabledImageAttr = new System.Drawing.Imaging.ImageAttributes();
				s_DisabledImageAttr.ClearColorKey();

				s_DisabledImageAttr.SetColorMatrix(matrix);
			}

			Bitmap bitmap = new Bitmap(image.Width, image.Height);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.DrawImage(image, new Rectangle(0, 0, imgSize.Width, imgSize.Height), 0, 0, imgSize.Width, imgSize.Height, GraphicsUnit.Pixel, s_DisabledImageAttr);
			}

			return bitmap;
		}

        public static SizeF CheckMeasure(SizeF measureSize, System.Drawing.SizeF minSize, System.Drawing.SizeF maxSize)
        {
            if (minSize.Width > 0 && measureSize.Width < minSize.Width)
                measureSize.Width = minSize.Width;
            if (minSize.Height > 0 && measureSize.Height < minSize.Height)
                measureSize.Height = minSize.Height;

            if (maxSize.Width > 0 && measureSize.Width > maxSize.Width)
                measureSize.Width = maxSize.Width;
            if (maxSize.Height > 0 && measureSize.Height > maxSize.Height)
                measureSize.Height = maxSize.Height;

            return measureSize;
        }

        /// <summary>
        /// Convert a SizeF structure to a Size structure rounding the value to the largest integer using Ceiling method.
        /// </summary>
        /// <param name="sizef"></param>
        /// <returns></returns>
        public static Size SizeFToSize(SizeF sizef)
        {
            return new Size((int)Math.Ceiling(sizef.Width), (int)Math.Ceiling(sizef.Height));
        }
	}
}
