using System;

namespace SourceGrid.Cells.Models
{
	/// <summary>
	/// A Model interface specific to contain the value of the cell.
	/// </summary>
	public interface IValueModel : IModel
	{
		#region GetValue, SetValue
		/// <summary>
		/// Get the value of the cell at the specified position
		/// </summary>
		/// <param name="cellContext"></param>
		/// <returns></returns>
		object GetValue(CellContext cellContext);

		/// <summary>
		/// Set the value of the cell at the specified position. This method must call OnValueChanging and OnValueChanged() event.
		/// </summary>
		/// <param name="cellContext"></param>
		/// <param name="p_Value"></param>
		void SetValue(CellContext cellContext, object p_Value);
		#endregion
	}
}

