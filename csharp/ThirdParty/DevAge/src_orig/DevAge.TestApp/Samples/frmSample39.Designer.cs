namespace DevAge.TestApp
{
    partial class frmSample39
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
            this.colorPickerSource = new DevAge.Windows.Forms.ColorPicker();
            this.trackBarLight = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelLightDestination = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLight)).BeginInit();
            this.SuspendLayout();
            // 
            // colorPickerSource
            // 
            this.colorPickerSource.Location = new System.Drawing.Point(78, 12);
            this.colorPickerSource.Name = "colorPickerSource";
            this.colorPickerSource.SelectedColor = System.Drawing.Color.Red;
            this.colorPickerSource.Size = new System.Drawing.Size(202, 20);
            this.colorPickerSource.TabIndex = 0;
            this.colorPickerSource.SelectedColorChanged += new System.EventHandler(this.colorPickerSource_SelectedColorChanged);
            // 
            // trackBarLight
            // 
            this.trackBarLight.Location = new System.Drawing.Point(78, 35);
            this.trackBarLight.Maximum = 100;
            this.trackBarLight.Minimum = -100;
            this.trackBarLight.Name = "trackBarLight";
            this.trackBarLight.Size = new System.Drawing.Size(202, 42);
            this.trackBarLight.TabIndex = 1;
            this.trackBarLight.TickFrequency = 10;
            this.trackBarLight.ValueChanged += new System.EventHandler(this.trackBarLight_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Source";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Light";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Destination";
            // 
            // panelLightDestination
            // 
            this.panelLightDestination.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLightDestination.Location = new System.Drawing.Point(78, 83);
            this.panelLightDestination.Name = "panelLightDestination";
            this.panelLightDestination.Size = new System.Drawing.Size(202, 24);
            this.panelLightDestination.TabIndex = 5;
            // 
            // frmSample39
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.panelLightDestination);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarLight);
            this.Controls.Add(this.colorPickerSource);
            this.Name = "frmSample39";
            this.Text = "Drawing color utilities";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevAge.Windows.Forms.ColorPicker colorPickerSource;
        private System.Windows.Forms.TrackBar trackBarLight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelLightDestination;
    }
}