
using System;

namespace SourceGrid.Extensions.PingGrids.Cells
{
	public class Link : SourceGrid.Cells.Virtual.Link
	{
	        public Link()
		{
	            Model.AddModel(new PingGridValueModel());
		}
	}
}
