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
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.AP.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.Interfaces.MFinance.AccountsPayable.WebConnectors;

namespace Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors
{
    ///<summary>
    /// This connector provides data for the finance Accounts Payable screens
    ///</summary>
    public class TTransactionWebConnector
    {
        /// <summary>
        /// Passes data as a Typed DataSet to the Transaction Edit Screen
        /// </summary>
        public static AccountsPayableTDS LoadAApDocument(Int32 ALedgerNumber, Int32 AAPNumber)
        {
            // create the DataSet that will later be passed to the Client
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            AApDocumentAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, AAPNumber, null);
            AApDocumentDetailAccess.LoadViaAApDocument(MainDS, ALedgerNumber, AAPNumber, null);
            AApSupplierAccess.LoadByPrimaryKey(MainDS, MainDS.AApDocument[0].PartnerKey, null);

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// create a new AP document (invoice or credit note) and fill with default values from the supplier
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APartnerKey">the supplier</param>
        /// <param name="ACreditNoteOrInvoice">true: credit note; false: invoice</param>
        /// <returns></returns>
        public static AccountsPayableTDS CreateAApDocument(Int32 ALedgerNumber, Int64 APartnerKey, bool ACreditNoteOrInvoice)
        {
            // create the DataSet that will later be passed to the Client
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            AApDocumentRow NewDocumentRow = MainDS.AApDocument.NewRowTyped();

            NewDocumentRow.ApNumber = -1; // ap number will be set in SubmitChanges
            NewDocumentRow.LedgerNumber = ALedgerNumber;
            NewDocumentRow.PartnerKey = APartnerKey;
            NewDocumentRow.CreditNoteFlag = ACreditNoteOrInvoice;
            NewDocumentRow.LastDetailNumber = -1;

            // get the supplier defaults
            AApSupplierTable tempTable;
            tempTable = AApSupplierAccess.LoadByPrimaryKey(APartnerKey, null);

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

        /// <summary>
        /// store the AP Document (and document details)
        ///
        /// All DataTables contained in the Typed DataSet are inspected for added,
        /// changed or deleted rows by submitting them to the DataStore.
        ///
        /// </summary>
        /// <param name="AInspectDS">Typed DataSet that needs to contain known DataTables</param>
        /// <param name="AVerificationResult">Null if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions)</param>
        /// <returns>true if all verifications are OK and all DB calls succeeded, false if
        /// any verification or DB call failed
        /// </returns>
        public static TSubmitChangesResult SaveAApDocument(ref AccountsPayableTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            AVerificationResult = null;

            if (AInspectDS != null)
            {
                AVerificationResult = new TVerificationResultCollection();
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    // set AP Number if it has not been set yet
                    if (AInspectDS.AApDocument[0].ApNumber == -1)
                    {
                        StringCollection fieldlist = new StringCollection();
                        ALedgerTable myLedgerTable;
                        fieldlist.Add(ALedgerTable.GetLastApInvNumberDBName());
                        myLedgerTable = ALedgerAccess.LoadByPrimaryKey(
                            AInspectDS.AApDocument[0].LedgerNumber,
                            fieldlist,
                            SubmitChangesTransaction);
                        myLedgerTable[0].LastApInvNumber++;
                        AInspectDS.AApDocument[0].ApNumber = myLedgerTable[0].LastApInvNumber;

                        foreach (AApDocumentDetailRow detailrow in AInspectDS.AApDocumentDetail.Rows)
                        {
                            detailrow.ApNumber = AInspectDS.AApDocument[0].ApNumber;
                        }

                        ALedgerAccess.SubmitChanges(myLedgerTable, SubmitChangesTransaction, out AVerificationResult);
                    }

                    if (AApDocumentAccess.SubmitChanges(AInspectDS.AApDocument, SubmitChangesTransaction,
                            out AVerificationResult))
                    {
                        if (AApDocumentDetailAccess.SubmitChanges(AInspectDS.AApDocumentDetail, SubmitChangesTransaction,
                                out AVerificationResult))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
                catch (Exception e)
                {
                    TLogging.Log("after submitchanges: exception " + e.Message);

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw new Exception(e.ToString() + " " + e.Message);
                }
            }

            return SubmissionResult;
        }

        /// <summary>
        /// create a new AP document detail for an existing AP document;
        /// attention: need to modify the LastDetailNumber on the client side!
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AApNumber"></param>
        /// <param name="AApSupplier_DefaultExpAccount"></param>
        /// <param name="AApSupplier_DefaultCostCentre"></param>
        /// <param name="AAmount">the amount that is still missing from the total amount of the invoice</param>
        /// <param name="ALastDetailNumber">AApDocument.LastDetailNumber</param>
        /// <returns>the new AApDocumentDetail row</returns>
        public static AccountsPayableTDS CreateAApDocumentDetail(Int32 ALedgerNumber,
            Int32 AApNumber,
            string AApSupplier_DefaultExpAccount,
            string AApSupplier_DefaultCostCentre,
            double AAmount,
            Int32 ALastDetailNumber)
        {
            // create the DataSet that will later be passed to the Client
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            AApDocumentDetailRow NewRow = MainDS.AApDocumentDetail.NewRowTyped();

            NewRow.ApNumber = AApNumber;
            NewRow.LedgerNumber = ALedgerNumber;
            NewRow.DetailNumber = ALastDetailNumber;
            NewRow.Amount = AAmount;
            NewRow.CostCentreCode = AApSupplier_DefaultCostCentre;
            NewRow.AccountCode = AApSupplier_DefaultExpAccount;

            MainDS.AApDocumentDetail.Rows.Add(NewRow);

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// Find AP Documents
        /// TODO: date
        /// </summary>
        public static AccountsPayableTDS FindAApDocument(Int32 ALedgerNumber, Int64 ASupplierKey,
            string ADocumentStatus,
            bool IsCreditNoteNotInvoice,
            bool AHideAgedTransactions)
        {
            // create the DataSet that will later be passed to the Client
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            AApSupplierAccess.LoadByPrimaryKey(MainDS, ASupplierKey, null);

            // TODO: filters for documents
            // TODO: filter by ledger number
            AApDocumentAccess.LoadViaAApSupplier(MainDS, ASupplierKey, null);

            return MainDS;
        }
    }
}