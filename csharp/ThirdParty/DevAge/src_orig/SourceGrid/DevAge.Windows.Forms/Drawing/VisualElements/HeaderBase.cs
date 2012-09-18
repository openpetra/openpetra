using System;
using System.Collections.Generic;
using System.Text;

namespace DevAge.Drawing.VisualElements
{
    public interface IHeader : IBackground
    {
        ControlDrawStyle Style
        {
            get;
            set;
        }
    }

    [Serializable]
    public abstract class HeaderBase : BackgroundBase, IHeader
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public HeaderBase()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public HeaderBase(HeaderBase other)
            : base(other)
        {
            Style = other.Style;
        }
        #endregion

        #region Properties
        private ControlDrawStyle mControlDrawStyle = ControlDrawStyle.Normal;
        public virtual ControlDrawStyle Style
        {
            get { return mControlDrawStyle; }
            set { mControlDrawStyle = value; }
        }
        protected virtual bool ShouldSerializeStyle()
        {
            return Style != ControlDrawStyle.Normal;
        }
        #endregion
    }
}
