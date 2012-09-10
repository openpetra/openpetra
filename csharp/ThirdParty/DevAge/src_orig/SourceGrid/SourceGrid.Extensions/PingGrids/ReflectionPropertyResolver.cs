
using System;

namespace SourceGrid.Extensions.PingGrids
{
	public class ReflectionPropertyResolver : IPropertyResolver
	{
		public static ReflectionPropertyResolver SharedInstance = new ReflectionPropertyResolver();
		
		public object ReadValue(object obj, string propertyPath)
		{
			var propertyInfo = obj.GetType().GetProperty(propertyPath);
			if (propertyInfo == null)
				return string.Empty;
			return propertyInfo.GetValue(obj, null);
		}
		
	}
}
