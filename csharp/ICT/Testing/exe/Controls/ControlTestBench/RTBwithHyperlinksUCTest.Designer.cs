/*
 * Created by SharpDevelop.
 * User: christiank
 * Date: 26/08/2013
 * Time: 17:21
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ControlTestBench
{
partial class RTBwithHyperlinksUCTest
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
        if (disposing)
        {
            if (components != null)
            {
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
        this.rtbHyperlinksTest = new Ict.Common.Controls.TRtbHyperlinks();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.panel1 = new System.Windows.Forms.Panel();
        this.rbtTestCaseEmailWithName2 = new System.Windows.Forms.RadioButton();
        this.rbtTestCaseEmailWithName1 = new System.Windows.Forms.RadioButton();
        this.rbtTestCaseSimpleEmails3 = new System.Windows.Forms.RadioButton();
        this.rbtTestCaseSimpleEmails2 = new System.Windows.Forms.RadioButton();
        this.rbtTestCaseSimpleEmails1 = new System.Windows.Forms.RadioButton();
        this.rbtTestCaseSimpleSingleEmail = new System.Windows.Forms.RadioButton();
        this.rbtTestCaseMultiLineDifferentHLTypes = new System.Windows.Forms.RadioButton();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.lblClickedLinkInfo = new System.Windows.Forms.Label();
        this.label1 = new System.Windows.Forms.Label();
        this.groupBox3 = new System.Windows.Forms.GroupBox();
        this.panel2 = new System.Windows.Forms.Panel();
        this.txtNumberOfLines = new System.Windows.Forms.TextBox();
        this.rbtMultiLine = new System.Windows.Forms.RadioButton();
        this.rbtSingleLine = new System.Windows.Forms.RadioButton();
        this.groupBox1.SuspendLayout();
        this.panel1.SuspendLayout();
        this.groupBox2.SuspendLayout();
        this.groupBox3.SuspendLayout();
        this.panel2.SuspendLayout();
        this.SuspendLayout();
        //
        // rtbHyperlinksTest
        //
        this.rtbHyperlinksTest.Anchor =
            ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                   System.Windows.Forms.AnchorStyles.Left) |
                                                  System.Windows.Forms.AnchorStyles.Right)));
        this.rtbHyperlinksTest.Location = new System.Drawing.Point(16, 19);
        this.rtbHyperlinksTest.Name = "rtbHyperlinksTest";
        this.rtbHyperlinksTest.Size = new System.Drawing.Size(438, 205);
        this.rtbHyperlinksTest.TabIndex = 0;
        this.rtbHyperlinksTest.LinkClicked += new Ict.Common.Controls.TRtbHyperlinks.THyperLinkClickedArgs(this.RtbHyperlinksTestLinkClicked);
        //
        // groupBox1
        //
        this.groupBox1.Controls.Add(this.panel1);
        this.groupBox1.Location = new System.Drawing.Point(11, 22);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(439, 315);
        this.groupBox1.TabIndex = 1;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Select Test Case";
        //
        // panel1
        //
        this.panel1.Controls.Add(this.rbtTestCaseEmailWithName2);
        this.panel1.Controls.Add(this.rbtTestCaseEmailWithName1);
        this.panel1.Controls.Add(this.rbtTestCaseSimpleEmails3);
        this.panel1.Controls.Add(this.rbtTestCaseSimpleEmails2);
        this.panel1.Controls.Add(this.rbtTestCaseSimpleEmails1);
        this.panel1.Controls.Add(this.rbtTestCaseSimpleSingleEmail);
        this.panel1.Controls.Add(this.rbtTestCaseMultiLineDifferentHLTypes);
        this.panel1.Location = new System.Drawing.Point(6, 19);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(427, 290);
        this.panel1.TabIndex = 1;
        //
        // rbtTestCaseEmailWithName2
        //
        this.rbtTestCaseEmailWithName2.Location = new System.Drawing.Point(3, 179);
        this.rbtTestCaseEmailWithName2.Name = "rbtTestCaseEmailWithName2";
        this.rbtTestCaseEmailWithName2.Size = new System.Drawing.Size(412, 24);
        this.rbtTestCaseEmailWithName2.TabIndex = 6;
        this.rbtTestCaseEmailWithName2.Text = "E-Mail Addresses, also with name #2  (BUGGY!)";
        this.rbtTestCaseEmailWithName2.UseVisualStyleBackColor = true;
        this.rbtTestCaseEmailWithName2.CheckedChanged += new System.EventHandler(this.AnyRadioButton_TestCaseCheckedChanged);
        //
        // rbtTestCaseEmailWithName1
        //
        this.rbtTestCaseEmailWithName1.Location = new System.Drawing.Point(3, 149);
        this.rbtTestCaseEmailWithName1.Name = "rbtTestCaseEmailWithName1";
        this.rbtTestCaseEmailWithName1.Size = new System.Drawing.Size(412, 24);
        this.rbtTestCaseEmailWithName1.TabIndex = 5;
        this.rbtTestCaseEmailWithName1.Text = "E-Mail Addresses, also with name #1";
        this.rbtTestCaseEmailWithName1.UseVisualStyleBackColor = true;
        this.rbtTestCaseEmailWithName1.CheckedChanged += new System.EventHandler(this.AnyRadioButton_TestCaseCheckedChanged);
        //
        // rbtTestCaseSimpleEmails3
        //
        this.rbtTestCaseSimpleEmails3.Location = new System.Drawing.Point(3, 123);
        this.rbtTestCaseSimpleEmails3.Name = "rbtTestCaseSimpleEmails3";
        this.rbtTestCaseSimpleEmails3.Size = new System.Drawing.Size(412, 24);
        this.rbtTestCaseSimpleEmails3.TabIndex = 4;
        this.rbtTestCaseSimpleEmails3.Text = "Simple E-Mail Addresses #3";
        this.rbtTestCaseSimpleEmails3.UseVisualStyleBackColor = true;
        this.rbtTestCaseSimpleEmails3.CheckedChanged += new System.EventHandler(this.AnyRadioButton_TestCaseCheckedChanged);
        //
        // rbtTestCaseSimpleEmails2
        //
        this.rbtTestCaseSimpleEmails2.Location = new System.Drawing.Point(3, 93);
        this.rbtTestCaseSimpleEmails2.Name = "rbtTestCaseSimpleEmails2";
        this.rbtTestCaseSimpleEmails2.Size = new System.Drawing.Size(412, 24);
        this.rbtTestCaseSimpleEmails2.TabIndex = 3;
        this.rbtTestCaseSimpleEmails2.Text = "Simple E-Mail Addresses #2";
        this.rbtTestCaseSimpleEmails2.UseVisualStyleBackColor = true;
        this.rbtTestCaseSimpleEmails2.CheckedChanged += new System.EventHandler(this.AnyRadioButton_TestCaseCheckedChanged);
        //
        // rbtTestCaseSimpleEmails1
        //
        this.rbtTestCaseSimpleEmails1.Location = new System.Drawing.Point(3, 63);
        this.rbtTestCaseSimpleEmails1.Name = "rbtTestCaseSimpleEmails1";
        this.rbtTestCaseSimpleEmails1.Size = new System.Drawing.Size(412, 24);
        this.rbtTestCaseSimpleEmails1.TabIndex = 2;
        this.rbtTestCaseSimpleEmails1.Text = "Simple E-Mail Addresses #1";
        this.rbtTestCaseSimpleEmails1.UseVisualStyleBackColor = true;
        this.rbtTestCaseSimpleEmails1.CheckedChanged += new System.EventHandler(this.AnyRadioButton_TestCaseCheckedChanged);
        //
        // rbtTestCaseSimpleSingleEmail
        //
        this.rbtTestCaseSimpleSingleEmail.Location = new System.Drawing.Point(3, 33);
        this.rbtTestCaseSimpleSingleEmail.Name = "rbtTestCaseSimpleSingleEmail";
        this.rbtTestCaseSimpleSingleEmail.Size = new System.Drawing.Size(412, 24);
        this.rbtTestCaseSimpleSingleEmail.TabIndex = 1;
        this.rbtTestCaseSimpleSingleEmail.Text = "Simple single E-Mail Address";
        this.rbtTestCaseSimpleSingleEmail.UseVisualStyleBackColor = true;
        this.rbtTestCaseSimpleSingleEmail.CheckedChanged += new System.EventHandler(this.AnyRadioButton_TestCaseCheckedChanged);
        //
        // rbtTestCaseMultiLineDifferentHLTypes
        //
        this.rbtTestCaseMultiLineDifferentHLTypes.Checked = true;
        this.rbtTestCaseMultiLineDifferentHLTypes.Location = new System.Drawing.Point(3, 3);
        this.rbtTestCaseMultiLineDifferentHLTypes.Name = "rbtTestCaseMultiLineDifferentHLTypes";
        this.rbtTestCaseMultiLineDifferentHLTypes.Size = new System.Drawing.Size(421, 24);
        this.rbtTestCaseMultiLineDifferentHLTypes.TabIndex = 0;
        this.rbtTestCaseMultiLineDifferentHLTypes.TabStop = true;
        this.rbtTestCaseMultiLineDifferentHLTypes.Text = "Multiline Text with different Hyperlink Types";
        this.rbtTestCaseMultiLineDifferentHLTypes.UseVisualStyleBackColor = true;
        this.rbtTestCaseMultiLineDifferentHLTypes.CheckedChanged += new System.EventHandler(this.AnyRadioButton_TestCaseCheckedChanged);
        //
        // groupBox2
        //
        this.groupBox2.Anchor =
            ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                   System.Windows.Forms.AnchorStyles.Left) |
                                                  System.Windows.Forms.AnchorStyles.Right)));
        this.groupBox2.Controls.Add(this.lblClickedLinkInfo);
        this.groupBox2.Controls.Add(this.label1);
        this.groupBox2.Controls.Add(this.groupBox3);
        this.groupBox2.Controls.Add(this.rtbHyperlinksTest);
        this.groupBox2.Location = new System.Drawing.Point(15, 348);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(716, 252);
        this.groupBox2.TabIndex = 2;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "rtbHyperlinks UserControl";
        //
        // lblClickedLinkInfo
        //
        this.lblClickedLinkInfo.Location = new System.Drawing.Point(152, 227);
        this.lblClickedLinkInfo.Name = "lblClickedLinkInfo";
        this.lblClickedLinkInfo.Size = new System.Drawing.Size(558, 23);
        this.lblClickedLinkInfo.TabIndex = 3;
        //
        // label1
        //
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(23, 227);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(123, 13);
        this.label1.TabIndex = 2;
        this.label1.Text = "Clicked Link Information:";
        //
        // groupBox3
        //
        this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLight;
        this.groupBox3.Controls.Add(this.panel2);
        this.groupBox3.Location = new System.Drawing.Point(472, 33);
        this.groupBox3.Name = "groupBox3";
        this.groupBox3.Size = new System.Drawing.Size(215, 131);
        this.groupBox3.TabIndex = 1;
        this.groupBox3.TabStop = false;
        this.groupBox3.Text = "Display Options for rbtHyperlinks Control";
        //
        // panel2
        //
        this.panel2.Controls.Add(this.txtNumberOfLines);
        this.panel2.Controls.Add(this.rbtMultiLine);
        this.panel2.Controls.Add(this.rbtSingleLine);
        this.panel2.Location = new System.Drawing.Point(12, 19);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(189, 94);
        this.panel2.TabIndex = 0;
        //
        // txtNumberOfLines
        //
        this.txtNumberOfLines.Enabled = false;
        this.txtNumberOfLines.Location = new System.Drawing.Point(22, 44);
        this.txtNumberOfLines.Name = "txtNumberOfLines";
        this.txtNumberOfLines.Size = new System.Drawing.Size(71, 20);
        this.txtNumberOfLines.TabIndex = 2;
        this.txtNumberOfLines.Text = "9";
        this.txtNumberOfLines.Validating += new System.ComponentModel.CancelEventHandler(this.TxtNumberOfLinesValidating);
        //
        // rbtMultiLine
        //
        this.rbtMultiLine.Checked = true;
        this.rbtMultiLine.Location = new System.Drawing.Point(3, 24);
        this.rbtMultiLine.Name = "rbtMultiLine";
        this.rbtMultiLine.Size = new System.Drawing.Size(104, 24);
        this.rbtMultiLine.TabIndex = 1;
        this.rbtMultiLine.TabStop = true;
        this.rbtMultiLine.Text = "Multiple Lines";
        this.rbtMultiLine.UseVisualStyleBackColor = true;
        //
        // rbtSingleLine
        //
        this.rbtSingleLine.Location = new System.Drawing.Point(3, 3);
        this.rbtSingleLine.Name = "rbtSingleLine";
        this.rbtSingleLine.Size = new System.Drawing.Size(104, 24);
        this.rbtSingleLine.TabIndex = 0;
        this.rbtSingleLine.Text = "Single Line";
        this.rbtSingleLine.UseVisualStyleBackColor = true;
        this.rbtSingleLine.CheckedChanged += new System.EventHandler(this.AnyRadioButton_DisplayOptionCheckedChanged);
        //
        // RTBwithHyperlinksUCTest
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(743, 612);
        this.Controls.Add(this.groupBox2);
        this.Controls.Add(this.groupBox1);
        this.Name = "RTBwithHyperlinksUCTest";
        this.Text = "RTBwithHyperlinksUCTest";
        this.groupBox1.ResumeLayout(false);
        this.panel1.ResumeLayout(false);
        this.groupBox2.ResumeLayout(false);
        this.groupBox2.PerformLayout();
        this.groupBox3.ResumeLayout(false);
        this.panel2.ResumeLayout(false);
        this.panel2.PerformLayout();
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lblClickedLinkInfo;
    private System.Windows.Forms.RadioButton rbtSingleLine;
    private System.Windows.Forms.RadioButton rbtMultiLine;
    private System.Windows.Forms.TextBox txtNumberOfLines;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.RadioButton rbtTestCaseMultiLineDifferentHLTypes;
    private System.Windows.Forms.RadioButton rbtTestCaseSimpleSingleEmail;
    private System.Windows.Forms.RadioButton rbtTestCaseSimpleEmails1;
    private System.Windows.Forms.RadioButton rbtTestCaseSimpleEmails2;
    private System.Windows.Forms.RadioButton rbtTestCaseSimpleEmails3;
    private System.Windows.Forms.RadioButton rbtTestCaseEmailWithName1;
    private System.Windows.Forms.RadioButton rbtTestCaseEmailWithName2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.GroupBox groupBox1;
    private Ict.Common.Controls.TRtbHyperlinks rtbHyperlinksTest;
}
}