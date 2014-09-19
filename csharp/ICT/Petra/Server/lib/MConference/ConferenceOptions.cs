//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using System.Collections.Specialized;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MConference.WebConnectors
{
    /// <summary>
    /// Description of ConferenceOptions.
    /// </summary>
    public class TConferenceOptions
    {
        private const int SELECTION = 0;
        private const int UNIT_KEY = 1;
        private const int UNIT_NAME = 2;
        private const int CAMPAIGN_CODE = 3;
        private const int USED_IN_CONFERENCE = 4;

        /// <summary>
        /// Get the units which start with the same outreach code as the given unit.
        /// </summary>
        /// <param name="AUnitKey">The unit which defines the outreach code</param>
        /// <returns>A table with all the relevant units</returns>
        [RequireModulePermission("CONFERENCE")]
        public static PUnitTable GetOutreachOptions(Int64 AUnitKey)
        {
            String ConferenceCodePrefix = "";
            PUnitTable UnitTable = new PUnitTable();
            PUnitRow TemplateRow = UnitTable.NewRowTyped(false);
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            TLogging.LogAtLevel(9, "TConferenceOptions.GetOutreachOptions called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                /* Load data */
                UnitTable = PUnitAccess.LoadByPrimaryKey(AUnitKey, ReadTransaction);

                if (UnitTable.Rows.Count > 0)
                {
                    String ConferenceCode = ((PUnitRow)UnitTable.Rows[0]).OutreachCode;

                    if (ConferenceCode.Length >= 5)
                    {
                        ConferenceCodePrefix = ConferenceCode.Substring(0, 5) + "%";
                    }

                    StringCollection operators = new StringCollection();
                    operators.Add("LIKE");
                    TemplateRow.OutreachCode = ConferenceCodePrefix;

                    UnitTable = PUnitAccess.LoadUsingTemplate(TemplateRow, operators, null, ReadTransaction);
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceOptions.GetOutreachOptions: committed own transaction.");
                }
            }
            return UnitTable;
        }

        /// <summary>
        /// Get the conferences which are set up in the system.
        /// If no prefix and conference name is given, return all of them.
        /// Otherwise only the conferences that start with the given parameters are returned.
        /// </summary>
        /// <param name="AConferenceName">Matching patterns for Unit Name</param>
        /// <param name="APrefix">Matching pattern for outreach code</param>
        /// <returns>A dataset with all the conferences in question</returns>
        [RequireModulePermission("CONFERENCE")]
        public static SelectConferenceTDS GetConferences(String AConferenceName, String APrefix)
        {
            SelectConferenceTDS ResultTable = new SelectConferenceTDS();

            PcConferenceTable ConferenceTable = new PcConferenceTable();
            PcConferenceRow TemplateRow = (PcConferenceRow)ConferenceTable.NewRow();

            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            if (APrefix == "*")
            {
                APrefix = "";
            }

            if (AConferenceName == "*")
            {
                AConferenceName = "";
            }
            else if (AConferenceName.EndsWith("*"))
            {
                AConferenceName = AConferenceName.Substring(0, AConferenceName.Length - 1);
            }

            TLogging.LogAtLevel(9, "TConferenceOptions.GetConferences called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                /* Load data */

                if (APrefix.Length > 0)
                {
                    APrefix = APrefix.Replace('*', '%') + "%";
                    TemplateRow.OutreachPrefix = APrefix;

                    StringCollection Operators = new StringCollection();
                    Operators.Add("LIKE");

                    ConferenceTable = PcConferenceAccess.LoadUsingTemplate(TemplateRow, Operators, null, ReadTransaction);
                }
                else
                {
                    ConferenceTable = PcConferenceAccess.LoadAll(ReadTransaction);
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceOptions.GetConferences: committed own transaction.");
                }
            }

            String ShortName;
            TPartnerClass PartnerClass;

            foreach (PcConferenceRow ConferenceRow in ConferenceTable.Rows)
            {
                TPartnerServerLookups.GetPartnerShortName(ConferenceRow.ConferenceKey, out ShortName, out PartnerClass);

                if ((AConferenceName.Length > 0)
                    && (!ShortName.StartsWith(AConferenceName, true, null)))
                {
                    continue;
                }

                ResultTable.PcConference.ImportRow(ConferenceRow);

                DataRow NewRow = ResultTable.PPartner.NewRow();
                NewRow[PPartnerTable.GetPartnerShortNameDBName()] = ShortName;
                NewRow[PPartnerTable.GetPartnerKeyDBName()] = ConferenceRow.ConferenceKey;

                ResultTable.PPartner.Rows.Add(NewRow);
            }

            return ResultTable;
        }

        /// <summary>
        /// Get the earlies arrival and latest departure date of a conference.
        /// If the conference key is -1 get it from all conferences.
        /// </summary>
        /// <param name="AConferenceKey">Unit Key of the conference</param>
        /// <param name="AEarliestArrivalDate">Earliest arrival date to the conference</param>
        /// <param name="ALatestDepartureDate">Latest departure date from the conference</param>
        /// <param name="AStartDate">Start Date of the Conference</param>
        /// <param name="AEndDate">End Date of the Conference</param>
        /// <returns>true if successful</returns>
        [RequireModulePermission("CONFERENCE")]
        public static bool GetEarliestAndLatestDate(Int64 AConferenceKey, out DateTime AEarliestArrivalDate,
            out DateTime ALatestDepartureDate, out DateTime AStartDate, out DateTime AEndDate)
        {
            AEarliestArrivalDate = DateTime.Today;
            ALatestDepartureDate = DateTime.Today;
            AStartDate = DateTime.Today;
            AEndDate = DateTime.Today;
            PmShortTermApplicationTable ShortTermerTable;
            PcConferenceTable ConferenceTable;

            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            TLogging.LogAtLevel(9, "TConferenceOptions.GetEarliestAndLatestDates called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                /* Load data */
                if (AConferenceKey == -1)
                {
                    ShortTermerTable = PmShortTermApplicationAccess.LoadAll(ReadTransaction);
                    ConferenceTable = PcConferenceAccess.LoadAll(ReadTransaction);
                }
                else
                {
                    ShortTermerTable = PmShortTermApplicationAccess.LoadViaPUnitStConfirmedOption(AConferenceKey, ReadTransaction);
                    ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(AConferenceKey, ReadTransaction);
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceOptions.GetEarliestAndLatestDates: committed own transaction.");
                }
            }

            DateTime TmpEarliestArrivalTime = DateTime.MaxValue;
            DateTime TmpLatestDepartureTime = DateTime.MinValue;
            DateTime TmpStartTime = DateTime.MaxValue;
            DateTime TmpEndTime = DateTime.MinValue;

            foreach (PmShortTermApplicationRow ShortTermerRow in ShortTermerTable.Rows)
            {
                if ((!ShortTermerRow.IsArrivalNull())
                    && (ShortTermerRow.Arrival < TmpEarliestArrivalTime))
                {
                    TmpEarliestArrivalTime = ShortTermerRow.Arrival.Value;
                }

                if ((!ShortTermerRow.IsDepartureNull())
                    && (ShortTermerRow.Departure > TmpLatestDepartureTime))
                {
                    TmpLatestDepartureTime = ShortTermerRow.Departure.Value;
                }
            }

            foreach (PcConferenceRow ConferenceRow in ConferenceTable.Rows)
            {
                if ((!ConferenceRow.IsStartNull())
                    && (ConferenceRow.Start.Value < TmpStartTime))
                {
                    TmpStartTime = ConferenceRow.Start.Value;
                }

                if ((!ConferenceRow.IsEndNull())
                    && (ConferenceRow.End.Value > TmpEndTime))
                {
                    TmpEndTime = ConferenceRow.End.Value;
                }
            }

            if (TmpEarliestArrivalTime != DateTime.MaxValue)
            {
                AEarliestArrivalDate = TmpEarliestArrivalTime;
            }

            if (TmpLatestDepartureTime != DateTime.MinValue)
            {
                ALatestDepartureDate = TmpLatestDepartureTime;
            }

            if (TmpStartTime != DateTime.MaxValue)
            {
                AStartDate = TmpStartTime;
            }

            if (TmpEndTime != DateTime.MinValue)
            {
                AEndDate = TmpEndTime;
            }

            return true;
        }

        /// <summary>
        /// Get the units which start with the same outreach code as given with the prefix.
        /// </summary>
        /// <param name="AUnitKey">Partner Key of the unit from which the outreach options are retrieved</param>
        /// <param name="AConferenceTable">A table with all the units</param>
        /// <returns></returns>
        [RequireModulePermission("CONFERENCE")]
        public static System.Boolean GetOutreachOptions(long AUnitKey,
            out System.Data.DataTable AConferenceTable)
        {
            AConferenceTable = new DataTable();
            AConferenceTable.Columns.Add("Partner Key", Type.GetType("System.Int64"));
            AConferenceTable.Columns.Add("Outreach Code");
            AConferenceTable.Columns.Add("Unit Name");

            String ConferenceCodePrefix = "";
            PUnitTable UnitTable = new PUnitTable();
            PUnitRow TemplateRow = UnitTable.NewRowTyped(false);

            TLogging.LogAtLevel(9, "TConferenceOptions.GetOutreachOptions called!");

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(ref Transaction,
                delegate
                {
                    /* Load data */
                    UnitTable = PUnitAccess.LoadByPrimaryKey(AUnitKey, Transaction);

                    if (UnitTable.Rows.Count > 0)
                    {
                        ConferenceCodePrefix = ((PUnitRow)UnitTable.Rows[0]).OutreachCode.Substring(0, 5);

                        UnitTable = PUnitAccess.LoadUsingTemplate(TemplateRow, null, null, Transaction);
                    }
                });

            foreach (PUnitRow UnitRow in UnitTable.Rows)
            {
                if (!UnitRow.OutreachCode.StartsWith(ConferenceCodePrefix, true, null))
                {
                    continue;
                }

                DataRow NewRow = AConferenceTable.NewRow();

                NewRow["Partner Key"] = UnitRow.PartnerKey;
                NewRow["Outreach Code"] = UnitRow.OutreachCode;
                NewRow["Unit Name"] = UnitRow.UnitName;

                AConferenceTable.Rows.Add(NewRow);
            }

            return true;
        }

        /// <summary>
        /// Get specific fields which are related to a conference. AFieldTypes defines which
        /// fields to retrieve.
        /// </summary>
        /// <param name="AConferenceKey">Unit Key of the conference. If it is -1
        /// all fields will be returned that relate to any confernce.</param>
        /// <param name="AFieldTypes">Defines the type of the fields to retrieve</param>
        /// <param name="AFieldsTable">A list of units that relate in to the conference.
        /// Column 0 is the Unit key
        /// Column 1 is the Unit name
        /// Column 2 is the Outreach Code
        /// Column 3 indicates if the unit is directly used by the current conference</param>
        /// <param name="AConferencePrefix">The prefix code of the conference</param>
        /// <returns>True if successful. Otherwise false</returns>
        [RequireModulePermission("CONFERENCE")]
        public static bool GetFieldUnits(Int64 AConferenceKey, TUnitTypeEnum AFieldTypes, out DataTable AFieldsTable, out String AConferencePrefix)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            AFieldsTable = new DataTable("Field Units");
            AConferencePrefix = "";
            PUnitTable UnitTable;
            DataTable TmpTable;

            PmShortTermApplicationTable ShortTermerTable = new PmShortTermApplicationTable();

            AFieldsTable.Columns.Add("Selection", System.Type.GetType("System.Boolean"));
            AFieldsTable.Columns.Add("Unit Key", System.Type.GetType("System.Int64"));
            AFieldsTable.Columns.Add("Unit Name", System.Type.GetType("System.String"));
            AFieldsTable.Columns.Add("Outreach Code", System.Type.GetType("System.String"));
            AFieldsTable.Columns.Add("Used_in_Conference", System.Type.GetType("System.Boolean"));

            AConferencePrefix = TConferenceOptions.GetConferencePrefix(AConferenceKey);

            switch (AFieldTypes)
            {
                case TUnitTypeEnum.utSendingFields:
                    return TConferenceOptions.GetSendingFields(AConferenceKey, ref AFieldsTable);

                case TUnitTypeEnum.utReceivingFields:
                    return TConferenceOptions.GetReceivingFields(AConferenceKey, ref AFieldsTable);

                case TUnitTypeEnum.utOutreachOptions:

                    if (TConferenceOptions.GetOutreachOptions(AConferenceKey, out TmpTable))
                    {
                        foreach (DataRow Row in TmpTable.Rows)
                        {
                            DataRow NewRow = AFieldsTable.NewRow();

                            NewRow[SELECTION] = false;
                            NewRow[UNIT_KEY] = Row["Partner Key"];
                            NewRow[UNIT_NAME] = Row["Unit Name"];
                            NewRow[CAMPAIGN_CODE] = Row["Outreach Code"];
                            NewRow[USED_IN_CONFERENCE] = true;

                            AFieldsTable.Rows.Add(NewRow);
                        }

                        return true;
                    }

                    return false;

                default:
                    break;
            }

            TLogging.LogAtLevel(9, "TConferenceOptions.GetFieldUnits called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                StringCollection FieldList = new StringCollection();
                FieldList.Add(PmShortTermApplicationTable.GetStFieldChargedDBName());
                FieldList.Add(PmShortTermApplicationTable.GetConfirmedOptionCodeDBName());
                FieldList.Add(PmShortTermApplicationTable.GetRegistrationOfficeDBName());

                ShortTermerTable = PmShortTermApplicationAccess.LoadAll(FieldList, ReadTransaction);

                long LastUnitKey = 0;
                long NewUnitKey = 0;
                bool IsUsedInOneConference = false;

                String ConfirmedOptionCode = "";
                System.Type StringType = System.Type.GetType("System.String");

                String SearchedColumnName = "";

                switch (AFieldTypes)
                {
                    case TUnitTypeEnum.utChargedFields:
                        SearchedColumnName = PmShortTermApplicationTable.GetStFieldChargedDBName();
                        break;

                    case TUnitTypeEnum.utRegisteringFields:
                        SearchedColumnName = PmShortTermApplicationTable.GetRegistrationOfficeDBName();
                        break;

                    default:
                        break;
                }

                foreach (DataRow ShortTermerRow in ShortTermerTable.Select("", SearchedColumnName))
                {
                    if ((ShortTermerRow[SearchedColumnName] != null)
                        && (ShortTermerRow[SearchedColumnName].ToString().Length > 0))
                    {
                        NewUnitKey = (long)ShortTermerRow[SearchedColumnName];
                    }
                    else
                    {
                        continue;
                    }

                    if (LastUnitKey != NewUnitKey)
                    {
                        if ((AFieldsTable.Rows.Count > 0)
                            && (IsUsedInOneConference))
                        {
                            AFieldsTable.Rows[AFieldsTable.Rows.Count - 1][USED_IN_CONFERENCE] = true;
                        }

                        IsUsedInOneConference = false;
                    }

                    // We have to check from every shorttermer if the charged field is used
                    // in this conference
                    if (IsUsedInOneConference)
                    {
                        continue;
                    }

                    if (ShortTermerRow[PmShortTermApplicationTable.GetConfirmedOptionCodeDBName()].GetType() == StringType)
                    {
                        ConfirmedOptionCode = (string)ShortTermerRow[PmShortTermApplicationTable.GetConfirmedOptionCodeDBName()];
                    }
                    else
                    {
                        ConfirmedOptionCode = "";
                    }

                    if (ConfirmedOptionCode.StartsWith(AConferencePrefix))
                    {
                        IsUsedInOneConference = true;
                    }

                    if (LastUnitKey == NewUnitKey)
                    {
                        continue;
                    }

                    UnitTable = PUnitAccess.LoadByPrimaryKey(NewUnitKey, ReadTransaction);

                    if (UnitTable.Rows.Count > 0)
                    {
                        DataRow ResultRow = AFieldsTable.NewRow();

                        ResultRow[SELECTION] = false;
                        ResultRow[UNIT_KEY] = NewUnitKey;
                        ResultRow[UNIT_NAME] = UnitTable[0][PUnitTable.GetUnitNameDBName()];
                        ResultRow[CAMPAIGN_CODE] = ConfirmedOptionCode;
                        ResultRow[USED_IN_CONFERENCE] = IsUsedInOneConference;

                        AFieldsTable.Rows.Add(ResultRow);
                        LastUnitKey = NewUnitKey;
                    }
                }

                // Check for the previous entry the "IsUsedInConference" field
                if ((AFieldsTable.Rows.Count > 0)
                    && (IsUsedInOneConference))
                {
                    AFieldsTable.Rows[AFieldsTable.Rows.Count - 1][USED_IN_CONFERENCE] = true;
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceOptions.GetFieldUnits: committed own transaction.");
                }
            }
            return true;
        }

        private static bool GetSendingFields(long AConferenceKey, ref DataTable AFieldsTable)
        {
            if (AConferenceKey == -1)
            {
                return GetAllSendingFields(AConferenceKey, ref AFieldsTable);
            }

            return GetSendingFieldsForOneConference(AConferenceKey, ref AFieldsTable);
        }

        private static bool GetSendingFieldsForOneConference(long AConferenceKey, ref DataTable AFieldsTable)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            PUnitTable UnitTable;

            PcAttendeeTable AttendeeTable = new PcAttendeeTable();

            TLogging.LogAtLevel(9, "TConferenceOptions.GetSendingFieldsForOneConference called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                StringCollection FieldList = new StringCollection();
                FieldList.Add(PcAttendeeTable.GetHomeOfficeKeyDBName());

                AttendeeTable = PcAttendeeAccess.LoadViaPcConference(AConferenceKey, FieldList, ReadTransaction);

                long LastUnitKey = 0;
                long NewUnitKey = 0;

                String HomeOfficeColumnName = PcAttendeeTable.GetHomeOfficeKeyDBName();

                foreach (DataRow AttendeeRow in AttendeeTable.Select("", HomeOfficeColumnName))
                {
                    if (AttendeeRow[HomeOfficeColumnName] != null)
                    {
                        NewUnitKey = (long)AttendeeRow[HomeOfficeColumnName];
                    }
                    else
                    {
                        continue;
                    }

                    if (LastUnitKey == NewUnitKey)
                    {
                        continue;
                    }

                    UnitTable = PUnitAccess.LoadByPrimaryKey(NewUnitKey, ReadTransaction);

                    if (UnitTable.Rows.Count > 0)
                    {
                        DataRow ResultRow = AFieldsTable.NewRow();

                        ResultRow[SELECTION] = false;
                        ResultRow[UNIT_KEY] = NewUnitKey;
                        ResultRow[UNIT_NAME] = UnitTable[0][PUnitTable.GetUnitNameDBName()];
                        ResultRow[USED_IN_CONFERENCE] = true;

                        AFieldsTable.Rows.Add(ResultRow);
                        LastUnitKey = NewUnitKey;
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceOptions.GetSendingFieldsForOneConference: committed own transaction.");
                }
            }
            return true;
        }

        private static bool GetAllSendingFields(long AConferenceKey, ref DataTable AFieldsTable)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            PUnitTable UnitTable;

            PcAttendeeTable AttendeeTable = new PcAttendeeTable();

            TLogging.LogAtLevel(9, "TConferenceOptions.GetAllSendingFields called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                StringCollection FieldList = new StringCollection();
                FieldList.Add(PcAttendeeTable.GetHomeOfficeKeyDBName());

                AttendeeTable = PcAttendeeAccess.LoadAll(FieldList, ReadTransaction);

                long LastUnitKey = 0;
                long NewUnitKey = 0;

                String HomeOfficeColumnName = PcAttendeeTable.GetHomeOfficeKeyDBName();

                foreach (DataRow AttendeeRow in AttendeeTable.Select("", HomeOfficeColumnName))
                {
                    if (AttendeeRow[HomeOfficeColumnName] != null)
                    {
                        NewUnitKey = (long)AttendeeRow[HomeOfficeColumnName];
                    }
                    else
                    {
                        continue;
                    }

                    if (LastUnitKey == NewUnitKey)
                    {
                        continue;
                    }

                    UnitTable = PUnitAccess.LoadByPrimaryKey(NewUnitKey, ReadTransaction);

                    if (UnitTable.Rows.Count > 0)
                    {
                        DataRow ResultRow = AFieldsTable.NewRow();

                        ResultRow[SELECTION] = false;
                        ResultRow[UNIT_KEY] = NewUnitKey;
                        ResultRow[UNIT_NAME] = UnitTable[0][PUnitTable.GetUnitNameDBName()];
                        ResultRow[USED_IN_CONFERENCE] = true;

                        AFieldsTable.Rows.Add(ResultRow);
                        LastUnitKey = NewUnitKey;
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceOptions.GetAllSendingFields: committed own transaction.");
                }
            }
            return true;
        }

        private static bool GetReceivingFields(long AConferenceKey, ref DataTable AFieldsTable)
        {
            if (AConferenceKey == -1)
            {
                return GetAllReceivingFields(AConferenceKey, ref AFieldsTable);
            }

            return GetReceivingFieldsForOneConference(AConferenceKey, ref AFieldsTable);
        }

        private static bool GetReceivingFieldsForOneConference(long AConferenceKey, ref DataTable AFieldsTable)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            TLogging.LogAtLevel(9, "TConferenceOptions.GetReceivingFieldsForOneConference called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                String PartnerKeyDBName = PcAttendeeTable.GetPartnerKeyDBName();
                PcAttendeeTable AttendeeTable;
                StringCollection FieldList = new StringCollection();
                FieldList.Add(PartnerKeyDBName);
                AttendeeTable = PcAttendeeAccess.LoadViaPcConference(AConferenceKey, FieldList, ReadTransaction);

                foreach (DataRow Row in AttendeeTable.Rows)
                {
                    long PartnerKey = (long)Row[PartnerKeyDBName];

                    GetReceivingFieldFromGiftDestination(PartnerKey, ref AFieldsTable);
                    GetReceivingFieldFromShortTermTable(PartnerKey, ref AFieldsTable);
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceOptions.GetReceivingFieldsForOneConference: committed own transaction.");
                }
            }
            return true;
        }

        private static bool GetAllReceivingFields(long AConferenceKey, ref DataTable AFieldsTable)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            TLogging.LogAtLevel(9, "TConferenceOptions.GetAllReceivingFields called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                OdbcParameter[] ParametersArray;

                ParametersArray = new OdbcParameter[0];

                DataSet TmpDS = DBAccess.GDBAccessObj.Select(
                    "SELECT PUB_" + PUnitTable.GetTableDBName() + '.' + PUnitTable.GetPartnerKeyDBName() +
                    ", PUB_" + PUnitTable.GetTableDBName() + '.' + PUnitTable.GetUnitNameDBName() +
                    " FROM PUB_" + PUnitTable.GetTableDBName() +
                    ", PUB_" + PPartnerTable.GetTableDBName() + ", " +
                    "PUB_" + PPartnerTypeTable.GetTableDBName() +
                    " WHERE PUB_" + PUnitTable.GetTableDBName() + '.' + PUnitTable.GetPartnerKeyDBName() +
                    " = PUB_" + PPartnerTable.GetTableDBName() + '.' + PPartnerTable.GetPartnerKeyDBName() +
                    " AND PUB_" + PPartnerTypeTable.GetTableDBName() + '.' + PPartnerTypeTable.GetPartnerKeyDBName() +
                    " = PUB_" + PPartnerTable.GetTableDBName() + '.' + PPartnerTable.GetPartnerKeyDBName() +
                    " AND PUB_" + PPartnerTable.GetTableDBName() + '.' + PPartnerTable.GetStatusCodeDBName() + " = \"ACTIVE\"" +
                    " AND PUB_" + PPartnerTypeTable.GetTableDBName() + '.' + PPartnerTypeTable.GetTypeCodeDBName() + " = \"LEDGER\"" +
                    " ORDER BY PUB_" + PUnitTable.GetTableDBName() + '.' + PUnitTable.GetUnitNameDBName() + " ASC",
                    "TempTable", ReadTransaction, ParametersArray);

                DataTable ResultTale = TmpDS.Tables[0];

                for (int Counter = 0; Counter < ResultTale.Rows.Count; ++Counter)
                {
                    DataRow NewRow = AFieldsTable.NewRow();

                    NewRow[SELECTION] = false;
                    NewRow[UNIT_KEY] = ResultTale.Rows[Counter][PUnitTable.GetPartnerKeyDBName()];
                    NewRow[UNIT_NAME] = ResultTale.Rows[Counter][PUnitTable.GetUnitNameDBName()];
                    NewRow[USED_IN_CONFERENCE] = true;

                    AFieldsTable.Rows.Add(NewRow);
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceOptions.GetReceivingFields: committed own transaction.");
                }
            }
            return true;
        }

        /// <summary>
        /// Adds the OMer field from the person and family record of the partner
        /// to the data table if it is not already there.
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AFieldsTable"></param>
        /// <returns></returns>
        private static bool GetReceivingFieldFromGiftDestination(long APartnerKey, ref DataTable AFieldsTable)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            TLogging.LogAtLevel(9, "TConferenceOptions.GetReceivingFieldFromGiftDestination called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                PPersonTable PersonTable;
                PPartnerGiftDestinationTable GiftDestinationTable;
                long FamilyKey = APartnerKey;

                PersonTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                if (PersonTable.Rows.Count > 0)
                {
                    PPersonRow PersonRow = (PPersonRow)PersonTable[0];

                    FamilyKey = PersonRow.FamilyKey;
                }

                GiftDestinationTable = PPartnerGiftDestinationAccess.LoadViaPPartner(FamilyKey, ReadTransaction);

                if (GiftDestinationTable.Rows.Count > 0)
                {
                    foreach (PPartnerGiftDestinationRow Row in GiftDestinationTable.Rows)
                    {
                        // check if the gift destination is currently active
                        if ((Row.DateEffective <= DateTime.Today)
                            && (Row.IsDateExpiresNull() || ((Row.DateExpires >= DateTime.Today) && (Row.DateExpires != Row.DateEffective))))
                        {
                            AddFieldToTable(Row.FieldKey, ref AFieldsTable, ref ReadTransaction);
                        }
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceOptions.GetReceivingFieldFromPartnerTable: committed own transaction.");
                }
            }
            return true;
        }

        /// <summary>
        /// Adds the confirmed option code to the data table, using the values
        /// from the shorttermtable of the current partner
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AFieldsTable"></param>
        /// <returns></returns>
        private static bool GetReceivingFieldFromShortTermTable(long APartnerKey, ref DataTable AFieldsTable)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            TLogging.LogAtLevel(9, "TConferenceOptions.GetReceivingFieldFromShortTermTable called!");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                PmShortTermApplicationTable ShortTermTable;

                ShortTermTable = PmShortTermApplicationAccess.LoadViaPPerson(APartnerKey, ReadTransaction);

                foreach (PmShortTermApplicationRow Row in ShortTermTable.Rows)
                {
                    if (!Row.IsStConfirmedOptionNull())
                    {
                        AddFieldToTable(Row.StConfirmedOption, ref AFieldsTable, ref ReadTransaction);
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceOptions.GetReceivingFieldFromShortTermTable: committed own transaction.");
                }
            }
            return true;
        }

        private static bool AddFieldToTable(long AFieldKey, ref DataTable AFieldsTable, ref TDBTransaction ATransaction)
        {
            // First check if the unit is already in the table
            foreach (DataRow Row in AFieldsTable.Rows)
            {
                if ((long)Row[UNIT_KEY] == AFieldKey)
                {
                    return true;
                }
            }

            bool IsLedger = false;

            PUnitTable UnitTable;
            PPartnerTypeTable PartnerTypeTable;

            StringCollection FieldList = new StringCollection();

            FieldList.Add(PUnitTable.GetUnitNameDBName());

            UnitTable = PUnitAccess.LoadByPrimaryKey(AFieldKey, FieldList, ATransaction);
            PartnerTypeTable = PPartnerTypeAccess.LoadViaPPartner(AFieldKey, ATransaction);

            foreach (PPartnerTypeRow PartnerTypeRow in PartnerTypeTable.Rows)
            {
                if (PartnerTypeRow.TypeCode == "LEDGER")
                {
                    IsLedger = true;
                    break;
                }
            }

            if (!IsLedger)
            {
                return false;
            }

            if (UnitTable.Rows.Count > 0)
            {
                DataRow NewRow = AFieldsTable.NewRow();

                NewRow[SELECTION] = false;
                NewRow[UNIT_KEY] = AFieldKey;
                NewRow[UNIT_NAME] = UnitTable.Rows[0][PUnitTable.GetUnitNameDBName()];
                NewRow[USED_IN_CONFERENCE] = true;

                AFieldsTable.Rows.Add(NewRow);
            }

            return true;
        }

        private static String GetConferencePrefix(long AConferenceKey)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            String ConferencePrefix = "-----";
            PUnitTable UnitTable;

            TLogging.LogAtLevel(9, "TConferenceOptions.GetOutreachPrefix: called.");

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                UnitTable = PUnitAccess.LoadByPrimaryKey(AConferenceKey, ReadTransaction);

                if (UnitTable.Rows.Count > 0)
                {
                    if (UnitTable.Rows[0][PUnitTable.GetOutreachCodeDBName()] != System.DBNull.Value)
                    {
                        ConferencePrefix = (string)UnitTable.Rows[0][PUnitTable.GetOutreachCodeDBName()];

                        if (ConferencePrefix.Length > 5)
                        {
                            ConferencePrefix = ConferencePrefix.Substring(0, 5);
                        }
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceOptions.GetOutreachPrefix: committed own transaction.");
                }
            }

            return ConferencePrefix;
        }
    }
}