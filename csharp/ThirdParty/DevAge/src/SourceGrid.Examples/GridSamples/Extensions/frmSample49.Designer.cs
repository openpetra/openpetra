namespace WindowsFormsSample.GridSamples
{
    partial class frmSample49
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
            this.btViewNewEntities = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGrid1 = new SourceGrid.DataGrid();
            this.gridChanges = new SourceGrid.DataGrid();
            this.btViewChangedEntities = new System.Windows.Forms.Button();
            this.btViewDeletedEntities = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btViewNewEntities
            // 
            this.btViewNewEntities.Location = new System.Drawing.Point(6, 6);
            this.btViewNewEntities.Name = "btViewNewEntities";
            this.btViewNewEntities.Size = new System.Drawing.Size(142, 23);
            this.btViewNewEntities.TabIndex = 1;
            this.btViewNewEntities.Text = "View New Entities";
            this.btViewNewEntities.UseVisualStyleBackColor = true;
            this.btViewNewEntities.Click += new System.EventHandler(this.btViewNewEntities_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(565, 409);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGrid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(557, 383);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Data";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btViewDeletedEntities);
            this.tabPage2.Controls.Add(this.btViewChangedEntities);
            this.tabPage2.Controls.Add(this.gridChanges);
            this.tabPage2.Controls.Add(this.btViewNewEntities);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(557, 383);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Changes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGrid1
            // 
            this.dataGrid1.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.FixedRows = 1;
            this.dataGrid1.Location = new System.Drawing.Point(3, 3);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dataGrid1.Size = new System.Drawing.Size(551, 377);
            this.dataGrid1.TabIndex = 0;
            this.dataGrid1.TabStop = true;
            this.dataGrid1.ToolTipText = "";
            // 
            // gridChanges
            // 
            this.gridChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridChanges.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.gridChanges.FixedRows = 1;
            this.gridChanges.Location = new System.Drawing.Point(6, 48);
            this.gridChanges.Name = "gridChanges";
            this.gridChanges.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.gridChanges.Size = new System.Drawing.Size(545, 329);
            this.gridChanges.TabIndex = 3;
            this.gridChanges.TabStop = true;
            this.gridChanges.ToolTipText = "";
            // 
            // btViewChangedEntities
            // 
            this.btViewChangedEntities.Location = new System.Drawing.Point(154, 6);
            this.btViewChangedEntities.Name = "btViewChangedEntities";
            this.btViewChangedEntities.Size = new System.Drawing.Size(142, 23);
            this.btViewChangedEntities.TabIndex = 4;
            this.btViewChangedEntities.Text = "View Changed Entities";
            this.btViewChangedEntities.UseVisualStyleBackColor = true;
            this.btViewChangedEntities.Click += new System.EventHandler(this.btViewChangedEntities_Click);
            // 
            // btViewDeletedEntities
            // 
            this.btViewDeletedEntities.Location = new System.Drawing.Point(302, 6);
            this.btViewDeletedEntities.Name = "btViewDeletedEntities";
            this.btViewDeletedEntities.Size = new System.Drawing.Size(142, 23);
            this.btViewDeletedEntities.TabIndex = 5;
            this.btViewDeletedEntities.Text = "View Deleted Entities";
            this.btViewDeletedEntities.UseVisualStyleBackColor = true;
            this.btViewDeletedEntities.Click += new System.EventHandler(this.btViewDeletedEntities_Click);
            // 
            // frmSample49
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 433);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmSample49";
            this.Text = "Custom Entities DataBinding";
            this.Load += new System.EventHandler(this.frmSample49_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SourceGrid.DataGrid dataGrid1;
        private System.Windows.Forms.Button btViewNewEntities;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private SourceGrid.DataGrid gridChanges;
        private System.Windows.Forms.Button btViewDeletedEntities;
        private System.Windows.Forms.Button btViewChangedEntities;
    }
}