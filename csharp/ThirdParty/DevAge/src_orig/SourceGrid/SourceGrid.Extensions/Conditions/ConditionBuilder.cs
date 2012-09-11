using SourceGrid.Cells.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGrid.Conditions
{
    public static class ConditionBuilder
    {
        public static ICondition AlternateView(
                                            IView view,
                                            System.Drawing.Color alternateBackcolor,
                                            System.Drawing.Color alternateForecolor)
        {
            SourceGrid.Cells.Views.IView viewAlternate = (SourceGrid.Cells.Views.IView)view.Clone();
            viewAlternate.BackColor = alternateBackcolor;
            viewAlternate.ForeColor = alternateForecolor;

            SourceGrid.Conditions.ConditionView condition =
                        new SourceGrid.Conditions.ConditionView(viewAlternate);

            condition.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
                                    {
                                        return (gridRow & 1) == 1;
                                    };

            return condition;
        }
    }
}
