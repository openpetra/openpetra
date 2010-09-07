http://sourcegrid.codeplex.com/
SourceGrid 4.30
Jun 14 2010


Modifications Timotheus, to make navigation with cursor keys work, and click on checkbox, in Row SelectionMode
(see also http://sourcegrid.codeplex.com/workitem/7127):

SourceGrid_4_30_src\SourceGrid_4_30_src\SourceGrid\SourceGrid\Selection\SelectionBase.cs:

    protected Position m_ActivePosition = Position.Empty;

SourceGrid_4_30_src\SourceGrid_4_30_src\SourceGrid\SourceGrid\Selection\RowSelection.cs:

    public override void SelectRow(int row, bool select)
    {
        Range rowRange = Grid.Rows.GetRange(row);
        if (select && mList.IsSelectedRow(row) == false)
        {
            // if multi selection is false, remove all previously selected rows
            if (this.EnableMultiSelection == false)
            {
                Position Backup = ActivePosition;
                this.Grid.Selection.ResetSelection(false);
                m_ActivePosition = Backup;
            }
            // continue with adding selection
            mList.AddRange(rowRange);
            OnSelectionChanged(new RangeRegionChangedEventArgs(rowRange, Range.Empty));
        } else
            if (!select && mList.IsSelectedRow(row))
        {
            mList.RemoveRange(rowRange);
            OnSelectionChanged(new RangeRegionChangedEventArgs(Range.Empty, rowRange));
        }
    }
