using SourceGrid.Cells.Controllers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Cells
{
	/// <summary>
	/// Interface to represents a cell virtual (without position or value information).
	/// </summary>
	public interface ICellVirtual
	{
		#region Editor
		/// <summary>
		/// Editor of this cell and others cells. If null no edit is supported. 
		///  You can share the same model between many cells to optimize memory size. Warning Changing this property can affect many cells
		/// </summary>
		Editors.EditorBase Editor
		{
			get;
			set;
		}
		#endregion

		#region Controller
		/// <summary>
		/// Controller of the cell. Represents the actions of a cell.
		/// </summary>
		Controllers.ControllerContainer Controller
		{
			get;
		}

		/// <summary>
		/// Add the specified controller.
		/// </summary>
		/// <param name="controller"></param>
		void AddController(Controllers.IController controller);
		/// <summary>
		/// Remove the specifed controller
		/// </summary>
		/// <param name="controller"></param>
		void RemoveController(Controllers.IController controller);
		/// <summary>
		/// Find the specified controller. Returns null if not found.
		/// </summary>
		/// <param name="pControllerType"></param>
		/// <returns></returns>
		Controllers.IController FindController(Type pControllerType);

		/// <summary>
		/// Find the specified controller. Returns null if not found.
		/// </summary>
		/// <returns></returns>
		T FindController<T>() where T: class, IController;
		
		#endregion

		#region View
		/// <summary>
		/// Visual properties of this cell and other cell. You can share the VisualProperties between many cell to optimize memory size.
		/// Warning Changing this property can affect many cells
		/// </summary>
		Views.IView View
		{
			get;
			set;
		}
		#endregion

		#region Model
		/// <summary>
		/// Model that contains the data of the cells. Cannot be null.
		/// </summary>
		Models.ModelContainer Model
		{
			get;
			set;
		}
		#endregion

		#region Copy
		/// <summary>
		/// Create a shallow copy of the current object. Note that this is not a deep clone, all the reference are the same.
		/// Use internally MemberwiseClone method.
		/// </summary>
		/// <returns></returns>
		ICellVirtual Copy();
		#endregion
	}
}
