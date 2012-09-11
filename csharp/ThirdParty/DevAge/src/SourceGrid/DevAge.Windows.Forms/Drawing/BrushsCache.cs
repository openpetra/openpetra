using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevAge.Drawing
{
    /// <summary>
    /// A class used to cache the Brush used by the drawing methods. Use the GetBrush method to retrive or insert new brushes.
    /// Remember to call Dispose when you don't need anymore this class to release all graphics resources.
    /// Usually you don't need to use this class directly but you can acces it using the GraphicsCache class.
    /// </summary>
    public class BrushsCache : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxCapacity">A positive number that specify the capacity of the cache.</param>
        public BrushsCache(int maxCapacity)
        {
            mList = new SolidBrush[maxCapacity];
            mKeys = new Color[maxCapacity];
        }

        private SolidBrush[] mList;
        private Color[] mKeys;
        private List<SolidBrush> mNonCachedList = new List<SolidBrush>();


        /// <summary>
        /// Returns the existing Brush object if already exist in the cache, otherwise create it and store in the cache.
        /// If there aren't anymore free slots (count > capacity) the this method create the pen but don't store it in the cache.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public SolidBrush GetBrush(Color color)
        {
            for (int i = 0; i < mList.Length; i++)
            {
                if (mList[i] == null)
                {
                    SolidBrush newBrush = new SolidBrush(color);

                    mList[i] = newBrush;
                    mKeys[i] = color;

                    return newBrush;
                }
                else if (mKeys[i].Equals(color))
                    return mList[i];
            }

            //The capacity is too small, so I must create a new pen
            SolidBrush brush2 = new SolidBrush(color);
            mNonCachedList.Add(brush2);
            return brush2;
        }

        #region IDisposable Members

        /// <summary>
        /// Dispose. Release the graphics resources.
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

            foreach (SolidBrush b in mNonCachedList)
            {
                b.Dispose();
            }
        }

        #endregion
    }
}
