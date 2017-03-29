//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//
// Copyright 2004-2016
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
using System.IO;
using System.Threading;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Conversion;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon.Gui;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Gui.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.AP
{
    public partial class TFrmAPPayment
    {
        string AP_PAYMENT_MSG_TITLE = Catalog.GetString("AP Payment");

        AccountsPayableTDS FMainDS = null;
        Int32 FLedgerNumber = -1;
        ALedgerRow FLedgerRow = null;
        AccountsPayableTDSAApPaymentRow FSelectedPaymentRow = null;
        AccountsPayableTDSAApDocumentPaymentRow FSelectedDocumentRow = null;
        ACurrencyTable FCurrencyTable = null;
        TFormLetterFinanceInfo FFormLetterFinanceInfo = null;
        string FBasePaymentDetailsText = string.Empty;

        // For Templater printing of remittance advice
        List <TFormData>FFormDataList = null;

        // Grid
        int FSupplierGridRow = -1;

        private void RunOnceOnActivationManual()
        {
            rbtPayFullOutstandingAmount.CheckedChanged += new EventHandler(EnablePartialPayment);
            chkClaimDiscount.Visible = false;
            txtExchangeRate.TextChanged += new EventHandler(UpdateTotalAmount);

            ALedgerRow LedgerRow =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];
            FCurrencyTable = (ACurrencyTable)TDataCache.TMCommon.GetCacheableCommonTable(TCacheableCommonTablesEnum.CurrencyCodeList);

            //txtExchangeRate.SetControlProperties(10);
            txtBaseAmount.CurrencyCode = LedgerRow.BaseCurrency;

            TExchangeRateCache.ResetCache();
            FocusedRowChanged(null, new SourceGrid.RowEventArgs(1));
        }

        private void LookupExchangeRate(Object sender, EventArgs e)
        {
            TFrmSetupDailyExchangeRate setupDailyExchangeRate =
                new TFrmSetupDailyExchangeRate(FPetraUtilsObject.GetForm());

            decimal selectedExchangeRate;
            DateTime selectedEffectiveDate;
            int selectedEffectiveTime;

            if (setupDailyExchangeRate.ShowDialog(
                    FLedgerNumber,
                    DateTime.Now,
                    txtCurrency.Text,
                    1.0m,
                    out selectedExchangeRate,
                    out selectedEffectiveDate,
                    out selectedEffectiveTime) == DialogResult.Cancel)
            {
                return;
            }

            txtExchangeRate.NumberValueDecimal = selectedExchangeRate;

            // Put the rate in our client-side cache
            if (FLedgerRow != null)
            {
                TExchangeRateCache.SetDailyExchangeRate(txtCurrency.Text, FLedgerRow.BaseCurrency, selectedEffectiveDate, selectedExchangeRate);
            }
        }

        private void ShowDataManual()
        {
            TFinanceControls.InitialiseAccountList(ref cmbBankAccount, FMainDS.AApDocument[0].LedgerNumber, true, false, true, true, "");

            FBasePaymentDetailsText = grpPaymentDetails.Text;

//          grdDocuments.AddTextColumn("AP No", FMainDS.AApDocumentPayment.ColumnApNumber, 50);
            grdDocuments.AddTextColumn("Invoice No", FMainDS.AApDocumentPayment.ColumnDocumentCode);
            grdDocuments.AddTextColumn("Type", FMainDS.AApDocumentPayment.ColumnDocType, 80);
//          grdDocuments.AddTextColumn("Discount used", FMainDS.AApDocumentPayment.ColumnUseDiscount, 80);
            grdDocuments.AddCurrencyColumn("Amount", FMainDS.AApDocumentPayment.ColumnAmount);
//          grdDocuments.AddTextColumn("Currency", FMainDS.AApPayment.ColumnCurrencyCode, 50);
            grdDocuments.Columns.StretchToFit();

            grdPayments.AddTextColumn("Supplier", FMainDS.AApPayment.ColumnListLabel);

            FMainDS.AApDocumentPayment.DefaultView.AllowNew = false;
            FMainDS.AApDocumentPayment.DefaultView.AllowEdit = false;
            FMainDS.AApPayment.DefaultView.AllowNew = false;
            grdPayments.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AApPayment.DefaultView);
            grdPayments.Refresh();
            grdPayments.Selection.SelectRow(1, true);

            // If this payment has a payment number, it's because it's already been paid, so I need to display it read-only.
            if ((FMainDS.AApPayment.Rows.Count > 0) && (FMainDS.AApPayment[0].PaymentNumber > 0))
            {
                txtPaymntNum.Text = FMainDS.AApPayment[0].PaymentNumber.ToString();
                txtAmountToPay.Enabled = false;
                txtChequeNumber.Enabled = false;
                txtCurrency.Enabled = false;
                txtExchangeRate.Enabled = false;
                txtAmountInWords.Enabled = false;
                btnLookupExchangeRate.Enabled = false;
                txtExchangeRate.NumberValueDecimal = FMainDS.AApPayment[0].ExchangeRateToBase;

                txtReference.Enabled = false;
                txtTotalAmount.Enabled = false;
                cmbBankAccount.Enabled = false;
                cmbPaymentType.Enabled = false;

                grdDocuments.Enabled = false;
                grdPayments.Enabled = false;

                tbbRemove.Enabled = false;
                tbbMakePayment.Enabled = false;
                tbbReprintCheque.Enabled = (string.Compare(FMainDS.AApPayment[0].MethodOfPayment, "Cheque", true) == 0);

                rgrAmountToPay.Enabled = false;
                tbbPrintReport.Enabled = true;
                chkPrintRemittance.Enabled = false;
                chkClaimDiscount.Enabled = false;
                chkPrintCheque.Enabled = false;
                chkPrintLabel.Enabled = false;
            }
            else
            {
                tbbPrintReport.Enabled = false;
                tbbReprintCheque.Enabled = false;
                tbbReprintRemittanceAdvice.Enabled = false;
            }
        }

        private static bool CurrencyIsOkForPaying(AccountsPayableTDS Atds, AApDocumentRow AApDocument)
        {
            if (AApDocument.CurrencyCode != Atds.AApSupplier[0].CurrencyCode)
            {
                System.Windows.Forms.MessageBox.Show(
                    String.Format(Catalog.GetString("Document {0} cannot be paid because the supplier's currency has been changed to {1}."),
                        AApDocument.DocumentCode, Atds.AApSupplier[0].CurrencyCode),
                    Catalog.GetString("Pay Document"));
                return false;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Atds"></param>
        /// <param name="AdocumentRow"></param>
        /// <returns></returns>
        public static bool ApDocumentCanPay(AccountsPayableTDS Atds, AApDocumentRow AdocumentRow)
        {
            if (!CurrencyIsOkForPaying(Atds, AdocumentRow))
            {
                return false;
            }

            if ("|POSTED|PARTPAID|".IndexOf("|" + AdocumentRow.DocumentStatus) < 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Set which payments should be paid; initialises the data of this screen
        /// </summary>
        /// <param name="ADataset"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADocumentsToPay"></param>
        /// <returns>true if there's something to pay</returns>
        public bool AddDocumentsToPayment(AccountsPayableTDS ADataset, Int32 ALedgerNumber, List <Int32>ADocumentsToPay)
        {
            FMainDS = ADataset;
            FLedgerNumber = ALedgerNumber;
            ALedgerTable Tbl = TRemote.MFinance.AP.WebConnectors.GetLedgerInfo(FLedgerNumber);
            FLedgerRow = Tbl[0];

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

            // I want to check that it'll be OK to pay these documents:
            for (Int32 Idx = ADocumentsToPay.Count - 1; Idx >= 0; Idx--)
            {
                Int32 DocId = ADocumentsToPay[Idx];
                AccountsPayableTDS tempDs = TRemote.MFinance.AP.WebConnectors.LoadAApDocument(ALedgerNumber, DocId);

                if (!ApDocumentCanPay(tempDs, tempDs.AApDocument[0]))
                {
                    ADocumentsToPay.Remove(DocId);
                }
            }

            if (ADocumentsToPay.Count == 0)
            {
                return false;
            }

            TRemote.MFinance.AP.WebConnectors.CreatePaymentTableEntries(ref FMainDS, ALedgerNumber, ADocumentsToPay);

            // Initialise the controls with our default values
            chkPrintRemittance.Checked = true;
            chkClaimDiscount.Enabled = false;
            chkPrintLabel.Enabled = false;
            ShowDataManual();

            OnChangePaymentType(null, null);    // set the enabled state for Print Cheque

            return true;
        }

        /// <summary>
        /// If asked to remove a payment,
        /// I need to modify the ApPayment to reflect the removed ApDocumentPayment row.
        /// If there's nothing left to pay, I should remove the ApPayment row.
        /// </summary>
        private void RemoveSelectedDocument(Object sender, EventArgs e)
        {
            if (FSelectedDocumentRow != null)
            {
                // It doesn't make much sense to remove the only invoice on the page
                if ((grdPayments.Rows.Count == 2) && (grdDocuments.Rows.Count == 2))
                {
                    string msg = Catalog.GetString(
                        "If you remove this invoice there will be nothing left to pay.  Do you want to close this screen without paying any invoices?");

                    if (MessageBox.Show(msg, AP_PAYMENT_MSG_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.Close();
                    }

                    return;
                }

                if (MessageBox.Show(Catalog.GetString("Are you sure you want to remove the highlighted invoice from this payment?"),
                        AP_PAYMENT_MSG_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                FSelectedPaymentRow.Amount -= FSelectedDocumentRow.Amount;

                FMainDS.AApDocumentPayment.Rows.Remove(FSelectedDocumentRow);
                FSelectedDocumentRow = null;
                FocusedRowChangedDetails(null, null);

                if (FSelectedPaymentRow.Amount <= 0)
                {
                    FMainDS.AApPayment.Rows.Remove(FSelectedPaymentRow);

                    int newSupplierRow = FSupplierGridRow;

                    if (newSupplierRow > grdPayments.Rows.Count - 1)
                    {
                        newSupplierRow = grdPayments.Rows.Count - 1;
                    }

                    grdPayments.Selection.SelectRow(newSupplierRow, true);
                    FSelectedPaymentRow = null;
                    FocusedRowChanged(null, new SourceGrid.RowEventArgs(newSupplierRow));
                }
            }
        }

        private void UpdateTotalAmount(Object sender, EventArgs e)
        {
            txtTotalAmount.NumberValueDecimal = FSelectedPaymentRow.Amount;

            if (txtExchangeRate.NumberValueDecimal.HasValue)
            {
                Decimal ExchangeRate = txtExchangeRate.NumberValueDecimal.Value;

                if (ExchangeRate != 0)
                {
                    FSelectedPaymentRow.ExchangeRateToBase = ExchangeRate;
                    txtBaseAmount.NumberValueDecimal = FSelectedPaymentRow.Amount / FSelectedPaymentRow.ExchangeRateToBase;
                }
            }
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

            UpdateTotalAmount(null, null);
        }

        private void EnablePartialPayment(object sender, EventArgs e)
        {
            //
            // If this invoice is already partpaid, the outstanding amount box
            // should show the amount remaining to be paid.
            //
            txtAmountToPay.Enabled = rbtPayPartialAmount.Checked;

            FSelectedDocumentRow.PayFullInvoice = rbtPayFullOutstandingAmount.Checked;

            if (rbtPayFullOutstandingAmount.Checked)
            {
                FSelectedDocumentRow.Amount = FSelectedDocumentRow.InvoiceTotal;
            }

            txtAmountToPay.NumberValueDecimal = FSelectedDocumentRow.Amount;
            CalculateTotalPayment();
        }

        private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            // The Grid Row that shows suppliers names has changed
            DataRowView[] SelectedPaymentRow = grdPayments.SelectedDataRowsAsDataRowView;

            if (e != null)
            {
                FSupplierGridRow = e.Row;
            }

            if (SelectedPaymentRow.Length >= 1)
            {
                GetPaymentDetailsFromControlsManual();

                FSelectedPaymentRow = (AccountsPayableTDSAApPaymentRow)SelectedPaymentRow[0].Row;

                tbbReprintRemittanceAdvice.Enabled = FSelectedPaymentRow.PaymentDate != null;
                tbbReprintCheque.Enabled =
                    ((FSelectedPaymentRow.PaymentDate != null) && (string.Compare(FSelectedPaymentRow.MethodOfPayment, "Cheque", true) == 0));

                if (!FSelectedPaymentRow.IsSupplierKeyNull())
                {
                    AApSupplierRow supplier = TFrmAPMain.GetSupplier(FMainDS.AApSupplier, FSelectedPaymentRow.SupplierKey);
                    txtCurrency.Text = supplier.CurrencyCode;
                    grpPaymentDetails.Text = FBasePaymentDetailsText + " for " + FSelectedPaymentRow.SupplierName;

                    if (FCurrencyTable != null)
                    {
                        ACurrencyRow row = (ACurrencyRow)FCurrencyTable.Rows.Find(supplier.CurrencyCode);
                        txtTotalAmount.CurrencyCode = row.CurrencyCode;
                        txtAmountToPay.CurrencyCode = row.CurrencyCode;
                    }

/*
 *                  decimal CurrentRate = TExchangeRateCache.GetDailyExchangeRate(supplier.CurrencyCode,
 *                      FLedgerRow.BaseCurrency,
 *                      DateTime.Today,
 *                      false);
 */
                    txtExchangeRate.NumberValueDecimal = FSelectedPaymentRow.ExchangeRateToBase;
                    cmbPaymentType.SetSelectedString(FSelectedPaymentRow.MethodOfPayment);     // This will trigger possible change to cheque controls

                    if (chkPrintCheque.Enabled)
                    {
                        chkPrintCheque.Checked = FSelectedPaymentRow.PrintCheque;
                        txtChequeNumber.NumberValueInt = FSelectedPaymentRow.ChequeNumber;
                        txtAmountInWords.Text = FSelectedPaymentRow.ChequeAmountInWords;
                    }
                    else
                    {
                        chkPrintCheque.Checked = false;
                        txtChequeNumber.NumberValueInt = 0;
                        txtAmountInWords.Text = string.Empty;
                    }

                    chkPrintCheque_Clicked(null, null);

                    if (txtCurrency.Text == FLedgerRow.BaseCurrency)
                    {
                        txtExchangeRate.Enabled = false;
                        btnLookupExchangeRate.Enabled = false;
                    }
                    else
                    {
                        txtExchangeRate.Enabled = true;
                        btnLookupExchangeRate.Enabled = true;
                    }
                }

                cmbBankAccount.SetSelectedString(FSelectedPaymentRow.BankAccount, -1);
                txtReference.Text = FSelectedPaymentRow.Reference;
                chkPrintRemittance.Checked = FSelectedPaymentRow.PrintRemittanceAdvice;

                FMainDS.AApDocumentPayment.DefaultView.RowFilter = AccountsPayableTDSAApDocumentPaymentTable.GetPaymentNumberDBName() +
                                                                   " = " + FSelectedPaymentRow.PaymentNumber.ToString();

                grdDocuments.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AApDocumentPayment.DefaultView);
                grdDocuments.Refresh();
                grdDocuments.Selection.SelectRow(1, true);
                FocusedRowChangedDetails(null, null);
            }
            else
            {
                tbbReprintRemittanceAdvice.Enabled = false;
                tbbReprintCheque.Enabled = false;
                tbbMakePayment.Enabled = false;
                tbbRemove.Enabled = false;
            }
        }

        private void FocusedRowChangedDetails(System.Object sender, SourceGrid.RowEventArgs e)
        {
            // The Invoices to be paid grid row has changed.  This is showing the default view on FMainDS.AApDocumentPaymen
            DataRowView[] SelectedGridRow = grdDocuments.SelectedDataRowsAsDataRowView;

            if (FSelectedDocumentRow != null)  // unload amount to pay into currently selected record
            {
                //FSelectedDocumentRow.Amount = Decimal.Parse(txtAmountToPay.Text);
                FSelectedDocumentRow.Amount = txtAmountToPay.NumberValueDecimal.Value;
            }

            if (SelectedGridRow.Length == 0)
            {
                FSelectedDocumentRow = null;
                CalculateTotalPayment();
            }
            else
            {
                FSelectedDocumentRow = (AccountsPayableTDSAApDocumentPaymentRow)SelectedGridRow[0].Row;
                rbtPayFullOutstandingAmount.Checked = FSelectedDocumentRow.PayFullInvoice;
                rbtPayPartialAmount.Checked = !rbtPayFullOutstandingAmount.Checked;

                EnablePartialPayment(null, null);
            }

            grdDocuments.AutoResizeGrid();
        }

        private bool GetPaymentDetailsFromControlsManual()
        {
            if (FSelectedPaymentRow != null)
            {
                FSelectedPaymentRow.PrintRemittanceAdvice = chkPrintRemittance.Checked;
                FSelectedPaymentRow.PrintCheque = chkPrintCheque.Checked;

                if (chkPrintCheque.Checked)
                {
                    FSelectedPaymentRow.ChequeNumber = txtChequeNumber.NumberValueInt.Value;
                    FSelectedPaymentRow.ChequeAmountInWords = txtAmountInWords.Text;
                }

                FSelectedPaymentRow.BankAccount = cmbBankAccount.GetSelectedString();
                FSelectedPaymentRow.Reference = txtReference.Text;
                FSelectedPaymentRow.MethodOfPayment = cmbPaymentType.GetSelectedString();
            }

            return true;
        }

        private Boolean GetPaymentNumbersAfterPosting(AccountsPayableTDSAApPaymentTable ANewPayments,
            out Int32 MinPaymentNumber,
            out Int32 MaxPaymentNumber)
        {
            //
            // I need to find the min and max payment numbers, which have been returned from PostAPPayments..
            //
            MinPaymentNumber = 99999999;
            MaxPaymentNumber = 0;

            Boolean MaxSet = false;
            Boolean MinSet = false;

            foreach (AccountsPayableTDSAApPaymentRow PaymentRow in ANewPayments.Rows)
            {
                if (PaymentRow.PaymentNumber < MinPaymentNumber)
                {
                    MinPaymentNumber = PaymentRow.PaymentNumber;
                    MinSet = true;
                }

                if (PaymentRow.PaymentNumber > MaxPaymentNumber)
                {
                    MaxPaymentNumber = PaymentRow.PaymentNumber;
                    MaxSet = true;
                }
            }

            return MinSet && MaxSet;
        }

        private void PrintPaymentReport(object sender, EventArgs e)
        {
            PrintPaymentReport(FSelectedPaymentRow.PaymentNumber, FSelectedPaymentRow.PaymentNumber);
        }

        private void PrintPaymentReport(Int32 AMinPaymentNumber, Int32 AMaxPaymentNumber)
        {
            Int32 LedgerNumber = FMainDS.AApPayment[0].LedgerNumber;

            // Print Payment report..
            TFrmAP_PaymentReport.CreateReportNoGui(LedgerNumber, AMinPaymentNumber, AMaxPaymentNumber, this);
        }

        /// <summary>
        /// Print the remittance advice using HTML forms
        /// </summary>
        private void PrintRemittanceAdvice(AccountsPayableTDSAApPaymentTable APaymentTable)
        {
            foreach (AccountsPayableTDSAApPaymentRow paymentRow in APaymentTable.Rows)
            {
                if (paymentRow.PrintRemittanceAdvice)
                {
                    TFrmAP_RemittanceAdvice PreviewFrame = new TFrmAP_RemittanceAdvice(this);
                    PreviewFrame.PrintRemittanceAdvice(paymentRow.PaymentNumber, FMainDS.AApPayment[0].LedgerNumber);
                    PreviewFrame.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Print the remittance advice using Templater
        /// </summary>
        private void PrintRemittanceAdviceTemplater(AccountsPayableTDSAApPaymentTable APaymentTable)
        {
            // Do any payments need a remittance advice?
            List <int>paymentNumberList = new List <int>();

            foreach (AccountsPayableTDSAApPaymentRow paymentRow in APaymentTable.Rows)
            {
                if (paymentRow.PrintRemittanceAdvice)
                {
                    paymentNumberList.Add(paymentRow.PaymentNumber);
                }
            }

            if (paymentNumberList.Count > 0)
            {
                GetTemplaterFinanceInfo(MCommonConstants.FORM_CODE_REMITTANCE, out FFormLetterFinanceInfo);

                if (FFormLetterFinanceInfo == null)
                {
                    // We failed either because the user cancelled the dialog or because we could not read the template from the server.
                    //  No need to display a message here because messages have been displayed already
                    return;
                }

                paymentNumberList.Sort();
                PrintRemittanceAdviceTemplater(paymentNumberList,
                    FMainDS.AApPayment[0].LedgerNumber,
                    FFormLetterFinanceInfo);
            }
        }

        /// <summary>
        /// Print the cheque using Templater
        /// </summary>
        private void PrintChequeTemplater(AccountsPayableTDSAApPaymentTable APaymentTable)
        {
            List <int>paymentNumberList = new List <int>();

            foreach (AccountsPayableTDSAApPaymentRow paymentRow in APaymentTable.Rows)
            {
                if (paymentRow.PrintCheque)
                {
                    paymentNumberList.Add(paymentRow.PaymentNumber);
                }
            }

            if (paymentNumberList.Count > 0)
            {
                TFormLetterFinanceInfo FormLetterFinanceInfo;
                GetTemplaterFinanceInfo(MCommonConstants.FORM_CODE_CHEQUE, out FormLetterFinanceInfo);

                if (FormLetterFinanceInfo == null)
                {
                    return;
                }

                paymentNumberList.Sort();
                PrintChequeTemplater(paymentNumberList, FMainDS.AApPayment[0].LedgerNumber, FormLetterFinanceInfo);
            }
        }

        /// <summary>
        /// This is called from the 'Reprint' button on the Payment screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReprintRemittanceAdvice(object sender, EventArgs e)
        {
            // This code uses HTML Letters
            //TFrmAP_RemittanceAdvice PreviewFrame = new TFrmAP_RemittanceAdvice(this);

            //PreviewFrame.PrintRemittanceAdvice(FSelectedDocumentRow.PaymentNumber, FMainDS.AApPayment[0].LedgerNumber);
            //PreviewFrame.ShowDialog();

            // This code uses Templater
            TFormLetterFinanceInfo FormLetterFinanceInfo;

            GetTemplaterFinanceInfo(MCommonConstants.FORM_CODE_REMITTANCE, out FormLetterFinanceInfo);

            if (FormLetterFinanceInfo == null)
            {
                return;
            }

            List <int>paymentNumberList = new List <int>();
            paymentNumberList.Add(FSelectedDocumentRow.PaymentNumber);
            PrintRemittanceAdviceTemplater(paymentNumberList,
                FMainDS.AApPayment[0].LedgerNumber,
                FormLetterFinanceInfo);
        }

        /// <summary>
        /// This is called from the 'Reprint' button on the Payment screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReprintCheque(object sender, EventArgs e)
        {
            // This code uses Templater
            TFormLetterFinanceInfo FormLetterFinanceInfo;

            GetTemplaterFinanceInfo(MCommonConstants.FORM_CODE_CHEQUE, out FormLetterFinanceInfo);

            if (FormLetterFinanceInfo == null)
            {
                return;
            }

            List <int>paymentNumberList = new List <int>();
            paymentNumberList.Add(FSelectedDocumentRow.PaymentNumber);
            PrintChequeTemplater(paymentNumberList, FMainDS.AApPayment[0].LedgerNumber, FormLetterFinanceInfo);
        }

        private void MakePayment(object sender, EventArgs e)
        {
            if (FSelectedDocumentRow == null)
            {
                return;
            }

            // Update the Row data from the controls
            FSelectedDocumentRow.Amount = txtAmountToPay.NumberValueDecimal.Value;
            GetPaymentDetailsFromControlsManual();

            AccountsPayableTDSAApPaymentTable AApPayment = FMainDS.AApPayment;

            int paymentsToPay = AApPayment.Rows.Count;
            int invoicesToPay = 0;
            int invoicesToPartPay = 0;
            int remittancesToPrint = 0;
            int chequesToPrint = 0;
            decimal totalToPay = 0.0m;

            // Do some validation
            foreach (AccountsPayableTDSAApPaymentRow PaymentRow in AApPayment.Rows)
            {
                // Got a valid bank account?
                if (PaymentRow.BankAccount == "")
                {
                    MessageBox.Show(
                        string.Format(Catalog.GetString("Error: no Bank account was specified for the payment to {0}."), PaymentRow.SupplierName),
                        AP_PAYMENT_MSG_TITLE);
                    return;
                }

                //
                // I want to check whether the user is paying more than the due amount on any of these payments...
                //
                FMainDS.AApDocumentPayment.DefaultView.RowFilter = String.Format("{0}={1}",
                    AApDocumentPaymentTable.GetPaymentNumberDBName(), PaymentRow.PaymentNumber);

                remittancesToPrint += PaymentRow.PrintRemittanceAdvice ? 1 : 0;
                chequesToPrint += PaymentRow.PrintCheque ? 1 : 0;
                invoicesToPay += FMainDS.AApDocumentPayment.DefaultView.Count;

                foreach (DataRowView rv in FMainDS.AApDocumentPayment.DefaultView)
                {
                    AccountsPayableTDSAApDocumentPaymentRow DocPaymentRow = (AccountsPayableTDSAApDocumentPaymentRow)rv.Row;
                    Boolean overPayment = (DocPaymentRow.DocType == "INVOICE") ?
                                          (DocPaymentRow.Amount > DocPaymentRow.InvoiceTotal) : (DocPaymentRow.Amount < DocPaymentRow.InvoiceTotal);

                    if (overPayment)
                    {
                        String strMessage =
                            String.Format(Catalog.GetString(
                                    "Payment of {0} {1} to {2}: Payment cannot be more than the due amount."),
                                StringHelper.FormatUsingCurrencyCode(DocPaymentRow.Amount, PaymentRow.CurrencyCode),
                                PaymentRow.CurrencyCode, PaymentRow.SupplierName);

                        System.Windows.Forms.MessageBox.Show(strMessage, AP_PAYMENT_MSG_TITLE);
                        return;
                    }

                    if (DocPaymentRow.Amount == 0.0m)
                    {
                        MessageBox.Show(
                            string.Format(Catalog.GetString("Error: there is nothing to pay on invoice {0} for the payment to {1}."),
                                DocPaymentRow.DocumentCode,
                                PaymentRow.SupplierName),
                            AP_PAYMENT_MSG_TITLE);
                        return;
                    }

                    if (DocPaymentRow.Amount < DocPaymentRow.InvoiceTotal)
                    {
                        invoicesToPartPay++;
                    }

                    totalToPay += (DocPaymentRow.Amount / PaymentRow.ExchangeRateToBase);
                }

                // Got the necessary cheque information
                if (PaymentRow.PrintCheque)
                {
                    if (PaymentRow.ChequeNumber == 0)
                    {
                        MessageBox.Show(string.Format(
                                Catalog.GetString("Please enter a cheque number for the cheque to supplier {0}"), PaymentRow.SupplierName),
                            AP_PAYMENT_MSG_TITLE,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (PaymentRow.ChequeAmountInWords.Trim().Length == 0)
                    {
                        MessageBox.Show(string.Format(
                                Catalog.GetString("Please enter an amount in words to be printed on the cheque to {0} (corresponding to {1} {2})"),
                                PaymentRow.SupplierName,
                                PaymentRow.Amount, PaymentRow.CurrencyCode),
                            AP_PAYMENT_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            TDlgGLEnterDateEffective dateEffectiveDialog = new TDlgGLEnterDateEffective(
                FMainDS.AApDocument[0].LedgerNumber,
                Catalog.GetString("Select payment date"),
                Catalog.GetString("The date effective for the payment") + ":");

            if (dateEffectiveDialog.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show(Catalog.GetString(
                        "No payments have been made."), AP_PAYMENT_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DateTime PaymentDate = dateEffectiveDialog.SelectedDate;

            string msg = string.Format(Catalog.GetPluralString(
                    "You are about to make {0} payment to the listed supplier", "You are about to make payments to all {0} listed suppliers",
                    paymentsToPay), paymentsToPay);
            msg += string.Format(Catalog.GetString(
                    "{0}-  Invoices to pay: {1}"),
                Environment.NewLine, invoicesToPay);
            msg += string.Format(Catalog.GetString(
                    "{0}     of which Paid in Full: {1}{0}     Part Paid: {2}"),
                Environment.NewLine, invoicesToPay - invoicesToPartPay, invoicesToPartPay);
            msg += string.Format(Catalog.GetString(
                    "{0}-  Remittance Advice Notes to print: {1}{0}-  Cheques to print: {2}"),
                Environment.NewLine, remittancesToPrint, chequesToPrint);

            string totalToPayString;

            if (totalToPay > 0.00m)
            {
                totalToPayString = totalToPay.ToString("F" + txtBaseAmount.DecimalPlaces.ToString()) + " " + txtBaseAmount.CurrencyCode;
            }
            else
            {
                totalToPayString = "(" + (-totalToPay).ToString("F" + txtBaseAmount.DecimalPlaces.ToString()) + ") " + txtBaseAmount.CurrencyCode;
            }

            msg += string.Format(Catalog.GetString("{0}The total of all payments is {1}"), Environment.NewLine, totalToPayString);
            msg += string.Format(Catalog.GetPluralString(
                    "{0}The payment will be dated {1}{0}{0}", "{0}All payments will be dated {1}{0}{0}", paymentsToPay),
                Environment.NewLine, PaymentDate.ToLongDateString());
            msg += Catalog.GetString("Are you ready to proceed?");

            if (MessageBox.Show(msg, AP_PAYMENT_MSG_TITLE,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            TVerificationResultCollection Verifications;

            this.Cursor = Cursors.WaitCursor;
            Int32 glBatchNumber;
            AccountsPayableTDSAApPaymentTable newPayments;

            if (!TRemote.MFinance.AP.WebConnectors.PostAPPayments(
                    ref FMainDS,
                    PaymentDate,
                    out glBatchNumber,
                    out newPayments,
                    out Verifications))
            {
                this.Cursor = Cursors.Default;
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
                this.Cursor = Cursors.Default;
                //
                // The GL Posting Register must be printed.
                TFrmBatchPostingRegister ReportGui = new TFrmBatchPostingRegister(null);
                ReportGui.PrintReportNoUi(FLedgerNumber, glBatchNumber);

                // For printing stuff about payments we need our newPayments because those rows have the new Payment Numbers in place of -1, -2 etc.
                int minNewPaymentNumber;
                int maxNewPaymentNumber;

                if (GetPaymentNumbersAfterPosting(newPayments, out minNewPaymentNumber, out maxNewPaymentNumber))
                {
                    PrintPaymentReport(minNewPaymentNumber, maxNewPaymentNumber);
                }

                //PrintRemittanceAdvice(newPayments);                // Use this for HTML printing
                PrintRemittanceAdviceTemplater(newPayments);       // Use this for Templater printing
                PrintChequeTemplater(newPayments);                 // Use this for Templater printing

                // After the payment(s), one or more documents may have changed.
                TFormsMessage broadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcAPTransactionChanged);
                broadcastMessage.SetMessageDataAPTransaction(String.Empty);
                TFormsList.GFormsList.BroadcastFormMessage(broadcastMessage);

                Close();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APaymentNumber"></param>
        public void ReloadPayment(Int32 ALedgerNumber, Int32 APaymentNumber)
        {
            FMainDS = TRemote.MFinance.AP.WebConnectors.LoadAPPayment(ALedgerNumber, APaymentNumber);
            FLedgerNumber = FMainDS.AApPayment[0].LedgerNumber;
            ALedgerTable Tbl = TRemote.MFinance.AP.WebConnectors.GetLedgerInfo(FLedgerNumber);
            FLedgerRow = Tbl[0];
            ShowData(FMainDS.AApSupplier[0]);
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

            TempDS.AApDocument.DefaultView.Sort = AApDocumentTable.GetApDocumentIdDBName();

            //
            // First I'll check that the amounts add up:
            //
            Decimal PaidDocumentsTotal = 0.0m;

            foreach (AApDocumentPaymentRow PaymentRow in TempDS.AApDocumentPayment.Rows)
            {
                Int32 DocIdx = TempDS.AApDocument.DefaultView.Find(PaymentRow.ApDocumentId);
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
                MessageBox.Show(ErrorMsg, Catalog.GetString("Reverse Payment"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Find out if this payment was already reversed,
            // because if it was, perhaps the user doesn't really want to
            // reverse it again?
            this.Cursor = Cursors.WaitCursor;

            if (TRemote.MFinance.AP.WebConnectors.WasThisPaymentReversed(ALedgerNumber, APaymentNumber))
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(Catalog.GetString("Cannot reverse Payment - there is already a matching reverse transaction."),
                    Catalog.GetString("Reverse Payment"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Cursor = Cursors.Default;

            //
            // Ask the user to confirm reversal of this payment
            //
            String PaymentMsg = Catalog.GetString("Do you want to reverse this payment?");

            PaymentMsg += ("\r\n" + String.Format("Payment made {0} to {1}\r\n\r\nRelated invoices:",
                               TDate.DateTimeToLongDateString2(TempDS.AApPayment[0].PaymentDate.Value), TempDS.PPartner[0].PartnerShortName));

            foreach (AApDocumentPaymentRow PaymentRow in TempDS.AApDocumentPayment.Rows)
            {
                Int32 DocIdx = TempDS.AApDocument.DefaultView.Find(PaymentRow.ApDocumentId);
                AApDocumentRow DocumentRow = TempDS.AApDocument[DocIdx];
                PaymentMsg += ("\r\n" + String.Format("     {2} ({3})  {0:n2} {1}",
                                   DocumentRow.TotalAmount, TempDS.AApSupplier[0].CurrencyCode, DocumentRow.DocumentCode, DocumentRow.Reference));
            }

            PaymentMsg += ("\r\n\r\n" + String.Format("Total payment {0:n2} {1}", TempDS.AApPayment[0].Amount, TempDS.AApSupplier[0].CurrencyCode));
            DialogResult YesNo = MessageBox.Show(PaymentMsg, Catalog.GetString("Reverse Payment"), MessageBoxButtons.YesNo);

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
                MessageBox.Show(Catalog.GetString("Reversal was cancelled."), Catalog.GetString("Reverse Payment"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime PostingDate = dateEffectiveDialog.SelectedDate;
            TVerificationResultCollection Verifications;

            this.Cursor = Cursors.WaitCursor;
            List <Int32>glBatchNumbers;

            if (TRemote.MFinance.AP.WebConnectors.ReversePayment(ALedgerNumber, APaymentNumber, PostingDate,
                    out glBatchNumbers,
                    out Verifications))
            {
                this.Cursor = Cursors.Default;
                // The GL Posting Register must be printed.
                TFrmBatchPostingRegister ReportGui = new TFrmBatchPostingRegister(null);

                foreach (Int32 glBatchNumber in glBatchNumbers)
                {
                    ReportGui.PrintReportNoUi(ALedgerNumber, glBatchNumber);
                }

                MessageBox.Show(Catalog.GetString("The AP payment has been reversed."), Catalog.GetString("Reverse Payment"));
                TFormsMessage broadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcAPTransactionChanged);
                broadcastMessage.SetMessageDataAPTransaction(String.Empty);
                TFormsList.GFormsList.BroadcastFormMessage(broadcastMessage);
            }
            else
            {
                this.Cursor = Cursors.Default;
                string ErrorMessages = String.Empty;

                foreach (TVerificationResult verif in Verifications)
                {
                    ErrorMessages += "[" + verif.ResultContext + "] " +
                                     verif.ResultTextCaption + ": " +
                                     verif.ResultText + Environment.NewLine;
                }

                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Reverse Payment Failed"));
            }
        }

        private void OnChangePaymentType(object sender, EventArgs e)
        {
            string paymentType = cmbPaymentType.GetSelectedString();
            bool isCheque = (string.Compare(paymentType, "Cheque", true) == 0);

            txtChequeNumber.Enabled = isCheque;
            txtAmountInWords.Enabled = isCheque;
            chkPrintCheque.Enabled = isCheque;
            chkPrintCheque.Checked = isCheque;

            if (FSelectedPaymentRow != null)
            {
                // Change the text in the supplier list
                FSelectedPaymentRow.ListLabel = FSelectedPaymentRow.ListLabel.Substring(0, FSelectedPaymentRow.ListLabel.IndexOf('(')) +
                                                "(" + paymentType + ")";
            }
        }

        private void Invoice_DoubleClicked(object sender, EventArgs e)
        {
            DataRowView[] SelectedGridRow = grdDocuments.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length > 0)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    AccountsPayableTDSAApDocumentPaymentRow row = (AccountsPayableTDSAApDocumentPaymentRow)SelectedGridRow[0].Row;
                    TFrmAPEditDocument frm = new TFrmAPEditDocument(FPetraUtilsObject.GetCallerForm());

                    if (frm.LoadAApDocument(FLedgerNumber, Convert.ToInt32(row.ApDocumentId)))
                    {
                        frm.Show();
                    }
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void GetTemplaterFinanceInfo(string AFormCode, out TFormLetterFinanceInfo AFormLetterFinanceInfo)
        {
            AFormLetterFinanceInfo = null;

            // not implemented in Open Source OpenPetra
            // TFrmFormSelectionDialog formDialog = new TFrmFormSelectionDialog(this.FindForm());
            return;
        }

        private Boolean CreateRemittanceAdviceFormData(TFormLetterFinanceInfo AFormLetterFinanceInfo,
            List <int>APaymentNumberList,
            int ALedgerNumber,
            ref Boolean ACallFinished)
        {
            bool formDataCreated = TRemote.MFinance.AP.WebConnectors.CreateRemittanceAdviceFormData(
                AFormLetterFinanceInfo, APaymentNumberList, ALedgerNumber, out FFormDataList);

            ACallFinished = true;
            return formDataCreated;
        }

        private void PrintRemittanceAdviceTemplater(List <int>APaymentNumberList, int ALedgerNumber,
            TFormLetterFinanceInfo AFormLetterFinanceInfo)
        {
            Boolean ThreadFinished = false;
            Boolean FormLetterDataCreated = false;

            // create form letter data in separate thread. This will fill FFormDataList
            Thread t = new Thread(() => FormLetterDataCreated = CreateRemittanceAdviceFormData(
                    AFormLetterFinanceInfo, APaymentNumberList, ALedgerNumber, ref ThreadFinished));

            using (TProgressDialog dialog = new TProgressDialog(t))
            {
                dialog.ShowDialog();
            }

            // wait here until Thread is really finished
            while (!ThreadFinished)
            {
                Thread.Sleep(50);
            }

            if (FormLetterDataCreated)
            {
                if ((FFormDataList == null)
                    || (FFormDataList.Count == 0))
                {
                    MessageBox.Show(Catalog.GetString("Failed to get data from server to print the Remittance Advice"));
                    return;
                }
            }

            // not implemented in OpenSource OpenPetra
            // string targetFolder = TTemplaterAccess.GetFormLetterBaseDirectory(TModule.mFinance);
        }

        private Boolean CreateChequeFormData(TFormLetterFinanceInfo AFormLetterFinanceInfo,
            List <int>APaymentNumberList,
            int ALedgerNumber,
            ref Boolean ACallFinished)
        {
            bool formDataCreated = TRemote.MFinance.AP.WebConnectors.CreateChequeFormData(AFormLetterFinanceInfo,
                APaymentNumberList, ALedgerNumber, out FFormDataList);

            ACallFinished = true;
            return formDataCreated;
        }

        private void PrintChequeTemplater(List <int>APaymentNumberList,
            int ALedgerNumber,
            TFormLetterFinanceInfo AFormLetterFinanceInfo)
        {
            Boolean ThreadFinished = false;
            Boolean FormLetterDataCreated = false;

            // create form letter data in separate thread. This will fill FFormDataList
            Thread t = new Thread(() => FormLetterDataCreated = CreateChequeFormData(AFormLetterFinanceInfo,
                    APaymentNumberList, ALedgerNumber, ref ThreadFinished));

            using (TProgressDialog dialog = new TProgressDialog(t))
            {
                dialog.ShowDialog();
            }

            // wait here until Thread is really finished
            while (!ThreadFinished)
            {
                Thread.Sleep(50);
            }

            if (FormLetterDataCreated)
            {
                if ((FFormDataList == null)
                    || (FFormDataList.Count == 0))
                {
                    MessageBox.Show(Catalog.GetString("Failed to get data from server to print the Cheque"));
                    return;
                }
            }

            // not implemented in OpenSource OpenPetra
            // string targetFolder = TTemplaterAccess.GetFormLetterBaseDirectory(TModule.mFinance);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void chkPrintCheque_Clicked(object sender, EventArgs e)
        {
            txtAmountInWords.Enabled = chkPrintCheque.Checked;
            txtChequeNumber.Enabled = chkPrintCheque.Checked;
        }
    }
}
