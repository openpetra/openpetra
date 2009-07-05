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
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.AP.Data.Access;
using Ict.Petra.Shared.Interfaces.MFinance.AccountsPayable.UIConnectors;

namespace Ict.Petra.Server.MFinance.AccountsPayable.UIConnectors
{
    ///<summary>
    /// This UIConnector provides data for the finance Accounts Payable screens
    ///
    /// UIConnector Objects are instantiated by the Client's User Interface via the
    /// Instantiator classes.
    /// They handle requests for data retrieval and saving of data (including data
    /// verification).
    ///
    /// Their role is to
    ///   - retrieve (and possibly aggregate) data using Business Objects,
    ///   - put this data into///one* DataSet that is passed to the Client and make
    ///     sure that no unnessary data is transferred to the Client,
    ///   - optionally provide functionality to retrieve additional, different data
    ///     if requested by the Client (for Client screens that load data initially
    ///     as well as later, eg. when a certain tab on the screen is clicked),
    ///   - save data using Business Objects.
    ///
    /// @Comment These Objects would usually not be instantiated by other Server
    ///          Objects, but only by the Client UI via the Instantiator classes.
    ///          However, Server Objects that derive from these objects and that
    ///          are also UIConnectors are feasible.
    ///</summary>
    public class TSupplierEditUIConnector : TConfigurableMBRObject, IAccountsPayableUIConnectorsSupplierEdit
    {
        Int64 FPartnerKey;
        AccountsPayableTDS FMainDS;

        /// <summary>
        /// Constructor which automatically loads data for the Partner.
        ///
        /// </summary>
        /// <param name="APartnerKey">PartnerKey for the Supplier to instantiate this object with
        /// </param>
        public TSupplierEditUIConnector(System.Int64 APartnerKey) : base()
        {
            FPartnerKey = APartnerKey;
        }

        /// <summary>
        /// constructor for new supplier
        /// </summary>
        public TSupplierEditUIConnector() : base()
        {
            FPartnerKey = -1;
        }

        private static string DATASETNAME = "AccountsPayable";

        /// <summary>
        /// Passes data as a Typed DataSet to the Supplier Edit Screen
        /// </summary>
        public AccountsPayableTDS GetData()
        {
            TDBTransaction ReadTransaction;

            // create the DataSet that will later be passed to the Client
            FMainDS = new AccountsPayableTDS(DATASETNAME);

            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.RepeatableRead, 5);
                try
                {
                    // Supplier
                    AApSupplierAccess.LoadByPrimaryKey(FMainDS, FPartnerKey, ReadTransaction);

                    if (FMainDS.AApSupplier.Rows.Count == 0)
                    {
                        // Supplier does not exist
                        throw new Exception("supplier does not exist");
                    }
                }
                catch (Exception Exp)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.Log("TSupplierEditUIConnector.LoadData exception: " + Exp.ToString(), TLoggingType.ToLogfile);
                    TLogging.Log(Exp.StackTrace, TLoggingType.ToLogfile);
                    throw Exp;
                }
            }
            finally
            {
                if (DBAccess.GDBAccessObj.Transaction != null)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }

            // Accept row changes here so that the Client gets 'unmodified' rows
            FMainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            FMainDS.RemoveEmptyTables();

            return FMainDS;
        }

        /// <summary>
        /// store the AP supplier
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
        public TSubmitChangesResult SubmitChanges(ref AccountsPayableTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TVerificationResultCollection SingleVerificationResultCollection;

            AVerificationResult = null;

            if (AInspectDS != null)
            {
                AVerificationResult = new TVerificationResultCollection();
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    AInspectDS.AcceptChanges();

                    if (AApSupplierAccess.SubmitChanges(AInspectDS.AApSupplier, SubmitChangesTransaction,
                            out SingleVerificationResultCollection))
                    {
                        SubmissionResult = TSubmitChangesResult.scrOK;
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

                    throw e;
                }
            }

            return SubmissionResult;
        }
    }
}