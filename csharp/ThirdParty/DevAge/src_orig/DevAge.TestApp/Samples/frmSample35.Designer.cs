namespace DevAge.TestApp
{
    partial class frmSample35
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSample35));
            this.txtStandard = new DevAge.Windows.Forms.DevAgeTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.validatorDouble = new DevAge.ComponentModel.Validator.ValidatorTypeConverter();
            this.validatorInt = new DevAge.ComponentModel.Validator.ValidatorTypeConverter();
            this.validatorDate = new DevAge.ComponentModel.Validator.ValidatorTypeConverter();
            this.validatorRectangle = new DevAge.ComponentModel.Validator.ValidatorTypeConverter();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDouble = new DevAge.Windows.Forms.DevAgeTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtInt = new DevAge.Windows.Forms.DevAgeTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDateTime = new DevAge.Windows.Forms.DevAgeTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRectangle = new DevAge.Windows.Forms.DevAgeTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMaskedDouble = new DevAge.Windows.Forms.DevAgeMaskedTextBox();
            this.cbEnum = new DevAge.Windows.Forms.DevAgeComboBox();
            this.validatorEnum = new DevAge.ComponentModel.Validator.ValidatorTypeConverter();
            this.cbStringArray = new DevAge.Windows.Forms.DevAgeComboBox();
            this.validatorStringArray = new DevAge.ComponentModel.Validator.ValidatorTypeConverter();
            this.label9 = new System.Windows.Forms.Label();
            this.cbCustomMapping = new DevAge.Windows.Forms.DevAgeComboBox();
            this.validatorCustomMapping = new DevAge.ComponentModel.Validator.ValidatorTypeConverter();
            this.label10 = new System.Windows.Forms.Label();
            this.lblCustomValue = new System.Windows.Forms.Label();
            this.cbCustomMapping2 = new DevAge.Windows.Forms.DevAgeComboBox();
            this.validatorCustomMapping2 = new DevAge.ComponentModel.Validator.ValidatorTypeConverter();
            this.label11 = new System.Windows.Forms.Label();
            this.lblCustomValue2 = new System.Windows.Forms.Label();
            this.txtEditorDate = new DevAge.Windows.Forms.TextBoxUITypeEditor();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtStandard
            // 
            this.txtStandard.Location = new System.Drawing.Point(209, 80);
            this.txtStandard.Name = "txtStandard";
            this.txtStandard.Size = new System.Drawing.Size(200, 20);
            this.txtStandard.TabIndex = 0;
            this.txtStandard.Text = "Hello";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "DevAgeTextBox standard";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(418, 59);
            this.label2.TabIndex = 2;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // validatorDouble
            // 
            this.validatorDouble.ValueTypeName = "System.Double, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c5" +
                "61934e089";
            // 
            // validatorInt
            // 
            this.validatorInt.ValueTypeName = "System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c56" +
                "1934e089";
            // 
            // validatorDate
            // 
            this.validatorDate.ValueTypeName = "System.DateTime, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5" +
                "c561934e089";
            // 
            // validatorRectangle
            // 
            this.validatorRectangle.ValueTypeName = "System.Drawing.Rectangle, System.Drawing, Version=2.0.0.0, Culture=neutral, Publi" +
                "cKeyToken=b03f5f7f11d50a3a";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "DevAgeTextBox double";
            // 
            // txtDouble
            // 
            this.txtDouble.FormatValue = true;
            this.txtDouble.Location = new System.Drawing.Point(209, 106);
            this.txtDouble.Name = "txtDouble";
            this.txtDouble.Size = new System.Drawing.Size(200, 20);
            this.txtDouble.TabIndex = 3;
            this.txtDouble.Text = "5";
            this.txtDouble.Validator = this.validatorDouble;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "DevAgeTextBox int";
            // 
            // txtInt
            // 
            this.txtInt.FormatValue = true;
            this.txtInt.Location = new System.Drawing.Point(209, 132);
            this.txtInt.Name = "txtInt";
            this.txtInt.Size = new System.Drawing.Size(200, 20);
            this.txtInt.TabIndex = 5;
            this.txtInt.Text = "10";
            this.txtInt.Validator = this.validatorInt;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "DevAgeTextBox DateTime";
            // 
            // txtDateTime
            // 
            this.txtDateTime.FormatValue = true;
            this.txtDateTime.Location = new System.Drawing.Point(209, 158);
            this.txtDateTime.Name = "txtDateTime";
            this.txtDateTime.Size = new System.Drawing.Size(200, 20);
            this.txtDateTime.TabIndex = 7;
            this.txtDateTime.Validator = this.validatorDate;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "DevAgeTextBox Rectangle";
            // 
            // txtRectangle
            // 
            this.txtRectangle.FormatValue = true;
            this.txtRectangle.Location = new System.Drawing.Point(209, 184);
            this.txtRectangle.Name = "txtRectangle";
            this.txtRectangle.Size = new System.Drawing.Size(200, 20);
            this.txtRectangle.TabIndex = 9;
            this.txtRectangle.Validator = this.validatorRectangle;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 216);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(158, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "DevAgeMaskedTextBox double";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 244);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "DevAgeComboBox Enum";
            // 
            // txtMaskedDouble
            // 
            this.txtMaskedDouble.Location = new System.Drawing.Point(209, 213);
            this.txtMaskedDouble.Name = "txtMaskedDouble";
            this.txtMaskedDouble.Size = new System.Drawing.Size(200, 20);
            this.txtMaskedDouble.TabIndex = 14;
            this.txtMaskedDouble.Text = "50";
            this.txtMaskedDouble.Validator = this.validatorDouble;
            // 
            // cbEnum
            // 
            this.cbEnum.FormattingEnabled = true;
            this.cbEnum.Items.AddRange(new object[] {
            System.Drawing.ContentAlignment.TopLeft,
            System.Drawing.ContentAlignment.TopCenter,
            System.Drawing.ContentAlignment.TopRight,
            System.Drawing.ContentAlignment.MiddleLeft,
            System.Drawing.ContentAlignment.MiddleCenter,
            System.Drawing.ContentAlignment.MiddleRight,
            System.Drawing.ContentAlignment.BottomLeft,
            System.Drawing.ContentAlignment.BottomCenter,
            System.Drawing.ContentAlignment.BottomRight});
            this.cbEnum.Location = new System.Drawing.Point(209, 241);
            this.cbEnum.Name = "cbEnum";
            this.cbEnum.Size = new System.Drawing.Size(200, 21);
            this.cbEnum.TabIndex = 15;
            this.cbEnum.Validator = this.validatorEnum;
            // 
            // validatorEnum
            // 
            this.validatorEnum.ValueTypeName = "System.Drawing.ContentAlignment, System.Drawing, Version=2.0.0.0, Culture=neutral" +
                ", PublicKeyToken=b03f5f7f11d50a3a";
            // 
            // cbStringArray
            // 
            this.cbStringArray.FormattingEnabled = true;
            this.cbStringArray.Location = new System.Drawing.Point(209, 268);
            this.cbStringArray.Name = "cbStringArray";
            this.cbStringArray.Size = new System.Drawing.Size(200, 21);
            this.cbStringArray.TabIndex = 17;
            this.cbStringArray.Validator = this.validatorStringArray;
            // 
            // validatorStringArray
            // 
            this.validatorStringArray.ValueTypeName = "System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c5" +
                "61934e089";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 271);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(151, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "DevAgeComboBox string array";
            // 
            // cbCustomMapping
            // 
            this.cbCustomMapping.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCustomMapping.FormattingEnabled = true;
            this.cbCustomMapping.FormatValue = true;
            this.cbCustomMapping.Location = new System.Drawing.Point(209, 295);
            this.cbCustomMapping.Name = "cbCustomMapping";
            this.cbCustomMapping.Size = new System.Drawing.Size(200, 21);
            this.cbCustomMapping.TabIndex = 19;
            this.cbCustomMapping.Validator = this.validatorCustomMapping;
            this.cbCustomMapping.SelectedValueChanged += new System.EventHandler(this.cbCustomMapping_SelectedValueChanged);
            // 
            // validatorCustomMapping
            // 
            this.validatorCustomMapping.ValueTypeName = "System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c5" +
                "61934e089";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 298);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(177, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "DevAgeComboBox custom mapping";
            // 
            // lblCustomValue
            // 
            this.lblCustomValue.AutoSize = true;
            this.lblCustomValue.Location = new System.Drawing.Point(416, 302);
            this.lblCustomValue.Name = "lblCustomValue";
            this.lblCustomValue.Size = new System.Drawing.Size(41, 13);
            this.lblCustomValue.TabIndex = 20;
            this.lblCustomValue.Text = "label11";
            // 
            // cbCustomMapping2
            // 
            this.cbCustomMapping2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCustomMapping2.FormattingEnabled = true;
            this.cbCustomMapping2.FormatValue = true;
            this.cbCustomMapping2.Location = new System.Drawing.Point(209, 322);
            this.cbCustomMapping2.Name = "cbCustomMapping2";
            this.cbCustomMapping2.Size = new System.Drawing.Size(200, 21);
            this.cbCustomMapping2.TabIndex = 22;
            this.cbCustomMapping2.Validator = this.validatorCustomMapping2;
            this.cbCustomMapping2.SelectedValueChanged += new System.EventHandler(this.cbCustomMapping2_SelectedValueChanged);
            // 
            // validatorCustomMapping2
            // 
            this.validatorCustomMapping2.ValueTypeName = "System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c56" +
                "1934e089";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 325);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(186, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "DevAgeComboBox custom mapping 2";
            // 
            // lblCustomValue2
            // 
            this.lblCustomValue2.AutoSize = true;
            this.lblCustomValue2.Location = new System.Drawing.Point(416, 330);
            this.lblCustomValue2.Name = "lblCustomValue2";
            this.lblCustomValue2.Size = new System.Drawing.Size(41, 13);
            this.lblCustomValue2.TabIndex = 23;
            this.lblCustomValue2.Text = "label11";
            // 
            // txtEditorDate
            // 
            this.txtEditorDate.BackColor = System.Drawing.Color.Transparent;
            this.txtEditorDate.Location = new System.Drawing.Point(209, 349);
            this.txtEditorDate.Name = "txtEditorDate";
            this.txtEditorDate.Size = new System.Drawing.Size(200, 20);
            this.txtEditorDate.TabIndex = 24;
            this.txtEditorDate.Validator = this.validatorDate;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 356);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(157, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "TextBoxUITypeEditor DateTime";
            // 
            // frmSample35
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 406);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtEditorDate);
            this.Controls.Add(this.lblCustomValue2);
            this.Controls.Add(this.cbCustomMapping2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblCustomValue);
            this.Controls.Add(this.cbCustomMapping);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbStringArray);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbEnum);
            this.Controls.Add(this.txtMaskedDouble);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtRectangle);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDateTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtInt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDouble);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtStandard);
            this.Name = "frmSample35";
            this.Text = "DevAge validation controls";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevAge.Windows.Forms.DevAgeTextBox txtStandard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevAge.ComponentModel.Validator.ValidatorTypeConverter validatorDouble;
        private DevAge.ComponentModel.Validator.ValidatorTypeConverter validatorInt;
        private DevAge.ComponentModel.Validator.ValidatorTypeConverter validatorDate;
        private DevAge.ComponentModel.Validator.ValidatorTypeConverter validatorRectangle;
        private System.Windows.Forms.Label label3;
        private DevAge.Windows.Forms.DevAgeTextBox txtDouble;
        private System.Windows.Forms.Label label4;
        private DevAge.Windows.Forms.DevAgeTextBox txtInt;
        private System.Windows.Forms.Label label5;
        private DevAge.Windows.Forms.DevAgeTextBox txtDateTime;
        private System.Windows.Forms.Label label6;
        private DevAge.Windows.Forms.DevAgeTextBox txtRectangle;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private DevAge.Windows.Forms.DevAgeMaskedTextBox txtMaskedDouble;
        private DevAge.Windows.Forms.DevAgeComboBox cbEnum;
        private DevAge.ComponentModel.Validator.ValidatorTypeConverter validatorEnum;
        private DevAge.Windows.Forms.DevAgeComboBox cbStringArray;
        private System.Windows.Forms.Label label9;
        private DevAge.Windows.Forms.DevAgeComboBox cbCustomMapping;
        private System.Windows.Forms.Label label10;
        private DevAge.ComponentModel.Validator.ValidatorTypeConverter validatorStringArray;
        private DevAge.ComponentModel.Validator.ValidatorTypeConverter validatorCustomMapping;
        private System.Windows.Forms.Label lblCustomValue;
        private DevAge.Windows.Forms.DevAgeComboBox cbCustomMapping2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblCustomValue2;
        private DevAge.ComponentModel.Validator.ValidatorTypeConverter validatorCustomMapping2;
        private DevAge.Windows.Forms.TextBoxUITypeEditor txtEditorDate;
        private System.Windows.Forms.Label label12;
    }
}