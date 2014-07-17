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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

using Ict.Common;

namespace Ict.Common.Printing
{
    /// <summary>
    /// Represents a dialog containing a <see cref="CoolPrintPreviewControl"/> control
    /// used to preview and print <see cref="PrintDocument"/> objects.
    /// </summary>
    /// <remarks>
    /// This dialog is similar to the standard <see cref="PrintPreviewDialog"/>
    /// but provides additional options such printer and page setup buttons,
    /// a better UI based on the <see cref="ToolStrip"/> control, and built-in
    /// PDF export.
    /// </remarks>
    public partial class CoolPrintPreviewDialog : Form
    {
        //--------------------------------------------------------------------
        #region ** fields

        PrintDocument _doc;

        #endregion

        //--------------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of a <see cref="CoolPrintPreviewDialog"/>.
        /// </summary>
        public CoolPrintPreviewDialog() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="CoolPrintPreviewDialog"/>.
        /// </summary>
        /// <param name="parentForm">Parent form that defines the initial size for this dialog.</param>
        public CoolPrintPreviewDialog(Control parentForm)
        {
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this._toolStrip.Text = Catalog.GetString("toolStrip1");
            this._btnPrint.Text = Catalog.GetString("Print Document");
            this._btnPageSetup.Text = Catalog.GetString("Page Setup");
            this._btnZoom.Text = Catalog.GetString("&Zoom");
            this._itemActualSize.Text = Catalog.GetString("Actual Size");
            this._itemFullPage.Text = Catalog.GetString("Full Page");
            this._itemPageWidth.Text = Catalog.GetString("Page Width");
            this._itemTwoPages.Text = Catalog.GetString("Two Pages");
            this._item500.Text = Catalog.GetString("500%");
            this._item200.Text = Catalog.GetString("200%");
            this._item150.Text = Catalog.GetString("150%");
            this._item100.Text = Catalog.GetString("100%");
            this._item75.Text = Catalog.GetString("75%");
            this._item50.Text = Catalog.GetString("50%");
            this._item25.Text = Catalog.GetString("25%");
            this._item10.Text = Catalog.GetString("10%");
            this._btnFirst.Text = Catalog.GetString("First Page");
            this._btnPrev.Text = Catalog.GetString("Previous Page");
            this._btnNext.Text = Catalog.GetString("Next Page");
            this._btnLast.Text = Catalog.GetString("Last Page");
            this._btnCancel.Text = Catalog.GetString("Cancel");
            this.Text = Catalog.GetString("Print Preview");
            #endregion

            if (parentForm != null)
            {
                Size = parentForm.Size;
            }
        }

        #endregion

        //--------------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Gets or sets the <see cref="PrintDocument"/> to preview.
        /// </summary>
        public PrintDocument Document
        {
            get
            {
                return _doc;
            }
            set
            {
                // unhook event handlers
                if (_doc != null)
                {
                    _doc.BeginPrint -= _doc_BeginPrint;
                    _doc.EndPrint -= _doc_EndPrint;
                }

                // save the value
                _doc = value;

                // hook up event handlers
                if (_doc != null)
                {
                    _doc.BeginPrint += _doc_BeginPrint;
                    _doc.EndPrint += _doc_EndPrint;
                }

                // don't assign document to preview until this form becomes visible
                if (Visible)
                {
                    _preview.Document = Document;
                }
            }
        }

        #endregion

        //--------------------------------------------------------------------
        #region ** overloads

        /// <summary>
        /// Overridden to assign document to preview control only after the
        /// initial activation.
        /// </summary>
        /// <param name="e"><see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _preview.Document = Document;
        }

        /// <summary>
        /// Overridden to cancel any ongoing previews when closing form.
        /// </summary>
        /// <param name="e"><see cref="FormClosingEventArgs"/> that contains the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (_preview.IsRendering && !e.Cancel)
            {
                _preview.Cancel();
            }
        }

        #endregion

        //--------------------------------------------------------------------
        #region ** main commands

        void _btnPrint_Click(object sender, EventArgs e)
        {
            using (var dlg = new PrintDialog())
            {
                // configure dialog
                dlg.AllowSomePages = true;
                dlg.AllowSelection = true;
                dlg.UseEXDialog = true;
                dlg.Document = Document;

                // show allowed page range
                var ps = dlg.PrinterSettings;
                ps.MinimumPage = ps.FromPage = 1;
                ps.MaximumPage = ps.ToPage = _preview.PageCount;

                // show dialog
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // print selected page range
                    _preview.Print();
                }
            }
        }

        void _btnPageSetup_Click(object sender, EventArgs e)
        {
            using (var dlg = new PageSetupDialog())
            {
                dlg.Document = Document;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // to show new page layout
                    _preview.RefreshPreview();
                }
            }
        }

        #endregion

        //--------------------------------------------------------------------
        #region ** zoom

        void _btnZoom_ButtonClick(object sender, EventArgs e)
        {
            _preview.ZoomMode = _preview.ZoomMode == ZoomMode.ActualSize
                                ? ZoomMode.FullPage
                                : ZoomMode.ActualSize;
        }

        void _btnZoom_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == _itemActualSize)
            {
                _preview.ZoomMode = ZoomMode.ActualSize;
            }
            else if (e.ClickedItem == _itemFullPage)
            {
                _preview.ZoomMode = ZoomMode.FullPage;
            }
            else if (e.ClickedItem == _itemPageWidth)
            {
                _preview.ZoomMode = ZoomMode.PageWidth;
            }
            else if (e.ClickedItem == _itemTwoPages)
            {
                _preview.ZoomMode = ZoomMode.TwoPages;
            }

            if (e.ClickedItem == _item10)
            {
                _preview.Zoom = .1;
            }
            else if (e.ClickedItem == _item100)
            {
                _preview.Zoom = 1;
            }
            else if (e.ClickedItem == _item150)
            {
                _preview.Zoom = 1.5;
            }
            else if (e.ClickedItem == _item200)
            {
                _preview.Zoom = 2;
            }
            else if (e.ClickedItem == _item25)
            {
                _preview.Zoom = .25;
            }
            else if (e.ClickedItem == _item50)
            {
                _preview.Zoom = .5;
            }
            else if (e.ClickedItem == _item500)
            {
                _preview.Zoom = 5;
            }
            else if (e.ClickedItem == _item75)
            {
                _preview.Zoom = .75;
            }
        }

        #endregion

        //--------------------------------------------------------------------
        #region ** page navigation

        void _btnFirst_Click(object sender, EventArgs e)
        {
            _preview.StartPage = 0;
        }

        void _btnPrev_Click(object sender, EventArgs e)
        {
            _preview.StartPage--;
        }

        void _btnNext_Click(object sender, EventArgs e)
        {
            _preview.StartPage++;
        }

        void _btnLast_Click(object sender, EventArgs e)
        {
            _preview.StartPage = _preview.PageCount - 1;
        }

        void _txtStartPage_Enter(object sender, EventArgs e)
        {
            _txtStartPage.SelectAll();
        }

        void _txtStartPage_Validating(object sender, CancelEventArgs e)
        {
            CommitPageNumber();
        }

        void _txtStartPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            var c = e.KeyChar;

            if (c == (char)13)
            {
                CommitPageNumber();
                e.Handled = true;
            }
            else if ((c > ' ') && !char.IsDigit(c))
            {
                e.Handled = true;
            }
        }

        void CommitPageNumber()
        {
            int page;

            if (int.TryParse(_txtStartPage.Text, out page))
            {
                _preview.StartPage = page - 1;
            }
        }

        void _preview_StartPageChanged(object sender, EventArgs e)
        {
            var page = _preview.StartPage + 1;

            _txtStartPage.Text = page.ToString();
        }

        private void _preview_PageCountChanged(object sender, EventArgs e)
        {
            this.Update();
            Application.DoEvents();
            _lblPageCount.Text = string.Format("of {0}", _preview.PageCount);
        }

        #endregion

        //--------------------------------------------------------------------
        #region ** job control

        void _btnCancel_Click(object sender, EventArgs e)
        {
            if (_preview.IsRendering)
            {
                _preview.Cancel();
            }
            else
            {
                Close();
            }
        }

        void _doc_BeginPrint(object sender, PrintEventArgs e)
        {
            _btnCancel.Text = "&Cancel";
            _btnPrint.Enabled = _btnPageSetup.Enabled = false;
        }

        void _doc_EndPrint(object sender, PrintEventArgs e)
        {
            _btnCancel.Text = "&Close";
            _btnPrint.Enabled = _btnPageSetup.Enabled = true;
        }

        #endregion
    }
}
