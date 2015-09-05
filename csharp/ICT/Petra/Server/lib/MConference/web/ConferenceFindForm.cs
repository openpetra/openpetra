//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Data;
using System.Data.Odbc;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Server.MConference.Conference.WebConnectors
{
    /// <summary>
    /// provides server side methods for the Conference Find Form
    /// </summary>
    public class TConferenceFindForm
    {
        /// <summary>
        /// Create a new Conference
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static void CreateNewConference(long APartnerKey)
        {
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            PcConferenceTable ConferenceTable;
            PUnitTable UnitTable;
            PPartnerLocationTable PartnerLocationTable;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref Transaction, ref SubmissionOK,
                delegate
                {
                    try
                    {
                        ConferenceTable = PcConferenceAccess.LoadAll(Transaction);
                        UnitTable = PUnitAccess.LoadByPrimaryKey(APartnerKey, Transaction);
                        PartnerLocationTable = PPartnerLocationAccess.LoadViaPPartner(APartnerKey, Transaction);

                        DateTime Start = new DateTime();
                        DateTime End = new DateTime();

                        foreach (PPartnerLocationRow PartnerLocationRow in PartnerLocationTable.Rows)
                        {
                            if ((PartnerLocationRow.DateEffective != null) || (PartnerLocationRow.DateGoodUntil != null))
                            {
                                if (PartnerLocationRow.DateEffective != null)
                                {
                                    Start = (DateTime)PartnerLocationRow.DateEffective;
                                }

                                if (PartnerLocationRow.DateGoodUntil != null)
                                {
                                    End = (DateTime)PartnerLocationRow.DateGoodUntil;
                                }

                                break;
                            }
                        }

                        // set column values
                        PcConferenceRow AddRow = ConferenceTable.NewRowTyped();
                        AddRow.ConferenceKey = APartnerKey;

                        string OutreachPrefix = ((PUnitRow)UnitTable.Rows[0]).OutreachCode;

                        if (OutreachPrefix.Length > 4)
                        {
                            AddRow.OutreachPrefix = OutreachPrefix.Substring(0, 5);
                        }
                        else
                        {
                            AddRow.OutreachPrefix = OutreachPrefix;
                        }

                        if (Start != DateTime.MinValue)
                        {
                            AddRow.Start = Start;
                        }

                        if (End != DateTime.MinValue)
                        {
                            AddRow.End = End;
                        }

                        string CurrencyCode = ((PUnitRow)UnitTable.Rows[0]).OutreachCostCurrencyCode;

                        if (!string.IsNullOrEmpty(CurrencyCode))
                        {
                            AddRow.CurrencyCode = CurrencyCode;
                        }
                        else
                        {
                            AddRow.CurrencyCode = "USD";
                        }

                        // add new row to database table
                        ConferenceTable.Rows.Add(AddRow);
                        PcConferenceAccess.SubmitChanges(ConferenceTable, Transaction);

                        SubmissionOK = true;
                    }
                    catch (Exception Exc)
                    {
                        TLogging.Log("An Exception occured during the creation of a new Conference:" + Environment.NewLine + Exc.ToString());
                    }
                });
        }

        /// <summary>
        /// deletes the complete conference including all conference data
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static bool DeleteConference(Int64 AConferenceKey, out TVerificationResultCollection AVerificationResult)
        {
            TVerificationResultCollection VerificationResult = null;

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), Catalog.GetString("Deleting conference"), 100);

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref Transaction, ref SubmissionOK,
                delegate
                {
                    try
                    {
                        string[] TableNames = new string[] {
                            PcAttendeeTable.GetTableDBName(),
                            PcConferenceCostTable.GetTableDBName(),
                            PcConferenceOptionTable.GetTableDBName(),
                            PcConferenceVenueTable.GetTableDBName(),
                            PcDiscountTable.GetTableDBName(),
                            PcEarlyLateTable.GetTableDBName(),
                            PcExtraCostTable.GetTableDBName(),
                            PcGroupTable.GetTableDBName(),
                            PcSupplementTable.GetTableDBName()
                        };

                        OdbcParameter[] ConferenceParameter = new OdbcParameter[] {
                            new OdbcParameter("conferencekey", OdbcType.BigInt)
                        };

                        ConferenceParameter[0].Value = AConferenceKey;

                        int Progress = 0;

                        foreach (string Table in TableNames)
                        {
                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Deleting: ") + Table, 10 *
                                Progress);

                            DBAccess.GDBAccessObj.ExecuteNonQuery(
                                String.Format("DELETE FROM PUB_{0} WHERE pc_conference_key_n = ?", Table),
                                Transaction, ConferenceParameter);

                            Progress++;
                        }

                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Deleting: Conference"), 90);
                        PcConferenceAccess.DeleteByPrimaryKey(AConferenceKey, Transaction);

                        if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == false)
                        {
                            SubmissionOK = true;
                        }

                        TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                    }
                    catch (Exception e)
                    {
                        TLogging.Log(e.ToString());

                        VerificationResult = new TVerificationResultCollection();
                        VerificationResult.Add(new TVerificationResult(
                                "Problems deleting conference " + AConferenceKey.ToString("0000000000"),
                                e.Message,
                                "Cannot delete conference",
                                string.Empty,
                                TResultSeverity.Resv_Critical,
                                Guid.Empty));
                        TProgressTracker.CancelJob(DomainManager.GClientID.ToString());
                    }
                });

            AVerificationResult = VerificationResult;

            return SubmissionOK;
        }
    }
}