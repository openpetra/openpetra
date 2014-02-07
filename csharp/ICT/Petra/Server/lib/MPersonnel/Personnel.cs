//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, matthiash
//
// Copyright 2004-2010 by OM International
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
using System.Data.Odbc;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPersonnel.Units.Data.Access;

namespace Ict.Petra.Server.MPersonnel.WebConnectors
{
    /// <summary>
    /// Description of Personnel.
    /// </summary>
    public partial class TPersonnelWebConnector
    {
        /// <summary>
        /// this will store PersonnelTDS
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("PERSONNEL")]
        public static TSubmitChangesResult SavePersonnelTDS(ref PersonnelTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            bool AllDataValidationsOK = true;

            AVerificationResult = new TVerificationResultCollection();

            // TODO: calculate debit and credit sums for journal and batch?

            if (AInspectDS.Tables.Contains(PmStaffDataTable.GetTableName()))
            {
                if (AInspectDS.PmStaffData.Rows.Count > 0)
                {
                    ValidatePersonnelStaff(ref AVerificationResult, AInspectDS.PmStaffData);
                    ValidatePersonnelStaffManual(ref AVerificationResult, AInspectDS.PmStaffData);

                    if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                    {
                        AllDataValidationsOK = false;
                    }
                }
            }

            if (AllDataValidationsOK)
            {
                PersonnelTDSAccess.SubmitChanges(AInspectDS);

                SubmissionResult = TSubmitChangesResult.scrOK;
            }
            else if (AVerificationResult.Count > 0)
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
            }

            return SubmissionResult;
        }

        /// <summary>
        /// loads the Staff Data Table ("commitments") for a single person
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <returns>PersonnelTDS</returns>
        [RequireModulePermission("PERSONNEL")]
        public static PersonnelTDS LoadPersonellStaffData(Int64 APartnerKey)
        {
            PersonnelTDS MainDS = new PersonnelTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            PPartnerAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);

            PmCommitmentStatusAccess.LoadAll(MainDS, Transaction);
            PmStaffDataAccess.LoadViaPPerson(MainDS, APartnerKey, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// retrieves information if person has a current commitment (staff data) record
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <returns>true if person has current commitment (staff data) record(s)</returns>
        [RequireModulePermission("PERSONNEL")]
        public static bool HasCurrentCommitmentRecord(Int64 APartnerKey)
        {
            PmStaffDataTable StaffDataDT;
            bool Result = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            StaffDataDT = PmStaffDataAccess.LoadByPrimaryKey(0, APartnerKey, Transaction);

            foreach (PmStaffDataRow row in StaffDataDT.Rows)
            {
                if ((row.IsEndOfCommitmentNull()
                     || (row.EndOfCommitment >= DateTime.Today))
                    && (row.IsStartOfCommitmentNull()
                        || (row.StartOfCommitment <= DateTime.Today)))
                {
                    Result = true;
                }
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            return Result;
        }

        /// <summary>
        /// Return UmJob.JobKey for existing job record or create a new one if not existing
        /// </summary>
        /// <param name="AUnitKey"></param>
        /// <param name="APositionName"></param>
        /// <param name="APositionScope"></param>
        /// <returns></returns>
        [RequireModulePermission("PERSONNEL")]
        public static int GetOrCreateUmJobKey(Int64 AUnitKey, string APositionName, string APositionScope)
        {
            int JobKey;
            bool NewTransaction;

            UmJobTable JobTableTemp = new UmJobTable();
            UmJobRow TemplateRow = (UmJobRow)JobTableTemp.NewRow();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            TemplateRow.UnitKey = AUnitKey;
            TemplateRow.PositionName = APositionName;
            TemplateRow.PositionScope = APositionScope;
            JobTableTemp = UmJobAccess.LoadUsingTemplate(TemplateRow, Transaction);

            // if no corresponding job record found then we need to create a new job key
            if (JobTableTemp.Count == 0)
            {
                JobKey = Convert.ToInt32(MCommon.WebConnectors.TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_job));

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            else
            {
                JobKey = ((UmJobRow)JobTableTemp.Rows[0]).JobKey;

                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return JobKey;
        }

        /// <summary>
        /// populate ApplicationTDS dataset with shortterm applications for a given event
        /// </summary>
        /// <param name="AMainDS">Dataset to be populated</param>
        /// <param name="AOutreachCode">match string for event's outreach code</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean LoadShortTermApplications(ref ApplicationTDS AMainDS, string AOutreachCode)
        {
            Boolean NewTransaction;

            string QueryShortTermApplication = "";

            List <OdbcParameter>Parameters = new List <OdbcParameter>();
            OdbcParameter Parameter = new OdbcParameter("eventcode", OdbcType.VarChar, PmShortTermApplicationTable.GetConfirmedOptionCodeLength());

            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                QueryShortTermApplication =
                    "SELECT PUB_pm_short_term_application.*, PUB_pm_general_application.*, PUB_p_partner.p_partner_short_name_c " +
                    "FROM PUB_pm_short_term_application, PUB_pm_general_application, PUB_p_partner ";

                if (AOutreachCode.Length == 0)
                {
                    // load all appicants with no event
                    QueryShortTermApplication += "WHERE PUB_pm_short_term_application.pm_confirmed_option_code_c IS NULL ";
                }
                else if (AOutreachCode.Length > 0)
                {
                    Parameter.Value = AOutreachCode.Substring(0, 5);
                    Parameters.Add(Parameter);

                    QueryShortTermApplication += "WHERE SUBSTRING(PUB_pm_short_term_application.pm_confirmed_option_code_c, 1, 5) = ? ";
                }

                QueryShortTermApplication += "AND PUB_pm_general_application.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n " +
                                             "AND PUB_pm_general_application.pm_application_key_i = PUB_pm_short_term_application.pm_application_key_i "
                                             +
                                             "AND PUB_pm_general_application.pm_registration_office_n = PUB_pm_short_term_application.pm_registration_office_n "
                                             +
                                             "AND PUB_p_partner.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n";

                DBAccess.GDBAccessObj.Select(AMainDS,
                    QueryShortTermApplication,
                    AMainDS.PmShortTermApplication.TableName, ReadTransaction, Parameters.ToArray());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.LoadShortTermApplications: commit own transaction.");
                }
            }

            return true;
        }

        /// <summary>
        /// populate ApplicationTDS dataset with longterm applications
        /// </summary>
        /// <param name="AMainDS">Dataset to be populated</param>
        /// <param name="ATargetFieldKey">match key for application's Target Field</param>
        /// <param name="APlacementPersonKey">match key for application's Placement Person</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean LoadLongTermApplications(ref ApplicationTDS AMainDS, long ATargetFieldKey, long APlacementPersonKey)
        {
            Boolean NewTransaction;

            string QueryLongTermApplication = "";

            List <OdbcParameter>Parameters = new List <OdbcParameter>();
            OdbcParameter Parameter1 = new OdbcParameter(
                "targetfieldkey", OdbcType.BigInt, PmGeneralApplicationTable.GetGenAppPossSrvUnitKeyLength());
            OdbcParameter Parameter2 = new OdbcParameter(
                "Placementpersonkey", OdbcType.BigInt, PmGeneralApplicationTable.GetPlacementPartnerKeyLength());

            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                QueryLongTermApplication =
                    "SELECT PUB_pm_general_application.*, PUB_pm_year_program_application.*, PUB_p_partner.p_partner_short_name_c " +
                    "FROM PUB_pm_general_application, PUB_pm_year_program_application, PUB_p_partner WHERE ";

                if (ATargetFieldKey != 0)
                {
                    Parameter1.Value = ATargetFieldKey;
                    Parameters.Add(Parameter1);

                    // load all appicants with given Target Field
                    QueryLongTermApplication += "PUB_pm_general_application.pm_gen_app_poss_srv_unit_key_n = ? AND ";
                }

                if (APlacementPersonKey != 0)
                {
                    Parameter2.Value = APlacementPersonKey;
                    Parameters.Add(Parameter2);

                    // load all appicants with given Placement Person
                    QueryLongTermApplication += "PUB_pm_general_application.pm_placement_partner_key_n = ? AND ";
                }

                QueryLongTermApplication += "PUB_pm_year_program_application.p_partner_key_n = PUB_pm_general_application.p_partner_key_n " +
                                            "AND PUB_pm_year_program_application.pm_application_key_i = PUB_pm_general_application.pm_application_key_i "
                                            +
                                            "AND PUB_pm_year_program_application.pm_registration_office_n = PUB_pm_general_application.pm_registration_office_n "
                                            +
                                            "AND PUB_p_partner.p_partner_key_n = PUB_pm_general_application.p_partner_key_n";

                DBAccess.GDBAccessObj.Select(AMainDS,
                    QueryLongTermApplication,
                    AMainDS.PmGeneralApplication.TableName, ReadTransaction, Parameters.ToArray());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.LoadLongTermApplications: commit own transaction.");
                }
            }

            return true;
        }

        #region Data Validation

        static partial void ValidatePersonnelStaff(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidatePersonnelStaffManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        #endregion Data Validation
    }
}