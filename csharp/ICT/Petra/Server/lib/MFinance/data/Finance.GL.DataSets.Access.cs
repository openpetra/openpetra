// auto generated with nant generateORM
// Do not modify this file manually!
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2010 by OM International
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

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Petra.Shared;
using System;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;

namespace Ict.Petra.Server.MFinance.GL.Data.Access
{
     /// auto generated
    [Serializable()]
    public class GLBatchTDSAccess
    {

        /// auto generated
        static public TSubmitChangesResult SubmitChanges(GLBatchTDS AInspectDS, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();

            if (AInspectDS == null)
            {
                return TSubmitChangesResult.scrOK;
            }

            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                SubmissionResult = TSubmitChangesResult.scrOK;

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ATransAnalAttrib, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AGeneralLedgerMasterPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AGeneralLedgerMaster, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountHierarchyDetail, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ACostCentre, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccount, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ATransaction, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AJournal, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ABatch, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ALedger, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ALedger, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ABatch, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AJournal, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ATransaction, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccount, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ACostCentre, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountHierarchyDetail, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK && AInspectDS.AGeneralLedgerMaster != null)
                {
                    SortedList<Int64, Int32> OldSequenceValuesRow = new SortedList<Int64, Int32>();
                    Int32 rowIndex = 0;
                    foreach (AGeneralLedgerMasterRow origRow in AInspectDS.AGeneralLedgerMaster.Rows)
                    {
                        if (origRow.RowState != DataRowState.Deleted)
                        {
                            OldSequenceValuesRow.Add(origRow.GlmSequence, rowIndex);
                        }

                        rowIndex++;
                    }
                    if (!TTypedDataAccess.SubmitChanges(AInspectDS.AGeneralLedgerMaster, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_general_ledger_master", "a_glm_sequence_i"))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                    else
                    {
                        if (AInspectDS.AGeneralLedgerMasterPeriod != null)
                        {
                            foreach (AGeneralLedgerMasterPeriodRow otherRow in AInspectDS.AGeneralLedgerMasterPeriod.Rows)
                            {
                                if ((otherRow.RowState != DataRowState.Deleted) && otherRow.GlmSequence < 0)
                                {
                                    otherRow.GlmSequence = AInspectDS.AGeneralLedgerMaster[OldSequenceValuesRow[otherRow.GlmSequence]].GlmSequence;
                                }
                            }
                        }
                    }
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AGeneralLedgerMasterPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ATransAnalAttrib, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
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
                TLogging.Log("exception during saving dataset GLBatchTDS:" + e.Message);

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw new Exception(e.ToString() + " " + e.Message);
            }

            return SubmissionResult;
        }
    }
     /// auto generated
    [Serializable()]
    public class GLSetupTDSAccess
    {

        /// auto generated
        static public TSubmitChangesResult SubmitChanges(GLSetupTDS AInspectDS, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();

            if (AInspectDS == null)
            {
                return TSubmitChangesResult.scrOK;
            }

            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                SubmissionResult = TSubmitChangesResult.scrOK;

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerLedger, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerLocation, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PLocation, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PUnit, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.SSystemDefaults, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.SModule, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AMotivationDetail, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AMotivationGroup, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AGeneralLedgerMasterPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AGeneralLedgerMaster, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AValidLedgerNumber, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ACostCentre, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ACostCentreTypes, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ABudgetType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ABudgetRevision, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ABudgetPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ABudget, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AFreeformAnalysis, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAnalysisType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAnalysisStoreTable, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAnalysisAttribute, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ATransactionType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountPropertyCode, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountProperty, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountHierarchyDetail, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountHierarchy, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccount, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ASystemInterface, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountingPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountingSystemParameter, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ALedgerInitFlag, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ALedger, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartner, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartner, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ALedger, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ALedgerInitFlag, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountingSystemParameter, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountingPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ASystemInterface, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccount, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountHierarchy, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountHierarchyDetail, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountProperty, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAccountPropertyCode, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ATransactionType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAnalysisAttribute, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAnalysisStoreTable, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AAnalysisType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AFreeformAnalysis, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK && AInspectDS.ABudget != null)
                {
                    SortedList<Int64, Int32> OldSequenceValuesRow = new SortedList<Int64, Int32>();
                    Int32 rowIndex = 0;
                    foreach (ABudgetRow origRow in AInspectDS.ABudget.Rows)
                    {
                        if (origRow.RowState != DataRowState.Deleted)
                        {
                            OldSequenceValuesRow.Add(origRow.BudgetSequence, rowIndex);
                        }

                        rowIndex++;
                    }
                    if (!TTypedDataAccess.SubmitChanges(AInspectDS.ABudget, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_budget", "a_budget_sequence_i"))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                    else
                    {
                        if (AInspectDS.ABudgetPeriod != null)
                        {
                            foreach (ABudgetPeriodRow otherRow in AInspectDS.ABudgetPeriod.Rows)
                            {
                                if ((otherRow.RowState != DataRowState.Deleted) && otherRow.BudgetSequence < 0)
                                {
                                    otherRow.BudgetSequence = AInspectDS.ABudget[OldSequenceValuesRow[otherRow.BudgetSequence]].BudgetSequence;
                                }
                            }
                        }
                    }
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ABudgetPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ABudgetRevision, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ABudgetType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ACostCentreTypes, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.ACostCentre, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AValidLedgerNumber, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK && AInspectDS.AGeneralLedgerMaster != null)
                {
                    SortedList<Int64, Int32> OldSequenceValuesRow = new SortedList<Int64, Int32>();
                    Int32 rowIndex = 0;
                    foreach (AGeneralLedgerMasterRow origRow in AInspectDS.AGeneralLedgerMaster.Rows)
                    {
                        if (origRow.RowState != DataRowState.Deleted)
                        {
                            OldSequenceValuesRow.Add(origRow.GlmSequence, rowIndex);
                        }

                        rowIndex++;
                    }
                    if (!TTypedDataAccess.SubmitChanges(AInspectDS.AGeneralLedgerMaster, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_general_ledger_master", "a_glm_sequence_i"))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                    else
                    {
                        if (AInspectDS.AGeneralLedgerMasterPeriod != null)
                        {
                            foreach (AGeneralLedgerMasterPeriodRow otherRow in AInspectDS.AGeneralLedgerMasterPeriod.Rows)
                            {
                                if ((otherRow.RowState != DataRowState.Deleted) && otherRow.GlmSequence < 0)
                                {
                                    otherRow.GlmSequence = AInspectDS.AGeneralLedgerMaster[OldSequenceValuesRow[otherRow.GlmSequence]].GlmSequence;
                                }
                            }
                        }
                    }
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AGeneralLedgerMasterPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AMotivationGroup, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.AMotivationDetail, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.SModule, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.SSystemDefaults, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PUnit, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK && AInspectDS.PLocation != null)
                {
                    SortedList<Int64, Int32> OldSequenceValuesRow = new SortedList<Int64, Int32>();
                    Int32 rowIndex = 0;
                    foreach (PLocationRow origRow in AInspectDS.PLocation.Rows)
                    {
                        if (origRow.RowState != DataRowState.Deleted)
                        {
                            OldSequenceValuesRow.Add(origRow.LocationKey, rowIndex);
                        }

                        rowIndex++;
                    }
                    if (!TTypedDataAccess.SubmitChanges(AInspectDS.PLocation, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_location_number", "p_location_key_i"))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                    else
                    {
                        if (AInspectDS.PPartnerLocation != null)
                        {
                            foreach (PPartnerLocationRow otherRow in AInspectDS.PPartnerLocation.Rows)
                            {
                                if ((otherRow.RowState != DataRowState.Deleted) && otherRow.LocationKey < 0)
                                {
                                    otherRow.LocationKey = AInspectDS.PLocation[OldSequenceValuesRow[otherRow.LocationKey]].LocationKey;
                                }
                            }
                        }
                    }
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerLocation, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerLedger, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
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
                TLogging.Log("exception during saving dataset GLSetupTDS:" + e.Message);

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw new Exception(e.ToString() + " " + e.Message);
            }

            return SubmissionResult;
        }
    }
}
