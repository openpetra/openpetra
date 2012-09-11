using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevAge.Drawing.VisualElements
{
    /// <summary>
    /// Interface for all the VisualElements classes. Inherits from IClonable.
    /// Support a deep clone.
    /// </summary>
    public interface IVisualElement : ICloneable
    {
        /// <summary>
        /// Gets or sets the area where the content must be drawed.
        /// You can set to align the content to the left, right, top or bottom using the relative properties (Left, Right, Top, Bottom).
        /// You can also set more than one properties to allign the content to more than one side.
        /// </summary>
        AnchorArea AnchorArea
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the area where the visual element will be drawed.
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        RectangleF GetDrawingArea(MeasureHelper measure, System.Drawing.RectangleF area);

        /// <summary>
        /// Measure the current VisualElement using the specified settings.
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="minSize">If Empty is not used.</param>
        /// <param name="maxSize">If Empty is not used.</param>
        /// <returns></returns>
        System.Drawing.SizeF Measure(MeasureHelper measure, System.Drawing.SizeF minSize, System.Drawing.SizeF maxSize);

        /// <summary>
        /// Draw the current VisualElement in the specified Graphics object.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="area"></param>
        void Draw(GraphicsCache graphics, System.Drawing.RectangleF area);

        /// <summary>
        /// Draw the current VisualElement in the specified Graphics object.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="area"></param>
        /// <param name="drawingArea"></param>
        void Draw(GraphicsCache graphics, System.Drawing.RectangleF area, out System.Drawing.RectangleF drawingArea);

        /// <summary>
        /// Get the element at the specified point. Usually this methods simply return the current element, but an element can return inner elements drawed inside the main elements.
        /// Returns a list of elements, where the last element is the upper element and the first element is the background element.
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="area"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        VisualElementList GetElementsAtPoint(MeasureHelper measure, System.Drawing.RectangleF area, PointF point);
    }

    /// <summary>
    /// VisualElement abstract base class. 
    /// You must override the OnDraw, OnMeasureContent and Clone.
    /// </summary>
    [Serializable]
    public abstract class VisualElementBase : IVisualElement
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        protected VisualElementBase()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public VisualElementBase(VisualElementBase other)
        {
            if (other.AnchorArea != null)
                AnchorArea = (AnchorArea)other.AnchorArea.Clone();
            else
                AnchorArea = null;
        }
        #endregion

        #region Properties
        private AnchorArea mAnchorArea = null;
        /// <summary>
        /// Gets or sets the area where the content must be drawed.
        /// You can set to align the content to the left, right, top or bottom using the relative properties (Left, Right, Top, Bottom).
        /// You can also set more than one properties to allign the content to more than one side.
        /// </summary>
        public virtual AnchorArea AnchorArea
        {
            get { return mAnchorArea; }
            set { mAnchorArea = value; }
        }
        protected virtual bool ShouldSerializeAnchorArea()
        {
            return AnchorArea != null && AnchorArea.IsEmpty == false;
        }
        #endregion

        #region Measure
        /// <summary>
        /// Measure the current VisualElement using the specified settings.
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="minSize">If Empty is not used.</param>
        /// <param name="maxSize">If Empty is not used.</param>
        /// <returns></returns>
        public System.Drawing.SizeF Measure(MeasureHelper measure, System.Drawing.SizeF minSize, System.Drawing.SizeF maxSize)
        {
            SizeF measureSize = OnMeasureContent(measure, maxSize);

            return Utilities.CheckMeasure(measureSize, minSize, maxSize);
        }

        /// <summary>
        /// Measure the current content of the VisualElement.
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="maxSize">If empty is not used.</param>
        /// <returns></returns>
        protected abstract System.Drawing.SizeF OnMeasureContent(MeasureHelper measure, System.Drawing.SizeF maxSize);
        #endregion

        #region Draw

        /// <summary>
        /// Draw the current VisualElement in the specified Graphics object.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="area"></param>
        /// <param name="drawingArea"></param>
        public void Draw(GraphicsCache graphics, System.Drawing.RectangleF area, out System.Drawing.RectangleF drawingArea)
        {
            using (MeasureHelper measure = new MeasureHelper(graphics))
            {
                drawingArea = GetDrawingArea(measure, area);

                OnDraw(graphics, drawingArea);
            }
        }

        /// <summary>
        /// Draw the current VisualElement in the specified Graphics object.
        /// Usually derived class don't override this method but the OnDraw method that automatically check the area to draw.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="area"></param>
        public void Draw(GraphicsCache graphics, System.Drawing.RectangleF area)
        {
            RectangleF drawArea;
            Draw(graphics, area, out drawArea);
        }

        /// <summary>
        /// Gets the area where the visual element will be drawed.
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public RectangleF GetDrawingArea(MeasureHelper measure, System.Drawing.RectangleF area)
        {
            RectangleF destContentArea = area;

            if (AnchorArea != null && !AnchorArea.IsEmpty)
            {
                SizeF measureSize = Measure(measure, SizeF.Empty, SizeF.Empty);
                destContentArea = AnchorArea.CalculateArea(area, measureSize, AnchorArea);
            }

            return destContentArea;
        }

        /// <summary>
        /// Method used to draw the specified content based on the Visual element. 
        /// This is the method that you must override to draw. The area used is already calculated based on the AnchorArea property.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="area"></param>
        protected abstract void OnDraw(GraphicsCache graphics, System.Drawing.RectangleF area);
        #endregion

        #region Utilities
        /// <summary>
        /// Get the element at the specified point. Usually this methods simply return the current element, but an element can return inner elements drawed inside the main elements.
        /// Returns a list of elements, where the last element is the upper element and the first element is the background element.
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="area"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual VisualElementList GetElementsAtPoint(MeasureHelper measure, System.Drawing.RectangleF area, PointF point)
        {
            VisualElementList list = new VisualElementList();

            if (GetDrawingArea(measure, area).Contains(point))
                list.Add(this);

            return list;
        }
        #endregion

        #region IClonable
        /// <summary>
        /// Clone the current VisualElement.
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();
        #endregion
    }
}
