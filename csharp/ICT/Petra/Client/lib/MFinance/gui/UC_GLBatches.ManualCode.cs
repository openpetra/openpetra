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

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLBatches
    {
        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewABatch();

//            this.FMainDS.ABatch[
        }

        /// <summary>
        /// cancel a batch (there is no deletion of batches)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CancelRow(System.Object sender, EventArgs e)
        {
        }

        /// <summary>
        /// get the ledger information
        /// </summary>
        private void InitializeManualCode()
        {
        }

        /// <summary>
        /// assign a temporary (negative) batch number
        /// </summary>
        /// <param name="ANewRow"></param>
        private void NewRowManual(ref Ict.Petra.Shared.MFinance.Account.Data.ABatchRow ANewRow)
        {
            ANewRow.LedgerNumber = FMainDS.ALedger[0].LedgerNumber;

            if (FMainDS.ALedger[0].LastBatchNumber > 0)
            {
                FMainDS.ALedger[0].LastBatchNumber *= -1;
            }

            FMainDS.ALedger[0].LastBatchNumber--;

            ANewRow.BatchNumber = FMainDS.ALedger[0].LastBatchNumber;
        }
    }
}