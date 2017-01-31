//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Threading;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Session;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MPartner.Extracts;
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared.Interfaces.MPartner;

namespace Ict.Petra.Server.MPartner.PartnerFind
{
    /// <summary>
    /// Base for the Partner Find Screen UIConnector.
    /// We need this in MPartner.Common, so that we can use the functionality from the SimplePartnerFind WebConnector as well
    /// </summary>
    public class TPartnerFind
    {
        /// <summary>Paged query object</summary>
        TPagedDataSet FPagedDataSetObject;

        /// <summary>Thread that is used for asynchronously executing the Find query</summary>
        Thread FFindThread;

        /// <summary>Returns current state of progress</summary>
        public TProgressState Progress
        {
            get
            {
                return FPagedDataSetObject.Progress;
            }
        }

        /// <summary>
        /// Procedure to execute a Find query. Although the full query results are retrieved from the DB and stored
        /// internally in an object, data will be returned in 'pages' of data, each page holding a defined number
        /// of records.
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Partner Find parameters.</param>
        /// <param name="ADetailedResults">Returns more (when true) or less (when false) columns.</param>
        public void PerformSearch(DataTable ACriteriaData, bool ADetailedResults)
        {
            String CustomWhereCriteria;
            Hashtable ColumnNameMapping;

            OdbcParameter[] ParametersArray;
            String FieldList;
            String FromClause;
            String WhereClause;
            System.Text.StringBuilder sb;
            DataRow CriteriaRow;
            TLogging.LogAtLevel(7, "TPartnerFind.PerformSearch called.");

            FPagedDataSetObject = new TPagedDataSet(new PartnerFindTDSSearchResultTable());

            // Build WHERE criteria string based on AFindCriteria
            CustomWhereCriteria = BuildCustomWhereCriteria(ACriteriaData, out ParametersArray);

            //
            // Set up find parameters
            //
            ColumnNameMapping = new Hashtable();
            CriteriaRow = ACriteriaData.Rows[0];

            // Create Field List
            sb = new System.Text.StringBuilder();
            sb.AppendFormat("{0},{1}", "PUB.p_partner.p_partner_class_c", Environment.NewLine);

            // short
            sb.AppendFormat("{0},{1}", "PUB.p_partner.p_partner_short_name_c", Environment.NewLine);

            sb.AppendFormat("{0},{1}", "PUB.p_location.p_city_c", Environment.NewLine);

            // short
            if (ADetailedResults == true)
            {
                sb.AppendFormat("{0},{1}", "PUB.p_location.p_postal_code_c", Environment.NewLine);
            }

            if (ADetailedResults == true)
            {
                sb.AppendFormat("{0},{1}", "PUB.p_location.p_locality_c", Environment.NewLine);
            }

            sb.AppendFormat("{0},{1}", "PUB.p_location.p_street_name_c", Environment.NewLine);

            // short
            if (ADetailedResults == true)
            {
                sb.AppendFormat("{0},{1}", "PUB.p_location.p_address_3_c", Environment.NewLine);
            }

            if (ADetailedResults == true)
            {
                sb.AppendFormat("{0},{1}", "PUB.p_location.p_county_c", Environment.NewLine);
            }

            if (ADetailedResults == true)
            {
                sb.AppendFormat("{0},{1}", "PUB.p_location.p_country_code_c", Environment.NewLine);
            }

            sb.AppendFormat("{0},{1}", "PUB.p_partner.p_partner_key_n", Environment.NewLine);

            if ((ADetailedResults == true)
                && ((CriteriaRow["PartnerClass"].ToString() == "PERSON")
                    || (CriteriaRow["PartnerClass"].ToString() == "*")))
            {
                sb.AppendFormat("{0},{1}", "PUB.p_person.p_family_key_n", Environment.NewLine);
                sb.AppendFormat("{0},{1}", "PUB.p_person.p_date_of_birth_d", Environment.NewLine);
            }

            sb.AppendFormat("{0},{1}", "PUB.p_partner_location.p_location_type_c", Environment.NewLine);

            if (ADetailedResults == true)
            {
                sb.AppendFormat("{0},{1}", "PUB.p_partner.p_previous_name_c", Environment.NewLine);
            }

            sb.AppendFormat("{0},{1}", "PUB.p_partner.p_status_code_c", Environment.NewLine);

            sb.AppendFormat("{0},{1}", "PUB.p_partner.p_acquisition_code_c", Environment.NewLine);

            sb.AppendFormat("{0},{1}", "PUB.p_partner.s_date_created_d", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_partner.s_created_by_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_partner.s_modification_id_t", Environment.NewLine);

            // short
            sb.AppendFormat("{0},{1}", "PUB.p_location.p_location_key_i", Environment.NewLine);

            // short
            sb.AppendFormat("{0}{1}", "PUB.p_partner_location.p_site_key_n", Environment.NewLine);

            // short
            FieldList = sb.ToString();

            // Create FROM From Clause
            sb = new System.Text.StringBuilder();
            System.Text.StringBuilder sbWhereClause = new System.Text.StringBuilder();

            // Crude Optimisation
            if ((CriteriaRow["locationKey"].ToString().Length > 0) || (CriteriaRow["PostCode"].ToString().Length > 0)
                || (CriteriaRow["Address1"].ToString().Length > 0) || (CriteriaRow["Address2"].ToString().Length > 0)
                || (CriteriaRow["Address3"].ToString().Length > 0) || (CriteriaRow["County"].ToString().Length > 0)
                || (CriteriaRow["Country"].ToString().Length > 0))
            {
                // If we are searching on p_location fields then
                // essential that the FIRST table referenced is p_Location
                // or we wait seconds for Progress
                sb.AppendFormat("{0}{1}", "PUB.p_location, ", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "PUB.p_partner_location", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "LEFT OUTER JOIN PUB.p_partner", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "ON PUB.p_partner.p_partner_key_n = PUB.p_partner_location.p_partner_key_n", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "LEFT OUTER JOIN PUB.p_person", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "ON PUB.p_person.p_partner_key_n = PUB.p_partner.p_partner_key_n", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "LEFT OUTER JOIN PUB.p_family", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "ON PUB.p_family.p_partner_key_n = PUB.p_partner.p_partner_key_n", Environment.NewLine);
                sbWhereClause.AppendFormat("{0}{1}", "PUB.p_location.p_location_key_i = PUB.p_partner_location.p_location_key_i", Environment.NewLine);
                sbWhereClause.AppendFormat("{0}{1}", "AND PUB.p_location.p_site_key_n = PUB.p_partner_location.p_site_key_n", Environment.NewLine);
            }
            else
            {
                // normally p_partner is first table referenced
                sb.AppendFormat("{0}{1}", "PUB.p_partner", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "LEFT OUTER JOIN PUB.p_partner_location", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "ON PUB.p_partner.p_partner_key_n = PUB.p_partner_location.p_partner_key_n", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "LEFT OUTER JOIN PUB.p_person", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "ON PUB.p_person.p_partner_key_n = PUB.p_partner.p_partner_key_n", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "LEFT OUTER JOIN PUB.p_family", Environment.NewLine);
                sb.AppendFormat("{0}{1}", "ON PUB.p_family.p_partner_key_n = PUB.p_partner.p_partner_key_n", Environment.NewLine);
                sb.AppendFormat("{0}{1}", ", PUB.p_location", Environment.NewLine);
                sbWhereClause.AppendFormat("{0}{1}", "PUB.p_partner_location.p_location_key_i = PUB.p_location.p_location_key_i", Environment.NewLine);
                sbWhereClause.AppendFormat("{0}{1}", "AND PUB.p_location.p_site_key_n = PUB.p_partner_location.p_site_key_n", Environment.NewLine);
            }

            FromClause = sb.ToString();
            WhereClause = CustomWhereCriteria;

            if (WhereClause.StartsWith(" AND") == true)
            {
                WhereClause = WhereClause.Substring(4);
            }

            if (sbWhereClause.ToString().Length > 0)
            {
                WhereClause += " AND " + sbWhereClause.ToString();
            }

            FPagedDataSetObject.FindParameters = new TPagedDataSet.TAsyncFindParameters(FieldList,
                FromClause,
                WhereClause,
                "PUB.p_partner.p_partner_short_name_c, PUB.p_partner.p_partner_class_c",
                ColumnNameMapping,
                ParametersArray);

            string session = TSession.GetSessionID();

            //
            // Start the Find Thread
            //
            try
            {
                ThreadStart myThreadStart = delegate {
                    FPagedDataSetObject.ExecuteQuery(session, "Partner Find (by Partner Details)");
                };
                FFindThread = new Thread(myThreadStart);
                FFindThread.Name = "PartnerFindPerformSearch" + Guid.NewGuid().ToString();
                FFindThread.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Procedure to execute a Find query. Although the full
        /// query results are retrieved from the DB and stored internally in an object,
        /// data will be returned in 'pages' of data, each page holding a defined number
        /// of records.
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Partner Find parameters.</param>
        public void PerformSearchByBankDetails(DataTable ACriteriaData)
        {
            String CustomWhereCriteria;
            Hashtable ColumnNameMapping;

            OdbcParameter[] ParametersArray;
            String FieldList;
            String FromClause;
            String WhereClause;
            System.Text.StringBuilder sb;
            TLogging.LogAtLevel(7, "TPartnerFind.PerformSearchByBankDetails called.");

            FPagedDataSetObject = new TPagedDataSet(new PartnerFindTDSSearchResultTable());

            // Build WHERE criteria string based on AFindCriteria
            CustomWhereCriteria = BuildCustomWhereCriteriaForBankDetails(ACriteriaData, out ParametersArray);

            //
            // Set up find parameters
            //
            ColumnNameMapping = new Hashtable();

            // Create Field List
            sb = new System.Text.StringBuilder();
            sb.AppendFormat("{0},{1}", "PUB.p_partner.p_partner_class_c", Environment.NewLine);

            // short
            sb.AppendFormat("{0},{1}", "PUB.p_partner.p_partner_short_name_c", Environment.NewLine);

            sb.AppendFormat("{0},{1}", "PUB.p_partner.p_partner_key_n", Environment.NewLine);

            sb.AppendFormat("{0},{1}", "PUB.p_partner.p_status_code_c", Environment.NewLine);

            sb.AppendFormat("{0},{1}", "PUB.p_partner.p_acquisition_code_c", Environment.NewLine);

            // search by bank details
            sb.AppendFormat("{0},{1}", "PUB.p_banking_details.p_banking_details_key_i", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_banking_details.p_account_name_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_banking_details.p_bank_account_number_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_banking_details.p_iban_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_banking_details.p_expiry_date_d", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_banking_details.p_comment_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_bank.p_bic_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_bank.p_partner_key_n", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_bank.p_branch_name_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_bank.p_branch_code_c", Environment.NewLine);

            sb.AppendFormat("{0},{1}", "PUB.p_partner.s_date_created_d", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_partner.s_created_by_c", Environment.NewLine);
            sb.AppendFormat("{0}{1}", "PUB.p_partner.s_modification_id_t", Environment.NewLine);

            // short
            FieldList = sb.ToString();

            // Create FROM From Clause
            sb = new System.Text.StringBuilder();
            System.Text.StringBuilder sbWhereClause = new System.Text.StringBuilder();

            sb.AppendFormat("{0},{1}", "PUB.p_banking_details", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_bank", Environment.NewLine);
            sb.AppendFormat("{0}{1}", "PUB.p_partner_banking_details", Environment.NewLine);
            sb.AppendFormat("{0}{1}", "LEFT OUTER JOIN PUB.p_partner", Environment.NewLine);
            sb.AppendFormat("{0}{1}", "ON PUB.p_partner.p_partner_key_n = PUB.p_partner_banking_details.p_partner_key_n", Environment.NewLine);
            sb.AppendFormat("{0}{1}", "LEFT OUTER JOIN PUB.p_person", Environment.NewLine);
            sb.AppendFormat("{0}{1}", "ON PUB.p_person.p_partner_key_n = PUB.p_partner.p_partner_key_n", Environment.NewLine);
            sb.AppendFormat("{0}{1}", "LEFT OUTER JOIN PUB.p_family", Environment.NewLine);
            sb.AppendFormat("{0}{1}", "ON PUB.p_family.p_partner_key_n = PUB.p_partner.p_partner_key_n", Environment.NewLine);
            sbWhereClause.AppendFormat("{0}{1}",
                "PUB.p_banking_details.p_banking_details_key_i = PUB.p_partner_banking_details.p_banking_details_key_i",
                Environment.NewLine);
            sbWhereClause.AppendFormat("{0}{1}", "AND PUB.p_bank.p_partner_key_n = PUB.p_banking_details.p_bank_key_n", Environment.NewLine);

            FromClause = sb.ToString();
            WhereClause = CustomWhereCriteria;

            if (WhereClause.StartsWith(" AND") == true)
            {
                WhereClause = WhereClause.Substring(4);
            }

            if (sbWhereClause.ToString().Length > 0)
            {
                WhereClause += " AND " + sbWhereClause.ToString();
            }

            FPagedDataSetObject.FindParameters = new TPagedDataSet.TAsyncFindParameters(FieldList,
                FromClause,
                WhereClause,
                "PUB.p_partner.p_partner_short_name_c, PUB.p_partner.p_partner_class_c",
                ColumnNameMapping,
                ParametersArray);

            string session = TSession.GetSessionID();

            //
            // Start the Find Thread
            //
            try
            {
                ThreadStart myThreadStart = delegate {
                    FPagedDataSetObject.ExecuteQuery(session, "Partner Find (by Bank Details)");
                };
                FFindThread = new Thread(myThreadStart);
                FFindThread.Start();
            }
            catch (Exception)
            {
                throw;
            }
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
            DataTable ReturnValue;

            TLogging.LogAtLevel(7, "TPartnerFind.GetDataPagedResult called.");
            ReturnValue = FPagedDataSetObject.GetData(APage, APageSize);
            ATotalPages = FPagedDataSetObject.TotalPages;
            ATotalRecords = FPagedDataSetObject.TotalRecords;

            // Thread.Sleep(500);    enable only for simulation of slow (Internet) connection!

            if (ReturnValue != null)
            {
                TPPartnerAddressAggregate.ApplySecurity(ref ReturnValue);
            }

            return ReturnValue;
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
            Int16 ReturnValue = 1;

            TLogging.LogAtLevel(7, "TPartnerFind.GetDataPagedResult called.");

            for (Int16 i = AStartPage; i <= FPagedDataSetObject.TotalPages; i++)
            {
                // load a page
                DataTable Table = FPagedDataSetObject.GetData(i, APageSize);

                // look for partner key on this page
                foreach (DataRow Row in Table.Rows)
                {
                    if (Convert.ToInt64(Row["p_partner_key_n"]) == APartnerKey)
                    {
                        return i;
                    }
                }
            }

            return ReturnValue;
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
            String PartnerKey;
            DataTable CriteriaDataTable;
            DataRow CriteriaRow;
            ArrayList InternalParameters;
            bool ExactPartnerKeyMatch;
            bool EmailAddressIsSearchedFor;
            bool PhoneNumberIsSearchedFor;
            Int32 pk_order;
            Int32 pk_power;
            Int64 pk_maxkey;
            Int64 pk_minkey;

            CriteriaDataTable = ACriteriaData;
            CriteriaRow = CriteriaDataTable.Rows[0];
            InternalParameters = new ArrayList();

            if (CriteriaRow["PartnerName"].ToString().Length > 0)
            {
                // Searched DB Field: 'p_partner_short_name_c'
                new TDynamicSearchHelper(PPartnerTable.TableId,
                    PPartnerTable.ColumnPartnerShortNameId, CriteriaRow, "PartnerName", "PartnerNameMatch",
                    ref CustomWhereCriteria, ref InternalParameters);
            }

            if (CriteriaRow["PersonalName"].ToString().Length > 0)
            {
                // Searched DB Field: 'p_first_name_c'
                // Search for family or person or both?
                if (CriteriaRow["PartnerClass"].ToString() == "PERSON")
                {
                    new TDynamicSearchHelper(PPersonTable.TableId,
                        PPersonTable.ColumnFirstNameId, CriteriaRow, "PersonalName", "PersonalNameMatch",
                        ref CustomWhereCriteria, ref InternalParameters);
                }
                else if (CriteriaRow["PartnerClass"].ToString() == "FAMILY")
                {
                    new TDynamicSearchHelper(PFamilyTable.TableId,
                        PFamilyTable.ColumnFirstNameId, CriteriaRow, "PersonalName", "PersonalNameMatch",
                        ref CustomWhereCriteria, ref InternalParameters);
                }
                else if (CriteriaRow["PartnerClass"].ToString() == "*")
                {
                    // search for first name in both family and person
                    String Criteria = " AND (";
                    String SubCriteria = "";

                    new TDynamicSearchHelper(PPersonTable.TableId,
                        PPersonTable.ColumnFirstNameId, CriteriaRow, "PersonalName", "PersonalNameMatch",
                        ref SubCriteria, ref InternalParameters);

                    Criteria += SubCriteria.Remove(0, 4) + " OR ";
                    SubCriteria = "";

                    new TDynamicSearchHelper(PFamilyTable.TableId,
                        PFamilyTable.ColumnFirstNameId, CriteriaRow, "PersonalName", "PersonalNameMatch",
                        ref SubCriteria, ref InternalParameters);

                    Criteria += SubCriteria.Remove(0, 4) + ")";

                    CustomWhereCriteria += Criteria;
                }
            }

            if (CriteriaRow["PreviousName"].ToString().Length > 0)
            {
                // Searched DB Field: 'p_previous_name_c'
                new TDynamicSearchHelper(PPartnerTable.TableId,
                    PPartnerTable.ColumnPreviousNameId, CriteriaRow, "PreviousName", "PreviousNameMatch",
                    ref CustomWhereCriteria, ref InternalParameters);
            }

            if (CriteriaRow["PartnerClass"].ToString() != "*")
            {
                // Split String into String Array is Restricted Partner Classes are being used
                string[] Classes = CriteriaRow["PartnerClass"].ToString().Split(new Char[] { (',') });

                String Criteria = null;

                foreach (string Class in Classes)
                {
                    if (Class != "*")
                    {
                        if (Criteria == null)
                        {
                            Criteria = " AND (";
                        }
                        else
                        {
                            Criteria += " OR ";
                        }

                        // Searched DB Field: 'p_partner_class_c': done manually!
                        Criteria = String.Format("{0} PUB.{1}.{2} = ?", Criteria,
                            PPartnerTable.GetTableDBName(),
                            PPartnerTable.GetPartnerClassDBName());
                        OdbcParameter miParam = TTypedDataTable.CreateOdbcParameter(PPartnerTable.TableId, PPartnerTable.ColumnPartnerClassId);
                        miParam.Value = (object)Class;

                        InternalParameters.Add(miParam);
                    }
                }

                CustomWhereCriteria += Criteria + ")";
            }

            if (CriteriaRow["Address1"].ToString().Length > 0)
            {
                // Searched DB Field: 'p_locality_c'
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnLocalityId, CriteriaRow, "Address1", "Address1Match",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            if (CriteriaRow["Address2"].ToString().Length > 0)
            {
                // Searched DB Field: 'p_street_name_c'
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnStreetNameId, CriteriaRow, "Address2", "Address2Match",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            if (CriteriaRow["Address3"].ToString().Length > 0)
            {
                // Searched DB Field: 'p_address_3_c'
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnAddress3Id, CriteriaRow, "Address3", "Address3Match",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'p_city_c'
            if (CriteriaRow["City"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnCityId, CriteriaRow, "City", "CityMatch", ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'p_postal_code_c'
            if (CriteriaRow["PostCode"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnPostalCodeId, CriteriaRow, "PostCode", "PostCodeMatch",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'p_county_c'
            if (CriteriaRow["County"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PLocationTable.TableId,
                    PLocationTable.ColumnCountyId, CriteriaRow, "County", "CountyMatch", ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'p_country_code_c' (Country): done manually
            if (CriteriaRow["Country"].ToString().Length > 0)
            {
                CustomWhereCriteria = String.Format("{0} AND {1} = ?", CustomWhereCriteria, PLocationTable.GetCountryCodeDBName());                 // CustomWhereCriteria + ' AND p_country_code_c = ?';
                OdbcParameter miParam = new OdbcParameter("", OdbcType.VarChar, 8);
                miParam.Value = (object)(CriteriaRow["Country"].ToString().ToUpper());
                InternalParameters.Add(miParam);
            }

            // Searched DB Field: 'p_account_name_c' (AccountName): done manually
            if (CriteriaRow["AccountName"].ToString().Length > 0)
            {
                // Searched DB Field: 'p_partner_short_name_c'
                new TDynamicSearchHelper(PBankingDetailsTable.TableId,
                    PBankingDetailsTable.ColumnAccountNameId, CriteriaRow, "AccountName", "AccountNameMatch",
                    ref CustomWhereCriteria, ref InternalParameters);
            }

            if ((Boolean)(CriteriaRow["MailingAddressOnly"]) == true)
            {
                // Searched DB Fields: 'p_send_mail_l', 'p_date_effective_d', 'p_date_good_until_d'
                CustomWhereCriteria = String.Format(
                    "{0} AND (PUB.p_partner_location.p_send_mail_l = true) AND (PUB.p_partner_location.p_date_effective_d <= ?) and (PUB.p_partner_location.p_date_good_until_d >= ? or PUB.p_partner_location.p_date_good_until_d is null)",
                    CustomWhereCriteria,
                    PPartnerLocationTable.GetSendMailDBName());  // CustomWhereCriteria + ' AND p_send_mail_l = true'

                OdbcParameter miParam = new OdbcParameter("", OdbcType.Date);
                miParam.Value = System.DateTime.Now;
                InternalParameters.Add(miParam);
                miParam = new OdbcParameter("", OdbcType.Date);
                miParam.Value = System.DateTime.Now;
                InternalParameters.Add(miParam);
            }

            if (((Boolean)(CriteriaRow["WorkerFamOnly"]) == true)
                && (CriteriaRow["PartnerClass"].ToString().StartsWith("FAMILY")))
            {
                string temp =
                    "EXISTS (select * FROM PUB.p_partner_gift_destination " +
                    "WHERE PUB.p_partner.p_partner_key_n = PUB.p_partner_gift_destination.p_partner_key_n " +
                    "AND (PUB.p_partner_gift_destination.p_date_expires_d IS NULL OR PUB.p_partner_gift_destination.p_date_effective_d <> PUB.p_partner_gift_destination.p_date_expires_d))";

                // A custom subquery seems to only speedy way of doing this!
                if (CriteriaRow["PartnerClass"].ToString() == "FAMILY")
                {
                    CustomWhereCriteria = String.Format(
                        "{0} AND " + temp,
                        CustomWhereCriteria);
                }
                else
                {
                    CustomWhereCriteria = String.Format(
                        "{0} AND (PUB.p_partner.p_partner_class_c = 'UNIT' OR " + temp + ")",
                        CustomWhereCriteria);
                }
            }

            if (CriteriaRow["LocationKey"].ToString().Length > 0)
            {
                // Searched DB Field: 'p_location_key_i'

                // DISREGARD ALL OTHER SEARCH CRITERIA!!!
                CustomWhereCriteria = "";
                InternalParameters.Clear();

                CustomWhereCriteria = String.Format("{0} AND PUB.{1}.{2} = ?", CustomWhereCriteria,
                    PLocationTable.GetTableDBName(), PLocationTable.GetLocationKeyDBName());

                // CustomWhereCriteria + '
                // AND p_location_key_i = ?';

                OdbcParameter miParam = new OdbcParameter("", OdbcType.Decimal, 10);
                miParam.Value = (object)CriteriaRow["LocationKey"];
                InternalParameters.Add(miParam);

                // if search is restricted then add this to the search criteria
                AddRestrictedClassesToCriteria(CriteriaRow, ref InternalParameters, ref CustomWhereCriteria);
            }

            // if AFindCriteria.Contains('Partner Key') then
            if ((Int64)(CriteriaRow["PartnerKey"]) != 0)
            {
                // Searched DB Field: 'p_partner_key_n'

                PartnerKey = CriteriaRow["PartnerKey"].ToString();
                ExactPartnerKeyMatch = (Boolean)(CriteriaRow["ExactPartnerKeyMatch"]);

                if (ExactPartnerKeyMatch == false)
                {
                    /* PARTNER KEY RANGE SEARCH:
                     * This was the method that was used by the Progress 4GL Partner Find screen.
                     */
                    pk_minkey = System.Convert.ToInt64(PartnerKey);
                    pk_maxkey = 1;

                    // need to do TrimEnd('0').
                    pk_order = pk_minkey.ToString().Length -
                               pk_minkey.ToString().TrimEnd(new Char[] { ('0') }).Length;

                    for (pk_power = 1; pk_power <= pk_order; pk_power += 1)
                    {
                        pk_maxkey = pk_maxkey * 10;
                    }

                    pk_maxkey = pk_maxkey + pk_minkey - 1;
                    CustomWhereCriteria = String.Format("{0} AND {1} BETWEEN ? AND ?",
                        CustomWhereCriteria,
                        "PUB.p_partner.p_partner_key_n");
                    OdbcParameter miParam = new OdbcParameter("", OdbcType.Decimal, 10);
                    miParam.Value = (object)pk_minkey;
                    InternalParameters.Add(miParam);
                    miParam = new OdbcParameter("", OdbcType.Decimal, 10);
                    miParam.Value = (object)pk_maxkey;
                    InternalParameters.Add(miParam);
                }
                else
                {
                    // EXACT PARTNER KEY SEARCH

                    // DISREGARD ALL OTHER SEARCH CRITERIA!!!

                    CustomWhereCriteria = "";
                    InternalParameters.Clear();

                    CustomWhereCriteria = String.Format("{0} AND {1} = ?",
                        CustomWhereCriteria,
                        "PUB.p_partner.p_partner_key_n");

                    // CustomWhereCriteria := CustomWhereCriteria + ' AND PUB_p_partner.p_partner_key_n = ?';

                    OdbcParameter miParam = new OdbcParameter("", OdbcType.Decimal, 10);
                    miParam.Value = (object)CriteriaRow["PartnerKey"];
                    InternalParameters.Add(miParam);

                    // if search is restricted then add this to the search criteria
                    AddRestrictedClassesToCriteria(CriteriaRow, ref InternalParameters, ref CustomWhereCriteria);
                }
            }

            EmailAddressIsSearchedFor = CriteriaRow["Email"].ToString().Length > 0;
            PhoneNumberIsSearchedFor = CriteriaRow["PhoneNumber"].ToString().Length > 0;

            if (EmailAddressIsSearchedFor || PhoneNumberIsSearchedFor)
            {
                BuildContactDetailsExtraCriteria(PhoneNumberIsSearchedFor, ref CriteriaRow, ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // this is only relevant for Person searches
            if ((CriteriaRow["DateOfBirth"]).ToString().Length > 0)
            {
                // Searched DB Field: 'p_date_of_birth_d'

                CustomWhereCriteria = String.Format("{0} AND PUB.{1}.{2} = ?", CustomWhereCriteria,
                    PPersonTable.GetTableDBName(), PPersonTable.GetDateOfBirthDBName());

                OdbcParameter miParam = new OdbcParameter("DateOfBirth", OdbcType.Date, 16);
                miParam.Value = CriteriaRow["DateOfBirth"];
                InternalParameters.Add(miParam);
            }

            // this is only relevant for Person searches
            if (((CriteriaRow["FamilyKey"]).ToString().Length > 0)
                && ((Int64)(CriteriaRow["FamilyKey"]) != 0))
            {
                // Searched DB Field: 'p_family_key_n'

                CustomWhereCriteria = String.Format("{0} AND PUB.{1}.{2} = ?", CustomWhereCriteria,
                    PPersonTable.GetTableDBName(), PPersonTable.GetFamilyKeyDBName());

                OdbcParameter miParam = new OdbcParameter("FamilyKey", OdbcType.Decimal, 10);
                miParam.Value = (object)CriteriaRow["FamilyKey"];
                InternalParameters.Add(miParam);
            }

            #region Partner Status

            if ((String)(CriteriaRow["PartnerStatus"]) == "ACTIVE")
            {
                // Searched DB Field: 'p_status_code_c'

                // This will automatically exclude other peoples' PRIVATE Partners!
                CustomWhereCriteria = String.Format("{0} AND PUB.{1}.{2} = ?", CustomWhereCriteria,  // CustomWhereCriteria + ' AND p_status_code_c = ?';
                    PPartnerTable.GetTableDBName(),
                    PPartnerTable.GetStatusCodeDBName());

                OdbcParameter miParam = new OdbcParameter("", OdbcType.VarChar, 16);
                miParam.Value = "ACTIVE";
                InternalParameters.Add(miParam);
            }

            if ((String)(CriteriaRow["PartnerStatus"]) == "PRIVATE")
            {
                // Searched DB Fields: 'p_status_code_c', 'p_user_id_c'
                CustomWhereCriteria = String.Format("{0} AND PUB.{1}.{2} = ? AND PUB.{3}.{4} = ?", CustomWhereCriteria,  // CustomWhereCriteria + ' AND p_status_code_c = ?';
                    PPartnerTable.GetTableDBName(),
                    PPartnerTable.GetStatusCodeDBName(),
                    PPartnerTable.GetTableDBName(),
                    PPartnerTable.GetUserIdDBName());


                OdbcParameter miParam = new OdbcParameter("", OdbcType.VarChar, 16);
                miParam.Value = "PRIVATE";
                InternalParameters.Add(miParam);

                // user must be current user
                miParam = new OdbcParameter("", OdbcType.VarChar, 20);
                miParam.Value = UserInfo.GUserInfo.UserID;
                InternalParameters.Add(miParam);
            }

            if ((String)(CriteriaRow["PartnerStatus"]) == "ALL")
            {
                // Searched DB Fields: 'p_status_code_c', 'p_user_id_c'

                // This must show all partners PLUS the users *own* PRIVATE partners
                CustomWhereCriteria = String.Format("{0} AND ((PUB.{1}.{2} = ? AND PUB.{3}.{4} = ? ) OR PUB.{1}.{2} <> ? )",
                    CustomWhereCriteria,
                    PPartnerTable.GetTableDBName(),
                    PPartnerTable.GetStatusCodeDBName(),
                    PPartnerTable.GetTableDBName(),
                    PPartnerTable.GetUserIdDBName());

                OdbcParameter miParam = new OdbcParameter("", OdbcType.VarChar, 16);
                miParam.Value = "PRIVATE";
                InternalParameters.Add(miParam);

                // user must be current user
                miParam = new OdbcParameter("", OdbcType.VarChar, 20);
                miParam.Value = UserInfo.GUserInfo.UserID;
                InternalParameters.Add(miParam);

                miParam = new OdbcParameter("", OdbcType.VarChar, 16);
                miParam.Value = "PRIVATE";
                InternalParameters.Add(miParam);
            }

            #endregion

//           TLogging.LogAtLevel(7, "CustomWhereCriteria: " + CustomWhereCriteria);

            /* Convert ArrayList to a array of ODBCParameters
             * seem to need to declare a type first
             */
            AParametersArray = ((OdbcParameter[])(InternalParameters.ToArray(typeof(OdbcParameter))));
            InternalParameters = null;             // ensure this is GC'd

            return CustomWhereCriteria;
        }

        private static void BuildContactDetailsExtraCriteria(bool APhoneNumberIsSearchedFor, ref DataRow ACriteriaRow,
            ref string ACustomWhereCriteria, ref ArrayList AInternalParameters)
        {
            string CustomSubQueryStr = String.Empty;
            string AttributeTypeListStr = String.Empty;
            string CountryCodeListStr = String.Empty;
            string CountryCodeWhereArgument = String.Empty;
            string PhoneNumber;
            string IntlPhonePrefix = String.Empty;
            int IndexOfIntlPhonePrefixSeparator;

            DataRow[] CountriesForIntlPhonePrefixDR;
            List <string>CountriesForIntlPhonePrefix = new List <string>(0);
            PCountryTable CountryDT;
            DataView PhoneAttributesDV;
            DataView EmailAttributesDV;

            // Searched DB Fields: None directly in the 'usual' query - custom 'Partner Contact Details' sub-queries are utilised!
            // --> DISREGARD ALL OTHER SEARCH CRITERIA!!!
            ACustomWhereCriteria = String.Empty;
            AInternalParameters.Clear();

            if (APhoneNumberIsSearchedFor)
            {
                // Build list of Partner Contact Attributes Types that constitute Phone Numbers and Fax Numbers
                PhoneAttributesDV = Calculations.DeterminePhoneAttributes((PPartnerAttributeTypeTable)
                    TSharedDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.ContactTypeList));

                for (int Counter = 0; Counter < PhoneAttributesDV.Count; Counter++)
                {
                    AttributeTypeListStr += "'" + ((PPartnerAttributeTypeRow)PhoneAttributesDV[Counter].Row).AttributeType + "', ";
                }

                // Strip off remaining Attribute Type separation concatenation characters
                AttributeTypeListStr = AttributeTypeListStr.Substring(0, AttributeTypeListStr.Length - 2);

                PhoneNumber = ACriteriaRow["PhoneNumber"].ToString();

                // Special processing is required if a Phone Number starts with an Int'l Phone Prefix
                if (PhoneNumber.StartsWith("+", StringComparison.InvariantCulture))
                {
                    // If we are to process an Int'l Phone Prefix then it must be separated with a space character from the
                    // rest of the Phone Number!

                    IndexOfIntlPhonePrefixSeparator = PhoneNumber.IndexOf(' ', 1);

                    if (IndexOfIntlPhonePrefixSeparator != -1)
                    {
                        IntlPhonePrefix = PhoneNumber.Substring(1, IndexOfIntlPhonePrefixSeparator);

                        // Get p_country DB Table from cache
                        CountryDT = (PCountryTable)TSharedDataCache.TMCommon.GetCacheableCommonTable(
                            TCacheableCommonTablesEnum.CountryList);

                        // Find any countries that have the specified Int'l Phone Prefix (there can be more than one Country
                        // with the same Int'l Phone Prefix! (Examples: +1, +7, +44, ...).
                        CountriesForIntlPhonePrefixDR = CountryDT.Select(PCountryTable.GetInternatTelephoneCodeDBName() +
                            " = " + IntlPhonePrefix);

                        for (int Counter2 = 0; Counter2 < CountriesForIntlPhonePrefixDR.Length; Counter2++)
                        {
                            CountriesForIntlPhonePrefix.Add(
                                CountriesForIntlPhonePrefixDR[Counter2][PCountryTable.GetCountryCodeDBName()].ToString());
                        }

                        // Check if matching countries were found
                        if (CountriesForIntlPhonePrefix.Count > 0)
                        {
                            for (int Counter3 = 0; Counter3 < CountriesForIntlPhonePrefix.Count; Counter3++)
                            {
                                CountryCodeListStr += "'" + CountriesForIntlPhonePrefix[Counter3] + "', ";
                            }

                            // Strip off remaining Attribute Type separation concatenation characters
                            CountryCodeListStr = CountryCodeListStr.Substring(0, CountryCodeListStr.Length - 2);

                            if (CountryCodeListStr != String.Empty)
                            {
                                CountryCodeWhereArgument = " AND " + PPartnerAttributeTable.GetValueCountryDBName() +
                                                           " IN (" + CountryCodeListStr + ")";

                                // Change the criteria that the user entered so it doesn't contain the Int'l Phone Prefix!
                                ACriteriaRow["PhoneNumber"] = ((string)ACriteriaRow["PhoneNumber"]).Substring(IndexOfIntlPhonePrefixSeparator + 1);
                            }
                        }
                    }
                }

                new TDynamicSearchHelper(PPartnerAttributeTable.TableId,
                    PPartnerAttributeTable.ColumnValueId, ACriteriaRow, "PhoneNumber", "PhoneNumberMatch",
                    ref ACustomWhereCriteria, ref AInternalParameters);

                CustomSubQueryStr =
                    String.Format(
                        "EXISTS (SELECT * FROM PUB.p_partner_attribute " +
                        "WHERE PUB.p_partner.p_partner_key_n = PUB.p_partner_attribute.p_partner_key_n " +
                        "{0} AND p_attribute_type_c IN ({1}){2})", ACustomWhereCriteria, AttributeTypeListStr, CountryCodeWhereArgument);
            }
            else
            {
                // Build list of Partner Contact Attributes Types that constitute Email Addresses
                EmailAttributesDV = Calculations.DetermineEmailAttributes((PPartnerAttributeTypeTable)
                    TSharedDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.ContactTypeList));

                for (int Counter = 0; Counter < EmailAttributesDV.Count; Counter++)
                {
                    AttributeTypeListStr += "'" + ((PPartnerAttributeTypeRow)EmailAttributesDV[Counter].Row).AttributeType + "', ";
                }

                // Strip off remaining Attribute Type separation concatenation characters
                AttributeTypeListStr = AttributeTypeListStr.Substring(0, AttributeTypeListStr.Length - 2);

                new TDynamicSearchHelper(PPartnerAttributeTable.TableId,
                    PPartnerAttributeTable.ColumnValueId, ACriteriaRow, "Email", "EmailMatch",
                    ref ACustomWhereCriteria, ref AInternalParameters);

                CustomSubQueryStr =
                    String.Format(
                        "EXISTS (SELECT * FROM PUB.p_partner_attribute " +
                        "WHERE PUB.p_partner.p_partner_key_n = PUB.p_partner_attribute.p_partner_key_n " +
                        "{0} AND p_attribute_type_c IN ({1}))", ACustomWhereCriteria, AttributeTypeListStr);
            }

            // Utilising the 'TDynamicSearchHelper' alters the main SQL query; we don't want that and hence
            // need to re-set it to empty!
            ACustomWhereCriteria = String.Empty;

            // Need to creata a custom sub-query
            ACustomWhereCriteria = String.Format(
                "{0} AND " + CustomSubQueryStr,
                ACustomWhereCriteria);

            // if search is restricted then add this to the search criteria
            AddRestrictedClassesToCriteria(ACriteriaRow, ref AInternalParameters, ref ACustomWhereCriteria);
        }

        private static void AddRestrictedClassesToCriteria(DataRow ACriteriaRow, ref ArrayList AInternalParameters, ref string ACustomWhereCriteria)
        {
            bool IsDonorSearch;

            if ((ACriteriaRow["RestrictedParterClasses"] != null) && (ACriteriaRow["RestrictedParterClasses"].ToString().Length > 0))
            {
                // Split String into String Array is Restricted Partner Classes are being used
                string[] Classes = ACriteriaRow["RestrictedParterClasses"].ToString().Split(new Char[] { (',') });
                IsDonorSearch = (Classes.Length == 1) && (Classes[0] == "DONOR");

                String Criteria = null;

                if (IsDonorSearch
                    && (!TSystemDefaults.GetBooleanDefault(SharedConstants.SYSDEFAULT_ALLOWPERSONPARTNERSASDONORS, true)))
                {
                    if (Criteria == null)
                    {
                        Criteria = " AND (";
                    }
                    else
                    {
                        Criteria += " OR ";
                    }

                    // Searched DB Field: 'p_partner_class_c': done manually - we are searching for any Partner Class except PERSON
                    Criteria = String.Format("{0} PUB.{1}.{2} != ?", Criteria,
                        PPartnerTable.GetTableDBName(),
                        PPartnerTable.GetPartnerClassDBName());
                    OdbcParameter miParam = TTypedDataTable.CreateOdbcParameter(PPartnerTable.TableId, PPartnerTable.ColumnPartnerClassId);
                    miParam.Value = (object)"PERSON";

                    AInternalParameters.Add(miParam);
                }
                else
                {
                    if (!IsDonorSearch)
                    {
                        foreach (string Class in Classes)
                        {
                            if (Criteria == null)
                            {
                                Criteria = " AND (";
                            }
                            else
                            {
                                Criteria += " OR ";
                            }

                            if (Class == "WORKER-FAM")
                            {
                                // A custom subquery seems to only speedy way of doing this!
                                Criteria = String.Format(
                                    "{0}EXISTS (select * FROM PUB.p_partner_gift_destination " +
                                    "WHERE PUB.p_partner.p_partner_key_n = PUB.p_partner_gift_destination.p_partner_key_n " +
                                    "AND (PUB.p_partner_gift_destination.p_date_expires_d IS NULL OR PUB.p_partner_gift_destination.p_date_effective_d <> PUB.p_partner_gift_destination.p_date_expires_d))",
                                    Criteria);
                            }
                            else
                            {
                                // Searched DB Field: 'p_partner_class_c': done manually!
                                Criteria = String.Format("{0} PUB.{1}.{2} = ?", Criteria,
                                    PPartnerTable.GetTableDBName(),
                                    PPartnerTable.GetPartnerClassDBName());
                                OdbcParameter miParam = TTypedDataTable.CreateOdbcParameter(PPartnerTable.TableId, PPartnerTable.ColumnPartnerClassId);
                                miParam.Value = (object)Class;

                                AInternalParameters.Add(miParam);
                            }
                        }
                    }
                }

                if (Criteria != null)
                {
                    ACustomWhereCriteria += Criteria + ")";
                }
            }
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
        private String BuildCustomWhereCriteriaForBankDetails(DataTable ACriteriaData, out OdbcParameter[] AParametersArray)
        {
            String CustomWhereCriteria = "";
            DataTable CriteriaDataTable;
            DataRow CriteriaRow;
            ArrayList InternalParameters;

            CriteriaDataTable = ACriteriaData;
            CriteriaRow = CriteriaDataTable.Rows[0];
            InternalParameters = new ArrayList();

            if (CriteriaRow["PartnerName"].ToString().Length > 0)
            {
                // Searched DB Field: 'p_partner_short_name_c'
                new TDynamicSearchHelper(PPartnerTable.TableId,
                    PPartnerTable.ColumnPartnerShortNameId, CriteriaRow, "PartnerName", "PartnerNameMatch",
                    ref CustomWhereCriteria, ref InternalParameters);
            }

            if (CriteriaRow["PartnerClass"].ToString() != "*")
            {
                // Split String into String Array is Restricted Partner Classes are being used
                string[] Classes = CriteriaRow["PartnerClass"].ToString().Split(new Char[] { (',') });

                String Criteria = null;

                foreach (string Class in Classes)
                {
                    if (Class != "*")
                    {
                        if (Criteria == null)
                        {
                            Criteria = " AND (";
                        }
                        else
                        {
                            Criteria += " OR ";
                        }

                        // Searched DB Field: 'p_partner_class_c': done manually!
                        Criteria = String.Format("{0} PUB.{1}.{2} = ?", Criteria,
                            PPartnerTable.GetTableDBName(),
                            PPartnerTable.GetPartnerClassDBName());
                        OdbcParameter miParam = TTypedDataTable.CreateOdbcParameter(PPartnerTable.TableId, PPartnerTable.ColumnPartnerClassId);
                        miParam.Value = (object)Class;

                        InternalParameters.Add(miParam);
                    }
                }

                CustomWhereCriteria += Criteria + ")";
            }

            // Searched DB Field: 'p_bank_account_number_c'
            if (CriteriaRow["AccountNumber"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PBankingDetailsTable.TableId,
                    PBankingDetailsTable.ColumnBankAccountNumberId, CriteriaRow, "AccountNumber", "AccountNumberMatch",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'p_account_name_c'
            if (CriteriaRow["AccountName"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PBankingDetailsTable.TableId,
                    PBankingDetailsTable.ColumnAccountNameId, CriteriaRow, "AccountName", "AccountNameMatch", ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'p_account_name_c'
            if (CriteriaRow["Iban"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PBankingDetailsTable.TableId,
                    PBankingDetailsTable.ColumnIbanId, CriteriaRow, "Iban", "IbanMatch", ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'p_account_name_c'
            if (CriteriaRow["Bic"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(PBankTable.TableId,
                    PBankTable.ColumnBicId, CriteriaRow, "Bic", "BicMatch", ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'p_account_name_c'
            if (Convert.ToInt64(CriteriaRow["BankKey"]) > 0)
            {
                CustomWhereCriteria = String.Format("{0} AND {1} = ?", CustomWhereCriteria,
                    PBankTable.GetTableDBName() + "." + PBankTable.GetPartnerKeyDBName());                                                                                         // CustomWhereCriteria + ' AND p_partner_key_n = ?';
                OdbcParameter miParam = new OdbcParameter("", OdbcType.BigInt, 10);
                miParam.Value = (object)(CriteriaRow["BankKey"]);
                InternalParameters.Add(miParam);
            }

            if (((Boolean)(CriteriaRow["WorkerFamOnly"]) == true)
                && (CriteriaRow["PartnerClass"].ToString() == "FAMILY"))
            {
                // A custom subquery seems to only speedy way of doing this!
                CustomWhereCriteria = String.Format(
                    "{0} AND EXISTS (select * FROM PUB.p_partner_gift_destination " +
                    "WHERE PUB.p_partner.p_partner_key_n = PUB.p_partner_gift_destination.p_partner_key_n " +
                    "AND (PUB.p_partner_gift_destination.p_date_expires_d IS NULL OR PUB.p_partner_gift_destination.p_date_effective_d <> PUB.p_partner_gift_destination.p_date_expires_d))",
                    CustomWhereCriteria);
            }

            #region Partner Status

            if ((String)(CriteriaRow["PartnerStatus"]) == "ACTIVE")
            {
                // Searched DB Field: 'p_status_code_c'

                // This will automatically exclude other peoples' PRIVATE Partners!
                CustomWhereCriteria = String.Format("{0} AND PUB.{1}.{2} = ?", CustomWhereCriteria,  // CustomWhereCriteria + ' AND p_status_code_c = ?';
                    PPartnerTable.GetTableDBName(),
                    PPartnerTable.GetStatusCodeDBName());

                OdbcParameter miParam = new OdbcParameter("", OdbcType.VarChar, 16);
                miParam.Value = "ACTIVE";
                InternalParameters.Add(miParam);
            }

            if ((String)(CriteriaRow["PartnerStatus"]) == "PRIVATE")
            {
                // Searched DB Fields: 'p_status_code_c', 'p_user_id_c'
                CustomWhereCriteria = String.Format("{0} AND PUB.{1}.{2} = ? AND PUB.{3}.{4} = ?", CustomWhereCriteria,  // CustomWhereCriteria + ' AND p_status_code_c = ?';
                    PPartnerTable.GetTableDBName(),
                    PPartnerTable.GetStatusCodeDBName(),
                    PPartnerTable.GetTableDBName(),
                    PPartnerTable.GetUserIdDBName());


                OdbcParameter miParam = new OdbcParameter("", OdbcType.VarChar, 16);
                miParam.Value = "PRIVATE";
                InternalParameters.Add(miParam);

                // user must be current user
                miParam = new OdbcParameter("", OdbcType.VarChar, 20);
                miParam.Value = UserInfo.GUserInfo.UserID;
                InternalParameters.Add(miParam);
            }

            if ((String)(CriteriaRow["PartnerStatus"]) == "ALL")
            {
                // Searched DB Fields: 'p_status_code_c', 'p_user_id_c'

                // This must show all partners PLUS the users *own* PRIVATE partners
                CustomWhereCriteria = String.Format("{0} AND ((PUB.{1}.{2} = ? AND PUB.{3}.{4} = ? ) OR PUB.{1}.{2} <> ? )",
                    CustomWhereCriteria,
                    PPartnerTable.GetTableDBName(),
                    PPartnerTable.GetStatusCodeDBName(),
                    PPartnerTable.GetTableDBName(),
                    PPartnerTable.GetUserIdDBName());

                OdbcParameter miParam = new OdbcParameter("", OdbcType.VarChar, 16);
                miParam.Value = "PRIVATE";
                InternalParameters.Add(miParam);

                // user must be current user
                miParam = new OdbcParameter("", OdbcType.VarChar, 20);
                miParam.Value = UserInfo.GUserInfo.UserID;
                InternalParameters.Add(miParam);

                miParam = new OdbcParameter("", OdbcType.VarChar, 16);
                miParam.Value = "PRIVATE";
                InternalParameters.Add(miParam);
            }

            #endregion

//           TLogging.LogAtLevel(7, "CustomWhereCriteria: " + CustomWhereCriteria);

            /* Convert ArrayList to a array of ODBCParameters
             * seem to need to declare a type first
             */
            AParametersArray = ((OdbcParameter[])(InternalParameters.ToArray(typeof(OdbcParameter))));
            InternalParameters = null;             // ensure this is GC'd

            return CustomWhereCriteria;
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
            TLogging.LogAtLevel(7, "TPartnerFindUIConnector.StopSearch: Starting StopQuery thread...");

            StopQueryThread = new Thread(new ThreadStart(FPagedDataSetObject.StopQuery));
            StopQueryThread.Name = "PartnerFindStopQuery" + Guid.NewGuid().ToString();
            StopQueryThread.Start();

            /* It might take some time until the executing query is cancelled by the DB,
             * but we consider it as done since the application can 'forget' about the
             * result of the cancellation process (but beware of executing another query
             * while the other is stopping - this leads to ADO.NET errors that state that
             * a ADO.NET command is still executing!
             */

            TLogging.LogAtLevel(7, "TPartnerFindUIConnector.StopSearch: Query cancelled!");
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
            DataTable FullFindResultDT;
            int AddedPartners = 0;
            bool NewTransaction;

            AVerificationResult = null;
            TLogging.LogAtLevel(8, "TPartnerFind.AddAllFoundPartnersToExtract: requesting Partner data of found Partners");

            // Request all found Partners from FPagedDataSetObject
            FullFindResultDT = FPagedDataSetObject.GetAllData();

            // Create a new table containing only the required data.
            // (Site Key and Location Key must be in columns 2 and 3 respectively for some reason.)

            DataTable PartnerKeysTable = new DataTable();
            PartnerKeysTable.Columns.Add("index", Type.GetType("System.Int32"));
            PartnerKeysTable.Columns.Add("p_partner_key_n", Type.GetType("System.Int64"));
            PartnerKeysTable.Columns.Add("p_site_key_n", Type.GetType("System.Int64"));
            PartnerKeysTable.Columns.Add("p_location_key_i", Type.GetType("System.Int64"));

            int i = 0;

            // false when doing a seatch from bank details tab
            bool LocationInfoProvided = FullFindResultDT.Columns.Contains("p_site_key_n")
                                        && FullFindResultDT.Columns.Contains("p_location_key_i");

            foreach (DataRow Row in FullFindResultDT.Rows)
            {
                DataRow NewRow = PartnerKeysTable.NewRow();
                NewRow[0] = i;
                NewRow[1] = Row["p_partner_key_n"];

                if (LocationInfoProvided)
                {
                    NewRow[2] = Row["p_site_key_n"];
                    NewRow[3] = Row["p_location_key_i"];
                }

                PartnerKeysTable.Rows.Add(NewRow);

                i++;
            }

            PartnerKeysTable.DefaultView.Sort = PPartnerTable.GetPartnerKeyDBName() + " ASC";

            if (FullFindResultDT.Rows.Count > 0)
            {
//              TLogging.LogAtLevel(8, "TPartnerFind.AddAllFoundPartnersToExtract: Add all Partners to the desired Extract");

                /*
                 * Add all Partners to the desired Extract
                 */
                DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    IsolationLevel.Serializable,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                try
                {
                    int keyCount;
                    List <long>ignoredPartnerKeys = null;
                    TExtractsHandling.CreateExtractFromListOfPartnerKeys(
                        AExtractName,
                        AExtractDescription,
                        out AExtractID,
                        PartnerKeysTable,
                        1,
                        LocationInfoProvided,
                        out keyCount,
                        out ignoredPartnerKeys,
                        false);

//                  TLogging.LogAtLevel(8, "TPartnerFind.AddAllFoundPartnersToExtract: Added " + AddedPartners.ToString() + " Partners to the desired Extract!");

                    AddedPartners = TExtractsHandling.GetExtractKeyCount(AExtractID);

                    return AddedPartners;
                }
                catch (Exception Exc)
                {
                    TLogging.Log("An Exception occured while adding all found Partners to an Extract:" + Environment.NewLine + Exc.ToString());

                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }

                    throw;
                }
                finally
                {
                    if (AddedPartners >= 0)
                    {
                        if (NewTransaction)
                        {
                            DBAccess.GDBAccessObj.CommitTransaction();
                            TLogging.LogAtLevel(8, "TPartnerFind.AddAllFoundPartnersToExtract: committed own transaction!");
                        }
                    }
                    else
                    {
                        if (NewTransaction)
                        {
                            DBAccess.GDBAccessObj.RollbackTransaction();
                            TLogging.LogAtLevel(8, "TPartnerFind.AddAllFoundPartnersToExtract: ROLLED BACK own transaction!");
                        }
                    }
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// modify search result: filter each result partner for best address only
        /// </summary>
        /// <returns>DataTable with filtered result</returns>
        public DataTable FilterResultByBestAddress()
        {
            Int64 CurrentPartnerKey = 0;
            Int64 NewPartnerKey = 0;
            DataTable FullFindResultDT = new PartnerFindTDSSearchResultTable();
            DataTable FilteredResultDT = new PartnerFindTDSSearchResultTable();
            DataRow fallbackRow = null;
            TLocationPK bestLocationPK = null;

            string partnerKeyDBname = PartnerFindTDSSearchResultTable.GetPartnerKeyDBName();
            string locationKeyDBName = PartnerFindTDSSearchResultTable.GetLocationKeyDBName();
            string siteKeyDBname = PartnerFindTDSSearchResultTable.GetSiteKeyDBName();

            // Request all found Partners from FPagedDataSetObject
            FullFindResultDT = FPagedDataSetObject.GetAllData();

            // Sort by PartnerKey so we get duplicates together
            FullFindResultDT.DefaultView.Sort = partnerKeyDBname + " ASC";

            foreach (DataRowView rv in FullFindResultDT.DefaultView)
            {
                DataRow row = rv.Row;
                NewPartnerKey = Convert.ToInt64(row[partnerKeyDBname]);

                if (NewPartnerKey != CurrentPartnerKey)
                {
                    // We have moved on to the next partner
                    if (fallbackRow != null)
                    {
                        // Not sure if/how this can happen but we didn't save anything  for the previous partner
                        // We must save something so we save the row we kept as a fallback.  It is not the 'best' address however.
                        FilteredResultDT.ImportRow(fallbackRow);
                    }

                    // Find the best address for the new partner
                    bestLocationPK = ServerCalculations.DetermineBestAddress(NewPartnerKey);
                    CurrentPartnerKey = NewPartnerKey;
                }

                // Is this row the same as the Best Address? If so its the one we import and we no longer need a fallback
                if ((bestLocationPK.LocationKey == Convert.ToInt32(row[locationKeyDBName]))
                    && (bestLocationPK.SiteKey == Convert.ToInt64(row[siteKeyDBname])))
                {
                    FilteredResultDT.ImportRow(row);
                    fallbackRow = null;
                }
            }

            return FilteredResultDT;
        }

        /// <summary>
        /// Checks if a search result contains a given partner
        /// </summary>
        /// <param name="APartnerKey">Partner key of partner</param>
        /// <returns>True if partner is included, false if not.</returns>
        public bool CheckIfResultsContainPartnerKey(long APartnerKey)
        {
            DataTable Table = FPagedDataSetObject.GetAllData();

            DataRow[] FoundRows = Table.Select(PPartnerTable.GetPartnerKeyDBName() + " = " + APartnerKey.ToString());

            if (FoundRows.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
