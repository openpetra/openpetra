using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
	/// <summary>
	/// Summary description for ColorPicker.
	/// </summary>
	public class ColorPicker : EditableControlBase
	{
		private System.Windows.Forms.Button btBrowse;
		private System.Windows.Forms.Panel panelColor;
		private System.Windows.Forms.Label labelColor;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ColorPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			SelectedColor = Color.Black;
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

		public virtual Color SelectedColor
		{
			get{return panelColor.BackColor;}
			set{panelColor.BackColor = value;}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.labelColor = new System.Windows.Forms.Label();
			this.panelColor = new System.Windows.Forms.Panel();
			this.btBrowse = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// labelColor
			// 
			this.labelColor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelColor.BackColor = System.Drawing.SystemColors.Window;
			this.labelColor.Location = new System.Drawing.Point(28, 2);
			this.labelColor.Name = "labelColor";
			this.labelColor.Size = new System.Drawing.Size(80, 16);
			this.labelColor.TabIndex = 2;
			this.labelColor.Text = "Black";
			this.labelColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panelColor
			// 
			this.panelColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.panelColor.BackColor = System.Drawing.Color.Black;
			this.panelColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelColor.Location = new System.Drawing.Point(2, 2);
			this.panelColor.Name = "panelColor";
			this.panelColor.Size = new System.Drawing.Size(20, 16);
			this.panelColor.TabIndex = 1;
			this.panelColor.BackColorChanged += new System.EventHandler(this.panelColor_BackColorChanged);
			// 
			// btBrowse
			// 
			this.btBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btBrowse.BackColor = System.Drawing.SystemColors.Control;
			this.btBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btBrowse.Location = new System.Drawing.Point(110, 2);
			this.btBrowse.Name = "btBrowse";
			this.btBrowse.Size = new System.Drawing.Size(24, 16);
			this.btBrowse.TabIndex = 0;
			this.btBrowse.Text = "...";
			this.btBrowse.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
			// 
			// ColorPicker
			// 
			this.Controls.Add(this.panelColor);
			this.Controls.Add(this.labelColor);
			this.Controls.Add(this.btBrowse);
			this.Name = "ColorPicker";
			this.Size = new System.Drawing.Size(136, 20);
			this.ResumeLayout(false);

		}
		#endregion

		private void panelColor_BackColorChanged(object sender, System.EventArgs e)
		{
			labelColor.Text = panelColor.BackColor.Name;

			if (SelectedColorChanged!=null)
				SelectedColorChanged(this, e);
		}

		private void btBrowse_Click(object sender, System.EventArgs e)
		{
			try
			{
				using (ColorDialog l_dlg = new ColorDialog())
				{
					l_dlg.Color = SelectedColor;
					if (l_dlg.ShowDialog(this) == DialogResult.OK)
					{
						SelectedColor = l_dlg.Color;
					}
				}
			}
			catch(Exception )
			{
			}
		}

		public new Color ForeColor
		{
			get{return labelColor.ForeColor;}
			set{labelColor.ForeColor = value;}
		}


		public event EventHandler SelectedColorChanged;
	}
}
