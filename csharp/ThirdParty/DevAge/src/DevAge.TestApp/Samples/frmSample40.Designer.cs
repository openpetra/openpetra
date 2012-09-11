namespace DevAge.TestApp.Samples
{
    partial class frmSample40
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.btTest = new System.Windows.Forms.Button();
            this.panelDraw = new System.Windows.Forms.Panel();
            this.lblPerfoamnceResult = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTestList = new DevAge.Windows.Forms.DevAgeComboBox();
            this.chkUseOnPaint = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(12, 48);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(75, 23);
            this.btTest.TabIndex = 0;
            this.btTest.Text = "Run Test";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // panelDraw
            // 
            this.panelDraw.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDraw.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDraw.Location = new System.Drawing.Point(218, 12);
            this.panelDraw.Name = "panelDraw";
            this.panelDraw.Size = new System.Drawing.Size(249, 198);
            this.panelDraw.TabIndex = 2;
            this.panelDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDraw_Paint);
            // 
            // lblPerfoamnceResult
            // 
            this.lblPerfoamnceResult.AutoSize = true;
            this.lblPerfoamnceResult.Location = new System.Drawing.Point(72, 78);
            this.lblPerfoamnceResult.Name = "lblPerfoamnceResult";
            this.lblPerfoamnceResult.Size = new System.Drawing.Size(13, 13);
            this.lblPerfoamnceResult.TabIndex = 3;
            this.lblPerfoamnceResult.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Results:";
            // 
            // cbTestList
            // 
            this.cbTestList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTestList.FormattingEnabled = true;
            this.cbTestList.Location = new System.Drawing.Point(12, 12);
            this.cbTestList.Name = "cbTestList";
            this.cbTestList.Size = new System.Drawing.Size(200, 21);
            this.cbTestList.TabIndex = 1;
            // 
            // chkUseOnPaint
            // 
            this.chkUseOnPaint.AutoSize = true;
            this.chkUseOnPaint.Location = new System.Drawing.Point(15, 103);
            this.chkUseOnPaint.Name = "chkUseOnPaint";
            this.chkUseOnPaint.Size = new System.Drawing.Size(86, 17);
            this.chkUseOnPaint.TabIndex = 5;
            this.chkUseOnPaint.Text = "Use OnPaint";
            this.chkUseOnPaint.UseVisualStyleBackColor = true;
            // 
            // frmSample40
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 222);
            this.Controls.Add(this.chkUseOnPaint);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPerfoamnceResult);
            this.Controls.Add(this.panelDraw);
            this.Controls.Add(this.cbTestList);
            this.Controls.Add(this.btTest);
            this.Name = "frmSample40";
            this.Text = "Performance test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btTest;
        private DevAge.Windows.Forms.DevAgeComboBox cbTestList;
        private System.Windows.Forms.Panel panelDraw;
        private System.Windows.Forms.Label lblPerfoamnceResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkUseOnPaint;
    }
}