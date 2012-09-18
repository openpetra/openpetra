using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SourceGrid.Utils;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample4.
	/// </summary>
	[Sample("SourceGrid - Generic Samples", 4, "Real Grid performance")]
	public class frmSample04 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btLoad;
		private SourceGrid.Grid grid;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtCols;
		private System.Windows.Forms.TextBox txtRows;
		private CheckBox chkAddHeaders;
		private CheckBox chkAddColspan;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample04()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.toolStripStatusLabelBuildTime.Text = string.Empty;
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
			this.grid = new SourceGrid.Grid();
			this.btLoad = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtCols = new System.Windows.Forms.TextBox();
			this.txtRows = new System.Windows.Forms.TextBox();
			this.chkAddHeaders = new System.Windows.Forms.CheckBox();
			this.chkAddColspan = new System.Windows.Forms.CheckBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabelBuildTime = new System.Windows.Forms.ToolStripStatusLabel();
			this.grid.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                          | System.Windows.Forms.AnchorStyles.Left)
			                                                         | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.grid.Controls.Add(this.statusStrip1);
			this.grid.EnableSort = true;
			this.grid.Location = new System.Drawing.Point(4, 32);
			this.grid.Name = "grid";
			this.grid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
			this.grid.SelectionMode = SourceGrid.GridSelectionMode.Cell;
			this.grid.Size = new System.Drawing.Size(573, 404);
			this.grid.TabIndex = 3;
			this.grid.TabStop = true;
			this.grid.ToolTipText = "";
			// 
			// btLoad
			// 
			this.btLoad.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btLoad.Location = new System.Drawing.Point(292, 4);
			this.btLoad.Name = "btLoad";
			this.btLoad.Size = new System.Drawing.Size(75, 23);
			this.btLoad.TabIndex = 2;
			this.btLoad.Text = "Load - F3";
			this.btLoad.Click += new System.EventHandler(this.btLoad_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 23);
			this.label2.TabIndex = 12;
			this.label2.Text = "Rows:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(152, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 11;
			this.label1.Text = "Columns:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtCols
			// 
			this.txtCols.Location = new System.Drawing.Point(216, 4);
			this.txtCols.Name = "txtCols";
			this.txtCols.Size = new System.Drawing.Size(72, 20);
			this.txtCols.TabIndex = 1;
			this.txtCols.Text = "15";
			// 
			// txtRows
			// 
			this.txtRows.Location = new System.Drawing.Point(64, 4);
			this.txtRows.Name = "txtRows";
			this.txtRows.Size = new System.Drawing.Size(72, 20);
			this.txtRows.TabIndex = 0;
			this.txtRows.Text = "200";
			// 
			// chkAddHeaders
			// 
			this.chkAddHeaders.AutoSize = true;
			this.chkAddHeaders.Location = new System.Drawing.Point(396, 9);
			this.chkAddHeaders.Name = "chkAddHeaders";
			this.chkAddHeaders.Size = new System.Drawing.Size(86, 17);
			this.chkAddHeaders.TabIndex = 13;
			this.chkAddHeaders.Text = "Add headers";
			this.chkAddHeaders.UseVisualStyleBackColor = true;
			// 
			// chkAddColspan
			// 
			this.chkAddColspan.AutoSize = true;
			this.chkAddColspan.Location = new System.Drawing.Point(488, 10);
			this.chkAddColspan.Name = "chkAddColspan";
			this.chkAddColspan.Size = new System.Drawing.Size(86, 17);
			this.chkAddColspan.TabIndex = 14;
			this.chkAddColspan.Text = "Add Colspan";
			this.chkAddColspan.UseVisualStyleBackColor = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			                                 	this.toolStripStatusLabelBuildTime});
			this.statusStrip1.Location = new System.Drawing.Point(0, 380);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(571, 22);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabelBuildTime
			// 
			this.toolStripStatusLabelBuildTime.Name = "toolStripStatusLabelBuildTime";
			this.toolStripStatusLabelBuildTime.Size = new System.Drawing.Size(61, 17);
			this.toolStripStatusLabelBuildTime.Text = "build time";
			// 
			// frmSample04
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(581, 439);
			this.Controls.Add(this.chkAddHeaders);
			this.Controls.Add(this.chkAddColspan);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtCols);
			this.Controls.Add(this.txtRows);
			this.Controls.Add(this.btLoad);
			this.Controls.Add(this.grid);
			this.KeyPreview = true;
			this.Name = "frmSample04";
			this.Text = "Grid Performance";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSample4_KeyDown);
			this.grid.ResumeLayout(false);
			this.grid.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelBuildTime;
		private System.Windows.Forms.StatusStrip statusStrip1;
		#endregion

		private void btLoad_Click(object sender, System.EventArgs e)
		{
			using (var counter = new PerformanceCounter())
			{
				grid.Redim(0, 0);
				//Visual properties shared between all the cells
				SourceGrid.Cells.Views.Cell view = new SourceGrid.Cells.Views.Cell();
				view.BackColor = Color.Snow;

				//Editor (IDataModel) shared between all the cells
				SourceGrid.Cells.Editors.TextBox editor = new
					SourceGrid.Cells.Editors.TextBox(typeof(string));

				grid.Redim(int.Parse(txtRows.Text), int.Parse(txtCols.Text));

				if (chkAddHeaders.Checked && grid.RowsCount > 0 &&
				    grid.ColumnsCount > 0)
				{
					grid.FixedRows = 1;
					grid.FixedColumns = 1;

					for (int r = grid.FixedRows; r < grid.RowsCount; r++)
						grid[r, 0] = new SourceGrid.Cells.RowHeader(r);

					for (int c = grid.FixedColumns; c < grid.ColumnsCount; c++)
					{
						SourceGrid.Cells.ColumnHeader header = new
							SourceGrid.Cells.ColumnHeader(c);
						header.AutomaticSortEnabled = false;
						grid[0, c] = header;
					}
					grid[0, 0] = new SourceGrid.Cells.Header();
				}
				else
				{
					grid.FixedRows = 0;
					grid.FixedColumns = 0;
				}

				for (int r = grid.FixedRows; r < grid.RowsCount; r++)
					for (int c = grid.FixedColumns; c < grid.ColumnsCount; c++)
				{
					grid[r, c] = new SourceGrid.Cells.Cell(r.ToString() + "," +
					                                       c.ToString());
					grid[r, c].Editor = editor;
					grid[r, c].View = view;
					var span = 3;
					if (chkAddColspan.Checked && (c + span < grid.ColumnsCount))
					{
						if (r % 2 == 0)
						{
							grid[r, c].ColumnSpan = span;
							c += span;
						}
					}
				}
				grid.Selection.Focus(new SourceGrid.Position(0, 0), true);
				this.toolStripStatusLabelBuildTime.Text = string.Format(
					"Rows added in {0} ms", counter.GetMilisec());
			}
		}

		private void frmSample4_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F3)
			{
				btLoad_Click(btLoad, EventArgs.Empty);
				e.Handled = true;
			}
		}

	}
}
