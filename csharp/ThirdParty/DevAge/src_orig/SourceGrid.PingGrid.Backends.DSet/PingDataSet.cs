
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

using SourceGrid.Extensions.PingGrids;

namespace SourceGrid.PingGrid.Backends.DSet
{
	public class PingDataSet : IPingData
	{
		private DataView m_dataView = null;
		
		public PingDataSet(DataView dataView)
		{
			this.m_dataView = dataView;
		}
		
		public object GetItemValue(int index, string propertyName)
		{
			var row = m_dataView[index];
			var value = row[propertyName];
			return value;
		}
		
		
		public int Count {
			get {
				return m_dataView.Table.Rows.Count;
			}
		}
		
		public bool AllowSort {
			get {
				return true;
			}
			set {
			}
		}
		
		
		public void ApplySort(string propertyName, bool @ascending)
		{
			var builder = new StringBuilder();
			builder.Append(propertyName);
			if (ascending == true)
				builder.Append(" ASC");
			else
				builder.Append(" DESC");
			var s = builder.ToString();
			m_dataView.Sort = s;
		}
		
	}
}