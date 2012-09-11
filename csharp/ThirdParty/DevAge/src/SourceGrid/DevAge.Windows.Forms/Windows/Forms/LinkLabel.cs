using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
	/// <summary>
	/// A LinkLabel with Image support and round border support.
	/// </summary>
	[DefaultEvent("Click")]
	public class LinkLabel : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LinkLabel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.StandardClick, true);
			SetStyle(ControlStyles.StandardDoubleClick, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);

			base.BackColor = Color.Transparent;
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
			// LinkLabel
			// 
			this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ForeColor = System.Drawing.Color.Blue;
			this.Name = "LinkLabel";
			this.Size = new System.Drawing.Size(148, 20);

		}
		#endregion

		private Image mImage;
		/// <summary>
		/// The default image to draw
		/// </summary>
		[DefaultValue(null)]
		public Image Image
		{
			get{return mImage;}
			set{mImage = value;Invalidate(true);}
		}

		private Image mMouseOverImage;
		/// <summary>
		/// Specifies the image to display when the mouse is inside the link area. If null is used the normal image.
		/// </summary>
		[DefaultValue(null)]
		public Image MouseOverImage
		{
			get{return mMouseOverImage;}
			set{mMouseOverImage = value;}
		}

		private Image mDisabledImage;
		/// <summary>
		/// Specifies the image to display when the link is disabled.
		/// </summary>
		[DefaultValue(null)]
		public Image DisabledImage
		{
			get{return mDisabledImage;}
			set{mDisabledImage = value;}
		}

		private DevAge.Drawing.ContentAlignment mImageAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
		[DefaultValue(DevAge.Drawing.ContentAlignment.MiddleLeft)]
		public DevAge.Drawing.ContentAlignment ImageAlignment
		{
			get{return mImageAlignment;}
			set{mImageAlignment = value;Invalidate(true);}
		}
		public DevAge.Drawing.ContentAlignment TextAlignment
		{
			get{return Drawing.Utilities.StringFormatToContentAlignment(mStringFormat);}
			set{Drawing.Utilities.ApplyContentAlignmentToStringFormat(value, mStringFormat);Invalidate(true);}
		}

		private StringFormat mStringFormat = new StringFormat(StringFormat.GenericDefault);
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StringFormat StringFormat
		{
			get{return mStringFormat;}
			set{mStringFormat = value;Invalidate(true);}
		}

		private bool mAlignTextToImage = true;
		[DefaultValue(true)]
		public bool AlignTextToImage
		{
			get{return mAlignTextToImage;}
			set{mAlignTextToImage = value;Invalidate(true);}
		}
		private bool mImageStretch = false;
		[DefaultValue(false)]
		public bool ImageStretch
		{
			get{return mImageStretch;}
			set{mImageStretch = value;Invalidate(true);}
		}
		private bool mEnableMouseEffect = false;
		[DefaultValue(false)]
		public bool EnableMouseEffect
		{
			get{return mEnableMouseEffect;}
			set{mEnableMouseEffect = value;}
		}

		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get{return base.Text;}
			set{base.Text = value;}
		}

		#region Border
		private int mBorderWidth = 0;
		/// <summary>
		/// Gets or sets the width of the border. If 0 no border is drawed.
		/// </summary>
		[DefaultValue(0)]
		public int BorderWidth
		{
			get{return mBorderWidth;}
			set{mBorderWidth = value;Invalidate(true);}
		}
		private double mBorderRound = 0;
		/// <summary>
		/// Round amount. If 0 the border is a not rounded.
		/// </summary>
		[DefaultValue(0)]
		public double BorderRound
		{
			get{return mBorderRound;}
			set{mBorderRound = value;Invalidate(true);}
		}
		private Color mBorderColor = Color.Black;
		/// <summary>
		/// Border color.
		/// </summary>
		public Color BorderColor
		{
			get{return mBorderColor;}
			set{mBorderColor = value;Invalidate(true);}
		}
		#endregion

		private Color mBackColor = Color.FromKnownColor(KnownColor.Control);
		public new Color BackColor
		{
			get{return mBackColor;}
			set{mBackColor = value;Invalidate();}
		}

		private bool mIsMouseOver = false;

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter (e);
			mIsMouseOver = true;
			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave (e);
			mIsMouseOver = false;
			Invalidate();
		}

		protected virtual void DrawBorderAndFill(Graphics graphics)
		{
			int borderWidth = BorderWidth;
			double borderRound = BorderRound;
			Color borderColor = BorderColor;
			Color fillColor = BackColor;

			if (mEnableMouseEffect && mIsMouseOver && Enabled)
			{
				Color highLight = Color.FromKnownColor(KnownColor.Highlight);
				fillColor = Color.FromArgb(75, highLight);
				if (borderWidth == 0)
					borderWidth = 1;
				borderColor = highLight;
			}

			DevAge.Drawing.RoundedRectangle roundedRect = new DevAge.Drawing.RoundedRectangle(ClientRectangle, borderRound);
			using (SolidBrush brush = new SolidBrush(fillColor))
			{
				DevAge.Drawing.Utilities.FillRoundedRectangle(graphics, roundedRect, brush);
			}

			if (borderWidth > 0)
			{
				using (Pen pen = new Pen(borderColor, borderWidth))
				{
					DevAge.Drawing.Utilities.DrawRoundedRectangle(graphics, roundedRect, pen);
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			if (BorderRound > 0)
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			DrawBorderAndFill(e.Graphics);

			bool disabledImage = false;

			Image image = mImage;
			if (Enabled == false)
			{
				if (mDisabledImage != null)
					image = mDisabledImage;
				else
					disabledImage = true;
			}
			else if (mIsMouseOver && mMouseOverImage != null)
				image = mMouseOverImage;

			Rectangle contentRect = ClientRectangle;
			if (BorderWidth > 0)
				contentRect = new Rectangle(contentRect.X + BorderWidth, contentRect.Y + BorderWidth,
											contentRect.Width - BorderWidth * 2, contentRect.Height - BorderWidth * 2);

            DevAge.Drawing.VisualElements.Container container = new DevAge.Drawing.VisualElements.Container();
            DevAge.Drawing.VisualElements.TextGDI textElement = new DevAge.Drawing.VisualElements.TextGDI(Text);
            DevAge.Drawing.VisualElements.Image imageElement = new DevAge.Drawing.VisualElements.Image(image);
            imageElement.AnchorArea = new DevAge.Drawing.AnchorArea(mImageAlignment, mImageStretch);
            imageElement.Enabled = !disabledImage;
            textElement.AnchorArea = new DevAge.Drawing.AnchorArea(DevAge.Drawing.Utilities.StringFormatToContentAlignment(mStringFormat), false);
            textElement.StringFormat = mStringFormat;
            textElement.Font = Font;
            textElement.ForeColor = ForeColor;
            textElement.Enabled = Enabled;
            container.Elements.Add(imageElement);
            container.Elements.Add(textElement);



            using (DevAge.Drawing.GraphicsCache cache = new DevAge.Drawing.GraphicsCache(e.Graphics, e.ClipRectangle))
            {
                container.Draw(cache, contentRect);
            }
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged (e);

			Invalidate(true);
		}
	}
}
