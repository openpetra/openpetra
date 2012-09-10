using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevAge.TestApp
{
	/// <summary>
	/// Summary description for frmSample32.
	/// </summary>
	[Sample("Other controls", 32, "Date Differences")]
	public class frmSample32 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblMonthsDifference;
		private System.Windows.Forms.Label lblYearsDifference;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.DateTimePicker dateB;
		private System.Windows.Forms.DateTimePicker dateA;
		private System.Windows.Forms.Label lblDaysDifference;
		private System.Windows.Forms.Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample32()
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
			this.lblMonthsDifference = new System.Windows.Forms.Label();
			this.lblYearsDifference = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.dateB = new System.Windows.Forms.DateTimePicker();
			this.dateA = new System.Windows.Forms.DateTimePicker();
			this.lblDaysDifference = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblMonthsDifference
			// 
			this.lblMonthsDifference.Location = new System.Drawing.Point(212, 96);
			this.lblMonthsDifference.Name = "lblMonthsDifference";
			this.lblMonthsDifference.Size = new System.Drawing.Size(100, 20);
			this.lblMonthsDifference.TabIndex = 15;
			// 
			// lblYearsDifference
			// 
			this.lblYearsDifference.Location = new System.Drawing.Point(212, 72);
			this.lblYearsDifference.Name = "lblYearsDifference";
			this.lblYearsDifference.Size = new System.Drawing.Size(100, 20);
			this.lblYearsDifference.TabIndex = 14;
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(100, 96);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(112, 20);
			this.label28.TabIndex = 13;
			this.label28.Text = "Months Difference";
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(100, 72);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(112, 20);
			this.label27.TabIndex = 12;
			this.label27.Text = "Years Difference";
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(16, 44);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(64, 23);
			this.label26.TabIndex = 11;
			this.label26.Text = "Date B";
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(16, 16);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(64, 23);
			this.label25.TabIndex = 10;
			this.label25.Text = "Date A";
			// 
			// dateB
			// 
			this.dateB.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateB.Location = new System.Drawing.Point(84, 44);
			this.dateB.Name = "dateB";
			this.dateB.Size = new System.Drawing.Size(104, 20);
			this.dateB.TabIndex = 9;
			this.dateB.Value = new System.DateTime(1981, 10, 6, 0, 0, 0, 0);
			this.dateB.ValueChanged += new System.EventHandler(this.dateDifference_ValueChanged);
			// 
			// dateA
			// 
			this.dateA.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateA.Location = new System.Drawing.Point(84, 16);
			this.dateA.Name = "dateA";
			this.dateA.Size = new System.Drawing.Size(104, 20);
			this.dateA.TabIndex = 8;
			this.dateA.Value = new System.DateTime(1999, 10, 6, 0, 0, 0, 0);
			this.dateA.ValueChanged += new System.EventHandler(this.dateDifference_ValueChanged);
			// 
			// lblDaysDifference
			// 
			this.lblDaysDifference.Location = new System.Drawing.Point(212, 120);
			this.lblDaysDifference.Name = "lblDaysDifference";
			this.lblDaysDifference.Size = new System.Drawing.Size(100, 20);
			this.lblDaysDifference.TabIndex = 17;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(100, 120);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 20);
			this.label2.TabIndex = 16;
			this.label2.Text = "Days Difference";
			// 
			// frmSample32
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(336, 146);
			this.Controls.Add(this.lblDaysDifference);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblMonthsDifference);
			this.Controls.Add(this.lblYearsDifference);
			this.Controls.Add(this.label28);
			this.Controls.Add(this.label27);
			this.Controls.Add(this.label26);
			this.Controls.Add(this.label25);
			this.Controls.Add(this.dateB);
			this.Controls.Add(this.dateA);
			this.Name = "frmSample32";
			this.Text = "Date Differences";
			this.Load += new System.EventHandler(this.frmSample32_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmSample32_Load(object sender, System.EventArgs e)
		{
			dateDifference_ValueChanged(this, EventArgs.Empty);
		}
		private void dateDifference_ValueChanged(object sender, System.EventArgs e)
		{
			lblMonthsDifference.Text = DevAge.DateTimeHelper.MonthsDifference(dateA.Value, dateB.Value).ToString();
			lblYearsDifference.Text = DevAge.DateTimeHelper.YearsDifference(dateA.Value, dateB.Value).ToString();
			lblDaysDifference.Text = (dateA.Value-dateB.Value).TotalDays.ToString();
		}
	}
}
