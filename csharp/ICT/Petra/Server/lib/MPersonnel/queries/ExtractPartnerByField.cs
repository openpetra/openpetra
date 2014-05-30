//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MCommon.queries;
using Ict.Petra.Server.MPartner.Extracts;
using Ict.Petra.Server.MPartner.Partner.Data.Access;

namespace Ict.Petra.Server.MPersonnel.queries
{
    /// <summary>
    /// this report is quite simple, and should be used as an example for more complex reports and extracts
    /// </summary>
    public class QueryPartnerByField : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// calculate an extract from a report: all partners working on a given field
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            // Sql statements will be initialized later on in special treatment
            string SqlStmt = "";

            // create a new object of this class and control extract calculation from base class
            QueryPartnerByField ExtractQuery = new QueryPartnerByField();

            return ExtractQuery.CalculateExtractInternal(AParameters, SqlStmt, AResults);
        }

        /// <summary>
        /// constructor
        /// </summary>
        public QueryPartnerByField() : base()
        {
            // This extract involves processing of several queries
            FSpecialTreatment = true;
        }

        /// <summary>
        /// This method needs to be implemented by extracts that can't follow the default processing with just
        /// one query.
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AExtractId"></param>
        protected override bool RunSpecialTreatment(TParameterList AParameters, TDBTransaction ATransaction, out int AExtractId)
        {
            AExtractId = -1;

            if (AParameters.Get("param_sending_receiving").ToString() == "ReceivingField")
            {
                return ProcessReceivingFields(AParameters, ATransaction, out AExtractId);
            }
            else if (AParameters.Get("param_sending_receiving").ToString() == "SendingField")
            {
                return ProcessSendingFields(AParameters, ATransaction, out AExtractId);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// retrieve parameters from client sent in AParameters and build up AParameterList to run SQL query
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ASqlStmt"></param>
        /// <param name="ASQLParameterList"></param>
        protected override void RetrieveParameters(TParameterList AParameters, ref string ASqlStmt, ref List <OdbcParameter>ASQLParameterList)
        {
            // this is not supposed to be called but needs to be here because of abstract method
        }

        /// <summary>
        /// Run extract in case the user wants to analyze sending fields. Find all persons or families
        /// that have a commitment with sending fields selected.
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AExtractId"></param>
        private bool ProcessSendingFields(TParameterList AParameters, TDBTransaction ATransaction, out int AExtractId)
        {
            /* Approach:
             * Only find persons that have a commitment record.
             * When interested in families only, find those families
             * for which a member matches the specified criteria. */

            bool ReturnValue = false;

            // for sending fields only commitments are taken into account
            ReturnValue = ProcessCommitments(false, AParameters, ATransaction, out AExtractId);

            // if result was true then commit transaction, otherwise rollback
            TExtractsHandling.FinishExtractFromListOfPartnerKeys(ReturnValue);

            return ReturnValue;
        }

        /// <summary>
        /// Run extract in case the user wants to analyze receiving fields.
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AExtractId"></param>
        private bool ProcessReceivingFields(TParameterList AParameters, TDBTransaction ATransaction, out int AExtractId)
        {
            /*Approach:
             * In case of a specified "Period" only find persons
             * that have a commitment record.
             * In case of "Now" or "Ever" also find partners
             * (persons & families) without such a commitment
             * record, that match the specified criteria.
             * In case of "Now" only find partners with a "Worker" type.
             * (This check is dropped in case of "Ever")
             * When interested in families only, also find families
             * for which a member matches the specified criteria.*/

            bool ReturnValue = false;

            // for receiving fields first look at commitments
            ReturnValue = ProcessCommitments(true, AParameters, ATransaction, out AExtractId);

            if (ReturnValue == false)
            {
                // if result was false then rollback transaction
                TExtractsHandling.FinishExtractFromListOfPartnerKeys(false);
                return ReturnValue;
            }

            // if only commitments need to be considered then no need to continue here
            if (AParameters.Get("param_commitments_and_worker_field").IsZeroOrNull()
                || (AParameters.Get("param_commitments_and_worker_field").ToString() == "CommitmentsOnly"))
            {
                return ReturnValue;
            }

            bool AddressFilterAdded;
            string SqlStmtWorkerFieldOriginal = TDataBase.ReadSqlFile("Partner.Queries.ExtractPartnerByField.WorkerField.sql");
            string SqlStmt;
            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();
            string TypeCodeParameter;

            // If date range was specified then only look at staff data. Otherwise look for persons and families seperately.
            if (AParameters.Get("param_field_dates").ToString() == "DateRange")
            {
                return ReturnValue;
            }

            // prepare parameter field for partner type code.
            if (AParameters.Get("param_field_dates").ToString() == "DateEver")
            {
                TypeCodeParameter = "";
            }
            else
            {
                TypeCodeParameter = "OMER%";
            }

            // prepare list of selected fields
            List <String>param_fields = new List <String>();

            foreach (TVariant choice in AParameters.Get("param_fields").ToComposite())
            {
                param_fields.Add(choice.ToString());
            }

            if (param_fields.Count == 0)
            {
                throw new NoNullAllowedException("At least one option must be checked.");
            }

            // now add parameters to sql parameter list
            SqlParameterList.Add(TDbListParameterValue.OdbcListParameterValue("fields", OdbcType.BigInt, param_fields));

            SqlParameterList.Add(new OdbcParameter("param_active", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_active").ToBool()
                });
            SqlParameterList.Add(new OdbcParameter("param_exclude_no_solicitations", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_exclude_no_solicitations").ToBool()
                });

            // ----------------------------------------------------------------------------------------
            // now start retrieving either families or persons whose worker field is set to given value
            // ----------------------------------------------------------------------------------------

            SqlStmt = SqlStmtWorkerFieldOriginal;
            SqlStmt = SqlStmt.Replace("##person_or_family_table##", ", pub_p_person");
            SqlStmt = SqlStmt.Replace("##person_or_family_table_name##", "pub_p_person");
            SqlStmt = SqlStmt.Replace("##exclude_familiy_members_existing_in_extract##", "");
            SqlStmt = SqlStmt.Replace("##worker_type##", TypeCodeParameter);

            if (AParameters.Get("param_families_only").ToBool())
            {
                /* In case that only family records are wanted a join via family key of a person is needed
                 * to find families of persons. */
                SqlStmt = SqlStmt.Replace("##join_for_person_or_family##",
                    " AND pub_p_partner.p_partner_key_n = pub_p_person.p_family_key_n");
            }
            else
            {
                // in this case there will be person records in the extract
                SqlStmt = SqlStmt.Replace("##join_for_person_or_family##",
                    " AND pub_p_partner.p_partner_key_n = pub_p_person.p_partner_key_n");
            }

            // add address filter information to sql statement and parameter list
            AddressFilterAdded = AddAddressFilter(AParameters, ref SqlStmt, ref SqlParameterList);

            // now run the database query
            TLogging.Log("getting the data from the database", TLoggingType.ToStatusBar);
            DataTable partnerkeys = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "partners", ATransaction,
                SqlParameterList.ToArray());

            // filter data by postcode (if applicable)
            ExtractQueryBase.PostcodeFilter(ref partnerkeys, ref AddressFilterAdded, AParameters, ATransaction);

            // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
            // TODO: we might need to add this functionality to TExtractsHandling.CreateExtractFromListOfPartnerKeys as well???
            if (AParameters.Get("CancelReportCalculation").ToBool() == true)
            {
                return false;
            }

            TLogging.Log("preparing the extract", TLoggingType.ToStatusBar);

            // create an extract with the given name in the parameters
            TExtractsHandling.ExtendExtractFromListOfPartnerKeys(
                AExtractId,
                partnerkeys,
                0,
                AddressFilterAdded,
                false,
                false);

            // ----------------------------------------------------------------------------------------
            // Now start retrieving families whose worker field is set to given value and that are not
            // already contained in the created extract.
            // ----------------------------------------------------------------------------------------

            SqlStmt = SqlStmtWorkerFieldOriginal;

            // need to rebuild parameter list as statement is also loaded again and filled
            SqlParameterList.Clear();
            SqlParameterList.Add(TDbListParameterValue.OdbcListParameterValue("fields", OdbcType.BigInt, param_fields));

            SqlParameterList.Add(new OdbcParameter("param_active", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_active").ToBool()
                });
            SqlParameterList.Add(new OdbcParameter("param_exclude_no_solicitations", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_exclude_no_solicitations").ToBool()
                });


            SqlStmt = SqlStmt.Replace("##person_or_family_table##", ", pub_p_family");
            SqlStmt = SqlStmt.Replace("##person_or_family_table_name##", "pub_p_family");
            SqlStmt = SqlStmt.Replace("##worker_type##", TypeCodeParameter);
            SqlStmt = SqlStmt.Replace("##join_for_person_or_family##",
                " AND pub_p_partner.p_partner_key_n = pub_p_family.p_partner_key_n");

            SqlStmt = SqlStmt.Replace("##exclude_familiy_members_existing_in_extract##",
                "AND NOT EXISTS (SELECT pub_p_family.p_partner_key_n " +
                " FROM pub_p_family, pub_p_person, pub_m_extract " +
                " WHERE pub_p_person.p_family_key_n = pub_p_family.p_partner_key_n " +
                " AND pub_m_extract.m_extract_id_i = " + AExtractId.ToString() +
                " AND pub_m_extract.p_partner_key_n = pub_p_person.p_partner_key_n)");

            // add address filter information to sql statement and parameter list
            AddressFilterAdded = AddAddressFilter(AParameters, ref SqlStmt, ref SqlParameterList);

            // now run the database query
            TLogging.Log("getting the data from the database", TLoggingType.ToStatusBar);
            partnerkeys.Clear();
            partnerkeys = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "partners", ATransaction,
                SqlParameterList.ToArray());

            // filter data by postcode (if applicable)
            ExtractQueryBase.PostcodeFilter(ref partnerkeys, ref AddressFilterAdded, AParameters, ATransaction);

            // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
            // TODO: we might need to add this functionality to TExtractsHandling.CreateExtractFromListOfPartnerKeys as well???
            if (AParameters.Get("CancelReportCalculation").ToBool() == true)
            {
                return false;
            }

            TLogging.Log("preparing the extract", TLoggingType.ToStatusBar);

            // create an extract with the given name in the parameters
            TExtractsHandling.ExtendExtractFromListOfPartnerKeys(
                AExtractId,
                partnerkeys,
                0,
                AddressFilterAdded,
                false,
                false);

            ReturnValue = true;

            // if result was true then commit transaction, otherwise rollback
            TExtractsHandling.FinishExtractFromListOfPartnerKeys(ReturnValue);

            return ReturnValue;
        }

        /// <summary>
        /// Add persons or families to extract that have commitments to specified receiving/sending fields
        /// </summary>
        /// <param name="AProcessReceivingFields"></param>
        /// <param name="AParameters"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AExtractId"></param>
        private bool ProcessCommitments(bool AProcessReceivingFields, TParameterList AParameters,
            TDBTransaction ATransaction, out int AExtractId)
        {
            bool ReturnValue = false;
            bool AddressFilterAdded;
            string SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractPartnerByField.Commitment.sql");

            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();

            // need to set initial value here in case method needs to return before value is set
            AExtractId = -1;

            // prepare list of selected fields
            List <String>param_fields = new List <String>();

            foreach (TVariant choice in AParameters.Get("param_fields").ToComposite())
            {
                param_fields.Add(choice.ToString());
            }

            if (param_fields.Count == 0)
            {
                throw new NoNullAllowedException("At least one option must be checked.");
            }

            // now add parameters to sql parameter list
            SqlParameterList.Add(TDbListParameterValue.OdbcListParameterValue("fields", OdbcType.BigInt, param_fields));

            SqlParameterList.Add(new OdbcParameter("param_from_date_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_from_date").IsZeroOrNull()
                });
            SqlParameterList.Add(new OdbcParameter("param_from_date", OdbcType.Date)
                {
                    Value = AParameters.Get("param_from_date").ToDate()
                });
            SqlParameterList.Add(new OdbcParameter("param_until_date_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_until_date").IsZeroOrNull()
                });
            SqlParameterList.Add(new OdbcParameter("param_until_date", OdbcType.Date)
                {
                    Value = AParameters.Get("param_until_date").ToDate()
                });
            SqlParameterList.Add(new OdbcParameter("param_active", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_active").ToBool()
                });
            SqlParameterList.Add(new OdbcParameter("param_exclude_no_solicitations", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_exclude_no_solicitations").ToBool()
                });

            if (AProcessReceivingFields)
            {
                // for receiving fields target field table field needs to be used
                SqlStmt = SqlStmt.Replace("##sending_or_receiving_field##", "pm_receiving_field_n");
            }
            else
            {
                // for sending fields home office table field needs to be used
                SqlStmt = SqlStmt.Replace("##sending_or_receiving_field##", "pm_home_office_n");
            }

            if (AParameters.Get("param_families_only").ToBool())
            {
                /* In case that only family records are wanted a join via family key of a person is needed
                 * to find families of persons. */
                SqlStmt = SqlStmt.Replace("##person_table##", ", pub_p_person");
                SqlStmt = SqlStmt.Replace("##join_for_person_or_family##",
                    " AND pub_p_person.p_partner_key_n = pub_pm_staff_data.p_partner_key_n" +
                    " AND pub_p_partner.p_partner_key_n = pub_p_person.p_family_key_n ");
            }
            else
            {
                // in this case there will be person records in the extract
                SqlStmt = SqlStmt.Replace("##person_table##", "");
                SqlStmt = SqlStmt.Replace("##join_for_person_or_family##",
                    " AND pub_p_partner.p_partner_key_n = pub_pm_staff_data.p_partner_key_n");
            }

            // add address filter information to sql statement and parameter list
            AddressFilterAdded = AddAddressFilter(AParameters, ref SqlStmt, ref SqlParameterList);

            // now run the database query
            TLogging.Log("getting the data from the database", TLoggingType.ToStatusBar);
            DataTable partnerkeys = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "partners", ATransaction,
                SqlParameterList.ToArray());

            // filter data by postcode (if applicable)
            ExtractQueryBase.PostcodeFilter(ref partnerkeys, ref AddressFilterAdded, AParameters, ATransaction);

            // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
            // TODO: we might need to add this functionality to TExtractsHandling.CreateExtractFromListOfPartnerKeys as well???
            if (AParameters.Get("CancelReportCalculation").ToBool() == true)
            {
                return false;
            }

            TLogging.Log("preparing the extract", TLoggingType.ToStatusBar);

            // create an extract with the given name in the parameters
            ReturnValue = TExtractsHandling.CreateExtractFromListOfPartnerKeys(
                AParameters.Get("param_extract_name").ToString(),
                AParameters.Get("param_extract_description").ToString(),
                out AExtractId,
                partnerkeys,
                0,
                AddressFilterAdded,
                false);

            return ReturnValue;
        }
    }
}