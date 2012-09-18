using System;
using System.Drawing;
using System.Windows.Forms;

using SourceGrid;
using SourceGrid.Cells.Controllers;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample14.
	/// </summary>
	[Sample("SourceGrid - Standard features", 56, "ColumnSpan and RowSpan with Sorting")]
	public class frmSample56 : System.Windows.Forms.Form
	{
		private SourceGrid.Grid grid1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
	
		public frmSample56()
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
			this.grid1.Location = new System.Drawing.Point(8, 8);
			this.grid1.Name = "grid1";
			this.grid1.Size = new System.Drawing.Size(612, 423);
			this.grid1.SpecialKeys = SourceGrid.GridSpecialKeys.Default;
			this.grid1.TabIndex = 0;
			// 
			// frmSample21
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(628, 438);
			this.Controls.Add(this.grid1);
			this.Name = "frmSample21";
			this.Text = "ColumnSpan and RowSpan";
			this.Load += new System.EventHandler(this.frmSample14_Load);
			this.ResumeLayout(false);
	
		}
		#endregion
	
		private void DoFull()
		{
			grid1.Redim(20, 4);
			grid1.FixedRows = 1;
	
			grid1[0, 0] = new MyHeader("1");
			grid1[0, 0].ColumnSpan = 3;
			grid1[0, 0].AddController(new SourceGrid.Cells.Controllers.SortableHeader());
			grid1[0, 3] = new MyHeader("2");
			grid1[0, 3].AddController(new SourceGrid.Cells.Controllers.SortableHeader());
	
			for (int r = 1; r < grid1.RowsCount; r++)
			{
				if (r % 2 == 0)
				{
					grid1[r, 0] = new SourceGrid.Cells.Cell("nospan" + r.ToString(), typeof(string));
					grid1[r, 1] = new SourceGrid.Cells.Cell("span" + r.ToString(), typeof(string));
					grid1[r, 1].ColumnSpan = 2;
				}
				else
				{
					grid1[r, 0] = new SourceGrid.Cells.Cell("span" + r.ToString(), typeof(string));
					grid1[r, 0].ColumnSpan = 2;
					grid1[r, 2] = new SourceGrid.Cells.Cell("nospan" + r.ToString(), typeof(string));
				}
	
				grid1[r, 3] = new SourceGrid.Cells.CheckBox("CheckBox Column/Row Span" + r.ToString(), false);
			}
	
			grid1.AutoSizeCells();
		}
		
		
		private void AddContextMenu()
		{
			var eventsController = new CustomEvents();
			var contextMenu = new ContextMenuStrip();
			var customEvents = new FrmSample21Events(this.grid1);
			contextMenu.Items.Add(customEvents.GetInsertRowItem());
			contextMenu.Items.Add(customEvents.GetRemoveRowItem());
			
			contextMenu.Items.Add(customEvents.GetInsertColItem());
			contextMenu.Items.Add(customEvents.GetRemoveColItem());
			
			eventsController.MouseDown += delegate(object sender, MouseEventArgs e)
			{
				if (e.Button != MouseButtons.Right)
					return;
				var context = (CellContext)sender;
				grid1.Selection.Focus(context.Position, true);
				grid1.Selection.SelectCell(context.Position, true);
				var rect = grid1.RangeToRectangle(new Range(context.Position, context.Position));
				customEvents.LastPosition = context.Position;
				contextMenu.Show(grid1, rect.Location);
			};
			grid1.Controller.AddController(eventsController);
		}
		
		private void frmSample14_Load(object sender, System.EventArgs e)
		{
			DoFull();
			AddContextMenu();
		}
	
		private class MyHeader : SourceGrid.Cells.ColumnHeader
		{
			public MyHeader(object value):base(value)
			{
				//1 Header Row
				SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
				view.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
				view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
				View = view;
	
				AutomaticSortEnabled = false;
			}
		}
	
		private void clickEvent_Click(object sender, EventArgs e)
		{
			SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
			MessageBox.Show(this, context.Position.ToString());
		}
	}
}
