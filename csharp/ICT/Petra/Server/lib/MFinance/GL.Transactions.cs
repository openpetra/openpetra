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
using System.Data;
using System.Collections.Specialized;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.GL.Data.Access;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.Interfaces.MFinance.GL.WebConnectors;

namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
    ///<summary>
    /// This connector provides data for the finance GL screens
    ///</summary>
    public class TTransactionWebConnector
    {
        /// <summary>
        /// create a new batch and increase the last batch number of the ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APartnerKey">the supplier</param>
        /// <param name="ACreditNoteOrInvoice">true: credit note; false: invoice</param>
        /// <returns></returns>
        public static GLBatchTDS CreateNewABatch(Int32 ALedgerNumber)
        {
            // create the DataSet that will later be passed to the Client
            GLBatchTDS MainDS = new AccountsPayableTDS();

            ABatchRow NewDocumentRow = MainDS.ABatchRow.NewRowTyped();

            ABatchRow.ApNumber = -1; // ap number will be set in SubmitChanges
            NewDocumentRow.LedgerNumber = ALedgerNumber;
            NewDocumentRow.PartnerKey = APartnerKey;
            NewDocumentRow.CreditNoteFlag = ACreditNoteOrInvoice;
            NewDocumentRow.LastDetailNumber = -1;

            // get the supplier defaults
            AApSupplierTable tempTable;
            AApSupplierAccess.LoadByPrimaryKey(out tempTable, APartnerKey, null);

            if (tempTable.Rows.Count == 1)
            {
                MainDS.AApSupplier.Merge(tempTable);

                AApSupplierRow Supplier = MainDS.AApSupplier[0];

                if (!Supplier.IsDefaultCreditTermsNull())
                {
                    NewDocumentRow.CreditTerms = Supplier.DefaultCreditTerms;
                }

                if (!Supplier.IsDefaultDiscountDaysNull())
                {
                    NewDocumentRow.DiscountDays = Supplier.DefaultDiscountDays;
                }

                if (!Supplier.IsDefaultDiscountPercentageNull())
                {
                    NewDocumentRow.DiscountPercentage = Supplier.DefaultDiscountPercentage;
                }

                if (!Supplier.IsDefaultApAccountNull())
                {
                    NewDocumentRow.ApAccount = Supplier.DefaultApAccount;
                }
            }

            MainDS.AApDocument.Rows.Add(NewDocumentRow);

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }
    }
}