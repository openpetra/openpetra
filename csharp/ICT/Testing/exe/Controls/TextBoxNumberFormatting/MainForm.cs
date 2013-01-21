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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Ict.Common.Controls;

namespace TextBoxNumberFormatting
{
/// <summary>
/// Description of MainForm.
/// </summary>
public partial class MainForm : Form
{
    /// <summary>
    /// constructor
    /// </summary>
    public MainForm()
    {
        //
        // The InitializeComponent() call is required for Windows Forms designer support.
        //
        InitializeComponent();

//            MessageBox.Show("BEFORE initialising Control values");
        this.txtDecimal.NumberValueDouble = 1234.21;
        this.txtCurrency.NumberValueDouble = 77.86;
        this.txtInteger.NumberValueInt = 2147483647;                    // highest allowed value
        this.txtLongInteger.NumberValueLongInt = 9223372036854775807;   // highest allowed value
//            MessageBox.Show("AFTER initialising Control values");
    }

    void NumericUpDown1ValueChanged(object sender, System.EventArgs e)
    {
        this.txtDecimal.DecimalPlaces = (int)numericUpDown1.Value;
        this.txtCurrency.DecimalPlaces = (int)numericUpDown1.Value;
        this.txtInteger.DecimalPlaces = (int)numericUpDown1.Value;         // while this doesn't make sense, it must do no harm to set this property on this Control
        this.txtLongInteger.DecimalPlaces = (int)numericUpDown1.Value;         // while this doesn't make sense, it must do no harm to set this property on this Control
        this.txtNormal.DecimalPlaces = (int)numericUpDown1.Value;          // while this doesn't make sense, it must do no harm to set this property on this Control
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
        this.txtDecimal.NullValueAllowed = checkBox1.Checked;
        this.txtCurrency.NullValueAllowed = checkBox1.Checked;
        this.txtInteger.NullValueAllowed = checkBox1.Checked;
        this.txtLongInteger.NullValueAllowed = checkBox1.Checked;
        this.txtNormal.NullValueAllowed = checkBox1.Checked;
    }

    private void button2_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            "Decimal: " + txtDecimal.NumberValueDouble.ToString() + Environment.NewLine +
            "  as Decimal: " + txtDecimal.NumberValueDecimal.ToString() + Environment.NewLine +
            "Currency: " + txtCurrency.NumberValueDecimal.ToString() + Environment.NewLine +
            "  as Decimal: " + txtCurrency.NumberValueDecimal.ToString() + Environment.NewLine +
            "Integer: " + txtInteger.NumberValueInt.ToString() + Environment.NewLine +
            "Long Integer: " + txtLongInteger.NumberValueLongInt.ToString() + Environment.NewLine +
            "Normal: " + txtNormal.Text.ToString() + Environment.NewLine,
            "Values of the txtNumericTextBoxes");
    }

    private void button3_Click(object sender, EventArgs e)
    {
        if (txtDecimal.NumberValueDouble != null)
        {
            //txtDecimal.NumberValueDouble = txtDecimal.NumberValueDouble + 3.12;
            txtDecimal.NumberValueDecimal = txtDecimal.NumberValueDecimal + (decimal)3.12;
        }
        else
        {
            //txtDecimal.NumberValueDouble = 23.56;
            txtDecimal.NumberValueDecimal = (decimal)23.56;
        }

        if (txtCurrency.NumberValueDouble != null)
        {
            //txtCurrency.NumberValueDouble = txtCurrency.NumberValueDouble + 5.80;
            txtCurrency.NumberValueDecimal = txtCurrency.NumberValueDecimal + (decimal)5.80;
        }
        else
        {
            //txtCurrency.NumberValueDouble = 42.89;
            txtCurrency.NumberValueDecimal = (decimal)42.89;
        }

        if (txtInteger.NumberValueInt != null)
        {
            txtInteger.NumberValueInt = txtInteger.NumberValueInt - 6;
        }
        else
        {
            txtInteger.NumberValueInt = 81;
        }

        if (txtLongInteger.NumberValueLongInt != null)
        {
            txtLongInteger.NumberValueLongInt = txtLongInteger.NumberValueLongInt - 8;
        }
        else
        {
            txtLongInteger.NumberValueLongInt = 101;
        }
    }

    private void button4_Click(object sender, EventArgs e)
    {
        txtDecimal.NumberValueDouble = null;
        txtCurrency.NumberValueDouble = null;
        txtInteger.NumberValueInt = null;
        txtLongInteger.NumberValueLongInt = null;
    }

    void ChkPercentFormattingCheckedChanged(object sender, EventArgs e)
    {
        this.txtDecimal.ShowPercentSign = chkPercentFormatting.Checked;
        this.txtInteger.ShowPercentSign = chkPercentFormatting.Checked;
        this.txtLongInteger.ShowPercentSign = chkPercentFormatting.Checked;
        this.txtNormal.ShowPercentSign = chkPercentFormatting.Checked;                 // while this doesn't make sense, it must do no harm to set this property on this Control
    }

    void TxtCurrencySymbolTextChanged(object sender, System.EventArgs e)
    {
        txtCurrency.CurrencySymbol = txtCurrencySymbol.Text;
    }
}
}