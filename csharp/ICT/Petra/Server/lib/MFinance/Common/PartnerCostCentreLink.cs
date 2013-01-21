//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// Contains Finance Module Setup-Subnamespace Business Logic.
    ///
    /// 'Business Logic' refers to any logic that retrieves data in a specific way,
    /// checks the validiy of modifications of data, or perform certain changes on
    /// data in a specific way.
    /// Business Logic can be contained in Classes or can be contained just in
    /// procedures/functions that are not part of a Class.
    /// </summary>
    public class Common
    {
        /// <summary>
        /// check if the partner has a link to a cost centre (eg. a worker)
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ACostCentreCode"></param>
        /// <returns></returns>
        public static Boolean HasPartnerCostCentreLink(Int64 APartnerKey, out String ACostCentreCode)
        {
            Boolean ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            StringCollection RequiredColumns;
            AValidLedgerNumberTable ValidLedgerNumberTable;

            ACostCentreCode = "";

            RequiredColumns = new StringCollection();
            RequiredColumns.Add(AValidLedgerNumberTable.GetCostCentreCodeDBName());
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            try
            {
                ValidLedgerNumberTable = AValidLedgerNumberAccess.LoadViaPPartnerPartnerKey(
                    APartnerKey,
                    RequiredColumns,
                    ReadTransaction,
                    null,
                    0,
                    0);

                if (ValidLedgerNumberTable.Rows.Count != 0)
                {
                    ACostCentreCode = ValidLedgerNumberTable[0].CostCentreCode;
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "HasPartnerCostCentreLink: committed own transaction.");
                }
            }
            return ReturnValue;
        }
    }
}