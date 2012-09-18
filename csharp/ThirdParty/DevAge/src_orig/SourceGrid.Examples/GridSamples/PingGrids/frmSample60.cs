
using System;
using System.Windows.Forms;
using FluentNHibernate.Cfg;
using Microsoft.Isam.Esent.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using SourceGrid.Examples.Threading;
using SourceGrid.PingGrid.Backend.Essent;
using SourceGrid.PingGrid.Backend.NHibernate;

namespace WindowsFormsSample.GridSamples.PingGrids
{
	[Sample("SourceGrid - PingGrid", 60, "PingGrid - NHibernate backend with FireBird")]
	public partial class frmSample60 : Form
	{
		string[] customerNames = {"FireBird", "MySQL", "PostGre", "DivanDB", "CouchDB"};
		Random rnd = new Random();
		NHibernatePingData<Track> source = null;
		
		public SessionFactoryManager SessionFactoryManager{get;set;}
		public FirebirdEmbeddedPreparer FirebirdEmbeddedPreparer {get;set;}
		public EmptyFirebirdDatabasePreparer EmptyFirebirdDatabasePreparer {get;set;}
		
		
		
		public frmSample60()
		{
			
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			ServiceFactory.Init(this);
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
			if (SessionFactoryManager.CreateSessionFactory() == null)
			{
				this.Close();
				return;
			}
			
			source = new NHibernatePingData<Track>(SessionFactoryManager.SessionFactory);
			
			AddColumns();
			
			this.toolStripStatusLabel1.Text = "Grid not ready";
			
			pingGrid1.DataSource = source;
			pingGrid1.Columns.StretchToFit();
			
			pingGrid1.VScrollPositionChanged += delegate { UpdateCount(); };
			toolStripMenuItem1.Click += this.AddMoreRowsClicked;
			
			
			// we must show form prior calling multi-threaded code
			this.Show();
			if (pingGrid1.DataSource.Count == 0)
			{
				AddRows(0, 5000);
			}
			
			InvalidateGrid();
		}

		private void AddColumns()
		{
			foreach (var prop in typeof(Track).GetProperties())
			{
				pingGrid1.Columns.Add(prop.Name, string.Format("{0} property", prop.Name), prop.PropertyType);
			}
		}
		
		void UpdateCount()
		{
			UpdateCountInternal(pingGrid1.DataSource.Count);
		}
		
		void UpdateCountInternal(int count)
		{
			if (pingGrid1.Rows.FirstVisibleScrollableRow == null)
				return;
			int row = pingGrid1.Rows.FirstVisibleScrollableRow.Value;
			this.toolStripStatusLabel1.Text = string.Format("Viewing record {0}/ {1}", row, pingGrid1.DataSource.Count);
		}

		
		private void AddRows(int from, int to)
		{
			var operation = new InsertRowsOperation(this, from, to);
			var form = new ProgressBarForm();
			var insert = new RowInsertCounter(form);
			
			operation.Progress += insert.Add;
			operation.Cancelled += delegate { InvalidateGrid(); };
			operation.Completed += delegate { InvalidateGrid(); };
			operation.Failed += delegate { InvalidateGrid(); };
			
			new ExecuteWithProgressBar(operation, form).Run();
		}
		
		void AddMoreRowsClicked(object sender, EventArgs e)
		{
			var max = pingGrid1.DataSource.Count + 1;
			AddRows(max, max + 50000);
		}

		void InvalidateGrid()
		{
			source.Invalidate();
			UpdateCount();
			pingGrid1.RecalcCustomScrollBars();
		}
	}
	


}
