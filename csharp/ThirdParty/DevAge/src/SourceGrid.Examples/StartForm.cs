using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Castle.Windsor;

namespace WindowsFormsSample
{

	
	/// <summary>
	/// Summary description for StartForm.
	/// </summary>
	public class StartForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnFile;
		private System.Windows.Forms.MenuItem mnExit;
		private System.Windows.Forms.MenuItem mnHelp;
		private System.Windows.Forms.MenuItem mnAbout;
		private SourceGrid.Grid grid1;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public StartForm()
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
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.mnFile = new System.Windows.Forms.MenuItem();
			this.mnExit = new System.Windows.Forms.MenuItem();
			this.mnHelp = new System.Windows.Forms.MenuItem();
			this.mnAbout = new System.Windows.Forms.MenuItem();
			this.grid1 = new SourceGrid.Grid();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnFile,
																					  this.mnHelp});
			// 
			// mnFile
			// 
			this.mnFile.Index = 0;
			this.mnFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.mnExit});
			this.mnFile.Text = "File";
			// 
			// mnExit
			// 
			this.mnExit.Index = 0;
			this.mnExit.Text = "Exit";
			this.mnExit.Click += new System.EventHandler(this.mnExit_Click);
			// 
			// mnHelp
			// 
			this.mnHelp.Index = 1;
			this.mnHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.mnAbout});
			this.mnHelp.Text = "Help";
			// 
			// mnAbout
			// 
			this.mnAbout.Index = 0;
			this.mnAbout.Text = "About";
			// 
			// grid1
			// 
			this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grid1.AutoStretchColumnsToFitWidth = false;
			this.grid1.AutoStretchRowsToFitHeight = false;
			this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.grid1.CustomSort = false;
			this.grid1.Location = new System.Drawing.Point(4, 4);
			this.grid1.Name = "grid1";
			this.grid1.OverrideCommonCmdKey = true;
			this.grid1.Size = new System.Drawing.Size(616, 296);
			this.grid1.SpecialKeys = SourceGrid.GridSpecialKeys.Default;
			this.grid1.TabIndex = 0;
			// 
			// StartForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(624, 305);
			this.Controls.Add(this.grid1);
			this.Menu = this.mainMenu1;
			this.Name = "StartForm";
			this.Text = "Examples Explorer";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();
			Application.DoEvents();

			//This is an optional line, used to enable Windows XP theme.
			//DevAge.Windows.Forms.ThemePainter.CurrentProvider = new DevAge.Windows.Forms.ThemeProviderXP();

			WindsorContainer container = new WindsorContainer();
			ServiceIntialization.Iinit(container);
			
			
			Application.Run(new StartForm());
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			grid1.ColumnsCount = 2;

			SourceGrid.Cells.Controllers.Button linkEvents = new SourceGrid.Cells.Controllers.Button();
			linkEvents.Executed += new EventHandler(linkEvents_Executed);

            //Category header
			SourceGrid.Cells.Views.Cell categoryView = new SourceGrid.Cells.Views.Cell();
            categoryView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
			categoryView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
			categoryView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            categoryView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            categoryView.Font = new Font(Font, FontStyle.Bold);

            //Title header
            SourceGrid.Cells.Views.ColumnHeader headerView = new SourceGrid.Cells.Views.ColumnHeader();


            //Load the forms
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			Type[] types = assembly.GetTypes();

            LoadCategorySample("SourceGrid - Basic concepts", linkEvents, types, categoryView, headerView);
            LoadCategorySample("SourceGrid - Standard features", linkEvents, types, categoryView, headerView);
            LoadCategorySample("SourceGrid - Advanced features", linkEvents, types, categoryView, headerView);
            LoadCategorySample("SourceGrid - Extensions", linkEvents, types, categoryView, headerView);
            LoadCategorySample("SourceGrid - Generic Samples", linkEvents, types, categoryView, headerView);
            LoadCategorySample("SourceGrid - Performance", linkEvents, types, categoryView, headerView);
            LoadCategorySample("SourceGrid - PingGrid", linkEvents, types, categoryView, headerView);

			//Stretch only the last column
			grid1.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
			grid1.Columns[1].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize|SourceGrid.AutoSizeMode.EnableStretch;
			grid1.AutoStretchColumnsToFitWidth = true;
			grid1.AutoSizeCells();
			grid1.Columns.StretchToFit();
		}

        private void LoadCategorySample(string category, SourceGrid.Cells.Controllers.Button linkEvents, Type[] assemblyTypes, SourceGrid.Cells.Views.IView categoryView, SourceGrid.Cells.Views.IView headerView)
		{
			int row;

			//Create Category Row
			row = grid1.RowsCount;
			grid1.Rows.Insert(row);
			grid1[row, 0] = new SourceGrid.Cells.Cell(category);
			grid1[row, 0].View = categoryView;
			grid1[row, 0].ColumnSpan = grid1.ColumnsCount;

			//Create Headers
			row = grid1.RowsCount;
			grid1.Rows.Insert(row);

			SourceGrid.Cells.ColumnHeader header1 = new SourceGrid.Cells.ColumnHeader("Sample N°");
            header1.View = headerView;
			header1.AutomaticSortEnabled = false;
			grid1[row, 0] = header1;

			SourceGrid.Cells.ColumnHeader header2 = new SourceGrid.Cells.ColumnHeader("Description");
            header2.View = headerView;
            header2.AutomaticSortEnabled = false;
			grid1[row, 1] = header2;

			//Create Data Cells
			for (int i = 0; i < assemblyTypes.Length; i++)
			{
				object[] attributes = assemblyTypes[i].GetCustomAttributes(typeof(SampleAttribute), true);
				if (attributes != null && attributes.Length > 0)
				{
					SampleAttribute sampleAttribute = (SampleAttribute)attributes[0];

					if (sampleAttribute.Category == category)
					{
						row = grid1.RowsCount;
						grid1.Rows.Insert(row);
						grid1[row, 0] = new SourceGrid.Cells.Cell( sampleAttribute.SampleNumber );
						grid1[row, 0].View = new SourceGrid.Cells.Views.Cell();
						grid1[row, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

						//Create a cell with a link
						grid1[row, 1] = new SourceGrid.Cells.Link( sampleAttribute.Description );
						grid1[row, 1].Tag = assemblyTypes[i];
						grid1[row, 1].Controller.AddController(linkEvents);
					}
				}
			}

			//Enable sorting of the category range
			SourceGrid.RangeLoader headerRange = new SourceGrid.RangeLoader(new SourceGrid.Range(header1.Range.Start.Row, 0, header1.Range.Start.Row, 1));
			SourceGrid.RangeLoader rangeToSort = new SourceGrid.RangeLoader(new SourceGrid.Range(header1.Range.Start.Row+1, 0, row, 1));
			SourceGrid.Cells.Controllers.SortableHeader sortableController = new SourceGrid.Cells.Controllers.SortableHeader(rangeToSort, headerRange);

			header1.AddController(sortableController);
			header1.Sort(true);
			header2.AddController(sortableController);
		}

		private void linkEvents_Executed(object sender, EventArgs e)
		{
			SourceGrid.CellContext cellContext = (SourceGrid.CellContext)sender;
			Type formType = (Type)((SourceGrid.Cells.Cell)cellContext.Cell).Tag;
			Form form = (Form) Activator.CreateInstance( formType );
			form.Owner = this;
			if (form.IsDisposed == true)
				return;
			form.Show();
		}

		private void mnExit_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
