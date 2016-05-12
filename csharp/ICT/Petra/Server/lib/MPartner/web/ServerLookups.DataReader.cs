//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.App.Core.Security;


namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MCommon DataReader sub-namespace.
    ///
    /// </summary>
    public class TPartnerDataReaderWebConnector
    {
        /// <summary>
        /// return unit table which contains conferences that match a given search string
        /// Only returns key, name and outreach code fields at the moment.
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static DataTable GetEventUnits()
        {
            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();
            DataColumn[] Key = new DataColumn[3];
            DataTable Events = new DataTable();

            if (TLogging.DL >= 9)
            {
                Console.WriteLine("GetEventUnits called!");
            }

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    string SqlStmt =
                        "SELECT DISTINCT " +
                        PPartnerTable.GetPartnerShortNameDBName() +
                        ", " + PPartnerTable.GetPartnerClassDBName() +
                        ", " + PUnitTable.GetOutreachCodeDBName() +
                        ", " + PCountryTable.GetTableDBName() + "." + PCountryTable.GetCountryNameDBName() +
                        ", " + PPartnerLocationTable.GetTableDBName() + "." + PPartnerLocationTable.GetSiteKeyDBName() +
                        ", " + PPartnerLocationTable.GetTableDBName() + "." + PPartnerLocationTable.GetLocationKeyDBName() +
                        ", " + PPartnerLocationTable.GetTableDBName() + "." + PPartnerLocationTable.GetDateEffectiveDBName() +
                        ", " + PPartnerLocationTable.GetTableDBName() + "." + PPartnerLocationTable.GetDateGoodUntilDBName() +

                        ", " + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName() +
                        ", " + PUnitTable.GetUnitTypeCodeDBName() +
                        ", " + PUnitTable.GetUnitNameDBName() +

                        " FROM pub_" + PPartnerTable.GetTableDBName() +
                        ", pub_" + PUnitTable.GetTableDBName() +
                        ", pub_" + PLocationTable.GetTableDBName() +
                        ", pub_" + PPartnerLocationTable.GetTableDBName() +
                        ", pub_" + PCountryTable.GetTableDBName() +

                        " WHERE " +
                        PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName() + " = " +
                        PUnitTable.GetTableDBName() + "." + PUnitTable.GetPartnerKeyDBName() + " AND " +

                        PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName() + " = " +
                        PPartnerLocationTable.GetTableDBName() + "." + PPartnerLocationTable.GetPartnerKeyDBName() + " AND " +
                        PLocationTable.GetTableDBName() + "." + PLocationTable.GetSiteKeyDBName() + " = " +
                        PPartnerLocationTable.GetTableDBName() + "." + PPartnerLocationTable.GetSiteKeyDBName() + " AND " +
                        PLocationTable.GetTableDBName() + "." + PLocationTable.GetLocationKeyDBName() + " = " +
                        PPartnerLocationTable.GetTableDBName() + "." + PPartnerLocationTable.GetLocationKeyDBName() + " AND " +
                        PCountryTable.GetTableDBName() + "." + PCountryTable.GetCountryCodeDBName() + " = " +
                        PLocationTable.GetTableDBName() + "." + PLocationTable.GetCountryCodeDBName() + " AND " +

                        PPartnerTable.GetStatusCodeDBName() + " = 'ACTIVE' " + " AND " +
                        PPartnerTable.GetPartnerClassDBName() + " = 'UNIT' ";

                    // sort rows according to name
                    SqlStmt = SqlStmt + " ORDER BY " + PPartnerTable.GetPartnerShortNameDBName();

                    Events = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "events",
                        Transaction, SqlParameterList.ToArray());

                    Key[0] = Events.Columns[PPartnerTable.GetPartnerKeyDBName()];
                    Key[1] = Events.Columns[PPartnerLocationTable.GetSiteKeyDBName()];
                    Key[2] = Events.Columns[PPartnerLocationTable.GetLocationKeyDBName()];
                    Events.PrimaryKey = Key;
                });

            return Events;
        }

        /// <summary>
        /// Return unit table which contains fields (unit partners) that match a given search string and have
        /// status ACTIVE.
        /// Only returns key and name field at the moment.
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PUnitTable GetActiveFieldUnits()
        {
            PUnitTable UnitTable = new PUnitTable();
            PUnitRow UnitRow;

            TLogging.LogAtLevel(9, "TPartnerDataReaderWebConnector.GetActiveFieldUnits called!");

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    // Load data
                    string SqlStmt = "SELECT pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetPartnerKeyDBName() +
                                     ", pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetUnitNameDBName() +
                                     " FROM " + PUnitTable.GetTableDBName() + ", " + PPartnerTable.GetTableDBName() +
                                     " WHERE ((" + PUnitTable.GetOutreachCodeDBName() + " IS NULL)" +
                                     "        OR(" + PUnitTable.GetOutreachCodeDBName() + " = ''))" +
                                     " AND " + PUnitTable.GetUnitTypeCodeDBName() + " <> 'KEY-MIN'" +
                                     " AND pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetPartnerKeyDBName() +
                                     " = pub_" + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName() +
                                     " AND " + PPartnerTable.GetStatusCodeDBName() + " = 'ACTIVE'";

                    // sort rows according to name
                    SqlStmt = SqlStmt + " ORDER BY " + PUnitTable.GetUnitNameDBName();

                    DataTable events = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "fields", Transaction);

                    foreach (DataRow eventRow in events.Rows)
                    {
                        UnitRow = (PUnitRow)UnitTable.NewRow();
                        UnitRow.PartnerKey = Convert.ToInt64(eventRow[0]);
                        UnitRow.UnitName = Convert.ToString(eventRow[1]);
                        UnitTable.Rows.Add(UnitRow);
                    }
                });

            return UnitTable;
        }

        /// <summary>
        /// Return unit table which contains ledger records (unit partners) that match a given search string
        /// Only returns key and name field at the moment.
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PUnitTable GetLedgerUnits()
        {
            PUnitTable UnitTable = new PUnitTable();
            PUnitRow UnitRow;

            TLogging.LogAtLevel(9, "TPartnerDataReaderWebConnector.GetLedgerUnits called!");

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    // Load data
                    string SqlStmt = "SELECT pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetPartnerKeyDBName() +
                                     ", pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetUnitNameDBName() +
                                     " FROM " + PUnitTable.GetTableDBName() + ", " + PPartnerTypeTable.GetTableDBName() +
                                     " WHERE pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetPartnerKeyDBName() +
                                     " = pub_" + PPartnerTypeTable.GetTableDBName() + "." + PPartnerTypeTable.GetPartnerKeyDBName() +
                                     " AND " + PPartnerTypeTable.GetTypeCodeDBName() + " = '" + MPartnerConstants.PARTNERTYPE_LEDGER + "'";

                    // sort rows according to name
                    SqlStmt = SqlStmt + " ORDER BY " + PUnitTable.GetUnitNameDBName();

                    DataTable events = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "ledgers", Transaction);

                    foreach (DataRow eventRow in events.Rows)
                    {
                        UnitRow = (PUnitRow)UnitTable.NewRow();
                        UnitRow.PartnerKey = Convert.ToInt64(eventRow[0]);
                        UnitRow.UnitName = Convert.ToString(eventRow[1]);
                        UnitTable.Rows.Add(UnitRow);
                    }
                });

            return UnitTable;
        }

        /// <summary>
        /// Check if a PUnit record has a corresponding PcConference record in the db
        /// </summary>
        /// <param name="APartnerKey">match long for partner key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean IsPUnitAConference(Int64 APartnerKey)
        {
            Boolean ReturnValue = false;
            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    PcConferenceTable ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(APartnerKey, Transaction);

                    if (ConferenceTable.Count == 0)
                    {
                        ReturnValue = false;
                    }
                    else
                    {
                        ReturnValue = true;
                    }
                });

            return ReturnValue;
        }

        /// <summary>
        /// Finds and returns a BankingDetailsRow with given BankingDetailsKey
        /// </summary>
        /// <param name="ABankingDetailsKey"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PBankingDetailsTable GetBankingDetailsRow(int ABankingDetailsKey)
        {
            PBankingDetailsTable ReturnRow = null;

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    ReturnRow = PBankingDetailsAccess.LoadByPrimaryKey(ABankingDetailsKey, Transaction);
                });

            return ReturnRow;
        }

        /// <summary>
        /// Finds any Partners that share ABankingDetailsKey
        /// </summary>
        /// <param name="ABankingDetailsKey"></param>
        /// <param name="APartnerKey">Partner key for current partner</param>
        /// <returns>Table containing records of Partners that share this Bank Account</returns>
        [RequireModulePermission("PTNRUSER")]
        public static PPartnerTable SharedBankAccountPartners(int ABankingDetailsKey, long APartnerKey)
        {
            PPartnerTable PartnerTable = new PPartnerTable();

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    PPartnerBankingDetailsTable PartnerBankingDetailsTable = PPartnerBankingDetailsAccess.LoadViaPBankingDetails(ABankingDetailsKey,
                        Transaction);

                    foreach (PPartnerBankingDetailsRow Row in PartnerBankingDetailsTable.Rows)
                    {
                        // if record exists with a different partner key then the Bank Account is shared
                        if (Row.PartnerKey != APartnerKey)
                        {
                            PPartnerRow PartnerRow = (PPartnerRow)PPartnerAccess.LoadByPrimaryKey(Row.PartnerKey, Transaction).Rows[0];

                            PPartnerRow NewRow = PartnerTable.NewRowTyped(false);
                            NewRow.PartnerKey = Row.PartnerKey;
                            NewRow.PartnerShortName = PartnerRow.PartnerShortName;
                            PartnerTable.Rows.Add(NewRow);
                        }
                    }
                });

            return PartnerTable;
        }

        /// <summary>
        /// Gets all Bank records.
        /// </summary>
        /// <returns>Dataset containing data for all Bank Partners.</returns>
        [RequireModulePermission("PTNRUSER")]
        public static BankTDS GetPBankRecords()
        {
            BankTDS ReturnValue = new BankTDS();

            TDBTransaction ReadTransaction = null;

            // Automatic handling of a Read-only DB Transaction - and also the automatic establishment and closing of a DB
            // Connection where a DB Transaction can be exectued (only if that should be needed).
            DBAccess.SimpleAutoReadTransactionWrapper(
                IsolationLevel.ReadCommitted,
                "TPartnerDataReaderWebConnector.GetPBankRecords",
                out ReadTransaction,
                delegate
                {
                    const string QUERY_BANKRECORDS = "SELECT PUB_p_bank.*, PUB_p_partner.p_status_code_c, PUB_p_location.* " +
                                                     "FROM PUB_p_bank JOIN PUB_p_partner ON PUB_p_partner.p_partner_key_n = PUB_p_bank.p_partner_key_n "
                                                     +
                                                     "LEFT OUTER JOIN PUB_p_partner_location ON PUB_p_bank.p_partner_key_n = PUB_p_partner_location.p_partner_key_n "
                                                     +
                                                     "AND (PUB_p_partner_location.p_date_good_until_d IS NULL OR PUB_p_partner_location.p_date_good_until_d >= DATE(NOW())) "
                                                     +
                                                     "JOIN PUB_p_location ON PUB_p_partner_location.p_site_key_n = PUB_p_location.p_site_key_n " +
                                                     "AND PUB_p_partner_location.p_location_key_i = PUB_p_location.p_location_key_i";

                    DBAccess.GetDBAccessObj(ReadTransaction).Select(ReturnValue, QUERY_BANKRECORDS, ReturnValue.PBank.TableName,
                        ReadTransaction, null);

                    foreach (BankTDSPBankRow Row in ReturnValue.PBank.Rows)
                    {
                        // mark inactive bank accounts
                        if (Row.StatusCode != SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE))
                        {
                            Row.BranchCode = SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " " + Row.BranchCode;
                        }
                    }
                });

            return ReturnValue;
        }

        /// <summary>
        /// Gets the next available key for PPartnerGiftDestination
        /// </summary>
        /// <returns>The next available key</returns>
        [RequireModulePermission("PTNRUSER")]
        public static int GetNewKeyForPartnerGiftDestination()
        {
            return GetNewKeyForPartnerGiftDestination(null);
        }

        /// <summary>
        /// Gets the next available key for PPartnerGiftDestination
        /// </summary>
        /// <param name="ADBTransaction">Transaction (if already exists in caller method)</param>
        /// <returns>The next available key</returns>
        internal static int GetNewKeyForPartnerGiftDestination(TDBTransaction ADBTransaction)
        {
            int ReturnValue = 0;

            DBAccess.GetDBAccessObj(ADBTransaction).GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadUncommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ADBTransaction,
                delegate
                {
                    PPartnerGiftDestinationTable Table = PPartnerGiftDestinationAccess.LoadAll(ADBTransaction);

                    foreach (PPartnerGiftDestinationRow Row in Table.Rows)
                    {
                        if (Row.Key >= ReturnValue)
                        {
                            ReturnValue = Row.Key + 1;
                        }
                    }
                });

            return ReturnValue;
        }
    }
}