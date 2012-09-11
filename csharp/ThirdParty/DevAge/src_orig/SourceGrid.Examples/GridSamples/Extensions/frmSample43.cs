using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample9.
	/// </summary>
	[Sample("SourceGrid - Extensions", 43, "Advanced Data Binding - DataGrid 3, validation rules, clipboard")]
	public class frmSample43 : System.Windows.Forms.Form
	{
		private SourceGrid.DataGrid dataGrid;
		private System.Windows.Forms.Label label1;
		private const int numberOfrows = 20000;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample43()
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
			this.dataGrid.Size = new System.Drawing.Size(492, 184);
			this.dataGrid.SpecialKeys = SourceGrid.GridSpecialKeys.Default;
			this.dataGrid.TabIndex = 15;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			                                                           | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(4, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(492, 40);
			this.label1.TabIndex = 16;
			this.label1.Text = "DataTable row validations: Id column is a primary key unique not null; Min must b" +
				"e < Max. Column validation: Min must be >= 0";
			// 
			// frmSample43
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(500, 242);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dataGrid);
			this.Name = "frmSample43";
			this.Text = "Advanced Data Binding - DataGrid 3, validation rules";
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
			DataTable custTable = new DataTable("Customers");
			// add columns
			custTable.Columns.Add( "id", typeof(int) );
			custTable.Columns.Add( "name", typeof(string) );
			custTable.Columns.Add( "address", typeof(string) );
			custTable.Columns.Add( "min", typeof(int) );
			custTable.Columns["min"].DefaultValue = 0;
			custTable.Columns.Add( "max", typeof(int) );
			

			// set PrimaryKey
			custTable.Columns[ "id" ].Unique = true;
			custTable.PrimaryKey = new DataColumn[] { custTable.Columns["id"] };

			custTable.ColumnChanging += new DataColumnChangeEventHandler(custTable_ColumnChanging);
			custTable.RowChanging += new DataRowChangeEventHandler(custTable_RowChanging);


			Random rnd = new Random();
			// add ten rows
			for( int id=1; id <= numberOfrows; id++ )
			{
				int valMin = rnd.Next(0, numberOfrows);

				custTable.Rows.Add(
					new object[] { id,
						string.Format("customer{0}", id),
						string.Format("address{0}", id ),
						valMin,
						valMin + rnd.Next(1, 1000),
					} );
			}

			
			dataGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			dataGrid.Selection.EnableMultiSelection = true;
			dataGrid.DataSource = new DevAge.ComponentModel.BoundDataView(custTable.DefaultView);

			dataGrid.Columns.AutoSizeView();


			//TODO Drag and drop
			//dataGrid.GridController.AddController(SourceGrid.Controllers.SelectionDrag.Copy);
			//dataGrid.GridController.AddController(SourceGrid.Controllers.SelectionDrop.Default);

			dataGrid.ClipboardMode = SourceGrid.ClipboardMode.All;
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