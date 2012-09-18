using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevAge.Drawing.VisualElements
{
    public interface IRowHeader : IHeader
    {
    }

    [Serializable]
    public abstract class RowHeaderBase : HeaderBase, IRowHeader
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public RowHeaderBase()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public RowHeaderBase(RowHeaderBase other)
            : base(other)
        {
        }
        #endregion
    }
}
