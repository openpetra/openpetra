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
        /// Upgrade to version 2022-02
        public static bool UpgradeDatabase202110_202202(TDataBase ADataBase)
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
                    sql = "SELECT COUNT(*) FROM PUB_p_type WHERE p_type_code_c = 'THANKYOU_NO_RECEIPT'";
                    if (Convert.ToInt32(ADataBase.ExecuteScalar(sql, SubmitChangesTransaction)) == 0)
                    {
                        sql = "INSERT INTO PUB_p_type( p_type_code_c, p_type_description_c, p_system_type_l, p_type_deletable_l) VALUES('THANKYOU_NO_RECEIPT','Receives no gift receipt but a thank you letter', false, true)";
                        ADataBase.ExecuteNonQuery(sql, SubmitChangesTransaction);
                    }

                    SubmitOK = true;
                });

            return true;
        }
    }
}
