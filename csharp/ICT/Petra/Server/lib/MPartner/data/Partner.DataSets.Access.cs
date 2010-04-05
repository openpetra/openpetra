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
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;

namespace Ict.Petra.Server.MPartner.Partner.Data.Access
{

     /// auto generated
    [Serializable()]
    public class PartnerEditTDSAccess
    {

        /// auto generated
        static public TSubmitChangesResult SubmitChanges(PartnerEditTDS AInspectDS, out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            AVerificationResult = new TVerificationResultCollection();

            try
            {
                SubmissionResult = TSubmitChangesResult.scrOK;

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PDataLabelValuePartner, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PDataLabelValueApplication, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerContact, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_contact", "p_contact_id_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerRelationship, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerReminder, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PInterest, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerInterest, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PFoundationProposalDetail, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_proposal_detail", "p_proposal_detail_id_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PFoundationProposal, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_foundation_proposal", "p_foundation_proposal_key_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PFoundationDeadline, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PFoundation, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PVenue, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerBankingDetails, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PBankingDetails, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_bank_details", "p_banking_details_key_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PBank, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PChurch, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.POrganisation, SubmitChangesTransaction,
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
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PFamily, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPerson, SubmitChangesTransaction,
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
                            UserInfo.GUserInfo.UserID, "seq_location_number", "p_location_key_i"))
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
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PSubscription, SubmitChangesTransaction,
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
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PSubscription, SubmitChangesTransaction,
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
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PLocation, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_location_number", "p_location_key_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPerson, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PFamily, SubmitChangesTransaction,
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
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.POrganisation, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PChurch, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PBank, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PBankingDetails, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_bank_details", "p_banking_details_key_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerBankingDetails, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PVenue, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PFoundation, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PFoundationDeadline, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PFoundationProposal, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_foundation_proposal", "p_foundation_proposal_key_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PFoundationProposalDetail, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_proposal_detail", "p_proposal_detail_id_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerInterest, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PInterest, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerReminder, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerRelationship, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerContact, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_contact", "p_contact_id_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PDataLabelValueApplication, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PDataLabelValuePartner, SubmitChangesTransaction,
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
                TLogging.Log("exception during saving dataset PartnerEditTDS:" + e.Message);

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw new Exception(e.ToString() + " " + e.Message);
            }

            return SubmissionResult;
        }
    }
     /// auto generated
    [Serializable()]
    public class PartnerAddressAggregateTDSAccess
    {

        /// auto generated
        static public TSubmitChangesResult SubmitChanges(PartnerAddressAggregateTDS AInspectDS, out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            AVerificationResult = new TVerificationResultCollection();

            try
            {
                SubmissionResult = TSubmitChangesResult.scrOK;

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.SimilarLocationParameters, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_location_number", "p_location_key_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.SimilarLocationParameters, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_location_number", "p_location_key_i"))
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
                TLogging.Log("exception during saving dataset PartnerAddressAggregateTDS:" + e.Message);

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw new Exception(e.ToString() + " " + e.Message);
            }

            return SubmissionResult;
        }
    }
     /// auto generated
    [Serializable()]
    public class PartnerInfoTDSAccess
    {

        /// auto generated
        static public TSubmitChangesResult SubmitChanges(PartnerInfoTDS AInspectDS, out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            AVerificationResult = new TVerificationResultCollection();

            try
            {
                SubmissionResult = TSubmitChangesResult.scrOK;

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PSubscription, SubmitChangesTransaction,
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
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerLocation, SubmitChangesTransaction,
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
                            UserInfo.GUserInfo.UserID, "seq_location_number", "p_location_key_i"))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PLocation, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_location_number", "p_location_key_i"))
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
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPartnerType, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PSubscription, SubmitChangesTransaction,
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
                TLogging.Log("exception during saving dataset PartnerInfoTDS:" + e.Message);

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw new Exception(e.ToString() + " " + e.Message);
            }

            return SubmissionResult;
        }
    }
}