//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

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
            return DBAccess.GetDBAccessObj(AReadTransaction).SelectDT(
                String.Format(
                    "SELECT {0} AS LedgerNumber, {1} AS LedgerName FROM pub_{2}, pub_{3} " +
                    "WHERE pub_{2}.{4} = pub_{3}.{4} " + "AND   pub_{2}.{5} = 1",
                    ALedgerTable.GetLedgerNumberDBName(),
                    PPartnerTable.GetPartnerShortNameDBName(),
                    ALedgerTable.GetTableDBName(),
                    PPartnerTable.GetTableDBName(),
                    PPartnerTable.GetPartnerKeyDBName(),
                    ALedgerTable.GetLedgerStatusDBName()),
                ATableName,
                AReadTransaction);
        }
    }

    /// <summary>
    /// allows to filter cost centres by partner class
    /// </summary>
    public class TCostCentresLinkedToPartner
    {
        /// <summary>
        /// Loads all costcentres that are linked to a partner, with the partner key and partner class
        /// </summary>
        public static DataTable GetData(String ATableName, Int32 ALedgerNumber, TDBTransaction AReadTransaction)
        {
            return DBAccess.GetDBAccessObj(AReadTransaction).SelectDT(
                String.Format(
                    "SELECT {0}, {3}.{1}, {3}.{5}, {2}, {8} FROM pub_{3} " +
                    "LEFT OUTER JOIN PUB_{7} ON PUB_{7}.{1} = PUB_{3}.{1}, pub_{4} " +
                    "WHERE pub_{3}.{1} = pub_{4}.{1} AND pub_{3}.{5} = {6}",
                    AValidLedgerNumberTable.GetCostCentreCodeDBName(),
                    AValidLedgerNumberTable.GetPartnerKeyDBName(),
                    PPartnerTable.GetPartnerClassDBName(),
                    AValidLedgerNumberTable.GetTableDBName(),
                    PPartnerTable.GetTableDBName(),
                    AValidLedgerNumberTable.GetLedgerNumberDBName(),
                    ALedgerNumber.ToString(),
                    PUnitTable.GetTableDBName(),
                    PUnitTable.GetUnitTypeCodeDBName()),
                ATableName,
                AReadTransaction);
        }
    }
}