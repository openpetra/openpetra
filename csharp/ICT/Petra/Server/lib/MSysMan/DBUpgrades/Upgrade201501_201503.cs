//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2015 by OM International
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
        /// Upgrade to version 2015-03
        public static bool UpgradeDatabase201501_201503()
        {
            // There are renamed and new fields
            // a_motivation_detail: a_tax_deductible_account_c  => a_tax_deductible_account_code_c
            // a_gift_detail: new field a_account_code_c, new field a_tax_deductible_account_code_c
            // and new foreign keys for the fields

            TDBTransaction SubmitChangesTransaction = null;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref SubmitChangesTransaction,
                ref SubmissionResult,
                delegate
                {
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_motivation_detail 
                                                            RENAME COLUMN a_tax_deductible_account_c TO a_tax_deductible_account_code_c", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_motivation_detail
                                                            ADD CONSTRAINT a_motivation_detail_fk5 
                                                              FOREIGN KEY (a_ledger_number_i,a_tax_deductible_account_code_c)
                                                              REFERENCES a_account(a_ledger_number_i,a_account_code_c)",
                                                          SubmitChangesTransaction);
                    
                    DBAccess.GDBAccessObj.ExecuteNonQuery("ALTER TABLE a_gift_detail ADD COLUMN a_account_code_c VARCHAR(16)", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery("ALTER TABLE a_gift_detail ADD COLUMN a_tax_deductible_account_code_c VARCHAR(16)", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_gift_detail
                                                              ADD CONSTRAINT a_gift_detail_fk8
                                                                FOREIGN KEY (a_ledger_number_i,a_account_code_c)
                                                                REFERENCES a_account(a_ledger_number_i,a_account_code_c)",
                                                          SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_gift_detail
                                                              ADD CONSTRAINT a_gift_detail_fk9
                                                                FOREIGN KEY (a_ledger_number_i,a_tax_deductible_account_code_c)
                                                                REFERENCES a_account(a_ledger_number_i,a_account_code_c)",
                                                          SubmitChangesTransaction);
                    SubmissionResult = TSubmitChangesResult.scrOK;
                });
            return true;
        }
    }
}