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
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MPartner.Partner.Data;


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

//			foreach(PUnitRow UnitRow in UnitTable.Rows)
//			{
//				if (!UnitRow.CampaignCode.StartsWith(ConferenceCodePrefix, true, null))
//				{
//					continue;
//				}
//
//				DataRow NewRow = AConferenceTable.NewRow();
//
//				NewRow["Partner Key"] = UnitRow.PartnerKey;
//				NewRow["Campaign Code"] = UnitRow.CampaignCode;
//				NewRow["Unit Name"] = UnitRow.UnitName;
//
//				AConferenceTable.Rows.Add(NewRow);
//			}
            return UnitTable;
        }
    }
}