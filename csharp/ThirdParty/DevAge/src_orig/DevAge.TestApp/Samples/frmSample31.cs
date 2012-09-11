using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevAge.TestApp
{
	/// <summary>
	/// Summary description for frmDemo2.
	/// </summary>
	[Sample("Other controls", 31, "Application settings")]
	public class frmSample31 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.ComponentModel.IContainer components = null;

		public frmSample31()
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
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.LargeButtons = false;
			this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid1.Location = new System.Drawing.Point(8, 12);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(384, 200);
			this.propertyGrid1.TabIndex = 0;
			this.propertyGrid1.Text = "propertyGrid1";
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// frmSample31
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(400, 241);
			this.Controls.Add(this.propertyGrid1);
			this.Name = "frmSample31";
			this.Text = "Application settings";
			this.ResumeLayout(false);

		}
		#endregion

		SettingsDemo setting = new SettingsDemo();

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			setting.Load();

			propertyGrid1.SelectedObject = setting;
		}

		protected override void OnClosed(EventArgs e)
		{
			setting.Save();

			base.OnClosed (e);
		}


	}

	public class SettingsDemo : DevAge.Configuration.PersistableSettings
	{
		public SettingsDemo()
		{
			AddPersistableItem(new DevAge.Configuration.PersistableItem(typeof(string), "Key_String", "Value 1"));
			AddPersistableItem(new DevAge.Configuration.PersistableItem(typeof(double), "Key_Double", 0.59));
			AddPersistableItem(new DevAge.Configuration.PersistableItem(typeof(DateTime), "Key_DateTime", DateTime.Now));
		}

		public void Load()
		{
			base.ReadFromAppSettings("");
			base.AcceptChangesAsDefault();

			if (base.IsolatedStorageExists("SettingsDemo.xml"))
				base.ReadFromIsolatedStorage("SettingsDemo.xml");
		}

		public void Save()
		{
			if (base.HasChanges)
				base.WriteToIsolatedStorage("SettingsDemo.xml", DevAge.Configuration.PersistenceFlags.OnlyChanges);
			else if (base.IsolatedStorageExists("SettingsDemo.xml"))
				base.RemoveIsolatedStorage("SettingsDemo.xml");
		}

		public string Key_String
		{
			get{return (string)this["Key_String"];}
			set{this["Key_String"] = value;}
		}
		public double Key_Double
		{
			get{return (double)this["Key_Double"];}
			set{this["Key_Double"] = value;}
		}
		public DateTime Key_DateTime
		{
			get{return (DateTime)this["Key_DateTime"];}
			set{this["Key_DateTime"] = value;}
		}
	}
}
