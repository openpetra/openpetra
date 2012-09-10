using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevAge.TestApp
{
	/// <summary>
	/// Summary description for frmDemo2.
	/// </summary>
	[Sample("Other controls", 34, "FixedLength Text Parser/Writer")]
	public class frmSample34 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btParse;
		private System.Windows.Forms.Button btWrite;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox2;
		private System.ComponentModel.IContainer components = null;

		public frmSample34()
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
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btParse = new System.Windows.Forms.Button();
			this.btWrite = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.LargeButtons = false;
			this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid1.Location = new System.Drawing.Point(4, 76);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(220, 172);
			this.propertyGrid1.TabIndex = 0;
			this.propertyGrid1.Text = "propertyGrid1";
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(4, 20);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(504, 20);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "AVAL12005032299.00      VAL248.95";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(436, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Fixed Length Text (see SampleClass code for informations about the format)";
			// 
			// btParse
			// 
			this.btParse.Location = new System.Drawing.Point(4, 44);
			this.btParse.Name = "btParse";
			this.btParse.TabIndex = 3;
			this.btParse.Text = "Parse";
			this.btParse.Click += new System.EventHandler(this.btParse_Click);
			// 
			// btWrite
			// 
			this.btWrite.Location = new System.Drawing.Point(4, 260);
			this.btWrite.Name = "btWrite";
			this.btWrite.TabIndex = 6;
			this.btWrite.Text = "Write";
			this.btWrite.Click += new System.EventHandler(this.btWrite_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 292);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Fixed Length Text";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(4, 308);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(504, 20);
			this.textBox2.TabIndex = 4;
			this.textBox2.Text = "";
			// 
			// frmSample34
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(520, 330);
			this.Controls.Add(this.btWrite);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.btParse);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.propertyGrid1);
			this.Name = "frmSample34";
			this.Text = "Fixed Length Text Parser/Writer";
			this.ResumeLayout(false);

		}
		#endregion


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			propertyGrid1.SelectedObject = mSampleClass;
		}

		private SampleClass mSampleClass = new SampleClass();
		private void btParse_Click(object sender, System.EventArgs e)
		{
			try
			{
				DevAge.Text.FixedLength.LineParser parser = new DevAge.Text.FixedLength.LineParser(typeof(SampleClass));
				parser.LoadLine(textBox1.Text);

				parser.FillLineClass(mSampleClass);
				propertyGrid1.SelectedObject = mSampleClass;
			}
			catch(Exception ex)
			{
                MessageBox.Show(this, ex.Message);
            }
		}

		private void btWrite_Click(object sender, System.EventArgs e)
		{
            try
            {
                DevAge.Text.FixedLength.LineWriter writer = new DevAge.Text.FixedLength.LineWriter(typeof(SampleClass));

                textBox2.Text = writer.CreateLineFromClass(mSampleClass);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
		}
	}

	public class SampleClass
	{
		private string mField0;
		private string mField1;
		private DateTime mField2;
		private double? mField3;
		private string mField4;
        private decimal mField5;

		[DevAge.Text.FixedLength.Field(0, 1)]
		[DevAge.Text.FixedLength.StandardValue("A")]
		[DevAge.Text.FixedLength.StandardValue("B")]
		[DevAge.Text.FixedLength.StandardValue("C")]
		public string F1_StringOnlyABC
		{
			get{return mField0;}
			set{mField0 = value;}
		}
		[DevAge.Text.FixedLength.Field(1, 4)]
        public string F2_String
		{
			get{return mField1;}
			set{mField1 = value;}
		}
		[DevAge.Text.FixedLength.Field(2, 8)]
		[DevAge.Text.FixedLength.ParseFormat(DateTimeFormat="yyyyMMdd")]
		[DevAge.Text.FixedLength.ValueMapping("00000000", "00010101")]
        public DateTime F3_DateTime
		{
			get{return mField2;}
			set{mField2 = value;}
		}
		[DevAge.Text.FixedLength.Field(3, 5)]
		[DevAge.Text.FixedLength.ParseFormat(NumberFormat="00.00")]
        public double? F4_DoubleNullable
		{
			get{return mField3;}
			set{mField3 = value;}
		}
		[DevAge.Text.FixedLength.Field(4, 10)]
        public string F5_String
		{
			get{return mField4;}
			set{mField4 = value;}
		}
        [DevAge.Text.FixedLength.Field(5, 5)]
        [DevAge.Text.FixedLength.ParseFormat(NumberFormat = "00.00")]
        public decimal F6_Decimal
        {
            get { return mField5; }
            set { mField5 = value; }
        }

	}
}
