namespace DevAge.TestApp
{
    partial class frmSample36
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
            this.components = new System.ComponentModel.Container();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.validatorSize = new DevAge.ComponentModel.Validator.ValidatorTypeConverter();
            this.btMeasure = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btSetSize = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbList = new System.Windows.Forms.ComboBox();
            this.propertyGridVisualElement = new System.Windows.Forms.PropertyGrid();
            this.txtMaxSize = new DevAge.Windows.Forms.DevAgeTextBox();
            this.txtSize = new DevAge.Windows.Forms.DevAgeTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.BackColor = System.Drawing.Color.White;
            this.contentPanel.Location = new System.Drawing.Point(16, 314);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(142, 63);
            this.contentPanel.TabIndex = 0;
            this.toolTip.SetToolTip(this.contentPanel, "ToolTip");
            this.contentPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.contentPanel_MouseMove);
            this.contentPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.contentPanel_Paint);
            // 
            // validatorSize
            // 
            this.validatorSize.ValueTypeName = "System.Drawing.Size, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyT" +
                "oken=b03f5f7f11d50a3a";
            // 
            // btMeasure
            // 
            this.btMeasure.Location = new System.Drawing.Point(16, 285);
            this.btMeasure.Name = "btMeasure";
            this.btMeasure.Size = new System.Drawing.Size(61, 23);
            this.btMeasure.TabIndex = 5;
            this.btMeasure.Text = "Measure";
            this.btMeasure.UseVisualStyleBackColor = true;
            this.btMeasure.Click += new System.EventHandler(this.btMeasure_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 264);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Size";
            // 
            // btSetSize
            // 
            this.btSetSize.Location = new System.Drawing.Point(83, 285);
            this.btSetSize.Name = "btSetSize";
            this.btSetSize.Size = new System.Drawing.Size(92, 23);
            this.btSetSize.TabIndex = 7;
            this.btSetSize.Text = "Set Size";
            this.btSetSize.UseVisualStyleBackColor = true;
            this.btSetSize.Click += new System.EventHandler(this.btSetSize_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(179, 264);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Max Size";
            // 
            // cbList
            // 
            this.cbList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbList.FormattingEnabled = true;
            this.cbList.Location = new System.Drawing.Point(92, 12);
            this.cbList.Name = "cbList";
            this.cbList.Size = new System.Drawing.Size(243, 21);
            this.cbList.TabIndex = 11;
            this.cbList.SelectedIndexChanged += new System.EventHandler(this.cbList_SelectedIndexChanged);
            // 
            // propertyGridVisualElement
            // 
            this.propertyGridVisualElement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridVisualElement.Location = new System.Drawing.Point(16, 40);
            this.propertyGridVisualElement.Name = "propertyGridVisualElement";
            this.propertyGridVisualElement.Size = new System.Drawing.Size(319, 211);
            this.propertyGridVisualElement.TabIndex = 12;
            this.propertyGridVisualElement.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // txtMaxSize
            // 
            this.txtMaxSize.Location = new System.Drawing.Point(235, 261);
            this.txtMaxSize.Name = "txtMaxSize";
            this.txtMaxSize.Size = new System.Drawing.Size(100, 20);
            this.txtMaxSize.TabIndex = 9;
            this.txtMaxSize.Validator = this.validatorSize;
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(46, 261);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(100, 20);
            this.txtSize.TabIndex = 4;
            this.txtSize.Validator = this.validatorSize;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "VisualElement";
            // 
            // toolTip
            // 
            // 
            // frmSample36
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 441);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.propertyGridVisualElement);
            this.Controls.Add(this.cbList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMaxSize);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.btMeasure);
            this.Controls.Add(this.btSetSize);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.label2);
            this.Name = "frmSample36";
            this.Text = "Measure and drawing elements";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel contentPanel;
        private DevAge.Windows.Forms.DevAgeTextBox txtSize;
        private System.Windows.Forms.Button btMeasure;
        private System.Windows.Forms.Label label2;
        private DevAge.ComponentModel.Validator.ValidatorTypeConverter validatorSize;
        private System.Windows.Forms.Button btSetSize;
        private DevAge.Windows.Forms.DevAgeTextBox txtMaxSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbList;
        private System.Windows.Forms.PropertyGrid propertyGridVisualElement;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip;

    }
}