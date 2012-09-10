using System;
using System.Collections.Generic;
using System.Drawing;

namespace SourceGrid
{
	public interface IRows
	{
		bool IsRowVisible(int row);
		
		
		
		void HideRow(int row);
		void ShowRow(int row);
		
		/// <summary>
		/// Use this method to show or hide row
		/// </summary>
		/// <param name="row"></param>
		/// <param name="isVisible"></param>
		void ShowRow(int row, bool isVisible);
	}
}
