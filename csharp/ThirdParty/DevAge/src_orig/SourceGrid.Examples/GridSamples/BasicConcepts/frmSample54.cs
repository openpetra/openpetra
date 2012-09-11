using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using SourceGrid;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample14.
	/// </summary>
	[Sample("SourceGrid - Basic concepts", 54, "Programmatically edit cell")]
	public class frmSample54 : System.Windows.Forms.Form
	{
		private SourceGrid.Grid grid1;
		private SourceGrid.Cells.Editors.ComboBox cbEditor;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample54()
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
			this.buttonEditCell = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.buttonInsertRow = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.numericUpDownRowIndex = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRowIndex)).BeginInit();
			this.SuspendLayout();
			// 
			// grid1
			// 
			this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.grid1.EnableSort = true;
			this.grid1.Location = new System.Drawing.Point(8, 94);
			this.grid1.Name = "grid1";
			this.grid1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
			this.grid1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
			this.grid1.Size = new System.Drawing.Size(682, 404);
			this.grid1.TabIndex = 0;
			this.grid1.TabStop = true;
			this.grid1.ToolTipText = "";
			// 
			// buttonEditCell
			// 
			this.buttonEditCell.Location = new System.Drawing.Point(109, 19);
			this.buttonEditCell.Name = "buttonEditCell";
			this.buttonEditCell.Size = new System.Drawing.Size(97, 20);
			this.buttonEditCell.TabIndex = 1;
			this.buttonEditCell.Text = "Edit cell";
			this.buttonEditCell.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(71, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Click to edit";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.buttonEditCell);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(8, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(212, 76);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Edit cell";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.buttonInsertRow);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.numericUpDownRowIndex);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Location = new System.Drawing.Point(226, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(238, 76);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Insert row";
			// 
			// buttonInsertRow
			// 
			this.buttonInsertRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonInsertRow.Location = new System.Drawing.Point(135, 19);
			this.buttonInsertRow.Name = "buttonInsertRow";
			this.buttonInsertRow.Size = new System.Drawing.Size(97, 20);
			this.buttonInsertRow.TabIndex = 1;
			this.buttonInsertRow.Text = "Insert row";
			this.buttonInsertRow.UseVisualStyleBackColor = true;
			this.buttonInsertRow.Click += new System.EventHandler(this.ButtonInsertRowClick);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(10, 47);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Insert after row:";
			// 
			// numericUpDownRowIndex
			// 
			this.numericUpDownRowIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDownRowIndex.Location = new System.Drawing.Point(135, 45);
			this.numericUpDownRowIndex.Name = "numericUpDownRowIndex";
			this.numericUpDownRowIndex.Size = new System.Drawing.Size(97, 20);
			this.numericUpDownRowIndex.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(10, 25);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Click to insert";
			// 
			// frmSample54
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(698, 505);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.grid1);
			this.Name = "frmSample54";
			this.Text = "Programatically edit cell";
			this.Load += new System.EventHandler(this.frmSample54_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRowIndex)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.NumericUpDown numericUpDownRowIndex;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonInsertRow;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonEditCell;
		#endregion

		private void PopulateRow(int r)
		{
			grid1[r, 0] = new SourceGrid.Cells.Cell("Hello " + r.ToString(), typeof(string));
			grid1[r, 1] = new SourceGrid.Cells.Cell(DateTime.Today, typeof(DateTime));
			grid1[r, 2] = new SourceGrid.Cells.CheckBox(null, true);
			grid1[r, 3] = new SourceGrid.Cells.Cell("Value 1", cbEditor);
			grid1[r, 3].View = SourceGrid.Cells.Views.ComboBox.Default;
		}
		
		private void frmSample54_Load(object sender, System.EventArgs e)
		{
			grid1.BorderStyle = BorderStyle.FixedSingle;

			grid1.ColumnsCount = 4;
			grid1.FixedRows = 1;
			grid1.Rows.Insert(0);
			grid1.Selection.FocusStyle = FocusStyle.None;

			cbEditor = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
			cbEditor.StandardValues = new string[]{"Value 1", "Value 2", "Value 3"};
			cbEditor.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.SingleClick | SourceGrid.EditableMode.AnyKey;

			grid1[0, 0] = new SourceGrid.Cells.ColumnHeader("String");
			grid1[0, 1] = new SourceGrid.Cells.ColumnHeader("DateTime");
			grid1[0, 2] = new SourceGrid.Cells.ColumnHeader("CheckBox");
			grid1[0, 3] = new SourceGrid.Cells.ColumnHeader("ComboBox");
			for (int r = 1; r < 10; r++)
			{
				grid1.Rows.Insert(r);
				PopulateRow(r);
			}

			grid1.AutoSizeCells();
			
			numericUpDownRowIndex.Value = 0;
			numericUpDownRowIndex.Minimum = 1;
			numericUpDownRowIndex.Value = 3;
			buttonEditCell.Click += delegate { StartEdit(grid1.Selection.ActivePosition); };
		}
		
		public void StartEdit ( SourceGrid.Position Position )
		{
			SourceGrid.CellContext _Context = new SourceGrid.CellContext ( grid1, Position, grid1[ Position.Row, Position.Column ] );
			_Context.StartEdit ( );
		}
		
		void ButtonInsertRowClick(object sender, EventArgs e)
		{
			var index = (int)numericUpDownRowIndex.Value;
			this.grid1.Rows.Insert(index);
			PopulateRow(index);
		}
	}
}
