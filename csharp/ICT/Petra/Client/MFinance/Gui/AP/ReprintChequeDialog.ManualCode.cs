//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2017 by OM International
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
using System.Windows.Forms;

using Ict.Common;

namespace Ict.Petra.Client.MFinance.Gui.AP
{
    public partial class TFrmReprintChequeDialog
    {
        /// <summary>
        /// Initialise the control content after creating the dialog
        /// </summary>
        public void Initialise(string ASupplierName, decimal APaymentAmount, string ACurrencyCode, int ADecimalPlaces)
        {
            txtSupplier.Text = ASupplierName;
            txtSupplier.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtSupplier.Top += 5;

            txtAmount.NumberValueDecimal = APaymentAmount;
            txtAmount.CurrencyCode = ACurrencyCode;
            txtAmount.DecimalPlaces = ADecimalPlaces;
        }

        /// <summary>
        /// Get the information that the user entered when the dialog was closed
        /// </summary>
        public void GetValidatedData(out int AChequeNumber, out string AChequeAmountInWords)
        {
            AChequeNumber = txtChequeNumber.NumberValueInt.Value;
            AChequeAmountInWords = txtAmountInWords.Text;
        }

        void BtnOK_Click(object sender, EventArgs e)
        {
            if (txtAmountInWords.Text.Trim().Length == 0)
            {
                MessageBox.Show(Catalog.GetString(
                        "Please enter the amount in words to be printed on the cheque."), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if ((txtChequeNumber.NumberValueInt.HasValue == false) || (txtChequeNumber.NumberValueInt.Value == 0))
            {
                MessageBox.Show(Catalog.GetString(
                        "Please enter the cheque number to be printed on the cheque."), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}