using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample3.
	/// </summary>
	[Sample("SourceGrid - Advanced features", 52, "Printing support")]
	public class frmSample52 : System.Windows.Forms.Form
	{
		private SourceGrid.Grid grid;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample52()
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
			this.grid = new SourceGrid.Grid();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.grid.Location = new System.Drawing.Point(8, 40);
			this.grid.Name = "grid";
			this.grid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
			this.grid.SelectionMode = SourceGrid.GridSelectionMode.Cell;
			this.grid.Size = new System.Drawing.Size(276, 224);
			this.grid.TabIndex = 0;
			this.grid.TabStop = true;
			this.grid.ToolTipText = "";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 7);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(99, 27);
			this.button1.TabIndex = 1;
			this.button1.Text = "Print";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.PrintButtonClick);
			// 
			// frmSample52
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 271);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.grid);
			this.Name = "frmSample52";
			this.Text = "Printing Grid";
			this.Load += new System.EventHandler(this.frmSample14_Load);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button button1;
		#endregion

		private void frmSample14_Load(object sender, System.EventArgs e)
		{
			grid.BorderStyle = BorderStyle.FixedSingle;

			grid.ColumnsCount = 4;
			grid.FixedRows = 1;
			grid.Rows.Insert(0);

            SourceGrid.Cells.Editors.ComboBox cbEditor = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cbEditor.StandardValues = new string[]{"Value 1", "Value 2", "Value 3"};
            cbEditor.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.SingleClick | SourceGrid.EditableMode.AnyKey;

			grid[0, 0] = new SourceGrid.Cells.ColumnHeader("String");
			grid[0, 1] = new SourceGrid.Cells.ColumnHeader("DateTime");
			grid[0, 2] = new SourceGrid.Cells.ColumnHeader("CheckBox");
            grid[0, 3] = new SourceGrid.Cells.ColumnHeader("ComboBox");
            for (int r = 1; r < 10; r++)
			{
				grid.Rows.Insert(r);
				grid[r, 0] = new SourceGrid.Cells.Cell("Hello " + r.ToString(), typeof(string));
				grid[r, 1] = new SourceGrid.Cells.Cell(DateTime.Today, typeof(DateTime));
				grid[r, 2] = new SourceGrid.Cells.CheckBox(null, true);
                grid[r, 3] = new SourceGrid.Cells.Cell("Value 1", cbEditor);
                grid[r, 3].View = SourceGrid.Cells.Views.ComboBox.Default;
            }

            grid.AutoSizeCells();
		}
		
		void PrintButtonClick(object sender, EventArgs e)
		{
			PrintPreviewDialog dlg = new PrintPreviewDialog();
			SourceGrid.Exporter.GridPrintDocument pd = new SourceGrid.Exporter.GridPrintDocument(this.grid);
			pd.RangeToPrint = new SourceGrid.Range(0, 0, this.grid.Rows.Count - 1, this.grid.Columns.Count - 1);
			pd.PageHeaderText = "Print sample\t\tSourceGrid print document sample";
			pd.PageTitleText = "\tSample grid";
			pd.PageFooterText = "\tPage [PageNo] from [PageCount]";
			dlg.Document = pd;
			dlg.ShowDialog(this);
		}
	}
}
