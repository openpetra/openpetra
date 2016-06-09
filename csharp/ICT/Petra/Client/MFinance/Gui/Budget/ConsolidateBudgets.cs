//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
//
// Copyright 2004-2012 by OM International
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
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;

namespace Ict.Petra.Client.MFinance.Gui.Budget
{
    /// <summary>
    /// Class for calling the Budget Consolidation code
    /// </summary>
    public static class TConsolidateBudgets
    {
        /// <summary>
        /// Call the code to consolidate all budgets
        /// </summary>
        /// <param name="AParentWindow"></param>
        /// <param name="ALedgerNumber"></param>
        public static void ConsolidateBudgets(Form AParentWindow, Int32 ALedgerNumber)         //, TVerificationResultCollection AVerificationResult = null)            /// <param name="AVerificationResult"></param>
        {
            string Msg = string.Empty;

            Msg = "You can either consolidate all of your budgets";
            Msg += " or just those that have changed since the last consolidation." + "\n\r\n\r";
            Msg += "Do you want to consolidate all of your budgets?";

            DialogResult DlgRes;

            DlgRes = MessageBox.Show(Msg,
                "Consolidate Budgets",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.DefaultDesktopOnly,
                false);

            if (DlgRes == DialogResult.Cancel)
            {
                return;
            }

            bool ConsolidateAll = (DlgRes == DialogResult.Yes);

            TFrmStatusDialog DlgStatus = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                DlgStatus = new TFrmStatusDialog(AParentWindow);

                DlgStatus.Show();
                DlgStatus.Heading = Catalog.GetString("Consolidating Budgets");

                DlgStatus.CurrentStatus = Catalog.GetString("Loading budget data...");
                TRemote.MFinance.Budget.WebConnectors.LoadBudgetForConsolidate(ALedgerNumber);

                DlgStatus.CurrentStatus = Catalog.GetString("Consolidating" + (ConsolidateAll ? " all " : " changed ") + "budget data...");
                TRemote.MFinance.Budget.WebConnectors.ConsolidateBudgets(ALedgerNumber, ConsolidateAll);

                DlgStatus.Close();
                DlgStatus = null;

                MessageBox.Show("Budget Consolidation Complete.", "Consolidate Budgets");
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                if (DlgStatus != null)
                {
                    DlgStatus.Close();
                    DlgStatus = null;
                }
            }
        }
    }
}