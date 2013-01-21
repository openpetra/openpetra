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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance;

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
            string msg = string.Empty;

            //msg = "You can either consolidate all of your budgets";
            //msg += " or just those that have changed since the last consolidation." + "\n\r\n\r";
            msg += "Do you want to consolidate all of your budgets?";

            bool ConsolidateAll =
                (MessageBox.Show(msg, "Consolidate Budgets", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2,
                     MessageBoxOptions.DefaultDesktopOnly, false) == DialogResult.Yes);

            //TODO: call code on the server. To be completed with Timo.
            TVerificationResultCollection AVerificationResult = null;

            try
            {
                if (ConsolidateAll)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    TRemote.MFinance.Budget.WebConnectors.LoadBudgetForConsolidate(ALedgerNumber);

                    TRemote.MFinance.Budget.WebConnectors.ConsolidateBudgets(ALedgerNumber, true, out AVerificationResult);

                    MessageBox.Show("Budget Consolidation Complete.", "Consolidate Budgets");

                    Cursor.Current = Cursors.Default;
                }
            }
            catch (InvalidOperationException ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            catch (Exception)
            {
                Cursor.Current = Cursors.Default;
                throw;
            }
        }
    }
}