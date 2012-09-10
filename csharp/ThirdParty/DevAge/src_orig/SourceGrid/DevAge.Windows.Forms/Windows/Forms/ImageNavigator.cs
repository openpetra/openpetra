using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
	/// <summary>
	/// Summary description for ImageNavigator.
	/// </summary>
	public class ImageNavigator : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Button btNext;
        private System.Windows.Forms.Button btPrevious;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ImageNavigator()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			SetStyle(ControlStyles.Selectable, true);
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ImageNavigator));
			this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btNext = new System.Windows.Forms.Button();
            this.btPrevious = new System.Windows.Forms.Button();
			this.lblStatus = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox.Location = new System.Drawing.Point(0, 0);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(344, 224);
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			// 
			// btNext
			// 
			this.btNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btNext.DialogResult = System.Windows.Forms.DialogResult.None;
			this.btNext.Image = ((System.Drawing.Image)(resources.GetObject("btNext.Image")));
			this.btNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btNext.Location = new System.Drawing.Point(176, 228);
			this.btNext.Name = "btNext";
			this.btNext.Size = new System.Drawing.Size(24, 24);
			this.btNext.TabIndex = 1;
			this.btNext.TabStop = false;
            this.btNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btNext.Click += new System.EventHandler(this.btNext_Click);
			// 
			// btPrevious
			// 
			this.btPrevious.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btPrevious.DialogResult = System.Windows.Forms.DialogResult.None;
			this.btPrevious.Image = ((System.Drawing.Image)(resources.GetObject("btPrevious.Image")));
            this.btPrevious.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btPrevious.Location = new System.Drawing.Point(144, 228);
			this.btPrevious.Name = "btPrevious";
			this.btPrevious.Size = new System.Drawing.Size(24, 24);
			this.btPrevious.TabIndex = 2;
			this.btPrevious.TabStop = false;
            this.btPrevious.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btPrevious.Click += new System.EventHandler(this.btPrevious_Click);
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblStatus.Location = new System.Drawing.Point(4, 228);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(132, 23);
			this.lblStatus.TabIndex = 3;
			this.lblStatus.Text = "status";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ImageNavigator
			// 
			this.Controls.Add(this.btPrevious);
			this.Controls.Add(this.btNext);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.pictureBox);
			this.Name = "ImageNavigator";
			this.Size = new System.Drawing.Size(344, 256);
			this.ResumeLayout(false);

		}
		#endregion

		private void RefreshImageList()
		{
			if (Images != null)
			{
				if (Images.Length > 0)
					CurrentImageIndex = 0;
				else
					CurrentImageIndex = -1;
			}
			else
				CurrentImageIndex = -1;

			RefreshImage();
		}

		private string m_StatusFormat = "{0} of {1}";
		public string StatusFormat
		{
			get{return m_StatusFormat;}
			set{m_StatusFormat = value;}
		}

		private void RefreshImage()
		{
			if (m_CurrentImageIndex < 0 || Images == null || m_CurrentImageIndex >= Images.Length)
			{
				pictureBox.Image = null;
				lblStatus.Text = "";
				btNext.Enabled = false;
				btPrevious.Enabled = false;
			}
			else
			{
				pictureBox.Image = Images[m_CurrentImageIndex];
				lblStatus.Text = string.Format(StatusFormat, m_CurrentImageIndex+1, Images.Length);

				btNext.Enabled = true;
				btPrevious.Enabled = true;
				if (m_CurrentImageIndex == 0)
					btPrevious.Enabled = false;
				if (m_CurrentImageIndex >= Images.Length-1)
					btNext.Enabled = false;
			}
		}

		private Image[] m_Images;

		public void NextImage()
		{
			if (Images != null && m_CurrentImageIndex < (Images.Length-1))
				CurrentImageIndex++;
		}

		public void PreviousImage()
		{
			if (Images != null && m_CurrentImageIndex > 0)
				CurrentImageIndex--;
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Image[] Images
		{
			get{return m_Images;}
			set{m_Images = value;RefreshImageList();}
		}

		private int m_CurrentImageIndex = -1;
	
		public int CurrentImageIndex
		{
			get{return m_CurrentImageIndex;}
			set
			{
				m_CurrentImageIndex = value;
				RefreshImage();
			}
		}

		public Size ImageAreaSize
		{
			get{return pictureBox.Size;}
			set
			{
				Width += value.Width-pictureBox.Width;
				Height += value.Height-pictureBox.Height;
			}
		}

		public System.Windows.Forms.BorderStyle ImageAreaBorderStyle
		{
			get{return pictureBox.BorderStyle;}
			set{pictureBox.BorderStyle = value;}
		}

		public PictureBoxSizeMode ImageAreaSizeMode
		{
			get{return pictureBox.SizeMode;}
			set{pictureBox.SizeMode = value;}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			RefreshImageList();
		}

//		protected override void OnKeyDown(KeyEventArgs e)
//		{
//			if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.Space)
//			{
//				NextImage();
//				e.Handled = true;
//			}
//			else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.Back)
//			{
//				PreviousImage();
//				e.Handled = true;
//			}
//
//			base.OnKeyDown (e);
//		}

		private void btPrevious_Click(object sender, System.EventArgs e)
		{
			PreviousImage();
		}

		private void btNext_Click(object sender, System.EventArgs e)
		{
			NextImage();
		}
	}
}