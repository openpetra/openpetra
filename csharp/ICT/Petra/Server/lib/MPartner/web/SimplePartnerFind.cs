//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
// Copyright 2013-2014 by SolidCharity
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using System.Globalization;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.PartnerFind;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// functions for a very simple partner find control
    /// </summary>
    public class TSimplePartnerFindWebConnector
    {
        /// <summary>
        /// Return all partners that match the given criteria. This is used for the partner import screen.
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerFindTDS FindPartners(string AFirstName, string AFamilyNameOrOrganisation, string ACity, string APartnerClass)
        {
            TPartnerFind PartnerFind = new TPartnerFind();

            PartnerFindTDSSearchCriteriaTable CriteriaData = new PartnerFindTDSSearchCriteriaTable();
            PartnerFindTDSSearchCriteriaRow CriteriaRow = CriteriaData.NewRowTyped();

            CriteriaData.Rows.Add(CriteriaRow);
            CriteriaRow.PartnerName = AFamilyNameOrOrganisation;

            // CriteriaRow.PersonalName = AFirstName;
            CriteriaRow.City = ACity;

            // TODO: only works for one partner class at the moment
            if (APartnerClass.Length > 0)
            {
                CriteriaRow.PartnerClass = APartnerClass;
            }
            else
            {
                CriteriaRow.PartnerClass = "*";
            }

            PartnerFind.PerformSearch(CriteriaData, true);

            Int32 TotalRecords;
            short TotalPages;
            const short MaxRecords = 50;

            PartnerFindTDS result = new PartnerFindTDS();

            DataTable typedResult = PartnerFind.GetDataPagedResult(0, MaxRecords, out TotalRecords, out TotalPages);

            if (typedResult != null)
            {
                result.SearchResult.Merge(typedResult);

                if (TotalRecords > MaxRecords)
                {
                    // TODO load all data into the datatable. the webconnector does not have paging yet?
                }
            }

            return result;
        }

        /// <summary>
        /// Return all partners that match the given criteria. This is used for the partner import screen.
        /// </summary>
        /// <param name="APartnerKey">The key to find</param>
        /// <param name="AExactMatch">If true a search is made for an exact match.  If false the search may return multiple partners with a near match
        /// if APartnerKey ends in one or more zero's.</param>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerFindTDS FindPartners(Int64 APartnerKey, Boolean AExactMatch)
        {
            TPartnerFind PartnerFind = new TPartnerFind();

            PartnerFindTDSSearchCriteriaTable CriteriaData = new PartnerFindTDSSearchCriteriaTable();
            PartnerFindTDSSearchCriteriaRow CriteriaRow = CriteriaData.NewRowTyped();

            CriteriaData.Rows.Add(CriteriaRow);
            CriteriaRow.PartnerKey = APartnerKey;
            CriteriaRow.PartnerClass = "*";
            CriteriaRow.ExactPartnerKeyMatch = AExactMatch;

            PartnerFind.PerformSearch(CriteriaData, true);

            // NOTE from AlanP - Dec 2015:
            // If CriteriaRow.ExactPartnerKeyMatch is false and the value of APartnerKey is, say, 0012345600 the search will return all partners between
            //  001234500 and 001234599.  This means that if we request 50 records we may not actually get the partnerkey record we asked for.
            Int32 TotalRecords;
            short TotalPages;
            const short MaxRecords = 50;

            PartnerFindTDS result = new PartnerFindTDS();

            DataTable typedResult = PartnerFind.GetDataPagedResult(0, MaxRecords, out TotalRecords, out TotalPages);

            if (typedResult != null)
            {
                result.SearchResult.Merge(typedResult);

                if (TotalRecords > MaxRecords)
                {
                    // TODO load all data into the datatable. the webconnector does not have paging yet?
                    // See above NOTE if ExactPartnerMatch is false!
                }
            }

            return result;
        }
    }
}