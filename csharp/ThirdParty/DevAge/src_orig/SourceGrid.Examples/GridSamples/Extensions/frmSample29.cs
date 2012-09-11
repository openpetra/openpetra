using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample9.
	/// </summary>
	[Sample("SourceGrid - Extensions", 29, "Advanced Data Binding - DataGrid, alternate backcolor, ..")]
	public class frmSample29 : System.Windows.Forms.Form
	{
		private SourceGrid.DataGrid dataGrid;
		private System.Windows.Forms.Button btLoadXml;
		private System.Windows.Forms.Button btSaveXml;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample29()
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
			this.dataGrid = new SourceGrid.DataGrid();
			this.btLoadXml = new System.Windows.Forms.Button();
			this.btSaveXml = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// dataGrid
			// 
			this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.dataGrid.DefaultWidth = 20;
			this.dataGrid.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
			this.dataGrid.FixedRows = 1;
			this.dataGrid.Location = new System.Drawing.Point(4, 32);
			this.dataGrid.Name = "dataGrid";
			this.dataGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			this.dataGrid.Size = new System.Drawing.Size(572, 424);
			this.dataGrid.TabIndex = 15;
			this.dataGrid.TabStop = true;
			this.dataGrid.ToolTipText = "";
			// 
			// btLoadXml
			// 
			this.btLoadXml.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btLoadXml.Location = new System.Drawing.Point(4, 4);
			this.btLoadXml.Name = "btLoadXml";
			this.btLoadXml.Size = new System.Drawing.Size(75, 23);
			this.btLoadXml.TabIndex = 16;
			this.btLoadXml.Text = "Load XML";
			this.btLoadXml.Click += new System.EventHandler(this.btLoadXml_Click);
			// 
			// btSaveXml
			// 
			this.btSaveXml.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btSaveXml.Location = new System.Drawing.Point(92, 4);
			this.btSaveXml.Name = "btSaveXml";
			this.btSaveXml.Size = new System.Drawing.Size(75, 23);
			this.btSaveXml.TabIndex = 17;
			this.btSaveXml.Text = "Save XML";
			this.btSaveXml.Click += new System.EventHandler(this.btSaveXml_Click);
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(182, 4);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 17;
			this.button1.Text = "Print";
			// 
			// frmSample29
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(580, 463);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btSaveXml);
			this.Controls.Add(this.btLoadXml);
			this.Controls.Add(this.dataGrid);
			this.Name = "frmSample29";
			this.Text = "Advanced DataGrid binding (XML DataSet)";
			this.Load += new System.EventHandler(this.frmSample29_Load);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button button1;
		#endregion

		private DataView mView;
		private void frmSample29_Load(object sender, System.EventArgs e)
		{
			//Read Data From xml
			DataSet ds = new DataSet();
			ds.ReadXml(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("WindowsFormsSample.GridSamples.SampleData2.xml"));
			mView = ds.Tables[0].DefaultView;


			dataGrid.FixedRows = 1;
			dataGrid.FixedColumns = 1;

			//Header row
			dataGrid.Columns.Insert(0, SourceGrid.DataGridColumn.CreateRowHeader(dataGrid));

			DevAge.ComponentModel.IBoundList bindList = new DevAge.ComponentModel.BoundDataView(mView);

			//Create default columns
			CreateColumns(dataGrid.Columns, bindList);

			dataGrid.DataSource = bindList;

			dataGrid.AutoSizeCells();
		}

		private void CreateColumns(SourceGrid.DataGridColumns columns,
		                           DevAge.ComponentModel.IBoundList bindList)
		{
			SourceGrid.Cells.Editors.TextBoxNumeric numericEditor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(decimal));
			numericEditor.TypeConverter = new DevAge.ComponentModel.Converter.NumberTypeConverter(typeof(decimal), "N");
			numericEditor.AllowNull = true;

			//Borders
			DevAge.Drawing.RectangleBorder border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.ForestGreen), new DevAge.Drawing.BorderLine(Color.ForestGreen));

			//Standard Views
			SourceGrid.Cells.Views.Link viewLink = new SourceGrid.Cells.Views.Link();
			viewLink.BackColor = Color.DarkSeaGreen;
			viewLink.Border = border;
			viewLink.ImageAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
			viewLink.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
			SourceGrid.Cells.Views.Cell viewString = new SourceGrid.Cells.Views.Cell();
			viewString.BackColor = Color.DarkSeaGreen;
			viewString.Border = border;
			viewString.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
			SourceGrid.Cells.Views.Cell viewNumeric = new SourceGrid.Cells.Views.Cell();
			viewNumeric.BackColor = Color.DarkSeaGreen;
			viewNumeric.Border = border;
			viewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
			SourceGrid.Cells.Views.Cell viewImage = new SourceGrid.Cells.Views.Cell();
			viewImage.BackColor = Color.DarkSeaGreen;
			viewImage.Border = border;
			viewImage.ImageStretch = false;
			viewImage.ImageAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

			//Create columns
			SourceGrid.DataGridColumn gridColumn;

			gridColumn = dataGrid.Columns.Add(null, "", new SourceGrid.Cells.Link());
			gridColumn.DataCell.AddController(new LinkClickDelete());
			gridColumn.DataCell.View = viewLink;
			((SourceGrid.Cells.Link)gridColumn.DataCell).Image = Properties.Resources.trash.ToBitmap();

			gridColumn = dataGrid.Columns.Add("Flag", "Flag", new SourceGrid.Cells.DataGrid.Image());
			gridColumn.DataCell.View = viewImage;

			gridColumn = dataGrid.Columns.Add("Country", "Country", typeof(string));
			gridColumn.DataCell.View = viewString;

			gridColumn = dataGrid.Columns.Add("Uniform", "Uniform", new SourceGrid.Cells.DataGrid.Image());
			gridColumn.DataCell.View = viewImage;

			gridColumn = dataGrid.Columns.Add("Capital", "Capital", typeof(string));
			gridColumn.DataCell.View = viewString;

			gridColumn = dataGrid.Columns.Add("Population", "Population", typeof(decimal));
			gridColumn.DataCell.Editor = numericEditor;
			gridColumn.DataCell.View = viewNumeric;

			gridColumn = dataGrid.Columns.Add("Surface", "Surface", typeof(decimal));
			gridColumn.DataCell.Editor = numericEditor;
			gridColumn.DataCell.View = viewNumeric;

			gridColumn = dataGrid.Columns.Add("Languages", "Languages", typeof(string));
			gridColumn.DataCell.View = viewString;

			gridColumn = dataGrid.Columns.Add("Currency", "Currency", typeof(string));
			gridColumn.DataCell.View = viewString;

			gridColumn = dataGrid.Columns.Add("Major Cities", "Major Cities", typeof(string));
			gridColumn.DataCell.View = viewString;

			gridColumn = dataGrid.Columns.Add("National Holiday", "National Holiday", typeof(string));
			gridColumn.DataCell.View = viewString;

			gridColumn = dataGrid.Columns.Add("Lowest point", "Lowest point", typeof(string));
			gridColumn.DataCell.View = viewString;

			gridColumn = dataGrid.Columns.Add("Highest point", "Highest point", typeof(string));
			gridColumn.DataCell.View = viewString;

			//Create a conditional view
			foreach (SourceGrid.DataGridColumn col in columns)
			{
				SourceGrid.Conditions.ICondition condition =
					SourceGrid.Conditions.ConditionBuilder.AlternateView(col.DataCell.View,
					                                                     Color.LightGray, Color.Black);
				col.Conditions.Add(condition);
			}
		}

		private void btLoadXml_Click(object sender, System.EventArgs e)
		{
			using (OpenFileDialog dg = new OpenFileDialog())
			{
				if (dg.ShowDialog(this) == DialogResult.OK)
				{
					mView.Table.DataSet.Clear();
					mView.Table.DataSet.ReadXml(dg.FileName, XmlReadMode.IgnoreSchema);
				}
			}
		}
		
		private void btSaveXml_Click(object sender, System.EventArgs e)
		{
			using (SaveFileDialog dg = new SaveFileDialog())
			{
				if (dg.ShowDialog(this) == DialogResult.OK)
					mView.Table.DataSet.WriteXml(dg.FileName, XmlWriteMode.WriteSchema);
			}
		}

		private class LinkClickDelete : SourceGrid.Cells.Controllers.ControllerBase
		{
			public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
			{
				base.OnClick(sender, e);

				SourceGrid.DataGrid grid = (SourceGrid.DataGrid)sender.Grid;
				grid.DeleteSelectedRows();
			}
		}
	}
}