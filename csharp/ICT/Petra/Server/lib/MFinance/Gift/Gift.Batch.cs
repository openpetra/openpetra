//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Server.MFinance.Gift
{
    ///<summary>
    /// Some methods for creating gift batches
    ///</summary>
    public class TGiftBatchFunctions
    {
        /// <summary>
        /// create a new batch with a consecutive batch number in the ledger
        /// for call inside a server function
        /// for performance reasons submitting (save the data in the database) is done later (not here)
        /// </summary>
        /// <param name="MainDS"></param>
        /// <param name="Transaction"></param>
        /// <param name="LedgerTable"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADateEffective"></param>
        /// <param name="AForceEffectiveDateToFit"></param>
        /// <returns>the new gift batch row</returns>
        public static AGiftBatchRow CreateANewGiftBatchRow(ref GiftBatchTDS MainDS,
            ref TDBTransaction Transaction,
            ref ALedgerTable LedgerTable,
            Int32 ALedgerNumber,
            DateTime ADateEffective,
            bool AForceEffectiveDateToFit = true)
        {
            AGiftBatchRow NewRow = MainDS.AGiftBatch.NewRowTyped(true);

            NewRow.LedgerNumber = ALedgerNumber;
            LedgerTable[0].LastGiftBatchNumber++;
            NewRow.BatchNumber = LedgerTable[0].LastGiftBatchNumber;
            Int32 BatchYear, BatchPeriod;
            // if DateEffective is outside the range of open periods, use the most fitting date
            TFinancialYear.GetLedgerDatePostingPeriod(ALedgerNumber,
                ref ADateEffective,
                out BatchYear,
                out BatchPeriod,
                Transaction,
                AForceEffectiveDateToFit);
            NewRow.BatchYear = BatchYear;
            NewRow.BatchPeriod = BatchPeriod;
            NewRow.GlEffectiveDate = ADateEffective;
            NewRow.ExchangeRateToBase = 1.0M;
            NewRow.BatchDescription = "PLEASE ENTER A DESCRIPTION";
            NewRow.BankAccountCode = TLedgerInfo.GetDefaultBankAccount(ALedgerNumber);
            NewRow.BankCostCentre = TLedgerInfo.GetStandardCostCentre(ALedgerNumber);
            NewRow.CurrencyCode = LedgerTable[0].BaseCurrency;
            MainDS.AGiftBatch.Rows.Add(NewRow);
            return NewRow;
        }

        /// <summary>
        /// create a new batch with a consecutive batch number in the ledger
        /// for call inside a server function
        /// for performance reasons submitting (save the data in the database) is done later (not here)
        /// </summary>
        /// <param name="MainDS"></param>
        /// <param name="Transaction"></param>
        /// <param name="LedgerTable"></param>
        /// <param name="ALedgerNumber"></param>
        /// <returns>the new gift batch row</returns>
        public static ARecurringGiftBatchRow CreateANewRecurringGiftBatchRow(ref GiftBatchTDS MainDS,
            ref TDBTransaction Transaction,
            ref ALedgerTable LedgerTable,
            Int32 ALedgerNumber)
        {
            GiftBatchTDS Temp = new GiftBatchTDS();

            ARecurringGiftBatchAccess.LoadViaALedger(Temp, LedgerTable[0].LedgerNumber, Transaction);

            DataView RecurringGiftBatchDV = new DataView(Temp.ARecurringGiftBatch);
            RecurringGiftBatchDV.RowFilter = string.Empty;
            RecurringGiftBatchDV.Sort = string.Format("{0} DESC",
                ARecurringGiftBatchTable.GetBatchNumberDBName());

            //Recurring batch numbers can be reused so check each time for current highest number
            if (RecurringGiftBatchDV.Count > 0)
            {
                LedgerTable[0].LastRecGiftBatchNumber = (int)(RecurringGiftBatchDV[0][ARecurringGiftBatchTable.GetBatchNumberDBName()]);
            }
            else
            {
                LedgerTable[0].LastRecGiftBatchNumber = 0;
            }

            ARecurringGiftBatchRow NewRow = MainDS.ARecurringGiftBatch.NewRowTyped(true);

            NewRow.LedgerNumber = ALedgerNumber;
            NewRow.BatchNumber = ++LedgerTable[0].LastRecGiftBatchNumber;
            NewRow.BatchDescription = Catalog.GetString("Please enter recurring batch description");
            NewRow.BankAccountCode = TLedgerInfo.GetDefaultBankAccount(ALedgerNumber);
            NewRow.BankCostCentre = TLedgerInfo.GetStandardCostCentre(ALedgerNumber);
            NewRow.CurrencyCode = LedgerTable[0].BaseCurrency;
            MainDS.ARecurringGiftBatch.Rows.Add(NewRow);
            return NewRow;
        }
    }
}