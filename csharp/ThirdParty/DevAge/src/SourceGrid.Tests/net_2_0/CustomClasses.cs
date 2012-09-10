/*
 * Created by SharpDevelop.
 * User: darius.damalakas
 * Date: 2009.07.08
 * Time: 16:12
 * 
 * 
 */

using System;
using NUnit.Framework;

namespace SourceGrid.Tests.net_2_0
{
	public class MyClass1
	{
		private int m_count = 0;
		
		public int Count {
			get { return m_count; }
			set { m_count = value; }
		}
		
		protected void Inc()
		{
			m_count ++;
		}
		
		public void Method()
		{
			Inc();
		}
	}
	
	public class MyClassInherited : MyClass1
	{
		public new void Method()
		{
			base.Method();
			Inc();
		}
	}
	

}
