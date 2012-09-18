using System;

namespace SourceGrid.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class GridBase : DevAge.ComponentModel.Controller.IController
	{
		public void Attach(object objectToObserve)
		{
			OnAttach((GridVirtual)objectToObserve);
		}
		protected abstract void OnAttach(GridVirtual grid);

		public void Detach(object objectToObserve)
		{
			OnDetach((GridVirtual)objectToObserve);
		}
		protected abstract void OnDetach(GridVirtual grid);
	}
}
