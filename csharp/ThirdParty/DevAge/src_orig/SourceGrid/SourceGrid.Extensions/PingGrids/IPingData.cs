using System;
using System.ComponentModel;

namespace SourceGrid.Extensions.PingGrids
{
	public interface IPingData
	{
		int Count {get;}
		bool AllowSort{get;set;}
		void ApplySort(string propertyName, bool ascending);
		
		object GetItemValue(int index, string propertyName);
	}
}
