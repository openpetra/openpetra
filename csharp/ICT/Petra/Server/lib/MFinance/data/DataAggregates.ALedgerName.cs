//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Common;
using Ict.Common.DB;

namespace Ict.Petra.Server.MFinance.DataAggregates
{
    /// <summary>
    /// The TALedgerNameAggregate Class contains logic to retrieve a list of ledger names,
    /// involving both a_ledger and p_partner tables.
    /// </summary>
    public class TALedgerNameAggregate
    {
        /// <summary>
        /// Loads all available Ledgers and their names into a DataTable
        ///
        /// </summary>
        /// <returns>void</returns>
        public static DataTable GetData(String ATableName, TDBTransaction AReadTransaction)
        {
            // TODO: use GetDBName of the table
            return DBAccess.GDBAccessObj.SelectDT(
                "SELECT a_ledger_number_i AS LedgerNumber, p_partner_short_name_c AS LedgerName " + "FROM pub_a_ledger, pub_p_partner " +
                "WHERE pub_a_ledger.p_partner_key_n = pub_p_partner.p_partner_key_n " + "AND   pub_a_ledger.a_ledger_status_l = 1",
                ATableName,
                AReadTransaction);
        }
    }
}