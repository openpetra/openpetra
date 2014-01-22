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
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;


namespace Ict.Petra.Server.MPersonnel.Person.DataElements.WebConnectors
{
    /// <summary>
    /// Web Connector for the Application Data of a PERSON.
    /// </summary>
    public class TApplicationDataWebConnector
    {
        /// <summary>Gets the OutreachCode for a conference/outreach.</summary>
        /// <param name="APartnerKey">PartnerKey of the conference/outreach.</param>
        /// <returns>PUnit OutreachCode for conference/outreach.</returns>
        [RequireModulePermission("AND(PERSONNEL,PTNRUSER)")]
        public static string GetOutreachCode(Int64 APartnerKey)
        {
            string ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            PUnitTable UnitTable = PUnitAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

            if (UnitTable.Count > 0)
            {
                ReturnValue = ((PUnitRow)UnitTable.Rows[0]).OutreachCode;
            }
            else
            {
                ReturnValue = "";
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                TLogging.LogAtLevel(7, "TIndividualDataWebConnector.GetOutreachCode: rollback own transaction.");
            }

            return ReturnValue;
        }
    }
}