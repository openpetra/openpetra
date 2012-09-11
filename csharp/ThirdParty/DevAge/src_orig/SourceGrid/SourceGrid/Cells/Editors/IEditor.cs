using System;
using System.Windows.Forms;

namespace SourceGrid3.Cells.Editors
{
	/// <summary>
	/// Class used for editing operation, string conversion and value formatting. Can be assigned to the Editor property of a cell.
	/// </summary>
	public interface IEditor : DevAge.ComponentModel.Validator.IValidator
	{
		#region Editing
		/// <summary>
		/// Cell in editing, if null no cell is in editing state
		/// </summary>
		CellContext EditCellContext
		{
			get;
		}
		/// <summary>
		/// Cell in editing, if null no cell is in editing state
		/// </summary>
		Cells.ICellVirtual EditCell
		{
			get;
		}
		/// <summary>
		/// Cell in editing, if Empty no cell is in editing state
		/// </summary>
		Position EditPosition
		{
			get;
		}

		/// <summary>
		/// Start editing the cell passed. Do not call this method for start editing a cell, you must use Cell.StartEdit. For internal use only, use Cell.StartEdit.
		/// </summary>
		/// <param name="cellContext">Cell to start edit</param>
		void InternalStartEdit(CellContext cellContext);

		/// <summary>
		/// Terminate the edit action. For internal use only, use Cell.EndEdit.
		/// </summary>
		/// <param name="cancel">True to cancel the editing and return to normal mode, false to call automatically ApplyEdit and terminate editing</param>
		/// <returns>Returns true if the cell terminate the editing mode</returns>
		bool InternalEndEdit(bool cancel);


		/// <summary>
		/// Returns true if the cell is in editing state
		/// </summary>
		bool IsEditing
		{
			get;
		}

		/// <summary>
		/// Enable or disable the cell editor (if disable no edit is allowed). If false also not UI editing are blocked.
		/// </summary>
		bool EnableEdit
		{
			get;
			set;
		}

		/// <summary>
		/// Mode to edit the cell.
		/// </summary>
		EditableMode EditableMode
		{
			get;
			set;
		}

		/// <summary>
		/// Indicates if the draw of the cell when in editing mode is enabled.
		/// </summary>
		bool EnableCellDrawOnEdit
		{
			get;
		}
		#endregion

		#region Modify Functions
		/// <summary>
		/// Clear the value of the cell using the default value
		/// </summary>
		/// <param name="cellContext">Cell to change value</param>
		void ClearCell(CellContext cellContext);

		/// <summary>
		/// Change the value of the cell applying the rule of the current editor. Is recommend to use this method to simulate a edit operation and to validate the cell value using the current model.
		/// </summary>
		/// <param name="cellContext">Cell to change value</param>
		/// <param name="newValue"></param>
		/// <returns>returns true if the value passed is valid and has been applied to the cell</returns>
		bool SetCellValue(CellContext cellContext, object newValue);

		#endregion

		#region Validating
		/// <summary>
		/// Fired to check if the value specified by the user is allowed
		/// this event is fired after the ValidatingValue (use ValidatingValue to check if the value is compatible with the cell)
		/// </summary>
		event ValidatingCellEventHandler Validating;
		/// <summary>
		/// Fired after the value specified by the user inserited in the cell
		/// </summary>
		event CellContextEventHandler Validated;
		#endregion

		#region Conversion
		/// <summary>
		/// Check if the given string is error
		/// </summary>
		/// <param name="p_str"></param>
		/// <returns></returns>
		bool IsErrorString(string p_str);
		#endregion

		#region Send Keys
		/// <summary>
		/// Used to send some keyboard keys to the active editor. It is only valid when there is an active edit operations.
		/// </summary>
		/// <param name="key"></param>
		void SendCharToEditor(char key);
		#endregion
	}
}
