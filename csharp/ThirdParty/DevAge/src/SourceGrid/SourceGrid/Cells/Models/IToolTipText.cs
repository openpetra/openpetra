using System;

namespace SourceGrid.Cells.Models
{
	/// <summary>
	/// Interface for informations about a tooltiptext
	/// </summary>
	public interface IToolTipText : IModel
	{
		/// <summary>
		/// Get the tooltiptext of the specified cell
		/// </summary>
		/// <param name="cellContext"></param>
		string GetToolTipText(CellContext cellContext);
	}
}
