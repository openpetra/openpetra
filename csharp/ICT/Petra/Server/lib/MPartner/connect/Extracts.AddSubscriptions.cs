//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       petrih, timop
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
using System.Collections.Specialized;
using System.Data;
using System.Threading;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Shared.MCommon;

namespace Ict.Petra.Server.MPartner.Extracts.UIConnectors
{
    /// <summary>
    /// Extract Add Screen UIConnector working with a Typed DataSet
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
    ///
    /// </summary>
    public class TExtractsAddSubscriptionsUIConnector : TConfigurableMBRObject, IPartnerUIConnectorsExtractsAddSubscriptions
    {
        private const Int32 MAX_PERCENTAGE_CHECKS = 70;
        private System.Int32 FExtractID;
        private DataTable FSubmissionDT;
        private TAsynchronousExecutionProgress FAsyncExecProgress;
        private TVerificationResultCollection FVerificationResult;
        private TSubmitChangesResult FSubmitResult;
        private DataTable FResponseDT;
        private PSubscriptionTable FInspectDT;
        private Exception FSubmitException;

        /// <summary>
        /// todoComment
        /// </summary>
        public IAsynchronousExecutionProgress AsyncExecProgress
        {
            get
            {
                if (FSubmitException == null)
                {
                    if (FAsyncExecProgress.ProgressPercentage >= MAX_PERCENTAGE_CHECKS)
                    {
                        if ((FSubmissionDT.Rows.Count > 0) && (TTypedDataAccess.RowCount > 0))
                        {
                            FAsyncExecProgress.ProgressPercentage =
                                (Int16)(MAX_PERCENTAGE_CHECKS +
                                        Convert.ToInt16(((double)(TTypedDataAccess.RowCount) /
                                                         (double)(FSubmissionDT.Rows.Count)) * (100 - MAX_PERCENTAGE_CHECKS)));
                        }
                    }
                }
                else
                {
                    throw FSubmitException;
                }

                return (IAsynchronousExecutionProgress)TCreateRemotableObject.CreateRemotableObject(
                    typeof(IAsynchronousExecutionProgress),
                    typeof(TAsynchronousExecutionProgressRemote),
                    FAsyncExecProgress);
            }
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <returns>void</returns>
        public TExtractsAddSubscriptionsUIConnector(System.Int32 AExtractID) : base()
        {
            FExtractID = AExtractID;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AInspectDT"></param>
        public void SubmitChangesAsync(PSubscriptionTable AInspectDT)
        {
            Thread TheThread;

            // Cleanup (might be left in a certain state from a possible earlier call)
            FSubmitException = null;
            FSubmissionDT = null;
            FAsyncExecProgress = null;
            FVerificationResult = null;
            FResponseDT = null;
            FInspectDT = null;
            this.FAsyncExecProgress = new TAsynchronousExecutionProgress();
            this.FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_ReadyToStart;
            FInspectDT = AInspectDT;
            ThreadStart ThreadStartDelegate = new ThreadStart(SubmitChangesInternal);
            TheThread = new Thread(ThreadStartDelegate);
            TheThread.Start();
            TLogging.LogAtLevel(6, "TExtractsAddSubscriptionsUIConnector.SubmitChangesAsync thread started.");
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AResponseDT"></param>
        /// <param name="AVerificationResult"></param>
        /// <param name="ASubmitChangesResult"></param>
        public void SubmitChangesAsyncResult(out DataTable AResponseDT,
            out TVerificationResultCollection AVerificationResult,
            out TSubmitChangesResult ASubmitChangesResult)
        {
            if (FSubmitException == null)
            {
                AResponseDT = FResponseDT;
                AVerificationResult = FVerificationResult;
                ASubmitChangesResult = FSubmitResult;
            }
            else
            {
                throw FSubmitException;
            }
        }

        /// <summary>
        /// First loads the extractTable needed. Goes throught all the Partners in the extract,  If the partner already has the subscription, returns all those partners back to client.  If the partner doesn't have the subscription, saves this
        /// subscription to those partners.
        /// </summary>
        /// <returns>void</returns>
        private void SubmitChangesInternal()
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrOK;
            MExtractTable ExtractDT;
            PSubscriptionTable SubscriptionTable;

            PPartnerTable PartnerTable;
            Int32 RowCounter;
            Int32 PartnersInExtract;
            StringCollection RequiredColumns;
            StringCollection RequiredColumns2;

            if (FInspectDT != null)
            {
                // Initialisations
                FVerificationResult = new TVerificationResultCollection();
                ExtractDT = new MExtractTable();
                SubscriptionTable = new PSubscriptionTable();
                FSubmissionDT = SubscriptionTable.Clone();
                ((TTypedDataTable)FSubmissionDT).InitVars();
                PartnerTable = new PPartnerTable();
                RequiredColumns = new StringCollection();
                RequiredColumns.Add(MExtractTable.GetPartnerKeyDBName());
                RequiredColumns2 = new StringCollection();
                RequiredColumns2.Add(PPartnerTable.GetPartnerKeyDBName());
                RequiredColumns2.Add(PPartnerTable.GetPartnerShortNameDBName());
                RowCounter = 0;

                // Set up asynchronous execution
                FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_Executing;
                FAsyncExecProgress.ProgressInformation = "Checking Partners' Subscriptions...";
                try
                {
                    SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
//                  TLogging.LogAtLevel(7, "TExtractsAddSubscriptionsUIConnector.SubmitChangesInternal: loading Subscriptions for ExtractID " + FExtractID.ToString() + "...");
                    ExtractDT = MExtractAccess.LoadViaMExtractMaster(FExtractID, RequiredColumns, SubmitChangesTransaction);
                    PartnersInExtract = ExtractDT.Rows.Count;
//                  TLogging.LogAtLevel(7, "TExtractsAddSubscriptionsUIConnector.SubmitChangesInternal: ExtractID has " + PartnersInExtract.ToString() + " Partners.");

                    // Go throught all the Partners in the extract
                    foreach (MExtractRow ExtractRow in ExtractDT.Rows)
                    {
                        RowCounter = RowCounter + 1;

                        // Calculate how much Partners we have checked. Let all Partners be a maximum of 70%.
                        FAsyncExecProgress.ProgressPercentage =
                            Convert.ToInt16((((double)RowCounter / (double)PartnersInExtract) * 100) * (MAX_PERCENTAGE_CHECKS / 100.0));
                        TLogging.LogAtLevel(7, "TExtractsAddSubscriptionsUIConnector.SubmitChangesInternal: loadbyPrimaryKey");

                        SubscriptionTable = PSubscriptionAccess.LoadByPrimaryKey(
                            FInspectDT[0].PublicationCode,
                            ExtractRow.PartnerKey,
                            SubmitChangesTransaction);

                        // if the Partner does not yet have the subscription, add the subscription to this partner.
                        if (SubscriptionTable.Rows.Count == 0)
                        {
                            TLogging.LogAtLevel(
                                7,
                                "TExtractsAddSubscriptionsUIConnector.SubmitChangesInternal: will add Subscription to Partner with PartnerKey " +
                                ExtractRow.PartnerKey.ToString());
                            FInspectDT[0].PartnerKey = ExtractRow.PartnerKey;
                            TLogging.LogAtLevel(7, "TExtractsAddSubscriptionsUIConnector.SubmitChangesInternal: importing Row into FSubmissionDT...");
                            FSubmissionDT.ImportRow(FInspectDT[0]);
                        }
                        else
                        {
                            // The partner already has this Subscription: add the partner to the ResponseTable
//                          TLogging.LogAtLevel(7, "TExtractsAddSubscriptionsUIConnector.SubmitChangesInternal: won't add Subscription to Partner with PartnerKey " + ExtractRow.PartnerKey.ToString());
                            PartnerTable = PPartnerAccess.LoadByPrimaryKey(ExtractRow.PartnerKey, RequiredColumns2, SubmitChangesTransaction);

                            if (FResponseDT == null)
                            {
                                FResponseDT = PartnerTable.Clone();
                            }

                            FResponseDT.ImportRow(PartnerTable[0]);
                        }
                    }

                    TLogging.LogAtLevel(7, "TExtractsAddSubscriptionsUIConnector.SubmitChangesInternal: Finished checking Partner's Subscriptions.");

                    if (FSubmissionDT.Rows.Count > 0)
                    {
                        // Submit the Partners with new Subscriptions to the PSubscription Table.
                        FAsyncExecProgress.ProgressInformation = "Adding Subscriptions to " + FSubmissionDT.Rows.Count.ToString() + " Partners...";
                        FAsyncExecProgress.ProgressPercentage = MAX_PERCENTAGE_CHECKS;
//                      TLogging.LogAtLevel(7, "TExtractsAddSubscriptionsUIConnector.SubmitChangesInternal: " + FAsyncExecProgress.ProgressInformation);

                        PSubscriptionAccess.SubmitChanges((PSubscriptionTable)FSubmissionDT, SubmitChangesTransaction);
                    }
                    else
                    {
                        TLogging.LogAtLevel(
                            7,
                            "TExtractsAddSubscriptionsUIConnector.SubmitChangesInternal: no Subscriptions were added to Partners because all the Partners in the Extract already had this Subscription.");
                    }

                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                catch (Exception Exp)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();

                    TLogging.LogAtLevel(7,
                        "TExtractsAddSubscriptionsUIConnector.SubmitChangesInternal: Exception occured, Transaction ROLLED BACK. Exception: " +
                        Exp.ToString());

                    FSubmitResult = TSubmitChangesResult.scrError;
                    FSubmitException = Exp;
                    FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_Stopped;

                    return;
                }
            }
            else
            {
                SubmissionResult = TSubmitChangesResult.scrNothingToBeSaved;
            }

            // if no values at response table, it needs to be created. If not creates, will raise exeption at client side.
            if (FResponseDT == null)
            {
                FResponseDT = new DataTable();
            }

            FAsyncExecProgress.ProgressPercentage = 100;
            FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_Finished;
            FSubmitResult = SubmissionResult;
        }
    }
}