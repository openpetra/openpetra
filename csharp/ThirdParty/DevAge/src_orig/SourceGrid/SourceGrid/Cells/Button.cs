using System;
using System.Windows.Forms;


namespace SourceGrid.Cells.Virtual
{
	/// <summary>
	/// A cell that rappresent a button 
	/// </summary>
	public abstract class Button : CellVirtual
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public Button()
		{
			View = Views.Button.Default;
			AddController(Controllers.MouseInvalidate.Default);
			//NOTE: I don't add the Unselectable controller because the Execute event of the Button it is not fired when the cell is not selected
		}
	}
}

namespace SourceGrid.Cells
{
	/// <summary>
	/// A cell that rappresent a button 
	/// </summary>
	public class Button : Cell
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_Value"></param>
		public Button(object p_Value):base(p_Value)
		{
			View = Views.Button.Default;
			AddController(Controllers.MouseInvalidate.Default);
			//NOTE: I don't add the Unselectable controller because the Execute event of the Button it is not fired when the cell is not selected
		}
	}
}
