//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.ComponentModel;
//using System.Drawing;

//namespace DevAge.Drawing.VisualElements
//{
//    /// <summary>
//    /// A background with a border.
//    /// Use the Background property to set an additional background
//    /// </summary>
//    [Serializable]
//    public class BackgroundBorder : BackgroundBorderBase
//    {
//        #region Constructor

//        /// <summary>
//        /// Default constructor
//        /// </summary>
//        public BackgroundBorder()
//        {
//        }

//        /// <summary>
//        /// Copy constructor
//        /// </summary>
//        /// <param name="other"></param>
//        public BackgroundBorder(BackgroundBorder other)
//            : base(other)
//        {
//            Border = other.Border;
//        }
//        #endregion

//        #region Properties
//        private RectangleBorder mBorder = RectangleBorder.NoBorder;

//        /// <summary>
//        /// Gets or sets the border to draw around the VisualElement. Default is RectangleBlack1Width.
//        /// </summary>
//        public virtual RectangleBorder Border
//        {
//            get { return mBorder; }
//            set { mBorder = value; }
//        }

//        protected virtual bool ShouldSerializeBorder()
//        {
//            return Border != RectangleBorder.NoBorder;
//        }
//        #endregion

//        /// <summary>
//        /// Calculate the client area where the content can be drawed, usually removing the area used by the background, for example removing a border.
//        /// </summary>
//        /// <param name="graphics"></param>
//        /// <param name="backGroundArea"></param>
//        /// <returns></returns>
//        public override RectangleF GetBackgroundContentRectangle(MeasureHelper measure, RectangleF backGroundArea)
//        {
//            backGroundArea = Border.RemoveBorderFromRectangle(backGroundArea);

//            return base.GetBackgroundContentRectangle(measure, backGroundArea);
//        }

//        /// <summary>
//        /// Calculate the total area used by the backgound and the content, adding the background area to the content area.
//        /// </summary>
//        /// <param name="graphics"></param>
//        /// <param name="contentSize"></param>
//        /// <returns></returns>
//        public override SizeF GetBackgroundExtent(MeasureHelper measure, SizeF contentSize)
//        {
//            contentSize = base.GetBackgroundExtent(measure, contentSize);

//            contentSize = Border.AddBorderToSize(contentSize);

//            return contentSize;
//        }

//        protected override void OnDrawBorder(GraphicsCache graphics, RectangleF area)
//        {
//            Border.DrawBorder(graphics, Rectangle.Round(area));
//        }

//        /// <summary>
//        /// Clone
//        /// </summary>
//        /// <returns></returns>
//        public override object Clone()
//        {
//            return new BackgroundBorder(this);
//        }
//    }
//}
