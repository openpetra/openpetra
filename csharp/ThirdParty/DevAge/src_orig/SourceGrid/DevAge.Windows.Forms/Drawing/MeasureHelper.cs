using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace DevAge.Drawing
{
	/// <summary>
	/// A class to help measure string operations. Remember to call the Dispose method on this class.
	/// </summary>
	public class MeasureHelper : IDisposable
	{
        /// <summary>
        /// Create a graphic object from the current control
        /// </summary>
        /// <param name="control"></param>
        public MeasureHelper(System.Windows.Forms.Control control)
        {
            mGraphics = control.CreateGraphics();
            mDisposeObj = true;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="graphics">This object is not automatically disposed.</param>
		public MeasureHelper(Graphics graphics)
		{
            mGraphics = graphics;
			mDisposeObj = false;
		}

        public MeasureHelper(GraphicsCache graphics)
        {
            mGraphics = graphics.Graphics;
            mDisposeObj = false;
        }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose()
		{
			if (mGraphics != null && mDisposeObj)
			{
                mGraphics.Dispose();
                mGraphics = null;
			}
			if (bitmap != null)
			{
				bitmap.Dispose();
				bitmap = null;
			}
		}

		private System.Drawing.Bitmap bitmap;
		private System.Drawing.Graphics mGraphics;
		private bool mDisposeObj;

        public System.Drawing.Graphics Graphics
        {
            get { return mGraphics; }
        }
	}
}
