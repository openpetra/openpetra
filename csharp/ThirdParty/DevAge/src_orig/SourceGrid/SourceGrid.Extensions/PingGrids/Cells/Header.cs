
using System;
using SourceGrid.Cells.Models;

namespace SourceGrid.Extensions.PingGrids.Cells
{
    /// <summary>
    /// A cell used for the top/left cell when using DataGridRowHeader.
    /// </summary>
    public class Header : SourceGrid.Cells.Virtual.Header
    {
        public Header()
        {
            Model.AddModel(new NullValueModel());
        }
    }
}
