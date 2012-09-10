using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample9.
	/// </summary>
	[Sample("SourceGrid - Extensions", 9, "Data Binding - DataGrid SQL Binding")]
	public class frmSample9 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtConnectionString;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtSqlQuery;
		private System.Windows.Forms.Button btLoad;
		private System.Windows.Forms.Label lbRowsNumber;
		private SourceGrid.DataGrid dataGrid;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox chkEdit;
		private System.Windows.Forms.CheckBox chkMultiSelection;
		private System.Windows.Forms.CheckBox chkAllowNew;
        private CheckBox chkAllowDelete;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample9()
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
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSqlQuery = new System.Windows.Forms.TextBox();
            this.btLoad = new System.Windows.Forms.Button();
            this.lbRowsNumber = new System.Windows.Forms.Label();
            this.dataGrid = new SourceGrid.DataGrid();
            this.label3 = new System.Windows.Forms.Label();
            this.chkEdit = new System.Windows.Forms.CheckBox();
            this.chkMultiSelection = new System.Windows.Forms.CheckBox();
            this.chkAllowNew = new System.Windows.Forms.CheckBox();
            this.chkAllowDelete = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(4, 20);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(564, 20);
            this.txtConnectionString.TabIndex = 1;
            this.txtConnectionString.Text = "Driver={SQL Server};Server=LOCALHOST;Database=Northwind;Trusted_Connection=yes;";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(380, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Connection String ODBC (Default: SQLServer LOCALHOST.Northwind)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(380, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "SQL Query";
            // 
            // txtSqlQuery
            // 
            this.txtSqlQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSqlQuery.Location = new System.Drawing.Point(4, 60);
            this.txtSqlQuery.Multiline = true;
            this.txtSqlQuery.Name = "txtSqlQuery";
            this.txtSqlQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSqlQuery.Size = new System.Drawing.Size(368, 56);
            this.txtSqlQuery.TabIndex = 3;
            this.txtSqlQuery.Text = "select * from [Orders Qry]";
            // 
            // btLoad
            // 
            this.btLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btLoad.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btLoad.Location = new System.Drawing.Point(496, 92);
            this.btLoad.Name = "btLoad";
            this.btLoad.Size = new System.Drawing.Size(75, 23);
            this.btLoad.TabIndex = 5;
            this.btLoad.Text = "Load";
            this.btLoad.Click += new System.EventHandler(this.btLoad_Click);
            // 
            // lbRowsNumber
            // 
            this.lbRowsNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbRowsNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRowsNumber.Location = new System.Drawing.Point(4, 119);
            this.lbRowsNumber.Name = "lbRowsNumber";
            this.lbRowsNumber.Size = new System.Drawing.Size(168, 16);
            this.lbRowsNumber.TabIndex = 11;
            this.lbRowsNumber.Text = "Rows Number: YYYY";
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
            this.dataGrid.Location = new System.Drawing.Point(4, 172);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dataGrid.Size = new System.Drawing.Size(572, 284);
            this.dataGrid.TabIndex = 15;
            this.dataGrid.TabStop = true;
            this.dataGrid.ToolTipText = "";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(564, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "Changing this grid affect only the DataTable, the original database is never upda" +
                "ted.";
            // 
            // chkEdit
            // 
            this.chkEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkEdit.Checked = true;
            this.chkEdit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkEdit.Location = new System.Drawing.Point(384, 48);
            this.chkEdit.Name = "chkEdit";
            this.chkEdit.Size = new System.Drawing.Size(100, 24);
            this.chkEdit.TabIndex = 16;
            this.chkEdit.Text = "Editable";
            this.chkEdit.CheckedChanged += new System.EventHandler(this.chkEdit_CheckedChanged);
            // 
            // chkMultiSelection
            // 
            this.chkMultiSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMultiSelection.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkMultiSelection.Location = new System.Drawing.Point(384, 68);
            this.chkMultiSelection.Name = "chkMultiSelection";
            this.chkMultiSelection.Size = new System.Drawing.Size(100, 24);
            this.chkMultiSelection.TabIndex = 17;
            this.chkMultiSelection.Text = "Multi Selection";
            this.chkMultiSelection.CheckedChanged += new System.EventHandler(this.chkMultiSelection_CheckedChanged);
            // 
            // chkAllowNew
            // 
            this.chkAllowNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllowNew.Checked = true;
            this.chkAllowNew.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowNew.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkAllowNew.Location = new System.Drawing.Point(384, 92);
            this.chkAllowNew.Name = "chkAllowNew";
            this.chkAllowNew.Size = new System.Drawing.Size(100, 23);
            this.chkAllowNew.TabIndex = 18;
            this.chkAllowNew.Text = "Allow New";
            this.chkAllowNew.CheckedChanged += new System.EventHandler(this.chkAllowNew_CheckedChanged);
            // 
            // chkAllowDelete
            // 
            this.chkAllowDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllowDelete.Checked = true;
            this.chkAllowDelete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkAllowDelete.Location = new System.Drawing.Point(384, 115);
            this.chkAllowDelete.Name = "chkAllowDelete";
            this.chkAllowDelete.Size = new System.Drawing.Size(100, 23);
            this.chkAllowDelete.TabIndex = 19;
            this.chkAllowDelete.Text = "Allow Delete";
            this.chkAllowDelete.CheckedChanged += new System.EventHandler(this.chkAllowDelete_CheckedChanged);
            // 
            // frmSample9
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(580, 463);
            this.Controls.Add(this.chkAllowDelete);
            this.Controls.Add(this.chkAllowNew);
            this.Controls.Add(this.chkEdit);
            this.Controls.Add(this.txtSqlQuery);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.chkMultiSelection);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.lbRowsNumber);
            this.Controls.Add(this.btLoad);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmSample9";
            this.Text = "DataGrid binding";
            this.Load += new System.EventHandler(this.frmSample9_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void btLoad_Click(object sender, System.EventArgs e)
		{
			try
			{

				System.Data.DataSet dataset = new System.Data.DataSet();
				using (System.Data.Odbc.OdbcDataAdapter adapter = new System.Data.Odbc.OdbcDataAdapter(txtSqlQuery.Text, txtConnectionString.Text))
				{
					adapter.Fill(dataset);
				}

				//Debug Trace
				dataset.Tables[0].RowDeleted += new System.Data.DataRowChangeEventHandler(frmSample9_RowDeleted);
				dataset.Tables[0].RowChanged += new System.Data.DataRowChangeEventHandler(frmSample9_RowChanged);
				dataset.Tables[0].ColumnChanged += new System.Data.DataColumnChangeEventHandler(frmSample9_ColumnChanged);

				lbRowsNumber.Text = "Rows: " + dataset.Tables[0].Rows.Count;

				dataGrid.FixedColumns = 1;
				dataGrid.FixedRows = 1;
				dataGrid.Columns.Clear();

                DevAge.ComponentModel.BoundDataView bd = new DevAge.ComponentModel.BoundDataView(dataset.Tables[0].DefaultView);
                bd.AllowNew = chkAllowNew.Checked;
                bd.AllowEdit = chkEdit.Checked;
                bd.AllowDelete = chkAllowDelete.Checked;

                dataGrid.DataSource = bd;
				dataGrid.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.MinimumSize;
				dataGrid.Columns[0].Width = 20;

				dataGrid.Columns.AutoSizeView();
			}
			catch(Exception err)
			{
				DevAge.Windows.Forms.ErrorDialog.Show(this, err, "Error loading dataset");
			}
		}

		private void chkEdit_CheckedChanged(object sender, System.EventArgs e)
		{
            if (dataGrid.DataSource != null)
                dataGrid.DataSource.AllowEdit = chkEdit.Checked;
        }

		private void frmSample9_Load(object sender, System.EventArgs e)
		{
			txtSqlQuery.Text = "select * from [Orders Qry] \r\n /*select TOP 50000 * from [Orders Qry] a CROSS JOIN [Orders Qry]  b*/";
		}

		private void chkMultiSelection_CheckedChanged(object sender, System.EventArgs e)
		{
			dataGrid.Selection.EnableMultiSelection = chkMultiSelection.Checked;
		}

		private void chkAllowNew_CheckedChanged(object sender, System.EventArgs e)
		{
			if (dataGrid.DataSource != null)
				dataGrid.DataSource.AllowNew = chkAllowNew.Checked;
		}

		private void frmSample9_RowDeleted(object sender, System.Data.DataRowChangeEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("RowDeleted");
		}

		private void frmSample9_RowChanged(object sender, System.Data.DataRowChangeEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("RowChanged");
		}

		private void frmSample9_ColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("ColumnChanged");
		}

        private void chkAllowDelete_CheckedChanged(object sender, EventArgs e)
        {
            if (dataGrid.DataSource != null)
                dataGrid.DataSource.AllowDelete = chkAllowDelete.Checked;
        }
	}
}