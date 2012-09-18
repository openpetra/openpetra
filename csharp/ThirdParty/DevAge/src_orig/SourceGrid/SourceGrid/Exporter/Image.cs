using System;
using System.Drawing;

namespace SourceGrid.Exporter
{
    /// <summary>
    /// An utility class to export a grid to a csv delimited format file.
    /// </summary>
    public class Image
    {
        public Image()
        {
        }

        public virtual System.Drawing.Bitmap Export(GridVirtual grid, Range rangeToExport)
        {
            System.Drawing.Bitmap bitmap = null;

            try
            {
                System.Drawing.Size size = grid.RangeToSize(rangeToExport);

                bitmap = new System.Drawing.Bitmap(size.Width, size.Height);

                using (System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bitmap))
                {
                    Export(grid, graphic, rangeToExport, new System.Drawing.Point(0, 0));
                }
            }
            catch (Exception)
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }

                throw;
            }

            return bitmap;
        }

        public virtual void Export(GridVirtual grid, System.Drawing.Graphics graphics, 
                                Range rangeToExport, System.Drawing.Point destinationLocation)
        {
            if (rangeToExport.IsEmpty())
                return;

            System.Drawing.Point cellPoint = destinationLocation;

            using (DevAge.Drawing.GraphicsCache graphicsCache = new DevAge.Drawing.GraphicsCache(graphics))
            {
                for (int r = rangeToExport.Start.Row; r <= rangeToExport.End.Row; r++)
                {
                    int rowHeight = grid.Rows.GetHeight(r);

                    for (int c = rangeToExport.Start.Column; c <= rangeToExport.End.Column; c++)
                    {
                        Rectangle cellRectangle;
                        Position pos = new Position(r, c);

                        Size cellSize = new Size(grid.Columns.GetWidth(c), rowHeight);

                        Range range = grid.PositionToCellRange(pos);

                        //support for RowSpan or ColSpan 
                        //Note: for now I draw only the merged cell at the first position 
                        // (this can cause a problem if you export partial range that contains a partial merged cells)
                        if ( range.ColumnsCount > 1 || range.RowsCount > 1)
                        {
                            //Is the first position
                            if (range.Start == pos)
                            {
                                Size rangeSize = grid.RangeToSize(range);

                                cellRectangle = new Rectangle(cellPoint,
                                                              rangeSize);
                            }
                            else
                                cellRectangle = Rectangle.Empty;
                        }
                        else
                        {
                            cellRectangle = new Rectangle(cellPoint, cellSize);
                        }

                        if (cellRectangle.IsEmpty == false)
                        {
                            Cells.ICellVirtual cell = grid.GetCell(pos);
                            CellContext context = new CellContext(grid, pos, cell);
                            ExportCell(context, graphicsCache, cellRectangle);
                        }

                        cellPoint = new Point(cellPoint.X + cellSize.Width, cellPoint.Y);
                    }

                    cellPoint = new Point(destinationLocation.X, cellPoint.Y + rowHeight);
                }
            }
        }

        protected virtual void ExportCell(CellContext context, DevAge.Drawing.GraphicsCache graphics, System.Drawing.Rectangle rectangle)
        {
            if (context.Cell != null)
            {
                context.Cell.View.DrawCell(context, graphics, rectangle);
            }
        }
    }

}
