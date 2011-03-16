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
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Client.MFinance.Logic;

namespace Ict.Petra.Client.MFinance.Gui.Common
{
    /// <summary>
    /// Select the ledger to work with
    /// </summary>
    public partial class TDlgSelectLedger : Form
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TDlgSelectLedger()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.Text = Catalog.GetString("Select a ledger");
            #endregion
        }

        /// <summary>
        /// Show the dialog for selecting the ledger if there are more than one ledgers available;
        /// otherwise return the only ledger number or the default ledger, or -1;
        /// does check for user permissions
        /// </summary>
        /// <returns>-1 if no ledger was selected, otherwise the ledger number</returns>
        public static Int32 SelectLedger()
        {
            Int32 ledgerNumber = TLedgerSelection.DetermineDefaultLedger();

            if (ledgerNumber == -2)
            {
                // TODO show the dialog
                // TDlgSelectLedger ledgerSelection = new TDlgSelectLedger();
                // ledgerSelection.FillGrid(ledgerTable);
                // if (ledgerSelection.ShowDialog())
                // {
                //   return ledgerSelection.GetSelectedLedger();
                // }
            }

            return ledgerNumber;
        }
    }
}