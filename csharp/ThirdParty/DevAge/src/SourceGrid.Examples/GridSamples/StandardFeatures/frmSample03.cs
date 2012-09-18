using SourceGrid;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevAge.Windows.Forms;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for frmSample3.
	/// </summary>
	[Sample("SourceGrid - Standard features", 3, "Editors and Types")]
	public class frmSample03 : System.Windows.Forms.Form
	{
		private SourceGrid.Grid grid;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSample03()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grid = new SourceGrid.Grid();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                          | System.Windows.Forms.AnchorStyles.Left)
			                                                         | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.AutoStretchColumnsToFitWidth = false;
			this.grid.AutoStretchRowsToFitHeight = false;
			this.grid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.grid.CustomSort = false;
			this.grid.Location = new System.Drawing.Point(12, 12);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(516, 368);
			this.grid.SpecialKeys = SourceGrid.GridSpecialKeys.Default;
			this.grid.TabIndex = 0;
			// 
			// frmSample3
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(540, 391);
			this.Controls.Add(this.grid);
			this.Name = "frmSample3";
			this.Text = "Cell Editors, Specials Cells, Formatting and Image";
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			grid.Redim(62, 3);

			SourceGrid.Cells.Views.Cell titleModel = new SourceGrid.Cells.Views.Cell();
			titleModel.BackColor = Color.SteelBlue;
			titleModel.ForeColor = Color.White;
			titleModel.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
			SourceGrid.Cells.Views.Cell captionModel = new SourceGrid.Cells.Views.Cell();
			captionModel.BackColor = grid.BackColor;

			int currentRow = 0;

			#region Base Types
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Base Types");
			grid[currentRow, 0].View = titleModel;
			grid[currentRow, 0].ColumnSpan = 3;
			currentRow++;

			//string
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("String");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell("String Value", typeof(string));

			currentRow++;

			//double
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Double");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(1.5, typeof(double));

			currentRow++;

			//int
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Int");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(5, typeof(int));

			currentRow++;

			//DateTime
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("DateTime");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(DateTime.Now, typeof(DateTime));

			currentRow++;

			//Boolean
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Boolean");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(true, typeof(Boolean));

			currentRow++;
			#endregion

			#region Complex Types
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Complex Types");
			grid[currentRow, 0].View = titleModel;
			grid[currentRow, 0].ColumnSpan = 3;
			currentRow++;

			//Font
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Font");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(this.Font, typeof(Font));

			currentRow++;

			//Cursor
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Cursor");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(Cursors.Arrow, typeof(Cursor));

			currentRow++;

			//Point
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Point");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(new Point(2, 3), typeof(Point));

			currentRow++;

			//Rectangle
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Rectangle");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(new Rectangle(100, 100, 200, 200), typeof(Rectangle));

			currentRow++;

			//Image
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Image");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Image(Properties.Resources.CalcioSmall);

			currentRow++;

			//Enum AnchorStyle
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("AnchorStyle");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(AnchorStyles.Bottom, typeof(AnchorStyles));

			currentRow++;

			//Enum
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Enum");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(System.Windows.Forms.BorderStyle.Fixed3D, typeof(System.Windows.Forms.BorderStyle));

			currentRow++;

			//String[]
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("String Array");
			grid[currentRow, 0].View = captionModel;
			string[] strArray = new string[] { "Value 1", "Value 2" };
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(strArray, typeof(string[]));

			currentRow++;

			//Double[]
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Double Array");
			grid[currentRow, 0].View = captionModel;
			double[] dblArray = new double[] { 1, 0.5, 0.1 };
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(dblArray, typeof(double[]));

			currentRow++;
			#endregion

			#region Special Editors
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Special Editors");
			grid[currentRow, 0].View = titleModel;
			grid[currentRow, 0].ColumnSpan = 3;
			currentRow++;

			//Time
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Time");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(DateTime.Now);
			grid[currentRow, 1].Editor = new SourceGrid.Cells.Editors.TimePicker();

			currentRow++;

			//Double Chars Validation
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Double Chars Validation");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.TextBoxNumeric numericEditor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(double));
			numericEditor.KeyPress += delegate(object sender, KeyPressEventArgs keyArgs)
			{
				bool isValid = char.IsNumber(keyArgs.KeyChar) ||
					keyArgs.KeyChar == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

				keyArgs.Handled = !isValid;
			};
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(0.5);
			grid[currentRow, 1].Editor = numericEditor;

			currentRow++;

			//String Chars (ABC)
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Chars Validation(only ABC)");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.TextBox stringEditor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
			stringEditor.KeyPress += delegate(object sender, KeyPressEventArgs keyArgs)
			{
				keyArgs.KeyChar = char.ToUpper(keyArgs.KeyChar);
				bool isValid = keyArgs.KeyChar == 'A' || keyArgs.KeyChar == 'B' || keyArgs.KeyChar == 'C';

				keyArgs.Handled = !isValid;
			};
			grid[currentRow, 1] = new SourceGrid.Cells.Cell("AABB");
			grid[currentRow, 1].Editor = stringEditor;

			currentRow++;

			//String Validating
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("String validating(min 6 chars)");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.TextBox stringEditorValidating = new SourceGrid.Cells.Editors.TextBox(typeof(string));
			stringEditorValidating.Control.Validating += delegate(object sender, CancelEventArgs cancelEvent)
			{
				string val = ((TextBox)sender).Text;
				if (val == null || val.Length < 6)
					cancelEvent.Cancel = true;
			};
			grid[currentRow, 1] = new SourceGrid.Cells.Cell("test string");
			grid[currentRow, 1].Editor = stringEditorValidating;

			currentRow++;

			//Int 0-100 or null
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Int 0-100 or null");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.TextBoxNumeric numericEditor0_100 = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(int));
			numericEditor0_100.MinimumValue = 0;
			numericEditor0_100.MaximumValue = 100;
			numericEditor0_100.AllowNull = true;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(7);
			grid[currentRow, 1].Editor = numericEditor0_100;

			currentRow++;

			//Double Custom Conversion
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Dbl custom conversion");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.TextBox dblCustomConversion = new SourceGrid.Cells.Editors.TextBox(typeof(double));
			dblCustomConversion.ConvertingObjectToValue += delegate(object sender,
			                                                        DevAge.ComponentModel.ConvertingObjectEventArgs conv)
			{
				if (conv.Value is string)
				{
					//Here you can add any custom code
					double val;
					if (double.TryParse((string)conv.Value, out val))
					{
						conv.Value = val;
						conv.ConvertingStatus = DevAge.ComponentModel.ConvertingStatus.Completed;
					}
				}
			};
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(73.839);
			grid[currentRow, 1].Editor = dblCustomConversion;

			currentRow++;

			//Enum Custom Display
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Enum Custom Display");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.ComboBox keysCombo = new SourceGrid.Cells.Editors.ComboBox(typeof(Keys));
			keysCombo.Control.FormattingEnabled = true;
			keysCombo.ConvertingValueToDisplayString += delegate(object sender, DevAge.ComponentModel.ConvertingObjectEventArgs convArgs)
			{
				if (convArgs.Value is Keys)
					convArgs.Value = (int)((Keys)convArgs.Value) + " - " + convArgs.Value.ToString();
			};
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(Keys.Enter);
			grid[currentRow, 1].Editor = keysCombo;

			currentRow++;

			string[] arraySample = new string[] { "Value 1", "Value 2", "Value 3" };
			//ComboBox 1
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("ComboBox String");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.ComboBox comboStandard = new SourceGrid.Cells.Editors.ComboBox(typeof(string), arraySample, false);
			comboStandard.Control.MaxLength = 10;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(arraySample[0], comboStandard);

			currentRow++;

			//ComboBox exclusive
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("ComboBox String Exclusive");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.ComboBox comboExclusive = new SourceGrid.Cells.Editors.ComboBox(typeof(string), arraySample, true);
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(arraySample[0], comboExclusive);

			currentRow++;

			//ComboBox AutoComplete
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("ComboBox AutoComplete");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.ComboBox comboAutoComplete =
				new SourceGrid.Cells.Editors.ComboBox(typeof(string),
				                                      new string[] { "AAA", "ABC", "AZA", "BAA", "ZAA" },
				                                      true);
			comboAutoComplete.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
			comboAutoComplete.Control.AutoCompleteMode = AutoCompleteMode.Append;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell("AAA", comboAutoComplete);

			currentRow++;

			//ComboBox DropDownList
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("ComboBox DropDownList");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.ComboBox comboNoText = new SourceGrid.Cells.Editors.ComboBox(typeof(string), arraySample, true);
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(arraySample[0]);
			grid[currentRow, 1].Editor = comboNoText;
			comboNoText.Control.DropDownStyle = ComboBoxStyle.DropDownList;

			currentRow++;

			//ComboBox DateTime Editable
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("ComboBox DateTime");
			grid[currentRow, 0].View = captionModel;
			DateTime[] arrayDt = new DateTime[] { new DateTime(1981, 10, 6), new DateTime(1991, 10, 6), new DateTime(2001, 10, 6) };
			SourceGrid.Cells.Editors.ComboBox comboDateTime = new SourceGrid.Cells.Editors.ComboBox(typeof(DateTime), arrayDt, false);
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(arrayDt[0], comboDateTime);

			currentRow++;

			//ComboBox Custom Display (create a datamodel that has a custom display string)
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("ComboBox Custom Display");
			grid[currentRow, 0].View = captionModel;
			int[] arrInt = new int[] { 0, 1, 2, 3, 4 };
			string[] arrStr = new string[] { "0 - Zero", "1 - One", "2 - Two", "3 - Three", "4- Four" };
			SourceGrid.Cells.Editors.ComboBox editorComboCustomDisplay = new SourceGrid.Cells.Editors.ComboBox(typeof(int), arrInt, true);
			editorComboCustomDisplay.Control.FormattingEnabled = true;
			DevAge.ComponentModel.Validator.ValueMapping comboMapping = new DevAge.ComponentModel.Validator.ValueMapping();
			comboMapping.DisplayStringList = arrStr;
			comboMapping.ValueList = arrInt;
			comboMapping.SpecialList = arrStr;
			comboMapping.SpecialType = typeof(string);
			comboMapping.BindValidator(editorComboCustomDisplay);
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(0);
			grid[currentRow, 1].Editor = editorComboCustomDisplay;

			SourceGrid.Cells.Cell l_CellComboRealValue = new SourceGrid.Cells.Cell(grid[currentRow, 1].Value);
			l_CellComboRealValue.View = captionModel;
			SourceGrid.Cells.Controllers.BindProperty l_ComboBindProperty = new SourceGrid.Cells.Controllers.BindProperty(typeof(SourceGrid.Cells.Cell).GetProperty("Value"), l_CellComboRealValue);
			grid[currentRow, 1].AddController(l_ComboBindProperty);
			grid[currentRow, 2] = l_CellComboRealValue;

			currentRow++;

			//ComboBox with inline View
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("ComboBox Inline View");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.ComboBox cbInline = new SourceGrid.Cells.Editors.ComboBox(typeof(string), arraySample, false);
			cbInline.EditableMode = SourceGrid.EditableMode.Default | SourceGrid.EditableMode.Focus;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(arraySample[0], cbInline);
			grid[currentRow, 1].View = SourceGrid.Cells.Views.ComboBox.Default;

			currentRow++;

			//Numeric Up Down Editor
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("NumericUpDown");
			grid[currentRow, 0].View = captionModel;

			grid[currentRow, 1] = new SourceGrid.Cells.Cell(0);
			SourceGrid.Cells.Editors.NumericUpDown l_NumericUpDownEditor = new SourceGrid.Cells.Editors.NumericUpDown(typeof(int), 50, -50, 1);
			grid[currentRow, 1].Editor = l_NumericUpDownEditor;

			currentRow++;

			//Multiline Textbox
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Multiline Textbox");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 0].ColumnSpan = 1;
			grid[currentRow, 0].RowSpan = 2;

			grid[currentRow, 1] = new SourceGrid.Cells.Cell("Hello\r\nWorld");
			SourceGrid.Cells.Editors.TextBox l_MultilineEditor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
			l_MultilineEditor.Control.Multiline = true;
			grid[currentRow, 1].Editor = l_MultilineEditor;
			grid[currentRow, 1].RowSpan = 2;

			currentRow++;
			currentRow++;

			//Boolean (CheckBox)
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Boolean (CheckBox)");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.CheckBox(null, true);
			grid[currentRow, 1].FindController<SourceGrid.Cells.Controllers.CheckBox>().CheckedChanged += InvertDisabledCheckBox(currentRow);
			

			SourceGrid.Cells.CheckBox l_DisabledCheckBox = new SourceGrid.Cells.CheckBox("Disabled Checkbox", true);
			l_DisabledCheckBox.Editor.EnableEdit = false;
			grid[currentRow, 2] = l_DisabledCheckBox;

			currentRow++;

			//DateTime with DateTimePicker Editor
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("DateTimePicker");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.DateTimePicker editorDtPicker = new SourceGrid.Cells.Editors.DateTimePicker();
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(DateTime.Today);
			grid[currentRow, 1].Editor = editorDtPicker;

			currentRow++;

			//DateTime with DateTimePicker nullable Editor
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("DateTimePicker nullable");
			grid[currentRow, 0].View = captionModel;
			SourceGrid.Cells.Editors.DateTimePicker editorDtPickerNull = new SourceGrid.Cells.Editors.DateTimePicker();
			editorDtPickerNull.AllowNull = true;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(null);
			grid[currentRow, 1].Editor = editorDtPickerNull;

			currentRow++;

			//File editor
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("File editor");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell("c:\\windows\\System32\\user32.dll", new EditorFileDialog());

			currentRow++;
			
			// Richtext box
            grid[currentRow, 0] = new SourceGrid.Cells.Cell("RichTextBox editor");
            grid[currentRow, 0].View = captionModel;
            string rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0" +
                    "Microsoft Sans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17 Only a \\b " +
                    "Test\\b0.\\par\r\n}\r\n";
			var richTextBox = new SourceGrid.Cells.RichTextBox(new RichText(rtf));
			grid[currentRow, 1] = richTextBox;

			currentRow++;

			#endregion

			#region Special Cells
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Special Cells");
			grid[currentRow, 0].View = titleModel;
			grid[currentRow, 0].ColumnSpan = 3;
			currentRow++;

			//Cell Button
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Cell Button");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Button("CellButton");
			grid[currentRow, 1].Image = Properties.Resources.FACE02.ToBitmap();
			SourceGrid.Cells.Controllers.Button buttonClickEvent = new SourceGrid.Cells.Controllers.Button();
			buttonClickEvent.Executed += new EventHandler(CellButton_Click);
			grid[currentRow, 1].Controller.AddController(buttonClickEvent);

			currentRow++;

			//Cell Link
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Cell Link");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Link("CellLink");
			SourceGrid.Cells.Controllers.Button linkClickEvent = new SourceGrid.Cells.Controllers.Button();
			linkClickEvent.Executed += new EventHandler(CellLink_Click);
			grid[currentRow, 1].Controller.AddController(linkClickEvent);

			currentRow++;

			//Custom draw cell
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Custom draw cell");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell("CustomView");
			grid[currentRow, 1].View = new RoundView();

			currentRow++;

			//Control Cell
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Control Cell");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell("control cell");
			ProgressBar progressBar = new ProgressBar();
			progressBar.Value = 50;
			grid.LinkedControls.Add(new SourceGrid.LinkedControlValue(progressBar, new SourceGrid.Position(currentRow, 1)));

			currentRow++;

			//Custom Border Cell
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Custom Border");
			grid[currentRow, 0].View = captionModel;

			SourceGrid.Cells.Views.Cell viewCustomBorder = new SourceGrid.Cells.Views.Cell();
			viewCustomBorder.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Red, 1),
			                                                             new DevAge.Drawing.BorderLine(Color.Blue, 1),
			                                                             new DevAge.Drawing.BorderLine(Color.Violet, 1),
			                                                             new DevAge.Drawing.BorderLine(Color.Green, 1));
			grid[currentRow, 1] = new SourceGrid.Cells.Cell("Custom Border");
			grid[currentRow, 1].View = viewCustomBorder;

			currentRow++;
			#endregion

			#region Custom Formatting
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Custom Formatting");
			grid[currentRow, 0].View = titleModel;
			grid[currentRow, 0].ColumnSpan = 3;
			currentRow++;

			//Format
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Default Format");
			grid[currentRow, 0].View = captionModel;

			grid[currentRow, 1] = new SourceGrid.Cells.Cell(88.5246);
			SourceGrid.Cells.Editors.TextBox editorCustom = new SourceGrid.Cells.Editors.TextBox(typeof(double));
			editorCustom.TypeConverter = new DevAge.ComponentModel.Converter.CurrencyTypeConverter(typeof(double));
			DevAge.ComponentModel.Converter.NumberTypeConverter numberFormatCustom = new DevAge.ComponentModel.Converter.NumberTypeConverter(typeof(double));
			editorCustom.TypeConverter = numberFormatCustom;
			grid[currentRow, 1].Editor = editorCustom;

			currentRow++;

			//Percent Editor
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Percent Format");
			grid[currentRow, 0].View = captionModel;

			grid[currentRow, 1] = new SourceGrid.Cells.Cell(88.5246);
			SourceGrid.Cells.Editors.TextBox l_PercentEditor = new SourceGrid.Cells.Editors.TextBox(typeof(double));
			l_PercentEditor.TypeConverter = new DevAge.ComponentModel.Converter.PercentTypeConverter(typeof(double));
			grid[currentRow, 1].Editor = l_PercentEditor;

			currentRow++;

			//Currency Editor
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Currency Format");
			grid[currentRow, 0].View = captionModel;

			grid[currentRow, 1] = new SourceGrid.Cells.Cell(88.5246M);
			SourceGrid.Cells.Editors.TextBox l_CurrencyEditor = new SourceGrid.Cells.Editors.TextBox(typeof(decimal));
			l_CurrencyEditor.TypeConverter = new DevAge.ComponentModel.Converter.CurrencyTypeConverter(typeof(decimal));
			grid[currentRow, 1].Editor = l_CurrencyEditor;

			currentRow++;

			//Format (#.00)
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Format #.00");
			grid[currentRow, 0].View = captionModel;

			grid[currentRow, 1] = new SourceGrid.Cells.Cell(88.5246);
			editorCustom = new SourceGrid.Cells.Editors.TextBox(typeof(double));
			numberFormatCustom = new DevAge.ComponentModel.Converter.NumberTypeConverter(typeof(double));
			numberFormatCustom.Format = "#.00";
			editorCustom.TypeConverter = numberFormatCustom;
			grid[currentRow, 1].Editor = editorCustom;

			currentRow++;

			//Format ("0000.0000")
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Format 0000.0000");
			grid[currentRow, 0].View = captionModel;

			grid[currentRow, 1] = new SourceGrid.Cells.Cell(88.5246);
			editorCustom = new SourceGrid.Cells.Editors.TextBox(typeof(double));
			numberFormatCustom = new DevAge.ComponentModel.Converter.NumberTypeConverter(typeof(double));
			numberFormatCustom.Format = "0000.0000";
			editorCustom.TypeConverter = numberFormatCustom;
			grid[currentRow, 1].Editor = editorCustom;

			currentRow++;

			//Format ("Scientific (exponential)")
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Format Scientific");
			grid[currentRow, 0].View = captionModel;

			grid[currentRow, 1] = new SourceGrid.Cells.Cell(0.0006);
			SourceGrid.Cells.Editors.TextBoxNumeric editorExponential = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(double));
			DevAge.ComponentModel.Converter.NumberTypeConverter exponentialConverter = new DevAge.ComponentModel.Converter.NumberTypeConverter(typeof(double), "e");
			exponentialConverter.NumberStyles = System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowExponent;
			editorExponential.TypeConverter = exponentialConverter;
			grid[currentRow, 1].Editor = editorExponential;

			currentRow++;

			//DateTime 2 (using custom formatting)
			string dtFormat2 = "yyyy MM dd";
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Date(" + dtFormat2 + ")");
			grid[currentRow, 0].View = captionModel;

			string[] dtParseFormats = new string[] { dtFormat2 };
			System.Globalization.DateTimeStyles dtStyles = System.Globalization.DateTimeStyles.AllowInnerWhite | System.Globalization.DateTimeStyles.AllowLeadingWhite | System.Globalization.DateTimeStyles.AllowTrailingWhite | System.Globalization.DateTimeStyles.AllowWhiteSpaces;
			TypeConverter dtConverter = new DevAge.ComponentModel.Converter.DateTimeTypeConverter(dtFormat2, dtParseFormats, dtStyles);
			SourceGrid.Cells.Editors.TextBoxUITypeEditor editorDt2 = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(DateTime));
			editorDt2.TypeConverter = dtConverter;

			grid[currentRow, 1] = new SourceGrid.Cells.Cell(DateTime.Today);
			grid[currentRow, 1].Editor = editorDt2;

			currentRow++;


			//Text Ellipses
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Text Ellipses");
			grid[currentRow, 0].View = captionModel;

			grid[currentRow, 1] = new SourceGrid.Cells.Cell("This text is very very long and shows how to trim and add ellipses", typeof(string));
			SourceGrid.Cells.Views.Cell ellipsesView = new SourceGrid.Cells.Views.Cell();
			ellipsesView.TrimmingMode = SourceGrid.TrimmingMode.Word;
			grid[currentRow, 1].View = ellipsesView;

			currentRow++;

			#endregion

			#region Image And Text Properties
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Image And Text Properties");
			grid[currentRow, 0].View = titleModel;
			grid[currentRow, 0].ColumnSpan = 3;
			currentRow++;

			//Cell Image
			SourceGrid.Cells.Cell cellImage1 = new SourceGrid.Cells.Cell("Single Image");
			SourceGrid.Cells.Views.Cell viewImage = new SourceGrid.Cells.Views.Cell(captionModel);
			cellImage1.View = viewImage;
			grid[currentRow, 2] = cellImage1;
			cellImage1.RowSpan = 4;
			cellImage1.Image = Properties.Resources.FACE02.ToBitmap();

			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Image Alignment");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(viewImage.ImageAlignment, typeof(DevAge.Drawing.ContentAlignment));
			grid[currentRow, 1].AddController(new SourceGrid.Cells.Controllers.BindProperty(typeof(SourceGrid.Cells.Views.Cell).GetProperty("ImageAlignment"), viewImage));

			currentRow++;

			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Stretch Image");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(viewImage.ImageStretch, typeof(bool));
			grid[currentRow, 1].AddController(new SourceGrid.Cells.Controllers.BindProperty(typeof(SourceGrid.Cells.Views.Cell).GetProperty("ImageStretch"), viewImage));

			currentRow++;

			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Text Alignment");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(viewImage.TextAlignment, typeof(DevAge.Drawing.ContentAlignment));
			grid[currentRow, 1].AddController(new SourceGrid.Cells.Controllers.BindProperty(typeof(SourceGrid.Cells.Views.Cell).GetProperty("TextAlignment"), viewImage));

			currentRow++;

			grid[currentRow, 0] = new SourceGrid.Cells.Cell("DrawMode");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell(viewImage.ElementsDrawMode, typeof(DevAge.Drawing.ElementsDrawMode));
			grid[currentRow, 1].AddController(new SourceGrid.Cells.Controllers.BindProperty(typeof(SourceGrid.Cells.Views.Cell).GetProperty("ElementsDrawMode"), viewImage));

			currentRow++;


			// Cell VisualModelMultiImages
			grid[currentRow, 1] = new SourceGrid.Cells.Cell("Multi Images");
			SourceGrid.Cells.Views.MultiImages modelMultiImages = new SourceGrid.Cells.Views.MultiImages();
			modelMultiImages.SubImages.Add(new DevAge.Drawing.VisualElements.Image(Properties.Resources.FACE00.ToBitmap()));
			modelMultiImages.SubImages.Add(new DevAge.Drawing.VisualElements.Image(Properties.Resources.FACE01.ToBitmap()));
			modelMultiImages.SubImages.Add(new DevAge.Drawing.VisualElements.Image(Properties.Resources.FACE02.ToBitmap()));
			modelMultiImages.SubImages.Add(new DevAge.Drawing.VisualElements.Image(Properties.Resources.FACE04.ToBitmap()));
			modelMultiImages.SubImages[0].AnchorArea = new DevAge.Drawing.AnchorArea(DevAge.Drawing.ContentAlignment.TopLeft, false);
			modelMultiImages.SubImages[1].AnchorArea = new DevAge.Drawing.AnchorArea(DevAge.Drawing.ContentAlignment.TopRight, false);
			modelMultiImages.SubImages[2].AnchorArea = new DevAge.Drawing.AnchorArea(DevAge.Drawing.ContentAlignment.BottomLeft, false);
			modelMultiImages.SubImages[3].AnchorArea = new DevAge.Drawing.AnchorArea(DevAge.Drawing.ContentAlignment.BottomRight, false);
			modelMultiImages.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
			grid[currentRow, 1].View = modelMultiImages;
			grid.Rows[currentRow].AutoSizeMode = SourceGrid.AutoSizeMode.MinimumSize;
			grid.Rows[currentRow].Height = 50;

			currentRow++;


			// Cell Rotated Text
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("Rotated by angle");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell("Rotated Text", typeof(string));
			SourceGrid.Cells.Views.Cell rotateView = new SourceGrid.Cells.Views.Cell();
			rotateView.ElementText = new RotatedText(45);
			grid[currentRow, 1].View = rotateView;
			grid.Rows[currentRow].AutoSizeMode = SourceGrid.AutoSizeMode.MinimumSize;
			grid.Rows[currentRow].Height = 50;

			currentRow++;

			// GDI+ Text
			grid[currentRow, 0] = new SourceGrid.Cells.Cell("GDI+ Text");
			grid[currentRow, 0].View = captionModel;
			grid[currentRow, 1] = new SourceGrid.Cells.Cell("Hello from GDI+", typeof(string));
			GDITextView gdiTextView = new GDITextView();
			gdiTextView.FormatFlags = StringFormatFlags.DirectionVertical | StringFormatFlags.NoWrap;
			grid[currentRow, 1].View = gdiTextView;

			currentRow++;
			#endregion


			grid.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.MinimumSize | SourceGrid.AutoSizeMode.Default;
			grid.Columns[1].AutoSizeMode = SourceGrid.AutoSizeMode.MinimumSize | SourceGrid.AutoSizeMode.Default;
			grid.Columns[2].AutoSizeMode = SourceGrid.AutoSizeMode.MinimumSize | SourceGrid.AutoSizeMode.Default;
			grid.MinimumWidth = 50;
			grid.AutoSizeCells();
			grid.AutoStretchColumnsToFitWidth = true;
			grid.Columns.StretchToFit();
		}

		private EventHandler InvertDisabledCheckBox(int row)
		{
			return delegate
			{
				grid[row, 2].Value = !(((bool)grid[row, 2].Value)) ;
			};
		}
		
		private void CellButton_Click(object sender, EventArgs e)
		{
			SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
			SourceGrid.Cells.Button btnCell = (SourceGrid.Cells.Button)context.Cell;
			btnCell.Value = DateTime.Now.TimeOfDay.ToString();
		}

		private void CellLink_Click(object sender, EventArgs e)
		{
			SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
			SourceGrid.Cells.Link btnCell = (SourceGrid.Cells.Link)context.Cell;
			btnCell.Value = DateTime.Now.TimeOfDay.ToString();
		}


		/// <summary>
		/// Customized View to draw a rounded background
		/// </summary>
		private class RoundView : SourceGrid.Cells.Views.Cell
		{
			public RoundView()
			{
				TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
				base.Background = mBackground;
				Border = DevAge.Drawing.RectangleBorder.NoBorder;
			}

			public double Round
			{
				get { return mBackground.Round; }
				set { mBackground.Round = value; }
			}

			private BackVisualElement mBackground = new BackVisualElement();
			
			private class BackVisualElement : DevAge.Drawing.VisualElements.VisualElementBase
			{
				#region Constuctor
				/// <summary>
				/// Default constructor
				/// </summary>
				public BackVisualElement()
				{
				}

				/// <summary>
				/// Copy constructor
				/// </summary>
				/// <param name="other"></param>
				public BackVisualElement(BackVisualElement other)
					: base(other)
				{
					Round = other.Round;
				}
				#endregion
				/// <summary>
				/// Clone
				/// </summary>
				/// <returns></returns>
				public override object Clone()
				{
					return new BackVisualElement(this);
				}

				private double mRound = 0.5;
				public double Round
				{
					get { return mRound; }
					set { mRound = value; }
				}

				protected override void OnDraw(DevAge.Drawing.GraphicsCache graphics, RectangleF area)
				{
					DevAge.Drawing.RoundedRectangle rounded = new DevAge.Drawing.RoundedRectangle(Rectangle.Round(area), Round);
					using (System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(area, Color.RoyalBlue, Color.WhiteSmoke, System.Drawing.Drawing2D.LinearGradientMode.Vertical))
					{
						graphics.Graphics.FillRegion(brush, new Region(rounded.ToGraphicsPath()));
					}


					//Border
					DevAge.Drawing.Utilities.DrawRoundedRectangle(graphics.Graphics, rounded, Pens.RoyalBlue);

				}

				protected override SizeF OnMeasureContent(DevAge.Drawing.MeasureHelper measure, SizeF maxSize)
				{
					return SizeF.Empty;
				}
			}
		}

		private class RotatedText : DevAge.Drawing.VisualElements.TextGDI
		{
			public RotatedText(float angle)
			{
				Angle = angle;
			}

			public float Angle = 0;

			protected override void OnDraw(DevAge.Drawing.GraphicsCache graphics, RectangleF area)
			{
				System.Drawing.Drawing2D.GraphicsState state = graphics.Graphics.Save();
				try
				{
					float width2 = area.Width / 2;
					float height2 = area.Height / 2;

					//For a better drawing use the clear type rendering
					graphics.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

					//Move the origin to the center of the cell (for a more easy rotation)
					graphics.Graphics.TranslateTransform(area.X + width2, area.Y + height2);

					graphics.Graphics.RotateTransform(Angle);

					StringFormat.Alignment = StringAlignment.Center;
					StringFormat.LineAlignment = StringAlignment.Center;
					graphics.Graphics.DrawString(Value, Font, graphics.BrushsCache.GetBrush(ForeColor), 0, 0, StringFormat);
				}
				finally
				{
					graphics.Graphics.Restore(state);
				}
			}

			//TODO Implement Clone and MeasureContent
			//Here I should also implement MeasureContent (think also for a solution to allow rotated text with any kind of allignment)
		}

		/// <summary>
		/// Customized View to directly use GDI+ StringFormat features.
		/// </summary>
		private class GDITextView : SourceGrid.Cells.Views.Cell
		{
			public GDITextView()
			{
				ElementText = new DevAge.Drawing.VisualElements.TextGDI();
			}

			private System.Drawing.StringFormatFlags mFormatFlags = StringFormatFlags.NoWrap;
			public System.Drawing.StringFormatFlags FormatFlags
			{
				get { return mFormatFlags; }
				set { mFormatFlags = value; }
			}

			protected override void PrepareVisualElementText(SourceGrid.CellContext context)
			{
				base.PrepareVisualElementText(context);

				((DevAge.Drawing.VisualElements.TextGDI)ElementText).StringFormat.FormatFlags = FormatFlags;
			}

			protected override SizeF OnMeasureContent(DevAge.Drawing.MeasureHelper measure, SizeF maxSize)
			{
				//TODO Here I must add some fixed pixels because seems that there are some little
				// difference from the measured content and the actual drawing surface (probably this is a little bug ... I must check)
				SizeF result = base.OnMeasureContent(measure, maxSize);
				return new SizeF(result.Width + 1, result.Height + 1);
			}
		}

		private class EditorFileDialog : SourceGrid.Cells.Editors.TextBoxButton
		{
			public EditorFileDialog()
				: base(typeof(string))
			{
				Control.DialogOpen += new EventHandler(Control_DialogOpen);
			}

			void Control_DialogOpen(object sender, EventArgs e)
			{
				using (OpenFileDialog dg = new OpenFileDialog())
				{
					dg.FileName = (string)Control.Value;

					if (dg.ShowDialog(EditCellContext.Grid) == DialogResult.OK)
					{
						Control.Value = dg.FileName;
					}
				}
			}
		}
	}
}
