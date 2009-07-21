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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using Ict.Common.Data;
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MFinance.AP.Data;

namespace Ict.Petra.Client.MFinance.Gui.AccountsPayable
{
    public partial class TFrmAccountsPayableSupplierTransactions
    {
        Int32 FLedgerNumber = -1;

        /// <summary>
        /// load the supplier, do the first search with the default search parameters
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APartnerKey"></param>
        public void LoadSupplier(Int32 ALedgerNumber, Int64 APartnerKey)
        {
            FLedgerNumber = ALedgerNumber;

            FMainDS.Merge(TRemote.MFinance.AccountsPayable.WebConnectors.FindAApDocument(
                    ALedgerNumber, APartnerKey,

                    // cmbStatus.GetSelectedString(),
                    "UNPOSTED",
                    cmbType.SelectedIndex == 1,
                    chkHideAgedTransactions.Checked));

            ShowData();
        }

        /// <summary>
        /// needed for generated code
        /// </summary>
        void ShowDataManual()
        {
            DataView myDataView = FMainDS.AApDocument.DefaultView;

            myDataView.AllowNew = false;
            grdResult.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            grdResult.AutoSizeCells();
        }

        private void OpenSelectedApDocument(System.Object sender, EventArgs args)
        {
            DataRowView[] SelectedGridRow = grdResult.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                TFrmAccountsPayableEditDocument frm = new TFrmAccountsPayableEditDocument(this.Handle);
                frm.LoadAApDocument(FLedgerNumber, Convert.ToInt32(SelectedGridRow[0][FMainDS.AApDocument.ColumnApNumber.ColumnName]));
                frm.Show();
            }
        }
    }
}