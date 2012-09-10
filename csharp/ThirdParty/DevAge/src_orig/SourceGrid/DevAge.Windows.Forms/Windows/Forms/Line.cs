using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
	/// <summary>
	/// Summary description for Line.
	/// </summary>
	public class Line : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Line()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.FixedHeight | ControlStyles.FixedWidth | ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.Selectable, false);
			TabStop = false;
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
			// 
			// Line
			// 
			this.Name = "Line";
			this.Size = new System.Drawing.Size(140, 140);

		}
		#endregion

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			int x1, x2, x3, x4, y1, y2, y3, y4;
			if (LineStyle == LineStyle.Horizontal)
			{
				x1 = 0;
				y1 = 0;
				x2 = ClientRectangle.Width;
				y2 = 0;

				x3 = 0;
				y3 = 1;
				x4 = ClientRectangle.Width;
				y4 = 1;
			}
			else //if (LineStyle == LineStyle.Vertical)
			{
				x1 = 0;
				y1 = 0;
				x2 = 0;
				y2 = ClientRectangle.Height;

				x3 = 1;
				y3 = 0;
				x4 = 1;
				y4 = ClientRectangle.Height;
			}

			using (Pen p = new Pen(m_FirstColor, 1))
			{
				p.DashStyle = mDashStyle;
				e.Graphics.DrawLine(p, x1, y1, x2, y2);
			}
			using (Pen p = new Pen(m_SecondColor, 1))
			{
				p.DashStyle = mDashStyle;
				e.Graphics.DrawLine(p, x3, y3, x4, y4);
			}
		}

		private System.Drawing.Drawing2D.DashStyle mDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
		public System.Drawing.Drawing2D.DashStyle DashStyle
		{
			get{return mDashStyle;}
			set{mDashStyle = value;Invalidate();}
		}

		private Color m_FirstColor = Color.FromKnownColor(KnownColor.ControlDark);
		public Color FirstColor
		{
			get{return m_FirstColor;}
			set{m_FirstColor = value;Invalidate();}
		}

		private Color m_SecondColor = Color.FromKnownColor(KnownColor.ControlLightLight);
		public Color SecondColor
		{
			get{return m_SecondColor;}
			set{m_SecondColor = value;Invalidate();}
		}
	
		private LineStyle m_LineStyle = LineStyle.Horizontal;
		public LineStyle LineStyle
		{
			get{return m_LineStyle;}
			set
			{
				m_LineStyle = value;
				Size = new Size(Height, Width);
				Invalidate();
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);

			ChangeControlSize();
		}

		private void ChangeControlSize()
		{
			if (LineStyle == LineStyle.Horizontal)
				Height = 2;
			else if (LineStyle == LineStyle.Vertical)
				Width = 2;
		}
	}

	public enum LineStyle
	{
		Horizontal = 1,
		Vertical = 2
	}
}
