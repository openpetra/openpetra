//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       AlanP
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
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MCommon;

namespace Ict.Testing.NUnitPetraClient
{
    /// <summary>
    /// A Utility class that can be used to Load data into client tests and save dummy data prior to running a screen test
    /// </summary>
    public class SerialisableDS
    {
        /// <summary>
        /// Loads all the current data from a specified table name into a Typed Table in a data set
        /// Uses TRemote.MCommon.DataReader.WebConnectors.GetData
        /// </summary>
        /// <param name="ATable">The typed table to load into</param>
        /// <param name="ATableName">The name of the server table to load</param>
        public static void LoadAll(TTypedDataTable ATable, string ATableName)
        {
            Ict.Common.Data.TTypedDataTable TypedTable;
            TRemote.MCommon.DataReader.WebConnectors.GetData(ATableName, null, out TypedTable);
            ATable.Merge(TypedTable);
        }

        /// <summary>
        /// Saves the data to the server
        /// </summary>
        /// <param name="ATable">The typed table from the data set</param>
        /// <param name="ATableChanges">The changes table</param>
        /// <param name="ATableDbName">The server table name to write to</param>
        /// <returns></returns>
        public static bool SaveChanges(TTypedDataTable ATable, TTypedDataTable ATableChanges, string ATableDbName)
        {
            TSubmitChangesResult SubmissionResult;
            TVerificationResultCollection VerificationResult;

            if (ATableChanges == null)
            {
                // There is nothing to be saved.
                return true;
            }

            // Submit changes to the PETRAServer
            try
            {
                SubmissionResult = TRemote.MCommon.DataReader.WebConnectors.SaveData(ATableDbName, ref ATableChanges, out VerificationResult);
            }
            catch (ESecurityDBTableAccessDeniedException)
            {
                Console.WriteLine("Error saving data prior to test: Access denied");
                return false;
            }
            catch (EDBConcurrencyException)
            {
                Console.WriteLine("Error saving data prior to test: Concurrency exception");
                return false;
            }
            catch (Exception Exp)
            {
                Console.WriteLine("Error saving data prior to test: General exception: {0}", Exp.Message);
                return false;
            }

            switch (SubmissionResult)
            {
                case TSubmitChangesResult.scrOK:
                    // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                    ATable.AcceptChanges();

                    // Merge back with data from the Server (eg. for getting Sequence values)
                    ATableChanges.AcceptChanges();
                    ATable.Merge(ATableChanges, false);

                    // need to accept the new modification ID
                    ATable.AcceptChanges();
                    return true;

                case TSubmitChangesResult.scrNothingToBeSaved:
                    return true;

                case TSubmitChangesResult.scrError:
                    Console.WriteLine("Error saving data prior to test: Submission of data failed");
                    break;

                case TSubmitChangesResult.scrInfoNeeded:
                    Console.WriteLine("Error saving data prior to test: Info Needed");
                    break;
            }

            return false;
        }
    }
}