//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2022 by OM International
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
        /// Upgrade to version 2022-06
        public static bool UpgradeDatabase202202_202206(TDataBase ADataBase)
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
                        sql = "SELECT COUNT(*) FROM PUB_s_session WHERE s_user_id_c = 'TEST'";
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
                        sql = "ALTER TABLE PUB_s_session ADD COLUMN s_user_id_c varchar(20) DEFAULT NULL AFTER s_session_values_c";
                        ADataBase.ExecuteNonQuery(sql, SubmitChangesTransaction);
                    }

                    SubmitOK = true;
                });

            return true;
        }
    }
}
