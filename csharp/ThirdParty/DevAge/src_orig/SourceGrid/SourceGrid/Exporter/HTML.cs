using System;

namespace SourceGrid.Exporter
{
	/// <summary>
    /// An utility class to export the grid in a html format file.
    /// </summary>
	public class HTML
	{
		protected ExportHTMLMode m_Mode = ExportHTMLMode.Default;
		protected System.IO.Stream m_Stream;
		protected string m_ImageFullPath;
		protected string m_ImageRelativePath;

		/// <summary>
		/// Key:Image, Value:ImageFileName
		/// </summary>
		protected System.Collections.Hashtable m_EmbeddedImagesPath = new System.Collections.Hashtable();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_Mode"></param>
		/// <param name="p_ImageFullPath">The path to write embedded images files</param>
		/// <param name="p_ImageRelativePath">The path used in the HTML source. If you save the images in the same path of the HTML file you can leave this path empty.</param>
		/// <param name="p_HtmlStream">The stream to write</param>
		public HTML(ExportHTMLMode p_Mode, string p_ImageFullPath , string p_ImageRelativePath, System.IO.Stream p_HtmlStream)
		{
			m_Mode = p_Mode;
			m_Stream = p_HtmlStream;
			m_ImageFullPath = p_ImageFullPath;
			m_ImageRelativePath = p_ImageRelativePath;
		}

		/// <summary>
		/// Export mode
		/// </summary>
		public virtual ExportHTMLMode Mode
		{
			get{return m_Mode;}
			set{m_Mode = value;}
		}

		/// <summary>
		/// List of images exported during HTML export
		/// </summary>
		public virtual string[] EmbeddedImagesPath
		{
			get
			{
				string[] l_Images = new string[m_EmbeddedImagesPath.Count];
				m_EmbeddedImagesPath.Values.CopyTo(l_Images,0);
				return l_Images;
			}
		}

		/// <summary>
		/// Clear the list of embedded images. This method don't delete the files only clear the list.
		/// </summary>
		public virtual void ClearEmbeddedImages()
		{
			m_EmbeddedImagesPath.Clear();
		}

		public virtual System.IO.Stream Stream
		{
			get{return m_Stream;}
		}

		/// <summary>
		/// Save the Image to file and returns the file
		/// </summary>
		/// <param name="p_Image"></param>
		/// <returns>Returns the path where the image is exported valid for the HTML page</returns>
		public virtual string ExportImage(System.Drawing.Image p_Image)
		{
			string l_FileName;
			if (m_EmbeddedImagesPath.ContainsKey(p_Image))
			{
				l_FileName = (string)m_EmbeddedImagesPath[p_Image];
			}
			else
			{
				l_FileName = System.IO.Path.Combine(m_ImageFullPath, System.IO.Path.GetTempFileName() + ".jpg");
				p_Image.Save(l_FileName,System.Drawing.Imaging.ImageFormat.Jpeg);

				m_EmbeddedImagesPath.Add(p_Image, l_FileName);
			}

			//change to use relative path
			l_FileName = l_FileName.Replace(m_ImageFullPath, m_ImageRelativePath);
			return l_FileName; 
		}

		/// <summary>
		/// Convert a Color to HTML compatible string
		/// </summary>
		/// <param name="p_Color"></param>
		/// <returns></returns>
		public static string ColorToHTML(System.Drawing.Color p_Color)
		{
			//non uso direttamente ToHtml su p_Color petchè se questo contiene dei valori tipo (Window, Control, cioè valori definiti in base al sistema) su Mozilla non funzionano
			return System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(p_Color.A,p_Color.R, p_Color.G, p_Color.B));
		}

		public static string BorderToHTMLStyle(DevAge.Drawing.BorderLine p_Border)
		{
			if (p_Border.Width>0)
			{
				return p_Border.Width.ToString() + "px solid " + ColorToHTML(p_Border.Color);
			}
			else
				return "none";
		}

        public static string BorderToHTMLStyle(DevAge.Drawing.IBorder border)
		{
            if (border is DevAge.Drawing.RectangleBorder)
            {
                DevAge.Drawing.RectangleBorder brd = (DevAge.Drawing.RectangleBorder)border;

                return "border-top:" + BorderToHTMLStyle(brd.Top) +
                    ";border-right:" + BorderToHTMLStyle(brd.Right) +
                    ";border-bottom:" + BorderToHTMLStyle(brd.Bottom) +
                    ";border-left:" + BorderToHTMLStyle(brd.Left) + ";";
            }
            else
                return string.Empty;
		}

		/// <summary>
		/// Export a font html element with the specified font and text
		/// </summary>
		/// <param name="p_Writer"></param>
		/// <param name="p_DisplayText"></param>
		/// <param name="p_Font"></param>
		public static void ExportHTML_Element_Font(System.Xml.XmlTextWriter p_Writer, string p_DisplayText, System.Drawing.Font p_Font)
		{
			if (p_Font != null)
			{
				p_Writer.WriteAttributeString("style","font-size:" + p_Font.SizeInPoints + "pt");

				if (p_Font.Bold)
					p_Writer.WriteStartElement("b");

				if (p_Font.Underline)
					p_Writer.WriteStartElement("u");

				if (p_Font.Italic)
					p_Writer.WriteStartElement("i");
			}

			//displaytext
			if (p_DisplayText == null || p_DisplayText.Trim().Length <= 0)
				p_Writer.WriteRaw("&nbsp;");
			else
			{
				//l_Display = l_Display.Replace("\r\n","<br>");
				p_Writer.WriteString(p_DisplayText);
			}

			if (p_Font != null)
			{
				//i
				if (p_Font.Italic)
					p_Writer.WriteEndElement();

				//u
				if (p_Font.Underline)
					p_Writer.WriteEndElement();

				//b
				if (p_Font.Bold)
					p_Writer.WriteEndElement();
			}
		}

		public void Export(GridVirtual grid)
		{
			System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(Stream, 
				System.Text.Encoding.UTF8);
			
			//write HTML and BODY
			if ( (Mode & ExportHTMLMode.HTMLAndBody) == ExportHTMLMode.HTMLAndBody)
			{
				writer.WriteStartElement("html");
				writer.WriteStartElement("body");
			}

			writer.WriteStartElement("table");

			writer.WriteAttributeString("cellspacing","0");
			writer.WriteAttributeString("cellpadding","0");

			for (int r = 0; r < grid.Rows.Count; r++)
			{
				writer.WriteStartElement("tr");

				for (int c = 0; c < grid.Columns.Count; c++)
				{
					Cells.ICellVirtual cell = grid.GetCell(r,c);
					Position pos = new Position(r,c);
					CellContext context = new CellContext(grid, pos, cell);
					ExportHTMLCell(context, writer);
				}

				//tr
				writer.WriteEndElement();
			}

			//table
			writer.WriteEndElement();

			//write end HTML and BODY
			if ( (Mode & ExportHTMLMode.HTMLAndBody) == ExportHTMLMode.HTMLAndBody)
			{
				//body
				writer.WriteEndElement();
				//html
				writer.WriteEndElement();
			}

			writer.Flush();
		}

		/// <summary>
		/// Export the specified cell to HTML
		/// </summary>
		/// <param name="context"></param>
		/// <param name="writer"></param>
		protected virtual void ExportHTMLCell(CellContext context, System.Xml.XmlTextWriter writer)
		{
			if (context.Cell == null)
			{
				//start element td
				writer.WriteStartElement("td");
				writer.WriteRaw("&nbsp;");
			}
			else
			{
				#region TD style
				Range rangeCell = context.Grid.PositionToCellRange(context.Position);
				if (rangeCell.Start != context.Position)
					return;

				//start element td
				writer.WriteStartElement("td");

				//check for rowspan and colspan 
				if (rangeCell.ColumnsCount > 1 || rangeCell.RowsCount > 1)
				{
					//colspan, rowspan
					writer.WriteAttributeString("colspan", rangeCell.ColumnsCount.ToString() );
					writer.WriteAttributeString("rowspan", rangeCell.RowsCount.ToString() );
				}

				if (context.Cell.View is Cells.Views.Cell)
				{
					Cells.Views.Cell viewCell = (Cells.Views.Cell)context.Cell.View;

					//write backcolor
					if ( (Mode & ExportHTMLMode.CellBackColor) == ExportHTMLMode.CellBackColor)
					{
						writer.WriteAttributeString("bgcolor", ColorToHTML(context.Cell.View.BackColor) );
					}

					string l_Style = "";

					//border
					l_Style = BorderToHTMLStyle(viewCell.Border);

					//style
					writer.WriteAttributeString("style", l_Style);

					//alignment
					writer.WriteAttributeString("align", DevAge.Windows.Forms.Utilities.ContentToHorizontalAlignment(viewCell.TextAlignment).ToString().ToLower());
					if (DevAge.Drawing.Utilities.IsBottom(viewCell.TextAlignment))
						writer.WriteAttributeString("valign", "bottom");
					else if (DevAge.Drawing.Utilities.IsTop(viewCell.TextAlignment))
						writer.WriteAttributeString("valign", "top");
					else if (DevAge.Drawing.Utilities.IsMiddle(viewCell.TextAlignment))
						writer.WriteAttributeString("valign", "middle");
				}
				#endregion

				if (context.Cell.View is Cells.Views.CheckBox)
				{
					Cells.Models.ICheckBox checkModel = (Cells.Models.ICheckBox)context.Cell.Model.FindModel(typeof(Cells.Models.ICheckBox));
					Cells.Models.CheckBoxStatus status = checkModel.GetCheckBoxStatus(context);
					if (status.Checked == true)
						writer.WriteRaw("<input type=\"checkbox\" checked>");
					else
						writer.WriteRaw("<input type=\"checkbox\">");
				}

				#region Font
				if (context.Cell.View is Cells.Views.Cell)
				{
					//Read the image
					System.Drawing.Image img = null;
					Cells.Models.IImage imgModel = (Cells.Models.IImage)context.Cell.Model.FindModel(typeof(Cells.Models.IImage));
					if (imgModel != null)
						img = imgModel.GetImage(context);


					Cells.Views.Cell viewCell = (Cells.Views.Cell)context.Cell.View;

					if (img != null )
					{
						writer.WriteStartElement("img");

						writer.WriteAttributeString("align", DevAge.Windows.Forms.Utilities.ContentToHorizontalAlignment(viewCell.ImageAlignment).ToString().ToLower());
						writer.WriteAttributeString("src", ExportImage(img));

						//img
						writer.WriteEndElement();
					}

					writer.WriteStartElement("font");

					writer.WriteAttributeString("color", ColorToHTML(viewCell.ForeColor));

					ExportHTMLCellContent(context, writer);

					//font
					writer.WriteEndElement();
				}
				else
					ExportHTMLCellContent(context, writer);
				#endregion

				//td
				writer.WriteEndElement();
			}
		}

		protected virtual void ExportHTMLCellContent(CellContext context, System.Xml.XmlTextWriter writer)
		{
			if (context.Cell.View is Cells.Views.CheckBox)
			{
				Cells.Models.ICheckBox checkModel = (Cells.Models.ICheckBox)context.Cell.Model.FindModel(typeof(Cells.Models.ICheckBox));
				Cells.Models.CheckBoxStatus status = checkModel.GetCheckBoxStatus(context);
				ExportHTML_Element_Font(writer, status.Caption, context.Cell.View.GetDrawingFont(context.Grid));
			}
			else
				ExportHTML_Element_Font(writer, context.DisplayText, context.Cell.View.GetDrawingFont(context.Grid));
		}	
	}


	/// <summary>
	/// Flags for the export html features (Flags)
	/// </summary>
	[Flags]
	public enum ExportHTMLMode
	{
		None = 0,
		HTMLAndBody = 1,
		GridBackColor = 2,
		CellBackColor = 4,
		RectangleBorder = 8,
		CellForeColor = 16,
		CellImages = 32,
		Default = (HTMLAndBody | GridBackColor | CellBackColor | RectangleBorder | CellForeColor | CellImages)
	}
}
