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
using Ict.Common.Data;
using Ict.Common.Conversion;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance;

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

        private void CreatePaymentTableEntries(AccountsPayableTDS ADataset, List <Int32>ADocumentsToPay)
        {
            ADataset.AApDocument.DefaultView.Sort = AApDocumentTable.GetApNumberDBName();

            foreach (Int32 apnumber in ADocumentsToPay)
            {
                int indexDocument = ADataset.AApDocument.DefaultView.Find(apnumber);

                if (indexDocument != -1)
                {
                    AccountsPayableTDSAApDocumentRow apdocument =
                        (AccountsPayableTDSAApDocumentRow)ADataset.AApDocument.DefaultView[indexDocument].Row;

                    AApSupplierRow supplier = TFrmAPMain.GetSupplier(ADataset.AApSupplier, apdocument.PartnerKey);

                    if (supplier == null)
                    {
                        // I need to load the supplier record into the TDS...
                        ADataset.Merge(TRemote.MFinance.AP.WebConnectors.LoadAApSupplier(apdocument.LedgerNumber, apdocument.PartnerKey));
                        supplier = TFrmAPMain.GetSupplier(ADataset.AApSupplier, apdocument.PartnerKey);
                    }

                    if (supplier != null)
                    {
                        AccountsPayableTDSAApPaymentRow supplierPaymentsRow = null;

                        // My TDS may already have a AApPayment row for this supplier.
                        ADataset.AApPayment.DefaultView.RowFilter = String.Format("{0}='{1}'", AccountsPayableTDSAApPaymentTable.GetSupplierKeyDBName(
                                ), supplier.PartnerKey);

                        if (ADataset.AApPayment.DefaultView.Count > 0)
                        {
                            supplierPaymentsRow = (AccountsPayableTDSAApPaymentRow)ADataset.AApPayment.DefaultView[0].Row;

                            if (apdocument.CreditNoteFlag)
                            {
                                supplierPaymentsRow.TotalAmountToPay -= apdocument.OutstandingAmount;
                            }
                            else
                            {
                                supplierPaymentsRow.TotalAmountToPay += apdocument.OutstandingAmount;
                            }

                            supplierPaymentsRow.Amount = supplierPaymentsRow.TotalAmountToPay; // The user may choose to change the amount paid.
                        }
                        else
                        {
                            supplierPaymentsRow = ADataset.AApPayment.NewRowTyped();
                            supplierPaymentsRow.LedgerNumber = ADataset.AApDocument[0].LedgerNumber;
                            supplierPaymentsRow.PaymentNumber = -1 * (ADataset.AApPayment.Count + 1);
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

                            if (apdocument.CreditNoteFlag)
                            {
                                supplierPaymentsRow.TotalAmountToPay = 0 - apdocument.OutstandingAmount;
                            }
                            else
                            {
                                supplierPaymentsRow.TotalAmountToPay = apdocument.OutstandingAmount;
                            }

                            supplierPaymentsRow.Amount = supplierPaymentsRow.TotalAmountToPay; // The user may choose to change the amount paid.

                            ADataset.AApPayment.Rows.Add(supplierPaymentsRow);
                        }

                        AccountsPayableTDSAApDocumentPaymentRow paymentdetails = ADataset.AApDocumentPayment.NewRowTyped();
                        paymentdetails.LedgerNumber = supplierPaymentsRow.LedgerNumber;
                        paymentdetails.PaymentNumber = supplierPaymentsRow.PaymentNumber;
                        paymentdetails.ApNumber = apnumber;
                        paymentdetails.CurrencyCode = supplier.CurrencyCode;
                        paymentdetails.Amount = apdocument.TotalAmount;
                        paymentdetails.InvoiceTotal = apdocument.OutstandingAmount;

                        if (apdocument.CreditNoteFlag)
                        {
                            paymentdetails.Amount = 0 - paymentdetails.Amount;
                            paymentdetails.InvoiceTotal = 0 - paymentdetails.InvoiceTotal;
                        }

                        paymentdetails.PayFullInvoice = true;

                        // TODO: discounts
                        paymentdetails.HasValidDiscount = false;
                        paymentdetails.DiscountPercentage = 0;
                        paymentdetails.UseDiscount = false;
                        paymentdetails.DocumentCode = apdocument.DocumentCode;
                        paymentdetails.DocType = (apdocument.CreditNoteFlag ? "CREDIT" : "INVOICE");
                        ADataset.AApDocumentPayment.Rows.Add(paymentdetails);
                    }
                }
            }

            ADataset.AApPayment.DefaultView.RowFilter = "";
        }

        /// <summary>
        /// set which payments should be paid; initialises the data of this screen
        /// </summary>
        /// <param name="ADataset"></param>
        /// <param name="ADocumentsToPay"></param>
        public void AddDocumentsToPayment(AccountsPayableTDS ADataset, List <Int32>ADocumentsToPay)
        {
            FMainDS = ADataset;

            if (FMainDS.AApPayment == null)
            {
                FMainDS.Merge(new AccountsPayableTDSAApPaymentTable()); // Because of these lines, AddDocumentsToPayment may only be called once per payment.
            }
            else
            {
                FMainDS.AApPayment.Clear();
            }

            if (FMainDS.AApDocumentPayment == null)
            {
                FMainDS.Merge(new AccountsPayableTDSAApDocumentPaymentTable());
            }
            else
            {
                FMainDS.AApDocumentPayment.Clear();
            }

            CreatePaymentTableEntries(FMainDS, ADocumentsToPay);


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
                FMainDS.AApDocumentPayment.DefaultView.RowFilter = String.Format("{0}={1}",
                    AApDocumentPaymentTable.GetPaymentNumberDBName(), PaymentRow.PaymentNumber);

                foreach (DataRowView rv in FMainDS.AApDocumentPayment.DefaultView)
                {
                    AccountsPayableTDSAApDocumentPaymentRow DocPaymentRow = (AccountsPayableTDSAApDocumentPaymentRow)rv.Row;

                    if (DocPaymentRow.Amount > DocPaymentRow.InvoiceTotal)
                    {
                        String strMessage =
                            String.Format(Catalog.GetString(
                                    "Payment of {0:n2} {1} to {2} is more than the due amount.\r\nPress OK to accept this amount."),
                                DocPaymentRow.Amount, PaymentRow.CurrencyCode, PaymentRow.SupplierName);

                        if (System.Windows.Forms.MessageBox.Show(strMessage, Catalog.GetString("OverPayment"), MessageBoxButtons.OKCancel)
                            == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }
            }

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
            TVerificationResultCollection Verifications;

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

                // After the payments screen, The status of this document may have changed.

                Form Opener = FPetraUtilsObject.GetCallerForm();

                if (Opener.GetType() == typeof(TFrmAPSupplierTransactions))
                {
                    ((TFrmAPSupplierTransactions)Opener).Reload();
                }

                Close();
            }
        }

        /// <summary>
        /// A payment made to a supplier needs to be reversed.
        /// It's done by creating and posting a set of matching "negatives" -
        /// In the simplest case this is a single credit note matching an invoice
        /// but it could be more complex. These negative documents are payed using
        /// a standard call to PostAPPayments.
        ///
        /// After the reversal, I'll also create and post new copies of all
        /// the invoices / credit notes that made up the original payment.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APaymentNumber"></param>
        public void ReversePayment(Int32 ALedgerNumber, Int32 APaymentNumber)
        {
            AccountsPayableTDS TempDS = TRemote.MFinance.AP.WebConnectors.LoadAPPayment(ALedgerNumber, APaymentNumber);

            if (TempDS.AApPayment.Rows.Count == 0) // Invalid Payment number?
            {
                MessageBox.Show(Catalog.GetString("The referenced payment Connot be loaded."), Catalog.GetString("Error"));
                return;
            }

            TempDS.AApDocument.DefaultView.Sort = AApDocumentTable.GetApNumberDBName();

            //
            // First I'll check that the amounts add up:
            //
            Decimal PaidDocumentsTotal = 0.0m;

            foreach (AApDocumentPaymentRow PaymentRow in TempDS.AApDocumentPayment.Rows)
            {
                Int32 DocIdx = TempDS.AApDocument.DefaultView.Find(PaymentRow.ApNumber);
                AApDocumentRow DocumentRow = TempDS.AApDocument[DocIdx];

                if (DocumentRow.CreditNoteFlag)
                {
                    PaidDocumentsTotal -= DocumentRow.TotalAmount;
                }
                else
                {
                    PaidDocumentsTotal += DocumentRow.TotalAmount;
                }
            }

            //
            // If this is a partial payment, I can't deal with that here...
            //
            if (PaidDocumentsTotal != TempDS.AApPayment[0].Amount)
            {
                String ErrorMsg =
                    String.Format(Catalog.GetString(
                            "This Payment cannot be reversed automatically because the total amount of the referenced documents ({0:n2} {1}) differs from the amount in the payment ({2:n2} {3})."),
                        PaidDocumentsTotal, TempDS.AApSupplier[0].CurrencyCode, TempDS.AApPayment[0].Amount, TempDS.AApSupplier[0].CurrencyCode);
                MessageBox.Show(ErrorMsg, "Reverse Payment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // TODO: Ideally find out if this payment was already reversed,
            // because if it was, perhaps the user doesn't really want to
            // reverse it again?


            //
            // Ask the user to confirm reversal of this payment
            //
            String PaymentMsg = Catalog.GetString("Do you want to reverse this payment?");

            PaymentMsg += ("\r\n" + String.Format("Payment made {0} to {1}\r\n\r\nRelated invoices:",
                               TDate.DateTimeToLongDateString2(TempDS.AApPayment[0].PaymentDate.Value), TempDS.PPartner[0].PartnerShortName));

            foreach (AApDocumentPaymentRow PaymentRow in TempDS.AApDocumentPayment.Rows)
            {
                Int32 DocIdx = TempDS.AApDocument.DefaultView.Find(PaymentRow.ApNumber);
                AApDocumentRow DocumentRow = TempDS.AApDocument[DocIdx];
                PaymentMsg += ("\r\n" + String.Format("     {2} ({3})  {0:n2} {1}",
                                   DocumentRow.TotalAmount, TempDS.AApSupplier[0].CurrencyCode, DocumentRow.DocumentCode, DocumentRow.Reference));
            }

            PaymentMsg += ("\r\n\r\n" + String.Format("Total payment {0:n2} {1}", TempDS.AApPayment[0].Amount, TempDS.AApSupplier[0].CurrencyCode));
            DialogResult YesNo = MessageBox.Show(PaymentMsg, "Reverse Payment", MessageBoxButtons.YesNo);

            if (YesNo == DialogResult.No)
            {
                return;
            }

            TDlgGLEnterDateEffective dateEffectiveDialog = new TDlgGLEnterDateEffective(
                ALedgerNumber,
                Catalog.GetString("Select posting date"),
                Catalog.GetString("The date effective for the reversal") + ":");

            if (dateEffectiveDialog.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show(Catalog.GetString("reversal was cancelled."), Catalog.GetString(
                        "Cancel"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime PostingDate = dateEffectiveDialog.SelectedDate;

            //
            // I need to create new documents and post them.

            // First, a squeaky clean TDS:
            AccountsPayableTDS ReverseDs = new AccountsPayableTDS();
            Int32 NewApNum = -1;
            TVerificationResultCollection Verifications;

            //
            // Then a reversed copy of each referenced document
            //
            foreach (AApDocumentPaymentRow PaymentRow in TempDS.AApDocumentPayment.Rows)
            {
                Int32 DocIdx = TempDS.AApDocument.DefaultView.Find(PaymentRow.ApNumber);
                AApDocumentRow OldDocumentRow = TempDS.AApDocument[DocIdx];
                AApDocumentRow NewDocumentRow = ReverseDs.AApDocument.NewRowTyped();

                DataUtilities.CopyAllColumnValues(OldDocumentRow, NewDocumentRow);
                NewDocumentRow.CreditNoteFlag = !OldDocumentRow.CreditNoteFlag; // Here's the actual reversal!
                NewDocumentRow.DocumentCode = "Reversal " + OldDocumentRow.DocumentCode;
                NewDocumentRow.Reference = "Reversal " + OldDocumentRow.Reference;
                NewDocumentRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED;

                NewDocumentRow.DateCreated = DateTime.Now;
                NewDocumentRow.DateEntered = DateTime.Now;
                NewDocumentRow.ApNumber = NewApNum;
                ReverseDs.AApDocument.Rows.Add(NewDocumentRow);

                TempDS.AApDocumentDetail.DefaultView.RowFilter = String.Format("{0}={1}",
                    AApDocumentDetailTable.GetApNumberDBName(), OldDocumentRow.ApNumber);

                foreach (DataRowView rv in TempDS.AApDocumentDetail.DefaultView)
                {
                    AApDocumentDetailRow OldDetailRow = (AApDocumentDetailRow)rv.Row;
                    AApDocumentDetailRow NewDetailRow = ReverseDs.AApDocumentDetail.NewRowTyped();
                    DataUtilities.CopyAllColumnValues(OldDetailRow, NewDetailRow);
                    NewDetailRow.ApNumber = NewApNum;
                    ReverseDs.AApDocumentDetail.Rows.Add(NewDetailRow);
                }

                //
                // if the invoice had AnalAttrib records attached, I need to copy those over..
                TempDS.AApAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1}",
                    AApAnalAttribTable.GetApNumberDBName(), OldDocumentRow.ApNumber);

                foreach (DataRowView rv in TempDS.AApAnalAttrib.DefaultView)
                {
                    AApAnalAttribRow OldAttribRow = (AApAnalAttribRow)rv.Row;
                    AApAnalAttribRow NewAttribRow = ReverseDs.AApAnalAttrib.NewRowTyped();
                    DataUtilities.CopyAllColumnValues(OldAttribRow, NewAttribRow);
                    NewAttribRow.ApNumber = NewApNum;
                    ReverseDs.AApAnalAttrib.Rows.Add(NewAttribRow);
                }

                NewApNum--; // These negative record numbers are replaced on saving.
            }

            //
            // Save these new documents:
            if (TRemote.MFinance.AP.WebConnectors.SaveAApDocument(ref ReverseDs, out Verifications) != TSubmitChangesResult.scrOK)
            {
                string ErrorMessages = String.Empty;

                foreach (TVerificationResult verif in Verifications)
                {
                    ErrorMessages += "[" + verif.ResultContext + "] " +
                                     verif.ResultTextCaption + ": " +
                                     verif.ResultText + Environment.NewLine;
                }

                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Failed to save Reversal Documents"));
                //
                // What to do now? I've nice looking documents, but I can't save them.
                //
                return;
            }

            //
            // The process of saving those new documents should have given them all shiny new ApNumbers,
            // So finally I need to make a list of those Document numbers, and post them.
            List <Int32>PostTheseDocs = new List <Int32>();

            foreach (AApDocumentRow DocumentRow in ReverseDs.AApDocument.Rows)
            {
                PostTheseDocs.Add(DocumentRow.ApNumber);
            }

            //
            // Now I can post these new documents, and pay them:
            //

            if (!TRemote.MFinance.AP.WebConnectors.PostAPDocuments(
                    ALedgerNumber,
                    PostTheseDocs,
                    PostingDate,
                    out Verifications))
            {
                string ErrorMessages = String.Empty;

                foreach (TVerificationResult verif in Verifications)
                {
                    ErrorMessages += "[" + verif.ResultContext + "] " +
                                     verif.ResultTextCaption + ": " +
                                     verif.ResultText + Environment.NewLine;
                }

                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Posting failed"));
                // What to do now? I've made new "reversal" documents, but I can't post them...
                return;
            }

            CreatePaymentTableEntries(ReverseDs, PostTheseDocs);

            if (!TRemote.MFinance.AP.WebConnectors.PostAPPayments(ReverseDs.AApPayment,
                    ReverseDs.AApDocumentPayment,
                    PostingDate, out Verifications))
            {
                string ErrorMessages = String.Empty;

                foreach (TVerificationResult verif in Verifications)
                {
                    ErrorMessages += "[" + verif.ResultContext + "] " +
                                     verif.ResultTextCaption + ": " +
                                     verif.ResultText + Environment.NewLine;
                }

                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Reverse Payment Failed"));
                //
                // What to do now? I've created these negative documents, and they're posted,
                // but they can't be paid for some reason.
                //
                return;
            }

            //
            // Now I need to re-create and Post new documents that match the previous ones that were reversed!
            //

            AccountsPayableTDS CreateDs = new AccountsPayableTDS();
            NewApNum = -1;

            foreach (AApDocumentPaymentRow PaymentRow in TempDS.AApDocumentPayment.Rows)
            {
                Int32 DocIdx = TempDS.AApDocument.DefaultView.Find(PaymentRow.ApNumber);
                AApDocumentRow OldDocumentRow = TempDS.AApDocument[DocIdx];
                AApDocumentRow NewDocumentRow = CreateDs.AApDocument.NewRowTyped();

                DataUtilities.CopyAllColumnValues(OldDocumentRow, NewDocumentRow);
                NewDocumentRow.DocumentCode = "Duplicate " + OldDocumentRow.DocumentCode;
                NewDocumentRow.Reference = "Duplicate " + OldDocumentRow.Reference;
                NewDocumentRow.DateEntered = PostingDate;
                NewDocumentRow.ApNumber = NewApNum;
                NewDocumentRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED;
                CreateDs.AApDocument.Rows.Add(NewDocumentRow);
                TempDS.AApDocumentDetail.DefaultView.RowFilter = String.Format("{0}={1}",
                    AApDocumentDetailTable.GetApNumberDBName(), OldDocumentRow.ApNumber);

                foreach (DataRowView rv in TempDS.AApDocumentDetail.DefaultView)
                {
                    AApDocumentDetailRow OldDetailRow = (AApDocumentDetailRow)rv.Row;
                    AApDocumentDetailRow NewDetailRow = CreateDs.AApDocumentDetail.NewRowTyped();
                    DataUtilities.CopyAllColumnValues(OldDetailRow, NewDetailRow);
                    NewDetailRow.ApNumber = NewApNum;
                    CreateDs.AApDocumentDetail.Rows.Add(NewDetailRow);
                }

                //
                // if the invoice had AnalAttrib records attached, I need to copy those over..
                TempDS.AApAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1}",
                    AApAnalAttribTable.GetApNumberDBName(), OldDocumentRow.ApNumber);

                foreach (DataRowView rv in TempDS.AApAnalAttrib.DefaultView)
                {
                    AApAnalAttribRow OldAttribRow = (AApAnalAttribRow)rv.Row;
                    AApAnalAttribRow NewAttribRow = CreateDs.AApAnalAttrib.NewRowTyped();
                    DataUtilities.CopyAllColumnValues(OldAttribRow, NewAttribRow);
                    NewAttribRow.ApNumber = NewApNum;
                    CreateDs.AApAnalAttrib.Rows.Add(NewAttribRow);
                }

                NewApNum--; // These negative record numbers should be replaced on posting.
            }

            if (TRemote.MFinance.AP.WebConnectors.SaveAApDocument(ref CreateDs, out Verifications) != TSubmitChangesResult.scrOK)
            {
                string ErrorMessages = String.Empty;

                foreach (TVerificationResult verif in Verifications)
                {
                    ErrorMessages += "[" + verif.ResultContext + "] " +
                                     verif.ResultTextCaption + ": " +
                                     verif.ResultText + Environment.NewLine;
                }

                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Failed to create Duplicate Documents"));
                //
                // What to do now? I've cancelled the previous payment, but I can't re-create it.
                //
                return;
            }

            //
            // The process of saving those new documents should have given them all shiny new ApNumbers,
            // So finally I need to make a list of those Document numbers, and post them.
            PostTheseDocs.Clear();

            foreach (AApDocumentRow DocumentRow in CreateDs.AApDocument.Rows)
            {
                PostTheseDocs.Add(DocumentRow.ApNumber);
            }

            if (!TRemote.MFinance.AP.WebConnectors.PostAPDocuments(ALedgerNumber, PostTheseDocs, PostingDate, out Verifications))
            {
                string ErrorMessages = String.Empty;

                foreach (TVerificationResult verif in Verifications)
                {
                    ErrorMessages += "[" + verif.ResultContext + "] " +
                                     verif.ResultTextCaption + ": " +
                                     verif.ResultText + Environment.NewLine;
                }

                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Failed to Post Duplicate Documents"));
                //
                // What to do now? These shiny new documents don't post!
                //
                return;
            }

            // TODO: print reports on successfully posted batch
            MessageBox.Show(Catalog.GetString("The AP payment has been reversed."));
            Form Opener = FPetraUtilsObject.GetCallerForm();

            if (Opener.GetType() == typeof(TFrmAPSupplierTransactions))
            {
                ((TFrmAPSupplierTransactions)Opener).Reload();
            }
        }
    }
}