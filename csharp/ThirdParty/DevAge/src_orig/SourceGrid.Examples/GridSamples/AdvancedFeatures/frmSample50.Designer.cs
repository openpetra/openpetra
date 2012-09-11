namespace WindowsFormsSample.GridSamples
{
    partial class frmSample50
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
            this.grid1 = new SourceGrid.Grid();
            this.chkCol0 = new System.Windows.Forms.CheckBox();
            this.chkCol1 = new System.Windows.Forms.CheckBox();
            this.chkCol2 = new System.Windows.Forms.CheckBox();
            this.chkRow1 = new System.Windows.Forms.CheckBox();
            this.chkRow2 = new System.Windows.Forms.CheckBox();
            this.chkFirst40Row = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // grid1
            // 
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.Location = new System.Drawing.Point(12, 67);
            this.grid1.Name = "grid1";
            this.grid1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grid1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grid1.Size = new System.Drawing.Size(268, 187);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = true;
            this.grid1.ToolTipText = "";
            // 
            // chkCol0
            // 
            this.chkCol0.AutoSize = true;
            this.chkCol0.Checked = true;
            this.chkCol0.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCol0.Location = new System.Drawing.Point(12, 12);
            this.chkCol0.Name = "chkCol0";
            this.chkCol0.Size = new System.Drawing.Size(70, 17);
            this.chkCol0.TabIndex = 1;
            this.chkCol0.Text = "Column 0";
            this.chkCol0.UseVisualStyleBackColor = true;
            this.chkCol0.CheckedChanged += new System.EventHandler(this.chkVisible_CheckedChange);
            // 
            // chkCol1
            // 
            this.chkCol1.AutoSize = true;
            this.chkCol1.Checked = true;
            this.chkCol1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCol1.Location = new System.Drawing.Point(12, 35);
            this.chkCol1.Name = "chkCol1";
            this.chkCol1.Size = new System.Drawing.Size(70, 17);
            this.chkCol1.TabIndex = 2;
            this.chkCol1.Text = "Column 1";
            this.chkCol1.UseVisualStyleBackColor = true;
            this.chkCol1.CheckedChanged += new System.EventHandler(this.chkVisible_CheckedChange);
            // 
            // chkCol2
            // 
            this.chkCol2.AutoSize = true;
            this.chkCol2.Checked = true;
            this.chkCol2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCol2.Location = new System.Drawing.Point(97, 12);
            this.chkCol2.Name = "chkCol2";
            this.chkCol2.Size = new System.Drawing.Size(70, 17);
            this.chkCol2.TabIndex = 3;
            this.chkCol2.Text = "Column 2";
            this.chkCol2.UseVisualStyleBackColor = true;
            this.chkCol2.CheckedChanged += new System.EventHandler(this.chkVisible_CheckedChange);
            // 
            // chkRow1
            // 
            this.chkRow1.AutoSize = true;
            this.chkRow1.Checked = true;
            this.chkRow1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRow1.Location = new System.Drawing.Point(97, 35);
            this.chkRow1.Name = "chkRow1";
            this.chkRow1.Size = new System.Drawing.Size(57, 17);
            this.chkRow1.TabIndex = 4;
            this.chkRow1.Text = "Row 1";
            this.chkRow1.UseVisualStyleBackColor = true;
            this.chkRow1.CheckedChanged += new System.EventHandler(this.chkVisibleRow_CheckedChange);
            // 
            // chkRow2
            // 
            this.chkRow2.AutoSize = true;
            this.chkRow2.Checked = true;
            this.chkRow2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRow2.Location = new System.Drawing.Point(190, 12);
            this.chkRow2.Name = "chkRow2";
            this.chkRow2.Size = new System.Drawing.Size(57, 17);
            this.chkRow2.TabIndex = 5;
            this.chkRow2.Text = "Row 2";
            this.chkRow2.UseVisualStyleBackColor = true;
            this.chkRow2.CheckedChanged += new System.EventHandler(this.chkVisibleRow_CheckedChange);
            // 
            // chkFirst40Row
            // 
            this.chkFirst40Row.AutoSize = true;
            this.chkFirst40Row.Checked = true;
            this.chkFirst40Row.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFirst40Row.Location = new System.Drawing.Point(190, 35);
            this.chkFirst40Row.Name = "chkFirst40Row";
            this.chkFirst40Row.Size = new System.Drawing.Size(60, 17);
            this.chkFirst40Row.TabIndex = 6;
            this.chkFirst40Row.Text = "First 40";
            this.chkFirst40Row.UseVisualStyleBackColor = true;
            this.chkFirst40Row.CheckedChanged += new System.EventHandler(this.chkFirst40Row_CheckedChanged);
            // 
            // frmSample50
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.chkFirst40Row);
            this.Controls.Add(this.chkRow2);
            this.Controls.Add(this.chkRow1);
            this.Controls.Add(this.chkCol2);
            this.Controls.Add(this.chkCol1);
            this.Controls.Add(this.chkCol0);
            this.Controls.Add(this.grid1);
            this.Name = "frmSample50";
            this.Text = "Hide/show columns";
            this.Load += new System.EventHandler(this.frmSample50_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SourceGrid.Grid grid1;
        private System.Windows.Forms.CheckBox chkCol0;
        private System.Windows.Forms.CheckBox chkCol1;
        private System.Windows.Forms.CheckBox chkCol2;
        private System.Windows.Forms.CheckBox chkRow1;
        private System.Windows.Forms.CheckBox chkRow2;
        private System.Windows.Forms.CheckBox chkFirst40Row;
    }
}