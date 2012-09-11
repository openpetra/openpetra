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
	/// <summary>
	/// This stest shows, that it is not possible for users to inherit
	/// from System.Windows.Forms.Control and override method Hide.
	/// This has some explicit repercussions of how we 
	/// call Hide methods in EditorControlBase.cs
	/// </summary>
	[TestFixture]
	public class TestOverride_With_New
	{
		
		[TestFixtureSetUp]
		public void Init()
		{
		}
		
		[Test]
		public void With_New()
		{
			MyClass1 class1 = new MyClassInherited();
			class1.Method();
			
			// this should be one, since we are calling method 
			// on type MyClass1
			Assert.AreEqual(1, class1.Count);
		}
		
		[Test]
		public void With_Reflection()
		{
			MyClass1 class1 = new MyClassInherited();
			
			Type myType = class1.GetType();
			myType.GetMethod("Method").Invoke(class1, null);
			
			// this should be two, since we are getting method dynamically
			// from an object, thus resulting in method MyClassInherited.Method()
			Assert.AreEqual(2, class1.Count);
		}
	}
}
