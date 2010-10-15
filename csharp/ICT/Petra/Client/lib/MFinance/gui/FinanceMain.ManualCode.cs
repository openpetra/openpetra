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
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Shared;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Gui.AP;
using Ict.Petra.Client.MFinance.Gui.Common;

namespace Ict.Petra.Client.MFinance.Gui
{
    /// <summary>
    /// the manually written part of TFrmFinanceMain
    /// </summary>
    public partial class TFrmFinanceMain
    {
        /// <summary>
        /// currently selected ledger
        /// </summary>
        private int FLedgerNumber = -1;

        /// <summary>
        /// called by constructor
        /// </summary>
        public void InitializeManualCode()
        {
            // does the user have access to Finance at all?
            if (!UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE1)
                && !UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE2)
                && !UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE3))
            {
                MessageBox.Show(String.Format(Catalog.GetString("You don't have enough permissions to access {0}."),
                        Catalog.GetString("the finance module")));
                Close();
            }

            // TODO: show dialog to select ledger, if there are more than one ledgers available
            // TODO: does the user have access to the selected ledger?
            // TODO: check user default for ledger number

            FLedgerNumber = TDlgSelectLedger.SelectLedger();

            if (FLedgerNumber == -1)
            {
                MessageBox.Show(String.Format(Catalog.GetString("You don't have enough permissions to access {0}."),
                        Catalog.GetString("any ledger")));
                Close();
            }

            // todo: load ledger details, display on the main finance screen, etc.
            // MessageBox.Show("selected ledger: " + FLedgerNumber.ToString());
        }
    }
}