using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DevAge.Drawing.VisualElements
{
    public interface IContainer : IVisualElement
    {
        /*
        IBorder Border
        {
            get;
            set;
        }
        Padding Padding
        {
            get;
            set;
        }
        IVisualElement Background
        {
            get;
            set;
        }
        ElementsDrawMode ElementsDrawMode
        {
            get;
            set;
        }
        IEnumerable<IVisualElement> GetElements();

         */
 
        RectangleF GetContentRectangle(MeasureHelper measure, RectangleF backGroundArea);

        SizeF GetExtent(MeasureHelper measure, SizeF contentSize);

    }

    /// <summary>
    /// The base abstract class for all container VisualElements. 
    /// Override the GetElements() to get a collection of elements to draw inside the container.
    /// Use the Border, Padding and Background properties to customize the container
    /// </summary>
    [Serializable]
    public abstract class ContainerBase : VisualElementBase, IContainer
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ContainerBase()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public ContainerBase(ContainerBase other)
            : base(other)
        {
            if (other.Background != null)
                Background = (IVisualElement)other.Background.Clone();
            else
                Background = null;

            ElementsDrawMode = other.ElementsDrawMode;
            Padding = other.Padding;
            if (other.Border != null)
                Border = (IBorder)other.Border.Clone();
            else
                Border = null;
        }
        #endregion

        #region Properties
        protected abstract IEnumerable<IVisualElement> GetElements();

        private ElementsDrawMode mDrawMode = ElementsDrawMode.Covering;

        /// <summary>
        /// Gets or sets a property that specify how to draw the elements.
        /// </summary>
        protected ElementsDrawMode ElementsDrawMode
        {
            get { return mDrawMode; }
            set { mDrawMode = value; }
        }
        protected virtual bool ShouldSerializeElementsDrawMode()
        {
            return ElementsDrawMode != ElementsDrawMode.Covering;
        }

        private IVisualElement mBackground = null;

        /// <summary>
        /// The background used to draw
        /// </summary>
        protected IVisualElement Background
        {
            get { return mBackground; }
            set { mBackground = value; }
        }
        protected virtual bool ShouldSerializeBackground()
        {
            return Background != null;
        }

        private Padding mPadding = Padding.Empty;
        protected Padding Padding
        {
            get { return mPadding; }
            set { mPadding = value; }
        }
        protected virtual bool ShouldSerializePadding()
        {
            return Padding != Padding.Empty;
        }

        private IBorder mBorder = null;
        protected IBorder Border
        {
            get { return mBorder; }
            set { mBorder = value; }
        }
        protected virtual bool ShouldSerializeBorder()
        {
            return Border != null;
        }
        #endregion

        /// <summary>
        /// Calculate the client area where the content can be drawed, usually removing the area used by the background, for example removing a border.
        /// </summary>
        /// <returns></returns>
        public RectangleF GetContentRectangle(MeasureHelper measure, RectangleF backGroundArea)
        {
            if (Border != null)
            {
                backGroundArea = Border.GetContentRectangle(backGroundArea);
            }

            if (Padding.IsEmpty == false)
            {
                backGroundArea = Padding.GetContentRectangle(backGroundArea);
            }

            if (Background is IBackground)
            {
                backGroundArea = ((IBackground)Background).GetBackgroundContentRectangle(measure, backGroundArea);
            }

            return backGroundArea;
        }

        /// <summary>
        /// Calculate the total area used by the backgound and the content, adding the background area to the content area.
        /// </summary>
        /// <returns></returns>
        public SizeF GetExtent(MeasureHelper measure, SizeF contentSize)
        {
            if (Background is IBackground)
            {
                contentSize = ((IBackground)Background).GetBackgroundExtent(measure, contentSize);
            }

            if (Padding.IsEmpty == false)
            {
                contentSize = Padding.GetExtent(contentSize);
            }

            if (Border != null)
            {
                contentSize = Border.GetExtent(contentSize);
            }

            return contentSize;
        }

        protected override void OnDraw(GraphicsCache graphics, RectangleF area)
        {
            OnDrawBackground(graphics, area);

            using (MeasureHelper measure = new MeasureHelper(graphics))
            {
                RectangleF contentArea = GetContentRectangle(measure, area);
                OnDrawContent(graphics, contentArea);
            }
        }

        protected virtual void OnDrawBackground(GraphicsCache graphics, RectangleF area)
        {
            if (Border != null)
            {
                Border.Draw(graphics, area);

                area = Border.GetContentRectangle(area);
            }

            if (Background != null)
            {
                Background.Draw(graphics, area);
            }
        }

        protected virtual void OnDrawContent(GraphicsCache graphics, RectangleF area)
        {
            //In this case the elements are drawed one over the another
            if (ElementsDrawMode == ElementsDrawMode.Covering)
            {
                foreach (IVisualElement element in GetElements())
                    element.Draw(graphics, area);
            }
            //In this case the elements are drawed considering an alignment
            else if (ElementsDrawMode == ElementsDrawMode.Align)
            {
                using (MeasureHelper measure = new MeasureHelper(graphics))
                {
                    foreach (IVisualElement element in GetElements())
                    {
                        RectangleF elementArea;
                        element.Draw(graphics, area, out elementArea);

                        area = CalculateRemainingArea(area, element.AnchorArea, elementArea);
                    }
                }
            }
            else
                throw new ApplicationException("DrawMode not supported");
        }

        protected override SizeF OnMeasureContent(MeasureHelper measure, SizeF maxSize)
        {
            SizeF clienSize = new SizeF(0, 0);

            //In this case the elements are drawed one over the another so to measure I must simply take the greatest
            if (ElementsDrawMode == ElementsDrawMode.Covering)
            {
                foreach (IVisualElement element in GetElements())
                {
                    SizeF elementSize = element.Measure(measure, Size.Empty, maxSize);

                    elementSize = CalculateSizeWithAnchor(elementSize, element.AnchorArea, maxSize);

                    if (elementSize.Width > clienSize.Width)
                        clienSize.Width = elementSize.Width;
                    if (elementSize.Height > clienSize.Height)
                        clienSize.Height = elementSize.Height;
                }
            }
            //In this case the elements are drawed considering an alignment, so to measure I must consider the anchor area of the element and add the size to the if the alignment is set. This code reflect the drawing code
            else if (ElementsDrawMode == ElementsDrawMode.Align)
            {
                AnchorArea previousAnchor = null;
                foreach (IVisualElement element in GetElements())
                {
                    SizeF elementSize = element.Measure(measure, Size.Empty, maxSize);

                    elementSize = CalculateSizeWithAnchor(elementSize, element.AnchorArea, maxSize);

                    clienSize = CalculateSizeWithContent(clienSize, previousAnchor, elementSize);

                    previousAnchor = element.AnchorArea;
                }
            }
            else
                throw new ApplicationException("DrawMode not supported");

            return GetExtent(measure, clienSize);
        }

        /// <summary>
        /// Get the element at the specified point. Usually this methods simply return the current element, but an element can return inner elements drawed inside the main elements.
        /// Returns a list of elements, where the last element is the upper (foremost) element and the first element is the background element.
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="area"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public override VisualElementList GetElementsAtPoint(MeasureHelper measure, System.Drawing.RectangleF area, PointF point)
        {
            VisualElementList list = base.GetElementsAtPoint(measure, area, point);

            area = GetContentRectangle(measure, area);

            //Use the same code of the OnDrawContent method
            //In this case the elements are drawed one over the another
            if (ElementsDrawMode == ElementsDrawMode.Covering)
            {
                foreach (IVisualElement element in GetElements())
                {
                    list.AddRange(element.GetElementsAtPoint(measure, area, point));
                }
            }
            //In this case the elements are drawed considering an alignment
            else if (ElementsDrawMode == ElementsDrawMode.Align)
            {
                foreach (IVisualElement element in GetElements())
                {
                    list.AddRange(element.GetElementsAtPoint(measure, area, point));

                    RectangleF elementArea = element.GetDrawingArea(measure, area);
                    area = CalculateRemainingArea(area, element.AnchorArea, elementArea);
                }
            }
            else
                throw new ApplicationException("DrawMode not supported");

            return list;
        }

        #region Helper methods
        /// <summary>
        /// Utility function. Calculates the remaining empty area of a specified area and a given content.
        /// </summary>
        /// <returns></returns>
        public static RectangleF CalculateRemainingArea(RectangleF parentArea, AnchorArea contentAnchor, RectangleF contentArea)
        {
            if (contentAnchor != null)
            {
                float left, top, height, width;

                if (contentAnchor.HasTop && !contentAnchor.HasBottom)
                {
                    top = contentArea.Bottom;
                    height = parentArea.Height - contentArea.Height;
                }
                else if (contentAnchor.HasBottom && !contentAnchor.HasTop)
                {
                    top = parentArea.Top;
                    height = parentArea.Height - contentArea.Height;
                }
                else
                {
                    top = parentArea.Top;
                    height = parentArea.Height;
                }

                if (contentAnchor.HasLeft && !contentAnchor.HasRight)
                {
                    left = contentArea.Right;
                    width = parentArea.Width - contentArea.Width;
                }
                else if (contentAnchor.HasRight && !contentAnchor.HasLeft)
                {
                    left = parentArea.Left;
                    width = parentArea.Width - contentArea.Width;
                }
                else
                {
                    left = parentArea.Left;
                    width = parentArea.Width;
                }

                RectangleF newArea = new RectangleF(left, top, width, height);
                newArea.Intersect(parentArea);

                return newArea;
            }
            else
                return parentArea;
        }

        /// <summary>
        /// Utility function. Calculate the required size adding using the anchor informations of the available area.
        /// </summary>
        /// <param name="currentSize"></param>
        /// <param name="contentAnchor"></param>
        /// <param name="contentSize"></param>
        /// <returns></returns>
        public static SizeF CalculateSizeWithContent(SizeF currentSize, AnchorArea contentAnchor, SizeF contentSize)
        {
            if (contentAnchor != null && (contentAnchor.HasLeft || contentAnchor.HasRight))
                currentSize.Width += contentSize.Width;
            else
            {
                if (contentSize.Width > currentSize.Width)
                    currentSize.Width = contentSize.Width;
            }

            if (contentAnchor != null && (contentAnchor.HasTop || contentAnchor.HasBottom))
                currentSize.Height += contentSize.Height;
            else
            {
                if (contentSize.Height > currentSize.Height)
                    currentSize.Height = contentSize.Height;
            }

            return currentSize;
        }

        /// <summary>
        /// Add the anchor informations to the element content size
        /// </summary>
        /// <returns></returns>
        public static SizeF CalculateSizeWithAnchor(SizeF contentSize, AnchorArea contentAnchor, SizeF clientSize)
        {
            if (contentAnchor == null)
                return contentSize;

            if (contentAnchor.HasLeft && contentAnchor.HasRight)
            {
                if (clientSize.IsEmpty == false)
                    contentSize.Width = clientSize.Width - (contentAnchor.Left + contentAnchor.Right);
            }
            else if (contentAnchor.HasLeft)
                contentSize.Width += contentAnchor.Left;
            else if (contentAnchor.HasRight)
                contentSize.Width += contentAnchor.Right;

            if (contentSize.Width < 0)
                contentSize.Width = 0;

            if (contentAnchor.HasTop && contentAnchor.HasBottom)
            {
                if (clientSize.IsEmpty == false)
                    contentSize.Height = clientSize.Height - (contentAnchor.Top + contentAnchor.Bottom);
            }
            else if (contentAnchor.HasTop)
                contentSize.Height += contentAnchor.Top;
            else if (contentAnchor.HasBottom)
                contentSize.Height += contentAnchor.Bottom;

            if (contentSize.Height < 0)
                contentSize.Height = 0;

            return contentSize;
        }
        #endregion
    }
}
