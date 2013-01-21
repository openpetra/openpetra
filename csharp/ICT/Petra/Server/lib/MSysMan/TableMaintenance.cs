//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;

namespace Ict.Petra.Server.MSysMan.TableMaintenance.UIConnectors
{
    /// <summary>
    /// User Interface Connector for table maintenance
    /// </summary>
    public class TSysManTableMaintenanceUIConnector : TConfigurableMBRObject, ISysManUIConnectorsTableMaintenance
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TSysManTableMaintenanceUIConnector() : base()
        {
            TLogging.LogAtLevel(7, "TSysManTableMaintenanceUIConnector created.");
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~TSysManTableMaintenanceUIConnector()
        {
            TLogging.LogAtLevel(7, "TSysManTableMaintenanceUIConnector.FINALIZE called!");
        }

        /// <summary>
        /// get the datatable from the database
        /// </summary>
        /// <param name="ATableName">name of table to retrieve</param>
        /// <returns></returns>
        public DataTable GetData(string ATableName)
        {
            DataTable table = new DataTable();
            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.RepeatableRead, 5);

            if (ATableName == SUserTable.GetTableDBName())
            {
                table = SUserAccess.LoadAll(ReadTransaction);
            }

            return table;
        }

        /// <summary>
        /// submit changes of table
        /// </summary>
        /// <param name="AInspectTable"></param>
        /// <param name="AResponseTable"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public TSubmitChangesResult SubmitChanges(ref DataTable AInspectTable,
            ref DataTable AResponseTable,
            out TVerificationResultCollection AVerificationResult)
        {
            // TODO
            AVerificationResult = new TVerificationResultCollection();
            return TSubmitChangesResult.scrError;
        }
    }
}