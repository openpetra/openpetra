using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
	/// <summary>
	/// Summary description for HeaderGroupBox.
	/// </summary>
	public class HeaderGroupBox : System.Windows.Forms.GroupBox
	{
		public HeaderGroupBox()
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			StringFormat format = new StringFormat();
			format.Trimming = StringTrimming.Character;
			format.Alignment = StringAlignment.Near;
			if( this.RightToLeft == RightToLeft.Yes )
			{
				format.FormatFlags = format.FormatFlags | StringFormatFlags.DirectionRightToLeft;
			}

			SizeF stringSize = e.Graphics.MeasureString( Text, Font, ClientRectangle.Size, format );

			if( Enabled )
			{
                using (Brush br = new SolidBrush(ForeColor))
                {
                    e.Graphics.DrawString(Text, Font, br, ClientRectangle, format);
                }
			}
			else
			{
				ControlPaint.DrawStringDisabled( e.Graphics, Text, Font, BackColor, ClientRectangle, format );
			}

			Pen forePen = new Pen( ControlPaint.LightLight( BackColor ), SystemInformation.BorderSize.Height );
			Pen forePenDark = new Pen( ControlPaint.Dark( BackColor ), SystemInformation.BorderSize.Height );
			Point lineLeft = new Point( ClientRectangle.Left, ClientRectangle.Top + (int)(Font.Height / 2f));
			Point lineRight = new Point( ClientRectangle.Right, ClientRectangle.Top + (int)(Font.Height / 2f));
			if( this.RightToLeft != RightToLeft.Yes )
			{
				lineLeft.X += (int)stringSize.Width;
				if (m_Image!=null)
					lineRight.X -= 17;

				if (m_Image!=null)
					e.Graphics.DrawImage(m_Image,lineRight.X+1,0,16,16);
			}
			else
			{
				lineRight.X -= (int)stringSize.Width;
				if (m_Image!=null)
					lineLeft.X += 17;

				if (m_Image!=null)
					e.Graphics.DrawImage(m_Image,0,0,16,16);
			}

			if( FlatStyle == FlatStyle.Flat )
			{
				e.Graphics.DrawLine( forePenDark, lineLeft, lineRight );				
			}
			else
			{
				e.Graphics.DrawLine( forePenDark, lineLeft, lineRight );				
				lineLeft.Offset( 0, (int)Math.Ceiling((float)SystemInformation.BorderSize.Height / 2f) );
				lineRight.Offset( 0, (int)Math.Ceiling((float)SystemInformation.BorderSize.Height / 2f) );
				e.Graphics.DrawLine( forePen, lineLeft, lineRight );
			}

			forePen.Dispose();
			forePenDark.Dispose();
		}

		private Image m_Image = null;
		public Image Image
		{
			get{return m_Image;}
			set{m_Image = value;}
		}
	}
}
