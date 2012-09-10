using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
	/// <summary>
	/// A Windows Forms Form that can be used to display a dialog similar to a message box to show an exception.
    /// Can be used to show the Exception.Message, call stack and inner exception by clicking on the Details link.
	/// </summary>
	public class ErrorDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btOk;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblErrorMessage;
		private System.Windows.Forms.LinkLabel linkDetails;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ErrorDialog()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorDialog));
            this.btOk = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.linkDetails = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btOk
            // 
            this.btOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOk.Location = new System.Drawing.Point(149, 59);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 0;
            this.btOk.Text = "OK";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(7, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblErrorMessage.Location = new System.Drawing.Point(45, 8);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(319, 48);
            this.lblErrorMessage.TabIndex = 2;
            this.lblErrorMessage.Text = "Message ....";
            this.lblErrorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkDetails
            // 
            this.linkDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkDetails.AutoSize = true;
            this.linkDetails.Location = new System.Drawing.Point(4, 64);
            this.linkDetails.Name = "linkDetails";
            this.linkDetails.Size = new System.Drawing.Size(48, 13);
            this.linkDetails.TabIndex = 3;
            this.linkDetails.TabStop = true;
            this.linkDetails.Text = "Details...";
            this.linkDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDetails_LinkClicked);
            // 
            // ErrorDialog
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btOk;
            this.ClientSize = new System.Drawing.Size(372, 88);
            this.Controls.Add(this.linkDetails);
            this.Controls.Add(this.lblErrorMessage);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(378, 120);
            this.Name = "ErrorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ErrorDialog";
            this.Load += new System.EventHandler(this.ErrorDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		private Exception m_Exception;
		public Exception Exception
		{
			get{return m_Exception;}
			set{m_Exception = value;}
		}

		public ErrorDialog(Exception p_Exception, string p_Caption):this()
		{
			Text = p_Caption;
			Exception = p_Exception;
		}

		public DialogResult ShowDialog(Exception p_Exception, string p_Caption)
		{
			Text = p_Caption;
			Exception = p_Exception;

			return base.ShowDialog();
		}

		public DialogResult ShowDialog(IWin32Window p_Owner, Exception p_Exception, string p_Caption)
		{
			Text = p_Caption;
			Exception = p_Exception;

			return base.ShowDialog(p_Owner);
		}

		public static void Show(Exception p_Exception, string p_Caption)
		{
			ErrorDialog l_dg = new ErrorDialog(p_Exception,p_Caption);
			l_dg.ShowDialog();
		}

		private void linkDetails_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				ErrorDialogDetails l_dg = new ErrorDialogDetails(Exception,Text);
				l_dg.ShowDialog(this);
			}
			catch(Exception )
			{
			}
		}

		private void ErrorDialog_Load(object sender, System.EventArgs e)
		{
			try
			{
				if (m_Exception != null)
					lblErrorMessage.Text = m_Exception.Message;
				else
					lblErrorMessage.Text = "Exception is null";
			}
			catch(Exception err)
			{
				lblErrorMessage.Text = "Error loading message:" + err.Message;
			}		
		}
	
		public static void Show(IWin32Window p_Owner, Exception p_Exception, string p_Caption)
		{
			ErrorDialog l_dg = new ErrorDialog(p_Exception,p_Caption);
			l_dg.ShowDialog(p_Owner);
		}
	}

}
