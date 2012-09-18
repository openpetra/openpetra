
using System;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Isam.Esent.Collections.Generic;
using SourceGrid.PingGrid.Backend.Essent;

namespace WindowsFormsSample.GridSamples.PingGrids
{
	[Sample("SourceGrid - PingGrid", 59, "PingGrid - Essent backend")]
	public partial class frmSample59 : Form
	{
		string[] customerNames = {"FireBird", "MySQL", "PostGre", "DivanDB", "CouchDB"};
		PersistentDictionary<int, Customer> dict = null;
		Random rnd = new Random();
		
		public frmSample59()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
			dict = new PersistentDictionary<int, Customer>("EssentData");
			var essentSource = new EseentPingData<Customer>(dict);
			
			AddRows(1, 10000);
			
			pingGrid1.Columns.Add("TotalRevenue", "TotalRevenue property", typeof(int));
			pingGrid1.Columns.Add("Name", "Name property", typeof(string));
			
			
			pingGrid1.DataSource = essentSource;
			pingGrid1.Columns.StretchToFit();
			
			pingGrid1.VScrollPositionChanged += delegate { UpdateCount(); };
			toolStripMenuItem1.Click += this.ToolStripMenuItem1Click;
			
			UpdateCount();
		}

		void UpdateCount()
		{
			if (pingGrid1.Rows.FirstVisibleScrollableRow == null)
				return;
			int row = pingGrid1.Rows.FirstVisibleScrollableRow.Value;
			this.toolStripStatusLabel1.Text = string.Format("Viewing record {0}/ {1}", row, dict.Count);
		}

		private void AddRow( int id)
		{
			if (dict.ContainsKey(id))
				return;
			
			var cust = new Customer();
			cust.Name = customerNames[rnd.Next(0, customerNames.Length)];
			cust.TotalRevenue = id;
			
			dict.Add(id, cust);
		}
		
		private void AddRows(int from, int to)
		{
			if (dict.Count >= to)
				return;
			// add rows
			for( int id = from; id <= to; id++ )
			{
				AddRow(id);
			}
		}
		
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			var max = dict.Count + 1;
			AddRows(max, max + 50000);
			UpdateCount();
		}
	}
	

}
