
using System;

namespace SourceGrid.Extensions.PingGrids.Cells
{
	public class Image : SourceGrid.Cells.Virtual.Image
	{
	        public Image()
		{
	            Model.AddModel(new PingGridValueModel());
		}
	}
}
