/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 13:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Controls;
namespace ControlTestBench
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.txtYaml = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbVisualStyle = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(52, 90);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(182, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "TaskList Test";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(52, 151);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(182, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "TPnlCollapsible Test";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// txtYaml
			// 
			this.txtYaml.Location = new System.Drawing.Point(134, 29);
			this.txtYaml.Name = "txtYaml";
			this.txtYaml.Size = new System.Drawing.Size(100, 20);
			this.txtYaml.TabIndex = 2;
			this.txtYaml.Text = "testYaml.yaml";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 3;
			this.label1.Text = "enter yaml file:";
			// 
			// cmbVisualStyle
			// 
			this.cmbVisualStyle.FormattingEnabled = true;
			this.cmbVisualStyle.Items.AddRange(new object[] {
									"AccordionPanel",
									"TaskPanel",
									"Dashboard",
									"Shepherd",
									"HorizontalCollapse"});
			this.cmbVisualStyle.Location = new System.Drawing.Point(134, 56);
			this.cmbVisualStyle.Name = "cmbVisualStyle";
			this.cmbVisualStyle.Size = new System.Drawing.Size(121, 21);
			this.cmbVisualStyle.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(13, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 5;
			this.label2.Text = "Visual Style";
			// 
			// linkLabel1
			// 
			this.linkLabel1.Location = new System.Drawing.Point(117, 206);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(100, 23);
			this.linkLabel1.TabIndex = 6;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "linkLabel1";
			this.linkLabel1.MouseLeave += new System.EventHandler(this.LinkLabelMouseLeave);
			this.linkLabel1.MouseHover += new System.EventHandler(this.LinkLabelMouseHover);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(52, 180);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(182, 23);
			this.button3.TabIndex = 7;
			this.button3.Text = "Shepherd Test";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.Button3Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbVisualStyle);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtYaml);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "MainForm";
			this.Text = "ControlTestBench";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.ComboBox cmbVisualStyle;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtYaml;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;

		
		void LinkLabelMouseHover(object sender, System.EventArgs e)
		{
			LinkLabel lbl = (LinkLabel)sender;
			lbl.BackColor = Color.Red;
		}
		void LinkLabelMouseLeave(object sender, System.EventArgs e)
		{
			LinkLabel lbl = (LinkLabel)sender;
			lbl.BackColor = Color.Transparent;
		}		
	}
}
