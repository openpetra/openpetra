using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
	/// <summary>
	/// Summary description for FormPosition.
	/// </summary>
	public class FormBase : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormBase()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			string p = AppDomain.CurrentDomain.FriendlyName + "." + GetType().ToString() + ".frp";
            char[] invalidChars = System.IO.Path.GetInvalidPathChars();
            for (int i = 0; i < invalidChars.Length; i++)
                p = p.Replace(new string(invalidChars[i], 1), "");

			StorageFileName = p;
			RestoreFlags = RestoreFlags.None;
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
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "FormPosition";
		}
		#endregion

		private PositionRecorderIsolatedStorage m_Setting = new PositionRecorderIsolatedStorage();
		[Description("Isolated Storage FileName where the form save the position information")]
		public virtual string StorageFileName
		{
			get{return m_Setting.StorageFileName;}
			set{m_Setting.StorageFileName = value;}
		}
		[Description("Restore flags")]
		public virtual RestoreFlags RestoreFlags
		{
			get{return m_Setting.RestoreFlags;}
			set{m_Setting.RestoreFlags = value;}
		}
		[Description("Save flags")]
		public virtual SaveFlags SaveFlags
		{
			get{return m_Setting.SaveFlags;}
			set{m_Setting.SaveFlags = value;}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			try
			{
				if (DesignMode == false && 
					(Control.ModifierKeys & Keys.Control) != Keys.Control)
				{
					m_Setting.Load(this);
				}
			}
			catch(Exception err)
			{
				System.Diagnostics.Debug.Assert(false, err.Message);
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);

			try
			{
				if (DesignMode == false && 
					(Control.ModifierKeys & Keys.Control) != Keys.Control)
				{
					m_Setting.Save(this);
				}
			}
			catch(Exception err)
			{
				System.Diagnostics.Debug.Assert(false, err.Message);
			}
		}

	}
}
