using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsSample.GridSamples
{
	/// <summary>
	/// Summary description for frmSample24.
	/// </summary>
	[Sample("SourceGrid - Standard features", 24, "Clipboard and Delete selection")]
	public class frmSample24 : System.Windows.Forms.Form
	{
		private SourceGrid.Grid grid1;
		private SourceGrid.Grid grid2;
        private Label label1;
        private Label label2;
        private Label label3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample24()
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
            this.grid2 = new SourceGrid.Grid();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // grid1
            // 
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.grid1.Location = new System.Drawing.Point(12, 37);
            this.grid1.Name = "grid1";
            this.grid1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grid1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grid1.Size = new System.Drawing.Size(256, 295);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = true;
            this.grid1.ToolTipText = "";
            // 
            // grid2
            // 
            this.grid2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grid2.Location = new System.Drawing.Point(274, 37);
            this.grid2.Name = "grid2";
            this.grid2.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grid2.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grid2.Size = new System.Drawing.Size(266, 295);
            this.grid2.TabIndex = 1;
            this.grid2.TabStop = true;
            this.grid2.ToolTipText = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Grid with double editors";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(271, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Grid with string editors";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(356, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "In the grids below I have used the clipboard features (Grid.ClipboardMode)";
            // 
            // frmSample24
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(552, 344);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grid2);
            this.Controls.Add(this.grid1);
            this.Name = "frmSample24";
            this.Text = "Clipboard";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			grid1.Redim(10, 10);
			grid2.Redim(10, 10);
			Random rnd = new Random();

			for (int r = 0; r < grid1.RowsCount; r++)
				for (int c = 0; c < grid1.ColumnsCount; c++)
					grid1[r, c] = new SourceGrid.Cells.Cell(rnd.NextDouble(), typeof(double));

			for (int r = 0; r < grid2.RowsCount; r++)
				for (int c = 0; c < grid2.ColumnsCount; c++)
					grid2[r, c] = new SourceGrid.Cells.Cell("str" + r.ToString(), typeof(string));


            grid1.ClipboardMode = SourceGrid.ClipboardMode.All;
            grid2.ClipboardMode = SourceGrid.ClipboardMode.All;
		}

	}
}
