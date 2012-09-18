using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DevAge.Drawing.VisualElements
{
    /// <summary>
    /// A container for other elements
    /// </summary>
    [Serializable]
    public class Container : ContainerBase
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Container()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public Container(Container other)
            : base(other)
        {
            if (other.Elements != null)
            {
                Elements = (DevAge.Drawing.VisualElements.VisualElementList)other.Elements.Clone();
            }
            else
            {
                Elements = null;
            }
        }
        #endregion

        #region Properties

        public new IBorder Border
        {
            get { return base.Border; }
            set { base.Border = value; }
        }
        public new Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }
        public new IVisualElement Background
        {
            get { return base.Background; }
            set { base.Background = value; }
        }
        public new ElementsDrawMode ElementsDrawMode
        {
            get { return base.ElementsDrawMode; }
            set { base.ElementsDrawMode = value; }
        }

        private VisualElementList mElements = new VisualElementList();

        public virtual VisualElementList Elements
        {
            get { return mElements; }
            set { mElements = value; }
        }
        #endregion

        protected override IEnumerable<IVisualElement> GetElements()
        {
            return Elements;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Container(this);
        }
    }
}
