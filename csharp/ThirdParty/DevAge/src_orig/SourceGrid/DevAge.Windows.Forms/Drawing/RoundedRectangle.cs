using System;
using System.Drawing;

namespace DevAge.Drawing
{
	/// <summary>
	/// Rapresents a rounded rectangle, takes a rectangle and a round value from 0 to 1. Can be converted to a GraphicsPath for drawing operations.
	/// See also DevAge.Drawing.Utilities.FillRoundedRectangle and DrawRoundedRectangle methods.
	/// </summary>
    [Serializable]
    public struct RoundedRectangle
	{
		/// <summary>
		/// Costructor
		/// </summary>
		/// <param name="rect">Content rectangle</param>
		/// <param name="roundValue">The amount to round the rectangle. Can be any vavlues from 0 to 1. Set to 0 to draw a standard rectangle, 1 to have a full rounded rectangle.</param>
		public RoundedRectangle(Rectangle rect, double roundValue)
		{
			mRectangle = rect;
			mRoundValue = roundValue;
		}

		private Rectangle mRectangle;
		public Rectangle Rectangle
		{
			get{return mRectangle;}
			set{mRectangle = value;}
		}

		private double mRoundValue;

		/// <summary>
		/// The amount to round the rectangle. Can be any values from 0 to 1. Set to 0 to draw a standard rectangle, 1 to have a full rounded rectangle.
		/// </summary>
		public double RoundValue
		{
			get{return mRoundValue;}
			set
			{
				if (mRoundValue < 0 || mRoundValue > 1)
					throw new ApplicationException("Invalid value, must be a value from 0 to 1");
				mRoundValue = value;
			}
		}

		/// <summary>
		/// Converts this structure to a GraphicsPath object, used to draw to a Graphics device.
		/// Consider that you can create a Region with a GraphicsPath object using one of the Region constructor.
		/// </summary>
		/// <returns></returns>
		public System.Drawing.Drawing2D.GraphicsPath ToGraphicsPath()
		{
			if (mRectangle.IsEmpty)
				return new System.Drawing.Drawing2D.GraphicsPath();

			System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

			if (mRoundValue == 0)
			{
				//Remove 1 from height and width to draw the border in the right location
				//path.AddRectangle(new Rectangle(new Point(mRectangle.X - 1, mRectangle.Y - 1), mRectangle.Size));
				path.AddRectangle(mRectangle);
			}
			else
			{
				int x = mRectangle.X;
				int y = mRectangle.Y;

				int lineShift = 0;
                int lineShiftX2 = 0;

                //Basically the RoundValue is a percentage of the line to curve, so I simply multiply it with the lower side (height or width)

				if (mRectangle.Height < mRectangle.Width)
				{
                    lineShift = (int)((double)mRectangle.Height * mRoundValue);
                    lineShiftX2 = lineShift * 2;
				}
				else
				{
                    lineShift = (int)((double)mRectangle.Width * mRoundValue);
                    lineShiftX2 = lineShift * 2;
				}

				//Top
                path.AddLine(lineShift + x, 0 + y, (mRectangle.Width - lineShift) + x, 0 + y);
				//Angle Top Right
                path.AddArc((mRectangle.Width - lineShiftX2) + x, 0 + y,
                    lineShiftX2, lineShiftX2, 
					270, 90);
				//Right
                path.AddLine(mRectangle.Width + x, lineShift + y, mRectangle.Width + x, (mRectangle.Height - lineShift) + y);
				//Angle Bottom Right
                path.AddArc((mRectangle.Width - lineShiftX2) + x, (mRectangle.Height - lineShiftX2) + y,
                    lineShiftX2, lineShiftX2, 
					0, 90);
				//Bottom
                path.AddLine((mRectangle.Width - lineShift) + x, mRectangle.Height + y, lineShift + x, mRectangle.Height + y);
				//Angle Bottom Left
                path.AddArc(0 + x, (mRectangle.Height - lineShiftX2) + y,
                    lineShiftX2, lineShiftX2, 
					90, 90);
				//Left
                path.AddLine(0 + x, (mRectangle.Height - lineShift) + y, 0 + x, lineShift + y);
				//Angle Top Left
				path.AddArc(0 + x, 0 + y,
                    lineShiftX2, lineShiftX2, 
					180, 90);
			}

			return path;
		}
	}
}