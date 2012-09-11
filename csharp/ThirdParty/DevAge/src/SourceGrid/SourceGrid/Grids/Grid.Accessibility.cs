using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using SourceGrid.Cells;  

namespace SourceGrid
{
   public partial class Grid
   {
      /// <summary>
      /// Overriden to return a custom accessibility object instance. 
      /// </summary>
      /// <returns>AccessibleObject for the grid control.</returns>
      protected override AccessibleObject CreateAccessibilityInstance()
      {
         return new GridAccessibleObject(this); 
      }

      /// <summary>
      /// Provides information about the grid control in the acessibility tree. 
      /// </summary>
      public class GridAccessibleObject : Control.ControlAccessibleObject
      {
         private Grid grid;

         /// <summary>
         /// Creates a new GridAccessibleObject
         /// </summary>
         /// <param name="owner">Owner</param>
         public GridAccessibleObject(Grid owner) : base(owner)
         {
            grid = owner;
         }

         /// <summary>
         /// Returns the role of the grid control
         /// </summary>
         public override AccessibleRole Role
         {
            get { return AccessibleRole.Table; }
         }

         /// <summary>
         /// Retrieves the accessible name of the grid control
         /// </summary>
         public override string Name
         {
            get { return grid.Name; } 
         }

         /// <summary>
         /// Retrieves the accessibility object of a row in the grid.  
         /// </summary>
         /// <param name="index">Index of child</param>
         /// <returns>AccessibleObject of a child element</returns>
         public override AccessibleObject GetChild(int index)
         {
            return new GridRowAccessibleObject(grid.Rows[index], this); 
         }

         /// <summary>
         /// Returns the number of children
         /// </summary>
         /// <returns>The number of rows in the grid.</returns>
         public override int GetChildCount()
         {
            return grid.RowsCount; 
         }
      }

      /// <summary>
      /// Provides for information about grid rows in the acessibility tree. 
      /// </summary>
      public class GridRowAccessibleObject : AccessibleObject
      {
         private GridRow gridRow;
         private GridAccessibleObject parent;

         /// <summary>
         /// Creates a new GridRowAccessibleObject
         /// </summary>
         /// <param name="gridRow">The grid row</param>
         /// <param name="parent">The grid row's parent grid</param>
         public GridRowAccessibleObject(GridRow gridRow, GridAccessibleObject parent)
            : base()
         {
            this.gridRow = gridRow;
            this.parent = parent; 
         }

         /// <summary>
         /// Gets the bounding rectangle in screen coordinates. 
         /// </summary>
         public override System.Drawing.Rectangle Bounds
         {
            get
            {
               // Return empty rectangle if row is not visible 
               if (!gridRow.Visible)
                  return System.Drawing.Rectangle.Empty;
               
               int vScroll = 0;
               if (gridRow.Grid.VerticalScroll.Enabled)
                  vScroll = gridRow.Grid.VScrollBar.Value;

               // Check if the row is before the current scroll position 
               if (gridRow.Index < vScroll)
                  return System.Drawing.Rectangle.Empty; 
               
               // Get the row height 
               int rowHeight = gridRow.Height;

               int hScroll = 0;
               if (gridRow.Grid.HorizontalScroll.Enabled)
                  hScroll = gridRow.Grid.HScrollBar.Value;

               // Calculate row width 
               int rowWidth = 0;
               for (int i = hScroll; i < gridRow.Grid.Columns.Count; i++)
               {
                  GridColumns cols = (GridColumns) gridRow.Grid.Columns;
                  rowWidth += cols[i].Width; 
               }
               
               // Get the row x position 
               int x = parent.Bounds.X; 

               // Calculate the row y position
               int y = parent.Bounds.Top; 
               for (int i = vScroll; i < gridRow.Index; i++)
               {
                  GridRows rows = (GridRows)gridRow.Grid.Rows;
                  y += rows[i].Height;
               }
               
               System.Drawing.Rectangle rowRect = new System.Drawing.Rectangle(x, y, rowWidth, rowHeight);

               // Double check if row is inside current grid view 
               if (parent.Bounds.IntersectsWith(rowRect)) 
                  return rowRect;
               else 
                  return System.Drawing.Rectangle.Empty;
            }
         }

         /// <summary>
         /// Returns the name of the grid row. 
         /// </summary>
         public override string Name
         {
            get { return "Row " + gridRow.Index; }
         }

         /// <summary>
         /// Returns the role of the grid row.  
         /// </summary>
         public override AccessibleRole Role
         {
            get { return AccessibleRole.Row; }
         }

         /// <summary>
         /// Returns the parent grid of the grid row.  
         /// </summary>
         public override AccessibleObject Parent
         {
            get { return parent; }
         }

         /// <summary>
         /// Returns a child cell of the row.  
         /// </summary>
         /// <param name="index">Index of child</param>
         /// <returns>AccessibleObject of a child element</returns>
         public override AccessibleObject GetChild(int index)
         {
            ICellVirtual[] cells = gridRow.Grid.GetCellsAtRow(gridRow.Index);
            if (index < cells.Length)
            {
               Cell cell = (Cell)cells[index];

               if (cell == null)
                  return null; 

               return new GridRowCellAccessibleObject(cell, this);
               
            }
            else
            {
               Cell cell = (Cell)cells[cells.Length - 1];
               return new GridRowCellAccessibleObject(cell, this); 
            }
         }

         /// <summary>
         /// Returns the number of children. 
         /// </summary>
         /// <returns>Number of cells in a row</returns>
         public override int GetChildCount()
         {
            return gridRow.Grid.GetCellsAtRow(gridRow.Index).Length; 
         }
      }

      /// <summary>
      /// Provides information about grid cells in the accessibility tree.  
      /// </summary>
      public class GridRowCellAccessibleObject : AccessibleObject
      {
         Cell cell; 
         GridRowAccessibleObject parent; 

         /// <summary>
         /// Creates a new GridRowCellAccessibleObject
         /// </summary>
         /// <param name="cell">The grid cell</param>
         /// <param name="parent">The parent row of the cell</param>
         public GridRowCellAccessibleObject(Cell cell, GridRowAccessibleObject parent) : base()
         {
            this.cell = cell;
            this.parent = parent;
         }

         /// <summary>
         /// Gets the bounding rectange in screen coordinates. 
         /// </summary>
         public override System.Drawing.Rectangle Bounds
         {
            get
            {
               // If the parent row isn't visible, neither is the cell
               if (parent.Bounds == System.Drawing.Rectangle.Empty)
                  return System.Drawing.Rectangle.Empty;

               int hScroll = 0;
               if (cell.Grid.HorizontalScroll.Enabled)
                  hScroll = cell.Grid.HScrollBar.Value;

               // If we scrolled horizontally past the cell it won't be visible anymore
               if (cell.Column.Index < hScroll)
                  return System.Drawing.Rectangle.Empty;

               int cellWidth = cell.Column.Width;
               int cellHeight = cell.Row.Height;

               int x = parent.Bounds.X;

               
               for (int i = hScroll; i < cell.Column.Index; i++)
               {
                  GridColumns cols = (GridColumns)cell.Grid.Columns;
                  x += cols[i].Width;
               }

               int y = parent.Bounds.Y;

               System.Drawing.Rectangle cellRect = new System.Drawing.Rectangle(x, y, cellWidth, cellHeight); 

               // Double check cell if cell is inside current grid view
               if (parent.Bounds.IntersectsWith(cellRect))
                  return cellRect;
               else 
                  return System.Drawing.Rectangle.Empty;
            }
         }

         /// <summary>
         /// Returns the role of the grid cell. 
         /// </summary>
         public override AccessibleRole Role
         {
            get { return AccessibleRole.Cell; }
         }

         /// <summary>
         /// Returns the name of the cell.
         /// </summary>
         /// <remarks>
         /// If display text not found, returns a default name containing the column index.
         /// </remarks>
         public override string Name
         {
            get
            {
               if (cell.DisplayText != null)
                  return cell.DisplayText;
               return "Column " + cell.Column.Index; 
            }
         }

         /// <summary>
         /// Set or get the value of the cell.
         /// </summary>
         public override string Value
         {
            get { return cell.DisplayText; }
            set { cell.Value = value; }
         }

         /// <summary>
         /// Returns the parent row of the cell. 
         /// </summary>
         public override AccessibleObject Parent
         {
            get { return parent; }
         }
      }
   }
}
