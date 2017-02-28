//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timh, timop
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
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Threading;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Common.Session;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Server.MCommon.Data.Cascading;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MCommon;
//using Ict.Petra.Server.MPartner.Partner;
using Ict.Petra.Shared.MCommon;

namespace Ict.Petra.Server.MPartner.Partner.UIConnectors
{
    /// <summary>
    /// Partner Location Search Screen UIConnector
    /// </summary>
    public class TPartnerLocationFindUIConnector : IPartnerUIConnectorsPartnerLocationFind
    {
        /// <summary>Paged query object</summary>
        private TPagedDataSet FPagedDataSetObject;

        /// <summary>Thread that is used for asynchronously executing the Find query</summary>
        private Thread FFindThread;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TPartnerLocationFindUIConnector() : base()
        {
        }

        /// <summary>Get the current state of progress</summary>
        public TProgressState Progress
        {
            get
            {
                return FPagedDataSetObject.Progress;
            }
        }

        /// <summary>
        /// Procedure to execute a Find query. Although the full
        /// query results are retrieved from the DB and stored internally in an object,
        /// data will be returned in 'pages' of data, each page holding a defined number
        /// of records.
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Partner Find parameters</param>
        public void PerformSearch(DataTable ACriteriaData)
        {
            String CustomWhereCriteria;

            OdbcParameter[] ParametersArray;

            FPagedDataSetObject = new TPagedDataSet(new PartnerFindTDSSearchResultTable());

            // Pass the TAsynchronousExecutionProgress object to FPagedDataSetObject so that it

            // Build WHERE criteria string based on AFindCriteria
            CustomWhereCriteria = BuildCustomWhereCriteria(ACriteriaData, out ParametersArray);

            TLogging.LogAtLevel(6, "WHERE CLAUSE: " + CustomWhereCriteria);

            FPagedDataSetObject.FindParameters = new TPagedDataSet.TAsyncFindParameters(
                " p_city_c, p_postal_code_c,  p_locality_c, p_street_name_c, p_address_3_c, p_county_c, p_country_code_c, p_location_key_i, p_site_key_n ",
                "PUB_p_location ",
                " p_location_key_i<>-1 " + CustomWhereCriteria + ' ',
                "p_city_c ",
                null,
                ParametersArray);

            // fields
            // table
            // where
            // order by
            // both empty for now

            string session = TSession.GetSessionID();

            //
            // Start the Find Thread
            //
            ThreadStart myThreadStart = delegate {
                FPagedDataSetObject.ExecuteQuery(session);
            };
            FFindThread = new Thread(myThreadStart);
            FFindThread.Name = "PartnerLocationFind" + Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Used internally to build a SQL WHERE criteria from the AFindCriteria HashTable.
        ///
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Partner Find parameters</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated OdbcParameters
        /// (including parameter Value)</param>
        /// <returns>SQL WHERE criteria
        /// </returns>
        private String BuildCustomWhereCriteria(DataTable ACriteriaData, out OdbcParameter[] AParametersArray)
        {
            String CustomWhereCriteria = "";
            DataTable CriteriaDataTable;
            DataRow CriteriaRow;
            ArrayList InternalParameters;
            OdbcParameter miParam;

            CriteriaDataTable = ACriteriaData;
            CriteriaRow = CriteriaDataTable.Rows[0];
            InternalParameters = new ArrayList();

            if (CriteriaRow["Addr1"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnLocalityId, CriteriaRow, "Addr1", "Addr1Match", ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            if (CriteriaRow["Street2"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnStreetNameId, CriteriaRow, "Street2", "Street2Match",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            if (CriteriaRow["Addr3"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnAddress3Id, CriteriaRow, "Addr3", "Addr3Match", ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            if (CriteriaRow["City"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnCityId, CriteriaRow, "City", "CityMatch", ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            if (CriteriaRow["PostCode"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnPostalCodeId, CriteriaRow, "PostCode", "PostCodeMatch",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            if (CriteriaRow["County"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnCountyId, CriteriaRow, "County", "CountyMatch", ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            if (CriteriaRow["Country"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnCountryCodeId, CriteriaRow, "Country", "CountryMatch",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            if (CriteriaRow["LocationKey"].ToString().Length > 0)
            {
                // DISREGARD ALL OTHER SEARCH CRITERIA!!!
                CustomWhereCriteria = "";
                InternalParameters = new ArrayList();

                CustomWhereCriteria = String.Format("{0} AND PUB.{1}.{2} = ?", CustomWhereCriteria,
                    PLocationTable.GetTableDBName(), PLocationTable.GetLocationKeyDBName());

                miParam = new OdbcParameter("", OdbcType.Decimal, 10);
                miParam.Value = (object)CriteriaRow["LocationKey"];
                InternalParameters = new ArrayList();
                InternalParameters.Add(miParam);
            }

//           TLogging.LogAtLevel(7, "CustomWhereCriteria: " + CustomWhereCriteria);

            /* Convert ArrayList to a array of ODBCParameters
             * seem to need to declare a type first
             */
            AParametersArray = ((OdbcParameter[])(InternalParameters.ToArray(typeof(OdbcParameter))));
            InternalParameters = null;             // ensure this is GC'd

            return CustomWhereCriteria;
        }

//        /// destructor
//        ~TPartnerLocationFindUIConnector()
//        {
//            TLogging.LogAtLevel (9, "TPartnerLocationFindUIConnector.FINALIZE called!");
//        }

        /// <summary>
        /// </summary>
        /// <param name="APage"></param>
        /// <param name="APageSize"></param>
        /// <param name="ATotalRecords"></param>
        /// <param name="ATotalPages"></param>
        /// <returns></returns>
        public DataTable GetDataPagedResult(System.Int16 APage, System.Int16 APageSize, out System.Int32 ATotalRecords, out System.Int16 ATotalPages)
        {
            DataTable ReturnValue;

            TLogging.LogAtLevel(7, "TPartnerLocationFindUIConnector.GetDataPagedResult called.");

            ReturnValue = FPagedDataSetObject.GetData(APage, APageSize);
            ATotalPages = FPagedDataSetObject.TotalPages;
            ATotalRecords = FPagedDataSetObject.TotalRecords;
            return ReturnValue;
        }

        /// <summary>
        /// Stops the query execution.
        /// <remarks>It might take some time until the executing query is cancelled by the DB, but this procedure returns
        /// immediately. The reason for this is that we consider the query cancellation as done since the application can
        /// 'forget' about the result of the cancellation process (but beware of executing another query while the other is
        /// stopping - this leads to ADO.NET errors that state that a ADO.NET command is still executing!).
        /// </remarks>
        /// </summary>
        public void StopSearch()
        {
            Thread StopQueryThread;

            /* Start a separate Thread that should cancel the executing query
             * (Microsoft recommends doing it this way!)
             */
            TLogging.LogAtLevel(7, "TPartnerLocationFindUIConnector.StopSearch: Starting StopQuery thread...");

            StopQueryThread = new Thread(new ThreadStart(FPagedDataSetObject.StopQuery));
            StopQueryThread.Name = UserInfo.GUserInfo.UserID + "__ParnterFind_StopSearch_Thread";
            StopQueryThread.Start();

            /* It might take some time until the executing query is cancelled by the DB,
             * but we consider it as done since the application can 'forget' about the
             * result of the cancellation process (but beware of executing another query
             * while the other is stopping - this leads to ADO.NET errors that state that
             * a ADO.NET command is still executing!
             */

            TLogging.LogAtLevel(7, "TPartnerLocationFindUIConnector.StopSearch: Query cancelled!");
        }
    }
}
