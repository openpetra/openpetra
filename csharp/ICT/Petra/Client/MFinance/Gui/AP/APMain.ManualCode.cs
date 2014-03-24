//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
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
using System;
using System.Windows.Forms;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.MFinance.Logic;


namespace Ict.Petra.Client.MFinance.Gui.AP
{
    public partial class TFrmAPMain
    {
        private IAPUIConnectorsFind FSupplierFindObject = null;
        private IAPUIConnectorsFind FInvoiceFindObject = null;

        private ALedgerRow FLedgerInfo;

        private Int32 FLedgerNumber;
        private String FInitialTab = "Suppliers";

        // Flags to indicate if the data has changed
        private Boolean FIsSupplierDataChanged = true;      // Initially we don't have any!
        private Boolean FIsInvoiceDataChanged = true;

        /// <summary>
        /// Static method to get a supplier from a partner key.  Used by the AP sub=system.
        /// </summary>
        /// <param name="Tbl"></param>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        public static AApSupplierRow GetSupplier(AApSupplierTable Tbl, Int64 APartnerKey)
        {
            Tbl.DefaultView.Sort = "p_partner_key_n";

            int indexSupplier = Tbl.DefaultView.Find(APartnerKey);

            if (indexSupplier == -1)
            {
                return null;
            }

            return Tbl[indexSupplier];
        }

        /// <summary>
        /// AP is opened in this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                FSupplierFindObject = TRemote.MFinance.AP.UIConnectors.Find();
                // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
                TEnsureKeepAlive.Register(FSupplierFindObject);

                FInvoiceFindObject = TRemote.MFinance.AP.UIConnectors.Find();
                // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
                TEnsureKeepAlive.Register(FInvoiceFindObject);

                ALedgerTable Tbl = FSupplierFindObject.GetLedgerInfo(FLedgerNumber);
                FLedgerInfo = Tbl[0];

                this.Text += " - " + TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
            }
            get
            {
                return FLedgerNumber;
            }
        }

        /// <summary>
        /// Set the initial tab that is shown when the screen loads.  Valid values are 'Suppliers' or 'Invoices'
        /// </summary>
        public String InitialTab
        {
            set
            {
                if (value == "Suppliers" || value == "Invoices")
                {
                    FInitialTab = value;
                }
                else
                {
                    throw new ArgumentException("The value of InitialTab must be either 'Suppliers' or 'Invoices'");
                }
            }
        }

        /// <summary>
        /// Gets the Find object.  Use this from the user control
        /// </summary>
        public IAPUIConnectorsFind SupplierFindObject
        {
            get
            {
                return FSupplierFindObject;
            }
        }

        /// <summary>
        /// Gets the Find object.  Use this from the user control
        /// </summary>
        public IAPUIConnectorsFind InvoiceFindObject
        {
            get
            {
                return FInvoiceFindObject;
            }
        }

        /// <summary>
        /// Gets the currency for the screen ledger
        /// </summary>
        public String LedgerCurrency
        {
            get
            {
                return FLedgerInfo.BaseCurrency;
            }
        }

        /// <summary>
        /// Gets/sets whether there has been a change to the supplier data that would require a re-load
        /// </summary>
        public Boolean IsSupplierDataChanged
        {
            get
            {
                return FIsSupplierDataChanged;
            }

            set
            {
                FIsSupplierDataChanged = value;
            }
        }

        /// <summary>
        /// Gets/sets whether there has been a change to the invoice data that would require a re-load
        /// </summary>
        public Boolean IsInvoiceDataChanged
        {
            get
            {
                return FIsInvoiceDataChanged;
            }

            set
            {
                FIsInvoiceDataChanged = value;
            }
        }

        private void InitializeManualCode()
        {
        }

        private void RunOnceOnActivationManual()
        {
            ucoSupplierResult.MainForm = this;
            ucoInvoiceResult.MainForm = this;

            // See if we were launched with an initial tab set??
            if (FInitialTab == "Invoices")
            {
                tabSearchResult.SelectedTab = tpgOutstandingInvoices;
            }
            
            TabChange(null, null);
        }

        private void mniFilterFind_Click(object sender, EventArgs e)
        {
            if (tabSearchResult.SelectedTab == tpgSuppliers)
            {
                ucoSupplierResult.MniFilterFind_Click(sender, e);
            }
            else
            {
                ucoInvoiceResult.MniFilterFind_Click(sender, e);
            }
        }

        private void SupplierTransactions(object sender, EventArgs e)
        {
            ucoSupplierResult.SupplierTransactions(sender, e);
        }

        private void ShowInvoice(object sender, EventArgs e)
        {
            ucoInvoiceResult.ShowInvoice(sender, e);
        }

        private void NewSupplier(object sender, EventArgs e)
        {
            ucoSupplierResult.NewSupplier(sender, e);
        }

        private void EditSupplier(object sender, EventArgs e)
        {
            ucoSupplierResult.EditSupplier(sender, e);
        }

        private void CreateInvoice(object sender, EventArgs e)
        {
            ucoSupplierResult.CreateInvoice(sender, e);
        }

        private void CreateCreditNote(object sender, EventArgs e)
        {
            ucoSupplierResult.CreateCreditNote(sender, e);
        }

        //private AccountsPayableTDS LoadTaggedDocuments()
        //{
        //    return ucoInvoiceResult.LoadTaggedDocuments();
        //}

        private void DeleteAllTagged(object sender, EventArgs e)
        {
            ucoInvoiceResult.DeleteAllTagged(sender, e);
        }

        private void OpenAllTagged(object sender, EventArgs e)
        {
            ucoInvoiceResult.OpenAllTagged(sender, e);
        }

        private void ApproveAllTagged(object sender, EventArgs e)
        {
            throw new NotImplementedException("Sorry - not implemented yet");
        }

        private void PostAllTagged(object sender, EventArgs e)
        {
            ucoInvoiceResult.PostAllTagged(sender, e);
        }

        private void ReverseAllTagged(object sender, EventArgs e)
        {
            ucoInvoiceResult.ReverseAllTagged(sender, e);
        }

        private void PayAllTagged(object sender, EventArgs e)
        {
            ucoInvoiceResult.PayAllTagged(sender, e);
        }

        private void TabChange(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (tabSearchResult.SelectedTab == tpgOutstandingInvoices)
            {
                // Invoice tab has been selected...
                mniInvoice.Visible = true;
                mniSupplier.Visible = false;

                tbbEditSupplier.Visible = false;
                tbbTransactions.Visible = false;
                tbbNewSupplier.Visible = false;
                tbbCreateInvoice.Visible = false;
                tbbCreateCreditNote.Visible = false;
                tbbSeparator0.Visible = false;
                tbbSeparator1.Visible = false;
                tbbOpenTagged.Visible = true;
                tbbPostTagged.Visible = true;
                tbbPayTagged.Visible = true;
                tbbApproveTagged.Visible = true;
                tbbReverseTagged.Visible = true;

                ucoInvoiceResult.LoadInvoices();
            }
            else
            {
                // Suppliers tab has been selected...
                mniSupplier.Visible = true;
                mniInvoice.Visible = false;

                tbbEditSupplier.Visible = true;
                tbbTransactions.Visible = true;
                tbbNewSupplier.Visible = true;
                tbbCreateInvoice.Visible = true;
                tbbCreateCreditNote.Visible = true;
                tbbSeparator0.Visible = true;
                tbbSeparator1.Visible = true;
                tbbOpenTagged.Visible = false;
                tbbPostTagged.Visible = false;
                tbbPayTagged.Visible = false;
                tbbApproveTagged.Visible = false;
                tbbReverseTagged.Visible = false;

                ucoSupplierResult.LoadSuppliers();
            }

            this.Cursor = Cursors.Default;
        }

        private void Form_Closed(object sender, EventArgs e)
        {
            if (FSupplierFindObject != null)
            {
                // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
                TEnsureKeepAlive.UnRegister(FSupplierFindObject);
                FSupplierFindObject = null;
            }

            if (FInvoiceFindObject != null)
            {
                // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
                TEnsureKeepAlive.UnRegister(FInvoiceFindObject);
                FInvoiceFindObject = null;
            }
        }
    }
}