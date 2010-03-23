/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
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

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Petra.Shared;
using System;
using System.Data;
using System.Data.Odbc;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Server.MFinance.GL.Data.Access
{

     /// auto generated
    [Serializable()]
    public class GLBatchTDSAccess : GLBatchTDS
    {

        /// auto generated
        public TSubmitChangesResult SubmitChanges(out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            AVerificationResult = new TVerificationResultCollection();

            try
            {
                SubmissionResult = TSubmitChangesResult.scrOK;

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ALedger, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ABatch, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AJournal, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ATransaction, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccount, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ACostCentre, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountHierarchyDetail, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AGeneralLedgerMaster, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_general_ledger_master", "a_glm_sequence_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AGeneralLedgerMasterPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ALedger, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ABatch, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AJournal, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ATransaction, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccount, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ACostCentre, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountHierarchyDetail, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AGeneralLedgerMaster, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_general_ledger_master", "a_glm_sequence_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AGeneralLedgerMasterPeriod, SubmitChangesTransaction,
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
    public class GLSetupTDSAccess : GLSetupTDS
    {

        /// auto generated
        public TSubmitChangesResult SubmitChanges(out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            AVerificationResult = new TVerificationResultCollection();

            try
            {
                SubmissionResult = TSubmitChangesResult.scrOK;

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ALedger, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ALedgerInitFlag, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountingSystemParameter, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountingPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccount, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountHierarchy, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountHierarchyDetail, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountProperty, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountPropertyCode, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAnalysisAttribute, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAnalysisStoreTable, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAnalysisType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AFreeformAnalysis, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ABudget, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_budget", "a_budget_sequence_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ABudgetPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ABudgetRevision, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ABudgetType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ACostCentre, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ACostCentreTypes, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AGeneralLedgerMaster, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_general_ledger_master", "a_glm_sequence_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AGeneralLedgerMasterPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ALedger, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ALedgerInitFlag, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountingSystemParameter, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountingPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccount, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountHierarchy, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountHierarchyDetail, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountProperty, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAccountPropertyCode, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAnalysisAttribute, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAnalysisStoreTable, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AAnalysisType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AFreeformAnalysis, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ABudget, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_budget", "a_budget_sequence_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ABudgetPeriod, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ABudgetRevision, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ABudgetType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ACostCentre, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(ACostCentreTypes, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AGeneralLedgerMaster, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_general_ledger_master", "a_glm_sequence_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AGeneralLedgerMasterPeriod, SubmitChangesTransaction,
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