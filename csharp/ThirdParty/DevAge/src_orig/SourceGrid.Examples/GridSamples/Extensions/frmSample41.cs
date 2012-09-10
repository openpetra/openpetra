using SourceGrid;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample9.
	/// </summary>
	[Sample("SourceGrid - Extensions", 41, "Advanced Data Binding - different Views, images and tooltip")]
	public class frmSample41 : System.Windows.Forms.Form
	{
		private SourceGrid.DataGrid dataGrid;
        private Label label1;
        private Label lblSelectedRow;
        private Button btFind;
        private TextBox txtFindName;
        private Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample41()
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblSelectedRow = new System.Windows.Forms.Label();
            this.btFind = new System.Windows.Forms.Button();
            this.txtFindName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
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
            this.dataGrid.Location = new System.Drawing.Point(4, 30);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dataGrid.Size = new System.Drawing.Size(492, 205);
            this.dataGrid.TabIndex = 15;
            this.dataGrid.TabStop = true;
            this.dataGrid.ToolTipText = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Selected Row:";
            // 
            // lblSelectedRow
            // 
            this.lblSelectedRow.AutoSize = true;
            this.lblSelectedRow.Location = new System.Drawing.Point(85, 8);
            this.lblSelectedRow.Name = "lblSelectedRow";
            this.lblSelectedRow.Size = new System.Drawing.Size(10, 13);
            this.lblSelectedRow.TabIndex = 17;
            this.lblSelectedRow.Text = "-";
            // 
            // btFind
            // 
            this.btFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btFind.Location = new System.Drawing.Point(421, 1);
            this.btFind.Name = "btFind";
            this.btFind.Size = new System.Drawing.Size(75, 23);
            this.btFind.TabIndex = 18;
            this.btFind.Text = "Find";
            this.btFind.UseVisualStyleBackColor = true;
            this.btFind.Click += new System.EventHandler(this.btFind_Click);
            // 
            // txtFindName
            // 
            this.txtFindName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFindName.Location = new System.Drawing.Point(315, 3);
            this.txtFindName.Name = "txtFindName";
            this.txtFindName.Size = new System.Drawing.Size(100, 20);
            this.txtFindName.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(238, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Find country:";
            // 
            // frmSample41
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(500, 242);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFindName);
            this.Controls.Add(this.btFind);
            this.Controls.Add(this.lblSelectedRow);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGrid);
            this.Name = "frmSample41";
            this.Text = "Advanced DataGrid binding (XML DataSet)";
            this.Load += new System.EventHandler(this.frmSample41_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private DataView mView;
		private void frmSample41_Load(object sender, System.EventArgs e)
		{
			//Read Data From xml
			DataSet ds = new DataSet();
			System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("WindowsFormsSample.GridSamples.SampleData2.xml");
			ds.ReadXml(stream);
			mView = ds.Tables[0].DefaultView;
			mView.AllowDelete = false;
			mView.AllowNew = false;

			dataGrid.FixedColumns = 1;
			dataGrid.Selection.EnableMultiSelection = true;

            DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);

			//Create default columns
            CreateColumns(bd);

			dataGrid.DataSource = bd;

            dataGrid.AutoSizeCells();

            dataGrid.Selection.SelectionChanged += new SourceGrid.RangeRegionChangedEventHandler(Selection_SelectionChanged);
		}

        void Selection_SelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
        {
            object[] rows = dataGrid.SelectedDataRows;
            if (rows != null && rows.Length > 0)
            {
                DataRowView row = (DataRowView)rows[0];

                lblSelectedRow.Text = row["Country"].ToString();
            }
        }

        private void CreateColumns(DevAge.ComponentModel.IBoundList boundList)
		{
            //Create the editors
			SourceGrid.Cells.Editors.TextBoxNumeric numericEditor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(decimal));
			numericEditor.TypeConverter = new DevAge.ComponentModel.Converter.NumberTypeConverter(typeof(decimal), "N");
			numericEditor.AllowNull = true;  //the database value can be null (System.DbNull)

			SourceGrid.Cells.Editors.ComboBox externalIdEditor = new SourceGrid.Cells.Editors.ComboBox(typeof(int));
			externalIdEditor.StandardValues = new int[]{1, 2, 3, 4};
			externalIdEditor.StandardValuesExclusive = true;
            externalIdEditor.Control.FormattingEnabled = true;

			DevAge.ComponentModel.Validator.ValueMapping mapping = new DevAge.ComponentModel.Validator.ValueMapping();
			mapping.ValueList = new int[]{1, 2, 3, 4};
            mapping.DisplayStringList = new string[] { "Reference 1", "Reference 2", "Reference 3", "Reference 4" };
			mapping.BindValidator(externalIdEditor);


            //Create the views
            SourceGrid.Cells.Views.Cell viewSelected = new SourceGrid.Cells.Views.Cell();
            viewSelected.Font = new Font(dataGrid.Font, FontStyle.Bold);
            viewSelected.ForeColor = Color.DarkGreen;

            //Create selected conditions
            SourceGrid.Conditions.ConditionView selectedConditionBold = new SourceGrid.Conditions.ConditionView(viewSelected);
            selectedConditionBold.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
                                                    {
                                                        DataRowView row = (DataRowView)itemRow;
                                                        return row["Selected"] is bool && (bool)row["Selected"] == true;
                                                    };
            SourceGrid.Conditions.ConditionCell selectedConditionStar = new SourceGrid.Conditions.ConditionCell(new SourceGrid.Cells.Virtual.CellVirtual());
            selectedConditionStar.Cell.Model.AddModel(new SourceGrid.Cells.Models.Image(Properties.Resources.Star.ToBitmap()));
            selectedConditionStar.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
                                                    {
                                                        DataRowView row = (DataRowView)itemRow;
                                                        return row["Selected"] is bool && (bool)row["Selected"] == true;
                                                    };

			//Create columns
            SourceGrid.DataGridColumn gridColumn;

            gridColumn = dataGrid.Columns.Add("Selected", "Selected", typeof(bool));

            gridColumn = dataGrid.Columns.Add("Country", "Country", typeof(string));
            gridColumn.DataCell.Model.AddModel(new BoundImage("Uniform"));
            gridColumn.Conditions.Add(selectedConditionBold);

            gridColumn = dataGrid.Columns.Add("Uniform", "Uniform", new SourceGrid.Cells.DataGrid.Image());
            gridColumn.Conditions.Add(selectedConditionBold);

            gridColumn = dataGrid.Columns.Add("Population", "Population", numericEditor);
            gridColumn.Conditions.Add(selectedConditionBold);

            gridColumn = dataGrid.Columns.Add("Surface", "Surface", numericEditor);
            gridColumn.Conditions.Add(selectedConditionBold);

            gridColumn = dataGrid.Columns.Add("ExternalID", "ExternalID", externalIdEditor);
            gridColumn.Conditions.Add(selectedConditionBold);

            gridColumn = dataGrid.Columns.Add("Star", "Star", new SourceGrid.Cells.Virtual.CellVirtual());
            gridColumn.DataCell.Model.AddModel(new SourceGrid.Cells.Models.Image(Properties.Resources.StarOff));
            gridColumn.Conditions.Add(selectedConditionStar);

            foreach (SourceGrid.DataGridColumn col in dataGrid.Columns)
            {
                col.DataCell.AddController(SourceGrid.Cells.Controllers.ToolTipText.Default);
                col.DataCell.Model.AddModel(MyToolTipModel.Default);
            }
		}

        public class BoundImage : SourceGrid.Cells.Models.IImage
        {
            private string imageProperty;
            public BoundImage(string imageProperty)
            {
                this.imageProperty = imageProperty;
            }

            public System.Drawing.Image GetImage(SourceGrid.CellContext cellContext)
            {
                SourceGrid.DataGrid dg = (SourceGrid.DataGrid)cellContext.Grid;
                int dsRow = dg.Rows.IndexToDataSourceIndex(cellContext.Position.Row);
                PropertyDescriptor property = dg.DataSource.GetItemProperty(imageProperty, StringComparison.InvariantCultureIgnoreCase);
                byte[] buffer = (byte[])dg.DataSource.GetItemValue(dsRow, property);

                using (System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer))
                {
                    return System.Drawing.Image.FromStream(stream);
                }
            }
        }

        private class MyToolTipModel : SourceGrid.Cells.Models.IToolTipText
        {
            public static readonly MyToolTipModel Default = new MyToolTipModel();

            public string GetToolTipText(SourceGrid.CellContext cellContext)
            {
                SourceGrid.DataGrid grid = (SourceGrid.DataGrid)cellContext.Grid;
                DataRowView row = (DataRowView)grid.Rows.IndexToDataSourceRow(cellContext.Position.Row);
                if (row != null)
                {
                    if (bool.Equals(row["Selected"], true))
                        return "Row " + cellContext.Position.Row.ToString() + " is selected";
                    else
                        return "Row " + cellContext.Position.Row.ToString() + " is NOT selected";
                }
                else
                    return string.Empty;
            }
        }

        private void btFind_Click(object sender, EventArgs e)
        {
            mView.Sort = "Country";
            int findRowIndex = mView.Find(txtFindName.Text);
            if (findRowIndex >= 0)
                dataGrid.SelectedDataRows = new object[] { mView[findRowIndex] };
            else
                MessageBox.Show(this, "Country not found");
        }
	}
}