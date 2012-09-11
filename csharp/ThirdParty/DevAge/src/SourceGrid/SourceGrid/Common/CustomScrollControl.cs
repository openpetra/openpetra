using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SourceGrid
{
	//Note about the new scrolling code:
	//This algorithm still has some bugs.
	//Right now the list is not long, but it seems you will have to deal with this, Davide:


	//(FIXED) 1. [MICK(22)] GetVisibleRows returns partially visible row, though it shouldnt.
	//  In sample 3, when focus is just above "boolean checkbox" and "boolean checkbox" is partially visible,
	//  GetVisibleRows returns also this row and it should not. This causes CustomScrollPageDown() not to scroll down.
	//DAVIDE: This is not a bug in the GetVisibleRows but in the GridVirtual.CustomScrollPageDown (now fixed) that didn't support Rows/Column span

	//2. [MICK(23)] [MICK(24)]
	//If there are no focusable cells below, the algorithm does not know what to do.
	//Same thing when one cell is spread over a few cells in other row (sample 3, "editors and types", all blue titles are examples of these). If the focus is on the cell "Single Image", there is no scrolling or acrolling is inapropriate (because Selection.MoveActiveCell(-1, 0); is done instead scrolling).

	/// <summary>
	/// A control with a custom implementation of a scrollable area
	/// </summary>
	[System.ComponentModel.ToolboxItem(false)]
	public abstract class CustomScrollControl : System.Windows.Forms.Panel
	{
		#region Constructor
		public CustomScrollControl()
		{
			SuspendLayout();

			Name = "CustomScrollControl";
			base.AutoScroll = false;
			//to remove flicker and use custom draw
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			CreateDockControls();

			ResumeLayout(false);
		}

		protected virtual void CreateDockControls()
		{
			panelDockBottom.SuspendLayout();
			panelDockBottom.Controls.Add(mHScrollBar);
			panelDockBottom.Controls.Add(mBottomRightPanel);
			panelDockBottom.Dock = System.Windows.Forms.DockStyle.Bottom;

			mBottomRightPanel.Dock = System.Windows.Forms.DockStyle.Right;

			mHScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;

			mVScrollBar.Dock = System.Windows.Forms.DockStyle.Right;

			Controls.Add(mVScrollBar);
			Controls.Add(panelDockBottom);

			mHScrollBar.ValueChanged += new EventHandler(HScroll_Change);
			mVScrollBar.ValueChanged += new EventHandler(VScroll_Change);

			mVScrollBar.TabStop = false;
			mHScrollBar.TabStop = false;
			mBottomRightPanel.TabStop = false;
			panelDockBottom.TabStop = false;
			panelDockBottom.TabIndex = 1;
			mBottomRightPanel.TabIndex = 2;
			mVScrollBar.TabIndex = 3;
			mHScrollBar.TabIndex = 4;

			mBottomRightPanel.BackColor = Color.FromKnownColor(KnownColor.Control);

			PrepareScrollBars(false, false);

			panelDockBottom.ResumeLayout(false);
		}
		#endregion

		#region override AutoScroll
		/// <summary>
		/// I disabled the default AutoScroll property because I have a custom implementation
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
		public override bool AutoScroll
		{
			get{return false;}
			set
			{
				if (value)
					throw new SourceGridException("Auto Scroll not supported in this control");
				base.AutoScroll = false;
			}
		}
		#endregion

		#region ScrollBars and Panels
		private HScrollBar mHScrollBar = new HScrollBar();
		private VScrollBar mVScrollBar = new VScrollBar();
		/// <summary>
		/// Panel showed on the bottom right of the grid when both scrollbars are visible
		/// </summary>
		private Panel mBottomRightPanel = new Panel();
		/// <summary>
		/// Internal panel that contains hScrollBar and mBottomRightPanel
		/// </summary>
		private System.Windows.Forms.Panel panelDockBottom = new System.Windows.Forms.Panel();

		/// <summary>
		/// Gets the vertical scrollbar. Can be visible or unvisible.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public VScrollBar VScrollBar
		{
			get{return mVScrollBar;}
		}

		/// <summary>
		/// Gets the horizontal scrollbar. Can be visible or unvisible.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HScrollBar HScrollBar
		{
			get{return mHScrollBar;}
		}

		/// <summary>
		/// Gets the panel at the bottom right of the control. This panel is valid only if HScrollBar and VScrollBar are valid. Otherwise is null.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Panel BottomRightPanel
		{
			get{return mBottomRightPanel;}
		}

		/// <summary>
		/// Invalidate the scrollable area
		/// </summary>
		protected abstract void InvalidateScrollableArea();

		/// <summary>
		/// Prepare the scrollbars with the specified dock option.
		/// </summary>
		/// <param name="showHScroll"></param>
		/// <param name="showVScroll"></param>
		protected virtual void PrepareScrollBars(bool showHScroll, bool showVScroll)
		{
			if (showHScroll != HScrollBarVisible)
			{
				if (showHScroll)
				{
					panelDockBottom.Height = System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight;
					//panelDockBottom.Enabled = true;
					//panelDockBottom.Visible = true;
					//mHScrollBar.Visible = true;
					SetHScrollBarVisible(true);
				}
				else
				{
					panelDockBottom.Height = 0;
					//panelDockBottom.Enabled = false;
					//panelDockBottom.Visible = false;
					//mHScrollBar.Visible = false;
					SetHScrollBarVisible(false);
				}
			}

			if (showVScroll != VScrollBarVisible)
			{
				if (showVScroll)
				{
					//mVScrollBar.Width = System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight;
					//mBottomRightPanel.Enabled = true;
					//mBottomRightPanel.Visible = true;
					//mVScrollBar.Enabled = true;
					//mVScrollBar.Visible = true;
					SetVScrollBarVisible(true);
					mBottomRightPanel.Width = mVScrollBar.Width;
				}
				else
				{
					//mVScrollBar.Width = 0;
					//mVScrollBar.Enabled = false;
					//mVScrollBar.Visible = false;
					SetVScrollBarVisible(false);
					mBottomRightPanel.Width = 0;
					//mBottomRightPanel.Enabled = false;
					//mBottomRightPanel.Visible = false;
				}
			}
		}

		protected void SetHScrollBarVisible(bool value)
		{
			if (value)
				mHScrollBar.Height = System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight;
			else
				mHScrollBar.Height = 0;
		}
		[Browsable(false)]
		public bool HScrollBarVisible
		{
			get{return mHScrollBar.Height != 0;}
		}
		protected void SetVScrollBarVisible(bool value)
		{
			if (value)
				mVScrollBar.Width = System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
			else
				mVScrollBar.Width = 0;
		}
		[Browsable(false)]
		public bool VScrollBarVisible
		{
			get{return mVScrollBar.Width != 0;}
		}

		#endregion

		#region ScrollArea, ScrollPosition, DisplayRectangle

		private int mHorizontalPage = 0;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected int HorizontalPage
		{
			get { return mHorizontalPage; }
		}

		private int mVerticalPage = 0;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected int VerticalPage
		{
			get { return mVerticalPage; }
		}

		/// <summary>
		/// Load the scrollable area that will affect the maximum scroll values.
		/// </summary>
		/// <param name="verticalPage"></param>
		/// <param name="horizontalPage"></param>
		protected void LoadScrollArea(int verticalPage, int horizontalPage)
		{
			if (verticalPage < 0)
				throw new ArgumentOutOfRangeException();
			mVerticalPage = verticalPage;
			
			if (horizontalPage < 0 )
				throw new ArgumentOutOfRangeException();
			mHorizontalPage = horizontalPage;

			RecalcCustomScrollBars();
		}

		/// <summary>
		/// Gets or sets the current scroll position relative to the CustomScrollArea.
		/// The value must be always between 0 and CustomScrollArea (0 or positive).
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual Point CustomScrollPosition
		{
			get
			{
				int x = 0;
				if (HScrollBarVisible)
					x = mHScrollBar.Value;

				int y = 0;
				if (VScrollBarVisible)
					y = mVScrollBar.Value;

				return new Point(x, y);
			}
			set
			{
				if (HScrollBarVisible)
				{
					if (value.X > 0)
						mHScrollBar.Value = value.X;
					else
						mHScrollBar.Value = 0;
				}

				if (VScrollBarVisible)
				{
					if (mVScrollBar.Maximum < value.Y)
						return;
					if (value.Y > 0)
						mVScrollBar.Value = value.Y;
					else
						mVScrollBar.Value = 0;
				}
			}
		}

		/// <summary>
		/// Display rectangle of the control, without ScrollBars.
		/// Note: I don't override this method because I use some dock feature that require the real display rectangle.
		/// </summary>
		public new Rectangle DisplayRectangle
		{
			get
			{
				int scrollH = 0;
				if (HScrollBarVisible)
					scrollH = mHScrollBar.Height;

				int scrollV = 0;
				if (VScrollBarVisible)
					scrollV = mVScrollBar.Width;

				Rectangle baseDisplay = base.DisplayRectangle;
				return new Rectangle(baseDisplay.X, baseDisplay.Y,
				                     baseDisplay.Width - scrollV, baseDisplay.Height - scrollH);
			}
		}

		#endregion

		#region Add/Remove ScrollBars
		/// <summary>
		/// recalculate the position of the horizontal scrollbar
		/// </summary>
		private void RecalcHScrollBar(int cols)
		{
			if (HScrollBarVisible == false)
				return;
			
			mHScrollBar.Minimum = 0;
			mHScrollBar.Maximum = cols + HorizontalPage - 1;
			
			if (HorizontalPage > 1)
				mHScrollBar.LargeChange = HorizontalPage; else
				mHScrollBar.LargeChange = 1;

			//mHScrollBar.SmallChange = 1;

			if (mHScrollBar.Value > MaximumHScroll)
				mHScrollBar.Value = MaximumHScroll;
		}

		/// <summary>
		/// Recalculate the position of the vertical scrollbar
		/// </summary>
		private void RecalcVScrollBar(int rows)
		{
			if (VScrollBarVisible == false)
				return;
			mVScrollBar.Minimum = 0;
			//MICK(4): Now (rows + VerticalPage - 1) equals the count of all rows minus fixed rows...
			// But I do not change it here because I do not know how to do that properly
			mVScrollBar.Maximum = rows + VerticalPage - 1;
			//MICK(5): this is changed too
			if (VerticalPage > 1)
				mVScrollBar.LargeChange = VerticalPage;
			else
				mVScrollBar.LargeChange = 1;

			//mVScrollBar.SmallChange = 1;

			if (mVScrollBar.Value > MaximumVScroll)
				mVScrollBar.Value = MaximumVScroll;
		}

		/// <summary>
		/// Recalculate the scrollbars position and size.
		/// Use this to refresh scroll bars
		/// </summary>
		public void RecalcCustomScrollBars()
		{
			SuspendLayout();


			//I use the base.DisplayRectangle to returns the actual Display without consider the size of the scrollbars

			int scrollRows = GetScrollRows(base.DisplayRectangle.Height);
			int scrollCols = GetScrollColumns(base.DisplayRectangle.Width);

			PrepareScrollBars(scrollCols > 0, scrollRows > 0);

			////now I recheck the area with the scrollbars
			//scrollRows = GetScrollRows(DisplayRectangle.Height);
			//scrollCols = GetScrollColumns(DisplayRectangle.Width);

			//PrepareScrollBars(scrollCols > 0, scrollRows > 0);

			//Finally I read the actual values to use (that can be changed because I have called PrepareScrollBars)
			if (scrollRows > 0)
				scrollRows = GetScrollRows(DisplayRectangle.Height);
			if (scrollCols > 0)
				scrollCols = GetScrollColumns(DisplayRectangle.Width);

			//MICK(6)
			scrollRows = scrollRows - GetActualFixedRows();

			RecalcVScrollBar(scrollRows);
			RecalcHScrollBar(scrollCols);

			//forzo un ridisegno
			InvalidateScrollableArea();

			ResumeLayout(true);
		}

		/// <summary>
		/// Calculate the number of rows to scroll. 0 to disable the scrollbar.
		/// The returned value is independent from the current scrolling position, must be a fixed value
		/// calculated based on the total number of rows and the available area.
		/// </summary>
		protected abstract int GetScrollRows(int displayHeight);

		//MICK(7)
		protected abstract int GetActualFixedRows();

		
		/// <summary>
		/// Calculate the number of columns to scroll. 0 to disable the scrollbar.
		/// The returned value is independent from the current scrolling position, must be a fixed value
		/// calculated based on the total number of columns and the available area.
		/// </summary>
		protected abstract int GetScrollColumns(int displayWidth);

		#endregion

		#region Maximum/Minimum Scroll Position
		/// <summary>
		/// Return the maximum position that can be scrolled
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected int MaximumVScroll
		{
			get
			{
				if (VScrollBarVisible)
					return mVScrollBar.Maximum - (mVScrollBar.LargeChange - 1);
				else
					return 0;
			}
		}
		/// <summary>
		/// Return the minimum position that can be scrolled
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected int MinimumVScroll
		{
			get
			{
				return 0;
			}
		}
		/// <summary>
		/// Return the minimum position that can be scrolled
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected int MinimumHScroll
		{
			get
			{
				return 0;
			}
		}
		/// <summary>
		/// Return the maximum position that can be scrolled
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected int MaximumHScroll
		{
			get
			{
				if (HScrollBarVisible)
					return mHScrollBar.Maximum - (mHScrollBar.LargeChange - 1);
				else
					return 0;
			}
		}

		#endregion

		#region Layout Events
		/// <summary>
		/// OnLayout Method
		/// </summary>
		/// <param name="levent"></param>
		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (levent.AffectedControl != this)
			{
				base.OnLayout(levent);
				return;
			}

			RecalcCustomScrollBars();

			base.OnLayout(levent);
		}

		private int mSuspendedCount = 0;
		public new void SuspendLayout()
		{
			base.SuspendLayout();

			mSuspendedCount++;
		}
		public new void ResumeLayout()
		{
			base.ResumeLayout();

			if (mSuspendedCount > 0)
				mSuspendedCount--;
		}
		public new void ResumeLayout(bool performLayout)
		{
			base.ResumeLayout(performLayout);

			if (mSuspendedCount > 0)
				mSuspendedCount--;
		}
		public new void PerformLayout()
		{
			PerformLayout(this, null);
		}
		public new void PerformLayout(Control affectedControl,string affectedProperty)
		{
			base.PerformLayout(affectedControl, affectedProperty);
		}
		public bool IsSuspended()
		{
			return mSuspendedCount > 0;
		}
		#endregion

		#region ScrollChangeEvent

		private int m_OldVScrollValue = 0;
		private void VScroll_Change(object sender, EventArgs e)
		{
			OnVScrollPositionChanged(new ScrollPositionChangedEventArgs(-mVScrollBar.Value,-m_OldVScrollValue));

			InvalidateScrollableArea();

			m_OldVScrollValue = mVScrollBar.Value;
		}
		private int m_OldHScrollValue = 0;
		private void HScroll_Change(object sender, EventArgs e)
		{
			OnHScrollPositionChanged(new ScrollPositionChangedEventArgs(-mHScrollBar.Value,-m_OldHScrollValue));

			InvalidateScrollableArea();

			m_OldHScrollValue = mHScrollBar.Value;
		}
		

		/// <summary>
		/// Fired when the scroll vertical posizion change
		/// </summary>
		public event ScrollPositionChangedEventHandler VScrollPositionChanged;
		/// <summary>
		/// Fired when the scroll vertical posizion change
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnVScrollPositionChanged(ScrollPositionChangedEventArgs e)
		{
			if (VScrollPositionChanged!=null)
				VScrollPositionChanged(this,e);
		}

		/// <summary>
		/// Fired when the scroll horizontal posizion change
		/// </summary>
		public event ScrollPositionChangedEventHandler HScrollPositionChanged;
		/// <summary>
		/// Fired when the scroll horizontal posizion change
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnHScrollPositionChanged(ScrollPositionChangedEventArgs e)
		{
			if (HScrollPositionChanged!=null)
				HScrollPositionChanged(this,e);
		}


		#endregion

		#region Scroll PageDown/Up/Right/Left/LineUp/Down/Right/Left
		/// <summary>
		/// Scroll the page down
		/// </summary>
		public virtual void CustomScrollPageDown()
		{
			if (VScrollBarVisible)
				mVScrollBar.Value = Math.Min(mVScrollBar.Value + mVScrollBar.LargeChange, mVScrollBar.Maximum-mVScrollBar.LargeChange);
		}
		//MICK(11)
		/// <summary>
		/// Scroll the page down to line
		/// </summary>
		public virtual void CustomScrollPageToLine(int line)
		{
			if (VScrollBarVisible)
				if (mVScrollBar.Value!=line)
				mVScrollBar.Value = Math.Min(line, mVScrollBar.Maximum);
		}
		//MICK(12)
		/// <summary>
		/// Scroll the page down to line
		/// </summary>
		public virtual bool IsMaxPage()
		{
			return (mVScrollBar.Value == mVScrollBar.Maximum);
		}
		

		/// <summary>
		/// Scroll the page up
		/// </summary>
		public virtual void CustomScrollPageUp()
		{
			if (VScrollBarVisible)
				mVScrollBar.Value = Math.Max(mVScrollBar.Value - mVScrollBar.LargeChange, mVScrollBar.Minimum);
		}
		/// <summary>
		/// Scroll the page right
		/// </summary>
		public virtual void CustomScrollPageRight()
		{
			if (HScrollBarVisible)
				mHScrollBar.Value = Math.Min(mHScrollBar.Value + mHScrollBar.LargeChange, mHScrollBar.Maximum-mHScrollBar.LargeChange);
		}
		/// <summary>
		/// Scroll the page left
		/// </summary>
		public virtual void CustomScrollPageLeft()
		{
			if (HScrollBarVisible)
				mHScrollBar.Value = Math.Max(mHScrollBar.Value - mHScrollBar.LargeChange, mHScrollBar.Minimum);
		}
		public virtual void CustomScrollWheel(int rotationDelta)
		{
			if (rotationDelta >= 120 || rotationDelta <= -120)
			{
				if (VScrollBarVisible)
				{
					Point current = CustomScrollPosition;
					int newY = current.Y +
						SystemInformation.MouseWheelScrollLines * VScrollBar.SmallChange * -Math.Sign(rotationDelta);

					//check that the value is between max and min
					if (newY < 0)
						newY = 0;
					if (newY > MaximumVScroll)
						newY = MaximumVScroll;

					CustomScrollPosition = new Point(current.X, newY);
				}
			}
		}


		/// <summary>
		/// Scroll the page down one line
		/// </summary>
		public virtual void CustomScrollLineDown()
		{
			if (VScrollBarVisible)
				mVScrollBar.Value = Math.Min(mVScrollBar.Value + mVScrollBar.SmallChange, mVScrollBar.Maximum);
		}
		/// <summary>
		/// Scroll the page up one line
		/// </summary>
		public virtual void CustomScrollLineUp()
		{
			if (VScrollBarVisible)
				mVScrollBar.Value = Math.Max(mVScrollBar.Value - mVScrollBar.SmallChange, mVScrollBar.Minimum);
		}
		/// <summary>
		/// Scroll the page right one line
		/// </summary>
		public virtual void CustomScrollLineRight()
		{
			if (HScrollBarVisible)
				mHScrollBar.Value = Math.Min(mHScrollBar.Value + mHScrollBar.SmallChange, mHScrollBar.Maximum);
		}
		/// <summary>
		/// Scroll the page left one line
		/// </summary>
		public virtual void CustomScrollLineLeft()
		{
			if (HScrollBarVisible)
				mHScrollBar.Value = Math.Max(mHScrollBar.Value - mHScrollBar.SmallChange, mHScrollBar.Minimum);
		}

		#endregion
	}
}
