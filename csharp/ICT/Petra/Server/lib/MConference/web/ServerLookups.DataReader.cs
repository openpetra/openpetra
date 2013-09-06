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


namespace Ict.Petra.Server.MConference.Conference.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MCommon DataReader sub-namespace.
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
            ACurrencyCode = "";
            ACurrencyName = "";

            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            PcConferenceTable ConferenceTable;
            ACurrencyTable CurrencyTable;
            Boolean ReturnValue = false;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                if (ConferenceTable.Rows.Count == 0)
                {
                    ReturnValue = false;
                }
                else
                {
                    ACurrencyCode = ((PcConferenceRow)ConferenceTable.Rows[0]).CurrencyCode;

                    // use the obtained currency code to retrieve the currency name
                    CurrencyTable = ACurrencyAccess.LoadByPrimaryKey(ACurrencyCode, ReadTransaction);
                    ACurrencyName = CurrencyTable[0].CurrencyName;

                    ReturnValue = true;
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.GetCurrency: rollback own transaction.");
                }
            }

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
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            PcConferenceTable ConferenceTable;
            DateTime ConferenceStartDate = new DateTime();

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                ConferenceStartDate = (DateTime)((PcConferenceRow)ConferenceTable.Rows[0]).Start;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.GetStartDate: rollback own transaction.");
                }
            }

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
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            PcConferenceTable ConferenceTable;
            DateTime ConferenceEndDate = new DateTime();

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                ConferenceEndDate = (DateTime)((PcConferenceRow)ConferenceTable.Rows[0]).End;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.GetEndDate: rollback own transaction.");
                }
            }

            return ConferenceEndDate;
        }

        /// <summary>
        /// Check Discount Criteria row exists
        /// </summary>
        /// <param name="ADiscountCriteriaCode">Primary Key to check exists</param>
        /// <param name="ADiscountCriteriaDescription">Criteria description to add if row does not exist</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static void CreateDiscountCriteriaIfNotExisting(string ADiscountCriteriaCode, string ADiscountCriteriaDescription)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            Boolean RowExists = false;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                RowExists = PcDiscountCriteriaAccess.Exists(ADiscountCriteriaCode, ReadTransaction);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.CreateDiscountCriteriaIfNotExisting: rollback own transaction.");
                }
            }

            // add row is it does not exist
            if (!RowExists)
            {
                AddDiscountCriteriaCode(ADiscountCriteriaCode, ADiscountCriteriaDescription);
            }
        }

        /// <summary>
        /// Create new Discount Criteria row
        /// </summary>
        /// <param name="ADiscountCriteriaCode">Criteria code to add</param>
        /// <param name="ADiscountCriteriaDescription">Criteria description to add</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        private static void AddDiscountCriteriaCode(string ADiscountCriteriaCode, string ADiscountCriteriaDescription)
        {
            TDBTransaction Transaction;
            PcDiscountCriteriaTable DiscountCriteriaTable = null;
            TVerificationResultCollection VerificationResult;

            Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                DiscountCriteriaTable = PcDiscountCriteriaAccess.LoadAll(Transaction);

                // set column values
                PcDiscountCriteriaRow AddRow = DiscountCriteriaTable.NewRowTyped();
                AddRow.DiscountCriteriaCode = ADiscountCriteriaCode;
                AddRow.DiscountCriteriaDesc = ADiscountCriteriaDescription;
                AddRow.DeletableFlag = false;

                // add new row to database table
                DiscountCriteriaTable.Rows.Add(AddRow);
                PcDiscountCriteriaAccess.SubmitChanges(DiscountCriteriaTable, Transaction, out VerificationResult);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.AddDiscountCriteriaCode: commit own transaction.");
            }
        }

        /// <summary>
        /// Check Cost Type row exists
        /// </summary>
        /// <param name="ACostTypeCode">Primary Key to check exists</param>
        /// <param name="ACostTypeDescription">Cost Type description to add if row does not exist</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static void CreateCostTypeIfNotExisting(string ACostTypeCode, string ACostTypeDescription)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            Boolean RowExists = false;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                RowExists = PcCostTypeAccess.Exists(ACostTypeCode, ReadTransaction);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.CreateCostTypeIfNotExisting: rollback own transaction.");
                }
            }

            // add row is it does not exist
            if (!RowExists)
            {
                AddCostTypeCode(ACostTypeCode, ACostTypeDescription);
            }
        }

        /// <summary>
        /// Create new Cost Type row
        /// </summary>
        /// <param name="ACostTypeCode">Cost Type Code to add</param>
        /// <param name="ACostTypeDescription">Cost Type Description to add</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        private static void AddCostTypeCode(string ACostTypeCode, string ACostTypeDescription)
        {
            TDBTransaction Transaction;
            PcCostTypeTable CostTypeTable = null;
            TVerificationResultCollection VerificationResult;

            Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                CostTypeTable = PcCostTypeAccess.LoadAll(Transaction);

                // set column values
                PcCostTypeRow AddRow = CostTypeTable.NewRowTyped();
                AddRow.CostTypeCode = ACostTypeCode;
                AddRow.CostTypeDescription = ACostTypeDescription;
                AddRow.DeletableFlag = false;

                // add new row to database table
                CostTypeTable.Rows.Add(AddRow);
                PcCostTypeAccess.SubmitChanges(CostTypeTable, Transaction, out VerificationResult);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.AddCostTypeCode: commit own transaction.");
            }
        }

        /// <summary>
        /// Create new Conference
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean ConferenceExists(long APartnerKey)
        {
            TDBTransaction ReadTransaction;
            Boolean Exists = false;

            ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                Exists = PcConferenceAccess.Exists(APartnerKey, ReadTransaction);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.CreateNewConference: commit own transaction.");
            }

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
            TDBTransaction ReadTransaction;

            ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            DataTable Table = new PUnitTable();

            try
            {
                string OutreachPrefixCode =
                    ((PcConferenceRow)PcConferenceAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction).Rows[0]).OutreachPrefix;

                PUnitTable UnitTable = PUnitAccess.LoadAll(ReadTransaction);

                // add PUnit rows with matching OutreachPrefixCode to the new DataTable
                foreach (PUnitRow Row in UnitTable.Rows)
                {
                    if ((Row.OutreachCode.Length == 13) && (Row.OutreachCode.Substring(0, 5) == OutreachPrefixCode))
                    {
                        DataRow CopyRow = Table.NewRow();
                        ((PUnitRow)CopyRow).PartnerKey = Row.PartnerKey;
                        ((PUnitRow)CopyRow).OutreachCode = Row.OutreachCode.Substring(5, 6);
                        ((PUnitRow)CopyRow).UnitName = Row.UnitName;
                        Table.Rows.Add(CopyRow);
                    }
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.GetOutreachTypes: commit own transaction.");
            }

            return Table;
        }

        /// <summary>
        /// Create a new Conference
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static void CreateNewConference(long APartnerKey)
        {
            TDBTransaction Transaction;
            TVerificationResultCollection VerificationResult;
            PcConferenceTable ConferenceTable;
            PUnitTable UnitTable;
            PPartnerLocationTable PartnerLocationTable;

            Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                ConferenceTable = PcConferenceAccess.LoadAll(Transaction);
                UnitTable = PUnitAccess.LoadByPrimaryKey(APartnerKey, Transaction);
                PartnerLocationTable = PPartnerLocationAccess.LoadAll(Transaction);

                DateTime Start = new DateTime();
                DateTime End = new DateTime();

                foreach (PPartnerLocationRow PartnerLocationRow in PartnerLocationTable.Rows)
                {
                    if (PartnerLocationRow.PartnerKey == APartnerKey)
                    {
                        if (PartnerLocationRow.DateEffective != null)
                        {
                            Start = (DateTime)PartnerLocationRow.DateEffective;
                        }

                        if (PartnerLocationRow.DateGoodUntil != null)
                        {
                            End = (DateTime)PartnerLocationRow.DateGoodUntil;
                        }
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

                AddRow.CurrencyCode = "USD";

                // add new row to database table
                ConferenceTable.Rows.Add(AddRow);
                PcConferenceAccess.SubmitChanges(ConferenceTable, Transaction, out VerificationResult);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.CreateNewConference: commit own transaction.");
            }
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
    }
}