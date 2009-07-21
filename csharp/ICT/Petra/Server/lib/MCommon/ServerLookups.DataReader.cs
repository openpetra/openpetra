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
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.AP.Data.Access;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MCommon.Data.Access;
using Ict.Petra.Shared.MCommon.Data;

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
                if (ATablename == AApSupplierTable.GetTableDBName())
                {
                    AApSupplierTable typedTable;
                    AApSupplierAccess.LoadUsingTemplate(out typedTable, (AApSupplierRow)AKeys.Rows[0], ReadTransaction);
                    tempTable = typedTable;
                }
                else if (ATablename == AApDocumentTable.GetTableDBName())
                {
                    AApDocumentTable typedTable;
                    AApDocumentAccess.LoadUsingTemplate(out typedTable, (AApDocumentRow)AKeys.Rows[0], ReadTransaction);
                    tempTable = typedTable;
                }
                else if (ATablename == ACurrencyTable.GetTableDBName())
                {
                    ACurrencyTable typedTable;
                    ACurrencyAccess.LoadAll(out typedTable, ReadTransaction);
                    tempTable = typedTable;
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

        /// <summary>
        /// generic function for saving some rows in a single table
        /// </summary>
        /// <param name="ATablename"></param>
        /// <param name="ASubmitTable"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static TSubmitChangesResult SaveData(string ATablename,
            ref TTypedDataTable ASubmitTable,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TVerificationResultCollection SingleVerificationResultCollection;

            AVerificationResult = null;

            // TODO: check write permissions

            if (ASubmitTable != null)
            {
                AVerificationResult = new TVerificationResultCollection();
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    if (ATablename == ACurrencyTable.GetTableDBName())
                    {
                        if (ACurrencyAccess.SubmitChanges((ACurrencyTable)ASubmitTable, SubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
                catch (Exception e)
                {
                    TLogging.Log("after submitchanges: exception " + e.Message);

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw new Exception(e.ToString() + " " + e.Message);
                }
            }

            return SubmissionResult;
        }
    }
}