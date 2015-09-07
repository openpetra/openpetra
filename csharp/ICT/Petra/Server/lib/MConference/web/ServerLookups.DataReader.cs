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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MConference.Applications;


namespace Ict.Petra.Server.MConference.Conference.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MConference DataReader sub-namespace.
    ///
    /// </summary>
    public class TConferenceDataReaderWebConnector
    {
        /// <summary>
        /// Return selected conference's currency code and currency name
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <param name="ACurrencyCode">CurrencyCode for selected conference</param>
        /// <param name="ACurrencyName">CurrencyName for selected conference</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean GetCurrency(Int64 APartnerKey, out string ACurrencyCode, out string ACurrencyName)
        {
            TDBTransaction ReadTransaction = null;

            PcConferenceTable ConferenceTable;
            ACurrencyTable CurrencyTable;
            Boolean ReturnValue = false;
            string CurrencyCode = string.Empty;
            string CurrencyName = string.Empty;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, ref ReadTransaction,
                delegate
                {
                    ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                    if (ConferenceTable.Rows.Count == 0)
                    {
                        ReturnValue = false;
                    }
                    else
                    {
                        CurrencyCode = ((PcConferenceRow)ConferenceTable.Rows[0]).CurrencyCode;

                        // use the obtained currency code to retrieve the currency name
                        CurrencyTable = ACurrencyAccess.LoadByPrimaryKey(CurrencyCode, ReadTransaction);
                        CurrencyName = CurrencyTable[0].CurrencyName;

                        ReturnValue = true;
                    }
                });

            ACurrencyCode = CurrencyCode;
            ACurrencyName = CurrencyName;

            return ReturnValue;
        }

        /// <summary>
        /// Return selected conference's start date
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static DateTime GetStartDate(Int64 APartnerKey)
        {
            TDBTransaction ReadTransaction = null;

            PcConferenceTable ConferenceTable;
            DateTime ConferenceStartDate = new DateTime();

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, ref ReadTransaction,
                delegate
                {
                    ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                    if (((PcConferenceRow)ConferenceTable.Rows[0]).Start != null)
                    {
                        ConferenceStartDate = (DateTime)((PcConferenceRow)ConferenceTable.Rows[0]).Start;
                    }
                });

            return ConferenceStartDate;
        }

        /// <summary>
        /// Return selected conference's end date
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static DateTime GetEndDate(Int64 APartnerKey)
        {
            TDBTransaction ReadTransaction = null;

            PcConferenceTable ConferenceTable;
            DateTime ConferenceEndDate = new DateTime();

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, ref ReadTransaction,
                delegate
                {
                    ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                    if (((PcConferenceRow)ConferenceTable.Rows[0]).End != null)
                    {
                        ConferenceEndDate = (DateTime)((PcConferenceRow)ConferenceTable.Rows[0]).End;
                    }
                });

            return ConferenceEndDate;
        }

        /// <summary>
        /// Check Discount Criteria row exists
        /// </summary>
        /// <param name="ADiscountCriteriaCode">Primary Key to check exists</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool CheckDiscountCriteriaCodeExists(string[] ADiscountCriteriaCode)
        {
            TDBTransaction ReadTransaction = null;

            Boolean CriteriaCodeExists = true;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, ref ReadTransaction,
                delegate
                {
                    foreach (string CriteriaCode in ADiscountCriteriaCode)
                    {
                        if (!PcDiscountCriteriaAccess.Exists(CriteriaCode, ReadTransaction))
                        {
                            CriteriaCodeExists = false;
                            break;
                        }
                    }
                });

            return CriteriaCodeExists;
        }

        /// <summary>
        /// Check Cost Type row exists
        /// </summary>
        /// <param name="ACostTypeCode">Primary Key to check exists</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool CheckCostTypeExists(string ACostTypeCode)
        {
            TDBTransaction ReadTransaction = null;

            Boolean RowExists = false;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, ref ReadTransaction,
                delegate
                {
                    RowExists = PcCostTypeAccess.Exists(ACostTypeCode, ReadTransaction);
                });

            return RowExists;
        }

        /// <summary>
        /// Check that a conference exists for a partner key
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean ConferenceExists(long APartnerKey)
        {
            TDBTransaction ReadTransaction = null;
            Boolean Exists = false;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
                delegate
                {
                    Exists = PcConferenceAccess.Exists(APartnerKey, ReadTransaction);
                });

            return Exists;
        }

        /// <summary>
        /// Get the outreach types for the selected conference
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static DataTable GetOutreachTypes(long APartnerKey)
        {
            TDBTransaction ReadTransaction = null;

            DataTable Table = new PUnitTable();

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
                delegate
                {
                    string OutreachPrefixCode =
                        ((PcConferenceRow)PcConferenceAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction).Rows[0]).OutreachPrefix;

                    string Query = "SELECT" +
                                   " p_unit.p_partner_key_n," +
                                   " SUBSTRING (p_unit.p_outreach_code_c,6,6) AS p_outreach_code_c," +
                                   " p_unit.p_unit_name_c" +

                                   " FROM p_unit" +

                                   " WHERE LENGTH(p_unit.p_outreach_code_c) = 13" +
                                   " AND SUBSTRING(p_unit.p_outreach_code_c,1,5) = '" + OutreachPrefixCode + "'";

                    DBAccess.GDBAccessObj.SelectDT(Table, Query, ReadTransaction);
                });

            return Table;
        }

        /// <summary>
        /// Counts the records that reference a 'DataRow' of a non-cachable DataTable. The record count is recursive, i.e.
        /// counts all records of all related DB tables that reference the 'DataRow' AND the records that reference
        /// the record(s) of all related DB tables that reference the 'DataRow'!
        /// </summary>
        /// <param name="ADataTable">Tells for which non-cachable DataTable the records that reference a 'DataRow'
        /// of that cachable DataTable should be counted.
        /// IMPORTANT NOTE: Only tables that have a client screen with a delete button are implemented.</param>
        /// <param name="APrimaryKeyValues">Values of the Primary Key of the DataRow in question represented as an Array of object.
        /// (This can easily be obtained using the Method 'Ict.Common.Data.DataUtilities.GetPKValuesFromDataRow()'). The reason why
        /// a DataRow isn't passed for this Argument is that the 'DataRow' Class is not Serializable. </param>
        /// <param name="AVerificationResult">A 'TVerificationResultCollection' containing a single
        /// 'TVerificationResult' that contains information about DB Table references created by a cascading count
        /// Method if the count yielded more than 0 referencing DataRows.</param>
        /// <returns>The number records that reference a 'DataRow' of a non-cachable DataTable.</returns>
        [RequireModulePermission("NONE")]
        public static int GetNonCacheableRecordReferenceCountManual(TTypedDataTable ADataTable,
            object[] APrimaryKeyValues,
            out TVerificationResultCollection AVerificationResult)
        {
            int ReturnValue = 0;

            Boolean NewTransaction;
            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                if (ADataTable is PcConferenceTable)
                {
                    ReturnValue = CountByPrimaryKey(APrimaryKeyValues, ReadTransaction, true, out AVerificationResult);
                }
                else
                {
                    AVerificationResult = new TVerificationResultCollection();
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(9, ADataTable.TableName + ": GetNonCacheableRecordReferenceCount: committed own transaction.");
                }
            }

            return ReturnValue;
        }

        /// cascading count
        private static int CountByPrimaryKey(Int64 AConferenceKey,
            TDBTransaction ATransaction,
            bool AWithCascCount,
            out List <TRowReferenceInfo>AReferences,
            int ANestingDepth = 0)
        {
            int OverallReferences = 0;

            AReferences = new List <TRowReferenceInfo>();

            return OverallReferences;
        }

        /// cascading count
        private static int CountByPrimaryKey(Int64 AConferenceKey,
            TDBTransaction ATransaction,
            bool AWithCascCount,
            out TVerificationResultCollection AVerificationResults,
            int ANestingDepth = 0,
            TResultSeverity AResultSeverity = TResultSeverity.Resv_Critical)
        {
            int ReturnValue;

            List <TRowReferenceInfo>References;
            Dictionary <string, object>PKInfo = null;

            ReturnValue = CountByPrimaryKey(AConferenceKey, ATransaction, AWithCascCount, out References, ANestingDepth);

            if (ReturnValue > 0)
            {
                PKInfo = new Dictionary <string, object>(1);
                PKInfo.Add("Partner Key", AConferenceKey);

                AVerificationResults = TTypedDataAccess.BuildVerificationResultCollectionFromRefTables("PcConference",
                    "Conference",
                    PKInfo,
                    References,
                    AResultSeverity);
            }
            else
            {
                AVerificationResults = null;
            }

            return ReturnValue;
        }

        /// cascading count
        private static int CountByPrimaryKey(object[] APrimaryKeyValues,
            TDBTransaction ATransaction,
            bool AWithCascCount,
            out TVerificationResultCollection AVerificationResults,
            int ANestingDepth = 0,
            TResultSeverity AResultSeverity = TResultSeverity.Resv_Critical)
        {
            if ((APrimaryKeyValues == null)
                || (APrimaryKeyValues.Length == 0))
            {
                throw new ArgumentException("APrimaryKeyValues must not be null and must contain at least one element");
            }

            return CountByPrimaryKey((Int64)APrimaryKeyValues[0], ATransaction, AWithCascCount, out AVerificationResults, ANestingDepth);
        }

        /// <summary>
        /// populate ConferenceApplicationTDS dataset
        /// </summary>
        /// <param name="AMainDS">Dataset to be populated</param>
        /// <param name="AConferenceKey">match long for conference key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean GetConferenceApplications(ref ConferenceApplicationTDS AMainDS, Int64 AConferenceKey)
        {
            Boolean NewTransaction;

            // make sure outreach codes are up to date in case it has changed in Unit record
            TAttendeeManagement.RefreshOutreachCode(AConferenceKey);

            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                PcConferenceTable ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(AConferenceKey, ReadTransaction);

                if (ConferenceTable.Count == 0)
                {
                    throw new Exception("Cannot find conference " + AConferenceKey.ToString("0000000000"));
                }

                string OutreachPrefix = ConferenceTable[0].OutreachPrefix;

                // load application data for all conference attendees from db
                TApplicationManagement.GetApplications(ref AMainDS, AConferenceKey, OutreachPrefix, "all", -1, true, null, false);

                // obtain PPartner records for all the home offices
                foreach (PcAttendeeRow AttendeeRow in AMainDS.PcAttendee.Rows)
                {
                    if (AMainDS.PPartner.Rows.Find(new object[] { AttendeeRow.HomeOfficeKey }) == null)
                    {
                        PPartnerAccess.LoadByPrimaryKey(AMainDS, AttendeeRow.HomeOfficeKey, ReadTransaction);
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.GetConferenceApplications: commit own transaction.");
                }
            }

            return true;
        }

        /// <summary>
        /// Load/Refresh Attendees for all outreaches for a conference
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static void RefreshAttendees(Int64 AConferenceKey)
        {
            TAttendeeManagement.RefreshAttendees(AConferenceKey);
        }
    }
}