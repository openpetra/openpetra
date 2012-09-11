using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSampleGrid1.
	/// </summary>
	[Sample("SourceGrid - Standard features", 26, "Use of Controllers: Cursor, Click event, ContextMenu and ToolTip")]
	public class frmSample26 : System.Windows.Forms.Form
	{
		private SourceGrid.Grid grid1;
        private Label label1;
        private Label lblKeyDown;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample26()
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
				if (components != null) 
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblKeyDown = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // grid1
            // 
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.Location = new System.Drawing.Point(12, 31);
            this.grid1.Name = "grid1";
            this.grid1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grid1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grid1.Size = new System.Drawing.Size(364, 210);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = true;
            this.grid1.ToolTipText = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "KeyDown:";
            // 
            // lblKeyDown
            // 
            this.lblKeyDown.AutoSize = true;
            this.lblKeyDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeyDown.Location = new System.Drawing.Point(71, 9);
            this.lblKeyDown.Name = "lblKeyDown";
            this.lblKeyDown.Size = new System.Drawing.Size(11, 13);
            this.lblKeyDown.TabIndex = 2;
            this.lblKeyDown.Text = ".";
            // 
            // frmSample26
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(388, 253);
            this.Controls.Add(this.lblKeyDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grid1);
            this.Name = "frmSample26";
            this.Text = "Controllers (ContextMenu, ToolTip, ...)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void Form1_Load(object sender, System.EventArgs e)
		{
			grid1.Redim(10, 5);

            //Setup the controllers
			CellClickEvent clickController = new CellClickEvent();
			PopupMenu menuController = new PopupMenu();
			CellCursor cursorController = new CellCursor();
            SourceGrid.Cells.Controllers.ToolTipText toolTipController = new SourceGrid.Cells.Controllers.ToolTipText();
            toolTipController.ToolTipTitle = "ToolTip example";
            toolTipController.ToolTipIcon = ToolTipIcon.Info;
            toolTipController.IsBalloon = true;
            ValueChangedEvent valueChangedController = new ValueChangedEvent();
            
			for (int r = 0; r < grid1.Rows.Count; r++)
			{
				if (r == 0)
				{
					grid1[r, 0] = new SourceGrid.Cells.ColumnHeader("Click event");
					grid1[r, 1] = new SourceGrid.Cells.ColumnHeader("Custom Cursor");
					grid1[r, 2] = new SourceGrid.Cells.ColumnHeader("ContextMenu");
                    grid1[r, 3] = new SourceGrid.Cells.ColumnHeader("ToolTip");
                    grid1[r, 4] = new SourceGrid.Cells.ColumnHeader("ValueChanged");
                }
				else
				{
					grid1[r, 0] = new SourceGrid.Cells.Cell("Value " + r.ToString(), typeof(string));
					grid1[r, 1] = new SourceGrid.Cells.Cell(DateTime.Now, typeof(DateTime));
					grid1[r, 2] = new SourceGrid.Cells.Cell("Right click");
                    grid1[r, 3] = new SourceGrid.Cells.Cell("Value " + r.ToString());
                    grid1[r, 3].ToolTipText = "Example of tooltip, bla bla bla ....\n Row: " + r.ToString();
                    grid1[r, 4] = new SourceGrid.Cells.Cell("Value " + r.ToString(), typeof(string));
                }

				grid1[r, 0].AddController(clickController);
				grid1[r, 1].AddController(cursorController);
				grid1[r, 2].AddController(menuController);
                grid1[r, 3].AddController(toolTipController);
                grid1[r, 4].AddController(valueChangedController);
			}

            grid1.AutoSizeCells();

            //Add the key controller to all the cells
            grid1.Controller.AddController(new KeyEvent(this));
		}

        public void SetKeyDownLabel(string val)
        {
            lblKeyDown.Text = val;
        }
	}

	public class CellCursor : SourceGrid.Cells.Controllers.MouseCursor
	{
		public CellCursor():base(Cursors.Cross, true)
		{
		}
	}

	public class PopupMenu : SourceGrid.Cells.Controllers.ControllerBase
	{
		ContextMenu menu = new ContextMenu();
		public PopupMenu()
		{
            menu.MenuItems.Add("Menu 1", new EventHandler(Menu1_Click));
			menu.MenuItems.Add("Menu 2", new EventHandler(Menu2_Click));
		}

		public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
		{
			base.OnMouseUp (sender, e);

			if (e.Button == MouseButtons.Right)
				menu.Show(sender.Grid, new Point(e.X, e.Y));
		}

        private void Menu1_Click(object sender, EventArgs e)
        {
            //TODO Your code here
        }
        private void Menu2_Click(object sender, EventArgs e)
        {
            //TODO Your code here
        }
	}

	public class CellClickEvent : SourceGrid.Cells.Controllers.ControllerBase
	{
		public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
		{
			base.OnClick (sender, e);

			MessageBox.Show(sender.Grid, sender.DisplayText);
		}
	}

    public class ValueChangedEvent : SourceGrid.Cells.Controllers.ControllerBase
    {
        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);

            string val = "Value of cell {0} is '{1}'";

            MessageBox.Show(sender.Grid, string.Format(val, sender.Position, sender.Value));
        }
    }

    public class KeyEvent : SourceGrid.Cells.Controllers.ControllerBase
    {
        private frmSample26 mFrm;
        public KeyEvent(frmSample26 frm)
        {
            mFrm = frm;
        }
        public override void OnKeyDown(SourceGrid.CellContext sender, KeyEventArgs e)
        {
            base.OnKeyDown(sender, e);

            mFrm.SetKeyDownLabel(e.KeyCode.ToString());
        }
    }
}
