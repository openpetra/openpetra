using SourceGrid.Utils;
using System;
using System.Data;
using System.Threading;

namespace WindowsFormsSample
{
	/// <summary>
	/// this sample breaks, if while data grid is still loading,
	/// you try to sort rows
	/// </summary>
	[Sample("SourceGrid - Performance", 53, "Datagrid - 1 000 000 rows, with asyncrhonous loading")]
	public class frmSample53 : System.Windows.Forms.Form
	{
		private SourceGrid.DataGrid dataGrid;
		private System.Windows.Forms.Label label1;
		private const int m_numberOfrows = 1000000;
		private DataTable m_custTable = null;
		Random rnd = new Random();
		bool closing = false;
		string[] customerNames = {"Oracle", "Microsot", "Agile shop", "SourceGrid shop"};
		string[] typesOfSoftware = {"CRM", "Databases", "ERP", "SourceGrid component"};
		
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		public frmSample53()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dataGrid = new SourceGrid.DataGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabelRowCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataGrid
			// 
			this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.dataGrid.DefaultWidth = 20;
			this.dataGrid.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
			this.dataGrid.FixedRows = 1;
			this.dataGrid.Location = new System.Drawing.Point(4, 48);
			this.dataGrid.Name = "dataGrid";
			this.dataGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			this.dataGrid.Size = new System.Drawing.Size(827, 381);
			this.dataGrid.TabIndex = 15;
			this.dataGrid.TabStop = true;
			this.dataGrid.ToolTipText = "";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(4, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(827, 40);
			this.label1.TabIndex = 16;
			this.label1.Text = "DataGrid performance. Adding 1 000 000 rows asyncrhonously. Internaly using " +
				" DataTable for storing data. This allows us to achieve better sorting performance " +
				" compared to using simple IList. However, the trade off is that loading the data " +
				" is comparatively slow (~20 seonds).";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStripStatusLabelRowCount,
									this.toolStripProgressBar1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 455);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(835, 22);
			this.statusStrip1.TabIndex = 18;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabelRowCount
			// 
			this.toolStripStatusLabelRowCount.Name = "toolStripStatusLabelRowCount";
			this.toolStripStatusLabelRowCount.Size = new System.Drawing.Size(168, 17);
			this.toolStripStatusLabelRowCount.Text = "toolStripStatusLabelRowCount";
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(500, 16);
			// 
			// frmSample53
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(835, 477);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dataGrid);
			this.Name = "frmSample53";
			this.Text = "Advanced Data Binding - DataGrid 3, validation rules";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelRowCount;
		private System.Windows.Forms.StatusStrip statusStrip1;
		#endregion
		
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
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
			
			// add rows
			AddRows(0, 100);
			UpdateTotalRowCount(null);
			
			dataGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			dataGrid.Selection.EnableMultiSelection = true;
			dataGrid.DataSource = new DevAge.ComponentModel.BoundDataView(m_custTable.DefaultView);
			dataGrid.Columns.AutoSizeView();

			dataGrid.ClipboardMode = SourceGrid.ClipboardMode.All;
			
			// HACK - disable checkinf of ilegal calls
			System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
			
			
			// ensure that thread is aborted, when we exit the form before 
			// the thread exits
			this.Closing += delegate 
			{  
				closing = true;
			};
			
			// disable sorting before the data is loaded
			dataGrid.DataSource.AllowSort = false;
			
			// start asynchronous row addition
			new Thread(CreateData).Start();
		}
		
		private void UpdateTotalRowCount(IPerformanceCounter counter)
		{
			if (counter == null)
			{
				this.toolStripStatusLabelRowCount.Text = string.Format(
					"Row count: {0}",
					m_custTable.Rows.Count);
				return;
			}
			
			this.toolStripStatusLabelRowCount.Text = string.Format(
				"Row count: {0}. added in {1} ms",
				m_custTable.Rows.Count,
				counter.GetMilisec());
		}
		
		
		private void AddRow( int id)
		{
			int valMin = rnd.Next(0, m_numberOfrows);
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
		
		private void CreateData()
		{
			this.toolStripProgressBar1.Maximum = m_numberOfrows;
			
			using (var counter = new PerformanceCounter())
			{
				dataGrid.SuspendLayout();
				for( int id = 101; id <= m_numberOfrows; id++ )
				{
					if (closing == true)
						return;
					AddRow(id);
					if (id % 15000 == 0)
					{
						this.toolStripProgressBar1.Value = id;
						//dataGrid.ResumeLayout();
						dataGrid.RecalcCustomScrollBars();
						UpdateTotalRowCount(counter);
						//dataGrid.SuspendLayout();
					}
					
					if (id % 100000 == 0)
					{
						dataGrid.ResumeLayout();
						//dataGrid.Refresh();
						dataGrid.SuspendLayout();
					}
				}
				//m_custTable.EndLoadData();
				dataGrid.ResumeLayout();
				dataGrid.RecalcCustomScrollBars();
				
				this.toolStripProgressBar1.Visible = false;
				UpdateTotalRowCount(counter);
				// enable sorting
				dataGrid.DataSource.AllowSort = true;
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
	}
}
