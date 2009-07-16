/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance.AP.Data.Access;
using Ict.Petra.Shared.MFinance.AP.Data;

namespace Ict.Petra.Server.MCommon.DataReader
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MCommon DataReader sub-namespace.
    ///
    /// </summary>
    public class TCommonDataReader
    {
        /// <summary>
        /// simple data reader;
        /// checks for permissions of the current user;
        /// </summary>
        /// <param name="ATablename"></param>
        /// <param name="AKeys">we need to use a TTypedDataTable instead of a DataRow for the key values since DataRow cannot be serialized on its own in .net</param>
        /// <param name="AResultTable">returns typed datatable</param>
        /// <returns></returns>
        public static bool GetData(string ATablename, TTypedDataTable AKeys, out TTypedDataTable AResultTable)
        {
            // TODO: check access permissions for the current user

            TDBTransaction ReadTransaction;

            TTypedDataTable tempTable = null;

            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.RepeatableRead, 5);

                // TODO: auto generate
                if (ATablename == "a_ap_supplier")
                {
                    Console.WriteLine(((AApSupplierRow)AKeys.Rows[0]).PartnerKey.ToString());
                    AApSupplierTable typedTable;
                    AApSupplierAccess.LoadUsingTemplate(out typedTable, (AApSupplierRow)AKeys.Rows[0], ReadTransaction);
                    Console.WriteLine("Datareader1: " + typedTable.Rows.Count.ToString());
                    tempTable = typedTable;
                    Console.WriteLine("Datareader2: " + tempTable.Rows.Count.ToString());
                }
                else
                {
                    throw new Exception("TCommonDataReader.LoadData: unknown table " + ATablename);
                }
            }
            catch (Exception Exp)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                TLogging.Log("TCommonDataReader.LoadData exception: " + Exp.ToString(), TLoggingType.ToLogfile);
                TLogging.Log(Exp.StackTrace, TLoggingType.ToLogfile);
                throw Exp;
            }
            finally
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }

            // Accept row changes here so that the Client gets 'unmodified' rows
            tempTable.AcceptChanges();

            // return the table
            AResultTable = tempTable;

            return true;
        }
    }
}