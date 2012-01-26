//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;

namespace Ict.Petra.Client.MFinance.Gui.AP
{
    public partial class TFrmAPPayment
    {
        AccountsPayableTDS FMainDS = null;
        AccountsPayableTDSAApPaymentRow FSelectedPaymentRow = null;
        AccountsPayableTDSAApDocumentPaymentRow FSelectedDocumentRow = null;


        private void RunOnceOnActivationManual()
        {
            rbtPayFullOutstandingAmount.CheckedChanged += new EventHandler(EnablePartialPayment);
        }

        /// <summary>
        /// set which payments should be paid; initialises the data of this screen
        /// </summary>
        /// <param name="ADataset"></param>
        /// <param name="ADocumentsToPay"></param>
        public void AddDocumentsToPayment(AccountsPayableTDS ADataset, List <Int32>ADocumentsToPay)
        {
            FMainDS = ADataset;

            FMainDS.AApDocument.DefaultView.Sort = AApDocumentTable.GetApNumberDBName();
            FMainDS.AApPayment.Clear(); // Because of this line, AddDocumentsToPayment may only be called once per payment.

            foreach (Int32 apnumber in ADocumentsToPay)
            {
                int indexDocument = FMainDS.AApDocument.DefaultView.Find(apnumber);

                if (indexDocument != -1)
                {
                    AccountsPayableTDSAApDocumentRow apdocument = (AccountsPayableTDSAApDocumentRow)FMainDS.AApDocument.DefaultView[indexDocument].Row;

                    AApSupplierRow supplier = TFrmAPMain.GetSupplier(FMainDS.AApSupplier, apdocument.PartnerKey);

                    if (supplier != null)
                    {
                        AccountsPayableTDSAApPaymentRow supplierPaymentsRow = null;

                        // My TDS may already have a AApPayment row for this supplier.
                        FMainDS.AApPayment.DefaultView.RowFilter = String.Format("{0}='{1}'", AccountsPayableTDSAApPaymentTable.GetSupplierKeyDBName(), supplier.PartnerKey);
                        if (FMainDS.AApPayment.DefaultView.Count > 0)
                        {
                            supplierPaymentsRow = (AccountsPayableTDSAApPaymentRow)FMainDS.AApPayment.DefaultView[0].Row;
                            supplierPaymentsRow.TotalAmountToPay += apdocument.OutstandingAmount;
                            supplierPaymentsRow.Amount = supplierPaymentsRow.TotalAmountToPay; // The user may choose to change the amount paid.
                        }
                        else
                        {
                            supplierPaymentsRow = FMainDS.AApPayment.NewRowTyped();
                            supplierPaymentsRow.LedgerNumber = FMainDS.AApDocument[0].LedgerNumber;
                            supplierPaymentsRow.PaymentNumber = -1 * (FMainDS.AApPayment.Count + 1);
                            supplierPaymentsRow.SupplierKey = supplier.PartnerKey;
                            supplierPaymentsRow.MethodOfPayment = supplier.PaymentType;
                            supplierPaymentsRow.BankAccount = supplier.DefaultBankAccount;
                            supplierPaymentsRow.CurrencyCode = supplier.CurrencyCode;

                            // TODO: use uptodate exchange rate?
                            supplierPaymentsRow.ExchangeRateToBase = 1.0M;

                            // TODO: leave empty
                            supplierPaymentsRow.Reference = "TODO";

                            TPartnerClass partnerClass;
                            string partnerShortName;
                            TRemote.MPartner.Partner.ServerLookups.GetPartnerShortName(
                                supplier.PartnerKey,
                                out partnerShortName,
                                out partnerClass);
                            supplierPaymentsRow.SupplierName = Ict.Petra.Shared.MPartner.Calculations.FormatShortName(partnerShortName,
                                eShortNameFormat.eReverseWithoutTitle);

                            supplierPaymentsRow.ListLabel = supplierPaymentsRow.SupplierName + " (" + supplierPaymentsRow.MethodOfPayment + ")";
                            supplierPaymentsRow.TotalAmountToPay = apdocument.OutstandingAmount;
                            supplierPaymentsRow.Amount = supplierPaymentsRow.TotalAmountToPay; // The user may choose to change the amount paid.

                            FMainDS.AApPayment.Rows.Add(supplierPaymentsRow);
                        }

                        AccountsPayableTDSAApDocumentPaymentRow paymentdetails = FMainDS.AApDocumentPayment.NewRowTyped();
                        paymentdetails.LedgerNumber = supplierPaymentsRow.LedgerNumber;
                        paymentdetails.PaymentNumber = supplierPaymentsRow.PaymentNumber;
                        paymentdetails.ApNumber = apnumber;
                        paymentdetails.CurrencyCode = supplier.CurrencyCode;
                        paymentdetails.Amount = apdocument.TotalAmount;
                        paymentdetails.InvoiceTotal = apdocument.OutstandingAmount;
                        paymentdetails.PayFullInvoice = true;

                        // TODO: discounts
                        paymentdetails.HasValidDiscount = false;
                        paymentdetails.DiscountPercentage = 0;
                        paymentdetails.UseDiscount = false;
                        paymentdetails.DocumentCode = apdocument.DocumentCode;
                        paymentdetails.DocType = (apdocument.CreditNoteFlag ? "CREDIT" : "INVOICE");
                        FMainDS.AApDocumentPayment.Rows.Add(paymentdetails);
                    }
                }
            }

            FMainDS.AApPayment.DefaultView.RowFilter = "";

            TFinanceControls.InitialiseAccountList(ref cmbBankAccount, FMainDS.AApDocument[0].LedgerNumber, true, false, true, true);

            grdDetails.AddTextColumn("AP No", FMainDS.AApDocumentPayment.ColumnApNumber, 50);

            grdDetails.AddTextColumn("Invoice No", FMainDS.AApDocumentPayment.ColumnDocumentCode, 80);
            grdDetails.AddTextColumn("Type", FMainDS.AApDocumentPayment.ColumnDocType, 80);
            grdDetails.AddTextColumn("Discount used", FMainDS.AApDocumentPayment.ColumnUseDiscount, 80);
            grdDetails.AddCurrencyColumn("Amount", FMainDS.AApDocumentPayment.ColumnAmount);
            // grdDetails.AddTextColumn("Currency", FMainDS.AApDocumentPayment.ColumnCurrencyCode, 50); // I like this, but it's not required...

            grdPayments.AddTextColumn("Supplier", FMainDS.AApPayment.ColumnListLabel);


            FMainDS.AApDocumentPayment.DefaultView.AllowNew = false;
            FMainDS.AApDocumentPayment.DefaultView.AllowEdit = false;
            FMainDS.AApPayment.DefaultView.AllowNew = false;
            grdPayments.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AApPayment.DefaultView);
            grdPayments.Refresh();
            grdPayments.Selection.SelectRow(1, true);
            FocusedRowChanged(null, null);
        }

        private void CalculateTotalPayment()
        {
            FMainDS.AApDocumentPayment.DefaultView.RowFilter = String.Format("{0}={1}", 
                AApDocumentPaymentTable.GetPaymentNumberDBName(), FSelectedPaymentRow.PaymentNumber);

            FSelectedPaymentRow.Amount = 0m;
            foreach (DataRowView rv in FMainDS.AApDocumentPayment.DefaultView)
            {
                AccountsPayableTDSAApDocumentPaymentRow DocPaymentRow = (AccountsPayableTDSAApDocumentPaymentRow)rv.Row;
                FSelectedPaymentRow.Amount += DocPaymentRow.Amount;
            }

            txtTotalAmount.Text = FSelectedPaymentRow.Amount.ToString("N2");
        }

        private void EnablePartialPayment(object sender, EventArgs e)
        {
            //
            // If this invoice is already partpaid, the outstanding amount box
            // should show the amount remaining to be paid.
            //
            txtAmountToPay.Enabled = rbtPayAPartialAmount.Checked;

            FSelectedDocumentRow.PayFullInvoice = rbtPayFullOutstandingAmount.Checked;

            if (rbtPayFullOutstandingAmount.Checked)
            {
                FSelectedDocumentRow.Amount = FSelectedDocumentRow.InvoiceTotal;
            }

            txtAmountToPay.Text = FSelectedDocumentRow.Amount.ToString("N2");
            CalculateTotalPayment();
        }

        private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            DataRowView[] SelectedGridRow = grdPayments.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                FSelectedPaymentRow = (AccountsPayableTDSAApPaymentRow)SelectedGridRow[0].Row;

                AApSupplierRow supplier = TFrmAPMain.GetSupplier(FMainDS.AApSupplier, FSelectedPaymentRow.SupplierKey);

                txtCurrency.Text = supplier.CurrencyCode;
                cmbBankAccount.SetSelectedString(FSelectedPaymentRow.BankAccount);
                txtExchangeRate.Text = "1.0";

                FMainDS.AApDocumentPayment.DefaultView.RowFilter = AccountsPayableTDSAApDocumentPaymentTable.GetPaymentNumberDBName() +
                                                                   " = " + FSelectedPaymentRow.PaymentNumber.ToString();

                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AApDocumentPayment.DefaultView);
                grdDetails.Refresh();
                grdDetails.Selection.SelectRow(1, true);
                FocusedRowChangedDetails(null, null);
            }
        }

        private void FocusedRowChangedDetails(System.Object sender, SourceGrid.RowEventArgs e)
        {
            DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

            if (FSelectedDocumentRow != null)  // unload amount to pay into currently selected record
            {
                FSelectedDocumentRow.Amount = Decimal.Parse(txtAmountToPay.Text);
            }


            FSelectedDocumentRow = (AccountsPayableTDSAApDocumentPaymentRow)SelectedGridRow[0].Row;
            rbtPayFullOutstandingAmount.Checked = FSelectedDocumentRow.PayFullInvoice;
            rbtPayAPartialAmount.Checked = !rbtPayFullOutstandingAmount.Checked;

            EnablePartialPayment(null, null);
        }

        private void MakePayment(object sender, EventArgs e)
        {
            FSelectedDocumentRow.Amount = Decimal.Parse(txtAmountToPay.Text);

            //
            // I want to check whether the user is paying more than the due amount on any of these payments...
            //
            foreach (AccountsPayableTDSAApPaymentRow PaymentRow in FMainDS.AApPayment.Rows)
            {
                FMainDS.AApDocumentPayment.DefaultView.RowFilter = String.Format("{0}={1}",AApDocumentPaymentTable.GetPaymentNumberDBName(), PaymentRow.PaymentNumber);
                foreach (DataRowView rv in FMainDS.AApDocumentPayment.DefaultView)
                {
                    AccountsPayableTDSAApDocumentPaymentRow DocPaymentRow = (AccountsPayableTDSAApDocumentPaymentRow)rv.Row;
                    if (DocPaymentRow.Amount > DocPaymentRow.InvoiceTotal)
                    {
                        String strMessage = String.Format(Catalog.GetString("Payment of {0:n2} {1} to {2} is more than the due amount.\r\nPress OK to accept this amount."),
                            DocPaymentRow.Amount, PaymentRow.CurrencyCode, PaymentRow.SupplierName);

                        if (System.Windows.Forms.MessageBox.Show(strMessage, Catalog.GetString("OverPayment"), MessageBoxButtons.OKCancel)
                            == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }
            }
            TVerificationResultCollection Verifications;

            TDlgGLEnterDateEffective dateEffectiveDialog = new TDlgGLEnterDateEffective(
                FMainDS.AApDocument[0].LedgerNumber,
                Catalog.GetString("Select payment date"),
                Catalog.GetString("The date effective for the payment") + ":");

            if (dateEffectiveDialog.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show(Catalog.GetString("The payment was cancelled."), Catalog.GetString(
                        "No Success"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime PaymentDate = dateEffectiveDialog.SelectedDate;

            if (!TRemote.MFinance.AP.WebConnectors.PostAPPayments(FMainDS.AApPayment,
                    FMainDS.AApDocumentPayment,
                    PaymentDate, out Verifications))
            {
                string ErrorMessages = String.Empty;

                foreach (TVerificationResult verif in Verifications)
                {
                    ErrorMessages += "[" + verif.ResultContext + "] " +
                                     verif.ResultTextCaption + ": " +
                                     verif.ResultText + Environment.NewLine;
                }

                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Payment failed"));
            }
            else
            {
                // TODO: print reports on successfully posted batch
                MessageBox.Show(Catalog.GetString("The AP payment has been posted successfully!"));

                // TODO: show posting register of GL Batch?

                // TODO: refresh the screen, to reflect that the documents have been payed
                // TODO: refresh/notify other screens as well?
                Close();
            }
        }
    }
}