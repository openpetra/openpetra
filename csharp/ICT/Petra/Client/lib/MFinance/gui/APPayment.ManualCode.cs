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
                        AccountsPayableTDSSupplierPaymentsRow supplierPaymentsRow = FMainDS.SupplierPayments.NewRowTyped();
                        supplierPaymentsRow.Id = FMainDS.SupplierPayments.Count;
                        supplierPaymentsRow.SupplierKey = supplier.PartnerKey;

                        TPartnerClass partnerClass;
                        string partnerShortName;
                        TRemote.MPartner.Partner.ServerLookups.GetPartnerShortName(
                            supplier.PartnerKey,
                            out partnerShortName,
                            out partnerClass);
                        supplierPaymentsRow.SupplierName = Ict.Petra.Shared.MPartner.Calculations.FormatShortName(partnerShortName,
                            eShortNameFormat.eReverseWithoutTitle);

                        if (supplierPaymentsRow.DocumentNumberCSV.Length > 0)
                        {
                            supplierPaymentsRow.DocumentNumberCSV += ",";
                        }

                        supplierPaymentsRow.DocumentNumberCSV += apnumber.ToString();
                        supplierPaymentsRow.PaymentType = supplier.PaymentType;
                        supplierPaymentsRow.ListLabel = supplierPaymentsRow.SupplierName + " (" + supplierPaymentsRow.PaymentType + ")";

                        FMainDS.SupplierPayments.Rows.Add(supplierPaymentsRow);
                    }
                }
            }

            grdSuppliers.Columns.Clear();
            grdSuppliers.AddTextColumn("Supplier", FMainDS.SupplierPayments.ColumnListLabel);
            grdSuppliers.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.SupplierPayments.DefaultView);
            grdSuppliers.Refresh();
        }

        private void MakePayment(object sender, EventArgs e)
        {
            // TODO create gl batches, and post them
        }

        private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            DataRowView[] SelectedGridRow = grdSuppliers.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                AccountsPayableTDSSupplierPaymentsRow row = (AccountsPayableTDSSupplierPaymentsRow)SelectedGridRow[0].Row;

                AApSupplierRow supplier = GetSupplier(row.SupplierKey);

                txtCurrency.Text = supplier.CurrencyCode;
            }
        }
    }
}