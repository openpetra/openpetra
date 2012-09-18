using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevAge.TestApp
{
	/// <summary>
	/// Summary description for frmDemo.
	/// </summary>
	[Sample("Other controls", 30, "Windows Forms Controls")]
	public class frmSample30 : DevAge.Windows.Forms.FormBase
	{
		private DevAge.Windows.Forms.HeaderGroupBox headerGroupBox1;
		private DevAge.Windows.Forms.ButtonMultiSelection buttonMultiSelectionClose;
		private DevAge.Windows.Forms.HeaderGroupBox headerGroupBox2;
		private DevAge.Windows.Forms.ButtonMultiSelection buttonMultiSelectionSample;
		private DevAge.Windows.Forms.DevAgeNumericUpDown numericUpDownExSample;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private DevAge.Windows.Forms.DevAgeMaskedTextBox textBoxTypedInt;
        private DevAge.Windows.Forms.DevAgeMaskedTextBox textBoxTypedDouble;
        private DevAge.Windows.Forms.DevAgeMaskedTextBox textBoxTypedDecimal;
        private DevAge.Windows.Forms.DevAgeMaskedTextBox textBoxTypedCurrency;
        private DevAge.Windows.Forms.DevAgeMaskedTextBox textBoxTypedPercent;
		private System.Windows.Forms.ErrorProvider errorProviderSample;
		private System.Windows.Forms.Button btShowError;
		private System.Windows.Forms.Label label8;
		private DevAge.Windows.Forms.HeaderGroupBox headerGroupBox3;
		private System.Windows.Forms.TextBox txtMailToTo;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtMailToSubject;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox txtMailToBody;
		private System.Windows.Forms.Label label12;
        private DevAge.Windows.Forms.DevAgeMaskedTextBox textBoxTypedDoubleAllowNull;
		private DevAge.Windows.Forms.DevAgeTextBox textBoxTypedDateTime;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.ImageList imageListMenu;
		private DevAge.Windows.Forms.HeaderGroupBox headerGroupBox4;
		private System.Windows.Forms.Panel panelGradientSample;
        private DevAge.Windows.Forms.DevAgeMaskedTextBox txtLightWidth;
		private System.Windows.Forms.Label label14;
        private DevAge.Windows.Forms.DevAgeMaskedTextBox txtDarkWidth;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.RadioButton rdRaised;
		private System.Windows.Forms.RadioButton rdSunken;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private DevAge.Windows.Forms.ColorPicker colorPickerBackColor;
		private DevAge.Windows.Forms.ColorPicker colorPickerDark;
		private DevAge.Windows.Forms.ColorPicker colorPickerLight;
		private DevAge.Windows.Forms.HeaderGroupBox headerGroupBox5;
		private System.Windows.Forms.Label label19;
        private DevAge.Windows.Forms.DevAgeComboBox cmbTypedEnumBorderStyle;
		private System.Windows.Forms.Label label20;
        private DevAge.Windows.Forms.DevAgeComboBox cmbTypedControls;
		private System.Windows.Forms.Label label21;
        private DevAge.Windows.Forms.TextBoxUITypeEditor txtBoxUITypeEditorAnchorStyle;
		private DevAge.Windows.Forms.Line line1;
        private DevAge.Windows.Forms.Line line2;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.TabPage tabPage7;
		private DevAge.Windows.Forms.ImageNavigator imageNavigator1;
		private System.Windows.Forms.Label label23;
		private DevAge.Windows.Forms.EditableControlBase editableControlBase1;
		private DevAge.Windows.Forms.LinkLabel linkLabel1;
		private DevAge.Windows.Forms.Line line3;
		private DevAge.Windows.Forms.Line line4;
		private System.Windows.Forms.Label lblFlatStyle;
        private DevAge.Windows.Forms.DevAgeComboBox cmbFlatStyle;
		private System.Windows.Forms.Label label25;
		private DevAge.Windows.Forms.DevAgeTextBox textBoxTypedStringAbc;
		private System.Windows.Forms.TabPage tabPage8;
		private DevAge.Windows.Forms.LinkLabel linkLabel4;
		private DevAge.Windows.Forms.LinkLabel linkLabel3;
		private System.Windows.Forms.Label label24;
		private DevAge.Windows.Forms.LinkLabel linkLabel2;
		private DevAge.Windows.Forms.LinkLabel linkLabel5;
		private DevAge.Windows.Forms.LinkLabel linkLabel6;
        private DevAge.Windows.Forms.LinkLabel linkLabel7;
        private Label label26;
        private DevAge.Windows.Forms.DevAgeComboBox cmbCustomDisplay;
        private Label lblCmbValue;
		private System.ComponentModel.IContainer components;

		public frmSample30()
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSample30));
            this.headerGroupBox1 = new DevAge.Windows.Forms.HeaderGroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btShowError = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownExSample = new DevAge.Windows.Forms.DevAgeNumericUpDown();
            this.buttonMultiSelectionSample = new DevAge.Windows.Forms.ButtonMultiSelection();
            this.buttonMultiSelectionClose = new DevAge.Windows.Forms.ButtonMultiSelection();
            this.headerGroupBox2 = new DevAge.Windows.Forms.HeaderGroupBox();
            this.textBoxTypedStringAbc = new DevAge.Windows.Forms.DevAgeTextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.textBoxTypedDateTime = new DevAge.Windows.Forms.DevAgeTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxTypedDoubleAllowNull = new DevAge.Windows.Forms.DevAgeMaskedTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxTypedPercent = new DevAge.Windows.Forms.DevAgeMaskedTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxTypedCurrency = new DevAge.Windows.Forms.DevAgeMaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxTypedDecimal = new DevAge.Windows.Forms.DevAgeMaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxTypedDouble = new DevAge.Windows.Forms.DevAgeMaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxTypedInt = new DevAge.Windows.Forms.DevAgeMaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProviderSample = new System.Windows.Forms.ErrorProvider(this.components);
            this.headerGroupBox3 = new DevAge.Windows.Forms.HeaderGroupBox();
            this.linkLabel1 = new DevAge.Windows.Forms.LinkLabel();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMailToBody = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMailToSubject = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMailToTo = new System.Windows.Forms.TextBox();
            this.imageListMenu = new System.Windows.Forms.ImageList(this.components);
            this.headerGroupBox4 = new DevAge.Windows.Forms.HeaderGroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.colorPickerLight = new DevAge.Windows.Forms.ColorPicker();
            this.label17 = new System.Windows.Forms.Label();
            this.colorPickerDark = new DevAge.Windows.Forms.ColorPicker();
            this.label16 = new System.Windows.Forms.Label();
            this.colorPickerBackColor = new DevAge.Windows.Forms.ColorPicker();
            this.rdSunken = new System.Windows.Forms.RadioButton();
            this.rdRaised = new System.Windows.Forms.RadioButton();
            this.txtDarkWidth = new DevAge.Windows.Forms.DevAgeMaskedTextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtLightWidth = new DevAge.Windows.Forms.DevAgeMaskedTextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.panelGradientSample = new System.Windows.Forms.Panel();
            this.cmbTypedEnumBorderStyle = new DevAge.Windows.Forms.DevAgeComboBox();
            this.headerGroupBox5 = new DevAge.Windows.Forms.HeaderGroupBox();
            this.cmbFlatStyle = new DevAge.Windows.Forms.DevAgeComboBox();
            this.lblFlatStyle = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.txtBoxUITypeEditorAnchorStyle = new DevAge.Windows.Forms.TextBoxUITypeEditor();
            this.label20 = new System.Windows.Forms.Label();
            this.cmbTypedControls = new DevAge.Windows.Forms.DevAgeComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.line1 = new DevAge.Windows.Forms.Line();
            this.line2 = new DevAge.Windows.Forms.Line();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.linkLabel7 = new DevAge.Windows.Forms.LinkLabel();
            this.linkLabel6 = new DevAge.Windows.Forms.LinkLabel();
            this.linkLabel5 = new DevAge.Windows.Forms.LinkLabel();
            this.linkLabel4 = new DevAge.Windows.Forms.LinkLabel();
            this.linkLabel3 = new DevAge.Windows.Forms.LinkLabel();
            this.label24 = new System.Windows.Forms.Label();
            this.linkLabel2 = new DevAge.Windows.Forms.LinkLabel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.line4 = new DevAge.Windows.Forms.Line();
            this.line3 = new DevAge.Windows.Forms.Line();
            this.editableControlBase1 = new DevAge.Windows.Forms.EditableControlBase();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.imageNavigator1 = new DevAge.Windows.Forms.ImageNavigator();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.lblCmbValue = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.cmbCustomDisplay = new DevAge.Windows.Forms.DevAgeComboBox();
            this.headerGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExSample)).BeginInit();
            this.headerGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderSample)).BeginInit();
            this.headerGroupBox3.SuspendLayout();
            this.headerGroupBox4.SuspendLayout();
            this.headerGroupBox5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerGroupBox1
            // 
            this.headerGroupBox1.Controls.Add(this.label8);
            this.headerGroupBox1.Controls.Add(this.btShowError);
            this.headerGroupBox1.Controls.Add(this.label2);
            this.headerGroupBox1.Controls.Add(this.label1);
            this.headerGroupBox1.Controls.Add(this.numericUpDownExSample);
            this.headerGroupBox1.Controls.Add(this.buttonMultiSelectionSample);
            this.headerGroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerGroupBox1.Image = ((System.Drawing.Image)(resources.GetObject("headerGroupBox1.Image")));
            this.headerGroupBox1.Location = new System.Drawing.Point(28, 20);
            this.headerGroupBox1.Name = "headerGroupBox1";
            this.headerGroupBox1.Size = new System.Drawing.Size(397, 108);
            this.headerGroupBox1.TabIndex = 0;
            this.headerGroupBox1.TabStop = false;
            this.headerGroupBox1.Text = "HeaderGroupBox Sample";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(16, 80);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(168, 23);
            this.label8.TabIndex = 5;
            this.label8.Text = "ErrorDialog";
            // 
            // btShowError
            // 
            this.btShowError.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btShowError.Location = new System.Drawing.Point(196, 80);
            this.btShowError.Name = "btShowError";
            this.btShowError.Size = new System.Drawing.Size(74, 23);
            this.btShowError.TabIndex = 4;
            this.btShowError.Text = "ShowError";
            this.btShowError.Click += new System.EventHandler(this.btShowError_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "NumericUpDownEx Sample";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Button Multi Selection Sample";
            // 
            // numericUpDownExSample
            // 
            this.numericUpDownExSample.Location = new System.Drawing.Point(192, 52);
            this.numericUpDownExSample.Name = "numericUpDownExSample";
            this.numericUpDownExSample.Size = new System.Drawing.Size(140, 20);
            this.numericUpDownExSample.TabIndex = 1;
            // 
            // buttonMultiSelectionSample
            // 
            this.buttonMultiSelectionSample.BackColor = System.Drawing.Color.Transparent;
            this.buttonMultiSelectionSample.DialogResult = System.Windows.Forms.DialogResult.None;
            this.buttonMultiSelectionSample.Image = null;
            this.buttonMultiSelectionSample.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonMultiSelectionSample.Location = new System.Drawing.Point(192, 20);
            this.buttonMultiSelectionSample.Name = "buttonMultiSelectionSample";
            this.buttonMultiSelectionSample.Size = new System.Drawing.Size(136, 23);
            this.buttonMultiSelectionSample.TabIndex = 0;
            this.buttonMultiSelectionSample.Text = "Hello";
            this.buttonMultiSelectionSample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonMultiSelectionClose
            // 
            this.buttonMultiSelectionClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMultiSelectionClose.BackColor = System.Drawing.Color.Transparent;
            this.buttonMultiSelectionClose.CausesValidation = false;
            this.buttonMultiSelectionClose.DialogResult = System.Windows.Forms.DialogResult.None;
            this.buttonMultiSelectionClose.Image = null;
            this.buttonMultiSelectionClose.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonMultiSelectionClose.Location = new System.Drawing.Point(396, 372);
            this.buttonMultiSelectionClose.Name = "buttonMultiSelectionClose";
            this.buttonMultiSelectionClose.Size = new System.Drawing.Size(75, 23);
            this.buttonMultiSelectionClose.TabIndex = 1;
            this.buttonMultiSelectionClose.Text = "Close";
            this.buttonMultiSelectionClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // headerGroupBox2
            // 
            this.headerGroupBox2.Controls.Add(this.textBoxTypedStringAbc);
            this.headerGroupBox2.Controls.Add(this.label25);
            this.headerGroupBox2.Controls.Add(this.textBoxTypedDateTime);
            this.headerGroupBox2.Controls.Add(this.label13);
            this.headerGroupBox2.Controls.Add(this.textBoxTypedDoubleAllowNull);
            this.headerGroupBox2.Controls.Add(this.label12);
            this.headerGroupBox2.Controls.Add(this.textBoxTypedPercent);
            this.headerGroupBox2.Controls.Add(this.label7);
            this.headerGroupBox2.Controls.Add(this.textBoxTypedCurrency);
            this.headerGroupBox2.Controls.Add(this.label6);
            this.headerGroupBox2.Controls.Add(this.textBoxTypedDecimal);
            this.headerGroupBox2.Controls.Add(this.label5);
            this.headerGroupBox2.Controls.Add(this.textBoxTypedDouble);
            this.headerGroupBox2.Controls.Add(this.label4);
            this.headerGroupBox2.Controls.Add(this.textBoxTypedInt);
            this.headerGroupBox2.Controls.Add(this.label3);
            this.headerGroupBox2.Image = ((System.Drawing.Image)(resources.GetObject("headerGroupBox2.Image")));
            this.headerGroupBox2.Location = new System.Drawing.Point(56, 32);
            this.headerGroupBox2.Name = "headerGroupBox2";
            this.headerGroupBox2.Size = new System.Drawing.Size(340, 244);
            this.headerGroupBox2.TabIndex = 2;
            this.headerGroupBox2.TabStop = false;
            this.headerGroupBox2.Text = "TextBoxTyped and TypeConverter Samples";
            // 
            // textBoxTypedStringAbc
            // 
            this.textBoxTypedStringAbc.Location = new System.Drawing.Point(156, 160);
            this.textBoxTypedStringAbc.Name = "textBoxTypedStringAbc";
            this.textBoxTypedStringAbc.Size = new System.Drawing.Size(156, 20);
            this.textBoxTypedStringAbc.TabIndex = 15;
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(16, 160);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(132, 20);
            this.label25.TabIndex = 14;
            this.label25.Text = "String only \"abc\"";
            // 
            // textBoxTypedDateTime
            // 
            this.textBoxTypedDateTime.Location = new System.Drawing.Point(156, 184);
            this.textBoxTypedDateTime.Name = "textBoxTypedDateTime";
            this.textBoxTypedDateTime.Size = new System.Drawing.Size(156, 20);
            this.textBoxTypedDateTime.TabIndex = 13;
            this.textBoxTypedDateTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(16, 184);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(132, 20);
            this.label13.TabIndex = 12;
            this.label13.Text = "DateTime (yyyy MM dd)";
            // 
            // textBoxTypedDoubleAllowNull
            // 
            this.textBoxTypedDoubleAllowNull.Location = new System.Drawing.Point(156, 136);
            this.textBoxTypedDoubleAllowNull.Name = "textBoxTypedDoubleAllowNull";
            this.textBoxTypedDoubleAllowNull.Size = new System.Drawing.Size(156, 20);
            this.textBoxTypedDoubleAllowNull.TabIndex = 11;
            this.textBoxTypedDoubleAllowNull.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(16, 136);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(132, 20);
            this.label12.TabIndex = 10;
            this.label12.Text = "Double (AllowNull)";
            // 
            // textBoxTypedPercent
            // 
            this.textBoxTypedPercent.Location = new System.Drawing.Point(156, 112);
            this.textBoxTypedPercent.Name = "textBoxTypedPercent";
            this.textBoxTypedPercent.Size = new System.Drawing.Size(156, 20);
            this.textBoxTypedPercent.TabIndex = 9;
            this.textBoxTypedPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 20);
            this.label7.TabIndex = 8;
            this.label7.Text = "Double Percent";
            // 
            // textBoxTypedCurrency
            // 
            this.textBoxTypedCurrency.Location = new System.Drawing.Point(156, 90);
            this.textBoxTypedCurrency.Name = "textBoxTypedCurrency";
            this.textBoxTypedCurrency.Size = new System.Drawing.Size(156, 20);
            this.textBoxTypedCurrency.TabIndex = 7;
            this.textBoxTypedCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 18);
            this.label6.TabIndex = 6;
            this.label6.Text = "Decimal Currency";
            // 
            // textBoxTypedDecimal
            // 
            this.textBoxTypedDecimal.Location = new System.Drawing.Point(156, 68);
            this.textBoxTypedDecimal.Name = "textBoxTypedDecimal";
            this.textBoxTypedDecimal.Size = new System.Drawing.Size(156, 20);
            this.textBoxTypedDecimal.TabIndex = 5;
            this.textBoxTypedDecimal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Decimal";
            // 
            // textBoxTypedDouble
            // 
            this.textBoxTypedDouble.Location = new System.Drawing.Point(156, 44);
            this.textBoxTypedDouble.Name = "textBoxTypedDouble";
            this.textBoxTypedDouble.Size = new System.Drawing.Size(156, 20);
            this.textBoxTypedDouble.TabIndex = 3;
            this.textBoxTypedDouble.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Double";
            // 
            // textBoxTypedInt
            // 
            this.textBoxTypedInt.Location = new System.Drawing.Point(156, 20);
            this.textBoxTypedInt.Name = "textBoxTypedInt";
            this.textBoxTypedInt.Size = new System.Drawing.Size(156, 20);
            this.textBoxTypedInt.TabIndex = 1;
            this.textBoxTypedInt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Int";
            // 
            // errorProviderSample
            // 
            this.errorProviderSample.ContainerControl = this;
            // 
            // headerGroupBox3
            // 
            this.headerGroupBox3.Controls.Add(this.linkLabel1);
            this.headerGroupBox3.Controls.Add(this.label11);
            this.headerGroupBox3.Controls.Add(this.txtMailToBody);
            this.headerGroupBox3.Controls.Add(this.label10);
            this.headerGroupBox3.Controls.Add(this.txtMailToSubject);
            this.headerGroupBox3.Controls.Add(this.label9);
            this.headerGroupBox3.Controls.Add(this.txtMailToTo);
            this.headerGroupBox3.Image = ((System.Drawing.Image)(resources.GetObject("headerGroupBox3.Image")));
            this.headerGroupBox3.Location = new System.Drawing.Point(28, 132);
            this.headerGroupBox3.Name = "headerGroupBox3";
            this.headerGroupBox3.Size = new System.Drawing.Size(340, 92);
            this.headerGroupBox3.TabIndex = 3;
            this.headerGroupBox3.TabStop = false;
            this.headerGroupBox3.Text = "MailTo Protocol Sample";
            // 
            // linkLabel1
            // 
            this.linkLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.linkLabel1.BorderColor = System.Drawing.Color.Black;
            this.linkLabel1.BorderRound = 0;
            this.linkLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel1.EnableMouseEffect = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.ForeColor = System.Drawing.Color.Blue;
            this.linkLabel1.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel1.Image")));
            this.linkLabel1.Location = new System.Drawing.Point(244, 68);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(92, 20);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.Text = "Open MailTo";
            this.linkLabel1.TextAlignment = DevAge.Drawing.ContentAlignment.BottomCenter;
            this.linkLabel1.Click += new System.EventHandler(this.btMailTo_Click);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(16, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 20);
            this.label11.TabIndex = 5;
            this.label11.Text = "Body";
            // 
            // txtMailToBody
            // 
            this.txtMailToBody.Location = new System.Drawing.Point(88, 68);
            this.txtMailToBody.Name = "txtMailToBody";
            this.txtMailToBody.Size = new System.Drawing.Size(152, 20);
            this.txtMailToBody.TabIndex = 4;
            this.txtMailToBody.Text = "Hello World !";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(16, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 20);
            this.label10.TabIndex = 3;
            this.label10.Text = "Subject";
            // 
            // txtMailToSubject
            // 
            this.txtMailToSubject.Location = new System.Drawing.Point(88, 44);
            this.txtMailToSubject.Name = "txtMailToSubject";
            this.txtMailToSubject.Size = new System.Drawing.Size(152, 20);
            this.txtMailToSubject.TabIndex = 2;
            this.txtMailToSubject.Text = "Hello";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(16, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 20);
            this.label9.TabIndex = 1;
            this.label9.Text = "To";
            // 
            // txtMailToTo
            // 
            this.txtMailToTo.Location = new System.Drawing.Point(88, 20);
            this.txtMailToTo.Name = "txtMailToTo";
            this.txtMailToTo.Size = new System.Drawing.Size(152, 20);
            this.txtMailToTo.TabIndex = 0;
            this.txtMailToTo.Text = "support@mymail.it";
            // 
            // imageListMenu
            // 
            this.imageListMenu.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMenu.ImageStream")));
            this.imageListMenu.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMenu.Images.SetKeyName(0, "");
            this.imageListMenu.Images.SetKeyName(1, "");
            this.imageListMenu.Images.SetKeyName(2, "");
            this.imageListMenu.Images.SetKeyName(3, "");
            // 
            // headerGroupBox4
            // 
            this.headerGroupBox4.Controls.Add(this.label18);
            this.headerGroupBox4.Controls.Add(this.colorPickerLight);
            this.headerGroupBox4.Controls.Add(this.label17);
            this.headerGroupBox4.Controls.Add(this.colorPickerDark);
            this.headerGroupBox4.Controls.Add(this.label16);
            this.headerGroupBox4.Controls.Add(this.colorPickerBackColor);
            this.headerGroupBox4.Controls.Add(this.rdSunken);
            this.headerGroupBox4.Controls.Add(this.rdRaised);
            this.headerGroupBox4.Controls.Add(this.txtDarkWidth);
            this.headerGroupBox4.Controls.Add(this.label15);
            this.headerGroupBox4.Controls.Add(this.txtLightWidth);
            this.headerGroupBox4.Controls.Add(this.label14);
            this.headerGroupBox4.Controls.Add(this.panelGradientSample);
            this.headerGroupBox4.Image = null;
            this.headerGroupBox4.Location = new System.Drawing.Point(76, 32);
            this.headerGroupBox4.Name = "headerGroupBox4";
            this.headerGroupBox4.Size = new System.Drawing.Size(328, 152);
            this.headerGroupBox4.TabIndex = 4;
            this.headerGroupBox4.TabStop = false;
            this.headerGroupBox4.Text = "Gradient 3D Border";
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(8, 120);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(128, 20);
            this.label18.TabIndex = 13;
            this.label18.Text = "LightColor";
            // 
            // colorPickerLight
            // 
            this.colorPickerLight.Location = new System.Drawing.Point(136, 120);
            this.colorPickerLight.Name = "colorPickerLight";
            this.colorPickerLight.SelectedColor = System.Drawing.Color.Azure;
            this.colorPickerLight.Size = new System.Drawing.Size(168, 20);
            this.colorPickerLight.TabIndex = 12;
            this.colorPickerLight.SelectedColorChanged += new System.EventHandler(this.colorPickerLight_SelectedColorChanged);
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(8, 96);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(128, 20);
            this.label17.TabIndex = 11;
            this.label17.Text = "DarkColor";
            // 
            // colorPickerDark
            // 
            this.colorPickerDark.Location = new System.Drawing.Point(136, 96);
            this.colorPickerDark.Name = "colorPickerDark";
            this.colorPickerDark.SelectedColor = System.Drawing.Color.CornflowerBlue;
            this.colorPickerDark.Size = new System.Drawing.Size(168, 20);
            this.colorPickerDark.TabIndex = 10;
            this.colorPickerDark.SelectedColorChanged += new System.EventHandler(this.colorPickerDark_SelectedColorChanged);
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(8, 72);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(128, 20);
            this.label16.TabIndex = 9;
            this.label16.Text = "BackColor";
            // 
            // colorPickerBackColor
            // 
            this.colorPickerBackColor.Location = new System.Drawing.Point(136, 72);
            this.colorPickerBackColor.Name = "colorPickerBackColor";
            this.colorPickerBackColor.SelectedColor = System.Drawing.Color.LightSteelBlue;
            this.colorPickerBackColor.Size = new System.Drawing.Size(168, 20);
            this.colorPickerBackColor.TabIndex = 8;
            this.colorPickerBackColor.SelectedColorChanged += new System.EventHandler(this.colorPickerBackColor_SelectedColorChanged);
            // 
            // rdSunken
            // 
            this.rdSunken.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdSunken.Location = new System.Drawing.Point(128, 44);
            this.rdSunken.Name = "rdSunken";
            this.rdSunken.Size = new System.Drawing.Size(88, 24);
            this.rdSunken.TabIndex = 7;
            this.rdSunken.Text = "Sunken";
            this.rdSunken.CheckedChanged += new System.EventHandler(this.rdSunken_CheckedChanged);
            // 
            // rdRaised
            // 
            this.rdRaised.Checked = true;
            this.rdRaised.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdRaised.Location = new System.Drawing.Point(128, 20);
            this.rdRaised.Name = "rdRaised";
            this.rdRaised.Size = new System.Drawing.Size(88, 24);
            this.rdRaised.TabIndex = 6;
            this.rdRaised.TabStop = true;
            this.rdRaised.Text = "Raised";
            this.rdRaised.CheckedChanged += new System.EventHandler(this.rdRaised_CheckedChanged);
            // 
            // txtDarkWidth
            // 
            this.txtDarkWidth.Location = new System.Drawing.Point(80, 44);
            this.txtDarkWidth.Name = "txtDarkWidth";
            this.txtDarkWidth.Size = new System.Drawing.Size(40, 20);
            this.txtDarkWidth.TabIndex = 5;
            this.txtDarkWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDarkWidth.Validated += new System.EventHandler(this.txtDarkWidth_Validated);
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(8, 44);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(68, 20);
            this.label15.TabIndex = 4;
            this.label15.Text = "Dark Width:";
            // 
            // txtLightWidth
            // 
            this.txtLightWidth.Location = new System.Drawing.Point(80, 20);
            this.txtLightWidth.Name = "txtLightWidth";
            this.txtLightWidth.Size = new System.Drawing.Size(40, 20);
            this.txtLightWidth.TabIndex = 3;
            this.txtLightWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLightWidth.Validated += new System.EventHandler(this.txtLightWidth_Validated);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(8, 20);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 20);
            this.label14.TabIndex = 2;
            this.label14.Text = "Light Width:";
            // 
            // panelGradientSample
            // 
            this.panelGradientSample.Location = new System.Drawing.Point(220, 28);
            this.panelGradientSample.Name = "panelGradientSample";
            this.panelGradientSample.Size = new System.Drawing.Size(100, 28);
            this.panelGradientSample.TabIndex = 0;
            this.panelGradientSample.Paint += new System.Windows.Forms.PaintEventHandler(this.panelGradientSample_Paint);
            // 
            // cmbTypedEnumBorderStyle
            // 
            this.cmbTypedEnumBorderStyle.Location = new System.Drawing.Point(152, 20);
            this.cmbTypedEnumBorderStyle.Name = "cmbTypedEnumBorderStyle";
            this.cmbTypedEnumBorderStyle.Size = new System.Drawing.Size(156, 21);
            this.cmbTypedEnumBorderStyle.TabIndex = 5;
            // 
            // headerGroupBox5
            // 
            this.headerGroupBox5.Controls.Add(this.cmbFlatStyle);
            this.headerGroupBox5.Controls.Add(this.lblFlatStyle);
            this.headerGroupBox5.Controls.Add(this.label21);
            this.headerGroupBox5.Controls.Add(this.txtBoxUITypeEditorAnchorStyle);
            this.headerGroupBox5.Controls.Add(this.label20);
            this.headerGroupBox5.Controls.Add(this.cmbTypedControls);
            this.headerGroupBox5.Controls.Add(this.label19);
            this.headerGroupBox5.Controls.Add(this.cmbTypedEnumBorderStyle);
            this.headerGroupBox5.Image = null;
            this.headerGroupBox5.Location = new System.Drawing.Point(8, 20);
            this.headerGroupBox5.Name = "headerGroupBox5";
            this.headerGroupBox5.Size = new System.Drawing.Size(428, 140);
            this.headerGroupBox5.TabIndex = 6;
            this.headerGroupBox5.TabStop = false;
            this.headerGroupBox5.Text = "Enums";
            // 
            // cmbFlatStyle
            // 
            this.cmbFlatStyle.Location = new System.Drawing.Point(152, 48);
            this.cmbFlatStyle.Name = "cmbFlatStyle";
            this.cmbFlatStyle.Size = new System.Drawing.Size(156, 21);
            this.cmbFlatStyle.TabIndex = 12;
            // 
            // lblFlatStyle
            // 
            this.lblFlatStyle.Location = new System.Drawing.Point(8, 51);
            this.lblFlatStyle.Name = "lblFlatStyle";
            this.lblFlatStyle.Size = new System.Drawing.Size(140, 16);
            this.lblFlatStyle.TabIndex = 11;
            this.lblFlatStyle.Text = "FlatStyle";
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(8, 108);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(140, 16);
            this.label21.TabIndex = 10;
            this.label21.Text = "UITypeEditor AnchorStyle";
            // 
            // txtBoxUITypeEditorAnchorStyle
            // 
            this.txtBoxUITypeEditorAnchorStyle.BackColor = System.Drawing.Color.Transparent;
            this.txtBoxUITypeEditorAnchorStyle.Location = new System.Drawing.Point(152, 104);
            this.txtBoxUITypeEditorAnchorStyle.Name = "txtBoxUITypeEditorAnchorStyle";
            this.txtBoxUITypeEditorAnchorStyle.Size = new System.Drawing.Size(156, 20);
            this.txtBoxUITypeEditorAnchorStyle.TabIndex = 9;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(8, 79);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(140, 16);
            this.label20.TabIndex = 8;
            this.label20.Text = "ComboBox Controls";
            // 
            // cmbTypedControls
            // 
            this.cmbTypedControls.Location = new System.Drawing.Point(152, 76);
            this.cmbTypedControls.Name = "cmbTypedControls";
            this.cmbTypedControls.Size = new System.Drawing.Size(156, 21);
            this.cmbTypedControls.TabIndex = 7;
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(8, 23);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(140, 16);
            this.label19.TabIndex = 6;
            this.label19.Text = "Enum BorderStyle";
            // 
            // line1
            // 
            this.line1.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.line1.FirstColor = System.Drawing.SystemColors.ControlDark;
            this.line1.LineStyle = DevAge.Windows.Forms.LineStyle.Horizontal;
            this.line1.Location = new System.Drawing.Point(120, 268);
            this.line1.Name = "line1";
            this.line1.SecondColor = System.Drawing.SystemColors.ControlLightLight;
            this.line1.Size = new System.Drawing.Size(48, 2);
            this.line1.TabIndex = 13;
            this.line1.TabStop = false;
            // 
            // line2
            // 
            this.line2.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.line2.FirstColor = System.Drawing.SystemColors.ControlDark;
            this.line2.LineStyle = DevAge.Windows.Forms.LineStyle.Vertical;
            this.line2.Location = new System.Drawing.Point(180, 272);
            this.line2.Name = "line2";
            this.line2.SecondColor = System.Drawing.SystemColors.ControlLightLight;
            this.line2.Size = new System.Drawing.Size(2, 20);
            this.line2.TabIndex = 14;
            this.line2.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(456, 348);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.headerGroupBox2);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(448, 322);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "TextBoxTyped";
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.linkLabel7);
            this.tabPage8.Controls.Add(this.linkLabel6);
            this.tabPage8.Controls.Add(this.linkLabel5);
            this.tabPage8.Controls.Add(this.linkLabel4);
            this.tabPage8.Controls.Add(this.linkLabel3);
            this.tabPage8.Controls.Add(this.label24);
            this.tabPage8.Controls.Add(this.linkLabel2);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(448, 322);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "LinkLabel";
            // 
            // linkLabel7
            // 
            this.linkLabel7.BackColor = System.Drawing.Color.RoyalBlue;
            this.linkLabel7.BorderColor = System.Drawing.Color.MidnightBlue;
            this.linkLabel7.BorderRound = 0.2;
            this.linkLabel7.BorderWidth = 1;
            this.linkLabel7.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkLabel7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel7.ForeColor = System.Drawing.Color.White;
            this.linkLabel7.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel7.Image")));
            this.linkLabel7.ImageAlignment = DevAge.Drawing.ContentAlignment.TopRight;
            this.linkLabel7.Location = new System.Drawing.Point(228, 132);
            this.linkLabel7.Name = "linkLabel7";
            this.linkLabel7.Size = new System.Drawing.Size(72, 40);
            this.linkLabel7.TabIndex = 30;
            this.linkLabel7.Text = "Test";
            this.linkLabel7.TextAlignment = DevAge.Drawing.ContentAlignment.BottomCenter;
            // 
            // linkLabel6
            // 
            this.linkLabel6.BackColor = System.Drawing.Color.Khaki;
            this.linkLabel6.BorderColor = System.Drawing.Color.Goldenrod;
            this.linkLabel6.BorderRound = 0.3;
            this.linkLabel6.BorderWidth = 3;
            this.linkLabel6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel6.EnableMouseEffect = true;
            this.linkLabel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel6.ForeColor = System.Drawing.Color.Blue;
            this.linkLabel6.ImageAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
            this.linkLabel6.Location = new System.Drawing.Point(216, 80);
            this.linkLabel6.Name = "linkLabel6";
            this.linkLabel6.Size = new System.Drawing.Size(96, 36);
            this.linkLabel6.TabIndex = 29;
            this.linkLabel6.Text = "Sample Link";
            this.linkLabel6.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel5
            // 
            this.linkLabel5.BackColor = System.Drawing.SystemColors.Control;
            this.linkLabel5.BorderColor = System.Drawing.Color.Black;
            this.linkLabel5.BorderRound = 0.3;
            this.linkLabel5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel5.EnableMouseEffect = true;
            this.linkLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel5.ForeColor = System.Drawing.Color.Blue;
            this.linkLabel5.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel5.Image")));
            this.linkLabel5.ImageAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
            this.linkLabel5.Location = new System.Drawing.Point(104, 92);
            this.linkLabel5.Name = "linkLabel5";
            this.linkLabel5.Size = new System.Drawing.Size(72, 40);
            this.linkLabel5.TabIndex = 28;
            this.linkLabel5.Text = "Sample Link";
            this.linkLabel5.TextAlignment = DevAge.Drawing.ContentAlignment.BottomCenter;
            // 
            // linkLabel4
            // 
            this.linkLabel4.BackColor = System.Drawing.Color.Khaki;
            this.linkLabel4.BorderColor = System.Drawing.Color.Goldenrod;
            this.linkLabel4.BorderRound = 0.3;
            this.linkLabel4.BorderWidth = 1;
            this.linkLabel4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel4.EnableMouseEffect = true;
            this.linkLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel4.ForeColor = System.Drawing.Color.Blue;
            this.linkLabel4.ImageAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
            this.linkLabel4.Location = new System.Drawing.Point(264, 32);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(88, 28);
            this.linkLabel4.TabIndex = 27;
            this.linkLabel4.Text = "Sample Link";
            this.linkLabel4.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel3
            // 
            this.linkLabel3.BackColor = System.Drawing.SystemColors.Control;
            this.linkLabel3.BorderColor = System.Drawing.Color.Black;
            this.linkLabel3.BorderRound = 0;
            this.linkLabel3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel3.Enabled = false;
            this.linkLabel3.EnableMouseEffect = true;
            this.linkLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel3.ForeColor = System.Drawing.Color.Blue;
            this.linkLabel3.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel3.Image")));
            this.linkLabel3.ImageAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
            this.linkLabel3.Location = new System.Drawing.Point(176, 20);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(72, 40);
            this.linkLabel3.TabIndex = 26;
            this.linkLabel3.Text = "Sample Link";
            this.linkLabel3.TextAlignment = DevAge.Drawing.ContentAlignment.BottomCenter;
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(24, 28);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(72, 23);
            this.label24.TabIndex = 25;
            this.label24.Text = "Link Label";
            // 
            // linkLabel2
            // 
            this.linkLabel2.BackColor = System.Drawing.SystemColors.Control;
            this.linkLabel2.BorderColor = System.Drawing.Color.Black;
            this.linkLabel2.BorderRound = 0;
            this.linkLabel2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel2.EnableMouseEffect = true;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.ForeColor = System.Drawing.Color.Blue;
            this.linkLabel2.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel2.Image")));
            this.linkLabel2.ImageAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
            this.linkLabel2.Location = new System.Drawing.Point(100, 20);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(72, 40);
            this.linkLabel2.TabIndex = 24;
            this.linkLabel2.Text = "Sample Link";
            this.linkLabel2.TextAlignment = DevAge.Drawing.ContentAlignment.BottomCenter;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.line4);
            this.tabPage1.Controls.Add(this.line3);
            this.tabPage1.Controls.Add(this.editableControlBase1);
            this.tabPage1.Controls.Add(this.label23);
            this.tabPage1.Controls.Add(this.label22);
            this.tabPage1.Controls.Add(this.headerGroupBox1);
            this.tabPage1.Controls.Add(this.headerGroupBox3);
            this.tabPage1.Controls.Add(this.line1);
            this.tabPage1.Controls.Add(this.line2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(448, 322);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Others";
            // 
            // line4
            // 
            this.line4.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            this.line4.FirstColor = System.Drawing.SystemColors.ControlDark;
            this.line4.LineStyle = DevAge.Windows.Forms.LineStyle.Vertical;
            this.line4.Location = new System.Drawing.Point(192, 256);
            this.line4.Name = "line4";
            this.line4.SecondColor = System.Drawing.SystemColors.ControlLightLight;
            this.line4.Size = new System.Drawing.Size(2, 36);
            this.line4.TabIndex = 19;
            this.line4.TabStop = false;
            // 
            // line3
            // 
            this.line3.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.line3.FirstColor = System.Drawing.SystemColors.ControlDark;
            this.line3.LineStyle = DevAge.Windows.Forms.LineStyle.Horizontal;
            this.line3.Location = new System.Drawing.Point(116, 276);
            this.line3.Name = "line3";
            this.line3.SecondColor = System.Drawing.SystemColors.ControlLightLight;
            this.line3.Size = new System.Drawing.Size(48, 2);
            this.line3.TabIndex = 18;
            this.line3.TabStop = false;
            // 
            // editableControlBase1
            // 
            this.editableControlBase1.Location = new System.Drawing.Point(108, 232);
            this.editableControlBase1.Name = "editableControlBase1";
            this.editableControlBase1.Size = new System.Drawing.Size(60, 20);
            this.editableControlBase1.TabIndex = 17;
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(28, 232);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(80, 23);
            this.label23.TabIndex = 16;
            this.label23.Text = "EditablePanel";
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(32, 268);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(76, 23);
            this.label22.TabIndex = 15;
            this.label22.Text = "Line Control";
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.imageNavigator1);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(448, 322);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "Image Navigator (Slide)";
            // 
            // imageNavigator1
            // 
            this.imageNavigator1.CurrentImageIndex = -1;
            this.imageNavigator1.ImageAreaBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageNavigator1.ImageAreaSize = new System.Drawing.Size(156, 56);
            this.imageNavigator1.ImageAreaSizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            this.imageNavigator1.Location = new System.Drawing.Point(164, 52);
            this.imageNavigator1.Name = "imageNavigator1";
            this.imageNavigator1.Size = new System.Drawing.Size(156, 88);
            this.imageNavigator1.StatusFormat = "{0} of {1}";
            this.imageNavigator1.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.headerGroupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(448, 322);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Gradient 3D Border";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.lblCmbValue);
            this.tabPage6.Controls.Add(this.label26);
            this.tabPage6.Controls.Add(this.cmbCustomDisplay);
            this.tabPage6.Controls.Add(this.headerGroupBox5);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(448, 322);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "ComboBox and enums";
            // 
            // lblCmbValue
            // 
            this.lblCmbValue.AutoSize = true;
            this.lblCmbValue.Location = new System.Drawing.Point(322, 156);
            this.lblCmbValue.Name = "lblCmbValue";
            this.lblCmbValue.Size = new System.Drawing.Size(22, 13);
            this.lblCmbValue.TabIndex = 14;
            this.lblCmbValue.Text = "Val";
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(16, 153);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(140, 16);
            this.label26.TabIndex = 13;
            this.label26.Text = "ComboBox custom display";
            // 
            // cmbCustomDisplay
            // 
            this.cmbCustomDisplay.FormattingEnabled = true;
            this.cmbCustomDisplay.Location = new System.Drawing.Point(160, 150);
            this.cmbCustomDisplay.Name = "cmbCustomDisplay";
            this.cmbCustomDisplay.Size = new System.Drawing.Size(156, 21);
            this.cmbCustomDisplay.TabIndex = 7;
            this.cmbCustomDisplay.SelectedValueChanged += new System.EventHandler(this.cmbCustomDisplay_SelectedValueChanged);
            // 
            // frmSample30
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(476, 402);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonMultiSelectionClose);
            this.Name = "frmSample30";
            this.RestoreFlags = ((DevAge.Windows.Forms.RestoreFlags)((DevAge.Windows.Forms.RestoreFlags.Size | DevAge.Windows.Forms.RestoreFlags.Location)));
            this.Text = "frmDemo";
            this.Load += new System.EventHandler(this.frmDemo_Load);
            this.headerGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExSample)).EndInit();
            this.headerGroupBox2.ResumeLayout(false);
            this.headerGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderSample)).EndInit();
            this.headerGroupBox3.ResumeLayout(false);
            this.headerGroupBox3.PerformLayout();
            this.headerGroupBox4.ResumeLayout(false);
            this.headerGroupBox4.PerformLayout();
            this.headerGroupBox5.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmDemo_Load(object sender, System.EventArgs e)
		{
			//Button MultiSelection
			buttonMultiSelectionSample.ButtonsItems.Add(new DevAge.Windows.Forms.SubButtonItem("Hello",new EventHandler(Event_Hello)));
			buttonMultiSelectionSample.ButtonsItems.Add(new DevAge.Windows.Forms.SubButtonItem("Ciao",new EventHandler(Event_Ciao)));
			buttonMultiSelectionSample.ButtonsItems.Add(new DevAge.Windows.Forms.SubButtonItem("Hi",new EventHandler(Event_Hi)));

			buttonMultiSelectionSample.ButtonsItems.Add(new DevAge.Windows.Forms.SubButtonItem("Sample 1",new EventHandler(Event_Hi), imageListMenu, 0));
			buttonMultiSelectionSample.ButtonsItems.Add(new DevAge.Windows.Forms.SubButtonItem("Sample 2",new EventHandler(Event_Hi), imageListMenu, 1));
			buttonMultiSelectionSample.ButtonsItems.Add(new DevAge.Windows.Forms.SubButtonItem("Sample 3",new EventHandler(Event_Hi), imageListMenu, 2));
			buttonMultiSelectionSample.ButtonsItems.Add(new DevAge.Windows.Forms.SubButtonItem("Sample 4",new EventHandler(Event_Hi), imageListMenu, 3));

			//Button MultiSelection Close
			buttonMultiSelectionClose.ButtonsItems.Add(new DevAge.Windows.Forms.SubButtonItem("Close",new EventHandler(Event_Close)));
			buttonMultiSelectionClose.ButtonsItems.Add(new DevAge.Windows.Forms.SubButtonItem("Bye",new EventHandler(Event_Close)));


			//TextBoxTyped
			textBoxTypedCurrency.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(decimal), new DevAge.ComponentModel.Converter.CurrencyTypeConverter(typeof(decimal)));
			textBoxTypedCurrency.Value = 8732.5M;

			textBoxTypedDecimal.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(decimal));
			textBoxTypedDecimal.Value = 4324.2M;

			textBoxTypedDouble.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(double));
			textBoxTypedDouble.Value = 87.3;

			DevAge.ComponentModel.Validator.ValidatorTypeConverter l_Converter = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(double));
			textBoxTypedDoubleAllowNull.Validator = l_Converter;
			textBoxTypedDoubleAllowNull.Value = 55.4;
			l_Converter.AllowNull = true;
			l_Converter.NullString = "";

			textBoxTypedInt.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(int));
			textBoxTypedInt.Value = 100;

			textBoxTypedPercent.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(double), new DevAge.ComponentModel.Converter.PercentTypeConverter(typeof(double)));
			textBoxTypedPercent.Value = 0.95;

			string l_dtFormat = "yyyy MM dd";
			textBoxTypedDateTime.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(DateTime), new DevAge.ComponentModel.Converter.DateTimeTypeConverter(l_dtFormat, new string[]{l_dtFormat} ));
			textBoxTypedDateTime.Value = DateTime.Now;


			DevAge.ComponentModel.Validator.ValidatorTypeConverter l_WidthEditor = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(int));
			l_WidthEditor.MinimumValue = 0;
			l_WidthEditor.MaximumValue = 20;
			txtLightWidth.Validator = l_WidthEditor;
			txtLightWidth.Value = 3;
			txtDarkWidth.Validator = l_WidthEditor;
			txtDarkWidth.Value = 5;

			//string test = "123A";
			//test = DevAge.Windows.Forms.TextBoxTyped.ValidateCharactersString(test, new char[]{'A'}, null);

			cmbTypedEnumBorderStyle.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(BorderStyle));
			cmbTypedEnumBorderStyle.Value = BorderStyle.None;

			cmbFlatStyle.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(FlatStyle));
			cmbFlatStyle.Value = cmbTypedEnumBorderStyle.FlatStyle;
			cmbFlatStyle.TextChanged += new EventHandler(cmbFlatStyle_ValueChanged);

			cmbTypedControls.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(Control));
			cmbTypedControls.Validator.AllowStringConversion = false;
			cmbTypedControls.Validator.StandardValues = this.Controls;
			cmbTypedControls.Validator.StandardValuesExclusive = false;
			cmbTypedControls.Value = null;

			txtBoxUITypeEditorAnchorStyle.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(AnchorStyles));
			txtBoxUITypeEditorAnchorStyle.Value = AnchorStyles.None;

            Image[] imgs = new Image[] { Properties.Resources.Sample1, Properties.Resources.Sample2, Properties.Resources.Sample3, Properties.Resources.Sample4};
			imageNavigator1.Images = imgs;


            //ComboBox custom display
            //Create a validator
            DevAge.ComponentModel.Validator.ValidatorTypeConverter customValidator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(int));

            //Assign a list of available values
            int[] intValues = new int[] { 0, 1, 2, 3 };
            customValidator.StandardValues = intValues;

            //Create a ValueMapping class used to convert a value to another specific value specified
            DevAge.ComponentModel.Validator.ValueMapping mapping = new DevAge.ComponentModel.Validator.ValueMapping();
            mapping.DisplayStringList = new string[] { "Zero", "One", "Two", "Three" };
            mapping.ValueList = intValues;
            mapping.SpecialType = typeof(string);
            mapping.SpecialList = mapping.DisplayStringList;
            mapping.ThrowErrorIfNotFound = false;

            //Bind the ValueMapping to the Validator
            mapping.BindValidator(customValidator);

            cmbCustomDisplay.Validator = customValidator;
            cmbCustomDisplay.Value = 0;
            cmbCustomDisplay.FormatValue = true;
		}

		private void Event_Close(object sender, EventArgs e)
		{
			Close();
		}

		private void Event_Hello(object sender, EventArgs e)
		{
			MessageBox.Show("Hello");
		}
		private void Event_Ciao(object sender, EventArgs e)
		{
			MessageBox.Show("Ciao");
		}
		private void Event_Hi(object sender, EventArgs e)
		{
			MessageBox.Show("Hi");
		}

		private void btShowError_Click(object sender, System.EventArgs e)
		{
			DevAge.Windows.Forms.ErrorDialog.Show(this,new ArgumentOutOfRangeException("param","20","Parameter not valid"),"Error");
		}

		private void btMailTo_Click(object sender, System.EventArgs e)
		{
			DevAge.Shell.MailToProtocol.Exec(txtMailToTo.Text.Split(',',';'),null,null,txtMailToSubject.Text,txtMailToBody.Text);
		}

		private void panelGradientSample_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			try
			{
				using (SolidBrush l_Brush = new SolidBrush(colorPickerBackColor.SelectedColor))
				{
					e.Graphics.FillRectangle(l_Brush, panelGradientSample.ClientRectangle);
				}

				if (rdRaised.Checked)
					DevAge.Drawing.Utilities.DrawGradient3DBorder(e.Graphics, panelGradientSample.ClientRectangle, colorPickerBackColor.SelectedColor, colorPickerDark.SelectedColor, colorPickerLight.SelectedColor, (int)txtDarkWidth.Value, (int)txtLightWidth.Value, DevAge.Drawing.Gradient3DBorderStyle.Raised);
				else
					DevAge.Drawing.Utilities.DrawGradient3DBorder(e.Graphics, panelGradientSample.ClientRectangle, colorPickerBackColor.SelectedColor, colorPickerDark.SelectedColor, colorPickerLight.SelectedColor, (int)txtDarkWidth.Value, (int)txtLightWidth.Value, DevAge.Drawing.Gradient3DBorderStyle.Sunken);
			}
			catch(Exception)
			{
			}
		}

		private void colorPickerBackColor_SelectedColorChanged(object sender, System.EventArgs e)
		{
			panelGradientSample.Invalidate();
		}

		private void colorPickerDark_SelectedColorChanged(object sender, System.EventArgs e)
		{
			panelGradientSample.Invalidate();
		}

		private void colorPickerLight_SelectedColorChanged(object sender, System.EventArgs e)
		{
			panelGradientSample.Invalidate();
		}

		private void rdRaised_CheckedChanged(object sender, System.EventArgs e)
		{
			panelGradientSample.Invalidate();
		}

		private void rdSunken_CheckedChanged(object sender, System.EventArgs e)
		{
			panelGradientSample.Invalidate();
		}

		private void txtLightWidth_Validated(object sender, System.EventArgs e)
		{
			panelGradientSample.Invalidate();
		}

		private void txtDarkWidth_Validated(object sender, System.EventArgs e)
		{
			panelGradientSample.Invalidate();
		}

		private void cmbFlatStyle_ValueChanged(object sender, EventArgs e)
		{
			cmbTypedEnumBorderStyle.FlatStyle = (FlatStyle)cmbFlatStyle.Value;
		}

        private void cmbCustomDisplay_SelectedValueChanged(object sender, EventArgs e)
        {
            lblCmbValue.Text = cmbCustomDisplay.Value.ToString();
        }
	}
}
