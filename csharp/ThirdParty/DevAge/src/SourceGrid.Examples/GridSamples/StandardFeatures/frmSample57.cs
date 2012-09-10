using System;
using System.Drawing;
using SourceGrid.Cells;
using SourceGrid.Selection;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample17.
	/// </summary>
	[Sample("SourceGrid - Standard features", 57, "Selection with hidden rows")]
	public class frmSample57 : System.Windows.Forms.Form
	{
		private SourceGrid.Grid grid1;
		private System.ComponentModel.Container components = null;
	
		public frmSample57()
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
			this.grid1 = new SourceGrid.Grid();
			this.SuspendLayout();
			// 
			// grid1
			// 
			this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                           | System.Windows.Forms.AnchorStyles.Left)
			                                                          | System.Windows.Forms.AnchorStyles.Right)));
			this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.grid1.Name = "grid1";
			this.grid1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
			this.grid1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
			this.grid1.Size = new System.Drawing.Size(432, 365);
			this.grid1.TabIndex = 0;
			this.grid1.TabStop = true;
			this.grid1.ToolTipText = "";

			// 
			// frmSample17
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 371);
			this.Controls.Add(this.grid1);
			this.Name = "frmSample17";
			this.Text = "Missing middle rows";
			this.Load += new System.EventHandler(this.frmSample17_Load);
			this.ResumeLayout(false);
	
		}
		#endregion
	
		private void frmSample17_Load(object sender, System.EventArgs e)
		{
			grid1.Redim(100, 40);
			grid1.FixedColumns = 0;
			grid1.FixedRows = 0;
	
			grid1[0,0] = new SourceGrid.Cells.Header(null);
			for (int c = 1; c < grid1.ColumnsCount; c++)
			{
				SourceGrid.Cells.ColumnHeader header = new SourceGrid.Cells.ColumnHeader("Header " + c.ToString());
				header.AutomaticSortEnabled = false;
	
				//header.ColumnSelectorEnabled = true;
				//header.ColumnFocusEnabled = true;
	
				grid1[0, c] = header;
			}
	
			Random rnd = new Random();
			for (int r = 1; r < grid1.RowsCount; r++)
			{
				grid1[r,0] = new SourceGrid.Cells.RowHeader("Header " + r.ToString());
				for (int c = 1; c < grid1.ColumnsCount; c++)
				{
					if (rnd.NextDouble() > 0.20)
					{
						grid1[r,c] = new SourceGrid.Cells.Cell(r*c, typeof(int));
					}
					else
						grid1[r,c] = null;
				}
			}
			
			var selection = grid1.Selection as SelectionBase;
	
			for (int i = 0; i < 100; i++)
				grid1[i, 0] = new Cell(i);
	
			for (int i = 30; i < 70; i++)
				grid1.Rows.ShowRow(i, false); 
			
		}
	
		public SelectionBase Selection
		{
			get
			{
				return grid1.Selection as SelectionBase;
			}
		}
	}
}
