//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2013 by OM International
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
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Threading;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.PartnerFind;
using Ict.Petra.Server.MPartner.Extracts;
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Shared.MCommon;

namespace Ict.Petra.Server.MPartner.Partner.UIConnectors
{
    /// <summary>
    /// Partner Find Screen UIConnector
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
    public class TPartnerFindUIConnector : IPartnerUIConnectorsPartnerFind
    {
        private TPartnerFind FPartnerFind = new TPartnerFind();

        /// <summary>
        /// constructor
        /// </summary>
        public TPartnerFindUIConnector() : base()
        {
        }

        /// <summary>Get the current state of progress</summary>
        public TProgressState Progress
        {
            get
            {
                return FPartnerFind.Progress;
            }
        }

        /// <summary>
        /// Procedure to execute a Find query. Although the full
        /// query results are retrieved from the DB and stored internally in an object,
        /// data will be returned in 'pages' of data, each page holding a defined number
        /// of records.
        ///
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Partner Find parameters</param>
        /// <param name="ADetailedResults">Returns more (when true) or less (when false) columns
        /// </param>
        public void PerformSearch(DataTable ACriteriaData, bool ADetailedResults)
        {
            FPartnerFind.PerformSearch(ACriteriaData, ADetailedResults);
        }

        /// <summary>
        /// Procedure to execute a Find query. Although the full
        /// query results are retrieved from the DB and stored internally in an object,
        /// data will be returned in 'pages' of data, each page holding a defined number
        /// of records.
        /// </summary>
        public void PerformSearchByBankDetails(DataTable ACriteriaData)
        {
            FPartnerFind.PerformSearchByBankDetails(ACriteriaData);
        }

        /// <summary>
        /// Returns the specified find results page.
        ///
        /// @comment Pages can be requested in any order!
        ///
        /// </summary>
        /// <param name="APage">Page to return</param>
        /// <param name="APageSize">Number of records to return per page</param>
        /// <param name="ATotalRecords">The amount of rows found by the SELECT statement</param>
        /// <param name="ATotalPages">The number of pages that will be needed on client-side to
        /// hold all rows found by the SELECT statement</param>
        /// <returns>DataTable containing the find result records for the specified page
        /// </returns>
        public DataTable GetDataPagedResult(System.Int16 APage, System.Int16 APageSize, out System.Int32 ATotalRecords, out System.Int16 ATotalPages)
        {
            return FPartnerFind.GetDataPagedResult(APage, APageSize, out ATotalRecords, out ATotalPages);
        }

        /// <summary>
        /// Gets the page number containing given partner key.
        /// </summary>
        /// <param name="APartnerKey">Partner key to search for</param>
        /// <param name="AStartPage">Page to start searching</param>
        /// <param name="APageSize">Size of a page.</param>
        /// <returns></returns>
        public Int16 GetPageNumberContainingPartnerKey(Int64 APartnerKey, System.Int16 AStartPage, System.Int16 APageSize)
        {
            return FPartnerFind.GetPageNumberContainingPartnerKey(APartnerKey, AStartPage, APageSize);
        }

        /// <summary>
        /// Stops the query execution.
        ///
        /// @comment It might take some time until the executing query is cancelled by
        /// the DB, but this procedure returns immediately. The reason for this is that
        /// we consider the query cancellation as done since the application can
        /// 'forget' about the result of the cancellation process (but beware of
        /// executing another query while the other is stopping - this leads to ADO.NET
        /// errors that state that a ADO.NET command is still executing!).
        ///
        /// </summary>
        public void StopSearch()
        {
            FPartnerFind.StopSearch();
        }

        /// <summary>
        /// Adds all Partners that were last found to an Extract.
        /// </summary>
        /// <param name="AExtractName">Name of the Extract to add the Partners to.</param>
        /// <param name="AExtractDescription">Description of the Extract to add the Partners to.</param>
        /// <param name="AExtractID">ExtractID of the Extract to add the Partners to.</param>
        /// <param name="AVerificationResult">Contains DB call exceptions, if there are any.</param>
        /// <returns>The number of Partners that were added to the Extract, or -1
        /// if DB call exeptions occured.</returns>
        public Int32 AddAllFoundPartnersToExtract(string AExtractName, string AExtractDescription, int AExtractID,
            out TVerificationResultCollection AVerificationResult)
        {
            return FPartnerFind.AddAllFoundPartnersToExtract(AExtractName, AExtractDescription, AExtractID, out AVerificationResult);
        }

        /// <summary>
        /// Checks if a search result contains a given partner
        /// </summary>
        /// <param name="APartnerKey">Partner key of partner</param>
        /// <returns>True if partner is included, false if not.</returns>
        public bool CheckIfResultsContainPartnerKey(long APartnerKey)
        {
            return FPartnerFind.CheckIfResultsContainPartnerKey(APartnerKey);
        }
    }
}