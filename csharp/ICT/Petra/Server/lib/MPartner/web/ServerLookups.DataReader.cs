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
        /// <param name="AIncludeConferenceUnits">true if conference units are to be retrieved</param>
        /// <param name="AIncludeOutreachUnits">true if outreach units are to be retrieved</param>
        /// <param name="AEventName">match string for event name search</param>
        /// <param name="AIncludeLocationData">true if location columns need to be returned</param>
        /// <param name="ACurrentAndFutureEventsOnly">indicate if only current or future events are to be returned</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static DataTable GetEventUnits(bool AIncludeConferenceUnits, bool AIncludeOutreachUnits,
            string AEventName, bool AIncludeLocationData, bool ACurrentAndFutureEventsOnly)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();
            DataColumn[] Key = new DataColumn[1];
            DataTable Events = new DataTable();

            if (AEventName == "*")
            {
                AEventName = "";
            }
            else if (AEventName.EndsWith("*"))
            {
                AEventName = AEventName.Substring(0, AEventName.Length - 1);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine("GetEventUnits called!");
            }

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                string SqlStmt =
                    "SELECT DISTINCT " +
                    PPartnerTable.GetPartnerShortNameDBName() +
                    ", " + PPartnerTable.GetPartnerClassDBName() +
                    ", " + PUnitTable.GetOutreachCodeDBName();

                if (AIncludeLocationData || ACurrentAndFutureEventsOnly)
                {
                    SqlStmt = SqlStmt +
                              ", " + PCountryTable.GetTableDBName() + "." + PCountryTable.GetCountryNameDBName() +
                              ", " + PPartnerLocationTable.GetTableDBName() + "." + PPartnerLocationTable.GetDateEffectiveDBName() +
                              ", " + PPartnerLocationTable.GetTableDBName() + "." + PPartnerLocationTable.GetDateGoodUntilDBName();
                }

                SqlStmt = SqlStmt +
                          ", " + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName() +
                          ", " + PUnitTable.GetUnitTypeCodeDBName() +

                          " FROM pub_" + PPartnerTable.GetTableDBName() +
                          ", pub_" + PUnitTable.GetTableDBName();

                if (AIncludeLocationData || ACurrentAndFutureEventsOnly)
                {
                    SqlStmt = SqlStmt +
                              ", pub_" + PLocationTable.GetTableDBName() +
                              ", pub_" + PPartnerLocationTable.GetTableDBName() +
                              ", pub_" + PCountryTable.GetTableDBName();
                }

                SqlStmt = SqlStmt +
                          " WHERE " +
                          PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName() + " = " +
                          PUnitTable.GetTableDBName() + "." + PUnitTable.GetPartnerKeyDBName() + " AND ";

                if (AIncludeLocationData || ACurrentAndFutureEventsOnly)
                {
                    SqlStmt = SqlStmt +
                              PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName() + " = " +
                              PPartnerLocationTable.GetTableDBName() + "." + PPartnerLocationTable.GetPartnerKeyDBName() + " AND " +
                              PLocationTable.GetTableDBName() + "." + PLocationTable.GetSiteKeyDBName() + " = " +
                              PPartnerLocationTable.GetTableDBName() + "." + PPartnerLocationTable.GetSiteKeyDBName() + " AND " +
                              PLocationTable.GetTableDBName() + "." + PLocationTable.GetLocationKeyDBName() + " = " +
                              PPartnerLocationTable.GetTableDBName() + "." + PPartnerLocationTable.GetLocationKeyDBName() + " AND " +
                              PCountryTable.GetTableDBName() + "." + PCountryTable.GetCountryCodeDBName() + " = " +
                              PLocationTable.GetTableDBName() + "." + PLocationTable.GetCountryCodeDBName() + " AND ";
                }

                SqlStmt = SqlStmt +
                          PPartnerTable.GetStatusCodeDBName() + " = 'ACTIVE' " + " AND " +
                          PPartnerTable.GetPartnerClassDBName() + " = 'UNIT' ";

                // add criteria for conference and/or outreach
                String ConferenceWhereClause = "(" +
                                               PUnitTable.GetUnitTypeCodeDBName() + " LIKE '%CONF%' OR " +
                                               PUnitTable.GetUnitTypeCodeDBName() + " LIKE '%CONG%')";

                String OutreachWhereClause = PUnitTable.GetOutreachCodeDBName() + " IS NOT NULL AND " +
                                             PUnitTable.GetOutreachCodeDBName() + " <> '' AND (" +
                                             PUnitTable.GetUnitTypeCodeDBName() + " NOT LIKE '%CONF%' AND " +
                                             PUnitTable.GetUnitTypeCodeDBName() + " NOT LIKE '%CONG%')";

                if (AIncludeConferenceUnits
                    && AIncludeOutreachUnits)
                {
                    SqlStmt = SqlStmt + " AND ((" + ConferenceWhereClause + ") OR (" + OutreachWhereClause + "))";
                }
                else if (AIncludeConferenceUnits)
                {
                    SqlStmt = SqlStmt + " AND (" + ConferenceWhereClause + ")";
                }
                else if (AIncludeOutreachUnits)
                {
                    SqlStmt = SqlStmt + " AND (" + OutreachWhereClause + ")";
                }

                // add criteria for event name filter
                if (AEventName.Length > 0)
                {
                    // in case there is a filter set for the event name
                    AEventName = AEventName.Replace('*', '%') + "%";
                    SqlStmt = SqlStmt + " AND " + PUnitTable.GetUnitNameDBName() +
                              " LIKE '" + AEventName + "'";
                }

                if (ACurrentAndFutureEventsOnly)
                {
                    SqlStmt = SqlStmt + " AND " + PPartnerLocationTable.GetDateGoodUntilDBName() + " >= ?";

                    SqlParameterList.Add(new OdbcParameter("param_date", OdbcType.Date)
                        {
                            Value = DateTime.Today.Date
                        });
                }

                // sort rows according to name
                SqlStmt = SqlStmt + " ORDER BY " + PPartnerTable.GetPartnerShortNameDBName();

                Events = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "events",
                    ReadTransaction, SqlParameterList.ToArray());

                Key[0] = Events.Columns[PPartnerTable.GetPartnerKeyDBName()];
                Events.PrimaryKey = Key;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();

                    if (TLogging.DL >= 7)
                    {
                        Console.WriteLine("GetEventUnits: committed own transaction.");
                    }
                }
            }

            return Events;
        }

        /// <summary>
        /// Return unit table which contains fields (unit partners) that match a given search string and have
        /// status ACTIVE.
        /// Only returns key and name field at the moment.
        /// </summary>
        /// <param name="AFieldName">match string for field name search</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PUnitTable GetActiveFieldUnits(string AFieldName)
        {
            PUnitTable UnitTable = new PUnitTable();
            PUnitRow UnitRow;

            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            if (AFieldName == "*")
            {
                AFieldName = "";
            }
            else if (AFieldName.EndsWith("*"))
            {
                AFieldName = AFieldName.Substring(0, AFieldName.Length - 1);
            }

            TLogging.LogAtLevel(9, "TPartnerDataReaderWebConnector.GetActiveFieldUnits called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
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

                if (AFieldName.Length > 0)
                {
                    // in case there is a filter set for the event name
                    AFieldName = AFieldName.Replace('*', '%') + "%";
                    SqlStmt = SqlStmt + " AND " + PUnitTable.GetUnitNameDBName() +
                              " LIKE '" + AFieldName + "'";
                }

                // sort rows according to name
                SqlStmt = SqlStmt + " ORDER BY " + PUnitTable.GetUnitNameDBName();

                DataTable events = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "fields", ReadTransaction);

                foreach (DataRow eventRow in events.Rows)
                {
                    UnitRow = (PUnitRow)UnitTable.NewRow();
                    UnitRow.PartnerKey = Convert.ToInt64(eventRow[0]);
                    UnitRow.UnitName = Convert.ToString(eventRow[1]);
                    UnitTable.Rows.Add(UnitRow);
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
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerDataReaderWebConnector.GetActiveFieldUnits: committed own transaction.");
                }
            }

            return UnitTable;
        }

        /// <summary>
        /// Return unit table which contains ledger records (unit partners) that match a given search string
        /// Only returns key and name field at the moment.
        /// </summary>
        /// <param name="ALedgerName">match string for ledger name search</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PUnitTable GetLedgerUnits(string ALedgerName)
        {
            PUnitTable UnitTable = new PUnitTable();
            PUnitRow UnitRow;

            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            if (ALedgerName == "*")
            {
                ALedgerName = "";
            }
            else if (ALedgerName.EndsWith("*"))
            {
                ALedgerName = ALedgerName.Substring(0, ALedgerName.Length - 1);
            }

            TLogging.LogAtLevel(9, "TPartnerDataReaderWebConnector.GetLedgerUnits called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                // Load data
                string SqlStmt = "SELECT pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetPartnerKeyDBName() +
                                 ", pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetUnitNameDBName() +
                                 " FROM " + PUnitTable.GetTableDBName() + ", " + PPartnerTypeTable.GetTableDBName() +
                                 " WHERE pub_" + PUnitTable.GetTableDBName() + "." + PUnitTable.GetPartnerKeyDBName() +
                                 " = pub_" + PPartnerTypeTable.GetTableDBName() + "." + PPartnerTypeTable.GetPartnerKeyDBName() +
                                 " AND " + PPartnerTypeTable.GetTypeCodeDBName() + " = '" + MPartnerConstants.PARTNERTYPE_LEDGER + "'";

                if (ALedgerName.Length > 0)
                {
                    // in case there is a filter set for the event name
                    ALedgerName = ALedgerName.Replace('*', '%') + "%";
                    SqlStmt = SqlStmt + " AND " + PUnitTable.GetUnitNameDBName() +
                              " LIKE '" + ALedgerName + "'";
                }

                // sort rows according to name
                SqlStmt = SqlStmt + " ORDER BY " + PUnitTable.GetUnitNameDBName();

                DataTable events = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "ledgers", ReadTransaction);

                foreach (DataRow eventRow in events.Rows)
                {
                    UnitRow = (PUnitRow)UnitTable.NewRow();
                    UnitRow.PartnerKey = Convert.ToInt64(eventRow[0]);
                    UnitRow.UnitName = Convert.ToString(eventRow[1]);
                    UnitTable.Rows.Add(UnitRow);
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
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerDataReaderWebConnector.GetLedgerUnits: committed own transaction.");
                }
            }

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
            Boolean NewTransaction;
            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                PcConferenceTable ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                if (ConferenceTable.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerDataReaderWebConnector.IsPUnitAConference: commit own transaction.");
                }
            }
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
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            try
            {
                ReturnRow = PBankingDetailsAccess.LoadByPrimaryKey(ABankingDetailsKey, ReadTransaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerDataReaderWebConnector.GetBankingDetailsRow: committed own transaction.");
                }
            }

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
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

            PPartnerTable PartnerTable = new PPartnerTable();

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);
            try
            {
                PPartnerBankingDetailsTable PartnerBankingDetailsTable = PPartnerBankingDetailsAccess.LoadViaPBankingDetails(ABankingDetailsKey,
                    ReadTransaction);

                foreach (PPartnerBankingDetailsRow Row in PartnerBankingDetailsTable.Rows)
                {
                    // if record exists with a different partner key then the Bank Account is shared
                    if (Row.PartnerKey != APartnerKey)
                    {
                        PPartnerRow PartnerRow = (PPartnerRow)PPartnerAccess.LoadByPrimaryKey(Row.PartnerKey, ReadTransaction).Rows[0];

                        PPartnerRow NewRow = PartnerTable.NewRowTyped(false);
                        NewRow.PartnerKey = Row.PartnerKey;
                        NewRow.PartnerShortName = PartnerRow.PartnerShortName;
                        PartnerTable.Rows.Add(NewRow);
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerDataReaderWebConnector.IsBankingDetailsRowShared: committed own transaction.");
                }
            }

            return PartnerTable;
        }

        /// <summary>
        /// Gets all Bank records
        /// </summary>
        /// <returns>Dataset containing data for all Bank partners</returns>
        [RequireModulePermission("PTNRUSER")]
        public static BankTDS GetPBankRecords()
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

            BankTDS ReturnValue = new BankTDS();

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);
            try
            {
                string QueryBankRecords =
                    "SELECT PUB_p_bank.*, PUB_p_partner.p_status_code_c, PUB_p_location.* " +
                    "FROM PUB_p_bank JOIN PUB_p_partner ON PUB_p_partner.p_partner_key_n = PUB_p_bank.p_partner_key_n " +
                    "LEFT OUTER JOIN PUB_p_partner_location ON PUB_p_bank.p_partner_key_n = PUB_p_partner_location.p_partner_key_n " + 
                	"AND (PUB_p_partner_location.p_date_good_until_d IS NULL OR PUB_p_partner_location.p_date_good_until_d >= DATE(NOW())) " +
                    "JOIN PUB_p_location ON PUB_p_partner_location.p_site_key_n = PUB_p_location.p_site_key_n " + 
                	"AND PUB_p_partner_location.p_location_key_i = PUB_p_location.p_location_key_i";

                DBAccess.GDBAccessObj.Select(ReturnValue,
                    QueryBankRecords,
                    ReturnValue.PBank.TableName, ReadTransaction, null);

                foreach (BankTDSPBankRow Row in ReturnValue.PBank.Rows)
                {
                    // mark inactive bank accounts
                    if (Row.StatusCode != SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE))
                    {
                        Row.BranchCode = SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " " + Row.BranchCode;
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.LogAtLevel(7, "TPartnerDataReaderWebConnector.GetPBankRecords: committed own transaction.");
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Gets the next available key for PPartnerGiftDestination
        /// </summary>
        /// <returns>The next available key</returns>
        [RequireModulePermission("PTNRUSER")]
        public static int GetNewKeyForPartnerGiftDestination()
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            int ReturnValue = 0;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);
            try
            {
                PPartnerGiftDestinationTable Table = PPartnerGiftDestinationAccess.LoadAll(ReadTransaction);

                foreach (PPartnerGiftDestinationRow Row in Table.Rows)
                {
                    if (Row.Key >= ReturnValue)
                    {
                        ReturnValue = Row.Key + 1;
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.LogAtLevel(7, "TPartnerDataReaderWebConnector.GetNewKeyForPartnerGiftDestination: committed own transaction.");
                }
            }

            return ReturnValue;
        }
    }
}