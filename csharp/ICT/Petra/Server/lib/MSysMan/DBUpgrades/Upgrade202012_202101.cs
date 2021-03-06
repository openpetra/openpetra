//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2021 by OM International
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
        /// Upgrade to version 2021-01
        public static bool UpgradeDatabase202012_202101(TDataBase ADataBase)
        {
            // there are no changes to the database structure

            // but there is data we might need to add
            TDBTransaction SubmitChangesTransaction = new TDBTransaction();
            bool SubmitOK = false;
            string sql = String.Empty;

            ADataBase.WriteTransaction(ref SubmitChangesTransaction,
                ref SubmitOK,
                delegate
                {
                    sql = "SELECT COUNT(*) FROM PUB_p_banking_type";
                    if (Convert.ToInt32(ADataBase.ExecuteScalar(sql, SubmitChangesTransaction)) == 0)
                    {
                        sql = "INSERT INTO PUB_p_banking_type( p_id_i, p_type_c) VALUES(0, 'BANK ACCOUNT')";
                        ADataBase.ExecuteNonQuery(sql, SubmitChangesTransaction);
                    }

                    sql = "SELECT COUNT(*) FROM PUB_p_banking_details_usage_type";
                    if (Convert.ToInt32(ADataBase.ExecuteScalar(sql, SubmitChangesTransaction)) == 0)
                    {
                        sql = "INSERT INTO PUB_p_banking_details_usage_type(p_type_c, p_type_description_c) VALUES ('MAIN','The default banking detail that should be used for this partner')";
                        ADataBase.ExecuteNonQuery(sql, SubmitChangesTransaction);
                    }

                    SubmitOK = true;
                });

            return true;
        }
    }
}
