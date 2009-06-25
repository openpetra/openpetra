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
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.MFinance.Gui
{
    public partial class TFrmAccountsPayableMain
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public void InitializeManualCode()
        {
        }

        /// <summary>
        /// open the transactions of the selected supplier
        /// </summary>
        public void SupplierTransactions(object sender, EventArgs e)
        {
            TFrmAccountsPayableSupplierTransactions frm = new TFrmAccountsPayableSupplierTransactions(this.Handle);

            // todo: frm.SupplierPartnerKey = currentRow.PartnerKey;
            frm.Show();
        }

        /// <summary>
        /// create a new supplier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewSupplier(object sender, EventArgs e)
        {
            Int64 PartnerKey = -1;
            String ResultStringLbl;
            TLocationPK ResultLocationPK;

            // the user has to select an existing partner to make that partner a supplier
            TPartnerFindScreenManager.OpenModalForm("ORGANISATION,FAMILY,CHURCH",
                out PartnerKey,
                out ResultStringLbl,
                out ResultLocationPK,
                this.Handle);

            TFrmAccountsPayableEditSupplier frm = new TFrmAccountsPayableEditSupplier(this.Handle);
            frm.CreateNewSupplier(PartnerKey);
            frm.Show();
        }

        /// <summary>
        /// edit an existing supplier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EditSupplier(object sender, EventArgs e)
        {
            TFrmAccountsPayableEditSupplier frm = new TFrmAccountsPayableEditSupplier(this.Handle);

            frm.Show();
        }
    }
}