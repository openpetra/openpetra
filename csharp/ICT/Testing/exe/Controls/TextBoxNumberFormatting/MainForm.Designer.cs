//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using Ict.Common.Controls;

namespace TextBoxNumberFormatting
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
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.textBox3 = new System.Windows.Forms.TextBox();
        this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.label9 = new System.Windows.Forms.Label();
        this.txtLongInteger = new Ict.Common.Controls.TTxtNumericTextBox();
        this.label5 = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.txtNormal = new Ict.Common.Controls.TTxtNumericTextBox();
        this.label2 = new System.Windows.Forms.Label();
        this.txtCurrency = new Ict.Common.Controls.TTxtCurrencyTextBox();
        this.txtDecimal = new Ict.Common.Controls.TTxtNumericTextBox();
        this.txtInteger = new Ict.Common.Controls.TTxtNumericTextBox();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.txtCurrencySymbol = new System.Windows.Forms.TextBox();
        this.label8 = new System.Windows.Forms.Label();
        this.chkPercentFormatting = new System.Windows.Forms.CheckBox();
        this.button4 = new System.Windows.Forms.Button();
        this.button3 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.checkBox1 = new System.Windows.Forms.CheckBox();
        this.label6 = new System.Windows.Forms.Label();
        this.label7 = new System.Windows.Forms.Label();
        ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
        this.groupBox1.SuspendLayout();
        this.groupBox2.SuspendLayout();
        this.SuspendLayout();
        //
        // textBox1
        //
        this.textBox1.Location = new System.Drawing.Point(114, 14);
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(170, 20);
        this.textBox1.TabIndex = 1;
        this.textBox1.Text = "8";
        this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        //
        // textBox3
        //
        this.textBox3.Location = new System.Drawing.Point(114, 200);
        this.textBox3.Name = "textBox3";
        this.textBox3.Size = new System.Drawing.Size(170, 20);
        this.textBox3.TabIndex = 4;
        this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        //
        // numericUpDown1
        //
        this.numericUpDown1.Location = new System.Drawing.Point(77, 24);
        this.numericUpDown1.Name = "numericUpDown1";
        this.numericUpDown1.Size = new System.Drawing.Size(44, 20);
        this.numericUpDown1.TabIndex = 1;
        this.numericUpDown1.Value = new decimal(new int[] {
                2,
                0,
                0,
                0
            });
        this.numericUpDown1.ValueChanged += new System.EventHandler(this.NumericUpDown1ValueChanged);
        //
        // groupBox1
        //
        this.groupBox1.Controls.Add(this.label9);
        this.groupBox1.Controls.Add(this.txtLongInteger);
        this.groupBox1.Controls.Add(this.label5);
        this.groupBox1.Controls.Add(this.label4);
        this.groupBox1.Controls.Add(this.label3);
        this.groupBox1.Controls.Add(this.txtNormal);
        this.groupBox1.Controls.Add(this.label2);
        this.groupBox1.Controls.Add(this.txtCurrency);
        this.groupBox1.Controls.Add(this.txtDecimal);
        this.groupBox1.Controls.Add(this.txtInteger);
        this.groupBox1.Location = new System.Drawing.Point(12, 40);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(295, 154);
        this.groupBox1.TabIndex = 2;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "TTxtNumericTextBoxes";
        //
        // label9
        //
        this.label9.AutoSize = true;
        this.label9.Location = new System.Drawing.Point(6, 101);
        this.label9.Name = "label9";
        this.label9.Size = new System.Drawing.Size(70, 13);
        this.label9.TabIndex = 6;
        this.label9.Text = "&Long Integer:";
        //
        // txtLongInteger
        //
        this.txtLongInteger.ControlMode = Ict.Common.Controls.TTxtNumericTextBox.TNumericTextBoxMode.LongInteger;
        this.txtLongInteger.DecimalPlaces = 0;
        this.txtLongInteger.Font =
            new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtLongInteger.Location = new System.Drawing.Point(102, 98);
        this.txtLongInteger.Name = "txtLongInteger";
        this.txtLongInteger.Size = new System.Drawing.Size(170, 21);
        this.txtLongInteger.TabIndex = 7;
        this.txtLongInteger.Text = "1234";
        this.txtLongInteger.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        //
        // label5
        //
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(6, 126);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(64, 13);
        this.label5.TabIndex = 8;
        this.label5.Text = "N&ormalText:";
        //
        // label4
        //
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(6, 76);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(43, 13);
        this.label4.TabIndex = 4;
        this.label4.Text = "&Integer:";
        //
        // label3
        //
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(6, 50);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(52, 13);
        this.label3.TabIndex = 2;
        this.label3.Text = "&Currency:";
        //
        // txtNormal
        //
        this.txtNormal.ControlMode = Ict.Common.Controls.TTxtNumericTextBox.TNumericTextBoxMode.NormalTextBox;
        this.txtNormal.DecimalPlaces = 0;
        this.txtNormal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtNormal.Location = new System.Drawing.Point(102, 123);
        this.txtNormal.Name = "txtNormal";
        this.txtNormal.Size = new System.Drawing.Size(170, 21);
        this.txtNormal.TabIndex = 9;
        this.txtNormal.Text = "asdf";
        this.txtNormal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        //
        // label2
        //
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(6, 28);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(53, 13);
        this.label2.TabIndex = 0;
        this.label2.Text = "&Decimals:";
        //
        // txtCurrency
        //
        this.txtCurrency.CurrencyCode = "WWW";
        this.txtCurrency.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtCurrency.Location = new System.Drawing.Point(102, 46);
        this.txtCurrency.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.txtCurrency.Name = "txtCurrency";
        this.txtCurrency.NumberValueDecimal = new decimal(new int[] {
                0,
                0,
                0,
                131072
            });
        this.txtCurrency.ReadOnly = false;
        this.txtCurrency.Size = new System.Drawing.Size(170, 21);
        this.txtCurrency.TabIndex = 1;
        this.txtCurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        //
        // txtDecimal
        //
        this.txtDecimal.ControlMode = Ict.Common.Controls.TTxtNumericTextBox.TNumericTextBoxMode.Decimal;
        this.txtDecimal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtDecimal.Location = new System.Drawing.Point(102, 21);
        this.txtDecimal.Name = "txtDecimal";
        this.txtDecimal.Size = new System.Drawing.Size(170, 21);
        this.txtDecimal.TabIndex = 1;
        this.txtDecimal.Text = "1,234.00";
        this.txtDecimal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        //
        // txtInteger
        //
        this.txtInteger.ControlMode = Ict.Common.Controls.TTxtNumericTextBox.TNumericTextBoxMode.Integer;
        this.txtInteger.DecimalPlaces = 0;
        this.txtInteger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtInteger.Location = new System.Drawing.Point(102, 73);
        this.txtInteger.Name = "txtInteger";
        this.txtInteger.Size = new System.Drawing.Size(170, 21);
        this.txtInteger.TabIndex = 5;
        this.txtInteger.Text = "1234";
        this.txtInteger.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        //
        // groupBox2
        //
        this.groupBox2.Controls.Add(this.txtCurrencySymbol);
        this.groupBox2.Controls.Add(this.label8);
        this.groupBox2.Controls.Add(this.chkPercentFormatting);
        this.groupBox2.Controls.Add(this.button4);
        this.groupBox2.Controls.Add(this.button3);
        this.groupBox2.Controls.Add(this.button2);
        this.groupBox2.Controls.Add(this.label1);
        this.groupBox2.Controls.Add(this.checkBox1);
        this.groupBox2.Controls.Add(this.numericUpDown1);
        this.groupBox2.Location = new System.Drawing.Point(313, 40);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(218, 176);
        this.groupBox2.TabIndex = 5;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "Dashboard";
        //
        // txtCurrencySymbol
        //
        this.txtCurrencySymbol.Location = new System.Drawing.Point(78, 68);
        this.txtCurrencySymbol.MaxLength = 3;
        this.txtCurrencySymbol.Name = "txtCurrencySymbol";
        this.txtCurrencySymbol.Size = new System.Drawing.Size(44, 20);
        this.txtCurrencySymbol.TabIndex = 9;
        this.txtCurrencySymbol.Text = "WWW";
        this.txtCurrencySymbol.TextChanged += new System.EventHandler(this.TxtCurrencySymbolTextChanged);
        //
        // label8
        //
        this.label8.Location = new System.Drawing.Point(7, 68);
        this.label8.Name = "label8";
        this.label8.Size = new System.Drawing.Size(74, 23);
        this.label8.TabIndex = 3;
        this.label8.Text = "Curr.Symbol:";
        //
        // chkPercentFormatting
        //
        this.chkPercentFormatting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
        this.chkPercentFormatting.Location = new System.Drawing.Point(16, 44);
        this.chkPercentFormatting.Name = "chkPercentFormatting";
        this.chkPercentFormatting.Size = new System.Drawing.Size(74, 24);
        this.chkPercentFormatting.TabIndex = 2;
        this.chkPercentFormatting.Text = "Percent?:";
        this.chkPercentFormatting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        this.chkPercentFormatting.UseVisualStyleBackColor = true;
        this.chkPercentFormatting.CheckedChanged += new System.EventHandler(this.ChkPercentFormattingCheckedChanged);
        //
        // button4
        //
        this.button4.Location = new System.Drawing.Point(113, 99);
        this.button4.Name = "button4";
        this.button4.Size = new System.Drawing.Size(75, 23);
        this.button4.TabIndex = 6;
        this.button4.Text = "S&et to null";
        this.button4.UseVisualStyleBackColor = true;
        this.button4.Click += new System.EventHandler(this.button4_Click);
        //
        // button3
        //
        this.button3.Location = new System.Drawing.Point(113, 131);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(75, 23);
        this.button3.TabIndex = 8;
        this.button3.Text = "&Set Values";
        this.button3.UseVisualStyleBackColor = true;
        this.button3.Click += new System.EventHandler(this.button3_Click);
        //
        // button2
        //
        this.button2.Location = new System.Drawing.Point(32, 131);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(75, 23);
        this.button2.TabIndex = 7;
        this.button2.Text = "&Get Values!";
        this.button2.UseVisualStyleBackColor = true;
        this.button2.Click += new System.EventHandler(this.button2_Click);
        //
        // label1
        //
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(18, 26);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(53, 13);
        this.label1.TabIndex = 0;
        this.label1.Text = "Decimals:";
        this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        //
        // checkBox1
        //
        this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
        this.checkBox1.AutoSize = true;
        this.checkBox1.Location = new System.Drawing.Point(33, 99);
        this.checkBox1.Name = "checkBox1";
        this.checkBox1.Size = new System.Drawing.Size(74, 23);
        this.checkBox1.TabIndex = 5;
        this.checkBox1.Text = "&Null allowed";
        this.checkBox1.UseVisualStyleBackColor = true;
        this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
        //
        // label6
        //
        this.label6.AutoSize = true;
        this.label6.Location = new System.Drawing.Point(12, 17);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(94, 13);
        this.label6.TabIndex = 0;
        this.label6.Text = "Normal TextBox &1:";
        //
        // label7
        //
        this.label7.AutoSize = true;
        this.label7.Location = new System.Drawing.Point(12, 203);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(94, 13);
        this.label7.TabIndex = 3;
        this.label7.Text = "Normal TextBox &2:";
        //
        // MainForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(544, 227);
        this.Controls.Add(this.groupBox2);
        this.Controls.Add(this.groupBox1);
        this.Controls.Add(this.label7);
        this.Controls.Add(this.label6);
        this.Controls.Add(this.textBox3);
        this.Controls.Add(this.textBox1);
        this.Name = "MainForm";
        this.Text = "TextBoxNumberFormatting";
        ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.groupBox2.ResumeLayout(false);
        this.groupBox2.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.TextBox txtCurrencySymbol;
    private Ict.Common.Controls.TTxtCurrencyTextBox txtCurrency;

    private Ict.Common.Controls.TTxtNumericTextBox txtLongInteger;
    private System.Windows.Forms.Label label9;

    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.CheckBox chkPercentFormatting;
    private System.Windows.Forms.NumericUpDown numericUpDown1;
    private System.Windows.Forms.TextBox textBox3;
    private Ict.Common.Controls.TTxtNumericTextBox txtDecimal;
    private System.Windows.Forms.TextBox textBox1;

    private TTxtNumericTextBox txtInteger;
    private TTxtNumericTextBox txtNormal;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button4;
}
}