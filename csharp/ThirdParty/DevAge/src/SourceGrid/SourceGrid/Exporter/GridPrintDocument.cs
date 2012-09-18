using SourceGrid.Cells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using DevAge.Drawing;

namespace SourceGrid.Exporter
{
	/// <summary>
	/// Grid print document.
	/// </summary>
	public class GridPrintDocument : PrintDocument
	{
		private GridVirtual m_Grid;

		private Cells.Views.Cell m_CellPrintView = new Cells.Views.Cell();
		/// <summary>
		/// Cell view when printing.
		/// </summary>
		public Cells.Views.Cell CellPrintView
		{
			get { return m_CellPrintView; }
		}

		private Cells.Views.Cell m_HeaderCellPrintView = new Cells.Views.Cell();
		/// <summary>
		/// Header cell view when printing.
		/// </summary>
		public Cells.Views.Cell HeaderCellPrintView
		{
			get { return m_HeaderCellPrintView; }
		}

		public GridPrintDocument(GridVirtual grid)
		{
			m_Grid = grid;
			// Default to empty background
			m_CellPrintView.BackColor = Color.Empty;
			m_HeaderCellPrintView.BackColor = Color.Empty;

			m_PageHeaderFont = new Font(grid.Font, FontStyle.Regular);
			m_PageTitleFont = new Font(grid.Font, FontStyle.Bold);
			m_PageFooterFont = new Font(grid.Font, FontStyle.Regular);
		}

		private Range m_RangeToPrint = Range.Empty;

		public Range RangeToPrint {
			get { return m_RangeToPrint; }
			set
			{
				if ( value.Start.Row < 0 || value.Start.Column < 0
				    || value.Start.Row >= m_Grid.Rows.Count || value.Start.Column >= m_Grid.Columns.Count )
				{
					throw new ArgumentOutOfRangeException("RangeToPrint");
				}
				m_RangeToPrint = value;
			}
		}

		#region Header, footer, title
		private Font m_PageHeaderFont = null;
		private string m_PageHeaderText = string.Empty;

		public Font PageHeaderFont {
			get { return m_PageHeaderFont; }
			set { m_PageHeaderFont = value; }
		}

		public string PageHeaderText {
			get { return m_PageHeaderText; }
			set { m_PageHeaderText = value; }
		}

		private Font m_PageFooterFont = null;
		private string m_PageFooterText = string.Empty;

		public Font PageFooterFont {
			get { return m_PageFooterFont; }
			set { m_PageFooterFont = value; }
		}

		public string PageFooterText {
			get { return m_PageFooterText; }
			set { m_PageFooterText = value; }
		}

		private Font m_PageTitleFont = null;
		private string m_PageTitleText = string.Empty;

		public Font PageTitleFont {
			get { return m_PageTitleFont; }
			set { m_PageTitleFont = value; }
		}

		public string PageTitleText {
			get { return m_PageTitleText; }
			set { m_PageTitleText = value; }
		}

		protected virtual float GetHeaderHeight(Graphics g)
		{
			if ( string.IsNullOrEmpty(this.PageHeaderText) )
				return 0;
			else
				return g.MeasureString(this.PageHeaderText, this.PageHeaderFont).Height * 1.5f;
		}

		protected virtual float GetTitleHeight(Graphics g)
		{
			if ( string.IsNullOrEmpty(this.PageTitleText) )
				return 0;
			else
				return g.MeasureString(this.PageTitleText, this.PageTitleFont).Height * 2f;
		}

		protected virtual float GetFooterHeight(Graphics g)
		{
			if ( string.IsNullOrEmpty(this.PageFooterText) )
				return 0;
			else
				return g.MeasureString(this.PageFooterText, this.PageFooterFont).Height * 1.5f;
		}
		
		private static string ReplaceValues(string text, int pageNo, int pageCount)
		{
			return text.Replace("[PageNo]", pageNo.ToString())
				.Replace("[PageCount]", pageCount.ToString());
		}

		private static void DrawText(Graphics g, string text, Font font, Brush brush, RectangleF clip, int pageNo, int pageCount)
		{
			if ( string.IsNullOrEmpty(text) )
				return;
			string[] s = text.Split('\t');
			StringFormat frmt = new StringFormat(StringFormat.GenericDefault);
			frmt.LineAlignment = StringAlignment.Center;
			frmt.Trimming = StringTrimming.Character;
			if ( ! string.IsNullOrEmpty(s[0]) )
			{
				frmt.Alignment = StringAlignment.Near;
				g.DrawString(ReplaceValues(s[0], pageNo, pageCount), font, brush, clip, frmt);
			}
			if ( s.GetLength(0) > 1 && ! string.IsNullOrEmpty(s[1]) )
			{
				frmt.Alignment = StringAlignment.Center;
				g.DrawString(ReplaceValues(s[1], pageNo, pageCount), font, brush, clip, frmt);
			}
			if ( s.GetLength(0) > 2 && ! string.IsNullOrEmpty(s[2]) )
			{
				frmt.Alignment = StringAlignment.Far;
				g.DrawString(ReplaceValues(s[2], pageNo, pageCount), font, brush, clip, frmt);
			}
		}

		protected virtual void DrawHeader(Graphics g, RectangleF clip, int pageNo, int pageCount)
		{
			DrawText(g, this.PageHeaderText, this.PageHeaderFont, Brushes.Black, clip, pageNo, pageCount);
		}

		protected virtual void DrawTitle(Graphics g, RectangleF clip, int pageNo, int pageCount)
		{
			DrawText(g, this.PageTitleText, this.PageTitleFont, Brushes.Black, clip, pageNo, pageCount);
		}

		protected virtual void DrawFooter(Graphics g, RectangleF clip, int pageNo, int pageCount)
		{
			DrawText(g, this.PageFooterText, this.PageFooterFont, Brushes.Black, clip, pageNo, pageCount);
		}
		#endregion

		private bool m_RepeatFixedRows = false;

		/// <summary>
		/// Property if fixed rows should be repeated on each page.
		/// </summary>
		public bool RepeatFixedRows
		{
			get { return m_RepeatFixedRows; }
			set { m_RepeatFixedRows = value; }
		}

		protected virtual int PrecalculatePageCount(PrintPageEventArgs e)
		{
			int pageCount = 1;
			int columnPageCount = 1;

			float totalWidth = e.MarginBounds.Width;
			float pageFirstColumn = RangeToPrint.Start.Column;
			for (int c = RangeToPrint.Start.Column; c <= RangeToPrint.End.Column; c++)
			{
				float colWidth = this.GetColumnWidth(e.Graphics, c);
				if ( c == pageFirstColumn && colWidth > e.MarginBounds.Width )
					colWidth = e.MarginBounds.Width;
				if ( totalWidth - colWidth >= 0 )
					totalWidth -= colWidth;
				else
				{
					columnPageCount++;
					totalWidth = e.MarginBounds.Width;
					pageFirstColumn = c;
					c--;
				}
			}

			float fixedRowHeight = 0;
			float totalHeight = e.MarginBounds.Height - m_HeaderHeight - m_TitleHeight - m_FooterHeight;
			for (int r = RangeToPrint.Start.Row; r <= RangeToPrint.End.Row; r++)
			{
				float rowHeight = this.GetRowHeight(e.Graphics, r);
				if ( RepeatFixedRows && r < m_Grid.ActualFixedRows - RangeToPrint.Start.Row )
					fixedRowHeight += rowHeight;
				if ( totalHeight - rowHeight >= 0 )
					totalHeight -= rowHeight;
				else
				{
					// If fixed rows takes whole page, disable fixed row repeating
					if ( r <= m_Grid.ActualFixedRows )
					{
						fixedRowHeight = 0;
						this.m_RepeatFixedRows = false;
					}
					totalHeight = e.MarginBounds.Height - m_HeaderHeight - m_FooterHeight - fixedRowHeight;
					pageCount++;
					r--;
				}
			}
			return pageCount * columnPageCount;
		}

		private int m_NextRowToPrint = 0;
		private int m_NextColumnToPrint = 0;
		private float m_HeaderHeight = 0;
		private float m_TitleHeight = 0;
		private float m_FooterHeight = 0;
		private int m_PageCount = 0;
		private int m_PageNo = 0;

		/// <summary>
		/// Return calculated page count.
		/// </summary>
		public int PageCount
		{
			get
			{
				if ( m_PageCount == 0 )
					throw new ArgumentNullException("Page count not yet calculated");
				return m_PageCount;
			}
		}

		protected override void OnBeginPrint(PrintEventArgs e)
		{
			base.OnBeginPrint(e);
			if (RangeToPrint.IsEmpty())
				RangeToPrint = new Range(0, 0, m_Grid.Rows.Count - 1, m_Grid.Columns.Count - 1);

			// Force no border
			m_CellPrintView.Border = RectangleBorder.NoBorder;
			m_HeaderCellPrintView.Border = RectangleBorder.NoBorder;

			m_NextRowToPrint = RangeToPrint.Start.Row;
			m_NextColumnToPrint = RangeToPrint.Start.Column;

			m_HeaderHeight = 0;
			m_TitleHeight = 0;
			m_FooterHeight = 0;
			m_PageCount = 0;
			m_PageNo = 0;
		}

		protected virtual float GetRowHeight(Graphics g, int row)
		{
			return (float)m_Grid.Rows.GetHeight(row);
		}

		protected virtual float GetColumnWidth(Graphics g, int column)
		{
			return (float)m_Grid.Columns.GetWidth(column);
		}

		private Pen m_BorderPen = null;
		
		protected virtual void DrawCell(Graphics g, CellContext ctx, RectangleF rect)
		{
			if ( ctx.Cell == null )
				return;
			if ( ctx.Cell is Cells.Virtual.ColumnHeader || ctx.Cell is Cells.ColumnHeader
			    || ctx.Cell is Cells.Virtual.RowHeader || ctx.Cell is Cells.RowHeader
			    || ctx.Cell is Cells.Virtual.Header || ctx.Cell is Cells.Header )
				m_HeaderCellPrintView.DrawCell(ctx, new GraphicsCache(g), rect);
			else
			{
				// Copy text alignment
				m_CellPrintView.TextAlignment = ctx.Cell.View.TextAlignment;
				// If cell view copy some more view options
				if ( ctx.Cell.View is SourceGrid.Cells.Views.Cell )
				{
					m_CellPrintView.AnchorArea = ((Cells.Views.Cell)ctx.Cell.View).AnchorArea;
					m_CellPrintView.ImageAlignment = ((Cells.Views.Cell)ctx.Cell.View).ImageAlignment;
					m_CellPrintView.ImageStretch = ((Cells.Views.Cell)ctx.Cell.View).ImageStretch;
					m_CellPrintView.Padding = ((Cells.Views.Cell)ctx.Cell.View).Padding;
					m_CellPrintView.TrimmingMode = ((Cells.Views.Cell)ctx.Cell.View).TrimmingMode;
					m_CellPrintView.WordWrap = ((Cells.Views.Cell)ctx.Cell.View).WordWrap;
				}
				m_CellPrintView.DrawCell(ctx, new GraphicsCache(g), rect);
			}
		}

		#region Helper methods
		private static void AddRange ( List<int> list, int from, int to )
		{
			for( int i = from; i <= to; i++ )
				list.Add(i);
		}

		private static bool ContainsInRange ( List<int> list, int from, int to )
		{
			foreach(int i in list)
				if ( i >= from && i <= to )
				return true;
			return false;
		}
		#endregion

		protected override void OnPrintPage(PrintPageEventArgs e)
		{
			base.OnPrintPage(e);
			if (m_PageCount == 0)
			{
				m_HeaderHeight = this.GetHeaderHeight(e.Graphics);
				m_TitleHeight = this.GetTitleHeight(e.Graphics);
				m_FooterHeight = this.GetFooterHeight(e.Graphics);
				m_PageCount = this.PrecalculatePageCount(e);
				m_PageNo = 1;
			}

			if (m_RangeToPrint.IsEmpty())
				return;

			if ( m_BorderPen == null )
				m_BorderPen = new Pen(Color.Black);

			RectangleF area = new RectangleF(e.MarginBounds.Left,
			                                 e.MarginBounds.Top + m_HeaderHeight + (m_NextRowToPrint == RangeToPrint.Start.Row ? m_TitleHeight : 0),
			                                 e.MarginBounds.Width,
			                                 e.MarginBounds.Height - m_HeaderHeight - (m_NextRowToPrint == RangeToPrint.Start.Row ? m_TitleHeight : 0) - m_FooterHeight);

			this.DrawHeader(e.Graphics, new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width, m_HeaderHeight),
			                m_PageNo, m_PageCount);

			if ( m_PageNo == 1 )
				this.DrawTitle(e.Graphics, new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top + m_HeaderHeight, e.MarginBounds.Width, m_TitleHeight),
				               m_PageNo, m_PageCount);

			List<int> columnHasTopBorder = new List<int>();
			List<int> rowHasLeftBorder = new List<int>();
			RangeCollection printedRanges = new RangeCollection();

			int pageFirstRow = m_NextRowToPrint;
			int pageFirstColumn = m_NextColumnToPrint;
			int pageLastColumn = RangeToPrint.End.Column;

			// Pre-calculate width of the table in current page
			float pageColumnWidth = 0;
			for ( int i = m_NextColumnToPrint; i <= RangeToPrint.End.Column; i++ )
			{
				float colWidth = this.GetColumnWidth(e.Graphics, i);
				if ( i == m_NextColumnToPrint && colWidth > area.Width )
					colWidth = area.Width;
				if ( pageColumnWidth + colWidth <= area.Width )
					pageColumnWidth += colWidth;
				else
					break;
			}

			// Support for fixed row repeat
			if ( RepeatFixedRows && m_Grid.ActualFixedRows > RangeToPrint.Start.Row )
				m_NextRowToPrint = RangeToPrint.Start.Row;

			float curY = area.Top;
			while (m_NextRowToPrint <= RangeToPrint.End.Row)
			{
				// If repeated rows are printed, resume printing next rows
				if ( RepeatFixedRows && m_NextRowToPrint >= m_Grid.ActualFixedRows && m_NextRowToPrint < pageFirstRow )
					m_NextRowToPrint = pageFirstRow;
				float rowHeight = this.GetRowHeight(e.Graphics, m_NextRowToPrint);
				// Check if row fits in current page
				if ( curY + rowHeight > area.Bottom )
					break;
				float curX = area.Left;
				while (m_NextColumnToPrint <= pageLastColumn)
				{
					float colWidth = this.GetColumnWidth(e.Graphics, m_NextColumnToPrint);
					// Check if column fits in current page
					if ( curX + colWidth > area.Right )
					{
						// If single column does not fit in page, force it
						if ( m_NextColumnToPrint == pageFirstColumn )
							colWidth = area.Right - curX;
						else
						{
							pageLastColumn = m_NextColumnToPrint - 1;
							break;
						}
					}
					RectangleF cellRectangle;
					Position pos = new Position(m_NextRowToPrint, m_NextColumnToPrint);
					Range range = m_Grid.PositionToCellRange(pos);

					// Check if cell is spanned
					if ( range.ColumnsCount > 1 || range.RowsCount > 1 )
					{
						Size rangeSize = m_Grid.RangeToSize(range);
						// Is the first position, draw allways
						if ( range.Start == pos )
						{
							cellRectangle = new RectangleF(curX, curY, rangeSize.Width, rangeSize.Height);
							printedRanges.Add(range);
						}
						else
						{
							// Draw only if this cell is not already drawn on current page
							if ( ! printedRanges.ContainsCell(pos) )
							{
								// Calculate offset
								float sX = curX;
								for (int i = pos.Column - 1; i >= range.Start.Column; i--)
								{
									float cw = this.GetColumnWidth(e.Graphics, i);
									sX -= cw;
								}
								float sY = curY;
								for ( int i = pos.Row - 1; i >= range.Start.Row; i-- )
								{
									float cw = this.GetRowHeight(e.Graphics, i);
									sY -= cw;
								}
								cellRectangle = new RectangleF(sX, sY, rangeSize.Width, rangeSize.Height);
								printedRanges.Add(range);
							}
							else
								cellRectangle = RectangleF.Empty;
						}
					}
					else
					{
						cellRectangle = new RectangleF(curX, curY, colWidth, rowHeight);
					}

					if ( ! cellRectangle.IsEmpty )
					{
						SourceGrid.Cells.ICellVirtual cell = this.m_Grid.GetCell(pos);
						if ( cell != null )
						{
							CellContext ctx = new CellContext(m_Grid, pos, cell);
							RectangleF clip = new RectangleF(Math.Max(cellRectangle.Left, area.Left),
							                                 Math.Max(cellRectangle.Top, area.Top),
							                                 Math.Min(cellRectangle.Right, area.Left + pageColumnWidth) - Math.Max(cellRectangle.Left, area.Left),
							                                 Math.Min(cellRectangle.Bottom, area.Bottom) - Math.Max(cellRectangle.Top, area.Top));
							Region prevClip = e.Graphics.Clip;
							try
							{
								e.Graphics.Clip = new Region(clip);
								this.DrawCell(e.Graphics, ctx, cellRectangle);
							}
							finally
							{
								// Restore clip region
								e.Graphics.Clip = prevClip;
							}
							// Check if left border can be drawn in current page
							if ( ! ContainsInRange(rowHasLeftBorder, range.Start.Row, range.End.Row)
							    && cellRectangle.Left >= area.Left )
							{
								e.Graphics.DrawLine(m_BorderPen, cellRectangle.Left, clip.Top, cellRectangle.Left, clip.Bottom);
								AddRange(rowHasLeftBorder, range.Start.Row, range.End.Row);
							}
							// Check if top border can be drawn in current page
							if ( ! ContainsInRange(columnHasTopBorder, range.Start.Column, range.End.Column)
							    && cellRectangle.Top >= area.Top )
							{
								e.Graphics.DrawLine(m_BorderPen, clip.Left,	cellRectangle.Top, clip.Right, cellRectangle.Top);
								AddRange(columnHasTopBorder, range.Start.Column, range.End.Column);
							}
							// Check if right border can be drawn in current page
							if ( cellRectangle.Right <= area.Right )
								e.Graphics.DrawLine(m_BorderPen, cellRectangle.Right, clip.Top, cellRectangle.Right, clip.Bottom);
							// Check if bottom border can be drawn in current page
							if ( cellRectangle.Bottom <= area.Bottom )
								e.Graphics.DrawLine(m_BorderPen, clip.Left, cellRectangle.Bottom, clip.Right, cellRectangle.Bottom);
						}
					}
					// Set next column position
					curX += colWidth;
					m_NextColumnToPrint++;
				}
				// Set next row Y position
				curY += rowHeight;
				m_NextRowToPrint++;
				m_NextColumnToPrint = pageFirstColumn;
			}
			// If we have not reached last column, we will continue on next page
			if ( pageLastColumn != RangeToPrint.End.Column )
			{
				m_NextRowToPrint = pageFirstRow;
				m_NextColumnToPrint = pageLastColumn + 1;
			}
			else
				m_NextColumnToPrint = RangeToPrint.Start.Column;

			this.DrawFooter(e.Graphics, new RectangleF(e.MarginBounds.Left, e.MarginBounds.Bottom - m_FooterHeight, e.MarginBounds.Width, m_FooterHeight),
			                m_PageNo, m_PageCount);
			
			// If we have not reached last row we will continue on next page
			e.HasMorePages = (m_NextRowToPrint <= RangeToPrint.End.Row);
			// If there are no more pages, release resources
			if ( e.HasMorePages )
				m_PageNo++;
		}

		protected override void OnEndPrint(PrintEventArgs e)
		{
			base.OnEndPrint(e);
			// Release resources
			if ( m_BorderPen != null )
			{
				m_BorderPen.Dispose();
				m_BorderPen = null;
			}
		}
	}
}
