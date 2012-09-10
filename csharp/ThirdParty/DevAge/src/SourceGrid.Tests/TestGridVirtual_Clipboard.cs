/* */

using System;
using NUnit.Framework;
using SourceGrid.Cells;

namespace SourceGrid.Tests
{
	[TestFixture]
	public class TestGridVirtual_Clipboard
	{
		private Grid m_grid = null;
		private readonly int m_firstValue = 2;
		private readonly int m_secondValue = 5;
		
		[SetUp]
		public void SetUp()
		{
			m_grid = new Grid();
			m_grid.ClipboardMode = ClipboardMode.All;
			m_grid.Redim(2, 2);
			m_grid[0, 0] = new Cell(m_firstValue, typeof(int));
			m_grid[1, 1] = new Cell(m_secondValue, typeof(int));
			
			Assert.AreEqual(m_firstValue, m_grid[0, 0].Value);
			Assert.AreEqual(m_secondValue, m_grid[1, 1].Value);
		}
		
		[Test]
		public void PerformDelete()
		{
			m_grid.PerformDelete(new RangeRegion(new Range(0, 0, 1, 1)));
			Assert.AreEqual(0, m_grid[0, 0].Value);
			Assert.AreEqual(0, m_grid[1, 1].Value);
		}
		
		[Test]
		public void PerformCut()
		{
			m_grid.PerformCut(new RangeRegion(new Range(0, 0, 0, 0)));
			Assert.AreEqual(0, m_grid[0, 0].Value);
			
			m_grid.PerformPaste(new RangeRegion(new Range(1, 1, 1, 1)));
			Assert.AreEqual(m_firstValue, m_grid[1, 1].Value);
		}
		
		[Test]
		public void PerformCopyAndPaste()
		{
			m_grid.PerformCopy(new RangeRegion(new Range(0, 0, 0, 0)));
			m_grid.PerformPaste(new RangeRegion(new Range(1, 1, 1, 1)));
			Assert.AreEqual(m_firstValue, m_grid[1, 1].Value);
		}
	}
}
