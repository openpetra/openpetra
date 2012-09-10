/* */

using System;
using NUnit.Framework;
using SourceGrid.Cells.Controllers;
using DevAge.ComponentModel;
using SourceGrid.Cells.Models;

namespace SourceGrid.Tests.Cells.Models
{
	[TestFixture]
	public class TestValueModel
	{
		[Test]
		public void IsNewValueEqual()
		{
			// null and various combinations
			Assert.AreEqual(true, new ValueModel(null).IsNewValueEqual(null));
			Assert.AreEqual(false, new ValueModel(null).IsNewValueEqual(new object()));
			Assert.AreEqual(false, new ValueModel(new object()).IsNewValueEqual(null));
			Assert.AreEqual(false, new ValueModel(new object()).IsNewValueEqual(new object()));
			
			// various string combinations
			Assert.AreEqual(true, new ValueModel(string.Empty).IsNewValueEqual(null));
			Assert.AreEqual(true, new ValueModel(null).IsNewValueEqual(string.Empty));
			Assert.AreEqual(true, new ValueModel(string.Empty).IsNewValueEqual(string.Empty));
			Assert.AreEqual(false, new ValueModel("my string").IsNewValueEqual(string.Empty));
			Assert.AreEqual(true, new ValueModel("my string").IsNewValueEqual("my string"));
			
			// default success scenario
			Assert.AreEqual(true, new ValueModel(1).IsNewValueEqual(1));
			Assert.AreEqual(false, new ValueModel(1).IsNewValueEqual(2));
			
		}
	}
}
