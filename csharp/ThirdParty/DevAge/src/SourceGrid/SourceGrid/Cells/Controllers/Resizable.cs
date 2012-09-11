using System;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Cells.Controllers
{
    /// <summary>
    /// Implement the mouse resize features of a cell. This behavior can be shared between multiple cells.
    /// </summary>
    public class Resizable : ControllerBase
    {
        /// <summary>
        /// Resize both width nd height behavior
        /// </summary>
        public readonly static Resizable ResizeBoth = new Resizable(CellResizeMode.Both);
        /// <summary>
        /// Resize width behavior
        /// </summary>
        public readonly static Resizable ResizeWidth = new Resizable(CellResizeMode.Width);
        /// <summary>
        /// Resize height behavior
        /// </summary>
        public readonly static Resizable ResizeHeight = new Resizable(CellResizeMode.Height);

        /// <summary>
        /// Border used to calculate the region where the resize is enabled.
        /// </summary>
        public DevAge.Drawing.RectangleBorder LogicalBorder = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(System.Drawing.Color.Black, 4), new DevAge.Drawing.BorderLine(System.Drawing.Color.Black, 4));

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_Mode"></param>
        public Resizable(CellResizeMode p_Mode)
        {
            m_ResizeMode = p_Mode;
        }

        private MouseCursor mWidthCursor = new MouseCursor(System.Windows.Forms.Cursors.VSplit, false);
        private MouseCursor mHeightCursor = new MouseCursor(System.Windows.Forms.Cursors.HSplit, false);

        #region IBehaviorModel Members

        public override void OnMouseDown(CellContext sender, MouseEventArgs e)
        {
            base.OnMouseDown(sender, e);

            m_IsHeightResize = false;
            m_IsWidthResize = false;

            Rectangle l_CellRect = sender.Grid.PositionToRectangle(sender.Position);
            Point mousePoint = new Point(e.X, e.Y);

            DevAge.Drawing.RectanglePartType partType = LogicalBorder.GetPointPartType(l_CellRect, mousePoint, out mDistanceFromBorder);

            if (((ResizeMode & CellResizeMode.Width) == CellResizeMode.Width) &&
                        partType == DevAge.Drawing.RectanglePartType.RightBorder)
                m_IsWidthResize = true;
            else if (((ResizeMode & CellResizeMode.Height) == CellResizeMode.Height) &&
                        partType == DevAge.Drawing.RectanglePartType.BottomBorder)
                m_IsHeightResize = true;
        }

        public override void OnMouseUp(CellContext sender, MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);

            m_IsWidthResize = false;
            m_IsHeightResize = false;
        }

        public override void OnMouseMove(CellContext sender, MouseEventArgs e)
        {
            base.OnMouseMove(sender, e);

            Rectangle cellRect = sender.Grid.PositionToRectangle(sender.Position);
            if (cellRect.IsEmpty)
                return;

            Point mousePoint = new Point(e.X, e.Y);

            float dummy;
            DevAge.Drawing.RectanglePartType partType = LogicalBorder.GetPointPartType(cellRect, mousePoint, out dummy);

            //sono già in fase di resizing
            if (sender.Grid.MouseDownPosition == sender.Position)
            {
                if (m_IsWidthResize)
                {
                    int newWidth = mousePoint.X - cellRect.Left;

                    if (newWidth > 0)
                        SetWidth(sender.Grid, sender.Position, (int)(newWidth + mDistanceFromBorder));

                    mWidthCursor.ApplyCursor(sender, e);
                    mHeightCursor.ResetCursor(sender, e);
                }
                else if (m_IsHeightResize)
                {
                    int newHeight = mousePoint.Y - cellRect.Top;

                    if (newHeight > 0)
                        SetHeight(sender.Grid, sender.Position, (int)(newHeight + mDistanceFromBorder));

                    mHeightCursor.ApplyCursor(sender, e);
                    mWidthCursor.ResetCursor(sender, e);
                }
            }
            else
            {
                if (partType == DevAge.Drawing.RectanglePartType.RightBorder && (ResizeMode & CellResizeMode.Width) == CellResizeMode.Width)
                    mWidthCursor.ApplyCursor(sender, e);
                else if (partType == DevAge.Drawing.RectanglePartType.BottomBorder && (ResizeMode & CellResizeMode.Height) == CellResizeMode.Height)
                    mHeightCursor.ApplyCursor(sender, e);
                else
                {
                    mWidthCursor.ResetCursor(sender, e);
                    mHeightCursor.ResetCursor(sender, e);
                }
            }
        }

        private void SetWidth(GridVirtual grid, Position position, int width)
        {
            Range range = grid.PositionToCellRange(position);
            int widthForCol = width / range.ColumnsCount;
            for (int c = range.Start.Column; c <= range.End.Column; c++)
                grid.Columns.SetWidth(c, widthForCol);
        }
        private void SetHeight(GridVirtual grid, Position position, int height)
        {
            Range range = grid.PositionToCellRange(position);
            int heightForCol = height / range.RowsCount;
            for (int r = range.Start.Row; r <= range.End.Row; r++)
                grid.Rows.SetHeight(r, heightForCol);
        }

        public override void OnMouseLeave(CellContext sender, EventArgs e)
        {
            base.OnMouseLeave(sender, e);

            mWidthCursor.ResetCursor(sender, e);
            mHeightCursor.ResetCursor(sender, e);
            m_IsWidthResize = false;
            m_IsHeightResize = false;
        }

        public override void OnDoubleClick(CellContext sender, EventArgs e)
        {
            base.OnDoubleClick(sender, e);

            Point currentPoint = sender.Grid.PointToClient(System.Windows.Forms.Control.MousePosition);
            Rectangle cellRect = sender.Grid.PositionToRectangle(sender.Position);

            float distance;
            DevAge.Drawing.RectanglePartType partType = LogicalBorder.GetPointPartType(cellRect, currentPoint, out distance);

            if ((ResizeMode & CellResizeMode.Width) == CellResizeMode.Width &&
                partType == DevAge.Drawing.RectanglePartType.RightBorder)
            {
                sender.Grid.Columns.AutoSizeColumn(sender.Position.Column);
            }
            else if ((ResizeMode & CellResizeMode.Height) == CellResizeMode.Height &&
                partType == DevAge.Drawing.RectanglePartType.BottomBorder)
            {
                sender.Grid.Rows.AutoSizeRow(sender.Position.Row);
            }
        }

        #endregion

        private CellResizeMode m_ResizeMode = CellResizeMode.Both;

        /// <summary>
        /// Resize mode of the cell
        /// </summary>
        public CellResizeMode ResizeMode
        {
            get { return m_ResizeMode; }
        }

        //		private Cursor m_ModelCursor = new Cursor();

        //Queste variabili indicano lo stato del resize (essendo usate però in un contesto di MouseEnter e MouseLeave possono essere tranquillamente condivise tra più cello o griglie, visto che il mouse in un dato momento sarà solo in una cella particolare, di un thread particolare, ...). Questo è un motivo in più per non poter usare questo controllo in multi thread (cosa che nessun controllo windows forms può fare ...)
        private bool m_IsWidthResize = false;
        private bool m_IsHeightResize = false;
        private float mDistanceFromBorder = 0;

        /// <summary>
        /// Indicates if the behavior is currently resizing a cell width
        /// </summary>
        public bool IsWidthResizing
        {
            get { return m_IsWidthResize; }
        }

        /// <summary>
        /// Indicates if the behavior is currently resizing a cell height
        /// </summary>
        public bool IsHeightResizing
        {
            get { return m_IsHeightResize; }
        }

        //		#region Support Functions
        //		/// <summary>
        //		/// 
        //		/// </summary>
        //		/// <param name="p_CellRectangle">A grid relative rectangle</param>
        //		/// <param name="p"></param>
        //		/// <returns></returns>
        //		public static bool IsInResizeHorRegion(Rectangle p_CellRectangle, Point p)
        //		{
        //			if (p.X >= p_CellRectangle.Right-c_MouseDelta && p.X <= p_CellRectangle.Right)
        //				return true;
        //			else
        //				return false;
        //		}
        //
        //		/// <summary>
        //		/// 
        //		/// </summary>
        //		/// <param name="p_CellRectangle">A grid relative rectangle</param>
        //		/// <param name="p"></param>
        //		/// <returns></returns>
        //		public static bool IsInResizeVerRegion(Rectangle p_CellRectangle, Point p)
        //		{
        //			if (p.Y >= p_CellRectangle.Bottom-c_MouseDelta && p.Y <= p_CellRectangle.Bottom)
        //				return true;
        //			else
        //				return false;
        //		}
        //
        //		private const int c_MouseDelta = 4;
        //
        //		#endregion
    }
}
