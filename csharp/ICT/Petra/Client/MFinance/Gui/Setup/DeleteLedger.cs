//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Reflection;
using System.Threading;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    /// <summary>
    /// methods for deletion of a ledger
    /// </summary>
    public class TDeleteLedger
    {
        private static void ProcessDeletion(Form AMainWindow, Int32 ALedgerNumber, string ALedgerNameAndNumber)
        {
            TVerificationResultCollection VerificationResult;
            MethodInfo method;

            if (!TRemote.MFinance.Setup.WebConnectors.DeleteLedger(ALedgerNumber, out VerificationResult))
            {
                MessageBox.Show(
                    string.Format(Catalog.GetString("Deletion of Ledger '{0}' failed"), ALedgerNameAndNumber) + "\r\n\r\n" +
                    VerificationResult.BuildVerificationResultString(),
                    Catalog.GetString("Deletion failed"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(
                    string.Format(Catalog.GetString("Ledger '{0}' has been deleted"), ALedgerNameAndNumber),
                    Catalog.GetString("Deletion successful"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            method = AMainWindow.GetType().GetMethod("ShowCurrentLedgerInfoInStatusBar");

            if (method != null)
            {
                method.Invoke(AMainWindow, new object[] { });
            }
        }

        /// delete ledger
        public static void DeleteLedger(Form AMainWindow, Int32 ALedgerNumber)
        {
            string LedgerNameAndNumber = TFinanceControls.GetLedgerNumberAndName(ALedgerNumber);

            if (MessageBox.Show(Catalog.GetString("Please save a backup of your database first!!!") + Environment.NewLine +
                    string.Format(Catalog.GetString("Do you REALLY want to delete ledger '{0}'?"),
                        LedgerNameAndNumber),
                    Catalog.GetString("Delete Ledger"),
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3
                    ) == DialogResult.Yes)
            {
                // ledger cannot be deleted if there are any transactions existing for it
                if (TRemote.MFinance.Setup.WebConnectors.ContainsTransactions(ALedgerNumber))
                {
                    MessageBox.Show(
                        string.Format(Catalog.GetString("There are still transactions associated with Ledger '{0}'. \r\n\r\nNothing has been done."),
                            LedgerNameAndNumber),
                        Catalog.GetString("Deletion not possible"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    Thread t = new Thread(() => ProcessDeletion(AMainWindow, ALedgerNumber, LedgerNameAndNumber));

                    using (TProgressDialog dialog = new TProgressDialog(t))
                    {
                        dialog.AllowCancellation = false;
                        dialog.ShowDialog();
                    }

                    // reload list of Ledger names
                    TDataCache.TMFinance.RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList);

                    // reload navigation

                    // Setting the "CurrentLedger" to -1 isn't strictly needed, but it eradicates the Ledger
                    // we have presently deleted to make sure the Main Menu isn't working any further with a
                    // Ledger that doesn't exist anymore.
                    PropertyInfo CurrentLedgerProperty = AMainWindow.GetType().GetProperty("CurrentLedger");
                    CurrentLedgerProperty.SetValue(AMainWindow, -1, null);

                    MethodInfo method = AMainWindow.GetType().GetMethod("LoadNavigationUI");

                    if (method != null)
                    {
                        method.Invoke(AMainWindow, new object[] { false });
                        method = AMainWindow.GetType().GetMethod("SelectFinanceFolder");
                        method.Invoke(AMainWindow, new object[] { });
                    }
                }
            }
        }
    }
}