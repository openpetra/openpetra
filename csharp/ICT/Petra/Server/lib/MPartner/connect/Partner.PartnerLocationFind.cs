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
using Ict.Petra.Server.MPartner.Partner;
using Ict.Petra.Shared.MCommon;

namespace Ict.Petra.Server.MPartner.Partner.UIConnectors
{
    /// <summary>
    /// Partner Location Search Screen UIConnector
    ///
    /// </summary>
    public class TPartnerLocationFindUIConnector : IPartnerUIConnectorsPartnerLocationFind
    {
        private Thread FFindThread;
        private TPagedDataSet FPagedDataSetObject;

        /// <summary>Get the current state of progress</summary>
        public TProgressState Progress
        {
            get
            {
                return FPagedDataSetObject.Progress;
            }
        }

        /// <summary>
        /// contructor
        /// </summary>
        /// <param name="ACriteriaData"></param>
        public TPartnerLocationFindUIConnector(DataTable ACriteriaData) : base()
        {
            Hashtable ColumnNameMapping;
            String CustomWhereCriteria;
            ArrayList InternalParameters;
            OdbcParameter miParam;
            DataRow CriteriaRow;

            FPagedDataSetObject = new TPagedDataSet(new PartnerFindTDSSearchResultTable());
            ColumnNameMapping = null;

            // get the first and only row
            CriteriaRow = ACriteriaData.Rows[0];

            // used to help with strong typing of columns
            InternalParameters = new ArrayList();
            CustomWhereCriteria = "";

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

            Console.WriteLine("WHERE CLAUSE: " + CustomWhereCriteria);
            FPagedDataSetObject.FindParameters = new TPagedDataSet.TAsyncFindParameters(
                " p_city_c, p_postal_code_c,  p_locality_c, p_street_name_c, p_address_3_c, p_county_c, p_country_code_c, p_location_key_i, p_site_key_n ",
                "PUB_p_location ",
                " p_location_key_i<>-1 " + CustomWhereCriteria + ' ',
                "p_city_c ",
                ColumnNameMapping,
                ((OdbcParameter[])(InternalParameters.ToArray(typeof(OdbcParameter)))));

            // fields
            // table
            // where
            // order by
            // both empty for now

            string session = TSession.GetSessionID();

            //
            // Start the Find Thread
            //
            try
            {
                ThreadStart myThreadStart = delegate {
                    FPagedDataSetObject.ExecuteQuery(session);
                };
                FFindThread = new Thread(myThreadStart);
                FFindThread.Name = "PartnerLocationFind" + Guid.NewGuid().ToString();
                FFindThread.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

/*
 * /// destructor
 *      ~TPartnerLocationFindUIConnector()
 *      {
 *          TLogging.LogAtLevel (9, "TPartnerLocationFindUIConnector.FINALIZE called!");
 *      }
 */


        /// <summary>
        /// </summary>
        /// <param name="APage"></param>
        /// <param name="APageSize"></param>
        /// <param name="ATotalRecords"></param>
        /// <param name="ATotalPages"></param>
        /// <returns></returns>
        public DataTable GetDataPagedResult(System.Int16 APage, System.Int16 APageSize, out System.Int32 ATotalRecords, out System.Int16 ATotalPages)
        {
            TLogging.LogAtLevel(7, "TPartnerLocationFindUIConnector.GetDataPagedResult called.");
            DataTable ReturnValue;

            ReturnValue = FPagedDataSetObject.GetData(APage, APageSize);
            ATotalPages = FPagedDataSetObject.TotalPages;
            ATotalRecords = FPagedDataSetObject.TotalRecords;
            return (PartnerFindTDSSearchResultTable)ReturnValue;
        }
    }
}
