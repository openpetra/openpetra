//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using System;
using System.Data;
using System.Data.Odbc;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MConference;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPartner.Partner.ServerLookups;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;


namespace Ict.Petra.Server.MConference.WebConnectors
{
    /// <summary>
    /// Description of ConferenceOptions.
    /// </summary>
    public class TConferenceOptions
    {
        /// <summary>
        /// Get the units which start with the same campaign code as the given unit.
        /// </summary>
        /// <param name="AUnitKey">The unit which defines the campaign code</param>
        /// <returns>A table with all the relevant units</returns>
        public static PUnitTable GetCampaignOptions(Int64 AUnitKey)
        {
            String ConferenceCodePrefix = "";
            PUnitTable UnitTable = new PUnitTable();
            PUnitRow TemplateRow = UnitTable.NewRowTyped(false);
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("GetCampaignOptions called!");
            }
#endif
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                /* Load data */
                UnitTable = PUnitAccess.LoadByPrimaryKey(AUnitKey, ReadTransaction);

                if (UnitTable.Rows.Count > 0)
                {
                    String ConferenceCode = ((PUnitRow)UnitTable.Rows[0]).XyzTbdCode;
                    ConferenceCodePrefix = ConferenceCode.Substring(0, 5) + "%";
                }

                StringCollection operators = new StringCollection();
                operators.Add("LIKE");
                TemplateRow.XyzTbdCode = ConferenceCodePrefix;

                UnitTable = PUnitAccess.LoadUsingTemplate(TemplateRow, operators, null, ReadTransaction);
                //null, 0, 0);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                    if (TSrvSetting.DL >= 7)
                    {
                        Console.WriteLine("GetCampaignOptions: committed own transaction.");
                    }
#endif
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
        /// <param name="APrefix">Matching pattern for campaign code</param>
        /// <returns>A dataset with all the conferences in question</returns>
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

#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("GetConferences called!");
            }
#endif

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                /* Load data */

                if (APrefix.Length > 0)
                {
                    APrefix = APrefix.Replace('*', '%') + "%";
                    TemplateRow.XyzTbdPrefix = APrefix;

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
#if DEBUGMODE
                    if (TSrvSetting.DL >= 7)
                    {
                        Console.WriteLine("GetConferences: committed own transaction.");
                    }
#endif
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
        /// <returns>true if successful</returns>
        public static bool GetEarliestAndLatestDate(Int64 AConferenceKey, out DateTime AEarliestArrivalDate,
            out DateTime ALatestDepartureDate)
        {
            AEarliestArrivalDate = DateTime.Today;
            ALatestDepartureDate = DateTime.Today;
            PmShortTermApplicationTable ShortTermerTable;

            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("GetEarliestAndLatestDates called!");
            }
#endif

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                /* Load data */
                if (AConferenceKey == -1)
                {
                    ShortTermerTable = PmShortTermApplicationAccess.LoadAll(ReadTransaction);
                }
                else
                {
                    ShortTermerTable = PmShortTermApplicationAccess.LoadViaPUnitStConfirmedOption(AConferenceKey, ReadTransaction);
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                    if (TSrvSetting.DL >= 7)
                    {
                        Console.WriteLine("GetEarliestAndLatestDates: committed own transaction.");
                    }
#endif
                }
            }

            DateTime TmpEarliestArrivalTime = DateTime.MaxValue;
            DateTime TmpLatestDepartureTime = DateTime.MinValue;

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

            if (TmpEarliestArrivalTime != DateTime.MaxValue)
            {
                AEarliestArrivalDate = TmpEarliestArrivalTime;
            }

            if (TmpLatestDepartureTime != DateTime.MinValue)
            {
                ALatestDepartureDate = TmpLatestDepartureTime;
            }

            return true;
        }
    }
}