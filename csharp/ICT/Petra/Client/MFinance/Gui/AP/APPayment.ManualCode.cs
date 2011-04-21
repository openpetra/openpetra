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

        private AApSupplierRow GetSupplier(Int64 APartnerKey)
        {
            FMainDS.AApSupplier.DefaultView.Sort = AApSupplierTable.GetPartnerKeyDBName();

            int indexSupplier = FMainDS.AApSupplier.DefaultView.Find(APartnerKey);

            if (indexSupplier == -1)
            {
                return null;
            }

            return FMainDS.AApSupplier[indexSupplier];
        }

        /// set which payments should be paid; initialises the data of this screen
        public void AddDocumentsToPayment(AccountsPayableTDS ADataset, List <Int32>ADocumentsToPay)
        {
            FMainDS = ADataset;

            FMainDS.AApDocument.DefaultView.Sort = AApDocumentTable.GetApNumberDBName();
            FMainDS.AApPayment.Clear();

            foreach (Int32 apnumber in ADocumentsToPay)
            {
                int indexDocument = FMainDS.AApDocument.DefaultView.Find(apnumber);

                if (indexDocument != -1)
                {
                    AApDocumentRow apdocument = FMainDS.AApDocument[indexDocument];

                    AApSupplierRow supplier = GetSupplier(apdocument.PartnerKey);

                    if (supplier == null)
                    {
                        // TODO: load supplier information if it is not already there

                        supplier = GetSupplier(apdocument.PartnerKey);
                    }

                    if (supplier != null)
                    {
                        AccountsPayableTDSAApPaymentRow supplierPaymentsRow = null;

                        FMainDS.AApPayment.DefaultView.Sort = AccountsPayableTDSAApPaymentTable.GetSupplierKeyDBName();
                        int indexSupplierPayments = FMainDS.AApPayment.DefaultView.Find(supplier.PartnerKey);

                        if (indexSupplierPayments != -1)
                        {
                            supplierPaymentsRow =
                                (AccountsPayableTDSAApPaymentRow)FMainDS.AApPayment.DefaultView[indexSupplierPayments].Row;
                        }
                        else
                        {
                            supplierPaymentsRow = FMainDS.AApPayment.NewRowTyped();
                            supplierPaymentsRow.LedgerNumber = FMainDS.AApDocument[0].LedgerNumber;
                            supplierPaymentsRow.PaymentNumber = -1 * (FMainDS.AApPayment.Count + 1);
                            supplierPaymentsRow.SupplierKey = supplier.PartnerKey;
                            supplierPaymentsRow.MethodOfPayment = supplier.PaymentType;
                            supplierPaymentsRow.BankAccount = supplier.DefaultBankAccount;

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

                            FMainDS.AApPayment.Rows.Add(supplierPaymentsRow);
                        }

                        FMainDS.AApDocument.DefaultView.Sort = AApDocumentTable.GetApNumberDBName();

                        AccountsPayableTDSAApDocumentPaymentRow paymentdetails = FMainDS.AApDocumentPayment.NewRowTyped();
                        paymentdetails.LedgerNumber = supplierPaymentsRow.LedgerNumber;
                        paymentdetails.PaymentNumber = supplierPaymentsRow.PaymentNumber;
                        paymentdetails.ApNumber = apnumber;
                        paymentdetails.PayFullInvoice = true;

                        // outstanding amount (TODO: consider partial payments!)
                        paymentdetails.TotalAmountToPay = apdocument.TotalAmount;
                        paymentdetails.Amount = paymentdetails.TotalAmountToPay;

                        // TODO: discounts
                        paymentdetails.HasValidDiscount = false;
                        paymentdetails.DiscountPercentage = 0;
                        paymentdetails.UseDiscount = false;
                        FMainDS.AApDocumentPayment.Rows.Add(paymentdetails);
                    }
                }
            }

            TFinanceControls.InitialiseAccountList(ref cmbBankAccount, FMainDS.AApDocument[0].LedgerNumber, true, false, true, true);

            grdDetails.AddTextColumn("AP No", FMainDS.AApDocumentPayment.ColumnApNumber);

            // TODO grdDetails.AddTextColumn("Invoice No", );
            // TODO grdDetails.AddTextColumn("Type", ); // invoice or credit note
            grdDetails.AddTextColumn("Discount used", FMainDS.AApDocumentPayment.ColumnUseDiscount);
            grdDetails.AddTextColumn("Amount", FMainDS.AApDocumentPayment.ColumnAmount);
            FMainDS.AApDocumentPayment.DefaultView.AllowNew = false;
            FMainDS.AApDocumentPayment.DefaultView.AllowEdit = false;

            grdPayments.AddTextColumn("Supplier", FMainDS.AApPayment.ColumnListLabel);
            FMainDS.AApPayment.DefaultView.AllowNew = false;
            grdPayments.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AApPayment.DefaultView);
            grdPayments.Refresh();
            grdPayments.Selection.SelectRow(1, true);
            FocusedRowChanged(null, null);
        }

        private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            DataRowView[] SelectedGridRow = grdPayments.SelectedDataRowsAsDataRowView;

            // TODO: store values in previously selected item???

            if (SelectedGridRow.Length >= 1)
            {
                AccountsPayableTDSAApPaymentRow row = (AccountsPayableTDSAApPaymentRow)SelectedGridRow[0].Row;

                AApSupplierRow supplier = GetSupplier(row.SupplierKey);

                txtCurrency.Text = supplier.CurrencyCode;
                cmbBankAccount.SetSelectedString(row.BankAccount);
                txtExchangeRate.Text = "1.0";

                FMainDS.AApDocumentPayment.DefaultView.RowFilter = AccountsPayableTDSAApDocumentPaymentTable.GetPaymentNumberDBName() +
                                                                   " = " + row.PaymentNumber.ToString();

                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AApDocumentPayment.DefaultView);
                grdDetails.Refresh();
                grdDetails.Selection.SelectRow(1, true);
            }
        }

        private void FocusedRowChangedDetails(System.Object sender, SourceGrid.RowEventArgs e)
        {
            // TODO
        }

        private void MakePayment(object sender, EventArgs e)
        {
            // TODO get data from controls into typed dataset

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
            }
        }
    }
}