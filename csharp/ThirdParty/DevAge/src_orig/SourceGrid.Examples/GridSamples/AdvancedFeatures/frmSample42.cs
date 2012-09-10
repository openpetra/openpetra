using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample14.
	/// </summary>
	[Sample("SourceGrid - Advanced features", 42, "Editable headers")]
	public class frmSample42 : System.Windows.Forms.Form
	{
		private SourceGrid.Grid grid1;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample42()
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
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// grid1
			// 
			this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grid1.Location = new System.Drawing.Point(8, 40);
			this.grid1.Name = "grid1";
			this.grid1.Size = new System.Drawing.Size(276, 224);
			this.grid1.SpecialKeys = SourceGrid.GridSpecialKeys.Default;
			this.grid1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(8, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(276, 32);
			this.label1.TabIndex = 1;
			this.label1.Text = "Left click on the headers to sort, right click to edit.";
			// 
			// frmSample42
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 271);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.grid1);
			this.Name = "frmSample42";
			this.Text = "Editable headers";
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			grid1.BorderStyle = BorderStyle.FixedSingle;

			grid1.ColumnsCount = 3;
			//grid1.FixedRows = 1;
			grid1.Rows.Insert(0);
			grid1[0,0] = new EditableColumnHeader("String");
			grid1[0,1] = new EditableColumnHeader("DateTime");
			grid1[0,2] = new EditableColumnHeader("CheckBox");

			for (int r = 1; r < 10; r++)
			{
				grid1.Rows.Insert(r);
				grid1[r,0] = new SourceGrid.Cells.Cell("Hello " + r.ToString(), typeof(string));
				grid1[r,1] = new SourceGrid.Cells.Cell(DateTime.Today, typeof(DateTime));
				grid1[r,2] = new SourceGrid.Cells.CheckBox(null, true);
			}

            grid1.AutoSizeCells();
		}

		private class EditableColumnHeader : SourceGrid.Cells.ColumnHeader
		{
			private static RightClickEditing rightClickEditing = new RightClickEditing();
			static EditableColumnHeader()
			{
			}

			public EditableColumnHeader(string caption):base(caption)
			{
				//Set the editor
				SourceGrid.Cells.Editors.TextBox headerEditor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
				headerEditor.EditableMode = SourceGrid.EditableMode.None;
				Editor = headerEditor;

				//Add the right click editing support
				AddController(rightClickEditing);

				//Remove the unselectable Controllers
				RemoveController(SourceGrid.Cells.Controllers.Unselectable.Default);
			}

			private class RightClickEditing : SourceGrid.Cells.Controllers.ControllerBase
			{
				public override void OnMouseDown(SourceGrid.CellContext sender, MouseEventArgs e)
				{
					base.OnMouseDown (sender, e);

					if (e.Button == MouseButtons.Right)
					{
						sender.StartEdit();
					}
				}
			}
		}
	}
}
