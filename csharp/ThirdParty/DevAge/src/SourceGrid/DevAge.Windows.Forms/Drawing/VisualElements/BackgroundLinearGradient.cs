using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevAge.Drawing.VisualElements
{
    /// <summary>
    /// Class used to draw a standard linear gradient background. If FirstColor == SecondColor a solid color is drawed.
    /// </summary>
    [Serializable]
    public class BackgroundLinearGradient : VisualElementBase
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public BackgroundLinearGradient()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="firstColor"></param>
        /// <param name="secondColor"></param>
        /// <param name="angle"></param>
        public BackgroundLinearGradient(Color firstColor, Color secondColor, float angle)
        {
            FirstColor = firstColor;
            SecondColor = secondColor;
            Angle = angle;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public BackgroundLinearGradient(BackgroundLinearGradient other):base(other)
        {
            FirstColor = other.FirstColor;
            SecondColor = other.SecondColor;
            Angle = other.Angle;
            BlendFactors = other.BlendFactors;
            BlendPositions = other.BlendPositions;
        }
        #endregion

        #region Properties
        private Color mFirstColor = Color.Empty;
        /// <summary>
        /// Gets or sets the first back color of the content.
        /// </summary>
        public virtual Color FirstColor
        {
            get { return mFirstColor; }
            set { mFirstColor = value; }
        }
        protected virtual bool ShouldSerializeFirstColor()
        {
            return FirstColor != Color.Empty;
        }

        private Color mSecondColor = Color.Empty;
        /// <summary>
        /// Gets or sets the second back color of the content.
        /// </summary>
        public virtual Color SecondColor
        {
            get { return mSecondColor; }
            set { mSecondColor = value; }
        }
        protected virtual bool ShouldSerializeSecondColor()
        {
            return SecondColor != Color.Empty;
        }

        private float mAngle = 0;
        /// <summary>
        /// Gets or sets the angle of the gradient
        /// </summary>
        public virtual float Angle
        {
            get { return mAngle; }
            set { mAngle = value; }
        }
        protected virtual bool ShouldSerializeAngle()
        {
            return Angle != 0;
        }

        private float[] mBlendFactors = null;
        /// <summary>
        /// Gradients are commonly used to smoothly shade the interiors of shapes. A blend pattern is defined by two arrays (Factors and Positions) that each contain the same number of elements. Each element of the Positions array represents a proportion of the distance along the gradient line. Each element of the Factors array represents the proportion of the starting and ending colors in the gradient blend at the position along the gradient line represented by the corresponding element in the Positions array.
        /// For example, if corresponding elements of the Positions and Factors arrays are 0.2 and 0.3, respectively, for a linear gradient from blue to red along a 100-pixel line, the color 20 pixels along that line (20 percent of the distance) consists of 30 percent blue and 70 percent red.
        /// 
        /// See System.Drawing.Drawing2D.Blend for details
        /// </summary>
        public virtual float[] BlendFactors
        {
            get { return mBlendFactors; }
            set { mBlendFactors = value; }
        }
        protected virtual bool ShouldSerializeBlendFactors()
        {
            return BlendFactors != null;
        }

        private float[] mBlendPositions = null;
        /// <summary>
        /// Gradients are commonly used to smoothly shade the interiors of shapes. A blend pattern is defined by two arrays (Factors and Positions) that each contain the same number of elements. Each element of the Positions array represents a proportion of the distance along the gradient line. Each element of the Factors array represents the proportion of the starting and ending colors in the gradient blend at the position along the gradient line represented by the corresponding element in the Positions array.
        /// For example, if corresponding elements of the Positions and Factors arrays are 0.2 and 0.3, respectively, for a linear gradient from blue to red along a 100-pixel line, the color 20 pixels along that line (20 percent of the distance) consists of 30 percent blue and 70 percent red.
        ///
        /// See System.Drawing.Drawing2D.Blend for details
        /// </summary>
        public virtual float[] BlendPositions
        {
            get { return mBlendPositions; }
            set { mBlendPositions = value; }
        }
        protected virtual bool ShouldSerializeBlendPositions()
        {
            return BlendPositions != null;
        }

        #endregion

        protected override void OnDraw(GraphicsCache graphics, System.Drawing.RectangleF area)
        {
            if (FirstColor != Color.Empty || SecondColor != Color.Empty)
            {
                if (FirstColor == SecondColor)
                {
                    SolidBrush brush = graphics.BrushsCache.GetBrush(FirstColor);
                    graphics.Graphics.FillRectangle(brush, area);
                }
                else
                {
                    using (System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(area, FirstColor, SecondColor, Angle))
                    {
                        if (BlendFactors != null && BlendPositions != null &&
                            BlendFactors.Length == BlendPositions.Length)
                        {
                            System.Drawing.Drawing2D.Blend blend = new System.Drawing.Drawing2D.Blend();
                            blend.Factors = BlendFactors;
                            blend.Positions = BlendPositions;

                            brush.Blend = blend;
                        }

                        graphics.Graphics.FillRectangle(brush, area);
                    }
                }
            }
        }

        protected override SizeF OnMeasureContent(MeasureHelper measure, SizeF maxSize)
        {
            return SizeF.Empty;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new BackgroundLinearGradient(this);
        }
    }
}
