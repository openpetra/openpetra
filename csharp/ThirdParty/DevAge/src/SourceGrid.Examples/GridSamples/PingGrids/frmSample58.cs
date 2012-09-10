using System;
using System.ComponentModel;
using System.Data;

using SourceGrid.Examples.Threading;
using SourceGrid.Extensions.PingGrids;
using SourceGrid.PingGrid.Backends.DSet;
using WindowsFormsSample.GridSamples.PingGrids;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample3.
	/// </summary>
	[Sample("SourceGrid - PingGrid", 58, "PingGrid - DataSet backend")]
	public class frmSample58 : System.Windows.Forms.Form
	{
		private PingGrid grid;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private DataTable m_custTable = null;
		Random rnd = new Random();
		string[] customerNames = {"FireBird", "MySQL", "PostGre", "DivanDB", "CouchDB"};
		
		public frmSample58()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grid = new PingGrid();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                          | System.Windows.Forms.AnchorStyles.Left)
			                                                         | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.AutoStretchColumnsToFitWidth = false;
			this.grid.AutoStretchRowsToFitHeight = false;
			this.grid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.grid.Location = new System.Drawing.Point(12, 12);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(516, 368);
			this.grid.SpecialKeys = SourceGrid.GridSpecialKeys.Default;
			this.grid.TabIndex = 0;
			// 
			// frmSample3
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(540, 391);
			this.Controls.Add(this.grid);
			this.Name = "frmSample3";
			this.Text = "Cell Editors, Specials Cells, Formatting and Image";
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			m_custTable= new DataTable("Customers");
			m_custTable.Columns.Add( "Id", typeof(int) );
			m_custTable.Columns.Add( "Name", typeof(string) );
			AddRows(1, 1000000);
			
			grid.Columns.Add("Id", "Id property", typeof(int));
			grid.Columns.Add("Name", "Name property", typeof(string));
			
			
			grid.DataSource = new PingDataSet(m_custTable.DefaultView);
			grid.Columns.StretchToFit();
		}

		private void AddRow( int id)
		{
			m_custTable.Rows.Add(
				new object[] { id,
					customerNames[rnd.Next(0, customerNames.Length)]
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
