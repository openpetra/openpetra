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
using Ict.Common.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance.AP.Data;

namespace Ict.Petra.Client.MFinance.Gui
{
    public partial class TFrmAccountsPayableEditDocument
    {
        AccountsPayableTDS FMainDS;

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitializeManualCode()
        {
            FMainDS = new AccountsPayableTDS();
        }

        /// <summary>
        /// create a new document for the given supplier
        /// </summary>
        /// <param name="ALedgerNumber">current ledger</param>
        /// <param name="APartnerKey">partner key of the supplier</param>
        /// <param name="ACreditNoteOrInvoice"></param>
        public void CreateNewDocument(Int32 ALedgerNumber, Int64 APartnerKey, bool ACreditNoteOrInvoice)
        {
            FPetraUtilsObject.SetChangedFlag();
            
            AApDocumentRow NewDocumentRow = FMainDS.AApDocument.NewRowTyped();
            NewDocumentRow.ApNumber = -1;
            NewDocumentRow.LedgerNumber = ALedgerNumber;
            NewDocumentRow.PartnerKey = APartnerKey;
            NewDocumentRow.CreditNoteFlag = ACreditNoteOrInvoice;
            NewDocumentRow.DateIssued = DateTime.Now;
            NewDocumentRow.DateEntered = DateTime.Now;
            
            // get the supplier defaults
            TTypedDataTable tempTable = new AApSupplierTable();;
            AApSupplierRow filterValues = ((AApSupplierTable)tempTable).NewRowTyped();
            filterValues.PartnerKey = APartnerKey;
            tempTable.Rows.Add(filterValues);
            if (TRemote.MCommon.DataReader.GetData(
                AApSupplierTable.GetTableDBName(), 
                tempTable,
                out tempTable) && tempTable.Rows.Count == 1)
            {
                FMainDS.AApSupplier.Merge(tempTable);
                AApSupplierRow Supplier = FMainDS.AApSupplier[0];
                if (!Supplier.IsDefaultCreditTermsNull())
                {
                    NewDocumentRow.CreditTerms = Supplier.DefaultCreditTerms;
                }
                if (!Supplier.IsDefaultDiscountDaysNull())
                {
                    NewDocumentRow.DiscountDays = Supplier.DefaultDiscountDays;
                }
                if (!Supplier.IsDefaultDiscountPercentageNull())
                {
                    NewDocumentRow.DiscountPercentage = Supplier.DefaultDiscountPercentage;
                }
                if (!Supplier.IsDefaultApAccountNull())
                {
                    NewDocumentRow.ApAccount = Supplier.DefaultApAccount;
                }
            }

            FMainDS.AApDocument.Rows.Add(NewDocumentRow);

            ShowData();
        }
       
        /// <summary>
        /// needed for interface
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            return false;
        }

        /// <summary>
        /// save the current document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FileSave(object sender, EventArgs e)
        {
            // TODO
        }

        /// <summary>
        /// displays the data from the local datatable
        /// </summary>
        private void ShowDataManual()
        {
        }

        /// <summary>
        /// get the data from the controls into the dataset
        /// </summary>
        private void GetDataFromControlsManual()
        {
        }

        private void UpdateCreditTerms(object sender, EventArgs e)
        {
            if (sender == dtpDateDue)
            {
                int diffDays = (dtpDateDue.Value - dtpDateIssued.Value).Days;

                if (diffDays < 0)
                {
                    diffDays = 0;
                    dtpDateDue.Value = dtpDateIssued.Value;
                }

                nudCreditTerms.Value = diffDays;
            }
            else if ((sender == dtpDateIssued) || (sender == nudCreditTerms))
            {
                dtpDateDue.Value = dtpDateIssued.Value.AddDays((double)nudCreditTerms.Value);
            }
        }
    }
}