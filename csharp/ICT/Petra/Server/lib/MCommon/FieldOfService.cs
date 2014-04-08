//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using System.Data;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Server.MCommon.UIConnectors
{
    /// <summary>
    /// Field Of Service Screen UIConnector
    ///
    /// UIConnector Objects are instantiated by the Client's User Interface via the
    /// Instantiator classes.
    /// They handle requests for data retrieval and saving of data (including data
    /// verification).
    ///
    /// Their role is to
    ///   - retrieve (and possibly aggregate) data using Business Objects,
    ///   - put this data into ///one/// DataSet that is passed to the Client and make
    ///     sure that no unnessary data is transferred to the Client,
    ///   - optionally provide functionality to retrieve additional, different data
    ///     if requested by the Client (for Client screens that load data initially
    ///     as well as later, eg. when a certain tab on the screen is clicked),
    ///   - save data using Business Objects.
    ///
    /// @Comment These Objects would usually not be instantiated by other Server
    ///          Objects, but only by the Client UI via the Instantiator classes.
    ///          However, Server Objects that derive from these objects and that
    ///          are also UIConnectors are feasible.
    /// </summary>
    public class TFieldOfServiceUIConnector : TConfigurableMBRObject, IPartnerUIConnectorsFieldOfService
    {
        /// <summary>Name of the Main DataSet for the Screen</summary>
        private const String DATASETNAME = "FieldOfServiceScreen";

        /// <summary>Main Parameter for the Screen</summary>
        private Int64 FPartnerKey;

        /// <summary>Main DataSet for the Screen</summary>
        private FieldOfServiceTDS FMainDS;

        #region TFieldOfServiceUIConnector

        /// <summary>
        /// Constructor.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TFieldOfServiceUIConnector(Int64 APartnerKey)
        {
            FPartnerKey = APartnerKey;
        }

        /// <summary>
        /// Passes data as a Typed DataSet to the Partner Edit Screen, containing multiple
        /// DataTables.
        ///
        /// </summary>
        /// <returns>void</returns>
        public FieldOfServiceTDS GetData()
        {
            LoadData();
            return FMainDS;
        }

        /// <summary>
        /// Loads required data from the DB.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void LoadData()
        {
            TDBTransaction ReadTransaction;

//          TLogging.LogAtLevel(9, (this.GetType().FullName + ": LoadData called.");

            // create the FMainDS DataSet that will later be passed to the Client
            FMainDS = new FieldOfServiceTDS(DATASETNAME);
            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.RepeatableRead, 5);
                try
                {
                    // Load data for Partner
                    PPartnerAccess.LoadByPrimaryKey(FMainDS, FPartnerKey, ReadTransaction);

                    if (FMainDS.PPartner.Rows.Count == 0)
                    {
                        throw new EPartnerNotExistantException();
                    }

                    // Load data for Field Of Service
                    PPartnerGiftDestinationAccess.LoadViaPPartner(FMainDS, FPartnerKey, ReadTransaction);
                }
                catch (EPartnerNotExistantException)
                {
                    // don't log this exception  this is thrown on purpose here and the Client deals with it.
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    throw;
                }
                catch (Exception Exp)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.Log(this.GetType().FullName + ".LoadData exception: " + Exp.ToString(), TLoggingType.ToLogfile);
                    TLogging.Log(Exp.StackTrace, TLoggingType.ToLogfile);
                    throw;
                }
            }
            finally
            {
                if (DBAccess.GDBAccessObj.Transaction != null)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }

            // Accept row changes here so that the Client gets 'unmodified' rows
            FMainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            // Examples for such DataTables are the ones that exist for a certain Partner
            // Class, eg. Person  only one of those Tables will be filled, the other ones
            // are not needed at the Client side.
            FMainDS.RemoveEmptyTables();
        }

        /// <summary>
        /// Saves data from the Screen (contained in a Typed DataSet).
        ///
        /// All DataTables contained in the Typed DataSet are inspected for added,
        /// changed or deleted rows by submitting them to the DataStore.
        ///
        /// </summary>
        /// <param name="AInspectDS">Typed DataSet that needs to contain known DataTables</param>
        /// <param name="AVerificationResult">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions)</param>
        /// <returns>true if all verifications are OK and all DB calls succeeded, false if
        /// any verification or DB call failed
        /// </returns>
        public TSubmitChangesResult SubmitChanges(ref FieldOfServiceTDS AInspectDS, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;
            return TSubmitChangesResult.scrError;
        }

        #endregion
    }
}