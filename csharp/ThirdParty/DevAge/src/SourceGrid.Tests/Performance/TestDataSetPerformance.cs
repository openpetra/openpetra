using SourceGrid.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;

namespace SourceGrid.Tests.Performance
{
	[TestFixture]
	public class TestDataSetPerformance
	{
		private DataTable m_custTable = null;
		Random rnd = new Random();
		string[] customerNames = {"Oracle", "Microsot", "Agile shop", "SourceGrid shop"};
		string[] typesOfSoftware = {"CRM", "Databases", "ERP", "SourceGrid component"};
		
		[Test, Explicit]
		public void CompareBuildingSpeeds()
		{
			m_custTable= new DataTable("Customers");
			// add columns
			m_custTable.Columns.Add( "id", typeof(int) );
			m_custTable.Columns.Add( "name", typeof(string) );
			m_custTable.Columns.Add( "address", typeof(string) );
			m_custTable.Columns.Add( "min", typeof(int) );
			m_custTable.Columns["min"].DefaultValue = 0;
			m_custTable.Columns.Add( "max", typeof(int) );
			m_custTable.Columns.Add( "Client name", typeof(string) );
			m_custTable.Columns.Add( "Type of software", typeof(string) );
			
			//m_custTable.Columns[ "id" ].Unique = true;
			//m_custTable.PrimaryKey = new DataColumn[] { m_custTable.Columns["id"] };
			
			//m_custTable.ColumnChanging += new DataColumnChangeEventHandler(custTable_ColumnChanging);
			//m_custTable.RowChanging += new DataRowChangeEventHandler(custTable_RowChanging);
			
			int maxRows = 1000000;
			using (IPerformanceCounter counter = new PerformanceCounter())
			{
				
				AddRows(0, maxRows);
				Console.WriteLine(string.Format(
					"Added {0} number of rows to datatable in {1} milisec",
					maxRows,
					counter.GetMilisec()));
			}
			
			List<object[]> objects = new List<object[]>();
			using (IPerformanceCounter counter = new PerformanceCounter())
			{
				for( int id = 0; id <= maxRows; id++ )
				{
					int valMin = rnd.Next(Int32.MinValue, Int32.MaxValue);
					objects.Add(
						new object[] { id,
							string.Format("customer{0}", id),
							string.Format("address{0}", id ),
							valMin,
							valMin + rnd.Next(1, 1000),
							customerNames[rnd.Next(0, customerNames.Length)],
							typesOfSoftware[rnd.Next(0, typesOfSoftware.Length)]
						} );
				}
				Console.WriteLine(string.Format(
					"Added {0} number of rows to list<object[]> in {1} milisec",
					maxRows,
					counter.GetMilisec()));
			}
		}
		
		private void custTable_ColumnChanging(object sender, DataColumnChangeEventArgs e)
		{
			if (e.Column.ColumnName.ToUpper() == "MIN" && e.ProposedValue is int && (int)e.ProposedValue < 0)
			{
				throw new ApplicationException("Min must be >= 0");
			}
		}
		
		private void custTable_RowChanging(object sender, DataRowChangeEventArgs e)
		{
			if (e.Action == System.Data.DataRowAction.Add || e.Action == System.Data.DataRowAction.Change)
			{
				if (e.Row["Min"] is System.DBNull || e.Row["Max"] is System.DBNull)
				{
				}
				else if ( (int)e.Row["Min"] > (int)e.Row["Max"])
				{
					throw new ApplicationException("Min must be <= Max");
				}
			}
		}
		
		private void AddRow(int id)
		{
			int valMin = rnd.Next(Int32.MinValue, Int32.MaxValue - 1);
			m_custTable.Rows.Add(
				new object[] { id,
					string.Format("customer{0}", id),
					string.Format("address{0}", id ),
					valMin,
					valMin + rnd.Next(1, 1000),
					customerNames[rnd.Next(0, customerNames.Length)],
					typesOfSoftware[rnd.Next(0, typesOfSoftware.Length)]
				} );
		}
		
		private void AddRows(int from, int to)
		{
			// add rows
			for( int id = from; id <= to; id++ )
			{
				AddRow(id);
			}
		}
	}
}
