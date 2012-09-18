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
//    /// Derived class must override the OnDrawBorder to draw the border.
//    /// </summary>
//    [Serializable]
//    public abstract class BackgroundBorderBase : BackgroundBase
//    {
//        #region Constructor

//        /// <summary>
//        /// Default constructor
//        /// </summary>
//        public BackgroundBorderBase()
//        {
//        }

//        /// <summary>
//        /// Copy constructor
//        /// </summary>
//        /// <param name="other"></param>
//        public BackgroundBorderBase(BackgroundBorderBase other)
//            : base(other)
//        {
//            if (other.Background != null)
//                Background = (IVisualElement)other.Background.Clone();
//            else
//                Background = null;
//        }
//        #endregion

//        #region Properties
//        private IVisualElement mBackground = null;

//        /// <summary>
//        /// The background used to draw
//        /// </summary>
//        public virtual IVisualElement Background
//        {
//            get { return mBackground; }
//            set { mBackground = value; }
//        }
//        protected virtual bool ShouldSerializeBackground()
//        {
//            return Background != null;
//        }
//        #endregion

//        public override RectangleF GetBackgroundContentRectangle(MeasureHelper measure, RectangleF backGroundArea)
//        {
//            backGroundArea = base.GetBackgroundContentRectangle(measure, backGroundArea);

//            if (Background is IBackground)
//                return ((IBackground)Background).GetBackgroundContentRectangle(measure, backGroundArea);
//            else
//                return backGroundArea;
//        }

//        public override SizeF GetBackgroundExtent(MeasureHelper measure, SizeF contentSize)
//        {
//            if (Background is IBackground)
//                contentSize = ((IBackground)Background).GetBackgroundExtent(measure, contentSize);

//            return base.GetBackgroundExtent(measure, contentSize);
//        }

//        protected override void OnDraw(GraphicsCache graphics, RectangleF area)
//        {
//            //The background is drawed on the background (back)
//            OnDrawBackground(graphics, area);

//            //The border is drawed on the background
//            OnDrawBorder(graphics, area);
//        }

//        protected abstract void OnDrawBorder(GraphicsCache graphics, RectangleF area);

//        protected virtual void OnDrawBackground(GraphicsCache graphics, RectangleF area)
//        {
//            if (Background != null)
//                Background.Draw(graphics, area);
//        }
//    }
//}
