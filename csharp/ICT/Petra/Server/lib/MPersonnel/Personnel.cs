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
using Ict.Petra.Server.MCommon.WebConnectors;
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
            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                delegate
                {
                    PPartnerAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);

                    PmCommitmentStatusAccess.LoadAll(MainDS, Transaction);
                    PmStaffDataAccess.LoadViaPPerson(MainDS, APartnerKey, Transaction);
                });

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

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                delegate
                {
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
                });

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
            int JobKey = 0;

            UmJobTable JobTableTemp = new UmJobTable();
            UmJobRow TemplateRow = (UmJobRow)JobTableTemp.NewRow();

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum,
                ref Transaction, ref SubmissionOK,
                delegate
                {
                    TemplateRow.UnitKey = AUnitKey;
                    TemplateRow.PositionName = APositionName;
                    TemplateRow.PositionScope = APositionScope;
                    JobTableTemp = UmJobAccess.LoadUsingTemplate(TemplateRow, Transaction);

                    // if no corresponding job record found then we need to create a new job key
                    if (JobTableTemp.Count == 0)
                    {
                        JobKey = Convert.ToInt32(MCommon.WebConnectors.TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_job));

                        SubmissionOK = true;
                    }
                    else
                    {
                        JobKey = ((UmJobRow)JobTableTemp.Rows[0]).JobKey;
                    }
                });

            return JobKey;
        }

        /// <summary>
        /// populate ApplicationTDS dataset with shortterm applications for a given event
        /// </summary>
        /// <param name="AMainDS">Dataset to be populated</param>
        /// <param name="AOutreachCode">match string for event's outreach code</param>
        /// <returns></returns>
        [RequireModulePermission("PERSONNEL")]
        public static Boolean LoadShortTermApplications(ref ApplicationTDS AMainDS, string AOutreachCode)
        {
            string QueryShortTermApplication = "";
            ApplicationTDS MainDS = new ApplicationTDS();
            String ShortTermAppTableName = AMainDS.PmShortTermApplication.TableName;

            List <OdbcParameter>Parameters = new List <OdbcParameter>();
            OdbcParameter Parameter = new OdbcParameter("eventcode", OdbcType.VarChar, PmShortTermApplicationTable.GetConfirmedOptionCodeLength());

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
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
                        Parameters.Add(
                            Parameter);

                        QueryShortTermApplication += "WHERE SUBSTRING(PUB_pm_short_term_application.pm_confirmed_option_code_c, 1, 5) = ? ";
                    }

                    QueryShortTermApplication += "AND PUB_pm_general_application.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n " +
                                                 "AND PUB_pm_general_application.pm_application_key_i = PUB_pm_short_term_application.pm_application_key_i "
                                                 +
                                                 "AND PUB_pm_general_application.pm_registration_office_n = PUB_pm_short_term_application.pm_registration_office_n "
                                                 +
                                                 "AND PUB_p_partner.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n";

                    DBAccess.GDBAccessObj.Select(MainDS,
                        QueryShortTermApplication,
                        ShortTermAppTableName, Transaction, Parameters.ToArray());
                });

            AMainDS.Merge(MainDS);

            return true;
        }

        /// <summary>
        /// populate ApplicationTDS dataset with longterm applications
        /// </summary>
        /// <param name="AMainDS">Dataset to be populated</param>
        /// <param name="ATargetFieldKey">match key for application's Target Field</param>
        /// <param name="APlacementPersonKey">match key for application's Placement Person</param>
        /// <returns></returns>
        [RequireModulePermission("PERSONNEL")]
        public static Boolean LoadLongTermApplications(ref ApplicationTDS AMainDS, long ATargetFieldKey, long APlacementPersonKey)
        {
            ApplicationTDS MainDS = new ApplicationTDS();
            String GenAppTableName = AMainDS.PmGeneralApplication.TableName;
            string QueryLongTermApplication = "";

            List <OdbcParameter>Parameters = new List <OdbcParameter>();
            OdbcParameter Parameter1 = new OdbcParameter(
                "targetfieldkey", OdbcType.BigInt, PmGeneralApplicationTable.GetGenAppPossSrvUnitKeyLength());
            OdbcParameter Parameter2 = new OdbcParameter(
                "Placementpersonkey", OdbcType.BigInt, PmGeneralApplicationTable.GetPlacementPartnerKeyLength());

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
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
                        Parameters.Add(
                            Parameter2);

                        // load all appicants with given Placement Person
                        QueryLongTermApplication += "PUB_pm_general_application.pm_placement_partner_key_n = ? AND ";
                    }

                    QueryLongTermApplication += "PUB_pm_year_program_application.p_partner_key_n = PUB_pm_general_application.p_partner_key_n " +
                                                "AND PUB_pm_year_program_application.pm_application_key_i = PUB_pm_general_application.pm_application_key_i "
                                                +
                                                "AND PUB_pm_year_program_application.pm_registration_office_n = PUB_pm_general_application.pm_registration_office_n "
                                                +
                                                "AND PUB_p_partner.p_partner_key_n = PUB_pm_general_application.p_partner_key_n";

                    DBAccess.GDBAccessObj.Select(MainDS,
                        QueryLongTermApplication,
                        GenAppTableName, Transaction, Parameters.ToArray());
                });

            AMainDS.Merge(MainDS);

            return true;
        }

        /// <summary>
        /// populate ApplicationTDS dataset with accepted applications that match the given criteria
        /// </summary>
        /// <param name="AMainDS">Dataset to be populated</param>
        /// <param name="AConvertTo">True if converting applications to past experiences.
        /// False if removing applications from past experiences.</param>
        /// <param name="AOutreachCode">Match string for event's outreach code (optional).</param>
        /// <param name="AAllOutreaches">True if finding all outreaches relating to an event.</param>
        /// <param name="AAllEvents">True if finding all event.</param>
        /// <param name="AYear">Year of events (optional).</param>
        /// <returns></returns>
        [RequireModulePermission("PERSONNEL")]
        public static Boolean LoadApplicationsForConverting(ref ApplicationTDS AMainDS, bool AConvertTo,
            string AOutreachCode, bool AAllOutreaches, bool AAllEvents, string AYear)
        {
            string Query = string.Empty;
            ApplicationTDS MainDS = new ApplicationTDS();
            String ShortTermAppTableName = AMainDS.PmShortTermApplication.TableName;

            List <OdbcParameter>Parameters = new List <OdbcParameter>();

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    Query =
                        "SELECT DISTINCT PUB_pm_short_term_application.*, PUB_pm_general_application.*, PUB_p_partner.p_partner_short_name_c "
                        +
                        "FROM PUB_pm_short_term_application, PUB_pm_general_application, PUB_p_partner, p_unit " +
                        "WHERE ";

                    // if we are looking for a specific event
                    if (!AAllEvents)
                    {
                        if (AOutreachCode.Length == 0)
                        {
                            // load all appicants with no event
                            Query += "PUB_pm_short_term_application.pm_confirmed_option_code_c IS NULL " +
                                     "AND ";
                        }
                        else if (AOutreachCode.Length > 0)
                        {
                            // get all outreaches relating to an event
                            if (AAllOutreaches)
                            {
                                OdbcParameter Parameter = new OdbcParameter("eventcode", OdbcType.VarChar, 5);
                                Parameter.Value = AOutreachCode.Substring(0, 5);
                                Parameters.Add(Parameter);

                                Query += "SUBSTRING(PUB_pm_short_term_application.pm_confirmed_option_code_c, 1, 5) = ? " +
                                         "AND ";
                            }
                            else
                            {
                                OdbcParameter Parameter =
                                    new OdbcParameter("eventcode", OdbcType.VarChar, PmShortTermApplicationTable.GetConfirmedOptionCodeLength());
                                Parameter.Value = AOutreachCode;
                                Parameters.Add(Parameter);

                                Query += "PUB_pm_short_term_application.pm_confirmed_option_code_c = ? " +
                                         "AND ";
                            }
                        }
                    }

                    // if year criteria has been supplied
                    if (!string.IsNullOrEmpty(AYear))
                    {
                        OdbcParameter Parameter = new OdbcParameter("year", OdbcType.VarChar, 2);
                        Parameter.Value = AYear.ToString().Substring(2, 2);
                        Parameters.Add(Parameter);

                        Query += "SUBSTRING(PUB_pm_short_term_application.pm_confirmed_option_code_c, 3, 2) = ? " +
                                 "AND ";
                    }

                    Query += "PUB_pm_general_application.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n " +
                             "AND PUB_pm_general_application.pm_application_key_i = PUB_pm_short_term_application.pm_application_key_i " +
                             "AND PUB_pm_general_application.pm_registration_office_n = PUB_pm_short_term_application.pm_registration_office_n " +
                             "AND PUB_pm_general_application.pm_gen_application_status_c = 'A' " +
                             "AND PUB_p_partner.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n " +
                             "AND p_unit.p_outreach_code_c = PUB_pm_short_term_application.pm_confirmed_option_code_c " +
                             "AND ";

                    if (AConvertTo)
                    {
                        // converting to past experience so need to make sure past experience does not already exist
                        Query += "NOT ";
                    }

                    Query += "EXISTS (SELECT * FROM pm_past_experience " +
                             "WHERE pm_past_experience.p_partner_key_n = pm_short_term_application.p_partner_key_n " +
                             "AND pm_past_experience.pm_prev_location_c = PUB_pm_short_term_application.pm_confirmed_option_code_c)";

                    DBAccess.GDBAccessObj.Select(MainDS, Query, ShortTermAppTableName, Transaction, Parameters.ToArray());
                });

            AMainDS.Merge(MainDS);

            return true;
        }

        /// <summary>
        /// Convert all event applications in dataset to past experience records.
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <returns></returns>
        [RequireModulePermission("PERSONNEL")]
        public static Boolean ConvertApplicationsToPreviousExperience(ApplicationTDS AMainDS)
        {
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref Transaction, ref SubmissionOK,
                delegate
                {
                    PmPastExperienceTable PastExperienceTable = new PmPastExperienceTable();

                    foreach (PmShortTermApplicationRow Row in AMainDS.PmShortTermApplication.Rows)
                    {
                        // create the new past experience record
                        PmPastExperienceRow PastExperienceRow = PastExperienceTable.NewRowTyped(true);
                        PastExperienceRow.Key = Convert.ToInt64(TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_past_experience));
                        PastExperienceRow.PartnerKey = Row.PartnerKey;
                        PastExperienceRow.PrevLocation = Row.ConfirmedOptionCode;
                        PastExperienceRow.StartDate = Row.Arrival;
                        PastExperienceRow.EndDate = Row.Departure;
                        PastExperienceRow.PrevWorkHere = true;
                        PastExperienceRow.PrevWork = true;
                        PastExperienceRow.PastExpComments = "Created from Event Application";
                        PastExperienceRow.OtherOrganisation = "";
                        PastExperienceRow.PrevRole = "";
                        PastExperienceTable.Rows.Add(PastExperienceRow);
                    }

                    PmPastExperienceAccess.SubmitChanges(PastExperienceTable, Transaction);

                    SubmissionOK = true;
                });

            return SubmissionOK;
        }

        /// <summary>
        /// Remove all event applications in dataset from past experience records.
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <returns></returns>
        [RequireModulePermission("PERSONNEL")]
        public static Boolean RemoveApplicationsFromPreviousExperience(ApplicationTDS AMainDS)
        {
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref Transaction, ref SubmissionOK,
                delegate
                {
                    foreach (PmShortTermApplicationRow Row in AMainDS.PmShortTermApplication.Rows)
                    {
                        PmPastExperienceTable PastExperienceTable = new PmPastExperienceTable();
                        PmPastExperienceRow TempRow = PastExperienceTable.NewRowTyped(false);
                        TempRow.PartnerKey = Row.PartnerKey;
                        TempRow.PrevLocation = Row.ConfirmedOptionCode;
                        PastExperienceTable = PmPastExperienceAccess.LoadUsingTemplate(TempRow, Transaction);

                        if ((PastExperienceTable != null) && (PastExperienceTable.Rows.Count > 0))
                        {
                            PastExperienceTable.Rows[0].Delete();
                            PmPastExperienceAccess.SubmitChanges(PastExperienceTable, Transaction);
                        }
                    }

                    SubmissionOK = true;
                });

            return SubmissionOK;
        }

        /// <summary>
        /// Gets the range of years for which events exist with accepted applications.
        /// </summary>
        /// <param name="AMinYear">First year. Will be 0 if there are no events</param>
        /// <param name="AMaxYear">Last year. Will be -1 if there are no events</param>
        [RequireModulePermission("PERSONNEL")]
        public static void GetRangeOfYearsWithEvents(out int AMinYear, out int AMaxYear)
        {
            string Query = "SELECT MIN(p_partner_location.p_date_effective_d) AS Min, MAX(p_partner_location.p_date_good_until_d) AS Max " +
                           "FROM p_unit, p_partner_location, pm_general_application, pm_short_term_application " +
                           "WHERE pm_general_application.p_partner_key_n = pm_short_term_application.p_partner_key_n " +
                           "AND pm_general_application.pm_application_key_i = pm_short_term_application.pm_application_key_i " +
                           "AND pm_general_application.pm_registration_office_n = pm_short_term_application.pm_registration_office_n " +
                           "AND pm_general_application.pm_gen_application_status_c = 'A' " +
                           "AND p_unit.p_outreach_code_c = pm_short_term_application.pm_confirmed_option_code_c " +
                           "AND p_unit.p_partner_key_n = p_partner_location.p_partner_key_n";

            TDBTransaction Transaction = null;
            DataTable MinAndMax = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    MinAndMax = DBAccess.GDBAccessObj.SelectDT(Query, "MinAndMax", Transaction);
                });

            AMinYear = 0;
            AMaxYear = -1;

            if (MinAndMax.Rows.Count == 1)
            {
                // Even if we get a row returned one or both of min and max can be DbNull
                object dtMin = MinAndMax.Rows[0]["Min"];
                object dtMax = MinAndMax.Rows[0]["Max"];

                if ((dtMin != DBNull.Value) && (dtMax != DBNull.Value))
                {
                    AMinYear = Convert.ToDateTime(dtMin).Year;
                    AMaxYear = Convert.ToDateTime(dtMax).Year;
                }
            }
        }

        #region Data Validation

        static partial void ValidatePersonnelStaff(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidatePersonnelStaffManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        #endregion Data Validation
    }
}