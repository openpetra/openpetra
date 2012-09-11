using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample44.
	/// </summary>
	[Sample("SourceGrid - Advanced features", 44, "Right To Left Support, not yet completed...")]
	public class frmSample44 : System.Windows.Forms.Form
    {
        private SourceGrid.Grid grid1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample44()
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
            this.grid1.Location = new System.Drawing.Point(12, 12);
            this.grid1.Mirrored = true;
            this.grid1.Name = "grid1";
            this.grid1.Size = new System.Drawing.Size(268, 247);
            this.grid1.SpecialKeys = ((SourceGrid.GridSpecialKeys)(((((((SourceGrid.GridSpecialKeys.Arrows | SourceGrid.GridSpecialKeys.Tab)
                        | SourceGrid.GridSpecialKeys.PageDownUp)
                        | SourceGrid.GridSpecialKeys.Enter)
                        | SourceGrid.GridSpecialKeys.Escape)
                        | SourceGrid.GridSpecialKeys.Control)
                        | SourceGrid.GridSpecialKeys.Shift)));
            this.grid1.TabIndex = 0;
            // 
            // frmSample44
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 271);
            this.Controls.Add(this.grid1);
            this.Name = "frmSample44";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "Basic Grid";
            this.Load += new System.EventHandler(this.frmSample14_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmSample14_Load(object sender, System.EventArgs e)
		{
			grid1.BorderStyle = BorderStyle.FixedSingle;

			grid1.ColumnsCount = 3;
			grid1.FixedRows = 1;
			grid1.Rows.Insert(0);

            RTLColumnHeaderView viewHeader = new RTLColumnHeaderView();
            RTLCheckBoxView viewCheckBox = new RTLCheckBoxView();

			grid1[0, 0] = new SourceGrid.Cells.ColumnHeader("String");
            grid1[0, 0].View = viewHeader;
			grid1[0, 1] = new SourceGrid.Cells.ColumnHeader("DateTime");
            grid1[0, 1].View = viewHeader;
            grid1[0, 2] = new SourceGrid.Cells.ColumnHeader("CheckBox");
            grid1[0, 2].View = viewHeader;
            for (int r = 1; r < 10; r++)
			{
				grid1.Rows.Insert(r);
				grid1[r, 0] = new SourceGrid.Cells.Cell("Hello " + r.ToString(), typeof(string));
				grid1[r, 1] = new SourceGrid.Cells.Cell(DateTime.Today, typeof(DateTime));
				grid1[r, 2] = new SourceGrid.Cells.CheckBox(null, true);
                grid1[r, 2].View = viewCheckBox;
			}

            grid1.AutoSizeCells();
		}

        private class RTLColumnHeaderView : SourceGrid.Cells.Views.ColumnHeader
        {
            public RTLColumnHeaderView()
            {
                 this.Background = new DevAge.Drawing.VisualElements.ColumnHeader();
            }
        }

        private class RTLCheckBoxView : SourceGrid.Cells.Views.CheckBox
        {
            public RTLCheckBoxView()
            {
                this.ElementCheckBox = new DevAge.Drawing.VisualElements.CheckBox();
            }
        }
	}
}
