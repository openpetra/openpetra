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
using Ict.Common.Exceptions;
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
        /// <param name="AMainDS"></param>
        /// <param name="ATransaction"></param>
        /// <param name="ALedgerTbl"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADateEffective"></param>
        /// <param name="AForceEffectiveDateToFit"></param>
        /// <returns>the new gift batch row</returns>
        public static AGiftBatchRow CreateANewGiftBatchRow(ref GiftBatchTDS AMainDS,
            ref TDBTransaction ATransaction,
            ref ALedgerTable ALedgerTbl,
            Int32 ALedgerNumber,
            DateTime ADateEffective,
            bool AForceEffectiveDateToFit = true)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Gift Batch dataset is NULL!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ATransaction == null)
            {
                throw new EFinanceSystemDBTransactionNullException(String.Format(Catalog.GetString(
                            "Method:{0} - Database Transaction must not be NULL!"),
                        Utilities.GetMethodName(true)));
            }
            else if ((ALedgerTbl == null) || (ALedgerTbl.Count == 0))
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Method:{0} - The Ledger table is NULL or is empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Method:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

            AGiftBatchRow NewRow = null;

            try
            {
                NewRow = AMainDS.AGiftBatch.NewRowTyped(true);

                NewRow.LedgerNumber = ALedgerNumber;
                NewRow.BatchNumber = ++ALedgerTbl[0].LastGiftBatchNumber;
                Int32 BatchYear, BatchPeriod;
                // if DateEffective is outside the range of open periods, use the most fitting date
                TFinancialYear.GetLedgerDatePostingPeriod(ALedgerNumber,
                    ref ADateEffective,
                    out BatchYear,
                    out BatchPeriod,
                    ATransaction,
                    AForceEffectiveDateToFit);

                NewRow.BatchYear = BatchYear;
                NewRow.BatchPeriod = BatchPeriod;
                NewRow.GlEffectiveDate = ADateEffective;
                NewRow.ExchangeRateToBase = 1.0M;
                NewRow.BatchDescription = "PLEASE ENTER A DESCRIPTION";
                NewRow.BankAccountCode = TLedgerInfo.GetDefaultBankAccount(ALedgerNumber);
                NewRow.BankCostCentre = TLedgerInfo.GetStandardCostCentre(ALedgerNumber);
                NewRow.CurrencyCode = ALedgerTbl[0].BaseCurrency;
                AMainDS.AGiftBatch.Rows.Add(NewRow);
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }

            return NewRow;
        }

        /// <summary>
        /// create a new batch with a consecutive batch number in the ledger
        /// for call inside a server function
        /// for performance reasons submitting (save the data in the database) is done later (not here)
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ATransaction"></param>
        /// <param name="ALedgerTbl"></param>
        /// <param name="ALedgerNumber"></param>
        /// <returns>the new gift batch row</returns>
        public static ARecurringGiftBatchRow CreateANewRecurringGiftBatchRow(ref GiftBatchTDS AMainDS,
            ref TDBTransaction ATransaction,
            ref ALedgerTable ALedgerTbl,
            Int32 ALedgerNumber)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Gift Batch dataset is NULL!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ATransaction == null)
            {
                throw new EFinanceSystemDBTransactionNullException(String.Format(Catalog.GetString(
                            "Function:{0} - Database Transaction must not be NULL!"),
                        Utilities.GetMethodName(true)));
            }
            else if ((ALedgerTbl == null) || (ALedgerTbl.Count == 0))
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger table is NULL or is empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

            GiftBatchTDS Temp = new GiftBatchTDS();

            ARecurringGiftBatchAccess.LoadViaALedger(Temp, ALedgerNumber, ATransaction);

            DataView RecurringGiftBatchDV = new DataView(Temp.ARecurringGiftBatch);
            RecurringGiftBatchDV.RowFilter = string.Empty;
            RecurringGiftBatchDV.Sort = string.Format("{0} DESC",
                ARecurringGiftBatchTable.GetBatchNumberDBName());

            //Recurring batch numbers can be reused so check each time for current highest number
            if (RecurringGiftBatchDV.Count > 0)
            {
                ALedgerTbl[0].LastRecGiftBatchNumber = (int)(RecurringGiftBatchDV[0][ARecurringGiftBatchTable.GetBatchNumberDBName()]);
            }
            else
            {
                ALedgerTbl[0].LastRecGiftBatchNumber = 0;
            }

            ARecurringGiftBatchRow NewRow = AMainDS.ARecurringGiftBatch.NewRowTyped(true);

            NewRow.LedgerNumber = ALedgerNumber;
            NewRow.BatchNumber = ++ALedgerTbl[0].LastRecGiftBatchNumber;
            NewRow.BatchDescription = Catalog.GetString("Please enter recurring batch description");
            NewRow.BankAccountCode = TLedgerInfo.GetDefaultBankAccount(ALedgerNumber);
            NewRow.BankCostCentre = TLedgerInfo.GetStandardCostCentre(ALedgerNumber);
            NewRow.CurrencyCode = ALedgerTbl[0].BaseCurrency;
            AMainDS.ARecurringGiftBatch.Rows.Add(NewRow);
            return NewRow;
        }
    }
}