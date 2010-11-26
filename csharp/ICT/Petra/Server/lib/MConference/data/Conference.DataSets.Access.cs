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
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;

namespace Ict.Petra.Server.MConference.Data.Access
{
     /// auto generated
    [Serializable()]
    public class SelectConferenceTDSAccess
    {

        /// auto generated
        static public TSubmitChangesResult SubmitChanges(SelectConferenceTDS AInspectDS, out TVerificationResultCollection AVerificationResult)
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
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PcConference, SubmitChangesTransaction,
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
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PcConference, SubmitChangesTransaction,
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
                TLogging.Log("exception during saving dataset SelectConferenceTDS:" + e.Message);

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw new Exception(e.ToString() + " " + e.Message);
            }

            return SubmissionResult;
        }
    }
     /// auto generated
    [Serializable()]
    public class ConferenceApplicationTDSAccess
    {

        /// auto generated
        static public TSubmitChangesResult SubmitChanges(ConferenceApplicationTDS AInspectDS, out TVerificationResultCollection AVerificationResult)
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
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PmShortTermApplication, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eDelete,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PmGeneralApplication, SubmitChangesTransaction,
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
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PPerson, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID))
                {
                    SubmissionResult = TSubmitChangesResult.scrError;
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK && AInspectDS.PmGeneralApplication != null)
                {
                    SortedList<Int64, Int32> OldSequenceValuesRow = new SortedList<Int64, Int32>();
                    Int32 rowIndex = 0;
                    foreach (PmGeneralApplicationRow origRow in AInspectDS.PmGeneralApplication.Rows)
                    {
                        if (origRow.RowState != DataRowState.Deleted)
                        {
                            OldSequenceValuesRow.Add(origRow.ApplicationKey, rowIndex);
                        }

                        rowIndex++;
                    }
                    if (!TTypedDataAccess.SubmitChanges(AInspectDS.PmGeneralApplication, SubmitChangesTransaction,
                            TTypedDataAccess.eSubmitChangesOperations.eInsert | TTypedDataAccess.eSubmitChangesOperations.eUpdate,
                            out AVerificationResult,
                            UserInfo.GUserInfo.UserID, "seq_application", "pm_application_key_i"))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                    else
                    {
                        if (AInspectDS.PmShortTermApplication != null)
                        {
                            foreach (PmShortTermApplicationRow otherRow in AInspectDS.PmShortTermApplication.Rows)
                            {
                                if ((otherRow.RowState != DataRowState.Deleted) && otherRow.ApplicationKey < 0)
                                {
                                    otherRow.ApplicationKey = AInspectDS.PmGeneralApplication[OldSequenceValuesRow[otherRow.ApplicationKey]].ApplicationKey;
                                }
                            }
                        }
                    }
                }
                if (SubmissionResult == TSubmitChangesResult.scrOK
                    && !TTypedDataAccess.SubmitChanges(AInspectDS.PmShortTermApplication, SubmitChangesTransaction,
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
                TLogging.Log("exception during saving dataset ConferenceApplicationTDS:" + e.Message);

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw new Exception(e.ToString() + " " + e.Message);
            }

            return SubmissionResult;
        }
    }
}
