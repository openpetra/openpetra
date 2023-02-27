//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2023 by OM International
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
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Data;

namespace Ict.Petra.Server.MSysMan.DBUpgrades
{
    /// <summary>
    /// Upgrade the database
    /// </summary>
    public static partial class TDBUpgrade
    {
        /// Upgrade to version 2023-01
        public static bool UpgradeDatabase202207_202301(TDataBase ADataBase)
        {
            // there are changes to the database structure
            TDBTransaction SubmitChangesTransaction = new TDBTransaction();
            bool SubmitOK = false;
            string sql = String.Empty;

            ADataBase.WriteTransaction(ref SubmitChangesTransaction,
                ref SubmitOK,
                delegate
                {
                    bool ColumnExists = false;

                    try
                    {
                        sql = "SELECT COUNT(*) FROM PUB_a_recurring_gift WHERE a_sepa_mandate_reference_c = 'TEST'";
                        if (Convert.ToInt32(ADataBase.ExecuteScalar(sql, SubmitChangesTransaction)) == 0)
                        {
                            ColumnExists = true;
                        }
                    }
                    catch (System.Exception)
                    {
                        // the column must be added
                    }

                    if (!ColumnExists)
                    {
                        sql = "ALTER TABLE PUB_a_recurring_gift ADD COLUMN a_sepa_mandate_reference_c varchar(70) DEFAULT NULL AFTER p_banking_details_key_i";
                        ADataBase.ExecuteNonQuery(sql, SubmitChangesTransaction);
                        sql = "ALTER TABLE PUB_a_recurring_gift ADD COLUMN a_sepa_mandate_given_d date DEFAULT NULL AFTER a_sepa_mandate_reference_c";
                        ADataBase.ExecuteNonQuery(sql, SubmitChangesTransaction);
                    }


                    bool TableExists = false;
                    try
                    {
                        sql = "SELECT COUNT(*) FROM PUB_p_partner_membership_paid";
                        if (Convert.ToInt32(ADataBase.ExecuteScalar(sql, SubmitChangesTransaction)) >= 0)
                        {
                            TableExists = true;
                        }
                    }
                    catch (System.Exception)
                    {
                        // the table must be added
                    }

                    if (!TableExists)
                    {
                        // insert new tables
                        string[] SqlStmts = TDataBase.ReadSqlFile("Upgrade202207_202301.sql").Split(new char[]{';'});

                        foreach (string stmt in SqlStmts)
                        {
                            if (stmt.Trim().Length > 0)
                            {
                                ADataBase.ExecuteNonQuery(stmt, SubmitChangesTransaction);
                            }
                        }
                    }

                    SubmitOK = true;
                });

            return true;
        }
    }
}
