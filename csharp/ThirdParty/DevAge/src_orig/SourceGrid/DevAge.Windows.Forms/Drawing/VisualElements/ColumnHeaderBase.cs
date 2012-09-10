using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevAge.Drawing.VisualElements
{
    public interface IColumnHeader : IHeader
    {

    }

    [Serializable]
    public abstract class ColumnHeaderBase : HeaderBase, IColumnHeader
    {
        #region Constuctor
        /// <summary>
        /// Default constructor
        /// </summary>
        public ColumnHeaderBase()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public ColumnHeaderBase(ColumnHeaderBase other)
            : base(other)
        {
        }
        #endregion
    }
}
