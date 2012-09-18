using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevAge.Drawing
{
    /// <summary>
    /// A class used to cache the Pens used by the drawing methods. Use the GetPen method to retrive or insert new pens.
    /// Remember to call Dispose when you don't need anymore this class to release all graphics resources.
    /// Usually you don't need to use this class directly but you can acces it using the GraphicsCache class.
    /// </summary>
    public class PensCache : IDisposable
    {
        private struct PenKey : IEquatable<PenKey>
        {
            public Color Color;
            public System.Drawing.Drawing2D.DashStyle Style;
            public float Width;

            #region IEquatable<PenKey> Members

            public bool Equals(PenKey other)
            {
                return other.Color == Color && other.Style == Style && other.Width == Width;
            }

            #endregion
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxCapacity">A positive number that specify the capacity of the cache.</param>
        public PensCache(int maxCapacity)
        {
            mList = new Pen[maxCapacity];
            mKeys = new PenKey[maxCapacity];
        }

        private Pen[] mList;
        private PenKey[] mKeys;
        private List<Pen> mNonCachedList = new List<Pen>();

        /// <summary>
        /// Returns the existing Pen object if already exist in the cache, otherwise create it and store in the cache.
        /// If there aren't anymore free slots (count > capacity) the this method create the pen but don't store it in the cache.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public Pen GetPen(Color color, float width, System.Drawing.Drawing2D.DashStyle style)
        {
            PenKey key;
            key.Color = color;
            key.Width = width;
            key.Style = style;

            for (int i = 0; i < mList.Length; i++)
            {
                if (mList[i] == null)
                {
                    Pen newPen = new Pen(color, width);
                    if (newPen.DashStyle != style)
                        newPen.DashStyle = style;

                    mList[i] = newPen;
                    mKeys[i] = key;

                    return newPen;
                }
                else if (mKeys[i].Equals(key))
                    return mList[i];
            }

            //The capacity is too small, so I must create a new pen
            Pen pen2 = new Pen(color, width);
            if (pen2.DashStyle != style)
                pen2.DashStyle = style;
            //I store the pen inside a list to dispose the resource at the end.
            mNonCachedList.Add(pen2);
            return pen2;
        }

        #region IDisposable Members
        /// <summary>
        /// Dipose. Release the graphics resources.
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < mList.Length; i++)
            {
                if (mList[i] != null)
                {
                    mList[i].Dispose();
                    mList[i] = null;
                }
            }

            foreach (Pen p in mNonCachedList)
            {
                p.Dispose();
            }
        }

        #endregion
    }
}
