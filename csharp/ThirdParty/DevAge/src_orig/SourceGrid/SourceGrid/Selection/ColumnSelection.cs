using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGrid.Selection
{
    public class ColumnSelection : SelectionBase
    {
        public ColumnSelection()
        {
        }

        public override void BindToGrid(GridVirtual p_grid)
        {
            base.BindToGrid(p_grid);

            mDecorator = new Decorators.DecoratorSelection(this);
            Grid.Decorators.Add(mDecorator);
        }

        public override void UnBindToGrid()
        {
            Grid.Decorators.Remove(mDecorator);

            base.UnBindToGrid();
        }

        private Decorators.DecoratorSelection mDecorator;

        private List<int> mList = new List<int>();

        public override bool IsSelectedColumn(int column)
        {
            return mList.Contains(column);
        }

        public override void SelectColumn(int column, bool select)
        {
            if (select && mList.Contains(column) == false)
            {
                mList.Add(column);

                OnSelectionChanged(new RangeRegionChangedEventArgs(Grid.Columns.GetRange(column) , Range.Empty));
            }
            else if (!select && mList.Contains(column))
            {
                mList.Remove(column);

                OnSelectionChanged(new RangeRegionChangedEventArgs(Range.Empty, Grid.Columns.GetRange(column)));
            } 
        }

        public override bool IsSelectedRow(int row)
        {
            return false;
        }

        public override void SelectRow(int row, bool select)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsSelectedCell(Position position)
        {
            return IsSelectedColumn(position.Column);
        }

        public override void SelectCell(Position position, bool select)
        {
            SelectColumn(position.Column, select);
        }

        public override bool IsSelectedRange(Range range)
        {
            for (int c = range.Start.Column; c <= range.End.Column; c++)
            {
                if (IsSelectedColumn(c) == false)
                    return false;
            }

            return true;
        }

        public override void SelectRange(Range range, bool select)
        {
            for (int c = range.Start.Column; c <= range.End.Column; c++)
            {
                SelectColumn(c, select);
            }
        }

        protected override void OnResetSelection()
        {
            RangeRegion prevRange = GetSelectionRegion();

            mList.Clear();

            OnSelectionChanged(new RangeRegionChangedEventArgs(null, prevRange));
        }

        public override bool IsEmpty()
        {
            return mList.Count == 0;
        }

        public override RangeRegion GetSelectionRegion()
        {
            RangeRegion region = new RangeRegion();

            if (Grid.Rows.Count > 0)
            {
                foreach (int col in mList)
                {
                    region.Add(ValidateRange(new Range(Grid.FixedRows, col, Grid.Rows.Count - 1, col)));
                }
            }

            return region;
        }

        public override bool IntersectsWith(Range rng)
        {
            for (int c = rng.Start.Column; c <= rng.End.Column; c++)
            {
                if (IsSelectedColumn(c))
                    return true;
            }

            return false;
        }
    }
}
