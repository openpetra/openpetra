/* */

using System;
using NUnit.Framework;
using SourceGrid.Cells.Controllers;
using DevAge.ComponentModel;
using SourceGrid.Cells.Models;

namespace SourceGrid.Tests.Cells.Models
{
	[TestFixture]
	public class TestModelContainer
	{
		[Test]
		public void GetIModel()
		{
			IModel model = new SourceGrid.Cells.Models.CheckBox();
			Assert.AreEqual(model,
			                new ModelContainer()
			                .AddModel(model)
			                .FindModel(typeof(SourceGrid.Cells.Models.CheckBox)));
			
		}
	}
}
