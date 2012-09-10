using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample18.
	/// </summary>
	[Sample("SourceGrid - Standard features", 19, "Grid background")]
	public class frmSample19 : System.Windows.Forms.Form
	{
		private SourceGrid.Grid grid1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample19()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmSample19));
			this.grid1 = new SourceGrid.Grid();
			this.SuspendLayout();
			// 
			// grid1
			// 
			this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grid1.AutoStretchColumnsToFitWidth = false;
			this.grid1.AutoStretchRowsToFitHeight = false;
			this.grid1.CustomSort = false;
			this.grid1.Location = new System.Drawing.Point(12, 8);
			this.grid1.Name = "grid1";
			this.grid1.Size = new System.Drawing.Size(264, 256);
			this.grid1.TabIndex = 0;
			// 
			// frmSample19
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 271);
			this.Controls.Add(this.grid1);
			this.Name = "frmSample19";
			this.Text = "GridBackGround";
			this.Load += new System.EventHandler(this.frmSample19_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmSample19_Load(object sender, System.EventArgs e)
		{
			grid1.Redim(20, 20);

            grid1.BackgroundImage = Properties.Resources.BackGround;

			Random rnd = new Random();

			SourceGrid.Cells.Views.Cell transparentView = new SourceGrid.Cells.Views.Cell();
			transparentView.BackColor = Color.Transparent;
			SourceGrid.Cells.Views.Cell semiTransparentView = new SourceGrid.Cells.Views.Cell();
			semiTransparentView.BackColor = Color.FromArgb(50, Color.Blue);

			for (int r = 0; r < grid1.RowsCount; r++)
				for (int c = 0; c < grid1.ColumnsCount; c++)
				{
					if (rnd.Next(0,100) < 50)
						grid1[r,c] = new SourceGrid.Cells.Cell("Opaque");
					else if (rnd.Next(0,100) < 75)
					{
						SourceGrid.Cells.Cell l_Cell = new SourceGrid.Cells.Cell("Transparent");
						l_Cell.View = transparentView;
						grid1[r,c] = l_Cell;
					}
					else
					{
						SourceGrid.Cells.Cell l_Cell = new SourceGrid.Cells.Cell("Transparent 50");
						l_Cell.View = semiTransparentView;
						grid1[r,c] = l_Cell;
					}
				}

            grid1.AutoSizeCells();
		}
	}
}
