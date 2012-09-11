using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevAge.Drawing.VisualElements
{
    public interface ISortIndicator : IVisualElement
    {
        HeaderSortStyle SortStyle
        {
            get;
            set;
        }
    }

    /// <summary>
    /// A class used to draw a generic sort indicator, usually a arrow. Use the SortStyle to customize the sort style (arrow up or arrow down)
    /// </summary>
    [Serializable]
    public class SortIndicator : Icon, ISortIndicator
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public SortIndicator()
        {
            AnchorArea = new AnchorArea(float.NaN, float.NaN, 0, float.NaN, false, true);
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public SortIndicator(SortIndicator other)
            : base(other)
        {
            SortStyle = other.SortStyle;
        }
        #endregion
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new SortIndicator(this);
        }

        #region Properties
        private HeaderSortStyle mHeaderSortStyle = HeaderSortStyle.None;

        [DefaultValue(HeaderSortStyle.None)]
        public virtual HeaderSortStyle SortStyle
        {
            get { return mHeaderSortStyle; }
            set
            {
                mHeaderSortStyle = value;
                if (mHeaderSortStyle == HeaderSortStyle.Ascending)
                    Value = Properties.Resources.ICO_SortUp;
                else if (mHeaderSortStyle == HeaderSortStyle.Descending)
                    Value = Properties.Resources.ICO_SortDown;
                else
                    Value = null;
            }
        }
        #endregion
    }
}
