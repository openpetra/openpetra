using SourceGrid.Cells;
using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGrid.Conditions
{
    public interface ICondition
    {
        bool Evaluate(DataGridColumn column, int gridRow, object itemRow);

        ICellVirtual ApplyCondition(ICellVirtual cell);
    }
}
