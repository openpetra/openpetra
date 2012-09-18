using SourceGrid.Cells.Views;
using System;
using System.Collections.Generic;
using System.Text;
using SourceGrid.Cells;

namespace SourceGrid.Conditions
{
    public class ConditionView : ICondition
    {
        public ConditionView(IView view)
        {
            mView = view;
        }

        public delegate bool EvaluateFunctionDelegate(DataGridColumn column, int gridRow, object itemRow);

        public EvaluateFunctionDelegate EvaluateFunction;

        private IView mView;
        public IView View
        {
            get { return mView; }
        }

        #region ICondition Members
        public bool Evaluate(DataGridColumn column, int gridRow, object itemRow)
        {
            if (EvaluateFunction == null)
                return false;

            return EvaluateFunction(column, gridRow, itemRow);
        }

        public ICellVirtual ApplyCondition(ICellVirtual cell)
        {
            SourceGrid.Cells.ICellVirtual copied = cell.Copy();
            copied.View = View;

            return copied;
        }
        #endregion
    }
}
