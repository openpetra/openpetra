
using System;
using System.Collections.Generic;
using Microsoft.Isam.Esent.Collections.Generic;
using SourceGrid.Extensions.PingGrids;

namespace SourceGrid.PingGrid.Backend.Essent
{
	public class EseentPingData<T> : IPingData
	{
		private PersistentDictionary<int, T> m_dict = null;
		
		/// <summary>
		/// Use this to inject some custom performance optimized
		/// property value reader.
		/// </summary>
		public IPropertyResolver PropertyResolver{get;set;}
		
		public EseentPingData(PersistentDictionary<int, T> persistentDictionary)
		{
			m_dict = persistentDictionary;
			PropertyResolver = ReflectionPropertyResolver.SharedInstance;
		}
		
		/// <summary>
		/// Uses IPropertyResolver to get value from object and position 
		/// indicated by index
		/// </summary>
		/// <param name="index"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public object GetItemValue(int index, string propertyName)
		{
			if (m_dict.ContainsKey(index) == false)
				return string.Empty;
			var obj = m_dict[index];
			var value = PropertyResolver.ReadValue(obj, propertyName);
			return value;
		}
		
		
		public int Count {
			get {
				return m_dict.Count;
			}
		}
		
		public bool AllowSort {
			get {
				return false;
			}
			set {
			}
		}
		
		
		public void ApplySort(string propertyName, bool @ascending)
		{
		}
		
	}
}