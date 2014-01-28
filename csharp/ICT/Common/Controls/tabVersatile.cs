//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
//
// This file is part of OpenPetra.org.
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
//
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using Ict.Common;

using Ict.Common.Exceptions;

namespace Ict.Common.Controls
{
    /// <summary>Event handler declaration</summary>
    public delegate void TSelectedIndexChangingEventHandler(System.Object Sender, TSelectedIndexChangingEventArgs e);

    /// <summary>
    /// Event Arguments declaration
    /// </summary>
    public class TSelectedIndexChangingEventArgs : System.ComponentModel.CancelEventArgs
    {
        /// <summary>
        /// previously selected index
        /// </summary>
        public Int32 SelectedIndex;

        /// <summary>
        /// now this is selected
        /// </summary>
        public Int32 NewSelectedIndex;
    }

    /// <summary>
    /// TTabVersatile is an extension of System.Windows.Forms.TabControl that
    /// contains several extensions:
    /// drag'n'drop support for re-ordering of TabPages
    /// + can be cancelled by pressing ESC while dragging and by moving the
    /// mouse out of the area where the TabPage headers are shown.
    /// disabling of certain TabPages
    /// + TabPage header is OwnerDrawn to show 'disabled' text and 'disabled'
    /// icons.
    /// - done for all TabPages when the whole TabControl is disabled
    /// + Prevent selection of disabled TabPage
    /// - with Keyboard (CURSOR-LEFT, CURSOR-RIGHT, CURSOR-UP, CURSOR-DOWN,
    /// HOME, END, CTRL+TAB, CTRL+SHIFT+TAB)
    /// - with Mouse (WM_LBUTTONDOWN message is caught)
    /// disabling of all TabPages except the current one
    /// SelectedIndexChanging Event
    /// + tells SelectedIndex of the current page and SelectedIndex of the page
    /// that would be switched to.
    /// + is cancellable, which allows to prevent changing of current TabPage!
    /// allow Mnemonics in TabPage's text
    /// + show underlined character in TabPage's text
    /// + switch to TabPage with ALT+underlined character (only if TabPage is
    /// enabled!)
    ///
    ///
    /// Some of the web pages where I found useful information on how to achieve
    /// things:
    /// OnDrawItem + tracking Enabling/Disabling of TabPages
    /// http://bethmassi.blogspot.com/2005_01_01_bethmassi_archive.html
    /// Drag'n'Drop support
    /// http://www.codeproject.com/cs/miscctrl/draggabletabcontrol.asp
    /// Basic disabling of TabPages (without Windows API calls!)
    /// http://www.devnewsgroups.net/group/microsoft.public.dotnet.framework.windowsforms/topic4852.aspx
    /// Mnemonic support
    /// http://dotnetrix.co.uk/tabcontrols.html
    ///
    ///
    /// </summary>
    public class TTabVersatile : System.Windows.Forms.TabControl
    {
        /// <summary>Operating System Message: left MouseButton pressed</summary>
        protected const Int32 WM_LBUTTONDOWN = 0x201;

        /// <summary> Required designer variable. </summary>
        protected System.ComponentModel.IContainer components = null;

        /// <summary>Maintains a list of TabPages that were disabled in EnableDisableAllOtherTabPages</summary>
        protected ArrayList FCustomDisabledPages;

        /// <summary>Selected TabPage before dragging operation began</summary>
        protected TabPage FSelectedTabPageBeforeDragging;

        /// <summary>Tells whether a TabPage is beeing dragged</summary>
        protected Boolean FTabPageIsDragging;

        /// <summary>TabControl.TabPages index of the TabPage that is beeing dragged</summary>
        protected Int32 FDraggedTabPageIndex;

        /// <summary>TabControl.TabPages index of the TabPage where the dragged TabPage is beeing dragged over</summary>
        protected Int32 FDragDestinationTabPageIndex;

        /// <summary>DragBox</summary>
        protected Rectangle FRectDragBoxFromMouseDown;

        /// <summary>
        /// area selected
        /// </summary>
        protected Rectangle FSelectedTabHeaderRectangle;

        /// <summary>Overridden property to prevent Selection of disabled TabPage</summary>
        public new Int32 SelectedIndex
        {
            get
            {
                return base.SelectedIndex;
            }

            set
            {
                if (this.TabPages[value].Enabled)
                {
                    base.SelectedIndex = value;
                }
                else
                {
                    throw new ESelectedIndexChangeDisallowedTabPagedIsDisabledException(
                        "TabPage with Index " + value.ToString() + " is disabled and therefore cannot be Selected");
                }
            }
        }

        /// <summary>Overridden property to prevent Selection of disabled TabPage</summary>
        public new TabPage SelectedTab
        {
            get
            {
                return base.SelectedTab;
            }

            set
            {
                if (value.Enabled)
                {
                    base.SelectedTab = value;
                }
                else
                {
                    throw new ESelectedIndexChangeDisallowedTabPagedIsDisabledException(
                        "TabPage '" + value.Name + "' is disabled and therefore cannot be Selected");
                }
            }
        }

        /// <summary>
        /// / Overridden properties follow
        /// Specifies whether the tabs in a tab control are owner-drawn (drawn by the
        /// parent window), or drawn by the operating system.
        ///
        /// </summary>
        public new TabDrawMode DrawMode
        {
            get
            {
                return base.DrawMode;
            }

            set
            {
                base.DrawMode = TabDrawMode.OwnerDrawFixed;

                // ignore value to prevent other settings!
            }
        }

        /// <summary>
        /// Specifies whether the tabs in a tab control are owner-drawn (drawn by the
        /// parent window), or drawn by the operating system.
        ///
        /// </summary>
        public new Boolean HotTrack
        {
            get
            {
                return base.HotTrack;
            }

            set
            {
                base.HotTrack = false;

                // ignore value to prevent other settings!
            }
        }

        /// <summary>
        /// Returns the Rectangle of the TabHeader of the selected Tab.
        ///
        /// </summary>
        public Rectangle SelectedTabHeaderRectangle
        {
            get
            {
                return FSelectedTabHeaderRectangle;
            }
        }

        /// <summary>
        /// / Custom Events follow
        /// This Event is thrown whenever the 'SelectedIndex' property for this control
        /// is about to change.
        ///
        /// </summary>
        public event TSelectedIndexChangingEventHandler SelectedIndexChanging;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            //
            // TTabVersatile
            //
            this.Name = "TTabVersatile";
        }

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public TTabVersatile() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            Set_DefaultProperties();
            InitializeComponent();

            FCustomDisabledPages = new ArrayList();

            // AlanP March 2013:  Use a try/catch block because nUnit testing on this screen does not support Drag/Drop in multi-threaded model
            // It is easier to do this than to configure all the different test execution methods to use STA
            try
            {
                this.AllowDrop = true;
            }
            catch (InvalidOperationException)
            {
                // ex.Message is: DragDrop registration did not succeed.
                // Inner exception is: Current thread must be set to single thread apartment (STA) mode before OLE calls can be made.
            }
        }

        /// <summary>
        /// destructor
        /// </summary>
        /// <param name="Disposing"></param>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        /// <summary>
        /// This procedure sets the default properties.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void Set_DefaultProperties()
        {
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            HotTrack = false;
        }

        #region Public Methods

        /// <summary>
        /// Disables and re-enables again all TabPages except the current TabPage.
        ///
        /// @comment When re-enabling the TabPages, only those TabPages are enabled that
        /// were not disabled at the time when this method was called to disable all
        /// other TabPages!
        ///
        /// </summary>
        /// <param name="AEnable">Set to false to disable, to true to re-enable
        /// </param>
        /// <returns>void</returns>
        public void EnableDisableAllOtherTabPages(Boolean AEnable)
        {
            Int16 Counter;

            if (AEnable == false)
            {
                for (Counter = 0; Counter <= this.TabPages.Count - 1; Counter += 1)
                {
                    if (this.TabPages[Counter].Enabled == true)
                    {
                        if (this.SelectedIndex != Counter)
                        {
                            this.TabPages[Counter].Enabled = false;
                            FCustomDisabledPages.Add(Counter);
                        }
                    }
                }
            }
            else
            {
                for (Counter = 0; Counter <= FCustomDisabledPages.Count - 1; Counter += 1)
                {
                    this.TabPages[(Int16)FCustomDisabledPages[Counter]].Enabled = true;
                }

                FCustomDisabledPages.Clear();
            }
        }

        #endregion

        #region Overridden Events

        /// <summary>
        /// process new control that has been added
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlAdded(System.Windows.Forms.ControlEventArgs e)
        {
            if (e.Control is TabPage)
            {
                // MessageBox.Show('TTabVersatile.OnControlAdded: ' + (e.Control as TabPage).Name );
                e.Control.EnabledChanged += new EventHandler(this.InvalidateTabHeader);
            }

            base.OnControlAdded(e);
        }

        /// <summary>
        /// react to removal of a control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlRemoved(System.Windows.Forms.ControlEventArgs e)
        {
            if (e.Control is TabPage)
            {
                // MessageBox.Show('TTabVersatile.OnControlRemoved: ' + (e.Control as TabPage).Name );
                e.Control.EnabledChanged -= new EventHandler(this.InvalidateTabHeader);
            }

            base.OnControlRemoved(e);
        }

        /// <summary>
        /// what happens if tab pages are dragged
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDragOver(System.Windows.Forms.DragEventArgs e)
        {
            base.OnDragOver(e);
            Point MousePoint = new Point(e.X, e.Y);

            // We need client coordinates
            MousePoint = PointToClient(MousePoint);

            // Get the tab we are hovering over
            TabPage DragDestinationTabPage = GetTabPageByPointInTabHeader(MousePoint, out FDragDestinationTabPageIndex);

            // Make sure we are hovering over a tab
            if (DragDestinationTabPage != null)
            {
                // Make sure there is a TabPage being dragged
                if (e.Data.GetDataPresent(typeof(TabPage)))
                {
                    e.Effect = DragDropEffects.Move;
                    TabPage DraggedTabPage = (TabPage)(e.Data.GetData(typeof(TabPage)));
                    Int16 DraggedTabPageIndex = (short)this.TabPages.IndexOf(DraggedTabPage);
                    Int16 MouseOverTabPageIndex = (short)this.TabPages.IndexOf(DragDestinationTabPage);

                    // TabPage is dropped on a different TabPage
                    if (DraggedTabPageIndex != MouseOverTabPageIndex)
                    {
                        this.SuspendLayout();
                        TabPage ReplacedTabPage = this.TabPages[MouseOverTabPageIndex];
                        ControlsUtilities.SwapTabPages(this, DraggedTabPage, ReplacedTabPage);

                        // this.TabPages[MouseOverTabPageIndex] := DraggedTabPage;
                        // this.TabPages[DraggedTabPageIndex] := ReplacedTabPage;
                        // Select the TabPage (bring it to front)  but only if it is not disabled and it's allowed
                        if (DraggedTabPage.Enabled)
                        {
                            if (FireSelectedIndexChanging(MouseOverTabPageIndex))
                            {
                                // Select dragged TabPage
                                this.SelectedTab = DraggedTabPage;
                            }
                            else
                            {
                                // Select only the one TabPage that was Selected when TabPage switching got disabled
                                this.SelectedTab = FSelectedTabPageBeforeDragging;
                            }
                        }

                        this.ResumeLayout();
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// what happens when a tab page is dragged away
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDragLeave(System.EventArgs e)
        {
            base.OnDragLeave(e);
            Int32 Counter;

            // MessageBox.Show('Dragging interrupted! FOriginalTabPageIndex: ' + FOriginalTabPageIndex.ToString + '; FDragTabPageIndex: ' + FDragTabPageIndex.ToString );
            // Restore the order of the TabPages as it was before the dragging operation started
            this.SuspendLayout();

            if (FDraggedTabPageIndex < FDragDestinationTabPageIndex)
            {
                for (Counter = FDragDestinationTabPageIndex; Counter <= FDraggedTabPageIndex + 1; Counter -= 1)
                {
                    // MessageBox.Show('Swapping TabPage ' + Counter.ToString + ' with TabPage ' + Convert.ToString(Counter  1) + '...');
                    ControlsUtilities.SwapTabPages(this, Counter, Counter - 1);
                }
            }
            else
            {
                for (Counter = FDragDestinationTabPageIndex; Counter <= FDraggedTabPageIndex - 1; Counter += 1)
                {
                    // MessageBox.Show('Swapping TabPage ' + Counter.ToString + ' with TabPage ' + Convert.ToString(Counter + 1) + '...');
                    ControlsUtilities.SwapTabPages(this, Counter, Counter + 1);
                }
            }

            this.ResumeLayout();
        }

        /// <summary>
        /// drawing the tab page
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Rectangle TabPageCanvas;
            Rectangle ClientRectangle;
            Rectangle RegionRightOfTabPageHeaders;

            Rectangle ColourRegionRightOfTabPageHeaders;
            RectangleF PaintingRect1;
            RectangleF PaintingRect2;
            RectangleF LineRect;
            RectangleF TopLineRect;
            RectangleF LeftLineRect;

            System.Drawing.Drawing2D.Matrix DrawingMatrix;
            StringFormat TextStringFormat;
            String TabName;
            Brush TextBrush;
            Int16 OffsetLeft;
            Int16 OffsetLeftIcon;
            Int16 OffsetTop;
            Int16 OffsetTopIcon;
            Boolean DisplayedVertical;
            Color ColourForRegionRightOfTabPageHeaders;
            SolidBrush ColourBrushForRegionRightOfTabPageHeaders;
            OffsetLeft = 0;
            OffsetLeftIcon = 0;
            OffsetTop = 0;
            OffsetTopIcon = 0;
            TabPageCanvas = e.Bounds;
            PaintingRect1 = new RectangleF(TabPageCanvas.X, TabPageCanvas.Y, TabPageCanvas.Width, TabPageCanvas.Height);
            DisplayedVertical = this.Alignment > TabAlignment.Bottom;

            // Distinguish between selected and nonselected TabPage
            if (e.Index == this.SelectedIndex)
            {
                // Determine Offsets for displaying Text and Icon
                switch (this.Alignment)
                {
                    case TabAlignment.Top:
                        OffsetLeft = 3;
                        OffsetLeftIcon = 4;
                        OffsetTop = 4;
                        OffsetTopIcon = 4;
                        break;

                    case TabAlignment.Left:
                        OffsetLeft = 3;
                        OffsetLeftIcon = 1;
                        OffsetTop = 3;
                        OffsetTopIcon = 3;
                        break;

                    case TabAlignment.Bottom:
                        OffsetLeft = 2;
                        OffsetLeftIcon = 6;
                        OffsetTop = 6;
                        OffsetTopIcon = 4;
                        break;

                    case TabAlignment.Right:
                        OffsetLeft = 9;
                        OffsetLeftIcon = -5;
                        OffsetTop = 6;
                        OffsetTopIcon = 5;
                        break;
                }

                // Determine Text colour
                if (this.Enabled)
                {
                    if (this.TabPages[e.Index].Enabled)
                    {
                        TextBrush = new SolidBrush(System.Drawing.Color.MediumBlue);
                    }
                    else
                    {
                        TextBrush = new SolidBrush(System.Drawing.SystemColors.ControlDarkDark);
                    }
                }
                else
                {
                    TextBrush = new SolidBrush(System.Drawing.SystemColors.ControlDarkDark);
                }

                // Fill Background with a Colour  this overpaints the 'white' line that is painted by Windows at the bottom of the Tab.
                e.Graphics.FillRectangle(new SolidBrush(System.Drawing.SystemColors.Control), PaintingRect1);

                switch (this.Alignment)
                {
                    case TabAlignment.Top:

                        // Create yellow top bar
                        TopLineRect = new RectangleF(PaintingRect1.Left, PaintingRect1.Top + 1, PaintingRect1.Right - PaintingRect1.Left, 2);
                        e.Graphics.FillRectangle(new SolidBrush(System.Drawing.Color.Orange), TopLineRect);
                        break;

                    case TabAlignment.Bottom:

                        // TODO
                        break;
                }

                // Create dark left Line
                LeftLineRect = new RectangleF(PaintingRect1.Left + 2, PaintingRect1.Top + 3, 1, PaintingRect1.Bottom - PaintingRect1.Top - 4);
                e.Graphics.FillRectangle(new SolidBrush(System.Drawing.SystemColors.ControlDarkDark), LeftLineRect);
                FSelectedTabHeaderRectangle = TabPageCanvas;
            }
            else
            {
                // (e.Index <> this.SelectedIndex)

                // Determine Offsets for displaying Text and Icon
                switch (this.Alignment)
                {
                    case TabAlignment.Top:
                        OffsetLeft = 2;
                        OffsetLeftIcon = 1;
                        OffsetTop = 4;
                        OffsetTopIcon = 3;
                        break;

                    case TabAlignment.Left:
                        OffsetLeft = 2;
                        OffsetLeftIcon = -2;
                        OffsetTop = 4;
                        OffsetTopIcon = 4;
                        break;

                    case TabAlignment.Bottom:
                        OffsetLeft = 3;
                        OffsetLeftIcon = 1;
                        OffsetTop = 1;
                        OffsetTopIcon = -1;
                        break;

                    case TabAlignment.Right:
                        OffsetLeft = 5;
                        OffsetLeftIcon = -3;
                        OffsetTop = 0;
                        OffsetTopIcon = -1;
                        break;
                }

                // Determine Text colour
                if (this.Enabled)
                {
                    if (this.TabPages[e.Index].Enabled)
                    {
                        TextBrush = new SolidBrush(e.ForeColor);
                    }
                    else
                    {
                        TextBrush = new SolidBrush(System.Drawing.SystemColors.ControlDarkDark);
                    }
                }
                else
                {
                    TextBrush = new SolidBrush(System.Drawing.SystemColors.ControlDarkDark);
                }
            }

            // (e.Index = this.SelectedIndex)
            // Rotate e.Graphics if Tabs are vertical aligned
            if (DisplayedVertical)
            {
                DrawingMatrix = new System.Drawing.Drawing2D.Matrix();
                DrawingMatrix.Translate(0, TabPageCanvas.Height - this.TabPages[0].Top);
                DrawingMatrix.RotateAt(270, new PointF(TabPageCanvas.X, TabPageCanvas.Y));
                e.Graphics.Transform = DrawingMatrix;
            }

            // Determine Text rectangle, draw Icon
            if ((this.TabPages[e.Index].ImageIndex != -1) && (this.ImageList != null))
            {
                // Smaller text rectangle because Icon is present
                if (DisplayedVertical)
                {
                    PaintingRect2 = new RectangleF(TabPageCanvas.Left - this.TabPages[0].Top + (8),
                        TabPageCanvas.Top + OffsetTop,
                        PaintingRect1.Height - OffsetTop,
                        PaintingRect1.Width);
                }
                else
                {
                    PaintingRect2 = new RectangleF(PaintingRect1.X + (8),
                        PaintingRect1.Y + OffsetTop,
                        PaintingRect1.Width,
                        PaintingRect1.Height - OffsetTop);
                }

                // Draw Icon
                if (this.TabPages[e.Index].Enabled)
                {
                    // Draw Icon as it is
                    e.Graphics.DrawImage(this.ImageList.Images[this.TabPages[e.Index].ImageIndex], Convert.ToInt32(
                            PaintingRect1.Left) + OffsetLeft + OffsetLeftIcon, Convert.ToInt32(PaintingRect1.Top) + OffsetTopIcon - 1);
                }
                else
                {
                    // Draw Icon in 'disabled' style ('grayed out')
                    System.Windows.Forms.ControlPaint.DrawImageDisabled(e.Graphics,
                        this.ImageList.Images[this.TabPages[e.Index].ImageIndex],
                        Convert.ToInt32(PaintingRect1.Left) + OffsetLeft + OffsetLeftIcon,
                        Convert.ToInt32(PaintingRect1.Top) + OffsetTopIcon - 1,
                        this.TabPages[e.Index].BackColor);
                }
            }
            else
            {
                // no Icon

                // Bigger text rectangle than if Icon is present
                if (DisplayedVertical)
                {
                    PaintingRect2 = new RectangleF(TabPageCanvas.Left - this.TabPages[0].Top,
                        TabPageCanvas.Top + OffsetTop,
                        PaintingRect1.Height,
                        PaintingRect1.Width);
                }
                else
                {
                    PaintingRect2 = new RectangleF(PaintingRect1.X, PaintingRect1.Y + OffsetTop, PaintingRect1.Width, PaintingRect1.Height);
                }
            }

            // Draw line at bottom of Tab.
            switch (this.Alignment)
            {
                case TabAlignment.Top:

                    if (e.Index == this.SelectedIndex)
                    {
                        LineRect = new RectangleF(PaintingRect1.Left - 0, PaintingRect1.Bottom - 2, PaintingRect1.Right - PaintingRect1.Left + 1, 1);
                        e.Graphics.FillRectangle(new SolidBrush(System.Drawing.SystemColors.Control), LineRect);
                    }
                    else
                    {
                        LineRect = new RectangleF(PaintingRect1.Left - 2, PaintingRect1.Bottom + 2, PaintingRect1.Right - PaintingRect1.Left + 4, 1);
                        e.Graphics.FillRectangle(new SolidBrush(System.Drawing.SystemColors.ControlDarkDark), LineRect);

                        // ControlDarkDark
                    }

                    break;

                case TabAlignment.Bottom:

                    // TODO
                    break;
            }

            // Draw text
            TabName = this.TabPages[e.Index].Text;
            TextStringFormat = new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
            TextStringFormat.Alignment = StringAlignment.Center;
            TextStringFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
            e.Graphics.DrawString(TabName, e.Font, TextBrush, PaintingRect2, TextStringFormat);

            // SolidBrush.Create(e.ForeColor), r2     this.GetTabRect(e.Index)

            /* Colour region right of TabPage Headers that is by default coloured in
             * Control colour in Background colour of the Parent.
             * FIXME: Works not correctly if the Control is displayed in Multiline mode and
             * more than one line of tabs is used.
             */

            // TabControl can be in Form or in Panel. Both have BackColor property.
            ColourForRegionRightOfTabPageHeaders = this.Parent.BackColor;
            ClientRectangle = this.ClientRectangle;
            RegionRightOfTabPageHeaders = this.GetTabRect(this.TabCount - 1);
            ColourBrushForRegionRightOfTabPageHeaders = new SolidBrush(ColourForRegionRightOfTabPageHeaders);

            switch (this.Alignment)
            {
                case TabAlignment.Top:
                    ColourRegionRightOfTabPageHeaders = new Rectangle(RegionRightOfTabPageHeaders.X + RegionRightOfTabPageHeaders.Width,
                    RegionRightOfTabPageHeaders.Y - 2,
                    ClientRectangle.Width - RegionRightOfTabPageHeaders.X - RegionRightOfTabPageHeaders.Width,
                    RegionRightOfTabPageHeaders.Height + 2);
                    e.Graphics.FillRectangle(ColourBrushForRegionRightOfTabPageHeaders, ColourRegionRightOfTabPageHeaders);

                    // Draw line where the bottom of a Tab would be.
                    LineRect = new RectangleF(ColourRegionRightOfTabPageHeaders.Left,
                    ColourRegionRightOfTabPageHeaders.Bottom,
                    ColourRegionRightOfTabPageHeaders.Right - ColourRegionRightOfTabPageHeaders.Left,
                    1);
                    e.Graphics.FillRectangle(new SolidBrush(System.Drawing.SystemColors.ControlDarkDark), LineRect);
                    break;

                case TabAlignment.Bottom:
                    ColourRegionRightOfTabPageHeaders = new Rectangle(RegionRightOfTabPageHeaders.X + RegionRightOfTabPageHeaders.Width,
                    RegionRightOfTabPageHeaders.Y,
                    ClientRectangle.Width - RegionRightOfTabPageHeaders.X - RegionRightOfTabPageHeaders.Width,
                    RegionRightOfTabPageHeaders.Height + 2);
                    e.Graphics.FillRectangle(ColourBrushForRegionRightOfTabPageHeaders, ColourRegionRightOfTabPageHeaders);

                    // Draw line where the top of a Tab would be.
                    // TODO
                    break;
            }

            /* The following code would get rid of the white 2pixel high line above the TabPage headers,
             * but unfortunately then the scroller buttons stick out quite ugly. Tryed to paint where the
             * scroller buttons are, but Windows overpaints them. So - perhaps someone finds sometime a
             * solution to that...
             *
             * // Colour few pixels above the TabPage header that are strangely left white...
             * TabPageHeaderRectangle := this.GetTabRect(e.Index);
             *
             * if (e.Index <> this.SelectedIndex) then
             * begin
             * ColourTabPageHeaderRectangle := new Rectangle(
             * TabPageHeaderRectangle.X,
             * TabPageHeaderRectangle.Y - 2,
             * TabPageHeaderRectangle.X + TabPageHeaderRectangle.Width,
             * 3);
             *
             * e.Graphics.FillRectangle(ColourBrushForRegionRightOfTabPageHeaders, ColourTabPageHeaderRectangle);
             * end
             * else
             * begin
             * ColourTabPageHeaderRectangle := new Rectangle(
             * TabPageHeaderRectangle.X,
             * TabPageHeaderRectangle.Y,
             * TabPageHeaderRectangle.X + TabPageHeaderRectangle.Width,
             * 1);
             * ColourBrushForRegionRightOfTabPageHeaders := new SolidBrush(this.BackColor);
             * e.Graphics.FillRectangle(ColourBrushForRegionRightOfTabPageHeaders, ColourTabPageHeaderRectangle);
             * end;
             *
             * // Take care of the scroller buttons...
             * ColourRegionRightOfTabPageHeaders := new Rectangle(
             * ClientRectangle.Width - 30,
             * RegionRightOfTabPageHeaders.Y - 5,
             * 30,
             * 5);
             *
             * e.Graphics.FillRectangle(SolidBrush.Create(Color.Black), ColourRegionRightOfTabPageHeaders);
             */

            // Reset e.Graphics rotation
            if (DisplayedVertical)
            {
                e.Graphics.ResetTransform();
            }

            // Dispose used System.Objects
            TextStringFormat.Dispose();
            TextBrush.Dispose();
            ColourBrushForRegionRightOfTabPageHeaders.Dispose();
        }

        /// <summary>
        /// deal with keys being pressed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            Int32 SelIndex;
            Int32 Counter1;
            Int32 Counter2;
            Boolean TabFound;

            // MessageBox.Show('TTabVersatile.OnKeyDown');
            SelIndex = this.SelectedIndex;
            TabFound = false;

            // Cursor Keys are only handled if a Tab has actually got the focus
            if (this.Focused)
            {
                if ((e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Up))
                {
                    for (Counter1 = (short)(SelIndex - 1); Counter1 >= 0; Counter1 -= 1)
                    {
                        if (this.TabPages[Counter1].Enabled)
                        {
                            if (FireSelectedIndexChanging(Counter1))
                            {
                                this.SelectedIndex = Counter1;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    e.Handled = true;
                }
                else if ((e.KeyCode == Keys.Right) || (e.KeyCode == Keys.Down))
                {
                    for (Counter1 = (short)(SelIndex + 1); Counter1 <= this.TabPages.Count - 1; Counter1 += 1)
                    {
                        if (this.TabPages[Counter1].Enabled)
                        {
                            if (FireSelectedIndexChanging(Counter1))
                            {
                                this.SelectedIndex = Counter1;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Home)
                {
                    for (Counter1 = 0; Counter1 <= this.TabPages.Count - 1; Counter1 += 1)
                    {
                        if (this.TabPages[Counter1].Enabled)
                        {
                            if (FireSelectedIndexChanging(Counter1))
                            {
                                this.SelectedIndex = Counter1;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.End)
                {
                    for (Counter1 = this.TabPages.Count - 1; Counter1 <= 0; Counter1 -= 1)
                    {
                        if (this.TabPages[Counter1].Enabled)
                        {
                            if (FireSelectedIndexChanging(Counter1))
                            {
                                this.SelectedIndex = Counter1;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    e.Handled = true;
                }
            }

            // this.Focused = true
            // CTRL+TAB and CTRL+SHIFT+TAB are handled even if the Tab hasn't actually got the focus!
            if ((e.KeyCode == Keys.Tab) && (e.Modifiers == Keys.Control))
            {
                // MessageBox.Show('CTRL+TAB pressed. Current SelectedIndex: ' + SelIndex.ToString);
                for (Counter1 = SelIndex + 1; Counter1 <= this.TabPages.Count - 1; Counter1 += 1)
                {
                    if (this.TabPages[Counter1].Enabled)
                    {
                        if (FireSelectedIndexChanging(Counter1))
                        {
                            this.SelectedIndex = Counter1;

                            // MessageBox.Show('CTRL+TAB pressed. New SelectedIndex: ' + this.SelectedIndex.ToString);
                            TabFound = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (!TabFound)
                {
                    for (Counter2 = 0; Counter2 <= this.TabPages.Count - 1; Counter2 += 1)
                    {
                        if (this.TabPages[Counter2].Enabled)
                        {
                            if (FireSelectedIndexChanging(Counter2))
                            {
                                this.SelectedIndex = Counter2;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                return;
            }
            else if ((e.KeyCode == Keys.Tab) && (e.Control && e.Shift))
            {
                // MessageBox.Show('CTRL+SHIFT+TAB pressed. Current SelectedIndex: ' + SelIndex.ToString);
                for (Counter1 = SelIndex - 1; Counter1 >= 0; Counter1 -= 1)
                {
                    if (this.TabPages[Counter1].Enabled)
                    {
                        if (FireSelectedIndexChanging(Counter1))
                        {
                            this.SelectedIndex = Counter1;

                            // MessageBox.Show('CTRL+SHIFT+TAB pressed. New SelectedIndex: ' + this.SelectedIndex.ToString);
                            TabFound = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (!TabFound)
                {
                    for (Counter2 = this.TabPages.Count - 1; Counter2 >= 0; Counter2 -= 1)
                    {
                        if (this.TabPages[Counter2].Enabled)
                        {
                            if (FireSelectedIndexChanging(Counter2))
                            {
                                this.SelectedIndex = Counter2;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                return;
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// what happens when mouse is clicked
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            // Calclulate DragBox
            CalcRectDragBox(e.X, e.Y);
        }

        /// <summary>
        /// moving the mouse
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            TabPage DraggedTabPage;
            Point MousePoint;

            base.OnMouseMove(e);

            if ((e.Button & System.Windows.Forms.MouseButtons.Left) == System.Windows.Forms.MouseButtons.Left)
            {
                if ((FRectDragBoxFromMouseDown != Rectangle.Empty) && (FRectDragBoxFromMouseDown.Contains(e.X, e.Y) == false))
                {
                    MousePoint = new Point(e.X, e.Y);
                    FSelectedTabPageBeforeDragging = this.SelectedTab;

                    // Get the tab we are hovering over
                    DraggedTabPage = GetTabPageByPointInTabHeader(MousePoint, out FDraggedTabPageIndex);
                    FTabPageIsDragging = true;

                    // Proceed with the drag and drop
                    try
                    {
                        if (DraggedTabPage != null)
                        {
                            DoDragDrop(DraggedTabPage, DragDropEffects.Move);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Please try again!");
                    }

                    // Reset the DragBox to avoid reentry of drag
                    CalcRectDragBox(e.X, e.Y);
                    FTabPageIsDragging = false;

                    // Repaint the TabControl so that we see the dragged TabPage
                    Invalidate();
                }
            }
        }

        #endregion

        #region Functionality customisation

        /// <summary>
        /// Prevents changing to a TabPage by MouseClick if the TabPage is disabled.
        ///
        /// </summary>
        /// <param name="AMessage">Operating System Message
        /// </param>
        /// <returns>void</returns>
        protected override void WndProc(ref System.Windows.Forms.Message AMessage)
        {
            Point ClickPoint;
            Int16 Counter;

            if (AMessage.Msg == WM_LBUTTONDOWN)
            {
                ClickPoint = new Point(AMessage.LParam.ToInt32());

                for (Counter = 0; Counter <= this.TabPages.Count - 1; Counter += 1)
                {
                    if (this.GetTabRect(Counter).Contains(ClickPoint))
                    {
                        if (this.SelectedIndex != Counter)
                        {
                            if (this.TabPages[Counter].Enabled)
                            {
                                if (FireSelectedIndexChanging(Counter))
                                {
                                    base.WndProc(ref AMessage);
                                }
                            }
                        }
                        else
                        {
                            base.WndProc(ref AMessage);
                        }

                        return;
                    }
                }
            }
            else
            {
                // AlanP March 2013:  Use a try/catch block because nUnit testing on this screen does not support Drag/Drop in multi-threaded model
                // It is easier to do this than to configure all the different test execution methods to use STA
                try
                {
                    base.WndProc(ref AMessage);
                }
                catch (InvalidOperationException)
                {
                    // ex.Message is: DragDrop registration did not succeed.
                    // Inner exception is: Current thread must be set to single thread apartment (STA) mode before OLE calls can be made.
                }
            }
        }

        /// <summary>
        /// Switches to a TabPage whose Mnemonic Character is pressed - but only if the
        /// TabPage is enabled.
        ///
        /// </summary>
        /// <param name="ACharCode">Character Code to evaluate.</param>
        /// <returns>true if pressing the Mnemonic Character caused a switch to the
        /// associated TabPage, false if not or the ACharcode is not associated with any
        /// TabPage.
        /// </returns>
        protected override Boolean ProcessMnemonic(Char ACharCode)
        {
            Int16 Counter;

            for (Counter = 0; Counter <= this.TabPages.Count - 1; Counter += 1)
            {
                if (Control.IsMnemonic(ACharCode, this.TabPages[Counter].Text) && (this.TabPages[Counter].Enabled))
                {
                    if (FireSelectedIndexChanging(Counter))
                    {
                        this.SelectedIndex = Counter;
                        this.Focus();
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Helper functions

        /// <summary>
        /// Builds the Arguments for and fires the SelectedIndexChanging Event.
        ///
        /// </summary>
        /// <param name="ANewSelectedIndex">Index of the TabPage to which the TabControl will
        /// change</param>
        /// <returns>true if the change is allowed, false if the Event was cancelled by
        /// the System.Object that was listening to it.
        /// </returns>
        private Boolean FireSelectedIndexChanging(Int32 ANewSelectedIndex)
        {
            Boolean ReturnValue;
            TSelectedIndexChangingEventArgs Args;

            Args = new TSelectedIndexChangingEventArgs();
            Args.SelectedIndex = this.SelectedIndex;
            Args.NewSelectedIndex = ANewSelectedIndex;

            if (SelectedIndexChanging != null)
            {
                SelectedIndexChanging(this, Args);

                if (Args.Cancel == true)
                {
                    ReturnValue = false;
                }
                else
                {
                    ReturnValue = true;
                }
            }
            else
            {
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Forces a repainting of the TabHeader.
        ///
        /// </summary>
        /// <param name="sender">A TabPage</param>
        /// <param name="e">EventArgs (Ignored!)
        /// </param>
        /// <returns>void</returns>
        private void InvalidateTabHeader(System.Object sender, EventArgs e)
        {
            Int16 Counter;

            if (sender is TabPage)
            {
                for (Counter = 0; Counter <= this.TabPages.Count - 1; Counter += 1)
                {
                    if (this.TabPages == sender)
                    {
                        // MessageBox.Show('Tab_EnabledChanged for TabPage: ' + this.TabPages[Counter].Text + ' [' + Counter.ToString + ']; Enabled: ' + (this.TabPages[Counter].Enabled).ToString);
                        this.Invalidate(this.GetTabRect(Counter));
                    }
                }
            }
        }

        /// <summary>
        /// Determines the TabPage that contains the Point in the TabHeader.
        ///
        /// </summary>
        /// <param name="APointInTabHeader">A Point in one of the TabHeaders</param>
        /// <param name="ATabPageIndex">Index of the TabPage that contains the Point in the
        /// TabHeader</param>
        /// <returns>TabPage that contains the Point in the TabHeader
        /// </returns>
        private TabPage GetTabPageByPointInTabHeader(Point APointInTabHeader, out Int32 ATabPageIndex)
        {
            Int32 Counter;
            TabPage TheTabPage;

            TheTabPage = null;
            ATabPageIndex = -1;

            for (Counter = 0; Counter <= this.TabPages.Count - 1; Counter += 1)
            {
                if (GetTabRect(Counter).Contains(APointInTabHeader))
                {
                    TheTabPage = this.TabPages[Counter];
                    ATabPageIndex = Counter;
                    break;
                }
            }

            return TheTabPage;
        }

        /// <summary>
        /// Remember the point where the mouse down occurred.
        ///
        /// </summary>
        /// <param name="AXCoordinate">X coordinate</param>
        /// <param name="AYCoordinate">Y coordinate
        /// </param>
        /// <returns>void</returns>
        private void CalcRectDragBox(Int32 AXCoordinate, Int32 AYCoordinate)
        {
            System.Drawing.Size DragRectangleSize;

            // The DragSize indicates the size that the mouse can move before a drag event should be started.
            DragRectangleSize = SystemInformation.DragSize;
            FRectDragBoxFromMouseDown =
                new Rectangle(new Point(Convert.ToInt32(AXCoordinate - (DragRectangleSize.Width / 2.0)),
                        Convert.ToInt32(AYCoordinate - (DragRectangleSize.Height / 2.0))), DragRectangleSize);
        }

        #endregion
    }

    #region ESelectedIndexChangeDisallowedTabPagedIsDisabledException
    
    /// <summary>
    /// Tab page is disabled and therefore cannot be selected.
    /// </summary>
    public class ESelectedIndexChangeDisallowedTabPagedIsDisabledException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ESelectedIndexChangeDisallowedTabPagedIsDisabledException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param> 
        public ESelectedIndexChangeDisallowedTabPagedIsDisabledException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ESelectedIndexChangeDisallowedTabPagedIsDisabledException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }
    
    #endregion
}