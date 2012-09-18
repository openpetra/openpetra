using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevAge.Drawing.VisualElements
{
    [Serializable]
    public class RichTextGDI : RichText
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RichTextGDI()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public RichTextGDI(DevAge.Windows.Forms.RichText value)
            : base(value)
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public RichTextGDI(RichTextGDI other)
            : base(other)
        {
        }

        /// <summary>
        /// Init rich text box control
        /// </summary>
        protected virtual void AssertRichTextBoxEditor()
        {
            if (RichTextBoxEditor == null)
            {
                RichTextBoxEditor = new SourceGrid.Cells.Editors.RichTextBox();
            }

            RichTextBoxEditor.Control.Clear();

            RichTextBoxEditor.Control.Value = Value;
            if (ForeColor != Color.FromKnownColor(KnownColor.WindowText))
            {
                RichTextBoxEditor.Control.SelectAll();
                RichTextBoxEditor.Control.SelectionColor = ForeColor;
            }
            if (TextAlignment != ContentAlignment.MiddleLeft)
            {
                RichTextBoxEditor.Control.SelectAll();
                RichTextBoxEditor.Control.SelectionAlignment = DevAge.Windows.Forms.Utilities.ContentToHorizontalAlignment(TextAlignment);
            }
            if (Font != System.Windows.Forms.Control.DefaultFont)
            {
                RichTextBoxEditor.Control.SelectAll();
                RichTextBoxEditor.Control.SelectionFont = Font;
            }
        }

        #endregion

        #region Members

        /// <summary>
        /// Will be used to draw picture of rich text. Is static for performance reasons
        /// and therefore needs to be locked.
        /// </summary>
        private static SourceGrid.Cells.Editors.RichTextBox m_RichTextBoxEditor = null;
        public SourceGrid.Cells.Editors.RichTextBox RichTextBoxEditor
        {
            get { return m_RichTextBoxEditor; }
            set { m_RichTextBoxEditor = value; }
        }

        #endregion

        #region Win32Api Layout

        /// <summary>
        /// Convert between 1/100 inch (unit used by the .NET framework)
        /// and twips (1/1440 inch, used by Win32 API calls)
        /// </summary>
        /// <param name="n">Value in 1/100 inch</param>
        /// <returns>Value in twips</returns>
        private Int32 HundredthInchToTwips(float n)
        {
            return (Int32)(n * 14.4);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct STRUCT_RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct STRUCT_CHARRANGE
        {
            public int cpMin;         //First character of range (0 for start of doc)
            public int cpMax;         //Last character of range (-1 for end of doc)
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct STRUCT_FORMATRANGE
        {
            public IntPtr hdc;             //Actual DC to draw on
            public IntPtr hdcTarget;       //Target DC for determining text formatting
            public STRUCT_RECT rc;                //Region of the DC to draw to (in twips)
            public STRUCT_RECT rcPage;            //Region of the whole DC (page size) (in twips)
            public STRUCT_CHARRANGE chrg;         //Range of text to draw (see earlier declaration)
        }

        [DllImport("USER32.dll")]
        private static extern Int32 SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, IntPtr lParam);
        private const int WM_USER = 0x0400;
        private const int EM_FORMATRANGE = WM_USER + 57;

        /// <summary>
        /// Calculate or render the contents of our RichTextBox for printing
        /// </summary>
        /// <param name="measureOnly">If true, only the calculation is performed,
        /// otherwise the text is rendered as well</param>
        /// <param name="b"></param>
        /// <param name="rtb"></param>
        /// <param name="charFrom">Index of first character to be printed</param>
        /// <param name="charTo">Index of last character to be printed</param>
        /// <returns>(Index of last character that fitted on the
        /// page) + 1</returns>
        //public int FormatRange(bool measureOnly, PrintPageEventArgs e, int charFrom, int charTo)
        public int FormatRange(bool measureOnly, DevAge.Windows.Forms.DevAgeRichTextBox rtb,
            ref Bitmap b, int charFrom, int charTo)
        {
            // Specify which characters to print
            STRUCT_CHARRANGE cr;
            cr.cpMin = charFrom;
            cr.cpMax = charTo;

            // Specify the area inside page margins
            STRUCT_RECT rc;
            rc.Top = HundredthInchToTwips(0);
            rc.Bottom = HundredthInchToTwips(b.Height);
            rc.Left = HundredthInchToTwips(0);
            rc.Right = HundredthInchToTwips(b.Width);

            // Specify the page area
            STRUCT_RECT rcPage;
            rcPage.Top = HundredthInchToTwips(0);
            rcPage.Bottom = HundredthInchToTwips(b.Height);
            rcPage.Left = HundredthInchToTwips(0);
            rcPage.Right = HundredthInchToTwips(b.Width);

            // Get device context of output device
            Graphics g = Graphics.FromImage(b);
            IntPtr hdc = g.GetHdc();

            // Fill in the FORMATRANGE struct
            STRUCT_FORMATRANGE fr;
            fr.chrg = cr;
            fr.hdc = hdc;
            fr.hdcTarget = hdc;
            fr.rc = rc;
            fr.rcPage = rcPage;

            // Non-Zero wParam means render, Zero means measure
            Int32 wParam = (measureOnly ? 0 : 1);

            // Allocate memory for the FORMATRANGE struct and
            // copy the contents of our struct to this memory
            IntPtr lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fr));
            Marshal.StructureToPtr(fr, lParam, false);

            // Send the actual Win32 message
            int res = SendMessage(rtb.Handle, EM_FORMATRANGE, wParam, lParam);

            // Free allocated memory
            Marshal.FreeCoTaskMem(lParam);

            // and release the device context
            g.ReleaseHdc(hdc);

            g.Dispose();

            return res;
        }

        /// <summary>
        /// Free cached data from rich edit control after printing
        /// </summary>
        public void FormatRangeDone(DevAge.Windows.Forms.DevAgeRichTextBox RTB)
        {
            IntPtr lParam = new IntPtr(0);
            SendMessage(RTB.Handle, EM_FORMATRANGE, 0, lParam);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Helper method to get bitmap with size according its area and rotate flip type
        /// </summary>
        /// <param name="area"></param>
        /// <param name="rotateFlipType"></param>
        /// <returns></returns>
        protected Bitmap GetBitmapArea(RectangleF area, RotateFlipType rotateFlipType)
        {
            int height = (int)area.Height;
            int width = (int)area.Width;

            // when rotation is 90 (resp. 270 which is equivalent)
            // height and width values need to be swapped
            if (rotateFlipType == RotateFlipType.Rotate90FlipNone
                || rotateFlipType == RotateFlipType.Rotate90FlipX
                || rotateFlipType == RotateFlipType.Rotate90FlipY
                || rotateFlipType == RotateFlipType.Rotate90FlipXY)
            {
                height = width;
                width = (int)area.Height;
            }

            return new Bitmap(width, height);
        }

        /// <summary>
        /// Render RichTextBox as GDI
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="area"></param>
        protected override void OnDraw(GraphicsCache graphics, RectangleF area)
        {
            // Do not call base.OnDraw as it would overwrite our drawing
            //base.OnDraw(graphics, area);

            lock (this)
            {
                // create bitmap
                Bitmap bmp = null;
                if (area.Width > 0 && area.Height > 0)
                {
                    AssertRichTextBoxEditor();
                    bmp = GetBitmapArea(area, RotateFlipType);

                    // render image
                    FormatRange(false, RichTextBoxEditor.Control, ref bmp, 0,
                        RichTextBoxEditor.Control.Text.Length);
                    FormatRangeDone(RichTextBoxEditor.Control);
                }
                else
                {
                    // create empty picture, in case the area is empty
                    bmp = new Bitmap(1, 1);
                }

                DrawImage(graphics, area, bmp);
            }
        }

        /// <summary>
        /// Draw actual picture
        /// </summary>
        protected virtual void DrawImage(GraphicsCache graphics, RectangleF area, Bitmap bmp)
        {
            bmp.MakeTransparent(Color.White);
            bmp.RotateFlip(RotateFlipType);
            graphics.Graphics.DrawImage(bmp, area);
        }

        #endregion

        #region Measure

        /// <summary>
        /// Measure the current content of the VisualElement.
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="maxSize">If empty is not used.</param>
        /// <returns></returns>
        protected override SizeF OnMeasureContent(MeasureHelper measure, System.Drawing.SizeF maxSize)
        {
            String s = String.Empty;

            if (Value != null && Value.Rtf.Length > 0)
            {
                s = DevAge.Windows.Forms.RichTextConversion.RichTextToString(Value);
            }

            return measure.Graphics.MeasureString(s, Font, maxSize);
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new RichTextGDI(this);
        }

        #endregion
    }
}
