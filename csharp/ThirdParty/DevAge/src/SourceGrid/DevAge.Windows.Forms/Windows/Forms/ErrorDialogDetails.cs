using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
	/// <summary>
	/// Summary description for ErrorDialogDetails.
	/// </summary>
	public class ErrorDialogDetails : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtDetailsError;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ErrorDialogDetails()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ErrorDialogDetails));
			this.txtDetailsError = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtDetailsError
			// 
			this.txtDetailsError.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDetailsError.Location = new System.Drawing.Point(0, 0);
			this.txtDetailsError.MaxLength = 0;
			this.txtDetailsError.Multiline = true;
			this.txtDetailsError.Name = "txtDetailsError";
			this.txtDetailsError.ReadOnly = true;
			this.txtDetailsError.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtDetailsError.Size = new System.Drawing.Size(492, 371);
			this.txtDetailsError.TabIndex = 0;
			this.txtDetailsError.Text = "";
			this.txtDetailsError.WordWrap = false;
			// 
			// ErrorDialogDetails
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(492, 371);
			this.Controls.Add(this.txtDetailsError);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ErrorDialogDetails";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Error Details";
			this.Load += new System.EventHandler(this.ErrorDialogDetails_Load);
			this.ResumeLayout(false);

		}
		#endregion


		private Exception m_Exception;
		public Exception Exception
		{
			get{return m_Exception;}
			set{m_Exception = value;}
		}

		public ErrorDialogDetails(Exception p_Exception, string p_Caption):this()
		{
			Text = "Error Details - " + p_Caption;
			Exception = p_Exception;
		}

		public DialogResult ShowDialog(Exception p_Exception, string p_Caption)
		{
			Text = "Error Details - " + p_Caption;
			Exception = p_Exception;

			return base.ShowDialog();
		}

		public DialogResult ShowDialog(IWin32Window p_Owner, Exception p_Exception, string p_Caption)
		{
			Text = "Error Details - " + p_Caption;
			Exception = p_Exception;

			return base.ShowDialog(p_Owner);
		}

		public static void Show(Exception p_Exception, string p_Caption)
		{
			ErrorDialogDetails l_dg = new ErrorDialogDetails(p_Exception,p_Caption);
			l_dg.ShowDialog();
		}

		private void ErrorDialogDetails_Load(object sender, System.EventArgs e)
		{
			try
			{
				if (m_Exception != null)
					txtDetailsError.Text = m_Exception.ToString();
				else
					txtDetailsError.Text = "Exception is null";
			}
			catch(Exception err)
			{
				txtDetailsError.Text = "Error loading message:" + err.ToString();
			}
		}
	
		public static void Show(IWin32Window p_Owner, Exception p_Exception, string p_Caption)
		{
			ErrorDialogDetails l_dg = new ErrorDialogDetails(p_Exception,p_Caption);
			l_dg.ShowDialog(p_Owner);
		}
	}
}
