/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;
using System.Collections.Generic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;

namespace Ict.Petra.Client.MFinance.Gui.AccountsPayable
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
            FMainDS.SupplierPayments.Clear();

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
                        AccountsPayableTDSSupplierPaymentsRow supplierPaymentsRow = null;

                        FMainDS.SupplierPayments.DefaultView.Sort = AccountsPayableTDSSupplierPaymentsTable.GetSupplierKeyDBName();
                        int indexSupplierPayments = FMainDS.SupplierPayments.DefaultView.Find(supplier.PartnerKey);

                        if (indexSupplierPayments != -1)
                        {
                            supplierPaymentsRow =
                                (AccountsPayableTDSSupplierPaymentsRow)FMainDS.SupplierPayments.DefaultView[indexSupplierPayments].Row;
                        }
                        else
                        {
                            supplierPaymentsRow = FMainDS.SupplierPayments.NewRowTyped();
                            supplierPaymentsRow.Id = FMainDS.SupplierPayments.Count;
                            supplierPaymentsRow.SupplierKey = supplier.PartnerKey;
                            supplierPaymentsRow.PaymentType = supplier.PaymentType;
                            supplierPaymentsRow.BankAccount = supplier.DefaultBankAccount;

                            TPartnerClass partnerClass;
                            string partnerShortName;
                            TRemote.MPartner.Partner.ServerLookups.GetPartnerShortName(
                                supplier.PartnerKey,
                                out partnerShortName,
                                out partnerClass);
                            supplierPaymentsRow.SupplierName = Ict.Petra.Shared.MPartner.Calculations.FormatShortName(partnerShortName,
                                eShortNameFormat.eReverseWithoutTitle);

                            supplierPaymentsRow.ListLabel = supplierPaymentsRow.SupplierName + " (" + supplierPaymentsRow.PaymentType + ")";

                            FMainDS.SupplierPayments.Rows.Add(supplierPaymentsRow);
                        }

                        if (supplierPaymentsRow.DocumentNumberCSV.Length > 0)
                        {
                            supplierPaymentsRow.DocumentNumberCSV += ",";
                        }

                        supplierPaymentsRow.DocumentNumberCSV += apnumber.ToString();

                        FMainDS.AApDocument.DefaultView.Sort = AApDocumentTable.GetApNumberDBName();

                        AccountsPayableTDSPaymentDetailsRow paymentdetails = FMainDS.PaymentDetails.NewRowTyped();
                        paymentdetails.ApNumber = apnumber;
                        paymentdetails.PayFullInvoice = true;

                        // outstanding amount (TODO: consider partial payments!)
                        paymentdetails.TotalAmountToPay = apdocument.TotalAmount;
                        paymentdetails.Amount = paymentdetails.TotalAmountToPay;

                        // TODO: discounts
                        paymentdetails.HasValidDiscount = false;
                        paymentdetails.DiscountPercentage = 0;
                        paymentdetails.UseDiscount = false;
                        FMainDS.PaymentDetails.Rows.Add(paymentdetails);
                    }
                }
            }

            TFinanceControls.InitialiseAccountList(ref cmbBankAccount, FMainDS.AApDocument[0].LedgerNumber, true, false, true, true);

            grdDetails.AddTextColumn("AP No", FMainDS.PaymentDetails.ColumnApNumber);

            // TODO grdDetails.AddTextColumn("Invoice No", );
            // TODO grdDetails.AddTextColumn("Type", ); // invoice or credit note
            grdDetails.AddCheckBoxColumn("Discount used", FMainDS.PaymentDetails.ColumnUseDiscount);
            grdDetails.AddTextColumn("Amount", FMainDS.PaymentDetails.ColumnAmount);
            FMainDS.PaymentDetails.DefaultView.AllowNew = false;
            FMainDS.PaymentDetails.DefaultView.AllowEdit = false;

            grdSuppliers.AddTextColumn("Supplier", FMainDS.SupplierPayments.ColumnListLabel);
            FMainDS.SupplierPayments.DefaultView.AllowNew = false;
            grdSuppliers.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.SupplierPayments.DefaultView);
            grdSuppliers.Refresh();
            grdSuppliers.Selection.SelectRow(1, true);
            FocusedRowChanged(null, null);
        }

        private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            DataRowView[] SelectedGridRow = grdSuppliers.SelectedDataRowsAsDataRowView;

            // TODO: store values in previously selected item???

            if (SelectedGridRow.Length >= 1)
            {
                AccountsPayableTDSSupplierPaymentsRow row = (AccountsPayableTDSSupplierPaymentsRow)SelectedGridRow[0].Row;

                AApSupplierRow supplier = GetSupplier(row.SupplierKey);

                txtCurrency.Text = supplier.CurrencyCode;
                cmbBankAccount.SetSelectedString(row.BankAccount);

                FMainDS.PaymentDetails.DefaultView.RowFilter = AccountsPayableTDSPaymentDetailsTable.GetApNumberDBName() +
                                                               " IN (" + row.DocumentNumberCSV + ")";

                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.PaymentDetails.DefaultView);
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
            // TODO create gl batches, and post them
        }
    }
}