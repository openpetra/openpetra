namespace WindowsFormsSample.GridSamples
{
    partial class frmSample45
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
            this.chkTabStop = new System.Windows.Forms.CheckBox();
            this.chkEscape = new System.Windows.Forms.CheckBox();
            this.chkEnter = new System.Windows.Forms.CheckBox();
            this.chkAutomaticFocus = new System.Windows.Forms.CheckBox();
            this.chkTab = new System.Windows.Forms.CheckBox();
            this.chkArrows = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grid1 = new SourceGrid.Grid();
            this.SuspendLayout();
            // 
            // chkTabStop
            // 
            this.chkTabStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkTabStop.AutoSize = true;
            this.chkTabStop.Checked = true;
            this.chkTabStop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTabStop.Location = new System.Drawing.Point(261, 200);
            this.chkTabStop.Name = "chkTabStop";
            this.chkTabStop.Size = new System.Drawing.Size(70, 17);
            this.chkTabStop.TabIndex = 17;
            this.chkTabStop.Text = "Tab Stop";
            this.chkTabStop.UseVisualStyleBackColor = true;
            this.chkTabStop.CheckedChanged += new System.EventHandler(this.Checkbox_CheckedChanged);
            // 
            // chkEscape
            // 
            this.chkEscape.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkEscape.Checked = true;
            this.chkEscape.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEscape.Location = new System.Drawing.Point(261, 131);
            this.chkEscape.Name = "chkEscape";
            this.chkEscape.Size = new System.Drawing.Size(104, 16);
            this.chkEscape.TabIndex = 15;
            this.chkEscape.Text = "Enable Escape";
            this.chkEscape.CheckedChanged += new System.EventHandler(this.Checkbox_CheckedChanged);
            // 
            // chkEnter
            // 
            this.chkEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkEnter.Checked = true;
            this.chkEnter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnter.Location = new System.Drawing.Point(261, 111);
            this.chkEnter.Name = "chkEnter";
            this.chkEnter.Size = new System.Drawing.Size(104, 16);
            this.chkEnter.TabIndex = 14;
            this.chkEnter.Text = "Enable Enter";
            this.chkEnter.CheckedChanged += new System.EventHandler(this.Checkbox_CheckedChanged);
            // 
            // chkAutomaticFocus
            // 
            this.chkAutomaticFocus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutomaticFocus.Checked = true;
            this.chkAutomaticFocus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutomaticFocus.Location = new System.Drawing.Point(261, 163);
            this.chkAutomaticFocus.Name = "chkAutomaticFocus";
            this.chkAutomaticFocus.Size = new System.Drawing.Size(104, 16);
            this.chkAutomaticFocus.TabIndex = 16;
            this.chkAutomaticFocus.Text = "Auto Focus Cell";
            this.chkAutomaticFocus.CheckedChanged += new System.EventHandler(this.Checkbox_CheckedChanged);
            // 
            // chkTab
            // 
            this.chkTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkTab.Checked = true;
            this.chkTab.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTab.Location = new System.Drawing.Point(261, 91);
            this.chkTab.Name = "chkTab";
            this.chkTab.Size = new System.Drawing.Size(104, 16);
            this.chkTab.TabIndex = 13;
            this.chkTab.Text = "Enable Tab";
            this.chkTab.CheckedChanged += new System.EventHandler(this.Checkbox_CheckedChanged);
            // 
            // chkArrows
            // 
            this.chkArrows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkArrows.Checked = true;
            this.chkArrows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkArrows.Location = new System.Drawing.Point(261, 71);
            this.chkArrows.Name = "chkArrows";
            this.chkArrows.Size = new System.Drawing.Size(104, 16);
            this.chkArrows.TabIndex = 12;
            this.chkArrows.Text = "Enable Arrows";
            this.chkArrows.CheckedChanged += new System.EventHandler(this.Checkbox_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(269, 43);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(96, 20);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "Hello !";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(269, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 32);
            this.label1.TabIndex = 10;
            this.label1.Text = "Control to test tab navigation";
            // 
            // grid1
            // 
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.Location = new System.Drawing.Point(12, 12);
            this.grid1.Name = "grid1";
            this.grid1.Size = new System.Drawing.Size(243, 231);
            this.grid1.TabIndex = 9;
            // 
            // frmSample45
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 255);
            this.Controls.Add(this.chkTabStop);
            this.Controls.Add(this.chkEscape);
            this.Controls.Add(this.chkEnter);
            this.Controls.Add(this.chkAutomaticFocus);
            this.Controls.Add(this.chkTab);
            this.Controls.Add(this.chkArrows);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grid1);
            this.Name = "frmSample45";
            this.Text = "Grid navigation, tab, arrows and other controls";
            this.Load += new System.EventHandler(this.frmSample45_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkTabStop;
        private System.Windows.Forms.CheckBox chkEscape;
        private System.Windows.Forms.CheckBox chkEnter;
        private System.Windows.Forms.CheckBox chkAutomaticFocus;
        private System.Windows.Forms.CheckBox chkTab;
        private System.Windows.Forms.CheckBox chkArrows;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private SourceGrid.Grid grid1;
    }
}