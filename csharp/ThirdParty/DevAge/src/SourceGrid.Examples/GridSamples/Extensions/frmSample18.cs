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
	[Sample("SourceGrid - Extensions", 18, "Planning grid")]
	public class frmSample18 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private SourceGrid.ListEditor listEditor1;
		private System.Windows.Forms.Splitter splitter1;
		private DevAge.Windows.Forms.TextBoxUITypeEditor cbStart;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private DevAge.Windows.Forms.TextBoxUITypeEditor cbEnd;
		private SourceGrid.Planning.PlanningGrid planningGrid1;
		private System.Windows.Forms.Button btReload;
        private Button btExport;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample18()
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.planningGrid1 = new SourceGrid.Planning.PlanningGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.listEditor1 = new SourceGrid.ListEditor();
            this.cbStart = new DevAge.Windows.Forms.TextBoxUITypeEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbEnd = new DevAge.Windows.Forms.TextBoxUITypeEditor();
            this.btReload = new System.Windows.Forms.Button();
            this.btExport = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.planningGrid1);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.listEditor1);
            this.panel1.Location = new System.Drawing.Point(4, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(692, 388);
            this.panel1.TabIndex = 1;
            // 
            // planningGrid1
            // 
            this.planningGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.planningGrid1.Location = new System.Drawing.Point(191, 0);
            this.planningGrid1.Name = "planningGrid1";
            this.planningGrid1.Size = new System.Drawing.Size(501, 388);
            this.planningGrid1.TabIndex = 3;
            this.planningGrid1.AppointmentClick += new SourceGrid.Planning.PlanningGrid.AppointmentEventHandler(this.planningGrid1_AppointmentClick);
            this.planningGrid1.AppointmentDoubleClick += new SourceGrid.Planning.PlanningGrid.AppointmentEventHandler(this.planningGrid1_AppointmentDoubleClick);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(188, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 388);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // listEditor1
            // 
            this.listEditor1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listEditor1.Editors = null;
            this.listEditor1.EnableAdd = true;
            this.listEditor1.EnableMove = true;
            this.listEditor1.EnableRefresh = true;
            this.listEditor1.EnableRemove = true;
            this.listEditor1.ItemType = null;
            this.listEditor1.List = null;
            this.listEditor1.Location = new System.Drawing.Point(0, 0);
            this.listEditor1.Name = "listEditor1";
            this.listEditor1.Properties = null;
            this.listEditor1.Size = new System.Drawing.Size(188, 388);
            this.listEditor1.TabIndex = 1;
            this.listEditor1.ListChanged += new System.EventHandler(this.listEditor1_ListChanged);
            // 
            // cbStart
            // 
            this.cbStart.BackColor = System.Drawing.Color.Transparent;
            this.cbStart.Location = new System.Drawing.Point(52, 4);
            this.cbStart.Name = "cbStart";
            this.cbStart.Size = new System.Drawing.Size(160, 20);
            this.cbStart.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(220, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "End";
            // 
            // cbEnd
            // 
            this.cbEnd.BackColor = System.Drawing.Color.Transparent;
            this.cbEnd.Location = new System.Drawing.Point(264, 4);
            this.cbEnd.Name = "cbEnd";
            this.cbEnd.Size = new System.Drawing.Size(160, 20);
            this.cbEnd.TabIndex = 4;
            // 
            // btReload
            // 
            this.btReload.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btReload.Location = new System.Drawing.Point(432, 4);
            this.btReload.Name = "btReload";
            this.btReload.Size = new System.Drawing.Size(75, 23);
            this.btReload.TabIndex = 6;
            this.btReload.Text = "Reload";
            this.btReload.Click += new System.EventHandler(this.btReload_Click);
            // 
            // btExport
            // 
            this.btExport.Location = new System.Drawing.Point(530, 3);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(102, 23);
            this.btExport.TabIndex = 7;
            this.btExport.Text = "Export Image";
            this.btExport.UseVisualStyleBackColor = true;
            this.btExport.Click += new System.EventHandler(this.btExport_Click);
            // 
            // frmSample18
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(700, 423);
            this.Controls.Add(this.btExport);
            this.Controls.Add(this.btReload);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbEnd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbStart);
            this.Controls.Add(this.panel1);
            this.Name = "frmSample18";
            this.Text = "Planning Sample";
            this.Load += new System.EventHandler(this.frmSample18_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmSample18_Load(object sender, System.EventArgs e)
		{
			DateTime toDayPlus3 = DateTime.Today.AddDays(3);
			DateTime toDayMinus3 = DateTime.Today.AddDays(-3);
			DateTime now = DateTime.Now;


			cbStart.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(DateTime));
            cbStart.Value = new DateTime(toDayMinus3.Year, toDayMinus3.Month, toDayMinus3.Day, 8, 0, 0);
			cbEnd.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(DateTime));
            cbEnd.Value = new DateTime(toDayPlus3.Year, toDayPlus3.Month, toDayPlus3.Day, 18, 0, 0);

			listEditor1.ItemType = typeof(SourceGrid.Planning.AppointmentBase);

            SourceGrid.Planning.IAppointment appointment1 = new SourceGrid.Planning.AppointmentBase("Dentist",
                                    new DateTime(now.Year, now.Month, now.Day, 9, 0, 0),
                                    new DateTime(now.Year, now.Month, now.Day, 11, 30, 0));
            SourceGrid.Planning.IAppointment appointment2 = new SourceGrid.Planning.AppointmentBase("Appointment 1",
                                    new DateTime(now.Year, now.Month, now.Day, 16, 0, 0),
                                    new DateTime(now.Year, now.Month, now.Day, 16, 45, 0));
			planningGrid1.Appointments.Add( appointment1 );
			planningGrid1.Appointments.Add( appointment2 );

			listEditor1.List = new ArrayList(planningGrid1.Appointments);
			listEditor1.LoadList();

			LoadPlanning();
		}

		private void LoadPlanning()
		{
			try
			{
				planningGrid1.Appointments.Clear();
				foreach (SourceGrid.Planning.AppointmentBase a in listEditor1.List)
				{
					planningGrid1.Appointments.Add(a);
				}

				planningGrid1.LoadPlanning((DateTime)cbStart.Value,
					(DateTime)cbEnd.Value,
					15);
			}
			catch(Exception err)
			{
				DevAge.Windows.Forms.ErrorDialog.Show(this, err, "Error");
			}
		}

		private void listEditor1_ListChanged(object sender, System.EventArgs e)
		{
			LoadPlanning();
		}

		private void btReload_Click(object sender, System.EventArgs e)
		{
			LoadPlanning();
		}

        private void planningGrid1_AppointmentClick(object sender, SourceGrid.Planning.AppointmentEventArgs e)
        {
            //TODO Add your code here
        }

        private void planningGrid1_AppointmentDoubleClick(object sender, SourceGrid.Planning.AppointmentEventArgs e)
        {
            if (e.Appointment == null)
                MessageBox.Show(this, "Clicked at " + e.DateTimeStart.ToString());
            else
                MessageBox.Show(this, e.Appointment.Title);
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            try
            {
                string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ImageFile.bmp");

                SourceGrid.Exporter.Image image = new SourceGrid.Exporter.Image();

                using (System.Drawing.Bitmap bitmap = image.Export(planningGrid1.Grid, planningGrid1.Grid.CompleteRange))
                    bitmap.Save(path);

                DevAge.Shell.Utilities.OpenFile(path);
            }
            catch (Exception err)
            {
                DevAge.Windows.Forms.ErrorDialog.Show(this, err, "BITMAP Export Error");
            }
        }
	}
}
