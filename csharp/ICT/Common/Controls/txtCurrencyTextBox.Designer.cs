//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
namespace Ict.Common.Controls
{
    partial class TTxtCurrencyTextBox
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Disposes resources used by the control.
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
            this.FTxtNumeric = new Ict.Common.Controls.TTxtNumericTextBox();
            this.FLblCurrency = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // FTxtNumeric
            // 
            this.FTxtNumeric.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.FTxtNumeric.ControlMode = Ict.Common.Controls.TTxtNumericTextBox.TNumericTextBoxMode.Decimal;
            this.FTxtNumeric.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FTxtNumeric.Location = new System.Drawing.Point(0, 0);
            this.FTxtNumeric.Name = "FTxtNumeric";
            this.FTxtNumeric.Size = new System.Drawing.Size(157, 21);
            this.FTxtNumeric.TabIndex = 0;
            this.FTxtNumeric.Text = "1,234.00";
            this.FTxtNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.FTxtNumeric.DoubleClick += new System.EventHandler(this.FLblCurrencyDoubleClick);   // only for debugging the layout of the Controls
            // 
            // FLblCurrency
            // 
            this.FLblCurrency.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.FLblCurrency.BackColor = System.Drawing.SystemColors.Control;
            this.FLblCurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F);
            this.FLblCurrency.Location = new System.Drawing.Point(157, 0);
            this.FLblCurrency.Name = "FLblCurrency";
            this.FLblCurrency.Padding = new System.Windows.Forms.Padding(2, 4, 0, 0);
            this.FLblCurrency.Size = new System.Drawing.Size(43, 22);
            this.FLblCurrency.TabIndex = 1;
            this.FLblCurrency.Text = "WWW";
            this.FLblCurrency.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.FLblCurrency.DoubleClick += new System.EventHandler(this.FLblCurrencyDoubleClick);   // only for debugging the layout of the Controls
            // 
            // TTxtCurrencyTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.FTxtNumeric);
            this.Controls.Add(this.FLblCurrency);
            this.Name = "TTxtCurrencyTextBox";
            this.Size = new System.Drawing.Size(200, 22);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.TTxtCurrencyTextBoxLayout);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Label FLblCurrency;
        private Ict.Common.Controls.TTxtNumericTextBox FTxtNumeric;
    }
}
