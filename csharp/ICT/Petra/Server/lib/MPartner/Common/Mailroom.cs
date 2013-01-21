//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, petrih
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
using System.Collections;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MPartner.Common
{
    /// <summary>
    /// Contains Partner Module Partner (Mailroom) -Subnamespace Business Logic.
    ///
    /// 'Business Logic' refers to any logic that retrieves data in a specific way,
    /// checks the validiy of modifications of data, or perform certain changes on
    /// data in a specific way.
    /// Business Logic can be contained in Classes or can be contained just in
    /// procedures/functions that are not part of a Class.
    /// </summary>
    public class TMailroom
    {
        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ALastContactDate"></param>
        public static void GetLastContactDate(Int64 APartnerKey, out DateTime ALastContactDate)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            DataSet LastContactDS;
            PPartnerContactRow ContactDR;

            LastContactDS = new DataSet("LastContactDate");
            LastContactDS.Tables.Add(new PPartnerContactTable());
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            PPartnerContactAccess.LoadViaPPartner(LastContactDS, APartnerKey,
                StringHelper.InitStrArr(new String[] { PPartnerContactTable.GetContactDateDBName() }), ReadTransaction,
                StringHelper.InitStrArr(new String[] { "ORDER BY " + PPartnerContactTable.GetContactDateDBName() + " DESC" }), 0, 1);

            if (LastContactDS.Tables[PPartnerContactTable.GetTableName()].Rows.Count > 0)
            {
                ContactDR = ((PPartnerContactTable)LastContactDS.Tables[PPartnerContactTable.GetTableName()])[0];
                ALastContactDate = ContactDR.ContactDate;
            }
            else
            {
                ALastContactDate = DateTime.MinValue;
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TMailroom.GetLastContactDate: committed own transaction.");
            }
        }
    }
}