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
	[Sample("Other controls", 33, "DataSet command builder")]
	public class frmSample33 : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components = null;

		public frmSample33()
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtDatasetDefinition = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSqlOutput = new System.Windows.Forms.TextBox();
            this.btGenerate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(460, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "This example create the SQL commands for INSERT, UPDATE AND DELETE from a typed D" +
                "ataSet for SQL Server database.";
            // 
            // txtDatasetDefinition
            // 
            this.txtDatasetDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatasetDefinition.Location = new System.Drawing.Point(4, 64);
            this.txtDatasetDefinition.Multiline = true;
            this.txtDatasetDefinition.Name = "txtDatasetDefinition";
            this.txtDatasetDefinition.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDatasetDefinition.Size = new System.Drawing.Size(460, 88);
            this.txtDatasetDefinition.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Dataset XSD definition";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Output SQL";
            // 
            // txtSqlOutput
            // 
            this.txtSqlOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSqlOutput.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSqlOutput.Location = new System.Drawing.Point(4, 196);
            this.txtSqlOutput.Multiline = true;
            this.txtSqlOutput.Name = "txtSqlOutput";
            this.txtSqlOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSqlOutput.Size = new System.Drawing.Size(460, 108);
            this.txtSqlOutput.TabIndex = 3;
            this.txtSqlOutput.WordWrap = false;
            // 
            // btGenerate
            // 
            this.btGenerate.Location = new System.Drawing.Point(388, 156);
            this.btGenerate.Name = "btGenerate";
            this.btGenerate.Size = new System.Drawing.Size(75, 23);
            this.btGenerate.TabIndex = 5;
            this.btGenerate.Text = "Generate";
            this.btGenerate.Click += new System.EventHandler(this.btGenerate_Click);
            // 
            // frmSample33
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(468, 306);
            this.Controls.Add(this.btGenerate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSqlOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDatasetDefinition);
            this.Controls.Add(this.label1);
            this.Name = "frmSample33";
            this.Text = "SQL Command builder";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDatasetDefinition;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtSqlOutput;
		private System.Windows.Forms.Button btGenerate;
		private System.Windows.Forms.Label label2;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			
			Samples.TypedDatasetSample dsTest = new Samples.TypedDatasetSample();
			txtDatasetDefinition.Text = dsTest.GetXmlSchema();
		}

		private void btGenerate_Click(object sender, System.EventArgs e)
		{
			try
			{
				System.Data.DataSet ds = new System.Data.DataSet();

				using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
				{
					System.IO.StreamWriter writer = new System.IO.StreamWriter(stream, System.Text.Encoding.Unicode);
					writer.Write(txtDatasetDefinition.Text);
					writer.Flush();
					stream.Seek(0, System.IO.SeekOrigin.Begin);
					ds.ReadXmlSchema(stream);
					writer.Close();
				}


				System.Text.StringBuilder outText = new System.Text.StringBuilder();
                outText.Append("--SQL Command builder\r\n");
				foreach (System.Data.DataTable table in ds.Tables)
				{
                    outText.Append("-- Table : " + table.TableName + "\r\n");

                    DevAge.Data.SqlClient.SqlCommandBuilder cmdBuilder = new DevAge.Data.SqlClient.SqlCommandBuilder(table);

                    outText.AppendLine(cmdBuilder.GetDeleteCommand().CommandText);
                    outText.AppendLine(cmdBuilder.GetUpdateCommand().CommandText);
                    outText.AppendLine(cmdBuilder.GetInsertCommand().CommandText);

                    outText.Append("-- ############################################################\r\n");
				}

                txtSqlOutput.Text = outText.ToString();
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message);
			}
		}


	}

}
