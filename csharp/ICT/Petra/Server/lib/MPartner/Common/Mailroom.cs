//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, petrih, andreww
//
// Copyright 2004-2014 by OM International
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
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null (default = null). If null
        /// gets passed then the Method executes DB commands with the 'globally available'
        /// <see cref="DBAccess.GDBAccessObj" /> instance, otherwise with the instance that gets passed in with this
        /// Argument!</param>
        public static void GetLastContactDate(Int64 APartnerKey, out DateTime ALastContactDate,
            TDataBase ADataBase = null)
        {
            TDBTransaction ReadTransaction = null;

            DataSet LastContactDS;
            PContactLogRow ContactDR;
            DateTime LastContactDate = new DateTime();

            LastContactDS = new DataSet("LastContactDate");
            LastContactDS.Tables.Add(new PContactLogTable());

            DBAccess.GetDBAccessObj(ADataBase).GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    PContactLogAccess.LoadViaPPartnerPPartnerContact(LastContactDS, APartnerKey,
                        StringHelper.InitStrArr(new String[] { PContactLogTable.GetContactDateDBName() }), ReadTransaction,
                        StringHelper.InitStrArr(new String[] { "ORDER BY " + PContactLogTable.GetContactDateDBName() + " DESC" }), 0, 1);

                    if (LastContactDS.Tables[PContactLogTable.GetTableName()].Rows.Count > 0)
                    {
                        ContactDR = ((PContactLogTable)LastContactDS.Tables[PContactLogTable.GetTableName()])[0];
                        LastContactDate = ContactDR.ContactDate;
                    }
                    else
                    {
                        LastContactDate = DateTime.MinValue;
                    }
                });

            ALastContactDate = LastContactDate;
        }
    }
}