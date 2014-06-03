//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Bernardo Castilho
//
// Copyright 2009-2014 by Bernardo Castilho
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Ict.Common.Printing
{
    /// <summary>
    /// Specifies the zoom mode for the <see cref="CoolPrintPreviewControl"/> control.
    /// </summary>
    internal enum ZoomMode
    {
        /// <summary>
        /// Show the preview in actual size.
        /// </summary>
        ActualSize,
        /// <summary>
        /// Show a full page.
        /// </summary>
        FullPage,
        /// <summary>
        /// Show a full page width.
        /// </summary>
        PageWidth,
        /// <summary>
        /// Show two full pages.
        /// </summary>
        TwoPages,
        /// <summary>
        /// Use the zoom factor specified by the <see cref="CoolPrintPreviewControl.Zoom"/> property.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Represents a preview of one or two pages in a <see cref="PrintDocument"/>.
    /// </summary>
    /// <remarks>
    /// This control is similar to the standard <see cref="PrintPreviewControl"/> but
    /// it displays pages as they are rendered. By contrast, the standard control
    /// waits until the entire document is rendered before it displays anything.
    /// </remarks>
    class CoolPrintPreviewControl : UserControl
    {
        //-------------------------------------------------------------
        #region ** fields

        PrintDocument _doc;
        ZoomMode _zoomMode;
        double _zoom;
        int _startPage;
        Brush _backBrush;
        Point _ptLast;
        PointF _himm2pix = new PointF(-1, -1);
        PageImageList _img = new PageImageList();
        bool _cancel, _rendering;

        const int MARGIN = 4;

        #endregion

        //-------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of a <see cref="CoolPrintPreviewControl"/> control.
        /// </summary>
        public CoolPrintPreviewControl()
        {
            BackColor = SystemColors.AppWorkspace;
            ZoomMode = ZoomMode.FullPage;
            StartPage = 0;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        #endregion

        //-------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Gets or sets the <see cref="PrintDocument"/> being previewed.
        /// </summary>
        public PrintDocument Document
        {
            get
            {
                return _doc;
            }
            set
            {
                if (value != _doc)
                {
                    _doc = value;
                    RefreshPreview();
                }
            }
        }
        /// <summary>
        /// Regenerates the preview to reflect changes in the document layout.
        /// </summary>
        public void RefreshPreview()
        {
            // render into PrintController
            if (_doc != null)
            {
                // prepare to render preview document
                _img.Clear();
                PrintController savePC = _doc.PrintController;

                // render preview document
                try
                {
                    _cancel = false;
                    _rendering = true;
                    _doc.PrintController = new PreviewPrintController();
                    _doc.PrintPage += _doc_PrintPage;
                    _doc.EndPrint += _doc_EndPrint;
                    _doc.Print();
                }
                finally
                {
                    _cancel = false;
                    _rendering = false;
                    _doc.PrintPage -= _doc_PrintPage;
                    _doc.EndPrint -= _doc_EndPrint;
                    _doc.PrintController = savePC;
                }
            }

            // update
            OnPageCountChanged(EventArgs.Empty);
            UpdatePreview();
            UpdateScrollBars();
        }

        /// <summary>
        /// Stops rendering the <see cref="Document"/>.
        /// </summary>
        public void Cancel()
        {
            _cancel = true;
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="Document"/> is being rendered.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsRendering
        {
            get
            {
                return _rendering;
            }
        }
        /// <summary>
        /// Gets or sets how the zoom should be adjusted when the control is resized.
        /// </summary>
        [DefaultValue(ZoomMode.FullPage)]
        public ZoomMode ZoomMode
        {
            get
            {
                return _zoomMode;
            }
            set
            {
                if (value != _zoomMode)
                {
                    _zoomMode = value;
                    UpdateScrollBars();
                    OnZoomModeChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Gets or sets a custom zoom factor used when the <see cref="ZoomMode"/> property
        /// is set to <b>Custom</b>.
        /// </summary>
        [
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public double Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                if ((value != _zoom) || (ZoomMode != ZoomMode.Custom))
                {
                    ZoomMode = ZoomMode.Custom;
                    _zoom = value;
                    UpdateScrollBars();
                    OnZoomModeChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Gets or sets the first page being previewed.
        /// </summary>
        /// <remarks>
        /// There may be one or two pages visible depending on the setting of the
        /// <see cref="ZoomMode"/> property.
        /// </remarks>
        [
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public int StartPage
        {
            get
            {
                return _startPage;
            }
            set
            {
                // validate new setting
                if (value > PageCount - 1)
                {
                    value = PageCount - 1;
                }

                if (value < 0)
                {
                    value = 0;
                }

                // apply new setting
                if (value != _startPage)
                {
                    _startPage = value;
                    UpdateScrollBars();
                    OnStartPageChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Gets the number of pages available for preview.
        /// </summary>
        /// <remarks>
        /// This number increases as the document is rendered into the control.
        /// </remarks>
        [
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public int PageCount
        {
            get
            {
                return _img.Count;
            }
        }
        /// <summary>
        /// Gets or sets the control's background color.
        /// </summary>
        [DefaultValue(typeof(Color), "AppWorkspace")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                _backBrush = new SolidBrush(value);
            }
        }
        /// <summary>
        /// Gets a list containing the images of the pages in the document.
        /// </summary>
        [Browsable(false)]
        public PageImageList PageImages
        {
            get
            {
                return _img;
            }
        }
        /// <summary>
        /// Prints the current document honoring the selected page range.
        /// </summary>
        public void Print()
        {
            // select pages to print
            var ps = _doc.PrinterSettings;
            int first = ps.MinimumPage - 1;
            int last = ps.MaximumPage - 1;

            switch (ps.PrintRange)
            {
                case PrintRange.AllPages:
                    Document.Print();
                    return;

                case PrintRange.CurrentPage:
                    first = last = StartPage;
                    break;

                case PrintRange.Selection:
                    first = last = StartPage;

                    if (ZoomMode == ZoomMode.TwoPages)
                    {
                        last = Math.Min(first + 1, PageCount - 1);
                    }

                    break;

                case PrintRange.SomePages:
                    first = ps.FromPage - 1;
                    last = ps.ToPage - 1;
                    break;
            }

            // print using helper class
            var dp = new DocumentPrinter(this, first, last);
            dp.Print();
        }

        #endregion

        //-------------------------------------------------------------
        #region ** events

        /// <summary>
        /// Occurs when the value of the <see cref="StartPage"/> property changes.
        /// </summary>
        public event EventHandler StartPageChanged;
        /// <summary>
        /// Raises the <see cref="StartPageChanged"/> event.
        /// </summary>
        /// <param name="e"><see cref="EventArgs"/> that provides the event data.</param>
        protected void OnStartPageChanged(EventArgs e)
        {
            if (StartPageChanged != null)
            {
                StartPageChanged(this, e);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="PageCount"/> property changes.
        /// </summary>
        public event EventHandler PageCountChanged;
        /// <summary>
        /// Raises the <see cref="PageCountChanged"/> event.
        /// </summary>
        /// <param name="e"><see cref="EventArgs"/> that provides the event data/</param>
        protected void OnPageCountChanged(EventArgs e)
        {
            if (PageCountChanged != null)
            {
                PageCountChanged(this, e);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ZoomMode"/> property changes.
        /// </summary>
        public event EventHandler ZoomModeChanged;
        /// <summary>
        /// Raises the <see cref="ZoomModeChanged"/> event.
        /// </summary>
        /// <param name="e"><see cref="EventArgs"/> that contains the event data.</param>
        protected void OnZoomModeChanged(EventArgs e)
        {
            if (ZoomModeChanged != null)
            {
                ZoomModeChanged(this, e);
            }
        }

        #endregion

        //-------------------------------------------------------------
        #region ** overrides

        // painting
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // we're painting it all, so don't call base class
            //base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Image img = GetImage(StartPage);

            if (img != null)
            {
                Rectangle rc = GetImageRectangle(img);

                if ((rc.Width > 2) && (rc.Height > 2))
                {
                    // adjust for scrollbars
                    rc.Offset(AutoScrollPosition);

                    // render single page
                    if (_zoomMode != ZoomMode.TwoPages)
                    {
                        RenderPage(e.Graphics, img, rc);
                    }
                    else // render two pages
                    {
                        // render first page
                        rc.Width = (rc.Width - MARGIN) / 2;
                        RenderPage(e.Graphics, img, rc);

                        // render second page
                        img = GetImage(StartPage + 1);

                        if (img != null)
                        {
                            // update bounds in case orientation changed
                            rc = GetImageRectangle(img);
                            rc.Width = (rc.Width - MARGIN) / 2;

                            // render second page
                            rc.Offset(rc.Width + MARGIN, 0);
                            RenderPage(e.Graphics, img, rc);
                        }
                    }
                }
            }

            // paint background
            e.Graphics.FillRectangle(_backBrush, ClientRectangle);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateScrollBars();
            base.OnSizeChanged(e);
        }

        // pan by dragging preview pane
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if ((e.Button == MouseButtons.Left) && (AutoScrollMinSize != Size.Empty))
            {
                Cursor = Cursors.NoMove2D;
                _ptLast = new Point(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if ((e.Button == MouseButtons.Left) && (Cursor == Cursors.NoMove2D))
            {
                Cursor = Cursors.Default;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (Cursor == Cursors.NoMove2D)
            {
                int dx = e.X - _ptLast.X;
                int dy = e.Y - _ptLast.Y;

                if ((dx != 0) || (dy != 0))
                {
                    Point pt = AutoScrollPosition;
                    AutoScrollPosition = new Point(-(pt.X + dx), -(pt.Y + dy));
                    _ptLast = new Point(e.X, e.Y);
                }
            }
        }

        // keyboard support
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Home:
                case Keys.End:
                    return true;
            }

            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Handled)
            {
                return;
            }

            switch (e.KeyCode)
            {
                // arrow keys scroll or browse, depending on ZoomMode
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:

                    // browse
                    if ((ZoomMode == ZoomMode.FullPage) || (ZoomMode == ZoomMode.TwoPages))
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.Left:
                            case Keys.Up:
                                StartPage--;
                                break;

                            case Keys.Right:
                            case Keys.Down:
                                StartPage++;
                                break;
                        }

                        break;
                    }

                    // scroll
                    Point pt = AutoScrollPosition;

                    switch (e.KeyCode)
                    {
                        case Keys.Left: pt.X += 20; break;

                        case Keys.Right: pt.X -= 20; break;

                        case Keys.Up: pt.Y += 20; break;

                        case Keys.Down: pt.Y -= 20; break;
                    }

                    AutoScrollPosition = new Point(-pt.X, -pt.Y);
                    break;

                // page up/down browse pages
                case Keys.PageUp:
                    StartPage--;
                    break;

                case Keys.PageDown:
                    StartPage++;
                    break;

                // home/end
                case Keys.Home:
                    AutoScrollPosition = Point.Empty;
                    StartPage = 0;
                    break;

                case Keys.End:
                    AutoScrollPosition = Point.Empty;
                    StartPage = PageCount - 1;
                    break;

                default:
                    return;
            }

            // if we got here, the event was handled
            e.Handled = true;
        }

        #endregion

        //-------------------------------------------------------------
        #region ** implementation

        void _doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            SyncPageImages(false);

            if (_cancel)
            {
                e.Cancel = true;
            }
        }

        void _doc_EndPrint(object sender, PrintEventArgs e)
        {
            SyncPageImages(true);
        }

        void SyncPageImages(bool lastPageReady)
        {
            var pv = (PreviewPrintController)_doc.PrintController;

            if (pv != null)
            {
                var pageInfo = pv.GetPreviewPageInfo();
                int count = lastPageReady ? pageInfo.Length : pageInfo.Length - 1;

                for (int i = _img.Count; i < count; i++)
                {
                    var img = pageInfo[i].Image;
                    _img.Add(img);

                    OnPageCountChanged(EventArgs.Empty);

                    if (StartPage < 0)
                    {
                        StartPage = 0;
                    }

                    if ((i == StartPage) || (i == StartPage + 1))
                    {
                        Refresh();
                    }

                    Application.DoEvents();
                }
            }
        }

        Image GetImage(int page)
        {
            return page > -1 && page < PageCount ? _img[page] : null;
        }

        Rectangle GetImageRectangle(Image img)
        {
            // start with regular image rectangle
            Size sz = GetImageSizeInPixels(img);
            Rectangle rc = new Rectangle(0, 0, sz.Width, sz.Height);

            // calculate zoom
            Rectangle rcCli = this.ClientRectangle;

            switch (_zoomMode)
            {
                case ZoomMode.ActualSize:
                    _zoom = 1;
                    break;

                case ZoomMode.TwoPages:
                    rc.Width *= 2; // << two pages side-by-side
                    goto case ZoomMode.FullPage;

                case ZoomMode.FullPage:
                    double zoomX = (rc.Width > 0) ? rcCli.Width / (double)rc.Width : 0;
                    double zoomY = (rc.Height > 0) ? rcCli.Height / (double)rc.Height : 0;
                    _zoom = Math.Min(zoomX, zoomY);
                    break;

                case ZoomMode.PageWidth:
                    _zoom = (rc.Width > 0) ? rcCli.Width / (double)rc.Width : 0;
                    break;
            }

            // apply zoom factor
            rc.Width = (int)(rc.Width * _zoom);
            rc.Height = (int)(rc.Height * _zoom);

            // center image
            int dx = (rcCli.Width - rc.Width) / 2;

            if (dx > 0)
            {
                rc.X += dx;
            }

            int dy = (rcCli.Height - rc.Height) / 2;

            if (dy > 0)
            {
                rc.Y += dy;
            }

            // add some extra space
            rc.Inflate(-MARGIN, -MARGIN);

            if (_zoomMode == ZoomMode.TwoPages)
            {
                rc.Inflate(-MARGIN / 2, 0);
            }

            // done
            return rc;
        }

        Size GetImageSizeInPixels(Image img)
        {
            // get image size
            SizeF szf = img.PhysicalDimension;

            // if it is a metafile, convert to pixels
            if (img is Metafile)
            {
                // get screen resolution
                if (_himm2pix.X < 0)
                {
                    using (Graphics g = this.CreateGraphics())
                    {
                        _himm2pix.X = g.DpiX / 2540f;
                        _himm2pix.Y = g.DpiY / 2540f;
                    }
                }

                // convert himetric to pixels
                szf.Width *= _himm2pix.X;
                szf.Height *= _himm2pix.Y;
            }

            // done
            return Size.Truncate(szf);
        }

        void RenderPage(Graphics g, Image img, Rectangle rc)
        {
            // draw the page
            rc.Offset(1, 1);
            g.DrawRectangle(Pens.Black, rc);
            rc.Offset(-1, -1);
            g.FillRectangle(Brushes.White, rc);
            g.DrawImage(img, rc);
            g.DrawRectangle(Pens.Black, rc);

            // exclude cliprect to paint background later
            rc.Width++;
            rc.Height++;
            g.ExcludeClip(rc);
            rc.Offset(1, 1);
            g.ExcludeClip(rc);
        }

        void UpdateScrollBars()
        {
            // get image rectangle to adjust scroll size
            Rectangle rc = Rectangle.Empty;
            Image img = this.GetImage(StartPage);

            if (img != null)
            {
                rc = GetImageRectangle(img);
            }

            // calculate new scroll size
            Size scrollSize = new Size(0, 0);

            switch (_zoomMode)
            {
                case ZoomMode.PageWidth:
                    scrollSize = new Size(0, rc.Height + 2 * MARGIN);
                    break;

                case ZoomMode.ActualSize:
                case ZoomMode.Custom:
                    scrollSize = new Size(rc.Width + 2 * MARGIN, rc.Height + 2 * MARGIN);
                    break;
            }

            // apply if needed
            if (scrollSize != AutoScrollMinSize)
            {
                AutoScrollMinSize = scrollSize;
            }

            // ready to update
            UpdatePreview();
        }

        void UpdatePreview()
        {
            // validate current page
            if (_startPage < 0)
            {
                _startPage = 0;
            }

            if (_startPage > PageCount - 1)
            {
                _startPage = PageCount - 1;
            }

            // repaint
            Invalidate();
        }

        #endregion

        //-------------------------------------------------------------
        #region ** nested class

        // helper class that prints the selected page range in a PrintDocument.
        internal class DocumentPrinter : PrintDocument
        {
            int _first, _last, _index;
            PageImageList _imgList;

            public DocumentPrinter(CoolPrintPreviewControl preview, int first, int last)
            {
                // save page range and image list
                _first = first;
                _last = last;
                _imgList = preview.PageImages;

                // copy page and printer settings from original document
                DefaultPageSettings = preview.Document.DefaultPageSettings;
                PrinterSettings = preview.Document.PrinterSettings;
            }

            protected override void OnBeginPrint(PrintEventArgs e)
            {
                // start from the first page
                _index = _first;
            }

            protected override void OnPrintPage(PrintPageEventArgs e)
            {
                // render the current page and increment the index
                e.Graphics.PageUnit = GraphicsUnit.Display;
                e.Graphics.DrawImage(_imgList[_index++], e.PageBounds);

                // stop when we reach the last page in the range
                e.HasMorePages = _index <= _last;
            }
        }

        #endregion
    }
}