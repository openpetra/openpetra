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
            TValidationControlsDict ValidationControlsDict = new TValidationControlsDict();
            bool AllDataValidationsOK = true;

            AVerificationResult = new TVerificationResultCollection();

            // TODO: calculate debit and credit sums for journal and batch?

            if (AInspectDS.Tables.Contains(PmStaffDataTable.GetTableName()))
            {
                if (AInspectDS.PmStaffData.Rows.Count > 0)
                {
                    ValidatePersonnelStaff(ValidationControlsDict, ref AVerificationResult, AInspectDS.PmStaffData);
                    ValidatePersonnelStaffManual(ValidationControlsDict, ref AVerificationResult, AInspectDS.PmStaffData);

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

        #region Data Validation

        static partial void ValidatePersonnelStaff(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidatePersonnelStaffManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        #endregion Data Validation
    }
}