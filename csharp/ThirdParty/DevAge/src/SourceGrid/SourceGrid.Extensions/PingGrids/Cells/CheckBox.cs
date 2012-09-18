
using System;

namespace SourceGrid.Extensions.PingGrids.Cells
{
	public class CheckBox : SourceGrid.Cells.Virtual.CheckBox
	{
	        public CheckBox()
		{
	            Model.AddModel(new PingGridValueModel());
		}
	}
}
